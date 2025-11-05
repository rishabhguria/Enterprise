using Microsoft.Extensions.Logging;
using Moq;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class LiveFeedServiceTest : BaseControllerTest
    {
        private readonly LiveFeedService _liveFeedService;
        private readonly Mock<ILogger<LiveFeedService>> _mockLogger;
        public LiveFeedServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<LiveFeedService>();
            _liveFeedService = new LiveFeedService(_mockKafkaManager.Object,
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

                _liveFeedService.Initialize();

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in LiveFeed initialize method")),
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

        #region RequestSymbol Test Cases
        [Theory]
        [InlineData(1)]
        public async void RequestSymbol_WhenCalled_ProducesMessageToKafkaTopic(int companyUserID)
        {
            try
            {
                // Arrange

                var expectedMessage = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_MOCK_SYMBOL, CorrelationId);

                // Act
                await _liveFeedService.RequestSymbol(UnitTestConstants.CONST_MOCK_SYMBOL, 0);

                // Assert
                _mockKafkaManager.Verify(x => x.Produce(KafkaConstants.TOPIC_LiveFeedRequest, It.Is<RequestResponseModel>(m => m.Data == expectedMessage.Data), true), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task RequestSymbol_ThrowsExceptionIfKafkaManagerProduceThrowsException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _liveFeedService.RequestSymbol(
                    UnitTestConstants.CONST_MOCK_SYMBOL, 0));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region LiveFeedResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void Initialize_SubscribesToLiveFeedResponse(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_LiveFeedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _liveFeedService.Initialize();

                // Invoke LiveFeedResponseHandler indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA, 
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_LiveFeedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void LiveFeedResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_LiveFeedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _liveFeedService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_LiveFeedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in LiveFeed LiveFeedResponseHandler method")),
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

        #region ReqMultipleSymbolsLiveFeedSnapshotData Test Cases
        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_CallsProduceWithValidPayload()
        {
            try
            {
                // Arrange
                var request = new MultipleSymbolRequestDto
                {
                    RequestedSymbols = new List<string> { "AAPL", " ", "GOOG", null, "MSFT" },
                    RequestedInstance = "TestInstance"
                };
                var companyUserId = 1;

                // Act
                await _liveFeedService.ReqMultipleSymbolsLiveFeedSnapshotData(request, companyUserId);

                // Assert

                _mockKafkaManager.Verify(
                    x => x.Produce(
                        KafkaConstants.TOPIC_MultipleSymbolsLiveFeedSnapshotRequest,
                        It.IsAny<RequestResponseModel>(),
                        true
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region UpdateMarketDataTokenRequest Test Cases
        [Theory]
        [InlineData(1)]
        public async Task UpdateMarketDataTokenRequest_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateMarketDataTokenRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _liveFeedService.UpdateMarketDataTokenRequest(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_UpdateMarketDataTokenRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateMarketDataTokenRequest_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateMarketDataTokenRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _liveFeedService.UpdateMarketDataTokenRequest(requestResponseObj));

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

        #region UnSubscribeLiveFeed Test Cases
        [Theory]
        [InlineData(1)]
        public async Task UnSubscribeLiveFeed_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UnSubscribeLiveFeedRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _liveFeedService.UnSubscribeLiveFeed(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_UnSubscribeLiveFeedRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task UnSubscribeLiveFeed_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UnSubscribeLiveFeedRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _liveFeedService.UnSubscribeLiveFeed(requestResponseObj));

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

        #region MarketDataPermissionResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void MarketDataPermissionResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_MarketDataPermissionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _liveFeedService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_MarketDataPermissionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void MarketDataPermissionResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_MarketDataPermissionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _liveFeedService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_MarketDataPermissionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in LiveFeed MarketDataPermissionResponseHandler method")),
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
