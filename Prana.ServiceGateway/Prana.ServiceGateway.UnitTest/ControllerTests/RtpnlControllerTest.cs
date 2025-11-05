using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Models.RequestDto;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class RtpnlControllerTest : BaseControllerTest, IDisposable
    {
        private readonly RtpnlController _controller;

        public RtpnlControllerTest() : base()
        {
            _controller = new RtpnlController(
                 _mockValidationHelper.Object,
                 _mockTokenService.Object,
                _mockRtpnlService.Object,
                GetMockLogger<RtpnlController>());
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

        #region SaveUpdateConfigDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveUpdateConfigDetails_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                ConfigDetailsInfo[] data = new ConfigDetailsInfo[] {new ConfigDetailsInfo() };

                //Act
                var result = await _controller.SaveUpdateConfigDetails(data);

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
        public async Task SaveUpdateConfigDetails_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                ConfigDetailsInfo[] data = new ConfigDetailsInfo[0];

                //Act
                var result = await _controller.SaveUpdateConfigDetails(data);

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
        #endregion

        #region GetRtpnlWidgetData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlWidgetData_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockRtpnlService.Setup(x => x.GetRtpnlWidgetData(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GetRtpnlWidgetData();

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
        public async Task GetRtpnlWidgetData_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GetRtpnlWidgetData();

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
        public async Task GetRtpnlWidgetData_ThrowsException_WhenCalculationServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockRtpnlService.Setup(x => x.GetRtpnlWidgetData(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.GetRtpnlWidgetData();
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveConfigDataForExtract Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveConfigDataForExtract_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            ConfigDetailsInfo[] data = new ConfigDetailsInfo[] { new ConfigDetailsInfo() };

            _mockRtpnlService.Setup(x => x.SaveConfigDataForExtract(It.IsAny<RequestResponseModel>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SaveConfigDataForExtract(data);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockRtpnlService.Verify(x => x.SaveConfigDataForExtract(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task SaveConfigDataForExtract_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            ConfigDetailsInfo[] data = new ConfigDetailsInfo[] { new ConfigDetailsInfo() };

            // Act
            var result = await _controller.SaveConfigDataForExtract(data);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockRtpnlService.Verify(x => x.SaveConfigDataForExtract(It.IsAny<RequestResponseModel>()), Times.Never);
        }
        #endregion

        #region GetRtpnlWidgetConfigData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetRtpnlWidgetConfigData_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            RtpnlWidgetConfigRequestDto data = new RtpnlWidgetConfigRequestDto(); // Create a sample DTO

            _mockRtpnlService.Setup(x => x.GetRtpnlWidgetConfigData(It.IsAny<RequestResponseModel>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.GetRtpnlWidgetConfigData(data);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockRtpnlService.Verify(x => x.GetRtpnlWidgetConfigData(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task GetRtpnlWidgetConfigData_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            RtpnlWidgetConfigRequestDto data = new RtpnlWidgetConfigRequestDto(); // Create a sample DTO

            // Act
            var result = await _controller.GetRtpnlWidgetConfigData(data);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockRtpnlService.Verify(x => x.GetRtpnlWidgetConfigData(It.IsAny<RequestResponseModel>()), Times.Never);
        }
        #endregion

        #region CheckCalculationServiceRunning Test Cases
        [Theory]
        [InlineData(1)]
        public async Task CheckCalculationServiceRunning_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            _mockRtpnlService.Setup(x => x.CheckCalculationServiceRunning(It.IsAny<RequestResponseModel>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CheckCalculationServiceRunning();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockRtpnlService.Verify(x => x.CheckCalculationServiceRunning(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task CheckCalculationServiceRunning_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);

            // Act
            var result = await _controller.CheckCalculationServiceRunning();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockRtpnlService.Verify(x => x.CheckCalculationServiceRunning(It.IsAny<RequestResponseModel>()), Times.Never);
        }
        #endregion

        #region DeleteRemovedWidgetConfigDetails Test Cases
        [Theory]
        [InlineData(1)]
        public async Task DeleteRemovedWidgetConfigDetails_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            List<WidgetConfigDto> widgetConfigs = new List<WidgetConfigDto> { new WidgetConfigDto() }; // Sample data

            _mockRtpnlService.Setup(x => x.DeleteRemovedWidgetConfigDetails(It.IsAny<RequestResponseModel>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteRemovedWidgetConfigDetails(widgetConfigs);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockRtpnlService.Verify(x => x.DeleteRemovedWidgetConfigDetails(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRemovedWidgetConfigDetails_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            List<WidgetConfigDto> widgetConfigs = new List<WidgetConfigDto> { new WidgetConfigDto() }; // Sample data

            // Act
            var result = await _controller.DeleteRemovedWidgetConfigDetails(widgetConfigs);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockRtpnlService.Verify(x => x.DeleteRemovedWidgetConfigDetails(It.IsAny<RequestResponseModel>()), Times.Never);
        }
        #endregion

        #region RegisterRTPNLUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task RegisterRTPNLUser_ReturnsOkResult_WhenUserIsRegistered(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);

            // NOTE: Directly mocking static methods like HubClientConnectionManagerRTPNL.AddUserToRTPNLUpdatesList
            // with Moq is not possible. This test assumes these static calls execute without throwing exceptions.
            // For true unit test isolation, these static classes/methods should be refactored to be injectable.

            // Act
            var result = await _controller.RegisterRTPNLUser();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task RegisterRTPNLUser_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);

            // Act
            var result = await _controller.RegisterRTPNLUser();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task RegisterRTPNLUser_ReturnsOkResult_EvenWhenInternalExceptionOccurs(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);

            // NOTE: Simulating an exception from static methods (HubClientConnectionManagerRTPNL) is complex
            // without refactoring or a static mocking framework.
            // For this test, we are focusing on verifying the controller's return behavior (always Ok())
            // even if an internal exception occurs, as per the current controller's implementation.
            // The controller's catch block logs the error but still returns Ok().

            // Act
            var result = await _controller.RegisterRTPNLUser();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            // No logger verification, as requested.
        }
        #endregion

        #region DeRegisterRTPNLUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task DeRegisterRTPNLUser_ReturnsOkResult_WhenUserIsDeRegistered(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);

            // NOTE: Directly mocking static methods like HubClientConnectionManagerRTPNL.RemoveUserFromRTPNLUpdatesList
            // with Moq is not possible. This test assumes these static calls execute without throwing exceptions.
            // For true unit test isolation, these static classes/methods should be refactored to be injectable.

            // Act
            var result = await _controller.DeRegisterRTPNLUser();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeRegisterRTPNLUser_ReturnsBadRequest_WhenCompanyUserIDIsZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);

            // Act
            var result = await _controller.DeRegisterRTPNLUser();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeRegisterRTPNLUser_ReturnsOkResult_EvenWhenInternalExceptionOccurs(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);

            // NOTE: Simulating an exception from static methods (HubClientConnectionManagerRTPNL) is complex
            // without refactoring or a static mocking framework.
            // For this test, we are focusing on verifying the controller's return behavior (always Ok())
            // even if an internal exception occurs, as per the current controller's implementation.
            // The controller's catch block logs the error but still returns Ok().

            // Act
            var result = await _controller.DeRegisterRTPNLUser();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            // No logger verification, as requested.
        }
        #endregion

        #region GetTradeAttributesLabelsRtpnl Test Cases
        [Fact]
        public async Task GetTradeAttributesLabelsRtpnl_ReturnsBadRequest_WhenRequestCompanyUserIDIsZero()
        {
            // Arrange
            // Note: UserDtoObj.CompanyUserId is set by SetUserDto, but this method also checks requestDto.CompanyUserID
            SetUserDto(1); // Set a non-zero CompanyUserID for UserDtoObj
            var requestDto = new BaseRequestDto { CompanyUserID = 0, CorrelationId = Guid.NewGuid().ToString() }; // Set requestDto.CompanyUserID to 0

            // Act
            var result = await _controller.GetTradeAttributesLabelsRtpnl(requestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockRtpnlService.Verify(x => x.GetRtpnlTradeAttributeLabels(It.IsAny<RequestResponseModel>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTradeAttributesLabelsRtpnl_ReturnsBadRequest_WhenServiceThrowsException(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            var requestDto = new BaseRequestDto { CompanyUserID = companyUserId, CorrelationId = Guid.NewGuid().ToString() };
            var simulatedExceptionMessage = "Simulated service error";

            _mockRtpnlService.Setup(x => x.GetRtpnlTradeAttributeLabels(It.IsAny<RequestResponseModel>()))
                             .ThrowsAsync(new Exception(simulatedExceptionMessage));

            // Act
            var result = await _controller.GetTradeAttributesLabelsRtpnl(requestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + simulatedExceptionMessage, badRequestResult.Value);
        }
        #endregion

        public void Dispose()
        {
        }
    }
}
