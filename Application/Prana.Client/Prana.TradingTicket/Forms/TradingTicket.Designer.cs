using Infragistics.Win;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket.Forms
{
    partial class TradingTicket
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
                if (ttPresenter != null)
                {
                    ttPresenter.Dispose();
                }
                if (borrowParameter != null)
                {
                    borrowParameter.Dispose();
                }
                if (ToolTip1 != null)
                {
                    ToolTip1.Dispose();
                }
                if (formshortLocateListPopup != null)
                {
                    formshortLocateListPopup.Dispose();
                }
                if (_secMasterSyncService != null)
                {
                    _secMasterSyncService.Dispose();
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
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab(true);
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpBxAlgo = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.grpBxPnlAlgo = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tblPnlAlgo = new System.Windows.Forms.TableLayoutPanel();
            this.algoStrategyControl = new Prana.AlgoStrategyControls.AlgoStrategyControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpBxCommision = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.uExpandableGrpBxPanelCommision = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tblPnlCommision = new System.Windows.Forms.TableLayoutPanel();
            this.nmrcSoftRate = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.cmbCommissionBasis = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblCommissionBasis = new Infragistics.Win.Misc.UltraLabel();
            this.lblCommissionRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblSoftCommissionBasis = new Infragistics.Win.Misc.UltraLabel();
            this.lblSoftRate = new Infragistics.Win.Misc.UltraLabel();
            this.cmbSoftCommissionBasis = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.nmrcCommissionRate = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpBxTradeAttribute = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.uExpandableGrpBxPanelTradeAttribute = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tblPnlTradeAttribute = new System.Windows.Forms.TableLayoutPanel();
            this.cmbTradeAttribute6 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute5 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute4 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute3 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbTradeAttribute2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTradeAttribute6 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute5 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeAttribute1 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbTradeAttribute1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grbBxOther = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.uExpandableGrpBxPanelOther = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tblPnlOthers = new System.Windows.Forms.TableLayoutPanel();
            this.cmbTradingAccount = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbFunds = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbHandlingInstructions = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbExecutionInstructions = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblVenue = new Infragistics.Win.Misc.UltraLabel();
            this.lblExecutionInstructions = new Infragistics.Win.Misc.UltraLabel();
            this.lblHandlingInstructions = new Infragistics.Win.Misc.UltraLabel();
            this.cmbVenue = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTradingAccount = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grpBxSwap = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.grpBxPnlSwap = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ctrlSwapParameters1 = new Prana.ClientCommon.CtrlSwapParameters();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.grbBxSettlementFields = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.grpBxPnlSettlementFields = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tblPnlSettlementFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblSettlementCurrencyPrice = new Infragistics.Win.Misc.UltraLabel();
            this.cmbSettlementCurrency = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblSettlementCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.nmrcSettlementPrice = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tblPnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.tblPnlAllControls = new System.Windows.Forms.TableLayoutPanel();
            this.tblPnlSymbolControl = new System.Windows.Forms.TableLayoutPanel();
            this.lblHiddenStrategyAdjust = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblFunds = new Infragistics.Win.Misc.UltraLabel();
            this.pranaSymbolCtrl = new Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl();
            this.ultraLegend2 = new Infragistics.Win.DataVisualization.UltraLegend();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanelSymbolLookup = new Infragistics.Win.Misc.UltraPanel();
            this.chkBoxOption = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBoxEquitySwap = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBoxCFD = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBoxConvertiableBond = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkBoxSwap = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.grbBoxOptionControl = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.grbBoxStrategyControl = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.uExoandalPnlOptionControl = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.uExoandalStrategyControl = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.pranaOptionCtrl = new PranaOptionCtrl();
            this.tblPnlTTMainControls = new System.Windows.Forms.TableLayoutPanel();
            this.oneSymbolL1Strip = new Prana.LiveFeed.UI.Controls.OneSymbolL1Strip();
            this.lblOrderSide = new Infragistics.Win.Misc.UltraLabel();
            this.cmbOrderSide = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbAllocation = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbBroker = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblBroker = new Infragistics.Win.Misc.UltraLabel();
            this.lblTIF = new Infragistics.Win.Misc.UltraLabel();
            this.lblOrderType = new Infragistics.Win.Misc.UltraLabel();
            this.lblPrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblStop = new Infragistics.Win.Misc.UltraLabel();
            this.cmbTIF = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbOrderType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblStrategy = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradeDate = new Infragistics.Win.Misc.UltraLabel();
            this.cmbStrategy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.dtTradeDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtExpireTime = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            this.txtNotes = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtBrokerNotes = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.bottomTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.nmrcQuantity = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.nmrcTargetQuantity = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.nmrcPrice = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.nmrcStop = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.tblPnlButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnViewAllocationDetails = new Infragistics.Win.Misc.UltraButton();
            this.btnOTCControl = new Infragistics.Win.Misc.UltraButton();
            this.btnSend = new Infragistics.Win.Misc.UltraButton();
            this.btnShortLocateList = new Infragistics.Win.Misc.UltraButton();
            this.btnCreateOrder = new Infragistics.Win.Misc.UltraButton();
            this.btnDoneAway = new Infragistics.Win.Misc.UltraButton();
            this.btnReplace = new Infragistics.Win.Misc.UltraButton();
            this.nmrcLimit = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.lblErrorMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblAlgoMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblBrokerMessage = new Infragistics.Win.Misc.UltraLabel();
            this.pnlLimitPrice = new Infragistics.Win.Misc.UltraPanel();
            this.btnGetLimitPrice = new Infragistics.Win.Misc.UltraButton();
            this.lblLimit = new Infragistics.Win.Misc.UltraLabel();
            this.PnlTargetQuantity = new Infragistics.Win.Misc.UltraPanel();
            this.btnTargetQuantityPercentage = new Infragistics.Win.Misc.UltraButton();
            this.PnlBroker = new Infragistics.Win.Misc.UltraPanel();
            this.btnPadlock = new Infragistics.Win.Misc.UltraButton();
            this.btnBrokerConnectionStatus = new Infragistics.Win.Misc.UltraButton();
            this.btnMultiBrokerConnectionStatus = new Infragistics.Win.Misc.UltraButton();
            this.lblTargetQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel5 = new Infragistics.Win.Misc.UltraPanel();
            this.btnAccountQty = new Infragistics.Win.Misc.UltraButton();
            this.btnExpireTime = new Infragistics.Win.Misc.UltraButton();
            this.lblAllocation = new Infragistics.Win.Misc.UltraLabel();
            this.lblExpireTime = new Infragistics.Win.Misc.UltraLabel();
            this.lblFXRate = new Infragistics.Win.Misc.UltraLabel();
            this.lblCCA = new Infragistics.Win.Misc.UltraLabel();
            this.nmrcFXRate = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.nmrcCCA = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.lblDealIn = new Infragistics.Win.Misc.UltraLabel();
            this.cmbDealIn = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblFxOperator = new Infragistics.Win.Misc.UltraLabel();
            this.cmbFxOperator = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.btnSymbolLookup = new System.Windows.Forms.Button();
            this.ultraLegend1 = new Infragistics.Win.DataVisualization.UltraLegend();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._TradingTicket_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TradingTicket_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TradingTicket_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlNotionalQuantity = new Infragistics.Win.Misc.UltraPanel();
            this.lblQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.btnQuantity = new Infragistics.Win.Misc.UltraButton();
            this.strategyControl1 = new Prana.TradingTicket.StrategyControl();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.shortLocateList1 = new Prana.ShortLocate.Controls.ShortLocateList();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxAlgo)).BeginInit();
            this.grpBxAlgo.SuspendLayout();
            this.grpBxPnlAlgo.SuspendLayout();
            this.tblPnlAlgo.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxCommision)).BeginInit();
            this.grpBxCommision.SuspendLayout();
            this.uExpandableGrpBxPanelCommision.SuspendLayout();
            this.tblPnlCommision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcSoftRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionBasis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSoftCommissionBasis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcCommissionRate)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxTradeAttribute)).BeginInit();
            this.grpBxTradeAttribute.SuspendLayout();
            this.uExpandableGrpBxPanelTradeAttribute.SuspendLayout();
            this.tblPnlTradeAttribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute1)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbBxOther)).BeginInit();
            this.grbBxOther.SuspendLayout();
            this.uExpandableGrpBxPanelOther.SuspendLayout();
            this.tblPnlOthers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandlingInstructions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecutionInstructions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            this.ultraTabPageControl9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxSwap)).BeginInit();
            this.grpBxSwap.SuspendLayout();
            this.grpBxPnlSwap.SuspendLayout();
            this.ultraTabPageControl10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbBxSettlementFields)).BeginInit();
            this.grbBxSettlementFields.SuspendLayout();
            this.grpBxPnlSettlementFields.SuspendLayout();
            this.tblPnlSettlementFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSettlementCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcSettlementPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.tblPnlMain.SuspendLayout();
            this.tblPnlAllControls.SuspendLayout();
            this.tblPnlSymbolControl.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            this.ultraPanelSymbolLookup.ClientArea.SuspendLayout();
            this.ultraPanelSymbolLookup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxEquitySwap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxCFD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxConvertiableBond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxSwap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbBoxOptionControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbBoxStrategyControl)).BeginInit();
            this.grbBoxStrategyControl.SuspendLayout();
            this.grbBoxOptionControl.SuspendLayout();
            this.uExoandalPnlOptionControl.SuspendLayout();
            this.uExoandalStrategyControl.SuspendLayout();
            this.tblPnlTTMainControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBroker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTradeDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtExpireTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBrokerNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomTabControl)).BeginInit();
            this.bottomTabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcTargetQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcStop)).BeginInit();
            this.tblPnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcLimit)).BeginInit();
            this.pnlLimitPrice.ClientArea.SuspendLayout();
            this.pnlLimitPrice.SuspendLayout();
            this.PnlTargetQuantity.ClientArea.SuspendLayout();
            this.PnlTargetQuantity.SuspendLayout();
            this.PnlBroker.ClientArea.SuspendLayout();
            this.PnlBroker.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel5.ClientArea.SuspendLayout();
            this.ultraPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcFXRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcCCA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDealIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFxOperator)).BeginInit();
            this.pnlNotionalQuantity.ClientArea.SuspendLayout();
            this.pnlNotionalQuantity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.ultraTabPageControl11.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ultraTabPageControl1.Controls.Add(this.grpBxAlgo);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            //this.ultraTabPageControl1.Size = new System.Drawing.Size(776, 132);
            // 
            // grpBxAlgo
            // 
            this.grpBxAlgo.Controls.Add(this.grpBxPnlAlgo);
            this.grpBxAlgo.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.grpBxAlgo.ExpandedSize = new System.Drawing.Size(776, 132);
            this.grpBxAlgo.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grpBxAlgo.Location = new System.Drawing.Point(0, 0);
            this.grpBxAlgo.Name = "grpBxAlgo";
            //this.grpBxAlgo.Size = new System.Drawing.Size(776, 132);
            this.grpBxAlgo.TabIndex = 0;
            // 
            // grpBxPnlAlgo
            // 
            this.grpBxPnlAlgo.Controls.Add(this.tblPnlAlgo);
            this.grpBxPnlAlgo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxPnlAlgo.Location = new System.Drawing.Point(3, 3);
            this.grpBxPnlAlgo.Margin = new System.Windows.Forms.Padding(0);
            this.grpBxPnlAlgo.Name = "grpBxPnlAlgo";
            //this.grpBxPnlAlgo.Size = new System.Drawing.Size(770, 126);
            this.grpBxPnlAlgo.TabIndex = 0;
            // 
            // tblPnlAlgo
            // 
            this.tblPnlAlgo.ColumnCount = 1;
            this.tblPnlAlgo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            //this.tblPnlAlgo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.8151F));
            //this.tblPnlAlgo.Controls.Add(this.strategyControl1, 0, 0);
            this.tblPnlAlgo.Controls.Add(this.algoStrategyControl, 0, 0);
            //this.tblPnlAlgo.Controls.Add(this.algoStrategyControl, 0, 0);
            this.tblPnlAlgo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlAlgo.Location = new System.Drawing.Point(0, 0);
            this.tblPnlAlgo.Margin = new System.Windows.Forms.Padding(0);
            this.tblPnlAlgo.Name = "tblPnlAlgo";
            this.tblPnlAlgo.RowCount = 1;
            this.tblPnlAlgo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //this.tblPnlAlgo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            //this.tblPnlAlgo.Size = new System.Drawing.Size(770, 126);
            this.tblPnlAlgo.TabIndex = 0;
            // 
            // algoStrategyControl
            // 
            this.algoStrategyControl.AutoScroll = true;
            this.algoStrategyControl.AutoScrollMargin = new System.Drawing.Size(5, 5);
            this.algoStrategyControl.AutoSize = true;
            this.algoStrategyControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algoStrategyControl.BackColor = System.Drawing.Color.Transparent;
            this.algoStrategyControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.algoStrategyControl.Location = new System.Drawing.Point(158, 3);
            this.algoStrategyControl.Name = "algoStrategyControl";
            //this.algoStrategyControl.Size = new System.Drawing.Size(609, 120);
            this.algoStrategyControl.TabIndex = 1;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.grpBxCommision);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(776, 132);
            // 
            // grpBxCommision
            // 
            this.grpBxCommision.Controls.Add(this.uExpandableGrpBxPanelCommision);
            this.grpBxCommision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxCommision.ExpandedSize = new System.Drawing.Size(776, 132);
            this.grpBxCommision.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grpBxCommision.Location = new System.Drawing.Point(0, 0);
            this.grpBxCommision.Name = "grpBxCommision";
            this.grpBxCommision.Size = new System.Drawing.Size(776, 132);
            this.grpBxCommision.TabIndex = 0;
            // 
            // uExpandableGrpBxPanelCommision
            // 
            this.uExpandableGrpBxPanelCommision.Controls.Add(this.tblPnlCommision);
            this.uExpandableGrpBxPanelCommision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uExpandableGrpBxPanelCommision.Location = new System.Drawing.Point(3, 3);
            this.uExpandableGrpBxPanelCommision.Name = "uExpandableGrpBxPanelCommision";
            this.uExpandableGrpBxPanelCommision.Size = new System.Drawing.Size(770, 126);
            this.uExpandableGrpBxPanelCommision.TabIndex = 0;
            // 
            // tblPnlCommision
            // 
            this.tblPnlCommision.ColumnCount = 4;
            this.tblPnlCommision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlCommision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlCommision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlCommision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlCommision.Controls.Add(this.nmrcSoftRate, 3, 1);
            this.tblPnlCommision.Controls.Add(this.cmbCommissionBasis, 0, 1);
            this.tblPnlCommision.Controls.Add(this.lblCommissionBasis, 0, 0);
            this.tblPnlCommision.Controls.Add(this.lblCommissionRate, 1, 0);
            this.tblPnlCommision.Controls.Add(this.lblSoftCommissionBasis, 2, 0);
            this.tblPnlCommision.Controls.Add(this.lblSoftRate, 3, 0);
            this.tblPnlCommision.Controls.Add(this.cmbSoftCommissionBasis, 2, 1);
            this.tblPnlCommision.Controls.Add(this.nmrcCommissionRate, 1, 1);
            this.tblPnlCommision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlCommision.Location = new System.Drawing.Point(0, 0);
            this.tblPnlCommision.Name = "tblPnlCommision";
            this.tblPnlCommision.RowCount = 2;
            this.tblPnlCommision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlCommision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlCommision.Size = new System.Drawing.Size(770, 126);
            this.tblPnlCommision.TabIndex = 0;
            // 
            // nmrcSoftRate
            // 
            this.nmrcSoftRate.Increment = 0.0001m;
            this.nmrcSoftRate.Location = new System.Drawing.Point(585, 26);
            this.nmrcSoftRate.Maximum = 999999999m;
            this.nmrcSoftRate.Minimum = 0m;
            this.nmrcSoftRate.Name = "nmrcSoftRate";
            this.nmrcSoftRate.Size = new System.Drawing.Size(188, 20);
            this.nmrcSoftRate.TabIndex = 7;
            this.nmrcSoftRate.Value = 0m;
            this.nmrcSoftRate.RemoveThousandSeperatorOnEdit = true;
            this.nmrcSoftRate.ShowCommaSeperatorOnEditing = true;
            this.nmrcSoftRate.AllowThousandSeperator = true;
            // 
            // cmbCommissionBasis
            // 
            this.cmbCommissionBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCommissionBasis.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbCommissionBasis.DropDownListWidth = -1;
            this.cmbCommissionBasis.Location = new System.Drawing.Point(3, 25);
            this.cmbCommissionBasis.Name = "cmbCommissionBasis";
            this.cmbCommissionBasis.NullText = "Select Commission Basis";
            appearance1.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbCommissionBasis.NullTextAppearance = appearance1;
            this.cmbCommissionBasis.Size = new System.Drawing.Size(186, 21);
            this.cmbCommissionBasis.TabIndex = 4;
            this.cmbCommissionBasis.ValueChanged += new System.EventHandler(this.cmbCommissionBasis_ValueChanged);
            this.cmbCommissionBasis.Leave += cmb_Leave;
            // 
            // lblCommissionBasis
            // 
            this.lblCommissionBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCommissionBasis.Location = new System.Drawing.Point(3, 3);
            this.lblCommissionBasis.Name = "lblCommissionBasis";
            this.lblCommissionBasis.Size = new System.Drawing.Size(186, 16);
            this.lblCommissionBasis.TabIndex = 0;
            this.lblCommissionBasis.Text = "Commission Basis";
            // 
            // lblCommissionRate
            // 
            this.lblCommissionRate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.lblCommissionRate.Appearance = appearance25;
            this.lblCommissionRate.Location = new System.Drawing.Point(195, 3);
            this.lblCommissionRate.Name = "lblCommissionRate";
            this.lblCommissionRate.Size = new System.Drawing.Size(186, 16);
            this.lblCommissionRate.TabIndex = 1;
            this.lblCommissionRate.Text = "Commission Rate";
            // 
            // lblSoftCommissionBasis
            // 
            this.lblSoftCommissionBasis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoftCommissionBasis.Location = new System.Drawing.Point(387, 3);
            this.lblSoftCommissionBasis.Name = "lblSoftCommissionBasis";
            this.lblSoftCommissionBasis.Size = new System.Drawing.Size(186, 16);
            this.lblSoftCommissionBasis.TabIndex = 2;
            this.lblSoftCommissionBasis.Text = "Soft Basis";
            // 
            // lblSoftRate
            // 
            this.lblSoftRate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSoftRate.Location = new System.Drawing.Point(579, 3);
            this.lblSoftRate.Name = "lblSoftRate";
            this.lblSoftRate.Size = new System.Drawing.Size(188, 16);
            this.lblSoftRate.TabIndex = 3;
            this.lblSoftRate.Text = "Soft Rate";
            // 
            // cmbSoftCommissionBasis
            // 
            this.cmbSoftCommissionBasis.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbSoftCommissionBasis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSoftCommissionBasis.DropDownListWidth = -1;
            this.cmbSoftCommissionBasis.Location = new System.Drawing.Point(387, 25);
            this.cmbSoftCommissionBasis.Name = "cmbSoftCommissionBasis";
            this.cmbSoftCommissionBasis.NullText = "Select Soft Basis";
            appearance3.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbSoftCommissionBasis.NullTextAppearance = appearance3;
            this.cmbSoftCommissionBasis.Size = new System.Drawing.Size(186, 21);
            this.cmbSoftCommissionBasis.TabIndex = 6;
            this.cmbSoftCommissionBasis.ValueChanged += new System.EventHandler(this.cmbSoftBasis_ValueChanged);
            this.cmbSoftCommissionBasis.Leave += cmb_Leave;
            // 
            // nmrcCommissionRate
            // 
            this.nmrcCommissionRate.Increment = 0.0001m;
            this.nmrcCommissionRate.Location = new System.Drawing.Point(197, 26);
            this.nmrcCommissionRate.Maximum = 999999999;
            this.nmrcCommissionRate.Minimum = 0;
            this.nmrcCommissionRate.Name = "nmrcCommissionRate";
            this.nmrcCommissionRate.Size = new System.Drawing.Size(186, 20);
            this.nmrcCommissionRate.TabIndex = 5;
            this.nmrcCommissionRate.RemoveThousandSeperatorOnEdit = true;
            this.nmrcCommissionRate.AllowThousandSeperator = true;
            this.nmrcCommissionRate.ShowCommaSeperatorOnEditing = true;
            this.nmrcCommissionRate.Value = 0;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.grpBxTradeAttribute);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(776, 132);
            // 
            // grpBxTradeAttribute
            // 
            this.grpBxTradeAttribute.Controls.Add(this.uExpandableGrpBxPanelTradeAttribute);
            this.grpBxTradeAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxTradeAttribute.ExpandedSize = new System.Drawing.Size(776, 132);
            this.grpBxTradeAttribute.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grpBxTradeAttribute.Location = new System.Drawing.Point(0, 0);
            this.grpBxTradeAttribute.Name = "grpBxTradeAttribute";
            this.grpBxTradeAttribute.Size = new System.Drawing.Size(776, 132);
            this.grpBxTradeAttribute.TabIndex = 0;
            // 
            // uExpandableGrpBxPanelTradeAttribute
            // 
            this.uExpandableGrpBxPanelTradeAttribute.AutoSize = true;
            this.uExpandableGrpBxPanelTradeAttribute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uExpandableGrpBxPanelTradeAttribute.Controls.Add(this.tblPnlTradeAttribute);
            this.uExpandableGrpBxPanelTradeAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uExpandableGrpBxPanelTradeAttribute.Location = new System.Drawing.Point(3, 3);
            this.uExpandableGrpBxPanelTradeAttribute.Name = "uExpandableGrpBxPanelTradeAttribute";
            this.uExpandableGrpBxPanelTradeAttribute.Size = new System.Drawing.Size(770, 120);
            this.uExpandableGrpBxPanelTradeAttribute.TabIndex = 0;
            // 
            // tblPnlTradeAttribute
            // 
            this.tblPnlTradeAttribute.AutoSize = true;
            this.tblPnlTradeAttribute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPnlTradeAttribute.ColumnCount = 6;
            this.tblPnlTradeAttribute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTradeAttribute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTradeAttribute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTradeAttribute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTradeAttribute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTradeAttribute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTradeAttribute.Controls.Add(this.cmbTradeAttribute6, 5, 1);
            this.tblPnlTradeAttribute.Controls.Add(this.cmbTradeAttribute5, 4, 1);
            this.tblPnlTradeAttribute.Controls.Add(this.cmbTradeAttribute4, 3, 1);
            this.tblPnlTradeAttribute.Controls.Add(this.cmbTradeAttribute3, 2, 1);
            this.tblPnlTradeAttribute.Controls.Add(this.cmbTradeAttribute2, 1, 1);
            this.tblPnlTradeAttribute.Controls.Add(this.lblTradeAttribute6, 5, 0);
            this.tblPnlTradeAttribute.Controls.Add(this.lblTradeAttribute5, 4, 0);
            this.tblPnlTradeAttribute.Controls.Add(this.lblTradeAttribute4, 3, 0);
            this.tblPnlTradeAttribute.Controls.Add(this.lblTradeAttribute3, 2, 0);
            this.tblPnlTradeAttribute.Controls.Add(this.lblTradeAttribute2, 1, 0);
            this.tblPnlTradeAttribute.Controls.Add(this.lblTradeAttribute1, 0, 0);
            this.tblPnlTradeAttribute.Controls.Add(this.cmbTradeAttribute1, 0, 1);
            this.tblPnlTradeAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlTradeAttribute.Location = new System.Drawing.Point(0, 0);
            this.tblPnlTradeAttribute.Name = "tblPnlTradeAttribute";
            this.tblPnlTradeAttribute.RowCount = 2;
            this.tblPnlTradeAttribute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTradeAttribute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTradeAttribute.Size = new System.Drawing.Size(770, 126);
            this.tblPnlTradeAttribute.TabIndex = 0;
            // 
            // cmbTradeAttribute6
            // 
            this.cmbTradeAttribute6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradeAttribute6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradeAttribute6.DropDownListWidth = -1;
            this.cmbTradeAttribute6.Location = new System.Drawing.Point(643, 24);
            this.cmbTradeAttribute6.Name = "cmbTradeAttribute6";
            this.cmbTradeAttribute6.NullText = "Select Attribute";
            appearance4.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradeAttribute6.NullTextAppearance = appearance4;
            this.cmbTradeAttribute6.Size = new System.Drawing.Size(124, 21);
            this.cmbTradeAttribute6.TabIndex = 11;
            this.cmbTradeAttribute6.ValueChanged += cmb_ValueChanged;
            // 
            // cmbTradeAttribute5
            // 
            this.cmbTradeAttribute5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradeAttribute5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradeAttribute5.DropDownListWidth = -1;
            this.cmbTradeAttribute5.Location = new System.Drawing.Point(515, 24);
            this.cmbTradeAttribute5.Name = "cmbTradeAttribute5";
            this.cmbTradeAttribute5.NullText = "Select Attribute";
            appearance5.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradeAttribute5.NullTextAppearance = appearance5;
            this.cmbTradeAttribute5.Size = new System.Drawing.Size(122, 21);
            this.cmbTradeAttribute5.TabIndex = 10;
            this.cmbTradeAttribute5.ValueChanged += cmb_ValueChanged;
            // 
            // cmbTradeAttribute4
            // 
            this.cmbTradeAttribute4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradeAttribute4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradeAttribute4.DropDownListWidth = -1;
            this.cmbTradeAttribute4.Location = new System.Drawing.Point(387, 24);
            this.cmbTradeAttribute4.Name = "cmbTradeAttribute4";
            this.cmbTradeAttribute4.NullText = "Select Attribute";
            appearance6.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradeAttribute4.NullTextAppearance = appearance6;
            this.cmbTradeAttribute4.Size = new System.Drawing.Size(122, 21);
            this.cmbTradeAttribute4.TabIndex = 9;
            this.cmbTradeAttribute4.ValueChanged += cmb_ValueChanged;
            // 
            // cmbTradeAttribute3
            // 
            this.cmbTradeAttribute3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradeAttribute3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradeAttribute3.DropDownListWidth = -1;
            this.cmbTradeAttribute3.Location = new System.Drawing.Point(259, 24);
            this.cmbTradeAttribute3.Name = "cmbTradeAttribute3";
            this.cmbTradeAttribute3.NullText = "Select Attribute";
            appearance7.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradeAttribute3.NullTextAppearance = appearance7;
            this.cmbTradeAttribute3.Size = new System.Drawing.Size(122, 21);
            this.cmbTradeAttribute3.TabIndex = 8;
            this.cmbTradeAttribute3.ValueChanged += cmb_ValueChanged;
            // 
            // cmbTradeAttribute2
            // 
            this.cmbTradeAttribute2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradeAttribute2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradeAttribute2.DropDownListWidth = -1;
            this.cmbTradeAttribute2.Location = new System.Drawing.Point(131, 24);
            this.cmbTradeAttribute2.Name = "cmbTradeAttribute2";
            this.cmbTradeAttribute2.NullText = "Select Attribute";
            appearance8.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradeAttribute2.NullTextAppearance = appearance8;
            this.cmbTradeAttribute2.Size = new System.Drawing.Size(122, 21);
            this.cmbTradeAttribute2.TabIndex = 7;
            this.cmbTradeAttribute2.ValueChanged += cmb_ValueChanged;
            // 
            // lblTradeAttribute6
            // 
            this.lblTradeAttribute6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeAttribute6.Location = new System.Drawing.Point(643, 3);
            this.lblTradeAttribute6.Name = "lblTradeAttribute6";
            this.lblTradeAttribute6.Size = new System.Drawing.Size(124, 15);
            this.lblTradeAttribute6.TabIndex = 5;
            this.lblTradeAttribute6.Text = "Trade Attribute6";
            // 
            // lblTradeAttribute5
            // 
            this.lblTradeAttribute5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeAttribute5.Location = new System.Drawing.Point(515, 3);
            this.lblTradeAttribute5.Name = "lblTradeAttribute5";
            this.lblTradeAttribute5.Size = new System.Drawing.Size(122, 15);
            this.lblTradeAttribute5.TabIndex = 4;
            this.lblTradeAttribute5.Text = "Trade Attribute5";
            // 
            // lblTradeAttribute4
            // 
            this.lblTradeAttribute4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeAttribute4.Location = new System.Drawing.Point(387, 3);
            this.lblTradeAttribute4.Name = "lblTradeAttribute4";
            this.lblTradeAttribute4.Size = new System.Drawing.Size(122, 15);
            this.lblTradeAttribute4.TabIndex = 3;
            this.lblTradeAttribute4.Text = "Trade Attribute4";
            // 
            // lblTradeAttribute3
            // 
            this.lblTradeAttribute3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeAttribute3.Location = new System.Drawing.Point(259, 3);
            this.lblTradeAttribute3.Name = "lblTradeAttribute3";
            this.lblTradeAttribute3.Size = new System.Drawing.Size(122, 15);
            this.lblTradeAttribute3.TabIndex = 2;
            this.lblTradeAttribute3.Text = "Trade Attribute3";
            // 
            // lblTradeAttribute2
            // 
            this.lblTradeAttribute2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeAttribute2.Location = new System.Drawing.Point(131, 3);
            this.lblTradeAttribute2.Name = "lblTradeAttribute2";
            this.lblTradeAttribute2.Size = new System.Drawing.Size(122, 15);
            this.lblTradeAttribute2.TabIndex = 1;
            this.lblTradeAttribute2.Text = "Trade Attribute2";
            // 
            // lblTradeAttribute1
            // 
            this.lblTradeAttribute1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeAttribute1.Location = new System.Drawing.Point(3, 3);
            this.lblTradeAttribute1.Name = "lblTradeAttribute1";
            this.lblTradeAttribute1.Size = new System.Drawing.Size(122, 15);
            this.lblTradeAttribute1.TabIndex = 0;
            this.lblTradeAttribute1.Text = "Trade Attribute 1";
            // 
            // cmbTradeAttribute1
            // 
            this.cmbTradeAttribute1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradeAttribute1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradeAttribute1.DropDownListWidth = -1;
            this.cmbTradeAttribute1.Location = new System.Drawing.Point(3, 24);
            this.cmbTradeAttribute1.Name = "cmbTradeAttribute1";
            this.cmbTradeAttribute1.NullText = "Select Attribute";
            appearance9.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradeAttribute1.NullTextAppearance = appearance9;
            this.cmbTradeAttribute1.Size = new System.Drawing.Size(122, 21);
            this.cmbTradeAttribute1.TabIndex = 6;
            this.cmbTradeAttribute1.ValueChanged += cmb_ValueChanged;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.grbBxOther);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(776, 132);
            // 
            // grbBxOther
            // 
            this.grbBxOther.Controls.Add(this.uExpandableGrpBxPanelOther);
            this.grbBxOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbBxOther.ExpandedSize = new System.Drawing.Size(776, 132);
            this.grbBxOther.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grbBxOther.Location = new System.Drawing.Point(0, 0);
            this.grbBxOther.Name = "grbBxOther";
            this.grbBxOther.Size = new System.Drawing.Size(776, 132);
            this.grbBxOther.TabIndex = 0;
            // 
            // uExpandableGrpBxPanelOther
            // 
            this.uExpandableGrpBxPanelOther.Controls.Add(this.tblPnlOthers);
            this.uExpandableGrpBxPanelOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uExpandableGrpBxPanelOther.Location = new System.Drawing.Point(3, 3);
            this.uExpandableGrpBxPanelOther.Name = "uExpandableGrpBxPanelOther";
            this.uExpandableGrpBxPanelOther.Size = new System.Drawing.Size(770, 126);
            this.uExpandableGrpBxPanelOther.TabIndex = 0;
            // 
            // tblPnlOthers
            // 
            this.tblPnlOthers.AutoSize = true;
            this.tblPnlOthers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPnlOthers.ColumnCount = 4;
            this.tblPnlOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlOthers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblPnlOthers.Controls.Add(this.cmbTradingAccount, 2, 1);
            this.tblPnlOthers.Controls.Add(this.cmbHandlingInstructions, 1, 1);
            this.tblPnlOthers.Controls.Add(this.cmbExecutionInstructions, 0, 1);

            this.tblPnlOthers.Controls.Add(this.lblExecutionInstructions, 0, 0);
            this.tblPnlOthers.Controls.Add(this.lblHandlingInstructions, 1, 0);
            this.tblPnlOthers.Controls.Add(this.lblTradingAccount, 2, 0);
            this.tblPnlOthers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlOthers.Location = new System.Drawing.Point(0, 0);
            this.tblPnlOthers.Name = "tblPnlOthers";
            this.tblPnlOthers.RowCount = 2;
            this.tblPnlOthers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlOthers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlOthers.Size = new System.Drawing.Size(770, 126);
            this.tblPnlOthers.TabIndex = 0;
            // 
            // cmbTradingAccount
            // 
            this.cmbTradingAccount.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTradingAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTradingAccount.DropDownListWidth = -1;
            this.cmbTradingAccount.Location = new System.Drawing.Point(579, 25);
            this.cmbTradingAccount.Name = "cmbTradingAccount";
            this.cmbTradingAccount.NullText = "Select Trading Account";
            appearance10.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTradingAccount.NullTextAppearance = appearance10;
            this.cmbTradingAccount.Size = new System.Drawing.Size(188, 21);
            this.cmbTradingAccount.TabIndex = 7;
            this.cmbTradingAccount.ValueChanged += cmb_ValueChanged;
            this.cmbTradingAccount.Leave += cmb_Leave;
            // 
            // cmbHandlingInstructions
            // 
            this.cmbHandlingInstructions.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbHandlingInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbHandlingInstructions.DropDownListWidth = -1;
            this.cmbHandlingInstructions.Location = new System.Drawing.Point(387, 25);
            this.cmbHandlingInstructions.Name = "cmbHandlingInstructions";
            this.cmbHandlingInstructions.NullText = "Select Handling Inst.";
            appearance11.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbHandlingInstructions.NullTextAppearance = appearance11;
            this.cmbHandlingInstructions.Size = new System.Drawing.Size(186, 21);
            this.cmbHandlingInstructions.TabIndex = 6;
            this.cmbHandlingInstructions.ValueChanged += cmb_ValueChanged;
            this.cmbHandlingInstructions.Leave += cmb_Leave;
            // 
            // cmbExecutionInstructions
            // 
            this.cmbExecutionInstructions.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbExecutionInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbExecutionInstructions.DropDownListWidth = -1;
            this.cmbExecutionInstructions.Location = new System.Drawing.Point(195, 25);
            this.cmbExecutionInstructions.Name = "cmbExecutionInstructions";
            this.cmbExecutionInstructions.NullText = "Select Execution Inst.";
            appearance12.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbExecutionInstructions.NullTextAppearance = appearance12;
            this.cmbExecutionInstructions.Size = new System.Drawing.Size(186, 21);
            this.cmbExecutionInstructions.TabIndex = 5;
            this.cmbExecutionInstructions.ValueChanged += cmb_ValueChanged;
            this.cmbExecutionInstructions.Leave += cmb_Leave;
            // 
            // lblVenue
            // 
            this.lblVenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVenue.Location = new System.Drawing.Point(3, 3);
            this.lblVenue.Name = "lblVenue";
            this.lblVenue.Size = new System.Drawing.Size(186, 16);
            this.lblVenue.TabIndex = 0;
            this.lblVenue.Text = "Venue";
            // 
            // lblExecutionInstructions
            // 
            this.lblExecutionInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExecutionInstructions.Location = new System.Drawing.Point(195, 3);
            this.lblExecutionInstructions.Name = "lblExecutionInstructions";
            this.lblExecutionInstructions.Size = new System.Drawing.Size(186, 16);
            this.lblExecutionInstructions.TabIndex = 1;
            this.lblExecutionInstructions.Text = "Execution Instructions";
            // 
            // lblHandlingInstructions
            // 
            this.lblHandlingInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHandlingInstructions.Location = new System.Drawing.Point(387, 3);
            this.lblHandlingInstructions.Name = "lblHandlingInstructions";
            this.lblHandlingInstructions.Size = new System.Drawing.Size(186, 16);
            this.lblHandlingInstructions.TabIndex = 2;
            this.lblHandlingInstructions.Text = "Handling Instructions";
            // 
            // cmbVenue
            // 
            this.cmbVenue.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbVenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVenue.DropDownListWidth = -1;
            this.cmbVenue.Location = new System.Drawing.Point(3, 25);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.NullText = "Select Venue";
            appearance13.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbVenue.NullTextAppearance = appearance13;
            this.cmbVenue.Size = new System.Drawing.Size(186, 21);
            this.cmbVenue.TabIndex = 11;
            this.cmbVenue.ValueChanged += new System.EventHandler(this.cmbVenue_ValueChanged);
            this.cmbVenue.Leave += cmb_Leave;
            // 
            // lblTradingAccount
            // 
            this.lblTradingAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradingAccount.Location = new System.Drawing.Point(579, 3);
            this.lblTradingAccount.Name = "lblTradingAccount";
            this.lblTradingAccount.Size = new System.Drawing.Size(188, 16);
            this.lblTradingAccount.TabIndex = 3;
            this.lblTradingAccount.Text = "Trader";
            // 
            // ultraTabPageControl9
            // 
            this.ultraTabPageControl9.Controls.Add(this.grpBxSwap);
            this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl9.Name = "ultraTabPageControl9";
            this.ultraTabPageControl9.Size = new System.Drawing.Size(776, 132);
            // 
            // grpBxSwap
            // 
            this.grpBxSwap.Controls.Add(this.grpBxPnlSwap);
            this.grpBxSwap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxSwap.ExpandedSize = new System.Drawing.Size(776, 132);
            this.grpBxSwap.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grpBxSwap.Location = new System.Drawing.Point(0, 0);
            this.grpBxSwap.Margin = new System.Windows.Forms.Padding(0);
            this.grpBxSwap.Name = "grpBxSwap";
            this.grpBxSwap.Size = new System.Drawing.Size(776, 132);
            this.grpBxSwap.TabIndex = 0;
            // 
            // grpBxPnlSwap
            // 


            this.grpBxPnlSwap.Controls.Add(this.ctrlSwapParameters1);
            this.grpBxPnlSwap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxPnlSwap.Location = new System.Drawing.Point(3, 3);
            this.grpBxPnlSwap.Margin = new System.Windows.Forms.Padding(0);
            this.grpBxPnlSwap.Name = "grpBxPnlSwap";
            this.grpBxPnlSwap.Size = new System.Drawing.Size(770, 126);
            this.grpBxPnlSwap.TabIndex = 0;
            // 
            // ctrlSwapParameters1
            // 
            this.ctrlSwapParameters1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSwapParameters1.IsPreTradeSwap = false;
            this.ctrlSwapParameters1.Location = new System.Drawing.Point(0, 0);
            this.ctrlSwapParameters1.Name = "ctrlSwapParameters1";
            this.ctrlSwapParameters1.Size = new System.Drawing.Size(770, 126);
            this.ctrlSwapParameters1.TabIndex = 0;
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Controls.Add(this.grbBxSettlementFields);
            this.ultraTabPageControl10.Controls.Add(this.ultraExpandableGroupBox1);
            this.ultraTabPageControl10.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(776, 132);
            // 
            // grbBxSettlementFields
            // 
            this.grbBxSettlementFields.Controls.Add(this.grpBxPnlSettlementFields);
            this.grbBxSettlementFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbBxSettlementFields.ExpandedSize = new System.Drawing.Size(776, 132);
            this.grbBxSettlementFields.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grbBxSettlementFields.Location = new System.Drawing.Point(0, 0);
            this.grbBxSettlementFields.Margin = new System.Windows.Forms.Padding(0);
            this.grbBxSettlementFields.Name = "grbBxSettlementFields";
            this.grbBxSettlementFields.Size = new System.Drawing.Size(776, 132);
            this.grbBxSettlementFields.TabIndex = 0;
            // 
            // grpBxPnlSettlementFields
            // 
            this.grpBxPnlSettlementFields.Controls.Add(this.tblPnlSettlementFields);
            this.grpBxPnlSettlementFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxPnlSettlementFields.Location = new System.Drawing.Point(3, 3);
            this.grpBxPnlSettlementFields.Margin = new System.Windows.Forms.Padding(0);
            this.grpBxPnlSettlementFields.Name = "grpBxPnlSettlementFields";
            this.grpBxPnlSettlementFields.Size = new System.Drawing.Size(770, 126);
            this.grpBxPnlSettlementFields.TabIndex = 0;
            // 
            // tblPnlSettlementFields
            // 
            this.tblPnlSettlementFields.ColumnCount = 5;
            this.tblPnlSettlementFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPnlSettlementFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPnlSettlementFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPnlSettlementFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPnlSettlementFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPnlSettlementFields.Controls.Add(this.lblSettlementCurrencyPrice, 1, 0);
            this.tblPnlSettlementFields.Controls.Add(this.cmbSettlementCurrency, 0, 1);
            this.tblPnlSettlementFields.Controls.Add(this.lblSettlementCurrency, 0, 0);
            this.tblPnlSettlementFields.Controls.Add(this.nmrcSettlementPrice, 1, 1);
            this.tblPnlSettlementFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlSettlementFields.Location = new System.Drawing.Point(0, 0);
            this.tblPnlSettlementFields.Margin = new System.Windows.Forms.Padding(0);
            this.tblPnlSettlementFields.Name = "tblPnlSettlementFields";
            this.tblPnlSettlementFields.RowCount = 2;
            this.tblPnlSettlementFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.03279F));
            this.tblPnlSettlementFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.96722F));
            this.tblPnlSettlementFields.Size = new System.Drawing.Size(770, 126);
            this.tblPnlSettlementFields.TabIndex = 0;
            // 
            // lblSettlementCurrencyPrice
            // 
            this.lblSettlementCurrencyPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSettlementCurrencyPrice.Location = new System.Drawing.Point(157, 3);
            this.lblSettlementCurrencyPrice.Name = "lblSettlementCurrencyPrice";
            this.lblSettlementCurrencyPrice.Size = new System.Drawing.Size(148, 16);
            this.lblSettlementCurrencyPrice.TabIndex = 2;
            this.lblSettlementCurrencyPrice.Text = "Settlement Price";
            // 
            // cmbSettlementCurrency
            // 
            this.cmbSettlementCurrency.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbSettlementCurrency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSettlementCurrency.DropDownListWidth = -1;
            this.cmbSettlementCurrency.Location = new System.Drawing.Point(3, 25);
            this.cmbSettlementCurrency.Name = "cmbSettlementCurrency";
            this.cmbSettlementCurrency.NullText = "Select Currency";
            appearance14.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbSettlementCurrency.NullTextAppearance = appearance14;
            this.cmbSettlementCurrency.Size = new System.Drawing.Size(148, 21);
            this.cmbSettlementCurrency.TabIndex = 4;
            this.cmbSettlementCurrency.ValueChanged += new System.EventHandler(this.cmbSettlementCurrency_ValueChanged);
            this.cmbSettlementCurrency.Leave += cmb_Leave;
            // 
            // lblSettlementCurrency
            // 
            this.lblSettlementCurrency.Location = new System.Drawing.Point(3, 3);
            this.lblSettlementCurrency.Name = "lblSettlementCurrency";
            this.lblSettlementCurrency.Size = new System.Drawing.Size(129, 14);
            this.lblSettlementCurrency.TabIndex = 0;
            this.lblSettlementCurrency.Text = "Settlement Currency";
            // 
            // nmrcSettlementPrice
            // 
            this.nmrcSettlementPrice.Increment = 0.01m;
            this.nmrcSettlementPrice.Location = new System.Drawing.Point(313, 26);
            this.nmrcSettlementPrice.Maximum = 999999999m;
            this.nmrcSettlementPrice.Minimum = 0m;
            this.nmrcSettlementPrice.Name = "nmrcSettlementPrice";
            this.nmrcSettlementPrice.Size = new System.Drawing.Size(148, 20);
            this.nmrcSettlementPrice.TabIndex = 6;
            this.nmrcSettlementPrice.Value = 0m;
            this.nmrcSettlementPrice.RemoveThousandSeperatorOnEdit = true;
            this.nmrcSettlementPrice.AllowThousandSeperator = true;
            this.nmrcSettlementPrice.ShowCommaSeperatorOnEditing = true;
            this.nmrcSettlementPrice.ValueChanged += nmrcSettlementPrice_ValueChanged;
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(0, 0);
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(261, 35);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox1.TabIndex = 0;
            this.ultraExpandableGroupBox1.Text = "ultraExpandableGroupBox1";
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(194, 163);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // tblPnlMain
            // 

            this.tblPnlMain.BackColor = System.Drawing.Color.Transparent;
            this.tblPnlMain.ColumnCount = 1;
            this.tblPnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlMain.Controls.Add(this.tblPnlAllControls, 0, 0);
            this.tblPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlMain.Location = new System.Drawing.Point(8, 32);
            this.tblPnlMain.Name = "tblPnlMain";
            this.tblPnlMain.RowCount = 1;
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlMain.Size = new System.Drawing.Size(1003, 444);
            this.tblPnlMain.TabIndex = 0;

            // 
            // tblPnlAllControls
            // 
            this.tblPnlAllControls.AutoSize = true;
            this.tblPnlAllControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPnlAllControls.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tblPnlAllControls.ColumnCount = 2;
            this.tblPnlMain.SetColumnSpan(this.tblPnlAllControls, 2);
            this.tblPnlAllControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.0041F));
            this.tblPnlAllControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.9959F));
            this.tblPnlAllControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblPnlAllControls.Controls.Add(this.tblPnlSymbolControl, 0, 0);
            this.tblPnlAllControls.Controls.Add(this.tblPnlTTMainControls, 1, 0);
            this.tblPnlAllControls.Location = new System.Drawing.Point(0, 0);
            this.tblPnlAllControls.Margin = new System.Windows.Forms.Padding(0);
            this.tblPnlAllControls.Name = "tblPnlAllControls";
            this.tblPnlAllControls.RowCount = 1;
            this.tblPnlAllControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            //this.tblPnlAllControls.Size = new System.Drawing.Size(1003, 444);
            this.tblPnlAllControls.TabIndex = 0;
            // 
            // tblPnlSymbolControl
            // 
            this.tblPnlSymbolControl.AutoSize = true;
            this.tblPnlSymbolControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPnlSymbolControl.ColumnCount = 1;
            this.tblPnlSymbolControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
            {
                this.tblPnlSymbolControl.AutoSize = true;
                this.tblPnlSymbolControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tblPnlSymbolControl.ColumnCount = 1;
                this.tblPnlSymbolControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));

                this.tblPnlSymbolControl.Controls.Add(this.lblFunds, 0, 0);
                this.tblPnlSymbolControl.Controls.Add(this.cmbFunds, 0, 1);
                //this.tblPnlSymbolControl.Controls.Add(this.lblSymbol, 0, 2);
                this.tblPnlSymbolControl.Controls.Add(this.pranaSymbolCtrl, 0, 3);
                //this.tblPnlSymbolControl.Controls.Add(this.btnSymbolLookup, 0, 3);
                this.tblPnlSymbolControl.Controls.Add(this.ultraPanelSymbolLookup, 0, 3);
                this.tblPnlSymbolControl.Controls.Add(this.ultraPanel3, 0, 4);
                this.tblPnlSymbolControl.Controls.Add(this.grbBoxOptionControl, 0, 5);
                this.tblPnlSymbolControl.Controls.Add(this.lblHiddenStrategyAdjust, 0, 6);
                this.tblPnlSymbolControl.Controls.Add(this.grbBoxStrategyControl, 0, 7);
                this.tblPnlSymbolControl.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tblPnlSymbolControl.Location = new System.Drawing.Point(6, 41);
                this.tblPnlSymbolControl.Name = "tblPnlSymbolControl";
                this.tblPnlSymbolControl.RowCount = 7;
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                //this.tblPnlSymbolControl.Size = new System.Drawing.Size(202, 390);
                this.tblPnlSymbolControl.TabIndex = 0;
            }
            else
            {
                this.tblPnlSymbolControl.AutoSize = true;
                this.tblPnlSymbolControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tblPnlSymbolControl.ColumnCount = 1;
                this.tblPnlSymbolControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                this.tblPnlSymbolControl.Controls.Add(this.ultraPanelSymbolLookup, 0, 0);
                this.tblPnlSymbolControl.Controls.Add(this.pranaSymbolCtrl, 0, 1);
                // this.tblPnlSymbolControl.Controls.Add(this.ultraLegend2, 0, 5);
                this.tblPnlSymbolControl.Controls.Add(this.ultraPanel3, 0, 2);
                this.tblPnlSymbolControl.Controls.Add(this.grbBoxOptionControl, 0, 3);
                this.tblPnlSymbolControl.Controls.Add(this.lblHiddenStrategyAdjust, 0, 4);
                this.tblPnlSymbolControl.Controls.Add(this.grbBoxStrategyControl, 0, 5);
                this.tblPnlSymbolControl.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tblPnlSymbolControl.Location = new System.Drawing.Point(6, 41);
                this.tblPnlSymbolControl.Name = "tblPnlSymbolControl";
                this.tblPnlSymbolControl.RowCount = 5;
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                this.tblPnlSymbolControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                //this.tblPnlSymbolControl.Size = new System.Drawing.Size(202, 390);
                this.tblPnlSymbolControl.TabIndex = 0;
            }

            // 
            // lblSymbol
            //
            appearance28.BorderColor = System.Drawing.Color.Black;
            appearance28.TextVAlignAsString = "Bottom";
            this.lblSymbol.Appearance = appearance28;
            this.lblSymbol.Location = new System.Drawing.Point(0, 7);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Padding = new System.Drawing.Size(0, 0);
            this.lblSymbol.Size = new System.Drawing.Size(100, 24);
            this.lblSymbol.TabIndex = 1;
            this.lblSymbol.Text = "Symbol";
            // 
            // pranaSymbolCtrl
            // 
            this.pranaSymbolCtrl.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.pranaSymbolCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pranaSymbolCtrl.Location = new System.Drawing.Point(3, 32);
            this.pranaSymbolCtrl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pranaSymbolCtrl.MaxLength = 32767;
            this.pranaSymbolCtrl.Name = "pranaSymbolCtrl";
            this.pranaSymbolCtrl.PrevSymbolEntered = "";
            this.pranaSymbolCtrl.Size = new System.Drawing.Size(196, 24);
            this.pranaSymbolCtrl.TabIndex = 2;

            // 
            // lblClient
            // 
            this.lblFunds.Appearance = appearance28;
            this.lblFunds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFunds.Location = new System.Drawing.Point(5, 3);
            this.lblFunds.Name = "lblFunds";
            this.lblFunds.Padding = new System.Drawing.Size(0, 0);
            this.lblFunds.Size = new System.Drawing.Size(196, 24);
            this.lblFunds.TabIndex = 1;
            this.lblFunds.Text = "Client";
            this.lblFunds.Visible = false;



            // 
            // cmbFunds
            // 
            this.cmbFunds.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbFunds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFunds.DropDownListWidth = -1;
            this.cmbFunds.Location = new System.Drawing.Point(3, 32);
            this.cmbFunds.Margin = new System.Windows.Forms.Padding(3, 5, 3, 2);
            this.cmbFunds.MaxLength = 32767;
            this.cmbFunds.Name = "cmbFunds";
            this.cmbFunds.Size = new System.Drawing.Size(196, 24);
            this.cmbFunds.TabIndex = 1;
            this.cmbFunds.Visible = false;
            this.cmbFunds.ValueChanged += cmbFunds_ValueChanged;
            this.cmbFunds.Leave += cmb_Leave;

            // 
            // ultraLegend2
            // 
            this.ultraLegend2.Location = new System.Drawing.Point(3, 206);
            this.ultraLegend2.Name = "ultraLegend2";
            this.ultraLegend2.Size = new System.Drawing.Size(3, 3);
            this.ultraLegend2.TabIndex = 3;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 

            this.ultraPanel3.ClientArea.Controls.Add(this.chkBoxOption);
            this.ultraPanel3.ClientArea.Controls.Add(this.chkBoxEquitySwap);
            this.ultraPanel3.ClientArea.Controls.Add(this.chkBoxCFD);
            this.ultraPanel3.ClientArea.Controls.Add(this.chkBoxConvertiableBond);
            this.ultraPanel3.ClientArea.Controls.Add(this.chkBoxSwap);
            this.ultraPanel3.ClientArea.Controls.Add(this.btnOTCControl);
            this.ultraPanel3.Location = new System.Drawing.Point(3, 61);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.AutoSize = true;// new System.Drawing.Size(296, 233);
            this.ultraPanel3.TabIndex = 4;
            this.ultraPanel3.TabStop = false;
            // 
            // ultraPanelSymbolLookup
            // 
            // 
            // ultraPanelSymbolLookup.ClientArea
            // 
            this.ultraPanelSymbolLookup.ClientArea.Controls.Add(this.lblSymbol);
            this.ultraPanelSymbolLookup.ClientArea.Controls.Add(this.btnSymbolLookup);
            this.ultraPanelSymbolLookup.Location = new System.Drawing.Point(3, 4);
            this.ultraPanelSymbolLookup.Name = "ultraPanel3";
            this.ultraPanelSymbolLookup.AutoSize = true;
            this.ultraPanelSymbolLookup.TabIndex = 1;
            this.ultraPanelSymbolLookup.TabStop = false;
            // 
            // chkBoxOption
            // 
            this.chkBoxOption.Location = new System.Drawing.Point(3, 4);
            this.chkBoxOption.Margin = new System.Windows.Forms.Padding(0);
            this.chkBoxOption.Name = "chkBoxOption";
            this.chkBoxOption.Size = new System.Drawing.Size(55, 24);
            this.chkBoxOption.TabIndex = 0;
            this.chkBoxOption.Text = "Option";
            this.chkBoxOption.CheckedChanged += new System.EventHandler(this.chkBoxOption_CheckedChanged);

            this.chkBoxEquitySwap.Location = new System.Drawing.Point(3, 25);
            this.chkBoxEquitySwap.Margin = new System.Windows.Forms.Padding(0);
            this.chkBoxEquitySwap.Name = "chkBoxEquitySwap";
            this.chkBoxEquitySwap.Size = new System.Drawing.Size(155, 24);
            this.chkBoxEquitySwap.TabIndex = 1;
            this.chkBoxEquitySwap.Text = "Equity Swap";
            this.chkBoxEquitySwap.CheckedChanged += new System.EventHandler(this.chkBoxEquitySwap_CheckedChanged);

            this.chkBoxCFD.Location = new System.Drawing.Point(3, 46);
            this.chkBoxCFD.Margin = new System.Windows.Forms.Padding(0);
            this.chkBoxCFD.Name = "chkBoxCFD";
            this.chkBoxCFD.Size = new System.Drawing.Size(55, 24);
            this.chkBoxCFD.TabIndex = 2;
            this.chkBoxCFD.Text = "CFD";
            this.chkBoxCFD.Visible = true;
            this.chkBoxCFD.CheckedChanged += new System.EventHandler(this.chkBoxCFD_CheckedChanged);



            this.chkBoxConvertiableBond.Location = new System.Drawing.Point(3, 67);
            this.chkBoxConvertiableBond.Margin = new System.Windows.Forms.Padding(0);
            this.chkBoxConvertiableBond.Name = "chkBoxConvertiableBond";
            this.chkBoxConvertiableBond.Size = new System.Drawing.Size(120, 24);
            this.chkBoxConvertiableBond.TabIndex = 3;
            this.chkBoxConvertiableBond.Text = "Convertiable Bond";
            this.chkBoxConvertiableBond.Visible = true;
            this.chkBoxConvertiableBond.CheckedChanged += new System.EventHandler(this.chkBoxConvertiableBond_CheckedChanged);



            this.btnOTCControl.BackColorInternal = System.Drawing.Color.Transparent;
            this.btnOTCControl.Location = new System.Drawing.Point(3, 94);
            this.btnOTCControl.Name = "btnOTCControl";
            this.btnOTCControl.Size = new System.Drawing.Size(119, 30);
            this.btnOTCControl.Visible = false;
            this.btnOTCControl.TabIndex = 4;
            this.btnOTCControl.Text = "SWAP Details";
            this.btnOTCControl.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnOTCControl.Click += new System.EventHandler(this.btnOTCControl_Click);

            // 
            // chkBoxSwap
            // 
            this.chkBoxSwap.Location = new System.Drawing.Point(70, 4);
            this.chkBoxSwap.Margin = new System.Windows.Forms.Padding(0);
            this.chkBoxSwap.Name = "chkBoxSwap";
            this.chkBoxSwap.Size = new System.Drawing.Size(95, 24);
            this.chkBoxSwap.TabIndex = 1;
            this.chkBoxSwap.Text = "Book as Swap";
            this.chkBoxSwap.Visible = false;
            this.chkBoxSwap.CheckedChanged += new System.EventHandler(this.chkBoxSwap_CheckedChanged);

            // grbBoxOptionControl
            // 
            this.grbBoxOptionControl.Controls.Add(this.uExoandalPnlOptionControl);
            this.grbBoxOptionControl.Enabled = false;
            this.grbBoxOptionControl.Expanded = false;
            this.grbBoxOptionControl.ExpandedSize = new System.Drawing.Size(200, 96);
            this.grbBoxOptionControl.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grbBoxOptionControl.Location = new System.Drawing.Point(0, 100);
            this.grbBoxOptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.grbBoxOptionControl.Name = "grbBoxOptionControl";
            this.grbBoxOptionControl.Size = new System.Drawing.Size(200, 2);
            this.grbBoxOptionControl.TabIndex = 5;
            this.grbBoxOptionControl.TabStop = false;
            this.grbBoxOptionControl.ExpandedStateChanged += new System.EventHandler(this.grbBoxOptionControl_ExpandedStateChanged);
            // 
            // grbBoxStrategyControl
            // 
            this.grbBoxStrategyControl.Controls.Add(this.uExoandalStrategyControl);
            this.grbBoxStrategyControl.Enabled = false;
            this.grbBoxStrategyControl.Expanded = false;
            this.grbBoxStrategyControl.ExpandedSize = new System.Drawing.Size(200, 96);
            this.grbBoxStrategyControl.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.None;
            this.grbBoxStrategyControl.Location = new System.Drawing.Point(0, 150);
            this.grbBoxStrategyControl.Margin = new System.Windows.Forms.Padding(0);
            this.grbBoxStrategyControl.Name = "grbBoxStrategyControl";
            this.grbBoxStrategyControl.Size = new System.Drawing.Size(200, 2);
            this.grbBoxStrategyControl.TabIndex = 6;
            this.grbBoxStrategyControl.TabStop = false;
            // 
            // lblClient
            // 
            this.lblHiddenStrategyAdjust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHiddenStrategyAdjust.Location = new System.Drawing.Point(0, 100);
            this.lblHiddenStrategyAdjust.Name = "lblHiddenStrategyAdjust";
            this.lblHiddenStrategyAdjust.Size = new System.Drawing.Size(200, 45);
            this.lblHiddenStrategyAdjust.TabIndex = 1;
            this.lblHiddenStrategyAdjust.Text = "";
            this.lblHiddenStrategyAdjust.Visible = true;



            //this.grbBoxStrategyControl.ExpandedStateChanged += new System.EventHandler(this.grbBoxOptionControl_ExpandedStateChanged);
            // 
            // uExoandalPnlOptionControl
            // 
            this.uExoandalPnlOptionControl.Controls.Add(this.pranaOptionCtrl);
            this.uExoandalPnlOptionControl.Location = new System.Drawing.Point(-10000, -10000);
            this.uExoandalPnlOptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.uExoandalPnlOptionControl.Name = "uExoandalPnlOptionControl";
            this.uExoandalPnlOptionControl.Size = new System.Drawing.Size(194, 90);
            this.uExoandalPnlOptionControl.TabIndex = 0;
            this.uExoandalPnlOptionControl.Visible = false;
            // 
            // uExoandalStrategyControl
            // 
            this.uExoandalStrategyControl.Controls.Add(this.strategyControl1);
            this.uExoandalStrategyControl.Location = new System.Drawing.Point(-10000, -10000);
            this.uExoandalStrategyControl.Margin = new System.Windows.Forms.Padding(0);
            this.uExoandalStrategyControl.Name = "uExoandalStrategyControl";
            this.uExoandalStrategyControl.Size = new System.Drawing.Size(194, 90);
            this.uExoandalStrategyControl.TabIndex = 0;
            this.uExoandalStrategyControl.Visible = false;
            // 
            // pranaOptionCtrl
            // 
            this.pranaOptionCtrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.pranaOptionCtrl.EnableValidate = false;
            this.pranaOptionCtrl.Location = new System.Drawing.Point(0, 3);
            this.pranaOptionCtrl.Margin = new System.Windows.Forms.Padding(0);
            this.pranaOptionCtrl.Name = "pranaOptionCtrl";
            this.pranaOptionCtrl.Size = new System.Drawing.Size(191, 85);
            this.pranaOptionCtrl.TabIndex = 0;
            this.pranaOptionCtrl.TabStop = false;
            this.pranaOptionCtrl.OptionGenerated += new System.EventHandler<Prana.Global.EventArgs<string>>(this.pranaOptionCtrl_OptionGenerated);
            // 
            // tblPnlTTMainControls
            // 
            this.tblPnlTTMainControls.AutoSize = true;
            this.tblPnlTTMainControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPnlTTMainControls.ColumnCount = 6;
            this.tblPnlTTMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTTMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTTMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTTMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTTMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTTMainControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tblPnlTTMainControls.Controls.Add(this.oneSymbolL1Strip, 0, 0);
            this.tblPnlTTMainControls.Controls.Add(this.lblOrderSide, 0, 1);
            this.tblPnlTTMainControls.Controls.Add(this.cmbOrderSide, 0, 2);
            //this.tblPnlTTMainControls.Controls.Add(this.cmbAllocation, 3, 2);
            //this.tblPnlTTMainControls.Controls.Add(this.cmbBroker, 4, 2);
            //this.tblPnlTTMainControls.Controls.Add(this.PnlBroker, 4, 1);
            //UPdated latest controls 
            this.tblPnlTTMainControls.Controls.Add(this.cmbAllocation, 2, 2);
            this.tblPnlTTMainControls.Controls.Add(this.ultraPanel2, 2, 1);
            this.tblPnlTTMainControls.Controls.Add(this.cmbBroker, 3, 2);
            this.tblPnlTTMainControls.Controls.Add(this.PnlBroker, 3, 1);
            this.tblPnlTTMainControls.Controls.Add(this.cmbVenue, 4, 2);
            this.tblPnlTTMainControls.Controls.Add(this.lblVenue, 4, 1);





            this.tblPnlTTMainControls.Controls.Add(this.lblStrategy, 0, 5);
            this.tblPnlTTMainControls.Controls.Add(this.cmbStrategy, 0, 6);
            this.tblPnlTTMainControls.Controls.Add(this.lblTradeDate, 1, 5);
            this.tblPnlTTMainControls.Controls.Add(this.dtTradeDate, 1, 6);
            this.tblPnlTTMainControls.Controls.Add(this.lblFXRate, 2, 5);
            this.tblPnlTTMainControls.Controls.Add(this.nmrcFXRate, 2, 6);
            this.tblPnlTTMainControls.Controls.Add(this.lblFxOperator, 3, 5);
            this.tblPnlTTMainControls.Controls.Add(this.cmbFxOperator, 3, 6);


            this.tblPnlTTMainControls.Controls.Add(this.ultraPanel5, 0, 3);
            this.tblPnlTTMainControls.Controls.Add(this.lblOrderType, 1, 3);
            this.tblPnlTTMainControls.Controls.Add(this.lblPrice, 4, 3);
            this.tblPnlTTMainControls.Controls.Add(this.lblStop, 2, 3);
            this.tblPnlTTMainControls.Controls.Add(this.cmbTIF, 0, 4);
            this.tblPnlTTMainControls.Controls.Add(this.cmbOrderType, 1, 4);

            this.tblPnlTTMainControls.Controls.Add(this.txtNotes, 0, 7);
            this.tblPnlTTMainControls.Controls.Add(this.txtBrokerNotes, 2, 7);
            this.tblPnlTTMainControls.Controls.Add(this.bottomTabControl, 0, 8);
            this.tblPnlTTMainControls.Controls.Add(this.nmrcQuantity, 1, 2);

            this.tblPnlTTMainControls.Controls.Add(this.nmrcPrice, 4, 4);
            this.tblPnlTTMainControls.Controls.Add(this.nmrcStop, 2, 4);
            this.tblPnlTTMainControls.Controls.Add(this.tblPnlButtons, 5, 2);
            this.tblPnlTTMainControls.Controls.Add(this.nmrcLimit, 3, 4);
            this.tblPnlTTMainControls.Controls.Add(this.lblAlgoMessage, 0, 9);
            this.tblPnlTTMainControls.Controls.Add(this.lblErrorMessage, 0, 10);
            this.tblPnlTTMainControls.Controls.Add(this.lblBrokerMessage, 0, 11);
            this.tblPnlTTMainControls.Controls.Add(this.pnlLimitPrice, 3, 3);

            //this.tblPnlTTMainControls.Controls.Add(this.ultraPanel2, 3, 1);

            this.tblPnlTTMainControls.Controls.Add(this.lblDealIn, 4, 5);
            this.tblPnlTTMainControls.Controls.Add(this.cmbDealIn, 4, 6);

            this.tblPnlTTMainControls.Controls.Add(this.pnlNotionalQuantity, 1, 1);
            this.tblPnlTTMainControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlTTMainControls.Location = new System.Drawing.Point(214, 38);
            this.tblPnlTTMainControls.Margin = new System.Windows.Forms.Padding(0);
            this.tblPnlTTMainControls.Name = "tblPnlTTMainControls";
            this.tblPnlTTMainControls.RowCount = 12;
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tblPnlTTMainControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            //this.tblPnlTTMainControls.Size = new System.Drawing.Size(786, 419);
            this.tblPnlTTMainControls.TabIndex = 1;
            this.tblPnlTTMainControls.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // oneSymbolL1Strip
            // 
            this.oneSymbolL1Strip.AutoSize = true;
            this.oneSymbolL1Strip.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPnlTTMainControls.SetColumnSpan(this.oneSymbolL1Strip, 6);
            this.oneSymbolL1Strip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oneSymbolL1Strip.Location = new System.Drawing.Point(3, 3);
            this.oneSymbolL1Strip.Name = "oneSymbolL1Strip";
            this.oneSymbolL1Strip.Size = new System.Drawing.Size(644, 40);
            this.oneSymbolL1Strip.Symbol = "";
            this.oneSymbolL1Strip.TabIndex = 0;
            // 
            // lblOrderSide
            // 
            this.lblOrderSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrderSide.Location = new System.Drawing.Point(3, 49);
            this.lblOrderSide.Name = "lblOrderSide";
            this.lblOrderSide.Size = new System.Drawing.Size(124, 16);
            this.lblOrderSide.TabIndex = 1;
            this.lblOrderSide.Text = "Order Side";
            // 
            // cmbOrderSide
            // 
            this.cmbOrderSide.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbOrderSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbOrderSide.DropDownListWidth = -1;
            this.cmbOrderSide.Location = new System.Drawing.Point(3, 71);
            this.cmbOrderSide.Name = "cmbOrderSide";
            this.cmbOrderSide.NullText = "Select Side";
            appearance15.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbOrderSide.NullTextAppearance = appearance15;
            this.cmbOrderSide.Size = new System.Drawing.Size(124, 21);
            this.cmbOrderSide.TabIndex = 6;
            this.cmbOrderSide.ValueChanged += new System.EventHandler(this.cmbOrderSide_ValueChanged);
            this.cmbOrderSide.Leave += new System.EventHandler(this.cmbOrderSide_Leave);
            // 
            // cmbAllocation
            // 
            this.cmbAllocation.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbAllocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAllocation.DropDownListWidth = -1;
            this.cmbAllocation.Location = new System.Drawing.Point(393, 71);
            this.cmbAllocation.Name = "cmbAllocation";
            this.cmbAllocation.NullText = "Select Account";
            appearance16.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbAllocation.NullTextAppearance = appearance16;
            this.cmbAllocation.Size = new System.Drawing.Size(124, 21);
            this.cmbAllocation.TabIndex = 9;
            this.cmbAllocation.ValueChanged += cmbAllocation_ValueChanged;
            this.cmbAllocation.Leave += cmb_Leave;
            // 
            // cmbBroker
            // 
            this.cmbBroker.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbBroker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBroker.DropDownListWidth = -1;
            this.cmbBroker.Location = new System.Drawing.Point(523, 71);
            this.cmbBroker.Name = "cmbBroker";
            this.cmbBroker.NullText = "Select Broker";
            appearance17.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbBroker.NullTextAppearance = appearance17;
            this.cmbBroker.Size = new System.Drawing.Size(124, 21);
            this.cmbBroker.TabIndex = 10;
            this.cmbBroker.ValueChanged += new System.EventHandler(this.cmbBroker_ValueChanged);
            this.cmbBroker.Leave += cmb_Leave;
            // 
            // lblBroker
            // 
            this.lblBroker.Location = new System.Drawing.Point(3, 3);
            this.lblBroker.Name = "lblBroker";
            this.lblBroker.Size = new System.Drawing.Size(124, 16);
            this.lblBroker.TabIndex = 5;
            this.lblBroker.Text = "Broker";
            // 
            // btnPadlock
            // 
            this.btnPadlock.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Borderless;
            this.btnPadlock.Appearance = appearance30;
            this.btnPadlock.Location = new System.Drawing.Point(75, 0);
            this.btnPadlock.Name = "btnPadlock";
            this.btnPadlock.Size = new System.Drawing.Size(20, 20);
            this.btnPadlock.ShowFocusRect = false;
            this.btnPadlock.ImageSize = new System.Drawing.Size(20, 20);
            this.btnPadlock.Visible = false;
            this.btnPadlock.AutoSize = true;
            this.btnPadlock.Appearance.BorderAlpha = Alpha.Transparent;
            this.btnPadlock.UseOsThemes = DefaultableBoolean.False;
            this.btnPadlock.UseAppStyling = false;
            this.btnPadlock.Click += new System.EventHandler(this.btnPadlock_Click);
            // 
            // lblOrderType
            // 
            this.lblOrderType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblOrderType.Location = new System.Drawing.Point(133, 99);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(124, 13);
            this.lblOrderType.TabIndex = 12;
            this.lblOrderType.Text = "Order Type";
            // 
            // lblPrice
            // 
            this.lblPrice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPrice.Location = new System.Drawing.Point(523, 99);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(124, 13);
            this.lblPrice.TabIndex = 13;
            this.lblPrice.Text = "Price";
            // 
            // lblStop
            // 
            this.lblStop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStop.Location = new System.Drawing.Point(263, 99);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(124, 13);
            this.lblStop.TabIndex = 14;
            this.lblStop.Text = "Stop";
            // 
            // cmbTIF
            // 
            this.cmbTIF.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbTIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTIF.DropDownListWidth = -1;
            this.cmbTIF.Location = new System.Drawing.Point(3, 118);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.NullText = "Select TIF";
            appearance18.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbTIF.NullTextAppearance = appearance18;
            this.cmbTIF.Size = new System.Drawing.Size(124, 21);
            this.cmbTIF.TabIndex = 16;
            this.cmbTIF.ValueChanged += cmbTIF_ValueChanged;
            this.cmbTIF.Leave += cmb_Leave;
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbOrderType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbOrderType.DropDownListWidth = -1;
            this.cmbOrderType.Location = new System.Drawing.Point(133, 118);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.NullText = "Select OrderType";
            appearance19.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbOrderType.NullTextAppearance = appearance19;
            this.cmbOrderType.Size = new System.Drawing.Size(124, 21);
            this.cmbOrderType.TabIndex = 17;
            this.cmbOrderType.ValueChanged += new System.EventHandler(this.cmbOrderType_ValueChanged);
            this.cmbOrderType.Leave += cmb_Leave;
            // 
            // lblStrategy
            // 
            this.lblStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStrategy.Location = new System.Drawing.Point(3, 144);
            this.lblStrategy.Name = "lblStrategy";
            this.lblStrategy.Size = new System.Drawing.Size(124, 14);
            this.lblStrategy.TabIndex = 21;
            this.lblStrategy.Text = "Strategy";
            // 
            // lblTradeDate
            // 
            this.lblTradeDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTradeDate.Location = new System.Drawing.Point(133, 144);
            this.lblTradeDate.Name = "lblTradeDate";
            this.lblTradeDate.Size = new System.Drawing.Size(124, 14);
            this.lblTradeDate.TabIndex = 22;
            this.lblTradeDate.Text = "Trade Date";
            // 
            // cmbStrategy
            // 
            this.cmbStrategy.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStrategy.DropDownListWidth = -1;
            this.cmbStrategy.Location = new System.Drawing.Point(3, 164);
            this.cmbStrategy.Name = "cmbStrategy";
            this.cmbStrategy.NullText = "Select Strategy";
            appearance20.ForeColor = System.Drawing.Color.DarkGray;
            this.cmbStrategy.NullTextAppearance = appearance20;
            this.cmbStrategy.Size = new System.Drawing.Size(124, 21);
            this.cmbStrategy.TabIndex = 25;
            this.cmbStrategy.ValueChanged += cmb_ValueChanged;
            this.cmbStrategy.Leave += cmb_Leave;
            // 
            // dtTradeDate
            // 
            this.dtTradeDate.Location = new System.Drawing.Point(133, 164);
            this.dtTradeDate.Name = "dtTradeDate";
            this.dtTradeDate.Size = new System.Drawing.Size(124, 21);
            this.dtTradeDate.TabIndex = 26;
            this.dtTradeDate.ValueChanged += dtTradeDate_ValueChanged;
            // 
            // txtNotes
            // 
            this.tblPnlTTMainControls.SetColumnSpan(this.txtNotes, 2);
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Location = new System.Drawing.Point(3, 195);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.NullText = "Notes";
            appearance21.ForeColor = System.Drawing.Color.Gray;
            this.txtNotes.NullTextAppearance = appearance21;
            this.txtNotes.Size = new System.Drawing.Size(254, 21);
            this.txtNotes.TabIndex = 30;
            // 
            // txtBrokerNotes
            // 
            this.tblPnlTTMainControls.SetColumnSpan(this.txtBrokerNotes, 2);
            this.txtBrokerNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBrokerNotes.Location = new System.Drawing.Point(263, 195);
            this.txtBrokerNotes.MaximumSize = new System.Drawing.Size(400, 400);
            this.txtBrokerNotes.Name = "txtBrokerNotes";
            this.txtBrokerNotes.NullText = "Broker Notes";
            appearance22.ForeColor = System.Drawing.Color.Gray;
            this.txtBrokerNotes.NullTextAppearance = appearance22;
            this.txtBrokerNotes.Size = new System.Drawing.Size(254, 21);
            this.txtBrokerNotes.TabIndex = 31;
            // 
            // bottomTabControl
            // 
            this.tblPnlTTMainControls.SetColumnSpan(this.bottomTabControl, 6);
            this.bottomTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl1);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl2);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl3);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl4);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl9);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl10);
            this.bottomTabControl.Controls.Add(this.ultraTabPageControl11);
            this.bottomTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomTabControl.Location = new System.Drawing.Point(3, 226);
            this.bottomTabControl.Name = "bottomTabControl";
            this.bottomTabControl.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.bottomTabControl.Size = new System.Drawing.Size(780, 158);
            this.bottomTabControl.MinimumSize = new System.Drawing.Size(780, 158);
            this.bottomTabControl.TabButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.bottomTabControl.TabIndex = 33;
            ultraTab7.Key = "tabAlgo";
            ultraTab7.TabPage = this.ultraTabPageControl1;
            ultraTab7.Text = "Algo";
            ultraTab7.Visible = false;
            ultraTab8.Key = "tabCommision";
            ultraTab8.TabPage = this.ultraTabPageControl2;
            ultraTab8.Text = "Commission";
            ultraTab9.Key = "tabTradeAttribute";
            ultraTab9.TabPage = this.ultraTabPageControl3;
            ultraTab9.Text = "Trade Attribute";
            ultraTab10.Key = "tabOther";
            ultraTab10.TabPage = this.ultraTabPageControl4;
            ultraTab10.Text = "Other";
            ultraTab11.Key = "tabSwap";
            ultraTab11.TabPage = this.ultraTabPageControl9;
            ultraTab11.Text = "Swap";
            ultraTab11.Visible = false;
            ultraTab12.Key = "tabSettlement";
            ultraTab12.TabPage = this.ultraTabPageControl10;
            ultraTab12.Text = "Settlement";
            ultraTab12.Visible = false;
            ultraTab1.Key = "tabBorrowParameter";
            ultraTab1.TabPage = this.ultraTabPageControl11;
            ultraTab1.Text = "Borrow Parameter";
            ultraTab1.Visible = false;
            this.bottomTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab10,
            ultraTab11,
            ultraTab12,
            ultraTab1});
            this.bottomTabControl.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Standard;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(730, 100);
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(776, 132);
            // 
            // nmrcQuantity
            // 
            this.nmrcQuantity.Increment = 0.0001m;
            this.nmrcQuantity.Location = new System.Drawing.Point(134, 73);
            this.nmrcQuantity.Maximum = 999999999;
            this.nmrcQuantity.Minimum = 0;
            this.nmrcQuantity.Name = "nmrcQuantity";
            this.nmrcQuantity.Size = new System.Drawing.Size(124, 20);
            this.nmrcQuantity.TabIndex = 7;
            this.nmrcQuantity.Value = 1;
            this.nmrcQuantity.RemoveThousandSeperatorOnEdit = true;
            this.nmrcQuantity.AllowThousandSeperator = true;
            this.nmrcQuantity.ShowCommaSeperatorOnEditing = true;
            this.nmrcQuantity.ValueChanged += new System.EventHandler(this.nmrcQuantity_ValueChanged);
            this.nmrcQuantity.Leave += new System.EventHandler<System.EventArgs>(this.nmrcQuantity_Leave);
            // 
            // nmrcTargetQuantity
            // 
            this.nmrcTargetQuantity.Increment = 0.0001m;
            this.nmrcTargetQuantity.Location = new System.Drawing.Point(265, 73);
            this.nmrcTargetQuantity.Maximum = 999999999;
            this.nmrcTargetQuantity.Minimum = 0;
            this.nmrcTargetQuantity.Name = "nmrcTargetQuantity";
            this.nmrcTargetQuantity.Size = new System.Drawing.Size(124, 20);
            this.nmrcTargetQuantity.TabIndex = 21;
            this.nmrcTargetQuantity.Value = 1;
            this.nmrcTargetQuantity.RemoveThousandSeperatorOnEdit = true;
            this.nmrcTargetQuantity.AllowThousandSeperator = true;
            this.nmrcTargetQuantity.ShowCommaSeperatorOnEditing = true;
            this.nmrcTargetQuantity.ValueChanged += new System.EventHandler(this.nmrcTargetQuantity_ValueChanged);
            // 
            // nmrcPrice
            // 
            this.nmrcPrice.Increment = 0.01m;
            this.nmrcPrice.Location = new System.Drawing.Point(265, 120);
            this.nmrcPrice.Maximum = 999999999;
            this.nmrcPrice.Minimum = 0;
            this.nmrcPrice.Name = "nmrcPrice";
            this.nmrcPrice.Size = new System.Drawing.Size(124, 20);
            this.nmrcPrice.TabIndex = 20;
            this.nmrcPrice.Value = 0m;
            this.nmrcPrice.RemoveThousandSeperatorOnEdit = true;
            this.nmrcPrice.AllowThousandSeperator = true;
            this.nmrcPrice.ShowCommaSeperatorOnEditing = true;
            this.nmrcPrice.ValueChanged += new System.EventHandler(this.nmrcPrice_ValueChanged);
            // 
            // nmrcStop
            // 
            this.nmrcStop.Increment = 0.01m;
            this.nmrcStop.Location = new System.Drawing.Point(396, 120);
            this.nmrcStop.Maximum = 999999999;
            this.nmrcStop.Minimum = 0;
            this.nmrcStop.Name = "nmrcStop";
            this.nmrcStop.Size = new System.Drawing.Size(124, 20);
            this.nmrcStop.TabIndex = 18;
            this.nmrcStop.RemoveThousandSeperatorOnEdit = true;
            this.nmrcStop.AllowThousandSeperator = true;
            this.nmrcStop.ShowCommaSeperatorOnEditing = true;
            this.nmrcStop.Value = 0m;
            // 
            // tblPnlButtons
            // 
            this.tblPnlButtons.ColumnCount = 1;
            this.tblPnlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPnlButtons.Controls.Add(this.btnViewAllocationDetails, 0, 3);
            this.tblPnlButtons.Controls.Add(this.btnShortLocateList, 0, 3);
            this.tblPnlButtons.Controls.Add(this.btnSend, 0, 2);
            this.tblPnlButtons.Controls.Add(this.btnCreateOrder, 0, 0);
            this.tblPnlButtons.Controls.Add(this.btnDoneAway, 0, 1);
            this.tblPnlButtons.Controls.Add(this.btnReplace, 0, 1);
            this.tblPnlButtons.Location = new System.Drawing.Point(653, 71);
            this.tblPnlButtons.Name = "tblPnlButtons";
            this.tblPnlButtons.RowCount = 5;
            this.tblPnlTTMainControls.SetRowSpan(this.tblPnlButtons, 6);
            this.tblPnlButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tblPnlButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tblPnlButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tblPnlButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tblPnlButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tblPnlButtons.Size = new System.Drawing.Size(125, 176);
            this.tblPnlButtons.TabIndex = 32;
            // 
            // btnViewAllocationDetails
            // 
            this.btnViewAllocationDetails.BackColorInternal = System.Drawing.Color.Transparent;
            this.btnViewAllocationDetails.Location = new System.Drawing.Point(3, 108);
            this.btnViewAllocationDetails.Name = "btnViewAllocationDetails";
            this.btnViewAllocationDetails.Size = new System.Drawing.Size(119, 30);
            this.btnViewAllocationDetails.TabIndex = 3;
            this.btnViewAllocationDetails.Text = "View Allocation";
            this.btnViewAllocationDetails.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnViewAllocationDetails.Visible = false;
            this.btnViewAllocationDetails.Click += new System.EventHandler(this.btnViewAllocationDetails_Click);
            // 
            // btnShortLocateList
            // 
            this.btnShortLocateList.BackColorInternal = System.Drawing.Color.Transparent;
            this.btnShortLocateList.Location = new System.Drawing.Point(3, 108);
            this.btnShortLocateList.Name = "btnShortLocateList";
            this.btnShortLocateList.Size = new System.Drawing.Size(119, 30);
            this.btnShortLocateList.TabIndex = 3;
            this.btnShortLocateList.Text = "Short Locate List";
            this.btnShortLocateList.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnShortLocateList.Visible = false;
            this.btnShortLocateList.Click += new System.EventHandler(this.btnShortLocateList_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColorInternal = System.Drawing.Color.Transparent;
            this.btnSend.Location = new System.Drawing.Point(3, 73);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(119, 29);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "&Send";
            this.btnSend.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.btnSend.MouseHover += message_MouseHover;
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.Location = new System.Drawing.Point(3, 3);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(119, 29);
            this.btnCreateOrder.TabIndex = 0;
            this.btnCreateOrder.Text = "&Create Order";
            this.btnCreateOrder.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            this.btnCreateOrder.MouseHover += message_MouseHover;
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(3, 63);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(119, 24);
            this.btnReplace.TabIndex = 1;
            this.btnReplace.Text = "&Replace";
            this.btnReplace.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnReplace.Visible = false;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            //btnDoneAway
            //
            this.btnDoneAway.Location = new System.Drawing.Point(3, 38);
            this.btnDoneAway.Name = "btnDoneAway";
            this.btnDoneAway.Size = new System.Drawing.Size(119, 29);
            this.btnDoneAway.TabIndex = 1;
            this.btnDoneAway.Text = "&Done Away";
            this.btnDoneAway.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.btnDoneAway.Click += new System.EventHandler(this.btnDoneAway_Click);
            // 
            // nmrcLimit
            // 
            this.nmrcLimit.Increment = 0.01m;
            this.nmrcLimit.Location = new System.Drawing.Point(527, 120);
            this.nmrcLimit.Maximum = 999999999m;
            this.nmrcLimit.Minimum = 0m;
            this.nmrcLimit.Name = "nmrcLimit";
            this.nmrcLimit.Size = new System.Drawing.Size(124, 20);
            this.nmrcLimit.TabIndex = 19;
            this.nmrcLimit.Value = 0m;
            this.nmrcLimit.RemoveThousandSeperatorOnEdit = true;
            this.nmrcLimit.AllowThousandSeperator = true;
            this.nmrcLimit.ShowCommaSeperatorOnEditing = true;
            this.nmrcLimit.ValueChanged += new System.EventHandler(this.nmrcLimit_ValueChanged);
            this.nmrcLimit.Leave += new System.EventHandler<System.EventArgs>(this.nmrcLimit_Leave);
            // 
            // lblErrorMessage
            // 
            this.tblPnlTTMainControls.SetColumnSpan(this.lblErrorMessage, 5);
            this.lblErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Gray;
            this.lblErrorMessage.Location = new System.Drawing.Point(3, 390);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(644, 15);
            this.lblErrorMessage.TabIndex = 20;
            this.lblErrorMessage.Margin = new System.Windows.Forms.Padding(1);
            this.lblErrorMessage.Text = " ";
            // 
            // lblErrorMessage
            // 
            this.tblPnlTTMainControls.SetColumnSpan(this.lblAlgoMessage, 5);
            this.lblAlgoMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAlgoMessage.ForeColor = System.Drawing.Color.Gray;
            this.lblAlgoMessage.Location = new System.Drawing.Point(3, 390);
            this.lblAlgoMessage.Name = "lblErrorMessage";
            this.lblAlgoMessage.Size = new System.Drawing.Size(644, 15);
            this.lblAlgoMessage.TabIndex = 20;
            this.lblAlgoMessage.Margin = new System.Windows.Forms.Padding(1);
            this.lblAlgoMessage.Visible = false;
            this.lblAlgoMessage.Text = "lblErrorMessage ";
            // 
            // lblBrokerMessage
            // 
            this.tblPnlTTMainControls.SetColumnSpan(this.lblBrokerMessage, 5);
            this.lblBrokerMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBrokerMessage.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblBrokerMessage.Location = new System.Drawing.Point(3, 0);
            this.lblBrokerMessage.Name = "lblBrokerMessage";
            this.lblBrokerMessage.Size = new System.Drawing.Size(644, 15);
            this.lblBrokerMessage.TabIndex = 21;
            this.lblBrokerMessage.Margin = new System.Windows.Forms.Padding(1);
            // 
            // pnlLimitPrice
            // 
            // 
            // pnlLimitPrice.ClientArea
            // 
            this.pnlLimitPrice.ClientArea.Controls.Add(this.btnGetLimitPrice);
            this.pnlLimitPrice.ClientArea.Controls.Add(this.lblLimit);
            this.pnlLimitPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLimitPrice.Location = new System.Drawing.Point(390, 96);
            this.pnlLimitPrice.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLimitPrice.Name = "pnlLimitPrice";
            this.pnlLimitPrice.Size = new System.Drawing.Size(130, 19);
            this.pnlLimitPrice.TabIndex = 35;
            // 
            // btnGetLimitPrice
            // 
            appearance26.BorderColor = System.Drawing.Color.Black;
            appearance26.Image = global::Prana.TradingTicket.Properties.Resources.level1;
            appearance26.TextHAlignAsString = "Center";
            appearance26.TextVAlignAsString = "Top";
            this.btnGetLimitPrice.Appearance = appearance26;
            this.btnGetLimitPrice.Location = new System.Drawing.Point(98, 3);
            this.btnGetLimitPrice.Margin = new System.Windows.Forms.Padding(0);
            this.btnGetLimitPrice.Name = "btnGetLimitPrice";
            this.btnGetLimitPrice.Size = new System.Drawing.Size(26, 20);
            this.btnGetLimitPrice.TabIndex = 1;
            this.btnGetLimitPrice.TabStop = false;
            this.btnGetLimitPrice.Click += new System.EventHandler(this.btnGetLimitPrice_Click);
            // 
            // lblLimit
            // 

            this.lblLimit.Location = new System.Drawing.Point(4, 10);
            this.lblLimit.Name = "lblLimit";
            this.lblLimit.Size = new System.Drawing.Size(45, 12);
            this.lblLimit.TabIndex = 1;
            this.lblLimit.Text = "Limit";
            // 
            // PnlTargetQuantity
            // 
            // 
            // PnlTargetQuantity.ClientArea
            // 
            this.PnlTargetQuantity.ClientArea.Controls.Add(this.btnTargetQuantityPercentage);
            this.PnlTargetQuantity.ClientArea.Controls.Add(this.lblTargetQuantity);
            this.PnlTargetQuantity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlTargetQuantity.Location = new System.Drawing.Point(260, 46);
            this.PnlTargetQuantity.Margin = new System.Windows.Forms.Padding(0);
            this.PnlTargetQuantity.Name = "PnlTargetQuantity";
            this.PnlTargetQuantity.Size = new System.Drawing.Size(130, 22);
            this.PnlTargetQuantity.TabIndex = 34;
            // 
            // PnlBroker
            // 
            this.PnlBroker.ClientArea.Controls.Add(this.btnPadlock);
            this.PnlBroker.ClientArea.Controls.Add(this.btnBrokerConnectionStatus);
            this.PnlBroker.ClientArea.Controls.Add(this.btnMultiBrokerConnectionStatus);
            this.PnlBroker.ClientArea.Controls.Add(this.lblBroker);
            this.PnlBroker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlBroker.Location = new System.Drawing.Point(260, 46);
            this.PnlBroker.Margin = new System.Windows.Forms.Padding(0);
            this.PnlBroker.Name = "PnlBroker";
            this.PnlBroker.Size = new System.Drawing.Size(130, 22);
            this.PnlBroker.TabIndex = 36;
            // 
            // btnTargetQuantityPercentage
            // 
            this.btnTargetQuantityPercentage.Location = new System.Drawing.Point(92, 2);
            this.btnTargetQuantityPercentage.Margin = new System.Windows.Forms.Padding(0);
            this.btnTargetQuantityPercentage.Name = "btnTargetQuantityPercentage";
            this.btnTargetQuantityPercentage.Size = new System.Drawing.Size(28, 18);
            this.btnTargetQuantityPercentage.TabIndex = 1;
            this.btnTargetQuantityPercentage.TabStop = false;
            this.btnTargetQuantityPercentage.Text = "N";
            this.btnTargetQuantityPercentage.Click += new System.EventHandler(this.btnTargetQuantityPercentage_Click);
            // 
            // btnBrokerConnectionStatus
            // 
            this.btnBrokerConnectionStatus.Location = new System.Drawing.Point(103, 4);
            this.btnBrokerConnectionStatus.Margin = new System.Windows.Forms.Padding(0);
            this.btnBrokerConnectionStatus.Name = "btnBrokerConnectionStatus";
            this.btnBrokerConnectionStatus.Size = new System.Drawing.Size(20, 15);
            this.btnBrokerConnectionStatus.TabIndex = 1;
            this.btnBrokerConnectionStatus.TabStop = false;
            this.btnBrokerConnectionStatus.Enabled = false;
            this.btnBrokerConnectionStatus.Appearance.BorderAlpha = Alpha.Transparent;
            this.btnBrokerConnectionStatus.Visible = false;
            // 
            // btnMultiBrokerConnectionStatus
            // 
            this.btnMultiBrokerConnectionStatus.Appearance = appearance27;
            this.btnMultiBrokerConnectionStatus.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnMultiBrokerConnectionStatus.Location = new System.Drawing.Point(99, 4);
            this.btnMultiBrokerConnectionStatus.Margin = new System.Windows.Forms.Padding(0);
            this.btnMultiBrokerConnectionStatus.Name = "btnMultiBrokerConnectionStatus";
            this.btnMultiBrokerConnectionStatus.Size = new System.Drawing.Size(26, 17);
            this.btnMultiBrokerConnectionStatus.TabIndex = 1;
            this.btnMultiBrokerConnectionStatus.TabStop = false;
            this.btnMultiBrokerConnectionStatus.Enabled = true;
            this.btnMultiBrokerConnectionStatus.Appearance.BorderAlpha = Alpha.Transparent;
            this.btnMultiBrokerConnectionStatus.Visible = false;
            this.btnMultiBrokerConnectionStatus.Click += new System.EventHandler(this.btnMultiBrokerConnectionStatus_Click);
            // 
            // lblTargetQuantity
            // 
            this.lblTargetQuantity.Location = new System.Drawing.Point(3, 3);
            this.lblTargetQuantity.Name = "lblTargetQuantity";
            this.lblTargetQuantity.Size = new System.Drawing.Size(86, 17);
            this.lblTargetQuantity.TabIndex = 1;
            this.lblTargetQuantity.Text = "Target Quantity";
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.btnAccountQty);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblAllocation);
            this.ultraPanel2.Location = new System.Drawing.Point(390, 46);
            this.ultraPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(130, 22);
            this.ultraPanel2.TabIndex = 34;

            // 
            // btnExpireTime
            // 
            appearance29.Image = global::Prana.TradingTicket.Properties.Resources.Calander;
            appearance29.TextHAlignAsString = HAlign.Right.ToString();
            appearance29.TextVAlignAsString = VAlign.Bottom.ToString();
            this.btnExpireTime.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Borderless;
            this.btnExpireTime.Appearance = appearance29;
            this.btnExpireTime.Location = new System.Drawing.Point(105, 3);
            this.btnExpireTime.Name = "btnExpireTime";
            this.btnExpireTime.ImageSize = new System.Drawing.Size(13, 13);
            this.btnExpireTime.Visible = false;
            this.btnExpireTime.AutoSize = true;
            this.btnExpireTime.UseOsThemes = DefaultableBoolean.False;
            this.btnExpireTime.UseAppStyling = false;
            this.btnExpireTime.Click += new System.EventHandler(this.btnExpireTime_Click);

            // 
            // lblExpireTime
            // 
            this.lblExpireTime.UseAppStyling = false;
            this.lblExpireTime.UseOsThemes = DefaultableBoolean.False;
            this.lblExpireTime.Location = new System.Drawing.Point(43, 7);
            this.lblExpireTime.Name = "lblExpireTime";
            this.lblExpireTime.Size = new System.Drawing.Size(63, 13);
            this.lblExpireTime.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblExpireTime.Text = "";
            this.lblExpireTime.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            this.lblExpireTime.Visible = false;
            this.lblExpireTime.Margin = new System.Windows.Forms.Padding(2);
            // 
            // dtExpireTime
            // 
            this.dtExpireTime.NullDateLabel = " -Select- ";
            this.dtExpireTime.DateButtons.Add(dateButton1);
            this.dtExpireTime.Location = new System.Drawing.Point(123, 1);
            this.dtExpireTime.Name = "dtExpireTime";
            this.dtExpireTime.Visible = false;
            this.dtExpireTime.NonAutoSizeHeight = 0;
            this.dtExpireTime.Size = new System.Drawing.Size(0, 0);
            this.dtExpireTime.AfterDropDown += DtExpireDate_AfterDropDown;
            this.dtExpireTime.TextChanged += DtExpireDate_TextChanged;
            this.dtExpireTime.AutoCloseUp = false;
            this.dtExpireTime.Format = "MM/dd/yyyy";
            this.dtExpireTime.UseAppStyling = false;
            // 
            // lblTIF
            // 
            //this.lblTIF.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTIF.Location = new System.Drawing.Point(1, 7);
            this.lblTIF.Name = "lblTIF";
            this.lblTIF.Size = new System.Drawing.Size(27, 13);
            this.lblTIF.TabIndex = 11;
            this.lblTIF.Text = "TIF";

            // 
            // ultraPanel5
            // 
            this.ultraPanel5.ClientArea.Controls.Add(this.lblTIF);
            this.ultraPanel5.ClientArea.Controls.Add(this.lblExpireTime);
            this.ultraPanel5.ClientArea.Controls.Add(this.btnExpireTime);
            this.ultraPanel5.ClientArea.Controls.Add(this.dtExpireTime);
            this.ultraPanel5.Location = new System.Drawing.Point(0, 99);
            this.ultraPanel5.Name = "ultraPanel5";
            this.ultraPanel5.Size = new System.Drawing.Size(130, 20);

            // 
            // btnAccountQty
            // 
            appearance27.BorderColor = System.Drawing.Color.Black;
            appearance27.Image = global::Prana.TradingTicket.Properties.Resources.level1;
            appearance27.TextHAlignAsString = "Center";
            appearance27.TextVAlignAsString = "Top";
            this.btnAccountQty.Appearance = appearance27;
            this.btnAccountQty.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAccountQty.Location = new System.Drawing.Point(97, 2);
            this.btnAccountQty.Name = "btnAccountQty";
            this.btnAccountQty.Size = new System.Drawing.Size(26, 17);
            this.btnAccountQty.TabIndex = 1;
            this.btnAccountQty.TabStop = false;
            this.btnAccountQty.Click += new System.EventHandler(this.btnAccountQty_Click);
            // 
            // lblAllocation
            // 
            this.lblAllocation.Location = new System.Drawing.Point(3, 4);
            this.lblAllocation.Margin = new System.Windows.Forms.Padding(0);
            this.lblAllocation.Name = "lblAllocation";
            this.lblAllocation.Size = new System.Drawing.Size(58, 16);
            this.lblAllocation.TabIndex = 0;
            this.lblAllocation.Text = "Allocation";
            // 
            // lblFXRate
            // 
            this.lblFXRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFXRate.Location = new System.Drawing.Point(263, 144);
            this.lblFXRate.Name = "lblFXRate";
            this.lblFXRate.Size = new System.Drawing.Size(124, 14);
            this.lblFXRate.TabIndex = 23;
            this.lblFXRate.Text = "FX Rate";
            // 
            // lblCCA
            // 
            this.lblCCA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCCA.Location = new System.Drawing.Point(523, 99);
            this.lblCCA.Name = "lblCCA";
            this.lblCCA.Size = new System.Drawing.Size(124, 14);
            this.lblCCA.TabIndex = 24;
            this.lblCCA.Text = "Counter Currency";
            // 
            // nmrcFXRate
            // 
            this.nmrcFXRate.Increment = 0.01m;
            this.nmrcFXRate.Location = new System.Drawing.Point(265, 166);
            this.nmrcFXRate.Maximum = 999999999m;
            this.nmrcFXRate.Minimum = 0m;
            this.nmrcFXRate.Name = "nmrcFXRate";
            this.nmrcFXRate.Size = new System.Drawing.Size(124, 20);
            this.nmrcFXRate.TabIndex = 27;
            this.nmrcFXRate.RemoveThousandSeperatorOnEdit = true;
            this.nmrcFXRate.AllowThousandSeperator = true;
            this.nmrcFXRate.ShowCommaSeperatorOnEditing = true;
            this.nmrcFXRate.Value = 0m;
            this.nmrcFXRate.ValueChanged += nmrcFXRate_ValueChanged;
            // 
            // nmrcCCA
            // 
            this.nmrcCCA.Increment = 0.01m;
            this.nmrcCCA.Location = new System.Drawing.Point(265, 166);
            this.nmrcCCA.Maximum = 999999999999m;
            this.nmrcCCA.Minimum = -999999999999m;
            this.nmrcCCA.Name = "nmrcFXRate";
            this.nmrcCCA.Size = new System.Drawing.Size(124, 20);
            this.nmrcCCA.TabIndex = 27;
            this.nmrcCCA.RemoveThousandSeperatorOnEdit = true;
            this.nmrcCCA.AllowThousandSeperator = true;
            this.nmrcCCA.ShowCommaSeperatorOnEditing = true;
            this.nmrcCCA.Value = 0m;
            // 
            // lblDealIn
            // 
            this.lblDealIn.Location = new System.Drawing.Point(523, 144);
            this.lblDealIn.Name = "lblDealIn";
            this.lblDealIn.Size = new System.Drawing.Size(109, 14);
            this.lblDealIn.TabIndex = 24;
            this.lblDealIn.Text = "Currency";
            this.lblDealIn.Visible = false;
            // 
            // cmbDealIn
            // 
            this.cmbDealIn.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
            this.cmbDealIn.DropDownListWidth = -1;
            this.cmbDealIn.Location = new System.Drawing.Point(523, 164);
            this.cmbDealIn.Name = "cmbDealIn";
            this.cmbDealIn.Size = new System.Drawing.Size(124, 21);
            this.cmbDealIn.TabIndex = 29;
            this.cmbDealIn.Visible = false;
            this.cmbDealIn.ValueChanged += new System.EventHandler(this.cmbDealIn_ValueChanged);
            // 
            // lblFxOperator
            // 
            this.lblFxOperator.Location = new System.Drawing.Point(393, 144);
            this.lblFxOperator.Name = "lblFxOperator";
            this.lblFxOperator.Size = new System.Drawing.Size(100, 14);
            this.lblFxOperator.TabIndex = 0;
            this.lblFxOperator.Text = "Fx Operator";
            // 
            // cmbFxOperator
            // 
            this.cmbFxOperator.Location = new System.Drawing.Point(393, 164);
            this.cmbFxOperator.Name = "cmbFxOperator";
            this.cmbFxOperator.Size = new System.Drawing.Size(124, 21);
            this.cmbFxOperator.TabIndex = 28;
            this.cmbFxOperator.ValueChanged += new System.EventHandler(this.cmbFxOperator_ValueChanged);
            // 
            // btnSymbolLookup
            // 
            this.btnSymbolLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.FlatAppearance.BorderSize = 0;
            this.btnSymbolLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.ForeColor = System.Drawing.Color.Transparent;
            this.btnSymbolLookup.UseVisualStyleBackColor = false;
            this.btnSymbolLookup.Location = new System.Drawing.Point(165, 3);
            this.btnSymbolLookup.Name = "btnSymbolLookup";
            this.btnSymbolLookup.Size = new System.Drawing.Size(30, 30);
            this.btnSymbolLookup.TabIndex = 1;
            this.btnSymbolLookup.BackgroundImage = global::Prana.TradingTicket.Properties.Resources.SecurityMaster;
            this.btnSymbolLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSymbolLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSymbolLookup.Click += new System.EventHandler(this.btnSymbolLookup_Click);
            this.btnSymbolLookup.GotFocus += btnSymbolLookup_GotFocus;
            this.btnSymbolLookup.LostFocus += btnSymbolLookup_LostFocus;
            this.btnSymbolLookup.MouseEnter += btnSymbolLookup_MouseEnter;
            this.btnSymbolLookup.MouseLeave += btnSymbolLookup_MouseLeave;
            // 
            // ultraLegend1
            // 
            this.ultraLegend1.AutoSize = false;
            this.ultraLegend1.Location = new System.Drawing.Point(129, 33);
            this.ultraLegend1.Name = "ultraLegend1";
            this.ultraLegend1.Size = new System.Drawing.Size(3, 3);
            this.ultraLegend1.TabIndex = 0;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _TradingTicket_UltraFormManager_Dock_Area_Left
            // 
            this._TradingTicket_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingTicket_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingTicket_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._TradingTicket_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingTicket_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._TradingTicket_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._TradingTicket_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._TradingTicket_UltraFormManager_Dock_Area_Left.Name = "_TradingTicket_UltraFormManager_Dock_Area_Left";
            this._TradingTicket_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 445);
            // 
            // _TradingTicket_UltraFormManager_Dock_Area_Right
            // 
            this._TradingTicket_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingTicket_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingTicket_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._TradingTicket_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingTicket_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._TradingTicket_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._TradingTicket_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1011, 31);
            this._TradingTicket_UltraFormManager_Dock_Area_Right.Name = "_TradingTicket_UltraFormManager_Dock_Area_Right";
            this._TradingTicket_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 445);
            // 
            // _TradingTicket_UltraFormManager_Dock_Area_Top
            // 
            this._TradingTicket_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingTicket_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingTicket_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._TradingTicket_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingTicket_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._TradingTicket_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TradingTicket_UltraFormManager_Dock_Area_Top.Name = "_TradingTicket_UltraFormManager_Dock_Area_Top";
            this._TradingTicket_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1019, 31);
            // 
            // _TradingTicket_UltraFormManager_Dock_Area_Bottom
            // 
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 476);
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.Name = "_TradingTicket_UltraFormManager_Dock_Area_Bottom";
            this._TradingTicket_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1019, 8);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(138, 100);
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(138, 100);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(138, 100);
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(138, 100);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ultraPanel4
            // 
            // 
            // ultraPanel4.ClientArea
            // 
            this.pnlNotionalQuantity.ClientArea.Controls.Add(this.btnQuantity);
            this.pnlNotionalQuantity.ClientArea.Controls.Add(this.lblQuantity);
            this.pnlNotionalQuantity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNotionalQuantity.Location = new System.Drawing.Point(130, 46);
            this.pnlNotionalQuantity.Margin = new System.Windows.Forms.Padding(0);
            this.pnlNotionalQuantity.Name = "ultraPanel4";
            this.pnlNotionalQuantity.Size = new System.Drawing.Size(130, 22);

            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Controls.Add(this.shortLocateList1);
            this.ultraTabPageControl11.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(510, 132);
            // 
            // shortLocateList1
            // 
            this.shortLocateList1.Location = new System.Drawing.Point(1, 3);
            this.shortLocateList1.Name = "shortLocateList1";
            this.shortLocateList1.Size = new System.Drawing.Size(505, 126);
            this.shortLocateList1.TabIndex = 0;
            // 
            // lblQuantity
            // 
            this.lblQuantity.Location = new System.Drawing.Point(3, 4);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(130, 22);
            this.lblQuantity.Text = "Quantity";
            // 
            // btnQuantity
            // 
            this.btnQuantity.Location = new System.Drawing.Point(92, 2);
            this.btnQuantity.Name = "btnQuantity";
            this.btnQuantity.Size = new System.Drawing.Size(28, 18);
            this.btnQuantity.Text = "Q";
            this.btnQuantity.Click += new System.EventHandler(this.btnQuantity_Click);
            // 
            // strategyControl1
            //// 
            //this.strategyControl1.AutoScroll = true;
            //this.strategyControl1.Location = new System.Drawing.Point(3, 3);
            //this.strategyControl1.Name = "strategyControl1";
            //this.strategyControl1.Size = new System.Drawing.Size(125, 152);
            //this.strategyControl1.TabIndex = 0;

            this.strategyControl1.AutoScroll = false;
            this.strategyControl1.Location = new System.Drawing.Point(0, 3);
            this.strategyControl1.Name = "strategyControl1";
            this.strategyControl1.Size = new System.Drawing.Size(191, 85);
            this.strategyControl1.TabIndex = 0;
            this.strategyControl1.TabStop = false;
            this.strategyControl1.Margin = new System.Windows.Forms.Padding(0);

            // 
            // TradingTicket
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1019, 490);
            this.Controls.Add(this.tblPnlMain);
            this.Controls.Add(this._TradingTicket_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._TradingTicket_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._TradingTicket_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._TradingTicket_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TradingTicket";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Trading Ticket";
            this.Load += new System.EventHandler(this.TradingTicket_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBxAlgo)).EndInit();
            this.grpBxAlgo.ResumeLayout(false);
            this.grpBxPnlAlgo.ResumeLayout(false);
            this.tblPnlAlgo.ResumeLayout(false);
            this.tblPnlAlgo.PerformLayout();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBxCommision)).EndInit();
            this.grpBxCommision.ResumeLayout(false);
            this.uExpandableGrpBxPanelCommision.ResumeLayout(false);
            this.tblPnlCommision.ResumeLayout(false);
            this.tblPnlCommision.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcSoftRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommissionBasis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSoftCommissionBasis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcCommissionRate)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBxTradeAttribute)).EndInit();
            this.grpBxTradeAttribute.ResumeLayout(false);
            this.grpBxTradeAttribute.PerformLayout();
            this.uExpandableGrpBxPanelTradeAttribute.ResumeLayout(false);
            this.uExpandableGrpBxPanelTradeAttribute.PerformLayout();
            this.tblPnlTradeAttribute.ResumeLayout(false);
            this.tblPnlTradeAttribute.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradeAttribute1)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grbBxOther)).EndInit();
            this.grbBxOther.ResumeLayout(false);
            this.uExpandableGrpBxPanelOther.ResumeLayout(false);
            this.uExpandableGrpBxPanelOther.PerformLayout();
            this.tblPnlOthers.ResumeLayout(false);
            this.tblPnlOthers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandlingInstructions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecutionInstructions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            this.ultraTabPageControl9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBxSwap)).EndInit();
            this.grpBxSwap.ResumeLayout(false);
            this.grpBxPnlSwap.ResumeLayout(false);
            this.ultraTabPageControl10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grbBxSettlementFields)).EndInit();
            this.grbBxSettlementFields.ResumeLayout(false);
            this.grpBxPnlSettlementFields.ResumeLayout(false);
            this.tblPnlSettlementFields.ResumeLayout(false);
            this.tblPnlSettlementFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSettlementCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcSettlementPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.tblPnlMain.ResumeLayout(false);
            this.tblPnlMain.PerformLayout();
            this.tblPnlAllControls.ResumeLayout(false);
            this.tblPnlAllControls.PerformLayout();
            this.tblPnlSymbolControl.ResumeLayout(false);
            this.tblPnlSymbolControl.PerformLayout();
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            this.ultraPanelSymbolLookup.ClientArea.ResumeLayout(false);
            this.ultraPanelSymbolLookup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxSwap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbBoxOptionControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbBoxStrategyControl)).EndInit();
            this.grbBoxOptionControl.ResumeLayout(false);
            this.grbBoxStrategyControl.ResumeLayout(false);
            this.uExoandalPnlOptionControl.ResumeLayout(false);
            this.tblPnlTTMainControls.ResumeLayout(false);
            this.tblPnlTTMainControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBroker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTradeDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBrokerNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomTabControl)).EndInit();
            this.bottomTabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcTargetQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcStop)).EndInit();
            this.tblPnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcLimit)).EndInit();
            this.pnlLimitPrice.ClientArea.ResumeLayout(false);
            this.pnlLimitPrice.ResumeLayout(false);
            this.PnlTargetQuantity.ClientArea.ResumeLayout(false);
            this.PnlTargetQuantity.ResumeLayout(false);
            this.PnlBroker.ClientArea.ResumeLayout(false);
            this.PnlBroker.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel5.ClientArea.ResumeLayout(false);
            this.ultraPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcFXRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcCCA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDealIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFxOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pnlNotionalQuantity.ClientArea.ResumeLayout(false);
            this.pnlNotionalQuantity.ResumeLayout(false);
            this.ultraTabPageControl11.ResumeLayout(false);
            this.ResumeLayout(false);
        }



        #endregion

        private System.Windows.Forms.TableLayoutPanel tblPnlMain;
        private System.Windows.Forms.TableLayoutPanel tblPnlAllControls;
        private System.Windows.Forms.TableLayoutPanel tblPnlSymbolControl;
        private Infragistics.Win.DataVisualization.UltraLegend ultraLegend2;
        private Infragistics.Win.Misc.UltraLabel lblSymbol;
        private Infragistics.Win.Misc.UltraLabel lblFunds;
        private Infragistics.Win.Misc.UltraLabel lblHiddenStrategyAdjust;

        private Infragistics.Win.DataVisualization.UltraLegend ultraLegend1;
        private Utilities.UI.UIUtilities.PranaSymbolCtrl pranaSymbolCtrl;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingTicket_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingTicket_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingTicket_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingTicket_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel tblPnlTTMainControls;
        private LiveFeed.UI.Controls.OneSymbolL1Strip oneSymbolL1Strip;
        private Infragistics.Win.Misc.UltraLabel lblOrderSide;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOrderSide;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbAllocation;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbBroker;
        private Infragistics.Win.Misc.UltraLabel lblBroker;
        private Infragistics.Win.Misc.UltraLabel lblTIF;
        private Infragistics.Win.Misc.UltraLabel lblOrderType;
        private Infragistics.Win.Misc.UltraLabel lblPrice;
        private Infragistics.Win.Misc.UltraLabel lblStop;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTIF;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOrderType;
        private Infragistics.Win.Misc.UltraLabel lblStrategy;
        private Infragistics.Win.Misc.UltraLabel lblTradeDate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbStrategy;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtTradeDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtExpireTime;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtNotes;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtBrokerNotes;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl bottomTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcQuantity;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcTargetQuantity;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcPrice;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcStop;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcFXRate;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcCCA;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpBxTradeAttribute;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel uExpandableGrpBxPanelTradeAttribute;
        private System.Windows.Forms.TableLayoutPanel tblPnlTradeAttribute;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute6;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute5;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute4;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute3;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute2;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute6;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute5;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute4;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute3;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute2;
        private Infragistics.Win.Misc.UltraLabel lblTradeAttribute1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradeAttribute1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grbBxOther;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel uExpandableGrpBxPanelOther;
        private System.Windows.Forms.TableLayoutPanel tblPnlOthers;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTradingAccount;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbHandlingInstructions;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbExecutionInstructions;
        private Infragistics.Win.Misc.UltraLabel lblVenue;
        private Infragistics.Win.Misc.UltraLabel lblExecutionInstructions;
        private Infragistics.Win.Misc.UltraLabel lblHandlingInstructions;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbVenue;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpBxCommision;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel uExpandableGrpBxPanelCommision;
        private System.Windows.Forms.TableLayoutPanel tblPnlCommision;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcSoftRate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbCommissionBasis;
        private Infragistics.Win.Misc.UltraLabel lblCommissionBasis;
        private Infragistics.Win.Misc.UltraLabel lblCommissionRate;
        private Infragistics.Win.Misc.UltraLabel lblSoftCommissionBasis;
        private Infragistics.Win.Misc.UltraLabel lblSoftRate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSoftCommissionBasis;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcCommissionRate;
        private Infragistics.Win.Misc.UltraLabel lblTradingAccount;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcLimit;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpBxAlgo;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel grpBxPnlAlgo;
        private System.Windows.Forms.TableLayoutPanel tblPnlAlgo;
        private StrategyControl strategyControl1;
        private AlgoStrategyControls.AlgoStrategyControl algoStrategyControl;
        private Infragistics.Win.Misc.UltraLabel lblErrorMessage;
        private Infragistics.Win.Misc.UltraLabel lblAlgoMessage;
        private Infragistics.Win.Misc.UltraLabel lblBrokerMessage;
        private Infragistics.Win.Misc.UltraPanel pnlLimitPrice;
        private Infragistics.Win.Misc.UltraLabel lblLimit;
        private Infragistics.Win.Misc.UltraButton btnGetLimitPrice;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpBxSwap;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel grpBxPnlSwap;
        private Infragistics.Win.Misc.UltraPanel PnlTargetQuantity;
        private Infragistics.Win.Misc.UltraPanel PnlBroker;
        private Infragistics.Win.Misc.UltraLabel lblTargetQuantity;
        private Infragistics.Win.Misc.UltraButton btnTargetQuantityPercentage;
        private Infragistics.Win.Misc.UltraButton btnPadlock;
        private Infragistics.Win.Misc.UltraButton btnBrokerConnectionStatus;
        private Infragistics.Win.Misc.UltraButton btnMultiBrokerConnectionStatus;
        private ClientCommon.CtrlSwapParameters ctrlSwapParameters1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel5;
        private Infragistics.Win.Misc.UltraButton btnAccountQty;
        private Infragistics.Win.Misc.UltraButton btnExpireTime;
        private Infragistics.Win.Misc.UltraLabel lblAllocation;
        private Infragistics.Win.Misc.UltraLabel lblExpireTime;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grbBxSettlementFields;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel grpBxPnlSettlementFields;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.TableLayoutPanel tblPnlSettlementFields;
        private Infragistics.Win.Misc.UltraLabel lblSettlementCurrency;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbSettlementCurrency;
        private Infragistics.Win.Misc.UltraLabel lblSettlementCurrencyPrice;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcSettlementPrice;
        private Infragistics.Win.Misc.UltraLabel lblFXRate;
        private Infragistics.Win.Misc.UltraLabel lblCCA;
        private Infragistics.Win.Misc.UltraLabel lblDealIn;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbDealIn;
        private System.Windows.Forms.TableLayoutPanel tblPnlButtons;
        private Infragistics.Win.Misc.UltraButton btnViewAllocationDetails;
        private Infragistics.Win.Misc.UltraButton btnOTCControl;

        private Infragistics.Win.Misc.UltraButton btnShortLocateList;
        private Infragistics.Win.Misc.UltraButton btnSend;
        private Infragistics.Win.Misc.UltraButton btnCreateOrder;
        private Infragistics.Win.Misc.UltraButton btnDoneAway;
        private Infragistics.Win.Misc.UltraButton btnReplace;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraPanel ultraPanelSymbolLookup;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxOption;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxEquitySwap;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxCFD;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxConvertiableBond;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxSwap;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grbBoxOptionControl;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grbBoxStrategyControl;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel uExoandalPnlOptionControl;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel uExoandalStrategyControl;
        private PranaOptionCtrl pranaOptionCtrl;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnSymbolLookup;
        private Infragistics.Win.Misc.UltraPanel pnlNotionalQuantity;
        private Infragistics.Win.Misc.UltraLabel lblQuantity;
        private Infragistics.Win.Misc.UltraButton btnQuantity;
        private Infragistics.Win.Misc.UltraLabel lblFxOperator;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbFxOperator;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbFunds;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private ShortLocate.Controls.ShortLocateList shortLocateList1;
    }
}