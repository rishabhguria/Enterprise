using System;

namespace Prana.BusinessObjects
{
    public class MarketTimes
    {
        private DateTime _marketStartTime = DateTimeConstants.MinValue;
        public DateTime MarketStartTime
        {
            get { return _marketStartTime; }
            set { _marketStartTime = value; }
        }

        private DateTime _marketEndTime = DateTimeConstants.MinValue;
        public DateTime MarketEndTime
        {
            get { return _marketEndTime; }
            set { _marketEndTime = value; }
        }

        private DateTime _marketClearanceTime = DateTimeConstants.MinValue;
        public DateTime MarketClearanceTime
        {
            get { return _marketClearanceTime; }
            set { _marketClearanceTime = value; }
        }
    }
}