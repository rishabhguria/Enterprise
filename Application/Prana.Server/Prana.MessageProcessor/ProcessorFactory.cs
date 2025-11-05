using Prana.AllocationProcessor;
using Prana.BasketProcessor;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.DropCopyProcessor;
using Prana.DropCopyProcessor_PostTrade;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OrderProcessor;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;

namespace Prana.MessageProcessor
{
    class ProcessorFactory
    {
        Dictionary<string, IProcessingUnit> _allProcessors = new Dictionary<string, IProcessingUnit>();
        public event EventHandler<EventArgs<string, PranaMessage>> Error;
        #region SingleTon Implementation
        private static ProcessorFactory _processorFactory = null;
        static ProcessorFactory()
        {
            _processorFactory = new ProcessorFactory();
        }
        public static ProcessorFactory GetInstance
        {
            get { return _processorFactory; }
        }
        #endregion
        public IProcessingUnit GetProcessor(PranaMessage pranaMessage)
        {
            IProcessingUnit processingUnit = null;
            try
            {
                switch (pranaMessage.MessageType)
                {
                    case CustomFIXConstants.MsgDropCopyReceived:
                        switch (int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginatorType].Value))
                        {
                            case (int)PranaServerConstants.OriginatorType.DropCopy:
                                processingUnit = PranaDropCopyProcessor.GetInstance;
                                break;
                            case (int)PranaServerConstants.OriginatorType.DropCopy_PM:
                                processingUnit = PranaDropCopyProcessor_PostTrade.GetInstance;
                                break;
                            default:
                                break;
                        }
                        break;
                    case CustomFIXConstants.MsgDropCopyReject:
                    case CustomFIXConstants.MsgDropCopyAck:
                        processingUnit = PranaDropCopyProcessor.GetInstance;
                        break;
                    case FIXConstants.MSGOrder:
                    case FIXConstants.MSGOrderCancelRequest:
                    case FIXConstants.MSGOrderRollOverRequest:
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                    case FIXConstants.MSGTransferUser:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:
                    case FIXConstants.MSGBusinessMessageReject:
                    case FIXConstants.MSGReject:
                    case CustomFIXConstants.MSG_CounterPartyUp:
                    //TODO remove code from here as soon as Post Trade handling starts on Server side
                    case CustomFIXConstants.MSG_REFRESH_DEFAULT:
                    case CustomFIXConstants.CUST_TAG_MultiTradeMessage:
                    case FIXConstants.MSGOrderCancelRequestFroze:
                    case FIXConstants.MSGOrderCancelRequestUnFroze:
                        processingUnit = PranaOrderProcessor.GetInstance;
                        break;
                    case FIXConstants.MSGExecution:
                    case FIXConstants.MSGOrderCancelReject:
                        PranaMessage cachedMessage = OrderCacheManager.GetCachedOrderForExecutionReoprt(pranaMessage);
                        if (cachedMessage == null)
                        {
                            Logger.HandleException(new Exception("Message could not be Validated" + pranaMessage.ToString()), LoggingConstants.POLICY_LOGANDSHOW);
                            processingUnit = null;
                            //processingUnit = PranaOrderProcessor.GetInstance;
                        }
                        else if (cachedMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OriginatorType))
                        {
                            switch (int.Parse(cachedMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OriginatorType].Value))
                            {
                                case (int)PranaServerConstants.OriginatorType.DropCopy:
                                    processingUnit = PranaDropCopyProcessor.GetInstance;
                                    break;
                                case (int)PranaServerConstants.OriginatorType.DropCopy_PM:

                                    processingUnit = PranaOrderProcessor.GetInstance;
                                    break;
                                default:
                                    processingUnit = PranaOrderProcessor.GetInstance;
                                    break;
                            }
                        }
                        else
                        {
                            processingUnit = PranaOrderProcessor.GetInstance;
                        }
                        break;
                    case FIXConstants.MSGOrderList:
                    case FIXConstants.MSGListCancelRequest:
                    case FIXConstants.MSGListReplace:
                        processingUnit = PranaBasketProcessor.GetInstance;
                        break;
                    case CustomFIXConstants.MsgUserConnected:
                        processingUnit = PranaDropCopyProcessor.GetInstance;
                        break;
                    case FIXConstants.MSGAllocation:
                    case FIXConstants.MSGAllocationACK:
                    case FIXConstants.MSGConfirmation:
                    case FIXConstants.MSGAllocationReport:
                    case FIXConstants.MSGAllocationReportAck:
                    case FIXConstants.MSGConfirmationAck:
                        processingUnit = PranaAllocationProcessor.GetInstance;
                        break;
                    default:
                        throw new Exception("No Processor found for message=" + pranaMessage.ToString());
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
            return processingUnit;

        }

        IAllocationServices _allocationServices = null;


        public void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, IAllocationServices allocationServices, ISecMasterServices secServices)
        {
            try
            {
                _allocationServices = allocationServices;
                IProcessingUnit orderProcessor = PranaOrderProcessor.GetInstance;
                orderProcessor.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);
                orderProcessor.Initlise(queueDispatching, dbQueue, secServices, _allocationServices, null);


                if (!_allProcessors.ContainsKey(orderProcessor.Name))
                    _allProcessors.Add(orderProcessor.Name, orderProcessor);

                PranaBasketProcessor.GetInstance.Initlise(queueDispatching, dbQueue, secServices, _allocationServices, orderProcessor);
                PranaBasketProcessor.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);

                if (!_allProcessors.ContainsKey(PranaBasketProcessor.GetInstance.Name))
                    _allProcessors.Add(PranaBasketProcessor.GetInstance.Name, PranaBasketProcessor.GetInstance);

                PranaDropCopyProcessor.GetInstance.Initlise(queueDispatching, dbQueue, secServices, _allocationServices, orderProcessor);
                PranaDropCopyProcessor.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);

                if (!_allProcessors.ContainsKey(PranaDropCopyProcessor.GetInstance.Name))
                    _allProcessors.Add(PranaDropCopyProcessor.GetInstance.Name, PranaDropCopyProcessor.GetInstance);

                PranaDropCopyProcessor_PostTrade.GetInstance.Initlise(queueDispatching, dbQueue, secServices, _allocationServices, orderProcessor);
                PranaDropCopyProcessor_PostTrade.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);

                if (!_allProcessors.ContainsKey(PranaDropCopyProcessor_PostTrade.GetInstance.Name))
                    _allProcessors.Add(PranaDropCopyProcessor_PostTrade.GetInstance.Name, PranaDropCopyProcessor_PostTrade.GetInstance);

                PranaAllocationProcessor.GetInstance.Initlise(queueDispatching, dbQueue, secServices, _allocationServices, orderProcessor);
                PranaAllocationProcessor.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);

                if (!_allProcessors.ContainsKey(PranaAllocationProcessor.GetInstance.Name))
                    _allProcessors.Add(PranaAllocationProcessor.GetInstance.Name, PranaAllocationProcessor.GetInstance);
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

        void HandleError(object sender, EventArgs<string, PranaMessage> e)
        {
            Error(this, new EventArgs<string, PranaMessage>(e.Value, e.Value2));
        }
        public List<IProcessingUnit> GetAllProcessors()
        {
            List<IProcessingUnit> list = new List<IProcessingUnit>();
            foreach (KeyValuePair<string, IProcessingUnit> item in _allProcessors)
            {
                list.Add(item.Value);
            }
            return list;
        }
        /// <summary>
        /// Return Processor 
        /// MOdified by: omshiv, checking name in _allProcessors Dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IProcessingUnit GetProcessorByName(string name)
        {
            IProcessingUnit processUInit = null;
            try
            {
                if (_allProcessors.ContainsKey(name))
                {
                    processUInit = _allProcessors[name];
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
            return processUInit;
        }
    }
}
