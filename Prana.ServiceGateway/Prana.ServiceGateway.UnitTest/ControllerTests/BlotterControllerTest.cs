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
using Prana.ServiceGateway.Models.RequestDto;
using System.Collections.Generic;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class BlotterControllerTest : BaseControllerTest, IDisposable
    {
        private readonly BlotterController _blotterController;
        public BlotterControllerTest() : base()
        {
            _blotterController = new BlotterController(
                 _mockValidationHelper.Object,
                 _mockTokenService.Object,
                _mockBlotterService.Object);
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
                _blotterController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #region GetBlotterData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetBlotterData_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                string expected = ServiceGatewayTestCommons.blotterData;
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.GetBlotterData(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.GetBlotterData("dummyId");

                //Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
                _mockBlotterService.Verify(x => x.GetBlotterData(It.IsAny<RequestResponseModel>()), Times.Once);
                _mockTokenService.Verify(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>()), Times.Exactly(1));
            }
            catch (Exception ex)
            {

                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex); ;
            }
        }

        [Fact]
        public async Task GetBlotterData_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.GetBlotterData("dummyId");

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
        public async Task GetBlotterData_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            // Arrange
            try
            {
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.GetBlotterData(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act
                var result = await _blotterController.GetBlotterData("dummyId");

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, errorMessage);
            }
            catch (Exception ex)
            {

                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetBlotterData_ThrowsException_WhenTokenInvalidOrEmpty()
        {
            try
            {
                int companyUserId = 0;
                SetUserDto(companyUserId);
                //  Assert
                var result = await _blotterController.GetBlotterData("dummyId");
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task GetPstData_ReturnsOkResult_WhenValidRequest(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.GetPstData(It.IsAny<RequestResponseModel>()));

                // Act
                var result = await _blotterController.GetPstData(100, "AAPL", "BUY");

                // Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);

                _mockBlotterService.Verify(x => x.GetPstData(It.IsAny<RequestResponseModel>()), Times.Once);
                _mockTokenService.Verify(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>()), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetPstData_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                string expectedMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;

                // Act
                var result = await _blotterController.GetPstData(100, "AAPL", "BUY");

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                var actualMessage = Assert.IsType<string>(badRequest.Value);
                Assert.Equal(expectedMessage, actualMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task GetPstData_ReturnsBadRequest_WhenTokenInvalidOrMissing()
        {
            try
            {
                // Arrange
                SetUserDto(0);

                // Act
                var result = await _blotterController.GetPstData(100, "AAPL", "BUY");

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequest.Value);
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
        public async Task CancelAllSubOrders_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.CancelAllSubOrders(It.IsAny<RequestResponseModel>()));
                //Act
                var result = await _blotterController.CancelAllSubOrders(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task CancelAllSubOrders_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.CancelAllSubOrders(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task CancelAllSubOrders_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.CancelAllSubOrders(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _blotterController.CancelAllSubOrders(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);

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
        public async Task RolloverAllSubOrders_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RolloverAllSubOrders(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.RolloverAllSubOrders(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task RolloverAllSubOrders_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.RolloverAllSubOrders(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task RolloverAllSubOrders_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RolloverAllSubOrders(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _blotterController.RolloverAllSubOrders(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);

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
        public async Task RemoveOrders_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RemoveOrders(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.RemoveOrders(UnitTestConstants.CONST_ORDER_ID);

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
        public async Task RemoveOrders_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.RemoveOrders(UnitTestConstants.CONST_ORDER_ID);

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
        public async Task RemoveOrders_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);

                _mockBlotterService.Setup(x => x.RemoveOrders(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result =  await  _blotterController.RemoveOrders(UnitTestConstants.CONST_ORDER_ID);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task FreezeOrdersInPendingComplianceUI_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);

                _mockBlotterService.Setup(x => x.FreezeOrdersInPendingComplianceUI(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.FreezeOrdersInPendingComplianceUI(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task FreezeOrdersInPendingComplianceUI_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.FreezeOrdersInPendingComplianceUI(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task FreezeOrdersInPendingComplianceUI_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.FreezeOrdersInPendingComplianceUI(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _blotterController.FreezeOrdersInPendingComplianceUI(UnitTestConstants.CONST_REQUEST_DTO_ORDER);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task UnfreezeOrdersInPendingComplianceUI_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.UnfreezeOrdersInPendingComplianceUI(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.UnfreezeOrdersInPendingComplianceUI(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task UnfreezeOrdersInPendingComplianceUI_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.UnfreezeOrdersInPendingComplianceUI(UnitTestConstants.CONST_REQUEST_DTO_ORDER);

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
        public async Task UnfreezeOrdersInPendingComplianceUI_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.UnfreezeOrdersInPendingComplianceUI(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _blotterController.UnfreezeOrdersInPendingComplianceUI(UnitTestConstants.CONST_REQUEST_DTO_ORDER);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task RemoveManualExecution_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RemoveManualExecution(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.RemoveManualExecution(UnitTestConstants.CONST_ORDER_ID);

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
        public async Task RemoveManualExecution_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.RemoveManualExecution(UnitTestConstants.CONST_ORDER_ID);

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
        public async Task RemoveManualExecution_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RemoveManualExecution(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _blotterController.RemoveManualExecution(UnitTestConstants.CONST_ORDER_ID);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task SaveManualFills_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange          
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.SaveManualFills(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.SaveManualFills(UnitTestConstants.CONST_REQUEST_DTO_SAVE_MANUAL);

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
        public async Task SaveManualFills_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.SaveManualFills(UnitTestConstants.CONST_REQUEST_DTO_SAVE_MANUAL);

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
        public async Task SaveManualFills_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.SaveManualFills(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await  _blotterController.SaveManualFills(UnitTestConstants.CONST_REQUEST_DTO_SAVE_MANUAL);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task GetBlotterManualFills_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                string expected = ServiceGatewayTestCommons.blotterData;
                _mockBlotterService.Setup(x => x.GetBlotterManualFills(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.GetBlotterManualFills(UnitTestConstants.CONST_REQUEST_DATA);

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
        public async Task GetBlotterManualFills_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.GetBlotterManualFills(UnitTestConstants.CONST_REQUEST_DATA);

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
        public async Task GetBlotterManualFills_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.GetBlotterManualFills(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result =  await _blotterController.GetBlotterManualFills(UnitTestConstants.CONST_ORDER_ID);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetAllocateDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetAllocateDetails_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.GetAllocationDetails(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.GetAllocateDetails(UnitTestConstants.CONST_ORDER_ID);

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
        public async Task GetAllocateDetails_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.GetAllocateDetails(UnitTestConstants.CONST_ORDER_ID);

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
        public async Task GetAllocateDetails_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);

                _mockBlotterService.Setup(x => x.GetAllocationDetails(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
               var result = await _blotterController.GetAllocateDetails(UnitTestConstants.CONST_ORDER_ID);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task GetPstAllocationDetails_ReturnsOkResult(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                var requestDto = new RequestDto<string>
                {
                    Data = UnitTestConstants.CONST_ORDER_ID,
                    CorrelationId = Guid.NewGuid().ToString()
                };

                _mockBlotterService
                    .Setup(x => x.GetPstAllocationDetails(It.IsAny<RequestResponseModel>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _blotterController.GetPstAllocationDetails(requestDto);

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
        public async Task GetPstAllocationDetails_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var requestDto = new RequestDto<string>
                {
                    Data = UnitTestConstants.CONST_ORDER_ID,
                    CorrelationId = Guid.NewGuid().ToString()
                };

                // Act
                var result = await _blotterController.GetPstAllocationDetails(requestDto);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                var error = Assert.IsType<string>(badRequest.Value);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, error);
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
        public async Task SaveAllocationDetails_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.SaveAllocationDetails(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _blotterController.SaveAllocationDetails(UnitTestConstants.CONST_REQUEST_DTO_SAVE_ALLOCATION);

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
        public async Task SaveAllocationDetails_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _blotterController.SaveAllocationDetails(UnitTestConstants.CONST_REQUEST_DTO_SAVE_ALLOCATION);

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
        public async Task SaveAllocationDetails_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.SaveAllocationDetails(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _blotterController.SaveAllocationDetails(UnitTestConstants.CONST_REQUEST_DTO_SAVE_ALLOCATION);
                Assert.IsType<BadRequestObjectResult>(result);
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
        public async Task RenameBlotterCustomTab_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                string expected = ServiceGatewayTestCommons.blotterData;
                _mockBlotterService.Setup(x => x.RenameBlotterCustomTab(It.IsAny<RequestResponseModel>()));
                await Task.CompletedTask;

                //Act
                //var result = await _blotterController.RenameBlotterCustomTab(UnitTestConstants.CONST_REQUEST_DATA);

                ////Assert
                //var okResult = Assert.IsType<OkObjectResult>(result);
                //Assert.IsType<string>(okResult.Value);
                //Assert.Equal(expected, okResult.Value);


            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Fact]
        public async Task RenameBlotterCustomTab_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                //var result = await _blotterController.RenameBlotterCustomTab(UnitTestConstants.CONST_REQUEST_DATA);

                ////Assert
                //var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                //var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                //Assert.Equal(expectedErrorMessage, errorMessage);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task RenameBlotterCustomTab_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RenameBlotterCustomTab(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                //var result = await _blotterController.RenameBlotterCustomTab(UnitTestConstants.CONST_REQUEST_DATA);
                //Assert.IsType<BadRequestObjectResult>(result);
                await Task.CompletedTask;
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
        public async Task RemoveBlotterCustomTab_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                string expected = ServiceGatewayTestCommons.blotterData;
                _mockBlotterService.Setup(x => x.RemoveBlotterCustomTab(It.IsAny<RequestResponseModel>()));

                //Act
                //var result = await _blotterController.RemoveBlotterCustomTab(UnitTestConstants.CONST_REQUEST_DATA);

                ////Assert
                //var okResult = Assert.IsType<OkObjectResult>(result);
                //Assert.IsType<string>(okResult.Value);
                //Assert.Equal(expected, okResult.Value);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Fact]
        public async Task RemoveBlotterCustomTab_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                //var result = await _blotterController.RemoveBlotterCustomTab(UnitTestConstants.CONST_REQUEST_DATA);

                ////Assert
                //var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                //var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                //Assert.Equal(expectedErrorMessage, errorMessage);
                await Task.CompletedTask;

            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task RemoveBlotterCustomTab_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.RemoveBlotterCustomTab(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                //var result = await _blotterController.RemoveBlotterCustomTab(UnitTestConstants.CONST_REQUEST_DATA);
                //Assert.IsType<BadRequestObjectResult>(result);
                await Task.CompletedTask;
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
        public async Task TransferUser_ReturnsOkResult_WhenValidDataProvided(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.TransferUser(It.IsAny<RequestResponseModel>()));

                var requestDto = new TransferUserRequestDto
                {
                    OrderIds = new List<string> { "2021111610291846", "2021111610291847" }, // replace with test values
                    IncludeSubOrders = true,
                    TargetUserId="user123",
                    IsOrderTab = false,
                    IsAllowUserToTansferTrade = true,
                    UniqueIdentifier = "test-uid-123"
                };
                // Act
                var result = await _blotterController.TransferUser(requestDto);

                // Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, (okResult as OkResult).StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task TransferUser_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                var requestDto = new TransferUserRequestDto
                {
                    OrderIds = new List<string> { "2021111610291846", "2021111610291847" }, // replace with test values
                    IncludeSubOrders = true,
                    IsOrderTab = false,
                    IsAllowUserToTansferTrade = true,
                    UniqueIdentifier = "test-uid-123"
                };
                // Act
                var result = await _blotterController.TransferUser(requestDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(expectedErrorMessage, badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task TransferUser_ThrowsException_WhenServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockBlotterService.Setup(x => x.TransferUser(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));
                var requestDto = new TransferUserRequestDto
                {
                    OrderIds = new List<string> { "2021111610291846", "2021111610291847" }, // replace with test values
                    IncludeSubOrders = true,
                    TargetUserId = "user123",
                    IsOrderTab = false,
                    IsAllowUserToTansferTrade = true,
                    UniqueIdentifier = "test-uid-123"
                };
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(async () => await _blotterController.TransferUser(requestDto));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion
    }
}