//
using Prana.BusinessObjects;
using System;

namespace Prana.PM.BLL
{
    public class CashTransactionList : SortableSearchableList<CashBalanceEntry>
    {
        /// <summary>
        /// Gets the retrieve.
        /// </summary>
        /// <value>The retrieve.</value>
        public static SortableSearchableList<CashBalanceEntry> Retrieve
        {
            get { return GetAllCashTransactions(); }
        }

        /// <summary>
        /// Gets all trade transactions.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<CashBalanceEntry> GetAllCashTransactions()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
