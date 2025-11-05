namespace Prana.ServiceGateway.Constants
{
    public class UtilityConstants
    {
        #region TokenDecrypterMiddleware constants

        public static string CONST_UNDEFINED = "undefined";
        #endregion


        #region TokenService constants

        public static string CONST_AUTH_SETTINGS_SECRET_KEY = "AuthSettings:secretKey";
        public static string CONST_AUTH_SETTINGS_EXPIRY_MINUTES = "AuthSettings:expiryMinutes";
        public static string CONST_AUTH_SETTINGS_ISSUER = "AuthSettings:issuer";
        public static string CONST_AUTH_SETTINGS_AUDIENCE = "AuthSettings:audience";
        public static string CONST_AUTHORIZATION = "authorization";
        public static string CONST_TRUE = "true";

        #endregion



        public static string CONST_CLIENT_TENANT_ID = "AzureAd:TenantId";
        public static string CONST_CLIENT_CLIENT_ID = "AzureAd:ClientIds";
        public static string CONST_ISSUERS = "AzureAd:Issuers";
        public static string CONST_CLIENT_CLIENT_SECRET = "AzureAd:ClientSecret";

    }
}
