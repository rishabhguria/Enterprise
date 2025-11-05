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
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class WatchlistDataServiceTest : BaseControllerTest
    {
        private readonly IWatchlistDataService _watchlistDataService;
        private readonly Mock<ILogger<WatchlistDataService>> _mockLogger;

        public WatchlistDataServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<WatchlistDataService>();
            _watchlistDataService = new WatchlistDataService(_mockKafkaManager.Object,
                _mockLogger.Object);
        }

        #region GetTabNames Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetTabNames_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var expectedResult = new Dictionary<string, int>
                {
                    { "Tab1", 1 },
                    { "Tab2", 2 }
                };
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, JsonConvert.SerializeObject(expectedResult),
                    KafkaConstants.TOPIC_WatchlistTabNamesRequest,
                    KafkaConstants.TOPIC_WatchlistTabNamesResponse);
                // Act
                var result = await _watchlistDataService.GetTabNames(1);

                // Assert
                Assert.Equal(expectedResult, result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetTabNames_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowTestException(requestResponseObj, KafkaConstants.TOPIC_WatchlistTabNamesResponse, KafkaConstants.TOPIC_WatchlistTabNamesRequest);

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _watchlistDataService.GetTabNames(1));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetTabWiseSymbols Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetTabWiseSymbols_ReturnsString(int companyUserId)
        {
            try
            {
                // Arrange
                var expectedResult = new Dictionary<string, HashSet<string>>
                {
                    { "Tab1", new HashSet<string>() }
                };
                var requestResponseObj = new RequestResponseModel(companyUserId, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, JsonConvert.SerializeObject(expectedResult),
                    KafkaConstants.TOPIC_WatchlistTabWiseSymbolsRequest,
                    KafkaConstants.TOPIC_WatchlistTabWiseSymbolsResponse);
                // Act
                var result = await _watchlistDataService.GetTabWiseSymbols(1);

                // Assert
                Assert.Equal(expectedResult, result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetTabWiseSymbols_ThrowsException_WhenProduceAndConsumeThrowsException(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock_ToThrowTestException(requestResponseObj, KafkaConstants.TOPIC_WatchlistTabWiseSymbolsResponse, KafkaConstants.TOPIC_WatchlistTabWiseSymbolsRequest);

                // Act and Assert
                await Assert.ThrowsAsync<Exception>(() => _watchlistDataService.GetTabWiseSymbols(1));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region AddTab Test Cases
        [Fact]
        public void AddTab_ValidRequest_CallsProduceMethodOnce()
        {
            try
            {
                // Arrange
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_WatchlistAddTabRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _watchlistDataService.AddTab(1, "Tab");

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_WatchlistAddTabRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task AddTab_ProduceThrowsException_LogsErrorAndThrowsCustomException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();

                // Act And Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                   _watchlistDataService.AddTab(1, "Tab"));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region RenameTab Test Cases
        [Fact]
        public void RenameTab_ValidRequest_CallsProduceMethodOnce()
        {
            try
            {
                // Arrange
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_WatchlistRenameTabRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _watchlistDataService.RenameTab(1, "Tab", "tab1");

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_WatchlistRenameTabRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task RenameTab_ProduceThrowsException_LogsErrorAndThrowsCustomException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();

                // Act And Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                   _watchlistDataService.RenameTab(1, "Tab", "tab1"));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region DeleteTab Test Cases
        [Fact]
        public void DeleteTab_ValidRequest_CallsProduceMethodOnce()
        {
            try
            {
                // Arrange
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_WatchlistDeleteTabRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _watchlistDataService.DeleteTab(1, "Tab");

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_WatchlistDeleteTabRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task DeleteTab_ProduceThrowsException_LogsErrorAndThrowsCustomException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();

                // Act And Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                   _watchlistDataService.DeleteTab(1, "Tab"));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region AddSymbolInTab Test Cases
        [Fact]
        public void AddSymbolInTab_ValidRequest_CallsProduceMethodOnce()
        {
            try
            {
                // Arrange
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_WatchlistAddSymbolInTabRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _watchlistDataService.AddSymbolInTab(1, "AAPl", "Tab");

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_WatchlistAddSymbolInTabRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task AddSymbolInTab_ProduceThrowsException_LogsErrorAndThrowsCustomException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();

                // Act And Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                   _watchlistDataService.AddSymbolInTab(1, "AAPL", "Tab"));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region RemoveSymbolFromTab Test Cases
        [Fact]
        public void RemoveSymbolFromTab_ValidRequest_CallsProduceMethodOnce()
        {
            try
            {
                // Arrange
                _mockKafkaManager
                    .Setup(x => x.Produce(KafkaConstants.TOPIC_WatchlistRemoveSymbolFromTabRequest, It.IsAny<RequestResponseModel>(), true))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                // Act
                _watchlistDataService.RemoveSymbolFromTab(1, "AAPl", "Tab");

                // Assert
                _mockKafkaManager.Verify(
                    x => x.Produce(KafkaConstants.TOPIC_WatchlistRemoveSymbolFromTabRequest, It.IsAny<RequestResponseModel>(), true),
                    Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task RemoveSymbolFromTab_ProduceThrowsException_LogsErrorAndThrowsCustomException()
        {
            try
            {
                // Arrange
                SetKafkaMock_ProduceThrowException();

                // Act And Assert
                var exception = await Assert.ThrowsAsync<Exception>(() =>
                   _watchlistDataService.RemoveSymbolFromTab(1, "AAPL", "Tab"));
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
                var watchlistDataService = new WatchlistDataService(
                    _mockKafkaManager.Object,
                    _mockLogger.Object
                );

                // Act
                watchlistDataService.Dispose();

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
