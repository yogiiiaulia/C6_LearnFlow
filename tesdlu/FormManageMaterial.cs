using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormManageMaterial : Form
    {
        private SqlConnection con;
        private int currentUserId;
        private int selectedMaterialId = -1;
        private string selectedFilePath = "";

        // SYARAT POIN 4: Tambahkan BindingSource
        private BindingSource bsMaterials = new BindingSource();

        public FormManageMaterial(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
            this.Load += FormManageMaterial_Load;
            cmbCourse.SelectedIndexChanged += CmbCourse_SelectedIndexChanged;
            btnBrowse.Click += BtnBrowse_Click;
            btnUpload.Click += BtnUpload_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            dgvMaterials.CellClick += DgvMaterials_CellClick;
        }

        private void FormManageMaterial_Load(object sender, EventArgs e)
        {
            ClearForm();
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                con.Open();
                // Mengambil course dari db menggunakan query inline karena SP ini tidak terdaftar di daftar persyaratan
                // Jika ingin dibuat View juga bisa, tapi ini bukan tabel utama form ini.
                SqlCommand cmd = new SqlCommand(@"
                    SELECT c.idCourse, c.title 
                    FROM Courses c
                    WHERE c.instructor_id = @userId OR c.assistant_id = @userId", con);
                cmd.Parameters.AddWithValue("@userId", currentUserId);
                SqlDataReader dr = cmd.ExecuteReader();
                cmbCourse.Items.Clear();
                while (dr.Read())
                {
                    cmbCourse.Items.Add(new { Text = dr["title"].ToString(), Value = dr["idCourse"] });
                }
                dr.Close();
                con.Close();

                cmbCourse.DisplayMember = "Text";
                cmbCourse.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error LoadCourses: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void CmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            if (cmbCourse.SelectedItem == null) return;

            dynamic course = cmbCourse.SelectedItem;
            int courseId = course.Value;

            try
            {
                con.Open();
                // SYARAT POIN 2: Menggunakan VIEW (vw_MaterialsByCourse)
                SqlCommand cmd = new SqlCommand("SELECT idMaterial, title, filePath FROM vw_MaterialsByCourse WHERE idCourse = @courseId", con);
                cmd.Parameters.AddWithValue("@courseId", courseId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // SYARAT POIN 4: Menggunakan BindingSource
                bsMaterials.DataSource = dt;
                dgvMaterials.DataSource = bsMaterials;

                bindingNavigator1.BindingSource = bsMaterials;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error LoadMaterials: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "All Files|*.*|PDF|*.pdf|Word|*.docx|PowerPoint|*.pptx";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                MessageBox.Show("File dipilih: " + Path.GetFileName(selectedFilePath));
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Pilih kursus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Judul Materi")
            {
                MessageBox.Show("Judul materi tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(selectedFilePath))
            {
                MessageBox.Show("Pilih file terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic course = cmbCourse.SelectedItem;
            int courseId = course.Value;

            string fileName = Path.GetFileName(selectedFilePath);
            string destPath = Path.Combine(Application.StartupPath, "Materials", courseId.ToString());
            Directory.CreateDirectory(destPath);
            string savedPath = Path.Combine(destPath, fileName);
            File.Copy(selectedFilePath, savedPath, true);

            try
            {
                con.Open();
                // SYARAT POIN 1: Menggunakan Stored Procedure untuk Insert
                SqlCommand cmd = new SqlCommand("sp_UpsertMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Karena ini Insert, idMaterial dibiarkan NULL (seperti di database)
                // Kita gunakan DBNull.Value
                cmd.Parameters.AddWithValue("@idMaterial", DBNull.Value);
                cmd.Parameters.AddWithValue("@idCourse", courseId);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@filePath", savedPath);
                cmd.Parameters.AddWithValue("@uploadedBy", currentUserId);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int result = Convert.ToInt32(dr["result"]);
                    string msg = dr["message"].ToString();

                    if (result == 1)
                    {
                        MessageBox.Show(msg, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(msg, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                dr.Close();
                con.Close();

                ClearForm();
                LoadMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Upload: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaterialId == -1)
            {
                MessageBox.Show("Pilih materi yang akan diupdate dari tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Pilih kursus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == "Judul Materi")
            {
                MessageBox.Show("Judul materi tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic course = cmbCourse.SelectedItem;
            int courseId = course.Value;

            try
            {
                con.Open();
                // SYARAT POIN 1: Menggunakan Stored Procedure untuk Update
                // Menggunakan SP yang sama (Upsert), bedanya kita kirim @idMaterial
                SqlCommand cmd = new SqlCommand("sp_UpsertMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idMaterial", selectedMaterialId);
                cmd.Parameters.AddWithValue("@idCourse", courseId);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);

                // Gunakan path lama jika user tidak pilih file baru
                // Jika selectedFilePath "", berarti user tidak klik Browse
                string pathToSave = string.IsNullOrWhiteSpace(selectedFilePath) ? GetCurrentFilePath(selectedMaterialId) : selectedFilePath;
                cmd.Parameters.AddWithValue("@filePath", pathToSave);
                cmd.Parameters.AddWithValue("@uploadedBy", currentUserId);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int result = Convert.ToInt32(dr["result"]);
                    string msg = dr["message"].ToString();

                    if (result == 1)
                    {
                        MessageBox.Show(msg, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(msg, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                dr.Close();
                con.Close();

                ClearForm();
                LoadMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        // Helper method untuk mendapatkan file path lama
        private string GetCurrentFilePath(int idMat)
        {
            string path = "";
            // Kita pakai koneksi baru sementara agar tidak bentrok dengan DataReader yg sedang buka koneksi
            using (SqlConnection tempCon = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True"))
            {
                tempCon.Open();
                SqlCommand cmd = new SqlCommand("SELECT filePath FROM Materials WHERE idMaterial = @id", tempCon);
                cmd.Parameters.AddWithValue("@id", idMat);
                object result = cmd.ExecuteScalar();
                if (result != null) path = result.ToString();
            }
            return path;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaterialId == -1)
            {
                MessageBox.Show("Pilih materi yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Yakin hapus materi ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    // SYARAT POIN 1: Menggunakan Stored Procedure untuk Delete
                    SqlCommand cmd = new SqlCommand("sp_DeleteMaterial", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idMaterial", selectedMaterialId);
                    cmd.Parameters.AddWithValue("@requestedBy", currentUserId); // Validasi keamanan di SQL

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        int res = Convert.ToInt32(dr["result"]);
                        string msg = dr["message"].ToString();

                        if (res == 1)
                        {
                            MessageBox.Show(msg, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    dr.Close();
                    con.Close();

                    ClearForm();
                    LoadMaterials();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (con.State == ConnectionState.Open) con.Close();
                }
            }
        }

        private void DgvMaterials_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMaterials.Rows[e.RowIndex];
                selectedMaterialId = Convert.ToInt32(row.Cells["idMaterial"].Value);
                txtTitle.Text = row.Cells["title"].Value.ToString();
                txtTitle.ForeColor = System.Drawing.Color.Black;
                // Jangan isi file path dengan file path yg panjang, biar user kalau mau update file, harus browse ulang
                selectedFilePath = "";
            }
        }

        private void ClearForm()
        {
            selectedMaterialId = -1;
            txtTitle.Text = "Judul Materi";
            txtTitle.ForeColor = System.Drawing.Color.Gray;
            selectedFilePath = "";
        }

        private void TxtTitle_GotFocus(object sender, EventArgs e)
        {
            if (txtTitle.Text == "Judul Materi")
            {
                txtTitle.Text = "";
                txtTitle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtTitle_LostFocus(object sender, EventArgs e)
        {
            if (txtTitle.Text == "" && selectedMaterialId == -1)
            {
                txtTitle.Text = "Judul Materi";
                txtTitle.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void FormManageMaterial_Load_1(object sender, EventArgs e)
        {
 
        }
    }
}