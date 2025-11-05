namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class CashReconManualEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashReconManualEntry));
            this.ctrlCashReconManualEntry1 = new Nirvana.Admin.PositionManagement.Controls.CtrlCashReconManualEntry();
            this.SuspendLayout();
            // 
            // ctrlCashReconManualEntry1
            // 
            this.ctrlCashReconManualEntry1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlCashReconManualEntry1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCashReconManualEntry1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlCashReconManualEntry1.IsInitialized = false;
            this.ctrlCashReconManualEntry1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCashReconManualEntry1.Name = "ctrlCashReconManualEntry1";
            this.ctrlCashReconManualEntry1.Size = new System.Drawing.Size(481, 453);
            this.ctrlCashReconManualEntry1.TabIndex = 0;
            this.ctrlCashReconManualEntry1.Load += new System.EventHandler(this.ctrlCashReconManualEntry1_Load);
            // 
            // CashReconManualEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(481, 453);
            this.Controls.Add(this.ctrlCashReconManualEntry1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CashReconManualEntry";
            this.Text = "Cash Reconciliation Manual Entry";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlCashReconManualEntry ctrlCashReconManualEntry1;
    }
}