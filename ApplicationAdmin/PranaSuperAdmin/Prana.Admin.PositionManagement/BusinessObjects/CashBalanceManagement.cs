using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashBalanceManagement
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

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get { return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }

        private  SortableSearchableList<CashBalanceEntry> _cashBalanceManagementDataListItems;

        public SortableSearchableList<CashBalanceEntry> CashBalanceManagementDataListItems
        {
            get { return _cashBalanceManagementDataListItems; }
            set { _cashBalanceManagementDataListItems = value; }
        }
	
    }
}
