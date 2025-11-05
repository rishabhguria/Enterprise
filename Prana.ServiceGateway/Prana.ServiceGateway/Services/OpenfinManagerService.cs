using Microsoft.AspNetCore.SignalR;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Utility;

namespace Prana.ServiceGateway.Services
{
    public class OpenfinManagerService : IOpenfinManagerService
    {

        private readonly IKafkaManager _kafkaManager;
        private readonly ILogger<OpenfinManagerService> _logger;
        private readonly IHubManager _hubManager;
        private readonly IHubManagerRTPNL _hubManagerRTPNL;

        public OpenfinManagerService(IKafkaManager kafkaManager,
            ILogger<OpenfinManagerService> logger,
            IHubManager hubManager,
            IHubManagerRTPNL hubManagerRTPNL)
        {
            _kafkaManager = kafkaManager;
            this._logger = logger;
            _hubManager = hubManager;
            _hubManagerRTPNL = hubManagerRTPNL;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutResponse, GetOpenfinWorkspaceLayoutHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutResponse, SaveOpenfinWorkspaceLayoutHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutResponse, DeleteOpenfinWorkspaceLayoutHandler);
        }


        public async Task GetOpenfinWorkspaceLayout(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task SaveOpenfinWorkspaceLayout(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task DeleteOpenfinWorkspaceLayout(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetOpenfinWorkspaceLayoutHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetOpenfinWorkspaceLayoutHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutResponse, message);
                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOpenfinWorkspaceLayoutHandler");
            }
        }

        /// <summary>
        /// SaveOpenfinWorkspaceLayoutHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveOpenfinWorkspaceLayoutHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutResponse, message);
                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveOpenfinWorkspaceLayoutHandler");
            }
        }

        /// <summary>
        /// DeleteOpenfinWorkspaceLayoutHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void DeleteOpenfinWorkspaceLayoutHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_DeleteOpenfinWorkspaceLayoutResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteOpenfinWorkspaceLayoutHandler");
            }
        }
    }
}
