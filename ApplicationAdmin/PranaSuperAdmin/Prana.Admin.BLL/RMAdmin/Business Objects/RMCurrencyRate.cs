namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for RMCurrencyRate.
    /// </summary>
    public class RMCurrencyRate
    {
        #region Private Method

        private int _fromCurrencyID = int.MinValue;
        private int _toCurrencyID = int.MinValue;
        private string _conversionType = string.Empty;
        private string _conversionFactor = string.Empty;
        private string _rMBaseCurrency = string.Empty;
        private string _allOtherCurrencies = string.Empty;

        #endregion Private Method

        #region Constructors
        public RMCurrencyRate()
        {
        }
        public RMCurrencyRate(string rMBaseCurrency, string allOtherCurrencies, string conversionType, string conversionFactor)
        {
            _rMBaseCurrency = rMBaseCurrency;
            _allOtherCurrencies = allOtherCurrencies;
            _conversionType = conversionType;
            _conversionFactor = conversionFactor;
        }
        #endregion Constructors

        #region Properties
        public string RMBaseCurrency
        {
            get { return _rMBaseCurrency; }
            set { _rMBaseCurrency = value; }
        }

        public string AllOtherCurrencies
        {
            get { return _allOtherCurrencies; }
            set { _allOtherCurrencies = value; }
        }

        public string Conversion
        {
            get { return _conversionType; }
            set { _conversionType = value; }
        }

        public int ToCurrency
        {
            get { return _toCurrencyID; }
            set { _toCurrencyID = value; }
        }

        public int FromCurrencyID
        {
            get { return _fromCurrencyID; }
            set { _fromCurrencyID = value; }
        }

        public string CurrencyRates
        {
            get { return _conversionFactor; }
            set { _conversionFactor = value; }
        }

        #endregion Properties
    }
}
