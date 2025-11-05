namespace Prana.BusinessObjects
{
    public class ExPnlLiveFeedData
    {
        private double _askPrice;

        private double _bidPrice;

        private double _highPrice;

        private double _lastPrice;

        private double _lowPrice;

        private double _midPrice;

        private double _closingPrice;
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

        public double MidPrice
        {
            get { return _midPrice; }
            set { _midPrice = value; }
        }

        public double ClosingPrice
        {
            get { return _closingPrice; }
            set { _closingPrice = value; }
        }

        public double LastPrice
        {
            get { return _lastPrice; }
            set
            {
                _lastPrice = value;
            }
        }
    }
}
