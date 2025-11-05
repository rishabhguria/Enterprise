using Prana.BusinessLogic;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.Dashboard
{
    class DashboardDataManager
    {
        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;

        /// <summary>
        /// get account wise dashboard/Work flow data 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="xmlAccounts"></param>
        /// <param name="isSearchByFileExecutionDate"></param>
        /// <returns></returns>
        internal static DataSet GetDashboradDatafromDB(DateTime fromDate, String xmlAccounts, bool isSearchByFileExecutionDate)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFundWorkflowData";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fundIDs",
                    ParameterType = DbType.String,
                    ParameterValue = xmlAccounts
                });
                queryData.DictionaryDatabaseParameter.Add("@isSearchByFileExecutionDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@isSearchByFileExecutionDate",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isSearchByFileExecutionDate
                });

                DataSet data = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return data;

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

        /// <summary>
        /// Save dashboard/Work flow data 
        /// </summary>
        /// <param name="dataAsXML"></param>
        /// <returns></returns>
        internal static int SaveDashboradDataInDB(String dataAsXML)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveWorkflowStats";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = dataAsXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
            return 1;
        }

        /// <summary>
        /// Get accounts batch mapping 
        /// </summary>
        /// <returns></returns>
        internal static ConcurrentDictionary<string, List<int>> GetAccountBatchMapping()
        {
            ConcurrentDictionary<string, List<int>> accountBatchMapping = new ConcurrentDictionary<string, List<int>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetFundWiseBatchData";
                queryData.CommandTimeout = 900;
                DataSet data = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                Parallel.ForEach(data.Tables[0].AsEnumerable(), dr =>
                {
                    int accountId = dr["CompanyFundID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CompanyFundID"].ToString());
                    String formatName = dr["FormatName"] == DBNull.Value ? String.Empty : dr["FormatName"].ToString();
                    if (accountId > 0 && !string.IsNullOrWhiteSpace(formatName))
                    {
                        if (!accountBatchMapping.ContainsKey(formatName))
                        {
                            accountBatchMapping.TryAdd(formatName, new List<int> { accountId });
                        }
                        else
                        {
                            accountBatchMapping[formatName].Add(accountId);
                        }
                    }
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
            return accountBatchMapping;

        }
    }
}
