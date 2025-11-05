//
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    public class CashTransactionManagement
    {
        private DateTime _date;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameID
        {
            get { return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }


        private SortableSearchableList<DividendEntry> _dividendEntryListItems;


        /// <summary>
        /// Gets or sets the dividend entry list items.
        /// </summary>
        /// <value>The dividend entry list items.</value>
        public SortableSearchableList<DividendEntry> DividendEntryListItems
        {
            get { return _dividendEntryListItems; }
            set { _dividendEntryListItems = value; }
        }

        private SortableSearchableList<CorporateActionEntry> _corporateActionEntryListItems;

        /// <summary>
        /// Gets or sets the corporate actions list.
        /// </summary>
        /// <value>The corporate actions list.</value>
        public SortableSearchableList<CorporateActionEntry> CorporateActionEntryListItems
        {
            get { return _corporateActionEntryListItems; }
            set { _corporateActionEntryListItems = value; }
        }

    }
}
