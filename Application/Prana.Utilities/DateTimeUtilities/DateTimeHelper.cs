using System;
using System.Globalization;

namespace Prana.Utilities.DateTimeUtilities
{
    public class DateTimeHelper
    {
        public static DateTime GetYesterdayDate()
        {
            DateTime today = DateTime.Today;
            return today.DayOfWeek == DayOfWeek.Monday ? today.AddDays(-3) : today.AddDays(-1);
        }

        /// <summary>
        /// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=1002193&SiteID=1
        /// http://www.precharge.net/forums/asp/448-get-week-number.html
        /// http://www.codecogs.com/cog-238
        /// Find the day in the local timezone
        /// - Check for local timezone
        /// - Check in Leap year
        /// - Check the Business Day using Business day calculator.
        /// </summary>
        /// <param name="Nth">Nth identifies which WeekDay from the start of the month to obtain (e.g. the 1st Friday, 3rd Monday). A zero value will return the date of the last WeekDay, with negative values returning earlier weekdays from the end of the month.</param>
        /// <param name="WeekDay">WeekDay is the day of the week required, with its value depending upon the Type (see below).</param>
        /// <param name="Year">Year can be any integer greater than 4800 BC (-4799). </param>
        /// <param name="Month">Month contains the month of the year. This value must be within the range -9 to 14 (inclusive), though value outside the range 1-12 will be divided by by 12, such that the Year incremented by the quotient and the Month set to the remainder.</param>
        /// <returns></returns>
        /// //, int type) - <param name="Type">Type defines the input form for the WeekDay: Type=1 : WeekDay contains numbers 1 (Sunday) through 7 (Saturday) - default behaviour of Microsoft cal_Excel. Type=2 : WeekDay contains numbers 1 (Monday) through 7 (Sunday). Type=3 : WeekDay contains numbers 0 (Monday) through 6 (Sunday).</param>
        /// Now the last parameter is used with the help of current curlture.
        public static DateTime GetNthWeekDay(int nth, DayOfWeek weekDay, int year, int month)
        {
            DateTime firstDate = new DateTime(year, month, 1);
            int noOfDaysInMonth = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDaysInMonth(year, month);
            DateTime lastDayOfMonth = firstDate.AddDays(noOfDaysInMonth - 1);  //firstDayOfMonth.AddMonths(1).AddDays(-1);
            //CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(d, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
            DayOfWeek firstWeekDayOfMonth = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDayOfWeek(firstDate);
            DateTime currentWeekSameDay = DateTime.MinValue;
            if (firstWeekDayOfMonth > weekDay)
            {
                //currentWeekSameDay = DateTime.MinValue;
                ///Check for the corner cases when that date is not available
                int noOfDays = firstWeekDayOfMonth - weekDay;

                currentWeekSameDay = firstDate.Add(TimeSpan.FromDays(7 - noOfDays));
            }
            else if (firstWeekDayOfMonth < weekDay)
            {
                ///Check for the corner cases when that date is not available
                currentWeekSameDay = firstDate.AddDays(weekDay - firstWeekDayOfMonth);
            }
            else
            {
                currentWeekSameDay = firstDate;
            }

            DateTime finalDate = currentWeekSameDay.AddDays((nth - 1) * 7);

            if (firstDate.Date.Subtract(TimeSpan.FromDays(1)) < finalDate.Date && finalDate.Date < lastDayOfMonth.Date.AddDays(1))
            {
                return finalDate;
            }
            else
            {
                throw new InvalidOperationException("nth(" + nth + ") " + weekDay.ToString() + " in year : " + year + " and month : " + month + " does not exist. Wrong data requested.");
            }
        }
    }
}
