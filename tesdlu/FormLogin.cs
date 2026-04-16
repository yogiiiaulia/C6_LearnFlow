using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace tesdlu
{
    public partial class FormLogin : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
        public FormLogin()
        {

            InitializeComponent();
            TestConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbpassword.UseSystemPasswordChar = true;
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void TestConnection()
        {
            try
            {
                con.Open();
                MessageBox.Show("Koneksi berhasil!");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtusername_Click(object sender, EventArgs e)
        {

        }

        private void txtpassword_Click(object sender, EventArgs e)
        {

        }

        private void tbpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbshowpassword.Checked)
            {
                tbpassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbpassword.UseSystemPasswordChar = true;
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string username = tbusername.Text.Trim();
            string password = tbpassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Username dan Password tidak boleh kosong!");
                return;
            }

            try
            {
                con.Open();

                // Ambil juga idUser
                SqlCommand cmd = new SqlCommand(@"
            SELECT idUser, fullName, role 
            FROM Users 
            WHERE username = @username AND password = @password", con);

                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Ambil userId sebagai int
                    int userId = Convert.ToInt32(dr["idUser"]);
                    string nama = dr["fullName"].ToString();
                    string role = dr["role"].ToString();

                    MessageBox.Show("Login BERHASIL!\nSelamat datang " + nama);

                    // Kirim userId (int), bukan username (string)
                    FormDashboard dashboard = new FormDashboard(userId, nama, role);
                    dashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username atau Password salah!");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }




        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
