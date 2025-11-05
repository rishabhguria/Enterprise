using Prana.BusinessObjects;
using Prana.ServiceCommon.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.PubSubService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IPublishing))]
    public interface ISubscription : IServiceOnDemandStatus
    {
        [OperationContract]
        void Subscribe(string topicName, List<FilterData> Filterdata);

        [OperationContract]
        void UnSubscribe(string topicName);
    }
}