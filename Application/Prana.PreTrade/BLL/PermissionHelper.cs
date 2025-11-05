using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.PreTrade.BLL
{
    internal class PermissionHelper
    {
        /// <summary>
        /// Return true whether user has override permission for specific rule
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="uniqueRules">The unique rules.</param>
        /// <returns></returns>
        internal static RuleOverrideType GetOverridePermissionForRule(int userID, HashSet<string> uniqueRules)
        {
            try
            {
                return ComplianceCacheManager.GetOverridePermissionForRule(userID, uniqueRules);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return RuleOverrideType.Soft;
            }
        }

        /// <summary>
        /// Returns alert type for specific rule
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="uniqueRules"></param>
        /// <returns></returns>
        internal static AlertType GetAlertTypeForRule(int userID, string uniqueRules)
        {
            try
            {
                return ComplianceCacheManager.GetAlertForRule(userID, uniqueRules);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return AlertType.HardAlert;
            }
        }
    }
}
