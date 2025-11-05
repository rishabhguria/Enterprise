namespace Prana.BusinessObjects.Compliance.Constants
{
    public class PreTradeConstants
    {
        public const string MsgTradeReject = "Blocked by Compliance";
        public const string MsgTradeCancelled = "Cancelled";
        public const string ConstComplianceFailed = "Compliance Failed";
        public const string ConstBasketComplianceFailed = "Basket Compliance Failed";
        public const string MsgTradePending = "Pending Compliance Approval";
        public const string Const_OverrideResponse = "OverrideResponse";
        public const string MSG_TRADE_EXPIRED = "Expired because the order was cancelled.";
        public const string MSG_TRADE_EXPIRED_REPLACED = "Expired because the order was replaced.";
        public const string MSG_NO_ALERT_RECEIVED = "NoAlertReceived";
        public const string MSG_PENDING_COMPLIANCE_APPROVAL_CHANGE_ORDER_STATUS = "ChangeOrderStatus";

        public const string CONST_CREATING_VIRTUAL_PORTFOLIO = "Creating virtual portfolio";
        public const string CONST_SENDING_DATA_FOR_CALCULATIONS = "Sending data for calculations";
        public const string CONST_VALIDATING_RULES = "Validating rules";
        public const string CONST_GENERATING_RESULTS = "Generating results";
        public const string CONST_SEPARATOR_CHAR = "~";

        public const string CONST_FAILED_ALERT_ID = "-2147483648";
        public const string CONST_USER_NOTE = "Original Compliance approval order replaced with an order passing all compliances.";

        public const string CONST_BASKET_COMPLIANCE= "BasketComplianceCache";
    }
}
