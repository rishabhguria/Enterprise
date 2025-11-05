namespace Prana.BusinessObjects.LiveFeed
{
    public class Level1StockPriceData
    {
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }


        private double _bidPrice;

        public double BidPrice
        {
            get { return _bidPrice; }
            set { _bidPrice = value; }
        }

        private double _askPrice;

        public double AskPrice
        {
            get { return _askPrice; }
            set { _askPrice = value; }
        }

        private double _midPrice;

        public double MidPrice
        {
            get { return _midPrice; }
            set { _midPrice = value; }
        }

        //private double _iMidPrice; //intelligent mid
        public double iMidPrice
        {
            get
            {
                // this if condition added for requirement JIRA ENG-16,
                if (_lastPrice == 0)
                {
                    return (_bidPrice + _askPrice) / 2.0;
                }
                else
                {
                    double minVal = (_bidPrice < _askPrice ? _bidPrice : _askPrice);
                    double maxVal = (_bidPrice > _askPrice ? _bidPrice : _askPrice);
                    if (minVal <= _lastPrice && _lastPrice <= maxVal)
                    {
                        return _lastPrice;
                    }
                    else
                    {
                        return (_bidPrice + _askPrice) / 2.0;
                    }
                }
            }
        }

        private double _lastPrice;

        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }

        private double _dividendYield;

        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }



    }
}
