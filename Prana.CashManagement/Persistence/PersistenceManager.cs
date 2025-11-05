using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Prana.CashManagement
{
    /// <summary>
    /// Specifies whether to begin a new session, continue an existing session, or end an existing session.
    /// </summary>
    /// 
    public enum SessionAction
    {
        Begin,
        Continue,
        End,
        BeginAndEnd
    }

    public class PersistenceManager : IDisposable
    {
        #region Declarations

        /// <summary>
        /// The m session factory
        /// </summary>
        private ISessionFactory m_SessionFactory = null;

        /// <summary>
        /// The m session
        /// </summary>
        private ISession m_Session = null;

        /// <summary>
        /// The dic last calculated balance date
        /// </summary>
        private Dictionary<string, BalanceUpdateDetail> _dicLastCalculatedBalanceDate;
        private int timeoutForQuery;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PersistenceManager()
        {
            string miscellanousTimeout = ConfigurationHelper.Instance.GetAppSettingValueByKey("QueryTimeoutForAllocation");
            if (!int.TryParse(miscellanousTimeout, out timeoutForQuery))
            {
                timeoutForQuery = 60;
            }
            this.ConfigureLog4Net();
            this.ConfigureNHibernate();
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_Session != null)
                    m_Session.Dispose();
                m_SessionFactory.Dispose();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Close this Persistence Manager and release all resources (connection pools, etc). It is the responsibility of the application to ensure that there are no open Sessions before calling Close().
        /// </summary>
        public void Close()
        {
            m_SessionFactory.Close();
        }

        /// <summary>
        /// Deletes an object of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public void Delete<T>(T item)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.Delete(item);
                    session.Transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Deletes objects of a specified type.
        /// </summary>
        /// <param name="itemsToDelete">The items to delete.</param>
        /// <typeparam name="T">The type of objects to delete.</typeparam>
        public void Delete<T>(IList<T> itemsToDelete)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                foreach (T item in itemsToDelete)
                {
                    using (session.BeginTransaction())
                    {
                        session.Delete(item);
                        session.Transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <returns>A list of all objects of the specified type.</returns>
        public IList<T> RetrieveAll<T>(SessionAction sessionAction)
        {
            /* Note that NHibernate guarantees that two object references will point to the
             * same object only if the references are set in the same session. For example,
             * Order #123 under the Customer object Able Inc and Order #123 in the Orders
             * list will point to the same object only if we load Customers and Orders in 
             * the same session. If we load them in different sessions, then changes that
             * we make to Able Inc's Order #123 will not be reflected in Order #123 in the
             * Orders list, since the references point to different objects. That's why we
             * maintain a session as a member variable, instead of as a local variable. */

            // Open a new session if specified
            if ((sessionAction == SessionAction.Begin) || (sessionAction == SessionAction.BeginAndEnd))
            {
                m_Session = m_SessionFactory.OpenSession();
            }

            // Retrieve all objects of the type passed in
            ICriteria targetObjects = m_Session.CreateCriteria(typeof(T));
            IList<T> itemList = targetObjects.List<T>();

            // Close the session if specified
            if ((sessionAction == SessionAction.End) || (sessionAction == SessionAction.BeginAndEnd))
            {
                m_Session.Close();
                m_Session.Dispose();
            }
            // Set return value
            return itemList;
        }

        /// <summary>
        /// Retrieves objects of a specified type where a specified property equals a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be retrieved.</typeparam>
        /// <param name="propertyName">The name of the property to be tested.</param>
        /// <param name="propertyValue">The value that the named property must hold.</param>
        /// <returns>A list of all objects meeting the specified criteria.</returns>
        public IList<T> RetrieveEquals<T>(string propertyName, object propertyValue)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Expression.Eq(propertyName, propertyValue));

                // Get the matching objects
                IList<T> matchingObjects = criteria.List<T>();

                // Set return value
                return matchingObjects;
            }
        }

        /// <summary>
        /// Saves an object and its persistent children.
        /// </summary>
        public void Save<T>(T item)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {
                    using (session.BeginTransaction())
                    {
                        session.SaveOrUpdate(item);
                        session.Flush();
                        session.Transaction.Commit();
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
        /// Creates the activity from manual journal.
        /// </summary>
        /// <param name="Xmltransaction">The xmltransaction.</param>
        /// <returns></returns>
        public List<CashActivity> CreateActivityFromManualJournal(String Xmltransaction)
        {
            List<CashActivity> activities = new List<CashActivity>();
            try
            {
                DataSet ds;

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_RuntimeActivityFromManualJournal";
                queryData.CommandTimeout = 900;
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.String,
                    ParameterValue = Xmltransaction
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt_Activities = ds.Tables[0];  // for deafult 
                    //TODO: Surendra :Comment
                    if (ds.Tables.Count == 3)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)  // server side activitytype
                            CachedDataManager.GetActivityType().Add(row[1].ToString().Trim(), int.Parse(row[0].ToString().Trim()));
                        foreach (DataRow row in ds.Tables[1].Rows)   //server side activityjournalmapping
                            CachedDataManager.GetActivityJournalMapping().Rows.Add(row.ItemArray);

                        CashmgmtServices.PublishActivityType(ds.Tables[0]);
                        CashmgmtServices.PublishActivityTypeMapping(ds.Tables[1]);
                        dt_Activities = ds.Tables[2];
                    }

                    #region Listing Activities for Subscription

                    /* if any field added to activity further nedd to set 0 in sp. 
                        not handled the code in Code for null values. Made this keeping in mind 
                            Nothing is Null in activity table. */

                    foreach (DataRow dr in dt_Activities.Rows)
                    {
                        activities.Add(new CashActivity()
                        {
                            ActivityId = dr["ActivityId"].ToString(),
                            ActivityTypeId = int.Parse(dr["ActivityTypeId_FK"].ToString()),
                            FKID = dr["FKID"].ToString(),
                            BalanceType = (BalanceType)int.Parse(dr["BalanceType"].ToString()),
                            Symbol = dr["Symbol"].ToString(),
                            AccountID = int.Parse(dr["FundID"].ToString()),
                            Date = Convert.ToDateTime(dr["TradeDate"].ToString()),
                            SettlementDate = Convert.ToDateTime(dr["SettlementDate"].ToString()),
                            CurrencyID = int.Parse(dr["CurrencyID"].ToString()),
                            LeadCurrencyID = int.Parse(dr["LeadCurrencyID"].ToString()),
                            VsCurrencyID = int.Parse(dr["VsCurrencyID"].ToString()),
                            ClosedQty = decimal.Parse(dr["ClosedQty"].ToString()),
                            Amount = decimal.Parse(dr["Amount"].ToString()),
                            Commission = decimal.Parse(dr["Commission"].ToString()),
                            SoftCommission = decimal.Parse(dr["SoftCommission"].ToString()),
                            OtherBrokerFees = decimal.Parse(dr["OtherBrokerFees"].ToString()),
                            ClearingBrokerFee = decimal.Parse(dr["ClearingBrokerFee"].ToString()),
                            StampDuty = decimal.Parse(dr["StampDuty"].ToString()),
                            TransactionLevy = decimal.Parse(dr["TransactionLevy"].ToString()),
                            ClearingFee = decimal.Parse(dr["ClearingFee"].ToString()),
                            TaxOnCommissions = decimal.Parse(dr["TaxOnCommissions"].ToString()),
                            MiscFees = decimal.Parse(dr["MiscFees"].ToString()),
                            SecFee = decimal.Parse(dr["SecFee"].ToString()),
                            OccFee = decimal.Parse(dr["OccFee"].ToString()),
                            OrfFee = decimal.Parse(dr["OrfFee"].ToString()),
                            FXRate = double.Parse(dr["FXRate"].ToString()),
                            OptionPremiumAdjustment = dr["OptionPremiumAdjustment"] is DBNull ? Convert.ToDecimal(0.0) : decimal.Parse(dr["OptionPremiumAdjustment"].ToString()),
                            FXConversionMethodOperator = dr["FXConversionMethodOperator"].ToString(),
                            ActivitySource = (ActivitySource)(int.Parse(dr["ActivitySource"].ToString())),
                            TransactionSource = (CashTransactionType)(int.Parse(dr["TransactionSource"].ToString())),
                            ActivityNumber = int.Parse(dr["ActivityNumber"].ToString()),
                            Description = dr["Description"].ToString(),
                            SubActivity = dr["SubActivity"].ToString(),
                            SideMultiplier = int.Parse(dr["SideMultiplier"].ToString()),
                            //PRANA-9777
                            ModifyDate = dr["ModifyDate"] is DBNull ? DateTimeConstants.MinValue : Convert.ToDateTime(dr["ModifyDate"]),
                            EntryDate = dr["EntryDate"] is DBNull ? DateTimeConstants.MinValue : Convert.ToDateTime(dr["EntryDate"]),
                            //PRANA-9776
                            UserId = dr["UserId"] is DBNull ? 0 : Convert.ToInt32(dr["UserId"]),
                            ActivityState = ApplicationConstants.TaxLotState.Updated   // it is needed for modified activity to be displayed through subscription.Works same as Activitystate_New with additional replace exiting activity with modified on Activity Tab. 
                        }
                        );
                    }
                    #endregion
                    NameValueFiller.SetNameValues(activities);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return activities;
        }

        /// <summary>
        /// Saves the or update manual journal.
        /// </summary>
        /// <param name="transactionsToSave">The transactions to save.</param>
        public void SaveOrUpdateManualJournal(List<Transaction> transactionsToSave)
        {
            List<CashActivity> lstCashActivity = new List<CashActivity>();
            Dictionary<int, DateTime> accountsTobeUpdated = new Dictionary<int, DateTime>();
            Dictionary<int, RevaluationUpdateDetail> revalDates = CachedDataManager.GetLastRevaluationCalculationDate();
            Dictionary<int, CashPreferences> cashPref = CashDataManager.GetCashPreferences();

            DataTable ListOfTransactions = new DataTable("Transactions");
            DataSet DsListOfTransactions = new DataSet("DS");

            ListOfTransactions.Columns.Add("TransactionEntryID", typeof(String));
            ListOfTransactions.Columns.Add("TaxLotID", typeof(String));
            ListOfTransactions.Columns.Add("ActivityId_FK", typeof(String));
            ListOfTransactions.Columns.Add("FundID", typeof(int));
            ListOfTransactions.Columns.Add("SubAccountID", typeof(int));
            ListOfTransactions.Columns.Add("CurrencyID", typeof(int));
            ListOfTransactions.Columns.Add("Symbol", typeof(String));
            ListOfTransactions.Columns.Add("PBDesc", typeof(String));
            ListOfTransactions.Columns.Add("TransactionDate", typeof(String));
            ListOfTransactions.Columns.Add("TransactionID", typeof(String));
            ListOfTransactions.Columns.Add("DR", typeof(decimal));
            ListOfTransactions.Columns.Add("CR", typeof(decimal));
            ListOfTransactions.Columns.Add("TransactionSource", typeof(int));
            ListOfTransactions.Columns.Add("TransactionNumber", typeof(int));
            ListOfTransactions.Columns.Add("AccountSide", typeof(int));
            ListOfTransactions.Columns.Add("ActivitySource", typeof(int));
            ListOfTransactions.Columns.Add("FxRate", typeof(decimal));
            ListOfTransactions.Columns.Add("FXConversionMethodOperator", typeof(char));
            //PRANA-9777
            ListOfTransactions.Columns.Add("ModifyDate", typeof(String));
            ListOfTransactions.Columns.Add("EntryDate", typeof(String));
            //PRANA-9776
            ListOfTransactions.Columns.Add("UserId", typeof(int));
            try
            {
                using (IStatelessSession session = m_SessionFactory.OpenStatelessSession())
                {
                    using (session.BeginTransaction())
                    {
                        foreach (Transaction t in transactionsToSave)
                        {
                            string taxlotState = t.GetTaxlotState();
                            if (taxlotState == ApplicationConstants.TaxLotState.Deleted.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.Updated.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.NotChanged.ToString())
                            {
                                int trSource = t.GetTransactionSource();
                                DeleteAllEntries(t.TransactionID, session, t.TransactionNumber, trSource);

                                if (trSource == (int)CashTransactionType.ManualJournalEntry || trSource == (int)CashTransactionType.OpeningBalance || trSource == (int)CashTransactionType.ImportedEditableData)
                                {
                                    if (taxlotState == ApplicationConstants.TaxLotState.Deleted.ToString())
                                    {
                                        CashActivity newCashActivity = new CashActivity();
                                        newCashActivity.FKID = t.TransactionID;
                                        newCashActivity.Date = t.Date;
                                        newCashActivity.TransactionSource = (CashTransactionType)trSource;
                                        newCashActivity.ActivityNumber = t.TransactionNumber;
                                        newCashActivity.ActivityState = ApplicationConstants.TaxLotState.Deleted;
                                        lstCashActivity.Add(newCashActivity);
                                    }
                                }
                            }

                            if (taxlotState == ApplicationConstants.TaxLotState.New.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.Updated.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.NotChanged.ToString())
                            {
                                foreach (TransactionEntry trEntry in t.TransactionEntries)
                                {
                                    //PRANA-9777
                                    ListOfTransactions.Rows.Add(trEntry.TransactionEntryID, trEntry.TaxLotId, trEntry.ActivityId_FK, trEntry.AccountID, trEntry.SubAcID, trEntry.CurrencyID,
                                        trEntry.Symbol, trEntry.Description, trEntry.TransactionDate, trEntry.TransactionID,
                                        trEntry.DR, trEntry.CR, t.TransactionEntries[0].TransactionSource, trEntry.TransactionNumber, trEntry.EntryAccountSide, trEntry.ActivitySource
                                        , trEntry.FxRate, trEntry.FXConversionMethodOperator, DateTime.Now, trEntry.EntryDate, trEntry.UserId);
                                }
                            }
                            //Modified by: Bharat Raturi, 10 Feb 2014
                            //The last revaluation date and last sub account balance calculation date must be updated for edited, new or deleted transactions
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6158
                            foreach (TransactionEntry trEntry in t.TransactionEntries)
                            {
                                UpdateLastCalculatedBalanceDate(trEntry, session, false);

                                if (trEntry.CurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                                {
                                    if (DateTime.Compare(trEntry.TransactionDate.Date, revalDates[trEntry.AccountID].LastRevaluationDate.Date) <= 0
                                        && DateTime.Compare(cashPref[trEntry.AccountID].CashMgmtStartDate, trEntry.TransactionDate.Date) <= 0)
                                    {
                                        revalDates[trEntry.AccountID].LastRevaluationDate = trEntry.TransactionDate.Date;
                                        if (!accountsTobeUpdated.ContainsKey(trEntry.AccountID))
                                        {
                                            accountsTobeUpdated.Add(trEntry.AccountID, trEntry.TransactionDate);
                                        }
                                        else
                                        {
                                            accountsTobeUpdated[trEntry.AccountID] = trEntry.TransactionDate;
                                        }
                                    }
                                }
                            }
                        }
                        if (accountsTobeUpdated != null && accountsTobeUpdated.Count > 0)
                        {
                            foreach (KeyValuePair<int, DateTime> kvp in accountsTobeUpdated)
                            {
                                UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, kvp.Value, kvp.Key.ToString(), false, false, false);
                            }
                        }
                        session.Transaction.Commit();
                        DsListOfTransactions.Tables.Add(ListOfTransactions);
                        CashmgmtServices.PublishCashActivity(CreateActivityFromManualJournal(DsListOfTransactions.GetXml()).Union(lstCashActivity).ToList());
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
        /// Saves the or update.
        /// </summary>
        /// <param name="transactionsToSave">The transactions to save.</param>
        public void SaveOrUpdate(List<Transaction> transactionsToSave)
        {
            try
            {
                Dictionary<int, RevaluationUpdateDetail> _revalDates = CachedDataManager.GetLastRevaluationCalculationDate();
                Dictionary<int, CashPreferences> _cashPreferences = CashDataManager.GetCashPreferences();
                Dictionary<int, DateTime> _accountsTobeUpdated = new Dictionary<int, DateTime>();

                using (IStatelessSession session = m_SessionFactory.OpenStatelessSession())
                {
                    using (session.BeginTransaction())
                    {
                        foreach (Transaction t in transactionsToSave)
                        {
                            string taxlotState = t.GetTaxlotState();
                            if (taxlotState == ApplicationConstants.TaxLotState.Deleted.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.Updated.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.NotChanged.ToString())
                                DeleteAllEntries(t.ActivityId_FK, session, t.TransactionNumber, t.GetTransactionSource());

                            if (taxlotState == ApplicationConstants.TaxLotState.New.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.Updated.ToString() ||
                                taxlotState == ApplicationConstants.TaxLotState.NotChanged.ToString())
                            {
                                foreach (TransactionEntry trEntry in t.TransactionEntries)
                                {
                                    session.Insert(trEntry);
                                }
                            }

                            //Modified by: Bharat Raturi, 10 Feb 2014
                            //The last revaluation date and last sub account balance calculation date must be updated for edited, new or deleted transactions
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6158
                            foreach (TransactionEntry trEntry in t.TransactionEntries)
                            {
                                UpdateLastCalculatedDate(_revalDates, _cashPreferences, _accountsTobeUpdated, session, trEntry);
                            }

                            foreach (TransactionEntry trEntry in t.DeletedTransactionEntries)
                            {
                                if (_dicLastCalculatedBalanceDate.ContainsKey(trEntry.AccountID + "_" + trEntry.CurrencyID + "_" + trEntry.SubAcID))
                                {
                                    UpdateLastCalculatedDate(_revalDates, _cashPreferences, _accountsTobeUpdated, session, trEntry);
                                }
                            }
                        }
                        if (_accountsTobeUpdated != null && _accountsTobeUpdated.Count > 0)
                        {
                            foreach (KeyValuePair<int, DateTime> kvp in _accountsTobeUpdated)
                            {
                                UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, kvp.Value, kvp.Key.ToString(), false, false, false);
                            }
                        }
                        session.Transaction.Commit();
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
        /// Saves the bulk activity.
        /// PRANA-9776 New Parameter UserId
        /// </summary>
        /// <param name="dtData">The dt data.</param>
        /// <param name="FKIDs">The fki ds.</param>
        public void SaveOrUpdateSymbolLevelAccrual(List<Transaction> transactionsToSave)
        {
            try
            {
                if (transactionsToSave.Count > 0)
                {
                    DataTable SymbolLevelAccrualJournalsData = new DataTable();
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionEntryID", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("TaxLotId", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("ActivityId_FK", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("AccountID", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("SubAcID", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("CurrencyID", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("Symbol", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("Description", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionDate", typeof(DateTime));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionID", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("DR", typeof(decimal));
                    SymbolLevelAccrualJournalsData.Columns.Add("CR", typeof(decimal));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionSource", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("TransactionNumber", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("EntryAccountSide", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("ActivitySource", typeof(int));
                    SymbolLevelAccrualJournalsData.Columns.Add("FxRate", typeof(double));
                    SymbolLevelAccrualJournalsData.Columns.Add("FXConversionMethodOperator", typeof(string));
                    SymbolLevelAccrualJournalsData.Columns.Add("ModifyDate", typeof(DateTime));
                    SymbolLevelAccrualJournalsData.Columns.Add("EntryDate", typeof(DateTime));
                    SymbolLevelAccrualJournalsData.Columns.Add("UserId", typeof(int));

                    foreach (Transaction t in transactionsToSave)
                    {
                        string taxlotState = t.GetTaxlotState();
                        //if (taxlotState == ApplicationConstants.TaxLotState.Deleted.ToString() ||
                        //    taxlotState == ApplicationConstants.TaxLotState.Updated.ToString() ||
                        //    taxlotState == ApplicationConstants.TaxLotState.NotChanged.ToString())
                        //    DeleteAllEntries(t.ActivityId_FK, session, t.TransactionNumber, t.GetTransactionSource());

                        if (taxlotState == ApplicationConstants.TaxLotState.New.ToString() ||
                            taxlotState == ApplicationConstants.TaxLotState.Updated.ToString() ||
                            taxlotState == ApplicationConstants.TaxLotState.NotChanged.ToString())
                        {
                            foreach (TransactionEntry trEntry in t.TransactionEntries)
                            {
                                DataRow dr = SymbolLevelAccrualJournalsData.NewRow();
                                fillSymbollevelAccrualDataRow(dr, trEntry);
                                SymbolLevelAccrualJournalsData.Rows.Add(dr);
                            }
                        }
                    }

                    using (SqlConnection conn = (SqlConnection)DatabaseManager.DatabaseManager.CreateConnection())
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn))
                        {
                            sqlBulkCopy.DestinationTableName = "T_SymbolLevelAccrualsJournal";

                            sqlBulkCopy.ColumnMappings.Add("TransactionEntryID", "TransactionEntryID");
                            sqlBulkCopy.ColumnMappings.Add("TaxLotId", "TaxLotID");
                            sqlBulkCopy.ColumnMappings.Add("ActivityId_FK", "ActivityId_FK");
                            sqlBulkCopy.ColumnMappings.Add("AccountID", "FundID");
                            sqlBulkCopy.ColumnMappings.Add("SubAcID", "SubAccountID");
                            sqlBulkCopy.ColumnMappings.Add("CurrencyID", "CurrencyID");
                            sqlBulkCopy.ColumnMappings.Add("Symbol", "Symbol");
                            sqlBulkCopy.ColumnMappings.Add("Description", "PBDesc");
                            sqlBulkCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");
                            sqlBulkCopy.ColumnMappings.Add("TransactionID", "TransactionID");
                            sqlBulkCopy.ColumnMappings.Add("DR", "DR");
                            sqlBulkCopy.ColumnMappings.Add("CR", "CR");
                            sqlBulkCopy.ColumnMappings.Add("TransactionSource", "TransactionSource");
                            sqlBulkCopy.ColumnMappings.Add("TransactionNumber", "TransactionNumber");
                            sqlBulkCopy.ColumnMappings.Add("EntryAccountSide", "AccountSide");
                            sqlBulkCopy.ColumnMappings.Add("ActivitySource", "ActivitySource");
                            //PRANA-9777
                            sqlBulkCopy.ColumnMappings.Add("FxRate", "FxRate");
                            sqlBulkCopy.ColumnMappings.Add("FXConversionMethodOperator", "FXConversionMethodOperator");
                            sqlBulkCopy.ColumnMappings.Add("ModifyDate", "ModifyDate"); //PRANA-9776
                            sqlBulkCopy.ColumnMappings.Add("EntryDate", "EntryDate");
                            sqlBulkCopy.ColumnMappings.Add("UserId", "UserId");
                            conn.Open();
                            //timeout set to 2 days.
                            sqlBulkCopy.BulkCopyTimeout = 172800;
                            sqlBulkCopy.WriteToServer(SymbolLevelAccrualJournalsData);
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

        void fillSymbollevelAccrualDataRow(DataRow dr, TransactionEntry trEntry)
        {
            try
            {
                dr["TransactionEntryID"] = trEntry.TransactionEntryID;
                dr["TaxLotId"] = trEntry.TaxLotId;
                dr["ActivityId_FK"] = trEntry.ActivityId_FK;
                dr["AccountID"] = trEntry.AccountID;
                dr["SubAcID"] = trEntry.SubAcID;
                dr["CurrencyID"] = trEntry.CurrencyID;
                dr["Symbol"] = trEntry.Symbol;
                dr["Description"] = trEntry.Description;
                dr["TransactionDate"] = trEntry.TransactionDate;
                dr["TransactionID"] = trEntry.TransactionID;
                dr["DR"] = trEntry.DR;
                dr["CR"] = trEntry.CR;
                dr["TransactionSource"] = trEntry.TransactionSource;
                dr["TransactionNumber"] = trEntry.TransactionNumber;
                dr["EntryAccountSide"] = trEntry.EntryAccountSide;
                dr["ActivitySource"] = trEntry.ActivitySource;
                dr["FxRate"] = trEntry.FxRate;
                dr["FXConversionMethodOperator"] = trEntry.FXConversionMethodOperator;
                dr["ModifyDate"] = trEntry.ModifyDate;
                dr["EntryDate"] = trEntry.EntryDate;
                dr["UserId"] = trEntry.UserId;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        ///  Update Last Calculated Date
        /// </summary>
        /// <param name="_revalDates"></param>
        /// <param name="_cashPreferences"></param>
        /// <param name="_accountsTobeUpdated"></param>
        /// <param name="session"></param>
        /// <param name="trEntry"></param>
        private void UpdateLastCalculatedDate(Dictionary<int, RevaluationUpdateDetail> _revalDates, Dictionary<int, CashPreferences> _cashPreferences, Dictionary<int, DateTime> _accountsTobeUpdated, IStatelessSession session, TransactionEntry trEntry)
        {
            try
            {
                UpdateLastCalculatedBalanceDate(trEntry, session, false);

                if (DateTime.Compare(trEntry.TransactionDate.Date, _revalDates[trEntry.AccountID].LastRevaluationDate.Date) <= 0
                     && DateTime.Compare(_cashPreferences[trEntry.AccountID].CashMgmtStartDate, trEntry.TransactionDate.Date) <= 0)
                {
                    _revalDates[trEntry.AccountID].LastRevaluationDate = trEntry.TransactionDate;
                    if (!_accountsTobeUpdated.ContainsKey(trEntry.AccountID))
                    {
                        _accountsTobeUpdated.Add(trEntry.AccountID, trEntry.TransactionDate);
                    }
                    else
                    {
                        _accountsTobeUpdated[trEntry.AccountID] = trEntry.TransactionDate;
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
        /// Deletes all entries.
        /// </summary>
        /// <param name="fkid">The fkid.</param>
        /// <param name="session">The session.</param>
        /// <param name="TransactionNumber">The transaction number.</param>
        /// <param name="trSource">The tr source.</param>
        private void DeleteAllEntries(string fkid, IStatelessSession session, int TransactionNumber, int trSource)
        {
            try
            {
                IQuery q;
                //Changed by: Bharat raturi, 17 march 2015
                //For imported editable data cash activities should be deleted by fkid
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6507
                if (trSource == (int)CashTransactionType.ManualJournalEntry || trSource == (int)CashTransactionType.OpeningBalance || trSource == (int)CashTransactionType.ImportedEditableData)
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_DeleteActivitiesByFKID";
                    queryData.CommandTimeout = 900;
                    queryData.DictionaryDatabaseParameter.Add("@FKID", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@FKID",
                        ParameterType = DbType.String,
                        ParameterValue = fkid
                    });
                    queryData.DictionaryDatabaseParameter.Add("@TransactionNumber", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@TransactionNumber",
                        ParameterType = DbType.Int32,
                        ParameterValue = TransactionNumber
                    });

                    DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                }
                else
                {
                    q = session.CreateQuery("delete from TransactionEntry where ActivityId_FK = '" + fkid + "'" + "AND TransactionNumber = '" + TransactionNumber + "'");
                    q.ExecuteUpdate();
                }
                q = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Modified By: Narendra Kumar Jangir
        /// Date: Nov 28,2013
        /// Description: Update LastCalculatedBalanceDate only if LastCalcDate is greater than or equal to cash management startdate
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="session"></param>
        private static readonly object _lockObjUpdateLastCalculatedBalanceDate = new object();
        private void UpdateLastCalculatedBalanceDate(TransactionEntry item, IStatelessSession session, bool isUpdated)
        {
            try
            {
                if (_dicLastCalculatedBalanceDate == null)
                    InitializeLastCalculatedBalanceDateCache(session);
                string transactionEntryKeyToCompare;
                //to compare with LastCalculatedBalanceDate's key 
                transactionEntryKeyToCompare = item.GetUniqueSubAcBalKey();
                lock (_lockObjUpdateLastCalculatedBalanceDate)
                {
                    //Update LastCalculatedBalanceDate only if LastCalcDate is greater than or equal to cash management startdate
                    if (DateTime.Compare(CashDataManager.GetCashPreferences()[item.AccountID].CashMgmtStartDate.Date, item.TransactionDate.Date) <= 0)
                    {
                        if (!_dicLastCalculatedBalanceDate.ContainsKey(transactionEntryKeyToCompare) && DateTime.Compare(CashDataManager.GetCashPreferences()[item.AccountID].CashMgmtStartDate.Date, item.TransactionDate.Date) <= 0)
                        {
                            LastCalculatedBalanceDate obj = new LastCalculatedBalanceDate();
                            obj.CurrencyID = item.CurrencyID;
                            obj.AccountID = item.AccountID;
                            obj.SubAcID = item.SubAcID;
                            obj.LastCalcDate = item.TransactionDate;
                            obj.UpdatedBalance = isUpdated;
                            session.Insert(obj);
                            BalanceUpdateDetail objBal = new BalanceUpdateDetail(item.TransactionDate, isUpdated);
                            _dicLastCalculatedBalanceDate.Add(transactionEntryKeyToCompare, objBal);
                        }
                        //Update LastCalculatedBalanceDate only if LastCalcDate is greater than or equal to cash management startdate
                        //Update LastCalculatedBalanceDate only if transactiondate is greater than LastCalcDate date of cache
                        else if (_dicLastCalculatedBalanceDate.ContainsKey(transactionEntryKeyToCompare) &&
                                DateTime.Compare(_dicLastCalculatedBalanceDate[transactionEntryKeyToCompare].LastBalanceDate, item.TransactionDate.Date) >= 0)
                        {
                            IQuery q = session.CreateQuery("Update LastCalculatedBalanceDate set LastCalcDate='"
                                + item.TransactionDate + "', UpdatedBalance=" + isUpdated + " where FundID = '"
                                + item.AccountID + "' AND SubAcID='" + item.SubAcID + "' AND CurrencyID='" + item.CurrencyID
                                  + "' AND cast(LastCalcDate AS Date) >= '" + item.TransactionDate.Date + "'");

                            q.ExecuteUpdate();
                            _dicLastCalculatedBalanceDate[transactionEntryKeyToCompare].LastBalanceDate = item.TransactionDate.Date;
                            _dicLastCalculatedBalanceDate[transactionEntryKeyToCompare].isUpdatedBalance = isUpdated;
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
        /// Modified By: Narendra Kumar Jangir
        /// Date: Nov 28,2013
        /// Description: Update LastCalculatedBalanceDate only if LastCalcDate is greater than or equal to cash management startdate
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="session"></param>
        public void UpdateLastCalculatedBalanceDateForTransactionEntries(Dictionary<string, TransactionEntry> lsModifiedTransactionEntries)
        {
            Dictionary<int, DateTime> accountsTobeUpdated = new Dictionary<int, DateTime>();
            Dictionary<int, RevaluationUpdateDetail> revalDates = CachedDataManager.GetLastRevaluationCalculationDate();
            Dictionary<int, CashPreferences> cashPref = CashDataManager.GetCashPreferences();
            try
            {
                using (IStatelessSession session = m_SessionFactory.OpenStatelessSession())
                {
                    using (session.BeginTransaction())
                    {
                        foreach (KeyValuePair<string, TransactionEntry> kvp in lsModifiedTransactionEntries)
                        {
                            //if cache is null then fill cahe from DB.
                            //Cache may be null if user delete data from T_LastCalculatedBalanceDate
                            if (_dicLastCalculatedBalanceDate == null)
                            {
                                InitializeLastCalculatedBalanceDateCache(session);
                            }
                            if (_dicLastCalculatedBalanceDate.ContainsKey(kvp.Key))
                            {
                                UpdateLastCalculatedBalanceDate(kvp.Value, session, false);
                            }
                            if (kvp.Value.CurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                            {
                                if (DateTime.Compare(kvp.Value.TransactionDate.Date, revalDates[kvp.Value.AccountID].LastRevaluationDate.Date) <= 0
                                    && DateTime.Compare(cashPref[kvp.Value.AccountID].CashMgmtStartDate, kvp.Value.TransactionDate.Date) <= 0)
                                {
                                    revalDates[kvp.Value.AccountID].LastRevaluationDate = kvp.Value.TransactionDate;
                                    if (!accountsTobeUpdated.ContainsKey(kvp.Value.AccountID))
                                    {
                                        accountsTobeUpdated.Add(kvp.Value.AccountID, kvp.Value.TransactionDate);
                                    }
                                    else
                                    {
                                        accountsTobeUpdated[kvp.Value.AccountID] = kvp.Value.TransactionDate;
                                    }
                                }
                            }
                        }
                        if (accountsTobeUpdated != null && accountsTobeUpdated.Count > 0)
                        {
                            foreach (KeyValuePair<int, DateTime> kvp in accountsTobeUpdated)
                            {
                                UpdateLastRevaluationCalculatedDateToGivenDate(DateTime.MinValue, kvp.Value, kvp.Key.ToString(), false, false, false);
                            }
                        }
                        session.Transaction.Commit();
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
        /// Initializes the last calculated balance date cache.
        /// </summary>
        /// <param name="session">The session.</param>
        private void InitializeLastCalculatedBalanceDateCache(IStatelessSession session)
        {
            try
            {
                _dicLastCalculatedBalanceDate = new Dictionary<string, BalanceUpdateDetail>();
                IList<LastCalculatedBalanceDate> dataFromDB = session.CreateQuery("From LastCalculatedBalanceDate").List<LastCalculatedBalanceDate>();
                foreach (LastCalculatedBalanceDate currentAc in dataFromDB)
                {
                    if (!_dicLastCalculatedBalanceDate.ContainsKey(currentAc.GetKey()))
                    {
                        BalanceUpdateDetail objBal = new BalanceUpdateDetail(currentAc.LastCalcDate, currentAc.UpdatedBalance);
                        _dicLastCalculatedBalanceDate.Add(currentAc.GetKey(), objBal);
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
        /// Update the last calculated balance date for the specified accounts in the DB
        /// </summary>
        /// <param name="date"></param>
        /// <param name="accountIDs"></param>
        public void UpdateLastCalculatedBalanceDateToGivenDate(String accountIDs)
        {
            try
            {
                UpdateLastCalcBalanceDateInCache(accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// The lock
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Updates the last revaluation calculated date to given date.
        /// Here date will be send only for manual revaluation and min value for all other places as for that we do not need it. 
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="isUpdated">if set to <c>true</c> [is updated].</param>
        /// <param name="fromRevaluation">if set to <c>true</c> [from revaluation].</param>
        /// <param name="isManualRevaluation">if set to <c>true</c> [is manual revaluation].</param>
        public void UpdateLastRevaluationCalculatedDateToGivenDate(DateTime date, DateTime endDate, String accountIDs, bool isUpdated, bool fromRevaluation, bool isManualRevaluation)
        {
            try
            {
                lock (_lock)
                {
                    Dictionary<int, RevaluationUpdateDetail> _revalDates = CachedDataManager.GetLastRevaluationCalculationDate();
                    using (ISession session = m_SessionFactory.OpenSession())
                    {
                        using (session.BeginTransaction())
                        {
                            IQuery q;
                            if (String.IsNullOrWhiteSpace(accountIDs))
                            {
                                q = session.CreateQuery("Update LastRevaluationCalculationDate set LastCalcDate='" + endDate + "', UpdatedRevaluation=" + isUpdated);
                            }
                            else
                            {
                                if (isManualRevaluation)
                                {
                                    List<int> FundIds = accountIDs.Split(',').Select(int.Parse).ToList();
                                    foreach (int fundID in FundIds)
                                    {
                                        if (DateTime.Compare(date.Date, _revalDates[fundID].LastRevaluationDate.Date) <= 0)
                                        {
                                            q = session.CreateQuery("Update LastRevaluationCalculationDate set LastCalcDate='" + endDate + "',LastCalcDateMW='" + date + "', UpdatedRevaluation=" + isUpdated + " where FundID =" + fundID);
                                            q.ExecuteUpdate();
                                        }
                                    }
                                }
                                else
                                {
                                    //If running revaluation the update the LastCalcDate for revaluation only.
                                    //if Updating Trade, Markprice (Any audit change) the update LastCalcDate for revaluation and MW 
                                    if (fromRevaluation)
                                    {
                                        q = session.CreateQuery("Update LastRevaluationCalculationDate set LastCalcDate='" + endDate + "', UpdatedRevaluation=" + isUpdated + " where FundID in (" + accountIDs + ")");
                                    }
                                    else
                                    {
                                        q = session.CreateQuery("Update LastRevaluationCalculationDate set LastCalcDate='" + endDate + "',LastCalcDateMW='" + endDate + "', UpdatedRevaluation=" + isUpdated + " where FundID in (" + accountIDs + ")");

                                    }
                                    q.ExecuteUpdate();
                                }
                            }

                            if (fromRevaluation)
                            {
                                if (isManualRevaluation)
                                {
                                    List<int> FundIds = accountIDs.Split(',').Select(int.Parse).ToList();
                                    foreach (int fundID in FundIds)
                                    {
                                        if (DateTime.Compare(date.Date, _revalDates[fundID].LastRevaluationDate.Date) <= 0)
                                        {
                                            q = session.CreateQuery("Update LastRevaluationCalculationDate set LastRevalRunDate='" + endDate + "' where FundID =" + fundID);
                                            q.ExecuteUpdate();
                                        }
                                    }
                                }
                                else
                                {
                                    q = session.CreateQuery("Update LastRevaluationCalculationDate set LastRevalRunDate='" + endDate + "' where FundID in (" + accountIDs + ")");
                                    q.ExecuteUpdate();
                                }
                                SetProcessedBitInAuditForRevaluedData(endDate, accountIDs);
                            }
                            session.Flush();
                            session.Transaction.Commit();
                        }
                    }
                    if (isManualRevaluation)
                    {
                        List<int> FundIds = accountIDs.Split(',').Select(int.Parse).ToList();
                        foreach (int fundID in FundIds)
                        {
                            if (DateTime.Compare(date.Date, _revalDates[fundID].LastRevaluationDate.Date) <= 0)
                            {
                                //To make cache in sync with LastRevalCalcDate Table update last calculation date to the given date
                                CachedDataManager.SetLastRevaluationCalculationDate(endDate, fundID.ToString(), isUpdated);
                                //To make cache in sync with LastRevalCalcDate Table auto update last calculation date to the given date(without restarting CM module )
                                CashmgmtServices.PublishRevaluationDate(endDate, fundID.ToString());
                            }
                        }
                    }
                    else
                    {
                        //To make cache in sync with LastRevalCalcDate Table update last calculation date to the given date
                        CachedDataManager.SetLastRevaluationCalculationDate(endDate, accountIDs, isUpdated);
                        //To make cache in sync with LastRevalCalcDate Table auto update last calculation date to the given date(without restarting CM module )
                        CashmgmtServices.PublishRevaluationDate(endDate, accountIDs);
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
        /// Sets the processed bit in audit for revalued data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        private void SetProcessedBitInAuditForRevaluedData(DateTime date, string accountIDs)
        {
            try
            {
                Logger.LoggerWrite("Run Revaluation Status: SetProcessedBitInAuditForRevaluedData started..accountIDs:" + accountIDs + ".. Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SetProcessedBitForRevaluedData";
                queryData.CommandTimeout = 6000;
                queryData.DictionaryDatabaseParameter.Add("@Date", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Date",
                    ParameterType = DbType.String,
                    ParameterValue = date
                });
                queryData.DictionaryDatabaseParameter.Add("@FundIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FundIDs",
                    ParameterType = DbType.String,
                    ParameterValue = accountIDs
                });

                DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                Logger.LoggerWrite("Run Revaluation Status: SetProcessedBitInAuditForRevaluedData Completed..accountIDs:" + accountIDs + ".. Time: " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Modified by: Bharat Raturi, 10 Oct 2014
        /// Update the cache with the updated data in database
        /// </summary>
        public void UpdateLastCalcBalanceDateInCache(String accountIDs)
        {
            try
            {
                //if cache is null then fill cache from DB.
                //Cache may be null if user delete data from T_LastCalculatedBalanceDate
                if (_dicLastCalculatedBalanceDate == null)
                {
                    using (IStatelessSession session = m_SessionFactory.OpenStatelessSession())
                    {
                        using (session.BeginTransaction())
                        {
                            InitializeLastCalculatedBalanceDateCache(session);
                        }
                    }
                }
                //Since in T_LastCalculatedBalanceDate table LastCalcDate is changed to CM Start Date
                //To make cache in sync with LastCalcBalanceDate Table
                else
                {
                    List<KeyValuePair<string, BalanceUpdateDetail>> list = new List<KeyValuePair<string, BalanceUpdateDetail>>(_dicLastCalculatedBalanceDate);
                    List<int> accountsList = accountIDs.Split(',').Select(int.Parse).ToList();

                    foreach (KeyValuePair<string, BalanceUpdateDetail> kvp in list)
                    {
                        int account = int.Parse(kvp.Key.Substring(0, kvp.Key.IndexOf('_')));
                        if (accountsList.Contains(account))
                        {
                            _dicLastCalculatedBalanceDate[kvp.Key].LastBalanceDate = DateTime.Today.Date;
                            _dicLastCalculatedBalanceDate[kvp.Key].isUpdatedBalance = true;
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
        /// Saves the cash currency value.
        /// </summary>
        /// <param name="dateWiseDayEndDictionary">The date wise day end dictionary.</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<CompanyAccountCashCurrencyValue> SaveCashCurrencyValue(Dictionary<string, GenericBindingList<CompanyAccountCashCurrencyValue>> dateWiseDayEndDictionary, List<CompanyAccountCashCurrencyValue> lstDeletedData, string accountIDs)
        {
            try
            {
                lock (_lock)
                {
                    using (ISession session = m_SessionFactory.OpenSession())
                    {
                        using (session.BeginTransaction())
                        {
                            List<CompanyAccountCashCurrencyValue> lsToPublish = new List<CompanyAccountCashCurrencyValue>();
                            if (dateWiseDayEndDictionary.Keys.Count > 0)
                            {
                                foreach (string date in dateWiseDayEndDictionary.Keys)
                                {
                                    //cash management overrides day end data. If here is cash exist in a date that is not being generated by cash management then it must be be cleaned up.
                                    //IQuery q = session.CreateQuery("delete from CompanyAccountCashCurrencyValue where FundID = '" + item.AccountID + "' AND LocalCurrencyID='" + item.LocalCurrencyID + "' AND datediff(dd,Date,'" + item.Date + "')=0 ");
                                    GenericBindingList<CompanyAccountCashCurrencyValue> dataToSave = dateWiseDayEndDictionary[date];
                                    IQuery q;
                                    if (!string.IsNullOrWhiteSpace(accountIDs))
                                    {
                                        q = session.CreateQuery("delete from CompanyAccountCashCurrencyValue where datediff(dd,Date,'" + date + "')=0 and FundID in (" + accountIDs + ")");
                                    }
                                    else
                                    {
                                        q = session.CreateQuery("delete from CompanyAccountCashCurrencyValue where datediff(dd,Date,'" + date + "')=0 ");
                                    }
                                    q.ExecuteUpdate();
                                    foreach (CompanyAccountCashCurrencyValue data in dataToSave)
                                    {
                                        session.Save(data);
                                        lsToPublish.Add(data);
                                    }

                                    if (lstDeletedData != null)
                                    {
                                        var lst = lstDeletedData.Where(item => item.Date.Date == Convert.ToDateTime(date).Date);
                                        foreach (CompanyAccountCashCurrencyValue item in lst)
                                        {
                                            CompanyAccountCashCurrencyValue publishItem = lsToPublish.FirstOrDefault(t => t.AccountID == item.AccountID && t.LocalCurrencyID == item.LocalCurrencyID) as CompanyAccountCashCurrencyValue;
                                            if (publishItem != null)
                                            {
                                                publishItem.CashValueLocal += item.CashValueLocal;
                                                publishItem.CashValueBase += item.CashValueBase;
                                            }
                                            else
                                            {
                                                lsToPublish.Add(item);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (lstDeletedData != null)
                                    lstDeletedData.ForEach(item => lsToPublish.Add(item));
                            }
                            session.Flush();
                            session.Transaction.Commit();
                            return lsToPublish;
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
            return null;
        }

        /// <summary>
        /// Saves the CI value.
        /// </summary>
        /// <param name="dateWiseDayEndDictionary">The date wise day end dictionary.</param>
        /// <param name="lstDeletedData">The LST deleted data.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<CollateralInterestValue> SaveCIValue(Dictionary<string, GenericBindingList<CollateralInterestValue>> dateWiseCIDictionary, List<CollateralInterestValue> lstDeletedData, string accountIDs)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {
                    using (session.BeginTransaction())
                    {
                        List<CollateralInterestValue> lsToPublish = new List<CollateralInterestValue>();
                        if (dateWiseCIDictionary.Keys.Count > 0)
                        {
                            foreach (string date in dateWiseCIDictionary.Keys)
                            {
                                //cash management overrides day end data. If here is cash exist in a date that is not being generated by cash management then it must be be cleaned up.
                                //IQuery q = session.CreateQuery("delete from CompanyAccountCashCurrencyValue where FundID = '" + item.AccountID + "' AND LocalCurrencyID='" + item.LocalCurrencyID + "' AND datediff(dd,Date,'" + item.Date + "')=0 ");
                                GenericBindingList<CollateralInterestValue> dataToSave = dateWiseCIDictionary[date];
                                IQuery q;
                                if (!string.IsNullOrWhiteSpace(accountIDs))
                                {
                                    q = session.CreateQuery("delete from CollateralInterestValue where datediff(dd,Date,'" + date + "')=0 and FundID in (" + accountIDs + ")");
                                }
                                else
                                {
                                    q = session.CreateQuery("delete from CollateralInterestValue where datediff(dd,Date,'" + date + "')=0 ");
                                }
                                q.ExecuteUpdate();
                                foreach (CollateralInterestValue data in dataToSave)
                                {
                                    session.Save(data);
                                    lsToPublish.Add(data);
                                }

                                if (lstDeletedData != null)
                                {
                                    var lst = lstDeletedData.Where(item => item.Date.Date == Convert.ToDateTime(date).Date);
                                    foreach (CollateralInterestValue item in lst)
                                    {
                                        CollateralInterestValue publishItem = lsToPublish.FirstOrDefault(t => t.AccountID == item.AccountID && t.BenchmarkName == item.BenchmarkName) as CollateralInterestValue;
                                        if (publishItem != null)
                                        {
                                            publishItem.BenchmarkName += item.BenchmarkName;
                                        }
                                        else
                                        {
                                            lsToPublish.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (lstDeletedData != null)
                                lstDeletedData.ForEach(item => lsToPublish.Add(item));
                        }
                        session.Flush();
                        session.Transaction.Commit();
                        return lsToPublish;
                    }
                }
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
        /// Added by: Bharat Raturi, 1 aug 2014
        /// Get the journal exceptions from DB 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountIDs"></param>
        /// <returns></returns>
        internal List<T> GetJournalExceptionDataFromDb<T>(DateTime startDate, DateTime endDate, string accountIDs)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {
                    string query = "exec P_GetJournalAllExceptions @startDate=N'" + startDate + "', @endDate=N'" + endDate + "', @fundIDs=N'" + accountIDs + "'";
                    IList<T> data =
                        session.CreateSQLQuery(query).AddEntity(typeof(T)).List<T>();
                    return (List<T>)data;
                }
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
        /// Get the account-wise minimum calculated date from the dictionary holding the minimum calculation dates 
        /// </summary>
        /// <returns>Dictionary holding the account and the least calculated date for that account</returns>
        internal Dictionary<int, BalanceUpdateDetail> GetLastCalculatedBalanceDetails()
        {
            try
            {
                Dictionary<int, BalanceUpdateDetail> dictLastBalCalcDateAccountWise = new Dictionary<int, BalanceUpdateDetail>();
                if (_dicLastCalculatedBalanceDate == null)
                {
                    using (IStatelessSession session = m_SessionFactory.OpenStatelessSession())
                    {
                        InitializeLastCalculatedBalanceDateCache(session);
                    }
                }
                foreach (string key in _dicLastCalculatedBalanceDate.Keys)
                {
                    int accountID = Convert.ToInt32(key.Split('_').ToArray()[0]);
                    if (accountID > 0)
                    {
                        if (dictLastBalCalcDateAccountWise.ContainsKey(accountID))
                        {
                            if (DateTime.Compare(dictLastBalCalcDateAccountWise[accountID].LastBalanceDate, _dicLastCalculatedBalanceDate[key].LastBalanceDate) > 0)
                            {
                                dictLastBalCalcDateAccountWise[accountID].LastBalanceDate = _dicLastCalculatedBalanceDate[key].LastBalanceDate;
                                if (dictLastBalCalcDateAccountWise[accountID].isUpdatedBalance)
                                    dictLastBalCalcDateAccountWise[accountID].isUpdatedBalance = _dicLastCalculatedBalanceDate[key].isUpdatedBalance;
                            }
                            else if (DateTime.Compare(dictLastBalCalcDateAccountWise[accountID].LastBalanceDate, _dicLastCalculatedBalanceDate[key].LastBalanceDate) == 0
                                && dictLastBalCalcDateAccountWise[accountID].isUpdatedBalance)
                            {
                                dictLastBalCalcDateAccountWise[accountID].isUpdatedBalance = _dicLastCalculatedBalanceDate[key].isUpdatedBalance;
                            }
                        }
                        else
                        {
                            dictLastBalCalcDateAccountWise.Add(accountID, _dicLastCalculatedBalanceDate[key]);
                        }
                    }
                }
                return dictLastBalCalcDateAccountWise;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Configures Log4Net to work with NHibernate.
        /// </summary>
        private void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Configures NHibernate and creates a member-level session factory.
        /// </summary>
        private void ConfigureNHibernate()
        {
            // Initialize
            Configuration cfg = new Configuration();
            cfg.Configure();

            /* Note: The AddAssembly() method requires that mappings be 
             * contained in hbm.xml files whose BuildAction properties 
             * are set to �Embedded Resource�. */

            Assembly CashAssembly = typeof(TransactionEntry).Assembly;
            cfg.AddAssembly(CashAssembly);

            // Create session factory from configuration object

            m_SessionFactory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IList<T> GetData<T>(string query)
        {

            using (ISession session = m_SessionFactory.OpenSession())
            {
                IList<T> data = session.CreateQuery(query).List<T>();

                return data;
            }
        }

        /// <summary>
        /// added by: Bharat raturi, 23 jul 2014
        /// purpose: get the day end cash with procedure
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="accountIDs"></param>
        /// <param name="dtStartDate"></param>
        /// <param name="dtEndDate"></param>
        /// <returns></returns>
        internal List<T> GetDayEndCash<T>(string accountIDs, DateTime dtStartDate, DateTime dtEndDate)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                IList<CompanyAccountCashCurrencyValue> data =
                    session.CreateSQLQuery("exec P_GetDayEndCashForDates @fundIDs=N'" + accountIDs + "', @startDate=N'" + dtStartDate +
                    "', @endDate=N'" + dtEndDate + "'").AddEntity(typeof(CompanyAccountCashCurrencyValue)).List<CompanyAccountCashCurrencyValue>();
                return (List<T>)data;
            }
        }

        /// <summary>
        /// Gets the transaction entries for given date.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountIDs">The account i ds.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        internal List<T> GetTransactionEntriesForGivenDate<T>(string accountIDs, DateTime fromDate, DateTime toDate)
        {
            using (ISession session = m_SessionFactory.OpenSession())
            {
                IList<TransactionEntry> data =
                    session.CreateSQLQuery("exec P_GetOnlyCashTransactionEntries @fundIDs=N'" + accountIDs + "', @fromDate=N'" + fromDate +
                    "', @toDate=N'" + toDate + "'").AddEntity(typeof(TransactionEntry)).SetTimeout(timeoutForQuery).List<TransactionEntry>();
                return (List<T>)data;
            }
        }

        #endregion
    }
}
