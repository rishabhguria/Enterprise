//
using Prana.BusinessObjects;
using System;

namespace Prana.PM.BLL
{
    public class CashTransactionDetailReconItemList : SortableSearchableList<CashTransactionDetailReconItem>
    {
        /// <summary>
        /// Gets the retrieve.
        /// </summary>
        /// <value>The retrieve.</value>
        public static SortableSearchableList<CashTransactionDetailReconItem> Retrieve
        {
            get { return GetAllCashTransactionDetailReconItems(); }
        }

        private static SortableSearchableList<CashTransactionDetailReconItem> GetAllCashTransactionDetailReconItems()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
