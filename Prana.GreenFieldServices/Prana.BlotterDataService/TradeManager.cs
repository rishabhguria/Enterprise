using Newtonsoft.Json;
using Prana.Admin.BLL;
using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.BlotterDataService.DTO;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.BlotterDataService;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.BusinessObjects.Classes.BusinessBaseClass;
using Prana.BusinessObjects.Classes.Utilities;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.TradeManager.Extension.CacheStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Prana.BlotterDataService
{
    public class TradeManager: AdditionalTradeAttributes
    {
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lockOnParentClOrderIDCache = BlotterOrderCollections.GetInstance()._lockOnParentClOrderIDCollection;

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static TradeManager _tradeManager = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static TradeManager GetInstance()
        {
            if (_tradeManager == null)
                _tradeManager = new TradeManager();
            return _tradeManager;
        }
        #endregion

        /// <summary>
        /// _companyID
        /// </summary>
        private int _companyID;
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        /// <summary>
        /// User wise TradingTicketUIPrefs
        /// </summary>
        private Dictionary<int, TradingTicketUIPrefs> _userwiseTradingTicketUIPrefs = new Dictionary<int, TradingTicketUIPrefs>();
        public Dictionary<int, TradingTicketUIPrefs> UserwiseTradingTicketUIPrefs
        {
            get { return _userwiseTradingTicketUIPrefs; }
        }

        /// <summary>
        /// User wise Permitted accounts
        /// </summary>
        private Dictionary<int, Dictionary<int, string>> _userPermittedAccounts = new Dictionary<int, Dictionary<int, string>>();
        public Dictionary<int, Dictionary<int, string>> UserPermittedAccounts
        {
            get { return _userPermittedAccounts; }
        }

        /// <summary>
        /// Stores error message for cancel all sub(s) operation.
        /// </summary>
        private string errorMsgForCancelAllSubs = string.Empty;

        /// <summary>
        /// Stores error message for remove order(s) operation.
        /// </summary>
        private string errorMsgForRemoveOrders = string.Empty;

        /// <summary>
        /// Stores error message for remove manual execution operation.
        /// </summary>
        private string errorMsgForRemoveManualExec = string.Empty;

        /// <summary>
        /// Stores error message for add/modify fills operation.
        /// </summary>
        private string errorMsgForAddModifyFills = string.Empty;

        /// <summary>
        /// Stores error message for allocation details fetch operation.
        /// </summary>
        private string errorMsgForAllocationDetails = string.Empty;

        /// <summary>
        /// Stores error message for save allocation details operation.
        /// </summary>
        private string errorMsgForSaveAllocationDetails = string.Empty;

        /// <summary>
        /// Stores error message for rollover all sub(s) operation.
        /// </summary>
        private string errorMsgForRolloverAllSubs = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        private TradeManager()
        {
            TradeManagerExtension.GetInstance().FixBrokerDownEventHandler += ShowFixBrokerDownMessageEvent;
        }

        /// <summary>
        /// Gets existing trades from Database for CompanyUserId.
        /// </summary>
        /// <param name="companyUserId"></param>
        public BlotterResponse GetTradesFromDatabase(int companyUserId)
        {
            BlotterResponse blotterResponse = null;
            List<BlotterOrder> orderTabData = new List<BlotterOrder>();
            List<BlotterOrder> workingTabData = new List<BlotterOrder>();

            try
            {
                Logger.LogMsg(LoggerLevel.Information, "GetTradesFromDatabase is invoked.");
                // Fetched data from DB
                OrderBindingList oldOrders = DBTradeManager.GetInstance().GetBlotterLaunchData(companyUserId);
                if (oldOrders != null && oldOrders.Count > 0)
                {
                    // Updated Cache
                    BlotterOrderCollections.GetInstance().UpdateDictionarybyDBOrders(oldOrders);
                    // Filled Order Tab data
                    OrderBindingList orderBlotterCollection = BlotterCommonCache.GetInstance().OrderBlotterCollection;
                    foreach (OrderSingle orderSingle in orderBlotterCollection)
                    {
                        BlotterOrder blotterOrder = new BlotterOrder();
                        blotterOrder.UpdateDataFromOrderSingle(orderSingle);
                        List<BlotterOrder> listOfSubOrders = new List<BlotterOrder>();
                        if (orderSingle != null && orderSingle.OrderCollection != null && orderSingle.OrderCollection.Count > 0)
                        {
                            foreach (OrderSingle subOrder in orderSingle.OrderCollection)
                            {
                                BlotterOrder subOrderBlotter = new BlotterOrder();
                                subOrderBlotter.UpdateDataFromOrderSingle(subOrder, true);
                                listOfSubOrders.Add(subOrderBlotter);
                            }
                        }
                        blotterOrder.OrderCollection = listOfSubOrders;
                        orderTabData.Add(blotterOrder);
                    }
                    // Filled Working Tab data
                    OrderBindingList workingSubBlotterCollection = BlotterCommonCache.GetInstance().WorkingSubBlotterCollection;
                    foreach (OrderSingle orderSingle in workingSubBlotterCollection)
                    {
                        BlotterOrder blotterOrder = new BlotterOrder();
                        blotterOrder.UpdateDataFromOrderSingle(orderSingle, true);
                        workingTabData.Add(blotterOrder);
                    }
                }

                blotterResponse = new BlotterResponse(BlotterRequestType.GetData, orderTabData, workingTabData);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return blotterResponse;
        }
        /// <summary>
        /// This is used to get pst data when uncomplicated trade with no sub order is edited in blotter right click operation
        /// </summary>
        /// <param name="pSTRequestDTO"></param>
        /// <returns></returns>
        public PSTResponseDTO GetPSTPreferenceDetails(PSTRequestDTO pSTRequestDTO)
        {
            AllocationOperationPreference operationPreference = AllocationClientServiceConnector.Allocation.InnerChannel.GetPreferenceById(pSTRequestDTO.allocationPrefID);
            List<int> accountIds = new List<int>();
            if (operationPreference != null)
            {
                accountIds = operationPreference.TargetPercentage.Keys.ToList();
            }
            DataSet result = null;
            PSTResponseDTO pSTResponseDTO = new PSTResponseDTO();
            StringBuilder errorMessage = new StringBuilder();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = pSTRequestDTO.allocationPrefID;
                parameter[1] = pSTRequestDTO.OrderSideId;

                result = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetPTTDetails", parameter);
                if (result != null && result.Tables.Count > 1)
                {
                    DataTable requestDataTable = result.Tables[0];
                    pSTResponseDTO.pttRequestObject.PSTExtractRequestObjectFromDataTable(requestDataTable, pSTRequestDTO.symbol, errorMessage);

                    DataTable resposeDataTable = result.Tables[1];
                    foreach (DataRow responseDataRow in resposeDataTable.Rows)
                    {
                        PTTResponseObject pttResponseObject = new PTTResponseObject();
                        pttResponseObject.ExtractResponseObjectFromDataRow(responseDataRow);
                        pSTResponseDTO.pttResponseObjects.Add(pttResponseObject);
                    }
                }
                if (pSTResponseDTO.pttResponseObjects.Count != accountIds.Count)
                {
                    pSTResponseDTO.pttRequestObject.IsTradeBreak = true;
                }
                return pSTResponseDTO;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                return pSTResponseDTO;
            }
        }

        /// <summary>
        /// Removes stage/sub order(s) from cache.
        /// </summary>
        /// <param name="commaSaperateParentClOrderId"></param>
        /// <returns> Collection of orders removed </returns>
        internal Dictionary<string, Dictionary<string, OrderSingle>> RemoveStageOrSubOrderFromCollection(string commaSaperateParentClOrderId, bool isRemoveSubOrder = false)
        {
            Dictionary<string, Dictionary<string, OrderSingle>> dictOrdersRemoved = new Dictionary<string, Dictionary<string, OrderSingle>>();
            try
            {
                dictOrdersRemoved = BlotterOrderCollections.GetInstance().HideOrderFromBlotterGrid(commaSaperateParentClOrderId, isRemoveSubOrder);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dictOrdersRemoved;
        }

        /// <summary>
        /// Updates the allocation details.
        /// </summary>
        /// <param name="allocationDetails"></param>
        /// <returns></returns>
        internal Dictionary<string, List<OrderSingle>> UpdateAllocationDetails(List<AllocationDetails> allocationDetails)
        {
            Dictionary<string, List<OrderSingle>> updatedBlotterData = new Dictionary<string, List<OrderSingle>>();
            Dictionary<string, OrderSingle> updatedOrders = new Dictionary<string, OrderSingle>();
            Dictionary<string, OrderSingle> updatedSubOrders = new Dictionary<string, OrderSingle>();
            try
            {
                if (allocationDetails.Count > 0)
                {
                    foreach (OrderSingle currentOrder in BlotterCommonCache.GetInstance().OrderBlotterCollection)
                    {
                        allocationDetails.ForEach(details =>
                        {
                            if (details.ClOrderID.Equals(currentOrder.ParentClOrderID))
                            {
                                if (!updatedOrders.ContainsKey(currentOrder.ParentClOrderID))
                                    updatedOrders.Add(currentOrder.ParentClOrderID, UpdateAllocationColumnsInBlotter(details, currentOrder));
                                else
                                    updatedOrders[currentOrder.ParentClOrderID] = UpdateAllocationColumnsInBlotter(details, currentOrder);
                            }

                            if (currentOrder.OrderCollection != null)
                            {
                                bool anyChildUpdated = false;
                                // Preprocess allocation ClOrderIDs for fast lookup
                                var allocationMap = allocationDetails.ToDictionary(d => d.ClOrderID);

                                // Get all existing ParentClOrderIDs from WorkingSubBlotterCollection
                                var workingSubOrderIds = new HashSet<string>(
                                    BlotterCommonCache.GetInstance().WorkingSubBlotterCollection?
                                        .Select(subOrder => subOrder.ParentClOrderID)
                                        ?? Enumerable.Empty<string>()
                                );

                                foreach (OrderSingle innerOrder in currentOrder.OrderCollection)
                                {
                                    // This block handles updates for GTC and GTD orders.
                                    //
                                    // Scenario:
                                    // - Each order (parent) have child orders, and those child orders(subOrders) can have
                                    //   their own children (grandchildren).
                                    // - We check if the incoming update (details.ClOrderID) matches the ParentClOrderID
                                    //   of any grandchild.
                                    // - If a match is found:
                                    //     1. Update the matching subOrder row in the blotter.
                                    //     2. Update the parent order in the OrderTab collection to keep both in sync.
                                    if (innerOrder.TIF == FIXConstants.TIF_GTC || innerOrder.TIF == FIXConstants.TIF_GTD)
                                    {
                                        // Skip if no grandchildren exist
                                        if (innerOrder.OrderCollection == null) continue;

                                        // Loop through each grandchild
                                        foreach (var grandChild in innerOrder.OrderCollection)
                                        {
                                            // Check if the incoming update applies to this grandchild
                                            if (details.ClOrderID.Equals(grandChild.ParentClOrderID))
                                            {
                                                // Update the child (sub-order)
                                                updatedSubOrders.Add(innerOrder.ParentClOrderID, UpdateAllocationColumnsInBlotter(details, innerOrder));

                                                // Update the parent order
                                                UpdateOrderTabCollection(updatedOrders, innerOrder.StagedOrderID);
                                            }
                                        }
                                    }

                                    else if (allocationMap.TryGetValue(innerOrder.ParentClOrderID, out var matchingDetails))
                                    {
                                        // Skip if same ClOrderID exists in WorkingSubBlotterCollection                                   
                                        if (workingSubOrderIds.Contains(matchingDetails.ClOrderID))
                                            continue;

                                        // Safe to update
                                        UpdateAllocationColumnsInBlotter(matchingDetails, innerOrder);
                                        anyChildUpdated = true;
                                    }
                                }

                                if (anyChildUpdated)
                                {
                                    updatedOrders[currentOrder.ParentClOrderID] = currentOrder;
                                }
                            }
                        });
                    }

                    foreach (OrderSingle currentSubOrder in BlotterCommonCache.GetInstance().WorkingSubBlotterCollection)
                    {
                        allocationDetails.ForEach(details =>
                        {
                            if (details.ClOrderID.Equals(currentSubOrder.ParentClOrderID))
                            {
                                if (!updatedSubOrders.ContainsKey(currentSubOrder.ParentClOrderID))
                                    updatedSubOrders.Add(currentSubOrder.ParentClOrderID, UpdateAllocationColumnsInBlotter(details, currentSubOrder));
                                else
                                    updatedSubOrders[currentSubOrder.ParentClOrderID] = UpdateAllocationColumnsInBlotter(details, currentSubOrder);

                                #region Update OrderTab collection as Working/Sub OrderTab is updated
                                UpdateOrderTabCollection(updatedOrders, currentSubOrder.StagedOrderID);
                                #endregion
                            }
                        });
                    }
                }

                updatedBlotterData.Add(BlotterDataConstants.CONST_OrderTab, updatedOrders.Values.ToList());
                updatedBlotterData.Add(BlotterDataConstants.CONST_WorkingTab, updatedSubOrders.Values.ToList());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return updatedBlotterData;
        }

        /// <summary>
        /// Ensures that the order identified by <paramref name="stagedOrderID"/> 
        /// is present in the dictionary.
        /// Adds the order if it does not exist, or updates it with the latest data
        /// from the blotter collection if it already exists.
        /// </summary>
        private void UpdateOrderTabCollection(Dictionary<string, OrderSingle> updatedOrders, string stagedOrderID)
        {
            try
            {
                OrderSingle currentOrder = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(stagedOrderID);
                if (currentOrder != null)
                {
                    if (!updatedOrders.ContainsKey(stagedOrderID))
                        updatedOrders.Add(stagedOrderID, currentOrder);
                    else
                        updatedOrders[stagedOrderID] = currentOrder;
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
        /// Updates the allocation details in blotter.
        /// </summary>
        /// <param name="details"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private OrderSingle UpdateAllocationColumnsInBlotter(AllocationDetails details, OrderSingle order)
        {
            try
            {
                //Forcefully updated value of Account, Master fund, Strategy and Allocation Status value to Dash (-) in case of Order status is Pending New or Rejected. 
                //Because in case of Pending new and Rejected case, These groups are not visible in Allocation.
                if (order.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew || order.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                {
                    order.AllocationSchemeName = order.AllocationStatus = order.Account = order.MasterFund = order.Strategy = OrderFields.PROPERTY_DASH;
                    //return;
                }

                //Allocation Status
                order.AllocationStatus = details.AllocationStatus.ToString();

                //Update Account, Master Fund and Strategy column Values
                if (details.Level1Allocation.Count > 0)
                {
                    //Skiping this for Custom Allocation/Multi-account trades
                    if (details.AllocationSchemeName.Contains(TradingTicketType.Manual.ToString()))
                    {
                        order.Level1ID = details.Level1Allocation[0].LevelnID;
                    }
                    order.Account = GetAccountName(details);
                    order.MasterFund = GetMasterFundText(details);
                    order.Strategy = GetStrategyText(details);
                    order.AllocationSchemeName = details.AllocationSchemeName;
                }
                else
                {
                    order.AllocationSchemeName = order.Account = order.MasterFund = order.Strategy = OrderFields.PROPERTY_DASH;
                    order.MasterFund = CachedDataManager.GetInstance.IsShowMasterFundonTT() && !string.IsNullOrEmpty(order.TradeAttribute6) ? order.TradeAttribute6 : OrderFields.PROPERTY_DASH;
                }

                //update broker based on IsUseCustodianBroker preference. if accounts are mapped with more than one brokers then show multiple in broker field.
                if (order.IsUseCustodianBroker && order.Account == OrderFields.PROPERTY_MULTIPLE && order.CounterPartyID == int.MinValue)
                {
                    order.CounterPartyName = OrderFields.PROPERTY_MULTIPLE;
                }
                else if (order.CounterPartyID != int.MinValue)
                {
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                }

                order.TradeAttribute1 = details.TradeAttribute1;
                order.TradeAttribute2 = details.TradeAttribute2;
                order.TradeAttribute3 = details.TradeAttribute3;
                order.TradeAttribute4 = details.TradeAttribute4;
                order.TradeAttribute5 = details.TradeAttribute5;
                order.TradeAttribute6 = details.TradeAttribute6;
                order.SetTradeAttribute(details.GetTradeAttributesAsDict());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return order;
        }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        internal static string GetAccountName(AllocationDetails details)
        {
            try
            {
                return details.Level1Allocation.Count > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetAccountText(details.Level1Allocation[0].LevelnID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OrderFields.PROPERTY_DASH;
        }

        /// <summary>
        /// Gets the master fund text.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        internal static string GetMasterFundText(AllocationDetails details)
        {
            try
            {
                //Update Master Fund Column Vallue
                HashSet<int> masterFunds = new HashSet<int>();

                details.Level1Allocation.ForEach(level1Allocation =>
                {
                    //Master Fund IDs
                    int masterFundId = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(level1Allocation.LevelnID);
                    if (!masterFunds.Contains(masterFundId))
                        masterFunds.Add(masterFundId);
                });


                if (masterFunds.Count > 1)
                {
                    return OrderFields.PROPERTY_MULTIPLE;
                }
                else if (masterFunds.Count == 1)
                {
                    //Master Fund Name
                    string masterFundName = CachedDataManager.GetInstance.GetMasterFund(masterFunds.FirstOrDefault());
                    return String.IsNullOrEmpty(masterFundName) ? OrderFields.PROPERTY_DASH : masterFundName;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OrderFields.PROPERTY_DASH;
        }

        /// <summary>
        /// Gets the strategy text.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        private static string GetStrategyText(AllocationDetails details)
        {
            try
            {
                //Update Strategy Column Vallue
                HashSet<int> strategyIDs = new HashSet<int>();

                details.Level1Allocation.Where(level1Allocation => level1Allocation.Childs != null && level1Allocation.Childs.Collection.Count > 0).ToList().ForEach(level1Allocation =>
                {
                    //Strategy IDs
                    level1Allocation.Childs.Collection.ForEach(strategy =>
                    {
                        strategyIDs.Add(strategy.LevelnID);
                    });
                });

                //Strategy
                if (strategyIDs.Count() > 0)
                    return strategyIDs.Count() > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetStrategyText(strategyIDs.FirstOrDefault());
                else
                    return BlotterDataConstants.MSG_StrategyUnallocated;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return OrderFields.PROPERTY_DASH;
        }

        /// <summary>
        /// Cancel all sub(s) of selected ParentClOrderId(s).
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="commaSaperateParentClOrderId"></param>
        /// <returns></returns>
        internal string CancelAllSubOrders(int companyUserID, string commaSaperateParentClOrderId)
        {
            try
            {
                lock (_lockOnParentClOrderIDCache)
                {
                    if (commaSaperateParentClOrderId != null)
                    {
                        OrderBindingList cancelOrderCollection = new OrderBindingList();
                        foreach (string parentClOrderId in commaSaperateParentClOrderId.Split(','))
                        {
                            OrderSingle subOrder = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                            if (subOrder != null)
                            {
                                OrderSingle cancelRequest = (OrderSingle)subOrder.Clone();
                                cancelRequest.MsgType = FIXConstants.MSGOrderCancelRequest;
                                cancelRequest.ModifiedUserId = companyUserID;
                                ValidationManagerExtension.GetOrderDetails(cancelRequest);
                                cancelRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                                cancelOrderCollection.Add(cancelRequest);
                            }
                        }

                        #region Send cancelOrderCollection to server, make audit trail and save it.
                        if (cancelOrderCollection.Count > 0)
                        {
                            lock (_lockOnParentClOrderIDCache)
                            {
                                errorMsgForCancelAllSubs = string.Empty;
                                TradeManagerExtension.GetInstance().SendGroupCancelOrRolloverRequest(cancelOrderCollection);

                                foreach (var item in cancelOrderCollection)
                                    BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(item, TradeAuditActionType.ActionType.SubOrderCancelRequested, companyUserID, "Sub Order cancel requested");

                                BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                            }
                        }
                        #endregion
                    }
                    else
                        errorMsgForCancelAllSubs = BlotterDataConstants.MSG_ErrorCancelOrders;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMsgForCancelAllSubs;
        }

        /// <summary>
        /// Remove selected ParentClOrderId(s).
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="commaSaperateParentClOrderId"></param>
        internal string RemoveOrders(int companyUserID, string commaSaperateParentClOrderId)
        {
            bool isAllSubOrdersRemovable = true;
            List<string> listParentClOrderId = new List<string>();
            List<int> uniqueTradingAccounts = new List<int>();
            List<string> listParentClOrderIdOfSubOrders = new List<string>();
            try
            {
                lock (_lockOnParentClOrderIDCache)
                {
                    errorMsgForRemoveOrders = string.Empty;
                    if (commaSaperateParentClOrderId != null)
                    {
                        foreach (string parentClOrderId in commaSaperateParentClOrderId.Split(','))
                        {
                            OrderSingle currentOrder = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                            if (currentOrder != null)
                            {
                                if (currentOrder.LeavesQty > 0)
                                {
                                    isAllSubOrdersRemovable = false;
                                }
                                else
                                {
                                    listParentClOrderId.Add(currentOrder.ParentClOrderID);
                                    BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(currentOrder, TradeAuditActionType.ActionType.OrderRemoved, companyUserID);

                                    if (!uniqueTradingAccounts.Contains(currentOrder.TradingAccountID))
                                    {
                                        uniqueTradingAccounts.Add(currentOrder.TradingAccountID);
                                    }

                                    if (currentOrder.OrderCollection != null)
                                    {
                                        foreach (OrderSingle or in currentOrder.OrderCollection)
                                        {
                                            listParentClOrderIdOfSubOrders.Add(or.ParentClOrderID);
                                            if (!uniqueTradingAccounts.Contains(or.TradingAccountID))
                                            {
                                                uniqueTradingAccounts.Add(or.TradingAccountID);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #region Send remove order(s) information to server, make audit trail and save it.
                        if (listParentClOrderId.Count > 0)
                        {
                            BlotterCacheManager.GetInstance().HideOrderFromBlotter(listParentClOrderId, listParentClOrderIdOfSubOrders, isAllSubOrdersRemovable, companyUserID, uniqueTradingAccounts);
                            BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                        }
                        #endregion
                    }
                    else
                        errorMsgForRemoveOrders = BlotterDataConstants.MSG_ErrorRemovingOrders;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMsgForRemoveOrders;
        }

        /// <summary>
        /// Get Trading Accounts datatable By User ID
        /// </summary>
        /// <param name="companyUserID"></param>
        internal DataTable GetTradingAccountsByUserID(int companyUserID)
        {
            try
            {
                return BlotterDataManager.GetInstance().GetAllUsersByUserID(companyUserID);
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
        /// Freeze selected order(s) rows in Pending Compliance UI for.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="commaSaperateParentClOrderId"></param>
        internal void FreezePendingComplianceRows(int companyUserID, string commaSaperateParentClOrderId)
        {
            try
            {
                if (commaSaperateParentClOrderId != null)
                {
                    foreach (string parentClOrderId in commaSaperateParentClOrderId.Split(','))
                    {
                        OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                        if (order != null)
                        {
                            OrderSingle freezeOrder = (OrderSingle)order.Clone();
                            freezeOrder.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                            ValidationManagerExtension.GetOrderDetails(freezeOrder);
                            if (freezeOrder.OrderStatusWithoutRollover.Equals(PreTradeConstants.MsgTradePending))
                                freezeOrder.OrigClOrderID = order.ClOrderID;
                            freezeOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                            TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(freezeOrder);
                        }
                        else
                        {
                            bool orderInPendingApprovalCache = BlotterOrderCollections.GetInstance().IsOrderInPendingApprovalCache(parentClOrderId);
                            if (orderInPendingApprovalCache)
                            {
                                OrderSingle pendingApprovalOrder = BlotterOrderCollections.GetInstance().GetPendingApprovalOrderByClOrderID(parentClOrderId);
                                if (pendingApprovalOrder != null)
                                {
                                    OrderSingle freezeOrder = (OrderSingle)pendingApprovalOrder.Clone();
                                    freezeOrder.MsgType = FIXConstants.MSGOrderCancelRequestFroze;
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(freezeOrder);
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
                    throw;
            }
        }

        /// <summary>
        /// Unfreeze selected order(s) rows in Pending Compliance UI for.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="commaSaperateParentClOrderId"></param>
        internal void UnfreezePendingComplianceRows(int companyUserID, string commaSaperateParentClOrderId)
        {
            try
            {
                if (commaSaperateParentClOrderId != null)
                {
                    foreach (string parentClOrderId in commaSaperateParentClOrderId.Split(','))
                    {
                        OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                        if (order != null)
                        {
                            OrderSingle unfreezeOrder = (OrderSingle)order.Clone();
                            unfreezeOrder.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                            ValidationManagerExtension.GetOrderDetails(unfreezeOrder);
                            if (unfreezeOrder.OrderStatusWithoutRollover.Equals(PreTradeConstants.MsgTradePending))
                                unfreezeOrder.OrigClOrderID = order.ClOrderID;
                            unfreezeOrder.MsgType = FIXConstants.MSGOrderCancelRequestUnFroze;
                            TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(unfreezeOrder);
                        }
                        else
                        {
                            bool orderInPendingApprovalCache = BlotterOrderCollections.GetInstance().IsOrderInPendingApprovalCache(parentClOrderId);
                            if (orderInPendingApprovalCache)
                            {
                                OrderSingle pendingApprovalOrder = BlotterOrderCollections.GetInstance().GetPendingApprovalOrderByClOrderID(parentClOrderId);
                                if (pendingApprovalOrder != null)
                                {
                                    OrderSingle unfreezeOrder = (OrderSingle)pendingApprovalOrder.Clone();
                                    unfreezeOrder.MsgType = FIXConstants.MSGOrderCancelRequestUnFroze;
                                    TradeManagerExtension.GetInstance().SendMessageToPendingApprovalUI(unfreezeOrder);
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
                    throw;
            }
        }

        /// <summary>
        /// Remove manual execution.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="parentClOrderId"></param>
        internal string RemoveManualExecution(int companyUserID, string parentClOrderId)
        {
            try
            {
                lock (_lockOnParentClOrderIDCache)
                {
                    errorMsgForRemoveManualExec = string.Empty;
                    if (parentClOrderId != null)
                    {
                        OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                        if (order != null)
                        {
                            if (order.IsManualOrder)
                            {
                                bool result = AllocationClientDataManager.GetInstance.RemoveManualExecution(order.ClOrderID, order.AUECLocalDate);
                                if (order.OrderStatusTagValue != FIXConstants.ORDSTATUS_New && !result)
                                {
                                    errorMsgForRemoveManualExec = "This order is grouped with another group. Please ungroup first.";
                                    return errorMsgForRemoveManualExec;
                                }
                                else
                                {
                                    #region Make audit trail and save it.
                                    BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(order, TradeAuditActionType.ActionType.SubOrderRemoveManualExcecution, companyUserID, " Sub-Order Manual Excecution Removed");
                                    BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                                    #endregion
                                }
                                #region Send remove manual execution information to server.
                                List<int> tradingAccountIds = new List<int> { order.TradingAccountID };
                                BlotterCacheManager.GetInstance().HideSubOrderFromBlotter(order.ClOrderID, companyUserID, tradingAccountIds);
                                #endregion

                                if (order.ShortLocateParameter != null)
                                {
                                    order.ShortLocateParameter.BorrowQuantity = -order.Quantity;
                                    ShortLocateManager.GetInstance().UpdateShortLocateData(order.ShortLocateParameter);
                                }
                            }
                        }
                    }
                    else
                        errorMsgForRemoveManualExec = BlotterDataConstants.MSG_ErrorRemovingManualExecution;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMsgForRemoveManualExec;
        }

        /// <summary>
        /// Get Manual Fills.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="parentClOrderId"></param>
        /// <returns></returns>
        internal List<BlotterOrder> BlotterGetManualFills(int companyUserID, string parentClOrderId)
        {
            List<BlotterOrder> fillsData = new List<BlotterOrder>();
            try
            {
                if (parentClOrderId != null)
                {
                    OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                    if (order != null)
                    {
                        ValidationManagerExtension.GetOrderDetails(order);
                        order.MsgType = BlotterDataConstants.CONST_MANUALFILLS;
                        OrderBindingList _orderCollection = ManualFillsManager.GetInstance().GetBlotterNewManualFills(order);
                        if (_orderCollection != null && _orderCollection.Count > 0)
                        {
                            foreach (OrderSingle _order in _orderCollection)
                            {
                                BlotterOrder blotterOrder = new BlotterOrder();
                                blotterOrder.UpdateDataFromOrderSingle(_order);
                                fillsData.Add(blotterOrder);
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
            return fillsData;
        }

        /// <summary>
        /// Save Manual Fills.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="parentClOrderId"></param>
        internal string SaveManualFills(int companyUserID, List<SaveManualFillsDetails> fillCollection)
        {
            double amount = double.Epsilon;
            double sum = 0.0;
            double avgPrice = 0.0;
            try
            {
                lock (_lockOnParentClOrderIDCache)
                {
                    errorMsgForAddModifyFills = string.Empty;
                    if (fillCollection != null && fillCollection.Count > 0)
                    {
                        string parentClOrderId = fillCollection[0].ParentClOrderID;
                        OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                        if (order != null)
                        {
                            OrderBindingList saveCollection = new OrderBindingList();
                            #region Creating OrderSingle from fills
                            foreach (SaveManualFillsDetails fill in fillCollection)
                            {
                                double lastShare = Convert.ToDouble(fill.fill);
                                double lastPrice = Convert.ToDouble(fill.price);
                                OrderSingle or = ManualFillsHelper.CreateNewFill(order, lastShare, lastPrice);
                                if (lastShare != double.MinValue && lastShare != 0.0)
                                {
                                    saveCollection.Add(or);
                                }
                            }
                            #endregion
                            if (saveCollection.Count == 0)
                            {
                                OrderSingle newFill = saveCollection.AddNew() as OrderSingle;
                                newFill.ClOrderID = order.ClOrderID;
                                newFill.TransactionTime = DateTime.Now.ToUniversalTime();
                                saveCollection.Add(newFill);
                            }
                            foreach (OrderSingle checksumOrder in saveCollection)
                            {
                                checksumOrder.AUECID = order.AUECID;
                                if (checksumOrder.LastShares != double.Epsilon && checksumOrder.LastShares != 0.0)
                                {
                                    sum += Convert.ToDouble(checksumOrder.LastShares);
                                }
                                if (sum > Convert.ToDouble(order.Quantity))
                                {
                                    if (UserwiseTradingTicketUIPrefs != null && UserwiseTradingTicketUIPrefs.ContainsKey(companyUserID)
                                        && UserwiseTradingTicketUIPrefs[companyUserID].IsShowTargetQTY.HasValue
                                        && UserwiseTradingTicketUIPrefs[companyUserID].IsShowTargetQTY.Value)
                                    {
                                        errorMsgForAddModifyFills = BlotterDataConstants.MSG_ExecQtyGreaterThanTargetQty;
                                    }
                                    else
                                    {
                                        errorMsgForAddModifyFills = BlotterDataConstants.MSG_ExecQtyGreaterThanWorkingQty;
                                    }
                                    return errorMsgForAddModifyFills;
                                }
                            }
                            sum = 0.0;
                            OrderSingle manualAck = new OrderSingle
                            {
                                ClOrderID = order.ClOrderID,
                                MsgType = FIXConstants.MSGExecutionReport,
                                OrderID = order.OrderID,
                                PranaMsgType = order.PranaMsgType,
                                ParentClOrderID = order.ParentClOrderID,
                                Symbol = order.Symbol,
                                Quantity = order.Quantity,
                                OrderTypeTagValue = order.OrderTypeTagValue,
                                OrderSideTagValue = order.OrderSideTagValue,
                                OrderStatusTagValue = FIXConstants.ORDSTATUS_New
                            };
                            TradeManagerExtension.GetInstance().SendTradeAfterCheckCPConnection(manualAck);

                            foreach (OrderSingle fill in saveCollection)
                            {
                                ManualFillsHelper.FillDetails(order, fill, ref sum, ref amount, ref avgPrice);
                                string errorMsg = ManualFillsHelper.SetOrderStatus(fill);
                                if (errorMsg != null && !string.IsNullOrEmpty(errorMsg))
                                    errorMsgForAddModifyFills = errorMsg;

                                if (fill.LastShares != double.Epsilon)
                                {
                                    TradeManagerExtension.GetInstance().SendTradeAfterCheckCPConnection(fill);
                                }
                            }
                        }
                    }
                    else
                        errorMsgForAddModifyFills = BlotterDataConstants.MSG_ErrorSavingModifiedFills;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMsgForAddModifyFills;
        }

        /// <summary>
        /// Gets Allocation details.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="parentClOrderId"></param>
        /// <returns></returns>
        internal AllocationDetailsInformation GetAllocationDetails(int companyUserID, string parentClOrderId)
        {
            DataTable allocationDetails = null;
            DataTable finalAllocationDetails = null;
            Dictionary<int, string> accountList = null;
            Dictionary<int, string> allocationPreferences = null;
            bool isGroupedOrder = false;
            string orderGroupId = string.Empty;
            string groupingErrorMessage = string.Empty;
            DataTable groupData = null;
            decimal totalQuantity = 0;
            decimal executedQuantity = 0;
            decimal percantage = 0;
            decimal unallocatedExecutedPercentage = 0;
            try
            {
                lock (_lockOnParentClOrderIDCache)
                {
                    errorMsgForAllocationDetails = string.Empty;
                    if (parentClOrderId != null)
                    {
                        OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                        if (order != null)
                        {
                            allocationDetails = BlotterCacheManager.GetInstance().GetAllocationDetailsByClOrderID(ValidationManagerExtension.GetSelectedOrderClOrderIDs(order));

                            #region Check if Allocation is possile
                            if (allocationDetails.Rows.Count == 0)
                            {
                                errorMsgForAllocationDetails = BlotterDataConstants.MSG_ErrorAllocationDeleted;
                            }
                            else
                            {
                                decimal totalCumQty = 0.0m;
                                allocationDetails.AsEnumerable().ToList().ForEach(row =>
                                {
                                    if (row[OrderFields.PROPERTY_EXECUTED_QTY] != DBNull.Value)
                                        totalCumQty += Convert.ToDecimal(row[OrderFields.PROPERTY_EXECUTED_QTY]);
                                });
                                if (totalCumQty == 0.0m)
                                {
                                    errorMsgForAllocationDetails = BlotterDataConstants.MSG_ErrorNoFillsAvailable;
                                }
                                #endregion
                                if (string.IsNullOrEmpty(errorMsgForAllocationDetails))
                                {
                                    #region Checking if Order is Grouped
                                    var distinctCLOrderIds = allocationDetails.AsEnumerable().DistinctBy(c => c[OrderFields.PROPERTY_PARENT_CL_ORDERID]).ToList();
                                    orderGroupId = allocationDetails.Rows[0][OrderFields.CAPTION_GROUPID].ToString();
                                    isGroupedOrder = distinctCLOrderIds.Count > 1;
                                    if (isGroupedOrder)
                                    {
                                        groupingErrorMessage = BlotterDataConstants.MSG_ErrorOrderGrouped;
                                    }
                                    groupData = new DataTable();
                                    groupData.Columns.Add(OrderFields.PROPERTY_ORDER_ID, typeof(string));
                                    groupData.Columns.Add(OrderFields.PROPERTY_QUANTITY, typeof(decimal));
                                    groupData.Columns.Add(OrderFields.PROPERTY_EXECUTED_QTY, typeof(decimal));
                                    groupData.Columns.Add(OrderFields.PROPERTY_AVGPRICE, typeof(decimal));

                                    for (int i = 0; i < distinctCLOrderIds.Count; i++)
                                    {
                                        DataRow row = groupData.NewRow();
                                        row[OrderFields.PROPERTY_ORDER_ID] = distinctCLOrderIds[i][OrderFields.PROPERTY_PARENT_CL_ORDERID];
                                        row[OrderFields.PROPERTY_QUANTITY] = distinctCLOrderIds[i][BlotterDataConstants.CONST_ORDER_QTY];
                                        row[OrderFields.PROPERTY_EXECUTED_QTY] = distinctCLOrderIds[i][OrderFields.PROPERTY_EXECUTED_QTY];
                                        decimal avgPriceBase = 0;
                                        decimal fxRate = Convert.ToDecimal(distinctCLOrderIds[i][OrderFields.PROPERTY_FXRATE]);
                                        decimal avgPrice = Convert.ToDecimal(distinctCLOrderIds[i][OrderFields.PROPERTY_AVGPRICE]);
                                        string fxOperator = distinctCLOrderIds[i][OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].ToString();
                                        decimal assetId = Convert.ToDecimal(distinctCLOrderIds[i][OrderFields.PROPERTY_ASSET_ID]);
                                        decimal currencyId = Convert.ToDecimal(distinctCLOrderIds[i][OrderFields.PROPERTY_CURRENCYID]);
                                        if (assetId == (int)AssetCategory.FX || assetId == (int)AssetCategory.FXForward || assetId == (int)AssetCategory.Forex)
                                        {
                                            if (currencyId == 1)
                                                avgPriceBase = avgPrice;
                                            else
                                                avgPriceBase = 1 / avgPrice;
                                        }
                                        else
                                            avgPriceBase = fxOperator.Trim().Equals("D") && fxRate > 0 ? avgPrice / fxRate : avgPrice * fxRate;
                                        row[OrderFields.PROPERTY_AVGPRICE] = avgPriceBase;
                                        groupData.Rows.Add(row);
                                    }
                                    #endregion

                                    #region Checking permitted accounts for user
                                    List<int> accountIDs = new List<int>();
                                    if (allocationDetails.Rows.Count > 0 && !allocationDetails.Rows[0][BlotterDataConstants.CONST_FUND_ID].Equals(DBNull.Value))
                                        accountIDs = allocationDetails.AsEnumerable().Select(r => r.Field<int>(BlotterDataConstants.CONST_FUND_ID)).ToList();

                                    accountList = _tradeManager.UserPermittedAccounts[companyUserID];
                                    bool isPermittedAccounts = accountIDs.All(account => accountList.ContainsKey(account));
                                    if (!isPermittedAccounts)
                                    {
                                        errorMsgForAllocationDetails = BlotterDataConstants.MSG_ErrorViewNotPermitted;
                                    }
                                    #endregion

                                    #region Calculation for Allocation details
                                    //Get Distinct Rows
                                    var distinctRows = allocationDetails.AsEnumerable().DistinctBy(c => c[OrderFields.CAPTION_GROUPID]).ToList();

                                    //Update total quantity value
                                    distinctRows.ForEach(x => { totalQuantity += Convert.ToDecimal(x[OrderFields.PROPERTY_QUANTITY]); });

                                    //If the Order is unallocated then the fund id is coming null 
                                    if (allocationDetails.Rows[0][BlotterDataConstants.CONST_FUND_ID] == DBNull.Value)
                                    {
                                        executedQuantity = Convert.ToDecimal(allocationDetails.Rows[0][OrderFields.PROPERTY_EXECUTED_QTY].ToString());
                                        percantage = totalQuantity != 0.0m ? (executedQuantity * 100) / totalQuantity : 0;
                                    }
                                    unallocatedExecutedPercentage = percantage;

                                    allocationDetails.Columns.Add(BlotterDataConstants.CONST_ACCOUNT, typeof(string));
                                    allocationDetails.Columns.Remove(OrderFields.PROPERTY_PARENT_CL_ORDERID);
                                    allocationDetails.Columns.Remove(OrderFields.PROPERTY_EXECUTED_QTY);
                                    allocationDetails.Columns.Remove(BlotterDataConstants.CONST_ORDER_QTY);
                                    allocationDetails.Columns.Remove(OrderFields.PROPERTY_AVGPRICE);
                                    allocationDetails.Columns.Remove(OrderFields.PROPERTY_FXRATE);
                                    allocationDetails.Columns.Remove(OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR);
                                    DataRow[] resultRow = allocationDetails.Select(BlotterDataConstants.CONST_FUND_ID + " is null");
                                    foreach (DataRow row in resultRow)
                                    {
                                        row.Delete();
                                    }

                                    finalAllocationDetails = allocationDetails.DefaultView.ToTable(true);

                                    //Expression that calculate % on Total quantity and Round upto 4 Decimal places
                                    string expression = "Convert(((" + OrderFields.CAPTION_ALLOCATEDQTY + " / " + totalQuantity + ") * 100) , 'System.Decimal') ";

                                    DataColumn colPercTotalQty = new DataColumn(BlotterDataConstants.CONST_PERCENTAGE_ON_TOTAL_QTY, typeof(decimal), expression)
                                    {
                                        DefaultValue = 0
                                    };

                                    //Add Percentage Column on Total Qty
                                    finalAllocationDetails.Columns.Add(colPercTotalQty);

                                    //Update Remaining Qty and percentage
                                    finalAllocationDetails.AsEnumerable().ToList().ForEach(row =>
                                    {
                                        executedQuantity += Convert.ToDecimal(row[OrderFields.CAPTION_ALLOCATEDQTY]);
                                        percantage += Convert.ToDecimal(row[BlotterDataConstants.CONST_PERCENTAGE_ON_TOTAL_QTY]);
                                    });

                                    for (int i = 0; i < finalAllocationDetails.Rows.Count; i++)
                                    {
                                        int accountID = Convert.ToInt32(finalAllocationDetails.Rows[i][BlotterDataConstants.CONST_FUND_ID]);
                                        finalAllocationDetails.Rows[i][BlotterDataConstants.CONST_ACCOUNT] = accountList[accountID];
                                    }
                                    #endregion

                                    #region Allocation preferences
                                    ModuleManager.GetModulesForCompanyUser(companyUserID);
                                    Modules modules = ModuleManager.CompanyModulesPermittedToUser;

                                    bool isLevelingPermitted = modules.Cast<Module>().Any(module => module.ModuleName.Equals(PranaModules.ALLOCATION_LEVELING_MODULE));
                                    bool isProrataByNavPermitted = modules.Cast<Module>().Any(module => module.ModuleName.Equals(PranaModules.ALLOCATION_PRORATA_NAV_MODULE));
                                    allocationPreferences = AllocationClientDataManager.GetInstance.GetAllocationPreferences(CompanyID, companyUserID, isLevelingPermitted, isProrataByNavPermitted);
                                    #endregion
                                }
                            }
                        }
                        else
                            errorMsgForAllocationDetails = BlotterDataConstants.MSG_ErrorFecthingAllocationDetails;

                        AllocationDetailsInformation result = new AllocationDetailsInformation(finalAllocationDetails, accountList, allocationPreferences, errorMsgForAllocationDetails, isGroupedOrder, orderGroupId, groupData, groupingErrorMessage, unallocatedExecutedPercentage);
                        return result;
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
        /// This method gets pst allocation details
        /// </summary>
        /// <param name="allocPrefId"></param>
        /// <param name="orderSide"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal string GetPstAllocationDetails(string allocPrefId, string orderSide, string symbol)
        {
            try {
                PTTAllocDetailsRequest pttRequestObject = new PTTAllocDetailsRequest();
                List<object> modifiedResponseObjects = new List<object>();
                object modifiedRequestObject = new object();
                StringBuilder errorMessage = new StringBuilder();
                DataSet result = null;
                object[] parameter = new object[2];
                parameter[0] = Convert.ToInt32(allocPrefId);
                parameter[1] = orderSide;

                result = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetPTTDetails", parameter);
                if (result != null && result.Tables.Count > 1)
                {
                    DataTable resposeDataTable = result.Tables[1];
                    List<int> tradedAccounts = new List<int>();
                    foreach (DataRow responseDataRow in resposeDataTable.Rows)
                    {
                        PTTResponseObject pttResponseObject = new PTTResponseObject();
                        pttResponseObject.ExtractResponseObjectFromDataRow(responseDataRow);
                        var modifiedResponseObject = new
                        {
                            side = pttResponseObject.OrderSide,
                            accountName = CachedDataManager.GetInstance.GetAccount(pttResponseObject.AccountId),
                            tradeQuantity = pttResponseObject.TradeQuantity,
                            percentageAllocation = pttResponseObject.PercentageAllocation,
                            endingValue = pttResponseObject.PercentageType,
                        };
                        modifiedResponseObjects.Add(modifiedResponseObject);
                        tradedAccounts.Add(pttResponseObject.AccountId);
                    }

                    DataTable requestDataTable = result.Tables[0];
                    var selectedFundIds = requestDataTable.Rows[0]["SelectedFundIds"].ToString();
                    int[] fundIds = selectedFundIds
                    .Split(',')
                    .Select(s => s.Split('~')[0].Trim())
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .ToArray();
                    pttRequestObject.PSTExtractRequestObjectFromDataTable(requestDataTable, symbol, errorMessage);

                    modifiedRequestObject = new
                    {
                        symbol = pttRequestObject.Symbol,
                        resizingDoneOn = GetResizedDoneOn(pttRequestObject, tradedAccounts, fundIds),
                        target = pttRequestObject.Target,
                        type = pttRequestObject.Type,
                        direction = pttRequestObject.AddOrSet,
                        masterFundOrAccount = pttRequestObject.MasterFundOrAccount == PTTConstants.COL_MASTERFUND ? PTTConstants.CAP_MASTERFUND : (pttRequestObject.MasterFundOrAccount == PTTConstants.COL_CUSTOM_GROUP ? PTTConstants.CAP_CUSTOM_GROUP : pttRequestObject.MasterFundOrAccount),
                        price = pttRequestObject.SelectedFeedPrice,
                    };
                }
                var jsonStructure = new
                {
                    AllocationParameters = modifiedRequestObject,
                    TargetAllocation = modifiedResponseObjects
                };

                // Serialize to JSON
                string json = JsonConvert.SerializeObject(jsonStructure, Formatting.Indented);
                return json;
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
        /// This method gets the information about accounts/masterfunds/custom groups that are being resized
        /// </summary>
        /// <param name="pttRequestObject"></param>
        /// <param name="tradedAccounts"></param>
        /// <returns></returns>
        private string GetResizedDoneOn(PTTAllocDetailsRequest pttRequestObject, List<int> tradedAccounts, int[] fundIds)
        {
            string result = string.Empty;
            try
            {
                if (pttRequestObject.MasterFundOrAccount == PTTConstants.COL_ACCOUNT)
                {
                    foreach (int accountId in tradedAccounts)
                    {
                        result += CachedDataManager.GetInstance.GetAccount(accountId) + ", ";
                    }
                }
                else if (pttRequestObject.MasterFundOrAccount == PTTConstants.COL_MASTERFUND)
                {
                    HashSet<int> masterFundIds = new HashSet<int>();
                    foreach (int accountId in tradedAccounts)
                    {
                        masterFundIds.Add(CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(accountId));
                    }
                    result = string.Join(", ", masterFundIds.Select(masterFindId => CachedDataManager.GetInstance.GetMasterFund(masterFindId)));
                }
                else if (pttRequestObject.MasterFundOrAccount == PTTConstants.COL_CUSTOM_GROUP)
                {
                    var allCustomGroups = CachedDataManager.GetInstance.GetUserCustomGroups();
                    var selectedGroups = allCustomGroups.Where(customGroup => fundIds.Any(id => id == customGroup.MasterFundOrGroupId));
                    var selectedGroupsForTrade = selectedGroups
                    .Where(x => x.AccountList.Select(a => a.AccountId).Any(id => tradedAccounts.Contains(id)))
                    .Select(x => x.MasterFundOrGroupName)
                    .ToList();
                    result = string.Join(", ", selectedGroupsForTrade);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result.EndsWith(", ") ? result.Substring(0, result.Length - 2) : result;
        }

        /// <summary>
        /// Allocation details information
        /// </summary>
        internal class AllocationDetailsInformation
        {
            public DataTable AllocationDetails { get; set; }
            public Dictionary<int, string> AccountList { get; set; }
            public Dictionary<int, string> AllocationPreferences { get; set; }
            public string ErrorMessage { get; set; }
            public bool IsGroupedOrder { get; set; }
            public string OrderGroupId { get; set; }
            public DataTable GroupedOrderDetails { get; set; }
            public string GroupingErrorMessage { get; set; }
            public decimal UnallocatedExecutedPercentage { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            internal AllocationDetailsInformation(DataTable allocationDetails, Dictionary<int, string> accountList, Dictionary<int, string> allocationPreferences, string errorMessage, bool isGroupedOrder, string orderGroupId, DataTable groupedOrderDetails, string groupingErrorMessage, decimal unallocatedExecutedPercentage)
            {
                this.AllocationDetails = allocationDetails;
                this.AccountList = accountList;
                this.AllocationPreferences = allocationPreferences;
                this.ErrorMessage = errorMessage;
                this.IsGroupedOrder = isGroupedOrder;
                this.OrderGroupId = orderGroupId;
                this.GroupedOrderDetails = groupedOrderDetails;
                this.GroupingErrorMessage = groupingErrorMessage;
                this.UnallocatedExecutedPercentage = unallocatedExecutedPercentage;
            }
        }

        /// <summary>
        /// SaveAllocationDetails
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="allocationDetails"></param>
        /// <returns></returns>
        internal string SaveAllocationDetails(int companyUserID, List<SaveEasyAllocateDetails> allocationDetails, string orderGroupId, string parentClOrderId, int allocationPrefValue, bool isGroupedOrder)
        {
            try
            {
                OrderSingle order = null;
                lock (_lockOnParentClOrderIDCache)
                {
                    errorMsgForSaveAllocationDetails = string.Empty;
                    AllocationResponse allocationResponse = null;
                    decimal totalPct = 0.0m;
                    order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                    if (order != null)
                    {
                        if (allocationPrefValue == -1)
                        {
                            if (allocationDetails != null && allocationDetails.Count > 0)
                            {
                                SerializableDictionary<int, AccountValue> targetPct = new SerializableDictionary<int, AccountValue>();
                                foreach (SaveEasyAllocateDetails row in allocationDetails)
                                {
                                    decimal quantityPct = Convert.ToDecimal(row.TargetPercent);
                                    if (quantityPct > 0)
                                    {
                                        string accountName = row.account;
                                         int accountID = _tradeManager.UserPermittedAccounts[companyUserID].First(x => x.Value.Equals(accountName)).Key;
                                        targetPct.Add(accountID, new AccountValue(accountID, quantityPct));
                                        totalPct += quantityPct;
                                    }
                                }

                                if (!EqualsPrecise(totalPct, 100M))
                                {
                                    errorMsgForSaveAllocationDetails = BlotterDataConstants.MSG_ErrorAllocationPercentExceed;
                                }

                                AllocationParameter allocationParameter = new AllocationParameter(new AllocationRule()
                                {
                                    BaseType = AllocationBaseType.CumQuantity,
                                    RuleType = MatchingRuleType.None,
                                    MatchClosingTransaction = MatchClosingTransactionType.None,
                                    PreferenceAccountId = -1,
                                    ProrataAccountList = new List<int>(),
                                    ProrataDaysBack = 0,
                                }, targetPct, -1, companyUserID, true);

                                allocationResponse = AllocationClientDataManager.GetInstance.ReallocateGroup_Blotter(orderGroupId, allocationParameter, int.MinValue, companyUserID, parentClOrderId);
                            }
                            else
                                errorMsgForSaveAllocationDetails = BlotterDataConstants.MSG_ErrorSavingAllocationDetails;
                        }
                        else
                        {
                            allocationResponse = AllocationClientDataManager.GetInstance.ReallocateGroup_Blotter(orderGroupId, null, allocationPrefValue, companyUserID, parentClOrderId);
                        }

                        #region Make audit trail and save it.
                        if (allocationResponse != null)
                        {
                            if (string.IsNullOrEmpty(allocationResponse.Response))
                            {
                               if (isGroupedOrder) 
                                    AddAuditTrailForAllocation(companyUserID, order, allocationResponse, true);
                                else 
                                    AddAuditTrailForAllocation(companyUserID, order, allocationResponse);
                            }
                            else
                            {
                                errorMsgForSaveAllocationDetails = allocationResponse.Response;
                                var regex = new Regex(@"\r\n?|\n|\t", RegexOptions.Compiled);
                                errorMsgForSaveAllocationDetails = regex.Replace(errorMsgForSaveAllocationDetails, " ");
                                errorMsgForSaveAllocationDetails = FormatMessage(errorMsgForSaveAllocationDetails, totalPct > 100 ? BlotterDataConstants.CONST_EXCESS : BlotterDataConstants.CONST_REMAINING);
                            }
                        }
                        #endregion
                    }
                    else
                        errorMsgForSaveAllocationDetails = BlotterDataConstants.MSG_ErrorSavingAllocationDetails;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMsgForSaveAllocationDetails;
        }

        /// <summary>
        /// Rollover all sub(s) of selected ParentClOrderId(s).
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="commaSaperateParentClOrderId"></param>
        /// <returns></returns>
        internal string RolloverAllSubOrders(int companyUserID, string commaSaperateParentClOrderId)
        {
            try
            {
                lock (_lockOnParentClOrderIDCache)
                {
                    if (commaSaperateParentClOrderId != null)
                    {
                        foreach (string parentClOrderId in commaSaperateParentClOrderId.Split(','))
                        {
                            OrderSingle currentOrder = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                            if (currentOrder != null && currentOrder.OrderCollection != null && currentOrder.OrderCollection.Count > 0)
                            {
                                foreach (OrderSingle subOrder in currentOrder.OrderCollection)
                                {
                                    if (ValidationManagerExtension.ISOrderRolloverable(subOrder) && BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(subOrder.AUECID) &&
                                        BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC.ContainsKey(subOrder.AUECID) && BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC[subOrder.AUECID])
                                    {
                                        TradeManagerExtension.GetInstance().SendOrderForRollOver(subOrder, companyUserID);
                                    }
                                }
                            }
                        }
                    }
                    else
                        errorMsgForRolloverAllSubs = BlotterDataConstants.MSG_ErrorRolloverOrders;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errorMsgForCancelAllSubs;
        }

        /// <summary>
        /// Modify the error msg for Target Alllocation %
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatMessage(string input, string msg)
        {
            string updatedMessage = input;
            try
            {
                updatedMessage = input.Replace(BlotterDataConstants.CONST_ALLOCATION_PERCENTAGE_ENTERED, $". {BlotterDataConstants.CONST_ALLOCATION_ENTERED}:");
                updatedMessage = updatedMessage.Replace($"{BlotterDataConstants.CONST_REMAINING} {BlotterDataConstants.CONST_PERCENTAGE}:", $"{BlotterDataConstants.CONST_REMAINING}:");
                string[] parts = updatedMessage.Split(new[] { $"{BlotterDataConstants.CONST_ALLOCATION_ENTERED}: ", $" {BlotterDataConstants.CONST_REMAINING}:" }, StringSplitOptions.None);
                string allocation = parts[1].Trim() + BlotterDataConstants.CONST_PERCENTAGE;
                string remaining = parts[2].Trim() + BlotterDataConstants.CONST_PERCENTAGE;
                remaining = remaining.StartsWith("-") ? remaining.Substring(1) : remaining;
                return $"{parts[0]}{BlotterDataConstants.CONST_ALLOCATION_ENTERED}: {allocation} {msg}: {remaining}";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return updatedMessage;
        }

        /// Equals Precise
        /// </summary>
        /// <param name="val"></param>
        /// <param name="decimalValue"></param>
        /// <returns></returns>
        public bool EqualsPrecise(decimal val, decimal decimalValue)
        {
            try
            {
                return Math.Round(val, 10) == Math.Round(decimalValue, 10);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;

        }

        /// <summary>
        /// AddAuditTrailForAllocation
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="order"></param>
        /// <param name="resp"></param>
        private void AddAuditTrailForAllocation(int companyUserId, OrderSingle order, AllocationResponse resp, bool isGroupOrderReallocated = false)
        {
            try
            {
                resp.OldAllocationGroups.ForEach(x =>
                {
                    var unallocatedTaxlots = x.TaxLots.ToList();
                    foreach (TaxLot taxlot in unallocatedTaxlots)
                    {
                        BlotterAuditTrailManager.GetInstance().AddDeletedTaxlotsFromGroupToAuditEntry(taxlot, order, companyUserId);
                    }
                });

                BlotterAuditTrailManager.GetInstance().AddTaxlotsFromGroupToAuditEntry(resp.GroupList, order, companyUserId, isGroupOrderReallocated);

                BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Event to show fix connection down message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowFixBrokerDownMessageEvent(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                OrderSingle order = e.Value as OrderSingle;
                errorMsgForCancelAllSubs = BlotterDataConstants.MSG_FixConnectionDown + order.CounterPartyName + BlotterDataConstants.MSG_ResendOrder;
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
        /// Transfer user for the specified orders and apply changes as required.
        /// </summary>
        /// <param name="companyUserID">The company user ID initiating the transfer.</param>
        /// <param name="orderIDs">List of parent order IDs to be transferred.</param>
        /// <param name="transferUserID">The target user ID to transfer the orders to.</param>
        /// <param name="isOrderTab">Flag indicating whether the operation involves order tabs.</param>
        /// <param name="isSendSubOrder">Flag indicating whether sub-orders are included in the transfer.</param>
        /// <param name="isAllowUserToTansferTrade">Flag indicating whether the user is allowed to transfer trade.</param>
        /// <returns>String containing error details, or an empty string if successful.</returns>
        internal Dictionary<string,string> TransferUser(int companyUserID, List<string> orderIDs, int transferUserID, bool isOrderTab, bool isSendSubOrder, bool isAllowUserToTansferTrade)
        {
            try
            {
                string errorMsg = string.Empty;
                int successCount = 0;
                lock (_lockOnParentClOrderIDCache)
                {
                    if (orderIDs.Count > 0)
                    {
                        foreach (string parentClOrderId in orderIDs)
                        {
                            OrderSingle order = BlotterOrderCollections.GetInstance().GetOrderByClOrderID(parentClOrderId);
                            string orderError = SendTUTrades(order, companyUserID, transferUserID, isOrderTab, isSendSubOrder);

                            if (string.IsNullOrEmpty(orderError))
                                successCount++;
                            else
                                errorMsg = string.IsNullOrEmpty(errorMsg) ? orderError : errorMsg;

                            if (isSendSubOrder && order.OrderCollection != null && order.OrderCollection.Count > 0)
                            {
                                foreach (OrderSingle subOrder in order.OrderCollection)
                                {
                                    // Check if user is allowed to transfer trade based on rules
                                    if (isAllowUserToTansferTrade || order.CompanyUserID == companyUserID)
                                    {
                                        SendTUTrades(subOrder, companyUserID, transferUserID, isOrderTab, isSendSubOrder);
                                    }
                                }
                            }
                        }
                    }
                }
                BlotterAuditTrailManager.GetInstance().SaveAuditTrailData();
                // Serialize result data
                Dictionary<string, string> data = new Dictionary<string, string>
                {
                   { "errorMessage", errorMsg },
                   { "successMessage", (successCount > 0 ? successCount.ToString():string.Empty) }
                };

                return data;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Sends the TU trades by transferring the specified order.
        /// </summary>
        /// <param name="order">The order to transfer.</param>
        /// <param name="companyUserID">The company user identifier initiating the transfer.</param>
        /// <param name="transferUserID">The target user identifier for the transfer.</param>
        /// <param name="isOrderTab">Indicates whether it's an order tab.</param>
        /// <param name="isSendSubOrder">Indicates whether sub-orders should be transferred.</param>
        /// <returns>An error message if the transfer fails, otherwise an empty string.</returns>
        private string SendTUTrades(OrderSingle order, int companyUserID, int transferUserID, bool isOrderTab, bool isSendSubOrder)
        {
            string error = string.Empty;
            try
            {
                OrderSingle orderRequest = (OrderSingle)order.Clone();
                orderRequest.MsgType = FIXConstants.MSGTransferUser;
                orderRequest.PranaMsgType = (int)OrderFields.PranaMsgTypes.MsgTransferUser;
                orderRequest.ModifiedUserId = companyUserID;
                var originalUser = orderRequest.CompanyUserID;

                ValidationManagerExtension.GetOrderDetails(orderRequest);
                orderRequest.CompanyUserID = Convert.ToInt32(transferUserID);
                orderRequest.ClOrderID = order.ParentClOrderID;
                orderRequest.TransactionTime = DateTime.Now.ToUniversalTime();
                orderRequest.ExecutionTimeLastFill = DateTime.Now.ToUniversalTime().ToString(DateTimeConstants.NirvanaDateTimeFormat);
                DateTime currentDate = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                if ((order.TIF == FIXConstants.TIF_GTC || order.TIF == FIXConstants.TIF_GTD) && order.AUECLocalDate.Date != currentDate.Date)
                {
                    error = "Cannot transfer the order(s) after 1st day of order execution";
                }
                else
                {
                    var tradeSuccessful = TradeManagerExtension.GetInstance().SendTradeAfterCheckCPConnection(orderRequest);
                    if (tradeSuccessful)
                    {
                        string comment = string.Empty;

                        var action = TradeAuditActionType.ActionType.OrderTransferToUser;
                        if (!isSendSubOrder && isOrderTab)
                        {
                            comment = "Order Transferred from User " + CachedDataManager.GetInstance.GetUserText(originalUser) + " to " + CachedDataManager.GetInstance.GetUserText(companyUserID);
                            action = TradeAuditActionType.ActionType.OrderTransferToUser;
                        }
                        else
                        {
                            comment = "Sub-Order Transferred from User " + CachedDataManager.GetInstance.GetUserText(originalUser) + " to " + CachedDataManager.GetInstance.GetUserText(companyUserID);
                            action = TradeAuditActionType.ActionType.SubOrderTransferToUser;
                        }

                        BlotterAuditTrailManager.GetInstance().AddAuditTrailCollection(orderRequest, action, companyUserID, comment);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return error;
        }
    }
}
