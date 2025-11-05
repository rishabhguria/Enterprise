using Prana.APIAdapter.Models;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.APIAdapter.Sessions
{
    internal class SessionManager : IDisposable
    {
        #region Singleton
        private static readonly Lazy<SessionManager> lazy = new Lazy<SessionManager>(() => new SessionManager());

        /// <summary>
        /// instance of SessionManager
        /// </summary>
        public static SessionManager Instance { get { return lazy.Value; } }

        /// <summary>
        /// Session Manager
        /// </summary>
        private SessionManager()
        {
            try
            {
                _incomingPricesCache = new List<SymbolData>();
                _dictSymbols = new ConcurrentDictionary<string, string>();
                _dictSymbolsReverse = new ConcurrentDictionary<string, string>();
                _dictSymbolDetails = new ConcurrentDictionary<string, SecMasterBaseObj>();
                _dictSymbolsReverseDBRetryCount = new ConcurrentDictionary<string, int>();
                CreateSecMasterServicesProxy();
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

        #endregion

        #region Cache objects

        /// <summary>
        /// Is Debug Enabled
        /// </summary>
        public bool IsDebugEnabled { get; set; }

        /// <summary>
        /// Symbols Base Prices
        /// </summary>
        public Dictionary<string, double> SymbolsBasePrices { get; set; }

        /// <summary>
        /// Percentage Tolerance
        /// </summary>
        public double PercentageTolerance { get; set; }

        /// <summary>
        /// _dict Symbols
        /// </summary>
        private ConcurrentDictionary<string, string> _dictSymbols;

        /// <summary>
        /// _dict Symbols Reverse mapping
        /// </summary>
        private ConcurrentDictionary<string, string> _dictSymbolsReverse;

        /// <summary>
        /// _dict Symbols Reverse DB Retry Count
        /// </summary>
        private ConcurrentDictionary<string, int> _dictSymbolsReverseDBRetryCount;


        private ConcurrentDictionary<string, SecMasterBaseObj> _dictSymbolDetails;

        /// <summary>
        /// Session
        /// </summary>
        public AuthSession Session { get; set; }

        /// <summary>
        /// Authentication HTTP Request Parameters
        /// </summary>
        public HTTPRequestParameters AuthHTTPRequestParameters { get; set; }

        /// <summary>
        /// Price Data HTTP Request Parameters
        /// </summary>
        public HTTPRequestParameters PriceDataHTTPRequestParameters { get; set; }

        /// <summary>
        /// Live Prices Data incoming
        /// </summary>
        List<SymbolData> _incomingPricesCache { get; set; }

        /// <summary>
        /// Live Prices Data outgoing
        /// </summary>
        List<SymbolData> _outgoingPricesCache { get; set; }

        /// <summary>
        /// _lockerObject
        /// </summary>
        object _lockerObject = new object();

        /// <summary>
        /// _outgoingCacheLocker
        /// </summary>
        object _outgoingCacheLocker = new object();
        #endregion



        static ProxyBase<ISecMasterSyncServices> _secMasterServices = null;
        public ProxyBase<ISecMasterSyncServices> SecMasterServices
        {
            set { _secMasterServices = value; }
        }

        /// <summary>
        /// Create Sec Master Services Proxy
        /// </summary>
        private void CreateSecMasterServicesProxy()
        {
            try
            {
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

        /// <summary>
        /// Signaling when prices updated
        /// </summary>
        public EventWaitHandle CacheWait = new EventWaitHandle(true, EventResetMode.AutoReset);

        /// <summary>
        /// Set Incoming Prices Data
        /// </summary>
        /// <param name="cache"></param>
        internal void SetIncomingPricesData(List<SymbolData> cache)
        {
            lock (_lockerObject)
            {
                _incomingPricesCache = cache;
                CacheWait.Set();
            }
        }

        /// <summary>
        /// Get outgoing data
        /// </summary>
        /// <returns></returns>
        internal List<SymbolData> GetOutgoingPricesData()
        {
            lock (_outgoingCacheLocker)
            {
                //Switching Cache
                lock (_lockerObject)
                {
                    if (_incomingPricesCache != null)
                    {

                        _outgoingPricesCache = _incomingPricesCache;

                    }
                    else
                        return null;
                }

                return _outgoingPricesCache;
            }
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_incomingPricesCache != null)
                    _incomingPricesCache = null;

                if (_outgoingPricesCache != null)
                    _outgoingPricesCache = null;

                if (SymbolsBasePrices != null)
                    SymbolsBasePrices = null;

                if (_dictSymbols != null)
                    _dictSymbols = null;

                if (_dictSymbolDetails != null)
                    _dictSymbolDetails = null;

                CacheWait.Dispose();
                GC.SuppressFinalize(this);
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

        /// <summary>
        /// Authentication Column Mapping Config
        /// </summary>
        public AutoMapper.MapperConfiguration AuthenticationColumnMappingConfig { get; set; }

        /// <summary>
        /// Pricing Column Mapping Config
        /// </summary>
        public AutoMapper.MapperConfiguration PricingColumnMappingConfig { get; set; }

        /// <summary>
        /// Update Symbol Ticker Dictionary
        /// </summary>
        /// <param name="dictSymbologyWiseSymbols"></param>
        /// <returns></returns>
        internal async Task<bool> UpdateSymbolTickerDictionary(ConcurrentDictionary<ApplicationConstants.SymbologyCodes, List<string>> dictSymbologyWiseSymbols)
        {
            bool isUpdated = false;
            try
            {

                //proxy is hosted on trade server,if trade server is not started then Innerchannel will be zero.
                if (_secMasterServices == null || _secMasterServices.InnerChannel == null)
                {
                    CreateSecMasterServicesProxy();
                }
                if (_secMasterServices != null && _secMasterServices.InnerChannel != null)
                {
                    await System.Threading.Tasks.Task.Run(() =>
                    {

                        foreach (var item in dictSymbologyWiseSymbols)
                        {
                            try
                            {
                                if (_secMasterServices != null && _secMasterServices.InnerChannel != null)
                                {
                                    if (!item.Key.Equals(ApplicationConstants.SymbologyCodes.OSIOptionSymbol))
                                    {
                                        var symbolList = item.Value.ToList();
                                        Dictionary<string, string> dictNewlyResponsedSymbols = _secMasterServices.InnerChannel.GetTickersBySymbolCurrency(symbolList, item.Key);
                                        foreach (var requestedSymbol in item.Value)
                                        {
                                            AddRequestedSymbolInCache(dictNewlyResponsedSymbols, requestedSymbol);
                                        }
                                    }
                                    else
                                    {
                                        var symbolList = item.Value.ToList();
                                        Dictionary<string, SecMasterBaseObj> dictNewlyResponsedSymbols = _secMasterServices.InnerChannel.GetSecMasterSymbolData(symbolList, item.Key);
                                        foreach (var requestedSymbol in item.Value)
                                        {
                                            AddRequestedSymbolInCache(dictNewlyResponsedSymbols, requestedSymbol);
                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                _secMasterServices = null;
                                // Invoke our policy that is responsible for making sure no secure information
                                // gets out of our layer.
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }

                        }

                        isUpdated = true;
                    });
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
            return isUpdated;
        }

        /// <summary>
        /// Add Requested Symbol In Cache
        /// </summary>
        /// <param name="dictNewlyResponsedSymbols"></param>
        /// <param name="requestedSymbol"></param>
        private void AddRequestedSymbolInCache(Dictionary<string, SecMasterBaseObj> dictNewlyResponsedSymbols, string requestedSymbol)
        {
            try
            {
                if (dictNewlyResponsedSymbols.ContainsKey(requestedSymbol) && dictNewlyResponsedSymbols[requestedSymbol] != null)
                {
                    if (!_dictSymbolsReverse.ContainsKey(requestedSymbol))
                    {
                        _dictSymbolsReverse.TryAdd(requestedSymbol, dictNewlyResponsedSymbols[requestedSymbol].TickerSymbol);
                        _dictSymbolDetails.TryAdd(requestedSymbol, dictNewlyResponsedSymbols[requestedSymbol]);
                        int _;
                        _dictSymbolsReverseDBRetryCount.TryRemove(requestedSymbol, out _);
                    }
                }
                else
                {
                    //request will not send again
                    if (!_dictSymbolsReverseDBRetryCount.ContainsKey(requestedSymbol))
                    {
                        _dictSymbolsReverseDBRetryCount.TryAdd(requestedSymbol, 1);
                    }
                    else
                    {
                        _dictSymbolsReverseDBRetryCount[requestedSymbol]++;
                        if (_dictSymbolsReverseDBRetryCount[requestedSymbol] >= 5)
                        {
                            _dictSymbolsReverse.TryAdd(requestedSymbol, string.Empty);
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


        /// <summary>
        /// Add Requested Symbol In Cache
        /// </summary>
        /// <param name="dictNewlyResponsedSymbols"></param>
        /// <param name="requestedSymbol"></param>
        private void AddRequestedSymbolInCache(Dictionary<string, string> dictNewlyResponsedSymbols, string requestedSymbol)
        {
            try
            {
                if (dictNewlyResponsedSymbols.ContainsKey(requestedSymbol) && !string.IsNullOrWhiteSpace(dictNewlyResponsedSymbols[requestedSymbol]))
                {
                    if (!_dictSymbolsReverse.ContainsKey(requestedSymbol))
                    {
                        _dictSymbolsReverse.TryAdd(requestedSymbol, dictNewlyResponsedSymbols[requestedSymbol]);
                        int _;
                        _dictSymbolsReverseDBRetryCount.TryRemove(requestedSymbol, out _);
                    }
                }
                else
                {
                    //request will not send again
                    if (!_dictSymbolsReverseDBRetryCount.ContainsKey(requestedSymbol))
                    {
                        _dictSymbolsReverseDBRetryCount.TryAdd(requestedSymbol, 1);
                    }
                    else
                    {
                        _dictSymbolsReverseDBRetryCount[requestedSymbol]++;
                        if (_dictSymbolsReverseDBRetryCount[requestedSymbol] >= 5)
                        {
                            _dictSymbolsReverse.TryAdd(requestedSymbol, string.Empty);
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

        /// <summary>
        /// Get Symbol Ticker Dictionary
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, string> GetSymbolTickerDictionary()
        {
            try
            {
                return _dictSymbolsReverse;
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
            return new ConcurrentDictionary<string, string>();
        }

        internal ConcurrentDictionary<string, SecMasterBaseObj> GetSymbolsUnderlyingSymbolDictionary()
        {
            try
            {
                return _dictSymbolDetails;
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
            return new ConcurrentDictionary<string, SecMasterBaseObj>();
        }

        /// <summary>
        /// Update Symbols Cache For New Security
        /// </summary>
        /// <param name="secMasterList"></param>
        internal void UpdateSymbolsCacheForNewSecurity(SecMasterbaseList secMasterList)
        {
            try
            {
                if (secMasterList != null)
                {
                    foreach (SecMasterBaseObj obj in secMasterList)
                    {

                        //Removing the symbol from cache
                        int _;
                        string symbol;
                        if (_dictSymbolsReverseDBRetryCount.ContainsKey(obj.OSIOptionSymbol))
                        {
                            _dictSymbolsReverse.TryRemove(obj.OSIOptionSymbol, out symbol);
                            _dictSymbolsReverseDBRetryCount.TryRemove(obj.OSIOptionSymbol, out _);
                        }

                        if (_dictSymbolsReverseDBRetryCount.ContainsKey(obj.IDCOOptionSymbol))
                        {
                            _dictSymbolsReverse.TryRemove(obj.IDCOOptionSymbol, out symbol);
                            _dictSymbolsReverseDBRetryCount.TryRemove(obj.IDCOOptionSymbol, out _);
                        }

                        if (_dictSymbolsReverseDBRetryCount.ContainsKey(obj.OpraSymbol))
                        {
                            _dictSymbolsReverse.TryRemove(obj.OpraSymbol, out symbol);
                            _dictSymbolsReverseDBRetryCount.TryRemove(obj.OpraSymbol, out _);
                        }


                        //For security other than equityOption, validating symbols with currency
                        var currency = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(obj.CurrencyID);
                        var cusipKey = !string.IsNullOrEmpty(obj.CusipSymbol) ? obj.CusipSymbol + Seperators.SEPERATOR_5 + currency : string.Empty;
                        var isinKey = !string.IsNullOrEmpty(obj.ISINSymbol) ? obj.ISINSymbol + Seperators.SEPERATOR_5 + currency : string.Empty;
                        var sedolKey = !string.IsNullOrEmpty(obj.SedolSymbol) ? obj.SedolSymbol + Seperators.SEPERATOR_5 + currency : string.Empty;


                        if (_dictSymbolsReverseDBRetryCount.ContainsKey(isinKey))
                        {
                            _dictSymbolsReverse.TryRemove(isinKey, out symbol);
                            _dictSymbolsReverseDBRetryCount.TryRemove(isinKey, out _);
                        }

                        if (_dictSymbolsReverseDBRetryCount.ContainsKey(sedolKey))
                        {
                            _dictSymbolsReverse.TryRemove(sedolKey, out symbol);
                            _dictSymbolsReverseDBRetryCount.TryRemove(sedolKey, out _);
                        }
                        if (_dictSymbolsReverseDBRetryCount.ContainsKey(cusipKey))
                        {
                            _dictSymbolsReverse.TryRemove(cusipKey, out symbol);
                            _dictSymbolsReverseDBRetryCount.TryRemove(cusipKey, out _);
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
    }
}
