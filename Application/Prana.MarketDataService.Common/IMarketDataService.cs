using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.MarketDataService.Common
{
    [ServiceContract(CallbackContract = typeof(IMarketDataCallback))]
    public interface IMarketDataService
    {
        [OperationContract(IsOneWay = true)]
        void AdviseSymbol(MDServiceReqObject smObject);

        [OperationContract(IsOneWay = true)]
        void AdviseSymbolList(List<MDServiceReqObject> smObject);

        [OperationContract]
        Dictionary<string, string> SnapshotSymbol(MDServiceReqObject smObject);

        [OperationContract]
        Dictionary<string, string> GetSMData(MDServiceReqObject symbol);

        [OperationContract(IsOneWay = true)]
        void DeleteSymbol(MDServiceReqObject smObject);
    }
}
