using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.UnitTest.Commons;
using System.Threading.Tasks;
using Xunit;
using Prana.ServiceGateway.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Prana.KafkaWrapper;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class OpenfinManagementControllerTest : BaseControllerTest, IDisposable
    {
        private readonly OpenfinManagementController _controller;

        public OpenfinManagementControllerTest() : base()
        {
            _controller = new OpenfinManagementController(
                 _mockValidationHelper.Object,
                 _mockTokenService.Object,
                 _mockOpenfinManagerService.Object);
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

        #region GetSavedWorkspaceInformationForUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetSavedWorkspaceInformationForUser_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockOpenfinManagerService.Setup(x => x.GetOpenfinWorkspaceLayout(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.GetSavedWorkspaceInformationForUser();

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
        public async Task GetSavedWorkspaceInformationForUser_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.GetSavedWorkspaceInformationForUser();

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
        public async Task GetSavedWorkspaceInformationForUser_ThrowsException_WhenOpenfinManagerServiceThrowsException(int companyUserId)
        {
            // Arrange
            try
            {
                SetUserDto(companyUserId);
                _mockOpenfinManagerService.Setup(x => x.GetOpenfinWorkspaceLayout(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.GetSavedWorkspaceInformationForUser();
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {

                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveWorkspaceInformationForUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveWorkspaceInformationForUser_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                GenericDto openfinWorkspaceInformation = new();
                _mockOpenfinManagerService.Setup(x => x.SaveOpenfinWorkspaceLayout(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.SaveWorkspaceInformationForUser(openfinWorkspaceInformation);

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
        public async Task SaveWorkspaceInformationForUser_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                GenericDto openfinWorkspaceInformation = new();

                //Act
                var result = await _controller.SaveWorkspaceInformationForUser(openfinWorkspaceInformation);

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
        public async Task SaveWorkspaceInformationForUser_ThrowsException_WhenOpenfinManagerServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                GenericDto openfinWorkspaceInformation = new();

                _mockOpenfinManagerService.Setup(x => x.SaveOpenfinWorkspaceLayout(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_LAYOUTTITLE_OR_LAYOUT_CANNOT_BLANK));

                // Act & Assert
                var result = await _controller.SaveWorkspaceInformationForUser(openfinWorkspaceInformation);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region DeleteWorkspaceInformationForUser Test Cases

        [Fact]
        public async Task DeleteWorkspaceInformationForUser_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var dto = new GenericDto { Data = "workspace-data" };

                // Act
                var result = await _controller.DeleteWorkspaceInformationForUser(dto);

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
        public async Task DeleteWorkspaceInformationForUser_ReturnsOk_WhenValidRequest()
        {
            try
            {
                // Arrange
                SetUserDto(1);
                var dto = new GenericDto { Data = "workspace-data" };
                _mockOpenfinManagerService.Setup(x => x.DeleteOpenfinWorkspaceLayout(It.IsAny<RequestResponseModel>())).Returns(Task.CompletedTask);

                // Act
                var result = await _controller.DeleteWorkspaceInformationForUser(dto);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockOpenfinManagerService.Verify(x => x.DeleteOpenfinWorkspaceLayout(It.IsAny<RequestResponseModel>()), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

    }
}
