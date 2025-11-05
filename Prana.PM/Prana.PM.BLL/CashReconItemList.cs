//
using Prana.BusinessObjects;
using System;

namespace Prana.PM.BLL
{
    public class CashReconItemList : SortableSearchableList<CashReconItem>
    {
        /// <summary>
        /// Gets the retrieve.
        /// </summary>
        /// <value>The retrieve.</value>
        public static SortableSearchableList<CashReconItem> Retrieve
        {
            get { return GetAllCashReconItems(); }
        }

        private static SortableSearchableList<CashReconItem> GetAllCashReconItems()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
