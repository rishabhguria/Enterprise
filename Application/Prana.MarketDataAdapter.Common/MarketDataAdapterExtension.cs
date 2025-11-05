using Microsoft.SqlServer.Server;
using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionCalculator.Common;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Prana.MarketDataAdapter.Common
{
    public static class MarketDataAdapterExtension
    {
        private static Dictionary<string, SymbolData> _snapShotSymbolDict = new Dictionary<string, SymbolData>();
        private static Dictionary<string, string> _dictSymbolCurrencies = new Dictionary<string, string>();

        private static Dictionary<string, string> _dictAssetWiseFieldsSnapshot = new Dictionary<string, string>();
        private static Dictionary<string, string> _dictAssetWiseFieldsSubcription = new Dictionary<string, string>();
        private static object _locker = new object();
        private static ConcurrentDictionary<string, string> _dictTickerSymbolAndMarketDataSymbolMapping = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, MarketDataSymbolResponse> _dictMarketSymbolWiseMarketDataSymbolResponse = new ConcurrentDictionary<string, MarketDataSymbolResponse>();
        private const string Const_BBGMnemonic = "BBGMnemonic";
        private const string Const_NirvanaFields = "NirvanaFields";
        private const string Const_Snapshot = "Snapshot";
        private const string Const_Subscription = "Subscription";
        private static ProxyBase<ISecMasterSyncServices> _secMasterServices = null;
        public static ProxyBase<ISecMasterSyncServices> SecMasterServices
        {
            set { _secMasterServices = value; }
        }

        public static Dictionary<string, string> DictAssetWiseFieldsSubcription
        {
            get { return _dictAssetWiseFieldsSubcription; }
        }

        public static Dictionary<string, string> DictAssetWiseFieldsSnapshot
        {
            get { return _dictAssetWiseFieldsSnapshot; }
        }

        public static SymbolData GetSnapShotSymbolData(string symbol)
        {
            SymbolData snapShotData = null;
            try
            {
                lock (_snapShotSymbolDict)
                {
                    _snapShotSymbolDict.TryGetValue(symbol, out snapShotData);
                }

                if (snapShotData != null)
                {
                    snapShotData = (SymbolData)DeepCopyHelper.Clone(snapShotData);
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
            return snapShotData;
        }

        public static void AddToSnapShotSymbolDataCollection(ref SymbolData snapShotData, MarketDataProvider provider)
        {
            try
            {
                if (snapShotData != null)
                {
                    lock (_snapShotSymbolDict)
                    {
                        switch (provider)
                        {
                            case MarketDataProvider.FactSet:
                                _snapShotSymbolDict[snapShotData.FactSetSymbol] = (SymbolData)snapShotData.Clone();
                                break;
                            case MarketDataProvider.ACTIV:
                                _snapShotSymbolDict[snapShotData.ActivSymbol] = (SymbolData)snapShotData.Clone();
                                break;
                            case MarketDataProvider.SAPI:
                                _snapShotSymbolDict[snapShotData.BloombergSymbol] = (SymbolData)snapShotData.Clone();
                                _snapShotSymbolDict[snapShotData.BloombergSymbolWithExchangeCode] = (SymbolData)snapShotData.Clone();
                                break;
                            default:
                                _snapShotSymbolDict[snapShotData.Symbol] = (SymbolData)snapShotData.Clone();
                                break;
                        }
                    }

                    if (provider == MarketDataProvider.Esignal)
                    {
                        if (snapShotData.UpdateTime == DateTime.MinValue || snapShotData.UpdateTime.Year == 0001 || snapShotData.UpdateTime.Year == 1800)
                        {
                            snapShotData.UpdateTime = DateTime.Now.ToUniversalTime();
                        }

                        if (string.IsNullOrEmpty(snapShotData.CurencyCode))
                        {
                            if (_dictSymbolCurrencies.ContainsKey(snapShotData.Symbol))
                            {
                                snapShotData.CurencyCode = _dictSymbolCurrencies[snapShotData.Symbol];
                                string logMessage = "Currency for the Symbol=" + snapShotData.Symbol + " is not coming from Esignal So refilling currency = " + snapShotData.CurencyCode + " from SymbolLookup (Cache).";
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(logMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
                            else
                            {
                                int currencyID = 0;
                                currencyID = OptionModelUserInputCache.GetcurrencyIdforSymbol(snapShotData.Symbol);
                                if (currencyID != 0)
                                {
                                    snapShotData.CurencyCode = CachedDataManager.GetInstance.GetCurrencyText(currencyID);
                                    if (!_dictSymbolCurrencies.ContainsKey(snapShotData.Symbol))
                                        _dictSymbolCurrencies.Add(snapShotData.Symbol, CachedDataManager.GetInstance.GetCurrencyText(currencyID));
                                    string logMessage = "Currency for the Symbol=" + snapShotData.Symbol + " is not coming from Esignal So refilling currency = " + snapShotData.CurencyCode + " from SymbolLookup.";
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(logMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                }
                                else
                                {
                                    snapShotData.CurencyCode = "USD";
                                    if (snapShotData.CategoryCode != AssetCategory.Indices)
                                    {
                                        Logger.HandleException(new Exception("symbol : " + snapShotData.Symbol + " is processed with Currency USD. Because currency is not coming from the Esignal and also not available in the symbollookup for this symbol. "), LoggingConstants.POLICY_LOGANDSHOW);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static int GetCountryIdFromFactsetCode(string code)
        {
            try
            {
                return CachedDataManager.GetInstance.GetCountryIDFromFactsetCountryCode(code);
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
            return -1;
        }

        public static int GetCountryIdFromBloombergCode(string code)
        {
            try
            {
                return CachedDataManager.GetInstance.GetCountryIDFromBloombergCountryCode(code);
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
            return -1;
        }

        public static void ClearCache()
        {
            lock (_snapShotSymbolDict)
            {
                _snapShotSymbolDict.Clear();
            }
        }

        public static List<SymbolData> GetAvailableLiveFeed()
        {
            List<SymbolData> list = null;
            try
            {
                lock (_snapShotSymbolDict)
                {
                    list = new List<SymbolData>(_snapShotSymbolDict.Values);
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
            return list;
        }

        public static void CreateSecMasterServicesProxy()
        {
            try
            {
                if (_secMasterServices == null)
                    _secMasterServices = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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

        public static MarketDataSymbolResponse GetMarketDataSymbolInformationFromTickerSymbol(string tickerSymbol)
        {
            try
            {
                lock (_locker)
                {
                    if (_dictTickerSymbolAndMarketDataSymbolMapping.ContainsKey(tickerSymbol))
                    {
                        string marketSymbol = _dictTickerSymbolAndMarketDataSymbolMapping[tickerSymbol];
                        if (_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketSymbol))
                            return _dictMarketSymbolWiseMarketDataSymbolResponse[marketSymbol];
                    }
                    if (_secMasterServices != null && _secMasterServices.InnerChannel != null && _secMasterServices.IsContainerServiceConnected())
                    {
                        MarketDataSymbolResponse marketDataSymbolResponse = _secMasterServices.InnerChannel.GetMarketDataSymbolFromTickerSymbol(tickerSymbol);

                        if (marketDataSymbolResponse != null)
                        {
                            switch (CachedDataManager.CompanyMarketDataProvider)
                            {
                                case MarketDataProvider.FactSet:
                                    if (string.IsNullOrWhiteSpace(marketDataSymbolResponse.FactSetSymbol))
                                        marketDataSymbolResponse.FactSetSymbol = tickerSymbol;

                                    if (!_dictTickerSymbolAndMarketDataSymbolMapping.ContainsKey(tickerSymbol))
                                        _dictTickerSymbolAndMarketDataSymbolMapping.TryAdd(tickerSymbol, marketDataSymbolResponse.FactSetSymbol);
                                    if (!_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbolResponse.FactSetSymbol))
                                        _dictMarketSymbolWiseMarketDataSymbolResponse.TryAdd(marketDataSymbolResponse.FactSetSymbol, marketDataSymbolResponse);
                                    break;

                                case MarketDataProvider.ACTIV:
                                    if (string.IsNullOrWhiteSpace(marketDataSymbolResponse.ActivSymbol))
                                        marketDataSymbolResponse.ActivSymbol = tickerSymbol;

                                    if (!_dictTickerSymbolAndMarketDataSymbolMapping.ContainsKey(tickerSymbol))
                                        _dictTickerSymbolAndMarketDataSymbolMapping.TryAdd(tickerSymbol, marketDataSymbolResponse.ActivSymbol);
                                    if (!_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbolResponse.ActivSymbol))
                                        _dictMarketSymbolWiseMarketDataSymbolResponse.TryAdd(marketDataSymbolResponse.ActivSymbol, marketDataSymbolResponse);
                                    break;

                                case MarketDataProvider.SAPI:
                                    if (string.IsNullOrWhiteSpace(marketDataSymbolResponse.BloombergSymbol))
                                        marketDataSymbolResponse.BloombergSymbol = tickerSymbol;
                                    if (!_dictTickerSymbolAndMarketDataSymbolMapping.ContainsKey(tickerSymbol))
                                        _dictTickerSymbolAndMarketDataSymbolMapping.TryAdd(tickerSymbol, marketDataSymbolResponse.BloombergSymbol);
                                    if (!_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbolResponse.BloombergSymbol))
                                        _dictMarketSymbolWiseMarketDataSymbolResponse.TryAdd(marketDataSymbolResponse.BloombergSymbol, marketDataSymbolResponse);
                                    break;
                            }

                            return marketDataSymbolResponse;
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("TradeService is not connected. Unable to fetch MarketDataSymbol response from SecMasterServices for symbol: {0}.", tickerSymbol), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
            return null;
        }

        public static MarketDataSymbolResponse GetTickerSymbolFromMarketDataSymbolUsingCache(string marketDataSymbol)
        {
            try
            {
                lock (_locker)
                {
                    MarketDataSymbolResponse marketDataSymbolResponse = null;

                    if (_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbol))
                        marketDataSymbolResponse = _dictMarketSymbolWiseMarketDataSymbolResponse[marketDataSymbol];

                    if (marketDataSymbolResponse == null)
                    {
                        if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet)
                            LogAndDisplayOnInformationReporter.GetInstance.Write("Unable to retrive Ticker symbol from FactSet symbol: " + marketDataSymbol, LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
                        else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV)
                            LogAndDisplayOnInformationReporter.GetInstance.Write("Unable to retrive Ticker symbol from ACTIV symbol: " + marketDataSymbol, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Warning);
                        else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                            LogAndDisplayOnInformationReporter.GetInstance.Write("Unable to retrive Ticker symbol from Bloomberg symbol: " + marketDataSymbol, LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
                    }

                    return marketDataSymbolResponse;
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
            return null;
        }

        public static MarketDataSymbolResponse GetTickerSymbolFromMarketData(SymbolData marketData)
        {
            MarketDataSymbolResponse marketDataSymbolResponse = null;

            try
            {
                if (marketDataSymbolResponse == null)
                {
                    if (_secMasterServices != null && _secMasterServices.InnerChannel != null && _secMasterServices.IsContainerServiceConnected())
                        marketDataSymbolResponse = _secMasterServices.InnerChannel.GetTickerSymbolFromMarketData(marketData);
                    else
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("GetTickerSymbolFromMarketData: TradeService is not connected. Unable to fetch MarketDataSymbol response from SecMasterServices for symbol: {0}.", GetMarketDataSymbol(marketData)), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }

                if (marketDataSymbolResponse == null)
                {
                    if (marketData.MarketDataProvider == MarketDataProvider.FactSet)
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Unable to retrive Ticker symbol from FactSet symbol: " + marketData.FactSetSymbol, LoggingConstants.FACTSET_LOGGING, 1, 1, TraceEventType.Warning);
                    else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV)
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Unable to retrive Ticker symbol from ACTIV symbol: " + marketData.ActivSymbol, LoggingConstants.ACTIV_LOGGING, 1, 1, TraceEventType.Warning);
                    else if (marketData.MarketDataProvider == MarketDataProvider.SAPI)
                        LogAndDisplayOnInformationReporter.GetInstance.Write("Unable to retrive Ticker symbol from Bloomberg symbol: " + marketData.BloombergSymbol, LoggingConstants.SAPI_LOGGING, 1, 1, TraceEventType.Warning);
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
            return marketDataSymbolResponse;
        }

        private static string GetMarketDataSymbol(SymbolData marketData)
        {
            string marketDataSymbol = string.Empty;
            try
            {
                if (marketData.MarketDataProvider == MarketDataProvider.FactSet)
                    marketDataSymbol = marketData.FactSetSymbol;
                else if (marketData.MarketDataProvider == MarketDataProvider.ACTIV)
                    marketDataSymbol = marketData.ActivSymbol;
                else if (marketData.MarketDataProvider == MarketDataProvider.SAPI)
                    marketDataSymbol = marketData.BloombergSymbol;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
            }
            return marketDataSymbol;
        }

        public static void AddMarketDataForTickerSymbolToCache(string tickerSymbol, MarketDataSymbolResponse marketDataSymbolResponse)
        {
            try
            {
                lock (_locker)
                {
                    if (!_dictTickerSymbolAndMarketDataSymbolMapping.ContainsKey(tickerSymbol))
                    {
                        switch (CachedDataManager.CompanyMarketDataProvider)
                        {
                            case MarketDataProvider.FactSet:
                                _dictTickerSymbolAndMarketDataSymbolMapping.TryAdd(tickerSymbol, marketDataSymbolResponse.FactSetSymbol);
                                if (!_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbolResponse.FactSetSymbol))
                                    _dictMarketSymbolWiseMarketDataSymbolResponse.TryAdd(marketDataSymbolResponse.FactSetSymbol, marketDataSymbolResponse);
                                break;

                            case MarketDataProvider.ACTIV:
                                _dictTickerSymbolAndMarketDataSymbolMapping.TryAdd(tickerSymbol, marketDataSymbolResponse.ActivSymbol);
                                if (!_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbolResponse.ActivSymbol))
                                    _dictMarketSymbolWiseMarketDataSymbolResponse.TryAdd(marketDataSymbolResponse.ActivSymbol, marketDataSymbolResponse);
                                break;
                            case MarketDataProvider.SAPI:
                                _dictTickerSymbolAndMarketDataSymbolMapping.TryAdd(tickerSymbol, marketDataSymbolResponse.BloombergSymbol);
                                if (!_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketDataSymbolResponse.BloombergSymbol))
                                    _dictMarketSymbolWiseMarketDataSymbolResponse.TryAdd(marketDataSymbolResponse.BloombergSymbol, marketDataSymbolResponse);
                                else
                                    _dictMarketSymbolWiseMarketDataSymbolResponse[marketDataSymbolResponse.BloombergSymbol] = marketDataSymbolResponse;
                                break;
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

        public static void RemoveMarketDataSymbolInformation(string tickerSymbol)
        {
            try
            {
                lock (_locker)
                {
                    if (_dictTickerSymbolAndMarketDataSymbolMapping.ContainsKey(tickerSymbol))
                    {
                        string marketSymbol = _dictTickerSymbolAndMarketDataSymbolMapping[tickerSymbol];
                        if (_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(marketSymbol))
                            _dictMarketSymbolWiseMarketDataSymbolResponse.TryRemove(marketSymbol, out MarketDataSymbolResponse marketDataSymbolResponseValue);
                        _dictTickerSymbolAndMarketDataSymbolMapping.TryRemove(tickerSymbol, out string value);
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

        public static void RefreshMarketDataSymbolInformation(string tickerSymbol)
        {
            try
            {
                RemoveMarketDataSymbolInformation(tickerSymbol);
                GetMarketDataSymbolInformationFromTickerSymbol(tickerSymbol);
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

        public static Dictionary<string, MarketDataSymbolResponse> GetAllMarketDataSymbolInformation()
        {
            try
            {
                lock (_locker)
                {
                    if (_dictTickerSymbolAndMarketDataSymbolMapping != null)
                    {
                        Dictionary<string, MarketDataSymbolResponse> marketData = new Dictionary<string, MarketDataSymbolResponse>();

                        foreach (KeyValuePair<string, string> tickerSymbolAndMarketSymbol in _dictTickerSymbolAndMarketDataSymbolMapping)
                        {
                            if (_dictMarketSymbolWiseMarketDataSymbolResponse.ContainsKey(tickerSymbolAndMarketSymbol.Value))
                                marketData.Add(tickerSymbolAndMarketSymbol.Key, _dictMarketSymbolWiseMarketDataSymbolResponse[tickerSymbolAndMarketSymbol.Value]);
                        }

                        return marketData;
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
            return null;
        }

        public static string GetExchangeName(int auecID)
        {
            try
            {
                string exchange = CachedDataManager.GetInstance.ExchangeIdentifiers.Where(kvp => kvp.Value == auecID).Select(x => x.Key).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(exchange))
                {
                    return exchange.Split('-')[0];
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
            return string.Empty;
        }

        public static string GetEnumDescription(Enum value)
        {
            return EnumHelper.GetDescription(value);
        }

        public static Dictionary<string, SymbolData> GetSnapshotsSymbolData()
        {
            return _snapShotSymbolDict;
        }

        public static OptionDetail GenerateOptionDataFromMarketDataSymbol(MarketDataSymbolResponse underlyingSymbol, OptionStaticData optionMarketSymbol, out string bbgSymbol)
        {
            try
            {
                OptionDetail optionTickerDetail = new OptionDetail()
                {
                    AssetCategory = underlyingSymbol.AssetCategory,
                    AUECID = underlyingSymbol.AUECID,
                    ExpirationDate = optionMarketSymbol.ExpirationDate,
                    OptionType = optionMarketSymbol.PutOrCall,
                    StrikePrice = optionMarketSymbol.StrikePrice,
                    UnderlyingSymbol = underlyingSymbol.TickerSymbol,
                    Symbology = ApplicationConstants.SymbologyCodes.TickerSymbol
                };
                OptionSymbolGenerator.GetOptionSymbol(optionTickerDetail);

                OptionDetail optionBBGDetail = new OptionDetail()
                {
                    AssetCategory = underlyingSymbol.AssetCategory,
                    AUECID = underlyingSymbol.AUECID,
                    ExpirationDate = optionMarketSymbol.ExpirationDate,
                    OptionType = optionMarketSymbol.PutOrCall,
                    StrikePrice = optionMarketSymbol.StrikePrice,
                    UnderlyingSymbol = underlyingSymbol.TickerSymbol,
                    Symbology = ApplicationConstants.SymbologyCodes.BloombergSymbol
                };
                OptionSymbolGenerator.GetOptionSymbol(optionBBGDetail);
                bbgSymbol = optionBBGDetail.Symbol;
                return optionTickerDetail;
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
                bbgSymbol = string.Empty;
            }
            return null;
        }

        public static void SecurityValidationLogging(string message)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(message, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To pupulate Bloomberg dicitionary.
        /// </summary>
        public static void PopulateBloombergDictionary()
        {
            try
            {
                DataSet ds = MarkCacheManagerNew.GetSAPIRequestFieldData(Const_Snapshot);
                _dictAssetWiseFieldsSnapshot = FormatDictionary(ds);
                ds = MarkCacheManagerNew.GetSAPIRequestFieldData(Const_Subscription);
                _dictAssetWiseFieldsSubcription = FormatDictionary(ds);
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
        /// to update the Bloomberg dicitionary when user updated the fields from PUI.
        /// </summary>
        /// <param name="UpdatedData"></param>
        /// <param name="policy"></param>
        public static void UpdateBloombergDictionary(DataSet UpdatedData, string policy)
        {
            try
            {
                if (policy == Const_Snapshot)
                {
                    _dictAssetWiseFieldsSnapshot = FormatDictionary(UpdatedData);
                }
                else
                {
                    _dictAssetWiseFieldsSubcription = FormatDictionary(UpdatedData);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// to format Dicitionary from DataSet.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Dictionary<string, string> FormatDictionary(DataSet ds)
        {
            Dictionary<string, string> dictNirvanaFields = new Dictionary<string, String>();
            try
            {
                DataTable dictTable = new DataTable();
                if (ds != null)
                {
                    dictTable = ds.Tables[0];
                }
                string passKey = string.Empty;
                foreach (DataColumn column in dictTable.Columns)
                {
                    passKey = string.Empty;
                    string columnName = column.ColumnName.ToString();
                    if (!columnName.Equals(Const_NirvanaFields) && !columnName.Equals(Const_BBGMnemonic))
                    {
                        foreach (DataRow row in dictTable.Rows)
                        {
                            bool value = Convert.ToBoolean(row[columnName]);
                            if (value == true)
                            {
                                passKey = passKey + row[Const_BBGMnemonic] + ",";
                            }
                        }
                        passKey = passKey.TrimEnd(',', ' ');
                        if (!dictNirvanaFields.ContainsKey(columnName))
                        {
                            dictNirvanaFields[columnName] = passKey;
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
            }
            return dictNirvanaFields;
        }

        /// <summary>
        /// Returns Asset Category of the Bloomberg symbol.
        /// </summary>
        /// <param name="bloombergSymbol"></param>
        /// <returns></returns>
        public static AssetCategory GetAssetCategoryBloombergSymbol(string bloombergSymbol)
        {
            try
            {
                if (_secMasterServices != null && _secMasterServices.InnerChannel != null && _secMasterServices.IsContainerServiceConnected())
                {
                    return _secMasterServices.InnerChannel.GetAssetCategoryForBloombergSymbology(bloombergSymbol);
                }
                else
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("GetAssetCategoryBloombergSymbol: TradeService is not connected. Unable to fetch Asset category from SecMasterServices for bloomberg symbol: {0}.", bloombergSymbol), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return AssetCategory.None;
        }
    }
}
