using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using System;

namespace Prana.ServerCommon
{
    public interface IDispatchingUnit
    {
        void Initlise(IQueueProcessor outTradeComMgrQueue, ITradeQueueProcessor outFixMgrQueue);
        void DispatchMessage(PranaMessage pranaMessage);
        event EventHandler<EventArgs<string, PranaMessage>> Error;
    }
}
