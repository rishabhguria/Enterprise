using System;
//using System.EnterpriseServices;

namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// Interface between the level1 data provided by the eSignal and Prana
    /// </summary>
    [Serializable]
    public class Level1Data
    {
        public Level1Data()
        {
            _symbol = string.Empty;
            _last = double.MinValue;
            _bidPrice = double.MinValue;
            _askPrice = double.MinValue;
            _high = double.MinValue;
            _low = double.MinValue;
            _volume = long.MinValue;
            _open = double.MinValue;
            _change = double.MinValue;
            _lastTick = string.Empty;
            _currencyCode = string.Empty;
        }

        string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        double _last;
        public double Last
        {
            get { return _last; }
            set { _last = value; }
        }

        double _bidPrice;
        public double Bid
        {
            get { return _bidPrice; }
            set { _bidPrice = value; }
        }

        double _askPrice;
        public double Ask
        {
            get { return _askPrice; }
            set { _askPrice = value; }
        }

        double _high;
        public double High
        {
            get { return _high; }
            set { _high = value; }
        }

        double _low;
        public double Low
        {
            get { return _low; }
            set { _low = value; }
        }

        long _volume;
        /// <summary>
        /// It is the cumulative volume
        /// </summary>
		public long Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        double _open;
        public double Open
        {
            get { return _open; }
            set { _open = value; }
        }


        double _change;
        public double Change
        {
            get { return _change; }
            set { _change = value; }
        }

        string _lastTick;
        public string LastTick
        {
            get { return _lastTick; }
            set { _lastTick = value; }
        }


        private string _currencyCode;
        public string CurencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        private double _vwap;

        /// <summary>
        /// Gets or sets the Volume Waited Average Price.
        /// </summary>
        /// <value>The VWAP.</value>
        public double VWAP
        {
            get { return _vwap; }
            set { _vwap = value; }
        }

    }
}
