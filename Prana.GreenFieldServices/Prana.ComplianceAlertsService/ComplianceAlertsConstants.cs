namespace Prana.ComplianceAlertsService
{
    public class ComplianceAlertsConstants
    {
        public static readonly string CONST_ORDER = "Order";
        public static readonly string CONST_USERID = "userId";
        public static readonly string CONST_ALERTS = "alerts";
        public static readonly string CONST_POPUP_TYPE = "popUpType";
        public static readonly string CONST_ORDERID = "OrderId";
        public static readonly string CONST_COMPANY_USERID = "CompanyUserId";
        public static readonly string CONST_HEADER_POPUP_TYPE = "PopUpType";
        public static readonly string CONST_HEADER_ALERTS = "Alerts";
        public static readonly string CONST_TRADE_TYPE = "TradeType";
        public static readonly string CONST_RULE_TYPE = "RuleType";
        public static readonly string CONST_RESULT_ALLOWED = "ResultAllowed";
        public const string MSG_STAGING_ALLOWED_RULE_OVERRIDEN = "Staging Allowed as rule was overridden by user";
        public const string MSG_STAGING_BLOCKED = "Staging Blocked.";
        public static readonly string CONST_HEADER_ALERTS_COUNT = "Alerts Count:";
        public static readonly string CONST_CORRELATIONID = "CorrelationId";
        public static readonly string CONST_PRETRADETYPE = "PreTradeType";

        #region Button text constants
        public static readonly string BTN_TEXT_YES = "YES";
        public static readonly string BTN_TEXT_SEND = "SEND";
        public static readonly string BTN_TEXT_NO = "NO";
        public static readonly string BTN_TEXT_CANCEL = "CANCEL";
        #endregion

        #region Message constants
        public static readonly string MSG_CONST_COMPLIANCE_ALERTS_SERVICE_STARTED = "Compliance Alerts Service started at:- {0} , (local time) {1}";
        public const string MSG_ShutdownService = "Shutting down service.";
        public const string MSG_ComplianceAlertsServiceClosed = "Compliance Alerts Service successfully closed at:- {0} , (local time) {1}";
        public static readonly string MSG_CONST_LOCAL_TIME = " (local time)";
        public static readonly string MSG_CONST_COMPANY_HAS_NO_COMPLIANCE_PERMISSION = "Company has no permission for Compliance Engine";
        public const string MSG_STAGING_ALLOWED = "Staging Allowed.";
        public const string CONST_MULTIPLE_ORDER = " (Multiple Orders)";
        public const string CONST_WAITING_FOR_COMMON_DATA_SERVICE = "Waiting for response from Common data service...";
        public const string CONST_COMPLIANCE_PERMISSION_CHANGED = "Company compliance permission is disabled. Please restart the service";
        public const string CONST_COMPLIANCE_PERMISSION_RECEIVED = "Compliance permissions response received";
        public const string CONST_COMPLIANCE_PERMISSION_PROCESSED = "Compliance permissions response processed";
        public const string CONST_OVERRIDE_REQUEST_RECEIVED = "Override request received for UserId ";
        public const string CONST_OVERRIDE_REQUEST_PROCESSED = "Override request processed for UserId ";
        public const string CONST_OVERRIDE_RESPONSE_RECEIVED = "Override response received from UserId ";
        public const string CONST_OVERRIDE_RESPONSE_PROCESSED = "Override response processed from UserId ";
        public const string CONST_STAGE_OVERRIDE_REQUEST_RECEIVED = "Stage Override request received for UserId ";
        public const string CONST_STAGE_OVERRIDE_REQUEST_PROCESSED = "Stage Override request processed for UserId ";
        public const string CONST_STAGE_OVERRIDE_RESPONSE_RECEIVED = "Stage Override response received from UserId ";
        public const string CONST_STAGE_OVERRIDE_RESPONSE_PROCESSED = "Stage Override response processed from UserId ";
        #endregion

        #region Logging constants
        public static readonly string CONST_METHOD_EXECUTION_STARTED = "Controller Method Execution Started, Method Name: ";
        public static readonly string CONST_METHOD_EXECUTION_ENDED = "Controller Method Execution Ended, Method Name: ";
        public static readonly string CONST_BRACKET_OPEN = "[";
        public static readonly string CONST_BRACKET_CLOSE = "] ";
        public static readonly string CONST_DATET_TIME_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";
        public static readonly string CONST_COLON = ": ";
        public static readonly string CONST_SPACE = " ";
        public static readonly string CONST_ARROW = " -> ";
        #endregion

        #region Method constants
        public static readonly string CONST_FETCH_PERMISSION_BASED_ON_COMPANY_USERID = "FetchingPermissionsBasedOnCompanyUserID";
        public static readonly string CONST_SEND_COMPLIANCE_ALERTS_FOR_STAGE_ORDER = "SendComplianceAlertsRequestForStageOrder";
        public static readonly string CONST_SEND_COMPLIANCE_ALERTS_FOR_MANUAL_AND_LIVE_ORDER = "SendComplianceAlertsRequestForManualAndLiveOrder";
        public static readonly string CONST_SEND_OVERRIDE_RESPONSE_TO_SERVER_FOR_STAGE_ORDER = "SendOverRideResonseToServerForStageOrder";
        public static readonly string CONST_SEND_OVERRIDE_RESPONSE_TO_SERVER_FOR_MANUAL_AND_LIVE_ORDER = "SendOverRideResonseToServerForManualAndLiveOrder";
        #endregion
    }
}
