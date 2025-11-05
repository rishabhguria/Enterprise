using Prana.LogManager;
using System;
using System.Collections;
using System.Text;

namespace Prana.Utilities.UI.CronUtility
{
    /// <summary>
    /// class consists of methods to fill UI and get schedule description
    /// </summary>
    public static class CronUtility
    {
        /// <summary>
        /// Function returns object to fill UI according to passed cronExpression
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public static CronDescription GetCronDescriptionObject(String cronExpression)
        {
            try
            {
                CronDescription objUIDesc = new CronDescription();
                string[] cronSubstring = cronExpression.Split(' ');
                int checkInt = 0;

                objUIDesc.StartTime = DateTime.Parse(cronSubstring[2] + ":" + cronSubstring[1] + ":" + cronSubstring[0]);

                // For Daily Schedule
                if (cronSubstring[3].Contains("/"))
                {
                    string[] dayOfMonth = cronSubstring[3].Split('/');
                    objUIDesc.Type = ScheduleType.Daily;
                    objUIDesc.RecurDay = Convert.ToDecimal(dayOfMonth[1]);
                }

                // For One time Schedule
                else if (cronSubstring[3] != "?" && cronSubstring[3] != "*" && cronSubstring[5] != "*" && !cronSubstring[3].Contains(",") && Int32.TryParse(cronSubstring[4], out checkInt))
                {
                    objUIDesc.Type = ScheduleType.OneTime;
                    objUIDesc.StartDate = DateTime.Parse(cronSubstring[4] + "/" + cronSubstring[3] + "/" + DateTime.Now.Year);
                }
                // For Weekly Schedule
                else if (cronSubstring[4].Contains("*") && cronSubstring[5] != "*" && cronSubstring[5] != "?" && !cronSubstring[5].Contains("#"))
                {
                    objUIDesc.WeeklyDaysList.AddRange(cronSubstring[5].Split(','));
                    objUIDesc.Type = ScheduleType.Weekly;
                }
                // For Monthly Schedule
                else
                {
                    objUIDesc.MonthlyMonthsList.AddRange(cronSubstring[4].Split(','));
                    objUIDesc.Type = ScheduleType.Monthly;

                    // For Day of Month option
                    if (!cronSubstring[5].Contains("#"))
                    {
                        objUIDesc.MonthlyDaysList.AddRange(cronSubstring[3].Split(','));
                        objUIDesc.MonthlyModeDayOfMonth = true;
                    }
                    // For WeekDay option
                    else
                    {
                        objUIDesc.MonthlyModeWeekday = true;

                        string[] dayOfWeek = cronSubstring[5].Split(',');
                        ArrayList strWeekNo = new ArrayList();
                        ArrayList strDay = new ArrayList();
                        foreach (string item in dayOfWeek)
                        {
                            string[] DayweekNo = item.Split('#');
                            if (DayweekNo[1] != "*")
                            {
                                strWeekNo.Add(DayweekNo[1]);
                            }
                            if (DayweekNo[0] != "*")
                            {
                                strDay.Add(DayweekNo[0]);
                            }
                        }
                        objUIDesc.MonthlyWeekNumbersList.AddRange((string[])strWeekNo.ToArray(typeof(string)));// (string[])strWeekNo.ToArray(typeof(string));
                        objUIDesc.MonthlyWeekDaysList.AddRange((string[])strDay.ToArray(typeof(string)));
                    }
                }
                return objUIDesc;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Function returns string describing scheduled task according to passed cronExpression
        /// </summary>
        /// <param name="cronExpr"></param>
        /// <returns></returns>
        public static String GetCronDescription(string cronExpr)
        {
            try
            {
                string[] cronSubstring = cronExpr.Split(' ');
                StringBuilder cronDesc = new StringBuilder();
                DateTime time = DateTime.Parse(cronSubstring[2] + ":" + cronSubstring[1] + ":" + cronSubstring[0]);

                CronDescription objUIDesc = GetCronDescriptionObject(cronExpr);
                // For One time schedule
                if (objUIDesc.Type == ScheduleType.OneTime)
                {
                    cronDesc.Append("Run on " + cronSubstring[3] + " of month " + cronSubstring[4] + " one time");
                }
                // For Daily schedule
                if (objUIDesc.Type == ScheduleType.Daily)
                {
                    string[] daysOfMonth = cronSubstring[3].Split('/');
                    cronDesc.Append("Run after every " + daysOfMonth[1] + " days");
                }
                // For Weekly schedule
                if (objUIDesc.Type == ScheduleType.Weekly)
                {
                    if (!cronSubstring[5].Contains("?"))
                        cronDesc.Append("Run on every " + cronSubstring[5]);
                }
                // For Monthly schedule
                if (objUIDesc.Type == ScheduleType.Monthly)
                {
                    if (cronSubstring[4].Contains("*"))
                    {
                        cronDesc.Append("Run on every month");
                    }
                    else
                    {
                        cronDesc.Append("Run on every " + cronSubstring[4]);
                    }
                    if (!cronSubstring[3].Contains("?"))
                    {
                        cronDesc.Append(" on date " + cronSubstring[3]);
                    }
                    else if (cronSubstring[5].Contains("#"))
                    {
                        string[] dayOfWeek = cronSubstring[5].Split(',');
                        ArrayList strWeekNo = new ArrayList();
                        ArrayList strDay = new ArrayList();
                        foreach (string item in dayOfWeek)
                        {
                            string[] DayweekNo = item.Split('#');
                            if (DayweekNo[1] != "*")
                            {
                                if (!strWeekNo.Contains(DayweekNo[1]))
                                    strWeekNo.Add(DayweekNo[1]);
                            }
                            if (DayweekNo[0] != "*")
                            {
                                if (!strDay.Contains(DayweekNo[0]))
                                    strDay.Add(DayweekNo[0]);
                            }
                        }
                        if (strWeekNo.Count >= 1)
                        {
                            cronDesc.Append(" on weekno's ");
                            for (int i = 0; i < strWeekNo.Count; i++)
                            {
                                cronDesc.Append(strWeekNo[i] + ",");
                            }
                        }
                        if (strDay.Count >= 1)
                        {
                            cronDesc.Append(" on weekday's ");
                            for (int i = 0; i < strDay.Count; i++)
                            {
                                cronDesc.Append(strDay[i] + ",");
                            }
                        }
                    }

                }
                cronDesc.Append(" at " + time.ToString("hh:mm:ss tt"));

                return cronDesc.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }
    }
}
