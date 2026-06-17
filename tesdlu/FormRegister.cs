using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormRegister : Form
    {
        private SqlConnection con;

        public FormRegister()
        {
            InitializeComponent();

            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");

            // Event tombol
            btnRegister.Click += BtnRegister_Click;
            btnBackToLogin.Click += BtnBackToLogin_Click;

            // Validasi input saat mengetik
            txtFullName.KeyPress += TxtHurufOnly_KeyPress;
            txtProdi.KeyPress += TxtHurufOnly_KeyPress;
            txtNim.KeyPress += TxtNim_KeyPress;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string nim = txtNim.Text.Trim();
            string prodi = txtProdi.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validasi kosong
            if (fullName == "" || nim == "" || prodi == "" || username == "" || password == "")
            {
                MessageBox.Show(
                    "Semua kolom harus diisi!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Nama hanya huruf dan spasi
            if (!Regex.IsMatch(fullName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show(
                    "Nama Lengkap hanya boleh berisi huruf dan spasi!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // NIM hanya angka
            if (!Regex.IsMatch(nim, @"^\d+$"))
            {
                MessageBox.Show(
                    "NIM hanya boleh berisi angka!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Prodi hanya huruf dan spasi
            if (!Regex.IsMatch(prodi, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show(
                    "Program Studi hanya boleh berisi huruf dan spasi!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Username hanya huruf, angka, underscore
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                MessageBox.Show(
                    "Username hanya boleh berisi huruf, angka, dan underscore (_)!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Password minimal 8 karakter
            if (password.Length < 8)
            {
                MessageBox.Show(
                    "Password minimal 8 karakter!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi password
            if (password != confirmPassword)
            {
                MessageBox.Show(
                    "Password dan Konfirmasi Password tidak cocok!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();

                string hashedPassword =
                    BCrypt.Net.BCrypt.HashPassword(password);

                SqlCommand cmd =
                    new SqlCommand("sp_Register", con);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@fullName", fullName);
                cmd.Parameters.AddWithValue("@nim", nim);
                cmd.Parameters.AddWithValue("@prodi", prodi);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    int result =
                        Convert.ToInt32(dr["result"]);

                    string message =
                        dr["message"].ToString();

                    if (result == 1)
                    {
                        MessageBox.Show(
                            message,
                            "Sukses",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        FormLogin loginForm =
                            new FormLogin();

                        loginForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show(
                            message,
                            "Gagal Registrasi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }

                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error Database: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        // Nama & Prodi hanya huruf dan spasi
        private void TxtHurufOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        // NIM hanya angka
        private void TxtNim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Hide();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {

        }
    }
}