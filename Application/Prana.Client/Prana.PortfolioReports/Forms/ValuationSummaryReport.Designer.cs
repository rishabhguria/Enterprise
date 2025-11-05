namespace Prana.PortfolioReports.Forms
{
    partial class ValuationSummaryReport
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
            this.ctrlValuationSummaryReport = new Prana.PortfolioReports.Controls.CtrlValuationSummaryReport();
            this.SuspendLayout();
            // 
            // ctrlValuationSummaryReport
            // 
            this.ctrlValuationSummaryReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlValuationSummaryReport.Location = new System.Drawing.Point(0, 0);
            this.ctrlValuationSummaryReport.LoginUser = null;
            this.ctrlValuationSummaryReport.Name = "ctrlValuationSummaryReport";
            this.ctrlValuationSummaryReport.Size = new System.Drawing.Size(892, 618);
            this.ctrlValuationSummaryReport.TabIndex = 0;
            this.ctrlValuationSummaryReport.Load += new System.EventHandler(this.ctrlValuationSummaryReport_Load);
            // 
            // ValuationSummaryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 618);
            this.Controls.Add(this.ctrlValuationSummaryReport);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "ValuationSummaryReport";
            this.Text = "Valuation Summary Report";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ValuationSummaryReport_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PortfolioReports.Controls.CtrlValuationSummaryReport ctrlValuationSummaryReport;
    }
}