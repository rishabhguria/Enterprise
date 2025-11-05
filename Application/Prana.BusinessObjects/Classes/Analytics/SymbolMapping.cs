using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class ProxyMappedData
    {

        private Dictionary<string, SymbolData> _dictParentSymbols = new Dictionary<string, SymbolData>();

        public Dictionary<string, SymbolData> DictParentSymbols
        {
            get { return _dictParentSymbols; }
            set { _dictParentSymbols = value; }
        }



        private SymbolData _underlyingSymbolData;

        public SymbolData UnderlyingSymbolData
        {
            get { return _underlyingSymbolData; }
            set { _underlyingSymbolData = value; }
        }


        private SymbolData _proxySymbolData;

        public SymbolData ProxySymbolData
        {
            get { return _proxySymbolData; }
            set { _proxySymbolData = value; }
        }


    }
}
