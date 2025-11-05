using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface ILayoutService
    {
        void Initialize();

        Task LoadLayout(RequestResponseModel requestResponseObj);

        Task SaveLayout(RequestResponseModel requestResponseObj);

        Task SaveOrUpdateRtpnlViews(RequestResponseModel requestResponseObj);

        Task LoadRtpnlViews(RequestResponseModel requestResponseObj);

        Task DeleteOpenfinPages(RequestResponseModel requestResponseObj);
		
		Task RemovePagesForAnUser(RequestResponseModel requestResponseObj);
    }
}
