using Prana.LogManager;
using Prana.WCFConnectionMgr;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static Prana.LogManager.LoggingConstants.SerilogConstant;

namespace Prana.LogManager.SerilogLibrary
{
    public class SerilogLibraryLogging : ILog
    {
        private static LoggingLevelSwitch _levelSwitch = new LoggingLevelSwitch();
        private const string OUTPUT_FORMAT = "serilog:write-to:File.outputTemplate";
        private const string APP_NAME = "serilog:enrich:with-property:AppName";
        private const string FILE_SIZE = "serilog:write-to:File.fileSizeLimitBytes";
        private ILogger _fileLogger = null;

        public void Initialize()
        {
            try
            {
                string msgTemplate = ConfigurationManager.AppSettings[OUTPUT_FORMAT];
                string appName = ConfigurationManager.AppSettings[APP_NAME];
                string fileSize = ConfigurationManager.AppSettings[FILE_SIZE];

                SetMinLevelSwitch();

                Log.Logger = GetLogger(appName, fileSize, msgTemplate);
                _fileLogger = GetLogger(appName, fileSize, msgTemplate, isFileLog: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing logger: {ex.Message}");
            }
        } 

        #region Exception Logging
        public bool HandleException(Exception exp, string policyName)
        {
            Log.Error(exp, "Error");
            return LoggingConstants.POLICY_LOGANDTHROW == policyName;
        }
        #endregion


        #region Message Logging
        public void Write(object message)
        {
            Log.Information(message.ToString());
        }

        public void Write(object message, string category)
        {
            Log.Information(message.ToString());
        }

        public void Write(object message, string category, int priority, int eventId, TraceEventType severity)
        {
            Log.Information(message.ToString());
        }

        public void Write(object message, string category, int priority, int eventId, TraceEventType severity, string title)
        {
            Log.Information(message.ToString());
        }

        public void Write(object message, string category,
            int priority, int eventId, TraceEventType severity,
            string title, IDictionary<string, object> properties)
        {
            Log.Information(message.ToString());
        }

        public IDisposable PushProperty(string name, object value)
        {
            return LogContext.PushProperty(name, value);
        }

        public void LogMsg(LoggerLevel level, string msg, params object[] obj)
        {
            if (level == LoggerLevel.Error)
                Log.Error(msg, obj);

            if (level == LoggerLevel.Debug)
                Log.Debug(msg, obj);

            if (level == LoggerLevel.Information)
                Log.Information(msg, obj);

            if (level == LoggerLevel.Verbose)
                Log.Verbose(msg, obj);

            if (level == LoggerLevel.Fatal)
                Log.Fatal(msg, obj);
        }

        public void LogError(Exception ex, string msg)
        {
            //if (msg == "LogAndShowPolicy")
            //    MessageBox.Show(ServiceModelHelper.ReturnExceptionMessage(ex), "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Log.Error(ex, msg);
        }

        public void LogToFile(string msg)
        {
            _fileLogger.Information(msg);
        }

        public string GetFlatFilelistnerLogFileName(string flatFileListnerName)
        {
            throw new NotImplementedException();
        }
        #endregion


        private void SetMinLevelSwitch()
        {
            string minLevel = ConfigurationManager.AppSettings["serilog:minimum-level"];

            LogEventLevel newLevel = LogEventLevel.Information;
            if (Enum.TryParse(minLevel, true, out newLevel))
                _levelSwitch.MinimumLevel = newLevel;

            SerilogUtility.WatchForConfigChanges(_levelSwitch);
        }

        private ILogger GetLogger(string appName,
           string fileSize,
           string messageTemplate,
           bool isFileLog = false)
        {
            long maxFileSizeInBytes = long.Parse(fileSize);

            var baseDir = AppDomain.CurrentDomain.BaseDirectory; 
            var logsDir = Path.Combine(baseDir, $"{appName}_Logs");
            Directory.CreateDirectory(logsDir); // optional; Serilog will also create it

            var logger = new LoggerConfiguration()
                    .WriteTo.Map(
                        evt =>
                        {
                            var level = evt.Level.ToString();

                            string fileNameSuffix = null;
                            if (evt.Properties.TryGetValue(LoggingConstants.PROPERTY_FILE_NAME, out var propValue))
                                fileNameSuffix = propValue.ToString().Replace("\"", "");

                            return fileNameSuffix != null && isFileLog
                                ? $"{fileNameSuffix}"
                                : level;
                        },
                        (mappedKey, wt) => wt
                            .File(
                                path: Path.Combine(logsDir, $"{appName}-{mappedKey}-.log"),
                                outputTemplate: messageTemplate,
                                fileSizeLimitBytes: maxFileSizeInBytes,
                                rollingInterval: RollingInterval.Day,
                                rollOnFileSizeLimit: true
                            )
                    )
                    .Enrich.FromLogContext()
                    .MinimumLevel.ControlledBy(_levelSwitch);


            return isFileLog ?
                logger.CreateLogger() :
                logger.ReadFrom.AppSettings().CreateLogger();
        }
    }

}
