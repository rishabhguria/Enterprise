using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServerCommon;
using System;

namespace Prana.BasketProcessor
{
    /// <summary>
    /// Class is used for dispatching Basket message.
    /// </summary>
    public class PranaBasketDispatcher : IDispatchingUnit
    {
        IQueueProcessor _outTradeComMgrQueue;
        ITradeQueueProcessor _outFixMgrQueue;

        private static IDispatchingUnit _pranaBasketDispatcher = null;
        static PranaBasketDispatcher()
        {
            _pranaBasketDispatcher = new PranaBasketDispatcher();
        }

        /// <summary>
        /// Gets instance of PranaBasketDispatcher.
        /// </summary>     
        public static IDispatchingUnit GetInstance
        {
            get { return _pranaBasketDispatcher; }
        }

        /// <summary>
        /// Creates Basket responce message.
        /// </summary>
        /// <param name="basketPranaMessage"></param>        
        public PranaMessage CreateBasketResponse(PranaMessage basketPranaMessage)
        {
            PranaMessage ackPranaMessage = null;

            try
            {
                ackPranaMessage = new PranaMessage();
                ackPranaMessage.MessageType = FIXConstants.MSGOrderList;
                ackPranaMessage.FIXMessageList.GroupID = basketPranaMessage.FIXMessageList.GroupID;
                ackPranaMessage.TradingAccountID = basketPranaMessage.TradingAccountID;
                ackPranaMessage.FIXMessageList.BasketID = basketPranaMessage.FIXMessageList.BasketID;
                ackPranaMessage.FIXMessageList.TradedBasketID = basketPranaMessage.FIXMessageList.TradedBasketID;
                ackPranaMessage.FIXMessageList.WaveID = basketPranaMessage.FIXMessageList.WaveID;

                foreach (FIXMessage orderMessage in basketPranaMessage.FIXMessageList.ListMessages)
                {
                    FIXMessage ackOrderMessage = orderMessage.Clone();

                    //   ackOrderMessage
                    //ackOrderMessage.ExternalInformation[FIXConstants.TagMsgType].Value = orderMessage.ExternalInformation[FIXConstants.TagMsgType].Value;
                    switch (ackOrderMessage.ExternalInformation[FIXConstants.TagMsgType].Value)
                    {
                        case FIXConstants.MSGOrder:
                            ackOrderMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingNew);
                            break;
                        case FIXConstants.MSGOrderCancelRequest:
                            ackOrderMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingCancel);
                            break;
                        case FIXConstants.MSGOrderRollOverRequest:
                            ackOrderMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingRollOver);
                            break;
                        case FIXConstants.MSGOrderCancelReplaceRequest:
                            ackOrderMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PendingReplace);
                            break;
                    }
                    ackPranaMessage.FIXMessageList.AddMessage(ackOrderMessage);
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
            return ackPranaMessage;
        }

        /// <summary>
        /// Initializes queues.
        /// </summary>
        /// <param name="outTradeComMgrQueue"></param>
        /// <param name="outFixMgrQueue"></param>
        public void Initlise(IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor outFixMgrQueue)
        {
            _outTradeComMgrQueue = outTradeComMgrQueue;
            _outFixMgrQueue = outFixMgrQueue;
        }

        #region IDispatcher Members
        /// <summary>
        /// event for handling errors.
        /// </summary>       
        public event EventHandler<EventArgs<string, PranaMessage>> Error;

        /// <summary>
        /// Dispatches message.
        /// </summary>
        /// <param name="pranaMessage"></param>        
        public void DispatchMessage(PranaMessage pranaMessage)
        {
            try
            {
                //send to clients
                PranaMessage ackPranaMessage = CreateBasketResponse(pranaMessage);
                if (ackPranaMessage != null)
                {
                    QueueMessage qMsg = new QueueMessage(ackPranaMessage);
                    if (ackPranaMessage.TradingAccountID != string.Empty)
                    {
                        qMsg.TradingAccountID = ackPranaMessage.TradingAccountID;
                    }
                    if (ackPranaMessage.UserID != string.Empty)
                    {
                        qMsg.UserID = ackPranaMessage.UserID;
                    }
                    _outTradeComMgrQueue.SendMessage(qMsg);
                }
                // send to fix side
                foreach (FIXMessage fixMsg in pranaMessage.FIXMessageList.ListMessages)
                {
                    PranaMessage PranaSingleOrderMsg = new PranaMessage();
                    PranaSingleOrderMsg.MessageType = fixMsg.ExternalInformation[FIXConstants.TagMsgType].Value;
                    PranaSingleOrderMsg.FIXMessage = fixMsg;
                    _outFixMgrQueue.SendMessage(PranaSingleOrderMsg);
                }
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaBasketDispatcher", pranaMessage));
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion
    }
}