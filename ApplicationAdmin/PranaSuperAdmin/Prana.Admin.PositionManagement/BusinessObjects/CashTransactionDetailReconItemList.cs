using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashTransactionDetailReconItemList:SortableSearchableList<CashTransactionDetailReconItem>
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
