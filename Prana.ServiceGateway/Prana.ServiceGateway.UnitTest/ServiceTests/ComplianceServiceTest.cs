using Microsoft.Extensions.Logging;
using Moq;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class ComplianceServiceTest : BaseControllerTest
    {
        private readonly IComplianceService _complianceService;
        private readonly Mock<ILogger<ComplianceService>> _mockLogger;

        public ComplianceServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<ComplianceService>();
            _complianceService = new ComplianceService(_mockKafkaManager.Object,
                _mockHubServiceGateway.Object,
                _mockLogger.Object);
        }

        #region Initialize Test Cases
        [Fact]
        public void Initialize_LogsError_WhenExceptionIsThrown()
        {
            try
            {
                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(It.IsAny<string>(), It.IsAny<Action<string, RequestResponseModel>>(), It.IsAny<bool>()))
                    .Throws(new Exception("Simulated exception"));

                _complianceService.Initialize();

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Initialize of ComplianceService")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendComplianceData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendComplianceData_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SendComplianceDataResponse, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _complianceService.SendComplianceData(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SendComplianceDataResponse, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendComplianceData_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SendComplianceDataResponse, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _complianceService.SendComplianceData(requestResponseObj));

                // Assert
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
                Assert.Contains(kafkaException.Message, exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendComplianceDataForStage Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendComplianceDataForStage_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SendComplianceDataResponseForStage, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _complianceService.SendComplianceDataForStage(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SendComplianceDataResponseForStage, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendComplianceDataForStage_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SendComplianceDataResponseForStage, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _complianceService.SendComplianceDataForStage(requestResponseObj));

                // Assert
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
                Assert.Contains(kafkaException.Message, exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CheckComplianceFrBasket Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CheckComplianceFrBasket_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_ProcessDataForCheckComplianceFromBasket, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _complianceService.CheckComplianceFrBasket(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_ProcessDataForCheckComplianceFromBasket, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task CheckComplianceFrBasket_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_ProcessDataForCheckComplianceFromBasket, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _complianceService.CheckComplianceFrBasket(requestResponseObj));

                // Assert
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
                Assert.Contains(kafkaException.Message, exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ComplianceAlertsDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void ComplianceAlertsDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _complianceService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void ComplianceAlertsDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _complianceService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Service")),
                              It.IsAny<Exception>(),
                              It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                          Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ComplianceAlertsDataHandlerSync Test Cases
        [Theory]
        [InlineData(1)]
        public void ComplianceAlertsDataHandlerSync_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsDataSync, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _complianceService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsDataSync, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void ComplianceAlertsDataHandlerSync_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsDataSync, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _complianceService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ComplianceAlertsDataSync, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Service")),
                              It.IsAny<Exception>(),
                              It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                          Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion
    }
}
