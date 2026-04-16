using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormViewParticipants : Form
    {
        private SqlConnection con;
        private int currentUserId;

        public FormViewParticipants(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
            this.Load += FormViewParticipants_Load;
            cmbCourse.SelectedIndexChanged += CmbCourse_SelectedIndexChanged;
        }

        private void FormViewParticipants_Load(object sender, EventArgs e)
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
                dgvParticipants.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void FormViewParticipants_Load_1(object sender, EventArgs e)
        {

        }
    }
}