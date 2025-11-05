using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using Prana.NotificationManager.DAL;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.NotificationManager.BLL.Extractor
{
    internal class DefaultExtractor : INotificationExtractor
    {
        public static Dictionary<String, DateTime> _ruleLastValidationTime = new Dictionary<String, DateTime>();
        private object _lockerObject = new object();

        #region INotificationExtractor Members

        /// <summary>
        /// Checks if notification for to be send or not 
        /// ifyes then returns settings else returns null
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public NotificationSetting Extract(Alert alert, RuleBase rule)
        {
            try
            {
                if (rule.Package == RulePackage.PreTrade || rule.Package == RulePackage.Basket)
                    return rule.Notification;
                else
                {
                    if (this.GetIsValidAlert(rule.Notification, alert))
                    {
                        if (IsSend(alert, rule.Notification))
                            return rule.Notification;
                        else
                            return null;
                    }
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
        /// On start up and and save operation from client UI it updates cache.
        /// </summary>
        public void InitializeCache()
        {
            try
            {
                lock (_lockerObject)
                {
                    _ruleLastValidationTime.Clear();
                    DataSet dsTemp = NotificationDataAccess.GetInstance().GetRuleLastValidationTime();

                    foreach (DataRow row in dsTemp.Tables[0].Rows)
                    {
                        String key = row["RuleId"].ToString() + MessageFormatter.FormatRuleNameForAlert(row["Dimension"].ToString());
                        if (_ruleLastValidationTime.ContainsKey(key))
                            _ruleLastValidationTime[key] = Convert.ToDateTime(row["ValidationTime"]);
                        else
                            _ruleLastValidationTime.Add(key, Convert.ToDateTime(row["ValidationTime"]));
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
                _ruleLastValidationTime = null;
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
        /// Determines whether [is one minute complete] [the specified alert].
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <param name="notificationSetting">The notification setting.</param>
        /// <returns>
        ///   <c>true</c> if [is one minute complete] [the specified alert]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOneMinuteComplete(Alert alert, NotificationSetting notificationSetting)
        {
            try
            {
                DateTime time;
                String key = alert.RuleId + MessageFormatter.FormatRuleNameForAlert(alert.Dimension);

                if (_ruleLastValidationTime.ContainsKey(key))
                    time = _ruleLastValidationTime[key].AddMinutes(1);
                else
                {
                    _ruleLastValidationTime.Add(key, alert.ValidationTime);
                    time = alert.ValidationTime;
                }

                if (((alert.ValidationTime.CompareTo(time)) < 0))
                    return false;
                else
                {
                    _ruleLastValidationTime[key] = alert.ValidationTime;
                    return true;
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

        /// <summary>
        /// Checks if alert to be send or not according to settings.
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="notificationSetting"></param>
        /// <returns></returns>
        private bool IsSend(Alert alert, NotificationSetting notificationSetting)
        {
            try
            {
                lock (_lockerObject)
                {
                    int duration = NotificationCache.GetInstance().GetMinutesForId(notificationSetting.LimitFrequencyMinutes);
                    DateTime time;
                    String key = alert.RuleId + MessageFormatter.FormatRuleNameForAlert(alert.Dimension);

                    if (notificationSetting.LimitFrequencyMinutes == 8)
                        return true;

                    if (_ruleLastValidationTime.ContainsKey(key))
                    {
                        time = _ruleLastValidationTime[key].AddMinutes(duration);
                    }
                    else
                    {
                        _ruleLastValidationTime.Add(key, alert.ValidationTime);
                        time = alert.ValidationTime;
                    }

                    //Checking by date instead of minutes if frequency is set to once a day
                    if (duration == 1440 && _ruleLastValidationTime[key].Date < alert.ValidationTime.Date)
                    {
                        _ruleLastValidationTime[key] = alert.ValidationTime;
                        return true;
                    }

                    //Comparing with trigger time to alert validation time 
                    if (((alert.ValidationTime.CompareTo(time)) < 0))
                        return false;
                    else
                    {
                        _ruleLastValidationTime[key] = alert.ValidationTime;
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
