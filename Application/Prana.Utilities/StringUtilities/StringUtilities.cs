using Prana.LogManager;
using System;
using System.Text.RegularExpressions;

namespace Prana.Utilities.StringUtilities
{
    public static class StringUtilities
    {
        /// <summary>
        /// Used to set the captions after splitting the propertyName of the columns on allocation UI, PRANA-13184
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SplitCamelCase(string name)
        {
            try
            {
                string fieldName = Regex.Replace(name, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                return Regex.Replace(fieldName, @"\s+", " ");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return String.Empty;
            }
        }
    }
}
