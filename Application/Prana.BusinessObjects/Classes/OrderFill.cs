using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OrderFill : PranaBasicMessage
    {
        private string _text = string.Empty;
        private string _msgType = string.Empty;
        private string _clOrderID = string.Empty;
        private string _origClOrderID = string.Empty;
        private string _orderID = string.Empty;
        private string _listID = string.Empty;
        private string _waveID = string.Empty;
        private double _price = double.Epsilon;

        private string _orderStatusTagValue = string.Empty;
        private double _leavesQty = double.Epsilon;
        private double _lastShares = double.Epsilon;
        private string _executionInstruction = string.Empty;
        private string _handlingInstruction = string.Empty;
        //private double _lastQuantity			= double.Epsilon;
        private double _lastPrice = double.Epsilon;
        private string _lastMarket = string.Empty;
        private int _basketSeqNumber = int.MinValue;
        private string _clientOrderID = string.Empty;
        private string _orderStatus = string.Empty;

        private OrderAlgoStartegyParameters _algoProps = new OrderAlgoStartegyParameters();
        private string _algoStrategyID = string.Empty;

        public OrderAlgoStartegyParameters AlgoProperties
        {
            get { return _algoProps; }
            set { _algoProps = value; }
        }
        public string AlgoStrategyID
        {
            get { return _algoStrategyID; }
            set { _algoStrategyID = value; }
        }
        public OrderFill(Order order)
        {
            try
            {
                _algoProps = order.AlgoProperties;
                _algoStrategyID = order.AlgoStrategyID;
                _price = order.Price;
                _avgPrice = order.AvgPrice;
                _clOrderID = order.ClOrderID;
                _cumQty = order.CumQty;
                _lastMarket = order.LastMarket;
                _lastPrice = order.LastPrice;
                _lastShares = order.LastShares;
                _leavesQty = order.LeavesQty;
                _listID = order.ListID;
                _msgType = order.MsgType;
                _orderID = order.OrderID;
                _orderStatusTagValue = order.OrderStatusTagValue;
                _orderStatus = order.OrderStatus;

                _origClOrderID = order.OrigClOrderID;
                _waveID = order.WaveID;
                _clientOrderID = order.ClientOrderID;

                _counterPartyID = order.CounterPartyID;
                _counterPartyName = order.CounterPartyName;

                _venueID = order.VenueID;
                _venue = order.Venue;

                _orderTypeTagValue = order.OrderTypeTagValue;
                _orderType = order.OrderType;

                _orderSideTagValue = order.OrderSideTagValue;
                _orderSide = order.OrderSide;

                _symbol = order.Symbol;
                _quantity = order.Quantity;
                _originalAllocationPreferenceID = order.OriginalAllocationPreferenceID;
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                throw new Exception("Message Format Error at Order Fill Constructor ");
            }
        }

        public OrderFill()
        {
        }

        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public string ClientOrderID
        {
            get { return _clientOrderID; }
            set { _clientOrderID = value; }
        }

        public string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }

        public string OrigClOrderID
        {
            get { return _origClOrderID; }
            set { _origClOrderID = value; }
        }

        public string OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }
        // commenting as base and derived have the same defination and use a common field
        //public double Quantity
        //{
        //    get { return _quantity; }
        //    set { _quantity = value; }
        //}
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

        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }

        public double LeavesQty
        {
            get { return _leavesQty; }
            set { _leavesQty = value; }
        }

        /// <summary>
        /// Fill recieved in last execution
        /// </summary>
        public double LastShares
        {
            get { return _lastShares; }
            set { _lastShares = value; }
        }
        /// <summary>
        /// Deprecated use LastShares instead of this
        /// </summary>
        //public double LastQuantity
        //{
        //    get { return _lastQuantity; }
        //    set { _lastQuantity = value; }
        //}
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
        public int BasketSequenceNumber
        {
            get { return _basketSeqNumber; }
            set { _basketSeqNumber = value; }

        }
        public string ExecutionID
        {
            get { return string.Empty; }
        }
        public string OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public string HandlingInstruction
        {
            get { return _handlingInstruction; }
            set { _handlingInstruction = value; }
        }
        public string ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }
    }
}
