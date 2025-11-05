using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OptionGreeks
    {
        private char _splitter = Seperators.SEPERATOR_2;
        public OptionGreeks()
        {
            _delta = 0.0;
            _theta = 0.0;
            _vega = 0.0;
            _rho = 0.0;
            _volatility = 0.0;
            _strikePrice = 0.0;
        }
        public OptionGreeks(string message)
        {
            string[] greeks = message.Split(_splitter);
            _symbol = greeks[0];
            double.TryParse(greeks[1], out _delta);
            double.TryParse(greeks[2], out _theta);
            double.TryParse(greeks[3], out _vega);
            double.TryParse(greeks[4], out _rho);
            double.TryParse(greeks[5], out _gamma);
            double.TryParse(greeks[6], out _volatility);
            Char.TryParse(greeks[7], out _putOrCalls);
            double.TryParse(greeks[8], out _strikePrice);

        }
        public void SetValues(PranaPositionWithGreeks position)
        {
            _delta = position.Delta;
            _theta = position.Theta;
            _vega = position.Vega;
            _rho = position.Rho;
            _gamma = position.Gamma;
        }

        private double _delta;
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private double _theta;
        public double Theta
        {
            get { return _theta; }
            set { _theta = value; }
        }

        private double _vega;
        public double Vega
        {
            get { return _vega; }
            set { _vega = value; }
        }

        private double _rho;
        public double Rho
        {
            get { return _rho; }
            set { _rho = value; }
        }

        private double _gamma;
        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        private double _volatility;
        public double Volatility
        {
            get { return _volatility; }
            set { _volatility = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _proxySymbol = string.Empty;
        public string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }

        private char _putOrCalls;
        public char PutOrCalls
        {
            get { return _putOrCalls; }
            set { _putOrCalls = value; }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private double _interestRate;
        public double InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        private int _daysToExpiration;
        public int DaysToExpiration
        {
            get { return _daysToExpiration; }
            set { _daysToExpiration = value; }
        }

        private double _simulatedUnderlyingStockPrice = 0.0;
        public double SimulatedUnderlyingStockPrice
        {
            get { return _simulatedUnderlyingStockPrice; }
            set { _simulatedUnderlyingStockPrice = value; }
        }

        private double _simulatedPrice;
        public double SimulatedPrice
        {
            get { return _simulatedPrice; }
            set { _simulatedPrice = value; }
        }

        private double _selectedFeedPrice;
        public double SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        private double _dividendYield;
        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }

        public override string ToString()
        {
            return _symbol.ToString() + _splitter + _delta.ToString() + _splitter + _theta.ToString() + _splitter + _vega.ToString() + _splitter + _rho.ToString() + _splitter + _gamma.ToString() + _splitter + _volatility.ToString() + _splitter + _putOrCalls.ToString() + _splitter + _strikePrice.ToString();
        }
    }
}
