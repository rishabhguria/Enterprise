using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Utility;
using Serilog.Context;
using Newtonsoft.Json;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.CacheStore;
using Newtonsoft.Json.Linq;

namespace Prana.ServiceGateway.Services
{
    public class TradingService : ITradingService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly ILogger<TradingService> _logger;
        private readonly IHubManagerRTPNL _hubManagerRTPNL;
        private readonly ServiceHealthStatusStore serviceHealthStatusStore;

        public TradingService(IKafkaManager kafkaManager,
            IHubManager hubManager,
            ILogger<TradingService> logger,
            IHubManagerRTPNL hubManagerRTPNL)
        {
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
            this._logger = logger;
            _hubManagerRTPNL = hubManagerRTPNL;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Initialize()
        {
            try
            {
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerStatusResponse, BrokerStatusResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SymbolAccountWisePositionResponse, SymbolAccountWisePositionResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, ShortLocateOrdersSymbolWiseResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, CompanyUserHotKeyPreferencesResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, CompanyUserHotKeyPreferencesDetailsResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderResponse, SendStageOrderHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SendManualOrderResponse, SendManualOrderHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SendReplaceOrderResponse, SendReplaceOrderHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SendLiveOrderResponse, SendLiveOrderHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetSMDetailsResponse, GetSMDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePopUpTextResponse, CreatePopupTextHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CreateOptionSymbolResponse, CreateOptionSymbolHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CustomAllocationDetailsResponse, GetCustomAllocationHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse, GetSavedCustomAllocationHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, GetAlgoStrategiesFromBrokerHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataResponse, GetBrokerConnectionAndVenuesDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, DetermineBorrowTypeResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse, GetRegionOfBrokerFrSymbolAUECIDResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasketResponse, GetAccountNavNStartingValueFromBasketResponse);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePstAllocationPrefResponse, CreatePstAllocationPrefResponse);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeLabelsResponse, KafkaManager_TradeAttributeLabelsResponse);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeValuesResponse, KafkaManager_TradeAttributeValuesResponse);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineMultipleSecurityBorrowTypeResponse, DetermineMultipleBorrowTypeResponseHandler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
            }
        }

        private void DetermineMultipleBorrowTypeResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                using (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, message?.CorrelationId))
                {
                    string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_DetermineMultipleSecurityBorrowTypeResponse, message);
                    _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
                    _logger.LogInformation("MultipleSecurityBorrowTypeResponse:{0}", message.Data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DetermineBorrowTypeResponseHandler encountered an error");
            }
        }

        public async Task DetermineMultipleSecurityBorrowType(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_DetermineMultipleSecurityBorrowTypeRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetermineMultipleSecurityBorrowType");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        private void GetRegionOfBrokerFrSymbolAUECIDResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                using (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, message?.CorrelationId))
                {
                    _hubManager.SendUserBasedMessage(ServicesMethodConstants.CONST_GET_REGION_OF_BROKER_FR_SYMBOL_AUECID_RESPONSE, message.Data, message.CompanyUserID, message.CorrelationId);
                    _logger.LogInformation("Sending region of broker from symbol AUECID in signalR with message Data:{0}", message.Data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRegionOfBrokerFrSymbolAUECIDResponseHandler ecnountered an error");
            }
        }

        private void DetermineBorrowTypeResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                using (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, message?.CorrelationId))
                {
                    string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, message);
                    _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
                    _logger.LogInformation("SecurityBorrowTypeResponse:{0}", message.Data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DetermineBorrowTypeResponseHandler encountered an error");
            }
        }

        /// <summary>
        /// Get Connected Broker Coonection/Disconnection Details.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private void BrokerStatusResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                using (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, message?.CorrelationId))
                {
                    if (HelperFunctions.IsBrokerConnectionStatusChanged(message))
                    {
                        _hubManager.SendUserBasedMessage(ServicesMethodConstants.CONST_BROKER_STATUS_RESPONSE, message.Data, message.CompanyUserID, message.CorrelationId);
                        _logger.LogTrace("Broker Connection Status Changed:{0}", message?.Data);
                        TradingTicketCache.GetInstance().UpdateBrokerCacheData(message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BrokerStatusResponseHandler ecnountered an error");
            }
        }

        /// <summary>
        /// Send stage Order from Web TT.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendStageOrderFromTT(RequestResponseModel requestResponseObj)
        {
            using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
            using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendStageOrderRequest, requestResponseObj);
                _logger.LogInformation("Stage order response message send to UI successfully for, User ID {0}, RequestResponseModel Data: {1}", requestResponseObj?.CompanyUserID.ToString(), requestResponseObj?.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendStageOrderFromTT");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Send Manual Order from Web TT.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendManualOrderFromTT(RequestResponseModel requestResponseObj)
        {
            using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
            using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendManualOrderRequest, requestResponseObj);
                _logger.LogInformation("Method Name: SendManualOrderFromTT, User ID {0}, RequestResponseModel Data: {1}", requestResponseObj?.CompanyUserID.ToString(), requestResponseObj?.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendManualOrderFromTT");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Send Live Order from Web TT.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendLiveOrderFromTT(RequestResponseModel requestResponseObj)
        {
            using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
            using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendLiveOrderRequest, requestResponseObj);
                _logger.LogInformation("Method Name: SendLiveOrderFromTT, User ID {0}, RequestResponseModel Data: {1}", requestResponseObj?.CompanyUserID.ToString(), requestResponseObj?.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendLiveOrderFromTT");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Gets EXPNL based values of a symbol.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetSymbolAccountWisePosition(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SymbolAccountWisePositionRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSymbolAccountWisePosition");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Gets Symbol-Wise Short Locate Orders.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetSymbolWiseShortLocateOrders(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSymbolWiseShortLocateOrders");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// DetermineSecurityBorrowTime
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DetermineSecurityBorrowType(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetermineSecurityBorrowType");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        private void SymbolAccountWisePositionResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_SymbolAccountWisePositionResponse, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SymbolAccountWisePositionResponseHandler");
            }
        }

        private void ShortLocateOrdersSymbolWiseResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ShortLocateOrdersSymbolWiseResponseHandler");
            }
        }

        ///<summary>
        /// Gets Company User Hot Key Preferences.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetCompanyUserHotKeyPreferences(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Update Company User Hot Key Preferences.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateCompanyUserHotKeyPreferences(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        private void CompanyUserHotKeyPreferencesResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        ///<summary>
        /// Gets Company User Hot Key Preferences Details.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Update Company User Hot Key Preferences Details.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Save Company User Hot Key Preferences Details.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SaveCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SaveCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Delete Company User Hot Key Preferences Details.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteCompanyUserHotKeyPreferencesDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_DeleteCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        private void CompanyUserHotKeyPreferencesDetailsResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// Create popup text for the trade
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CreatePopUpText(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CreatePopUpTextRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Create Option Symbol with underlying symbol
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateOptionSymbol(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CreateOptionSymbolRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get AlgoStrategies From Broker
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetAlgoStrategiesFromBroker(RequestResponseModel requestResponseObj)
        {
            using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
            using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesRequest, requestResponseObj);
                _logger.LogInformation("AlgoStrategies from broker message send to UI successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Send Replaced Order from Web TT.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendReplaceOrderFromTT(RequestResponseModel requestResponseObj)
        {
            using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
            using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendReplaceOrderRequest, requestResponseObj);
                _logger.LogInformation("SendReplaceOrderFromTT response message send to UI successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Book As Swap Replace Method
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task BookAsSwapReplace(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BookAsSwapRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Custom Allocation Details
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetCustomAllocationDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CustomAllocationDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Custom Allocation Details
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetSavedCustomAllocationDetails(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SavedCustomAllocationDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Custom Allocation Details Bulk for multiple preference ID's
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetSavedCustomAllocationDetailsBulk(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_FetchSavedCustomAllocationDetailsBulkRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task GetSMData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetSMDetailsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        ///<summary>
        /// Unsubrcibe the SymbolCompressionFeed
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UnsubscribeSymbolCompressionFeed(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_UnsubscribeSymbolCompressionFeedRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }


        /// <summary>        
        /// Get total data from Trading service       
        /// </summary>        
        /// <param name="requestResponseObj"></param>        
        /// <returns></returns>        
        /// <exception cref="Exception"></exception>
        public async Task GetBrokerConnectionAndVenuesData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service"); throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }



        /// <summary>
        /// SendStageOrderHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SendStageOrderHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SendStageOrderResponse, message);
                if (data.ContainsKey(ServicesMethodConstants.CONST_TRADING_TICKET_ID))
                {
                    tradingTicketId = data[ServicesMethodConstants.CONST_TRADING_TICKET_ID];
                    // Append only if it ends with "_HotButton"
                    if (tradingTicketId is string idStr && idStr.EndsWith(ServicesMethodConstants.CONST_HOTBUTTON))
                    {
                        topicToListen = topicToListen + "_" + tradingTicketId;
                    }
                }
                _hubManager.SendUserBasedMessage(topicToListen, message);

                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// SendManualOrderHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SendManualOrderHandler(string topic, RequestResponseModel message)
        {
            using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
            using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SendManualOrderResponse, message);
                if (data.ContainsKey(ServicesMethodConstants.CONST_TRADING_TICKET_ID))
                {
                    tradingTicketId = data[ServicesMethodConstants.CONST_TRADING_TICKET_ID];
                    // Append only if it ends with "_HotButton"
                    if (tradingTicketId is string idStr && idStr.EndsWith(ServicesMethodConstants.CONST_HOTBUTTON))
                    {
                        topicToListen = topicToListen + "_" + tradingTicketId;
                    }
                }
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// SendReplaceOrderHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SendReplaceOrderHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SendReplaceOrderResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// SendLiveOrderHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void SendLiveOrderHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SendLiveOrderResponse, message);
                if (data.ContainsKey(ServicesMethodConstants.CONST_TRADING_TICKET_ID))
                {
                    tradingTicketId = data[ServicesMethodConstants.CONST_TRADING_TICKET_ID];
                    // Append only if it ends with "_HotButton"
                    if (tradingTicketId is string idStr && idStr.EndsWith(ServicesMethodConstants.CONST_HOTBUTTON))
                    {
                        topicToListen = topicToListen + "_" + tradingTicketId;
                    }
                }
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// GetSMDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetSMDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_GetSMDetailsResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// CreatePopupTextHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void CreatePopupTextHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_CreatePopUpTextResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// CreateOptionSymbolHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void CreateOptionSymbolHandler(string topic, RequestResponseModel message)
        {
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_CreateOptionSymbolResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// GetCustomAllocationHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetCustomAllocationHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_CustomAllocationDetailsResponse;
                if (data.ContainsKey("ViewId") && message.CompanyUserID != 0)
                {
                    tradingTicketId = data["ViewId"];
                    topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + tradingTicketId;
                }
                Console.WriteLine("SignalR response sent on topic: " + topicToListen);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _hubManagerRTPNL.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// GetSavedCustomAllocationHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetSavedCustomAllocationHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse;
                if (data.ContainsKey("tradingTicketId") && message.CompanyUserID != 0)
                {
                    tradingTicketId = data["tradingTicketId"];
                    topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + tradingTicketId;
                }
                Console.WriteLine("SignalR response sent on topic: " + topicToListen);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// GetAlgoStrategiesFromBrokerHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetAlgoStrategiesFromBrokerHandler(string topic, RequestResponseModel message)
        {
            using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
            using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);
            try
            {
                string topicToListen = HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, message);
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// GetBrokerConnectionAndVenuesDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetBrokerConnectionAndVenuesDataHandler(string topic, RequestResponseModel message)
        {
            using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
            using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);
            try
            {
                Dictionary<dynamic, dynamic> data = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataResponse;
                if (data.ContainsKey("tradingTicketId") && message.CompanyUserID != 0)
                {
                    tradingTicketId = data["tradingTicketId"];
                    topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + tradingTicketId;
                }
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogInformation(MessageConstants.MSG_SIGNALR_RESPONSE_GENERATED + topicToListen);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }


        private void CreatePstAllocationPrefResponse(string topic, RequestResponseModel message)
        {
            using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
            using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);
            try
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(message?.Data);
                string topicToListen = KafkaConstants.TOPIC_CreatePstAllocationPrefResponse;
                topicToListen = topicToListen + "_" + message.CompanyUserID + "_" + data.TradingTicketId;
                _hubManager.SendUserBasedMessage(topicToListen, message);
                _logger.LogTrace("Received CreatedPstAllocation response for signalR");
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }

        }

        /// <summary>
        ///  For PST NAV and starting value response received from compliance service -> basket compliance
        /// </summary> >

        private void GetAccountNavNStartingValueFromBasketResponse(string topic, RequestResponseModel message)
        {
            using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message?.CorrelationId);
            using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message?.CompanyUserID);
            try
            {
                {
                    _hubManager.SendUserBasedMessage(ServicesMethodConstants.CONST_GET_ACCOUNT_NAV_RESP,
                        message.Data,
                        message.CompanyUserID,
                        message.CorrelationId);

                    var respTime = HelperFunctions.GetTimeDiffInSecs(message.CorrelationId);
                    _logger.LogInformation("Received NavAndStartingPositionOfAccountsResponse Payload from basket compliance for PST in {time} sec", respTime);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAccountNavNStartingValueFromBasketResponse ecnountered an error");
            }
        }

        public async Task GetPstAccountNav(RequestResponseModel message)
        {
            try
            {
                using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
                using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);

                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasket, message);
                _logger.LogInformation("Request of GetPstAccountNav has been send in kafka topic.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPstAccountNav");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task SendPstOrders(RequestResponseModel message)
        {
            try
            {
                using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
                using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);

                await _kafkaManager.Produce(KafkaConstants.TOPIC_PSTOrderRequest, message);
                _logger.LogInformation("Request of GetPstAccountNav has been send in kafka topic.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPstAccountNav");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task SendOrdersToMarket(RequestResponseModel message)
        {
            try
            {
                using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
                using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);

                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendOrdersToMarketRequest, message);
                _logger.LogInformation("Request of SendOrdersToMarket has been send in kafka topic.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendOrdersToMarket");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task CreatePstAllocatonPreference(RequestResponseModel message)
        {
            try
            {
                using var prop1 = LogContext.PushProperty(LogConstant.CORRELATION_ID, message.CorrelationId);
                using var prop2 = LogContext.PushProperty(LogConstant.USER_ID, message.CompanyUserID);

                await _kafkaManager.Produce(KafkaConstants.TOPIC_CreatePstAllocatonPreferenceRequest, message);
                _logger.LogInformation("Request of GetPstAccountNav has been send in kafka topic.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreatePstAllocatonPreference");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Produces a Kafka event to request trade attribute labels.
        /// </summary>
        public async Task ProduceTradeAttributeLabelsEvent(RequestResponseModel requestResponseObj)
        {
            try
            {
                using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
                using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetTradeAttributeLabels, requestResponseObj);
                _logger.LogInformation("Trade Attribute labels event is produce with CoorelationId {0}", requestResponseObj?.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProduceTradeAttributeLabelsEvent");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Produces a Kafka event to request trade attribute values for a given user.
        /// </summary>
        public async Task ProduceTradeAttributeValuesEvent(RequestResponseModel requestResponseObj)
        {
            try
            {
                using var p1 = (LogContext.PushProperty(Constants.LogConstant.CORRELATION_ID, requestResponseObj?.CorrelationId));
                using var p2 = (LogContext.PushProperty(Constants.LogConstant.USER_ID, requestResponseObj?.CompanyUserID));
                await _kafkaManager.Produce(KafkaConstants.TOPIC_GetTradeAttributeValues, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProduceTradeAttributeValuesEvent");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Handles the response for trade attribute labels received from Kafka and forwards it to the user via SignalR.
        /// </summary>
        private void KafkaManager_TradeAttributeLabelsResponse(string topic, RequestResponseModel message)
        {
            try
            {
                var jObject = JObject.Parse(message.Data);
                var requestId = jObject["Data"]?["RequestId"]?.ToString();
                string newtopic = KafkaConstants.TOPIC_TradeAttributeLabelsResponse + '_' + requestId + '_' + message.CompanyUserID;
                _hubManager.SendUserBasedMessage(newtopic, message.Data, message.CompanyUserID, message.CorrelationId);
                _logger.LogInformation("Received TradeAttributeDetails response for signalR with CorrelationId: {0}", message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }

        /// <summary>
        /// Handles the response for trade attribute values received from Kafka and forwards it to the user via SignalR.
        /// </summary>
        private void KafkaManager_TradeAttributeValuesResponse(string topic, RequestResponseModel message)
        {
            try
            {
                var jObject = JObject.Parse(message.Data);
                var requestId = jObject["Data"]?["RequestId"]?.ToString();
                string newtopic = KafkaConstants.TOPIC_TradeAttributeValuesResponse + '_' + requestId + '_' + message.CompanyUserID;
                _hubManager.SendUserBasedMessage(newtopic, message.Data, message.CompanyUserID, message.CorrelationId);
                _logger.LogInformation("Received TradeAttributeDetails response for signalR");
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogError);
            }
        }
    }
}

