using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.BlotterDataService;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessLogic;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.TradeManager.Extension.CacheStore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Prana.Fix.FixDictionary;
using Prana.Interfaces;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.WCFConnectionMgr;
using Prana.BusinessObjects.Classes.Blotter;

namespace Prana.TradeManager.Extension
{
    public delegate void UpdateBlotterCollectionOnBlotterThreadHandler(object sender, EventArgs<List<OrderSingle>> e);
    public class BlotterOrderCollections
    {
        private static BlotterOrderCollections _blotterOrderCollections = null;
        BlotterPreferenceData _blotterPreferenceData = null;
        private bool _isOnlyAUECTimeBasedBlotterClearance = Convert.ToBoolean(ConfigurationManager.AppSettings["IsOnlyAUECTimeBasedBlotterClearance"]);
        private OrderBindingList newOrderList = new OrderBindingList();
        private OrderBindingList newWorkingSubsList = new OrderBindingList();
        public event EventHandler<EventArgs<ShortLocateListParameter>> UpdateShortLocateData;
        public event EventHandler<EventArgs<OrderSingle>> ShowCustomMessageBoxEventHandler;
        public event EventHandler<EventArgs<OrderSingle>> RequiresComplianceApprovalEventHandler;


        /// <summary>
        /// proxy for the pending apporval UI to get data from server
        /// </summary>
        private ProxyBase<IPreTradeService> _preTradeService = null;
        private BlotterOrderCollections()
        {
            BlotterPreferenceManager.GetInstance().SetUser(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser);
            _blotterPreferenceData = (BlotterPreferenceData)BlotterPreferenceManager.GetInstance().GetPreferencesBinary();
            _preTradeService = new ProxyBase<IPreTradeService>("TradePreTradeComplianceServiceEndpointAddress");
            InitializePendingApprovalCache();
        }

        public static BlotterOrderCollections GetInstance()
        {
            if (_blotterOrderCollections == null)
            {
                _blotterOrderCollections = new BlotterOrderCollections();
            }
            return _blotterOrderCollections;
        }

        private void InitializePendingApprovalCache()
        {
            List<PreTradeApprovalInfo> pendingApprovalData = _preTradeService.InnerChannel.GetPendingApprovalData();
            foreach (var preTradeApprovalInfo in pendingApprovalData)
            {
                foreach (var pranaMessage in preTradeApprovalInfo.PendingOrderCache)
                {
                    OrderSingle order = Transformer.CreateOrderSingle(pranaMessage);
                    order.Text = PreTradeConstants.MsgTradePending;
                    AddRemovePendingApprovalOrders(order);
                }
            }
        }
        Dictionary<string, OrderSingle> dictParentClOrderIDCollection = new Dictionary<string, OrderSingle>();
        OrderBindingList workingSubsTabCollection = new OrderBindingList();
        OrderBindingList ordersTabCollection = new OrderBindingList();
        List<OrderSingle> listMultiBrokerOrders = new List<OrderSingle>();
        /// <summary>
        /// The pending approval orders cache
        /// </summary>
        Dictionary<string, OrderSingle> _pendingApprovalOrders = new Dictionary<string, OrderSingle>();
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        #region Public Properties
        object _lockOnWorkingSubsTabCollection = new object();
        public OrderBindingList WorkingSubsTabCollection
        {
            get
            {
                lock (_lockOnWorkingSubsTabCollection)
                {
                    return workingSubsTabCollection;
                }
            }
        }

        object _lockOnOrdersTabCollection = new object();
        public OrderBindingList OrdersTabCollection
        {
            get
            {
                lock (_lockOnOrdersTabCollection)
                {
                    return ordersTabCollection;
                }
            }
        }

        public object _lockOnParentClOrderIDCollection = new object();
        public Dictionary<string, OrderSingle> DictParentClOrderIDCollection
        {
            get
            {
                lock (_lockOnParentClOrderIDCollection)
                {
                    return dictParentClOrderIDCollection;
                }
            }
        }

        /// <summary>
        /// Updates the StagedOrder on Blotter when the trades are sent from the MTT
        /// https://jira.nirvanasolutions.com:8443/browse/CI-6183
        /// </summary>
        /// <param name="subOrder"></param>
        public void UpdateOrderTabCollectionFromMTT(OrderSingle subOrder)
        {
            lock (_lockOnParentClOrderIDCollection)
            {
                if (dictParentClOrderIDCollection.ContainsKey(subOrder.StagedOrderID))
                {
                    OrderSingle stageOrder = dictParentClOrderIDCollection[subOrder.StagedOrderID];
                    stageOrder.LeavesQty += subOrder.Quantity;
                    stageOrder.UnsentQty = stageOrder.Quantity - stageOrder.LeavesQty - stageOrder.CumQty;
                    stageOrder.PropertyHasChanged();
                }
            }
        }
        #endregion

        public void ClearAllCollections()
        {
            lock (_lockOnWorkingSubsTabCollection)
            {
                workingSubsTabCollection.Clear();
            }
            lock (_lockOnOrdersTabCollection)
            {
                ordersTabCollection.Clear();
                if (ordersTabCollection.Count != 0)
                    ordersTabCollection.Clear();
            }
            lock (_lockOnParentClOrderIDCollection)
            {
                dictParentClOrderIDCollection.Clear();
            }
        }

