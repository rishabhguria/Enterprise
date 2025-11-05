using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.CustomMapper;
using Prana.DataManager;
using Prana.DropCopyProcessor_PostTrade;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PreTrade;
using Prana.PubSubService.Interfaces;
using Prana.QueueManager;
using Prana.ServerCommon;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Prana.BusinessLogic;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.PositionManagement;
using System.Text;
using System.Linq;

namespace Prana.OrderProcessor
{
    public class PranaOrderProcessor : IProcessingUnit, IDisposable
    {
        ITradeQueueProcessor _queueDispatching;
        private int _hashCode = int.MinValue;
        IQueueProcessor _dbQueue;
        private MSMQQueueManager _connectionUnAvailableQueue;
        private IPreTradeService _preTradeService = new PreTradeService();
        private bool _isMultiBrokerWorkFlow = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("MultiBrokerWorkflow"));
        private string _multiDayTIF = ConfigurationHelper.Instance.GetAppSettingValueByKey("MultiDayTIF").ToString();
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        BufferBlock<PranaMessage> bufferBlock;

        #region IProcessor Members
        public event EventHandler<EventArgs<string, PranaMessage>> Error;

        private static IProcessingUnit _pranaOrderProcessor = null;
        static PranaOrderProcessor()
        {
            _pranaOrderProcessor = new PranaOrderProcessor();
            OrderCacheManager.FillCacheFromDataBase();
        }

        public static IProcessingUnit GetInstance
        {
            get { return _pranaOrderProcessor; }
        }

