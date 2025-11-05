using System;
using System.Configuration;

namespace Prana.LogManager
{
    public class LoggingConstants
    {
        #region Listeners

        public const string LISTENER_RollingFlatFileTraceListener = "RollingFlatFileTraceListener";
        public const string LISTENER_RollingWarningsTraceListener = "RollingWarningsTraceListener";
        public const string LISTENER_RollingInformationsTraceListener = "RollingInformationsTraceListener";
        public const string LISTENER_RollingFile_ExceptionListener = "RollingFile ExceptionListener";
        public const string LISTENER_RollingInformationReporterTraceListener = "RollingInformationReporterTraceListener";

        public const string LISTENER_RollingFlatFileLiveDataListener = "RollingFlatFileLiveDataListener";
        public const string LISTENER_RollingFlatFileTraceListenerFactSet = "RollingFlatFileTraceListenerFactSet";
        public const string LISTENER_RollingFlatFileTraceListenerActiv = "RollingFlatFileTraceListenerActiv";
        public const string LISTENER_RollingFlatFileTraceListenerAtdl = "RollingFlatFileTraceListenerAtdl";

        public const string LISTENER_RollingClientMessagesTraceListener = "RollingClientMessagesTraceListener";
        public const string LISTENER_Rollingflat_file_for_exception_handling = "Rollingflat file for exception handling";
        public const string LISTENER_RollingFlatFile_Error_Message_Logging = "RollingFlatFile Error Message Logging";
        public const string LISTENER_RollingReceivedFromCPTraceListener = "RollingReceivedFromCPTraceListener";
        public const string LISTENER_RollingSentToCPTraceListener = "RollingSentToCPTraceListener";
        public const string LISTENER_RollingFlatFileTraceListenerAllocation = "RollingFlatFileTraceListenerAllocation";
        public const string LISTENER_RollingFlatFileTraceListenerGeneralLedger = "RollingFlatFileTraceListenerGeneralLedger";

        public const string LISTENER_FlatFileDestination = "FlatFileDestination";
        public const string LISTENER_RollingFileErrorListener = "RollingFileErrorListener";
        public const string LISTENER_RollingFileVerboseListener = "RollingFileVerboseListener";
        public const string LISTENER_RollingFileConfigListener = "RollingFileConfigListener";
        public const string LISTENER_Flat_File_Destination = "Flat File Destination";
        public const string LISTENER_RollingFile_TraceListener = "RollingFile TraceListener";
        public const string LISTENER_KafkaRollingInformationReporterTraceListener = "KafkaRollingInformationReporterTraceListener";
        #endregion

        #region Policies
        public const string POLICY_LOGANDSOCKET = "LogAndSocketPolicy";
        public const string POLICY_LOGANDTHROW = "LogAndThrowPolicy";
        public const string POLICY_LOGANDSHOW = "LogAndShowPolicy";
        public const string POLICY_SHOWONLY = "ShowOnlyPolicy";
        public const string POLICY_LOGONLY = "LogOnlyPolicy";
        public const string POLICY_EVENTLOGONLYPOLICY = "EventLogOnlyPolicy";
        public const string POLICY_GLOBAL = "Global Policy";
        #endregion

        #region Logging
        public const string LOG_CATEGORY_UI = "Prana.Client";

        public const string LOG_CATEGORY_HARD_CODING = "Warning. Data Hard Coded need to improve. ";
        public const string LOG_CATEGORY_EXCEPTION = "Exception";
        public const string CATEGORY_FLAT_FILE_ERROR_MSG = "Category_FlatFileErrorMessages";
        public const string CATEGORY_FLAT_FILE_EXCEPTION = "Category_ExceptionHandling";
        public const string CATEGORY_FLAT_FILE_SentToCp = "Category_SentToCp";
        public const string CATEGORY_FLAT_FILE_ReceivedFromCP = "Category_ReceivedFromCP";
        public const string CATEGORY_FLAT_FILE_ClientMessages = "Category_ClientMessages";

        public const string CATEGORY_ALLLOG = "Category_AllLog";
        public const string CATEGORY_INFORMATION = "Category_Information";
        public const string CATEGORY_ERROR = "Category_Error";
        public const string CATEGORY_WARNING = "Category_Warning";
        public const string CATEGORY_INFORMATION_REPORTER = "Category_Information_Reporter";
        public const string CATEGORY_ACTIVITYTRACINGANDDEBUGING = "Category_ActivityTracingAndDebuging";
        public const string CATEGORY_CONFIGBASEDLOGGING = "Category_ConfigBasedLogging";
        public const string CATEGORY_FLAT_FILE_TRACING = "Category_Tracing";
        public const string CATEGORY_GENERAL = "General";
        public const string CATEGORY_GENERAL_COMPLIANCE = "General_Client_Compliance";
        public const string CATEGORY_INFORMATION_REPORTER_COMPLIANCE = "Category_Information_Reporter_Compliance";
        public const string CATEGORY_INFORMATION_REPORTER_CLOSINGCORRUPTION = "Category_Information_Reporter_ClosingCorruption";
        public const string RISK_LOGGING = "Risk_Logging";
        public const string LIVEDATA_LOGGING = "LiveData_Logging";
        public const string FACTSET_LOGGING = "FactSet_Logging";
        public const string ACTIV_LOGGING = "Activ_Logging";
        public const string SAPI_LOGGING = "SAPI_Logging";
        public const string ATDL_LOGGING = "Atdl_Logging";
        public const string OptionChain_Logging = "OptionChain_Logging";
        public const string Category_Kafka_Reporter = "Category_Kafka_Reporter";
        public const string CATEGORY_START_UP_CONFIG = "Category_Start_Up_Config";
        public const string PROPERTY_FILE_NAME = "FileName";
        public const string START_UP_CONFIG_FILE_NAME= "StartUpConfig";
        public const string ATDL_LOG_FILE_NAME = "AtdlLog";

