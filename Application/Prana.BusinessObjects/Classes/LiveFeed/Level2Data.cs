using System;

namespace Prana.BusinessObjects.LiveFeed
{
    [Serializable]
    public class Level2Data
    {
        //		private DateTime _enqueuetime;
        //		public DateTime Enqueuetime
        //		{
        //			get{return _enqueuetime;}
        //			set{_enqueuetime = value;}
        //		}

        string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        string _exchange = string.Empty;
        /// <summary>
        /// NSDQ, ISLAND, ARCA
        /// </summary>
        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }

        char _updateFlag = char.MinValue;
        /// <summary>
        /// Add, Delete and modify record
        /// </summary>
        public char UpdateFlag
        {
            get { return _updateFlag; }
            set { _updateFlag = value; }
        }

        bool _isBidPresent = false;
        public bool IsBidPresent
        {
            get { return _isBidPresent; }
            set { _isBidPresent = value; }
        }

        [field: NonSerializedAttribute()]
        private MarketMakerRecord _bidInfo;
        public MarketMakerRecord BidInfo
        {
            get { return _bidInfo; }
            set
            {
                _bidInfo = value;
            }
        }


        bool _isAskPresent = false;
        public bool IsAskPresent
        {
            get { return _isAskPresent; }
            set { _isAskPresent = value; }
        }

        [field: NonSerializedAttribute()]
        private MarketMakerRecord _askInfo;
        public MarketMakerRecord AskInfo
        {
            get { return _askInfo; }
            set { _askInfo = value; }
        }

        #region commented 
        //		string _mmid = string.Empty;
        //		public string Mmid
        //		{
        //			get{return _mmid;}
        //			set{_mmid = value;}
        //		}
        //
        //		double _bidPrice = double.MinValue ;
        //		public double BidPrice
        //		{
        //			get{return _bidPrice;}
        //			set{_bidPrice = value;}
        //		}
        //
        //		long _bidSize = long.MinValue ;
        //		public long BidSize
        //		{
        //			get{return _bidSize;}
        //			set{_bidSize = value;}
        //		}
        //
        //		string _bidTime = string.Empty;
        //		public string BidTime
        //		{
        //			get{return _bidTime;}
        //			set{_bidTime = value;}
        //		}

        //		double _askPrice = double.MinValue ;
        //		public double AskPrice
        //		{
        //			get{return _askPrice;}
        //			set{_askPrice = value;}
        //		}
        //
        //		long _askSize = long.MinValue ;
        //		public long AskSize
        //		{
        //			get{return _askSize;}
        //			set{_askSize = value;}
        //		}
        //
        //		string _askTime = string.Empty ;
        //		public string AskTime
        //		{
        //			get{return _askTime;}
        //			set{_askTime = value;}
        //		}



        #endregion



    }
}
