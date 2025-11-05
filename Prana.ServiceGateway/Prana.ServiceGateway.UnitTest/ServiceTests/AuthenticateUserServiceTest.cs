using Microsoft.Extensions.Logging;
using Moq;
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
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class AuthenticateUserServiceTest : BaseControllerTest
    {
        private readonly IAuthenticateUserService _authenticateUserService;
        private readonly Mock<ILogger<AuthenticateUserService>> _mockLogger;

        public AuthenticateUserServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<AuthenticateUserService>();
            _authenticateUserService = new AuthenticateUserService(_mockKafkaManager.Object, 
                _mockHubServiceGateway.Object,
                _mockLogger.Object);
        }

        #region LoginUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task LoginUser_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, 
                    KafkaConstants.TOPIC_AuthServiceRequest, 
                    KafkaConstants.TOPIC_AuthServiceResponse);

                _authenticateUserService.Initialize();
                // Act
                var result = await _authenticateUserService.LoginUser(requestResponseObj);

                // Assert
                Assert.Equal(UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task LoginUser_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowTestException(requestResponseObj, KafkaConstants.TOPIC_AuthServiceResponse, KafkaConstants.TOPIC_AuthServiceRequest);

                _authenticateUserService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _authenticateUserService.LoginUser(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region LogoutUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task LogoutUser_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_LogoutRequest, KafkaConstants.TOPIC_LogoutResponse);
                _authenticateUserService.Initialize();
                // Act
                await _authenticateUserService.LogoutUser(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task LogoutUser_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowTestException(requestResponseObj, KafkaConstants.TOPIC_LogoutResponse, KafkaConstants.TOPIC_LogoutRequest);
                _authenticateUserService.Initialize();
                await Assert.ThrowsAsync<Exception>(() => _authenticateUserService.LogoutUser(requestResponseObj));
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

                _authenticateUserService.Initialize();

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Initialize of AuthenticateUserService")),
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

        #region GetStatusForLogin Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetStatusForLogin_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE,
                    KafkaConstants.TOPIC_ServiceStatusRequest,
                    KafkaConstants.TOPIC_ServiceStatusResponse);

                _authenticateUserService.Initialize();
                // Act
                var result = await _authenticateUserService.GetStatusForLogin(requestResponseObj);

                // Assert
                Assert.Equal(UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetStatusForLogin_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowTestException(requestResponseObj, KafkaConstants.TOPIC_ServiceStatusResponse, KafkaConstants.TOPIC_ServiceStatusRequest);

                _authenticateUserService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _authenticateUserService.GetStatusForLogin(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region UpdateCacheForLoginUser Test Cases
        [Theory]
        [InlineData(1)]
        public void UpdateCacheForLoginUser_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateCacheRequestForLoginUser, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _authenticateUserService.UpdateCacheForLoginUser(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_UpdateCacheRequestForLoginUser, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void UpdateCacheForLoginUser_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                SetKafkaMock_ProduceThrowException();

                 // Act
                 var exception = Assert.Throws<Exception>(() =>
                    _authenticateUserService.UpdateCacheForLoginUser(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region Dispose Test Cases
        [Fact]
        public void Dispose_CallsDisposeMethod()
        {
            try
            {
                // Arrange
                var authenticateUserService = new AuthenticateUserService(
                    _mockKafkaManager.Object,
                    _mockHubServiceGateway.Object,
                    _mockLogger.Object
                );

                // Act
                authenticateUserService.Dispose();

                // Assert
                // No exception should be thrown
                Assert.True(true);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ForcefulLogoutWeb Test Cases
        [Theory]
        [InlineData(1)]
        public void ForcefulLogoutWeb_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ForcefulLogoutWeb, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _authenticateUserService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ForcefulLogoutWeb, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void ForcefulLogoutWeb_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ForcefulLogoutWeb, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _authenticateUserService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in ForcefulLogoutWeb")),
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

        #region ConnectionStatusDisconnected Test Cases
        [Fact]
        public void ConnectionStatusDisconnected_CallsSendUserBasedMessage()
        {
            // Act
            _authenticateUserService.ConnectionStatusDisconnected();

            // Assert
            _mockHubServiceGateway.Verify(
                x => x.SendUserBasedMessage(
                    KafkaConstants.TOPIC_ConnectionStatusDisconnected,
                    KafkaConstants.TOPIC_StatusDisconnected,
                    0,
                    string.Empty,
                    false),
                Times.Once
            );
        }


        [Fact]
        public void ConnectionStatusDisconnected_HandlesException()
        {
            try
            {
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _authenticateUserService.ConnectionStatusDisconnected();

                // Assert
                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in ConnectionStatusDisconnected")),
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

        #region AuthServiceInitialized Test Cases
        [Theory]
        [InlineData(1)]
        public void AuthServiceInitialized_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_AuthServiceInitialized, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _authenticateUserService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_AuthServiceInitialized, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void AuthServiceInitialized_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_AuthServiceInitialized, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _authenticateUserService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_AuthServiceInitialized, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in AuthServiceInitialized")),
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

        #region ProcessBloombergEventResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void ProcessBloombergEventResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BloombergEventResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _authenticateUserService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BloombergEventResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void ProcessBloombergEventResponse_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BloombergEventResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
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
                _authenticateUserService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BloombergEventResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in ProcessBloombergEventResponse")),
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

        #region EncrytPassword Test Cases
        [Fact]
        public void EncrytPassword_ReturnsExpectedAsciiCodes()
        {
            try
            {
                // Arrange
                var password = "Test123";
                var expectedAsciiCodes = new List<int> { 86, 100, 117, 115, 51, 49, 53 };

                // Act
                var result = _authenticateUserService.EncrytPassword(password);

                // Assert
                Assert.Equal(expectedAsciiCodes, result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public void EncrytPassword_ReturnsEmptyList_WhenPasswordIsEmpty()
        {
            // Arrange
            var password = string.Empty;

            // Act
            var result = _authenticateUserService.EncrytPassword(password);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void EncrytPassword_ThrowsException_WhenPasswordIsNull()
        {
            // Arrange
            string password = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _authenticateUserService.EncrytPassword(password));
        }

        [Fact]
        public void EncrytPassword_HandlesSpecialCharacters()
        {
            // Arrange
            var password = "!@#";
            var expectedAsciiCodes = new List<int> { 35, 63, 37 };

            // Act
            var result = _authenticateUserService.EncrytPassword(password);

            // Assert
            Assert.Equal(expectedAsciiCodes, result);
        }
        #endregion

    }
}
