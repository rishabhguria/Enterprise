using System.Collections.Generic;

namespace Prana.Allocation.Common.Definitions
{
    public class AllocationFilterFields
    {
        /// <summary>
        /// The _symbol
        /// </summary>
        private string _symbol;

        /// <summary>
        /// The _asset
        /// </summary>
        private Dictionary<int, string> _asset;

        /// <summary>
        /// The _side
        /// </summary>
        private Dictionary<string, string> _side;

        /// <summary>
        /// The _account
        /// </summary>
        private Dictionary<int, string> _account;

        /// <summary>
        /// The _fund
        /// </summary>
        private Dictionary<int, string> _fund;

        /// <summary>
        /// The _broker
        /// </summary>
        private Dictionary<int, string> _broker;

        /// <summary>
        /// The _currency
        /// </summary>
        private Dictionary<int, string> _currency;

        /// <summary>
        /// The _exchange
        /// </summary>
        private Dictionary<int, string> _exchange;

        /// <summary>
        /// The _strategy
        /// </summary>
        private Dictionary<int, string> _strategy;

        /// <summary>
        /// The _underlying
        /// </summary>
        private Dictionary<int, string> _underlying;

        /// <summary>
        /// The _venue
        /// </summary>
        private Dictionary<int, string> _venue;

        /// <summary>
        /// The _trading account
        /// </summary>
        private Dictionary<int, string> _tradingAccount;

        /// <summary>
        /// The _bool list
        /// </summary>
        private List<string> _boolList;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>
        /// The asset.
        /// </value>
        public Dictionary<int, string> Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>
        /// The side.
        /// </value>
        public Dictionary<string, string> Side
        {
            get { return _side; }
            set { _side = value; }
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Dictionary<int, string> Account
        {
            get { return _account; }
            set { _account = value; }
        }

        /// <summary>
        /// Gets or sets the Fund.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public Dictionary<int, string> Fund
        {
            get { return _fund; }
            set { _fund = value; }
        }


        /// <summary>
        /// Gets or sets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        public Dictionary<int, string> Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public Dictionary<int, string> Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        /// <value>
        /// The exchange.
        /// </value>
        public Dictionary<int, string> Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>
        /// The strategy.
        /// </value>
        public Dictionary<int, string> Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        /// <summary>
        /// Gets or sets the underlying.
        /// </summary>
        /// <value>
        /// The underlying.
        /// </value>
        public Dictionary<int, string> Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }

        /// <summary>
        /// Gets or sets the venue.
        /// </summary>
        /// <value>
        /// The venue.
        /// </value>
        public Dictionary<int, string> Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        /// <summary>
        /// Gets or sets the trading account.
        /// </summary>
        /// <value>
        /// The trading account.
        /// </value>
        public Dictionary<int, string> TradingAccount
        {
            get { return _tradingAccount; }
            set { _tradingAccount = value; }
        }

        /// <summary>
        /// Gets or sets the bool list.
        /// </summary>
        /// <value>
        /// The bool list.
        /// </value>
        public List<string> BoolList
        {
            get { return _boolList; }
            set { _boolList = value; }
        }
    }
}
