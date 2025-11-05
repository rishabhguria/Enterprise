using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashBalanceEntry
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
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new DataSourceNameID();
                }
                return _dataSourceNameID;
            }
            set { _dataSourceNameID = value; }
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

        private string _symbol;
        /// <summary>
        /// Added Rajat 02 Nov 2006
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
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

        private double _openingCash;

        /// <summary>
        /// Gets or sets the opening cash.
        /// </summary>
        /// <value>The opening cash.</value>
        public double OpeningCash
        {
            get { return _openingCash; }
            set { _openingCash = value; }
        }

        private double _tradingCashSpent;

        public double TradingCashSpent
        {
            get { return _tradingCashSpent; }
            set { _tradingCashSpent = value; }
        }

        private double _tradingCashReceived;

        /// <summary>
        /// Gets or sets the trading cash received.
        /// </summary>
        /// <value>The trading cash received.</value>
        public double TradingCashReceived
        {
            get { return _tradingCashReceived; }
            set { _tradingCashReceived = value; }
        }

        private string _transactions;

        public string Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }
	
        private double _netAmount = double.MinValue;

        /// <summary>
        /// Gets or sets the net amount.
        /// </summary>
        /// <value>The net amount.</value>
        public double NetAmount
        {
            get { return _netAmount; }
            set { _netAmount = value; }
        }

        private double  _projectedClosingCash;

        public double  ProjectedClosingCash
        {
            get { return _projectedClosingCash; }
            set { _projectedClosingCash = value; }
        }
	
    }
}
