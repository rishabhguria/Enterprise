
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
using Serilog.Context;

namespace Prana.ServiceGateway.Services
{
    public class RtpnlService : IRtpnlService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManagerRTPNL _hubManagerRTPNL;
        private readonly IHubManagerRTPNLUpdates _hubManagerRTPNLUpdates;
        private readonly IHubManager _hubManagerNormal;
        private readonly ILogger<RtpnlService> _logger;

        public RtpnlService(IKafkaManager kafkaManager,
             IHubManagerRTPNL hubManager,
             ILogger<RtpnlService> logger,
             IHubManager hubManager1,
             IHubManagerRTPNLUpdates hubManagerRTPNLUpdates)
        {
            _kafkaManager = kafkaManager;
            _hubManagerRTPNL = hubManager;
            this._logger = logger;
            _hubManagerNormal = hubManager1;
            _hubManagerRTPNLUpdates = hubManagerRTPNLUpdates;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved, RtpnlRowCalculationRemovedHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, CheckCalculationServiceIsRunningHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, SaveUpdateConfigDetailsHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, SaveConfigDataForExtractHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetDataResponse, GetRtpnlWidgetDataHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, GetRtpnlWidgetConfigDataHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, RtpnlRowCalculationUpdatesHandler);
            _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, RtpnlTradeAttributeLabelsResponse);
        }

        /// <summary>
        /// RtpnlRowCalculationUpdatesHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RtpnlRowCalculationUpdatesHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<string, dynamic> updatesInBatch = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message.Data);
                Dictionary<int, Dictionary<string, dynamic>> filteredBatchUpdatesByUser = new Dictionary<int, Dictionary<string, dynamic>>();
                foreach (KeyValuePair<string, dynamic> update in updatesInBatch)
                {
                    List<int> filteredUsers = PermissionManager.GetInstance().FilterAppliedForDataUpdates(update);

                    foreach (int userId in filteredUsers)
                    {
                        if (HubClientConnectionManagerRTPNLUpdates.UserExistsInRTPNLUpdatesList(userId))
                        {
                            if (!filteredBatchUpdatesByUser.ContainsKey(userId))
                            {
                                filteredBatchUpdatesByUser[userId] = new Dictionary<string, dynamic>();
                            }

                            // Add the update to the user's dictionary
                            filteredBatchUpdatesByUser[userId][update.Key] = update.Value;
                        }
                    }
                }

                foreach (KeyValuePair<int, Dictionary<string, dynamic>> userWiseRows in filteredBatchUpdatesByUser)
                {
                    var compressedRTPNLData = HelperFunctions.CompressData(JsonConvert.SerializeObject(userWiseRows.Value));
                    Dictionary<string, dynamic> batchToSend = new Dictionary<string, dynamic>{{ "data", compressedRTPNLData } };
                   
                    _hubManagerRTPNLUpdates.SendUserBasedMessage(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates + "_" + userWiseRows.Key.ToString(), JsonConvert.SerializeObject(batchToSend), userWiseRows.Key, message.CorrelationId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RtpnlRowCalculationUpdatesHandler");
            }
        }

        /// <summary>
        /// Save and Update the Configuration details
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task SaveUpdateConfigDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveUpdateConfigDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveUpdateConfigDetails");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);

            }
        }

        /// <summary>
        /// Save the Configuration details for Extract
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task SaveConfigDataForExtract(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveConfigDataForExtractRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveConfigDataForExtract");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);

            }
        }

        /// <summary>
        /// GetRtpnlWidgetData
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task GetRtpnlWidgetData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_RtpnlWidgetDataRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRtpnlWidgetData");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetRtpnlConfigWidgetData
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        public async Task GetRtpnlWidgetConfigData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_RtpnlWidgetConfigDataRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRtpnlWidgetConfigData");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// RtpnlRowCalculationRemovedHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void RtpnlRowCalculationRemovedHandler(string topic, RequestResponseModel message)
        {
            try
            {
                KeyValuePair<string, dynamic> parsedData = JsonConvert.DeserializeObject<KeyValuePair<string, dynamic>>(message.Data);
                string parsedDataKey = parsedData.Key;
                var filteredUsers = PermissionManager.GetInstance().FilterAppliedForRemovedKeys(parsedDataKey);
                foreach (var user in filteredUsers)
                {
                    if (HubClientConnectionManagerRTPNLUpdates.UserExistsInRTPNLUpdatesList(user))
                    {
                        _hubManagerRTPNLUpdates.SendUserBasedMessage(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved + "_" + user, message.Data, user, message.CorrelationId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RtpnlRowCalculationRemovedHandler");
            }
        }

        /// <summary>
        /// RtpnlMasterFundUpdatesHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void CheckCalculationServiceIsRunningHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManagerNormal.SendUserBasedMessage(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckCalculationServiceIsRunningHandler");
            }
        }

        /// <summary>
        /// CheckCalculationServiceRunning
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CheckCalculationServiceRunning(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(
                    KafkaConstants.TOPIC_CheckCalculationServicIsRunningRequest,
                    requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckCalculationServiceRunning");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetRtpnlWidgetConfigDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetRtpnlWidgetConfigDataHandler(string topic , RequestResponseModel message)
        {
            try
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);
                string viewId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse;

                if (data.ContainsKey("ViewId") && message.CompanyUserID != 0)
                {
                    viewId = data["ViewId"];
                    topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + viewId;
                }

                Console.WriteLine("SignalR response: " + topicToListen);
                _logger.LogInformation("SignalR response: " + topicToListen);
                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRtpnlWidgetConfigDataHandler");
            }
        }

        /// <summary>
        /// GetRtpnlWidgetDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetRtpnlWidgetDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);
                Dictionary<string, string> startUpData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data["StartUpGridData"]);

                foreach (KeyValuePair<string, string> keyValuePair in startUpData)
                {
                    var filteredData = PermissionManager.GetInstance().FilterDataBasedOnPermittedAccountsForStartUpData(keyValuePair.Value, message.CompanyUserID);
                    startUpData[keyValuePair.Key] = JsonConvert.SerializeObject(filteredData);
                }

                data["StartUpGridData"] = JsonConvert.SerializeObject(startUpData);
                bool isUnallocatedAccountPermission = PermissionManager.GetInstance().GetUserUnallocatedPermission(message.CompanyUserID);
                List<int> permittedMasterFunds =  new List<int>(PermissionManager.GetInstance().GetUserPermittedMasterFunds(message.CompanyUserID));
                if (isUnallocatedAccountPermission)
                {
                    permittedMasterFunds.Add(-1);
                }
                data["UserPermittedMasterFunds"] = JsonConvert.SerializeObject(permittedMasterFunds);

                var compressedRTPNLData = HelperFunctions.CompressData(JsonConvert.SerializeObject(data));
                message.Data = string.Empty;
                message.CustomData = compressedRTPNLData;
                string topicToListen = KafkaConstants.TOPIC_RtpnlWidgetDataResponse + "_" + message.CompanyUserID;

                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRtpnlWidgetDataHandler");
            }
        }

        /// <summary>
        /// Deletes removed widgets Config details from database
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteRemovedWidgetConfigDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_DeleteRemovedWidgetConfigDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveUpdateConfigDetailsHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveUpdateConfigDetailsHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, message);
                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveUpdateConfigDetailsHandler");
            }
        }

        /// <summary>
        /// SaveConfigDataForExtractHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SaveConfigDataForExtractHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, message);
                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveConfigDataForExtractHandler");
            }
        }

        /// <summary>
        /// Produces a Kafka event to request trade attribute labels.
        /// </summary>
        public async Task GetRtpnlTradeAttributeLabels(RequestResponseModel requestResponseObj)
        {
            try
            {
                using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
                using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
                await _kafkaManager.Produce(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsRequest, requestResponseObj);
                _logger.LogInformation("Trade Attribute labels Rtpnl event is produce with CoorelationId {0}", requestResponseObj?.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRtpnlTradeAttributeLabels");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Handles the response for trade attribute labels received from Kafka and forwards it to the user via SignalR.
        /// </summary>
        private void RtpnlTradeAttributeLabelsResponse(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManagerRTPNL.SendUserBasedMessage(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, message.Data, message.CompanyUserID, message.CorrelationId);
                _logger.LogInformation("Received Rtpnl TradeAttributeDetails response for signalR with CorrelationId: {0}", message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                _logger.LogError(ex, "Error in RtpnlTradeAttributeLabelsResponse");
            }
        }
    }
}
