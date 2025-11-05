using Prana.LogManager;
using System;

namespace Prana.CommonDataCache.Cache_Classes
{
    public class HeatMapCacheManager
    {

        /// <summary>
        /// Getting HeatMap Module Enabled For User or not
        /// </summary>
        /// <param name="userId">User Id </param>
        /// <returns> Permission </returns>
        public static bool GetHeatMapModuleEnabledForUser(int userId)
        {
            if (HeatMapCacheData.GetInstance().GetHeatMapModuleEnabledForCompany())
            {
                if (HeatMapCacheData.GetInstance().GetHeatMapModuleEnabledForUser(userId))
                    return true;
                else
                    return false;
            }
            else
            {
                //returning false if compliance module is disabled for company
                return false;
            }
        }

        /// <summary>
        /// Returns true if any of the module is enabled
        /// </summary>
        /// <returns></returns>
        public static bool GetHeatMapModuleEnabled()
        {
            try
            {
                return HeatMapCacheData.GetInstance().GetHeatMapModuleEnabledForCompany();
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
                return false;
            }
        }


    }
}
