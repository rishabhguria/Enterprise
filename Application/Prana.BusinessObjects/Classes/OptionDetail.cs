using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;

namespace Prana.BusinessObjects
{
    public class OptionDetail
    {
        private int _auecID = int.MinValue;
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            set { _symbol = value; }
            get { return _symbol; }
        }

        private string _underlyingSymbol = string.Empty;
        public string UnderlyingSymbol
        {
            set { _underlyingSymbol = value; }
            get { return _underlyingSymbol; }
        }

        private DateTime _expirationDate = DateTimeConstants.MinValue;
        public DateTime ExpirationDate
        {
            set { _expirationDate = value; }
            get { return _expirationDate; }
        }

        private double _strikePrice = 0.0;
        public double StrikePrice
        {
            set { _strikePrice = value; }
            get { return _strikePrice; }
        }

        private ApplicationConstants.SymbologyCodes _symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
        public ApplicationConstants.SymbologyCodes Symbology
        {
            set { _symbology = value; }
            get { return _symbology; }
        }

        private OptionType _optionType;
        public OptionType OptionType
        {
            set { _optionType = value; }
            get { return _optionType; }
        }

        private AssetCategory _assetCategory;
        public AssetCategory AssetCategory
        {
            set { _assetCategory = value; }
            get { return _assetCategory; }
        }

        private double _strikePriceMultiplier = 1;
        public double StrikePriceMultiplier
        {
            set { _strikePriceMultiplier = value; }
            get { return _strikePriceMultiplier; }
        }

        private string _esignalOptionRoot = string.Empty;
        public string EsignalOptionRoot
        {
            get { return _esignalOptionRoot; }
            set { _esignalOptionRoot = value; }
        }

        private string _bloombergOptionRoot = string.Empty;
        public string BloombergOptionRoot
        {
            get { return _bloombergOptionRoot; }
            set { _bloombergOptionRoot = value; }
        }
    }
}