using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.ExposurePnlCache;
using Prana.BusinessObjects.AppConstants;
using System.Diagnostics;
using System.ComponentModel;
using Infragistics.Win.Misc;
using System.Configuration;
using Prana.Global;
using Prana.PNL.UI.Preferences;

namespace Prana.PNL.UI.Controls
{
    public partial class MainPnlGrid : UserControl
    {

        #region Grid Column Names

        const string CAPTION_DataSourceNameIDValue = "DataSourceNameIDValue";
        const string CAPTION_InternalFund = "InternalFund";
        const string CAPTION_ExternalFund = "ExternalFund";
        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_UnderlyingSymbol = "UnderlyingSymbol";
        const string CAPTION_Qty = "Quantity";
        const string CAPTION_AvgPrice = "AvgPrice";
        //const string CAPTION_Multiplier = "Multiplier";

        const string CAPTION_Asset = "Asset";
        const string CAPTION_Underlying = "Underlying";
        const string CAPTION_Exchange = "Exchange";
        const string CAPTION_Currency = "Currency";

        const string CAPTION_FXRate = "FXRate";
        const string CAPTION_ExecutedQty = "ExecutedQty";
        const string CAPTION_ExecutionPrice = "ExecutionPrice";
        const string CAPTION_NetExposure = "NetExposure";
        const string CAPTION_NetExposureBase = "NetExposureInCompnayBaseCurrency";

        const string CAPTION_NetPosition = "NetPosition";
        const string CAPTION_LastPrice = "LastPrice";
        const string CAPTION_BidPrice = "BidPrice";
        const string CAPTION_AskPrice = "AskPrice";
        const string CAPTION_ClosingPrice = "ClosingPrice";
        const string CAPTION_HighPrice = "HighPrice";
        const string CAPTION_LowPrice = "LowPrice";
        const string CAPTION_MidPrice = "MidPrice";
        const string CAPTION_PercentageChange = "PercentageChange";

        const string CAPTION_WeightedLongPrice = "WeightedLongPrice";
        const string CAPTION_WeightedShortPrice = "WeightedShortPrice";
        const string CAPTION_NetLong = "NetLong";
        const string CAPTION_NetLongBase = "NetLongInCompnayBaseCurrency";

        const string CAPTION_NetShort = "NetShort";
        const string CAPTION_NetShortBase = "NetShortInCompnayBaseCurrency";

        const string CAPTION_UnrealizedNetPnL = "UnrealizedNetPnL";
        const string CAPTION_UnrealizedNetPnLBase = "UnrealizedNetPnLInCompnayBaseCurrency";

        const string CAPTION_RealisedPNL = "CostBasisRealizedPNL";
        const string CAPTION_NetLongExposure = "NetLongExposure";
        const string CAPTION_NetShortExposure = "NetShortExposure";
        const string CAPTION_SecurityWeightLong = "SecurityWeightLong";
        const string CAPTION_SecurityWeightShort = "SecurityWeightShort";

        const string CAPTION_UnrealizedPnLLong = "UnrealizedPnLLong";
        const string CAPTION_UnrealizedPnLLongBase = "UnrealizedPnLLongInCompnayBaseCurrency";

        const string CAPTION_UnrealizedPnLShort = "UnrealizedPnLShort";
        const string CAPTION_UnrealizedPnLShortBase = "UnrealizedPnLShortInCompnayBaseCurrency";

        const string CAPTION_LongExposure = "LongExposure";
        const string CAPTION_LongExposureBase = "LongExposureInCompnayBaseCurrency";

        const string CAPTION_ShortExposure = "ShortExposure";
        const string CAPTION_ShortExposureBase = "ShortExposureInCompnayBaseCurrency";

        private const string CAPTION_PostitionID = "PositionID";
        const string CAPTION_TaxLotID = "TaxLotID";
        const string CAPTION_SideName = "SideName";

        const string CAPTION_IsPosition = "IsPosition";

        const string CAPTION_Strategy = "Strategy";

        const string CAPTION_TradeDate = "TradeDate";
        const string CAPTION_SettlementDate = "SettlementDate";

        const string CAPTION_Multiplier = "Multiplier";

        const string CAPTION_Sector = "Sector";
        const string CAPTION_StartOfDayPositions = "StartOfDayPosition";

        //const string CAPTION_UserMark = "UserMark";
        const string CAPTION_CostBasisRealizedPNL = "CostBasisRealizedPNL";
        const string CAPTION_MTDRealizedPnL = "MTDRealizedPnL";
        const string CAPTION_MTDUnrealizedPnL = "MTDUnrealizedPnL";
        const string CAPTION_YTDUnrealizedPnL = "YTDUnrealizedPnL";
        const string CAPTION_TotalMTDPnL = "TotalMTDPnL";
        const string CAPTION_YesterdayMarkPrice = "YesterdayMarkPrice";
        const string CAPTION_MonthMarkPrice = "MonthMarkPrice";
        const string CAPTION_Commission = "Commission";
        const string CAPTION_Fees = "Fees";

        private const string CAPTION_SideMultiplier = "SideMultiplier";

        const string CAPTION_ShortNotionalValue = "ShortNotionalValue";
        const string CAPTION_ShortNotionalValueInCompnayBaseCurrency = "ShortNotionalValueInCompnayBaseCurrency";

        const string CAPTION_LongNotionalValue = "LongNotionalValue";
        const string CAPTION_LongNotionalValueInCompnayBaseCurrency = "LongNotionalValueInCompnayBaseCurrency";

        const string CAPTION_NetNotionalValue = "NetNotionalValue";
        const string CAPTION_NetNotionalValueInCompnayBaseCurrency = "NetNotionalValueInCompnayBaseCurrency";

        const string CAPTION_OrderSideTagValue = "OrderSideTagValue";

        const string CAPTION_TradingAccount = "TradingAccount";

        const string CAPTION_PercentagePositionLong = "PercentagePositionLong";

        const string CAPTION_PercentagePositionShort = "PercentagePositionShort";

        const string CAPTION_PositionPNLLocal = "PositionPNLLocal";

        const string CAPTION_PositionPNLBase = "PositionPNLBase";

        const string CAPTION_Delta = "Delta";

        const string CAPTION_SelectedFeedPrice = "SelectedFeedPrice";

        const string CAPTION_PositionType = "PositionType";

        private const string SUB_MODULE_NAME = "CustomView";

        private const string CAPTION_ConsolidatedInfoType = "ConsolidationInfoType";

        #endregion Grid Column Names

        #region Formatting of columns
        private const string FORMAT_QTY = "#,##,###";
        private const string FORMAT_COST = "#,##,###.####";
        private const string FORMAT_PNL_EXPOSURE_CASH = "#,##,###.##";
        //private const string FORMAT_EXPOSURE = "";
        //private const string FORMAT_CASH = "";
        private const string FORMAT_FEEDS = "#,00.00";
        private const string FORMAT_PERCENTAGE = "###.##";
        #endregion Formatting of columns

        BindingSource _formBindingSource = new BindingSource();
        ExPnlBindableView _exPnlBindableView = null;
        static CompanyUser _loginUser;

        private PNLPreference _customViewPreference = null;

        SummarySettings summary = new SummarySettings();

        public event EventHandler TradeClick = null;
        public event EventHandler ChartsClick = null;
        public event EventHandler DepthClick = null;
        public event EventHandler OptionChainClick = null;

        public event EventHandler AddNewPNLView = null;

        public event EventHandler DeleteViewClick = null;
        public event EventHandler RenameViewClick = null;

        Color _firstLevelColor = Color.Gray;
        Color _secondLevelColor = Color.DarkKhaki;
        Color _thirdLevelColor = Color.Olive;

        public MainPnlGrid()
        {

            InitializeComponent();
        }


        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        #region InitControl Method
        //ConsolidatedInfoManager _consolidatedInfoManagerInstance = null;
        /// <summary>
        /// Initialize the control.
        /// </summary>

