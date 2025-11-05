using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinSchedule.CalendarCombo;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.TradingTicket.Forms
{
    partial class MultiTradingTicket
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearanceCustomBtn = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
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
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.MultiTradingTicket_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.grpBoxBulkUpdate = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpBoxCommisionAttribute = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.BulkUpdateButtonPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnCommit = new Infragistics.Win.Misc.UltraButton();
            this.pnlCommission = new System.Windows.Forms.TableLayoutPanel();
            this.tblTrade = new System.Windows.Forms.TableLayoutPanel();
            this.tblOther = new System.Windows.Forms.TableLayoutPanel();
            this.dtExpireTime = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.pnlExpireTime = new Infragistics.Win.Misc.UltraPanel();
            this.tblOrderButtons = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBulkUpdateFields = new System.Windows.Forms.TableLayoutPanel();
            this.nmrcStop = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.cmbOrderType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTIF = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbCommission = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute3 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute4 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute5 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute6 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbSoft = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbvenue = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbBroker = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.btnCustomAllocation = new Infragistics.Win.Misc.UltraButton();
            this.btnAlgo = new Infragistics.Win.Misc.UltraButton();
            this.btnExpireTime = new Infragistics.Win.Misc.UltraButton();
            this.cmbAllocation = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.pnlAllocation = new Infragistics.Win.Misc.UltraPanel();
            this.pnlVenue = new Infragistics.Win.Misc.UltraPanel();
            this.pnlTIF = new Infragistics.Win.Misc.UltraPanel();
            this.lblStrategy = new Infragistics.Win.Misc.UltraLabel();
            this.lblStop = new Infragistics.Win.Misc.UltraLabel();
            this.lblLimit = new Infragistics.Win.Misc.UltraLabel();
            this.lblOrderType = new Infragistics.Win.Misc.UltraLabel();
            this.lblTIF = new Infragistics.Win.Misc.UltraLabel();
            this.lblExecutionInstructions = new Infragistics.Win.Misc.UltraLabel();
            this.lblHandlingInstructions = new Infragistics.Win.Misc.UltraLabel();
            this.lblTrader = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute5 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute6 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCommission = new Infragistics.Win.Misc.UltraLabel();
            this.lblRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblSoftBasis = new Infragistics.Win.Misc.UltraLabel();
            this.lblSoftRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblVenue = new Infragistics.Win.Misc.UltraLabel();
            this.lblBroker = new Infragistics.Win.Misc.UltraLabel();
            this.lblAllocation = new Infragistics.Win.Misc.UltraLabel();
            this.lblOrderSide = new Infragistics.Win.Misc.UltraLabel();
            this.cmbOrderSide = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbStrategy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbExecution = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbHandling = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTrader = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.nmrcLimit = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.nmrcCommission = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.nmrcSoft = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.grdTrades = new Prana.Utilities.UI.UIUtilities.PranaUltraGrid();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRefreshPrice = new Infragistics.Win.Misc.UltraButton();
            this.btnDoneAway = new Infragistics.Win.Misc.UltraButton();
            this.btnStage = new Infragistics.Win.Misc.UltraButton();
            this.btnReplace = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSend = new Infragistics.Win.Misc.UltraButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.MultiTradingTicket_Fill_Panel.ClientArea.SuspendLayout();
            this.MultiTradingTicket_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxBulkUpdate)).BeginInit();
            this.grpBoxBulkUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxCommisionAttribute)).BeginInit();
            this.grpBoxCommisionAttribute.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            this.BulkUpdateButtonPanel.SuspendLayout();
            this.pnlBulkUpdateFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSoft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).BeginInit();
            this.pnlVenue.ClientArea.SuspendLayout();
            this.pnlVenue.SuspendLayout();
            this.pnlTIF.ClientArea.SuspendLayout();
            this.pnlTIF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbvenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBroker)).BeginInit();
            this.pnlAllocation.ClientArea.SuspendLayout();
            this.pnlAllocation.SuspendLayout();
            this.pnlExpireTime.ClientArea.SuspendLayout();
            this.pnlExpireTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcSoft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrades)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.tblOrderButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtExpireTime)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // MultiTradingTicket_Fill_Panel
            // 
            // 
            // MultiTradingTicket_Fill_Panel.ClientArea
            // 
            this.MultiTradingTicket_Fill_Panel.ClientArea.Controls.Add(this.grpBoxBulkUpdate);
            this.MultiTradingTicket_Fill_Panel.ClientArea.Controls.Add(this.grdTrades);
            this.MultiTradingTicket_Fill_Panel.ClientArea.Controls.Add(this.pnlExpireTime);
            this.MultiTradingTicket_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MultiTradingTicket_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MultiTradingTicket_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.MultiTradingTicket_Fill_Panel.Name = "MultiTradingTicket_Fill_Panel";
            this.MultiTradingTicket_Fill_Panel.Size = new System.Drawing.Size(947, 549);
            this.MultiTradingTicket_Fill_Panel.TabIndex = 0;
            // 
            // grpBoxBulkUpdate
            // 
            this.grpBoxBulkUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxBulkUpdate.Controls.Add(this.grpBoxCommisionAttribute);
            this.grpBoxBulkUpdate.Controls.Add(this.BulkUpdateButtonPanel);
            this.grpBoxBulkUpdate.Controls.Add(this.pnlBulkUpdateFields);
            this.grpBoxBulkUpdate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpBoxBulkUpdate.Location = new System.Drawing.Point(6, 6);
            this.grpBoxBulkUpdate.Name = "grpBoxBulkUpdate";
            this.grpBoxBulkUpdate.Size = new System.Drawing.Size(935, 136);
            this.grpBoxBulkUpdate.TabIndex = 1;
            this.grpBoxBulkUpdate.Text = "Bulk Update";
            this.grpBoxBulkUpdate.UseAppStyling = false;
            // 
            // grpBoxCommisionAttribute
            // 
            this.grpBoxCommisionAttribute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxCommisionAttribute.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.grpBoxCommisionAttribute.Expanded = false;
            this.grpBoxCommisionAttribute.ExpandedSize = new System.Drawing.Size(786, 78);
            this.grpBoxCommisionAttribute.Location = new System.Drawing.Point(9, 85);
            this.grpBoxCommisionAttribute.Name = "grpBoxCommisionAttribute";
            this.grpBoxCommisionAttribute.Size = new System.Drawing.Size(786, 44);
            this.grpBoxCommisionAttribute.TabIndex = 2;
            this.grpBoxCommisionAttribute.Text = "Commission, Trade Attribute && Others";
            this.grpBoxCommisionAttribute.ExpansionIndicatorCollapsed = global::Prana.TradingTicket.Properties.Resources.Maximize;
            this.grpBoxCommisionAttribute.ExpansionIndicatorExpanded = global::Prana.TradingTicket.Properties.Resources.Minimise;
            this.grpBoxCommisionAttribute.UseAppStyling = false;
            this.grpBoxCommisionAttribute.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grpBoxCommisionAttribute.ExpandedStateChanged += grpBoxCommisionAttribute_expandedStateChanged;
            this.grpBoxCommisionAttribute.HeaderClickAction = Infragistics.Win.Misc.GroupBoxHeaderClickAction.ToggleExpansion;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.ultraTabControl1);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(780, 56);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // BulkUpdateButtonPanel
            // 
            this.BulkUpdateButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BulkUpdateButtonPanel.BackColor = System.Drawing.Color.Transparent;
            this.BulkUpdateButtonPanel.ColumnCount = 1;
            this.BulkUpdateButtonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BulkUpdateButtonPanel.Controls.Add(this.btnClear, 0, 1);
            this.BulkUpdateButtonPanel.Controls.Add(this.btnCommit, 0, 0);
            this.BulkUpdateButtonPanel.Location = new System.Drawing.Point(801, 19);
            this.BulkUpdateButtonPanel.Name = "BulkUpdateButtonPanel";
            this.BulkUpdateButtonPanel.RowCount = 2;
            this.BulkUpdateButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BulkUpdateButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BulkUpdateButtonPanel.Size = new System.Drawing.Size(128, 111);
            this.BulkUpdateButtonPanel.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClear.Location = new System.Drawing.Point(26, 96);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += BtnClear_Click;

            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCommit.Location = new System.Drawing.Point(26, 24);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(75, 23);
            this.btnCommit.TabIndex = 0;
            this.btnCommit.Text = "Commit";
            this.btnCommit.Click += BtnCommit_Click;
            // 
            // pnlBulkUpdateFields
            // 
            this.pnlBulkUpdateFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBulkUpdateFields.BackColor = System.Drawing.Color.Transparent;
            this.pnlBulkUpdateFields.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlBulkUpdateFields.ColumnCount = 9;
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlBulkUpdateFields.Controls.Add(this.nmrcStop, 7, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbOrderType, 5, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbTIF, 4, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbvenue, 3, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbBroker, 2, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.lblStrategy, 8, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.lblStop, 7, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.lblLimit, 6, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.lblOrderType, 5, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.pnlTIF, 4, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.pnlVenue, 3, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.lblBroker, 2, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbAllocation, 1, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.pnlAllocation, 1, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.lblOrderSide, 0, 0);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbOrderSide, 0, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.cmbStrategy, 8, 1);
            this.pnlBulkUpdateFields.Controls.Add(this.nmrcLimit, 6, 1);
            this.pnlBulkUpdateFields.Location = new System.Drawing.Point(6, 19);
            this.pnlBulkUpdateFields.Name = "pnlBulkUpdateFields";
            this.pnlBulkUpdateFields.RowCount = 2;
            this.pnlBulkUpdateFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBulkUpdateFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlBulkUpdateFields.Size = new System.Drawing.Size(789, 59);
            this.pnlBulkUpdateFields.TabIndex = 0;
            // 
            // nmrcStop
            // 
            this.nmrcStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrcStop.Location = new System.Drawing.Point(627, 34);
            this.nmrcStop.Maximum = 999999999;
            this.nmrcStop.Name = "nmrcStop";
            this.nmrcStop.Size = new System.Drawing.Size(72, 20);
            this.nmrcStop.TabIndex = 18;
            this.nmrcStop.Enabled = false;
            this.nmrcStop.AllowThousandSeperator = true;
            this.nmrcStop.ShowCommaSeperatorOnEditing = true;
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbOrderType.NullText = "Select";
            this.cmbOrderType.DropDownListWidth = -1;
            this.cmbOrderType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOrderType.Location = new System.Drawing.Point(471, 33);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(72, 21);
            this.cmbOrderType.TabIndex = 16;
            this.cmbOrderType.ValueChanged += CmbOrderType_ValueChanged;
            //
            //cmbCommission
            //
            this.cmbCommission.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbCommission.NullText = "Select";
            this.cmbCommission.DropDownListWidth = -1;
            this.cmbCommission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCommission.Location = new System.Drawing.Point(3, 23);
            this.cmbCommission.Name = "cmbCommission";
            this.cmbCommission.Size = new System.Drawing.Size(70, 10);
            this.cmbCommission.TabIndex = 0;
            this.cmbCommission.ValueChanged += CmbCommission_ValueChanged;
            //
            //cmbExecution
            //
            this.cmbExecution.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbExecution.NullText = "Select";
            this.cmbExecution.DropDownListWidth = -1;
            this.cmbExecution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbExecution.Location = new System.Drawing.Point(3, 23);
            this.cmbExecution.Name = "cmbExecution";
            this.cmbExecution.Size = new System.Drawing.Size(70, 10);
            this.cmbExecution.TabIndex = 0;
            this.cmbExecution.ValueChanged += cmb_ValueChanged;
            // 
            // dtExpireTime
            // 
            this.dtExpireTime.AutoCloseUp = false;
            this.dtExpireTime.DateButtons.Add(dateButton1);
            this.dtExpireTime.Format = "MM/dd/yyyy";
            this.dtExpireTime.Name = "dtExpireTime";
            this.dtExpireTime.NonAutoSizeHeight = 21;
            this.dtExpireTime.NullDateLabel = " -Select- ";
            this.dtExpireTime.Size = new System.Drawing.Size(0, 21);
            this.dtExpireTime.Value = "";
            this.dtExpireTime.AfterDropDown += DtExpireDate_AfterDropDown;
            this.dtExpireTime.TextChanged += DtExpireDate_TextChanged;
            this.dtExpireTime.AfterCloseUp += DtExpireDate_AfterCloseUp;
            this.dtExpireTime.UseAppStyling = false;
            // 
            // pnlExpireTime
            // 
            this.pnlExpireTime.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pnlExpireTime.Margin = new System.Windows.Forms.Padding(0);
            this.pnlExpireTime.Name = "pnlExpireTime";
            this.pnlExpireTime.Size = new System.Drawing.Size(1, 1);
            this.pnlExpireTime.UseAppStyling = false;
            this.pnlExpireTime.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.pnlExpireTime.ClientArea.Controls.Add(this.dtExpireTime);
            //
            //cmbHandling
            //
            this.cmbHandling.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbHandling.NullText = "Select";
            this.cmbHandling.DropDownListWidth = -1;
            this.cmbHandling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbHandling.Location = new System.Drawing.Point(193, 23);
            this.cmbHandling.Name = "cmbHandling";
            this.cmbHandling.Size = new System.Drawing.Size(70, 10);
            this.cmbHandling.TabIndex = 1;
            this.cmbHandling.ValueChanged += cmb_ValueChanged;
            //
            //cmbTrader
            //
            this.cmbTrader.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTrader.NullText = "Select";
            this.cmbTrader.DropDownListWidth = -1;
            this.cmbTrader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTrader.Location = new System.Drawing.Point(383, 23);
            this.cmbTrader.Name = "cmbTrader";
            this.cmbTrader.Size = new System.Drawing.Size(70, 10);
            this.cmbTrader.TabIndex = 2;
            this.cmbTrader.ValueChanged += cmb_ValueChanged;
            //
            //cmbSoft
            //
            this.cmbSoft.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbSoft.NullText = "Select";
            this.cmbSoft.DropDownListWidth = -1;
            this.cmbSoft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSoft.Location = new System.Drawing.Point(383, 23);
            this.cmbSoft.Name = "cmbSoft";
            this.cmbSoft.Size = new System.Drawing.Size(70, 10);
            this.cmbSoft.TabIndex = 2;
            this.cmbSoft.ValueChanged += CmbSoft_ValueChanged;

            // 
            // cmbTIF
            // 
            this.cmbTIF.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTIF.NullText = "Select";
            this.cmbTIF.DropDownListWidth = -1;
            this.cmbTIF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTIF.Location = new System.Drawing.Point(393, 33);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.Size = new System.Drawing.Size(72, 21);
            this.cmbTIF.TabIndex = 15;
            this.cmbTIF.ValueChanged += cmbTIF_ValueChanged;
            // 
            // cmbvenue
            //
            this.cmbvenue.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbvenue.NullText = "Select";
            this.cmbvenue.DropDownListWidth = -1;
            this.cmbvenue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbvenue.Location = new System.Drawing.Point(237, 33);
            this.cmbvenue.Name = "cmbvenue";
            this.cmbvenue.Size = new System.Drawing.Size(72, 21);
            this.cmbvenue.TabIndex = 13;
            this.cmbvenue.ValueChanged += Cmbvenue_ValueChanged;
            // 
            // cmbBroker
            // 
            this.cmbBroker.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbBroker.NullText = "Select";
            this.cmbBroker.DropDownListWidth = -1;
            this.cmbBroker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBroker.Location = new System.Drawing.Point(159, 33);
            this.cmbBroker.Name = "cmbBroker";
            this.cmbBroker.Size = new System.Drawing.Size(72, 21);
            this.cmbBroker.TabIndex = 12;
            this.cmbBroker.ValueChanged += CmbBroker_ValueChanged;
            //
            //cmbTradeAttribute1
            //
            this.cmbTradeAttribute1.NullText = "Select";
            this.cmbTradeAttribute1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeAttribute1.Location = new System.Drawing.Point(3, 23);
            this.cmbTradeAttribute1.Name = "cmbTradeAttribute1";
            this.cmbTradeAttribute1.Size = new System.Drawing.Size(70, 10);
            this.cmbTradeAttribute1.TabIndex = 0;
            this.cmbTradeAttribute1.ValueChanged += cmb_ValueChanged;
            //
            //cmbTradeAttribute2
            //
            this.cmbTradeAttribute2.NullText = "Select";
            this.cmbTradeAttribute2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeAttribute2.Location = new System.Drawing.Point(129, 23);
            this.cmbTradeAttribute2.Name = "cmbTradeAttribute2";
            this.cmbTradeAttribute2.Size = new System.Drawing.Size(70, 10);
            this.cmbTradeAttribute2.TabIndex = 1;
            this.cmbTradeAttribute2.ValueChanged += cmb_ValueChanged;
            //
            //cmbTradeAttribute3
            //
            this.cmbTradeAttribute3.NullText = "Select";
            this.cmbTradeAttribute3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeAttribute3.Location = new System.Drawing.Point(3, 23);
            this.cmbTradeAttribute3.Name = "cmbTradeAttribute3";
            this.cmbTradeAttribute3.Size = new System.Drawing.Size(70, 10);
            this.cmbTradeAttribute3.TabIndex = 2;
            this.cmbTradeAttribute3.ValueChanged += cmb_ValueChanged;
            //
            //cmbTradeAttribute4
            //
            this.cmbTradeAttribute4.NullText = "Select";
            this.cmbTradeAttribute4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeAttribute4.Location = new System.Drawing.Point(384, 23);
            this.cmbTradeAttribute4.Name = "cmbTradeAttribute4";
            this.cmbTradeAttribute4.Size = new System.Drawing.Size(70, 10);
            this.cmbTradeAttribute4.TabIndex = 3;
            this.cmbTradeAttribute4.ValueChanged += cmb_ValueChanged;
            //
            //cmbTradeAttribute5
            //
            this.cmbTradeAttribute5.NullText = "Select";
            this.cmbTradeAttribute5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeAttribute5.Location = new System.Drawing.Point(513, 23);
            this.cmbTradeAttribute5.Name = "cmbTradeAttribute5";
            this.cmbTradeAttribute5.Size = new System.Drawing.Size(70, 10);
            this.cmbTradeAttribute5.TabIndex = 4;
            this.cmbTradeAttribute5.ValueChanged += cmb_ValueChanged;
            //
            //cmbTradeAttribute6
            //
            this.cmbTradeAttribute6.NullText = "Select";
            this.cmbTradeAttribute6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTradeAttribute6.Location = new System.Drawing.Point(642, 23);
            this.cmbTradeAttribute6.Name = "cmbTradeAttribute6";
            this.cmbTradeAttribute6.Size = new System.Drawing.Size(70, 10);
            this.cmbTradeAttribute6.TabIndex = 5;
            this.cmbTradeAttribute6.ValueChanged += cmb_ValueChanged;
            // 
            // btnAlgo
            // 
            this.btnAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAlgo.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlgo.Location = new System.Drawing.Point(65, 10);
            this.btnAlgo.Name = "btnAlgo";
            this.btnAlgo.Size = new System.Drawing.Size(61, 19);
            this.btnAlgo.Text = "NONE";
            this.btnAlgo.TabStop = false;
            this.btnAlgo.Visible = false;
            this.btnAlgo.Click += BtnAlgo_Click;
            // 
            // 
            // btnExpireTime
            // 
            this.btnExpireTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpireTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpireTime.Location = new System.Drawing.Point(70, 10);
            this.btnExpireTime.Name = "btnExpireTime";
            this.btnExpireTime.Size = new System.Drawing.Size(55, 23);
            this.btnExpireTime.Text = "N/A";
            this.btnExpireTime.TabStop = false;
            this.btnExpireTime.Visible = false;
            this.btnExpireTime.Click += BtnExpireTime_Click;
            //
            //pnlVenue
            //            
            this.pnlVenue.ClientArea.Controls.Add(this.btnAlgo);
            this.pnlVenue.ClientArea.Controls.Add(this.lblVenue);
            this.pnlVenue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlVenue.BackColor = System.Drawing.Color.Transparent;
            this.pnlVenue.Location = new System.Drawing.Point(237, 3);
            this.pnlVenue.Margin = new System.Windows.Forms.Padding(0);
            this.pnlVenue.Name = "pnlVenue";
            this.pnlVenue.Size = new System.Drawing.Size(130, 34);
            this.pnlVenue.TabIndex = 3;

            //
            //pnlTIF
            //            
            this.pnlTIF.ClientArea.Controls.Add(this.btnExpireTime);
            this.pnlTIF.ClientArea.Controls.Add(this.lblTIF);
            this.pnlTIF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTIF.BackColor = System.Drawing.Color.Transparent;
            this.pnlTIF.Location = new System.Drawing.Point(393, 33);
            this.pnlTIF.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTIF.Name = "pnlTIF";
            this.pnlTIF.Size = new System.Drawing.Size(130, 34);
            this.pnlTIF.TabIndex = 3;
            //
            //pnlAllocation
            //
            this.pnlAllocation.ClientArea.Controls.Add(this.btnCustomAllocation);
            this.pnlAllocation.ClientArea.Controls.Add(this.lblAllocation);
            this.pnlAllocation.BackColor = System.Drawing.Color.Transparent;
            this.pnlAllocation.Location = new System.Drawing.Point(87, 0);
            this.pnlAllocation.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAllocation.Name = "pnlAllocation";
            this.pnlAllocation.Size = new System.Drawing.Size(132, 34);
            this.pnlAllocation.TabIndex = 1;
            //
            //btnCustomAllocation
            //
            appearanceCustomBtn.BorderColor = System.Drawing.Color.Black;
            appearanceCustomBtn.Image = global::Prana.TradingTicket.Properties.Resources.level1;
            appearanceCustomBtn.TextHAlignAsString = "Center";
            appearanceCustomBtn.TextVAlignAsString = "Top";
            this.btnCustomAllocation.Appearance = appearanceCustomBtn;
            this.btnCustomAllocation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCustomAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustomAllocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomAllocation.Location = new System.Drawing.Point(110, 10);
            this.btnCustomAllocation.Name = "btnCustomAllocation";
            this.btnCustomAllocation.Size = new System.Drawing.Size(19, 19);
            this.btnCustomAllocation.TabIndex = 1;
            this.btnCustomAllocation.Click += BtnCustomAllocation_Click;
            // 
            // cmbAllocation
            // 
            this.cmbAllocation.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbAllocation.NullText = "Select";
            this.cmbAllocation.DropDownListWidth = -1;
            this.cmbAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAllocation.Location = new System.Drawing.Point(81, 33);
            this.cmbAllocation.Name = "cmbAllocation";
            this.cmbAllocation.Size = new System.Drawing.Size(72, 21);
            this.cmbAllocation.TabIndex = 11;
            this.cmbAllocation.ValueChanged += CmbAllocation_ValueChanged;
            // 
            // lblStrategy
            // 
            this.lblStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance23.TextVAlignAsString = "Bottom";
            this.lblStrategy.Appearance = appearance23;
            this.lblStrategy.Location = new System.Drawing.Point(705, 3);
            this.lblStrategy.Name = "lblStrategy";
            this.lblStrategy.Size = new System.Drawing.Size(72, 23);
            this.lblStrategy.TabIndex = 9;
            this.lblStrategy.Text = "Strategy";
            // 
            // lblStop
            // 
            this.lblStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance24.TextVAlignAsString = "Bottom";
            this.lblStop.Appearance = appearance24;
            this.lblStop.Location = new System.Drawing.Point(627, 3);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(72, 23);
            this.lblStop.TabIndex = 8;
            this.lblStop.Text = "Stop";
            // 
            // lblLimit
            // 
            this.lblLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance25.TextVAlignAsString = "Bottom";
            this.lblLimit.Appearance = appearance25;
            this.lblLimit.Location = new System.Drawing.Point(549, 3);
            this.lblLimit.Name = "lblLimit";
            this.lblLimit.Size = new System.Drawing.Size(72, 23);
            this.lblLimit.TabIndex = 7;
            this.lblLimit.Text = "Limit";
            // 
            // lblOrderType
            // 
            this.lblOrderType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance26.TextVAlignAsString = "Bottom";
            this.lblOrderType.Appearance = appearance26;
            this.lblOrderType.Location = new System.Drawing.Point(471, 4);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(72, 23);
            this.lblOrderType.TabIndex = 6;
            this.lblOrderType.Text = "Order Type";
            // 
            // lblTIF
            // 
            this.lblTIF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance27.TextVAlignAsString = "Bottom";
            this.lblTIF.Appearance = appearance27;
            this.lblTIF.Location = new System.Drawing.Point(2, 8);
            this.lblTIF.Name = "lblTIF";
            this.lblTIF.Size = new System.Drawing.Size(72, 23);
            this.lblTIF.TabIndex = 5;
            this.lblTIF.Text = "TIF";
            // 
            // lblVenue
            // 
            this.lblVenue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance29.TextVAlignAsString = "Bottom";
            this.lblVenue.Appearance = appearance29;
            this.lblVenue.Location = new System.Drawing.Point(2, 8);
            this.lblVenue.Name = "lblVenue";
            this.lblVenue.Size = new System.Drawing.Size(72, 23);
            this.lblVenue.TabIndex = 0;
            this.lblVenue.Text = "Venue";
            //
            //lblExecutionInstructions
            //
            this.lblExecutionInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblExecutionInstructions.Location = new System.Drawing.Point(3, 13);
            this.lblExecutionInstructions.Name = "lblExecutionInstructions";
            this.lblExecutionInstructions.Size = new System.Drawing.Size(120, 14);
            this.lblExecutionInstructions.TabIndex = 0;
            this.lblExecutionInstructions.Text = "Execution Instructions";
            //
            //lblHandlingInstructions
            //
            this.lblHandlingInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHandlingInstructions.Location = new System.Drawing.Point(193, 13);
            this.lblHandlingInstructions.Name = "lblHandlingInstructions";
            this.lblHandlingInstructions.Size = new System.Drawing.Size(120, 14);
            this.lblHandlingInstructions.TabIndex = 1;
            this.lblHandlingInstructions.Text = "Handling Instructions";
            //
            //lblTrader
            //
            this.lblTrader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrader.Location = new System.Drawing.Point(383, 13);
            this.lblTrader.Name = "lblTrader";
            this.lblTrader.Size = new System.Drawing.Size(100, 14);
            this.lblTrader.TabIndex = 1;
            this.lblTrader.Text = "Trader";
            //
            //lblTradeAttribute1
            //
            this.lblTradeAttribute1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTradeAttribute1.Location = new System.Drawing.Point(3, 13);
            this.lblTradeAttribute1.Name = "lblTradeAttribute1";
            this.lblTradeAttribute1.Size = new System.Drawing.Size(100, 14);
            this.lblTradeAttribute1.TabIndex = 0;
            this.lblTradeAttribute1.Text = "Trade Attribute 1";
            //
            //lblTradeAttribute2
            //
            this.lblTradeAttribute2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTradeAttribute2.Location = new System.Drawing.Point(129, 13);
            this.lblTradeAttribute2.Name = "lblTradeAttribute2";
            this.lblTradeAttribute2.Size = new System.Drawing.Size(100, 14);
            this.lblTradeAttribute2.TabIndex = 1;
            this.lblTradeAttribute2.Text = "Trade Attribute 2";
            //
            //lblTradeAttribute3
            //
            this.lblTradeAttribute3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTradeAttribute3.Location = new System.Drawing.Point(259, 13);
            this.lblTradeAttribute3.Name = "lblTradeAttribute3";
            this.lblTradeAttribute3.Size = new System.Drawing.Size(100, 14);
            this.lblTradeAttribute3.TabIndex = 2;
            this.lblTradeAttribute3.Text = "Trade Attribute 3";
            //
            //lblTradeAttribute4
            //
            this.lblTradeAttribute4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTradeAttribute4.Location = new System.Drawing.Point(385, 13);
            this.lblTradeAttribute4.Name = "lblTradeAttribute4";
            this.lblTradeAttribute4.Size = new System.Drawing.Size(100, 14);
            this.lblTradeAttribute4.TabIndex = 3;
            this.lblTradeAttribute4.Text = "Trade Attribute 4";
            //
            //lblTradeAttribute5
            //
            this.lblTradeAttribute5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTradeAttribute5.Location = new System.Drawing.Point(513, 13);
            this.lblTradeAttribute5.Name = "lblTradeAttribute5";
            this.lblTradeAttribute5.Size = new System.Drawing.Size(100, 14);
            this.lblTradeAttribute5.TabIndex = 4;
            this.lblTradeAttribute5.Text = "Trade Attribute 5";
            //
            //lblTradeAttribute6
            //
            this.lblTradeAttribute6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTradeAttribute6.Location = new System.Drawing.Point(642, 13);
            this.lblTradeAttribute6.Name = "lblTradeAttribute6";
            this.lblTradeAttribute6.Size = new System.Drawing.Size(100, 14);
            this.lblTradeAttribute6.TabIndex = 5;
            this.lblTradeAttribute6.Text = "Trade Attribute 6";
            //
            //lblCommission
            //
            this.lblCommission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCommission.Location = new System.Drawing.Point(3, 13);
            this.lblCommission.Name = "lblCommission";
            this.lblCommission.Size = new System.Drawing.Size(100, 14);
            this.lblCommission.TabIndex = 0;
            this.lblCommission.Text = "Commission Basis";
            //
            //lblRate
            //
            this.lblRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRate.Location = new System.Drawing.Point(193, 13);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(100, 14);
            this.lblRate.TabIndex = 1;
            this.lblRate.Text = "Commission Rate";
            //
            //lblSoftBasis
            //
            this.lblSoftBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSoftBasis.Location = new System.Drawing.Point(383, 13);
            this.lblSoftBasis.Name = "lblSoftBasis";
            this.lblSoftBasis.Size = new System.Drawing.Size(100, 14);
            this.lblSoftBasis.TabIndex = 2;
            this.lblSoftBasis.Text = "Soft Basis";
            //
            //lblSoftRate
            //
            this.lblSoftRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSoftRate.Location = new System.Drawing.Point(573, 13);
            this.lblSoftRate.Name = "lblSoftRate";
            this.lblSoftRate.Size = new System.Drawing.Size(100, 14);
            this.lblSoftRate.TabIndex = 2;
            this.lblSoftRate.Text = "Soft Rate";
            // 
            // lblBroker
            // 
            this.lblBroker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance30.TextVAlignAsString = "Bottom";
            this.lblBroker.Appearance = appearance30;
            this.lblBroker.Location = new System.Drawing.Point(2, 8);
            this.lblBroker.Name = "lblBroker";
            this.lblBroker.Size = new System.Drawing.Size(72, 23);
            this.lblBroker.TabIndex = 0;
            this.lblBroker.Text = "Broker";
            // 
            // lblAllocation
            // 
            this.lblAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance31.TextVAlignAsString = "Bottom";
            this.lblAllocation.Appearance = appearance31;
            this.lblAllocation.Location = new System.Drawing.Point(2, 8);
            this.lblAllocation.Name = "lblAllocation";
            this.lblAllocation.Size = new System.Drawing.Size(80, 23);
            this.lblAllocation.TabIndex = 0;
            this.lblAllocation.Text = "Allocation";
            // 
            // lblOrderSide
            // 
            this.lblOrderSide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance32.TextVAlignAsString = "Bottom";
            this.lblOrderSide.Appearance = appearance32;
            this.lblOrderSide.Location = new System.Drawing.Point(3, 3);
            this.lblOrderSide.Name = "lblOrderSide";
            this.lblOrderSide.Size = new System.Drawing.Size(72, 23);
            this.lblOrderSide.TabIndex = 0;
            this.lblOrderSide.Text = "Order Side";
            // 
            // cmbOrderSide
            // 
            this.cmbOrderSide.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbOrderSide.NullText = "Select";
            this.cmbOrderSide.DropDownListWidth = -1;
            this.cmbOrderSide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOrderSide.Location = new System.Drawing.Point(3, 33);
            this.cmbOrderSide.Enabled = false;
            this.cmbOrderSide.Name = "cmbOrderSide";
            this.cmbOrderSide.Size = new System.Drawing.Size(72, 21);
            this.cmbOrderSide.TabIndex = 10;
            this.cmbOrderSide.ValueChanged += cmb_ValueChanged;
            // 
            // cmbStrategy
            // 
            this.cmbStrategy.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbStrategy.DropDownListWidth = -1;
            this.cmbStrategy.NullText = "Select";
            this.cmbStrategy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStrategy.Location = new System.Drawing.Point(705, 33);
            this.cmbStrategy.Name = "cmbStrategy";
            this.cmbStrategy.Size = new System.Drawing.Size(81, 21);
            this.cmbStrategy.TabIndex = 19;
            this.cmbStrategy.ValueChanged += cmb_ValueChanged;
            // 
            // nmrcCommission
            // 
            this.nmrcCommission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrcCommission.Location = new System.Drawing.Point(193, 8);
            this.nmrcCommission.Name = "nmrcCommission";
            this.nmrcCommission.Size = new System.Drawing.Size(70, 10);
            this.nmrcCommission.TabIndex = 1;
            this.nmrcCommission.Maximum = 999999999m;
            this.nmrcCommission.Minimum = 0m;
            this.nmrcCommission.AllowThousandSeperator = true;
            this.nmrcCommission.ShowCommaSeperatorOnEditing = true;
            this.nmrcCommission.Value = 0m;
            //
            //nmrcSoft
            this.nmrcSoft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrcSoft.Location = new System.Drawing.Point(573, 5);
            this.nmrcSoft.Name = "nmrcSoft";
            this.nmrcSoft.Size = new System.Drawing.Size(70, 10);
            this.nmrcSoft.TabIndex = 3;
            this.nmrcSoft.Maximum = 999999999m;
            this.nmrcSoft.Minimum = 0m;
            this.nmrcSoft.AllowThousandSeperator = true;
            this.nmrcSoft.ShowCommaSeperatorOnEditing = true;
            this.nmrcSoft.Value = 0m;
            // 
            // nmrcLimit
            //
            this.nmrcLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nmrcLimit.Maximum = 999999999;
            this.nmrcLimit.Location = new System.Drawing.Point(549, 34);
            this.nmrcLimit.Name = "nmrcLimit";
            this.nmrcLimit.Size = new System.Drawing.Size(72, 20);
            this.nmrcLimit.TabIndex = 17;
            this.nmrcLimit.AllowThousandSeperator = true;
            this.nmrcLimit.ShowCommaSeperatorOnEditing = true;
            this.nmrcLimit.Enabled = false;
            // 
            // grdTrades
            // 
            this.grdTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdTrades.ContextMenuStrip = this.contextMenuStrip;
            this.grdTrades.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdTrades.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.BorderColor = System.Drawing.SystemColors.Window;
            this.grdTrades.DisplayLayout.GroupByBox.Appearance = appearance12;
            appearance13.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdTrades.DisplayLayout.GroupByBox.BandLabelAppearance = appearance13;
            this.grdTrades.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance14.BackColor2 = System.Drawing.SystemColors.Control;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance14.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdTrades.DisplayLayout.GroupByBox.PromptAppearance = appearance14;
            this.grdTrades.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTrades.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdTrades.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdTrades.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            this.grdTrades.DisplayLayout.Override.CardAreaAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Silver;
            appearance18.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdTrades.DisplayLayout.Override.CellAppearance = appearance18;
            this.grdTrades.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdTrades.DisplayLayout.Override.CellPadding = 0;
            appearance19.BackColor = System.Drawing.SystemColors.Control;
            appearance19.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance19.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance19.BorderColor = System.Drawing.SystemColors.Window;
            this.grdTrades.DisplayLayout.Override.GroupByRowAppearance = appearance19;
            appearance20.TextHAlignAsString = "Left";
            this.grdTrades.DisplayLayout.Override.HeaderAppearance = appearance20;
            this.grdTrades.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdTrades.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdTrades.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance22.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdTrades.DisplayLayout.Override.TemplateAddRowAppearance = appearance22;
            this.grdTrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdTrades.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdTrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.grdTrades.Location = new System.Drawing.Point(0, 148);
            this.grdTrades.Name = "grdTrades";
            this.grdTrades.Size = new System.Drawing.Size(947, 156);
            this.grdTrades.TabIndex = 0;
            this.grdTrades.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdTrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdTrades_InitializeLayout);
            this.grdTrades.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.grdTrades_BeforeHeaderCheckStateChanged);
            this.grdTrades.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.grdTrades_AfterHeaderCheckStateChanged);
            this.grdTrades.AfterRowsDeleted += new System.EventHandler(this.grdTrades_AfterRowsDeleted);
            this.grdTrades.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.grdTrades_BeforeCellListDropDown);
            this.grdTrades.FilterRow += new Infragistics.Win.UltraWinGrid.FilterRowEventHandler(this.grdTrades_FilterRow);
            this.grdTrades.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdTrades_BeforeCustomRowFilterDialog);
            this.grdTrades.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdTrades_BeforeColumnChooserDisplayed);
            this.grdTrades.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdTrades_ClickCellButton);
            this.grdTrades.MouseClick += GrdTrades_MouseClick;
            this.grdTrades.CellDataError += GrdTrades_CellDataError;
            this.grdTrades.Error += GrdTrades_Error;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayout,
            this.removeFilter});
            this.contextMenuStrip.Name = "mnuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(147, 48);
            // 
            // saveLayout
            // 
            this.saveLayout.Name = "saveLayout";
            this.saveLayout.Size = new System.Drawing.Size(146, 22);
            this.saveLayout.Text = "Save Layout";
            this.saveLayout.Click += new System.EventHandler(this.saveLayout_Click);
            // 
            // removeFilter
            // 
            this.removeFilter.Name = "removeFilter";
            this.removeFilter.Size = new System.Drawing.Size(146, 22);
            this.removeFilter.Text = "Remove Filter";
            this.removeFilter.Click += new System.EventHandler(this.removeFilter_Click);
            // 
            // btnRefreshPrice
            // 
            this.btnRefreshPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRefreshPrice.Location = new System.Drawing.Point(223, 2);
            this.btnRefreshPrice.Name = "btnRefreshPrice";
            this.btnRefreshPrice.Size = new System.Drawing.Size(85, 28);
            this.btnRefreshPrice.TabIndex = 4;
            this.btnRefreshPrice.Text = "Refresh Price";
            this.btnRefreshPrice.Click += new System.EventHandler(this.btnRefreshPrice_Click);
            // 
            // btnDoneAway
            // 
            this.btnDoneAway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnDoneAway.Location = new System.Drawing.Point(483, 2);
            this.btnDoneAway.Name = "btnDoneAway";
            this.btnDoneAway.Size = new System.Drawing.Size(85, 28);
            this.btnDoneAway.TabIndex = 3;
            this.btnDoneAway.Text = "Done Away";
            this.btnDoneAway.Click += new System.EventHandler(this.btnDoneAway_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnCancel.Location = new System.Drawing.Point(353, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStage
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnReplace.Location = new System.Drawing.Point(353, 2);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(85, 28);
            this.btnReplace.TabIndex = 2;
            this.btnReplace.Text = "Replace";
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnStage
            // 
            this.btnStage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnStage.Location = new System.Drawing.Point(353, 2);
            this.btnStage.Name = "btnStage";
            this.btnStage.Size = new System.Drawing.Size(85, 28);
            this.btnStage.TabIndex = 2;
            this.btnStage.Text = "Create Order";
            this.btnStage.Click += new System.EventHandler(this.btnStage_Click);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnSend.Location = new System.Drawing.Point(613, 2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(85, 28);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnSend.MouseHover += message_MouseHover;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // _MultiTradingTicket_UltraFormManager_Dock_Area_Left
            // 
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.Name = "_MultiTradingTicket_UltraFormManager_Dock_Area_Left";
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 549);
            // 
            // _MultiTradingTicket_UltraFormManager_Dock_Area_Right
            // 
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(955, 32);
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.Name = "_MultiTradingTicket_UltraFormManager_Dock_Area_Right";
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 549);
            // 
            // _MultiTradingTicket_UltraFormManager_Dock_Area_Top
            // 
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.Name = "_MultiTradingTicket_UltraFormManager_Dock_Area_Top";
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(963, 32);
            // 
            // _MultiTradingTicket_UltraFormManager_Dock_Area_Bottom
            // 
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 581);
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.Name = "_MultiTradingTicket_UltraFormManager_Dock_Area_Bottom";
            this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(963, 8);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.tblOrderButtons);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraStatusBar);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel1.Location = new System.Drawing.Point(8, 527);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(947, 54);
            this.ultraPanel1.TabIndex = 4;
            // 
            // tblOrderButtons
            // 
            this.tblOrderButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblOrderButtons.Location = new System.Drawing.Point(0, 0);
            this.tblOrderButtons.Name = "tblOrderButtons";
            this.tblOrderButtons.RowCount = 1;
            this.tblOrderButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblOrderButtons.Size = new System.Drawing.Size(947, 33);
            this.tblOrderButtons.TabIndex = 5;
            // 
            // ultraStatusBar
            // 
            this.ultraStatusBar.Location = new System.Drawing.Point(0, 33);
            this.ultraStatusBar.Name = "ultraStatusBar";
            this.ultraStatusBar.Appearance.BackColor = System.Drawing.Color.DimGray;
            this.ultraStatusBar.Appearance.ForeColor = System.Drawing.Color.White;
            this.ultraStatusBar.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraStatusBar.UseAppStyling = false;
            this.ultraStatusBar.Size = new System.Drawing.Size(947, 21);
            this.ultraStatusBar.TabIndex = 4;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Location = new System.Drawing.Point(4, 11);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(773, 111);
            this.ultraTabControl1.TabIndex = 0;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Commission";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Trade Attributes";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Other";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(196, 16);
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.pnlCommission);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(196, 16);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.tblTrade);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(196, 16);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.tblOther);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(196, 16);
            // 
            // tblOther
            // 
            this.tblOther.ColumnCount = 4;
            this.tblOther.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblOther.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblOther.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblOther.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblOther.Controls.Add(lblExecutionInstructions, 0, 0);
            this.tblOther.Controls.Add(lblHandlingInstructions, 1, 0);
            this.tblOther.Controls.Add(lblTrader, 2, 0);
            this.tblOther.Controls.Add(cmbExecution, 0, 1);
            this.tblOther.Controls.Add(cmbHandling, 1, 1);
            this.tblOther.Controls.Add(cmbTrader, 2, 1);
            this.tblOther.Location = new System.Drawing.Point(3, 3);
            this.tblOther.Name = "tblOther";
            this.tblOther.RowCount = 2;
            this.tblOther.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblOther.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblOther.Size = new System.Drawing.Size(763, 56);
            this.tblOther.TabIndex = 0;
            //
            //pnlCommission
            //
            this.pnlCommission.ColumnCount = 4;
            this.pnlCommission.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlCommission.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlCommission.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlCommission.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlCommission.Controls.Add(lblCommission, 0, 0);
            this.pnlCommission.Controls.Add(lblRate, 1, 0);
            this.pnlCommission.Controls.Add(lblSoftBasis, 2, 0);
            this.pnlCommission.Controls.Add(lblSoftRate, 3, 0);
            this.pnlCommission.Controls.Add(cmbCommission, 0, 1);
            this.pnlCommission.Controls.Add(cmbSoft, 2, 1);
            this.pnlCommission.Controls.Add(nmrcCommission, 1, 1);
            this.pnlCommission.Controls.Add(nmrcSoft, 3, 1);
            this.pnlCommission.Location = new System.Drawing.Point(3, 3);
            this.pnlCommission.Name = "pnlCommission";
            this.pnlCommission.RowCount = 2;
            this.pnlCommission.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlCommission.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlCommission.Size = new System.Drawing.Size(763, 56);
            this.pnlCommission.TabIndex = 0;

            //
            //tblTrade
            //
            this.tblTrade.ColumnCount = 6;
            this.tblTrade.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTrade.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTrade.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTrade.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTrade.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTrade.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblTrade.Controls.Add(lblTradeAttribute1, 0, 0);
            this.tblTrade.Controls.Add(lblTradeAttribute2, 1, 0);
            this.tblTrade.Controls.Add(lblTradeAttribute3, 2, 0);
            this.tblTrade.Controls.Add(lblTradeAttribute4, 3, 0);
            this.tblTrade.Controls.Add(lblTradeAttribute5, 4, 0);
            this.tblTrade.Controls.Add(lblTradeAttribute6, 5, 0);
            this.tblTrade.Controls.Add(cmbTradeAttribute1, 0, 1);
            this.tblTrade.Controls.Add(cmbTradeAttribute2, 1, 1);
            this.tblTrade.Controls.Add(cmbTradeAttribute3, 2, 1);
            this.tblTrade.Controls.Add(cmbTradeAttribute4, 3, 1);
            this.tblTrade.Controls.Add(cmbTradeAttribute5, 4, 1);
            this.tblTrade.Controls.Add(cmbTradeAttribute6, 5, 1);
            this.tblTrade.Location = new System.Drawing.Point(3, 3);
            this.tblTrade.Name = "tblTrade";
            this.tblTrade.RowCount = 2;
            this.tblTrade.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTrade.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTrade.Size = new System.Drawing.Size(763, 56);
            this.tblTrade.TabIndex = 0;

            // 
            // MultiTradingTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 400);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.MultiTradingTicket_Fill_Panel);
            this.Controls.Add(this._MultiTradingTicket_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._MultiTradingTicket_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._MultiTradingTicket_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._MultiTradingTicket_UltraFormManager_Dock_Area_Bottom);
            this.Name = "MultiTradingTicket";
            this.ShowIcon = false;
            this.Text = "Multi Trading Ticket";
            this.Shown += new System.EventHandler(this.MultiTradingTicket_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.MultiTradingTicket_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MultiTradingTicket_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxBulkUpdate)).EndInit();
            this.grpBoxBulkUpdate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxCommisionAttribute)).EndInit();
            this.grpBoxCommisionAttribute.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.BulkUpdateButtonPanel.ResumeLayout(false);
            this.pnlVenue.ClientArea.ResumeLayout(false);
            this.pnlVenue.ClientArea.PerformLayout();
            this.pnlVenue.ResumeLayout(false);
            this.pnlTIF.ClientArea.ResumeLayout(false);
            this.pnlTIF.ClientArea.PerformLayout();
            this.pnlTIF.ResumeLayout(false);
            this.pnlAllocation.ClientArea.ResumeLayout(false);
            this.pnlAllocation.ClientArea.PerformLayout();
            this.pnlAllocation.ResumeLayout(false);
            this.pnlExpireTime.ClientArea.ResumeLayout(false);
            this.pnlExpireTime.ClientArea.PerformLayout();
            this.pnlExpireTime.ResumeLayout(false);
            this.pnlBulkUpdateFields.ResumeLayout(false);
            this.pnlBulkUpdateFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSoft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbvenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBroker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcSoft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrades)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.tblOrderButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtExpireTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel MultiTradingTicket_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MultiTradingTicket_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MultiTradingTicket_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MultiTradingTicket_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MultiTradingTicket_UltraFormManager_Dock_Area_Bottom;
        private PranaUltraGrid grdTrades;
        private Infragistics.Win.Misc.UltraButton btnSend;
        private Infragistics.Win.Misc.UltraButton btnDoneAway;
        private Infragistics.Win.Misc.UltraButton btnStage;
        private Infragistics.Win.Misc.UltraButton btnReplace;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnRefreshPrice;
        private Infragistics.Win.Misc.UltraButton btnCustomAllocation;
        private Infragistics.Win.Misc.UltraButton btnAlgo;
        private Infragistics.Win.Misc.UltraButton btnExpireTime;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel pnlAllocation;
        private Infragistics.Win.Misc.UltraPanel pnlVenue;
        private Infragistics.Win.Misc.UltraPanel pnlTIF;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem saveLayout;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar;
        private System.Windows.Forms.ToolStripMenuItem removeFilter;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxBulkUpdate;
        private System.Windows.Forms.TableLayoutPanel BulkUpdateButtonPanel;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnCommit;
        private System.Windows.Forms.TableLayoutPanel pnlBulkUpdateFields;
        private PranaNumericUpDown nmrcStop;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOrderType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTIF;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbvenue;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbBroker;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAllocation;
        private Infragistics.Win.Misc.UltraLabel lblStrategy;
        private Infragistics.Win.Misc.UltraLabel lblStop;
        private Infragistics.Win.Misc.UltraLabel lblLimit;
        private Infragistics.Win.Misc.UltraLabel lblOrderType;
        private Infragistics.Win.Misc.UltraLabel lblExecutionInstructions;
        private Infragistics.Win.Misc.UltraLabel lblHandlingInstructions;
        private Infragistics.Win.Misc.UltraLabel lblTrader;
        private Infragistics.Win.Misc.UltraLabel lblCommission;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute1;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute2;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute3;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute4;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute5;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute6;
        private Infragistics.Win.Misc.UltraLabel lblRate;
        private Infragistics.Win.Misc.UltraLabel lblSoftBasis;
        private Infragistics.Win.Misc.UltraLabel lblSoftRate;
        private Infragistics.Win.Misc.UltraLabel lblTIF;
        private Infragistics.Win.Misc.UltraLabel lblVenue;
        private Infragistics.Win.Misc.UltraLabel lblBroker;
        private Infragistics.Win.Misc.UltraLabel lblAllocation;
        private Infragistics.Win.Misc.UltraLabel lblOrderSide;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOrderSide;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbStrategy;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCommission;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSoft;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbExecution;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbHandling;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTrader;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute3;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute4;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute5;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute6;
        private PranaNumericUpDown nmrcLimit;
        private PranaNumericUpDown nmrcCommission;
        private PranaNumericUpDown nmrcSoft;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpBoxCommisionAttribute;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private System.Windows.Forms.TableLayoutPanel pnlCommission;
        private System.Windows.Forms.TableLayoutPanel tblTrade;
        private System.Windows.Forms.TableLayoutPanel tblOther;
        private System.Windows.Forms.TableLayoutPanel tblOrderButtons;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtExpireTime;
        private Infragistics.Win.Misc.UltraPanel pnlExpireTime;
    }
}