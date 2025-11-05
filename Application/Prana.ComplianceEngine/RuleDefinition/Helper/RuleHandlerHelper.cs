using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Prana.ComplianceEngine.RuleDefinition.Helper
{

    internal static class RuleHandlerHelper
    {
        /// <summary>
        /// to log the message and dispose the logger so that the log file is free for another logging process
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        internal static void LogAndDispose(StringBuilder message, String category, int priority, int eventId, TraceEventType severity)
        {
            try
            {
                Dictionary<string, object> arguments = new Dictionary<string, object>();
                Logger.LoggerWrite(message, category, priority, eventId, severity, "log-entry", arguments);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// log the request of user action on rules
        /// </summary>
        /// <param name="operation"></param>
        internal static void UserComplianceActionLogger(Dictionary<String, String> operation)
        {
            try
            {
                StringBuilder log_message = new StringBuilder();
                log_message.Append("UserID : " + Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                log_message.Append("  UserName : " + Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName));

                string method = operation["Method"];
                switch (method)
                {
                    case "OperationOnRuleUDRH":
                    case "ImportRuleUDRH":
                    case "ExportRuleUDRH":
                        log_message.Append("  Rule Name : " + operation["ruleName"]);
                        log_message.Append("  Rule Package : " + operation["packageName"]);
                        log_message.Append("  Rule Category : " + operation["ruleCategory"]);
                        log_message.Append("  Operation Type : " + EnumHelper.GetDescription((RuleOperations)Enum.Parse(typeof(RuleOperations), operation["operationType"].ToString())));
                        break;
                    case "ImportRuleCRH":
                    case "ExportRuleCRH":
                        log_message.Append("  Rule Name : " + operation["ruleName"]);
                        log_message.Append("  Rule Package : " + operation["packageName"]);
                        log_message.Append("  Rule Category : " + operation["ruleCategory"]);
                        log_message.Append("  Operation Type : " + EnumHelper.GetDescription((RuleOperations)Enum.Parse(typeof(RuleOperations), operation["OperationType"].ToString())));
                        break;
                    case "OperationOnRuleCRH":
                        log_message.Append("  Rule Name : " + operation["NewName"]);
                        log_message.Append("  Rule Package : " + operation["rulePackage"]);
                        log_message.Append("  Rule Category : " + operation["ruleCategory"]);
                        log_message.Append("  Operation Type : " + EnumHelper.GetDescription((RuleOperations)Enum.Parse(typeof(RuleOperations), operation["OperationType"].ToString())));
                        break;
                }
                log_message.Append("  Status : Tried");
                LogAndDispose(log_message, RuleNavigatorConstants.COMPLIANCE_LOG, 1, 1, TraceEventType.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// log the response of user action on rules
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="operation"></param>
        /// <param name="status"></param>
        /// <param name="userID"></param>
        internal static void UserComplianceActionLoggerResponse(RuleBase rule, string operation, string status, int userID)
        {
            try
            {
                if (userID == Convert.ToInt32(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                {
                    StringBuilder log_message = new StringBuilder();
                    log_message.Append("UserID : " + Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID));
                    log_message.Append("  UserName : " + Convert.ToString(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName));
                    log_message.Append("  Rule Name : " + rule.RuleName.ToString());
                    log_message.Append("  Rule Package : " + rule.Package.ToString());
                    log_message.Append("  Rule Category : " + rule.Category.ToString());
                    log_message.Append("  Operation Type : " + operation);
                    log_message.Append("  Status : " + status);
                    LogAndDispose(log_message, RuleNavigatorConstants.COMPLIANCE_LOG, 1, 1, TraceEventType.Information);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
