using Prana.Global;
using System;

namespace Prana.BusinessObjects.Compliance.Constants
{
    /// <summary>
    /// Constants for user defined rules
    /// WebDav and rest Api paths
    /// File formats
    /// username and password.
    /// </summary>
    public class UserDefinedRuleConstants
    {
        public static String SERVER = "http://localhost:8080";
        public static String GUVNOR_STANDALONE_BASE_URL = string.Empty;
        public static String GUVNOR_REST_BASE_URL = string.Empty;

        static UserDefinedRuleConstants()
        {
            SERVER = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleServer);
            GUVNOR_STANDALONE_BASE_URL = SERVER + "/" + UserDefinedRuleConstants.GUVNOR_WARNAME + "/org.drools.guvnor.Guvnor/standaloneEditorServlet?";
            GUVNOR_REST_BASE_URL = SERVER + "/" + UserDefinedRuleConstants.GUVNOR_WARNAME + "/rest";
        }

        public const String ADMIN = "admin";
        public const String USER = "user";

        public const String RULE_FILE_FORMATE = "brl";


        public const String GUVNOR_WARNAME = "drools-guvnor";
        public static String CLIENT_NAME = "admin";
        public static String CLIENT_PASSWORD = "admin";
    }
}
