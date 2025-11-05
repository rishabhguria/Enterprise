using Prana.BusinessObjects;
using Prana.BusinessObjects.EventArguments;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.DropCopyProcessor_PostTrade;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTradeServices;
using Prana.PostTradeServices.RollOver;
using Prana.QueueManager;
using Prana.ServerCommon;
using Prana.SocketCommunication;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.MessageProcessor
{
    public class MessageEngine : IDisposable
    {
        IQueueProcessor _inTradeComMgrQueue;
        IQueueProcessor _outTradeComMgrQueue;

        ITradeQueueProcessor _inFixMgrQueue;
        ITradeQueueProcessor _outFixMgrQueue;
        ITradeQueueProcessor _queueDispatching;

        IQueueProcessor _dbQueue;
        IQueueProcessor _errorQueue;

        IAllocationServices _allocationServices = null;
        IAllocationManager _allocationManager = null;
        ISecMasterServices _secMasterServices;
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));

        private static MessageEngine _messageEngine;

        public static MessageEngine GetInstance()
        {
            if (_messageEngine == null)
            {
                _messageEngine = new MessageEngine();
            }
            return _messageEngine;
        }

        private MessageEngine()
        {

        }

        public void Initilise(IQueueProcessor inTradeComMgrQueue, IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor inFixMgrQueue, ITradeQueueProcessor outFixMgrQueue, IQueueProcessor dbQueue, IQueueProcessor errorQueue, IAllocationServices allocationServices, ISecMasterServices secMasterServices, IAllocationManager allocationManager)
        {
            try
            {
                FixDictionaryHelper.LoadFixDictionary();
                OrderProcessor.PranaOrderProcessor.FillStagedSubsCollection();
                _secMasterServices = secMasterServices;
                _allocationServices = allocationServices;
                _allocationManager = allocationManager;
                _queueDispatching = new TradeQueueManager(this);
                _inTradeComMgrQueue = inTradeComMgrQueue;
                _outTradeComMgrQueue = outTradeComMgrQueue;
                _inFixMgrQueue = inFixMgrQueue;
                _outFixMgrQueue = outFixMgrQueue;
                _dbQueue = dbQueue;
                _errorQueue = errorQueue;
                _inTradeComMgrQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_inTradeComMgrQueue_MessageQueued);
                _inFixMgrQueue.MessageQueued += new EventHandler<EventArgs<PranaMessage>>(_inFixMgrQueue_MessageQueued);
                _queueDispatching.MessageQueued += new EventHandler<EventArgs<PranaMessage>>(_queueDispatching_MessageQueued);

                CustomMapper.PranaCustomMapper.LoadDictionary();

                ProcessorFactory.GetInstance.Initlise(_queueDispatching, _dbQueue, _allocationServices, _secMasterServices);
                DispatcherFactory.GetInstance.Initlise(_outTradeComMgrQueue, _outFixMgrQueue);
                ProcessorFactory.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleErrorMessages);
                DispatcherFactory.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleErrorMessages);
                PostTradeServicesServer.GetInstance.GetErrorMsgEvent += new PostTradeServicesServer.GetErrorMsgEventHandler(MessageEngine_GetErrorMsgEvent);
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
        /// get stuck messages, Created by omshiv, Nov 2013
        /// </summary>
        /// <param name="MsgType"></param>
        /// <param name="UserID"></param>
        /// <param name="reqID"></param>
        void MessageEngine_GetErrorMsgEvent(object sender, PostTradeServicesErrorEventArgs e)
        {
            try
            {
                switch (e.PranaMsgType)
                {
                    case "DropCopy_PostTrade":
                        IProcessingUnit processingUnit = ProcessorFactory.GetInstance.GetProcessorByName("DropCopy_PostTrade");
                        if (processingUnit != null)
                        {
                            List<PranaMessage> msgs = processingUnit.GetCachedErrorOrders();
                            PostTradeServicesServer.GetInstance.SendErrorMsgToClient(msgs); //, e.UserId, e.ReqId); removed as parameters not used in method, Microsoft Managed Rules
                        }
                        break;
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
        /// Received from Comm Manager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void _inTradeComMgrQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                if (e.Value.Message.ToString() == "CounterPartyDetails")
                {
                    Dictionary<int, FixPartyDetails> allPartyDetails = FixEngineConnectionManager.FixEngineConnectionPoolManager.GetInstance().GetAllFixConnections();
                    foreach (KeyValuePair<int, FixPartyDetails> partyDetails in allPartyDetails)
                    {
                        int status;
                        if (partyDetails.Value.BuySideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED && partyDetails.Value.BuyToSellSideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            status = (int)PranaInternalConstants.ConnectionStatus.CONNECTED;
                        }
                        else
                        {
                            status = (int)PranaInternalConstants.ConnectionStatus.DISCONNECTED; ;
                        }
                        QueueMessage qMsg = new QueueMessage(AdminMessageHandler.CreateCounterPartyStatusReport(partyDetails.Value.ConnectionID, partyDetails.Value.PartyID, partyDetails.Value.PartyName, status, partyDetails.Value.HostName, partyDetails.Value.Port, partyDetails.Value.OriginatorType, partyDetails.Value.BrokerConnectionType));
                        ServerCustomCommunicationManager.GetInstance().SendMsgToAllUsers(qMsg);
                    }
                }
                else if (e.Value.MsgType.Equals(CustomFIXConstants.MSG_NAV_LOCK_DATE_UPDATE))
                {
                    if (DateTime.TryParse(e.Value.Message.ToString(), out DateTime lockDate))
                        CachedDataManager.GetInstance.NAVLockDate = lockDate;
                    else if (e.Value.Message.ToString().Equals(string.Empty))
                        CachedDataManager.GetInstance.NAVLockDate = null;
                    ServerCustomCommunicationManager.GetInstance().SendMsgToAllUsers(HandlerType.TradeHandler, e.Value);
                }
                else if (e.Value.MsgType.Equals(CustomFIXConstants.MSG_MANUAL_ROLLOVER))
                {
                    int userid = Convert.ToInt32(e.Value.UserID);
                    PranaMessage pranaMsg = new PranaMessage(e.Value.Message.ToString());
                    OrderSingle orderReceived = Transformer.CreateOrderSingle(pranaMsg);
                    ClearanceManager.GetInstance.RolloverOfSubOrder(orderReceived, userid);
                }
                else
                {
                    PranaMessage pranaMsg = new PranaMessage(e.Value.Message.ToString());
                    bool sendAllocationsViaFix = CommonDataCache.CachedDataManager.GetSendAllocationsViaFix();
                    //if level1Id is provided then get allocation details and add custom tags based on allocation in accounts, PRANA-27196
                    if (sendAllocationsViaFix && pranaMsg.FIXMessage.InternalInformation.MessageFields.Exists(x => x.Tag.Equals(CustomFIXConstants.CUST_TAG_Level1ID)))
                    {
                        int level1ID = Convert.ToInt32(pranaMsg.FIXMessage.InternalInformation.MessageFields.Find(x => x.Tag.Equals(CustomFIXConstants.CUST_TAG_Level1ID)).Value);
                        if (level1ID != int.MinValue)
                        {
                            //if preference exists, then get allocation details from allocation service, else add total quantity in custom account value tag
                            string preferenceName = _allocationManager.GetAllocationPreferenceNameById(level1ID);
                            if (!string.IsNullOrWhiteSpace(preferenceName) && !preferenceName.Equals("Manual"))
                            {
                                Order order = Transformer.CreateOrder(pranaMsg);
                                order.CumQty = order.Quantity;
                                AllocationGroup group = _allocationServices.CreateVirtualAllocationGroup(order, false);
                                if (group.Allocations != null && group.Allocations.Collection != null && group.Allocations.Collection.Count > 0)
                                {
                                    foreach (AllocationLevelClass alloc in group.Allocations.Collection)
                                    {
                                        pranaMsg.FIXMessage.InternalInformation.AddField(GetInternalTagFromAccountId(alloc.LevelnID), alloc.AllocatedQty.ToString());
                                    }
                                }
                            }
                            else
                            {
                                pranaMsg.FIXMessage.InternalInformation.AddField(GetInternalTagFromAccountId(level1ID), pranaMsg.FIXMessage.ExternalInformation.MessageFields.Find(x => x.Tag.Equals(FIXConstants.TagOrderQty)).Value);
                            }
                        }
                        List<int> swapAccounts = CommonDataCache.CachedDataManager.GetSwapAccounts();
                        foreach (int accountId in swapAccounts)
                        {
                            string accountTag = GetInternalTagFromAccountId(accountId);
                            if (!pranaMsg.FIXMessage.InternalInformation.ContainsKey(accountTag))
                                pranaMsg.FIXMessage.InternalInformation.AddField(accountTag, "0");
                        }
                    }
                    ProcessMessage(pranaMsg);
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

        /// <summary>
        /// recived from Fix Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void _inFixMgrQueue_MessageQueued(object sender, EventArgs<PranaMessage> e)
        {
            try
            {
                ProcessMessage(e.Value);
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

        public void _dummyQueue_MessageQueued(PranaMessage message)
        {
            try
            {
                ProcessMessage(message);
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

        private void ProcessMessage(PranaMessage message)
        {
            IProcessingUnit processingUnit = ProcessorFactory.GetInstance.GetProcessor(message);
            if (processingUnit != null)
            {
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow7] Data received at Process point in MessageEngine, Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }
                StuckTradeMailManager.CheckAndProcessStuckTrades();
                processingUnit.ProcessMessage(message);
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow7] Data Processed in MessageEngine, Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                    }
                }
            }
        }

        void _queueDispatching_MessageQueued(object sender, EventArgs<PranaMessage> e)
        {
            DispatchMessage(e.Value);
        }

        private void DispatchMessage(PranaMessage pranaMessage)
        {
            IDispatchingUnit dispatchingUnit = DispatcherFactory.GetInstance.GetDispatcher(pranaMessage);
            dispatchingUnit.DispatchMessage(pranaMessage);
        }

        void HandleErrorMessages(object sender, EventArgs<string, PranaMessage> e)
        {
            try
            {
                string errorMsgToCient = "Problem in processing at Server";
                string errorMsgForLogging = e.Value + " Message=" + e.Value2.ToString();
                e.Value2.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, errorMsgForLogging);
                PranaMessage pranaMsgForClient = e.Value2.Clone();
                pranaMsgForClient.MessageType = CustomFIXConstants.MsgServerReject;
                pranaMsgForClient.FIXMessage.ExternalInformation.AddField(FIXConstants.TagText, errorMsgToCient);
                QueueMessage qMsg = new QueueMessage(pranaMsgForClient);
                //send to client
                _outTradeComMgrQueue.SendMessage(qMsg);
                // save to db Queue
                _errorQueue.SendMessage(qMsg);
                //show in UI
                Logger.HandleException(new Exception(errorMsgForLogging + "for Msg=" + e.Value2), LoggingConstants.POLICY_LOGANDSHOW);
            }
            catch (Exception)
            {
            }
        }

        public static void SaveAllErrorMessages()
        {
            List<IProcessingUnit> allProcessors = ProcessorFactory.GetInstance.GetAllProcessors();
            foreach (IProcessingUnit processor in allProcessors)
            {
                processor.SaveCachedErrorOrders();
            }
        }

        public static List<string> Names
        {
            get
            {
                List<string> list = new List<string>();
                List<IProcessingUnit> allProcessors = ProcessorFactory.GetInstance.GetAllProcessors();
                foreach (IProcessingUnit processor in allProcessors)
                {
                    list.Add(processor.Name);
                }
                return list;
            }
        }

        /// <summary>
        /// Get Processor based on Processor name, like- DropCopy_PostTrade
        ///  by -omshiv, Nov 2013
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IProcessingUnit GetProcessor(string name)
        {
            IProcessingUnit processingUnit = null;
            try
            {
                processingUnit = ProcessorFactory.GetInstance.GetProcessorByName(name);
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
            return processingUnit;
        }

        /// <summary>
        /// Get FIX Stuck Trades based on Processor name, like- DropCopy_PostTrade
        /// omshiv, Nov 2013
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<PranaMessage> GetFIXStuckTrades()
        {
            IProcessingUnit processingUnit = null;
            List<PranaMessage> msgs = new List<PranaMessage>();
            try
            {
                processingUnit = ProcessorFactory.GetInstance.GetProcessorByName("DropCopy_PostTrade");
                if (processingUnit != null)
                {
                    msgs = processingUnit.GetCachedErrorOrders();
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
            return msgs;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_queueDispatching != null)
                        _queueDispatching.Dispose();
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
    }
}