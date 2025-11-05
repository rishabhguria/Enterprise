using System;

namespace Prana.BusinessObjects
{
    public class PSSymbolRequestObject
    {
        public PSSymbolRequestObject()
        {
        }

        public PSSymbolRequestObject(TaxLot taxlot, double volatility)
        {
            _symbol = taxlot.Symbol;
            _underlyingSymbol = taxlot.UnderlyingSymbol;
            _strikePrice = taxlot.StrikePrice;
            if (taxlot.PutOrCall == 0)
                // _putOrCall = taxlot.PutOrCalls.ToString();
                _putOrCall = "P";
            else if (taxlot.PutOrCall == 1)
                _putOrCall = "C";
            _assetID = taxlot.AssetID;
            _expirationDate = taxlot.ExpirationDate;
            _exchangeID = taxlot.ExchangeID;
            _exchangeName = taxlot.ExchangeName;
            _auecID = taxlot.AUECID;
            _volatility = volatility;
        }

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private string _putOrCall = string.Empty;
        public string PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private int _assetID = int.MinValue;
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private DateTime _expirationDate = DateTimeConstants.MinValue;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }

        }

        private int _auecID = int.MinValue;
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private int _exchangeID = int.MinValue;
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private string _exchangeName = string.Empty;
        public string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }

        private double _volatility = double.MinValue;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }
    }
}
