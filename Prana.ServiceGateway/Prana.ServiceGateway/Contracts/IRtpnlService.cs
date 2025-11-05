using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface IRtpnlService
    {
        void Initialize();

        Task SaveUpdateConfigDetails(RequestResponseModel requestResponseObj);

        Task GetRtpnlWidgetData(RequestResponseModel requestResponseObj);

        Task GetRtpnlWidgetConfigData(RequestResponseModel requestResponseObj);

        Task SaveConfigDataForExtract(RequestResponseModel requestResponseObj);

        Task CheckCalculationServiceRunning(RequestResponseModel requestResponseObj);

        Task DeleteRemovedWidgetConfigDetails(RequestResponseModel requestResponseObj);

        Task GetRtpnlTradeAttributeLabels(RequestResponseModel requestResponseObj);
    }
}
