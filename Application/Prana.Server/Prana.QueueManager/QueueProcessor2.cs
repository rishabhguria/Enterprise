using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Threading.Tasks.Dataflow;

namespace Prana.QueueManager
{
    public class QueueProcessor2<TQueueMessage> : IQueueProcessor where TQueueMessage : IPropagatorBlock<QueueMessage, QueueMessage>
                                                                                        , ITargetBlock<QueueMessage>
                                                                                        , IReceivableSourceBlock<QueueMessage>
                                                                                        , ISourceBlock<QueueMessage>
                                                                                        , IDataflowBlock, new()
    {
        TQueueMessage broadcastBlock;

        public QueueProcessor2()
        {
            broadcastBlock = new TQueueMessage();
            broadcastBlock = default(TQueueMessage);
            System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(broadcastBlock)).ConfigureAwait(false);
        }


        private void BufferMessage(TQueueMessage target, QueueMessage message)
        {
            try
            {
                ((ITargetBlock<QueueMessage>)target).Post(message);
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

        private async System.Threading.Tasks.Task<QueueMessage> ConsumeBufferMessageAsync(TQueueMessage source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                IReceivableSourceBlock<QueueMessage> source2 = (IReceivableSourceBlock<QueueMessage>)source;
                while (await source2.OutputAvailableAsync())
                {
                    QueueMessage message;
                    while (source2.TryReceive(out message))
                    {
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
