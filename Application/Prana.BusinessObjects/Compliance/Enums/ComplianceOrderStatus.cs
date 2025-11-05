using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Compliance.Enums
{
    public enum ComplianceOrderStatus
        {
            None = 0,

            /// <summary>
            /// when user creates order
            /// </summary>
            New = 1,

            /// <summary>
            /// when request goes to CO for approval.
            /// </summary>
            PendingApproval = 2,

            /// <summary>
            /// when CO approves the order
            /// </summary>
            Approved = 3,

            /// <summary>
            /// when broker Acknowledged/Execute the order.
            /// </summary>
            Acknowledged = 4
        }
    
}
