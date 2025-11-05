using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Compliance.Enums
{
    public enum AlertType
    {

        [EnumDescription("Hard")]
        HardAlert,
        [EnumDescription("Requires Approval")]
        RequiresApproval,
        [EnumDescription("Soft with Notes")]
        SoftAlertWithNotes,
        [EnumDescription("Soft")]
        SoftAlert
    }
}
