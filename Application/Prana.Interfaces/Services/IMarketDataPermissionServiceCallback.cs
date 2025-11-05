using System.ServiceModel;

namespace Prana.Interfaces
{
    public interface IMarketDataPermissionServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void PermissionCheckResponse(int companyUserID, bool isPermitted);
    }
}