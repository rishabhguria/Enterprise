using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CommonDatabaseAccess
{
    internal static class DataManagerInternalRepository
    {
        private static KeyValueDataManager _keyValueDataManager;
        private static ClientsCommonDataManager _clientsCommonDataManager;

        #region activity relationship
        private const string TABLE_ACTIVITYTYPE = "ActivityType";
        private const string TABLE_ACTIVITYAMOUNTTYPE = "AmountType";
        private const string TABLE_ACTIVITYJOURNALMAPPING = "ActivityJournalMapping";
        private const string TABLE_SUBACCOUNTS = "SubAccounts";
        private const string TABLE_ACTIVITYDATETYPE = "ActivityDateType";
        private const string TABLE_LASTREVALCALCDATE = "LastRevaluationCalcDate";
        private const string TABLE_SUBACCOUNTTYPE = "SubAccountType";
        private const string TABLE_TRANSACTIONSOURCE = "TransactionSource";
        #endregion

        static DataManagerInternalRepository()
        {
            if (_keyValueDataManager == null)
            {
                _keyValueDataManager = new KeyValueDataManager();
            }
            if (_clientsCommonDataManager == null)
            {
                _clientsCommonDataManager = new ClientsCommonDataManager();
            }
        }

        internal static KeyValueDataManager KeyValueDataManager
        {
            get { return _keyValueDataManager; }
            set { _keyValueDataManager = value; }
        }

        internal static ClientsCommonDataManager ClientsCommonDataManager
        {
            get { return _clientsCommonDataManager; }
            set { _clientsCommonDataManager = value; }
        }

        public static DataSet GetAllAccountTablesFromDB()
        {
            DataSet ds = new DataSet();

            try
            {
                string[] tables = { "MasterCategory", "SubCategory", "TransactionType", "SubCashAccounts", "MasterFundSide" };
                DatabaseManager.DatabaseManager.LoadDataSet("P_GetAllAccountTables", ds, tables, new object[0]);
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

        public static DataSet GetAllAccountsWithRelation(DataSet _masterCategorySubCategory)
        {
            try
            {
                if (_masterCategorySubCategory == null)
                    _masterCategorySubCategory = GetAllAccountTablesFromDB();
                if (_masterCategorySubCategory != null && _masterCategorySubCategory.Tables.Count > 0)
                {
                    if (_masterCategorySubCategory.Tables.Contains("MasterCategory") && _masterCategorySubCategory.Tables.Contains("SubCategory") && _masterCategorySubCategory.Tables.Contains("TransactionType"))
                    {

                        if (!_masterCategorySubCategory.Relations.Contains("masterCategorySubCategory"))
                            _masterCategorySubCategory.Relations.Add("masterCategorySubCategory", _masterCategorySubCategory.Tables["MasterCategory"].Columns["MasterCategoryId"], _masterCategorySubCategory.Tables["SubCategory"].Columns["MasterCategoryId"], false);

                        if (!_masterCategorySubCategory.Relations.Contains("subCategorySubAccounts"))
                            _masterCategorySubCategory.Relations.Add("subCategorySubAccounts", _masterCategorySubCategory.Tables["SubCategory"].Columns["SubCategoryID"], _masterCategorySubCategory.Tables["SubCashAccounts"].Columns["SubCategoryID"], false);

                        if (!_masterCategorySubCategory.Relations.Contains("subAccountsAccountType"))
                            _masterCategorySubCategory.Relations.Add("subAccountsAccountType", _masterCategorySubCategory.Tables["SubCashAccounts"].Columns["TransactionTypeID"], _masterCategorySubCategory.Tables["TransactionType"].Columns["TransactionTypeID"], false);

                        if (!_masterCategorySubCategory.Relations.Contains("masterCategoryAccountSide"))
                            _masterCategorySubCategory.Relations.Add("masterCategoryAccountSide", _masterCategorySubCategory.Tables["MasterCategory"].Columns["MasterCategoryId"], _masterCategorySubCategory.Tables["MasterFundSide"].Columns["MasterCategoryId"], false);

                    }
                }

                return _masterCategorySubCategory;
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

        public static DataSet GetAllActivitiesFromDB()
        {
            try
            {
                DataSet dataSetActivities = new DataSet();
                try
                {
                    string[] tables = { TABLE_ACTIVITYTYPE, TABLE_ACTIVITYAMOUNTTYPE, TABLE_ACTIVITYJOURNALMAPPING, TABLE_SUBACCOUNTS, TABLE_ACTIVITYDATETYPE, TABLE_LASTREVALCALCDATE, TABLE_SUBACCOUNTTYPE, TABLE_TRANSACTIONSOURCE };
                    DatabaseManager.DatabaseManager.LoadDataSet("P_GetAllActivityTables", dataSetActivities, tables, new object[0]);
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
                return dataSetActivities;
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

        public static DataSet GetKeyValuePairs()
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

        public static Dictionary<int, string> FillKeyValuePairs(DataTable keyValues, int offset)
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
    }
}