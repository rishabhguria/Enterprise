using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    public abstract class ImportBase
    {

        private int _assetID = 0;

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private AssetCategory _assetType;

        public AssetCategory AssetType
        {
            get { return _assetType; }
            set { _assetType = value; }
        }
        private int _call_Put = 0;

        public int Call_Put
        {
            get { return _call_Put; }
            set { _call_Put = value; }
        }

        private int _underlyingID = 0;

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        private double _strikePrice = 0;

        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private string _cUSIP = string.Empty;
        public string CUSIP
        {
            get { return _cUSIP; }
            set { _cUSIP = value; }
        }

        private string _sEDOL = string.Empty;
        public string SEDOL
        {
            get { return _sEDOL; }
            set { _sEDOL = value; }
        }

        private string _iSIN = string.Empty;
        public string ISIN
        {
            get { return _iSIN; }
            set { _iSIN = value; }
        }

        private string _rIC = string.Empty;
        public string RIC
        {
            get { return _rIC; }
            set { _rIC = value; }
        }

        private string _osiOptionSymbol = string.Empty;

        public string OSIOptionSymbol
        {
            get { return _osiOptionSymbol; }
            set { _osiOptionSymbol = value; }
        }

        private string _idcoOptionSymbol = string.Empty;

        public string IDCOOptionSymbol
        {
            get { return _idcoOptionSymbol; }
            set { _idcoOptionSymbol = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private int _aUECID = 0;
        public int AUECID
        {
            get { return _aUECID; }
            set { _aUECID = value; }
        }


        private int _exchangeID = 0;
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private string _sideTagValue = string.Empty;
        public string SideTagValue
        {
            get { return _sideTagValue; }
            set { _sideTagValue = value; }
        }

        private string _side = string.Empty;
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }



        private int _venueID = 0;
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }
        private int _counterPartyID = 0;
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }
        private int _userID = 0;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private int _tradingAccountID = 0;
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        private string _aUECLocalDate = string.Empty;
        public string AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }
        }

        private int _strategyID = 0;
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private string _strategy = string.Empty;
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

    }
}
