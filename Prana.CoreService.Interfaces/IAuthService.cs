using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract]
    public interface IAuthService: IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
        [OperationContract]
        bool CompanyUserLogout(string companyUserId, bool isForcefulLogoutEnterprise, bool isForcefulLogoutWeb);
    }
}
