using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.CommonDataCache
{
    public class TimeZoneHelper
    {
        private static TimeZoneHelper _timeZoneHelper = null;
        private Dictionary<int, DateTime> _auecWiseCurrentDates;
        private Dictionary<int, DateTime> _clearanceTime;
        private Dictionary<int, DateTime> _currentOffsetAdjustedAUECDates;
        private readonly object _currentOffsetAdjustedAUECDatesLock = new object();
        private List<int> _inUseAUECIDs;
        private readonly object _lockerObj = new object();
        private Dictionary<int, DateTime> _marketEndTimeInfo = new Dictionary<int, DateTime>();
        private Dictionary<int, MarketTimes> _marketTimingInformation = new Dictionary<int, MarketTimes>();
        private TimeSpan _refreshTimeSpan;
        private IKeyValueDataManager _keyValueDataManager;

        private TimeZoneHelper()
        {
            try
            {
                _keyValueDataManager = WindsorContainerManager.Container.Resolve<IKeyValueDataManager>();
                _marketTimingInformation = _keyValueDataManager.GetMarketStartEndTime();
                _clearanceTime = _keyValueDataManager.FetchClearanceTime();
                _marketEndTimeInfo = GetAUECWiseMarketEndTime();
                _inUseAUECIDs = _keyValueDataManager.GetInUseAUECIDs();
                _refreshTimeSpan = new TimeSpan(24, 0, 0);

                if (_clearanceTime.Count == 0)
                {
                    SetMidNightClearanceTime();
                }

                CentralClearanceLogic();
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

        public Dictionary<int, DateTime> AUECWiseCurrentDates
        {
            get { return _auecWiseCurrentDates; }
            set { _auecWiseCurrentDates = value; }
        }

        public Dictionary<int, DateTime> ClearanceTime
        {
            get { return _clearanceTime; }
            set { _clearanceTime = value; }
        }

        public Dictionary<int, DateTime> CurrentOffsetAdjustedAUECDates
        {
            get { return _currentOffsetAdjustedAUECDates; }
            set { _currentOffsetAdjustedAUECDates = value; }
        }

        public List<int> InUseAUECIDs
        {
            get { return _inUseAUECIDs; }
            set { _inUseAUECIDs = value; }
        }

        public Dictionary<int, DateTime> MarketEndTimeInfo
        {
            get { return _marketEndTimeInfo; }
            set { _marketEndTimeInfo = value; }
        }

        public Dictionary<int, MarketTimes> MarketTimingInformation
        {
            get { return _marketTimingInformation; }
            set { _marketTimingInformation = value; }
        }

        public DateTime MostLeadingAUECDateTime(bool isCallingFromExpnl)
        {
            DateTime mostLeadingdate = DateTime.UtcNow;
            try
            {
                if (!isCallingFromExpnl)
                {
                    _clearanceTime = _keyValueDataManager.FetchClearanceTime();
                }

                lock (_currentOffsetAdjustedAUECDatesLock)
                {
                    CentralClearanceLogic();

                    if (_currentOffsetAdjustedAUECDates != null && _currentOffsetAdjustedAUECDates.Count > 0)
                    {
                        mostLeadingdate = _currentOffsetAdjustedAUECDates[_currentOffsetAdjustedAUECDates.Keys.First()].Date;
                        foreach (KeyValuePair<int, DateTime> var in _currentOffsetAdjustedAUECDates)
                        {
                            if (var.Value.Date.CompareTo(mostLeadingdate) > 0 && _inUseAUECIDs.Contains(var.Key))
                            {
                                mostLeadingdate = var.Value.Date;
                            }
                        }
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
            return mostLeadingdate;
        }

        public static TimeZoneHelper GetInstance()
        {
            if (_timeZoneHelper == null)
            {
                _timeZoneHelper = new TimeZoneHelper();
            }
            return _timeZoneHelper;
        }

        public void CurrentAUECDateTimes(Dictionary<int, DateTime> currentAUECDateTimes)
        {
            try
            {
                if (_auecWiseCurrentDates == null)
                {
                    GetAllAUECLocalDatesFromUTC(DateTime.UtcNow);
                }

                foreach (int AUECID in _auecWiseCurrentDates.Keys)
                {
                    currentAUECDateTimes.Add(AUECID, Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(AUECID)));
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

        public string GetAllAUECDateInUseAUECStr(DateTime theDateTime)
        {
            StringBuilder uTCDateForInUseAUEC = new StringBuilder();
            try
            {
                List<int> inUseAUECList = _keyValueDataManager.GetInUseAUECIDs();
                if (inUseAUECList.Count > 0)
                {
                    uTCDateForInUseAUEC.Append("0");
                    uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    uTCDateForInUseAUEC.Append(theDateTime.Date);
                    uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                    foreach (int AuecID in inUseAUECList)
                    {
                        if (inUseAUECList.Contains(AuecID))
                        {
                            uTCDateForInUseAUEC.Append(AuecID);
                            uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            uTCDateForInUseAUEC.Append(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(theDateTime, CachedDataManager.GetInstance.GetAUECTimeZone(AuecID)));
                            uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                        }
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
            return uTCDateForInUseAUEC.ToString();
        }

        /// <summary>
        /// Create UTC date based upon passed AUEC List
        /// </summary>
        /// <param name="theDateTime"></param>
        /// <param name="AUECList"></param>
        /// <returns></returns>
        public string GetAUECDateBasedOnAUECList(DateTime theDateTime, List<int> AUECList)
        {
            StringBuilder uTCDateForAUEC = new StringBuilder();
            try
            {
                if (AUECList.Count > 0)
                {
                    uTCDateForAUEC.Append("0");
                    uTCDateForAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    uTCDateForAUEC.Append(theDateTime.Date);
                    uTCDateForAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                    foreach (int AuecID in AUECList)
                    {
                        uTCDateForAUEC.Append(AuecID);
                        uTCDateForAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                        uTCDateForAUEC.Append(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(theDateTime, CachedDataManager.GetInstance.GetAUECTimeZone(AuecID)));
                        uTCDateForAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return uTCDateForAUEC.ToString();
        }

        public Dictionary<int, DateTime> GetAllAUECLocalDatesFromUTC(DateTime dateTime)
        {
            Dictionary<int, DateTime> allAUECLocalDatesFromUTC = new Dictionary<int, DateTime>();
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> allAUECTimeZones = CachedDataManager.GetInstance.GetAllAUECTimeZones();

                foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> AUECTimeZone in allAUECTimeZones)
                {
                    int AuecID = AUECTimeZone.Key;
                    allAUECLocalDatesFromUTC.Add(AuecID, Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dateTime, CachedDataManager.GetInstance.GetAUECTimeZone(AuecID)).Date);

                }

                lock (_lockerObj)
                {
                    _auecWiseCurrentDates = allAUECLocalDatesFromUTC;
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
            return allAUECLocalDatesFromUTC;
        }

        public string GetAllAUECLocalDatesFromUTCStr(DateTime theDateTime)
        {
            StringBuilder AllAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> AllAUECTimeZones = CachedDataManager.GetInstance.GetAllAUECTimeZones();

                // Add UTC date to the string by default with 0 AUECID
                AllAUECLocalDatesFromUTC.Append("0");
                AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                AllAUECLocalDatesFromUTC.Append(theDateTime.Date);
                AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);

                foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> AUECTimeZone in AllAUECTimeZones)
                {
                    int AuecID = AUECTimeZone.Key;
                    AllAUECLocalDatesFromUTC.Append(AuecID);
                    AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(theDateTime, CachedDataManager.GetInstance.GetAUECTimeZone(AuecID)).Date);
                    AllAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return AllAUECLocalDatesFromUTC.ToString();
        }

        public string GetAllAUECLocalDateTimesFromUTCStr(DateTime theDateTime)
        {
            StringBuilder allAUECLocalDatesTimeFromUTC = new StringBuilder();
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> AllAUECTimeZones = CachedDataManager.GetInstance.GetAllAUECTimeZones();

                // Add UTC date to the string by default with 0 AUECID
                allAUECLocalDatesTimeFromUTC.Append("0");
                allAUECLocalDatesTimeFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesTimeFromUTC.Append(theDateTime);
                allAUECLocalDatesTimeFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);

                foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> AUECTimeZone in AllAUECTimeZones)
                {
                    int AuecID = AUECTimeZone.Key;
                    allAUECLocalDatesTimeFromUTC.Append(AuecID);
                    allAUECLocalDatesTimeFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    allAUECLocalDatesTimeFromUTC.Append(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(theDateTime, CachedDataManager.GetInstance.GetAUECTimeZone(AuecID)));
                    allAUECLocalDatesTimeFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return allAUECLocalDatesTimeFromUTC.ToString();
        }

        public Dictionary<int, DateTime> GetAUECDateDictfromAUECString(string auecString)
        {
            Dictionary<int, DateTime> auecWiseDateDict = new Dictionary<int, DateTime>();
            try
            {
                string[] auecDateArr = auecString.Split(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                if (auecDateArr.Length > 0)
                {
                    foreach (string str in auecDateArr)
                    {
                        string[] auecDate = str.Split(Prana.BusinessObjects.Seperators.SEPERATOR_5);

                        if (auecDate.Length == 2)
                        {
                            int auecID = Convert.ToInt32(auecDate[0]);
                            DateTime date = Convert.ToDateTime(auecDate[1]).Date;
                            if (!auecWiseDateDict.ContainsKey(auecID))
                            {
                                auecWiseDateDict.Add(auecID, date);
                            }
                        }
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
            return auecWiseDateDict;
        }

        public string GetAUECOffsetAdjustedCurrentDateTimeString()
        {
            StringBuilder allAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                allAUECLocalDatesFromUTC.Append(0);
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesFromUTC.Append(DateTime.Now.ToUniversalTime());
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);

                lock (_currentOffsetAdjustedAUECDatesLock)
                {
                    foreach (KeyValuePair<int, DateTime> AUECOffsetAdjustedTime in _currentOffsetAdjustedAUECDates)
                    {
                        int AuecID = AUECOffsetAdjustedTime.Key;
                        if (_inUseAUECIDs.Contains(AuecID))
                        {
                            allAUECLocalDatesFromUTC.Append(AuecID);
                            allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            allAUECLocalDatesFromUTC.Append(AUECOffsetAdjustedTime.Value);
                            allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                        }
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
            return allAUECLocalDatesFromUTC.ToString();
        }

        public string GetAUECOffsetAdjustedCurrentDateTimeString(List<int> auecsList)
        {
            Dictionary<int, DateTime> consoladatedDict = new Dictionary<int, DateTime>();
            StringBuilder allAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                lock (_currentOffsetAdjustedAUECDatesLock)
                {
                    foreach (int key in auecsList)
                    {
                        if (_currentOffsetAdjustedAUECDates.ContainsKey(key))
                        {
                            consoladatedDict.Add(key, _currentOffsetAdjustedAUECDates[key]);
                        }
                    }
                }
                allAUECLocalDatesFromUTC.Append(0);
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesFromUTC.Append(DateTime.Now.ToUniversalTime());
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                foreach (KeyValuePair<int, DateTime> AUECOffsetAdjustedTime in consoladatedDict)
                {
                    int AuecID = AUECOffsetAdjustedTime.Key;
                    if (_inUseAUECIDs.Contains(AuecID))
                    {
                        allAUECLocalDatesFromUTC.Append(AuecID);
                        allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                        allAUECLocalDatesFromUTC.Append(AUECOffsetAdjustedTime.Value);
                        allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return allAUECLocalDatesFromUTC.ToString();
        }

        public string GetAUECOffsetAdjustedYesterdayDateTimeString()
        {
            StringBuilder allAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                allAUECLocalDatesFromUTC.Append(0);
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesFromUTC.Append(DateTime.Now.ToUniversalTime());
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);

                lock (_currentOffsetAdjustedAUECDatesLock)
                {
                    foreach (KeyValuePair<int, DateTime> AUECOffsetAdjustedTime in _currentOffsetAdjustedAUECDates)
                    {
                        int AuecID = AUECOffsetAdjustedTime.Key;
                        if (_inUseAUECIDs.Contains(AuecID))
                        {
                            allAUECLocalDatesFromUTC.Append(AuecID);
                            allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            allAUECLocalDatesFromUTC.Append(AUECOffsetAdjustedTime.Value.AddDays(-1));
                            allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                        }
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
            return allAUECLocalDatesFromUTC.ToString();
        }

        public string GetAUECOffsetBusinessAdjustedYesterdayDateTimeString()
        {
            StringBuilder allAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                allAUECLocalDatesFromUTC.Append(0);
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesFromUTC.Append(DateTime.Now.ToUniversalTime());
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);

                lock (_currentOffsetAdjustedAUECDatesLock)
                {
                    foreach (KeyValuePair<int, DateTime> AUECOffsetAdjustedTime in _currentOffsetAdjustedAUECDates)
                    {
                        int AuecID = AUECOffsetAdjustedTime.Key;
                        if (_inUseAUECIDs.Contains(AuecID))
                        {
                            allAUECLocalDatesFromUTC.Append(AuecID);
                            allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            DateTime yesterdayBusinessAdjustedDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(AUECOffsetAdjustedTime.Value, -1, AuecID).Date;
                            allAUECLocalDatesFromUTC.Append(yesterdayBusinessAdjustedDate);
                            allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                        }
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
            return allAUECLocalDatesFromUTC.ToString();
        }

        public string GetAUECOffsetBusinessAdjustedYesterdayDateTimeString(List<int> auecsList)
        {
            Dictionary<int, DateTime> consolidatedDict = new Dictionary<int, DateTime>();
            StringBuilder allAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                lock (_currentOffsetAdjustedAUECDatesLock)
                {
                    foreach (int key in auecsList)
                    {
                        if (_currentOffsetAdjustedAUECDates.ContainsKey(key))
                        {
                            consolidatedDict.Add(key, _currentOffsetAdjustedAUECDates[key]);
                        }
                    }
                }

                allAUECLocalDatesFromUTC.Append(0);
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesFromUTC.Append(DateTime.Now.ToUniversalTime());
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                foreach (KeyValuePair<int, DateTime> AUECOffsetAdjustedTime in consolidatedDict)
                {
                    int AuecID = AUECOffsetAdjustedTime.Key;
                    if (_inUseAUECIDs.Contains(AuecID))
                    {
                        allAUECLocalDatesFromUTC.Append(AuecID);
                        allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                        DateTime yesterdayBusinessAdjustedDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(AUECOffsetAdjustedTime.Value, -1, AuecID).Date;
                        allAUECLocalDatesFromUTC.Append(yesterdayBusinessAdjustedDate);
                        allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return allAUECLocalDatesFromUTC.ToString();
        }

        public DateTime GetCurrentDateByAUEC(int AUECID)
        {
            lock (_currentOffsetAdjustedAUECDatesLock)
            {
                if (_currentOffsetAdjustedAUECDates.ContainsKey(AUECID))
                {
                    return _currentOffsetAdjustedAUECDates[AUECID];
                }
            }
            return DateTimeConstants.MinValue;
        }

        public DateTime GetCurrentDateForAUECID(int AUECID)
        {
            lock (_currentOffsetAdjustedAUECDatesLock)
            {
                if (_currentOffsetAdjustedAUECDates.ContainsKey(AUECID))
                {
                    return _currentOffsetAdjustedAUECDates[AUECID];
                }
            }
            return DateTimeConstants.MinValue;
        }

        public void GetNextClearanceDateTimes()
        {
            try
            {
                Dictionary<int, DateTime> nextClearanceDateTimes = new Dictionary<int, DateTime>();
                foreach (KeyValuePair<int, DateTime> clearanceTime in _clearanceTime)
                {
                    if (clearanceTime.Value.CompareTo(Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime
                        (DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(clearanceTime.Key))) <= 0)
                    {
                        nextClearanceDateTimes.Add(clearanceTime.Key, clearanceTime.Value.AddDays(1));
                    }
                    else
                    {
                        nextClearanceDateTimes.Add(clearanceTime.Key, clearanceTime.Value);
                    }
                }

                _clearanceTime = nextClearanceDateTimes;
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

        public string GetSameDateForAllAUEC(DateTime theDateTime)
        {
            StringBuilder uTCDateForAllAUEC = new StringBuilder();
            try
            {
                Dictionary<int, Prana.BusinessObjects.TimeZone> AllAUECTimeZones = CachedDataManager.GetInstance.GetAllAUECTimeZones();
                uTCDateForAllAUEC.Append("0");
                uTCDateForAllAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                uTCDateForAllAUEC.Append(theDateTime.Date);
                uTCDateForAllAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> AUECTimeZone in AllAUECTimeZones)
                {
                    int AuecID = AUECTimeZone.Key;
                    uTCDateForAllAUEC.Append(AuecID);
                    uTCDateForAllAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    uTCDateForAllAUEC.Append(theDateTime);
                    uTCDateForAllAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
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
            return uTCDateForAllAUEC.ToString();
        }

        public string GetSameDateInUseAUECStr(DateTime theDateTime)
        {
            StringBuilder uTCDateForInUseAUEC = new StringBuilder();
            try
            {
                List<int> inUseAUECList = _keyValueDataManager.GetInUseAUECIDs();
                if (inUseAUECList.Count > 0)
                {
                    Dictionary<int, Prana.BusinessObjects.TimeZone> AllAUECTimeZones = CachedDataManager.GetInstance.GetAllAUECTimeZones();
                    uTCDateForInUseAUEC.Append("0");
                    uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    uTCDateForInUseAUEC.Append(theDateTime.Date);
                    uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                    foreach (KeyValuePair<int, Prana.BusinessObjects.TimeZone> AUECTimeZone in AllAUECTimeZones)
                    {
                        int AuecID = AUECTimeZone.Key;
                        if (inUseAUECList.Contains(AuecID))
                        {
                            uTCDateForInUseAUEC.Append(AuecID);
                            uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                            uTCDateForInUseAUEC.Append(theDateTime);
                            uTCDateForInUseAUEC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                        }
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
            return uTCDateForInUseAUEC.ToString();
        }

        private void CentralClearanceLogic()
        {
            try
            {
                Dictionary<int, DateTime> currentAUECDateTimes = new Dictionary<int, DateTime>(CachedDataManager.GetInstance.GetAUECCount() + 1);
                CurrentAUECDateTimes(currentAUECDateTimes);

                //+1 size to keep GMT Timezone date as AUECID 0
                _currentOffsetAdjustedAUECDates = new Dictionary<int, DateTime>(CachedDataManager.GetInstance.GetAUECCount() + 1);
                DateTime latestDate = DateTime.UtcNow.AddDays(-1);

                foreach (KeyValuePair<int, DateTime> auecTimes in currentAUECDateTimes)
                {
                    if (!_marketTimingInformation.ContainsKey(auecTimes.Key))
                    {
                        continue;
                    }
                    MarketTimes currentAUECMarketTime = _marketTimingInformation[auecTimes.Key];
                    ClearanceData clearanceDataForAUEC = new ClearanceData();
                    clearanceDataForAUEC.AUECID = auecTimes.Key;
                    TimeSpan timeRemainingToClearance;

                    if (_clearanceTime.ContainsKey(auecTimes.Key))
                    {
                        //The following check finds if choices are T-1/T or T/T+1. Actual value will be one of them
                        int clearanceTimeComparedToMarketCloseTime = _clearanceTime[auecTimes.Key].TimeOfDay.CompareTo(currentAUECMarketTime.MarketEndTime.TimeOfDay);
                        switch (Math.Sign(clearanceTimeComparedToMarketCloseTime))
                        {
                            case 1:
                            case 0:
                                //Clearance Time After Market End Time
                                //Move from T to T+1
                                //The following check will determine if the value is T or the value is T+1
                                timeRemainingToClearance = _clearanceTime[auecTimes.Key].TimeOfDay - auecTimes.Value.TimeOfDay;

                                //Time remaining to clearance less than zero => clearance expired for today
                                if (timeRemainingToClearance < TimeSpan.Zero)
                                {
                                    //Current day T+1
                                    if (!_currentOffsetAdjustedAUECDates.ContainsKey(auecTimes.Key))
                                    {
                                        _currentOffsetAdjustedAUECDates.Add(auecTimes.Key, auecTimes.Value.AddDays(1));
                                    }

                                    //Clearance expired for today
                                    //Only to find out what is the leading most date in any AUEC
                                    if (latestDate.Date < _currentOffsetAdjustedAUECDates[auecTimes.Key].Date)
                                    {
                                        latestDate = _currentOffsetAdjustedAUECDates[auecTimes.Key].Date;
                                    }
                                    timeRemainingToClearance = timeRemainingToClearance.Add(_refreshTimeSpan);
                                }
                                else
                                {
                                    //Clearance Time not reached for today
                                    //Current day T
                                    if (!_currentOffsetAdjustedAUECDates.ContainsKey(auecTimes.Key))
                                    {
                                        _currentOffsetAdjustedAUECDates.Add(auecTimes.Key, auecTimes.Value);
                                    }

                                    if (latestDate.Date < _currentOffsetAdjustedAUECDates[auecTimes.Key].Date)
                                    {
                                        latestDate = _currentOffsetAdjustedAUECDates[auecTimes.Key].Date;
                                    }
                                }
                                break;

                            case -1:
                                //Clearance Time before Market End Time
                                //Move from T-1 to T
                                //The following check will determine if the value is T-1 or the value is T
                                timeRemainingToClearance = _clearanceTime[auecTimes.Key].TimeOfDay - auecTimes.Value.TimeOfDay;
                                if (timeRemainingToClearance < TimeSpan.Zero)
                                {
                                    //Current day T
                                    if (!_currentOffsetAdjustedAUECDates.ContainsKey(auecTimes.Key))
                                    {
                                        _currentOffsetAdjustedAUECDates.Add(auecTimes.Key, auecTimes.Value);
                                    }

                                    //Clearance expired for today
                                    if (latestDate.Date < _currentOffsetAdjustedAUECDates[auecTimes.Key].Date)
                                    {
                                        latestDate = _currentOffsetAdjustedAUECDates[auecTimes.Key].Date;
                                    }
                                    timeRemainingToClearance = timeRemainingToClearance.Add(_refreshTimeSpan);
                                }
                                else
                                {
                                    //Clearance still to be hit for today
                                    //So current day T-1
                                    if (!_currentOffsetAdjustedAUECDates.ContainsKey(auecTimes.Key))
                                    {
                                        _currentOffsetAdjustedAUECDates.Add(auecTimes.Key, auecTimes.Value.AddDays(-1));
                                    }
                                }
                                break;
                        }
                    }
                    else //That implies clearance not set for this ID, so we default it to midnight clearance
                    {
                        DateTime midNightTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        timeRemainingToClearance = midNightTime.TimeOfDay - auecTimes.Value.TimeOfDay;
                        if (!_currentOffsetAdjustedAUECDates.ContainsKey(auecTimes.Key))
                        {
                            _currentOffsetAdjustedAUECDates.Add(auecTimes.Key, auecTimes.Value);
                        }
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

        private Dictionary<int, DateTime> GetAUECWiseMarketEndTime()
        {
            Dictionary<int, DateTime> auecWiseMarketEndTime = new Dictionary<int, DateTime>();
            try
            {
                foreach (KeyValuePair<int, MarketTimes> auecs in _marketTimingInformation)
                {
                    if (!auecWiseMarketEndTime.ContainsKey(auecs.Key))
                    {
                        MarketTimes currentAUECMarketTime = auecs.Value;
                        auecWiseMarketEndTime.Add(auecs.Key, currentAUECMarketTime.MarketEndTime);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return auecWiseMarketEndTime;
        }

        private void SetMidNightClearanceTime()
        {
            try
            {
                foreach (KeyValuePair<int, MarketTimes> auecs in _marketTimingInformation)
                {
                    if (!_clearanceTime.ContainsKey(auecs.Key))
                    {
                        _clearanceTime.Add(auecs.Key, Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(auecs.Key)).Date.AddDays(1));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}