        /// <summary>
        /// Adds the remove pending approval orders.
        /// </summary>
        /// <param name="order">The order.</param>
        public void AddRemovePendingApprovalOrders(OrderSingle order)
        {
            try
            {
                if (order.StagedOrderID != string.Empty && order.StagedOrderID != int.MinValue.ToString())
                {
                    if (!_pendingApprovalOrders.ContainsKey(order.ClOrderID) && order.Text == PreTradeConstants.MsgTradePending)
                    {
                        _pendingApprovalOrders.Add(order.ClOrderID, order);
                        RequiresComplianceApprovalEventHandler?.Invoke(this, new EventArgs<OrderSingle>(order));
                    }
                    else if (_pendingApprovalOrders.ContainsKey(order.ClOrderID) && (order.Text == string.Empty || order.Text == PreTradeConstants.MsgTradeReject))
                    {
                        _pendingApprovalOrders.Remove(order.ClOrderID);
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
        /// Checks the order is in Pending approval trades cache or not.
        /// </summary>
        /// <param name="ClOrderID"></param>
        /// <returns></returns>
        public bool IsOrderInPendingApprovalCache(string ClOrderID)
        {
            bool result = false;
            try
            {
                if (_pendingApprovalOrders != null && ClOrderID != null && _pendingApprovalOrders.ContainsKey(ClOrderID))
                    result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns pending compliance approval order by clOrderID.
        /// </summary>
        /// <param name="clOrderID"></param>
        /// <returns></returns>
        public OrderSingle GetPendingApprovalOrderByClOrderID(string clOrderID)
        {
            if (_pendingApprovalOrders != null && _pendingApprovalOrders.ContainsKey(clOrderID))
                return _pendingApprovalOrders[clOrderID];
            else
                return null;
        }

        /// <summary>
        /// Adds the pending approval orders in cache.
        /// </summary>
        public void AddPendingApprovalOrdersInCache()
        {
            try
            {
                UpdateBlotterCollection(_pendingApprovalOrders.Values.ToList());
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

        public event EventHandler<IndexEventArgs> OrderCollectionIndexChanged;
        public event EventHandler<IndexEventArgs> WorkingSubCollectionIndexChanged;
        public event EventHandler UpdateOnRolloverComplete;
        public event EventHandler<EventArgs<OrderSingle>> SendBlotterTradesEventHandler;
        public class IndexEventArgs : EventArgs
        {
            private int index = 0;

            public IndexEventArgs()
            { }
            public IndexEventArgs(int indexRow)
            {
                index = indexRow;
            }
            public int Index
            {
                get { return index; }
                set { index = value; }
            }
        }

        public Dictionary<string, List<OrderSingle>> UpdateBlotterCollection(List<OrderSingle> incomingOrders)
        {
            Dictionary<string, List<OrderSingle>> newAddedBlotterData = new Dictionary<string, List<OrderSingle>>();
            Dictionary<string, OrderSingle> _orderTab = new Dictionary<string, OrderSingle>();
            Dictionary<string, OrderSingle> _workingTab = new Dictionary<string, OrderSingle>();
            try
            {
                lock (_lockOnParentClOrderIDCollection)
                {
                    foreach (OrderSingle incomingOrder in incomingOrders)
                    {
                        DateTime dtTransactionTime;
                        if (!(incomingOrder.TransactionTime.ToString().Contains("/")))
                        {
                            dtTransactionTime = Convert.ToDateTime(incomingOrder.TransactionTime);
                        }
                        else
                        {
                            dtTransactionTime = Convert.ToDateTime(incomingOrder.TransactionTime);
                        }

                        incomingOrder.TransactionTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dtTransactionTime, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));

                        incomingOrder.ExecutionTimeLastFill = incomingOrder.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                        if (incomingOrder.ListID == string.Empty)
                        {
                            if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                            {
                                OrderSingle existingOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];
                                OrderSingle grandParentOrder = null;
                                if (incomingOrder.VenueID != int.MinValue)
                                {
                                    incomingOrder.Venue = CachedDataManager.GetInstance.GetVenueText(incomingOrder.VenueID);
                                }
                                if (incomingOrder.CounterPartyID != int.MinValue)
                                {
                                    incomingOrder.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(incomingOrder.CounterPartyID);
                                }
                                if (existingOrder.ShortLocateParameter == null && !string.IsNullOrEmpty(existingOrder.BorrowerID) && existingOrder.NirvanaLocateID != 0)
                                {
                                    existingOrder.ShortLocateParameter = new ShortLocateListParameter();
                                    existingOrder.ShortLocateParameter.BorrowerId = existingOrder.BorrowerID;
                                    existingOrder.ShortLocateParameter.NirvanaLocateID = existingOrder.NirvanaLocateID;
                                }
                                if (incomingOrder.SwapParameters != null)
                                {
                                    existingOrder.SwapParameters = incomingOrder.SwapParameters;
                                    existingOrder.AssetName = "EquitySwap";
                                }
                                UpdateOrderDetails(existingOrder, incomingOrder);
                                //In case of Parent Sub orders of GTC/GTD, re-calcualte Quantity from the child orders for New/Replace/Cancel scenarios
                                if (incomingOrder.PranaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged && OrderInformation.IsMultiDayOrderHistory(existingOrder))
                                {
                                    UpdateStatusFromChildCollection(existingOrder);
                                    existingOrder.PropertyHasChanged();
                                }
                                if (incomingOrder.StagedOrderID != string.Empty && incomingOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                {
                                    OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                    double parentOrderprevQty = parentOrder.CumQty;
                                    UpdateStatusFromChildCollection(parentOrder);
                                    UpdateParentStatus(incomingOrder, parentOrder);

                                    if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                    {
                                        grandParentOrder = dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                        grandParentOrder.LastPrice = incomingOrder.LastPrice;

                                        grandParentOrder.LastShares = incomingOrder.LastShares;
                                        UpdateParentStatusFromSub(parentOrder, grandParentOrder, parentOrderprevQty, incomingOrder.AvgPrice);
                                    }
                                    if (incomingOrder.LastPrice != double.MinValue)
                                    {
                                        parentOrder.LastPrice = incomingOrder.LastPrice;
                                    }
                                    if (incomingOrder.LastShares != double.MinValue)
                                    {
                                        parentOrder.LastShares = incomingOrder.LastShares;
                                    }

                                    #region Adding for Web
                                    UpdateBlotterDataForWeb(grandParentOrder,parentOrder,incomingOrder,_orderTab,_workingTab);
                                    #endregion

                                    parentOrder.PropertyHasChanged();
                                }
                                else if (incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                                {
                                    OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];
                                    UpdateStatusFromChildCollection(parentOrder);
                                    UpdateParentStatus(incomingOrder, parentOrder);
                                    if (incomingOrder.LastPrice != double.MinValue)
                                    {
                                        parentOrder.LastPrice = incomingOrder.LastPrice;
                                    }

                                    if (incomingOrder.LastShares != double.MinValue)
                                    {
                                        parentOrder.LastShares = incomingOrder.LastShares;
                                    }

                                    #region Adding for Web
                                    if (_orderTab != null && !_orderTab.ContainsKey(parentOrder.ParentClOrderID))
                                        _orderTab.Add(parentOrder.ParentClOrderID, parentOrder);
                                    #endregion

                                    parentOrder.PropertyHasChanged();
                                }
                                else if (incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.MsgTransferUser)
                                {
                                    OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];
                                    parentOrder.PropertyHasChanged();
                                }
                                existingOrder.PropertyHasChanged();
                                UpdateStagedOrderForMultidayWithMultiBroker(incomingOrder);
                            }
                            else
                            {
                                ProcessNewOrder(incomingOrder);
                                if (incomingOrder.ShortLocateParameter != null && incomingOrder.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged && incomingOrder.Text != PreTradeConstants.MsgTradeReject)
                                {
                                    if (UpdateShortLocateData != null)
                                        UpdateShortLocateData(this, new EventArgs<ShortLocateListParameter>(incomingOrder.ShortLocateParameter));
                                }
                                if (!dictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                                {
                                    dictParentClOrderIDCollection.Add(incomingOrder.ParentClOrderID, incomingOrder);
                                    if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                    {
                                        OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                        parentOrder.IsMultiBrokerTrade = incomingOrder.IsMultiBrokerTrade;
                                    }   
                                    Dictionary<string, OrderSingle> addedBlotterData = AddToOrderCollection(incomingOrder);

                                    #region Adding for Web
                                    if (addedBlotterData != null && addedBlotterData.Count > 0)
                                    {
                                        if (addedBlotterData.ContainsKey(BlotterDataConstants.CONST_OrderTab))
                                        {
                                            if (_orderTab != null && !_orderTab.ContainsKey(addedBlotterData[BlotterDataConstants.CONST_OrderTab].ParentClOrderID))
                                                _orderTab.Add(addedBlotterData[BlotterDataConstants.CONST_OrderTab].ParentClOrderID, addedBlotterData[BlotterDataConstants.CONST_OrderTab]);
                                        }
                                        if (addedBlotterData.ContainsKey(BlotterDataConstants.CONST_WorkingTab))
                                        {
                                            if (_workingTab != null && !_workingTab.ContainsKey(addedBlotterData[BlotterDataConstants.CONST_WorkingTab].ParentClOrderID))
                                                _workingTab.Add(addedBlotterData[BlotterDataConstants.CONST_WorkingTab].ParentClOrderID, addedBlotterData[BlotterDataConstants.CONST_WorkingTab]);
                                        }
                                    }
                                    #endregion
                                }
                                incomingOrder.PropertyHasChanged();
                                if (incomingOrder.StagedOrderID != string.Empty && incomingOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                {
                                    OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                    parentOrder.IsMultiBrokerTrade = incomingOrder.IsMultiBrokerTrade;
                                    UpdateStatusFromChildCollection(parentOrder);
                                    UpdateParentStatus(incomingOrder, parentOrder);
                                    if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                    {
                                        OrderSingle grandParentOrder = dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                        grandParentOrder.LastPrice = incomingOrder.LastPrice;
                                        grandParentOrder.LastShares = incomingOrder.LastShares;
                                        UpdateParentStatusFromSub(parentOrder, grandParentOrder, parentOrder.CumQty, incomingOrder.AvgPrice);

                                        #region Adding for Web
                                        UpdateBlotterDataForWeb(grandParentOrder,parentOrder,incomingOrder,_orderTab,_workingTab);
                                        #endregion
                                    }
                                    parentOrder.PropertyHasChanged();
                                }
                                UpdateStagedOrderForMultidayWithMultiBroker(incomingOrder);
                            }
                            UpdateMultiBrokerOrderList(dictParentClOrderIDCollection[incomingOrder.ParentClOrderID]);
                        }

                        //Forcefully updated value of Account, Master fund, Strategy and Allocation Status value to Dash (-) in case of Order status is Pending New or Rejected. 
                        //Because in case of Pending new and Rejected case, These groups are not visible in Allocation.
                        if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew || incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                        {
                            incomingOrder.AllocationSchemeName = OrderFields.PROPERTY_DASH;
                            incomingOrder.AllocationStatus = OrderFields.PROPERTY_DASH;
                            incomingOrder.Strategy = OrderFields.PROPERTY_DASH;
                            incomingOrder.Account = OrderFields.PROPERTY_DASH;
                            incomingOrder.MasterFund = OrderFields.PROPERTY_DASH;
                        }

                        if (_blotterPreferenceData.RejectionPopup && incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                        {
                            if (TradeManagerExtension.GetInstance().IsWebApplication || (CachedDataManager.GetInstance.LoggedInUser != null && CachedDataManager.GetInstance.LoggedInUser.CompanyUserID == incomingOrder.CompanyUserID))
                            {
                                if (ShowCustomMessageBoxEventHandler != null)
                                    ShowCustomMessageBoxEventHandler(this, new EventArgs<OrderSingle>(incomingOrder));
                            }
                        }

                        if (_enableTradeFlowLogging)
                        {
                            try
                            {
                                Logger.LoggerWrite("[Trade-Flow] Fix Message bound to grid for display userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Symbol = " + incomingOrder.Symbol + ", ExecID = " + incomingOrder.ExecID + ", MsgSeqNum = " + incomingOrder.MsgSeqNum +
                                  ", OrderID = " + incomingOrder.OrderID + ", CumQty = " + incomingOrder.CumQty + ", TransactionTime = " + incomingOrder.TransactionTime, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            }
                        }
                    }
                    if (newOrderList.Count > 0)
                    {
                        ordersTabCollection.AddRange(newOrderList);
                        newOrderList.Clear();
                    }
                    if (newWorkingSubsList.Count > 0)
                    {
                        workingSubsTabCollection.AddRange(newWorkingSubsList);
                        newWorkingSubsList.Clear();
                    }

                    if (newAddedBlotterData != null && !newAddedBlotterData.ContainsKey(BlotterDataConstants.CONST_OrderTab) && _orderTab != null && _orderTab.Count > 0)
                        newAddedBlotterData.Add(BlotterDataConstants.CONST_OrderTab, _orderTab.Values.ToList());
                    if (newAddedBlotterData != null && !newAddedBlotterData.ContainsKey(BlotterDataConstants.CONST_WorkingTab) && _workingTab != null && _workingTab.Count > 0)
                        newAddedBlotterData.Add(BlotterDataConstants.CONST_WorkingTab, _workingTab.Values.ToList());
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
           return newAddedBlotterData;
        }

        /// <summary>
        /// Updating blotter data in the case of garndParent orders
        /// </summary>
        private void UpdateBlotterDataForWeb(OrderSingle grandParentOrder, OrderSingle parentOrder, OrderSingle incomingOrder, Dictionary<string, OrderSingle> orderTab, Dictionary<string, OrderSingle> workingTab)
        {
            try
            {
                if (grandParentOrder == null)
                {
                    if (orderTab != null && !orderTab.ContainsKey(parentOrder.ParentClOrderID))
                        orderTab.Add(parentOrder.ParentClOrderID, parentOrder);
                    if (workingTab != null && !workingTab.ContainsKey(incomingOrder.ParentClOrderID))
                    {
                        if (!(incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver) && string.IsNullOrEmpty(incomingOrder.OrderStatus)))
                        {
                            workingTab.Add(incomingOrder.ParentClOrderID, incomingOrder);
                        }
                    }
                }
                else
                {
                    if (orderTab != null && !orderTab.ContainsKey(grandParentOrder.ParentClOrderID))
                        orderTab.Add(grandParentOrder.ParentClOrderID, grandParentOrder);
                    if (workingTab != null && !workingTab.ContainsKey(parentOrder.ParentClOrderID))
                    {
                        if (!(incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver) && string.IsNullOrEmpty(incomingOrder.OrderStatus)))
                        {
                            workingTab.Add(parentOrder.ParentClOrderID, parentOrder);
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
        }
        /// <summary>
        /// Shows the pending pop up message.
        /// </summary>
        /// <param name="ClOrderID">The cl order identifier.</param>
        /// <returns></returns>
        public string ShowPendingPopUpMessage(OrderSingle order)
        {
            string message = string.Empty;
            try
            {
                var ordSingle = workingSubsTabCollection.FirstOrDefault(ord => ord.ClOrderID.Equals(order.ClOrderID));
                if (ordSingle != null && ordSingle.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                {
                    string orderside = TagDatabaseManager.GetInstance.GetOrderSideText(ordSingle.OrderSideTagValue);
                    string ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(ordSingle.OrderTypeTagValue);
                    if (ordertype == "Limit")
                        message = orderside + ", " + ordSingle.Quantity + " " + ordSingle.Symbol + " at " + ordertype + " Price: " + ordSingle.Price + " to " + ordSingle.CounterPartyName + Environment.NewLine + "This order is not acknowledged by Broker." + Environment.NewLine + "Please contact your support representative.";
                    else
                        message = orderside + ", " + ordSingle.Quantity + " " + ordSingle.Symbol + " at " + ordertype + " Price to " + ordSingle.CounterPartyName + Environment.NewLine + "This order is not acknowledged by Broker." + Environment.NewLine + "Please contact your support representative.";
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
            return message;
        }

        private void UpdateParentStatusFromSub(OrderSingle parentOrder, OrderSingle grandParentOrder, double parentOrderPrevQty, double avgPx)
        {
            try
            {
                if (OrderInformation.IsMultiDayOrderHistory(parentOrder) &&
                    (parentOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Expired) || parentOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_DoneForDay)))
                {
                    UpdateStatusFromChildCollection(grandParentOrder);
                }
                else
                {
                    double amount = grandParentOrder.CumQty * grandParentOrder.AvgPrice;

                    if (parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingNew && parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_New && parentOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected && parentOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                    {
                        grandParentOrder.CumQty = grandParentOrder.CumQty + parentOrder.CumQty - parentOrderPrevQty;
                        grandParentOrder.LeavesQty = grandParentOrder.Quantity - grandParentOrder.CumQty - grandParentOrder.UnsentQty;
                    }

                    parentOrder.UnsentQty = 0;
                    parentOrder.LeavesQty = parentOrder.Quantity - parentOrder.CumQty;
                    if (OrderInformation.IsOrderInEndState(parentOrder))
                        parentOrder.LeavesQty = 0.0;
                    amount += ((parentOrder.CumQty - parentOrderPrevQty) * avgPx);

                    if (grandParentOrder.CumQty > 0)
                    {
                        grandParentOrder.AvgPrice = amount / grandParentOrder.CumQty;
                    }
                }
                if (grandParentOrder.CumQty == 0)
                {
                    grandParentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                }
                else if (grandParentOrder.CumQty == grandParentOrder.Quantity)
                {
                    grandParentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                }
                else if (grandParentOrder.CumQty > 0 && grandParentOrder.CumQty < grandParentOrder.Quantity)
                {
                    grandParentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                }

                grandParentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(grandParentOrder.OrderStatusTagValue.ToString());
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

        private Dictionary<string, OrderSingle> AddToOrderCollection(OrderSingle incomingOrder)
        {
            Dictionary<string, OrderSingle> newAddedBlotterData = new Dictionary<string, OrderSingle>();
            try
            {
                bool isSkipOrderFromWorkingSubCollection = false;
                DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                bool isClearanceCalculated = false;
                DateTime clearanceTime = new DateTime();
                if (incomingOrder.MsgType != FIXConstants.MSGOrderRollOverRequest && BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.TryGetValue(incomingOrder.AUECID, out clearanceTime))
                {
                    clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(clearanceTime, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                    if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                    {
                        clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                    }
                    else if (_isOnlyAUECTimeBasedBlotterClearance)
                    {
                        clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                    }
                    else
                    {
                        clearanceTime = currentAUECDate.Date;
                    }
                    isClearanceCalculated = true;
                }
                if (incomingOrder.MsgType == FIXConstants.MSGOrderRollOverRequest || incomingOrder.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDNewSubChild ||
                    (isClearanceCalculated && clearanceTime > incomingOrder.AUECLocalDate
                      && !OrderInformation.IsMultiDayOrderHistory(incomingOrder)))
                {
                    //Skip those working sub order whose clearence have been passed and those comes from DB with their staged order (multiday staging)
                    isSkipOrderFromWorkingSubCollection = true;
                }

                if (incomingOrder.PranaMsgType == (int)Global.OrderFields.PranaMsgTypes.ORDStaged)
                {
                    lock (_lockOnOrdersTabCollection)
                    {
                        //ordersTabCollection.Add(incomingOrder);
                        newOrderList.Add(incomingOrder);
                        newAddedBlotterData.Add(BlotterDataConstants.CONST_OrderTab, incomingOrder);
                    }
                }
                else
                {
                    if (!isSkipOrderFromWorkingSubCollection)
                    {
                        lock (_lockOnWorkingSubsTabCollection)
                        {
                            DateTime currentAuecdate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                            if ((incomingOrder.StagedOrderID != string.Empty))
                            {
                                if (_isOnlyAUECTimeBasedBlotterClearance || incomingOrder.AUECLocalDate.Date >= currentAuecdate.Date || (!incomingOrder.TIF.Equals(FIXConstants.TIF_Day) && !OrderInformation.IsMultiDayOrderInEndState(incomingOrder)))
                                {
                                    if (!workingSubsTabCollection.Contains(incomingOrder))
                                    {
                                        if (workingSubsTabCollection.Where(x => x.ParentClOrderID.Equals(incomingOrder.StagedOrderID.ToString())).Count() == 0 && newWorkingSubsList.Where(x => x.ParentClOrderID.Equals(incomingOrder.StagedOrderID.ToString())).Count() == 0 && !incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver))
                                        {
                                            //  workingSubsTabCollection.Add(incomingOrder);
                                            newWorkingSubsList.Add(incomingOrder);
                                            newAddedBlotterData.Add(BlotterDataConstants.CONST_WorkingTab, incomingOrder);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!workingSubsTabCollection.Contains(incomingOrder) && !newWorkingSubsList.Contains(incomingOrder))
                                {
                                    //workingSubsTabCollection.Add(incomingOrder);
                                    newWorkingSubsList.Add(incomingOrder);
                                    newAddedBlotterData.Add(BlotterDataConstants.CONST_WorkingTab, incomingOrder);
                                }
                            }
                        }
                    }
                    lock (_lockOnParentClOrderIDCollection)
                    {
                        if ((incomingOrder.StagedOrderID != string.Empty) && (incomingOrder.StagedOrderID != incomingOrder.ClOrderID))
                        {
                            if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                            {
                                OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                if (parentOrder.OrderCollection == null)
                                {
                                    parentOrder.OrderCollection = new OrderBindingList();
                                }
                                if (parentOrder.OrderCollection.Where(x => x.ParentClOrderID == incomingOrder.ParentClOrderID).Count() == 0)
                                {
                                    parentOrder.OrderCollection.Add(incomingOrder);
                                }
                                else
                                {
                                    // When SubOrder is already present update sub order details
                                    OrderSingle existingOrder = parentOrder.OrderCollection.Where(x => x.ParentClOrderID == incomingOrder.ParentClOrderID).FirstOrDefault();
                                    UpdateOrderDetails(existingOrder, incomingOrder);
                                }

                                // In case of multi-day order with rollover status, add its parent order details to blotter data 
                                bool isStatusRollOver = (OrderInformation.IsMultiDayOrder(incomingOrder) && !workingSubsTabCollection.Contains(incomingOrder) && workingSubsTabCollection.Where(x => x.ParentClOrderID.Equals(incomingOrder.StagedOrderID.ToString())).Count() == 0
                                                                && newWorkingSubsList.Where(x => x.ParentClOrderID.Equals(incomingOrder.StagedOrderID.ToString())).Count() == 0 && incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver));

                                if ((newAddedBlotterData != null && newAddedBlotterData.Count > 0) || isStatusRollOver)
                                {
                                    if (!newAddedBlotterData.Keys.Contains(BlotterDataConstants.CONST_OrderTab))
                                    {
                                        newAddedBlotterData.Add(BlotterDataConstants.CONST_OrderTab, parentOrder);
                                    }
                                }
                               
                                /* the following code ensures that new suborders for which ack 
                                 * is not received still update parent quantities
                                */
                                double parentOrderPrevQty = parentOrder.CumQty;
                                UpdateStatusFromChildCollection(parentOrder);
                                UpdateParentStatus(incomingOrder, parentOrder);

                                if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                {
                                    OrderSingle grandParentOrder = dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                    grandParentOrder.LastPrice = incomingOrder.LastPrice;
                                    grandParentOrder.LastShares = incomingOrder.LastShares;
                                    UpdateParentStatusFromSub(parentOrder, grandParentOrder, parentOrderPrevQty, incomingOrder.AvgPrice);
                                }
                                parentOrder.LastPrice = incomingOrder.LastPrice;
                                parentOrder.LastShares = incomingOrder.LastShares;
                                parentOrder.PropertyHasChanged();
                            }
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
            return newAddedBlotterData;
        }

        public OrderSingle GetOrderByClOrderID(string clOrderID)
        {
            lock (_lockOnParentClOrderIDCollection)
            {
                if (dictParentClOrderIDCollection.ContainsKey(clOrderID))
                    return dictParentClOrderIDCollection[clOrderID];
                else
                    return null;
            }
        }

        private void UpdateOrderDetails(OrderSingle blotterOrder, OrderSingle incomingOrder)
        {
            try
            {
                string msgType = incomingOrder.MsgType;
                switch (msgType)
                {
                    case FIXConstants.MSGOrder:
                        if (blotterOrder.AlgoStrategyID != String.Empty && blotterOrder.AlgoStrategyID != int.MinValue.ToString() && blotterOrder.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew)
                        {
                            ProcessNewFIXAlgoOrder(blotterOrder, incomingOrder);
                        }
                        else
                            UpdateExecutionReport(blotterOrder, incomingOrder);
                        break;
                    case FIXConstants.MSGExecutionReport:
                        UpdateExecutionReport(blotterOrder, incomingOrder);
                        break;
                    case FIXConstants.MSGOrderCancelReject:
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                        UpdateCancelAndReplaceRequests(blotterOrder, incomingOrder);
                        break;
                    case FIXConstants.MSGOrderCancelRequest:
                        UpdateCancelRequests(blotterOrder, incomingOrder);
                        break;
                    case FIXConstants.MSGOrderRollOverRequest:
                        UpdateRolloverRequests(blotterOrder, incomingOrder);
                        break;
                    case FIXConstants.MSGTransferUser:
                        UpdateTransferUserDetails(blotterOrder, incomingOrder);
                        break;
                    case FIXConstants.MSGOrderList:
                        return;
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                        return;
                    default:
                        throw new Exception("Blotter Main. This msg type is not handled. \\n");
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

        private void UpdateExecutionReport(OrderSingle blotterOrder, OrderSingle orderResponse)
        {
            try
            {
                blotterOrder.ClOrderID = orderResponse.ClOrderID;
                blotterOrder.OrderTypeTagValue = orderResponse.OrderTypeTagValue;
                blotterOrder.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(orderResponse.OrderTypeTagValue.ToString());
                blotterOrder.OrderSideTagValue = orderResponse.OrderSideTagValue;
                blotterOrder.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderResponse.OrderSideTagValue.ToString());
                blotterOrder.Text = orderResponse.Text;
                blotterOrder.Quantity = orderResponse.Quantity;
                blotterOrder.InternalComments = orderResponse.InternalComments;
                blotterOrder.TradeAttribute1 = orderResponse.TradeAttribute1;
                blotterOrder.TradeAttribute2 = orderResponse.TradeAttribute2;
                blotterOrder.TradeAttribute3 = orderResponse.TradeAttribute3;
                blotterOrder.TradeAttribute4 = orderResponse.TradeAttribute4;
                blotterOrder.TradeAttribute5 = orderResponse.TradeAttribute5;
                blotterOrder.TradeAttribute6 = orderResponse.TradeAttribute6;
                blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue);
                if (!string.IsNullOrEmpty(orderResponse.TIF))
                    blotterOrder.TIF = orderResponse.TIF;
                blotterOrder.IsUseCustodianBroker = orderResponse.IsUseCustodianBroker;

                //The following check is reqd while updating data from DB
                //We need to handle the case when we havent recieved an ack from the EP corresponding
                //to a particular order.
                if (orderResponse.ExecID != string.Empty)
                {
                    blotterOrder.CounterPartyID = orderResponse.CounterPartyID;
                    if (blotterOrder.IsUseCustodianBroker && blotterOrder.Account == OrderFields.PROPERTY_MULTIPLE && blotterOrder.CounterPartyID == int.MinValue)
                    {
                        blotterOrder.CounterPartyName = OrderFields.PROPERTY_MULTIPLE;
                    }
                    else if (blotterOrder.CounterPartyID != int.MinValue)
                    {
                        blotterOrder.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(blotterOrder.CounterPartyID);
                    }
                    else
                    {
                        blotterOrder.CounterPartyName = string.Empty;
                    }

                    blotterOrder.VenueID = orderResponse.VenueID;
                    if (blotterOrder.VenueID != int.MinValue)
                    {
                        blotterOrder.Venue = CachedDataManager.GetInstance.GetVenueText(orderResponse.VenueID);
                    }
                    else
                    {
                        blotterOrder.Venue = string.Empty;
                    }

                    if (blotterOrder.TradingAccountID != int.MinValue)
                    {
                        blotterOrder.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(orderResponse.TradingAccountID);
                    }
                    blotterOrder.LastShares = orderResponse.LastShares;
                    //ExectuionTimeLastFill is being utilized in the Multi-Broker flow and we dont need Rollover timestamp so added this condition
                    if (!(OrderInformation.IsMultiDayOrder(blotterOrder) &&
                          (orderResponse.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_RollOver) || orderResponse.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingRollOver))))
                        blotterOrder.ExecutionTimeLastFill = orderResponse.ExecutionTimeLastFill;
                    blotterOrder.CumQty = orderResponse.CumQty;
                    blotterOrder.LastPrice = orderResponse.LastPrice;
                    blotterOrder.SettlementCurrencyID = orderResponse.SettlementCurrencyID;
                    blotterOrder.AvgPrice = orderResponse.AvgPrice;
                    if (!OrderInformation.IsMultiDayOrderHistory(blotterOrder))
                    {
                        blotterOrder.DayCumQty = blotterOrder.CumQty;
                        blotterOrder.DayAvgPx = blotterOrder.AvgPrice;
                        blotterOrder.DayOrderQty = blotterOrder.Quantity;
                    }
                    blotterOrder.FXRate = orderResponse.FXRate;
                    blotterOrder.FXConversionMethodOperator = orderResponse.FXConversionMethodOperator;
                    blotterOrder.Price = orderResponse.Price;
                    if (orderResponse.StopPrice != double.Epsilon)
                    {
                        blotterOrder.StopPrice = orderResponse.StopPrice;
                    }
                    blotterOrder.LastMarket = orderResponse.LastMarket;
                    blotterOrder.OrderID = orderResponse.OrderID;
                    if (orderResponse.OrigClOrderID != string.Empty)
                    {
                        blotterOrder.OrigClOrderID = orderResponse.OrigClOrderID;
                    }
                    if (orderResponse.OrderStatusTagValue != string.Empty)
                    {
                        blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue.ToString());
                        blotterOrder.OrderStatusTagValue = orderResponse.OrderStatusTagValue;
                    }
                    blotterOrder.OrderSeqNumber = orderResponse.OrderSeqNumber;
                    blotterOrder.CommissionAmt = orderResponse.CommissionAmt;
                    blotterOrder.SoftCommissionAmt = orderResponse.SoftCommissionAmt;

                    switch (orderResponse.OrderStatusTagValue)
                    {
                        case FIXConstants.ORDSTATUS_New:
                        case FIXConstants.ORDSTATUS_PendingNew:
                            //We dont recieve the correct Leaves Quantity for Acks.. so to handle that
                            orderResponse.LeavesQty = orderResponse.Quantity - orderResponse.CumQty;
                            blotterOrder.LeavesQty = orderResponse.Quantity - orderResponse.CumQty;

                            /*
                              * the following code is to ensure that for a new staged order 
                              * leaves and unsent quantities are set to their default values

                              * For a new staged order ->
                              * the working quantity(leaves quantity) shud equal 0.0
                              * the unsent quantity shud equal order quantity
                            */

                            switch (blotterOrder.PranaMsgType)
                            {
                                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged:
                                    blotterOrder.LeavesQty = 0.0;
                                    break;
                                case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                                    if (blotterOrder.LeavesQty == 0.0)
                                    {
                                        blotterOrder.LeavesQty = 0.0;
                                        blotterOrder.UnsentQty = blotterOrder.Quantity;
                                    }
                                    break;
                                default:
                                    blotterOrder.LeavesQty = orderResponse.Quantity - orderResponse.CumQty; ;
                                    break;
                            }
                            break;

                        case FIXConstants.ORDSTATUS_Rejected:
                            blotterOrder.LeavesQty = 0.0;
                            blotterOrder.UnsentQty = orderResponse.Quantity;
                            if (orderResponse.ShortLocateParameter != null)
                            {
                                orderResponse.ShortLocateParameter.BorrowQuantity = -orderResponse.ShortLocateParameter.BorrowQuantity;
                                if (UpdateShortLocateData != null)
                                    UpdateShortLocateData(this, new EventArgs<ShortLocateListParameter>(orderResponse.ShortLocateParameter));
                            }
                            break;

                        case FIXConstants.ORDSTATUS_PartiallyFilled:
                            blotterOrder.LeavesQty = orderResponse.LeavesQty;
                            break;

                        case FIXConstants.ORDSTATUS_Cancelled:
                        case FIXConstants.ORDSTATUS_Expired:
                        case FIXConstants.ORDSTATUS_RollOver:
                            if (blotterOrder.ShortLocateParameter != null)
                            {
                                ShortLocateListParameter param = new ShortLocateListParameter();
                                param.NirvanaLocateID = blotterOrder.ShortLocateParameter.NirvanaLocateID;
                                param.BorrowerId = blotterOrder.ShortLocateParameter.BorrowerId;
                                if (blotterOrder.Text == PreTradeConstants.MsgTradeReject || blotterOrder.Text == PreTradeConstants.MsgTradePending)
                                    param.BorrowQuantity = -(blotterOrder.Quantity - blotterOrder.CumQty);
                                else
                                    param.BorrowQuantity = -blotterOrder.LeavesQty;
                                if (UpdateShortLocateData != null)
                                    UpdateShortLocateData(this, new EventArgs<ShortLocateListParameter>(param));
                            }
                            blotterOrder.LeavesQty = 0.0;

                            if (orderResponse.AlgoStrategyID != string.Empty && orderResponse.AlgoStrategyID != int.MinValue.ToString())
                            {
                                if (AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary.ContainsKey(blotterOrder.ParentClOrderID))
                                {
                                    AlgoReplaceManager.GetInstance().SendReplaceOrder(blotterOrder.ParentClOrderID);
                                }
                            }
                            break;

                        case FIXConstants.ORDSTATUS_Filled:
                            blotterOrder.LeavesQty = orderResponse.LeavesQty;
                            break;

                        case FIXConstants.ORDSTATUS_Replaced:
                            if (blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged && blotterOrder.LeavesQty == 0.0)
                            {
                                blotterOrder.LeavesQty = 0.0;
                            }
                            else
                            {
                                if (blotterOrder.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged && blotterOrder.ShortLocateParameter != null)
                                {
                                    ShortLocateListParameter param = new ShortLocateListParameter();
                                    param.NirvanaLocateID = blotterOrder.ShortLocateParameter.NirvanaLocateID;
                                    param.BorrowerId = blotterOrder.ShortLocateParameter.BorrowerId;
                                    if ((blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub || blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild) && blotterOrder.StagedOrderID != string.Empty && blotterOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(blotterOrder.StagedOrderID))
                                    {
                                        Dictionary<string, OrderSingle> dictParentCl = new Dictionary<string, OrderSingle>();
                                        dictParentCl = dictParentClOrderIDCollection.Where(dic => dic.Value.ShortLocateParameter != null && blotterOrder.ShortLocateParameter != null && dic.Value.ShortLocateParameter.NirvanaLocateID == blotterOrder.ShortLocateParameter.NirvanaLocateID).ToDictionary(dic => dic.Key, dic => dic.Value);

                                        foreach (OrderSingle subOrder in dictParentCl.Values)
                                        {

                                            if (subOrder.StagedOrderID != string.Empty && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Expired && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_RollOver && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                                                param.ReplaceQuantity += subOrder.Quantity;

                                            else if (subOrder.StagedOrderID != string.Empty && (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled || subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired || subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver || subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected))
                                            {
                                                param.ReplaceQuantity += subOrder.CumQty;
                                            }
                                        }
                                    }
                                    param.BorrowQuantity = -((blotterOrder.LeavesQty + blotterOrder.CumQty) - blotterOrder.Quantity);
                                    if (UpdateShortLocateData != null)
                                        UpdateShortLocateData(this, new EventArgs<ShortLocateListParameter>(param));
                                }

                                if (blotterOrder.Quantity > orderResponse.Quantity)
                                {
                                    blotterOrder.LeavesQty = blotterOrder.Quantity - blotterOrder.CumQty;

                                }
                                else if (blotterOrder.Quantity <= orderResponse.Quantity)
                                {
                                    blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                                }
                            }
                            //following code added to handle the case that an order is replaced to 
                            //quantity equal to it's cum qty.
                            if (orderResponse.CumQty == orderResponse.Quantity)
                            {
                                blotterOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                                blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(blotterOrder.OrderStatusTagValue.ToString());
                            }
                            if (orderResponse.AlgoStrategyID != string.Empty && orderResponse.AlgoStrategyID != int.MinValue.ToString())
                            {
                                blotterOrder.AlgoProperties = orderResponse.AlgoProperties;
                                blotterOrder.AlgoStrategyID = orderResponse.AlgoStrategyID;
                                blotterOrder.AlgoStrategyName = AlgoStrategyNamesDetails.GetAlgoStrategyText(
                                    blotterOrder.AlgoStrategyID, blotterOrder.CounterPartyID);
                            }
                            else
                            {
                                blotterOrder.AlgoStrategyID = string.Empty;
                                blotterOrder.AlgoProperties = new OrderAlgoStartegyParameters();
                                blotterOrder.AlgoStrategyName = string.Empty;
                            }
                            blotterOrder.Level2ID = orderResponse.Level2ID;
                            blotterOrder.Level1ID = orderResponse.Level1ID;
                            blotterOrder.ExecutionInstruction = orderResponse.ExecutionInstruction;
                            blotterOrder.HandlingInstruction = orderResponse.HandlingInstruction;
                            blotterOrder.TradingAccountID = orderResponse.TradingAccountID;
                            if (blotterOrder.TradingAccountID != int.MinValue)
                            {
                                blotterOrder.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(orderResponse.TradingAccountID);
                            }
                            blotterOrder.TIF = orderResponse.TIF;
                            if (orderResponse.ExpireTime != null && !(string.IsNullOrEmpty(orderResponse.ExpireTime)) && orderResponse.TIF == FIXConstants.TIF_GTD)
                            {
                                blotterOrder.ExpireTime = orderResponse.ExpireTime;
                            }
                            else if (orderResponse.TIF != FIXConstants.TIF_GTD)
                                blotterOrder.ExpireTime = "N/A";
                            break;
                        case FIXConstants.ORDSTATUS_DoneForDay:
                            //Kuldeep A.:- This is done considering only Day orders.
                            // If anytime in future, we need to handle Multiday orders then this handling should also be changed as in that case LeavesWty might change.
                            blotterOrder.LeavesQty = 0;
                            break;
                    }

                    if (orderResponse.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver && UpdateOnRolloverComplete != null)
                    {
                        UpdateOnRolloverComplete(this, new EventArgs());
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

        private void UpdateCancelAndReplaceRequests(OrderSingle blotterOrder, OrderSingle orderResponse)
        {
            try
            {
                if (orderResponse.ClOrderID != string.Empty)
                {
                    if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReject)
                    {
                        blotterOrder.ClOrderID = orderResponse.OrigClOrderID;
                        if (DictParentClOrderIDCollection.ContainsKey(blotterOrder.ParentClOrderID))
                        {
                            if (AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary.ContainsKey(blotterOrder.ParentClOrderID))
                            {
                                OrderSingle or = AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary[blotterOrder.ParentClOrderID];
                                OrderSingle existingReplaceOrder = DictParentClOrderIDCollection[or.ParentClOrderID];
                                existingReplaceOrder.OrderStatusTagValue = CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected;
                                existingReplaceOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(existingReplaceOrder.OrderStatusTagValue);
                                existingReplaceOrder.Text = "Previous cancel rejected";
                                AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary.Remove(orderResponse.ParentClOrderID);
                            }
                        }
                    }
                    if (orderResponse.MsgType == FIXConstants.MSGOrderCancelRequest)
                    {
                        blotterOrder.ClOrderID = orderResponse.ClOrderID;
                        blotterOrder.OrderStatusTagValue = orderResponse.OrderStatusTagValue;
                        blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue);
                    }

                    //In case we have received an execution report which had wrong Quantity Price etc.
                    //We should revert them.
                    blotterOrder.OrigClOrderID = orderResponse.OrigClOrderID;
                    blotterOrder.OrderStatusTagValue = orderResponse.OrderStatusTagValue;
                    blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue);
                    blotterOrder.Text = orderResponse.Text;

                    blotterOrder.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(orderResponse.OrderTypeTagValue);
                    blotterOrder.OrderTypeTagValue = orderResponse.OrderTypeTagValue;
                    blotterOrder.Price = orderResponse.Price;
                    blotterOrder.StopPrice = orderResponse.StopPrice;

                    if (blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.InternalOrder)
                    {
                        if (blotterOrder.Quantity > orderResponse.Quantity)
                        {
                            blotterOrder.LeavesQty = blotterOrder.Quantity - blotterOrder.CumQty;

                        }
                        else if (blotterOrder.Quantity < orderResponse.Quantity)
                        {
                            blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                        }
                    }
                    if (blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManual)
                    {
                        if (blotterOrder.Quantity > orderResponse.Quantity)
                        {
                            blotterOrder.LeavesQty = blotterOrder.Quantity - blotterOrder.CumQty;

                        }
                        else if (blotterOrder.Quantity < orderResponse.Quantity)
                        {
                            blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                        }
                    }

                    if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                    {
                        blotterOrder.ClOrderID = orderResponse.ClOrderID;

                        // the following code is to ensure that replace of staged order
                        // has correct Leaves and Unsent quantity 
                        switch (orderResponse.PranaMsgType)
                        {
                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged:
                                blotterOrder.AlgoProperties = orderResponse.AlgoProperties;
                                break;

                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub:
                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild:
                                // checks have to be put to check whether the child is replaced to a lower qty or higher
                                // then the working qty has to be accordingly adjusted
                                if (blotterOrder.Quantity < orderResponse.Quantity)
                                {
                                    blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                                }
                                else if (blotterOrder.Quantity > orderResponse.Quantity)
                                {
                                    blotterOrder.LeavesQty = blotterOrder.Quantity - blotterOrder.CumQty;
                                }
                                break;

                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                                blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                                blotterOrder.UnsentQty = 0.0;
                                break;
                        }
                        blotterOrder.Quantity = orderResponse.Quantity;
                    }
                    else if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReject)
                    {
                        blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                    }

                    //we can't update quantities here as this has to be checked for staged sub
                    blotterOrder.Quantity = orderResponse.Quantity;
                    blotterOrder.CommissionAmt = orderResponse.CommissionAmt;
                    blotterOrder.SoftCommissionAmt = orderResponse.SoftCommissionAmt;

                    blotterOrder.CommissionRate = orderResponse.CommissionRate;
                    blotterOrder.CalcBasis = orderResponse.CalcBasis;
                    blotterOrder.SoftCommissionRate = orderResponse.SoftCommissionRate;
                    blotterOrder.SoftCommissionCalcBasis = orderResponse.SoftCommissionCalcBasis;

                    if(orderResponse.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingReplace)
                    {
                        if(!orderResponse.FXRate.Equals(0))
                            blotterOrder.FXRate = orderResponse.FXRate;
                    }
                    else
                        blotterOrder.FXRate = orderResponse.FXRate;
                    blotterOrder.SettlementCurrencyID = orderResponse.SettlementCurrencyID;
                    blotterOrder.Level2ID = orderResponse.Level2ID;

                    blotterOrder.CounterPartyID = orderResponse.CounterPartyID;
                    blotterOrder.CounterPartyName = orderResponse.CounterPartyName;
                    blotterOrder.VenueID = orderResponse.VenueID;
                    blotterOrder.Venue = orderResponse.Venue;
                    blotterOrder.Level1ID = orderResponse.Level1ID;
                    blotterOrder.ExecutionInstruction = orderResponse.ExecutionInstruction;
                    blotterOrder.HandlingInstruction = orderResponse.HandlingInstruction;
                    blotterOrder.TradingAccountID = orderResponse.TradingAccountID;
                    blotterOrder.TIF = orderResponse.TIF;
                    blotterOrder.OrderSeqNumber = orderResponse.OrderSeqNumber;

                    if (blotterOrder.IsUseCustodianBroker && blotterOrder.Account == OrderFields.PROPERTY_MULTIPLE && blotterOrder.CounterPartyID == int.MinValue)
                    {
                        blotterOrder.CounterPartyName = OrderFields.PROPERTY_MULTIPLE;
                    }
                    //Update TraderName
                    if (orderResponse.TradingAccountID != Int32.MinValue)
                        blotterOrder.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(orderResponse.TradingAccountID);

                    if (orderResponse.ExpireTime != null && !(string.IsNullOrEmpty(orderResponse.ExpireTime)) && orderResponse.TIF == FIXConstants.TIF_GTD)
                    {
                        blotterOrder.ExpireTime = orderResponse.ExpireTime;
                    }
                    else if (orderResponse.TIF != FIXConstants.TIF_GTD)
                        blotterOrder.ExpireTime = "N/A";
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

        private void UpdateCancelRequests(OrderSingle blotterOrder, OrderSingle orderResponse)
        {
            try
            {
                if (orderResponse.ClOrderID != string.Empty)
                {
                    blotterOrder.InternalComments = orderResponse.InternalComments;
                    blotterOrder.TradeAttribute1 = orderResponse.TradeAttribute1;
                    blotterOrder.TradeAttribute2 = orderResponse.TradeAttribute2;
                    blotterOrder.TradeAttribute3 = orderResponse.TradeAttribute3;
                    blotterOrder.TradeAttribute4 = orderResponse.TradeAttribute4;
                    blotterOrder.TradeAttribute5 = orderResponse.TradeAttribute5;
                    blotterOrder.TradeAttribute6 = orderResponse.TradeAttribute6;
                    if (orderResponse.MsgType == FIXConstants.MSGOrderCancelRequest)
                    {
                        blotterOrder.ClOrderID = orderResponse.ClOrderID;
                    }

                    //In case we have recieved an execution report which had wrong Quantity Price etc.
                    //We should take care to revert them back.
                    blotterOrder.OrigClOrderID = orderResponse.OrigClOrderID;
                    blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue);
                    blotterOrder.OrderStatusTagValue = orderResponse.OrderStatusTagValue;

                    //we can't update quantities here as this has to be checked for staged sub
                    //blotterOrder.Quantity = orderResponse.Quantity;

                    //price should not be updated as cancel request does not contain price 
                    //blotterOrder.Price = orderResponse.Price;

                    if (blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.InternalOrder)
                    {
                        blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                    }

                    if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                    {
                        blotterOrder.ClOrderID = orderResponse.ClOrderID;
                        // the following code is to ensure that replace of staged order
                        // has correct Leaves and Unsent quantity 
                        switch (orderResponse.PranaMsgType)
                        {
                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged:
                                break;

                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub:
                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild:
                                // checks have to be put to check whether the child is replaced to a lower qty or higher
                                // then the working qty has to be accordingly adjusted
                                if (blotterOrder.Quantity <= orderResponse.Quantity)
                                {
                                    blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                                    //blotterOrder.UnsentQty = orderResponse.Quantity;
                                }
                                else
                                {
                                    blotterOrder.LeavesQty = blotterOrder.Quantity - blotterOrder.CumQty;
                                }
                                break;

                            case (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub:
                                blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
                                blotterOrder.UnsentQty = 0.0;
                                break;
                        }
                        blotterOrder.Quantity = orderResponse.Quantity;
                        blotterOrder.CommissionAmt = orderResponse.CommissionAmt;
                        blotterOrder.SoftCommissionAmt = orderResponse.SoftCommissionAmt;
                    }
                    else if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReject)
                    {
                        blotterOrder.Quantity = orderResponse.Quantity;
                        blotterOrder.LeavesQty = orderResponse.Quantity - orderResponse.CumQty;
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

        private void UpdateRolloverRequests(OrderSingle blotterOrder, OrderSingle orderResponse)
        {
            try
            {
                if (orderResponse.ClOrderID != string.Empty)
                {
                    blotterOrder.InternalComments = orderResponse.InternalComments;
                    blotterOrder.TradeAttribute1 = orderResponse.TradeAttribute1;
                    blotterOrder.TradeAttribute2 = orderResponse.TradeAttribute2;
                    blotterOrder.TradeAttribute3 = orderResponse.TradeAttribute3;
                    blotterOrder.TradeAttribute4 = orderResponse.TradeAttribute4;
                    blotterOrder.TradeAttribute5 = orderResponse.TradeAttribute5;
                    blotterOrder.TradeAttribute6 = orderResponse.TradeAttribute6;
                    if (orderResponse.MsgType == FIXConstants.MSGOrderRollOverRequest)
                    {
                        blotterOrder.ClOrderID = orderResponse.ClOrderID;
                    }

                    //In case we have recieved an execution report which had wrong Quantity Price etc.
                    //We should take care to revert them back.
                    blotterOrder.OrigClOrderID = orderResponse.OrigClOrderID;
                    blotterOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue);
                    blotterOrder.OrderStatusTagValue = orderResponse.OrderStatusTagValue;

                    //we can't update quantities here as this has to be checked for staged sub
                    //blotterOrder.Quantity = orderResponse.Quantity;

                    //price should not be updated as rollover request does not contain price 
                    //blotterOrder.Price = orderResponse.Price;

                    if (blotterOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.InternalOrder)
                    {
                        blotterOrder.LeavesQty = orderResponse.Quantity - blotterOrder.CumQty;
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

        private void ProcessNewOrder(OrderSingle orderResponse)
        {
            try
            {
                orderResponse.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderResponse.OrderSideTagValue.ToString());
                orderResponse.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(orderResponse.OrderTypeTagValue.ToString());
                orderResponse.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(orderResponse.OrderStatusTagValue.ToString());
                if (orderResponse.CMTAID != int.MinValue)
                {
                    orderResponse.CMTA = TagDatabaseManager.GetInstance.GetCMTAText(orderResponse.CMTAID.ToString());
                }
                if (orderResponse.GiveUpID != int.MinValue)
                {
                    orderResponse.GiveUp = TagDatabaseManager.GetInstance.GetGiveUpText(orderResponse.GiveUpID.ToString());
                }

                if (orderResponse.TradingAccountID != Int32.MinValue)
                {
                    orderResponse.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(orderResponse.TradingAccountID);
                }

                if (orderResponse.AssetID != int.MinValue)
                {

                    if (orderResponse.SwapParameters != null)
                    {
                        orderResponse.AssetName = "EquitySwap";

                    }
                    else
                    {
                        orderResponse.AssetName = CachedDataManager.GetInstance.GetAssetText(orderResponse.AssetID);
                    }
                }

                if (orderResponse.UnderlyingID != int.MinValue)
                {
                    orderResponse.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(orderResponse.UnderlyingID);
                }

                if (orderResponse.VenueID != int.MinValue)
                {
                    orderResponse.Venue = CachedDataManager.GetInstance.GetVenueText(orderResponse.VenueID);
                }

                orderResponse.Flag = CachedDataManager.GetInstance.GetFlagImage(orderResponse.AUECID);
                orderResponse.ExchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(orderResponse.AUECID);

                if (orderResponse.CompanyUserID != Int32.MinValue)
                {
                    orderResponse.CompanyUserName = CachedDataManager.GetInstance.GetUserText(orderResponse.CompanyUserID);
                }

                if (orderResponse.ActualCompanyUserID != Int32.MinValue)
                {
                    orderResponse.ActualCompanyUserName = CachedDataManager.GetInstance.GetUserText(orderResponse.ActualCompanyUserID);
                }

                //next four checks to avoid garbage rows in blotter
                if (orderResponse.LastPrice == double.MinValue)
                {
                    orderResponse.LastPrice = 0.0;
                }

                if (orderResponse.LastShares == double.MinValue)
                {
                    orderResponse.LastShares = 0.0;
                }

                if (orderResponse.StrikePrice == double.Epsilon)
                {
                    orderResponse.StrikePrice = 0.0;
                }

                if (orderResponse.StopPrice == double.Epsilon)
                {
                    orderResponse.StopPrice = 0.0;
                }

                if (orderResponse.PegDifference == double.Epsilon)
                {
                    orderResponse.PegDifference = 0.0;
                }

                if (orderResponse.DiscretionOffset == double.Epsilon)
                {
                    orderResponse.DiscretionOffset = 0.0;
                }

                if (orderResponse.DisplayQuantity == double.Epsilon)
                {
                    orderResponse.DisplayQuantity = 0.0;
                }

                if (orderResponse.CurrencyID != int.MinValue)
                {
                    orderResponse.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(orderResponse.CurrencyID);
                }

                if (orderResponse.Level1ID != int.MinValue && orderResponse.Account == "-")
                {
                    orderResponse.Account = CachedDataManager.GetInstance.GetAccount(orderResponse.Level1ID);
                }

                if (orderResponse.ShortRebate == double.Epsilon)
                {
                    orderResponse.ShortRebate = 0.0;
                }
                //The working Qty for the 
                // In case of staged orders we need not to change Leaves Qty.
                if (orderResponse.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                {
                    switch (orderResponse.OrderStatusTagValue)
                    {
                        case FIXConstants.ORDSTATUS_PendingNew:
                            orderResponse.LeavesQty = 0.0;
                            break;

                        case FIXConstants.ORDSTATUS_New:
                            orderResponse.LeavesQty = orderResponse.Quantity;
                            break;

                        case CustomFIXConstants.ORDSTATUS_AlgoPreviousPendingReplace:
                        case FIXConstants.ORDSTATUS_PartiallyFilled:
                        case FIXConstants.ORDSTATUS_Cancelled:
                        case FIXConstants.ORDSTATUS_PendingCancel:
                        case FIXConstants.ORDSTATUS_PendingReplace:
                        case FIXConstants.ORDSTATUS_RollOver:
                        case FIXConstants.ORDSTATUS_Rejected:
                            break;

                        case FIXConstants.ORDSTATUS_Filled:
                        //Kuldeep A.:- This is done considering only Day orders.
                        // If anytime in future, we need to handle Multiday orders then this handling should also be changed as in that case LeavesWty might change.
                        case FIXConstants.ORDSTATUS_DoneForDay:
                        //PRANA-40864
                        case FIXConstants.ORDSTATUS_Expired:
                            orderResponse.LeavesQty = 0.0;
                            break;

                        default:
                            orderResponse.LeavesQty = orderResponse.Quantity;
                            break;
                    }
                }
                if (orderResponse.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                {
                    if (!string.IsNullOrEmpty(orderResponse.MultiTradeId))
                    {
                        orderResponse.MultiTradeId = string.Empty;
                    }
                    UpdateStatusFromChildCollection(orderResponse);
                }
                else if ((orderResponse.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSub) ||
                    (orderResponse.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild) ||
                    (orderResponse.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDManualSub))
                {
                    lock (_lockOnParentClOrderIDCollection)
                    {
                        if (dictParentClOrderIDCollection.ContainsKey(orderResponse.StagedOrderID))
                        {
                            OrderSingle parentOrder = dictParentClOrderIDCollection[orderResponse.StagedOrderID];
                            UpdateStatusFromChildCollection(parentOrder);
                        }
                    }
                }
                if ((orderResponse.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged) &&
                   OrderInformation.IsMultiDayOrderHistory(orderResponse))
                {
                    UpdateStatusFromChildCollection(orderResponse);
                }
                if (orderResponse.Venue == "Algo")
                {
                    orderResponse.AlgoStrategyName = AlgoStrategyNamesDetails.GetAlgoStrategyText(orderResponse.AlgoStrategyID, orderResponse.CounterPartyID);
                    if (orderResponse.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew)
                    {
                        orderResponse.ParentClOrderID = orderResponse.ClOrderID;
                        orderResponse.Text = "Awaiting Original Order Cancel";
                        if (AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary.ContainsKey(orderResponse.AlgoSyntheticRPLParent))
                        {
                            double cumQty = AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary[orderResponse.AlgoSyntheticRPLParent].CumQty;
                            AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary.Remove(orderResponse.AlgoSyntheticRPLParent);
                            OrderSingle algoRplOrder = (OrderSingle)orderResponse.Clone();
                            algoRplOrder.CumQty = cumQty;

                            AlgoReplaceManager.GetInstance().AlgoReplaceOrdersDictionary.Add(orderResponse.AlgoSyntheticRPLParent, algoRplOrder);
                        }
                    }
                }

                if (orderResponse.AssetID == (int)AssetCategory.FX || orderResponse.AssetID == (int)AssetCategory.FXForward)
                {
                    int currencyId = orderResponse.SettlementCurrencyID == orderResponse.LeadCurrencyID ? orderResponse.VsCurrencyID : orderResponse.LeadCurrencyID;
                    orderResponse.CounterCurrency = CachedDataManager.GetInstance.GetCurrencyText(currencyId);
                }

                if (orderResponse.CounterPartyID != int.MinValue)
                {
                    orderResponse.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(orderResponse.CounterPartyID);
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

        #region Update Parent Methods
        private void UpdateParentStatus(OrderSingle orderResponse, OrderSingle parentOrder)
        {
            try
            {
                //In case of Multi-day sub orders the execution time of last fill should be updated
                if (orderResponse.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDNewSubChild)
                    parentOrder.ExecutionTimeLastFill = orderResponse.ExecutionTimeLastFill;

                if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingCancel)
                {
                    bool isCancelleable = true;
                    if (parentOrder.OrderCollection != null)
                    {
                        foreach (OrderSingle subOrder in parentOrder.OrderCollection)
                        {
                            if (subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Filled &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Expired &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Cancelled &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_RollOver &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Stopped &&
                                subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected &&
                                subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_Aborted &&
                                subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected)
                            {
                                isCancelleable = false;
                                break;
                            }
                        }
                    }
                    if (isCancelleable)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Cancelled;
                        parentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(parentOrder.OrderStatusTagValue.ToString());
                        parentOrder.OrderSeqNumber = Int64.MinValue;
                        SendExecutionReport(parentOrder);
                    }
                    else
                    {
                        if (orderResponse.MsgType == FIXConstants.MSGOrderCancelReject)
                        {
                            if (parentOrder.CumQty == 0.0)
                            {
                                parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                            }
                            else if (parentOrder.CumQty > 0.0 && parentOrder.CumQty < parentOrder.Quantity)
                            {
                                parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                            }
                            else if (parentOrder.CumQty == parentOrder.Quantity)
                            {
                                parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                            }
                        }
                    }
                }
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New || parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                {
                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                    }
                    else if (parentOrder.CumQty == parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                    }
                }
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PartiallyFilled)
                {
                    if (parentOrder.CumQty == 0)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                    }
                    if (parentOrder.CumQty == parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                    }

                }
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced)
                {
                    //Changed by: Bharat Raturi
                    //Parent order status must be updated if order is replaced 
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6708

                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity
                        && orderResponse.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged
                        && orderResponse.OrderStatusTagValue != FIXConstants.ORDSTATUS_Replaced && orderResponse.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingReplace && orderResponse.OrderSeqNumber != parentOrder.OrderSeqNumber)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;

                    }
                    // case: parent is replaced to qty = cumqty.
                    if (parentOrder.Quantity == parentOrder.CumQty && parentOrder.LeavesQty == 0)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                    }
                }

                // Added the condition to update orderside tag value, when order status is filled
                else if (parentOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled)
                {
                    if (parentOrder.CumQty > 0 && parentOrder.CumQty < parentOrder.Quantity)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;
                    }
                    else if (parentOrder.CumQty == 0)
                    {
                        parentOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                    }
                }

                // get current status text to show in blotter
                parentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(parentOrder.OrderStatusTagValue.ToString());
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

        private void SendExecutionReport(OrderSingle stageOrderCancel)
        {
            try
            {
                OrderSingle stageOrderCancelResponse = (OrderSingle)stageOrderCancel.Clone();
                stageOrderCancelResponse.MsgType = FIXConstants.MSGExecutionReport;
                stageOrderCancelResponse.OrderStatusTagValue = FIXConstants.ORDSTATUS_Cancelled;
                stageOrderCancelResponse.ExecType = FIXConstants.ORDSTATUS_Cancelled;
                stageOrderCancelResponse.ExecID = System.Guid.NewGuid().ToString();
                stageOrderCancelResponse.LastShares = 0;
                stageOrderCancelResponse.LastPrice = 0;
                stageOrderCancelResponse.LeavesQty = 0;
                stageOrderCancelResponse.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                stageOrderCancelResponse.TransactionTime = DateTime.Now.ToUniversalTime();
                if (SendBlotterTradesEventHandler != null)
                    SendBlotterTradesEventHandler(this, new EventArgs<OrderSingle>(stageOrderCancelResponse));
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

        #region TransferUser Methods
        private void UpdateTransferUserDetails(OrderSingle blotterOrder, OrderSingle orderResponse)
        {
            try
            {
                if (orderResponse.CompanyUserID != Int32.MinValue)
                {
                    blotterOrder.CompanyUserID = orderResponse.CompanyUserID;
                    blotterOrder.CompanyUserName = CachedDataManager.GetInstance.GetUserText(orderResponse.CompanyUserID);
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
        #endregion

        public void UpdateDictionarybyDBOrders(OrderBindingList incomingOrders)
        {
            try
            {
                if (incomingOrders.Count > 0)
                {
                    lock (_lockOnWorkingSubsTabCollection)
                        workingSubsTabCollection.Clear();

                    lock (_lockOnOrdersTabCollection)
                    {
                        ordersTabCollection.Clear();
                        if (ordersTabCollection.Count != 0)
                            ordersTabCollection.Clear();
                    }

                    lock (_lockOnParentClOrderIDCollection)
                    {
                        dictParentClOrderIDCollection.Clear();
                        foreach (OrderSingle incomingOrder in incomingOrders)
                        {
                            if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                            {
                                OrderSingle existingOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];

                                if (existingOrder.OrderSeqNumber > incomingOrder.OrderSeqNumber && incomingOrder.OrderSeqNumber != long.MinValue || ((OrderInformation.IsMultiDayOrderHistory(existingOrder)) && incomingOrder.MsgType == FIXConstants.MSGOrderCancelReject))
                                {
                                    continue;
                                }
                                else
                                {
                                    if (incomingOrder.VenueID != int.MinValue)
                                    {
                                        incomingOrder.Venue = CachedDataManager.GetInstance.GetVenueText(incomingOrder.VenueID);
                                    }
                                    if (incomingOrder.CounterPartyID != int.MinValue)
                                    {
                                        incomingOrder.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(incomingOrder.CounterPartyID);
                                    }
                                    UpdateOrderDetails(existingOrder, incomingOrder);
                                    //In case of Parent Sub orders of GTC/GTD, re-calcualte Quantity from the child orders for Replace/Cancel scenarios
                                    if (incomingOrder.PranaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged && OrderInformation.IsMultiDayOrderHistory(existingOrder))
                                    {
                                        UpdateStatusFromChildCollection(existingOrder);
                                        existingOrder.PropertyHasChanged();
                                    }
                                    //Special handling for staged orders...
                                    if (incomingOrder.StagedOrderID != string.Empty && incomingOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                    {

                                        if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.StagedOrderID))
                                        {
                                            OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.StagedOrderID];
                                            double parentOrderPrevQty = parentOrder.CumQty;
                                            UpdateStatusFromChildCollection(parentOrder);
                                            UpdateParentStatus(incomingOrder, parentOrder);

                                            if (parentOrder.StagedOrderID != string.Empty && parentOrder.StagedOrderID != int.MinValue.ToString() && dictParentClOrderIDCollection.ContainsKey(parentOrder.StagedOrderID))
                                            {
                                                OrderSingle grandParentOrder = dictParentClOrderIDCollection[parentOrder.StagedOrderID];
                                                grandParentOrder.LastPrice = incomingOrder.LastPrice;
                                                grandParentOrder.LastShares = incomingOrder.LastShares;
                                                UpdateParentStatusFromSub(parentOrder, grandParentOrder, parentOrderPrevQty, incomingOrder.AvgPrice);
                                            }
                                            parentOrder.LastPrice = incomingOrder.LastPrice;
                                            parentOrder.LastShares = incomingOrder.LastShares;
                                            parentOrder.PropertyHasChanged();
                                        }
                                    }
                                    else if (incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                                    {
                                        if (dictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                                        {
                                            OrderSingle parentOrder = dictParentClOrderIDCollection[incomingOrder.ParentClOrderID];
                                            UpdateStatusFromChildCollection(parentOrder);
                                            UpdateParentStatus(incomingOrder, parentOrder);
                                            parentOrder.LastPrice = incomingOrder.LastPrice;
                                            parentOrder.FXRate = incomingOrder.FXRate;
                                            parentOrder.LastShares = incomingOrder.LastShares;
                                            parentOrder.PropertyHasChanged();
                                        }
                                    }
                                    existingOrder.PropertyHasChanged();
                                }
                            }
                            else
                            {
                                // new order hence add in the dictionary
                                ProcessNewOrder(incomingOrder);
                                if (!OrderInformation.IsMultiDayOrderHistory(incomingOrder) && incomingOrder.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged)
                                {
                                    incomingOrder.DayCumQty = incomingOrder.CumQty;
                                    incomingOrder.DayAvgPx = incomingOrder.AvgPrice;
                                    incomingOrder.DayOrderQty = incomingOrder.Quantity;
                                }
                                dictParentClOrderIDCollection.Add(incomingOrder.ParentClOrderID, incomingOrder);
                                AddToOrderCollection(incomingOrder);
                                UpdateStagedOrderForMultidayWithMultiBroker(incomingOrder);
                                incomingOrder.PropertyHasChanged();
                            }
                        }
                    }
                    AddPendingApprovalOrdersInCache();
                }
                RemoveMultiDayEndStateOrders();
                RemoveFilledAndDropCopyStagedOrders(incomingOrders);
                RemoveMultiBrokerSubFromAllOrderCollection();
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
        /// For orders that are both multiday and multibroker would need to update Staged order as great grand parent
        /// </summary>
        /// <param name="incomingOrder"></param>
        private void UpdateStagedOrderForMultidayWithMultiBroker(OrderSingle incomingOrder)
        {
            try
            {
                if (incomingOrder == null || !OrderInformation.IsMultiDayOrderHistory(incomingOrder))
                    return;

                if (!dictParentClOrderIDCollection.TryGetValue(incomingOrder.ParentClOrderID, out var brokerChildOrder))
                    return;

                if (!dictParentClOrderIDCollection.TryGetValue(brokerChildOrder.StagedOrderID, out var dayChildOrder))
                    return;

                if (!dictParentClOrderIDCollection.TryGetValue(dayChildOrder.StagedOrderID, out var subOrder))
                    return;

                if (!subOrder.IsMultiBrokerTrade)
                {
                    return;
                }

                UpdateStatusFromChildCollection(subOrder);
                UpdateParentStatus(dayChildOrder, subOrder);
                subOrder.PropertyHasChanged();

                if (!subOrder.IsMultiBrokerTrade || !dictParentClOrderIDCollection.TryGetValue(subOrder.StagedOrderID, out var stagedOrder))
                    return;

                UpdateStatusFromChildCollection(stagedOrder);
                UpdateParentStatus(subOrder, stagedOrder);
                //UpdateParentStatusFromSub(subOrder, stagedOrder, stagedOrder.CumQty, incomingOrder.AvgPrice);
                stagedOrder.LastPrice = incomingOrder.LastPrice;
                stagedOrder.LastShares = incomingOrder.LastShares;
                stagedOrder.PropertyHasChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void RemoveMultiBrokerSubFromAllOrderCollection()
        {
            try
            {
                List<string> listStagedOrderIDs = new List<string>();
                listStagedOrderIDs = workingSubsTabCollection.Select(x => x.StagedOrderID).Distinct().ToList();

                for (int counter = 0; counter < workingSubsTabCollection.Count; counter++)
                {
                    if (!listStagedOrderIDs.Contains(workingSubsTabCollection[counter].StagedOrderID))
                    {
                        workingSubsTabCollection.Remove(workingSubsTabCollection[counter]);
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
        /// This method gets the order details from cache for orders of allocation details
        /// </summary>
        public Dictionary<string, OrderSingle> getSelectedOrderDetails(List<AllocationDetails> allocationDetails)
        {
            Dictionary<string, OrderSingle> selectedOrders = new Dictionary<string, OrderSingle>();
            try
            {
                allocationDetails.ForEach(details =>
                {
                    if (dictParentClOrderIDCollection.ContainsKey(details.ClOrderID))
                    {
                        OrderSingle existingOrder = dictParentClOrderIDCollection[details.ClOrderID];
                        if (!selectedOrders.ContainsKey(details.ClOrderID))
                        {
                            selectedOrders.Add(details.ClOrderID, existingOrder);
                        }
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
            return selectedOrders;
        }

        /// <summary>
        /// Remove MultiDay End StateOrders from Workign SubGrid
        /// </summary>
        private void RemoveMultiDayEndStateOrders()
        {
            try
            {
                Dictionary<int, DateTime> clearanceTimeCache = new Dictionary<int, DateTime>();
                List<OrderSingle> listMultiDayOrdersToRemove = new List<OrderSingle>();
                foreach (OrderSingle incomingOrder in workingSubsTabCollection)
                {
                    bool isMultiDayEndState = OrderInformation.IsMultiDayOrderInEndState(incomingOrder);
                    if (isMultiDayEndState)
                    {
                        DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));

                        DateTime clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                        if (clearanceTimeCache.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = clearanceTimeCache[incomingOrder.AUECID];
                        }
                        else if (BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[incomingOrder.AUECID], CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                            clearanceTimeCache.Add(incomingOrder.AUECID, clearanceTime);
                        }

                        bool isWorkingSubOrderCanBeRemoved = false;

                        if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                        {
                            clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                        }
                        else if (_isOnlyAUECTimeBasedBlotterClearance)
                        {
                            clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                        }
                        else
                        {
                            clearanceTime = currentAUECDate.Date;
                        }
                        DateTime lastActivityTimeOnThisOrder = OrderInformation.GetLastActivityTime(incomingOrder);
                        if (lastActivityTimeOnThisOrder <= clearanceTime)
                        {
                            isWorkingSubOrderCanBeRemoved = true;

                            if (incomingOrder.OrderCollection != null && incomingOrder.OrderCollection.Any(d => d.AUECLocalDate > clearanceTime))
                            {
                                isWorkingSubOrderCanBeRemoved = false;
                            }
                        }

                        if (isWorkingSubOrderCanBeRemoved)
                        {
                            listMultiDayOrdersToRemove.Add(incomingOrder);
                        }
                    }
                }
                foreach (OrderSingle endStateWorkingSubToRemove in listMultiDayOrdersToRemove)
                {

                    IndexEventArgs arg;
                    lock (_lockOnWorkingSubsTabCollection)
                    {
                        arg = new IndexEventArgs(workingSubsTabCollection.IndexOf(endStateWorkingSubToRemove));
                    }
                    lock (_lockOnWorkingSubsTabCollection)
                    {
                        workingSubsTabCollection.Remove(endStateWorkingSubToRemove);
                    }

                    if (WorkingSubCollectionIndexChanged != null && arg.Index != -1)
                    {
                        WorkingSubCollectionIndexChanged(this, arg);
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
        private void RemoveFilledAndDropCopyStagedOrders(OrderBindingList incomingOrders)
        {
            try
            {

                OrderSingle[] incomingOrdersCopy = new OrderSingle[incomingOrders.Count];
                incomingOrders.CopyTo(incomingOrdersCopy, 0);
                Dictionary<int, DateTime> clearanceTimeCache = new Dictionary<int, DateTime>();

                foreach (OrderSingle incomingOrder in incomingOrdersCopy)
                {
                    //bool isDropcopyStage =  incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged
                    //                    &&  incomingOrder.TransactionSourceTag == (int)TransactionSource.FIX 
                    //                    && incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New && 
                    //                    !(incomingOrder.OrderCollection != null && incomingOrder.OrderCollection.Count > 0);
                    bool isFilledStage = (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled || incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled)
                                        && incomingOrder.PranaMsgType == (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged;

                    //if (isDropcopyStage || isFilledStage)
                    if (isFilledStage)
                    {
                        DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));

                        DateTime clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                        if (clearanceTimeCache.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = clearanceTimeCache[incomingOrder.AUECID];
                        }
                        else if (BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(incomingOrder.AUECID))
                        {
                            clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[incomingOrder.AUECID], CachedDataManager.GetInstance.GetAUECTimeZone(incomingOrder.AUECID));
                            clearanceTimeCache.Add(incomingOrder.AUECID, clearanceTime);
                        }

                        bool isStagedOrderCanBeRemoved = false;

                        if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                        {
                            clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                        }
                        else if (_isOnlyAUECTimeBasedBlotterClearance)
                        {
                            clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                        }
                        else
                        {
                            clearanceTime = currentAUECDate.Date;
                        }

                        if (incomingOrder.AUECLocalDate <= clearanceTime)
                        {
                            isStagedOrderCanBeRemoved = true;

                            if (incomingOrder.OrderCollection != null && incomingOrder.OrderCollection.Any(d => d.AUECLocalDate > clearanceTime || ((OrderInformation.IsMultiDayOrder(d) && workingSubsTabCollection.Contains(d))))) //TODO:MultiDay History check
                            {
                                isStagedOrderCanBeRemoved = false;
                            }
                        }

                        if (isStagedOrderCanBeRemoved)
                        {
                            IndexEventArgs argStaged;
                            lock (_lockOnOrdersTabCollection)
                            {
                                argStaged = new IndexEventArgs(ordersTabCollection.IndexOf(incomingOrder));
                            }
                            lock (_lockOnOrdersTabCollection)
                            {
                                ordersTabCollection.Remove(incomingOrder);
                            }
                            if (argStaged.Index != -1)
                            {
                                if (OrderCollectionIndexChanged != null && argStaged.Index != -1)
                                {
                                    OrderCollectionIndexChanged(this, argStaged);
                                }
                            }
                            lock (_lockOnParentClOrderIDCollection)
                            {
                                if (DictParentClOrderIDCollection.ContainsKey(incomingOrder.ParentClOrderID))
                                {
                                    DictParentClOrderIDCollection.Remove(incomingOrder.ParentClOrderID);
                                }
                                if (incomingOrder.OrderCollection != null)
                                {
                                    foreach (OrderSingle SubOrder in incomingOrder.OrderCollection)
                                    {
                                        if (DictParentClOrderIDCollection.ContainsKey(SubOrder.ParentClOrderID))
                                        {
                                            DictParentClOrderIDCollection.Remove(SubOrder.ParentClOrderID);
                                        }
                                        IndexEventArgs arg;
                                        lock (_lockOnWorkingSubsTabCollection)
                                        {
                                            arg = new IndexEventArgs(workingSubsTabCollection.IndexOf(SubOrder));
                                        }
                                        lock (_lockOnWorkingSubsTabCollection)
                                        {
                                            workingSubsTabCollection.Remove(SubOrder);
                                        }

                                        if (WorkingSubCollectionIndexChanged != null && arg.Index != -1)
                                        {
                                            WorkingSubCollectionIndexChanged(this, arg);
                                        }
                                    }
                                }
                            }
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
        }

        private Dictionary<string, OrderSingle> RemoveFromCollectionAndUpdateGrid(OrderSingle orderToBeRemoved, IndexEventArgs arg, IndexEventArgs argOrder, bool isRemoveSubOrder = false)
        {
            Dictionary<string, OrderSingle> updatedStageOrders = new Dictionary<string, OrderSingle>();
            try
            {
                lock (_lockOnWorkingSubsTabCollection)
                {
                    workingSubsTabCollection.Remove(orderToBeRemoved);
                }
                lock (_lockOnOrdersTabCollection)
                {
                    ordersTabCollection.Remove(orderToBeRemoved);
                    if (isRemoveSubOrder)
                    {
                        foreach (OrderSingle stageOrder in ordersTabCollection)
                        {
                            if (stageOrder != null && stageOrder.ParentClOrderID.Equals(orderToBeRemoved.StagedOrderID))
                            {
                                OrderSingle subOrder = stageOrder.OrderCollection.FirstOrDefault(x => x.ClOrderID.Equals(orderToBeRemoved.ClOrderID));
                                if (subOrder != null)
                                {
                                    stageOrder.OrderCollection.Remove(subOrder);
                                    BlotterOrderCollections.GetInstance().UpdateStatusFromChildCollection(stageOrder);
                                    decimal qty = Convert.ToDecimal(stageOrder.Quantity);
                                    decimal cumQty = Convert.ToDecimal(stageOrder.CumQty);
                                    if (qty == cumQty)
                                        stageOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_Filled;
                                    else if (cumQty == 0.0m)
                                        stageOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_New;
                                    else
                                        stageOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PartiallyFilled;

                                    stageOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(stageOrder.OrderStatusTagValue);

                                    if (!updatedStageOrders.ContainsKey(stageOrder.ParentClOrderID))
                                        updatedStageOrders.Add(stageOrder.ParentClOrderID, stageOrder);
                                    else
                                        updatedStageOrders[stageOrder.ParentClOrderID] = stageOrder;
                                }
                            }
                        }
                    }
                }

                if (WorkingSubCollectionIndexChanged != null && arg.Index != -1)
                    WorkingSubCollectionIndexChanged(this, arg);

                if (argOrder.Index != -1)
                {
                    if (OrderCollectionIndexChanged != null && argOrder.Index != -1)
                        OrderCollectionIndexChanged(this, argOrder);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updatedStageOrders;
        }

        private void ProcessNewFIXAlgoOrder(OrderSingle existingOrder, OrderSingle incomingOrder)
        {
            try
            {
                existingOrder.Quantity = incomingOrder.Quantity;
                existingOrder.LeavesQty = 0.0;

                existingOrder.OrderStatusTagValue = incomingOrder.OrderStatusTagValue;
                existingOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(existingOrder.OrderStatusTagValue.ToString());
                existingOrder.Text = "New";
                if (existingOrder.DiscretionOffset == double.MinValue)
                {
                    existingOrder.DiscretionOffset = 0.0;
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

        string _parentClOrderID = string.Empty;
        public List<OrderSingle> PrepareMultiBrokerOrdersList(string parentclOrderID)
        {
            try
            {
                _parentClOrderID = parentclOrderID;
                listMultiBrokerOrders.Clear();
                foreach (KeyValuePair<string, OrderSingle> kvp in dictParentClOrderIDCollection)
                {
                    if (kvp.Value.StagedOrderID.Equals(parentclOrderID))
                    {
                        if (!listMultiBrokerOrders.Contains(kvp.Value))
                        {
                            listMultiBrokerOrders.Add(kvp.Value);
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
            return listMultiBrokerOrders;
        }

        public void UpdateMultiBrokerOrderList(OrderSingle order)
        {
            try
            {
                if (listMultiBrokerOrders != null)
                {
                    if (_parentClOrderID == order.StagedOrderID)
                    {
                        AddOrUpdateToList(order, listMultiBrokerOrders);
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

        private void AddOrUpdateToList(OrderSingle order, List<OrderSingle> listMultiBrokerOrders)
        {
            try
            {
                for (int counter = 0; counter < listMultiBrokerOrders.Count; counter++)
                {
                    if (listMultiBrokerOrders[counter].ClOrderID == order.ClOrderID)
                    {
                        listMultiBrokerOrders.Remove(listMultiBrokerOrders[counter]);
                        listMultiBrokerOrders.Insert(counter, order);
                        return;
                    }
                }
                listMultiBrokerOrders.Add(order);
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

        public Dictionary<string, Dictionary<string, OrderSingle>> HideOrderFromBlotterGrid(string commaSaperateParentClOrderId, bool isRemoveSubOrder = false)
        {
            Dictionary<string, Dictionary<string, OrderSingle>> finalResult = new Dictionary<string, Dictionary<string, OrderSingle>>();
            Dictionary<string, OrderSingle> dictOrdersRemoved = new Dictionary<string, OrderSingle>();
            Dictionary<string, OrderSingle> updatedStageOrders = new Dictionary<string, OrderSingle>();
            try
            {
                lock (_lockOnParentClOrderIDCollection)
                {
                    foreach (string key in commaSaperateParentClOrderId.Split(','))
                    {
                        if (DictParentClOrderIDCollection.ContainsKey(key))
                        {
                            OrderSingle orderToBeRemoved = DictParentClOrderIDCollection[key];

                            IndexEventArgs arg;
                            lock (_lockOnWorkingSubsTabCollection)
                            {
                                arg = new IndexEventArgs(workingSubsTabCollection.IndexOf(orderToBeRemoved));
                            }
                            IndexEventArgs argOrder;
                            lock (_lockOnOrdersTabCollection)
                            {
                                argOrder = new IndexEventArgs(ordersTabCollection.IndexOf(orderToBeRemoved));
                            }

                            DictParentClOrderIDCollection.Remove(key);
                            Dictionary<string, OrderSingle> resultStageOrders = RemoveFromCollectionAndUpdateGrid(orderToBeRemoved, arg, argOrder, isRemoveSubOrder);
                            foreach (KeyValuePair<string, OrderSingle> kvp in resultStageOrders)
                            {
                                if (updatedStageOrders.ContainsKey(kvp.Key))
                                    updatedStageOrders[kvp.Key] = kvp.Value;
                                else
                                    updatedStageOrders.Add(kvp.Key, kvp.Value);
                            }
                            dictOrdersRemoved.Add(key, orderToBeRemoved);
                        }
                    }
                }
                finalResult.Add(BlotterDataConstants.CONST_OrderTab, updatedStageOrders);
                finalResult.Add(BlotterDataConstants.CONST_RemovedOrders, dictOrdersRemoved);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return finalResult;
        }

        /// <summary>
        /// Used for staged orders parent update from Child updates
        /// </summary>
        string previousBroker = string.Empty;
        public void UpdateStatusFromChildCollection(OrderSingle orderSingle)
        {
            double cumQty = 0;
            double leavesQty = 0;
            double amount = 0;
            double avgPrice = 0;
            double dayExecutedQty = 0;
            double dayAveragePrice = 0;
            double beforeStartOfDayExecuted = 0;
            string orderStatus = orderSingle.OrderStatus;
            long seqNum = 0;

            if (orderSingle.OrderCollection != null)
            {
                previousBroker = string.Empty;
                foreach (OrderSingle subOrder in orderSingle.OrderCollection)
                {
                    if (!orderSingle.IsUseCustodianBroker && !orderSingle.IsMultiBrokerTrade)
                    {
                        if (previousBroker == string.Empty)
                        {
                            previousBroker = subOrder.CounterPartyName;
                            orderSingle.CounterPartyName = subOrder.CounterPartyName;
                        }
                        else if (previousBroker != subOrder.CounterPartyName)
                            orderSingle.CounterPartyName = "Multiple";
                    }
                    if (subOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                    {
                        leavesQty += subOrder.Quantity;
                        if (OrderInformation.IsMultiDayOrderHistory(orderSingle))
                        {
                            cumQty += subOrder.CumQty;
                            dayExecutedQty += subOrder.CumQty;
                        }
                    }
                    //Changed by: Bharat Raturi, 24 Mar 2015
                    //Status of the parent trade should not be updated if the order has been rejected
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6738
                    else if (subOrder.OrderStatusTagValue != CustomFIXConstants.ORDSTATUS_AlgoPreviousCancelRejected && subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                    {
                        cumQty += subOrder.CumQty;
                        leavesQty += subOrder.LeavesQty;

                        //Updating Day Avg Price and Day Executed Price column value for current date
                        DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(subOrder.AUECID));
                        if (subOrder.OrderStatusTagValue != FIXConstants.ORDSTATUS_RollOver && BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(subOrder.AUECID))
                        {
                            DateTime clearanceTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance[subOrder.AUECID], CachedDataManager.GetInstance.GetAUECTimeZone(subOrder.AUECID));

                            if (clearanceTime.TimeOfDay < currentAUECDate.TimeOfDay)
                            {
                                clearanceTime = currentAUECDate.Date + clearanceTime.TimeOfDay;
                            }
                            else if (_isOnlyAUECTimeBasedBlotterClearance)
                            {
                                clearanceTime = currentAUECDate.Date.AddDays(-1) + clearanceTime.TimeOfDay;
                            }
                            else
                            {
                                clearanceTime = currentAUECDate.Date;
                            }
                            if (subOrder.AUECLocalDate > clearanceTime)
                            {
                                if (dayExecutedQty + subOrder.CumQty != 0)
                                    dayAveragePrice = ((dayExecutedQty * dayAveragePrice) + (subOrder.AvgPrice * subOrder.CumQty)) / (dayExecutedQty + subOrder.CumQty);
                                dayExecutedQty += subOrder.CumQty;
                            }
                            else
                            {
                                beforeStartOfDayExecuted += subOrder.CumQty;
                            }
                        }
                        else
                        {
                            beforeStartOfDayExecuted += subOrder.CumQty;
                        }

                        avgPrice = subOrder.AvgPrice;
                        amount += avgPrice * subOrder.CumQty;
                        orderStatus = subOrder.OrderStatus;
                        seqNum = subOrder.OrderSeqNumber;
                    }
                }
                //Special Handling for Leaves Quanity in case of GTC/GTD orders
                if ((orderSingle.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged) &&
                      OrderInformation.IsMultiDayOrderHistory(orderSingle))
                {
                    leavesQty = orderSingle.Quantity - cumQty;
                    if (OrderInformation.IsOrderInEndState(orderSingle))
                        leavesQty = 0.0;
                }
                orderSingle.UnsentQty = orderSingle.Quantity - cumQty - leavesQty;
                orderSingle.LeavesQty = leavesQty;
                orderSingle.CumQty = cumQty;
                orderSingle.DayCumQty = dayExecutedQty;
                orderSingle.DayAvgPx = dayAveragePrice;
                orderSingle.DayOrderQty = orderSingle.Quantity - beforeStartOfDayExecuted;
                if (orderSingle.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced && orderSingle.OrderSeqNumber < seqNum)
                    orderSingle.OrderStatus = orderStatus;

                if (cumQty > 0)
                {
                    orderSingle.AvgPrice = amount / cumQty;
                    ((PranaBasicMessage)(orderSingle)).AvgPrice = amount / cumQty;
                }
                else
                {
                    orderSingle.AvgPrice = 0.0;
                    ((PranaBasicMessage)(orderSingle)).AvgPrice = 0.0;
                }
            }
            else
            {
                //https://dev.azure.com/NirvanaSolutions/Enterprise/_workitems/edit/14456
                if ((orderSingle.PranaMsgType != (int)Prana.Global.OrderFields.PranaMsgTypes.ORDStaged) &&
                     OrderInformation.IsMultiDayOrderHistory(orderSingle))
                {
                    orderSingle.DayOrderQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                    orderSingle.LeavesQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                    orderSingle.UnsentQty = 0;

                    if (OrderInformation.IsOrderInEndState(orderSingle))
                        orderSingle.LeavesQty = 0;
                }
                else
                {
                    orderSingle.UnsentQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                    orderSingle.LeavesQty = 0;
                    orderSingle.DayOrderQty = ((PranaBasicMessage)(orderSingle)).Quantity;
                }
            }
        }
    }
}