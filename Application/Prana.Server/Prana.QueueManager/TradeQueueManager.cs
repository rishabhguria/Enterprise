using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
namespace Prana.QueueManager
{

    public class TradeQueueManager : ITradeQueueProcessor
    {
        public event EventHandler<EventArgs<PranaMessage>> MessageQueued;
        private bool _enableTradeFlowLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableTradeFlowLogging"));
        object _sender = null;

        public TradeQueueManager(object sender)
        {
            _sender = sender;
            dataBuffer = new BufferBlock<PranaMessage>();
            System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(dataBuffer)).ConfigureAwait(false);
        }

        BufferBlock<PranaMessage> dataBuffer;
        void BufferMessage(ITargetBlock<PranaMessage> target, PranaMessage message)
        {
            try
            {
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow5] Before Post Buffer In TradeQueueManager, Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
                }
                target.Post(message);
                if (_enableTradeFlowLogging)
                {
                    try
                    {
                        Logger.LoggerWrite("[Trade-Flow5] After Post Buffer In TradeQueueManager, Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    }
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

        async System.Threading.Tasks.Task<PranaMessage> ConsumeBufferMessageAsync(IReceivableSourceBlock<PranaMessage> source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                while (await source.OutputAvailableAsync())
                {
                    PranaMessage message;
                    while (source.TryReceive(out message))
                    {
                        if (message != null)
                        {
                            if (_enableTradeFlowLogging)
                            {
                                try
                                {
                                    Logger.LoggerWrite("[Trade-Flow6] Before Consume Buffer In TradeQueueManager, Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                }
                            }
                            MessageQueued(_sender, new EventArgs<PranaMessage>(message));
                            if (_enableTradeFlowLogging)
                            {
                                try
                                {
                                    Logger.LoggerWrite("[Trade-Flow6] After Consume Buffer In TradeQueueManager, Fix Message: " + Convert.ToString(message.FIXMessage.ExternalInformation), LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY); ;
                                }
                            }
                        }
                    }
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

            return null;
        }
        public void SendMessage(PranaMessage message)
        {
            try
            {
                BufferMessage(dataBuffer, message);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #region ITradeQueueProcessor Members
        private bool _isUpdating = true;

        public bool IsUpdating
        {
            get { return _isUpdating; }
            set
            {
                _isUpdating = value;
            }
        }

        public void StartListening()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose Internal Channel and remove connections from ConnectionManager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    //processsignaller.Dispose();
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

        #endregion
    }

}
