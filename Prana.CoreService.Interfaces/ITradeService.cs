using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface ITradeService : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task ReloadRules();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task ReloadXslt();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool> IsTradeServiceReadyForClose();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task GetMessageStatus(Order order);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task MoveOldTrade();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> FetchProcessorNames();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<OrderCollection> ShowMessagesForProcessor(string processorName);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<OrderCollection> ShowErrorMessagesForProcessor(string processorName);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> PersistedMessagesReceivedFromClient();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> PersistedMessagesSentToBroker();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> PersistedMessagesReceivedFromBroker();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<OrderCollection> PendingPreTradeCompliance();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<OrderCollection> PendingApprovalTrades();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RefreshCacheClosing();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RefreshPreferenceCache();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SendManualDropsOnFix();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task ClearFixTradeOrderCache();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task ClearFixTradeOrder(string orderID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<int, FixPartyDetails>> GetFixAllPartyDetails();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task OverideTrade(bool isAllowed, string orderId);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task ReProcessMsg(string jsonDataRow);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task ReProcessMsg2(Order pranaOrder);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task FixEngineConnectBuySide(int connectionID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task FixEngineDisconnectBuySide(int connectionID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SetFixConnectionsAutoReconnectStatus(bool autoConnectStatus);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool> IsComplianceModulePermitted();
    }
}