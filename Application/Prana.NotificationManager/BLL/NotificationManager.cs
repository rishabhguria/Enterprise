using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.NotificationManager.BLL.AmqpConnector;
using Prana.NotificationManager.BLL.Extractor;
using Prana.NotificationManager.BLL.Processor;
using Prana.NotificationManager.Delegates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.NotificationManager.BLL
{
    public class NotificationManager
    {
        #region singletonInstance
        private static NotificationManager _notificationManager;
        private static object _notificationManagerLocker = new Object();

        public static NotificationManager GetInstance()
        {
            lock (_notificationManagerLocker)
            {
                if (_notificationManager == null)
                    _notificationManager = new NotificationManager();
                return _notificationManager;
            }
        }


        private NotificationManager()
        {
        }


        #endregion

        /// <summary>
        /// Initializes manager.
        /// </summary>

        /// <summary>
        /// list contains string field of compliance rules.
        /// </summary>
        private static List<string> fieldLists = new List<string> { "Symbol", "UnderlyingSymbol", "AccountShortName", "AccountLongName", "MasterFundName", "Exchange" };

        public void InitializeManager()
        {
            try
            {
                AmqpConnectionManager.InitializeAmqpConnection();
                NotificationCache.GetInstance().InitializeCache();
                NotificationExtractionManager.GetInstance().Initialize();
                AmqpConnectionManager.alertEvent += new AlertDelegate(AmqpConnectionManager_alertEvent);
                AmqpConnectionManager.RuleSaved += AmqpConnectionManager_RuleSaved;
                AmqpConnectionManager.PreTradeApprovalInfoEvent += AmqpConnectionManager_PreTradeApprovalInfoEvent;
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
        /// Updating the rule Id for Pending Approval Alerts and getting unique rule Id
        /// </summary>
        /// <param name="preTradeApprovalInfoEvent"></param>
        private void AmqpConnectionManager_PreTradeApprovalInfoEvent(DataSet preTradeApprovalInfoEvent)
        {
            try
            {
                DataTable dataTable = preTradeApprovalInfoEvent.Tables["TriggeredAlerts"];
                NotificationCache.GetInstance().UpdateRuleIdForAlerts(dataTable);
                List<string> uniqueRuleName = (from c in preTradeApprovalInfoEvent.Tables["TriggeredAlerts"].Select().AsEnumerable()
                                               where !string.IsNullOrWhiteSpace(c["RuleName"].ToString())
                                               select c["RuleName"].ToString()).Distinct().ToList();

                dataTable.Select().ToList<DataRow>().ForEach(x =>
                {
                    if (x["OverrideUserId"].ToString().Trim().Equals(String.Empty))
                        dataTable.Rows.Remove(x);
                });


                List<Alert> alerts = Alert.GetAlertObjectFromDataTable(preTradeApprovalInfoEvent.Tables["TriggeredAlerts"]);

                // Distinct() is not working in case of Alert class list. I have used foreach to eliminate duplicate alerts before sending email notification
                // To minimize the impact on Compliance module, I have not created override methods for Equals() and GetHashCode() by which Distinct() can work properly
                List<Alert> distinctAlerts = new List<Alert>();
                foreach (Alert alert in alerts)
                {
                    if (!distinctAlerts.Contains(alert))
                        distinctAlerts.Add(alert);
                }

                foreach (Alert alert in distinctAlerts)
                {
                    RuleBase rule = NotificationCache.GetInstance().GetRuleForAlert(alert);
                    if (rule != null)
                    {
                        alert.RuleId = rule.RuleId;
                        alert.GroupId = rule.GroupId;
                        alert.Summary = "A request has been made by “user” to approve a trade that would breach compliance. Please review the details in the Pending Approval module.";
                    }
                }
                bool isTradeFromRebalancer = Convert.ToBoolean(preTradeApprovalInfoEvent.Tables["preTradeApprovalInfo"].Rows[0]["IsTradeFromRebalancer"].ToString());

                if (isTradeFromRebalancer)
                    EmailProcessor.TotalAlerts = distinctAlerts.Count;
                else
                    EmailProcessor.TotalAlerts = 1;
                NotificationProcessingManager.GetInstance().ProcessTradeApproval(preTradeApprovalInfoEvent, distinctAlerts);
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
        /// Reloads on save operation
        /// </summary>
        /// <param name="data"></param>
        void AmqpConnectionManager_RuleSaved(DataSet data)
        {
            try
            {
                NotificationCache.GetInstance().InitializeCache();
                NotificationExtractionManager.GetInstance().Initialize();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get rule for alert and trigger according to notification settings.
        /// </summary>
        /// <param name="alert"></param>
        private void AmqpConnectionManager_alertEvent(Alert alert, bool isCancel,string replaceAlertType)
        {
            try
            {
                RuleBase rule = NotificationCache.GetInstance().GetRuleForAlert(alert);
                if (rule != null)
                {
                    alert.RuleId = rule.RuleId;
                    alert.GroupId = rule.GroupId;
                    if (alert.PackageName == RulePackage.PostTrade)
                        alert.Status = "RuleViolated";

                    List<string> updatedConstraints = UpdateConstraints(alert.ConstraintFields, alert.Threshold, alert.ActualResult);
                    alert.ConstraintFields = updatedConstraints[0];
                    alert.Threshold = updatedConstraints[1];
                    alert.ActualResult = updatedConstraints[2];

                    NotificationSetting notification = NotificationExtractionManager.GetInstance().Extract(alert, rule);
                    if (notification == null)
                        return;
                    else if (isCancel)
                    {
                        alert.UserNotes = "Original order cancelled.";
                        NotificationProcessingManager.GetInstance().ProcessTradeCancel(new List<Alert> { alert });
                    }
                    else if(!string.IsNullOrWhiteSpace(replaceAlertType))
                    {
                        NotificationProcessingManager.GetInstance().ProcessTradeReplace(new List<Alert> { alert }, replaceAlertType);
                    }
                    else
                        NotificationProcessingManager.GetInstance().Process(alert, notification);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// TO update Thresold and Actual Result if there is single numeric and multiple string values.
        /// </summary>
        /// <param name="constraintFields"></param>
        /// <param name="threshold"></param>
        /// <param name="actualResult"></param>
        /// <returns></returns>
        private List<string> UpdateConstraints(string constraintFields, string threshold, string actualResult)
        {
            List<string> finalList = new List<string>();
            try
            {
                List<string> constraintFieldsList = new List<string>();
                List<string> thresholdList = new List<string>();
                List<string> actualResultList = new List<string>();

                constraintFieldsList = constraintFields.Split("~".ToCharArray()).ToList();
                thresholdList = threshold.Split("~".ToCharArray()).ToList();
                actualResultList = actualResult.Split("~".ToCharArray()).ToList();

                double numericValue;
                string newConstraintFields = "";
                string newThreshold = "";
                string newActualResult = "";

                for (int i = 0; i < thresholdList.Count; i++)
                {
                    bool isNumeric = double.TryParse(thresholdList[i], out numericValue);
                    if (isNumeric && !fieldLists.Contains(constraintFieldsList[i]))
                    {
                        newConstraintFields += constraintFieldsList[i] + "~";
                        newThreshold += thresholdList[i] + "~";
                        newActualResult += actualResultList[i] + "~";
                    }
                }
                if (newConstraintFields.Length > 0)
                {
                    newConstraintFields = newConstraintFields.Substring(0, newConstraintFields.Length - 1);
                    newThreshold = newThreshold.Substring(0, newThreshold.Length - 1);
                    newActualResult = newActualResult.Substring(0, newActualResult.Length - 1);

                    finalList.Add(newConstraintFields);
                    finalList.Add(newThreshold);
                    finalList.Add(newActualResult);
                }
                else
                {
                    finalList.Add("N/A");
                    finalList.Add("N/A");
                    finalList.Add("N/A");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return finalList;
        }

        /// <summary>
        /// Disposes all cache.
        /// </summary>
        public void Close()
        {
            try
            {
                AmqpConnectionManager.alertEvent -= new AlertDelegate(AmqpConnectionManager_alertEvent);
                AmqpConnectionManager.RuleSaved -= AmqpConnectionManager_RuleSaved;
                NotificationCache.GetInstance().Dispose();
                NotificationExtractionManager.GetInstance().Dispose();
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
