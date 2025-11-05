using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    public class CloseTradeMatchedTradeReportItem
    {
        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private DateTime _tradeDate;

        /// <summary>
        /// Gets or sets the trade date.
        /// </summary>
        /// <value>The trade date.</value>
        public DateTime TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }

        private long _totalQty;

        /// <summary>
        /// Gets or sets the total qty.
        /// </summary>
        /// <value>The total qty.</value>
        public long TotalQty
        {
            get { return _totalQty; }
            set { _totalQty = value; }
        }

        private Account _account;

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public Account Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new Account();
                }
                return _account;
            }
            set { _account = value; }
        }

        private long _closedQty;

        /// <summary>
        /// Gets or sets the closed qty.
        /// </summary>
        /// <value>The closed qty.</value>
        public long ClosedQty
        {
            get { return _closedQty; }
            set { _closedQty = value; }
        }

        private long _openQty;

        /// <summary>
        /// Gets or sets the open qty.
        /// </summary>
        /// <value>The open qty.</value>
        public long OpenQty
        {
            get { return _openQty; }
            set { _openQty = value; }
        }

        private double _realizedPNL;

        /// <summary>
        /// Gets or sets the realized PNL.
        /// </summary>
        /// <value>The realized PNL.</value>
        public double RealizedPNL
        {
            get { return _realizedPNL; }
            set { _realizedPNL = value; }
        }


    }
}
