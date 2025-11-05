using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Prana.ComplianceEngine.RuleDefinition.DAL
{
    internal class CommonRuleHandler
    {
        private static CommonRuleHandler _commonRuleHandler;
        private static object _commonRuleHandlerLockerObject = new object();

        //static String connetionString = ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString;
        //static SqlConnection sqlCnn = new SqlConnection(connetionString);
        String _notificationExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_NotificationExchange);

        public static CommonRuleHandler GetInstance()
        {
            try
            {
                lock (_commonRuleHandlerLockerObject)
                {
                    if (_commonRuleHandler == null)
                        _commonRuleHandler = new CommonRuleHandler();
                    return _commonRuleHandler;
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

        internal void DisposeInstance()
        {
            try
            {
                lock (_commonRuleHandlerLockerObject)
                {
                    _commonRuleHandler = null;
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
        /// Initialize Sender which sends data to EXPNL when notification is updated.
        /// </summary>
        private CommonRuleHandler()
        {
            try
            {
                AmqpHelper.InitializeSender("RuleSaveSender", _notificationExchange, MediaType.Exchange_Direct);
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
        /// Load Alert frequency for drop down menu.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllNotificationFrequency()
        {
            /*string sql = @"select ID,MeasurementDescription  from T_CA_NotifyFrequency";
            SqlCommand sqlCmd = new SqlCommand(sql, sqlCnn); ;
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = sqlCmd;
            try
            {
                sqlCnn.Open();
                adapter.Fill(ds);
                adapter.Dispose();
                sqlCmd.Dispose();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.

                
            }
            finally
            {
                sqlCnn.Close();
            }*/
            Dictionary<int, String> notificationFrequency = new Dictionary<int, string>();
            try
            {
                DataSet ds = new DataSet();
                //Dictionary<String, NotificationSetting> notificationSetting = new Dictionary<string, NotificationSetting>();
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetNotifyFrequency";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int key = Convert.ToInt32(dr["ID"].ToString());
                    if (!notificationFrequency.ContainsKey(key))
                        notificationFrequency.Add(key, dr["MeasurementDescription"].ToString());
                    else
                        notificationFrequency[key] = dr["MeasurementDescription"].ToString();

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
            return notificationFrequency;
        }


        /// <summary>
        /// Save and update Rule in Db. If new then inserted if already exists then update rule in DB
        /// </summary>
        /// <param name="ruleList"></param>
        internal void SaveUpdateRule(List<RuleBase> ruleList)
        {

            try
            {
                String procedureName = "P_CA_SaveNotifySettings";
                //DataTable dtList = dsRule.Tables[0];

                //StringBuilder sbTemp = new StringBuilder();
                foreach (RuleBase rule in ruleList)
                {
                    object[] parameters = new object[21];
                    parameters[0] = rule.RuleId;
                    parameters[1] = rule.RuleName;
                    parameters[2] = rule.RuleId;
                    parameters[3] = rule.Package;
                    parameters[4] = rule.Notification.PopUpEnabled;
                    parameters[5] = rule.Notification.EmailEnabled;
                    parameters[6] = rule.Notification.EmailCCList;
                    parameters[7] = rule.Notification.EmailToList;
                    parameters[8] = Convert.ToInt32(rule.Notification.LimitFrequencyMinutes);
                    parameters[9] = rule.Notification.AlertInTimeRange;
                    parameters[10] = rule.Notification.StopAlertOnHolidays;
                    parameters[11] = rule.Notification.StartTime;
                    parameters[12] = rule.Notification.EndTime;
                    parameters[13] = rule.Notification.SendInOneEmail;
                    parameters[14] = rule.Notification.TimeSlots[0];
                    parameters[15] = rule.Notification.TimeSlots[1];
                    parameters[16] = rule.Notification.TimeSlots[2];
                    parameters[17] = rule.Notification.TimeSlots[3];
                    parameters[18] = rule.Notification.TimeSlots[4];
                    parameters[19] = rule.GroupId;
                    parameters[20] = rule.Notification.EmailSubject;


                    //parameters[8] = 1;
                    //parameters[9] = false;
                    //parameters[10] = false;
                    //parameters[11] = "";
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(procedureName, parameters);

                }
                Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                savedMessage.Add("ApplicationStatus", "Saved");
                AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
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
        /// Gets notification for all rules from DB. 
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, NotificationSetting> GetNotificationSettings()
        {
            try
            {
                DataSet ds = new DataSet();
                Dictionary<String, NotificationSetting> notificationSetting = new Dictionary<string, NotificationSetting>();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetAlertNotificationSettings";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NotificationSetting notification = new NotificationSetting();
                    notification.PopUpEnabled = Convert.ToBoolean(dr["PopUpEnabled"].ToString());
                    notification.LimitFrequencyMinutes = Convert.ToInt32(dr["LimitFrequencyMinutes"].ToString());
                    notification.EmailToList = dr["EmailToList"].ToString();
                    notification.EmailCCList = dr["EmailCCList"].ToString();
                    notification.EmailSubject = dr["EmailSubject"].ToString();
                    notification.EmailEnabled = Convert.ToBoolean(dr["EmailEnabled"].ToString());

                    notification.AlertInTimeRange = Convert.ToBoolean(dr["AlertInTimeRange"].ToString());
                    notification.StopAlertOnHolidays = Convert.ToBoolean(dr["StopAlertOnHolidays"].ToString());
                    notification.StartTime = Convert.ToDateTime(dr["StartTime"].ToString());
                    notification.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                    notification.SendInOneEmail = Convert.ToBoolean(dr["SendInOneEmail"].ToString());
                    notification.TimeSlots[0] = (Convert.ToDateTime(dr["Slot1"].ToString()));
                    notification.TimeSlots[1] = (Convert.ToDateTime(dr["Slot2"].ToString()));
                    notification.TimeSlots[2] = (Convert.ToDateTime(dr["Slot3"].ToString()));
                    notification.TimeSlots[3] = (Convert.ToDateTime(dr["Slot4"].ToString()));
                    notification.TimeSlots[4] = (Convert.ToDateTime(dr["Slot5"].ToString()));
                    //notification.EmailCCList = dr["EmailCCList"].ToString();                   
                    if (notificationSetting.ContainsKey(dr["RuleId"].ToString()))
                    {
                        notificationSetting[dr["RuleId"].ToString()] = notification;
                    }
                    else
                    {
                        notificationSetting.Add(dr["RuleId"].ToString(), notification);
                    }
                }
                return notificationSetting;
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
        /// Delete rule from DB and Sends data to EXPNL to update its notification cache.
        /// </summary>
        /// <param name="list"></param>
        internal void DeleteRule(List<RuleBase> list)
        {
            try
            {
                foreach (RuleBase rule in list)
                {

                    String procedureName = "P_CA_DeleteRule";
                    //DataTable dtList = dsRule.Tables[0];

                    //StringBuilder sbTemp = new StringBuilder();

                    object[] parameters = new object[1];
                    parameters[0] = rule.RuleId;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(procedureName, parameters);

                }
                #region
                /* string sql = @"delete from T_CA_RulesUserDefined where RuleId= '" + rule.RuleId + "'";

                    SqlCommand sqlCmd = new SqlCommand(sql, sqlCnn);
                    try
                    {
                        sqlCnn.Open();
                        int result = sqlCmd.ExecuteNonQuery();
                        sqlCmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        sqlCnn.Close();
                    }*/
                #endregion
                Dictionary<String, String> savedMessage = new Dictionary<string, string>();
                savedMessage.Add("ApplicationStatus", "Saved");
                AmqpHelper.SendObject(savedMessage, "RuleSaveSender", "RuleNoficationSettingsSaved");
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
        /// Exoprt rule metadata to import export path
        /// </summary>
        /// <param name="ruleList"></param>
        /// <param name="importExportPath"></param>
        internal void ExportRuleDef(List<RuleBase> ruleList, String importExportPath)
        {
            try
            {
                foreach (RuleBase rule in ruleList)
                {
                    String filePath = importExportPath + "\\" + rule.Package.ToString() + "\\" + rule.RuleName + "." + rule.Category.ToString();

                    XmlDocument myxml = new XmlDocument();

                    XmlElement ruleDef_tag = myxml.CreateElement("RuleDef");

                    XmlElement ruleName_tag = myxml.CreateElement("RuleName");
                    ruleName_tag.InnerText = rule.RuleName;
                    ruleDef_tag.AppendChild(ruleName_tag);

                    XmlElement packageName_tag = myxml.CreateElement("PackageName");
                    packageName_tag.InnerText = rule.Package.ToString();
                    ruleDef_tag.AppendChild(packageName_tag);


                    XmlElement directoryPath_tag = myxml.CreateElement("DirectoryPath");
                    directoryPath_tag.InnerText = filePath;
                    ruleDef_tag.AppendChild(directoryPath_tag);


                    XmlElement ruleType_tag = myxml.CreateElement("RuleCategory");
                    ruleType_tag.InnerText = rule.Category.ToString();
                    ruleDef_tag.AppendChild(ruleType_tag);

                    XmlElement popUpEnabled_tag = myxml.CreateElement("PopUpEnabled");
                    popUpEnabled_tag.InnerText = rule.Notification.PopUpEnabled.ToString();
                    ruleDef_tag.AppendChild(popUpEnabled_tag);

                    XmlElement emailEnabled_tag = myxml.CreateElement("EmailEnabled");
                    emailEnabled_tag.InnerText = rule.Notification.EmailEnabled.ToString();
                    ruleDef_tag.AppendChild(emailEnabled_tag);

                    XmlElement emailList_tag = myxml.CreateElement("EmailList");
                    emailList_tag.InnerText = rule.Notification.EmailToList;
                    ruleDef_tag.AppendChild(emailList_tag);

                    XmlElement emailCCList_tag = myxml.CreateElement("EmailCCList");
                    emailCCList_tag.InnerText = rule.Notification.EmailCCList;
                    ruleDef_tag.AppendChild(emailCCList_tag);

                    XmlElement limitFrequencyMinutes_tag = myxml.CreateElement("LimitFrequencyMinutes");
                    limitFrequencyMinutes_tag.InnerText = rule.Notification.LimitFrequencyMinutes.ToString();
                    ruleDef_tag.AppendChild(limitFrequencyMinutes_tag);

                    XmlElement alertMarket_tag = myxml.CreateElement("AlertInTimeRange");
                    alertMarket_tag.InnerText = rule.Notification.AlertInTimeRange.ToString();
                    ruleDef_tag.AppendChild(alertMarket_tag);

                    XmlElement alertHoliday_tag = myxml.CreateElement("StopAlertOnHolidays");
                    alertHoliday_tag.InnerText = rule.Notification.StopAlertOnHolidays.ToString();
                    ruleDef_tag.AppendChild(alertHoliday_tag);

                    XmlElement alertStartTime_tag = myxml.CreateElement("StartTime");
                    alertStartTime_tag.InnerText = rule.Notification.StartTime.ToString();
                    ruleDef_tag.AppendChild(alertStartTime_tag);

                    XmlElement alertEndTime_tag = myxml.CreateElement("EndTime");
                    alertEndTime_tag.InnerText = rule.Notification.EndTime.ToString();
                    ruleDef_tag.AppendChild(alertEndTime_tag);

                    XmlElement allInOneLot_tag = myxml.CreateElement("SendInOneEmail");
                    allInOneLot_tag.InnerText = rule.Notification.SendInOneEmail.ToString();
                    ruleDef_tag.AppendChild(allInOneLot_tag);

                    XmlElement slot1_tag = myxml.CreateElement("Slot1");
                    slot1_tag.InnerText = rule.Notification.TimeSlots[0].ToString();
                    ruleDef_tag.AppendChild(slot1_tag);

                    XmlElement slot2_tag = myxml.CreateElement("Slot2");
                    slot2_tag.InnerText = rule.Notification.TimeSlots[1].ToString();
                    ruleDef_tag.AppendChild(slot2_tag);

                    XmlElement slot3_tag = myxml.CreateElement("Slot3");
                    slot3_tag.InnerText = rule.Notification.TimeSlots[2].ToString();
                    ruleDef_tag.AppendChild(slot3_tag);

                    XmlElement slot4_tag = myxml.CreateElement("Slot4");
                    slot4_tag.InnerText = rule.Notification.TimeSlots[3].ToString();
                    ruleDef_tag.AppendChild(slot4_tag);

                    XmlElement slot5_tag = myxml.CreateElement("Slot5");
                    slot5_tag.InnerText = rule.Notification.TimeSlots[4].ToString();
                    ruleDef_tag.AppendChild(slot5_tag);

                    XmlElement alertClient_tag = myxml.CreateElement("ClientName");
                    alertClient_tag.InnerText = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyName;
                    ruleDef_tag.AppendChild(alertClient_tag);

                    myxml.AppendChild(ruleDef_tag);
                    myxml.Save(filePath + "\\MetaData.xml");

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
        /// Rename rule name in alerts after rename rule operation.
        /// </summary>
        /// <param name="oldRuleId"></param>
        /// <param name="ruleList"></param>
        internal void RenameRuleInAlerts(string oldRuleId, List<RuleBase> ruleList)
        {
            try
            {
                String procedureName = "P_CA_UpdateRenamedRuleAlerts";

                foreach (RuleBase rule in ruleList)
                {
                    object[] parameters = new object[2];
                    parameters[0] = oldRuleId;
                    parameters[1] = rule.RuleId;

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
        /// Returns setting from DB for all rules that are updated from other user.
        /// </summary>
        /// <param name="updationFromDifferentClient"></param>
        /// <returns></returns>
        internal Dictionary<string, NotificationSetting> GetNotificationSettings(List<RuleBase> updationFromDifferentClient)
        {

            try
            {
                Dictionary<String, NotificationSetting> notificationList = new Dictionary<string, NotificationSetting>();
                Dictionary<String, NotificationSetting> temp = GetNotificationSettings();

                foreach (RuleBase rule in updationFromDifferentClient)
                {
                    if (temp.ContainsKey(rule.RuleId))
                        notificationList.Add(rule.RuleId, temp[rule.RuleId]);
                }

                return notificationList;
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

