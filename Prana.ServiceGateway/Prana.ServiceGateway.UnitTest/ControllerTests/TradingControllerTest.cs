using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.UnitTest.Commons;
using Prana.ServiceGateway.Controllers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Collections.Generic;
using Prana.ServiceGateway.Models.RequestDto;
using Microsoft.Extensions.Logging;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class TradingControllerTest : BaseControllerTest, IDisposable
    {
        private readonly TradingController _controller;
        private readonly Mock<ILogger<TradingController>> _mockLogger;

        public TradingControllerTest() : base()
        {
            _mockLogger = new Mock<ILogger<TradingController>>();
            _controller = new TradingController(
                 _mockValidationHelper.Object,
                 _mockTokenService.Object,
                 _mockLiveFeedService.Object,
                 _mockSecurityValidationService.Object,
                _mockTradingService.Object,
                _mockLogger.Object
                );
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

        #region SendReplaceOrder Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendReplaceOrder_RetunsOkResult(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                _mockTradingService.Setup(x => x.SendReplaceOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Act  
                var result = await _controller.SendReplaceOrder(tradeOrderDto);

                // Assert  
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendReplaceOrder_RetunsBadResult_WhenDataIsEmpty(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = null
                };

                _mockTradingService.Setup(x => x.SendReplaceOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Act  
                var result = await _controller.SendReplaceOrder(tradeOrderDto);

                // Assert  
                Assert.IsType<BadRequestResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Fact]
        public async Task SendReplaceOrder_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                // Arrange  
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act  
                var result = await _controller.SendReplaceOrder(tradeOrderDto);

                // Assert  
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendReplaceOrder_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.SendReplaceOrderFromTT(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Create a TradeOrderDto object to pass as the argument  
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act & Assert  
                var result = await _controller.SendReplaceOrder(tradeOrderDto);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task SendLiveOrderFromTT_RetunsOkResult(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                _mockTradingService.Setup(x => x.SendLiveOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Act  
                var result = await _controller.SendLiveOrderFromTT(tradeOrderDto);

                // Assert  
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendLiveOrderFromTT_RetunsBadResult_WhenDataIsEmpty(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = string.Empty
                };

                _mockTradingService.Setup(x => x.SendLiveOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Act  
                var result = await _controller.SendLiveOrderFromTT(tradeOrderDto);

                // Assert  
                Assert.IsType<BadRequestResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Fact]
        public async Task SendLiveOrderFromTT_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                // Arrange  
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act  
                var result = await _controller.SendLiveOrderFromTT(tradeOrderDto);

                // Assert  
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendLiveOrderFromTT_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.SendLiveOrderFromTT(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Create a TradeOrderDto object to pass as the argument  
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act & Assert  
                var result = await _controller.SendLiveOrderFromTT(tradeOrderDto);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task SendManualOrderFromTT_RetunsOkResult(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                _mockTradingService.Setup(x => x.SendManualOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Act  
                var result = await _controller.SendManualOrderFromTT(tradeOrderDto);

                // Assert  
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendManualOrderFromTT_RetunsBadResult_WhenDataIsEmpty(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = string.Empty
                };

                _mockTradingService.Setup(x => x.SendManualOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Act  
                var result = await _controller.SendManualOrderFromTT(tradeOrderDto);

                // Assert  
                Assert.IsType<BadRequestResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Fact]
        public async Task SendManualOrderFromTT_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                // Arrange  
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act  
                var result = await _controller.SendManualOrderFromTT(tradeOrderDto);

                // Assert  
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendManualOrderFromTT_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.SendManualOrderFromTT(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act & Assert  
                var result = await _controller.SendManualOrderFromTT(tradeOrderDto);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task SendStageOrderFromTT_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);

                _mockTradingService.Setup(x => x.SendStageOrderFromTT(It.IsAny<RequestResponseModel>()));
                TradingTicketInfo info = new TradingTicketInfo
                {
                    StageTradeHandler = new List<dynamic>()
                };
                var tradeData = new Dictionary<string, object>
        {
            { "Symbol", "testSymbol" },
            { "Broker", "testBroker" }
        };
                info.StageTradeHandler.Add(tradeData);

                // Convert the data to a TradeOrderDto object
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = JsonConvert.SerializeObject(info)
                };

                //Act
                var result = await _controller.SendStageOrderFromTT(tradeOrderDto);

                //Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task SendStageOrderFromTT_RetunsBadResult_WhenDataIsEmpty(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);

                _mockTradingService.Setup(x => x.SendStageOrderFromTT(It.IsAny<RequestResponseModel>()));

                // Create an empty TradeOrderDto object to pass as the argument  
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = null
                };

                // Act  
                var result = await _controller.SendStageOrderFromTT(tradeOrderDto);

                // Assert  
                Assert.IsType<BadRequestResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Fact]
        public async Task SendStageOrderFromTT_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                // Arrange  
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                // Create a TradeOrderDto object to pass as the argument  
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act  
                var result = await _controller.SendStageOrderFromTT(tradeOrderDto);

                // Assert  
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task SendStageOrderFromTT_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.SendStageOrderFromTT(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                TradingTicketInfo info = new TradingTicketInfo
                {
                    StageTradeHandler = new List<dynamic>()
                };
                var tradeData = new Dictionary<string, object>
               {
                   { "Symbol", "testSymbol" },
                   { "Broker", "testBroker" }
               };
                info.StageTradeHandler.Add(tradeData);
                string data = JsonConvert.SerializeObject(info);

                // Convert the string `data` to a `TradeOrderDto` object  
                var tradeOrderDto = new TradeOrderDto
                {
                    Data = data
                };

                // Act & Assert  
                var result = await _controller.SendStageOrderFromTT(tradeOrderDto);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task GetAlgoStrategiesFromBroker_RetunsOkResult(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                var expectedResponse = new RequestResponseModel
                {
                    CompanyUserID = companyUserId,
                    CorrelationId = "250115085033-512",
                    Data = null,
                    ErrorMsg = null,
                    IsDisplayError = false
                };

                _mockTradingService.Setup(x => x.GetAlgoStrategiesFromBroker(It.IsAny<RequestResponseModel>()));

                // Act
                var result = await _controller.GetAlgoStrategiesFromBroker(UnitTestConstants.CONST_REQUEST_DATA);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var actualResponse = Assert.IsType<RequestResponseModel>(okResult.Value);
                Assert.Equal(expectedResponse.CompanyUserID, actualResponse.CompanyUserID);
                Assert.Equal(expectedResponse.Data, actualResponse.Data);
                Assert.Equal(expectedResponse.ErrorMsg, actualResponse.ErrorMsg);
                Assert.Equal(expectedResponse.IsDisplayError, actualResponse.IsDisplayError);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetAlgoStrategiesFromBroker_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GetAlgoStrategiesFromBroker(UnitTestConstants.CONST_REQUEST_DATA);

                //Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetAlgoStrategiesFromBroker_RetunsBadResult_WhenDataIsEmpty(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.GetAlgoStrategiesFromBroker(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GetAlgoStrategiesFromBroker(string.Empty);

                //Assert
                Assert.IsType<BadRequestResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Theory]
        [InlineData(1)]
        public async Task GetAlgoStrategiesFromBroker_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.GetAlgoStrategiesFromBroker(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.GetAlgoStrategiesFromBroker(UnitTestConstants.CONST_REQUEST_DATA);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GET_SymbolAccountWisePosition Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GET_SymbolAccountWisePosition_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.GetSymbolAccountWisePosition(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GET_SymbolAccountWisePosition(UnitTestConstants.SymbolAccountPositionRequestDto);

                //Assert
                Assert.IsType<OkResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Fact]
        public async Task GET_SymbolAccountWisePosition_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GET_SymbolAccountWisePosition(UnitTestConstants.SymbolAccountPositionRequestDto);

                //Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_SymbolAccountWisePosition_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.GetSymbolAccountWisePosition(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.GET_SymbolAccountWisePosition(UnitTestConstants.SymbolAccountPositionRequestDto);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task GetCustomAllocationDetails_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                GenericDto customData = new GenericDto();
                customData.Data = UnitTestConstants.CONST_REQUEST_DATA;

                _mockTradingService.Setup(x => x.GetCustomAllocationDetails(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GetCustomAllocationDetails(customData);

                //Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Fact]
        public async Task GetCustomAllocationDetails_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                GenericDto customData = new GenericDto();
                customData.Data = UnitTestConstants.CONST_REQUEST_DATA;

                //Act
                var result = await _controller.GetCustomAllocationDetails(customData);

                //Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetCustomAllocationDetails_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                GenericDto customData = new GenericDto();
                customData.Data = UnitTestConstants.CONST_REQUEST_DATA;

                _mockTradingService.Setup(x => x.GetCustomAllocationDetails(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.GetCustomAllocationDetails(customData);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task GetSavedCustomAllocationDetails_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);

                _mockTradingService.Setup(x => x.GetSavedCustomAllocationDetails(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GetSavedCustomAllocationDetails(UnitTestConstants.CONST_REQUEST_DATA, UnitTestConstants.CONST_ID);

                //Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Fact]
        public async Task GetSavedCustomAllocationDetails_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GetSavedCustomAllocationDetails(UnitTestConstants.CONST_REQUEST_DATA, UnitTestConstants.CONST_ID);

                //Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task GetSavedCustomAllocationDetails_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.GetSavedCustomAllocationDetails(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.GetSavedCustomAllocationDetails(UnitTestConstants.CONST_REQUEST_DATA, UnitTestConstants.CONST_ID);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ValidateSymbol Test Cases
        [Fact]
        public async Task ValidateSymbolUnified_ReturnsOkResult()
        {
            try
            {
                // Arrange  
                SetUserDto(1);
                var validateSymbolUnifiedRequestDto = new ValidateSymbolUnifiedRequestDto
                {
                    Symbol = UnitTestConstants.ValidateSymbolRequestDto.Symbol,
                    RequestID = UnitTestConstants.ValidateSymbolRequestDto.RequestID
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(validateSymbolUnifiedRequestDto);

                // Assert  
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.Is<int>(id => id == 1),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<string>(),
                        It.IsAny<int>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateSymbol_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var validateSymbolUnifiedRequestDto = new ValidateSymbolUnifiedRequestDto
                {
                    Symbol = UnitTestConstants.ValidateSymbolRequestDto.Symbol,
                    RequestID = UnitTestConstants.ValidateSymbolRequestDto.RequestID
                };

                // Act
                var result = await _controller.ValidateSymbolUnified(validateSymbolUnifiedRequestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);

                _mockSecurityValidationService.Verify(x => x.ValidateSymbolUnifiedAsync(It.IsAny<List<string>>(),0, CorrelationId, string.Empty, false, string.Empty,0), Times.Never);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateSymbol_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange  
                SetUserDto(1);
                var requestDto = new BaseRequestDto();

                _mockSecurityValidationService.Setup(x => x.ValidateSymbolUnifiedAsync(
                    It.IsAny<List<string>>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    false,
                    string.Empty,
                    0
                ));

                var validateSymbolUnifiedRequestDto = new ValidateSymbolUnifiedRequestDto
                {
                    Symbol = null,
                    RequestID = UnitTestConstants.ValidateSymbolRequestDto.RequestID
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(validateSymbolUnifiedRequestDto);

                // Assert  
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_INPUT, badRequestResult.Value);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.IsAny<int>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        false,
                        string.Empty,
                        0
                    ),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region SymbolSearch Test Cases
        [Fact]
        public async Task SymbolSearch_ReturnsOkResult()
        {
            try
            {
                SetUserDto(1);
                // Act
                var requestResponseObj = new RequestResponseModel(0, UnitTestConstants.CONST_MOCK_SYMBOL, CorrelationId);
                var result = await _controller.SymbolSearch(UnitTestConstants.SymbolSearchReqDto);

                //Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SymbolSearch_ThrowsException()
        {
            try
            {
                SetUserDto(1);
                _mockSecurityValidationService.Setup(x => x.SymbolSearch(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                var result = await _controller.SymbolSearch(UnitTestConstants.SymbolSearchReqDto);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region BookAsSwapReplace Test Cases
        [Theory]
        [InlineData(1, "TestData")]
        public async Task BookAsSwapReplace_ReturnsOkResult_WhenValidInputsAreProvided(int companyUserId, string data)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockTradingService.Setup(x => x.BookAsSwapReplace(It.IsAny<RequestResponseModel>())).Returns(Task.CompletedTask);

                // Act
                var dto = new TradeOrderDto { Data = data };
                var result = await _controller.BookAsSwapReplace(dto);

                // Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task BookAsSwapReplace_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                // Act
                var dto = new TradeOrderDto { Data = "TestData" };
                var result = await _controller.BookAsSwapReplace(dto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(expectedErrorMessage, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task BookAsSwapReplace_ReturnsBadRequest_WhenDataIsNullOrEmpty()
        {
            try
            {
                // Arrange
                int companyUserId = 1;
                SetUserDto(companyUserId);

                // Act
                var dto = new TradeOrderDto { Data = "" };
                var result = await _controller.BookAsSwapReplace(dto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestResult>(result);
                Assert.Equal(400, badRequestResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1, "TestData")]
        public async Task BookAsSwapReplace_ReturnsBadRequest_WhenServiceThrowsException(int companyUserId, string data)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockTradingService
                    .Setup(x => x.BookAsSwapReplace(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act
                var dto = new TradeOrderDto { Data = data };
                var result = await _controller.BookAsSwapReplace(dto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region HotKey Test Cases
        [Fact]
        public async Task GET_CompanyUserHotKeyPreferences_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            // Arrange
            //Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            var requestDto = new BaseRequestDto();

            // Act
            var result = await _controller.GET_CompanyUserHotKeyPreferences(requestDto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequest.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task GET_CompanyUserHotKeyPreferences_CallsServiceAndReturnsOk(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            var requestDto = new BaseRequestDto();
                    // Act
                    var result = await _controller.GET_CompanyUserHotKeyPreferences(requestDto);

            // Assert
            _mockTradingService.Verify(s => s.GetCompanyUserHotKeyPreferences(
                It.Is<RequestResponseModel>(r =>
                    r.CompanyUserID == companyUserId &&
                    JsonConvert.DeserializeObject<BaseRequestDto>(r.Data).CompanyUserID == companyUserId
                )
            ), Times.Once);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Save_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            var dto = new SaveHotKeyPreferencesRequestDto();

            // Act
            var result = await _controller.Save_CompanyUserHotKeyPreferencesDetails(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequest.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Save_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenDtoIsNull(int companyUserId)
        {
            SetUserDto(companyUserId);
            // Act
            var result = await _controller.Save_CompanyUserHotKeyPreferencesDetails(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequest.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task Save_CompanyUserHotKeyPreferencesDetails_CallsServiceAndReturnsOk(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            var dto = new SaveHotKeyPreferencesRequestDto();

            // Act
            var result = await _controller.Save_CompanyUserHotKeyPreferencesDetails(dto);

            // Assert
            _mockTradingService.Verify(s => s.SaveCompanyUserHotKeyPreferencesDetails(
                It.Is<RequestResponseModel>(r =>
                    r.CompanyUserID == companyUserId &&
                    JsonConvert.DeserializeObject<SaveHotKeyPreferencesRequestDto>(r.Data) != null
                )
            ), Times.Once);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Save_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_OnException()
        {
            // Arrange
            var dto = new SaveHotKeyPreferencesRequestDto();
            _mockTradingService
                .Setup(s => s.SaveCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()))
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act
            var result = await _controller.Save_CompanyUserHotKeyPreferencesDetails(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("An error occurred", badRequest.Value.ToString());
        }
        #endregion

        #region ValidateOptionSymbol Test Cases
        [Fact]
        public async Task ValidateOptionSymbol_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var validateSymbolUnifiedRequestDto = new ValidateSymbolUnifiedRequestDto
                {
                    Symbol = UnitTestConstants.ValidateSymbolRequestDto.Symbol,
                    RequestID = UnitTestConstants.ValidateSymbolRequestDto.RequestID
                };

                // Act
                var result = await _controller.ValidateSymbolUnified(validateSymbolUnifiedRequestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.Is<int>(id => id == 1),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<string>(),
                        0
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateOptionSymbol_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange  
                SetUserDto(0);
                var validateSymbolUnifiedRequestDto = new ValidateSymbolUnifiedRequestDto
                {
                    Symbol = UnitTestConstants.ValidateSymbolRequestDto.Symbol,
                    RequestID = UnitTestConstants.ValidateSymbolRequestDto.RequestID
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(validateSymbolUnifiedRequestDto);

                // Assert  
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);

                _mockSecurityValidationService.Verify(x => x.ValidateSymbolUnifiedAsync(
                    It.IsAny<List<string>>(),
                    It.Is<int>(id => id == 0),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(), 0),
                    Times.Never);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SMSaveNewSymbol Test Cases
        [Fact]
        public void SMSaveNewSymbol_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new SmSymbolDto();

                // Act
                var result = _controller.SMSaveNewSymbol(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockSecurityValidationService.Verify(
                    x => x.SMSaveNewSymbol(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public void SMSaveNewSymbol_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new SmSymbolDto();

                // Act
                var result = _controller.SMSaveNewSymbol(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockSecurityValidationService.Verify(
                    x => x.SMSaveNewSymbol(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CreatePopUpText Test Cases
        [Fact]
        public async Task CreatePopUpText_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new TradeOrderDto();

                // Act
                var result = await _controller.CreatePopUpText(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.CreatePopUpText(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task CreatePopUpText_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new TradeOrderDto();

                // Act
                var result = await _controller.CreatePopUpText(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.CreatePopUpText(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CreateOptionSymbol Test Cases
        [Fact]
        public async Task CreateOptionSymbol_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new CreateOptionSymbolRequestDto();

                // Act
                var result = await _controller.CreateOptionSymbol(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.CreateOptionSymbol(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task CreateOptionSymbol_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new CreateOptionSymbolRequestDto();

                // Act
                var result = await _controller.CreateOptionSymbol(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.CreateOptionSymbol(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region UnSubscribeSymbolCompressionFeed Test Cases
        [Fact]
        public async Task UnSubscribeSymbolCompressionFeed_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new UnSubscribeSymbolComprFeedRequestDto();

                // Act
                var result = await _controller.UnSubscribeSymbolCompressionFeed(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.UnsubscribeSymbolCompressionFeed(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task UnSubscribeSymbolCompressionFeed_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new UnSubscribeSymbolComprFeedRequestDto();

                // Act
                var result = await _controller.UnSubscribeSymbolCompressionFeed(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.UnsubscribeSymbolCompressionFeed(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SMSymbolSearch Test Cases
        [Fact]
        public async Task SMSymbolSearch_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new GenericDto();

                // Act
                var result = await _controller.SMSymbolSearch(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockSecurityValidationService.Verify(
                    x => x.SMSymbolSearch(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SMSymbolSearch_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new GenericDto();

                // Act
                var result = await _controller.SMSymbolSearch(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockSecurityValidationService.Verify(
                    x => x.SMSymbolSearch(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetSymbolWiseShortLocateOrders Test Cases
        [Fact]
        public async Task GetSymbolWiseShortLocateOrders_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new GenericDto();

                // Act
                var result = await _controller.GetSymbolWiseShortLocateOrders(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.GetSymbolWiseShortLocateOrders(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetSymbolWiseShortLocateOrders_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new GenericDto();

                // Act
                var result = await _controller.GetSymbolWiseShortLocateOrders(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.GetSymbolWiseShortLocateOrders(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region DetermineSecurityBorrowType Test Cases
        [Fact]
        public async Task DetermineSecurityBorrowType_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = new GenericDto();

                // Act
                var result = await _controller.DetermineSecurityBorrowType(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.DetermineSecurityBorrowType(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task DetermineSecurityBorrowType_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = new GenericDto();

                // Act
                var result = await _controller.DetermineSecurityBorrowType(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.DetermineSecurityBorrowType(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetSMData Test Cases
        [Fact]
        public async Task GetSMData_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = "Test";

                // Act
                var result = await _controller.GetSMData(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.GetSMData(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetSMData_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = "Test";

                // Act
                var result = await _controller.GetSMData(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.GetSMData(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetBrokerConnectionAndVenuesData Test Cases
        [Fact]
        public async Task GetBrokerConnectionAndVenuesData_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var data = "Test";

                // Act
                var result = await _controller.GetBrokerConnectionAndVenuesData(data);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.GetBrokerConnectionAndVenuesData(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetBrokerConnectionAndVenuesData_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var data = "Test";

                // Act
                var result = await _controller.GetBrokerConnectionAndVenuesData(data);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.GetBrokerConnectionAndVenuesData(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task ValidateMultipleSymbols_ReturnsOk_WhenCalledWithValidSymbols(int companyUserId)
        {
            try
            {
                // Arrange  
                SetUserDto(companyUserId);
                var request = new ValidateSymbolUnifiedRequestDto
                {
                    Symbols = new List<string> { "AAPL", "GOOG", "MSFT" },
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(request);

                // Assert  
                Assert.IsType<OkResult>(result);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.Is<int>(id => id == 1),
                        It.IsAny<string>(),
                        string.Empty, // Explicitly pass the default value for optional argument `requestId`  
                        false,        // Explicitly pass the default value for optional argument `isOptionSymbol`  
                        string.Empty,  // Explicitly pass the default value for optional argument `underLyingSymbol`
                        0
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateMultipleSymbols_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var request = new ValidateSymbolUnifiedRequestDto
                {
                    Symbols = new List<string> { "AAPL", "GOOG", "MSFT" },
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(request);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.Is<int>(id => id == 1),
                        It.IsAny<string>(),
                        string.Empty, // Explicitly pass the default value for optional argument `requestId`  
                        false,        // Explicitly pass the default value for optional argument `isOptionSymbol`  
                        string.Empty,  // Explicitly pass the default value for optional argument `underLyingSymbol`
                         0
                    ),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateMultipleSymbols_ReturnsBadRequest_WhenSymbolsListIsNull()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var request = new ValidateSymbolUnifiedRequestDto
                {
                    Symbols = null,
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(request);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_INPUT, badRequestResult.Value);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.Is<int>(id => id == 1),
                        It.IsAny<string>(),
                        string.Empty, // Explicitly pass the default value for optional argument `requestId`  
                        false,        // Explicitly pass the default value for optional argument `isOptionSymbol`  
                        string.Empty, 0  // Explicitly pass the default value for optional argument `underLyingSymbol`  
                    ),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateMultipleSymbols_ReturnsBadRequest_WhenSymbolsListIsEmpty()
        {
            try
            {
                // Arrange
                SetUserDto(1);

                var request = new ValidateSymbolUnifiedRequestDto
                {
                    Symbols = new List<string>(),
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(request); ;

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_INPUT, badRequestResult.Value);
                _mockSecurityValidationService.Verify(
                    x => x.ValidateSymbolUnifiedAsync(
                        It.IsAny<List<string>>(),
                        It.Is<int>(id => id == 1),
                        It.IsAny<string>(),
                        string.Empty, // Explicitly pass the default value for optional argument `requestId`  
                        false,        // Explicitly pass the default value for optional argument `isOptionSymbol`  
                        string.Empty, 0  // Explicitly pass the default value for optional argument `underLyingSymbol`  
                    ),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task ValidateMultipleSymbols_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);

                _mockSecurityValidationService
                   .Setup(x => x.ValidateSymbolUnifiedAsync(It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<string>(), string.Empty, false, string.Empty, 0))  
                    .ThrowsAsync(new Exception("Simulated exception"));

                var request = new ValidateSymbolUnifiedRequestDto
                {
                    Symbols = new List<string> { "AAPL", "GOOG", "MSFT" },
                };

                // Act  
                var result = await _controller.ValidateSymbolUnified(request);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString()); 
                _mockSecurityValidationService.Verify(
                   x => x.ValidateSymbolUnifiedAsync(
                       It.IsAny<List<string>>(),
                       It.IsAny<int>(),
                       It.IsAny<string>(),
                       string.Empty, // Explicitly pass the default value for optional argument `requestId`.  
                       false,        // Explicitly pass the default value for optional argument `isOptionSymbol`.  
                       string.Empty, 0  // Explicitly pass the default value for optional argument `underLyingSymbol`.  
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

        #region Update_CompanyUserHotKeyPreferences Test Cases

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferences_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new UserHotKeyPreferencesRequestDto
                {
                    HotKeyPreferenceElements = "Test",
                    EnableBookMarkIcon = true
                };

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferences(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferences(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferences_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new UserHotKeyPreferencesRequestDto
                {
                    HotKeyPreferenceElements = "Test",
                    EnableBookMarkIcon = true
                };

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferences(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferences(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferences_ReturnsBadRequest_WhenRequestDtoIsNull()
        {
            try
            {
                // Arrange
                SetUserDto(1);

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferences(null);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferences(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferences_ReturnsBadRequest_WhenHotKeyPreferenceElementsIsEmpty()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new UserHotKeyPreferencesRequestDto
                {
                    HotKeyPreferenceElements = string.Empty,
                    EnableBookMarkIcon = true
                };

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferences(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferences(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferences_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new UserHotKeyPreferencesRequestDto
                {
                    HotKeyPreferenceElements = "Test",
                    EnableBookMarkIcon = true
                };

                _mockTradingService
                    .Setup(x => x.UpdateCompanyUserHotKeyPreferences(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferences(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferences(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region GET_CompanyUserHotKeyPreferencesDetails Test Cases

        [Fact]
        public async Task GET_CompanyUserHotKeyPreferencesDetails_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new BaseRequestDto();

                // Act
                var result = await _controller.GET_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.GetCompanyUserHotKeyPreferencesDetails(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GET_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new BaseRequestDto()
                {
                    CompanyUserID = 0
                };

                // Act
                var result = await _controller.GET_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.GetCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GET_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new BaseRequestDto();

                _mockTradingService
                    .Setup(x => x.GetCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.GET_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.GetCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region Update_CompanyUserHotKeyPreferencesDetails Test Cases

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferencesDetails_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new SaveHotKeyPreferencesRequestDto();

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferencesDetails(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new SaveHotKeyPreferencesRequestDto();

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenRequestDtoIsNull()
        {
            try
            {
                // Arrange
                SetUserDto(1);

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferencesDetails(null);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Update_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new SaveHotKeyPreferencesRequestDto();

                _mockTradingService
                    .Setup(x => x.UpdateCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.Update_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.UpdateCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region Delete_CompanyUserHotKeyPreferencesDetails Test Cases

        [Fact]
        public async Task Delete_CompanyUserHotKeyPreferencesDetails_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new DeleteHotKeyRequestDto();

                // Act
                var result = await _controller.Delete_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.DeleteCompanyUserHotKeyPreferencesDetails(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Delete_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new DeleteHotKeyRequestDto();

                // Act
                var result = await _controller.Delete_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.DeleteCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Delete_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenRequestDtoIsNull()
        {
            try
            {
                // Arrange
                SetUserDto(1);

                // Act
                var result = await _controller.Delete_CompanyUserHotKeyPreferencesDetails(null);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.DeleteCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task Delete_CompanyUserHotKeyPreferencesDetails_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new DeleteHotKeyRequestDto();

                _mockTradingService
                    .Setup(x => x.DeleteCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.Delete_CompanyUserHotKeyPreferencesDetails(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.DeleteCompanyUserHotKeyPreferencesDetails(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region SendPstOrders Test Cases

        [Fact]
        public async Task SendPstOrders_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = "Test Data",
                    CorrelationId = "12345"
                };

                // Act
                var result = await _controller.SendPstOrders(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.SendPstOrders(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SendPstOrders_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new RequestDto<string>();

                // Act
                var result = await _controller.SendPstOrders(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.SendPstOrders(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SendPstOrders_ReturnsBadRequest_WhenHotKeyPreferenceElementsIsEmpty()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = string.Empty
                };

                // Act
                var result = await _controller.SendPstOrders(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.SendPstOrders(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SendPstOrders_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = "Test Data",
                    CorrelationId = "12345"
                };

                _mockTradingService
                    .Setup(x => x.SendPstOrders(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.SendPstOrders(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.SendPstOrders(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region SendOrdersToMarket Test Cases

        [Fact]
        public async Task SendOrdersToMarket_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = "Test Data",
                    CorrelationId = "12345"
                };

                // Act
                var result = await _controller.SendOrdersToMarket(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.SendOrdersToMarket(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SendOrdersToMarket_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new RequestDto<string>();

                // Act
                var result = await _controller.SendOrdersToMarket(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.SendOrdersToMarket(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SendOrdersToMarket_ReturnsBadRequest_WhenHotKeyPreferenceElementsIsEmpty()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = string.Empty
                };

                // Act
                var result = await _controller.SendOrdersToMarket(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.SendOrdersToMarket(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task SendOrdersToMarket_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = "Test Data",
                    CorrelationId = "12345"
                };

                _mockTradingService
                    .Setup(x => x.SendOrdersToMarket(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.SendOrdersToMarket(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.SendOrdersToMarket(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region CreatePstAllocatonPreference Test Cases

        [Fact]
        public async Task CreatePstAllocatonPreference_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = "Test Data"
                };

                // Act
                var result = await _controller.CreatePstAllocatonPreference(requestDto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockTradingService.Verify(
                    x => x.CreatePstAllocatonPreference(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task CreatePstAllocatonPreference_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new RequestDto<string>();

                // Act
                var result = await _controller.CreatePstAllocatonPreference(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.CreatePstAllocatonPreference(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task CreatePstAllocatonPreference_ReturnsBadRequest_WhenHotKeyPreferenceElementsIsEmpty()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = string.Empty
                };

                // Act
                var result = await _controller.CreatePstAllocatonPreference(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal("Invalid request paramters", badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.CreatePstAllocatonPreference(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task CreatePstAllocatonPreference_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new RequestDto<string>
                {
                    Data = "Test Data"
                };

                _mockTradingService
                    .Setup(x => x.CreatePstAllocatonPreference(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.CreatePstAllocatonPreference(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.CreatePstAllocatonPreference(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region GetTradeAttributesLabels Test Cases

        [Fact]
        public async Task GetTradeAttributesLabels_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new TradeAttributesDto()
                {
                    RequestId = "12345",
                    CompanyUserID = 1
                };

                // Act
                var result = await _controller.GetTradeAttributesLabels(requestDto);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                _mockTradingService.Verify(
                    x => x.ProduceTradeAttributeLabelsEvent(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetTradeAttributesLabels_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                var requestDto = new TradeAttributesDto()
                {
                    RequestId = "12345",
                    CompanyUserID = 0,
                };

                // Act
                var result = await _controller.GetTradeAttributesLabels(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
                _mockTradingService.Verify(
                    x => x.ProduceTradeAttributeLabelsEvent(It.IsAny<RequestResponseModel>()),
                    Times.Never
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetTradeAttributesLabels_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {

                // Arrange
                SetUserDto(1);
                var requestDto = new TradeAttributesDto()
                {
                    RequestId = "12345",
                    CompanyUserID = 1
                };

                _mockTradingService
                    .Setup(x => x.ProduceTradeAttributeLabelsEvent(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.GetTradeAttributesLabels(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.ProduceTradeAttributeLabelsEvent(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region GetTradeAttributesValues Test Cases

        [Fact]
        public async Task GetTradeAttributesValues_ReturnsOk_WhenCalledWithValidData()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new TradeAttributesDto()
                {
                    RequestId = "12345",
                    CompanyUserID = 1
                };

                // Act
                var result = await _controller.GetTradeAttributesValues(requestDto);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                _mockTradingService.Verify(
                    x => x.ProduceTradeAttributeValuesEvent(
                        It.IsAny<RequestResponseModel>()
                    ),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetTradeAttributesValues_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var requestDto = new TradeAttributesDto()
                {
                    RequestId = "12345",
                    CompanyUserID = 1
                };

                _mockTradingService
                    .Setup(x => x.ProduceTradeAttributeValuesEvent(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Simulated exception"));

                // Act
                var result = await _controller.GetTradeAttributesValues(requestDto);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
                _mockTradingService.Verify(
                    x => x.ProduceTradeAttributeValuesEvent(It.IsAny<RequestResponseModel>()),
                    Times.Once
                );
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region PstAccountsNav Test Cases

        [Fact]
        public async Task PstAccountsNav_ReturnsOk_WhenCalledWithValidData()
        {
            // Arrange
            SetUserDto(1);
            var requestDto = new RequestDto<List<PstAccountNavsRequestDto>>
            {
                Data = new List<PstAccountNavsRequestDto>
                {
                    new PstAccountNavsRequestDto
                    {
                        UnderlyingSymbol = "AAPL",
                        Symbol = "AAPL",
                        SymbolAUECID = "12345",
                        UserId = 1
                    }
                },
                CorrelationId = "12345"
            };

            // Act
            var result = await _controller.PstAccountsNav(requestDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockTradingService.Verify(
                x => x.GetPstAccountNav(
                    It.IsAny<RequestResponseModel>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task PstAccountsNav_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            SetUserDto(1);
            var requestDto = new RequestDto<List<PstAccountNavsRequestDto>>
            {
                Data = new List<PstAccountNavsRequestDto>
                {
                    new PstAccountNavsRequestDto
                    {
                        UnderlyingSymbol = "AAPL",
                        Symbol = "AAPL",
                        SymbolAUECID = "12345",
                        UserId = 1
                    }
                },
                CorrelationId = "12345"
            };

            _mockTradingService
                .Setup(x => x.GetPstAccountNav(It.IsAny<RequestResponseModel>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.PstAccountsNav(requestDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
            _mockTradingService.Verify(
                x => x.GetPstAccountNav(It.IsAny<RequestResponseModel>()),
                Times.Once
            );
        }


        #endregion

        #region DetermineMultipleSecurityBorrowType Test Cases
        [Theory]
        [InlineData(1)]
        public async Task DetermineMultipleSecurityBorrowType_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            GenericDto data = new GenericDto { Data = "Some test data" };

            _mockTradingService.Setup(x => x.DetermineMultipleSecurityBorrowType(It.IsAny<RequestResponseModel>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DetermineMultipleSecurityBorrowType(data);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockTradingService.Verify(x => x.DetermineMultipleSecurityBorrowType(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task DetermineMultipleSecurityBorrowType_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            GenericDto data = new GenericDto { Data = "Some test data" };

            // Act
            var result = await _controller.DetermineMultipleSecurityBorrowType(data);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockTradingService.Verify(x => x.DetermineMultipleSecurityBorrowType(It.IsAny<RequestResponseModel>()), Times.Never);
        }
        #endregion

        #region ValidateSymbolUnified Test Cases (Additional Coverage)

        [Fact]
        public async Task ValidateSymbolUnified_ReturnsOk_WhenCalledWithOptionSymbol()
        {
            // Arrange
            SetUserDto(1);
            var request = new ValidateSymbolUnifiedRequestDto
            {
                OptionSymbol = "MSFT_CALL_20240920_150", // Provide an OptionSymbol
                RequestID = "optionReq1",
                UnderLyingSymbol = "MSFT" // Ensure UnderLyingSymbol is provided for this path
            };

            _mockSecurityValidationService.Setup(x => x.ValidateSymbolUnifiedAsync(
                It.IsAny<List<string>>(),
                It.Is<int>(id => id == 1),
                It.IsAny<string>(),
                It.Is<string>(reqId => reqId == request.RequestID),
                It.Is<bool>(isOption => isOption == true), // Expect true for isOptionSymbol
                It.Is<string>(underlying => underlying == request.UnderLyingSymbol), // Expect underlying symbol
                It.IsAny<int>()
            )).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ValidateSymbolUnified(request);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockSecurityValidationService.Verify(
                x => x.ValidateSymbolUnifiedAsync(
                    It.Is<List<string>>(list => list.Contains(request.OptionSymbol)),
                    It.Is<int>(id => id == 1),
                    It.IsAny<string>(),
                    It.Is<string>(reqId => reqId == request.RequestID),
                    It.Is<bool>(isOption => isOption == true),
                    It.Is<string>(underlying => underlying == request.UnderLyingSymbol),
                    It.IsAny<int>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task ValidateSymbolUnified_ReturnsBadRequest_WhenNoSymbolProvided()
        {
            // Arrange
            SetUserDto(1);
            var request = new ValidateSymbolUnifiedRequestDto
            {
                Symbol = null,
                OptionSymbol = null,
                Symbols = null // Ensure all three are null or empty to hit the 'else' block
            };

            // Act
            var result = await _controller.ValidateSymbolUnified(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(MessageConstants.MSG_INVALID_INPUT, badRequestResult.Value);
            _mockSecurityValidationService.Verify(
                x => x.ValidateSymbolUnifiedAsync(
                    It.IsAny<List<string>>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()
                ),
                Times.Never
            );
        }

        [Fact]
        public async Task ValidateSymbolUnified_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            SetUserDto(1);
            var request = new ValidateSymbolUnifiedRequestDto
            {
                Symbol = "AAPL", // Provide a valid symbol to enter the try block
                RequestID = "testReq"
            };

            _mockSecurityValidationService.Setup(x => x.ValidateSymbolUnifiedAsync(
                It.IsAny<List<string>>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<int>()
            )).ThrowsAsync(new Exception("Simulated service error"));

            // Act
            var result = await _controller.ValidateSymbolUnified(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, badRequestResult.Value.ToString());
            _mockSecurityValidationService.Verify(
                x => x.ValidateSymbolUnifiedAsync(
                    It.IsAny<List<string>>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()
                ),
                Times.Once // Verify that the service method was called once before throwing
            );
        }
        #endregion
    }
}
