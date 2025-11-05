using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    public interface IFixEngineAdapter : IDisposable
    {
        void SetUp(IQueueProcessor cpReceivedQueue, IQueueProcessor cpsentQueue);
        void SendMessage(PranaMessage pranaMsg);
        void ReProcessMessage(PranaMessage pranaMsg);
        void Connect(FixPartyDetails fixPartyDetails);
        void Disconnect();
        PranaMessage CreatePranaMessageFromFixMessage(string message);

        event EventHandler<EventArgs<FixPartyDetails>> FixConnectionEvent;
        event EventHandler<EventArgs<PranaMessage>> MessageReceivedEvent;
    }

}
