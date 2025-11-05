// ***********************************************************************
// <copyright file="ValidationHelper.cs" company="Nirvana">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Utility
{
    /// <summary>
    /// Class to help with Logging of Frontend Logs
    /// </summary>
    /// <seealso cref="ILoggingHelper" />
    public class LoggingHelper : ILoggingHelper
    {

        private readonly ILogger<LoggingHelper> _logger;

        public LoggingHelper(ILogger<LoggingHelper> logger)
        {
            _logger = logger;
        }


        public void LogMessage(LoggerInfo loggerInfo)
        {
            try
            {
                switch (loggerInfo.level)
                {
                    case (int)LogLevel.Information:
                        _logger.LogInformation(loggerInfo.message);
                        break;
                    case (int)LogLevel.Warning:
                        _logger.LogWarning(loggerInfo.message);
                        break;
                    case (int)LogLevel.Error:
                        _logger.LogError(loggerInfo.message);
                        break;
                    case (int)LogLevel.Trace:
                        _logger.LogTrace(loggerInfo.message);
                        break;
                    case (int)LogLevel.Debug:
                        _logger.LogDebug(loggerInfo.message);
                        break;
                    default:
                        _logger.LogInformation(MessageConstants.MSG_CONST_UNDETERMINED_LOG);
                        _logger.LogInformation(loggerInfo.message);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error in Service");
            }
        }
    }
}
