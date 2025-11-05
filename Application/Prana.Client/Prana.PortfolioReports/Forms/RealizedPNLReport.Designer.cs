namespace Prana.PortfolioReports.Forms
{
    partial class RealizedPNLReport
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
            this.ctrlRealizedPNLReport = new Prana.PortfolioReports.Controls.CtrlRealizedPNLReport();
            this.SuspendLayout();
            // 
            // ctrlRealizedPNLReport
            // 
            this.ctrlRealizedPNLReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlRealizedPNLReport.Location = new System.Drawing.Point(0, 0);
            this.ctrlRealizedPNLReport.LoginUser = null;
            this.ctrlRealizedPNLReport.Name = "ctrlRealizedPNLReport";
            this.ctrlRealizedPNLReport.Size = new System.Drawing.Size(951, 587);
            this.ctrlRealizedPNLReport.TabIndex = 0;
            this.ctrlRealizedPNLReport.Load += new System.EventHandler(this.ctrlRealizedPNLReport_Load);
            // 
            // RealizedPNLReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 587);
            this.Controls.Add(this.ctrlRealizedPNLReport);
            this.Name = "RealizedPNLReport";
            this.Text = "RealizedPNL Report";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RealizedPNLReport_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PortfolioReports.Controls.CtrlRealizedPNLReport ctrlRealizedPNLReport;
    }
}