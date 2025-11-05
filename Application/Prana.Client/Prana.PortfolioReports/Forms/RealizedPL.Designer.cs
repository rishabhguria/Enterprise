namespace Prana.PortfolioReports.Forms
{
    partial class RealizedPL
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (frmClosingHistory != null)
                {
                    frmClosingHistory.Dispose();
                }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ctrlRealizedPL = new Prana.PortfolioReports.Controls.CntrlRealizedPL();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.cntrlDailyPositions = new Prana.PortfolioReports.Controls.CntrlDailyPositions();
            this.btnClosingHistory = new Infragistics.Win.Misc.UltraButton();
            this.tabCtrlPMReports = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlPMReports)).BeginInit();
            this.tabCtrlPMReports.SuspendLayout();
            this.ultraTabSharedControlsPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ctrlRealizedPL);
            this.ultraTabPageControl1.Controls.Add(this.btnClosingHistory);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(814, 384);
            // 
            // ctrlRealizedPL
            // 
            this.ctrlRealizedPL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlRealizedPL.Location = new System.Drawing.Point(0, 0);
            this.ctrlRealizedPL.Name = "ctrlRealizedPL";
            this.ctrlRealizedPL.Size = new System.Drawing.Size(814, 353);
            this.ctrlRealizedPL.TabIndex = 0;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.cntrlDailyPositions);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(814, 384);
            // 
            // cntrlDailyPositions
            // 
            this.cntrlDailyPositions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cntrlDailyPositions.Location = new System.Drawing.Point(0, 0);
            this.cntrlDailyPositions.Name = "cntrlDailyPositions";
            this.cntrlDailyPositions.Size = new System.Drawing.Size(814, 352);
            this.cntrlDailyPositions.TabIndex = 0;
            // 
            // btnClosingHistory
            // 
            this.btnClosingHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosingHistory.Location = new System.Drawing.Point(363, 358);
            this.btnClosingHistory.Name = "btnClosingHistory";
            this.btnClosingHistory.Size = new System.Drawing.Size(91, 23);
            this.btnClosingHistory.TabIndex = 1;
            this.btnClosingHistory.Text = "Closing History";
            this.btnClosingHistory.Click += new System.EventHandler(this.btnClosingHistory_Click);
            // 
            // tabCtrlPMReports
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tabCtrlPMReports.Appearance = appearance1;
            this.tabCtrlPMReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tabCtrlPMReports.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabCtrlPMReports.Controls.Add(this.ultraTabPageControl1);
            this.tabCtrlPMReports.Controls.Add(this.ultraTabPageControl2);
            this.tabCtrlPMReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlPMReports.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlPMReports.Name = "tabCtrlPMReports";
            this.tabCtrlPMReports.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.btnClosingHistory});
            this.tabCtrlPMReports.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabCtrlPMReports.Size = new System.Drawing.Size(818, 410);
            this.tabCtrlPMReports.TabIndex = 0;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab1.ActiveAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab1.Appearance = appearance3;
            ultraTab1.Key = "tbpRealizedPL";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Realized P&&L";
            ultraTab2.Key = "tbpDailyPositions";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Daily Positions";
            this.tabCtrlPMReports.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Controls.Add(this.btnClosingHistory);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(814, 384);
            // 
            // RealizedPL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 410);
            this.Controls.Add(this.tabCtrlPMReports);
            this.MinimumSize = new System.Drawing.Size(826, 444);
            this.Name = "RealizedPL";
            this.Text = "RealizedPL";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RealizedPL_FormClosed);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlPMReports)).EndInit();
            this.tabCtrlPMReports.ResumeLayout(false);
            this.ultraTabSharedControlsPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCtrlPMReports;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Prana.PortfolioReports.Controls.CntrlRealizedPL ctrlRealizedPL;
        private Prana.PortfolioReports.Controls.CntrlDailyPositions cntrlDailyPositions;
        private Infragistics.Win.Misc.UltraButton btnClosingHistory;
    }
}