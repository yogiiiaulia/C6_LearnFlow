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

        // SYARAT POIN 4: Menggunakan BindingSource
        private BindingSource bsParticipants = new BindingSource();

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
                // SYARAT POIN 2: Menggunakan VIEW (vw_AllCourses)
                SqlCommand cmd = new SqlCommand(@"
                    SELECT idCourse, title 
                    FROM vw_AllCourses 
                    WHERE instructor_id = @userId OR assistant_id = @userId", con);
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
            if (cmbCourse.SelectedItem == null) return;

            dynamic course = cmbCourse.SelectedItem;
            int courseId = course.Value;

            try
            {
                con.Open();

                // SYARAT POIN 2: Menggunakan VIEW (Bukan lagi SP, karena ini murni query SELECT)
                SqlCommand cmd = new SqlCommand(@"
                    SELECT idEnrollment, fullName, nim, prodi, enrollDate, grade 
                    FROM vw_CourseParticipants 
                    WHERE idCourse = @idCourse", con);
                cmd.Parameters.AddWithValue("@idCourse", courseId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // SYARAT POIN 4: Menggunakan BindingSource ke DataGridView
                bsParticipants.DataSource = dt;
                dgvParticipants.DataSource = bsParticipants;

                bindingNavigator1.BindingSource = bsParticipants;

                con.Close();

                // Opsional: Merapikan header kolom agar terlihat lebih profesional
                if (dgvParticipants.Columns.Contains("idEnrollment")) dgvParticipants.Columns["idEnrollment"].Visible = false;
                if (dgvParticipants.Columns.Contains("fullName")) dgvParticipants.Columns["fullName"].HeaderText = "Nama Lengkap";
                if (dgvParticipants.Columns.Contains("nim")) dgvParticipants.Columns["nim"].HeaderText = "NIM";
                if (dgvParticipants.Columns.Contains("prodi")) dgvParticipants.Columns["prodi"].HeaderText = "Program Studi";
                if (dgvParticipants.Columns.Contains("enrollDate")) dgvParticipants.Columns["enrollDate"].HeaderText = "Tanggal Daftar";
                if (dgvParticipants.Columns.Contains("grade")) dgvParticipants.Columns["grade"].HeaderText = "Nilai Akhir";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error LoadParticipants: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void FormViewParticipants_Load_1(object sender, EventArgs e)
        {

        }
    }
}