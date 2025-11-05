using System;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable]
    public class UDASymbolReq
    {
        private string _tickerSymbol = string.Empty;
        public string TickerSymbol
        {
            get { return _tickerSymbol; }
            set { _tickerSymbol = value; }
        }

        private string _underlyingSymbol = string.Empty;
        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }


    }
}
