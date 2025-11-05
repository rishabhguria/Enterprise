using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.NotificationManager.BLL.HelperClasses
{
    internal class PendingApprovalHelper
    {
        /// <summary>
        /// Get EmailIds CSV string for override user for Pending Approval Trades
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="overRideUserEmailIds"></param>
        /// <returns></returns>
        static internal string GetEmailIdsForUser(List<string> userIds)
        {
            try
            {
                string userEmailIds = String.Empty;
                Dictionary<int, string> companyUserEmailIds = CommonDataCache.ComplianceCacheManager.GetCompanyUserEmailIds();
                List<string> userEmail = new List<string>();

                foreach (KeyValuePair<int, string> userId in companyUserEmailIds)
                {
                    if (userIds.Contains(userId.Key.ToString()))
                    {
                        if (!userEmail.Contains(userId.Value))
                            userEmail.Add(userId.Value);
                    }
                }
                userEmailIds = string.Join(",", userEmail);
                return userEmailIds;
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
                return null;
            }
        }
    }
}
