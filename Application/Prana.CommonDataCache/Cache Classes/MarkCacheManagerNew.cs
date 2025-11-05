using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CommonDatabaseAccess;
using Prana.Global.Utilities;
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
    public static class MarkCacheManagerNew
    {
        private static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> _auecwiseBussinessAdjustedYesterdayMarkPriceDict = new Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>>();
        private static Dictionary<int, DateTime> _auecWiseBusinessAdjustedYesterdayDate;
        private static readonly object _publishLock = new object();
        private static readonly object _locker = new object();
        private static ProxyBase<IPublishing> _proxyPublishing = null;
        private static SortedDictionary<string, List<MarkPriceInfo>> _fxForwardMarkPrices = null;
        private static Dictionary<string, DateTime> _symbolYesterdayDate = new Dictionary<string, DateTime>();
        private static IMarkDataManager _markDataManager;

        private static Dictionary<string, double> _todaysSplitFactorForSymbol;

        public static Dictionary<string, double> TodaysSplitFactorForSymbol
        {
            get { return _todaysSplitFactorForSymbol; }
            set { _todaysSplitFactorForSymbol = value; }
        }

        private static Dictionary<int, Dictionary<string, MarkPriceInfo>> _latestMarkPrices;

        public static Dictionary<int, Dictionary<string, MarkPriceInfo>> LatesMarkPrices
        {
            get { lock (_locker) { return _latestMarkPrices; } }
        }

        public static Dictionary<int, Dictionary<string, MarkPriceInfo>> GetLatestMarkPriceDic()
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

        static MarkCacheManagerNew()
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

        private static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceCacheFromTable(DateTime date, DataTable dtMarkPrices)
        {
            string symbol = string.Empty;
            Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> markPriceDict = null;

            try
            {
                if (dtMarkPrices == null)
                {
                    return null;
                }
                else
                {
                    markPriceDict = new Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>>();
                }

                foreach (DataRow dr in dtMarkPrices.Rows)
                {
                    MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);

                    if (markPriceInfo != null)
                    {
                        int accountId = markPriceInfo.AccountID;
                        symbol = markPriceInfo.Symbol;
                        if (!String.IsNullOrEmpty(symbol))
                        {
                            if (markPriceDict.ContainsKey(date))
                            {
                                if (markPriceDict[date].ContainsKey(accountId))
                                {
                                    Dictionary<string, MarkPriceInfo> symbolMarkPriceInfo = markPriceDict[date][accountId];
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
                                    markPriceDict[date].Add(accountId, symbolMarkPriceInfoNew);
                                }
                            }
                            else
                            {
                                Dictionary<string, MarkPriceInfo> symbolMarkPriceInfoNew = new Dictionary<string, MarkPriceInfo>();
                                symbolMarkPriceInfoNew.Add(symbol, markPriceInfo);

                                Dictionary<int, Dictionary<string, MarkPriceInfo>> accountSymbolMarkPriceInfoNew = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();

                                accountSymbolMarkPriceInfoNew.Add(accountId, symbolMarkPriceInfoNew);
                                if (date == DateTimeConstants.MinValue)
                                {
                                    date = markPriceInfo.DateActual;
                                }

                                markPriceDict.Add(date, accountSymbolMarkPriceInfoNew);
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

        private static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceCacheFromTableForMonth(DataTable dtMarkPrices)
        {
            string symbol = string.Empty;
            Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> markPriceDict = null;
            try
            {
                if (dtMarkPrices == null)
                {
                    return null;
                }
                markPriceDict = new Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>>();

                foreach (DataRow dr in dtMarkPrices.Rows)
                {
                    MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);
                    int accountId = markPriceInfo.AccountID;
                    //commented by - omshiv symbol should be initialise after null check
                    if (markPriceInfo != null && !String.IsNullOrEmpty(markPriceInfo.Symbol))
                    {
                        symbol = markPriceInfo.Symbol;
                        if (markPriceDict.ContainsKey(markPriceInfo.DateActual))
                        {
                            if (markPriceDict[markPriceInfo.DateActual].ContainsKey(accountId))
                            {
                                Dictionary<string, MarkPriceInfo> symbolMarkPriceInfo = markPriceDict[markPriceInfo.DateActual][accountId];
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

                                Dictionary<int, Dictionary<string, MarkPriceInfo>> accountSymbolMarkPriceInfoNew = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();
                                markPriceDict[markPriceInfo.DateActual].Add(accountId, symbolMarkPriceInfoNew);
                            }
                        }
                        else
                        {
                            Dictionary<string, MarkPriceInfo> symbolMarkPriceInfoNew = new Dictionary<string, MarkPriceInfo>();
                            symbolMarkPriceInfoNew.Add(symbol, markPriceInfo);

                            Dictionary<int, Dictionary<string, MarkPriceInfo>> accountSymbolMarkPriceInfoNew = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();
                            accountSymbolMarkPriceInfoNew.Add(accountId, symbolMarkPriceInfoNew);
                            markPriceDict.Add(markPriceInfo.DateActual, accountSymbolMarkPriceInfoNew);
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
                //Dictionary with all the auecids and their respective dates derived from the auecstring
                //sent from EPNL.
                Dictionary<int, DateTime> auecWiseDateDict = TimeZoneHelper.GetInstance().GetAUECDateDictfromAUECString(auecString);

                //Get distinct dates from the above dictionary with its values as list of auecids.
                Dictionary<DateTime, List<int>> distinctDateAUECList = GetDistinctDateWithAUECList(auecWiseDateDict);

                lock (_locker)
                {
                    if (_latestMarkPrices == null)
                        _latestMarkPrices = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();
                    _auecwiseBussinessAdjustedYesterdayMarkPriceDict = new Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>>();

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
                        foreach (int accountId in _latestMarkPrices.Keys)
                        {
                            Dictionary<string, MarkPriceInfo> accountLatestMarkPrices = _latestMarkPrices[accountId];
                            foreach (MarkPriceInfo markPriceObj in accountLatestMarkPrices.Values)
                            {
                                markPriceObj.SplitFactor = 1;
                            }
                        }
                    }
                }

                _todaysSplitFactorForSymbol = _markDataManager.GetSplitFactors(auecString);
                if (_todaysSplitFactorForSymbol != null)
                    foreach (string symbol in _todaysSplitFactorForSymbol.Keys)
                    {
                        foreach (int accountId in _latestMarkPrices.Keys)
                        {
                            Dictionary<string, MarkPriceInfo> accountLatestMarkPrices = _latestMarkPrices[accountId];
                            if (_latestMarkPrices != null && accountLatestMarkPrices.ContainsKey(symbol) && _todaysSplitFactorForSymbol[symbol] != 0)
                                lock (_locker)
                                    accountLatestMarkPrices[symbol].SplitFactor = _todaysSplitFactorForSymbol[symbol];
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

        private static void FillBusinessAdjustedDayMarkPricesForDate(DateTime requestDate, Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> dicToFill)
        {
            try
            {
                lock (_locker)
                {
                    DataTable dtMarkPrices = _markDataManager.GetBusinessAdjustedDayMarkPriceForGivenDateCH(requestDate.Date);
                    if (dtMarkPrices == null)
                    {
                        return;
                    }
                    if (dtMarkPrices.Rows.Count == 0)
                    {
                        return;
                    }
                    Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> dateMarkPrice = GetMarkPriceCacheFromTable(requestDate.Date, dtMarkPrices);

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

        /// <summary>
        /// Here when expnl is asking data for yesterday mark price, then we assign the yesterday dates for each symbol. Then further
        /// Pricing service refers this and fetches yesterday mark for particular symbol.
        /// </summary>
        /// <param name="auecWiseDateDict"></param>
        /// <param name="auecs"></param>
        /// <param name="inputMarkPriceDict"></param>
        /// <param name="outputMarkPriceDict"></param>
        private static void GetAUECDateMarkPrices(Dictionary<int, DateTime> auecWiseDateDict, List<int> auecs, Dictionary<int, Dictionary<string, MarkPriceInfo>> inputMarkPriceDict, ref Dictionary<int, Dictionary<string, MarkPriceInfo>> outputMarkPriceDict)
        {
            try
            {
                lock (_locker)
                {
                    foreach (int accountId in inputMarkPriceDict.Keys)
                    {
                        Dictionary<string, MarkPriceInfo> accountWiseInputMarkPriceDict = inputMarkPriceDict[accountId];
                        foreach (KeyValuePair<string, MarkPriceInfo> symbolMarkKeyValue in accountWiseInputMarkPriceDict)
                        {
                            int auecID = symbolMarkKeyValue.Value.AUECID;
                            string symbol = symbolMarkKeyValue.Key;

                            if (auecs.Contains(auecID))
                            {
                                symbolMarkKeyValue.Value.YesterdayBusinessAdjustedDate = auecWiseDateDict[auecID];
                                if (outputMarkPriceDict.ContainsKey(accountId))
                                {
                                    if (!outputMarkPriceDict[accountId].ContainsKey(symbol))
                                    {
                                        outputMarkPriceDict[accountId].Add(symbol, symbolMarkKeyValue.Value);
                                        //RahulGupta:20130125: This is to fill the fx/forward mark px cache which we are separately maintaining for recalculating closing marks if
                                        //underlying spot rates are changes from dailyvaluation...
                                        UpdateFxForwardsMarkPxCache(symbolMarkKeyValue.Value);
                                    }
                                    else
                                    {
                                        outputMarkPriceDict[accountId][symbol] = symbolMarkKeyValue.Value;
                                    }
                                }
                                else
                                {
                                    Dictionary<string, MarkPriceInfo> markPriceInfoDict = new Dictionary<string, MarkPriceInfo>();
                                    markPriceInfoDict.Add(symbol, symbolMarkKeyValue.Value);
                                    outputMarkPriceDict.Add(accountId, markPriceInfoDict);
                                }

                                //further no need will be of this
                                FillSymbolAndYesterdayDate(symbol, auecID, auecWiseDateDict);
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

                    #endregion TO maintain Auec Wise BusinessAdjustedYesterdayDate
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
        public static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceForDate(DateTime date, int dateMethodology, bool isFxFXForwardData, bool getSameDayClosedDataOnDV)
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

        private static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetBusinessAdjustedDayMarkPriceForDate(DateTime date)
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
                    foreach (int accountId in _latestMarkPrices.Keys)
                    {
                        Dictionary<string, MarkPriceInfo> accountLatestMarkPrices = _latestMarkPrices[accountId];
                        if (_latestMarkPrices != null && accountLatestMarkPrices.ContainsKey(kvp.Key))
                        {
                            //date actual is the date for which taxlot have not zero mark price
                            //if cash settlement date and actual date are equal, mark price of date actual would be cash settlement price
                            if (accountLatestMarkPrices[kvp.Key].DateActual.Date.Equals(kvp.Value.Date))
                            {
                                dictSymbolWithMarkPrice.Add(kvp.Key, accountLatestMarkPrices[kvp.Key].MarkPrice);
                            }
                            else
                            {
                                //if cash settlement date and actual date are not actual than cash settlement price would be zero
                                dictSymbolWithMarkPrice.Add(kvp.Key, 0);
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
            return dictSymbolWithMarkPrice;
        }

        public static double GetMarkPriceForDateAndSymbol(DateTime date, string symbol, int accountId)
        {
            double markPrice = 0;

            try
            {
                if (_latestMarkPrices != null)
                {
                    if (_latestMarkPrices.ContainsKey(accountId) && _latestMarkPrices[accountId][symbol].YesterdayBusinessAdjustedDate == date.Date)
                    {
                        return _latestMarkPrices[accountId][symbol].MarkPrice;
                    }
                    else
                    {
                        Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> _fetchedData = GetMarkPriceForDate(date, 0, false, false);
                        if (_fetchedData != null && _fetchedData.ContainsKey(date) && _fetchedData[date].ContainsKey(accountId) && _fetchedData[date][accountId].ContainsKey(symbol))
                            return _fetchedData[date][accountId][symbol].MarkPrice;
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
            return markPrice;
        }

        /// <summary>
        /// It ask for the yesterday mark price for any symbol. The yesterday date is the same as provided by expnl using clearance logic
        /// </summary>
        /// <param name="date"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static double GetMarkPriceForSymbol(string symbol, int accountId)
        {
            try
            {
                if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(accountId) && _latestMarkPrices[accountId].ContainsKey(symbol))
                {
                    return _latestMarkPrices[accountId][symbol].MarkPrice;
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

        /// <summary>
        /// It ask for the yesterday mark price for any account + symbol. The yesterday date is the same as provided by expnl using clearance logic
        /// </summary>
        /// <param name="dictAccountSymbolCollection">dictionary of account->symbol+markprice</param>
        public static void GetMarkPriceForAccountSymbolCollection(ref Dictionary<int, Dictionary<string, double>> dictAccountSymbolCollection)
        {
            try
            {
                Dictionary<int, Dictionary<string, double>> dictAccountSymbolCollectionClone = DeepCopyHelper.Clone(dictAccountSymbolCollection); ;

                foreach (KeyValuePair<int, Dictionary<string, double>> accountSymbolItem in dictAccountSymbolCollectionClone)
                {
                    if (_latestMarkPrices != null)
                    {
                        int accountID = 0;
                        //if accountid available in cache then pick data for that accountid otherwise pick data for 0 accountid
                        if (_latestMarkPrices.ContainsKey(accountSymbolItem.Key))
                            accountID = accountSymbolItem.Key;
                        foreach (KeyValuePair<string, double> symbolItem in accountSymbolItem.Value)
                        {
                            if (_latestMarkPrices.ContainsKey(accountID) && _latestMarkPrices[accountID].ContainsKey(symbolItem.Key))
                            {
                                dictAccountSymbolCollection[accountSymbolItem.Key][symbolItem.Key] = _latestMarkPrices[accountID][symbolItem.Key].MarkPrice;
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
        }

        public static double GetForwardPoints(string symbol, int accountId)
        {
            try
            {
                if (_latestMarkPrices != null && _latestMarkPrices.ContainsKey(accountId) && _latestMarkPrices[accountId].ContainsKey(symbol))
                {
                    return _latestMarkPrices[accountId][symbol].MarkPrice;
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

        /// <summary>
        /// Get the Request Field data from DB
        /// </summary>
        public static DataSet GetSAPIRequestFieldData(string requestField)
        {
            try
            {
                return _markDataManager.GetSAPIRequestFieldData(requestField);
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
        /// Save the Request Field Data in DB
        /// </summary>
        public static void SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField)
        {
            try
            {
                _markDataManager.SaveSAPIRequestFieldData(saveDataSetTemp, requestField);
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

        public static int SaveMarkPrices(DataTable dt, bool isAutoApproved)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                // Save into db
                rowsAffected = _markDataManager.SaveMarkPrices(dt, isAutoApproved);

                //Commnented by omshiv, now upgrade cache only after approve changes, not on saving unApproved changes
                if (isAutoApproved)
                {
                    WaitCallback callBackHandler = new WaitCallback(UpdateMarkCacheAndPublishDataCH);
                    ThreadPool.QueueUserWorkItem(callBackHandler, dt);
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
            return rowsAffected;
        }

        private static void UpdateMarkCacheAndPublishDataCH(object dataTable)
        {
            try
            {
                DataTable dt = dataTable as DataTable;

                // Update cache
                UpdateBusinessAdjustedDayMarkPriceCacheNew(dt);
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
                    Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> latestFetchedData = null;
                    foreach (DataRow dr in dtMarkPrices.Rows)
                    {
                        if (!String.IsNullOrEmpty(dr["Date"].ToString()) && !String.IsNullOrEmpty(dr["Symbol"].ToString()))
                        {
                            MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);
                            date = markPriceInfo.DateActual.Date;
                            int accountId = markPriceInfo.AccountID;

                            dr["DateActual"] = date;
                            symbol = markPriceInfo.Symbol;
                            double markPrice = markPriceInfo.MarkPrice;
                            double forwardPoints = markPriceInfo.ForwardPoints;
                            auecID = markPriceInfo.AUECID;
                            int assetId = CachedDataManager.GetInstance.GetAssetIdByAUECId(auecID);

                            #region If markprice is updated for a symbol which doesnot exist in cache
                            if (_latestMarkPrices == null)
                                _latestMarkPrices = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();

                            if (_latestMarkPrices.ContainsKey(accountId))
                            {
                                Dictionary<string, MarkPriceInfo> accountLatestMarkPrices = _latestMarkPrices[accountId];
                                if (!accountLatestMarkPrices.ContainsKey(symbol))
                                {
                                    if (auecID != int.MinValue && _auecWiseBusinessAdjustedYesterdayDate != null && _auecWiseBusinessAdjustedYesterdayDate.ContainsKey(auecID))
                                    {
                                        markPriceInfo.YesterdayBusinessAdjustedDate = _auecWiseBusinessAdjustedYesterdayDate[auecID];
                                        if (!accountLatestMarkPrices.ContainsKey(symbol))
                                            accountLatestMarkPrices.Add(symbol, markPriceInfo);

                                        if (assetId != int.MinValue && (assetId == (int)(AssetCategory.FXForward) || assetId == (int)(AssetCategory.FX)))
                                        {
                                            int leadCurrency = 0;
                                            int vsCurrency = 0;

                                            markPriceInfo.LeadCurrencyID = leadCurrency;
                                            markPriceInfo.VsCurrencyID = vsCurrency;
                                            markPriceInfo.AssetID = assetId;
                                            UpdateFxForwardsMarkPxCache(markPriceInfo);
                                        }
                                    }
                                }
                                else
                                {
                                    //Added check if mark price does not exist in data base then it must be updated by mark price provided
                                    //for any date > DateTime.MinValue
                                    if (accountLatestMarkPrices[symbol].MarkPrice == 0)
                                        accountLatestMarkPrices[symbol].DateActual = DateTime.MinValue;

                                    if (accountLatestMarkPrices[symbol].DateActual <= date && accountLatestMarkPrices[symbol].YesterdayBusinessAdjustedDate >= date)
                                    {
                                        if (BusinessDayCalculator.GetInstance().IsBusinessDayForAUEC(date, accountLatestMarkPrices[symbol].AUECID))
                                        {
                                            //if user make mark price 0 for the actualdate mark price
                                            if (accountLatestMarkPrices[symbol].DateActual == date && markPrice == 0)
                                            {
                                                if (latestFetchedData == null || (latestFetchedData != null && !latestFetchedData.ContainsKey(date)))
                                                    latestFetchedData = GetBusinessAdjustedDayMarkPriceForDate(date);
                                                if (latestFetchedData != null && latestFetchedData.ContainsKey(date) && latestFetchedData[date][accountId].ContainsKey(symbol))
                                                {
                                                    accountLatestMarkPrices[symbol].DateActual = latestFetchedData[date][accountId][symbol].DateActual;
                                                    accountLatestMarkPrices[symbol].MarkPrice = latestFetchedData[date][accountId][symbol].MarkPrice;
                                                    accountLatestMarkPrices[symbol].ForwardPoints = latestFetchedData[date][accountId][symbol].ForwardPoints;
                                                    //If No Mark Price Is Saved In Db Then dateActual Sholud be minimum
                                                    if (latestFetchedData[date][accountId][symbol].MarkPrice == 0)
                                                        latestFetchedData[date][accountId][symbol].DateActual = DateTime.MinValue;
                                                    dr["MarkPrice"] = latestFetchedData[date][accountId][symbol].MarkPrice;
                                                    dr["DateActual"] = latestFetchedData[date][accountId][symbol].DateActual;
                                                    if (dtMarkPrices.Columns.Contains("ForwardPoints"))
                                                    {
                                                        dr["ForwardPoints"] = latestFetchedData[date][accountId][symbol].ForwardPoints;
                                                    }
                                                }
                                            }
                                            else if (markPrice != 0)
                                            {
                                                accountLatestMarkPrices[symbol].DateActual = date;
                                                accountLatestMarkPrices[symbol].MarkPrice = markPrice;
                                                accountLatestMarkPrices[symbol].ForwardPoints = forwardPoints;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Dictionary<string, MarkPriceInfo> accountLatestMarkPrices = new Dictionary<string, MarkPriceInfo>();

                                if (auecID != int.MinValue && _auecWiseBusinessAdjustedYesterdayDate != null && _auecWiseBusinessAdjustedYesterdayDate.ContainsKey(auecID))
                                {
                                    markPriceInfo.YesterdayBusinessAdjustedDate = _auecWiseBusinessAdjustedYesterdayDate[auecID];
                                    if (!accountLatestMarkPrices.ContainsKey(symbol))
                                        accountLatestMarkPrices.Add(symbol, markPriceInfo);

                                    if (assetId != int.MinValue && (assetId == (int)(AssetCategory.FXForward) || assetId == (int)(AssetCategory.FX)))
                                    {
                                        int leadCurrency = 0;
                                        int vsCurrency = 0;

                                        markPriceInfo.LeadCurrencyID = leadCurrency;
                                        markPriceInfo.VsCurrencyID = vsCurrency;
                                        markPriceInfo.AssetID = assetId;
                                        UpdateFxForwardsMarkPxCache(markPriceInfo);
                                    }
                                    _latestMarkPrices.Add(accountId, accountLatestMarkPrices);
                                }
                            }
                            #endregion
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
                        int SourceID = int.Parse(dr["SourceID"].ToString());
                        PricingSource pricingSourceEnum = (PricingSource)Enum.ToObject(typeof(PricingSource), SourceID);
                        markPriceInfo.PricingSource = pricingSourceEnum.ToString();
                    }
                    else
                    {
                        markPriceInfo.PricingSource = PricingSource.None.ToString();
                    }

                    if (dr.Table.Columns.Contains("FundName") && dr["FundName"] != DBNull.Value)
                    {
                        markPriceInfo.AccountName = dr["FundName"].ToString();
                    }
                    if (dr.Table.Columns.Contains("ISINSymbol") && dr["ISINSymbol"] != DBNull.Value)
                    {
                        markPriceInfo.ISINSymbol = dr["ISINSymbol"].ToString();
                    }

                    if (dr.Table.Columns.Contains("CUSIPSymbol") && dr["CUSIPSymbol"] != DBNull.Value)
                    {
                        markPriceInfo.CUSIPSymbol = dr["CUSIPSymbol"].ToString();
                    }
                    if (dr.Table.Columns.Contains("Currency") && dr["Currency"] != DBNull.Value)
                    {
                        markPriceInfo.Currency = dr["Currency"].ToString();
                    }
                    if (dr.Table.Columns.Contains("ExpirationDate"))
                    {
                        if (dr["ExpirationDate"] == DBNull.Value)
                            markPriceInfo.ExpirationDate = DateTimeConstants.MinValue;
                        else
                            markPriceInfo.ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"]).Date;
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

        public static int SaveBeta(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                rowsAffected = _markDataManager.SaveBeta(dt);

                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_Beta;
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

        public static int SaveOutStandings(DataTable dt)
        {
            int rowsAffected = 0;
            if (dt == null)
            {
                return rowsAffected;
            }
            try
            {
                rowsAffected = _markDataManager.SaveOutStandings(dt);

                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_OutStandings;
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
                            CentralizePublish(msg);
                        }
                    }

                    //Publish Forex Rate
                    MessageData e = new MessageData();
                    e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                    e.TopicName = Topics.Topic_ForexRate;
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

        public static int SaveStandardCurrencyPair(DataTable dtCurrencyPair)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = _markDataManager.SaveStandardCurrencyPair(dtCurrencyPair);
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

        public static int SaveForexRateWithAccount(DataTable dt)
        {
            int rowsAffected = 0;
            try
            {
                //Save Fx rates into database.
                rowsAffected = _markDataManager.SaveForexRateWithAccount(dt);

                // TODO : Need to handle account wise fx rate in cash management

                #region commented to handle account wise fx rate in cash management and update cache in forex converter
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
                            //_proxyPublishing.InnerChannel.Publish(msg, Topics.Topic_MarkPrice);
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
                #endregion commented to handle account wise fx rate in cash management and update cache in forex converter
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
                // Save into db
                rowsAffected = _markDataManager.SaveCollateralValues(dt);
                MessageData e = new MessageData();
                e.EventData = DataTableToListConverter.GetListFromDataTable(dt);
                e.TopicName = Topics.Topic_DailyCollateral;
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

        public static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceForDateRange(string xmlAccounts, DateTime startDate, DateTime endDate, int dateMethodology, bool isFxFxForward, int filter)
        {
            try
            {
                lock (_locker)
                {
                    DataTable dtMarkPrices = _markDataManager.GetMarkPricesForGivenDateRange(xmlAccounts, startDate.Date, endDate.Date, dateMethodology, isFxFxForward, filter);
                    if (dateMethodology == 0)
                    {
                        return GetMarkPriceCacheFromTable(startDate.Date, dtMarkPrices);
                    }
                    else if (dateMethodology == 2)
                    {
                        return GetMarkPriceCacheFromTableForMonth(dtMarkPrices);
                    }
                    else if (dateMethodology == 3)
                    {
                        // Modified by Bhavana for fetching data for custom date range at JIRA: CHMW-714
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

        public static int ApproveMarkPrices(DataTable dtMarkPrice)
        {
            int i = 0;
            try
            {
                if (dtMarkPrice != null && dtMarkPrice.Rows.Count > 0)
                {
                    DataSet ds = new DataSet("dsMarkPrice");
                    DataTable dt = dtMarkPrice.Copy();
                    dt.TableName = "dtMarkPrice";
                    ds.Tables.Add(dt);
                    String xmlMarkPrice = ds.GetXml();
                    DataSet newMarkPriceData = _markDataManager.ApproveMarkPricesinDB(xmlMarkPrice);

                    if (newMarkPriceData != null && newMarkPriceData.Tables.Count > 0)
                    {
                        WaitCallback callBackHandler = new WaitCallback(UpdateMarkCacheAndPublishDataCH);
                        ThreadPool.QueueUserWorkItem(callBackHandler, newMarkPriceData.Tables[0]);
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
            return i;
        }

        public static int RescindMarkPrices(DataTable dtMarkPrice)
        {
            int i = 0;
            try
            {
                if (dtMarkPrice != null && dtMarkPrice.Rows.Count > 0)
                {
                    DataSet ds = new DataSet("dsMarkPrice");
                    DataTable dt = dtMarkPrice.Copy();
                    dt.TableName = "dtMarkPrice";
                    ds.Tables.Add(dt);
                    String xmlMarkPrice = ds.GetXml();
                    i = _markDataManager.RescindMarkPricesinDB(xmlMarkPrice);
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
            return i;
        }

        /// <summary>
        /// added by: Bharat Raturi, 20 may 2014
        /// Get unapproved mark prices from the database
        /// </summary>
        /// <returns>Datatable holding the mark prices</returns>
        public static DataTable GetUnapprovedMarkPrices(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _markDataManager.GetUnapprovedMarkPricesFromDB(startDate, endDate);
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

        private static Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceCacheFromTableForDateRange(DataTable dtMarkPrices)
        {
            string symbol = string.Empty;

            Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> markPriceDict = null;
            try
            {
                if (dtMarkPrices == null)
                {
                    return null;
                }
                markPriceDict = new Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>>();

                foreach (DataRow dr in dtMarkPrices.Rows)
                {
                    MarkPriceInfo markPriceInfo = GetMarkPriceInfoFromRow(dr);
                    int accountId = markPriceInfo.AccountID;
                    if (markPriceInfo != null && !String.IsNullOrEmpty(markPriceInfo.Symbol))
                    {
                        symbol = markPriceInfo.Symbol;
                        if (markPriceDict.ContainsKey(markPriceInfo.DateActual))
                        {
                            if (markPriceDict[markPriceInfo.DateActual].ContainsKey(accountId))
                            {
                                Dictionary<string, MarkPriceInfo> symbolMarkPriceInfo = markPriceDict[markPriceInfo.DateActual][accountId];
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

                                Dictionary<int, Dictionary<string, MarkPriceInfo>> accountSymbolMarkPriceInfoNew = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();
                                markPriceDict[markPriceInfo.DateActual].Add(accountId, symbolMarkPriceInfoNew);
                            }
                        }
                        else
                        {
                            Dictionary<string, MarkPriceInfo> symbolMarkPriceInfoNew = new Dictionary<string, MarkPriceInfo>();
                            symbolMarkPriceInfoNew.Add(symbol, markPriceInfo);

                            Dictionary<int, Dictionary<string, MarkPriceInfo>> accountSymbolMarkPriceInfoNew = new Dictionary<int, Dictionary<string, MarkPriceInfo>>();
                            accountSymbolMarkPriceInfoNew.Add(accountId, symbolMarkPriceInfoNew);
                            markPriceDict.Add(markPriceInfo.DateActual, accountSymbolMarkPriceInfoNew);
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
    }
}