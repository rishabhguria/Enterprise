using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.TradeManager.Extension.CacheStore
{
    public class BlotterCommonCache
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static BlotterCommonCache _blotterCommonCache = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static BlotterCommonCache GetInstance()
        {
            lock (_lock)
            {
                if (_blotterCommonCache == null)
                    _blotterCommonCache = new BlotterCommonCache();
                return _blotterCommonCache;
            }
        }
        #endregion

        private Dictionary<int, DateTime> _dictAUECIDWiseBlotterClearance = new Dictionary<int, DateTime>();
        public Dictionary<int, DateTime> DictAUECIDWiseBlotterClearance
        {
            get { return _dictAUECIDWiseBlotterClearance; }
            set { _dictAUECIDWiseBlotterClearance = value; }
        }
        private GenericRepository<ClearanceData> _clearanceDataFull = null;
        public GenericRepository<ClearanceData> ClearanceDataFull
        {
            get { return _clearanceDataFull; }
            set { _clearanceDataFull = value; }
        }
        public Dictionary<int, bool> DictRolloverPermittedAUEC = new Dictionary<int, bool>();

        #region Blotter Orders Collections
        public OrderBindingList WorkingSubBlotterCollection
        {
            get { return BlotterOrderCollections.GetInstance().WorkingSubsTabCollection; }
        }

        public OrderBindingList OrderBlotterCollection
        {
            get { return BlotterOrderCollections.GetInstance().OrdersTabCollection; }
        }
        public Dictionary<string, OrderSingle> WorkingSubOrderDictionary
        {
            get { return BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection; }
        }
        #endregion
    }
}
