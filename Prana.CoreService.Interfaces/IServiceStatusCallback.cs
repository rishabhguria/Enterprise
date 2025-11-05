using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract]
    public interface IServiceStatusCallback
    {
        [OperationContract(IsOneWay = true)]
        void HeartbeatReceived();

        [OperationContract(IsOneWay = true)]
        void ServiceClosed();
    }
}
