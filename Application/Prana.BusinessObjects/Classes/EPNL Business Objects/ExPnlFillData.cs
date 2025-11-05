namespace Prana.BusinessObjects
{
    public class ExPnlFillData
    {
        private double _avgPrice = 0;

        private double _cumQty = 0;

        private string _orderStatusTagValue = string.Empty;
        public double CumQty
        {
            set { _cumQty = value; }
            get { return _cumQty; }
        }

        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        public string OrderStatusTagValue
        {
            get { return _orderStatusTagValue; }
            set { _orderStatusTagValue = value; }
        }
    }
}
