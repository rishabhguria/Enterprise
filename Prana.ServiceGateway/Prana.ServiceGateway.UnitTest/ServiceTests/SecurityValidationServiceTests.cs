using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class SecurityValidationServiceTests : BaseControllerTest
    {
        private readonly SecurityValidationService _securityValidationService;
        private readonly Mock<ILogger<SecurityValidationService>> _mockLogger;

        public SecurityValidationServiceTests() : base()
        {
            _mockLogger = CreateMockLogger<SecurityValidationService>();
            _securityValidationService = new SecurityValidationService(_mockKafkaManager.Object, 
                _mockHubServiceGateway.Object,
                _mockConfiguration.Object,
                _mockLogger.Object,
                _mockHubServiceGatewayRTPNL.Object);
        }

        #region SymbolSearch Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SymbolSearch_WhenCalled_ProducesMessageToKafkaTopic(int companyUserID)
        {
            try
            {
                // Arrange
                var expectedMessage = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_MOCK_SYMBOL, CorrelationId);
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_MOCK_SYMBOL, CorrelationId);
                // Act
                await _securityValidationService.SymbolSearch(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(x => x.Produce(KafkaConstants.TOPIC_SecuritySearchRequest, It.Is<RequestResponseModel>(m => m.Data == expectedMessage.Data), true), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SymbolSearch_ThrowsExceptionIfKafkaManagerProduceThrowsException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();
                var requestResponseObj = new RequestResponseModel(0, UnitTestConstants.CONST_MOCK_SYMBOL, CorrelationId);
                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _securityValidationService.SymbolSearch(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SecuritySearchResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void Initialize_SubscribesToSecuritySearchResponse(int companyUserID)
        {
            try
            {
                var message = new RequestResponseModel(companyUserID, "{\"HashCode\":\"12345\"}", CorrelationId);
                Action<string, RequestResponseModel> actualHandler = null;
                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _securityValidationService.Initialize();

                // Invoke SecuritySearchResponseHandler indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SecuritySearchResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"HashCode\":\"12345\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _securityValidationService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SecuritySearchResponseHandler")),
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

        #region SecurityValidationResponse Test Cases
        [Theory]
        [InlineData(1, "{\"TickerSymbol\":\"MSFT\"}")]
        [InlineData(2, "{\"TickerSymbol\":\"A\"}")]
        public void Initialize_SubscribesToSecurityValidationResponse(int companyUserID,string json)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SecurityValidationResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _securityValidationService.Initialize();

                // Invoke SecurityValidationResponse indirectly by calling actualHandler
                actualHandler(KafkaConstants.TOPIC_SecurityValidationResponse, new RequestResponseModel(companyUserID, json,CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SecurityValidationResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region Initialize Test Cases
        [Fact]
        public void Initialize_LogsError_WhenExceptionIsThrown()
        {
            try
            {
                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(It.IsAny<string>(), It.IsAny<Action<string, RequestResponseModel>>(), It.IsAny<bool>()))
                    .Throws(new Exception("Simulated exception"));

                _securityValidationService.Initialize();

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Initialize of SecurityValidationService")),
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

        #region SMSymbolSaveClickedResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void SMSymbolSaveClickedResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _securityValidationService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SMSymbolSaveClickedResponse_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _securityValidationService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SMSymbolSaveClickedResponse")),
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

        #region SMSymbolUpdateResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SMSymbolUpdateResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolUpdateResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _securityValidationService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolUpdateResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SMSymbolUpdateResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolUpdateResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _securityValidationService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSymbolUpdateResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SMSymbolUpdateResponseHandler")),
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

        #region SMSecuritySearchResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SMSecuritySearchResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _securityValidationService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SMSecuritySearchResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _securityValidationService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SMSecuritySearchResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SMSecuritySearchResponseHandler")),
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

        #region SMSaveNewSymbol Test Cases

        [Theory]
        [InlineData(1)]
        public void SMSaveNewSymbol_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SMSaveNewSymbolRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _securityValidationService.SMSaveNewSymbol(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SMSaveNewSymbolRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SMSaveNewSymbol_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SMSaveNewSymbolRequest, requestResponseObj, true))
                    .Throws(kafkaException);

                // Act
                var exception = Assert.Throws<Exception>(() =>
                    _securityValidationService.SMSaveNewSymbol(requestResponseObj));

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

        #region SMSymbolSearch Test Cases

        [Theory]
        [InlineData(1)]
        public async Task SMSymbolSearch_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SMSecuritySearchRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _securityValidationService.SMSymbolSearch(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SMSecuritySearchRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SMSymbolSearch_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SMSecuritySearchRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _securityValidationService.SMSymbolSearch(requestResponseObj));

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

        #region ValidateSymbols Test Cases
        [Fact]
        public async Task ValidateSymbols_CallsProduceAndCreatesTimers()
        {
            try
            {
                // Arrange
                var symbols = new List<string> { "AAPL", "GOOG" };
                var companyUserID = 1;
                var correlationId = "TestCorrelationId";
                var requestId = "TestRequestId";
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockConfiguration.Setup(x => x[ServicesMethodConstants.CONST_TT_VALIDATION_TIMEOUT]).Returns("10");
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _securityValidationService.ValidateSymbolUnifiedAsync(symbols, companyUserID, correlationId, requestId);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateSymbols_LogsErrorAndThrowsException_WhenExceptionOccurs()
        {
            try
            {
                // Arrange
                _mockConfiguration.Setup(x => x[ServicesMethodConstants.CONST_TT_VALIDATION_TIMEOUT]).Returns("10");

                var symbols = new List<string> { "AAPL", "GOOG" };
                var companyUserID = 1;
                var correlationId = "TestCorrelationId";
                var requestId = "TestRequestId";

                _mockKafkaManager
                    .Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<RequestResponseModel>(), true))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                    _securityValidationService.ValidateSymbolUnifiedAsync(symbols, companyUserID, correlationId, requestId));

                Assert.Contains("Simulated exception", exception.Message);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Service - ValidateSymbol")),
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

        #region ValidateOptionSymbol Test Cases
        [Fact]
        public async Task ValidateOptionSymbol_CallsProduce()
        {
            // Arrange  
            var symbols = new List<string> { "AAPL", "GOOG" };
            var companyUserID = 1;
            var correlationId = "TestCorrelationId";
            var requestId = "TestRequestId";
            var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
            _mockConfiguration.Setup(x => x[ServicesMethodConstants.CONST_TT_VALIDATION_TIMEOUT]).Returns("10");
            _mockKafkaManager
                .Setup(x => x.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, It.IsAny<RequestResponseModel>(), true))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act  
            await _securityValidationService.ValidateSymbolUnifiedAsync(symbols, companyUserID, correlationId, requestId);

            // Assert  
            _mockKafkaManager.Verify(
                x => x.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, It.IsAny<RequestResponseModel>(), true),
                Times.Once);
        }

        [Fact]
        public async Task ValidateOptionSymbol_LogsErrorAndThrowsException()
        {
            // Arrange  
            var symbols = new List<string> { "AAPL", "GOOG" };
            var companyUserID = 1;
            var correlationId = "TestCorrelationId";
            var requestId = "TestRequestId";
            var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, correlationId);

            _mockKafkaManager
                .Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<RequestResponseModel>(), true))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert  
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _securityValidationService.ValidateSymbolUnifiedAsync(symbols, companyUserID, correlationId, requestId));

            Assert.Contains("Simulated exception", exception.Message);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Service")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }
        #endregion

        #region ValidateSymbols Test Cases
        [Fact]
        public async Task ValidateSymbolUnifiedAsync_CallsProduceAndCreatesTimers()
        {
            try
            {
                // Arrange
                var symbols = new List<string> { "AAPL", "GOOG" };
                var companyUserID = 1;
                var correlationId = "TestCorrelationId";
                var requestId = "TestRequestId";
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockConfiguration.Setup(x => x[ServicesMethodConstants.CONST_TT_VALIDATION_TIMEOUT]).Returns("10");
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _securityValidationService.ValidateSymbolUnifiedAsync(symbols, companyUserID, correlationId, requestId);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SecurityValidationRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateSymbolUnifiedAsync_LogsErrorAndThrowsException_WhenExceptionOccurs()
        {
            try
            {
                // Arrange
                _mockConfiguration.Setup(x => x[ServicesMethodConstants.CONST_TT_VALIDATION_TIMEOUT]).Returns("10");

                var symbols = new List<string> { "AAPL", "GOOG" };
                var companyUserID = 1;
                var correlationId = "TestCorrelationId";
                var requestId = "TestRequestId";

                _mockKafkaManager
                    .Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<RequestResponseModel>(), true))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                    _securityValidationService.ValidateSymbolUnifiedAsync(symbols, companyUserID, correlationId, requestId));

                Assert.Contains("Simulated exception", exception.Message);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Service - ValidateSymbol")),
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
    }

}
