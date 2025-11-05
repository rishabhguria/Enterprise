using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface ICommonDataService
    {
        void Initialize();

        Task GetCompanyTransferTradeRules(RequestResponseModel requestResponseObj);

        Task GetTradingTicketAllData(RequestResponseModel requestResponseObj);

    }
}