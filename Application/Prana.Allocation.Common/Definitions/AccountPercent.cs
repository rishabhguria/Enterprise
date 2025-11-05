using System;

namespace Prana.Allocation.Common.Definitions
{
    public class AccountPercent
    {
        public readonly int AccountID;

        /// <summary>
        /// The original percent
        /// </summary>
        public readonly decimal OriginalPercent;

        /// <summary>
        /// The current qty
        /// </summary>
        public readonly decimal CurrentQty;

        /// <summary>
        /// The target percent
        /// </summary>
        public readonly decimal TargetPercent;

        /// <summary>
        /// The initial deviation
        /// </summary>
        public readonly decimal initialDeviation;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPercent"/> class.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="originalPercent">The original percent.</param>
        /// <param name="targetPercent">The target percent.</param>
        /// <param name="currentQty">The current qty.</param>
        public AccountPercent(int accountID, decimal originalPercent, decimal targetPercent, decimal currentQty)
        {
            this.AccountID = accountID;
            this.OriginalPercent = originalPercent;
            this.CurrentQty = currentQty;
            this.TargetPercent = targetPercent;
            this.initialDeviation = Math.Abs(targetPercent - originalPercent);
        }
    }
}
