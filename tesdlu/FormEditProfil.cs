using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormEditProfil : Form
    {
        private int userId;
        private string currentFullName;
        private SqlConnection con;
        private bool isUpdatingPasswordField = false;

        public FormEditProfil(int userId, string fullName)
        {
            InitializeComponent();

            this.userId = userId;
            this.currentFullName = fullName;

            con = new SqlConnection(
                @"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");

            // Placeholder events
            txtPassword.GotFocus += TxtPassword_GotFocus;
            txtPassword.LostFocus += TxtPassword_LostFocus;

            txtConfirmPassword.GotFocus += TxtConfirmPassword_GotFocus;
            txtConfirmPassword.LostFocus += TxtConfirmPassword_LostFocus;

            // Nama hanya huruf dan spasi
            txtFullName.KeyPress += TxtFullName_KeyPress;
        }

        private void FormEditProfil_Load(object sender, EventArgs e)
        {
            txtFullName.Text = currentFullName;

            txtPassword.Text = "Password";
            txtPassword.ForeColor = System.Drawing.Color.Gray;
            txtPassword.UseSystemPasswordChar = false;

            txtConfirmPassword.Text = "Confirm Password";
            txtConfirmPassword.ForeColor = System.Drawing.Color.Gray;
            txtConfirmPassword.UseSystemPasswordChar = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newFullName = txtFullName.Text.Trim();
            string newPassword = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validasi nama kosong
            if (string.IsNullOrWhiteSpace(newFullName))
            {
                MessageBox.Show(
                    "Nama lengkap tidak boleh kosong!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Validasi nama hanya huruf dan spasi
            if (!Regex.IsMatch(newFullName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show(
                    "Nama lengkap hanya boleh berisi huruf dan spasi!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (newPassword == "Password" ||
                newPassword == "Confirm Password" ||
                newPassword == "")
            {
                newPassword = "";
            }

            if (confirmPassword == "Password" ||
                confirmPassword == "Confirm Password" ||
                confirmPassword == "")
            {
                confirmPassword = "";
            }

            // Validasi password cocok
            if (newPassword != "" &&
                newPassword != confirmPassword)
            {
                MessageBox.Show(
                    "Password dan konfirmasi password tidak cocok!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                con.Open();

                SqlCommand cmd =
                    new SqlCommand("sp_UpdateProfile", con);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUser", userId);
                cmd.Parameters.AddWithValue("@fullName", newFullName);

                // Jika password kosong, tidak diubah
                string passwordToStore =
                    string.IsNullOrEmpty(newPassword)
                    ? ""
                    : BCrypt.Net.BCrypt.HashPassword(newPassword);

                cmd.Parameters.AddWithValue("@password", passwordToStore);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    MessageBox.Show(
                        dr["message"].ToString(),
                        "Informasi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                dr.Close();
                con.Close();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Nama hanya huruf dan spasi
        private void TxtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void TxtPassword_GotFocus(object sender, EventArgs e)
        {
            if (isUpdatingPasswordField)
                return;

            isUpdatingPasswordField = true;

            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = System.Drawing.Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }

            isUpdatingPasswordField = false;
        }

        private void TxtPassword_LostFocus(object sender, EventArgs e)
        {
            if (isUpdatingPasswordField)
                return;

            isUpdatingPasswordField = true;

            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = System.Drawing.Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }

            isUpdatingPasswordField = false;
        }

        private void TxtConfirmPassword_GotFocus(object sender, EventArgs e)
        {
            if (isUpdatingPasswordField)
                return;

            isUpdatingPasswordField = true;

            if (txtConfirmPassword.Text == "Confirm Password")
            {
                txtConfirmPassword.Text = "";
                txtConfirmPassword.ForeColor = System.Drawing.Color.Black;
                txtConfirmPassword.UseSystemPasswordChar = true;
            }

            isUpdatingPasswordField = false;
        }

        private void TxtConfirmPassword_LostFocus(object sender, EventArgs e)
        {
            if (isUpdatingPasswordField)
                return;

            isUpdatingPasswordField = true;

            if (txtConfirmPassword.Text == "")
            {
                txtConfirmPassword.Text = "Confirm Password";
                txtConfirmPassword.ForeColor = System.Drawing.Color.Gray;
                txtConfirmPassword.UseSystemPasswordChar = false;
            }

            isUpdatingPasswordField = false;
        }
    }
}