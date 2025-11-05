using System;

namespace Prana.BusinessObjects
{
    public class DateTimeConstants
    {
        //Also defined in D:\NirvanaOMS\SourceCode\Dev\Prana\Application\Prana.Utilities\DateTimeUtilities\DateTimeConstants.cs because of reference issue.
        /// <summary>
        /// If we take DateTimeConstants.MinValue then system will throw exception - 
        /// SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM. Hence put this date.
        /// This will represent null date in our system and there will not be any exception while casting values like DateTimeConstants.MinValue
        /// </summary>
        public static readonly DateTime MinValue = new DateTime(1800, 1, 1, 12, 0, 0); // "1/1/1800 12:00:00 AM";
        public static readonly int MinYear = 1980; // "1/1/1800 12:00:00 AM";
        public static string GetCurrentTimeInFixFormat()
        {
            return DateTime.UtcNow.ToString(NirvanaDateTimeFormat);
        }

        public static string GetNoonTimeInFixFormat()
        {
            return DateTime.UtcNow.Date.AddHours(12).ToString(NirvanaDateTimeFormat);
        }
        public const string DateFormat = "d";
        public const string DateformatForClosing = "MM/dd/yyyy";
        public const string NirvanaDateTimeFormat = "yyyyMMdd-HH:mm:ss";
        public const string NirvanaDateTimeFormat_WithoutTime = "yyyyMMdd";

        public const string DefaultDateTimeFormat = "MMddyyyy HH:mm:ss";
        public const string DateTimeMinVal = "01/01/1800 12:00:00 AM";
    }
}
