namespace Prana.BusinessObjects
{
    public class ThirdPartyConstants
    {

        //FTP Encryption constants
        public const string FTP_ENCRYPTION_NONE = "None";
        public const string FTP_ENCRYPTION_EXPLICIT = "Explicit";
        public const string FTP_ENCRYPTION_IMPLICIT = "Implicit";

        //FIX Connection Status constants
        public const string FIX_CONNECTIONSTATUS_CONNECTED = "Connected";
        public const string FIX_CONNECTIONSTATUS_DISCONNECTED = "Disconnected";

        // constant for warning message when FIX connection is down
        public const string FIX_DISCONNECTION_MESSAGE = "FIX Connection Down: Unable to Send Allocation Instructions.";
        public const string FIX_DISCONNECTION_TITLE = "FIX Disconnection Notice";

        // const for status messages when user clicks View, Export & Send
        public const string STATUS_LOADING_DATA = "Loading the data";
        public const string STATUS_NO_DATA_FOUND = "No data found (Review XSLT Settings)";
        public const string STATUS_EXPORT_SUCCESSFUL = "Exporting the data successful";
        public const string STATUS_EXPORT_FAILED = "Exporting the data failed";
        public const string STATUS_GENERATING_FIX_MESSAGES = "Generating FIX messages";
        public const string STATUS_FIX_GENERATION_SUCCESSFUL = "FIX messages generation Successful";
        public const string STATUS_FIX_GENERATION_UNSUCCESSFUL = "FIX messages generation Unsuccessful";
        public const string STATUS_ALLOCATION_INSTRUCTIONS_SENT = "Allocation Instructions Sent";
        public const string STATUS_ALLOCATION_INSTRUCTIONS_FAILED = "Allocation Instructions Sending failed";
        public const string STATUS_DATA_LOADED_SUCCESSFULLY = "Data loaded succesfully";

        public const string JOB_SEND_THIRD_PARTY_TIME_BATCHES = "JobSendThirdPartyTimeBatches";
        public const string THIRD_PARTY_TIME_BATCH_GROUP = "ThirdPartyTimeBatchGroup";
        public const string THIRD_PARTY_TIME_BATCH_SCHEDULER = "ThirdPartyTimeBatchScheduler";
        public const string THIRD_PARTY_TIME_BATCH_TRIGGER_GROUP = "ThirdPartyTimeBatchTriggerGroup";

        public const string JOB_SEND_THIRD_PARTY_JOB_EXECUTION_NOTIFICATION = "JobSendThirdPartyJobExecutionNotification";
        public const string THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_GROUP = "ThirdPartyJobExecutionNotificationGroup";
        public const string THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_SCHEDULER = "ThirdPartyJobExecutionNotificationScheduler";
        public const string THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_TRIGGER_GROUP = "ThirdPartyJobExecutionNotificationTriggerGroup";

        public const double CONST_TIME_BEFORE_JOB_EXECUTION_FOR_NOTIFICATION = 10;
        public const string CONST_AUTOMATED_BATCH_STATUS_NA = "NA";
        public const string CONST_AUTOMATED_BATCH_STATUS_NO_BATCH_SET = "No batch set";

    }
}