        public void InitControl(bool isGroupByAllowed, CompanyUser loginUser, PNLPreference preferences, ExPnlBindableView exPnlBindableView)
        {
            try
            {
                _customViewPreference = preferences;

                _firstLevelColor = Color.FromName(ConfigurationManager.AppSettings["PnLViewFirstLevelColor"]);
                _secondLevelColor = Color.FromName(ConfigurationManager.AppSettings["PnLViewSecondLevelColor"]);
                _thirdLevelColor = Color.FromName(ConfigurationManager.AppSettings["PnLViewThirdLevelColor"]);

                _exPnlBindableView = exPnlBindableView;

                _isGroupByAllowed = isGroupByAllowed;
                _loginUser = loginUser;

                if (!_isInitialized)
                {

                    try
                    {
                        SetupBinding();

                    }
                    catch (Exception ex)
                    {

                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        #endregion

        #endregion

        #region Setup Binding
        private void SetupBinding()
        {
            try
            {
                _formBindingSource.DataSource = _exPnlBindableView;

                grdPNL.DataBindings.Clear();
                grdPNL.DataMember = "GridData.Values";
                grdPNL.DataSource = null;
                grdPNL.DataSource = _formBindingSource;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        #endregion


        #region Property - IsGroupByAllowed
        private bool _isGroupByAllowed = false;

        /// <summary>
        /// Gets or sets a value indicating whether group is allowed  on grid or not by allowed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is group by allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsGroupByAllowed
        {
            set
            {
                _isGroupByAllowed = value;
            }
        }


        public bool DeleteViewEnabled
        {
            set
            {
                contextMenuStrip.Items["deleteViewToolStripMenuItem"].Enabled = value;
            }
        }

        public bool RenameViewEnabled
        {
            set
            {
                contextMenuStrip.Items["renameViewToolStripMenuItem"].Enabled = value;
            }
        }


        #endregion

        #region Caption Declarations here
        private const string CAP_Asset = "Asset Class";
        private const string CAP_Underlying = "Underlying";
        private const string CAP_Exchange = "Exchange";
        private const string CAP_Symbol = "Symbol";
        private const string CAP_InternalFund = "Fund";
        private const string CAP_Sector = "Sector";
        private const string CAP_Strategy = "Strategy";
        private const string CAP_TradingAccount = "Trading Account";
        private const string CAP_DataSource = "Upload Source";
        private const string CAP_Position = "Position";
        private const string CAP_PositionType = "Position Type";
        private const string CAP_SideName = "Order Side";

        #endregion

        #region Grid Initialize Layout

        bool firstTime = false;

        private void grdPNL_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {


            try
            {

                UltraGridLayout gridLayout = this.grdPNL.DisplayLayout;
                UltraGridBand band = this.grdPNL.DisplayLayout.Bands[0];
                gridLayout.Override.BorderStyleRowSelector = UIElementBorderStyle.None;
                gridLayout.Override.HeaderAppearance.TextHAlign = HAlign.Center;
                gridLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

                gridLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;

                band.ColHeaderLines = 1;

                band.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
                //band.Override.HeaderAppearance.BackColor = Color.WhiteSmoke;
                //band.Override.HeaderAppearance.BackColor2 = Color.DarkGray;
                //band.Override.HeaderAppearance.BackGradientStyle = GradientStyle.Vertical;

                band.Override.RowAppearance.TextVAlign = VAlign.Middle;
                band.Override.RowAlternateAppearance.TextVAlign = VAlign.Middle;

                Infragistics.Win.Appearance headerAppearance = new Infragistics.Win.Appearance();
                headerAppearance.BackColor = Color.WhiteSmoke;
                headerAppearance.BackColor2 = Color.SteelBlue;
                headerAppearance.BackGradientStyle = GradientStyle.Vertical;
                headerAppearance.TextHAlign = HAlign.Center;
                headerAppearance.TextVAlign = VAlign.Middle;

                #region Grid Columns
                UltraGridColumn colSymbol = band.Columns[CAPTION_Symbol];
                colSymbol.Header.Caption = CAP_Symbol;
                colSymbol.Header.VisiblePosition = 1;
                colSymbol.CellActivation = Activation.NoEdit;
                colSymbol.Header.Appearance = headerAppearance;
                colSymbol.AllowGroupBy = DefaultableBoolean.True;
                colSymbol.CellAppearance.FontData.Bold = DefaultableBoolean.True;
                colSymbol.Width = 120;

                UltraGridColumn colLastPrice = band.Columns[CAPTION_LastPrice];
                colLastPrice.Header.Caption = "Last Price";
                colLastPrice.Header.VisiblePosition = 2;
                colLastPrice.Format = FORMAT_FEEDS;
                colLastPrice.CellActivation = Activation.NoEdit;
                colLastPrice.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colBidPrice = band.Columns[CAPTION_BidPrice];
                colBidPrice.Header.Caption = "Bid Price";
                colBidPrice.Header.VisiblePosition = 3;
                colBidPrice.Format = FORMAT_FEEDS;
                colBidPrice.CellActivation = Activation.NoEdit;
                colBidPrice.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colAskPrice = band.Columns[CAPTION_AskPrice];
                colAskPrice.Header.Caption = "Ask Price";
                colAskPrice.Header.VisiblePosition = 4;
                colAskPrice.Format = FORMAT_FEEDS;
                colAskPrice.CellActivation = Activation.NoEdit;
                colAskPrice.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colPercentageChange = band.Columns[CAPTION_PercentageChange];
                colPercentageChange.Header.Caption = "% Change";
                colPercentageChange.Header.VisiblePosition = 5;
                colPercentageChange.Format = FORMAT_PERCENTAGE;
                colPercentageChange.CellActivation = Activation.NoEdit;
                colPercentageChange.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colLongExposure = band.Columns[CAPTION_LongExposure];
                colLongExposure.Header.Caption = "Long Exposure";
                colLongExposure.Format = FORMAT_PNL_EXPOSURE_CASH;
                colLongExposure.Header.VisiblePosition = 6;
                colLongExposure.CellActivation = Activation.NoEdit;
                colLongExposure.Hidden = true;
                colLongExposure.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colShortExposure = band.Columns[CAPTION_ShortExposure];
                colShortExposure.Header.Caption = "Short Exposure";
                colShortExposure.Format = FORMAT_PNL_EXPOSURE_CASH;
                colShortExposure.Header.VisiblePosition = 7;
                colShortExposure.CellActivation = Activation.NoEdit;
                colShortExposure.Hidden = true;
                colShortExposure.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colNetExposure = band.Columns[CAPTION_NetExposure];
                colNetExposure.Header.Caption = "Net Exposure";
                colNetExposure.Header.VisiblePosition = 8;
                colNetExposure.Format = FORMAT_PNL_EXPOSURE_CASH;
                colNetExposure.CellActivation = Activation.NoEdit;
                colNetExposure.AllowGroupBy = DefaultableBoolean.False;
                colNetExposure.Width = 150;

                UltraGridColumn colPNLLong = band.Columns[CAPTION_UnrealizedPnLLong];
                colPNLLong.Header.Caption = "P&L Long";
                colPNLLong.Format = FORMAT_PNL_EXPOSURE_CASH;
                colPNLLong.Header.VisiblePosition = 9;
                colPNLLong.CellActivation = Activation.NoEdit;
                colPNLLong.Hidden = true;
                colPNLLong.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colPNLShort = band.Columns[CAPTION_UnrealizedPnLShort];
                colPNLShort.Header.Caption = "P&L Short";
                colPNLShort.Format = FORMAT_PNL_EXPOSURE_CASH;
                colPNLShort.Header.VisiblePosition = 10;
                colPNLShort.CellActivation = Activation.NoEdit;
                colPNLShort.Hidden = true;
                colPNLShort.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colNetPNL = band.Columns[CAPTION_UnrealizedNetPnL];
                colNetPNL.Header.Caption = "Net P&L";
                colNetPNL.Header.VisiblePosition = 11;
                colNetPNL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colNetPNL.CellActivation = Activation.NoEdit;
                colNetPNL.AllowGroupBy = DefaultableBoolean.False;
                colNetPNL.Width = 150;

                UltraGridColumn colAsset = band.Columns[CAPTION_Asset];
                colAsset.Header.Caption = CAP_Asset;
                colAsset.Header.VisiblePosition = 12;
                colAsset.CellActivation = Activation.NoEdit;
                colAsset.Header.Appearance = headerAppearance;
                colAsset.AllowGroupBy = DefaultableBoolean.True;
                colAsset.Width = 120;

                UltraGridColumn colUnderlying = band.Columns[CAPTION_Underlying];
                colUnderlying.Header.Caption = CAP_Underlying;
                colUnderlying.Header.VisiblePosition = 13;
                colUnderlying.CellActivation = Activation.NoEdit;
                colUnderlying.Header.Appearance = headerAppearance;
                colUnderlying.AllowGroupBy = DefaultableBoolean.True;
                colUnderlying.Width = 120;

                UltraGridColumn colExchange = band.Columns[CAPTION_Exchange];
                colExchange.Header.Caption = CAP_Exchange;
                colExchange.Header.VisiblePosition = 14;
                colExchange.CellActivation = Activation.NoEdit;
                colExchange.Header.Appearance = headerAppearance;
                colExchange.AllowGroupBy = DefaultableBoolean.True;
                colExchange.Width = 120;

                UltraGridColumn colCurrency = band.Columns[CAPTION_Currency];
                colCurrency.Header.Caption = "Currency";
                colCurrency.Header.VisiblePosition = 15;
                colCurrency.CellActivation = Activation.NoEdit;
                colCurrency.AllowGroupBy = DefaultableBoolean.False;
                colCurrency.Width = 120;

                UltraGridColumn colAvgPrice = band.Columns[CAPTION_AvgPrice];
                colAvgPrice.Header.Caption = "Avg Price";
                colAvgPrice.Header.VisiblePosition = 16;
                colAvgPrice.Format = FORMAT_COST;
                colAvgPrice.CellActivation = Activation.NoEdit;
                colAvgPrice.Layout.Override.AllowRowSummaries = AllowRowSummaries.True;
                colAvgPrice.AllowGroupBy = DefaultableBoolean.False;
                colAvgPrice.Width = 150;

                UltraGridColumn colDaysTradedQty = band.Columns[CAPTION_ExecutedQty];
                colDaysTradedQty.Header.Caption = "Executed Qty";
                colDaysTradedQty.Header.VisiblePosition = 17;
                colDaysTradedQty.CellActivation = Activation.NoEdit;
                colDaysTradedQty.AllowGroupBy = DefaultableBoolean.False;
                colDaysTradedQty.Format = FORMAT_QTY;
                colDaysTradedQty.Width = 150;

                UltraGridColumn colOrderSideTagValue = band.Columns[CAPTION_OrderSideTagValue];
                colOrderSideTagValue.Header.VisiblePosition = 18;
                colOrderSideTagValue.Hidden = true;
                colOrderSideTagValue.CellActivation = Activation.NoEdit;
                colOrderSideTagValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colOrderSideTagValue.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colTradingAccount = band.Columns[CAPTION_TradingAccount];
                colTradingAccount.Header.Caption = CAP_TradingAccount;
                colTradingAccount.Header.VisiblePosition = 19;
                colTradingAccount.CellActivation = Activation.NoEdit;
                colTradingAccount.Header.Appearance = headerAppearance;
                colTradingAccount.AllowGroupBy = DefaultableBoolean.True;
                colTradingAccount.CellAppearance.FontData.Bold = DefaultableBoolean.True;

                UltraGridColumn colSideName = band.Columns[CAPTION_SideName];
                colSideName.Header.Caption = CAP_SideName;
                colSideName.Header.VisiblePosition = 18;
                colSideName.CellActivation = Activation.NoEdit;
                colSideName.AllowGroupBy = DefaultableBoolean.True;
                colSideName.Hidden = false;
                colSideName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                //Hidden Cols under

                UltraGridColumn colTradeDate = band.Columns[CAPTION_TradeDate];
                colTradeDate.Header.Caption = "Trade Date";
                colTradeDate.Header.VisiblePosition = 2;
                colTradeDate.CellActivation = Activation.NoEdit;
                colTradeDate.AllowGroupBy = DefaultableBoolean.False;
                colTradeDate.Hidden = true;
                colTradeDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colTradeDate.Width = 150;


                UltraGridColumn colSettlementDate = band.Columns[CAPTION_SettlementDate];
                colSettlementDate.Header.Caption = "Settlement Date";
                colSettlementDate.Header.VisiblePosition = 3;
                colSettlementDate.CellActivation = Activation.NoEdit;
                colSettlementDate.AllowGroupBy = DefaultableBoolean.False;
                colSettlementDate.Hidden = true;
                colSettlementDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSettlementDate.Width = 150;



                UltraGridColumn colInternalFund = band.Columns[CAPTION_InternalFund];
                colInternalFund.Header.Caption = CAP_InternalFund;
                colInternalFund.Header.VisiblePosition = 8;
                colInternalFund.CellActivation = Activation.NoEdit;
                colInternalFund.AllowGroupBy = DefaultableBoolean.True;
                colInternalFund.Header.Appearance = headerAppearance;
                colInternalFund.Hidden = true;
                colInternalFund.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colInternalFund.Width = 150;

                UltraGridColumn colExternalFund = band.Columns[CAPTION_ExternalFund];
                colExternalFund.Header.Caption = "External Fund";
                colExternalFund.Header.VisiblePosition = 9;
                colExternalFund.CellActivation = Activation.NoEdit;
                colExternalFund.Hidden = true;
                colExternalFund.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colExternalFund.AllowGroupBy = DefaultableBoolean.False;
                colExternalFund.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colStrategy = band.Columns[CAPTION_Strategy];
                colStrategy.Header.Caption = CAP_Strategy;
                colStrategy.Header.VisiblePosition = 10;
                colStrategy.CellActivation = Activation.NoEdit;
                colStrategy.Header.Appearance = headerAppearance;
                colStrategy.AllowGroupBy = DefaultableBoolean.True;
                colStrategy.Hidden = true;
                colStrategy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colStrategy.Width = 150;



                UltraGridColumn colDataSourceNameIDValue = band.Columns[CAPTION_DataSourceNameIDValue];
                colDataSourceNameIDValue.Header.Caption = CAP_DataSource;
                colDataSourceNameIDValue.Header.VisiblePosition = 12;
                colDataSourceNameIDValue.CellActivation = Activation.NoEdit;
                colDataSourceNameIDValue.Header.Appearance = headerAppearance;
                colDataSourceNameIDValue.AllowGroupBy = DefaultableBoolean.True;
                colDataSourceNameIDValue.Hidden = true;
                colDataSourceNameIDValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colDataSourceNameIDValue.Width = 150;

                UltraGridColumn colStartOfDayPositions = band.Columns[CAPTION_StartOfDayPositions];
                colStartOfDayPositions.Header.Caption = "Start Of Day Positions";
                colStartOfDayPositions.Header.VisiblePosition = 13;
                colStartOfDayPositions.Format = FORMAT_QTY;
                colStartOfDayPositions.CellActivation = Activation.NoEdit;
                colStartOfDayPositions.AllowGroupBy = DefaultableBoolean.False;
                colStartOfDayPositions.Hidden = true;
                colStartOfDayPositions.Width = 150;
                colStartOfDayPositions.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;


                UltraGridColumn colDayAveragePrice = band.Columns[CAPTION_ExecutionPrice];
                colDayAveragePrice.Header.Caption = "Execution Price";
                colDayAveragePrice.Header.VisiblePosition = 15;
                colDayAveragePrice.CellActivation = Activation.NoEdit;
                colDayAveragePrice.AllowGroupBy = DefaultableBoolean.False;
                colDayAveragePrice.Format = FORMAT_PNL_EXPOSURE_CASH;
                colDayAveragePrice.Hidden = true;
                colDayAveragePrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colDayAveragePrice.Width = 150;

                UltraGridColumn colPosition = band.Columns[CAPTION_Qty];
                colPosition.Header.Caption = CAP_Position;
                colPosition.Header.VisiblePosition = 16;
                colPosition.CellActivation = Activation.NoEdit;
                colPosition.Header.Appearance = headerAppearance;
                colPosition.AllowGroupBy = DefaultableBoolean.True;
                colPosition.Format = FORMAT_QTY;
                colPosition.Hidden = true;
                colPosition.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colPosition.Width = 150;

                UltraGridColumn colMultiplier = band.Columns[CAPTION_Multiplier];
                colMultiplier.Header.Caption = "Multiplier";
                colMultiplier.Header.VisiblePosition = 17;
                colMultiplier.CellActivation = Activation.NoEdit;
                colMultiplier.AllowGroupBy = DefaultableBoolean.False;
                colMultiplier.Hidden = true;
                colMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colSector = band.Columns[CAPTION_Sector];
                colSector.Header.Caption = "Sector";
                colSector.Header.VisiblePosition = 18;
                colSector.CellActivation = Activation.NoEdit;
                colSector.Header.Appearance = headerAppearance;
                colSector.AllowGroupBy = DefaultableBoolean.True;
                colSector.Width = 150;
                colSector.Hidden = true;
                colSector.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colFXRate = band.Columns[CAPTION_FXRate];
                colFXRate.Header.Caption = "FX Rate";
                colFXRate.Header.VisiblePosition = 19;
                colFXRate.CellActivation = Activation.NoEdit;
                colFXRate.AllowGroupBy = DefaultableBoolean.False;
                colFXRate.Hidden = true;
                colFXRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                //UltraGridColumn colUserMark = band.Columns[CAPTION_UserMark];
                //colUserMark.Header.Caption = "UserMark";
                //colUserMark.Header.VisiblePosition = 20;
                //colUserMark.CellActivation = Activation.NoEdit;
                //colUserMark.Format = FORMAT_FEEDS;
                //colUserMark.AllowGroupBy = DefaultableBoolean.False;
                //colUserMark.Hidden = true;
                //colUserMark.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colClosingPrice = band.Columns[CAPTION_ClosingPrice];
                colClosingPrice.Header.Caption = "Closing Price";
                colClosingPrice.Header.VisiblePosition = 4;
                colClosingPrice.Format = FORMAT_FEEDS;
                colClosingPrice.CellActivation = Activation.NoEdit;
                colClosingPrice.AllowGroupBy = DefaultableBoolean.False;
                //colClosingPrice.Hidden = true;
                //colClosingPrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colHighPrice = band.Columns[CAPTION_HighPrice];
                colHighPrice.Header.Caption = "High Price";
                colHighPrice.Header.VisiblePosition = 22;
                colHighPrice.Format = FORMAT_FEEDS;
                colHighPrice.CellActivation = Activation.NoEdit;
                colHighPrice.AllowGroupBy = DefaultableBoolean.False;
                colHighPrice.Hidden = true;
                colHighPrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colLowPrice = band.Columns[CAPTION_LowPrice];
                colLowPrice.Header.Caption = "Low Price";
                colLowPrice.Header.VisiblePosition = 23;
                colLowPrice.Format = FORMAT_FEEDS;
                colLowPrice.CellActivation = Activation.NoEdit;
                colLowPrice.AllowGroupBy = DefaultableBoolean.False;
                colLowPrice.Hidden = true;
                colLowPrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colMidPrice = band.Columns[CAPTION_MidPrice];
                colMidPrice.Header.Caption = "Mid Price";
                colMidPrice.Header.VisiblePosition = 27;
                colMidPrice.Format = FORMAT_FEEDS;
                colMidPrice.CellActivation = Activation.NoEdit;
                colMidPrice.AllowGroupBy = DefaultableBoolean.False;
                colMidPrice.Hidden = true;
                colMidPrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;


                //UltraGridColumn colNetExposureBase = band.Columns[CAPTION_NetExposureBase];
                //colNetExposureBase.Header.Caption = "Exposure Base";
                //colNetExposureBase.Header.VisiblePosition = 29;
                //colNetExposureBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colNetExposureBase.CellActivation = Activation.NoEdit;
                //colNetExposureBase.AllowGroupBy = DefaultableBoolean.False;
                //colNetExposureBase.Width = 150;
                //colNetExposure.Hidden = true;


                //UltraGridColumn colNetPNLBase = band.Columns[CAPTION_NetPNLBase];
                //colNetPNLBase.Header.Caption = "Trading P & L Base";
                //colNetPNLBase.Header.VisiblePosition = 31;
                //colNetPNLBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colNetPNLBase.CellActivation = Activation.NoEdit;
                //colNetPNLBase.AllowGroupBy = DefaultableBoolean.False;
                //colNetPNLBase.Hidden = true;

                //UltraGridColumn colPositionPNLBase = band.Columns[CAPTION_PositionPNLBase];
                //colPositionPNLBase.Header.Caption = "Position P & L Base";
                //colPositionPNLBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colPositionPNLBase.Header.VisiblePosition = 33;
                //colPositionPNLBase.CellActivation = Activation.NoEdit;
                //colPositionPNLBase.AllowGroupBy = DefaultableBoolean.False;
                //colPositionPNLBase.Hidden = true;

                UltraGridColumn colPercentagePositionLong = band.Columns[CAPTION_PercentagePositionLong];
                colPercentagePositionLong.Header.Caption = "% Position Long";
                colPercentagePositionLong.Header.VisiblePosition = 34;
                colPercentagePositionLong.CellActivation = Activation.NoEdit;
                colPercentagePositionLong.Hidden = true;
                colPercentagePositionLong.Format = FORMAT_PERCENTAGE;
                colPercentagePositionLong.AllowGroupBy = DefaultableBoolean.False;
                colPercentagePositionLong.Width = 150;
                colPercentagePositionLong.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                //UltraGridColumn colPositionPnLLocal = band.Columns[CAPTION_PositionPNLLocal];
                //colPositionPnLLocal.Header.Caption = "Position P&L Local";
                //colPositionPnLLocal.Header.VisiblePosition = 35;
                //colPositionPnLLocal.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colPositionPnLLocal.CellActivation = Activation.NoEdit;
                //colPositionPnLLocal.Hidden = true;
                //colPositionPnLLocal.AllowGroupBy = DefaultableBoolean.False;
                //colPositionPnLLocal.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colPercentagePositionShort = band.Columns[CAPTION_PercentagePositionShort];
                colPercentagePositionShort.Header.Caption = "% Position Short";
                colPercentagePositionShort.Header.VisiblePosition = 35;
                colPercentagePositionShort.Format = FORMAT_PERCENTAGE;
                colPercentagePositionShort.CellActivation = Activation.NoEdit;
                colPercentagePositionShort.Hidden = true;
                colPercentagePositionShort.AllowGroupBy = DefaultableBoolean.False;
                colPercentagePositionShort.Width = 150;
                colPercentagePositionShort.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colPositionType = band.Columns[CAPTION_PositionType];
                colPositionType.Header.Caption = CAP_PositionType;
                colPositionType.Header.VisiblePosition = 36;
                colPositionType.CellActivation = Activation.NoEdit;
                colPositionType.Header.Appearance = headerAppearance;
                colPositionType.AllowGroupBy = DefaultableBoolean.True;
                colPositionType.Hidden = true;
                colPositionType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;


                UltraGridColumn colRealisedPNL = band.Columns[CAPTION_RealisedPNL];
                colRealisedPNL.Header.Caption = "Realized PNL";
                colRealisedPNL.Header.VisiblePosition = 37;
                colRealisedPNL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colRealisedPNL.CellActivation = Activation.NoEdit;
                colRealisedPNL.Hidden = true;
                colRealisedPNL.AllowGroupBy = DefaultableBoolean.False;
                colRealisedPNL.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;


                //UltraGridColumn colPNLLongBase = band.Columns[CAPTION_PNLLongBase];
                //colPNLLongBase.Header.Caption = "P & L Long Base";
                //colPNLLongBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colPNLLongBase.Header.VisiblePosition = 40;
                //colPNLLongBase.CellActivation = Activation.NoEdit;
                //colPNLLongBase.Hidden = true;
                //colPNLLongBase.AllowGroupBy = DefaultableBoolean.False;



                //UltraGridColumn colPNLShortBase = band.Columns[CAPTION_PNLShortBase];
                //colPNLShortBase.Header.Caption = "P & L Short Base";
                //colPNLShortBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colPNLShortBase.Header.VisiblePosition = 42;
                //colPNLShortBase.CellActivation = Activation.NoEdit;
                //colPNLShortBase.Hidden = true;
                //colPNLShortBase.AllowGroupBy = DefaultableBoolean.False;


                //UltraGridColumn colLongExposureBase = band.Columns[CAPTION_LongExposureBase];
                //colLongExposureBase.Header.Caption = "Long Exposure Base";
                //colLongExposureBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colLongExposureBase.Header.VisiblePosition = 44;
                //colLongExposureBase.CellActivation = Activation.NoEdit;
                //colLongExposureBase.Hidden = true;
                //colLongExposureBase.AllowGroupBy = DefaultableBoolean.False;

                //UltraGridColumn colShortExposureBase = band.Columns[CAPTION_ShortExposureBase];
                //colShortExposureBase.Header.Caption = "Short Exposure Base";
                //colShortExposureBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colShortExposureBase.Header.VisiblePosition = 46;
                //colShortExposureBase.CellActivation = Activation.NoEdit;
                //colShortExposureBase.Hidden = true;
                //colShortExposureBase.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colShortNotionalValue = band.Columns[CAPTION_ShortNotionalValue];
                colShortNotionalValue.Header.Caption = "Short Notional";
                colShortNotionalValue.Format = FORMAT_PNL_EXPOSURE_CASH;
                colShortNotionalValue.Header.VisiblePosition = 47;
                colShortNotionalValue.CellActivation = Activation.NoEdit;
                colShortNotionalValue.Hidden = true;
                colShortNotionalValue.AllowGroupBy = DefaultableBoolean.False;
                colShortNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                //UltraGridColumn colShortNotionalValueBase = band.Columns[CAPTION_ShortNotionalValueInCompnayBaseCurrency];
                //colShortNotionalValueBase.Header.Caption = "Short Notional Base";
                //colShortNotionalValueBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colShortNotionalValueBase.Header.VisiblePosition = 48;
                //colShortNotionalValueBase.CellActivation = Activation.NoEdit;
                //colShortNotionalValueBase.Hidden = true;
                //colShortNotionalValueBase.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colLongNotionalValue = band.Columns[CAPTION_LongNotionalValue];
                colLongNotionalValue.Header.Caption = "Long Notional";
                colLongNotionalValue.Format = FORMAT_PNL_EXPOSURE_CASH;
                colLongNotionalValue.Header.VisiblePosition = 49;
                colLongNotionalValue.CellActivation = Activation.NoEdit;
                colLongNotionalValue.Hidden = true;
                colLongNotionalValue.AllowGroupBy = DefaultableBoolean.False;
                colLongNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                //UltraGridColumn colLongNotionalValueBase = band.Columns[CAPTION_LongNotionalValueInCompnayBaseCurrency];
                //colLongNotionalValueBase.Header.Caption = "Long Notional Base";
                //colLongNotionalValueBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colLongNotionalValueBase.Header.VisiblePosition = 50;
                //colLongNotionalValueBase.CellActivation = Activation.NoEdit;
                //colLongNotionalValueBase.Hidden = true;
                //colLongNotionalValueBase.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colNetNotionalValue = band.Columns[CAPTION_NetNotionalValue];
                colNetNotionalValue.Header.Caption = "Net Notional";
                colNetNotionalValue.Format = FORMAT_PNL_EXPOSURE_CASH;
                colNetNotionalValue.Header.VisiblePosition = 51;
                colNetNotionalValue.CellActivation = Activation.NoEdit;
                colNetNotionalValue.Hidden = true;
                colNetNotionalValue.AllowGroupBy = DefaultableBoolean.False;
                colNetNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                //UltraGridColumn colNetNotionalValueBase = band.Columns[CAPTION_NetNotionalValueInCompnayBaseCurrency];
                //colNetNotionalValueBase.Header.Caption = "Net Notional Base";
                //colNetNotionalValueBase.Format = FORMAT_PNL_EXPOSURE_CASH;
                //colNetNotionalValueBase.Header.VisiblePosition = 52;
                //colNetNotionalValueBase.CellActivation = Activation.NoEdit;
                //colNetNotionalValueBase.Hidden = true;
                //colNetNotionalValueBase.AllowGroupBy = DefaultableBoolean.False;





                UltraGridColumn colConsolidatedInfoType = band.Columns[CAPTION_ConsolidatedInfoType];
                colConsolidatedInfoType.Header.Caption = "Allocation Status";
                colConsolidatedInfoType.Header.VisiblePosition = 55;
                colConsolidatedInfoType.Hidden = true;
                colConsolidatedInfoType.CellActivation = Activation.NoEdit;
                colConsolidatedInfoType.AllowGroupBy = DefaultableBoolean.False;
                colConsolidatedInfoType.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colDelta = band.Columns[CAPTION_Delta];
                colDelta.Header.Caption = "Delta";
                colDelta.Header.VisiblePosition = 56;
                colDelta.Hidden = true;
                colDelta.CellActivation = Activation.NoEdit;
                colDelta.AllowGroupBy = DefaultableBoolean.False;
                colDelta.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colSelectedFeedPrice = band.Columns[CAPTION_SelectedFeedPrice];
                colSelectedFeedPrice.Header.Caption = "Selected Feed Price";
                colSelectedFeedPrice.Header.VisiblePosition = 57;
                colSelectedFeedPrice.Hidden = true;
                colSelectedFeedPrice.CellActivation = Activation.NoEdit;
                colSelectedFeedPrice.AllowGroupBy = DefaultableBoolean.False;
                colSelectedFeedPrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colUnderlyingSymbol = band.Columns[CAPTION_UnderlyingSymbol];
                colUnderlyingSymbol.Header.Caption = "Underlying Symbol";
                colUnderlyingSymbol.Header.VisiblePosition = 1;
                colUnderlyingSymbol.CellActivation = Activation.NoEdit;
                colUnderlyingSymbol.Header.Appearance = headerAppearance;
                colUnderlyingSymbol.AllowGroupBy = DefaultableBoolean.True;
                colUnderlyingSymbol.CellAppearance.FontData.Bold = DefaultableBoolean.True;
                colUnderlyingSymbol.Width = 120;
                colUnderlyingSymbol.Hidden = true;

                UltraGridColumn colCostBasisRealizedPNL = band.Columns[CAPTION_CostBasisRealizedPNL];
                colCostBasisRealizedPNL.Header.Caption = "Cost Basis Realized P&L";
                colCostBasisRealizedPNL.Header.VisiblePosition = 37;
                colCostBasisRealizedPNL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colCostBasisRealizedPNL.CellActivation = Activation.NoEdit;
                //colCostBasisRealizedPNL.Hidden = true;
                colCostBasisRealizedPNL.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colMTDRealizedPnL = band.Columns[CAPTION_MTDRealizedPnL];
                colMTDRealizedPnL.Header.Caption = "MTD Realized P&L";
                colMTDRealizedPnL.Header.VisiblePosition = 38;
                colMTDRealizedPnL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colMTDRealizedPnL.CellActivation = Activation.NoEdit;
                //colMTDRealizedPnL.Hidden = true;
                colMTDRealizedPnL.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colMTDUnrealizedPnL = band.Columns[CAPTION_MTDUnrealizedPnL];
                colMTDUnrealizedPnL.Header.Caption = "MTD Unrealized P&L";
                colMTDUnrealizedPnL.Header.VisiblePosition = 39;
                colMTDUnrealizedPnL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colMTDUnrealizedPnL.CellActivation = Activation.NoEdit;
                //colMTDUnrealizedPnL.Hidden = true;
                colMTDUnrealizedPnL.AllowGroupBy = DefaultableBoolean.False;


                UltraGridColumn colYTDUnrealizedPnL = band.Columns[CAPTION_YTDUnrealizedPnL];
                colYTDUnrealizedPnL.Header.Caption = "YTD Unrealized P&L";
                colYTDUnrealizedPnL.Header.VisiblePosition = 40;
                colYTDUnrealizedPnL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colYTDUnrealizedPnL.CellActivation = Activation.NoEdit;
                //colYTDUnrealizedPnL.Hidden = true;
                colYTDUnrealizedPnL.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colTotalMTDPnL = band.Columns[CAPTION_TotalMTDPnL];
                colTotalMTDPnL.Header.Caption = "Total MTD P&L";
                colTotalMTDPnL.Header.VisiblePosition = 41;
                colTotalMTDPnL.Format = FORMAT_PNL_EXPOSURE_CASH;
                colTotalMTDPnL.CellActivation = Activation.NoEdit;
                //colTotalMTDPnL.Hidden = true;
                colTotalMTDPnL.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colMonthMarkPrice = band.Columns[CAPTION_MonthMarkPrice];
                colMonthMarkPrice.Header.Caption = "Month Mark Price";
                colMonthMarkPrice.Header.VisiblePosition = 42;
                colMonthMarkPrice.Format = FORMAT_PNL_EXPOSURE_CASH;
                colMonthMarkPrice.CellActivation = Activation.NoEdit;
                //colMonthMarkPrice.Hidden = true;
                colMonthMarkPrice.AllowGroupBy = DefaultableBoolean.False;


                UltraGridColumn colYesterdayMarkPrice = band.Columns[CAPTION_YesterdayMarkPrice];
                colYesterdayMarkPrice.Header.Caption = "Yesterday Mark Price";
                colYesterdayMarkPrice.Header.VisiblePosition = 43;
                colYesterdayMarkPrice.Format = FORMAT_PNL_EXPOSURE_CASH;
                colYesterdayMarkPrice.CellActivation = Activation.NoEdit;
                //colYesterdayMarkPrice.Hidden = true;
                colYesterdayMarkPrice.AllowGroupBy = DefaultableBoolean.False;


                UltraGridColumn colCommission = band.Columns[CAPTION_Commission];
                colCommission.Header.Caption = "Commission";
                colCommission.Header.VisiblePosition = 44;
                colCommission.Format = FORMAT_PNL_EXPOSURE_CASH;
                colCommission.CellActivation = Activation.NoEdit;
                //colCommission.Hidden = true;
                colCommission.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colFees = band.Columns[CAPTION_Fees];
                colFees.Header.Caption = "Fees";
                colFees.Header.VisiblePosition = 45;
                colFees.Format = FORMAT_PNL_EXPOSURE_CASH;
                colFees.CellActivation = Activation.NoEdit;
                //colFees.Hidden = true;
                colFees.AllowGroupBy = DefaultableBoolean.False;

                UltraGridColumn colSideMultiplier = band.Columns[CAPTION_SideMultiplier];
                colSideMultiplier.Header.VisiblePosition = 59;
                colSideMultiplier.Hidden = true;
                colSideMultiplier.CellActivation = Activation.NoEdit;
                colSideMultiplier.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                colSideMultiplier.AllowGroupBy = DefaultableBoolean.False;

                #endregion

                //Summay setting
                SummarySettings();

                if (_isGroupByAllowed)
                {
                    gridLayout.GroupByBox.Hidden = false;
                    gridLayout.Override.AllowGroupBy = DefaultableBoolean.True;

                }
                else
                {
                    gridLayout.GroupByBox.Hidden = true;
                    gridLayout.Override.AllowGroupBy = DefaultableBoolean.False;


                }



                if (_customViewPreference != null)
                {
                    //Set Deselected columns
                    //foreach (string item in _customViewPreference.DeselectedColumnsCollection)
                    //{
                    //    e.Layout.Bands[0].Columns[item].Hidden = true;
                    //}

                    foreach (UltraGridColumn col in e.Layout.Bands[0].Columns)
                    {
                        if (_customViewPreference.DeselectedColumnsCollection.Contains(col.Key))
                        {
                            e.Layout.Bands[0].Columns[col.Key].Hidden = true;
                        }
                        else
                        {
                            if (e.Layout.Bands[0].Columns[col.Key].ExcludeFromColumnChooser != ExcludeFromColumnChooser.True)
                            {
                                e.Layout.Bands[0].Columns[col.Key].Hidden = false;
                            }

                        }
                    }

                    if (_customViewPreference.GroupByColumnsCollection.Count > 0)
                    {
                        //Set GroupBy Columns
                        e.Layout.Bands[0].SortedColumns.Clear();
                        foreach (string item in _customViewPreference.GroupByColumnsCollection)
                        {
                            e.Layout.Bands[0].SortedColumns.Add(item, false, true);
                        }
                    }
                    else
                    {
                        gridLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                    }
                }



            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Grid Initialize Row

        private void grdPNL_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                //To Do: fix it using enum!
                string orderSideTagValue = e.Row.Cells[CAPTION_OrderSideTagValue].Value.ToString();

                if (orderSideTagValue != null)
                {
                    switch (orderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_BuyMinus:
                            e.Row.Appearance.ForeColor = Color.FromArgb(177, 216, 64);
                            break;
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_SellPlus:
                        case FIXConstants.SIDE_SellShortExempt:
                            e.Row.Appearance.ForeColor = Color.FromArgb(255, 91, 71);
                            break;
                        default:
                            e.Row.Appearance.ForeColor = Color.FromArgb(255, 255, 255);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Summary Settings
        private void SummarySettings()
        {
            try
            {
                UltraGridBand band = this.grdPNL.DisplayLayout.Bands[0];

                // Add the OrderTotals custom summary. 
                if (!band.Summaries.Exists("AvgPriceSummary"))
                {
                    summary = band.Summaries.Add(
                        "AvgPriceSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderTotalsSummary(),        // Our custom summary calculator 
                        band.Columns[CAPTION_AvgPrice],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_AvgPrice]                            // Since SummaryPosition is Left, pass in null 
                        );

                    summary.DisplayFormat = "{0:#,###0.0000}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("ExecutedQtySummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "ExecutedQtySummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new PositionSummary(),        // Our custom summary calculator 
                        band.Columns[CAPTION_ExecutedQty],       // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // 
                        band.Columns[CAPTION_ExecutedQty]                            // 
                        );
                    summary.DisplayFormat = "{0:###,##,##0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("LongExposureSummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "LongExposureSummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderExposure(CAPTION_LongExposure),        // Our custom summary calculator 
                        band.Columns[CAPTION_LongExposure],       // Column being summarized. Just use Unit Price column. 
                        SummaryPosition.UseSummaryPositionColumn,           // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_LongExposure] // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0:#,###0.00}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("ShortExposureSummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "ShortExposureSummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderExposure(CAPTION_ShortExposure),        // Our custom summary calculator 
                        band.Columns[CAPTION_ShortExposure],       // Column being summarized. Just use Unit Price column. 
                        SummaryPosition.UseSummaryPositionColumn,           // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_ShortExposure] // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0:#,###0.00}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("NetExposureSummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "NetExposureSummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderExposure(CAPTION_NetExposure),        // Our custom summary calculator 
                        band.Columns[CAPTION_NetExposure],       // Column being summarized. Just use Unit Price column. 
                        SummaryPosition.UseSummaryPositionColumn,           // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_NetExposure] // Since SummaryPosition is Left, pass in null 
                        );
                    //summary2.DisplayFormat = "Total Net Exposure = {0:#,###0.00}";
                    summary.DisplayFormat = "{0:#,###0.00}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("LongPNLSummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "LongPNLSummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderPNL(CAPTION_UnrealizedPnLLong),        // Our custom summary calculator 
                        band.Columns[CAPTION_UnrealizedPnLLong],       // Column being summarized. Just use Unit Price column. 
                        SummaryPosition.UseSummaryPositionColumn,           // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_UnrealizedPnLLong] // Since SummaryPosition is Left, pass in null 
                        );

                    summary.DisplayFormat = "{0:#,###0.00}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("ShortPNLSummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "ShortPNLSummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderPNL(CAPTION_UnrealizedPnLShort),        // Our custom summary calculator 
                        band.Columns[CAPTION_UnrealizedPnLShort],       // Column being summarized. Just use Unit Price column. 
                        SummaryPosition.UseSummaryPositionColumn,           // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_UnrealizedPnLShort] // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0:#,###0.00}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("NetPNLSummary"))
                {
                    SummarySettings summary = band.Summaries.Add(
                        "NetPNLSummary",                    // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new OrderPNL(CAPTION_UnrealizedNetPnL),        // Our custom summary calculator 
                        band.Columns[CAPTION_UnrealizedNetPnL],       // Column being summarized. Just use Unit Price column. 
                        SummaryPosition.UseSummaryPositionColumn,           // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_UnrealizedNetPnL] // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0:#,###0.00}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }


                if (!band.Summaries.Exists("SymbolSummary"))
                {
                    summary = band.Summaries.Add(
                        "SymbolSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_Symbol),        // Our custom summary calculator 
                        band.Columns[CAPTION_Symbol],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_Symbol]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }


                if (!band.Summaries.Exists("AssetSummary"))
                {
                    summary = band.Summaries.Add(
                        "AssetSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_Asset),        // Our custom summary calculator 
                        band.Columns[CAPTION_Asset],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_Asset]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("UnderlyingSummary"))
                {
                    summary = band.Summaries.Add(
                        "UnderlyingSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_Underlying),        // Our custom summary calculator 
                        band.Columns[CAPTION_Underlying],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_Underlying]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("ExchangeSummary"))
                {
                    summary = band.Summaries.Add(
                        "ExchangeSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_Exchange),        // Our custom summary calculator 
                        band.Columns[CAPTION_Exchange],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_Exchange]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("CurrencySummary"))
                {
                    summary = band.Summaries.Add(
                        "CurrencySummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_Currency),        // Our custom summary calculator 
                        band.Columns[CAPTION_Currency],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_Currency]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

                }

                if (!band.Summaries.Exists("AskPriceSummary"))
                {
                    summary = band.Summaries.Add(
                        "AskPriceSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_AskPrice),        // Our custom summary calculator 
                        band.Columns[CAPTION_AskPrice],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_AskPrice]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("BidPriceSummary"))
                {
                    summary = band.Summaries.Add(
                        "BidPriceSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_BidPrice),        // Our custom summary calculator 
                        band.Columns[CAPTION_BidPrice],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_BidPrice]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("ClosingPriceSummary"))
                {
                    summary = band.Summaries.Add(
                        "ClosingPriceSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_ClosingPrice),        // Our custom summary calculator 
                        band.Columns[CAPTION_ClosingPrice],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_ClosingPrice]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }


                if (!band.Summaries.Exists("LowPriceSummary"))
                {
                    summary = band.Summaries.Add(
                        "LowPriceSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_LastPrice),        // Our custom summary calculator 
                        band.Columns[CAPTION_LastPrice],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_LastPrice]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("PercentageChangeSummary"))
                {
                    summary = band.Summaries.Add(
                        "PercentageChangeSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_PercentageChange),        // Our custom summary calculator 
                        band.Columns[CAPTION_PercentageChange],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_PercentageChange]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }

                if (!band.Summaries.Exists("TradingAccountSummary"))
                {
                    summary = band.Summaries.Add(
                        "TradingAccountSummary",                   // Give an identifier (key) for this summary 
                        SummaryType.Custom,                // Summary type is custom 
                        new TextColumnsSummary(CAPTION_TradingAccount),        // Our custom summary calculator 
                        band.Columns[CAPTION_TradingAccount],        // Column being summarized. 
                        SummaryPosition.UseSummaryPositionColumn,            // Position the summary on the left of summary footer 
                        band.Columns[CAPTION_TradingAccount]                            // Since SummaryPosition is Left, pass in null 
                        );
                    summary.DisplayFormat = "{0}";
                    summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Custom Summary Calculator Class Position Summary [Position]
        private class PositionSummary : ICustomSummaryCalculator
        {
            //			object[] parameter = new object[2];

            private decimal totals = Decimal.Zero;
            private bool isMultiple = false;
            private string previousSymbol = string.Empty;
            private bool isUndefined = false;
            //private decimal totalQty =  Decimal.Zero;
            internal PositionSummary()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                // Begins the summary for the SummarySettings object passed in. Implementation of  
                // this method should reset any state variables used for calculating the summary. 
                this.totals = 0;
                this.previousSymbol = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {

                // Here is where we process each row that gets passed in. 
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }


                    object symbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_Symbol]) ?? string.Empty;

                    if (previousSymbol == string.Empty)
                    {
                        //for the first time
                        previousSymbol = symbol.ToString();
                    }


                    if (symbol.ToString() != previousSymbol)
                    {
                        isMultiple = true;
                        return;
                    }

                    object quantity = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_Qty]) ?? string.Empty;
                    //object orderside = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[Global.OrderFields.PROPERTY_ORDER_SIDE]);
                    // Handle null values 
                    if (quantity is DBNull || quantity.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    // Convert to decimal. 

                    //decimal nUnitPrice = Decimal.Zero;
                    decimal nQuantity = Decimal.Zero;

                    if (quantity.ToString() != string.Empty || quantity.ToString() != Double.Epsilon.ToString() || quantity.ToString() != int.MinValue.ToString())
                    {
                        nQuantity = Convert.ToDecimal(quantity);
                    }

                    this.totals += nQuantity;


                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }

            }


            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                if (isMultiple)
                {
                    return "-";
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return "Undefined";
                    }
                    else
                    {
                        return "-";
                    }
                }
                else
                {
                    return this.totals;
                }
            }

        }
        #endregion

        #region Custom Summary Calculator Class OrderTotalsSummary [avg price]
        private class OrderTotalsSummary : ICustomSummaryCalculator
        {
            //			object[] parameter = new object[2];

            private decimal totals = Decimal.Zero;
            private decimal totalQty = Decimal.Zero;
            private bool isMultiple = false;
            private string previousSide = string.Empty;
            private string previousSymbol = string.Empty;
            private bool isUndefined = false;

            internal OrderTotalsSummary()
            {
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                // Begins the summary for the SummarySettings object passed in. Implementation of  
                // this method should reset any state variables used for calculating the summary. 
                this.totals = 0;
                this.totalQty = 0;
                this.previousSide = string.Empty;
                this.previousSymbol = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {

                try
                { // Here is where we process each row that gets passed in. 

                    if (isMultiple)
                    {
                        return;
                    }

                    object side = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_OrderSideTagValue]) ?? string.Empty;
                    object symbol = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_Symbol]) ?? string.Empty;


                    if (previousSide == string.Empty && previousSymbol == string.Empty)
                    {
                        previousSide = side.ToString();
                        previousSymbol = symbol.ToString();

                    }

                    if (side.ToString() != previousSide)
                    {
                        isMultiple = true;
                        return;
                    }

                    if (symbol.ToString() != previousSymbol)
                    {
                        isMultiple = true;
                        return;
                    }

                    object unitPrice = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_AvgPrice]) ?? string.Empty;
                    object quantity = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_Qty]) ?? string.Empty;

                    // Handle null values 
                    if (side is DBNull || symbol is DBNull)
                    {
                        return;
                    }

                    if (unitPrice is DBNull || quantity is DBNull || unitPrice.Equals(string.Empty) || quantity.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    // Convert to decimal. 

                    decimal nUnitPrice = Decimal.Zero;
                    decimal nQuantity = Decimal.Zero;
                    if (unitPrice.ToString() != string.Empty || unitPrice.ToString() != Double.Epsilon.ToString() || unitPrice.ToString() != int.MinValue.ToString())
                    {
                        nUnitPrice = Convert.ToDecimal(unitPrice);
                    }
                    if (quantity.ToString() != string.Empty || quantity.ToString() != Double.Epsilon.ToString() || quantity.ToString() != int.MinValue.ToString())
                    {
                        nQuantity = Convert.ToDecimal(quantity);
                    }

                    this.totals += nQuantity * nUnitPrice;
                    this.totalQty += nQuantity;
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }

            }


            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                decimal avgPrice = Decimal.Zero;
                try
                {
                    // This gets called when the every row has been processed so here is where we 
                    // would return the calculated summary value. 
                    if (this.totalQty != 0 && isMultiple == false)
                    {
                        avgPrice = this.totals / this.totalQty;
                    }
                    else if (isMultiple) //isMultiple
                    {
                        return "-";
                    }
                    else if (isUndefined == true)
                    {
                        if (this.totals == 0)
                        {
                            return "Undefined";
                        }
                        else
                        {
                            return "-";
                        }
                    }
                }

                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                return avgPrice;
            }

        }
        #endregion

        #region Custom Summary Calculator Class OrderExposure [Exposure]
        private class OrderExposure : ICustomSummaryCalculator
        {
            private decimal totals = Decimal.Zero;
            private bool isMultiple = false;
            private string previousCurrency = string.Empty;
            private bool isUndefined = false;

            string _exposureColumnName = string.Empty;

            internal OrderExposure(string exposureColumnName)
            {
                this._exposureColumnName = exposureColumnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                // Begins the summary for the SummarySettings object passed in. Implementation of  
                // this method should reset any state variables used for calculating the summary. 
                this.totals = 0;
                this.previousCurrency = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {

                // Here is where we process each row that gets passed in. 
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }


                    object currency = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_Currency]) ?? string.Empty;

                    if (currency == null)
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        //for the first time
                        previousCurrency = currency.ToString();
                    }


                    if (currency.ToString() != previousCurrency)
                    {
                        isMultiple = true;
                        return;
                    }

                    object exposure = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_exposureColumnName]) ?? string.Empty;

                    // Handle null values 
                    if (exposure is DBNull || exposure.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    // Convert to decimal. 

                    //decimal nUnitPrice = Decimal.Zero;
                    decimal nExposure = Decimal.Zero;

                    if (exposure.ToString() != string.Empty || exposure.ToString() != Double.Epsilon.ToString() || exposure.ToString() != int.MinValue.ToString())
                    {
                        nExposure = Convert.ToDecimal(exposure);
                    }

                    this.totals += nExposure;

                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }

            }


            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                // This gets called when the every row has been processed so here is where we 
                // would return the calculated summary value. 

                if (isMultiple)
                {
                    return "-";
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return "Undefined";
                    }
                    else
                    {
                        return "-";
                    }
                }
                else
                {
                    return this.totals;
                }
            }

        }
        #endregion

        #region Custom Summary Calculator Class OrderPNL [PNL]
        private class OrderPNL : ICustomSummaryCalculator
        {
            private decimal totals = Decimal.Zero;
            private bool isMultiple = false;
            private string previousCurrency = string.Empty;
            private bool isUndefined = false;
            string _pnlColumnName = string.Empty;

            internal OrderPNL(string pnlColumnName)
            {
                this._pnlColumnName = pnlColumnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {
                // Begins the summary for the SummarySettings object passed in. Implementation of  
                // this method should reset any state variables used for calculating the summary. 
                this.totals = 0;
                this.previousCurrency = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {

                // Here is where we process each row that gets passed in. 
                try
                {
                    if (isMultiple)
                    {
                        return;
                    }


                    object currency = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[CAPTION_Currency]) ?? string.Empty;

                    if (currency == null)
                    {
                        return;
                    }

                    if (previousCurrency == string.Empty)
                    {
                        //for the first time
                        previousCurrency = currency.ToString();
                    }


                    if (currency.ToString() != previousCurrency)
                    {
                        isMultiple = true;
                        return;
                    }

                    object pnl = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_pnlColumnName]) ?? string.Empty;

                    // Handle null values 
                    if (pnl is DBNull || pnl.Equals(string.Empty))
                    {
                        isUndefined = true;
                        return;
                    }

                    // Convert to decimal. 

                    //decimal nUnitPrice = Decimal.Zero;
                    decimal nPNL = Decimal.Zero;

                    if (pnl.ToString() != string.Empty || pnl.ToString() != Double.Epsilon.ToString() || pnl.ToString() != int.MinValue.ToString())
                    {
                        nPNL = Convert.ToDecimal(pnl);
                    }

                    this.totals += nPNL;

                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                    if (rethrow)
                    {
                        throw;
                    }
                }

            }


            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                // This gets called when the every row has been processed so here is where we 
                // would return the calculated summary value. 

                if (isMultiple)
                {
                    return "-";
                }
                else if (isUndefined == true)
                {
                    if (this.totals == 0)
                    {
                        return "Undefined";
                    }
                    else
                    {
                        return "-";
                    }
                }
                else
                {
                    return this.totals;
                }
            }

        }
        #endregion


        #region Custom  Class TextColumnsSummary [Text Columns Summary]
        private class TextColumnsSummary : ICustomSummaryCalculator
        {
            private bool isMultiple = false;
            private bool isUndefined = false;

            private string current = string.Empty;
            private string previous = string.Empty;
            private string _columnName = string.Empty;
            private string key = string.Empty;

            internal TextColumnsSummary(string columnName)
            {
                this._columnName = columnName;
            }

            public void BeginCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                // Begins the summary for the SummarySettings object passed in. Implementation of  
                // this method should reset any state variables used for calculating the summary. 
                this.current = string.Empty;
                this.previous = string.Empty;
                this.isMultiple = false;
                this.isUndefined = false;
            }

            public void AggregateCustomSummary(SummarySettings summarySettings, UltraGridRow row)
            {

                try
                { // Here is where we process each row that gets passed in. 

                    if (isMultiple)
                    {
                        return;
                    }

                    object value = row.GetCellValue(summarySettings.SourceColumn.Band.Columns[_columnName]) ?? string.Empty;

                    if (value == null || value == DBNull.Value)
                    {
                        isUndefined = true;
                        return;
                    }

                    current = value.ToString();

                    if (previous == string.Empty)
                    {

                        if (current == string.Empty || current == null)
                        {
                            isUndefined = true;
                            return;
                        }

                        previous = current.ToString();

                    }

                    if (current != previous)
                    {
                        isMultiple = true;
                        return;
                    }

                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                    if (rethrow)
                    {
                        throw;
                    }
                }

            }


            public object EndCustomSummary(SummarySettings summarySettings, RowsCollection rows)
            {

                if (isMultiple)
                {
                    switch (_columnName)
                    {
                        case CAPTION_AskPrice:
                        case CAPTION_BidPrice:
                        case CAPTION_ClosingPrice:
                        case CAPTION_LastPrice:
                        case CAPTION_HighPrice:
                        case CAPTION_LowPrice:
                        case CAPTION_MidPrice:
                        //case CAPTION_UserMark:
                            return "-";
                        default:
                            return "Multiple";
                            break;
                    }
                    //return "Multiple";
                }
                else if (isUndefined)
                {
                    if (current != string.Empty)
                    {
                        switch (_columnName)
                        {
                            case CAPTION_AskPrice:
                            case CAPTION_BidPrice:
                            case CAPTION_ClosingPrice:
                            case CAPTION_LastPrice:
                            case CAPTION_HighPrice:
                            case CAPTION_LowPrice:
                            case CAPTION_MidPrice:
                            //case CAPTION_UserMark:
                                return "-";
                            default:
                                return "Multiple";
                                break;
                        }
                    }
                    else
                    {
                        return "Undefined";
                    }
                }
                else
                {
                    return current;
                }


            }
        }
        #endregion

        #region Contex Menu items here!

        public void SaveLayout()
        {
            try
            {
                if (_customViewPreference != null)
                {

                    //ArrayList groupByColsList = new ArrayList();
                    ArrayList colsCollection = new ArrayList();

                    //////update existing tabs collection !!
                    ////foreach (UltraGridColumn groupCols in grdPNL.DisplayLayout.Bands[0].SortedColumns)
                    ////{

                    ////    if (groupByColsList.Count < 3 && groupCols.AllowGroupBy == DefaultableBoolean.True && groupCols.IsGroupByColumn == true)
                    ////    {
                    ////        groupByColsList.Add(groupCols.Key);
                    ////    }

                    ////}

                    ////_customViewPreference.GroupByColumnsCollection = groupByColsList;

                    //add columns collection !!
                    foreach (UltraGridColumn column in grdPNL.DisplayLayout.Bands[0].Columns)
                    {
                        if (column.Hidden)
                        {
                            colsCollection.Add(column.Key);
                        }

                    }

                    _customViewPreference.DeselectedColumnsCollection = colsCollection;
                    //Set the order of groupby cols correctly
                    if (grdPNL.DisplayLayout.UIElement.ChildElements.Count > 1) //if count is one the it is main consolidation view. In which case there is no group box
                    {
                        GroupByBoxUIElement GrpElem = (GroupByBoxUIElement)grdPNL.DisplayLayout.UIElement.ChildElements[0];
                        _customViewPreference.GroupByColumnsCollection.Clear();

                        foreach (UIElement elm in GrpElem.ChildElements)
                        {
                            Infragistics.Win.UltraWinGrid.ColumnHeader obj = elm.GetContext() as Infragistics.Win.UltraWinGrid.ColumnHeader;
                            if (obj != null)
                                _customViewPreference.GroupByColumnsCollection.Add(GetColumnNameByCaption(obj.Caption));
                        }
                    }
                    PNLPrefrenceManager.GetInstance(SUB_MODULE_NAME).SetCustomViewPreference(_customViewPreference);
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveLayout();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void depthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPNL.ActiveRow != null && !(grdPNL.ActiveRow is UltraGridGroupByRow))
                {
                    OpenDepth();

                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void tradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPNL.ActiveRow != null && !(grdPNL.ActiveRow is UltraGridGroupByRow))
                {
                    OnTradeClick();

                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row to trade!", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPNL.ActiveRow != null && !(grdPNL.ActiveRow is UltraGridGroupByRow))
                {
                    OpenCharts();

                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void optionChainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPNL.ActiveRow != null && !(grdPNL.ActiveRow is UltraGridGroupByRow))
                {
                    OpenOptionChain();

                }
                else
                {
                    MessageBox.Show(this, "Please select a valid row!", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Opens the charts for passed symbol.
        /// </summary>
        private void OpenOptionChain()
        {

            try
            {
                if (grdPNL.ActiveRow != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = grdPNL.ActiveRow;
                    ExposurePnlCacheItem consolidatedInfo = (ExposurePnlCacheItem)grdPNL.ActiveRow.ListObject;

                    string optionChainSymbol = consolidatedInfo.Symbol.ToUpper();

                    if (OptionChainClick != null)
                    {
                        OptionChainClick(optionChainSymbol, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


        }

        /// <summary>
        /// Opens the charts for passed symbol.
        /// </summary>
        private void OpenDepth()
        {
            try
            {
                if (grdPNL.ActiveRow != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = grdPNL.ActiveRow;
                    ExposurePnlCacheItem consolidatedInfo = (ExposurePnlCacheItem)grdPNL.ActiveRow.ListObject;

                    string depthSymbol = consolidatedInfo.Symbol.ToUpper();

                    if (DepthClick != null)
                    {
                        DepthClick(depthSymbol, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


        }

        /// <summary>
        /// Opens the charts for passed symbol.
        /// </summary>
        private void OpenCharts()
        {
            try
            {
                if (grdPNL.ActiveRow != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridRow row = grdPNL.ActiveRow;
                    ExposurePnlCacheItem consolidatedInfo = (ExposurePnlCacheItem)grdPNL.ActiveRow.ListObject;

                    string chartsSymbol = consolidatedInfo.Symbol.ToUpper();

                    if (ChartsClick != null)
                    {
                        ChartsClick(chartsSymbol, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


        }

        /// <summary>
        /// Call this function on the click of trade.
        /// </summary>
        private void OnTradeClick()
        {
            try
            {

                TradeParametersArgs tradeParameters = new TradeParametersArgs();


                if (grdPNL.ActiveRow != null)
                {

                    Infragistics.Win.UltraWinGrid.UltraGridRow row = grdPNL.ActiveRow;
                    ExposurePnlCacheItem consolidatedInfo = (ExposurePnlCacheItem)grdPNL.ActiveRow.ListObject;

                    tradeParameters.AssetId = (AssetCategory)consolidatedInfo.Asset;
                    tradeParameters.AUECId = (int)consolidatedInfo.AUECID;
                    if (consolidatedInfo.Exchange != null)
                    {
                        tradeParameters.ListedExchangeId = consolidatedInfo.Exchange.ExchangeID;
                    }
                    if (consolidatedInfo.Currency != null)
                    {
                        tradeParameters.CurrencyId = consolidatedInfo.Currency.CurrencyID;
                    }

                    tradeParameters.OrderSide = consolidatedInfo.OrderSideTagValue;
                    tradeParameters.Price = consolidatedInfo.AvgPrice;

                    if (consolidatedInfo.ConsolidationInfoType == ConsolidationInfoType.AllocatedOrder || consolidatedInfo.ConsolidationInfoType == ConsolidationInfoType.UnAllocatedOrder)
                    {
                        tradeParameters.Quantity = consolidatedInfo.ExecutedQty;
                    }
                    else
                    {
                        tradeParameters.Quantity = consolidatedInfo.Quantity;
                    }
                    tradeParameters.Symbol = consolidatedInfo.Symbol;
                    if (consolidatedInfo.Underlying != Underlying.None)
                    {
                        tradeParameters.UnderlyingId = (Prana.BusinessObjects.AppConstants.Underlying)consolidatedInfo.Underlying;
                    }


                    if (TradeClick != null)
                    {
                        TradeClick(tradeParameters, EventArgs.Empty);

                    }


                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        UltraGridRow _CurrentUltraGridGroupByRow = null;
        void grdPNL_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
                if (e.Button == MouseButtons.Right)
                {
                    Point mousePoint = new Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;

                    if (cell != null)
                    {
                        cell.Row.Activate();
                        _CurrentUltraGridGroupByRow = cell.Row.ParentRow;
                    }
                    else
                    {
                    _CurrentUltraGridGroupByRow = element.SelectableItem as UltraGridRow;
                    }
                }

                //Verify that the click is a right-click
                if (e.Button == MouseButtons.Left)
                {
                _MouseDownOnHeader = true;
                _MouseDownOnGroupBox = false;

                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = this.grdPNL.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

                    // Exit the event handler if no UIElement is found
                    if (thisElem == null)
                        return;

                    if (thisElem is HeaderUIElement || thisElem.GetAncestor(typeof(HeaderUIElement)) is HeaderUIElement)
                    {

                        if (thisElem is Infragistics.Win.TextUIElement)
                        {
                        _MouseDownOnHeader = true;
                        _MouseDownOnGroupBox = false;
                        _StrSelectedGroupByCol = String.Empty;
                        if (thisElem is Infragistics.Win.TextUIElement)
                            {
                            _StrSelectedGroupByCol = GetColumnNameByCaption(((Infragistics.Win.TextUIElement)(thisElem)).Text);
                            }
                        }

                    }
                    //See if the UIElement at the current mouse position is a GroupByBoxUIElement,
                    // or if it is contained as a child of a GroupByBoxUIElement
                    if (thisElem is GroupByBoxUIElement ||
                        thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement)
                    {
                    _MouseDownOnHeader = false;
                    _MouseDownOnGroupBox = true;
                    _StrSelectedGroupByCol = String.Empty;
                        if (thisElem is Infragistics.Win.TextUIElement)
                        {
                        _StrSelectedGroupByCol = GetColumnNameByCaption(((Infragistics.Win.TextUIElement)(thisElem)).Text);
                        }

                    }
                }
        }


        private string GetColumnNameByCaption(string caption)
        {
            string col = string.Empty;

            try
            {
                switch (caption)
                {
                    case CAP_Asset:
                        col = CAPTION_Asset;
                        break;

                    case CAP_Underlying:
                        col = CAPTION_Underlying;
                        break;

                    case CAP_Exchange:
                        col = CAPTION_Exchange;
                        break;

                    case CAP_Symbol:
                        col = CAPTION_Symbol;
                        break;

                    case CAP_InternalFund:
                        col = CAPTION_InternalFund;
                        break;

                    case CAP_Sector:
                        col = CAPTION_Sector;
                        break;

                    case CAP_Strategy:
                        col = CAPTION_Strategy;
                        break;

                    case CAP_DataSource:
                        col = CAPTION_DataSourceNameIDValue;
                        break;

                    case CAP_Position:
                        col = CAPTION_Qty;
                        break;

                    case CAP_PositionType:
                        col = CAPTION_PositionType;
                        break;

                    case CAP_TradingAccount:
                        col = CAPTION_TradingAccount;
                        break;
                    case CAP_SideName:
                        col = CAPTION_SideName;
                        break;
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return col;
        }


        private void grdPNL_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {

                if (e.Row is UltraGridGroupByRow)
                {
                    if (!e.Row.HasParent())
                    {
                        //let it to be the default black color
                        e.Row.Appearance.BackColor = _firstLevelColor;
                    }
                    else
                    {
                        ///this is the intermediate node
                        if (!e.Row.ParentRow.HasParent())
                        {
                            e.Row.Appearance.BackColor = _secondLevelColor;
                        }
                        else
                        {// this is the child node
                            e.Row.Appearance.BackColor = _thirdLevelColor;
                        }
                    }

                    e.Row.Appearance.BackGradientStyle = GradientStyle.None;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void addNewPNLViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (AddNewPNLView != null)
                {
                    AddNewPNLView(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void expandCollapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_CurrentUltraGridGroupByRow != null)
                {
                    //Selected selected = grdPNL.Selected;
                    //bool b = selected.Rows[0].Expanded;

                    if (_CurrentUltraGridGroupByRow.Expanded)
                    {
                        // Collapse if already expanded
                        //selected.Rows[0].CollapseAll();
                        _CurrentUltraGridGroupByRow.CollapseAll();
                    }
                    else
                    {
                        // Expand if already collapsed
                        //selected.Rows[0].ExpandAll();
                        _CurrentUltraGridGroupByRow.ExpandAll();

                    }
                }
                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void deleteViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DeleteViewClick != null)
                {
                    DeleteViewClick(_customViewPreference.TabName, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void renameViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (RenameViewClick != null)
                {
                    RenameViewClick(_customViewPreference.TabName, e);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        bool _MouseDownOnHeader = false;
        bool _MouseDownOnGroupBox = false;
        string _StrSelectedGroupByCol = String.Empty;

        private void grdPNL_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UIElement thisElem = this.grdPNL.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
                if (thisElem == null || !(thisElem is GroupByBoxUIElement) &&
                    !(thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement))
                {
                    if (_MouseDownOnGroupBox)
                    {
                        if (_StrSelectedGroupByCol != string.Empty)
                        {
                            //grdPNL.DisplayLayout.Bands[0].Columns[col].AllowGroupBy = DefaultableBoolean.True;
                            if (grdPNL.DisplayLayout.Bands[0].Columns[_StrSelectedGroupByCol].IsGroupByColumn)
                            {
                                _customViewPreference.GroupByColumnsCollection.Remove(_StrSelectedGroupByCol);
                                grdPNL.DisplayLayout.Bands[0].Columns[_StrSelectedGroupByCol].AllowGroupBy = DefaultableBoolean.True;
                            }
                        }
                    }
                    //remove if groupbybutton was captured
                }
                else
                {
                    if (_MouseDownOnHeader)
                    {
                        if (_StrSelectedGroupByCol != string.Empty)
                        {
                            if (this.grdPNL.DisplayLayout.Bands[0].SortedColumns.Count < 4 && _customViewPreference.GroupByColumnsCollection.Count < 3)
                            {
                                grdPNL.DisplayLayout.Bands[0].Columns[_StrSelectedGroupByCol].AllowGroupBy = DefaultableBoolean.True;
                                if (!_customViewPreference.GroupByColumnsCollection.Contains(_StrSelectedGroupByCol))
                                    _customViewPreference.GroupByColumnsCollection.Add(_StrSelectedGroupByCol);
    
                 }
                            else
                            {
                                grdPNL.DisplayLayout.Bands[0].Columns[_StrSelectedGroupByCol].AllowGroupBy = DefaultableBoolean.False;
                            }
                        }
                        //Add to the group by collection if header was captured
                    }
                }
            }
          _StrSelectedGroupByCol = String.Empty;
        }



    }
}
