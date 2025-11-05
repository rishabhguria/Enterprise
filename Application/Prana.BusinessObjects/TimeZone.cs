using System;

namespace Prana.BusinessObjects
{
    public class TimeZone
    {
        public string TimeZoneKey { get; set; }

        public string DisplayName { get; set; }

        public string StandardName { get; set; }

        public string DaylightName { get; set; }

        public int Index { get; set; }

        public bool SupportsDaylightSavings { get; set; }

        public TimeSpan Bias { get; set; }

        public TimeSpan DaylightBias { get; set; }

        public DateTime StandardTransitionTimeOfDay { get; set; }

        public int StandardTransitionMonth { get; set; }

        public int StandardTransitionWeek { get; set; }

        public int StandardTransitionDayOfWeek { get; set; }


        public DateTime DaylightTransitionTimeOfDay { get; set; }

        public int DaylightTransitionMonth { get; set; }


        public int DaylightTransitionWeek { get; set; }

        public int DaylightTransitionDayOfWeek { get; set; }
    }
}
