using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.PostTradeServices.RollOver
{
    public class ClearanceCommonCache
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static ClearanceCommonCache _clearanceCommonCache = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static ClearanceCommonCache GetInstance()
        {
            lock (_lock)
            {
                if (_clearanceCommonCache == null)
                    _clearanceCommonCache = new ClearanceCommonCache();
                return _clearanceCommonCache;
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
    }
}
