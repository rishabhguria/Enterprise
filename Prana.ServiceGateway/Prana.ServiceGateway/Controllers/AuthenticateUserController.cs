using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using static Prana.ServiceGateway.Constants.ControllerMethodConstants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using Prana.ServiceGateway.CacheStore;
using Serilog.Context;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Models.RequestDto;
using System.Text.Json;
using Prana.ServiceGateway.Utility.CustomAttributes;

namespace Prana.ServiceGateway.Controllers
{
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_Auth_Name, ServiceNameConstants.CONST_Auth_DisplayName)]
    [ApiController]
    public class AuthenticateUserController : BaseController
    {
        private readonly IAuthenticateUserService _authenticateUserService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateUserController> _logger;
        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(1200) };

        /// <summary>
        /// Constructor for API Controller including injection of parameters
        /// </summary>
        /// <param name="validationHelper"></param>
        /// <param name="tokenService"></param>
        /// <param name="authenticateUserService"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// 
        public AuthenticateUserController(IValidationHelper validationHelper,
            ITokenService tokenService,
            IAuthenticateUserService authenticateUserService,
            IConfiguration configuration,
            ILogger<AuthenticateUserController> logger)
            : base(validationHelper, tokenService)
        {
            _configuration = configuration;
            this._logger = logger;
            _authenticateUserService = authenticateUserService;
        }

        /// <summary>
        /// This endpoint is used to Authenticate User
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {
        ///       "userName": "string",
        ///       "password": "string"
        ///     }
        /// </remarks>

        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "{\"CompanyUserId\":28,\"ErrorMessage\":\"\",\"token\":\"\"}"
        /// 
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Incorrect Credentials"
        ///     
        ///     "Username or Password cannot be Blank"
        ///     
        ///     "Support1 is already logged in to Web."
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_LOGINUSER)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(AuthenticationRequestDto authenticationRequestDto)
        {
            if (authenticationRequestDto == null ||
                (string.IsNullOrEmpty(authenticationRequestDto.UserName) &&
                 string.IsNullOrWhiteSpace(authenticationRequestDto.MsalToken)))
            {
                var msg = "Both Username and samsaraAzureId is blank or null or AuthenticationRequestDto is null";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }

            //Try block to handle the password encryption if request comes from swagger .
            try
            {

                string userAgent = Request.Headers[GlobalConstants.SWAGGER_CLIENT].ToString();
                if (IsSwaggerClient(userAgent))
                {
                    _logger.LogTrace("Received Request from Swagger");
                    authenticationRequestDto.Password = _authenticateUserService.EncrytPassword(Convert.ToString(authenticationRequestDto.Password));
                }
                else
                {
                    ParsePasswordIfJsonArray(authenticationRequestDto);  //We recevied password as ascii int array[] from client request. We need to set as int[] in dto as password is object type (string in swagger and int[] in client reqiest)
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, MessageConstants.MSG_CONST_SWAGGER_LOGIN_REQUEST_ERROR);
            }

            authenticationRequestDto.ClientIds = _configuration[UtilityConstants.CONST_CLIENT_CLIENT_ID];
            authenticationRequestDto.Issuers = _configuration[UtilityConstants.CONST_ISSUERS];

            var settings = SpecificCamelCaseResolver.GetSettingsForType<AuthenticationRequestDto>();
            var userDetails = JsonConvert.SerializeObject(authenticationRequestDto, settings);

            string message = await _authenticateUserService.LoginUser(
                new RequestResponseModel(0, userDetails, CorrelationId));

            if (message != null)
            {
                LoginUserDto data = JsonConvert.DeserializeObject<LoginUserDto>(message);
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.ErrorMessage?.ToString()))
                    {
                        _logger.LogDebug("Login request failed with error: {0}", data.ErrorMessage.ToString());
                        return BadRequest(JsonConvert.SerializeObject(data));
                    }
                    if (data.CompanyUserId > 0)
                    {
                        using (LogContext.PushProperty(LogConstant.USER_ID, data.CompanyUserId))
                        {
                            var userdetailsDto = JsonConvert.DeserializeObject<UserdetailsDto>(userDetails);
                            string usrname = userdetailsDto.userName;
                            string webAzureId = GetUserSamsaraAzureId(authenticationRequestDto.MsalToken);
                            string commonUserName = "";
                            if (!string.IsNullOrEmpty(webAzureId))
                                commonUserName = webAzureId;
                            else
                                commonUserName = usrname;

                            UserDto userDto = new()
                            {
                                CompanyUserId = Convert.ToInt32(data.CompanyUserId),
                                UserName = commonUserName
                            };
                            data.token = _tokenService.CreateToken(userDto);
                            data.TouchUrl = _configuration[ControllerMethodConstants.CONST_TOUCH_BASE_URL];

                            //CorrelationId for Touch Authentication
                            data.CorrelationId = CorrelationId;

                            var userDetail = new
                            {
                                companyUserId = data.CompanyUserId,
                                token = data.token,
                                userName = usrname
                            };

                            _logger.LogInformation("Login is successfull for user {userId}", data.CompanyUserId);

                            //Logger.LogMessage(MessageConstants.CONST_LOGIN_SUCCESS + data.CompanyUserId, LogEnums.LogPolicy.LogInformation);

                            PermissionManager.GetInstance().UpdateCacheOnLoginUser(data.CompanyUser);

                            _logger.LogInformation("Updating cache for loginUser {userId}", data.CompanyUserId);
                            _authenticateUserService.UpdateCacheForLoginUser(new RequestResponseModel(0, JsonConvert.SerializeObject(userDetail), CorrelationId));

                            return Ok(JsonConvert.SerializeObject(data));
                        }
                    }
                    else if (data.AuthenticationType == (int)AuthenticationTypes.InvalidCredentials)
                    {
                        data.ErrorMessage = MessageConstants.MSG_INCORRECT_CREDENTIALS;
                        return BadRequest(JsonConvert.SerializeObject(data));
                    }
                }
            }
            return StatusCode(500, MessageConstants.MSG_CONST_INTERNAL_SERVER_ERROR);
        }

        /// <summary>
        /// this is used for getting the running status of the services(auth,trade,gateway)
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(CONST_METHOD_GETSTATUSFRLOGIN)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatusForLogin(string userDetails)
        {
            string message = await _authenticateUserService.GetStatusForLogin(new RequestResponseModel(0, userDetails, CorrelationId));

            if (message != null)
            {
                StatusForLoginDto data = JsonConvert.DeserializeObject<StatusForLoginDto>(message);
                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.ErrorMessage.ToString()))
                    {
                        _logger.LogDebug("GetStatusForLogin request failed with error: {0}", data.ErrorMessage.ToString());
                        return BadRequest(JsonConvert.SerializeObject(data));
                    }
                    return Ok(JsonConvert.SerializeObject(data));
                }
            }
            return StatusCode(500, MessageConstants.MSG_CONST_INTERNAL_SERVER_ERROR);
        }

        /// <summary>
        /// This endpoint is used to Logout User
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "CompanyUserId 17 has been logged out successfully"
        /// 
        ///     "Invalid User or you have been already logged out"
        ///     
        /// </response>
        /// 
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">        
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_LOGOUTUSER)]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutUser()
        {
            string jwtToken = await GetJwtToken();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken);
                var tokenS = jsonToken as JwtSecurityToken;
                var companyUserId = tokenS.Claims.First(claim => claim.Type == ControllerMethodConstants.CONST_COMPANY_USER_ID).Value;

                if (int.Parse(companyUserId) == 0)
                {
                    _logger.LogInformation("Logout request failed with error:{0}", MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }

                string message = await _authenticateUserService.LogoutUser(new(int.Parse(companyUserId), string.Empty, CorrelationId));
                //await _tokenService.DeactivateCurrentAsync();

                PermissionManager.GetInstance().UpdateCacheOnLogoutUser(int.Parse(companyUserId));
                _logger.LogInformation("Removing user from rtpnl updates list on logout");
                HubClientConnectionManager.RemoveUserFromRTPNLUpdatesList(int.Parse(companyUserId));
                return Ok(message);
            }
            return Unauthorized(MessageConstants.MSG_CONST_UNAUTHORIZED);

        }

        /// <summary>
        /// This endpoint is used to validate the JWT Token
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Token is valid"
        ///     
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Token is Invalid"
        ///    
        /// </response>
        ///  <response code="401">        
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_VALIDATETOKEN)]
        public async Task<IActionResult> ValidateToken()
        {
            string jwtToken = await GetJwtToken();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = HelperFunctions.GetTokenValitionParam(_configuration);

                IPrincipal principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

                return Ok(MessageConstants.MSG_CONST_VALID_TOKEN);
            }

            return BadRequest(MessageConstants.MSG_CONST_INVALID_TOKEN);
        }

        /// <summary>
        /// GetJwtToken
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<string> GetJwtToken()
        {
            var authorization = Request.Headers[HeaderNames.Authorization];
            if (Request.Headers[GlobalConstants.CONST_TOKEN].Count > 0)
                return Request.Headers[GlobalConstants.CONST_TOKEN];
            else if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var scheme = headerValue.Scheme;
                var parameter = headerValue.Parameter;
                return parameter;
            }
            else
                return await HttpContext.GetTokenAsync(GlobalConstants.CONST_BEARER, GlobalConstants.CONST_ACCESS_TOKEN) ?? string.Empty;
        }

        /// <summary>
        /// This endpoint is used to Logout User from Enterprise if same user try to login from Web
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "CompanyUserId 17 has been logged out successfully"
        /// 
        ///     "Invalid User or you have been already logged out"
        ///     
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_FORCELOGOUTUSER)]
        [AllowAnonymous]
        public async Task<IActionResult> ForceLogoutUser(string responseData)
        {
            LoginUserDto data = JsonConvert.DeserializeObject<LoginUserDto>(responseData);
            int companyUserId = data.CompanyUserId;

            await _authenticateUserService.LogoutUser(new(companyUserId, responseData, CorrelationId));
            return Ok();
        }

        /// <summary>
        /// This endpoint is used to update the cache of login user
        /// </summary>
        /// <response code="200">
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "An error occurred. Please try again..."
        ///     
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_UPDATECACHEFORLOGINUSER)]
        [AllowAnonymous]
        public IActionResult UpdateCacheForLoginUser(string userDetails)
        {
            string companyUserId = string.Empty;

            try
            {
                companyUserId = UserDtoObj.CompanyUserId.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in UpdateCacheForLoginUser for userId {userId} & error {err}. Ignoring this error and deserializing again userDetails object", companyUserId, ex.Message);
                dynamic data = (JObject)JsonConvert.DeserializeObject<dynamic>(userDetails);
                companyUserId = data.companyUserId;
            }
            if (!string.IsNullOrEmpty(companyUserId))
            {
                // TODO: This code area get hits multiple times. Need to refactor in #39817
                RequestResponseModel model = new(0, userDetails, CorrelationId);
                using (LogKafkaId(model))
                    _authenticateUserService.UpdateCacheForLoginUser(model);
            }
            else
            {
                _logger.LogCritical("Enable to UpdateCacheForLoginUser as userDetails is null/blank, userDetails Obj:{obj} ", userDetails);
            }
            return Ok();
        }

        /// <summary>
        /// This endpoint is used to get the connection status
        /// </summary>
        /// <response code="200">
        /// </response>
        /// <response code="401">        
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_GETCONNECTIONSTATUS)]
        [AllowAnonymous]
        public IActionResult GetConnectionStatus()
        {
            return Ok();
        }

        /// <summary>
        /// This endpoint is used to validate the already loogedin user
        /// </summary>
        /// <response code="200">  
        /// </response>
        ///  <response code="401">        
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_VALIDATEALREADYLOGGEDINUSER)]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateAlreadyLoggedInUser(string userDetails)
        {
            try
            {
                IActionResult validateTokenResult = await ValidateToken();
                if (validateTokenResult is BadRequestResult)
                {
                    return BadRequest(MessageConstants.MSG_CONST_ERROR);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ValidateAlreadyLoggedInUser() for userDetails {userDet}", userDetails);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.MSG_CONST_AN_ERROR_OCCURRED);
            }

            string message = string.Empty;
            var companyUserId = UserDtoObj.CompanyUserId.ToString();

            if (!string.IsNullOrEmpty(companyUserId))
            {
                message = await _authenticateUserService.LoginUser(new(0, userDetails, CorrelationId));
                LoginUserDto data = JsonConvert.DeserializeObject<LoginUserDto>(message);

                UserValidationInfo UserValidationInfo = new()
                {
                    message = message
                };

                if (data.AuthenticationType == (int)AuthenticationTypes.EnterpriseLoggedIn || data.AuthenticationType == (int)AuthenticationTypes.WebAlreadyLoggedInForAnotherWebSession)
                {
                    UserValidationInfo.status = true;
                    await _tokenService.DeactivateCurrentAsync();
                    return Ok(JsonConvert.SerializeObject(UserValidationInfo));
                }
                else
                {
                    PermissionManager.GetInstance().UpdateCacheOnLoginUser(data.CompanyUser);
                    UserValidationInfo.status = false;
                    return Ok(JsonConvert.SerializeObject(UserValidationInfo));
                }
            }
            return BadRequest(MessageConstants.MSG_CONST_ERROR);
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_BLOOMBERG_TOKEN)]
        [AllowAnonymous]
        public async Task<IActionResult> ProcessBloombergAuthentication(string userDetails, string companyUserID)
        {
            if (string.IsNullOrEmpty(userDetails) || string.IsNullOrEmpty(companyUserID))
            {
                return BadRequest("User details cannot be null or empty.");
            }
            //Need to send CompanyUserId in userDetails
            await Task.Run(() => _authenticateUserService.ProcessBloombergAuthentication(new RequestResponseModel(Int32.Parse(companyUserID), userDetails, CorrelationId)));
            return Ok();
        }

        /// <summary>
        /// This endpoint is used to get the touch otk
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_TOUCH_OTK)]
        public IActionResult GetTouchOtk()
        {
            // Null check for UserDtoObj
            if (UserDtoObj == null)
            {
                _logger.LogWarning(MessageConstants.CONST_USER_DTO_NULL);
                return BadRequest(new
                {
                    IsSuccess = false,
                    ErrorMsg = MessageConstants.CONST_USER_DTO_NULL,
                    Data = (string)null
                });
            }
            string userName = UserDtoObj.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogWarning(MessageConstants.CONST_USER_NAME_IN_DTO_NULL_OR_EMPTY);
                return BadRequest(new
                {
                    IsSuccess = false,
                    ErrorMsg = MessageConstants.CONST_USER_NAME_IN_DTO_NULL_OR_EMPTY,
                    Data = (string)null
                });
            }

            try
            {
                var touchOtk = CreateTouchOtk(userName);
                return Ok(new
                {
                    IsSuccess = true,
                    ErrorMsg = string.Empty,
                    Data = touchOtk
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageConstants.CONST_ERROR_IN_GET_TOUCH_ACCESS_TOKEN, userName);
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    ErrorMsg = MessageConstants.CONST_ERROR_IN_GET_TOUCH_ACCESS_TOKEN,
                    Data = (string)null
                });
            }
        }

        /// <summary>
        /// Creates the touch one time key.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string CreateTouchOtk(string userName)
        {
            try
            {
                var touchTokenDto = new TouchTokenDto(userName);
                var touchOtk = _tokenService.CreateTouchOtk(touchTokenDto);

                _logger.LogInformation(MessageConstants.CONST_TOUCH_OTK_CREATION_SUCCESS, touchTokenDto.UserName);

                return touchOtk;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessageConstants.CONST_TOUCH_OTK_CREATION_ERROR, userName);
                return string.Empty;
            }
        }

        private bool IsSwaggerClient(string userAgent)
        {
            return !string.IsNullOrEmpty(userAgent) && userAgent.Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }

        private void ParsePasswordIfJsonArray(AuthenticationRequestDto dto)
        {
            if (dto.Password is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
            {
                dto.Password = jsonElement.EnumerateArray()
                                          .Select(e => e.GetInt32())
                                          .ToArray();
            }
        }
        /// <summary>
        /// Get SamsaraAzureId from msalToken
        /// </summary>
        /// <param name="msalToken"></param>
        /// <returns></returns>
        private string GetUserSamsaraAzureId(string msalToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(msalToken);
                string samsaraAzureId = jwt.Claims
                    .FirstOrDefault(c => c.Type == "preferred_username")?
                    .Value;
                _logger.LogInformation("SamsaraAzureId from msalToken is {0}", samsaraAzureId);
                if (string.IsNullOrWhiteSpace(samsaraAzureId))
                    _logger.LogInformation("Unable to fetch user details (samsaraAzureId) from MSAL token. Token may be malformed or missing required claims");
                return samsaraAzureId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while extracting Samsara Azure ID from MSAL token");
                return null;
            }
        }
    }
}
