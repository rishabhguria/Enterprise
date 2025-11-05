using System.ServiceModel;

namespace Prana.Authentication.Common
{
    public interface IClientConnectivityServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void ClientConnectivityResponseEnterprise(int companyUserID, bool isLoggedOut);

        [OperationContract(IsOneWay = true)]
        void ClientConnectivityResponseWeb(int companyUserID, bool isLoggedOut);
    }
}
