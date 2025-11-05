using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashReconItem
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get {
                if (_dataSourceNameID==null)
                {
                    _dataSourceNameID = new DataSourceNameID();
                }
                return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }

        private Currency _currency;

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public Currency Currency
        {
            get
            {
                if (_currency == null)
                {
                    _currency = new Currency();
                }
                return _currency; 
            }
            set { _currency = value; }
        }

        private Fund _fund;

        /// <summary>
        /// Gets or sets the fund.
        /// </summary>
        /// <value>The fund.</value>
        public Fund Fund
        {
            get 
            {
                if (_fund == null)
                {
                    _fund = new Fund();
                }
                return _fund; 
            }
            set { _fund = value; }
        }

        private double _estimatedClosingBalance;

        /// <summary>
        /// Gets or sets the projected closing balance.
        /// </summary>
        /// <value>The projected closing balance.</value>
        public double EstimatedClosingBalance
        {
            get { return _estimatedClosingBalance; }
            set { _estimatedClosingBalance = value; }
        }

        private double _balanceBF;

        /// <summary>
        /// Gets or sets the balance B/F.
        /// </summary>
        /// <value>The balance B/F.</value>
        public double BalanceBF
        {
            get { return _balanceBF; }
            set { _balanceBF = value; }
        }

        private double _difference = double.MinValue;

        /// <summary>
        /// Gets or sets the difference.
        /// </summary>
        /// <value>The difference.</value>
        public double Difference
        {
            get { return _difference; }
            set { _difference = value; }
        }


        private double _manualEntry;

        /// <summary>
        /// Gets or sets the manual entry.
        /// </summary>
        /// <value>The manual entry.</value>
        public double ManualEntry
        {
            get { return _manualEntry; }
            set { _manualEntry = value; }
        }

        private ReconStatus _status;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ReconStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
	
       
    }
}
