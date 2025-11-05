using Prana.Allocation.Common.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.PostTrade;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Allocation.Core.CacheStore
{
    public class GroupCache
    {
        /// <summary>
        /// _dictPreAllocatedGroups maps parentClOrderID to GroupID since we are creating new Group for each incoming fill so this cache is used to fetch groupID corresponding to parentClOrderID 
        ///  of the incoming order which can then be set in the new group object...
        /// </summary>
        private static Dictionary<string, List<string>> _dictPreAllocatedGroups = new Dictionary<string, List<string>>();

        /// <summary>
        /// The cache to store groupId for ClOrderId
        /// </summary>
        private static Dictionary<string, string> _clOrderIdGroupIdCache = new Dictionary<string, string>();

        /// <summary>
        /// dictionary of dirty allocation groups
        /// </summary>
        Dictionary<string, AllocationGroup> _dictDirtyGroups = new Dictionary<string, AllocationGroup>();

        /// <summary>
        /// The automatic group cache
        /// </summary>
        readonly List<AllocationGroup> _autoGroupingCache = new List<AllocationGroup>();

        public List<AllocationGroup> AutoGroupingCache
        {
            get
            {
                lock (_autoGroupingCache)
                {
                    return _autoGroupingCache;
                }
            }
        }

        /// <summary>
        /// The locker object
        /// </summary>
        static readonly object locker = new object();
        static GroupCache instance = null;

        /// <summary>
        /// This _dictGroups caches groups other than those groups which have persistencestatus = New.
        /// This separate cache of new groups is created so that those groups could be saved first before their further updates.
        /// </summary>
        private static Dictionary<string, AllocationGroup> _dictGroups = new Dictionary<string, AllocationGroup>();

        /// <summary>
        /// This _dictGroupsNew saves only the groups which have persistencestatus = New
        /// </summary>
        private static Dictionary<string, AllocationGroup> _dictGroupsNew = new Dictionary<string, AllocationGroup>();

        static Dictionary<string, List<StateObject>> _groupTaxlotCache;
        static Dictionary<string, string> _pendingReplacedOrders = new Dictionary<string, string>();

        static GroupCache()
        {
            lock (locker)
            {
                _groupTaxlotCache = new Dictionary<string, List<StateObject>>();
                instance = new GroupCache();
                _dictPreAllocatedGroups = PostTradeDataManager.GetAllGroupIDsAndParentClOrderID();
                _pendingReplacedOrders = PostTradeDataManager.GetPendingReplacedOrderClOrderID();
            }
        }

        private GroupCache()
        {

        }

        public static GroupCache GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// add group in cache and respective allocation type
        /// </summary>
        /// <param name="allocationGroup"></param>
        public void AddGroup(AllocationGroup allocationGroup)
        {
            try
            {
                lock (locker)
                {
                    //bool shouldAddGroup = true;
                    if (allocationGroup.PersistenceStatus == ApplicationConstants.PersistenceStatus.Deleted || allocationGroup.PersistenceStatus == ApplicationConstants.PersistenceStatus.UnGrouped)
                    {
                        if (_dictGroupsNew.ContainsKey(allocationGroup.GroupID))
                        {
                            _dictGroupsNew.Remove(allocationGroup.GroupID);
                        }
                        _dictGroups[allocationGroup.GroupID] = allocationGroup;
                    }
                    /// If New group then add into _dictGroupsNew else add and update into _dictGroups
                    else if (allocationGroup.PersistenceStatus == ApplicationConstants.PersistenceStatus.New)
                    {
                        if (!_dictGroupsNew.ContainsKey(allocationGroup.GroupID))
                        {
                            _dictGroupsNew.Add(allocationGroup.GroupID, allocationGroup);
                        }
                    }
                    else
                    {
                        if (!_dictGroups.ContainsKey(allocationGroup.GroupID))
                        {
                            _dictGroups.Add(allocationGroup.GroupID, allocationGroup);
                        }
                        else
                        {
                            _dictGroups[allocationGroup.GroupID] = allocationGroup;
                        }
                    }
                    checkGroupTaxlotStatus(allocationGroup, false);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the automatic groups cache.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void UpdateAutoGroupsCache(AllocationGroup group)
        {
            try
            {
                lock (_autoGroupingCache)
                {
                    if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.Deleted || group.PersistenceStatus == ApplicationConstants.PersistenceStatus.UnGrouped)
                    {
                        DeleteFromAutoGroupingCache(group);
                    }
                    else
                    {
                        AllocationGroup existingGroup = _autoGroupingCache.Find(g => g.GroupID == group.GroupID);
                        foreach (AllocationOrder order in group.Orders)
                        {
                            order.OriginalCumQty = order.CumQty;
                        }
                        _autoGroupingCache.Remove(existingGroup);
                        // Used deep copy to maintain state of taxlot qty in group.Taxlots after closing
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7147
                        _autoGroupingCache.Add(DeepCopyHelper.Clone(group));
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
        }

        /// <summary>
        /// Deletes from automatic grouping cache.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void DeleteFromAutoGroupingCache(AllocationGroup group)
        {
            try
            {
                AllocationGroup existingGroup = _autoGroupingCache.Find(g => g.GroupID == group.GroupID);
                _autoGroupingCache.Remove(existingGroup);
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
        /// Determines whether [is GRP exist in automatic grouping cache] [the specified GRP identifier].
        /// </summary>
        /// <param name="grpId">The GRP identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is GRP exist in automatic grouping cache] [the specified GRP identifier]; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsGrpExistInAutoGroupingCache(string grpId)
        {
            bool isExist = false;
            try
            {
                _autoGroupingCache.ForEach(grp =>
                {
                    if (grp.GroupID == grpId || grp.Orders.Any(grpOrd => grpOrd.GroupID == grpId))
                    {
                        isExist = true;
                        return;
                    }
                });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isExist;
        }

        /// <summary>
        /// GroupList for new groups and other than new groups are returned separately so that new groups can be saved first
        /// before the other groups.
        /// </summary>
        /// <param name="groupListNew"></param>
        /// <param name="groupList"></param>
        public void GetAllocationGroups(ref List<AllocationGroup> groupListNew, ref List<AllocationGroup> groupList)
        {
            lock (locker)
            {
                try
                {
                    if (_dictGroupsNew.Count > 0)
                    {
                        foreach (KeyValuePair<string, AllocationGroup> groupKeyValue in _dictGroupsNew)
                        {
                            //_dictGroupsNew cache contains only group come first time, Acknowledge of the trade, so taxlot state should be new
                            checkGroupTaxlotStatus(groupKeyValue.Value, false);
                            groupListNew.Add(groupKeyValue.Value);
                        }
                    }
                    if (_dictGroups.Count > 0)
                    {
                        foreach (KeyValuePair<string, AllocationGroup> groupKeyValue in _dictGroups)
                        {
                            checkGroupTaxlotStatus(groupKeyValue.Value, true);
                            groupList.Add(groupKeyValue.Value);
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
                finally
                {
                    //  PRANA-1330 - Rajat
                    /// This clearing of cache was at the calling code in Posttradecachemanager from SaveUnSavedGroups 
                    /// which was invoked on timer. Moved it here. 
                    _dictGroupsNew.Clear();
                    _dictGroups.Clear();
                }
            }
        }

        /// <summary>
        /// gets status of taxlots of the group
        /// </summary>
        /// <param name="allocationGroup">the allocation group</param>
        /// <param name="issending">checks if taxlot is sent or not</param>
        private void checkGroupTaxlotStatus(AllocationGroup allocationGroup, bool issending)
        {
            try
            {
                if (!_groupTaxlotCache.ContainsKey(allocationGroup.GroupID))
                {
                    if (allocationGroup.TaxLots.Count > 0)
                    {
                        List<StateObject> taxlotdata = new List<StateObject>();
                        foreach (TaxLot taxlot in allocationGroup.TaxLots)
                        {
                            StateObject taxlotstate = new StateObject();
                            taxlotstate.Id = taxlot.TaxLotID;
                            taxlotstate.State = Prana.Global.ApplicationConstants.TaxLotState.New;
                            taxlotstate.IsSent = issending;
                            taxlotdata.Add(taxlotstate);
                        }
                        _groupTaxlotCache.Add(allocationGroup.GroupID, taxlotdata);
                    }
                }
                else
                {
                    List<StateObject> taxlotdata = _groupTaxlotCache[allocationGroup.GroupID];
                    foreach (TaxLot taxlot in allocationGroup.TaxLots)
                    {
                        foreach (StateObject taxlotstate in taxlotdata)
                        {
                            if (taxlotstate.Id == taxlot.TaxLotID)
                            {
                                if (taxlotstate.IsSent)
                                    taxlot.TaxLotState = Prana.Global.ApplicationConstants.TaxLotState.Updated;
                                else
                                {
                                    if (taxlot.TaxLotState != ApplicationConstants.TaxLotState.Updated)
                                    {
                                        taxlot.TaxLotState = Prana.Global.ApplicationConstants.TaxLotState.New;
                                    }
                                    taxlotstate.IsSent = issending;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// checks if groups are unsaved
        /// </summary>
        /// <returns>true if groups are unsaved, false otherwise</returns>
        public bool CheckIfUnsavedGroups()
        {
            lock (locker)
            {
                if (_dictGroups.Count > 0 && _dictGroupsNew.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// clears the cache
        /// </summary>
        public void ClearCache()
        {
            try
            {
                lock (locker)
                {
                    _dictGroupsNew.Clear();
                    _dictGroups.Clear();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Gets GroupID for order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetGroupID(string parentClOrderID)
        {
            try
            {
                if (_clOrderIdGroupIdCache.ContainsKey(parentClOrderID))
                    return _clOrderIdGroupIdCache[parentClOrderID];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// create group id
        /// </summary>
        /// <param name="order">the order</param>
        /// <param name="group"> the allocation group</param>
        public void CreateGroupID(Order order, AllocationGroup group, bool isReal)
        {
            try
            {
                lock (locker)
                {
                    List<string> list = new List<string>();
                    string groupID = string.Empty;
                    double cumQty = 0.0;
                    #region
                    if (order.MsgType == FIXConstants.MSGOrder)
                    {
                        if (!_clOrderIdGroupIdCache.ContainsKey(order.ParentClOrderID))
                        {
                            groupID = AllocationIDGenerator.GenerateGroupID();
                            _clOrderIdGroupIdCache.Add(order.ParentClOrderID, groupID);
                        }
                        else
                        {
                            groupID = _clOrderIdGroupIdCache[order.ParentClOrderID];
                        }
                    }
                    else
                    {
                        string clorderId = order.ParentClOrderID;
                        #region
                        // gtc\gtd order handling.
                        string origClorderId = "";
                        if (order.MsgType == FIXConstants.MSGTransferUser)
                        {
                            string orderID = clorderId + "_" + order.AUECLocalDate.ToString("yyyy/MM/dd").Replace("/", "");
                            if (OrderCacheManager.StagedSubsCollection.ContainsKey(order.OrigClOrderID) && OrderCacheManager.StagedSubsCollection[order.OrigClOrderID].dictOrderIDWiseNewCLOrderIDs.ContainsKey(orderID))
                            {
                                clorderId = order.OrigClOrderID;
                                origClorderId = OrderCacheManager.StagedSubsCollection[order.OrigClOrderID].dictOrderIDWiseNewCLOrderIDs[orderID].clOrderID;
                            }
                            else
                            {
                                origClorderId = PostTradeDataManager.GetClOrderIDFromParentClOrderID(clorderId);
                                if (OrderCacheManager.StagedSubsCollection.ContainsKey(origClorderId) && OrderCacheManager.StagedSubsCollection[origClorderId].dictOrderIDWiseNewCLOrderIDs.ContainsKey(orderID))
                                {
                                    clorderId = origClorderId;
                                    origClorderId = OrderCacheManager.StagedSubsCollection[origClorderId].dictOrderIDWiseNewCLOrderIDs[orderID].clOrderID;
                                }
                                else
                                {
                                    origClorderId = clorderId;
                                }
                            }
                        }
                        if (order.MsgType == FIXConstants.MSGTransferUser && (order.TIF == FIXConstants.TIF_GTD || order.TIF == FIXConstants.TIF_GTC || OrderCacheManager.StagedSubsCollection.ContainsKey(clorderId)))
                        {
                            clorderId = origClorderId;
                        }
                        #endregion
                        if (_clOrderIdGroupIdCache.ContainsKey(clorderId))
                        {
                            groupID = _clOrderIdGroupIdCache[clorderId];

                        }
                        else
                        {
                            groupID = PostTradeDataManager.GetGroupIDFromParentClOrderID(clorderId);
                            if (groupID != string.Empty)
                            {
                                _clOrderIdGroupIdCache.Add(clorderId, groupID);
                            }
                            else if (String.IsNullOrEmpty(groupID) && !isReal)
                            {
                                groupID = AllocationIDGenerator.GenerateGroupID();
                                _clOrderIdGroupIdCache.Add(clorderId, groupID);
                            }
                        }
                        group.PersistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                    }
                    if (isReal)
                    {
                        string origGroupID = groupID;
                        if (_dictPreAllocatedGroups.ContainsKey(order.ParentClOrderID))
                        {
                            groupID = _dictPreAllocatedGroups[order.ParentClOrderID][0];
                            _dictPreAllocatedGroups[order.ParentClOrderID][1] = order.CumQty.ToString();
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(groupID) && order.CumQty != 0.0)
                            {
                                cumQty = order.CumQty;
                                list.Add(groupID);
                                list.Add(cumQty.ToString());
                                _dictPreAllocatedGroups.Add(order.ParentClOrderID, list);
                            }
                        }
                        if ((order.TIF.Equals(FIXConstants.TIF_GTD) || order.TIF.Equals(FIXConstants.TIF_GTC))
                            && (order.MsgType.Equals(FIXConstants.MSGExecutionReport) || order.MsgType.Equals(FIXConstants.MSGOrderCancelReject)))
                        {
                            groupID = origGroupID;
                        }
                    }
                    #endregion
                    group.GroupID = groupID;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sandeep Singh,July 2, 2013: this is used to create a group from an order and checks is group dirty
        /// Dirty group: suppose a partially filled unAllocated trade comes through drop copy, in the mean while we allocate it from Allocation UI,
        /// this group is marked as Dirty and kept it in group cache. When next fill comes, we check it in the Dirty group cache, if exists, we update it as per the new fill.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="isDirty"></param>
        /// <returns></returns>
        public AllocationGroup CreateGroup(Order order, ref bool isDirty, bool isReal)
        {
            AllocationGroup group = new AllocationGroup();
            CreateGroupID(order, group, isReal);

            ///TODO: PLease explain the concept of dirty groups here?
            ///WHy is this used? Ashish Poddar 20121002
            ///Adding ClOrderId check here, because after Replace no need to get from Cache
            if (_dictDirtyGroups.ContainsKey(group.GroupID) && (isReal || order.MsgType == FIXConstants.MSGTransferUser) && _dictDirtyGroups[group.GroupID].Orders.Any(x => x.ClOrderID == order.ClOrderID))//Adding is real check and in case of compliance need not to get from cache.
            {
                isDirty = true;
                group = _dictDirtyGroups[group.GroupID];
                group.CommissionAmt = order.CommissionAmt;
                group.SoftCommissionAmt = order.SoftCommissionAmt;
            }
            //checking for replace in case of grouped trades.
            else if (_dictDirtyGroups.ContainsKey(group.GroupID) && (isReal || order.MsgType == FIXConstants.MSGTransferUser) && _dictDirtyGroups[group.GroupID].Orders.Any(x => x.ParentClOrderID.Equals(order.ParentClOrderID)))
            {
                isDirty = true;
                group = _dictDirtyGroups[group.GroupID];
                group.CommissionAmt = order.CommissionAmt;
                group.SoftCommissionAmt = order.SoftCommissionAmt;
            }
            else
            {
                #region copy basic properties

                group.CopyBasicDetails(order);

                group.IsOverbuyOversellAccepted = order.IsOverbuyOversellAccepted;
                if (order.SwapParameters != null)
                {
                    group.SwapParameters = order.SwapParameters.Clone();
                    group.SwapParameters.GroupID = group.GroupID;
                    group.IsSwapped = true;
                }
                if (order.OTCParameters != null)
                {
                    group.OTCParameters = order.OTCParameters;
                    group.OTCParameters.GroupID = group.GroupID;

                }
                if (order.ShortLocateParameter != null)
                {
                    group.BorrowerID = order.BorrowerID;
                    group.BorrowerBroker = order.BorrowerBroker;
                    group.ShortRebate = order.ShortRebate;
                }
                group.Description = order.Text;
                group.InternalComments = order.InternalComments;
                group.FXRate = order.FXRate;
                if (order.CalcBasis != Prana.BusinessObjects.AppConstants.CalculationBasis.Auto)
                {
                    group.CommSource = CommisionSource.Manual;
                }
                if (order.SoftCommissionCalcBasis != Prana.BusinessObjects.AppConstants.CalculationBasis.Auto)
                {
                    group.SoftCommSource = CommisionSource.Manual;
                }

                if (string.IsNullOrEmpty(group.FXConversionMethodOperator))
                    group.FXConversionMethodOperator = Prana.BusinessObjects.AppConstants.Operator.M.ToString();

                if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDManual || (OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDManualSub)
                {
                    group.IsManualGroup = true;
                    SetTransactionSource(group, order, TransactionSource.TradingTicket);
                }
                else
                {
                    if (!order.IsManualOrder)
                        SetTransactionSource(group, order, TransactionSource.FIX);
                }
                group.TransactionType = CachedDataManager.GetInstance.GetTransactionTypeAcronymByOrderSideTagValue(group.OrderSideTagValue);
                if (order.ModifiedUserId != int.MinValue)
                    group.ModifiedUserId = order.ModifiedUserId;
                #endregion

                #region add orders

                AllocationOrder allocOrder = new AllocationOrder();
                allocOrder.ClOrderID = order.ClOrderID;
                //http://jira.nirvanasolutions.com:8080/browse/CI-744 and http://jira.nirvanasolutions.com:8080/browse/PRANA-5787
                allocOrder.ParentClOrderID = order.ParentClOrderID;
                allocOrder.GroupID = group.GroupID;
                allocOrder.CumQty = group.CumQty;
                allocOrder.AvgPrice = group.AvgPrice;
                allocOrder.FXRate = group.FXRate;
                allocOrder.Quantity = group.Quantity;

                // Corrected http://jira.nirvanasolutions.com:8080/browse/PRANA-2106
                //More fields added as they are needed while groupin data
                //allocOrder.Quantity = group.Quantity;
                allocOrder.OriginalPurchaseDate = group.OriginalPurchaseDate;
                allocOrder.AUECLocalDate = group.AUECLocalDate;
                allocOrder.ProcessDate = group.ProcessDate;
                allocOrder.VenueID = group.VenueID;
                allocOrder.CounterPartyID = group.CounterPartyID;
                allocOrder.TradingAccountID = group.TradingAccountID;
                allocOrder.OrderSideTagValue = group.OrderSideTagValue;
                allocOrder.SettlementDate = group.SettlementDate;
                allocOrder.ImportFileLogObj = new ImportFileLog();
                allocOrder.ImportFileLogObj.ImportFileID = order.ImportFileID;
                allocOrder.ImportFileLogObj.ImportFileName = order.ImportFileName;
                allocOrder.MultiTradeName = order.MultiTradeName;
                allocOrder.CompanyUserID = order.CompanyUserID;

                allocOrder.TradeAttribute1 = order.TradeAttribute1;
                allocOrder.TradeAttribute2 = order.TradeAttribute2;
                allocOrder.TradeAttribute3 = order.TradeAttribute3;
                allocOrder.TradeAttribute4 = order.TradeAttribute4;
                allocOrder.TradeAttribute5 = order.TradeAttribute5;
                allocOrder.TradeAttribute6 = order.TradeAttribute6;
                allocOrder.SetTradeAttribute(order.GetTradeAttributesAsDict());

                allocOrder.CurrencyID = order.CurrencyID;
                allocOrder.SettlementCurrencyID = order.SettlementCurrencyID;
                allocOrder.InternalComments = order.InternalComments;
                allocOrder.Description = order.Text;
                allocOrder.ChangeType = order.ChangeType;
                allocOrder.FXConversionMethodOperator = order.FXConversionMethodOperator;
                allocOrder.OriginalAllocationPreferenceID = order.OriginalAllocationPreferenceID;
                allocOrder.BorrowerID = order.BorrowerID;
                allocOrder.BorrowerBroker = order.BorrowerBroker;
                allocOrder.ShortRebate = order.ShortRebate;
                if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDManual || (OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDManualSub)
                {
                    SetTransactionSource(allocOrder, order, TransactionSource.TradingTicket);
                }
                else
                {
                    if (!order.IsManualOrder)
                        SetTransactionSource(allocOrder, order, TransactionSource.FIX);
                }
                group.AddOrder(allocOrder);
                #endregion
            }
            return group;
        }

        /// <summary>
        /// Sets Transaction Source of a group 
        /// </summary>
        /// <param name="pranaBasicmsg"></param>
        /// <param name="transactionSource"></param>
        private void SetTransactionSource(PranaBasicMessage pranaBasicmsg, Order order, TransactionSource transactionSource)
        {
            //Removed the check for pranaBasicmsg.OriginalAllocationPreferenceID == 0 as it is not required, We have handled all the transaction source in generic way refer to  PRANA-41721
            //Check for Imported stagedorders with dynamic AllocationScheme
            //https://jira.nirvanasolutions.com:8443/browse/PRANA-25709 
            if (order.TransactionSourceTag == (int)TransactionSource.TradeImport)
            {
                pranaBasicmsg.TransactionSource = TransactionSource.TradeImport;
                pranaBasicmsg.TransactionSourceTag = (int)TransactionSource.TradeImport;
            }
            else if (order.TransactionSourceTag == (int)TransactionSource.PST)
            {
                pranaBasicmsg.TransactionSource = TransactionSource.PST;
                pranaBasicmsg.TransactionSourceTag = (int)TransactionSource.PST;
            }
            else if (order.TransactionSourceTag == (int)TransactionSource.Rebalancer)
            {
                pranaBasicmsg.TransactionSource = TransactionSource.Rebalancer;
                pranaBasicmsg.TransactionSourceTag = (int)TransactionSource.Rebalancer;
            }
            else if (order.TransactionSourceTag == (int)TransactionSource.TradingTicket)
            {
                pranaBasicmsg.TransactionSource = TransactionSource.TradingTicket;
                pranaBasicmsg.TransactionSourceTag = (int)TransactionSource.TradingTicket;
            }
            else if (order.TransactionSourceTag == (int)TransactionSource.HotButton)
            {
                pranaBasicmsg.TransactionSource = TransactionSource.HotButton;
                pranaBasicmsg.TransactionSourceTag = (int)TransactionSource.HotButton;
            }
            else if (order.TransactionSourceTag == -1)
            {
                pranaBasicmsg.TransactionSource = TransactionSource.None;
                pranaBasicmsg.TransactionSourceTag = (int)TransactionSource.None;
            }
            else
            {
                pranaBasicmsg.TransactionSource = transactionSource;
                pranaBasicmsg.TransactionSourceTag = (int)transactionSource;
            }

        }
        /// <summary>
        /// mark group as dirty
        /// </summary>
        /// <param name="group">the allocation group</param>
        internal void MarkGroupDirty(AllocationGroup group)
        {
            try
            {
                string groupID = string.Empty;
                double cumQty = 0.0;
                if (group.PersistenceStatus == ApplicationConstants.PersistenceStatus.UnGrouped)
                {
                    return;
                }
                // add the new orders in the preallocated cache
                foreach (AllocationOrder order in group.Orders)
                {
                    groupID = group.GroupID;
                    cumQty = order.CumQty;
                    List<string> list = new List<string>();
                    list.Add(groupID);
                    list.Add(cumQty.ToString());
                    if (!_dictPreAllocatedGroups.ContainsKey(order.ParentClOrderID))
                    {
                        _dictPreAllocatedGroups.Add(order.ParentClOrderID, list);
                    }
                    else
                    {
                        _dictPreAllocatedGroups[order.ParentClOrderID][0] = group.GroupID;
                        _dictPreAllocatedGroups[order.ParentClOrderID][1] = order.CumQty.ToString();
                    }
                }
                if (!_dictDirtyGroups.ContainsKey(group.GroupID))
                {
                    _dictDirtyGroups.Add(group.GroupID, group);
                }
                else
                {
                    _dictDirtyGroups[group.GroupID] = group;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// mark list of groups as dirty
        /// </summary>
        /// <param name="groups">the list of allocation groups</param>
        internal void MarkGroupDirty(List<AllocationGroup> groups)
        {
            try
            {
                foreach (AllocationGroup group in groups)
                {
                    AllocationGroup clonedGroup = (AllocationGroup)group.Clone();
                    MarkGroupDirty(clonedGroup);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal bool CheckGroupsForQuantityChange(List<AllocationGroup> groups)
        {
            bool result = true;
            try
            {
                Dictionary<string, double> groupWiseCumQty = new Dictionary<string, double>();
                foreach (AllocationGroup group in groups)
                {
                    foreach (AllocationOrder order in group.Orders)
                    {
                        if (!groupWiseCumQty.ContainsKey(order.ParentClOrderID))
                        {
                            groupWiseCumQty.Add(order.ParentClOrderID, order.OriginalCumQty);
                        }
                    }
                }
                lock (locker)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in _dictPreAllocatedGroups)
                    {
                        if (groupWiseCumQty.ContainsKey(kvp.Key))
                        {
                            if (groupWiseCumQty[kvp.Key] < Convert.ToDouble(kvp.Value[1]))
                            {
                                result = false;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return result;
            }
        }

        internal bool IsOrderInPendingReplaceState(string clOrdId)
        {
            try
            {
                if (_pendingReplacedOrders.ContainsKey(clOrdId))
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        internal void addInPendingReplaceDict(string parenctClOrdId, string clOrdId)
        {
            try
            {
                if (!_pendingReplacedOrders.ContainsKey(parenctClOrdId))
                    _pendingReplacedOrders.Add(parenctClOrdId, clOrdId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void removeFromPendingReplaceDict(string parentClOrdId)
        {
            try
            {
                if (_pendingReplacedOrders.ContainsKey(parentClOrdId))
                    _pendingReplacedOrders.Remove(parentClOrdId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}
