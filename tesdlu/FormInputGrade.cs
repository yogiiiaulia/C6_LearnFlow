using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormInputGrade : Form
    {
        private SqlConnection con;
        private int instructorId;
        private int currentCourseId;
        
        private BindingSource bsStudents = new BindingSource();

        public FormInputGrade(int instructorId)
        {
            InitializeComponent();
            this.instructorId = instructorId;
            this.con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");

            this.Load += FormInputGrade_Load;
            this.cmbCourse.SelectedIndexChanged += CmbCourse_SelectedIndexChanged;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnRefresh.Click += BtnRefresh_Click;
            this.btnSave.Click += BtnSave_Click;
            this.dgvStudents.EditingControlShowing += DgvStudents_EditingControlShowing;
            this.txtSearchStudent.GotFocus += TxtSearchStudent_GotFocus;
            this.txtSearchStudent.LostFocus += TxtSearchStudent_LostFocus;
        }

        private void FormInputGrade_Load(object sender, EventArgs e)
        {
            this.txtSearchStudent.Text = "Search Student";
            this.txtSearchStudent.ForeColor = System.Drawing.Color.Gray;
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                this.con.Open();
                // PERBAIKAN: Menggunakan VIEW vw_ActiveCourses
                // Mengambil kursus yang diajar oleh instruktur yang sedang login
                SqlCommand cmd = new SqlCommand(@"
                    SELECT idCourse, title 
                    FROM vw_ActiveCourses 
                    WHERE instructorName = (SELECT fullName FROM Users WHERE idUser = @id)", this.con);

                cmd.Parameters.AddWithValue("@id", this.instructorId);
                SqlDataReader dr = cmd.ExecuteReader();

                this.cmbCourse.Items.Clear();

                // Gunakan objek anonymous agar ComboBox menyimpan Text dan Value (idCourse)
                while (dr.Read())
                {
                    this.cmbCourse.Items.Add(new { Text = dr["title"].ToString(), Value = Convert.ToInt32(dr["idCourse"]) });
                }
                dr.Close();
                this.con.Close();

                this.cmbCourse.DisplayMember = "Text";
                this.cmbCourse.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat kursus: " + ex.Message);
                if (this.con.State == ConnectionState.Open) this.con.Close();
            }
        }

        private void CmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbCourse.SelectedIndex == -1) return;

            // Ambil idCourse dari ComboBox yang dipilih
            dynamic selectedCourse = this.cmbCourse.SelectedItem;
            this.currentCourseId = selectedCourse.Value;

            this.txtSearchStudent.Text = "Search Student";
            this.txtSearchStudent.ForeColor = System.Drawing.Color.Gray;
            LoadStudents();
        }

        private void LoadStudents()
        {
            if (this.currentCourseId == 0) return;

            try
            {
                this.con.Open();
                // PERBAIKAN: Menggunakan VIEW vw_CourseParticipants
                SqlCommand cmd = new SqlCommand(@"
                    SELECT idEnrollment, fullName, nim, grade 
                    FROM vw_CourseParticipants 
                    WHERE idCourse = @courseId", this.con);

                cmd.Parameters.AddWithValue("@courseId", this.currentCourseId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Menggunakan BindingSource
                bsStudents.DataSource = dt;
                this.dgvStudents.DataSource = bsStudents;

                this.bindingNavigator1.BindingSource = bsStudents;

                // Tambahkan kolom InputGrade jika belum ada
                if (!this.dgvStudents.Columns.Contains("InputGrade"))
                {
                    DataGridViewTextBoxColumn gradeColumn = new DataGridViewTextBoxColumn();
                    gradeColumn.Name = "InputGrade";
                    gradeColumn.HeaderText = "Input Nilai";
                    this.dgvStudents.Columns.Add(gradeColumn);
                }

                // KUNCI KOLOM: Set ReadOnly = true untuk semua kolom kecuali "InputGrade"
                foreach (DataGridViewColumn col in this.dgvStudents.Columns)
                {
                    if (col.Name != "InputGrade")
                    {
                        col.ReadOnly = true; // Tidak bisa diedit
                        col.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray; // Ubah warna jadi abu-abu
                    }
                    else
                    {
                        col.ReadOnly = false; // Bisa diedit
                        col.DefaultCellStyle.BackColor = System.Drawing.Color.White; // Warna putih untuk kolom input
                    }
                }

                // Isi kolom InputGrade dengan nilai yang sudah ada (jika ada)
                foreach (DataGridViewRow row in this.dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (row.Cells["grade"].Value != DBNull.Value && row.Cells["grade"].Value != null)
                    {
                        row.Cells["InputGrade"].Value = row.Cells["grade"].Value.ToString();
                    }
                }
                this.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat siswa: " + ex.Message);
                if (this.con.State == ConnectionState.Open) this.con.Close();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (this.currentCourseId == 0)
            {
                MessageBox.Show("Pilih kursus terlebih dahulu!");
                return;
            }

            string keyword = this.txtSearchStudent.Text.Trim();

            if (keyword == "Search Student" || string.IsNullOrWhiteSpace(keyword))
            {
                // Jika kosong, hilangkan filter (tampilkan semua)
                bsStudents.Filter = string.Empty;
            }
            else
            {
                // PERBAIKAN: Memanfaatkan BindingSource untuk melakukan pencarian di memori (tidak perlu query SQL lagi)
                bsStudents.Filter = string.Format("fullName LIKE '%{0}%' OR nim LIKE '%{0}%'", keyword.Replace("'", "''"));
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.txtSearchStudent.Text = "Search Student";
            this.txtSearchStudent.ForeColor = System.Drawing.Color.Gray;
            bsStudents.Filter = string.Empty; // Bersihkan filter pencarian
            LoadStudents(); // Ambil ulang dari database
            MessageBox.Show("Data berhasil di-refresh!");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.currentCourseId == 0)
            {
                MessageBox.Show("Pilih kursus terlebih dahulu!");
                return;
            }

            // PERBAIKAN: Paksa DataGridView untuk menyimpan nilai yang sedang diketik user saat itu juga
            this.dgvStudents.EndEdit();

            int savedCount = 0;
            int errorCount = 0;

            try
            {
                this.con.Open();
                foreach (DataGridViewRow row in this.dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue;

                    int enrollmentId = Convert.ToInt32(row.Cells["idEnrollment"].Value);
                    // PERBAIKAN: Tambahkan .Trim() untuk menghapus spasi yang tidak sengaja terketik
                    string gradeText = row.Cells["InputGrade"].Value?.ToString().Trim();

                    // Cek apakah nilai berubah (agar tidak update data yang tidak diedit)
                    string oldGrade = row.Cells["grade"].Value != DBNull.Value ? row.Cells["grade"].Value.ToString().Trim() : "";

                    if (!string.IsNullOrEmpty(gradeText) && gradeText != oldGrade)
                    {
                        if (!float.TryParse(gradeText, out float grade))
                        {
                            MessageBox.Show($"Nilai untuk {row.Cells["fullName"].Value} hanya boleh berupa angka!");
                            errorCount++;
                            continue;
                        }

                        if (grade < 0 || grade > 100)
                        {
                            MessageBox.Show($"Nilai untuk {row.Cells["fullName"].Value} harus antara 0 - 100!");
                            errorCount++;
                            continue;
                        }

                        SqlCommand cmd = new SqlCommand("sp_InputGrade", this.con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idEnrollment", enrollmentId);
                        cmd.Parameters.AddWithValue("@grade", grade);

                        cmd.ExecuteNonQuery();
                        savedCount++;
                    }
                }
                this.con.Close();

                if (savedCount > 0)
                {
                    MessageBox.Show($"{savedCount} nilai berhasil disimpan!");
                    LoadStudents(); // Refresh DataGridView setelah simpan
                }
                else if (errorCount == 0)
                {
                    MessageBox.Show("Tidak ada perubahan nilai untuk disimpan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error menyimpan nilai: " + ex.Message);
                if (this.con.State == ConnectionState.Open) this.con.Close();
            }
        }

        private void DgvStudents_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvStudents.CurrentCell.ColumnIndex ==
                dgvStudents.Columns["InputGrade"].Index)
            {
                TextBox txt = e.Control as TextBox;

                if (txt != null)
                {
                    txt.KeyPress -= TxtGrade_KeyPress;
                    txt.KeyPress += TxtGrade_KeyPress;
                }
            }
        }

        private void TxtGrade_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Izinkan angka
            if (char.IsDigit(e.KeyChar))
                return;

            // Izinkan Backspace
            if (e.KeyChar == (char)Keys.Back)
                return;

            // Izinkan titik desimal sekali
            TextBox txt = sender as TextBox;

            if (e.KeyChar == '.' && !txt.Text.Contains("."))
                return;

            // Selain itu ditolak
            e.Handled = true;
        }
        // ... existing Focus events remain unchanged ...
        private void TxtSearchStudent_GotFocus(object sender, EventArgs e)
        {
            if (this.txtSearchStudent.Text == "Search Student")
            {
                this.txtSearchStudent.Text = "";
                this.txtSearchStudent.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtSearchStudent_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtSearchStudent.Text))
            {
                this.txtSearchStudent.Text = "Search Student";
                this.txtSearchStudent.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void FormInputGrade_Load_1(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }
    }
}