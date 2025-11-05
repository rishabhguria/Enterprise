using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PranaPositionWithGreekReqCarrier
    {
        private List<PranaPositionWithGreeks> _pranaPositionWithGreeks;

        public List<PranaPositionWithGreeks> PranaPositionWithGreeksColl
        {
            get { return _pranaPositionWithGreeks; }
            set { _pranaPositionWithGreeks = value; }
        }
        public List<string> SymbolList
        {
            get
            {
                List<string> symbolist = new List<string>();
                foreach (PranaPositionWithGreeks obj in _pranaPositionWithGreeks)
                {
                    if (!symbolist.Contains(obj.Symbol))
                    {
                        symbolist.Add(obj.Symbol);
                    }

                }
                return symbolist;

            }
        }
        private string _requestID;

        public string RequestID
        {
            get { return _requestID; }
            set { _requestID = value; }
        }
        private string _userID;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}
