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
            if (txtUsername.Text == "" || txtFullName.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Isi semua field!");
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users (username, password, fullName, role) VALUES (@u, @p, @f, @r)", con);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", txtPassword.Text);
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
                SqlCommand cmd = new SqlCommand("UPDATE Users SET username=@u, fullName=@f, role=@r, password=@p WHERE idUser=@id", con);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", txtPassword.Text);
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
                txtFullName.Text = row.Cells["fullName"].Value.ToString();
                cmbRole.Text = row.Cells["role"].Value.ToString();
                txtPassword.Text = "";
            }
        }

        private void ClearForm()
        {
            selectedUserId = -1;
            txtUsername.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            cmbRole.Text = "Student";
        }

        private void FormUsers_Load_1(object sender, EventArgs e)
        {

        }
    }
}