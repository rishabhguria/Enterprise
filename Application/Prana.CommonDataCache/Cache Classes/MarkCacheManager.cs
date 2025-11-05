using Prana.AmqpAdapter.Amqp;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CommonDatabaseAccess;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Prana.CommonDataCache
{
    public static class MarkCacheManager
    {
        private static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> _auecwiseBussinessAdjustedYesterdayMarkPriceDict = new Dictionary<DateTime, Dictionary<string, MarkPriceInfo>>();
        private static ProxyBase<IPublishing> _proxyPublishing = null;
        private static readonly object _locker = new object();
        private static Dictionary<int, DateTime> _auecWiseBusinessAdjustedYesterdayDate;
        private static SortedDictionary<string, List<MarkPriceInfo>> _fxForwardMarkPrices = null;
        private static readonly object _publishLock = new object();
        private static IMarkDataManager _markDataManager;

        private static Dictionary<string, double> _todaysSplitFactorForSymbol;

        public static Dictionary<string, double> TodaysSplitFactorForSymbol
        {
            get { return _todaysSplitFactorForSymbol; }
            set { _todaysSplitFactorForSymbol = value; }
        }

        private static Dictionary<string, MarkPriceInfo> _latestMarkPrices;

        public static Dictionary<string, MarkPriceInfo> LatesMarkPrices
        {
            get { lock (_locker) { return _latestMarkPrices; } }
        }

        public static Dictionary<string, MarkPriceInfo> GetLatestMarkPriceDic()
        {
            try
            {
                return LatesMarkPrices;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        static MarkCacheManager()
        {
            try
            {
                CreatePublishingProxy();
                _markDataManager = WindsorContainerManager.Container.Resolve<IMarkDataManager>();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("PricingPublishingEndpointAddress");
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> GetMarkPriceCacheFromTable(DateTime date, DataTable dtMarkPrices)
        {
            string symbol = string.Empty;
            Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> markPriceDict = null;

            try
            {
                if (dtMarkPrices == null)
                {
                    return null;
                }
                else
                {
                    markPriceDict = new Dictionary<DateTime, Dictionary<string, MarkPriceInfo>>();
                }

                foreach (DataRow dr in dtMarkPrices.Rows)
                {
                    MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);
                    if (markPriceInfo != null)
                    {
                        symbol = markPriceInfo.Symbol;
                        if (!String.IsNullOrEmpty(symbol))
                        {
                            if (markPriceDict.ContainsKey(date))
                            {
                                Dictionary<string, MarkPriceInfo> symbolMarkPriceInfo = markPriceDict[date];
                                if (symbolMarkPriceInfo.ContainsKey(symbol))
                                {
                                    symbolMarkPriceInfo[symbol] = markPriceInfo;
                                }
                                else
                                {
                                    symbolMarkPriceInfo.Add(symbol, markPriceInfo);
                                }
                            }
                            else
                            {
                                Dictionary<string, MarkPriceInfo> symbolMarkPriceInfoNew = new Dictionary<string, MarkPriceInfo>();
                                symbolMarkPriceInfoNew.Add(symbol, markPriceInfo);

                                if (date == DateTimeConstants.MinValue)
                                {
                                    date = markPriceInfo.DateActual;
                                }

                                markPriceDict.Add(date, symbolMarkPriceInfoNew);
                            }
                        }
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

                throw;
            }
            return markPriceDict;
        }

        private static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> GetMarkPriceCacheFromTableForMonth(DataTable dtMarkPrices)
        {
            string symbol = string.Empty;

            Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> markPriceDict = null;
            try
            {
                if (dtMarkPrices == null)
                {
                    return null;
                }
                markPriceDict = new Dictionary<DateTime, Dictionary<string, MarkPriceInfo>>();

                foreach (DataRow dr in dtMarkPrices.Rows)
                {
                    MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);

                    if (markPriceInfo != null && !String.IsNullOrEmpty(markPriceInfo.Symbol))
                    {
                        symbol = markPriceInfo.Symbol;
                        if (markPriceDict.ContainsKey(markPriceInfo.DateActual))
                        {
                            Dictionary<string, MarkPriceInfo> symbolMarkPriceInfo = markPriceDict[markPriceInfo.DateActual];
                            if (symbolMarkPriceInfo.ContainsKey(symbol))
                            {
                                symbolMarkPriceInfo[symbol] = markPriceInfo;
                            }
                            else
                            {
                                symbolMarkPriceInfo.Add(symbol, markPriceInfo);
                            }
                        }
                        else
                        {
                            Dictionary<string, MarkPriceInfo> symbolMarkPriceInfoNew = new Dictionary<string, MarkPriceInfo>();
                            symbolMarkPriceInfoNew.Add(symbol, markPriceInfo);
                            markPriceDict.Add(markPriceInfo.DateActual, symbolMarkPriceInfoNew);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                throw;
            }
            return markPriceDict;
        }

        public static void InitializeMarkPricesCache(string auecString)
        {
            try
            {
                Dictionary<int, DateTime> auecWiseDateDict = TimeZoneHelper.GetInstance().GetAUECDateDictfromAUECString(auecString);

                //Get distinct dates from the above dictionary with its values as list of auecids.
                Dictionary<DateTime, List<int>> distinctDateAUECList = GetDistinctDateWithAUECList(auecWiseDateDict);

                lock (_locker)
                {
                    if (_latestMarkPrices == null)
                    {
                        _latestMarkPrices = new Dictionary<string, MarkPriceInfo>();
                    }

                    _auecwiseBussinessAdjustedYesterdayMarkPriceDict = new Dictionary<DateTime, Dictionary<string, MarkPriceInfo>>();

                    foreach (KeyValuePair<DateTime, List<int>> dateAUECs in distinctDateAUECList)
                    {
                        if (!_auecwiseBussinessAdjustedYesterdayMarkPriceDict.ContainsKey(dateAUECs.Key.Date))
                        {
                            FillBusinessAdjustedDayMarkPricesForDate(dateAUECs.Key.Date, _auecwiseBussinessAdjustedYesterdayMarkPriceDict);
                        }

                        if (_auecwiseBussinessAdjustedYesterdayMarkPriceDict != null && _auecwiseBussinessAdjustedYesterdayMarkPriceDict.ContainsKey(dateAUECs.Key.Date))
                        {
                            GetAUECDateMarkPrices(auecWiseDateDict, dateAUECs.Value, _auecwiseBussinessAdjustedYesterdayMarkPriceDict[dateAUECs.Key.Date], ref _latestMarkPrices);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void AdjustMarkPriceByTodaysSplitFactor(string auecString, bool isUpdated)
        {
            try
            {
                //clear all the split applied before so that cache will be corrected in case of split undo
                if (isUpdated)
                {
                    lock (_locker)
                    {
                        foreach (MarkPriceInfo markPriceObj in _latestMarkPrices.Values)
                            markPriceObj.SplitFactor = 1;
                    }
                }

                _todaysSplitFactorForSymbol = _markDataManager.GetSplitFactors(auecString);
                if (_todaysSplitFactorForSymbol != null)
                    foreach (string symbol in _todaysSplitFactorForSymbol.Keys)
                        if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(symbol) && _todaysSplitFactorForSymbol[symbol] != 0)
                            lock (_locker)
                                _latestMarkPrices[symbol].SplitFactor = _todaysSplitFactorForSymbol[symbol];
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

        private static void FillBusinessAdjustedDayMarkPricesForDate(DateTime requestDate, Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> dicToFill)
        {
            try
            {
                lock (_locker)
                {
                    DataTable dtMarkPrices = _markDataManager.GetBusinessAdjustedDayMarkPriceForGivenDate(requestDate.Date);
                    if (dtMarkPrices == null)
                    {
                        return;
                    }
                    if (dtMarkPrices.Rows.Count == 0)
                    {
                        return;
                    }
                    Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> dateMarkPrice = GetMarkPriceCacheFromTable(requestDate.Date, dtMarkPrices);

                    if (dicToFill.ContainsKey(requestDate.Date))
                    {
                        dicToFill[requestDate.Date] = dateMarkPrice[requestDate.Date];
                    }
                    else
                    {
                        dicToFill.Add(requestDate.Date, dateMarkPrice[requestDate.Date]);
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

        private static Dictionary<string, DateTime> _symbolYesterdayDate = new Dictionary<string, DateTime>();

        /// <summary>
        /// Here when expnl is asking data for yesterday mark price, then we assign the yesterday dates for each symbol. Then further
        /// Pricing service refers this and fetches yesterday mark for particular symbol.
        /// </summary>
        /// <param name="auecWiseDateDict"></param>
        /// <param name="auecs"></param>
        /// <param name="inputMarkPriceDict"></param>
        /// <param name="outputMarkPriceDict"></param>
        private static void GetAUECDateMarkPrices(Dictionary<int, DateTime> auecWiseDateDict, List<int> auecs, Dictionary<string, MarkPriceInfo> inputMarkPriceDict, ref Dictionary<string, MarkPriceInfo> outputMarkPriceDict)
        {
            try
            {
                lock (_locker)
                {
                    foreach (KeyValuePair<string, MarkPriceInfo> symbolMarkKeyValue in inputMarkPriceDict)
                    {
                        int auecID = symbolMarkKeyValue.Value.AUECID;
                        string symbol = symbolMarkKeyValue.Key;

                        if (auecs.Contains(auecID))
                        {
                            symbolMarkKeyValue.Value.YesterdayBusinessAdjustedDate = auecWiseDateDict[auecID];
                            if (!outputMarkPriceDict.ContainsKey(symbol))
                            {
                                outputMarkPriceDict.Add(symbol, symbolMarkKeyValue.Value);
                                UpdateFxForwardsMarkPxCache(symbolMarkKeyValue.Value);
                            }
                            else
                                outputMarkPriceDict[symbol] = symbolMarkKeyValue.Value;

                            //further no need will be of this
                            FillSymbolAndYesterdayDate(symbol, auecID, auecWiseDateDict);
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

        /// <summary>
        /// It fills for the yesterday date for any symbol. The yesterday date is the same as provided by expnl using clearance logic
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="auecID"></param>
        /// <param name="auecWiseDateDict"></param>
        private static void FillSymbolAndYesterdayDate(string symbol, int auecID, Dictionary<int, DateTime> auecWiseDateDict)
        {
            try
            {
                if (_symbolYesterdayDate.ContainsKey(symbol))
                {
                    if (auecWiseDateDict.ContainsKey(auecID))
                    {
                        _symbolYesterdayDate[symbol] = auecWiseDateDict[auecID];
                    }
                }
                else
                {
                    if (auecWiseDateDict.ContainsKey(auecID))
                    {
                        _symbolYesterdayDate.Add(symbol, auecWiseDateDict[auecID]);
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

        private static Dictionary<DateTime, List<int>> GetDistinctDateWithAUECList(Dictionary<int, DateTime> auecWiseDateDict)
        {
            Dictionary<DateTime, List<int>> distinctDateAUECList = new Dictionary<DateTime, List<int>>();
            try
            {
                if (_auecWiseBusinessAdjustedYesterdayDate == null)
                    _auecWiseBusinessAdjustedYesterdayDate = new Dictionary<int, DateTime>();

                foreach (KeyValuePair<int, DateTime> keyValueDate in auecWiseDateDict)
                {
                    if (distinctDateAUECList.ContainsKey(keyValueDate.Value.Date))
                    {
                        distinctDateAUECList[keyValueDate.Value.Date].Add(keyValueDate.Key);
                    }
                    else
                    {
                        List<int> auecIDs = new List<int>();
                        auecIDs.Add(keyValueDate.Key);
                        distinctDateAUECList.Add(keyValueDate.Value.Date, auecIDs);
                    }

                    #region TO maintain Auec Wise BusinessAdjustedYesterdayDate

                    if (_auecWiseBusinessAdjustedYesterdayDate.ContainsKey(keyValueDate.Key))
                        _auecWiseBusinessAdjustedYesterdayDate[keyValueDate.Key] = keyValueDate.Value.Date;
                    else
                        _auecWiseBusinessAdjustedYesterdayDate.Add(keyValueDate.Key, keyValueDate.Value.Date);

                    #endregion
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
            return distinctDateAUECList;
        }

        /// <summary>
        /// dateMethodology = 0 for daily, 2 for monthly
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateMethodology"></param>
        /// <returns></returns>
        public static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> GetMarkPriceForDate(DateTime date, int dateMethodology, bool isFxFXForwardData, bool getSameDayClosedDataOnDV)
        {
            try
            {
                lock (_locker)
                {
                    if (dateMethodology == 0)
                    {
                        DataTable dtMarkPrices = _markDataManager.GetMarkPricesForGivenDate(date.Date, 0, isFxFXForwardData, getSameDayClosedDataOnDV);
                        if (dtMarkPrices == null)
                        {
                            return null;
                        }
                        if (dtMarkPrices.Rows.Count == 0)
                        {
                            return null;
                        }
                        return GetMarkPriceCacheFromTable(date.Date, dtMarkPrices);
                    }
                    else if (dateMethodology == 2)
                    {
                        DataTable dtMarkPrices = _markDataManager.GetMarkPricesForGivenDate(date.Date, 2, isFxFXForwardData, getSameDayClosedDataOnDV);
                        return GetMarkPriceCacheFromTableForMonth(dtMarkPrices);
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
            return null;
        }

        private static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> GetBusinessAdjustedDayMarkPriceForDate(DateTime date)
        {
            try
            {
                lock (_locker)
                {
                    DataTable dtBusinessAdjustedDayMarkPrices = _markDataManager.GetBusinessAdjustedDayMarkPriceForGivenDate(date.Date);
                    if (dtBusinessAdjustedDayMarkPrices == null)
                    {
                        return null;
                    }
                    if (dtBusinessAdjustedDayMarkPrices.Rows.Count == 0)
                    {
                        return null;
                    }
                    return GetMarkPriceCacheFromTable(date.Date, dtBusinessAdjustedDayMarkPrices);
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

            return null;
        }

        /// <summary>
        /// Gets the markprices for the exact date and symbol list
        /// </summary>
        /// <param name="dictSymbolWithSettlementDate">Used for passing dictionary of string,date type.</param>
        /// <returns></returns>
        public static Dictionary<string, double> GetMarkPricesForSymbolAndExactDate(Dictionary<string, DateTime> dictSymbolWithSettlementDate)
        {
            Dictionary<string, double> dictSymbolWithMarkPrice = new Dictionary<string, double>();
            try
            {
                foreach (KeyValuePair<string, DateTime> kvp in dictSymbolWithSettlementDate)
                {
                    if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(kvp.Key))
                    {
                        //date actual is the date for which taxlot have not zero mark price
                        //if cash settlement date and actual date are equal, mark price of date actual would be cash settlement price
                        if (_latestMarkPrices[kvp.Key].DateActual.Date.Equals(kvp.Value.Date))
                        {
                            dictSymbolWithMarkPrice.Add(kvp.Key, _latestMarkPrices[kvp.Key].MarkPrice);
                        }
                    }
                    if (!dictSymbolWithMarkPrice.ContainsKey(kvp.Key))
                    {
                        dictSymbolWithMarkPrice.Add(kvp.Key, GetMarkPriceForDateAndSymbol(kvp.Value.Date, kvp.Key));
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
            return dictSymbolWithMarkPrice;
        }

        /// <summary>
        /// Gets the mark prices for symbol on exact date.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static double GetMarkPricesForSymbolOnExactDate(string symbol, DateTime date)
        {
            double markPrice = double.MinValue;
            try
            {
                if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(symbol) && _latestMarkPrices[symbol].DateActual.Date.Equals(date.Date))
                {
                    markPrice = _latestMarkPrices[symbol].MarkPrice;
                }
                else
                {
                    Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> _fetchedData = GetMarkPriceForDate(date, 0, false, false);
                    if (_fetchedData != null && _fetchedData.ContainsKey(date) && _fetchedData[date].ContainsKey(symbol) && _fetchedData[date][symbol].DateActual.Date.Equals(date.Date))
                        markPrice = _fetchedData[date][symbol].MarkPrice;
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
            return markPrice;
        }

        public static double GetMarkPriceForDateAndSymbol(DateTime date, string symbol)
        {
            double markPrice = 0;
            try
            {
                if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(symbol) && _latestMarkPrices[symbol].YesterdayBusinessAdjustedDate == date.Date)
                {
                    return _latestMarkPrices[symbol].MarkPrice;
                }
                else
                {
                    Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> _fetchedData = GetMarkPriceForDate(date, 0, false, false);
                    if (_fetchedData != null && _fetchedData.ContainsKey(date) && _fetchedData[date].ContainsKey(symbol))
                        return _fetchedData[date][symbol].MarkPrice;
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
            return markPrice;
        }

        /// <summary>
        /// Gets the mark price for option expiry.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="expiryDate">The expiry date.</param>
        /// <returns></returns>
        public static Dictionary<Tuple<string, DateTime>, double> GetMarkPricesForOptExpiry(List<Tuple<string, DateTime>> symbolDatePairs)
        {
            Dictionary<Tuple<string, DateTime>, double> markPrices = new Dictionary<Tuple<string, DateTime>, double>();
            try
            {
                DataTable data = new DataTable("SymbolDate");
                data.Columns.Add("Symbol");
                data.Columns.Add("Date");
                foreach (Tuple<string, DateTime> pair in symbolDatePairs)
                {
                    if (!markPrices.ContainsKey(pair))
                    {
                        markPrices.Add(pair, 0);
                        string symbol = pair.Item1;
                        DateTime expiryDate = pair.Item2;

                        if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(symbol) && _latestMarkPrices[symbol].DateActual.Date.Equals(expiryDate.Date))
                        {
                            markPrices[pair] = _latestMarkPrices[symbol].MarkPrice;
                        }
                        else if (expiryDate == DateTime.Today)
                        {
                            data.Rows.Add(symbol, expiryDate);
                            data.Rows.Add(symbol, DateTimeHelper.GetYesterdayDate());
                        }
                        else
                        {
                            data.Rows.Add(symbol, expiryDate);
                        }
                    }
                }
                if (data.Rows.Count > 0)
                {
                    DataSet ds = _markDataManager.GetMarkPricesForSymbolsAndExactDates(data);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            Tuple<string, DateTime> pair = new Tuple<string, DateTime>(row[0].ToString(), Convert.ToDateTime(row[1].ToString()));
                            markPrices[pair] = Convert.ToDouble(row[2].ToString());
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
            return markPrices;
        }

        /// <summary>
        /// It ask for the yesterday mark price for any symbol. The yesterday date is the same as provided by expnl using clearance logic
        /// </summary>
        /// <param name="date"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static double GetMarkPriceForSymbol(string symbol)
        {
            try
            {
                if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(symbol))
                {
                    return _latestMarkPrices[symbol].MarkPrice;
                }
                else
                {
                    ///double.MinValue represents that markprices doesn't exist for the supplied symbol.
                    return double.MinValue;
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
            return 0;
        }

        public static int SaveMarkPrices(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                bool isAutoApproved = true;
                rowsAffected = _markDataManager.SaveMarkPrices(dt, isAutoApproved);

                WaitCallback callBackHandler = new WaitCallback(UpdateMarkCacheAndPublishData);
                ThreadPool.QueueUserWorkItem(callBackHandler, dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return rowsAffected;
        }

        private static void UpdateMarkCacheAndPublishData(object dataTable)
        {
            try
            {
                DataTable dt = dataTable as DataTable;
                UpdateBusinessAdjustedDayMarkPriceCacheNew(dt);

                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_MarkPrice;
                CentralizePublish(e);
                string symbol = dt.Rows[0]["Symbol"].ToString();
                if(symbol != null && LatesMarkPrices.ContainsKey(symbol))
                    AmqpHelper.SendObject(LatesMarkPrices[symbol], "OtherData", "MarkPrice");
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

        /// <summary>
        /// It will recalculate FX and FXForward mark prices for date on which FX rates has been updated.
        /// </summary>
        /// <param name="dtFXRates"></param>
        private static DataTable RecalculateFXForwardMarkPrice(DataTable dtFXRates)
        {
            DataSet ds = null;
            DataTable dtMarkPricesNew = null;
            try
            {
                if (_fxForwardMarkPrices != null && _fxForwardMarkPrices.Count > 0)
                {
                    //List containing FX/Forwards need to be published.
                    List<MarkPriceInfo> updatedClosingMark = new List<MarkPriceInfo>();

                    foreach (DataRow dr in dtFXRates.Rows)
                    {
                        string leadCurrency = CachedDataManager.GetInstance.GetCurrencyText(int.Parse(dr["FromCurrencyID"].ToString()));
                        string vsCurrency = CachedDataManager.GetInstance.GetCurrencyText(int.Parse(dr["ToCurrencyID"].ToString()));

                        string currencyPair = leadCurrency + Seperators.SEPERATOR_7 + vsCurrency;
                        string reverseCurrencyPair = vsCurrency + Seperators.SEPERATOR_7 + leadCurrency;

                        //Date on which FX rates has been saved from Daily Valuation.
                        DateTime currentDate = DateTime.Parse(dr["Date"].ToString());
                        double fxRate = double.Parse(dr["ConversionFactor"].ToString());

                        //Currency pairs like "USD-JPY".
                        if (_fxForwardMarkPrices.ContainsKey(currencyPair))
                        {
                            List<MarkPriceInfo> fxSymbolsList = _fxForwardMarkPrices[currencyPair];
                            foreach (MarkPriceInfo mpInfo in fxSymbolsList)
                            {
                                //Actual date should match with the date on which fx rates has been saved for
                                //correct calculations.
                                if (mpInfo.DateActual.Date.Equals(currentDate.Date))
                                {
                                    if (fxRate > 0)
                                    {
                                        mpInfo.FxRate = fxRate;
                                        mpInfo.MarkPrice = mpInfo.FxRate + mpInfo.ForwardPoints;
                                        updatedClosingMark.Add(mpInfo);
                                    }
                                }
                            }
                        }

                        //Reverse currency pairs like "JPY-USD".
                        if (_fxForwardMarkPrices.ContainsKey(reverseCurrencyPair))
                        {
                            List<MarkPriceInfo> fxSymbolsList = _fxForwardMarkPrices[reverseCurrencyPair];
                            foreach (MarkPriceInfo mpInfo in fxSymbolsList)
                            {
                                if (mpInfo.DateActual.Date.Equals(currentDate.Date))
                                {
                                    if (fxRate > 0)
                                    {
                                        mpInfo.FxRate = 1 / fxRate;
                                        mpInfo.MarkPrice = mpInfo.FxRate + mpInfo.ForwardPoints;
                                        updatedClosingMark.Add(mpInfo);
                                    }
                                }
                            }
                        }
                    }

                    ds = GeneralUtilities.CreateDataSetFromCollection(updatedClosingMark, null);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable dtMarkPrices = ds.Tables[0];
                        //Creating a table with the stipulated columns to convert it to XML before saving.
                        dtMarkPricesNew = new DataTable();
                        dtMarkPricesNew.TableName = "MarkPriceImport";
                        dtMarkPricesNew.Columns.Add(new DataColumn("Symbol"));
                        dtMarkPricesNew.Columns.Add(new DataColumn("Date"));
                        dtMarkPricesNew.Columns.Add(new DataColumn("MarkPrice"));
                        dtMarkPricesNew.Columns.Add(new DataColumn("MarkPriceImportType"));
                        dtMarkPricesNew.Columns.Add(new DataColumn("ForwardPoints"));
                        dtMarkPricesNew.Columns.Add(new DataColumn("DateActual"));

                        foreach (DataRow dr in dtMarkPrices.Rows)
                        {
                            //Assigning the row having symbol being not blank.
                            if (!dr["Symbol"].ToString().Equals(""))
                            {
                                DataRow drNew = dtMarkPricesNew.NewRow();

                                drNew["Date"] = dr["DateActual"].ToString();
                                drNew["DateActual"] = dr["DateActual"].ToString();
                                drNew["MarkPrice"] = dr["MarkPrice"].ToString();
                                drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                drNew["ForwardPoints"] = dr["ForwardPoints"].ToString();

                                // this column value has been fixed to differentiate whether data save into the DB from Import module or Mark price UI
                                // 'L' stands for Live feed Data
                                drNew["MarkPriceImportType"] = Prana.BusinessObjects.AppConstants.MarkPriceImportType.L.ToString();

                                dtMarkPricesNew.Rows.Add(drNew);
                                dtMarkPricesNew.AcceptChanges();
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
            return dtMarkPricesNew;
        }

        private static void UpdateBusinessAdjustedDayMarkPriceCacheNew(DataTable dtMarkPrices)
        {
            DateTime date = DateTimeConstants.MinValue;
            string symbol = string.Empty;
            int auecID = int.MinValue;
            if (dtMarkPrices == null || dtMarkPrices.Rows.Count == 0)
            {
                return;
            }
            if (!dtMarkPrices.Columns.Contains("DateActual"))
            {
                dtMarkPrices.Columns.Add("DateActual", typeof(DateTime));
            }

            try
            {
                lock (_locker)
                {
                    Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> latestFetchedData = null;
                    foreach (DataRow dr in dtMarkPrices.Rows)
                    {
                        if (!String.IsNullOrEmpty(dr["Date"].ToString()) && !String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            date = Convert.ToDateTime(dr["Date"]).Date;
                            dr["DateActual"] = date;
                            symbol = dr["Symbol"].ToString().ToUpper();
                            double markPrice = Convert.ToDouble(dr["MarkPrice"]);
                            double forwardPoints = 0;
                            if (dtMarkPrices.Columns.Contains("ForwardPoints") && dr["ForwardPoints"] != System.DBNull.Value)
                            {
                                forwardPoints = Convert.ToDouble(dr["ForwardPoints"]);
                            }

                            #region If markprice is updated for a symbol which doesnot exist in cache

                            if (_latestMarkPrices == null)
                                _latestMarkPrices = new Dictionary<string, MarkPriceInfo>();
                            if (!_latestMarkPrices.ContainsKey(symbol))
                            {
                                if (dtMarkPrices.Columns.Contains("AUECID") && dr["AUECID"] != System.DBNull.Value)
                                    auecID = Convert.ToInt32(dr["AUECID"]);

                                int assetId = CachedDataManager.GetInstance.GetAssetIdByAUECId(auecID);

                                if (auecID != int.MinValue && _auecWiseBusinessAdjustedYesterdayDate != null && _auecWiseBusinessAdjustedYesterdayDate.ContainsKey(auecID))
                                {
                                    MarkPriceInfo newObj = new MarkPriceInfo();
                                    newObj.AUECID = auecID;
                                    if (dtMarkPrices.Columns.Contains("AUECIdentifier") && dr["AUECIdentifier"] != System.DBNull.Value)
                                        newObj.AUECIdentifier = dr["AUECIdentifier"].ToString();
                                    newObj.YesterdayBusinessAdjustedDate = _auecWiseBusinessAdjustedYesterdayDate[auecID];
                                    if (!_latestMarkPrices.ContainsKey(symbol))
                                        _latestMarkPrices.Add(symbol, newObj);

                                    if (assetId != int.MinValue && (assetId == (int)(AssetCategory.FXForward) || assetId == (int)(AssetCategory.FX)))
                                    {
                                        int leadCurrency = 0;
                                        int vsCurrency = 0;

                                        newObj.LeadCurrencyID = leadCurrency;
                                        newObj.VsCurrencyID = vsCurrency;
                                        newObj.AssetID = assetId;
                                        UpdateFxForwardsMarkPxCache(newObj);
                                    }
                                }
                            }
                            #endregion

                            if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(symbol))
                            {
                                //Added check if mark price does not exist in data base then it must be updated by mark price provided
                                //for any date > DateTime.MinValue
                                if (_latestMarkPrices[symbol].MarkPrice == 0)
                                    _latestMarkPrices[symbol].DateActual = DateTime.MinValue;

                                if (_latestMarkPrices[symbol].DateActual <= date && _latestMarkPrices[symbol].YesterdayBusinessAdjustedDate >= date)
                                {
                                    if (BusinessDayCalculator.GetInstance().IsBusinessDayForAUEC(date, _latestMarkPrices[symbol].AUECID))
                                    {
                                        //if user make mark price 0 for the actualdate mark price
                                        if (_latestMarkPrices[symbol].DateActual == date && markPrice == 0)
                                        {
                                            if (latestFetchedData == null || (latestFetchedData != null && !latestFetchedData.ContainsKey(date)))
                                                latestFetchedData = GetBusinessAdjustedDayMarkPriceForDate(date);
                                            if (latestFetchedData != null && latestFetchedData.ContainsKey(date) && latestFetchedData[date].ContainsKey(symbol))
                                            {
                                                _latestMarkPrices[symbol].DateActual = latestFetchedData[date][symbol].DateActual;
                                                _latestMarkPrices[symbol].MarkPrice = latestFetchedData[date][symbol].MarkPrice;
                                                _latestMarkPrices[symbol].ForwardPoints = latestFetchedData[date][symbol].ForwardPoints;
                                                //If No Mark Price Is Saved In Db Then dateActual Sholud be minimum
                                                if (latestFetchedData[date][symbol].MarkPrice == 0)
                                                    latestFetchedData[date][symbol].DateActual = DateTime.MinValue;
                                                dr["MarkPrice"] = latestFetchedData[date][symbol].MarkPrice;
                                                dr["DateActual"] = latestFetchedData[date][symbol].DateActual;
                                                if (dtMarkPrices.Columns.Contains("ForwardPoints"))
                                                {
                                                    dr["ForwardPoints"] = latestFetchedData[date][symbol].ForwardPoints;
                                                }
                                            }
                                        }
                                        else if (markPrice != 0)
                                        {
                                            _latestMarkPrices[symbol].DateActual = date;
                                            _latestMarkPrices[symbol].MarkPrice = markPrice;
                                            _latestMarkPrices[symbol].ForwardPoints = forwardPoints;
                                        }

                                        //if (_latestMarkPrices[symbol].DateActual == _latestMarkPrices[symbol].YesterdayBusinessAdjustedDate)
                                        //    _latestMarkPrices[symbol].MarkPriceIndicator = 1;
                                        //else
                                        //    _latestMarkPrices[symbol].MarkPriceIndicator = 0;

                                        //[RG:20130125] Commented.
                                        //if (!listSymbolsUpdated.Contains(symbol))
                                        //{
                                        //    listSymbolsUpdated.Add(symbol);
                                        //}

                                        //if (_auecwiseBussinessAdjustedYesterdayMarkPriceDict.ContainsKey(date) && _auecwiseBussinessAdjustedYesterdayMarkPriceDict[date].ContainsKey(symbol))
                                        //{
                                        //    //what if mark price is feeded 0
                                        //    _auecwiseBussinessAdjustedYesterdayMarkPriceDict[date][symbol].MarkPrice = Convert.ToDouble(dr["MarkPrice"].ToString());

                                        //}
                                    }
                                }
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
            //return listSymbolsUpdated;
        }

        private static void UpdateFxForwardsMarkPxCache(MarkPriceInfo markInfo)
        {
            try
            {
                if (_fxForwardMarkPrices == null)
                {
                    _fxForwardMarkPrices = new SortedDictionary<string, List<MarkPriceInfo>>();
                }

                string symbol = string.Empty;
                if (markInfo.AssetID.Equals((int)AssetCategory.FX) || markInfo.AssetID.Equals((int)AssetCategory.FXForward))
                    symbol = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetPranaForexSymbolFromCurrencies(markInfo.LeadCurrencyID, markInfo.VsCurrencyID);

                if (!string.IsNullOrEmpty(symbol))
                {
                    if (_fxForwardMarkPrices.ContainsKey(symbol))
                    {
                        if (!_fxForwardMarkPrices[symbol].Contains(markInfo))
                        {
                            _fxForwardMarkPrices[symbol].Add(markInfo);
                        }
                    }
                    else
                    {
                        List<MarkPriceInfo> listNew = new List<MarkPriceInfo>();
                        listNew.Add(markInfo);
                        _fxForwardMarkPrices.Add(symbol, listNew);
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
        }

        private static MarkPriceInfo GetMarkPriceInfoFromRow(DataRow dr)
        {
            MarkPriceInfo markPriceInfo = null;
            try
            {
                if (!String.IsNullOrEmpty(dr["Symbol"].ToString()))
                {
                    markPriceInfo = new MarkPriceInfo();

                    markPriceInfo.Symbol = dr["Symbol"].ToString().ToUpper();
                    if (dr.Table.Columns.Contains("AUECID") && dr["AUECID"] != DBNull.Value)
                    {
                        markPriceInfo.AUECID = Convert.ToInt32(dr["AUECID"]);
                    }
                    if (dr.Table.Columns.Contains("AUECIdentifier") && dr["AUECIdentifier"] != DBNull.Value)
                    {
                        markPriceInfo.AUECIdentifier = dr["AUECIdentifier"].ToString().ToUpper();
                    }

                    if (dr.Table.Columns.Contains("Date") && dr["Date"] != DBNull.Value)
                    {
                        markPriceInfo.DateActual = Convert.ToDateTime(dr["Date"]).Date;
                    }

                    if (dr.Table.Columns.Contains("MarkPrice") && dr["MarkPrice"] != DBNull.Value)
                    {
                        markPriceInfo.MarkPrice = Convert.ToDouble(dr["MarkPrice"].ToString());
                    }
                    if (dr.Table.Columns.Contains("FinalMarkPrice") && dr["FinalMarkPrice"] != DBNull.Value)
                    {
                        markPriceInfo.MarkPrice = Convert.ToDouble(dr["FinalMarkPrice"].ToString());
                    }
                    if (dr.Table.Columns.Contains("ForwardPoints") && dr["ForwardPoints"] != DBNull.Value)
                    {
                        markPriceInfo.ForwardPoints = Convert.ToDouble(dr["ForwardPoints"].ToString());
                    }
                    if (dr.Table.Columns.Contains("FxRate") && dr["FxRate"] != DBNull.Value)
                    {
                        markPriceInfo.FxRate = Convert.ToDouble(dr["FxRate"].ToString());
                    }
                    if (dr.Table.Columns.Contains("AssetID") && dr["AssetID"] != DBNull.Value)
                    {
                        markPriceInfo.AssetID = Convert.ToInt32(dr["AssetID"].ToString());
                    }
                    if (dr.Table.Columns.Contains("LeadCurrencyID") && dr["LeadCurrencyID"] != DBNull.Value)
                    {
                        markPriceInfo.LeadCurrencyID = Convert.ToInt32(dr["LeadCurrencyID"].ToString());
                    }
                    if (dr.Table.Columns.Contains("VsCurrencyID") && dr["VsCurrencyID"] != DBNull.Value)
                    {
                        markPriceInfo.VsCurrencyID = Convert.ToInt32(dr["VsCurrencyID"].ToString());
                    }
                    if (dr.Table.Columns.Contains("BloombergSymbol") && dr["BloombergSymbol"] != DBNull.Value)
                    {
                        markPriceInfo.BloombergSymbol = dr["BloombergSymbol"].ToString();
                    }
                    if (dr.Table.Columns.Contains("FundID") && dr["FundID"] != DBNull.Value)
                    {
                        markPriceInfo.AccountID = Convert.ToInt32(dr["FundID"].ToString());
                    }
                    if (dr.Table.Columns.Contains("SourceID") && dr["SourceID"] != DBNull.Value)
                    {
                        markPriceInfo.PricingSource = dr["SourceID"].ToString();
                    }
                    else
                    {
                        markPriceInfo.PricingSource = PricingSource.None.ToString();
                    }
                    if (dr.Table.Columns.Contains("PricingType") && dr["PricingType"] != DBNull.Value)
                    {
                        markPriceInfo.PricingType = dr["PricingType"].ToString();
                    }
                    if (dr.Table.Columns.Contains("FundName") && dr["FundName"] != DBNull.Value)
                    {
                        markPriceInfo.AccountName = dr["FundName"].ToString();
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
            return markPriceInfo;
        }

        // TODO : Received only the changed dataset.
        public static int SaveBeta(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                // Save into db
                rowsAffected = _markDataManager.SaveBeta(dt);

                // TODO : Rather than refreshing cache from db, have a logic on the front end which updates the values correctly in cache.
                //RefreshCache();

                // Update cache
                //UpdateMarkPriceCache(dt);

                // Publish prices
                MessageData e = new MessageData();

                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_Beta;
                //_proxyPublishing.InnerChannel.Publish(e, Topics.Topic_Beta);
                CentralizePublish(e);
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

            // To inform LiveFeed
            //if (MarkPriceUpdate != null)
            //{
            //    MarkPriceUpdate();
            //}

            return rowsAffected;
        }

        // TODO : Received only the changed dataset.
        public static int SaveOutStandings(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                // Save into db
                rowsAffected = _markDataManager.SaveOutStandings(dt);

                // TODO : Rather than refreshing cache from db, have a logic on the front end which updates the values correctly in cache.
                //RefreshCache();

                // Update cache
                //UpdateMarkPriceCache(dt);

                // Publish prices
                MessageData e = new MessageData();

                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_OutStandings;
                // _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_OutStandings);
                CentralizePublish(e);
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

            // To inform LiveFeed
            //if (MarkPriceUpdate != null)
            //{
            //    MarkPriceUpdate();
            //}
            return rowsAffected;
        }

        public static int SaveForexRate(DataTable dt)
        {
            int rowsAffected = 0;
            try
            {
                //Save Fx rates into database.
                rowsAffected = _markDataManager.SaveForexRate(dt);

                if (rowsAffected > 0)
                {
                    //ForexConversionCache updated though Pub-Sub
                    //ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).UpdateForexConversionCache(dt);
                    if (_fxForwardMarkPrices != null && _fxForwardMarkPrices.Count > 0)
                    {
                        //Publishing FX/Forward Mark PX after saving FX rates since Mark Px is dependent
                        //on FX rate.
                        DataTable dtNew = RecalculateFXForwardMarkPrice(dt);
                        if (dtNew != null && dtNew.Rows.Count > 0)
                        {
                            MessageData msg = new MessageData();
                            msg.EventData = DataTableToListConverter.GetListFromDataTable(dtNew);
                            msg.TopicName = Topics.Topic_MarkPrice;
                            // _proxyPublishing.InnerChannel.Publish(msg, Topics.Topic_MarkPrice);
                            CentralizePublish(msg);
                        }
                    }
                    //Publish Forex Rate
                    MessageData e = new MessageData();
                    e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                    e.TopicName = Topics.Topic_ForexRate;
                    //_proxyPublishing.InnerChannel.Publish(e, Topics.Topic_ForexRate);
                    CentralizePublish(e);
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
            return rowsAffected;
        }

        public static int SaveVolatility(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                // Save into db
                rowsAffected = _markDataManager.SaveVolatility(dt);
                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_DailyVolatility;
                //  _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_DailyVolatility);
                CentralizePublish(e);
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
            return rowsAffected;
        }

        /// <summary>
        /// Saves the vwap.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static int SaveVWAP(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                // Save into db
                rowsAffected = _markDataManager.SaveVWAP(dt);
                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_DailyVWAP;
                //  _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_DailyVolatility);
                CentralizePublish(e);
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
            return rowsAffected;
        }

        public static int SaveDividendYield(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                // Save into db
                rowsAffected = _markDataManager.SaveDividendYield(dt);
                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_DailyDividendYield;
                // _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_DailyDividendYield);
                CentralizePublish(e);
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
            return rowsAffected;
        }

        public static int SavePerformanceNumbers(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                rowsAffected = _markDataManager.SavePerformanceNumberValues(dt);
                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_PerformanceNumber;
                //_proxyPublishing.InnerChannel.Publish(e, Topics.Topic_PerformanceNumber);
                CentralizePublish(e);
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
            return rowsAffected;
        }

        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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

        public static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> GetMarkPriceForDateRange(string xmlAccounts, DateTime startDate, DateTime endDate, int dateMethodology, bool isFxForwardData, int filter)
        {
            try
            {
                lock (_locker)
                {
                    //if (dateMethodology == 0)
                    //{
                    //    DataTable dtMarkPrices = _markDataManager.GetMarkPricesForGivenDateRange(xmlAccounts, startdate.Date, endDate.Date, dateMethodology, isFxFXForwardData);
                    //    return GetMarkPriceCacheFromTableForMonth(startdate.Date, dtMarkPrices);
                    //}
                    //else if (dateMethodology == 2)
                    //{
                    //    DataTable dtMarkPrices = _markDataManager.GetMarkPricesForGivenDateRange(xmlAccounts, startdate.Date, endDate.Date, dateMethodology, isFxFXForwardData);
                    //    return GetMarkPriceCacheFromTableForMonth(startdate.Date, dtMarkPrices);
                    //}
                    //else if (dateMethodology == 3)
                    //{
                    DataTable dtMarkPrices = _markDataManager.GetMarkPricesForGivenDateRange(xmlAccounts, startDate.Date, endDate.Date, dateMethodology, isFxForwardData, filter);
                    if (dateMethodology == 0)
                    {
                        return GetMarkPriceCacheFromTable(startDate.Date, dtMarkPrices);
                        //return GetMarkPriceCacheFromTableForMonth(startdate.Date, dtMarkPrices);
                    }
                    else if (dateMethodology == 2)
                    {
                        return GetMarkPriceCacheFromTableForMonth(dtMarkPrices);
                    }
                    else if (dateMethodology == 3)
                    {
                        return GetMarkPriceCacheFromTableForDateRange(dtMarkPrices);
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

            return null;
        }

        private static Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> GetMarkPriceCacheFromTableForDateRange(DataTable dtMarkPrices)
        {
            string symbol = string.Empty;
            //int auecID = 0;
            //double markPrice = 0;
            //int markPriceIndicator = 0;

            Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> markPriceDict = null;
            try
            {
                if (dtMarkPrices == null)
                {
                    return null;
                }
                markPriceDict = new Dictionary<DateTime, Dictionary<string, MarkPriceInfo>>();

                foreach (DataRow dr in dtMarkPrices.Rows)
                {
                    MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);

                    //commented by - omshiv symbol should be initialise after null check
                    // symbol = markPriceInfo.Symbol;
                    if (markPriceInfo != null && !String.IsNullOrEmpty(markPriceInfo.Symbol))
                    {
                        //modified by omshiv,
                        symbol = markPriceInfo.Symbol;
                        if (markPriceDict.ContainsKey(markPriceInfo.DateActual))
                        {
                            Dictionary<string, MarkPriceInfo> symbolMarkPriceInfo = markPriceDict[markPriceInfo.DateActual];
                            if (symbolMarkPriceInfo.ContainsKey(symbol))
                            {
                                symbolMarkPriceInfo[symbol] = markPriceInfo;
                            }
                            else
                            {
                                symbolMarkPriceInfo.Add(symbol, markPriceInfo);
                            }
                        }
                        else
                        {
                            Dictionary<string, MarkPriceInfo> symbolMarkPriceInfoNew = new Dictionary<string, MarkPriceInfo>();
                            symbolMarkPriceInfoNew.Add(symbol, markPriceInfo);
                            markPriceDict.Add(markPriceInfo.DateActual, symbolMarkPriceInfoNew);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

                throw;
            }

            return markPriceDict;
        }

        /// <summary>
        /// Saves the collateral values.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static int SaveCollateralValues(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                rowsAffected = _markDataManager.SaveCollateralValues(dt);

                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_DailyCollateral;
                //  _proxyPublishing.InnerChannel.Publish(e, Topics.Topic_DailyVolatility);
                CentralizePublish(e);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// Gets the collateral price date wise.
        /// </summary>
        /// <param name="dateSelected">The date selected.</param>
        /// <param name="dateMethodology">The date methodology.</param>
        /// <param name="getSameDayClosedDataOnDV">if set to <c>true</c> [get same day closed data on dv].</param>
        /// <param name="isOnlyFixedIncomeSymbols">if set to <c>true</c> [is only fixed income symbols].</param>
        /// <returns></returns>
        public static DataTable GetCollateralPriceDateWise(DateTime dateSelected, int dateMethodology, bool getSameDayClosedDataOnDV, bool isOnlyFixedIncomeSymbols)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _markDataManager.GetCollateralPriceDateWise(dateSelected, dateMethodology, getSameDayClosedDataOnDV, isOnlyFixedIncomeSymbols);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }
    }
}