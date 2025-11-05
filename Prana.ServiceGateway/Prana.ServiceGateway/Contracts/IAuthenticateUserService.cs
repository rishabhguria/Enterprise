using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface IAuthenticateUserService
    {
        void Initialize();

        Task<string> LoginUser(RequestResponseModel requestResponseObj);

        Task<string> LogoutUser(RequestResponseModel requestResponseObj);

        void ConnectionStatusDisconnected();

        void UpdateCacheForLoginUser(RequestResponseModel requestResponseObj);

        List<int> EncrytPassword(string password);

        Task<string> GetStatusForLogin(RequestResponseModel requestResponseObj);

        void ProcessBloombergAuthentication(RequestResponseModel requestResponseObj);
    }
}