using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.OptionCalculator.Common
{
    public class PricingUtilityCache
    {
        private static Dictionary<int, DateTime> _dictMarketEndTime = new Dictionary<int, DateTime>();

        private static object _locker = new object();

        public static DateTime GetMarketEndTimeForAUEC(int auecID)
        {
            lock (_locker)
            {
                if (_dictMarketEndTime.ContainsKey(auecID))
                {
                    return _dictMarketEndTime[auecID];
                }
            }
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Update market end time dictionary
        /// </summary>
        /// <param name="dictMarketEndTime"></param>
        public static void UpdateMarketEndTimeCache(Dictionary<int, DateTime> dictMarketEndTime)
        {
            try
            {
                lock (_locker)
                {
                    _dictMarketEndTime = dictMarketEndTime;
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
        }
    }
}
