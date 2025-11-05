using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Compliance.Enums
{
    public enum PreTradeActionType
    {
        [EnumDescription("Blocked")]
        Blocked,
        [EnumDescription("Approved")]
        Allowed,
        [EnumDescription("Pending Approval")]
        NoAction
    }
}
