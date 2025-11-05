namespace Prana.Admin.Controls.Company
{
    partial class UserSetupDetails
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAllGrpAccess = new System.Windows.Forms.CheckBox();
            this.grpUserType = new System.Windows.Forms.GroupBox();
            this.rdbPowerUser = new System.Windows.Forms.RadioButton();
            this.rdbSysAdmin = new System.Windows.Forms.RadioButton();
            this.rdbAdministrator = new System.Windows.Forms.RadioButton();
            this.rdbUser = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblShortName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRegion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.txtLoginName = new System.Windows.Forms.TextBox();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEMail = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.appStylistRuntime2 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.groupBox1.SuspendLayout();
            this.grpUserType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkAllGrpAccess);
            this.groupBox1.Controls.Add(this.grpUserType);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblShortName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblRegion);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.txtRegion);
            this.groupBox1.Controls.Add(this.txtLoginName);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtEMail);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.MinimumSize = new System.Drawing.Size(589, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(653, 200);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Detail";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(388, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 8);
            this.label6.TabIndex = 61;
            this.label6.Text = "*";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(89, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 8);
            this.label5.TabIndex = 60;
            this.label5.Text = "*";
            // 
            // chkAllGrpAccess
            // 
            this.chkAllGrpAccess.AutoSize = true;
            this.chkAllGrpAccess.Location = new System.Drawing.Point(299, 129);
            this.chkAllGrpAccess.Name = "chkAllGrpAccess";
            this.chkAllGrpAccess.Size = new System.Drawing.Size(281, 17);
            this.chkAllGrpAccess.TabIndex = 59;
            this.chkAllGrpAccess.Text = "Enable access to all groups refereceable data";
            this.chkAllGrpAccess.UseVisualStyleBackColor = true;
            // 
            // grpUserType
            // 
            this.grpUserType.Controls.Add(this.rdbPowerUser);
            this.grpUserType.Controls.Add(this.rdbSysAdmin);
            this.grpUserType.Controls.Add(this.rdbAdministrator);
            this.grpUserType.Controls.Add(this.rdbUser);
            this.grpUserType.Location = new System.Drawing.Point(299, 41);
            this.grpUserType.Name = "grpUserType";
            this.grpUserType.Size = new System.Drawing.Size(281, 85);
            this.grpUserType.TabIndex = 58;
            this.grpUserType.TabStop = false;
            // 
            // rdbPowerUser
            // 
            this.rdbPowerUser.AutoSize = true;
            this.rdbPowerUser.Location = new System.Drawing.Point(99, 17);
            this.rdbPowerUser.Name = "rdbPowerUser";
            this.rdbPowerUser.Size = new System.Drawing.Size(86, 17);
            this.rdbPowerUser.TabIndex = 0;
            this.rdbPowerUser.TabStop = true;
            this.rdbPowerUser.Text = "PowerUser";
            this.rdbPowerUser.UseVisualStyleBackColor = true;
            // 
            // rdbSysAdmin
            // 
            this.rdbSysAdmin.AutoSize = true;
            this.rdbSysAdmin.Checked = true;
            this.rdbSysAdmin.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdbSysAdmin.Location = new System.Drawing.Point(3, 51);
            this.rdbSysAdmin.Name = "rdbSysAdmin";
            this.rdbSysAdmin.Size = new System.Drawing.Size(275, 17);
            this.rdbSysAdmin.TabIndex = 0;
            this.rdbSysAdmin.TabStop = true;
            this.rdbSysAdmin.Text = "System Administrator";
            this.rdbSysAdmin.UseVisualStyleBackColor = true;
            // 
            // rdbAdministrator
            // 
            this.rdbAdministrator.AutoSize = true;
            this.rdbAdministrator.Checked = true;
            this.rdbAdministrator.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdbAdministrator.Location = new System.Drawing.Point(3, 34);
            this.rdbAdministrator.Name = "rdbAdministrator";
            this.rdbAdministrator.Size = new System.Drawing.Size(275, 17);
            this.rdbAdministrator.TabIndex = 0;
            this.rdbAdministrator.TabStop = true;
            this.rdbAdministrator.Text = "Administrator";
            this.rdbAdministrator.UseVisualStyleBackColor = true;
            // 
            // rdbUser
            // 
            this.rdbUser.AutoSize = true;
            this.rdbUser.Checked = true;
            this.rdbUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdbUser.Location = new System.Drawing.Point(3, 17);
            this.rdbUser.Name = "rdbUser";
            this.rdbUser.Size = new System.Drawing.Size(275, 17);
            this.rdbUser.TabIndex = 0;
            this.rdbUser.TabStop = true;
            this.rdbUser.Text = "User";
            this.rdbUser.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(89, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 8);
            this.label8.TabIndex = 57;
            this.label8.Text = "*";
            // 
            // label24
            // 
            this.label24.ForeColor = System.Drawing.Color.Red;
            this.label24.Location = new System.Drawing.Point(89, 135);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(12, 8);
            this.label24.TabIndex = 57;
            this.label24.Text = "*";
            // 
            // label19
            // 
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(89, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(12, 8);
            this.label19.TabIndex = 52;
            this.label19.Text = "*";
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(89, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(12, 8);
            this.label18.TabIndex = 51;
            this.label18.Text = "*";
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(89, 83);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(12, 8);
            this.label16.TabIndex = 49;
            this.label16.Text = "*";
            // 
            // txtFirstName
            // 
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFirstName.Location = new System.Drawing.Point(124, 74);
            this.txtFirstName.MaxLength = 50;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(150, 21);
            this.txtFirstName.TabIndex = 2;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(6, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 22);
            this.label1.TabIndex = 28;
            this.label1.Text = "First Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblShortName
            // 
            this.lblShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortName.Location = new System.Drawing.Point(6, 157);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(84, 22);
            this.lblShortName.TabIndex = 27;
            this.lblShortName.Text = "Short Name";
            this.lblShortName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(6, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 22);
            this.label2.TabIndex = 27;
            this.label2.Text = "Last Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRegion
            // 
            this.lblRegion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRegion.Location = new System.Drawing.Point(296, 20);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(60, 22);
            this.lblRegion.TabIndex = 29;
            this.lblRegion.Text = "Region :";
            this.lblRegion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 22);
            this.label3.TabIndex = 29;
            this.label3.Text = "Login";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(6, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 22);
            this.label4.TabIndex = 31;
            this.label4.Text = "Password";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(6, 130);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 23);
            this.label11.TabIndex = 17;
            this.label11.Text = "E-Mail Address ";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(6, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 23);
            this.label9.TabIndex = 17;
            this.label9.Text = "E-Mail Address ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLastName
            // 
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLastName.Location = new System.Drawing.Point(124, 101);
            this.txtLastName.MaxLength = 50;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(150, 21);
            this.txtLastName.TabIndex = 3;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // txtRegion
            // 
            this.txtRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegion.Location = new System.Drawing.Point(410, 20);
            this.txtRegion.MaxLength = 50;
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(170, 21);
            this.txtRegion.TabIndex = 9;
            // 
            // txtLoginName
            // 
            this.txtLoginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLoginName.Location = new System.Drawing.Point(124, 20);
            this.txtLoginName.MaxLength = 50;
            this.txtLoginName.Name = "txtLoginName";
            this.txtLoginName.Size = new System.Drawing.Size(150, 21);
            this.txtLoginName.TabIndex = 0;
            this.txtLoginName.TextChanged += new System.EventHandler(this.txtLoginName_TextChanged);
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(124, 157);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(150, 21);
            this.txtShortName.TabIndex = 8;
            this.txtShortName.TextChanged += new System.EventHandler(this.txtShortName_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPassword.Location = new System.Drawing.Point(124, 47);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(150, 21);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtEMail
            // 
            this.txtEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEMail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEMail.Location = new System.Drawing.Point(124, 128);
            this.txtEMail.MaxLength = 50;
            this.txtEMail.Name = "txtEMail";
            this.txtEMail.Size = new System.Drawing.Size(150, 21);
            this.txtEMail.TabIndex = 6;
            this.txtEMail.TextChanged += new System.EventHandler(this.txtEMail_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // UserSetupDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(589, 160);
            this.Name = "UserSetupDetails";
            this.Size = new System.Drawing.Size(653, 195);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpUserType.ResumeLayout(false);
            this.grpUserType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtLoginName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEMail;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.GroupBox grpUserType;
        private System.Windows.Forms.CheckBox chkAllGrpAccess;
        private System.Windows.Forms.RadioButton rdbPowerUser;
        private System.Windows.Forms.RadioButton rdbUser;
        private System.Windows.Forms.RadioButton rdbSysAdmin;
        private System.Windows.Forms.RadioButton rdbAdministrator;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label5;
       // private System.Windows.Forms.Label lblTradingAccount;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime2;
       // private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDown1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblShortName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Label label6;

    }
}
