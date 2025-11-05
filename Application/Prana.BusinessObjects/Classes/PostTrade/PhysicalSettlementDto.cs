namespace Prana.PostTrade.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class PhysicalSettlementDto
    {
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the type of the transaction type.
        /// </summary>
        /// <value>
        /// The type of the transaction type.
        /// </value>
        public string TransactionType { get; set; }
    }
}
