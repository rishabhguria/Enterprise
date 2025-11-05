namespace Prana.Blotter
{
    partial class CtrlBlotterMain
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
            Infragistics.Win.UltraWinTabControl.UltraTab OrdersBlotterTab = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab WorkingSubsBlotterTab = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab SummaryBlotterTab = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.OrdersTabPageControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.WorkingSubsTabPageControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.SummaryTabPageControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.BlotterTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.BlotterTabSharedControlsPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.OrderBlotterGrid = new Prana.Blotter.OrderBlotterGrid();
            this.WorkingSubBlotterGrid = new Prana.Blotter.WorkingSubBlotterGrid();
            this.SummaryBlotterGrid = new Prana.Blotter.SummaryBlotterGrid();
            this.OrdersTabPageControl.SuspendLayout();
            this.WorkingSubsTabPageControl.SuspendLayout();
            this.SummaryTabPageControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlotterTabControl)).BeginInit();
            this.BlotterTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // OrdersTabPageControl
            // 
            this.OrdersTabPageControl.Controls.Add(this.OrderBlotterGrid);
            this.OrdersTabPageControl.Location = new System.Drawing.Point(1, 20);
            this.OrdersTabPageControl.Name = "OrdersTabPageControl";
            this.OrdersTabPageControl.Size = new System.Drawing.Size(925, 492);
            // 
            // WorkingSubsTabPageControl
            // 
            this.WorkingSubsTabPageControl.Controls.Add(this.WorkingSubBlotterGrid);
            this.WorkingSubsTabPageControl.Location = new System.Drawing.Point(-10000, -10000);
            this.WorkingSubsTabPageControl.Name = "WorkingSubsTabPageControl";
            this.WorkingSubsTabPageControl.Size = new System.Drawing.Size(925, 492);
            // 
            // SummaryTabPageControl
            // 
            this.SummaryTabPageControl.Controls.Add(this.SummaryBlotterGrid);
            this.SummaryTabPageControl.Location = new System.Drawing.Point(-10000, -10000);
            this.SummaryTabPageControl.Name = "SummaryTabPageControl";
            this.SummaryTabPageControl.Size = new System.Drawing.Size(925, 492);
            // 
            // BlotterTabControl
            // 
            this.BlotterTabControl.AllowTabMoving = true;
            this.BlotterTabControl.Controls.Add(this.BlotterTabSharedControlsPage);
            this.BlotterTabControl.Controls.Add(this.OrdersTabPageControl);
            this.BlotterTabControl.Controls.Add(this.WorkingSubsTabPageControl);
            this.BlotterTabControl.Controls.Add(this.SummaryTabPageControl);
            this.BlotterTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BlotterTabControl.Location = new System.Drawing.Point(0, 0);
            this.BlotterTabControl.Name = "BlotterTabControl";
            this.BlotterTabControl.SharedControlsPage = this.BlotterTabSharedControlsPage;
            this.BlotterTabControl.Size = new System.Drawing.Size(927, 513);
            this.BlotterTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.BlotterTabControl.TabIndex = 0;
            OrdersBlotterTab.Key = "Orders";
            OrdersBlotterTab.TabPage = this.OrdersTabPageControl;
            OrdersBlotterTab.Text = "Orders";
            WorkingSubsBlotterTab.Key = "WorkingSubs";
            WorkingSubsBlotterTab.TabPage = this.WorkingSubsTabPageControl;
            WorkingSubsBlotterTab.Text = "Working Subs";
            SummaryBlotterTab.Key = "Summary";
            SummaryBlotterTab.TabPage = this.SummaryTabPageControl;
            SummaryBlotterTab.Text = "Summary";
            this.BlotterTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            OrdersBlotterTab,
            WorkingSubsBlotterTab,
            SummaryBlotterTab});
            BlotterTabControl.MouseClick += BlotterTabControl_TabIndexChanged;
            // 
            // BlotterTabSharedControlsPage
            // 
            this.BlotterTabSharedControlsPage.Location = new System.Drawing.Point(-10000, -10000);
            this.BlotterTabSharedControlsPage.Name = "BlotterTabSharedControlsPage";
            this.BlotterTabSharedControlsPage.Size = new System.Drawing.Size(925, 492);
            // 
            // OrderBlotterGrid
            // 
            this.OrderBlotterGrid.BlotterType = Prana.Global.OrderFields.BlotterTypes.Orders;
            this.OrderBlotterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrderBlotterGrid.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.OrderBlotterGrid.Key = "Orders";
            this.OrderBlotterGrid.Location = new System.Drawing.Point(0, 2);
            this.OrderBlotterGrid.Name = "OrderBlotterGrid";
            this.OrderBlotterGrid.Size = new System.Drawing.Size(339, 311);
            this.OrderBlotterGrid.TabIndex = 11;
            this.OrderBlotterGrid.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
            this.OrderBlotterGrid.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
            this.OrderBlotterGrid.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
            this.OrderBlotterGrid.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
            this.OrderBlotterGrid.VisibleChanged += new System.EventHandler(this.OrderBlotterGrid_VisibleChanged);
            this.OrderBlotterGrid.GoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
            //SubOrder Blotter grid Events wiring
            this.OrderBlotterGrid.SubOrderBlotterLaunchAddFills += this.WorkingSubBlotterGrid_AddFillsClick;
            this.OrderBlotterGrid.SubOrderBlotterLaunchAuditTrail += this.WorkingSubBlotterGrid_AuditTrailClick;
            this.OrderBlotterGrid.SubOrderBlotterGridTradeClick += this.WorkingSubBlotterGrid_TradeClick;
            this.OrderBlotterGrid.SubOrderBloterGoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
            this.OrderBlotterGrid.SubOrderBloterReplaceOrEditOrderClicked += this.BlotterGrid_ReplaceOrEditClicked;
            this.OrderBlotterGrid.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
            this.OrderBlotterGrid.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
            this.OrderBlotterGrid.SubOrderBloterUpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
            this.OrderBlotterGrid.DisableRolloverButton += this.blotterGrid_DisableRolloverButton;
            this.OrderBlotterGrid.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
            // 
            // WorkingSubBlotterGrid
            // 
            this.WorkingSubBlotterGrid.BlotterType = Prana.Global.OrderFields.BlotterTypes.WorkingSubs;
            this.WorkingSubBlotterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkingSubBlotterGrid.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.WorkingSubBlotterGrid.Key = "Working Subs";
            this.WorkingSubBlotterGrid.Location = new System.Drawing.Point(0, 2);
            this.WorkingSubBlotterGrid.Name = "WorkingSubBlotterGrid";
            this.WorkingSubBlotterGrid.Size = new System.Drawing.Size(339, 311);
            this.WorkingSubBlotterGrid.TabIndex = 2;
            this.WorkingSubBlotterGrid.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
            this.WorkingSubBlotterGrid.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
            this.WorkingSubBlotterGrid.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
            this.WorkingSubBlotterGrid.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
            this.WorkingSubBlotterGrid.VisibleChanged += new System.EventHandler(this.WorkingSubBlotterGrid_VisibleChanged);
            this.WorkingSubBlotterGrid.GoToAllocationClicked += this.BlotterGrid_GoToAllocationClicked;
            this.WorkingSubBlotterGrid.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
            this.WorkingSubBlotterGrid.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
            this.WorkingSubBlotterGrid.DisableRolloverButton += this.blotterGrid_DisableRolloverButton;
            this.WorkingSubBlotterGrid.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
            // 
            // SummaryBlotterGrid
            // 
            this.SummaryBlotterGrid.BlotterType = Prana.Global.OrderFields.BlotterTypes.Summary;
            this.SummaryBlotterGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SummaryBlotterGrid.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.SummaryBlotterGrid.Key = "Summary";
            this.SummaryBlotterGrid.Location = new System.Drawing.Point(0, 2);
            this.SummaryBlotterGrid.LoginUSer = null;
            this.SummaryBlotterGrid.Name = "SummaryBlotterGrid";
            this.SummaryBlotterGrid.Size = new System.Drawing.Size(339, 311);
            this.SummaryBlotterGrid.TabIndex = 8;
            this.SummaryBlotterGrid.TradeClick += new System.EventHandler(this.WorkingSubBlotterGrid_TradeClick);
            this.SummaryBlotterGrid.ReplaceOrEditOrderClicked += new System.EventHandler(this.BlotterGrid_ReplaceOrEditClicked);
            this.SummaryBlotterGrid.LaunchAuditTrail += new System.EventHandler(this.WorkingSubBlotterGrid_AuditTrailClick);
            this.SummaryBlotterGrid.LaunchAddFills += new System.EventHandler(this.WorkingSubBlotterGrid_AddFillsClick);
            this.SummaryBlotterGrid.VisibleChanged += new System.EventHandler(this.SummaryBlotterGrid_VisibleChanged);
            this.SummaryBlotterGrid.UpdateCountStatusBar += this.BlotterGrid_UpdateCountStatusBar;
            this.SummaryBlotterGrid.UpdateStatusBar += this.BlotterGrid_UpdateStatusBar;
            this.SummaryBlotterGrid.DisableRolloverButton += this.blotterGrid_DisableRolloverButton;
            this.SummaryBlotterGrid.UpdateOnRolloverComplete += this.blotterGrid_UpdateOnRolloverComplete;
            // 
            // CtrlBlotterMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BlotterTabControl);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "CtrlBlotterMain";
            this.Size = new System.Drawing.Size(927, 513);
            this.OrdersTabPageControl.ResumeLayout(false);
            this.WorkingSubsTabPageControl.ResumeLayout(false);
            this.SummaryTabPageControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BlotterTabControl)).EndInit();
            this.BlotterTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public Infragistics.Win.UltraWinTabControl.UltraTabControl BlotterTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage BlotterTabSharedControlsPage;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl OrdersTabPageControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl WorkingSubsTabPageControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl SummaryTabPageControl;
        private OrderBlotterGrid OrderBlotterGrid;
        private WorkingSubBlotterGrid WorkingSubBlotterGrid;
        private SummaryBlotterGrid SummaryBlotterGrid;

    }
}