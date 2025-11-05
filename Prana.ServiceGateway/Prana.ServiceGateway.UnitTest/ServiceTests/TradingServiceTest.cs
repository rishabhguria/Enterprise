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
using Prana.ServiceGateway.Utility;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class TradingServiceTest : BaseControllerTest
    {
        private readonly ITradingService _tradingService;
        private readonly Mock<ILogger<TradingService>> _mockLogger;

        public TradingServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<TradingService>();
            var mockServiceHealthStatusStore = new Mock<ServiceHealthStatusStore>(); // Mocking the missing dependency  
            _tradingService = new TradingService(_mockKafkaManager.Object,
                _mockHubServiceGateway.Object,
                _mockLogger.Object,
                _mockHubServiceGatewayRTPNL.Object); // Passing the mocked dependency  
        }

        #region GetSymbolAccountWisePosition Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetSymbolAccountWisePosition_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                // Act
                await _tradingService.GetSymbolAccountWisePosition(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(x => x.Produce(KafkaConstants.TOPIC_SymbolAccountWisePositionRequest, requestResponseObj, true), Times.Once);

            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetSymbolAccountWisePosition_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ProduceThrowException();

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.GetSymbolAccountWisePosition(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetAlgoStrategiesFromBroker Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetAlgoStrategiesFromBroker_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesRequest, KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.GetAlgoStrategiesFromBroker(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetAlgoStrategiesFromBroker_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.GetAlgoStrategiesFromBroker(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendStageOrderFromTT Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendStageOrderFromTT_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SendStageOrderRequest, KafkaConstants.TOPIC_SendStageOrderResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.SendStageOrderFromTT(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendStageOrderFromTT_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SendStageOrderResponse, KafkaConstants.TOPIC_SendStageOrderRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.SendStageOrderFromTT(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendManualOrderFromTT Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendManualOrderFromTT_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SendManualOrderRequest, KafkaConstants.TOPIC_SendManualOrderResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.SendManualOrderFromTT(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendManualOrderFromTT_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {

                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SendManualOrderResponse, KafkaConstants.TOPIC_SendManualOrderRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.SendManualOrderFromTT(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendLiveOrderFromTT Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendLiveOrderFromTT_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SendLiveOrderRequest, KafkaConstants.TOPIC_SendLiveOrderResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.SendLiveOrderFromTT(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendLiveOrderFromTT_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SendLiveOrderResponse, KafkaConstants.TOPIC_SendLiveOrderRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.SendLiveOrderFromTT(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendReplaceOrderFromTT Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendReplaceOrderFromTT_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SendReplaceOrderRequest, KafkaConstants.TOPIC_SendReplaceOrderResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.SendReplaceOrderFromTT(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendReplaceOrderFromTT_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SendReplaceOrderResponse, KafkaConstants.TOPIC_SendReplaceOrderRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.SendReplaceOrderFromTT(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetCustomAllocationDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetCustomAllocationDetails_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_CustomAllocationDetailsRequest, KafkaConstants.TOPIC_CustomAllocationDetailsResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.GetCustomAllocationDetails(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetCustomAllocationDetails_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_CustomAllocationDetailsResponse, KafkaConstants.TOPIC_CustomAllocationDetailsRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.GetCustomAllocationDetails(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetSavedCustomAllocationDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetSavedCustomAllocationDetails_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SavedCustomAllocationDetailsRequest, KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse);

                _tradingService.Initialize();
                // Act
                await _tradingService.GetSavedCustomAllocationDetails(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetSavedCustomAllocationDetails_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse, KafkaConstants.TOPIC_SavedCustomAllocationDetailsRequest);

                _tradingService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _tradingService.GetSavedCustomAllocationDetails(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region BrokerStatusResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void Initialize_SubscribesToBrokerStatusResponse(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerStatusResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerStatusResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Initialize_BrokerStatusResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerStatusResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        ServicesMethodConstants.CONST_BROKER_STATUS_RESPONSE,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerStatusResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region BookAsSwapReplace Test Cases

        [Fact]
        public async Task BookAsSwapReplace_ValidRequest_CallsProduceMethodOnce()
        {
            // Arrange
            var requestResponseObj = new RequestResponseModel
            {
                CorrelationId = "12345",
                Data = "SampleData",
                CompanyUserID = 1
            };

            _mockKafkaManager
                .Setup(x => x.Produce(KafkaConstants.TOPIC_BookAsSwapRequest, requestResponseObj, true))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _tradingService.BookAsSwapReplace(requestResponseObj);

            // Assert
            _mockKafkaManager.Verify(
                x => x.Produce(KafkaConstants.TOPIC_BookAsSwapRequest, requestResponseObj, true),
                Times.Once);
        }

        [Fact]
        public async Task BookAsSwapReplace_ProduceThrowsException_LogsErrorAndThrowsCustomException()
        {
            // Arrange
            var requestResponseObj = new RequestResponseModel
            {
                CorrelationId = "12345",
                Data = "SampleData",
                CompanyUserID = 1
            };

            var kafkaException = new Exception("Kafka error");

            _mockKafkaManager
                .Setup(x => x.Produce(KafkaConstants.TOPIC_BookAsSwapRequest, requestResponseObj, true))
                .ThrowsAsync(kafkaException);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _tradingService.BookAsSwapReplace(requestResponseObj));

            // Assert
            Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
            Assert.Contains(kafkaException.Message, exception.Message);
        }
        #endregion

        #region GetSymbolWiseShortLocateOrders Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetSymbolWiseShortLocateOrders_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.GetSymbolWiseShortLocateOrders(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetSymbolWiseShortLocateOrders_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.GetSymbolWiseShortLocateOrders(requestResponseObj));

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

        #region DetermineSecurityBorrowType Test Cases

        [Theory]
        [InlineData(1)]
        public async Task DetermineSecurityBorrowType_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.DetermineSecurityBorrowType(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DetermineSecurityBorrowType_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.DetermineSecurityBorrowType(requestResponseObj));

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

        #region GetCompanyUserHotKeyPreferences Test Cases

        [Theory]
        [InlineData(1)]
        public async Task GetCompanyUserHotKeyPreferences_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.GetCompanyUserHotKeyPreferences(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetCompanyUserHotKeyPreferences_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.GetCompanyUserHotKeyPreferences(requestResponseObj));

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

        #region UpdateCompanyUserHotKeyPreferences Test Cases

        [Theory]
        [InlineData(1)]
        public async Task UpdateCompanyUserHotKeyPreferences_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.UpdateCompanyUserHotKeyPreferences(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateCompanyUserHotKeyPreferences_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.UpdateCompanyUserHotKeyPreferences(requestResponseObj));

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

        #region GetCompanyUserHotKeyPreferencesDetails Test Cases

        [Theory]
        [InlineData(1)]
        public async Task GetCompanyUserHotKeyPreferencesDetails_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.GetCompanyUserHotKeyPreferencesDetails(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetCompanyUserHotKeyPreferencesDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.GetCompanyUserHotKeyPreferencesDetails(requestResponseObj));

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

        #region UpdateCompanyUserHotKeyPreferencesDetails Test Cases

        [Theory]
        [InlineData(1)]
        public async Task UpdateCompanyUserHotKeyPreferencesDetails_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.UpdateCompanyUserHotKeyPreferencesDetails(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateCompanyUserHotKeyPreferencesDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.UpdateCompanyUserHotKeyPreferencesDetails(requestResponseObj));

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

        #region SaveCompanyUserHotKeyPreferencesDetails Test Cases

        [Theory]
        [InlineData(1)]
        public async Task SaveCompanyUserHotKeyPreferencesDetails_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SaveCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.SaveCompanyUserHotKeyPreferencesDetails(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SaveCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveCompanyUserHotKeyPreferencesDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SaveCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.SaveCompanyUserHotKeyPreferencesDetails(requestResponseObj));

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

        #region DeleteCompanyUserHotKeyPreferencesDetails Test Cases

        [Theory]
        [InlineData(1)]
        public async Task DeleteCompanyUserHotKeyPreferencesDetails_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_DeleteCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.DeleteCompanyUserHotKeyPreferencesDetails(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_DeleteCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteCompanyUserHotKeyPreferencesDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_DeleteCompanyUserHotKeyPreferencesDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.DeleteCompanyUserHotKeyPreferencesDetails(requestResponseObj));

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

        #region CreatePopUpText Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CreatePopUpText_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CreatePopUpTextRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.CreatePopUpText(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_CreatePopUpTextRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task CreatePopUpText_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CreatePopUpTextRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.CreatePopUpText(requestResponseObj));

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

        #region CreateOptionSymbol Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CreateOptionSymbol_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CreateOptionSymbolRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.CreateOptionSymbol(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_CreateOptionSymbolRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task CreateOptionSymbol_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CreateOptionSymbolRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.CreateOptionSymbol(requestResponseObj));

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

        #region GetSMData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetSMData_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetSMDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.GetSMData(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_GetSMDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetSMData_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetSMDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.GetSMData(requestResponseObj));

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

        #region UnsubscribeSymbolCompressionFeed Test Cases
        [Theory]
        [InlineData(1)]
        public async Task UnsubscribeSymbolCompressionFeed_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UnsubscribeSymbolCompressionFeedRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.UnsubscribeSymbolCompressionFeed(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_UnsubscribeSymbolCompressionFeedRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task UnsubscribeSymbolCompressionFeed_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_UnsubscribeSymbolCompressionFeedRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.UnsubscribeSymbolCompressionFeed(requestResponseObj));

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

        #region GetBrokerConnectionAndVenuesData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetBrokerConnectionAndVenuesData_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.GetBrokerConnectionAndVenuesData(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetBrokerConnectionAndVenuesData_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.GetBrokerConnectionAndVenuesData(requestResponseObj));

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

        #region GetPstAccountNav Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetPstAccountNav_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasket, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.GetPstAccountNav(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasket, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPstAccountNav_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasket, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.GetPstAccountNav(requestResponseObj));

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

        #region SendPstOrders Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendPstOrders_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_PSTOrderRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.SendPstOrders(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_PSTOrderRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendPstOrders_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_PSTOrderRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.SendPstOrders(requestResponseObj));

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

        #region CreatePstAllocatonPreference Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CreatePstAllocatonPreference_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CreatePstAllocatonPreferenceRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.CreatePstAllocatonPreference(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_CreatePstAllocatonPreferenceRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task CreatePstAllocatonPreference_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_CreatePstAllocatonPreferenceRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.CreatePstAllocatonPreference(requestResponseObj));

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

        #region GetRegionOfBrokerFrSymbolAUECIDResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetRegionOfBrokerFrSymbolAUECIDResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetRegionOfBrokerFrSymbolAUECIDResponseHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        ServicesMethodConstants.CONST_GET_REGION_OF_BROKER_FR_SYMBOL_AUECID_RESPONSE,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region DetermineBorrowTypeResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void DetermineBorrowTypeResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void DetermineBorrowTypeResponseHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, request),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SymbolAccountWisePositionResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SymbolAccountWisePositionResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SymbolAccountWisePositionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SymbolAccountWisePositionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SymbolAccountWisePositionResponseHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SymbolAccountWisePositionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        KafkaConstants.TOPIC_SymbolAccountWisePositionResponse,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SymbolAccountWisePositionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ShortLocateOrdersSymbolWiseResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void ShortLocateOrdersSymbolWiseResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void ShortLocateOrdersSymbolWiseResponseHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, request),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CompanyUserHotKeyPreferencesResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void CompanyUserHotKeyPreferencesResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CompanyUserHotKeyPreferencesResponseHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, request),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CompanyUserHotKeyPreferencesDetailsResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void CompanyUserHotKeyPreferencesDetailsResponseHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CompanyUserHotKeyPreferencesDetailsResponseHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, request),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendStageOrderHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SendStageOrderHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SendStageOrderHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendManualOrderHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SendManualOrderHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendManualOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendManualOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SendManualOrderHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendManualOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendManualOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendReplaceOrderHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SendReplaceOrderHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendReplaceOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendReplaceOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SendReplaceOrderHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendReplaceOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendReplaceOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendLiveOrderHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SendLiveOrderHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendLiveOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendLiveOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SendLiveOrderHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendLiveOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SendLiveOrderResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetSMDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetSMDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetSMDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetSMDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetSMDataHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetSMDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetSMDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CreatePopupTextHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void CreatePopupTextHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePopUpTextResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePopUpTextResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CreatePopupTextHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePopUpTextResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePopUpTextResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CreateOptionSymbolHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void CreateOptionSymbolHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreateOptionSymbolResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreateOptionSymbolResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CreateOptionSymbolHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreateOptionSymbolResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreateOptionSymbolResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetCustomAllocationHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetCustomAllocationHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"ViewId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetCustomAllocationHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"ViewId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetSavedCustomAllocationHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetSavedCustomAllocationHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetSavedCustomAllocationHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SavedCustomAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetAlgoStrategiesFromBrokerHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetAlgoStrategiesFromBrokerHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetAlgoStrategiesFromBrokerHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetBrokerConnectionAndVenuesDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetBrokerConnectionAndVenuesDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetBrokerConnectionAndVenuesDataHandler_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CreatePstAllocationPrefResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void CreatePstAllocationPrefResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePstAllocationPrefResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePstAllocationPrefResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CreatePstAllocationPrefResponse_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"tradingTicketId\": \"someTradingTicketIdValue\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePstAllocationPrefResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePstAllocationPrefResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetAccountNavNStartingValueFromBasketResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void GetAccountNavNStartingValueFromBasketResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetAccountNavNStartingValueFromBasketResponse_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        ServicesMethodConstants.CONST_GET_ACCOUNT_NAV_RESP,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasketResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region KafkaManager_TradeAttributeLabelsResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void KafkaManager_TradeAttributeLabelsResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void KafkaManager_TradeAttributeLabelsResponse_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        KafkaConstants.TOPIC_TradeAttributeLabelsResponse,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeLabelsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region KafkaManager_TradeAttributeValuesResponse Test Cases
        [Theory]
        [InlineData(1)]
        public void KafkaManager_TradeAttributeValuesResponse_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeValuesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponse indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID,
                    UnitTestConstants.CONST_REQUEST_DATA,
                    CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeValuesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void KafkaManager_TradeAttributeValuesResponse_LogsError_OnException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeValuesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in SendUserBasedMessage
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        KafkaConstants.TOPIC_TradeAttributeValuesResponse,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        false))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _tradingService.Initialize();

                // Invoke BrokerStatusResponseHandler indirectly

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TradeAttributeValuesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ProduceTradeAttributeValuesEvent Test Cases
        [Theory]
        [InlineData(1)]
        public async Task ProduceTradeAttributeValuesEvent_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetTradeAttributeValues, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.ProduceTradeAttributeValuesEvent(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_GetTradeAttributeValues, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task ProduceTradeAttributeValuesEvent_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetTradeAttributeValues, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.ProduceTradeAttributeValuesEvent(requestResponseObj));

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

        #region ProduceTradeAttributeLabelsEvent Test Cases
        [Theory]
        [InlineData(1)]
        public async Task ProduceTradeAttributeLabelsEvent_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetTradeAttributeLabels, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.ProduceTradeAttributeLabelsEvent(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_GetTradeAttributeLabels, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task ProduceTradeAttributeLabelsEvent_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetTradeAttributeLabels, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.ProduceTradeAttributeLabelsEvent(requestResponseObj));

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

        #region SendOrdersToMarket Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendOrdersToMarket_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SendOrdersToMarketRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                await _tradingService.SendOrdersToMarket(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_SendOrdersToMarketRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendOrdersToMarket_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_SendOrdersToMarketRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _tradingService.SendOrdersToMarket(requestResponseObj));

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

        #region Initialize Test Cases
        [Fact]
        public void Initialize_LogsError_WhenExceptionIsThrown()
        {
            // Simulate an exception when SubscribeAndConsume is called
            _mockKafkaManager
                .Setup(x => x.SubscribeAndConsume(It.IsAny<string>(), It.IsAny<Action<string, RequestResponseModel>>(), It.IsAny<bool>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            _tradingService.Initialize();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Service")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once
            );
        }

        #endregion
    }
}
