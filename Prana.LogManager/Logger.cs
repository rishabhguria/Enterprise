using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using static Prana.LogManager.LoggingConstants.SerilogConstant;

namespace Prana.LogManager
{
    public class Logger
    {
        private static ILog _logger;
        private static bool _isLoggingEnabled = false;

        public static void Initialize(IWindsorContainer container)
        {
            _isLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLoggingEnabled"]);
            _logger = container.Resolve<ILog>();
            _logger.Initialize();
        }

        #region Exception Logging
        public static bool HandleException(
            Exception exp,
            string policyName,
            string errMsg = "")
        {
            var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
            var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);

            // this ensure the serilog logging to be used else enterprise flow.
            if (propertyContext != null && !(propertyContext is DefaultDispoable))
            {
                using (propertyContext)
                    _logger.LogError(exp, $"Error :{errMsg}");
                return LoggingConstants.POLICY_LOGANDTHROW == policyName;
            }
            return _logger.HandleException(exp, policyName);
        }
        #endregion

        #region Message Logging
        public static void LoggerWrite(
            object message,
            bool isOverrideLoggingPrefs = false)
        {
            if (_isLoggingEnabled || isOverrideLoggingPrefs)
            {
                var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
                var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
                if (propertyContext != null && !(propertyContext is DefaultDispoable))
                {
                    using (propertyContext)
                        _logger.LogMsg(LoggerLevel.Information, message.ToString(), null);
                }
                else
                    _logger.Write(message);
            }
        }

        ///remove 1
        public static void LoggerWrite(
            object message,
            string category,
            bool isOverrideLoggingPrefs = false)
        {
            if (_isLoggingEnabled || isOverrideLoggingPrefs)
            {
                var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
                var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
                if (propertyContext != null && !(propertyContext is DefaultDispoable))
                {
                    using (propertyContext)
                        _logger.LogMsg(LoggerLevel.Information, message.ToString(), null);
                }
                else
                    _logger.Write(message, category);
            }
        }

        ///remove 2
        public static void LoggerWrite(
            object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            bool isOverrideLoggingPrefs = false)
        {
            if (_isLoggingEnabled || isOverrideLoggingPrefs)
            {
                var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
                var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
                if (propertyContext != null && !(propertyContext is DefaultDispoable))
                {
                    using (propertyContext)
                        _logger.LogMsg(GetLogLevel(severity), message.ToString(), null);
                }
                else
                    _logger.Write(message, category, priority, eventId, severity);
            }
        }

        //remove 3
        public static void LoggerWrite(
            object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            string title,
            bool isOverrideLoggingPrefs = false)
        {
            if (_isLoggingEnabled || isOverrideLoggingPrefs)
            {
                var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
                var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
                if (propertyContext != null && !(propertyContext is DefaultDispoable))
                {
                    using (propertyContext)
                        _logger.LogMsg(GetLogLevel(severity), message.ToString(), title);
                }
                else
                    _logger.Write(message, category, priority, eventId, severity, title);
            }
        }

        //remove 4
        public static void LoggerWrite(
            object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            string title,
            IDictionary<string, object> properties,
            bool isOverrideLoggingPrefs = false)
        {
            if (_isLoggingEnabled || isOverrideLoggingPrefs)
            {
                var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
                var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
                if (propertyContext != null && !(propertyContext is DefaultDispoable))
                {
                    using (propertyContext)
                        _logger.LogMsg(GetLogLevel(severity), message.ToString(), title);
                }
                else
                    _logger.Write(message, category, priority, eventId, severity, title, properties);
            }
        }
        #endregion

        #region logging configuration
        public static string GetFlatFilelistnerLogFileName(string flatFileListnerName)
        {
            return _logger.GetFlatFilelistnerLogFileName(flatFileListnerName);
        }

        public static void LogMsg(
            LoggerLevel loggerLevel,
            string msg,
            params object[] args)
        {
            var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
            var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);

            if (propertyContext != null && !(propertyContext is DefaultDispoable))
            {
                using (propertyContext)
                    _logger.LogMsg(loggerLevel, msg, args);
            }
            else
                _logger.LogMsg(loggerLevel, msg, args);
        }

        public static void LogError(Exception ex, string errMsg)
        {
            var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
            var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
            if (propertyContext != null && !(propertyContext is DefaultDispoable))
            {
                using (propertyContext)
                    _logger.LogError(ex, errMsg);
            }
            else
                _logger.LogError(ex, errMsg);
        }

        public static void LogToFile(string msg, string fileName = null)
        {
            var callingType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
            var propertyContext = _logger.PushProperty(CUSTOM_SOURCE, callingType.FullName);
            if(propertyContext != null && !(propertyContext is DefaultDispoable))
            {
                using (propertyContext)
                {
                    if (fileName != null)
                    {
                        var propertyContextFileName = _logger.PushProperty(LoggingConstants.PROPERTY_FILE_NAME, fileName);
                        if (propertyContextFileName != null && !(propertyContextFileName is DefaultDispoable))
                        {
                            using (propertyContextFileName)
                                _logger.LogToFile(msg);
                        }
                        else
                            _logger.LogToFile(msg);
                    }
                    else
                        _logger.LogToFile(msg);
                }
            }
            else
                _logger.LogToFile(msg);
        }

        public static IDisposable PushProperty(
            string name,
            object value)
        {
            return _logger.PushProperty(name, value);
        }
        #endregion


        private static LoggerLevel GetLogLevel(TraceEventType severity)
        {
            if (severity == TraceEventType.Error)
                return LoggerLevel.Error;
            if (severity == TraceEventType.Warning)
                return LoggerLevel.Fatal;
            if (severity == TraceEventType.Information)
                return LoggerLevel.Information;
            if (severity == TraceEventType.Verbose)
                return LoggerLevel.Verbose;
            return LoggerLevel.Information;
        }
    }

    public class DefaultDispoable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
