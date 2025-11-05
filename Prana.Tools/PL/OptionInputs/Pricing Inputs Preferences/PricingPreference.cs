using System;
using System.Xml.Serialization;

namespace Prana.Tools
{
    [XmlRoot("PricingInputPreferences")]
    [Serializable]
    public class PricingPreference
    {
        public PricingPreference()
        {
        }

        #region PI Preferences
        private int _symbolsIndexVal;
        public int SymbolsIndexVal
        {
            get { return _symbolsIndexVal; }
            set { _symbolsIndexVal = value; }
        }

        private bool _closingMark;
        public bool ClosingMark
        {
            get { return _closingMark; }
            set { _closingMark = value; }
        }

        private bool _sharesOutstanding;
        public bool SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set { _sharesOutstanding = value; }
        }

        private bool _all;
        public bool All
        {
            get { return _all; }
            set { _all = value; }
        }

        private bool _lastPrice;
        public bool LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        }

        private bool _delta;
        public bool Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private bool _dividend;
        public bool Dividend
        {
            get { return _dividend; }
            set { _dividend = value; }
        }

        private bool _interestRate;
        public bool InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        private bool _volatility;
        public bool Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private bool _theoreticalPrice;
        public bool TheoreticalPrice
        {
            get { return _theoreticalPrice; }
            set { _theoreticalPrice = value; }
        }

        private bool _manualInput;
        public bool ManualInput
        {
            get { return _manualInput; }
            set { _manualInput = value; }
        }
        #endregion
    }
}
