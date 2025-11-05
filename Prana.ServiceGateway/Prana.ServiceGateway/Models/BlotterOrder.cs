using System.ComponentModel;

namespace Prana.ServiceGateway.Models
{
    public class BlotterOrder
    {       
        private int _currencyID;
        public virtual int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private string internalComments;
        public string InternalComments
        {
            get { return internalComments; }
            set { internalComments = value; }
        }
        private double _unsentQty;
        public double UnsentQty
        {
            get { return _unsentQty; }
            set { _unsentQty = value; }
        }

        private int _auecid;
        public int AUECID
        {
            get { return _auecid; }
            set { _auecid = value; }
        }

        private int _underlyingID;
        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        private int _exchangeID;
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private int _assetID;
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private string _orderId;
        public string OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        private string _stagedOrderID;
        public string StagedOrderID
        {
            get { return _stagedOrderID; }
            set { _stagedOrderID = value; }
        }

        private string _parentClOrderID;
        public string ParentClOrderID
        {
            get { return _parentClOrderID; }
            set { _parentClOrderID = value; }
        }

        private string _clOrderID;
        public string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }

        private int _companyUserID;
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }

        private DateTime _transactionTime = DateTime.UtcNow;
        public DateTime TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _side = string.Empty;
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private string _orderSideTagValue = string.Empty;
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        private string _orderType = string.Empty;
        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        private string _orderTypeTagValue = string.Empty;
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        private string _statusWithoutRollover = string.Empty;
        public string StatusWithoutRollover
        {
            get { return _statusWithoutRollover; }
            set { _statusWithoutRollover = value; }
        }

        private string _status = string.Empty;
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _orderStatusTagValue = string.Empty;
        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }

        private double _quantity = 0;
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        protected int _brokerID = int.MinValue;
        public virtual int BrokerID
        {
            get { return _brokerID; }
            set { _brokerID = value; }
        }

        protected string _broker = string.Empty;
        public virtual string Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        private double _workingQuantity = double.Epsilon;
        public double WorkingQuantity
        {
            get { return _workingQuantity; }
            set { _workingQuantity = value; }
        }

        private double _executedQuantity = double.Epsilon;
        public double ExecutedQuantity
        {
            get { return _executedQuantity; }
            set { _executedQuantity = value; }
        }

        private double _avgFillPrice = 0;
        public double AvgFillPrice
        {
            get { return _avgFillPrice; }
            set { _avgFillPrice = value; }
        }

        private double _avgPriceBase = 0;
        public double AvgPriceBase
        {
            get { return _avgPriceBase; }
            set { _avgPriceBase = value; }
        }

        private int _accountID = int.MinValue;
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private string _account = String.Empty;
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private string _allocationSchemeName = String.Empty;
        public string AllocationSchemeName
        {
            get { return _allocationSchemeName; }
            set { _allocationSchemeName = value; }
        }

        private double _limit = 0;
        public double Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        private double _stop = 0;
        public double Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }

        private double _cumQty = 0;
        public double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }

        private double _percentExecuted;
        public double PercentExecuted
        {

            get { return _percentExecuted; }
            set { _percentExecuted = value; }
        }

        private int _venueID = int.MinValue;
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private string _venue = string.Empty;
        public string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        private string _algoStrategyID;
        public string AlgoStrategyID
        {
            get { return _algoStrategyID; }
            set { _algoStrategyID = value; }
        }

        private string _algo;
        public string Algo
        {
            get { return _algo; }
            set { _algo = value; }
        }

        private string _tif;
        public string TIF
        {
            get { return _tif; }
            set { _tif = value; }
        }

        private string _trader;
        public string Trader
        {
            get { return _trader; }
            set { _trader = value; }
        }

        private int _transactionSourceTag;
        public virtual int TransactionSourceTag
        {
            get { return _transactionSourceTag; }
            set { _transactionSourceTag = value; }
        }

        private int _pranaMsgType;
        public int PranaMsgType
        {
            get { return _pranaMsgType; }
            set { _pranaMsgType = value; }
        }

        private bool _isManualOrder = false;
        public bool IsManualOrder
        {
            get { return _isManualOrder; }
            set { _isManualOrder = value; }
        }

        private double _lastShares;
        public double LastShares
        {
            get { return _lastShares; }
            set { _lastShares = value; }
        }

        private double _lastPrice;
        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }

        private double _avgFXRateForTrade;
        public virtual double FXRate
        {
            get { return _avgFXRateForTrade; }
            set { _avgFXRateForTrade = value; }
        }

        private string _FXConversionMethodOperator;
        public virtual string FXConversionMethodOperator
        {
            get { return _FXConversionMethodOperator; }
            set { _FXConversionMethodOperator = value; }
        }

        private int _settlementCurrencyID;
        public virtual int SettlementCurrencyID
        {
            get { return _settlementCurrencyID; }
            set { _settlementCurrencyID = value; }
        }

        private double _notionalValue;
        public virtual double NotionalValue
        {
            get { return _notionalValue; }
            set { _notionalValue = value; }
        }

        private double _notionalValueBase;
        public virtual double NotionalValueBase
        {
            get { return _notionalValueBase; }
            set { _notionalValueBase = value; }
        }

        private Dictionary<string, string> _tagValueDictionary = new Dictionary<string, string>();
        public Dictionary<string, string> TagValueDictionary
        {
            get { return _tagValueDictionary; }
            set { _tagValueDictionary = value; }
        }

        List<BlotterOrder> _orderCollection;
        [Browsable(false)]
        public List<BlotterOrder> OrderCollection
        {
            get { return _orderCollection; }
            set { _orderCollection = value; }
        }

        private DateTime _aUECLocalDate = new DateTime(1800, 1, 1, 12, 0, 0);
        public virtual DateTime AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }
        }

        private string _origClOrderID = string.Empty;
        public string OrigClOrderID
        {
            get { return _origClOrderID; }
            set { _origClOrderID = value; }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _bloombergSymbol;
        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        private string _executionTimeLastFill;
        public string ExecutionTimeLastFill
        {
            get { return _executionTimeLastFill; }
            set { _executionTimeLastFill = value; }
        }
        private int _tradingAccountID = int.MinValue;
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        private string _handlingInstruction;
        public string HandlingInstruction
        {
            get { return _handlingInstruction; }
            set { _handlingInstruction = value; }
        }

        private string _executionInstruction;
        public string ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }

        private string _expiryDate;
        public string ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        private double _dayAveragePrice = 0;
        public double DayAveragePrice
        {
            get { return _dayAveragePrice; }
            set { _dayAveragePrice = value; }
        }

        private double _startOfDay;
        public double StartOfDay
        {
            get { return _startOfDay; }
            set { _startOfDay = value; }
        }

        private double _dayExecutedQuantity;
        public double DayExecutedQuantity
        {
            get { return _dayExecutedQuantity; }
            set { _dayExecutedQuantity = value; }
        }

        private double _originalAllocationPreferenceID;
        public double OriginalAllocationPreferenceID
        {
            get { return _originalAllocationPreferenceID; }
            set { _originalAllocationPreferenceID = value; }
        }

        private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        private string _actualUser;
        public string ActualUser
        {
            get { return _actualUser; }
            set { _actualUser = value; }
        }

        private SwapParameters _swapParameter;
        public SwapParameters SwapParameter
        {
            get { return _swapParameter; }
            set { _swapParameter = value; }
        }

        private string _assetClass;
        public string AssetClass
        {
            get { return _assetClass; }
            set { _assetClass = value; }
        }

        private string _tradeAttribute1;
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2;
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3;
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }

        private string _tradeAttribute4;
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5;
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6;
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        private string _tradeAttribute7;
        public string TradeAttribute7
        {
            get { return _tradeAttribute7; }
            set { _tradeAttribute7 = value; }
        }

        private string _tradeAttribute8;
        public string TradeAttribute8
        {
            get { return _tradeAttribute8; }
            set { _tradeAttribute8 = value; }
        }

        private string _tradeAttribute9;
        public string TradeAttribute9
        {
            get { return _tradeAttribute9; }
            set { _tradeAttribute9 = value; }
        }

        private string _tradeAttribute10;
        public string TradeAttribute10
        {
            get { return _tradeAttribute10; }
            set { _tradeAttribute10 = value; }
        }

        private string _tradeAttribute11;
        public string TradeAttribute11
        {
            get { return _tradeAttribute11; }
            set { _tradeAttribute11 = value; }
        }

        private string _tradeAttribute12;
        public string TradeAttribute12
        {
            get { return _tradeAttribute12; }
            set { _tradeAttribute12 = value; }
        }

        private string _tradeAttribute13;
        public string TradeAttribute13
        {
            get { return _tradeAttribute13; }
            set { _tradeAttribute13 = value; }
        }

        private string _tradeAttribute14;
        public string TradeAttribute14
        {
            get { return _tradeAttribute14; }
            set { _tradeAttribute14 = value; }
        }

        private string _tradeAttribute15;
        public string TradeAttribute15
        {
            get { return _tradeAttribute15; }
            set { _tradeAttribute15 = value; }
        }

        private string _tradeAttribute16;
        public string TradeAttribute16
        {
            get { return _tradeAttribute16; }
            set { _tradeAttribute16 = value; }
        }

        private string _tradeAttribute17;
        public string TradeAttribute17
        {
            get { return _tradeAttribute17; }
            set { _tradeAttribute17 = value; }
        }

        private string _tradeAttribute18;
        public string TradeAttribute18
        {
            get { return _tradeAttribute18; }
            set { _tradeAttribute18 = value; }
        }

        private string _tradeAttribute19;
        public string TradeAttribute19
        {
            get { return _tradeAttribute19; }
            set { _tradeAttribute19 = value; }
        }

        private string _tradeAttribute20;
        public string TradeAttribute20
        {
            get { return _tradeAttribute20; }
            set { _tradeAttribute20 = value; }
        }

        private string _tradeAttribute21;
        public string TradeAttribute21
        {
            get { return _tradeAttribute21; }
            set { _tradeAttribute21 = value; }
        }

        private string _tradeAttribute22;
        public string TradeAttribute22
        {
            get { return _tradeAttribute22; }
            set { _tradeAttribute22 = value; }
        }

        private string _tradeAttribute23;
        public string TradeAttribute23
        {
            get { return _tradeAttribute23; }
            set { _tradeAttribute23 = value; }
        }

        private string _tradeAttribute24;
        public string TradeAttribute24
        {
            get { return _tradeAttribute24; }
            set { _tradeAttribute24 = value; }
        }

        private string _tradeAttribute25;
        public string TradeAttribute25
        {
            get { return _tradeAttribute25; }
            set { _tradeAttribute25 = value; }
        }

        private string _tradeAttribute26;
        public string TradeAttribute26
        {
            get { return _tradeAttribute26; }
            set { _tradeAttribute26 = value; }
        }

        private string _tradeAttribute27;
        public string TradeAttribute27
        {
            get { return _tradeAttribute27; }
            set { _tradeAttribute27 = value; }
        }

        private string _tradeAttribute28;
        public string TradeAttribute28
        {
            get { return _tradeAttribute28; }
            set { _tradeAttribute28 = value; }
        }

        private string _tradeAttribute29;
        public string TradeAttribute29
        {
            get { return _tradeAttribute29; }
            set { _tradeAttribute29 = value; }
        }

        private string _tradeAttribute30;
        public string TradeAttribute30
        {
            get { return _tradeAttribute30; }
            set { _tradeAttribute30 = value; }
        }

        private string _tradeAttribute31;
        public string TradeAttribute31
        {
            get { return _tradeAttribute31; }
            set { _tradeAttribute31 = value; }
        }

        private string _tradeAttribute32;
        public string TradeAttribute32
        {
            get { return _tradeAttribute32; }
            set { _tradeAttribute32 = value; }
        }

        private string _tradeAttribute33;
        public string TradeAttribute33
        {
            get { return _tradeAttribute33; }
            set { _tradeAttribute33 = value; }
        }

        private string _tradeAttribute34;
        public string TradeAttribute34
        {
            get { return _tradeAttribute34; }
            set { _tradeAttribute34 = value; }
        }

        private string _tradeAttribute35;
        public string TradeAttribute35
        {
            get { return _tradeAttribute35; }
            set { _tradeAttribute35 = value; }
        }

        private string _tradeAttribute36;
        public string TradeAttribute36
        {
            get { return _tradeAttribute36; }
            set { _tradeAttribute36 = value; }
        }

        private string _tradeAttribute37;
        public string TradeAttribute37
        {
            get { return _tradeAttribute37; }
            set { _tradeAttribute37 = value; }
        }

        private string _tradeAttribute38;
        public string TradeAttribute38
        {
            get { return _tradeAttribute38; }
            set { _tradeAttribute38 = value; }
        }

        private string _tradeAttribute39;
        public string TradeAttribute39
        {
            get { return _tradeAttribute39; }
            set { _tradeAttribute39 = value; }
        }

        private string _tradeAttribute40;
        public string TradeAttribute40
        {
            get { return _tradeAttribute40; }
            set { _tradeAttribute40 = value; }
        }

        private string _tradeAttribute41;
        public string TradeAttribute41
        {
            get { return _tradeAttribute41; }
            set { _tradeAttribute41 = value; }
        }

        private string _tradeAttribute42;
        public string TradeAttribute42
        {
            get { return _tradeAttribute42; }
            set { _tradeAttribute42 = value; }
        }

        private string _tradeAttribute43;
        public string TradeAttribute43
        {
            get { return _tradeAttribute43; }
            set { _tradeAttribute43 = value; }
        }

        private string _tradeAttribute44;
        public string TradeAttribute44
        {
            get { return _tradeAttribute44; }
            set { _tradeAttribute44 = value; }
        }

        private string _tradeAttribute45;
        public string TradeAttribute45
        {
            get { return _tradeAttribute45; }
            set { _tradeAttribute45 = value; }
        }


        public string TransactionSource { get; set; }

        public string BorrowerBroker { get; set; }

        public double PegDifference { get; set; }

        private bool _isUseCustodianAsExecutingBroker;
        public bool IsUseCustodianBroker
        {
            get { return _isUseCustodianAsExecutingBroker; }
            set { _isUseCustodianAsExecutingBroker = value; }
        }

        public string Strategy { get; set; }

        public int Level2ID { get; set; }

        public bool IsMultiBrokerTrade { get; set; }

        private string _factSetSymbol;
        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }
    }
}
