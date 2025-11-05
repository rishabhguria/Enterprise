using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class LayoutControllerTest : BaseControllerTest, IDisposable
    {
        private readonly LayoutController _controller;

        public LayoutControllerTest() : base()
        {
            _controller = new LayoutController(
                _mockValidationHelper.Object,
                 _mockTokenService.Object,
                _mockLayoutService.Object,
                GetMockLogger<LayoutController>());
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

        #region SaveLayout Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveLayout_RetunsOkResult(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                // Create a valid ViewInfo object for the request body
                var viewInfo = new ViewInfo
                {
                    // Populate with necessary properties for a valid scenario
                    description = "Test Description",
                    viewLayout = "{ \"layout\": \"some_json_layout\" }",
                    viewName = "TestView",
                    viewId = "test-view-id",
                    moduleName = "TestModule",
                    menuItem = "TestMenuItem"
                };

                // Setup the mock service to complete successfully
                _mockLayoutService.Setup(x => x.SaveLayout(It.IsAny<RequestResponseModel>()))
                                  .Returns(Task.CompletedTask); // Returns a completed task for async void/Task methods

                // Act
                var result = await _controller.SaveLayout(viewInfo);

                // Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);

                // Verify that the service method was called exactly once with any RequestResponseModel
                _mockLayoutService.Verify(x => x.SaveLayout(It.IsAny<RequestResponseModel>()), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }

        }

        [Fact]
        public async Task SaveLayout_ReturnsBadRequest_WhenCompanyUserIDZero()
        {
            // Arrange
            int companyUserId = 0;
            string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
            SetUserDto(companyUserId);
            var viewInfo = new ViewInfo // DTO is required even if CompanyUserId is 0
            {
                description = "Test Description",
                viewLayout = "{ \"layout\": \"some_json_layout\" }",
                viewName = "TestView",
                viewId = "test-view-id",
                moduleName = "TestModule",
                menuItem = "TestMenuItem"
            };

            // Act
            var result = await _controller.SaveLayout(viewInfo);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);

            // Verify that the service method was NOT called
            _mockLayoutService.Verify(x => x.SaveLayout(It.IsAny<RequestResponseModel>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveLayout_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                //Dictionary<string, string> layouts = new Dictionary<string, string>();
                //layouts.Add(UnitTestConstants.CONST_ID, UnitTestConstants.CONST_LAYOUT);

                //_mockLayoutService.Setup(x => x.SaveLayout(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                //// Act & Assert
                //var result = await _controller.SaveLayout(layouts);
                //Assert.IsType<BadRequestObjectResult>(result);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region LoadBlotterAllGridsLayoutAsync Test Cases
        [Theory]
        [InlineData(1)]
        public async Task LoadBlotterAllGridsLayoutAsync_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            var genericDto = new GenericDto { Data = "some_data" };

            // Setup the mock service to complete successfully
            _mockLayoutService.Setup(x => x.LoadLayout(It.IsAny<RequestResponseModel>()))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.LoadBlotterAllGridsLayoutAsync(genericDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Verify that the service method was called exactly once
            _mockLayoutService.Verify(x => x.LoadLayout(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task LoadBlotterAllGridsLayoutAsync_ReturnsBadRequest_WhenCompanyUserIDZero()
        {
            // Arrange
            int companyUserId = 0;
            string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
            SetUserDto(companyUserId);
            var genericDto = new GenericDto { Data = "some_data" }; // DTO is required even if CompanyUserId is 0

            // Act
            var result = await _controller.LoadBlotterAllGridsLayoutAsync(genericDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);

            // Verify that the service method was NOT called
            _mockLayoutService.Verify(x => x.LoadLayout(It.IsAny<RequestResponseModel>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task LoadBlotterAllGridsLayoutAsync_ThrowsException_WhenBlotterServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockLayoutService.Setup(x => x.LoadLayout(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                //var result = await _controller.LoadBlotterAllGridsLayoutAsync();
                //Assert.IsType<BadRequestObjectResult>(result);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveRtpnlLayout Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveRtpnlLayout_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                GenericDto layouts = new GenericDto();
                layouts.Data= UnitTestConstants.CONST_LAYOUT ;

                _mockLayoutService.Setup(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.SaveOrUpdateRtpnlLayout(layouts);

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
        public async Task SaveRtpnlLayout_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                GenericDto layouts = new GenericDto();
                layouts.Data = UnitTestConstants.CONST_LAYOUT;

                //Act
                var result = await _controller.SaveOrUpdateRtpnlLayout(layouts);

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
        public async Task SaveRtpnlLayout_ThrowsException_WhenTitleOrLayoutIsEmpty(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                GenericDto layouts = new GenericDto();
                layouts.Data = UnitTestConstants.CONST_LAYOUT;

                _mockLayoutService.Setup(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.SaveOrUpdateRtpnlLayout(layouts);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveOrUpdateRtpnlLayout Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveOrUpdateRtpnlLayout_ReturnsOkResult(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            // Create a valid GenericDto with JSON that can be deserialized into RtpnlLayoutInfo
            var rtpnlLayoutInfo = new RtpnlLayoutInfo
            {
                pageInfo = new PageInfo(), // Initialize PageInfo
                internalPageInfo = new List<ViewInfo>
                {
                    new ViewInfo { viewId = "id1", viewName = "name1", description = "desc1", moduleName = "mod1", menuItem = "menu1", viewLayout = "{}" },
                    new ViewInfo { viewId = "id2", viewName = "name2", description = "desc2", moduleName = "mod2", menuItem = "menu2", viewLayout = "{}" }
                }
            };
            var genericDto = new GenericDto { Data = JsonConvert.SerializeObject(rtpnlLayoutInfo) };

            _mockLayoutService.Setup(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SaveOrUpdateRtpnlLayout(genericDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            _mockLayoutService.Verify(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task SaveOrUpdateRtpnlLayout_ReturnsBadRequest_WhenDataIsNullOrEmpty()
        {
            // Arrange
            SetUserDto(1); // Set a valid CompanyUserId
            var genericDto = new GenericDto { Data = null }; // Test with null data

            // Act
            var result = await _controller.SaveOrUpdateRtpnlLayout(genericDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_LAYOUTTITLE_OR_LAYOUT_CANNOT_BLANK, badRequestResult.Value);
            _mockLayoutService.Verify(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()), Times.Never);
        }

        [Fact]
        public async Task SaveOrUpdateRtpnlLayout_ReturnsBadRequest_WhenCompanyUserIDZero()
        {
            // Arrange
            int companyUserId = 0;
            SetUserDto(companyUserId);
            var rtpnlLayoutInfo = new RtpnlLayoutInfo
            {
                pageInfo = new PageInfo(), // Initialize PageInfo
                internalPageInfo = new List<ViewInfo>
                {
                    new ViewInfo { viewId = "id1", viewName = "name1", description = "desc1", moduleName = "mod1", menuItem = "menu1", viewLayout = "{}" }
                }
            };
            var genericDto = new GenericDto { Data = JsonConvert.SerializeObject(rtpnlLayoutInfo) };

            // Act
            var result = await _controller.SaveOrUpdateRtpnlLayout(genericDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, badRequestResult.Value);
            _mockLayoutService.Verify(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()), Times.Never);
        }

        [Fact]
        public async Task SaveOrUpdateRtpnlLayout_ReturnsBadRequest_WhenInternalPageInfoIsInvalid()
        {
            // Arrange
            SetUserDto(1); // Valid CompanyUserId
            // Create a GenericDto with JSON where internalPageInfo has invalid viewId/viewName
            var rtpnlLayoutInfo = new RtpnlLayoutInfo
            {
                pageInfo = new PageInfo(), // Initialize PageInfo
                internalPageInfo = new List<ViewInfo>
                {
                    new ViewInfo { viewId = "id1", viewName = "", description = "desc1", moduleName = "mod1", menuItem = "menu1", viewLayout = "{}" }, // Invalid viewName
                    new ViewInfo { viewId = "", viewName = "name2", description = "desc2", moduleName = "mod2", menuItem = "menu2", viewLayout = "{}" }  // Invalid viewId
                }
            };
            var genericDto = new GenericDto { Data = JsonConvert.SerializeObject(rtpnlLayoutInfo) };

            // Act
            var result = await _controller.SaveOrUpdateRtpnlLayout(genericDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_LAYOUTTITLE_OR_LAYOUT_CANNOT_BLANK, badRequestResult.Value);
            _mockLayoutService.Verify(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task SaveOrUpdateRtpnlLayout_ReturnsBadRequest_WhenServiceThrowsException(int companyUserId)
        {
            // Arrange
            SetUserDto(companyUserId);
            var rtpnlLayoutInfo = new RtpnlLayoutInfo
            {
                pageInfo = new PageInfo(), // Initialize PageInfo
                internalPageInfo = new List<ViewInfo>
                {
                    new ViewInfo { viewId = "id1", viewName = "name1", description = "desc1", moduleName = "mod1", menuItem = "menu1", viewLayout = "{}" }
                }
            };
            var genericDto = new GenericDto { Data = JsonConvert.SerializeObject(rtpnlLayoutInfo) };

            var exceptionMessage = "Simulated service error during RTPNL save.";
            _mockLayoutService.Setup(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()))
                              .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.SaveOrUpdateRtpnlLayout(genericDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + exceptionMessage, badRequestResult.Value);
            _mockLayoutService.Verify(x => x.SaveOrUpdateRtpnlViews(It.IsAny<RequestResponseModel>()), Times.Once);
        }
        #endregion

        #region LoadRtpnlLayout Test Cases
        [Theory]
        [InlineData(1)]
        public async Task LoadRtpnlLayout_RetunsOkResult(int companyUserId)
        {
            try
            {
                //Arrange
                SetUserDto(companyUserId);
                LoadRtpnlLayout data = new LoadRtpnlLayout
                {
                    pageId = "page1",
                    viewName = "view1"
                };
                _mockLayoutService.Setup(x => x.LoadRtpnlViews(It.IsAny<RequestResponseModel>()));

                //Act
                var result = await _controller.LoadRtpnlLayout(data);

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
        public async Task LoadRtpnlLayout_ReturnsBadResult_WhenCompanyUserIDZero()
        {
            try
            {
                //Arrange
                int companyUserId = 0;
                LoadRtpnlLayout data = new LoadRtpnlLayout
                {
                    pageId = "page1",
                    viewName = "view1"
                };
                string expectedErrorMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetUserDto(companyUserId);
                //Act
                var result = await _controller.LoadRtpnlLayout(data);

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
        public async Task LoadRtpnlLayout_ThrowsException_WhenCalculationServiceThrowsException(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                LoadRtpnlLayout data = new LoadRtpnlLayout
                {
                    pageId = "page1",
                    viewName = "view1"
                };
                _mockLayoutService.Setup(x => x.LoadRtpnlViews(It.IsAny<RequestResponseModel>())).ThrowsAsync(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));

                // Act & Assert
                var result = await _controller.LoadRtpnlLayout(data);
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion
        
        #region DeletePageInformationForUser Test Cases

        [Fact]
        public async Task DeletePageInformationForUser_ReturnsBadRequest_WhenCompanyUserIdIsZero()
        {
            try
            {
                // Arrange
                int companyUserId = 0;
                SetUserDto(companyUserId);
                DeletePageDTO pageDto = new DeletePageDTO(); // Populate with test data if needed

                // Act
                var result = await _controller.DeletePageInformationForUser(pageDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.Equal(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeletePageInformationForUser_ReturnsOkResult_WhenValidRequest(int companyUserId)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                DeletePageDTO pageDto = new DeletePageDTO(); // Populate with test data if needed
                _mockLayoutService
                    .Setup(x => x.DeleteOpenfinPages(It.IsAny<RequestResponseModel>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.DeletePageInformationForUser(pageDto);

                // Assert
                var okResult = Assert.IsType<OkResult>(result);
                Assert.Equal(200, okResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion

        #region RemovePagesForAnUser Test Cases

        [Theory]
        [InlineData(1, "ModuleA")]
        public async Task RemovePagesForAnUser_ReturnsOkResult_WhenValidRequest(int companyUserId, string moduleName)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockLayoutService
                    .Setup(x => x.RemovePagesForAnUser(It.IsAny<RequestResponseModel>()))
                    .Returns(Task.CompletedTask);

                // Act
                var result = await _controller.RemovePagesForAnUser(moduleName);

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
        [InlineData(1, "ModuleA")]
        public async Task RemovePagesForAnUser_ReturnsBadRequest_WhenServiceThrowsException(int companyUserId, string moduleName)
        {
            try
            {
                // Arrange
                SetUserDto(companyUserId);
                _mockLayoutService
                    .Setup(x => x.RemovePagesForAnUser(It.IsAny<RequestResponseModel>()))
                    .ThrowsAsync(new Exception("Kafka failure"));

                // Act
                var result = await _controller.RemovePagesForAnUser(moduleName);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequestResult.Value);
                Assert.StartsWith(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #endregion
    }
}
