using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Properties;

namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlDSPrimaryContactDetails
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.grpbxDSPrimaryContact = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblWorkNumberRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblEmailRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblTitleRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblLastNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.lblFirstNameRequired = new Infragistics.Win.Misc.UltraLabel();
            this.txtPCCellNumber = new System.Windows.Forms.TextBox();
            this.lblPCCellNumber = new Infragistics.Win.Misc.UltraLabel();
            this.txtPCWorkNumber = new System.Windows.Forms.TextBox();
            this.lblPCWorkNumber = new Infragistics.Win.Misc.UltraLabel();
            this.txtPCEmail = new System.Windows.Forms.TextBox();
            this.lblPCEmail = new Infragistics.Win.Misc.UltraLabel();
            this.txtPCLastName = new System.Windows.Forms.TextBox();
            this.lblPCLastName = new Infragistics.Win.Misc.UltraLabel();
            this.txtPCTitle = new System.Windows.Forms.TextBox();
            this.txtPCFirstName = new System.Windows.Forms.TextBox();
            this.lblPCTitle = new Infragistics.Win.Misc.UltraLabel();
            this.lblPCFirstName = new Infragistics.Win.Misc.UltraLabel();
            this.errPrimaryDetails = new System.Windows.Forms.ErrorProvider(this.components);
            this.bindingSourcePrimaryContact = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grpbxDSPrimaryContact)).BeginInit();
            this.grpbxDSPrimaryContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errPrimaryDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePrimaryContact)).BeginInit();
            this.SuspendLayout();
            // 
            // grpbxDSPrimaryContact
            // 
            this.grpbxDSPrimaryContact.Controls.Add(this.lblWorkNumberRequired);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblEmailRequired);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblTitleRequired);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblLastNameRequired);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblFirstNameRequired);
            this.grpbxDSPrimaryContact.Controls.Add(this.txtPCCellNumber);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblPCCellNumber);
            this.grpbxDSPrimaryContact.Controls.Add(this.txtPCWorkNumber);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblPCWorkNumber);
            this.grpbxDSPrimaryContact.Controls.Add(this.txtPCEmail);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblPCEmail);
            this.grpbxDSPrimaryContact.Controls.Add(this.txtPCLastName);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblPCLastName);
            this.grpbxDSPrimaryContact.Controls.Add(this.txtPCTitle);
            this.grpbxDSPrimaryContact.Controls.Add(this.txtPCFirstName);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblPCTitle);
            this.grpbxDSPrimaryContact.Controls.Add(this.lblPCFirstName);
            this.grpbxDSPrimaryContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpbxDSPrimaryContact.Location = new System.Drawing.Point(0, 0);
            this.grpbxDSPrimaryContact.Name = "grpbxDSPrimaryContact";
            this.grpbxDSPrimaryContact.Size = new System.Drawing.Size(252, 184);
            this.grpbxDSPrimaryContact.SupportThemes = false;
            this.grpbxDSPrimaryContact.TabIndex = 0;
            this.grpbxDSPrimaryContact.Text = "Primary Contact";
            this.grpbxDSPrimaryContact.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
            // 
            // lblWorkNumberRequired
            // 
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.lblWorkNumberRequired.Appearance = appearance1;
            this.lblWorkNumberRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblWorkNumberRequired.Location = new System.Drawing.Point(79, 129);
            this.lblWorkNumberRequired.Name = "lblWorkNumberRequired";
            this.lblWorkNumberRequired.Size = new System.Drawing.Size(10, 15);
            this.lblWorkNumberRequired.TabIndex = 29;
            this.lblWorkNumberRequired.Text = "*";
            // 
            // lblEmailRequired
            // 
            appearance2.ForeColor = System.Drawing.Color.Red;
            this.lblEmailRequired.Appearance = appearance2;
            this.lblEmailRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEmailRequired.Location = new System.Drawing.Point(79, 100);
            this.lblEmailRequired.Name = "lblEmailRequired";
            this.lblEmailRequired.Size = new System.Drawing.Size(10, 15);
            this.lblEmailRequired.TabIndex = 28;
            this.lblEmailRequired.Text = "*";
            // 
            // lblTitleRequired
            // 
            appearance3.ForeColor = System.Drawing.Color.Red;
            this.lblTitleRequired.Appearance = appearance3;
            this.lblTitleRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitleRequired.Location = new System.Drawing.Point(79, 72);
            this.lblTitleRequired.Name = "lblTitleRequired";
            this.lblTitleRequired.Size = new System.Drawing.Size(10, 15);
            this.lblTitleRequired.TabIndex = 27;
            this.lblTitleRequired.Text = "*";
            // 
            // lblLastNameRequired
            // 
            appearance4.ForeColor = System.Drawing.Color.Red;
            this.lblLastNameRequired.Appearance = appearance4;
            this.lblLastNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblLastNameRequired.Location = new System.Drawing.Point(79, 48);
            this.lblLastNameRequired.Name = "lblLastNameRequired";
            this.lblLastNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblLastNameRequired.TabIndex = 26;
            this.lblLastNameRequired.Text = "*";
            // 
            // lblFirstNameRequired
            // 
            appearance5.ForeColor = System.Drawing.Color.Red;
            this.lblFirstNameRequired.Appearance = appearance5;
            this.lblFirstNameRequired.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFirstNameRequired.Location = new System.Drawing.Point(79, 22);
            this.lblFirstNameRequired.Name = "lblFirstNameRequired";
            this.lblFirstNameRequired.Size = new System.Drawing.Size(10, 15);
            this.lblFirstNameRequired.TabIndex = 25;
            this.lblFirstNameRequired.Text = "*";
            // 
            // txtPCCellNumber
            // 
            
            this.txtPCCellNumber.Location = new System.Drawing.Point(95, 152);
            this.txtPCCellNumber.Name = "txtPCCellNumber";
            this.txtPCCellNumber.Size = new System.Drawing.Size(150, 20);
            this.txtPCCellNumber.TabIndex = 5;
            // 
            // lblPCCellNumber
            // 
            this.lblPCCellNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCCellNumber.Location = new System.Drawing.Point(6, 152);
            this.lblPCCellNumber.Name = "lblPCCellNumber";
            this.lblPCCellNumber.Size = new System.Drawing.Size(62, 15);
            this.lblPCCellNumber.TabIndex = 22;
            this.lblPCCellNumber.Text = "CellNumber";
            // 
            // txtPCWorkNumber
            // 
            
            this.txtPCWorkNumber.Location = new System.Drawing.Point(95, 126);
            this.txtPCWorkNumber.Name = "txtPCWorkNumber";
            this.txtPCWorkNumber.Size = new System.Drawing.Size(150, 20);
            this.txtPCWorkNumber.TabIndex = 4;
            // 
            // lblPCWorkNumber
            // 
            this.lblPCWorkNumber.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCWorkNumber.Location = new System.Drawing.Point(6, 126);
            this.lblPCWorkNumber.Name = "lblPCWorkNumber";
            this.lblPCWorkNumber.Size = new System.Drawing.Size(74, 15);
            this.lblPCWorkNumber.TabIndex = 20;
            this.lblPCWorkNumber.Text = "Work Number";
            // 
            // txtPCEmail
            // 
            
            this.txtPCEmail.Location = new System.Drawing.Point(95, 100);
            this.txtPCEmail.Name = "txtPCEmail";
            this.txtPCEmail.Size = new System.Drawing.Size(150, 20);
            this.txtPCEmail.TabIndex = 3;
            // 
            // lblPCEmail
            // 
            this.lblPCEmail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCEmail.Location = new System.Drawing.Point(6, 100);
            this.lblPCEmail.Name = "lblPCEmail";
            this.lblPCEmail.Size = new System.Drawing.Size(34, 15);
            this.lblPCEmail.TabIndex = 18;
            this.lblPCEmail.Text = "E-Mail";
            // 
            // txtPCLastName
            // 
            
            this.txtPCLastName.Location = new System.Drawing.Point(95, 48);
            this.txtPCLastName.Name = "txtPCLastName";
            this.txtPCLastName.Size = new System.Drawing.Size(150, 20);
            this.txtPCLastName.TabIndex = 1;
            // 
            // lblPCLastName
            // 
            this.lblPCLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCLastName.Location = new System.Drawing.Point(6, 48);
            this.lblPCLastName.Name = "lblPCLastName";
            this.lblPCLastName.Size = new System.Drawing.Size(57, 15);
            this.lblPCLastName.TabIndex = 14;
            this.lblPCLastName.Text = "Last Name";
            // 
            // txtPCTitle
            // 
            
            this.txtPCTitle.Location = new System.Drawing.Point(95, 74);
            this.txtPCTitle.Name = "txtPCTitle";
            this.txtPCTitle.Size = new System.Drawing.Size(150, 20);
            this.txtPCTitle.TabIndex = 2;
            // 
            // txtPCFirstName
            // 
            this.txtPCFirstName.Location = new System.Drawing.Point(95, 22);
            this.txtPCFirstName.Name = "txtPCFirstName";
            this.txtPCFirstName.Size = new System.Drawing.Size(150, 20);
            this.txtPCFirstName.TabIndex = 0;
            // 
            // lblPCTitle
            // 
            this.lblPCTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCTitle.Location = new System.Drawing.Point(6, 74);
            this.lblPCTitle.Name = "lblPCTitle";
            this.lblPCTitle.Size = new System.Drawing.Size(26, 15);
            this.lblPCTitle.TabIndex = 16;
            this.lblPCTitle.Text = "Title";
            // 
            // lblPCFirstName
            // 
            this.lblPCFirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPCFirstName.Location = new System.Drawing.Point(6, 22);
            this.lblPCFirstName.Name = "lblPCFirstName";
            this.lblPCFirstName.Size = new System.Drawing.Size(58, 15);
            this.lblPCFirstName.TabIndex = 12;
            this.lblPCFirstName.Text = "First Name";
            // 
            // errPrimaryDetails
            // 
            this.errPrimaryDetails.ContainerControl = this;
            // 
            // bindingSourcePrimaryContact
            // 
            this.bindingSourcePrimaryContact.DataSource = typeof(Nirvana.Admin.PositionManagement.BusinessObjects.DataSourcePrimaryContact);
            // 
            // CtrlDSPrimaryContactDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.grpbxDSPrimaryContact);
            this.Name = "CtrlDSPrimaryContactDetails";
            this.Size = new System.Drawing.Size(252, 184);
            ((System.ComponentModel.ISupportInitialize)(this.grpbxDSPrimaryContact)).EndInit();
            this.grpbxDSPrimaryContact.ResumeLayout(false);
            this.grpbxDSPrimaryContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errPrimaryDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePrimaryContact)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private Infragistics.Win.Misc.UltraGroupBox grpbxDSPrimaryContact;
        private System.Windows.Forms.TextBox txtPCFirstName;
        private Infragistics.Win.Misc.UltraLabel lblPCFirstName;
        private System.Windows.Forms.TextBox txtPCTitle;
        private Infragistics.Win.Misc.UltraLabel lblPCTitle;
        private System.Windows.Forms.TextBox txtPCLastName;
        private Infragistics.Win.Misc.UltraLabel lblPCLastName;
        private System.Windows.Forms.TextBox txtPCCellNumber;
        private Infragistics.Win.Misc.UltraLabel lblPCCellNumber;
        private System.Windows.Forms.TextBox txtPCWorkNumber;
        private Infragistics.Win.Misc.UltraLabel lblPCWorkNumber;
        private System.Windows.Forms.TextBox txtPCEmail;
        private Infragistics.Win.Misc.UltraLabel lblPCEmail;
        private Infragistics.Win.Misc.UltraLabel lblFirstNameRequired;
        private Infragistics.Win.Misc.UltraLabel lblWorkNumberRequired;
        private Infragistics.Win.Misc.UltraLabel lblEmailRequired;
        private Infragistics.Win.Misc.UltraLabel lblTitleRequired;
        private Infragistics.Win.Misc.UltraLabel lblLastNameRequired;
        public System.Windows.Forms.ErrorProvider errPrimaryDetails;
        private System.Windows.Forms.BindingSource bindingSourcePrimaryContact;
    }
}
