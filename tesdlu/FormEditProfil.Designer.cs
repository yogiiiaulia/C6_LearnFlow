using System;
using System.Drawing;
using System.Windows.Forms;

namespace tesdlu
{
    partial class FormEditProfil
    {
        private TextBox txtFullName, txtPassword, txtConfirmPassword;
        private Button btnSave, btnCancel;
        private Label lblTitle, lblFullName, lblPassword, lblConfirm;

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.txtFullName = new TextBox();
            this.txtPassword = new TextBox();
            this.txtConfirmPassword = new TextBox();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.lblFullName = new Label();
            this.lblPassword = new Label();
            this.lblConfirm = new Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.Location = new Point(50, 30);
            this.lblTitle.Text = "Edit Profil";

            // lblFullName
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new Point(50, 100);
            this.lblFullName.Text = "Nama Lengkap:";

            // txtFullName
            this.txtFullName.Location = new Point(50, 130);
            this.txtFullName.Size = new Size(300, 27);

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(50, 180);
            this.lblPassword.Text = "Password Baru:";

            // txtPassword
            this.txtPassword.Location = new Point(50, 210);
            this.txtPassword.Size = new Size(300, 27);
            this.txtPassword.UseSystemPasswordChar = true;

            // lblConfirm
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new Point(50, 260);
            this.lblConfirm.Text = "Konfirmasi Password:";

            // txtConfirmPassword
            this.txtConfirmPassword.Location = new Point(50, 290);
            this.txtConfirmPassword.Size = new Size(300, 27);
            this.txtConfirmPassword.UseSystemPasswordChar = true;

            // btnSave
            this.btnSave.Location = new Point(50, 350);
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.Text = "Simpan";
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new Point(170, 350);
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Text = "Batal";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // FormEditProfil
            this.ClientSize = new Size(450, 450);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblConfirm);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Text = "Edit Profil";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(this.FormEditProfil_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}