using Prana.BusinessObjects;
using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IMarketDataPermissionServiceCallback))]
    public interface IMarketDataPermissionService : IServiceOnDemandStatus
    {
        [OperationContract]
        void PermissionCheck(MarketDataPermissionRequest marketDataPermissionRequest, string source, string userDetails = null);

        [OperationContract]
        void AddSubscriptionToGetPermissionFromCache(int companyUserID, string source);

        [OperationContract]
        void RemoveSubscriptionToGetPermissionFromCache(int companyUserID, string source, bool isResponseRequired = true);
    }
}
