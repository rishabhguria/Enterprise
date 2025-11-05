using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
    public class LayoutService : ILayoutService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly ILogger<LayoutService> _logger;
        private readonly IHubManagerRTPNL _hubManager;
        private readonly IHubManager _hubManagerNormal;


        public LayoutService(IKafkaManager kafkaManager,
            ILogger<LayoutService> logger,
            IConfiguration configuration,
            IHubManagerRTPNL hubManager,
            IHubManager hubManager1)
        {
            _kafkaManager = kafkaManager;
            this._logger = logger;
            _hubManager = hubManager;
            _hubManagerNormal = hubManager1;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_LoadViewsResponse, LoadRtpnlViewsHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveLayoutResponse, SaveLayoutHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteOpenfinPageResponse, DeleteOpenfinPagesHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveOrUpdateViewsResponse, SaveOrUpdateRtpnlViewsHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_LoadLayoutResponse, LoadLayoutHandler);
        }

        /// <summary>
        /// LoadLayout
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task LoadLayout(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_LoadLayoutRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoadLayout");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveLayout
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task SaveLayout(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveLayoutRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveLayout");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveOrUpdateRtpnlViews
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task SaveOrUpdateRtpnlViews(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveOrUpdateViewsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveOrUpdateRtpnlViews");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// LoadRtpnlViews
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task LoadRtpnlViews(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_LoadViewsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoadRtpnlViews");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// DeleteOpenfinPages
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task DeleteOpenfinPages(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_DeleteOpenfinPageRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteOpenfinPages");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// LoadRtpnlViewsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void LoadRtpnlViewsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_LoadViewsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoadRtpnlViewsHandler");
            }
        }

        /// <summary>
        /// SaveLayoutHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveLayoutHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveLayoutResponse, message);
                string viewId = ServicesMethodConstants.CONST_VIEW_ID;
                if (data.ContainsKey(viewId) && data[viewId] == ServicesMethodConstants.CONST_UNIQUE_GROUP_LEVEL_CHIPS_VIEW_ID)
                {
                    topicToListen = topicToListen + ServicesMethodConstants.CONST_SEPARATOR + data[viewId];
                    _hubManager.SendUserBasedMessage(topicToListen, message);
                    return;
                }
                _hubManagerNormal.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveLayoutHandler");
            }
        }

        /// <summary>
        /// LoadLayoutHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void LoadLayoutHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string blotterId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_LoadLayoutResponse;
                if (data.ContainsKey("moduleId") && message.CompanyUserID != 0)
                {
                    blotterId = data["moduleId"];
                    topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + blotterId;
                }
                Console.WriteLine("SignalR response sent on topic: " + topicToListen);
                _hubManagerNormal.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoadLayoutHandler");
            }
        }

        /// <summary>
        /// DeleteOpenfinPagesHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void DeleteOpenfinPagesHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_DeleteOpenfinPageResponse, message);
                Console.WriteLine("SignalR response sent on topic: " + topicToListen);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteOpenfinPagesHandler");
            }
        }

        /// <summary>
        /// SaveOrUpdateRtpnlViewsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveOrUpdateRtpnlViewsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveOrUpdateViewsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveOrUpdateRtpnlViewsHandler");
            }
        }
		
		public async Task RemovePagesForAnUser(RequestResponseModel message)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_RemovePagesForAnUser, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemovePagesForAnUser");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}
