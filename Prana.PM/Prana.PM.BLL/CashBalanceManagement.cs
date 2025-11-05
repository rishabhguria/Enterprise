//
using Prana.BusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    public class CashBalanceManagement
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

        private SortableSearchableList<CashBalanceEntry> _cashBalanceManagementDataListItems;

        public SortableSearchableList<CashBalanceEntry> CashBalanceManagementDataListItems
        {
            get { return _cashBalanceManagementDataListItems; }
            set { _cashBalanceManagementDataListItems = value; }
        }

    }
}
