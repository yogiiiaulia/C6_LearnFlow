using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormManageCourse : Form
    {
        private SqlConnection con;
        private int instructorId;
        private int selectedCourseId = -1;

        public FormManageCourse(int instructorId)
        {
            InitializeComponent();
            this.instructorId = instructorId;
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
            this.Load += FormManageCourse_Load;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;
            dgvCourses.CellClick += DgvCourses_CellClick;

            // Placeholder events
            txtTitle.GotFocus += TxtTitle_GotFocus;
            txtTitle.LostFocus += TxtTitle_LostFocus;
            txtDescription.GotFocus += TxtDescription_GotFocus;
            txtDescription.LostFocus += TxtDescription_LostFocus;
        }

        private void FormManageCourse_Load(object sender, EventArgs e)
        {
            ClearForm();
            LoadCourses();
        }

        // Tambahkan variabel BindingSource di tingkat kelas (di bawah deklarasi con)
        private BindingSource bindingSource1 = new BindingSource();

        private void LoadCourses()
        {
            try
            {
                con.Open();
                // SYARAT POIN 2: Menggunakan VIEW (vw_ManageCourses)
                SqlCommand cmd = new SqlCommand("SELECT idCourse, title, description, quota FROM vw_ManageCourses WHERE instructor_id = @id", con);
                cmd.Parameters.AddWithValue("@id", instructorId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // SYARAT POIN 4 & 5: Menggunakan BindingSource & BindingNavigator
                bindingSource1.DataSource = dt;
                dgvCourses.DataSource = bindingSource1;
                bindingNavigator1.BindingSource = bindingSource1;

                // Jika kamu sudah menambahkan bindingNavigator1 di UI design, uncomment baris di bawah:
                // bindingNavigator1.BindingSource = bindingSource1; 

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Judul Kursus" ||
                string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text == "Deskripsi Kursus" ||
                string.IsNullOrWhiteSpace(txtQuota.Text))
            {
                MessageBox.Show("Semua kolom tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuota.Text, out int quota))
            {
                MessageBox.Show("Kolom Kuota harus berupa angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();
                // SYARAT POIN 1: Menggunakan Stored Procedure (sp_InsertCourse)
                SqlCommand cmd = new SqlCommand("sp_InsertCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@quota", quota);
                cmd.Parameters.AddWithValue("@instructor_id", instructorId);

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Kursus berhasil ditambahkan!");
                ClearForm();
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCourseId == -1)
            {
                MessageBox.Show("Pilih kursus yang akan diupdate!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Judul Kursus" ||
                string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text == "Deskripsi Kursus" ||
                string.IsNullOrWhiteSpace(txtQuota.Text))
            {
                MessageBox.Show("Semua kolom tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuota.Text, out int quota))
            {
                MessageBox.Show("Kolom Kuota harus berupa angka!");
                return;
            }

            try
            {
                con.Open();
                // SYARAT POIN 1: Menggunakan Stored Procedure (sp_UpdateCourse)
                SqlCommand cmd = new SqlCommand("sp_UpdateCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCourse", selectedCourseId);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                cmd.Parameters.AddWithValue("@quota", quota);

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Kursus berhasil diupdate!");
                ClearForm();
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCourseId == -1)
            {
                MessageBox.Show("Pilih kursus yang akan dihapus!");
                return;
            }

            DialogResult result = MessageBox.Show("Yakin hapus kursus ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    // SYARAT POIN 1: Menggunakan Stored Procedure (sp_DeleteCourse)
                    SqlCommand cmd = new SqlCommand("sp_DeleteCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCourse", selectedCourseId);

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Kursus berhasil dihapus!");
                    ClearForm();
                    LoadCourses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    if (con.State == ConnectionState.Open) con.Close();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadCourses();
            ClearForm();
        }

        private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCourses.Rows[e.RowIndex];
                selectedCourseId = Convert.ToInt32(row.Cells["idCourse"].Value);
                txtTitle.Text = row.Cells["title"].Value.ToString();
                txtTitle.ForeColor = System.Drawing.Color.Black;
                txtDescription.Text = row.Cells["description"].Value.ToString();
                txtDescription.ForeColor = System.Drawing.Color.Black;
                txtQuota.Text = row.Cells["quota"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            selectedCourseId = -1;
            txtTitle.Text = "Judul Kursus";
            txtTitle.ForeColor = System.Drawing.Color.Gray;
            txtDescription.Text = "Deskripsi Kursus";
            txtDescription.ForeColor = System.Drawing.Color.Gray;
            txtQuota.Text = "";
        }

        private void TxtTitle_GotFocus(object sender, EventArgs e)
        {
            if (txtTitle.Text == "Judul Kursus")
            {
                txtTitle.Text = "";
                txtTitle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtTitle_LostFocus(object sender, EventArgs e)
        {
            if (txtTitle.Text == "" && selectedCourseId == -1)
            {
                txtTitle.Text = "Judul Kursus";
                txtTitle.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void TxtDescription_GotFocus(object sender, EventArgs e)
        {
            if (txtDescription.Text == "Deskripsi Kursus")
            {
                txtDescription.Text = "";
                txtDescription.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtDescription_LostFocus(object sender, EventArgs e)
        {
            if (txtDescription.Text == "" && selectedCourseId == -1)
            {
                txtDescription.Text = "Deskripsi Kursus";
                txtDescription.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void FormManageCourse_Load_1(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }
    }
}