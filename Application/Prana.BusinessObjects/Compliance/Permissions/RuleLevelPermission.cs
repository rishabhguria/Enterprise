using Prana.BusinessObjects.Compliance.Enums;

namespace Prana.BusinessObjects.Compliance.Permissions
{
    /// <summary>
    /// Rule Level individual permission
    /// </summary>
    public class RuleLevelPermission
    {
        public string RuleId { get; set; }
        public bool OverridePermission { get; set; }
        public string RuleName { get; set; }
        public RuleOverrideType RuleOverrideType { get; set; }

        public RuleLevelPermission(string ruleId, bool overRiddenPer, string ruleName, RuleOverrideType ruleOverrideType)
        {
            RuleId = ruleId;
            OverridePermission = overRiddenPer;
            RuleName = ruleName;
            RuleOverrideType = ruleOverrideType;
        }
    }
}
