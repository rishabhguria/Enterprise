using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.BusinessBaseClass;
using Prana.CommonDataCache;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;

//Please ensure to update the BlotterOrder class in the Service Gateway solution when new properties are added/updated in this class
namespace Prana.BlotterDataService.DTO
{
    public class BlotterOrder: AdditionalTradeAttributes
    {
        internal void UpdateDataFromOrderSingle(OrderSingle orderSingle, bool isOrderCollectionRequired = false, bool orderInPendingApprovalCache = false)
        {
            #region Allocation details updation for Pending New and Rejected orders
            //Forcefully updated value of Account, Master fund, Strategy and Allocation Status value to Dash (-) in case of Order status is Pending New or Rejected. 
            //Because in case of Pending new and Rejected case, These groups are not visible in Allocation.
            if (orderSingle.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew || orderSingle.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                orderSingle.AllocationSchemeName = orderSingle.AllocationStatus = orderSingle.Account = orderSingle.MasterFund = orderSingle.Strategy = OrderFields.PROPERTY_DASH;
            #endregion
            Account = orderSingle.Account;
            Algo = orderSingle.AlgoStrategyName;
            AvgFillPrice = orderSingle.AvgPrice;
            AvgPriceBase = orderSingle.AvgPriceBase;
            if (orderSingle.AllocationSchemeName.Contains(TradingTicketType.Manual.ToString()))
            {
                AccountID = CachedDataManager.GetInstance.GetAccountID(orderSingle.Account);
            }
            else
            {
                AccountID = orderSingle.Level1ID;
            }
            if (orderSingle.Price != double.Epsilon)
                Limit = orderSingle.Price;
            Stop = orderSingle.StopPrice;
            WorkingQuantity = orderSingle.LeavesQty;
            AllocationSchemeName = orderSingle.AllocationSchemeName;
            BrokerID = orderSingle.CounterPartyID;
            Broker = orderSingle.CounterPartyName;
            CumQty = orderSingle.CumQty;
            ExecutedQuantity = orderSingle.CumQty;
            if (string.IsNullOrEmpty(orderSingle.OrderSide) && !string.IsNullOrEmpty(orderSingle.OrderSideTagValue))
                orderSingle.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderSingle.OrderSideTagValue);
            Side = orderSingle.OrderSide;
            OrderSideTagValue = orderSingle.OrderSideTagValue;
            if (string.IsNullOrEmpty(orderSingle.OrderStatus) && !string.IsNullOrEmpty(orderSingle.OrderStatusTagValue))
                orderSingle.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderSingle.OrderStatusTagValue);
            // Commenting for the User Story 22165
            //if (orderSingle.OrderStatus.Equals(BlotterDataConstants.CONST_Cancelled) && orderSingle.Text.Equals(BlotterDataConstants.CONST_BlockedByCompliance))
            //    Status = orderSingle.Text;
            //else if ((orderSingle.OrderStatus.Equals(BlotterDataConstants.CONST_PendingNew) || orderInPendingApprovalCache) && orderSingle.OrderStatusWithoutRollover.Equals(PreTradeConstants.MsgTradePending))
            //    Status = orderSingle.OrderStatusWithoutRollover;
            //else
            //    Status = orderSingle.OrderStatus;
            Status = orderSingle.OrderStatus;
            StatusWithoutRollover = orderSingle.OrderStatusWithoutRollover;
            OrderStatusTagValue = orderSingle.OrderStatusTagValue;
            if (string.IsNullOrEmpty(orderSingle.OrderType) && !string.IsNullOrEmpty(orderSingle.OrderTypeTagValue))
                orderSingle.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(orderSingle.OrderTypeTagValue);
            OrderType = orderSingle.OrderType;
            OrderTypeTagValue = orderSingle.OrderTypeTagValue;
            AlgoStrategyID = orderSingle.AlgoStrategyID;
            TagValueDictionary = orderSingle.AlgoProperties.TagValueDictionary;
            Quantity = orderSingle.Quantity;
            Symbol = orderSingle.Symbol;
            VenueID = orderSingle.VenueID;
            Venue = orderSingle.Venue;
            TransactionTime = orderSingle.TransactionTime;
            TIF = TagDatabaseManager.GetInstance.GetTIFText(orderSingle.TIF);
            Trader = orderSingle.TradingAccountName;
            OrderId = orderSingle.OrderID;
            StagedOrderID = orderSingle.StagedOrderID;
            ParentClOrderID = orderSingle.ParentClOrderID;
            ClOrderID = orderSingle.ClOrderID;
            CompanyUserID = orderSingle.CompanyUserID;
            PercentExecuted = Math.Round(orderSingle.PercentExecuted, 2);
            List<BlotterOrder> blotterOrderList = new List<BlotterOrder>();
            if (isOrderCollectionRequired && orderSingle.OrderCollection != null)
            {
                foreach (OrderSingle order in orderSingle.OrderCollection)
                {
                    BlotterOrder item = new BlotterOrder();
                    item.UpdateDataFromOrderSingle(order);
                    blotterOrderList.Add(item);
                }
            }
            OrderCollection = blotterOrderList;
            AssetID = orderSingle.AssetID;
            TransactionSourceTag = orderSingle.TransactionSourceTag;
            UnderlyingID = orderSingle.UnderlyingID;
            ExchangeID = orderSingle.ExchangeID == int.MinValue ? CachedDataManager.GetInstance.GetExchangeIdFromAUECId(orderSingle.AUECID) : orderSingle.ExchangeID;
            PranaMsgType = orderSingle.PranaMsgType;
            IsManualOrder = orderSingle.IsManualOrder;
            LastShares = orderSingle.LastShares;
            LastPrice = orderSingle.LastPrice;
            FXRate = orderSingle.FXRate;
            FXConversionMethodOperator = orderSingle.FXConversionMethodOperator;
            SettlementCurrencyID = orderSingle.SettlementCurrencyID;
            NotionalValue = orderSingle.NotionalValue;
            NotionalValueBase = orderSingle.NotionalValueBase;
            AUECID = orderSingle.AUECID;
            UnsentQty = orderSingle.UnsentQty;
            InternalComments = orderSingle.InternalComments;
            AUECLocalDate = orderSingle.AUECLocalDate;
            CurrencyID = orderSingle.CurrencyID;
            OrigClOrderID = orderSingle.OrigClOrderID;
            Text = orderSingle.Text;
            BloombergSymbol = orderSingle.BloombergSymbol;
            ExecutionTimeLastFill = orderSingle.ExecutionTimeLastFill;
            TradingAccountID = orderSingle.TradingAccountID;
            HandlingInstruction = orderSingle.HandlingInstruction;
            ExecutionInstruction = orderSingle.ExecutionInstruction;
            if (string.IsNullOrEmpty(orderSingle.TradingAccountName) && orderSingle.TradingAccountID != int.MinValue)
                Trader = CachedDataManager.GetInstance.GetTradingAccountText(orderSingle.TradingAccountID);
            ShortLocateParameter = orderSingle.ShortLocateParameter;
            ExpiryDate = !string.IsNullOrWhiteSpace(orderSingle.ExpireTime) ? orderSingle.ExpireTime : ApplicationConstants.C_NotAvailable;
            DayAveragePrice = orderSingle.DayAvgPx;
            StartOfDay = orderSingle.DayOrderQty;
            DayExecutedQuantity = orderSingle.DayCumQty;
            OriginalAllocationPreferenceID = orderSingle.OriginalAllocationPreferenceID;
            ActualUser = orderSingle.ActualCompanyUserName;
            CurrentUser = orderSingle.CompanyUserName;
            SwapParameter = orderSingle.SwapParameters;
            AssetClass = orderSingle.AssetName;
            TransactionSource = ((TransactionSource)orderSingle.TransactionSourceTag).ToString();
            BorrowerBroker = orderSingle.BorrowerBroker;
            PegDifference = orderSingle.PegDifference;
            TradeAttribute1 = orderSingle.TradeAttribute1;
            TradeAttribute2 = orderSingle.TradeAttribute2;
            TradeAttribute3 = orderSingle.TradeAttribute3;
            TradeAttribute4 = orderSingle.TradeAttribute4;
            TradeAttribute5 = orderSingle.TradeAttribute5;
            TradeAttribute6 = orderSingle.TradeAttribute6;
            SetTradeAttribute(orderSingle.GetTradeAttributesAsDict());
            IsUseCustodianBroker = orderSingle.IsUseCustodianBroker;
            Strategy = orderSingle.Strategy;
            Level2ID = orderSingle.Level2ID;
            IsMultiBrokerTrade = orderSingle.IsMultiBrokerTrade;
            FactSetSymbol = orderSingle.FactSetSymbol;
        }

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

        private DateTime _aUECLocalDate = DateTimeConstants.MinValue;
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

        private ShortLocateListParameter _shortLocateParameter;
        public ShortLocateListParameter ShortLocateParameter
        {
            get { return _shortLocateParameter; }
            set { _shortLocateParameter = value; }
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

        public string TransactionSource { get; set; }
		public string BorrowerBroker { get; set; }
		public double PegDifference { get; set; }
        public string TradeAttribute1 { get; set; }
        public string TradeAttribute2 { get; set; }
        public string TradeAttribute3 { get; set; }
        public string TradeAttribute4 { get; set; }
        public string TradeAttribute5 { get; set; }
        public string TradeAttribute6 { get; set; }

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
