using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class SymbolPriceWithDate
    {
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _price = 0;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private DateTime _dateActual;
        /// The symbol price is actually of this date.       
        public DateTime DateActual
        {
            get { return _dateActual; }
            set { _dateActual = value; }
        }
        private DateTime _dateRequired;
        /// The symbol price should be of this date.       
        public DateTime DateRequired
        {
            get { return _dateRequired; }
            set { _dateRequired = value; }
        }
        private int _indicator;
        public int Indicator
        {
            get { return _indicator; }
            set { _indicator = value; }
        }
    }
}
