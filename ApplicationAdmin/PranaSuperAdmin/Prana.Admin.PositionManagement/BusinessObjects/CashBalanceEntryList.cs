using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashBalanceEntryList:SortableSearchableList<CashBalanceEntry>
    {
        /// <summary>
        /// Gets the retrieve.
        /// </summary>
        /// <value>The retrieve.</value>
        public static SortableSearchableList<CashBalanceEntry> Retrieve
        {
            get { return GetAllCBMTrades(); }
        }

        private static SortableSearchableList<CashBalanceEntry> GetAllCBMTrades()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
