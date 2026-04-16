using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormMyGrades : Form
    {
        private int userId;

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
                    SqlCommand cmd = new SqlCommand("sp_ViewGrades", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUser", userId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvGrades.DataSource = dt;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}