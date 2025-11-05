using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Utility;

namespace Prana.ServiceGateway.Services
{
    public class BlotterService : IBlotterService, IDisposable
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly ILogger<BlotterService> _logger;

        public BlotterService(IKafkaManager kafkaManager,
            IHubManager hubManager,
            ILogger<BlotterService> logger)
        {
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
            this._logger = logger;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            try
            {
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterUpdatedData, BlotterUpdatedDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemovedData, BlotterRemovedDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_OrderUpdatesData, OrderUpdatesDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, RenameBlotterCustomTabHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, RemoveBlotterCustomTabHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, RemoveOrdersHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, FreezeOrdersInPendingComplianceUIHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, UnfreezeOrdersInPendingComplianceUIHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetAllocationDetailsResponse, GetAllocationDetailsHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, SaveAllocationDetailsHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetPstAllocationDetailsResponse, GetPstAllocationDetailsHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, CancelAllSubOrdersHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, RolloverAllSubOrdersHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetManualFillsResponse, GetBlotterManualFillsHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, RemoveManualExecutionHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, SaveManualFillsHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetDataResponse, GetBlotterDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsResponse, GetPstDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseTradingAccountsRequest, TradingAccountsDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserResponse, TransferUserHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserFromRequest, TransferUserFromHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, GetOrderDetailsForEditTradeAttributesResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, SaveEditedTradeAttributesResponseHandler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Initialize of BlotterService");
            }
        }

        /// <summary>
        /// GetBlotterData
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task GetBlotterData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterGetDataRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBlotterData");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetPstData
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetPstData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPstData");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// BlotterUpdatedDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void BlotterUpdatedDataHandler(string topic, RequestResponseModel message)
        {
            try
            {               
                Dictionary<string, string> userWiseBlotterData = PermissionManager
                    .GetInstance()
                    .GetPermittedUserToSendData(message.Data);

                foreach (KeyValuePair<string, string> blotterData in userWiseBlotterData)
                {
                    _logger.LogDebug("Sending BlotterUpdatedDataHandler event for user {user}", blotterData.Key);
                    _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_BlotterUpdatedData + "_" + blotterData.Key, blotterData.Value,message.CompanyUserID,message.CorrelationId);                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BlotterUpdatedDataHandler");
            }
        }

        /// <summary>
        /// BlotterRemovedDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void BlotterRemovedDataHandler(string topic, RequestResponseModel message)
        {
            try
            {                
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_BlotterRemovedData, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BlotterRemovedDataHandler");
            }
        }

        /// <summary>
        /// Cancel all subs
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CancelAllSubOrders(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterCancelAllSubsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CancelAllSubOrders");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Transfer User
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task TransferUser(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_TransferUserRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CancelAllSubOrders");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Rollover all subs
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RolloverAllSubOrders(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterRolloverAllSubsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RolloverAllSubOrders");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Remove orders
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RemoveOrders(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterRemoveOrdersRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveOrders");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Freeze order(s) in Pending Compliance UI.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task FreezeOrdersInPendingComplianceUI(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_FreezePendingComplianceRowsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FreezeOrdersInPendingComplianceUI");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Unfreeze order(s) in Pending Compliance UI.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UnfreezeOrdersInPendingComplianceUI(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UnfreezeOrdersInPendingComplianceUI");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Remove manual execution
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RemoveManualExecution(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterRemoveManualExecutionRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveManualExecution");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetBlotterManualFills
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task GetBlotterManualFills(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterGetManualFillsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBlotterManualFills");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveAddModifyFills
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task SaveManualFills(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BlotterSaveManualFillsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveManualFills");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetAllocationDetails
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task GetAllocationDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetAllocationDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllocationDetails");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveAllocationDetails
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task SaveAllocationDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveAllocationDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveAllocationDetails");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetPstAllocationDetails
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetPstAllocationDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetPstAllocationDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPstAllocationDetails");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// RenameBlotterCustomTab
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task RenameBlotterCustomTab(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_RenameBlotterCustomTabRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RenameBlotterCustomTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// RemoveBlotterCustomTab
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task RemoveBlotterCustomTab(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_RemoveBlotterCustomTabRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveBlotterCustomTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Asynchronously publishes a request message to Kafka to retrieve order details
        /// required for editing trade attributes.
        /// </summary>
        /// <param name="requestResponseObj">The request and response model containing
        /// necessary data for the order details request.</param>
        public async Task GetOrderDetailsForEditTradeAttributes(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveBlotterCustomTab");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Asynchronously publishes a request message to Kafka to retrieve order details
        /// required for saving trade attributes.
        /// </summary>
        /// <param name="requestResponseObj">The request object containing the updated trade attributes data.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task SaveEditedTradeAttributes(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveEditedTradeAttributesRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveEditedTradeAttributes");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>SaveEditedTradeAttributes
        /// OrderUpdatesDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void OrderUpdatesDataHandler(string topic, RequestResponseModel message)
        {
            try
            {                
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_OrderUpdatesData, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OrderUpdatesDataHandler");
            }
        }

        /// <summary>
        /// RenameBlotterCustomTabHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RenameBlotterCustomTabHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RenameBlotterCustomTabHandler");
            }
        }

        /// <summary>
        /// RemoveBlotterCustomTabHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RemoveBlotterCustomTabHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveBlotterCustomTabHandler");
            }
        }

        /// <summary>
        /// RemoveOrdersHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RemoveOrdersHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveOrdersHandler");
            }
        }

        /// <summary>
        /// FreezeOrdersInPendingComplianceUIHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void FreezeOrdersInPendingComplianceUIHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FreezeOrdersInPendingComplianceUIHandler");
            }
        }

        /// <summary>
        /// UnfreezeOrdersInPendingComplianceUIHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void UnfreezeOrdersInPendingComplianceUIHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UnfreezeOrdersInPendingComplianceUIHandler");
            }
        }

