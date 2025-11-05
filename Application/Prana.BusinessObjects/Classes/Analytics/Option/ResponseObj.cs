using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ResponseObj
    {
        private Dictionary<string, OptionGreeks> _calculatedGreeks = new Dictionary<string, OptionGreeks>();

        public Dictionary<string, OptionGreeks> CalculatedGreeks
        {
            get { return _calculatedGreeks; }
            set { _calculatedGreeks = value; }
        }

        // This field is only populated for non option symbols
        private SymbolData _data;

        public SymbolData Data
        {
            get { return _data; }
            set { _data = value; }
        }


    }
}
