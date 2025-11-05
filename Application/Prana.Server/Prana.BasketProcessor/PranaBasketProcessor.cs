using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;

namespace Prana.BasketProcessor
{
    /// <summary>
    /// Class is used for Processing Basket message.
    /// </summary>
    public class PranaBasketProcessor : IProcessingUnit
    {
        ITradeQueueProcessor _queueDispatching;
        private IProcessingUnit _orderProcessor = null;
        IQueueProcessor _dbQueue;

        private static IProcessingUnit _pranaBasketProcessor = null;
        static PranaBasketProcessor()
        {
            _pranaBasketProcessor = new PranaBasketProcessor();
        }

        /// <summary>
        /// Gets instance of Basket processor.
        /// </summary>       
        public static IProcessingUnit GetInstance
        {
            get { return _pranaBasketProcessor; }
        }

        #region IProcessingUnit Members
        /// <summary>
        /// event for handling errors.
        /// </summary>       
        public event EventHandler<EventArgs<string, PranaMessage>> Error;

        /// <summary>
        /// Initializes queues and services.
        /// </summary>
        /// <param name="queueDispatching"></param>
        /// <param name="dbQueue"></param>
        /// /// <param name="secmasterServices"></param>
        public void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, ISecMasterServices secmasterServices, IAllocationServices allocationServices, IProcessingUnit orderProcessor)
        {
            _queueDispatching = queueDispatching;
            _dbQueue = dbQueue;
            _orderProcessor = orderProcessor;
        }

        /// <summary>
        /// Processes the basket message.
        /// </summary>       
        public void ProcessMessage(PranaMessage pranaMessage)
        {
            try
            {
                if (BasketCacheManager.DoesBasketExist(pranaMessage.FIXMessageList.BasketID))
                {
                    pranaMessage.FIXMessageList.TradedBasketID = BasketCacheManager.GetTradedBasketID(pranaMessage.FIXMessageList.BasketID);
                }
                else
                {
                    pranaMessage.FIXMessageList.TradedBasketID = UniqueIDGenerator.GenerateListID();
                    BasketCacheManager.AddBasket(pranaMessage.FIXMessageList.BasketID, pranaMessage.FIXMessageList.TradedBasketID);
                }

                foreach (FIXMessage fixSingleMsg in pranaMessage.FIXMessageList.ListMessages)
                {
                    fixSingleMsg.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.BasketOrder).ToString());
                    fixSingleMsg.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ListID, pranaMessage.FIXMessageList.TradedBasketID);
                    PranaMessage PranaSingleOrderMsg = new PranaMessage();
                    PranaSingleOrderMsg.MessageType = fixSingleMsg.ExternalInformation[FIXConstants.TagMsgType].Value;
                    PranaSingleOrderMsg.FIXMessage = fixSingleMsg;
                    _orderProcessor.ProcessMessage(PranaSingleOrderMsg);
                }
                QueueMessage qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMessage);
                _dbQueue.SendMessage(qPersisted);
                _queueDispatching.SendMessage(pranaMessage);
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaBasketProcessor", pranaMessage));
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion

        public List<PranaMessage> GetAllCachedMessages()
        {
            return OrderCacheManager.GetCachedMessages();
        }

        public void SaveCachedErrorOrders()
        {
        }

        /// <summary>
        /// Gets all the errorenous orders from cache.
        /// </summary>     
        public List<PranaMessage> GetCachedErrorOrders()
        {
            return new List<PranaMessage>();
        }

        private string _name = "Basket";
        public string Name
        {
            get { return _name; }
        }

        #region IProcessingUnit Members
        /// <summary>
        /// Gets and then clear all messages from list.
        /// </summary>        
        public Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages()
        {
            return new Dictionary<string, List<PranaMessage>>();
        }
        #endregion
    }
}
