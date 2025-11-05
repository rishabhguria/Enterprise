using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.ComplianceEngine.AlertHistory.DAL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.ComplianceEngine.AlertHistory.BLL
{
    internal class AlertHistoryManager
    {
        private static AlertHistoryManager _alertHistoryManager;
        private static Object _lockerObject = new Object();

        internal event UpdateAlertGrid UpdateAlertGridEvent;

        /// <summary>
        /// Default constructor 
        /// Initializes Amqp
        /// and Bind events.
        /// UpdateNewAlertEvent raised from AlertsAmqpPlugin when new alert is triggered.
        /// </summary>
        private AlertHistoryManager()
        {
            try
            {
                AlertsAmqpPlugin.GetInstance().UpdateNewAlertEvent += AlertHistoryManager_UpdateAlertGridEvent;
                AlertsAmqpPlugin.GetInstance().InitializeAmqp();
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
        /// UpdateNewAlertEvent raised from AlertsAmqpPlugin when new alert is triggered.
        /// raises UpdateAlertGridEvent which is Binded with UI(User control).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">Alert DataSet, And Update Operation</param>
        void AlertHistoryManager_UpdateAlertGridEvent(object sender, UpdateAlertGridEventArgs args)
        {
            try
            {
                if (UpdateAlertGridEvent != null)
                    UpdateAlertGridEvent(this, args);
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
        /// Singleton Instance
        /// </summary>
        /// <returns></returns>
        public static AlertHistoryManager GetInstance()
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_alertHistoryManager == null)
                        _alertHistoryManager = new AlertHistoryManager();
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
            return _alertHistoryManager;
        }

        /// <summary>
        /// Get alerts from DB in the date range  
        /// </summary>
        /// <param name="dateFrom">From Date</param> 
        /// <param name="dateTo">To Date</param> 
        /// /// <param name="pageNum">For Page No.</param> 
        /// <param name="pageSize">For No. of Pages</param> 
        /// <param name="totalRows">For Total No. of Rows</param> 
        /// <returns></returns>
        internal DataSet GetAlertHistory(DateTime dateFrom, DateTime dateTo, int pageNo, int pageSize, string sortedColumnName, string filteredColumns, out int totalRows)
        {
            try
            {
                return AlertsDAO.GetInstance().GetComplianceAlertHist(dateFrom, dateTo, pageSize, pageNo, sortedColumnName, filteredColumns, out totalRows);
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
                totalRows = 0;
                return null;
            }
        }

        /// <summary>
        /// Archive alerts in date range from T_CA_AlertHistory to T_CA_AlertHistoryBackup
        /// returns num of rows affected.
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        internal int ArchiveAlerts(List<String> keys, int action)
        {
            try
            {
                int rows = AlertsDAO.GetInstance().ArchiveAlertHistory(keys, action);
                AlertsAmqpPlugin.GetInstance().PublishArchiveAlert();
                return rows;
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
            return -1;
        }

        /// <summary>
        /// Make the Where condition according to filter Collection which applied by the user
        /// </summary>
        /// <param name="filterValues"></param>
        /// <returns></returns>
        public string MakeQuery(Dictionary<string, string> filterValues)
        {
            try
            {
                string filterString = String.Empty;
                foreach (KeyValuePair<string, string> filterData in filterValues)
                {
                    string key = filterData.Key.Replace(" ", String.Empty);
                    string value = filterData.Value;

                    //If Key is "RuleDeleted", then we will send r.IsDeleted, because In DB column name is IsDeleted
                    if (key.Equals("RuleDeleted"))
                    {
                        key = "r.IsDeleted";
                    }
                    //If Key is "Name", then we will send a.RuleID and Value is -1, because we will check condition on Rule Id if rule name is "N/A", otherwise Rule Name
                    if (key.Equals("Name"))
                    {
                        if (value.Equals("N/A"))
                        {
                            key = "a.RuleId";
                            value = "-1";
                        }
                        else
                            key = "r.RuleName";
                    }

                    //If Key is "UserName", then we will send c.FirstName, because In DB column name is FirstName
                    if (key.Equals("UserName"))
                        key = "c.FirstName";
                    if (key.Equals("PreTradeType"))
                        key = "a.PreTradeType";

                    if (key.Equals("Checkbox") && filterValues.Count == 1)
                        filterString = String.Empty;
                    else
                        filterString += key + " = '" + value + "'";

                    if (!filterValues.Keys.Last().Equals(filterData.Key) && !String.IsNullOrEmpty(filterString))
                        filterString += " AND ";
                }
                return filterString;
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
