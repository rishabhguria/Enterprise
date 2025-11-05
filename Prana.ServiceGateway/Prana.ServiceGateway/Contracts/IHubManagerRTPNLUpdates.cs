using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.Contracts
{
    public interface IHubManagerRTPNLUpdates
    {
        public void SendUserBasedMessage(string topic, RequestResponseModel message);

        public void SendUserBasedMessage(string topic, string data, int userId, string CorrelationId, bool isCompressedData = false);

        public void BroadcastMessage(RequestResponseModel message, string topic);

        public void BroadcastMessage(string data, string topic, int Userid);
    }
}
