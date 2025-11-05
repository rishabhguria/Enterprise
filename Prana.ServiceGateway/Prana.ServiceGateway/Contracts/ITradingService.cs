using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface ITradingService
    {
        void Initialize();

        Task SendReplaceOrderFromTT(RequestResponseModel requestResponseObj);

        Task BookAsSwapReplace(RequestResponseModel requestResponseObj);

        Task SendLiveOrderFromTT(RequestResponseModel requestResponseObj);

        Task SendManualOrderFromTT(RequestResponseModel requestResponseObj);

        Task SendStageOrderFromTT(RequestResponseModel requestResponseObj);

        Task GetSMData(RequestResponseModel requestResponseObj);

        Task GetCustomAllocationDetails(RequestResponseModel requestResponseObj);

        Task CreatePopUpText(RequestResponseModel requestResponseObj);

        Task CreateOptionSymbol(RequestResponseModel requestResponseObj);

        Task GetSavedCustomAllocationDetails(RequestResponseModel requestResponseObj);

        Task GetSavedCustomAllocationDetailsBulk(RequestResponseModel requestResponseObj);

        Task GetSymbolAccountWisePosition(RequestResponseModel requestResponseObj);

        Task GetSymbolWiseShortLocateOrders(RequestResponseModel requestResponseObj);

        Task DetermineSecurityBorrowType(RequestResponseModel requestResponseObj);

        Task GetCompanyUserHotKeyPreferences(RequestResponseModel requestResponseObj);

        Task UpdateCompanyUserHotKeyPreferences(RequestResponseModel requestResponseObj);

        Task GetCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj);

        Task UpdateCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj);

        Task SaveCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj);

        Task DeleteCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj);

        Task GetAlgoStrategiesFromBroker(RequestResponseModel requestResponseObj);

        Task UnsubscribeSymbolCompressionFeed(RequestResponseModel requestResponseObj);

        Task GetBrokerConnectionAndVenuesData(RequestResponseModel requestResponseObj);

        Task GetPstAccountNav(RequestResponseModel model);

        Task SendPstOrders(RequestResponseModel requestResponseModel);

        Task CreatePstAllocatonPreference(RequestResponseModel message);

        Task ProduceTradeAttributeLabelsEvent(RequestResponseModel requestResponseObj);

        Task ProduceTradeAttributeValuesEvent(RequestResponseModel requestResponseObj);

        Task SendOrdersToMarket(RequestResponseModel requestResponseModel);

        Task DetermineMultipleSecurityBorrowType(RequestResponseModel requestResponseObj);

    }
}
