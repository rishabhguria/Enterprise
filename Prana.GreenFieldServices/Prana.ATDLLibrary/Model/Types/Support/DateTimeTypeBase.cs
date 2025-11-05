using System;

namespace Prana.ATDLLibrary.Model.Types.Support
{
    /// <summary>
    /// Base class for all date and time related FIXatdl types (except MonthYear_t).
    /// </summary>
    public abstract class DateTimeTypeBase : AtdlValueType<DateTime>
    {
        /// <summary>
        /// Maximum value for this date/time type, i.e., the latest acceptable date/time.
        /// </summary>
        public DateTime? MaxValue { get; set; }

        /// <summary>
        /// Minimum value for this date/time type, i.e., the earliest acceptable date/time.
        /// </summary>
        public DateTime? MinValue { get; set; }
    }
}
