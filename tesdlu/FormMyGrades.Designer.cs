namespace tesdlu
{
    partial class FormMyGrades
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvGrades;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvGrades = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(118, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Nilai Saya";

            // dgvGrades
            this.dgvGrades.Location = new System.Drawing.Point(20, 70);
            this.dgvGrades.Name = "dgvGrades";
            this.dgvGrades.Size = new System.Drawing.Size(760, 400);
            this.dgvGrades.TabIndex = 1;
            this.dgvGrades.AllowUserToAddRows = false;
            this.dgvGrades.ReadOnly = true;
            this.dgvGrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // FormMyGrades
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvGrades);
            this.Name = "FormMyGrades";
            this.Text = "Lihat Nilai";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}