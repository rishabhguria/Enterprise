using Prana.KafkaWrapper;
using Prana.ServiceGateway.Models;

namespace Prana.ServiceGateway.Contracts
{
    public interface ILiveFeedService
    {
        void Initialize();

        Task RequestSymbol(string symbol, int companyUserId);

        Task UpdateMarketDataTokenRequest(RequestResponseModel requestResponseObj);

        Task UnSubscribeLiveFeed(RequestResponseModel requestResponseObj);

        Task ReqMultipleSymbolsLiveFeedSnapshotData(MultipleSymbolRequestDto request, int companyUserId);
    }
}