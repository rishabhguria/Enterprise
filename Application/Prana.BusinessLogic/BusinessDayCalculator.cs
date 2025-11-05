using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using Prana

namespace Prana.BusinessLogic
{
    /// <summary>
    /// Calculates business days by assuming that saturday and sunday are weekends.
    /// Code break scenarios are given below...
    //  TODO : Remove the following breaks. 
    //- What if we need to use it where 6 days week
    //- what if we need to use it where sat/sun are not weekends but some other like friday/saturday (in middleeast)
    //- add user defined holidays in it.
    /// </summary>
    public class BusinessDayCalculator
    {
        #region Singleton instance
        private BusinessDayCalculator()
        {
            GetExchangeHolidayDatesPerAUEC();
            GetExchangeHolidayDatesInfoPerAUEC();
            _weeklyAUECHolidayIDs = GetAUECWiseWeeklyHolidays();
        }

        private static readonly object _lockerObj = new object();

        private static BusinessDayCalculator _businessDayCalculator = null;

        public static BusinessDayCalculator GetInstance()
        {
            //needed to make thread safe as extensively user and can be called from some other thread
            //although currently only UI thread makes call from different components
            if (_businessDayCalculator == null)
            {
                lock (_lockerObj)
                {
                    if (_businessDayCalculator == null)
                    {
                        _businessDayCalculator = new BusinessDayCalculator();
                    }
                }
            }
            return _businessDayCalculator;
        }
        #endregion

        #region Private members

        static Dictionary<long, List<DateTime>> _holidayDatesPerAUEC = new Dictionary<long, List<DateTime>>();
        private static readonly object _holidayDatesPerAUECLock = new object();
        static Dictionary<long, Dictionary<DateTime, bool>> _holidayDatesINFOPerAUEC = new Dictionary<long, Dictionary<DateTime, bool>>();
        static object lockerObject = new object();
        #endregion

