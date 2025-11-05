using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessObjects
{
    public class FactSetSymbolMapper
    {
        private int _auecID;
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private int _assetID;
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private string _exchangeIdentifier;
        public string ExchangeIdentifier
        {
            get { return _exchangeIdentifier; }
            set { _exchangeIdentifier = value; }
        }

        private string _exchangeToken;
        public string ExchangeToken
        {
            get { return _exchangeToken; }
            set { _exchangeToken = value; }
        }

        private string _factSetFormatString;
        public string FactSetFormatString
        {
            get { return _factSetFormatString; }
            set { _factSetFormatString = value; }
        }

        private string _factSetRegionCode;
        public string FactSetRegionCode
        {
            get { return _factSetRegionCode; }
            set { _factSetRegionCode = value; }
        }

        private string _esignalExchangeCode;
        public string EsignalExchangeCode
        {
            get { return _esignalExchangeCode; }
            set { _esignalExchangeCode = value; }
        }

        private string _factSetExchangeCode;
        public string FactSetExchangeCode
        {
            get { return _factSetExchangeCode; }
            set { _factSetExchangeCode = value; }
        }
    }
}
