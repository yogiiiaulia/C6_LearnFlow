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
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                con.Open();
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
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
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
                SqlCommand cmd = new SqlCommand("SELECT idMaterial, title, filePath FROM Materials WHERE idCourse = @courseId", con);
                cmd.Parameters.AddWithValue("@courseId", courseId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMaterials.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
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
                MessageBox.Show("Pilih kursus terlebih dahulu!");
                return;
            }
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Judul materi tidak boleh kosong!");
                return;
            }
            if (selectedFilePath == "")
            {
                MessageBox.Show("Pilih file terlebih dahulu!");
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
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Materials (idCourse, title, filePath, uploadedBy)
                    VALUES (@courseId, @title, @path, @userId)", con);
                cmd.Parameters.AddWithValue("@courseId", courseId);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@path", savedPath);
                cmd.Parameters.AddWithValue("@userId", currentUserId);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Materi berhasil diupload!");
                ClearForm();
                LoadMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMaterialId == -1)
            {
                MessageBox.Show("Pilih materi yang akan diupdate!");
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Materials SET title = @title WHERE idMaterial = @id", con);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@id", selectedMaterialId);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Materi berhasil diupdate!");
                ClearForm();
                LoadMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaterialId == -1)
            {
                MessageBox.Show("Pilih materi yang akan dihapus!");
                return;
            }

            DialogResult result = MessageBox.Show("Yakin hapus materi ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Materials WHERE idMaterial = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedMaterialId);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Materi berhasil dihapus!");
                    ClearForm();
                    LoadMaterials();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    con.Close();
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
            }
        }

        private void ClearForm()
        {
            selectedMaterialId = -1;
            txtTitle.Text = "";
            selectedFilePath = "";
        }

        private void FormManageMaterial_Load_1(object sender, EventArgs e)
        {

        }
    }
}