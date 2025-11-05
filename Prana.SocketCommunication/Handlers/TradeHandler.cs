using Prana.BusinessObjects;
using Prana.Interfaces;

namespace Prana.SocketCommunication.Handlers
{
    public class TradeHandler
    {
        private IQueueProcessor _inComMgrQueue;
        public void HandleMessage(QueueMessage message)
        {
            _inComMgrQueue.SendMessage(message);
        }
        public void Initlise(IQueueProcessor inComMgrQueue)
        {
            _inComMgrQueue = inComMgrQueue;
        }

    }
}
