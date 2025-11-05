using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace System
{
 
    /// <summary>
    /// DateTime Extensions
    /// </summary>
    /// <remarks></remarks>
    public static class DateExtensions
    {
        /// <summary>
        /// Gets the Last date in month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime LastDateInMonth(this DateTime value)
        {
            int daySpan = DateTime.DaysInMonth(value.Year, value.Month);
            return new DateTime(value.Year, value.Month, daySpan);
        }
        /// <summary>
        /// Gets the first date in month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDateInMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        /// <summary>
        /// Firsts the date in week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime FirstDateInWeek(this DateTime value)
        {
            int days = 1 - (int)value.DayOfWeek;
            return value.AddDays(days);
        }

        /// <summary>
        /// Shorts the date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ShortDate(this DateTime value)
        {
            return value.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// Keys the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Key(this DateTime value)
        {
            return value.ToString("yyyyMMdd");
        }
   
    }
}
