using Microsoft.AspNetCore.Mvc;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;
using Serilog.Context;
using Microsoft.AspNetCore.Authorization;

namespace Prana.ServiceGateway.Controllers
{
    [Route(ControllerMethodConstants.CONST_CONTROLLER_ACTION)]
    [ApiController]
    public class LoggingController : ControllerBase
    {

        /// <summary>
        /// Used for Logging
        /// </summary>
        /// <value>Injected Value of LoggingHelper</value>
        private readonly ILoggingHelper _loggingHelper;

        /// <summary>
        /// Constructor for API Controller including injection of parameters
        /// </summary>
        /// <param name="loggingHelper">ILoggingHelper</param>        
        /// <inheritdoc />
        public LoggingController(ILoggingHelper loggingHelper)
        {
            _loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This endpoint is used to Log Data from Frontend Application
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {
        ///       "level": int,
        ///       "message": "string"
        ///     }
        /// </remarks>
        /// <response code="200">
        /// 
        /// Returns void
        /// </response>        
        [HttpPost]
        [AllowAnonymous]
        public void PostLogs([FromBody] LoggerInfo loggerInfo)
        {
            try
            {
                using var pp = LogContext.PushProperty(LogConstant.APP_NAME, LogConstant.NIRVANA_WEB_APP);
                using var pp1 = LogContext.PushProperty(LogConstant.LOG_TYPE, LogConstant.NIRVANA_WEB_APP);
                {
                    if (string.IsNullOrEmpty(loggerInfo.userId))
                        _loggingHelper.LogMessage(loggerInfo);
                    else
                    {
                        using var pp3 = LogContext.PushProperty(LogConstant.USER_ID, loggerInfo.userId);
                        _loggingHelper.LogMessage(loggerInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
            }
        }

    }
}
