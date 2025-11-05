using Prana.BusinessLogic.Symbol;
using Prana.BusinessLogic.SymbolUtilities.BAL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
//using Prana.InstanceCreator;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using Prana.FeedSubscriber;
using System.ServiceModel;

namespace Prana.SecurityMasterNew
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class SecMasterSyncCacheManager : ISecMasterSyncServices
    {
        #region singleton instance
        static SecMasterSyncCacheManager _secMasterSyncCacheManager = null;

        static SecMasterSyncCacheManager()
        {
            _secMasterSyncCacheManager = SecMasterSyncCacheManager.GetInstance();
            if (_secMasterSyncCacheManager == null)
            {
                _secMasterSyncCacheManager = new SecMasterSyncCacheManager();
            }
        }

        private static ISecMasterServices _secMasterServices;

        private const string Const_forexIdentificatonString = "CURNCY";

        public static ISecMasterServices SecMasterServices
        {
            get { return _secMasterServices; }
            set { _secMasterServices = value; }
        }

        public static SecMasterSyncCacheManager GetInstance()
        {
            return _secMasterSyncCacheManager;
        }
        #endregion

        /// <summary>
        /// Return dictionary with the requested symbol and ticker symbol
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSecMasterData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            Dictionary<string, string> dictSymbols = new Dictionary<string, string>();
            try
            {
                dictSymbols = GetSymbolDictForListSync(symbolList, symbologyCode);
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
            return dictSymbols;
        }

        ///<inheritdoc/>
        public Dictionary<string, string> GetTickersBySymbolCurrency(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            Dictionary<string, string> requestedSMData = new Dictionary<string, string>();
            try
            {

                if (_secMasterServices != null && symbolList != null)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                    secMasterRequestObj.IsSearchInLocalOnly = true;
                    symbolList.ForEach(x =>
                    {
                        var symbol = x.IndexOf(Seperators.SEPERATOR_5) > 0 ? x.Substring(0, x.IndexOf(Seperators.SEPERATOR_5)) : x;
                        secMasterRequestObj.AddData(symbol, symbologyCode);
                    });

                    List<SecMasterBaseObj> foundData = _secMasterServices.GetSecMasterDataForListSync(secMasterRequestObj, 0);
                    // Dictionary<String, SecMasterBaseObj> secMasterFrmCacheDataDict = SecMasterDataCache.GetInstance.GetSecMasterDataDict(symbologyCode);
                    foreach (SecMasterBaseObj secMasterBaseObj in foundData)
                    {

                        var symbol = secMasterBaseObj.TickerSymbol;
                        switch (symbologyCode)
                        {
                            case ApplicationConstants.SymbologyCodes.TickerSymbol:
                                symbol = secMasterBaseObj.TickerSymbol;
                                break;

                            case ApplicationConstants.SymbologyCodes.ISINSymbol:
                                symbol = secMasterBaseObj.ISINSymbol;
                                break;

                            case ApplicationConstants.SymbologyCodes.SEDOLSymbol:
                                symbol = secMasterBaseObj.SedolSymbol;
                                break;

                            case ApplicationConstants.SymbologyCodes.CUSIPSymbol:
                                symbol = secMasterBaseObj.CusipSymbol;
                                break;
                        }
                        var currency = CachedDataManager.GetInstance.GetCurrencyText(secMasterBaseObj.CurrencyID);
                        var key = symbol + Seperators.SEPERATOR_5 + currency;
                        if (!requestedSMData.ContainsKey(key))
                        {
                            requestedSMData.Add(key, secMasterBaseObj.TickerSymbol);
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
            return requestedSMData;
        }

        /// <summary>
        ///  Get dictionary of requested symbol and ticker symbol
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <param name="senderCode"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSymbolDictForListSync(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode)      //, int senderCode)
        {

            Dictionary<string, string> requestedSMData = new Dictionary<string, string>();
            try
            {
                if (_secMasterServices != null)
                {
                    List<SecMasterBaseObj> foundData = _secMasterServices.GetSecMasterDataForListSync(symbolList, symbologyCode, 0);
                    // Dictionary<String, SecMasterBaseObj> secMasterFrmCacheDataDict = SecMasterDataCache.GetInstance.GetSecMasterDataDict(symbologyCode);
                    foreach (SecMasterBaseObj secMasterBaseObj in foundData)
                    {
                        switch (symbologyCode)
                        {
                            case ApplicationConstants.SymbologyCodes.TickerSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.TickerSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.TickerSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.ReutersSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.ReutersSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.ReutersSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.ISINSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.ISINSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.ISINSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.SEDOLSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.SedolSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.SedolSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.CUSIPSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.CusipSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.CusipSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.BloombergSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.BloombergSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.OSIOptionSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.OSIOptionSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.OSIOptionSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.IDCOOptionSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.IDCOOptionSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            case ApplicationConstants.SymbologyCodes.OPRAOptionSymbol:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.OpraSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.OpraSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
                            default:
                                if (!requestedSMData.ContainsKey(secMasterBaseObj.TickerSymbol))
                                {
                                    requestedSMData.Add(secMasterBaseObj.TickerSymbol, secMasterBaseObj.TickerSymbol);
                                }
                                break;
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
            return requestedSMData;
        }

        public void Initlise(ISecMasterServices secMasterServices)
        {
            try
            {
                _secMasterServices = secMasterServices;
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
        /// Getting Dynamic UDA Data
        /// </summary>
        /// <returns></returns>
        public SerializableDictionary<string, DynamicUDA> GetDynamicUDAList()
        {
            try
            {
                SerializableDictionary<string, DynamicUDA> dynamicUDA = new SerializableDictionary<string, DynamicUDA>();
                if (_secMasterServices != null)
                {
                    dynamicUDA = _secMasterServices.GetDynamicUDAList();
                }
                return dynamicUDA;
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
                return null;
            }
        }

        /// <summary>
        /// Saving Dynamic UDA Data
        /// </summary>
        /// <returns></returns>
        public bool SaveDynamicUDA(DynamicUDA dynamicUda, string renamedKeys)
        {
            try
            {
                bool saved = _secMasterServices.SaveDynamicUDA(dynamicUda, renamedKeys);
                return saved;
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
                return false;
            }
        }

        /// <summary>
        /// Return dictionary with the requested symbol and ticker symbol
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <returns></returns>
        public Dictionary<string, SecMasterBaseObj> GetSecMasterSymbolData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            Dictionary<string, SecMasterBaseObj> dictSymbols = new Dictionary<string, SecMasterBaseObj>();

            try
            {
                if (_secMasterServices != null)
                {
                    List<SecMasterBaseObj> foundData = _secMasterServices.GetSecMasterDataForListSync(symbolList, symbologyCode, 0);

                    foreach (SecMasterBaseObj secMasterBaseObj in foundData)
                    {
                        if (symbologyCode.Equals(ApplicationConstants.SymbologyCodes.TickerSymbol))
                        {
                            if (!dictSymbols.ContainsKey(secMasterBaseObj.TickerSymbol))
                            {
                                dictSymbols.Add(secMasterBaseObj.TickerSymbol, secMasterBaseObj);
                            }
                        }
                        else if (symbologyCode.Equals(ApplicationConstants.SymbologyCodes.BloombergSymbol))
                        {
                            if (!dictSymbols.ContainsKey(secMasterBaseObj.BloombergSymbol))
                            {
                                dictSymbols.Add(secMasterBaseObj.BloombergSymbol, secMasterBaseObj);
                            }
                        }
                        else if (symbologyCode.Equals(ApplicationConstants.SymbologyCodes.OSIOptionSymbol))
                        {
                            if (!dictSymbols.ContainsKey(secMasterBaseObj.OSIOptionSymbol))
                            {
                                dictSymbols.Add(secMasterBaseObj.OSIOptionSymbol, secMasterBaseObj);
                            }
                        }
                        else if (symbologyCode.Equals(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol))
                        {
                            if (!dictSymbols.ContainsKey(secMasterBaseObj.IDCOOptionSymbol))
                            {
                                dictSymbols.Add(secMasterBaseObj.IDCOOptionSymbol, secMasterBaseObj);
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
            return dictSymbols;
        }

        /// <summary>
        /// To check master value is used
        /// </summary>
        /// <returns></returns>
        public bool CheckMasterValueAssigned(string tag, string value)
        {
            try
            {
                return _secMasterServices.CheckMasterValueAssigned(tag, value);
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
                return false;
            }
        }

        public int GetCurrencyIdForSymbol(string symbol)
        {
            try
            {
                if (_secMasterServices != null)
                {
                    SecMasterBaseObj secMasterBaseObject = _secMasterServices.GetSecMasterDataForSymbol(symbol);
                    if (secMasterBaseObject != null)
                    {
                        return secMasterBaseObject.CurrencyID;
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
            return 0;
        }
        /// <summary>
        /// provides search response for given symbol from secmaster
        /// </summary>
        /// <returns></returns>
        public SecMasterSymbolSearchRes SearchSymbols(SecMasterSymbolSearchReq request)
        {
            SecMasterSymbolSearchRes symbolListResponse = null;
            try
            {
                if (_secMasterServices != null)
                {
                    symbolListResponse = _secMasterServices.ReqSymbolSearch(request);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return symbolListResponse;
        }

        public MarketDataSymbolResponse GetMarketDataSymbolFromTickerSymbol(string tickerSymbol)
        {
            try
            {
                string symbol = tickerSymbol;
                SecMasterBaseObj secMasterBaseObject = null;
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI && tickerSymbol.Contains(Const_forexIdentificatonString))
                {
                    tickerSymbol = tickerSymbol.Substring(0, 3) + "/" + tickerSymbol.Substring(3, 3);
                }
                if (_secMasterServices != null)
                {
                    secMasterBaseObject = _secMasterServices.GetSecMasterDataForSymbol(tickerSymbol);
                    if (secMasterBaseObject != null)
                    {
                        MarketDataSymbolResponse marketDataSymbolResponse = new MarketDataSymbolResponse();
                        marketDataSymbolResponse.TickerSymbol = symbol;
                        marketDataSymbolResponse.AssetCategory = secMasterBaseObject.AssetCategory;
                        marketDataSymbolResponse.AUECID = secMasterBaseObject.AUECID;

                        if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet)
                        {
                            if (!string.IsNullOrWhiteSpace(secMasterBaseObject.FactSetSymbol))
                            {
                                marketDataSymbolResponse.FactSetSymbol = secMasterBaseObject.FactSetSymbol;
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FactSet symbol not present in database for Ticker Symbol: " + tickerSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
                        }
                        else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV)
                        {
                            if (!string.IsNullOrWhiteSpace(secMasterBaseObject.ActivSymbol))
                            {
                                marketDataSymbolResponse.ActivSymbol = secMasterBaseObject.ActivSymbol;
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ACTIV symbol not present in database for Ticker Symbol: " + tickerSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
                        }
                        else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                        {
                            if (!string.IsNullOrWhiteSpace(secMasterBaseObject.BloombergSymbol))
                            {
                                marketDataSymbolResponse.BloombergSymbol = secMasterBaseObject.BloombergSymbol;
                            }
                            else if (!string.IsNullOrWhiteSpace(secMasterBaseObject.BloombergSymbolWithExchangeCode))
                            {
                                marketDataSymbolResponse.BloombergSymbol = secMasterBaseObject.BloombergSymbolWithExchangeCode;
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Bloomberg symbol not present in database for Ticker Symbol: " + tickerSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                            }
                        }

                        return marketDataSymbolResponse;
                    }
                }

                if (secMasterBaseObject == null)
                {
                    //use case for new symbol in which ticker not exists in cache/database or secmasterservice not connected - never execute this workflow, just code written for safe side.
                    MarketDataSymbolResponse marketDataSymbolResponseReq = new MarketDataSymbolResponse();
                    marketDataSymbolResponseReq.TickerSymbol = tickerSymbol;
                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataSymbolGenerator.GetMarketDataSymbolFromTickerSymbol(marketDataSymbolResponseReq, CachedDataManager.CompanyMarketDataProvider, SecMasterDataCache.GetInstance.GetFutSymbolRootdata(marketDataSymbolResponseReq.TickerSymbol));
                    marketDataSymbolResponse.TickerSymbol = symbol;
                    if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.FactSetSymbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FactSet symbol generated from AUEC mapping for Ticker Symbol: " + tickerSymbol + ", FactSet Symbol: " + marketDataSymbolResponse.FactSetSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                    else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.ActivSymbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ACTIV symbol generated from AUEC mapping for Ticker Symbol: " + tickerSymbol + ", ACTIV Symbol: " + marketDataSymbolResponse.ActivSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                    else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.BloombergSymbol))
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Bloomberg symbol generated from AUEC mapping for Ticker Symbol: " + tickerSymbol + ", Bloomberg Symbol: " + marketDataSymbolResponse.BloombergSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        public MarketDataSymbolResponse GetTickerSymbolFromMarketData(SymbolData marketData)
        {
            try
            {
                if (marketData.MarketDataProvider == MarketDataProvider.SAPI && marketData.CategoryCode == AssetCategory.Future && !string.IsNullOrWhiteSpace(marketData.BloombergSymbol))
                {
                    string[] symbolPart = marketData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string bbgRoot = symbolPart[0].Substring(0, symbolPart[0].Length - 3);
                    return TickerSymbolGenerator.GetTickerSymbolFromMarketData(marketData, SecMasterDataCache.GetInstance.GetFutureRootByBBGRoot(bbgRoot));
                }
                else
                    return TickerSymbolGenerator.GetTickerSymbolFromMarketData(marketData);
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
        /// <summary>
        /// Return Asset category
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public AssetCategory GetAssetCategoryForBloombergSymbology(string symbol)
        {
            try
            {
                return BloombergAssetCategory.GetAssetCategoryUsingRegex(symbol);
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
            return AssetCategory.None;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
