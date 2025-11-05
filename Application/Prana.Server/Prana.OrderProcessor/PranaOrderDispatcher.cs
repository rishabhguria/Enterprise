using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ServerCommon;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;

namespace Prana.OrderProcessor
{
    public class PranaOrderDispatcher : IDispatchingUnit
    {
        private static IDispatchingUnit _pranaOrderDispatcher = null;
        static ProxyBase<IPublishing> _proxyPublishing;
        static PranaOrderDispatcher()
        {
            _pranaOrderDispatcher = new PranaOrderDispatcher();
            _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
        }
        IQueueProcessor _outTradeComMgrQueue;
        ITradeQueueProcessor _outFixMgrQueue;

        private void HandleByPranaMessageType(PranaMessage pranaMessage)
        {
            try
            {
                switch (int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value))
                {
                    case (int)OrderFields.PranaMsgTypes.MsgDropCopy_PM:
                        SendAck(pranaMessage);
                        break;
                    case (int)OrderFields.PranaMsgTypes.InternalOrder:
                    case (int)OrderFields.PranaMsgTypes.ORDNewSub:
                    case (int)OrderFields.PranaMsgTypes.ORDNewSubChild:
                        SendAck(pranaMessage);
                        if (pranaMessage.MessageType != FIXConstants.MSGOrderRollOverRequest)
                        {
                            _outFixMgrQueue.SendMessage(pranaMessage);
                        }
                        break;
                    case (int)OrderFields.PranaMsgTypes.ORDManual:
                    case (int)OrderFields.PranaMsgTypes.ORDManualSub:
                    case (int)OrderFields.PranaMsgTypes.ORDStaged:
                        // No SendAck needed for Manual Execution
                        if (pranaMessage.MessageType != FIXConstants.MSGExecution)
                            SendAck(pranaMessage);
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DCStageWorkFlow) && int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) == (int)OrderFields.PranaMsgTypes.ORDStaged && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DCStageWorkFlow].Value == "SendAck")
                        {
                            pranaMessage.MessageType = FIXConstants.MSGExecution;
                            pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_New);
                            _outFixMgrQueue.SendMessage(pranaMessage);
                        }

                        if (int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value) != (int)OrderFields.PranaMsgTypes.ORDStaged)
                        {
                            if (CachedDataManager.GetInstance.IsSendRealtimeManualOrderViaFix())
                            {
                                List<PranaMessage> msgList = new List<PranaMessage>();
                                pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGExecutionReport);
                                msgList.Add(pranaMessage);
                                FixEngineConnectionManager.FixEngineConnectionPoolManager.GetInstance().SendManualOrdersToThirdPartyFixLine(msgList);
                            }
                        }
                        break;

                    default:
                        throw new Exception("Msg Type not Recognised at MessageDispatcher.HandleByPranaMessageType()");
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

        private void SendAck(PranaMessage pranaMessage)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(pranaMessage);
                switch (pranaMessage.MessageType)
                {
                    case FIXConstants.MSGOrder:
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingNew);
                        //PublishMessageUpdatesForWeb(pranaMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;
                    case FIXConstants.MSGOrderCancelRequest:
                        if(!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                            pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingCancel);
                        //PublishMessageUpdatesForWeb(pranaMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;
                    case FIXConstants.MSGOrderRollOverRequest:
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingRollOver);
                        //PublishMessageUpdatesForWeb(pranaMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                        if (!pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrdStatus))
                            pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingReplace);
                        //PublishMessageUpdatesForWeb(pranaMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;
                    default:
                        throw new Exception("Msg Type not Recognised at MessageDispatcher.SendAck()");
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Publish message updates for Web application.
        /// </summary>
        /// <param name="pranaMessage"></param>
        private void PublishMessageUpdatesForWeb(PranaMessage pranaMessage)
        {
            try
            {
                List<PranaMessage> orders = new List<PranaMessage>();
                orders.Add(pranaMessage);
                MessageData e = new MessageData
                {
                    EventData = orders,
                    TopicName = Topics.Topic_UpdatesForWebBlotter
                };
                _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_UpdatesForWebBlotter);
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

        #region IDispatchingUnit Members
        public event EventHandler<EventArgs<string, PranaMessage>> Error;
        public static IDispatchingUnit GetInstance
        {
            get { return _pranaOrderDispatcher; }
        }

        public void Initlise(IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor outFixMgrQueue)
        {
            _outTradeComMgrQueue = outTradeComMgrQueue;
            _outFixMgrQueue = outFixMgrQueue;
        }

        public void DispatchMessage(PranaMessage pranaMessage)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(pranaMessage);

                switch (pranaMessage.MessageType)
                {
                    case FIXConstants.MSGExecution:
                        //If Manual Order and preference true then only send the Execution messages for Handling by MessageType
                        int pranaMesgType = int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value);
                        if (CachedDataManager.GetInstance.IsSendRealtimeManualOrderViaFix() && (pranaMesgType == (int)OrderFields.PranaMsgTypes.ORDManual || pranaMesgType == (int)OrderFields.PranaMsgTypes.ORDManualSub))
                        {
                            HandleByPranaMessageType(pranaMessage);
                        }

                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;

                    case FIXConstants.MSGOrderCancelReject:
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;

                    case FIXConstants.MSGOrder:
                    case FIXConstants.MSGOrderCancelRequest:
                    case FIXConstants.MSGOrderRollOverRequest:
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                        HandleByPranaMessageType(pranaMessage);
                        break;

                    case FIXConstants.MSGTransferUser:
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;

                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;

                    case CustomFIXConstants.MSG_CounterPartyDown:
                    case CustomFIXConstants.MSG_CounterPartyUp:
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;

                    default:
                        Logger.HandleException(new Exception("Msg Type not Recognised at MessageDispatcher.DispatchMessage()"), LoggingConstants.POLICY_LOGANDSHOW);
                        break;
                }
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaOrderDispatcher", pranaMessage));

                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion
    }
}