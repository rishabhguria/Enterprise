using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.NotificationManager.BLL.Extractor;
using Prana.NotificationManager.BLL.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Prana.NotificationManager.BLL.Processor
{
    internal class NotificationProcessingManager
    {
        #region singletonInstance
        private static NotificationProcessingManager _notificationProcessManager;
        private static object _notificationProcessManagerLocker = new Object();

        internal static NotificationProcessingManager GetInstance()
        {
            lock (_notificationProcessManagerLocker)
            {
                if (_notificationProcessManager == null)
                    _notificationProcessManager = new NotificationProcessingManager();
                return _notificationProcessManager;
            }
        }

        private NotificationProcessingManager()
        {
            LoadProcessorCache();
            LoadPostTradeInfoCache();
        }



        #endregion

        /// <summary>
        /// The post trade information cache
        /// </summary>
        private Dictionary<string, List<Alert>> _scheduleAlertsCache = new Dictionary<string, List<Alert>>();


        /// <summary>
        /// Loads the post trade information cache.
        /// </summary>
        private void LoadPostTradeInfoCache()
        {
            try
            {
                DataTable dataTable = Prana.NotificationManager.DAL.AlertsDataManager.GetPostTradeScheduleAlertHis();
                List<Alert> alerts = Alert.GetAlertObjectFromDataTable(dataTable);
                alerts.ForEach(alert =>
                {
                    if (!_scheduleAlertsCache.ContainsKey(alert.RuleName))
                    {
                        _scheduleAlertsCache.Add(alert.RuleName, new List<Alert>());
                        _scheduleAlertsCache[alert.RuleName].Add(alert);
                    }
                    else if (!_scheduleAlertsCache[alert.RuleName].Any(listAlert => listAlert.Dimension.Equals(alert.Dimension)))
                        _scheduleAlertsCache[alert.RuleName].Add(alert);
                });
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
        /// Loads all processor instance.
        /// </summary>
        private void LoadProcessorCache()
        {
            try
            {
                lock (_processorCache)
                {
                    _processorCache.Add(ProcessingBehavior.PopUp, new PopUpProcessor());
                    _processorCache.Add(ProcessingBehavior.Email, new EmailProcessor());
                    _processorCache.Add(ProcessingBehavior.DBUpdater, new DBUpdator());
                    _processorCache.Add(ProcessingBehavior.AlertHistoryPublisher, new AlertHistoryPublisher());
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

        /// <summary>
        /// Process for mail
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="notification"></param>
        internal void Process(Alert alert, NotificationSetting notification)
        {
            try
            {
                if (alert.PackageName == RulePackage.PostTrade && notification.LimitFrequencyMinutes == 8)
                {
                    if (DefaultExtractor.IsOneMinuteComplete(alert, notification))
                    {
                        GetProcessorFor(ProcessingBehavior.DBUpdater).Process(alert, null, NotificationStrategy.Alerting);
                        if (!_scheduleAlertsCache.ContainsKey(alert.RuleName))
                            _scheduleAlertsCache.Add(alert.RuleName, new List<Alert> { alert });
                        else
                        {
                            int ind = _scheduleAlertsCache[alert.RuleName].FindIndex(listAlert => listAlert.Dimension.Equals(alert.Dimension));
                            if (ind == -1)
                                _scheduleAlertsCache[alert.RuleName].Add(alert);
                            else
                                _scheduleAlertsCache[alert.RuleName][ind] = alert;
                        }
                    }
                }
                else
                {
                    GetProcessorFor(ProcessingBehavior.DBUpdater).Process(alert, null, NotificationStrategy.Alerting);
                    GetProcessorFor(ProcessingBehavior.AlertHistoryPublisher).Process(alert, null, NotificationStrategy.Alerting);
                    if (alert.PreTradeType != PreTradeType.ComplianceCheck)
                    {
                        if (alert.PackageName == RulePackage.PostTrade && notification.PopUpEnabled && notification.PopUpEnabledUsers.Count > 0)
                            GetProcessorFor(ProcessingBehavior.PopUp).Process(alert, notification, NotificationStrategy.Alerting);

                        /*Following changes made by Sunil Sharma as per requirements on https://jira.nirvanasolutions.com:8443/browse/PRANA-20455 */

                        //if (alert.PreTradeType != PreTradeType.Stage)
                        GetProcessorFor(ProcessingBehavior.Email).Process(alert, notification, NotificationStrategy.Alerting);
                    }
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


        /// <summary>
        /// Processes the in one batch.
        /// </summary>
        /// <param name="ruleName">Name of the rule.</param>
        /// <param name="timeout">The timeout.</param>
        public void ProcessScheduleAlerts(string ruleName)
        {
            try
            {
                if (_scheduleAlertsCache.ContainsKey(ruleName) && _scheduleAlertsCache[ruleName].Count > 0)
                {
                    RuleBase rule = NotificationCache.GetInstance().GetRuleForAlert(_scheduleAlertsCache[ruleName][0]);
                    NotificationSetting notification = NotificationExtractionManager.GetInstance().Extract(_scheduleAlertsCache[ruleName][0], rule);
                    _scheduleAlertsCache[ruleName].ForEach(alert =>
                    {
                        GetProcessorFor(ProcessingBehavior.AlertHistoryPublisher).Process(alert, null, NotificationStrategy.Alerting);
                        if (notification.PopUpEnabled && notification.PopUpEnabledUsers.Count > 0)
                            GetProcessorFor(ProcessingBehavior.PopUp).Process(alert, notification, NotificationStrategy.Alerting);
                    });
                    if (notification.SendInOneEmail)
                        ((EmailProcessor)GetProcessorFor(ProcessingBehavior.Email)).ProcessInOneBatch(_scheduleAlertsCache[ruleName], NotificationStrategy.Alerting);
                    else
                        _scheduleAlertsCache[ruleName].ForEach(alert => GetProcessorFor(ProcessingBehavior.Email).Process(alert, notification, NotificationStrategy.Alerting));
                    TimeSpan triggerTime = TimeSpan.Zero;
                    notification.TimeSlots.Sort((a, b) => a.CompareTo(b));
                    foreach (DateTime dateTime in notification.TimeSlots)
                    {
                        DateTime curTime = DateTime.Now;
                        if (dateTime.TimeOfDay - curTime.TimeOfDay > TimeSpan.FromSeconds(10) && dateTime.Year != DateTimeConstants.MinValue.Year)
                        {
                            triggerTime = dateTime.TimeOfDay - curTime.TimeOfDay;
                            break;
                        }
                    }
                    if (triggerTime != TimeSpan.Zero)
                    {
                        CancellationTokenSource cancellationToken = new CancellationTokenSource();
                        Action<string> processPostTradeCacheAction = Prana.NotificationManager.BLL.Processor.NotificationProcessingManager.GetInstance().ProcessScheduleAlerts;
                        System.Threading.Tasks.Task WaitTask = System.Threading.Tasks.Task.Delay(triggerTime);
                        WaitTask.ContinueWith(_ => processPostTradeCacheAction(rule.RuleName));
                        NotificationCache.GetInstance().CancelTokensForPostTradeTaskDict[rule.RuleName] = cancellationToken;
                    }
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


        /// <summary>
        /// Returns procesoor for all behaviours.
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        private INotificationProcessor GetProcessorFor(ProcessingBehavior behavior)
        {
            try
            {
                lock (_processorCache)
                {
                    if (_processorCache.ContainsKey(behavior))
                        return _processorCache[behavior];
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Processor not found for ", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        return null;
                    }

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
                return null;
            }
        }

        /// <summary>
        /// Process on Approval Trades alerts
        /// </summary>
        /// <param name="list"></param>
        internal void ProcessTradeApproval(DataSet preTradeApprovalInfoEvent, List<Alert> alerts)
        {
            try
            {
                ProcessPendingApproval.Process(preTradeApprovalInfoEvent);
                EmailProcessorForPendingApprovalAlerts(alerts);
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
        /// Process on Approval Trades alerts
        /// </summary>
        /// <param name="list"></param>
        internal void ProcessTradeCancel( List<Alert> alerts)
        {
            try
            {
                GetProcessorFor(ProcessingBehavior.DBUpdater).Process(alerts[0], null, NotificationStrategy.Alerting);
                GetProcessorFor(ProcessingBehavior.AlertHistoryPublisher).Process(alerts[0], null, NotificationStrategy.Alerting);
                string userName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(alerts[0].ActionUser);
                alerts[0].Summary = "A request made by the " + userName + " to approve a trade that would breach compliance has been CANCELLED. Please review the details in the Alert History module.";
                EmailProcessorForPendingApprovalAlerts(alerts);
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
        /// Process on Approval Trades alerts
        /// </summary>
        /// <param name="list"></param>
        internal void ProcessTradeReplace(List<Alert> alerts, string replaceAlertType)
        {
            try
            {
                GetProcessorFor(ProcessingBehavior.DBUpdater).Process(alerts[0], null, NotificationStrategy.Alerting);
                GetProcessorFor(ProcessingBehavior.AlertHistoryPublisher).Process(alerts[0], null, NotificationStrategy.Alerting);
                string userName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(alerts[0].ActionUser);
                if (replaceAlertType.Equals(BusinessObjects.Compliance.Constants.PreTradeConstants.MSG_NO_ALERT_RECEIVED))
                {
                    alerts[0].Summary = "A previous request made by the " + userName + " to approve trade that would breach compliance has been REPLACED and passed all validations. NO ACTION IS REQUIRED. The details can be reviewed in the Alert History Module.";
                    alerts[0].UserNotes = BusinessObjects.Compliance.Constants.PreTradeConstants.CONST_USER_NOTE;
                }
                else if (replaceAlertType.Equals(AlertType.RequiresApproval.ToString()))
                    alerts[0].Summary = "A previous request made by the " + userName + " to approve a trade has been REPLACED with a trade that would breach compliance and NEEDS APPROVAL. Please review the details in the Pending Approval module.";
                
                else
                    alerts[0].Summary = "A previous request made by the " + userName + " to approve a trade that would breach compliance has been REPLACED and NEEDS NO ACTION. Please review the details in the Alert History module.";
                
                EmailProcessorForPendingApprovalAlerts(alerts);
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
        /// Email Precessor for Pending approval alerts
        /// </summary>
        private void EmailProcessorForPendingApprovalAlerts( List<Alert> alerts)
        {
            try
            {
                foreach (Alert alert in alerts)
                {
                    string overRideUserEmailIds = String.Empty;
                    NotificationSetting notification = new NotificationSetting();
                    if (!String.IsNullOrEmpty(alert.OverrideUserId))
                    {
                        List<string> userIds = new List<String>(alert.OverrideUserId.ToString().Split(','));
                        String getEmailIds = PendingApprovalHelper.GetEmailIdsForUser(userIds);
                        overRideUserEmailIds = getEmailIds;
                    }
                    notification.EmailToList = overRideUserEmailIds;
                    notification.EmailEnabled = true;
                    GetProcessorFor(ProcessingBehavior.Email).Process(alert, notification, NotificationStrategy.Approval);
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


        Dictionary<ProcessingBehavior, INotificationProcessor> _processorCache = new Dictionary<ProcessingBehavior, INotificationProcessor>();
    }
}