        /// <summary>
        /// GetAllocationDetailsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetAllocationDetailsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_GetAllocationDetailsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllocationDetailsHandler");
            }
        }

        /// <summary>
        /// SaveAllocationDetailsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveAllocationDetailsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveAllocationDetailsHandler");
            }
        }

        /// <summary>
        /// GetPstAllocationDetailsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetPstAllocationDetailsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_GetPstAllocationDetailsResponse, message);
                dynamic data = JsonConvert.DeserializeObject<dynamic>(message?.Data);
                topicToListen = topicToListen + "_" + data.Identifier;
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPstAllocationDetailsHandler");
            }
        }

        /// <summary>
        /// CancelAllSubOrdersHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void CancelAllSubOrdersHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CancelAllSubOrdersHandler");
            }
        }

        /// <summary>
        /// TransferUserFromHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void TransferUserFromHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_TransferUserFromResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TransferUserFromHandler");
            }
        }

        /// <summary>
        /// TransferUserHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void TransferUserHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string uniqueIdentifier = string.Empty;
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_TransferUserResponse, message);
                if (data.ContainsKey("uniqueIdentifier") && message.CompanyUserID != 0)
                {
                    uniqueIdentifier = data["uniqueIdentifier"];
                    topicToListen = topicToListen + "_" + uniqueIdentifier;
                }
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TransferUserHandler");
            }
        }

        /// <summary>
        /// RolloverAllSubOrdersHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RolloverAllSubOrdersHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RolloverAllSubOrdersHandler");
            }
        }

        /// <summary>
        /// GetBlotterDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetBlotterDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string blotterId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_BlotterGetDataResponse;
                if (data.ContainsKey("blotterId") && message.CompanyUserID != 0)
                {
                    blotterId = data["blotterId"];
                    topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + blotterId;
                }
                Console.WriteLine("SignalR response sent on topic: " + topicToListen);
                var compressedBlotterData = HelperFunctions.CompressData(message.Data);
                message.Data = String.Empty;
                message.CustomData = compressedBlotterData;
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBlotterDataHandler");
            }
        }

        /// <summary>
        /// GetPstDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetPstDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string blotterId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsResponse;
                if (message.CompanyUserID != 0)
                {
                    topicToListen = topicToListen + "_" + message.CompanyUserID;
                }
                Console.WriteLine("SignalR response sent on topic: " + topicToListen);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPstDataHandler");
            }
        }

        /// <summary>
        /// GetBlotterManualFillsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetBlotterManualFillsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BlotterGetManualFillsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBlotterManualFillsHandler");
            }
        }

        /// <summary>
        /// RemoveManualExecutionHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RemoveManualExecutionHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveManualExecutionHandler");
            }
        }

        /// <summary>
        /// SaveManualFillsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveManualFillsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveManualFillsHandler");
            }
        }

        /// <summary>
        /// TradingAccountsDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        public void TradingAccountsDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_UserWiseTradingAccountsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TradingAccountsDataHandler");
            }
        }

        /// <summary>
        /// Handles the response for the Edit Trade Attributes request by sending the data to the appropriate SignalR hub topic.
        /// </summary>
        /// <param name="Topic">The Kafka topic from which the response was received.</param>
        /// <param name="message">The response message containing user and request data.</param>
        public void GetOrderDetailsForEditTradeAttributesResponseHandler(string Topic, RequestResponseModel message)
        {
            try
            {
                _logger.LogInformation("GetOrderDetailsForEditTradeAttributes response received. Topic: {Topic}, CompanyUserID: {CompanyUserID}, CorrelationID: {CorrelationID}", Topic, message.CompanyUserID, message.CorrelationId);
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation("Message successfully sent to topic: {Topic}, CompanyUserID: {CompanyUserID}, CorrelationID: {CorrelationID}", topicToListen, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrderDetailsForEditTradeAtteibutes");
            }
        }

        /// <summary>
        /// Handles the response for the SaveEditedTradeAttributes Kafka topic.
        /// </summary>
        /// <param name="Topic">The Kafka topic from which the response was received.</param>
        /// <param name="message">The deserialized response message containing correlation and user context.</param>

        public void SaveEditedTradeAttributesResponseHandler(string Topic, RequestResponseModel message)
        {
            try
            {
                _logger.LogInformation("SaveEditedTradeAttributes response received. Topic: {Topic}, CompanyUserID: {CompanyUserID}, CorrelationID: {CorrelationID}", Topic, message.CompanyUserID, message.CorrelationId);
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation("Message successfully sent to topic: {Topic}, CompanyUserID: {CompanyUserID}, CorrelationID: {CorrelationID}", topicToListen, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrderDetailsForEditTradeAtteibutes");
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