        #region DAL
        //TODO : This dal should have been in the business logic
        public static bool CheckForHoliday(DateTime DateToCheck)
        {
            bool isHoliday = false;
            try
            {
                if (DateToCheck.DayOfWeek == DayOfWeek.Saturday || DateToCheck.DayOfWeek == DayOfWeek.Sunday)
                    isHoliday = true;
                else
                {
                    //DAL CODE
                    QueryData queryData = new QueryData();
                    queryData.Query = "select count(*) from T_AUECHolidays where datediff(dd,HolidayDate,'" + DateToCheck + "')=0";

                    int NoOfRows;
                    int.TryParse(DatabaseManager.DatabaseManager.ExecuteScalar(queryData).ToString(), out NoOfRows);
                    if (NoOfRows > 0)
                        isHoliday = true;
                    else
                        isHoliday = false;

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
            return isHoliday;
        }

        private void GetExchangeHolidayDatesPerAUEC()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetHolidaysByAUEC";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        FillAUECHolidays(row, 0);
                    }
                }
            }
            #region Catch
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
            #endregion
        }

        private void GetExchangeHolidayDatesInfoPerAUEC()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetHolidaysInfoByAUEC";

            int auecIdValue = Int16.MinValue;
            DateTime dateValue = DateTimeConstants.MinValue;
            bool IsSettlementOFF = true;
            Dictionary<DateTime, bool> HolidayDictionay = null;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        auecIdValue = int.Parse(row[0].ToString());
                        dateValue = DateTime.Parse(row[1].ToString());
                        IsSettlementOFF = bool.Parse(row[2].ToString());

                        lock (lockerObject)
                        {
                            if (_holidayDatesINFOPerAUEC.ContainsKey(auecIdValue))
                            {
                                HolidayDictionay = _holidayDatesINFOPerAUEC[auecIdValue];
                            }
                            else
                            {
                                HolidayDictionay = new Dictionary<DateTime, bool>();
                                _holidayDatesINFOPerAUEC.Add(auecIdValue, HolidayDictionay);
                            }

                            if (!HolidayDictionay.ContainsKey(dateValue))
                            {
                                HolidayDictionay.Add(dateValue, IsSettlementOFF);
                            }
                        }


                    }
                }
            }
            #region Catch
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
            #endregion
        }

        private static void FillAUECHolidays(object[] row, int offSet)
        {
            int auecID = 0 + offSet;
            int date = 1 + offSet;
            int auecIdValue = int.MinValue;
            DateTime dateValue = DateTimeConstants.MinValue;
            List<DateTime> holidayList = null;
            try
            {
                if (row[auecID] != null)
                {
                    auecIdValue = int.Parse(row[auecID].ToString());
                }
                if (row[date] != null)
                {
                    dateValue = DateTime.Parse(row[date].ToString());
                }

                lock (lockerObject)
                {
                    if (_holidayDatesPerAUEC.ContainsKey(auecIdValue))
                    {
                        holidayList = _holidayDatesPerAUEC[auecIdValue];
                    }
                    else
                    {
                        holidayList = new List<DateTime>();
                        _holidayDatesPerAUEC.Add(auecIdValue, holidayList);
                    }

                    if (!holidayList.Contains(dateValue))
                    {
                        holidayList.Add(dateValue.Date);
                    }
                }
            }
            #region Catch
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
            #endregion
        }

        #region AUECWeeklyHolidays
        Dictionary<long, List<int>> _weeklyAUECHolidayIDs = new Dictionary<long, List<int>>();
        private readonly object _weeklyAUECHolidayIDsLock = new object();
        /// <summary>
        /// This fills up the weekly holidays for all the auec's in the system.
        /// </summary>
        private Dictionary<long, List<int>> GetAUECWiseWeeklyHolidays()
        {
            Dictionary<long, List<int>> weeklyAUECWiseHolidayIDList = new Dictionary<long, List<int>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetWeeklyHolidayIdsForAllAUECs";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        weeklyAUECWiseHolidayIDList = FillAUECWeeklyHolidayIds(row, 0);
                    }
                }
            }
            #region Catch
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
            #endregion
            return weeklyAUECWiseHolidayIDList;
        }

        public Dictionary<long, List<DateTime>> GetAUECWiseYearlyHolidaysFromCache()
        {
            Dictionary<long, List<DateTime>> holidayTemp = null;// = new Dictionary<long, List<DateTime>>();
            try
            {
                lock (_holidayDatesPerAUECLock)
                {

                    holidayTemp = DeepCopyHelper.Clone<Dictionary<long, List<DateTime>>>(_holidayDatesPerAUEC);
                    //foreach (long auecid in _holidayDatesPerAUEC.Keys)
                    //{
                    //    holidayTemp.Add(auecid, new List<DateTime>());
                    //    holidayTemp[auecid].AddRange(_holidayDatesPerAUEC[auecid]);
                    //}
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

            return holidayTemp;

        }


        public Dictionary<long, List<int>> GetAUECWiseWeeklyHolidaysFromCache()
        {
            Dictionary<long, List<int>> holidayTemp = null;// = new Dictionary<long, List<DateTime>>();
            try
            {
                lock (_weeklyAUECHolidayIDsLock)
                {

                    holidayTemp = DeepCopyHelper.Clone<Dictionary<long, List<int>>>(_weeklyAUECHolidayIDs);
                    //foreach (long auecid in _holidayDatesPerAUEC.Keys)
                    //{
                    //    holidayTemp.Add(auecid, new List<DateTime>());
                    //    holidayTemp[auecid].AddRange(_holidayDatesPerAUEC[auecid]);
                    //}
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

            return holidayTemp;

        }

        private Dictionary<long, List<int>> FillAUECWeeklyHolidayIds(object[] row, int offSet)
        {
            int auecID = 0 + offSet;
            int weeklyHolidayID = 1 + offSet;
            long auecIdValue = long.MinValue;
            int weeklyHolidayIDValue = int.MinValue;
            List<int> weeklyHolidayIDList = null;
            try
            {
                if (row[auecID] != null)
                {
                    auecIdValue = int.Parse(row[auecID].ToString());
                }
                if (row[weeklyHolidayID] != null)
                {
                    weeklyHolidayIDValue = int.Parse(row[weeklyHolidayID].ToString());
                }

                if (_weeklyAUECHolidayIDs.ContainsKey(auecIdValue))
                {
                    weeklyHolidayIDList = _weeklyAUECHolidayIDs[auecIdValue];
                }
                else
                {
                    weeklyHolidayIDList = new List<int>();
                    _weeklyAUECHolidayIDs.Add(auecIdValue, weeklyHolidayIDList);
                }

                if (!weeklyHolidayIDList.Contains(weeklyHolidayIDValue))
                {
                    weeklyHolidayIDList.Add(weeklyHolidayIDValue);
                }
            }
            #region Catch
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
            #endregion
            return _weeklyAUECHolidayIDs;
        }
        #endregion

        #endregion

        #region BusinessDayCalculator functions

        public string GetLastBusinessDateOfMonth(int auecid, DateTime currentDate)
        {
            DateTime d1 = new DateTime(currentDate.Year, currentDate.Month, 1);
            return AdjustBusinessDaysForAUEC(d1, -1, auecid).Date.ToString();
        }

        public string GetLastBusinessDateOfQuarter(int auecid, DateTime currentDate)
        {
            DateTime firstDate = DateTime.Now;
            string currentYear = currentDate.Year.ToString();
            if (currentDate.Month >= 1 && currentDate.Month <= 3)
            {
                firstDate = DateTime.Parse("1/1/" + currentYear);
            }
            else if (currentDate.Month >= 4 && currentDate.Month <= 6)
            {
                firstDate = DateTime.Parse("4/1/" + currentYear);
            }
            else if (currentDate.Month >= 7 && currentDate.Month <= 9)
            {
                firstDate = DateTime.Parse("7/1/" + currentYear);
            }
            else if (currentDate.Month >= 10 && currentDate.Month <= 12)
            {
                firstDate = DateTime.Parse("10/1/" + currentYear);
            }

            return AdjustBusinessDaysForAUEC(firstDate, -1, auecid).Date.ToString();
        }

        public string GetLastBusinessDateOfYear(int auecid, DateTime currentDate)
        {
            DateTime firstDate = DateTime.Parse("1/1/" + currentDate.Year.ToString());
            return AdjustBusinessDaysForAUEC(firstDate, -1, auecid).Date.ToString();
        }


        public bool IsBusinessDayForAnyAuec(DateTime bizDate, long[] auecIDs)
        {
            bizDate = bizDate.Date;
            List<int> _currentAUECWeeklyHolidayIDs = null;
            foreach (long auec in auecIDs)
            {
                if (_weeklyAUECHolidayIDs.ContainsKey(auec))
                {
                    _currentAUECWeeklyHolidayIDs = _weeklyAUECHolidayIDs[auec];
                    if (!_currentAUECWeeklyHolidayIDs.Contains((int)bizDate.DayOfWeek))
                        return true;
                }
                if (_holidayDatesPerAUEC != null)
                {
                    if (_holidayDatesPerAUEC.ContainsKey(auec))
                    {
                        List<DateTime> holidayList = _holidayDatesPerAUEC[auec];
                        if (!holidayList.Contains(bizDate))
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public bool IsSettlementDayOffForAUEC(DateTime bizDate, long auecID)
        {
            bizDate = bizDate.Date;
            List<int> _currentAUECWeeklyHolidayIDs = null;
            int countWeeklyOffs = 0;
            if (_weeklyAUECHolidayIDs.ContainsKey(auecID))
            {
                _currentAUECWeeklyHolidayIDs = _weeklyAUECHolidayIDs[auecID];
                countWeeklyOffs = _currentAUECWeeklyHolidayIDs.Count;
                for (int index = 0; index < countWeeklyOffs; index++)
                {
                    if (bizDate.DayOfWeek == (DayOfWeek)_currentAUECWeeklyHolidayIDs[index])
                    {
                        return false;
                    }
                }
            }

            //if (_holidayDatesPerAUEC == null)
            //    GetExchangeHolidayDatesPerAUEC();
            if (_holidayDatesPerAUEC != null)
            {
                if (_holidayDatesPerAUEC.ContainsKey(auecID))
                {
                    List<DateTime> holidayList = _holidayDatesPerAUEC[auecID];
                    if (holidayList.Contains(bizDate))
                    {
                        Dictionary<DateTime, bool> holidaydictionary = _holidayDatesINFOPerAUEC[auecID];//Checks for IsSettlementOff checkbox
                        if (holidaydictionary[bizDate] == true)
                            return false;
                    }

                }

            }
            return true;
        }
        /// <summary>
        /// Checks if the supplied date is a business day for in a particular AUEC
        /// </summary>
        /// <param name="bizDate"></param>
        /// <param name="auecID"></param>
        /// <returns></returns>
        public bool IsBusinessDayForAUEC(DateTime bizDate, long auecID)
        {
            bizDate = bizDate.Date;
            List<int> _currentAUECWeeklyHolidayIDs = null;
            int countWeeklyOffs = 0;
            if (_weeklyAUECHolidayIDs.ContainsKey(auecID))
            {
                _currentAUECWeeklyHolidayIDs = _weeklyAUECHolidayIDs[auecID];
                countWeeklyOffs = _currentAUECWeeklyHolidayIDs.Count;
                for (int index = 0; index < countWeeklyOffs; index++)
                {
                    if (bizDate.DayOfWeek == (DayOfWeek)_currentAUECWeeklyHolidayIDs[index])
                    {
                        return false;
                    }
                }
            }

            //if (_holidayDatesPerAUEC == null)
            //    GetExchangeHolidayDatesPerAUEC();
            if (_holidayDatesPerAUEC != null)
            {
                if (_holidayDatesPerAUEC.ContainsKey(auecID))
                {
                    List<DateTime> holidayList = _holidayDatesPerAUEC[auecID];
                    if (holidayList.Contains(bizDate))
                    {
                        return false;
                    }

                }

            }
            return true;
        }

        /// <summary>
        /// Returns the date after adjusting the passed no of business days.
        /// </summary>
        /// <param name="startDate">Date start</param>
        /// <param name="noOfDays">noOfDays to adjust, can be positive or negative</param>
        /// <param name="auecID">AUECId for which holidays to be accounted for</param>
        /// <returns>returns the exact date after adjustment</returns>
        public DateTime AdjustBusinessDaysForAUEC(DateTime startDate, int noOfDays, long auecID, bool IsSettlement = false)
        {
            int bizDayCounter = 0;
            DateTime nextDay = startDate;
            bool isAddRequired = false;
            try
            {
                if (noOfDays > 0)
                {
                    isAddRequired = true;
                }
                else
                {
                    isAddRequired = false;
                    noOfDays = -1 * noOfDays;
                }

                if (isAddRequired)
                {

                    do
                    {
                        nextDay = nextDay.AddDays(1);
                        if (IsSettlement == true ? IsSettlementDayOffForAUEC(nextDay, auecID) : IsBusinessDayForAUEC(nextDay, auecID))
                        {
                            ++bizDayCounter;
                        }
                    } while (bizDayCounter < noOfDays);

                }
                else
                {
                    do
                    {
                        nextDay = nextDay.AddDays(-1);
                        if (IsBusinessDayForAUEC(nextDay, auecID))
                        {
                            ++bizDayCounter;
                        }
                    } while (bizDayCounter < noOfDays);
                }
            }
            catch (Exception)
            {
            }
            return nextDay;
        }

        public DateTime GetPreviousBusinessDay(DateTime Date, int AuecID)
        {
            DateTime previousDate = Date.AddDays(-1);
            bool isHoliday = !IsBusinessDayForAUEC(previousDate, AuecID);
            while (isHoliday)
            {
                previousDate = previousDate.AddDays(-1);
                isHoliday = !IsBusinessDayForAUEC(previousDate, AuecID);
            }
            return previousDate;
        }

        public List<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>> GetAUECWiseHolidayList(HashSet<int> auecIDs, DateTime startDate, DateTime endDate)
        {
            List<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>> auecWiseWorkingDays = new List<Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>>();
            try
            {
                List<Tuple<HashSet<int>, HashSet<DateTime>>> auecWiseHolidayList = new List<Tuple<HashSet<int>, HashSet<DateTime>>>();
                foreach (int auecID in auecIDs)
                {
                    HashSet<DateTime> holidayList = new HashSet<DateTime>();
                    DateTime sDate = startDate;
                    bool isAddedIntoList = false;
                    while (sDate <= endDate)
                    {
                        if (!IsBusinessDayForAUEC(sDate, auecID))
                            holidayList.Add(sDate);
                        sDate = sDate.AddDays(1);
                    }
                    //If holidays same as other auec then merge
                    if (auecWiseHolidayList.Count > 0)
                    {
                        var auecsWithSameHolidays = auecWiseHolidayList.FirstOrDefault(x => x.Item2.SetEquals(holidayList));
                        if (auecsWithSameHolidays != null)
                        {
                            auecsWithSameHolidays.Item1.Add(auecID);
                            isAddedIntoList = true;
                        }
                    }
                    //else add into the auecwise holiday
                    if (!isAddedIntoList)
                    {
                        auecWiseHolidayList.Add(new Tuple<HashSet<int>, HashSet<DateTime>>(new HashSet<int>() { auecID }, holidayList));
                    }
                }

                #region updation of auecWiseWorkingDays for continuous working days

                if (auecWiseHolidayList.Count > 0)
                {
                    foreach (Tuple<HashSet<int>, HashSet<DateTime>> auecHolidayList in auecWiseHolidayList)
                    {
                        List<Tuple<DateTime, DateTime>> workingDaysInterval = new List<Tuple<DateTime, DateTime>>();
                        DateTime cursorDay = startDate;
                        while (cursorDay <= endDate)
                        {
                            DateTime sDate = cursorDay;
                            DateTime eDate = endDate;
                            if (auecHolidayList.Item2.Contains(cursorDay))
                            {
                                cursorDay = cursorDay.AddDays(1);
                                continue;
                            }
                            else
                            {
                                sDate = cursorDay;
                                while (cursorDay <= endDate && !auecHolidayList.Item2.Contains(cursorDay))
                                {
                                    cursorDay = cursorDay.AddDays(1);
                                }
                                eDate = cursorDay.AddDays(-1);
                                workingDaysInterval.Add(new Tuple<DateTime, DateTime>(sDate, eDate));
                            }
                        }
                        auecWiseWorkingDays.Add(new Tuple<HashSet<int>, List<Tuple<DateTime, DateTime>>, HashSet<DateTime>>(auecHolidayList.Item1, workingDaysInterval, auecHolidayList.Item2));
                    }

                }
                #endregion
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
            return auecWiseWorkingDays;
        }

        #endregion BusinessDayCalculator functions

    }
}
