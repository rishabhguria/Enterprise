using Microsoft.AspNetCore.Mvc;
using Moq;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.UnitTest.Commons;
using Prana.ServiceGateway.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;
using Prana.ServiceGateway.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class LiveFeedControllerTest : BaseControllerTest, IDisposable
    {
        private readonly LiveFeedController _controller;

        public LiveFeedControllerTest() : base()
        {
            _controller = new LiveFeedController(
                 _mockValidationHelper.Object,
                 _mockTokenService.Object,
                 _mockLiveFeedService.Object,
                 GetMockLogger<LiveFeedController>());
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Sets the value for UserDto object
        /// </summary>
        /// <param name="companyUserId"></param>
        private void SetUserDto(int companyUserId)
        {
            try
            {
                var userDto = new UserDto
                {
                    CompanyUserId = Convert.ToInt32(companyUserId),
                    IsAdmin = true,
                    IsSupport = false,
                };
                _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>())).Returns(userDto);
                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #region SubscribeLiveFeed Test Cases
        [Fact]
        public async Task SubscribeLiveFeed_ReturnsOkResult()
        {
            try
            {
                SetUserDto(1);
                // Act
                var result = await _controller.SubscribeLiveFeed(UnitTestConstants.CONST_MOCK_SYMBOL);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockLiveFeedService.Verify(x => x.RequestSymbol(UnitTestConstants.CONST_MOCK_SYMBOL, 1), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SubscribeLiveFeed_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);

                // Act
                var result = await _controller.SubscribeLiveFeed(UnitTestConstants.CONST_MOCK_SYMBOL);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SubscribeLiveFeed_ThrowsException()
        {
            try
            {
                SetUserDto(1);
                _mockLiveFeedService.Setup(x => x.RequestSymbol(It.IsAny<string>(), It.IsAny<int>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                var result = await _controller.SubscribeLiveFeed(UnitTestConstants.CONST_MOCK_SYMBOL);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region UnSubscribeLiveFeed Test Cases

        [Fact]
        public async Task UnSubscribeLiveFeed_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);

                // Act
                var result = await _controller.UnSubscribeLiveFeed(UnitTestConstants.CONST_MOCK_SYMBOL);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task UnSubscribeLiveFeed_ReturnsOk_WhenValidRequest()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                _mockLiveFeedService.Setup(x => x.UnSubscribeLiveFeed(It.IsAny<RequestResponseModel>())).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.UnSubscribeLiveFeed(UnitTestConstants.CONST_MOCK_SYMBOL);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockLiveFeedService.Verify(x => x.UnSubscribeLiveFeed(It.IsAny<RequestResponseModel>()), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region UpdateMarketDataTokenRequest Test Cases

        [Fact]
        public async Task UpdateMarketDataTokenRequest_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var dto = new GenericDto { Data = "token-data" };

                // Act
                var result = await _controller.UpdateMarketDataTokenRequest(dto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task UpdateMarketDataTokenRequest_ReturnsOk_WhenValidRequest()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var dto = new GenericDto { Data = "token-data" };
                _mockLiveFeedService.Setup(x => x.UpdateMarketDataTokenRequest(It.IsAny<RequestResponseModel>())).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.UpdateMarketDataTokenRequest(dto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockLiveFeedService.Verify(x => x.UpdateMarketDataTokenRequest(It.IsAny<RequestResponseModel>()), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region ReqMultipleSymbolsLiveFeedSnapshotData Test Cases

        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                SetUserDto(0);
                var request = new MultipleSymbolRequestDto { RequestedSymbols = new List<string> { "AAPL" }, RequestedInstance = "inst1" };
                var result = await _controller.ReqMultipleSymbolsLiveFeedSnapshotData(request);
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_ReturnsBadRequest_WhenRequestedSymbolsIsNull()
        {
            try
            {
                SetUserDto(1);
                var request = new MultipleSymbolRequestDto { RequestedSymbols = null, RequestedInstance = "inst1" };
                var result = await _controller.ReqMultipleSymbolsLiveFeedSnapshotData(request);
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Symbol list cannot be empty.", badResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_ReturnsBadRequest_WhenRequestedSymbolsIsEmpty()
        {
            try
            {
                SetUserDto(1);
                var request = new MultipleSymbolRequestDto { RequestedSymbols = new List<string>(), RequestedInstance = "inst1" };
                var result = await _controller.ReqMultipleSymbolsLiveFeedSnapshotData(request);
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Symbol list cannot be empty.", badResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_ReturnsBadRequest_WhenRequestedInstanceIsNullOrWhiteSpace()
        {
            try
            {
                SetUserDto(1);
                var request = new MultipleSymbolRequestDto { RequestedSymbols = new List<string> { "AAPL" }, RequestedInstance = "" };
                var result = await _controller.ReqMultipleSymbolsLiveFeedSnapshotData(request);
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("RequestedInstance is required.", badResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_ReturnsOk_WhenValidRequest()
        {
            try
            {
                SetUserDto(1);
                var request = new MultipleSymbolRequestDto { RequestedSymbols = new List<string> { "AAPL" }, RequestedInstance = "inst1" };
                _mockLiveFeedService.Setup(s => s.ReqMultipleSymbolsLiveFeedSnapshotData(request, 1)).Returns(Task.CompletedTask);
                var result = await _controller.ReqMultipleSymbolsLiveFeedSnapshotData(request);
                Assert.IsType<OkResult>(result);
                _mockLiveFeedService.Verify(s => s.ReqMultipleSymbolsLiveFeedSnapshotData(request, 1), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ReqMultipleSymbolsLiveFeedSnapshotData_ThrowsException_ReturnsBadRequest()
        {
            try
            {
                SetUserDto(1);
                var request = new MultipleSymbolRequestDto { RequestedSymbols = new List<string> { "AAPL" }, RequestedInstance = "inst1" };
                _mockLiveFeedService.Setup(s => s.ReqMultipleSymbolsLiveFeedSnapshotData(request, 1)).ThrowsAsync(new Exception("fail"));
                var result = await _controller.ReqMultipleSymbolsLiveFeedSnapshotData(request);
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                var message = Assert.IsType<string>(badResult.Value);
                Assert.StartsWith(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, message);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

    }
}
