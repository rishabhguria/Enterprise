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

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class ComplianceControllerTest : BaseControllerTest, IDisposable
    {
        private readonly ComplianceController _controller;

        public ComplianceControllerTest() : base()
        {
            _controller = new ComplianceController(
                 _mockValidationHelper.Object,
                 _mockTokenService.Object,
                _mockComplianceService.Object);
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

        #region SendComplianceData Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendComplianceData_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockComplianceService.Setup(x => x.SendComplianceData(It.IsAny<RequestResponseModel>()));
                //Act
                var result = await _controller.SendComplianceData(new GenericDto { Data =  UnitTestConstants.CONST_REQUEST_DATA });

                //Assert
                Assert.IsType<OkResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }


        [Fact]
        public async Task SendComplianceData_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                //Act
                var result = await _controller.SendComplianceData(new GenericDto { Data = UnitTestConstants.CONST_REQUEST_DATA });

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
        public async Task SendComplianceData_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockComplianceService.Setup(x => x.SendComplianceData(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.SendComplianceData(new GenericDto { Data = UnitTestConstants.CONST_REQUEST_DATA });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SendComplianceDataForStage Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SendComplianceDataForStage_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                _mockComplianceService.Setup(x => x.SendComplianceDataForStage(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.SendComplianceDataForStage(new GenericDto { Data = UnitTestConstants.CONST_REQUEST_DATA });

                //Assert
                Assert.IsType<OkResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }


        [Fact]
        public async Task SendComplianceDataForStage_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);

                //Act
                var result = await _controller.SendComplianceDataForStage(new GenericDto { Data = UnitTestConstants.CONST_REQUEST_DATA });

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
        public async Task SendComplianceDataForStage_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockComplianceService.Setup(x => x.SendComplianceDataForStage(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.SendComplianceDataForStage(new GenericDto { Data = UnitTestConstants.CONST_REQUEST_DATA });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region CheckComplianceFrBasket Test Cases

        [Theory]
        [InlineData(1)]
        public async Task CheckComplianceFrBasket_ReturnsOkResult(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                var request = new RequestResponseModel
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };
                _mockComplianceService.Setup(x => x.CheckComplianceFrBasket(It.IsAny<RequestResponseModel>()));

                // Act
                var result = await _controller.CheckComplianceFrBasket(request);

                // Assert
                Assert.IsType<OkResult>(result);
                _mockComplianceService.Verify(x => x.CheckComplianceFrBasket(It.IsAny<RequestResponseModel>()), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task CheckComplianceFrBasket_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                SetUserDto(0);
                var request = new RequestResponseModel
                {
                    Data = UnitTestConstants.CONST_REQUEST_DATA
                };

                // Act
                var result = await _controller.CheckComplianceFrBasket(request);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task CheckComplianceFrBasket_ReturnsBadRequest_WhenDataIsEmptyArray(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                var request = new RequestResponseModel
                {
                    Data = "[]"
                };

                // Act
                var result = await _controller.CheckComplianceFrBasket(request);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Parameter Data is misssing", badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion
    }

}
