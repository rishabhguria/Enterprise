using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface IComplianceService
    {
        void Initialize();

        Task SendComplianceData(RequestResponseModel requestResponseObj);

        Task SendComplianceDataForStage(RequestResponseModel requestResponseObj);

        Task CheckComplianceFrBasket(RequestResponseModel requestResponseObj);
    }
}
