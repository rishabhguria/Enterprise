using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Utilities.UI.CronUtility
{
    /// <summary>
    /// CronDescription class consists of properties to describe UI according to cron expression
    /// </summary>
    public class CronDescription
    {
        /// <summary>
        /// start time on which task gets started
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// start date on which task gets started
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// type of task scheduled
        /// </summary>
        public ScheduleType Type { get; set; }

        /// <summary>
        // task gets started after every recurDay
        /// </summary>
        public Decimal RecurDay { get; set; }

        /// <summary>
        /// List of WeekDays on which task execute every week
        /// </summary>
        public List<String> WeeklyDaysList { get; set; }

        /// <summary>
        /// List of month on which task gets executed
        /// </summary>
        public List<String> MonthlyMonthsList { get; set; }

        /// <summary>
        /// List of monthDays on which task gets executed
        /// </summary>
        public List<String> MonthlyDaysList { get; set; }

        /// <summary>
        /// List of weekNumber in month on which task gets executed
        /// </summary>
        public List<String> MonthlyWeekNumbersList { get; set; }

        /// <summary>
        /// List of weekDays in month on which task gets executed
        /// </summary>
        public List<String> MonthlyWeekDaysList { get; set; }

        /// <summary>
        /// DayOfMonth mode in Monthly task schedule
        /// </summary>
        public bool MonthlyModeDayOfMonth { get; set; }

        /// <summary>
        /// Weekday mode in Monthly task schedule
        /// </summary>
        public bool MonthlyModeWeekday { get; set; }

        /// <summary>
        ///  constructor to initialize all string Lists
        /// </summary>
        public CronDescription()
        {
            try
            {
                WeeklyDaysList = new List<string>();
                MonthlyMonthsList = new List<string>();
                MonthlyDaysList = new List<string>();
                MonthlyWeekNumbersList = new List<string>();
                MonthlyWeekDaysList = new List<string>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
    }
}