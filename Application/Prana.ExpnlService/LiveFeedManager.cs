using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Linq;

namespace Prana.ExpnlService
{
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// 
    /// Thread safe implementation of LiveFeedManager. This class facilitates the fetching of LiveFeed prices from the PricingService2 not connected. The call to 
    /// PricingService2 not connected has been limited to one per symbol per cycle. So if there are multiple taxlots for the same symbol, then the LiveFeed data
    /// is fetched from the local cache. This local cache is cleared when the calculation cycle starts.
    /// </summary>
    internal sealed class LiveFeedManager : IDisposable
    {
        private static volatile LiveFeedManager _instance;
        private static object _syncRoot = new Object();
        private ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _fileCacheLock = new ReaderWriterLockSlim();
        private bool _isFilePricing = false;

        private DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        private static Dictionary<string, SymbolData> _liveFeedCache = new Dictionary<string, SymbolData>();
        private static Dictionary<string, SymbolData> _liveFeedCacheTouch = new Dictionary<string, SymbolData>();
        private static HashSet<string> _bulkRequestedSymbols = new HashSet<string>();
        private static HashSet<string> _bulkRequestedSymbolsTouch = new HashSet<string>();
        public EventHandler<EventArgs<List<SymbolData>>> LiveFeedReceived = null;
        /// <summary>
        /// A cache to store the adviced symbol list from esper
        /// [Note : Using the SymbolData class as we need to store variables like currencies and asset id]
        /// </summary>
        private Dictionary<String, AdviceSymbolInfo> _allSymbols = new Dictionary<String, AdviceSymbolInfo>();
        /// <summary>
        /// Stores Symbol data for each cycle that needs to be sent to the Compliance.
        /// </summary>
        private static Dictionary<string, SymbolData> _dictSymbolData = new Dictionary<string, SymbolData>();

        private LiveFeedManager()
        {

        }

