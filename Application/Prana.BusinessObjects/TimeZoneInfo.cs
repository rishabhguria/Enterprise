using Microsoft.Win32;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// The Time Zone Info Business Object
    /// </summary>
    public class TimeZoneInfo
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="TimeZoneInfo"/> class from being created.
        /// </summary>
        private TimeZoneInfo()
        {
        }

        /// <summary>
        /// The exchangetimezonestring
        /// </summary>
        private const string EXCHANGETIMEZONESTRING = "(UTC-05:00) Eastern Time (US & Canada)";
        /// <summary>
        /// The eastern time zone
        /// </summary>
        private static TimeZone _easternTimeZone;
        /// <summary>
        /// All time zones
        /// </summary>
        private static List<TimeZone> _allTimeZones;


        /// <summary>
        /// Gets the eastern time zone.
        /// </summary>
        /// <value>
        /// The eastern time zone.
        /// </value>
        public static TimeZone EasternTimeZone
        {
            get
            {
                return _easternTimeZone ?? (_easternTimeZone = FindTimeZone(EXCHANGETIMEZONESTRING));
            }
        }

        /// <summary>
        /// Use this method if the string format is not known in advance.It checks for all the possible name formats
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        public static TimeZone FindTimeZoneByString(string timeZoneName)
        {
            TimeZone requiredGlobalTimeZone = null;
            try
            {
                requiredGlobalTimeZone = FindTimeZoneByDayLightName(timeZoneName);
                if (requiredGlobalTimeZone == null)
                {
                    requiredGlobalTimeZone = FindTimeZone(timeZoneName);
                }
                if (requiredGlobalTimeZone == null)
                {
                    requiredGlobalTimeZone = FindTimeZoneByStandardName(timeZoneName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return requiredGlobalTimeZone;
        }

        /// <summary>
        /// Find Time Zone by Display Name
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        public static TimeZone FindTimeZone(String timeZoneName)
        {
            try
            {
                if (_allTimeZones == null)
                {
                    GetTimeZonesFromRegistry();
                }

                foreach (TimeZone zone in _allTimeZones)
                {
                    if (String.Equals(zone.DisplayName, timeZoneName))
                    {
                        return zone;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts the local time to UTC.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        public static DateTime ConvertLocalTimeToUTC(DateTime time, TimeZone zone)
        {
            DateTime utcConverted = new DateTime();
            try
            {
                TimeSpan baseOffset = new TimeSpan(0);
                if (GetIsDalightSavingsFromLocalTime(time, zone))
                {
                    baseOffset = -(zone.Bias + zone.DaylightBias);
                }
                else
                {
                    baseOffset = -zone.Bias;
                }

                utcConverted = new DateTime(time.Ticks - baseOffset.Ticks);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return utcConverted;
        }

        /// <summary>
        /// Converts the UTC time to local time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        public static DateTime ConvertUtcTimeToLocalTime(DateTime time, TimeZone zone)
        {
            DateTime localConvertedTime = new DateTime();
            try
            {
                TimeSpan baseOffset = GetBaseOffset(time, zone);

                localConvertedTime = new DateTime(time.Ticks - baseOffset.Ticks);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return localConvertedTime;
        }

        /// <summary>
        /// Get time span between the UTC time and local time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        public static TimeSpan GetBaseOffset(DateTime time, TimeZone zone)
        {
            TimeSpan baseOffset = new TimeSpan(0);
            try
            {
                if (GetIsDalightSavingsFromUtcTime(time, zone))
                {
                    baseOffset = (zone.Bias + zone.DaylightBias);
                }
                else
                {
                    baseOffset = zone.Bias;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return baseOffset;
        }

        /// <summary>
        /// Finds the name of the time zone by standard.
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        public static TimeZone FindTimeZoneByStandardName(String timeZoneName)
        {
            try
            {
                if (_allTimeZones == null)
                {
                    GetTimeZonesFromRegistry();
                }

                foreach (TimeZone zone in _allTimeZones)
                {
                    if (String.Equals(zone.StandardName, timeZoneName))
                    {
                        return zone;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the name of the time zone by day light.
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        /// <returns></returns>
        public static TimeZone FindTimeZoneByDayLightName(String timeZoneName)
        {
            try
            {
                if (_allTimeZones == null)
                {
                    GetTimeZonesFromRegistry();
                }

                foreach (TimeZone zone in _allTimeZones)
                {
                    if (String.Equals(zone.DaylightName, timeZoneName))
                    {
                        return zone;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the time zones from registry.
        /// </summary>
        private static void GetTimeZonesFromRegistry()
        {
            try
            {
                _allTimeZones = new List<TimeZone>();

                const string timeZoneKeyPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones";
                using (RegistryKey timeZonesKey = Registry.LocalMachine.OpenSubKey(timeZoneKeyPath))
                {
                    String[] zoneKeys = timeZonesKey.GetSubKeyNames();
                    Int32 zoneKeyCount = zoneKeys.Length;
                    for (Int32 i = 0; i < zoneKeyCount; i++)
                    {
                        using (RegistryKey timeZoneKey = timeZonesKey.OpenSubKey(zoneKeys[i]))
                        {
                            TimeZone newTimeZone = new TimeZone();
                            newTimeZone.TimeZoneKey = (String)zoneKeys[i];
                            newTimeZone.DisplayName = (String)timeZoneKey.GetValue("Display");
                            newTimeZone.DaylightName = (String)timeZoneKey.GetValue("Dlt");
                            newTimeZone.StandardName = (String)timeZoneKey.GetValue("Std");
                            Byte[] bytes = (Byte[])timeZoneKey.GetValue("TZI");
                            newTimeZone.Bias = new TimeSpan(0, BitConverter.ToInt32(bytes, 0), 0);
                            newTimeZone.DaylightBias = new TimeSpan(0, BitConverter.ToInt32(bytes, 8), 0);
                            newTimeZone.StandardTransitionMonth = BitConverter.ToInt16(bytes, 14);
                            newTimeZone.StandardTransitionDayOfWeek = BitConverter.ToInt16(bytes, 16);
                            newTimeZone.StandardTransitionWeek = BitConverter.ToInt16(bytes, 18);
                            newTimeZone.StandardTransitionTimeOfDay = new DateTime(1, 1, 1,
                                                                        BitConverter.ToInt16(bytes, 20),
                                                                        BitConverter.ToInt16(bytes, 22),
                                                                        BitConverter.ToInt16(bytes, 24),
                                                                        BitConverter.ToInt16(bytes, 26));
                            newTimeZone.DaylightTransitionMonth = BitConverter.ToInt16(bytes, 30);
                            newTimeZone.DaylightTransitionDayOfWeek = BitConverter.ToInt16(bytes, 32);
                            newTimeZone.DaylightTransitionWeek = BitConverter.ToInt16(bytes, 34);
                            newTimeZone.DaylightTransitionTimeOfDay = new DateTime(1, 1, 1,
                                                                        BitConverter.ToInt16(bytes, 36),
                                                                        BitConverter.ToInt16(bytes, 38),
                                                                        BitConverter.ToInt16(bytes, 40),
                                                                        BitConverter.ToInt16(bytes, 42));
                            newTimeZone.SupportsDaylightSavings = (newTimeZone.StandardTransitionMonth != 0);
                            _allTimeZones.Add(newTimeZone);
                        }
                    }
                }

                Comparison<TimeZone> timeZoneComparisonHandler = CompareTimeZones;
                _allTimeZones.Sort(timeZoneComparisonHandler);
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

        /// <summary>
        /// Compares the time zones.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        private static int CompareTimeZones(TimeZone first, TimeZone second)
        {
            try
            {
                return first.Bias.CompareTo(second.Bias);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Gets the relative date.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="targetDayOfWeek">The target day of week.</param>
        /// <param name="numberOfSundays">The number of sundays.</param>
        /// <returns></returns>
        private static DateTime GetRelativeDate(int year, int month, int targetDayOfWeek, int numberOfSundays)
        {
            DateTime time = new DateTime();
            try
            {
                if (numberOfSundays <= 4)
                {
                    time = new DateTime(year, month, 1);

                    int dayOfWeek = (int)time.DayOfWeek;
                    int delta = targetDayOfWeek - dayOfWeek;
                    if (delta < 0)
                    {
                        delta += 7;
                    }
                    delta += 7 * (numberOfSundays - 1);

                    if (delta > 0)
                    {
                        time = time.AddDays(delta);
                    }
                }
                else
                {
                    // If numberOfSunday is greater than 4, we will get the last sunday.
                    Int32 daysInMonth = DateTime.DaysInMonth(year, month);
                    time = new DateTime(year, month, daysInMonth);
                    // This is the day of week for the last day of the month.
                    int dayOfWeek = (int)time.DayOfWeek;
                    int delta = dayOfWeek - targetDayOfWeek;
                    if (delta < 0)
                    {
                        delta += 7;
                    }

                    if (delta > 0)
                    {
                        time = time.AddDays(-delta);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return time;
        }

        /// <summary>
        /// Gets the daylight time.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        private static DaylightTime GetDaylightTime(Int32 year, TimeZone zone)
        {
            DaylightTime dlTime = null;
            try
            {
                DateTime startTime = new DateTime();
                DateTime endTime = new DateTime();
                TimeSpan delta = zone.DaylightBias;

                startTime = GetRelativeDate(year, zone.DaylightTransitionMonth, zone.DaylightTransitionDayOfWeek, zone.DaylightTransitionWeek);
                startTime = startTime.AddTicks(zone.DaylightTransitionTimeOfDay.Ticks);
                endTime = GetRelativeDate(year, zone.StandardTransitionMonth, zone.StandardTransitionDayOfWeek, zone.StandardTransitionWeek);
                endTime = endTime.AddTicks(zone.StandardTransitionTimeOfDay.Ticks);
                dlTime = new DaylightTime(startTime, endTime, delta);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dlTime;
        }

        /// <summary>
        /// Gets the is dalight savings from local time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        private static Boolean GetIsDalightSavingsFromLocalTime(DateTime time, TimeZone zone)
        {
            Boolean isDst = false;
            try
            {
                if (!zone.SupportsDaylightSavings)
                {
                    return false;
                }
                DaylightTime daylightTime = GetDaylightTime(time.Year, zone);

                // startTime and endTime represent the period from either the start of DST to the end and includes the 
                // potentially overlapped times
                DateTime startTime = daylightTime.Start - zone.DaylightBias;
                DateTime endTime = daylightTime.End;

                if (startTime > endTime)
                {
                    // In southern hemisphere, the daylight saving time starts later in the year, and ends in the beginning of next year.
                    // Note, the summer in the southern hemisphere begins late in the year.
                    if (time >= startTime || time < endTime)
                    {
                        isDst = true;
                    }
                }
                else if (time >= startTime && time < endTime)
                {
                    // In northern hemisphere, the daylight saving time starts in the middle of the year.
                    isDst = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isDst;
        }

        /// <summary>
        /// Gets the is dalight savings from UTC time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        private static Boolean GetIsDalightSavingsFromUtcTime(DateTime time, TimeZone zone)
        {
            Boolean isDst = false;
            try
            {
                if (!zone.SupportsDaylightSavings)
                {
                    return false;
                }

                // Get the daylight changes for the year of the specified time.
                TimeSpan offset = -zone.Bias;
                DaylightTime daylightTime = GetDaylightTime(time.Year, zone);

                // The start and end times represent the range of universal times that are in DST for that year.                
                // Within that there is an ambiguous hour, usually right at the end, but at the beginning in
                // the unusual case of a negative daylight savings delta.
                DateTime startTime = daylightTime.Start - offset;
                DateTime endTime = daylightTime.End - offset + zone.DaylightBias;

                if (startTime > endTime)
                {
                    // In southern hemisphere, the daylight saving time starts later in the year, and ends in the beginning of next year.
                    // Note, the summer in the southern hemisphere begins late in the year.
                    isDst = (time < endTime || time >= startTime);
                }
                else
                {
                    // In northern hemisphere, the daylight saving time starts in the middle of the year.
                    isDst = (time >= startTime && time < endTime);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isDst;
        }
    }
}
