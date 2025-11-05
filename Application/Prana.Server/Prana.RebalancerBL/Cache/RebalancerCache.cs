using System.Collections.Generic;

namespace Prana.RebalancerBL.Cache
{
    public class RebalancerCache
    {
        private int smartNameCount = int.MinValue;
        public int SmartNameCount
        {
            get
            {
                return smartNameCount;
            }
            set
            {
                smartNameCount = value;
            }


        }

        private Dictionary<int, string> tradeListDataDict = new Dictionary<int, string>();
        public Dictionary<int, string> TradeListDataDict
        {
            get
            {
                return tradeListDataDict;
            }
            set
            {
                tradeListDataDict = value;
            }


        }

        #region Singleton Constructor Implementation.
        private static RebalancerCache _instance;
        public static RebalancerCache GetInstance()
        {
            //ensure if Thread Safe
            if (_instance == null)
            {
                _instance = new RebalancerCache();
            }
            return _instance;
        }
        #endregion

    }
}
