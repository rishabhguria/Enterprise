using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.MarketDataService.Common
{
    public interface IMarketDataCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendResult(Dictionary<string, Dictionary<string, string>> result);
    }
}
