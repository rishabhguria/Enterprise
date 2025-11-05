using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Prana.NotificationManager.DAL
{
    internal class NotificationDataAccess
    {


        #region singleton
        private static NotificationDataAccess _notificationDataAccess;
        private static object _notificationDataAccessLockerObject = new object();

        /// <summary>
        /// returns instance
        /// </summary>
        /// <returns></returns>
        public static NotificationDataAccess GetInstance()
        {
            try
            {
                lock (_notificationDataAccessLockerObject)
                {
                    if (_notificationDataAccess == null)
                        _notificationDataAccess = new NotificationDataAccess();
                    return _notificationDataAccess;
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

        private NotificationDataAccess()
        {

        }
        #endregion

        /// <summary>
        /// Returns rule notification settings.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, RuleBase> GetRuleNotificationSettings()
        {
            try
            {
                DataSet dsRuleBase = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetAlertNotificationSettings";

                dsRuleBase = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                Dictionary<String, RuleBase> ruleCache = new Dictionary<string, RuleBase>();
                foreach (DataRow dr in dsRuleBase.Tables[0].Rows)
                {
                    RuleBase rule = new UserDefinedRule();
                    rule.RuleId = dr["RuleID"].ToString();
                    rule.RuleName = dr["RuleName"].ToString();
                    rule.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dr["PackageName"].ToString());
                    rule.GroupId = dr["GroupId"].ToString();
                    rule.Notification.PopUpEnabled = Convert.ToBoolean(dr["PopUpEnabled"].ToString());
                    rule.Notification.LimitFrequencyMinutes = Convert.ToInt32(dr["LimitFrequencyMinutes"].ToString());
                    rule.Notification.EmailToList = dr["EmailToList"].ToString();
                    rule.Notification.EmailCCList = dr["EmailCCList"].ToString();
                    rule.Notification.EmailSubject = dr["EmailSubject"].ToString();
                    rule.Notification.EmailEnabled = Convert.ToBoolean(dr["EmailEnabled"].ToString());

                    rule.Notification.AlertInTimeRange = Convert.ToBoolean(dr["AlertInTimeRange"].ToString());
                    rule.Notification.StopAlertOnHolidays = Convert.ToBoolean(dr["StopAlertOnHolidays"].ToString());
                    rule.Notification.StartTime = Convert.ToDateTime(dr["StartTime"].ToString());
                    rule.Notification.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());

                    if (dr["PopUpEnabledUsers"] != null && !string.IsNullOrEmpty(dr["PopUpEnabledUsers"].ToString()))
                        rule.Notification.PopUpEnabledUsers = (dr["PopUpEnabledUsers"].ToString()).Split(',').Select(int.Parse).ToList();
                    rule.Notification.SendInOneEmail = Convert.ToBoolean(dr["SendInOneEmail"].ToString());
                    rule.Notification.TimeSlots[0] = (Convert.ToDateTime(dr["Slot1"].ToString()));
                    rule.Notification.TimeSlots[1] = (Convert.ToDateTime(dr["Slot2"].ToString()));
                    rule.Notification.TimeSlots[2] = (Convert.ToDateTime(dr["Slot3"].ToString()));
                    rule.Notification.TimeSlots[3] = (Convert.ToDateTime(dr["Slot4"].ToString()));
                    rule.Notification.TimeSlots[4] = (Convert.ToDateTime(dr["Slot5"].ToString()));
                    //notification.EmailCCList = dr["EmailCCList"].ToString();                   
                    if (ruleCache.ContainsKey(dr["RuleID"].ToString()))
                    {
                        ruleCache[dr["RuleID"].ToString()] = rule;
                    }
                    else
                    {
                        ruleCache.Add(dr["RuleID"].ToString(), rule);
                    }
                }
                return ruleCache;

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
                return null;
            }
        }

        /// <summary>
        /// returns rules last validation time
        /// </summary>
        /// <returns></returns>
        internal DataSet GetRuleLastValidationTime()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetRuleValidationTime";
                return DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
                return null;
            }
        }

        /// <summary>
        /// Returns group notification settings.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, GroupBase> GroupNotificationSettings()
        {
            try
            {
                DataSet ds = new DataSet();
                Dictionary<string, GroupBase> groupList = new Dictionary<string, GroupBase>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetGroupData";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GroupBase group = new GroupBase();
                    group.GroupId = dr["GroupId"].ToString();
                    group.GroupName = dr["GroupName"].ToString();
                    group.Notification.PopUpEnabled = Convert.ToBoolean(dr["PopUpEnabled"].ToString());
                    group.Notification.LimitFrequencyMinutes = Convert.ToInt32(dr["LimitFrequencyMinutes"].ToString());
                    group.Notification.EmailToList = dr["EmailToList"].ToString();
                    group.Notification.EmailCCList = dr["EmailCCList"].ToString();
                    group.Notification.EmailSubject = dr["EmailSubject"].ToString();
                    group.Notification.EmailEnabled = Convert.ToBoolean(dr["EmailEnabled"].ToString());

                    group.Notification.AlertInTimeRange = Convert.ToBoolean(dr["AlertInTimeRange"].ToString());
                    group.Notification.StopAlertOnHolidays = Convert.ToBoolean(dr["StopAlertOnHolidays"].ToString());
                    group.Notification.StartTime = Convert.ToDateTime(dr["StartTime"].ToString());
                    group.Notification.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                    groupList.Add(group.GroupId, group);

                }
                return groupList;
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
        /// returns group last validation time with rule id and group id.
        /// </summary>
        /// <returns></returns>
        internal DataSet GetGroupLastValidatedRule()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetGroupValidationTime";
                return DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
        /// returns dictionary of limit frequency minutes and id.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, int> GetNotificationFrequency()
        {
            try
            {
                DataSet ds = new DataSet();
                Dictionary<int, int> notificationFrequency = new Dictionary<int, int>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetNotifyFrequency";
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (notificationFrequency.ContainsKey(Convert.ToInt32(dr["ID"].ToString())))
                        notificationFrequency[Convert.ToInt32(dr["ID"].ToString())] = Convert.ToInt32(dr["Minutes"].ToString());
                    else
                        notificationFrequency.Add(Convert.ToInt32(dr["ID"].ToString()), Convert.ToInt32(dr["Minutes"].ToString()));
                }
                return notificationFrequency;
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
