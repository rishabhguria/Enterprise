using Prana.BusinessObjects.Constants;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using System.Threading;
using Prana.Global;
using Prana.Utilities.UI.UIUtilities;
using System;
using Prana.LogManager;
namespace Prana.Tools
{
    partial class OptionModelInputs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        //Modified By Kashish Goyal               
        //Date: 12 June 2015
        //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-8520

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (headerCheckBox != null)
                {
                    headerCheckBox.Dispose();
                }
                if (_pricingServiceProxy != null)
                {
                    _pricingServiceProxy.Dispose();
                }

                if (_riskServiceProxy != null)
                {
                    _riskServiceProxy.Dispose();
                }

                if (_positionManagementServices != null)
                {
                    _positionManagementServices.Dispose();
                }

                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.Dispose();
                }

                if (_proxyPricing != null)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(
                    (obj) =>
                    {
                        try
                        {
                            _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_OMIData);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            if (rethrow)
                            {
                                throw;
                            }
                        }
                    }));
                    _proxyPricing.Dispose();
                }

                if (dtOMI != null)
                {
                    dtOMI.Dispose();
                }

                if (bgGetHistoricalvol != null)
                {
                    bgGetHistoricalvol.Dispose();
                }

                if (_bgSaveData != null)
                {
                    _bgSaveData.Dispose();
                }

                if (historicalVolInputsUI != null)
                {
                    historicalVolInputsUI.Dispose();
                }
                RiskPreferenceManager.CleanUp();
                PricingPreferenceManager.Dispose();
                PricingLayoutManager.Dispose();
                UnwireEvents();
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance116 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance117 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance118 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance119 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance120 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance121 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance122 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance123 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance124 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance125 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance126 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance127 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance128 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance129 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance130 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance131 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance132 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance133 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance134 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance135 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance136 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance137 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance138 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance139 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance140 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance145 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance146 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance147 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance148 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance149 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance150 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance151 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance152 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance153 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance154 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance155 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance156 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance157 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance158 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance159 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance160 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance161 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance162 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.upnlBody = new Infragistics.Win.Misc.UltraPanel();
            this.grdOptionModel = new PranaUltraGrid();
            this.contextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSaveLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClearFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeleteSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripOMI = new System.Windows.Forms.StatusStrip();
            this.toolStripLabelOMI = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.upnlButtons = new Infragistics.Win.Misc.UltraPanel();
            this.btnHistoricalVolInputs = new Infragistics.Win.Misc.UltraButton();
            this.btnGetHistoricalVol = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnRefreshLiveData = new Infragistics.Win.Misc.UltraButton();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.btnSymbolLookup = new Infragistics.Win.Misc.UltraButton();
            this.grpBoxShowCols = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkBoxClosingMark = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxManualInput = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxSharesOutstanding = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxAll = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxLastPrice = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxDelta = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxDividend = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxIR = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxVolatility = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxTheoreticalPrice = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ugbxSymbols = new Infragistics.Win.Misc.UltraGroupBox();
            this.uOSetSymbols = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.grpBoxSymbols = new Infragistics.Win.Misc.UltraGroupBox();
            this.radioNonZeroPositions = new System.Windows.Forms.RadioButton();
            this.radioOptionUnder = new System.Windows.Forms.RadioButton();
            this.radioOptions = new System.Windows.Forms.RadioButton();
            this.radioAllSymbols = new System.Windows.Forms.RadioButton();
            this.lblSymbols = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.GrpBoxPricingDataSource = new Infragistics.Win.Misc.UltraGroupBox();
            this.rBtnClosingMark = new System.Windows.Forms.RadioButton();
            this.rBtnLiveData = new System.Windows.Forms.RadioButton();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnSavePreferences = new Infragistics.Win.Misc.UltraButton();
            this.grpBoxOtherAssetsFeedPrice = new Infragistics.Win.Misc.UltraGroupBox();
            this.isLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ifLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbBxOverrideCheckOthers = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbBxOverrideConditionOthers = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.priceBarOthers = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.cmbBxOverrideWithOthers = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbStockPrice = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.label6 = new Infragistics.Win.Misc.UltraLabel();
            this.ugbxPricingSource = new Infragistics.Win.Misc.UltraGroupBox();
            this.uOsetPricingSource = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.grpBoxUseDefaultDelta = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkBoxUseDefaultDelta = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.grpBoxOptionSelectedFeedPrice = new Infragistics.Win.Misc.UltraGroupBox();
            this.isLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ifLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbBxOverrideCheckOptions = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbBxOverrideConditionOptions = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.priceBarOptions = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.cmbBxOverrideWithOptions = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbOptPrice = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraTabPageControl1.SuspendLayout();
            this.upnlBody.ClientArea.SuspendLayout();
            this.upnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOptionModel)).BeginInit();
            this.contextmenu.SuspendLayout();
            this.statusStripOMI.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.upnlButtons.ClientArea.SuspendLayout();
            this.upnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxShowCols)).BeginInit();
            this.grpBoxShowCols.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxClosingMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxManualInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxSharesOutstanding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxLastPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxDividend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxVolatility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxTheoreticalPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxSymbols)).BeginInit();
            this.ugbxSymbols.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uOSetSymbols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxSymbols)).BeginInit();
            this.grpBoxSymbols.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpBoxPricingDataSource)).BeginInit();
            this.GrpBoxPricingDataSource.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxOtherAssetsFeedPrice)).BeginInit();
            this.grpBoxOtherAssetsFeedPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideCheckOthers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideConditionOthers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceBarOthers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideWithOthers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStockPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxPricingSource)).BeginInit();
            this.ugbxPricingSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uOsetPricingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxUseDefaultDelta)).BeginInit();
            this.grpBoxUseDefaultDelta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxUseDefaultDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxOptionSelectedFeedPrice)).BeginInit();
            this.grpBoxOptionSelectedFeedPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideCheckOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideConditionOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceBarOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideWithOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.upnlBody);
            this.ultraTabPageControl1.Controls.Add(this.ultraPanel2);
            this.ultraTabPageControl1.Controls.Add(this.grpBoxSymbols);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(909, 529);
            // 
            // upnlBody
            // 
            // 
            // upnlBody.ClientArea
            // 
            this.upnlBody.ClientArea.Controls.Add(this.grdOptionModel);
            this.upnlBody.ClientArea.Controls.Add(this.statusStripOMI);
            this.upnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upnlBody.Location = new System.Drawing.Point(0, 138);
            this.upnlBody.Name = "upnlBody";
            this.upnlBody.Size = new System.Drawing.Size(909, 391);
            this.upnlBody.TabIndex = 10;
            // 
            // grdOptionModel
            // 
            this.grdOptionModel.ContextMenuStrip = this.contextmenu;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.grdOptionModel.DisplayLayout.Appearance = appearance1;
            this.grdOptionModel.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdOptionModel.DisplayLayout.CaptionAppearance = appearance2;
            this.grdOptionModel.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.grdOptionModel.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdOptionModel.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.grdOptionModel.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdOptionModel.DisplayLayout.GroupByBox.Hidden = true;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdOptionModel.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.grdOptionModel.DisplayLayout.MaxColScrollRegions = 1;
            this.grdOptionModel.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.grdOptionModel.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.LightSlateGray;
            appearance7.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance7.BorderColor = System.Drawing.Color.DimGray;
            appearance7.ForeColor = System.Drawing.Color.White;
            this.grdOptionModel.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.grdOptionModel.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdOptionModel.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            this.grdOptionModel.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.grdOptionModel.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.TextHAlignAsString = "Right";
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdOptionModel.DisplayLayout.Override.CellAppearance = appearance9;
            this.grdOptionModel.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdOptionModel.DisplayLayout.Override.CellPadding = 0;
            appearance10.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance10.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.grdOptionModel.DisplayLayout.Override.FixedHeaderAppearance = appearance10;
            appearance11.BackColor = System.Drawing.SystemColors.Control;
            appearance11.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance11.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance11.BorderColor = System.Drawing.SystemColors.Window;
            this.grdOptionModel.DisplayLayout.Override.GroupByRowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance12.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance12.TextHAlignAsString = "Center";
            this.grdOptionModel.DisplayLayout.Override.HeaderAppearance = appearance12;
            this.grdOptionModel.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdOptionModel.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdOptionModel.DisplayLayout.Override.InvalidValueBehavior = Infragistics.Win.UltraWinGrid.InvalidValueBehavior.RevertValue;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.grdOptionModel.DisplayLayout.Override.RowAlternateAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.Black;
            appearance14.BorderColor = System.Drawing.Color.Black;
            appearance14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdOptionModel.DisplayLayout.Override.RowAppearance = appearance14;
            this.grdOptionModel.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdOptionModel.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdOptionModel.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdOptionModel.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance15.BackColor = System.Drawing.Color.Transparent;
            appearance15.BackColor2 = System.Drawing.Color.Transparent;
            appearance15.BorderColor = System.Drawing.Color.Transparent;
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdOptionModel.DisplayLayout.Override.SelectedRowAppearance = appearance15;
            this.grdOptionModel.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOptionModel.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOptionModel.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdOptionModel.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOptionModel.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdOptionModel.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.grdOptionModel.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdOptionModel.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdOptionModel.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdOptionModel.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdOptionModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOptionModel.Location = new System.Drawing.Point(0, 0);
            this.grdOptionModel.Name = "grdOptionModel";
            this.grdOptionModel.Size = new System.Drawing.Size(909, 369);
            this.grdOptionModel.TabIndex = 1;
            this.grdOptionModel.Text = "ultraGrid1";
            this.grdOptionModel.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdOptionModel_AfterCellUpdate);
            this.grdOptionModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdOptionModel_InitializeLayout);
            this.grdOptionModel.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdOptionModel_CellChange);
            this.grdOptionModel.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.grdOptionModel_AfterColPosChanged);
            this.grdOptionModel.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdOptionModel_BeforeCustomRowFilterDialog);
            this.grdOptionModel.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdOptionModel_BeforeColumnChooserDisplayed);
            this.grdOptionModel.AfterRowFilterChanged +=grdOptionModel_AfterRowFilterChanged;
            // 
            // contextmenu
            // 
            this.contextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveLayout,
            this.menuClearFilters,
            this.menuDeleteSymbol});
            this.contextmenu.Name = "contextmenu";
            this.contextmenu.Size = new System.Drawing.Size(149, 70);
            this.inboxControlStyler1.SetStyleSettings(this.contextmenu, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.contextmenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextmenu_Opening);
            // 
            // menuSaveLayout
            // 
            this.menuSaveLayout.Name = "menuSaveLayout";
            this.menuSaveLayout.Size = new System.Drawing.Size(148, 22);
            this.menuSaveLayout.Text = "Save Layout";
            this.menuSaveLayout.Click += new System.EventHandler(this.menuSaveLayout_Click);
            // 
            // menuClearFilters
            // 
            this.menuClearFilters.Name = "menuClearFilters";
            this.menuClearFilters.Size = new System.Drawing.Size(148, 22);
            this.menuClearFilters.Text = "Clear Filters";
            this.menuClearFilters.Click += new System.EventHandler(this.menuClearFilters_Click);
            // 
            // menuDeleteSymbol
            // 
            this.menuDeleteSymbol.Enabled = false;
            this.menuDeleteSymbol.Name = "menuDeleteSymbol";
            this.menuDeleteSymbol.Size = new System.Drawing.Size(148, 22);
            this.menuDeleteSymbol.Text = "Delete";
            this.menuDeleteSymbol.Click += new System.EventHandler(this.menuDeleteSymbol_Click);
            // 
            // statusStripOMI
            // 
            this.statusStripOMI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelOMI});
            this.statusStripOMI.Location = new System.Drawing.Point(0, 369);
            this.statusStripOMI.Name = "statusStripOMI";
            this.statusStripOMI.Size = new System.Drawing.Size(909, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStripOMI, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStripOMI.TabIndex = 0;
            this.statusStripOMI.Text = "statusStrip1";
            // 
            // toolStripLabelOMI
            // 
            this.toolStripLabelOMI.Name = "toolStripLabelOMI";
            this.toolStripLabelOMI.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.upnlButtons);
            this.ultraPanel2.ClientArea.Controls.Add(this.grpBoxShowCols);
            this.ultraPanel2.ClientArea.Controls.Add(this.ugbxSymbols);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(909, 138);
            this.ultraPanel2.TabIndex = 9;
            // 
            // upnlButtons
            // 
            // 
            // upnlButtons.ClientArea
            // 
            this.upnlButtons.ClientArea.Controls.Add(this.btnHistoricalVolInputs);
            this.upnlButtons.ClientArea.Controls.Add(this.btnGetHistoricalVol);
            this.upnlButtons.ClientArea.Controls.Add(this.btnSave);
            this.upnlButtons.ClientArea.Controls.Add(this.btnRefreshLiveData);
            this.upnlButtons.ClientArea.Controls.Add(this.btnRefresh);
            this.upnlButtons.ClientArea.Controls.Add(this.btnExport);
            this.upnlButtons.ClientArea.Controls.Add(this.btnSymbolLookup);
            this.upnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upnlButtons.Location = new System.Drawing.Point(0, 98);
            this.upnlButtons.Name = "upnlButtons";
            this.upnlButtons.Size = new System.Drawing.Size(917, 38);
            this.upnlButtons.TabIndex = 10;
            // 
            // btnHistoricalVolInputs
            // 
            appearance17.FontData.SizeInPoints = 9F;
            this.btnHistoricalVolInputs.Appearance = appearance17;
            this.btnHistoricalVolInputs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistoricalVolInputs.Location = new System.Drawing.Point(618, 8);
            this.btnHistoricalVolInputs.Name = "btnHistoricalVolInputs";
            this.btnHistoricalVolInputs.Size = new System.Drawing.Size(167, 23);
            this.btnHistoricalVolInputs.TabIndex = 7;
            this.btnHistoricalVolInputs.Text = "Historical Vol Inputs";
            this.btnHistoricalVolInputs.Click += new System.EventHandler(this.btnHistoricalVolInputs_Click);
            // 
            // btnGetHistoricalVol
            // 
            appearance18.FontData.SizeInPoints = 9F;
            this.btnGetHistoricalVol.Appearance = appearance18;
            this.btnGetHistoricalVol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetHistoricalVol.Location = new System.Drawing.Point(466, 8);
            this.btnGetHistoricalVol.Name = "btnGetHistoricalVol";
            this.btnGetHistoricalVol.Size = new System.Drawing.Size(146, 23);
            this.btnGetHistoricalVol.TabIndex = 8;
            this.btnGetHistoricalVol.Text = "Get Historical Vol";
            this.btnGetHistoricalVol.Click += new System.EventHandler(this.btnGetHistoricalVol_Click);
            // 
            // btnSave
            // 
            appearance19.FontData.SizeInPoints = 9F;
            this.btnSave.Appearance = appearance19;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(265, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefreshLiveData
            // 
            appearance20.FontData.SizeInPoints = 9F;
            this.btnRefreshLiveData.Appearance = appearance20;
            this.btnRefreshLiveData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshLiveData.Location = new System.Drawing.Point(112, 8);
            this.btnRefreshLiveData.Name = "btnRefreshLiveData";
            this.btnRefreshLiveData.Size = new System.Drawing.Size(145, 23);
            this.btnRefreshLiveData.TabIndex = 3;
            this.btnRefreshLiveData.Text = "Refresh Live Data";
            this.btnRefreshLiveData.Click += new System.EventHandler(this.btnRefreshLiveData_Click);
            // 
            // btnRefresh
            // 
            appearance21.FontData.SizeInPoints = 9F;
            this.btnRefresh.Appearance = appearance21;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(9, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(95, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExport
            // 
            appearance22.FontData.SizeInPoints = 9F;
            this.btnExport.Appearance = appearance22;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(333, 8);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(125, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export To Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSymbolLookup
            // 
            appearance23.FontData.SizeInPoints = 9F;
            this.btnSymbolLookup.Appearance = appearance23;
            this.btnSymbolLookup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSymbolLookup.Location = new System.Drawing.Point(790, 8);
            this.btnSymbolLookup.Name = "btnSymbolLookup";
            this.btnSymbolLookup.Size = new System.Drawing.Size(126, 23);
            this.btnSymbolLookup.TabIndex = 8;
            this.btnSymbolLookup.Text = "Security Master";
            this.btnSymbolLookup.Click += new System.EventHandler(this.btnSymbolLookup_Click);
            // 
            // grpBoxShowCols
            // 
            appearance24.FontData.SizeInPoints = 9F;
            this.grpBoxShowCols.Appearance = appearance24;
            this.grpBoxShowCols.BackColorInternal = System.Drawing.Color.Transparent;
            this.grpBoxShowCols.Controls.Add(this.checkBoxClosingMark);
            this.grpBoxShowCols.Controls.Add(this.checkBoxManualInput);
            this.grpBoxShowCols.Controls.Add(this.checkBoxSharesOutstanding);
            this.grpBoxShowCols.Controls.Add(this.checkBoxAll);
            this.grpBoxShowCols.Controls.Add(this.checkBoxLastPrice);
            this.grpBoxShowCols.Controls.Add(this.checkBoxDelta);
            this.grpBoxShowCols.Controls.Add(this.checkBoxDividend);
            this.grpBoxShowCols.Controls.Add(this.checkBoxIR);
            this.grpBoxShowCols.Controls.Add(this.checkBoxVolatility);
            this.grpBoxShowCols.Controls.Add(this.checkBoxTheoreticalPrice);
            this.grpBoxShowCols.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBoxShowCols.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxShowCols.Location = new System.Drawing.Point(0, 46);
            this.grpBoxShowCols.Name = "grpBoxShowCols";
            this.grpBoxShowCols.Size = new System.Drawing.Size(909, 61);
            this.grpBoxShowCols.TabIndex = 5;
            this.grpBoxShowCols.Text = "Show Columns";
            // 
            // checkBoxClosingMark
            // 
            appearance25.FontData.SizeInPoints = 9F;
            this.checkBoxClosingMark.Appearance = appearance25;
            this.checkBoxClosingMark.AutoSize = true;
            this.checkBoxClosingMark.Location = new System.Drawing.Point(772, 25);
            this.checkBoxClosingMark.Name = "checkBoxClosingMark";
            this.checkBoxClosingMark.Size = new System.Drawing.Size(90, 21);
            this.checkBoxClosingMark.TabIndex = 13;
            this.checkBoxClosingMark.Text = "ClosingMark";
            this.checkBoxClosingMark.CheckedChanged += new System.EventHandler(this.checkBoxClosingMark_CheckedChanged);
            //
            // checkBoxManualInput
            // 
            appearance26.FontData.SizeInPoints = 9F;
            this.checkBoxManualInput.Appearance = appearance26;
            this.checkBoxManualInput.AutoSize = true;
            this.checkBoxManualInput.Location = new System.Drawing.Point(880, 25);
            this.checkBoxManualInput.Name = "checkBoxManualInput";
            this.checkBoxManualInput.Size = new System.Drawing.Size(94, 21);
            this.checkBoxManualInput.TabIndex = 14;
            this.checkBoxManualInput.Text = "Manual Input";
            this.checkBoxManualInput.CheckedChanged += new System.EventHandler(this.checkBoxManualInput_CheckedChanged);
            // 
            // checkBoxSharesOutstanding
            // 
            appearance27.FontData.SizeInPoints = 9F;
            this.checkBoxSharesOutstanding.Appearance = appearance27;
            this.checkBoxSharesOutstanding.AutoSize = true;
            this.checkBoxSharesOutstanding.Location = new System.Drawing.Point(625, 25);
            this.checkBoxSharesOutstanding.Name = "checkBoxSharesOutstanding";
            this.checkBoxSharesOutstanding.Size = new System.Drawing.Size(131, 21);
            this.checkBoxSharesOutstanding.TabIndex = 12;
            this.checkBoxSharesOutstanding.Text = "Shares Outstanding";
            this.checkBoxSharesOutstanding.CheckedChanged += new System.EventHandler(this.checkBoxSharesOutstanding_CheckedChanged);
            // 
            // checkBoxAll
            // 
            appearance28.FontData.SizeInPoints = 9F;
            this.checkBoxAll.Appearance = appearance28;
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(7, 25);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(35, 21);
            this.checkBoxAll.TabIndex = 11;
            this.checkBoxAll.Text = "All";
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // checkBoxLastPrice
            // 
            appearance29.FontData.SizeInPoints = 9F;
            this.checkBoxLastPrice.Appearance = appearance29;
            this.checkBoxLastPrice.AutoSize = true;
            this.checkBoxLastPrice.Location = new System.Drawing.Point(403, 25);
            this.checkBoxLastPrice.Name = "checkBoxLastPrice";
            this.checkBoxLastPrice.Size = new System.Drawing.Size(87, 21);
            this.checkBoxLastPrice.TabIndex = 10;
            this.checkBoxLastPrice.Text = "Selected Px";
            this.checkBoxLastPrice.CheckedChanged += new System.EventHandler(this.checkBoxLastPrice_CheckedChanged);
            // 
            // checkBoxDelta
            // 
            appearance30.FontData.SizeInPoints = 9F;
            this.checkBoxDelta.Appearance = appearance30;
            this.checkBoxDelta.AutoSize = true;
            this.checkBoxDelta.Location = new System.Drawing.Point(337, 25);
            this.checkBoxDelta.Name = "checkBoxDelta";
            this.checkBoxDelta.Size = new System.Drawing.Size(50, 21);
            this.checkBoxDelta.TabIndex = 9;
            this.checkBoxDelta.Text = "Delta";
            this.checkBoxDelta.CheckedChanged += new System.EventHandler(this.checkBoxDelta_CheckedChanged);
            // 
            // checkBoxDividend
            // 
            appearance31.FontData.SizeInPoints = 9F;
            this.checkBoxDividend.Appearance = appearance31;
            this.checkBoxDividend.AutoSize = true;
            this.checkBoxDividend.Location = new System.Drawing.Point(251, 25);
            this.checkBoxDividend.Name = "checkBoxDividend";
            this.checkBoxDividend.Size = new System.Drawing.Size(70, 21);
            this.checkBoxDividend.TabIndex = 8;
            this.checkBoxDividend.Text = "Dividend";
            this.checkBoxDividend.CheckedChanged += new System.EventHandler(this.checkBoxDividend_CheckedChanged);
            // 
            // checkBoxIR
            // 
            appearance32.FontData.SizeInPoints = 9F;
            this.checkBoxIR.Appearance = appearance32;
            this.checkBoxIR.AutoSize = true;
            this.checkBoxIR.Location = new System.Drawing.Point(143, 25);
            this.checkBoxIR.Name = "checkBoxIR";
            this.checkBoxIR.Size = new System.Drawing.Size(92, 21);
            this.checkBoxIR.TabIndex = 7;
            this.checkBoxIR.Text = "Interest Rate";
            this.checkBoxIR.CheckedChanged += new System.EventHandler(this.checkBoxIR_CheckedChanged);
            // 
            // checkBoxVolatility
            // 
            appearance33.FontData.SizeInPoints = 9F;
            this.checkBoxVolatility.Appearance = appearance33;
            this.checkBoxVolatility.AutoSize = true;
            this.checkBoxVolatility.Location = new System.Drawing.Point(58, 25);
            this.checkBoxVolatility.Name = "checkBoxVolatility";
            this.checkBoxVolatility.Size = new System.Drawing.Size(69, 21);
            this.checkBoxVolatility.TabIndex = 6;
            this.checkBoxVolatility.Text = "Volatility";
            this.checkBoxVolatility.CheckedChanged += new System.EventHandler(this.checkBoxVolatility_CheckedChanged);
            // 
            // checkBoxTheoreticalPrice
            // 
            appearance34.FontData.SizeInPoints = 9F;
            this.checkBoxTheoreticalPrice.Appearance = appearance34;
            this.checkBoxTheoreticalPrice.AutoSize = true;
            this.checkBoxTheoreticalPrice.Location = new System.Drawing.Point(495, 25);
            this.checkBoxTheoreticalPrice.Name = "checkBoxTheoreticalPrice";
            this.checkBoxTheoreticalPrice.Size = new System.Drawing.Size(114, 21);
            this.checkBoxTheoreticalPrice.TabIndex = 6;
            this.checkBoxTheoreticalPrice.Text = "Theoretical Price";
            this.checkBoxTheoreticalPrice.CheckedChanged += new System.EventHandler(this.checkBoxTheoreticalPrice_CheckedChanged);
            // 
            // ugbxSymbols
            // 
            appearance35.FontData.SizeInPoints = 9F;
            this.ugbxSymbols.Appearance = appearance35;
            this.ugbxSymbols.Controls.Add(this.uOSetSymbols);
            this.ugbxSymbols.Dock = System.Windows.Forms.DockStyle.Top;
            this.ugbxSymbols.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugbxSymbols.Location = new System.Drawing.Point(0, 0);
            this.ugbxSymbols.Name = "ugbxSymbols";
            this.ugbxSymbols.Size = new System.Drawing.Size(909, 46);
            this.ugbxSymbols.TabIndex = 1;
            this.ugbxSymbols.Text = "Symbols";
            // 
            // uOSetSymbols
            // 
            appearance36.FontData.SizeInPoints = 9F;
            this.uOSetSymbols.Appearance = appearance36;
            this.uOSetSymbols.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.uOSetSymbols.CheckedIndex = 0;
            valueListItem1.DataValue = "radioNonZeroPositions";
            valueListItem1.DisplayText = "All";
            valueListItem2.DataValue = "radioOptions";
            valueListItem2.DisplayText = "Options";
            valueListItem3.DataValue = "radioOptionUnder";
            valueListItem3.DisplayText = "Option and Underliers";
            valueListItem4.DataValue = "ValueListItem3";
            valueListItem4.DisplayText = "Non Zero Positions";
            this.uOSetSymbols.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4});
            this.uOSetSymbols.ItemSpacingHorizontal = 10;
            this.uOSetSymbols.Location = new System.Drawing.Point(20, 23);
            this.uOSetSymbols.Name = "uOSetSymbols";
            this.uOSetSymbols.Size = new System.Drawing.Size(474, 23);
            this.uOSetSymbols.TabIndex = 0;
            this.uOSetSymbols.Text = "All";
            this.uOSetSymbols.ValueChanged += new System.EventHandler(this.uOSetSymbols_ValueChanged);
            // 
            // grpBoxSymbols
            // 
            this.grpBoxSymbols.BackColorInternal = System.Drawing.Color.Transparent;
            this.grpBoxSymbols.Controls.Add(this.radioNonZeroPositions);
            this.grpBoxSymbols.Controls.Add(this.radioOptionUnder);
            this.grpBoxSymbols.Controls.Add(this.radioOptions);
            this.grpBoxSymbols.Controls.Add(this.radioAllSymbols);
            this.grpBoxSymbols.Controls.Add(this.lblSymbols);
            this.grpBoxSymbols.Location = new System.Drawing.Point(1000, 1000);
            this.grpBoxSymbols.Name = "grpBoxSymbols";
            this.grpBoxSymbols.Size = new System.Drawing.Size(429, 32);
            this.grpBoxSymbols.TabIndex = 6;
            this.grpBoxSymbols.Visible = false;
            // 
            // radioNonZeroPositions
            // 
            this.radioNonZeroPositions.AutoSize = true;
            this.radioNonZeroPositions.BackColor = System.Drawing.Color.Transparent;
            this.radioNonZeroPositions.Location = new System.Drawing.Point(309, 12);
            this.radioNonZeroPositions.Name = "radioNonZeroPositions";
            this.radioNonZeroPositions.Size = new System.Drawing.Size(120, 19);
            this.inboxControlStyler1.SetStyleSettings(this.radioNonZeroPositions, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.radioNonZeroPositions.TabIndex = 3;
            this.radioNonZeroPositions.Text = "NonZeroPositions";
            this.radioNonZeroPositions.UseVisualStyleBackColor = false;
            this.radioNonZeroPositions.CheckedChanged += new System.EventHandler(this.radioNonZeroPositions_CheckedChanged);
            // 
            // radioOptionUnder
            // 
            this.radioOptionUnder.AutoSize = true;
            this.radioOptionUnder.BackColor = System.Drawing.Color.Transparent;
            this.radioOptionUnder.Location = new System.Drawing.Point(173, 12);
            this.radioOptionUnder.Name = "radioOptionUnder";
            this.radioOptionUnder.Size = new System.Drawing.Size(141, 19);
            this.inboxControlStyler1.SetStyleSettings(this.radioOptionUnder, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.radioOptionUnder.TabIndex = 2;
            this.radioOptionUnder.Text = "Option and Underliers";
            this.radioOptionUnder.UseVisualStyleBackColor = false;
            this.radioOptionUnder.CheckedChanged += new System.EventHandler(this.radioOptionUnder_CheckedChanged);
            // 
            // radioOptions
            // 
            this.radioOptions.AutoSize = true;
            this.radioOptions.BackColor = System.Drawing.Color.Transparent;
            this.radioOptions.Location = new System.Drawing.Point(104, 12);
            this.radioOptions.Name = "radioOptions";
            this.radioOptions.Size = new System.Drawing.Size(67, 19);
            this.inboxControlStyler1.SetStyleSettings(this.radioOptions, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.radioOptions.TabIndex = 2;
            this.radioOptions.Text = "Options";
            this.radioOptions.UseVisualStyleBackColor = false;
            this.radioOptions.CheckedChanged += new System.EventHandler(this.radioOptions_CheckedChanged);
            // 
            // radioAllSymbols
            // 
            this.radioAllSymbols.AutoSize = true;
            this.radioAllSymbols.Checked = true;
            this.radioAllSymbols.Location = new System.Drawing.Point(61, 12);
            this.radioAllSymbols.Name = "radioAllSymbols";
            this.radioAllSymbols.Size = new System.Drawing.Size(39, 19);
            this.inboxControlStyler1.SetStyleSettings(this.radioAllSymbols, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.radioAllSymbols.TabIndex = 1;
            this.radioAllSymbols.TabStop = true;
            this.radioAllSymbols.Text = "All";
            this.radioAllSymbols.UseVisualStyleBackColor = true;
            this.radioAllSymbols.CheckedChanged += new System.EventHandler(this.radioAllSymbols_CheckedChanged);
            // 
            // lblSymbols
            // 
            this.lblSymbols.AutoSize = true;
            this.lblSymbols.Location = new System.Drawing.Point(9, 14);
            this.lblSymbols.Name = "lblSymbols";
            this.lblSymbols.Size = new System.Drawing.Size(53, 18);
            this.lblSymbols.TabIndex = 0;
            this.lblSymbols.Text = "Symbols:";
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.GrpBoxPricingDataSource);
            this.ultraTabPageControl3.Controls.Add(this.ultraPanel1);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(1, 25);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(909, 529);
            // 
            // GrpBoxPricingDataSource
            // 
            appearance37.BackColor = System.Drawing.Color.Transparent;
            appearance37.FontData.SizeInPoints = 9F;
            this.GrpBoxPricingDataSource.Appearance = appearance37;
            this.GrpBoxPricingDataSource.Controls.Add(this.rBtnClosingMark);
            this.GrpBoxPricingDataSource.Controls.Add(this.rBtnLiveData);
            this.GrpBoxPricingDataSource.Controls.Add(this.label1);
            this.GrpBoxPricingDataSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpBoxPricingDataSource.Location = new System.Drawing.Point(1000, 1000);
            this.GrpBoxPricingDataSource.Name = "GrpBoxPricingDataSource";
            this.GrpBoxPricingDataSource.Size = new System.Drawing.Size(547, 51);
            this.GrpBoxPricingDataSource.TabIndex = 115;
            this.GrpBoxPricingDataSource.Visible = false;
            // 
            // rBtnClosingMark
            // 
            this.rBtnClosingMark.AutoSize = true;
            this.rBtnClosingMark.Location = new System.Drawing.Point(205, 19);
            this.rBtnClosingMark.Name = "rBtnClosingMark";
            this.rBtnClosingMark.Size = new System.Drawing.Size(282, 19);
            this.inboxControlStyler1.SetStyleSettings(this.rBtnClosingMark, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rBtnClosingMark.TabIndex = 1;
            this.rBtnClosingMark.TabStop = true;
            this.rBtnClosingMark.Text = "Use Closing Marks to simulate EOD T-1 snapshot";
            this.rBtnClosingMark.UseVisualStyleBackColor = true;
            this.rBtnClosingMark.CheckedChanged += new System.EventHandler(this.rBtnClosingMark_CheckedChanged);
            // 
            // rBtnLiveData
            // 
            this.rBtnLiveData.AutoSize = true;
            this.rBtnLiveData.Location = new System.Drawing.Point(129, 19);
            this.rBtnLiveData.Name = "rBtnLiveData";
            this.rBtnLiveData.Size = new System.Drawing.Size(73, 19);
            this.inboxControlStyler1.SetStyleSettings(this.rBtnLiveData, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rBtnLiveData.TabIndex = 0;
            this.rBtnLiveData.TabStop = true;
            this.rBtnLiveData.Text = "Live Data";
            this.rBtnLiveData.UseVisualStyleBackColor = true;
            this.rBtnLiveData.CheckedChanged += new System.EventHandler(this.rBtnLiveData_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pricing Data Source";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.btnSavePreferences);
            this.ultraPanel1.ClientArea.Controls.Add(this.grpBoxOtherAssetsFeedPrice);
            this.ultraPanel1.ClientArea.Controls.Add(this.ugbxPricingSource);
            this.ultraPanel1.ClientArea.Controls.Add(this.grpBoxUseDefaultDelta);
            this.ultraPanel1.ClientArea.Controls.Add(this.grpBoxOptionSelectedFeedPrice);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(909, 529);
            this.ultraPanel1.TabIndex = 117;
            // 
            // btnSavePreferences
            // 
            appearance38.FontData.SizeInPoints = 9F;
            this.btnSavePreferences.Appearance = appearance38;
            this.btnSavePreferences.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavePreferences.Location = new System.Drawing.Point(413, 371);
            this.btnSavePreferences.Name = "btnSavePreferences";
            this.btnSavePreferences.Size = new System.Drawing.Size(72, 25);
            this.btnSavePreferences.TabIndex = 116;
            this.btnSavePreferences.Text = "Save";
            this.btnSavePreferences.Click += new System.EventHandler(this.btnSavePreferences_Click);
            // 
            // grpBoxOtherAssetsFeedPrice
            // 
            appearance39.FontData.SizeInPoints = 9F;
            this.grpBoxOtherAssetsFeedPrice.Appearance = appearance39;
            this.grpBoxOtherAssetsFeedPrice.BackColorInternal = System.Drawing.Color.Transparent;
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.isLabel2);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.ifLabel2);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.cmbBxOverrideCheckOthers);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.cmbBxOverrideConditionOthers);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.priceBarOthers);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.cmbBxOverrideWithOthers);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.cmbStockPrice);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.label3);
            this.grpBoxOtherAssetsFeedPrice.Controls.Add(this.label6);
            this.grpBoxOtherAssetsFeedPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxOtherAssetsFeedPrice.Location = new System.Drawing.Point(123, 198);
            this.grpBoxOtherAssetsFeedPrice.Name = "grpBoxOtherAssetsFeedPrice";
            this.grpBoxOtherAssetsFeedPrice.Size = new System.Drawing.Size(653, 108);
            this.grpBoxOtherAssetsFeedPrice.TabIndex = 114;
            this.grpBoxOtherAssetsFeedPrice.Text = "Other Assets Selected Feed Price";
            // 
            // isLabel2
            // 
            appearance40.FontData.SizeInPoints = 9F;
            this.isLabel2.Appearance = appearance40;
            this.isLabel2.AutoSize = true;
            this.isLabel2.Location = new System.Drawing.Point(388, 65);
            this.isLabel2.Name = "isLabel2";
            this.isLabel2.Size = new System.Drawing.Size(13, 18);
            this.isLabel2.TabIndex = 117;
            this.isLabel2.Text = "is";
            // 
            // ifLabel2
            // 
            appearance41.FontData.SizeInPoints = 9F;
            this.ifLabel2.Appearance = appearance41;
            this.ifLabel2.AutoSize = true;
            this.ifLabel2.Location = new System.Drawing.Point(249, 65);
            this.ifLabel2.Name = "ifLabel2";
            this.ifLabel2.Size = new System.Drawing.Size(11, 18);
            this.ifLabel2.TabIndex = 117;
            this.ifLabel2.Text = "if";
            // 
            // cmbBxOverrideCheckOthers
            // 
            appearance42.FontData.SizeInPoints = 9F;
            this.cmbBxOverrideCheckOthers.Appearance = appearance42;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            appearance43.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Appearance = appearance43;
            this.cmbBxOverrideCheckOthers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBxOverrideCheckOthers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance44.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance44.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance44.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideCheckOthers.DisplayLayout.GroupByBox.Appearance = appearance44;
            appearance45.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideCheckOthers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance45;
            this.cmbBxOverrideCheckOthers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance46.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance46.BackColor2 = System.Drawing.SystemColors.Control;
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance46.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideCheckOthers.DisplayLayout.GroupByBox.PromptAppearance = appearance46;
            this.cmbBxOverrideCheckOthers.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBxOverrideCheckOthers.DisplayLayout.MaxRowScrollRegions = 1;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.ActiveCellAppearance = appearance47;
            appearance48.BackColor = System.Drawing.SystemColors.Highlight;
            appearance48.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.ActiveRowAppearance = appearance48;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance49.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.CardAreaAppearance = appearance49;
            appearance50.BorderColor = System.Drawing.Color.Silver;
            appearance50.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.CellAppearance = appearance50;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.CellPadding = 0;
            appearance51.BackColor = System.Drawing.SystemColors.Control;
            appearance51.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance51.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance51.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.GroupByRowAppearance = appearance51;
            appearance52.TextHAlignAsString = "Left";
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.HeaderAppearance = appearance52;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance53.BackColor = System.Drawing.SystemColors.Window;
            appearance53.BorderColor = System.Drawing.Color.Silver;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.RowAppearance = appearance53;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance54.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBxOverrideCheckOthers.DisplayLayout.Override.TemplateAddRowAppearance = appearance54;
            this.cmbBxOverrideCheckOthers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBxOverrideCheckOthers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBxOverrideCheckOthers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBxOverrideCheckOthers.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBxOverrideCheckOthers.Location = new System.Drawing.Point(273, 60);
            this.cmbBxOverrideCheckOthers.Name = "cmbBxOverrideCheckOthers";
            this.cmbBxOverrideCheckOthers.Size = new System.Drawing.Size(100, 26);
            this.cmbBxOverrideCheckOthers.TabIndex = 115;
            // 
            // cmbBxOverrideConditionOthers
            // 
            appearance55.FontData.SizeInPoints = 9F;
            this.cmbBxOverrideConditionOthers.Appearance = appearance55;
            appearance56.BackColor = System.Drawing.SystemColors.Window;
            appearance56.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Appearance = appearance56;
            this.cmbBxOverrideConditionOthers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBxOverrideConditionOthers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance57.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance57.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideConditionOthers.DisplayLayout.GroupByBox.Appearance = appearance57;
            appearance58.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideConditionOthers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance58;
            this.cmbBxOverrideConditionOthers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance59.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance59.BackColor2 = System.Drawing.SystemColors.Control;
            appearance59.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance59.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideConditionOthers.DisplayLayout.GroupByBox.PromptAppearance = appearance59;
            this.cmbBxOverrideConditionOthers.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBxOverrideConditionOthers.DisplayLayout.MaxRowScrollRegions = 1;
            appearance60.BackColor = System.Drawing.SystemColors.Window;
            appearance60.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.ActiveCellAppearance = appearance60;
            appearance61.BackColor = System.Drawing.SystemColors.Highlight;
            appearance61.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.ActiveRowAppearance = appearance61;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance62.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.CardAreaAppearance = appearance62;
            appearance63.BorderColor = System.Drawing.Color.Silver;
            appearance63.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.CellAppearance = appearance63;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.CellPadding = 0;
            appearance64.BackColor = System.Drawing.SystemColors.Control;
            appearance64.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance64.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance64.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.GroupByRowAppearance = appearance64;
            appearance65.TextHAlignAsString = "Left";
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.HeaderAppearance = appearance65;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance66.BackColor = System.Drawing.SystemColors.Window;
            appearance66.BorderColor = System.Drawing.Color.Silver;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.RowAppearance = appearance66;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance67.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBxOverrideConditionOthers.DisplayLayout.Override.TemplateAddRowAppearance = appearance67;
            this.cmbBxOverrideConditionOthers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBxOverrideConditionOthers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBxOverrideConditionOthers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBxOverrideConditionOthers.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBxOverrideConditionOthers.Location = new System.Drawing.Point(414, 60);
            this.cmbBxOverrideConditionOthers.Name = "cmbBxOverrideConditionOthers";
            this.cmbBxOverrideConditionOthers.Size = new System.Drawing.Size(100, 26);
            this.cmbBxOverrideConditionOthers.TabIndex = 114;
            // 
            // priceBarOthers
            // 
            this.priceBarOthers.Location = new System.Drawing.Point(536, 61);
            this.priceBarOthers.Name = "priceBarOthers";
            this.priceBarOthers.Size = new System.Drawing.Size(100, 23);
            this.inboxControlStyler1.SetStyleSettings(this.priceBarOthers, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.priceBarOthers.TabIndex = 113;
            this.priceBarOthers.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.priceBarOthers.Maximum = decimal.MaxValue;
            this.priceBarOthers.DecimalPlaces = 6;
            this.priceBarOthers.AllowThousandSeperator = true;
            // 
            // cmbBxOverrideWithOthers
            // 
            appearance68.FontData.SizeInPoints = 9F;
            this.cmbBxOverrideWithOthers.Appearance = appearance68;
            appearance69.BackColor = System.Drawing.SystemColors.Window;
            appearance69.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBxOverrideWithOthers.DisplayLayout.Appearance = appearance69;
            this.cmbBxOverrideWithOthers.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBxOverrideWithOthers.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance70.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance70.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance70.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance70.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideWithOthers.DisplayLayout.GroupByBox.Appearance = appearance70;
            appearance71.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideWithOthers.DisplayLayout.GroupByBox.BandLabelAppearance = appearance71;
            this.cmbBxOverrideWithOthers.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance72.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance72.BackColor2 = System.Drawing.SystemColors.Control;
            appearance72.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance72.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideWithOthers.DisplayLayout.GroupByBox.PromptAppearance = appearance72;
            this.cmbBxOverrideWithOthers.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBxOverrideWithOthers.DisplayLayout.MaxRowScrollRegions = 1;
            appearance73.BackColor = System.Drawing.SystemColors.Window;
            appearance73.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.ActiveCellAppearance = appearance73;
            appearance74.BackColor = System.Drawing.SystemColors.Highlight;
            appearance74.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.ActiveRowAppearance = appearance74;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance75.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.CardAreaAppearance = appearance75;
            appearance76.BorderColor = System.Drawing.Color.Silver;
            appearance76.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.CellAppearance = appearance76;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.CellPadding = 0;
            appearance77.BackColor = System.Drawing.SystemColors.Control;
            appearance77.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance77.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance77.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance77.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.GroupByRowAppearance = appearance77;
            appearance78.TextHAlignAsString = "Left";
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.HeaderAppearance = appearance78;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance79.BackColor = System.Drawing.SystemColors.Window;
            appearance79.BorderColor = System.Drawing.Color.Silver;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.RowAppearance = appearance79;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance80.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBxOverrideWithOthers.DisplayLayout.Override.TemplateAddRowAppearance = appearance80;
            this.cmbBxOverrideWithOthers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBxOverrideWithOthers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBxOverrideWithOthers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBxOverrideWithOthers.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBxOverrideWithOthers.Location = new System.Drawing.Point(130, 60);
            this.cmbBxOverrideWithOthers.Name = "cmbBxOverrideWithOthers";
            this.cmbBxOverrideWithOthers.Size = new System.Drawing.Size(100, 26);
            this.cmbBxOverrideWithOthers.TabIndex = 112;
            this.cmbBxOverrideWithOthers.ValueChanged += new System.EventHandler(this.cmbBxOverrideSelectedPxOthers_ValueChanged);
            // 
            // cmbStockPrice
            // 
            appearance81.FontData.SizeInPoints = 9F;
            this.cmbStockPrice.Appearance = appearance81;
            appearance82.BackColor = System.Drawing.SystemColors.Window;
            appearance82.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbStockPrice.DisplayLayout.Appearance = appearance82;
            this.cmbStockPrice.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStockPrice.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance83.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance83.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance83.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance83.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStockPrice.DisplayLayout.GroupByBox.Appearance = appearance83;
            appearance84.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStockPrice.DisplayLayout.GroupByBox.BandLabelAppearance = appearance84;
            this.cmbStockPrice.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance85.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance85.BackColor2 = System.Drawing.SystemColors.Control;
            appearance85.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance85.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStockPrice.DisplayLayout.GroupByBox.PromptAppearance = appearance85;
            this.cmbStockPrice.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbStockPrice.DisplayLayout.MaxRowScrollRegions = 1;
            appearance86.BackColor = System.Drawing.SystemColors.Window;
            appearance86.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbStockPrice.DisplayLayout.Override.ActiveCellAppearance = appearance86;
            appearance87.BackColor = System.Drawing.SystemColors.Highlight;
            appearance87.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbStockPrice.DisplayLayout.Override.ActiveRowAppearance = appearance87;
            this.cmbStockPrice.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbStockPrice.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance88.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStockPrice.DisplayLayout.Override.CardAreaAppearance = appearance88;
            appearance89.BorderColor = System.Drawing.Color.Silver;
            appearance89.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbStockPrice.DisplayLayout.Override.CellAppearance = appearance89;
            this.cmbStockPrice.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbStockPrice.DisplayLayout.Override.CellPadding = 0;
            appearance90.BackColor = System.Drawing.SystemColors.Control;
            appearance90.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance90.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance90.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance90.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStockPrice.DisplayLayout.Override.GroupByRowAppearance = appearance90;
            appearance91.TextHAlignAsString = "Left";
            this.cmbStockPrice.DisplayLayout.Override.HeaderAppearance = appearance91;
            this.cmbStockPrice.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbStockPrice.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance92.BackColor = System.Drawing.SystemColors.Window;
            appearance92.BorderColor = System.Drawing.Color.Silver;
            this.cmbStockPrice.DisplayLayout.Override.RowAppearance = appearance92;
            this.cmbStockPrice.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance93.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbStockPrice.DisplayLayout.Override.TemplateAddRowAppearance = appearance93;
            this.cmbStockPrice.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbStockPrice.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStockPrice.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbStockPrice.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbStockPrice.Location = new System.Drawing.Point(130, 27);
            this.cmbStockPrice.Name = "cmbStockPrice";
            this.cmbStockPrice.Size = new System.Drawing.Size(100, 26);
            this.cmbStockPrice.TabIndex = 111;
            this.cmbStockPrice.ValueChanged += new System.EventHandler(this.cmbStockPrice_ValueChanged);
            // 
            // label3
            // 
            appearance94.FontData.SizeInPoints = 9F;
            this.label3.Appearance = appearance94;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 18);
            this.label3.TabIndex = 110;
            this.label3.Text = "Override with: ";
            // 
            // label6
            // 
            appearance95.FontData.SizeInPoints = 9F;
            this.label6.Appearance = appearance95;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 18);
            this.label6.TabIndex = 98;
            this.label6.Text = "Price : ";
            // 
            // ugbxPricingSource
            // 
            appearance96.FontData.SizeInPoints = 9F;
            this.ugbxPricingSource.Appearance = appearance96;
            this.ugbxPricingSource.Controls.Add(this.uOsetPricingSource);
            this.ugbxPricingSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ugbxPricingSource.Location = new System.Drawing.Point(123, 20);
            this.ugbxPricingSource.Name = "ugbxPricingSource";
            this.ugbxPricingSource.Size = new System.Drawing.Size(653, 56);
            this.ugbxPricingSource.TabIndex = 115;
            this.ugbxPricingSource.Text = "Pricing Source";
            // 
            // uOsetPricingSource
            // 
            appearance97.FontData.SizeInPoints = 9F;
            this.uOsetPricingSource.Appearance = appearance97;
            this.uOsetPricingSource.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.uOsetPricingSource.CheckedIndex = 0;
            appearance98.FontData.SizeInPoints = 9F;
            this.uOsetPricingSource.ItemAppearance = appearance98;
            valueListItem5.DataValue = "LiveData";
            valueListItem5.DisplayText = "Live Data";
            valueListItem6.DataValue = "EODTClosingMark";
            valueListItem6.DisplayText = "Use Closing Marks to simulate EOD T-1 snapshot";
            this.uOsetPricingSource.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6});
            this.uOsetPricingSource.ItemSpacingHorizontal = 10;
            this.uOsetPricingSource.ItemSpacingVertical = 5;
            this.uOsetPricingSource.Location = new System.Drawing.Point(96, 18);
            this.uOsetPricingSource.Name = "uOsetPricingSource";
            this.uOsetPricingSource.Size = new System.Drawing.Size(397, 32);
            this.uOsetPricingSource.TabIndex = 3;
            this.uOsetPricingSource.Text = "Live Data";
            this.uOsetPricingSource.ValueChanged += new System.EventHandler(this.ultraOptionSet1_ValueChanged);
            // 
            // grpBoxUseDefaultDelta
            // 
            appearance99.FontData.SizeInPoints = 9F;
            this.grpBoxUseDefaultDelta.Appearance = appearance99;
            this.grpBoxUseDefaultDelta.BackColorInternal = System.Drawing.Color.Transparent;
            this.grpBoxUseDefaultDelta.Controls.Add(this.checkBoxUseDefaultDelta);
            this.grpBoxUseDefaultDelta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxUseDefaultDelta.Location = new System.Drawing.Point(123, 313);
            this.grpBoxUseDefaultDelta.Name = "grpBoxUseDefaultDelta";
            this.grpBoxUseDefaultDelta.Size = new System.Drawing.Size(653, 51);
            this.grpBoxUseDefaultDelta.TabIndex = 114;
            this.grpBoxUseDefaultDelta.Text = "Use Default Delta";
            // 
            // checkBoxUseDefaultDelta
            // 
            appearance100.FontData.SizeInPoints = 9F;
            this.checkBoxUseDefaultDelta.Appearance = appearance100;
            this.checkBoxUseDefaultDelta.AutoSize = true;
            this.checkBoxUseDefaultDelta.Location = new System.Drawing.Point(140, 21);
            this.checkBoxUseDefaultDelta.Name = "checkBoxUseDefaultDelta";
            this.checkBoxUseDefaultDelta.Size = new System.Drawing.Size(118, 21);
            this.checkBoxUseDefaultDelta.TabIndex = 12;
            this.checkBoxUseDefaultDelta.Text = "Use Default Delta";
            // 
            // grpBoxOptionSelectedFeedPrice
            // 
            appearance101.FontData.SizeInPoints = 9F;
            this.grpBoxOptionSelectedFeedPrice.Appearance = appearance101;
            this.grpBoxOptionSelectedFeedPrice.BackColorInternal = System.Drawing.Color.Transparent;
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.isLabel1);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.ifLabel1);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.cmbBxOverrideCheckOptions);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.cmbBxOverrideConditionOptions);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.priceBarOptions);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.cmbBxOverrideWithOptions);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.cmbOptPrice);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.label2);
            this.grpBoxOptionSelectedFeedPrice.Controls.Add(this.label4);
            this.grpBoxOptionSelectedFeedPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxOptionSelectedFeedPrice.Location = new System.Drawing.Point(123, 83);
            this.grpBoxOptionSelectedFeedPrice.Name = "grpBoxOptionSelectedFeedPrice";
            this.grpBoxOptionSelectedFeedPrice.Size = new System.Drawing.Size(653, 108);
            this.grpBoxOptionSelectedFeedPrice.TabIndex = 113;
            this.grpBoxOptionSelectedFeedPrice.Text = "Option Selected Feed Price";
            // 
            // isLabel1
            // 
            appearance102.FontData.SizeInPoints = 9F;
            this.isLabel1.Appearance = appearance102;
            this.isLabel1.AutoSize = true;
            this.isLabel1.Location = new System.Drawing.Point(388, 63);
            this.isLabel1.Name = "isLabel1";
            this.isLabel1.Size = new System.Drawing.Size(13, 18);
            this.isLabel1.TabIndex = 116;
            this.isLabel1.Text = "is";
            // 
            // ifLabel1
            // 
            appearance103.FontData.SizeInPoints = 9F;
            this.ifLabel1.Appearance = appearance103;
            this.ifLabel1.AutoSize = true;
            this.ifLabel1.Location = new System.Drawing.Point(249, 63);
            this.ifLabel1.Name = "ifLabel1";
            this.ifLabel1.Size = new System.Drawing.Size(11, 18);
            this.ifLabel1.TabIndex = 115;
            this.ifLabel1.Text = "if";
            // 
            // cmbBxOverrideCheckOptions
            // 
            appearance104.FontData.SizeInPoints = 9F;
            this.cmbBxOverrideCheckOptions.Appearance = appearance104;
            appearance105.BackColor = System.Drawing.SystemColors.Window;
            appearance105.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Appearance = appearance105;
            this.cmbBxOverrideCheckOptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBxOverrideCheckOptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance106.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance106.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance106.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance106.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideCheckOptions.DisplayLayout.GroupByBox.Appearance = appearance106;
            appearance107.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideCheckOptions.DisplayLayout.GroupByBox.BandLabelAppearance = appearance107;
            this.cmbBxOverrideCheckOptions.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance108.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance108.BackColor2 = System.Drawing.SystemColors.Control;
            appearance108.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance108.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideCheckOptions.DisplayLayout.GroupByBox.PromptAppearance = appearance108;
            this.cmbBxOverrideCheckOptions.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBxOverrideCheckOptions.DisplayLayout.MaxRowScrollRegions = 1;
            appearance109.BackColor = System.Drawing.SystemColors.Window;
            appearance109.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.ActiveCellAppearance = appearance109;
            appearance110.BackColor = System.Drawing.SystemColors.Highlight;
            appearance110.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.ActiveRowAppearance = appearance110;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance111.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.CardAreaAppearance = appearance111;
            appearance112.BorderColor = System.Drawing.Color.Silver;
            appearance112.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.CellAppearance = appearance112;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.CellPadding = 0;
            appearance113.BackColor = System.Drawing.SystemColors.Control;
            appearance113.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance113.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance113.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance113.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.GroupByRowAppearance = appearance113;
            appearance114.TextHAlignAsString = "Left";
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.HeaderAppearance = appearance114;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance115.BackColor = System.Drawing.SystemColors.Window;
            appearance115.BorderColor = System.Drawing.Color.Silver;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.RowAppearance = appearance115;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance116.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBxOverrideCheckOptions.DisplayLayout.Override.TemplateAddRowAppearance = appearance116;
            this.cmbBxOverrideCheckOptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBxOverrideCheckOptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBxOverrideCheckOptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBxOverrideCheckOptions.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBxOverrideCheckOptions.Location = new System.Drawing.Point(273, 60);
            this.cmbBxOverrideCheckOptions.Name = "cmbBxOverrideCheckOptions";
            this.cmbBxOverrideCheckOptions.Size = new System.Drawing.Size(100, 26);
            this.cmbBxOverrideCheckOptions.TabIndex = 114;
            // 
            // cmbBxOverrideConditionOptions
            // 
            appearance117.FontData.SizeInPoints = 9F;
            this.cmbBxOverrideConditionOptions.Appearance = appearance117;
            appearance118.BackColor = System.Drawing.SystemColors.Window;
            appearance118.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Appearance = appearance118;
            this.cmbBxOverrideConditionOptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBxOverrideConditionOptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance119.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance119.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance119.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance119.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideConditionOptions.DisplayLayout.GroupByBox.Appearance = appearance119;
            appearance120.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideConditionOptions.DisplayLayout.GroupByBox.BandLabelAppearance = appearance120;
            this.cmbBxOverrideConditionOptions.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance121.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance121.BackColor2 = System.Drawing.SystemColors.Control;
            appearance121.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance121.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideConditionOptions.DisplayLayout.GroupByBox.PromptAppearance = appearance121;
            this.cmbBxOverrideConditionOptions.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBxOverrideConditionOptions.DisplayLayout.MaxRowScrollRegions = 1;
            appearance122.BackColor = System.Drawing.SystemColors.Window;
            appearance122.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.ActiveCellAppearance = appearance122;
            appearance123.BackColor = System.Drawing.SystemColors.Highlight;
            appearance123.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.ActiveRowAppearance = appearance123;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance124.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.CardAreaAppearance = appearance124;
            appearance125.BorderColor = System.Drawing.Color.Silver;
            appearance125.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.CellAppearance = appearance125;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.CellPadding = 0;
            appearance126.BackColor = System.Drawing.SystemColors.Control;
            appearance126.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance126.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance126.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance126.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.GroupByRowAppearance = appearance126;
            appearance127.TextHAlignAsString = "Left";
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.HeaderAppearance = appearance127;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance128.BackColor = System.Drawing.SystemColors.Window;
            appearance128.BorderColor = System.Drawing.Color.Silver;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.RowAppearance = appearance128;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance129.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBxOverrideConditionOptions.DisplayLayout.Override.TemplateAddRowAppearance = appearance129;
            this.cmbBxOverrideConditionOptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBxOverrideConditionOptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBxOverrideConditionOptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBxOverrideConditionOptions.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBxOverrideConditionOptions.Location = new System.Drawing.Point(414, 60);
            this.cmbBxOverrideConditionOptions.Name = "cmbBxOverrideConditionOptions";
            this.cmbBxOverrideConditionOptions.Size = new System.Drawing.Size(100, 26);
            this.cmbBxOverrideConditionOptions.TabIndex = 113;
            // 
            // priceBarOptions
            // 
            this.priceBarOptions.Location = new System.Drawing.Point(536, 61);
            this.priceBarOptions.Name = "priceBarOptions";
            this.priceBarOptions.Size = new System.Drawing.Size(100, 23);
            this.inboxControlStyler1.SetStyleSettings(this.priceBarOptions, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.priceBarOptions.TabIndex = 112;
            this.priceBarOptions.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.priceBarOptions.Maximum = decimal.MaxValue;
            this.priceBarOptions.DecimalPlaces = 6;
            this.priceBarOptions.AllowThousandSeperator = true;
            // 
            // cmbBxOverrideWithOptions
            // 
            appearance130.FontData.SizeInPoints = 9F;
            this.cmbBxOverrideWithOptions.Appearance = appearance130;
            appearance131.BackColor = System.Drawing.SystemColors.Window;
            appearance131.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBxOverrideWithOptions.DisplayLayout.Appearance = appearance131;
            this.cmbBxOverrideWithOptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBxOverrideWithOptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance132.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance132.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance132.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance132.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideWithOptions.DisplayLayout.GroupByBox.Appearance = appearance132;
            appearance133.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideWithOptions.DisplayLayout.GroupByBox.BandLabelAppearance = appearance133;
            this.cmbBxOverrideWithOptions.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance134.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance134.BackColor2 = System.Drawing.SystemColors.Control;
            appearance134.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance134.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBxOverrideWithOptions.DisplayLayout.GroupByBox.PromptAppearance = appearance134;
            this.cmbBxOverrideWithOptions.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBxOverrideWithOptions.DisplayLayout.MaxRowScrollRegions = 1;
            appearance135.BackColor = System.Drawing.SystemColors.Window;
            appearance135.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.ActiveCellAppearance = appearance135;
            appearance136.BackColor = System.Drawing.SystemColors.Highlight;
            appearance136.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.ActiveRowAppearance = appearance136;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance137.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.CardAreaAppearance = appearance137;
            appearance138.BorderColor = System.Drawing.Color.Silver;
            appearance138.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.CellAppearance = appearance138;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.CellPadding = 0;
            appearance139.BackColor = System.Drawing.SystemColors.Control;
            appearance139.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance139.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance139.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance139.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.GroupByRowAppearance = appearance139;
            appearance140.TextHAlignAsString = "Left";
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.HeaderAppearance = appearance140;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance141.BackColor = System.Drawing.SystemColors.Window;
            appearance141.BorderColor = System.Drawing.Color.Silver;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.RowAppearance = appearance141;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance142.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBxOverrideWithOptions.DisplayLayout.Override.TemplateAddRowAppearance = appearance142;
            this.cmbBxOverrideWithOptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBxOverrideWithOptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBxOverrideWithOptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBxOverrideWithOptions.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbBxOverrideWithOptions.Location = new System.Drawing.Point(130, 60);
            this.cmbBxOverrideWithOptions.Name = "cmbBxOverrideWithOptions";
            this.cmbBxOverrideWithOptions.Size = new System.Drawing.Size(100, 26);
            this.cmbBxOverrideWithOptions.TabIndex = 111;
            this.cmbBxOverrideWithOptions.ValueChanged += new System.EventHandler(this.cmbBxOverrideSelectedPxOptions_ValueChanged);
            // 
            // cmbOptPrice
            // 
            appearance143.FontData.SizeInPoints = 9F;
            this.cmbOptPrice.Appearance = appearance143;
            appearance144.BackColor = System.Drawing.SystemColors.Window;
            appearance144.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOptPrice.DisplayLayout.Appearance = appearance144;
            this.cmbOptPrice.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOptPrice.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance145.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance145.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance145.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance145.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOptPrice.DisplayLayout.GroupByBox.Appearance = appearance145;
            appearance146.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOptPrice.DisplayLayout.GroupByBox.BandLabelAppearance = appearance146;
            this.cmbOptPrice.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance147.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance147.BackColor2 = System.Drawing.SystemColors.Control;
            appearance147.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance147.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOptPrice.DisplayLayout.GroupByBox.PromptAppearance = appearance147;
            this.cmbOptPrice.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOptPrice.DisplayLayout.MaxRowScrollRegions = 1;
            appearance148.BackColor = System.Drawing.SystemColors.Window;
            appearance148.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOptPrice.DisplayLayout.Override.ActiveCellAppearance = appearance148;
            appearance149.BackColor = System.Drawing.SystemColors.Highlight;
            appearance149.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOptPrice.DisplayLayout.Override.ActiveRowAppearance = appearance149;
            this.cmbOptPrice.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOptPrice.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance150.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOptPrice.DisplayLayout.Override.CardAreaAppearance = appearance150;
            appearance151.BorderColor = System.Drawing.Color.Silver;
            appearance151.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOptPrice.DisplayLayout.Override.CellAppearance = appearance151;
            this.cmbOptPrice.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOptPrice.DisplayLayout.Override.CellPadding = 0;
            appearance152.BackColor = System.Drawing.SystemColors.Control;
            appearance152.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance152.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance152.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance152.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOptPrice.DisplayLayout.Override.GroupByRowAppearance = appearance152;
            appearance153.TextHAlignAsString = "Left";
            this.cmbOptPrice.DisplayLayout.Override.HeaderAppearance = appearance153;
            this.cmbOptPrice.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOptPrice.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance154.BackColor = System.Drawing.SystemColors.Window;
            appearance154.BorderColor = System.Drawing.Color.Silver;
            this.cmbOptPrice.DisplayLayout.Override.RowAppearance = appearance154;
            this.cmbOptPrice.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance155.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOptPrice.DisplayLayout.Override.TemplateAddRowAppearance = appearance155;
            this.cmbOptPrice.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOptPrice.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOptPrice.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOptPrice.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOptPrice.Location = new System.Drawing.Point(130, 27);
            this.cmbOptPrice.Name = "cmbOptPrice";
            this.cmbOptPrice.Size = new System.Drawing.Size(100, 26);
            this.cmbOptPrice.TabIndex = 110;
            this.cmbOptPrice.ValueChanged += new System.EventHandler(this.cmbOptPrice_ValueChanged);
            // 
            // label2
            // 
            appearance156.FontData.SizeInPoints = 9F;
            this.label2.Appearance = appearance156;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 18);
            this.label2.TabIndex = 109;
            this.label2.Text = "Override with:";
            // 
            // label4
            // 
            appearance157.FontData.SizeInPoints = 9F;
            this.label4.Appearance = appearance157;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 18);
            this.label4.TabIndex = 98;
            this.label4.Text = "Option Price : ";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(925, 547);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(909, 529);
            // 
            // ultraTabControl1
            // 
            appearance158.FontData.SizeInPoints = 9F;
            this.ultraTabControl1.Appearance = appearance158;
            appearance159.BackColor = System.Drawing.Color.White;
            this.ultraTabControl1.ClientAreaAppearance = appearance159;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTabControl1.Location = new System.Drawing.Point(8, 29);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(913, 557);
            this.ultraTabControl1.TabIndex = 0;
            if (!CustomThemeHelper.ApplyTheme)
            {
                appearance160.BackColor = System.Drawing.SystemColors.Control;
                appearance160.BackColor2 = System.Drawing.SystemColors.Control;
                ultraTab1.ActiveAppearance = appearance160;
                appearance161.BackColor = System.Drawing.SystemColors.Control;
                appearance161.BackColor2 = System.Drawing.SystemColors.Control;
                ultraTab1.Appearance = appearance161;
                appearance162.BackColor = System.Drawing.SystemColors.Control;
                appearance162.BackColor2 = System.Drawing.SystemColors.Control;
            }
            ultraTab1.ClientAreaAppearance = appearance162;
            ultraTab1.Key = "tabOptionModelInputs";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Pricing Inputs";
            ultraTab3.Key = "tabPreferences";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Preferences";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab3});
            this.ultraTabControl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(21, 52);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(223, 17);
            this.inboxControlStyler1.SetStyleSettings(this.radioButton4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.radioButton4.TabIndex = 111;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Use Last When either Ask,Bid or Mid Zero";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(253, 45);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(94, 22);
            this.comboBox1.TabIndex = 108;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _OptionModelInputs_UltraFormManager_Dock_Area_Left
            // 
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 29);
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.Name = "_OptionModelInputs_UltraFormManager_Dock_Area_Left";
            this._OptionModelInputs_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 557);
            // 
            // _OptionModelInputs_UltraFormManager_Dock_Area_Right
            // 
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(921, 29);
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.Name = "_OptionModelInputs_UltraFormManager_Dock_Area_Right";
            this._OptionModelInputs_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 557);
            // 
            // _OptionModelInputs_UltraFormManager_Dock_Area_Top
            // 
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.Name = "_OptionModelInputs_UltraFormManager_Dock_Area_Top";
            this._OptionModelInputs_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(929, 29);
            // 
            // _OptionModelInputs_UltraFormManager_Dock_Area_Bottom
            // 
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 586);
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.Name = "_OptionModelInputs_UltraFormManager_Dock_Area_Bottom";
            this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(929, 8);
            // 
            // OptionModelInputs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 594);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this._OptionModelInputs_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._OptionModelInputs_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._OptionModelInputs_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._OptionModelInputs_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "OptionModelInputs";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Pricing Inputs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionModelInputs_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionModelInputs_FormClosed);
            this.Load += new System.EventHandler(this.OptionModelInputs_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.upnlBody.ClientArea.ResumeLayout(false);
            this.upnlBody.ClientArea.PerformLayout();
            this.upnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOptionModel)).EndInit();
            this.contextmenu.ResumeLayout(false);
            this.statusStripOMI.ResumeLayout(false);
            this.statusStripOMI.PerformLayout();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.upnlButtons.ClientArea.ResumeLayout(false);
            this.upnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxShowCols)).EndInit();
            this.grpBoxShowCols.ResumeLayout(false);
            this.grpBoxShowCols.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxClosingMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxManualInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxSharesOutstanding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxLastPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxDividend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxVolatility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxTheoreticalPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxSymbols)).EndInit();
            this.ugbxSymbols.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uOSetSymbols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxSymbols)).EndInit();
            this.grpBoxSymbols.ResumeLayout(false);
            this.grpBoxSymbols.PerformLayout();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpBoxPricingDataSource)).EndInit();
            this.GrpBoxPricingDataSource.ResumeLayout(false);
            this.GrpBoxPricingDataSource.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxOtherAssetsFeedPrice)).EndInit();
            this.grpBoxOtherAssetsFeedPrice.ResumeLayout(false);
            this.grpBoxOtherAssetsFeedPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideCheckOthers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideConditionOthers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceBarOthers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideWithOthers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStockPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbxPricingSource)).EndInit();
            this.ugbxPricingSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uOsetPricingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxUseDefaultDelta)).EndInit();
            this.grpBoxUseDefaultDelta.ResumeLayout(false);
            this.grpBoxUseDefaultDelta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxUseDefaultDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxOptionSelectedFeedPrice)).EndInit();
            this.grpBoxOptionSelectedFeedPrice.ResumeLayout(false);
            this.grpBoxOptionSelectedFeedPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideCheckOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideConditionOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceBarOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBxOverrideWithOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        //private System.Windows.Forms.ToolStripMenuItem mnuAddRow;
        //private System.Windows.Forms.ToolStripMenuItem mnuDeleteRow;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.ContextMenuStrip contextmenu;
        private System.Windows.Forms.ToolStripMenuItem menuSaveLayout;
        private System.Windows.Forms.ToolStripMenuItem menuClearFilters;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteSymbol;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.Misc.UltraButton btnHistoricalVolInputs;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxSymbols;
        private System.Windows.Forms.RadioButton radioOptionUnder;
        private System.Windows.Forms.RadioButton radioOptions;
        private System.Windows.Forms.RadioButton radioAllSymbols;
        private Infragistics.Win.Misc.UltraLabel lblSymbols;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxShowCols;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxAll;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxLastPrice;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxDelta;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxDividend;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxIR;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxVolatility;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxTheoreticalPrice;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.Misc.UltraButton btnRefreshLiveData;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnSymbolLookup;
        private PranaUltraGrid grdOptionModel;
        private System.Windows.Forms.StatusStrip statusStripOMI;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelOMI;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.Misc.UltraLabel label1;
        private System.Windows.Forms.RadioButton rBtnClosingMark;
        private System.Windows.Forms.RadioButton rBtnLiveData;
        //private Infragistics.Win.Misc.UltraGroupBox grpBoxOtherAssetsSelectedPrice;
        private Infragistics.Win.Misc.UltraLabel label6;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxOptionSelectedFeedPrice;
        private Infragistics.Win.Misc.UltraLabel label4;
        private Infragistics.Win.Misc.UltraGroupBox GrpBoxPricingDataSource;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxOtherAssetsFeedPrice;
        private Infragistics.Win.Misc.UltraButton btnSavePreferences;
        private Infragistics.Win.Misc.UltraButton btnGetHistoricalVol;
        //private Infragistics.Win.Misc.UltraButton btnScreenshot;
        private System.Windows.Forms.RadioButton radioButton4;
        private Infragistics.Win.Misc.UltraLabel label3;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.UltraWinGrid.UltraCombo comboBox1;
        //private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbOptPrice;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbBxOverrideWithOthers;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbStockPrice;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbBxOverrideWithOptions;
        private System.Windows.Forms.RadioButton radioNonZeroPositions;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxUseDefaultDelta;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxUseDefaultDelta;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxSharesOutstanding;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxClosingMark;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxManualInput;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uOSetSymbols;
        private Infragistics.Win.Misc.UltraGroupBox ugbxSymbols;
        private Infragistics.Win.Misc.UltraPanel upnlBody;
        private Infragistics.Win.Misc.UltraPanel upnlButtons;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uOsetPricingSource;
        private Infragistics.Win.Misc.UltraGroupBox ugbxPricingSource;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _OptionModelInputs_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _OptionModelInputs_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _OptionModelInputs_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _OptionModelInputs_UltraFormManager_Dock_Area_Bottom;
        private PranaNumericUpDown priceBarOthers;
        private PranaNumericUpDown priceBarOptions;
        private UltraCombo cmbBxOverrideConditionOthers;
        private UltraCombo cmbBxOverrideConditionOptions;
        private UltraCombo cmbBxOverrideCheckOthers;
        private UltraCombo cmbBxOverrideCheckOptions;
        private Infragistics.Win.Misc.UltraLabel isLabel2;
        private Infragistics.Win.Misc.UltraLabel ifLabel2;
        private Infragistics.Win.Misc.UltraLabel isLabel1;
        private Infragistics.Win.Misc.UltraLabel ifLabel1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}