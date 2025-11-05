using Microsoft.AspNetCore.SignalR;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Models;
using Serilog.Context;
using System.Text.Json;

namespace Prana.ServiceGateway.Services
{
    public class LiveFeedService : ILiveFeedService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly ILogger<LiveFeedService> logger;

        public LiveFeedService(IKafkaManager kafkaManager,
            IHubManager hubManager,
            ILogger<LiveFeedService> logger)
        {
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
            this.logger = logger;
        }

        public void Initialize()
        {
            try
            {
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_LiveFeedResponse, LiveFeedResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_MarketDataPermissionResponse, MarketDataPermissionResponseHandler);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error in LiveFeed initialize method");
            }
        }

        public async Task RequestSymbol(string data, int companyUserId)
        {
            try
            {
                var reqObj = new RequestResponseModel(companyUserId, data, null);

                using (LogContext.PushProperty(Constants.LogConstant.KAFKA_REQUEST_ID, reqObj.RequestID))
                    await _kafkaManager.Produce(KafkaConstants.TOPIC_LiveFeedRequest, reqObj);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in LiveFeed RequestSymbol method");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task ReqMultipleSymbolsLiveFeedSnapshotData(MultipleSymbolRequestDto request, int companyUserId)
        {
            var cleanedSymbols = request.RequestedSymbols
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList();

            var payloadData = new
            {
                RequestedSymbols = cleanedSymbols,
                RequestedInstance = request.RequestedInstance
            };

            var payloadJson = JsonSerializer.Serialize(payloadData); 

            var payload = new RequestResponseModel(companyUserId, payloadJson, null);

            await _kafkaManager.Produce(KafkaConstants.TOPIC_MultipleSymbolsLiveFeedSnapshotRequest, payload);
        }

        public async Task UpdateMarketDataTokenRequest(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(
                    KafkaConstants.TOPIC_UpdateMarketDataTokenRequest,
                    requestResponseObj);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in LiveFeed UpdateMarketDataTokenRequest method");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        private void LiveFeedResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_LiveFeedResponse, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in LiveFeed LiveFeedResponseHandler method");
            }
        }

        private void MarketDataPermissionResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                PermissionManager.GetInstance().AddOrUpdateMarketDataPermissionCache(message.CompanyUserID, message.Data);
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_MarketDataPermissionResponse + "_" + message.CompanyUserID.ToString(), message.Data, message.CompanyUserID, message.CorrelationId);
                
                //Front end handling for FactSet specific use case (in case of other provider, there is no listner in front end)
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_MarketDataPermissionResponseFactSet + "_" + message.CompanyUserID.ToString(), message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in LiveFeed MarketDataPermissionResponseHandler method");
            }
        }

        public async Task UnSubscribeLiveFeed(RequestResponseModel reqObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_UnSubscribeLiveFeedRequest, reqObj);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in LiveFeed UnSubscribeLiveFeed method");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
    }
}
