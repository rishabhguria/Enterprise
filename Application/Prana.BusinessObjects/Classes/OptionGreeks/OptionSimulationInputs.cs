using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OptionSimulationInputs
    {
        private double _changeVolatility = 1;
        public double ChangeVolatility
        {
            get { return _changeVolatility; }
            set { _changeVolatility = value; }
        }

        private double _changeUnderlyingPrice = 1;
        public double ChangeUnderlyingPrice
        {
            get { return _changeUnderlyingPrice; }
            set { _changeUnderlyingPrice = value; }
        }

        private double _changeInterestRate = 1;
        public double ChangeInterestRate
        {
            get { return _changeInterestRate; }
            set { _changeInterestRate = value; }
        }

        private int _changeDaysToExpiration = 0;
        public int ChangeDaysToExpiration
        {
            get { return _changeDaysToExpiration; }
            set { _changeDaysToExpiration = value; }
        }

        private bool _underlyingPriceAsAbosluteValue = false;
        public bool UnderlyingPriceAsAbosluteValue
        {
            get { return _underlyingPriceAsAbosluteValue; }
            set { _underlyingPriceAsAbosluteValue = value; }
        }
    }
}
