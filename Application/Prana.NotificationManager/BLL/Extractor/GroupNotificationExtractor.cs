using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.LogManager;
using Prana.NotificationManager.DAL;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.NotificationManager.BLL.Extractor
{
    internal class GroupNotificationExtractor : INotificationExtractor
    {
        private Dictionary<String, GroupNotificationDetails> _groupLastValidationTime = new Dictionary<string, GroupNotificationDetails>();
        private object _lockerObject = new object();

        #region INotificationExtractor Members       

        /// <summary>
        /// Returns notification setting of rule group.
        /// If alert to be send
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public NotificationSetting Extract(Alert alert, RuleBase rule)
        {
            try
            {
                GroupBase group = NotificationCache.GetInstance().GetGroupForRule(alert.GroupId);
                if (this.GetIsValidAlert(group.Notification, alert))
                {
                    if (IsSend(alert, group))
                        return group.Notification;
                    else
                        return null;
                }
                else
                    return null;
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
        /// On startup and save from client ui reloaded cache.
        /// </summary>
        public void InitializeCache()
        {
            try
            {
                lock (_lockerObject)
                {
                    _groupLastValidationTime.Clear();
                    DataSet dsTemp = NotificationDataAccess.GetInstance().GetGroupLastValidatedRule();
                    foreach (DataRow row in dsTemp.Tables[0].Rows)
                    {
                        String key = row["GroupId"].ToString() + MessageFormatter.FormatRuleNameForAlert(row["Dimension"].ToString());
                        if (_groupLastValidationTime.ContainsKey(key))
                        {
                            _groupLastValidationTime[key].LastTriggeredRuleId = _groupLastValidationTime[key].CurrentTriggeredRuleId;
                            _groupLastValidationTime[key].CurrentTriggeredRuleId = row["ruleId"].ToString();
                            _groupLastValidationTime[key].LastValidationTime = Convert.ToDateTime(row["ValidationTime"]);
                        }
                        else
                            _groupLastValidationTime.Add(key, new GroupNotificationDetails { LastValidationTime = Convert.ToDateTime(row["ValidationTime"]), CurrentTriggeredRuleId = row["ruleId"].ToString(), Dimension = row["Dimension"].ToString() });
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

        public void CallDispose()
        {
            try
            {
                _groupLastValidationTime = null;
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
        #endregion

        /// <summary>
        /// Checks if alert to be send or not according to group settings
        /// Updates current and last triggered rule id in rule.
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private bool IsSend(Alert alert, GroupBase group)
        {

            try
            {
                lock (_lockerObject)
                {
                    int duration = NotificationCache.GetInstance().GetMinutesForId(group.Notification.LimitFrequencyMinutes);
                    String key = alert.GroupId + MessageFormatter.FormatRuleNameForAlert(alert.Dimension);
                    DateTime time;
                    if (_groupLastValidationTime.ContainsKey(key))
                    {
                        if (alert.RuleId == _groupLastValidationTime[key].CurrentTriggeredRuleId)
                            return false;
                        else if (alert.RuleId == _groupLastValidationTime[key].LastTriggeredRuleId)
                        {
                            time = Convert.ToDateTime(_groupLastValidationTime[key].LastValidationTime.AddMinutes(duration));

                            //Comparing with trigger time to alert validation time 
                            if (((alert.ValidationTime.CompareTo(time)) < 0))
                            {
                                return false;
                            }
                            else
                            {
                                _groupLastValidationTime[key].LastValidationTime = alert.ValidationTime;
                                _groupLastValidationTime[key].LastTriggeredRuleId = _groupLastValidationTime[key].CurrentTriggeredRuleId;
                                _groupLastValidationTime[key].CurrentTriggeredRuleId = alert.RuleId;
                                return true;
                            }
                        }
                        else
                        {
                            _groupLastValidationTime[key].LastTriggeredRuleId = _groupLastValidationTime[key].CurrentTriggeredRuleId;
                            _groupLastValidationTime[key].CurrentTriggeredRuleId = alert.RuleId;
                            _groupLastValidationTime[key].LastValidationTime = alert.ValidationTime;
                            return true;
                        }
                    }
                    else
                    {
                        _groupLastValidationTime.Add(key, new GroupNotificationDetails { LastValidationTime = alert.ValidationTime, CurrentTriggeredRuleId = alert.RuleId, LastTriggeredRuleId = String.Empty, Dimension = alert.Dimension });
                        return true;
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
                return false;
            }
        }


    }
}
