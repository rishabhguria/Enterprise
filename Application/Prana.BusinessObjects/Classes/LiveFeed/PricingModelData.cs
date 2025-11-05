using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects.LiveFeed
{
    public class PricingModelData
    {
        public PricingModelData()
        {
        }

        public PricingModelData(PricingModelData pricingModelData)
        {
            _volatility = pricingModelData.Volatility;
            _optionPrice = pricingModelData.OptionPrice;
            _optSymbol = pricingModelData.OptSymbol;
            _putOrCall = pricingModelData.PutOrCall;
            _stockPrice = pricingModelData.StockPrice;
            _strikePrice = pricingModelData.StrikePrice;
            _underlyingSymbol = pricingModelData.UnderlyingSymbol;
            _expirationDate = pricingModelData.ExpirationDate;
        }

        private double _interestRate = double.MinValue;
        public double InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        private string _optSymbol;
        public string OptSymbol
        {
            get { return _optSymbol; }
            set { _optSymbol = value; }
        }

        private string _underlyingSymbol;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        string _bloombergSymbol = string.Empty;
        public string BloombergSymbol
        {
            get { return _bloombergSymbol; }
            set { _bloombergSymbol = value; }
        }

        double _strikePrice = 0.0;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set
            {
                _strikePrice = value;
            }
        }

        private double _stockPrice = 0.0;
        public double StockPrice
        {
            get { return _stockPrice; }
            set { _stockPrice = value; }
        }

        private Char _putOrCall;
        public Char PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private double _delta = double.MinValue;
        public double Delta
        {
            get
            {
                return _delta;
            }
            set
            {
                _delta = value;
            }
        }

        private double _optionPrice;
        public double OptionPrice
        {
            get { return _optionPrice; }
            set { _optionPrice = value; }
        }

        private double _volatility = double.MinValue;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private double _dividendYield;
        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }

        string _listedExchange = string.Empty;
        public string ListedExchange
        {
            get { return _listedExchange; }
            set { _listedExchange = value; }
        }

        AssetCategory _categoryCode = AssetCategory.None;
        public AssetCategory CategoryCode
        {
            get { return _categoryCode; }
            set { _categoryCode = value; }
        }

        private int _auecID = 0;
        public int AUECID
        {
            get
            {
                return _auecID;
            }
            set
            {
                if (_auecID == value)
                    return;
                _auecID = value;
            }
        }

        private DateTime _expirationDate = DateTimeConstants.MinValue;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }

        }
    }
}
