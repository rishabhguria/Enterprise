using System;

namespace Prana.Tools
{
    class ZeroPositionAlert
    {
        private string _symbol = String.Empty;
        private double _zeroPosFrequncy = 0;
        private double _netQuantity = 0;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public double ZeroPositionFrequnecy
        {
            get { return _zeroPosFrequncy; }
            set { _zeroPosFrequncy = value; }
        }
        public double NetQuantity
        {
            get { return _netQuantity; }
            set { _netQuantity = value; }
        }
    }
}
