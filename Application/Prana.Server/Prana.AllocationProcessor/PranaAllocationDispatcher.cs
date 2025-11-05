using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServerCommon;
using System;

namespace Prana.AllocationProcessor
{
    public class PranaAllocationDispatcher : IDispatchingUnit
    {
        IQueueProcessor _outTradeComMgrQueue;
        ITradeQueueProcessor _outFixMgrQueue;
        private static IDispatchingUnit _pranaAllocationDispatcher = null;

        static PranaAllocationDispatcher()
        {
            _pranaAllocationDispatcher = new PranaAllocationDispatcher();
        }

        public static IDispatchingUnit GetInstance
        {
            get { return _pranaAllocationDispatcher; }
        }

        #region IDispatchingUnit Members
        public event EventHandler<EventArgs<string, PranaMessage>> Error;

        public void DispatchMessage(PranaMessage pranaMessage)
        {
            try
            {
                QueueMessage qMsg = new QueueMessage(pranaMessage);

                switch (pranaMessage.MessageType)
                {
                    case FIXConstants.MSGAllocation:
                    case FIXConstants.MSGAllocationReportAck:
                    case FIXConstants.MSGConfirmationAck:
                        _outFixMgrQueue.SendMessage(pranaMessage);
                        _outTradeComMgrQueue.SendMessage(qMsg);
                        PranaAllocationManager.SaveBlockAllocationDetails(pranaMessage);
                        break;

                    case FIXConstants.MSGAllocationACK:
                    case FIXConstants.MSGAllocationReport:
                    case FIXConstants.MSGConfirmation:
                        PranaAllocationManager.SaveBlockAllocationDetails(pranaMessage);
                        break;
                    default:
                        Logger.HandleException(new Exception("Msg Type not Recognised at PranaAllocationDispatcher.DispatchMessage()"), LoggingConstants.POLICY_LOGANDSHOW);
                        break;
                }
            }
            catch (Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaOrderDispatcher", pranaMessage));
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void Initlise(IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor outFixMgrQueue)
        {
            _outTradeComMgrQueue = outTradeComMgrQueue;
            _outFixMgrQueue = outFixMgrQueue;
        }
        #endregion

    }
}
