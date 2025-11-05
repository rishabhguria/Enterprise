using Microsoft.AspNetCore.SignalR;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Hubs;

namespace Prana.ServiceGateway.Services
{
    public class ComplianceService : IComplianceService
    {
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly ILogger<ComplianceService> _logger;

        public ComplianceService(IKafkaManager kafkaManager, IHubManager hubManager, ILogger<ComplianceService> logger)
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
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsData, ComplianceAlertsDataHandler);
                _kafkaManager.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsDataSync, ComplianceAlertsDataHandlerSync);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Initialize of ComplianceService");
            }
        }


        /// <summary>
        /// ComplianceAlertsDataHandler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void ComplianceAlertsDataHandler(string topic, RequestResponseModel message)
        {
            try
            {
                _logger.LogDebug("Method ComplianceAlertsDataHandler started with topic: {topic}", topic);

                Console.WriteLine(MessageConstants.CONST_BRACKET_OPEN + DateTime.Now.ToString(MessageConstants.CONST_DATET_TIME_FORMAT) + MessageConstants.CONST_BRACKET_CLOSE + MessageConstants.CONST_METHOD_EXECUTION_STARTED + ControllerMethodConstants.CONST_METHOD_RECEIVED_COMPLIANCE_DATA);
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_ComplianceAlertsData, message.Data, message.CompanyUserID, message.CorrelationId);

                _logger.LogDebug("Method ComplianceAlertsDataHandler complete for topic: {topic}", topic);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
            }
        }


        /// <summary>
        /// ComplianceAlertsDataHandlerSync
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void ComplianceAlertsDataHandlerSync(string topic, RequestResponseModel message)
        {
            try
            {
                _logger.LogDebug("Method ComplianceAlertsDataHandlerSync started with topic: {topic}", topic);
                _hubManager.SendUserBasedMessage(KafkaConstants.TOPIC_ComplianceAlertsDataSync, message.Data, message.CompanyUserID, message.CorrelationId);
                _logger.LogDebug("Method ComplianceAlertsDataHandler complete for topic: {topic}", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
            }
        }

        /// <summary>
        /// SendComplianceData
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendComplianceData(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendComplianceDataResponse, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        /// <summary>
        /// SendComplianceDataForStage
        /// </summary>
        /// <param name="requestResponseObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SendComplianceDataForStage(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_SendComplianceDataResponseForStage, requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }


        public async Task CheckComplianceFrBasket(RequestResponseModel requestResponseObj)
        {
            try
            {
                await _kafkaManager.Produce(KafkaConstants.TOPIC_ProcessDataForCheckComplianceFromBasket, 
                    requestResponseObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckComplianceFrBasket");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
    }
}
