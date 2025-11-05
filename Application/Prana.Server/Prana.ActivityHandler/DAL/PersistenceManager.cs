using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Prana.ActivityHandler
{
    /// <summary>
    /// Specifies whether to begin a new session, continue an existing session, or end an existing session.
    /// </summary>
    /// 
    public enum SessionAction { Begin, Continue, End, BeginAndEnd }

    public class PersistenceManager : IDisposable
    {
        #region Declarations

        // Member variables
        private ISessionFactory m_SessionFactory = null;
        private NHibernate.ISession m_Session = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PersistenceManager()
        {
            this.ConfigureNHibernate();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
            using (NHibernate.ISession session = m_SessionFactory.OpenSession())
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
            using (NHibernate.ISession session = m_SessionFactory.OpenSession())
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
            using (NHibernate.ISession session = m_SessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));
                criteria.Add(Expression.Eq(propertyName, propertyValue));
                // Get the matching objects
                IList<T> matchingObjects = criteria.List<T>();
                session.Flush();
                // Set return value
                return matchingObjects;
            }
        }

        public IList<T> RetrieveEquals<T>(T obj, string matchingPattern)
        {
            using (NHibernate.ISession session = m_SessionFactory.OpenSession())
            {
                // Create a criteria object with the specified criteria
                ICriteria criteria = session.CreateCriteria(typeof(T));
                if (matchingPattern == "BYTaxlotIDAndFromDate")
                {
                    CashActivity cashActivity = obj as CashActivity;
                    if (cashActivity != null)
                    {
                        criteria.Add(Expression.Gt("Date", cashActivity.Date));
                        criteria.Add(Expression.Eq("ActivitySource", CashTransactionType.DailyCalculation));
                    }
                }
                // Get the matching objects
                IList<T> matchingObjects = criteria.List<T>();
                session.Flush();
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
                using (NHibernate.ISession session = m_SessionFactory.OpenSession())
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
        /// Saves the or update.
        /// </summary>
        /// <param name="lsCashActivity">The ls cash activity.</param>
        public void SaveOrUpdate(List<CashActivity> lsCashActivity)
        {
            try
            {
                using (IStatelessSession session = m_SessionFactory.OpenStatelessSession())
                {
                    using (session.BeginTransaction())
                    {
                        List<string> deletedTaxlotIDs = new List<string>();
                        List<string> listtaxlotIDstoDelete = new List<string>();
                        List<CashActivity> deletedItemsTaxlotIDWise = new List<CashActivity>();
                        StringBuilder _dataforUpdatedRevaluationXML = new StringBuilder();
                        _dataforUpdatedRevaluationXML.Append("<Data>");
                        StringBuilder _dataforDeletedRevaluationXML = new StringBuilder();
                        _dataforDeletedRevaluationXML.Append("<Data>");
                        foreach (CashActivity cashActivity in lsCashActivity)
                        {
                            if (cashActivity.TransactionSource == CashTransactionType.Trading && cashActivity.ActivityState == ApplicationConstants.TaxLotState.Updated)
                            {
                                if (DateTime.Compare(cashActivity.Date.Date, DateTime.Today) < 0)
                                {
                                    if (!listtaxlotIDstoDelete.Contains(cashActivity.FKID))
                                    {
                                        listtaxlotIDstoDelete.Add(cashActivity.FKID);
                                    }
                                    CreateRevaluationXML(ref _dataforUpdatedRevaluationXML, cashActivity);
                                }
                            }
                            if (cashActivity.TransactionSource == CashTransactionType.Trading && cashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted)
                            {
                                if (!deletedTaxlotIDs.Contains(cashActivity.FKID))
                                {
                                    //TODO: istradeattribute field
                                    string sql = "From CashActivity where FKID='" + cashActivity.FKID + "'";
                                    IList<CashActivity> lsDeletedCActivity = session.CreateQuery(sql).List<CashActivity>();
                                    if (lsDeletedCActivity != null && lsDeletedCActivity.Count > 0)
                                    {
                                        foreach (CashActivity deletedCashActivity in lsDeletedCActivity)
                                        {
                                            if (deletedCashActivity.UniqueKey != cashActivity.UniqueKey)
                                            {
                                                deletedCashActivity.ActivityState = ApplicationConstants.TaxLotState.Deleted;
                                                deletedItemsTaxlotIDWise.Add(deletedCashActivity);
                                            }
                                            else
                                                cashActivity.ActivityId = deletedCashActivity.ActivityId;
                                        }
                                        //DeleteAllEntries(cashActivity.FKID, session);
                                        deletedTaxlotIDs.Add(cashActivity.FKID);
                                    }
                                }
                                CreateRevaluationXML(ref _dataforDeletedRevaluationXML, cashActivity);
                            }
                            else
                            {
                                int count = 0;
                                // When trades are coming from fix then we receive taxlots with same taxlotid and updated execution quantity and taxlot state new.
                                //At activity level we are dealing using unique key (Taxlotid + date + activitynumber + transaction source), so we can identify fills of same taxlot and delete the data.
                                //But at journal level because of changes activity id we are unable to update journals and they are generated new each time.
                                //So now are checking data in db that data for that unique key exists or not, if data for that unique key exists then we set state to updated and journals are also updated

                                // PRANA-13860
                                // KashishG.
                                // In case if the data is coming from Daily Calculation for Deleted Activity State,we can skip this inner function
                                // as the Activity ID will be same and it will not have impact in anyway,instead we can directly Delete the cash Activity,
                                // so we have skip this function in case if Transaction Source is Daily Calculation.
                                if ((cashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted && (int)cashActivity.TransactionSource != 3) ||
                                      cashActivity.ActivityState == ApplicationConstants.TaxLotState.Updated ||
                                  (cashActivity.ActivityState == ApplicationConstants.TaxLotState.New && cashActivity.TransactionSource == CashTransactionType.Trading))
                                {
                                    string query = "Select ActivityId From CashActivity where UniqueKey='" + cashActivity.UniqueKey + "'";
                                    IList<string> lsCActivity = session.CreateQuery(query).List<string>();
                                    count = lsCActivity.Count;
                                    if (lsCActivity != null && lsCActivity.Count > 0)
                                    {
                                        cashActivity.ActivityId = lsCActivity[0];
                                        if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.New)
                                        {
                                            cashActivity.ActivityState = ApplicationConstants.TaxLotState.Updated;
                                        }
                                    }
                                }
                                if (((cashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted || cashActivity.ActivityState == ApplicationConstants.TaxLotState.Updated) && count > 0) || (cashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted && (int)cashActivity.TransactionSource == 3))
                                {
                                    session.Delete(cashActivity);
                                }
                                if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.New && cashActivity.TransactionSource == CashTransactionType.TradeImport)
                                {
                                    cashActivity.TransactionSource = CashTransactionType.Trading;
                                    session.Insert(cashActivity);
                                }
                                else if (cashActivity.ActivityState != ApplicationConstants.TaxLotState.Deleted)
                                {
                                    if (cashActivity.TransactionSource == CashTransactionType.TradeImport)
                                    {
                                        cashActivity.TransactionSource = CashTransactionType.Trading;
                                    }
                                    session.Insert(cashActivity);
                                }
                            }
                        }

                        _dataforUpdatedRevaluationXML.Append("</Data>");
                        _dataforDeletedRevaluationXML.Append("</Data>");

                        // PRANA-13860
                        // KashishG.
                        // Here we are deleting the Data on the basis of FKID contained in the "deletedTaxlotIDs" at one time.
                        // Earlier the data was getting deleted one by one and every time it calls the Database which is time taking.
                        // Now it will just hit the Database one time and deletes the Data at once.
                        if (deletedTaxlotIDs.Count > 0)
                        {
                            string activities = string.Join(",", deletedTaxlotIDs);
                            string deleteactivities = "'" + activities.Replace(",", "','") + "'";
                            DeleteAllEntries(deleteactivities, session);
                        }
                        if (deletedItemsTaxlotIDWise.Count > 0)
                            lsCashActivity.AddRange(deletedItemsTaxlotIDWise);

                        lsCashActivity.RemoveAll(delegate (CashActivity deletedCashActivity)
                                   {
                                       return deletedCashActivity.TransactionSource == CashTransactionType.Closing && deletedCashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted && string.IsNullOrEmpty(deletedCashActivity.UniqueKey);
                                   });

                        session.Transaction.Commit();
                        if (listtaxlotIDstoDelete.Count > 0)
                        {
                            DeleteTradingandRevaluationEntries(string.Empty, _dataforUpdatedRevaluationXML);
                        }
                        if (deletedTaxlotIDs.Count > 0)
                        {
                            DeleteTradingandRevaluationEntries(string.Empty, _dataforDeletedRevaluationXML);
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
        /// Creates the revaluation XML.
        /// </summary>
        /// <param name="revalXML">The reval XML.</param>
        /// <param name="cashActivity">The cash activity.</param>
        private static void CreateRevaluationXML(ref StringBuilder revalXML, CashActivity cashActivity)
        {
            try
            {
                revalXML.Append("<CashActivity>");
                revalXML.Append("<AccountID>");
                revalXML.Append(cashActivity.AccountID);
                revalXML.Append("</AccountID>");
                revalXML.Append("<Symbol>");
                revalXML.Append(cashActivity.Symbol);
                revalXML.Append("</Symbol>");
                revalXML.Append("<Date>");
                revalXML.Append(cashActivity.Date);
                revalXML.Append("</Date>");
                revalXML.Append("</CashActivity>");
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
        /// <param name="FKID">The fkid.</param>
        /// <param name="session">The session.</param>
        private void DeleteAllEntries(string FKID, IStatelessSession session)
        {
            try
            {
                IQuery q = session.CreateQuery("delete from CashActivity where FKID IN (" + FKID + ")");
                q.ExecuteUpdate();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Deletes the tradingand revaluation entries.
        /// </summary>
        /// <param name="FKID">The fkid.</param>
        /// <param name="data">The data.</param>
        private void DeleteTradingandRevaluationEntries(string FKID, StringBuilder data)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_DeleteTradingAndRevaluationActivities";
                queryData.CommandTimeout = 1000;
                queryData.DictionaryDatabaseParameter.Add("@FKID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@FKID",
                    ParameterType = DbType.String,
                    ParameterValue = FKID
                });
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.String,
                    ParameterValue = data.ToString()
                });

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
        /// Configures NHibernate and creates a member-level session factory.
        /// </summary>
        private void ConfigureNHibernate()
        {
            // Initialize
            Configuration cfg = new Configuration();
            cfg.Configure();

            /* Note: The AddAssembly() method requires that mappings be 
             * contained in hbm.xml files whose BuildAction properties 
             * are set to ‘Embedded Resource’. */

            Assembly CashAssembly = typeof(TransactionEntry).Assembly;
            cfg.AddAssembly(CashAssembly);
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
            using (NHibernate.ISession session = m_SessionFactory.OpenSession())
            {
                IList<T> data = session.CreateQuery(query).List<T>();
                return data;
            }
        }

        /// <summary>
        /// Added by: Bharat raturi, 19 aug 2014
        /// Get overriding activities for creating journals from DB 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="accountIDs"></param>
        /// <returns></returns>
        internal List<CashActivity> GetActivityForJournalFromDB<CashActivity>(DateTime fromDate, DateTime toDate, string accountIDs, string activityDateType, bool isActivitySource)
        {
            try
            {
                using (m_Session = m_SessionFactory.OpenSession())
                {
                    string query = "exec P_GetAllActivities @fromdate=N'" + fromDate + "', @todate=N'" + toDate + "', @fundIDs=N'" + accountIDs + "', @activitydatetype=N'" + activityDateType + "', @isActivitySource=N'" + isActivitySource + "'";
                    IList<CashActivity> data =
                        m_Session.CreateSQLQuery(query).AddEntity(typeof(CashActivity)).List<CashActivity>();
                    return (List<CashActivity>)data;
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
    }
}
