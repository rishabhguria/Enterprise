using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessObjects
{
    public class FactSetSymbolResponse
    {
        private string _tickerSymbol = string.Empty;
        public string TickerSymbol
        {
            get { return _tickerSymbol; }
            set { _tickerSymbol = value; }
        }

        private string _factSetSymbol = string.Empty;
        public string FactSetSymbol
        {
            get { return _factSetSymbol; }
            set { _factSetSymbol = value; }
        }

        private int _auecID;
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private AssetCategory _assetCategory;
        public AssetCategory AssetCategory
        {
            get { return _assetCategory; }
            set { _assetCategory = value; }
        }
    }
}
