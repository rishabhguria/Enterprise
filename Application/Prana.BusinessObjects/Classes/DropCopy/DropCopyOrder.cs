namespace Prana.BusinessObjects
{
    public class DropCopyOrder
    {

        #region private Variable
        private string _ClOrderId = string.Empty;
        private string _orderSide = string.Empty;
        private string _orderType = string.Empty;
        private string _symbol = string.Empty;
        private string _symbolMap = string.Empty;
        private double _quantity = double.Epsilon;
        private double _limitPrice = double.Epsilon;
        private double _price = double.Epsilon;
        private string _tif = string.Empty;
        private string _brokerID = string.Empty;
        private string _statusText = string.Empty;
        private string _strategy = string.Empty;
        private string _orderTypeTagValue = string.Empty;
        private string _orderSideTagValue = string.Empty;
        private int _tradingAccountId = int.MinValue;
        private string _msgType = string.Empty;
        private int _companyUserID = int.MinValue;
        private int _counterPartyID = int.MinValue;
        private int _venueID = int.MinValue;
        private string _orderStatusTagValue = string.Empty;
        private string _orderStatus = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// If false means this is a sell side order.
        /// Default is true which means the order is generated from internal clients.
        /// </summary>

        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        public string ClientOrderID
        {
            get { return _ClOrderId; }
            set { _ClOrderId = value; }
        }

        public string Text
        {
            get { return _statusText; }
            set { _statusText = value; }
        }
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }
        public string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }
        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }
        public string SymbolMap
        {
            get { return _symbolMap; }
            set { _symbolMap = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string TIF
        {
            get { return _tif; }
            set { _tif = value; }
        }
        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }
        public double LimitPrice
        {
            get { return _limitPrice; }
            set { _limitPrice = value; }
        }
        public string Broker
        {
            get { return _brokerID; }
            set { _brokerID = value; }
        }
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public int TradingAccountID
        {
            get { return _tradingAccountId; }
            set { _tradingAccountId = value; }
        }
        double _cumQty = 0.0;
        public double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }
        double _avgPrice = 0.0;
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }
        private int _auecID = int.MinValue;

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        private int _assetID;

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        private int _underlyingID;

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }
        private int _currencyID;

        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }
        private int _exchangeID;

        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }
        private int _originatorType;

        public int OriginatorType
        {
            get { return _originatorType; }
            set { _originatorType = value; }
        }


        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }


        public string OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }

        private bool _isAcked = false;

        public bool IsAcked
        {
            get { return _isAcked; }
            set { _isAcked = value; }
        }
        private bool _isReject = false;

        public bool IsReject
        {
            get { return _isReject; }
            set { _isReject = value; }
        }
        private string _execID = string.Empty;

        public string ExecID
        {
            get { return _execID; }
            set { _execID = value; }
        }
        //private string _transactionTime = DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss");
        //public string TransactionTime
        //{
        //    get { return _transactionTime; }
        //    set { _transactionTime = value; }
        //}
        //private DateTime _settlementDate = DateTimeConstants.MinValue;
        //public DateTime SettlementDate
        //{
        //    get { return _settlementDate; }
        //    set { _settlementDate = value; }
        //}
        #endregion

        /// <summary>
        /// converts all epsilon values to 0
        /// </summary>
        public void SetForUI()
        {
            if (_price == double.Epsilon)
            {
                _price = 0;
            }
        }

    }

}
