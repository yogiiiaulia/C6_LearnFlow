using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tesdlu
{
    public partial class FormUsers : Form
    {
        private SqlConnection con;
        private int selectedUserId = -1;
        private bool isUpdatingPasswordField = false;

        public FormUsers()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=LAPTOP-IUIDNP6D\YOGI;Initial Catalog=DBlearnFlow;Integrated Security=True");
            this.Load += FormUsers_Load;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;
            dgvUsers.CellClick += DgvUsers_CellClick;

            // Placeholder events
            txtUsername.GotFocus += TxtUsername_GotFocus;
            txtUsername.LostFocus += TxtUsername_LostFocus;
            txtFullName.GotFocus += TxtFullName_GotFocus;
            txtFullName.LostFocus += TxtFullName_LostFocus;
            txtPassword.GotFocus += TxtPassword_GotFocus;
            txtPassword.LostFocus += TxtPassword_LostFocus;
        }

        private void FormUsers_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT idUser, username, fullName, role FROM Users", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUsers.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtUsername.Text == "Username" || 
                txtFullName.Text == "" || txtFullName.Text == "Full Name" || 
                txtPassword.Text == "" || txtPassword.Text == "Password")
            {
                MessageBox.Show("Isi semua field!");
                return;
            }

            try
            {
                con.Open();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (username, password, fullName, role) VALUES (@u, @p, @f, @r)", con);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", hashedPassword);
                cmd.Parameters.AddWithValue("@f", txtFullName.Text);
                cmd.Parameters.AddWithValue("@r", cmbRole.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("User berhasil ditambahkan!");
                ClearForm();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Pilih user yang akan diupdate!");
                return;
            }

            try
            {
                con.Open();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                SqlCommand cmd = new SqlCommand("UPDATE Users SET username=@u, fullName=@f, role=@r, password=@p WHERE idUser=@id", con);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", hashedPassword);
                cmd.Parameters.AddWithValue("@f", txtFullName.Text);
                cmd.Parameters.AddWithValue("@r", cmbRole.Text);
                cmd.Parameters.AddWithValue("@id", selectedUserId);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("User berhasil diupdate!");
                ClearForm();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                con.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Pilih user yang akan dihapus!");
                return;
            }

            DialogResult result = MessageBox.Show("Yakin hapus user ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE idUser=@id", con);
                    cmd.Parameters.AddWithValue("@id", selectedUserId);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("User berhasil dihapus!");
                    ClearForm();
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    con.Close();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
            ClearForm();
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUserId = Convert.ToInt32(row.Cells["idUser"].Value);
                txtUsername.Text = row.Cells["username"].Value.ToString();
                txtUsername.ForeColor = System.Drawing.Color.Black;
                txtFullName.Text = row.Cells["fullName"].Value.ToString();
                txtFullName.ForeColor = System.Drawing.Color.Black;
                cmbRole.Text = row.Cells["role"].Value.ToString();
                txtPassword.Text = "";
                txtPassword.ForeColor = System.Drawing.Color.Black;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void ClearForm()
        {
            selectedUserId = -1;
            txtUsername.Text = "Username";
            txtUsername.ForeColor = System.Drawing.Color.Gray;
            txtFullName.Text = "Full Name";
            txtFullName.ForeColor = System.Drawing.Color.Gray;
            txtPassword.Text = "Password";
            txtPassword.ForeColor = System.Drawing.Color.Gray;
            txtPassword.UseSystemPasswordChar = false;
            cmbRole.Text = "Student";
        }

        private void TxtUsername_GotFocus(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtUsername_LostFocus(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Text = "Username";
                txtUsername.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void TxtFullName_GotFocus(object sender, EventArgs e)
        {
            if (txtFullName.Text == "Full Name")
            {
                txtFullName.Text = "";
                txtFullName.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TxtFullName_LostFocus(object sender, EventArgs e)
        {
            if (txtFullName.Text == "")
            {
                txtFullName.Text = "Full Name";
                txtFullName.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void TxtPassword_GotFocus(object sender, EventArgs e)
        {
            if (isUpdatingPasswordField) return;
            
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
            if (isUpdatingPasswordField) return;

            isUpdatingPasswordField = true;
            if (txtPassword.Text == "" && selectedUserId == -1)
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = System.Drawing.Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
            else if (txtPassword.Text != "" && txtPassword.Text != "Password")
            {
                txtPassword.ForeColor = System.Drawing.Color.Black;
            }
            isUpdatingPasswordField = false;
        }
    }
}