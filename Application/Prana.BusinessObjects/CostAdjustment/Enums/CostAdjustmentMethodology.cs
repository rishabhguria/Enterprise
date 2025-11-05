namespace Prana.BusinessObjects.CostAdjustment.Definitions
{
    /// <summary>
    /// Enum CostAdjustmentMethodology
    /// </summary>
    public enum CostAdjustmentMethodology
    {
        /// <summary>
        /// The fifo Sort date ascending
        /// </summary>
        FIFO = 1,
        /// <summary>
        /// The lifo Sort date Descending
        /// </summary>
        LIFO = 2,
        /// <summary>
        /// The hifo Sort Avg price ascending
        /// </summary>
        HIFO = 3,
        /// <summary>
        /// The hiho Sort Avg price Descending
        /// </summary>
        HIHO = 4
    }
}
