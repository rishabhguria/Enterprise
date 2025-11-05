using Prana.Allocation.Common.Constants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Diagnostics;
using System.Text;

namespace Prana.Allocation.Common.Helper
{
    public static class AllocationLoggingHelper
    {
        /// <summary>
        /// Loggers the write send to server.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="message">The message.</param>
        public static void LoggerWriteMessage(string component, string message)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey(AllocationLoggingConstants.IS_ALLOCATION_LOGGING_REQUIRED)))
                {
                    StringBuilder sb = new StringBuilder(DateTime.Now + ": ");
                    sb.Append(component + message);
                    sb.Append(Environment.NewLine);
                    Logger.LoggerWrite(sb, AllocationLoggingConstants.ALLOCATION_LOGGING, 1, 1, TraceEventType.Information);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Loggers the write Message.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="message">The message.</param>
        public static void LoggerWriteMessage(string message)
        {
            try
            {
                StringBuilder sb = new StringBuilder(DateTime.Now + ":");
                sb.Append(Environment.NewLine);
                sb.Append(message);
                sb.Append(Environment.NewLine);
                Logger.LoggerWrite(sb, AllocationLoggingConstants.ALLOCATION_RESPONSE_LOGGING, 1, 1, TraceEventType.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }



    }
}
