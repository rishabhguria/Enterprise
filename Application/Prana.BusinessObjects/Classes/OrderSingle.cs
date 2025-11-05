using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OrderSingle : PranaBasicMessage, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructor for a new OrderSingle, all properties initialized in this default constructor only
        /// </summary>
        public OrderSingle()
        {
            _orderSeqNumber = Int64.MinValue;
            _msgType = string.Empty;
            _discInst = string.Empty;
            _quantity = 0.0;
            _avgPrice = double.Epsilon;
            _price = double.Epsilon;
            _stopPrice = double.Epsilon;
            _discOffset = double.Epsilon;
            _pegDiff = double.Epsilon;
            _display = double.Epsilon;
            _handlingInstruction = string.Empty;
            _targetSubID = string.Empty;
            _targetCompID = string.Empty;
            _executionInstruction = string.Empty;
            _tif = string.Empty;
            _expireTime = string.Empty;
            _dayOrderQty = 0.0;
            _dayCumQty = 0.0;
            _dayAvgPx = 0.0;
            _pnp = string.Empty;
            _origClOrderID = string.Empty;
            _orderID = string.Empty;
            _clOrderID = string.Empty;
            _stagedOrderID = string.Empty;
            _listID = string.Empty;
            _clientTime = DateTime.Now.ToString();
            _orderStatus = string.Empty;
            _orderStatusTagValue = string.Empty;
            _parentClOrderID = string.Empty;
            _senderSubID = string.Empty;
            _senderCompID = string.Empty;
            _sendingTime = string.Empty;
            _msgSeqNum = Int64.MinValue;
            _level2ID = int.MinValue;
            _level1ID = int.MinValue;
            _locateReqd = false;
            _shortRebate = double.Epsilon;
            _borrowerID = string.Empty;
            _borrowerBroker = string.Empty;
            _cmta = string.Empty;
            _cmtaid = int.MinValue;
            _basketSeqNumber = int.MinValue;
            _waveID = string.Empty;
            _underLyingsymbol = string.Empty; // By Sandeep 24-04-07
            _algoStrategyID = string.Empty;
            _lastShares = double.MinValue;
            _lastPrice = double.MinValue;
            _leavesQty = 0.0;
            _lastMarket = string.Empty;
            _openClose = string.Empty;
            _execType = string.Empty;
            _transactionTime = DateTime.UtcNow;
            _PranaMsgType = 0;
            _text = string.Empty;
            _securityType = string.Empty;
            _maturityMonthYear = string.Empty;
            _maturityDay = string.Empty;
            _putOrCalls = string.Empty; // 0 = Put, 1 = Call
            _strikePrice = double.Epsilon;
            _isInternalOrder = true;
            _isOverbuyOversellAccepted = false;
            _parentClientOrderID = string.Empty;
            _clientOrderID = string.Empty;
            _execID = string.Empty;
            _unsentQty = 0.0;
            _flag = null;
            _giveUp = string.Empty;
            _giveUpID = int.MinValue;
            _algoSyntheticRPLParent = string.Empty;
            _algoStrategyName = "N.A.";
            _listedExchangeIdentifier = string.Empty;
            _algoProps = new OrderAlgoStartegyParameters();
            _commissionAmt = 0.0;
            _commissionRate = 0.0;
            _calcBasis = CalculationBasis.Auto;
            _softCommissionAmt = 0.0;
            _softCommissionRate = 0.0;
            _softCommissionCalcBasis = CalculationBasis.Auto;
            _importFileName = string.Empty;
            _importFileID = 0;
            _multiTradeName = string.Empty;
            _multiTradeId = string.Empty;
            _validationStatus = ApplicationConstants.ValidationStatus.None.ToString();
            _validationError = string.Empty;
            _rowIndex = int.MinValue;
            _importStatus = Prana.BusinessObjects.AppConstants.ImportStatus.None.ToString();
            _tradeAttribute1 = string.Empty;
            _tradeAttribute2 = string.Empty;
            _tradeAttribute3 = string.Empty;
            _tradeAttribute4 = string.Empty;
            _tradeAttribute5 = string.Empty;
            _tradeAttribute6 = string.Empty;
            _internalComments = string.Empty;
            base.ResetTradeAttributes();
            _originalAllocationPreferenceID = 0;
            _rebalancerFileName = string.Empty;
            _account = _masterFund = _strategy = _allocationStatus = _allocationSchemeName = OrderFields.PROPERTY_DASH;
            _pmtype = PMType.None;
            _executionTimeLastFill = DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss");
            _averagePriceForCompliance = double.Epsilon;
            _isSamsaraUser = false;
        }

        /// <summary>
        /// makes OrderSingle from Order, calling this.default constructor to initialize fields
        /// </summary>
        /// <param name="or"></param>
        public OrderSingle(Order or)
            : this()
        {
            _assetID = or.AssetID;
            _assetName = or.AssetName;
            _auecID = or.AUECID;
            _avgPrice = or.AvgPrice;
            _basketSeqNumber = or.BasketSequenceNumber;
            _borrowerID = or.BorrowerID;
            _borrowerBroker = or.BorrowerBroker;
            _clientOrderID = or.ClientOrderID;
            _clientTime = or.ClientTime;
            _clOrderID = or.ClOrderID;
            _cmta = or.CMTA;
            _cmtaid = or.CMTAID;
            _userID = or.CompanyUserID;
            _actualUserId = or.ActualCompanyUserID;
            _counterPartyID = or.CounterPartyID;
            _counterPartyName = or.CounterPartyName;
            _cumQty = or.CumQty;
            _currencyID = or.CurrencyID;
            _currencyName = or.CurrencyName;
            _discInst = or.DiscretionInst;
            _discOffset = or.DiscretionOffset;
            _display = or.DisplayQuantity;
            _exchangeID = or.ExchangeID;
            _exchangeName = or.ExchangeName;
            _execID = or.ExecID;
            _execType = or.ExecType;
            _executionInstruction = or.ExecutionInstruction;
            _expirationDate = or.ExpirationDate;
            _level1ID = or.Level1ID;
            _giveUp = or.GiveUp; // added for futures
            _giveUpID = or.GiveUpID;

            _handlingInstruction = or.HandlingInstruction;
            _isInternalOrder = or.IsInternalOrder;
            _isOverbuyOversellAccepted = or.IsOverbuyOversellAccepted;
            _lastMarket = or.LastMarket;
            _lastPrice = or.LastPrice;
            _lastShares = or.LastShares;
            _leavesQty = or.LeavesQty;
            _listID = or.ListID;
            _locateReqd = or.LocateReqd;
            _maturityMonthYear = or.MaturityMonthYear;
            _maturityDay = or.MaturityDay;
            _msgSeqNum = or.MsgSeqNum;
            _msgType = or.MsgType;
            _PranaMsgType = or.PranaMsgType;
            _openClose = or.OpenClose;
            _orderID = or.OrderID;
            _orderSeqNumber = or.OrderSeqNumber;
            _orderSide = or.OrderSide;
            _orderSideTagValue = or.OrderSideTagValue;
            _orderStatus = or.OrderStatus;
            _orderStatusTagValue = or.OrderStatusTagValue;
            _orderType = or.OrderType;
            _orderTypeTagValue = or.OrderTypeTagValue;
            _origClOrderID = or.OrigClOrderID;
            _parentClientOrderID = or.ParentClientOrderID;
            _parentClOrderID = or.ParentClOrderID;
            _pegDiff = or.PegDifference;
            _pnp = or.PNP;
            _price = or.Price;
            _putOrCalls = or.PutOrCall;
            _quantity = or.Quantity;
            _securityType = or.SecurityType;
            _senderCompID = or.SenderCompID;
            _senderSubID = or.SenderSubID;
            _sendingTime = or.SendingTime;
            _settlementDate = or.SettlementDate;
            _shortRebate = or.ShortRebate;
            _stagedOrderID = or.StagedOrderID;
            _stopPrice = or.StopPrice;
            _level2ID = or.Level2ID;
            _strikePrice = or.StrikePrice;
            _symbol = or.Symbol;
            _targetCompID = or.TargetCompID;
            _targetSubID = or.TargetSubID;
            _text = or.Text;
            _tif = or.TIF;
            _expireTime = or.ExpireTime;
            _dayOrderQty = or.DayOrderQty;
            _dayCumQty = or.DayCumQty;
            _dayAvgPx = or.DayAvgPx;
            _tradingAccountID = or.TradingAccountID;
            _tradingAccountName = or.TradingAccountName;
            _transactionTime = or.TransactionTime;
            _underlyingID = or.UnderlyingID;
            _underlyingName = or.UnderlyingName;
            _unsentQty = or.UnsentQty;
            _venue = or.Venue;
            _venueID = or.VenueID;
            _waveID = or.WaveID;
            _underLyingsymbol = or.UnderlyingSymbol;
            _algoStrategyID = or.AlgoStrategyID;
            _algoProps = or.AlgoProperties.Clone();
            _aUECLocalDate = or.AUECLocalDate;
            _processDate = or.ProcessDate;
            _originalPurchaseDate = or.OriginalPurchaseDate;
            _algoSyntheticRPLParent = or.AlgoSyntheticRPLParent;
            _swapParameters = or.SwapParameters.Clone();
            _otcParameters = or.OTCParameters;
            _avgFXRateForTrade = or.FXRate;
            _FXConversionMethodOperator = or.FXConversionMethodOperator;
            _companyName = or.CompanyName;
            _notionalValue = or.NotionalValue;
            _commissionAmt = or.CommissionAmt;
            _commissionRate = or.CommissionRate;
            _calcBasis = or.CalcBasis;
            _softCommissionAmt = or.SoftCommissionAmt;
            _softCommissionRate = or.SoftCommissionRate;
            _softCommissionCalcBasis = or.SoftCommissionCalcBasis;
            _tradeAttribute1 = or.TradeAttribute1;
            _tradeAttribute2 = or.TradeAttribute2;
            _tradeAttribute3 = or.TradeAttribute3;
            _tradeAttribute4 = or.TradeAttribute4;
            _tradeAttribute5 = or.TradeAttribute5;
            _tradeAttribute6 = or.TradeAttribute6;
            SetTradeAttribute(or.GetTradeAttributesAsDict());
            _internalComments = or.InternalComments;
            _isCurrencyFuture = or.IsCurrencyFuture;
            _originalAllocationPreferenceID = or.OriginalAllocationPreferenceID;
            _shortLocateParameter = or.ShortLocateParameter;
            _executionTimeLastFill = or.ExecutionTimeLastFill;
            _modifiedUserId = or.ModifiedUserId;
            _averagePriceForCompliance = or.AvgPriceForCompliance;
            _isSamsaraUser = or.IsSamsaraUser;
            _tradeApplicationSource = or.TradeApplicationSource;
        }

        /// <summary>
        /// Makes OrderSingle from TradeParameters, calling this.default constructor to initialize values
        /// </summary>
        /// <param name="tradeParams"></param>
        public OrderSingle(TradeParametersArgs tradeParams)
            : this()
        {
            AssetID = (int)tradeParams.AssetId;
            AUECID = tradeParams.AUECId;
            CurrencyID = tradeParams.CurrencyId;
            Level1ID = tradeParams.AccountID;
            ExchangeID = tradeParams.ListedExchangeId;
            OrderSideTagValue = tradeParams.OrderSide;
            Price = tradeParams.Price;
            Quantity = tradeParams.Quantity;
            Level2ID = tradeParams.StrategyID;
            Symbol = tradeParams.Symbol;
            UnderlyingID = (int)tradeParams.UnderlyingId;
            UnderlyingSymbol = tradeParams.UnderlyingSymbol;
            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)tradeParams.AssetId);

            if (baseAssetCategory == Prana.BusinessObjects.AppConstants.AssetCategory.Option)
            {
                MaturityMonthYear = tradeParams.ExpirationMonth.ToString();
                MaturityDay = tradeParams.ExpirationDay.ToString();
                StrikePrice = tradeParams.StrikePrice;
                PutOrCalls = tradeParams.UnderlyingPutOrCall;
                SecurityType = FIXConstants.SECURITYTYPE_Options;
            }
            if (baseAssetCategory == Prana.BusinessObjects.AppConstants.AssetCategory.Future)
            {
                MaturityMonthYear = tradeParams.ExpirationMonth.ToString();
                MaturityDay = tradeParams.ExpirationDay.ToString();
                StrikePrice = tradeParams.StrikePrice;
                PutOrCalls = tradeParams.UnderlyingPutOrCall;
                SecurityType = FIXConstants.SECURITYTYPE_Futures;
            }

        }
        #endregion

        #region private Variable
        private Int64 _orderSeqNumber;
        private string _msgType;
        private string _discInst;
        // adding new as base and derived has different initial value (0 and double.epsilon)
        private double _price;
        private double _stopPrice;
        private double _discOffset;
        private double _pegDiff;
        private double _display;
        private string _handlingInstruction;
        private string _targetSubID;
        private string _targetCompID;
        private string _executionInstruction;
        private string _tif;
        private string _expireTime;
        private double _dayOrderQty;
        private double _dayCumQty;
        private double _dayAvgPx;
        private string _pnp;
        private string _origClOrderID;
        private string _orderID;
        private string _clOrderID;
        private string _stagedOrderID;
        private string _listID;
        private string _clientTime;
        private string _orderStatus;
        private string _orderStatusWithoutRollover;
        private string _orderStatusTagValue;
        private string _parentClOrderID;
        private string _senderSubID;
        private string _senderCompID;
        private string _sendingTime;
        private Int64 _msgSeqNum;
        private int _level2ID;
        private int _level1ID;
        private bool _locateReqd;
        private string _cmta;
        private int _cmtaid;
        private int _basketSeqNumber;
        private string _waveID;

        private string _algoStrategyID;
        private double _lastShares;
        private double _lastPrice;
        private double _leavesQty;
        private string _lastMarket;
        private string _openClose;
        private string _execType;
        private DateTime _transactionTime;
        private Int32 _PranaMsgType;
        private string _text;
        private string _securityType;
        private string _maturityMonthYear;
        private string _maturityDay;
        // adding new as base and derived has different initial value 
        private string _putOrCalls; // 0 = Put, 1 = Call
        new private double _strikePrice;
        private bool _isInternalOrder;
        private bool _isOverbuyOversellAccepted;
        private string _parentClientOrderID;
        private string _clientOrderID;
        private string _execID;
        private double _unsentQty;
        private byte[] _flag;
        private int _originatorType;
        private string _giveUp;
        private int _giveUpID;

        private string _algoSyntheticRPLParent;
        private string _algoStrategyName;
        private SwapParameters _swapParameters;
        private OTCTradeData _otcParameters;
        private string _listedExchangeIdentifier;
        private OrderAlgoStartegyParameters _algoProps;
        new private string _companyName;
        private double _notionalValue;
        private string _importFileName;
        private int _importFileID;
        private string _multiTradeName;
        private string _multiTradeId;
        new private string _bloombergSymbol;
        new private string _bloombergSymbolWithExchangeCode;
        new private string _factSetSymbol;
        new private string _activSymbol;
        new private string _sedolSymbol;
        private string _validationStatus;
        private string _validationError;
        private int _rowIndex;
        private string _importStatus;
        private string _allocationStatus;
        private string _masterFund;
        private string _account;
        private string _strategy;
        private string _allocationSchemeName;
        private Guid _orderSingleGuid;
        private string _counterCurrencyName;
        private string _rebalancerFileName;
        private ShortLocateListParameter _shortLocateParameter;
        private PMType _pmtype;
        #endregion

        #region Public Properties

        public virtual PMType PMType
        {
            get { return _pmtype; }
            set { _pmtype = value; }
        }

        public Guid OrderSingleGuid
        {
            get { return _orderSingleGuid; }
            set { _orderSingleGuid = value; }
        }

        public Int64 OrderSeqNumber
        {
            get { return _orderSeqNumber; }
            set { _orderSeqNumber = value; }
        }
        public int Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }
        public int Level2ID
        {
            get { return _level2ID; }
            set { _level2ID = value; }
        }
        public bool LocateReqd
        {
            get { return _locateReqd; }
            set { _locateReqd = value; }
        }
        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public double StopPrice
        {
            get { return _stopPrice; }
            set { _stopPrice = value; }
        }
        public string HandlingInstruction
        {
            get { return _handlingInstruction; }
            set { _handlingInstruction = value; }
        }
        public string TargetSubID
        {
            get { return _targetSubID; }
            set { _targetSubID = value; }
        }
        public string OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }
        public string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }
        public string TIF
        {
            get { return _tif; }
            set { _tif = value; }
        }

        public string ExpireTime
        {
            get { return _expireTime; }
            set { _expireTime = value; }
        }

        public string ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }
        public string SenderSubID
        {
            get { return _senderSubID; }
            set { _senderSubID = value; }
        }
        public string SenderCompID
        {
            get { return _senderCompID; }
            set { _senderCompID = value; }
        }
        public string TargetCompID
        {
            get { return _targetCompID; }
            set { _targetCompID = value; }
        }
        public string SendingTime
        {
            get { return _sendingTime; }
            set { _sendingTime = value; }
        }
        public Int64 MsgSeqNum
        {
            get { return _msgSeqNum; }
            set { _msgSeqNum = value; }
        }
        public string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }
        public string WaveID
        {
            get { return _waveID; }
            set { _waveID = value; }
        }
        public int BasketSequenceNumber
        {
            get { return _basketSeqNumber; }
            set { _basketSeqNumber = value; }

        }
        /// <summary>
        /// Not Yet Used :: We are sending ExecInst = 6 for PNP order (For ARCA only)
        /// </summary>
        public string PNP
        {
            get { return _pnp; }
            set { _pnp = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientTime
        {
            get { return _clientTime; }
            set { _clientTime = value; }
        }
        public string OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                if ((value.Equals("RollOver") || value.Equals("PendingRollOver")) && !_orderStatus.Equals("RollOver") && !_orderStatus.Equals("PendingRollOver"))
                {
                    _orderStatusWithoutRollover = _orderStatus;
                    if (!string.IsNullOrEmpty(TIF) && TIF.Equals(FIXConstants.TIF_GTD) && value.Equals("RollOver"))
                    {
                        _orderStatusWithoutRollover = "Expired";
                    }
                }
                _orderStatus = value;
            }
        }
        public string OrderStatusWithoutRollover
        {
            get
            {
                if (_orderStatus.Equals("Cancelled") && !string.IsNullOrEmpty(_text) && _text.Equals(Prana.BusinessObjects.Compliance.Constants.PreTradeConstants.MsgTradeReject))
                {
                    return _text;
                }
                else if ((_orderStatus.Equals("New") || _orderStatus.Equals("PendingNew") || _orderStatus.Equals("PendingReplace")) && !string.IsNullOrEmpty(_text) && _text.Equals(Prana.BusinessObjects.Compliance.Constants.PreTradeConstants.MsgTradePending))
                {
                    return _text;
                }
                else if (!_orderStatus.Equals("RollOver") && !_orderStatus.Equals("PendingRollOver"))
                {
                    return _orderStatus;
                }
                else
                {
                    return _orderStatusWithoutRollover;
                }
            }
        }
        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }
        /// <summary>
        /// Tag 41
        /// </summary>
        public string OrigClOrderID
        {
            get { return _origClOrderID; }
            set { _origClOrderID = value; }
        }
        /// <summary>
        /// Captures the ClOrderID of the parent if this order is a sub Order 
        /// </summary>
        public string StagedOrderID
        {
            get { return _stagedOrderID; }
            set { _stagedOrderID = value; }
        }
        /// <summary>
        /// Tag 388
        /// </summary>
        public string DiscretionInst
        {
            get { return _discInst; }
            set { _discInst = value; }
        }
        /// <summary>
        /// Tag 389
        /// </summary>
        public double DiscretionOffset
        {
            get { return _discOffset; }
            set { _discOffset = value; }
        }
        /// <summary>
        /// Tag 211
        /// </summary>
        public double PegDifference
        {
            get { return _pegDiff; }
            set { _pegDiff = value; }
        }
        public string ParentClOrderID
        {
            get { return _parentClOrderID; }
            set { _parentClOrderID = value; }
        }
        public int CMTAID
        {
            get { return _cmtaid; }
            set { _cmtaid = value; }
        }
        public string CMTA
        {
            get { return _cmta; }
            set { _cmta = value; }
        }
        public string GiveUp
        {
            get { return _giveUp; }
            set { _giveUp = value; }
        }
        public int GiveUpID
        {
            get { return _giveUpID; }
            set { _giveUpID = value; }
        }
        public int OriginatorType
        {
            get { return _originatorType; }
            set { _originatorType = value; }
        }
        public string AlgoStrategyID
        {
            get { return _algoStrategyID; }
            set { _algoStrategyID = value; }
        }
        public double DisplayQuantity
        {
            get { return _display; }
            set { _display = value; }
        }
        public DateTime TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }
        public Int32 PranaMsgType
        {
            get { return _PranaMsgType; }
            set { _PranaMsgType = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public string PutOrCalls
        {
            get { return _putOrCalls; }
            set
            {
                if (value == "0")
                {
                    _putOrCalls = OptionType.PUT.ToString();
                    base.PutOrCall = 0;
                }
                else if (value == "1")
                {
                    _putOrCalls = OptionType.CALL.ToString();
                    base.PutOrCall = 1;
                }
                else
                {
                    if (value.Equals("CALL"))
                    {
                        base.PutOrCall = 1;
                    }
                    else if (value.Equals("PUT"))
                    {
                        base.PutOrCall = 0;
                    }
                    _putOrCalls = value;
                }
            }
        }
        public string SecurityType
        {
            get { return _securityType; }
            set { _securityType = value; }
        }
        public string MaturityMonthYear
        {
            get { return _maturityMonthYear; }
            set { _maturityMonthYear = value; }
        }

        /// <summary>
        /// Gets or sets the name of the allocation scheme.
        /// </summary>
        /// <value>
        /// The name of the allocation scheme.
        /// </value>
        public string AllocationSchemeName
        {
            get { return _allocationSchemeName; }
            set { _allocationSchemeName = value; }
        }

        public string MaturityDay
        {
            get { return _maturityDay; }
            set { _maturityDay = value; }
        }

        // adding new as field has an initial value in base class
        new public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }
        /// <summary>
        /// Fill recieved in last execution
        /// </summary>
        public double LastShares
        {
            get { return _lastShares; }
            set { _lastShares = value; }
        }
        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }
        public string OpenClose
        {
            get { return _openClose; }
            set { _openClose = value; }
        }
        public string ExecType
        {
            get { return _execType; }
            set { _execType = value; }
        }
        public string ExecID
        {
            get { return _execID; }
            set { _execID = value; }
        }
        public string LastMarket
        {
            get { return _lastMarket; }
            set { _lastMarket = value; }
        }
        public double LeavesQty
        {
            get { return _leavesQty; }
            set { _leavesQty = value; }
        }
        public double UnsentQty
        {
            get { return _unsentQty; }
            set { _unsentQty = value; }
        }
        public string ClientOrderID
        {
            get { return _clientOrderID; }
            set { _clientOrderID = value; }
        }
        public string ParentClientOrderID
        {
            get { return _parentClientOrderID; }
            set { _parentClientOrderID = value; }
        }
        /// <summary>
        /// If false means this is a sell side order.
        /// Default is true which means the order is generated from internal clients.
        /// </summary>
        public bool IsInternalOrder
        {
            get { return _isInternalOrder; }
            set { _isInternalOrder = value; }
        }
        public bool IsOverbuyOversellAccepted
        {
            get { return _isOverbuyOversellAccepted; }
            set { _isOverbuyOversellAccepted = value; }
        }
        public byte[] Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }


        public string AlgoStrategyName
        {
            get { return _algoStrategyName; }
            set { _algoStrategyName = value; }
        }
        public string ListedExchangeIdentifier
        {
            get { return _listedExchangeIdentifier; }
            set { _listedExchangeIdentifier = value; }
        }


        public string AlgoSyntheticRPLParent
        {
            get { return _algoSyntheticRPLParent; }
            set { _algoSyntheticRPLParent = value; }
        }

        // adding new keyword as field is initialised in derived
        new public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        new public string BloombergSymbolWithExchangeCode
        {
            get { return _bloombergSymbolWithExchangeCode; }
            set { _bloombergSymbolWithExchangeCode = value; }
        }

        new public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        new public string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }

        new public string SEDOLSymbol
        {
            get { return _sedolSymbol; }
            set { _sedolSymbol = value; }
        }

        [Browsable(false)]
        public SwapParameters SwapParameters
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }

        [Browsable(false)]
        public OTCTradeData OTCParameters
        {
            get { return _otcParameters; }
            set { _otcParameters = value; }
        }

        [Browsable(false)]
        public ShortLocateListParameter ShortLocateParameter
        {
            get { return _shortLocateParameter; }
            set { _shortLocateParameter = value; }
        }

        new public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public double NotionalValue
        {
            get
            {
                if (CumQty != 0)
                {
                    if (string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Closed) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Open) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Cover))
                    {
                        _notionalValue = (AvgPrice * CumQty * ContractMultiplier);
                        return _notionalValue;
                    }
                    else
                    {
                        _notionalValue = (AvgPrice * CumQty * ContractMultiplier * -1);
                        return _notionalValue;
                    }
                }
                else
                    return _notionalValue;
            }
            set
            {
                _notionalValue = value;
            }
        }
        public double NotionalValueBase
        {
            get
            {
                double fxRateTemp = 0.0;
                if (FXRate != 0)
                    fxRateTemp = FXConversionMethodOperator.Equals("M") ? FXRate : 1 / FXRate;

                return (_notionalValue * fxRateTemp);
            }
        }
        public string ImportFileName
        {
            get { return _importFileName; }
            set { _importFileName = value; }
        }
        public int ImportFileID
        {
            get { return _importFileID; }
            set { _importFileID = value; }
        }

        public string MultiTradeName
        {
            get { return _multiTradeName; }
            set { _multiTradeName = value; }
        }

        public string MultiTradeId
        {
            get { return _multiTradeId; }
            set { _multiTradeId = value; }
        }

        public string ValidationStatus
        {
            get { return _validationStatus; }
            set { _validationStatus = value; }
        }

        public string ValidationError
        {
            get { return _validationError; }
            set { _validationError = value; }
        }

        [Browsable(false)]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }


        public string ImportStatus
        {
            get { return _importStatus; }
            set { _importStatus = value; }
        }

        public string AllocationStatus
        {
            get { return _allocationStatus; }
            set { _allocationStatus = value; }
        }

        public string MasterFund
        {
            get { return _masterFund; }
            set { _masterFund = value; }
        }

        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        public double PercentExecuted
        {
            get { return (CumQty / Quantity) * 100; }
        }

        public double PercentCompleted
        {
            get { return double.IsNaN(CumQty / (CumQty + LeavesQty) * 100) ? 0.0 : CumQty / (CumQty + LeavesQty) * 100; }
        }

        public double DayOrderQty
        {
            get { return _dayOrderQty; }
            set { _dayOrderQty = value; }
        }
        public double DayCumQty
        {
            get { return _dayCumQty; }
            set { _dayCumQty = value; }
        }

        public double DayAvgPx
        {
            get { return _dayAvgPx; }
            set { _dayAvgPx = value; }
        }


        public double UnexecutedQuantity
        {
            get { return (Quantity - CumQty); }
        }

        /// <summary>
        /// Gets the counter currency identifier.
        /// </summary>
        /// <value>
        /// The counter currency identifier.
        /// </value>
        public string CounterCurrency
        {
            get { return _counterCurrencyName; }
            set { _counterCurrencyName = value; }
        }

        /// <summary>
        /// Gets the counter currency amount.
        /// </summary>
        /// <value>
        /// The counter currency amount.
        /// </value>
        public double CounterCurrencyAmount
        {
            get
            {
                if (AssetID == (int)AssetCategory.FX || AssetID == (int)AssetCategory.FXForward)
                {
                    double position = _quantity;
                    if (position == 0)
                        return 0;
                    if (OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                        position = position * -1;
                    if (position != 0 && AvgPrice != 0)
                    {
                        if (LeadCurrencyID == SettlementCurrencyID)
                            return Math.Round(position * AvgPrice, 4);
                        else
                            return Math.Round(position / AvgPrice, 4);
                    }
                }
                return 0.0;
            }
        }
        #endregion

        OrderBindingList _orderCollection;

        /// <summary>
        /// it returns default null value. Check for null before using. Instantiate by new OrderBindingList
        /// </summary>
        [Browsable(false)]
        public OrderBindingList OrderCollection
        {
            get
            {
                return _orderCollection;
            }
            set
            {
                _orderCollection = value;
            }
        }

        public OrderBindingList BindableCollection
        {
            get
            {
                return _orderCollection;
            }
        }


        #region update staged orders from child fills
        /// <summary>
        /// Used for staged orders parent update from Child updates
        /// </summary>
        public void UpdateWorkingQtyFromChildCollection()
        {
            double leavesQty = 0;
            double pendingCancelOrdersleavesQty = 0;
            double pendingNewOrdersleavesQty = 0;

            foreach (OrderSingle subOrder in OrderCollection)
            {
                if (subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected)
                {
                    if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                        pendingNewOrdersleavesQty += subOrder.LeavesQty;

                    else if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                        pendingCancelOrdersleavesQty += subOrder.LeavesQty;

                    else
                        leavesQty += subOrder.LeavesQty;
                }
            }

            if (pendingNewOrdersleavesQty > pendingCancelOrdersleavesQty)
                _leavesQty = leavesQty + pendingNewOrdersleavesQty;

            else
                _leavesQty = leavesQty + pendingCancelOrdersleavesQty;
        }
        #endregion

        public OrderAlgoStartegyParameters AlgoProperties
        {
            get { return _algoProps; }
            set { _algoProps = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Rebalancer File.
        /// </summary>
        /// <value>
        /// The Rebalancer File name.
        /// </value>
        public string RebalancerFileName
        {
            get { return _rebalancerFileName; }
            set { _rebalancerFileName = value; }
        }

        /// <summary>
        /// Gets and sets Trade application source for trade
        /// </summary>
        protected int _tradeApplicationSource;
        [Browsable(false)]
        public virtual int TradeApplicationSource
        {
            get { return _tradeApplicationSource; }
            set { _tradeApplicationSource = value; }
        }

        #region INotifyPropertyChanged Members
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, null);
            }
        }

        #endregion

        public void CopyBasicDetails(OrderSingle basicMsg)
        {
            base.CopyBasicDetails(basicMsg);
            _bloombergSymbol = basicMsg.BloombergSymbol;
            _bloombergSymbolWithExchangeCode = basicMsg.BloombergSymbolWithExchangeCode;
            _factSetSymbol = basicMsg.FactSetSymbol;
            _activSymbol = basicMsg.ActivSymbol;
        }
    }
}
