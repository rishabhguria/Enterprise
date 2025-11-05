using System.Collections.Generic;

namespace Prana.CashManagement.Classes
{
    public class CashJournalMappingItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the mapping identifier.
        /// </summary>
        /// <value>
        /// The mapping identifier.
        /// </value>
        public int MappingID { get; set; }

        /// <summary>
        /// Gets or sets the debit account.
        /// </summary>
        /// <value>
        /// The debit account.
        /// </value>
        public List<int> DebitAccount { get; set; }

        /// <summary>
        /// Gets or sets the credit account.
        /// </summary>
        /// <value>
        /// The credit account.
        /// </value>
        public List<int> CreditAccount { get; set; }
        #endregion
    }
}
