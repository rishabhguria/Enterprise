using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashReconItemList:SortableSearchableList<CashReconItem>
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
