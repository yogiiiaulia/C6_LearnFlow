namespace tesdlu
{
    partial class FormEnroll
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Button btnEnroll;
        private System.Windows.Forms.DataGridView dgvMyCourses;

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
            this.btnEnroll = new System.Windows.Forms.Button();
            this.dgvMyCourses = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyCourses)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(210, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Daftar Kursus";
            // 
            // cmbCourse
            // 
            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.Location = new System.Drawing.Point(20, 70);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(400, 24);
            this.cmbCourse.TabIndex = 1;
            // 
            // btnEnroll
            // 
            this.btnEnroll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnEnroll.ForeColor = System.Drawing.Color.White;
            this.btnEnroll.Location = new System.Drawing.Point(440, 68);
            this.btnEnroll.Name = "btnEnroll";
            this.btnEnroll.Size = new System.Drawing.Size(100, 30);
            this.btnEnroll.TabIndex = 2;
            this.btnEnroll.Text = "Daftar";
            this.btnEnroll.UseVisualStyleBackColor = false;
            // 
            // dgvMyCourses
            // 
            this.dgvMyCourses.AllowUserToAddRows = false;
            this.dgvMyCourses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMyCourses.ColumnHeadersHeight = 29;
            this.dgvMyCourses.Location = new System.Drawing.Point(20, 120);
            this.dgvMyCourses.Name = "dgvMyCourses";
            this.dgvMyCourses.ReadOnly = true;
            this.dgvMyCourses.RowHeadersWidth = 51;
            this.dgvMyCourses.Size = new System.Drawing.Size(760, 350);
            this.dgvMyCourses.TabIndex = 3;
            // 
            // FormEnroll
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmbCourse);
            this.Controls.Add(this.btnEnroll);
            this.Controls.Add(this.dgvMyCourses);
            this.Name = "FormEnroll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daftar Kursus";
            this.Load += new System.EventHandler(this.FormEnroll_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyCourses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}