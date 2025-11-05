using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.EventArguments;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension.CacheStore;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using static Prana.BusinessObjects.PostTradeEnums;
using static Prana.BusinessObjects.PranaServerConstants;

namespace Prana.TradeManager.Extension
{
    public class TradeManagerExtension
    {
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        private Dictionary<int, CounterPartyDetails> _counterPartyConnectionStatusDetails = new Dictionary<int, CounterPartyDetails>();
        public BufferBlock<OrderSingle> dataBuffer;
        private ICommunicationManager _communicationManager = null;
        private static TradeManagerExtension _tradeManagerExtension = null;

        public delegate void BasketOrderMessageDelegate(object sender, EventArgs<Order> e);
        public delegate void BasketMessageDelegate(object sender, EventArgs<BasketDetail> e);
        public delegate void QueuedDelegate(object sender, QueuedDelegateEventArgs e);
        public delegate void CounterPartyConnectHandler(object sender, EventArgs<CounterPartyDetails> e);
        public delegate void DropCopyOrderRecievedDelegate(object sender, EventArgs<DropCopyOrder> e);
        public delegate void ExternalOrderImportHandler(object sender, EventArgs<OrderSingle> e);
        public delegate bool ReturnCheckForDuplicateTradeEvent(object sender, EventArgs<OrderSingle> args);

        public event ReturnCheckForDuplicateTradeEvent CheckForDuplicateTradeEvent;
        public event EventHandler<EventArgs<OrderSingle>> UpdateTradeAttributeListEvent;
        public event ExternalOrderImportHandler ExternalOrderImport;
        public event QueuedDelegate OrderQueued;
        public event BasketOrderMessageDelegate BasketOrderReceived;
        public event BasketMessageDelegate BasketReceived;
        public event DropCopyOrderRecievedDelegate InBoxOrderReceived;
        public event DropCopyOrderRecievedDelegate OutBoxOrderReceived;
        public event CounterPartyConnectHandler CounterPartyStatusUpdate;
        public event EventHandler<EventArgs<OrderSingle>> FixBrokerDownEventHandler;
        public event EventHandler<EventArgs<string>> ShowMessageBoxOnEnterpise;

        string tradeStatus = string.Empty;
        public string TradeStatus
        {
            get { return tradeStatus; }
            set { tradeStatus = value; }
        }

        List<string> permittedAUECCV = null;
        public List<string> PermittedAUECCV
        {
            get { return permittedAUECCV; }
            set { permittedAUECCV = value; }
        }

        string _applicationPath = string.Empty;
        public string ApplicationPath
        {
            get { return _applicationPath; }
            set { _applicationPath = value; }
        }

        /// <summary>
        /// BlotterClearanceCommonData
        /// </summary>
        public BlotterClearanceCommonData BlotterClearanceCommonData { get; set; } = null;

        /// <summary>
        /// IsWebApplication:set true when using web application.
        /// </summary>
        bool _isWebApplication = false;
        public bool IsWebApplication
        {
            get { return _isWebApplication; }
            set { _isWebApplication = value; }
        }

        private static int _userID;
        public int UserID
        {
            set { _userID = value; }
        }

        private int _companyID;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        public static TradeManagerExtension GetInstance()
        {
            if (_tradeManagerExtension == null)
            {
                _tradeManagerExtension = new TradeManagerExtension();
            }
            return _tradeManagerExtension;
        }

