using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormSearchCourse : Form
    {
        private SqlConnection con;

        public FormSearchCourse()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");

            // Hook events
            btnSearch.Click += BtnSearch_Click;
            this.Load += FormSearchCourse_Load;
            txtSearch.GotFocus += TxtSearch_GotFocus;
            txtSearch.LostFocus += TxtSearch_LostFocus;
        }

        private void FormSearchCourse_Load(object sender, EventArgs e)
        {
            txtSearch.Text = "Search Course";
            txtSearch.ForeColor = System.Drawing.Color.Gray;
            LoadAllCourses();
        }

        private void LoadAllCourses()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT idCourse, title, description, quota, 
                           (SELECT COUNT(*) FROM Enrollments WHERE idCourse = c.idCourse) AS enrolled
                    FROM Courses c
                    WHERE isActive = 1", con);
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (keyword == "" || keyword == "Search Course")
            {
                LoadAllCourses();
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT idCourse, title, description, quota, 
                           (SELECT COUNT(*) FROM Enrollments WHERE idCourse = c.idCourse) AS enrolled
                    FROM Courses c
                    WHERE isActive = 1 AND title LIKE @keyword", con);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
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

        private void TxtSearch_GotFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search Course")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtSearch_LostFocus(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Search Course";
                txtSearch.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void FormSearchCourse_Load_1(object sender, EventArgs e)
        {

        }
    }
}