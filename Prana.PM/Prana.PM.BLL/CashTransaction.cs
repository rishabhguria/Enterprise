//
using Prana.BusinessObjects;

namespace Prana.PM.BLL
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
