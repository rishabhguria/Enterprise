namespace Prana.BusinessObjects.Compliance.Enums
{
    /// <summary>
    /// Type of Pre trade check
    /// </summary>
    public enum PreTradeType
    {
        /// <summary>
        /// Normal what if trade
        /// </summary>
        Trade,
        /// <summary>
        /// Staging
        /// </summary>
        Stage,
        /// <summary>
        /// Rebalancer work area
        /// </summary>
        ComplianceCheck
    }
}
