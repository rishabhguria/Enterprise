using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Prana.QueueManager
{
    public class BroadcastMemoryQueueManager : IQueueProcessor
    {
        BroadcastBlock<QueueMessage> broadcastBlock;
        private int _greeksCalculationInterval_Consumer = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("GreeksCalculationInterval_Consumer"));
        public BroadcastMemoryQueueManager()
        {
            broadcastBlock = new BroadcastBlock<QueueMessage>(null);
            System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(broadcastBlock)).ConfigureAwait(false);
        }

        private void BufferMessage(ITargetBlock<QueueMessage> target, QueueMessage message)
        {
            try
            {
                target.Post(message);
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

        private async System.Threading.Tasks.Task<QueueMessage> ConsumeBufferMessageAsync(IReceivableSourceBlock<QueueMessage> source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                while (await source.OutputAvailableAsync())
                {
                    QueueMessage message;
                    while (source.TryReceive(out message))
                    {
                        Thread.Sleep(_greeksCalculationInterval_Consumer);
                        if (message != null)
                        {
                            MessageQueued(this, new EventArgs<QueueMessage>(message));
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

        #region IQueueProcessor Members
        public event EventHandler<EventArgs<QueueMessage>> MessageQueued;
        public void SendMessage(QueueMessage message)
        {
            try
            {
                BufferMessage(broadcastBlock, message);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void StartListening()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long getLastSeqNumber()
        {
            return long.MinValue;
        }

        private HandlerType _handlerType = HandlerType.NotSet;
        public HandlerType HandlerType
        {
            get { return _handlerType; }
            set { _handlerType = value; }
        }
        #endregion
    }
}
