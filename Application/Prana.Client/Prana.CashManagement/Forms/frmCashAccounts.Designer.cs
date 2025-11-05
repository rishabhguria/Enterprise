namespace Prana.CashManagement
{
    partial class frmCashAccounts
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctrlCashAccounts1 = new Prana.CashManagement.ctrlCashAccounts();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctrlCashAccounts1
            // 
            this.ctrlCashAccounts1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCashAccounts1.Name = "ctrlCashAccounts1";
            this.ctrlCashAccounts1.Size = new System.Drawing.Size(442, 207);
            this.ctrlCashAccounts1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(190, 216);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmCashAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 251);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ctrlCashAccounts1);
            this.Name = "frmCashAccounts";
            this.Text = "Cash Accounts";
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlCashAccounts ctrlCashAccounts1;
        private System.Windows.Forms.Button btnSave;
    }
}