using Prana.BusinessObjects.Constants;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlMarkPriceAndForexConversion
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
                if (_pricingServicesProxy != null)
                {
                    _pricingServicesProxy.Dispose();
                    _pricingServicesProxy = null;
                }

                if (_SubscriptionProxy != null && _SubscriptionProxy.InnerChannel != null)
                {
                    _SubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DayEndCash);
                    _SubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_DailyCreditLimit);
                    _SubscriptionProxy.Dispose();
                    _SubscriptionProxy = null;
                }
                if (cmbFromCurrency != null)
                {
                    cmbFromCurrency.Dispose();
                }
                if (cmbToCurrency != null)
                {
                    cmbToCurrency.Dispose();
                }
                if (cmbLocalCurrency != null)
                {
                    cmbLocalCurrency.Dispose();
                }
                if (cmbBaseCurrency != null)
                {
                    cmbBaseCurrency.Dispose();
                }
                if (cmbAccount != null)
                {
                    cmbAccount.Dispose();
                }
                if (_dtGridDataSource != null)
                {
                    _dtGridDataSource.Dispose();
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
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.grpBoxImportExport = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnImport = new Infragistics.Win.Misc.UltraButton();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.btnGetLiveFeedData = new Infragistics.Win.Misc.UltraButton();
            this.optUseImportExport = new System.Windows.Forms.RadioButton();
            this.optUseLiveFeed = new System.Windows.Forms.RadioButton();
            this.cmbExchangeGroup = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblExchangeGroup = new Infragistics.Win.Misc.UltraLabel();
            this.lblSelectLiveFeedDate = new Infragistics.Win.Misc.UltraLabel();
            this.grpBoxLiveFeedWhole = new Infragistics.Win.Misc.UltraGroupBox();
            this.optIMidPrice = new System.Windows.Forms.RadioButton();
            this.optMidPrice = new System.Windows.Forms.RadioButton();
            this.optSelectedFeedPrice = new System.Windows.Forms.RadioButton();
            this.lblLiveFeedDatePrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblUpdatePrice = new Infragistics.Win.Misc.UltraLabel();
            this.grpBoxSymbolUpdationChoice = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.optOverwriteAllZeroSymbols = new System.Windows.Forms.RadioButton();
            this.optOverwriteAllSymbols = new System.Windows.Forms.RadioButton();
            this.optLastPrice = new System.Windows.Forms.RadioButton();
            this.optPreviousPrice = new System.Windows.Forms.RadioButton();
            this.lblLastDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblPreviousDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblIMidDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblMidDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblSelectedFeedDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtLiveFeed = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.optOMIPrice = new System.Windows.Forms.RadioButton();
            this.lblSelectDateView = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.btnGetFilteredData = new Infragistics.Win.Misc.UltraButton();
            this.lblFilteredSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.txtSymbolFilteration = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnClearFilter = new Infragistics.Win.Misc.UltraButton();
            this.grpBoxFilter = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblCopyFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtCopyFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnFetchData = new Infragistics.Win.Misc.UltraButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.lblCopyFrom = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCopyFromAccount = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblCopyTo = new Infragistics.Win.Misc.UltraLabel();
            this.multiSelectDropDown1 = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.btnAccountCopy = new Infragistics.Win.Misc.UltraButton();
            this.pnlAccountCopy = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grdPivotDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxGrid)).BeginInit();
            this.grpBoxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpSelectDateMethodology)).BeginInit();
            this.grpSelectDateMethodology.SuspendLayout();
            this.ultrapanelTop.ClientArea.SuspendLayout();
            this.ultrapanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxImportExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxLiveFeedHandling)).BeginInit();
            this.grpBoxLiveFeedHandling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExchangeGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxLiveFeedWhole)).BeginInit();
            this.grpBoxLiveFeedWhole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxSymbolUpdationChoice)).BeginInit();
            this.grpBoxSymbolUpdationChoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtLiveFeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymbolFilteration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCopyFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCopyFromAccount)).BeginInit();
            this.pnlAccountCopy.ClientArea.SuspendLayout();
            this.pnlAccountCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // optDaily
            // 
            this.optDaily.Location = new System.Drawing.Point(6, 31);
            this.optDaily.Size = new System.Drawing.Size(51, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optDaily, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optDaily.CheckedChanged += new System.EventHandler(this.optDaily_CheckedChanged);
            // 
            // grdPivotDisplay
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdPivotDisplay.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdPivotDisplay.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPivotDisplay.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPivotDisplay.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdPivotDisplay.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPivotDisplay.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPivotDisplay.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPivotDisplay.DisplayLayout.Override.ActiveCellAppearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.Highlight;
            appearance3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdPivotDisplay.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            this.grdPivotDisplay.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdPivotDisplay.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdPivotDisplay.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            this.grdPivotDisplay.DisplayLayout.Override.CardAreaAppearance = appearance4;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdPivotDisplay.DisplayLayout.Override.CellAppearance = appearance5;
            this.grdPivotDisplay.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdPivotDisplay.DisplayLayout.Override.CellPadding = 0;
            appearance6.BackColor = System.Drawing.SystemColors.Control;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPivotDisplay.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            appearance7.TextHAlignAsString = "Left";
            this.grdPivotDisplay.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdPivotDisplay.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdPivotDisplay.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            this.grdPivotDisplay.DisplayLayout.Override.RowAppearance = appearance8;
            this.grdPivotDisplay.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdPivotDisplay.DisplayLayout.Override.TemplateAddRowAppearance = appearance9;
            this.grdPivotDisplay.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdPivotDisplay.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdPivotDisplay.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.grdPivotDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPivotDisplay.Location = new System.Drawing.Point(3, 3);
            this.grdPivotDisplay.Size = new System.Drawing.Size(1088, 327);
            this.grdPivotDisplay.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPivotDisplay_AfterCellUpdate);
            this.grdPivotDisplay.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPivotDisplay_InitializeLayout);
            this.grdPivotDisplay.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdPivotDisplay_InitializeRow);
            this.grdPivotDisplay.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdPivotDisplay_CellChange);
            this.grdPivotDisplay.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdPivotDisplay_Error);
            this.grdPivotDisplay.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdPivotDisplay_DragDrop);
            this.grdPivotDisplay.DragEnter += new System.Windows.Forms.DragEventHandler(this.grdPivotDisplay_DragEnter);
            // 
            // dtDateMonth
            // 
            appearance10.FontData.SizeInPoints = 9F;
            this.dtDateMonth.Appearance = appearance10;
            this.dtDateMonth.AutoSize = false;
            this.dtDateMonth.Location = new System.Drawing.Point(109, 60);
            this.dtDateMonth.Size = new System.Drawing.Size(131, 25);
            this.dtDateMonth.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dtDateMonth.TabIndex = 0;
            this.dtDateMonth.ValueChanged += new System.EventHandler(this.dtDateMonth_ValueChanged);
            this.dtDateMonth.AfterCloseUp += new System.EventHandler(this.dtDateMonth_AfterCloseUp);
            this.dtDateMonth.Leave += new System.EventHandler(this.dtDateMonth_Leave);
            // 
            // optMonthly
            // 
            this.optMonthly.Location = new System.Drawing.Point(71, 31);
            this.optMonthly.Size = new System.Drawing.Size(70, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optMonthly, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optMonthly.TabIndex = 1;
            this.optMonthly.CheckedChanged += new System.EventHandler(this.optMonthly_CheckedChanged);
            // 
            // optWeekly
            // 
            this.optWeekly.Location = new System.Drawing.Point(157, 31);
            this.optWeekly.Size = new System.Drawing.Size(63, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optWeekly, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optWeekly.TabIndex = 2;
            this.optWeekly.Visible = false;
            // 
            // grpBoxGrid
            // 
            this.grpBoxGrid.Controls.Add(this.grpBoxFilter);
            this.grpBoxGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxGrid.Location = new System.Drawing.Point(0, 272);
            this.grpBoxGrid.Size = new System.Drawing.Size(1094, 333);
            this.grpBoxGrid.TabIndex = 3;
            this.grpBoxGrid.Controls.SetChildIndex(this.grpBoxFilter, 0);
            this.grpBoxGrid.Controls.SetChildIndex(this.grdPivotDisplay, 0);
            // 
            // grpSelectDateMethodology
            // 
            appearance11.FontData.SizeInPoints = 9F;
            this.grpSelectDateMethodology.Appearance = appearance11;
            this.grpSelectDateMethodology.Controls.Add(this.pnlAccountCopy);
            this.grpSelectDateMethodology.Controls.Add(this.lblFilteredSymbol);
            this.grpSelectDateMethodology.Controls.Add(this.txtSymbolFilteration);
            this.grpSelectDateMethodology.Controls.Add(this.btnGetFilteredData);
            this.grpSelectDateMethodology.Controls.Add(this.btnClearFilter);
            this.grpSelectDateMethodology.Controls.Add(this.btnFetchData);
            this.grpSelectDateMethodology.Controls.Add(this.dtCopyFromDate);
            this.grpSelectDateMethodology.Controls.Add(this.lblCopyFromDate);
            this.grpSelectDateMethodology.Controls.Add(this.lblSelectDateView);
            this.grpSelectDateMethodology.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpSelectDateMethodology.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSelectDateMethodology.Location = new System.Drawing.Point(0, 0);
            this.grpSelectDateMethodology.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.grpSelectDateMethodology.Size = new System.Drawing.Size(316, 272);
            this.grpSelectDateMethodology.TabIndex = 1;
            this.grpSelectDateMethodology.Text = "Select View";
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.dtDateMonth, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.lblSelectDateView, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.lblCopyFromDate, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.dtCopyFromDate, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.btnFetchData, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.btnClearFilter, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.btnGetFilteredData, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.txtSymbolFilteration, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.lblFilteredSymbol, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.optDaily, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.optMonthly, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.optWeekly, 0);
            this.grpSelectDateMethodology.Controls.SetChildIndex(this.pnlAccountCopy, 0);
            // 
            // ultrapanelTop
            // 
            // 
            // ultrapanelTop.ClientArea
            // 
            this.ultrapanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultrapanelTop.Location = new System.Drawing.Point(0, 0);
            this.ultrapanelTop.Size = new System.Drawing.Size(1094, 272);
            this.ultrapanelTop.TabIndex = 10;
            // 
            // grpBoxImportExport
            // 
            this.grpBoxImportExport.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpBoxImportExport.Location = new System.Drawing.Point(52, 40);
            this.grpBoxImportExport.Name = "grpBoxImportExport";
            this.grpBoxImportExport.Size = new System.Drawing.Size(188, 32);
            this.grpBoxImportExport.TabIndex = 3;
            // 
            // btnImport
            // 
            appearance20.FontData.SizeInPoints = 9F;
            this.btnImport.Appearance = appearance20;
            this.btnImport.Location = new System.Drawing.Point(87, 171);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            appearance21.FontData.SizeInPoints = 9F;
            this.btnExport.Appearance = appearance21;
            this.btnExport.Location = new System.Drawing.Point(178, 171);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnGetLiveFeedData
            // 
            this.btnGetLiveFeedData.Location = new System.Drawing.Point(417, 56);
            this.btnGetLiveFeedData.Name = "btnGetLiveFeedData";
            this.btnGetLiveFeedData.Size = new System.Drawing.Size(100, 23);
            this.btnGetLiveFeedData.TabIndex = 6;
            this.btnGetLiveFeedData.Text = "Get Prices";
            this.btnGetLiveFeedData.Click += new System.EventHandler(this.btnGetLiveFeedData_Click);
            // 
            // grpBoxLiveFeedHandling
            // 
            appearance19.FontData.SizeInPoints = 9F;
            this.grpBoxLiveFeedHandling.Appearance = appearance19;
            this.grpBoxLiveFeedHandling.Controls.Add(this.btnImport);
            this.grpBoxLiveFeedHandling.Controls.Add(this.btnExport);
            this.grpBoxLiveFeedHandling.Controls.Add(this.optUseImportExport);
            this.grpBoxLiveFeedHandling.Controls.Add(this.optUseLiveFeed);
            this.grpBoxLiveFeedHandling.Controls.Add(this.cmbExchangeGroup);
            this.grpBoxLiveFeedHandling.Controls.Add(this.lblExchangeGroup);
            this.grpBoxLiveFeedHandling.Controls.Add(this.lblSelectLiveFeedDate);
            this.grpBoxLiveFeedHandling.Controls.Add(this.grpBoxLiveFeedWhole);
            this.grpBoxLiveFeedHandling.Controls.Add(this.dtLiveFeed);
            this.grpBoxLiveFeedHandling.Controls.Add(this.optOMIPrice);
            this.grpBoxLiveFeedHandling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxLiveFeedHandling.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxLiveFeedHandling.Location = new System.Drawing.Point(316, 0);
            this.grpBoxLiveFeedHandling.Size = new System.Drawing.Size(778, 272);
            this.grpBoxLiveFeedHandling.TabIndex = 2;
            // 
            // optUseImportExport
            // 
            this.optUseImportExport.AutoSize = true;
            this.optUseImportExport.Location = new System.Drawing.Point(7, 170);
            this.optUseImportExport.Name = "optUseImportExport";
            this.optUseImportExport.Size = new System.Drawing.Size(74, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optUseImportExport, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optUseImportExport.TabIndex = 6;
            this.optUseImportExport.TabStop = true;
            this.optUseImportExport.Text = "From File";
            this.optUseImportExport.UseVisualStyleBackColor = true;
            this.optUseImportExport.CheckedChanged += new System.EventHandler(this.optUseImportExport_CheckedChanged);
            // 
            // optUseLiveFeed
            // 
            this.optUseLiveFeed.AutoSize = true;
            this.optUseLiveFeed.Location = new System.Drawing.Point(7, 59);
            this.optUseLiveFeed.Name = "optUseLiveFeed";
            this.optUseLiveFeed.Size = new System.Drawing.Size(105, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optUseLiveFeed, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optUseLiveFeed.TabIndex = 4;
            this.optUseLiveFeed.TabStop = true;
            this.optUseLiveFeed.Text = "From Provider";
            this.optUseLiveFeed.UseVisualStyleBackColor = true;
            this.optUseLiveFeed.CheckedChanged += new System.EventHandler(this.optUseLiveFeed_CheckedChanged);
            // 
            // cmbExchangeGroup
            // 
            appearance22.FontData.SizeInPoints = 9F;
            this.cmbExchangeGroup.Appearance = appearance22;
            this.cmbExchangeGroup.AutoSize = false;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbExchangeGroup.DisplayLayout.Appearance = appearance23;
            this.cmbExchangeGroup.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExchangeGroup.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExchangeGroup.DisplayLayout.GroupByBox.Appearance = appearance24;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExchangeGroup.DisplayLayout.GroupByBox.BandLabelAppearance = appearance25;
            this.cmbExchangeGroup.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance26.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExchangeGroup.DisplayLayout.GroupByBox.PromptAppearance = appearance26;
            this.cmbExchangeGroup.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbExchangeGroup.DisplayLayout.MaxRowScrollRegions = 1;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbExchangeGroup.DisplayLayout.Override.ActiveCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.SystemColors.Highlight;
            appearance28.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbExchangeGroup.DisplayLayout.Override.ActiveRowAppearance = appearance28;
            this.cmbExchangeGroup.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbExchangeGroup.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            this.cmbExchangeGroup.DisplayLayout.Override.CardAreaAppearance = appearance29;
            appearance30.BorderColor = System.Drawing.Color.Silver;
            appearance30.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbExchangeGroup.DisplayLayout.Override.CellAppearance = appearance30;
            this.cmbExchangeGroup.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbExchangeGroup.DisplayLayout.Override.CellPadding = 0;
            appearance31.BackColor = System.Drawing.SystemColors.Control;
            appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance31.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance31.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExchangeGroup.DisplayLayout.Override.GroupByRowAppearance = appearance31;
            appearance32.TextHAlignAsString = "Left";
            this.cmbExchangeGroup.DisplayLayout.Override.HeaderAppearance = appearance32;
            this.cmbExchangeGroup.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbExchangeGroup.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            appearance33.BorderColor = System.Drawing.Color.Silver;
            this.cmbExchangeGroup.DisplayLayout.Override.RowAppearance = appearance33;
            this.cmbExchangeGroup.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance34.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbExchangeGroup.DisplayLayout.Override.TemplateAddRowAppearance = appearance34;
            this.cmbExchangeGroup.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbExchangeGroup.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbExchangeGroup.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbExchangeGroup.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbExchangeGroup.Location = new System.Drawing.Point(345, 33);
            this.cmbExchangeGroup.Name = "cmbExchangeGroup";
            this.cmbExchangeGroup.Size = new System.Drawing.Size(145, 23);
            this.cmbExchangeGroup.TabIndex = 3;
            this.cmbExchangeGroup.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);
            // 
            // lblExchangeGroup
            // 
            appearance35.FontData.SizeInPoints = 9F;
            appearance35.TextHAlignAsString = "Left";
            appearance35.TextVAlignAsString = "Middle";
            this.lblExchangeGroup.Appearance = appearance35;
            this.lblExchangeGroup.AutoSize = true;
            this.lblExchangeGroup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExchangeGroup.Location = new System.Drawing.Point(219, 35);
            this.lblExchangeGroup.Name = "lblExchangeGroup";
            this.lblExchangeGroup.Size = new System.Drawing.Size(94, 18);
            this.lblExchangeGroup.TabIndex = 2;
            this.lblExchangeGroup.Text = "Exchange Group";
            // 
            // lblSelectLiveFeedDate
            // 
            appearance36.FontData.SizeInPoints = 9F;
            appearance36.TextHAlignAsString = "Left";
            appearance36.TextVAlignAsString = "Middle";
            this.lblSelectLiveFeedDate.Appearance = appearance36;
            this.lblSelectLiveFeedDate.AutoSize = true;
            this.lblSelectLiveFeedDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectLiveFeedDate.Location = new System.Drawing.Point(10, 35);
            this.lblSelectLiveFeedDate.Name = "lblSelectLiveFeedDate";
            this.lblSelectLiveFeedDate.Size = new System.Drawing.Size(66, 18);
            this.lblSelectLiveFeedDate.TabIndex = 0;
            this.lblSelectLiveFeedDate.Text = "Select Date";
            // 
            // grpBoxLiveFeedWhole
            // 
            this.grpBoxLiveFeedWhole.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpBoxLiveFeedWhole.Controls.Add(this.optIMidPrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.optMidPrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.optSelectedFeedPrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblLiveFeedDatePrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.btnGetLiveFeedData);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblUpdatePrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.grpBoxSymbolUpdationChoice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.optLastPrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.optPreviousPrice);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblLastDate);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblPreviousDate);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblIMidDate);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblMidDate);
            this.grpBoxLiveFeedWhole.Controls.Add(this.lblSelectedFeedDate);
            this.grpBoxLiveFeedWhole.Location = new System.Drawing.Point(6, 78);
            this.grpBoxLiveFeedWhole.Name = "grpBoxLiveFeedWhole";
            this.grpBoxLiveFeedWhole.Size = new System.Drawing.Size(748, 86);
            this.grpBoxLiveFeedWhole.TabIndex = 5;
            // 
            // optIMidPrice
            // 
            this.optIMidPrice.AutoSize = true;
            this.optIMidPrice.Location = new System.Drawing.Point(435, 9);
            this.optIMidPrice.Name = "optIMidPrice";
            this.optIMidPrice.Size = new System.Drawing.Size(78, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optIMidPrice, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optIMidPrice.TabIndex = 8;
            this.optIMidPrice.TabStop = true;
            this.optIMidPrice.Text = "iMid Price";
            this.optIMidPrice.UseVisualStyleBackColor = true;
            // 
            // optMidPrice
            // 
            this.optMidPrice.AutoSize = true;
            this.optMidPrice.Location = new System.Drawing.Point(594, 9);
            this.optMidPrice.Name = "optMidPrice";
            this.optMidPrice.Size = new System.Drawing.Size(75, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optMidPrice, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optMidPrice.TabIndex = 11;
            this.optMidPrice.TabStop = true;
            this.optMidPrice.Text = "Mid Price";
            this.optMidPrice.UseVisualStyleBackColor = true;
            this.optMidPrice.CheckedChanged += new System.EventHandler(this.optMidPrice_CheckedChanged);
            // 
            // optSelectedFeedPrice
            // 
            this.optSelectedFeedPrice.AutoSize = true;
            this.optSelectedFeedPrice.Location = new System.Drawing.Point(94, 31);
            this.optSelectedFeedPrice.Name = "optSelectedFeedPrice";
            this.optSelectedFeedPrice.Size = new System.Drawing.Size(126, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optSelectedFeedPrice, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optSelectedFeedPrice.TabIndex = 12;
            this.optSelectedFeedPrice.TabStop = true;
            this.optSelectedFeedPrice.Text = "Selected Feed Price";
            this.optSelectedFeedPrice.UseVisualStyleBackColor = true;
            this.optSelectedFeedPrice.CheckedChanged += new System.EventHandler(this.optSelectedFeedPrice_CheckedChanged);
            // 
            // lblLiveFeedDatePrice
            // 
            this.lblLiveFeedDatePrice.AutoSize = true;
            this.lblLiveFeedDatePrice.Location = new System.Drawing.Point(524, 58);
            this.lblLiveFeedDatePrice.Name = "lblLiveFeedDatePrice";
            this.lblLiveFeedDatePrice.Size = new System.Drawing.Size(104, 18);
            this.lblLiveFeedDatePrice.TabIndex = 7;
            this.lblLiveFeedDatePrice.Text = "LiveFeedDatePrice";
            // 
            // lblUpdatePrice
            // 
            this.lblUpdatePrice.AutoSize = true;
            this.lblUpdatePrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdatePrice.Location = new System.Drawing.Point(2, 11);
            this.lblUpdatePrice.Name = "lblUpdatePrice";
            this.lblUpdatePrice.Size = new System.Drawing.Size(77, 18);
            this.lblUpdatePrice.TabIndex = 0;
            this.lblUpdatePrice.Text = "Update Price:";
            // 
            // grpBoxSymbolUpdationChoice
            // 
            appearance37.FontData.SizeInPoints = 9F;
            this.grpBoxSymbolUpdationChoice.Appearance = appearance37;
            this.grpBoxSymbolUpdationChoice.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpBoxSymbolUpdationChoice.Controls.Add(this.ultraLabel1);
            this.grpBoxSymbolUpdationChoice.Controls.Add(this.optOverwriteAllZeroSymbols);
            this.grpBoxSymbolUpdationChoice.Controls.Add(this.optOverwriteAllSymbols);
            this.grpBoxSymbolUpdationChoice.Controls.Add(this.grpBoxImportExport);
            this.grpBoxSymbolUpdationChoice.ForeColor = System.Drawing.Color.Black;
            this.grpBoxSymbolUpdationChoice.Location = new System.Drawing.Point(7, 50);
            this.grpBoxSymbolUpdationChoice.Name = "grpBoxSymbolUpdationChoice";
            this.grpBoxSymbolUpdationChoice.Size = new System.Drawing.Size(398, 27);
            this.grpBoxSymbolUpdationChoice.TabIndex = 5;
            this.grpBoxSymbolUpdationChoice.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
            // 
            // ultraLabel1
            // 
            appearance38.FontData.SizeInPoints = 9F;
            appearance38.TextHAlignAsString = "Left";
            appearance38.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance38;
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(-1, 6);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(98, 20);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "Overwrite Data :";
            // 
            // optOverwriteAllZeroSymbols
            // 
            this.optOverwriteAllZeroSymbols.AutoSize = true;
            this.optOverwriteAllZeroSymbols.Location = new System.Drawing.Point(206, 6);
            this.optOverwriteAllZeroSymbols.Name = "optOverwriteAllZeroSymbols";
            this.optOverwriteAllZeroSymbols.Size = new System.Drawing.Size(150, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optOverwriteAllZeroSymbols, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optOverwriteAllZeroSymbols.TabIndex = 3;
            this.optOverwriteAllZeroSymbols.TabStop = true;
            this.optOverwriteAllZeroSymbols.Text = "Symbols with zero price";
            this.optOverwriteAllZeroSymbols.UseVisualStyleBackColor = true;
            // 
            // optOverwriteAllSymbols
            // 
            this.optOverwriteAllSymbols.AutoSize = true;
            this.optOverwriteAllSymbols.Location = new System.Drawing.Point(100, 6);
            this.optOverwriteAllSymbols.Name = "optOverwriteAllSymbols";
            this.optOverwriteAllSymbols.Size = new System.Drawing.Size(86, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optOverwriteAllSymbols, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optOverwriteAllSymbols.TabIndex = 2;
            this.optOverwriteAllSymbols.TabStop = true;
            this.optOverwriteAllSymbols.Text = "All symbols";
            this.optOverwriteAllSymbols.UseVisualStyleBackColor = true;
            this.optOverwriteAllSymbols.CheckedChanged += new System.EventHandler(this.optOverwriteAllSymbols_CheckedChanged);
            // 
            // optLastPrice
            // 
            this.optLastPrice.AutoSize = true;
            this.optLastPrice.Location = new System.Drawing.Point(94, 9);
            this.optLastPrice.Name = "optLastPrice";
            this.optLastPrice.Size = new System.Drawing.Size(75, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optLastPrice, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optLastPrice.TabIndex = 1;
            this.optLastPrice.TabStop = true;
            this.optLastPrice.Text = "Last Price";
            this.optLastPrice.UseVisualStyleBackColor = true;
            this.optLastPrice.CheckedChanged += new System.EventHandler(this.optLastPrice_CheckedChanged);
            // 
            // optPreviousPrice
            // 
            this.optPreviousPrice.AutoSize = true;
            this.optPreviousPrice.Location = new System.Drawing.Point(250, 9);
            this.optPreviousPrice.Name = "optPreviousPrice";
            this.optPreviousPrice.Size = new System.Drawing.Size(95, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optPreviousPrice, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optPreviousPrice.TabIndex = 3;
            this.optPreviousPrice.TabStop = true;
            this.optPreviousPrice.Text = "Closing Mark";
            this.optPreviousPrice.UseVisualStyleBackColor = true;
            this.optPreviousPrice.CheckedChanged += new System.EventHandler(this.optPreviousPrice_CheckedChanged);
            // 
            // lblLastDate
            // 
            this.lblLastDate.AutoSize = true;
            this.lblLastDate.Location = new System.Drawing.Point(168, 11);
            this.lblLastDate.Name = "lblLastDate";
            this.lblLastDate.Size = new System.Drawing.Size(52, 18);
            this.lblLastDate.TabIndex = 2;
            this.lblLastDate.Text = "LastDate";
            // 
            // lblPreviousDate
            // 
            this.lblPreviousDate.AutoSize = true;
            this.lblPreviousDate.Location = new System.Drawing.Point(349, 11);
            this.lblPreviousDate.Name = "lblPreviousDate";
            this.lblPreviousDate.Size = new System.Drawing.Size(76, 18);
            this.lblPreviousDate.TabIndex = 4;
            this.lblPreviousDate.Text = "PreviousDate";
            // 
            // lblIMidDate
            // 
            this.lblIMidDate.AutoSize = true;
            this.lblIMidDate.Location = new System.Drawing.Point(515, 11);
            this.lblIMidDate.Name = "lblIMidDate";
            this.lblIMidDate.Size = new System.Drawing.Size(54, 18);
            this.lblIMidDate.TabIndex = 9;
            this.lblIMidDate.Text = "iMidDate";
            // 
            // lblMidDate
            // 
            this.lblMidDate.AutoSize = true;
            this.lblMidDate.Location = new System.Drawing.Point(674, 11);
            this.lblMidDate.Name = "lblMidDate";
            this.lblMidDate.Size = new System.Drawing.Size(51, 18);
            this.lblMidDate.TabIndex = 9;
            this.lblMidDate.Text = "MidDate";
            // 
            // lblSelectedFeedDate
            // 
            this.lblSelectedFeedDate.Location = new System.Drawing.Point(223, 33);
            this.lblSelectedFeedDate.Name = "lblSelectedFeedDate";
            this.lblSelectedFeedDate.Size = new System.Drawing.Size(136, 18);
            this.lblSelectedFeedDate.TabIndex = 9;
            this.lblSelectedFeedDate.Text = "SelectedFeedDate";
            // 
            // dtLiveFeed
            // 
            appearance39.FontData.SizeInPoints = 9F;
            this.dtLiveFeed.Appearance = appearance39;
            this.dtLiveFeed.AutoSize = false;
            this.dtLiveFeed.Location = new System.Drawing.Point(102, 33);
            this.dtLiveFeed.Name = "dtLiveFeed";
            this.dtLiveFeed.Size = new System.Drawing.Size(109, 23);
            this.dtLiveFeed.TabIndex = 1;
            this.dtLiveFeed.ValueChanged += new System.EventHandler(this.dtLiveFeed_ValueChanged);
            this.dtLiveFeed.AfterCloseUp += new System.EventHandler(this.dtLiveFeed_AfterCloseUp);
            this.dtLiveFeed.Leave += new System.EventHandler(this.dtLiveFeed_Leave);
            // 
            // optOMIPrice
            // 
            this.optOMIPrice.AutoSize = true;
            this.optOMIPrice.Location = new System.Drawing.Point(141, 59);
            this.optOMIPrice.Name = "optOMIPrice";
            this.optOMIPrice.Size = new System.Drawing.Size(66, 19);
            this.inboxControlStyler1.SetStyleSettings(this.optOMIPrice, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.optOMIPrice.TabIndex = 9;
            this.optOMIPrice.TabStop = true;
            this.optOMIPrice.Text = "From PI";
            this.optOMIPrice.UseVisualStyleBackColor = true;
            this.optOMIPrice.CheckedChanged += new System.EventHandler(this.optOMIPrice_CheckedChanged);
            // 
            // lblSelectDateView
            // 
            appearance18.FontData.SizeInPoints = 9F;
            this.lblSelectDateView.Appearance = appearance18;
            this.lblSelectDateView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectDateView.Location = new System.Drawing.Point(6, 60);
            this.lblSelectDateView.Name = "lblSelectDateView";
            this.lblSelectDateView.Size = new System.Drawing.Size(87, 20);
            this.lblSelectDateView.TabIndex = 3;
            this.lblSelectDateView.Text = "Select Date";
            // 
            // btnGetFilteredData
            // 
            this.btnGetFilteredData.Location = new System.Drawing.Point(107, 148);
            this.btnGetFilteredData.Name = "btnGetFilteredData";
            this.btnGetFilteredData.Size = new System.Drawing.Size(51, 23);
            this.btnGetFilteredData.TabIndex = 6;
            this.btnGetFilteredData.Text = "Filter";
            this.btnGetFilteredData.Click += new System.EventHandler(this.btnGetFilteredData_Click);
            // 
            // lblFilteredSymbol
            // 
            appearance15.TextHAlignAsString = "Left";
            appearance15.TextVAlignAsString = "Middle";
            this.lblFilteredSymbol.Appearance = appearance15;
            this.lblFilteredSymbol.Location = new System.Drawing.Point(6, 122);
            this.lblFilteredSymbol.Name = "lblFilteredSymbol";
            this.lblFilteredSymbol.Size = new System.Drawing.Size(100, 19);
            this.lblFilteredSymbol.TabIndex = 4;
            this.lblFilteredSymbol.Text = "Enter Symbol";
            // 
            // txtSymbolFilteration
            // 
            this.txtSymbolFilteration.AcceptsReturn = true;
            this.txtSymbolFilteration.Location = new System.Drawing.Point(107, 120);
            this.txtSymbolFilteration.MaxLength = 25;
            this.txtSymbolFilteration.Multiline = true;
            this.txtSymbolFilteration.Name = "txtSymbolFilteration";
            this.txtSymbolFilteration.Size = new System.Drawing.Size(189, 23);
            this.txtSymbolFilteration.TabIndex = 5;
            this.txtSymbolFilteration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSymbolFilteration_KeyPress);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(164, 149);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(74, 23);
            this.btnClearFilter.TabIndex = 7;
            this.btnClearFilter.Text = "Clear Filter";
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // grpBoxFilter
            // 
            this.grpBoxFilter.Location = new System.Drawing.Point(731, 30);
            this.grpBoxFilter.Name = "grpBoxFilter";
            this.grpBoxFilter.Size = new System.Drawing.Size(39, 14);
            this.grpBoxFilter.TabIndex = 32;
            this.grpBoxFilter.Text = "Filter Data";
            // 
            // lblCopyFromDate
            // 
            this.lblCopyFromDate.Location = new System.Drawing.Point(6, 89);
            this.lblCopyFromDate.Name = "lblCopyFromDate";
            this.lblCopyFromDate.Size = new System.Drawing.Size(87, 19);
            this.lblCopyFromDate.TabIndex = 8;
            this.lblCopyFromDate.Text = "Copy From";
            // 
            // dtCopyFromDate
            // 
            appearance17.FontData.SizeInPoints = 9F;
            this.dtCopyFromDate.Appearance = appearance17;
            this.dtCopyFromDate.AutoSize = false;
            this.dtCopyFromDate.Location = new System.Drawing.Point(107, 87);
            this.dtCopyFromDate.Name = "dtCopyFromDate";
            this.dtCopyFromDate.Size = new System.Drawing.Size(131, 23);
            this.dtCopyFromDate.TabIndex = 9;
            // 
            // btnFetchData
            // 
            appearance16.FontData.SizeInPoints = 9F;
            this.btnFetchData.Appearance = appearance16;
            this.btnFetchData.Location = new System.Drawing.Point(247, 87);
            this.btnFetchData.Name = "btnFetchData";
            this.btnFetchData.Size = new System.Drawing.Size(49, 24);
            this.btnFetchData.TabIndex = 10;
            this.btnFetchData.Text = "Get";
            this.btnFetchData.Click += new System.EventHandler(this.btnFetchData_Click);
            // 
            // lblCopyFrom
            // 
            appearance14.FontData.SizeInPoints = 9F;
            this.lblCopyFrom.Appearance = appearance14;
            this.lblCopyFrom.AutoSize = true;
            this.lblCopyFrom.Location = new System.Drawing.Point(8, 5);
            this.lblCopyFrom.Name = "lblCopyFrom";
            this.lblCopyFrom.Size = new System.Drawing.Size(62, 18);
            this.lblCopyFrom.TabIndex = 0;
            this.lblCopyFrom.Text = "Copy from";
            // 
            // cmbCopyFromAccount
            // 
            this.cmbCopyFromAccount.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbCopyFromAccount.Location = new System.Drawing.Point(100, 2);
            this.cmbCopyFromAccount.Name = "cmbCopyFromAccount";
            this.cmbCopyFromAccount.NullText = "Select Account";
            appearance13.ForeColor = System.Drawing.Color.Gray;
            this.cmbCopyFromAccount.NullTextAppearance = appearance13;
            this.cmbCopyFromAccount.Size = new System.Drawing.Size(189, 25);
            this.cmbCopyFromAccount.TabIndex = 1;
            // 
            // lblCopyTo
            // 
            appearance12.FontData.SizeInPoints = 9F;
            this.lblCopyTo.Appearance = appearance12;
            this.lblCopyTo.AutoSize = true;
            this.lblCopyTo.Location = new System.Drawing.Point(8, 31);
            this.lblCopyTo.Name = "lblCopyTo";
            this.lblCopyTo.Size = new System.Drawing.Size(47, 18);
            this.lblCopyTo.TabIndex = 2;
            this.lblCopyTo.Text = "Copy to";
            // 
            // multiSelectDropDown1
            // 
            this.multiSelectDropDown1.Location = new System.Drawing.Point(100, 31);
            this.multiSelectDropDown1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.multiSelectDropDown1.Name = "multiSelectDropDown1";
            this.multiSelectDropDown1.Size = new System.Drawing.Size(189, 23);
            this.multiSelectDropDown1.TabIndex = 33;
            this.multiSelectDropDown1.TitleText = "Account";
            // 
            // btnAccountCopy
            // 
            this.btnAccountCopy.Location = new System.Drawing.Point(100, 60);
            this.btnAccountCopy.Name = "btnAccountCopy";
            this.btnAccountCopy.Size = new System.Drawing.Size(51, 23);
            this.btnAccountCopy.TabIndex = 34;
            this.btnAccountCopy.Text = "Copy";
            this.btnAccountCopy.Click += new System.EventHandler(this.btnAccountCopy_Click);
            // 
            // pnlAccountCopy
            // 
            // 
            // pnlAccountCopy.ClientArea
            // 
            this.pnlAccountCopy.ClientArea.Controls.Add(this.btnAccountCopy);
            this.pnlAccountCopy.ClientArea.Controls.Add(this.multiSelectDropDown1);
            this.pnlAccountCopy.ClientArea.Controls.Add(this.lblCopyTo);
            this.pnlAccountCopy.ClientArea.Controls.Add(this.cmbCopyFromAccount);
            this.pnlAccountCopy.ClientArea.Controls.Add(this.lblCopyFrom);
            this.pnlAccountCopy.Location = new System.Drawing.Point(7, 176);
            this.pnlAccountCopy.Name = "pnlAccountCopy";
            this.pnlAccountCopy.Size = new System.Drawing.Size(296, 86);
            this.pnlAccountCopy.TabIndex = 13;
            this.pnlAccountCopy.Visible = false;
            // 
            // CtrlMarkPriceAndForexConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 12F);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "CtrlMarkPriceAndForexConversion";
            this.Size = new System.Drawing.Size(1094, 605);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.CtrlMarkPriceAndForexConversion_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CtrlMarkPriceAndForexConversion_KeyUp);
            this.Disposed += new System.EventHandler(this.CtrlMarkPriceAndForexConversion_Disposed);
            ((System.ComponentModel.ISupportInitialize)(this.grdPivotDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDateMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxGrid)).EndInit();
            this.grpBoxGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpSelectDateMethodology)).EndInit();
            this.grpSelectDateMethodology.ResumeLayout(false);
            this.grpSelectDateMethodology.PerformLayout();
            this.ultrapanelTop.ClientArea.ResumeLayout(false);
            this.ultrapanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxImportExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxLiveFeedHandling)).EndInit();
            this.grpBoxLiveFeedHandling.ResumeLayout(false);
            this.grpBoxLiveFeedHandling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExchangeGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxLiveFeedWhole)).EndInit();
            this.grpBoxLiveFeedWhole.ResumeLayout(false);
            this.grpBoxLiveFeedWhole.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxSymbolUpdationChoice)).EndInit();
            this.grpBoxSymbolUpdationChoice.ResumeLayout(false);
            this.grpBoxSymbolUpdationChoice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtLiveFeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymbolFilteration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtCopyFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCopyFromAccount)).EndInit();
            this.pnlAccountCopy.ClientArea.ResumeLayout(false);
            this.pnlAccountCopy.ClientArea.PerformLayout();
            this.pnlAccountCopy.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpBoxImportExport;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.Misc.UltraButton btnImport;
        private Infragistics.Win.Misc.UltraButton btnGetLiveFeedData;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtLiveFeed;
        protected System.Windows.Forms.RadioButton optPreviousPrice;
        private Infragistics.Win.Misc.UltraLabel lblUpdatePrice;
        private Infragistics.Win.Misc.UltraLabel lblSelectDateView;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxSymbolUpdationChoice;
        protected System.Windows.Forms.RadioButton optOverwriteAllSymbols;
        private System.Windows.Forms.RadioButton optOverwriteAllZeroSymbols;
        protected System.Windows.Forms.RadioButton optLastPrice;
        private Infragistics.Win.Misc.UltraButton btnGetFilteredData;
        private Infragistics.Win.Misc.UltraLabel lblFilteredSymbol;
        internal Infragistics.Win.UltraWinEditors.UltraTextEditor txtSymbolFilteration;
        private Infragistics.Win.Misc.UltraButton btnClearFilter;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblSelectLiveFeedDate;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxFilter;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxLiveFeedWhole;
        private Infragistics.Win.Misc.UltraLabel lblExchangeGroup;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbExchangeGroup;
        private System.Windows.Forms.RadioButton optUseLiveFeed;
        private System.Windows.Forms.RadioButton optUseImportExport;
        private Infragistics.Win.Misc.UltraLabel lblLiveFeedDatePrice;
        private Infragistics.Win.Misc.UltraLabel lblPreviousDate;
        private Infragistics.Win.Misc.UltraLabel lblLastDate;
        private Infragistics.Win.Misc.UltraLabel lblIMidDate;
        private Infragistics.Win.Misc.UltraLabel lblMidDate;
        private Infragistics.Win.Misc.UltraLabel lblSelectedFeedDate;
        protected System.Windows.Forms.RadioButton optIMidPrice;
        protected System.Windows.Forms.RadioButton optMidPrice;
        private Infragistics.Win.Misc.UltraLabel lblCopyFromDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtCopyFromDate;
        private Infragistics.Win.Misc.UltraButton btnFetchData;
        protected System.Windows.Forms.RadioButton optOMIPrice;
        protected System.Windows.Forms.RadioButton optSelectedFeedPrice;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel pnlAccountCopy;
        private Infragistics.Win.Misc.UltraButton btnAccountCopy;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDown1;
        private Infragistics.Win.Misc.UltraLabel lblCopyTo;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCopyFromAccount;
        private Infragistics.Win.Misc.UltraLabel lblCopyFrom;
    }
}
