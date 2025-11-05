using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PostTradeServices
{
    class HolidayHelper
    {
        private static Dictionary<int, List<int>> _dictAUECSpecificHolidays = new Dictionary<int, List<int>>();
        private static Dictionary<int, List<int>> _dictAUECWeekendHolidays = new Dictionary<int, List<int>>();

        public static void CreateDictionary()
        {
            try
            {
                _dictAUECSpecificHolidays.Clear();
                _dictAUECWeekendHolidays.Clear();
                FillAUECSpecificHolidaysDict();
                FillAUECWeekendsDict();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public static bool IsHoliday(int auecID, int timeKey)
        {
            bool result = false;

            if (auecID == 0) // Invalid AUEC
            {
                return result;
            }
            if (_dictAUECSpecificHolidays.ContainsKey(auecID))
            {
                result = result || _dictAUECSpecificHolidays[auecID].Contains(timeKey);
            }
            if (_dictAUECWeekendHolidays.ContainsKey(auecID))
            {
                result = result || _dictAUECWeekendHolidays[auecID].Contains(timeKey);
            }

            return result;
        }

        private static void FillAUECWeekendsDict()
        {
            try
            {
                DataTable dt = DataBaseManager.GetAUECWeekends();
                DataSet ds = DataBaseManager.GetAUECWeekendHolidays();
                DataTable dtSaturday = ds.Tables[0];
                DataTable dtSunday = ds.Tables[1];

                List<int> listSaturday = new List<int>();
                List<int> listSunday = new List<int>();
                List<int> listSatSunDay = new List<int>();

                foreach (DataRow row in dtSaturday.Rows)
                {
                    int timeKey = Int32.Parse(row["TimeKey"].ToString());
                    listSaturday.Add(timeKey);
                    listSatSunDay.Add(timeKey);
                }

                foreach (DataRow row in dtSunday.Rows)
                {
                    int timeKey = Int32.Parse(row["TimeKey"].ToString());
                    listSunday.Add(timeKey);
                    listSatSunDay.Add(timeKey);
                }

                //****// may not be neeeded //****// 
                Dictionary<int, List<int>> dictAUECWeekends = new Dictionary<int, List<int>>();
                foreach (DataRow row in dt.Rows)
                {
                    int auecID = Int32.Parse(row["AUECID"].ToString());
                    int weekendID = Int32.Parse(row["WeeklyHoliDayID"].ToString());

                    if (!dictAUECWeekends.ContainsKey(auecID))
                    {
                        List<int> listTimeKeys = new List<int>();
                        listTimeKeys.Add(weekendID);
                        dictAUECWeekends.Add(auecID, listTimeKeys);
                    }
                    else
                    {
                        dictAUECWeekends[auecID].Add(weekendID);
                    }
                }
                //****// may not be neeeded //****// 

                // Very specific logic. Assumes only Sunday and Saturday are the weekends, Which is actually true ;)
                // Key to Sunday and Saturday are HardCoded
                foreach (KeyValuePair<int, List<int>> item in dictAUECWeekends)
                {
                    if (item.Value.Count == 1)
                    {
                        if (item.Value[0] == 6) // Saturday
                        {
                            _dictAUECWeekendHolidays.Add(item.Key, listSaturday);
                        }
                        else                   // Sunday
                        {
                            _dictAUECWeekendHolidays.Add(item.Key, listSunday);
                        }
                    }

                    if (item.Value.Count == 2) // Both
                    {
                        _dictAUECWeekendHolidays.Add(item.Key, listSatSunDay);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private static void FillAUECSpecificHolidaysDict()
        {
            try
            {
                DataTable dtAUECSpecificHolidays = DataBaseManager.GetAUECSpecificHolidays();

                foreach (DataRow row in dtAUECSpecificHolidays.Rows)
                {
                    int auecID = Int32.Parse(row["AUECID"].ToString());
                    int timeKey = Int32.Parse(row["HolidayDateKey"].ToString());

                    if (!_dictAUECSpecificHolidays.ContainsKey(auecID))
                    {
                        List<int> listTimeKeys = new List<int>();
                        listTimeKeys.Add(timeKey);
                        _dictAUECSpecificHolidays.Add(auecID, listTimeKeys);
                    }
                    else
                    {
                        _dictAUECSpecificHolidays[auecID].Add(timeKey);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
