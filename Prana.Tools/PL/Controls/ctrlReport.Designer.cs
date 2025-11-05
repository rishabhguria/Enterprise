namespace Prana.Tools
{
    partial class ctrlReport
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlExcepptionReport1 = new Prana.Tools.ctrlExceptionReport();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlAuditTrail1 = new Prana.AuditTrail.ctrlAuditTrail();
            this.tbReport = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbReport)).BeginInit();
            this.tbReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlExcepptionReport1);
            this.ultraTabPageControl1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1086, 468);
            // 
            // ctrlExcepptionReport1
            // 
            this.ctrlExcepptionReport1.BackColor = System.Drawing.Color.Transparent;
            this.ctrlExcepptionReport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlExcepptionReport1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ctrlExcepptionReport1.Location = new System.Drawing.Point(0, 0);
            this.ctrlExcepptionReport1.Name = "ctrlExcepptionReport1";
            this.ctrlExcepptionReport1.Size = new System.Drawing.Size(1086, 468);
            this.ctrlExcepptionReport1.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ctrlAuditTrail1);
            this.ultraTabPageControl2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1086, 468);
            // 
            // ctrlAuditTrail1
            // 
            this.ctrlAuditTrail1.BackColor = System.Drawing.Color.Transparent;
            this.ctrlAuditTrail1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlAuditTrail1.Location = new System.Drawing.Point(0, 0);
            this.ctrlAuditTrail1.MinimumSize = new System.Drawing.Size(968, 248);
            this.ctrlAuditTrail1.Name = "ctrlAuditTrail1";
            this.ctrlAuditTrail1.Size = new System.Drawing.Size(1086, 468);
            this.ctrlAuditTrail1.TabIndex = 0;
            // 
            // tbReport
            // 
            this.tbReport.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbReport.Controls.Add(this.ultraTabPageControl1);
            this.tbReport.Controls.Add(this.ultraTabPageControl2);
            this.tbReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tbReport.Location = new System.Drawing.Point(0, 0);
            this.tbReport.Name = "tbReport";
            this.tbReport.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbReport.Size = new System.Drawing.Size(1090, 494);
            this.tbReport.TabIndex = 0;
            ultraTab1.Key = "tbExceptionReport";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Exception Report";
            ultraTab2.Key = "tbAuditTrailReport";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Audit Trail Report";
            this.tbReport.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1086, 468);
            // 
            // ctrlReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbReport);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "ctrlReport";
            this.Size = new System.Drawing.Size(1090, 494);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbReport)).EndInit();
            this.tbReport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbReport;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Prana.Tools.ctrlExceptionReport ctrlExcepptionReport1;
        private Prana.AuditTrail.ctrlAuditTrail ctrlAuditTrail1;
    }
}
