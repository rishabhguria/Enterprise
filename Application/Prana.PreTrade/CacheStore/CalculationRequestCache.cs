using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Prana.PreTrade.CacheStore
{
    class CalculationRequestCache
    {
        private Dictionary<String, DataTable> _calculationCache = new Dictionary<String, DataTable>();

        private Dictionary<String, ManualResetEvent> _calculationWaiter = new Dictionary<String, ManualResetEvent>();

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static CalculationRequestCache _calculationRequestCache = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private CalculationRequestCache()
        { }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static CalculationRequestCache GetInstance()
        {
            lock (_lock)
            {
                if (_calculationRequestCache == null)
                    _calculationRequestCache = new CalculationRequestCache();
                return _calculationRequestCache;
            }
        }
        #endregion

        /// <summary>
        /// Add a new request to the cache
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="manualResetEvent"></param>
        public void addNewRequest(String reqId, ManualResetEvent manualResetEvent)
        {
            lock (_lock)
            {
                _calculationWaiter.Add(reqId, manualResetEvent);
                _calculationCache.Add(reqId, null);
            }
        }

        /// <summary>
        /// Add the calculated DataTable to the cache
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="calculations"></param>
        public void addCalculations(String reqId, DataTable calculations)
        {
            lock (_lock)
            {
                _calculationCache[reqId] = calculations;
                _calculationWaiter[reqId].Set();
            }
        }

        /// <summary>
        /// Return the calculations and clear the cache
        /// </summary>
        /// <param name="reqID"></param>
        /// <returns></returns>
        public DataTable getCaclulations(String reqID)
        {
            try
            {
                lock (_lock)
                {
                    DataTable dt = _calculationCache[reqID];
                    _calculationCache.Remove(reqID);
                    _calculationWaiter.Remove(reqID);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
    }
}
