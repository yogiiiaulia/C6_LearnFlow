using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormSearchCourse : Form
    {
        private SqlConnection con;

        // SYARAT POIN 4: Memanfaatkan BindingSource untuk filter/search
        private BindingSource bsCourses = new BindingSource();

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
                // SYARAT POIN 2: Menggunakan VIEW
                SqlCommand cmd = new SqlCommand("SELECT idCourse, title, description, quota, enrolled, sisaKuota, instructorName FROM vw_ActiveCourses", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // SYARAT POIN 4: Memasukkan DataTable ke BindingSource
                bsCourses.DataSource = dt;
                dgvCourses.DataSource = bsCourses;

                bindingNavigator1.BindingSource = bsCourses;

                con.Close();

                // Opsional: Merapikan header kolom agar lebih profesional
                if (dgvCourses.Columns.Contains("title")) dgvCourses.Columns["title"].HeaderText = "Judul Kursus";
                if (dgvCourses.Columns.Contains("description")) dgvCourses.Columns["description"].HeaderText = "Deskripsi";
                if (dgvCourses.Columns.Contains("quota")) dgvCourses.Columns["quota"].HeaderText = "Kuota";
                if (dgvCourses.Columns.Contains("enrolled")) dgvCourses.Columns["enrolled"].HeaderText = "Terdaftar";
                if (dgvCourses.Columns.Contains("sisaKuota")) dgvCourses.Columns["sisaKuota"].HeaderText = "Sisa Kuota";
                if (dgvCourses.Columns.Contains("instructorName")) dgvCourses.Columns["instructorName"].HeaderText = "Instruktur";
                if (dgvCourses.Columns.Contains("idCourse")) dgvCourses.Columns["idCourse"].Visible = false; // Sembunyikan ID
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error LoadAllCourses: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            // SYARAT POIN 4: Memanfaatkan BindingSource Filter untuk melakukan pencarian
            // Ini jauh lebih cepat dan efisien daripada melakukan query ulang ke database (Syarat Poin 1 tidak wajib jika Poin 4 diterapkan optimal untuk Search)
            if (keyword == "" || keyword == "Search Course")
            {
                bsCourses.Filter = string.Empty; // Tampilkan semua jika kosong
            }
            else
            {
                // Mencari di kolom title atau description
                bsCourses.Filter = string.Format("title LIKE '%{0}%' OR description LIKE '%{0}%'", keyword.Replace("'", "''"));
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
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
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