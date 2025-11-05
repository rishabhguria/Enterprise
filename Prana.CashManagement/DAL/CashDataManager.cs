using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Prana.CashManagement
{
    class CashDataManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CashDataManager"/> class.
        /// </summary>
        public CashDataManager()
        {
        }

        /// <summary>
        /// The persistence manager
        /// </summary>
        static PersistenceManager _persistenceManager = new PersistenceManager();

        /// <summary>
        /// The update lock
        /// </summary>
        private static readonly object _updateLock = new object();

        /// <summary>
        /// Gets the transaction entries for given date.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public static List<T> GetTransactionEntriesForGivenDate<T>(string accountIDs, DateTime fromDate, DateTime toDate)
        {
            List<T> dataList = new List<T>();
            try
            {
                dataList = (List<T>)_persistenceManager.GetTransactionEntriesForGivenDate<T>(accountIDs, fromDate, toDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataList;
        }

        /// <summary>
        /// Gets the day end data in base currency.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <param name="IsAccrualsNeeded">if set to <c>true</c> [is accruals needed].</param>
        /// <returns></returns>
        public static DataSet GetDayEndDataInBaseCurrency(DateTime givenDate, bool IsAccrualsNeeded, bool isIncludeTradingDayAccruals = true)
        {
            DataSet ds = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetDayEndDataInBaseCurrency";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@DateForCashValues", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@DateForCashValues",
                    ParameterType = DbType.DateTime,
                    ParameterValue = givenDate
                });
                queryData.DictionaryDatabaseParameter.Add("@IsAccrualsNeeded", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsAccrualsNeeded",
                    ParameterType = DbType.Boolean,
                    ParameterValue = IsAccrualsNeeded
                });

                queryData.DictionaryDatabaseParameter.Add("@IsIncludeTradingDayAccruals", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsIncludeTradingDayAccruals",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isIncludeTradingDayAccruals
                });


                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return ds;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Get the values between the supplied dates
        /// </summary>
        /// <typeparam name="T">Name of the type which is to be returned</typeparam>
        /// <param name="startDate">First date of the range</param>
        /// <param name="endDate">second date of the range</param>
        /// <param name="className">Name of the hibernate class that will be used for fetching the data</param>
        /// <param name="accountIDs">List of accountIDs</param>
        /// <returns>List of objects of type T</returns>
        public static List<T> GetValueBeetweenTwoDates<T>(DateTime startDate, DateTime endDate, string className, String accountIDs, int activitySource)
        {
            List<T> cashDataList = new List<T>();
            try
            {
                string query = "";
                if (className == "TransactionEntry")
                {
                    query = "from TransactionEntry where (datediff(dd,TransactionDate,'" + startDate + "')<=0 And datediff(dd,TransactionDate,'" + endDate + "')>=0) and fundID in(" + accountIDs + ") and ActivitySource = " + activitySource + " ORDER BY TransactionID";
                }
                else if (className == "CompanyAccountCashCurrencyValue")
                {
                    query = "from CompanyAccountCashCurrencyValue where (datediff(dd,Date,'" + startDate + "')<=0 And datediff(dd,Date,'" + endDate + "')>=0) ORDER BY Date";
                }
                cashDataList = GetDataFromDb<T>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return cashDataList;
        }

        /// <summary>
        /// Gets the data from database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static List<T> GetDataFromDb<T>(string query)
        {
            List<T> dataList = new List<T>();
            try
            {
                dataList = (List<T>)_persistenceManager.GetData<T>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataList;
        }

        /// <summary>
        /// added by: Bharat raturi, 23 jul 2014
        /// get the day end cash for the accounts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> GetDayEndCashFromDb<T>(string accountIDs, DateTime dtStartDate, DateTime dtEndDate)
        {
            List<T> dataList = new List<T>();
            try
            {
                dataList = (List<T>)_persistenceManager.GetDayEndCash<T>(accountIDs, dtStartDate, dtEndDate);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataList;
        }

        /// <summary>
        /// Saves the cash currency value in data base.
        /// </summary>
        /// <param name="dateWiseDayEndDictionary">The date wise day end dictionary.</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public static List<CompanyAccountCashCurrencyValue> SaveCashCurrencyValueInDataBase(Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseDayEndDictionary, List<CompanyAccountCashCurrencyValue> lstDeletedData, string accountIDs)
        {
            List<CompanyAccountCashCurrencyValue> lsToPublish = new List<CompanyAccountCashCurrencyValue>();
            try
            {
                lsToPublish = _persistenceManager.SaveCashCurrencyValue(dateWiseDayEndDictionary, lstDeletedData, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsToPublish;
        }

        /// <summary>
        /// Saves the CI value in data base.
        /// </summary>
        /// <param name="dateWiseDayEndDictionary">The date wise day end dictionary.</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public static List<CollateralInterestValue> SaveCIValueInDataBase(Dictionary<string, GenericBindingList<CollateralInterestValue>> dateWiseCIDictionary, List<CollateralInterestValue> lstDeletedData, string accountIDs)
        {
            List<CollateralInterestValue> lsToPublish = new List<CollateralInterestValue>();
            try
            {
                lsToPublish = _persistenceManager.SaveCIValue(dateWiseCIDictionary, lstDeletedData, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsToPublish;
        }

        /// <summary>
        /// Calculates the difference in mark price.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="dateToCheck">The date to check.</param>
        /// <param name="level1Id">The level1 identifier.</param>
        /// <returns></returns>
        public static decimal calculateDiffInMarkPrice(string symbol, DateTime dateToCheck, int level1Id)
        {
            try
            {
                //Have to check if Date to check==AUECModifiedDate then logic will be (db avgprice- current avg price)
                //SqlCommand cmd = new SqlCommand("select dbo.DIFFINMARKPRICEBYDATE('" + symbol + "','" + dateToCheck + "')");

                QueryData queryData = new QueryData();
                queryData.Query = "select dbo.DIFFINMARKPRICEBYDATE(@symbol,@datetocheck,@level1Id)";
                queryData.DictionaryDatabaseParameter.Add("@symbol", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@symbol",
                    ParameterType = DbType.String,
                    ParameterValue = symbol
                });
                queryData.DictionaryDatabaseParameter.Add("@datetocheck", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@datetocheck",
                    ParameterType = DbType.DateTime,
                    ParameterValue = dateToCheck
                });
                queryData.DictionaryDatabaseParameter.Add("@level1Id", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@level1Id",
                    ParameterType = DbType.Int32,
                    ParameterValue = level1Id
                });

                object result = DatabaseManager.DatabaseManager.ExecuteScalar(queryData).ToString();
                if (DatabaseManager.DatabaseManager.ExecuteScalar(queryData) != DBNull.Value)
                {
                    return decimal.Parse(result.ToString(), System.Globalization.NumberStyles.Float);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return 0;
        }

        /// <summary>
        /// Gets the mark price for day and symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="dateToCheck">The date to check.</param>
        /// <param name="level1Id">The level1 identifier.</param>
        /// <returns></returns>
        public static decimal GetMarkPriceForDayAndSymbol(string symbol, DateTime dateToCheck, int level1Id)
        {
            try
            {
                //Have to check if Date to check==AUECModifiedDate then logic will be (db avgprice- current avg price)
                //SqlCommand cmd = new SqlCommand("select dbo.GetMarkPriceForDayAndSymbol('" + symbol + "','" + dateToCheck.Date + "')");
                QueryData queryData = new QueryData();
                queryData.Query = "select dbo.GetMarkPriceForDayAndSymbol(@symbol,@datetocheck,@level1Id)";
                queryData.DictionaryDatabaseParameter.Add("@symbol", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@symbol",
                    ParameterType = DbType.String,
                    ParameterValue = symbol
                });
                queryData.DictionaryDatabaseParameter.Add("@datetocheck", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@datetocheck",
                    ParameterType = DbType.DateTime,
                    ParameterValue = dateToCheck.Date
                });
                queryData.DictionaryDatabaseParameter.Add("@level1Id", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@level1Id",
                    ParameterType = DbType.Int32,
                    ParameterValue = level1Id
                });

                object result = DatabaseManager.DatabaseManager.ExecuteScalar(queryData).ToString();
                if (DatabaseManager.DatabaseManager.ExecuteScalar(queryData) != DBNull.Value)
                {
                    return decimal.Parse(result.ToString(), System.Globalization.NumberStyles.Float);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return 0;
        }

        /// <summary>
        /// Saves the or update data in data base.
        /// </summary>
        /// <param name="dataToSave">The data to save.</param>
        /// <param name="lsModifiedTransactionEntries">The ls modified transaction entries.</param>
        public static void SaveOrUpdateDataInDataBase(List<Transaction> dataToSave, Dictionary<string, TransactionEntry> lsModifiedTransactionEntries, bool isSymbolLevelAccruals)
        {
            try
            {
                if (lsModifiedTransactionEntries != null && lsModifiedTransactionEntries.Count > 0)
                {
                    _persistenceManager.UpdateLastCalculatedBalanceDateForTransactionEntries(lsModifiedTransactionEntries);
                }

                //Modified by: Bharat raturi, 01 oct 2014
                //This part should be executed for manual journal entry, opening balances and imported data, so that for both of them activity IDs could be generated 
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5041
                List<Transaction> manualTranList = dataToSave.Where(((transaction => (transaction.GetTransactionSource() == (int)CashTransactionType.ManualJournalEntry) || (transaction.GetTransactionSource() == (int)CashTransactionType.OpeningBalance) || (transaction.GetTransactionSource() == (int)CashTransactionType.ImportedEditableData)))).ToList();

                List<Transaction> tradeTranList = dataToSave.Where(((transaction => (transaction.GetTransactionSource() != (int)CashTransactionType.ManualJournalEntry) && (transaction.GetTransactionSource() != (int)CashTransactionType.OpeningBalance) && (transaction.GetTransactionSource() != (int)CashTransactionType.ImportedEditableData)))).ToList();

                if (manualTranList != null && manualTranList.Count != 0)
                    _persistenceManager.SaveOrUpdateManualJournal(manualTranList);  //this one is for only manual journal

                if (tradeTranList != null && tradeTranList.Count != 0 && !isSymbolLevelAccruals)
                    _persistenceManager.SaveOrUpdate(tradeTranList);   //this one is for All journals other than Manual Journal

                if (tradeTranList != null && tradeTranList.Count != 0 && isSymbolLevelAccruals)
                    _persistenceManager.SaveOrUpdateSymbolLevelAccrual(tradeTranList);   //this one is for All journals other than Manual Journal

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the last revaluation calculated date to given date.
        /// </summary>
        /// <param name="CMStartDate">The cm start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="isUpdated">if set to <c>true</c> [is updated].</param>
        /// <param name="fromRevaluation">if set to <c>true</c> [from revaluation].</param>
        /// <param name="isManualRevaluation">if set to <c>true</c> [is manual revaluation].</param>
        public static void UpdateLastRevaluationCalculatedDateToGivenDate(DateTime CMStartDate, DateTime endDate, String accountIDs, bool isUpdated, bool fromRevaluation, bool isManualRevaluation)
        {
            try
            {
                _persistenceManager.UpdateLastRevaluationCalculatedDateToGivenDate(CMStartDate, endDate, accountIDs, isUpdated, fromRevaluation, isManualRevaluation);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// added by: Bharat raturi, 1 jul 2014, create the dictionary to hold the cash preferences 
        /// </summary>
        static Dictionary<int, CashPreferences> _dictCashPreferences;

        /// <summary>
        /// Added by: Bharat Raturi, 1 jul 2014
        /// purpose: Get the dictionary of accountID-cash preferences pairs
        /// </summary>
        /// <returns>dictionary of accountID-cash preferences pairs</returns>
        public static Dictionary<int, CashPreferences> GetCashPreferences()
        {
            try
            {
                if (_dictCashPreferences == null)
                    _dictCashPreferences = CashDataManager.GetCashPreferencesFromDB();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _dictCashPreferences;
        }

        /// <summary>
        /// Added by: Bharat raturi, 1 jul 2014
        /// purpose: Get the dictionary of the accountID-CashPreference pairs from DB
        /// </summary>
        /// <returns>dictionary of the accountID-CashPreference pairs from DB</returns>
        internal static Dictionary<int, CashPreferences> GetCashPreferencesFromDB()
        {
            Dictionary<int, CashPreferences> dictCashPreferences = new Dictionary<int, CashPreferences>();
            try
            {
                string query = "FROM CashPreferences";
                List<CashPreferences> lsData = GetDataFromDb<CashPreferences>(query);
                if (lsData.Count > 0)
                {
                    foreach (CashPreferences item in lsData)
                    {
                        int accountID = item.AccountID;
                        if (!dictCashPreferences.ContainsKey(accountID))
                        {
                            dictCashPreferences.Add(accountID, item);
                        }
                    }
                }
                #region Temporary Seted To True Becouse of some further Enhancement

                //ObjCashPreferences.IsCalculateBondAccurals = true;
                //ObjCashPreferences.IsCalculateDividend = true;
                //ObjCashPreferences.IsCalculatePnL = true;

                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dictCashPreferences;
        }

        /// <summary>
        /// Gets the day end cash for datewise currency identifier.
        /// </summary>
        /// <param name="dicDateWiseCurrencyID">The dic date wise currency identifier.</param>
        /// <returns></returns>
        internal static List<CompanyAccountCashCurrencyValue> getDayEndCashForDatewiseCurrencyID(Dictionary<string, List<int>> dicDateWiseCurrencyID)
        {
            List<CompanyAccountCashCurrencyValue> lsDayEndCash = new List<CompanyAccountCashCurrencyValue>();
            try
            {
                StringBuilder query = new StringBuilder("from CompanyAccountCashCurrencyValue where");
                bool isfirstTimeForDateLoop = true;

                //commented and updated to solve SEA-17 and PRANA-1836
                //foreach (string date in dicDateWiseCurrencyID.Keys)
                //{
                //    bool isfirstTimeForCurrencyLoop = true;
                //    if (!isfirstTimeForDateLoop)
                //        query.Append(" And ");
                //    query.Append("(datediff(dd,Date,'" + date + "')=0 And LocalCurrencyID IN (");
                //    foreach (int localCurrencyID in dicDateWiseCurrencyID[date])
                //    {
                //        if (!isfirstTimeForCurrencyLoop)
                //            query.Append(",");
                //        query.Append(localCurrencyID);
                //        isfirstTimeForCurrencyLoop = false;
                //    }
                //    query.Append("))");
                //    isfirstTimeForDateLoop = false;
                //}
                foreach (string date in dicDateWiseCurrencyID.Keys)
                {
                    if (!isfirstTimeForDateLoop)
                        query.Append(" OR ");
                    query.Append(" (datediff(dd,Date,'" + date + "')=0)");
                    isfirstTimeForDateLoop = false;
                }
                lsDayEndCash = GetDataFromDb<CompanyAccountCashCurrencyValue>(query.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsDayEndCash;
        }

        /// <summary>
        /// Gets the cash dividend from activities.
        /// </summary>
        /// <param name="auecDateString">The auec date string.</param>
        /// <returns></returns>
        internal static DataSet GetCashDividendFromActivities(string auecDateString)
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCashDividendFromActivities";
            queryData.DictionaryDatabaseParameter.Add("@AUECDateString", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@AUECDateString",
                ParameterType = DbType.String,
                ParameterValue = auecDateString
            });

            DataSet ds = null;

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
        /// Gets the cash dividends.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="commaSeparatedAccountIds">The comma separated account ids.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns></returns>
        internal static DataSet GetCashDividends(string symbol, string commaSeparatedAccountIds, string dateType, DateTime dateFrom, DateTime dateTo)
        {
            DataSet ds = new DataSet();

            try
            {
                string[] tables = { "CashDividends" };
                object[] param = new object[5];
                param[0] = symbol;
                param[1] = commaSeparatedAccountIds;
                param[2] = dateType;
                param[3] = dateFrom;
                param[4] = dateTo;
                DatabaseManager.DatabaseManager.LoadDataSet("P_GetCashTransactions", ds, tables, param);
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
        /// Calculate the Sub account balances account-wise
        /// </summary>
        /// <param name="userID">ID of the currently logged in user</param>
        /// <param name="accountIDs">Comma separated account ids of the selected accounts</param>
        public static void CalculateAndSaveBalances(DateTime endDate, int userID, string accountIDs)
        {
            try
            {
                DataSet ds = new DataSet();
                CreatePublishingProxy();
                DateTime StartDayOfAccrualDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(false);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CalculateAndSaveSubAccountBalances";
                queryData.DictionaryDatabaseParameter.Add("@userID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@userID",
                    ParameterType = DbType.Int32,
                    ParameterValue = userID
                });
                queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fundIDs",
                    ParameterType = DbType.String,
                    ParameterValue = accountIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@startDayOfAccrualDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@startDayOfAccrualDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = StartDayOfAccrualDate
                });
                queryData.DictionaryDatabaseParameter.Add("@endBalDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@endBalDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = endDate
                });

                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3268
                //Time out on trade server while fetching sub account balances
                //timeout set to 2 Days
                queryData.CommandTimeout = 172800;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds.Tables.Count > 0)
                {
                    List<CompanyAccountCashCurrencyValue> lstAccruals = new List<CompanyAccountCashCurrencyValue>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CompanyAccountCashCurrencyValue newAccounts = new CompanyAccountCashCurrencyValue();
                        newAccounts.AccountID = int.Parse(dr["FundID"].ToString());
                        newAccounts.CashValueBase = Convert.ToDecimal(dr["Cash"].ToString());
                        newAccounts.LocalCurrencyID = int.Parse(dr["CurrencyID"].ToString());
                        lstAccruals.Add(newAccounts);
                    }
                    PublishStartDayOfAccruals(lstAccruals);
                }
                //If cache is null then fill data in cache from DB
                //In SP P_CalculateAndSaveSubAccountBalances table T_LastCalculatedBalanceDate is filled
                //So to make cache in sync it should be filled
                //_persistenceManager.UpdateLastCalcBalanceDateInCache(DateTime.Now.Date);
                _persistenceManager.UpdateLastCalcBalanceDateInCache(accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Add Symbol Level Accruals Data
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="userID"></param>
        /// <param name="accountIDs"></param>
        public static void AddSymbolLevelAccrualsData(Nullable<DateTime> startDate, DateTime endDate, int userID, string accountIDs, bool isManualReval)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_AddSymbolLevelAccrualsData";

                queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = startDate
                });
                queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@EndDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = endDate
                });
                queryData.DictionaryDatabaseParameter.Add("@userID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@userID",
                    ParameterType = DbType.String,
                    ParameterValue = userID
                });
                queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fundIDs",
                    ParameterType = DbType.String,
                    ParameterValue = accountIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@IsManualRevaluation", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsManualRevaluation",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isManualReval
                });

                //Time out on trade server while Adding SymbolLevel Accruals Data
                //timeout set to 3 min
                queryData.CommandTimeout = 172800;
                DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Modified by: Bharat raturi, 31 jul 2014
        /// pass the comma separated accountIDs as the parameter to get the balances only for specified accounts
        /// </summary>
        /// <param name="balDate">the date till which the balances are to be gotten</param>
        /// <param name="accountIDs">comma separated string of accountIDs</param>
        /// <returns></returns>
        public static DataSet GetAccountBalancesAsOnDate(DateTime balDate, string accountIDs)
        {
            DataSet ds = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSubAccountBalancesForDate";
                queryData.DictionaryDatabaseParameter.Add("@date", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@date",
                    ParameterType = DbType.DateTime,
                    ParameterValue = balDate
                });

                //Added by: Bharat raturi, 31 jul 2014
                //added parameter to pass the comma separated accountIDs to get the balances only for the sub accounts of the selected accounts
                queryData.DictionaryDatabaseParameter.Add("@fundIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@fundIDs",
                    ParameterType = DbType.String,
                    ParameterValue = accountIDs
                });
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3268
                //Time out on trade server while fetching sub account balances
                //timeout set to 3 min
                queryData.CommandTimeout = 180;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
        /// Gets the sub account transaction entries for date range.
        /// </summary>
        /// <param name="subAccountID">The sub account identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public static DataSet GetSubAccountTransactionEntriesForDateRange(int subAccountID, DateTime fromDate, DateTime toDate)
        {
            DataSet ds = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetSubAccTransactionEntriesForDateRange";
                queryData.DictionaryDatabaseParameter.Add("@subAccountID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@subAccountID",
                    ParameterType = DbType.Int32,
                    ParameterValue = subAccountID
                });
                queryData.DictionaryDatabaseParameter.Add("@startDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@startDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@endDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@endDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toDate
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
        /// Updates the cash accounts tables in database.
        /// </summary>
        /// <param name="updatedDataSet">The updated data set.</param>
        /// <returns></returns>
        internal static int UpdateCashAccountsTablesInDB(DataSet updatedDataSet)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtDeletedSubAccounts = updatedDataSet.Tables["SubCashAccounts"].GetChanges(DataRowState.Deleted);
                if (dtDeletedSubAccounts != null && dtDeletedSubAccounts.Rows.Count > 0)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubAccounts", dtDeletedSubAccounts);
                    rowsAffected++;
                }

                DataTable dtAddedSubAccounts = updatedDataSet.Tables["SubCashAccounts"].GetChanges(DataRowState.Added);
                if (dtAddedSubAccounts != null)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubAccounts", dtAddedSubAccounts);
                    rowsAffected++;
                }

                DataTable dtModifiedSubAccounts = updatedDataSet.Tables["SubCashAccounts"].GetChanges(DataRowState.Modified);
                if (dtModifiedSubAccounts != null)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubAccounts", dtModifiedSubAccounts);
                    rowsAffected++;
                }

                //For Update SubCategory
                DataTable dtDeletedSubCategory = updatedDataSet.Tables["SubCategory"].GetChanges(DataRowState.Deleted);
                if (dtDeletedSubCategory != null && dtDeletedSubCategory.Rows.Count > 0)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubCategory", dtDeletedSubCategory);
                    rowsAffected++;
                }

                DataTable dtAddedSubCategory = updatedDataSet.Tables["SubCategory"].GetChanges(DataRowState.Added);
                if (dtAddedSubCategory != null)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubCategory", dtAddedSubCategory);
                    rowsAffected++;
                }

                DataTable dtModifiedSubCategory = updatedDataSet.Tables["SubCategory"].GetChanges(DataRowState.Modified);
                if (dtModifiedSubCategory != null)
                {
                    DatabaseManager.DatabaseManager.Update("SELECT * FROM T_SubCategory", dtModifiedSubCategory);
                    rowsAffected++;
                }
                updatedDataSet.AcceptChanges();
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
        /// Updates the cash activity tables in database.
        /// </summary>
        /// <param name="updatedDataSet">The updated data set.</param>
        /// <returns></returns>
        internal static int UpdateCashActivityTablesInDB(DataSet updatedDataSet)
        {

            int rowsAffected = 0;
            try
            {
                SqlCommand command = new SqlCommand();
                for (int i = 0; i < 4; i++)
                {
                    #region save Data
                    //for activity types
                    switch (i)
                    {
                        case 0:
                            command.CommandText = "SELECT ActivityTypeId, ActivityType,Description,BalanceType,ActivitySource,Acronym FROM T_ActivityType";
                            rowsAffected += UpdateDBCashActivityTables(updatedDataSet.Tables["ActivityType"], command);
                            break;
                        case 1:
                            command.CommandText = "SELECT AmountTypeId, AmountType FROM T_ActivityAmountType";
                            rowsAffected += UpdateDBCashActivityTables(updatedDataSet.Tables["AmountType"], command);
                            break;
                        case 2:
                            command.CommandText = "SELECT ActivityTypeId_FK,AmountTypeId_FK, DebitAccount,CreditAccount,CashValueType,ActivityDateType,Id FROM T_ActivityJournalMapping";
                            rowsAffected += UpdateDBCashActivityTables(updatedDataSet.Tables["ActivityJournalMapping"], command);
                            break;
                        case 3:
                            command.CommandText = "select SubAccountTypeId,SubAccountType from T_SubAccountType";
                            rowsAffected += UpdateDBCashActivityTables(updatedDataSet.Tables["SubAccountType"], command);
                            break;
                        default:
                            break;
                    }

                    #endregion
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

        private static int UpdateDBCashActivityTables(DataTable tableToUpdate, SqlCommand command)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtDeletedActivity = tableToUpdate.GetChanges(DataRowState.Deleted);
                if (dtDeletedActivity != null && dtDeletedActivity.Rows.Count > 0)
                {
                    DatabaseManager.DatabaseManager.Update(command.CommandText, dtDeletedActivity);
                    rowsAffected++;
                }

                DataTable dtAddedActivity = tableToUpdate.GetChanges(DataRowState.Added);
                if (dtAddedActivity != null)
                {
                    DatabaseManager.DatabaseManager.Update(command.CommandText, dtAddedActivity);
                    rowsAffected++;
                }

                DataTable dtModifiedActivity = tableToUpdate.GetChanges(DataRowState.Modified);
                if (dtModifiedActivity != null)
                {
                    DatabaseManager.DatabaseManager.Update(command.CommandText, dtModifiedActivity);
                    rowsAffected++;
                }
            }
            catch (SqlException sqlException)
            {
                if (tableToUpdate != null && !tableToUpdate.TableName.Equals("SubAccountType"))
                {
                    bool rethrow = Logger.HandleException(sqlException, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                        throw;
                }
            }
            catch (DBConcurrencyException dbConcurrencyException)
            {
                if (tableToUpdate != null && !tableToUpdate.TableName.Equals("SubAccountType"))
                {
                    bool rethrow = Logger.HandleException(dbConcurrencyException, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                        throw;
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
        /// Modified By: Narendra Kumar Jangir, July 24, 2013
        /// Description: Dataset is passed with reference because new column ActivityTypeId is added.
        /// </summary>
        /// <param name="modifiedData"></param>
        /// <returns></returns>
        internal static DataSet UpdateCashDividendsInDB(DataSet modifiedData)
        {
            DataSet dataChanges = null;
            try
            {
                if (modifiedData != null && modifiedData.Tables[0] != null)
                {
                    #region  get CashTransactionId of deleted and modified data
                    //table name set as table so that in case of import different name table can be renamed 
                    modifiedData.Tables[0].TableName = "Table";

                    if (!modifiedData.Tables[0].Columns.Contains("CashTransactionId"))
                        modifiedData.Tables[0].Columns.Add("CashTransactionId", typeof(System.Int64));
                    DataSet newlyAddedData = modifiedData.Clone();
                    dataChanges = modifiedData.Clone();
                    //commaSeperatedValues will have cash transaction id of deleted and modified rows 
                    string commaSeperatedValues = string.Empty;
                    foreach (DataRow dr in modifiedData.Tables[0].Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Modified)
                        {
                            commaSeperatedValues += dr["CashTransactionId", DataRowVersion.Original].ToString() + ',';
                        }
                        else
                        {
                            newlyAddedData.Tables[0].Rows.Add(dr.ItemArray);
                        }
                    }
                    #endregion

                    #region create dictionary for all the neccesary column to insert
                    //cashTransactionColumns will have all the neccesary column to insert into the table . so that we can check in the modified table have all the columns as in cashTransactionColumns.
                    Dictionary<string, System.Type> cashTransactionColumns = new Dictionary<string, System.Type>() { { "CashTransactionId", typeof(System.Int64) }, { "TaxlotId", typeof(System.String) }, { "FundID", typeof(System.Int64) }, { "Level2Id", typeof(System.Int64) }, { "Symbol", typeof(System.String) }, { "Amount", typeof(System.Double) }, { "PayoutDate", typeof(System.DateTime) }, { "ExDate", typeof(System.DateTime) }, { "CurrencyID", typeof(System.Int64) }, { "ActivityTypeId", typeof(System.Int64) }, { "RecordDate", typeof(System.DateTime) }, { "DeclarationDate", typeof(System.DateTime) }, { "Description", typeof(System.String) }, { "OtherCurrencyID", typeof(System.Int64) }, { "FXRate", typeof(System.Double) }, { "FXConversionMethodOperator", typeof(System.String) }, { "TransactionSource", typeof(System.String) }, { "ModifyDate", typeof(System.DateTime) }, { "EntryDate", typeof(System.DateTime) } };
                    #endregion

                    #region Update deleted and modified rows in DB
                    if (!string.IsNullOrEmpty(commaSeperatedValues))
                    {
                        commaSeperatedValues = commaSeperatedValues.Substring(0, commaSeperatedValues.Length - 1);

                        SqlConnection conn = null;
                        SqlDataAdapter adapterCashDividends = DatabaseManager.DatabaseManager.CreateSqlDataAdapter(ref conn);

                        using (conn)
                        {
                            using (adapterCashDividends)
                            {
                                conn.Open();
                                adapterCashDividends.AcceptChangesDuringUpdate = false;

                                adapterCashDividends.SelectCommand = new SqlCommand("SELECT CashTransactionId,TaxlotId,FundID,Level2Id,Symbol,Amount,PayoutDate,ExDate,CurrencyID,ActivityTypeId,RecordDate,DeclarationDate,Description,OtherCurrencyID,FXRate,FXConversionMethodOperator,TransactionSource, ModifyDate, EntryDate FROM T_CashTransactions where CashTransactionId in (Select Items as FundID from dbo.Split(@commaSeperatedValues,','))", conn);
                                adapterCashDividends.SelectCommand.Parameters.Add(new SqlParameter("@commaSeperatedValues", SqlDbType.VarChar, 0));
                                adapterCashDividends.SelectCommand.Parameters["@commaSeperatedValues"].Value = commaSeperatedValues;

                                adapterCashDividends.DeleteCommand = new SqlCommand("delete from T_CashTransactions where CashTransactionId =@CashTransactionId", conn);
                                adapterCashDividends.DeleteCommand.Parameters.Add(new SqlParameter("@CashTransactionId", SqlDbType.BigInt, 0, "CashTransactionId"));
                                adapterCashDividends.DeleteCommand.UpdatedRowSource = UpdateRowSource.Both;
                                adapterCashDividends.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                                DataSet dsDiv = modifiedData.Clone();
                                adapterCashDividends.Fill(dsDiv);

                                dsDiv.Tables[0].PrimaryKey = new DataColumn[] { dsDiv.Tables[0].Columns["CashTransactionId"] };
                                if (!dsDiv.Tables[0].Columns.Contains("RowStateType"))
                                    dsDiv.Tables[0].Columns.Add("RowStateType", typeof(System.String));
                                UpdateDividendsInDb(modifiedData, dsDiv);
                                dataChanges = dsDiv.GetChanges();
                                if (dataChanges != null && dataChanges.Tables.Count > 0)
                                {
                                    lock (_updateLock)
                                    {
                                        adapterCashDividends.Update(dataChanges);
                                    }

                                    dataChanges.Tables[0].PrimaryKey = new DataColumn[] { dataChanges.Tables[0].Columns["CashTransactionId"] };
                                }
                                else if(dataChanges == null)
                                {
                                    throw new ArgumentNullException(nameof(dataChanges));
                                }
                            }
                        }
                    }
                    #endregion

                    #region Update newly added data in the db
                    if (newlyAddedData.Tables[0].Rows.Count > 0)
                    {
                        SqlConnection conn = null;
                        SqlDataAdapter adapterCashDividends = DatabaseManager.DatabaseManager.CreateSqlDataAdapter(ref conn);

                        using (conn)
                        {
                            using (adapterCashDividends)
                            {
                                conn.Open();
                                adapterCashDividends.AcceptChangesDuringUpdate = false;
                                //Code : To have the identity column on insert at data set level
                                //TODO; Here ActivityTypeId is hardcoded, make it generic
                                //PRANA-9777
                                adapterCashDividends.InsertCommand = new SqlCommand(
                                           "INSERT INTO dbo.T_CashTransactions (FundID,Level2Id,Symbol,Amount,PayoutDate,ExDate,CurrencyID,ActivityTypeId,RecordDate,DeclarationDate,Description,OtherCurrencyID,FXRate,FXConversionMethodOperator,TransactionSource, ModifyDate, EntryDate, TaxlotId, UserId) " +
                                           "VALUES (@FundID,@Level2Id,@Symbol,@Amount,@PayoutDate,@ExDate,@CurrencyID,@ActivityTypeId,@RecordDate,@DeclarationDate,@Description,@OtherCurrencyID,@FXRate,@FXConversionMethodOperator,@TransactionSource, @ModifyDate, @EntryDate, @TaxlotId ,@UserId);" +
                        "SELECT CashTransactionId,FundID,Level2Id,Symbol,Amount,PayoutDate,ExDate,CurrencyID,ActivityTypeId,RecordDate,DeclarationDate,Description,OtherCurrencyID,FXRate,FXConversionMethodOperator,TransactionSource,ModifyDate,EntryDate,TaxlotId ,UserId from dbo.T_CashTransactions " +
                        "WHERE CashTransactionId = SCOPE_IDENTITY();", conn);

                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@FundID", SqlDbType.Int, 0, "FundID"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@Level2Id", SqlDbType.Int, 0, "Level2Id"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@Symbol", SqlDbType.VarChar, 50, "Symbol"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Float, 0, "Amount"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@PayoutDate", SqlDbType.DateTime, 0, "PayoutDate"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@ExDate", SqlDbType.DateTime, 0, "ExDate"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.Int, 0, "CurrencyID"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@ActivityTypeId", SqlDbType.Int, 0, "ActivityTypeId"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@RecordDate", SqlDbType.DateTime, 0, "RecordDate"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@DeclarationDate", SqlDbType.DateTime, 0, "DeclarationDate"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 0, "Description"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@OtherCurrencyID", SqlDbType.Int, 0, "OtherCurrencyID"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@FXRate", SqlDbType.Float, 0, "FXRate"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@FXConversionMethodOperator", SqlDbType.VarChar, 0, "FXConversionMethodOperator"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@TransactionSource", SqlDbType.VarChar, 100, "TransactionSource"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@TaxlotId", SqlDbType.VarChar, 50, "TaxlotId"));
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@ModifyDate", SqlDbType.DateTime, 0, "ModifyDate")); //PRANA-9777                        
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@EntryDate", SqlDbType.DateTime, 0, "EntryDate")); //PRANA-9777                        
                                adapterCashDividends.InsertCommand.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 0, "UserId"));
                                adapterCashDividends.InsertCommand.UpdatedRowSource = UpdateRowSource.Both;

                                adapterCashDividends.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                                //Int64 UniqueId = 1;
                                //SqlCommand Identity = new SqlCommand("SELECT IDENT_CURRENT('T_CashTransactions')", conn);
                                //UniqueId = Convert.ToInt64(db.ExecuteScalar(Identity));

                                foreach (var pair in cashTransactionColumns)
                                {
                                    if (!newlyAddedData.Tables[0].Columns.Contains(pair.Key))
                                    {
                                        newlyAddedData.Tables[0].Columns.Add(pair.Key, pair.Value);
                                    }
                                }

                                if (!newlyAddedData.Tables[0].Columns.Contains("RowStateType"))
                                    newlyAddedData.Tables[0].Columns.Add("RowStateType", typeof(System.String));

                                foreach (DataRow dr in newlyAddedData.Tables[0].Rows)
                                {
                                    dr["TransactionSource"] = string.IsNullOrEmpty(dr["TransactionSource"].ToString()) ? '5' : dr["TransactionSource"]; //Transaction Source 'CashTransaction'
                                    dr["RowStateType"] = "Added";
                                    //No need of UniqueId because it will automatically add CashTransactionId
                                    //      dr["CashTransactionId"] = ++UniqueId;                               
                                    DateTime tempRecordDate;
                                    dr["RecordDate"] = (DateTime.TryParse(Convert.ToString(dr["RecordDate"]), out tempRecordDate) ? tempRecordDate : (object)DBNull.Value);
                                    DateTime tempDeclarationDate;
                                    dr["DeclarationDate"] = (DateTime.TryParse(Convert.ToString(dr["DeclarationDate"]), out tempDeclarationDate) ? tempDeclarationDate : (object)DBNull.Value);
                                    DateTime tempPayOutDate;
                                    dr["PayoutDate"] = (DateTime.TryParse(Convert.ToString(dr["PayoutDate"]), out tempPayOutDate) ? tempPayOutDate : (object)DBNull.Value);
                                    dr["Symbol"] = dr["Symbol"] == DBNull.Value ? string.Empty : dr["Symbol"];
                                    if (dr.Table.Columns["Date"] != null)
                                    {
                                        if (dr["PayoutDate"] == DBNull.Value)
                                            dr["PayoutDate"] = dr["Date"];
                                        if (dr["ExDate"] == DBNull.Value)
                                            dr["ExDate"] = dr["Date"];
                                        if (dr["RecordDate"] == DBNull.Value)
                                            dr["RecordDate"] = dr["Date"];
                                        if (dr["DeclarationDate"] == DBNull.Value)
                                            dr["DeclarationDate"] = dr["Date"];
                                    }
                                    dr["ModifyDate"] = DateTime.Now;
                                    dr["EntryDate"] = dr["EntryDate"] != DBNull.Value && !(string.IsNullOrEmpty(dr["EntryDate"].ToString())) ? dr["EntryDate"] : DateTime.Now;

                                    //currency id is not present/invalid and currency name is present in table
                                    if (dr.Table.Columns.Contains("CurrencyName"))
                                        dr["CurrencyID"] = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dr["CurrencyName"].ToString());
                                    //currency name is not available in cache/ fill default currency id
                                    if ((string.IsNullOrEmpty(dr["CurrencyID"].ToString())) || (int.Parse(dr["CurrencyID"].ToString()) <= 0))
                                        dr["CurrencyID"] = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                                }
                                lock (_updateLock)
                                {
                                    adapterCashDividends.Update(newlyAddedData);
                                }

                                newlyAddedData.Tables[0].PrimaryKey = new DataColumn[] { newlyAddedData.Tables[0].Columns["CashTransactionId"] };

                                #endregion

                                #region update final data set for return with updated rows

                                if (dataChanges != null && dataChanges.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in newlyAddedData.Tables[0].Rows)
                                    {
                                        dataChanges.Tables[0].Rows.Add(dr.ItemArray);
                                    }
                                }
                                else
                                    dataChanges = newlyAddedData.Copy();
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataChanges;
        }

        /// <summary>
        /// update Dividends In Db of row state type deleted and modified 
        /// </summary>
        /// <param name="modifiedData"></param>
        /// <param name="adapterCashDividends"></param>
        /// <param name="dsDiv"></param>
        private static void UpdateDividendsInDb(DataSet modifiedData, DataSet dsDiv)
        {
            try
            {
                if (modifiedData.Tables[0] != null)
                {
                    foreach (DataRow dr in modifiedData.Tables[0].Rows)
                    {
                        DataRow row = null;
                        if (dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Modified)
                        {
                            row = dsDiv.Tables[0].Rows.Find(dr["CashTransactionId", DataRowVersion.Original]);
                        }

                        if (row != null)
                        {
                            if (dr.RowState == DataRowState.Deleted)
                            {
                                row.Delete();
                            }
                            else
                            {
                                row["FundID"] = dr.Table.Columns.Contains("FundID") ? dr["FundID"] : DBNull.Value;
                                row["Level2Id"] = dr.Table.Columns.Contains("Level2Id") ? dr["Level2Id"] : DBNull.Value;
                                row["Symbol"] = (dr.Table.Columns.Contains("Symbol") && dr["Symbol"] != DBNull.Value) ? dr["Symbol"] : string.Empty;
                                row["Amount"] = dr.Table.Columns.Contains("Amount") ? dr["Amount"] : DBNull.Value;
                                //whenever cash transaction is entered using dividend ui then cash transaction type will be supplied by user from that ui.
                                if (dr.Table.Columns["ActivityTypeId"] != null)
                                    row["ActivityTypeId"] = dr["ActivityTypeId"];
                                else
                                    continue;
                                row["PayoutDate"] = dr.Table.Columns.Contains("PayoutDate") ? dr["PayoutDate"] : DBNull.Value;
                                row["ExDate"] = dr.Table.Columns.Contains("ExDate") ? dr["ExDate"] : DBNull.Value;
                                DateTime tempRecordDate;
                                row["RecordDate"] = dr.Table.Columns.Contains("RecordDate") ? (DateTime.TryParse(Convert.ToString(dr["RecordDate"]), out tempRecordDate) ? tempRecordDate : (object)DBNull.Value) : DBNull.Value;
                                DateTime tempDeclarationDate;
                                row["DeclarationDate"] = dr.Table.Columns.Contains("DeclarationDate") ? (DateTime.TryParse(Convert.ToString(dr["DeclarationDate"]), out tempDeclarationDate) ? tempDeclarationDate : (object)DBNull.Value) : DBNull.Value;

                                if (dr.Table.Columns["Date"] != null)
                                {
                                    if (row["PayoutDate"] == DBNull.Value)
                                        row["PayoutDate"] = dr["Date"];
                                    if (row["ExDate"] == DBNull.Value)
                                        row["ExDate"] = dr["Date"];
                                    if (row["RecordDate"] == DBNull.Value)
                                        row["RecordDate"] = dr["Date"];
                                    if (row["DeclarationDate"] == DBNull.Value)
                                        row["DeclarationDate"] = dr["Date"];
                                }

                                row["Description"] = dr.Table.Columns.Contains("Description") ? (Convert.ToString(dr["Description"]).Length != 0 ? dr["Description"] : DBNull.Value) : DBNull.Value;

                                //currency id is present in table and currency is is not empty/null or 0
                                if (dr.Table.Columns.Contains("CurrencyID") && !((string.IsNullOrEmpty(dr["CurrencyID"].ToString())) || (int.Parse(dr["CurrencyID"].ToString()) <= 0)))
                                    row["CurrencyID"] = dr["CurrencyID"];
                                //currency id is not present/invalid and currency name is present in table
                                else if (dr.Table.Columns.Contains("CurrencyName"))
                                    row["CurrencyID"] = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dr["CurrencyName"].ToString());

                                //currency name is not available in cache/ fill default currency id
                                if ((string.IsNullOrEmpty(row["CurrencyID"].ToString())) || (int.Parse(row["CurrencyID"].ToString()) <= 0))
                                    row["CurrencyID"] = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                                row["OtherCurrencyID"] = dr.Table.Columns["OtherCurrencyID"] != null && !(string.IsNullOrEmpty(dr["OtherCurrencyID"].ToString())) ? Convert.ToInt32(dr["OtherCurrencyID"]) : (object)DBNull.Value;
                                row["FXRate"] = dr.Table.Columns["FXRate"] != null && !(string.IsNullOrEmpty(dr["FXRate"].ToString())) ? Convert.ToDouble(dr["FXRate"]) : (object)DBNull.Value;
                                row["FxConversionMethodOperator"] = (dr.Table.Columns["FxConversionMethodOperator"] != null && !(string.IsNullOrEmpty(dr["FxConversionMethodOperator"].ToString()))) ? Convert.ToString(dr["FxConversionMethodOperator"]) : (object)DBNull.Value;

                                if (dr.Table.Columns.Contains("TransactionSource") && dr.Table.Columns["TransactionSource"] != null && !(string.IsNullOrEmpty(dr["TransactionSource"].ToString())))
                                    row["TransactionSource"] = dr["TransactionSource"];
                                else if (dr.Table.Columns.Contains("TransactionSource"))
                                    row["TransactionSource"] = '5'; //Transaction Source 'CashTransaction'

                                if (dr.Table.Columns.Contains("TaxlotId") && dr.Table.Columns["TaxlotId"] != null && !(string.IsNullOrEmpty(dr["TaxlotId"].ToString())))
                                    row["TaxlotId"] = dr["TaxlotId"];
                                else if (dr.Table.Columns.Contains("TaxlotId"))
                                    row["TaxlotId"] = DBNull.Value;

                                //PRANA-9777
                                row["ModifyDate"] = DateTime.Now;

                                //PRANA-9777
                                row["EntryDate"] = dr.Table.Columns.Contains("EntryDate") && dr.Table.Columns["EntryDate"] != null && !(string.IsNullOrEmpty(dr["EntryDate"].ToString())) ? dr["EntryDate"] : DateTime.Now;
                            }
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
        }

        /// <summary>
        /// modified by: Bharat raturi, 31 jul 2014
        /// Get the journal exceptions of selected accounts from the database
        /// </summary>
        /// <typeparam name="T">Type of data object</typeparam>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <param name="accountIDs">comma separated IDs of the accounts </param>
        /// <returns>List of data objects</returns>
        public static List<T> GetJournalExceptions<T>(DateTime startDate, DateTime endDate, string accountIDs)
        {
            List<T> lsJournalException = new List<T>();
            try
            {
                lsJournalException = _persistenceManager.GetJournalExceptionDataFromDb<T>(startDate, endDate, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsJournalException;
        }

        /// <summary>
        /// Modified by: Bharat Raturi, 7 aug 2014
        /// This method get asset revaluation data
        /// fromDate is set as Nullable Datetime as for Normal Revaluation we send fromDate as null.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="fundIDs">The fund i ds.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="isManualReval">if set to <c>true</c> [is manual reval].</param>
        /// <returns></returns>
        internal static DataSet GetAssetRevaluationData(Nullable<DateTime> fromDate, DateTime toDate, string fundIDs, int userID, bool isManualReval)
        {
            bool isIncludeCommission = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsIncludeCommission"));
            bool isIncludeFXPNLonSwapForDiffSettleAndBaseCurr = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsIncludeFXPNLonSwapForDiffSettleAndBaseCurr"));
            bool isIncludeFXPNLonSwapForSameSettleAndBaseCurr = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsIncludeFXPNLonSwapForSameSettleAndBaseCurr"));
            CreatePublishingProxy();
            DataSet ds = null;
            try
            {
                SqlConnection conn = null;

                //timeout set to 2 days
                SqlCommand cmd = DatabaseManager.DatabaseManager.CreateSqlCommand(ref conn, "P_RunDailyAssetRevaluation", 172800, conn_InfoMessage);

                using (conn)
                {
                    if (fromDate == null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@StartDate", DBNull.Value));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@StartDate", fromDate));
                    }
                    cmd.Parameters.Add(new SqlParameter("@EndDate", toDate));
                    cmd.Parameters.Add(new SqlParameter("@FundIDs", fundIDs));
                    cmd.Parameters.Add(new SqlParameter("@userID", userID));
                    cmd.Parameters.Add(new SqlParameter("@isincludecommission", isIncludeCommission));
                    cmd.Parameters.Add(new SqlParameter("@isIncludeFXPNLonSwapForDiffSettleAndBaseCurr", isIncludeFXPNLonSwapForDiffSettleAndBaseCurr));
                    cmd.Parameters.Add(new SqlParameter("@isIncludeFXPNLonSwapForSameSettleAndBaseCurr", isIncludeFXPNLonSwapForSameSettleAndBaseCurr));
                    cmd.Parameters.Add(new SqlParameter("@IsManualRevaluation", isManualReval));

                    SqlDataAdapter da = DatabaseManager.DatabaseManager.CreateSqlDataAdapter(cmd);
                    conn.Open();

                    // Create a DataTable and fill it.     
                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return ds;
        }

        /// </summary>
        /// Get the PreferenceValueFromKey
        /// <param name="PreferenceKey"></param>
        /// <returns>Valid Fund Ids</returns>
        internal static string GetEligibleFundsForSymbolWiseAccrual(string accountsIds, Nullable<DateTime> endDate)
        {
            string fundIds = string.Empty;
            try
            {
                foreach (string fundId in accountsIds.Split(','))
                {
                    int fund = Convert.ToInt32(fundId);
                    if (_dictCashPreferences.ContainsKey(fund) && _dictCashPreferences[fund].SymbolWiseRevaluationDate != DateTime.MinValue && _dictCashPreferences[fund].SymbolWiseRevaluationDate <= endDate)
                    {
                        fundIds += fundId + ",";
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
            return fundIds;
        }

        /// <summary>
        /// The proxy publishing
        /// </summary>
        static ProxyBase<IPublishing> _proxyPublishing;

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private static void CreatePublishingProxy()
        {
            try
            {
                if (_proxyPublishing == null)
                    _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the revaluation progress.
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <param name="userID">The user identifier.</param>
        private static void PublishRevaluationProgress(int percent, int userID)
        {
            try
            {
                MessageData e1 = new MessageData();
                List<int> progress = new List<int>();
                progress.Add(userID);
                progress.Add(percent);
                e1.EventData = progress;
                e1.TopicName = Topics.Topic_RevaluationProgress;
                CentralizePublish(e1);
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the revaluation progress message.
        /// </summary>
        /// <param name="progressMsg">The progress MSG.</param>
        /// <param name="userID">The user identifier.</param>
        public static void PublishRevaluationProgressMessage(string progressMsg, int userID)
        {
            try
            {
                MessageData e1 = new MessageData();
                List<string> progress = new List<string>();
                progress.Add(userID.ToString());
                progress.Add(progressMsg);
                e1.EventData = progress;
                e1.TopicName = Topics.Topic_RevaluationProgress;
                CentralizePublish(e1);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the InfoMessage event of the conn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlInfoMessageEventArgs"/> instance containing the event data.</param>
        private static void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            try
            {
                int number;
                string message = e.Message;
                //The message that we receive from the currently executing stored procedure, is in the following format: Progress$UserID
                // So we split the message based on $ and sent it as different parameters on client side.
                string[] parameters = message.Split('$');
                if (parameters.Length > 1)
                {
                    bool result = Int32.TryParse(parameters[0], out number);
                    if (result)
                    {
                        PublishRevaluationProgress(number, int.Parse(parameters[1]));
                    }
                    else
                    {
                        PublishRevaluationProgressMessage(parameters[0], int.Parse(parameters[1]));
                    }
                    if (parameters.Length > 2 && parameters[2] == "1")
                    {
                        Logger.LoggerWrite("Run Revaluation Status:" + message + ", Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Publishes the start day of accruals.
        /// </summary>
        /// <param name="startDayOfAccruals">The start day of accruals.</param>
        private static void PublishStartDayOfAccruals(List<CompanyAccountCashCurrencyValue> startDayOfAccruals)
        {
            try
            {
                MessageData e = new MessageData();
                e.EventData = startDayOfAccruals;
                e.TopicName = Topics.Topic_StartDayOfAccrual;
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// The publish lock
        /// </summary>
        private static readonly object _publishLock = new object();
        /// <summary>
        /// Centralizes the publish.
        /// </summary>
        /// <param name="msgData">The MSG data.</param>
        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            if (rethrow)
                                throw;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This method get cash and accruals revaluation data
        /// </summary>
        /// <param name="endDate"></param>
        /// <returns></returns>
        internal static DataSet GetCashAccrualsRevaluationData(Nullable<DateTime> fromDate, DateTime endDate, string fundIDs, int userID, bool isManualReval)
        {
            DataSet ds = null;
            try
            {
                string spName = string.Empty;
                if (isManualReval)
                {
                    spName = "P_RunDailyCashAccrualsRevaluation_manual";
                }
                else
                {
                    spName = "P_RunDailyCashAccrualsRevaluation_AuditTrail";
                }

                SqlConnection conn = null;

                //timeout set to 2 days
                SqlCommand cmd = DatabaseManager.DatabaseManager.CreateSqlCommand(ref conn, spName, 172800, conn_InfoMessage);

                using (conn)
                {
                    if (fromDate == null)
                        cmd.Parameters.Add(new SqlParameter("@StartDate", DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter("@StartDate", fromDate));

                    cmd.Parameters.Add(new SqlParameter("@EndDate", endDate));
                    cmd.Parameters.Add(new SqlParameter("@FundIDs", fundIDs));
                    cmd.Parameters.Add(new SqlParameter("@userID", userID));

                    SqlDataAdapter da = DatabaseManager.DatabaseManager.CreateSqlDataAdapter(cmd);
                    conn.Open();

                    // Create a DataTable and fill it.     
                    ds = new DataSet();

                    da.Fill(ds);
                }
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
        /// Saves the daily credit limit values.
        /// </summary>
        /// <param name="dtDailyCreditLimitValue">The dt daily credit limit value.</param>
        /// <param name="isSavingFromImport">if set to <c>true</c> [is saving from import].</param>
        /// <returns></returns>
        internal static int SaveDailyCreditLimitValues(DataTable dtDailyCreditLimitValue, bool isSavingFromImport)
        {
            int rowsAffected = 0;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDailyCreditLimitValue.Copy());
                string generatedXml = string.Empty;
                generatedXml = ds.GetXml();

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMSaveDailyCreditLimitValues";
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = generatedXml
                });
                queryData.DictionaryDatabaseParameter.Add("@IsSavingFromImport", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@IsSavingFromImport",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isSavingFromImport
                });

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
        /// Gets the daily credit limit values.
        /// </summary>
        /// <returns></returns>
        internal static DataTable GetDailyCreditLimitValues()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetDailyCreditLimitValues";

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the last calculated balance details.
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<int, BalanceUpdateDetail> GetLastCalculatedBalanceDetails()
        {
            try
            {
                return _persistenceManager.GetLastCalculatedBalanceDetails();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Gets the transactions by transaction identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        public static List<T> GetTransactionsByTransactionID<T>(string transactionID)
        {
            List<T> lstTransaction = new List<T>();
            try
            {
                string query = "from TransactionEntry where TransactionID = '" + transactionID + "'";
                lstTransaction = GetDataFromDb<T>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lstTransaction;
        }

        /// <summary>
        /// Gets the transactions by taxlotid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionID">The transaction identifier.</param>
        /// <returns></returns>
        public static List<T> GetTransactionsBytaxlotID<T>(string taxlotID)
        {
            List<T> lstTransaction = new List<T>();
            try
            {
                string query = "from TransactionEntry where taxlotID = '" + taxlotID + "'";
                lstTransaction = GetDataFromDb<T>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lstTransaction;
        }

        /// <summary>
        /// Get the Opertaion Mode of each account from DB
        /// </summary>
        /// <returns>datatable with columns FundID and OperationMode</returns>
        public static DataTable GetAccountIdByRevaluationOperationMode()
        {
            DataTable dtAccountIdByRevaluationOperationMode = new DataTable();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetRevaluationPreference";
                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (ds.Tables != null && ds.Tables.Count > 0)
                    dtAccountIdByRevaluationOperationMode = ds.Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dtAccountIdByRevaluationOperationMode;
        }
    }
}
