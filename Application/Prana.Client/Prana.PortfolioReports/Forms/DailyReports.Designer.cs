namespace Prana.PortfolioReports.Forms
{
    partial class DailyReports
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
            this.ctrlDailySheets1 = new Prana.PortfolioReports.Controls.CtrlDailySheets();
            this.SuspendLayout();
            // 
            // ctrlDailySheets1
            // 
            this.ctrlDailySheets1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlDailySheets1.Location = new System.Drawing.Point(0, 0);
            this.ctrlDailySheets1.Name = "ctrlDailySheets1";
            this.ctrlDailySheets1.Size = new System.Drawing.Size(710, 399);
            this.ctrlDailySheets1.TabIndex = 0;
            // 
            // DailyReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 399);
            this.Controls.Add(this.ctrlDailySheets1);
            this.MinimumSize = new System.Drawing.Size(409, 398);
            this.Name = "DailyReports";
            this.Text = "Monthly Summary";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DailyReports_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PortfolioReports.Controls.CtrlDailySheets ctrlDailySheets1;


    }
}