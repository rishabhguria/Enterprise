namespace Prana.BusinessObjects
{
    public class MarketDataSymbolMapper
    {
        private int _auecID;
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

        private string _exchangeIdentifier;
        public string ExchangeIdentifier
        {
            get { return _exchangeIdentifier; }
            set { _exchangeIdentifier = value; }
        }

        private string _exchangeToken;
        public string ExchangeToken
        {
            get { return _exchangeToken; }
            set { _exchangeToken = value; }
        }

        private string _esignalExchangeCode;
        public string EsignalExchangeCode
        {
            get { return _esignalExchangeCode; }
            set { _esignalExchangeCode = value; }
        }

        private string _factSetExchangeCode;
        public string FactSetExchangeCode
        {
            get { return _factSetExchangeCode; }
            set { _factSetExchangeCode = value; }
        }

        private string _factSetRegionCode;
        public string FactSetRegionCode
        {
            get { return _factSetRegionCode; }
            set { _factSetRegionCode = value; }
        }

        private string _esignalFormatString;
        public string EsignalFormatString
        {
            get { return _esignalFormatString; }
            set { _esignalFormatString = value; }
        }

        private string _factSetFormatString;
        public string FactSetFormatString
        {
            get { return _factSetFormatString; }
            set { _factSetFormatString = value; }
        }

        private string _activFormatString;
        public string ActivFormatString
        {
            get { return _activFormatString; }
            set { _activFormatString = value; }
        }

        private string _bloombergCompositeCode;
        public string BloombergCompositeCode
        {
            get { return _bloombergCompositeCode; }
            set { _bloombergCompositeCode = value; }
        }

        private string _bloombergExchangeCode;
        public string BloombergExchangeCode
        {
            get { return _bloombergExchangeCode; }
            set { _bloombergExchangeCode = value; }
        }

        private string _bloombergFormatString;
        public string BloombergFormatString
        {
            get { return _bloombergFormatString; }
            set { _bloombergFormatString = value; }
        }
    }
}
