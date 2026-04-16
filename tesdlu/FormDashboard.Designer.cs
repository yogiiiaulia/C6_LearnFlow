namespace tesdlu
{
    partial class FormDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Button btnEnrollment;
        private System.Windows.Forms.Button btnNilai;
        private System.Windows.Forms.Button btnMaterials;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnEditProfil;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Label lblWelcomeText;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label lblTotalUser;
        private System.Windows.Forms.Label lblTotalCourse;
        private System.Windows.Forms.Label lblTotalEnrollment;
        private System.Windows.Forms.Label lblTotalInstructor;
        private System.Windows.Forms.Label lblTotalStudent;
        private System.Windows.Forms.Label lblStatsTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnEditProfil = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnMaterials = new System.Windows.Forms.Button();
            this.btnNilai = new System.Windows.Forms.Button();
            this.btnEnrollment = new System.Windows.Forms.Button();
            this.btnCourses = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelStats = new System.Windows.Forms.Panel();
            this.lblTotalStudent = new System.Windows.Forms.Label();
            this.lblTotalInstructor = new System.Windows.Forms.Label();
            this.lblTotalEnrollment = new System.Windows.Forms.Label();
            this.lblTotalCourse = new System.Windows.Forms.Label();
            this.lblTotalUser = new System.Windows.Forms.Label();
            this.lblStatsTitle = new System.Windows.Forms.Label();
            this.lblUserText = new System.Windows.Forms.Label();
            this.lblCourseText = new System.Windows.Forms.Label();
            this.lblEnrollText = new System.Windows.Forms.Label();
            this.lblInstructorText = new System.Windows.Forms.Label();
            this.lblStudentText = new System.Windows.Forms.Label();
            this.lblDashboardTitle = new System.Windows.Forms.Label();
            this.lblWelcomeText = new System.Windows.Forms.Label();
            this.panelSidebar.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.panelSidebar.Controls.Add(this.btnEditProfil);
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Controls.Add(this.btnMaterials);
            this.panelSidebar.Controls.Add(this.btnNilai);
            this.panelSidebar.Controls.Add(this.btnEnrollment);
            this.panelSidebar.Controls.Add(this.btnCourses);
            this.panelSidebar.Controls.Add(this.btnUsers);
            this.panelSidebar.Controls.Add(this.lblTitle);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(251, 640);
            this.panelSidebar.TabIndex = 0;
            // 
            // btnEditProfil
            // 
            this.btnEditProfil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnEditProfil.FlatAppearance.BorderSize = 0;
            this.btnEditProfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditProfil.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEditProfil.ForeColor = System.Drawing.Color.White;
            this.btnEditProfil.Location = new System.Drawing.Point(0, 363);
            this.btnEditProfil.Name = "btnEditProfil";
            this.btnEditProfil.Size = new System.Drawing.Size(251, 43);
            this.btnEditProfil.TabIndex = 6;
            this.btnEditProfil.Text = "✏️ Edit Profil";
            this.btnEditProfil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditProfil.UseVisualStyleBackColor = false;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.btnLogout.Location = new System.Drawing.Point(0, 533);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(251, 43);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "🚪 Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // btnMaterials
            // 
            this.btnMaterials.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnMaterials.FlatAppearance.BorderSize = 0;
            this.btnMaterials.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaterials.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnMaterials.ForeColor = System.Drawing.Color.White;
            this.btnMaterials.Location = new System.Drawing.Point(0, 299);
            this.btnMaterials.Name = "btnMaterials";
            this.btnMaterials.Size = new System.Drawing.Size(251, 43);
            this.btnMaterials.TabIndex = 5;
            this.btnMaterials.Text = "📄 Materials";
            this.btnMaterials.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaterials.UseVisualStyleBackColor = false;
            // 
            // btnNilai
            // 
            this.btnNilai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnNilai.FlatAppearance.BorderSize = 0;
            this.btnNilai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNilai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNilai.ForeColor = System.Drawing.Color.White;
            this.btnNilai.Location = new System.Drawing.Point(0, 245);
            this.btnNilai.Name = "btnNilai";
            this.btnNilai.Size = new System.Drawing.Size(251, 43);
            this.btnNilai.TabIndex = 4;
            this.btnNilai.Text = "⭐ Nilai";
            this.btnNilai.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNilai.UseVisualStyleBackColor = false;
            // 
            // btnEnrollment
            // 
            this.btnEnrollment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnEnrollment.FlatAppearance.BorderSize = 0;
            this.btnEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnrollment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEnrollment.ForeColor = System.Drawing.Color.White;
            this.btnEnrollment.Location = new System.Drawing.Point(0, 192);
            this.btnEnrollment.Name = "btnEnrollment";
            this.btnEnrollment.Size = new System.Drawing.Size(251, 43);
            this.btnEnrollment.TabIndex = 3;
            this.btnEnrollment.Text = "📝 Enrollment";
            this.btnEnrollment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnrollment.UseVisualStyleBackColor = false;
            // 
            // btnCourses
            // 
            this.btnCourses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnCourses.FlatAppearance.BorderSize = 0;
            this.btnCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCourses.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCourses.ForeColor = System.Drawing.Color.White;
            this.btnCourses.Location = new System.Drawing.Point(0, 139);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(251, 43);
            this.btnCourses.TabIndex = 2;
            this.btnCourses.Text = "📚 Courses";
            this.btnCourses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCourses.UseVisualStyleBackColor = false;
            // 
            // btnUsers
            // 
            this.btnUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.btnUsers.FlatAppearance.BorderSize = 0;
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnUsers.ForeColor = System.Drawing.Color.White;
            this.btnUsers.Location = new System.Drawing.Point(0, 85);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(251, 43);
            this.btnUsers.TabIndex = 1;
            this.btnUsers.Text = "👥 Users";
            this.btnUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsers.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(57, 32);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(162, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "LearnFlow";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.lblWelcome);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(251, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(892, 53);
            this.panelHeader.TabIndex = 1;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.lblWelcome.Location = new System.Drawing.Point(23, 16);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(130, 23);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Selamat datang";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panelContent.Controls.Add(this.panelStats);
            this.panelContent.Controls.Add(this.lblDashboardTitle);
            this.panelContent.Controls.Add(this.lblWelcomeText);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(251, 53);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(892, 587);
            this.panelContent.TabIndex = 2;
            this.panelContent.Paint += new System.Windows.Forms.PaintEventHandler(this.panelContent_Paint);
            // 
            // panelStats
            // 
            this.panelStats.BackColor = System.Drawing.Color.White;
            this.panelStats.Controls.Add(this.lblTotalStudent);
            this.panelStats.Controls.Add(this.lblTotalInstructor);
            this.panelStats.Controls.Add(this.lblTotalEnrollment);
            this.panelStats.Controls.Add(this.lblTotalCourse);
            this.panelStats.Controls.Add(this.lblTotalUser);
            this.panelStats.Controls.Add(this.lblStatsTitle);
            this.panelStats.Controls.Add(this.lblUserText);
            this.panelStats.Controls.Add(this.lblCourseText);
            this.panelStats.Controls.Add(this.lblEnrollText);
            this.panelStats.Controls.Add(this.lblInstructorText);
            this.panelStats.Controls.Add(this.lblStudentText);
            this.panelStats.Location = new System.Drawing.Point(34, 117);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(823, 213);
            this.panelStats.TabIndex = 2;
            // 
            // lblTotalStudent
            // 
            this.lblTotalStudent.AutoSize = true;
            this.lblTotalStudent.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalStudent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.lblTotalStudent.Location = new System.Drawing.Point(206, 149);
            this.lblTotalStudent.Name = "lblTotalStudent";
            this.lblTotalStudent.Size = new System.Drawing.Size(28, 32);
            this.lblTotalStudent.TabIndex = 5;
            this.lblTotalStudent.Text = "0";
            // 
            // lblTotalInstructor
            // 
            this.lblTotalInstructor.AutoSize = true;
            this.lblTotalInstructor.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalInstructor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lblTotalInstructor.Location = new System.Drawing.Point(46, 149);
            this.lblTotalInstructor.Name = "lblTotalInstructor";
            this.lblTotalInstructor.Size = new System.Drawing.Size(28, 32);
            this.lblTotalInstructor.TabIndex = 4;
            this.lblTotalInstructor.Text = "0";
            // 
            // lblTotalEnrollment
            // 
            this.lblTotalEnrollment.AutoSize = true;
            this.lblTotalEnrollment.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalEnrollment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.lblTotalEnrollment.Location = new System.Drawing.Point(377, 64);
            this.lblTotalEnrollment.Name = "lblTotalEnrollment";
            this.lblTotalEnrollment.Size = new System.Drawing.Size(35, 41);
            this.lblTotalEnrollment.TabIndex = 3;
            this.lblTotalEnrollment.Text = "0";
            // 
            // lblTotalCourse
            // 
            this.lblTotalCourse.AutoSize = true;
            this.lblTotalCourse.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalCourse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblTotalCourse.Location = new System.Drawing.Point(206, 64);
            this.lblTotalCourse.Name = "lblTotalCourse";
            this.lblTotalCourse.Size = new System.Drawing.Size(35, 41);
            this.lblTotalCourse.TabIndex = 2;
            this.lblTotalCourse.Text = "0";
            // 
            // lblTotalUser
            // 
            this.lblTotalUser.AutoSize = true;
            this.lblTotalUser.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblTotalUser.Location = new System.Drawing.Point(46, 64);
            this.lblTotalUser.Name = "lblTotalUser";
            this.lblTotalUser.Size = new System.Drawing.Size(35, 41);
            this.lblTotalUser.TabIndex = 1;
            this.lblTotalUser.Text = "0";
            // 
            // lblStatsTitle
            // 
            this.lblStatsTitle.AutoSize = true;
            this.lblStatsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblStatsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.lblStatsTitle.Location = new System.Drawing.Point(23, 16);
            this.lblStatsTitle.Name = "lblStatsTitle";
            this.lblStatsTitle.Size = new System.Drawing.Size(192, 32);
            this.lblStatsTitle.TabIndex = 0;
            this.lblStatsTitle.Text = "Ringkasan Data";
            // 
            // lblUserText
            // 
            this.lblUserText.AutoSize = true;
            this.lblUserText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUserText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblUserText.Location = new System.Drawing.Point(46, 101);
            this.lblUserText.Name = "lblUserText";
            this.lblUserText.Size = new System.Drawing.Size(85, 23);
            this.lblUserText.TabIndex = 6;
            this.lblUserText.Text = "Total User";
            // 
            // lblCourseText
            // 
            this.lblCourseText.AutoSize = true;
            this.lblCourseText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCourseText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblCourseText.Location = new System.Drawing.Point(206, 101);
            this.lblCourseText.Name = "lblCourseText";
            this.lblCourseText.Size = new System.Drawing.Size(104, 23);
            this.lblCourseText.TabIndex = 7;
            this.lblCourseText.Text = "Total Course";
            // 
            // lblEnrollText
            // 
            this.lblEnrollText.AutoSize = true;
            this.lblEnrollText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEnrollText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblEnrollText.Location = new System.Drawing.Point(377, 101);
            this.lblEnrollText.Name = "lblEnrollText";
            this.lblEnrollText.Size = new System.Drawing.Size(134, 23);
            this.lblEnrollText.TabIndex = 8;
            this.lblEnrollText.Text = "Total Enrollment";
            // 
            // lblInstructorText
            // 
            this.lblInstructorText.AutoSize = true;
            this.lblInstructorText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInstructorText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInstructorText.Location = new System.Drawing.Point(46, 179);
            this.lblInstructorText.Name = "lblInstructorText";
            this.lblInstructorText.Size = new System.Drawing.Size(91, 23);
            this.lblInstructorText.TabIndex = 9;
            this.lblInstructorText.Text = "Instructors";
            // 
            // lblStudentText
            // 
            this.lblStudentText.AutoSize = true;
            this.lblStudentText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStudentText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblStudentText.Location = new System.Drawing.Point(206, 179);
            this.lblStudentText.Name = "lblStudentText";
            this.lblStudentText.Size = new System.Drawing.Size(76, 23);
            this.lblStudentText.TabIndex = 10;
            this.lblStudentText.Text = "Students";
            // 
            // lblDashboardTitle
            // 
            this.lblDashboardTitle.AutoSize = true;
            this.lblDashboardTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblDashboardTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(42)))), ((int)(((byte)(68)))));
            this.lblDashboardTitle.Location = new System.Drawing.Point(23, 21);
            this.lblDashboardTitle.Name = "lblDashboardTitle";
            this.lblDashboardTitle.Size = new System.Drawing.Size(193, 46);
            this.lblDashboardTitle.TabIndex = 0;
            this.lblDashboardTitle.Text = "Dashboard";
            // 
            // lblWelcomeText
            // 
            this.lblWelcomeText.AutoSize = true;
            this.lblWelcomeText.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblWelcomeText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblWelcomeText.Location = new System.Drawing.Point(29, 69);
            this.lblWelcomeText.Name = "lblWelcomeText";
            this.lblWelcomeText.Size = new System.Drawing.Size(645, 25);
            this.lblWelcomeText.TabIndex = 1;
            this.lblWelcomeText.Text = "Selamat Datang di LearnFlow - Sistem manajemen belajar untuk universitas";
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 640);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.Name = "FormDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LearnFlow - Dashboard";
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelStats.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblUserText;
        private System.Windows.Forms.Label lblCourseText;
        private System.Windows.Forms.Label lblEnrollText;
        private System.Windows.Forms.Label lblInstructorText;
        private System.Windows.Forms.Label lblStudentText;
    }
}