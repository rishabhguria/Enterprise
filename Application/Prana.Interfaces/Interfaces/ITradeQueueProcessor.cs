using Prana.BusinessObjects.FIX;
using Prana.Global;
using System;
namespace Prana.Interfaces
{
    public interface ITradeQueueProcessor : IDisposable
    {
        void SendMessage(PranaMessage message);
        event EventHandler<EventArgs<PranaMessage>> MessageQueued;
        void StartListening();
        bool IsUpdating
        {
            get;
            set;
        }
    }
}