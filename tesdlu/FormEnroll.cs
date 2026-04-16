using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormEnroll : Form
    {
        private int userId;

        public FormEnroll(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            btnEnroll.Click += BtnEnroll_Click;
            this.Load += FormEnroll_Load;
        }

        private void FormEnroll_Load(object sender, EventArgs e)
        {
            LoadAvailableCourses();
            LoadMyCourses();
        }

        private void LoadAvailableCourses()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT idCourse, title 
                        FROM Courses 
                        WHERE isActive = 1 
                        AND idCourse NOT IN (SELECT idCourse FROM Enrollments WHERE idUser = @userId)", con);
                    cmd.Parameters.AddWithValue("@userId", userId);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadMyCourses()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT c.title, e.enrollDate, 
                               CASE WHEN e.grade IS NULL THEN 'Belum ada nilai' ELSE e.grade.ToString() END AS grade
                        FROM Enrollments e
                        JOIN Courses c ON e.idCourse = c.idCourse
                        WHERE e.idUser = @userId", con);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMyCourses.DataSource = dt;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BtnEnroll_Click(object sender, EventArgs e)
        {
            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Pilih kursus yang akan didaftar!");
                return;
            }

            dynamic course = cmbCourse.SelectedItem;
            int courseId = course.Value;

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_EnrollStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUser", userId);
                    cmd.Parameters.AddWithValue("@idCourse", courseId);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show(dr["message"].ToString());
                    }
                    dr.Close();
                    con.Close();
                }

                LoadAvailableCourses();
                LoadMyCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FormEnroll_Load_1(object sender, EventArgs e)
        {

        }
    }
}