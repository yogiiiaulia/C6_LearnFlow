using System;
using System.Data;
using System.Data.SqlClient;
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

            // Pasang event klik untuk tombol
            btnRegister.Click += BtnRegister_Click;
            btnBackToLogin.Click += BtnBackToLogin_Click;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // 1. Ambil input
            string fullName = txtFullName.Text.Trim();
            string nim = txtNim.Text.Trim();
            string prodi = txtProdi.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // 2. Validasi Input Kosong
            if (fullName == "" || nim == "" || prodi == "" || username == "" || password == "")
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validasi kecocokan password
            if (password != confirmPassword)
            {
                MessageBox.Show("Password dan Konfirmasi Password tidak cocok!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Proses Insert ke Database
            try
            {
                con.Open();

                // Hash Password dengan BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // Menggunakan Stored Procedure (Syarat Ujian Poin 1)
                SqlCommand cmd = new SqlCommand("sp_Register", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@fullName", fullName);
                cmd.Parameters.AddWithValue("@nim", nim);
                cmd.Parameters.AddWithValue("@prodi", prodi);
                // Angkatan sudah dihapus

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int result = Convert.ToInt32(dr["result"]);
                    string message = dr["message"].ToString();

                    if (result == 1) // Jika Berhasil
                    {
                        MessageBox.Show(message, "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tutup form register, tampilkan form login kembali
                        FormLogin loginForm = new FormLogin();
                        loginForm.Show();
                        this.Hide();
                    }
                    else // Jika Username/NIM sudah ada atau error
                    {
                        MessageBox.Show(message, "Gagal Registrasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnBackToLogin_Click(object sender, EventArgs e)
        {
            // Kembali ke halaman login tanpa memproses apa-apa
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Hide();
        }
    }
}