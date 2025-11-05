using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.NotificationManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Prana.NotificationManager.BLL.Extractor
{
    internal class NotificationCache
    {
        private Dictionary<String, RuleBase> _ruleNotificationCache = new Dictionary<string, RuleBase>();

        private object _cacheLockerObject = new object();
        private Dictionary<String, GroupBase> _groupNotificationCache = new Dictionary<string, GroupBase>();

        private Dictionary<int, int> _notificationFrequency = new Dictionary<int, int>();

        /// <summary>
        /// The cancel tokens for post trade task dictionary
        /// </summary>
        private Dictionary<string, CancellationTokenSource> _cancelTokensForPostTradeTaskDict = new Dictionary<string, CancellationTokenSource>();

        /// <summary>
        /// Gets the cancel tokens for post trade task dictionary.
        /// </summary>
        /// <value>
        /// The cancel tokens for post trade task dictionary.
        /// </value>
        public Dictionary<string, CancellationTokenSource> CancelTokensForPostTradeTaskDict
        {
            get { return _cancelTokensForPostTradeTaskDict; }
        }

        #region singletonInstance
        private static NotificationCache _notificationCache;
        private static object _notificationCacheLocker = new Object();

        internal static NotificationCache GetInstance()
        {
            lock (_notificationCacheLocker)
            {
                if (_notificationCache == null)
                    _notificationCache = new NotificationCache();
                return _notificationCache;
            }
        }


        public NotificationCache()
        {

        }

        #endregion

        /// <summary>
        /// returns rule base for the alert triggered
        /// if alert is not in cache then return default rule.
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        internal RuleBase GetRuleForAlert(Alert alert)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    foreach (String key in _ruleNotificationCache.Keys)
                    {
                        if (alert.PackageName == _ruleNotificationCache[key].Package && alert.RuleName == _ruleNotificationCache[key].RuleName)
                            return _ruleNotificationCache[key];
                    }
                    foreach (String key in _groupNotificationCache.Keys)
                    {
                        foreach (RuleBase rule in _groupNotificationCache[key].RuleList)
                        {
                            if (alert.PackageName == rule.Package && alert.RuleName == rule.RuleName)
                            {
                                return rule;
                            }
                        }
                    }
                    RuleBase defaultRule = DefaultRule();
                    defaultRule.Package = alert.PackageName;
                    defaultRule.Notification.PopUpEnabledUsers.Add(alert.UserId);
                    return defaultRule;
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
        /// Creates default rule 
        /// 
        /// </summary>
        /// <returns></returns>
        private RuleBase DefaultRule()
        {
            try
            {
                //TODO: Currently creating user defined rule need to correct.
                RuleBase rule = new UserDefinedRule();
                rule.GroupId = "-1";
                rule.RuleId = "-1";
                return rule;
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
        /// Returns group settings for rule triggered.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        internal GroupBase GetGroupForRule(string groupId)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    if (_groupNotificationCache.ContainsKey(groupId))
                        return _groupNotificationCache[groupId];
                    else
                        return null;
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
        /// Initializes cache on stratup and save.
        /// </summary>
        internal void InitializeCache()
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    _notificationFrequency.Clear();
                    _groupNotificationCache.Clear();
                    _ruleNotificationCache.Clear();
                    _notificationFrequency = NotificationDataAccess.GetInstance().GetNotificationFrequency();
                    Dictionary<String, RuleBase> ruleNotificationCache = new Dictionary<string, RuleBase>();
                    _groupNotificationCache = NotificationDataAccess.GetInstance().GroupNotificationSettings();
                    ruleNotificationCache = NotificationDataAccess.GetInstance().GetRuleNotificationSettings();
                    foreach (String key in ruleNotificationCache.Keys)
                    {
                        if (ruleNotificationCache[key].GroupId == "-1")
                        {
                            _ruleNotificationCache.Add(key, ruleNotificationCache[key]);
                        }
                        else
                        {
                            if (_groupNotificationCache.ContainsKey(ruleNotificationCache[key].GroupId))
                                _groupNotificationCache[ruleNotificationCache[key].GroupId].RuleList.Add(ruleNotificationCache[key]);
                            else
                                InformationReporter.GetInstance.Write("Rule in unknown Group. Rule Name: " + ruleNotificationCache[key].RuleName);
                        }
                    }
                    //this for loop is used to set the trigger time for email sending in one lot.
                    foreach (RuleBase rule in ruleNotificationCache.Values)
                    {
                        NotificationSetting notification = rule.Notification;
                        if (notification.LimitFrequencyMinutes == 8)
                        {
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
                                if (_cancelTokensForPostTradeTaskDict.ContainsKey(rule.RuleName))
                                {
                                    _cancelTokensForPostTradeTaskDict[rule.RuleName].Cancel();
                                    CancellationTokenSource cancellationToken = new CancellationTokenSource();
                                    Action<string> processPostTradeCacheAction = Prana.NotificationManager.BLL.Processor.NotificationProcessingManager.GetInstance().ProcessScheduleAlerts;
                                    System.Threading.Tasks.Task WaitTask = System.Threading.Tasks.Task.Delay(triggerTime);
                                    WaitTask.ContinueWith(_ => processPostTradeCacheAction(rule.RuleName), cancellationToken.Token);
                                    _cancelTokensForPostTradeTaskDict[rule.RuleName] = cancellationToken;
                                }
                                else
                                {
                                    CancellationTokenSource cancellationToken = new CancellationTokenSource();
                                    Action<string> processPostTradeCacheAction = Prana.NotificationManager.BLL.Processor.NotificationProcessingManager.GetInstance().ProcessScheduleAlerts;
                                    System.Threading.Tasks.Task WaitTask = System.Threading.Tasks.Task.Delay(triggerTime);
                                    WaitTask.ContinueWith(_ => processPostTradeCacheAction(rule.RuleName), cancellationToken.Token);
                                    _cancelTokensForPostTradeTaskDict.Add(rule.RuleName, cancellationToken);
                                }
                            }
                        }
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
        /// Returns minutes for Limit frequency minutes id.
        /// </summary>
        /// <param name="limitFrequencyMinutes"></param>
        /// <returns></returns>
        internal int GetMinutesForId(int limitFrequencyMinutes)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    return _notificationFrequency[limitFrequencyMinutes];
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
                //TODO: default minutes to trigger is 10 mins
                return 10;
            }
        }

        /// <summary>
        /// Disposes cache.
        /// </summary>
        internal void Dispose()
        {
            try
            {
                _ruleNotificationCache = null;
                _notificationFrequency = null;
                _groupNotificationCache = null;
                _notificationCache = null;
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
        /// Updating the RuleId for alerts
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        internal void UpdateRuleIdForAlerts(DataTable triggeredAlerts)
        {
            try
            {
                if (triggeredAlerts != null)
                {
                    foreach (DataRow row in triggeredAlerts.Rows)
                    {
                        foreach (String key in _ruleNotificationCache.Keys)
                        {
                            var pack = (RulePackage)(Enum.Parse(typeof(RulePackage), row["PackageName"].ToString()));
                            var ruleName = row["RuleName"].ToString();
                            if ((RulePackage)(Enum.Parse(typeof(RulePackage), row["PackageName"].ToString())) == _ruleNotificationCache[key].Package && row["RuleName"].ToString() == _ruleNotificationCache[key].RuleName)
                            {
                                row["RuleId"] = _ruleNotificationCache[key].RuleId;
                            }
                        }
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
    }
}
