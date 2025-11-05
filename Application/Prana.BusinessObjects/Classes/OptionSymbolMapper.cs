namespace Prana.BusinessObjects
{
    public class OptionSymbolMapper
    {
        private int _AUECID;
        public int AUECID
        {
            get { return _AUECID; }
            set { _AUECID = value; }
        }
        private int _AssetID;
        public int AssetID
        {
            get { return _AssetID; }
            set { _AssetID = value; }
        }
        private string _ExchangeIdentifier;
        public string ExchangeIdentifier
        {
            get { return _ExchangeIdentifier; }
            set { _ExchangeIdentifier = value; }
        }
        private string _ExchangeToken;
        public string ExchangeToken
        {
            get { return _ExchangeToken; }
            set { _ExchangeToken = value; }
        }
        private string _EsignalOptionFormatString;
        public string EsignalOptionFormatString
        {
            get { return _EsignalOptionFormatString; }
            set { _EsignalOptionFormatString = value; }
        }
        private string _BloombergOptionFormatString;
        public string BloombergOptionFormatString
        {
            get { return _BloombergOptionFormatString; }
            set { _BloombergOptionFormatString = value; }
        }
        private string _EsignalRootToken;
        public string EsignalRootToken
        {
            get { return _EsignalRootToken; }
            set { _EsignalRootToken = value; }
        }
        private string _BloombergRootToken;
        public string BloombergRootToken
        {
            get { return _BloombergRootToken; }
            set { _BloombergRootToken = value; }
        }
    }
}