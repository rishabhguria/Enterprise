namespace Prana.CommonDataCache
{
    public class StandardCurrencyPair
    {
        int _fromCurrency;
        int _toCurrency;
        string _symbol;

        public int FromCurrency
        {
            get { return _fromCurrency; }
            set { _fromCurrency = value; }
        }

        public int ToCurrency
        {
            get { return _toCurrency; }
            set { _toCurrency = value; }
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
    }
}
