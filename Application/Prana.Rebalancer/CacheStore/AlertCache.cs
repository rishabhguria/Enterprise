using Prana.BusinessObjects.Compliance.Alerting;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Rebalancer.CacheStore
{
    class AlertCache
    {
        private List<Alert> _alertCache = new List<Alert>();

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static AlertCache _alertCacheObject = null;

        /// <summary>
        /// private cunstructor, Initialises the proxy
        /// </summary>
        private AlertCache()
        {

        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static AlertCache GetInstance()
        {
            lock (_lock)
            {
                if (_alertCacheObject == null)
                    _alertCacheObject = new AlertCache();
                return _alertCacheObject;
            }
        }

        #endregion

        /// <summary>
        /// Clears the cache
        /// </summary>
        internal void Clear()
        {
            try
            {
                _alertCache.Clear();
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
        }

        /// <summary>
        /// Addds the triggered alerts to the cache
        /// </summary>
        /// <param name="alerts"></param>
        internal void Add(List<Alert> alerts)
        {
            try
            {
                _alertCache.AddRange(alerts);
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
        }

        /// <summary>
        /// Returns the cache
        /// </summary>
        /// <returns></returns>
        internal List<Alert> GetAlerts()
        {
            try
            {
                return _alertCache;
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
