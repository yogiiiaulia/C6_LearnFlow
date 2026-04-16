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
        }

        private void FormManageCourse_Load(object sender, EventArgs e)
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT idCourse, title, description, quota FROM Courses WHERE instructor_id = @id", con);
                cmd.Parameters.AddWithValue("@id", instructorId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCourses.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Judul kursus tidak boleh kosong!");
                return;
            }

            int quota = txtQuota.Text == "" ? 30 : int.Parse(txtQuota.Text);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Courses (title, description, quota, instructor_id, isActive)
                    VALUES (@title, @desc, @quota, @instId, 1)", con);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@desc", txtDescription.Text);
                cmd.Parameters.AddWithValue("@quota", quota);
                cmd.Parameters.AddWithValue("@instId", instructorId);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Kursus berhasil ditambahkan!");
                ClearForm();
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCourseId == -1)
            {
                MessageBox.Show("Pilih kursus yang akan diupdate!");
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Courses 
                    SET title = @title, description = @desc, quota = @quota 
                    WHERE idCourse = @id", con);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text);
                cmd.Parameters.AddWithValue("@desc", txtDescription.Text);
                cmd.Parameters.AddWithValue("@quota", txtQuota.Text == "" ? 30 : int.Parse(txtQuota.Text));
                cmd.Parameters.AddWithValue("@id", selectedCourseId);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Kursus berhasil diupdate!");
                ClearForm();
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM Courses WHERE idCourse = @id", con);
                    cmd.Parameters.AddWithValue("@id", selectedCourseId);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Kursus berhasil dihapus!");
                    ClearForm();
                    LoadCourses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    con.Close();
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
                txtDescription.Text = row.Cells["description"].Value.ToString();
                txtQuota.Text = row.Cells["quota"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            selectedCourseId = -1;
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtQuota.Text = "";
        }

        private void FormManageCourse_Load_1(object sender, EventArgs e)
        {

        }
    }
}