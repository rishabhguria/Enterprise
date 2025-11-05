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
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class CommonDataServiceTest : BaseControllerTest
    {
        private readonly ICommonDataService _commonDataService;
        private readonly Mock<ILogger<CommonDataService>> _mockLogger;

        public CommonDataServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<CommonDataService>();
            _commonDataService = new CommonDataService(
                _mockKafkaManager.Object,
                _mockHubServiceGateway.Object,
                _mockLogger.Object);
        }

        #region GetCompanyTransferTradeRules Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetCompanyTransferTradeRules_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest, KafkaConstants.TOPIC_CompanyTransferTradeRulesResponse);

                _commonDataService.Initialize();
                // Act
                await _commonDataService.GetCompanyTransferTradeRules(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetCompanyTransferTradeRules_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_CompanyTransferTradeRulesResponse, KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest);

                _commonDataService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _commonDataService.GetCompanyTransferTradeRules(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetTradingTicketAllData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetTradingTicketAllData_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_CompanyTradingTicketRequest, KafkaConstants.TOPIC_CompanyTradingTicketResponse);

                _commonDataService.Initialize();
                // Act
                await _commonDataService.GetTradingTicketAllData(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetTradingTicketAllData_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_CompanyTradingTicketResponse, KafkaConstants.TOPIC_CompanyTradingTicketRequest);

                _commonDataService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _commonDataService.GetTradingTicketAllData(requestResponseObj));
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

                var exception = Assert.Throws<Exception>(() => _commonDataService.Initialize());
                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Initialize of commonDataService")),
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

        #region GetCompanyTransferTradeRulesHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetCompanyTransferTradeRulesHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTransferTradeRulesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _commonDataService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTransferTradeRulesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region GetTradingTicketAllDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetTradingTicketAllDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\":\"12345\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingTicketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _commonDataService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingTicketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetTradingTicketAllDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingTicketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _commonDataService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingTicketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetTradingTicketAllDataHandler")),
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

        #region GetUserOrderType Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetUserOrderType_ReturnsExpectedResult(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                var expectedResponse = "ExpectedResult";

                _mockKafkaManager
                    .Setup(x => x.ProduceAndConsume(
                        requestResponseObj,
                        KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest,
                        KafkaConstants.TOPIC_UserWiseOrderTypeDataResponse,
                        false))
                    .ReturnsAsync(expectedResponse);

                var commonDataService = new CommonDataService(
                    _mockKafkaManager.Object,
                    _mockHubServiceGateway.Object,
                    _mockLogger.Object
                );

                // Act
                var result = await commonDataService.GetUserOrderType(requestResponseObj);

                // Assert
                Assert.Equal(expectedResponse, result);
                _mockKafkaManager.Verify(
                    x => x.ProduceAndConsume(
                        requestResponseObj,
                        KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest,
                        KafkaConstants.TOPIC_UserWiseOrderTypeDataResponse,
                        false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetUserOrderType_LogsErrorAndThrowsException_WhenExceptionOccurs(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                var simulatedException = new Exception("Simulated exception");

                _mockKafkaManager
                    .Setup(x => x.ProduceAndConsume(
                        requestResponseObj,
                        KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest,
                        KafkaConstants.TOPIC_UserWiseOrderTypeDataResponse,
                        false))
                    .ThrowsAsync(simulatedException);

                var commonDataService = new CommonDataService(
                    _mockKafkaManager.Object,
                    _mockHubServiceGateway.Object,
                    _mockLogger.Object
                );

                // Act & Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => commonDataService.GetUserOrderType(requestResponseObj));
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
                Assert.Contains(simulatedException.Message, exception.Message);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetUserOrderType")),
                        simulatedException,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
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
                var commonDataService = new CommonDataService(
                    _mockKafkaManager.Object,
                    _mockHubServiceGateway.Object,
                    _mockLogger.Object
                );

                // Act
                commonDataService.Dispose();

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
    }
}
