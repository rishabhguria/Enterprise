using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnlOrder : EPnLOrderBase
    {
        #region Private Fields
        private bool _isMarkedForCalculation = false;
        private EPnLClassID _classID = EPnLClassID.EPnlOrder;
        private string _userName;
        private string _counterPartyName;
        private string _id;
        private AssetCategory _asset;
        private string _assetName;
        private int _exchangeID;
        private string _exchangeName;
        private int _currencyID;
        private int _auecID;
        private double _underlyingValueForOptions;
        private double _quantity;
        private double _avgPrice;
        private string _symbol;
        private int _sideMultiplier;
        private string _orderSideTagValue;
        private string _orderStatusTagValue;
        private double _dayPnLShort;
        private double _dayPnLLong;
        private double _dayPnL;
        private double _dayPnLInBaseCurrency;
        private double _shortNotionalValue;
        private double _shortNotionalValueInBaseCurrency;
        private double _longNotionalValue;
        private double _longNotionalValueInBaseCurrency;
        private double _netExposure;
        private double _exposure;
        private double _deltaAdjPosition;
        private double _netExposureInBaseCurrency;
        private double _exposureInBaseCurrency;
        private double _betaAdjExposure;
        private double _betaAdjExposureInBaseCurrency;
        private double _exposureBPInBaseCurrency;
        private double _fxRate;
        private Operator _fxConversionMethodOperator;
        private Operator _feedPriceOperator;
        private double _askPrice;
        private double _bidPrice;
        private double _dividendYield;
        private double _lastPrice;
        private double _closingPrice;
        private double _highPrice;
        private double _lowPrice;
        private double _multiplier;
        private int _level1ID;
        private int _level2ID;
        private DateTime _transactionDate;
        private double _percentagePositionLong;
        private double _percentagePositionShort;
        private PositionType _transactionSide;
        private string _underlyingSymbol;
        private double _underlyingStockPrice;
        private double _yesterdayMarkPrice;
        private DateTime _yesterdayMarkDateActual;
        private string _fullSecurityName;
        private DateTime _auecLocalDate;
        private string _description;
        private string _internalComments;
        private DateTime _lastUpdatedUTC;
        private bool _isServerUpdated;
        private double _fxRateOnTradeDate;
        private Operator _fxConversionMethodOnTradeDate;
        private string _fxRateOnTradeDateStr;
        private string _yesterdayMarkPriceStr;
        private int _masterFundID;
        private int _masterStrategyID;
        private bool _isSwapped;
        private double _costBasisUnrealizedPnL;
        private double _costBasisUnrealizedPnlInBaseCurrency;
        private double _marketValue;
        private double _marketValueInBaseCurrency;
        private double _percentageGainLoss;
        private bool _isFXRateSavedWithTrade;
        private double _cashImpact;
        private double _beta;
        private double _marketCapitalization;
        private double _sharesOutstanding;
        private bool _isLiveFeedSharesOutstanding;
        private double _averageVolume20Day;
        private double _averageVolume20DayUnderlyingSymbol;
        private DateTime _exDividendDate;
        private double _cashImpactInBaseCurrency;
        private double _iMid;
        private bool _getLastIfZero = false;
        private int _hasBeenSentToUser = 1;
        private double _yesterdayMarketValueInBaseCurrency;
        private double _earnedDividendLocal;
        private double _earnedDividendBase;
        private double _percentageAverageVolume;
        private int _underlyingID;
        private double _selectedFeedPrice;
        private double _selectedFeedPriceInBaseCurrency;
        private DateTime _settlementDate;
        private double _percentageAverageVolumeDeltaAdjusted;
        private double _tradeDayPnl;
        private double _fxDayPnl;
        private double _fxCostBasisPnl;
        private double _tradeCostBasisPnl;
        private double _percentageGainLossCostBasis;
        private double _leveragedFactor;
        private string _idcoSymbol;
        private string _osiSymbol;
        private string _sedolSymbol;
        private string _cusipSymbol;
        private string _bloombergSymbol;
        private string _bloombergSymbolWithExchangeCode;
        private string _factSetSymbol;
        private string _activSymbol;
        private string _isinSymbol;
        private PricingSource _pricingSource;
        private DeltaSource _deltaSource;
        private double _percentDayPnLGrossMV;
        private double _percentDayPnLNetMV;
        private double _betaAdjGrossExposure;
        private double _deltaAdjPositionLME;
        private double _premium;
        private double _premiumDollar;
        private double _forwardPoints;
        private ApplicationConstants.TaxLotState _epnlOrderState;
        private int _counterPartyId;
        private int _venueId;
        private double _yesterdayUnderlyingMarkPrice;
        private string _yesterdayUnderlyingMarkPriceStr;
        private string _transactionType;
        private double _yesterdayMarketValue;
        private double _yesterdayFXRate;
        private string _reutersSymbol;
        private PositionType _positionSideExposureBoxed;
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
        private double _fxRateDisplay;
        private double _netNotionalForCostBasisBreakEven;
        private PositionType _positionSideMV;
        private PositionType _positionSideExposure;
        private string _UDAAsset;
        private string _UDASecurityType;
        private string _UDASector;
        private string _UDASubSector;
        private string _UDACountry;
        private string _tradeAttribute1 = string.Empty;
        private string _tradeAttribute2 = string.Empty;
        private string _tradeAttribute3 = string.Empty;
        private string _tradeAttribute4 = string.Empty;
        private string _tradeAttribute5 = string.Empty;
        private string _tradeAttribute6 = string.Empty;
        private string _proxySymbol = string.Empty;
        private double _changeInUnderlyingPrice;
        private double _midPrice;
        private double _navTouch;
        private double _netNotionalValue;
        private double _netNotionalInBaseCurrency;
        private string _orderSide = string.Empty;
        private string _venue = string.Empty;
        private string _orderTypeTagValue = string.Empty;
        private string _orderType = string.Empty;
        private string _underlyingName = string.Empty;
        private double _dayTradedPosition;
        private double _tradeVolume;
        private PricingStatus _pricingStatus;

        private static readonly Dictionary<string, Action<EPnlOrder, string>> _attributeSetters =
            new Dictionary<string, Action<EPnlOrder, string>>
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

        private static readonly Dictionary<string, Func<EPnlOrder, string>> _attributeGetters =
            new Dictionary<string, Func<EPnlOrder, string>>
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
        #endregion

        #region Public Fields
        public bool IsMarkedForCalculation
        {
            get { return _isMarkedForCalculation; }
            set { _isMarkedForCalculation = value; }
        }
        public ApplicationConstants.TaxLotState EpnlOrderState
        {
            get { return _epnlOrderState; }
            set { _epnlOrderState = value; }
        }
        public int CounterPartyId
        {
            get { return _counterPartyId; }
            set { _counterPartyId = value; }
        }
        public int VenueId
        {
            get { return _venueId; }
            set { _venueId = value; }
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
        public override EPnLClassID ClassID
        {
            get { return _classID; }
        }
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public AssetCategory Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }
        public string AssetName
        {
            get { return _assetName; }
            set { _assetName = value; }
        }
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }
        public string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }
        public Int32 AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public int SideMultiplier
        {
            get { return _sideMultiplier; }
            //To Do : Remove this setter and make this property readonly when Prana.Constants is created
            set
            {
                _sideMultiplier = value;
            }
        }
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set
            {
                _orderSideTagValue = value;
            }
        }
        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }
        public double DayPnLShort
        {
            get { return _dayPnLShort; }
            set { _dayPnLShort = value; }
        }
        public double DayPnLLong
        {
            get { return _dayPnLLong; }
            set { _dayPnLLong = value; }
        }
        public double UnderlyingValueForOptions
        {
            get { return _underlyingValueForOptions; }
            set { _underlyingValueForOptions = value; }
        }
        public double DayPnL
        {
            get { return _dayPnL; }
            set
            {
                _dayPnL = value;
            }
        }
        public double DayPnLBP
        {
            get { return (_dayPnL / Math.Abs(_netNotionalValue)) * 10000; }
        }
        public double DayPnLInBaseCurrency
        {
            get { return _dayPnLInBaseCurrency; }
            set { _dayPnLInBaseCurrency = value; }
        }
        public double ShortNotionalValue
        {
            get { return _shortNotionalValue; }
        }
        public double ShortNotionalValueInBaseCurrency
        {
            get { return _shortNotionalValueInBaseCurrency; }
        }
        public double LongNotionalValue
        {
            get { return _longNotionalValue; }
        }
        public double LongNotionalValueInBaseCurrency
        {
            get { return _longNotionalValueInBaseCurrency; }
        }
        public double NetExposure
        {
            get { return _netExposure; }
            set
            {
                _netExposure = value;
            }
        }
        public double Exposure
        {
            get { return _exposure; }
            set
            {
                _exposure = value;
            }
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
        //Special case for FX. FXRate is for "VsCurrency - BaseCurrency". While in other cases FXRate is "Symbol - BaseCurrency"
        public double FxRate
        {
            get { return _fxRate; }
            set
            {
                _fxRate = value;
            }
        }
        /// <summary>
        /// Can hold divide or multiply. important for fx trades. It is better to assign this operator before assigning fxRate
        /// FXRate is for "VsCurrency - BaseCurrency". Special case for FX.  For non-fx trades While in other cases FXRate is "Symbol - BaseCurrency"
        /// </summary>
        public Operator FXConversionMethodOperator
        {
            get { return _fxConversionMethodOperator; }
            set { _fxConversionMethodOperator = value; }
        }
        /// <summary>
        /// Feed price operator is different than FXConversionMethodOperator, becuase for FX feed price is - "Symbol - VsCurrency" while
        /// FXRate is for "TradedCurrency - BaseCurrency". In other asset classes except FX, we get feed price in the traded currency
        /// </summary>
        public Operator FeedPriceOperator
        {
            get { return _feedPriceOperator; }
            set { _feedPriceOperator = value; }
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
        public DateTime SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }
        public double LastPrice
        {
            get { return _lastPrice; }
            set
            {
                _lastPrice = value;
            }
        }
        public double ClosingPrice
        {
            get { return _closingPrice; }
            set { _closingPrice = value; }
        }
        public double HighPrice
        {
            get { return _highPrice; }
            set { _highPrice = value; }
        }
        public double LowPrice
        {
            get { return _lowPrice; }
            set { _lowPrice = value; }
        }
        public double LeveragedFactor
        {
            get { return _leveragedFactor; }
            set { _leveragedFactor = value; }
        }
        public double MidPrice
        {
            get
            {
                if (_midPrice == 0)
                {
                    if (_getLastIfZero)
                    {
                        return _lastPrice;
                    }
                }
                return _midPrice;
            }
            set { _midPrice = value; }
        }
        public double PercentageChange
        {
            get
            {
                if (_closingPrice > 0 && (_yesterdayMarkPriceStr.EndsWith("*") || _yesterdayMarkPriceStr.Equals(ApplicationConstants.CONST_UNDEFINED) || _yesterdayMarkPriceStr.Equals(String.Empty)))
                {
                    return Math.Round((((_selectedFeedPrice - _closingPrice) / _closingPrice) * 100), 4);
                }
                else if (_yesterdayMarkPrice != 0)
                {
                    return Math.Round((((_selectedFeedPrice - _yesterdayMarkPrice) / _yesterdayMarkPrice) * 100), 4);
                }
                else
                {
                    return 0;
                }
            }
        }
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }
        /// <summary>
        /// _accountID = -1 represents Un-allocation
        /// </summary>
        public int Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public int Level2ID
        {
            get { return _level2ID; }
            set { _level2ID = value; }
        }
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set
            {
                _transactionDate = value;
            }
        }
        public double PercentagePositionLong
        {
            get { return _percentagePositionLong; }
            set { _percentagePositionLong = value; }
        }
        public double PercentagePositionShort
        {
            get { return _percentagePositionShort; }
            set { _percentagePositionShort = value; }
        }
        public PositionType TransactionSide
        {
            get { return _transactionSide; }
            set { _transactionSide = value; }
        }
        /// <summary>
        /// In case of derivatives, this property holds the underlying symbol
        /// </summary>
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }
        /// <summary>
        /// Stock price of the underlying
        /// </summary>
        public double UnderlyingStockPrice
        {
            get { return _underlyingStockPrice; }
            set { _underlyingStockPrice = value; }
        }
        public double YesterdayMarkPrice
        {
            get { return _yesterdayMarkPrice; }
            set { _yesterdayMarkPrice = value; }
        }
        public DateTime YesterdayMarkDateActual
        {
            get { return _yesterdayMarkDateActual; }
            set { _yesterdayMarkDateActual = value; }
        }
        public string FullSecurityName
        {
            get { return _fullSecurityName; }
            set { _fullSecurityName = value; }
        }
        public DateTime AUECLocalDate
        {
            get { return _auecLocalDate; }
            set { _auecLocalDate = value; }
        }
        /// <summary>
        /// Added 14 April 08, Mainly keeps the create transaction description.
        /// </summary>
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
        /// Added 14th, November. Keeps last updated time of a LiveFeed data.
        /// </summary>
        public DateTime LastUpdatedUTC
        {
            get { return _lastUpdatedUTC; }
            set
            {
                _lastUpdatedUTC = value;
            }
        }
        /// <summary>
        /// If we get the latest fill from server and it is to be updated in the expnl cache, Then set its value to be true. Once updated this
        /// fill into the cache, we set it "False".
        /// Scenario 1 : Cancel-amend : The server fill cache don't update the expnl calculation cache each time because IsServerUpdated = false. so cancel amend changes will be reflected.
        /// Scenario 2 : Fill receive from server after changing the same order from cancel amend : New fill will overwrite the cancel amend changes.
        /// </summary>
        public bool IsServerUpdated
        {
            get { return _isServerUpdated; }
            set { _isServerUpdated = value; }
        }
        /// <summary>
        /// Added 01 May 08, Keeps the FX rate info for FX transactions.
        /// this field added for expnl updation with respect to change in net exposure with the traded date.
        /// Field added on 30th April.
        /// </summary>
        public double FXRateOnTradeDate
        {
            get { return _fxRateOnTradeDate; }
            set { _fxRateOnTradeDate = value; }
        }
        /// <summary>
        /// Added 01 May 08, Keeps the FX conversion method info for FX transactions.
        /// </summary>
        public Operator FXConversionMethodOnTradeDate
        {
            get { return _fxConversionMethodOnTradeDate; }
            set { _fxConversionMethodOnTradeDate = value; }
        }
        /// <summary>
        /// appended * means old value else required date value.
        /// </summary>
        public string FXRateOnTradeDateStr
        {
            get { return _fxRateOnTradeDateStr; }
            set
            {
                _fxRateOnTradeDateStr = (value == string.Empty ? "N/A" : value);
                switch (_fxConversionMethodOnTradeDate)
                {
                    case Operator.M:
                        break;

                    case Operator.D:
                        break;
                }
            }
        }
        /// <summary>
        /// Added 13 May 08, To Keep track on the yesterday mark price,if get yesterday mark price then value will be 0 else 1.
        /// default value is 1
        /// </summary>
        public string YesterdayMarkPriceStr
        {
            get { return _yesterdayMarkPriceStr; }
            set { _yesterdayMarkPriceStr = value; }
        }
        public int MasterFundID
        {
            get { return _masterFundID; }
            set { _masterFundID = value; }
        }
        public int MasterStrategyID
        {
            get { return _masterStrategyID; }
            set { _masterStrategyID = value; }
        }
        public bool IsSwapped
        {
            get { return _isSwapped; }
            set { _isSwapped = value; }
        }
        public double CostBasisUnrealizedPnL
        {
            get { return _costBasisUnrealizedPnL; }
            set
            {
                _costBasisUnrealizedPnL = value;
            }
        }
        public double CostBasisUnrealizedPnLInBaseCurrency
        {
            get { return _costBasisUnrealizedPnlInBaseCurrency; }
            set { _costBasisUnrealizedPnlInBaseCurrency = value; }
        }
        public double MarketValue
        {
            get { return _marketValue; }
            set
            {
                _marketValue = value;
            }
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
        public bool IsFXRateSavedWithTrade
        {
            get { return _isFXRateSavedWithTrade; }
            set { _isFXRateSavedWithTrade = value; }
        }
        public double CashImpact
        {
            get { return _cashImpact; }
            set { _cashImpact = value; }
        }
        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
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
        public bool IsLiveFeedSharesOutstanding
        {
            get { return _isLiveFeedSharesOutstanding; }
            set { _isLiveFeedSharesOutstanding = value; }
        }
        public double AverageVolume20Day
        {
            get { return _averageVolume20Day; }
            set
            {
                _averageVolume20Day = value;
            }
        }
        public double AverageVolume20DayUnderlyingSymbol
        {
            get { return _averageVolume20DayUnderlyingSymbol; }
            set { _averageVolume20DayUnderlyingSymbol = value; }
        }
        public double PercentageAverageVolumeDeltaAdjusted
        {
            get { return _percentageAverageVolumeDeltaAdjusted; }
            set { _percentageAverageVolumeDeltaAdjusted = value; }
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
        public DateTime ExDividendDate
        {
            get { return _exDividendDate; }
            set { _exDividendDate = value; }
        }
        public double CashImpactInBaseCurrency
        {
            get { return _cashImpactInBaseCurrency; }
            set { _cashImpactInBaseCurrency = value; }
        }
        public double iMidPrice
        {
            get
            {
                if (_iMid == 0)
                {
                    if (_getLastIfZero)
                    {
                        return _lastPrice;
                    }
                }
                return _iMid;
            }
            set { _iMid = value; }
        }
        public bool GetLastIfZero
        {
            get { return _getLastIfZero; }
            set { _getLastIfZero = value; }
        }
        public int HasBeenSentToUser
        {
            get { return _hasBeenSentToUser; }
            set { _hasBeenSentToUser = value; }
        }
        public double YesterdayMarketValueInBaseCurrency
        {
            get { return _yesterdayMarketValueInBaseCurrency; }
            set { _yesterdayMarketValueInBaseCurrency = value; }
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
        public double PercentageAverageVolume
        {
            get { return _percentageAverageVolume; }
            set { _percentageAverageVolume = value; }
        }
        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }
        public double PercentageGainLossCostBasis
        {
            get { return _percentageGainLossCostBasis; }
            set { _percentageGainLossCostBasis = value; }
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
        public PricingSource PricingSource
        {
            get { return _pricingSource; }
            set { _pricingSource = value; }
        }
        public DeltaSource DeltaSource
        {
            get { return _deltaSource; }
            set { _deltaSource = value; }
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
        public double YesterdayUnderlyingMarkPrice
        {
            get { return _yesterdayUnderlyingMarkPrice; }
            set { _yesterdayUnderlyingMarkPrice = value; }
        }
        public string YesterdayUnderlyingMarkPriceStr
        {
            get { return _yesterdayUnderlyingMarkPriceStr; }
            set { _yesterdayUnderlyingMarkPriceStr = value; }
        }
        public double PercentageUnderlyingChange
        {
            get
            {
                if (_yesterdayUnderlyingMarkPrice != 0)
                {
                    return Math.Round((((_underlyingStockPrice - _yesterdayUnderlyingMarkPrice) / _yesterdayUnderlyingMarkPrice) * 100), 4);
                }
                else
                {
                    return 0;
                }
            }
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
        public double YesterdayFXRate
        {
            get { return _yesterdayFXRate; }
            set { _yesterdayFXRate = value; }
        }
        public double ChangeInUnderlyingPrice
        {
            get
            {
                _changeInUnderlyingPrice = Math.Round((_underlyingStockPrice - _yesterdayUnderlyingMarkPrice), 4);
                return _changeInUnderlyingPrice;
            }
        }
        public string ReutersSymbol
        {
            get { return _reutersSymbol; }
            set { _reutersSymbol = value; }
        }
        public double BetaAdjGrossExposure
        {
            get { return _betaAdjGrossExposure; }
            set { _betaAdjGrossExposure = value; }
        }
        public PositionType PositionSideExposureBoxed
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
        public double FxRateDisplay
        {
            get { return _fxRateDisplay; }
            set { _fxRateDisplay = value; }
        }
        public double NetNotionalForCostBasisBreakEven
        {
            get { return _netNotionalForCostBasisBreakEven; }
            set { _netNotionalForCostBasisBreakEven = value; }
        }
        public PositionType PositionSideMV
        {
            get { return _positionSideMV; }
            set { _positionSideMV = value; }
        }
        public PositionType PositionSideExposure
        {
            get { return _positionSideExposure; }
            set { _positionSideExposure = value; }
        }
        public double NavTouch
        {
            get { return _navTouch; }
            set
            {
                _navTouch = value;
            }
        }
        public double NetNotionalValue
        {
            get { return _netNotionalValue; }
            set { _netNotionalValue = value; }
        }
        public double NetNotionalValueInBaseCurrency
        {
            get { return _netNotionalInBaseCurrency; }
            set { _netNotionalInBaseCurrency = value; }
        }

        public string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }

        public string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        public string UnderlyingName
        {
            get { return _underlyingName; }
            set { _underlyingName = value; }
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
        public PricingStatus PricingStatus
        {
            get { return _pricingStatus; }
            set { _pricingStatus = value; }
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

        #endregion

        public EPnlOrder()
        {
            _id = string.Empty;
            _exchangeID = int.MinValue;
            _exchangeName = string.Empty;
            _currencyID = int.MinValue;
            _auecID = 0;
            _assetName = string.Empty;
            _avgPrice = 0;
            _symbol = string.Empty;
            _sideMultiplier = 1;
            _orderSideTagValue = string.Empty;
            _orderStatusTagValue = string.Empty;
            _dayPnLShort = 0;
            _dayPnLLong = 0;
            _dayPnL = 0;
            _dayPnLInBaseCurrency = 0;
            _shortNotionalValue = 0;
            _shortNotionalValueInBaseCurrency = 0;
            _longNotionalValue = 0;
            _longNotionalValueInBaseCurrency = 0;
            _netExposure = 0;
            _exposure = 0;
            _deltaAdjPosition = 0;
            _betaAdjExposure = 0;
            _netExposureInBaseCurrency = 0;
            _exposureInBaseCurrency = 0;
            _betaAdjExposureInBaseCurrency = 0;
            _exposureBPInBaseCurrency = 0;
            _fxRate = 0;
            _fxConversionMethodOperator = Operator.M;
            _feedPriceOperator = Operator.M;
            _multiplier = 1;
            _level1ID = -1;
            _level2ID = -1;
            _transactionSide = PositionType.Long;
            _underlyingSymbol = string.Empty;
            _auecLocalDate = DateTimeConstants.MinValue;
            _description = string.Empty;
            _internalComments = string.Empty;
            _lastUpdatedUTC = DateTimeConstants.MinValue;
            _isServerUpdated = false;
            _fxRateOnTradeDate = 0;
            _fxConversionMethodOnTradeDate = Operator.M;
            _fxRateOnTradeDateStr = string.Empty;
            _yesterdayMarkPrice = 0.0;
            _yesterdayMarkDateActual = DateTimeConstants.MinValue;
            _yesterdayMarkPriceStr = string.Empty;
            _masterFundID = int.MinValue;
            _masterStrategyID = int.MinValue;
            _isSwapped = false;
            _costBasisUnrealizedPnL = 0;
            _costBasisUnrealizedPnlInBaseCurrency = 0;
            _marketValue = 0.0;
            _marketValueInBaseCurrency = 0.0;
            _percentageGainLoss = 0.0;
            _isFXRateSavedWithTrade = false;
            _cashImpact = 0.0;
            _beta = 0.0;
            _marketCapitalization = 0.0;
            _sharesOutstanding = 0.0;
            _averageVolume20Day = 0.0;
            _averageVolume20DayUnderlyingSymbol = 0.0;
            _exDividendDate = DateTimeConstants.MinValue;
            _cashImpactInBaseCurrency = 0.0;
            _midPrice = 0.0;
            _yesterdayMarketValueInBaseCurrency = 0.0;
            _userName = string.Empty;
            _counterPartyName = string.Empty;
            _earnedDividendLocal = 0.0;
            _earnedDividendBase = 0.0;
            _percentageAverageVolume = 0.0;
            _selectedFeedPrice = 0.0;
            _selectedFeedPriceInBaseCurrency = 0.0;
            _settlementDate = DateTimeConstants.MinValue;
            _percentageAverageVolumeDeltaAdjusted = 0.0;
            _tradeDayPnl = 0.0;
            _fxDayPnl = 0.0;
            _fxCostBasisPnl = 0.0;
            _tradeCostBasisPnl = 0.0;
            _percentageGainLossCostBasis = 0.0;
            _leveragedFactor = 1.0;
            _idcoSymbol = string.Empty;
            _osiSymbol = string.Empty;
            _sedolSymbol = string.Empty;
            _cusipSymbol = string.Empty;
            _bloombergSymbol = string.Empty;
            _bloombergSymbolWithExchangeCode = string.Empty;
            _isinSymbol = string.Empty;
            _pricingSource = PricingSource.None;
            _percentDayPnLGrossMV = 0.0;
            _percentDayPnLNetMV = 0.0;
            _counterPartyId = -1;
            _venueId = -1;
            _deltaAdjPositionLME = 0.0;
            _premium = 0.0;
            _premiumDollar = 0.0;
            _forwardPoints = 0.0;
            _UDAAsset = "Undefined";
            _UDACountry = "Undefined";
            _UDASector = "Undefined";
            _UDASubSector = "Undefined";
            _UDASecurityType = "Undefined";
            _deltaSource = DeltaSource.Default;
            _yesterdayUnderlyingMarkPrice = 0.0;
            _yesterdayUnderlyingMarkPriceStr = string.Empty;
            _transactionType = string.Empty;
            _yesterdayMarketValue = 0.0;
            _yesterdayFXRate = 0.0;
            _reutersSymbol = string.Empty;
            _betaAdjGrossExposure = 0.0;
            _positionSideExposureBoxed = PositionType.Long;
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
            _dividendYield = 0.0;
            _closingPrice = 0.0;
            _askPrice = 0.0;
            _bidPrice = 0.0;
            _midPrice = 0.0;
            _highPrice = 0.0;
            _lowPrice = 0.0;
            _lastPrice = 0.0;
            _positionSideExposure = PositionType.Long;
            _positionSideMV = PositionType.Long;
            _changeInUnderlyingPrice = 0.0;
            _navTouch = 0.0;
            _netNotionalValue = 0;
            _netNotionalInBaseCurrency = 0;
            _orderSide = string.Empty;
            _venue = string.Empty;
            _orderTypeTagValue = string.Empty;
            _orderType = string.Empty;
            _underlyingName = string.Empty;
            _factSetSymbol = string.Empty;
            _activSymbol = string.Empty;
            _customUDA8 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA9 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA10 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA11 = ApplicationConstants.CONST_UNDEFINED;
            _customUDA12 = ApplicationConstants.CONST_UNDEFINED;
            _pricingStatus = PricingStatus.None;
        }

        public EPnlOrder Clone()
        {
            EPnlOrder targetEPnLOrder = new EPnlOrder();
            targetEPnLOrder.EpnlOrderState = EpnlOrderState;
            targetEPnLOrder.IsMarkedForCalculation = _isMarkedForCalculation;
            targetEPnLOrder.ID = _id;
            targetEPnLOrder.Symbol = _symbol;
            targetEPnLOrder.AUECID = _auecID;
            targetEPnLOrder.AssetName = _assetName;
            targetEPnLOrder.AUECLocalDate = _auecLocalDate;
            targetEPnLOrder.Asset = _asset;
            targetEPnLOrder.OrderSideTagValue = _orderSideTagValue;
            targetEPnLOrder.AvgPrice = _avgPrice;
            targetEPnLOrder.NetExposure = _netExposure;
            targetEPnLOrder.Exposure = _exposure;
            targetEPnLOrder.DeltaAdjPosition = _deltaAdjPosition;
            targetEPnLOrder.BetaAdjExposure = _betaAdjExposure;
            targetEPnLOrder.DayPnL = _dayPnL;
            targetEPnLOrder.SideMultiplier = _sideMultiplier;
            targetEPnLOrder.FxRate = _fxRate;
            targetEPnLOrder.LastPrice = _lastPrice;
            targetEPnLOrder.ClosingPrice = _closingPrice;
            targetEPnLOrder.Multiplier = _multiplier;
            targetEPnLOrder.Level1ID = _level1ID;
            targetEPnLOrder.Level2ID = _level2ID;
            targetEPnLOrder.UnderlyingSymbol = _underlyingSymbol;
            targetEPnLOrder.YesterdayMarkPrice = _yesterdayMarkPrice;
            targetEPnLOrder.YesterdayMarkDateActual = _yesterdayMarkDateActual;
            targetEPnLOrder.TransactionDate = _transactionDate;
            targetEPnLOrder.DayPnLInBaseCurrency = _dayPnLInBaseCurrency;
            targetEPnLOrder.NetExposureInBaseCurrency = _netExposureInBaseCurrency;
            targetEPnLOrder.ExposureInBaseCurrency = _exposureInBaseCurrency;
            targetEPnLOrder.BetaAdjExposureInBaseCurrency = _betaAdjExposureInBaseCurrency;
            targetEPnLOrder.AskPrice = _askPrice;
            targetEPnLOrder.BidPrice = _bidPrice;
            targetEPnLOrder.DividendYield = _dividendYield;
            targetEPnLOrder.MidPrice = _midPrice;
            targetEPnLOrder.FullSecurityName = _fullSecurityName;
            targetEPnLOrder.FXConversionMethodOperator = _fxConversionMethodOperator;
            targetEPnLOrder.FeedPriceOperator = _feedPriceOperator;
            targetEPnLOrder.Description = _description;
            targetEPnLOrder.InternalComments = _internalComments;
            targetEPnLOrder.YesterdayMarkPriceStr = _yesterdayMarkPriceStr;
            targetEPnLOrder.FXRateOnTradeDate = _fxRateOnTradeDate;
            targetEPnLOrder.FXRateOnTradeDateStr = _fxRateOnTradeDateStr;
            targetEPnLOrder.CurrencyID = _currencyID;
            targetEPnLOrder.MasterFundID = _masterFundID;
            targetEPnLOrder.MasterStrategyID = _masterStrategyID;
            targetEPnLOrder.CostBasisUnrealizedPnL = _costBasisUnrealizedPnL;
            targetEPnLOrder.CostBasisUnrealizedPnLInBaseCurrency = _costBasisUnrealizedPnlInBaseCurrency;

            targetEPnLOrder.MarketValue = _marketValue;
            targetEPnLOrder.MarketValueInBaseCurrency = _marketValueInBaseCurrency;
            targetEPnLOrder.TransactionSide = _transactionSide;
            targetEPnLOrder.PercentageGainLoss = _percentageGainLoss;
            targetEPnLOrder.LastUpdatedUTC = _lastUpdatedUTC;

            targetEPnLOrder.Quantity = _quantity;
            targetEPnLOrder.ExposureBPInBaseCurrency = _exposureBPInBaseCurrency;
            targetEPnLOrder.CashImpact = _cashImpact;
            targetEPnLOrder.Beta = _beta;
            targetEPnLOrder.ExDividendDate = _exDividendDate;
            targetEPnLOrder.MarketCapitalization = _marketCapitalization;
            targetEPnLOrder.SharesOutstanding = _sharesOutstanding;
            targetEPnLOrder.AverageVolume20Day = _averageVolume20Day;
            targetEPnLOrder.AverageVolume20DayUnderlyingSymbol = _averageVolume20DayUnderlyingSymbol;
            targetEPnLOrder.CashImpactInBaseCurrency = _cashImpactInBaseCurrency;
            targetEPnLOrder.UserName = _userName;
            targetEPnLOrder.CounterPartyName = _counterPartyName;
            targetEPnLOrder.HasBeenSentToUser = _hasBeenSentToUser;
            targetEPnLOrder.EarnedDividendBase = _earnedDividendBase;
            targetEPnLOrder.EarnedDividendLocal = _earnedDividendLocal;
            targetEPnLOrder.PercentageAverageVolume = _percentageAverageVolume;
            targetEPnLOrder.UnderlyingID = _underlyingID;
            targetEPnLOrder.ExchangeID = _exchangeID;
            targetEPnLOrder.ExchangeName = _exchangeName;
            targetEPnLOrder.SelectedFeedPrice = _selectedFeedPrice;
            targetEPnLOrder.SelectedFeedPriceInBaseCurrency = _selectedFeedPriceInBaseCurrency;
            targetEPnLOrder.SettlementDate = _settlementDate;
            targetEPnLOrder.PercentageAverageVolumeDeltaAdjusted = _percentageAverageVolumeDeltaAdjusted;
            targetEPnLOrder.TradeDayPnl = _tradeDayPnl;
            targetEPnLOrder.FxDayPnl = _fxDayPnl;
            targetEPnLOrder.FxCostBasisPnl = _fxCostBasisPnl;
            targetEPnLOrder.TradeCostBasisPnl = _tradeCostBasisPnl;
            targetEPnLOrder.PercentageGainLossCostBasis = _percentageGainLossCostBasis;
            targetEPnLOrder.UnderlyingValueForOptions = _underlyingValueForOptions;
            targetEPnLOrder.LeveragedFactor = _leveragedFactor;

            //http://jira.nirvanasolutions.com:8080/browse/TW-40
            targetEPnLOrder.IdcoSymbol = _idcoSymbol;
            targetEPnLOrder.OsiSymbol = _osiSymbol;
            targetEPnLOrder.SedolSymbol = _sedolSymbol;
            targetEPnLOrder.CusipSymbol = _cusipSymbol;
            targetEPnLOrder.BloombergSymbol = _bloombergSymbol;
            targetEPnLOrder.BloombergSymbolWithExchangeCode = _bloombergSymbolWithExchangeCode;
            targetEPnLOrder.FactSetSymbol = _factSetSymbol;
            targetEPnLOrder.ActivSymbol = _activSymbol;
            targetEPnLOrder.IsinSymbol = _isinSymbol;
            targetEPnLOrder.TradeAttribute1 = _tradeAttribute1;
            targetEPnLOrder.TradeAttribute2 = _tradeAttribute2;
            targetEPnLOrder.TradeAttribute3 = _tradeAttribute3;
            targetEPnLOrder.TradeAttribute4 = _tradeAttribute4;
            targetEPnLOrder.TradeAttribute5 = _tradeAttribute5;
            targetEPnLOrder.TradeAttribute6 = _tradeAttribute6;
            targetEPnLOrder.ProxySymbol = _proxySymbol;
            targetEPnLOrder.PricingSource = _pricingSource;
            targetEPnLOrder.PricingStatus = _pricingStatus;
            targetEPnLOrder.CounterPartyId = _counterPartyId;
            targetEPnLOrder.VenueId = _venueId;

            targetEPnLOrder.PercentDayPnLGrossMV = _percentDayPnLGrossMV;
            targetEPnLOrder.PercentDayPnLNetMV = _percentDayPnLNetMV;
            targetEPnLOrder.DeltaAdjPositionLME = _deltaAdjPositionLME;
            targetEPnLOrder.Premium = _premium;
            targetEPnLOrder.PremiumDollar = _premiumDollar;

            #region UDA DATA related fields

            targetEPnLOrder.UDAAsset = _UDAAsset;
            targetEPnLOrder.UDACountry = _UDACountry;
            targetEPnLOrder.UDASector = _UDASector;
            targetEPnLOrder.UDASubSector = _UDASubSector;
            targetEPnLOrder.UDASecurityType = _UDASecurityType;
            targetEPnLOrder.ForwardPoints = _forwardPoints;

            #endregion UDA DATA related fields

            targetEPnLOrder.DeltaSource = _deltaSource;
            targetEPnLOrder.YesterdayUnderlyingMarkPrice = _yesterdayUnderlyingMarkPrice;
            targetEPnLOrder.YesterdayUnderlyingMarkPriceStr = _yesterdayUnderlyingMarkPriceStr;
            targetEPnLOrder.TransactionType = _transactionType;
            targetEPnLOrder.ReutersSymbol = _reutersSymbol;
            targetEPnLOrder.BetaAdjGrossExposure = _betaAdjGrossExposure;
            targetEPnLOrder.PositionSideExposureBoxed = _positionSideExposureBoxed;
            targetEPnLOrder.Analyst = _analyst;
            targetEPnLOrder.CountryOfRisk = _countryOfRisk;
            targetEPnLOrder.CustomUDA1 = _customUDA1;
            targetEPnLOrder.CustomUDA2 = _customUDA2;
            targetEPnLOrder.CustomUDA3 = _customUDA3;
            targetEPnLOrder.CustomUDA4 = _customUDA4;
            targetEPnLOrder.CustomUDA5 = _customUDA5;
            targetEPnLOrder.CustomUDA6 = _customUDA6;
            targetEPnLOrder.CustomUDA7 = _customUDA7;
            targetEPnLOrder.Issuer = _issuer;
            targetEPnLOrder.LiquidTag = _liquidTag;
            targetEPnLOrder.MarketCap = _marketCap;
            targetEPnLOrder.Region = _region;
            targetEPnLOrder.RiskCurrency = _riskCurrency;
            targetEPnLOrder.UcitsEligibleTag = _ucitsEligibleTag;
            targetEPnLOrder.FxRateDisplay = _fxRateDisplay;
            targetEPnLOrder.NetNotionalForCostBasisBreakEven = _netNotionalForCostBasisBreakEven;
            targetEPnLOrder.PositionSideMV = _positionSideMV;
            targetEPnLOrder.PositionSideExposure = _positionSideExposure;
            targetEPnLOrder.NavTouch = _navTouch;
            targetEPnLOrder.NetNotionalValue = _netNotionalValue;
            targetEPnLOrder.NetNotionalValueInBaseCurrency = _netNotionalInBaseCurrency;
            targetEPnLOrder.OrderSide = _orderSide;
            targetEPnLOrder.Venue = _venue;
            targetEPnLOrder.OrderTypeTagValue = _orderTypeTagValue;
            targetEPnLOrder.OrderType = _orderType;
            targetEPnLOrder.UnderlyingName = _underlyingName;
            targetEPnLOrder.DayTradedPosition = _dayTradedPosition;
            targetEPnLOrder.TradeVolume = _tradeVolume;
            targetEPnLOrder.CustomUDA8 = _customUDA8;
            targetEPnLOrder.CustomUDA9 = _customUDA9;
            targetEPnLOrder.CustomUDA10 = _customUDA10;
            targetEPnLOrder.CustomUDA11 = _customUDA11;
            targetEPnLOrder.CustomUDA12 = _customUDA12;

            targetEPnLOrder.TradeAttribute7 = TradeAttribute7;
            targetEPnLOrder.TradeAttribute8 = TradeAttribute8;
            targetEPnLOrder.TradeAttribute9 = TradeAttribute9;
            targetEPnLOrder.TradeAttribute10 = TradeAttribute10;
            targetEPnLOrder.TradeAttribute11 = TradeAttribute11;
            targetEPnLOrder.TradeAttribute12 = TradeAttribute12;
            targetEPnLOrder.TradeAttribute13 = TradeAttribute13;
            targetEPnLOrder.TradeAttribute14 = TradeAttribute14;
            targetEPnLOrder.TradeAttribute15 = TradeAttribute15;
            targetEPnLOrder.TradeAttribute16 = TradeAttribute16;
            targetEPnLOrder.TradeAttribute17 = TradeAttribute17;
            targetEPnLOrder.TradeAttribute18 = TradeAttribute18;
            targetEPnLOrder.TradeAttribute19 = TradeAttribute19;
            targetEPnLOrder.TradeAttribute20 = TradeAttribute20;
            targetEPnLOrder.TradeAttribute21 = TradeAttribute21;
            targetEPnLOrder.TradeAttribute22 = TradeAttribute22;
            targetEPnLOrder.TradeAttribute23 = TradeAttribute23;
            targetEPnLOrder.TradeAttribute24 = TradeAttribute24;
            targetEPnLOrder.TradeAttribute25 = TradeAttribute25;
            targetEPnLOrder.TradeAttribute26 = TradeAttribute26;
            targetEPnLOrder.TradeAttribute27 = TradeAttribute27;
            targetEPnLOrder.TradeAttribute28 = TradeAttribute28;
            targetEPnLOrder.TradeAttribute29 = TradeAttribute29;
            targetEPnLOrder.TradeAttribute30 = TradeAttribute30;
            targetEPnLOrder.TradeAttribute31 = TradeAttribute31;
            targetEPnLOrder.TradeAttribute32 = TradeAttribute32;
            targetEPnLOrder.TradeAttribute33 = TradeAttribute33;
            targetEPnLOrder.TradeAttribute34 = TradeAttribute34;
            targetEPnLOrder.TradeAttribute35 = TradeAttribute35;
            targetEPnLOrder.TradeAttribute36 = TradeAttribute36;
            targetEPnLOrder.TradeAttribute37 = TradeAttribute37;
            targetEPnLOrder.TradeAttribute38 = TradeAttribute38;
            targetEPnLOrder.TradeAttribute39 = TradeAttribute39;
            targetEPnLOrder.TradeAttribute40 = TradeAttribute40;
            targetEPnLOrder.TradeAttribute41 = TradeAttribute41;
            targetEPnLOrder.TradeAttribute42 = TradeAttribute42;
            targetEPnLOrder.TradeAttribute43 = TradeAttribute43;
            targetEPnLOrder.TradeAttribute44 = TradeAttribute44;
            targetEPnLOrder.TradeAttribute45 = TradeAttribute45;
            return targetEPnLOrder;
        }

        public void Clone(EPnlOrder targetEPnLOrder)
        {
            targetEPnLOrder.IsMarkedForCalculation = _isMarkedForCalculation;
            targetEPnLOrder.ID = _id;
            targetEPnLOrder.Symbol = _symbol;
            targetEPnLOrder.AUECID = _auecID;
            targetEPnLOrder.AssetName = _assetName;
            targetEPnLOrder.AUECLocalDate = _auecLocalDate;
            targetEPnLOrder.Asset = _asset;
            targetEPnLOrder.OrderSideTagValue = _orderSideTagValue;
            targetEPnLOrder.AvgPrice = _avgPrice;
            targetEPnLOrder.NetExposure = _netExposure;
            targetEPnLOrder.Exposure = _exposure;
            targetEPnLOrder.DeltaAdjPosition = _deltaAdjPosition;
            targetEPnLOrder.BetaAdjExposure = _betaAdjExposure;
            targetEPnLOrder.DayPnL = _dayPnL;
            targetEPnLOrder.SideMultiplier = _sideMultiplier;
            targetEPnLOrder.FxRate = _fxRate;
            targetEPnLOrder.LastPrice = _lastPrice;
            targetEPnLOrder.ClosingPrice = _closingPrice;
            targetEPnLOrder.Multiplier = _multiplier;
            targetEPnLOrder.Level1ID = _level1ID;
            targetEPnLOrder.Level2ID = _level2ID;
            targetEPnLOrder.UnderlyingSymbol = _underlyingSymbol;
            targetEPnLOrder.YesterdayMarkPrice = _yesterdayMarkPrice;
            targetEPnLOrder.YesterdayMarkDateActual = _yesterdayMarkDateActual;
            targetEPnLOrder.TransactionDate = _transactionDate;
            targetEPnLOrder.DayPnLInBaseCurrency = _dayPnLInBaseCurrency;
            targetEPnLOrder.NetExposureInBaseCurrency = _netExposureInBaseCurrency;
            targetEPnLOrder.ExposureInBaseCurrency = _exposureInBaseCurrency;
            targetEPnLOrder.BetaAdjExposureInBaseCurrency = _betaAdjExposureInBaseCurrency;
            targetEPnLOrder.AskPrice = _askPrice;
            targetEPnLOrder.BidPrice = _bidPrice;
            targetEPnLOrder.DividendYield = _dividendYield;
            targetEPnLOrder.MidPrice = _midPrice;
            targetEPnLOrder.FullSecurityName = _fullSecurityName;
            targetEPnLOrder.FXConversionMethodOperator = _fxConversionMethodOperator;
            targetEPnLOrder.FXConversionMethodOnTradeDate = _fxConversionMethodOnTradeDate;
            targetEPnLOrder.FeedPriceOperator = _feedPriceOperator;
            targetEPnLOrder.Description = _description;
            targetEPnLOrder.InternalComments = _internalComments;
            targetEPnLOrder.YesterdayMarkPriceStr = _yesterdayMarkPriceStr;
            targetEPnLOrder.IsFXRateSavedWithTrade = _isFXRateSavedWithTrade;
            targetEPnLOrder.FXRateOnTradeDate = _fxRateOnTradeDate;
            targetEPnLOrder.FXRateOnTradeDateStr = _fxRateOnTradeDateStr;
            targetEPnLOrder.CurrencyID = _currencyID;
            targetEPnLOrder.MasterFundID = _masterFundID;
            targetEPnLOrder.MasterStrategyID = _masterStrategyID;
            targetEPnLOrder.CostBasisUnrealizedPnL = _costBasisUnrealizedPnL;
            targetEPnLOrder.CostBasisUnrealizedPnLInBaseCurrency = _costBasisUnrealizedPnlInBaseCurrency;
            targetEPnLOrder.MarketValue = _marketValue;
            targetEPnLOrder.MarketValueInBaseCurrency = _marketValueInBaseCurrency;
            targetEPnLOrder.TransactionSide = _transactionSide;
            targetEPnLOrder.PercentageGainLoss = _percentageGainLoss;
            targetEPnLOrder.LastUpdatedUTC = _lastUpdatedUTC;
            targetEPnLOrder.Quantity = _quantity;
            targetEPnLOrder.ExposureBPInBaseCurrency = _exposureBPInBaseCurrency;
            targetEPnLOrder.CashImpact = _cashImpact;
            targetEPnLOrder.Beta = _beta;
            targetEPnLOrder.ExDividendDate = _exDividendDate;
            targetEPnLOrder.MarketCapitalization = _marketCapitalization;
            targetEPnLOrder.SharesOutstanding = _sharesOutstanding;
            targetEPnLOrder.AverageVolume20DayUnderlyingSymbol = _averageVolume20DayUnderlyingSymbol;
            targetEPnLOrder.AverageVolume20Day = _averageVolume20Day;
            targetEPnLOrder.CashImpactInBaseCurrency = _cashImpactInBaseCurrency;
            targetEPnLOrder.HasBeenSentToUser = _hasBeenSentToUser;
            targetEPnLOrder.YesterdayMarketValueInBaseCurrency = _yesterdayMarketValueInBaseCurrency;
            targetEPnLOrder.CounterPartyName = _counterPartyName;
            targetEPnLOrder.UserName = _userName;
            targetEPnLOrder.EarnedDividendBase = _earnedDividendBase;
            targetEPnLOrder.EarnedDividendLocal = _earnedDividendLocal;
            targetEPnLOrder.PercentageAverageVolume = _percentageAverageVolume;
            targetEPnLOrder.UnderlyingID = _underlyingID;
            targetEPnLOrder.ExchangeID = _exchangeID;
            targetEPnLOrder.ExchangeName = _exchangeName;
            targetEPnLOrder.SelectedFeedPrice = _selectedFeedPrice;
            targetEPnLOrder.SelectedFeedPriceInBaseCurrency = _selectedFeedPriceInBaseCurrency;
            targetEPnLOrder.SettlementDate = _settlementDate;
            targetEPnLOrder.PercentageAverageVolumeDeltaAdjusted = _percentageAverageVolumeDeltaAdjusted;
            targetEPnLOrder.TradeDayPnl = _tradeDayPnl;
            targetEPnLOrder.FxDayPnl = _fxDayPnl;
            targetEPnLOrder.FxCostBasisPnl = _fxCostBasisPnl;
            targetEPnLOrder.TradeCostBasisPnl = _tradeCostBasisPnl;
            targetEPnLOrder.PercentageGainLossCostBasis = _percentageGainLossCostBasis;
            targetEPnLOrder.UnderlyingValueForOptions = _underlyingValueForOptions;
            targetEPnLOrder.LeveragedFactor = _leveragedFactor;
            targetEPnLOrder.IdcoSymbol = _idcoSymbol;
            targetEPnLOrder.OsiSymbol = _osiSymbol;
            targetEPnLOrder.SedolSymbol = _sedolSymbol;
            targetEPnLOrder.CusipSymbol = _cusipSymbol;
            targetEPnLOrder.BloombergSymbol = _bloombergSymbol;
            targetEPnLOrder.FactSetSymbol = _factSetSymbol;
            targetEPnLOrder.ActivSymbol = _activSymbol;
            targetEPnLOrder.IsinSymbol = _isinSymbol;
            targetEPnLOrder.TradeAttribute1 = _tradeAttribute1;
            targetEPnLOrder.TradeAttribute2 = _tradeAttribute2;
            targetEPnLOrder.TradeAttribute3 = _tradeAttribute3;
            targetEPnLOrder.TradeAttribute4 = _tradeAttribute4;
            targetEPnLOrder.TradeAttribute5 = _tradeAttribute5;
            targetEPnLOrder.TradeAttribute6 = _tradeAttribute6;
            targetEPnLOrder.ProxySymbol = _proxySymbol;
            targetEPnLOrder.PricingSource = _pricingSource;
            targetEPnLOrder.PricingStatus = _pricingStatus;
            targetEPnLOrder.CounterPartyId = _counterPartyId;
            targetEPnLOrder.VenueId = _venueId;
            targetEPnLOrder.PercentDayPnLGrossMV = _percentDayPnLGrossMV;
            targetEPnLOrder.PercentDayPnLNetMV = _percentDayPnLNetMV;
            targetEPnLOrder.DeltaAdjPositionLME = _deltaAdjPositionLME;
            targetEPnLOrder.Premium = _premium;
            targetEPnLOrder.PremiumDollar = _premiumDollar;
            targetEPnLOrder.ForwardPoints = _forwardPoints;
            targetEPnLOrder.UDAAsset = _UDAAsset;
            targetEPnLOrder.UDACountry = _UDACountry;
            targetEPnLOrder.UDASector = _UDASector;
            targetEPnLOrder.UDASubSector = _UDASubSector;
            targetEPnLOrder.UDASecurityType = _UDASecurityType;
            targetEPnLOrder.DeltaSource = _deltaSource;
            targetEPnLOrder.YesterdayUnderlyingMarkPrice = _yesterdayUnderlyingMarkPrice;
            targetEPnLOrder.YesterdayUnderlyingMarkPriceStr = _yesterdayUnderlyingMarkPriceStr;
            targetEPnLOrder.TransactionType = _transactionType;
            targetEPnLOrder.YesterdayMarketValue = _yesterdayMarketValue;
            targetEPnLOrder.YesterdayFXRate = _yesterdayFXRate;
            targetEPnLOrder.ReutersSymbol = _reutersSymbol;
            targetEPnLOrder.BetaAdjGrossExposure = _betaAdjGrossExposure;
            targetEPnLOrder.PositionSideExposureBoxed = _positionSideExposureBoxed;
            targetEPnLOrder.Analyst = _analyst;
            targetEPnLOrder.CountryOfRisk = _countryOfRisk;
            targetEPnLOrder.CustomUDA1 = _customUDA1;
            targetEPnLOrder.CustomUDA2 = _customUDA2;
            targetEPnLOrder.CustomUDA3 = _customUDA3;
            targetEPnLOrder.CustomUDA4 = _customUDA4;
            targetEPnLOrder.CustomUDA5 = _customUDA5;
            targetEPnLOrder.CustomUDA6 = _customUDA6;
            targetEPnLOrder.CustomUDA7 = _customUDA7;
            targetEPnLOrder.Issuer = _issuer;
            targetEPnLOrder.LiquidTag = _liquidTag;
            targetEPnLOrder.MarketCap = _marketCap;
            targetEPnLOrder.Region = _region;
            targetEPnLOrder.RiskCurrency = _riskCurrency;
            targetEPnLOrder.UcitsEligibleTag = _ucitsEligibleTag;
            targetEPnLOrder.FxRateDisplay = _fxRateDisplay;
            targetEPnLOrder.NetNotionalForCostBasisBreakEven = _netNotionalForCostBasisBreakEven;
            targetEPnLOrder.PositionSideMV = _positionSideMV;
            targetEPnLOrder.PositionSideExposure = _positionSideExposure;
            targetEPnLOrder.NavTouch = _navTouch;
            targetEPnLOrder.NetNotionalValue = _netNotionalValue;
            targetEPnLOrder.NetNotionalValueInBaseCurrency = _netNotionalInBaseCurrency;
            targetEPnLOrder.OrderSide = _orderSide;
            targetEPnLOrder.Venue = _venue;
            targetEPnLOrder.OrderTypeTagValue = _orderTypeTagValue;
            targetEPnLOrder.OrderType = _orderType;
            targetEPnLOrder.UnderlyingName = _underlyingName;
            targetEPnLOrder.DayTradedPosition = _dayTradedPosition;
            targetEPnLOrder.TradeVolume = _tradeVolume;
            targetEPnLOrder.CustomUDA8 = _customUDA8;
            targetEPnLOrder.CustomUDA9 = _customUDA9;
            targetEPnLOrder.CustomUDA10 = _customUDA10;
            targetEPnLOrder.CustomUDA11 = _customUDA11;
            targetEPnLOrder.CustomUDA12 = _customUDA12;
            targetEPnLOrder.BloombergSymbolWithExchangeCode = _bloombergSymbolWithExchangeCode;
            targetEPnLOrder.TradeAttribute7 = TradeAttribute7;
            targetEPnLOrder.TradeAttribute8 = TradeAttribute8;
            targetEPnLOrder.TradeAttribute9 = TradeAttribute9;
            targetEPnLOrder.TradeAttribute10 = TradeAttribute10;
            targetEPnLOrder.TradeAttribute11 = TradeAttribute11;
            targetEPnLOrder.TradeAttribute12 = TradeAttribute12;
            targetEPnLOrder.TradeAttribute13 = TradeAttribute13;
            targetEPnLOrder.TradeAttribute14 = TradeAttribute14;
            targetEPnLOrder.TradeAttribute15 = TradeAttribute15;
            targetEPnLOrder.TradeAttribute16 = TradeAttribute16;
            targetEPnLOrder.TradeAttribute17 = TradeAttribute17;
            targetEPnLOrder.TradeAttribute18 = TradeAttribute18;
            targetEPnLOrder.TradeAttribute19 = TradeAttribute19;
            targetEPnLOrder.TradeAttribute20 = TradeAttribute20;
            targetEPnLOrder.TradeAttribute21 = TradeAttribute21;
            targetEPnLOrder.TradeAttribute22 = TradeAttribute22;
            targetEPnLOrder.TradeAttribute23 = TradeAttribute23;
            targetEPnLOrder.TradeAttribute24 = TradeAttribute24;
            targetEPnLOrder.TradeAttribute25 = TradeAttribute25;
            targetEPnLOrder.TradeAttribute26 = TradeAttribute26;
            targetEPnLOrder.TradeAttribute27 = TradeAttribute27;
            targetEPnLOrder.TradeAttribute28 = TradeAttribute28;
            targetEPnLOrder.TradeAttribute29 = TradeAttribute29;
            targetEPnLOrder.TradeAttribute30 = TradeAttribute30;
            targetEPnLOrder.TradeAttribute31 = TradeAttribute31;
            targetEPnLOrder.TradeAttribute32 = TradeAttribute32;
            targetEPnLOrder.TradeAttribute33 = TradeAttribute33;
            targetEPnLOrder.TradeAttribute34 = TradeAttribute34;
            targetEPnLOrder.TradeAttribute35 = TradeAttribute35;
            targetEPnLOrder.TradeAttribute36 = TradeAttribute36;
            targetEPnLOrder.TradeAttribute37 = TradeAttribute37;
            targetEPnLOrder.TradeAttribute38 = TradeAttribute38;
            targetEPnLOrder.TradeAttribute39 = TradeAttribute39;
            targetEPnLOrder.TradeAttribute40 = TradeAttribute40;
            targetEPnLOrder.TradeAttribute41 = TradeAttribute41;
            targetEPnLOrder.TradeAttribute42 = TradeAttribute42;
            targetEPnLOrder.TradeAttribute43 = TradeAttribute43;
            targetEPnLOrder.TradeAttribute44 = TradeAttribute44;
            targetEPnLOrder.TradeAttribute45 = TradeAttribute45;
        }

        #region GetBindableObject
        /// <summary>
        /// Bind/Update ExposurePnlCacheItem object from EpnlOrder object
        /// </summary>
        /// <param name="bindableObject"></param>
        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            try
            {
                if (bindableObject == null)
                {
                    bindableObject = new ExposurePnlCacheItem();
                }
                bindableObject.Asset = _asset.ToString();
                bindableObject.AUECID = _auecID;
                bindableObject.AverageVolume20Day = _averageVolume20Day;
                bindableObject.AverageVolume20DayUnderlyingSymbol = _averageVolume20DayUnderlyingSymbol;
                bindableObject.AvgPrice = _avgPrice;
                bindableObject.Beta = _beta;
                bindableObject.CashImpact = _cashImpact;
                bindableObject.CashImpactInBaseCurrency = _cashImpactInBaseCurrency;
                bindableObject.CostBasisUnrealizedPnL = _costBasisUnrealizedPnL;
                bindableObject.CostBasisUnrealizedPnLInBaseCurrency = _costBasisUnrealizedPnlInBaseCurrency;
                bindableObject.CurrencyID = _currencyID;
                bindableObject.DayPnL = _dayPnL;
                bindableObject.DayPnLInBaseCurrency = _dayPnLInBaseCurrency;
                bindableObject.Description = _description;
                bindableObject.InternalComments = _internalComments;
                bindableObject.ExchangeID = _exchangeID;

                if (_exDividendDate != null)
                    bindableObject.ExDividendDate = _exDividendDate;
                else
                    bindableObject.ExDividendDate = DateTime.MinValue;
                bindableObject.NetNotionalValue = _netNotionalValue;
                bindableObject.NetNotionalValueInBaseCurrency = _netNotionalInBaseCurrency;
                bindableObject.ExposureBPInBaseCurrency = _exposureBPInBaseCurrency;
                bindableObject.FeedPriceOperator = _feedPriceOperator;
                bindableObject.FullSecurityName = _fullSecurityName;
                bindableObject.ID = _id;
                bindableObject.Level1ID = _level1ID;
                bindableObject.Level2ID = _level2ID;
                bindableObject.MarketCapitalization = _marketCapitalization;
                bindableObject.MarketValue = _marketValue;
                bindableObject.GrossMarketValue = Math.Abs(_marketValueInBaseCurrency);
                bindableObject.MarketValueInBaseCurrency = _marketValueInBaseCurrency;
                bindableObject.MasterFundID = _masterFundID;
                bindableObject.MasterStrategyID = _masterStrategyID;
                bindableObject.Multiplier = _multiplier;
                bindableObject.NetExposure = _netExposure;
                bindableObject.Exposure = _exposure;
                bindableObject.DeltaAdjPosition = _deltaAdjPosition;
                bindableObject.GrossExposure = Math.Abs(_netExposureInBaseCurrency);
                bindableObject.GrossExposureLocal = Math.Abs(_netExposure);
                bindableObject.NetExposureInBaseCurrency = _netExposureInBaseCurrency;
                bindableObject.ExposureInBaseCurrency = _exposureInBaseCurrency;
                bindableObject.BetaAdjExposure = _betaAdjExposure;
                bindableObject.BetaAdjExposureInBaseCurrency = _betaAdjExposureInBaseCurrency;
                bindableObject.OrderSideTagValue = _orderSideTagValue;
                bindableObject.PercentageChange = PercentageChange;
                bindableObject.PercentageGainLoss = _percentageGainLoss;
                bindableObject.Quantity = _quantity * _sideMultiplier;
                bindableObject.SharesOutstanding = _sharesOutstanding;
                bindableObject.SideMultiplier = _sideMultiplier;
                bindableObject.Symbol = _symbol;
                bindableObject.TradeDate = _transactionDate;
                bindableObject.TransactionSide = _transactionSide.ToString();
                bindableObject.UnderlyingSymbol = _underlyingSymbol;
                bindableObject.UnderlyingStockPrice = _underlyingStockPrice;
                bindableObject.UnderlyingValueForOptions = _underlyingValueForOptions;
                bindableObject.FXConversionMethodOperator = _fxConversionMethodOperator;
                bindableObject.FxRate = _fxRate;
                bindableObject.LastPrice = _lastPrice;
                bindableObject.AskPrice = _askPrice;
                bindableObject.BidPrice = _bidPrice;
                bindableObject.ClosingPrice = _closingPrice;
                bindableObject.HighPrice = _highPrice;
                if (_lastUpdatedUTC != DateTimeConstants.MinValue)
                {
                    bindableObject.LastUpdatedUTC = _lastUpdatedUTC;
                }
                bindableObject.LowPrice = _lowPrice;
                bindableObject.MidPrice = _midPrice;
                bindableObject.FXRateOnTradeDateStr = _fxRateOnTradeDateStr;
                bindableObject.YesterdayMarkPrice = _yesterdayMarkPrice;
                bindableObject.YesterdayMarkPriceStr = _yesterdayMarkPriceStr;
                bindableObject.HasBeenSentToUser = _hasBeenSentToUser;
                bindableObject.UserName = _userName;
                bindableObject.CounterPartyName = _counterPartyName;
                bindableObject.EarnedDividendBase = _earnedDividendBase;
                bindableObject.EarnedDividendLocal = _earnedDividendLocal;
                bindableObject.PercentageAverageVolume = _percentageAverageVolume;
                bindableObject.UnderlyingID = _underlyingID;
                bindableObject.DividendYield = _dividendYield;
                bindableObject.SelectedFeedPrice = SelectedFeedPrice;
                bindableObject.SelectedFeedPriceInBaseCurrency = SelectedFeedPriceInBaseCurrency;
                bindableObject.SettlementDate = _settlementDate;
                bindableObject.PercentageAverageVolumeDeltaAdjusted = _percentageAverageVolumeDeltaAdjusted;
                bindableObject.TradeDayPnl = _tradeDayPnl;
                bindableObject.FxDayPnl = _fxDayPnl;
                bindableObject.FxCostBasisPnl = _fxCostBasisPnl;
                bindableObject.TradeCostBasisPnl = _tradeCostBasisPnl;
                bindableObject.IsSwap = _isSwapped;
                bindableObject.PercentageGainLossCostBasis = _percentageGainLossCostBasis;
                bindableObject.LeveragedFactor = _leveragedFactor;
                bindableObject.IdcoSymbol = _idcoSymbol;
                bindableObject.OsiSymbol = _osiSymbol;
                bindableObject.SedolSymbol = _sedolSymbol;
                bindableObject.CusipSymbol = _cusipSymbol;
                bindableObject.BloombergSymbol = _bloombergSymbol;
                bindableObject.BloombergSymbolWithExchangeCode = _bloombergSymbolWithExchangeCode;
                bindableObject.FactSetSymbol = _factSetSymbol;
                bindableObject.ActivSymbol = _activSymbol;
                bindableObject.IsinSymbol = _isinSymbol;
                bindableObject.StartTradeDate = _transactionDate;
                bindableObject.TradeAttribute1 = _tradeAttribute1;
                bindableObject.TradeAttribute2 = _tradeAttribute2;
                bindableObject.TradeAttribute3 = _tradeAttribute3;
                bindableObject.TradeAttribute4 = _tradeAttribute4;
                bindableObject.TradeAttribute5 = _tradeAttribute5;
                bindableObject.TradeAttribute6 = _tradeAttribute6;
                bindableObject.CostBasisBreakEven = _avgPrice;
                bindableObject.ProxySymbol = _proxySymbol;
                bindableObject.PricingSource = _pricingSource.ToString();
                bindableObject.PricingStatus = _pricingStatus.ToString();
                bindableObject.PercentDayPnLGrossMV = _percentDayPnLGrossMV;
                bindableObject.PercentDayPnLNetMV = _percentDayPnLNetMV;
                bindableObject.DeltaAdjPositionLME = _deltaAdjPositionLME;
                bindableObject.Premium = _premium;
                bindableObject.PremiumDollar = _premiumDollar;
                bindableObject.UDAAsset = _UDAAsset;
                bindableObject.UDACountry = _UDACountry;
                bindableObject.UDASector = _UDASector;
                bindableObject.UDASubSector = _UDASubSector;
                bindableObject.UDASecurityType = _UDASecurityType;
                bindableObject.ForwardPoints = _forwardPoints;
                bindableObject.DeltaSource = _deltaSource;

                if (bindableObject.Asset != AssetCategory.EquityOption.ToString() && bindableObject.Asset != AssetCategory.FutureOption.ToString() && bindableObject.Asset != AssetCategory.FXOption.ToString() && bindableObject.Asset != AssetCategory.ConvertibleBond.ToString())
                    bindableObject.Delta = 1;

                bindableObject.YesterdayUnderlyingMarkPrice = _yesterdayUnderlyingMarkPrice;
                bindableObject.YesterdayUnderlyingMarkPriceStr = _yesterdayUnderlyingMarkPriceStr;
                bindableObject.PercentageUnderlyingChange = PercentageUnderlyingChange;
                bindableObject.TransactionType = _transactionType;
                bindableObject.YesterdayMarketValue = _yesterdayMarketValue;
                bindableObject.YesterdayMarketValueInBaseCurrency = _yesterdayMarketValueInBaseCurrency;
                bindableObject.YesterdayFXRate = _yesterdayFXRate;
                bindableObject.ChangeInUnderlyingPrice = ChangeInUnderlyingPrice;
                bindableObject.ReutersSymbol = _reutersSymbol;
                bindableObject.BetaAdjGrossExposure = _betaAdjGrossExposure;
                bindableObject.PositionSideExposureBoxed = _positionSideExposureBoxed.ToString();
                bindableObject.Analyst = _analyst;
                bindableObject.CountryOfRisk = _countryOfRisk;
                bindableObject.CustomUDA1 = _customUDA1;
                bindableObject.CustomUDA2 = _customUDA2;
                bindableObject.CustomUDA3 = _customUDA3;
                bindableObject.CustomUDA4 = _customUDA4;
                bindableObject.CustomUDA5 = _customUDA5;
                bindableObject.CustomUDA6 = _customUDA6;
                bindableObject.CustomUDA7 = _customUDA7;
                bindableObject.Issuer = _issuer;
                bindableObject.LiquidTag = _liquidTag;
                bindableObject.MarketCap = _marketCap;
                bindableObject.Region = _region;
                bindableObject.RiskCurrency = _riskCurrency;
                bindableObject.UcitsEligibleTag = _ucitsEligibleTag;
                bindableObject.FxRateDisplay = _fxRateDisplay;
                bindableObject.NetNotionalForCostBasisBreakEven = _netNotionalForCostBasisBreakEven;
                bindableObject.PositionSideMV = _positionSideMV.ToString();
                bindableObject.PositionSideExposure = _positionSideExposure.ToString();
                bindableObject.NavTouch = this._navTouch;
                bindableObject.DayTradedPosition = _dayTradedPosition;
                bindableObject.TradeVolume = _tradeVolume;
                bindableObject.CustomUDA8 = _customUDA8;
                bindableObject.CustomUDA9 = _customUDA9;
                bindableObject.CustomUDA10 = _customUDA10;
                bindableObject.CustomUDA11 = _customUDA11;
                bindableObject.CustomUDA12 = _customUDA12;

                bindableObject.TradeAttribute7 = TradeAttribute7;
                bindableObject.TradeAttribute8 = TradeAttribute8;
                bindableObject.TradeAttribute9 = TradeAttribute9;
                bindableObject.TradeAttribute10 = TradeAttribute10;
                bindableObject.TradeAttribute11 = TradeAttribute11;
                bindableObject.TradeAttribute12 = TradeAttribute12;
                bindableObject.TradeAttribute13 = TradeAttribute13;
                bindableObject.TradeAttribute14 = TradeAttribute14;
                bindableObject.TradeAttribute15 = TradeAttribute15;
                bindableObject.TradeAttribute16 = TradeAttribute16;
                bindableObject.TradeAttribute17 = TradeAttribute17;
                bindableObject.TradeAttribute18 = TradeAttribute18;
                bindableObject.TradeAttribute19 = TradeAttribute19;
                bindableObject.TradeAttribute20 = TradeAttribute20;
                bindableObject.TradeAttribute21 = TradeAttribute21;
                bindableObject.TradeAttribute22 = TradeAttribute22;
                bindableObject.TradeAttribute23 = TradeAttribute23;
                bindableObject.TradeAttribute24 = TradeAttribute24;
                bindableObject.TradeAttribute25 = TradeAttribute25;
                bindableObject.TradeAttribute26 = TradeAttribute26;
                bindableObject.TradeAttribute27 = TradeAttribute27;
                bindableObject.TradeAttribute28 = TradeAttribute28;
                bindableObject.TradeAttribute29 = TradeAttribute29;
                bindableObject.TradeAttribute30 = TradeAttribute30;
                bindableObject.TradeAttribute31 = TradeAttribute31;
                bindableObject.TradeAttribute32 = TradeAttribute32;
                bindableObject.TradeAttribute33 = TradeAttribute33;
                bindableObject.TradeAttribute34 = TradeAttribute34;
                bindableObject.TradeAttribute35 = TradeAttribute35;
                bindableObject.TradeAttribute36 = TradeAttribute36;
                bindableObject.TradeAttribute37 = TradeAttribute37;
                bindableObject.TradeAttribute38 = TradeAttribute38;
                bindableObject.TradeAttribute39 = TradeAttribute39;
                bindableObject.TradeAttribute40 = TradeAttribute40;
                bindableObject.TradeAttribute41 = TradeAttribute41;
                bindableObject.TradeAttribute42 = TradeAttribute42;
                bindableObject.TradeAttribute43 = TradeAttribute43;
                bindableObject.TradeAttribute44 = TradeAttribute44;
                bindableObject.TradeAttribute45 = TradeAttribute45;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion GetBindableObject

        #region Virtual Methods To Set Base Values (To Be overridden in FX order class)
        public virtual void SetYesterdayMarketValueInBaseCurrency()
        {
            // Trades are in the traded currency, so to convert in the base currency - divided it.
            if (YesterdayFXRate != 0.0)
            {
                _yesterdayMarketValueInBaseCurrency = YesterdayMarketValue * YesterdayFXRate;
            }
        }

        public virtual void SetDayPnLInBaseCurrency()
        {
            if (_fxRate != 0.0)
            {
                _earnedDividendBase = _earnedDividendLocal * _fxRate;
                _dayPnLInBaseCurrency = _dayPnL * _fxRate;
            }
        }

        public virtual void SetNetExposureInBaseCurrency()
        {
            if (_fxRate != 0.0)
            {
                _netExposureInBaseCurrency = _netExposure * _fxRate;
            }
        }

        public virtual void SetCostBasisUnrealizedPnlInBaseCurrency()
        {
            if (_fxRate != 0.0)
            {
                _costBasisUnrealizedPnlInBaseCurrency = _costBasisUnrealizedPnL * _fxRate;
            }
        }

        public virtual void SetMarketValueInBaseCurrency()
        {
            if (_fxRate != 0.0)
            {
                _marketValueInBaseCurrency = _marketValue * _fxRate;
            }
        }

        public virtual void SetCashImpactInBaseCurrency()
        {
            _cashImpactInBaseCurrency = -1 * _netNotionalInBaseCurrency;
        }

        public virtual void SetNetNotionalInCompanyBaseCurrency()
        {
            // Trades are in the traded currency, so to convert in the base currency - divided it.
            //Set the pnl in the company base currency as well.
            if (_fxRateOnTradeDate != 0.0)
            {
                switch (_fxConversionMethodOnTradeDate)
                {
                    case Operator.M:
                        _netNotionalInBaseCurrency = _netNotionalValue * _fxRateOnTradeDate;
                        break;
                    case Operator.D:
                        _netNotionalInBaseCurrency = _netNotionalValue / _fxRateOnTradeDate;
                        break;
                }
            }
            else
            {
                _netNotionalInBaseCurrency = 0.0;
            }
        }

        /// <summary>
        /// Copy Sec master details to epnlOrder on updating SecMaster
        /// </summary>
        /// <param name="secMasterObject"></param>
        /// <param name="isUnderlyingData"></param>
        public virtual void CopyBasicDetails(SecMasterBaseObj secMasterObject, bool isUnderlyingData)
        {
            if (!isUnderlyingData)
            {
                _multiplier = secMasterObject.Multiplier;
                _bloombergSymbol = secMasterObject.BloombergSymbol;
                _bloombergSymbolWithExchangeCode = secMasterObject.BloombergSymbolWithExchangeCode;
                _cusipSymbol = secMasterObject.CusipSymbol;
                _fullSecurityName = secMasterObject.LongName;
                _isinSymbol = secMasterObject.ISINSymbol;
                _sedolSymbol = secMasterObject.SedolSymbol;
                _proxySymbol = secMasterObject.ProxySymbol;
                _symbol = secMasterObject.TickerSymbol.ToUpper();
                _reutersSymbol = secMasterObject.ReutersSymbol;
                if (!_isLiveFeedSharesOutstanding || _sharesOutstanding <= 0)
                    _sharesOutstanding = secMasterObject.SharesOutstanding;
                // Kuldeep A.: Applied this check as now we are populating the 'leveraged factor' field for options from their base class.
                // So no need to publish that.
                if (secMasterObject.AssetCategory != AssetCategory.EquityOption && secMasterObject.AssetCategory != AssetCategory.FutureOption && secMasterObject.AssetCategory != AssetCategory.FXOption)
                    _leveragedFactor = secMasterObject.Delta;
            }
            else
            {
                // Kuldeep A.: un-commented below code as so that the Leveraged Factor can be published for options as well by changing Leveraged Factor of their base classes.
                _leveragedFactor = secMasterObject.Delta;
            }

            #region UDA DATA related fields

            //merged UDA class to UDA symbolData - om, nov 2013
            UDAData udaSymbolData = secMasterObject.SymbolUDAData;

            if (udaSymbolData != null)
            {
                if (!isUnderlyingData)
                {
                    _UDAAsset = udaSymbolData.UDAAsset;
                }
                _UDACountry = udaSymbolData.UDACountry;
                _UDASector = udaSymbolData.UDASector;
                _UDASubSector = udaSymbolData.UDASubSector;
                _UDASecurityType = udaSymbolData.UDASecurityType;
            }
            #endregion

            #region Dynamic-UDA

            SerializableDictionary<String, Object> dynamicUDA = secMasterObject.DynamicUDA;
            if (dynamicUDA != null)
            {
                if (dynamicUDA.ContainsKey("Analyst") && dynamicUDA["Analyst"] != null)
                    _analyst = dynamicUDA["Analyst"].ToString();

                if (dynamicUDA.ContainsKey("CountryOfRisk") && dynamicUDA["CountryOfRisk"] != null)
                    _countryOfRisk = dynamicUDA["CountryOfRisk"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA1") && dynamicUDA["CustomUDA1"] != null)
                    _customUDA1 = dynamicUDA["CustomUDA1"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA2") && dynamicUDA["CustomUDA2"] != null)
                    _customUDA2 = dynamicUDA["CustomUDA2"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA3") && dynamicUDA["CustomUDA3"] != null)
                    _customUDA3 = dynamicUDA["CustomUDA3"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA4") && dynamicUDA["CustomUDA4"] != null)
                    _customUDA4 = dynamicUDA["CustomUDA4"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA5") && dynamicUDA["CustomUDA5"] != null)
                    _customUDA5 = dynamicUDA["CustomUDA5"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA6") && dynamicUDA["CustomUDA6"] != null)
                    _customUDA6 = dynamicUDA["CustomUDA6"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA7") && dynamicUDA["CustomUDA7"] != null)
                    _customUDA7 = dynamicUDA["CustomUDA7"].ToString();

                if (dynamicUDA.ContainsKey("Issuer") && dynamicUDA["Issuer"] != null)
                    _issuer = dynamicUDA["Issuer"].ToString();

                if (dynamicUDA.ContainsKey("LiquidTag") && dynamicUDA["LiquidTag"] != null)
                    _liquidTag = dynamicUDA["LiquidTag"].ToString();

                if (dynamicUDA.ContainsKey("MarketCap") && dynamicUDA["MarketCap"] != null)
                    _marketCap = dynamicUDA["MarketCap"].ToString();

                if (dynamicUDA.ContainsKey("Region") && dynamicUDA["Region"] != null)
                    _region = dynamicUDA["Region"].ToString();

                if (dynamicUDA.ContainsKey("RiskCurrency") && dynamicUDA["RiskCurrency"] != null)
                    _riskCurrency = dynamicUDA["RiskCurrency"].ToString();

                if (dynamicUDA.ContainsKey("UCITSEligibleTag") && dynamicUDA["UCITSEligibleTag"] != null)
                    _ucitsEligibleTag = dynamicUDA["UCITSEligibleTag"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA8") && dynamicUDA["CustomUDA8"] != null)
                    _customUDA8 = dynamicUDA["CustomUDA8"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA9") && dynamicUDA["CustomUDA9"] != null)
                    _customUDA9 = dynamicUDA["CustomUDA9"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA10") && dynamicUDA["CustomUDA10"] != null)
                    _customUDA10 = dynamicUDA["CustomUDA10"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA11") && dynamicUDA["CustomUDA11"] != null)
                    _customUDA11 = dynamicUDA["CustomUDA11"].ToString();

                if (dynamicUDA.ContainsKey("CustomUDA12") && dynamicUDA["CustomUDA12"] != null)
                    _customUDA12 = dynamicUDA["CustomUDA12"].ToString();
            }
            #endregion
        }
        #endregion

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
        /// Gets a single trade attribute value by name.
        /// </summary>
        public virtual string GetTradeAttributeValue(string attributeName)
        {
            try
            {
                return _attributeGetters.TryGetValue(attributeName, out var getter) ? getter(this) : null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets trade attribute values from a dictionary.
        /// </summary>
        public virtual void SetTradeAttribute(Dictionary<string, string> attributeValues)
        {
            try
            {
                foreach (var attributePair in attributeValues)
                {
                    if (_attributeSetters.TryGetValue(attributePair.Key, out var setter))
                    {
                        setter(this, attributePair.Value ?? string.Empty);
                    }
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
