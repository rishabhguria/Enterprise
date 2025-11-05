namespace Prana.BusinessObjects.Compliance.Enums
{
    public enum AlertPopUpType
    {
        None,

        /// <summary>
        /// Compliance check (from work aera)
        /// </summary>
        ComplianceCheck,

        /// <summary>
        /// Ask the user for Override
        /// </summary>
        Override,

        /// <summary>
        /// Inform the user about the blocked trades
        /// </summary>
        Inform,

        /// <summary>
        /// 
        /// </summary>
        PendingApproval,

        /// <summary>
        /// Basket compliance check
        /// </summary>
        BasketComplianceCheck,

        /// <summary>
        /// Inform the user about the basket blocked trades
        /// </summary>
        BasketOverride
    }
}
