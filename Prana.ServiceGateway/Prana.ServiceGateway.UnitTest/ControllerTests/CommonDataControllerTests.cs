using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Models;
using System;
using System.IO.Compression;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using System.Text;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class CommonDataControllerTests : BaseControllerTest, IDisposable
    {

        private readonly CommonDataController _controller;

        public CommonDataControllerTests() : base()
        {
            _controller = new CommonDataController(
                _mockValidationHelper.Object,
                _mockTokenService.Object,
                _mockCommonDataService.Object,
                GetMockLogger<CommonDataController>());
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
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

        private static string Decompress(byte[] gzip)
        {
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        #region GetCompanyTransferTradeRules Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetCompanyTransferTradeRules_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockCommonDataService.Setup(x => x.GetCompanyTransferTradeRules(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GetCompanyTransferTradeRules();

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
        public async Task GetCompanyTransferTradeRules_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GetCompanyTransferTradeRules();

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
        public async Task GetCompanyTransferTradeRules_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockCommonDataService.Setup(x => x.GetCompanyTransferTradeRules(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));
                // Act & Assert
                var result = await _controller.GetCompanyTransferTradeRules();
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetTradingTicketData Test Cases 
        [Theory]
        [InlineData(1)]
        public async Task GetTradingTicketData_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockCommonDataService.Setup(x => x.GetTradingTicketAllData(It.IsAny<RequestResponseModel>()));
                //Act
                var result = await _controller.GetTradingTicketData("dummyId");

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
        public async Task GetTradingTicketData_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GetTradingTicketData("dummyId");

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
        public async Task GetTradingTicketData_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockCommonDataService.Setup(x => x.GetTradingTicketAllData(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));
                // Act & Assert
                var result = await _controller.GetTradingTicketData("dummyId");
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion
        
        #region GetTradingTicketCacheData Test Cases 
        [Fact]
        public void GetTradingTicketCacheData_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetTradingTicketCacheData();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
        }

        [Fact]
        public void GetTradingTicketCacheData_ReturnsOk_WhenDataIsFetchedSuccessfully()
        {
            // Arrange
            int companyUserId = 10;
            SetUserDto(companyUserId);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetTradingTicketCacheData();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var decompressedData = Decompress((byte[])okResult.Value);
            Assert.Contains("TradingTicketData", decompressedData);
            Assert.Contains("BrokerData", decompressedData);
        }

        [Fact]
        public void GetTradingTicketCacheData_ReturnsBadRequest_WhenUserDtoIsMissing()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetTradingTicketCacheData();

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("Error fetching trading ticket cache data: Object reference not set to an instance of an object.", badRequestResult.Value);
        }

        [Fact]
        public void GetTradingTicketCacheData_ReturnsOkWithEmptyData_WhenNoDataIsAvailable()
        {
            // Arrange
            int companyUserId = 10;
            SetUserDto(companyUserId);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetTradingTicketCacheData();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains(string.Empty, okResult.Value.ToString());
        }
        #endregion

        #region GetBrokerConnectionCacheData Test Cases

        [Fact]
        public void GetBrokerConnectionCacheData_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetBrokerConnectionCacheData();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
        }

        [Fact]
        public void GetBrokerConnectionCacheData_ReturnsOk_WhenDataIsFetchedSuccessfully()
        {
            // Arrange
            int companyUserId = 10;
            SetUserDto(companyUserId);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetBrokerConnectionCacheData();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var decompressedData = Decompress((byte[])okResult.Value);
            Assert.Contains("BrokerData", decompressedData);
        }

        [Fact]
        public void GetBrokerConnectionCacheData_ReturnsError_WhenUserDtoIsMissing()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetBrokerConnectionCacheData();

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Contains("Error fetching broker connection status cache data", errorResult.Value.ToString());
            Assert.Equal(500, errorResult.StatusCode);
        }

        [Fact]
        public void GetBrokerConnectionCacheData_ReturnsOkWithEmptyData_WhenNoBrokerDataAvailable()
        {
            // Arrange
            int companyUserId = 10;
            SetUserDto(companyUserId);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _controller.GetBrokerConnectionCacheData();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var decompressedData = Decompress((byte[])okResult.Value);
            Assert.Contains("BrokerData", decompressedData);
        }

        #endregion
    }
}
