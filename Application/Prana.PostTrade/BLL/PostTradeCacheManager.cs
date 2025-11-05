using Prana.Allocation.Common.Constants;
using Prana.Allocation.Common.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTrade.Commission;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.ServiceModel;
using System.Threading;

namespace Prana.PostTrade
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class PostTradeCacheManager : IAllocationServices, IDisposable
    {
        /// <summary>
        /// The data cached variable check
        /// TODO: Vinod remove it
        /// </summary>
        bool _dataCached = false;

        /// <summary>
        /// The commission calculation time
        /// </summary>
        private bool _commissionCalculationTime = false;

        /// <summary>
        /// The allocated trade audit collection
        /// </summary>
        List<TradeAuditEntry> _allocatedTradeAuditCollection = new List<TradeAuditEntry>();

        /// <summary>
        /// The session services
        /// </summary>
        private ISessionServices _sessionServices;

        /// <summary>
        /// Gets or sets the session services.
        /// </summary>
        /// <value>
        /// The session services.
        /// </value>
        public ISessionServices SessionServices
        {
            get { return _sessionServices; }
            set { _sessionServices = value; }
        }

        /// <summary>
        /// The m_persistence manager
        /// Modified by: Bharat raturi, 
        /// After discussion with gaurav, made the object static as instance object was creating problems while getting the groups.
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5157        
        /// </summary>
        static PersistenceManager _m_PersistenceManager;

        //TODO - need to check here, this constructor called 3 times on starting server- Omshiv        
        /// <summary>
        /// Initializes a new instance of the <see cref="PostTradeCacheManager"/> class.
        /// </summary>
        public PostTradeCacheManager()
        {
            try
            {
                Initlise();
                CreateClosingServicesProxy();
                CreatePublishingProxy();
                CreatePositionManagementProxy();
                if (_m_PersistenceManager == null)
                    _m_PersistenceManager = new PersistenceManager();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates the closing services proxy.
        /// </summary>
        private void CreateClosingServicesProxy()
        {

            string endpointAddressInString = ConfigurationManager.AppSettings["ClosingEndpointAddress"];
        }

        /// <summary>
        /// The proxy publishing
        /// </summary>
        static ProxyBase<IPublishing> _proxyPublishing;

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The allocation manager
        /// </summary>
        private IAllocationManager _allocationManager;

        /// <summary>
        /// Sets the allocation manager.
        /// </summary>
        /// <value>
        /// The allocation manager.
        /// </value>
        public IAllocationManager AllocationManager
        {
            set { _allocationManager = value; }
        }

        /// <summary>
        /// The closing services
        /// </summary>
        private IClosingServices _closingServices;

        /// <summary>
        /// Sets the closing services.
        /// </summary>
        /// <value>
        /// The closing services.
        /// </value>
        public IClosingServices ClosingServices
        {
            set
            {
                _closingServices = value;
                if (_m_PersistenceManager != null)
                    _m_PersistenceManager.ClosingServices = value;
            }
        }

        /// <summary>
        /// The cash management service
        /// </summary>
        private ICashManagementService _CashManagementService;

        /// <summary>
        /// Sets the cash management services.
        /// </summary>
        /// <value>
        /// The cash management services.
        /// </value>
        public ICashManagementService CashManagementServices
        {
            set { _CashManagementService = value; }
        }

        /// <summary>
        /// The activity services
        /// </summary>
        private IActivityServices _activityServices;

        /// <summary>
        /// Sets the activity services.
        /// </summary>
        /// <value>
        /// The activity services.
        /// </value>
        public IActivityServices ActivityServices
        {
            set { _activityServices = value; }
        }

        #region Proxy Section

        /// <summary>
        /// The position management services
        /// </summary>
        static ProxyBase<IPranaPositionServices> _positionManagementServices = null;

        /// <summary>
        /// Gets or sets the position management services.
        /// </summary>
        /// <value>
        /// The position management services.
        /// </value>
        public static ProxyBase<IPranaPositionServices> PositionManagementServices
        {
            set { _positionManagementServices = value; }
            get { return _positionManagementServices; }
        }

        /// <summary>
        /// Creates the position management proxy.
        /// </summary>
        public static void CreatePositionManagementProxy()
        {
            try
            {
                PositionManagementServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Position Management Proxy", ex), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Initlises this instance.
        /// </summary>
        private void Initlise()
        {
            LoadCommisionRules();
        }

        /// <summary> 
        /// fills up commsion rules and caches it only on first time
        /// </summary>
        /// <param name="loginUser"></param>
        private void LoadCommisionRules()
        {
            try
            {
                if (!_dataCached)
                {
                    _commissionCalculationTime = PostTradeDataManager.GetCommissionCalculationTime();
                    PostTradeDataManager.GetAllOtherFeesRulesForAUEC();
                    PostTradeDataManager.GetAllSavedCommissionRules();
                    CommissionRulesCacheManager.GetInstance().SetAllocatedCalculationProperty(_commissionCalculationTime);
                    PostTradeDataManager.GetAllCommissionRulesForCVAUEC();
                    _dataCached = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Unallocate group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public AllocationGroup UnAllocateGroup(AllocationGroup group)
        {
            KeepAttributesValues(group);
            group.UnAllocate();
            List<AllocationGroup> groups = new List<AllocationGroup>();
            groups.Add(group);
            AllocationCheck(groups);
            return group;
        }

        /// <summary>
        /// Added by Narendra Kumar jangir 19 Mar 2013
        /// This method keeps taxlots corresponding to group with the attributes
        /// </summary>
        /// <param name="group"></param>
        private void KeepAttributesValues(AllocationGroup group)
        {
            try
            {
                if (group.TaxLots.Count > 0)
                {
                    //make empty DeletedTaxlotsWithExtraFields while unallocating data and fill with new details
                    group.TaxLotIdsWithAttributes = string.Empty;
                    foreach (TaxLot taxlot in group.TaxLots)
                    {
                        if (!string.IsNullOrEmpty(taxlot.LotId) || !string.IsNullOrEmpty(taxlot.ExternalTransId))
                        {
                            group.TaxLotIdsWithAttributes = group.TaxLotIdsWithAttributes + taxlot.TaxLotID + Seperators.SEPERATOR_5 + taxlot.LotId + Seperators.SEPERATOR_5 + taxlot.ExternalTransId + Seperators.SEPERATOR_6;
                        }
                    }
                    //remove last seperator from the DeletedTaxlotsWithExtraFields
                    if (group.TaxLotIdsWithAttributes.Length > 0)
                    {
                        group.TaxLotIdsWithAttributes = group.TaxLotIdsWithAttributes.Substring(0, group.TaxLotIdsWithAttributes.Length - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the positions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<TaxLot> GetPositions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<TaxLot> taxlotsList = new List<TaxLot>();
            try
            {
                List<AllocationGroup> groups = new List<AllocationGroup>();
                groups = _m_PersistenceManager.GetGroups(toDate.ToString(), fromDate.ToString(), accountIDs);
                AllocationCheck(groups);
                List<AllocationGroup> validGroups = new List<AllocationGroup>();
                foreach (AllocationGroup group in groups)
                {
                    if (group.CumQty > 0)
                    {
                        validGroups.Add(group);
                    }
                }
                taxlotsList = GetTaxlotsFromGroups(validGroups);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return taxlotsList;
        }

        /// <summary>
        /// Adds entry to the Audit List for the Allocated data Traded in some back date
        /// </summary>
        /// <param name="order">Not Null, Order from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        private bool AddAllocatedDataAuditEntry(TaxLot taxlot, TradeAuditActionType.ActionType action, string comment)
        {
            TradeAuditEntry newEntry = new TradeAuditEntry();
            try
            {
                if (taxlot != null && comment != null)
                {
                    newEntry.Action = action;
                    newEntry.AUECLocalDate = DateTime.Now;
                    newEntry.OriginalDate = DateTime.Parse(taxlot.AUECLocalDate.ToString());
                    newEntry.Comment = comment;
                    newEntry.CompanyUserId = taxlot.CompanyUserID;
                    newEntry.Symbol = taxlot.Symbol;
                    newEntry.Level1ID = taxlot.Level1ID;
                    newEntry.GroupID = taxlot.GroupID;
                    newEntry.TaxLotClosingId = taxlot.TaxLotClosingId;
                    newEntry.TaxLotID = taxlot.TaxLotID;
                    newEntry.Level1AllocationID = taxlot.Level1AllocationID;
                    newEntry.OrderSideTagValue = taxlot.OrderSideTagValue;
                    newEntry.OriginalValue = "";
                    newEntry.Source = TradeAuditActionType.ActionSource.Allocation;
                    if (!string.IsNullOrEmpty(newEntry.GroupID))
                        _allocatedTradeAuditCollection.Add(newEntry);
                    else
                        Logger.LoggerWrite("GroupID is null for TaxLotID=" + taxlot.TaxLotID + " GroupID = " + taxlot.GroupID + " Symbol= " + newEntry.Symbol + " taxlot.AUECLocalDate= " + taxlot.AUECLocalDate.ToString() + " taxlot.Level1ID= " + taxlot.Level1ID.ToString() + " taxlot.TaxLotClosingId= " + taxlot.TaxLotClosingId.ToString() + "");
                }
                else
                    throw new NullReferenceException("The Data Table to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Publishes the allocation data.
        /// </summary>
        /// <param name="taxlotsList">The taxlots list.</param>
        private void PublishAllocationData(List<TaxLot> taxlotsList)
        {
            try
            {
                List<TaxLot> TaxlotsList = new List<TaxLot>();
                foreach (TaxLot taxlot in taxlotsList)
                {
                    _secMasterServices.SetSecuritymasterDetails(taxlot);
                    // merging UDA details before publishing
                    _secMasterServices.SetSecurityUDADetails(taxlot);
                    _closingServices.SetTaxlotClosingStatus(taxlot);
                    TaxlotsList.Add(taxlot);
                    if (!String.IsNullOrEmpty(taxlot.Level1ID.ToString()) && taxlot.Level1ID != int.MinValue && taxlot.Level1ID != 0 && taxlot.AUECLocalDate.Date < DateTime.UtcNow.Date)
                    {
                        AddAllocatedDataAuditEntry(taxlot, Prana.BusinessObjects.TradeAuditActionType.ActionType.REALLOCATE, "Group Allocated New Taxlots Created");
                        AuditTrailDataManager.Instance.SaveAuditList(_allocatedTradeAuditCollection);
                        _allocatedTradeAuditCollection.Clear();
                    }
                }
                MessageData e = new MessageData();
                //Applied chunking for better publishing and no memory exceptions.
                List<List<TaxLot>> allocationTaxlots = ChunkingManager.CreateChunks(TaxlotsList, 1000);
                foreach (List<TaxLot> eventData in allocationTaxlots)
                {
                    e.EventData = eventData;
                    e.TopicName = Topics.Topic_Allocation;
                    CentralizePublish(e);
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
        }

        /// <summary>
        ///  creates a allocation group from Order object and saves it in db. //Used in Server Side(DbQueueManager and PranaTaxlotChacheManager)
        /// </summary>
        public AllocationGroup CreateAllocationGroup(Order order, bool isCached)
        {
            try
            {
                return _allocationManager.CreateRealVirtualAllocationGroup(order, isCached, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the preference by identifier.
        /// </summary>
        /// <param name="id">The preference identifier.</param>
        /// <returns>AllocationOperationPreference.</returns>
        public AllocationOperationPreference GetPreferenceById(int id)
        {
            try
            {
                return _allocationManager.GetPreferenceById(id);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Create allocation group virtually. does not impact closing
        /// </summary>
        /// <param name="order"></param>
        /// <param name="isCached"></param>
        /// <returns></returns>
        public AllocationGroup CreateVirtualAllocationGroup(Order order, bool isCached, bool isForceAllocation = false)
        {
            try
            {
                return _allocationManager.CreateRealVirtualAllocationGroup(order, isCached, false, isForceAllocation);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets GroupID for order
        /// </summary>
        /// <param name="parentClOrderID"></param>
        /// <returns></returns>
        public string GetGroupID(string parentClOrderID)
        {
            try
            {
                return _allocationManager.GetGroupID(parentClOrderID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Sets the side multiplier.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        private void SetSideMultiplier(List<TaxLot> taxlotList)
        {
            foreach (TaxLot taxlot in taxlotList)
            {
                taxlot.SideMultiplier = Calculations.GetSideMultilpier(taxlot.OrderSideTagValue);
                taxlot.PositionTag = (PositionTag)(GetPositionTagBySide(taxlot.OrderSideTagValue));
            }
        }

        /// <summary>
        /// Sets the closing mode.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        private void SetClosingMode(List<TaxLot> taxlots)
        {
            foreach (TaxLot taxlot in taxlots)
            {
                taxlot.ClosingMode = ClosingMode.Exercise;
            }
        }

        /// <summary>
        /// Allocations the check.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private void AllocationCheck(List<AllocationGroup> groups)
        {
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                    {
                        List<TaxLot> taxlots = new List<TaxLot>();
                        TaxLot taxlot = CreateUnAllocatedTaxLot(group, group.GroupID);
                        taxlots.Add(taxlot);
                        CopySwapParameters(taxlots, group);
                        group.ResetTaxlotDictionary(taxlots);
                    }
                    else
                    {
                        SetSideMultiplier(group.TaxLots);
                        CopySwapParameters(group.TaxLots, group);
                        group.ResetTaxlotDictionary(group.TaxLots);
                        string schemeName = _allocationManager.GetAllocationSchemeNameByID(group.AllocationSchemeID);
                        //update scheme name only in case of fixed preferences, PRANA-20901
                        if (!string.IsNullOrWhiteSpace(schemeName))
                            group.AllocationSchemeName = schemeName;

                        //[Rahul: 20130320] http://jira.nirvanasolutions.com:8080/browse/MON-44
                        //After discussion with closing team, taxlotclosingID only comes when equity is generated
                        //through option exercise. For Cash management, we are setting its closing mode.
                        if (group.TaxLotClosingId != null && !group.TaxLotClosingId.Equals(string.Empty))
                        {
                            SetClosingMode(group.TaxLots);
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
        }

        /// <summary>
        /// Copies the swap parameters.
        /// </summary>
        /// <param name="taxlots">The taxlots.</param>
        /// <param name="group">The group.</param>
        private void CopySwapParameters(List<TaxLot> taxlots, AllocationGroup group)
        {
            try
            {
                if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && group.IsSwapped.Equals(true))
                {
                    taxlots[0].ISSwap = true;
                    taxlots[0].SwapParameters = group.SwapParameters.Clone();
                }
                else if (group.State == PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED && group.IsSwapped.Equals(true))
                {
                    foreach (TaxLot taxlotVar in taxlots)
                    {
                        taxlotVar.ISSwap = true;
                        taxlotVar.SwapParameters = new SwapParameters();
                        taxlotVar.SwapParameters = group.SwapParameters.Clone();
                        if (group.Quantity != 0)
                            taxlotVar.SwapParameters.NotionalValue = (taxlotVar.TaxLotQty * group.SwapParameters.NotionalValue) / group.Quantity;
                        else
                            taxlotVar.SwapParameters.NotionalValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Allocations the group created.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private void AllocationGroupCreated(List<AllocationGroup> groups)
        {
            List<TaxLot> taxlotsList = new List<TaxLot>();
            try
            {
                List<AllocationGroup> validGroups = new List<AllocationGroup>();
                foreach (AllocationGroup group in groups)
                {
                    if (group.CumQty > 0)
                    {
                        validGroups.Add(group);
                    }
                    else if ((group.PersistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated) && (group.CumQty == 0))
                    {
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.Deleted;
                        validGroups.Add(group);
                    }
                }
                taxlotsList = GetTaxlotsFromGroups(validGroups);
                PublishGroup(validGroups);
                if (taxlotsList.Count > 0)
                {
                    PublishAllocationData(taxlotsList);
                    try
                    {
                        //_CashManagementService.CalculateCashImpact(taxlotsList); 
                        if (taxlotsList[0].TransactionSource.ToString().Equals("TradeImport"))
                        {
                            _activityServices.GenerateCashActivity(taxlotsList, CashTransactionType.TradeImport);
                        }
                        else
                        {
                            _activityServices.GenerateCashActivity(taxlotsList, CashTransactionType.Trading);
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(new Exception("Problem in calculating Cash Details +" + ex.Message + " Stack Trace " + ex.StackTrace), LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                        {
                            throw;
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
        }

        /// <summary>
        /// Gets the taxlots from groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private List<TaxLot> GetTaxlotsFromGroups(List<AllocationGroup> groups)
        {
            List<TaxLot> taxlotsList = new List<TaxLot>();
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    taxlotsList.AddRange(group.GetAllTaxlots());
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
            return taxlotsList;
        }

        /// <summary>
        /// Publishes the group.
        /// </summary>
        /// <param name="groups">The groups.</param>
        private void PublishGroup(List<AllocationGroup> groups)
        {
            try
            {
                if (groups.Count > 0)
                {
                    MessageData e = new MessageData();
                    e.EventData = groups;
                    e.TopicName = Topics.Topic_CreateGroup;
                    CentralizePublish(e);
                    List<UserOptModelInput> listOMIData = new List<UserOptModelInput>();
                    foreach (AllocationGroup grp in groups)
                    {
                        //if (!string.IsNullOrEmpty(grp.ProxySymbol) && !grp.ProxySymbol.Equals(grp.Symbol))
                        //{
                        //Divya : 02042013 : When a new symbol is traded, if its proxy is defined, we have to update the OMI cache, and use proxy pricing
                        //instead of the original symbol. 
                        UserOptModelInput userOMI = new UserOptModelInput();
                        userOMI.Symbol = grp.Symbol;
                        userOMI.ProxySymbol = grp.ProxySymbol;
                        userOMI.UnderlyingSymbol = grp.UnderlyingSymbol;
                        userOMI.SecurityDescription = grp.CompanyName;
                        userOMI.Bloomberg = grp.BloombergSymbol;
                        userOMI.OSIOptionSymbol = grp.OSISymbol;
                        userOMI.IDCOOptionSymbol = grp.IDCOSymbol;
                        userOMI.StrikePrice = grp.StrikePrice;
                        userOMI.LeadCurrencyID = grp.LeadCurrencyID;
                        userOMI.VsCurrencyID = grp.VsCurrencyID;
                        userOMI.ExpirationDate = grp.ExpirationDate;
                        if (grp.PutCall != int.MinValue)
                            userOMI.PutorCall = (OptionType)grp.PutCall;
                        userOMI.AssetID = grp.AssetID;
                        userOMI.AuecID = grp.AUECID;
                        userOMI.PersistenceStatus = grp.PersistenceStatus;
                        if (!string.IsNullOrEmpty(userOMI.ProxySymbol))
                        {
                            userOMI.ProxySymbolUsed = true;
                        }
                        //Divya : 02042013 If proxy is defined from symbol lookup, only then we pblish the proxy data. 
                        listOMIData.Add(userOMI);
                    }

                    if (listOMIData.Count > 0)
                    {
                        MessageData e1 = new MessageData();
                        e1.EventData = listOMIData;
                        e1.TopicName = Topics.Topic_PricingInputsData;
                        CentralizePublish(e1);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the group from taxlot identifier.
        /// </summary>
        /// <param name="taxlotID">The taxlot identifier.</param>
        /// <returns></returns>
        public AllocationGroup GetGroupFromTaxlotID(string taxlotID)
        {
            try
            {
                List<AllocationGroup> groups = _m_PersistenceManager.GetTaxlotParentGroup(taxlotID);
                if (groups.Count > 0)
                {
                    return groups[0];
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        ///   Used in Server Side(PranaTaxlotChacheManager)
        /// </summary>
        /// <param name="order"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public TaxLot CreateUnAllocatedTaxLot(PranaBasicMessage baseMsg, string groupID)
        {
            TaxLot taxLot = null;
            try
            {
                taxLot = _allocationManager.CreateUnAllocatedTaxLot(baseMsg, groupID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return taxLot;
        }

        #endregion Public Methods

        #region Private methods

        /// <summary>
        /// Added by Narendra Kumar jangir 19 Mar 2013
        /// Update attributes of taxlots from unallocated trades
        /// </summary>
        /// <param name="grpAttributes"></param>
        /// <param name="taxlot"></param>
        private void UpdateTaxlotAttributes(string[] grpAttributes, TaxLot taxlot)
        {
            try
            {
                //grpAttributes is string array which contains taxlotid's with attributes
                for (int i = 0; i < grpAttributes.Length; i++)
                {
                    //grpMinAttribute is string array which contains taxlotid as first element and remaining elements are attributes
                    string[] grpMinAttribute = grpAttributes[i].Split(Seperators.SEPERATOR_5);
                    if (grpMinAttribute[0].Length > 0 && (grpMinAttribute[0].Equals(taxlot.TaxLotID)))
                    {
                        //second element of grpMinAttribute contains LotId
                        if (grpMinAttribute[1].Length > 0)
                        {
                            taxlot.LotId = grpMinAttribute[1];
                        }
                        //third element of grpMinAttribute contains ExternalTransId
                        if (grpMinAttribute[2].Length > 0)
                        {
                            taxlot.ExternalTransId = grpMinAttribute[2];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates the groups XML.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private string CreateGroupsXml(AllocationGroupCollection groups)
        {
            string groupXml = string.Empty;
            try
            {
                CustomXmlSerializer _Xml = new CustomXmlSerializer();
                groupXml = "<Groups>";
                foreach (AllocationGroup group in groups)
                {
                    groupXml += _Xml.WriteString(group);
                }
                groupXml += "</Groups>";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return groupXml;
        }

        /// <summary>
        /// Gets the position tag by side.
        /// </summary>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <returns></returns>
        private int GetPositionTagBySide(string orderSideTagValue)
        {
            switch (orderSideTagValue)
            {
                case FIXConstants.SIDE_Buy:
                case FIXConstants.SIDE_BuyMinus:
                case FIXConstants.SIDE_Buy_Open:
                case FIXConstants.SIDE_Buy_Closed:
                case FIXConstants.SIDE_Buy_Cover:
                default:
                    return (int)PositionTag.Long;
                //unreachable code
                //  break;

                case FIXConstants.SIDE_Sell:
                case FIXConstants.SIDE_SellShort:
                case FIXConstants.SIDE_Sell_Open:
                case FIXConstants.SIDE_Sell_Closed:
                    return (int)PositionTag.Short;
                //unreachable code
                // break;
            }
        }

        #endregion

        /// <summary>
        /// Calculates the commission order wise.
        /// </summary>
        /// <param name="order">The order.</param>
        public void CalculateCommissionOrderWise(ref Order order)
        {
            CommissionCalculator.GetInstance.CalculateCommissionOrderWise(ref order);
        }

        /// <summary>
        /// Checks if unsaved groups.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfUnsavedGroups()
        {
            // Moved the GroupCache to CacheStore in Allocation.Core, so called this function through AllocationManager
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7506
            return _allocationManager.CheckIfUnsavedGroups();
        }

        /// <summary>
        /// Saves the un saved groups.
        /// </summary>
        public void SaveUnSavedGroups( List<AllocationGroup> groupListNew,  List<AllocationGroup> groupList)
        {
            _allocationManager.SaveUnSavedGroups( groupListNew,  groupList);
        }

        /// <summary>
        /// Saves the un saved groups.
        /// </summary>
        public void GetUnSavedGroupsData(ref List<AllocationGroup> groupListNew, ref List<AllocationGroup> groupList)
        {
            _allocationManager.GetUnSavedGroupsData(ref groupListNew, ref groupList);
        }

        /// <summary>
        /// The sec master services
        /// </summary>
        private ISecMasterServices _secMasterServices;

        /// <summary>
        /// Sets the sec master services.
        /// </summary>
        /// <value>
        /// The sec master services.
        /// </value>
        public ISecMasterServices SecMasterServices
        {
            set
            {
                _secMasterServices = value;
                if (_m_PersistenceManager != null)
                    _m_PersistenceManager.SecMasterServices = value;
            }
        }

        /// <summary>
        /// Unwind closing.
        /// </summary>
        /// <param name="TaxlotClosingID">The taxlot closing identifier.</param>
        /// <param name="taxlotIDList">The taxlot identifier list.</param>
        /// <param name="TaxlotClosingIDWithClosingDate">The taxlot closing identifier with closing date.</param>
        /// <returns></returns>
        public ClosingData UnWindClosing(string TaxlotClosingID, string taxlotIDList, string TaxlotClosingIDWithClosingDate)
        {
            ClosingData closedData = _allocationManager.UnWindClosing(TaxlotClosingID, taxlotIDList, TaxlotClosingIDWithClosingDate);
            return closedData;
        }

        /// <summary>
        /// Get groups from database for all the requsted taxlotids.
        /// This method is used in new cancel-amd+recon CH UI
        /// </summary>
        /// <param name="lstTaxLotIds"></param>
        /// <returns></returns>
        public List<AllocationGroup> GetGroupsForTaxLotIDs(List<string> lstTaxLotIds)
        {
            List<AllocationGroup> groups = new List<AllocationGroup>();
            try
            {
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.REQUEST_RECEIVED, AllocationLoggingConstants.GET_GROUPS_FOR_TAXLOT_ID);
                String query = Prana.PostTrade.BLL.QueryGenerator.GenerateQueryForTaxlotIDs(lstTaxLotIds);
                groups = _m_PersistenceManager.GetGroupsOnQuery(query);
                AllocationCheck(groups);
                AllocationLoggingHelper.LoggerWriteMessage(AllocationLoggingConstants.COMPONENT_ALLOCATION + AllocationLoggingConstants.RESPONSE_SEND, AllocationLoggingConstants.GET_GROUPS_FOR_TAXLOT_ID);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return groups;
        }

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
                _proxyPublishing.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// The _publish lock
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
        /// Saves the groups.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public int SaveGroups(List<AllocationGroup> groups, int userID)
        {
            int rowsAffected = 0;
            try
            {
                List<AllocationGroup> updatedGroups = _allocationManager.SaveGroups(groups, -1);
                if (updatedGroups != null)
                    rowsAffected = updatedGroups.Count;
                //rowsAffected = _allocationManager.SaveGroupsForFills(groups, string.Empty, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Creates the allcation group from taxlot base.
        /// </summary>
        /// <param name="taxlotBase">The taxlot base.</param>
        /// <returns></returns>
        public AllocationGroup CreateAllcationGroupFromTaxlotBase(TaxlotBase taxlotBase)
        {
            AllocationGroup group = new AllocationGroup();
            try
            {
                group = _allocationManager.CreateAllcationGroupFromTaxlotBase(taxlotBase);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return group;
        }

        /// <summary>
        /// Deletes the groups from ca.
        /// </summary>
        /// <param name="dTable">The d table.</param>
        /// <returns></returns>
        public int DeleteGroupsFromCA(DataTable dTable)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = _allocationManager.DeleteGroupsFromCA(dTable);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return rowsAffected;
        }

        /// <summary>
        /// Virtual allocation of In Market Quantities - This thing is doing just after real allocation of fills
        /// </summary>
        /// <param name="virtualAllocationGroup"></param>
        /// <returns></returns>
        public AllocationGroup DoVirtualAllocation(AllocationGroup virtualAllocationGroup)
        {
            try
            {
                return _allocationManager.DoVirtualAllocation(virtualAllocationGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the allocation preference name by identifier.
        /// </summary>
        /// <param name="preferenceId">The preference identifier.</param>
        /// <returns></returns>
        public string GetAllocationPreferenceNameById(int preferenceId)
        {
            string preferenceName = string.Empty;
            try
            {
                preferenceName = _allocationManager.GetAllocationPreferenceNameById(preferenceId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return preferenceName;
        }

        /// <summary>
        /// Adds the given preference information to database and cache
        /// </summary>
        /// <param name="name">Name of the preference</param>
        /// <param name="companyId">companyId of the preference</param>
        /// <param name="allocationPrefType"></param>
        /// <param name="isPrefVisible">if set to <c>true</c> [is preference visible].</param>
        /// <returns>Update result of the preference</returns>
        public PreferenceUpdateResult AddPreference(string name, int companyId, AllocationPreferencesType allocationPrefType, bool isPrefVisible)
        {
            PreferenceUpdateResult result = new PreferenceUpdateResult(); ;
            try
            {
                result = _allocationManager.AddPreference(name, companyId, allocationPrefType,isPrefVisible);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// This method update AllocationOperationPreference for given company
        /// </summary>
        /// <param name="preference">Preference which to be updated</param>
        /// <returns>True if updated successfully, otherwise false</returns>
        public PreferenceUpdateResult UpdatePreference(AllocationOperationPreference aop)
        {
            PreferenceUpdateResult result = new PreferenceUpdateResult(); ;
            try
            {
                result = _allocationManager.UpdatePreference(aop);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }

        public decimal GetCurrentStateForSymbolInAccount(string symbol, int userID, int accountId)
        {
            try
            {
                return _allocationManager.GetCurrentStateForSymbolInAccount(symbol, userID, accountId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
        }
        #endregion
    }
}