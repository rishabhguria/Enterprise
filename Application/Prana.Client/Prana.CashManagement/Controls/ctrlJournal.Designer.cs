using Prana.LogManager;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using System;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.CashManagement
{
    partial class ctrlJournal
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
                    if (_proxy != null && _proxy.InnerChannel != null)
                    {
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_CashData);
                        _proxy.Dispose();
						_proxy = null;
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    _subAccount[0] = null;
                    _subAccount[1] = null;
                    _subAccount[2] = null;
                    _subAccount[3] = null;
                    CashAccountsUI.SubAccountUpdated -= new EventHandler(CashAccountsUI_SubAccountUpdated);
                    if (_columnSorted != null)
                    {
                        _columnSorted.Dispose();
                    }
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            this.ultraTabPageTradingTrans = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelTradingTrans = new Infragistics.Win.Misc.UltraPanel();
            this.grdTradingTransactions = new PranaUltraGrid();
            this.menuStripCashValue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTranscationToolStripMenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTranscationItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTranscationToolStripMenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTranscationItemtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowAllLegstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripTradingTrans = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraTabPageNonTradingTrans = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelNonTradingTrans = new Infragistics.Win.Misc.UltraPanel();
            this.grdNonTradingTransactions = new PranaUltraGrid();
            this.statusStripNonTrading = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraTabPageDividendTrans = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelDividend = new Infragistics.Win.Misc.UltraPanel();
            this.grdDividend = new PranaUltraGrid();
            this.statusStripDividend = new System.Windows.Forms.StatusStrip();
            this.statusStripLabelDividend = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraTabPageRevaluation = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelRevaluation = new Infragistics.Win.Misc.UltraPanel();
            this.grdRevaluation = new PranaUltraGrid();
            this.statusStripRevaluation = new System.Windows.Forms.StatusStrip();
            this.toolStripRevaluation = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraTabPageOpeningBalance = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelOpeningBal = new Infragistics.Win.Misc.UltraPanel();
            this.grdOpeningBalance = new PranaUltraGrid();
            this.statusStripOpeningBal = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.miniToolStrip = new System.Windows.Forms.StatusStrip();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControlCashMainValue = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ugbxJournalParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.dtPickerUpper = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnGetCash = new Infragistics.Win.Misc.UltraButton();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.dtPickerlower = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl13 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl14 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl15 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.upnlBody = new Infragistics.Win.Misc.UltraPanel();
            this.upnlBodyTop = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraTabPageTradingTrans.SuspendLayout();
            this.ultraPanelTradingTrans.ClientArea.SuspendLayout();
            this.ultraPanelTradingTrans.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTradingTransactions)).BeginInit();
            this.menuStripCashValue.SuspendLayout();
            this.statusStripTradingTrans.SuspendLayout();
            this.ultraTabPageNonTradingTrans.SuspendLayout();
            this.ultraPanelNonTradingTrans.ClientArea.SuspendLayout();
            this.ultraPanelNonTradingTrans.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNonTradingTransactions)).BeginInit();
            this.statusStripNonTrading.SuspendLayout();
            this.ultraTabPageDividendTrans.SuspendLayout();
            this.ultraPanelDividend.ClientArea.SuspendLayout();
            this.ultraPanelDividend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDividend)).BeginInit();
            this.statusStripDividend.SuspendLayout();
            this.ultraTabPageRevaluation.SuspendLayout();
            this.ultraPanelRevaluation.ClientArea.SuspendLayout();
            this.ultraPanelRevaluation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRevaluation)).BeginInit();
            this.statusStripRevaluation.SuspendLayout();
            this.ultraTabPageOpeningBalance.SuspendLayout();
            this.ultraPanelOpeningBal.ClientArea.SuspendLayout();
            this.ultraPanelOpeningBal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOpeningBalance)).BeginInit();
            this.statusStripOpeningBal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControlCashMainValue)).BeginInit();
            this.ultraTabControlCashMainValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJournalParams)).BeginInit();
            this.ugbxJournalParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerlower)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.upnlBody.ClientArea.SuspendLayout();
            this.upnlBody.SuspendLayout();
            this.upnlBodyTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageTradingTrans
            // 
            this.ultraTabPageTradingTrans.Controls.Add(this.ultraPanelTradingTrans);
            this.ultraTabPageTradingTrans.Controls.Add(this.statusStripTradingTrans);
            this.ultraTabPageTradingTrans.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageTradingTrans.Name = "ultraTabPageTradingTrans";
            this.ultraTabPageTradingTrans.Size = new System.Drawing.Size(1073, 359);
            // 
            // ultraPanelTradingTrans
            // 
            // 
            // ultraPanelTradingTrans.ClientArea
            // 
            this.ultraPanelTradingTrans.ClientArea.Controls.Add(this.grdTradingTransactions);
            this.ultraPanelTradingTrans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelTradingTrans.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelTradingTrans.Name = "ultraPanelTradingTrans";
            this.ultraPanelTradingTrans.Size = new System.Drawing.Size(1073, 337);
            this.ultraPanelTradingTrans.TabIndex = 2;
            // 
            // grdTradingTransactions
            // 
            this.grdTradingTransactions.ContextMenuStrip = this.menuStripCashValue;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdTradingTransactions.DisplayLayout.Appearance = appearance1;
            this.grdTradingTransactions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTradingTransactions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdTradingTransactions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdTradingTransactions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTradingTransactions.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdTradingTransactions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdTradingTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdTradingTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdTradingTransactions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdTradingTransactions.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdTradingTransactions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdTradingTransactions.DisplayLayout.Override.CellPadding = 0;
            this.grdTradingTransactions.DisplayLayout.Override.CellSpacing = 0;
            this.grdTradingTransactions.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance2.FontData.Name = "Segoe UI";
            appearance2.FontData.SizeInPoints = 9F;
            appearance2.TextHAlignAsString = "Center";
            this.grdTradingTransactions.DisplayLayout.Override.HeaderAppearance = appearance2;
            this.grdTradingTransactions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdTradingTransactions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdTradingTransactions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdTradingTransactions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            appearance3.FontData.BoldAsString = "True";
            this.grdTradingTransactions.DisplayLayout.Override.SelectedRowAppearance = appearance3;
            this.grdTradingTransactions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdTradingTransactions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdTradingTransactions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTradingTransactions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdTradingTransactions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdTradingTransactions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdTradingTransactions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdTradingTransactions.DisplayLayout.Override.TemplateAddRowAppearance = appearance4;
            this.grdTradingTransactions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTradingTransactions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdTradingTransactions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdTradingTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTradingTransactions.ExitEditModeOnLeave = false;
            this.grdTradingTransactions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdTradingTransactions.Location = new System.Drawing.Point(0, 0);
            this.grdTradingTransactions.Name = "grdTradingTransactions";
            this.grdTradingTransactions.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdTradingTransactions.Size = new System.Drawing.Size(1073, 337);
            this.grdTradingTransactions.TabIndex = 0;
            this.grdTradingTransactions.Text = "Cash Journal";
            this.grdTradingTransactions.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdTradingTransactions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdTradingTransactions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdTradingTransactions_InitializeLayout);
            this.grdTradingTransactions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdTradingTransactions_InitializeRow);
            this.grdTradingTransactions.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdTradingTransactions_InitializeGroupByRow);
            this.grdTradingTransactions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdTradingTransactions_AfterSortChange);
            this.grdTradingTransactions.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdTradingTransactions_BeforeCustomRowFilterDialog);
            this.grdTradingTransactions.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdTradingTransactions_BeforeColumnChooserDisplayed);
            this.grdTradingTransactions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdTradingTransactions_MouseClick);
            this.grdTradingTransactions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdTradingTransactions_MouseDown);
            this.grdTradingTransactions.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdTradingTransactions.AfterRowFilterChanged += grdTradingTransactions_AfterRowFilterChanged;      
            // 
            // menuStripCashValue
            // 
            this.menuStripCashValue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTranscationToolStripMenuitem,
            this.addTranscationItemToolStripMenuItem,
            this.deleteTranscationToolStripMenuitem,
            this.deleteTranscationItemtoolStripMenuItem,
            this.ShowAllLegstoolStripMenuItem,
            this.saveLayoutToolStripMenuItem});
            this.menuStripCashValue.Name = "menuStripCashValue";
            this.menuStripCashValue.Size = new System.Drawing.Size(213, 136);
            this.inboxControlStyler1.SetStyleSettings(this.menuStripCashValue, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.menuStripCashValue.Opening += new System.ComponentModel.CancelEventHandler(this.menuStripCashValue_Opening);
            this.menuStripCashValue.MouseEnter += new System.EventHandler(this.menuStripCashValue_MouseEnter);
            // 
            // addTranscationToolStripMenuitem
            // 
            this.addTranscationToolStripMenuitem.Name = "addTranscationToolStripMenuitem";
            this.addTranscationToolStripMenuitem.Size = new System.Drawing.Size(212, 22);
            this.addTranscationToolStripMenuitem.Text = "Add Transaction";
            this.addTranscationToolStripMenuitem.Click += new System.EventHandler(this.addTranscationToolStripMenuitem_Click);
            // 
            // addTranscationItemToolStripMenuItem
            // 
            this.addTranscationItemToolStripMenuItem.Name = "addTranscationItemToolStripMenuItem";
            this.addTranscationItemToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.addTranscationItemToolStripMenuItem.Text = "Add Transaction Item";
            this.addTranscationItemToolStripMenuItem.Click += new System.EventHandler(this.addRowToolStripMenuItem_Click);
            // 
            // deleteTranscationToolStripMenuitem
            // 
            this.deleteTranscationToolStripMenuitem.Name = "deleteTranscationToolStripMenuitem";
            this.deleteTranscationToolStripMenuitem.Size = new System.Drawing.Size(212, 22);
            this.deleteTranscationToolStripMenuitem.Text = "Delete Transaction";
            this.deleteTranscationToolStripMenuitem.Click += new System.EventHandler(this.deleteTranscationToolStripMenuitem_Click);
            // 
            // deleteTranscationItemtoolStripMenuItem
            // 
            this.deleteTranscationItemtoolStripMenuItem.Name = "deleteTranscationItemtoolStripMenuItem";
            this.deleteTranscationItemtoolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.deleteTranscationItemtoolStripMenuItem.Text = "Delete Transaction Item";
            this.deleteTranscationItemtoolStripMenuItem.Click += new System.EventHandler(this.deleteRowtoolStripMenuItem_Click);
            // 
            // ShowAllLegstoolStripMenuItem
            // 
            this.ShowAllLegstoolStripMenuItem.Name = "ShowAllLegstoolStripMenuItem";
            this.ShowAllLegstoolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.ShowAllLegstoolStripMenuItem.Text = "Show All Transaction Legs";
            this.ShowAllLegstoolStripMenuItem.Click += new System.EventHandler(this.ShowAllLegstoolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutCurrentToolStripMenuItem,
            this.saveLayoutAllToolStripMenuItem});
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            // 
            // saveLayoutCurrentToolStripMenuItem
            // 
            this.saveLayoutCurrentToolStripMenuItem.Name = "saveLayoutCurrentToolStripMenuItem";
            this.saveLayoutCurrentToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveLayoutCurrentToolStripMenuItem.Text = "Current";
            this.saveLayoutCurrentToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutCurrentToolStripMenuItem_Click);
            // 
            // saveLayoutAllToolStripMenuItem
            // 
            this.saveLayoutAllToolStripMenuItem.Name = "saveLayoutAllToolStripMenuItem";
            this.saveLayoutAllToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveLayoutAllToolStripMenuItem.Text = "All";
            this.saveLayoutAllToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutAllToolStripMenuItem_Click);
            // 
            // statusStripTradingTrans
            // 
            this.statusStripTradingTrans.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStripTradingTrans.Location = new System.Drawing.Point(0, 337);
            this.statusStripTradingTrans.Name = "statusStripTradingTrans";
            this.statusStripTradingTrans.Size = new System.Drawing.Size(1073, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStripTradingTrans, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStripTradingTrans.TabIndex = 1;
            this.statusStripTradingTrans.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraTabPageNonTradingTrans
            // 
            this.ultraTabPageNonTradingTrans.Controls.Add(this.ultraPanelNonTradingTrans);
            this.ultraTabPageNonTradingTrans.Controls.Add(this.statusStripNonTrading);
            this.ultraTabPageNonTradingTrans.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageNonTradingTrans.Name = "ultraTabPageNonTradingTrans";
            this.ultraTabPageNonTradingTrans.Size = new System.Drawing.Size(1073, 359);
            // 
            // ultraPanelNonTradingTrans
            // 
            // 
            // ultraPanelNonTradingTrans.ClientArea
            // 
            this.ultraPanelNonTradingTrans.ClientArea.Controls.Add(this.grdNonTradingTransactions);
            this.ultraPanelNonTradingTrans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelNonTradingTrans.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelNonTradingTrans.Name = "ultraPanelNonTradingTrans";
            this.ultraPanelNonTradingTrans.Size = new System.Drawing.Size(1073, 337);
            this.ultraPanelNonTradingTrans.TabIndex = 2;
            // 
            // grdNonTradingTransactions
            // 
            this.grdNonTradingTransactions.ContextMenuStrip = this.menuStripCashValue;
            appearance5.BackColor = System.Drawing.Color.Black;
            this.grdNonTradingTransactions.DisplayLayout.Appearance = appearance5;
            this.grdNonTradingTransactions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdNonTradingTransactions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdNonTradingTransactions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdNonTradingTransactions.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdNonTradingTransactions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdNonTradingTransactions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdNonTradingTransactions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdNonTradingTransactions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdNonTradingTransactions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdNonTradingTransactions.DisplayLayout.Override.CellPadding = 0;
            this.grdNonTradingTransactions.DisplayLayout.Override.CellSpacing = 0;
            this.grdNonTradingTransactions.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.grdNonTradingTransactions.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            appearance7.FontData.Name = "Segoe UI";
            appearance7.FontData.SizeInPoints = 9F;
            appearance7.TextHAlignAsString = "Center";
            this.grdNonTradingTransactions.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdNonTradingTransactions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdNonTradingTransactions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance8.BackColor = System.Drawing.Color.Black;
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.TextHAlignAsString = "Right";
            appearance8.TextVAlignAsString = "Middle";
            this.grdNonTradingTransactions.DisplayLayout.Override.RowAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            appearance9.FontData.BoldAsString = "True";
            this.grdNonTradingTransactions.DisplayLayout.Override.SelectedRowAppearance = appearance9;
            this.grdNonTradingTransactions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdNonTradingTransactions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdNonTradingTransactions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdNonTradingTransactions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdNonTradingTransactions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdNonTradingTransactions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdNonTradingTransactions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance10.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdNonTradingTransactions.DisplayLayout.Override.TemplateAddRowAppearance = appearance10;
            this.grdNonTradingTransactions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdNonTradingTransactions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdNonTradingTransactions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdNonTradingTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdNonTradingTransactions.ExitEditModeOnLeave = false;
            this.grdNonTradingTransactions.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdNonTradingTransactions.Location = new System.Drawing.Point(0, 0);
            this.grdNonTradingTransactions.Name = "grdNonTradingTransactions";
            this.grdNonTradingTransactions.Size = new System.Drawing.Size(1073, 337);
            this.grdNonTradingTransactions.TabIndex = 0;
            this.grdNonTradingTransactions.Text = "Cash Journal";
            this.grdNonTradingTransactions.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdNonTradingTransactions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdNonTradingTransactions.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdNonTradingTransactions_AfterCellUpdate);
            this.grdNonTradingTransactions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdNonTradingTransactions_InitializeLayout);
            this.grdNonTradingTransactions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdNonTradingTransactions_InitializeRow);
            this.grdNonTradingTransactions.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdNonTradingTransactions_InitializeGroupByRow);
            this.grdNonTradingTransactions.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdNonTradingTransactions_CellChange);
            this.grdNonTradingTransactions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdNonTradingTransactions_AfterSortChange);
            this.grdNonTradingTransactions.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdNonTradingTransactions_BeforeCustomRowFilterDialog);
            this.grdNonTradingTransactions.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdNonTradingTransactions_BeforeColumnChooserDisplayed);
            this.grdNonTradingTransactions.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdNonTradingTransactions_MouseClick);
            this.grdNonTradingTransactions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdNonTradingTransactions_MouseDown);
            this.grdNonTradingTransactions.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdNonTradingTransactions.AfterRowFilterChanged += grdNonTradingTransactions_AfterRowFilterChanged;
            // 
            // statusStripNonTrading
            // 
            this.statusStripNonTrading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.statusStripNonTrading.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3});
            this.statusStripNonTrading.Location = new System.Drawing.Point(0, 337);
            this.statusStripNonTrading.Name = "statusStripNonTrading";
            this.statusStripNonTrading.Size = new System.Drawing.Size(1073, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStripNonTrading, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStripNonTrading.TabIndex = 1;
            this.statusStripNonTrading.Text = "statusStripDividend";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraTabPageDividendTrans
            // 
            this.ultraTabPageDividendTrans.Controls.Add(this.ultraPanelDividend);
            this.ultraTabPageDividendTrans.Controls.Add(this.statusStripDividend);
            this.ultraTabPageDividendTrans.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageDividendTrans.Name = "ultraTabPageDividendTrans";
            this.ultraTabPageDividendTrans.Size = new System.Drawing.Size(1073, 359);
            // 
            // ultraPanelDividend
            // 
            // 
            // ultraPanelDividend.ClientArea
            // 
            this.ultraPanelDividend.ClientArea.Controls.Add(this.grdDividend);
            this.ultraPanelDividend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelDividend.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelDividend.Name = "ultraPanelDividend";
            this.ultraPanelDividend.Size = new System.Drawing.Size(1073, 337);
            this.ultraPanelDividend.TabIndex = 2;
            // 
            // grdDividend
            // 
            appearance11.BackColor = System.Drawing.Color.Black;
            this.grdDividend.DisplayLayout.Appearance = appearance11;
            this.grdDividend.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDividend.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdDividend.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDividend.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDividend.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDividend.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDividend.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdDividend.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdDividend.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdDividend.DisplayLayout.Override.CellPadding = 0;
            this.grdDividend.DisplayLayout.Override.CellSpacing = 0;
            this.grdDividend.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.ForeColor = System.Drawing.Color.White;
            this.grdDividend.DisplayLayout.Override.GroupByRowAppearance = appearance12;
            appearance13.FontData.Name = "Segoe UI";
            appearance13.FontData.SizeInPoints = 9F;
            appearance13.TextHAlignAsString = "Center";
            this.grdDividend.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.grdDividend.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDividend.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance14.BackColor = System.Drawing.Color.Black;
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextHAlignAsString = "Right";
            appearance14.TextVAlignAsString = "Middle";
            this.grdDividend.DisplayLayout.Override.RowAppearance = appearance14;
            this.grdDividend.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdDividend.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance15.BackColor = System.Drawing.Color.Transparent;
            appearance15.BorderColor = System.Drawing.Color.Transparent;
            appearance15.FontData.BoldAsString = "True";
            this.grdDividend.DisplayLayout.Override.SelectedRowAppearance = appearance15;
            this.grdDividend.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDividend.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDividend.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDividend.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDividend.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdDividend.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdDividend.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdDividend.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.grdDividend.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDividend.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDividend.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdDividend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDividend.ExitEditModeOnLeave = false;
            this.grdDividend.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdDividend.Location = new System.Drawing.Point(0, 0);
            this.grdDividend.Name = "grdDividend";
            this.grdDividend.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdDividend.Size = new System.Drawing.Size(1073, 337);
            this.grdDividend.TabIndex = 0;
            this.grdDividend.Text = "Cash Journal";
            this.grdDividend.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDividend.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdDividend.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDividend_AfterCellUpdate);
            this.grdDividend.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDividend_InitializeLayout);
            this.grdDividend.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdDividend_InitializeRow);
            this.grdDividend.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdDividend_InitializeGroupByRow);
            this.grdDividend.AfterRowActivate += new System.EventHandler(this.grdDividend_AfterRowActivate);
            this.grdDividend.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDividend_AfterRowUpdate);
            this.grdDividend.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDividend_CellChange);
            this.grdDividend.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdDividend_AfterSortChange);
            this.grdDividend.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdDividend_BeforeCustomRowFilterDialog);
            this.grdDividend.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdDividend_BeforeColumnChooserDisplayed);
            this.grdDividend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdDividend_MouseClick);
            this.grdDividend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdDividend_MouseDown);
            this.grdDividend.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdDividend.AfterRowFilterChanged += grdDividend_AfterRowFilterChanged;
            // 
            // statusStripDividend
            // 
            this.statusStripDividend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripLabelDividend});
            this.statusStripDividend.Location = new System.Drawing.Point(0, 337);
            this.statusStripDividend.Name = "statusStripDividend";
            this.statusStripDividend.Size = new System.Drawing.Size(1073, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStripDividend, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStripDividend.TabIndex = 1;
            // 
            // statusStripLabelDividend
            // 
            this.statusStripLabelDividend.Name = "statusStripLabelDividend";
            this.statusStripLabelDividend.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraTabPageRevaluation
            // 
            this.ultraTabPageRevaluation.Controls.Add(this.ultraPanelRevaluation);
            this.ultraTabPageRevaluation.Controls.Add(this.statusStripRevaluation);
            this.ultraTabPageRevaluation.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageRevaluation.Name = "ultraTabPageRevaluation";
            this.ultraTabPageRevaluation.Size = new System.Drawing.Size(1073, 359);
            // 
            // ultraPanelRevaluation
            // 
            // 
            // ultraPanelRevaluation.ClientArea
            // 
            this.ultraPanelRevaluation.ClientArea.Controls.Add(this.grdRevaluation);
            this.ultraPanelRevaluation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelRevaluation.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelRevaluation.Name = "ultraPanelRevaluation";
            this.ultraPanelRevaluation.Size = new System.Drawing.Size(1073, 337);
            this.ultraPanelRevaluation.TabIndex = 2;
            // 
            // grdRevaluation
            // 
            appearance17.BackColor = System.Drawing.Color.Black;
            this.grdRevaluation.DisplayLayout.Appearance = appearance17;
            this.grdRevaluation.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdRevaluation.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdRevaluation.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdRevaluation.DisplayLayout.MaxColScrollRegions = 1;
            this.grdRevaluation.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdRevaluation.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdRevaluation.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdRevaluation.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdRevaluation.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdRevaluation.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdRevaluation.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdRevaluation.DisplayLayout.Override.CellPadding = 0;
            this.grdRevaluation.DisplayLayout.Override.CellSpacing = 0;
            this.grdRevaluation.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance18.FontData.Name = "Segoe UI";
            appearance18.FontData.SizeInPoints = 9F;
            appearance18.TextHAlignAsString = "Center";
            this.grdRevaluation.DisplayLayout.Override.HeaderAppearance = appearance18;
            this.grdRevaluation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdRevaluation.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdRevaluation.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdRevaluation.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance19.BackColor = System.Drawing.Color.Transparent;
            appearance19.BorderColor = System.Drawing.Color.Transparent;
            appearance19.FontData.BoldAsString = "True";
            this.grdRevaluation.DisplayLayout.Override.SelectedRowAppearance = appearance19;
            this.grdRevaluation.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdRevaluation.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdRevaluation.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdRevaluation.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdRevaluation.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdRevaluation.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdRevaluation.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance20.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdRevaluation.DisplayLayout.Override.TemplateAddRowAppearance = appearance20;
            this.grdRevaluation.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdRevaluation.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdRevaluation.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdRevaluation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRevaluation.ExitEditModeOnLeave = false;
            this.grdRevaluation.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdRevaluation.Location = new System.Drawing.Point(0, 0);
            this.grdRevaluation.Name = "grdRevaluation";
            this.grdRevaluation.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdRevaluation.Size = new System.Drawing.Size(1073, 337);
            this.grdRevaluation.TabIndex = 1;
            this.grdRevaluation.Text = "Cash Journal";
            this.grdRevaluation.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdRevaluation.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdRevaluation.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdRevaluation_InitializeLayout);
            this.grdRevaluation.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdRevaluation_InitializeRow);
            this.grdRevaluation.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdRevaluation_InitializeGroupByRow);
            this.grdRevaluation.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdRevaluation_AfterSortChange);
            this.grdRevaluation.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdRevaluation_BeforeCustomRowFilterDialog);
            this.grdRevaluation.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdRevaluation_BeforeColumnChooserDisplayed);
            this.grdRevaluation.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdRevaluation_MouseClick);
            this.grdRevaluation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdRevaluation_MouseDown);
            this.grdRevaluation.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdRevaluation.AfterRowFilterChanged += grdRevaluation_AfterRowFilterChanged;
            // 
            // statusStripRevaluation
            // 
            this.statusStripRevaluation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRevaluation});
            this.statusStripRevaluation.Location = new System.Drawing.Point(0, 337);
            this.statusStripRevaluation.Name = "statusStripRevaluation";
            this.statusStripRevaluation.Size = new System.Drawing.Size(1073, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStripRevaluation, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStripRevaluation.TabIndex = 1;
            this.statusStripRevaluation.Text = "statusStrip2";
            // 
            // toolStripRevaluation
            // 
            this.toolStripRevaluation.Name = "toolStripRevaluation";
            this.toolStripRevaluation.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraTabPageOpeningBalance
            // 
            this.ultraTabPageOpeningBalance.Controls.Add(this.ultraPanelOpeningBal);
            this.ultraTabPageOpeningBalance.Controls.Add(this.statusStripOpeningBal);
            this.ultraTabPageOpeningBalance.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageOpeningBalance.Name = "ultraTabPageOpeningBalance";
            this.ultraTabPageOpeningBalance.Size = new System.Drawing.Size(1073, 359);
            // 
            // ultraPanelOpeningBal
            // 
            // 
            // ultraPanelOpeningBal.ClientArea
            // 
            this.ultraPanelOpeningBal.ClientArea.Controls.Add(this.grdOpeningBalance);
            this.ultraPanelOpeningBal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelOpeningBal.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelOpeningBal.Name = "ultraPanelOpeningBal";
            this.ultraPanelOpeningBal.Size = new System.Drawing.Size(1073, 337);
            this.ultraPanelOpeningBal.TabIndex = 5;
            // 
            // grdOpeningBalance
            // 
            this.grdOpeningBalance.ContextMenuStrip = this.menuStripCashValue;
            appearance21.BackColor = System.Drawing.Color.Black;
            this.grdOpeningBalance.DisplayLayout.Appearance = appearance21;
            ultraGridBand1.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpeningBalance.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdOpeningBalance.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdOpeningBalance.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdOpeningBalance.DisplayLayout.MaxColScrollRegions = 1;
            this.grdOpeningBalance.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdOpeningBalance.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdOpeningBalance.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdOpeningBalance.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpeningBalance.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpeningBalance.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpeningBalance.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdOpeningBalance.DisplayLayout.Override.CellPadding = 0;
            this.grdOpeningBalance.DisplayLayout.Override.CellSpacing = 0;
            this.grdOpeningBalance.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance22.BorderColor = System.Drawing.Color.Transparent;
            appearance22.ForeColor = System.Drawing.Color.White;
            this.grdOpeningBalance.DisplayLayout.Override.GroupByRowAppearance = appearance22;
            appearance23.FontData.Name = "Segoe UI";
            appearance23.FontData.SizeInPoints = 9F;
            appearance23.TextHAlignAsString = "Center";
            this.grdOpeningBalance.DisplayLayout.Override.HeaderAppearance = appearance23;
            this.grdOpeningBalance.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdOpeningBalance.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance24.BackColor = System.Drawing.Color.Black;
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Right";
            appearance24.TextVAlignAsString = "Middle";
            this.grdOpeningBalance.DisplayLayout.Override.RowAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.Transparent;
            appearance25.BorderColor = System.Drawing.Color.Transparent;
            appearance25.FontData.BoldAsString = "True";
            this.grdOpeningBalance.DisplayLayout.Override.SelectedRowAppearance = appearance25;
            this.grdOpeningBalance.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOpeningBalance.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOpeningBalance.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdOpeningBalance.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOpeningBalance.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdOpeningBalance.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdOpeningBalance.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdOpeningBalance.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.grdOpeningBalance.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdOpeningBalance.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdOpeningBalance.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdOpeningBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOpeningBalance.ExitEditModeOnLeave = false;
            this.grdOpeningBalance.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdOpeningBalance.Location = new System.Drawing.Point(0, 0);
            this.grdOpeningBalance.Name = "grdOpeningBalance";
            this.grdOpeningBalance.Size = new System.Drawing.Size(1073, 337);
            this.grdOpeningBalance.TabIndex = 0;
            this.grdOpeningBalance.Text = "Cash Journal";
            this.grdOpeningBalance.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdOpeningBalance.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpeningBalance.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdOpeningBalance_AfterCellUpdate);
            this.grdOpeningBalance.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdOpeningBalance_InitializeLayout);
            this.grdOpeningBalance.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdOpeningBalance_InitializeRow);
            this.grdOpeningBalance.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdOpeningBalance_InitializeGroupByRow);
            this.grdOpeningBalance.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdOpeningBalance_AfterRowUpdate);
            this.grdOpeningBalance.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdOpeningBalance_CellChange);
            this.grdOpeningBalance.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdOpeningBalance_AfterSortChange);
            this.grdOpeningBalance.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdOpeningBalance_BeforeCustomRowFilterDialog);
            this.grdOpeningBalance.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdOpeningBalance_BeforeColumnChooserDisplayed);
            this.grdOpeningBalance.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdOpeningBalance_MouseClick);
            this.grdOpeningBalance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdOpeningBalance_MouseDown);
            this.grdOpeningBalance.BeforeRowFilterDropDown += grd_BeforeRowFilterDropDown;
            this.grdOpeningBalance.AfterRowFilterChanged += grdOpeningBalance_AfterRowFilterChanged;
            // 
            // statusStripOpeningBal
            // 
            this.statusStripOpeningBal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4});
            this.statusStripOpeningBal.Location = new System.Drawing.Point(0, 337);
            this.statusStripOpeningBal.Name = "statusStripOpeningBal";
            this.statusStripOpeningBal.Size = new System.Drawing.Size(1073, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStripOpeningBal, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStripOpeningBal.TabIndex = 4;
            this.statusStripOpeningBal.Text = "statusStrip4";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 17);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.Location = new System.Drawing.Point(681, 23);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(679, 22);
            this.inboxControlStyler1.SetStyleSettings(this.miniToolStrip, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.miniToolStrip.TabIndex = 4;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(679, 368);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(679, 368);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(679, 368);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(679, 368);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(679, 368);
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(1073, 359);
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl9
            // 
            this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl9.Name = "ultraTabPageControl9";
            this.ultraTabPageControl9.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabControlCashMainValue
            // 
            appearance27.FontData.SizeInPoints = 9F;
            this.ultraTabControlCashMainValue.Appearance = appearance27;
            this.ultraTabControlCashMainValue.ContextMenuStrip = this.menuStripCashValue;
            this.ultraTabControlCashMainValue.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControlCashMainValue.Controls.Add(this.ultraTabPageTradingTrans);
            this.ultraTabControlCashMainValue.Controls.Add(this.ultraTabPageNonTradingTrans);
            this.ultraTabControlCashMainValue.Controls.Add(this.ultraTabPageDividendTrans);
            this.ultraTabControlCashMainValue.Controls.Add(this.ultraTabPageRevaluation);
            this.ultraTabControlCashMainValue.Controls.Add(this.ultraTabPageOpeningBalance);
            this.ultraTabControlCashMainValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControlCashMainValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTabControlCashMainValue.Location = new System.Drawing.Point(0, 10);
            this.ultraTabControlCashMainValue.Name = "ultraTabControlCashMainValue";
            this.ultraTabControlCashMainValue.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.ultraTabControlCashMainValue.Size = new System.Drawing.Size(1077, 387);
            this.ultraTabControlCashMainValue.TabIndex = 2;
            ultraTab1.Key = "tbTradingTrans";
            ultraTab1.TabPage = this.ultraTabPageTradingTrans;
            ultraTab1.Text = "Trading Transaction";
            ultraTab2.Key = "tbNonTradingTran";
            ultraTab2.TabPage = this.ultraTabPageNonTradingTrans;
            ultraTab2.Text = "Non Trading Transaction";
            ultraTab3.Key = "tbDividend";
            ultraTab3.TabPage = this.ultraTabPageDividendTrans;
            ultraTab3.Text = "Dividend";
            ultraTab4.Key = "tbRevaluation";
            ultraTab4.TabPage = this.ultraTabPageRevaluation;
            ultraTab4.Text = "Revaluation";
            ultraTab5.Key = "tbOtherJournals";
            ultraTab5.TabPage = this.ultraTabPageOpeningBalance;
            ultraTab5.Text = "Opening Balance";
            this.ultraTabControlCashMainValue.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab4,
            ultraTab5});
            this.ultraTabControlCashMainValue.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControlCashMainValue_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(679, 368);
            // 
            // ugbxJournalParams
            // 
            this.ugbxJournalParams.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxJournalParams.Controls.Add(this.dtPickerUpper);
            this.ugbxJournalParams.Controls.Add(this.btnGetCash);
            this.ugbxJournalParams.Controls.Add(this.lblTo);
            this.ugbxJournalParams.Controls.Add(this.btnSave);
            this.ugbxJournalParams.Controls.Add(this.dtPickerlower);
            this.ugbxJournalParams.Controls.Add(this.lblFrom);
            this.ugbxJournalParams.Controls.Add(this.btnExport);
            this.ugbxJournalParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxJournalParams.Location = new System.Drawing.Point(0, 0);
            this.ugbxJournalParams.Name = "ugbxJournalParams";
            this.ugbxJournalParams.Size = new System.Drawing.Size(1077, 47);
            this.ugbxJournalParams.TabIndex = 0;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(2, 3);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(483, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 99;
            // 
            // dtPickerUpper
            // 
            appearance28.FontData.SizeInPoints = 9F;
            this.dtPickerUpper.Appearance = appearance28;
            this.dtPickerUpper.AutoSize = false;
            this.dtPickerUpper.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerUpper.Location = new System.Drawing.Point(669, 12);
            this.dtPickerUpper.Name = "dtPickerUpper";
            this.dtPickerUpper.Size = new System.Drawing.Size(109, 23);
            this.dtPickerUpper.TabIndex = 1;
            this.dtPickerUpper.ValueChanged += new System.EventHandler(this.dtPickerUpper_ValueChanged);
            // 
            // btnGetCash
            // 
            appearance29.FontData.SizeInPoints = 9F;
            this.btnGetCash.Appearance = appearance29;
            this.btnGetCash.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetCash.Location = new System.Drawing.Point(784, 12);
            this.btnGetCash.Name = "btnGetCash";
            this.btnGetCash.Size = new System.Drawing.Size(132, 23);
            this.btnGetCash.TabIndex = 2;
            this.btnGetCash.Text = "Get Transactions";
            this.btnGetCash.Click += new System.EventHandler(this.btnGetCash_Click);
            // 
            // lblTo
            // 
            appearance30.FontData.SizeInPoints = 9F;
            appearance30.TextHAlignAsString = "Left";
            appearance30.TextVAlignAsString = "Middle";
            this.lblTo.Appearance = appearance30;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(638, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 23);
            this.lblTo.TabIndex = 98;
            this.lblTo.Text = "To";
            // 
            // btnSave
            // 
            appearance31.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance31;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(999, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dtPickerlower
            // 
            appearance32.FontData.SizeInPoints = 9F;
            this.dtPickerlower.Appearance = appearance32;
            this.dtPickerlower.AutoSize = false;
            this.dtPickerlower.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerlower.Location = new System.Drawing.Point(523, 12);
            this.dtPickerlower.Name = "dtPickerlower";
            this.dtPickerlower.Size = new System.Drawing.Size(109, 23);
            this.dtPickerlower.TabIndex = 0;
            this.dtPickerlower.ValueChanged += new System.EventHandler(this.dtPickerlower_ValueChanged);
            // 
            // lblFrom
            // 
            appearance33.FontData.SizeInPoints = 9F;
            appearance33.TextHAlignAsString = "Left";
            appearance33.TextVAlignAsString = "Middle";
            this.lblFrom.Appearance = appearance33;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(483, 12);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(34, 23);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "From";
            // 
            // btnExport
            // 
            appearance34.FontData.SizeInPoints = 9F;
            this.btnExport.Appearance = appearance34;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(922, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(71, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl12
            // 
            this.ultraTabPageControl12.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            this.ultraTabPageControl12.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl13
            // 
            this.ultraTabPageControl13.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl13.Name = "ultraTabPageControl13";
            this.ultraTabPageControl13.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl14
            // 
            this.ultraTabPageControl14.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl14.Name = "ultraTabPageControl14";
            this.ultraTabPageControl14.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraTabPageControl15
            // 
            this.ultraTabPageControl15.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl15.Name = "ultraTabPageControl15";
            this.ultraTabPageControl15.Size = new System.Drawing.Size(679, 352);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.upnlBody);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxJournalParams);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1077, 444);
            this.ultraPanel1.TabIndex = 107;
            // 
            // upnlBody
            // 
            // 
            // upnlBody.ClientArea
            // 
            this.upnlBody.ClientArea.Controls.Add(this.ultraTabControlCashMainValue);
            this.upnlBody.ClientArea.Controls.Add(this.upnlBodyTop);
            this.upnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upnlBody.Location = new System.Drawing.Point(0, 47);
            this.upnlBody.Name = "upnlBody";
            this.upnlBody.Size = new System.Drawing.Size(1077, 397);
            this.upnlBody.TabIndex = 3;
            // 
            // upnlBodyTop
            // 
            this.upnlBodyTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.upnlBodyTop.Location = new System.Drawing.Point(0, 0);
            this.upnlBodyTop.Name = "upnlBodyTop";
            this.upnlBodyTop.Size = new System.Drawing.Size(1077, 10);
            this.upnlBodyTop.TabIndex = 3;
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // ctrlJournal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlJournal";
            this.Size = new System.Drawing.Size(1077, 444);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.CtrlSubAccountCashImport_Load);
            this.ultraTabPageTradingTrans.ResumeLayout(false);
            this.ultraTabPageTradingTrans.PerformLayout();
            this.ultraPanelTradingTrans.ClientArea.ResumeLayout(false);
            this.ultraPanelTradingTrans.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTradingTransactions)).EndInit();
            this.menuStripCashValue.ResumeLayout(false);
            this.statusStripTradingTrans.ResumeLayout(false);
            this.statusStripTradingTrans.PerformLayout();
            this.ultraTabPageNonTradingTrans.ResumeLayout(false);
            this.ultraTabPageNonTradingTrans.PerformLayout();
            this.ultraPanelNonTradingTrans.ClientArea.ResumeLayout(false);
            this.ultraPanelNonTradingTrans.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdNonTradingTransactions)).EndInit();
            this.statusStripNonTrading.ResumeLayout(false);
            this.statusStripNonTrading.PerformLayout();
            this.ultraTabPageDividendTrans.ResumeLayout(false);
            this.ultraTabPageDividendTrans.PerformLayout();
            this.ultraPanelDividend.ClientArea.ResumeLayout(false);
            this.ultraPanelDividend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDividend)).EndInit();
            this.statusStripDividend.ResumeLayout(false);
            this.statusStripDividend.PerformLayout();
            this.ultraTabPageRevaluation.ResumeLayout(false);
            this.ultraTabPageRevaluation.PerformLayout();
            this.ultraPanelRevaluation.ClientArea.ResumeLayout(false);
            this.ultraPanelRevaluation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRevaluation)).EndInit();
            this.statusStripRevaluation.ResumeLayout(false);
            this.statusStripRevaluation.PerformLayout();
            this.ultraTabPageOpeningBalance.ResumeLayout(false);
            this.ultraTabPageOpeningBalance.PerformLayout();
            this.ultraPanelOpeningBal.ClientArea.ResumeLayout(false);
            this.ultraPanelOpeningBal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOpeningBalance)).EndInit();
            this.statusStripOpeningBal.ResumeLayout(false);
            this.statusStripOpeningBal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControlCashMainValue)).EndInit();
            this.ultraTabControlCashMainValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxJournalParams)).EndInit();
            this.ugbxJournalParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerlower)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.upnlBody.ClientArea.ResumeLayout(false);
            this.upnlBody.ResumeLayout(false);
            this.upnlBodyTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuStripCashValue;
        private System.Windows.Forms.ToolStripMenuItem addTranscationToolStripMenuitem;
        private System.Windows.Forms.ToolStripMenuItem addTranscationItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTranscationToolStripMenuitem;
        private System.Windows.Forms.ToolStripMenuItem deleteTranscationItemtoolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private System.Windows.Forms.StatusStrip miniToolStrip;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControlCashMainValue;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageTradingTrans;
        private Infragistics.Win.Misc.UltraPanel ultraPanelTradingTrans;
        private PranaUltraGrid grdTradingTransactions;
        private System.Windows.Forms.StatusStrip statusStripTradingTrans;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageNonTradingTrans;
        private Infragistics.Win.Misc.UltraPanel ultraPanelNonTradingTrans;
        private PranaUltraGrid grdNonTradingTransactions;
        private System.Windows.Forms.StatusStrip statusStripNonTrading;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageDividendTrans;
        private Infragistics.Win.Misc.UltraPanel ultraPanelDividend;
        private PranaUltraGrid grdDividend;
        private System.Windows.Forms.StatusStrip statusStripDividend;
        private System.Windows.Forms.ToolStripStatusLabel statusStripLabelDividend;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageRevaluation;
        private Infragistics.Win.Misc.UltraPanel ultraPanelRevaluation;
        private PranaUltraGrid grdRevaluation;
        private System.Windows.Forms.StatusStrip statusStripRevaluation;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRevaluation;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageOpeningBalance;
        private Infragistics.Win.Misc.UltraPanel ultraPanelOpeningBal;
        private PranaUltraGrid grdOpeningBalance;
        private System.Windows.Forms.StatusStrip statusStripOpeningBal;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private Infragistics.Win.Misc.UltraGroupBox ugbxJournalParams;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerUpper;
        private Infragistics.Win.Misc.UltraButton btnGetCash;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerlower;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl13;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl14;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl15;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.Misc.UltraPanel upnlBody;
        private Infragistics.Win.Misc.UltraPanel upnlBodyTop;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private System.Windows.Forms.ToolStripMenuItem ShowAllLegstoolStripMenuItem;
        private Controls.ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutAllToolStripMenuItem;
    }
}
