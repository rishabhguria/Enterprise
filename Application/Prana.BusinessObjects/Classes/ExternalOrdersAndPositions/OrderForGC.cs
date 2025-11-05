using System;

namespace Prana.BusinessObjects
{
    public class OrderForGC : ImportBase
    {
        #region private Variable
        private string _msgType = string.Empty;
        private string _orderSide = string.Empty;
        private string _orderSideTagValue = string.Empty;
        private string _orderType = string.Empty;
        private string _orderTypeTagValue = string.Empty;
        private double _quantity = 0.0;
        private double _avgPrice = double.Epsilon;

        private double _price = double.Epsilon;

        private string _venue = string.Empty;
        private int _venueID = int.MinValue;
        private string _handlingInstruction = string.Empty;
        private string _executionInstruction = string.Empty;
        private int _counterPartyID = int.MinValue;
        private string _counterPartyName = string.Empty;

        private string _tif = string.Empty;
        private int _tradingAccountID = Int32.MinValue;

        private string _companyUserName = string.Empty;

        private string _orderID = string.Empty;
        private string _clOrderID = string.Empty;

        private string _orderStatus = string.Empty;
        private string _orderStatusTagValue = string.Empty;

        private string _sendingTime = string.Empty;

        private int _companyUserID = int.MinValue;

        private int _assetID = Int32.MinValue;
        private string _assetName = string.Empty;
        private int _underlyingID = Int32.MinValue;
        private string _underlyingName = string.Empty;
        private int _exchangeID = Int32.MinValue;
        private string _exchangeName = string.Empty;
        private int _currencyID = Int32.MinValue;
        private string _currencyName = string.Empty;
        private int _auecID = Int32.MinValue;
        private string _tradingAccountName = string.Empty;

        private int _strategyID = int.MinValue;
        private int _accountID = int.MinValue;

        private string _underlyingSymbol = string.Empty; // By Sandeep 24-04-07
        private double _cumQty = 0.0;
        private double _lastShares = double.MinValue;
        private double _lastPrice = double.MinValue;
        private string _lastMarket = string.Empty;

        private string _transactionTime = DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss");

        private string _text = string.Empty;
        //***************** Option Fields: Ashish
        private string _maturityMonthYear = string.Empty;
        private string _maturityDay = string.Empty;
        private int _putOrCall = int.MinValue; // 0 = Put, 1 = Call
        private double _strikePrice = double.Epsilon;
        //********************
        private bool _isInternalOrder = true;

        private DateTime _settlementDate = DateTimeConstants.MinValue;
        private DateTime _expirationDate = DateTimeConstants.MinValue;

        private DateTime _auecLocalDate = DateTimeConstants.MinValue;
        private string _listedExchangeIdentifier = string.Empty;

        #endregion


        #region private Variable
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }


        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }
        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public string HandlingInstruction
        {
            get { return _handlingInstruction; }
            set { _handlingInstruction = value; }
        }

        public string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
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

        public string ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }

        public string SendingTime
        {
            get { return _sendingTime; }
            set { _sendingTime = value; }
        }

        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
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

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        public string AssetName
        {
            get { return _assetName; }
            set { _assetName = value; }
        }

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        public string UnderlyingName
        {
            get { return _underlyingName; }
            set { _underlyingName = value; }
        }

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int ExchangeID
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

        public string CurrencyName
        {
            get { return _currencyName; }
            set { _currencyName = value; }
        }

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        // adding new keyword as initial balue in base and derived is different (0 and int.min)
        new public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        public string TradingAccountName
        {
            get { return _tradingAccountName; }
            set { _tradingAccountName = value; }
        }


        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }
        public string CounterPartyName
        {
            get { return _counterPartyName; }
            set { _counterPartyName = value; }
        }

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        public double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }

        public string TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        public int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
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

        // adding the new keyword as the initial val is different in base and derived (0 and int.minval)
        new public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

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


        public string LastMarket
        {
            get { return _lastMarket; }
            set { _lastMarket = value; }
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
        public string CompanyUserName
        {
            get { return _companyUserName; }
            set { _companyUserName = value; }
        }

        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }
        public DateTime SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        // adding the new keyword as the type is different in base and derived (string and datetime)
        new public DateTime AUECLocalDate
        {
            get { return _auecLocalDate; }
            set { _auecLocalDate = value; }
        }

        public string ListedExchangeIdentifier
        {
            get { return _listedExchangeIdentifier; }
            set { _listedExchangeIdentifier = value; }
        }

        #endregion

    }
}
