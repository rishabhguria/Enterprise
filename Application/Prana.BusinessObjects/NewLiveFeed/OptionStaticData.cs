using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OptionStaticData
    {
        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _underlyingSymbol = string.Empty;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private OptionType _putOrCall;
        public OptionType PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        private long _contractSize;
        public long ContractSize
        {
            get { return _contractSize; }
            set { _contractSize = value; }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        #region Other Symbologies
        private string bloombergSymbol = string.Empty;
        public string BloombergSymbol
        {
            get { return bloombergSymbol; }
            set
            {
                if (bloombergSymbol != value)
                    bloombergSymbol = value;
            }
        }

        private string factsetSymbol = string.Empty;
        public string FactSetSymbol
        {
            get { return factsetSymbol; }
            set
            {
                if (factsetSymbol != value)
                    factsetSymbol = value;
            }
        }

        private string activeSymbol = string.Empty;
        public string ActivSymbol
        {
            get { return activeSymbol; }
            set
            {
                if (activeSymbol != value)
                    activeSymbol = value;
            }
        }
        #endregion

        public OptionStaticData()
        {
        }
    }
}
