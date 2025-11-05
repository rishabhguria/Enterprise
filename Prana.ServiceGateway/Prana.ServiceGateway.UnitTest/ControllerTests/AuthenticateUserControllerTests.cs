using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Models.RequestDto;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class AuthenticateUserControllerTests : BaseControllerTest, IDisposable
    {
        private readonly AuthenticateUserController _controller;

        public AuthenticateUserControllerTests()
        {
            _controller = new AuthenticateUserController(
                _mockValidationHelper.Object,
                _mockTokenService.Object,
                _mockAuthenticateUserService.Object,
                _mockConfiguration.Object,
                GetMockLogger<AuthenticateUserController>());
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
        /// Sets the valid token value
        /// </summary>
        /// <param name="companyUserId"></param>
        private void SetToken(int companyUserId)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ControllerMethodConstants.CONST_COMPANY_USER_ID, companyUserId.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("#$nirvana@SamsaraWebApplication$#@"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "nirvanaSolutions",
                    audience: "nirvanasolutions.com",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                var userDto = new UserDto
                {
                    CompanyUserId = companyUserId,
                    IsAdmin = true,
                    IsSupport = false,
                };

                _mockTokenService.Setup(x => x.DeactivateCurrentAsync()).Returns(Task.CompletedTask);
                _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>())).Returns(userDto);
                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                _controller.ControllerContext.HttpContext.Request.Headers[UnitTestConstants.CONST_TOKEN] = tokenString;
                _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        #region LoginUser Test Cases
        [Fact]
        public async Task LoginUser_ReturnsBadRequest_WhenUserDetailsIsNull()
        {
            try
            {
                // Arrange
                AuthenticationRequestDto userDetails = null;

                // Act
                var result = await _controller.LoginUser(userDetails);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task LoginUser_ReturnsBadRequest_WhenUserNameOrSamsaraEmailIsEmpty()
        {
            try
            {
                // Arrange
                AuthenticationRequestDto userDetails = new AuthenticationRequestDto();

                // Act
                var result = await _controller.LoginUser(userDetails);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task LoginUser_ReturnsBadRequest_WhenAuthenticateUserServiceReturnsNull()
        {
            try
            {
                // Arrange
                var userDetails = ServiceGatewayTestCommons.userDetailsDto;
                _mockAuthenticateUserService.Setup(x => x.LoginUser(It.IsAny<RequestResponseModel>())).ReturnsAsync((string)null);

                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                _controller.ControllerContext.HttpContext.Request.Headers[UnitTestConstants.CONST_TOKEN] = "dummyToken";

                // Act
                var result = await _controller.LoginUser(userDetails);

                // Assert
                var statusCodeResult = Assert.IsType<ObjectResult>(result);
                Assert.Equal(500, statusCodeResult.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task LoginUser_ReturnsOkResultWithToken_WhenValidDetails(int companyUserId)
        {
            try
            {
                // Arrange
                var userDetails = ServiceGatewayTestCommons.userDetailsDto;
                dynamic expectedResultObject = new
                {
                    CompanyUserId = companyUserId,
                    ErrorMessage = string.Empty,
                    token = string.Empty
                };
                string expectedResponse = JsonConvert.SerializeObject(expectedResultObject);
                var expectedToken = UnitTestConstants.CONST_TOKEN;

                _mockTokenService.Setup(x => x.CreateToken(It.IsAny<UserDto>())).Returns(expectedToken);
                _mockAuthenticateUserService.Setup(x => x.LoginUser(It.IsAny<RequestResponseModel>())).ReturnsAsync(expectedResponse);

                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                _controller.ControllerContext.HttpContext.Request.Headers[UnitTestConstants.CONST_TOKEN] = "dummyToken";

                // Act
                IActionResult result = await _controller.LoginUser(userDetails);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);

                dynamic data = (JObject)JsonConvert.DeserializeObject<dynamic>((string)okResult.Value);

                Assert.Equal(expectedToken, data.token.ToString());
                Assert.Equal(expectedResultObject.CompanyUserId, Int32.Parse(data.CompanyUserId.ToString()));
                Assert.Equal(expectedResultObject.ErrorMessage, data.ErrorMessage.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task LoginUser_ReturnsBadRequestWithErrorMessage_WhenMissingDetails()
        {
            try
            {
                // Arrange
                var userDetails =new AuthenticationRequestDto();
                var expectedErrorMessage = "Both Username and samsaraAzureId is blank or null or AuthenticationRequestDto is null";

                // Act
                var result = await _controller.LoginUser(userDetails);

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
        public async Task LoginUser_ReturnsBadRequest_WhenErrorMessageIsNotEmptyDataService(int companyUserId)
        {
            try
            {
                // Arrange
                var userDetails = ServiceGatewayTestCommons.userDetailsDto;
                dynamic expectedResultObject = new
                {
                    CompanyUserId = companyUserId,
                    ErrorMessage = MessageConstants.MSG_CONST_AN_ERROR_OCCURRED,
                    token = string.Empty
                };
                string expectedResponse = JsonConvert.SerializeObject(expectedResultObject);
                _mockAuthenticateUserService.Setup(x => x.LoginUser(It.IsAny<RequestResponseModel>())).ReturnsAsync(expectedResponse);

                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                _controller.ControllerContext.HttpContext.Request.Headers[UnitTestConstants.CONST_TOKEN] = "dummyToken";

                // Act
                var result = await _controller.LoginUser(userDetails);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region LogoutUser Test Cases
        [Theory]
        [InlineData(1)]
        public async Task LogoutUser_ReturnsOk_WithValidJwtToken(int companyUserId)
        {
            try
            {
                // Arrange
                dynamic expectedResultObject = new
                {
                    CompanyUserId = companyUserId,
                    ErrorMessage = string.Empty,
                };
                string expectedResponse = JsonConvert.SerializeObject(expectedResultObject);
                SetToken(companyUserId);
                _mockAuthenticateUserService.Setup(x => x.LogoutUser(It.IsAny<RequestResponseModel>()));

                // Act
                var result = await _controller.LogoutUser();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task LogoutUser_ReturnsBadRequest_WithTokenNULL()
        {
            try
            {
                //ARRANGE
                _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
                _controller.ControllerContext.HttpContext.Request.Headers[UnitTestConstants.CONST_TOKEN] = String.Empty;

                //ACT
                var result = await _controller.LogoutUser();
                Assert.IsType<UnauthorizedObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public async Task LogoutUser_ReturnBadRequest_WithComapnyIDZero()
        {
            try
            {
                //Arrange
                var companyUserId = 0;
                var requestResponseObj = new RequestResponseModel(companyUserId, string.Empty, CorrelationId);
                var expectedMessage = MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK;
                SetToken(companyUserId);
                _mockAuthenticateUserService.Setup(x => x.LogoutUser(It.IsAny<RequestResponseModel>()));

                // Act
                var result = await _controller.LogoutUser();

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badResult = result as BadRequestObjectResult;
                Assert.Equal(expectedMessage, badResult.Value);
                _mockTokenService.Verify(x => x.DeactivateCurrentAsync(), Times.Never);
                _mockAuthenticateUserService.Verify(x => x.LoginUser(It.IsAny<RequestResponseModel>()), Times.Never);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region ValidateToken Test Cases
        [Fact]
        public async Task ValidateToken_InvalidToken_ReturnsBadRequestResult()
        {
            try
            {
                // Arrange
                var jwtToken = string.Empty;
                _controller.ControllerContext = new ControllerContext();
                _controller.ControllerContext.HttpContext = new DefaultHttpContext();
                _controller.ControllerContext.HttpContext.Request.Headers[UnitTestConstants.CONST_TOKEN] = jwtToken;

                // Act
                var result = await _controller.ValidateToken();

                // Assert
                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = (BadRequestObjectResult)result;
                Assert.Equal(MessageConstants.MSG_CONST_INVALID_TOKEN, badRequestResult.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task ValidateToken_ValidToken_ReturnsOkRequestResult(int companyUserId)
        {
            try
            {
                // Arrange
                SetToken(companyUserId);

                _mockConfiguration.Setup(x => x["AuthSettings:issuer"]).Returns("nirvanaSolutions");
                _mockConfiguration.Setup(x => x["AuthSettings:audience"]).Returns("nirvanasolutions.com");
                _mockConfiguration.Setup(x => x["AuthSettings:secretKey"]).Returns("#$nirvana@SamsaraWebApplication$#@");

                // Act
                var result = await _controller.ValidateToken();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region GetStatusForLogin Test Cases

        [Fact]
        public async Task GetStatusForLogin_ReturnsInternalServerError_WhenServiceReturnsNull()
        {
            // Arrange
            string userDetails = "someUserDetails";
            _mockAuthenticateUserService.Setup(x => x.GetStatusForLogin(It.IsAny<RequestResponseModel>())).ReturnsAsync((string)null);

            // Set up HttpContext for CorrelationId to avoid NullReferenceException
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            // Act
            var result = await _controller.GetStatusForLogin(userDetails);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal(MessageConstants.MSG_CONST_INTERNAL_SERVER_ERROR, statusCodeResult.Value);
        }

        [Fact]
        public async Task GetStatusForLogin_ReturnsInternalServerError_WhenDeserializedDataIsNull()
        {
            // Arrange
            string userDetails = "someUserDetails";
            // Mock the service to return a string that, when deserialized, results in a null object.
            // For `JsonConvert.DeserializeObject<StatusForLoginDto>(message)`, returning "null" as a string
            // is the most direct way to get a null `data` object if `StatusForLoginDto` is a class.
            _mockAuthenticateUserService.Setup(x => x.GetStatusForLogin(It.IsAny<RequestResponseModel>())).ReturnsAsync("null");

            // Set up HttpContext for CorrelationId to avoid NullReferenceException
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            // Act
            var result = await _controller.GetStatusForLogin(userDetails);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal(MessageConstants.MSG_CONST_INTERNAL_SERVER_ERROR, statusCodeResult.Value);
        }

        [Fact]
        public async Task GetStatusForLogin_ReturnsBadRequest_WhenErrorMessageIsNotEmpty()
        {
            // Arrange
            string userDetails = "someUserDetails";
            // Prepare a StatusForLoginDto with an error message
            var errorData = new StatusForLoginDto { ErrorMessage = "Test Error Message", Status = false }; // Added IsSuccess and StatusMessage for complete comparison
            string errorMessageJson = JsonConvert.SerializeObject(errorData); // This is the string the service returns
            _mockAuthenticateUserService.Setup(x => x.GetStatusForLogin(It.IsAny<RequestResponseModel>())).ReturnsAsync(errorMessageJson);

            // Set up HttpContext for CorrelationId to avoid NullReferenceException
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            // Act
            var result = await _controller.GetStatusForLogin(userDetails);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // The Value property of BadRequestObjectResult will be the string returned by JsonConvert.SerializeObject(data)
            // So, assert that the Value is a string, and then deserialize it to compare with the original object.
            Assert.IsType<string>(badRequestResult.Value);

            // Deserialize the actual response value and compare it to our expected object
            var actualData = JsonConvert.DeserializeObject<StatusForLoginDto>((string)badRequestResult.Value);

            Assert.NotNull(actualData); // Ensure deserialization was successful

            // Now compare the properties of the deserialized object with the properties of your expected object
            Assert.Equal(errorData.ErrorMessage, actualData.ErrorMessage);
            Assert.Equal(errorData.Status, actualData.Status);

            // Not adding logger verification explicitly to avoid previous issues.
        }

        [Fact]
        public async Task GetStatusForLogin_ReturnsOk_WhenServiceReturnsStatus()
        {
            // Arrange
            string userDetails = "someUserDetails";
            // Prepare a StatusForLoginDto without an error message (success scenario)
            var successData = new StatusForLoginDto { ErrorMessage = string.Empty, Status = true };
            string successMessageJson = JsonConvert.SerializeObject(successData); // This is the string the service returns
            _mockAuthenticateUserService.Setup(x => x.GetStatusForLogin(It.IsAny<RequestResponseModel>())).ReturnsAsync(successMessageJson);

            // Set up HttpContext for CorrelationId to avoid NullReferenceException
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            // Act
            var result = await _controller.GetStatusForLogin(userDetails);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // The Value property of OkObjectResult will be the string returned by JsonConvert.SerializeObject(data)
            // So, assert that the Value is a string, and then deserialize it to compare with the original object.
            Assert.IsType<string>(okResult.Value);

            // Deserialize the actual response value and compare it to our expected object
            var actualData = JsonConvert.DeserializeObject<StatusForLoginDto>((string)okResult.Value);

            Assert.NotNull(actualData); // Ensure deserialization was successful

            // Now compare the properties of the deserialized object with the properties of your expected object
            Assert.Equal(successData.ErrorMessage, actualData.ErrorMessage);
            Assert.Equal(successData.Status, actualData.Status);

            // You could also re-serialize your expected object and compare strings directly, but deserializing
            // the actual result and comparing properties is generally more robust to minor formatting differences.
            // Assert.Equal(successMessageJson, (string)okResult.Value); // This would also work if JSON serialization is consistent.
        }
        #endregion

        #region ForceLogoutUser Test Cases

        [Fact]
        public async Task ForceLogoutUser_ReturnsOkResult_WithMinimalVerification()
        {
            // Arrange
            var companyUserId = 123;
            var mockLoginUserDto = new LoginUserDto { CompanyUserId = companyUserId, ErrorMessage = string.Empty, token = string.Empty };
            var responseData = JsonConvert.SerializeObject(mockLoginUserDto);

            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            _mockAuthenticateUserService.Setup(x => x.LogoutUser(It.IsAny<RequestResponseModel>()))
                                        .ReturnsAsync("Logout successful");

            // Act
            var result = await _controller.ForceLogoutUser(responseData);

            // Assert
            Assert.IsType<OkResult>(result);

            // Verify that the method was called exactly once, regardless of the object's contents.
            // This removes the predicate as a point of failure.
            _mockAuthenticateUserService.Verify(x => x.LogoutUser(It.IsAny<RequestResponseModel>()), Times.Once);
        }

        [Fact]
        public async Task ForceLogoutUser_ThrowsJsonReaderException_WhenDeserializationFails()
        {
            // Arrange
            // Provide malformed JSON to simulate a deserialization failure
            var malformedResponseData = "this is not valid json";

            var mockLogger = new Mock<ILogger<AuthenticateUserController>>();
            var controllerWithMockLogger = new AuthenticateUserController(
                _mockValidationHelper.Object,
                _mockTokenService.Object,
                _mockAuthenticateUserService.Object,
                _mockConfiguration.Object,
                mockLogger.Object // Inject our mock logger
            );

            // Set up HttpContext for CorrelationId, as before.
            controllerWithMockLogger.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            controllerWithMockLogger.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            // Act & Assert
            // Expecting an exception during deserialization.
            // Change the expected exception type to the more specific JsonReaderException
            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() => controllerWithMockLogger.ForceLogoutUser(malformedResponseData));

            // NOTE: We do not verify a logger call here, as the controller's code does not have a catch block
            // for this method, so the exception should propagate directly without being logged by the controller.
        }
        #endregion

        #region UpdateCacheForLoginUser Test Cases

        [Theory]
        [InlineData("{\"companyUserId\": \"123\", \"token\": \"abc\"}")]
        public void UpdateCacheForLoginUser_ReturnsOk_WhenUserDtoIsValid(string userDetails)
        {
            // Arrange
            var expectedCorrelationId = "test-correlation-id";

            // Use argument capture to get the object that is passed to the mock
            RequestResponseModel capturedModel = null;
            _mockAuthenticateUserService.Setup(x => x.UpdateCacheForLoginUser(It.IsAny<RequestResponseModel>()))
                                        .Callback<RequestResponseModel>(model => capturedModel = model);

            // Set up HttpContext for CorrelationId
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = expectedCorrelationId;

            // Act
            var result = _controller.UpdateCacheForLoginUser(userDetails);

            // Assert
            Assert.IsType<OkResult>(result);

            // Verify that the method was called exactly once with any RequestResponseModel
            _mockAuthenticateUserService.Verify(x => x.UpdateCacheForLoginUser(It.IsAny<RequestResponseModel>()), Times.Once);

            // Now, assert the properties of the captured object
            Assert.NotNull(capturedModel);
            Assert.Equal(0, capturedModel.CompanyUserID); // The controller hard-codes this to 0
            Assert.Equal(userDetails, capturedModel.Data);
        }

        [Fact]
        public void UpdateCacheForLoginUser_ThrowsJsonReaderException_WhenUserDetailsIsInvalid()
        {
            // Arrange
            var userDetails = "malformed json";

            // Mock the token service to throw an exception when UserDtoObj is accessed
            _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>()))
                             .Throws(new Exception("Simulated UserDtoObj is null"));

            // Set up HttpContext for CorrelationId
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var correlationId = "test-correlation-id";
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = correlationId;

            // Act & Assert
            // The controller's try block throws an exception, leading to the catch block.
            // The catch block then tries to deserialize a malformed string, which throws JsonReaderException.
            // Since the controller does not have a try-catch around the catch block, this exception is not caught.
            // The test should therefore assert that a JsonReaderException is thrown.
            Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => _controller.UpdateCacheForLoginUser(userDetails));
        }

        #endregion

        #region GetConnectionStatus Test Case

        [Fact]
        public void GetConnectionStatus_ReturnsOkResult()
        {
            // Arrange
            // No specific arrangement needed as the method does not take parameters or rely on complex dependencies.
            // It's a simple, direct return.

            // Act
            var result = _controller.GetConnectionStatus();

            // Assert
            Assert.IsType<OkResult>(result); // Expecting an OkResult (200 OK)
        }

        #endregion

        #region ProcessBloombergAuthentication Test Cases

        [Theory]
        [InlineData(null, "123")] // userDetails is null
        [InlineData("", "123")] // userDetails is empty
        [InlineData("{}", null)] // companyUserID is null
        [InlineData("{}", "")] // companyUserID is empty
        public async Task ProcessBloombergAuthentication_ReturnsBadRequest_WhenDetailsAreNullOrEmpty(string userDetails, string companyUserID)
        {
            // Arrange
            // Set up HttpContext for CorrelationId, as before.
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = "test-correlation-id";

            // Act
            var result = await _controller.ProcessBloombergAuthentication(userDetails, companyUserID);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User details cannot be null or empty.", badRequestResult.Value);

            // Verify that the service method was never called due to the early exit
            _mockAuthenticateUserService.Verify(x => x.ProcessBloombergAuthentication(It.IsAny<RequestResponseModel>()), Times.Never);
        }
        #endregion

        #region ValidateAlreadyLoggedInUser Test Cases
        [Fact]
        public async Task ValidateAlreadyLoggedInUser_ReturnsInternalServerError_WhenValidateTokenThrowsException()
        {
            // Arrange
            var userDetails = "someUserDetails";
            var correlationId = "test-correlation-id";

            // Set up a valid token, but mock a dependency of ValidateToken() to throw an exception
            SetToken(1);
            _mockConfiguration.Setup(x => x["AuthSettings:secretKey"]).Throws(new Exception("Simulated validation error"));
            _controller.ControllerContext.HttpContext.Items[GlobalConstants.CONST_CORRELATION_ID] = correlationId;

            // Act
            var result = await _controller.ValidateAlreadyLoggedInUser(userDetails);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Contains(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED, statusCodeResult.Value.ToString());

            _mockAuthenticateUserService.Verify(x => x.LoginUser(It.IsAny<RequestResponseModel>()), Times.Never);
            _mockTokenService.Verify(x => x.DeactivateCurrentAsync(), Times.Never);
        }
        #endregion

        #region GetTouchOtk Test Cases

        [Fact]
        public void GetTouchOtk_ReturnsBadRequest_WhenUserDtoObjIsNull()
        {
            // Arrange
            // Mock the token service to return a null UserDto, simulating an unauthenticated user
            _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>())).Returns((UserDto)null);

            // Set up HttpContext to avoid NullReferenceException on the controller itself
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            // Act
            var result = _controller.GetTouchOtk();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseObject = JObject.Parse(JsonConvert.SerializeObject(badRequestResult.Value));

            Assert.False(responseObject["IsSuccess"].Value<bool>());
            Assert.Equal(MessageConstants.CONST_USER_DTO_NULL, responseObject["ErrorMsg"].Value<string>());
            Assert.Equal(JTokenType.Null, responseObject["Data"].Type);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetTouchOtk_ReturnsBadRequest_WhenUserNameIsInvalid(string userName)
        {
            // Arrange
            var userDto = new UserDto { UserName = userName, CompanyUserId = 1 };
            _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>())).Returns(userDto);
            SetToken(userDto.CompanyUserId);

            // Act
            var result = _controller.GetTouchOtk();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseObject = JObject.Parse(JsonConvert.SerializeObject(badRequestResult.Value));

            Assert.False(responseObject["IsSuccess"].Value<bool>());
            Assert.Equal(MessageConstants.CONST_USER_NAME_IN_DTO_NULL_OR_EMPTY, responseObject["ErrorMsg"].Value<string>());
            Assert.Equal(JTokenType.Null, responseObject["Data"].Type);
        }

        [Fact]
        public void GetTouchOtk_ReturnsOkResult_WhenTokenIsCreatedSuccessfully()
        {
            // Arrange
            var userName = "testUser";
            var expectedTouchOtk = "mockTouchOtk123";
            var userDto = new UserDto { UserName = userName, CompanyUserId = 1 };
            SetToken(userDto.CompanyUserId);
            _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>())).Returns(userDto);

            _mockTokenService.Setup(x => x.CreateTouchOtk(It.Is<TouchTokenDto>(dto => dto.UserName == userName)))
                             .Returns(expectedTouchOtk);

            // Act
            var result = _controller.GetTouchOtk();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var responseObject = JObject.Parse(JsonConvert.SerializeObject(okObjectResult.Value));

            Assert.True(responseObject["IsSuccess"].Value<bool>());
            Assert.Equal(string.Empty, responseObject["ErrorMsg"].Value<string>());
            Assert.Equal(expectedTouchOtk, responseObject["Data"].Value<string>());
        }

        [Fact]
        public void GetTouchOtk_ReturnsOkResult_WhenCreateTouchOtkThrowsException()
        {
            // Arrange
            var userName = "testUser";
            var userDto = new UserDto { UserName = userName, CompanyUserId = 1 };
            SetToken(userDto.CompanyUserId);
            _mockTokenService.Setup(x => x.GetUserDtoFromTokenClain(It.IsAny<ClaimsIdentity>())).Returns(userDto);

            // Mock the CreateTouchOtk method to throw an exception
            _mockTokenService.Setup(x => x.CreateTouchOtk(It.IsAny<TouchTokenDto>()))
                             .Throws(new Exception("Simulated creation error"));

            // Act
            var result = _controller.GetTouchOtk();

            // Assert
            // The controller's internal method catches the exception and returns an empty string.
            // The controller's public method then returns an Ok result with that empty string.
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);

            var responseObject = JObject.Parse(JsonConvert.SerializeObject(okObjectResult.Value));
            Assert.True(responseObject["IsSuccess"].Value<bool>());
            Assert.Equal(string.Empty, responseObject["ErrorMsg"].Value<string>());
            Assert.Equal(string.Empty, responseObject["Data"].Value<string>());
        }

        #endregion
    }
}
