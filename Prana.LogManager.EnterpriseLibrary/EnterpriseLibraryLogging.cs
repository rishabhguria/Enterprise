using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.LogManager.EnterpriseLibrary
{
    public class EnterpriseLibraryLogging : ILog
    {
        #region Initialize
        public void Initialize()
        {
            IConfigurationSource config = ConfigurationSourceFactory.Create();
            ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);

            var logwriterFactory = new LogWriterFactory(config);
            var logWriter = logwriterFactory.Create();
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logWriter);

            ExceptionManager exManager = factory.CreateManager();
            ExceptionPolicy.SetExceptionManager(exManager);
        }
        #endregion

        #region Exception Logging
        public bool HandleException(Exception exp, string policyName)
        {
            return ExceptionPolicy.HandleException(exp, policyName);
        }
        #endregion

        public string GetFlatFilelistnerLogFileName(string flatFileListnerName)
        {
            string logFileName = string.Empty;

            try
            {
                LoggingSettings loggingSettings = LoggingSettings.GetLoggingSettings(new FileConfigurationSource(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

                RollingFlatFileTraceListenerData eachFlatFileTracelistner = loggingSettings.TraceListeners.Get(flatFileListnerName) as RollingFlatFileTraceListenerData;

                if (null != eachFlatFileTracelistner)
                {
                    logFileName = eachFlatFileTracelistner.FileName;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return logFileName;
        }

        #region Message Logging
        public void Write(object message)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
        }

        public void Write(object message,
            string category)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category);
        }

        public void Write(object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity);
        }

        public void Write(object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            string title)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity, title);
        }

        public void Write(object message,
            string category,
            int priority,
            int eventId,
            TraceEventType severity,
            string title,
            IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity, title, properties);
        }

        public void LogMsg(LoggerLevel level,
            string msg,
            params object[] obj)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(msg);
        }

        public IDisposable PushProperty(
            string name,
            object value)
        {
            return new DefaultDispoable();
        }

        public void LogError(
            Exception ex,
            string msg)
        {
            ExceptionPolicy.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
        }

        public void LogToFile(string msg)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(msg);
        }

        #endregion
    }

    
}
