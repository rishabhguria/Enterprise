using System;

namespace Prana.BusinessObjects
{
    public class TimeZoneAndTime
    {
        private string timeZone;
        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        private DateTime baseTime;
        public DateTime BaseTime
        {
            get { return baseTime; }
            set { baseTime = value; }
        }
    }
}
