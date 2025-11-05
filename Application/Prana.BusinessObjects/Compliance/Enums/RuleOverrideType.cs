using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Compliance.Enums
{
    public enum RuleOverrideType
    {
        [EnumDescription("Soft")]
        Soft = 1,
        [EnumDescription("Requires Approval")]
        RequiresApproval = 2,
        [EnumDescription("Hard")]
        Hard = 3,
        [EnumDescription("Soft with Notes")]
        SoftWithNotes = 4
    }
}
