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
        }

        private void FormInputGrade_Load(object sender, EventArgs e)
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                this.con.Open();
                SqlCommand cmd = new SqlCommand("SELECT idCourse, title FROM Courses WHERE instructor_id = @id", this.con);
                cmd.Parameters.AddWithValue("@id", this.instructorId);
                SqlDataReader dr = cmd.ExecuteReader();
                this.cmbCourse.Items.Clear();
                while (dr.Read())
                {
                    this.cmbCourse.Items.Add(dr["title"].ToString());
                }
                dr.Close();
                this.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.con.Close();
            }
        }

        private void CmbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbCourse.SelectedIndex == -1) return;

            string selectedCourse = this.cmbCourse.SelectedItem.ToString();
            this.currentCourseId = GetCourseId(selectedCourse);
            this.txtSearchStudent.Text = "";
            LoadStudents();
        }

        private int GetCourseId(string title)
        {
            int id = 0;
            try
            {
                this.con.Open();
                SqlCommand cmd = new SqlCommand("SELECT idCourse FROM Courses WHERE title = @title", this.con);
                cmd.Parameters.AddWithValue("@title", title);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                this.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.con.Close();
            }
            return id;
        }

        private void LoadStudents()
        {
            if (this.currentCourseId == 0) return;

            try
            {
                this.con.Open();
                string sql = @"
                    SELECT e.idEnrollment, u.fullName, s.nim, e.grade
                    FROM Enrollments e
                    JOIN Users u ON e.idUser = u.idUser
                    JOIN Students s ON s.idUser = u.idUser
                    WHERE e.idCourse = @courseId";
                SqlCommand cmd = new SqlCommand(sql, this.con);
                cmd.Parameters.AddWithValue("@courseId", this.currentCourseId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dgvStudents.DataSource = dt;

                if (!this.dgvStudents.Columns.Contains("InputGrade"))
                {
                    DataGridViewTextBoxColumn gradeColumn = new DataGridViewTextBoxColumn();
                    gradeColumn.Name = "InputGrade";
                    gradeColumn.HeaderText = "Input Nilai";
                    this.dgvStudents.Columns.Add(gradeColumn);
                }

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
                MessageBox.Show("Error: " + ex.Message);
                this.con.Close();
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

            try
            {
                this.con.Open();
                string sql = @"
                    SELECT e.idEnrollment, u.fullName, s.nim, e.grade
                    FROM Enrollments e
                    JOIN Users u ON e.idUser = u.idUser
                    JOIN Students s ON s.idUser = u.idUser
                    WHERE e.idCourse = @courseId";

                if (keyword != "")
                {
                    sql += " AND u.fullName LIKE @keyword";
                }

                SqlCommand cmd = new SqlCommand(sql, this.con);
                cmd.Parameters.AddWithValue("@courseId", this.currentCourseId);
                if (keyword != "")
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.dgvStudents.DataSource = dt;

                if (!this.dgvStudents.Columns.Contains("InputGrade"))
                {
                    DataGridViewTextBoxColumn gradeColumn = new DataGridViewTextBoxColumn();
                    gradeColumn.Name = "InputGrade";
                    gradeColumn.HeaderText = "Input Nilai";
                    this.dgvStudents.Columns.Add(gradeColumn);
                }
                this.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.con.Close();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.txtSearchStudent.Text = "";
            LoadStudents();
            MessageBox.Show("Data berhasil di-refresh!");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (this.currentCourseId == 0)
            {
                MessageBox.Show("Pilih kursus terlebih dahulu!");
                return;
            }

            int savedCount = 0;

            try
            {
                this.con.Open();
                foreach (DataGridViewRow row in this.dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue;

                    int enrollmentId = Convert.ToInt32(row.Cells["idEnrollment"].Value);
                    string gradeText = row.Cells["InputGrade"].Value?.ToString();

                    if (!string.IsNullOrEmpty(gradeText))
                    {
                        if (float.TryParse(gradeText, out float grade))
                        {
                            if (grade < 0 || grade > 100)
                            {
                                MessageBox.Show($"Nilai untuk {row.Cells["fullName"].Value} harus antara 0-100!");
                                continue;
                            }

                            SqlCommand cmd = new SqlCommand("UPDATE Enrollments SET grade = @grade WHERE idEnrollment = @id", this.con);
                            cmd.Parameters.AddWithValue("@grade", grade);
                            cmd.Parameters.AddWithValue("@id", enrollmentId);
                            cmd.ExecuteNonQuery();
                            savedCount++;
                        }
                        else
                        {
                            MessageBox.Show($"Nilai untuk {row.Cells["fullName"].Value} tidak valid!");
                        }
                    }
                }
                this.con.Close();

                MessageBox.Show($"{savedCount} nilai berhasil disimpan!");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.con.Close();
            }
        }
    }
}