using System;

namespace Prana.OptionCalculator.Common
{
    public class ProxyDataEventArgs : EventArgs
    {
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _proxySymbol;

        public string ProxySymbol
        {
            get { return _proxySymbol; }
            set { _proxySymbol = value; }
        }

        private bool _useProxySymbol;

        public bool UseProxySymbol
        {
            get { return _useProxySymbol; }
            set { _useProxySymbol = value; }
        }
    }
}
