namespace Prana.BusinessObjects
{
    public class MarketDataSymbolMapperHelper
    {
        private string _key = string.Empty;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _root = string.Empty;
        public string Root
        {
            get { return _root; }
            set { _root = value; }
        }

        private string _yearCode1D = string.Empty;
        public string YearCode1D
        {
            get { return _yearCode1D; }
            set { _yearCode1D = value; }
        }

        private string _yearCode2D = string.Empty;
        public string YearCode2D
        {
            get { return _yearCode2D; }
            set { _yearCode2D = value; }
        }

        private string _monthCode1D = string.Empty;
        public string MonthCode1D
        {
            get { return _monthCode1D; }
            set { _monthCode1D = value; }
        }

        private string _monthCode2D = string.Empty;
        public string MonthCode2D
        {
            get { return _monthCode2D; }
            set { _monthCode2D = value; }
        }

        private string _strikePrice = string.Empty;
        public string StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private string _strikePrice6D = string.Empty;
        public string StrikePrice6D
        {
            get { return _strikePrice6D; }
            set { _strikePrice6D = value; }
        }

        private string _strikePrice8D = string.Empty;
        public string StrikePrice8D
        {
            get { return _strikePrice8D; }
            set { _strikePrice8D = value; }
        }

        private string _strikePrice11D = string.Empty;
        public string StrikePrice11D
        {
            get { return _strikePrice11D; }
            set { _strikePrice11D = value; }
        }

        private string _day = string.Empty;
        public string Day
        {
            get { return _day; }
            set { _day = value; }
        }

        private string _optionType = string.Empty;
        public string OptionType
        {
            get { return _optionType; }
            set { _optionType = value; }
        }

        private string _fromCountry = string.Empty;
        public string FromCountry
        {
            get { return _fromCountry; }
            set { _fromCountry = value; }
        }

        private string _toCountry = string.Empty;
        public string ToCountry
        {
            get { return _toCountry; }
            set { _toCountry = value; }
        }
    }
}
