using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.CashManagement.Controls
{
    partial class ctrlAccountsChart
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
                if (transactionDetails != null)
                {
                    transactionDetails.Close();
                    transactionDetails = null;
                }
                if (_dataFromDB != null)
                {
                    _dataFromDB.Dispose();
                }
                if (_proxy != null)
                {
                    _proxy.Dispose();
                    _proxy = null;
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
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tbAccountBalance = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ugbxAccountBal = new Infragistics.Win.Misc.UltraGroupBox();
            this.grdAccBalances = new PranaUltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ugbxAccountBalParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.ctrlMasterFundAndAccountsDropdown1 = new Prana.CashManagement.Controls.ctrlMasterFundAndAccountsDropdown();
            this.lblBalDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnExportBalances = new Infragistics.Win.Misc.UltraButton();
            this.btnGetAccBalances = new Infragistics.Win.Misc.UltraButton();
            this.btnRunRevaluation = new Infragistics.Win.Misc.UltraButton();
            this.udtBalanceDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnCalculateBalances = new Infragistics.Win.Misc.UltraButton();
            this.tbAccountDetails = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ugbxAccountDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.grdAccDetails = new PranaUltraGrid();
            this.ugbxAccountDetailsParams = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblSubAccounts = new Infragistics.Win.Misc.UltraLabel();
            this.btnGetAccountDetails = new Infragistics.Win.Misc.UltraButton();
            this.btnExportDetails = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbAccounts = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.udtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.udtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            //this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tbcAccountsChart = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripAccountsChart = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.tbAccountBalance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountBal)).BeginInit();
            this.ugbxAccountBal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccBalances)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountBalParams)).BeginInit();
            this.ugbxAccountBalParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udtBalanceDate)).BeginInit();
            this.tbAccountDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountDetails)).BeginInit();
            this.ugbxAccountDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAccDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountDetailsParams)).BeginInit();
            this.ugbxAccountDetailsParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcAccountsChart)).BeginInit();
            this.tbcAccountsChart.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbAccountBalance
            // 
            this.tbAccountBalance.Controls.Add(this.ugbxAccountBal);
            this.tbAccountBalance.Controls.Add(this.ugbxAccountBalParams);
            this.tbAccountBalance.Location = new System.Drawing.Point(1, 20);
            this.tbAccountBalance.Name = "tbAccountBalance";
            this.tbAccountBalance.Size = new System.Drawing.Size(1298, 589);
            // 
            // ugbxAccountBal
            // 
            this.ugbxAccountBal.Controls.Add(this.grdAccBalances);
            this.ugbxAccountBal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxAccountBal.Location = new System.Drawing.Point(0, 47);
            this.ugbxAccountBal.Name = "ugbxAccountBal";
            this.ugbxAccountBal.Size = new System.Drawing.Size(1298, 542);
            this.ugbxAccountBal.TabIndex = 104;
            // 
            // grdAccBalances
            // 
            this.grdAccBalances.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdAccBalances.DisplayLayout.Appearance = appearance1;
            this.grdAccBalances.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccBalances.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccBalances.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccBalances.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdAccBalances.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccBalances.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdAccBalances.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdAccBalances.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdAccBalances.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAccBalances.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdAccBalances.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccBalances.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccBalances.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccBalances.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccBalances.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.grdAccBalances.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAccBalances.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.grdAccBalances.DisplayLayout.Override.CellPadding = 0;
            this.grdAccBalances.DisplayLayout.Override.CellSpacing = 0;
            this.grdAccBalances.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.Color.Gray;
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance3.BorderColor = System.Drawing.Color.Black;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.grdAccBalances.DisplayLayout.Override.GroupByRowAppearance = appearance3;
            this.grdAccBalances.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.grdAccBalances.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance4.BorderColor = System.Drawing.Color.Transparent;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.grdAccBalances.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance4;
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.Name = "Segoe UI";
            appearance5.FontData.SizeInPoints = 9F;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.grdAccBalances.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.grdAccBalances.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdAccBalances.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdAccBalances.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Middle";
            this.grdAccBalances.DisplayLayout.Override.RowAlternateAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.Black;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Right";
            appearance7.TextVAlignAsString = "Middle";
            this.grdAccBalances.DisplayLayout.Override.RowAppearance = appearance7;
            this.grdAccBalances.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdAccBalances.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccBalances.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            appearance8.FontData.BoldAsString = "True";
            this.grdAccBalances.DisplayLayout.Override.SelectedRowAppearance = appearance8;
            this.grdAccBalances.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccBalances.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccBalances.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdAccBalances.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccBalances.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdAccBalances.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdAccBalances.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance9.BorderColor = System.Drawing.Color.Transparent;
            this.grdAccBalances.DisplayLayout.Override.SummaryFooterAppearance = appearance9;
            this.grdAccBalances.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Transparent;
            appearance10.FontData.BoldAsString = "False";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.grdAccBalances.DisplayLayout.Override.SummaryValueAppearance = appearance10;
            this.grdAccBalances.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance11.TextHAlignAsString = "Center";
            this.grdAccBalances.DisplayLayout.Override.TemplateAddRowAppearance = appearance11;
            this.grdAccBalances.DisplayLayout.PriorityScrolling = true;
            this.grdAccBalances.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdAccBalances.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccBalances.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccBalances.DisplayLayout.UseFixedHeaders = true;
            this.grdAccBalances.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAccBalances.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccBalances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAccBalances.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdAccBalances.Location = new System.Drawing.Point(3, 3);
            this.grdAccBalances.Name = "grdAccBalances";
            this.grdAccBalances.Size = new System.Drawing.Size(1292, 536);
            this.grdAccBalances.TabIndex = 10;
            this.grdAccBalances.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccBalances.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccBalances_InitializeLayout);
            this.grdAccBalances.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdAccBalances_InitializeGroupByRow);
            this.grdAccBalances.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdAccBalances_DoubleClickRow);
            this.grdAccBalances.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdAccBalances_AfterSortChange);
            this.grdAccBalances.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAccBalances_BeforeCustomRowFilterDialog);
            this.grdAccBalances.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAccBalances_BeforeColumnChooserDisplayed);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            this.inboxControlStyler1.SetStyleSettings(this.contextMenuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // ugbxAccountBalParams
            // 
            this.ugbxAccountBalParams.Controls.Add(this.ctrlMasterFundAndAccountsDropdown1);
            this.ugbxAccountBalParams.Controls.Add(this.lblBalDate);
            this.ugbxAccountBalParams.Controls.Add(this.btnExportBalances);
            this.ugbxAccountBalParams.Controls.Add(this.btnGetAccBalances);
            this.ugbxAccountBalParams.Controls.Add(this.btnRunRevaluation);
            this.ugbxAccountBalParams.Controls.Add(this.udtBalanceDate);
            this.ugbxAccountBalParams.Controls.Add(this.btnCalculateBalances);
            this.ugbxAccountBalParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxAccountBalParams.Location = new System.Drawing.Point(0, 0);
            this.ugbxAccountBalParams.Name = "ugbxAccountBalParams";
            this.ugbxAccountBalParams.Size = new System.Drawing.Size(1298, 47);
            this.ugbxAccountBalParams.TabIndex = 103;
            // 
            // ctrlMasterFundAndAccountsDropdown1
            // 
            this.ctrlMasterFundAndAccountsDropdown1.Location = new System.Drawing.Point(3, 2);
            this.ctrlMasterFundAndAccountsDropdown1.Name = "ctrlMasterFundAndAccountsDropdown1";
            this.ctrlMasterFundAndAccountsDropdown1.Size = new System.Drawing.Size(488, 36);
            this.ctrlMasterFundAndAccountsDropdown1.TabIndex = 8;
            // 
            // lblBalDate
            // 
            appearance12.FontData.SizeInPoints = 9F;
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.TextHAlignAsString = "Left";
            appearance12.TextVAlignAsString = "Middle";
            this.lblBalDate.Appearance = appearance12;
            this.lblBalDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalDate.Location = new System.Drawing.Point(488, 12);
            this.lblBalDate.Name = "lblBalDate";
            this.lblBalDate.Size = new System.Drawing.Size(112, 23);
            this.lblBalDate.TabIndex = 0;
            this.lblBalDate.Text = "Balances as on date";
            // 
            // btnExportBalances
            // 
            appearance13.FontData.SizeInPoints = 9F;
            this.btnExportBalances.Appearance = appearance13;
            this.btnExportBalances.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportBalances.Location = new System.Drawing.Point(1019, 12);
            this.btnExportBalances.Name = "btnExportBalances";
            this.btnExportBalances.Size = new System.Drawing.Size(119, 23);
            this.btnExportBalances.TabIndex = 4;
            this.btnExportBalances.Text = "Export to Excel";
            this.btnExportBalances.Click += new System.EventHandler(this.btnExportBalances_Click);
            // 
            // btnGetAccBalances
            // 
            appearance14.FontData.SizeInPoints = 9F;
            this.btnGetAccBalances.Appearance = appearance14;
            this.btnGetAccBalances.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetAccBalances.Location = new System.Drawing.Point(848, 12);
            this.btnGetAccBalances.Name = "btnGetAccBalances";
            this.btnGetAccBalances.Size = new System.Drawing.Size(165, 23);
            this.btnGetAccBalances.TabIndex = 4;
            this.btnGetAccBalances.Text = "Get Account Balances";
            this.btnGetAccBalances.Click += new System.EventHandler(this.btnGetAccBalances_Click);
            // 
            // btnRunRevaluation
            // 
            appearance15.FontData.SizeInPoints = 9F;
            this.btnRunRevaluation.Appearance = appearance15;
            this.btnRunRevaluation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunRevaluation.Location = new System.Drawing.Point(714, 12);
            this.btnRunRevaluation.Name = "btnRunRevaluation";
            this.btnRunRevaluation.Size = new System.Drawing.Size(128, 23);
            this.btnRunRevaluation.TabIndex = 5;
            this.btnRunRevaluation.Text = "Run Revaluation";
            this.btnRunRevaluation.Click += new System.EventHandler(this.btnRunRevaluation_Click);
            // 
            // udtBalanceDate
            // 
            appearance16.FontData.SizeInPoints = 9F;
            this.udtBalanceDate.Appearance = appearance16;
            this.udtBalanceDate.AutoSize = false;
            this.udtBalanceDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udtBalanceDate.Location = new System.Drawing.Point(603, 12);
            this.udtBalanceDate.Name = "udtBalanceDate";
            this.udtBalanceDate.Size = new System.Drawing.Size(109, 23);
            this.udtBalanceDate.TabIndex = 1;
            // 
            // btnCalculateBalances
            // 
            appearance17.FontData.SizeInPoints = 9F;
            this.btnCalculateBalances.Appearance = appearance17;
            this.btnCalculateBalances.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculateBalances.Location = new System.Drawing.Point(1144, 12);
            this.btnCalculateBalances.Name = "btnCalculateBalances";
            this.btnCalculateBalances.Size = new System.Drawing.Size(144, 23);
            this.btnCalculateBalances.TabIndex = 4;
            this.btnCalculateBalances.Text = "Calculate Balances";
            this.btnCalculateBalances.Visible = false;
            this.btnCalculateBalances.Click += new System.EventHandler(this.btnCalculateBalances_Click);
            // 
            // tbAccountDetails
            // 
            this.tbAccountDetails.Controls.Add(this.ugbxAccountDetails);
            this.tbAccountDetails.Controls.Add(this.ugbxAccountDetailsParams);
            this.tbAccountDetails.Location = new System.Drawing.Point(-10000, -10000);
            this.tbAccountDetails.Name = "tbAccountDetails";
            this.tbAccountDetails.Size = new System.Drawing.Size(1298, 589);
            // 
            // ugbxAccountDetails
            // 
            this.ugbxAccountDetails.Controls.Add(this.grdAccDetails);
            this.ugbxAccountDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugbxAccountDetails.Location = new System.Drawing.Point(0, 47);
            this.ugbxAccountDetails.Name = "ugbxAccountDetails";
            this.ugbxAccountDetails.Size = new System.Drawing.Size(1298, 542);
            this.ugbxAccountDetails.TabIndex = 12;
            // 
            // grdAccDetails
            // 
            appearance18.BackColor = System.Drawing.Color.Black;
            this.grdAccDetails.DisplayLayout.Appearance = appearance18;
            this.grdAccDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccDetails.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccDetails.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdAccDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccDetails.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.Color.LightSlateGray;
            appearance19.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance19.BorderColor = System.Drawing.Color.DimGray;
            appearance19.FontData.BoldAsString = "True";
            appearance19.ForeColor = System.Drawing.Color.White;
            this.grdAccDetails.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            this.grdAccDetails.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdAccDetails.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAccDetails.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdAccDetails.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccDetails.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccDetails.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccDetails.DisplayLayout.Override.BorderStyleRowSelector = Infragistics.Win.UIElementBorderStyle.None;
            this.grdAccDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAccDetails.DisplayLayout.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            this.grdAccDetails.DisplayLayout.Override.CellPadding = 0;
            this.grdAccDetails.DisplayLayout.Override.CellSpacing = 0;
            this.grdAccDetails.DisplayLayout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.Color.Gray;
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance20.BorderColor = System.Drawing.Color.Black;
            appearance20.FontData.BoldAsString = "True";
            appearance20.ForeColor = System.Drawing.Color.White;
            this.grdAccDetails.DisplayLayout.Override.GroupByRowAppearance = appearance20;
            this.grdAccDetails.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.grdAccDetails.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance21.BorderColor = System.Drawing.Color.Transparent;
            appearance21.TextHAlignAsString = "Right";
            appearance21.TextVAlignAsString = "Middle";
            this.grdAccDetails.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance21;
            appearance22.FontData.BoldAsString = "False";
            appearance22.FontData.Name = "Segoe UI";
            appearance22.FontData.SizeInPoints = 9F;
            appearance22.TextHAlignAsString = "Center";
            appearance22.TextVAlignAsString = "Middle";
            this.grdAccDetails.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.grdAccDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdAccDetails.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdAccDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.TextHAlignAsString = "Right";
            appearance23.TextVAlignAsString = "Middle";
            this.grdAccDetails.DisplayLayout.Override.RowAlternateAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.Black;
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Right";
            appearance24.TextVAlignAsString = "Middle";
            this.grdAccDetails.DisplayLayout.Override.RowAppearance = appearance24;
            this.grdAccDetails.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdAccDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccDetails.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance25.BackColor = System.Drawing.Color.Transparent;
            appearance25.BorderColor = System.Drawing.Color.Transparent;
            appearance25.FontData.BoldAsString = "True";
            this.grdAccDetails.DisplayLayout.Override.SelectedRowAppearance = appearance25;
            this.grdAccDetails.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccDetails.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccDetails.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdAccDetails.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccDetails.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdAccDetails.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdAccDetails.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance26.BorderColor = System.Drawing.Color.Transparent;
            this.grdAccDetails.DisplayLayout.Override.SummaryFooterAppearance = appearance26;
            this.grdAccDetails.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.Color.Transparent;
            appearance27.BorderColor = System.Drawing.Color.Transparent;
            appearance27.FontData.BoldAsString = "False";
            appearance27.ForeColor = System.Drawing.Color.White;
            this.grdAccDetails.DisplayLayout.Override.SummaryValueAppearance = appearance27;
            this.grdAccDetails.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance28.TextHAlignAsString = "Center";
            this.grdAccDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this.grdAccDetails.DisplayLayout.PriorityScrolling = true;
            this.grdAccDetails.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdAccDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccDetails.DisplayLayout.UseFixedHeaders = true;
            this.grdAccDetails.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAccDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAccDetails.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdAccDetails.Location = new System.Drawing.Point(3, 3);
            this.grdAccDetails.Name = "grdAccDetails";
            this.grdAccDetails.Size = new System.Drawing.Size(1292, 536);
            this.grdAccDetails.TabIndex = 10;
            this.grdAccDetails.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccDetails.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAccDetails_InitializeLayout);
            this.grdAccDetails.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdAccDetails_DoubleClickRow);
            this.grdAccDetails.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAccDetails_BeforeCustomRowFilterDialog);
            this.grdAccDetails.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAccDetails_BeforeColumnChooserDisplayed);
            // 
            // ugbxAccountDetailsParams
            // 
            this.ugbxAccountDetailsParams.Controls.Add(this.lblSubAccounts);
            this.ugbxAccountDetailsParams.Controls.Add(this.btnGetAccountDetails);
            this.ugbxAccountDetailsParams.Controls.Add(this.btnExportDetails);
            this.ugbxAccountDetailsParams.Controls.Add(this.ultraLabel1);
            this.ugbxAccountDetailsParams.Controls.Add(this.cmbAccounts);
            this.ugbxAccountDetailsParams.Controls.Add(this.udtFromDate);
            this.ugbxAccountDetailsParams.Controls.Add(this.ultraLabel2);
            this.ugbxAccountDetailsParams.Controls.Add(this.udtToDate);
            this.ugbxAccountDetailsParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxAccountDetailsParams.Location = new System.Drawing.Point(0, 0);
            this.ugbxAccountDetailsParams.Name = "ugbxAccountDetailsParams";
            this.ugbxAccountDetailsParams.Size = new System.Drawing.Size(1298, 47);
            this.ugbxAccountDetailsParams.TabIndex = 11;
            // 
            // lblSubAccounts
            // 
            appearance29.FontData.SizeInPoints = 9F;
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.TextHAlignAsString = "Left";
            appearance29.TextVAlignAsString = "Middle";
            this.lblSubAccounts.Appearance = appearance29;
            this.lblSubAccounts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubAccounts.Location = new System.Drawing.Point(6, 12);
            this.lblSubAccounts.Name = "lblSubAccounts";
            this.lblSubAccounts.Size = new System.Drawing.Size(51, 23);
            this.lblSubAccounts.TabIndex = 5;
            this.lblSubAccounts.Text = "Account";
            // 
            // btnGetAccountDetails
            // 
            appearance30.FontData.SizeInPoints = 9F;
            this.btnGetAccountDetails.Appearance = appearance30;
            this.btnGetAccountDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetAccountDetails.Location = new System.Drawing.Point(636, 12);
            this.btnGetAccountDetails.Name = "btnGetAccountDetails";
            this.btnGetAccountDetails.Size = new System.Drawing.Size(154, 23);
            this.btnGetAccountDetails.TabIndex = 4;
            this.btnGetAccountDetails.Text = "Get Account Details";
            this.btnGetAccountDetails.Click += new System.EventHandler(this.btnGetAccountDetails_Click);
            // 
            // btnExportDetails
            // 
            appearance31.FontData.SizeInPoints = 9F;
            this.btnExportDetails.Appearance = appearance31;
            this.btnExportDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportDetails.Location = new System.Drawing.Point(796, 12);
            this.btnExportDetails.Name = "btnExportDetails";
            this.btnExportDetails.Size = new System.Drawing.Size(127, 23);
            this.btnExportDetails.TabIndex = 7;
            this.btnExportDetails.Text = "Export to Excel";
            this.btnExportDetails.Click += new System.EventHandler(this.btnExportDetails_Click);
            // 
            // ultraLabel1
            // 
            appearance32.FontData.SizeInPoints = 9F;
            appearance32.ForeColor = System.Drawing.Color.Black;
            appearance32.TextHAlignAsString = "Left";
            appearance32.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance32;
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(492, 12);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(23, 23);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "To";
            // 
            // cmbAccounts
            // 
            appearance33.FontData.SizeInPoints = 9F;
            this.cmbAccounts.Appearance = appearance33;
            this.cmbAccounts.AutoSize = false;
            this.cmbAccounts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccounts.Location = new System.Drawing.Point(63, 12);
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new System.Drawing.Size(262, 23);
            this.cmbAccounts.TabIndex = 6;
            // 
            // udtFromDate
            // 
            appearance34.FontData.SizeInPoints = 9F;
            this.udtFromDate.Appearance = appearance34;
            this.udtFromDate.AutoSize = false;
            this.udtFromDate.DateTime = new System.DateTime(2012, 2, 1, 0, 0, 0, 0);
            this.udtFromDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udtFromDate.Location = new System.Drawing.Point(377, 12);
            this.udtFromDate.Name = "udtFromDate";
            this.udtFromDate.Size = new System.Drawing.Size(109, 23);
            this.udtFromDate.TabIndex = 1;
            this.udtFromDate.Value = new System.DateTime(2012, 2, 1, 0, 0, 0, 0);
            // 
            // ultraLabel2
            // 
            appearance35.FontData.SizeInPoints = 9F;
            appearance35.ForeColor = System.Drawing.Color.Black;
            appearance35.TextHAlignAsString = "Left";
            appearance35.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance35;
            this.ultraLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(331, 12);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(40, 23);
            this.ultraLabel2.TabIndex = 0;
            this.ultraLabel2.Text = "From";
            // 
            // udtToDate
            // 
            appearance36.FontData.SizeInPoints = 9F;
            this.udtToDate.Appearance = appearance36;
            this.udtToDate.AutoSize = false;
            this.udtToDate.DateTime = new System.DateTime(2012, 2, 1, 0, 0, 0, 0);
            this.udtToDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udtToDate.Location = new System.Drawing.Point(521, 12);
            this.udtToDate.Name = "udtToDate";
            this.udtToDate.Size = new System.Drawing.Size(109, 23);
            this.udtToDate.TabIndex = 2;
            this.udtToDate.Value = new System.DateTime(2012, 2, 1, 0, 0, 0, 0);
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(707, 187);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // tbcAccountsChart
            // 
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance37.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tbcAccountsChart.ActiveTabAppearance = appearance37;
            appearance38.FontData.SizeInPoints = 9F;
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.tbcAccountsChart.Appearance = appearance38;
            this.tbcAccountsChart.ContextMenuStrip = this.contextMenuStrip1;
            this.tbcAccountsChart.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbcAccountsChart.Controls.Add(this.tbAccountBalance);
            this.tbcAccountsChart.Controls.Add(this.tbAccountDetails);
            this.tbcAccountsChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcAccountsChart.Location = new System.Drawing.Point(0, 0);
            this.tbcAccountsChart.Name = "tbcAccountsChart";
            this.tbcAccountsChart.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbcAccountsChart.Size = new System.Drawing.Size(1300, 610);
            this.tbcAccountsChart.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tbcAccountsChart.StyleSetName = "AccountsChartStyleSet";
            this.tbcAccountsChart.TabIndex = 2;
            this.tbcAccountsChart.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.TopLeft;
            ultraTab1.Key = "tbAccBalances";
            ultraTab1.TabPage = this.tbAccountBalance;
            ultraTab1.Text = "Account Balances";
            ultraTab2.Key = "tbAccDetails";
            ultraTab2.TabPage = this.tbAccountDetails;
            ultraTab2.Text = "Account Details";
            this.tbcAccountsChart.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1298, 589);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripAccountsChart});
            this.statusStrip1.Location = new System.Drawing.Point(0, 610);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1300, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripAccountsChart
            // 
            this.toolStripAccountsChart.ForeColor = System.Drawing.Color.Black;
            this.toolStripAccountsChart.Name = "toolStripAccountsChart";
            this.toolStripAccountsChart.Size = new System.Drawing.Size(0, 17);
            this.toolStripAccountsChart.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.BackColor = System.Drawing.Color.White;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1300, 632);
            this.ultraPanel1.TabIndex = 4;
            // 
            // ctrlAccountsChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tbcAccountsChart);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ultraPanel1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "ctrlAccountsChart";
            this.Size = new System.Drawing.Size(1300, 632);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings("AccountsChartStyleSet", Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.ctrlAccountsChart_Load);
            this.tbAccountBalance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountBal)).EndInit();
            this.ugbxAccountBal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccBalances)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountBalParams)).EndInit();
            this.ugbxAccountBalParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udtBalanceDate)).EndInit();
            this.tbAccountDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountDetails)).EndInit();
            this.ugbxAccountDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxAccountDetailsParams)).EndInit();
            this.ugbxAccountDetailsParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udtFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbcAccountsChart)).EndInit();
            this.tbcAccountsChart.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        
        //private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tbcAccountsChart;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbAccountBalance;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbAccountDetails;
        private Infragistics.Win.Misc.UltraButton btnGetAccBalances;
        private Infragistics.Win.Misc.UltraLabel lblBalDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtBalanceDate;
        private PranaUltraGrid grdAccBalances;
        private Infragistics.Win.Misc.UltraButton btnGetAccountDetails;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtToDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor udtFromDate;
        private PranaUltraGrid grdAccDetails;
        private Infragistics.Win.Misc.UltraLabel lblSubAccounts;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAccounts;
        private Infragistics.Win.Misc.UltraButton btnCalculateBalances;
        private Infragistics.Win.Misc.UltraButton btnExportBalances;
        private Infragistics.Win.Misc.UltraButton btnExportDetails;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripAccountsChart;
        private Infragistics.Win.Misc.UltraButton btnRunRevaluation;
        private Infragistics.Win.Misc.UltraGroupBox ugbxAccountBal;
        private Infragistics.Win.Misc.UltraGroupBox ugbxAccountBalParams;
        private Infragistics.Win.Misc.UltraGroupBox ugbxAccountDetails;
        private Infragistics.Win.Misc.UltraGroupBox ugbxAccountDetailsParams;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private ctrlMasterFundAndAccountsDropdown ctrlMasterFundAndAccountsDropdown1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
    }
}
