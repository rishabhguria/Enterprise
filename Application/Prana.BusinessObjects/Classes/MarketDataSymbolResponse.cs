using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    public class MarketDataSymbolResponse
    {
        private string _tickerSymbol = string.Empty;
        public string TickerSymbol
        {
            get { return _tickerSymbol; }
            set { _tickerSymbol = value; }
        }

        private string _factSetSymbol = string.Empty;
        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        private string _activSymbol = string.Empty;
        public string ActivSymbol
        {
            get { return _activSymbol; }
            set { _activSymbol = value; }
        }
        private string _bloombergSymbol = string.Empty;
        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }
        private int _auecID;
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private AssetCategory _assetCategory;
        public AssetCategory AssetCategory
        {
            get { return _assetCategory; }
            set { _assetCategory = value; }
        }
    }
}