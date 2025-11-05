using System;
using System.Globalization;

namespace Prana.ATDLLibrary.Fix
{
    /// <summary>
    /// Static class that provides utility methods for dealing with FIX format dates and times.
    /// </summary>
    public static class FixDateTime
    {
        /// <summary>
        /// Attempts to convert the supplied string to a <see cref="DateTime"/> using either the specified
        /// format provider or any of the valid FIX date/time formats.
        /// </summary>
        /// <param name="value">String value to attempt to convert.</param>
        /// <param name="provider">Format provider to use.</param>
        /// <param name="result">If successful, the DateTime equivalent representation of the supplied string; undefined otherwise.</param>
        /// <returns>True if the supplied value could be converted; false otherwise.</returns>
        public static bool TryParse(string value, IFormatProvider provider, out DateTime result)
        {
            result = DateTime.MinValue;

            // Try loose parsing using the supplied locale, and if that fails, try all the supported FIX formats.
            return DateTime.TryParse(value, provider, DateTimeStyles.AllowWhiteSpaces, out result) ||
                DateTime.TryParseExact(value, FixDateTimeFormat.AllFormats, provider, DateTimeStyles.AllowWhiteSpaces, out result);
        }
    }
}
