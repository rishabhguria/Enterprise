using System;

namespace Prana.BusinessObjects.LiveFeed
{
    public struct TickStruct
    {
        public DateTime TickTime;
        public string symbol;
        public double tradePrice;
        public double tradeVolume;
        public double bidPrice;
        public double askPrice;
        public double askSize;
        public double bidSize;

    }
}
