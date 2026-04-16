using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormDashboard : Form
    {
        private int currentUserId;
        private string currentRole;
        private string currentFullName;
        private SqlConnection con;

        public FormDashboard(int userId, string fullName, string role)
        {
            InitializeComponent();
            currentUserId = userId;
            currentFullName = fullName;
            currentRole = role;
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");

            // Hook events
            btnUsers.Click += BtnUsers_Click;
            btnCourses.Click += BtnCourses_Click;
            btnEnrollment.Click += BtnEnrollment_Click;
            btnNilai.Click += BtnNilai_Click;
            btnMaterials.Click += BtnMaterials_Click;
            btnEditProfil.Click += BtnEditProfil_Click;
            btnLogout.Click += BtnLogout_Click;
            this.Load += FormDashboard_Load;
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Selamat datang, {currentFullName} ({currentRole})";
            LoadStatistics();

            // Sembunyikan menu berdasarkan role
            if (currentRole == "Student")
            {
                btnUsers.Visible = false;
                btnMaterials.Visible = false;
            }
            else if (currentRole == "Assistant")
            {
                btnUsers.Visible = false;
                btnNilai.Visible = false;
            }
        }

        private void LoadStatistics()
        {
            try
            {
                con.Open();

                SqlCommand cmdUser = new SqlCommand("SELECT COUNT(*) FROM Users", con);
                lblTotalUser.Text = cmdUser.ExecuteScalar().ToString();

                SqlCommand cmdCourse = new SqlCommand("SELECT COUNT(*) FROM Courses", con);
                lblTotalCourse.Text = cmdCourse.ExecuteScalar().ToString();

                SqlCommand cmdEnroll = new SqlCommand("SELECT COUNT(*) FROM Enrollments", con);
                lblTotalEnrollment.Text = cmdEnroll.ExecuteScalar().ToString();

                SqlCommand cmdInstructor = new SqlCommand("SELECT COUNT(*) FROM Instructors", con);
                lblTotalInstructor.Text = cmdInstructor.ExecuteScalar().ToString();

                SqlCommand cmdStudent = new SqlCommand("SELECT COUNT(*) FROM Students", con);
                lblTotalStudent.Text = cmdStudent.ExecuteScalar().ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            FormUsers form = new FormUsers();
            form.ShowDialog();
            LoadStatistics();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            if (currentRole == "Instructor")
            {
                FormManageCourse form = new FormManageCourse(currentUserId);
                form.ShowDialog();
                LoadStatistics();
            }
            else
            {
                FormSearchCourse form = new FormSearchCourse();
                form.ShowDialog();
            }
        }

        private void BtnEnrollment_Click(object sender, EventArgs e)
        {
            if (currentRole == "Student")
            {
                FormEnroll form = new FormEnroll(currentUserId);
                form.ShowDialog();
                LoadStatistics();
            }
            else if (currentRole == "Instructor" || currentRole == "Assistant")
            {
                FormViewParticipants form = new FormViewParticipants(currentUserId);
                form.ShowDialog();
            }
            else
            {
                FormSearchCourse form = new FormSearchCourse();
                form.ShowDialog();
            }
        }

        private void BtnNilai_Click(object sender, EventArgs e)
        {
            if (currentRole == "Instructor")
            {
                FormInputGrade form = new FormInputGrade(currentUserId);
                form.ShowDialog();
            }
            else if (currentRole == "Student")
            {
                FormMyGrades form = new FormMyGrades(currentUserId);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Fitur ini hanya untuk Instructor dan Student!");
            }
        }

        private void BtnMaterials_Click(object sender, EventArgs e)
        {
            FormManageMaterial form = new FormManageMaterial(currentUserId);
            form.ShowDialog();
        }

        private void BtnEditProfil_Click(object sender, EventArgs e)
        {
            FormEditProfil form = new FormEditProfil(currentUserId, currentFullName);
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT fullName FROM Users WHERE idUser = @id", con);
                    cmd.Parameters.AddWithValue("@id", currentUserId);
                    currentFullName = cmd.ExecuteScalar().ToString();
                    con.Close();
                    lblWelcome.Text = $"Selamat datang, {currentFullName} ({currentRole})";
                }
                catch { con.Close(); }
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                FormLogin login = new FormLogin();
                login.Show();
                this.Close();
            }
        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}