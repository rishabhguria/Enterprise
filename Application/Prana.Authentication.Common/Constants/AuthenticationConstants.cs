namespace Prana.Authentication.Common
{
    public class AuthenticationConstants
    {
        #region Auth Message section

        public const string MSG_ENTERPRISE_LOGGED_IN = " is already logged in.";
        public const string MSG_ENTERPRISE_LOGGED_IN_WEB = " is already logged in on Enterprise. Please select 'Proceed' if you wish to sign out from Enterprise to access Nirvāna ONE. Note: All unsaved changes will be lost.";
        public const string MSG_WEB_LOGGED_IN = " is already logged in on Nirvāna ONE on another device. Please select 'Proceed' if you wish to sign out from there and access Nirvāna ONE on this device. Note: All unsaved changes will be lost.";
        public const string MSG_WEB_LOGGED_IN_ENTERPRISE = " is already logged in via Web. Please select 'Proceed' if you wish to sign out from Web Application to access Desktop. \n\n Note: All unsaved changes will be lost.";
        public const string MSG_CORE_SERVICES_NOT_RUNNING = "Core Service(s) are not running. Please contact Administrator.";
        public const string SERVICE_NOT_AVAILABLE_ERROR = "No connection could be made because the target machine actively refused it";
        public const string ENCRYPTION_ERROR = "Please run password encryption utility before logging into the application.";
        public const string MSG_INCORRECT_USER = "Invalid user";
        public const string MSG_SUCCESSFUL_LOGGED_OUT = " has been logged out successfully";
        public const string MSG_FAILED_LOGGED_OUT = "Invalid User or you have been already logged out";
        public const string MSG_AUTHSERVICE_START = "AuthService started at:- {0} , (local time) {1}";
        public const string MSG_AUTHSERVICE_CLOSED = "AuthService successfully closed at:- {0} , (local time) {1}";
        public const string MSG_SERVICE_SHUTDOWN = "Shutting down service.";
        public const string MSG_KAFKA_PRODUCE = "Producing {0} to Kafka";
        public const string MSG_KAFKA_CONSUMER = " message recieved from Kafka";
        public const string MSG_LOGIN_REQUEST_RECEIVED = "Login request received from : ";
        public const string MSG_LOGIN_REQUEST_PROCESSED = "Login request processed : ";
        public const string MSG_LOGOUT_REQUEST_RECEIVED = "Logout request received";
        public const string MSG_LOGOUT_REQUEST_RECEIVED_WITH_COMPANYUSERID = "Logout request received for CompanyUserId : ";
        public const string MSG_LOGIN_REQUEST_FAILED = "Login request failed with error : ";
        public const string MSG_SUCCESSFUL_LOGGED_IN = " has been logged in successfully";
        public const string MSG_UPDATE_CACHE_REQUEST_RECEIVED = "Update cache request received from : ";
        public const string MSG_LOGOUT_REQUEST_PROCESSED = "Logout request processed : ";
        public const string MSG_CACHE_UPDATED = "Added the details in the cache for user : CompanyUserId ";
        public const string MSG_REMOVED_USER_FROM_CACHE = "Removed the details for this user from cache : CompanyUserId ";
        public const string MSG_AUTHENTICATION_TYPE = " AuthenticationType : ";
        public const string MSG_FORCEFULLY_REMOVED_USER_FROM_CACHE = "Forcefully removed the details for this user from cache : CompanyUserId ";
        public const string MSG_ISFORCEFULLOGOUTWEB = " isForcefulLogoutWeb ";
        public const string MSG_ISFORCEFULLOGOUTENTERPRISE = " isForcefulLogoutEnterprise ";
        public const string MSG_LOCAL_TIME = " (local time)";
        public const string MSG_FORCEFUL_LOGOUT_REQUEST_RECEIVED_FOR_ENTERPRISE = "Forceful logout request received for enterprise for user : CompanyUserId ";
        public const string MSG_UPDATE_CACHE_REQUEST_FAILED = "Update cache request failed with error: ";
        public const string MSG_INCORRECT_PASSWORD = "Incorrect password";
        public const string MSG_LOGIN_INFORMATION_REQUEST_PROCESSED = "Logged-In user information request processed for user ID:";

        public const string MSG_LOGIN_RESPONSE_GUID = " Login Response > GUID: ";
        public const string MSG_FORCEFULLY_KILLING_REQUEST_ERROR_FROM_CACHE = "Killing request: but error found on Forcefully removal the details for this user from cache : CompanyUserId ";
        public const string MSG_LOGIN_REQUEST_FAILED_ERRORL = "Login request failed with error {0}";
        public const string MSG_CONST_MSAL_TOKEN_EXPIRED = "MSAL Token has expired.";
        public const string MSG_CONST_MSAL_TOKEN_INVALID = "MSAL Token validation failed.";
        public const string MSG_CONST_INVALID_ISSUER_FOR_MSAL_TOKEN = "Invalid issuer for MSAL token.";
        public const string MSG_CONST_OPENID_CONFIGURATION_PATH = "/.well-known/openid-configuration";
        public const string MST_CONST_BASE_ISSUER_URL = "https://login.microsoftonline.com/";
        public const string MST_CONST_API_VERSION_PATH = "/v2.0";

        #endregion

        #region DateTime

        public const string NirvanaDateFormat_withTime = "M/d/yyyy hh:mm:ss tt";
        public const string DateTimeFormat = "MM/dd/yyyy hh:mm:ss.fff tt";

        #endregion

        public const string CONST_USERNAME = "UserName";
        public const string CONST_PASSWORD = "Password";
        public const string CONST_LOGGEDIN_USERNAME = "userName";
        public const string CONST_TOKEN = "token";
        public const string CONST_LOGOUT = "Logout";


    }
}
