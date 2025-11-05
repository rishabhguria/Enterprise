using Prana.LogManager;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface IServiceStatus
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool?> Subscribe(string subscriberName, bool isRetryRequest);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task UnSubscribe(string subscriberName);
    }
}
