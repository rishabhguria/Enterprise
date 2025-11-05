namespace Prana.PortfolioReports.Forms
{
    partial class MTDDailyPNL
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
            this.ctrlMTDDailyPNL1 = new Prana.PortfolioReports.Controls.CtrlMTDDailyPNL();
            this.SuspendLayout();
            // 
            // ctrlMTDDailyPNL1
            // 
            this.ctrlMTDDailyPNL1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlMTDDailyPNL1.Location = new System.Drawing.Point(0, 0);
            this.ctrlMTDDailyPNL1.Name = "ctrlMTDDailyPNL1";
            this.ctrlMTDDailyPNL1.Size = new System.Drawing.Size(696, 369);
            this.ctrlMTDDailyPNL1.TabIndex = 0;
            // 
            // MTDDailyPNL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 369);
            this.Controls.Add(this.ctrlMTDDailyPNL1);
            this.MinimumSize = new System.Drawing.Size(376, 401);
            this.Name = "MTDDailyPNL";
            this.Text = "MTDDailyPNL";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MonthlyReports_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PortfolioReports.Controls.CtrlMTDDailyPNL ctrlMTDDailyPNL1;
    }
}