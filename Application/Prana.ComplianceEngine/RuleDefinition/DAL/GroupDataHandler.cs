using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ComplianceEngine.RuleDefinition.DAL
{
    internal class GroupDataHandler : IDisposable
    {

        internal event GroupOperationHandler GroupOperationResponse;

        #region AMQP Initialization

        String _groupCacheKey = "GroupCommunication";
        String _groupExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);

        /// <summary>
        /// Initializes amqp for group updation.
        /// </summary>
        private void InitializeAmqp()
        {
            try
            {
                AmqpHelper.Started += AmqpHelper_Started;
                AmqpHelper.Stopped += AmqpHelper_Stopped;


                AmqpHelper.InitializeSender(_groupCacheKey, _groupExchangeName, MediaType.Exchange_Direct);

                List<String> groupRoutingKey = new List<string>();
                groupRoutingKey.Add("GroupOperationResponse");
                AmqpHelper.InitializeListenerForExchange(_groupExchangeName, MediaType.Exchange_Direct, groupRoutingKey);

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
        /// Unbind event
        /// </summary>
        /// <param name="amqpReceiver"></param>
        /// <param name="cause"></param>
        void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _groupExchangeName)
                    e.AmqpReceiver.AmqpDataReceived -= new DataReceived(amqpReceiver_AmqpDataReceived);
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
        /// Binds event
        /// </summary>
        /// <param name="amqpReceiver"></param>
        void AmqpHelper_Started(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _groupExchangeName)
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived);
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
        /// Raises event for updating group UI is Group operation is done by different client.
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                int clientUserId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (e.RoutingKey == "_GroupOperationResponse")
                {
                    int userId = Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["UserId"].ToString());
                    if (GroupOperationResponse != null && userId != clientUserId)
                        GroupOperationResponse(this, new GroupOperationsEventArgs());
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



        #endregion

        #region singleton
        private static GroupDataHandler _groupDataHandler;
        private static object _groupDataHandlerLockerObject = new object();

        public static GroupDataHandler GetInstance()
        {
            try
            {
                lock (_groupDataHandlerLockerObject)
                {
                    if (_groupDataHandler == null)
                        _groupDataHandler = new GroupDataHandler();
                    return _groupDataHandler;
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

        private GroupDataHandler()
        {
            InitializeAmqp();
        }
        #endregion


        /// <summary>
        /// Publishes Group saved event to notification manager and other clients.
        /// </summary>
        internal void PublishSavedSettings()
        {
            try
            {
                Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                savedMessage.Add("ApplicationStatus", "Saved");
                AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
                savedMessage.Add("UserId", CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString());
                AmqpHelper.SendObject(savedMessage, _groupCacheKey, "GroupOperationRequest");
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
        /// Returns list of groups from DB
        /// </summary>
        /// <returns></returns>
        internal List<GroupBase> GetGroupList()
        {
            try
            {
                DataSet ds = new DataSet();
                List<GroupBase> groupList = new List<GroupBase>();

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
                    group.Notification.SendInOneEmail = Convert.ToBoolean(dr["SendInOneEmail"].ToString());
                    group.Notification.TimeSlots[0] = (Convert.ToDateTime(dr["Slot1"].ToString()));
                    group.Notification.TimeSlots[1] = (Convert.ToDateTime(dr["Slot2"].ToString()));
                    group.Notification.TimeSlots[2] = (Convert.ToDateTime(dr["Slot3"].ToString()));
                    group.Notification.TimeSlots[3] = (Convert.ToDateTime(dr["Slot4"].ToString()));
                    group.Notification.TimeSlots[4] = (Convert.ToDateTime(dr["Slot5"].ToString()));
                    groupList.Add(group);

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
        /// Returns cache of rule id and group id
        /// key rule id value group id
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> GetGroupIdForRules()
        {
            try
            {
                DataSet ds = new DataSet();
                Dictionary<string, string> groupRuleCache = new Dictionary<string, string>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetGroupIdForRules";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String groupId = dr["GroupId"].ToString();
                    if (groupRuleCache.ContainsKey(dr["RuleId"].ToString()))
                    {
                        groupRuleCache[dr["RuleId"].ToString()] = groupId;
                    }
                    else
                    {
                        groupRuleCache.Add(dr["RuleId"].ToString(), groupId);
                    }
                }
                return groupRuleCache;
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
        /// Update group id in rules
        /// </summary>
        /// <param name="groupIdDict"></param>
        internal void AddUpdateGroupIdInRules(Dictionary<string, string> groupIdDict)
        {
            try
            {
                String procedureName = "P_CA_UpdateGroupId";

                foreach (string ruleId in groupIdDict.Keys)
                {
                    object[] parameters = new object[2];
                    parameters[0] = ruleId;
                    parameters[1] = groupIdDict[ruleId];

                    DatabaseManager.DatabaseManager.ExecuteNonQuery(procedureName, parameters);

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
            //Dictionary<String, String> savedMessage = new Dictionary<string, string>();
            //savedMessage.Add("ApplicationStatus", "Saved");
            //AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
        }

        /// <summary>
        /// Add Update groups in group table
        /// </summary>
        /// <param name="list"></param>
        internal void AddUpdateGroup(List<GroupBase> list)
        {
            try
            {
                String procedureName = "P_CA_SaveGroupNotifySettings";

                foreach (GroupBase group in list)
                {
                    object[] parameters = new object[18];
                    parameters[0] = group.GroupId;
                    parameters[1] = group.GroupName;
                    parameters[2] = group.Notification.PopUpEnabled;
                    parameters[3] = group.Notification.EmailEnabled;
                    parameters[4] = group.Notification.EmailCCList;
                    parameters[5] = group.Notification.EmailToList;
                    parameters[6] = Convert.ToInt32(group.Notification.LimitFrequencyMinutes);
                    parameters[7] = group.Notification.AlertInTimeRange;
                    parameters[8] = group.Notification.StopAlertOnHolidays;
                    parameters[9] = group.Notification.StartTime;
                    parameters[10] = group.Notification.EndTime;
                    parameters[11] = group.Notification.SendInOneEmail;
                    parameters[12] = group.Notification.TimeSlots[0];
                    parameters[13] = group.Notification.TimeSlots[1];
                    parameters[14] = group.Notification.TimeSlots[2];
                    parameters[15] = group.Notification.TimeSlots[3];
                    parameters[16] = group.Notification.TimeSlots[4];
                    parameters[17] = group.Notification.EmailSubject;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(procedureName, parameters);

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
            //Dictionary<String, String> savedMessage = new Dictionary<string, string>();
            //savedMessage.Add("ApplicationStatus", "Saved");
            //AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
        }

        /// <summary>
        /// Deletes group from group table.
        /// </summary>
        /// <param name="deletedList"></param>
        internal void DeleteGroup(List<string> deletedList)
        {
            try
            {
                foreach (String groupId in deletedList)
                {

                    String procedureName = "P_CA_DeleteGroup";
                    //DataTable dtList = dsRule.Tables[0];

                    //StringBuilder sbTemp = new StringBuilder();

                    object[] parameters = new object[1];
                    parameters[0] = groupId;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(procedureName, parameters);

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
        /// Returns cache of rule id and group id.
        /// </summary>
        /// <param name="updationFromDifferentClient"></param>
        /// <returns></returns>
        internal Dictionary<string, string> GetGroupIdForRules(List<RuleBase> updationFromDifferentClient)
        {
            try
            {
                DataSet ds = new DataSet();
                Dictionary<string, string> groupRuleCache = new Dictionary<string, string>();
                foreach (RuleBase rule in updationFromDifferentClient)
                {
                    if (!groupRuleCache.ContainsKey(rule.RuleId))
                    {
                        groupRuleCache.Add(rule.RuleId, String.Empty);
                    }
                }

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetGroupIdForAllRules";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String groupId = dr["GroupId"].ToString();
                    if (groupRuleCache.ContainsKey(dr["RuleId"].ToString()))
                    {
                        groupRuleCache[dr["RuleId"].ToString()] = groupId;
                    }
                    //else
                    //{
                    //    groupRuleCache.Add(dr["RuleId"].ToString(), groupId);
                    //}
                }
                return groupRuleCache;
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
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
