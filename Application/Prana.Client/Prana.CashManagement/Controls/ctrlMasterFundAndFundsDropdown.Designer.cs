namespace Prana.CashManagement.Controls
{
    partial class ctrlMasterFundAndAccountsDropdown
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
            UnWireEvents();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.lblAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.cmbMultiAccounts = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.lblMasterFund = new Infragistics.Win.Misc.UltraLabel();
            this.cmbMasterFund = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.SuspendLayout();
            // 
            // lblAccounts
            // 
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.lblAccounts.Appearance = appearance1;
            this.lblAccounts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccounts.Location = new System.Drawing.Point(257, 10);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new System.Drawing.Size(57, 23);
            this.lblAccounts.TabIndex = 105;
            this.lblAccounts.Text = "Accounts";
            // 
            // cmbMultiAccounts
            // 
            this.cmbMultiAccounts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMultiAccounts.Location = new System.Drawing.Point(317, 10);
            this.cmbMultiAccounts.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMultiAccounts.Name = "cmbMultiAccounts";
            this.cmbMultiAccounts.Size = new System.Drawing.Size(165, 23);
            this.cmbMultiAccounts.TabIndex = 106;
            this.cmbMultiAccounts.TitleText = "";
            // 
            // lblMasterFund
            // 
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.lblMasterFund.Appearance = appearance2;
            this.lblMasterFund.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasterFund.Location = new System.Drawing.Point(2, 10);
            this.lblMasterFund.Name = "lblMasterFund";
            this.lblMasterFund.Size = new System.Drawing.Size(69, 23);
            this.lblMasterFund.TabIndex = 107;
            this.lblMasterFund.Text = "MasterFund";
            // 
            // cmbMasterFund
            // 
            this.cmbMasterFund.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMasterFund.Location = new System.Drawing.Point(73, 10);
            this.cmbMasterFund.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMasterFund.Name = "cmbMasterFund";
            this.cmbMasterFund.Size = new System.Drawing.Size(181, 23);
            this.cmbMasterFund.TabIndex = 108;
            this.cmbMasterFund.TitleText = "";
            // 
            // ctrlMasterFundAndAccountsDropdown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMasterFund);
            this.Controls.Add(this.cmbMasterFund);
            this.Controls.Add(this.lblAccounts);
            this.Controls.Add(this.cmbMultiAccounts);
            this.Name = "ctrlMasterFundAndAccountsDropdown";
            this.Size = new System.Drawing.Size(496, 42);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblAccounts;
        private Utilities.UI.UIUtilities.MultiSelectDropDown cmbMultiAccounts;
        private Infragistics.Win.Misc.UltraLabel lblMasterFund;
        private Utilities.UI.UIUtilities.MultiSelectDropDown cmbMasterFund;
    }
}
