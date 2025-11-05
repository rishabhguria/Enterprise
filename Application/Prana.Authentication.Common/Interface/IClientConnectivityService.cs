using System.ServiceModel;

namespace Prana.Authentication.Common
{
    [ServiceContract(CallbackContract = typeof(IClientConnectivityServiceCallback))]
    public interface IClientConnectivityService
    {
        [OperationContract]
        void AddClientInfoInCache(int companyUserID);

        [OperationContract]
        void RemoveClientInfoFromCache(int companyUserID, bool isForcefulLogoutEnterprise, bool isForcefulLogoutWeb);

        [OperationContract]
        AuthenticatedUserInfo UpdateCacheForLoginUser(string companyUserId, string userName, string token);
    }
}
