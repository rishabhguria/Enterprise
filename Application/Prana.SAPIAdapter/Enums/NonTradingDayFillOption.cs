using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Non Trading Day Fill Option: Sets to include/exclude non trading days where no data was generated.
    /// </summary>
    [Serializable]
    public enum NonTradingDayFillOption
    {
        /// <summary>
        /// The none
        /// </summary>
        [ElementValue(null)]
        None,
        /// <summary>
        /// Include all weekdays (Monday to Friday) in the data set
        /// </summary>
        [ElementValue("NON_TRADING_WEEKDAYS")]
        NonTradingWeekdays,
        /// <summary>
        /// Include all days of the calendar in the data set returned
        /// </summary>
        [ElementValue("All_CALENDAR_DAYS")]
        AllCalendarDays,
        /// <summary>
        /// Include only active days (days where the instrument and field pair updated) in the data set returned
        /// </summary>
        [ElementValue("ACTIVE_DAYS_ONLY")]
        ActiveDaysOnly
    }
}
