using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.Classes.Allocation
{
    [Serializable]
    public class CurrentSymbolState
    {
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the notional.
        /// </summary>
        /// <value>
        /// The notional.
        /// </value>
        public decimal Notional { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSymbolState"/> class.
        /// </summary>
        public CurrentSymbolState()
            : this(string.Empty, -1, 0.0M, 0.0M, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSymbolState"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="notional">The notional.</param>
        public CurrentSymbolState(string symbol, int accountId, decimal quantity, decimal notional, string accountName)
        {
            try
            {
                this.Symbol = symbol;
                this.AccountId = accountId;
                this.Quantity = quantity;
                this.Notional = notional;
                this.AccountName = accountName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
