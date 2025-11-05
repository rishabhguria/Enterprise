using Microsoft.Extensions.Logging;
using Moq;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using Prana.ServiceGateway.Utility;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class BlotterServiceTest : BaseControllerTest
    {
        private readonly IBlotterService _blotterService;
        private readonly Mock<ILogger<BlotterService>> _mockLogger;
        public BlotterServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<BlotterService>();
            _blotterService = new BlotterService(_mockKafkaManager.Object, _mockHubServiceGateway.Object, _mockLogger.Object);
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

                _blotterService.Initialize();

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in Initialize of BlotterService")),
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

        #region GetBlotterData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetBlotterData_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BlotterGetDataRequest, KafkaConstants.TOPIC_BlotterGetDataResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.GetBlotterData(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetBlotterData_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BlotterGetDataResponse, KafkaConstants.TOPIC_BlotterGetDataRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.GetBlotterData(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CancelAllSubOrders Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CancelAllSubOrders_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BlotterCancelAllSubsRequest, KafkaConstants.TOPIC_BlotterCancelAllSubsResponse);
                _blotterService.Initialize();
                // Act
                await _blotterService.CancelAllSubOrders(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task CancelAllSubOrders_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, KafkaConstants.TOPIC_BlotterCancelAllSubsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.CancelAllSubOrders(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region RemoveOrders Test Cases
        [Theory]
        [InlineData(1)]
        public async Task RemoveOrders_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BlotterRemoveOrdersRequest, KafkaConstants.TOPIC_BlotterRemoveOrdersResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.RemoveOrders(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task RemoveOrders_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, KafkaConstants.TOPIC_BlotterRemoveOrdersRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.RemoveOrders(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region FreezeOrdersInPendingComplianceUI Test Cases
        [Theory]
        [InlineData(1)]
        public async Task FreezeOrdersInPendingComplianceUI_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_FreezePendingComplianceRowsRequest, KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.FreezeOrdersInPendingComplianceUI(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task FreezeOrdersInPendingComplianceUI_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, KafkaConstants.TOPIC_FreezePendingComplianceRowsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.FreezeOrdersInPendingComplianceUI(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region UnfreezeOrdersInPendingComplianceUI Test Cases
        [Theory]
        [InlineData(1)]
        public async Task UnfreezeOrdersInPendingComplianceUI_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_UnfreezePendingComplianceRowsRequest, KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.UnfreezeOrdersInPendingComplianceUI(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task UnfreezeOrdersInPendingComplianceUI_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, KafkaConstants.TOPIC_UnfreezePendingComplianceRowsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.UnfreezeOrdersInPendingComplianceUI(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region RemoveManualExecution Test Cases
        [Theory]
        [InlineData(1)]
        public async Task RemoveManualExecution_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BlotterRemoveManualExecutionRequest, KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.RemoveManualExecution(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task RemoveManualExecution_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, KafkaConstants.TOPIC_BlotterRemoveManualExecutionRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.RemoveManualExecution(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetBlotterManualFills Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetBlotterManualFills_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BlotterGetManualFillsRequest, KafkaConstants.TOPIC_BlotterGetManualFillsResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.GetBlotterManualFills(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetBlotterManualFills_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BlotterGetManualFillsResponse, KafkaConstants.TOPIC_BlotterGetManualFillsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.GetBlotterManualFills(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveManualFills Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveManualFills_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_BlotterSaveManualFillsRequest, KafkaConstants.TOPIC_BlotterSaveManualFillsResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.SaveManualFills(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SaveManualFills_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, KafkaConstants.TOPIC_BlotterSaveManualFillsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.SaveManualFills(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetAllocationDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetAllocationDetails_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_GetAllocationDetailsRequest, KafkaConstants.TOPIC_GetAllocationDetailsResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.GetAllocationDetails(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetAllocationDetails_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_GetAllocationDetailsResponse, KafkaConstants.TOPIC_GetAllocationDetailsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.GetAllocationDetails(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveAllocationDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveAllocationDetails_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SaveAllocationDetailsRequest, KafkaConstants.TOPIC_SaveAllocationDetailsResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.SaveAllocationDetails(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SaveAllocationDetails_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SaveAllocationDetailsResponse, KafkaConstants.TOPIC_SaveAllocationDetailsRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.SaveAllocationDetails(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region BlotterUpdatedDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void Initialize_BlotterUpdatedDataResponse(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterUpdatedData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                // Invoke BlotterUpdatedDataHandler indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterUpdatedData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region BlotterRemovedDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void Initialize_BlotterRemovedDataResponse(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemovedData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                // Invoke BlotterRemovedDataHandler indirectly by calling actualHandler
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemovedData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void BlotterRemovedDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var loggerInfo = new LoggerInfo();
                loggerInfo.message = UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemovedData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in BroadcastMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        KafkaConstants.TOPIC_BlotterRemovedData,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                // Invoke BlotterRemovedDataHandler indirectly by calling actualHandler
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemovedData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                // Optionally, verify logger was called with LogError (if you have a mock logger)
                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in BlotterRemovedDataHandler")),
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

        #region RenameBlotterCustomTab Test Cases
        [Theory]
        [InlineData(1)]
        public async Task RenameBlotterCustomTab_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_RenameBlotterCustomTabRequest, KafkaConstants.TOPIC_RenameBlotterCustomTabResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.RenameBlotterCustomTab(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task RenameBlotterCustomTab_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, KafkaConstants.TOPIC_RenameBlotterCustomTabRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.RenameBlotterCustomTab(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region RemoveBlotterCustomTab Test Cases
        [Theory]
        [InlineData(1)]
        public async Task RemoveBlotterCustomTab_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_RemoveBlotterCustomTabRequest, KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse);

                _blotterService.Initialize();
                // Act
                 await _blotterService.RemoveBlotterCustomTab(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task RemoveBlotterCustomTab_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, KafkaConstants.TOPIC_RemoveBlotterCustomTabRequest);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.RemoveBlotterCustomTab(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region TradingAccountsDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void TradingAccountsDataHandler_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_UserWiseTradingAccountsRequest, KafkaConstants.TOPIC_UserWiseTradingAccountsResponse);

                _blotterService.Initialize();
                // Act
                _blotterService.TradingAccountsDataHandler(KafkaConstants.TOPIC_UserWiseTradingAccountsRequest, requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public void TradingAccountsDataHandler_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_UserWiseTradingAccountsResponse, KafkaConstants.TOPIC_UserWiseTradingAccountsRequest);

                _blotterService.Initialize();

                // Act and Assert
                Assert.ThrowsAsync<Exception>(() => Task.Run(() => _blotterService.TradingAccountsDataHandler(KafkaConstants.TOPIC_UserWiseTradingAccountsRequest, requestResponseObj)));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void TradingAccountsDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseTradingAccountsRequest, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseTradingAccountsRequest, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in TradingAccountsDataHandler")),
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

        #region TransferUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task TransferUser_ReturnsString(int companyUserId)
        {
            try
            {
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_TransferUserRequest, KafkaConstants.TOPIC_TransferUserResponse);

                // Act
                await _blotterService.TransferUser(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task TransferUser_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_TransferUserRequest, KafkaConstants.TOPIC_TransferUserResponse);

                _blotterService.Initialize();

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _blotterService.TransferUser(requestResponseObj));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetPstData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetPstData_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
                _blotterService.Initialize();

                // Act
                await _blotterService.GetPstData(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPstData_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);
                _blotterService.Initialize();

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _blotterService.GetPstData(requestResponseObj));

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

        #region RolloverAllSubOrders Test Cases
        [Theory]
        [InlineData(1)]
        public async Task RolloverAllSubOrders_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_BlotterRolloverAllSubsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
                _blotterService.Initialize();

                // Act
                await _blotterService.RolloverAllSubOrders(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_BlotterRolloverAllSubsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task RolloverAllSubOrders_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_BlotterRolloverAllSubsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);
                _blotterService.Initialize();

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _blotterService.RolloverAllSubOrders(requestResponseObj));

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

        #region GetPstAllocationDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetPstAllocationDetails_ValidRequest_CallsProduceMethodOnce(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetPstAllocationDetailsRequest, requestResponseObj, true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
                _blotterService.Initialize();

                // Act
                await _blotterService.GetPstAllocationDetails(requestResponseObj);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_GetPstAllocationDetailsRequest, requestResponseObj, true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetPstAllocationDetails_ProduceThrowsException_LogsErrorAndThrowsCustomException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                var kafkaException = new Exception("Kafka error");

                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_GetPstAllocationDetailsRequest, requestResponseObj, true))
                    .ThrowsAsync(kafkaException);
                _blotterService.Initialize();

                // Act
                var exception = await Assert.ThrowsAsync<Exception>(async () =>
                    await _blotterService.GetPstAllocationDetails(requestResponseObj));

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

        #region OrderUpdatesDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void OrderUpdatesDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_OrderUpdatesData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_OrderUpdatesData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void OrderUpdatesDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_OrderUpdatesData, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception in BroadcastMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        KafkaConstants.TOPIC_OrderUpdatesData,
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_OrderUpdatesData, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                // Optionally, verify logger was called with LogError (if you have a mock logger)
                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in OrderUpdatesDataHandler")),
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

        #region RenameBlotterCustomTabHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RenameBlotterCustomTabHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RenameBlotterCustomTabHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                // Simulate exception in BroadcastMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RenameBlotterCustomTabHandler")),
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

        #region RemoveBlotterCustomTabHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RemoveBlotterCustomTabHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RemoveBlotterCustomTabHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);
                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                // Simulate exception in BroadcastMessage
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();
                                
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RemoveBlotterCustomTabHandler")),
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

        #region RemoveOrdersHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RemoveOrdersHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RemoveOrdersHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RemoveOrdersHandler")),
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

        #region FreezeOrdersInPendingComplianceUIHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void FreezeOrdersInPendingComplianceUIHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void FreezeOrdersInPendingComplianceUIHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_FreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in FreezeOrdersInPendingComplianceUIHandler")),
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

        #region UnfreezeOrdersInPendingComplianceUIHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void UnfreezeOrdersInPendingComplianceUIHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void UnfreezeOrdersInPendingComplianceUIHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in UnfreezeOrdersInPendingComplianceUIHandler")),
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

        #region GetAllocationDetailsHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetAllocationDetailsHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetAllocationDetailsHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetAllocationDetailsHandler")),
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

        #region SaveAllocationDetailsHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SaveAllocationDetailsHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SaveAllocationDetailsHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SaveAllocationDetailsHandler")),
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

        #region GetPstAllocationDetailsHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetPstAllocationDetailsHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"Identifier\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetPstAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetPstAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetPstAllocationDetailsHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"Identifier\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetPstAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetPstAllocationDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetPstAllocationDetailsHandler")),
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

        #region CancelAllSubOrdersHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void CancelAllSubOrdersHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void CancelAllSubOrdersHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in CancelAllSubOrdersHandler")),
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

        #region TransferUserFromHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void TransferUserFromHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"uniqueIdentifier\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserFromRequest, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserFromRequest, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void TransferUserFromHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"uniqueIdentifier\": \"1234\"}", CorrelationId);
                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserFromRequest, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserFromRequest, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in TransferUserFromHandler")),
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

        #region TransferUserHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void TransferUserHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"uniqueIdentifier\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void TransferUserHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"uniqueIdentifier\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in TransferUserHandler")),
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

        #region RolloverAllSubOrdersHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RolloverAllSubOrdersHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RolloverAllSubOrdersHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RolloverAllSubOrdersHandler")),
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

        #region GetBlotterDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetBlotterDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"blotterId\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetBlotterDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"blotterId\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetDataResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetBlotterDataHandler")),
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

        #region GetPstDataHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetPstDataHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"blotterId\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetPstDataHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, "{\"blotterId\": \"1234\"}", CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetPstDataHandler")),
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

        #region GetBlotterManualFillsHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetBlotterManualFillsHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void GetBlotterManualFillsHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetBlotterManualFillsHandler")),
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

        #region RemoveManualExecutionHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void RemoveManualExecutionHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void RemoveManualExecutionHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RemoveManualExecutionHandler")),
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

        #region SaveManualFillsHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SaveManualFillsHandler_CallsSendUserBasedMessage_AndLogsTrace(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Act
                _blotterService.Initialize();

                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId));

                // Assert
                _mockKafkaManager.Verify(
                x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public void SaveManualFillsHandler_HandlesException(int companyUserID)
        {
            try
            {
                Action<string, RequestResponseModel> actualHandler = null;

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(), // Accept any topic
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated exception"));

                // Act
                _blotterService.Initialize();

                var request = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                actualHandler(UnitTestConstants.CONST_MOCK_TOPIC, request);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                          x => x.Log(
                              LogLevel.Error,
                              It.IsAny<EventId>(),
                              It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SaveManualFillsHandler")),
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

        #region GetOrderDetailsForEditTradeAttributes Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetOrderDetailsForEditTradeAttributes_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesRequest, KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.GetOrderDetailsForEditTradeAttributes(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetOrderDetailsForEditTradeAttributes_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesRequest);

                _blotterService.Initialize();

                // Act and Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => _blotterService.GetOrderDetailsForEditTradeAttributes(requestResponseObj));

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in RemoveBlotterCustomTab")), // This is the specific log message in your service
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once
                );

                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveEditedTradeAttributes Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveEditedTradeAttributes_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SaveEditedTradeAttributesRequest, KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse);

                _blotterService.Initialize();
                // Act
                await _blotterService.SaveEditedTradeAttributes(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveEditedTradeAttributes_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowException(requestResponseObj, KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, KafkaConstants.TOPIC_SaveEditedTradeAttributesRequest);

                _blotterService.Initialize();

                // Act and Assert
                var exception = await Assert.ThrowsAsync<Exception>(() => _blotterService.SaveEditedTradeAttributes(requestResponseObj));

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in SaveEditedTradeAttributes")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once
                );

                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, exception.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetOrderDetailsForEditTradeAttributesResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void GetOrderDetailsForEditTradeAttributesResponseHandler_CallsSendUserBasedMessage_AndLogsInformation(int companyUserID)
        {
            try
            {
                // Arrange
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _blotterService.Initialize();

                // Act
                actualHandler(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockHubServiceGateway.Verify(
                    x => x.SendUserBasedMessage(
                        HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, message),
                        message),
                    Times.Once);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("GetOrderDetailsForEditTradeAttributes response received.")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once
                );

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Message successfully sent to topic:")),
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

        [Theory]
        [InlineData(1)]
        public void GetOrderDetailsForEditTradeAttributesResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                // Arrange
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception when SendUserBasedMessage is called
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated SignalR exception"));

                // Act
                _blotterService.Initialize();
                actualHandler(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetOrderDetailsForEditTradeAtteibutes")),
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

        #region SaveEditedTradeAttributesResponseHandler Test Cases
        [Theory]
        [InlineData(1)]
        public void SaveEditedTradeAttributesResponseHandler_CallsSendUserBasedMessage_AndLogsInformation(int companyUserID)
        {
            try
            {
                // Arrange
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                _blotterService.Initialize();

                // Act
                actualHandler(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockHubServiceGateway.Verify(
                    x => x.SendUserBasedMessage(
                        HelperFunctions.GetUpdatedTopicForResponse(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, message),
                        message),
                    Times.Once);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("SaveEditedTradeAttributes response received.")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once
                );

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Message successfully sent to topic:")),
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

        [Theory]
        [InlineData(1)]
        public void SaveEditedTradeAttributesResponseHandler_HandlesException(int companyUserID)
        {
            try
            {
                // Arrange
                Action<string, RequestResponseModel> actualHandler = null;
                var message = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);

                _mockKafkaManager
                    .Setup(x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false))
                    .Callback<string, Action<string, RequestResponseModel>, bool>((topic, handler, flag) => actualHandler = handler);

                // Simulate exception when SendUserBasedMessage is called
                _mockHubServiceGateway
                    .Setup(x => x.SendUserBasedMessage(
                        It.IsAny<string>(),
                        It.IsAny<RequestResponseModel>()))
                    .Throws(new Exception("Simulated SignalR exception"));

                // Act
                _blotterService.Initialize();
                actualHandler(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, message);

                // Assert
                _mockKafkaManager.Verify(
                    x => x.SubscribeAndConsume(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, It.IsAny<Action<string, RequestResponseModel>>(), false),
                    Times.Once);

                _mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error in GetOrderDetailsForEditTradeAtteibutes")), // Note: This error message is from the original code, ensure it's correct for this method
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
