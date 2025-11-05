using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ComplianceEngine.Pending_Approval_UI.BLL
{
    static internal class PendingApprovalHelperClass
    {
        /// <summary>
        /// Remove Current user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        static internal string RemoveUserId(string userId)
        {
            try
            {
                List<string> userIds = new List<String>(userId.Split(','));
                string currentUser = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                if ((userIds.Contains(currentUser)))
                {
                    userIds.Remove(currentUser);
                }
                return string.Join(",", userIds);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }
    }
}
