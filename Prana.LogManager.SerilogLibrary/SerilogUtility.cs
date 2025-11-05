using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.IO;
using static Prana.LogManager.LoggingConstants.SerilogConstant;

namespace Prana.LogManager.SerilogLibrary
{
    public class SerilogUtility
    {
        private static LoggingLevelSwitch _levelSwitch;

        public static void WatchForConfigChanges(LoggingLevelSwitch levelSwitch)
        {
            _levelSwitch = levelSwitch;
            string configFilePath = string.Empty;
            try
            {
                string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
                configFilePath = Path.Combine(binDirectory, SERILOG_LEVEL_FILE_NAME);

                if (!File.Exists(configFilePath))
                {
                    Log.Warning("No logLevel file is found for dynamic logLevel changes. The file is json and it path is {}.", configFilePath);
                    return;
                }

                var watcher = new System.IO.FileSystemWatcher(Path.GetDirectoryName(configFilePath))
                {
                    Filter = Path.GetFileName(configFilePath),
                    NotifyFilter = NotifyFilters.LastWrite
                };

                watcher.Changed += OnLogLevelFileChanged;
                watcher.EnableRaisingEvents = true;

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error in tracking changes serilogLevel.json for dynamic logLevel changes");
            }
        }

        private static void UpdateLogLevelFromConfig(string configFilePath)
        {
            try
            {
                var json = File.ReadAllText(configFilePath);
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<LogLevelConfig>(json);

                if (Enum.TryParse(config?.LogLevel, true, out LogEventLevel newLevel))
                {
                    _levelSwitch.MinimumLevel = newLevel;
                    Log.Information($"Log level changed to {newLevel} from serilogLogLevel.json file.");
                }
                else
                {
                    Log.Warning($"Invalid log level in JSON config file: {config?.LogLevel}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to read log level from serilogLogLevel.json file. {configFilePath}");
            }
        }

        private static void OnLogLevelFileChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(500);   //wait for some time to have a file write.
            UpdateLogLevelFromConfig(e.FullPath);
        }

        private class LogLevelConfig
        {
            public string LogLevel { get; set; }
        }
    }

}
