using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.GreenFieldModels;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Prana.CommonDatabaseAccess
{
    public class KeyValueDataManager : IKeyValueDataManager
    {
        //TODO- most of the functions here are to get data are same,why are we adding new functions 
        //to get data each time?? We can create generic function for them -OMshiv

        //private static KeyValueDataManager _instance = null;
        DataSet _dsAllCashAccountTables;
        DataSet _dsAllCashAccountTablesWithRelation;
        DataSet _dsAllActivityTables;
        private readonly int _heavyGetTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["HeavySaveTimeout"]);
        static Dictionary<int, string> _auecs = new Dictionary<int, string>();
        static object _locker = new object();
        internal static Dictionary<int, Prana.BusinessObjects.TimeZone> auecIDTimeZones = new Dictionary<int, Prana.BusinessObjects.TimeZone>();
        private const string BloombergCountryCode = "BloombergCountryCode";
        private const string CountryId = "CountryID";
        private Prana.BusinessObjects.TimeZone _currentTimeZone;
        public Prana.BusinessObjects.TimeZone CurrentTimeZone
        {
            get
            {
                if (_currentTimeZone == null)
                    _currentTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByString(System.TimeZoneInfo.Local.DaylightName);
                return _currentTimeZone;
            }
            set { _currentTimeZone = value; }
        }
        public KeyValueDataManager()
        {
        }

        //public static KeyValueDataManager GetInstance()
        //{
        //    if (_instance == null)
        //    {
        //        lock (_locker)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new KeyValueDataManager(null);
        //            }
        //        }
        //    }
        //    return _instance;
        //}

        #region Company Datasources lookup
        public Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> GetAllDataSources()
        {

            Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> dataSourceLookup = new Dictionary<int, Prana.BusinessObjects.PositionManagement.ThirdPartyNameID>();
            Prana.BusinessObjects.PositionManagement.ThirdPartyNameID dataSourceNameID = null;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetAllDataSourceForComany";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        dataSourceNameID = new Prana.BusinessObjects.PositionManagement.ThirdPartyNameID();
                        int accountID = Convert.ToInt32(row[0]);

                        dataSourceNameID.ID = Convert.ToInt32(row[1]);
                        dataSourceNameID.FullName = row[2].ToString();
                        dataSourceNameID.ShortName = row[3].ToString();
                        if (!dataSourceLookup.ContainsKey(accountID))
                        {
                            dataSourceLookup.Add(accountID, dataSourceNameID);
                        }
                    }

                }
                return dataSourceLookup;
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
            return dataSourceLookup;
        }
        #endregion Company Datasources lookup

        #region KeyValuePairs

        // modified by Ankit Gupta on 10 Oct, 2014.
        // For CH mode, only those entries are to be fetched from DB, which are currently active.
        // For Prana mode, the concept of active and inactive doesn't exists, therefore, all the entries from the DB are to be fetched.
        // Therefore, using two different stored procedures, for different release types.
        public DataSet GetKeyValuePairs()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetKeyValuePair";

            DataSet keyValuePairs = new DataSet();
            try
            {
                keyValuePairs = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return keyValuePairs;
        }

        //Method Created By: Bharat raturi
        //get the list of accounts by CompanyID
        /// <summary>
        /// Get the dictionary of accounts companyWise
        /// </summary>
        /// <param name="keyValues">Datable holding the details</param>
        /// <returns>The dictionary of account details companywise</returns>
        public Dictionary<int, List<Account>> FillAccountsCompanyWise(DataTable keyValues)
        {
            Dictionary<int, List<Account>> dictCompanyWiseAccounts = new Dictionary<int, List<Account>>();
            try
            {
                foreach (DataRow dr in keyValues.Rows)
                {
                    if (!string.IsNullOrEmpty(dr[3].ToString()))
                    {
                        int companyID = Convert.ToInt32(dr[3]);
                        Account account = new Account();
                        if (!string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            account.AccountID = Convert.ToInt32(dr[0]);
                        }
                        if (!string.IsNullOrEmpty(dr[1].ToString()))
                        {
                            account.Name = Convert.ToString(dr[1]);
                        }
                        if (!string.IsNullOrEmpty(dr[2].ToString()))
                        {
                            account.FullName = Convert.ToString(dr[2]);
                        }
                        if (!string.IsNullOrEmpty(dr[5].ToString()))
                        {
                            account.IsSwapAccount = Convert.ToBoolean(dr[5]);
                        }

                        if (dictCompanyWiseAccounts.ContainsKey(companyID))
                        {
                            dictCompanyWiseAccounts[companyID].Add(account);
                        }
                        else
                        {
                            List<Account> accountList = new List<Account>();
                            accountList.Add(account);
                            dictCompanyWiseAccounts.Add(companyID, accountList);
                        }
                    }
                }
                return dictCompanyWiseAccounts;
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
        /// Added By: Bharat Raturi, 27 may 2014
        /// purpose: Get more frequently used details refreshed in the cache
        /// </summary>
        /// <returns>Dataset holding the details</returns>
        public DataSet GetFrequentlyUsedData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetFrequentlyUsedData";

            DataSet keyValuePairs = new DataSet();
            try
            {
                keyValuePairs = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return keyValuePairs;
        }

        public Dictionary<int, string> FillKeyValuePairs(DataTable keyValues, int offset)
        {
            Dictionary<int, string> keyValue = new Dictionary<int, string>();
            int id = 0;
            int value = 1 + offset;
            try
            {
                foreach (DataRow dr in keyValues.Rows)
                {
                    int rowID = Convert.ToInt32(dr[id]);
                    if (keyValue.ContainsKey(rowID))
                    {
                        keyValue[rowID] = dr[value].ToString();
                    }
                    else
                    {
                        keyValue.Add(rowID, dr[value].ToString());
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
            return keyValue;
        }

        public Dictionary<string, string> FillImportTag(DataTable keyValues, int offset)
        {
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            int id = 0;
            int value = 1 + offset;
            try
            {
                foreach (DataRow dr in keyValues.Rows)
                {
                    string rowID = dr[id].ToString();
                    if (keyValue.ContainsKey(rowID))
                    {
                        keyValue[rowID] = dr[value].ToString();
                    }
                    else
                    {
                        keyValue.Add(rowID, dr[value].ToString());
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
            return keyValue;
        }

        /// <summary>
        /// Fill the Third party-Account mapping
        /// </summary>
        /// <param name="keyValues">datatble to get the data from</param>
        /// <param name="offset">Index of the value</param>
        /// <returns>Dictionary of mapping</returns>
        public Dictionary<int, List<int>> FillThirdPartyAccounts(DataTable keyValues, int offset)
        {
            Dictionary<int, List<int>> keyValue = new Dictionary<int, List<int>>();
            int id = 0;
            int value = 1 + offset;
            try
            {
                foreach (DataRow dr in keyValues.Rows)
                {
                    if (keyValue.ContainsKey(Convert.ToInt32(dr[id])))
                    {
                        if (dr[value] != DBNull.Value)
                        {
                            keyValue[Convert.ToInt32(dr[id])].Add(Convert.ToInt32(dr[value]));
                        }
                        else
                        {
                            keyValue[Convert.ToInt32(dr[id])].Add(-1);
                        }
                    }
                    else
                    {
                        List<int> accountList = new List<int>();
                        if (dr[value] != DBNull.Value)
                        {
                            accountList.Add(Convert.ToInt32(dr[value]));
                        }
                        else
                        {
                            accountList.Add(-1);
                        }
                        keyValue.Add(Convert.ToInt32(dr[id]), accountList);
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
            return keyValue;
        }

        public Dictionary<int, string> FillAUEC(DataTable DtAUECs)
        {
            Dictionary<int, string> dictAUECs = new Dictionary<int, string>();
            int AUECID = 0;
            int AssetID = 1;
            int UnderlyingID = 2;
            int ExchangeID = 3;
            int CurrencyID = 4;
            int CountryISOCode = 6;
            try
            {
                foreach (DataRow dr in DtAUECs.Rows)
                {
                    if (dictAUECs.ContainsKey(Convert.ToInt32(dr[AUECID])))
                    {
                        dictAUECs[Convert.ToInt32(dr[AUECID])] = dr[AssetID].ToString() + "," + dr[UnderlyingID].ToString() + "," + dr[ExchangeID].ToString() + "," + dr[CurrencyID].ToString() + "," + Convert.ToString(dr[CountryISOCode]);
                    }
                    else
                    {
                        dictAUECs.Add(Convert.ToInt32(dr[AUECID]), dr[AssetID].ToString() + "," + dr[UnderlyingID].ToString() + "," + dr[ExchangeID].ToString() + "," + dr[CurrencyID].ToString() + "," + Convert.ToString(dr[CountryISOCode].ToString()));
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
            return dictAUECs;
        }
        #endregion

        public int GetLastPreferencedAccountID()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetLastPreferencedFund";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        if (row.Length > 0)
                        {
                            return Convert.ToInt32(row[0]);

                        }

                    }
                }
            }
            #region Catch
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
            #endregion
            return int.MinValue;

        }

        #region AUEC Multiplier and Identifier and AUEC related info(Asset Exchange and Currency)
        public System.Collections.Generic.Dictionary<int, double> GetAUECMultipliers()
        {
            System.Collections.Generic.Dictionary<int, double> dictionaryAuecMultipliers = new Dictionary<int, double>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetKeyValuePairAUECMultipliers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dictionaryAuecMultipliers.Add(Convert.ToInt32(row[0]), Convert.ToDouble(row[1]));
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryAuecMultipliers;

        }

        /// <summary>
        /// Get AUEC RoundLot Value from DB
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<int, decimal> GetAUECRoundLots()
        {
            System.Collections.Generic.Dictionary<int, decimal> dictionaryAuecRoundLots = new Dictionary<int, decimal>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetKeyValuePairAUECRoundLots";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dictionaryAuecRoundLots.Add(Convert.ToInt32(row[0]), Convert.ToDecimal(row[1]));
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryAuecRoundLots;

        }

        public System.Collections.Generic.Dictionary<string, int> GetExchangeIdentifiers()
        {
            System.Collections.Generic.Dictionary<string, int> dictionaryExchangeIdentifiers = new Dictionary<string, int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetKeyValuePairExchangeIdentifiers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!dictionaryExchangeIdentifiers.ContainsKey(row[1].ToString()))
                            dictionaryExchangeIdentifiers.Add(row[1].ToString(), Convert.ToInt32(row[0]));
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryExchangeIdentifiers;
        }

        public System.Collections.Generic.Dictionary<int, int> GetRoundOffRules()
        {
            System.Collections.Generic.Dictionary<int, int> dictionaryRoundOffs = new Dictionary<int, int>();

            try
            {
                Object[] parameter = new object[1];
                parameter[0] = 0;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetRoundOffRules", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dictionaryRoundOffs.Add(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]));// FillKeyValuePair(row, 0));
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryRoundOffs;

        }

        public System.Collections.Generic.Dictionary<int, int> GetAUECIdToAssetMapping()
        {
            System.Collections.Generic.Dictionary<int, int> dictionaryAUECs = new Dictionary<int, int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetKeyValuePairAUECIdToAssetId";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dictionaryAUECs.Add(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]));// FillKeyValuePair(row, 0));
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryAUECs;

        }

        public Dictionary<int, List<int>> GetCompanyMasterFundSubAccountAssociation(int companyID)
        {
            Dictionary<int, List<int>> dictmasterFundsAssociation = new Dictionary<int, List<int>>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_CompanyMasterFundSubAccountAssociation", new object[] { companyID }))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int key = Convert.ToInt16(row[0]);
                        int accountID = Convert.ToInt16(row[1]);

                        if (dictmasterFundsAssociation.ContainsKey(key))
                        {
                            dictmasterFundsAssociation[key].Add(accountID);
                        }
                        else
                        {
                            List<int> listAccountID = new List<int>();
                            listAccountID.Add(accountID);
                            dictmasterFundsAssociation.Add(key, listAccountID);
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictmasterFundsAssociation;
        }

        /// <summary>  
        /// This method returns the user permitted masterfunds in the form of list.
        /// This is based on if the user has permissions for all the funds associated with those master funds.  
        /// </summary>  
        /// <param name="companyID"></param>  
        /// <param name="userID"></param>  
        /// <returns>
        public List<int> GetUserPermittedMasterFundBasedOnFunds(int companyID, int userID)
        {
            List<int> masterFundIds = new List<int>();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = companyID;
                parameter[1] = userID;

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetPermittedMasterFundsBasedOnFunds", parameter))
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value)
                        {
                            masterFundIds.Add(Convert.ToInt32(reader[0]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                    throw;
            }
            return masterFundIds;
        }

        public Dictionary<int, List<int>> GetCompanyDataSourceSubAccountAssociation()
        {
            Dictionary<int, List<int>> dictDataSourceAccountsAssociation = new Dictionary<int, List<int>>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_CompanyDataSourceSubAccountAssociation";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        int key = Convert.ToInt16(row[0]);
                        int accountID = Convert.ToInt16(row[1]);

                        if (dictDataSourceAccountsAssociation.ContainsKey(key))
                        {
                            dictDataSourceAccountsAssociation[key].Add(accountID);
                        }
                        else
                        {
                            List<int> listAccountID = new List<int>();
                            listAccountID.Add(accountID);
                            dictDataSourceAccountsAssociation.Add(key, listAccountID);
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictDataSourceAccountsAssociation;
        }
        #endregion

        #region Account SubAccount Key Value Pair

        public void ResetAllAccountTable(DataSet dsCashAccountTables, DataSet dsCashAccountTablesWithRelation)
        {
            try
            {
                //dsAllCashAccountTables = _clientsCommonDataManager.GetAllAccountTablesFromDB();
                _dsAllCashAccountTables = dsCashAccountTables;
                //dsAllCashAccountTablesWithRelation = _clientsCommonDataManager.GetAllAccountsWithRelation();
                _dsAllCashAccountTablesWithRelation = dsCashAccountTablesWithRelation;
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

        public void SetAllActivityTables(DataSet dsActivityTables)
        {
            try
            {
                _dsAllActivityTables = dsActivityTables;
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

        public Dictionary<int, string> getSubAccounts()
        {
            Dictionary<int, string> dictionarySubAccounts = new Dictionary<int, string>();
            try
            {
                if (_dsAllCashAccountTables == null)
                    _dsAllCashAccountTables = DataManagerInternalRepository.GetAllAccountTablesFromDB();
                if (_dsAllCashAccountTables != null && _dsAllCashAccountTables.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsAllCashAccountTables.Tables["SubCashAccounts"].Rows)
                    {
                        if (!dictionarySubAccounts.ContainsKey(Convert.ToInt32(dr["SubAccountID"])))
                            dictionarySubAccounts.Add(Convert.ToInt32(dr["SubAccountID"]), dr["Name"].ToString());
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
            return dictionarySubAccounts;
        }

        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6293
        public Dictionary<int, string> getSubAccountsWithMasterCategoryName(bool isStaleData = false)
        {
            Dictionary<int, string> dictionarySubAccounts = new Dictionary<int, string>();
            try
            {
                if (_dsAllCashAccountTables == null || isStaleData)
                    _dsAllCashAccountTables = DataManagerInternalRepository.GetAllAccountTablesFromDB();
                if (_dsAllCashAccountTables != null && _dsAllCashAccountTables.Tables.Count > 0)
                {
                    var subAccounts = (from SubAcc in _dsAllCashAccountTables.Tables["SubCashAccounts"].AsEnumerable()
                                       join SubCat in _dsAllCashAccountTables.Tables["SubCategory"].AsEnumerable()
                                       on SubAcc.Field<int>("SubCategoryID") equals SubCat.Field<int>("SubCategoryID")
                                       join MasCat in _dsAllCashAccountTables.Tables["MasterCategory"].AsEnumerable()
                                       on SubCat.Field<int>("MasterCategoryID") equals MasCat.Field<int>("MasterCategoryID")
                                       select new
                                       {
                                           Id = SubAcc.Field<int>("SubAccountID"),
                                           Name = SubAcc.Field<string>("Name") + " [" + MasCat.Field<string>("MasterCategoryName") + "]"
                                       }).ToList();

                    foreach (var acc in subAccounts)
                    {
                        if (!dictionarySubAccounts.ContainsKey(acc.Id))
                            dictionarySubAccounts.Add(acc.Id, Convert.ToString(acc.Name));
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
            return dictionarySubAccounts;
        }

        public DataSet getSubAccountsForExport()
        {
            DataSet getSubAccounts = new DataSet();
            try
            {
                if (_dsAllCashAccountTables == null)
                    _dsAllCashAccountTables = DataManagerInternalRepository.GetAllAccountTablesFromDB();

                if (_dsAllCashAccountTables != null && _dsAllCashAccountTables.Tables.Count > 0)
                {
                    var subAccounts = (from SubAcc in _dsAllCashAccountTables.Tables["SubCashAccounts"].AsEnumerable()
                                       join SubCat in _dsAllCashAccountTables.Tables["SubCategory"].AsEnumerable()
                                       on SubAcc.Field<int>("SubCategoryID") equals SubCat.Field<int>("SubCategoryID")
                                       join MasCat in _dsAllCashAccountTables.Tables["MasterCategory"].AsEnumerable()
                                       on SubCat.Field<int>("MasterCategoryID") equals MasCat.Field<int>("MasterCategoryID")
                                       join TType in _dsAllCashAccountTables.Tables["TransactionType"].AsEnumerable()
                                       on SubAcc.Field<int>("TransactionTypeID") equals TType.Field<int>("TransactionTypeID")

                                       join SubAccName in _dsAllActivityTables.Tables["SubAccountType"].AsEnumerable()
                                       on SubAcc.Field<int?>("SubAccountTypeId") equals SubAccName.Field<int?>("SubAccountTypeId") into temp

                                       from ex in temp.DefaultIfEmpty()

                                       select new
                                       {
                                           MasterCategory = MasCat.Field<string>("MasterCategoryName"),
                                           SubCategory = SubCat.Field<string>("SubCategoryName"),
                                           Name = SubAcc.Field<string>("Name"),
                                           TTypeId = TType.Field<string>("TransactionType"),
                                           Acronym = SubAcc.Field<string>("Acronym"),
                                           SubAccountType = ex == null ? String.Empty : ex.Field<string>("SubAccountType")
                                       });

                    if (subAccounts != null)
                    {
                        getSubAccounts.Tables.Add();
                        getSubAccounts.Tables[0].Columns.Add("MasterCategory", typeof(string));
                        getSubAccounts.Tables[0].Columns.Add("SubCategory", typeof(string));
                        getSubAccounts.Tables[0].Columns.Add("FundName", typeof(string));
                        getSubAccounts.Tables[0].Columns.Add("TransactionType", typeof(string));
                        getSubAccounts.Tables[0].Columns.Add("Acronym", typeof(string));
                        getSubAccounts.Tables[0].Columns.Add("SubAccountType", typeof(string));

                        foreach (var item in subAccounts)
                        {
                            getSubAccounts.Tables[0].Rows.Add(item.MasterCategory, item.SubCategory, item.Name, item.TTypeId, item.Acronym, item.SubAccountType);
                        }
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
            return getSubAccounts;
        }

        public Dictionary<string, int> getSubAccounts_Acronym()
        {
            Dictionary<string, int> dictionarySubAccounts_Acronym = new Dictionary<string, int>();
            try
            {
                if (_dsAllCashAccountTables == null)
                    _dsAllCashAccountTables = DataManagerInternalRepository.GetAllAccountTablesFromDB();

                if (_dsAllCashAccountTables != null && _dsAllCashAccountTables.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsAllCashAccountTables.Tables["SubCashAccounts"].Rows)
                    {
                        if (!dictionarySubAccounts_Acronym.ContainsKey(dr["Acronym"].ToString()))
                            dictionarySubAccounts_Acronym.Add(dr["Acronym"].ToString(), (int)dr["SubAccountID"]);
                        else
                            throw new Exception("REDUNDANCY IN SubAccountAcronym :--" + dr["Acronym"].ToString());
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
            return dictionarySubAccounts_Acronym;
        }

        public Dictionary<int, Dictionary<string, int>> getSubAccounts_Side_Multipier()
        {
            Dictionary<int, Dictionary<string, int>> dictionarySubAccounts_Side_Multipier = new Dictionary<int, Dictionary<string, int>>();
            Dictionary<string, int> dicAccountSide;
            try
            {
                if (_dsAllCashAccountTablesWithRelation == null)
                {
                    _dsAllCashAccountTablesWithRelation = DataManagerInternalRepository.GetAllAccountsWithRelation(DataManagerInternalRepository.GetAllAccountTablesFromDB());
                }
                if (_dsAllCashAccountTablesWithRelation != null && _dsAllCashAccountTablesWithRelation.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsAllCashAccountTablesWithRelation.Tables["SubCashAccounts"].Rows)
                    {
                        DataRow[] lsSubcategory = dr.GetParentRows("subCategorySubAccounts");
                        DataRow[] lsMasterCategory;
                        DataRow[] lsAccountSide;
                        if (lsSubcategory.Length > 1)
                            throw new Exception("SubAccount :-- '" + dr["Name"].ToString() + "' is Associated with more then one Sub Category");
                        else if (lsSubcategory.Length > 0)
                        {
                            lsMasterCategory = lsSubcategory[0].GetParentRows("masterCategorySubCategory");
                            if (lsMasterCategory.Length > 1)
                                throw new Exception("SubCategory :-- '" + lsSubcategory[0]["SubCategoryName"].ToString() + "' is Associated with more then one Master Category");
                            else if (lsMasterCategory.Length > 0)
                            {
                                lsAccountSide = lsMasterCategory[0].GetChildRows("masterCategoryAccountSide");
                                if (lsAccountSide.Length > 0)
                                {
                                    dicAccountSide = new Dictionary<string, int>();
                                    foreach (DataRow drAccountSide in lsAccountSide)
                                        dicAccountSide.Add(drAccountSide["AccountSide"].ToString(), Convert.ToInt32(drAccountSide["AccountSideMultiplier"]));

                                    if (!dictionarySubAccounts_Side_Multipier.ContainsKey(Convert.ToInt32(dr["SubAccountID"])) && dicAccountSide.Count > 0)
                                    {
                                        dictionarySubAccounts_Side_Multipier.Add(Convert.ToInt32(dr["SubAccountID"]), dicAccountSide);
                                    }
                                    else
                                        throw new Exception("REDUNDANCY IN SubAccountID :--" + dr["SubAccountID"].ToString());
                                }
                            }
                        }
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
            return dictionarySubAccounts_Side_Multipier;
        }

        public Dictionary<int, string> getSubAccounts_AccountType()
        {
            Dictionary<int, string> dictionarygetSubAccounts_AccountType = new Dictionary<int, string>();
            try
            {
                if (_dsAllCashAccountTablesWithRelation == null)
                {
                    _dsAllCashAccountTablesWithRelation = DataManagerInternalRepository.GetAllAccountsWithRelation(DataManagerInternalRepository.GetAllAccountTablesFromDB());
                }
                if (_dsAllCashAccountTablesWithRelation != null && _dsAllCashAccountTablesWithRelation.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsAllCashAccountTablesWithRelation.Tables["SubCashAccounts"].Rows)
                    {
                        DataRow[] accountType = dr.GetChildRows("subAccountsAccountType");
                        if (accountType.Length > 0)
                        {
                            if (!dictionarygetSubAccounts_AccountType.ContainsKey(Convert.ToInt32(dr["SubAccountID"])))
                            {
                                dictionarygetSubAccounts_AccountType.Add(Convert.ToInt32(dr["SubAccountID"]), accountType[0]["TransactionTypeAcronym"].ToString());
                            }
                            else
                                throw new Exception("REDUNDANCY IN SubAccountID :--" + dr["SubAccountID"].ToString());
                        }
                        else
                        {
                            throw new Exception("SubAccount:--" + dr["Name"].ToString() + " doesn't have Account Type");
                        }
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
            return dictionarygetSubAccounts_AccountType;
        }
        #endregion

        #region Flags by AUEC ID
        public System.Collections.Generic.Dictionary<int, byte[]> GetFlagsbyAUECs()
        {
            System.Collections.Generic.Dictionary<int, byte[]> dictionaryFlags = new Dictionary<int, byte[]>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFlagsByAUECID";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[1].ToString() != string.Empty)
                        {
                            dictionaryFlags.Add(Convert.ToInt32(row[0]), (byte[])row[1]);// FillKeyValuePair(row, 0));
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryFlags;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet GetAUECandCVPermissionsForUser(int userID, int companyID)
        {

            DataSet dsAUECCVPermissions = new DataSet();

            try
            {
                object[] parameter = new object[2];
                parameter[0] = userID;
                parameter[1] = companyID;
                dsAUECCVPermissions = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetAllCacheDataForAUECPermission_New", parameter);

            }
            #region Catch
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
            #endregion
            return dsAUECCVPermissions;
        }

        public DataTable GetAllExecutionInstruction()
        {
            DataTable executionInstruction = new DataTable();
            executionInstruction.Columns.Add(OrderFields.PROPERTY_EXECUTION_INSTID);
            executionInstruction.Columns.Add(OrderFields.PROPERTY_EXECUTION_INST_TagValue);
            executionInstruction.Columns.Add(OrderFields.PROPERTY_EXECUTION_INST);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllExecutionInstructions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstruction.Rows.Add(new Object[3] { row[0].ToString(), row[2].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return executionInstruction;
        }

        public Dictionary<string, string> GetAllSides()
        {
            Dictionary<string, string> sides = new Dictionary<string, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSides";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!sides.ContainsKey(row[2].ToString()))
                        {
                            sides.Add(row[2].ToString(), row[1].ToString());
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return sides;
        }

        public DataTable GetAllSidesWithID()
        {
            DataTable sideWithID = new DataTable();
            sideWithID.Columns.Add(OrderFields.PROPERTY_ORDER_SIDEID);
            sideWithID.Columns.Add(OrderFields.CAPTION_ORDER_SIDE);
            sideWithID.Columns.Add(OrderFields.PROPERTY_ORDER_SIDETAGVALUE);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSides";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        sideWithID.Rows.Add(new object[3] { row[0].ToString(), row[1].ToString(), row[2].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return sideWithID;
        }

        public DataTable GetAllBasicSides()
        {
            DataTable sides = new DataTable();
            sides.Columns.Add(OrderFields.PROPERTY_ORDER_SIDETAGVALUE);
            sides.Columns.Add(OrderFields.PROPERTY_ORDER_SIDE);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllBasicSides";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        sides.Rows.Add(new Object[2] { row[2].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return sides;
        }

        public DataTable GetAllOrderTypes()
        {
            DataTable orderTypes = new DataTable();
            orderTypes.Columns.Add(OrderFields.PROPERTY_ORDER_TYPE_ID);
            orderTypes.Columns.Add(OrderFields.PROPERTY_ORDER_TYPETAGVALUE);
            orderTypes.Columns.Add(OrderFields.PROPERTY_ORDER_TYPE);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllOrderTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Rows.Add(new Object[3] { row[0].ToString(), row[2].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return orderTypes;
        }

        public DataTable GetAllHandlingInstruction()
        {
            DataTable handlingInstruction = new DataTable();
            handlingInstruction.Columns.Add(OrderFields.PROPERTY_HANDLING_INSTID);
            handlingInstruction.Columns.Add(OrderFields.PROPERTY_HANDLING_INST_TagValue);
            handlingInstruction.Columns.Add(OrderFields.PROPERTY_HANDLING_INST);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllHandlingInstructions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstruction.Rows.Add(new Object[3] { row[0].ToString(), row[2].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return handlingInstruction;
        }

        public DataTable GetAllTIFs()
        {
            DataTable tifs = new DataTable();
            tifs.Columns.Add(OrderFields.PROPERTY_TIFID);
            tifs.Columns.Add(OrderFields.PROPERTY_TIF_TAGVALUE);
            tifs.Columns.Add(OrderFields.PROPERTY_TIF);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllTimeInForce";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tifs.Rows.Add(new Object[3] { row[0].ToString(), row[2].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return tifs;
        }

        public DataTable GetAllCMTA()
        {
            DataTable cmta = new DataTable();
            cmta.Columns.Add(OrderFields.PROPERTY_CMTA);
            cmta.Columns.Add(OrderFields.PROPERTY_CMTAID);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCMTA";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        cmta.Rows.Add(new Object[2] { row[0].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return cmta;
        }

        public DataTable GetAllGiveUp()
        {
            DataTable giveUp = new DataTable();
            giveUp.Columns.Add(OrderFields.PROPERTY_GIVEUP);
            giveUp.Columns.Add(OrderFields.PROPERTY_GIVEUPID);

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllGiveUp";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        giveUp.Rows.Add(new Object[2] { row[0].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return giveUp;
        }

        public DataTable GetAssetsExchangeIdentifiers()
        {
            DataTable assetsExchangeIdentifiers = new DataTable();
            assetsExchangeIdentifiers.Columns.Add("AssetID");
            assetsExchangeIdentifiers.Columns.Add("ExchangeIdentifier");

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAssetsExchangeIdentifiers";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        assetsExchangeIdentifiers.Rows.Add(new Object[2] { row[0], row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return assetsExchangeIdentifiers;

        }

        public System.Collections.Generic.Dictionary<int, Prana.BusinessObjects.TimeZone> GetAUECIDTimeZones()
        {
            System.Collections.Generic.Dictionary<int, Prana.BusinessObjects.TimeZone> AuecIDTimeZonesKeyValueCollection = new Dictionary<int, Prana.BusinessObjects.TimeZone>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAUECIDTimeZones";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        Prana.BusinessObjects.TimeZone matchedTimeZone = Prana.BusinessObjects.TimeZoneInfo.FindTimeZoneByString(row[1].ToString());
                        try
                        {
                            if (matchedTimeZone == null)
                                throw new Exception("Time zone could not be found for AUECID " + row[0].ToString() + " and Time Zone string:" + row[1].ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                        }
                        if (matchedTimeZone != null)
                            AuecIDTimeZonesKeyValueCollection.Add(Convert.ToInt32(row[0]), matchedTimeZone);
                    }
                    auecIDTimeZones = AuecIDTimeZonesKeyValueCollection;
                }
            }
            #region Catch
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
            #endregion
            return AuecIDTimeZonesKeyValueCollection;

        }

        public System.Collections.Generic.Dictionary<int, StructSettlementPeriodSidewise> GetAUECIDSettlementPeriods()
        {
            System.Collections.Generic.Dictionary<int, StructSettlementPeriodSidewise> AuecIDSettlementPeriod = new Dictionary<int, StructSettlementPeriodSidewise>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAUECSettlementPeriods";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        StructSettlementPeriodSidewise settlementPeriodSidewise = new StructSettlementPeriodSidewise();
                        if (row[1] != System.DBNull.Value)
                        {
                            settlementPeriodSidewise.Long = Convert.ToInt32(row[1]);
                        }
                        if (row[2] != System.DBNull.Value)
                        {
                            settlementPeriodSidewise.Short = Convert.ToInt32(row[2]);
                        }

                        AuecIDSettlementPeriod.Add(Convert.ToInt32(row[0]), settlementPeriodSidewise);
                    }
                }
            }
            #region Catch
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
            #endregion
            return AuecIDSettlementPeriod;
        }

        public DataTable GetCompany()
        {
            DataTable dtCompany = new DataTable();
            dtCompany.Columns.Add("CompanyID");
            dtCompany.Columns.Add("CompanyName");

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanies";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        // reader.Read(); // Read first record
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dtCompany.Rows.Add(new Object[2] { row[0].ToString(), row[1].ToString() });
                    }
                }
            }
            #region Catch
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
            #endregion
            return dtCompany;
        }

        public Dictionary<int, string> GetCompanies()
        {
            Dictionary<int, string> companies = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanies";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {

                        if (reader[0] != DBNull.Value && reader[1] != DBNull.Value)
                        {
                            int copmanyID = Convert.ToInt16(reader[0]);
                            if (!companies.ContainsKey(copmanyID))
                            {
                                companies.Add(copmanyID, reader[1].ToString());
                            }

                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return companies;
        }

        public Dictionary<int, string> GetAllMasterFunds()
        {

            Dictionary<int, string> masterFunds = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetCompanyFundMasterFundRelationShip";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //master fund ID coming at third position
                        if (!masterFunds.ContainsKey(Convert.ToInt32(row[2])))
                        {
                            masterFunds.Add(Convert.ToInt32(row[2]), row[3].ToString());
                        }

                    }
                }
            }
            #region Catch
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
            #endregion
            return masterFunds;
        }

        public IList<MasterFundAccountDetails> GetAllCustomGroups()
        {
            Dictionary<int, string> groupIds = new Dictionary<int, string>();
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCustomFundGroupsMapping";

            var list = new List<MasterFundAccountDetails>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        var groupId = Convert.ToInt32(row[0]);
                        var accountId = Convert.ToInt32(row[1]);
                        var groupName = Convert.ToString(row[2]);
                        var accountName = Convert.ToString(row[3]);

                        var account = new AccountDto(accountName, accountId);

                        // Check if the group already exists in the list
                        var masterFundAccountDetails = list.FirstOrDefault(x => x.MasterFundOrGroupId == groupId);

                        if (masterFundAccountDetails == null)
                        {
                            masterFundAccountDetails = new MasterFundAccountDetails   //create new if not exists
                            {
                                MasterFundOrGroupId = groupId,
                                MasterFundOrGroupName = groupName,
                                IsCustomGroup = true 
                            };

                            list.Add(masterFundAccountDetails);    // Add to list
                        }
                        masterFundAccountDetails.AccountList.Add(account);    // Add account to the group's AccountList
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while fetching custom group from db");
            }

            return list;
        }

        /// <summary>
        /// returns dictionary of all FactsetCode with CountryID
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetCountryWiseFactsetCodes()
        {
            Dictionary<string, int> dictGetFactsetCodeWithCountryID = new Dictionary<string, int>();
            QueryData query = new QueryData();
            query.StoredProcedureName = "P_GetCountryFactsetCode";

            try
            {
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(query);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!(Convert.ToString(dr["ISOCode"]) == ""))
                            dictGetFactsetCodeWithCountryID.Add(Convert.ToString(dr["ISOCode"]), Convert.ToInt32(dr["CountryID"]));
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
            return dictGetFactsetCodeWithCountryID;
        }
        /// <summary>
        /// returns dictionary of all BloombergCode with CountryID
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetCountryWiseBloombergCodes()
        {
            Dictionary<string, int> dictGetBloombergCodeWithCountryID = new Dictionary<string, int>();
            QueryData query = new QueryData();
            query.StoredProcedureName = "P_GetCountryBloombergCode";

            try
            {
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(query);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr[BloombergCountryCode] != DBNull.Value && !(Convert.ToString(dr[BloombergCountryCode]) == "") && !dictGetBloombergCodeWithCountryID.ContainsKey(Convert.ToString(dr[BloombergCountryCode])))
                            dictGetBloombergCodeWithCountryID.Add(Convert.ToString(dr[BloombergCountryCode]), Convert.ToInt32(dr[CountryId]));
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
            return dictGetBloombergCodeWithCountryID;
        }

        /// <summary>
        /// returns dictionary of all account groups
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllAccountGroups()
        {
            Dictionary<int, string> accountGroups = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFundGroups";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //master fund ID coming at third position
                        if (!accountGroups.ContainsKey(Convert.ToInt32(row[0])))
                        {
                            accountGroups.Add(Convert.ToInt32(row[0]), row[1].ToString());
                        }

                    }
                }
            }
            #region Catch
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
            #endregion
            return accountGroups;
        }

        /// <summary>
        /// returns dictionary of all counter party venues
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllCounterPartyVenues()
        {
            Dictionary<int, string> counterPartyVenues = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCounterPartyVenues";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!counterPartyVenues.ContainsKey(Convert.ToInt32(row[0])))
                        {
                            counterPartyVenues.Add(Convert.ToInt32(row[0]), row[1].ToString());
                        }

                    }
                }
            }
            #region Catch
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
            #endregion
            return counterPartyVenues;
        }

        /// <summary>
        /// Gets the ex assign venue identifier.
        /// </summary>
        /// <returns></returns>
        public Venue GetExAssignVenueID()
        {
            Venue venue = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetExAssignVenue";
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row.Length > 1)
                        {
                            venue = new Venue();
                            venue.VenueID = Convert.ToInt32(row[0]);
                            venue.Name = row[1].ToString();
                        }
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
            return venue;
        }

        public Dictionary<int, string> GetAllMasterStrategy()
        {
            Dictionary<int, string> masterStrategy = new Dictionary<int, string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetCompanyStrategyMasterStrategyRelationShip";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        //master fund ID coming at third position
                        if (!masterStrategy.ContainsKey(Convert.ToInt32(row[2])))
                        {
                            masterStrategy.Add(Convert.ToInt32(row[2]), row[3].ToString());
                        }

                    }
                }
            }
            #region Catch
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
            #endregion
            return masterStrategy;
        }

        public Dictionary<int, string> GetClosingAssets()
        {
            Dictionary<int, string> closingAssets = new Dictionary<int, string>();
            closingAssets.Add((int)AssetCategory.Equity, AssetCategory.Equity.ToString());
            closingAssets.Add((int)AssetCategory.EquityOption, AssetCategory.EquityOption.ToString());
            closingAssets.Add((int)AssetCategory.Future, AssetCategory.Future.ToString());
            closingAssets.Add((int)AssetCategory.FutureOption, AssetCategory.FutureOption.ToString());
            closingAssets.Add((int)AssetCategory.FixedIncome, AssetCategory.FixedIncome.ToString());
            closingAssets.Add((int)AssetCategory.PrivateEquity, AssetCategory.PrivateEquity.ToString());
            closingAssets.Add((int)AssetCategory.ConvertibleBond, AssetCategory.ConvertibleBond.ToString());
            closingAssets.Add((int)AssetCategory.CreditDefaultSwap, AssetCategory.CreditDefaultSwap.ToString());
            return closingAssets;
        }

        /// <summary>
        /// Gets the side for asset identifier.
        /// </summary>
        /// <param name="assetID">The asset identifier.</param>
        /// <returns></returns>
        //public DataTable GetSideForAssetID(int assetID)
        //{
        //    //TO DO remove this hardcoding .... need asset wise sides
        //    int baseAssetID = Mapper.GetBaseAsset(assetID);
        //    DataTable dtSide = new DataTable();
        //    dtSide.Columns.Add("ID");
        //    dtSide.Columns.Add("Value");
        //    if ((AssetCategory)baseAssetID == AssetCategory.Option)
        //    {
        //        dtSide.Rows.Add(new object[] { "B", "Buy to Close" });
        //        dtSide.Rows.Add(new object[] { "A", "Buy to Open" });
        //        dtSide.Rows.Add(new object[] { "D", "Sell to Close" });
        //        dtSide.Rows.Add(new object[] { "C", "Sell to Open" });
        //    }
        //    else
        //    {
        //        dtSide.Rows.Add(new object[] { 1, "Buy" });
        //        dtSide.Rows.Add(new object[] { "B", "Buy to Close" });
        //        dtSide.Rows.Add(new object[] { 2, "Sell" });
        //        dtSide.Rows.Add(new object[] { 5, "Sell Short" });
        //    }

        //    return dtSide;
        //}

        public List<GenericNameID> GetAllPrimeBrokers()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAllDataSourceNames";

            List<GenericNameID> dataSourceList = new List<GenericNameID>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(new GenericNameID(row));

                    }
                    dataSourceList.Add(new GenericNameID(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
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

            return dataSourceList;
        }

        public List<int> GetInUseAUECIDs()
        {
            List<int> inUseAUECIDs = new List<int>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "GetInUseAUECs";
            queryData.CommandTimeout = _heavyGetTimeout;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        if (reader[0] != DBNull.Value)
                        {
                            inUseAUECIDs.Add(Convert.ToInt32(reader[0]));
                        }
                    }
                }
            }
            #region catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                #endregion
            }
            return inUseAUECIDs;
        }

        public Dictionary<int, DateTime> FetchClearanceTime()
        {
            try
            {
                Dictionary<int, DateTime> auecWiseClearanceTime = new Dictionary<int, DateTime>(GetAUECCount());

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPMClearanceTime";

                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                    {
                        while (reader.Read())
                        {
                            int auecID = Convert.ToInt32(reader[0]);
                            DateTime clearanceTime = DateTime.Parse(reader[1].ToString());

                            auecWiseClearanceTime.Add(auecID, DateTime.Now.Date + clearanceTime.TimeOfDay);
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
                return auecWiseClearanceTime;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }
        public Prana.BusinessObjects.TimeZone GetAUECTimeZone(int auecID)
        {
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> dt = auecIDTimeZones;
                if (dt.ContainsKey(auecID))
                {
                    return dt[auecID];
                }
                else
                {
                    return CurrentTimeZone;
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
            return CurrentTimeZone;
        }
        public int GetAUECCount()
        {
            try
            {
                DataSet keyValuePairs = GetKeyValuePairs();
                _auecs = FillAUEC(keyValuePairs.Tables[6]);
                return _auecs.Count;
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
            return 0;
        }
        public Dictionary<int, MarketTimes> GetMarketStartEndTime()
        {
            Dictionary<int, MarketTimes> marketStartEndTimes = new Dictionary<int, MarketTimes>(GetAUECCount());

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllMarketStartEndTimes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];

                        reader.GetValues(row);

                        MarketTimes currentAUECMarketHours = new MarketTimes();
                        int AuecID = Convert.ToInt32(row[0]);
                        Prana.BusinessObjects.TimeZone auecTimeZone = GetAUECTimeZone(AuecID);
                        if (auecTimeZone != null)
                        {
                            currentAUECMarketHours.MarketStartTime = DateTime.Now.Date + Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.Parse(row[1].ToString()), auecTimeZone).TimeOfDay;
                            currentAUECMarketHours.MarketEndTime = DateTime.Now.Date + Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.Parse(row[2].ToString()), auecTimeZone).TimeOfDay;
                            if (!marketStartEndTimes.ContainsKey(AuecID))
                            {
                                marketStartEndTimes.Add(AuecID, currentAUECMarketHours);
                            }
                        }
                        else
                        {
                            throw new Exception("Time zone could not be found for AUECID " + AuecID);
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
            return marketStartEndTimes;
        }

        public Dictionary<string, int> GetActivityType()
        {
            Dictionary<string, int> dictionaryActivity = new Dictionary<string, int>();

            try
            {
                if (_dsAllActivityTables == null)
                    _dsAllActivityTables = DataManagerInternalRepository.GetAllActivitiesFromDB();
                if (_dsAllActivityTables != null && _dsAllActivityTables.Tables.Count > 0)
                {
                    foreach (DataRow row in _dsAllActivityTables.Tables["ActivityType"].Rows)
                    {
                        if (!dictionaryActivity.ContainsKey(row[1].ToString().Trim()))
                        {
                            dictionaryActivity.Add(row[1].ToString().Trim(), Convert.ToInt32(row[0]));//(FillKeyValuePair(row, 0));
                        }
                    }
                }
                #region old commented code
                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetActivities"))
                //{
                //    while (reader.Read())
                //    {
                //        object[] row = new object[reader.FieldCount];
                //        reader.GetValues(row);
                //        if (!dictionaryActivity.ContainsKey(row[0].ToString().Trim()))
                //        {
                //            dictionaryActivity.Add(row[0].ToString().Trim(), int.Parse(row[1].ToString().Trim()));//(FillKeyValuePair(row, 0));
                //        }
                //    }
                //}
                #endregion
            }
            #region Catch
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
            #endregion
            return dictionaryActivity;
        }

        /// <summary>
        /// Creates a dictionary of activity types with acronym as a key
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetActivityTypeWithAcronym()
        {
            Dictionary<string, string> dictActivityTypeWithAcronym = new Dictionary<string, string>();
            try
            {
                if (_dsAllActivityTables == null)
                    _dsAllActivityTables = DataManagerInternalRepository.GetAllActivitiesFromDB();
                if (_dsAllActivityTables != null && _dsAllActivityTables.Tables.Count > 0)
                {
                    foreach (DataRow row in _dsAllActivityTables.Tables["ActivityType"].Rows)
                    {
                        //row[5] is the acronym column and row[1] is the activity type column
                        if (!dictActivityTypeWithAcronym.ContainsKey(row[5].ToString().Trim()))
                        {
                            dictActivityTypeWithAcronym.Add(row[5].ToString().Trim(), row[1].ToString().Trim());
                        }
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
            return dictActivityTypeWithAcronym;
        }

        /// <summary>
        /// Get the dictionary of the last calculated revaluation dates accountwise
        /// </summary>
        /// <returns>dictionary of last calculated revaluation dates account-wise</returns>
        public Dictionary<int, RevaluationUpdateDetail> GetLastRevaluationCalcDate()
        {
            Dictionary<int, RevaluationUpdateDetail> lastRevalCalcDate = new Dictionary<int, RevaluationUpdateDetail>();

            try
            {
                if (_dsAllActivityTables == null)
                    _dsAllActivityTables = DataManagerInternalRepository.GetAllActivitiesFromDB();
                if (_dsAllActivityTables != null && _dsAllActivityTables.Tables.Count > 0)
                {
                    foreach (DataRow row in _dsAllActivityTables.Tables["LastRevaluationCalcDate"].Rows)
                    {
                        if (!string.IsNullOrEmpty(row[0].ToString()) && !string.IsNullOrEmpty(row[1].ToString())
                            && !string.IsNullOrEmpty(row[2].ToString())
                            && !lastRevalCalcDate.ContainsKey(Convert.ToInt32(row[0])))
                        {
                            DateTime dateReval = Convert.ToDateTime(row[1].ToString());
                            bool isUpdateReval = Convert.ToBoolean(row[2].ToString());
                            RevaluationUpdateDetail objReval = new RevaluationUpdateDetail(dateReval, isUpdateReval);
                            lastRevalCalcDate.Add(Convert.ToInt32(row[0]), objReval);
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return lastRevalCalcDate;

        }

        public Dictionary<int, byte> GetActivityTypeActivitySource()
        {
            Dictionary<int, byte> dictionaryActivityTypeActivitySource = new Dictionary<int, byte>();

            try
            {
                if (_dsAllActivityTables == null)
                    _dsAllActivityTables = DataManagerInternalRepository.GetAllActivitiesFromDB();
                if (_dsAllActivityTables != null && _dsAllActivityTables.Tables.Count > 0)
                {
                    foreach (DataRow row in _dsAllActivityTables.Tables["ActivityType"].Rows)
                    {
                        if (!dictionaryActivityTypeActivitySource.ContainsKey(Convert.ToInt32(row[0])))
                        {
                            byte activitySource = Convert.ToByte(row[4].ToString());
                            dictionaryActivityTypeActivitySource.Add(Convert.ToInt32(row[0]), activitySource);//(FillKeyValuePair(row, 0));
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryActivityTypeActivitySource;

        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetActivityAmountType()
        {
            Dictionary<int, string> dictionaryActivityAmountType = new Dictionary<int, string>();

            try
            {
                if (_dsAllActivityTables == null)
                    _dsAllActivityTables = DataManagerInternalRepository.GetAllActivitiesFromDB();
                if (_dsAllActivityTables != null && _dsAllActivityTables.Tables.Count > 0)
                {
                    foreach (DataRow row in _dsAllActivityTables.Tables["AmountType"].Rows)
                    {
                        if (!dictionaryActivityAmountType.ContainsKey(Convert.ToInt32(row[0])))
                        {
                            dictionaryActivityAmountType.Add(Convert.ToInt32(row[0]), row[1].ToString().Trim());//(FillKeyValuePair(row, 0));
                        }
                    }
                }
                #region old commented code
                //using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetActivityAmountType"))
                //{
                //    while (reader.Read())
                //    {
                //        object[] row = new object[reader.FieldCount];
                //        reader.GetValues(row);
                //        if (!dictionaryActivityAmountType.ContainsKey(int.Parse(row[0].ToString().Trim())))
                //        {
                //            dictionaryActivityAmountType.Add(int.Parse(row[0].ToString().Trim()), row[1].ToString().Trim());//(FillKeyValuePair(row, 0));
                //        }
                //    }
                //}
                #endregion
            }
            #region Catch
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
            #endregion
            return dictionaryActivityAmountType;

        }

        public Dictionary<string, int> GetActivityWithBalanceTypeID()
        {
            Dictionary<string, int> dictionaryActivity = new Dictionary<string, int>();
            try
            {
                if (_dsAllActivityTables == null)
                    _dsAllActivityTables = DataManagerInternalRepository.GetAllActivitiesFromDB();
                if (_dsAllActivityTables != null && _dsAllActivityTables.Tables.Count > 0)
                {
                    foreach (DataRow row in _dsAllActivityTables.Tables["ActivityType"].Rows)
                    {
                        if (!dictionaryActivity.ContainsKey(row[1].ToString().Trim()))
                        {
                            dictionaryActivity.Add(row[1].ToString().Trim(), Convert.ToInt32(row[3]));//(FillKeyValuePair(row, 0));
                        }
                    }
                }
            }
            #region Catch
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
            #endregion
            return dictionaryActivity;
        }

        public DataSet GetAttributeNames()
        {
            DataSet dtAttributes = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAttributeNames";

                dtAttributes = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            #region Catch
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
            #endregion
            return dtAttributes;
        }

        public static DataSet GetCompanyRiskPreferences(int companyID)
        {
            DataSet dsMaxViews = new DataSet();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                dsMaxViews = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetCompanyRiskUIPrefs", parameter);
            }
            #region Catch
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
            #endregion
            return dsMaxViews;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPranaPreference()
        {
            DataSet ds = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPranaPreferences";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return ds;
        }

        /// <summary>
        /// This method fills account wise base currency in the cache and maintains a dictionary
        /// </summary>
        /// <param name="dtKeyValues"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ConcurrentDictionary<int, int> FillAccountWiseBaseCurrency(DataTable dtKeyValues, int offset)
        {
            ConcurrentDictionary<int, int> keyValue = new ConcurrentDictionary<int, int>();
            int id = 0;
            int value = 1 + offset;
            try
            {
                foreach (DataRow dr in dtKeyValues.Rows)
                {
                    int rowID = Convert.ToInt32(dr[id]);

                    keyValue.TryAdd(rowID, Convert.ToInt32(dr[value]));
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
            return keyValue;
        }

        /// <summary>
        /// This method fills  release wise all accounts
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public Dictionary<int, List<int>> FillReleaseWiseAccount(DataTable dataTable)
        {
            Dictionary<int, List<int>> keyValue = new Dictionary<int, List<int>>();
            try
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    if (dr["ReleaseID"] != DBNull.Value && dr["CompanyFundID"] != DBNull.Value)
                    {
                        int releaseID = Convert.ToInt32(dr["ReleaseID"]);
                        int accountID = Convert.ToInt32(dr["CompanyFundID"]);
                        if (keyValue.ContainsKey(releaseID))
                        {
                            keyValue[releaseID].Add(accountID);
                        }
                        else
                        {
                            keyValue.Add(releaseID, new List<int>() { accountID });
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
            return keyValue;
        }

        public Dictionary<int, string> GetCompanyModulesPermissioning(int companyID)
        {
            DataSet ds = new DataSet();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyID;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetCompanyModules", parameter, "PranaConnectionString");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dict.Add(Convert.ToInt32(dr["ModuleID"]), dr["ModuleName"].ToString());
                }
            }
            #region Catch
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
            #endregion
            return dict;
        }

        public Dictionary<int, string> GetAlgoBrokersWithFullName()
        {
            DataSet dtAlgoBrokers = new DataSet();
            Dictionary<int, string> algoBrokersWithFullName = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAlAlgoBrokers";

                dtAlgoBrokers = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                foreach (DataRow dr in dtAlgoBrokers.Tables[0].Rows)
                {
                    algoBrokersWithFullName.Add(Convert.ToInt32(dr["CounterPartyID"]), dr["FullName"].ToString());
                }
            }
            #region Catch
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
            #endregion
            return algoBrokersWithFullName;
        }

        public Dictionary<int, string> GetAlgoBrokersWithShortName()
        {
            DataSet dtAlgoBrokers = new DataSet();
            Dictionary<int, string> algoBrokersWithShortName = new Dictionary<int, string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAlAlgoBrokers";

                dtAlgoBrokers = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                foreach (DataRow dr in dtAlgoBrokers.Tables[0].Rows)
                {
                    algoBrokersWithShortName.Add(Convert.ToInt32(dr["CounterPartyID"]), dr["ShortName"].ToString());
                }
            }
            #region Catch
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
            #endregion
            return algoBrokersWithShortName;
        }

        /// <summary>
        /// Gets the send allocations via fix.
        /// </summary>
        /// <returns></returns>
        public bool GetSendAllocationsViaFix()
        {
            bool isSendAllocations = false;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetCompanies";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        // reader.Read(); // Read first record
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        isSendAllocations = Convert.ToBoolean(row[25]);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isSendAllocations;
        }

        /// <summary>
        /// Gets the send allocations via fix.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetCurrentNavLockDate()
        {
            DateTime? currentNavLockDate = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetNAVLocks";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    if (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        currentNavLockDate = Convert.ToDateTime(row[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return currentNavLockDate;
        }
    }
}