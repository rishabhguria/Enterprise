using Prana.AllocationProcessor;
using Prana.BasketProcessor;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.DropCopyProcessor;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OrderProcessor;
using Prana.ServerCommon;
using System;

namespace Prana.MessageProcessor
{
    class DispatcherFactory
    {
        public event EventHandler<EventArgs<string, PranaMessage>> Error;
        #region SingleTon Implementation
        private static DispatcherFactory _dispatcherFactory = null;
        static DispatcherFactory()
        {
            _dispatcherFactory = new DispatcherFactory();
        }
        public static DispatcherFactory GetInstance
        {
            get { return _dispatcherFactory; }
        }
        #endregion

        public IDispatchingUnit GetDispatcher(PranaMessage pranaMessage)
        {
            IDispatchingUnit dispatchingUnit = null;
            try
            {
                switch (pranaMessage.MessageType)
                {
                    case CustomFIXConstants.MsgDropCopyReceived:
                    case CustomFIXConstants.MsgDropCopyReject:
                    case CustomFIXConstants.MsgDropCopyExecution:
                        dispatchingUnit = PranaDropCopyDispatcher.GetInstance;
                        break;

                    case FIXConstants.MSGOrder:
                    case FIXConstants.MSGOrderCancelRequest:
                    case FIXConstants.MSGOrderRollOverRequest:
                    case FIXConstants.MSGOrderCancelReplaceRequest:
                    case CustomFIXConstants.MSG_CounterPartyDown:
                    case CustomFIXConstants.MSG_CounterPartyUp:
                        // if basket Order request
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ListID) &&
                            pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ListID].ToString() != string.Empty)
                        {
                            dispatchingUnit = PranaBasketDispatcher.GetInstance;
                        }
                        else // normal order
                        {
                            dispatchingUnit = PranaOrderDispatcher.GetInstance;
                        }
                        break;

                    case FIXConstants.MSGExecution:
                    case FIXConstants.MSGReject:
                    case FIXConstants.MSGOrderCancelReject:
                    case FIXConstants.MSGTransferUser:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderNew:
                    case CustomFIXConstants.MSGAlgoSyntheticReplaceOrderFIX:
                    case FIXConstants.MSGBusinessMessageReject:
                        dispatchingUnit = PranaOrderDispatcher.GetInstance;
                        break;

                    case FIXConstants.MSGOrderList:
                    case FIXConstants.MSGListCancelRequest:
                    case FIXConstants.MSGListReplace:
                        dispatchingUnit = PranaBasketDispatcher.GetInstance;
                        break;

                    case CustomFIXConstants.MsgUserConnected:
                        dispatchingUnit = PranaDropCopyDispatcher.GetInstance;
                        break;

                    case FIXConstants.MSGAllocation:
                    case FIXConstants.MSGAllocationACK:
                    case FIXConstants.MSGAllocationReportAck:
                    case FIXConstants.MSGAllocationReport:
                    case FIXConstants.MSGConfirmation:
                    case FIXConstants.MSGConfirmationAck:
                        dispatchingUnit = PranaAllocationDispatcher.GetInstance;
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
            return dispatchingUnit;
        }

        public void Initlise(IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor outFixMgrQueue)
        {
            PranaOrderDispatcher.GetInstance.Initlise(outTradeComMgrQueue, outFixMgrQueue);
            PranaOrderDispatcher.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);
            PranaBasketDispatcher.GetInstance.Initlise(outTradeComMgrQueue, outFixMgrQueue);
            PranaBasketDispatcher.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);
            PranaDropCopyDispatcher.GetInstance.Initlise(outTradeComMgrQueue, outFixMgrQueue);
            PranaDropCopyDispatcher.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);
            PranaAllocationDispatcher.GetInstance.Initlise(outTradeComMgrQueue, outFixMgrQueue);
            PranaAllocationDispatcher.GetInstance.Error += new EventHandler<EventArgs<string, PranaMessage>>(HandleError);
        }

        void HandleError(object sender, EventArgs<string, PranaMessage> e)
        {
            Error(this, new EventArgs<string, PranaMessage>(e.Value, e.Value2));
        }
    }
}
