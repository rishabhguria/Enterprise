using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;

namespace Prana.DropCopyProcessor
{
    public class PranaDropCopyDispatcher : IDispatchingUnit
    {
        IQueueProcessor _outTradeComMgrQueue;
        ITradeQueueProcessor _outFixMgrQueue;
        private static IDispatchingUnit _pranaDropCopyDispatcher = null;
        static PranaDropCopyDispatcher()
        {
            _pranaDropCopyDispatcher = new PranaDropCopyDispatcher();
        }
        public static IDispatchingUnit GetInstance
        {
            get { return _pranaDropCopyDispatcher; }
        }

        #region IDispatchingUnit Members

        public event EventHandler<EventArgs<string, PranaMessage>> Error;
        public void Initlise(IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor outFixMgrQueue)
        {
            _outTradeComMgrQueue = outTradeComMgrQueue;
            _outFixMgrQueue = outFixMgrQueue;
        }

        public void DispatchMessage(PranaMessage pranaMessage)
        {
            QueueMessage qMsg = new QueueMessage(pranaMessage);
            try
            {
                switch (pranaMessage.MessageType)
                {
                    // received from drop copy client
                    case CustomFIXConstants.MsgDropCopyReceived:
                        // _dbQueue.SendMessage(PranaMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;
                    //order acked from client
                    case CustomFIXConstants.MsgDropCopyAck:
                        //DropCopyCacheManager.AddSentMessage(PranaMessage);
                        //_comOutQueue.SendMessage(PranaMessage);
                        break;

                    //execution report from Cp
                    case CustomFIXConstants.MsgDropCopyExecution:
                        //case FIXConstants.MSGOrderCancelReject:
                        // send messages to drop copy source after mapping details
                        _outFixMgrQueue.SendMessage(pranaMessage);
                        // PranaMessage outBoxMessage = PranaMessage.Clone();
                        // outBoxMessage.MessageType = CustomFIXConstants.MsgDropCopyExecution;
                        //outBoxMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = CustomFIXConstants.MsgDropCopyExecution;
                        DropCopyCacheManager.AddSentMessage(pranaMessage);
                        //_dbQueue.SendMessage(outBoxMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        break;

                    case CustomFIXConstants.MsgUserConnected:
                        string userID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;
                        foreach (KeyValuePair<string, PranaMessage> PranaMsgItem in DropCopyCacheManager.ReceivedMessages)
                        {
                            PranaMessage msgToSend = PranaMsgItem.Value;
                            msgToSend.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserID, userID);
                            qMsg.UserID = userID;
                            _outTradeComMgrQueue.SendMessage(qMsg);
                        }
                        foreach (KeyValuePair<string, PranaMessage> PranaMsgItem in DropCopyCacheManager.SentMessages)
                        {
                            PranaMessage msgToSend = PranaMsgItem.Value;
                            msgToSend.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyUserID, userID);
                            qMsg.UserID = userID;
                            _outTradeComMgrQueue.SendMessage(qMsg);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaDropOrderDispatcher", pranaMessage));

                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion
    }
}
