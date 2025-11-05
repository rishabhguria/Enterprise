using NHibernate;
using NHibernate.Cfg;
//using NHibernate.Expression;
using NHibernate.Criterion;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTrade.BLL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Prana.PostTrade
{
    /// <summary>
    /// Specifies whether to begin a new session, continue an existing session, or end an existing session.
    /// </summary>
    public enum SessionAction { Begin, Continue, End, BeginAndEnd }

    public class PersistenceManager : IDisposable
    {
        #region Declarations

        // Member variables
        private ISessionFactory m_SessionFactory = null;
        int _timeoutOnAllocationNumeric;
        private ISession m_Session = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PersistenceManager()
        {
            try
            {
                string timeoutOnAllocation = ConfigurationHelper.Instance.GetAppSettingValueByKey("QueryTimeoutForAllocation");
                if (!string.IsNullOrEmpty(timeoutOnAllocation) && int.TryParse(timeoutOnAllocation, out _timeoutOnAllocationNumeric))
                {
                    if (_timeoutOnAllocationNumeric < 30)
                        _timeoutOnAllocationNumeric = 60;
                }
                else
                {
                    _timeoutOnAllocationNumeric = 60;
                }
                this.ConfigureLog4Net();
                this.ConfigureNHibernate();
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

        #region Public Methods

        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set
            {
                _secMasterServices = value;
            }
        }

        private IClosingServices _closingServices;
        public IClosingServices ClosingServices
        {
            set
            {
                _closingServices = value;
            }

        }
        /// <summary>
        /// Clears all records from all tables in the database
        /// </summary>
        public void ClearDatabase()
        {
            // Initialize
            //SqlConnection connection = m_SessionFactory.ConnectionProvider.GetConnection() as SqlConnection;
            //SqlCommand command = null;
            //string[] dataTables = new string[] { "OrderItems", "Products", "Orders", "Customers" };
            //string sql = null;

            //// Delete all records from all tables
            //using (connection)
            //{
            //    // Iterate tables
            //    for (int i = 0; i < dataTables.Length; i++)
            //    {
            //        // Build query and command, and execute
            //        sql = String.Format("Delete from {0}", dataTables[i]);
            //        command = new SqlCommand(sql, connection);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

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
            using (ISession session = m_SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(item);
                    session.Transaction.Commit();
                }
            }
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
            try
            {
                // Initialize
                Configuration cfg = new Configuration();
                cfg.Configure();

                /* Note: The AddAssembly() method requires that mappings be 
                 * contained in hbm.xml files whose BuildAction properties 
                 * are set to ?Embedded Resource?. */

                // Add class mappings to configuration object
                //Assembly thisAssembly = typeof(Customer).Assembly;
                Assembly allocationAssembly = typeof(AllocationGroup).Assembly;
                cfg.AddAssembly(allocationAssembly);
                // cfg.AddAssembly(thisAssembly);

                // Create session factory from configuration object
                m_SessionFactory = cfg.BuildSessionFactory();
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
        IList<AllocationGroup> groups;
        public List<AllocationGroup> GetGroups(string groupID)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {

                    StringBuilder query = new StringBuilder();
                    query.Append("from AllocationGroup g ");//This needs to be changed probably by joining tables
                    ///* Extra join to reduce no of queries
                    query.Append(" left outer join fetch g.Level1AllocationList l");
                    query.Append(" left outer join fetch l.TaxLotsH ");
                    query.Append(" left outer join fetch g.OrdersH left outer join fetch g.SwapParametersH ");
                    query.Append(" where g.GroupID in (" + groupID + ")");
                    IQuery iQuery = session.CreateQuery(query.ToString());
                    iQuery.SetTimeout(_timeoutOnAllocationNumeric);
                    iQuery.SetReadOnly(true);
                    UniqueResultTransformer transformer = new UniqueResultTransformer();//This custom transformer removes duplicate data.
                    iQuery.SetResultTransformer(transformer);
                    groups = iQuery.List<AllocationGroup>();
                    //   groups = (List<AllocationGroup>)session.CreateQuery("from AllocationGroup where GroupID='" + groupID +"'").List<AllocationGroup>();                    
                    foreach (AllocationGroup group in groups)
                    {
                        _secMasterServices.SetSecuritymasterDetails(group);
                        group.TaxLots = new List<TaxLot>();
                        foreach (Level1Allocation level1Allocation in group.Level1AllocationList)
                        {
                            AllocationLevelClass level1 = new AllocationLevelClass(level1Allocation.GroupID);
                            #region Setting Properties For Level 1

                            level1.AllocatedQty = level1Allocation.AllocatedQty;
                            //level1.LevelnAllocationID = level1Allocation.LevelnAllocationID;//ReadOnly Property
                            level1.LevelnID = Convert.ToInt32(level1Allocation.AccountID);
                            //level1.Name=level1Allocation.
                            level1.Percentage = level1Allocation.Percentage;

                            foreach (TaxLot taxlot in level1Allocation.TaxLotsH)
                            {
                                taxlot.CopyBasicDetails(group);

                                taxlot.Level1ID = level1.LevelnID;

                                group.TaxLots.Add(taxlot);
                                AllocationLevelClass level2 = new AllocationLevelClass(taxlot.GroupID);
                                level2.AllocatedQty = taxlot.TaxLotQty;

                                //level2.GroupID = taxlot.GroupID;
                                //level2.LevelnAllocationID = taxlot.Level1AllocationID;
                                level2.LevelnID = taxlot.Level2ID;
                                //level2.Name = taxlot.Level2Name;
                                if (taxlot.Level2Percentage != 0)
                                {
                                    level2.Percentage = taxlot.Level2Percentage;
                                }
                                level1.AddChilds(level2);
                            }

                            group.Allocations.Add(level1);

                            #endregion
                        }
                        //group.CalculateTaxLots();
                    }

                    return (List<AllocationGroup>)groups;
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
                return new List<AllocationGroup>();
            }
        }

        internal List<AllocationGroup> GetTaxlotParentGroup(string taxlotID)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {
                    groups = (List<AllocationGroup>)session.CreateQuery("from AllocationGroup where GroupID in (Select Distinct GroupID from TaxLot where TaxlotID='" + taxlotID + "')").List<AllocationGroup>();
                    foreach (AllocationGroup group in groups)
                    {
                        _secMasterServices.SetSecuritymasterDetails(group);
                        group.TaxLots = new List<TaxLot>();
                        foreach (Level1Allocation level1Allocation in group.Level1AllocationList)
                        {
                            AllocationLevelClass level1 = new AllocationLevelClass(level1Allocation.GroupID);
                            #region Setting Properties For Level 1

                            level1.AllocatedQty = level1Allocation.AllocatedQty;
                            //level1.LevelnAllocationID = level1Allocation.LevelnAllocationID;//ReadOnly Property
                            level1.LevelnID = Convert.ToInt32(level1Allocation.AccountID);
                            //level1.Name=level1Allocation.
                            level1.Percentage = level1Allocation.Percentage;

                            foreach (TaxLot taxlot in level1Allocation.TaxLotsH)
                            {
                                taxlot.CopyBasicDetails(group);

                                taxlot.Level1ID = level1.LevelnID;
                                group.TaxLots.Add(taxlot);
                                AllocationLevelClass level2 = new AllocationLevelClass(taxlot.GroupID);
                                level2.AllocatedQty = taxlot.TaxLotQty;

                                //level2.GroupID = taxlot.GroupID;
                                //level2.LevelnAllocationID = taxlot.Level1AllocationID;
                                level2.LevelnID = taxlot.Level2ID;
                                //level2.Name = taxlot.Level2Name;
                                if (taxlot.Level2Percentage != 0)
                                {
                                    level2.Percentage = taxlot.Level2Percentage;
                                }
                                level1.AddChilds(level2);
                            }
                            group.Allocations.Add(level1);

                            #endregion
                        }
                        //group.CalculateTaxLots();
                    }

                    return (List<AllocationGroup>)groups;
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
                return new List<AllocationGroup>();
            }
        }

        /// <summary>
        /// Get groups from database on the basis of 2 date range
        /// This method is called from commission form. If we open commission form directly from main menu, them current date is passed
        /// </summary>
        /// <param name="ToAllAUECDatesString"></param>
        /// <param name="FromAllAUECDatesString"></param>
        /// <returns></returns>
        public List<AllocationGroup> GetGroups(string ToAllAUECDatesString, string FromAllAUECDatesString, string accountIDs)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {
                    session.Clear();
                    //ToAllAUECDatesString = DateTime.Now.AddMonths(1).ToString();
                    //FromAllAUECDatesString = DateTime.Now.ToString();
                    //string s = "from AllocationGroup where (StateID=1 and DATEDIFF(d,AUECLocalDate,'" + ToAllAUECDatesString + "')>=0 )  or (StateID=2 and DATEDIFF(d,AllocationDate,'" + ToAllAUECDatesString + "')>=0 and DATEDIFF(d,AllocationDate,'" + FromAllAUECDatesString + "')<=0) ";


                    //groups =(List<AllocationGroup>) session.CreateQuery("from AllocationGroup").List<AllocationGroup>();

                    //groups = (List<AllocationGroup>)session.CreateQuery("from AllocationGroup g left outer join fetch g.Level1AllocationList as l left outer join fetch g.Level1AllocationList as l left outer join fetch g.OrdersH left outer join fetch g.SwapParametersH where (g.StateID=1 and DATEDIFF(d,g.AUECLocalDate,'" + ToAllAUECDatesString + "')>=0 and g.CumQty>0 )  or (g.StateID=2 and DATEDIFF(d,g.AllocationDate,'" + ToAllAUECDatesString + "')>=0 and DATEDIFF(d,g.AllocationDate,'" + FromAllAUECDatesString + "')<=0 and g.CumQty>0)").List<AllocationGroup>();

                    //IEnumerable<TaxLot> txlt = session.CreateQuery("from AllocationGroup g left outer join fetch g.Level1AllocationList.TaxLotsH").Future<TaxLot>();
                    StringBuilder query = new StringBuilder();
                    query.Append("from AllocationGroup g ");//This needs to be changed probably by joining tables
                    ///* Extra join to reduce no of queries
                    query.Append(" left outer join fetch g.Level1AllocationList l");
                    query.Append(" left outer join fetch l.TaxLotsH ");
                    query.Append(" left outer join fetch g.OrdersH left outer join fetch g.SwapParametersH ");
                    query.Append(" where (g.StateID=1 and DATEDIFF(d,g.AUECLocalDate,'");
                    query.Append(ToAllAUECDatesString);
                    query.Append("')>=0 and g.CumQty>0 )  or (g.StateID=2 and DATEDIFF(d,g.AllocationDate,'");
                    query.Append(ToAllAUECDatesString);
                    query.Append("')>=0 and DATEDIFF(d,g.AllocationDate,'");
                    query.Append(FromAllAUECDatesString);
                    query.Append("')<=0 and g.CumQty>0)");
                    if (!string.IsNullOrWhiteSpace(accountIDs))
                    {
                        query.Append(" and l.AccountID in (" + accountIDs + ")");
                    }
                    IQuery iQuery = session.CreateQuery(query.ToString());
                    iQuery.SetTimeout(_timeoutOnAllocationNumeric);
                    iQuery.SetReadOnly(true);
                    UniqueResultTransformer transformer = new UniqueResultTransformer();//This custom transformer removes duplicate data.
                    iQuery.SetResultTransformer(transformer);
                    groups = iQuery.List<AllocationGroup>();
                    //session.Dispose();
                    foreach (AllocationGroup group in groups)
                    {
                        group.FillAdditionalParameters();//this method assign two more variables _swapParameters and _orders after initialization 
                        _secMasterServices.SetSecuritymasterDetails(group);

                        group.OrderCount = group.Orders.Count;
                        group.TaxLots = new List<TaxLot>();
                        foreach (Level1Allocation level1Allocation in group.Level1AllocationList)
                        {
                            AllocationLevelClass level1 = new AllocationLevelClass(level1Allocation.GroupID);

                            #region Setting Properties For Level 1

                            level1.AllocatedQty = level1Allocation.AllocatedQty;
                            //level1.LevelnAllocationID = level1Allocation.LevelnAllocationID;//ReadOnly Property
                            level1.LevelnID = Convert.ToInt32(level1Allocation.AccountID);
                            //level1.Name=level1Allocation.
                            level1.Percentage = level1Allocation.Percentage;

                            foreach (TaxLot taxlot in level1Allocation.TaxLotsH)
                            {
                                taxlot.CopyBasicDetails(group);

                                taxlot.Level1ID = level1.LevelnID;
                                taxlot.Percentage = level1.Percentage;

                                // update taxlot closing status i.e. Open. Closed or Partially closed
                                // we use closing services in persistence manager to make code more optimized and to avoid looping through
                                _closingServices.SetTaxlotClosingStatus(taxlot);

                                group.TaxLots.Add(taxlot);
                                AllocationLevelClass level2 = new AllocationLevelClass(taxlot.GroupID);
                                level2.AllocatedQty = taxlot.TaxLotQty;
                                //taxlot.Level2Percentage=(float) (taxlot.TaxLotQty / level1.AllocatedQty);
                                //level2.GroupID = taxlot.GroupID;
                                //level2.LevelnAllocationID = taxlot.Level1AllocationID;
                                level2.LevelnID = taxlot.Level2ID;
                                //level2.Name = taxlot.Level2Name;
                                if (taxlot.Level2Percentage != 0)
                                {
                                    level2.Percentage = taxlot.Level2Percentage;
                                }
                                level1.AddChilds(level2);
                            }
                            group.Allocations.Add(level1);
                            // Update group status based on its taxlot(s) status
                            UpdateGroupClosingStatus(group);

                            #endregion
                        }
                    }

                    return (List<AllocationGroup>)groups;
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
                return new List<AllocationGroup>();
            }
        }

        /// <summary>
        /// Update group status based on its taxlot(s) status
        /// if any taxlot is partially closed then we set group closing status to partially closed
        /// </summary>
        /// <param name="group"></param>
        private void UpdateGroupClosingStatus(AllocationGroup group)
        {
            try
            {
                //assume group closing status is open by default   
                group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Open;
                //update group status whether group is generated by Option Exercise or Corp Action
                UpdateGroupStatus(group);

                if (group.TaxLots.Count > 0)
                {
                    int closeCount = 0;
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        //update the minimum of close trade of all taxlots in a group
                        //AUECMODIFIEDDATE IS TRADE DATE
                        if (taxlot.AUECModifiedDate != DateTimeConstants.MinValue && (group.ClosingDate > taxlot.AUECModifiedDate || group.ClosingDate == DateTimeConstants.MinValue))
                        {
                            group.ClosingDate = taxlot.AUECModifiedDate;
                        }
                        //if any one of the group taxlot is partially closed then group will be partially closed
                        if ((taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed))
                        {
                            group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
                            closeCount++;
                            break;
                        }
                        if (taxlot.ClosingStatus == Prana.BusinessObjects.AppConstants.ClosingStatus.Closed)
                        {
                            closeCount += 2;
                        }
                    }

                    if ((closeCount / 2) == group.TaxLots.Count)// in case of closed taxlots, closeCount is always incremented by 2
                        group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.Closed;
                    else if ((closeCount / 2) > 0)//if closeCount is greater than 0 (closeCount is always incremented by 1)
                        group.ClosingStatus = Prana.BusinessObjects.AppConstants.ClosingStatus.PartiallyClosed;
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
        }

        /// <summary>
        /// here we update group status whether group is generated by Option Exercise or Corp Action
        /// </summary>
        /// <param name="group"></param>
        private void UpdateGroupStatus(AllocationGroup group)
        {
            try
            {
                PostTradeEnums.Status groupStatus = _closingServices.CheckGroupStatus(group);
                if (groupStatus.Equals(PostTradeEnums.Status.Closed))
                {
                    group.GroupStatus = PostTradeEnums.Status.None;
                }
                else
                {
                    group.GroupStatus = groupStatus;
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
        }

        /// <summary>
        /// Get groups based on query i.e. date range and symbol/account etc
        /// This method is called from main allocation UI
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<AllocationGroup> GetGroupsOnQuery(String query)
        {
            try
            {
                using (ISession session = m_SessionFactory.OpenSession())
                {
                    session.Clear();
                    //ToAllAUECDatesString = DateTime.Now.AddMonths(1).ToString();
                    //FromAllAUECDatesString = DateTime.Now.ToString();
                    //string s = "from AllocationGroup where (StateID=1 and DATEDIFF(d,AUECLocalDate,'" + ToAllAUECDatesString + "')>=0 )  or (StateID=2 and DATEDIFF(d,AllocationDate,'" + ToAllAUECDatesString + "')>=0 and DATEDIFF(d,AllocationDate,'" + FromAllAUECDatesString + "')<=0) ";


                    //groups =(List<AllocationGroup>) session.CreateQuery("from AllocationGroup").List<AllocationGroup>();

                    //groups = (List<AllocationGroup>)session.CreateQuery("from AllocationGroup g left outer join fetch g.Level1AllocationList as l left outer join fetch g.Level1AllocationList as l left outer join fetch g.OrdersH left outer join fetch g.SwapParametersH where (g.StateID=1 and DATEDIFF(d,g.AUECLocalDate,'" + ToAllAUECDatesString + "')>=0 and g.CumQty>0 )  or (g.StateID=2 and DATEDIFF(d,g.AllocationDate,'" + ToAllAUECDatesString + "')>=0 and DATEDIFF(d,g.AllocationDate,'" + FromAllAUECDatesString + "')<=0 and g.CumQty>0)").List<AllocationGroup>();

                    //IEnumerable<TaxLot> txlt = session.CreateQuery("from AllocationGroup g left outer join fetch g.Level1AllocationList.TaxLotsH").Future<TaxLot>();
                    IQuery iQuery = session.CreateQuery(query);
                    iQuery.SetTimeout(_timeoutOnAllocationNumeric);
                    iQuery.SetReadOnly(true);
                    UniqueResultTransformer transformer = new UniqueResultTransformer();//This custom transformer removes duplicate data.
                    iQuery.SetResultTransformer(transformer);
                    groups = iQuery.List<AllocationGroup>();
                    //session.Dispose();
                    foreach (AllocationGroup group in groups)
                    {
                        group.FillAdditionalParameters();//this method assign two more variables _swapParameters and _orders after initializaion 
                        _secMasterServices.SetSecuritymasterDetails(group);

                        group.OrderCount = group.Orders.Count;
                        group.TaxLots = new List<TaxLot>();
                        foreach (Level1Allocation level1Allocation in group.Level1AllocationList)
                        {
                            AllocationLevelClass level1 = new AllocationLevelClass(level1Allocation.GroupID);
                            #region Setting Properties For Level 1

                            level1.AllocatedQty = level1Allocation.AllocatedQty;
                            //level1.LevelnAllocationID = level1Allocation.LevelnAllocationID;//ReadOnly Property
                            level1.LevelnID = Convert.ToInt32(level1Allocation.AccountID);
                            //level1.Name=level1Allocation.
                            level1.Percentage = level1Allocation.Percentage;

                            foreach (TaxLot taxlot in level1Allocation.TaxLotsH)
                            {
                                taxlot.CopyBasicDetails(group);

                                taxlot.Level1ID = level1.LevelnID;
                                taxlot.Percentage = level1.Percentage;
                                // update taxlot closing status i.e. Open, Closed or Partially closed
                                // we use closing services in persistence manager to make code more optimized and to avoid looping through
                                _closingServices.SetTaxlotClosingStatus(taxlot);
                                group.TaxLots.Add(taxlot);
                                AllocationLevelClass level2 = new AllocationLevelClass(taxlot.GroupID);
                                level2.AllocatedQty = taxlot.TaxLotQty;
                                //taxlot.Level2Percentage=(float) (taxlot.TaxLotQty / level1.AllocatedQty);
                                //level2.GroupID = taxlot.GroupID;
                                //level2.LevelnAllocationID = taxlot.Level1AllocationID;
                                level2.LevelnID = taxlot.Level2ID;
                                //level2.Name = taxlot.Level2Name;
                                if (taxlot.Level2Percentage != 0)
                                {
                                    level2.Percentage = taxlot.Level2Percentage;
                                }
                                level1.AddChilds(level2);
                            }
                            group.Allocations.Add(level1);

                            // Update group status based on its taxlot(s) status
                            UpdateGroupClosingStatus(group);
                            #endregion




                        }
                    }


                    return (List<AllocationGroup>)groups;
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
                return new List<AllocationGroup>();
            }
        }


        #endregion


    }
}
