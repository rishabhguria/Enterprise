using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using System.Windows.Forms;

namespace Prana.RuleEngine
{
    internal class Constants
    {
        public static String SERVER = "http://localhost:8080";
        public static String GUVNOR_STANDALONE_BASE_URL = string.Empty;
        public static String GUVNOR_REST_BASE_URL = string.Empty;
        public static String CUSTOM_RULE_PATH = string.Empty;
        public static String CUSTOM_RULE_TEMPLATE_PATH = string.Empty;

        static Constants()
        {
            SERVER = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleServer); 
            GUVNOR_STANDALONE_BASE_URL = SERVER + "/" + Constants.GUVNOR_WARNAME + "/org.drools.guvnor.Guvnor/standaloneEditorServlet?";
            GUVNOR_REST_BASE_URL = SERVER + "/" + Constants.GUVNOR_WARNAME + "/rest";
            CUSTOM_RULE_PATH = Application.StartupPath + "\\CustomRuleHtml";
            CUSTOM_RULE_TEMPLATE_PATH = Application.StartupPath + "\\CustomRule.htm";
        }

        public const String CATEGORY_TAG = "category";
        public const String USER_DEFINED_RULES_TAG = "UserDefinedRules";
        public const String RULE_TAG = "rule";
        public const String CUSTOM_RULE_NODE_TAG = "customRuleNode";
        public const String CUSTOM_RULE_TAG = "customRule";
       
        public const String ADMIN = "admin";
        public const String USER = "user";
       
        public const String RULE_FILE_FORMATE = "brl";

        
        public const String GUVNOR_WARNAME = "drools-guvnor";
        public static String CLIENT_NAME = "admin";
        public static String CLIENT_PASSWORD = "admin";
        
        public const String SIXTEEN_B_SYMBOL = "SIXTEEN_B_SYMBOL";
        public const String SIXTEEN_B_DATE = "SIXTEEN_B_DATE";
        
        public const String POST_TRADE = "PostTrade";
        public const String PRE_TRADE = "PreTrade";
        
        public const String PRE_TRADE_COMPLIANCE = "PreTradeCompliance";
        public const String POST_TRADE_COMPLIANCE = "PostTradeCompliance";
        
        public const String PH_RULE_NAME = "#RULE_NAME#";
        public const String PH_DESCRIPTION = "#DESCRIPTION#";
        public const String PH_COMPLIANCE_LEVEL = "#COMPRESSION_LEVEL#";
      
        
        
    }
}
