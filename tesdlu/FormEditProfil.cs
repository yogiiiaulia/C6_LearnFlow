using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormEditProfil : Form
    {
        private int userId;
        private string currentFullName;
        private SqlConnection con;

        public FormEditProfil(int userId, string fullName)
        {
            InitializeComponent();
            this.userId = userId;
            this.currentFullName = fullName;
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
        }

        private void FormEditProfil_Load(object sender, EventArgs e)
        {
            txtFullName.Text = currentFullName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newFullName = txtFullName.Text.Trim();
            string newPassword = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (newFullName == "")
            {
                MessageBox.Show("Nama lengkap tidak boleh kosong!");
                return;
            }

            if (newPassword != "" && newPassword != confirmPassword)
            {
                MessageBox.Show("Password dan konfirmasi password tidak cocok!");
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUser", userId);
                cmd.Parameters.AddWithValue("@fullName", newFullName);
                cmd.Parameters.AddWithValue("@password", newPassword == "" ? currentFullName : newPassword);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show(dr["message"].ToString());
                }
                dr.Close();
                con.Close();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}