namespace Prana.ServiceGateway.Constants
{
    public class MessageConstants
    {
        #region Error Messages

        public static readonly string MSG_CONST_USERNAME_PASSWORD_BLANK = "Username or Password cannot be Blank";
        public static readonly string MSG_CONST_COMPANYUSERID_CANNOT_BLANK = "CompanyUserId cannot be Blank or 0";
        public static readonly string MSG_CONST_AN_ERROR_OCCURRED = "An error occurred. Please try again...";
        public static readonly string MSG_CONST_VALID_TOKEN = "Token is valid";
        public static readonly string MSG_CONST_INVALID_TOKEN = "Token is Invalid";
        public static readonly string MSG_CONST_GRIDNAME_CANNOT_BLANK = "GridName cannot be Blank";
        public static readonly string MSG_CONST_SAVEDLAYOUTTEXT_OR_GRIDNAME_CANNOT_BLANK = "Either SavedLayoutText or GridName is Blank";
        public static readonly string MSG_CONST_UNAUTHORIZED = "Status Code: 401; Unauthorized";
        public static readonly string MSG_CONST_TOKEN_FETCHING_ERROR = "Error while retreiving token from headers in custom middleware  : ";
        public static readonly string MSG_CONST_TOKEN_DECRYPTING_ERROR = "Error while decrypting token from headers in custom middleware  : ";
        public static readonly string MSG_INCORRECT_CREDENTIALS = "Incorrect Credentials";
        public static readonly string MSG_CONST_LOGIN_ERROR = "Error occured while encrypting token upon login";
        public static readonly string MSG_CONST_TOKEN_CREATION_ERROR = "Error while creating token : ";
        public static readonly string MSG_CONST_LOGIN_FAILED_WITH_ERROR = "Login request failed with error: ";
        public static readonly string MSG_CONST_LOGOUT_FAILED_WITH_ERROR = "Logout request failed with error: ";
        public static readonly string MSG_CONST_CANNOT_BE_NULL = "Value cannot be Null : ";
        public static readonly string MSG_CONST_INTERNAL_SERVER_ERROR = "An internal server error occurred";
        public static readonly string MSG_CONST_SWAGGER_LOGIN_REQUEST = " Login Request made from Swagger , hence encrypting the password";
        public static readonly string MSG_CONST_SWAGGER_LOGIN_REQUEST_ERROR = "Error while encrypting the password string provided from Swagger UI";
        public static readonly string MSG_CONST_LAYOUTTITLE_OR_LAYOUT_CANNOT_BLANK = "Either LayoutTitle or Layout is Blank";
        public static readonly string MSG_CONST_SIGNALR_CONNECTION_REJECTED = "SignalR Connection rejected: ";
        public static readonly string MSG_CONST_USERID_MISSING = "User ID is missing.";
        public static readonly string MSG_CONST_TOKEN_VALIDATION_FAILD = "Token validation failed.";
        public static readonly string MSG_CONST_TOKEN_MISSING_OR_EMPTY = "Token is missing or empty in the request.";
        public static readonly string MSG_CONST_INVALID_TOKEN_FORMAT = "Invalid token format:";
        public static readonly string MSG_CONST_USERNAME_BLANK = "Username cannot be null or empty. Please provide a valid username.";
        public static readonly string MSG_CONST_TOUCH_ACCESS_TOKEN_ERROR = "An error occurred while creating the touch otk. Please try again later.";
        public static readonly string MSG_CONST_WEB_APP_SECRET_KEY_BLANK = "Web app secret key cannot be null or empty.";
        public static readonly string MSG_CONST_TOUCH_SECRET_KEY_BLANK = "Touch secret key cannot be null or empty.";
        public static readonly string MSG_INVALID_REQUEST_PARAMETERS = "Invalid Request Parameters";
        public static readonly string MSG_INVALID_INPUT = "Invalid input: Must provide either Symbol, OptionSymbol or MultipleSymbol.";

        #endregion

        #region Message Section

        public const string MSG_CONST_SETTING_USER_DETAILS_CLAIM_IN_TOKEN = "Setting user detail claim in token. CompanyUserId: {0} , Support: {1}, Admin: {2}";
        public const string MSG_CONST_CLAIM_IDENTITY_NULL_CALL_GENERATE_API = "Claim identity is null while fetching user detail from token, please call generate api token";
        public const string MSG_CONST_NO_CLAIM_FOUND_GENERATE_API_AND_TRY_AGAIN = "No Claim found in tokens, please initiate generate token api and try again";
        public const string MSG_CONST_ERROR = "An error occurred ";
        public const string MSG_CONST_LOG_LEVEL_REQUIRED = "Log Level is Required";
        public const string MSG_CONST_LOG_MESSAGE_REQUIRED = "Log Message is Required";
        public const string MSG_CONST_UNDETERMINED_LOG = "Undetermined Log Level";
        public const string MSG_CONST_FILE_SAVED = "File Saved";
        public const string MSG_CONST_BEARER = "Bearer ";
        public const string MSG_CONST_RECEIVE_MESSAGE = "ReceiveMessage";
        public const string MSG_UPDATE_CACHE_REQUEST_RECEIVED = "Update cache request received from : ";
        public const string MSG_SIGNALR_RESPONSE_GENERATED = "SignalR response generated for topic : ";
        public const string MSG_GET_BROKE_CONNECTION_CACHE_DATA_REQUEST = "GetBrokerConnectionCacheData request has been received for UserId: ";
        public const string MSG_GET_BROKE_CONNECTION_CACHE_DATA_RESPONSE = "GetBrokerConnectionCacheData response generated for UserId: ";

