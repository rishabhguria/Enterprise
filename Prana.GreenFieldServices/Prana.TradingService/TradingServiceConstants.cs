

namespace Prana.TradingService
{
    public class TradingServiceConstants
    {
        public static readonly string CONST_ACCOUNT_NAME = "account";
        public static readonly string CONST_CURRENT_QUANTITY = "currentQty";
        public static readonly string CONST_CURRENT_PERCENT = "currentPercent";
        public static readonly string CONST_ALLOCATED_QUANTITY = "allocatedQty";
        public static readonly string CONST_ALLOCATED_PERCENT = "allocated";
        public static readonly string CONST_USERID = "userId";
        public static readonly string CONST_ORDERID = "OrderId";

        #region Logging constants
        public static readonly string MSG_BROKER_AND_VENUES_REQUEST_RECEIVED = "Broker and Venues Request received for UserID:";
        public static readonly string MSG_BROKER_AND_VENUES_REQUEST_PROCESSED = "Broker and Venues Request processed for UserID:";
        public static readonly string MSG_GET_CUSTOM_ALLOCATION_FROM_DB_RECEIVED = "Get Custom Allocation from DB request received for UserID:";
        public static readonly string MSG_GET_CUSTOM_ALLOCATION_FROM_DB_PROCESSED = "Get Custom Allocation from DB request processed for UserID:";
        public static readonly string MSG_SAVE_CUSTOM_ALLOCATION_TO_DB_RECEIVED = "Save Custom Allocation to DB request received for UserID:";
        public static readonly string MSG_SAVE_CUSTOM_ALLOCATION_TO_DB_PROCESSED = "Save Custom Allocation to DB request processed for UserID:";
        public static readonly string MSG_UNSUBSCRIBE_SYMBOL_COMPRESSION_RECEIVED = "Unsubscribe Symbol compression request received for UserID:";
        public static readonly string MSG_UNSUBSCRIBE_SYMBOL_COMPRESSION_PROCESSED = "Unsubscribe Symbol compression request processed for UserID:";
        public static readonly string MSG_SUBSCRIBE_SYMBOL_COMPRESSION_RECEIVED = "Subscribe Symbol compression request received for UserID:";
        public static readonly string MSG_SUBSCRIBE_SYMBOL_COMPRESSION_PROCESSED = "Subscribe Symbol compression request processed for UserID:";
        public static readonly string MSG_SYMBOL_COMPRESSION_INITIATED = "Symbol compression Initiated for Symbol:";
        public static readonly string MSG_USER_PERMITTED_AUECCV_RECEIVED = "User Permitted AUECCV request received for UserID:";
        public static readonly string MSG_USER_PERMITTED_AUECCV_PROCESSED = "User Permitted AUECCV request processed for UserID:";
        public static readonly string MSG_HOT_KEY_PREFERENCES_RECEIVED = "Get User Hot Key Preferences request received for UserID:";
        public static readonly string MSG_HOT_KEY_PREFERENCES_PROCESSED = "Get User Hot Key Preferences request processed for UserID:";
        public static readonly string MSG_UPDATE_HOT_KEY_PREFERENCES_RECEIVED = "Update User Hot Key Preferences request received for UserID:";
        public static readonly string MSG_UPDATE_HOT_KEY_PREFERENCES_PROCESSED = "Update User Hot Key Preferences request processed for UserID:";
        public static readonly string MSG_HOT_KEY_PREFERENCES_DETAILS_RECEIVED = "Get User Hot Key Preferences Details request received for UserID:";
        public static readonly string MSG_HOT_KEY_PREFERENCES_DETAILS_PROCESSED = "Get User Hot Key Preferences Details request processed for UserID:";
        public static readonly string MSG_UPDATE_HOT_KEY_PREFERENCES_DETAILS_RECEIVED = "Update User Hot Key Preferences Details request received for UserID:";
        public static readonly string MSG_UPDATE_HOT_KEY_PREFERENCES_DETAILS_PROCESSED = "Update User Hot Key Preferences Details request processed for UserID:";
        public static readonly string MSG_SAVE_HOT_KEY_PREFERENCES_DETAILS_RECEIVED = "Save User Hot Key Preferences Details request received for UserID:";
        public static readonly string MSG_SAVE_HOT_KEY_PREFERENCES_DETAILS_PROCESSED = "Save User Hot Key Preferences Details request processed for UserID:";
        public static readonly string MSG_DELETE_HOT_KEY_PREFERENCES_DETAILS_RECEIVED = "Delete User Hot Key Preferences Details request received for UserID:";
        public static readonly string MSG_DELETE_HOT_KEY_PREFERENCES_DETAILS_PROCESSED = "Delete User Hot Key Preferences Details request processed for UserID:";
        public static readonly string MSG_USER_PERMITTED_ACCOUNTS_RECEIVED = "User Permitted Accounts request received for UserID:";
        public static readonly string MSG_USER_PERMITTED_ACCOUNTS_PROCESSED = "User Permitted Accounts request processed for UserID:";
        public static readonly string MSG_USER_PERMITTED_BROKER_RECEIVED = "User Permitted Broker request received for UserID:";
        public static readonly string MSG_USER_PERMITTED_BROKER_PROCESSED = "User Permitted Broker request processed for UserID:";
        public static readonly string MSG_USER_PERMITTED_BROKER_WISE_VENUES_RECEIVED = "User Permitted Broker wise Venues request received for UserID:";
        public static readonly string MSG_USER_PERMITTED_BROKER_WISE_VENUES_PROCESSED = "User Permitted Broker wise Venues request processed for UserID:";
        public static readonly string MSG_COMPANY_WISE_TRADING_PREFERENCE_RECEIVED = "Company Wise Trading Preference request received for UserID:";
        public static readonly string MSG_COMPANY_WISE_TRADING_PREFERENCE_PROCESSED = "Company Wise Trading Preference request processed for UserID:";
        public static readonly string MSG_SM_DATA_FOR_BINDING_RECEIVED = "SM Data for binding request received for UserID:";
        public static readonly string MSG_SM_DATA_FOR_BINDING_PROCESSED = "SM Data for binding request processed for UserID:";
        public static readonly string MSG_CREATE_POPUP_TEXT_RECEIVED = "Create Popup text request received for UserID:";
        public static readonly string MSG_CREATE_POPUP_TEXT_PROCESSED = "Create Popup text request processed for UserID:";
        public static readonly string MSG_CREATE_EQUITY_OPTION_SYMBOL_RECEIVED = "Create Equity Option Symbol request received for UserID:";
        public static readonly string MSG_CREATE_EQUITY_OPTION_SYMBOL_PROCESSED = "Create Equity Option Symbol request processed for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_REPLACE_RECEIVED = "Send Order for Replace operation request received for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_REPLACE_PROCESSED = "Send Order for Replace operation request processed for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_STAGE_RECEIVED = "Send Order for Stage operation request received for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_STAGE_PROCESSED = "Send Order for Stage operation request processed for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_MANUAL_RECEIVED = "Send Order for Manual operation request received for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_MANUAL_PROCESSED = "Send Order for Manual operation request processed for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_LIVE_RECEIVED = "Send Order for Live operation request received for UserID:";
        public static readonly string MSG_SEND_ORDER_FOR_LIVE_PROCESSED = "Send Order for Live operation request processed for UserID:";
        public static readonly string MSG_TRADE_SENT_FOR_COMPLIANCE = "Trade order sent for Compliance operation for UserID:";
        public static readonly string MSG_PROBLEM_IN_CACHE_CREATION_FOR_AUECCV = "Problem encountered in cache creation for AUECCV by UserID:";
        public static readonly string MSG_COMPLIANCE_PERMISSION_RECEIVED = "Compliance permission for cache generation received";
        public static readonly string MSG_COMPLIANCE_PERMISSION_PROCESSED = "Compliance permission for cache generation processed";
        public static readonly string CONST_BRACKET_OPEN = "[";
        public static readonly string CONST_BRACKET_CLOSE = "]";
        public static readonly string CONST_DATET_TIME_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";
        public static readonly string CONST_COLON = ": ";
        public static readonly string CONST_SPACE = " ";
        public static readonly string CONST_ARROW = " -> ";
        public static readonly string CONST_CACHE_CLEARED = "'s Cache Cleared.";
        public static readonly string CONST_CACHE_CREATED = "'s Trade Cache creation initiated.";
        public static readonly string CONST_AUECCVPERMITTED = "PermittedAUECCV";
        public static readonly string CONST_METHOD_EXECUTION_STARTED = " Method Execution Started:- ";
        public static readonly string CONST_METHOD_EXECUTION_ENDED = " Method Execution Ended:- ";
        public static readonly string CONST_BROKER_COUNT = " Broker Count: ";
        public static readonly string CONST_ORDER_RECEIVED = "Order received to replace for Symbol: ";
        public static readonly string CONST_TRANSACTION_TIME = ", Transaction Time: ";
        public static readonly string CONST_QUANTITY = ", Quantity: ";
        public static readonly string CONST_ORDER_RECEIVED_FOR_TRADE_SERVICE = "Order received to Trading Service for Symbol: ";
        public static readonly string CONST_BROKER_STATUS_CHANGED = "Broker Connection Status Changed:";
        #endregion

        #region Order type and FX Operator
        public static readonly string CONST_FXOPERATOR_MULTIPLY= "M";
        public static readonly string CONST_FXOPERATOR_DIVIDE = "D";
        public static readonly string CONST_ORDER_STATUS_PENDINGNEW = "PendingNew";
        public static readonly string CONST_MINUS_ONE = "-1";
        #endregion

    }
}
