using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Order.
    /// </summary>
    [Serializable]
    [KnownType(typeof(Order))]
    public class Order : PranaBasicMessage
    {
        #region Constructors
        public Order()
        {
            _clientOrderID = IDGenerator.GenerateClientOrderID();
            _parentClientOrderID = _clientOrderID;
        }

        public Order(bool zeroSet)
        {
            if (zeroSet)
            {
                _quantity = 0;
                _price = 0.0;
                _stopPrice = 0.0;
                _avgPrice = 0.0f;
                _replacableQty = 0;
                _cumQty = 0;
                _stopPrice = 0.0;
                _leavesQty = 0;
                _lastPrice = 0.0;
                _lastShares = 0;
                _clientOrderID = IDGenerator.GenerateClientOrderID();
                _parentClientOrderID = _clientOrderID;
            }
        }

        public Order(OrderSingle or)
        {
            this._assetID = or.AssetID;
            this._assetName = or.AssetName;
            this._auecID = or.AUECID;
            this._avgPrice = or.AvgPrice;
            this._basketSeqNumber = or.BasketSequenceNumber;
            this._borrowerID = or.BorrowerID;
            this._borrowerBroker = or.BorrowerBroker;
            this._clientOrderID = or.ClientOrderID;
            this._clientTime = or.ClientTime;
            this._clOrderID = or.ClOrderID;
            this._cmta = or.CMTA;
            this._cmtaid = or.CMTAID;
            _userID = or.CompanyUserID;
            this._counterPartyID = or.CounterPartyID;
            this._counterPartyName = or.CounterPartyName;
            this._cumQty = or.CumQty;
            this._currencyID = or.CurrencyID;
            this._currencyName = or.CurrencyName;
            this._discInst = or.DiscretionInst;
            this._discOffset = or.DiscretionOffset;
            this._display = or.DisplayQuantity;
            this._exchangeID = or.ExchangeID;
            this._exchangeName = or.ExchangeName;
            this._execID = or.ExecID;
            this._execType = or.ExecType;
            this._executionInstruction = or.ExecutionInstruction;
            this._accountID = or.Level1ID;
            this._giveUp = or.GiveUp;
            this._giveUpID = or.GiveUpID;
            this._handlingInstruction = or.HandlingInstruction;
            this._isInternalOrder = or.IsInternalOrder;
            this._isOverbuyOversellAccepted = or.IsOverbuyOversellAccepted;
            this._lastMarket = or.LastMarket;
            this._lastPrice = or.LastPrice;
            this._lastShares = or.LastShares;
            this._leavesQty = or.LeavesQty;
            this._listID = or.ListID;
            this._locateReqd = or.LocateReqd;
            this._maturityMonthYear = or.MaturityMonthYear;
            this._maturityDay = or.MaturityDay;
            this._msgSeqNum = or.MsgSeqNum;
            this._msgType = or.MsgType;
            this._PranaMsgType = or.PranaMsgType;
            this._openClose = or.OpenClose;
            this._orderID = or.OrderID;
            this._orderSeqNumber = or.OrderSeqNumber;
            this._orderSide = or.OrderSide;
            this._orderSideTagValue = or.OrderSideTagValue;
            this._orderStatus = or.OrderStatus;
            this._orderStatusTagValue = or.OrderStatusTagValue;
            this._orderType = or.OrderType;
            this._orderTypeTagValue = or.OrderTypeTagValue;
            this._origClOrderID = or.OrigClOrderID;
            this._parentClientOrderID = or.ParentClientOrderID;
            this._parentClOrderID = or.ParentClOrderID;
            this._pegDiff = or.PegDifference;
            this._pnp = or.PNP;
            this._price = or.Price;
            this._putOrCall = or.PutOrCalls;
            this._quantity = or.Quantity;
            this._securityType = or.SecurityType;
            this._senderCompID = or.SenderCompID;
            this._senderSubID = or.SenderSubID;
            this._sendingTime = or.SendingTime;
            this._shortRebate = or.ShortRebate;
            this._stagedOrderID = or.StagedOrderID;
            this._stopPrice = or.StopPrice;
            this._strategyID = or.Level2ID;
            this._strikePrice = or.StrikePrice;
            this._symbol = or.Symbol;
            this._targetCompID = or.TargetCompID;
            this._targetSubID = or.TargetSubID;
            this._text = or.Text;
            this._tif = or.TIF;
            this._tradingAccountID = or.TradingAccountID;
            this._tradingAccountName = or.TradingAccountName;
            this._transactionTime = or.TransactionTime;
            this._underlyingID = or.UnderlyingID;
            this._underlyingName = or.UnderlyingName;
            this._unsentQty = or.UnsentQty;
            this._venue = or.Venue;
            this._venueID = or.VenueID;
            this._waveID = or.WaveID;
            this._underLyingsymbol = or.UnderlyingSymbol;
            this._algoStrategyID = or.AlgoStrategyID;
            this._algoStrategyName = or.AlgoStrategyName;
            this._algoProps = or.AlgoProperties.Clone();
            this._aUECLocalDate = or.AUECLocalDate;
            this._processDate = or.ProcessDate;
            this._originalPurchaseDate = or.OriginalPurchaseDate;
            _settlementDate = or.SettlementDate;
            this._algoSyntheticRPLParent = or.AlgoSyntheticRPLParent;
            if (or.SwapParameters != null)
            {
                this._swapParameters = or.SwapParameters.Clone();
            }
            if (or.OTCParameters != null)
            {
                this._otcParameters = or.OTCParameters;
            }
            if (or.ShortLocateParameter != null)
            {
                this._shortLocateParameter = or.ShortLocateParameter;
            }
            this.FXRate = or.FXRate;
            this.FXConversionMethodOperator = or.FXConversionMethodOperator;
            this.Delta = or.Delta;
            this._notionalValue = or.NotionalValue;
            this._notionalValueBase = or.NotionalValueBase;
            this._importFileID = or.ImportFileID;
            this._importFileName = or.ImportFileName;
            this._multiTradeName = or.MultiTradeName;
            this.MultiTradeId = or.MultiTradeId;
            this._internalComments = or.InternalComments;
            this.OriginalAllocationPreferenceID = or.OriginalAllocationPreferenceID;
            this.ExecutionTimeLastFill = or.ExecutionTimeLastFill;
        }
        #endregion

        #region Private Variable
        private string _currentUser = string.Empty;
        private string _modifiedUser = string.Empty;
        private Int64 _orderSeqNumber = Int64.MinValue;
        private string _msgType = string.Empty;
        private string _discInst = string.Empty;
        private double _price = double.Epsilon;
        private double _stopPrice = double.Epsilon;
        private double _lastShares = double.Epsilon;
        private double _lastPrice = double.Epsilon;
        private double _leavesQty = double.Epsilon;
        private double _workingQty = double.Epsilon;
        private double _discOffset = double.Epsilon;
        private double _pegDiff = double.Epsilon;
        private double _display = double.Epsilon;
        private string _handlingInstruction = string.Empty;
        private string _targetSubID = string.Empty;
        private string _targetCompID = string.Empty;
        private string _executionInstruction = string.Empty;
        private string _tif = string.Empty;
        private string _expireTime = string.Empty;
        private double _dayOrderQty = 0.0;
        private double _dayCumQty = 0.0;
        private double _dayAvgPx = 0.0;
        private string _pnp = string.Empty;
        private string _origClOrderID = string.Empty;
        private string _orderID = string.Empty;
        private string _clOrderID = string.Empty;
        private string _stagedOrderID = string.Empty;
        private string _listID = string.Empty;
        //private string _clientTime = DateTime.Now.ToUniversalTime().GetDateTimeFormats()[105].Remove(4, 1).Remove(6, 1).Replace('T', '-'); //"20050816-09:14:16");			// DateTimeConstants.MinValue.ToString();
        private string _clientTime = DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss");
        private string _orderStatus = string.Empty;
        private string _orderStatusTagValue = string.Empty;
        private string _parentClOrderID = string.Empty;
        private string _parentClientOrderID = string.Empty;
        private string _possDupFlag = "N";
        private string _senderSubID = string.Empty;
        private string _senderCompID = string.Empty;
        private string _sendingTime = string.Empty;
        private Int64 _msgSeqNum = Int64.MinValue;
        private string _execTransType = string.Empty;
        private string _execID = string.Empty;
        private string _execType = string.Empty;
        private string _lastMarket = string.Empty;
        private string _text = string.Empty;
        private DateTime _transactionTime = DateTime.UtcNow;
        private string _internalID = string.Empty;
        private byte[] _flag = null;
        private Int64 _replacableQty = -1;
        private Int32 _PranaMsgType = 0;
        private int _strategyID = int.MinValue;
        private int _accountID = int.MinValue;
        private string _strategy = string.Empty;
        private string _account = string.Empty;
        private bool _locateReqd = false;
        private int _basketSeqNumber = int.MinValue;
        private string _waveID = string.Empty;
        private string _groupID = string.Empty;
        private string _securityType = string.Empty;
        private string _maturityMonthYear = string.Empty;
        private string _maturityDay = string.Empty;
        new private string _putOrCall = string.Empty; // 0 = Put, 1 = Call
        new private double _strikePrice = double.Epsilon;
        private string _onBehalfOfCompID = string.Empty;
        private string _openClose = string.Empty;
        private TradingStatus _orderTradingStatus = TradingStatus.NotTraded;
        private PreTradingStatus _orderPreTradingStatus = PreTradingStatus.NotValidated;
        private bool _isInternalOrder = true;
        private bool _isOverbuyOversellAccepted = false;
        private bool _isTIAccepted = false;
        private double _percentageQty = 0.0;
        private double _bidPrice = 0.0;
        private double _askPrice = 0.0;
        private int _cmtaid = int.MinValue;
        private string _cmta = string.Empty;
        private int _giveUpID = int.MinValue;
        private string _giveUp = string.Empty;
        private string _algoStrategyID = string.Empty;
        private string _algoStrategyName = "N.A";
        private string _clientOrderID = string.Empty;
        private double _sendQty = 0.0;
        private double _unsentQty = 0.0;
        private double _subOrderQty = 0.0;
        private Order _parent = null;
        [NonSerialized]
        OrderCollection _subOrders = new OrderCollection();
        private int _originatorType;
        private string _listedExchangeIdentifier = string.Empty;
        string _handlingInstructionText;
        string _tifText;
        string _executionInstructionText;
        private string _algoSyntheticRPLParent = string.Empty;
        private double _notionalValue;
        private double _notionalValueBase;
        private int _importFileID;
        private string _importFileName;
        private string _multiTradeName;
        private string _multiTradeId;
        private bool _isMultiDayParentOrder = false;
        #endregion

        #region Public Properties
        public string CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        public string ModifiedUser
        {
            get { return _modifiedUser; }
            set { _modifiedUser = value; }
        }
        public OrderCollection SubOrders
        {
            get { return _subOrders; }
            set { _subOrders = value; }
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

        public string OnBehalfOfCompID
        {
            get { return _onBehalfOfCompID; }
            set { _onBehalfOfCompID = value; }
        }

        public string OpenClose
        {
            get { return _openClose; }
            set { _openClose = value; }
        }

        public Int64 OrderSeqNumber
        {
            get { return _orderSeqNumber; }
            set { _orderSeqNumber = value; }
        }

        // using new keyword as the field has a initial value in the derived class
        new public string PutOrCall
        {
            get { return _putOrCall; }
            set
            {
                if (value == "0")
                {
                    _putOrCall = OptionType.PUT.ToString();
                    base.PutOrCall = 0;
                }
                else if (value == "1")
                {
                    _putOrCall = OptionType.CALL.ToString();
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
                    _putOrCall = value;
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

        public string MaturityDay
        {
            get { return _maturityDay; }
            set { _maturityDay = value; }
        }

        // adding new keyword as derived class has different initial value than base value (0 and double.Epsilon)
        new public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        public int Level1ID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        public int Level2ID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        public string Level1Name
        {
            get { return _account; }
            set { _account = value; }
        }

        public string Level2Name
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        public bool LocateReqd
        {
            get { return _locateReqd; }
            set { _locateReqd = value; }
        }

        public Int32 PranaMsgType
        {
            get { return _PranaMsgType; }
            set { _PranaMsgType = value; }
        }

        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        // adding new as base defination has an attribute [XmlIgnore]
        new public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        public double DisplayQuantity
        {
            get { return _display; }
            set { _display = value; }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public double BidPrice
        {
            get { return _bidPrice; }
            set { _bidPrice = value; }
        }

        public double AskPrice
        {
            get { return _askPrice; }
            set { _askPrice = value; }
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

        public string HandlingInstructionText
        {
            get { return _handlingInstructionText; }
            set { _handlingInstructionText = value; }
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

        public string ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }

        public string ExecutionInstructionText
        {
            get { return _executionInstructionText; }
            set { _executionInstructionText = value; }
        }

        public string TIFText
        {
            get { return _tifText; }
            set { _tifText = value; }
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

        public string ExecTransType
        {
            get { return _execTransType; }
            set { _execTransType = value; }
        }

        public string ExecID
        {
            get { return _execID; }
            set { _execID = value; }
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

        /// <summary>
        /// For Basket Wave
        /// </summary>
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        public int BasketSequenceNumber
        {
            get { return _basketSeqNumber; }
            set { _basketSeqNumber = value; }
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

        public double LeavesQty
        {
            get { return _leavesQty; }
            set { _leavesQty = value; }
        }

        public double WorkingQty
        {
            get { return _workingQty; }
            set { _workingQty = value; }
        }

        public string ExecType
        {
            get { return _execType; }
            set { _execType = value; }
        }

        public string LastMarket
        {
            get { return _lastMarket; }
            set { _lastMarket = value; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
            }
        }

        public DateTime TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }

        public string InternalID
        {
            get { return _internalID; }
            set { _internalID = value; }
        }

        //Modified By Sandeep
        public string CMTA
        {
            get { return _cmta; }
            set { _cmta = value; }
        }

        public int CMTAID
        {
            get { return _cmtaid; }
            set { _cmtaid = value; }
        }

        public int GiveUpID
        {
            get { return _giveUpID; }
            set { _giveUpID = value; }
        }

        public string GiveUp
        {
            get { return _giveUp; }
            set { _giveUp = value; }
        }

        /// <summary>
        /// Not Yet Used :: We are sending ExecInst = 6 for PNP order (For ARCA only)
        /// </summary>
        public string PNP
        {
            get { return _pnp; }
            set { _pnp = value; }
        }

        public string ClientTime
        {
            get { return _clientTime; }
            set { _clientTime = value; }
        }

        public string OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }

        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }

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

        /// <summary>
        /// To be used for all single orders to TakE down the CLOrderID of the parent order. Do not Confuse with Staged OrderID
        /// </summary>
        public string ParentClOrderID
        {
            get { return _parentClOrderID; }
            set { _parentClOrderID = value; }
        }

        public byte[] Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public Int64 ReplacableQty
        {
            set { _replacableQty = value; }
            get { return _replacableQty; }
        }

        public string ClientOrderID
        {
            get { return _clientOrderID; }
            set { _clientOrderID = value; }
        }

        public double SendQty
        {
            set { _sendQty = value; }
            get { return _sendQty; }
        }

        public double SubOrderQty
        {
            get
            {
                _subOrderQty = 0;
                foreach (Order order in _subOrders)
                {
                    _subOrderQty += order.Quantity;
                }
                return _subOrderQty;
            }
        }

        public double RemainingQty
        {
            get { return _quantity - _cumQty; }
        }

        public double UnsentQty
        {
            set { _unsentQty = value; }
            get { return _unsentQty; }
        }

        public string ParentClientOrderID
        {
            get { return _parentClientOrderID; }
            set { _parentClientOrderID = value; }
        }

        public string PossDupFlag
        {
            get { return _possDupFlag; }
            set { _possDupFlag = value; }
        }

        public TradingStatus OrderTradingStatus
        {
            get { return _orderTradingStatus; }
            set { _orderTradingStatus = value; }
        }

        public PreTradingStatus OrderPreTradingStatus
        {
            get { return _orderPreTradingStatus; }
            set { _orderPreTradingStatus = value; }
        }

        public Order Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public bool IsAccepted
        {
            get { return _isTIAccepted; }
            set { _isTIAccepted = value; }
        }

        public double PercentageQty
        {
            get { return _percentageQty; }
            set { _percentageQty = value; }
        }

        public string AlgoStrategyID
        {
            get { return _algoStrategyID; }
            set { _algoStrategyID = value; }
        }

        public string AlgoStrategyName
        {
            get { return _algoStrategyName; }
            set { _algoStrategyName = value; }
        }

        private OrderAlgoStartegyParameters _algoProps = new OrderAlgoStartegyParameters();
        public OrderAlgoStartegyParameters AlgoProperties
        {
            get { return _algoProps; }
            set { _algoProps = value; }
        }

        [Browsable(false)]
        private SwapParameters _swapParameters;
        public SwapParameters SwapParameters
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }

        [Browsable(false)]
        private OTCTradeData _otcParameters;
        public OTCTradeData OTCParameters
        {
            get { return _otcParameters; }
            set { _otcParameters = value; }
        }

        [Browsable(false)]
        private ShortLocateListParameter _shortLocateParameter;
        public ShortLocateListParameter ShortLocateParameter
        {
            get { return _shortLocateParameter; }
            set { _shortLocateParameter = value; }
        }

        public int OriginatorType
        {
            get { return _originatorType; }
            set { _originatorType = value; }
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

        public double NotionalValue
        {
            get
            {
                if (string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Closed) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Open) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Cover))
                {
                    _notionalValue = (AvgPrice * CumQty * ContractMultiplier);
                    return _notionalValue;
                }
                else

                    _notionalValue = (AvgPrice * CumQty * ContractMultiplier * -1);
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
                double fxrateTemp = 0;
                if (FXRate != 0)
                {
                    fxrateTemp = FXConversionMethodOperator.Equals("M") ? FXRate : (1 / FXRate);
                }

                _notionalValueBase = _notionalValue * fxrateTemp;
                return _notionalValueBase;
            }
            set
            {
                _notionalValueBase = value;
            }
        }

        public int ImportFileID
        {
            get { return _importFileID; }
            set { _importFileID = value; }
        }

        public string ImportFileName
        {
            get { return _importFileName; }
            set { _importFileName = value; }
        }

        public string MultiTradeName
        {
            get { return _multiTradeName; }
            set { _multiTradeName = value; }
        }

        public bool IsMultiDayParentOrder
        {
            get { return _isMultiDayParentOrder; }
            set { _isMultiDayParentOrder = value; }
        }

        public string MultiTradeId
        {
            get { return _multiTradeId; }
            set { _multiTradeId = value; }
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
        #endregion

        public bool CreateSubOrder(Order order)
        {

            if (CanCreateSubOrders(order)) //if left Qty allows to cteate a SubOrder
            {
                order.Parent = this;
                _subOrders.Add(order);
                _subOrderQty = 0;
                foreach (Order subOrder in _subOrders)
                {
                    _subOrderQty += subOrder.Quantity;
                }
                return true;
            }
            else
                return false;
        }

        public bool CanCreateSubOrders()
        {
            if (AvailableQtyForSubOrders > 0 && _unsentQty > 0
                && _orderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled
                && _orderStatusTagValue != FIXConstants.ORDSTATUS_RollOver
                && _orderStatusTagValue != FIXConstants.ORDSTATUS_Rejected
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double AvailableQtyForSubOrders
        {
            get
            {
                double subOrderqty = 0.0;
                foreach (Order subOrder in _subOrders)
                {
                    if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled ||
                        subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver ||
                        subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                    {
                        subOrderqty += subOrder.Quantity - subOrder.UnsentQty;
                    }
                    else if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace)
                    {
                        subOrderqty += subOrder.Quantity + subOrder.LeavesQty;
                    }
                    else
                    {
                        subOrderqty += subOrder.Quantity;
                    }
                }
                return _quantity - subOrderqty;
            }
        }

        public bool CanCreateSubOrders(Order subOrder)
        {

            if (AvailableQtyForSubOrders >= subOrder.Quantity && _unsentQty > 0
                  && _orderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled
                  && _orderStatusTagValue != FIXConstants.ORDSTATUS_RollOver
                && _orderStatusTagValue != FIXConstants.ORDSTATUS_Rejected
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Order GetSubOrder(string clientOrderID)
        {
            Order subOrder = _subOrders.GetOrder(clientOrderID);
            return subOrder;
        }

        public void AddFill(OrderFill order)
        {
            try
            {
                _leavesQty = order.LeavesQty;

                if (order.MsgType == FIXConstants.MSGOrderCancelReject)
                {
                    _clOrderID = order.OrigClOrderID;
                    _orderTypeTagValue = order.OrderTypeTagValue;
                    _sendQty = 0;
                }

                if (order.AlgoStrategyID != string.Empty)
                {
                    _algoProps = order.AlgoProperties;
                    _algoStrategyID = order.AlgoStrategyID;
                }

                _price = order.Price;
                _cumQty = order.CumQty;
                _avgPrice = order.AvgPrice;
                _orderStatusTagValue = order.OrderStatusTagValue;
                _orderStatus = order.OrderStatus;
                _lastShares = order.LastShares;
                _quantity = order.Quantity;
                _text = order.Text;

                if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_New ||
                    order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced ||
                    order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                {
                    _sendQty = 0;
                    _orderID = order.OrderID;
                }

                _unsentQty = _quantity - (_sendQty + _cumQty + _leavesQty);
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

        public void SetDefaultValues()
        {
            _orderStatusTagValue = string.Empty;
            _orderStatus = string.Empty;
            _cumQty = 0;
            _avgPrice = 0;
            _lastShares = 0;
            _lastMarket = string.Empty;
            _lastPrice = 0.0;
            _leavesQty = 0;
        }

        /// <summary>
        /// converts all epsilon values to 0
        /// </summary>
        public void SetForUI()
        {
            if (_price == double.Epsilon)
            {
                _price = 0;
            }

            if (_leavesQty == double.Epsilon)
            {
                _leavesQty = _quantity - _cumQty;
            }

            if (_lastShares == double.Epsilon)
            {
                _lastShares = _cumQty;
            }
        }
    }

    /// <summary>
    /// For Basket Orders Trading Status
    /// </summary>
    public enum TradingStatus
    {
        Valid,
        InValid,
        NotTraded,
        SentToServer,
        AckReceived
    }

    /// <summary>
    /// For Basket Orders PreTrading Status
    /// </summary>
    public enum PreTradingStatus
    {
        NotValidated,
        Valid,
        InValid
    }
}