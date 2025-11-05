using Prana.AmqpAdapter.Amqp;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTrade;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.DataManager
{
    public class DbQueueManager : IDisposable
    {
        private IQueueProcessor _dbQueue;
        private static DbQueueManager _dbQManager;
        System.Threading.Timer _timer = null;
        const int CONST_GROUPSAVEINTERVAL = 2000;
        private bool disposed = false;

        /// <summary>
        /// Event handler to refresh Blotter on Client.
        /// </summary>
        public event EventHandler RefreshBlotterAfterImport;
        /// <summary>
        /// sets true if Import has started.
        /// </summary>
        private bool _isImportStarted = false;
        /// <summary>
        /// Event handler to sets ImportStarted as true on client.
        /// </summary>
        public event EventHandler ImportStarted;
        /// <summary>
        /// Event handler to update MultiClOrderIdsCache in OrderCacheManager
        /// </summary>
        public event EventHandler<EventArgs<string>> UpdateMultiClOrderIdsCache;

        private DbQueueManager()
        {
        }

        public void Initlise(IQueueProcessor dbQueue, IAllocationServices allocationServices)
        {
            try
            {
                _allocationServices = allocationServices;
                _dbQueue = dbQueue;
                _dbQueue.StartListening();
                _dbQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_dbQueue_MessageQueued);
                _timer = new System.Threading.Timer(SaveGroups);
                _timer.Change(5000, CONST_GROUPSAVEINTERVAL);
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

        private bool _pendingStop = false;
        public event EventHandler GroupsSaved = null;

        public void SaveGroups(object groups)
        {
            try
            {
                // Once timer tick, stop the timer until the data is saved. Probably even if the data wasn't saved,
                // it was generating a new call to save the data on a separate thread and that might create deadlock in the db.
                _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                while (true)
                {
                    try
                    {
                        lock (_saveLocker)
                        {
                            // fill save
                            List<AllocationGroup> groupListNew = new List<AllocationGroup>();
                            List<AllocationGroup> groupList = new List<AllocationGroup>();
                            Queue<Order> queueOrder = new Queue<Order>();
                            lock (_fillLocker)
                            {
                                _allocationServices.GetUnSavedGroupsData(ref groupListNew, ref groupList);
                                queueOrder = OrderCacheManager.Instance.GetOrders();
                            }

                            if (queueOrder.Count == 0 && _isImportStarted && RefreshBlotterAfterImport != null)
                            {
                                RefreshBlotterAfterImport(this, null);
                                _isImportStarted = false;
                            }

                            while (queueOrder.Count > 0)
                            {
                                try
                                {
                                    Order order = queueOrder.Dequeue();
                                    SaveOrderInDb_FromColl(order);
                                    if (!string.IsNullOrEmpty(order.ImportFileName))
                                    {
                                        if (!_isImportStarted && ImportStarted != null)
                                        {
                                            _isImportStarted = true;
                                            ImportStarted(this, null);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Invoke our policy that is responsible for making sure no secure information
                                    // gets out of our layer.
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                }
                             }

                            _allocationServices.SaveUnSavedGroups(groupListNew, groupList);
                        }
                        if (_pendingStop && _allocationServices.CheckIfUnsavedGroups())
                        {
                            //This event will be fired to close the server after saving the unSaved groups.
                            if (GroupsSaved != null)
                            {
                                GroupsSaved(this, null);
                            }
                        }
                        System.Threading.Thread.Sleep(CONST_GROUPSAVEINTERVAL);
                    }
                    catch (Exception ex)
                    {
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    }
                }
            }
            catch (Exception ex)
            {
                _pendingStop = false;
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
        /// Setting flag to true in order to close the server to save data corruption.
        /// Unwiring the event so that we will not receive further messages just to have a preventive check.
        /// </summary>
        public void StopTimerAndUnwireEvent()
        {
            _pendingStop = true;
            _dbQueue.MessageQueued -= new EventHandler<EventArgs<QueueMessage>>(_dbQueue_MessageQueued);
        }

        IAllocationServices _allocationServices = null;

        /// <summary>
        /// This locker is used to save the data in a database on a single thread.
        /// </summary>
        readonly object _saveLocker = new object();
        readonly object _fillLocker = new object();
        void _dbQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage message = e.Value;
                switch (message.MsgType)
                {
                    case CustomFIXConstants.MSG_Trade:
                        PranaMessage pranaMsg = (PranaMessage)(message.Message);

                        ///This lock may slow the data saving as only 1 thread is saving the data sequentially.
                        ///For data consistency it is good as there won't be the issue of locks at the db level
                        ///Need to test is in stress scenarios and improve it
                        lock (_fillLocker)
                        {
                            Order order = ConvertMessagesToPranaTypes(pranaMsg);

                            // preallocation cases
                            switch (pranaMsg.MessageType)
                            {
                                case FIXConstants.MSGOrder:
                                    SaveGroupFromOrder(order);

                                    var orderList = new List<Order>() { order };
                                    AddOrderDataAuditEntryAndSaveInDB(orderList, pranaMsg.MessageType);

                                    break;
                                case FIXConstants.MSGExecutionReport:
                                case FIXConstants.MSGOrderCancelReject:
                                    if (message.IsMultiBroker) //Setting CumQTY to 0 if order is main order and executing by order brokers.
                                    {
                                        order.CumQty = 0;
                                        order.IsMultiDayParentOrder = true;
                                    }
                                    //Cancel does not impact post trade so, no need to send to allocation PM. As cancel is applied only on non exected QTY.
                                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-9203
                                    string status = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                                    string executionStatus = status;
                                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecType))
                                    {
                                        executionStatus = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExecType].Value;
                                    }
                                    if (status != FIXConstants.ORDSTATUS_Cancelled && executionStatus != FIXConstants.ORDSTATUS_PendingCancel
                                        && executionStatus != FIXConstants.ORDSTATUS_Suspended && status != FIXConstants.ORDSTATUS_RollOver
                                        && status != FIXConstants.ORDSTATUS_PendingRollOver && executionStatus != FIXConstants.ORDSTATUS_DoneForDay
                                        && executionStatus != FIXConstants.ORDSTATUS_Rejected && executionStatus != FIXConstants.ORDSTATUS_PendingNew
                                        && executionStatus != FIXConstants.ORDSTATUS_PendingReplace)
                                    {
                                        AllocationGroup allocationGroup = SaveGroupFromFills(order);

                                        //Multiday Orders are being sent to compliance from PreTradeManager only
                                        #region Compliance
                                        if (allocationGroup.CumQty > 0 && allocationGroup.AssetID != (int)AssetCategory.FX &&  !order.TIF.Equals(FIXConstants.TIF_GTC) && !order.TIF.Equals(FIXConstants.TIF_GTD))
                                        {
                                            string clOrderId = status == FIXConstants.EXECTYPE_Replaced ? order.OrderID : order.ClOrderID;
                                            order.ContractMultiplier = order.ContractMultiplier != 0 ? order.ContractMultiplier : (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagContractMultiplier) ? Convert.ToDouble(pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagContractMultiplier].Value) : order.ContractMultiplier);
                                            SendDataToEsperEngine(DoVirtualAllocation((AllocationGroup)allocationGroup.Clone()), clOrderId, order.OrderStatusTagValue, TagDatabaseManager.GetInstance.GetTIFText(order.TIF), order.ContractMultiplier);
                                        }
                                        #endregion
                                    }
                                    break;
                                case FIXConstants.MSGTransferUser:
                                    if (string.IsNullOrEmpty(order.StagedOrderID))
                                        order.CumQty = 0;
                                    if (message.IsMultiBroker) //Setting CumQTY to 0 if order is main order and executing by order brokers.
                                    {
                                        order.CumQty = 0;
                                        order.IsMultiDayParentOrder = true;
                                    }
                                    SaveGroupFromOrder(order);
                                    break;
                            }
                        }
                        break;

                    case CustomFIXConstants.MSG_Grp_Trade:
                        List<QueueMessage> msgList = (List<QueueMessage>)(message.Message);
                        foreach (QueueMessage msgQ in msgList)
                        {
                            PranaMessage pranaMsgTrade = new PranaMessage(msgQ.Message.ToString());
                            ///This lock may slow the data saving as only 1 thread is saving the data sequentially.
                            ///For data consistency it is good as there won't be the issue of locks at the db level
                            ///Need to test is in stress scenarios and improve it
                            lock (_fillLocker)
                            {
                                Order orderMsgTrade = ConvertMessagesToPranaTypes(pranaMsgTrade);
                                SaveGroupFromOrder(orderMsgTrade);
                            }
                        }
                        break;
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

        static DbQueueManager()
        {
            _dbQManager = new DbQueueManager();
        }

        public static DbQueueManager GetInstance()
        {
            return _dbQManager;
        }

        private AllocationGroup SaveGroupFromOrder(Order order)
        {
            return _allocationServices.CreateAllocationGroup(order, true);
        }

        private AllocationGroup SaveGroupFromFills(Order order)
        {
            return _allocationServices.CreateAllocationGroup(order, true);
        }

        private AllocationGroup DoVirtualAllocation(AllocationGroup virtualAllocationGroup)
        {
            return _allocationServices.DoVirtualAllocation(virtualAllocationGroup);
        }

        private void SendDataToEsperEngine(AllocationGroup virtualAllocationGroup, string clOrderID, string orderStatusTagValue, string tif, double contractMultiplier)
        {
            try
            {
                if (virtualAllocationGroup != null)
                {
                    List<TaxLot> virtualTaxlots = virtualAllocationGroup.GetAllTaxlots();
                    if (virtualTaxlots != null && virtualTaxlots.Count > 0)
                    {
                        foreach (TaxLot taxlot in virtualTaxlots)
                        {
                            if (String.IsNullOrEmpty(taxlot.TaxLotID))
                                taxlot.TaxLotID = "WI" + IDGenerator.GenerateClientOrderID();

                            if (taxlot.Level1ID == int.MinValue || taxlot.Level1ID == 0)
                                taxlot.Level1ID = -1;

                            if (taxlot.Level2ID == int.MinValue)
                                taxlot.Level2ID = -1;

                            taxlot.TIF = tif;

                            if (taxlot.FXConversionMethodOperator.ToString().Trim().ToUpper() != "M")
                            {
                                if (taxlot.FXRate != 0)
                                {
                                    taxlot.FXRate = 1 / taxlot.FXRate;
                                }
                                taxlot.FXConversionMethodOperator = "M";
                            }

                            taxlot.LotId = clOrderID;
                            if (virtualAllocationGroup.CumQty == 0 || orderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Cancelled))
                            {
                                taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                            }
                            taxlot.AssetCategoryValue = (AssetCategory)taxlot.AssetID;
                            taxlot.AssetName = taxlot.AssetCategoryValue.ToString();
                            taxlot.ContractMultiplier = contractMultiplier;

                            if (ComplianceCacheManager.GetInMarket())
                                AmqpHelper.SendObject(taxlot, "InTrade", "InTradeMarket");
                        }
                        ExpnlServiceConnector.GetInstance().UpdateInMarketTaxlots(virtualTaxlots);
                        TradingRulesInMarketCache.GetInstance().addToCache(virtualTaxlots, false);
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
        private Order ConvertMessagesToPranaTypes(PranaMessage message)
        {
            Order order = new Order();
            try
            {
                switch (message.MessageType)
                {
                    case FIXConstants.MSGOrder:
                    case FIXConstants.MSGOrderCancelRequest:
                    case FIXConstants.MSGOrderRollOverRequest:
                    case FIXConstants.MSGOrderCancelReplaceRequest:

                    case FIXConstants.MSGExecution:
                    case FIXConstants.MSGOrderCancelReject:
                    case FIXConstants.MSGTransferUser:
                        order = Transformer.CreateOrder(message);
                        if (order.AssetID == (int)AssetCategory.FixedIncome)
                        {
                            order.MaturityDate = Convert.ToDateTime(order.MaturityDay);
                        }
                        calculateFxRateFromCurrenyChecks(order);

                        //Divya:14022013: As now, we have given the option to apply commission on TT, thus if user apply some commission on TT, it will be calculated 
                        // at the order level. But if the calculation basis is AUTO,then commission will be calculated according to the counter party venue wise commission rules.
                        if (order.CalcBasis != Prana.BusinessObjects.AppConstants.CalculationBasis.Auto || order.SoftCommissionCalcBasis != Prana.BusinessObjects.AppConstants.CalculationBasis.Auto)
                        {
                            _allocationServices.CalculateCommissionOrderWise(ref order);
                        }
                        // Kuldeep A.: Setting Underlying Delta's value by delta because when trading from TT, leveraged factor's value flows in Delta field but we have used Underlying Delta to publish
                        // Leveraged factor on PM for non-option classes (Class CommonCachehelper.. Method: internal static EPnlOrder GetEPnlOrderFromTaxlot(TaxLot taxLot))
                        order.UnderlyingDelta = order.Delta;

                        SaveOrderInDb(order);
                        break;

                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                        order = Transformer.CreateOrder(message);
                        calculateFxRateFromCurrenyChecks(order);
                        SaveOrderInDb(order);
                        break;

                    case FIXConstants.MSGOrderList:
                        BasketDetail basket = Transformer.CreateBasket(message);
                        BasketDataManager.GetInstance().SaveBasketRequest(basket);
                        foreach (Order basketOrder in basket.BasketOrders)
                        {
                            calculateFxRateFromCurrenyChecks(basketOrder);
                            SaveOrderInDb(basketOrder);
                        }
                        break;

                    case FIXConstants.MSGListReplace:
                    case FIXConstants.MSGListCancelRequest:
                        BasketDetail basketCXL = Transformer.CreateBasket(message);
                        foreach (Order basketOrder in basketCXL.BasketOrders)
                        {
                            SaveOrderInDb(basketOrder);
                        }
                        break;

                    case CustomFIXConstants.MsgDropCopyReceived:
                        order = Transformer.CreateOrder(message);
                        ClientSubsAndFillsManager.GetInstance().SaveClientRequest(order);
                        break;

                    case CustomFIXConstants.MsgDropCopyExecution:
                        order = Transformer.CreateOrder(message);
                        ClientSubsAndFillsManager.GetInstance().SaveClientResponse(order);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                Logger.HandleException(new Exception("Problem at  Db Saving at DBQueueManager for saving msg=" + message.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
            }
            return order;
        }

        /// <summary>
        /// Add Order Data Audit Entry And Save In DB
        /// </summary>
        /// <param name="orderList"></param>
        /// <param name="messageType"></param>
        private static void AddOrderDataAuditEntryAndSaveInDB(List<Order> orderList, string messageType)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                List<TradeAuditEntry> auditTrailCol = new List<TradeAuditEntry>();
                try
                {
                    foreach (var order in orderList)
                    {
                        if (order != null)
                        {
                            TradeAuditEntry newEntry = new TradeAuditEntry();
                            Prana.BusinessObjects.TradeAuditActionType.ActionType action = TradeAuditActionType.ActionType.TradeEdited;
                            string comment = string.Empty;
                            TradeAuditActionTypeConverter ac = TypeDescriptor.GetConverter(typeof(TradeAuditActionType.ActionType)) as TradeAuditActionTypeConverter;
                            switch (messageType)
                            {
                                case FIXConstants.MSGOrder:

                                    if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.InternalOrder)
                                    {
                                        action = TradeAuditActionType.ActionType.OrderDoneAway;
                                        comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderDoneAway, typeof(string));
                                    }
                                    else if ((order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged && !order.IsManualOrder))
                                    {

                                        if (order.IsStageRequired)
                                        {
                                            action = TradeAuditActionType.ActionType.OrderLive;
                                            comment = "Live order with quantity " + order.Quantity;
                                        }
                                        else
                                        {
                                            action = TradeAuditActionType.ActionType.OrderStaged;
                                            comment = "Staged order with quantity " + order.Quantity;
                                        }

                                    }
                                    else if ((order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged && order.IsManualOrder))
                                    {
                                        action = TradeAuditActionType.ActionType.OrderDoneAway;
                                        comment = "Manual order with quantity " + order.Quantity;
                                    }
                                    else if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDNewSub || (OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDNewSubChild)
                                    {
                                        action = TradeAuditActionType.ActionType.OrderNewSub;
                                        comment = "New Sub(Live) Created with quantity " + order.Quantity;

                                    }
                                    else if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDStaged)
                                    {
                                        action = TradeAuditActionType.ActionType.OrderStaged;
                                        comment = "Staged Order Created with quantity " + order.Quantity;

                                    }
                                    else if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDManual)
                                    {
                                        action = TradeAuditActionType.ActionType.OrderDoneAway;
                                        comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderDoneAway, typeof(string));

                                    }
                                    else if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.ORDManualSub)
                                    {
                                        action = TradeAuditActionType.ActionType.OrderManualSub;
                                        comment = "New Sub(Manual) Created with quantity " + order.Quantity;
                                    }
                                    else if ((OrderFields.PranaMsgTypes)order.PranaMsgType == OrderFields.PranaMsgTypes.MsgDropCopy_PM)
                                    {
                                        action = TradeAuditActionType.ActionType.OrderAcknowledged;
                                        comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderAcknowledged, typeof(string));
                                    }
                                    break;

                                case FIXConstants.MSGOrderCancelRequest:
                                    action = TradeAuditActionType.ActionType.OrderCancelRequested;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderCancelRequested, typeof(string));
                                    break;

                                case FIXConstants.MSGOrderRollOverRequest:
                                    action = TradeAuditActionType.ActionType.SubOrderRollover;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.SubOrderRollover, typeof(string));
                                    break;

                                case FIXConstants.MSGOrderCancelReplaceRequest:
                                    action = TradeAuditActionType.ActionType.OrderCancelReplaceRequest;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderCancelReplaceRequest, typeof(string));
                                    break;

                                case FIXConstants.MSGExecution:
                                    switch (order.OrderStatus)
                                    {
                                        case FIXConstants.ORDSTATUS_Calculated:
                                            break;
                                        case FIXConstants.ORDSTATUS_Cancelled:
                                            break;
                                        case FIXConstants.ORDSTATUS_RollOver:
                                            break;
                                        case FIXConstants.ORDSTATUS_DoneForDay:
                                            break;
                                        case FIXConstants.ORDSTATUS_Expired:
                                            break;
                                        case FIXConstants.ORDSTATUS_Filled:
                                            break;
                                        case FIXConstants.ORDSTATUS_New:
                                            break;
                                        case FIXConstants.ORDSTATUS_PartiallyFilled:
                                            break;
                                        case FIXConstants.ORDSTATUS_PendingCancel:
                                            break;
                                        case FIXConstants.ORDSTATUS_PendingRollOver:
                                            break;
                                        case FIXConstants.ORDSTATUS_PendingNew:
                                            break;
                                        case FIXConstants.ORDSTATUS_PendingReplace:
                                            break;
                                        case FIXConstants.ORDSTATUS_Rejected:
                                            break;
                                        case FIXConstants.ORDSTATUS_Replaced:
                                            break;
                                        case FIXConstants.ORDSTATUS_Stopped:
                                            break;
                                        case FIXConstants.ORDSTATUS_Suspended:
                                            break;

                                    }

                                    action = TradeAuditActionType.ActionType.OrderExecuted;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderExecuted, typeof(string));
                                    break;

                                case FIXConstants.MSGOrderCancelReject:
                                    action = TradeAuditActionType.ActionType.OrderCancelReject;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderCancelReject, typeof(string));
                                    break;

                                case FIXConstants.MSGTransferUser:
                                    action = TradeAuditActionType.ActionType.OrderTransferToUser;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderTransferToUser, typeof(string));
                                    break;

                                case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                                    action = TradeAuditActionType.ActionType.OrderReplaced;
                                    comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, TradeAuditActionType.ActionType.OrderReplaced, typeof(string));
                                    break;
                            }
                            newEntry.Action = action;
                            newEntry.AUECLocalDate = DateTime.Now;
                            newEntry.OriginalDate = order.AUECLocalDate;
                            newEntry.Comment = comment;
                            newEntry.CompanyUserId = order.CompanyUserID;
                            newEntry.Symbol = order.Symbol;
                            newEntry.Level1ID = order.Level1ID;
                            newEntry.GroupID = string.Empty;
                            newEntry.TaxLotID = string.Empty;
                            newEntry.ParentClOrderID = order.ParentClOrderID;
                            newEntry.Source = TradeAuditActionType.ActionSource.Trade;
                            newEntry.ClOrderID = order.ClOrderID;
                            newEntry.OrderSideTagValue = order.OrderSideTagValue;
                            newEntry.OriginalValue = "";

                            auditTrailCol.Add(newEntry);
                        }
                        else
                            throw new NullReferenceException("The Data Table to add in audit dictionary is null");
                    }
                    AuditTrailDataManager.Instance.SaveAuditList(auditTrailCol);

                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Function used to set values of FXRate and SettlFXRate on the basis of Base Currency, Trade Currency and Settlement Currency,PRANA-11446
        /// Implemented for PRANA-11705
        /// </summary>
        void calculateFxRateFromCurrenyChecks(Order order)
        {
            try
            {
                if (order.AssetID == (int)AssetCategory.FX || order.AssetID == (int)AssetCategory.FXForward) return;

                int companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();

                if (companyBaseCurrencyID == order.CurrencyID)
                {
                    order.FXRate = 1;
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

        private void SaveOrderInDb(Order order)
        {
            try
            {
                OrderCacheManager.Instance.Add(order);
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

        public void SaveOrderInDb_FromColl(Order order)
        {
            try
            {
                //TODO: Make a save method for saving Sell side orders in a separate table.
                if ((order.MsgType == FIXConstants.MSGOrderSingle)
                    || (order.MsgType == FIXConstants.MSGOrderCancelReplaceRequest)
                    || (order.MsgType == FIXConstants.MSGOrderCancelRequest)
                    || (order.MsgType == FIXConstants.MSGOrderRollOverRequest))
                {
                    SubsAndFillsManager.GetInstance().SaveRequest(order);
                }
                else if ((order.MsgType == FIXConstants.MSGExecutionReport) ||
                       (order.MsgType == FIXConstants.MSGOrderCancelReject))
                {
                    if (SubsAndFillsManager.GetInstance().SaveResponse(order) && UpdateMultiClOrderIdsCache != null)
                    {
                        if (order.TIF.Equals(FIXConstants.TIF_GTC) || order.TIF.Equals(FIXConstants.TIF_GTD))
                        {
                            UpdateMultiClOrderIdsCache(this, new EventArgs<string>(order.ClOrderID));
                        }
                    }
                }
                else if (order.MsgType == FIXConstants.MSGTransferUser)
                {
                    SubsAndFillsManager.GetInstance().SaveUserChangeRequest(order);
                }
                else if (order.MsgType == CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew)
                {
                    SubsAndFillsManager.GetInstance().SaveAlgoSyntheticReplaceOrder(order);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _timer.Dispose();
            }
            disposed = true;
        }
    }
}