        private TradeManagerExtension()
        {
            try
            {
                dataBuffer = new BufferBlock<OrderSingle>();
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
        /// Sent the multitrade name and no. of orders to trade server
        /// </summary>
        /// <param name="multiTradeName"></param>
        /// <param name="sucessfullTrades"></param>
        public void SendMultiTradeDetails(String multiTradeId, int sucessfullTrades, int userId = 0)
        {
            try
            {
                string loggedInUserId =  userId > 0 ? 
                    Convert.ToString(userId): CachedDataManager.GetInstance.LoggedInUser?.CompanyUserID.ToString();

                if (sucessfullTrades == 0)
                    return;
                PranaMessage msg = new PranaMessage();
                msg.MessageType = CustomFIXConstants.CUST_TAG_MultiTradeMessage;
                msg.FIXMessage.InternalInformation.AddField("MultiTradeId", multiTradeId);
                msg.FIXMessage.InternalInformation.AddField("SucessfullTrades", sucessfullTrades.ToString());
                msg.FIXMessage.InternalInformation.AddField("UserId", loggedInUserId);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.CUST_TAG_MultiTradeMessage, msg);
                _communicationManager.SendMessage(qMsg);
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

        public bool CheckServerStatus()
        {
            if (ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                return true;
            }
            else if (ConnectionStatus == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
            {
                tradeStatus = "Please Connect to Server.";
                if (ShowMessageBoxOnEnterpise != null)
                {
                    ShowMessageBoxOnEnterpise(this, new EventArgs<string>("Please Connect to Server."));
                }
            }
            else
            {
                tradeStatus = "No Server is Running at specified Port !";
                if (ShowMessageBoxOnEnterpise != null)
                {
                    ShowMessageBoxOnEnterpise(this, new EventArgs<string>("No Server is Running at specified Port !"));
                }
            }
            return false;
        }

        public bool ValidateAUECandCV(int AUECID, int counterPartyID, int VenueID)
        {
            if (permittedAUECCV == null)
            {
                if (CachedDataManager.GetInstance.CheckTradePermissionByCVandAUECID(AUECID, counterPartyID, VenueID))
                {
                    return true;
                }
                else
                {
                    Logger.LoggerWrite("[REBALANCER] Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.");
                    throw new Exception("Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.");
                }
            }
            else
            {
                string key = "AUEC" + AUECID.ToString() + ":C" + counterPartyID.ToString() + ":V" + VenueID.ToString();
                if (permittedAUECCV.Contains(key))
                {
                    return true;
                }
                else
                {
                    tradeStatus = "Check if you have permission for the AUEC. \nAlso Check that the CounterParty Venue you want to trade has permissions to trade the same AUEC.";
                }
            }
            return false;
        }

        public bool CheckTradeConditions(OrderSingle or)
        {
            if (CheckServerStatus())
            {
                if ((!or.IsStageRequired && or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged))
                {
                    return true;
                }
                if (or.MsgType == FIXConstants.MSGOrderCancelReplaceRequest && or.IsUseCustodianBroker)
                {
                    return true;
                }
                if (or.ChangeType == (int)ChangeType.Trade || ValidateAUECandCV(or.AUECID, or.CounterPartyID, or.VenueID))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public bool CheckTradeConditions(OrderSingle or, Dictionary<int, int> accountBrokerMapping)
        {
            try
            {
                if (CheckServerStatus())
                {
                    if ((!or.IsStageRequired && or.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged))
                    {
                        return true;
                    }
                    if (or.ChangeType == (int)ChangeType.Trade)
                    {
                        return true;
                    }
                    foreach (int counterPartyID in accountBrokerMapping.Values)
                    {
                        if (!ValidateAUECandCV(or.AUECID, counterPartyID, or.VenueID)) return false;
                    }
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }

        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get { return _communicationManager.ConnectionStatus; }
        }

        public ICommunicationManager SetCommunicationManager
        {
            set
            {
                _communicationManager = null;
                _communicationManager = value;
                _communicationManager.MessageReceived -= new MessageReceivedDelegate(_communicationManager_MessageReceived);
                _communicationManager.MessageReceived += new MessageReceivedDelegate(_communicationManager_MessageReceived);
            }
        }

        /// <summary>
        /// Handles the event when the trade service is disconnected.
        /// </summary>
        public void DisconnectAllCounterParties()
        {
            try
            {
                foreach (KeyValuePair<int, CounterPartyDetails> kvp in _counterPartyConnectionStatusDetails)
                {
                    _counterPartyConnectionStatusDetails[kvp.Key].ConnStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    if (CounterPartyStatusUpdate != null)
                    {
                        CounterPartyStatusUpdate(this, new EventArgs<CounterPartyDetails>(_counterPartyConnectionStatusDetails[kvp.Key]));
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

        void _communicationManager_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage qmsg = e.Value;
                PranaMessage pranaMsg;
                switch (qmsg.MsgType)
                {
                    case CustomFIXConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT:
                        pranaMsg = new PranaMessage(qmsg.Message.ToString());
                        int connectionID = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ConnectionID].Value);
                        int connStatus = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyStatus].Value);


                        if (!_counterPartyConnectionStatusDetails.ContainsKey(connectionID))
                        {
                            CounterPartyDetails counterPartyDetails = new CounterPartyDetails();

                            counterPartyDetails.ConnectionID = connectionID;
                            counterPartyDetails.CounterPartyID = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value);
                            counterPartyDetails.OriginatorType = (OriginatorType)int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginatorType].Value);
                            counterPartyDetails.BrokerConnectionType = (BrokerConnectionType)int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BrokerConnectionType].Value);

                            _counterPartyConnectionStatusDetails.Add(connectionID, counterPartyDetails);
                        }

                        if (connStatus == (int)PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            _counterPartyConnectionStatusDetails[connectionID].ConnStatus = PranaInternalConstants.ConnectionStatus.CONNECTED;
                        }
                        else
                        {
                            _counterPartyConnectionStatusDetails[connectionID].ConnStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                        }

                        if (CounterPartyStatusUpdate != null)
                        {
                            CounterPartyStatusUpdate(this, new EventArgs<CounterPartyDetails>(_counterPartyConnectionStatusDetails[connectionID]));
                        }
                        break;

                    case CustomFIXConstants.MSG_CounterPartyDown:
                        pranaMsg = new PranaMessage(qmsg.Message.ToString());
                        Order ordercpDown = Transformer.CreateOrder(pranaMsg);
                        OrderQueued(this, new QueuedDelegateEventArgs(ordercpDown, PranaInternalConstants.ConnectionStatus.DISCONNECTED));
                        break;

                    case CustomFIXConstants.MSG_CounterPartyUp:
                        pranaMsg = new PranaMessage(qmsg.Message.ToString());
                        Order ordercpup = Transformer.CreateOrder(pranaMsg);
                        OrderQueued(this, new QueuedDelegateEventArgs(ordercpup, PranaInternalConstants.ConnectionStatus.CONNECTED));
                        break;

                    case FIXConstants.MSGOrderList:
                        if (BasketReceived != null)
                        {
                            pranaMsg = new PranaMessage(qmsg.Message.ToString());
                            BasketDetail basket = Transformer.CreateBasket(pranaMsg);
                            BasketReceived(this, new EventArgs<BasketDetail>(basket));
                        }
                        break;

                    case CustomFIXConstants.MsgDropCopyReceived:
                        pranaMsg = new PranaMessage(qmsg.Message.ToString());
                        DropCopyOrder dropCopyOrder = Transformer.CreateDropCopy(pranaMsg);
                        if (InBoxOrderReceived != null)
                        {
                            InBoxOrderReceived(this, new EventArgs<DropCopyOrder>(dropCopyOrder));
                        }
                        break;

                    case CustomFIXConstants.MsgDropCopyExecution:
                        pranaMsg = new PranaMessage(qmsg.Message.ToString());
                        DropCopyOrder dropCopyOrder1 = Transformer.CreateDropCopy(pranaMsg);
                        if (OutBoxOrderReceived != null)
                        {
                            OutBoxOrderReceived(this, new EventArgs<DropCopyOrder>(dropCopyOrder1));
                        }
                        InBoxOrderReceived(this, new EventArgs<DropCopyOrder>(dropCopyOrder1));
                        break;

                    case FIXConstants.MSGOrder:
                    case FIXConstants.MSGOrderCancelRequest:
                    case FIXConstants.MSGOrderRollOverRequest:
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                    case FIXConstants.MSGExecutionReport:
                    case FIXConstants.MSGOrderCancelReject:
                    case FIXConstants.MSGTransferUser:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:
                        pranaMsg = new PranaMessage(qmsg.Message.ToString());
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ListID))
                        {
                            if (BasketOrderReceived != null)
                            {
                                Order order = Transformer.CreateOrder(pranaMsg);
                                BasketOrderReceived(this, new EventArgs<Order>(order));
                            }
                        }
                        else
                        {
                            OrderProcess(pranaMsg);
                        }
                        break;
                    case CustomFIXConstants.MSG_NAV_LOCK_DATE_UPDATE:
                        if (DateTime.TryParse(qmsg.Message.ToString(), out DateTime lockDate))
                            CachedDataManager.GetInstance.NAVLockDate = lockDate;
                        else if (qmsg.Message.ToString().Equals(string.Empty))
                            CachedDataManager.GetInstance.NAVLockDate = null;
                        break;
                    default:
                        break;
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

        public PranaInternalConstants.ConnectionStatus GetCounterPartyConnectionSatus(int counterPartyID, OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS)
        {
            try
            {
                if (originatorTypeCategory == OriginatorTypeCategory.EOD)
                {
                    //creating composite ConnectionID "9" is prefix then originator type & counterpartyID, this logic is based on Trade Service logic
                    int connectionID = Convert.ToInt32("94" + counterPartyID);
                    if (_counterPartyConnectionStatusDetails.ContainsKey(connectionID))
                    {
                        return _counterPartyConnectionStatusDetails[connectionID].ConnStatus;
                    }
                }
                else
                {
                    //creating composite ConnectionID "9" is prefix then originator type & counterpartyID, this logic is based on Trade Service logic. here we are not sure that connection is for dropcopy/two way, so checking both ids in collection
                    int connectionID1 = Convert.ToInt32("90" + counterPartyID);
                    int connectionID2 = Convert.ToInt32("93" + counterPartyID);

                    if (_counterPartyConnectionStatusDetails.ContainsKey(connectionID1))
                    {
                        return _counterPartyConnectionStatusDetails[connectionID1].ConnStatus;
                    }
                    else if (_counterPartyConnectionStatusDetails.ContainsKey(connectionID2))
                    {
                        return _counterPartyConnectionStatusDetails[connectionID2].ConnStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        }

        public BrokerConnectionType GetBrokerConnectionType(int counterPartyID)
        {
            try
            {
                int connectionID = Convert.ToInt32("94" + counterPartyID);
                if (_counterPartyConnectionStatusDetails.ContainsKey(connectionID))
                {
                    return _counterPartyConnectionStatusDetails[connectionID].BrokerConnectionType;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return BrokerConnectionType.None;
        }


        public Dictionary<int, CounterPartyDetails> GetAllCounterPartyConnectionSatus()
        {
            return _counterPartyConnectionStatusDetails;
        }

        public void LoadUserEnteredTradesCache()
        {
            try
            {
                UserTradesCache.GetInstance.LoadUserEnteredTradesCache();
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

        public void RemoveFromUserTradesCache(OrderSingle orderReceived)
        {
            try
            {
                UserTradesCache.GetInstance.RemoveFromUserTradesCache(orderReceived);
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

        public void UserTradesDispose()
        {
            try
            {
                UserTradesCache.GetInstance.Dispose();
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

        public bool ExistsInUserTradesCache(OrderSingle order, int timeInterval)
        {
            try
            {
                return UserTradesCache.GetInstance.ExistsInUserTradesCache(order, timeInterval);
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

        public void AddTradesToUserTradesCache(OrderSingle order, UserAction userAction, string userActionType)
        {
            try
            {
                UserTradesCache.GetInstance.AddTradesToUserTradesCache(order, userAction, userActionType);
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
        /// SendGroupCancelOrRolloverRequest
        /// </summary>
        /// <param name="validatedCollection"></param>
        /// <returns></returns>
        public OrderBindingList SendGroupCancelOrRolloverRequest(OrderBindingList validatedCollection)
        {
            try
            {
                foreach (OrderSingle validatedOrder in validatedCollection)
                {
                    validatedOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                    SendTradeAfterCheckCPConnection(validatedOrder);
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
            return validatedCollection;
        }

        /// <summary>
        /// Sends the trade after check cp connection.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="isComingFromTT">if set to <c>true</c> [is coming from tt].</param>
        /// <returns></returns>
        public bool SendTradeAfterCheckCPConnection(OrderSingle order, bool isComingFromTT = false)
        {
            try
            {
                if (!(order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual
                   || order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub
                   || order.PranaMsgType == (int)OrderFields.PranaMsgTypes.MsgTransferUser
                   || order.MsgType == FIXConstants.MSGOrderRollOverRequest))
                {
                    if (GetCounterPartyConnectionSatus(order.CounterPartyID) == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        // if connected, send message, return true
                        return SendValidatedTrades(order, isComingFromTT);
                    }
                    else
                    {
                        if (order.PranaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged || (order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged && order.IsStageRequired && !order.IsManualOrder && !order.IsUseCustodianBroker))
                        {
                            // TODO: Show disconnected broker status
                            FixBrokerDownEventHandler?.Invoke(this, new EventArgs<OrderSingle>(order));
                            return false;
                        }
                        else
                        {
                            return SendValidatedTrades(order, isComingFromTT);
                        }
                    }
                }
                else
                {
                    //manual trade or rollover, no need to check CP connection
                    return SendValidatedTrades(order, isComingFromTT);
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
            return false;
        }

        /// <summary>
        /// Check multiple brokers connection status
        /// <summary>
        public HashSet<int> CheckAllFixConnectionsStatus(OrderSingle order, Dictionary<int, int> accountBrokerMapping)
        {
            HashSet<int> disconnectedCP = new HashSet<int>();
            try
            {
                if (!(order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual
                   || order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub
                   || order.PranaMsgType == (int)OrderFields.PranaMsgTypes.MsgTransferUser
                   || order.MsgType == FIXConstants.MSGOrderRollOverRequest))
                {
                    if (order.PranaMsgType != (int)OrderFields.PranaMsgTypes.ORDStaged || (order.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged && order.IsStageRequired && !order.IsManualOrder))
                    {
                        foreach (int brokerId in accountBrokerMapping.Values)
                        {
                            if (GetCounterPartyConnectionSatus(brokerId) != PranaInternalConstants.ConnectionStatus.CONNECTED)
                            {
                                disconnectedCP.Add(brokerId);
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
            return disconnectedCP;
        }

        /// <summary>
        /// Sends the validated trades.
        /// </summary>
        /// <param name="validatedOrder">The validated order.</param>
        /// <param name="isComingFromTT">if set to <c>true</c> [is coming from tt].</param>
        public bool SendValidatedTrades(OrderSingle validatedOrder, bool isComingFromTT = false, bool isDuplicateCheckReq = true)
        {
            bool allowTrade = true;
            try
            {
                //if trade is coming from TT and is not a cancel/replace request then check for duplicate trade
                if (isComingFromTT && !validatedOrder.MsgType.Equals(FIXConstants.MSGOrderCancelReplaceRequest) && isDuplicateCheckReq)
                {
                    switch ((OrderFields.PranaMsgTypes)validatedOrder.PranaMsgType)
                    {
                        case OrderFields.PranaMsgTypes.ORDNewSub:
                        case OrderFields.PranaMsgTypes.ORDNewSubChild:
                        case OrderFields.PranaMsgTypes.ORDManual:
                        case OrderFields.PranaMsgTypes.ORDManualSub:
                            if (CheckForDuplicateTradeEvent != null)
                                allowTrade = CheckForDuplicateTradeEvent(this, new EventArgs<OrderSingle>(validatedOrder));
                            break;

                        case OrderFields.PranaMsgTypes.ORDStaged:
                            if (validatedOrder.IsStageRequired && CheckForDuplicateTradeEvent != null)
                                allowTrade = CheckForDuplicateTradeEvent(this, new EventArgs<OrderSingle>(validatedOrder));
                            break;
                    }
                }
                if (allowTrade)
                {
                    if (validatedOrder.ListID != string.Empty)
                    {
                        BasketDetail basket = new BasketDetail();
                        basket.UserID = validatedOrder.CompanyUserID;
                        basket.TradingAccountID = validatedOrder.TradingAccountID;
                        basket.BasketID = validatedOrder.ListID;
                        OrderCollection basketOrders = new OrderCollection();
                        Order basketOrder = new Order(validatedOrder);
                        basketOrders.Add(basketOrder);
                        Prana.BusinessObjects.FIX.PranaMessage basketMessage = Transformer.CreatePranaMessageThroughReflection(basket, basketOrders, FIXConstants.MSGOrderList);
                        if (_enableTradeFlowLogging)
                        {
                            try
                            {
                                Logger.LoggerWrite("[Trade-Flow Out2] Before SendMessage In TradeManager, userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Fix Message: " + Convert.ToString(basketMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            }
                        }
                        _communicationManager.SendMessage(basketMessage.ToString());
                    }
                    Prana.BusinessObjects.FIX.PranaMessage message = Transformer.CreatePranaMessageThroughReflection(validatedOrder);
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow Out2] After SendMessage In TradeManager, userID: " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID + ", Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    }
                    _communicationManager.SendMessage(message.ToString());
                    if (UpdateTradeAttributeListEvent != null)
                        UpdateTradeAttributeListEvent(this, new EventArgs<OrderSingle>(validatedOrder));
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
            return allowTrade;
        }

        void BufferMessage(ITargetBlock<OrderSingle> target, OrderSingle message)
        {
            try
            {
                target.Post(message);
                //Logger.LoggerWrite("BufferMessageIn " + Thread.CurrentThread.ManagedThreadId.ToString() + " : SplitToAnalyze " + message.ToString() + " SplitToAnalyze", ApplicationConstants.CATEGORY_FLAT_FILE_ClientMessages);
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
        private void CalculateCommissionOrderWise(ref OrderSingle order)
        {
            try
            {
                double calculatedCommission = 0;
                double calculatedSoftCommission = 0;
                double commissionRate = 0.0;
                double softCommissionRate = 0.0;
                double notional = 0.0;

                CommissionRule rule = new CommissionRule();
                rule.Commission.RuleAppliedOn = (CalculationBasis)order.CalcBasis;
                rule.SoftCommission.RuleAppliedOn = (CalculationBasis)order.SoftCommissionCalcBasis;

                commissionRate = order.CommissionRate;
                softCommissionRate = order.SoftCommissionRate;

                switch (rule.Commission.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                        calculatedCommission = order.CumQty * commissionRate;
                        break;
                    case CalculationBasis.Contracts:
                        calculatedCommission = order.CumQty * commissionRate;
                        break;
                    case CalculationBasis.Notional:
                        notional = order.CumQty * order.AvgPrice * order.ContractMultiplier;
                        calculatedCommission = notional * commissionRate * 0.0001;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedCommission = commissionRate;
                        break;
                    case CalculationBasis.FlatRateProrata:
                        calculatedCommission = commissionRate * (order.CumQty / order.Quantity);
                        break;
                    default:
                        break;
                }

                switch (rule.SoftCommission.RuleAppliedOn)
                {
                    case CalculationBasis.Shares:
                        calculatedSoftCommission = order.CumQty * softCommissionRate;
                        break;
                    case CalculationBasis.Contracts:
                        calculatedSoftCommission = order.CumQty * softCommissionRate;
                        break;
                    case CalculationBasis.Notional:
                        notional = order.CumQty * order.AvgPrice * order.ContractMultiplier;
                        calculatedSoftCommission = notional * softCommissionRate * 0.0001;
                        break;
                    case CalculationBasis.FlatAmount:
                        calculatedSoftCommission = softCommissionRate;
                        break;
                    case CalculationBasis.FlatRateProrata:
                        calculatedSoftCommission = softCommissionRate * (order.CumQty / order.Quantity);
                        break;
                    default:
                        break;
                }
                order.CommissionAmt = calculatedCommission;
                order.SoftCommissionAmt = calculatedSoftCommission;
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

        void OrderProcess(PranaMessage pranaMsg)
        {
            try
            {
                //ResetTimers(true);
                OrderSingle orderReceived = Transformer.CreateOrderSingle(pranaMsg);
                if (orderReceived.TransactionSourceTag == (int)TransactionSource.FIX || orderReceived.TransactionSourceTag == (int)TransactionSource.None)
                    orderReceived.TransactionSource = TransactionSource.FIX;
                else if (orderReceived.TransactionSourceTag == (int)TransactionSource.TradingTicket)
                    orderReceived.TransactionSource = TransactionSource.TradingTicket;
                SetTransactionSource(orderReceived);

                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText) && pranaMsg.FIXMessage.InternalInformation.ContainsKey(FIXConstants.TagText))
                {
                    orderReceived.Text = pranaMsg.FIXMessage.InternalInformation[FIXConstants.TagText].Value;
                }

                BlotterOrderCollections.GetInstance().AddRemovePendingApprovalOrders(orderReceived);
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow] Published data received at the client side for userID: " + _userID + ", Symbol = " + orderReceived.Symbol + ", ExecID = " + orderReceived.ExecID + ", MsgSeqNum = "
                            + orderReceived.MsgSeqNum + ", OrderID = " + orderReceived.OrderID + ", CumQty = " + orderReceived.CumQty + ", TransactionTime = " + orderReceived.TransactionTime, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                if (orderReceived.OriginalAllocationPreferenceID != 0)
                {
                    //Check for Imported stagedorders with dynamic AllocationScheme
                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-25709 
                    SetTransactionSource(orderReceived);
                }

                if (orderReceived.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew || orderReceived.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected)
                {
                    orderReceived.AllocationSchemeName = orderReceived.AllocationStatus = orderReceived.Account = orderReceived.MasterFund = orderReceived.Strategy = OrderFields.PROPERTY_DASH;
                }


                //Calcuting the commission at server side would have needed to make the changes in Prana Message. 
                // As we have to show it ob blotter just once when the trade is executed, we are calculating it on client side.
                if (orderReceived.CalcBasis != Prana.BusinessObjects.AppConstants.CalculationBasis.Auto)
                {
                    CalculateCommissionOrderWise(ref orderReceived);
                }
                //lock (_listOrders)
                //{
                BufferMessage(dataBuffer, orderReceived);
                //_listOrders.Add(orderReceived);
                //remove order from user entered trades cache if trade is blocked by compliance
                if (orderReceived.Text.Equals(PreTradeConstants.MsgTradeReject))
                    UserTradesCache.GetInstance.RemoveFromUserTradesCache(orderReceived);
                //}

                if (orderReceived.OrderID != string.Empty)
                {
                    // it means that the order was imported
                    if (ExternalOrderImport != null)
                    {
                        //Raise Event to PM with eventArgs Order of the imported Order 
                        //so that other fills can be calculated and send to the server
                        ExternalOrderImport(this, new EventArgs<OrderSingle>(orderReceived));
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
        /// Set Transaction Source 
        /// </summary>
        /// <param name="orderReceived"></param>
        private static void SetTransactionSource(OrderSingle orderReceived)
        {
            try
            {
                if (orderReceived.TransactionSourceTag == (int)TransactionSource.TradeImport)
                {
                    orderReceived.TransactionSource = TransactionSource.TradeImport;
                }
                else if (orderReceived.TransactionSourceTag == (int)TransactionSource.PST)
                {
                    orderReceived.TransactionSource = TransactionSource.PST;
                }
                else if (orderReceived.TransactionSourceTag == (int)TransactionSource.Rebalancer)
                {
                    orderReceived.TransactionSource = TransactionSource.Rebalancer;
                }
                else if (orderReceived.TransactionSourceTag == (int)TransactionSource.HotButton)
                {
                    orderReceived.TransactionSource = TransactionSource.HotButton;
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

        public void SendMessage(PranaMessage message)
        {
            _communicationManager.SendMessage(message.ToString());
        }

        public void SendMessageToPendingApprovalUI(OrderSingle cancelOrder)
        {
            try
            {
                PranaMessage message = Transformer.CreatePranaMessageThroughReflection(cancelOrder);

                _communicationManager.SendMessage(message.ToString());
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
        /// AddClearanceSchedulerTasks
        /// </summary>
        public void AddClearanceSchedulerTasks()
        {
            try
            {
                //clearing collection before adding the AUEC data in case of logout and login from toolbar, PRANA-33163
                BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC.Clear();
                BlotterClearanceCommonData = DBTradeManager.GetInstance().GetCompanyClearanceCommonData(_companyID);
                BlotterCommonCache.GetInstance().ClearanceDataFull = DBTradeManager.GetInstance().GetClearanceData(_companyID, ref BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC);
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
        /// SetupAUECWiseClearanceTime
        /// </summary>
        public void SetupAUECWiseClearanceTime()
        {
            try
            {
                //clearing collection before adding the AUEC data in case of logout and login from toolbar, PRANA-33163
                BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.Clear();
                foreach (ClearanceData clearanceData in BlotterCommonCache.GetInstance().ClearanceDataFull)
                {
                    foreach (string auecid in clearanceData.AUECIDStr.Split(','))
                    {
                        if (!string.IsNullOrEmpty(auecid))
                        {
                            if (!BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.ContainsKey(int.Parse(auecid.Trim())))
                            {
                                BlotterCommonCache.GetInstance().DictAUECIDWiseBlotterClearance.Add(int.Parse(auecid.Trim()), clearanceData.ClearanceTime);
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Blotter multiple clearance setup for AUECID : " + clearanceData.AUECID + " in Admin.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        /// <summary>
        ///  This method is used to send the NAV lock date update
        /// </summary>
        /// <param name="lockDate">The NAV lock date in string format.</param>
        public void SendNAVLockDateUpdate(string lockDate)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_NAV_LOCK_DATE_UPDATE, lockDate);
                _communicationManager.SendMessage(qMsg);
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
        /// This method is used to send order for rollover
        /// </summary>
        /// <param name="order"></param>
        public void SendOrderForRollOver(OrderSingle order, int userid)
        {
            try
            {
                PranaMessage message = Transformer.CreatePranaMessageThroughReflection(order);
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_MANUAL_ROLLOVER, string.Empty, userid.ToString(), message);
                _communicationManager.SendMessage(qMsg);
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
    }
}
