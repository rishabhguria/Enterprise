using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.Interfaces;
using System;
using System.Collections.Generic;

namespace Prana.ServerCommon
{
    public interface IProcessingUnit
    {

        void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, ISecMasterServices secmasterServices, IAllocationServices allocationServices, IProcessingUnit orderProcessor);
        void ProcessMessage(PranaMessage pranaMsg);
        event EventHandler<EventArgs<string, PranaMessage>> Error;
        void SaveCachedErrorOrders();
        List<PranaMessage> GetCachedErrorOrders();
        List<PranaMessage> GetAllCachedMessages();
        string Name
        {
            get;
        }
        // get error msg and clear collection when reprocessing all trades - omshiv, Nov 2013
        Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages();
    }
}
