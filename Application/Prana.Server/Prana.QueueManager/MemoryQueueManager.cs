using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Threading.Tasks.Dataflow;

namespace Prana.QueueManager
{
    public class QueueProcessor : IQueueProcessor
    {
        BufferBlock<QueueMessage> bufferBlock;
        public QueueProcessor()
        {
            SetupBufferBlock();
        }

        public QueueProcessor(HandlerType handlerType)
        {
            _handlerType = handlerType;

            SetupBufferBlock();
        }

        private void SetupBufferBlock()
        {
            bufferBlock = new BufferBlock<QueueMessage>();
            System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(bufferBlock)).ConfigureAwait(false);
        }

        void BufferMessage(ITargetBlock<QueueMessage> target, QueueMessage message)
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

        async System.Threading.Tasks.Task<QueueMessage> ConsumeBufferMessageAsync(IReceivableSourceBlock<QueueMessage> source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                while (await source.OutputAvailableAsync())
                {
                    if (MessageQueued != null)
                    {
                        QueueMessage message;
                        while (source.TryReceive(out message))
                        {
                            if (message != null)
                            {
                                MessageQueued(this, new EventArgs<QueueMessage>(message));
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

        #region IQueueProcessor Members
        public event EventHandler<EventArgs<QueueMessage>> MessageQueued;
        public void SendMessage(QueueMessage message)
        {
            try
            {
                BufferMessage(bufferBlock, message);
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

        HandlerType _handlerType = HandlerType.NotSet;
        public HandlerType HandlerType
        {
            get { return _handlerType; }
            set { _handlerType = value; }
        }
        #endregion
    }
}
