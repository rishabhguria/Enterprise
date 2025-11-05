using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;

namespace BusinessObjects
{
    /// <summary>
    /// To log messages and exceptions
    /// </summary>
    public  static class Logger
    {
        public static event EventHandler<string> messageLogged;
        static Logger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Messages.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 2000000))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Exception.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 2000000))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Warning.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, fileSizeLimitBytes: 2000000))
                .CreateLogger();
                
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void LogError(Exception ex)
        {
            try
            {
                Log.Error(ex, string.Empty);
                if (messageLogged != null)
                    messageLogged(null, DateTime.Now + " : " + ex.Message);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void LogWarning(string msg)
        {
            try
            {
                Log.Warning(msg);
                if (messageLogged != null)
                    messageLogged(null, DateTime.Now + " : " + msg);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public static void LogMessage(string msg)
        {
            try
            {
                Log.Information(msg);
                if (messageLogged != null)
                    messageLogged(null, DateTime.Now + " : " + msg);
            }
            catch (Exception)
            { }
        }
    }
}
