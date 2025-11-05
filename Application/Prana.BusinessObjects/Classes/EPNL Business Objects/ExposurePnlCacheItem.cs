using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable()]
    public class ExposurePnlCacheItem : INotifyPropertyChanged
    {
        #region Private Fields
        private const string COLORBASIS_DayPnL = "DayPnLInBaseCurrency";
        private const string COLORBASIS_OrderSide = "OrderSideTagValue";
        private int _hasBeenSenttoUser;
        private string _id;
        private string _dataSourceNameIDValue;
        private string _account;
        private string _level2Name;
        private string _symbol;
        private int _vsCurrencyID;
        private int _leadCurrencyID;
        private double _quantity;
        private double? _avgPrice;
        private string _orderSideTagValue;
        private int _auecID;
        private string _asset;
        private string _underlying;
        private string _exchange;
        private int _exchangeID;
        private int _currencyID;
        private string _currency;
        private double _multiplier;
        private double? _fxRate;
        private Operator _fxConversionMethodOperator;
        private Operator _feedPriceOperator;
        private string _vsCurrencySymbol;
        private string _leadCurrencySymbol;
        private double _bidPrice;
        private double _dividendYield;
        private double _askPrice;
        private double _lastPrice;
        private double? _closingPrice;
        private double _highPrice;
        private double _lowPrice;
        private double _midPrice;
        private double _percentageChange;
        private double _netExposure;
        private double _exposure;
        private double _deltaAdjPosition;
        private double _grossExposure;
        private double _betaAdjGrossExposure;
        private double _netExposureInBaseCurrency;
        private double _exposureInBaseCurrency;
        private double _exposureBPInBaseCurrency;
        private double _dayPnL;
        private double _dayPnLBP;
        private double _dayPnLInBaseCurrency;
        private DateTime? _tradeDate;
        private double _delta;
        private double _underlyingDelta;
        private string _sideName;
        private int _masterFundID;
        private string _masterFund;
        private int _masterStrategyID;
        private string _masterStrategy;
        private int _level1ID;
        private int _level2ID;
        private string _transactionSide;
        private string _underlyingSymbol;
        private double _underlyingStockPrice;
        private double _yesterdayMarkPrice;
        private int _sideMultiplier;
        private string _fullSecurityName;
        private string _description;
        private string _internalComments;
        private string _yesterdayMarkPriceStr;
        private string _fxRateOnTradeDateStr;
        private double _costBasisUnrealizedPnL;
        private double _dayInterest;
        private double _totalInterest;
        private double _costBasisUnrealizedPnLInBaseCurrency;
        private double _marketValue;
        private double _grossMarketValue;
        private double _marketValueInBaseCurrency;
        private double _underlyingValueForOptions;
        private double _percentageGainLoss;
        private DateTime? _lastUpdatedUTC;
        private double _beta;
        private double _cashImpact;
        private string _UDAAsset;
        private string _UDASecurityType;
        private string _UDASector;
        private string _UDASubSector;
        private string _UDACountry;
        private double _marketCapitalization;
        private double _sharesOutstanding;
        private double _averageVolume20Day;
        private double _averageVolume20DayUnderlyingSymbol;
        private DateTime? _exDividendDate;
        private double _cashImpactInBaseCurrency;
        private string _contractType;
        private DateTime? _expirationDate;
        private double _strikePrice;
        private double _betaAdjExposure;
        private double _betaAdjExposureInBaseCurrency;
        private double _volatility;
        private double _earnedDividendLocal;
        private double _earnedDividendBase;
        private string _userName;
        private string _counterPartyName;
        private double _percentageAverageVolume;
        private int _underlyingID;
        private double _selectedFeedPrice;
        private double _selectedFeedPriceInBaseCurrency;
        private DateTime? _settlementDate;
        private double _percentageAverageVolumeDeltaAdj;
        private double _tradeDayPnl;
        private double _fxDayPnl;
        private double _fxCostBasisPnl;
        private double _tradeCostBasisPnl;
        private bool _isSwap;
        private double _percentGainLossCostBasis;
        private string _idcoSymbol;
        private string _osiSymbol;
        private string _sedolSymbol;
        private string _cusipSymbol;
        private string _bloombergSymbol;
        private string _bloombergSymbolWithExchangeCode;
        private string _factSetSymbol;
        private string _activSymbol;
        private string _isinSymbol;
        private DateTime _startTradeDate;
        private double? _costBasisBreakEven;
        private string _pricingSource;
        private DeltaSource _deltaSource;
        private string _pricingStatus;
        private double _yesterdayUnderlyingMarkPrice;
        private string _yesterdayUnderlyingMarkPriceStr;
        private double _percentageUnderlyingChange;
        private string _transactionType;
        private double _yesterdayMarketValue;
        private double _yesterdayMarketValueInBaseCurrency;
        private double _yesterdayFXRate;
        private double _changeInUnderlyingPrice;
        private string _reutersSymbol;
        private string _positionSideExposureBoxed;
        private string _analyst;
        private string _countryOfRisk;
        private string _customUDA1;
        private string _customUDA2;
        private string _customUDA3;
        private string _customUDA4;
        private string _customUDA5;
        private string _customUDA6;
        private string _customUDA7;
        private string _customUDA8;
        private string _customUDA9;
        private string _customUDA10;
        private string _customUDA11;
        private string _customUDA12;
        private string _issuer;
        private string _liquidTag;
        private string _marketCap;
        private string _region;
        private string _riskCurrency;
        private string _ucitsEligibleTag;
        private double? _fxRateDisplay;
        private double _netNotionalForCostBasisBreakEven;
        private double _percentNetExposureInBaseCurrency;
        private double _percentGrossExposureInBaseCurrency;
        private double _percentExposureInBaseCurrency;
        private double _percentUnderlyingGrossExposureInBaseCurrency;
        private double _percentBetaAdjGrossExposureInBaseCurrency;
        private string _positionSideExposureUnderlying;
        private string _positionSideMV;
        private double _underlyingGrossExposureInBaseCurrency;
        private double _underlyingGrossExposure;
        private double _dayReturn;
        private double _startOfDayNAV;
        private double _percentGrossMarketValueInBaseCurrency;
        private double _betaAdjGrossExposureUnderlying;
        private double _betaAdjGrossExposureUnderlyingInBaseCurrency;
        private double _nav;
        private double _percentagePNLContribution;
        private double _percentNetMarketValueInBaseCurrency;
        private string _positionSideExposure;
        private bool _isStaleData;
        private string _tradeAttribute1 = string.Empty;
        private string _tradeAttribute2 = string.Empty;
        private string _tradeAttribute3 = string.Empty;
        private string _tradeAttribute4 = string.Empty;
        private string _tradeAttribute5 = string.Empty;
        private string _tradeAttribute6 = string.Empty;
        private string _proxySymbol;
        private double _grossExposureLocal;
        private double _percentDayPnLGrossMV;
        private double _percentDayPnLNetMV;
        private DateTime _expirationMonth;
        private double _deltaAdjPositionLME;
        private double _premium;
        private double _premiumDollar;
        private double _forwardPoints;
        private double _navTouch;
        private double _netNotionalValue;
        private double _netNotionalValueInBaseCurrency;
        /// <summary>
        /// The counter currency identifier
        /// </summary>
        private int _counterCurrencyID;
        /// <summary>
        /// The counter currency amount
        /// </summary>
        private double _counterCurrencyAmount;
        /// <summary>
        /// The counter currency cost basis pn l
        /// </summary>
        private double _counterCurrencyCostBasisPnL;
        /// <summary>
        /// The counter currency symbol
        /// </summary>
        private string _counterCurrencySymbol;

        /// <summary>
        /// The counter currency day pnl
        /// </summary>
        private double _counterCurrencyDayPnL;
        private OptionMoneyness _itmOtm;
        private double _percentOfITMOTM;
        private double _intrinsicValue;
        private int _daysToExpiry;
        private double _gainLossIfExerciseAssign;

        /// <summary>
        /// The Day Traded Position
        /// </summary>
        private double _dayTradedPosition;

        /// <summary>
        /// The Trade Volume
        /// </summary>
        private double _tradeVolume;

        private static readonly Dictionary<string, Action<ExposurePnlCacheItem, string>> _attributeSetters =
            new Dictionary<string, Action<ExposurePnlCacheItem, string>>
            {
                { "TradeAttribute7",  (obj, val) => obj.TradeAttribute7 = val },
                { "TradeAttribute8",  (obj, val) => obj.TradeAttribute8 = val },
                { "TradeAttribute9",  (obj, val) => obj.TradeAttribute9 = val },
                { "TradeAttribute10", (obj, val) => obj.TradeAttribute10 = val },
                { "TradeAttribute11", (obj, val) => obj.TradeAttribute11 = val },
                { "TradeAttribute12", (obj, val) => obj.TradeAttribute12 = val },
                { "TradeAttribute13", (obj, val) => obj.TradeAttribute13 = val },
                { "TradeAttribute14", (obj, val) => obj.TradeAttribute14 = val },
                { "TradeAttribute15", (obj, val) => obj.TradeAttribute15 = val },
                { "TradeAttribute16", (obj, val) => obj.TradeAttribute16 = val },
                { "TradeAttribute17", (obj, val) => obj.TradeAttribute17 = val },
                { "TradeAttribute18", (obj, val) => obj.TradeAttribute18 = val },
                { "TradeAttribute19", (obj, val) => obj.TradeAttribute19 = val },
                { "TradeAttribute20", (obj, val) => obj.TradeAttribute20 = val },
                { "TradeAttribute21", (obj, val) => obj.TradeAttribute21 = val },
                { "TradeAttribute22", (obj, val) => obj.TradeAttribute22 = val },
                { "TradeAttribute23", (obj, val) => obj.TradeAttribute23 = val },
                { "TradeAttribute24", (obj, val) => obj.TradeAttribute24 = val },
                { "TradeAttribute25", (obj, val) => obj.TradeAttribute25 = val },
                { "TradeAttribute26", (obj, val) => obj.TradeAttribute26 = val },
                { "TradeAttribute27", (obj, val) => obj.TradeAttribute27 = val },
                { "TradeAttribute28", (obj, val) => obj.TradeAttribute28 = val },
                { "TradeAttribute29", (obj, val) => obj.TradeAttribute29 = val },
                { "TradeAttribute30", (obj, val) => obj.TradeAttribute30 = val },
                { "TradeAttribute31", (obj, val) => obj.TradeAttribute31 = val },
                { "TradeAttribute32", (obj, val) => obj.TradeAttribute32 = val },
                { "TradeAttribute33", (obj, val) => obj.TradeAttribute33 = val },
                { "TradeAttribute34", (obj, val) => obj.TradeAttribute34 = val },
                { "TradeAttribute35", (obj, val) => obj.TradeAttribute35 = val },
                { "TradeAttribute36", (obj, val) => obj.TradeAttribute36 = val },
                { "TradeAttribute37", (obj, val) => obj.TradeAttribute37 = val },
                { "TradeAttribute38", (obj, val) => obj.TradeAttribute38 = val },
                { "TradeAttribute39", (obj, val) => obj.TradeAttribute39 = val },
                { "TradeAttribute40", (obj, val) => obj.TradeAttribute40 = val },
                { "TradeAttribute41", (obj, val) => obj.TradeAttribute41 = val },
                { "TradeAttribute42", (obj, val) => obj.TradeAttribute42 = val },
                { "TradeAttribute43", (obj, val) => obj.TradeAttribute43 = val },
                { "TradeAttribute44", (obj, val) => obj.TradeAttribute44 = val },
                { "TradeAttribute45", (obj, val) => obj.TradeAttribute45 = val },
            };

        private static readonly Dictionary<string, Func<ExposurePnlCacheItem, string>> _attributeGetters =
            new Dictionary<string, Func<ExposurePnlCacheItem, string>>
            {
                { "TradeAttribute7",  (obj) => obj.TradeAttribute7 },
                { "TradeAttribute8",  (obj) => obj.TradeAttribute8 },
                { "TradeAttribute9",  (obj) => obj.TradeAttribute9 },
                { "TradeAttribute10", (obj) => obj.TradeAttribute10 },
                { "TradeAttribute11", (obj) => obj.TradeAttribute11 },
                { "TradeAttribute12", (obj) => obj.TradeAttribute12 },
                { "TradeAttribute13", (obj) => obj.TradeAttribute13 },
                { "TradeAttribute14", (obj) => obj.TradeAttribute14 },
                { "TradeAttribute15", (obj) => obj.TradeAttribute15 },
                { "TradeAttribute16", (obj) => obj.TradeAttribute16 },
                { "TradeAttribute17", (obj) => obj.TradeAttribute17 },
                { "TradeAttribute18", (obj) => obj.TradeAttribute18 },
                { "TradeAttribute19", (obj) => obj.TradeAttribute19 },
                { "TradeAttribute20", (obj) => obj.TradeAttribute20 },
                { "TradeAttribute21", (obj) => obj.TradeAttribute21 },
                { "TradeAttribute22", (obj) => obj.TradeAttribute22 },
                { "TradeAttribute23", (obj) => obj.TradeAttribute23 },
                { "TradeAttribute24", (obj) => obj.TradeAttribute24 },
                { "TradeAttribute25", (obj) => obj.TradeAttribute25 },
                { "TradeAttribute26", (obj) => obj.TradeAttribute26 },
                { "TradeAttribute27", (obj) => obj.TradeAttribute27 },
                { "TradeAttribute28", (obj) => obj.TradeAttribute28 },
                { "TradeAttribute29", (obj) => obj.TradeAttribute29 },
                { "TradeAttribute30", (obj) => obj.TradeAttribute30 },
                { "TradeAttribute31", (obj) => obj.TradeAttribute31 },
                { "TradeAttribute32", (obj) => obj.TradeAttribute32 },
                { "TradeAttribute33", (obj) => obj.TradeAttribute33 },
                { "TradeAttribute34", (obj) => obj.TradeAttribute34 },
                { "TradeAttribute35", (obj) => obj.TradeAttribute35 },
                { "TradeAttribute36", (obj) => obj.TradeAttribute36 },
                { "TradeAttribute37", (obj) => obj.TradeAttribute37 },
                { "TradeAttribute38", (obj) => obj.TradeAttribute38 },
                { "TradeAttribute39", (obj) => obj.TradeAttribute39 },
                { "TradeAttribute40", (obj) => obj.TradeAttribute40 },
                { "TradeAttribute41", (obj) => obj.TradeAttribute41 },
                { "TradeAttribute42", (obj) => obj.TradeAttribute42 },
                { "TradeAttribute43", (obj) => obj.TradeAttribute43 },
                { "TradeAttribute44", (obj) => obj.TradeAttribute44 },
                { "TradeAttribute45", (obj) => obj.TradeAttribute45 },
            };
        #endregion Private Fields

        #region Public Fields
        [Browsable(false)]
        public int HasBeenSentToUser
        {
            get { return _hasBeenSenttoUser; }
            set
            {
                _hasBeenSenttoUser = value;
            }
        }
        [Browsable(false)]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string DataSourceNameIDValue
        {
            get { return _dataSourceNameIDValue; }
            set { _dataSourceNameIDValue = value; }
        }
        public string Level1Name
        {
            get { return _account; }
            set
            {
                _account = value;
            }
        }
        public string Level2Name
        {
            get { return _level2Name; }
            set { _level2Name = value; }
        }
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        [Browsable(false)]
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }
        [Browsable(false)]
        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public double? AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set
            {
                _orderSideTagValue = value;
            }
        }
        [Browsable(false)]
        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
            }
        }
        public string Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }
        public string Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }
        [Browsable(false)]
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }
        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }
        [Browsable(false)]
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }
        public string CurrencySymbol
        {
            get { return _currency; }
            set { _currency = value; }
        }
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        /// <summary>
        /// Changed this field to string from earlier double value and it will contain the 117 or 1/117 based on the FXConversionMethodOperator
        /// property
        /// </summary>
        public double? FxRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }
        /// <summary>
        /// Can hold divide or multiply. important for fx trades. It is better to assign this operator before assigning fxRate
        /// FXRate is for "VsCurrency - BaseCurrency". Special case for FX.  For non-fx trades While in other cases FXRate is "Symbol - BaseCurrency"
        /// </summary>
        [Browsable(false)]
        public Operator FXConversionMethodOperator
        {
            get { return _fxConversionMethodOperator; }
            set { _fxConversionMethodOperator = value; }
        }
        /// <summary>
        /// Feed price operator is different than FXConversionMethodOperator, because for FX feed price is - "Symbol - VsCurrency" while
        /// FXRate is for "VsCurrency - BaseCurrency". In other cases we get feed price in the traded currency
        /// </summary>
        [Browsable(false)]
        public Operator FeedPriceOperator
        {
            get { return _feedPriceOperator; }
            set { _feedPriceOperator = value; }
        }
        /// <summary>
        /// Currency symbol for Vs Currency
        /// </summary>
        public string VsCurrencySymbol
        {
            get { return _vsCurrencySymbol; }
            set { _vsCurrencySymbol = value; }
        }
        public string LeadCurrencySymbol
        {
            get { return _leadCurrencySymbol; }
            set { _leadCurrencySymbol = value; }
        }
        public double AskPrice
        {
            get { return _askPrice; }
            set { _askPrice = value; }
        }
        public double BidPrice
        {
            get { return _bidPrice; }
            set { _bidPrice = value; }
        }
        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }
        public double LastPrice
        {
            get { return _lastPrice; }
            set
            {
                _lastPrice = value;
            }
        }
        public double? ClosingPrice
        {
            get { return _closingPrice; }
            set { _closingPrice = value; }
        }
        [Browsable(false)]
        public double HighPrice
        {
            get { return _highPrice; }
            set { _highPrice = value; }
        }
        [Browsable(false)]
        public double LowPrice
        {
            get { return _lowPrice; }
            set { _lowPrice = value; }
        }
        public double MidPrice
        {
            get { return _midPrice; }
            set { _midPrice = value; }
        }
        public double PercentageChange
        {
            get { return _percentageChange; }
            set { _percentageChange = value; }
        }
        public double NetExposure
        {
            get { return _netExposure; }
            set { _netExposure = value; }
        }
        public double Exposure
        {
            get { return _exposure; }
            set { _exposure = value; }
        }
        public double DeltaAdjPosition
        {
            get { return _deltaAdjPosition; }
            set { _deltaAdjPosition = value; }
        }
        public double NetExposureInBaseCurrency
        {
            get { return _netExposureInBaseCurrency; }
            set { _netExposureInBaseCurrency = value; }
        }
        public double ExposureInBaseCurrency
        {
            get { return _exposureInBaseCurrency; }
            set { _exposureInBaseCurrency = value; }
        }
        public double BetaAdjExposure
        {
            get { return _betaAdjExposure; }
            set { _betaAdjExposure = value; }
        }
        public double BetaAdjExposureInBaseCurrency
        {
            get { return _betaAdjExposureInBaseCurrency; }
            set { _betaAdjExposureInBaseCurrency = value; }
        }
        public double ExposureBPInBaseCurrency
        {
            get { return _exposureBPInBaseCurrency; }
            set { _exposureBPInBaseCurrency = value; }
        }
        public double DayPnL
        {
            get { return _dayPnL; }
            set { _dayPnL = value; }
        }
        [Browsable(false)]
        public double DayPnLBP
        {
            get { return _dayPnLBP; }
            set { _dayPnLBP = value; }
        }
        public double DayPnLInBaseCurrency
        {
            get { return _dayPnLInBaseCurrency; }
            set { _dayPnLInBaseCurrency = value; }
        }
        [Browsable(false)]
        public bool IsSwap
        {
            get { return _isSwap; }
            set { _isSwap = value; }
        }
        public DateTime? TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }
        public double LeveragedFactor
        {
            get { return _underlyingDelta; }
            set { _underlyingDelta = value; }
        }
        public string SideName
        {
            get { return _sideName; }
            set { _sideName = value; }
        }
        [Browsable(false)]
        public int MasterFundID
        {
            get { return _masterFundID; }
            set { _masterFundID = value; }
        }
        public string MasterFund
        {
            get { return _masterFund; }
            set { _masterFund = value; }
        }
        [Browsable(false)]
        public int MasterStrategyID
        {
            get { return _masterStrategyID; }
            set { _masterStrategyID = value; }
        }
        public string MasterStrategy
        {
            get { return _masterStrategy; }
            set { _masterStrategy = value; }
        }
        [Browsable(false)]
        public int Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }
        [Browsable(false)]
        public int Level2ID
        {
            get { return _level2ID; }
            set { _level2ID = value; }
        }
        public string TransactionSide
        {
            get { return _transactionSide; }
            set { _transactionSide = value; }
        }
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }
        public double UnderlyingStockPrice
        {
            get { return _underlyingStockPrice; }
            set { _underlyingStockPrice = value; }
        }
        [Browsable(false)]
        public double YesterdayMarkPrice
        {
            get { return _yesterdayMarkPrice; }
            set { _yesterdayMarkPrice = value; }
        }
        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }
        public string FullSecurityName
        {
            get { return _fullSecurityName; }
            set { _fullSecurityName = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }

        /// <summary>
        /// Gets or sets the counter currency symbol.
        /// </summary>
        /// <value>
        /// The counter currency symbol.
        /// </value>
        public string CounterCurrencySymbol
        {
            get { return _counterCurrencySymbol; }
            set { _counterCurrencySymbol = value; }
        }

        /// <summary>
        /// Gets or sets the counter currency day pn l.
        /// </summary>
        /// <value>
        /// The counter currency day pn l.
        /// </value>
        public double CounterCurrencyDayPnL
        {
            get { return _counterCurrencyDayPnL; }
            set { _counterCurrencyDayPnL = value; }
        }

        /// <summary>
        /// Gets or sets the itm otm.
        /// </summary>
        /// <value>
        /// The itm otm.
        /// </value>
        public OptionMoneyness ItmOtm
        {
            get { return _itmOtm; }
            set { _itmOtm = value; }
        }

        /// <summary>
        /// Gets or sets the percent of underlying price.
        /// </summary>
        /// <value>
        /// The percent of underlying price.
        /// </value>
        public double PercentOfITMOTM
        {
            get { return _percentOfITMOTM; }
            set { _percentOfITMOTM = value; }
        }

        /// <summary>
        /// Gets or sets the intrinsic value.
        /// </summary>
        /// <value>
        /// The intrinsic value.
        /// </value>
        public double IntrinsicValue
        {
            get { return _intrinsicValue; }
            set { _intrinsicValue = value; }
        }

        /// <summary>
        /// Gets or sets the days to expiry.
        /// </summary>
        /// <value>
        /// The days to expiry.
        /// </value>
        public int DaysToExpiry
        {
            get { return _daysToExpiry; }
            set { _daysToExpiry = value; }
        }

        /// <summary>
        /// Gets or sets the gain loss if exercise assign.
        /// </summary>
        /// <value>
        /// The gain loss if exercise assign.
        /// </value>
        public double GainLossIfExerciseAssign
        {
            get { return _gainLossIfExerciseAssign; }
            set { _gainLossIfExerciseAssign = value; }
        }

        /// <summary>
        /// Added 13 May 08, To Keep track on the yesterday mark price,
        /// if yesterday mark price do not get for the required date then append * else required date value
        /// </summary>
        public string YesterdayMarkPriceStr
        {
            get
            {

                if (string.IsNullOrEmpty(_yesterdayMarkPriceStr))
                    return "Undefined";
                else
                {
                    return _yesterdayMarkPriceStr;
                }
            }
            set { _yesterdayMarkPriceStr = value; }
        }
        /// <summary>
        /// appended * means old value else required date value.
        /// </summary>
        public string FXRateOnTradeDateStr
        {
            get { return _fxRateOnTradeDateStr; }
            set
            {
                _fxRateOnTradeDateStr = value;
            }
        }
        public double CostBasisUnrealizedPnL
        {
            get { return _costBasisUnrealizedPnL; }
            set { _costBasisUnrealizedPnL = value; }
        }
        public double DayInterest
        {
            get { return _dayInterest; }
            set { _dayInterest = value; }
        }
        public double TotalInterest
        {
            get { return _totalInterest; }
            set { _totalInterest = value; }
        }
        public double CostBasisUnrealizedPnLInBaseCurrency
        {
            get { return _costBasisUnrealizedPnLInBaseCurrency; }
            set { _costBasisUnrealizedPnLInBaseCurrency = value; }
        }
        public double MarketValue
        {
            get { return _marketValue; }
            set { _marketValue = value; }
        }
        public double MarketValueInBaseCurrency
        {
            get { return _marketValueInBaseCurrency; }
            set { _marketValueInBaseCurrency = value; }
        }
        public double PercentageGainLoss
        {
            get { return _percentageGainLoss; }
            set { _percentageGainLoss = value; }
        }
        public DateTime? LastUpdatedUTC
        {
            get { return _lastUpdatedUTC; }
            set
            {
                if (String.Compare(value.ToString(), DateTimeConstants.DateTimeMinVal, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    _lastUpdatedUTC = value;
                }
            }
        }
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }
        public double CashImpact
        {
            get { return _cashImpact; }
            set { _cashImpact = value; }
        }
        public string UDAAsset
        {
            get { return _UDAAsset; }
            set { _UDAAsset = value; }
        }
        public string UDASecurityType
        {
            get { return _UDASecurityType; }
            set { _UDASecurityType = value; }
        }
        public string UDASector
        {
            get { return _UDASector; }
            set { _UDASector = value; }
        }
        public string UDASubSector
        {
            get { return _UDASubSector; }
            set { _UDASubSector = value; }
        }
        public string UDACountry
        {
            get { return _UDACountry; }
            set { _UDACountry = value; }
        }
        public double MarketCapitalization
        {
            get { return _marketCapitalization; }
            set { _marketCapitalization = value; }
        }
        public double SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set { _sharesOutstanding = value; }
        }
        public double AverageVolume20Day
        {
            get { return _averageVolume20Day; }
            set { _averageVolume20Day = value; }
        }
        public double PercentageAverageVolumeDeltaAdjusted
        {
            get { return _percentageAverageVolumeDeltaAdj; }
            set { _percentageAverageVolumeDeltaAdj = value; }
        }
        public DateTime? ExDividendDate
        {
            get { return _exDividendDate; }
            set { _exDividendDate = value; }
        }
        public double CashImpactInBaseCurrency
        {
            get { return _cashImpactInBaseCurrency; }
            set { _cashImpactInBaseCurrency = value; }
        }
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string CounterPartyName
        {
            get { return _counterPartyName; }
            set { _counterPartyName = value; }
        }
        public double EarnedDividendLocal
        {
            get { return _earnedDividendLocal; }
            set { _earnedDividendLocal = value; }
        }
        public double EarnedDividendBase
        {
            get { return _earnedDividendBase; }
            set { _earnedDividendBase = value; }
        }
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }
        public DateTime? ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                _expirationDate = value;
                DateTime tempDate;
                if (DateTime.TryParse(_expirationDate.ToString(), out tempDate))
                {
                    _expirationMonth = new DateTime(tempDate.Year, tempDate.Month, 01);
                }
            }
        }
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        public double GrossExposure
        {
            get { return _grossExposure; }
            set { _grossExposure = value; }
        }
        public double BetaAdjGrossExposure
        {
            get { return _betaAdjGrossExposure; }
            set { _betaAdjGrossExposure = value; }
        }
        public double GrossMarketValue
        {
            get { return _grossMarketValue; }
            set { _grossMarketValue = value; }
        }
        public double SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }
        public double SelectedFeedPriceInBaseCurrency
        {
            get { return _selectedFeedPriceInBaseCurrency; }
            set { _selectedFeedPriceInBaseCurrency = value; }
        }
        public DateTime? SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }
        public double PercentageAverageVolume
        {
            get { return _percentageAverageVolume; }
            set { _percentageAverageVolume = value; }
        }
        public double TradeDayPnl
        {
            get { return _tradeDayPnl; }
            set { _tradeDayPnl = value; }
        }
        public double FxDayPnl
        {
            get { return _fxDayPnl; }
            set { _fxDayPnl = value; }
        }
        public double FxCostBasisPnl
        {
            get { return _fxCostBasisPnl; }
            set { _fxCostBasisPnl = value; }
        }
        public double TradeCostBasisPnl
        {
            get { return _tradeCostBasisPnl; }
            set { _tradeCostBasisPnl = value; }
        }
        [Browsable(false)]
        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }
        public double PercentageGainLossCostBasis
        {
            get { return _percentGainLossCostBasis; }
            set { _percentGainLossCostBasis = value; }
        }
        public double UnderlyingValueForOptions
        {
            get { return _underlyingValueForOptions; }
            set { _underlyingValueForOptions = value; }
        }
        public double AverageVolume20DayUnderlyingSymbol
        {
            get { return _averageVolume20DayUnderlyingSymbol; }
            set { _averageVolume20DayUnderlyingSymbol = value; }
        }
        public bool IsStaleData
        {
            get { return _isStaleData; }
            set { _isStaleData = value; }
        }
        public string IdcoSymbol
        {
            get { return _idcoSymbol; }
            set { _idcoSymbol = value; }
        }
        public string OsiSymbol
        {
            get { return _osiSymbol; }
            set { _osiSymbol = value; }
        }
        public string SedolSymbol
        {
            get { return _sedolSymbol; }
            set { _sedolSymbol = value; }
        }
        public string CusipSymbol
        {
            get { return _cusipSymbol; }
            set { _cusipSymbol = value; }
        }
        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }
        public string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set { _bloombergSymbolWithExchangeCode = value; }
        }
        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }
        public string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }
        public string IsinSymbol
        {
            get { return _isinSymbol; }
            set { _isinSymbol = value; }
        }
        public DateTime StartTradeDate
        {
            get { return _startTradeDate; }
            set { _startTradeDate = value; }
        }
        public double? CostBasisBreakEven
        {
            get { return _costBasisBreakEven; }
            set { _costBasisBreakEven = value; }
        }
        public virtual string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }
        public virtual string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }
        public virtual string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }
        public virtual string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }
        public virtual string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }
        public virtual string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }
        public virtual string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }
        public string PricingSource
        {
            get { return _pricingSource; }
            set { _pricingSource = value; }
        }
        public DeltaSource DeltaSource
        {
            get { return _deltaSource; }
            set { _deltaSource = value; }
        }
        public string PricingStatus
        {
            get { return _pricingStatus; }
            set { _pricingStatus = value; }
        }
        public double GrossExposureLocal
        {
            get { return _grossExposureLocal; }
            set { _grossExposureLocal = value; }
        }
        public double PercentDayPnLGrossMV
        {
            get { return _percentDayPnLGrossMV; }
            set { _percentDayPnLGrossMV = value; }
        }
        public double PercentDayPnLNetMV
        {
            get { return _percentDayPnLNetMV; }
            set { _percentDayPnLNetMV = value; }
        }
        public DateTime ExpirationMonth
        {
            get { return _expirationMonth; }
            set { _expirationMonth = value; }
        }
        public double DeltaAdjPositionLME
        {
            get { return _deltaAdjPositionLME; }
            set { _deltaAdjPositionLME = value; }
        }
        public double Premium
        {
            get { return _premium; }
            set { _premium = value; }
        }
        public double PremiumDollar
        {
            get { return _premiumDollar; }
            set { _premiumDollar = value; }
        }
        public double ForwardPoints
        {
            get { return _forwardPoints; }
            set { _forwardPoints = value; }
        }
        [Browsable(false)]
        public double YesterdayUnderlyingMarkPrice
        {
            get { return _yesterdayUnderlyingMarkPrice; }
            set { _yesterdayUnderlyingMarkPrice = value; }
        }
        public string YesterdayUnderlyingMarkPriceStr
        {
            get
            {
                if (string.IsNullOrEmpty(_yesterdayUnderlyingMarkPriceStr))
                    return "Undefined";
                else
                    return _yesterdayUnderlyingMarkPriceStr;
            }
            set { _yesterdayUnderlyingMarkPriceStr = value; }
        }
        public double PercentageUnderlyingChange
        {
            get { return _percentageUnderlyingChange; }
            set { _percentageUnderlyingChange = value; }
        }
        public string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }
        public double YesterdayMarketValue
        {
            get { return _yesterdayMarketValue; }
            set { _yesterdayMarketValue = value; }
        }
        public double YesterdayMarketValueInBaseCurrency
        {
            get { return _yesterdayMarketValueInBaseCurrency; }
            set { _yesterdayMarketValueInBaseCurrency = value; }
        }
        public double YesterdayFXRate
        {
            get { return _yesterdayFXRate; }
            set { _yesterdayFXRate = value; }
        }
        public double ChangeInUnderlyingPrice
        {
            get { return _changeInUnderlyingPrice; }
            set { _changeInUnderlyingPrice = value; }
        }
        public string ReutersSymbol
        {
            get { return _reutersSymbol; }
            set { _reutersSymbol = value; }
        }
        public string PositionSideExposureBoxed
        {
            get { return _positionSideExposureBoxed; }
            set { _positionSideExposureBoxed = value; }
        }
        public string Analyst
        {
            get { return _analyst; }
            set { _analyst = value; }
        }
        public string CountryOfRisk
        {
            get { return _countryOfRisk; }
            set { _countryOfRisk = value; }
        }
        public string CustomUDA1
        {
            get { return _customUDA1; }
            set { _customUDA1 = value; }
        }
        public string CustomUDA2
        {
            get { return _customUDA2; }
            set { _customUDA2 = value; }
        }
        public string CustomUDA3
        {
            get { return _customUDA3; }
            set { _customUDA3 = value; }
        }
        public string CustomUDA4
        {
            get { return _customUDA4; }
            set { _customUDA4 = value; }
        }
        public string CustomUDA5
        {
            get { return _customUDA5; }
            set { _customUDA5 = value; }
        }
        public string CustomUDA6
        {
            get { return _customUDA6; }
            set { _customUDA6 = value; }
        }
        public string CustomUDA7
        {
            get { return _customUDA7; }
            set { _customUDA7 = value; }
        }
        public string CustomUDA8
        {
            get { return _customUDA8; }
            set { _customUDA8 = value; }
        }
        public string CustomUDA9
        {
            get { return _customUDA9; }
            set { _customUDA9 = value; }
        }
        public string CustomUDA10
        {
            get { return _customUDA10; }
            set { _customUDA10 = value; }
        }
        public string CustomUDA11
        {
            get { return _customUDA11; }
            set { _customUDA11 = value; }
        }
        public string CustomUDA12
        {
            get { return _customUDA12; }
            set { _customUDA12 = value; }
        }
        public string Issuer
        {
            get { return _issuer; }
            set { _issuer = value; }
        }
        public string LiquidTag
        {
            get { return _liquidTag; }
            set { _liquidTag = value; }
        }
        public string MarketCap
        {
            get { return _marketCap; }
            set { _marketCap = value; }
        }
        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }
        public string RiskCurrency
        {
            get { return _riskCurrency; }
            set { _riskCurrency = value; }
        }
        public string UcitsEligibleTag
        {
            get { return _ucitsEligibleTag; }
            set { _ucitsEligibleTag = value; }
        }
        public double? FxRateDisplay
        {
            get { return _fxRateDisplay; }
            set { _fxRateDisplay = value; }
        }
        public double NetNotionalForCostBasisBreakEven
        {
            get { return _netNotionalForCostBasisBreakEven; }
            set { _netNotionalForCostBasisBreakEven = value; }
        }
        public double PercentNetExposureInBaseCurrency
        {
            get { return _percentNetExposureInBaseCurrency; }
            set { _percentNetExposureInBaseCurrency = value; }
        }
        public double PercentGrossExposureInBaseCurrency
        {
            get { return _percentGrossExposureInBaseCurrency; }
            set { _percentGrossExposureInBaseCurrency = value; }
        }
        public double PercentExposureInBaseCurrency
        {
            get { return _percentExposureInBaseCurrency; }
            set { _percentExposureInBaseCurrency = value; }
        }
        public double PercentUnderlyingGrossExposureInBaseCurrency
        {
            get { return _percentUnderlyingGrossExposureInBaseCurrency; }
            set { _percentUnderlyingGrossExposureInBaseCurrency = value; }
        }
        public double PercentBetaAdjGrossExposureInBaseCurrency
        {
            get { return _percentBetaAdjGrossExposureInBaseCurrency; }
            set { _percentBetaAdjGrossExposureInBaseCurrency = value; }
        }
        public string PositionSideExposureUnderlying
        {
            get { return _positionSideExposureUnderlying; }
            set
            {
                _positionSideExposureUnderlying = value;
            }
        }
        public double UnderlyingGrossExposureInBaseCurrency
        {
            get { return _underlyingGrossExposureInBaseCurrency; }
            set { _underlyingGrossExposureInBaseCurrency = value; }
        }
        public double UnderlyingGrossExposure
        {
            get { return _underlyingGrossExposure; }
            set { _underlyingGrossExposure = value; }
        }
        public double DayReturn
        {
            get { return _dayReturn; }
            set { _dayReturn = value; }
        }
        public double StartOfDayNAV
        {
            get { return _startOfDayNAV; }
            set { _startOfDayNAV = value; }
        }
        public double PercentGrossMarketValueInBaseCurrency
        {
            get { return _percentGrossMarketValueInBaseCurrency; }
            set { _percentGrossMarketValueInBaseCurrency = value; }
        }
        public double BetaAdjGrossExposureUnderlying
        {
            get { return _betaAdjGrossExposureUnderlying; }
            set { _betaAdjGrossExposureUnderlying = value; }
        }
        public double BetaAdjGrossExposureUnderlyingInBaseCurrency
        {
            get { return _betaAdjGrossExposureUnderlyingInBaseCurrency; }
            set { _betaAdjGrossExposureUnderlyingInBaseCurrency = value; }
        }
        public double NAV
        {
            get { return _nav; }
            set { _nav = value; }
        }
        public double PercentagePNLContribution
        {
            get { return _percentagePNLContribution; }
            set { _percentagePNLContribution = value; }
        }
        public double PercentNetMarketValueInBaseCurrency
        {
            get { return _percentNetMarketValueInBaseCurrency; }
            set { _percentNetMarketValueInBaseCurrency = value; }
        }
        public string PositionSideExposure
        {
            get { return _positionSideExposure; }
            set
            {
                _positionSideExposure = value;
            }
        }
        public string PositionSideMV
        {
            get { return _positionSideMV; }
            set
            {
                _positionSideMV = value;
            }
        }
        public double NavTouch
        {
            get { return _navTouch; }
            set { _navTouch = value; }
        }

        //[Browsable(false)]
        public double NetNotionalValue
        {
            get { return _netNotionalValue; }
            set { _netNotionalValue = value; }
        }
        // [Browsable(false)]
        public double NetNotionalValueInBaseCurrency
        {
            get { return _netNotionalValueInBaseCurrency; }
            set { _netNotionalValueInBaseCurrency = value; }
        }

        /// <summary>
        /// Gets or sets the counter currency identifier.
        /// </summary>
        /// <value>
        /// The counter currency identifier.
        /// </value>
        public int CounterCurrencyID
        {
            get { return _counterCurrencyID; }
            set { _counterCurrencyID = value; }
        }

        /// <summary>
        /// Gets or sets the counter currency amount.
        /// </summary>
        /// <value>
        /// The counter currency amount.
        /// </value>
        public double CounterCurrencyAmount
        {
            get { return _counterCurrencyAmount; }
            set { _counterCurrencyAmount = value; }
        }

        /// <summary>
        /// Gets or sets the counter currency cost basis pnl.
        /// </summary>
        /// <value>
        /// The counter currency cost basis pnl.
        /// </value>
        public double CounterCurrencyCostBasisPnL
        {
            get { return _counterCurrencyCostBasisPnL; }
            set { _counterCurrencyCostBasisPnL = value; }
        }

        public double DayTradedPosition
        {
            get { return _dayTradedPosition; }
            set { _dayTradedPosition = value; }
        }

        public double TradeVolume
        {
            get { return _tradeVolume; }
            set { _tradeVolume = value; }
        }

        #region Additional Trade Attributes

        public virtual string TradeAttribute7 { get; set; } = string.Empty;
        public virtual string TradeAttribute8 { get; set; } = string.Empty;
        public virtual string TradeAttribute9 { get; set; } = string.Empty;
        public virtual string TradeAttribute10 { get; set; } = string.Empty;
        public virtual string TradeAttribute11 { get; set; } = string.Empty;
        public virtual string TradeAttribute12 { get; set; } = string.Empty;
        public virtual string TradeAttribute13 { get; set; } = string.Empty;
        public virtual string TradeAttribute14 { get; set; } = string.Empty;
        public virtual string TradeAttribute15 { get; set; } = string.Empty;
        public virtual string TradeAttribute16 { get; set; } = string.Empty;
        public virtual string TradeAttribute17 { get; set; } = string.Empty;
        public virtual string TradeAttribute18 { get; set; } = string.Empty;
        public virtual string TradeAttribute19 { get; set; } = string.Empty;
        public virtual string TradeAttribute20 { get; set; } = string.Empty;
        public virtual string TradeAttribute21 { get; set; } = string.Empty;
        public virtual string TradeAttribute22 { get; set; } = string.Empty;
        public virtual string TradeAttribute23 { get; set; } = string.Empty;
        public virtual string TradeAttribute24 { get; set; } = string.Empty;
        public virtual string TradeAttribute25 { get; set; } = string.Empty;
        public virtual string TradeAttribute26 { get; set; } = string.Empty;
        public virtual string TradeAttribute27 { get; set; } = string.Empty;
        public virtual string TradeAttribute28 { get; set; } = string.Empty;
        public virtual string TradeAttribute29 { get; set; } = string.Empty;
        public virtual string TradeAttribute30 { get; set; } = string.Empty;
        public virtual string TradeAttribute31 { get; set; } = string.Empty;
        public virtual string TradeAttribute32 { get; set; } = string.Empty;
        public virtual string TradeAttribute33 { get; set; } = string.Empty;
        public virtual string TradeAttribute34 { get; set; } = string.Empty;
        public virtual string TradeAttribute35 { get; set; } = string.Empty;
        public virtual string TradeAttribute36 { get; set; } = string.Empty;
        public virtual string TradeAttribute37 { get; set; } = string.Empty;
        public virtual string TradeAttribute38 { get; set; } = string.Empty;
        public virtual string TradeAttribute39 { get; set; } = string.Empty;
        public virtual string TradeAttribute40 { get; set; } = string.Empty;
        public virtual string TradeAttribute41 { get; set; } = string.Empty;
        public virtual string TradeAttribute42 { get; set; } = string.Empty;
        public virtual string TradeAttribute43 { get; set; } = string.Empty;
        public virtual string TradeAttribute44 { get; set; } = string.Empty;
        public virtual string TradeAttribute45 { get; set; } = string.Empty;

        #endregion Additional Trade Attributes

        #endregion Public Fields

        #region INotifyPropertyChanged Members
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IgnorePropertyChangeEvents { get; set; }

        public void RaisePropertyChangedEvent(string propertyName)
        {
            // Exit if changes ignored
            if (IgnorePropertyChangeEvents) return;

            // Exit if no subscribers
            if (PropertyChanged == null) return;

            // Raise event
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, e);
        }

        public void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }
        #endregion INotifyPropertyChanged Members

        public ExposurePnlCacheItem()
        {
            _hasBeenSenttoUser = 1;
            _dataSourceNameIDValue = string.Empty;
            _account = string.Empty;
            _level2Name = string.Empty;
            _auecID = 0;
            _asset = string.Empty;
            _underlying = string.Empty;
            _exchange = string.Empty;
            _currency = string.Empty;
            _multiplier = 0.0;
            _fxRate = 0.0;
            _bidPrice = 0.0;
            _dividendYield = 0.0;
            _askPrice = 0.0;
            _lastPrice = 0.0;
            _closingPrice = 0.0;
            _highPrice = 0.0;
            _lowPrice = 0.0;
            _midPrice = 0.0;
            _percentageChange = 0.0;
            _netExposure = 0.0;
            _exposure = 0.0;
            _deltaAdjPosition = 0.0;
            _grossExposure = 0.0;
            _betaAdjGrossExposure = 0.0;
            _netExposureInBaseCurrency = 0;
            _exposureInBaseCurrency = 0;
            _dayPnL = 0;
            _dayPnLBP = 0;
            _dayPnLInBaseCurrency = 0;
            _tradeDate = null;
            _delta = 0.0;
            _sideName = string.Empty;
            _level1ID = 0;
            _level2ID = 0;
            _underlyingSymbol = string.Empty;
            _underlyingStockPrice = 0.0;
            _underlyingValueForOptions = 0.0;
            _yesterdayMarkPrice = 0.0;
            _fullSecurityName = string.Empty;
            _volatility = 0.0;
            _id = string.Empty;
            _symbol = string.Empty;
            _vsCurrencyID = 0;
            _leadCurrencyID = 0;
            _quantity = 0;
            _avgPrice = 0.0;
            _orderSideTagValue = string.Empty;
            _currencyID = 0;
            _fxConversionMethodOperator = Operator.M;
            _feedPriceOperator = Operator.M;
            _vsCurrencySymbol = string.Empty;
            _leadCurrencySymbol = string.Empty;
            _exposureBPInBaseCurrency = 0;
            _masterFundID = int.MinValue;
            _masterFund = string.Empty;
            _masterStrategyID = int.MinValue;
            _masterStrategy = string.Empty;
            _underlyingDelta = 1.0;
            _sideMultiplier = 1;
            _description = string.Empty;
            _internalComments = string.Empty;
            _yesterdayMarkPriceStr = string.Empty;
            _fxRateOnTradeDateStr = string.Empty;
            _costBasisUnrealizedPnL = 0.0;
            _dayInterest = 0.0;
            _totalInterest = 0.0;
            _costBasisUnrealizedPnLInBaseCurrency = 0.0;
            _marketValue = 0.0;
            _grossMarketValue = 0.0;
            _marketValueInBaseCurrency = 0.0;
            _percentageGainLoss = 0.0;
            _lastUpdatedUTC = null;
            _beta = 0.0;
            _UDAAsset = string.Empty;
            _UDASecurityType = string.Empty;
            _UDASector = string.Empty;
            _UDASubSector = string.Empty;
            _UDACountry = string.Empty;
            _earnedDividendLocal = 0.0;
            _earnedDividendBase = 0.0;
            _cashImpact = 0.0;
            _marketCapitalization = 0.0;
            _sharesOutstanding = 0.0;
            _averageVolume20Day = 0.0;
            _averageVolume20DayUnderlyingSymbol = 0.0;
            _exDividendDate = null;
            _cashImpactInBaseCurrency = 0.0;
            _transactionSide = string.Empty;
            _betaAdjExposure = 0.0;
            _betaAdjExposureInBaseCurrency = 0.0;
            _userName = string.Empty;
            _counterPartyName = string.Empty;
            _contractType = string.Empty;
            _expirationDate = null;
            _percentageAverageVolume = 0.0;
            _selectedFeedPrice = 0.0;
            _selectedFeedPriceInBaseCurrency = 0.0;
            _settlementDate = null;
            _percentageAverageVolumeDeltaAdj = 0.0;
            _tradeDayPnl = 0.0;
            _fxDayPnl = 0.0;
            _fxCostBasisPnl = 0.0;
            _tradeCostBasisPnl = 0.0;
            _percentGainLossCostBasis = 0.0;
            _idcoSymbol = string.Empty;
            _osiSymbol = string.Empty;
            _sedolSymbol = string.Empty;
            _cusipSymbol = string.Empty;
            _bloombergSymbol = string.Empty;
            _bloombergSymbolWithExchangeCode = string.Empty;
            _isinSymbol = string.Empty;
            _startTradeDate = DateTimeConstants.MinValue;
            _costBasisBreakEven = 0.0;
            _proxySymbol = string.Empty;
            _pricingSource = AppConstants.PricingSource.None.ToString();
            _pricingStatus = AppConstants.PricingStatus.None.ToString();
            _percentDayPnLGrossMV = 0.0;
            _percentDayPnLNetMV = 0.0;
            _grossExposureLocal = 0.0;
            _expirationMonth = DateTimeConstants.MinValue;
            _deltaAdjPositionLME = 0.0;
            _premium = 0.0;
            _premiumDollar = 0.0;
            _forwardPoints = 0.0;
            _deltaSource = DeltaSource.Default;
            _yesterdayUnderlyingMarkPrice = 0.0;
            _yesterdayUnderlyingMarkPriceStr = string.Empty;
            _percentageUnderlyingChange = 0.0;
            _transactionType = string.Empty;
            _yesterdayMarketValue = 0.0;
            _yesterdayMarketValueInBaseCurrency = 0.0;
            _yesterdayFXRate = 0.0;
            _changeInUnderlyingPrice = 0.0;
            _reutersSymbol = string.Empty;
            _positionSideExposureBoxed = string.Empty;
            _analyst = ApplicationConstants.CONST_UNDEFINED;
            _countryOfRisk = ApplicationConstants.CONST_UNDEFINED;
            _customUDA1 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA2 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA3 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA4 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA5 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA6 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA7 = ApplicationConstants.CONST_UNDEFINED;
            _issuer = ApplicationConstants.CONST_UNDEFINED;
            _liquidTag = ApplicationConstants.CONST_UNDEFINED;
            _marketCap = ApplicationConstants.CONST_UNDEFINED;
            _region = ApplicationConstants.CONST_UNDEFINED;
            _riskCurrency = ApplicationConstants.CONST_UNDEFINED;
            _ucitsEligibleTag = ApplicationConstants.CONST_UNDEFINED;
            _fxRateDisplay = 0.0;
            _netNotionalForCostBasisBreakEven = 0.0;
            _percentNetExposureInBaseCurrency = 0.0;
            _percentGrossExposureInBaseCurrency = 0.0;
            _percentExposureInBaseCurrency = 0.0;
            _percentUnderlyingGrossExposureInBaseCurrency = 0.0;
            _percentBetaAdjGrossExposureInBaseCurrency = 0.0;
            _positionSideExposureUnderlying = string.Empty;
            _positionSideMV = string.Empty;
            _underlyingGrossExposureInBaseCurrency = 0.0;
            _underlyingGrossExposure = 0.0;
            _dayReturn = 0.0;
            _startOfDayNAV = 0.0;
            _percentGrossMarketValueInBaseCurrency = 0.0;
            _betaAdjGrossExposureUnderlying = 0.0;
            _betaAdjGrossExposureUnderlyingInBaseCurrency = 0.0;
            _nav = 0.0;
            _percentagePNLContribution = 0.0;
            _percentNetMarketValueInBaseCurrency = 0.0;
            _positionSideExposure = string.Empty;
            _navTouch = 0;
            _netNotionalValue = 0.0;
            _netNotionalValueInBaseCurrency = 0.0;
            _counterCurrencyID = 0;
            _counterCurrencyAmount = 0.0;
            _counterCurrencyCostBasisPnL = 0.0;
            _counterCurrencySymbol = string.Empty;
            _counterCurrencyDayPnL = 0.0;
            _itmOtm = OptionMoneyness.NA;
            _percentOfITMOTM = 0.0;
            _intrinsicValue = 0.0;
            _daysToExpiry = 0;
            _gainLossIfExerciseAssign = 0.0;
            _factSetSymbol = string.Empty;
            _activSymbol = string.Empty;
            _dayTradedPosition = 0.0;
            _tradeVolume = 0.0;
            _customUDA8 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA9 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA10 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA11 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA12 = ApplicationConstants.CONST_UNDEFINED;
        }

        public ExposurePnlCacheItem(string epnlOrderString)
            : this()
        {
            try
            {
                string[] str = epnlOrderString.Split(Seperators.SEPERATOR_2);
                epnlOrderString = null;
                if (str.Length > 229)
                {
                    Logger.LoggerWrite("the array length should be 229" + "the input string is: " + str);
                }
                else
                {
                    _hasBeenSenttoUser = int.Parse(str[0]);
                    _id = str[1];
                    _askPrice = double.Parse(str[2]);
                    _auecID = int.Parse(str[3]);
                    _averageVolume20Day = double.Parse(str[4]);
                    _avgPrice = !String.IsNullOrEmpty(str[5]) ? (double?)double.Parse(str[5]) : null;
                    _beta = double.Parse(str[6]);
                    _bidPrice = double.Parse(str[7]);
                    _cashImpact = double.Parse(str[8]);
                    _cashImpactInBaseCurrency = double.Parse(str[9]);
                    _closingPrice = !String.IsNullOrEmpty(str[10]) ? (double?)double.Parse(str[10]) : null; ;
                    _costBasisUnrealizedPnL = double.Parse(str[11]);
                    _costBasisUnrealizedPnLInBaseCurrency = double.Parse(str[12]);
                    _currencyID = int.Parse(str[13]);
                    _dayInterest = double.Parse(str[14]);
                    _dayPnL = double.Parse(str[15]);
                    _dayPnLInBaseCurrency = double.Parse(str[16]);
                    _delta = Convert.ToDouble(str[17]);
                    _description = str[18];
                    DateTime dt;
                    if (DateTime.TryParse(str[19], out dt))
                    {
                        _exDividendDate = dt;
                    }
                    else
                    {
                        _exDividendDate = null;
                    }
                    _exposureBPInBaseCurrency = double.Parse(str[20]);
                    if (String.Compare(str[21], Operator.D.ToString(), StringComparison.Ordinal) == 0)
                    {
                        _feedPriceOperator = Operator.D;
                    }
                    _fullSecurityName = str[22];
                    if (String.Compare(str[23], Operator.D.ToString(), StringComparison.Ordinal) == 0)
                    {
                        _fxConversionMethodOperator = Operator.D;
                    }
                    _fxRate = !String.IsNullOrEmpty(str[24]) ? (double?)double.Parse(str[24]) : null;
                    _fxRateOnTradeDateStr = str[25];
                    _highPrice = double.Parse(str[26]);
                    _lastPrice = double.Parse(str[27]);
                    if (DateTime.TryParse(str[28], out dt))
                    {
                        _lastUpdatedUTC = dt;
                    }
                    else
                    {
                        _lastUpdatedUTC = null;
                    }
                    _level1ID = int.Parse(str[29]);
                    _level2ID = int.Parse(str[30]);
                    _lowPrice = double.Parse(str[31]);
                    _marketCapitalization = double.Parse(str[32]);
                    _marketValue = double.Parse(str[33]);
                    _marketValueInBaseCurrency = double.Parse(str[34]);
                    _masterFundID = int.Parse(str[35]);
                    _midPrice = double.Parse(str[36]);
                    _multiplier = double.Parse(str[37]);
                    _netExposure = double.Parse(str[38]);
                    _netExposureInBaseCurrency = double.Parse(str[39]);
                    _orderSideTagValue = str[40];
                    _percentageChange = double.Parse(str[41]);
                    _percentageGainLoss = double.Parse(str[42]);
                    _quantity = double.Parse(str[43]);
                    _asset = str[44];
                    _sharesOutstanding = double.Parse(str[45]);
                    _sideMultiplier = int.Parse(str[46]);
                    _symbol = str[47];
                    _totalInterest = double.Parse(str[48]);
                    if (DateTime.TryParse(str[49], out dt))
                    {
                        _tradeDate = dt;
                    }
                    else
                    {
                        _tradeDate = null;
                    }
                    _transactionSide = str[50];
                    _underlyingID = int.Parse(str[51]);
                    _underlyingSymbol = str[52];
                    _vsCurrencyID = int.Parse(str[53]);
                    _yesterdayMarkPrice = double.Parse(str[54]);
                    _yesterdayMarkPriceStr = str[55];
                    _betaAdjExposure = double.Parse(str[56]);
                    _betaAdjExposureInBaseCurrency = double.Parse(str[57]);
                    _volatility = double.Parse(str[58]);
                    _userName = str[59];
                    _counterPartyName = str[60];
                    _contractType = str[61];
                    _strikePrice = double.Parse(str[62]);
                    if (DateTime.TryParse(str[63], out dt))
                    {
                        _expirationDate = dt;
                    }
                    else
                    {
                        _expirationDate = null;
                    }
                    _masterStrategyID = int.Parse(str[64]);
                    _earnedDividendBase = double.Parse(str[65]);
                    _earnedDividendLocal = double.Parse(str[66]);
                    _percentageAverageVolume = double.Parse(str[67]);
                    _exchangeID = int.Parse(str[68]);
                    _dividendYield = double.Parse(str[69]);
                    _grossExposure = double.Parse(str[70]);
                    _grossMarketValue = double.Parse(str[71]);
                    _underlyingStockPrice = double.Parse(str[72]);
                    _selectedFeedPrice = double.Parse(str[73]);
                    _deltaAdjPosition = double.Parse(str[74]);
                    if (DateTime.TryParse(str[75], out dt))
                    {
                        _settlementDate = dt;
                    }
                    else
                    {
                        _settlementDate = null;
                    }
                    _percentageAverageVolumeDeltaAdj = double.Parse(str[76]);
                    _tradeDayPnl = double.Parse(str[77]);
                    _fxDayPnl = double.Parse(str[78]);
                    _fxCostBasisPnl = double.Parse(str[79]);
                    _tradeCostBasisPnl = double.Parse(str[80]);
                    _isSwap = bool.Parse(str[81]);
                    _percentGainLossCostBasis = double.Parse(str[82]);
                    _leadCurrencyID = int.Parse(str[83]);
                    _underlyingValueForOptions = double.Parse(str[84]);
                    _underlyingDelta = double.Parse(str[85]);
                    _averageVolume20DayUnderlyingSymbol = double.Parse(str[86]);
                    _idcoSymbol = str[87];
                    _osiSymbol = str[88];
                    _sedolSymbol = str[89];
                    _cusipSymbol = str[90];
                    _bloombergSymbol = str[91];
                    _isinSymbol = str[92];
                    _startTradeDate = DateTime.Parse(str[93]);
                    _tradeAttribute1 = str[94];
                    _tradeAttribute2 = str[95];
                    _tradeAttribute3 = str[96];
                    _tradeAttribute4 = str[97];
                    _tradeAttribute5 = str[98];
                    _tradeAttribute6 = str[99];
                    _costBasisBreakEven = !String.IsNullOrEmpty(str[100]) ? (double?)double.Parse(str[100]) : null;
                    _proxySymbol = str[101];
                    _pricingSource = str[102];
                    _percentDayPnLGrossMV = double.Parse(str[103]);
                    _percentDayPnLNetMV = double.Parse(str[104]);
                    _UDAAsset = str[105];
                    _UDACountry = str[106];
                    _UDASector = str[107];
                    _UDASubSector = str[108];
                    _UDASecurityType = str[109];
                    DateTime tempExpirationMonth;
                    if (DateTime.TryParse(str[110], out tempExpirationMonth))
                    {
                        _expirationMonth = tempExpirationMonth;
                    }
                    _deltaAdjPositionLME = double.Parse(str[111]);
                    _premium = double.Parse(str[112]);
                    _premiumDollar = double.Parse(str[113]);
                    _forwardPoints = double.Parse(str[114]);
                    _deltaSource = (DeltaSource)Enum.Parse(typeof(DeltaSource), str[115]);

                    _yesterdayUnderlyingMarkPrice = double.Parse(str[116]);
                    _yesterdayUnderlyingMarkPriceStr = str[117];
                    _percentageUnderlyingChange = double.Parse(str[118]);
                    _transactionType = str[119];
                    _yesterdayMarketValue = double.Parse(str[120]);
                    _yesterdayMarketValueInBaseCurrency = double.Parse(str[121]);
                    _yesterdayFXRate = double.Parse(str[122]);
                    _changeInUnderlyingPrice = double.Parse(str[123]);
                    _reutersSymbol = str[124];
                    _betaAdjGrossExposure = Double.Parse(str[125]);
                    _internalComments = str[126];
                    _grossExposureLocal = Double.Parse(str[127]);
                    _exposure = double.Parse(str[128]);
                    _exposureInBaseCurrency = double.Parse(str[129]);
                    _positionSideExposureBoxed = str[130];
                    _analyst = str[131];
                    _countryOfRisk = str[132];
                    _customUDA1 = str[133];
                    _customUDA2 = str[134];
                    _customUDA3 = str[135];
                    _customUDA4 = str[136];
                    _customUDA5 = str[137];
                    _customUDA6 = str[138];
                    _customUDA7 = str[139];
                    _issuer = str[140];
                    _liquidTag = str[141];
                    _marketCap = str[142];
                    _region = str[143];
                    _riskCurrency = str[144];
                    _ucitsEligibleTag = str[145];
                    _selectedFeedPriceInBaseCurrency = double.Parse(str[146]);
                    _fxRateDisplay = !String.IsNullOrEmpty(str[147]) ? (double?)double.Parse(str[147]) : null;
                    _netNotionalForCostBasisBreakEven = double.Parse(str[148]);
                    _percentNetExposureInBaseCurrency = double.Parse(str[149]);
                    _percentGrossExposureInBaseCurrency = double.Parse(str[150]);
                    _percentExposureInBaseCurrency = double.Parse(str[151]);
                    _percentUnderlyingGrossExposureInBaseCurrency = double.Parse(str[152]);
                    _percentBetaAdjGrossExposureInBaseCurrency = double.Parse(str[153]);
                    _positionSideExposureUnderlying = str[154];
                    _positionSideMV = str[155];
                    _underlyingGrossExposureInBaseCurrency = double.Parse(str[156]);
                    _underlyingGrossExposure = double.Parse(str[157]);
                    _dayReturn = double.Parse(str[158]);
                    _startOfDayNAV = double.Parse(str[159]);
                    _percentGrossMarketValueInBaseCurrency = double.Parse(str[160]);
                    _betaAdjGrossExposureUnderlying = double.Parse(str[161]);
                    _betaAdjGrossExposureUnderlyingInBaseCurrency = double.Parse(str[162]);
                    _nav = double.Parse(str[163]);
                    _percentagePNLContribution = double.Parse(str[164]);
                    _percentNetMarketValueInBaseCurrency = double.Parse(str[165]);
                    _positionSideExposure = str[166];
                    _navTouch = double.Parse(str[167]);
                    _netNotionalValue = double.Parse(str[168]);
                    _netNotionalValueInBaseCurrency = double.Parse(str[169]);
                    _counterCurrencyID = int.Parse(str[170]);
                    _counterCurrencyAmount = double.Parse(str[171]);
                    _counterCurrencyCostBasisPnL = double.Parse(str[172]);
                    _counterCurrencyDayPnL = double.Parse(str[173]);
                    _itmOtm = (OptionMoneyness)Enum.Parse(typeof(OptionMoneyness), str[174]);
                    _percentOfITMOTM = double.Parse(str[175]);
                    _intrinsicValue = double.Parse(str[176]);
                    _daysToExpiry = int.Parse(str[177]);
                    _gainLossIfExerciseAssign = double.Parse(str[178]);
                    _factSetSymbol = str[179];
                    _activSymbol = str[180];
                    _dayTradedPosition = double.Parse(str[181]);
                    _tradeVolume = double.Parse(str[182]);
                    _customUDA8 = str[183];
                    _customUDA9 = str[184];
                    _customUDA10 = str[185];
                    _customUDA11 = str[186];
                    _customUDA12 = str[187];
                    _bloombergSymbolWithExchangeCode = str[188];
                    TradeAttribute7 = str[189];
                    TradeAttribute8 = str[190];
                    TradeAttribute9 = str[191];
                    TradeAttribute10 = str[192];
                    TradeAttribute11 = str[193];
                    TradeAttribute12 = str[194];
                    TradeAttribute13 = str[195];
                    TradeAttribute14 = str[196];
                    TradeAttribute15 = str[197];
                    TradeAttribute16 = str[198];
                    TradeAttribute17 = str[199];
                    TradeAttribute18 = str[200];
                    TradeAttribute19 = str[201];
                    TradeAttribute20 = str[202];
                    TradeAttribute21 = str[203];
                    TradeAttribute22 = str[204];
                    TradeAttribute23 = str[205];
                    TradeAttribute24 = str[206];
                    TradeAttribute25 = str[207];
                    TradeAttribute26 = str[208];
                    TradeAttribute27 = str[209];
                    TradeAttribute28 = str[210];
                    TradeAttribute29 = str[211];
                    TradeAttribute30 = str[212];
                    TradeAttribute31 = str[213];
                    TradeAttribute32 = str[214];
                    TradeAttribute33 = str[215];
                    TradeAttribute34 = str[216];
                    TradeAttribute35 = str[217];
                    TradeAttribute36 = str[218];
                    TradeAttribute37 = str[219];
                    TradeAttribute38 = str[220];
                    TradeAttribute39 = str[221];
                    TradeAttribute40 = str[222];
                    TradeAttribute41 = str[223];
                    TradeAttribute42 = str[224];
                    TradeAttribute43 = str[225];
                    TradeAttribute44 = str[226];
                    TradeAttribute45 = str[227];
                    _pricingStatus = str[228];
                    str = null;
                }
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(HasBeenSentToUser.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ID);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(AskPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(AUECID.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(AverageVolume20Day);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(AvgPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Beta);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BidPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CashImpact.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CashImpactInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ClosingPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CostBasisUnrealizedPnL.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CostBasisUnrealizedPnLInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CurrencyID.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DayInterest.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DayPnL.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DayPnLInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Delta.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Description);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ExDividendDate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ExposureBPInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FeedPriceOperator);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FullSecurityName);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FXConversionMethodOperator);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FxRate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FXRateOnTradeDateStr);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(HighPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(LastPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(LastUpdatedUTC);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Level1ID.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Level2ID.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(LowPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MarketCapitalization);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MarketValue.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MarketValueInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MasterFundID.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MidPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Multiplier);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NetExposure.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NetExposureInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(OrderSideTagValue);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentageChange);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentageGainLoss.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Quantity.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Asset);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(SharesOutstanding);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(SideMultiplier.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Symbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TotalInterest.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeDate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TransactionSide);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UnderlyingID);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UnderlyingSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(VsCurrencyID);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayMarkPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayMarkPriceStr);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BetaAdjExposure.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BetaAdjExposureInBaseCurrency.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Volatility);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UserName);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CounterPartyName);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(_contractType);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(_strikePrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(_expirationDate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MasterStrategyID.ToString());
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(EarnedDividendBase);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(EarnedDividendLocal);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentageAverageVolume);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ExchangeID);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DividendYield);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(GrossExposure);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(GrossMarketValue);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UnderlyingStockPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(SelectedFeedPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DeltaAdjPosition);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(SettlementDate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentageAverageVolumeDeltaAdjusted);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeDayPnl);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FxDayPnl);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FxCostBasisPnl);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeCostBasisPnl);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(IsSwap);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentageGainLossCostBasis);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(LeadCurrencyID);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UnderlyingValueForOptions);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(LeveragedFactor);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(AverageVolume20DayUnderlyingSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(IdcoSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(OsiSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(SedolSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CusipSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BloombergSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(IsinSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(StartTradeDate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute1);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute2);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute3);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute4);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute5);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute6);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CostBasisBreakEven);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ProxySymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PricingSource);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentDayPnLGrossMV);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentDayPnLNetMV);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UDAAsset);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UDACountry);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UDASector);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UDASubSector);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UDASecurityType);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ExpirationMonth);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DeltaAdjPositionLME);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Premium);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PremiumDollar);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ForwardPoints);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DeltaSource);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayUnderlyingMarkPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayUnderlyingMarkPriceStr);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentageUnderlyingChange);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TransactionType);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayMarketValue);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayMarketValueInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(YesterdayFXRate);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ChangeInUnderlyingPrice);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ReutersSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BetaAdjGrossExposure);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(InternalComments);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(GrossExposureLocal);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Exposure);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PositionSideExposureBoxed);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Analyst);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CountryOfRisk);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA1);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA2);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA3);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA4);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA5);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA6);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA7);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Issuer);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(LiquidTag);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(MarketCap);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(Region);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(RiskCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UcitsEligibleTag);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(SelectedFeedPriceInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FxRateDisplay);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NetNotionalForCostBasisBreakEven);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentNetExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentGrossExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentUnderlyingGrossExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentBetaAdjGrossExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PositionSideExposureUnderlying);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PositionSideMV);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UnderlyingGrossExposureInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(UnderlyingGrossExposure);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DayReturn);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(StartOfDayNAV);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentGrossMarketValueInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BetaAdjGrossExposureUnderlying);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BetaAdjGrossExposureUnderlyingInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NAV);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentagePNLContribution);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentNetMarketValueInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PositionSideExposure);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NavTouch);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NetNotionalValue);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(NetNotionalValueInBaseCurrency);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CounterCurrencyID);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CounterCurrencyAmount);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CounterCurrencyCostBasisPnL);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CounterCurrencyDayPnL);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ItmOtm);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PercentOfITMOTM);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(IntrinsicValue);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DaysToExpiry);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(GainLossIfExerciseAssign);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(FactSetSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(ActivSymbol);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(DayTradedPosition);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeVolume);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA8);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA9);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA10);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA11);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(CustomUDA12);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(BloombergSymbolWithExchangeCode);

                // Add TradeAttributes 7 to 45
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute7);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute8);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute9);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute10);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute11);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute12);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute13);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute14);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute15);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute16);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute17);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute18);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute19);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute20);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute21);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute22);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute23);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute24);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute25);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute26);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute27);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute28);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute29);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute30);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute31);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute32);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute33);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute34);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute35);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute36);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute37);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute38);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute39);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute40);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute41);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute42);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute43);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute44);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(TradeAttribute45);
                sb.Append(Seperators.SEPERATOR_2);
                sb.Append(PricingStatus);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return sb.ToString();
        }

        public void UpdateDynamicData(ref List<PropertyInfo> dynamicPropertyList, string dynamicDataString, ref bool isColumnListChanged)
        {
            Type type = null;
            string dynamicPropvalue = string.Empty;
            try
            {
                string[] dynamicPropArr = dynamicDataString.Split(Seperators.SEPERATOR_2);

                if (dynamicPropArr.Length > 0)
                {
                    for (int i = 0; i < dynamicPropertyList.Count; i++)
                    {
                        //System.Reflection.PropertyInfo
                        try
                        {
                            //The complete order is not being updated, If Dynamic column list is changed while oredr collection is being updating from expnl end
                            //Collection will be updated in next interval cycle.
                            //Check on "i > 1 " is must, as first 2 columns are ID & HasBeenSentToUsers.
                            if (isColumnListChanged && i > 1)
                            {
                                IsStaleData = true;
                                break;
                            }
                            if (dynamicPropertyList[i].PropertyType.IsEnum)
                            {
                                if (dynamicPropertyList[i].PropertyType == typeof(PricingSource) && Enum.IsDefined(typeof(PricingSource), dynamicPropArr[i]))
                                {
                                    dynamicPropertyList[i].SetValue(this, (PricingSource)Enum.Parse(typeof(PricingSource), dynamicPropArr[i]), null);
                                }
                                if (dynamicPropertyList[i].PropertyType == typeof(DeltaSource) && Enum.IsDefined(typeof(DeltaSource), dynamicPropArr[i]))
                                {
                                    dynamicPropertyList[i].SetValue(this, (DeltaSource)Enum.Parse(typeof(DeltaSource), dynamicPropArr[i]), null);
                                }
                                if (dynamicPropertyList[i].PropertyType == typeof(OptionMoneyness) && Enum.IsDefined(typeof(OptionMoneyness), dynamicPropArr[i]))
                                {
                                    dynamicPropertyList[i].SetValue(this, (OptionMoneyness)Enum.Parse(typeof(OptionMoneyness), dynamicPropArr[i]), null);
                                }
                            }
                            else
                            {
                                if (dynamicPropertyList[i].PropertyType.IsGenericType && dynamicPropertyList[i].PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    if (String.IsNullOrEmpty(dynamicPropArr[i]))
                                        dynamicPropertyList[i].SetValue(this, null, null);
                                    else
                                        dynamicPropertyList[i].SetValue(this, Convert.ChangeType(dynamicPropArr[i], dynamicPropertyList[i].PropertyType.GetGenericArguments()[0]), null);
                                }
                                else
                                    dynamicPropertyList[i].SetValue(this, Convert.ChangeType(dynamicPropArr[i], dynamicPropertyList[i].PropertyType), null);
                            }
                        }
                        catch
                        {
                            //Do nothing. If here is any exception, it wont bother the further updations
                            IsStaleData = true;
                            Logger.LoggerWrite("\n Stale Order");
                            break;
                        }
                    }
                }
                dynamicPropArr = null;
            }
            catch (Exception ex)
            {
                string typeString = type != null ? type.ToString() : "Unknown";
                Logger.LoggerWrite(dynamicDataString + "\n Inner Exception:" + ex.InnerException + "\n Dynamic property Value:" + dynamicPropvalue + "\n Property Type" + typeString);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool UpdateDynamicDataFromOrder(ref List<PropertyInfo> dynamicPropertyList, ExposurePnlCacheItem incomingOrder, int pmRowColorbasis, List<string> pmCurrentViewGroupedColumns, ref bool isGroupingColumnValueChanged)
        {
            bool isOrderChanged = false;
            try
            {
                //Divya : 28062012: Row coloring is done on the basis of day pnl or Order Side.
                //If day Pnl switches sign i.e become +ve from -ve or vice versa, we have to change thwe color of the respective row
                string rowColorUpdatebasis = string.Empty;
                if (pmRowColorbasis == 1)
                {
                    rowColorUpdatebasis = COLORBASIS_DayPnL;
                }
                else if (pmRowColorbasis == 0)
                {
                    rowColorUpdatebasis = COLORBASIS_OrderSide;
                }
                try
                {
                    for (int i = 0; i < dynamicPropertyList.Count; i++)
                    {
                        string propName = dynamicPropertyList[i].Name;
                        object previousValue = dynamicPropertyList[i].GetValue(this, null);
                        object newValue = dynamicPropertyList[i].GetValue(incomingOrder, null);

                        //This check is for forcefully refresh PM grid becasue sometimes Grouping and Grid Data messed up
                        if (!isGroupingColumnValueChanged && pmCurrentViewGroupedColumns.Contains(propName) && !newValue.ToString().Equals(previousValue.ToString()))
                        {
                            isGroupingColumnValueChanged = true;
                        }

                        if (propName.Equals(rowColorUpdatebasis))
                        {
                            isOrderChanged = UpdateRowColor(propName, newValue.ToString(), previousValue.ToString());
                        }
                        dynamicPropertyList[i].SetValue(this, newValue, null);
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                    //Do nothing. If here is any exception, it wont bother the further updations
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return isOrderChanged;
        }

        private bool UpdateRowColor(string propName, string newValue, string previousValue)
        {
            bool isOrderChanged = false;
            try
            {
                if (propName.Equals(COLORBASIS_DayPnL))
                {
                    double prevDayPnL = Convert.ToDouble(previousValue);
                    double newDayPnl = Convert.ToDouble(newValue);

                    if (Math.Sign(Math.Round(prevDayPnL)) != (Math.Sign(Math.Round(newDayPnl))))
                    {
                        //either if day pnl switches sign or becomes zero
                        isOrderChanged = true;
                    }
                }
                else if (!previousValue.Equals(newValue))
                {
                    isOrderChanged = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return isOrderChanged;
        }

        public string GetDynamicDataString(List<string> dynamicColumnList)
        {
            StringBuilder sbSub = new StringBuilder();
            try
            {
                foreach (string str in dynamicColumnList)
                {
                    PropertyInfo pInfo = GetType().GetProperty(str);
                    if (pInfo != null)
                    {
                        sbSub.Append(pInfo.GetValue(this, null));
                        sbSub.Append(Seperators.SEPERATOR_2);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return sbSub.ToString();
        }

        public string GetDynamicDataString2(List<string> dynamicColumnList, List<string> columnDataAsZero)
        {
            if (columnDataAsZero == null)
            {
                return GetDynamicDataString(dynamicColumnList);
            }
            else
            {
                StringBuilder sbSub = new StringBuilder();
                try
                {
                    foreach (string str in dynamicColumnList)
                    {
                        PropertyInfo pInfo = GetType().GetProperty(str);
                        if (pInfo != null)
                        {
                            if (columnDataAsZero.Contains(pInfo.Name))
                            {
                                if (pInfo.PropertyType == typeof(double) || pInfo.PropertyType == typeof(double?))
                                    sbSub.Append(0.0);
                                else if (pInfo.Name.Equals("PositionSideExposure") || pInfo.Name.Equals("PositionSideMV") || pInfo.Name.Equals("PositionSideExposureBoxed"))
                                    sbSub.Append(PositionType.Long.ToString());
                                else if (pInfo.Name.Equals("PricingSource"))
                                    sbSub.Append(AppConstants.PricingSource.None.ToString());
                                else if (pInfo.Name.Equals("PricingStatus"))
                                    sbSub.Append(AppConstants.PricingStatus.None.ToString());
                                else if (pInfo.Name.Equals("LastUpdatedUTC"))
                                    sbSub.Append(DateTimeConstants.DateTimeMinVal);
                                else if (pInfo.Name.Equals("DeltaSource"))
                                    sbSub.Append(DeltaSource.Default.ToString());
                                else
                                    sbSub.Append(0);
                            }
                            else
                            {
                                sbSub.Append(pInfo.GetValue(this, null));
                            }
                            sbSub.Append(Seperators.SEPERATOR_2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

                return sbSub.ToString();
            }
        }

        public ExposurePnlCacheItem ApplyMarketDataPermission(List<string> columnDataAsZero)
        {
            ExposurePnlCacheItem exposurePnlCacheItem = new ExposurePnlCacheItem();
            try
            {
                foreach (PropertyInfo pInfo in GetType().GetProperties())
                {
                    if (pInfo != null)
                    {
                        if (columnDataAsZero.Contains(pInfo.Name))
                        {
                            if (pInfo.PropertyType == typeof(double) || pInfo.PropertyType == typeof(double?))
                                pInfo.SetValue(exposurePnlCacheItem, 0.0);
                            else if (pInfo.Name.Equals("PositionSideExposure") || pInfo.Name.Equals("PositionSideMV") || pInfo.Name.Equals("PositionSideExposureBoxed"))
                                pInfo.SetValue(exposurePnlCacheItem, PositionType.Long.ToString());
                            else if (pInfo.Name.Equals("PricingSource"))
                                pInfo.SetValue(exposurePnlCacheItem, AppConstants.PricingSource.None.ToString());
                            else if (pInfo.Name.Equals("PricingStatus"))
                                pInfo.SetValue(exposurePnlCacheItem, AppConstants.PricingStatus.None.ToString());
                            else if (pInfo.Name.Equals("LastUpdatedUTC"))
                                pInfo.SetValue(exposurePnlCacheItem, DateTimeConstants.MinValue);
                            else if (pInfo.Name.Equals("DeltaSource"))
                                pInfo.SetValue(exposurePnlCacheItem, DeltaSource.Default);
                            else
                                pInfo.SetValue(exposurePnlCacheItem, 0);
                        }
                        else
                        {
                            pInfo.SetValue(exposurePnlCacheItem, pInfo.GetValue(this, null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return exposurePnlCacheItem;
        }

        #region AdditionalTradeAttributes Utilities
        /// <summary>
        /// Returns all trade attributes as a dictionary.
        /// </summary>
        public virtual Dictionary<string, string> GetTradeAttributesAsDict()
        {
            var attributesArray = new Dictionary<string, string>();
            try
            {      
                foreach (var entry in _attributeGetters)
                {
                    attributesArray.Add(entry.Key, entry.Value(this));
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return attributesArray;
        }

        /// <summary>
        /// Sets a single trade attribute value.
        /// </summary>
        public virtual void SetTradeAttributeValue(string attributeName, string value)
        {
            try
            {
                if (_attributeSetters.TryGetValue(attributeName, out var setter))
                {
                    setter(this, value ?? string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion
    }
}
