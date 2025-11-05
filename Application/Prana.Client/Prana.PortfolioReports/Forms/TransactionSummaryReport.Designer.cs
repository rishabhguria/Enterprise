namespace Prana.PortfolioReports.Forms
{
    partial class TransactionSummaryReport
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
            this.ctrlTransactionSummaryReport = new Prana.PortfolioReports.CtrlTransactionSummaryReport();
            this.SuspendLayout();
            // 
            // ctrlTransactionSummaryReport
            // 
            this.ctrlTransactionSummaryReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlTransactionSummaryReport.Location = new System.Drawing.Point(0, 0);
            this.ctrlTransactionSummaryReport.LoginUser = null;
            this.ctrlTransactionSummaryReport.Name = "ctrlTransactionSummaryReport";
            this.ctrlTransactionSummaryReport.Size = new System.Drawing.Size(1105, 587);
            this.ctrlTransactionSummaryReport.TabIndex = 0;
            this.ctrlTransactionSummaryReport.Load += new System.EventHandler(this.ctrlTransactionSummaryReport_Load);
            // 
            // TransactionSummaryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 587);
            this.Controls.Add(this.ctrlTransactionSummaryReport);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "TransactionSummaryReport";
            this.Text = "Transaction Summary Report";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TransactionSummaryReport_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PortfolioReports.CtrlTransactionSummaryReport ctrlTransactionSummaryReport;
    }
}