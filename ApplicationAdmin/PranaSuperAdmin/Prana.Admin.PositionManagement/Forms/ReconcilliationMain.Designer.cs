namespace Nirvana.Admin.PositionManagement.Forms
{
    partial class ReconcilliationMain
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReconcilliationMain));
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabReconciliation = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage5 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ctrlSetupTradeRecon1 = new Nirvana.Admin.PositionManagement.Controls.CtrlSetupTradeRecon();
            this.ctrlRunTradeRecon1 = new Nirvana.Admin.PositionManagement.Controls.CtrlRunTradeRecon();
            this.ultraTabPageControl11.SuspendLayout();
            this.ultraTabPageControl12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabReconciliation)).BeginInit();
            this.tabReconciliation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Controls.Add(this.ctrlSetupTradeRecon1);
            this.ultraTabPageControl11.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(830, 451);
            // 
            // ultraTabPageControl12
            // 
            this.ultraTabPageControl12.Controls.Add(this.ctrlRunTradeRecon1);
            this.ultraTabPageControl12.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            this.ultraTabPageControl12.Size = new System.Drawing.Size(830, 451);
            // 
            // tabReconciliation
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabReconciliation.ActiveTabAppearance = appearance1;
            this.tabReconciliation.Controls.Add(this.ultraTabSharedControlsPage5);
            this.tabReconciliation.Controls.Add(this.ultraTabPageControl11);
            this.tabReconciliation.Controls.Add(this.ultraTabPageControl12);
            this.tabReconciliation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReconciliation.Location = new System.Drawing.Point(0, 0);
            this.tabReconciliation.Name = "tabReconciliation";
            appearance2.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            this.tabReconciliation.SelectedTabAppearance = appearance2;
            this.tabReconciliation.SharedControlsPage = this.ultraTabSharedControlsPage5;
            this.tabReconciliation.Size = new System.Drawing.Size(832, 472);
            this.tabReconciliation.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabReconciliation.TabIndex = 2;
            ultraTab1.Key = "SetUp Recon";
            ultraTab1.TabPage = this.ultraTabPageControl11;
            ultraTab1.Text = "SetUp Recon";
            ultraTab2.Key = "Run Re-con";
            ultraTab2.TabPage = this.ultraTabPageControl12;
            ultraTab2.Text = "Run Re-con";
            this.tabReconciliation.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.tabReconciliation.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabReconciliation_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage5
            // 
            this.ultraTabSharedControlsPage5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage5.Name = "ultraTabSharedControlsPage5";
            this.ultraTabSharedControlsPage5.Size = new System.Drawing.Size(830, 451);
            // 
            // ctrlSetupTradeRecon1
            // 
            this.ctrlSetupTradeRecon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlSetupTradeRecon1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSetupTradeRecon1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlSetupTradeRecon1.IsInitialized = false;
            this.ctrlSetupTradeRecon1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSetupTradeRecon1.Name = "ctrlSetupTradeRecon1";
            this.ctrlSetupTradeRecon1.Size = new System.Drawing.Size(830, 451);
            this.ctrlSetupTradeRecon1.TabIndex = 0;
            // 
            // ctrlRunTradeRecon1
            // 
            this.ctrlRunTradeRecon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlRunTradeRecon1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlRunTradeRecon1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlRunTradeRecon1.IsInitialized = false;
            this.ctrlRunTradeRecon1.Location = new System.Drawing.Point(0, 0);
            this.ctrlRunTradeRecon1.Name = "ctrlRunTradeRecon1";
            this.ctrlRunTradeRecon1.Size = new System.Drawing.Size(830, 451);
            this.ctrlRunTradeRecon1.TabIndex = 0;
            // 
            // ReconcilliationMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 472);
            this.Controls.Add(this.tabReconciliation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReconcilliationMain";
            this.Text = "ReconcilliationMain";
            this.Load += new System.EventHandler(this.ReconcilliationMain_Load);
            this.ultraTabPageControl11.ResumeLayout(false);
            this.ultraTabPageControl12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabReconciliation)).EndInit();
            this.tabReconciliation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabReconciliation;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Nirvana.Admin.PositionManagement.Controls.CtrlSetupTradeRecon ctrlSetupTradeRecon1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Nirvana.Admin.PositionManagement.Controls.CtrlRunTradeRecon ctrlRunTradeRecon1;
    }
}