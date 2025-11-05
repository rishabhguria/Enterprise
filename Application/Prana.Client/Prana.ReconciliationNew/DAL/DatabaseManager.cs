using Prana.BusinessLogic;
using Prana.DatabaseManager;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Data;
namespace Prana.ReconciliationNew
{
    public partial class DatabaseManager : IDisposable
    {
        private static DatabaseManager _instance = null;
        static object _locker = new object();
        /// <summary>
        /// modified by: sachin mishra,10 Feb 2015 purpose: Add try catch block
        /// </summary>
        /// <returns></returns>
        public static DatabaseManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseManager();

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _instance;
        }

        ProxyBase<IPranaPositionServices> _pranaPositionServices = null;
        //Here column name can be an enum which will contain most commonly used column names. e.g. AccountId, AssetId
        //Need to pass custom parametrs of SP
        /// <summary>
        /// Input: spname and filter parameters
        /// Output: Dataset
        /// </summary>
        /// <param name="SpName"></param>
        /// <param name="filterParameters"></param>
        /// <returns></returns>
        //private DataSet FetchPositions(string SpName, Dictionary<Prana.ReconciliationNew.Enums.ColumnName, List<string>> filterParameters)
        //{                       
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        #region commented
        //        //object[] arguments = arg as object[];
        //        //string strReconType = arguments[0].ToString();
        //        //string commaSeparatedAssetIDs = arguments[1].ToString();
        //        //string commaSeparatedAccountIDs = arguments[2].ToString();
        //        //SpName = ReconPrefManager.ReconPreferences.GetTemplates(cmbbxReconTemplates.Text).SpName;
        //        //if sp name is not empty
        //        #endregion
        //        string customFilter = string.Empty;
        //        //foreach (KeyValuePair<Prana.ReconciliationNew.Enums.ColumnName, List<string>> item in filterParameters)
        //        //{
        //        //    GenerateQueryForFilters(item.Key,item.Value,ref customFilter);
        //        //}
        //        if (!SpName.Equals(string.Empty))
        //        {
        //            //ds = _pranaPositionServices.InnerChannel.FetchDataForGivenSpName(dtFromDatePicker.DateTime, dtDatePicker.DateTime, SpName, commaSeparatedAssetIDs.ToString(), commaSeparatedAccountIDs.ToString(), (ReconType)Enum.Parse(typeof(ReconType), strReconType));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return ds;
        //}
        //private void GenerateQueryForFilters(Prana.ReconciliationNew.Enums.ColumnName columnName, List<string> condition, ref string customFilter)
        //{
        //    try
        //    {
        //        //customFilter += "AND" + customFilter +"IN("+condition+")";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}


        private void CreatePositionServicesProxy()
        {
            try
            {
                _pranaPositionServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");

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
        /// Fetch details for Recon FTP file Download
        /// </summary>
        /// <param name="formatName"></param>
        /// <returns>Return Dataset</returns>
        public static DataSet GetThirdPartyFileSettingDetails()
        {
            DataSet dsImportedFileDetails = new DataSet();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetThirdPartyReconFileSettingDetails";

                //Need to write a new SP, following SP does not exist,
                //We need to write this
                dsImportedFileDetails = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return dsImportedFileDetails;
        }


        private static int _errorNumber = 0;
        private static string _errorMessage = string.Empty;
        //Saving in to DB
        internal static void SaveTaxlotWorkFlowStates(DataTable workflowStatDT)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(workflowStatDT);
                String dataAsXML = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveTaxlotWorkFlowStates";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = dataAsXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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

        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pranaPositionServices.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Created By: Pranay Deep, 20 Oct 2015
        /// Save Recon prefrences Table in the database table T_ReconPreferences 
        /// </summary>
        /// <param name="PrefrenceTable"></param>

        private static int _errorNumbers = 0;
        private static string _errorMessages = string.Empty;
        internal static void SaveReconPreferencesInDB(string xml)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveReconPreferences";
                queryData.CommandTimeout = 300;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessages, ref _errorNumbers, queryData.DictionaryDatabaseParameter);
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
        /// Created By: Pranay Deep, 04 Sep 2015
        /// get grouping critera from database
        /// </summary>
        public DataSet GetReconGroupingCriteria()
        {
            DataSet dsImportedFileDetails = new DataSet();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_ReconGroupingCriteria";

            try
            {
                //Need to write a new SP, following SP does not exist,
                //We need to write this
                dsImportedFileDetails = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return dsImportedFileDetails;
        }

        /// <summary>
        /// Created By: Pranay Deep, 16 Oct 2015
        /// Get Recon prefrences Table from the database table T_ReconPreferences
        /// </summary>
        /// <returns></returns>
        public DataSet GetReconPrefrencesFromDB()
        {
            DataSet dsImportedFileDetails = new DataSet();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetReconPreferences";

            try
            {
                //Need to write a new SP, following SP does not exist,
                //We need to write this
                dsImportedFileDetails = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return dsImportedFileDetails;
        }
    }
}

