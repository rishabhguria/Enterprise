using System.Collections.Concurrent;
using System.ServiceModel;

namespace Prana.Authentication.Common
{
    [ServiceContract]
    public interface IAuthenticateUser
    {
        [OperationContract]
        AuthenticatedUserInfo ValidateCompanyUserLogin(string login, string password, bool isLoggedInFromSamsara, string samsaraAzureId = "");

        [OperationContract]
        string CompanyUserLogout(string companyUserId, bool isForcefulLogoutEnterprise, bool isForcefulLogoutWeb);

        [OperationContract]
        ConcurrentDictionary<int, AuthenticatedUserInfo> GetLoggedInUser();

        [OperationContract]
        ServiceStatusInfo GetServiceStatus();
    }
}
