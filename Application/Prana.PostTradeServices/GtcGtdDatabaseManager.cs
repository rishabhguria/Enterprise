using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.PostTradeServices
{
    public class GtcGtdDatabaseManager
    {
        /// <summary>
        /// Gets notify time for email notifications.
        /// </summary>
        /// <returns></returns>
        public DateTime GetEmailNotifyTime()
        {
            DateTime dtEmailNotify = DateTime.MinValue;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "GetNotifyActiveGtcGtdOrdersByCompanyId";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyId",
                    ParameterType = DbType.Int32,
                    ParameterValue = CachedDataManager.GetInstance.GetCompanyID()
                });
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row.Count() > 2 && row[1] != DBNull.Value && row[2] != DBNull.Value)
                        {
                            bool isNotifyActiveGtcGtdOrders = Convert.ToBoolean(Convert.ToInt32(row[1].ToString()));
                            if (isNotifyActiveGtcGtdOrders)
                                dtEmailNotify = DateTime.Parse(row[2].ToString());
                        }
                    }
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
            return dtEmailNotify;
        }

        /// <summary>
        /// Gets email details for each company user.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,string> GetCompanyUserEmailIds()
        {
            Dictionary<int,string> companyUserEmailIds = new Dictionary < int,string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetCompanyUserEmailIds";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyId",
                    ParameterType = DbType.Int32,
                    ParameterValue = CachedDataManager.GetInstance.GetCompanyID()
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int userId = int.Parse(row[0].ToString(), System.Globalization.NumberStyles.Integer);
                        string emailId = row[1].ToString();
                        if (!companyUserEmailIds.ContainsKey(userId))
                            companyUserEmailIds.Add(userId, emailId);
                    }
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
            return companyUserEmailIds;
        }

        #region Singleton
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static GtcGtdDatabaseManager GetInstance()
        {
            lock (_locker)
            {
                if (_gtcGtdDatabaseManager == null)
                    _gtcGtdDatabaseManager = new GtcGtdDatabaseManager();
                return _gtcGtdDatabaseManager;
            }
        }
        private GtcGtdDatabaseManager()
        { 
        }
        private static GtcGtdDatabaseManager _gtcGtdDatabaseManager = null;
        readonly static object _locker = new object();
        #endregion
    }
}
