using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BasketTrading
{
    public class TemplateExchange
    {
        private string _templateExchangeID = string.Empty;
        private string _exchangeIDList = string.Empty;
        private string _exchangeMappingList = string.Empty;

        public string TemplateExchangeID
        {
            set { _templateExchangeID = value; }
            get { return _templateExchangeID; }
        }

        public string ExchangeIDlist
        {
            set { _exchangeIDList = value; }
            get { return _exchangeIDList; }
        }

        public string ExchangeMappinglist
        {
            set { _exchangeMappingList = value; }
            get { return _exchangeMappingList; }
        }
        

    }
}
