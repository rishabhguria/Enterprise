namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    public class ComplainceConstants
    {
        public const string CONST_THRESHOLD = "Threshold";
        public const string CONST_FIELD_NAME = "Field Name";
        public const string CONST_CAPTION_ACTUAL_RESULT = "Actual Result";
        public const string CONST_CONSTRAINT_FIELDS = "ConstraintFields";
        public const string CONST_SEPARATOR_CHAR = "~";
        public const string CONST_MULTIPLE = "Multiple";
        public const string CONST_ACTUAL_RESULT = "ActualResult";
        public const string CONST_ALERTS = "Alerts";
        public const string CONST_PRE = "Pre";
        public const string CONST_POST = "Post";
        public const string CONST_Threshold_And_Actual_Result = "Threshold and Actual Result";
        public const string CONST_LIST = "List";
        public const string CONST_DATA_EXPORT = " Data Export";

        public const string CONST_VALIDATION_TIME = "ValidationTime";
        public const string CONST_ORDER_ID = "OrderId";
        public const string CONST_ISVIOLATED = "IsViolated";
        public const string CONST_ISEOM = "IsEOM";
        public const string CONST_COMPRESSION_LEVEL = "CompressionLevel";
        public const string CONST_USER_ID = "UserId";
        public const string CONST_PACKAGE_NAME = "PackageName";
        public const string CONST_SUMMARY = "Summary";
        public const string CONST_BLOCKED = "Blocked";
        public const string CONST_RULE_ID = "RuleId";
        public const string CONST_STATUS = "Status";
        public const string CONST_GROUPID = "GroupId";
        public const string CONST_PRE_TRADE_TYPE = "PreTradeType";
        public const string CONST_PRE_TRADE_ACTIONTYPE = "PreTradeActionType";
        public const string CONST_ACTION_USER = "ActionUser";
        public const string CONST_ACTION_USER_NAME = "ActionUserName";
        public const string CONST_TRADE_DETAILS = "TradeDetails";
        public const string CONST_OVERRIDE_USER_ID = "OverrideUserId";
        public const string CONST_USER_NAME = "UserName";
        public const string CONST_COMPLIANCE_OFFICER_NOTES = "ComplianceOfficerNotes";
        public const string CONST_ALERT_TYPE = "AlertType";
        public const string CONST_ALERT_ID = "AlertId";
        public const string CONST_ALERT_TYPE_NAME = "AlertTypeName";
        public const string CAPS_ALERT_TYPE = "Alert Type";
        public const string CONST_RULE_NAME = "RuleName";
        public const string CAPS_RULE_NAME = "Rule Name";
        public const string CONST_DESCRIPTION = "Description";
        public const string CAPS_DESCRIPTION = "Description of Rule";
        public const string CONST_DIMENSION = "Dimension";
        public const string CAPS_DIMENSIONS = "Dimensions";
        public const string CAPS_ACTUAL_RESULT = "Actual Result";
        public const string CONST_PARAMETERS = "Parameters";
        public const string CAPS_COMMENTS = "Comments";
        public const string CONST_HARD_ALERT = "HardAlert";
        public const string CONST_SOFT_ALERT = "SoftAlert";
        public const string CONST_SOFT_ALERT_WITH_NOTES = "SoftAlertWithNotes";
        public const string CONST_REQUIRES_APPROVAL = "RequiresApproval";
        public const string CONST_USER_NOTES = "UserNotes";
        public const string CAPS_USER_NOTES = "User Notes";
        public const string CONST_USER_NOTES_STATEMENT = "User Notes is required !!";
        public const string CONST_PLEASE_APPROVE = "Compliance Officer - Please Approve";
        public const string CONST_RULE_TYPE = "RuleType";
        public const string CONST_KEY = "Key";
        public const string CONST_TIME = "Time: ";
        public const string CONST_TIME_TRIGGERED = "#TimeTriggered";
        public const string CAPS_TIME_TRIGGERED = "# Times Triggered";
        public const string CONST_TRADE_COMPLIANCE_RESULTS = "-Trade Compliance Results";
        public const string CONST_POST_TRADE = "Post-Trade Compliance Results - Date: ";
        public const string CONST_PRE_TRADE = "Pre-Trade Compliance Results - Date: ";
        public const string CONST_ALERT_COUNT = "P_CA_GetAlertCount";
        public const string CONST_MESSAGE_OVERRIDE = "Do you want to proceed?";
        public const string CONST_MESSAGE_PENDINGAPPROVAL = "Do you want to send this order to Compliance officer?";
        public const string CONST_TIME_FORMAT = "HH:mm:ss";
        public const string CONST_DATE_FORMAT = "MM/dd/yyyy";
        public const string CONST_PRECISION_FORMAT = "{0:0.##}";
        public const string CONST_ALERT_POP_UP_RESPONSE = "AlertPopUpResponse";

        public const string CONST_CAPS_OK = "OK";
        public const string CONST_CAPS_CANCEL = "Cancel";
        public const string CONST_CAPS_SEND = "Send";
        public const string CONST_CAPS_YES = "Yes";
        public const string CONST_CAPS_NO = "No";
        public const string CONST_MULTIPLE_ORDER = " (Multiple Orders)";

        public const string MSG_STAGING_ALLOWED = "Staging Allowed.";
        public const string MSG_STAGING_ALLOWED_RULE_OVERRIDEN = "Staging Allowed as rule was overridden by user";
        public const string MSG_STAGING_BLOCKED = "Staging Blocked.";
        public const string MSG_COULD_NOT_VALIDATE_COMPLIANCE_ALERT = "Could not validate compliance rules";
        public const string MSG_EXPORT_FAILED = "Export failed.";
        public const string MSG_NOTHING_TO_EXPORT = "Nothing to export!";
        public const string MSG_FILE_EXPORTED_SUCCESSFULLY = "File exported successfully!";
        public const string HEADER_ALERT = "Alert";

        public const string Feedback_Initial_Msg = "Sending data to trade engine";
        public const string CONST_FEEDBACK_MESSAGE = "FeedbackMessage";
        public const string CONST_CensorValue = "****";
        public const string CONST_ColumnParameter = "Parameters";
        public const string CONST_ColumnDescription = "Description";
        public const string CONST_ColumnSummary = "Summary";
        public const string CONST_NA = "N/A";
        public const string CONST_FieldDataStr = "userId,quantity,symbol,underlyingSymbol,accountShortName,accountLongName,masterStrategyId,masterStrategyName,strategyId,strategyName,strategyFullName,exchange,currency,masterFundName,orderSide,orderType,asset,fxSymbol,sector,subsector,counterParty,venue,putOrCall,strikePrice,isTodayTrade,todayAbsTradeQuantity,todayLongTradeQuantity,todayShortTradeQuantity,isSwapped,monthAbsQuantityGlobal,currentCash,accrual,masterFundNav,assetId,defaultPositionSide,underlyingAsset,underlyingCountry,udaCountry,underlyingExposurePositionSide";
    }
}