using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Prana.ServiceGateway.ExceptionHandling;
using Serilog.Context;
using Prana.KafkaWrapper;
using Serilog;

namespace Prana.ServiceGateway.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        private static readonly Serilog.ILogger logger = Log.ForContext<BaseController>();
        
        /// <summary>
        /// Gets or sets the validation helper instance.
        /// </summary>
        /// <value>The validation helper instance.</value>
        public IValidationHelper ValidationHelperInstance { get; set; }

        /// <summary>
        ///Token Service
        /// </summary>
        protected readonly ITokenService _tokenService;

        public BaseController(IValidationHelper validationHelper, ITokenService tokenService)
        {
            ValidationHelperInstance = validationHelper;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Currents the user name.
        /// </summary>
        /// <returns>System.String.</returns>
        internal string CurrentUsername()
        {
            try
            {
                string username = Request.Headers[GlobalConstants.UsernameHeaderKey].ToString();
                return username;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CurrentUsername encountered an error");
                return MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message;
            }
        }

        private UserDto userDtoObj;
        protected UserDto UserDtoObj
        {
            get
            {
                if (userDtoObj == null)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    userDtoObj = _tokenService.GetUserDtoFromTokenClain(identity);
                }
                return userDtoObj;
            }
        }

        protected string CorrelationId
            => Request.Headers["CorrelationId"];

        protected IDisposable LogKafkaId(RequestResponseModel model)
        {
            return LogContext.PushProperty(LogConstant.KAFKA_REQUEST_ID, model.RequestID);
        }

        /// <summary>
        /// Validates the parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal void ValidateParameter(string parameterName, object parameterValue)
        {
            try
            {
                if (parameterValue == null || string.IsNullOrEmpty(parameterValue.ToString()))
                {
                    throw new ArgumentNullException(parameterName, $"{parameterName} is required and cannot be null.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ValidateParameter encountered an error");
            }
        }

        /// <summary>
        /// GetStringContentFromJson
        /// </summary>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        private static string GetStringContentFromJson(object jsonContent)
        {
            try
            {
                StringContent content = new(Regex.Escape(JsonConvert.SerializeObject(jsonContent)));

                string result = content.ReadAsStringAsync().Result.Replace("\\r\\n", string.Empty);
                result = Regex.Unescape(result).Trim('"');
                result = result.TrimStart('"');
                result = result.TrimEnd('"');

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "GetStringContentFromJson encountered an error");
                return MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message;
            }
        }

        /// <summary>
        /// GenerateConsoleAndLog
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="methodName"></param>
        /// <param name="executionStartandEnd"></param>
        internal void GenerateConsoleAndLog(string controllerName, string methodName, string executionStartandEnd)
        {
            try
            {
                Logger.LogMessage(MessageConstants.CONST_BRACKET_OPEN + controllerName + MessageConstants.CONST_BRACKET_CLOSE + executionStartandEnd + methodName, LogEnums.LogPolicy.LogInformation);
                Logger.LogMessage(MessageConstants.CONST_LINE_BREAK, LogEnums.LogPolicy.LogInformation);
                Console.WriteLine(MessageConstants.CONST_BRACKET_OPEN + DateTime.Now.ToString(MessageConstants.CONST_DATET_TIME_FORMAT) + MessageConstants.CONST_BRACKET_CLOSE + executionStartandEnd + methodName);
                Console.WriteLine(MessageConstants.CONST_LINE_BREAK);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
            }
        }



    }
}
