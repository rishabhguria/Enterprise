using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.MarketDataService.Client;
using Prana.OptionCalculator.Common;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.LiveFeedManager
{
    public class LiveFeedManager : IDisposable
    {
        private int _level1TimerStartDueTime = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_LiveFeed, ConfigurationHelper.CONFIGKEY_LiveFeed_Level1TimerStartDueTime));
        private int _timerInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_LiveFeed, ConfigurationHelper.CONFIGKEY_LiveFeed_TimerInterval));
        private int _level1TimerIntervalMultiple = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_LiveFeed, ConfigurationHelper.CONFIGKEY_LiveFeed_Level1TimerIntervalMultiple));
        private bool _enableMarketDataLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableMarketDataLogging"));
        private bool _isExchangePricesFromFile = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsExchangePricesFromFile"));
        private List<string> _filePriceExchangeList = new List<string>();
        /// <summary>
        /// The dictionary symbols to convert in higher currency
        /// </summary>
        private static Dictionary<string, CurrencyConversions> _dictSymbolsToConvertInHigherCurrency = new Dictionary<string, CurrencyConversions>();

        /// <summary>
        /// The is live prices from file
        /// </summary>
        private bool _isLivePricesFromFile = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsLivePricesFromFile"));

        /// <summary>
        /// The enable esignalcontinuous data logging
        /// </summary>
        private bool _enableMarketData_ContinuousDataLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableMarketData_ContinuousDataLogging"));

        private System.Threading.Timer tmrL1 = null;
        private static int timerCount = 1;
        private ILiveFeedAdapter _liveFeedDataProvider = null;
        private IStore _store = new DictionaryMode();
        private LiveFeedFactory _liveFeedFactoryInstance = LiveFeedFactory.GetInstance();
        ILiveFeedAdapter _secondaryFeedDataProvider = null;
        ILiveFeedAdapter _marketFeedService = null;

        public event EventHandler<EventArgs<bool>> LiveFeedConnected;
        public event EventHandler<EventArgs<bool>> LiveFeedDisconnected;

        private bool _isLiveFeedConnected = false;
        public bool IsLiveFeedConnected
        {
            get
            {
                return _isLiveFeedConnected;
            }
        }

        private static bool _isEsperConnected = false;
        public static bool IsEsperConnected
        {
            get { return _isEsperConnected; }
            set { _isEsperConnected = value; }
        }

        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
        public event EventHandler<Data> OptionSnapshotResponse;

        private static LiveFeedManager _liveFeedManagerInstance = null;

        public event EventHandler<EventArgs<List<SymbolData>>> LiveFeedDataReceived;
        public ILiveFeedAdapter PricingApiServices { get; set; }
        private static object _lockerObject = new object();
        private bool _isDebugEnable;
        private double _pctTolerance = 50;
        private List<string> _listOfWithoutGreeksSnapshotSymbol = new List<string>();
        private static Dictionary<string, SymbolData> _liveFeedCache = null;
        private object _lockOnLiveFeedCache = new object();
        private object _lockOnListOfWithoutGreeksSnapshotSymbol = new object();
        private static List<string> _listOfAdviseSymbols = new List<string>();
        static List<string> _listOfTTDMAdviseSymbols = new List<string>();
        private static List<string> _listFxSymbols = new List<string>();
        private static Dictionary<string, List<string>> _listOfOptionChainAdvisedOptions = new Dictionary<string, List<string>>();
        private static Dictionary<string, List<fxInfo>> _dictFxMapping = new Dictionary<string, List<fxInfo>>();
        private ConcurrentDictionary<string, int> _dictRequestedUnderlyingSymbols = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// IsStartUpPricesSentToEsper is true when initial prices send to Esper engine else false.
        /// </summary>
        private static bool _isStartUpPricesSentToEsper = false;
        public static bool IsStartUpPricesSentToEsper
        {
            get { return _isStartUpPricesSentToEsper; }
            set { _isStartUpPricesSentToEsper = value; }
        }

        /// <summary>
        /// IsEsperStartedCompletely is true when Esper engine started completely else false.
        /// </summary>
        private static bool _isEsperStartedCompletely = false;
        public static bool IsEsperStartedCompletely
        {
            get { return _isEsperStartedCompletely; }
            set { _isEsperStartedCompletely = value; }
        }

        public static LiveFeedManager GetInstance()
        {
            try
            {
                if (_liveFeedManagerInstance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_liveFeedManagerInstance == null)
                        {
                            _liveFeedManagerInstance = new LiveFeedManager();
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (true)
                {
                    throw;
                }
            }
            return _liveFeedManagerInstance;
        }

        private LiveFeedManager()
        {
        }

        public void Initialize()
        {
            try
            {

                _liveFeedCache = new Dictionary<string, SymbolData>();
                tmrL1 = new System.Threading.Timer(new TimerCallback(PeriodicTask));
                tmrL1.Change(_level1TimerStartDueTime, _timerInterval);

                if (CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.API) && PricingApiServices != null)
                {
                    _liveFeedFactoryInstance.PricingApiServiceInstance = PricingApiServices;
                    ProxySymbolHelper.SecurityUpdateRecieved += SecurityMaster_SecurityUpdateRecieved;
                }

                _liveFeedDataProvider = _liveFeedFactoryInstance.LiveFeedProviderInstance();
                _secondaryFeedDataProvider = _liveFeedFactoryInstance.SecondaryProviderInstance();
                if (_isExchangePricesFromFile)
                {
                    string configExchangeString = ConfigurationHelper.Instance.GetAppSettingValueByKey("ExchangesForFilePricing");
                    _filePriceExchangeList = new List<string>(configExchangeString.Split(','));
                }
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                {
                    MarketDataAdapterExtension.PopulateBloombergDictionary();
                }
                _marketFeedService = MarketDataServiceManager.GetInstance();
                Start();
                ConnectLiveFeed();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SecurityMaster_SecurityUpdateRecieved(object sender, ListEventAargs e)
        {
            try
            {
                SecMasterbaseList secMasterList = e.argsObject as SecMasterbaseList;
                if (secMasterList != null)
                {
                    _liveFeedDataProvider.UpdateSecurityDetails(secMasterList);
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

        public void ConnectLiveFeed()
        {
            try
            {
                _liveFeedDataProvider.Connect();
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
        /// wire pricing events
        /// </summary>
        private void Start()
        {
            try
            {

                if (_isLivePricesFromFile)
                {
                    if (_isExchangePricesFromFile)
                    {
                        WireEventsForFilePricing();
                        WireEventsForLiveFeedProvider();
                    }
                    else
                    {
                        if (CachedDataManager.GetInstance.IsFilePricingForTouch())
                        {
                            WireEventsForLiveFeedProvider();
                        }
                        else
                        {
                            WireEventsForFilePricing();
                        }
                    }
                }
                else
                {
                    WireEventsForLiveFeedProvider();
                }

                _liveFeedDataProvider.ContinuousDataResponse += new EventHandler<Data>(Continuous_DataResponse);
                _liveFeedDataProvider.SnapShotDataResponse += new EventHandler<Data>(SnapShot_DataResPonse);
                _marketFeedService.SnapShotDataResponse += new EventHandler<Data>(SnapShot_DataResPonse);
                _liveFeedDataProvider.OptionChainResponse += new EventHandler<EventArgs<string, List<OptionStaticData>>>(OptionChain_Response);
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
        /// Wire Events For File Pricing
        /// </summary>
        private void WireEventsForFilePricing()
        {
            FilePricingCache.Connected += new EventHandler<EventArgs<bool>>(ConnectedFilePricing);
        }

        /// <summary>
        /// Wire Events For Live Feed Provider
        /// </summary>
        private void WireEventsForLiveFeedProvider()
        {
            _liveFeedDataProvider.Connected += new EventHandler<EventArgs<bool>>(Connected);
            _liveFeedDataProvider.Disconnected += new EventHandler<EventArgs<bool>>(Disconnected);
        }

        /// <summary>
        /// Disconnected File Pricing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectedFilePricing(object sender, EventArgs<bool> e)
        {
            try
            {
                _isLiveFeedConnected = e.Value;
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("File Pricing Connected."), LoggingConstants.CATEGORY_GENERAL, 1, 1, TraceEventType.Information);

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
        /// Connected File Pricing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectedFilePricing(object sender, EventArgs<bool> e)
        {
            try
            {
                _isLiveFeedConnected = e.Value;
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("File Pricing Connected."), LoggingConstants.CATEGORY_GENERAL, 1, 1, TraceEventType.Information);

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
        /// Divya:02042013 : If proxy is plugged/unplugged from OMI, then this event is fired to update live feed data accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlugUnplugProxy(ProxyDataEventArgs e)
        {
            try
            {
                //when proxy is unplugged.
                if (!e.UseProxySymbol)
                {
                    lock (_listOfAdviseSymbols)
                    {
                        //If the symbol is in list of advice symbols, it means the symbol is a proxy as well as a traded symbol.
                        //Thus we delete the symbol only if it is only a proxy but not a traded symbol.
                        if (!_listOfAdviseSymbols.Contains(e.ProxySymbol))
                        {
                            DeleteSymbol(e.ProxySymbol);
                        }
                        //when the proxy is unplugged, we have to advice the original symbol.
                        AdviseSymbol(e.Symbol);
                        if (!_listOfAdviseSymbols.Contains(e.Symbol))
                            _listOfAdviseSymbols.Add(e.Symbol);
                    }
                    lock (_lockOnLiveFeedCache)
                    {
                        //remove the proxy data from LiveFeed cache...
                        if (_liveFeedCache.ContainsKey(e.Symbol))
                        {
                            _liveFeedCache.Remove(e.Symbol);
                        }
                    }
                }
                //when proxy is plugged.
                else
                {
                    lock (_listOfAdviseSymbols)
                    {
                        //Delete the response of original symbol
                        if (_listOfAdviseSymbols.Contains(e.Symbol))
                        {
                            DeleteSymbol(e.Symbol);
                        }
                        //Advice the proxy Symbol
                        AdviseSymbol(e.ProxySymbol);
                    }
                    lock (_lockOnLiveFeedCache)
                    {
                        //remove the original symbol data from LiveFeed cache...
                        if (_liveFeedCache.ContainsKey(e.Symbol))
                        {
                            _liveFeedCache.Remove(e.Symbol);
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

        public void Connected(object sender, EventArgs<bool> e)
        {
            try
            {
                _isLiveFeedConnected = e.Value;

                lock (_listOfAdviseSymbols)
                {
                    if (_listOfAdviseSymbols != null)
                    {
                        foreach (string symbol in _listOfAdviseSymbols)
                        {
                            AdviseSymbol(symbol);
                        }
                    }

                    if (_listOfTTDMAdviseSymbols != null)
                    {
                        foreach (string symbol in _listOfTTDMAdviseSymbols)
                        {
                            //call proxy helper to fetch proxy info..
                            string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(symbol);
                            if (proxySymbol.Equals(string.Empty))
                            {
                                proxySymbol = symbol;
                            }
                            if (_listOfAdviseSymbols == null || (_listOfAdviseSymbols != null && !_listOfAdviseSymbols.Contains(proxySymbol)))
                                AdviseSymbol(proxySymbol);
                        }
                    }
                }

                if (LiveFeedConnected != null && e.Value)
                {
                    LiveFeedConnected(this, new EventArgs<bool>(e.Value));
                }

                SendMailForConnentDisconnect(e.Value);
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

        public void Disconnected(object sender, EventArgs<bool> e)
        {
            try
            {
                _isLiveFeedConnected = e.Value;

                if (LiveFeedDisconnected != null && !e.Value)
                {
                    LiveFeedDisconnected(this, new EventArgs<bool>(e.Value));
                }
                SendMailForConnentDisconnect(e.Value);
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

        public List<string> GetAdvicedSymbols()
        {
            try
            {
                HashSet<string> advisedSymbols = new HashSet<string>(_listOfAdviseSymbols.Union(_listOfTTDMAdviseSymbols));

                foreach (List<string> options in _listOfOptionChainAdvisedOptions.Values)
                    foreach (string option in options)
                        if (!advisedSymbols.Contains(option))
                            advisedSymbols.Add(option);

                return advisedSymbols.ToList();
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

            return new List<string>();
        }

        public void CheckAndAdviseSymbol(string symbol, bool advicedFromTT = false)
        {
            try
            {
                if (String.IsNullOrEmpty(symbol))
                {
                    return;
                }

                lock (_listOfAdviseSymbols)
                {
                    string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(symbol);
                    if (string.IsNullOrEmpty(proxySymbol))
                    {
                        proxySymbol = symbol;
                    }
                    if (!_listOfAdviseSymbols.Contains(proxySymbol))
                    {
                        if (!advicedFromTT)
                            _listOfAdviseSymbols.Add(proxySymbol);

                        AdviseSymbol(proxySymbol);
                    }
                    if (advicedFromTT && !_listOfTTDMAdviseSymbols.Contains(symbol))
                        _listOfTTDMAdviseSymbols.Add(symbol);
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

        public void CheckAndAdviseOptionForOptionChain(string underlyingSymbol, string option)
        {
            try
            {
                if (String.IsNullOrEmpty(option))
                {
                    return;
                }

                lock (_listOfOptionChainAdvisedOptions)
                {
                    string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(option);
                    if (string.IsNullOrEmpty(proxySymbol))
                    {
                        proxySymbol = option;
                    }
                    if (!string.IsNullOrEmpty(proxySymbol))
                    {
                        if (_listOfOptionChainAdvisedOptions.ContainsKey(underlyingSymbol))
                        {
                            if (!_listOfOptionChainAdvisedOptions[underlyingSymbol].Contains(proxySymbol))
                            {
                                _listOfOptionChainAdvisedOptions[underlyingSymbol].Add(proxySymbol);
                                AdviseSymbol(proxySymbol);
                            }
                        }
                        else
                        {
                            _listOfOptionChainAdvisedOptions.Add(underlyingSymbol, new List<string>() { proxySymbol });
                            AdviseSymbol(proxySymbol);
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

        public void DeleteAdvisedSymbol(string symbol, bool advicedFromTT = false)
        {
            try
            {
                if (String.IsNullOrEmpty(symbol))
                {
                    return;
                }
                if (_isLiveFeedConnected)
                {
                    lock (_listOfAdviseSymbols)
                    {
                        string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(symbol);
                        if (string.IsNullOrEmpty(proxySymbol))
                        {
                            proxySymbol = symbol;
                        }
                        if (advicedFromTT)
                        {
                            if (_listOfTTDMAdviseSymbols.Contains(symbol))
                                _listOfTTDMAdviseSymbols.Remove(symbol);
                            if (!_listOfAdviseSymbols.Contains(symbol))
                                DeleteSymbol(symbol);
                            if (!_listOfAdviseSymbols.Contains(proxySymbol))
                                DeleteSymbol(proxySymbol);
                        }
                        else if (_listOfAdviseSymbols.Contains(proxySymbol))
                        {
                            _listOfAdviseSymbols.Remove(proxySymbol);
                            DeleteSymbol(proxySymbol);
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

        public void DeleteOptionChainAdvisedOption(string underlyingSymbol, string option)
        {
            try
            {
                if (String.IsNullOrEmpty(option))
                {
                    return;
                }
                if (_isLiveFeedConnected)
                {
                    lock (_listOfOptionChainAdvisedOptions)
                    {
                        string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(option);
                        if (string.IsNullOrEmpty(proxySymbol))
                        {
                            proxySymbol = option;
                        }
                        if (!string.IsNullOrEmpty(proxySymbol) && _listOfOptionChainAdvisedOptions.ContainsKey(underlyingSymbol) && _listOfOptionChainAdvisedOptions[underlyingSymbol].Contains(option))
                        {
                            _listOfOptionChainAdvisedOptions[underlyingSymbol].Remove(proxySymbol);
                            DeleteSymbol(proxySymbol);

                            if (_listOfOptionChainAdvisedOptions[underlyingSymbol].Count == 0)
                                _listOfOptionChainAdvisedOptions.Remove(underlyingSymbol);
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

        public void CheckAndAdviseSymbolForFX(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode)
        {
            try
            {
                if (String.IsNullOrEmpty(symbol))
                {
                    return;
                }
                if (categoryCode.Equals(AssetCategory.FX) || categoryCode.Equals(AssetCategory.FXForward) || categoryCode.Equals(AssetCategory.Forex))
                {
                    //////CHMW-3132	Fund wise fx rate handling for expiration settlement
                    ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(fromCurrency, toCurrency, 0);
                    if (conversionRate != null && !String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                    {
                        UpdateFxSymbolMapping(symbol, conversionRate.FXeSignalSymbol, categoryCode, conversionRate);
                        lock (_listOfAdviseSymbols)
                        {
                            string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(symbol);
                            if (string.IsNullOrEmpty(proxySymbol))
                            {
                                proxySymbol = conversionRate.FXeSignalSymbol;
                            }
                            if (!_listOfAdviseSymbols.Contains(proxySymbol))
                            {
                                _listOfAdviseSymbols.Add(proxySymbol);
                                AdviseSymbol(proxySymbol);
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

        private void UpdateFxSymbolMapping(string pranaSymbol, string FXeSignalSymbol, AssetCategory categoryCode, ConversionRate conversionRate)
        {
            try
            {
                bool isSymbolAdded = false;
                lock (_listFxSymbols)
                {
                    if (!_listFxSymbols.Contains(pranaSymbol))
                    {
                        _listFxSymbols.Add(pranaSymbol);
                    }
                    else
                    {
                        isSymbolAdded = true;
                    }
                }
                lock (_dictFxMapping)
                {
                    if (!_dictFxMapping.ContainsKey(FXeSignalSymbol))
                    {
                        fxInfo fxMappedDataNew = new fxInfo();
                        fxMappedDataNew.CategoryCode = categoryCode;
                        fxMappedDataNew.ConversionRate = conversionRate;
                        fxMappedDataNew.PranaSymbol = pranaSymbol;

                        List<fxInfo> listFXReq = new List<fxInfo>();
                        listFXReq.Add(fxMappedDataNew);
                        _dictFxMapping.Add(FXeSignalSymbol, listFXReq);
                    }
                    else
                    {
                        if (!isSymbolAdded)
                        {
                            fxInfo fxMappedData = new fxInfo();
                            fxMappedData.CategoryCode = categoryCode;
                            fxMappedData.ConversionRate = conversionRate;
                            fxMappedData.PranaSymbol = pranaSymbol;
                            _dictFxMapping[FXeSignalSymbol].Add(fxMappedData);
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
        /// Send Mail For Disconnection
        /// </summary>
        private void SendMailForConnentDisconnect(bool isConnected)
        {
            try
            {
                String emailAlertStartTime = ConfigurationHelper.Instance.GetAppSettingValueByKey("EmailAlertStartTime");
                String emailAlertEndTime = ConfigurationHelper.Instance.GetAppSettingValueByKey("EmailAlertEndTime");

                DateTime currentTime = DateTime.Now;
                DateTime startTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, Convert.ToInt16(emailAlertStartTime.Split(':')[0]), Convert.ToInt16(emailAlertStartTime.Split(':')[1]), 0);
                DateTime endTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, Convert.ToInt16(emailAlertEndTime.Split(':')[0]), Convert.ToInt16(emailAlertEndTime.Split(':')[1]), 0);
                String _recipients = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailRecieverAddress");
                if ((currentTime - startTime).Seconds > 0 && (endTime - currentTime).Seconds > 0 && !string.IsNullOrEmpty(_recipients))
                {
                    String _senderAddress = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderAddress");
                    String _senderName = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderName");
                    String _senderId = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderId");
                    String _password = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailPassword");
                    String _hostName = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailHostName");
                    int _port = Convert.ToInt16(ConfigurationHelper.Instance.GetAppSettingValueByKey("MailPort"));

                    bool _enableSSL = Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableSSL"));
                    string[] recipent = _recipients.Split(';');
                    string subject = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyName"].ToString() + " -Live Feed Alert. Price Provider: " + CachedDataManager.CompanyMarketDataProvider.ToString();

                    string body = string.Empty;
                    if (isConnected)
                        body = "Live Feed Connected. Price Provider: " + CachedDataManager.CompanyMarketDataProvider.ToString() + ", time: " + DateTime.Now;
                    else
                        body = "Live Feed Disconnected. Price Provider: " + CachedDataManager.CompanyMarketDataProvider.ToString() + ", time: " + DateTime.Now;

                    EmailsHelper.MailSend(subject, body, _senderId, _senderName, _password, recipent, _port, _hostName, _enableSSL, true);
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

        // sud: need to remove this
        // added because e signal is not working fine
        // because listed exchange is not coming in Last record type.
        private OptionSymbolData _temp = null;

        private object _lock = new object();
        public void Continuous_DataResponse(object sender, Data e)
        {
            try
            {
                SymbolData data = e.Info;
                DataEnricher _dataEnricher = new DataEnricher();
                lock (_lock)
                {
                    if (data.CategoryCode == AssetCategory.EquityOption)
                    {
                        _temp = data as OptionSymbolData;
                    }
                }
                _dataEnricher.ProcessValue(ref data);
                _store.AddOrUpdateData(data);
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

        private void OptionChain_Response(object sender, EventArgs<string, List<OptionStaticData>> e)
        {
            try
            {
                if (OptionChainResponse != null)
                {
                    OptionChainResponse(sender, new EventArgs<string, List<OptionStaticData>>(e.Value, e.Value2));
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

        public void SnapShot_DataResPonse(object sender, Data e)
        {
            try
            {
                SymbolData data = e.Info;
                if (_enableMarketDataLogging)
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                    {
                        Logger.LoggerWrite("Snapshot Response Received from MarketData for Symbol : " + data.Symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        InformationReporter.GetInstance.Write("Snapshot Response Received from MarketData for Symbol : " + data.Symbol + " Time : " + DateTime.UtcNow);
                    }
                    else
                    {
                        Logger.LoggerWrite("Snapshot Response Received from MarketData for Symbol : " + data.Symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                }
                DataEnricher dataEnricher = new DataEnricher();
                lock (_lock)
                {
                    if (_temp != null)
                    {
                        if (data.CategoryCode == AssetCategory.EquityOption && _temp.Symbol == data.Symbol)
                        {
                            data.UpdateContinuousData(_temp);
                        }
                    }
                }
                dataEnricher.ProcessValue(ref data);
                _store.AddOrUpdateData(data);
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

        private void RequestRemainingUnderlying()
        {
            try
            {
                lock (_lockOnLiveFeedCache)
                {
                    foreach (KeyValuePair<string, SymbolData> symbolKeyValue in _liveFeedCache)
                    {
                        if (symbolKeyValue.Value.CategoryCode == AssetCategory.EquityOption || symbolKeyValue.Value.CategoryCode == AssetCategory.FutureOption)
                        {
                            string underlyingSymbol = symbolKeyValue.Value.UnderlyingSymbol;

                            if ((!String.IsNullOrEmpty(underlyingSymbol)) && (!_liveFeedCache.ContainsKey(underlyingSymbol)) && (!_dictRequestedUnderlyingSymbols.ContainsKey(underlyingSymbol) || _dictRequestedUnderlyingSymbols[underlyingSymbol] < 3))
                            {
                                SnapshotSymbol(underlyingSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol, false, true);

                                //To limit infinite underlying symbol requests and to manage risk, we are allowing 3 requests max.
                                if (_dictRequestedUnderlyingSymbols.ContainsKey(underlyingSymbol))
                                {
                                    _dictRequestedUnderlyingSymbols[underlyingSymbol] += 1;
                                }
                                else
                                {
                                    _dictRequestedUnderlyingSymbols.TryAdd(underlyingSymbol, 1);
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
        }

        /// <summary>
        /// Sets the connection status for Esper engine.
        /// </summary>
        /// <param name="isConnected"></param>
        public static void SetEsperEngineConnectionStatus(bool isConnected)
        {
            try
            {
                IsEsperConnected = isConnected;

                //If Esper is disconnected then reset the value
                if (!isConnected)
                {
                    IsEsperStartedCompletely = false;
                    IsStartUpPricesSentToEsper = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void PeriodicTask(object state)
        {
            try
            {
                if (timerCount % _level1TimerIntervalMultiple == 0) // Fires every 700 miliseconds
                {
                    List<SymbolData> list = null;
                    list = _store.GetData();
                    if (LiveFeedDataReceived != null && (list != null || (IsEsperStartedCompletely && IsEsperConnected && !IsStartUpPricesSentToEsper)))
                    {

                        IsStartUpPricesSentToEsper = true;
                        LiveFeedDataReceived(this, new EventArgs<List<SymbolData>>(list));
                    }
                    //Divya :20132504 call proxy helper to replace proxy symbol with original symbol...

                    if (list != null)
                    {
                        SymbolPreferenceHandling(list);
                        OnResponse(list);
                    }
                }
                ++timerCount;
                if (timerCount == 3000)
                {
                    timerCount = 1;
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
        /// Symbol Preference Handling
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dicFromList"></param>
        public static void SymbolPreferenceHandling(List<SymbolData> list, Dictionary<string, SymbolData> dicFromList = null)
        {
            try
            {
                Dictionary<string, SymbolData> dictSymbols = new Dictionary<string, SymbolData>();
                foreach (SymbolData sym in list)
                {
                    if (dicFromList != null)
                    {
                        if (!dicFromList.ContainsKey(sym.Symbol))
                        {
                            dicFromList.Add(sym.Symbol, sym);
                        }
                    }
                    //Handle the preferences of CurrenciesToUpdate
                    CurrencyConvertHandling(sym);

                    //Handling of proxy symbol
                    ProxySymbolHandling(dictSymbols, sym);
                }
                foreach (KeyValuePair<string, SymbolData> symbolData in dictSymbols)
                {
                    list.Add(symbolData.Value);
                    if (dicFromList != null)
                    {
                        if (!dicFromList.ContainsKey(symbolData.Key))
                        {
                            dicFromList.Add(symbolData.Key, symbolData.Value);
                        }
                        else
                        {
                            dicFromList[symbolData.Key] = symbolData.Value;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Currency Convert Handling based on preference
        /// </summary>
        /// <param name="sym"></param>
        private static void CurrencyConvertHandling(SymbolData sym)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.API) && CommonData.GetInstance().CurrenciesToUpdate.ContainsKey(sym.CurencyCode))
                {
                    ConvertToHigherCurrency_New(sym);
                }
                else if (!CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.API))
                {
                    if (CommonData.GetInstance().CurrenciesToUpdate.ContainsKey(sym.CurencyCode))
                    {
                        lock (_dictSymbolsToConvertInHigherCurrency)
                        {
                            if (!_dictSymbolsToConvertInHigherCurrency.ContainsKey(sym.Symbol))
                            {
                                _dictSymbolsToConvertInHigherCurrency.Add(sym.Symbol, CommonData.GetInstance().CurrenciesToUpdate[sym.CurencyCode]);
                            }
                        }

                        lock (_dictSymbolsToConvertInHigherCurrency)
                        {
                            if (_dictSymbolsToConvertInHigherCurrency.ContainsKey(sym.Symbol))
                            {
                                ConvertToHigherCurrency(sym);
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

        /// <summary>
        /// Proxy Symbol Handling
        /// </summary>
        /// <param name="dictSymbols"></param>
        /// <param name="sym"></param>
        private static void ProxySymbolHandling(Dictionary<string, SymbolData> dictSymbols, SymbolData sym)
        {
            try
            {
                List<SymbolInfo> listsymbol = ProxySymbolHelper.GetInstance().GetSymbolsForProxy(sym.Symbol);
                if (listsymbol != null)
                {
                    //if (_listOfAdviseSymbols.Contains(sym.Symbol))
                    //{
                    //    if (!dictSymbols.ContainsKey(sym.Symbol))
                    //    {
                    //        //Divya :20132504 it means that the proxy symbol is spearately advised so we have to clone the symboldata object
                    //        // and append to this list so that both proxy and original symbol response can be moved..
                    //        SymbolData symClone = DeepCopyHelper.Clone<SymbolData>(sym);
                    //        dictSymbols.Add(sym.Symbol, symClone);
                    //    }
                    //}

                    //Divya :20132504 to check if there are mutltiple original symbols present for a particular proxy so we have to clone the symboldata 
                    //for each symbol and append to the cache as for all these symbols there is a single advise request of the proxy symbol...
                    if (listsymbol.Count > 0)
                    {
                        foreach (SymbolInfo symbol in listsymbol)
                        {
                            if (!dictSymbols.ContainsKey(symbol.Symbol))
                            {
                                SymbolData symClone = DeepCopyHelper.Clone<SymbolData>(sym);
                                symClone.Symbol = symbol.Symbol;
                                symClone.UnderlyingSymbol = symbol.UnderlyingSymbol;
                                dictSymbols.Add(symbol.Symbol, symClone);
                            }
                        }
                    }
                    //else
                    //{
                    //    if (listsymbol.Count > 0)
                    //    {
                    //        SymbolData symClone = DeepCopyHelper.Clone<SymbolData>(sym);
                    //        symClone.Symbol = listsymbol[0].Symbol;
                    //        symClone.UnderlyingSymbol = listsymbol[0].UnderlyingSymbol;
                    //        if (!dictSymbols.ContainsKey(sym.Symbol))
                    //        {
                    //            dictSymbols.Add(symClone.Symbol, symClone);
                    //        }
                    //    }
                    //}
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
        /// Converts to higher currency.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        private static void ConvertToHigherCurrency_New(SymbolData snapshot)
        {
            try
            {
                var conversionDetails = CommonData.GetInstance().CurrenciesToUpdate[snapshot.CurencyCode];
                double value = conversionDetails.AdjustValue; // _dictSymbolsToConvertInHigherCurrency[snapshot.Symbol].AdjustValue;
                snapshot.CurencyCode = conversionDetails.HigherCurrency; // _dictSymbolsToConvertInHigherCurrency[snapshot.Symbol].HigherCurrency;
                snapshot.Ask /= value;
                snapshot.Bid /= value;
                snapshot.Change /= value;
                snapshot.Dividend /= value;
                snapshot.DividendAmtRate /= (float)value;
                snapshot.High /= value;
                snapshot.LastPrice /= value;
                snapshot.Low /= value;
                snapshot.Open /= value;
                snapshot.Previous /= value;
                snapshot.GapOpen /= value;
                snapshot.Spread /= value;


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
        /// Converts to higher currency.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        private static void ConvertToHigherCurrency(SymbolData snapshot)
        {
            try
            {
                double value = _dictSymbolsToConvertInHigherCurrency[snapshot.Symbol].AdjustValue;

                snapshot.CurencyCode = _dictSymbolsToConvertInHigherCurrency[snapshot.Symbol].HigherCurrency;
                snapshot.Ask /= value;
                snapshot.Bid /= value;
                snapshot.Change /= value;
                snapshot.Dividend /= value;
                snapshot.DividendAmtRate /= (float)value;
                snapshot.High /= value;
                snapshot.LastPrice /= value;
                snapshot.Low /= value;
                snapshot.Open /= value;
                snapshot.Previous /= value;
                snapshot.GapOpen /= value;
                snapshot.Spread /= value;
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

        public Dictionary<string, SymbolData> GetLiveFeedDataDictCopy()
        {
            Dictionary<string, SymbolData> LiveFeedDataDictCopy = null;
            try
            {
                RequestRemainingUnderlying();
                ///Here we are first copying this dictionary into the similar collection and leave the lock as soon as possible.
                ///
                if (CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.API))
                {
                    lock (_lockOnLiveFeedCache)
                    {
                        List<SymbolData> priceData = _liveFeedDataProvider.GetAvailableLiveFeed();
                        var priceDataCloned = DeepCopyHelper.Clone<List<SymbolData>>(priceData);
                        var apiFeedDataDict = new Dictionary<string, SymbolData>();
                        SymbolPreferenceHandling(priceDataCloned, apiFeedDataDict);
                        _liveFeedCache = apiFeedDataDict;
                        LiveFeedDataDictCopy = DeepCopyHelper.Clone<Dictionary<string, SymbolData>>(apiFeedDataDict);
                    }
                }
                else
                {
                    lock (_lockOnLiveFeedCache)
                    {
                        if (_liveFeedCache != null && _liveFeedCache.Count > 0)
                        {
                            LiveFeedDataDictCopy = DeepCopyHelper.Clone<Dictionary<string, SymbolData>>(_liveFeedCache);
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
                return null;
            }
            return LiveFeedDataDictCopy;
        }

        public void OnResponse(List<SymbolData> listResponse)
        {
            string symbol = string.Empty;
            string idcoSymbol = string.Empty;
            OptionSymbolData optData = null;
            List<SymbolData> listfxData = new List<SymbolData>();

            try
            {
                if (listResponse != null)
                {
                    for (int i = 0; i < listResponse.Count; i++)
                    {
                        SymbolData data = listResponse[i];
                        symbol = data.Symbol;

                        lock (_lockOnLiveFeedCache)
                        {
                            if (data is OptionSymbolData)
                            {
                                /// This case is specially handled for those cases when the data is requested by the idco symbol for options.
                                optData = data as OptionSymbolData;
                                idcoSymbol = optData.IDCOOptionSymbol;
                            }
                            // this condition will always be false in case of fx/forward as the esignal symbol is different from symbol in LiveFeed cache, so
                            //there is a special handling for fx.
                            if (!_liveFeedCache.ContainsKey(symbol))
                            {
                                if (data.CategoryCode == AssetCategory.EquityOption || data.CategoryCode == AssetCategory.FutureOption)
                                {
                                    if (string.IsNullOrEmpty(data.UnderlyingSymbol))
                                        continue;

                                    string underlyingsymbol = data.UnderlyingSymbol;
                                    if ((!String.IsNullOrEmpty(underlyingsymbol)) && (!_liveFeedCache.ContainsKey(underlyingsymbol)))
                                    {
                                        Logger.LoggerWrite("OnResponse() Request Received on Pricing for Symbol : " + underlyingsymbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                                        SnapshotSymbol(underlyingsymbol, data.RequestedSymbology, false, false);
                                    }
                                }
                                double markPx = MarkCacheManager.GetMarkPriceForSymbol(symbol);
                                ///Double.MinValue represent that mark prices were not found in the cache. In that case, we won't assign
                                ///anything here.
                                if (markPx != double.MinValue)
                                {
                                    data.MarkPrice = markPx;
                                }
                                if (data.CategoryCode.Equals(AssetCategory.FX))
                                {
                                    List<fxInfo> listFxMappedData = null;
                                    SymbolData dataNew = null;
                                    lock (_dictFxMapping)
                                    {
                                        ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(symbol.Replace('/', '-'), 0);
                                        if (conversionRate != null)
                                        {
                                            if (!String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                                            {
                                                symbol = conversionRate.FXeSignalSymbol;
                                                data.Symbol = symbol;
                                            }
                                        }
                                        if (_dictFxMapping.ContainsKey(symbol))
                                        {
                                            if (listFxMappedData == null)
                                            {
                                                listFxMappedData = new List<fxInfo>();
                                            }
                                            listFxMappedData = _dictFxMapping[symbol];
                                        }
                                    }
                                    if (listFxMappedData != null)
                                    {
                                        foreach (fxInfo info in listFxMappedData)
                                        {
                                            if (!_liveFeedCache.ContainsKey(info.PranaSymbol))
                                            {
                                                double fxMarkPx = MarkCacheManager.GetMarkPriceForSymbol(info.PranaSymbol);
                                                if (info.CategoryCode.Equals(AssetCategory.FXForward) && info.ConversionRate != null)
                                                {
                                                    dataNew = new FxForwardContractSymbolData(data, info.ConversionRate.ConversionMethod);
                                                }
                                                else if (info.CategoryCode.Equals(AssetCategory.FX) && info.ConversionRate != null)
                                                {
                                                    dataNew = new FxContractSymbolData(data, info.ConversionRate.ConversionMethod);
                                                }
                                                else if (info.CategoryCode.Equals(AssetCategory.Forex) && info.ConversionRate != null)
                                                {
                                                    dataNew = (SymbolData)DeepCopyHelper.Clone(data);
                                                    ((FxSymbolData)(dataNew)).ConversionMethod = info.ConversionRate.ConversionMethod;
                                                }
                                                if (dataNew != null)
                                                {
                                                    dataNew.Symbol = info.PranaSymbol;
                                                    dataNew.CategoryCode = info.CategoryCode;
                                                    // double closingMark = MarkCacheManager.GetMarkPriceForSymbol(info.PranaSymbol);
                                                    ///Double.MinValue represent that mark prices were not found in the cache. In that case, we won't assign
                                                    ///anything here.
                                                    if (fxMarkPx != double.MinValue)
                                                    {
                                                        dataNew.MarkPrice = fxMarkPx;
                                                    }
                                                    dataNew.PricingSource = PricingSource.LiveFeed;
                                                    _liveFeedCache.Add(dataNew.Symbol, dataNew);

                                                    lock (_listOfWithoutGreeksSnapshotSymbol)
                                                    {
                                                        if (_listOfWithoutGreeksSnapshotSymbol.Contains(dataNew.Symbol))
                                                        {
                                                            listfxData.Add(dataNew);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                SymbolData fxData = _liveFeedCache[info.PranaSymbol];
                                                data.PricingSource = PricingSource.LiveFeed;
                                                fxData.UpdateContinuousData(data);
                                                lock (_listOfWithoutGreeksSnapshotSymbol)
                                                {
                                                    if (_listOfWithoutGreeksSnapshotSymbol.Contains(fxData.Symbol))
                                                    {
                                                        listfxData.Add(fxData);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (!_liveFeedCache.ContainsKey(symbol))
                                {
                                    data.PricingSource = PricingSource.LiveFeed;
                                    _liveFeedCache.Add(symbol, data);
                                }
                            }
                            else
                            {
                                data.PricingSource = PricingSource.LiveFeed;
                                _liveFeedCache[symbol].UpdateContinuousData(data);
                            }
                        }
                        if (string.IsNullOrEmpty(data.UnderlyingSymbol))
                            continue;
                        bool isSymbolSnapShotReqWithoutGreeks = false;
                        lock (_lockOnListOfWithoutGreeksSnapshotSymbol)
                        {
                            if (data.CategoryCode == AssetCategory.FX)
                            {
                                foreach (SymbolData fxdata in listfxData)
                                {
                                    if (_listOfWithoutGreeksSnapshotSymbol.Contains(fxdata.Symbol))
                                    {
                                        Data obj = new Data();
                                        obj.Info = fxdata;
                                        if (OptionSnapshotResponse != null)
                                        {
                                            OptionSnapshotResponse(this, obj);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (_listOfWithoutGreeksSnapshotSymbol.Contains(symbol))
                                {
                                    _listOfWithoutGreeksSnapshotSymbol.Remove(symbol);
                                    isSymbolSnapShotReqWithoutGreeks = true;
                                    if (data is OptionSymbolData)
                                    {
                                        ((OptionSymbolData)data).RequestedSymbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                                    }
                                }
                                else if (!String.IsNullOrEmpty(idcoSymbol) && _listOfWithoutGreeksSnapshotSymbol.Contains(idcoSymbol))
                                {
                                    isSymbolSnapShotReqWithoutGreeks = true;
                                    _listOfWithoutGreeksSnapshotSymbol.Remove(idcoSymbol);
                                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-3692
                                    if (data is OptionSymbolData)
                                    {
                                        ((OptionSymbolData)data).RequestedSymbology = ApplicationConstants.SymbologyCodes.IDCOOptionSymbol;
                                    }
                                }
                                if (isSymbolSnapShotReqWithoutGreeks)
                                {
                                    Data obj = new Data();
                                    obj.Info = data;
                                    if (OptionSnapshotResponse != null)
                                    {
                                        OptionSnapshotResponse(this, obj);
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
        }

        public void DeleteSymbol(string Symbol)
        {
            _liveFeedDataProvider.DeleteSymbol(Symbol);
        }

        public void AdviseSymbol(string symbol)
        {
            try
            {
                if (_isLiveFeedConnected)
                {
                    if (_enableMarketData_ContinuousDataLogging)
                    {
                        Logger.LoggerWrite(DateTime.UtcNow.ToLongTimeString() + " Symbol Advised = " + symbol, LoggingConstants.LIVEDATA_LOGGING);
                    }
                    _liveFeedDataProvider.GetContinuousData(symbol);
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

        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            Dictionary<string, bool> isInternational = new Dictionary<string, bool>();
            try
            {
                isInternational = _liveFeedDataProvider.CheckIfInternationalSymbols(symbols);
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
            return isInternational;
        }

        public void SnapshotSymbol(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool isGreekRequired, bool completeInfo, bool isSmRequest = false)
        {
            try
            {
                string toupperSymbol = symbol.Trim().ToUpper();
                if (false == isGreekRequired)
                {
                    lock (_lockOnListOfWithoutGreeksSnapshotSymbol)
                    {
                        if (!_listOfWithoutGreeksSnapshotSymbol.Contains(toupperSymbol))
                        {
                            _listOfWithoutGreeksSnapshotSymbol.Add(toupperSymbol);
                        }
                    }
                }

                //Divya:02042013 :call proxy helper to fetch proxy info..
                //If a proxy symbol is defined for a symbol, then we fetch the feed for proxy symbol instead of the original symbol.
                string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(symbol);
                if (proxySymbol.Equals(string.Empty))
                {
                    proxySymbol = symbol;
                }
                else
                {
                    Logger.LoggerWrite("ProxySymbol (" + proxySymbol + ") found for Symbol : " + symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                }

                if (IsMarketDataServiceReq(proxySymbol))
                {
                    if (!_listOfAdviseSymbols.Contains(proxySymbol))
                        _marketFeedService.GetSnapShotData(proxySymbol.Trim().ToUpper(), symbologyCode, isSmRequest);
                }
                else
                    _liveFeedDataProvider.GetSnapShotData(proxySymbol.Trim().ToUpper(), symbologyCode, completeInfo);
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
        /// Determines whether [is market data service req] [the specified symbol].
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>
        ///   <c>true</c> if [is market data service req] [the specified symbol]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMarketDataServiceReq(string symbol)
        {
            bool output = false;
            try
            {
                if (_isLivePricesFromFile)
                {
                    bool isAllFilePrices = !CachedDataManager.GetInstance.IsFilePricingForTouch() && !_isExchangePricesFromFile;
                    if (isAllFilePrices)
                        output = true;
                    else
                    {
                        MarketDataSymbolResponse resp = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);

                        if (resp != null)
                        {
                            string exchange = CachedDataManager.GetInstance.GetAUECText(resp.AUECID);
                            if (!string.IsNullOrEmpty(exchange) && _filePriceExchangeList.Contains(exchange))
                            {
                                output = true;
                            }
                        }
                        else
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Exchange wise prices are supported by market data utility only in case of ACTIV and FactSet primary providers.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
            return output;
        }

        public void GetSMData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            try
            {
                string toupperSymbol = symbol.Trim().ToUpper();
                if (IsMarketDataServiceReq(symbol))
                {
                    lock (_lockOnListOfWithoutGreeksSnapshotSymbol)
                    {
                        if (!_listOfWithoutGreeksSnapshotSymbol.Contains(toupperSymbol))
                        {
                            _listOfWithoutGreeksSnapshotSymbol.Add(toupperSymbol);
                        }
                    }
                    _marketFeedService.GetSnapShotData(symbol, symbologyCode, false);
                }
                else
                    SnapshotSymbol(symbol, symbologyCode, false, false, true);
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

        public void SnapshotSymbol(fxInfo fxReqData, ApplicationConstants.SymbologyCodes symbologyCode, bool isGreekRequired, bool completeInfo)
        {
            try
            {
                string toupperSymbol = fxReqData.PranaSymbol.Trim().ToUpper();
                string FXesginalSymbol = string.Empty;

                AssetCategory categoryCode = fxReqData.CategoryCode;
                lock (_dictFxMapping)
                {
                    if (categoryCode.Equals(AssetCategory.FX) || categoryCode.Equals(AssetCategory.FXForward) || categoryCode.Equals(AssetCategory.Forex))
                    {
                        ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromCurrencies(fxReqData.FromCurrencyID, fxReqData.ToCurrencyID, 0);
                        if (conversionRate != null)
                        {
                            if (!String.IsNullOrEmpty(conversionRate.FXeSignalSymbol))
                            {
                                FXesginalSymbol = conversionRate.FXeSignalSymbol;
                                UpdateFxSymbolMapping(fxReqData.PranaSymbol, FXesginalSymbol, fxReqData.CategoryCode, conversionRate);
                            }
                        }
                    }
                }
                if (false == isGreekRequired)
                {
                    lock (_lockOnListOfWithoutGreeksSnapshotSymbol)
                    {
                        if (!_listOfWithoutGreeksSnapshotSymbol.Contains(toupperSymbol))
                        {
                            _listOfWithoutGreeksSnapshotSymbol.Add(toupperSymbol);
                        }
                    }
                }
                //Divya:02042013 :call proxy helper to fetch proxy info..
                //If a proxy symbol is defined for a symbol, then we fetch the feed for proxy symbol instead of the original symbol.
                string proxySymbol = ProxySymbolHelper.GetInstance().GetProxySymbol(fxReqData.PranaSymbol);
                string mduSymbol = string.IsNullOrEmpty(proxySymbol) ? toupperSymbol : proxySymbol;
                if (IsMarketDataServiceReq(mduSymbol))
                {
                    if (!_listOfAdviseSymbols.Contains(mduSymbol))
                        _marketFeedService.GetSnapShotData(mduSymbol.Trim().ToUpper(), symbologyCode, false);
                }
                else if (proxySymbol.Equals(string.Empty))
                {
                    //If proxy is not defined, it is overridenm with the original symbol.
                    proxySymbol = FXesginalSymbol;
                }
                _liveFeedDataProvider.GetSnapShotData(proxySymbol.Trim().ToUpper(), symbologyCode, completeInfo);
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

        public void GetStaticOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            try
            {
                _liveFeedDataProvider.GetOptionChain(underlyingSymbol, optionChainFilter);
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
        /// Gets the live feed data list.
        /// </summary>
        /// <returns></returns>
        public List<SymbolData> GetLiveFeedDataList()
        {
            try
            {
                return _liveFeedDataProvider.GetAvailableLiveFeed();
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
        /// Gets the live feed data list.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<SymbolData>> GetLiveFeedDataListAsync()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return GetLiveFeedDataList();
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

        public List<fxInfo> GetTickerSymbolFromFX(string fxSymbol)
        {
            try
            {
                lock (_dictFxMapping)
                {
                    if (_dictFxMapping.ContainsKey(fxSymbol) && _dictFxMapping[fxSymbol].Count > 0)
                    {
                        return new List<fxInfo>(_dictFxMapping[fxSymbol]);
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

        /// <summary>
        /// Get Live Data Directly From Feed
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetLiveDataDirectlyFromFeed()
        {
            // await System.Threading.Tasks.Task.CompletedTask;
            var data = await _liveFeedDataProvider.GetLiveDataDirectlyFromFeed();
            return data;
        }

        /// <summary>
        /// Get Updated Live Feed Data from cache
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, SymbolData>> GetUpdatedLiveFeedDataFromCache()
        {
            try
            {

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
                return DeepCopyHelper.Clone<Dictionary<string, SymbolData>>(_liveFeedCache);

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

        public Dictionary<string, string> GetUserInformation()
        {
            try
            {
                return _liveFeedDataProvider.GetUserInformation();
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

            return new Dictionary<string, string>();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            try
            {
                return _liveFeedDataProvider.GetUserPermissionsInformation();
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

            return new List<Dictionary<string, string>>();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            try
            {
                return _liveFeedDataProvider.GetSubscriptionInformation();
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

            return new Dictionary<string, int>();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            try
            {
                return _liveFeedDataProvider.GetTickersLastStatusCode();
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

            return new Dictionary<string, string>();
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_liveFeedDataProvider != null)
                    _liveFeedDataProvider.Disconnect();
                _liveFeedDataProvider = null;
                if (tmrL1 != null)
                    tmrL1.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Set Debug Enable Disable
        /// </summary>
        /// <param name="isDebugEnable"></param>
        /// <param name="pctTolerance"></param>
        public async System.Threading.Tasks.Task SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                this._isDebugEnable = isDebugEnable;
                this._pctTolerance = pctTolerance;
                _liveFeedDataProvider.SetDebugEnableDisable(isDebugEnable, pctTolerance);
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
        /// Get Debug Enable Disable Params
        /// </summary>
        public async Task<Tuple<bool, double>> GetDebugEnableDisableParams()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return new Tuple<bool, double>(_isDebugEnable, _pctTolerance);
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
    }
}