        #endregion

        #region Logger Messages

        public static readonly string CONST_METHOD_EXECUTION_STARTED = "Controller Method Execution Started, Method Name: ";
        public static readonly string CONST_METHOD_EXECUTION_ENDED = "Controller Method Execution Ended, Method Name: ";
        public static readonly string CONST_BRACKET_OPEN = "[";
        public static readonly string CONST_BRACKET_CLOSE = "] ";
        public static readonly string CONST_DATET_TIME_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";
        public static readonly string CONST_BAD_REQUEST_FORMAT = ", BadRequest, Method Name:";
        public static readonly string CONST_LOGIN_SUCCESS = "Login success for user: ";
        public const string MSG_TRADING_ACCOUNTS_NOT_AVAILABLE = "Trading accounts information not available for userId ";
        public static readonly string CONST_LAYOUT_CONTROLLER = "LayoutController";
        public static readonly string CONST_OPENFIN_MANAGEMENT_CONTROLLER = "OpenfinManagementController";
        public static readonly string CONST_AUTHENTICATE_USER_CONTROLLER = "AuthenticateUserController";
        public static readonly string CONST_BLOTTER_CONTROLLER = "BlotterController";
        public static readonly string CONST_RTPNL_CONTROLLER = "RtpnlController";
        public static readonly string CONST_COMMON_DATA_CONTROLLER = "CommonDataController";
        public static readonly string CONST_COMPLIANCE_CONTROLLER = "ComplianceController";
        public static readonly string CONST_LIVE_FEED_CONTROLLER = "LiveFeedController";
        public static readonly string CONST_TRADING_CONTROLLER = "TradingController";
        public static readonly string CONST_LINE_BREAK = "_______________________________________________________________________________________________";
        public static readonly string CONST_MASTER_FUND_CACHE_CREATED = "User permitted Funds and MasterFunds cache created for userId:";
        public const string MSG_TRADING_TICKET_CACHE_UPDATED = "Trading ticket cache updated for UserId: ";
        public const string MSG_BROKER_CACHE_UPDATED = "Broker cache updated for UserId: ";
        public const string MSG_DATA_REMOVED_FROM_TRADING_TICKET_CACHE = "Data removed from trading ticket cache for UserId: ";
        public const string MSG_DATA_REMOVED_FROM_BROKER_CACHE = "Data removed from broker cache for UserId: ";
        public const string MSG_BROKER_CONN_STATUS_UPDATED = "Broker connection status has been updated";
        public const string MSG_GET_TT_CACHE_DATA_REQUEST = "GetTradingTicketCacheData request has been received for UserId: ";
        public const string MSG_GET_TT_CACHE_DATA_RESPONSE = "GetTradingTicketCacheData response generated for UserId: ";
        public static readonly string CONST_USER_DTO_NULL = "UserDtoObj is null which is fetched from claims of WebApp token";
        public static readonly string CONST_USER_NAME_IN_DTO_NULL_OR_EMPTY = "User name cannot be null or empty which is fetched from UserDtoObj claims of WebApp token";
        public static readonly string CONST_ERROR_IN_GET_TOUCH_ACCESS_TOKEN = "Error in GetTouchOtk for user {userName}";
        public static readonly string CONST_TOUCH_OTK_CREATIION = "Creating touch otk for UserName:{uId}";
        public static readonly string CONST_TOUCH_OTK_CREATION_SUCCESS = "Touch otk is created successfully for UserName:{uId}";
        public static readonly string CONST_TOUCH_OTK_CREATION_ERROR = "CreateTouchOtk encountered an error for user {userName}";
        public static readonly string CONST_TOUCH_TOKEN_DTO_NULL = "CreateTouchOtk Method- TouchTokenDto cannot be null.";

        #endregion
    }

    public static class LogConstant
    {
        public static readonly string KAFKA_REQUEST_ID = "Kaf_ReqId";
        public static readonly string CORRELATION_ID = "CorrelationId";
        public static readonly string USER_ID = "UserId";
        public static readonly string APP_NAME = "ApplicationName";
        public static readonly string EXCEPTION = "Exception";
        public static readonly string IP_ADDRESS = "IpAddress"; 

        public static readonly string LOG_TYPE = "LogType";
        public static readonly string NIRVANA_WEB_APP = "Nirvana_Web_App";
    }
}
