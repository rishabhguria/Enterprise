using System.ServiceModel;

namespace Nirvana.TestAutomation.Interfaces
{
    [ServiceContract]
    public interface IAccessBridgeService
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string commandType, string message);
    }
}
