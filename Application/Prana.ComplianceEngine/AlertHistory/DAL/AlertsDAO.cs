using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ComplianceEngine.AlertHistory.DAL
{
    internal class AlertsDAO
    {
        private static AlertsDAO _alertHistory;
        private static Object _lockerObject = new Object();

        internal static AlertsDAO GetInstance()
        {
            lock (_lockerObject)
            {
                if (_alertHistory == null)
                    _alertHistory = new AlertsDAO();
            }
            return _alertHistory;
        }

        /// <summary>
        /// Getting AlertHistory Data From DB in Date Range
        /// </summary>
        /// <param name="from">From Date</param> 
        /// <param name="to"> To Date</param>
        /// <param name="pageSize"> For No. of Pages</param>
        /// <param name="pageNum">For Page No.</param> 
        /// <param name="totalRows">For Total No. of Rows</param> 
        /// <returns></returns>
        internal DataSet GetComplianceAlertHist(DateTime from, DateTime to, int pageSize, int pageNum, string sortedColumnName, string filteredColumns, out int totalRows)
        {
            try
            {
                DataSet ds = new DataSet();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetAlertHistory";
                queryData.DictionaryDatabaseParameter.Add("@totalRows", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@totalRows",
                    ParameterType = DbType.Int32,
                    ParameterValue = 0
                });
                queryData.DictionaryDatabaseParameter.Add("@dateFrom", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@dateFrom",
                    ParameterType = DbType.DateTime,
                    ParameterValue = from
                });
                queryData.DictionaryDatabaseParameter.Add("@dateTo", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@dateTo",
                    ParameterType = DbType.DateTime,
                    ParameterValue = to
                });
                queryData.DictionaryDatabaseParameter.Add("@pageSize", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@pageSize",
                    ParameterType = DbType.Int32,
                    ParameterValue = pageSize
                });
                queryData.DictionaryDatabaseParameter.Add("@pageNO", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@pageNO",
                    ParameterType = DbType.Int32,
                    ParameterValue = pageNum
                });
                queryData.DictionaryDatabaseParameter.Add("@sortedColumnName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@sortedColumnName",
                    ParameterType = DbType.String,
                    ParameterValue = sortedColumnName
                });
                queryData.DictionaryDatabaseParameter.Add("@filteredColumns", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@filteredColumns",
                    ParameterType = DbType.String,
                    ParameterValue = filteredColumns
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                totalRows = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@totalRows"].ParameterValue);
                return ds;
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
        /// Add Alert in date range from T_CA_AlertHistory to T_CA_AlertHistoryBackup and delete from it
        /// returns num of rows affected.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        internal int ArchiveAlertHistory(List<String> keys, int action)
        {
            try
            {
                String csv = String.Join(",", keys);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_ArchiveDeleteAlerts";
                queryData.DictionaryDatabaseParameter.Add("@NoOfRows", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@NoOfRows",
                    ParameterType = DbType.Int32,
                    ParameterValue = 0,
                    OutParameterSize = sizeof(Int32)
                });
                queryData.DictionaryDatabaseParameter.Add("@Alerts", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Alerts",
                    ParameterType = DbType.String,
                    ParameterValue = csv
                });
                queryData.DictionaryDatabaseParameter.Add("@DeletOrArchive", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@DeletOrArchive",
                    ParameterType = DbType.Int32,
                    ParameterValue = action
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                return Convert.ToInt32(queryData.DictionaryDatabaseParameter["@NoOfRows"].ParameterValue);
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
    }
}
