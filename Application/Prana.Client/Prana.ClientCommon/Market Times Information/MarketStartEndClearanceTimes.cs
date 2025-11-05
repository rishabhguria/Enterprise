using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ClientCommon
{
    public class MarketStartEndClearanceTimes
    {

        private static MarketStartEndClearanceTimes _marketStartEndClearanceTimes = null;
        private Dictionary<int, MarketTimes> _marketData = new Dictionary<int, MarketTimes>();

        public static MarketStartEndClearanceTimes GetInstance()
        {
            if (_marketStartEndClearanceTimes == null)
            {
                _marketStartEndClearanceTimes = new MarketStartEndClearanceTimes();
            }
            return _marketStartEndClearanceTimes;
        }


        private MarketStartEndClearanceTimes()
        {
            try
            {
                SetMarketData();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void SetMarketData()
        {
            _marketData = GetAUECIDMarketTimes();
        }


        public System.Collections.Generic.Dictionary<int, MarketTimes> GetAUECIDMarketTimes()
        {
            System.Collections.Generic.Dictionary<int, MarketTimes> AuecIDMarketDataKeyValueCollection = new Dictionary<int, MarketTimes>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllMarketStartEndTimes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {


                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        int AuecID = int.Parse(row[0].ToString());
                        Prana.BusinessObjects.TimeZone auecTimeZone = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECTimeZone(AuecID);
                        DateTime dtMarketStartTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.Parse(row[1].ToString()), auecTimeZone);
                        DateTime dtMarketEndTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.Parse(row[2].ToString()), auecTimeZone);

                        string currentMarketStartDateTimeString = dtMarketStartTime.ToString();
                        string currentMarketEndDateTimeString = dtMarketEndTime.ToString();
                        string[] marketStartDateAndTime = currentMarketStartDateTimeString.Split(' ');
                        string[] marketEndDateAndTime = currentMarketEndDateTimeString.Split(' ');
                        //marketStartDateAndTime[0] = currentDateString;
                        //marketEndDateAndTime[0] = currentDateString;

                        string todayMarketStartDateTime = string.Empty;
                        string todayMarketEndDateTime = string.Empty;
                        for (int i = 0; i < marketStartDateAndTime.Length; i++)
                        {
                            todayMarketStartDateTime += marketStartDateAndTime[i];
                            todayMarketStartDateTime += " ";
                        }
                        todayMarketStartDateTime = todayMarketStartDateTime.Trim();

                        for (int i = 0; i < marketEndDateAndTime.Length; i++)
                        {
                            todayMarketEndDateTime += marketEndDateAndTime[i];
                            todayMarketEndDateTime += " ";
                        }
                        todayMarketEndDateTime = todayMarketEndDateTime.Trim();

                        if (auecTimeZone != null)
                        {
                            //auecTimeZone.SupportsDaylightSavings = false;
                            MarketTimes entry = new MarketTimes();
                            entry.MarketStartTime = DateTime.Parse(todayMarketStartDateTime);
                            entry.MarketEndTime = DateTime.Parse(todayMarketEndDateTime);
                            AuecIDMarketDataKeyValueCollection.Add(int.Parse(row[0].ToString()), entry);
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
            return AuecIDMarketDataKeyValueCollection;

        }

        public MarketTimes GetAUECMarketTimes(int auecID)
        {
            if (_marketData.ContainsKey(auecID))
            {
                return _marketData[auecID];
            }
            else
            {
                return new MarketTimes();
            }
        }

        public DateTime GetAUECMarketEndTime(int auecID)
        {
            if (_marketData.ContainsKey(auecID))
            {
                return _marketData[auecID].MarketEndTime;
            }
            else
            {
                return DateTimeConstants.MinValue;
            }
        }

        public MarketTimes GetAUECMarketTimesByIdentifier(string auecIdentifier)
        {
            int auecID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(auecIdentifier);
            if (_marketData.ContainsKey(auecID))
            {
                return _marketData[auecID];
            }
            else
            {
                return new MarketTimes();
            }
        }

    }
}
