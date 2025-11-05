using System;
using System.IO;

namespace Prana.BusinessObjects.Compliance.Constants
{
    /// <summary>
    /// Constants for custom rules.
    /// Path for Template.
    /// path for where to save html file of rules.
    /// Tags to be replaced while making htmll for rules.
    /// </summary>
    public class CustomRuleConstants
    {
        public static String CUSTOM_RULE_PATH = string.Empty;
        public static String CUSTOM_RULE_TEMPLATE_PATH = string.Empty;

        static CustomRuleConstants()
        {
            CUSTOM_RULE_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\CustomRuleHtml";
            CUSTOM_RULE_TEMPLATE_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\HtmlFiles\\CustomRule.htm";
        }

        public const String PH_RULE_NAME = "#RULE_NAME#";
        public const String PH_DESCRIPTION = "#DESCRIPTION#";
        public const String PH_CONSTANTHTML = "#RULE_PARAMETER_CONSTANTS#";
        public const String PH_COMPLIANCE_LEVEL = "#COMPRESSION_LEVEL#";
        public const String CUSTOM_RULE_EXPORT_ROUTING_KEY = "ExportCustomRule";
        public const String CUSTOM_RULE_IMPORT_ROUTING_KEY = "ImportCustomRule";
    }
}
