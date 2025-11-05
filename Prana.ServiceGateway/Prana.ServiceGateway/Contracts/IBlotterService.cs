using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface IBlotterService
    {
        void Initialize();

        Task GetBlotterData(RequestResponseModel requestResponseObj);

        Task CancelAllSubOrders(RequestResponseModel requestResponseObj);

        Task RolloverAllSubOrders(RequestResponseModel requestResponseObj);

        Task RemoveOrders(RequestResponseModel requestResponseObj);

        Task FreezeOrdersInPendingComplianceUI(RequestResponseModel requestResponseObj);

        Task UnfreezeOrdersInPendingComplianceUI(RequestResponseModel requestResponseObj);

        Task RemoveManualExecution(RequestResponseModel requestResponseObj);

        Task GetBlotterManualFills(RequestResponseModel requestResponseObj);

        Task SaveManualFills(RequestResponseModel requestResponseObj);

        Task GetAllocationDetails(RequestResponseModel requestResponseObj);

        Task GetPstAllocationDetails(RequestResponseModel requestResponseObj);

        Task SaveAllocationDetails(RequestResponseModel requestResponseObj);

        Task RenameBlotterCustomTab(RequestResponseModel requestResponseObj);

        Task RemoveBlotterCustomTab(RequestResponseModel requestResponseObj);

        Task GetPstData(RequestResponseModel requestResponseObj);

        void TradingAccountsDataHandler(string topic, RequestResponseModel message);

        Task TransferUser(RequestResponseModel requestResponseObj);

        Task GetOrderDetailsForEditTradeAttributes(RequestResponseModel requestResponseObj);

        Task SaveEditedTradeAttributes(RequestResponseModel requestResponseObj);
    }
}