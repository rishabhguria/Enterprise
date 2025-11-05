using System;
using Prana.Global;
using Prana.PM.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using ExportGridsData;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlMainConsolidationView
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
            try
            {
                if (disposing)
                {
                    if (bindingSource != null)
                    {
                        bindingSource.Clear();
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (grid != null)
                    {
                        grid.Dispose();
                    }
                    if (bindingSource != null)
                    {
                        bindingSource.Dispose();
                    }
                    if (_tblView != null)
                    {
                        _tblView.Dispose();
                    }
                    if (_neutralAppearace != null)
                    {
                        _neutralAppearace.Dispose();
                    }
                    if (_positiveAppearace != null)
                    {
                        _positiveAppearace.Dispose();
                    }
                    if (_negativeAppearace != null)
                    {
                        _negativeAppearace.Dispose();
                    }
                    if (_excelUtil != null)
                    {
                        _excelUtil.Dispose();
                    }
                    _isInitialized = false;

                    this.expandCollapseLevel1ToolStripMenuItem.Click -= new System.EventHandler(this.expandCollapseLevel1ToolStripMenuItem_Click);
                    this.expandCollapseLevel2ToolStripMenuItem.Click -= new System.EventHandler(this.expandCollapseLevel2ToolStripMenuItem_Click);
                    this.expandCollapseLevel3ToolStripMenuItem.Click -= new System.EventHandler(this.expandCollapseLevel3ToolStripMenuItem_Click);
                    this.expandCollapseLevel4ToolStripMenuItem.Click -= new System.EventHandler(this.expandCollapseLevel4ToolStripMenuItem_Click);
                    this.expandCollapseLevel5ToolStripMenuItem.Click -= new System.EventHandler(this.expandCollapseLevel5ToolStripMenuItem_Click);

                    if(_secMasterSyncService != null)
                    _secMasterSyncService.Dispose();
                }
                base.Dispose(disposing);
                InstanceManager.ReleaseInstance(typeof(CtrlMainConsolidationView));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlMainConsolidationView));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            this.appearanceChoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pricingInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.customViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.expandCollapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandCollapseLevel1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandCollapseLevel2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandCollapseLevel3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandCollapseLevel4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandCollapseLevel5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideDashboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IncreasePositionStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           // this.closetaxlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IncreaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DecreaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         //   this.symbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           // this.accountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pttToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grdConsolidation = new PranaUltraGrid();
            
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmbPositionCategory = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblPositionCategory = new Infragistics.Win.Misc.UltraLabel();
            this.lblExandCollapse = new Infragistics.Win.Misc.UltraLabel();
            this.timerFindRow = new System.Windows.Forms.Timer(this.components);
            this.ultraDataSourcegrdConsolidation = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdConsolidation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPositionCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSourcegrdConsolidation)).BeginInit();
            this.SuspendLayout();
            // 
            // pricingInputToolStripMenuItem
            // 
            this.pricingInputToolStripMenuItem.Name = "pricingInputToolStripMenuItem";
            this.pricingInputToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.pricingInputToolStripMenuItem.Text = "Pricing Inputs";
            this.pricingInputToolStripMenuItem.Click += new System.EventHandler(this.pricingInputToolStripMenuItem_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pttToolStripMenuItem,   
            this.exitAllToolStripMenuItem,
            this.IncreasePositionStripMenuItem,
            this.toolStripSeparator1,
            this.expandCollapseToolStripMenuItem,
            this.clearFiltersToolStripMenuItem,
            this.saveLayoutToolStripMenuItem,
            //this.showSummaryToolStripMenuItem,
            //this.hideSummaryToolStripMenuItem,  
            this.showHideDashboardToolStripMenuItem,
            this.appearanceChoiceToolStripMenuItem,
            this.toolStripSeparator3,
            //this.addNewConsolidationViewToolStripMenuItem,
            //this.deleteViewToolStripMenuItem,
          //  this.renameViewToolStripMenuItem,
            this.customViewToolStripMenuItem,
            this.toolStripSeparator4,
            this.exportToolStripMenuItem,
           // this.exportToExcelToolStripMenuItem,
           // this.exportToCSVToolStripMenuItem,
            this.toolStripSeparator5,            
            this.pricingInputToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip2";
            this.contextMenuStrip.Size = new System.Drawing.Size(191, 452);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // addNewConsolidationViewToolStripMenuItem
            // 
            this.addNewViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNewConsolidationViewToolStripMenuItem.Image")));
            this.addNewViewToolStripMenuItem.Name = "addNewConsolidationViewToolStripMenuItem";
            this.addNewViewToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.addNewViewToolStripMenuItem.Text = "Add";
            this.addNewViewToolStripMenuItem.Click += new System.EventHandler(this.addNewConsolidationViewToolStripMenuItem_Click);
            // 
            // deleteViewToolStripMenuItem
            // 
            this.deleteViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteViewToolStripMenuItem.Image")));
            this.deleteViewToolStripMenuItem.Name = "deleteViewToolStripMenuItem";
            this.deleteViewToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.deleteViewToolStripMenuItem.Text = "Delete";
            this.deleteViewToolStripMenuItem.Click += new System.EventHandler(this.deleteViewToolStripMenuItem_Click);
            // 
            // renameViewToolStripMenuItem
            // 
            this.renameViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameViewToolStripMenuItem.Image")));
            this.renameViewToolStripMenuItem.Name = "renameViewToolStripMenuItem";
            this.renameViewToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.renameViewToolStripMenuItem.Text = "Rename";
            this.renameViewToolStripMenuItem.Click += new System.EventHandler(this.renameViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // expandCollapseToolStripMenuItem
            // 
            this.expandCollapseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandCollapseLevel1ToolStripMenuItem,
            this.expandCollapseLevel2ToolStripMenuItem,
            this.expandCollapseLevel3ToolStripMenuItem,
            this.expandCollapseLevel4ToolStripMenuItem,
            this.expandCollapseLevel5ToolStripMenuItem});
            this.expandCollapseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("expandCollapseToolStripMenuItem.Image")));
            this.expandCollapseToolStripMenuItem.Name = "expandCollapseToolStripMenuItem";
            this.expandCollapseToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.expandCollapseToolStripMenuItem.Text = "Expand/Collapse";
            this.expandCollapseToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseToolStripMenuItem_Click);
            // 
            // ExportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToExcelToolStripMenuItem,
            this.exportToCSVToolStripMenuItem});
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exportToolStripMenuItem.Text = "Export";
            //
            // CustomViewToolStripMenuItem
            // 
            this.customViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewViewToolStripMenuItem,
            this.renameViewToolStripMenuItem,
            this.deleteViewToolStripMenuItem});
            this.customViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("customViewToolStripMenuItem.Image")));
            this.customViewToolStripMenuItem.Name = "customViewToolStripMenuItem";
            this.customViewToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.customViewToolStripMenuItem.Text = "Custom View";
            //
            // expandCollapseLevel1ToolStripMenuItem
            // 
            this.expandCollapseLevel1ToolStripMenuItem.Name = "expandCollapseLevel1ToolStripMenuItem";
            this.expandCollapseLevel1ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.expandCollapseLevel1ToolStripMenuItem.Text = "Level 1";
            this.expandCollapseLevel1ToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseLevel1ToolStripMenuItem_Click);
            // 
            // expandCollapseLevel2ToolStripMenuItem
            // 
            this.expandCollapseLevel2ToolStripMenuItem.Name = "expandCollapseLevel2ToolStripMenuItem";
            this.expandCollapseLevel2ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.expandCollapseLevel2ToolStripMenuItem.Text = "Level 2";
            this.expandCollapseLevel2ToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseLevel2ToolStripMenuItem_Click);
            // 
            // expandCollapseLevel3ToolStripMenuItem
            // 
            this.expandCollapseLevel3ToolStripMenuItem.Name = "expandCollapseLevel3ToolStripMenuItem";
            this.expandCollapseLevel3ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.expandCollapseLevel3ToolStripMenuItem.Text = "Level 3";
            this.expandCollapseLevel3ToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseLevel3ToolStripMenuItem_Click);
            // 
            // expandCollapseLevel4ToolStripMenuItem
            // 
            this.expandCollapseLevel4ToolStripMenuItem.Name = "expandCollapseLevel4ToolStripMenuItem";
            this.expandCollapseLevel4ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.expandCollapseLevel4ToolStripMenuItem.Text = "Level 4";
            this.expandCollapseLevel4ToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseLevel4ToolStripMenuItem_Click);
            // 
            // expandCollapseLevel5ToolStripMenuItem
            // 
            this.expandCollapseLevel5ToolStripMenuItem.Name = "expandCollapseLevel5ToolStripMenuItem";
            this.expandCollapseLevel5ToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.expandCollapseLevel5ToolStripMenuItem.Text = "Level 5";
            this.expandCollapseLevel5ToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseLevel5ToolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asDefaultToolStripMenuItem,
            this.currentToolStripMenuItem,
            this.forAllToolStripMenuItem1});
            this.saveLayoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveLayoutToolStripMenuItem.Image")));
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            // 
            // asDefaultToolStripMenuItem
            // 
            this.asDefaultToolStripMenuItem.Name = "asDefaultToolStripMenuItem";
            this.asDefaultToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.asDefaultToolStripMenuItem.Text = "As Default";
            this.asDefaultToolStripMenuItem.Click += new System.EventHandler(this.asDefaultToolStripMenuItem_Click);
            // 
            // currentToolStripMenuItem
            // 
            this.currentToolStripMenuItem.Name = "currentToolStripMenuItem";
            this.currentToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.currentToolStripMenuItem.ShowShortcutKeys = false;
            this.currentToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.currentToolStripMenuItem.Text = "Current";
            this.currentToolStripMenuItem.Click += new System.EventHandler(this.currentToolStripMenuItem_Click);
            // 
            // forAllToolStripMenuItem1
            // 
            this.forAllToolStripMenuItem1.Name = "forAllToolStripMenuItem1";
            this.forAllToolStripMenuItem1.Size = new System.Drawing.Size(128, 22);
            this.forAllToolStripMenuItem1.Text = "All";
            this.forAllToolStripMenuItem1.Click += new System.EventHandler(this.forAllToolStripMenuItem1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
            // 
            // clearFiltersToolStripMenuItem
            // 
            this.clearFiltersToolStripMenuItem.Image = global::Prana.PM.Client.UI.Properties.Resources.ClearFilter;
            this.clearFiltersToolStripMenuItem.Name = "clearFiltersToolStripMenuItem";
            this.clearFiltersToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.clearFiltersToolStripMenuItem.Text = "Remove Filter";
            this.clearFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearFiltersToolStripMenuItem_Click);
            //
            // showDashboardToolStripMenuItem
            //
            this.showHideDashboardToolStripMenuItem.Name = "showDashboardToolStripMenuItem";
            this.showHideDashboardToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.showHideDashboardToolStripMenuItem.Click += new System.EventHandler(this.showHideDashboardToolStripMenuItem_Click);
            // 
            // showSummaryToolStripMenuItem
            // 
            this.showSummaryToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showSummaryToolStripMenuItem.Image")));
            this.showSummaryToolStripMenuItem.Name = "showSummaryToolStripMenuItem";
            this.showSummaryToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.showSummaryToolStripMenuItem.Text = "Show Summary Tool";
            this.showSummaryToolStripMenuItem.Click += new System.EventHandler(this.showSummaryToolStripMenuItem_Click);
            // 
            // hideSummaryToolStripMenuItem
            // 
            this.hideSummaryToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("hideSummaryToolStripMenuItem.Image")));
            this.hideSummaryToolStripMenuItem.Name = "hideSummaryToolStripMenuItem";
            this.hideSummaryToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.hideSummaryToolStripMenuItem.Text = "Hide Summary Tool";
            this.hideSummaryToolStripMenuItem.Click += new System.EventHandler(this.hideSummaryToolStripMenuItem_Click);
            // 
            // appearanceToolStripMenuItem
            // 
            this.appearanceChoiceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("appearanceChoiceToolStripMenuItem.Image")));
            this.appearanceChoiceToolStripMenuItem.Name = "appearanceChoiceToolStripMenuItem";
            this.appearanceChoiceToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.appearanceChoiceToolStripMenuItem.Text = "Appearance";
            this.appearanceChoiceToolStripMenuItem.Click += new System.EventHandler(this.AppearanceChoiceToolStripMenuItem_Click);
            //this.appearanceChoiceToolStripMenuItem.Image = ((System.Drawing.Bitmap)(C:\\Users\\sahil.nara\\Desktop\\setting.PNG);
            //
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
            // 
            // closeAllToolStripMenuItem
            // 
            //this.closeAllToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            ////this.addToolStripMenuItem,
            //this.accountToolStripMenuItem});
            this.exitAllToolStripMenuItem.Image = global::Prana.PM.Client.UI.Properties.Resources.trade;
            this.exitAllToolStripMenuItem.Name = "exitAllToolStripMenuItem";
            this.exitAllToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exitAllToolStripMenuItem.Text = "Exit Position";
            this.exitAllToolStripMenuItem.Click += new System.EventHandler(this.ClosePosition_Click);
            // 
            // IncreasePositionStripMenuItem
            // 
            this.IncreasePositionStripMenuItem.Name = "IncreasePositionStripMenuItem";
            this.IncreasePositionStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.IncreasePositionStripMenuItem.Text = "Increase Position";
            this.IncreasePositionStripMenuItem.Click += IncreasePositionStripMenuItem_Click;
            // 
            // addToolStripMenuItem
            // 
            //this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            //this.addToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            //this.addToolStripMenuItem.Text = "Add";
            //this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // closetaxlotToolStripMenuItem
            // 
            //this.closetaxlotToolStripMenuItem.Name = "closetaxlotToolStripMenuItem";
            //this.closetaxlotToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            //this.closetaxlotToolStripMenuItem.Text = "Close Selected Taxlot";
            //this.closetaxlotToolStripMenuItem.Click += new System.EventHandler(this.closetaxlotToolStripMenuItem_Click);
            // 
            // symbolToolStripMenuItem
            // 
            //this.symbolToolStripMenuItem.Name = "symbolToolStripMenuItem";
            //this.symbolToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            //this.symbolToolStripMenuItem.Text = "Close Position";
            //this.symbolToolStripMenuItem.Click += new System.EventHandler(this.symbolToolStripMenuItem_Click);
            // 
            // accountToolStripMenuItem
            // 
            //this.accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            //this.accountToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            //this.accountToolStripMenuItem.Text = "Close Account Position";
            //this.accountToolStripMenuItem.Click += new System.EventHandler(this.accountToolStripMenuItem_Click);
            //
            //IncreaseToolStripMenuItem
            //
            this.IncreaseToolStripMenuItem.Name = "IncreaseToolStripMenuItem";
            this.IncreaseToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.IncreaseToolStripMenuItem.Text = "Increase";
            this.IncreaseToolStripMenuItem.Click += delegate(object sender, EventArgs e) { pttToolStripMenuItem_Click(sender, e, "Increase"); };
            //
            //DecreaseToolStripMenuItem
            //
            this.DecreaseToolStripMenuItem.Name = "DecreaseToolStripMenuItem";
            this.DecreaseToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.DecreaseToolStripMenuItem.Text = "Decrease";
            this.DecreaseToolStripMenuItem.Click += delegate(object sender, EventArgs e) { pttToolStripMenuItem_Click(sender, e, "Decrease"); };
            //
            //SetToolStripMenuItem
            //
            this.SetToolStripMenuItem.Name = "SetToolStripMenuItem";
            this.SetToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.SetToolStripMenuItem.Text = "Set";
            this.SetToolStripMenuItem.Click += delegate(object sender, EventArgs e) { pttToolStripMenuItem_Click(sender, e, "Set"); };
            // 
            // pttToolStripMenuItem
            // 
            this.pttToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.IncreaseToolStripMenuItem,
            this.DecreaseToolStripMenuItem,
            this.SetToolStripMenuItem});
            this.pttToolStripMenuItem.Name = "pttToolStripMenuItem";
            this.pttToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.pttToolStripMenuItem.Text = "Adjust Position";
        //    this.pttToolStripMenuItem.Click += new System.EventHandler(this.pttToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(187, 6);
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exportToExcelToolStripMenuItem.Text = "Excel";
            this.exportToExcelToolStripMenuItem.Click += new System.EventHandler(this.exportToExcelToolStripMenuItem_Click);
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exportToCSVToolStripMenuItem.Text = "CSV";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // grdConsolidation
            // 
            this.grdConsolidation.ContextMenuStrip = this.contextMenuStrip;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdConsolidation.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Indentation = 0;
            appearance2.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance2.BackColor2 = System.Drawing.Color.DarkGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            ultraGridBand1.Override.HeaderAppearance = appearance2;
            ultraGridBand1.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance3.TextVAlignAsString = "Middle";
            ultraGridBand1.Override.RowAlternateAppearance = appearance3;
            appearance4.TextVAlignAsString = "Middle";
            ultraGridBand1.Override.RowAppearance = appearance4;
            this.grdConsolidation.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdConsolidation.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdConsolidation.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance5.BackColor = System.Drawing.Color.Silver;
            appearance5.BackColor2 = System.Drawing.Color.DimGray;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdConsolidation.DisplayLayout.GroupByBox.Appearance = appearance5;
            this.grdConsolidation.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdConsolidation.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.Black;
            this.grdConsolidation.DisplayLayout.GroupByBox.ButtonConnectorStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdConsolidation.DisplayLayout.MaxBandDepth = 4;
            this.grdConsolidation.DisplayLayout.MaxColScrollRegions = 1;
            this.grdConsolidation.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdConsolidation.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdConsolidation.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdConsolidation.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.grdConsolidation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdConsolidation.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.grdConsolidation.DisplayLayout.Override.CellPadding = 0;
            this.grdConsolidation.DisplayLayout.Override.CellSpacing = 0;
            this.grdConsolidation.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdConsolidation.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            this.grdConsolidation.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.grdConsolidation.DisplayLayout.Override.GroupByRowPadding = 0;
            this.grdConsolidation.DisplayLayout.Override.GroupByRowSpacingAfter = 0;
            this.grdConsolidation.DisplayLayout.Override.GroupByRowSpacingBefore = 0;
            this.grdConsolidation.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;

            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.TextHAlignAsString = "Right";
            appearance7.TextVAlignAsString = "Middle";
            this.grdConsolidation.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance7;
            appearance8.FontData.BoldAsString = "False";
            appearance8.FontData.SizeInPoints = 8F;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.grdConsolidation.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdConsolidation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdConsolidation.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdConsolidation.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.TextHAlignAsString = "Right";
            appearance9.TextVAlignAsString = "Middle";
            this.grdConsolidation.DisplayLayout.Override.RowAlternateAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.TextHAlignAsString = "Right";
            appearance10.TextVAlignAsString = "Middle";
            this.grdConsolidation.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdConsolidation.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdConsolidation.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            appearance11.FontData.BoldAsString = "True";
            this.grdConsolidation.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this.grdConsolidation.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdConsolidation.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdConsolidation.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdConsolidation.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdConsolidation.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdConsolidation.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdConsolidation.DisplayLayout.Override.SummaryDisplayArea = ((Infragistics.Win.UltraWinGrid.SummaryDisplayAreas)((Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows | Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.RootRowsFootersOnly)));
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            this.grdConsolidation.DisplayLayout.Override.SummaryFooterAppearance = appearance12;
            this.grdConsolidation.DisplayLayout.Override.SummaryFooterSpacingAfter = 0;
            this.grdConsolidation.DisplayLayout.Override.SummaryFooterSpacingBefore = 0;
            appearance13.BackColor = System.Drawing.Color.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            appearance13.FontData.BoldAsString = "False";
            appearance13.ForeColor = System.Drawing.Color.White;
            this.grdConsolidation.DisplayLayout.Override.SummaryValueAppearance = appearance13;
            this.grdConsolidation.DisplayLayout.PriorityScrolling = true;
            this.grdConsolidation.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdConsolidation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdConsolidation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdConsolidation.DisplayLayout.UseFixedHeaders = true;
            this.grdConsolidation.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdConsolidation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdConsolidation.Location = new System.Drawing.Point(0, 0);
            this.grdConsolidation.MinimumSize = new System.Drawing.Size(100, 10);
            this.grdConsolidation.Name = "grdConsolidation";
            this.grdConsolidation.Size = new System.Drawing.Size(946, 354);
            this.grdConsolidation.TabIndex = 53;
            this.grdConsolidation.Text = "ConsolidationViewGrid";
            this.grdConsolidation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdConsolidation.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdConsolidation_InitializeLayout);
            this.grdConsolidation.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdConsolidation_InitializeRow);
            this.grdConsolidation.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdConsolidation_InitializeGroupByRow);
            this.grdConsolidation.AfterRowActivate += new System.EventHandler(this.grdConsolidation_AfterRowActivate);
            this.grdConsolidation.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.grdConsolidation_BeforeRowDeactivate);
            this.grdConsolidation.InitializePrint += new Infragistics.Win.UltraWinGrid.InitializePrintEventHandler(this.grdConsolidation_InitializePrint);
            this.grdConsolidation.BeforeSortChange += new Infragistics.Win.UltraWinGrid.BeforeSortChangeEventHandler(this.grdConsolidation_BeforeSortChange);
            this.grdConsolidation.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdConsolidation_AfterSortChange);
            this.grdConsolidation.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.grdConsolidation_AfterColPosChanged);
            this.grdConsolidation.AfterRowFilterChanged += new Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventHandler(this.grdConsolidation_AfterRowFilterChanged);
            this.grdConsolidation.BeforeRowFilterDropDown += new Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventHandler(this.grdConsolidation_BeforeRowFilterDropDown);
            this.grdConsolidation.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdConsolidation_BeforeCustomRowFilterDialog);
            this.grdConsolidation.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdConsolidation_BeforeColumnChooserDisplayed);
            this.grdConsolidation.AfterRowFilterDropDownPopulate += new Infragistics.Win.UltraWinGrid.AfterRowFilterDropDownPopulateEventHandler(this.grdConsolidation_AfterRowFilterDropDownPopulate);
            this.grdConsolidation.DoubleClickRow += grdConsolidation_DoubleClickRow;
            this.grdConsolidation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdConsolidation_KeyDown);
            this.grdConsolidation.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdConsolidation_MouseClick);
            this.grdConsolidation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdConsolidation_MouseDown);
            this.grdConsolidation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdConsolidation_MouseUp);
            this.grdConsolidation.SyncWithCurrencyManager = false;
            // 
            // ultraGridFilterUIProvider
            // 
            this.ultraGridFilterUIProvider = new Infragistics.Win.SupportDialogs.FilterUIProvider.UltraGridFilterUIProvider(this.components);
            
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cmbPositionCategory);
            this.splitContainer1.Panel1.Controls.Add(this.lblPositionCategory);
            this.splitContainer1.Panel1.Controls.Add(this.lblExandCollapse);
            
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdConsolidation);
            this.splitContainer1.Size = new System.Drawing.Size(946, 380);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 59;
            // 
            // cmbPositionCategory
            // 
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbPositionCategory.DisplayLayout.Appearance = appearance19;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbPositionCategory.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbPositionCategory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbPositionCategory.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbPositionCategory.DisplayLayout.GroupByBox.Appearance = appearance20;
            appearance21.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbPositionCategory.DisplayLayout.GroupByBox.BandLabelAppearance = appearance21;
            this.cmbPositionCategory.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance22.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance22.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbPositionCategory.DisplayLayout.GroupByBox.PromptAppearance = appearance22;
            this.cmbPositionCategory.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbPositionCategory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbPositionCategory.DisplayLayout.Override.ActiveCellAppearance = appearance23;
            appearance24.BackColor = System.Drawing.SystemColors.Highlight;
            appearance24.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbPositionCategory.DisplayLayout.Override.ActiveRowAppearance = appearance24;
            this.cmbPositionCategory.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbPositionCategory.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            this.cmbPositionCategory.DisplayLayout.Override.CardAreaAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Silver;
            appearance26.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbPositionCategory.DisplayLayout.Override.CellAppearance = appearance26;
            this.cmbPositionCategory.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbPositionCategory.DisplayLayout.Override.CellPadding = 0;
            appearance27.BackColor = System.Drawing.SystemColors.Control;
            appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance27.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance27.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbPositionCategory.DisplayLayout.Override.GroupByRowAppearance = appearance27;
            appearance28.TextHAlignAsString = "Left";
            this.cmbPositionCategory.DisplayLayout.Override.HeaderAppearance = appearance28;
            this.cmbPositionCategory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbPositionCategory.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.BorderColor = System.Drawing.Color.Silver;
            this.cmbPositionCategory.DisplayLayout.Override.RowAppearance = appearance29;
            this.cmbPositionCategory.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance30.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbPositionCategory.DisplayLayout.Override.TemplateAddRowAppearance = appearance30;
            this.cmbPositionCategory.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbPositionCategory.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbPositionCategory.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbPositionCategory.Location = new System.Drawing.Point(701, 3);
            this.cmbPositionCategory.Name = "cmbPositionCategory";
            this.cmbPositionCategory.Size = new System.Drawing.Size(100, 23);
            this.cmbPositionCategory.TabIndex = 60;
            this.cmbPositionCategory.Visible = false;
            // 
            // lblPositionCategory
            // 
            appearance31.FontData.SizeInPoints = 10F;
            appearance31.ForeColor = System.Drawing.Color.White;
            this.lblPositionCategory.Appearance = appearance31;
            this.lblPositionCategory.AutoSize = true;
            this.lblPositionCategory.Location = new System.Drawing.Point(640, 7);
            this.lblPositionCategory.Name = "lblPositionCategory";
            this.lblPositionCategory.Size = new System.Drawing.Size(58, 18);
            this.lblPositionCategory.TabIndex = 57;
            this.lblPositionCategory.Text = "Positions";
            this.lblPositionCategory.Visible = false;
            // 
            // lblExandCollapse
            // 
            appearance32.BorderColor = System.Drawing.Color.Black;
            appearance32.BorderColor2 = System.Drawing.Color.Black;
            appearance32.FontData.SizeInPoints = 10F;
            appearance32.ForeColor = System.Drawing.Color.White;
            this.lblExandCollapse.Appearance = appearance32;
            this.lblExandCollapse.AutoSize = true;
            this.lblExandCollapse.Location = new System.Drawing.Point(3, 4);
            this.lblExandCollapse.Name = "lblExandCollapse";
            this.lblExandCollapse.Size = new System.Drawing.Size(217, 18);
            this.lblExandCollapse.TabIndex = 57;
            this.lblExandCollapse.Text = "Expand/Collapse Grouping @ Level";
            // 
            // timerFindRow
            // 
            this.timerFindRow.Interval = 750;
            this.timerFindRow.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CtrlMainConsolidationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CtrlMainConsolidationView";
            this.Size = new System.Drawing.Size(946, 380);
            this.Load += new System.EventHandler(this.CtrlMainConsolidationView_Load);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdConsolidation)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPositionCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSourcegrdConsolidation)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private PranaUltraGrid grdConsolidation;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameViewToolStripMenuItem;
        
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraLabel lblExandCollapse;
        private Infragistics.Win.Misc.UltraLabel lblPositionCategory;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbPositionCategory;
        private System.Windows.Forms.ToolStripMenuItem clearFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHideDashboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideSummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem IncreasePositionStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pttToolStripMenuItem;
      //  private System.Windows.Forms.ToolStripMenuItem symbolToolStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
        //private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
       // private System.Windows.Forms.ToolStripMenuItem closetaxlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem IncreaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DecreaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        //private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.Timer timerFindRow;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSourcegrdConsolidation;
        private System.Windows.Forms.ToolStripMenuItem pricingInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appearanceChoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseLevel1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseLevel2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseLevel3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseLevel4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseLevel5ToolStripMenuItem;
        private Infragistics.Win.SupportDialogs.FilterUIProvider.UltraGridFilterUIProvider ultraGridFilterUIProvider;
    }
}
