using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Prana.Interfaces
{
    public interface ILiveFeedCallback
    {
        [OperationContract(IsOneWay = true)]
        void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData);

        [OperationContract(IsOneWay = true)]
        void OptionChainResponse(string symbol, List<OptionStaticData> data);

        [OperationContract(IsOneWay = true)]
        void LiveFeedConnected();

        [OperationContract(IsOneWay = true)]
        void LiveFeedDisConnected();
    }
}