        IAllocationServices _allocationServices = null;
        public void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, ISecMasterServices secmasterServices, IAllocationServices allocationServices, IProcessingUnit orderProcessor)
        {
            _allocationServices = allocationServices;
            _secMasterServices = secmasterServices;
            _dbQueue = dbQueue;
            _queueDispatching = queueDispatching;
            _connectionUnAvailableQueue = new MSMQQueueManager(ConfigurationManager.AppSettings[PranaServerConstants.CONNECTION_UNAVAILABLE_PATH].ToString() + "_" + Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
            _connectionUnAvailableQueue.StartListening();
            _connectionUnAvailableQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_connectionUnAvailableQueue_MessageQueued);
            _hashCode = this.GetHashCode();

            bufferBlock = new BufferBlock<PranaMessage>();
            System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(bufferBlock)).ConfigureAwait(false);
            CreatePublishingProxy();

            #region Compliance Section
            try
            {
                //Initialising listener only if module is enabled for pre-trade
                if (Prana.CommonDataCache.ComplianceCacheManager.GetPreComplianceModuleEnabled())
                {
                    _preTradeService.RuleCheckReceived += _preTradeService_RuleCheckReceived;
                    _preTradeService.SendPendingApprovelNotificationEvent += _preTradeService_PendingApprovelRequestReceivedEvent;
                    _isPreTradeCheckEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
        }

        void _preTradeService_RuleCheckReceived(object sender, RuleCheckRecievedArguments e)
        {
            try
            {
                if (e.isPassed)
                {
                    foreach (KeyValuePair<String, PranaMessage> order in e.orders)
                    {
                        if (_enableTradeFlowLogging)
                        {
                            try
                            {
                                Logger.LoggerWrite("[Trade-Flow Out2] Rule Passed _preTradeService_RuleCheckReceived In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(order.Value.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                            }
                        }
                        //// Generate Sequence Number
                        if (order.Value.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrderSeqNumber))
                        {
                            if (Int64.Parse(order.Value.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrderSeqNumber].Value.ToString()) == Int64.MinValue)
                            {
                                string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();
                                order.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                            }
                        }
                        else
                        {
                            order.Value.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, UniqueIDGenerator.GetOrderSeqNumber());
                        }
                        if (e.isSimulation)
                            HandleByFixMsgType(order.Value);
                        else
                            HandleByPranaMessageType(order.Value, true);
                    }
                }
                else
                {
                    foreach (KeyValuePair<String, PranaMessage> order in e.orders)
                    {
                        PranaMessage originalMessage = null;
                        if (order.Value.MessageType == FIXConstants.MSGOrderCancelReplaceRequest)
                        {
                            originalMessage = new PranaMessage(order.Value.ToString());
                            originalMessage.MessageType = FIXConstants.MSGOrderCancelReject;
                            if (order.Value.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText)
                               && order.Value.FIXMessage.ExternalInformation[FIXConstants.TagText].Value.Equals(PreTradeConstants.MsgTradePending))
                            {
                                originalMessage.MessageType = FIXConstants.MSGOrderCancelRequest;
                                originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MsgTradeReject);
                            }
                        }
                        else
                        {
                            originalMessage = new PranaMessage(order.Value.ToString());
                            originalMessage.MessageType = FIXConstants.MSGOrderCancelRequest;
                            originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                        }

                        if (originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value != ((int)OrderFields.PranaMsgTypes.ORDStaged).ToString())
                            originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value = ((int)OrderFields.PranaMsgTypes.ORDManual).ToString();

                        if (e.isCancelOrder)
                            originalMessage.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MsgTradeCancelled);
                        else
                        {
                            originalMessage.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MsgTradeReject);
                            InformationReporter.GetInstance.Write("Trade blocked either due to pre-trade compliance or by user. OrderId: " + order.Key);
                            Logger.LoggerWrite("Trade blocked either due to pre-trade compliance or blocked by user. OrderId: " + order.Key, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        HandleByPranaMessageType(originalMessage, true);
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
        /// Pres the trade service pending approvel request received event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void _preTradeService_PendingApprovelRequestReceivedEvent(object sender, EventArgs<List<PranaMessage>> e)
        {
            try
            {
                e.Value.ForEach(order =>
                {
                    PranaMessage originalMessage = DeepCopyHelper.Clone<PranaMessage>(order);
                    originalMessage.MessageType = FIXConstants.MSGExecutionReport;
                    originalMessage.FIXMessage.InternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MsgTradePending);
                    originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                    //int PranaMsgType = int.Parse(originalMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                    //if ((PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual) || (PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub))
                    //    originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_New);
                    //else
                    //    originalMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingNew);
                    _queueDispatching.SendMessage(originalMessage);

                    #region Send order to Web application
                    PublishOrderUpdatesForWeb(originalMessage);
                    #endregion
                });
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
        /// Publish updated message for Web application
        /// </summary>
        /// <param name="originalMessage"></param>
        private static void PublishOrderUpdatesForWeb(PranaMessage originalMessage)
        {
            try
            {
                #region Send order to Web application
                List<PranaMessage> orders = new List<PranaMessage>();
                orders.Add(originalMessage);
                MessageData eventData = new MessageData
                {
                    EventData = orders,
                    TopicName = Topics.Topic_UpdatesForWebBlotter
                };
                _proxyPublishing.InnerChannel.Publish(eventData, Topics.Topic_UpdatesForWebBlotter);
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// The proxy publishing
        /// </summary>
        static ProxyBase<IPublishing> _proxyPublishing;

        /// <summary>
        /// Creates the publishing proxy.
        /// </summary>
        private static void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy", ex), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        public void ProcessMessage(PranaMessage pranaMessage)
        {
            try
            {
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus)
                    && pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID)
                    && pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID)
                    && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Cancelled
                    && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value == pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value)
                {
                    PranaMessage pranaCXLReqMsg = PranaDropCopyProcessor_PostTrade.GetSameInstance.CreatePranaCXLReqMsgFromExecutionReport(ref pranaMessage);
                    if (pranaCXLReqMsg != null)
                    {
                        OrderCacheManager.AddToCache(pranaCXLReqMsg);
                        SendAndSaveToDBQueue(pranaCXLReqMsg, false);
                    }
                    else
                    {
                        Logger.HandleException(new Exception("Unable to generate cancel request message. ClOrderID: " + pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value), LoggingConstants.POLICY_LOGONLY);
                    }
                }

                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DCStageWorkFlow) && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DCStageWorkFlow].Value == "StageAndSendAck")
                {
                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDStaged).ToString());
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, "0");
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, "0");
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecTransType, FIXConstants.EXECTYPE_New);
                    pranaMessage.MessageType = FIXConstants.MSGOrderSingle;
                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_SoftCommissionRate, "0");
                }

                if (pranaMessage.MessageType == CustomFIXConstants.CUST_TAG_MultiTradeMessage)
                {
                    String userId = pranaMessage.FIXMessage.InternalInformation.GetField("UserId");
                    //just storing the values, Will use them once the new pretrade manager is done
                    //Added the Check condition for Pre Trade permission user wise
                    if (_isPreTradeCheckEnabled && ComplianceCacheManager.GetPreTradeCheck(Convert.ToInt32(userId)))
                    {
                        String multiTradeId = pranaMessage.FIXMessage.InternalInformation.GetField("MultiTradeId");
                        int orderCount = Convert.ToInt32(pranaMessage.FIXMessage.InternalInformation.GetField("SucessfullTrades"));
                        _preTradeService.InformAboutMultiTradeEOM(multiTradeId, userId, orderCount);
                    }
                    return;
                }

                //TODO remove code from here as soon as Post Trade handling starts on Server side
                if (pranaMessage.MessageType == CustomFIXConstants.MSG_REFRESH_DEFAULT) // refersh message for allocation default refreh
                {
                    PranaTaxLotCacheManager.GetInstance.RefreshAllocationDefaults();
                    return;
                }

                //// Generate Sequence Number
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrderSeqNumber))
                {
                    if (Int64.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrderSeqNumber].Value.ToString()) == Int64.MinValue)
                    {
                        string orderIDSeqNumber = UniqueIDGenerator.GetOrderSeqNumber();
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, orderIDSeqNumber);
                    }
                }
                else
                {
                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, UniqueIDGenerator.GetOrderSeqNumber());
                }

                bool isDropCopyOrder = false;
                if (pranaMessage.MessageType == CustomFIXConstants.MsgDropCopyReceived)
                {
                    CreateOrUpdateDCOrder(pranaMessage);
                    isDropCopyOrder = true;
                }

                if (ValidationManager.GetInstance().ValidateOrder(pranaMessage))
                {
                    _secMasterServices.SetSecuritymasterDetails(pranaMessage);
                    if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTimeInForce) && !string.IsNullOrEmpty(_multiDayTIF))
                    {
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTimeInForce, _multiDayTIF);
                    }
                    PranaMessage updateFillForMultiDayChild = null;
                    if (OrderCacheManager.HasMultiDayHistory(pranaMessage))
                    {
                        // To Handle the scenarios wherein we receive different values in tag 150 and tag 39
                        updateFillForMultiDayChild = OrderInformation.CreateReplaceCancelUpdateForMultiDayChild(pranaMessage);
                        if (OrderInformation.IsExecutionMsg(pranaMessage))
                        {
                            CacheManagerDAL.GetInstance().UpdateDayWiseMultiDayFields(pranaMessage);
                        }
                    }
                    HandleByFixMsgType(pranaMessage, isDropCopyOrder);
                    if (updateFillForMultiDayChild != null)
                    {
                        CacheManagerDAL.GetInstance().UpdateDayWiseMultiDayFields(updateFillForMultiDayChild);
                        if (updateFillForMultiDayChild.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagDayCumQty) &&
                            Convert.ToDouble(updateFillForMultiDayChild.FIXMessage.ExternalInformation[FIXConstants.TagDayCumQty].Value) > 0)
                        {
                            HandleByFixMsgType(updateFillForMultiDayChild, isDropCopyOrder);
                        }
                    }
                }
                else
                {
                    Logger.HandleException(new Exception("Message not validated , Received from : " + pranaMessage.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
                }
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Problem at PranaOrderProcessor", pranaMessage));
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void CreateOrUpdateDCOrder(PranaMessage pranaMessage)
        {
            try
            {
                PranaMessage pranaReqMsg = null;
                PranaMessage multidayACKMessage = null;
                string orderID = string.Empty;
                string clOrderID = string.Empty;
                string origClOrderID = string.Empty;
                if (DropCopyCacheManager_PostTrade.IsCXLReplaceNewOrder(pranaMessage)) // it means start from here .. create a replace order
                {
                    pranaReqMsg = ((PranaDropCopyProcessor_PostTrade)(PranaDropCopyProcessor_PostTrade.GetInstance)).CreatePranaCXLReplaceMsgFromExecutionReport(pranaMessage);
                    if (pranaReqMsg != null)
                    {
                        string orderStatus = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                        switch (orderStatus)
                        {
                            case FIXConstants.ORDSTATUS_Cancelled:
                                pranaReqMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingCancel);
                                break;
                            case FIXConstants.ORDSTATUS_Replaced:
                                pranaReqMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingReplace);
                                break;

                        }
                    }

                }

                else if (!DropCopyCacheManager_PostTrade.DoesOrderExist(pranaMessage)) // it means start from here .. create a new order
                {
                    pranaReqMsg = PranaDropCopyProcessor_PostTrade.GetSameInstance.CreatePranaReqMsgFromExecutionReport(pranaMessage);

                    /* https://jira.nirvanasolutions.com:8443/browse/PRANA-41664
                       For MultidayOrders, we need the new (39=0) entry for the dropcopy so in case it is not coming
                       so we are creating it at this point of trade-flow */
                    if (pranaReqMsg != null && OrderInformation.IsMultiDayOrder(pranaMessage) && OrderInformation.IsExecutionMsg(pranaMessage))
                    {
                        pranaReqMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                        pranaReqMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, "0");
                        multidayACKMessage = ManualMessageHandler.GenerateExecutionReport(pranaReqMsg);
                        /* In order to make sure that any later Fix message does not get a lesser sequence number, assigning the
                           first fill seq number to this 39=0 message. Note: Order Sequence assigning code is on another 
                           thread at the Fix connector level. Also, as per the Multi-day architecture, the 39=0 message will be associated with 
                           Parent order while 39 = 1 will be associated with child order so for a given ClorderID, the numbers won't override*/
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrderSeqNumber))
                            multidayACKMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber,
                                pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrderSeqNumber].Value);
                    }
                }

                if (pranaReqMsg != null)
                {
                    ProcessMessage(pranaReqMsg);
                    DropCopyCacheManager_PostTrade.AddNewOrder(pranaReqMsg);
                    orderID = pranaReqMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                    clOrderID = DropCopyCacheManager_PostTrade.GetClOrderID(orderID);
                    if (OrderCacheManager.HasMultiDayHistory(pranaReqMsg))
                    {
                        if (pranaReqMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                            origClOrderID = pranaReqMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                        if (!string.IsNullOrEmpty(origClOrderID))
                            pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, origClOrderID);
                    }

                }
                else
                {
                    orderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                    clOrderID = DropCopyCacheManager_PostTrade.GetClOrderID(orderID);
                }

                //Add it to the fill               
                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);

                //Receiver-side code for RealtimeManualOrderFix Handling. Needed ParentCloOrderID to delete the fills on receiver side as well
                //https://jira.nirvanasolutions.com:8443/browse/PRANA-39418
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                {
                    int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                    if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual || nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub)
                    {
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, DropCopyCacheManager_PostTrade.GetParentClOrderID(orderID));
                    }
                }
                if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecID) || pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value == string.Empty)
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, Guid.NewGuid().ToString());
                }
                pranaMessage.MessageType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value;
                if (multidayACKMessage != null)
                {
                    multidayACKMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                    multidayACKMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, DropCopyCacheManager_PostTrade.GetParentClOrderID(orderID));
                    multidayACKMessage.MessageType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value;
                    ServerCommonBusinessLogic.SetExpiryDateDetails(multidayACKMessage);
                    DropCopyCacheManager_PostTrade.AddReceivedMessage(multidayACKMessage);
                    SendAndSaveToDBQueue(multidayACKMessage, true);
                }
                DropCopyCacheManager_PostTrade.AddReceivedMessage(pranaMessage);
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

        void _connectionUnAvailableQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage qMsg = e.Value;
                PranaMessage message = (PranaMessage)qMsg.Message;
                switch (qMsg.MsgType)
                {
                    case CustomFIXConstants.MSG_CounterPartyDown:
                        PranaMessage rejectMessage = ManualMessageHandler.GenerateExecutionReport(message);
                        rejectMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Rejected);
                        rejectMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, "CounterParty Down.");
                        SendAndSaveToDBQueue(rejectMessage, true);
                        break;

                    case CustomFIXConstants.MSG_CounterPartySendingProblem:
                        // create Reject Message
                        PranaMessage rejectMsg = ManualMessageHandler.GenerateExecutionReport(message);
                        rejectMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Rejected);
                        rejectMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, "Problem while Sending to CounterParty");
                        SendAndSaveToDBQueue(rejectMsg, true);
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

        private void HandleByFixMsgType(PranaMessage pranaMessage, bool isDropCopyOrder = false)
        {
            try
            {
                List<PranaMessage> pranaMessages = new List<PranaMessage>();
                bool isMultiTradeIdCreated = false;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsUseCustodianBroker) &&
                    bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsUseCustodianBroker].Value))
                {
                    if (!pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_MultiTradeMessage))
                    {
                        isMultiTradeIdCreated = true;
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_MultiTradeMessage, IDGenerator.GenerateMultiTradeId().ToString());
                    }
                    pranaMessages = MultiBrokerOrderHelper.GetInstance.CreatePranaMessagesList(pranaMessage, _allocationServices);
                }
                else
                {
                    pranaMessages.Add(pranaMessage);
                }

                foreach (PranaMessage PranaMessage in pranaMessages)
                {
                    string clOrderID = "";
                    if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                    {
                        clOrderID = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    }
                    switch (PranaMessage.MessageType)
                    {
                        case FIXConstants.MSGOrder:
                            bool isCreateSubMessage = false;
                            if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsStageRequired) && bool.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsStageRequired].Value))
                            {
                                isCreateSubMessage = true;
                            }
                            ServerCommonBusinessLogic.SetDateDetails(PranaMessage);
                            clOrderID = UniqueIDGenerator.GetClOrderID();
                            Logger.LoggerWrite("ClOrder used=" + clOrderID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                            PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, clOrderID);
                            OrderCacheManager.AddToCache(PranaMessage);
                            _preTradeService.AddOrUpdateStatusToOrderStatusTrackCache(clOrderID, ComplianceOrderStatus.New);
                            #region Compliance Section
                            try
                            {
                                if (_isPreTradeCheckEnabled && IsPreTradeCheckNeeded(PranaMessage) && int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AssetID].Value) != (int)AssetCategory.FX /*&& assetId != (int)AssetCategory.Forex && assetId != (int)AssetCategory.FX && assetId != (int)AssetCategory.FXOption && assetId != (int)AssetCategory.FXForward*/ )
                                {
                                    if (_enableTradeFlowLogging)
                                    {
                                        try
                                        {
                                            Logger.LoggerWrite("[Trade-Flow Out1] Before _preTradeService(MSGOrder) In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(PranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                        }
                                    }
                                    _preTradeService.ProcessOrder(PranaMessage);
                                    if (_enableTradeFlowLogging)
                                    {
                                        try
                                        {
                                            Logger.LoggerWrite("[Trade-Flow Out1] After _preTradeService(MSGOrder) In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(PranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                        }
                                    }
                                }
                                else
                                    HandleByPranaMessageType(PranaMessage, true);
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                            }
                            #endregion
                            if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType) && PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value.Equals(((int)OrderFields.PranaMsgTypes.ORDStaged).ToString()))
                            {
                                if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsUseCustodianBroker) &&
                                    bool.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsUseCustodianBroker].Value) &&
                                    isCreateSubMessage)
                                {
                                    CreateMultiSubForLiveAndManualOrders(PranaMessage);
                                }
                                else
                                {
                                    CreateSubForLiveAndManualOrders(PranaMessage, isCreateSubMessage);
                                }
                            }
                            break;
                        case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:
                            ServerCommonBusinessLogic.SetDateDetails(PranaMessage);
                            PranaMessage.MessageType = FIXConstants.MSGOrder;
                            PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrder;
                            PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value);
                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSendingTime, DateTimeConstants.GetCurrentTimeInFixFormat());

                            OrderCacheManager.AddToCache(PranaMessage);
                            HandleByPranaMessageType(PranaMessage, true);
                            break;

                        case FIXConstants.MSGOrderCancelRequest:
                        case FIXConstants.MSGOrderRollOverRequest:
                        case FIXConstants.MSGOrderCancelReplaceRequest:
                            ServerCommonBusinessLogic.SetDateDetails(PranaMessage);
                            clOrderID = UniqueIDGenerator.GetClOrderID();
                            bool isDispatchNeeded = true;
                            if (_isMultiBrokerWorkFlow || OrderCacheManager.HasMultiDayHistory(PranaMessage))
                            {
                                if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                                {
                                    //Kuldeep A.: Updating Staged OrderID of order.
                                    //This code is written to handle replace on third level childs. Third level cheld can't be replaced from our system. We may recive these replaces from middle EMS syetem.
                                    //So this code is handling incoming replace messages to mainatin linking and update third level childs accordingly 
                                    string stagedOrderId = PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                                    if (OrderCacheManager.StagedSubsCollection.ContainsKey(stagedOrderId) && OrderCacheManager.StagedSubsCollection[stagedOrderId].dictOrderIDWiseNewCLOrderIDs.ContainsKey(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value))
                                    {
                                        isDispatchNeeded = false;
                                        if (!string.IsNullOrEmpty(OrderCacheManager.StagedSubsCollection[stagedOrderId].parentClOrderID))
                                        {
                                            PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, OrderCacheManager.StagedSubsCollection[stagedOrderId].parentClOrderID);
                                        }
                                    }
                                    else
                                    {
                                        PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                                    }
                                }
                                else
                                {
                                    PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                                }
                            }
                            else
                            {
                                PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                            }
                            Logger.LoggerWrite("ClOrder used=" + clOrderID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);

                            _preTradeService.UpdateReplaceOrderAlerts(PranaMessage);

                            OrderCacheManager.UpdateReplaceOrder(PranaMessage);
                            OrderCacheManager.AddToCache(PranaMessage);
                            _preTradeService.CancelPendingComplianceApprovalTrades(PranaMessage);

                            //In case of Replace response Compliance will not check
                            if (isDispatchNeeded && PranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest && _isPreTradeCheckEnabled && IsPreTradeCheckNeeded(PranaMessage)/*&& assetId != (int)AssetCategory.Forex && assetId != (int)AssetCategory.FX && assetId != (int)AssetCategory.FXOption && assetId != (int)AssetCategory.FXForward*/ )
                            {
                                if (_enableTradeFlowLogging)
                                {
                                    try
                                    {
                                        Logger.LoggerWrite("[Trade-Flow Out1] Before _preTradeService(MSGOrderCancelReplaceRequest) In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(PranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                    }
                                }

                                if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                                {
                                    int nirvanaMsgType = int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                                    if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual)
                                    {
                                        PranaMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_MultiTradeMessage);
                                    }
                                }

                                PranaMessage message = new PranaMessage(PranaMessage.ToString());
                                _preTradeService.ProcessOrder(message);
                                if (_enableTradeFlowLogging)
                                {
                                    try
                                    {
                                        Logger.LoggerWrite("[Trade-Flow Out1] After _preTradeService(MSGOrderCancelReplaceRequest) In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(PranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                    }
                                }
                            }
                            else
                                HandleByPranaMessageType(PranaMessage, isDispatchNeeded);
                            break;

                        case FIXConstants.MSGExecution:
                        case FIXConstants.MSGOrderCancelReject:
                            if (PranaMessage.MessageType == FIXConstants.MSGExecution && !PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value.Equals(FIXConstants.ORDSTATUS_Rejected))
                            {
                                _preTradeService.AddOrUpdateStatusToOrderStatusTrackCache(clOrderID, ComplianceOrderStatus.Acknowledged);
                                _preTradeService.AddOrUpdateAcknowledgeOrderId(clOrderID, PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value.ToString());
                            }
                            bool isCxlRplOnChildOrder = true;
                            bool isMultiChildParentSub = false;
                            double multiDayCumQty = 0.0;
                            double multiDayAvgPx = 0.0;
                            PranaMessage afterTIFReplacePranaMessage = null;
                            #region Multi Broker Code Commented
                            if (_isMultiBrokerWorkFlow || OrderCacheManager.HasMultiDayHistory(PranaMessage))
                            {
                                if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                                {
                                    string orderStatus = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                                    if ((orderStatus == FIXConstants.ORDSTATUS_Replaced || orderStatus == FIXConstants.ORDSTATUS_Cancelled || orderStatus == FIXConstants.ORDSTATUS_RollOver || orderStatus == FIXConstants.ORDSTATUS_Rejected))
                                    {
                                        if (!isDropCopyOrder)
                                        {
                                            ValidateAndApplyCustomRulesFixOrders(PranaMessage, Direction.In);
                                            UpdateMultiBrokerTradeMessageCache(PranaMessage);
                                        }

                                        if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradeBlocked) && PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradeBlocked].Value == "1")
                                        {
                                            return;
                                        }

                                        bool isDayToMultiDayTIFReplace = OrderCacheManager.UpdateStagedSubCollectionOnRPL(PranaMessage, clOrderID);

                                        //First time Replace of a Day order to GTC order
                                        if (isDayToMultiDayTIFReplace && orderStatus == FIXConstants.ORDSTATUS_Replaced)
                                        {
                                            afterTIFReplacePranaMessage = PranaMessage.Clone();
                                            afterTIFReplacePranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PartiallyFilled);
                                            afterTIFReplacePranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_PartiallyFilled);
                                            HandleByFixMsgType(afterTIFReplacePranaMessage);
                                        }
                                        // In case of reject on internal orders, we need not to do anything.
                                        if (orderStatus == FIXConstants.ORDSTATUS_Rejected && OrderCacheManager.StagedSubsCollection.ContainsKey(clOrderID) && OrderCacheManager.StagedSubsCollection[clOrderID].dictOrderIDWiseNewCLOrderIDs.ContainsKey(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value))
                                        {
                                            isCxlRplOnChildOrder = true;
                                        }
                                        isCxlRplOnChildOrder = CheckAndProcessCXLRPLOnChildOrders(PranaMessage);
                                        if (isCxlRplOnChildOrder)
                                        {
                                            return;
                                        }
                                        //if Childs order count for sub order is greater that 0 then it is main order.
                                        if (IsMultiDayParentSub(PranaMessage))
                                        {
                                            isMultiChildParentSub = true;
                                        }
                                    }
                                }
                            }
                            #endregion

                            //Lets put the symbol, that was sent out, back in TAG 55 //SK JIRA PRANA-7340
                            if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                            {
                                //Get cached order can not return null with the passed CL OrderID, as execution has been already validated. //SK JIRA PRANA-7340
                                PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = OrderCacheManager.GetCachedOrderForExecutionReoprt(PranaMessage).FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value;
                            }

                            OrderCacheManager.UpdateExecutionReport(PranaMessage);
                            if (OrderCacheManager.HasMultiDayHistory(PranaMessage))
                                ServerCommonBusinessLogic.SetExpiryDateDetails(PranaMessage);

                            if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecID))
                            {
                                if (PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value == string.Empty)
                                {
                                    PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value = Guid.NewGuid().ToString();
                                }
                            }
                            else if (PranaMessage.MessageType.Equals(FIXConstants.MSGExecution) && !PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecID))
                            {
                                if (CachedDataManager.GetInstance.IsSendRealtimeManualOrderViaFix())
                                {
                                    List<PranaMessage> msgList = new List<PranaMessage>();
                                    PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGExecutionReport);
                                    msgList.Add(PranaMessage);
                                    FixEngineConnectionManager.FixEngineConnectionPoolManager.GetInstance().SendManualOrdersToThirdPartyFixLine(msgList);
                                }
                                return;
                            }

                            OrderCacheManager.UpdateCachedMessage(PranaMessage);
                            if (int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSub ||
                                int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSubChild ||
                                int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM)
                            {
                                // We're saving the actual OrderId for manual Acknowledgement.
                                string actualOrderId = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                                string parentClOrderID = string.Empty;

                                if (!isDropCopyOrder)
                                {
                                    ValidateAndApplyCustomRulesFixOrders(PranaMessage, Direction.In);
                                    UpdateMultiBrokerTradeMessageCache(PranaMessage);
                                }

                                if (OrderCacheManager.HasMultiDayHistory(PranaMessage))
                                {
                                    if (OrderInformation.IsExecutionMsg(PranaMessage))
                                    {
                                        string clOrderId = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                                        PranaMessage parentMessage = OrderCacheManager.GetCachedOrder(clOrderId);
                                        if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AssetID))
                                            PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AssetID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AssetID].Value);
                                        if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECID))
                                            PranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AUECID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                                        if (parentMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExpireTime))
                                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExpireTime, parentMessage.FIXMessage.ExternalInformation[FIXConstants.TagExpireTime].Value);
                                        ServerCommonBusinessLogic.SetDateDetails(PranaMessage);
                                        if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ParentClOrderID) && !string.IsNullOrEmpty(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value))
                                        {
                                            //While selecting the OrderID for Multi-Day child orders, we want a value which will remain unique throughout the life of the order.
                                            //ParentClorderID of any order is always unique so we are using it and appending date to it to get ChildOrder IDs
                                            parentClOrderID = parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value;
                                            parentClOrderID = parentClOrderID + "_" + Convert.ToDateTime(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECLocalDate].Value).ToString("yyyy/MM/dd").Replace("/", "");

                                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, parentClOrderID);
                                        }


                                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                                        {
                                            multiDayCumQty = Convert.ToDouble(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                                        }
                                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagAvgPx))
                                        {
                                            multiDayAvgPx = Convert.ToDouble(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagAvgPx].Value);
                                        }
                                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagDayCumQty))
                                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagDayCumQty].Value);
                                        if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagDayAvgPx))
                                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagDayAvgPx].Value);
                                    }
                                }

                                if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradeBlocked) && PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradeBlocked].Value == "1")
                                {
                                    return;
                                }
                                string orderStatus = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value;
                                bool hasMultiDayHistory = OrderCacheManager.HasMultiDayHistory(PranaMessage);
                                if (_isMultiBrokerWorkFlow || hasMultiDayHistory)
                                {
                                    PranaMessage orderMessage = null;
                                    string origClOrdId = PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID) ?
                                        PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value : string.Empty;
                                    //DropCopyFlow for BlotterExecution Report error is already handled with these code changes https://jira.nirvanasolutions.com:8443/browse/CI-6046
                                    //so no need to handle it here therefore !isDropCopyOrder is placed
                                    if (!isDropCopyOrder && hasMultiDayHistory && PranaMessage.MessageType == FIXConstants.MSGExecution && orderStatus == FIXConstants.ORDSTATUS_PartiallyFilled &&
                                           !OrderCacheManager.IsFillForClOrderIDPresent(clOrderID) && !OrderCacheManager.IsFillForClOrderIDPresent(origClOrdId))
                                    {
                                        PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, actualOrderId);
                                        CreateAndSendAckMessage(PranaMessage, clOrderID);
                                        PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, parentClOrderID);
                                    }
                                    orderMessage = OrderCacheManager.UpdateSubMessage(PranaMessage);
                                    if (orderMessage != null)
                                    {
                                        bool shouldDispatch = false;
                                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade)
                                            && bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade].Value))
                                        {
                                            // Dispatching so that we can add day-child entry in dictParentClOrderIDCollection in Client to establish great grand parent relation
                                            shouldDispatch = true;
                                        }
                                        SendAndSaveToDBQueue(orderMessage, shouldDispatch, isMultiChildParentSub);
                                    }
                                }
                            }
                            PranaMessage msgExecBroker = OrderCacheManager.UpdateSubMessageForExecBroker(PranaMessage);
                            if (msgExecBroker != null)
                            {
                                SendAndSaveToDBQueue(msgExecBroker, false, isMultiChildParentSub);
                            }
                            if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                            {
                                //CumQty = 0 this Condition is to handle the new state of Multi-day orders
                                if (OrderCacheManager.HasMultiDayHistory(PranaMessage)
                                    && (IsMultiDayParentSub(PranaMessage) || (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty) && Convert.ToDecimal(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) == 0)))
                                    isMultiChildParentSub = true;
                            }
                            SendAndSaveToDBQueue(PranaMessage, true, isMultiChildParentSub, multiDayCumQty, multiDayAvgPx, isDropCopyOrder);
                            break;

                        case FIXConstants.MSGTransferUser:
                            OrderCacheManager.ModifyOrderUserID(PranaMessage);
                            SendAndSaveToDBQueue(PranaMessage, true);
                            #region
                            // GTC\GTD orders handling.
                            Order order = Transformer.CreateOrder(PranaMessage);
                            string origClorderId = "";
                            string clorderId = order.ParentClOrderID;
                            string orderID = order.ParentClOrderID + "_" + order.AUECLocalDate.ToString("yyyy/MM/dd").Replace("/", "");
                            clorderId = PostTradeDataManager.GetClOrderIDFromParentClOrderID(clorderId);
                            if (OrderCacheManager.StagedSubsCollection.ContainsKey(clorderId) && OrderCacheManager.StagedSubsCollection[clorderId].dictOrderIDWiseNewCLOrderIDs.ContainsKey(orderID))
                            {
                                origClorderId = OrderCacheManager.StagedSubsCollection[clorderId].dictOrderIDWiseNewCLOrderIDs[orderID].clOrderID;
                            }
                            if (order.TIF == FIXConstants.TIF_GTD || order.TIF == FIXConstants.TIF_GTC || OrderCacheManager.StagedSubsCollection.ContainsKey(clorderId) || OrderCacheManager.StagedSubsCollection.ContainsKey(order.OrigClOrderID))
                            {
                                PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = clorderId;
                                OrderCacheManager.ModifyOrderUserID(PranaMessage);
                                SendAndSaveToDBQueue(PranaMessage, true);
                            }
                            #endregion
                            break;

                        //This is a Prana custom message type and not related to fix
                        case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                            clOrderID = UniqueIDGenerator.GetClOrderID();
                            PranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                            Logger.LoggerWrite("ClOrder used=" + clOrderID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                            SendAndSaveToDBQueue(PranaMessage, true);
                            break;

                        case FIXConstants.MSGReject:
                        case FIXConstants.MSGBusinessMessageReject:
                            PranaMessage fill = HandleBusinessRejectAndRejectMessages(PranaMessage);

                            if (fill != null)
                            {
                                SendAndSaveToDBQueue(fill, true);
                            }
                            break;

                        case FIXConstants.MSGOrderCancelRequestFroze:
                        case FIXConstants.MSGOrderCancelRequestUnFroze:
                            _preTradeService.FreezeUnfreezePendingComplianceApprovalTrades(PranaMessage);
                            break;

                        default:
                            throw new Exception("Msg Type not Recognised at MessageProcessor.HandleByFixMsgType()");
                    }
                }

                if (isMultiTradeIdCreated)
                {
                    SendMultiTradeDetails(pranaMessage, pranaMessages.Count);
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
        /// Sent the multitrade name and no. of orders
        /// </summary>
        /// <param name="multiTradeName"></param>
        /// <param name="sucessfullTrades"></param>
        public void SendMultiTradeDetails(PranaMessage pranaMessage, int sucessfullTrades)
        {
            try
            {
                PranaMessage msg = new PranaMessage();
                msg.MessageType = CustomFIXConstants.CUST_TAG_MultiTradeMessage;
                msg.FIXMessage.InternalInformation.AddField("MultiTradeId", pranaMessage.FIXMessage.InternalInformation.GetField(CustomFIXConstants.CUST_TAG_MultiTradeMessage));
                msg.FIXMessage.InternalInformation.AddField("SucessfullTrades", sucessfullTrades.ToString());
                msg.FIXMessage.InternalInformation.AddField("UserId", pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value);
                ProcessMessage(msg);
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
        /// For manually acknowledging executions
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <param name="clOrderID"></param>
        private void CreateAndSendAckMessage(PranaMessage pranaMessage, string clOrderID)
        {
            try
            {
                var pranaReqMsg = pranaMessage.Clone();
                pranaReqMsg.MessageType = FIXConstants.MSGOrder;
                pranaReqMsg.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrder;

                PranaMessage multidayACKMessage = null;
                if (pranaReqMsg != null)
                {
                    pranaReqMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                    pranaReqMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, "0");
                    multidayACKMessage = ManualMessageHandler.GenerateExecutionReport(pranaReqMsg);

                    /* In order to make sure that any later Fix message does not get a lesser sequence number, assigning the
                       first fill seq number to this 39=0 message. Note: Order Sequence assigning code is on another 
                       thread at the Fix connector level. Also, as per the Multi-day architecture, the 39=0 message will be associated with 
                       Parent order while 39 = 1 will be associated with child order so for a given ClorderID, the numbers won't override*/
                    if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrderSeqNumber))
                        multidayACKMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber,
                            pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrderSeqNumber].Value);
                }
                if (multidayACKMessage != null)
                {
                    multidayACKMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                    multidayACKMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, clOrderID);
                    multidayACKMessage.MessageType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value;
                    ServerCommonBusinessLogic.SetExpiryDateDetails(multidayACKMessage);
                    SendAndSaveToDBQueue(multidayACKMessage, true);
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
        /// To check if this is the main Sub in case of mult-day or multi-broker orders
        /// </summary>
        /// <param name="clOrderID"></param>
        /// <returns></returns>
        private bool IsMultiDayParentSub(PranaMessage pranaMessage)
        {
            try
            {
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    string clOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                    //if Childs order count for sub order is greater that 0 then it is main order.
                    if (OrderCacheManager.StagedSubsCollection.ContainsKey(clOrderID) && OrderCacheManager.StagedSubsCollection[clOrderID].dictOrderIDWiseNewCLOrderIDs.Count > 0)
                    {
                        return true;
                    }
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    string origClorderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                    //if Childs order count for sub order is greater that 0 then it is main order.
                    if (OrderCacheManager.StagedSubsCollection.ContainsKey(origClorderID) && OrderCacheManager.StagedSubsCollection[origClorderID].dictOrderIDWiseNewCLOrderIDs.Count > 0)
                    {
                        return true;
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
            return false;
        }

        private bool CheckAndProcessCXLRPLOnChildOrders(PranaMessage pranaMessage)
        {
            try
            {
                string clorderIDReceived = string.Empty;
                string orderIDReceived = string.Empty;

                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    clorderIDReceived = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                }
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                {
                    orderIDReceived = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                }

                if (!OrderCacheManager.StagedSubsCollection.ContainsKey(clorderIDReceived) || !OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs.ContainsKey(orderIDReceived))
                {
                    return false;
                }
                else
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID);

                    OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].origClOrderID = OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID;
                    if (OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID.Equals(OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].parentClOrderID))
                        OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].parentClOrderID = OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID;

                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, clorderIDReceived);

                    PranaMessage parentMessage = OrderCacheManager.GetCachedOrder(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value);

                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value);
                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CounterPartyID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value);
                    if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_Level1ID))
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Level1ID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value);
                    if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_Level2ID))
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Level2ID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level2ID].Value);
                    if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AssetID))
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AssetID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AssetID].Value);
                    if (parentMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECID))
                        pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AUECID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value);
                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_VenueID, parentMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_VenueID].Value);

                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.ORDNewSub).ToString());

                    string clOrderID = UniqueIDGenerator.GetClOrderID();
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagClOrdID, clOrderID);
                    OrderCacheManager.StagedSubsCollection[clorderIDReceived].dictOrderIDWiseNewCLOrderIDs[orderIDReceived].clOrderID = clOrderID;

                    PranaMessage requestMessage = pranaMessage.Clone();
                    if (requestMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Replaced)
                    {
                        requestMessage.MessageType = FIXConstants.MSGOrderCancelReplaceRequest;
                        requestMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrderCancelReplaceRequest);
                        requestMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingReplace);
                    }
                    else if (requestMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_Cancelled)
                    {
                        requestMessage.MessageType = FIXConstants.MSGOrderCancelRequest;
                        requestMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrderCancelRequest);
                        requestMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingCancel);
                    }
                    else if (requestMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value == FIXConstants.ORDSTATUS_RollOver)
                    {
                        requestMessage.MessageType = FIXConstants.MSGOrderRollOverRequest;
                        requestMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrderRollOverRequest);
                        requestMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingRollOver);
                    }

                    HandleByFixMsgType(requestMessage);
                    HandleByFixMsgType(pranaMessage);

                    return true;
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

        private void CreateSubForLiveAndManualOrders(PranaMessage pranaMessage, bool isCreateSubMessage)
        {
            try
            {
                bool isManualOrder = false;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsManualOrder) && bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsManualOrder].Value))
                {
                    isManualOrder = true;
                    //PranaMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_IsManualOrder);
                }

                if (isCreateSubMessage)
                {
                    PranaMessage subMessage = pranaMessage.Clone();
                    CreateSubOrder(subMessage, isManualOrder);
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
        /// Creates Multiple SubOrders For Live And Manual Orders
        /// </summary>
        /// <param name="PranaMessage"></param>
        private void CreateMultiSubForLiveAndManualOrders(PranaMessage PranaMessage)
        {
            try
            {
                Dictionary<int, List<int>> brokerWiseAccountMapping = new Dictionary<int, List<int>>();
                int level1ID = int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value);
                double subOrderCumQty = double.Parse(PranaMessage.FIXMessage.InternalInformation.GetField(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder));
                double totalQtyValue = double.Parse(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                string symbol = PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value;
                string companyUserID = PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
                var accountWiseExecutingBrokerMapping = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                int transactionSource = int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TransactionSourceTag].Value);
                StringBuilder newAllocationPrefIDs = new StringBuilder();
                List<PranaMessage> subMessages = new List<PranaMessage>();

                if (level1ID != int.MinValue)
                {
                    //if preference exists, then get allocation details from allocation service
                    AllocationOperationPreference aop = _allocationServices.GetPreferenceById(level1ID);
                    SerializableDictionary<int, AccountValue> allocatedAccountsTargetPercentage = null;
                    if (aop != null)
                    {
                        allocatedAccountsTargetPercentage = aop.TargetPercentage;
                        if (transactionSource == (int)TransactionSource.PST)
                        {
                            allocatedAccountsTargetPercentage = MultiBrokerOrderHelper.GetInstance.GetTargetPercentageForPTTOrder(aop, PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSide].Value);
                        }
                        foreach (int accountID in allocatedAccountsTargetPercentage.Keys)
                        {
                            int brokerID = accountWiseExecutingBrokerMapping[accountID];
                            if (!brokerWiseAccountMapping.ContainsKey(brokerID))
                            {
                                brokerWiseAccountMapping.Add(brokerID, new List<int>());
                            }
                            brokerWiseAccountMapping[brokerID].Add(accountID);
                        }
                    }
                    else
                    {
                        int brokerID = accountWiseExecutingBrokerMapping[level1ID];
                        brokerWiseAccountMapping.Add(brokerID, new List<int>());
                        brokerWiseAccountMapping[brokerID].Add(level1ID);
                    }

                    foreach (int brokerID in brokerWiseAccountMapping.Keys)
                    {
                        PranaMessage subMessage = PranaMessage.Clone();
                        int subMsglevel1ID = int.MinValue;
                        double subMsgQty = 0;
                        double subMsgSubOrderQty = 0;
                        if (brokerWiseAccountMapping.Keys.Count == 1)
                        {
                            subMsglevel1ID = level1ID;
                            subMsgSubOrderQty = subOrderCumQty;
                            subMsgQty = totalQtyValue;
                        }
                        else if (brokerWiseAccountMapping[brokerID].Count > 1 || transactionSource == (int)TransactionSource.PST)
                        {
                            var allocationOperationPreference = MultiBrokerOrderHelper.GetInstance.CreateAllocationOperationPreference(allocatedAccountsTargetPercentage, brokerWiseAccountMapping[brokerID], subOrderCumQty, out subMsgSubOrderQty, symbol, companyUserID, _allocationServices);
                            if (allocationOperationPreference != null)
                            {
                                subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginalLevel1ID].Value = allocationOperationPreference.OperationPreferenceId.ToString();
                                subMsglevel1ID = brokerWiseAccountMapping[brokerID].Count == 1 ? brokerWiseAccountMapping[brokerID][0] : allocationOperationPreference.OperationPreferenceId;
                                if (transactionSource == (int)TransactionSource.PST)
                                {
                                    newAllocationPrefIDs.Append(allocationOperationPreference.OperationPreferenceId).Append(',');
                                }
                            }
                            subMsgQty = MultiBrokerOrderHelper.GetInstance.DistributeQtyForCalculation(allocatedAccountsTargetPercentage, brokerWiseAccountMapping[brokerID], totalQtyValue);
                        }
                        else
                        {
                            subMsglevel1ID = brokerWiseAccountMapping[brokerID][0];
                            subMsgSubOrderQty = Math.Round(Convert.ToDouble(allocatedAccountsTargetPercentage[subMsglevel1ID].Value) * subOrderCumQty / 100, 10);
                            subMsgQty = Math.Round(Convert.ToDouble(allocatedAccountsTargetPercentage[subMsglevel1ID].Value) * totalQtyValue / 100, 10);
                            subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginalLevel1ID].Value = int.MinValue.ToString();
                        }

                        subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsUseCustodianBroker].Value = false.ToString();
                        subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value = brokerID.ToString();
                        subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Level1ID].Value = subMsglevel1ID.ToString();
                        subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CumQtyForSubOrder].Value = subMsgSubOrderQty.ToString();
                        subMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = subMsgQty.ToString();

                        subMessages.Add(subMessage);
                    }

                    bool isManualOrder = false;
                    if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsManualOrder) && bool.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsManualOrder].Value))
                    {
                        isManualOrder = true;
                    }

                    if (newAllocationPrefIDs.Length > 0)
                    {
                        newAllocationPrefIDs.Remove(newAllocationPrefIDs.Length - 1, 1);
                        ServerDataManager.SavePTTAllocationMapping(level1ID, newAllocationPrefIDs.ToString());
                    }

                    foreach (PranaMessage subMessage in subMessages)
                    {
                        CreateSubOrder(subMessage, isManualOrder);
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

        private void CreateSubOrder(PranaMessage subMessage, bool isManualOrder)
        {
            try
            {
                if (isManualOrder)
                {
                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value = ((int)OrderFields.PranaMsgTypes.ORDManualSub).ToString();
                }
                else
                {
                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value = ((int)OrderFields.PranaMsgTypes.ORDNewSub).ToString();
                    subMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingNew);
                }

                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID))
                {
                    subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value = subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value;
                }
                else
                {
                    subMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_StagedOrderID, subMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value);
                }

                if (!isManualOrder)
                    UpdateLiveOrderFields(subMessage);


                if (isManualOrder && subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder))
                {
                    if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                    {
                        string quantityValue = subMessage.FIXMessage.InternalInformation.GetField(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder);
                        if (!String.IsNullOrEmpty(quantityValue))
                        {
                            subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagCumQty);
                            subMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, quantityValue);
                        }
                    }
                }
                HandleByFixMsgType(subMessage);
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

        private void UpdateLiveOrderFields(PranaMessage subMessage)
        {
            try
            {
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagClOrdID);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagOrderID);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSendingTime))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagSendingTime);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagCumQty);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecID))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagExecID);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecTransType))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagExecTransType);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLeavesQty))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagLeavesQty);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLastShares))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagLastShares);
                }
                if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagLastPx))
                {
                    subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagLastPx);
                }
                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ParentClOrderID))
                {
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_ParentClOrderID);
                }
                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECLocalDate))
                {
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_AUECLocalDate);
                }
                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ProcessDate))
                {
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_ProcessDate);
                }
                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OriginalPurchaseDate))
                {
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_OriginalPurchaseDate);
                }
                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_SettlementDate))
                {
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_SettlementDate);
                }
                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OrderSeqNumber))
                {
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_OrderSeqNumber);
                    subMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OrderSeqNumber, UniqueIDGenerator.GetOrderSeqNumber());
                }

                if (subMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder))
                {
                    if (subMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderQty))
                    {
                        string quantityValue = subMessage.FIXMessage.InternalInformation.GetField(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder);
                        if (!String.IsNullOrEmpty(quantityValue))
                        {
                            subMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagOrderQty);
                            subMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderQty, quantityValue);
                        }
                    }
                    subMessage.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_CumQtyForSubOrder);
                }

                //TODO: Same code is in MessageEngine.cs class. Need to combine this code
                //update allocation tag fields
                bool sendAllocationsViaFix = CommonDataCache.CachedDataManager.GetSendAllocationsViaFix();
                //if level1Id is provided then get allocation details and add custom tags based on allocation in accounts, PRANA-27196
                if (sendAllocationsViaFix && subMessage.FIXMessage.InternalInformation.MessageFields.Exists(x => x.Tag.Equals(CustomFIXConstants.CUST_TAG_Level1ID)))
                {
                    int level1ID = Convert.ToInt32(subMessage.FIXMessage.InternalInformation.MessageFields.Find(x => x.Tag.Equals(CustomFIXConstants.CUST_TAG_Level1ID)).Value);
                    if (level1ID != int.MinValue)
                    {
                        //if preference exists, then get allocation details from allocation service, else add total quantity in custom account value tag
                        string preferenceName = _allocationServices.GetAllocationPreferenceNameById(level1ID);
                        if (!string.IsNullOrWhiteSpace(preferenceName) && !preferenceName.Equals("Manual"))
                        {
                            Order order = Transformer.CreateOrder(subMessage);
                            order.CumQty = order.Quantity;
                            AllocationGroup group = _allocationServices.CreateVirtualAllocationGroup(order, false);
                            if (group.Allocations != null && group.Allocations.Collection != null && group.Allocations.Collection.Count > 0)
                            {
                                foreach (AllocationLevelClass alloc in group.Allocations.Collection)
                                {
                                    subMessage.FIXMessage.InternalInformation.AddField(GetInternalTagFromAccountId(alloc.LevelnID), alloc.AllocatedQty.ToString());
                                }
                            }
                        }
                        else
                        {
                            subMessage.FIXMessage.InternalInformation.AddField(GetInternalTagFromAccountId(level1ID), subMessage.FIXMessage.ExternalInformation.MessageFields.Find(x => x.Tag.Equals(FIXConstants.TagOrderQty)).Value);
                        }
                    }
                    List<int> swapAccounts = CommonDataCache.CachedDataManager.GetSwapAccounts();
                    foreach (int accountId in swapAccounts)
                    {
                        string accountTag = GetInternalTagFromAccountId(accountId);
                        if (!subMessage.FIXMessage.InternalInformation.ContainsKey(accountTag))
                            subMessage.FIXMessage.InternalInformation.AddField(accountTag, "0");
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
        /// Gets the internal tag from account identifier.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        private static string GetInternalTagFromAccountId(int accountId)
        {
            string customTag = string.Empty;
            try
            {
                customTag = "Account_" + accountId.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return customTag;
        }
        #region Compliance methods
        bool _isPreTradeCheckEnabled = false;

        /// <summary>
        /// Checking if pre-trade rules need to be checked
        /// </summary>
        /// <param name="pranaMessage">Message to be checked</param>
        /// <returns>true if needs to be checked otherwise false</returns>
        private bool IsPreTradeCheckNeeded(PranaMessage pranaMessage)
        {
            bool ret = false;
            try
            {

                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                {
                    int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);

                    if (nirvanaMsgType == (int)OrderFields.PranaMsgTypes.ORDStaged
                        && pranaMessage.MessageType != FIXConstants.MSGOrderCancelReplaceRequest)
                        return false;
                    
                    if (int.TryParse(pranaMessage.UserIDForCompliance, out int userIdForCompliance))
                    {
                        switch (nirvanaMsgType)
                        {
                            case (int)OrderFields.PranaMsgTypes.ORDManual:
                            case (int)OrderFields.PranaMsgTypes.ORDManualSub:
                                if (ComplianceCacheManager.GetPreTradeCheck(userIdForCompliance))
                                {
                                    ret = ComplianceCacheManager.GetApplyToManualPermission(userIdForCompliance);
                                }
                                break;
                            case (int)OrderFields.PranaMsgTypes.InternalOrder:
                            case (int)OrderFields.PranaMsgTypes.ORDNewSub:
                            case (int)OrderFields.PranaMsgTypes.ORDNewSubChild:
                                ret = ComplianceCacheManager.GetPreTradeCheck(userIdForCompliance);
                                break;
                            case (int)OrderFields.PranaMsgTypes.ORDStaged:
                                ret = ComplianceCacheManager.GetPreTradeCheckStaging(userIdForCompliance);
                                break;
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

            return ret;
        }
        #endregion

        /// <summary>
        /// Send And Save To DBQueue
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <param name="dispatchNeeded"></param>
        /// <param name="isMultiBroker"></param>
        /// <param name="multiDayCumQty">Overall CumQty in case of Multi-Day orders</param>
        /// <param name="multiDayAvgPx">Overall AvgPx in case of Multi-Day orders</param>
        private void SendAndSaveToDBQueue(PranaMessage PranaMessage, bool dispatchNeeded, bool isMultiBroker = false, double multiDayCumQty = 0.0, double multiDayAvgPx = 0.0, bool isDropCopyOrder = false)
        {
            try
            {
                //Setting the variable to true for Multi-ChildSub orders
                if (PranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType)
                    && (int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) != (int)OrderFields.PranaMsgTypes.ORDNewSubChild && int.Parse(PranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) != (int)OrderFields.PranaMsgTypes.ORDStaged)
                    && OrderCacheManager.HasMultiDayHistory(PranaMessage)
                    && (IsMultiDayParentSub(PranaMessage) || (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty) && Convert.ToDecimal(PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) == 0)) && PranaMessage.MessageType != FIXConstants.MSGTransferUser)
                    isMultiBroker = true;

                // SK 20100416, JIRA issues NEWLAND-260
                // Group is also created from dispatcher by a call to CreateAndSendTaxlots method. Follow the dispatch method of order processor.
                // However race conditions are happening and at times. Sometimes the time dispatcher takes to place allocation message
                // in dbqueue for new order next fill is processced and group is not updated. It doesn't affect the preallocation or 
                // auto allocation using fix rules as in those cases every execution is updated DB using pranataxlotcachemanager. Where as 
                // in case of unallocated trades saveresponse sp updates group when subsequent fills arrive.
                // Added following if condition of add group for new order as soon as trade is placed in db queue.

                QueueMessage qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", PranaMessage, isMultiBroker);
                _dbQueue.SendMessage(qPersisted);

                #region Send order to Web application
                PublishOrderUpdatesForWeb(PranaMessage);
                #endregion
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow8] Data sent to DBQueue In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(PranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }
                if (dispatchNeeded)
                {
                    _queueDispatching.SendMessage(PranaMessage);
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow8] Data published to client side In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(PranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    }
                }
                //Before sending Multi-Day Orders,the PranaMessage received from the broker is being reverted to its original form i.e all the handling done
                //for Multi-day while trade processing is being reverted so that the in-market cache is correctly formed
                PranaMessage pranaMessageSendToEsper = PranaMessage.Clone();
                if (pranaMessageSendToEsper.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagClOrdID)
                    && pranaMessageSendToEsper.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_StagedOrderID)
                    && pranaMessageSendToEsper.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType)
                    && int.Parse(pranaMessageSendToEsper.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value)
                    == (int)OrderFields.PranaMsgTypes.ORDNewSubChild
                    && OrderInformation.IsExecutionMsg(pranaMessageSendToEsper) && OrderCacheManager.HasMultiDayHistory(pranaMessageSendToEsper))
                {
                    pranaMessageSendToEsper.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = pranaMessageSendToEsper.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_StagedOrderID].Value;
                    pranaMessageSendToEsper.FIXMessage.InternalInformation.RemoveField(CustomFIXConstants.CUST_TAG_StagedOrderID);
                    pranaMessageSendToEsper.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value = ((int)OrderFields.PranaMsgTypes.ORDNewSub).ToString();
                    if (multiDayCumQty != 0.0)
                        pranaMessageSendToEsper.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, multiDayCumQty.ToString());
                    if (multiDayAvgPx != 0.0)
                        pranaMessageSendToEsper.FIXMessage.ExternalInformation.AddField(FIXConstants.TagAvgPx, multiDayAvgPx.ToString());
                }
                //For Multi-Day orders need to block this as the current MsgType of the child orders would be 14 (NewORDSubChild)
                if (!isDropCopyOrder)
                    SendWorkingQuantityToEsper(pranaMessageSendToEsper);

                if(isDropCopyOrder && _enableTradeFlowLogging)
                {
                    string symbol = string.Empty;
                    if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                        symbol = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value.ToString();
                    string msgType = string.Empty;
                    if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMsgType))
                        msgType = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value.ToString();
                    string securityType = string.Empty;
                    if (PranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSecurityType))
                        securityType = PranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSecurityType].Value.ToString();

                    string logMsg = "DropCopy order details: Symbol: " + symbol + ", Message Type: " +msgType + ", Security Type: "+ securityType;
                    Logger.LoggerWrite(logMsg, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
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
        /// Either send data to database or update the client
        /// </summary>
        /// <param name="pranaMessage"></param>
        /// <param name="dispatchNeeded"></param>
        /// <param name="saveInDb"></param>
        /// <param name="isMultiBroker"></param>
        private void SendOrSaveToDBQueue(PranaMessage pranaMessage, bool dispatchNeeded, bool saveInDb, bool isMultiBroker = false)
        {
            try
            {
                if (saveInDb)
                {
                    if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType)
                    && (int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) != (int)OrderFields.PranaMsgTypes.ORDNewSubChild && int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) != (int)OrderFields.PranaMsgTypes.ORDStaged)
                    && OrderCacheManager.HasMultiDayHistory(pranaMessage)
                    && (IsMultiDayParentSub(pranaMessage) || (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty) && Convert.ToDecimal(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) == 0)))
                        isMultiBroker = true;

                    QueueMessage qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMessage, isMultiBroker);
                    _dbQueue.SendMessage(qPersisted);

                    #region Send order to Web application
                    PublishOrderUpdatesForWeb(pranaMessage);
                    #endregion
                }
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow8] Data sent to DBQueue In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(pranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }
                if (dispatchNeeded && !saveInDb)
                {
                    _queueDispatching.SendMessage(pranaMessage);
                    if (_enableTradeFlowLogging)
                    {
                        try
                        {
                            Logger.LoggerWrite("[Trade-Flow8] Data published to client side In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(pranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                        }
                    }
                }
                if (saveInDb)
                    SendWorkingQuantityToEsper(pranaMessage);
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
        /// Sends working Quantity to esper based on permission
        /// </summary>
        /// <param name="pranaMessage"></param>
        private void SendWorkingQuantityToEsper(PranaMessage pranaMessage)
        {
            try
            {
                int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                if ((_isPreTradeCheckEnabled || CachedDataManager.GetInstance.GetIsMarketDataPermissionEnabledForTradingRules()) && nirvanaMsgType != (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM && nirvanaMsgType != (int)OrderFields.PranaMsgTypes.ORDNewSubChild)
                {
                    bufferBlock.Post(pranaMessage);
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

        async System.Threading.Tasks.Task<PranaMessage> ConsumeBufferMessageAsync(IReceivableSourceBlock<PranaMessage> source)
        {
            try
            {
                while (await source.OutputAvailableAsync())
                {
                    PranaMessage pranaMessage;
                    while (source.TryReceive(out pranaMessage))
                    {
                        if (pranaMessage != null)
                        {
                            if (_enableTradeFlowLogging)
                            {
                                try
                                {
                                    Logger.LoggerWrite("[Trade-Flow9] Before SendInTradeToEsper In NirvanaOrderProcessor, Fix Message: " + Convert.ToString(pranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                }
                            }
                            PranaMessage message = new PranaMessage(pranaMessage.ToString());
                            List<PranaMessage> pranaMessageList = new List<PranaMessage>();
                            pranaMessageList.Add(message);
                            _preTradeService.SendInTradeToEsper(pranaMessageList, false);
                            if (_enableTradeFlowLogging)
                            {
                                try
                                {
                                    Logger.LoggerWrite("[Trade-Flow9] After SendInTradeToEsperIn NirvanaOrderProcessor, Fix Message: " + Convert.ToString(pranaMessage.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
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

            return null;
        }

        private void HandleByPranaMessageType(PranaMessage pranaMessage, bool isDispatchNeeded)
        {
            try
            {
                if (!pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                {
                    return;
                }

                int nirvanaMsgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                switch (nirvanaMsgType)
                {
                    case (int)OrderFields.PranaMsgTypes.ORDStaged:
                    case (int)OrderFields.PranaMsgTypes.ORDManual:
                    case (int)OrderFields.PranaMsgTypes.ORDManualSub:
                        PranaMessage fillPranaMessage = null;
                        switch (pranaMessage.MessageType)
                        {
                            case FIXConstants.MSGOrderCancelReplaceRequest:
                                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText) && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagText].Value.Equals(PreTradeConstants.MsgTradePending))
                                    pranaMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagText);
                                fillPranaMessage = ManualMessageHandler.GenerateReplacedReport(pranaMessage);
                                SendAndSaveToDBQueue(pranaMessage, true);
                                SendAndSaveToDBQueue(fillPranaMessage, true);
                                break;

                            case FIXConstants.MSGOrderCancelRequest:
                                fillPranaMessage = ManualMessageHandler.GenerateCancelledReport(pranaMessage);
                                SendAndSaveToDBQueue(pranaMessage, true);
                                SendAndSaveToDBQueue(fillPranaMessage, true);
                                break;

                            case FIXConstants.MSGOrderRollOverRequest:
                                fillPranaMessage = ManualMessageHandler.GenerateRolloverReport(pranaMessage);
                                SendAndSaveToDBQueue(pranaMessage, true);
                                SendAndSaveToDBQueue(fillPranaMessage, true);
                                break;

                            case FIXConstants.MSGOrder:
                                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DCStageWorkFlow) && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DCStageWorkFlow].Value == "StageAndSendAck")
                                    pranaMessage.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TransactionSourceTag, "1");
                                fillPranaMessage = ManualMessageHandler.GenerateExecutionReport(pranaMessage);
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, "0");
                                fillPranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, fillPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value.ToString());
                                SendAndSaveToDBQueue(pranaMessage, true);
                                if (nirvanaMsgType.Equals((int)OrderFields.PranaMsgTypes.ORDManualSub))
                                {
                                    OrderCacheManager.UpdateCachedMessage(fillPranaMessage);
                                }
                                SendAndSaveToDBQueue(fillPranaMessage, true);
                                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DCStageWorkFlow) && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DCStageWorkFlow].Value == "StageAndSendAck")
                                {
                                    SendAckMessageForStageOrder(pranaMessage, fillPranaMessage);
                                }
                                break;

                            case FIXConstants.MSGOrderCancelReject:
                                bool isMultiChildParentSub = false;
                                if (OrderCacheManager.HasMultiDayHistory(pranaMessage)
                                    && (IsMultiDayParentSub(pranaMessage) || (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty) && Convert.ToDecimal(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value) == 0)))
                                    isMultiChildParentSub = true;
                                PranaMessage originalMessage = new PranaMessage(pranaMessage.ToString());
                                fillPranaMessage = ManualMessageHandler.GenerateReplacedCancelledReport(originalMessage);
                                originalMessage.MessageType = FIXConstants.MSGOrderCancelReplaceRequest;
                                originalMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrderCancelReplaceRequest;
                                SendAndSaveToDBQueue(originalMessage, true, isMultiChildParentSub);

                                if (fillPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                                {
                                    //Get cached order can not return null with the passed CL OrderID, as execution has been already validated. //SK JIRA PRANA-7340
                                    fillPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = OrderCacheManager.GetCachedOrderForExecutionReoprt(fillPranaMessage).FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value;
                                }

                                OrderCacheManager.UpdateExecutionReport(fillPranaMessage);
                                if (fillPranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecID))
                                {
                                    if (fillPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value == string.Empty)
                                    {
                                        fillPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value = Guid.NewGuid().ToString();
                                    }
                                }

                                OrderCacheManager.UpdateCachedMessage(fillPranaMessage);
                                if (int.Parse(fillPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSub ||
                                    int.Parse(fillPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDNewSubChild)
                                {
                                    if (_isMultiBrokerWorkFlow)
                                    {
                                        PranaMessage orderMessage = null;
                                        orderMessage = OrderCacheManager.UpdateSubMessage(fillPranaMessage);
                                        if (orderMessage != null)
                                        {
                                            SendAndSaveToDBQueue(orderMessage, false);
                                        }
                                    }
                                }
                                SendAndSaveToDBQueue(fillPranaMessage, true, isMultiChildParentSub);
                                break;
                        }
                        break;

                    case (int)OrderFields.PranaMsgTypes.InternalOrder:
                        if (ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.Out))
                        {
                            SendAndSaveToDBQueue(pranaMessage, true);
                        }
                        break;

                    case (int)OrderFields.PranaMsgTypes.BasketOrder:
                        ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.Out);
                        break;

                    case (int)OrderFields.PranaMsgTypes.ORDNewSub:
                    case (int)OrderFields.PranaMsgTypes.ORDNewSubChild:
                        if (pranaMessage.MessageType == FIXConstants.MSGOrderRollOverRequest)
                        {
                            PranaMessage fillPranaMessageRollover = ManualMessageHandler.GenerateRolloverReport(pranaMessage);
                            SendAndSaveToDBQueue(pranaMessage, true);
                            SendAndSaveToDBQueue(fillPranaMessageRollover, true);
                        }
                        else
                        {
                            string status = string.Empty;
                            if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText))
                                status = pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagText);
                            if (pranaMessage.MessageType == FIXConstants.MSGOrderCancelRequest && status.Equals(PreTradeConstants.MsgTradePending))
                            {
                                ComplianceOrderStatus _orderStatus = ComplianceOrderStatus.None;
                                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                                    _orderStatus = _preTradeService.GetOrderStatusFromOrderStatusTrackCache(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value);
                                if (_orderStatus == ComplianceOrderStatus.Acknowledged)
                                {
                                    pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value = _preTradeService.GetAcknowledgedClOrderId(pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagOrderID));
                                    pranaMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagText);
                                    pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = pranaMessage.FIXMessage.ExternalInformation.GetField(FIXConstants.TagDayOrderQty);
                                    if (ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.Out))
                                    {
                                        SendAndSaveToDBQueue(pranaMessage, isDispatchNeeded);
                                    }
                                }
                                else
                                {
                                    fillPranaMessage = ManualMessageHandler.GenerateCancelledReport(pranaMessage);
                                    SendOrSaveToDBQueue(pranaMessage, true, true);
                                    SendAndSaveToDBQueue(fillPranaMessage, true);
                                }
                            }
                            else if (pranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest && status.Equals(PreTradeConstants.MsgTradePending))
                            {
                                pranaMessage.FIXMessage.ExternalInformation.RemoveField(FIXConstants.TagText);
                                PranaMessage originalMessage = new PranaMessage(pranaMessage.ToString());
                                pranaMessage.MessageType = FIXConstants.MSGOrder;
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrder);
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, PreTradeConstants.MSG_PENDING_COMPLIANCE_APPROVAL_CHANGE_ORDER_STATUS);
                                OrderCacheManager.UpdateChildDetailsFromParent(pranaMessage);
                                if (ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.Out))
                                {
                                    SendOrSaveToDBQueue(pranaMessage, isDispatchNeeded, false);
                                    SendOrSaveToDBQueue(originalMessage, isDispatchNeeded, true);
                                }
                            }
                            else
                            {
                                OrderCacheManager.UpdateChildDetailsFromParent(pranaMessage);
                                if (ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.Out))
                                {
                                    SendAndSaveToDBQueue(pranaMessage, isDispatchNeeded);
                                    SendUpdateToMultiDayChildOrder(pranaMessage);
                                }
                            }
                        }
                        break;

                    //As call to process self created CXL RPCL and New orders is delegated to Order processor. Handle that call here SK 20100204
                    case (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM:
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DCStageWorkFlow) && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DCStageWorkFlow].Value == "StageAndSendAck")
                        {
                            return;
                        }
                        SendAndSaveToDBQueue(pranaMessage, true);
                        break;

                    default:
                        throw new Exception("Msg Type not Recognised at MessageProcessor.HandleByPranaMessageType()");
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
        /// This method is for sending udpated values to child order of multi day order
        /// </summary>
        /// <param name="pranaMessage"></param>
        private void SendUpdateToMultiDayChildOrder(PranaMessage pranaMessage)
        {
            try
            {
                if (pranaMessage.MessageType == FIXConstants.MSGOrderCancelReplaceRequest && OrderCacheManager.HasMultiDayHistory(pranaMessage))
                {
                    PranaMessage childPranaMessage = null;
                    var parentClOrderId = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ParentClOrderID].Value;
                    MultiBrokerChildOrders childOrder = null;
                    if (OrderCacheManager.StagedSubsCollection.ContainsKey(parentClOrderId) && OrderCacheManager.StagedSubsCollection[parentClOrderId].dictOrderIDWiseNewCLOrderIDs != null && OrderCacheManager.StagedSubsCollection[parentClOrderId].dictOrderIDWiseNewCLOrderIDs.Count > 0)
                    {
                        childOrder = OrderCacheManager.StagedSubsCollection[parentClOrderId].dictOrderIDWiseNewCLOrderIDs.Last().Value;
                    }
                    else if (!string.IsNullOrEmpty(OrderCacheManager.GetMultiDayOrderReplacedClOrderId(parentClOrderId))
                        && OrderCacheManager.StagedSubsCollection.ContainsKey(OrderCacheManager.GetMultiDayOrderReplacedClOrderId(parentClOrderId))
                        && OrderCacheManager.StagedSubsCollection[OrderCacheManager.GetMultiDayOrderReplacedClOrderId(parentClOrderId)].dictOrderIDWiseNewCLOrderIDs != null
                        && OrderCacheManager.StagedSubsCollection[OrderCacheManager.GetMultiDayOrderReplacedClOrderId(parentClOrderId)].dictOrderIDWiseNewCLOrderIDs.Count > 0)
                    {
                        childOrder = OrderCacheManager.StagedSubsCollection[OrderCacheManager.GetMultiDayOrderReplacedClOrderId(parentClOrderId)].dictOrderIDWiseNewCLOrderIDs.Last().Value;
                    }

                    if (childOrder != null)
                    {
                        childPranaMessage = OrderCacheManager.GetCachedOrder(childOrder.clOrderID);
                        double leavesQty = Convert.ToDouble(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value) - childOrder.CumQty;
                        childPranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Replaced);
                        childPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value;
                        childPranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagLeavesQty, leavesQty.ToString());
                        childPranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCumQty, childOrder.CumQty.ToString());
                        childPranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGExecutionReport);
                        childPranaMessage.MessageType = childPranaMessage.MessageType == String.Empty ? FIXConstants.MSGExecutionReport : childPranaMessage.MessageType;
                        SendAndSaveToDBQueue(childPranaMessage, true);
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

        private void SendAckMessageForStageOrder(PranaMessage pranaMessage, PranaMessage fillPranaMessage)
        {
            try
            {
                PranaMessage ackPranaMessage = fillPranaMessage.Clone();
                //This is done to optimize the flow at PranaOrderDispatcher. We get frequest message exutions (FIXConstants.MSGExecution) so setting FIXConstants.MSGOrder will reduce the check of DCstageOrderFlow. 
                ackPranaMessage.MessageType = FIXConstants.MSGOrder;
                //By Default, Nirvana is appending AUECdate(YYYYMMDD) in the orderID. So string need to be trim to get orginal TAG 11 value.
                string OriginalCLOrderID = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value.ToString();
                OriginalCLOrderID = OriginalCLOrderID.Remove(OriginalCLOrderID.Length - 9);
                ackPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = OriginalCLOrderID;
                ackPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value = OriginalCLOrderID;
                ackPranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value = OriginalCLOrderID + ":0:Ack";
                ValidateAndApplyCustomRulesFixOrders(ackPranaMessage, Direction.Out);
                //Based on 100154=SendAck, PranaOrderDispatcher will send ack to the couterparty
                ackPranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DCStageWorkFlow].Value = "SendAck";
                _queueDispatching.SendMessage(ackPranaMessage);
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

        private bool ValidateAndApplyCustomRulesFixOrders(PranaMessage pranaMessage, Direction direction)
        {
            try
            {
                string returnMessage = FixMessageValidator.ValidateMessage(pranaMessage);
                if (returnMessage != string.Empty)
                {
                    PranaMessage rejectMessage = ManualMessageHandler.GenerateExecutionReport(pranaMessage);
                    rejectMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Rejected);
                    rejectMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, "Message not valid according to Fix Dictionary Rules!  Error:= " + returnMessage);
                    QueueMessage qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMessage);
                    _dbQueue.SendMessage(qPersisted);
                    SendAndSaveToDBQueue(rejectMessage, true);
                    Logger.HandleException(new Exception("Message not valid according to Fix Dictionary Rules!  Error:= " + returnMessage + " Message :=" + pranaMessage.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
                    return false;
                }
                else
                {
                    PranaCustomMapper.ApplyRules(pranaMessage, direction);
                    return true;
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

        public List<PranaMessage> GetCachedErrorOrders()
        {
            return new List<PranaMessage>();
        }

        public PranaMessage HandleBusinessRejectAndRejectMessages(PranaMessage pranaMessage)
        {
            PranaMessage fill = null;
            try
            {
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagRefSeqNum))
                {
                    string RefSeqNum = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagRefSeqNum].Value;
                    PranaMessage cachedPranaMessage = OrderCacheManager.GetCachedOrderByMsgSeqNumber(RefSeqNum);
                    if (cachedPranaMessage != null)
                    {
                        fill = cachedPranaMessage.Clone();
                        fill.MessageType = FIXConstants.MSGExecutionReport;
                        fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGExecutionReport);
                        fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecID, System.Guid.NewGuid().ToString());
                        fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecTransType, FIXConstants.EXECTYPE_New);
                        fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Rejected);
                        // copy reject message
                        if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagText))
                        {
                            fill.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagText].Value);
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
            return fill;
        }

        public static void FillStagedSubsCollection()
        {
            try
            {
                Dictionary<string, MultiBrokersSubsCollection> dict = CacheManagerDAL.FillStagedSubsCollection();
                OrderCacheManager.StagedSubsCollection = dict;
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

        public void SaveCachedErrorOrders()
        {

        }

        public List<PranaMessage> GetAllCachedMessages()
        {
            return OrderCacheManager.GetCachedMessages();
        }

        /// <summary>
        /// This method is to setting executing broker in Tag 76 when the trade is coming from OTD broker
        /// </summary>
        /// <param name="pranaMessage"></param>
        private void UpdateMultiBrokerTradeMessageCache(PranaMessage pranaMessage)
        {
            try
            {
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade)
                        && bool.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsMultiBrokerTrade].Value))
                {
                    OrderCacheManager.AddToExecBrokerCache(pranaMessage);
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

        private string _name = "Order";
        public string Name
        {
            get { return _name; }
        }

        private ISecMasterServices _secMasterServices;

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_connectionUnAvailableQueue != null)
                    {
                        _connectionUnAvailableQueue.Dispose();
                        _connectionUnAvailableQueue = null;
                    }
                    if (_proxyPublishing != null)
                        _proxyPublishing.Dispose();
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

        #region IProcessingUnit Members
        public Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages()
        {
            return new Dictionary<string, List<PranaMessage>>();
        }
        #endregion
    }
}