        public static readonly bool LoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LOGGINGENABLED"]);
        #endregion

        /// <summary>
        /// The request sent
        /// </summary>
        public const string REQUEST_SENT = "Request Sent";

        /// <summary>
        /// The request received
        /// </summary>
        public const string REQUEST_RECEIVED = "Request Received";

        /// <summary>
        /// The response sent
        /// </summary>
        public const string RESPONSE_SENT = "Response Sent";

        /// <summary>
        /// The response received
        /// </summary>
        public const string RESPONSE_RECEIVED = "Response Received";

        /// <summary>
        /// The colon
        /// </summary>
        public const string COLON = ": ";

        /// <summary>
        /// The user
        /// </summary>
        public const string USER = "User: ";

        /// <summary>
        /// The action
        /// </summary>
        public const string ACTION = "Action: ";

        #region General Ledger Constants
        /// <summary>
        /// The get activity
        /// </summary>
        public const string GET_ACTIVITY = "Get Activity";

        /// <summary>
        /// The run manual reval
        /// </summary>
        public const string RUN_MANUAL_REVAL = "Run Manual Revaluation";

        /// <summary>
        /// The run reval
        /// </summary>
        public const string RUN_REVAL = "Run Revaluation";

        /// <summary>
        /// The get cash transactions
        /// </summary>
        public const string GET_CASH_TRANSACTIONS = "Get Cash Transactions";

        /// <summary>
        /// The get sub account details
        /// </summary>
        public const string GET_SUB_ACCOUNT_DETAILS = "Get Account Details";

        /// <summary>
        /// The get account balances
        /// </summary>
        public const string GET_ACCOUNT_BALANCES = "Get Account Balances";

        /// <summary>
        /// The get overriding activity
        /// </summary>
        public const string GET_OVERRIDING_ACTIVITY = "Get Overriding Activity";

        /// <summary>
        /// The get activity exceptions
        /// </summary>
        public const string GET_ACTIVITY_EXCEPTIONS = "Get Activity Exceptions";

        /// <summary>
        /// The get overriding journals
        /// </summary>
        public const string GET_OVERRIDING_JOURNALS = "Get Overriding Journals";

        /// <summary>
        /// The get journal exceptions
        /// </summary>
        public const string GET_JOURNAL_EXCEPTIONS = "Get Journal Exceptions";

        /// <summary>
        /// The get precalculated daily calculations
        /// </summary>
        public const string GET_PRECALCULATED_DAILY_CALCULATIONS = "Get Daily Calculations for already calculated data";

        /// <summary>
        /// The calculate daily cash
        /// </summary>
        public const string CALCULATE_DAILY_CASH = "Calculate Daily Cash";

        /// <summary>
        /// The get daily calculation
        /// </summary>
        public const string GET_DAILY_CALCULATION = "Get Daily Calculations for Open Positions";

        /// <summary>
        /// The get day end cash
        /// </summary>
        public const string GET_DAY_END_CASH = "Get Day End Cash";

        /// <summary>
        /// The generate journal exceptions
        /// </summary>
        public const string GENERATE_JOURNAL_EXCEPTIONS = "Generate Journal Exceptions";

        /// <summary>
        /// The save activity exceptions
        /// </summary>
        public const string SAVE_ACTIVITY_EXCEPTIONS = "Save Activity Exceptions";

        /// <summary>
        /// The Save cash transactions
        /// </summary>
        public const string SAVE_CASH_TRANSACTIONS = "Save Cash Transactions";

        /// <summary>
        /// The Save day end cash
        /// </summary>
        public const string SAVE_DAY_END_CASH = "Save Day End Cash";

        /// <summary>
        /// The Save CI
        /// </summary>
        public const string SAVE_CI = "Save Collateral Interest";

        /// <summary>
        /// The Save daily calculations
        /// </summary>
        public const string SAVE_DAILY_CALCULATIONS = "Save Daily Calculations";
        #endregion

        public static class SerilogConstant
        {
            public static readonly string KAFKA_REQUEST_ID = "Kaf_ReqId";
            public static readonly string CORRELATION_ID = "CorrelationId";
            public static readonly string USER_ID = "UserId";
            public static readonly string APP_NAME = "ApplicationName";
            public static readonly string EXCEPTION = "Exception";
            public static readonly string IP_ADDRESS = "IpAddress";
            public static readonly string CUSTOM_SOURCE = "CustomSource";
            public static readonly string MACHINE_NAME = "MachineName";
            public static readonly string THREAD_ID = "ThreadId"; 
            public static readonly string SERILOG_LEVEL_FILE_NAME = "serilogLogLevel.json"; 


        }
    }
}
