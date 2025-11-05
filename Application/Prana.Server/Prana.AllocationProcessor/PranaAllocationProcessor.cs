using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CustomMapper;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.AllocationProcessor
{
    public class PranaAllocationProcessor : IProcessingUnit, IDisposable
    {
        private static IProcessingUnit _pranaAllocationProcessor = null;
        private ITradeQueueProcessor _queueDispatching;
        private IQueueProcessor _dbQueue;
        private bool disposedValue;

        static PranaAllocationProcessor()
        {
            _pranaAllocationProcessor = new PranaAllocationProcessor();
        }

        public static IProcessingUnit GetInstance
        {
            get { return _pranaAllocationProcessor; }
        }

        #region Private Methods
        private void HandleByFixMsgType(PranaMessage pranaMessage)
        {
            try
            {
                switch (pranaMessage.MessageType)
                {
                    case FIXConstants.MSGAllocation:
                    case FIXConstants.MSGConfirmationAck:
                    case FIXConstants.MSGAllocationReportAck:
                        pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, pranaMessage.MessageType);

                        ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.Out);
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradeBlocked) && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradeBlocked].Value == "1")
                        {
                            return;
                        }
                        HandleByPranaMessageType(pranaMessage);
                        break;

                    case FIXConstants.MSGAllocationACK:           
                    case FIXConstants.MSGAllocationReport:
                    case FIXConstants.MSGConfirmation:
                        ValidateAndApplyCustomRulesFixOrders(pranaMessage, Direction.In);
                        if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradeBlocked) && pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradeBlocked].Value == "1")
                        {
                            return;
                        }
                        if ((PranaServerConstants.BrokerConnectionType)int.Parse(pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BrokerConnectionType].Value) == PranaServerConstants.BrokerConnectionType.SendAndConfirmBack)
                        {
                            HandleByPranaMessageType(pranaMessage);
                        }
                        break;

                    default:
                        throw new Exception("Msg Type not Recognised at MessageProcessor.HandleByFixMsgType()");
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

        private void HandleByPranaMessageType(PranaMessage pranaMessage)
        {
            _queueDispatching.SendMessage(pranaMessage);
        }

        private bool ValidateAndApplyCustomRulesFixOrders(PranaMessage pranaMessage, Direction direction)
        {
            try
            {
                PranaCustomMapper.ApplyRules(pranaMessage, direction, PranaServerConstants.OriginatorTypeCategory.EOD);
                return true;
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
        #endregion

        #region IProcessingUnit Members
        private string _name = "Allocation";
        public string Name
        {
            get { return _name; }
        }

        public event EventHandler<EventArgs<string, PranaMessage>> Error;

        public List<PranaMessage> GetAllCachedMessages()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages()
        {
            throw new NotImplementedException();
        }

        public List<PranaMessage> GetCachedErrorOrders()
        {
            throw new NotImplementedException();
        }

        public void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, ISecMasterServices secmasterServices, IAllocationServices allocationServices, IProcessingUnit orderProcessor)
        {
            _queueDispatching = queueDispatching;
            _dbQueue = dbQueue;
        }

        public void ProcessMessage(PranaMessage pranaMessage)
        {
            try
            {
                HandleByFixMsgType(pranaMessage);
            }
            catch(Exception ex)
            {
                Error(this, new EventArgs<string, PranaMessage>("Problem at PranaAllocationProcessor", pranaMessage));
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SaveCachedErrorOrders()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable Members
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
