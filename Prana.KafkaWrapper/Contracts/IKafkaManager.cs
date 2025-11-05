using System;
using System.Threading.Tasks;

namespace Prana.KafkaWrapper.Contracts
{
    public interface IKafkaManager
    {
        void Initialize(string configPath);

        Task<bool> CreateTopics();

        Task<string> ProduceAndConsume(RequestResponseModel requestResponseObj, string produceTopic, string consumeTopic, bool isEarliest = false);

        Task Produce(string produceTopic, RequestResponseModel requestResponseObj, bool isLoggingRequired = true);

        void SubscribeAndConsume(string consumeTopic, Action<string, RequestResponseModel> callback, bool isEarliest = false);

        void Cleanup(NirvanaConsumer[] nirvanaConsumers);
    }
}
