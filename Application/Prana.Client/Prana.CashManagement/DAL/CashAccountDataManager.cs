using Infragistics.Win;
using Prana.BusinessLogic;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;

namespace Prana.CashManagement
{
    public class CashAccountDataManager
    {
        #region Members

        /// <summary>
        /// The error number
        /// </summary>
        private static int _errorNumber = 0;

        /// <summary>
        /// The error message
        /// </summary>
        private static string _errorMessage = string.Empty;

        #endregion Members

        #region Methods

        /// <summary>
        /// Fills the ds.
        /// </summary>
        public static void FillDS()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_MultipleTable";
            queryData.CommandTimeout = 200;
            DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
        }

        /// <summary>
        /// Gets the accruals value for give date.
        /// </summary>
        /// <param name="fromdate">The fromdate.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        internal static DataSet GetAccrualsValueForGiveDate(DateTime fromdate, DateTime toDate)
        {
            DataSet ds = new DataSet();

            try
            {
                string[] tables = { "Accruals" };
                object[] param = new object[2];
                param[0] = fromdate;
                param[1] = toDate;

                DatabaseManager.DatabaseManager.LoadDataSet("P_GetAccruals", ds, tables, param);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return ds;
        }

        /// <summary>
        /// Gets all account types.
        /// </summary>
        /// <returns></returns>
        internal static ValueList GetAllAccountTypes()
        {
            DataTable _accountTypes = CachedDataManager.GetInstance.GetMasterCategorySubCategoryTables().Tables[CashManagementConstants.TABLE_ACCOUNTTYPE];
            ValueList listAccountTypes = new ValueList();
            listAccountTypes.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

            try
            {
                foreach (DataRow row in _accountTypes.Rows)
                {
                    listAccountTypes.ValueListItems.Add(Convert.ToInt32(row[CashManagementConstants.COLUMN_ACCOUNTTYPEID]), row[CashManagementConstants.COLUMN_ACCOUNTTYPE].ToString());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return listAccountTypes;
        }

        /// <summary>
        /// Gets all Sub account types.
        /// </summary>
        /// <returns></returns>
        internal static ValueList GetAllSubAccountTypes()
        {
            DataTable _dataTableSubAccountType = CachedDataManager.GetInstance.GetAllActivityTables().Tables[CashManagementConstants.TABLE_SUBACCOUNTSTYPE];
            ValueList _vlsubAccountType = new ValueList();
            _vlsubAccountType.ValueListItems.Add(DBNull.Value, ApplicationConstants.C_COMBO_SELECT);

            try
            {
                foreach (DataRow row in _dataTableSubAccountType.Rows)
                {
                    _vlsubAccountType.ValueListItems.Add(Convert.ToInt32(row[CashManagementConstants.COLUMN_SUBACCOUNTTYPEID]), row[CashManagementConstants.COLUMN_SUBACCOUNTTYPE].ToString());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return _vlsubAccountType;
        }

        /// <summary>
        /// Gets all master types.
        /// </summary>
        /// <returns></returns>
        internal static ValueList GetAllMasterTypes()
        {
            DataTable _masterCategoryType = CachedDataManager.GetInstance.GetMasterCategorySubCategoryTables().Tables[CashManagementConstants.TABLE_MASTERCATEGORY];
            ValueList lsmasterCategoryTypes = new ValueList();
            // lsmasterCategoryTypes.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);

            try
            {
                foreach (DataRow row in _masterCategoryType.Rows)
                {
                    lsmasterCategoryTypes.ValueListItems.Add(Convert.ToInt32(row[0]), row[1].ToString());
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return lsmasterCategoryTypes;
        }

        /// <summary>
        /// Ifs the activity type is in use.
        /// </summary>
        /// <param name="activityTypeId">The activity type identifier.</param>
        /// <returns></returns>
        internal static bool IfActivityTypeIsInUse(int activityTypeId)
        {
            bool result = false; ;

            try
            {
                object[] parameter = new object[1];

                parameter[0] = activityTypeId;

                string spName = "P_IfCashActivityTypeInUse";

                result = Convert.ToBoolean(DatabaseManager.DatabaseManager.ExecuteScalar(spName, parameter));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return result;
        }

        /// <summary>
        /// Ifs the journal mapping is in use.
        /// </summary>
        /// <param name="activityTypeID">The activity type identifier.</param>
        /// <returns></returns>
        internal static bool IfJournalMappingIsInUse(int activityTypeID)
        {
            bool result = false; ;

            try
            {
                object[] parameter = new object[1];

                parameter[0] = activityTypeID;

                string spName = "P_IfJournalMappingInUse";

                result = Convert.ToBoolean(DatabaseManager.DatabaseManager.ExecuteScalar(spName, parameter));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return result;
        }

        /// <summary>
        /// Ifs the sub account is in use.
        /// </summary>
        /// <param name="subAccountID">The sub account identifier.</param>
        /// <returns></returns>
        internal static bool IfSubAccountIsInUse(int subAccountID)
        {
            bool result = false; ;

            try
            {
                object[] parameter = new object[1];

                parameter[0] = subAccountID;

                string spName = "P_IfCashSubAccountInUse";

                result = Convert.ToBoolean(DatabaseManager.DatabaseManager.ExecuteScalar(spName, parameter));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return result;
        }

        /// <summary>
        /// Restores the default activity journal mapping.
        /// </summary>
        internal static void RestoreDefaultActivityJournalMapping()
        {
            try
            {
                object[] parameter = new object[0];
                string spName = "P_RestoreDefaultActivityJournalMapping";

                DatabaseManager.DatabaseManager.ExecuteScalar(spName, parameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the cash dividend value.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        internal static int SaveCashDividendValue(string xml)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveCashDividendValues";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Saves the sub account cash value.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        internal static int SaveSubAccountCashValue(string xml)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveSubAccountCashValues";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xml
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Updates the accruals values in database.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        internal static int UpdateAccrualsValuesInDB(DataSet ds)
        {
            int rowsAffected = 0;

            try
            {
                DataTable dtAccruals = ds.Tables[0];
                if (dtAccruals != null)
                {
                    DatabaseManager.DatabaseManager.Update("Select * from T_Accruals", dtAccruals);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return rowsAffected;
        }

        /// <summary>
        /// Updates the cash accounts tables in database.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public static int UpdateCashAccountsTablesInDB(DataSet dataSet)
        {
            int rowsAffected = 0;

            try
            {
                DataTable dtDeletedSubAccounts = dataSet.Tables["SubCashAccounts"].GetChanges(DataRowState.Deleted);
                if (dtDeletedSubAccounts != null && dtDeletedSubAccounts.Rows.Count > 0)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubAccounts", dtDeletedSubAccounts);
                    rowsAffected++;
                }

                DataTable dtAddedSubAccounts = dataSet.Tables["SubCashAccounts"].GetChanges(DataRowState.Added);
                if (dtAddedSubAccounts != null)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubAccounts", dtAddedSubAccounts);
                    rowsAffected++;
                }

                DataTable dtModifiedSubAccounts = dataSet.Tables["SubCashAccounts"].GetChanges(DataRowState.Modified);
                if (dtModifiedSubAccounts != null)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubAccounts", dtModifiedSubAccounts);
                    rowsAffected++;
                }

                dataSet.AcceptChanges();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

            return rowsAffected;
        }

        #endregion Methods
    }
}
