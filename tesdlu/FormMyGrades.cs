using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormMyGrades : Form
    {
        private int userId;

        // SYARAT POIN 4: Deklarasi BindingSource
        private BindingSource bsGrades = new BindingSource();

        public FormMyGrades(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.Load += FormMyGrades_Load;
        }

        private void FormMyGrades_Load(object sender, EventArgs e)
        {
            LoadGrades();
        }

        private void LoadGrades()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True"))
                {
                    con.Open();

                    // Menggunakan Stored Procedure untuk menampilkan nilai
                    SqlCommand cmd = new SqlCommand("sp_ViewGrades", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUser", userId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // SYARAT POIN 4: Memasukkan DataTable ke BindingSource, lalu ke DataGridView
                    bsGrades.DataSource = dt;
                    dgvGrades.DataSource = bsGrades;

                    bindingNavigator1.BindingSource = bsGrades;

                    con.Close();

                    // (Opsional) Merapikan nama kolom jika ingin UI terlihat lebih rapi
                    if (dgvGrades.Columns.Count > 0)
                    {
                        if (dgvGrades.Columns.Contains("CourseName"))
                            dgvGrades.Columns["CourseName"].HeaderText = "Nama Kursus";
                        if (dgvGrades.Columns.Contains("enrollDate"))
                            dgvGrades.Columns["enrollDate"].HeaderText = "Tanggal Enroll";
                        if (dgvGrades.Columns.Contains("grade"))
                            dgvGrades.Columns["grade"].HeaderText = "Nilai Angka";
                        if (dgvGrades.Columns.Contains("letterGrade"))
                            dgvGrades.Columns["letterGrade"].HeaderText = "Nilai Huruf";

                        // Menyembunyikan idCourse dari user
                        if (dgvGrades.Columns.Contains("idCourse"))
                            dgvGrades.Columns["idCourse"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMyGrades_Load_1(object sender, EventArgs e)
        {

        }
    }
}