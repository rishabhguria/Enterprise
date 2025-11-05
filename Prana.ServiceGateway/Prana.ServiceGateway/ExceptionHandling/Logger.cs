using Prana.ServiceGateway.Constants;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Prana.ServiceGateway.ExceptionHandling
{
    public static class Logger
    {
       
        private static ILogger _consoleLogger = null;

        private static ILogger _fileLogger = null;

        public static ILogger SetupFileLogger(WebApplicationBuilder builder)
        {
            _fileLogger = new LoggerConfiguration()
                   .ReadFrom.Configuration(builder.Configuration)
                   .CreateLogger();
            return _fileLogger;
        }

        public static ILogger SetupConsoleLogger(WebApplicationBuilder builder)
        {
            _consoleLogger = new LoggerConfiguration()
                   .WriteTo.Console()
                   .CreateLogger();
            return _consoleLogger;
        }

        public static void LogMessage(Exception ex, LogEnums.LogPolicy policyName)
        {
            if(_fileLogger != null)
            	HandleException(ex, policyName);
        }

        public static void LogMessage(string message, LogEnums.LogPolicy policyName)
        {
            LoggerWrite(message, policyName);
        }

        private static void HandleException(Exception ex, LogEnums.LogPolicy policyName)
        {
            try
            {
                switch (policyName)
                {
                    case LogEnums.LogPolicy.LogAndShow:
                        _fileLogger.Error(ex, ex.Message);
                        _consoleLogger.Error(ex, ex.Message);
                        break;
                    case LogEnums.LogPolicy.LogError:
                        _fileLogger.Error(ex, ex.Message);
                        break;
                    case LogEnums.LogPolicy.LogInformation:
                        _fileLogger.Information(ex, ex.Message);
                        break;
                    case LogEnums.LogPolicy.LogWarning:
                        _fileLogger.Warning(ex, ex.Message);
                        break;
                    case LogEnums.LogPolicy.LogVerbose:
                        _fileLogger.Verbose(ex, ex.Message);
                        break;
                    case LogEnums.LogPolicy.LogFatal:
                        _fileLogger.Fatal(ex, ex.Message);
                        break;
                    default:
                        _fileLogger.Error(ex, ex.Message);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " \n " + e.StackTrace);
            }
        }
        private static void LoggerWrite(string message, LogEnums.LogPolicy policyName)
        {
            Log.Information(message);    //this method will be rmeoved in future
        }
    }
}
