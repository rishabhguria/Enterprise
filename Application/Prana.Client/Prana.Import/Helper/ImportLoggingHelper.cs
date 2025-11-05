using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Prana.Import.Helper
{
    public static class ImportLoggingHelper
    {
        //Listener Category 
        private const String AUTOIMPORT_LOGGING = "AutoImport_Logging";

        //Import Started Status
        public const String IMPORT_STARTED = "Import Started ";

        //Import Completed Status
        public const String IMPORT_ENDED = "Import Completed ";

        //Filename Constant
        private const String FILENAME = " Filename: ";

        //Status Constant
        private const String STATUS = " Status: ";

        //Exception Occured Constant
        public const String EXCEPTION = "Exception Occured: ";

        //Import Type Constant
        public const String IMPORT_TYPE = " Import Type: ";

        /// <summary>
        /// Loggers the write send to server.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public static void LoggerWriteMessage(string component, string fileName, string status, string message)
        {
            try
            {
                StringBuilder sb = new StringBuilder(DateTime.Now + ": ");
                sb.Append(component + FILENAME + fileName + STATUS + status);
                if (message != string.Empty)
                {
                    sb.Append("(" + message + ")");
                }
                sb.Append(Environment.NewLine);
                Dictionary<string, object> arguments = new Dictionary<string, object>();
                Logger.LoggerWrite(sb, AUTOIMPORT_LOGGING, 1, 1, TraceEventType.Information, "log-entry", arguments);
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
