using Prana.BusinessObjects;
using Prana.ServiceCommon.Interfaces;
using System.ServiceModel;

namespace Prana.PubSubService.Interfaces
{
    [ServiceContract]
    public interface IPublishing : IServiceOnDemandStatus
    {
        [OperationContract(IsOneWay = true)]
        void Publish(MessageData e, string topicName);

        [OperationContract]
        string getReceiverUniqueName();
    }
}
