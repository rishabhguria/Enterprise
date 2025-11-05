using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CashTransaction
    {
        private SortableSearchableList<CorporateActionEntry> _corporateActionEntryList;

        public SortableSearchableList<CorporateActionEntry> CorporateActionEntryList
        {
            get { return _corporateActionEntryList; }
            set { _corporateActionEntryList = value; }
        }
	
    }
}
