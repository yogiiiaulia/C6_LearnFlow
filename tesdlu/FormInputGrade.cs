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

        public FormInputGrade(int instructorId)
        {
            InitializeComponent();
            this.instructorId = instructorId;
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
            this.Load += FormInputGrade_Load;
            cmbCourse.SelectedIndexChanged += CmbCourse_SelectedIndexChanged;
            btnSave.Click += BtnSave_Click;
        }

        private void FormInputGrade_Load(object sender, EventArgs e)
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT idCourse, title FROM Courses WHERE instructor_id = @id", con);
                cmd.Parameters.AddWithValue("@id", instructorId);
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
            if (cmbCourse.SelectedItem == null) return;

            dynamic course = cmbCourse.SelectedItem;
            int courseId = course.Value;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_GetCourseParticipants", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCourse", courseId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvStudents.DataSource = dt;

                // Tambah kolom input nilai
                if (!dgvStudents.Columns.Contains("InputGrade"))
                {
                    DataGridViewTextBoxColumn gradeColumn = new DataGridViewTextBoxColumn();
                    gradeColumn.Name = "InputGrade";
                    gradeColumn.HeaderText = "Input Nilai";
                    dgvStudents.Columns.Add(gradeColumn);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Pilih kursus terlebih dahulu!");
                return;
            }

            try
            {
                con.Open();
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    if (row.IsNewRow) continue;

                    int enrollmentId = Convert.ToInt32(row.Cells["idEnrollment"].Value);
                    string gradeText = row.Cells["InputGrade"].Value?.ToString();

                    if (!string.IsNullOrEmpty(gradeText) && float.TryParse(gradeText, out float grade))
                    {
                        SqlCommand cmd = new SqlCommand("sp_InputGrade", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idEnrollment", enrollmentId);
                        cmd.Parameters.AddWithValue("@grade", grade);
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();

                MessageBox.Show("Nilai berhasil disimpan!");
                CmbCourse_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void FormInputGrade_Load_1(object sender, EventArgs e)
        {

        }
    }
}