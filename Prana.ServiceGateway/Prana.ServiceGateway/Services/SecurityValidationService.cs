using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using System.Text.Json;
using static Prana.ServiceGateway.Constants.GlobalConstants;

namespace Prana.ServiceGateway.Services
{
    public class SecurityValidationService : ISecurityValidationService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly IHubManagerRTPNL _hubManagerRTPNL;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SecurityValidationService> _logger;
        private int _validationTimeout;
        private Dictionary<string, Timer> _securityValidationTimeouts;

        public SecurityValidationService(IKafkaManager kafkaManager,
            IHubManager hubManager,
            IConfiguration configuration,
            ILogger<SecurityValidationService> logger,
            IHubManagerRTPNL hubManagerRTPNL)
        {
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
            _configuration = configuration;
            this._logger = logger;
            _hubManagerRTPNL = hubManagerRTPNL;
        }

        public void Initialize()
        {
            try
            {
                _securityValidationTimeouts = new Dictionary<string, Timer>();
                _validationTimeout = Convert.ToInt32(_configuration[ServicesMethodConstants.CONST_TT_VALIDATION_TIMEOUT]);

                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SecurityValidationResponse, SecurityValidationResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SecuritySearchResponse, SecuritySearchResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SMSecuritySearchResponse, SMSecuritySearchResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolUpdateResponse, SMSymbolUpdateResponseHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, SMSymbolSaveClickedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Initialize of SecurityValidationService");
            }
        }

        private void SMSymbolSaveClickedResponse(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SMSymbolSaveClickedResponse");
            }
        }

        private void SMSymbolUpdateResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_SMSymbolUpdateResponse, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SMSymbolUpdateResponseHandler");
            }
        }

        public async Task SymbolSearch(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SecuritySearchRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void SMSaveNewSymbol(RequestResponseModel requestResponseObj)
        {
            try
            {
                _kafkaManager.Produce(KafkaConstants.TOPIC_SMSaveNewSymbolRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task SMSymbolSearch(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SMSecuritySearchRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        private void SendTimeoutResponse(object state)
        {
            try
            {
                if (_securityValidationTimeouts.ContainsKey(state.ToString()))
                {
                    _hubManager.BroadcastMessage(state.ToString(), ServicesMethodConstants.CONST_SEC_VALIDATION_FAILED_RESPONSE, 0);

                    _securityValidationTimeouts[state.ToString()].Dispose();
                    _securityValidationTimeouts.Remove(state.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
            }
        }

        private void SMSecuritySearchResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_SMSecuritySearchResponse, message.Data, message.CompanyUserID, message.CorrelationId);
                _hubManagerRTPNL.SendUserBasedMessage(KafkaConstants.TOPIC_SMSecuritySearchResponse, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SMSecuritySearchResponseHandler");
            }
        }

        private void SecurityValidationResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                JsonDocument obj = JsonDocument.Parse(message.Data);
                string symbol = obj.RootElement.GetProperty(ServicesMethodConstants.CONST_TICKER_SYMBOL).ToString();
                string RequestID = obj.RootElement.GetProperty(ServicesMethodConstants.CONST_REQUESTED_HASHCODE).ToString();

                if (_securityValidationTimeouts.ContainsKey(symbol))
                {
                    _securityValidationTimeouts[symbol].Dispose();
                    _securityValidationTimeouts.Remove(symbol);
                }

                string topicToListen = string.IsNullOrEmpty(RequestID) ? ServicesMethodConstants.CONST_SEC_VALIDATION_RESPONSE : ServicesMethodConstants.CONST_SEC_VALIDATION_RESPONSE + "_" + RequestID;
                _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
            }
        }
        private void SecuritySearchResponseHandler(string topic, RequestResponseModel message)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(message.Data);
                string topicToListen = ServicesMethodConstants.CONST_SEC_SEARCH_RESPONSE + "_" + data.HashCode;
                _hubManager.SendUserBasedMessage(topicToListen, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SecuritySearchResponseHandler");
            }
        }

        /// <summary>
        /// ValidateSymbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="companyUserID"></param>
        /// <param name="correlationId"></param>
        /// <param name="requestId"></param>
        /// <param name="isOptionSymbol"></param>
        /// <param name="underLyingSymbol"></param>
        /// <param name="symbology"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ValidateSymbolUnifiedAsync(List<string> symbols, int companyUserID, string correlationId, string requestId = "", bool isOptionSymbol = false, string underLyingSymbol = "", int symbology = 0)
        {
            try
            {
                bool hasMarketDataPermission = PermissionManager.GetInstance().FetchMarketDataPermission(companyUserID);
                var requestResponseObj = new RequestResponseModel();

                if (isOptionSymbol)
                {
                    // Option symbol request 
                    var data = new
                    {
                        OptionSymbol = symbols?.FirstOrDefault() ?? string.Empty,
                        UnderLyingSymbol = underLyingSymbol,
                        RequestID = requestId,
                        hasMarketDataPermission
                    };

                    requestResponseObj.CompanyUserID = companyUserID;
                    requestResponseObj.Data = JsonConvert.SerializeObject(data);
                    requestResponseObj.CorrelationId = correlationId;
                }
                else
                {
                    // Standard multiple-symbol validation
                    string serializedSymbols = JsonConvert.SerializeObject(symbols);
                    var data = new
                    {
                        serializedSymbols,
                        hasMarketDataPermission,
                        requestId
                    };
                    var dataJson = JsonConvert.SerializeObject(data);
                    //For Factset Symbology
                    if ((SymbologyCodes)symbology == SymbologyCodes.FactSetSymbol)
                    {
                        var dataWithSymbology = new
                        {
                            serializedSymbols,
                            hasMarketDataPermission,
                            requestId,
                            symbology
                        };
                        dataJson = JsonConvert.SerializeObject(dataWithSymbology);
                    }

                    requestResponseObj.CompanyUserID = companyUserID;
                    requestResponseObj.Data = dataJson;
                    requestResponseObj.CorrelationId = correlationId;

                    // Timeout logic for non-option symbols
                    foreach (string symbol in symbols)
                    {
                        if (_securityValidationTimeouts != null && !_securityValidationTimeouts.ContainsKey(symbol))
                        {
                            Timer timer = new Timer(SendTimeoutResponse, symbol, _validationTimeout * 1000, Timeout.Infinite);
                            _securityValidationTimeouts.Add(symbol, timer);
                        }
                    }
                }
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service - ValidateSymbolUnifiedAsync");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

    }
}
