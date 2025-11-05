namespace Prana.MarketDataService.Common
{
    public class MDServiceReqObject
    {
        private string _ticker = string.Empty;

        public string Ticker
        {
            get { return _ticker; }
            set { _ticker = value; }
        }

        private string _bloombergSymbol = string.Empty;

        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        private string _asset = string.Empty;

        public string Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        private string _exchange = string.Empty;

        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }


    }
}
