using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable()]
    public class ExposureAndPnlOrderSummary
    {
        #region Private Fields

        private double _dayPnLLong;
        private double _dayPnLShort;
        private double _dayPnL;
        private double _longExposure;
        private double _shortExposure;
        private double _netExposure;
        private double _netExposureLocal;
        private double _netAssetValue;
        private int _accountID;
        private double _pnlContributionPercentageSummary;
        private double _longMarketValue;
        private double _shortMarketValue;
        private double _netMarketValue;
        private double _cashProjected;
        private double _yesterdayMarketValue;
        private double _netPosition;
        private PositionType _positionSideMV;
        private PositionType _positionSideExposure;
        private string _navString;
        private double _netPercentExposure;
        private double _yesterdayNAV;
        private double _dayReturn;
        private double _betaAdjustedExposure;
        private double _costBasisPnL;
        private double _startOfDayCash;
        private double _startOfDayAccruals;
        private double _dayAccruals;
        private double _CashInflow;
        private double _CashOutflow;
        private double _percentageYTDReturn;
        private double _percentageQTDReturn;
        private double _percentageMTDReturn;
        private double _grossMarketValue;
        private double _grossExposure;
        private double _grossExposureLocal;
        private double _averageVol20Day;
        private double _netPercentBetaAdjExposure;
        private double _netCashProjected;
        private double _netGrossMktValue;
        private double _netLongMktValue;
        private double _underlyingValueForOptions;
        private double _percentNetMarketValue;
        private double _netPercentExposureGross;
        private double _dayReturnGrossMarketValue;
        private string _underlyingSymbol;
        private string _symbol;
        private double _underlyingGrossExposure;
        private double _percentUnderlyingGrossExposure;
        private double _percentNetMarketValueGrossMV;
        private double _longDebitLimit;
        private double _shortCreditLimit;
        private double _longDebitBalance;
        private double _shortCreditBalance;
        private double _underlyingLongExposure;
        private double _underlyingShortExposure;
        private double _percentUnderlyingLongExposure;
        private double _percentUnderlyingShortExposure;
        private double _betaAdjustedGrossExposure;
        private double _betaAdjustedGrossExposureLocal;
        private double _betaAdjustedLongExposure;
        private double _betaAdjustedShortExposure;
        private double _beta;
        private double _betaAdjustedGrossExposureUnderlying;
        private double _netPercentBetaAdjustedGrossExposureUnderlying;
        private double _betaAdjustedLongExposureUnderlying;
        private double _netPercentBetaAdjustedLongExposureUnderlying;
        private double _betaAdjustedShortExposureUnderlying;
        private double _netPercentBetaAdjustedShortExposureUnderlying;
        private int _masterfundID;
        private double _mtdPnL;
        private double _qtdPnL;
        private double _ytdPnL;
        private double _dayPnLFX;
        private AssetCategory _assetID;
        private double _netPercentDayPnLShort;
        private double _netPercentDayPnLLong;
        private double _netPercentDayPnLFX;
        private double _longShortExposureRatioUnderlying;
        private double _leverageFactor;
        private double _netShortMktValue;
        private double _earnedDividendBase;
        private string _currencyID;
        private double _positionBeforeZero;
        private double _netExposureBeforeZero;
        #endregion Private Fields

        #region Properties

        [ColProperty(Caption = "Start Of Day Cash", Format = "#,##,##0", OnlyInColChooser = true)]
        public double StartOfDayCash
        {
            get { return _startOfDayCash; }
            set { _startOfDayCash = value; }
        }

        [ColProperty(Caption = "Start Of Day Accruals", Format = "#,##,##0", OnlyInColChooser = true)]
        public double StartOfDayAccruals
        {
            get { return _startOfDayAccruals; }
            set { _startOfDayAccruals = value; }
        }

        [ColProperty(Caption = "Day Accruals", Format = "#,##,##0", OnlyInColChooser = true)]
        public double DayAccruals
        {
            get { return _dayAccruals; }
            set { _dayAccruals = value; }
        }

        [ColProperty(Caption = "Cash Inflow", Format = "#,##,##0", OnlyInColChooser = true)]
        public double CashInflow
        {
            get { return _CashInflow; }
            set { _CashInflow = value; }
        }

        [ColProperty(Caption = "Cash Outflow", Format = "#,##,##0", OnlyInColChooser = true)]
        public double CashOutflow
        {
            get { return _CashOutflow; }
            set { _CashOutflow = value; }
        }

        [ColProperty(Caption = "Day P&L (Long)", Format = "#,##,##0", OnlyInColChooser = true)]
        public double DayPnLLong
        {
            get { return _dayPnLLong; }
            set { _dayPnLLong = value; }
        }

        [ColProperty(Caption = "Day P&L (Short)", Format = "#,##,##0", OnlyInColChooser = true)]
        public double DayPnLShort
        {
            get { return _dayPnLShort; }
            set { _dayPnLShort = value; }
        }

        [ColProperty(Caption = "Day P&L", Format = "#,##,##0")]
        public double DayPnL
        {
            get { return _dayPnL; }
            set { _dayPnL = value; }
        }

        [Browsable(false)]
        public double LongExposure
        {
            get { return _longExposure; }
            set { _longExposure = value; }
        }

        [Browsable(false)]
        public double ShortExposure
        {
            get { return _shortExposure; }
            set { _shortExposure = value; }
        }

        [ColProperty(Caption = "Net Exposure", Format = "#,##,##0")]
        public double NetExposure
        {
            get { return _netExposure; }
            set { _netExposure = value; }
        }

        [ColProperty(Caption = "NAV", Format = "#,##,##0")]
        public double NetAssetValue
        {
            get { return _netAssetValue; }
            set { _netAssetValue = value; }
        }

        [ColProperty(Caption = "Underlying Value (Options)", Format = "#,##,##0", OnlyInColChooser = true)]
        public double UnderlyingValueForOptions
        {
            get
            {
                return _underlyingValueForOptions;
            }
            set
            {
                _underlyingValueForOptions = value;
            }
        }

        [Browsable(false)]
        public int Level1ID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        [Browsable(false)]
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        [Browsable(false)]
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        [ColProperty(Caption = "P&L Contribution", Format = "#,0.00%")]
        public double PNLContributionPercentageSummary
        {
            get { return _pnlContributionPercentageSummary; }
            set { _pnlContributionPercentageSummary = value; }
        }

        [ColProperty(Caption = "Long Market Value", Format = "#,##,##0")]
        public double LongMarketValue
        {
            get { return _longMarketValue; }
            set { _longMarketValue = value; }
        }

        [ColProperty(Caption = "Short Market Value", Format = "#,##,##0")]
        public double ShortMarketValue
        {
            get { return _shortMarketValue; }
            set { _shortMarketValue = value; }
        }

        [ColProperty(Caption = "Net Market Value", Format = "#,##,##0")]
        public double NetMarketValue
        {
            get { return _netMarketValue; }
            set { _netMarketValue = value; }
        }

        [ColProperty(Caption = "Cash Value", Format = "#,##,##0")]
        public double CashProjected
        {
            get { return _cashProjected; }
            set { _cashProjected = value; }
        }

        [ColProperty(Caption = "Closing Market Value", Format = "#,##,##0")]
        public double YesterdayMarketValue
        {
            get { return _yesterdayMarketValue; }
            set { _yesterdayMarketValue = value; }
        }

        [Browsable(false)]
        public double NetPosition
        {
            get { return _netPosition; }
            set { _netPosition = value; }
        }

        [Browsable(false)]
        public PositionType PositionSideMV
        {
            get { return _positionSideMV; }
            set { _positionSideMV = value; }
        }

        [Browsable(false)]
        public PositionType PositionSideExposure
        {
            get { return _positionSideExposure; }
            set { _positionSideExposure = value; }
        }

        [Browsable(false)]
        public string NAVString
        {
            get { return _navString; }
            set { _navString = value; }
        }

        [ColProperty(Caption = "Net Exposure %", Format = "#,0.00%")]
        public double NetPercentExposure
        {
            get { return _netPercentExposure; }
            set { _netPercentExposure = value; }
        }

        [ColProperty(Caption = "Beta Adj. Exposure %", Format = "#,0.00%")]
        public double NetPercentBetaAdjExposure
        {
            get { return _netPercentBetaAdjExposure; }
            set { _netPercentBetaAdjExposure = value; }
        }

        [ColProperty(Caption = "Cash Value %", Format = "#,0.00%")]
        public double NetPercentCashProjected
        {
            get { return _netCashProjected; }
            set { _netCashProjected = value; }
        }

        [ColProperty(Caption = "Gross Market Value %", Format = "#,0.00%")]
        public double NetPercentGrossMktValue
        {
            get { return _netGrossMktValue; }
            set { _netGrossMktValue = value; }
        }

        [ColProperty(Caption = "Long Market Value %", Format = "#,0.00%")]
        public double NetPercentLongMktValue
        {
            get { return _netLongMktValue; }
            set { _netLongMktValue = value; }
        }

        [ColProperty(Caption = "Net Exposure % (Gross Exposure)", Format = "#,0.00%")]
        public double NetPercentExposureGross
        {
            get { return _netPercentExposureGross; }
            set { _netPercentExposureGross = value; }
        }

        [ColProperty(Caption = "Short Market Value %", Format = "#,0.00%")]
        public double NetPercentShortMktValue
        {
            get { return _netShortMktValue; }
            set { _netShortMktValue = value; }
        }

        [ColProperty(Caption = "Cost Basis P&L", Format = "#,##,##0")]
        public double CostBasisPNL
        {
            get { return _costBasisPnL; }
            set { _costBasisPnL = value; }
        }

        [ColProperty(Caption = "Start of Day NAV", Format = "#,##,##0", OnlyInColChooser = true)]
        public double YesterdayNAV
        {
            get { return _yesterdayNAV; }
            set { _yesterdayNAV = value; }
        }

        [ColProperty(Caption = "Day Return", Format = "#,0.00%", OnlyInColChooser = true)]
        public double DayReturn
        {
            get { return _dayReturn; }
            set { _dayReturn = value; }
        }

        [ColProperty(Caption = "Beta Adj. Exposure", Format = "#,##,##0", OnlyInColChooser = true)]
        public double BetaAdjustedExposure
        {
            get { return _betaAdjustedExposure; }
            set { _betaAdjustedExposure = value; }
        }

        [ColProperty(Caption = "Earned Dividend Base", Format = "#,##,##0")]
        public double EarnedDividendBase
        {
            get { return _earnedDividendBase; }
            set { _earnedDividendBase = value; }
        }

        [ColProperty(Caption = "YTD Return", Format = "#,0.00%")]
        public double YTDReturn
        {
            get { return _percentageYTDReturn; }
            set { _percentageYTDReturn = value; }
        }

        [ColProperty(Caption = "QTD Return", Format = "#,0.00%")]
        public double QTDReturn
        {
            get { return _percentageQTDReturn; }
            set { _percentageQTDReturn = value; }
        }

        [ColProperty(Caption = "MTD Return", Format = "#,0.00%")]
        public double MTDReturn
        {
            get { return _percentageMTDReturn; }
            set { _percentageMTDReturn = value; }
        }

        [ColProperty(Caption = "Gross Market Value", Format = "#,##,##0", OnlyInColChooser = true)]
        public double GrossMarketValue
        {
            get { return _grossMarketValue; }
            set { _grossMarketValue = value; }
        }

        [Browsable(false)]
        public double GrossExposure
        {
            get { return _grossExposure; }
            set { _grossExposure = value; }
        }

        public double AverageVolume20Day
        {
            get { return _averageVol20Day; }
            set { _averageVol20Day = value; }
        }

        [ColProperty(Caption = "Net Market Value %", Format = "#,0.00%", OnlyInColChooser = true)]
        public double PercentNetMarketValue
        {
            get { return _percentNetMarketValue; }
            set { _percentNetMarketValue = value; }
        }

        [ColProperty(Caption = "Net Market Value % (Gross MV)", Format = "#,0.00%", OnlyInColChooser = true)]
        public double PercentNetMarketValueGrossMV
        {
            get { return _percentNetMarketValueGrossMV; }
            set { _percentNetMarketValueGrossMV = value; }
        }

        [ColProperty(Caption = "Day Return % (Gross MV)", Format = "#,0.00%")]
        public double DayReturnGrossMarketValue
        {
            get { return _dayReturnGrossMarketValue; }
            set { _dayReturnGrossMarketValue = value; }
        }

        [ColProperty(Caption = "Gross Exposure", Format = "#,##,##0")]
        public double UnderlyingGrossExposure
        {
            get { return _underlyingGrossExposure; }
            set { _underlyingGrossExposure = value; }
        }

        [ColProperty(Caption = "Gross Exposure %", Format = "#,0.00%")]
        public double PercentUnderlyingGrossExposure
        {
            get { return _percentUnderlyingGrossExposure; }
            set { _percentUnderlyingGrossExposure = value; }
        }

        [ColProperty(Caption = "Long Debit Limit", Format = "#,##,##0", OnlyInColChooser = true)]
        public double LongDebitLimit
        {
            get { return _longDebitLimit; }
            set { _longDebitLimit = value; }
        }

        [ColProperty(Caption = "Short Credit Limit", Format = "#,##,##0", OnlyInColChooser = true)]
        public double ShortCreditLimit
        {
            get { return _shortCreditLimit; }
            set { _shortCreditLimit = value; }
        }

        [ColProperty(Caption = "Long Debit Balance", Format = "#,##,##0", OnlyInColChooser = true)]
        public double LongDebitBalance
        {
            get { return _longDebitBalance; }
            set { _longDebitBalance = value; }
        }

        [ColProperty(Caption = "Short Credit Balance", Format = "#,##,##0", OnlyInColChooser = true)]
        public double ShortCreditBalance
        {
            get { return _shortCreditBalance; }
            set { _shortCreditBalance = value; }
        }

        [ColProperty(Caption = "Long Exposure", Format = "#,##,##0")]
        public double UnderlyingLongExposure
        {
            get { return _underlyingLongExposure; }
            set { _underlyingLongExposure = value; }
        }

        [ColProperty(Caption = "Long Exposure %", Format = "#,0.00%")]
        public double PercentUnderlyingLongExposure
        {
            get { return _percentUnderlyingLongExposure; }
            set { _percentUnderlyingLongExposure = value; }
        }

        [ColProperty(Caption = "Short Exposure", Format = "#,##,##0")]
        public double UnderlyingShortExposure
        {
            get { return _underlyingShortExposure; }
            set { _underlyingShortExposure = value; }
        }

        [ColProperty(Caption = "Short Exposure %", Format = "#,0.00%")]
        public double PercentUnderlyingShortExposure
        {
            get { return _percentUnderlyingShortExposure; }
            set { _percentUnderlyingShortExposure = value; }
        }

        [Browsable(false)]
        public double BetaAdjustedGrossExposure
        {
            get { return _betaAdjustedGrossExposure; }
            set { _betaAdjustedGrossExposure = value; }
        }

        [Browsable(false)]
        public double BetaAdjustedLongExposure
        {
            get { return _betaAdjustedLongExposure; }
            set { _betaAdjustedLongExposure = value; }
        }

        [Browsable(false)]
        public double BetaAdjustedShortExposure
        {
            get { return _betaAdjustedShortExposure; }
            set { _betaAdjustedShortExposure = value; }
        }

        [Browsable(false)]
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        [ColProperty(Caption = "Beta Adj. Gross Exposure", Format = "#,##,##0", OnlyInColChooser = true)]
        public double BetaAdjustedGrossExposureUnderlying
        {
            get { return _betaAdjustedGrossExposureUnderlying; }
            set { _betaAdjustedGrossExposureUnderlying = value; }
        }

        [ColProperty(Caption = "Beta Adj. Gross Exposure %", Format = "#,0.00%", OnlyInColChooser = true)]
        public double NetPercentBetaAdjustedGrossExposureUnderlying
        {
            get { return _netPercentBetaAdjustedGrossExposureUnderlying; }
            set { _netPercentBetaAdjustedGrossExposureUnderlying = value; }
        }

        [ColProperty(Caption = "Beta Adj. Long Exposure", Format = "#,##,##0", OnlyInColChooser = true)]
        public double BetaAdjustedLongExposureUnderlying
        {
            get { return _betaAdjustedLongExposureUnderlying; }
            set { _betaAdjustedLongExposureUnderlying = value; }
        }

        [ColProperty(Caption = "Beta Adj. Long Exposure %", Format = "#,0.00%", OnlyInColChooser = true)]
        public double NetPercentBetaAdjustedLongExposureUnderlying
        {
            get { return _netPercentBetaAdjustedLongExposureUnderlying; }
            set { _netPercentBetaAdjustedLongExposureUnderlying = value; }
        }

        [ColProperty(Caption = "Beta Adj. Short Exposure", Format = "#,##,##0", OnlyInColChooser = true)]
        public double BetaAdjustedShortExposureUnderlying
        {
            get { return _betaAdjustedShortExposureUnderlying; }
            set { _betaAdjustedShortExposureUnderlying = value; }
        }

        [ColProperty(Caption = "Beta Adj. Short Exposure %", Format = "#,0.00%", OnlyInColChooser = true)]
        public double NetPercentBetaAdjustedShortExposureUnderlying
        {
            get { return _netPercentBetaAdjustedShortExposureUnderlying; }
            set { _netPercentBetaAdjustedShortExposureUnderlying = value; }
        }

        [ColProperty(Caption = "MTD P&L", Format = "#,##,##0", OnlyInColChooser = true)]
        public double MTDPnL
        {
            get { return _mtdPnL; }
            set { _mtdPnL = value; }
        }

        [ColProperty(Caption = "QTD P&L", Format = "#,##,##0", OnlyInColChooser = true)]
        public double QTDPnL
        {
            get { return _qtdPnL; }
            set { _qtdPnL = value; }
        }

        [ColProperty(Caption = "YTD P&L", Format = "#,##,##0", OnlyInColChooser = true)]
        public double YTDPnL
        {
            get { return _ytdPnL; }
            set { _ytdPnL = value; }
        }

        [Browsable(false)]
        public int MasterfundID
        {
            get { return _masterfundID; }
            set { _masterfundID = value; }
        }

        [Browsable(false)]
        public double NetExposureLocal
        {
            get { return _netExposureLocal; }
            set { _netExposureLocal = value; }
        }

        [Browsable(false)]
        public double GrossExposureLocal
        {
            get { return _grossExposureLocal; }
            set { _grossExposureLocal = value; }
        }

        [Browsable(false)]
        public double BetaAdjustedGrossExposureLocal
        {
            get { return _betaAdjustedGrossExposureLocal; }
            set { _betaAdjustedGrossExposureLocal = value; }
        }

        [Browsable(false)]
        public AssetCategory AssetCategory
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        [ColProperty(Caption = "Day P&L (FX)", Format = "#,##,##0", OnlyInColChooser = true)]
        public double DayPnLFX
        {
            get { return _dayPnLFX; }
            set { _dayPnLFX = value; }
        }

        [ColProperty(Caption = "Day P&L (Short) %", Format = "#,0.00%")]
        public double NetPercentDayPnLShort
        {
            get { return _netPercentDayPnLShort; }
            set { _netPercentDayPnLShort = value; }
        }

        [ColProperty(Caption = "Day P&L (Long) %", Format = "#,0.00%")]
        public double NetPercentDayPnLLong
        {
            get { return _netPercentDayPnLLong; }
            set { _netPercentDayPnLLong = value; }
        }

        [ColProperty(Caption = "Day P&L (FX) %", Format = "#,0.00%")]
        public double NetPercentDayPnLFX
        {
            get { return _netPercentDayPnLFX; }
            set { _netPercentDayPnLFX = value; }
        }

        [ColProperty(Caption = "Long/Short Ratio", Format = "#,##,##0.00")]
        public double LongShortExposureRatioUnderlying
        {
            get { return _longShortExposureRatioUnderlying; }
            set { _longShortExposureRatioUnderlying = value; }
        }

        [Browsable(false)]
        public double LeverageFactor
        {
            get { return _leverageFactor; }
            set { _leverageFactor = value; }
        }


        [Browsable(false)]
        public string CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        [Browsable(false)]
        public double PositionBeforeZero
        {
            get { return _positionBeforeZero; }
            set { _positionBeforeZero = value; }
        }

        [Browsable(false)]
        public double NetExposureBeforeZero
        {
            get { return _netExposureBeforeZero; }
            set { _netExposureBeforeZero = value; }
        }

        [Browsable(false)]
        public double YesterdayNetPosition { get; set; }

        [Browsable(false)]
        public double TodayNetPosition { get; set; }

        #endregion Properties

        public ExposureAndPnlOrderSummary()
        {
            _accountID = int.MinValue;
            _cashProjected = 0.0;
            _positionSideMV = PositionType.Long;
            _positionSideExposure = PositionType.Long;
            _netAssetValue = 0.0;
            _navString = string.Empty;
            _costBasisPnL = 0.0;
            _netPercentExposure = 0.0;
            _yesterdayNAV = 0.0;
            _dayReturn = 0.0;
            _betaAdjustedExposure = 0.0;
            _startOfDayCash = 0.0;
            _startOfDayAccruals = 0.0;
            _dayAccruals = 0.0;
            _CashInflow = 0.0;
            _CashOutflow = 0.0;
            _earnedDividendBase = 0.0;
            _percentageMTDReturn = 0.0;
            _percentageQTDReturn = 0.0;
            _percentageYTDReturn = 0.0;
            _grossExposure = 0.0;
            _grossMarketValue = 0.0;
            _averageVol20Day = 0.0;
            _underlyingValueForOptions = 0.0;
            _percentNetMarketValue = 0.0;
            _netPercentExposureGross = 0.0;
            _percentNetMarketValueGrossMV = 0.0;
            _longDebitLimit = 0.0;
            _shortCreditLimit = 0.0;
            _longDebitBalance = 0.0;
            _shortCreditBalance = 0.0;
            _betaAdjustedGrossExposure = 0.0;
            _betaAdjustedLongExposure = 0.0;
            _betaAdjustedShortExposure = 0.0;
            _yesterdayMarketValue = 0.0;
            _betaAdjustedGrossExposureUnderlying = 0.0;
            _betaAdjustedLongExposureUnderlying = 0.0;
            _betaAdjustedShortExposureUnderlying = 0.0;
            _netPercentBetaAdjustedGrossExposureUnderlying = 0.0;
            _netPercentBetaAdjustedLongExposureUnderlying = 0.0;
            _netPercentBetaAdjustedShortExposureUnderlying = 0.0;
            _masterfundID = int.MinValue;
            _qtdPnL = 0.0;
            _mtdPnL = 0.0;
            _ytdPnL = 0.0;
            _netExposure = 0.0;
            _netExposureLocal = 0.0;
            _grossExposureLocal = 0.0;
            _betaAdjustedGrossExposureLocal = 0.0;
            _dayPnLFX = 0.0;
            _assetID = 0;
            _netPercentDayPnLShort = 0.0;
            _netPercentDayPnLLong = 0.0;
            _netPercentDayPnLFX = 0.0;
            _longShortExposureRatioUnderlying = 0.0;
            _currencyID = string.Empty;
        }

        public ExposureAndPnlOrderSummary(ExposureAndPnlOrderSummary witholdcashvalues)
            : this()
        {
            this.CashProjected = witholdcashvalues.CashProjected;
        }
    }
}
