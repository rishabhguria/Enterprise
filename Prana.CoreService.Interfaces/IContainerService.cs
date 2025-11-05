using Prana.BusinessObjects;
using Prana.LogManager;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract]
    public interface IContainerService
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RequestStartupData();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<byte[]> OpenLog();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<byte[]> LoadLog();

        [OperationContract(IsOneWay = true)]
        System.Threading.Tasks.Task StopService();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool> GetDebugModeStatus();
    }
}
