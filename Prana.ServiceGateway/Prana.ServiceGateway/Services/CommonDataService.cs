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
    public class CommonDataService : ICommonDataService, IDisposable
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly ILogger<CommonDataService> _logger;

        public CommonDataService(IKafkaManager kafkaManager,
            IHubManager hubManager,
            ILogger<CommonDataService> logger)
        {
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
            this._logger = logger;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Initialize()
        {
            try
            {
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTransferTradeRulesResponse, GetCompanyTransferTradeRulesHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingTicketResponse, GetTradingTicketAllDataHandler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Initialize of commonDataService");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets user order type details.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetUserOrderType(RequestResponseModel requestResponseObj)
        {
            try
            {
                Task<string> task = _kafkaManager.ProduceAndConsume(requestResponseObj, 
                    KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest, 
                    KafkaConstants.TOPIC_UserWiseOrderTypeDataResponse);

                return await task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserOrderType");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get company transfer trade rules.
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task GetCompanyTransferTradeRules(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCompanyTransferTradeRules");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task GetTradingTicketAllData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_CompanyTradingTicketRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTradingTicketAllData");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        /// <summary>
        /// GetCompanyTransferTradeRulesHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetCompanyTransferTradeRulesHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_CompanyTransferTradeRules, message.Data, message.CompanyUserID, message.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCompanyTransferTradeRulesHandler");
            }
        }

        /// <summary>
        /// GetTradingTicketAllDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void GetTradingTicketAllDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);
                string tradingTicketId = string.Empty;
                string topicToListen = KafkaConstants.TOPIC_CompanyTradingTicketResponse;
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
                _logger.LogError(ex, "Error in GetTradingTicketAllDataHandler");
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
