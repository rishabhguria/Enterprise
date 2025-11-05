using System;
using System.Collections.Generic;
using Moq;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using System.Threading.Tasks;
using Xunit;
using Prana.ServiceGateway.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Microsoft.Extensions.Logging;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class RtpnlServiceTest : BaseControllerTest
    {
        private readonly IRtpnlService _rtpnlService;
        private readonly Mock<ILogger<RtpnlService>> _mockLogger;
        public RtpnlServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<RtpnlService>();
            _rtpnlService = new RtpnlService(_mockKafkaManager.Object,
                _mockHubServiceGatewayRTPNL.Object,
                _mockLogger.Object, _mockHubServiceGateway.Object, _mockHubServiceGatewayRTPNLUpdates.Object);
        }

        #region SaveUpdateConfigDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveUpdateConfigDetails_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SaveUpdateConfigDetailsRequest, KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse);

                _rtpnlService.Initialize();
                // Act
                await _rtpnlService.SaveUpdateConfigDetails(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveUpdateConfigDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SaveUpdateConfigDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _rtpnlService.SaveUpdateConfigDetails(requestResponseObj));

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

        #region GetRtpnlWidgetData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlWidgetData_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_RtpnlWidgetDataRequest, KafkaConstants.TOPIC_RtpnlWidgetDataResponse);

                _rtpnlService.Initialize();
                // Act
                await _rtpnlService.GetRtpnlWidgetData(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlWidgetData_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_RtpnlWidgetDataResponse, KafkaConstants.TOPIC_RtpnlWidgetDataRequest);

                _rtpnlService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _rtpnlService.GetRtpnlWidgetData(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveConfigDataForExtract Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveConfigDataForExtract_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SaveConfigDataForExtractRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _rtpnlService.SaveConfigDataForExtract(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SaveConfigDataForExtractRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveConfigDataForExtract_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SaveConfigDataForExtractRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _rtpnlService.SaveConfigDataForExtract(requestResponseObj));

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

        #region GetRtpnlWidgetConfigData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlWidgetConfigData_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_RtpnlWidgetConfigDataRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _rtpnlService.GetRtpnlWidgetConfigData(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_RtpnlWidgetConfigDataRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlWidgetConfigData_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_RtpnlWidgetConfigDataRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _rtpnlService.GetRtpnlWidgetConfigData(requestResponseObj));

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

        #region CheckCalculationServiceRunning Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CheckCalculationServiceRunning_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CheckCalculationServicIsRunningRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _rtpnlService.CheckCalculationServiceRunning(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_CheckCalculationServicIsRunningRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task CheckCalculationServiceRunning_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CheckCalculationServicIsRunningRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _rtpnlService.CheckCalculationServiceRunning(requestResponseObj));

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

        #region DeleteRemovedWidgetConfigDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task DeleteRemovedWidgetConfigDetails_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_DeleteRemovedWidgetConfigDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _rtpnlService.DeleteRemovedWidgetConfigDetails(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_DeleteRemovedWidgetConfigDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteRemovedWidgetConfigDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_DeleteRemovedWidgetConfigDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _rtpnlService.DeleteRemovedWidgetConfigDetails(requestResponseObj));

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

        #region GetRtpnlTradeAttributeLabels Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlTradeAttributeLabels_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _rtpnlService.GetRtpnlTradeAttributeLabels(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlTradeAttributeLabels_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _rtpnlService.GetRtpnlTradeAttributeLabels(requestResponseObj));

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

        #region SaveConfigDataForExtractHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SaveConfigDataForExtractHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SaveConfigDataForExtractHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SaveConfigDataForExtractHandler")),
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

        #region RtpnlTradeAttributeLabelsResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void RtpnlTradeAttributeLabelsResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RtpnlTradeAttributeLabelsResponse_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RtpnlTradeAttributeLabelsResponse")),
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

        #region SaveUpdateConfigDetailsHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SaveUpdateConfigDetailsHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SaveUpdateConfigDetailsHandler_HandlesException(int companyUserID)
        {
            try
            {
                // Arrange
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SaveUpdateConfigDetailsHandler")),
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

        #region GetRtpnlWidgetDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetRtpnlWidgetDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"StartUpGridData\": \"{\\\"GridItem1\\\": \\\"Value1\\\", \\\"GridItem2\\\": \\\"Value2\\\", \\\"AnotherGridKey\\\": \\\"AnotherGridValue\\\"}\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetRtpnlWidgetDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"StartUpGridData\": \"{\\\"GridItem1\\\": \\\"Value1\\\", \\\"GridItem2\\\": \\\"Value2\\\", \\\"AnotherGridKey\\\": \\\"AnotherGridValue\\\"}\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetRtpnlWidgetDataHandler")),
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

        #region GetRtpnlWidgetConfigDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetRtpnlWidgetConfigDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"ViewId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetRtpnlWidgetConfigDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"ViewId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetRtpnlWidgetConfigDataHandler")),
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

        #region CheckCalculationServiceIsRunningHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void CheckCalculationServiceIsRunningHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CheckCalculationServiceIsRunningHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in CheckCalculationServiceIsRunningHandler")),
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

        #region RtpnlRowCalculationRemovedHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RtpnlRowCalculationRemovedHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RtpnlRowCalculationRemovedHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationRemoved, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RtpnlRowCalculationRemovedHandler")),
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

        #region RtpnlRowCalculationUpdatesHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RtpnlRowCalculationUpdatesHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _rtpnlService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RtpnlRowCalculationUpdatesHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGatewayRTPNL
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _rtpnlService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RtpnlRowCalculationUpdatesHandler")),
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
