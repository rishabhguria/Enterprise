using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// Responsible for Cash Reconciliation!
    /// </summary>
    class CashRecon
    {
        private DateTime _date = DateTime.Today;

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

        private CashReconItemList _cashReconItemList;

        /// <summary>
        /// Gets or sets the cash recon item list.
        /// </summary>
        /// <value>The cash recon item list.</value>
        public CashReconItemList CashReconItemList
        {
            get { return _cashReconItemList; }
            set { _cashReconItemList = value; }
        }
	
    }
}
