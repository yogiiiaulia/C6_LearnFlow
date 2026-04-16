namespace tesdlu
{
    partial class FormInputGrade
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearchStudent;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.cmbCourse = new System.Windows.Forms.ComboBox();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearchStudent = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(146, 32);
            this.lblTitle.Text = "Input Nilai";

            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.Location = new System.Drawing.Point(20, 70);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(350, 23);

            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 115);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(102, 15);
            this.lblSearch.Text = "Cari Mahasiswa:";

            this.txtSearchStudent.Location = new System.Drawing.Point(130, 112);
            this.txtSearchStudent.Name = "txtSearchStudent";
            this.txtSearchStudent.Size = new System.Drawing.Size(180, 23);

            this.btnSearch.Location = new System.Drawing.Point(320, 110);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 27);
            this.btnSearch.Text = "Cari";
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.UseVisualStyleBackColor = false;

            this.btnRefresh.Location = new System.Drawing.Point(410, 110);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 27);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.UseVisualStyleBackColor = false;

            this.dgvStudents.Location = new System.Drawing.Point(20, 155);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.Size = new System.Drawing.Size(760, 300);
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.btnSave.Location = new System.Drawing.Point(680, 470);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Simpan Nilai";
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.UseVisualStyleBackColor = false;

            this.ClientSize = new System.Drawing.Size(800, 530);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmbCourse);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearchStudent);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvStudents);
            this.Controls.Add(this.btnSave);
            this.Name = "FormInputGrade";
            this.Text = "Input Nilai";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}