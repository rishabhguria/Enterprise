using Prana.BusinessObjects;
using Prana.Global;
using System;
namespace Prana.Interfaces
{
    public interface IQueueManager
    {
        IQueueProcessor CreateQueue(string queueName, object sender);
    }

    public interface IQueueProcessor
    {
        void SendMessage(QueueMessage message);
        event EventHandler<EventArgs<QueueMessage>> MessageQueued;
        HandlerType HandlerType { get; set; }
        void StartListening();
        long getLastSeqNumber();
    }
}
