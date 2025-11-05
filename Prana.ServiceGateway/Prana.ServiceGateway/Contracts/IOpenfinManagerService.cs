using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface IOpenfinManagerService
    {
        void Initialize();

        Task GetOpenfinWorkspaceLayout(RequestResponseModel requestResponseObj);

        Task SaveOpenfinWorkspaceLayout(RequestResponseModel requestResponseObj);

        Task DeleteOpenfinWorkspaceLayout(RequestResponseModel requestResponseObj);
    }
}