        internal static LiveFeedManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new LiveFeedManager();
                            _liveFeedCache = new Dictionary<string, SymbolData>();
                        }
                    }
                }

                return _instance;
            }
        }

        internal DuplexProxyBase<IPricingService> PricingServiceProxy
        {
            set
            {
                _pricingServiceProxy = value;
            }
        }

        public Dictionary<String, SymbolData> GetLiveFeedCacheClone()
        {
            Dictionary<String, SymbolData> liveFeedCacheLocal = null;
            _cacheLock.EnterReadLock();
            try
            {
                liveFeedCacheLocal = DeepCopyHelper.Clone<Dictionary<String, SymbolData>>(_liveFeedCache);
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
            finally
            {
                _cacheLock.ExitReadLock();
            }
            return liveFeedCacheLocal;
        }

        /// <summary>
        /// This function is called on every refresh interval, so that new cache could be created after every refresh interval.
        /// </summary>
        internal void ClearLiveFeedCache(bool isTouchData)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _fileCacheLock.EnterWriteLock();
                try
                {
                    _liveFeedCache.Clear();
                    _bulkRequestedSymbols.Clear();
                    _liveFeedCacheTouch.Clear();
                    _bulkRequestedSymbolsTouch.Clear();
                    _isFilePricing = isTouchData;
                    if (LiveFeedReceived != null && _dictSymbolData.Count > 0)
                    {
                        LiveFeedReceived(this, new EventArgs<List<SymbolData>>(_dictSymbolData.Values.ToList()));
                    }
                    _dictSymbolData.Clear();

                }
                finally
                {
                    _fileCacheLock.ExitWriteLock();
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
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the file symbol data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        private SymbolData GetTouchSymbolData(string symbol)
        {
            SymbolData data = null;
            try
            {
                _fileCacheLock.EnterReadLock();
                try
                {
                    if (_bulkRequestedSymbolsTouch.Contains(symbol) || symbol.StartsWith("$"))
                    {
                        _liveFeedCacheTouch.TryGetValue(symbol, out data);
                    }
                }
                finally
                {
                    _fileCacheLock.ExitReadLock();
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
            return data;
        }

        /// <summary>
        /// Fetches the data from either local cache or from pricing service. The local cache is refreshed after every calculation cycle.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="isIncludeFilePricing">if set to <c>true</c> [is include file pricing].</param>
        /// <returns></returns>
        internal SymbolData GetDynamicSymbolData(string symbol, bool isIncludeFilePricing = true)
        {
            SymbolData data = null;
            try
            {
                if (_isFilePricing && isIncludeFilePricing)
                {
                    return GetTouchSymbolData(symbol);
                }

                _cacheLock.EnterWriteLock();
                try
                {
                    if (_bulkRequestedSymbols.Contains(symbol))
                    {
                        _liveFeedCache.TryGetValue(symbol, out data);
                        if (!_dictSymbolData.ContainsKey(symbol) && data != null)
                        {
                            _dictSymbolData.Add(symbol, data);
                        }
                        return data;
                    }
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }

                if (_pricingServiceProxy.IsContainerServiceConnected())
                {
                    try
                    {
                        data = _pricingServiceProxy.InnerChannel.GetDynamicSymbolData(symbol);
                    }
                    catch
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                }

                _cacheLock.EnterWriteLock();
                try
                {
                    _bulkRequestedSymbols.Add(symbol);
                    if (data != null)
                    {
                        _liveFeedCache[symbol] = data;
                    }
                    if (!_dictSymbolData.ContainsKey(symbol) && data != null)
                    {
                        _dictSymbolData.Add(symbol, data);
                    }
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
            }
            // To pass the faulted or aborted state exception of Channel, so that it doesn't bother further Orders.
            catch (CommunicationException)
            {
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
            return data;
        }

        /// <summary>
        /// Overload made to handle live feed request for fx/forward symbols as fx esginal symbol is diff from pranaSymbol.
        ///  
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal SymbolData GetDynamicSymbolData(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode, bool skipFilePricing = false)
        {
            SymbolData data = null;
            try
            {
                if (!skipFilePricing && _isFilePricing)
                    return GetTouchSymbolData(symbol);
                _cacheLock.EnterWriteLock();
                try
                {
                    if (_bulkRequestedSymbols.Contains(symbol))
                    {
                        _liveFeedCache.TryGetValue(symbol, out data);
                        if (!_dictSymbolData.ContainsKey(symbol) && data != null)
                        {
                            _dictSymbolData.Add(symbol, data);
                        }
                        return data;
                    }
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }

                if (_pricingServiceProxy.IsContainerServiceConnected())
                {
                    try
                    {
                        data = _pricingServiceProxy.InnerChannel.GetDynamicSymbolData(symbol, fromCurrency, toCurrency, categoryCode);
                    }
                    catch
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                }

                if (data != null)
                {
                    if (data.SelectedFeedPrice == 0 && (data.PricingSource == PricingSource.ClosingMark || data.PricingSource == PricingSource.StaleClosingMark || data.PricingSource == PricingSource.None || data.PricingSource == PricingSource.EODT_1Snapshot))
                    {
                        ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(fromCurrency, toCurrency, DateTime.Now, 0);

                        if (conversionRate != null)
                        {
                            if (conversionRate.ConversionMethod == Operator.M)
                            {
                                data.SelectedFeedPrice = conversionRate.RateValue;
                            }
                            else if (conversionRate.RateValue != 0 && conversionRate.ConversionMethod == Operator.D)
                            {
                                data.SelectedFeedPrice = (1 / conversionRate.RateValue);
                            }
                        }
                    }
                }

                _cacheLock.EnterWriteLock();
                try
                {
                    _bulkRequestedSymbols.Add(symbol);
                    if (data != null)
                    {
                        //Kuldeep A.: http://jira.nirvanasolutions.com:8080/browse/PRANA-4379
                        _liveFeedCache[symbol] = data;
                    }

                    if (!_dictSymbolData.ContainsKey(symbol) && data != null)
                    {
                        _dictSymbolData.Add(symbol, data);
                    }
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
            }
            // To pass the faulted or aborted state exception of Channel, so that it doesn't bother further Orders.
            catch (CommunicationException)
            {

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
            return data;
        }

        public void AdviseSymbol(string symbol)
        {
            try
            {
                _pricingServiceProxy.InnerChannel.CheckAndAdviseSymbol(symbol);
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

        public void AdviseSymbolBulk(List<string> symbols)
        {
            try
            {
                _pricingServiceProxy.InnerChannel.CheckAndAdviseSymbolBulk(symbols);
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

        public void AdviseSymbolForFX(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode)
        {
            try
            {
                _pricingServiceProxy.InnerChannel.CheckAndAdviseSymbolForFX(symbol, fromCurrency, toCurrency, categoryCode);
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
        /// Add the symbol to the advie cache
        /// </summary>
        /// <param name="symbol"></param>
        internal void AddToAdviceCache(AdviceSymbolInfo symbol)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                if (!_allSymbols.ContainsKey(symbol.Symbol))
                    _allSymbols.Add(symbol.Symbol, symbol);
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
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Send the intrade symbol data
        /// </summary>
        internal void SendBlotterPrices()
        {
            try
            {
                List<AdviceSymbolInfo> unFoundSymbols = new List<AdviceSymbolInfo>();
                _cacheLock.EnterReadLock();
                try
                {
                    foreach (String symbol in _allSymbols.Keys)
                    {
                        if (!_liveFeedCache.ContainsKey(symbol))
                        {
                            unFoundSymbols.Add(_allSymbols[symbol]);
                        }
                    }
                }
                finally
                {
                    _cacheLock.ExitReadLock();
                }
                foreach (AdviceSymbolInfo symbolData in unFoundSymbols)
                {
                    if (symbolData.AssetId == AssetCategory.FXForward || symbolData.AssetId == AssetCategory.FX)
                        GetDynamicSymbolData(symbolData.Symbol, symbolData.FromCurrencyId, symbolData.ToCurrencyId, symbolData.AssetId);
                    else
                        GetDynamicSymbolData(symbolData.Symbol);
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
        /// Gets the live feed for symbol list.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <returns></returns>
        internal void GetLiveFeedForSymbolList(Dictionary<string, AdviceSymbolInfo> symbols)
        {
            if (_isFilePricing)
            {
                _fileCacheLock.EnterWriteLock();
                try
                {
                    if (_pricingServiceProxy.IsContainerServiceConnected())
                    {
                        Dictionary<string, SymbolData> temp = _pricingServiceProxy.InnerChannel.GetLiveFeedForSymbolListTouch(symbols);
                        if (temp != null)
                            _liveFeedCacheTouch = temp;
                    }
                    _bulkRequestedSymbolsTouch = new HashSet<string>(symbols.Keys);
                }
                catch
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                finally
                {
                    _fileCacheLock.ExitWriteLock();
                }
            }
            else
            {
                _cacheLock.EnterWriteLock();
                try
                {
                    if (_pricingServiceProxy.IsContainerServiceConnected())
                    {
                        Dictionary<string, SymbolData> temp = _pricingServiceProxy.InnerChannel.GetLiveFeedForSymbolList(symbols);
                        if (temp != null)
                            _liveFeedCache = temp;
                    }
                    _bulkRequestedSymbols = new HashSet<string>(symbols.Keys);
                }
                catch
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_pricingServiceProxy != null)
                _pricingServiceProxy.Dispose();
            _fileCacheLock.Dispose();
            _cacheLock.Dispose();
        }
    }
}
