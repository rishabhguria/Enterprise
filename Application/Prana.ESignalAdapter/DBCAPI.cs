namespace Prana.ESignalAdapter
{
    public class DBCAPI
    {
        public enum RequestType
        {
            eREQUEST = 0,
            eADVISE = 1,
            ePULSE = 2,
            eREQUESTSNAPSHOT = 3
        };
        public enum DMMessageStatusTypes
        {
            eSTATUS_RECEPTION = 0,
            eSTATUS_PASSWORD,
            eSTATUS_INVALID_SYMBOL,
            eSTATUS_NO_DATA,
            eSTATUS_END_OF_LIST,
            eSTATUS_NO_PASSWORD_FOR_GLOBALS,
            eSTATUS_NEED_HIGHER_SYMBOL_COUNT,
            eSTATUS_NEED_HIGHER_AUTHORIZATION,
            eSTATUS_SOL_NEED_HIGHER_SYMBOL_COUNT,
            eSTATUS_NOT_AUTHORIZED,
            eSTATUS_END_OF_SERVER_LIST,
            eSTATUS_NOT_AUTHORIZED_FOR_SYMBOL,
            eSTATUS_TIMEBASE,
            eSTATUS_TIMESYNC,
            eSTATUS_UNKNOWN,
            eSTATUS_MAX,
            eSTATUS_BACKEND_DISCONNECT,
        }
        public static short DBCAPI_STATUS_CLOSED = 0;
        public static short DBCAPI_STATUS_TRYCONNECT = 1;
        public static short DBCAPI_STATUS_WAITRETRY = 2;
        public static short DBCAPI_STATUS_CONNECTED = 3;
        public static short DBCAPI_STATUS_SHUTDOWN = 4;

        public const int DBCAPI_NOTIFY_CONNECT = 0;/// DBCAPI_NOTIFY_CONNECT -> 0
        public const int DBCAPI_NOTIFY_DISCONNECT = 1;/// DBCAPI_NOTIFY_DISCONNECT -> 1

        public const short DBCAPI_SUCCESS = 0; ///Success
        public const short DBCAPI_ERROR_REFUSED_SERVER_BUSY = -1; /// Request refused because computer is overloaded
        public const short DBCAPI_ERROR_REFUSED_TOO_MANY = -2; /// Request refused because QSrv has too many open connections
        public const short DBCAPI_ERROR_HEADER_ERROR = -3; /// error in transaction header
        public const short DBCAPI_ERROR_WRONG_SIZE = -4; /// error wrong buffer size sent
        public const short DBCAPI_ERROR_MUST_UPGRADE = -5; /// Client Version not compatable, must upgrade.
        public const short DBCAPI_ERROR_ALREADY_CONNECTED = -6; /// Client is already connected.
        public const short DBCAPI_ERROR_WRONG_USERNAMEPASSWORD = -7; /// Invalid Username or Password
        public const short DBCAPI_ERROR_CLOSED_BY_ADMIN = -8; /// Connection was closed by administrator
        public const short DBCAPI_ERROR_SERVER_NOT_AVAILABLE = -9; /// Server is not available
        public const short DBCAPI_ERROR_NO_HEARTBEAT = -10; /// Connection closed, because no heart beat was received
        public const short DBCAPI_ERROR_NOT_ENTITLED = -11; /// Not entitled for this service
        public const short DBCAPI_ERROR_ADDRESS_CHANGE = -12; /// Address Change Disconnect / User must manually reconnect.
        public const short DBCAPI_ERROR_NOT_ENDOFDAY = -13; /// Not authorized for Day Time Use. Try again after market hours.

        public const short DBCAPI_ERROR_NOT_AUTHORIZED = -100; /// Obsolete: Same as DBCAPI_ERROR_NOT_ENTITLED
        public const short DBCAPI_ERROR_TOMANYCONNECTED = -101; /// Obsolete: Same as DBCAPI_ERROR_REFUSED_TOO_MANY
        public const short DBCAPI_ERROR_USERNAMECONNECTED = -102; /// Obsolete: Same as DBCAPI_ERROR_ALREADY_CONNECTED
        public const short DBCAPI_ERROR_UPGRADEVERSION = -103; /// Obsolete: Same as DBCAPI_ERROR_MUST_UPGRADE

        public const short DBCAPI_ERROR_INVALIDSID = -104; /// Invalid Session ID
        public const short DBCAPI_ERROR_NOTCONNECTED = -105; /// Client is not connected.
        public const short DBCAPI_ERROR_MALLOC = -106; /// Memory Allocation Error
        public const short DBCAPI_ERROR_INVALIDKEYWORDTABLE = -107; /// Invalid News Keyword Table
        public const short DBCAPI_ERROR_NOT_SUPPORTED = -108; /// Function not supported
        public const short DBCAPI_ERROR_QUEUE_EMPTY = -109; /// Message queue is empty.
        public const short DBCAPI_ERROR_NOT_INITIALIZED = -110; /// API is not initialized
        public const short DBCAPI_ERROR_INVALID_PARAM = -111; /// Invalid Parameter passed in.
        public const short DBCAPI_ERROR_INVALIDNAME = -112; /// Invalid Host Name or IP Address
        public const short DBCAPI_ERROR_CONNREFUSED = -113; /// Connection Refused or Server not Listening
        public const short DBCAPI_ERROR_WINSOCK = -114; /// Winsock communication error
        public const short DBCAPI_ERROR_PROXY = -115; /// Proxy Server error.
        public const short DBCAPI_ERROR_UNKNOWN = -999; /// Unknown error;

        public const int DBCAPI_DM_LRT = 28;/// DBCAPI_DM_LRT -> 28
        public const int DBCAPI_DM_MESSAGE = 29;/// DBCAPI_DM_MESSAGE -> 29
        public const int DBCAPI_DM_INTERNATIONALLONG = 23;
        public const int DBCAPI_DM_GENERALLONG = 19;

        public static short DBCAPI_LVL2_BID_PRESENT = 0x00000001;
        public static short DBCAPI_LVL2_ASK_PRESENT = 0x00000002;

        public static byte DBCAPI_ECN_ISLAND_DATA = 0x00000008; // to be passed with request
        public static byte DBCAPI_ECN_ARCA_DATA = 0x00000020;

        public const int DBCAPI_FLAG_USE_DM_MESSAGE_EX = 0x00000004;/// DBCAPI_FLAG_USE_DM_MESSAGE_EX -> 0x00000004
        public const uint DBCAPI_FLAG_REMOTEDM = 0x80000000;/// The dbcapi flag remotedm
        public const uint DBCAPI_FLAG_WINROSMT = 0x01000000;/// The dbcapi flag winrosmt
        public const uint DBCAPI_DATA_MANAGER = 0x0001;/// Server Data Manager 
        public const uint WM_USER_DATA_MANAGER = 0x0400 + 2;/// 
    }
}
