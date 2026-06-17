using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace tesdlu
{
    public partial class FormUsers : Form
    {
        private SqlConnection con;
        private int selectedUserId = -1;
        private bool isUpdatingPasswordField = false;

        // SYARAT POIN 4: BindingSource
        private BindingSource bsUsers = new BindingSource();

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
            txtFullName.KeyPress += TxtFullName_KeyPress;
        }

        private void FormUsers_Load(object sender, EventArgs e)
        {
            ClearForm();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                con.Open();
                // SYARAT POIN 2: View
                SqlCommand cmd = new SqlCommand("SELECT idUser, username, fullName, role FROM vw_ManageUsers", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // SYARAT POIN 4: BindingSource
                bsUsers.DataSource = dt;
                dgvUsers.DataSource = bsUsers;
                bindingNavigator1.BindingSource = bsUsers;

                con.Close();

                if (dgvUsers.Columns.Contains("idUser")) dgvUsers.Columns["idUser"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
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

                 if (!Regex.IsMatch(txtFullName.Text.Trim(), @"^[a-zA-Z\s\.]+$"))
                 {
                    MessageBox.Show("Full Name hanya boleh berisi huruf, spasi, dan titik (.)!");
                    return;
                 }

            if (!Regex.IsMatch(txtUsername.Text.Trim(), @"^[a-zA-Z0-9_]+$"))
                {
                    MessageBox.Show("Username hanya boleh huruf, angka, dan underscore (_)");
                    return;
                }

                if (txtPassword.Text.Length < 8)
                {
                    MessageBox.Show("Password minimal 8 karakter!");
                    return;
                }

                try
            {
                con.Open();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);

                // SYARAT POIN 1: SP
                SqlCommand cmd = new SqlCommand("sp_InsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@fullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@role", cmbRole.Text);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int result = Convert.ToInt32(dr["result"]);
                    MessageBox.Show(dr["message"].ToString());

                    if (result == 1)
                    {
                        ClearForm();
                    }
                }
                dr.Close();
                con.Close();

                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Pilih user yang akan diupdate!");
                return;
            }

            if (txtUsername.Text == "" || txtUsername.Text == "Username" ||
                txtFullName.Text == "" || txtFullName.Text == "Full Name")
            {
                MessageBox.Show("Username dan Full Name tidak boleh kosong!");
                return;
            }

            if (!Regex.IsMatch(txtFullName.Text.Trim(), @"^[a-zA-Z\s\.]+$"))
            {
                MessageBox.Show("Full Name hanya boleh berisi huruf, spasi, dan titik (.)!");
                return;
            }

            if (!Regex.IsMatch(txtUsername.Text.Trim(), @"^[a-zA-Z0-9_]+$"))
            {
                MessageBox.Show("Username hanya boleh huruf, angka, dan underscore (_)");
                return;
            }

            if (txtPassword.Text != "" &&
                txtPassword.Text != "Password" &&
                txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Password minimal 8 karakter!");
                return;
            }

            try
            {
                con.Open();

                // Jika password kosong, kirim string kosong. SP akan mengabaikan update password.
                string hashedPassword = "";
                if (txtPassword.Text != "" && txtPassword.Text != "Password")
                {
                    hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                }

                // SYARAT POIN 1: SP
                SqlCommand cmd = new SqlCommand("sp_UpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUser", selectedUserId);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@fullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@role", cmbRole.Text);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show(dr["message"].ToString());
                    int result = Convert.ToInt32(dr["result"]);
                    if (result == 1)
                    {
                        ClearForm();
                    }
                }
                dr.Close();
                con.Close();

                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
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
                    // SYARAT POIN 1: SP
                    SqlCommand cmd = new SqlCommand("sp_DeleteUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUser", selectedUserId);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show(dr["message"].ToString());
                    }
                    dr.Close();
                    con.Close();

                    ClearForm();
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    if (con.State == ConnectionState.Open) con.Close();
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
            cmbRole.SelectedIndex = -1;
            if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = 0; // Set default ke item pertama
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

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void TxtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }
}