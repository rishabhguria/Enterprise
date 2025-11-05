using Prana.ActivityHandler.DAL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Prana.ActivityHandler
{
    public class ActivityDataManager
    {
        /// <summary>
        /// The persistence manager
        /// </summary>
        static PersistenceManager _persistenceManager = new PersistenceManager();

        /// <summary>
        /// Saves the activity.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        internal static void SaveActivity(List<CashActivity> lsCashActivity)
        {
            try
            {
                _persistenceManager.SaveOrUpdate(lsCashActivity);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves the bulk activity.
        /// PRANA-9776 New Parameter UserId
        /// </summary>
        /// <param name="dtData">The dt data.</param>
        /// <param name="FKIDs">The fki ds.</param>
        internal static void SaveBulkActivity(DataTable dtData, string FKIDs)
        {
            try
            {
                if (!string.IsNullOrEmpty(FKIDs))
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_DeleteActivitiesAndJournalsByFKID";
                    queryData.CommandTimeout = 12000;
                    queryData.DictionaryDatabaseParameter.Add("@FKIDs", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@FKIDs",
                        ParameterType = DbType.String,
                        ParameterValue = FKIDs
                    });

                    DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                }

                if (dtData.Rows.Count > 0)
                {
                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn))
                        {
                            sqlBulkCopy.DestinationTableName = "T_AllActivity";
                            sqlBulkCopy.ColumnMappings.Add("ActivityID", "ActivityID");
                            sqlBulkCopy.ColumnMappings.Add("ActivityTypeId_FK", "ActivityTypeId_FK");
                            sqlBulkCopy.ColumnMappings.Add("FKID", "FKID");
                            sqlBulkCopy.ColumnMappings.Add("BalanceType", "BalanceType");
                            sqlBulkCopy.ColumnMappings.Add("FundID", "FundID");
                            sqlBulkCopy.ColumnMappings.Add("TransactionSource", "TransactionSource");
                            sqlBulkCopy.ColumnMappings.Add("Symbol", "Symbol");
                            sqlBulkCopy.ColumnMappings.Add("TradeDate", "TradeDate");
                            sqlBulkCopy.ColumnMappings.Add("CurrencyID", "CurrencyID");
                            sqlBulkCopy.ColumnMappings.Add("Amount", "Amount");
                            sqlBulkCopy.ColumnMappings.Add("FXRate", "FXRate");
                            sqlBulkCopy.ColumnMappings.Add("FXConversionMethodOperator", "FXConversionMethodOperator");
                            sqlBulkCopy.ColumnMappings.Add("ActivitySource", "ActivitySource");
                            sqlBulkCopy.ColumnMappings.Add("Description", "Description");
                            sqlBulkCopy.ColumnMappings.Add("SideMultiplier", "SideMultiplier");
                            sqlBulkCopy.ColumnMappings.Add("UniqueKey", "UniqueKey");
                            sqlBulkCopy.ColumnMappings.Add("ActivityNumber", "ActivityNumber");
                            //PRANA-9777
                            sqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate");
                            sqlBulkCopy.ColumnMappings.Add("EntryDate", "EntryDate");
                            sqlBulkCopy.ColumnMappings.Add("UserId", "UserId"); //PRANA-9776
                            conn.Open();
                            //timeout set to 2 days.
                            sqlBulkCopy.BulkCopyTimeout = 172800;
                            sqlBulkCopy.WriteToServer(dtData);
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
        /// Get the calculated cash data
        /// Modified by: Bharat raturi, 16 Sep 2014
        /// purpose: send the AccountIDs to get the records for only selected accounts
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="accountIDs"></param>
        /// <returns></returns>
        internal static List<CashActivity> GetAlreadyCalculatedDailyCashData(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<CashActivity> lsAlreadyCalculatedDailyCashData = null;
            try
            {
                //string query = "FROM CashActivity WHERE ((datediff(dd,TradeDate,'" + fromDate + "')<=0 AND datediff(dd,TradeDate,'" + toDate + "')>=0) OR (datediff(dd,SettlementDate,'" + fromDate + "')<=0 AND datediff(dd,SettlementDate,'" + toDate + "')>=0)) AND ActivitySource='" + (int)CashTransactionType.DailyCalculation + "' ORDER BY ActivitySource";
                string query = "FROM CashActivity WHERE ((datediff(dd,TradeDate,'" + fromDate + "')<=0 AND datediff(dd,TradeDate,'" + toDate + "')>=0) OR (datediff(dd,SettlementDate,'" + fromDate + "')<=0 AND datediff(dd,SettlementDate,'" + toDate + "')>=0)) AND TransactionSource='" + (int)CashTransactionType.DailyCalculation + "' and FundID in (" + accountIDs + ") ORDER BY TransactionSource";
                lsAlreadyCalculatedDailyCashData = GetDataFromDb<CashActivity>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsAlreadyCalculatedDailyCashData;
        }

        /// <summary>
        /// Gets the activities by activity identifier.
        /// </summary>
        /// <param name="activityIds">The activity ids.</param>
        /// <returns></returns>
        internal static List<CashActivity> GetActivitiesByActivityId(string activityIds)
        {
            List<CashActivity> lsActivity = new List<CashActivity>();
            try
            {
                string query = "From CashActivity Where ActivityID IN ('" + activityIds + "')";
                lsActivity = GetDataFromDb<CashActivity>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsActivity;
        }

        /// <summary>
        /// Get All Activities of the same TransactionId
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        internal static List<CashActivity> GetActivitiesByTransactionId(string transactionId)
        {
            List<CashActivity> lsActivity = new List<CashActivity>();
            try
            {
                string query = "From CashActivity Where FKID IN ('" + transactionId + "')";
                lsActivity = GetDataFromDb<CashActivity>(query);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsActivity;
        }

        /// <summary>
        /// Gets the activity.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="activityDateType">Type of the activity date.</param>
        /// <returns></returns>
        internal static List<CashActivity> GetActivity(DateTime fromDate, DateTime toDate, String accountIDs, string activityDateType, bool isActivitySource)
        {
            List<CashActivity> lsActivity = new List<CashActivity>();
            try
            {
                //string query = string.Empty;
                //if (accountIDs.Trim().Equals(string.Empty))
                //{
                //    query = "from CashActivity where (datediff(d,TradeDate,'" + fromDate + "')<=0 AND datediff(d,TradeDate,'" + toDate + "')>=0) or (datediff(d,SettlementDate,'" + fromDate + "')<=0 AND datediff(d,SettlementDate,'" + toDate + "')>=0) ORDER BY TradeDate,SettlementDate";
                //}
                //else
                //{
                //    query = "from CashActivity where FundID in (" + accountIDs + ") and ((datediff(d,TradeDate,'" + fromDate + "')<=0 AND datediff(d,TradeDate,'" + toDate + "')>=0) or (datediff(d,SettlementDate,'" + fromDate + "')<=0 AND datediff(d,SettlementDate,'" + toDate + "')>=0)) ORDER BY TradeDate,SettlementDate";
                //}
                //lsActivity = GetDataFromDb<CashActivity>(query);

                lsActivity = _persistenceManager.GetActivityForJournalFromDB<CashActivity>(fromDate, toDate, accountIDs, activityDateType, isActivitySource);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsActivity;
        }

        /// <summary>
        /// Generate & Create Exception activities from Manual Journal
        /// Activities for Pre-existing journals 
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        internal static DataSet SaveManualJournalExceptions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            try
            {
                DataSet ds = new DataSet();
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_RuntimeActivityFromManualJournal";

                queryData.CommandTimeout = 2700000;  //set to 45 mins for large data.
                queryData.DictionaryDatabaseParameter.Add("@StartDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@StartDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = fromDate
                });
                queryData.DictionaryDatabaseParameter.Add("@EndDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@EndDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = toDate
                });

                if (!string.IsNullOrWhiteSpace(accountIDs))
                    queryData.DictionaryDatabaseParameter.Add("@FundIDs", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@FundIDs",
                        ParameterType = DbType.String,
                        ParameterValue = accountIDs
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
        /// Modified by: Bharat Raturi, 01 Aug 2014
        /// purpose: filter the activities by cash managements start dates of the respective accounts at the db level
        /// </summary>
        /// <param name="fromDate">start date </param>
        /// <param name="toDate">end date</param>
        /// <param name="accountIDs">comma separated accountIDs</param>
        /// <returns>List of activities </returns>
        internal static List<CashActivity> GetActivityExceptions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<CashActivity> lsActivity = new List<CashActivity>();
            try
            {
                #region Trading Section

                //Modified by: Bharat raturi, 1 aug 2014
                //send the accountIDs
                //List<TaxLot> lsTaxlot = ActivityHelperClass.CashManagementServices.GetExceptionalTradingData(fromDate, toDate);
                List<TaxLot> lsTaxlot = ServiceProxyConnector.CashManagementServices.GetExceptionalTradingData(fromDate, toDate, accountIDs);
                //remove closing taxlots generated by settlement of FX/FXForward trades
                lsTaxlot.RemoveAll(x => (x.AssetID == (int)AssetCategory.FX || x.AssetID == (int)AssetCategory.FXForward) && x.TransactionSource == TransactionSource.Closing);
                lsActivity.AddRange(ActivityHelperClass.CreateCashActivity(lsTaxlot, CashTransactionType.Trading));

                #endregion

                #region Dividend Section

                //DataSet dsCashTransactionException = GetCashTransactionExceptions(string.Empty, fromDate, toDate);
                DataSet dsCashTransactionException = GetCashTransactionExceptions(string.Empty, fromDate, toDate, accountIDs);
                //Before TransactionSource was TransactionSource.CorpAction
                lsActivity.AddRange(ActivityHelperClass.CreateCashActivity(dsCashTransactionException, CashTransactionType.CashTransaction));

                #endregion


                #region Closing Section

                List<Position> lsPositions = ServiceProxyConnector.ClosingServices.GetClosingTransactionExceptions(fromDate, toDate, accountIDs);
                lsActivity.AddRange(ActivityHelperClass.CreateCashActivity<List<Position>>(lsPositions, CashTransactionType.Closing));

                #endregion

                ActivityHelperClass.SetActiviyState(lsActivity, ApplicationConstants.TaxLotState.New);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsActivity;
        }

        /// <summary>
        /// Modified by: Bharat raturi, 1 aug 2014
        /// purpose: send the accountIDs to get the transaction only after the cash mgmt start date
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountIDs"></param>
        /// <returns></returns>
        internal static DataSet GetCashTransactionExceptions(string symbol, object dateFrom, object dateTo, string accountIDs)
        {
            DataSet ds = new DataSet();

            try
            {
                string[] tables = { "CashDividendsExceptions" };
                //object[] param = new object[3];
                //param[0] = symbol;
                //param[1] = dateFrom;
                //param[2] = dateTo;
                //modified by: Bharat raturi, 1 aug 2014
                //send fundIDs as parameter
                object[] param = { symbol, dateFrom, dateTo, accountIDs };

                DatabaseManager.DatabaseManager.LoadDataSet("P_GetCashTransactionsExceptions", ds, tables, param);
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
        /// Gets the overriding activity.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        internal static List<CashActivity> GetOverridingActivity(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<CashActivity> lsActivity = new List<CashActivity>();
            try
            {
                #region Trading Section

                List<TaxLot> lstTaxlot = ServiceProxyConnector.AllocationServices.InnerChannel.GetPositions(fromDate, toDate, accountIDs);
                //remove closing taxlots generated by settlement of FX/FXForward trades
                lstTaxlot.RemoveAll(x => (x.AssetID == (int)AssetCategory.FX || x.AssetID == (int)AssetCategory.FXForward) && x.TransactionSource == TransactionSource.Closing);
                ActivityHelperClass.SetTaxLotState(lstTaxlot, ApplicationConstants.TaxLotState.Updated);
                lsActivity.AddRange(ActivityHelperClass.CreateCashActivity(lstTaxlot, CashTransactionType.Trading));

                #endregion

                #region Dividend Section

                // fromDate and toDate basis on ExDate
                // -1 for getting all account records
                //-1 userId is used to indicate server internal call
                DataSet dsCashTransactionException = ServiceProxyConnector.CashManagementServices.GetCashDividends(string.Empty, accountIDs, "ExDate", fromDate, toDate, -1);
                lsActivity.AddRange(ActivityHelperClass.CreateCashActivity(dsCashTransactionException, CashTransactionType.CorpAction));

                #endregion

                #region Closing Section

                List<Position> lsPositions = ServiceProxyConnector.ClosingServices.GetNetPositionsFromDB(fromDate, toDate, false, accountIDs, string.Empty, string.Empty, string.Empty);
                lsActivity.AddRange(ActivityHelperClass.CreateCashActivity<List<Position>>(lsPositions, CashTransactionType.Closing));

                #endregion


                ActivityHelperClass.SetActiviyState(lsActivity, ApplicationConstants.TaxLotState.Updated);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsActivity;
        }
    }
}
