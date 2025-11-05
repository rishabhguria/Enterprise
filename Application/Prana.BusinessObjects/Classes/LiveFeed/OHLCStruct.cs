using System;

namespace Prana.BusinessObjects.LiveFeed
{
    public struct OHLCStruct
    {
        public DateTime BarStartTime;
        public long BarLength;
        public string symbol;
        public double open;
        public double close;
        public double high;
        public double low;
        public double volume;
        public double jDate; // Julian date


    }


}
