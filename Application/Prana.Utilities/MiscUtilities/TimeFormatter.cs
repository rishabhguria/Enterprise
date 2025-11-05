using System;
using System.Text;

namespace Prana.Utilities.MiscUtilities
{
    public static class TimeFormatter
    {
        public static string Format(int seconds)
        {
            if (seconds > 0)
            {
                TimeSpan elapsed = TimeSpan.FromSeconds(seconds);

                StringBuilder formattedTime = new StringBuilder();
                if (elapsed.Days != 0)
                {
                    formattedTime.Append(elapsed.Days);
                    formattedTime.Append(" Days ");
                    formattedTime.Append(elapsed.Hours);
                    formattedTime.Append(" Hours ");
                    formattedTime.Append(elapsed.Minutes);
                    formattedTime.Append(" Minutes ");
                    formattedTime.Append(elapsed.Seconds);
                    formattedTime.Append(" Seconds");
                }
                else if (elapsed.Hours != 0)
                {
                    formattedTime.Append(elapsed.Hours);
                    formattedTime.Append(" Hours ");
                    formattedTime.Append(elapsed.Minutes);
                    formattedTime.Append(" Minutes ");
                    formattedTime.Append(elapsed.Seconds);
                    formattedTime.Append(" Seconds");
                }
                else if (elapsed.Minutes != 0)
                {
                    formattedTime.Append(elapsed.Minutes);
                    formattedTime.Append(" Minutes ");
                    formattedTime.Append(elapsed.Seconds);
                    formattedTime.Append(" Seconds");
                }
                else
                {
                    formattedTime.Append(elapsed.Seconds);
                    formattedTime.Append(" Seconds");
                }

                return formattedTime.ToString();
            }
            else
                return string.Empty;
        }
    }
}
