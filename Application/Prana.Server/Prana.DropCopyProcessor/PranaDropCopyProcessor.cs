using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CustomMapper;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;

namespace Prana.DropCopyProcessor
{
    public class PranaDropCopyProcessor : IProcessingUnit
    {
        ITradeQueueProcessor _queueDispatching;
        private IProcessingUnit _orderProcessor = null;
        IQueueProcessor _dbQueue;
        private static IProcessingUnit _pranaDropCopyProcessor = null;
        static PranaDropCopyProcessor()
        {
            _pranaDropCopyProcessor = new PranaDropCopyProcessor();
        }
        public static IProcessingUnit GetInstance
        {
            get { return _pranaDropCopyProcessor; }
        }

        #region IProcessingUnit Members
        public event EventHandler<EventArgs<string, PranaMessage>> Error;
        public void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, ISecMasterServices secmasterServices, IAllocationServices allocationServices, IProcessingUnit orderProcessor)
        {
            _queueDispatching = queueDispatching;
            _dbQueue = dbQueue;
            _orderProcessor = orderProcessor;
            DropCopyCacheManager.FillClientDropCopyOrdersFromDB();
        }

        public void ProcessMessage(Prana.BusinessObjects.FIX.PranaMessage pranaMsg)
        {
            try
            {
                QueueMessage qPersisted = null;
                switch (pranaMsg.MessageType)
                {
                    case CustomFIXConstants.MsgDropCopyReceived:
                        //apply custom rules
                        //added by harsh, apply rules before processing messages
                        string clOrderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value;
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ClientOrderID, clOrderID);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, clOrderID);
                        pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, CustomFIXConstants.MsgDropCopyReceived);

                        DropCopyCacheManager.AddReceivedMessage(pranaMsg);

                        PranaCustomMapper.ApplyRules(pranaMsg, Direction.In);
                        qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMsg);
                        _dbQueue.SendMessage(qPersisted);
                        _queueDispatching.SendMessage(pranaMsg);
                        break;
                    //order acked from client
                    case CustomFIXConstants.MsgDropCopyAck:
                        pranaMsg.MessageType = FIXConstants.MSGOrder;
                        _orderProcessor.ProcessMessage(pranaMsg);
                        break;
                    //order rejected from client
                    case CustomFIXConstants.MsgDropCopyReject:

                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrdStatus].Value = FIXConstants.ORDSTATUS_Rejected;
                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = UniqueIDGenerator.GetClOrderID();
                        Logger.LoggerWrite("ClOrder used=" + pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);

                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExecID].Value = System.Guid.NewGuid().ToString();
                        // PranaMessage sendtoFix = PranaMessage.Clone();

                        DropCopyCacheManager.UpdateParentOrderAndChildOrders(pranaMsg);
                        // create a business reject message
                        pranaMsg.MessageType = FIXConstants.MSGExecutionReport;
                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGExecutionReport;
                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExecTransType].Value = FIXConstants.EXECTRANSTYPE_New;
                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExecType].Value = FIXConstants.EXECTYPE_Rejected;
                        pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagLastShares].Value = "0";
                        string validationMessage = Prana.Fix.FixDictionary.FixMessageValidator.ValidateMessage(pranaMsg);
                        if (validationMessage == string.Empty)
                        {
                            PranaCustomMapper.ApplyRules(pranaMsg, Direction.In);
                            qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMsg);
                            _dbQueue.SendMessage(qPersisted);
                            _queueDispatching.SendMessage(pranaMsg);
                        }
                        else
                        {
                            throw new Exception(validationMessage);
                        }

                        break;
                    //  execution report from Cp
                    case FIXConstants.MSGExecution:
                    case FIXConstants.MSGOrderCancelReject:
                        //To Do:
                        //use muti thread(queue with 1 processor ) to process executions. all executions with same parent ID will process in same thread.
                        //make a class ExecutionProcess that do the same code below and create a configable array of IQueueProcessor associate with single 
                        //ExecutionProcess. 
                        //IQueueProcessor [] _exectionProcess =..
                        //String parentClOrdID = getParentClOrdID(pranaMsg);
                        //int processIndex = parentClOrdID.getHashCode()%_exectionProcess.size();
                        //_exectionProcess[processIndex].SendMessage(pranaMsg);
                        //break;

                        _orderProcessor.ProcessMessage(pranaMsg);
                        Prana.BusinessObjects.FIX.PranaMessage messageToSend = pranaMsg.Clone();

                        if (DropCopyCacheManager.UpdateParentOrderAndChildOrders(messageToSend))
                        {
                            string validationMessage1 = FixMessageValidator.ValidateMessage(messageToSend);
                            messageToSend.MessageType = CustomFIXConstants.MsgDropCopyExecution;
                            if (validationMessage1 == string.Empty)
                            {
                                qPersisted = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMsg);
                                _dbQueue.SendMessage(qPersisted);
                                _queueDispatching.SendMessage(messageToSend);
                                //?? the _queueDispatching.SendMessage(messageToSend); maybe should be called by _dbQueue after save the order to db. It should not be called if save to db fail. 
                                //It should be in same transaction.
                                //also _dbQueue.SendMessage(qPersisted) should be called by _orderProcessor
                            }
                            else
                            {
                                throw new Exception(validationMessage1);
                            }
                        }
                        break;
                    case CustomFIXConstants.MsgUserConnected:
                        _queueDispatching.SendMessage(pranaMsg);
                        break;
                }
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaDropOrderProcessor", pranaMsg));
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        public void SaveCachedErrorOrders()
        {

        }
        public List<PranaMessage> GetAllCachedMessages()
        {
            return OrderCacheManager.GetCachedMessages();
        }
        public List<PranaMessage> GetCachedErrorOrders()
        {
            return new List<PranaMessage>();
        }
        private string _name = "DropCopy_Trade";

        public string Name
        {
            get { return _name; }
        }

        #region IProcessingUnit Members

        /// <summary>
        /// Get And Clear Prana Messages from cache (stuck)
        /// created By- omshiv,nov 2013
        /// </summary>
        public Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages()
        {
            return new Dictionary<string, List<PranaMessage>>();
        }

        #endregion
    }
}
