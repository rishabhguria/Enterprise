using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.LogManager
{
    public interface ILog
    {
        #region Initialize
        void Initialize();
        #endregion

        #region Exception Logging
        bool HandleException(Exception exp, string policyName);
        #endregion

        #region Message Logging
        void Write(object message);

        void Write(object message, string category);

        void Write(object message, string category, int priority, int eventId, TraceEventType severity);

        void Write(object message, string category, int priority, int eventId, TraceEventType severity, string title);

        void Write(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties);
        #endregion

        #region logging configuration
        string GetFlatFilelistnerLogFileName(string flatFileListnerName);
        #endregion
        IDisposable PushProperty(string name, object value);
        void LogMsg(LoggerLevel level, string msg, params object[] obj);
        void LogError(Exception ex, string msg);

        void LogToFile(string msg);    
    }
    public enum LoggerLevel
    {
        Verbose = 0,
        Debug = 1,
        Information = 2,
        Fatal = 3,
        Error = 4
    }
}