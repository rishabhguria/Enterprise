using System.ComponentModel;

namespace Prana.ServiceGateway.Constants
{
    public class GlobalConstants
    {
        #region Authentication Constants

        public static readonly string UsernameHeaderKey = "x-nirvana-web-username";
        public static readonly string CONST_TOKEN = "token";
        public static readonly string CONST_BEARER = "Bearer";
        public static readonly string CONST_ACCESS_TOKEN = "access_token";
        public static readonly string CONST_AUTHSETTINGS_SECRET_KEY = "AuthSettings:tokenSymmetricKey";
        public static readonly string CONST_AUTHSETTINGS_SALT = "AuthSettings:saltValue";
        public static readonly string CONST_TOKEN_SALT = "tokenSalt";
        public static readonly string CONST_HEADER_AUTHORIZATION = "Authorization";
        public static readonly string SWAGGER_CLIENT = "is-swagger-client";
        public static readonly string USERID = "userId";
        public static readonly string BEARER = "Bearer";
        public static readonly string CONST_CORRELATION_ID = "X-Correlation-ID";
        public enum AuthenticationTypes
        {
            EnterpriseLoggedIn,
            WebLoggedIn,
            InvalidCredentials,
            WebAlreadyLoggedInForAnotherWebSession
        }

        #endregion

        #region Configuration Constants
        public static readonly string REQUEST_FILTERING_BLOCKHOST = "RequestFiltering:BlockedHost";
        public static readonly string REQUEST_FILTERING_BLOCKPATHS = "RequestFiltering:BlockedPaths";

        #endregion

        #region Security Validation Constants 
        public enum SymbologyCodes
        {
            [Description("Ticker Symbol")]
            TickerSymbol = 0,
            [Description("Reuters Symbol")]
            ReutersSymbol = 1,
            [Description("ISIN Symbol")]
            ISINSymbol = 2,
            [Description("SEDOL Symbol")]
            SEDOLSymbol = 3,
            [Description("CUSIP Symbol")]
            CUSIPSymbol = 4,
            [Description("Bloomberg Symbol")]
            BloombergSymbol = 5,
            [Description("OSIOption Symbol")]
            OSIOptionSymbol = 6,
            [Description("IDCOOption Symbol")]
            IDCOOptionSymbol = 7,
            [Description("OPRAOption Symbol")]
            OPRAOptionSymbol = 8,
            [Description("FactSet Symbol")]
            FactSetSymbol = 9,
            [Description("Activ Symbol")]
            ActivSymbol = 10
        }
        #endregion
        #region Service Health 
        public static readonly string SERVICE_STATUS_CACHE_KEY = "ServiceHealthStatusCollection";
        #endregion
    }
}
