using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Models;
using System.Text.Json;

namespace Prana.ServiceGateway.Services
{
    public sealed class ServiceHealthStatusSubscriber : BackgroundService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly ServiceHealthStatusStore _serviceHealthStatusStore;
        private readonly ILogger<ServiceHealthStatusSubscriber> _logger;

        public ServiceHealthStatusSubscriber(
            IKafkaManager kafkaManager,
            ServiceHealthStatusStore store,
            ILogger<ServiceHealthStatusSubscriber> logger)
        {
            _kafkaManager = kafkaManager;
            _serviceHealthStatusStore = store;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_ServiceHealthStatus, OnMessage);

                    // Block until cancellation is requested
                    stoppingToken.WaitHandle.WaitOne();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while consuming Kafka messages.");
                }
            }, stoppingToken);
        }

        private void OnMessage(string topic, RequestResponseModel message)
        {
            try
            {
                var statusDto = JsonSerializer.Deserialize<ServiceStatusDto>(message.Data);
                if (statusDto != null && !string.IsNullOrWhiteSpace(statusDto.ServiceName))
                {
                    _serviceHealthStatusStore.UpsertStatus(statusDto.ServiceName, statusDto);
                }
                else
                {
                    _logger.LogWarning("Received invalid ServiceStatusDto from topic {Topic}.", topic);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling service health status message from {Topic}.", topic);
            }
        }
    }
}
