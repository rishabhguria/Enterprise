using Prana.AmqpAdapter.Amqp;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionCalculator.CalculationComponent;
using Prana.OptionCalculator.Common;
using Prana.SocketCommunication;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.OptionServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class PricingServiceManager : IPricingService, IDisposable
    {
        private LiveFeedManager.LiveFeedManager _liveFeedManager = null;
        private ILiveFeedAdapter _pricingApiServices = null;
        private IQueueProcessor _outPricingQueue = null;
        private Dictionary<string, List<ILiveFeedCallback>> _snapshotSubscribers = new Dictionary<string, List<ILiveFeedCallback>>();
        private Dictionary<string, List<ILiveFeedCallback>> _WithoutGreeksSnapshotSubscribers = new Dictionary<string, List<ILiveFeedCallback>>();
        private Dictionary<string, List<ILiveFeedCallback>> _optionChainSubscribers = new Dictionary<string, List<ILiveFeedCallback>>();
        private Dictionary<ILiveFeedCallback, List<string>> _optionChainSubscriberOptionMappings = new Dictionary<ILiveFeedCallback, List<string>>();
        private HashSet<ILiveFeedCallback> _optionChainRecentSubscribers = new HashSet<ILiveFeedCallback>();
        private Dictionary<string, SymbolData> _pricingCache = new Dictionary<string, SymbolData>();
        private Dictionary<string, SymbolData> _pricingCacheTouch = new Dictionary<string, SymbolData>();
        private Dictionary<string, List<ILiveFeedCallback>> _clientCallBackTTPTT = new Dictionary<string, List<ILiveFeedCallback>>();

        private System.Timers.Timer _snapShotTmr = new System.Timers.Timer();
        private DateTime _lastOptionChainDataTime = DateTime.MinValue;
        private object _lockOnPricingCache = new object();
        private object _lockOnTouchPricingCache = new object();
        private System.Timers.Timer _complianceSnapshotTmr = new System.Timers.Timer();
        private object _lockOnComplianceSnapshot = new object();
        private System.Timers.Timer _sendSymbolDataToEsperTimer = new System.Timers.Timer();

        public bool IsLiveFeedActive
        {
            get
            {
                if (_liveFeedManager != null && _liveFeedManager.IsLiveFeedConnected)
                {
                    return _liveFeedManager.IsLiveFeedConnected;
                }
                return false;
            }
        }

        /// <summary>
        /// Only used for subscribe/unsubscribe
        /// </summary>
        private List<ILiveFeedCallback> _liveFeedConnectionSubscriberslist = new List<ILiveFeedCallback>();

        private object _lockOnConnectionSubscribersList = new object();
        private object _lockOnWithoutGreeksSnapshotSubscribersList = new object();
        private bool _getSameDayClosedDataOnDV = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("GetSameDayClosedDataOnDailyValuation"));
        private bool isOnlyFixedIncomeSymbols = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("GetOnlyFixedIncomeOnCollateral"));
        private bool _enableMarketDataLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableMarketDataLogging"));
        private bool _isLivePricesFromFile = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsLivePricesFromFile"));
        private int _optionChainDataInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_LiveFeed, ConfigurationHelper.CONFIGKEY_LiveFeed_OptionChainDataInterval));
        private int _sendSymbolDataTimerInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_SendSymbolDataTimerInterval));

        private static bool _isEsperEngineConnected = false;
        public static bool IsEsperEngineConnected
        {
            get { return _isEsperEngineConnected; }
            set { _isEsperEngineConnected = value; }
        }

        /// <summary>
        /// The enable esignalcontinuous data logging
        /// </summary>
        private bool _enableMarketData_ContinuousDataLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableMarketData_ContinuousDataLogging"));

        public PricingServiceManager()
        {
            try
            {
                _sendSymbolDataToEsperTimer.Interval = _sendSymbolDataTimerInterval;
                _sendSymbolDataToEsperTimer.Elapsed += SendLiveFeedDataToEsperEngine;
                _sendSymbolDataToEsperTimer.Start();
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

        private void SetupLiveFeedManager()
        {
            try
            {
                _liveFeedManager = LiveFeedManager.LiveFeedManager.GetInstance();
                _liveFeedManager.LiveFeedConnected += new EventHandler<EventArgs<bool>>(LiveFeedConnected);
                _liveFeedManager.LiveFeedDisconnected += new EventHandler<EventArgs<bool>>(LiveFeedDisconnected);
                _liveFeedManager.OptionSnapshotResponse += new EventHandler<Data>(OptionsnapshotWithoutGreeks);
                _liveFeedManager.OptionChainResponse += new EventHandler<EventArgs<string, List<OptionStaticData>>>(OptionChain_Response);
                //_liveFeedManager.LiveFeedDataReceived += SendLiveFeedDataToEsperEngine;

                //set service for provider type API
                if (CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.API) && _pricingApiServices != null)
                    _liveFeedManager.PricingApiServices = _pricingApiServices;

                _liveFeedManager.Initialize();

                if (_liveFeedManager == null)
                {
                    return;
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
        /// Sets the connection status for Esper engine.
        /// </summary>
        /// <param name="isConnected"></param>
        public static void SetEsperEngineConnectionStatus(bool isConnected)
        {
            try
            {
                IsEsperEngineConnected = isConnected;
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

        /// <summary>
        /// Sends live feed symbol data to Esper engine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendLiveFeedDataToEsperEngine(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (IsEsperEngineConnected && _pricingCache != null && _pricingCache.Count > 0)
                    AmqpHelper.SendObject(_pricingCache.Values.ToList(), "LiveFeed", "All");
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

        /// <summary>
        /// Sends OMI data for changed symbols to Esper engine.
        /// </summary>
        /// <param name="symbolsDataChange"></param>
        private void SendOMIDataToEsperEngine(List<string> modifiedSymbolsList)
        {
            try
            {
                if (IsEsperEngineConnected)
                {
                    List<SymbolData> symbolsData = new List<SymbolData>();
                    List<string> symbolsFXForwards = new List<string>();
                    if (modifiedSymbolsList != null && modifiedSymbolsList.Count > 0 && _pricingCache != null && _pricingCache.Count > 0)
                    {
                        foreach (string symbol in modifiedSymbolsList)
                        {
                            _pricingCache.TryGetValue(symbol, out SymbolData data);
                            if (data != null)
                                symbolsData.Add(data);
                        }
                    }
                    if (symbolsData != null && symbolsData.Count > 0)
                        AmqpHelper.SendObject(symbolsData, "LiveFeed", "All");

                    // Send FX and FX Forward symbols information to the Esper engine for pricing calculations from PI.
                    #region Sending symbols information to Esper engine
                    List<UserOptModelInput> listOMIdata = GetOMIDataFromCacheForCompliance();

                    foreach (UserOptModelInput input in listOMIdata)
                    {
                        if ((input.AssetID == (int)AssetCategory.FX || input.AssetID == (int)AssetCategory.FXForward) && input.LastPriceUsed)
                        {
                            symbolsFXForwards.Add(input.Symbol);
                        }
                    }

                    if (symbolsFXForwards?.Count > 0)
                    {
                        AmqpHelper.SendObject(symbolsFXForwards, "OtherData", "FxFwdPriceAvailableInPricingInput");
                    }
                    #endregion
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

        /// <summary>
        /// Set Pricing Api Service
        /// </summary>
        /// <param name="pricingApiServices"></param>
        public void SetPricingApiService(ILiveFeedAdapter pricingApiServices)
        {
            _pricingApiServices = pricingApiServices;
        }

        private void GetAUECFromMapping(ref SymbolData data)
        {
            Dictionary<Tuple<int, string>, int> dictDefaultAUECMappingPresent = CachedDataManager.GetInstance.GetDefaultAUECMapping();
            Tuple<int, string> countryCurrencyPair = new Tuple<int, string>(data.CountryID, data.CurencyCode);

            try
            {
                if (dictDefaultAUECMappingPresent.ContainsKey(countryCurrencyPair))
                {
                    data.AUECID = dictDefaultAUECMappingPresent[countryCurrencyPair];
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("AUECID not found for Ticker Symbol = " + data.Symbol.ToString() + " Underlying = " + data.UnderlyingCategory.ToString() + " Asset = " + data.CategoryCode.ToString() + " Currency = " + data.CurencyCode.ToString() + "Exchange = " + data.ListedExchange.ToString() + ". So Processed with Default AUECID(From Country-Currency) =" + data.AUECID.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                }

                else
                {
                    switch (data.CategoryCode)
                    {
                        //Gaurav: Cuurently we are saving Indices as Equity in SecMaster
                        case AssetCategory.Indices:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultIndicesAUECID;
                            break;

                        case AssetCategory.Equity:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultEquityAUECID;
                            break;

                        case AssetCategory.EquityOption:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultOptionAUECID;
                            break;

                        case AssetCategory.Future:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultFutureAUECID;
                            break;

                        case AssetCategory.FutureOption:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultFutureOptionAUECID;
                            break;

                        case AssetCategory.FXForward:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultForwardAUECID;
                            break;

                        case AssetCategory.FX:
                            data.AUECID = AssetCategoryWiseAuecConstants.DefaultFxAUECID;
                            break;
                    }
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("AUECID not found for Ticker Symbol = " + data.Symbol.ToString() + " Underlying = " + data.UnderlyingCategory.ToString() + " Asset = " + data.CategoryCode.ToString() + " Currency = " + data.CurencyCode.ToString() + "Exchange = " + data.ListedExchange.ToString() + ". So Processed with Default AUECID =" + data.AUECID.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                if (data.AUECID != int.MinValue && data.AUECID != 0)
                {
                    int currencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(data.AUECID);
                    string currencyText = CachedDataManager.GetInstance.GetCurrencyText(currencyID);
                    data.CurencyCode = currencyText;
                    int exchangeID = int.MinValue;
                    Underlying underlying = Underlying.None;
                    CachedDataManager.GetInstance.GetUnderlyingExchangeIDFromAUECID(data.AUECID, ref underlying, ref exchangeID);
                    data.UnderlyingCategory = underlying;
                    data.ExchangeID = exchangeID;
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

        private void OptionsnapshotWithoutGreeks(object sender, Data e)
        {
            SymbolData data = e.Info;
            string idcoSymbol = string.Empty;
            List<string> currentSymbols = new List<string>();
            try
            {
                lock (_lockOnWithoutGreeksSnapshotSubscribersList)
                {
                    if (_WithoutGreeksSnapshotSubscribers.Count == 0)
                    {
                        return;
                    }

                    if (data is OptionSymbolData)
                    {
                        /// This case is specially handled for those cases when the data is requested by the idco symbol for options.
                        OptionSymbolData optData = data as OptionSymbolData;
                        idcoSymbol = optData.IDCOOptionSymbol;
                    }

                    foreach (KeyValuePair<string, List<ILiveFeedCallback>> symbolSubscribers in _WithoutGreeksSnapshotSubscribers)
                    {
                        if (data.Symbol == symbolSubscribers.Key || (!string.IsNullOrEmpty(idcoSymbol) && idcoSymbol.Equals(symbolSubscribers.Key)) || (!string.IsNullOrEmpty(data.FactSetSymbol) && data.FactSetSymbol.Equals(symbolSubscribers.Key)) || (!string.IsNullOrEmpty(data.ActivSymbol) && data.ActivSymbol.Equals(symbolSubscribers.Key)))
                        {
                            if (data.AUECID == int.MinValue || data.AUECID == 0)
                            {
                                GetAUECFromMapping(ref data);
                            }

                            SubscriberInfo subInfo = new SubscriberInfo();
                            subInfo.SubscribersList.AddRange(symbolSubscribers.Value);
                            subInfo.Data = data;
                            if (_optionChainSubscriberOptionMappings.Values.FirstOrDefault(o => o.Contains(symbolSubscribers.Key)) != null)
                                subInfo.SaveSymbolInDb = false;

                            if (data != null)
                            {
                                if (!currentSymbols.Contains(data.Symbol))
                                {
                                    if (idcoSymbol.Equals(symbolSubscribers.Key))
                                    {
                                        currentSymbols.Add(idcoSymbol);
                                    }
                                    else
                                    {
                                        currentSymbols.Add(data.Symbol);
                                    }

                                    ThreadPool.QueueUserWorkItem(new WaitCallback(SnapshotResponse), subInfo);
                                }
                            }
                        }
                    }

                    foreach (string symbol in currentSymbols)
                    {
                        if (_WithoutGreeksSnapshotSubscribers.ContainsKey(symbol))
                        {
                            _WithoutGreeksSnapshotSubscribers.Remove(symbol);
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

        private List<ILiveFeedCallback> _faultySubscriberslist = new List<ILiveFeedCallback>();
        private object _lockOnLiveFeedConnectionSubscriberslist = new object();

        private void LiveFeedConnected(object sender, EventArgs<bool> e)
        {
            try
            {
                if (e.Value)
                {
                    if (LiveFeedConnectionStatusChanged != null)
                    {
                        LiveFeedConnectionStatusChanged(sender, new EventArgs<bool>(true));
                    }

                    lock (_lockOnLiveFeedConnectionSubscriberslist)
                    {
                        foreach (ILiveFeedCallback subscribers in _liveFeedConnectionSubscriberslist)
                        {
                            try
                            {
                                subscribers.LiveFeedConnected();
                            }
                            catch (Exception ex)
                            {
                                _faultySubscriberslist.Add(subscribers);
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            }
                        }

                        if (_faultySubscriberslist.Count > 0)
                        {
                            foreach (ILiveFeedCallback subscribers in _faultySubscriberslist)
                            {
                                if (_liveFeedConnectionSubscriberslist.Contains(subscribers))
                                {
                                    _liveFeedConnectionSubscriberslist.Remove(subscribers);
                                }
                            }
                            _faultySubscriberslist.Clear();
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

        private void LiveFeedDisconnected(object sender, EventArgs<bool> e)
        {
            try
            {
                if (!e.Value)
                {
                    if (LiveFeedConnectionStatusChanged != null)
                    {
                        LiveFeedConnectionStatusChanged(sender, new EventArgs<bool>(false));
                    }

                    lock (_lockOnLiveFeedConnectionSubscriberslist)
                    {
                        foreach (ILiveFeedCallback subscribers in _liveFeedConnectionSubscriberslist)
                        {
                            try
                            {
                                subscribers.LiveFeedDisConnected();
                            }
                            catch (Exception ex)
                            {
                                _faultySubscriberslist.Add(subscribers);
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            }
                        }

                        if (_faultySubscriberslist.Count > 0)
                        {
                            foreach (ILiveFeedCallback subscribers in _faultySubscriberslist)
                            {
                                if (_liveFeedConnectionSubscriberslist.Contains(subscribers))
                                {
                                    _liveFeedConnectionSubscriberslist.Remove(subscribers);
                                }
                            }
                            _faultySubscriberslist.Clear();
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

        private void OptionChain_Response(object sender, EventArgs<string, List<OptionStaticData>> e)
        {
            try
            {
                lock (_optionChainSubscribers)
                {
                    List<ILiveFeedCallback> faultCallback = new List<ILiveFeedCallback>();

                    if (_optionChainSubscribers.ContainsKey(e.Value))
                    {
                        foreach (ILiveFeedCallback subscriber in _optionChainSubscribers[e.Value])
                        {
                            try
                            {
                                lock (_optionChainSubscriberOptionMappings)
                                {
                                    if (_optionChainSubscriberOptionMappings.ContainsKey(subscriber))
                                    {
                                        _optionChainSubscriberOptionMappings[subscriber] = e.Value2.Select(s => s.Symbol).ToList();
                                    }
                                    else
                                    {
                                        _optionChainSubscriberOptionMappings.Add(subscriber, e.Value2.Select(s => s.Symbol).ToList());
                                    }
                                }

                                lock (_optionChainRecentSubscribers)
                                    if (!_optionChainRecentSubscribers.Contains(subscriber))
                                        _optionChainRecentSubscribers.Add(subscriber);

                                subscriber.OptionChainResponse(e.Value, e.Value2);
                                if (e.Value2 != null && e.Value2.Count > 0)
                                    RequestMultipleOptions(e.Value, e.Value2.Select(s => s.Symbol).ToHashSet<string>(), subscriber);
                            }
                            catch
                            {
                                faultCallback.Add(subscriber);
                            }
                        }

                        foreach (ILiveFeedCallback subscriber in faultCallback)
                        {
                            _optionChainSubscribers[e.Value].Remove(subscriber);
                            _optionChainSubscriberOptionMappings.Remove(subscriber);
                        }
                    }
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

        private void _outPricingQueue_MessageQueued(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                QueueMessage message = e.Value;
                switch (message.MsgType)
                {
                    case OptionDataFormatter.MSGTYPE_PricingData:
                        string newMsg = message.Message.ToString();
                        lock (_lockOnPricingCache)
                        {
                            _pricingCache = OptionDataFormatter.CreateOptionGreeksFromString(newMsg);
                        }
                        break;
                    case OptionDataFormatter.MSGTYPE_PricingDataFile:
                        string newMsg1 = message.Message.ToString();
                        lock (_lockOnTouchPricingCache)
                        {
                            _pricingCacheTouch = OptionDataFormatter.CreateOptionGreeksFromString(newMsg1);
                        }
                        break;
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

        #region IPricingService Members
        public LiveFeedPreferences GetOMILiveFeedPreferences()
        {
            LiveFeedPreferences prefs = null;
            try
            {
                prefs = OptionModelUserInputCache.GetLiveFeedPreferences();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return prefs;
        }

        public void SaveOMILiveFeedPreferences(LiveFeedPreferences Preferences)
        {
            try
            {
                OptionModelDataManager.SaveOMIPrefDataintoDB(Preferences);
                OptionModelUserInputCache.UpdateLiveFeedPreferences(Preferences);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public void Initialize(IQueueProcessor outPricingQueue)
        {
            try
            {
                SetupLiveFeedManager();
                _outPricingQueue = outPricingQueue;
                _outPricingQueue.MessageQueued += new EventHandler<EventArgs<QueueMessage>>(_outPricingQueue_MessageQueued);
                _snapShotTmr.Elapsed += new System.Timers.ElapsedEventHandler(_snapShotDataTmr_Elapsed);
                ProxySymbolHelper.PlugUnplugProxy += new EventHandler<ProxyDataEventArgs>(ProxyHelper_PlugUnplugProxy);
                _snapShotTmr.Interval = 500;
                _complianceSnapshotTmr.Elapsed += new System.Timers.ElapsedEventHandler(_complianceSnapshotTmr_Elapsed);
                _complianceSnapshotTmr.Interval = 500;

                LoadBalancer.GetInstance.Initialise(outPricingQueue);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            Dictionary<string, bool> isInternational = new Dictionary<string, bool>();
            try
            {
                isInternational = _liveFeedManager.CheckIfInternationalSymbols(symbols);
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
            return isInternational;
        }

        public List<UserOptModelInput> GetDataFromOMI(List<string> symbols)
        {
            List<UserOptModelInput> listSymbolWiseOMIdata = new List<UserOptModelInput>();
            foreach (string symbol in symbols)
            {
                UserOptModelInput userOmi = OptionModelUserInputCache.GetUserOMIData(symbol);
                if (userOmi != null)
                {
                    if (!listSymbolWiseOMIdata.Contains(userOmi))
                    {
                        listSymbolWiseOMIdata.Add(userOmi);
                    }
                }
            }
            return listSymbolWiseOMIdata;
        }

        private void ProxyHelper_PlugUnplugProxy(object sender, ProxyDataEventArgs e)
        {
            try
            {
                if (_liveFeedManager != null)
                {
                    _liveFeedManager.PlugUnplugProxy(e);
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

        public void RequestSnapshot(List<string> symbols, ApplicationConstants.SymbologyCodes symbologyCode, bool isGreekRequired, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, [Optional, DefaultParameterValue(true)] bool completeInfo)
        {
            try
            {
                ILiveFeedCallback subscriber = null;
                /// Workarround to fetch the instance of optionservermanager class...
                if (callBackClass == null)
                {
                    subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                }
                else
                {
                    subscriber = callBackClass;
                }

                ThreadInfo tinfo = new ThreadInfo();
                tinfo.ListOfSymbol = symbols;
                tinfo.SymbologyCode = symbologyCode;
                tinfo.IsGreekRequired = isGreekRequired;
                tinfo.Subscriber = subscriber;
                tinfo.CompleteInfo = completeInfo;

                RequestLiveFeedForSnapShotData(tinfo);
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

        public void RequestSMData(List<string> symbols, ApplicationConstants.SymbologyCodes symbologyCode)
        {
            try
            {
                ILiveFeedCallback subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();

                foreach (string symbol in symbols)
                {
                    UpdateSubscriberCache(symbol, false, subscriber);
                    Logger.LoggerWrite("RequestSMData Request Received on Pricing for Symbol : " + symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    _liveFeedManager.SnapshotSymbol(symbol, symbologyCode, false, false, true);
                }
                lock (_lockOnConnectionSubscribersList)
                {
                    if (_snapshotSubscribers.Count > 0 || _WithoutGreeksSnapshotSubscribers.Count > 0)
                    {
                        _snapShotTmr.Start();
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

        public void RequestSnapshot(List<fxInfo> fxSymbols, ApplicationConstants.SymbologyCodes symbologyCode, bool isGreekRequired, ILiveFeedCallback callBackClass, bool completeInfo)
        {
            try
            {
                ILiveFeedCallback subscriber = null;
                /// Workarround to fetch the instance of optionservermanager class...
                if (callBackClass == null)
                {
                    subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                }
                else
                {
                    subscriber = callBackClass;
                }

                ThreadInfo tinfo = new ThreadInfo();
                tinfo.ListOfFxSymbols = fxSymbols;
                tinfo.SymbologyCode = symbologyCode;
                tinfo.IsGreekRequired = isGreekRequired;
                tinfo.Subscriber = subscriber;
                tinfo.CompleteInfo = completeInfo;
                RequestLiveFeedForSnapShotDataForFx(tinfo);
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

        private void RequestLiveFeedForSnapShotData(Object obj)
        {
            try
            {
                ThreadInfo tinfo = obj as ThreadInfo;
                List<string> symbols = tinfo.ListOfSymbol;
                ApplicationConstants.SymbologyCodes symbologyCode = tinfo.SymbologyCode;
                bool isGreekRequired = tinfo.IsGreekRequired;
                ILiveFeedCallback subscriber = tinfo.Subscriber;
                bool completeInfo = tinfo.CompleteInfo;
                foreach (string symbol in symbols)
                {
                    UpdateSubscriberCache(symbol, isGreekRequired, subscriber);
                    if (_enableMarketDataLogging)
                    {
                        if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                        {
                            Logger.LoggerWrite("MarketData Snapshot Request Received on Pricing for Symbol : " + symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            InformationReporter.GetInstance.Write("MarketData Snapshot Request Received on Pricing for Symbol : " + symbol + " Time : " + DateTime.UtcNow);
                        }
                        else
                        {
                            Logger.LoggerWrite("MarketData Snapshot Request Received on Pricing for Symbol : " + symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                    }
                    //so that deividend is available for implied vol calculation,
                    //If underlying dividend not available then it would save the default value
                    OptionInputValuesCache.GetInstance.GetDividend(symbol);
                    OptionInputValuesCache.GetInstance.GetStockBorrowCost(symbol);
                    _liveFeedManager.SnapshotSymbol(symbol, symbologyCode, isGreekRequired, completeInfo);
                }
                lock (_lockOnConnectionSubscribersList)
                {
                    if (_snapshotSubscribers.Count > 0 || _WithoutGreeksSnapshotSubscribers.Count > 0)
                    {
                        _snapShotTmr.Start();
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
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        private void RequestLiveFeedForSnapShotDataForFx(Object obj)
        {
            try
            {
                ThreadInfo tinfo = obj as ThreadInfo;
                List<fxInfo> listFxReq = tinfo.ListOfFxSymbols;
                ApplicationConstants.SymbologyCodes symbologyCode = tinfo.SymbologyCode;
                bool isGreekRequired = tinfo.IsGreekRequired;
                ILiveFeedCallback subscriber = tinfo.Subscriber;
                bool completeInfo = tinfo.CompleteInfo;

                foreach (fxInfo info in listFxReq)
                {
                    UpdateSubscriberCache(info.PranaSymbol, isGreekRequired, subscriber);
                    if (_enableMarketDataLogging)
                    {
                        if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                        {
                            Logger.LoggerWrite("LiveFeed Snapshot Request Received on Pricing for Symbol : " + info.PranaSymbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                            InformationReporter.GetInstance.Write("LiveFeed Snapshot Request Received on Pricing for Symbol : " + info.PranaSymbol + " Time : " + DateTime.UtcNow);
                        }
                        else
                        {
                            Logger.LoggerWrite("LiveFeed Snapshot Request Received on Pricing for Symbol : " + info.PranaSymbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        }
                    }
                    // so that deividend is available for implied vol calculation, 
                    //If underlying dividend not available then it would save the default value
                    OptionInputValuesCache.GetInstance.GetDividend(info.PranaSymbol);
                    OptionInputValuesCache.GetInstance.GetStockBorrowCost(info.PranaSymbol);

                    _liveFeedManager.SnapshotSymbol(info, symbologyCode, isGreekRequired, completeInfo);
                }
                lock (_lockOnConnectionSubscribersList)
                {
                    if (_snapshotSubscribers.Count > 0 || _WithoutGreeksSnapshotSubscribers.Count > 0)
                    {
                        _snapShotTmr.Start();
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
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        private void UpdateSubscriberCache(string symbol, bool isGreekRequired, ILiveFeedCallback subscriber)
        {
            List<ILiveFeedCallback> subscribers = null;
            if (isGreekRequired == true)
            {
                lock (_lockOnConnectionSubscribersList)
                {
                    if (_snapshotSubscribers.ContainsKey(symbol))
                    {
                        subscribers = _snapshotSubscribers[symbol];
                        if (!subscribers.Contains(subscriber))
                        {
                            subscribers.Add(subscriber);
                        }
                    }
                    else
                    {
                        subscribers = new List<ILiveFeedCallback>();
                        subscribers.Add(subscriber);
                        _snapshotSubscribers.Add(symbol, subscribers);
                    }
                }
            }
            else
            {
                lock (_lockOnWithoutGreeksSnapshotSubscribersList)
                {
                    if (_WithoutGreeksSnapshotSubscribers.ContainsKey(symbol))
                    {
                        subscribers = _WithoutGreeksSnapshotSubscribers[symbol];
                        if (!subscribers.Contains(subscriber))
                        {
                            subscribers.Add(subscriber);
                        }
                    }
                    else
                    {
                        subscribers = new List<ILiveFeedCallback>();
                        subscribers.Add(subscriber);
                        _WithoutGreeksSnapshotSubscribers.Add(symbol, subscribers);
                    }
                }
            }
        }

        public SymbolData GetDynamicSymbolData(string symbol)
        {
            SymbolData data = null;
            try
            {
                if (_liveFeedManager != null)
                {
                    _liveFeedManager.CheckAndAdviseSymbol(symbol);
                }

                lock (_lockOnPricingCache)
                {
                    _pricingCache.TryGetValue(symbol, out data);
                }
                if (data != null)
                {
                    if (_enableMarketData_ContinuousDataLogging)
                    {
                        Logger.LoggerWrite(DateTime.Now.ToLongTimeString() + " Symbol= " + symbol + " Last= " + data.LastPrice + " Bid= " + data.Bid + " Ask= " + data.Ask + " Change= " + data.Change + " Low= " + data.Low + " High= " + data.High + " Currency= " + data.CurencyCode, LoggingConstants.LIVEDATA_LOGGING);
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
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return data;
        }

        /// <summary>
        // returns the live feed data for symbol. Made to handle the special cases for Fx and FX forward as in those cases the prana symbol is different from esignal symbol.
        /// </summary>
        public SymbolData GetDynamicSymbolData(string Symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode)
        {
            SymbolData data = null;
            try
            {
                if (_liveFeedManager != null)
                {
                    _liveFeedManager.CheckAndAdviseSymbolForFX(Symbol, fromCurrency, toCurrency, categoryCode);
                }

                lock (_lockOnPricingCache)
                {
                    _pricingCache.TryGetValue(Symbol, out data);
                }
                if (data != null)
                {
                    if (_enableMarketData_ContinuousDataLogging)
                    {
                        Logger.LoggerWrite(DateTime.Now.ToLongTimeString() + " Symbol= " + Symbol + " Last= " + data.LastPrice + " Bid= " + data.Bid + " Ask= " + data.Ask + " Change= " + data.Change + " Low= " + data.Low + " High= " + data.High + " Currency= " + data.CurencyCode, LoggingConstants.LIVEDATA_LOGGING);
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
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return data;
        }

        public void Subscribe()
        {
            try
            {
                ILiveFeedCallback subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                lock (_lockOnLiveFeedConnectionSubscriberslist)
                {
                    _liveFeedConnectionSubscriberslist.Add(subscriber);
                }

                if (IsLiveFeedActive)
                {
                    LiveFeedConnected(null, new EventArgs<bool>(true));
                }
                else
                {
                    LiveFeedDisconnected(null, new EventArgs<bool>(false));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public void UnSubscribe([Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass)
        {
            try
            {
                ILiveFeedCallback subscriber;
                if (callBackClass != null)
                {
                    subscriber = callBackClass;
                    lock (_lockOnConnectionSubscribersList)
                    {
                        foreach (KeyValuePair<string, List<ILiveFeedCallback>> subObj in _snapshotSubscribers)
                        {
                            if (subObj.Value.Contains(subscriber))
                            {
                                subObj.Value.Remove(subscriber);
                            }
                        }
                    }
                }
                else
                {
                    subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                    lock (_lockOnLiveFeedConnectionSubscriberslist)
                    {
                        _liveFeedConnectionSubscriberslist.Remove(subscriber);
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
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public event EventHandler<EventArgs<bool>> LiveFeedConnectionStatusChanged;

        private void _snapShotDataTmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                OnSnapshotTimerElapsed();
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

        private void OnSnapshotTimerElapsed()
        {
            try
            {
                SymbolData greeksData = null;
                List<string> greekCurrentSymbols = new List<string>();

                SymbolData nonGreeksData = null;
                List<string> nonGreekCurrentSymbols = new List<string>();

                _snapShotTmr.Stop();
                lock (_lockOnConnectionSubscribersList)
                {
                    if (_snapshotSubscribers.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<ILiveFeedCallback>> symbolSubscribers in _snapshotSubscribers)
                        {
                            greeksData = null;
                            SendSnapshotSubscribers(ref greeksData, greekCurrentSymbols, symbolSubscribers);
                        }

                        foreach (string symbol in greekCurrentSymbols)
                        {
                            if (_snapshotSubscribers.ContainsKey(symbol))
                            {
                                _snapshotSubscribers.Remove(symbol);
                            }
                        }
                    }
                }
                //added lock on withoutgreek snapshot subscribers cache, PRANA-12881
                lock (_lockOnWithoutGreeksSnapshotSubscribersList)
                {
                    if (_WithoutGreeksSnapshotSubscribers.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<ILiveFeedCallback>> symbolSubscribers in _WithoutGreeksSnapshotSubscribers)
                        {
                            nonGreeksData = null;
                            SendSnapshotSubscribers(ref nonGreeksData, nonGreekCurrentSymbols, symbolSubscribers);
                        }

                        foreach (string symbol in nonGreekCurrentSymbols)
                        {
                            if (_WithoutGreeksSnapshotSubscribers.ContainsKey(symbol))
                            {
                                _WithoutGreeksSnapshotSubscribers.Remove(symbol);
                            }
                        }
                    }
                }
                lock (_clientCallBackTTPTT)
                {
                    SendSnaphotResponseToClient(_clientCallBackTTPTT);
                }

                try
                {
                    TimeSpan lastOptionChainProcessInterval = DateTime.Now - _lastOptionChainDataTime;
                    if (lastOptionChainProcessInterval.TotalMilliseconds >= _optionChainDataInterval)
                    {
                        _lastOptionChainDataTime = DateTime.Now;

                        Dictionary<string, List<ILiveFeedCallback>> optionSubscribers = new Dictionary<string, List<ILiveFeedCallback>>();

                        lock (_optionChainSubscriberOptionMappings)
                        {
                            foreach (KeyValuePair<ILiveFeedCallback, List<string>> kvp in _optionChainSubscriberOptionMappings)
                            {
                                foreach (string option in kvp.Value)
                                {
                                    if (!optionSubscribers.ContainsKey(option))
                                        optionSubscribers.Add(option, new List<ILiveFeedCallback>() { kvp.Key });
                                    else
                                        optionSubscribers[option].Add(kvp.Key);
                                }
                            }
                        }

                        if (optionSubscribers.Count > 0)
                        {
                            if (SendSnaphotResponseToClient(optionSubscribers))
                                lock (_optionChainRecentSubscribers)
                                    _optionChainRecentSubscribers.Clear();
                        }
                    }
                    else if (_optionChainRecentSubscribers.Count > 0)
                    {
                        Dictionary<string, List<ILiveFeedCallback>> optionSubscribers = new Dictionary<string, List<ILiveFeedCallback>>();

                        lock (_optionChainSubscriberOptionMappings)
                        {
                            foreach (ILiveFeedCallback subscriber in _optionChainRecentSubscribers)
                            {
                                foreach (string option in _optionChainSubscriberOptionMappings[subscriber])
                                {
                                    if (!optionSubscribers.ContainsKey(option))
                                        optionSubscribers.Add(option, new List<ILiveFeedCallback>() { subscriber });
                                    else
                                        optionSubscribers[option].Add(subscriber);
                                }
                            }
                        }

                        if (optionSubscribers.Count > 0)
                        {
                            if (SendSnaphotResponseToClient(optionSubscribers))
                                lock (_optionChainRecentSubscribers)
                                    _optionChainRecentSubscribers.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                }

                if (_snapshotSubscribers.Count > 0 || _WithoutGreeksSnapshotSubscribers.Count > 0 || _clientCallBackTTPTT.Count > 0 || _optionChainSubscribers.Count > 0)
                {
                    _snapShotTmr.Start();
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
        /// Sends snapshot response to all the subscribers
        /// </summary>
        /// <param name="symbolDataSubscriberMapping">Symbol and their Subscribers mapping</param>
        /// <returns>True - All symbols data was sent</returns>
        private bool SendSnaphotResponseToClient(Dictionary<string, List<ILiveFeedCallback>> symbolDataSubscriberMapping)
        {
            bool symbolDataNotFound = false;

            try
            {
                Parallel.ForEach(symbolDataSubscriberMapping, (KeyValuePair<string, List<ILiveFeedCallback>> kvp) =>
                {
                    SymbolData data = null;
                    lock (_lockOnPricingCache)
                    {
                        _pricingCache.TryGetValue(kvp.Key, out data);
                    }

                    if (data == null)
                    {
                        List<fxInfo> fxInfo = _liveFeedManager.GetTickerSymbolFromFX(kvp.Key);
                        fxInfo inf = null;
                        if (fxInfo != null)
                        {
                            lock (_lockOnPricingCache)
                            {
                                for (int i = 0; i < fxInfo.Count; i++)
                                {
                                    inf = fxInfo[i];
                                    _pricingCache.TryGetValue(inf.PranaSymbol, out data);
                                    if (data != null)
                                    {
                                        data = DeepCopyHelper.Clone(data);
                                        break;
                                    }
                                }
                            }

                            if (data != null && inf != null)
                            {
                                data.Symbol = kvp.Key;
                                if (inf.ConversionRate.ConversionMethod == Operator.D)
                                {
                                    data.SelectedFeedPrice = data.SelectedFeedPrice == 0d ? 0d : 1 / data.SelectedFeedPrice;
                                    data.LastPrice = data.LastPrice == 0d ? 0d : 1 / data.LastPrice;
                                    data.Ask = data.Ask == 0d ? 0d : 1 / data.Ask;
                                    data.Bid = data.Bid == 0d ? 0d : 1 / data.Bid;
                                    data.Mid = data.Mid == 0d ? 0d : 1 / data.Mid;
                                    data.iMid = data.iMid == 0d ? 0d : 1 / data.iMid;
                                    data.High = data.High == 0d ? 0d : 1 / data.High;
                                    data.Low = data.Low == 0d ? 0d : 1 / data.Low;
                                    data.Previous = data.Previous == 0d ? 0d : 1 / data.Previous;
                                }
                            }
                        }
                    }

                    if (data != null)
                    {
                        List<int> faultCallbackIndexes = new List<int>();

                        Parallel.For(0, kvp.Value.Count, (index) =>
                        {
                            try
                            {
                                ILiveFeedCallback callBack = kvp.Value[index];
                                callBack.SnapshotResponse(data);
                            }
                            catch (Exception ex)
                            {
                                faultCallbackIndexes.Add(index);
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            }
                        });

                        if (faultCallbackIndexes.Count > 0)
                            foreach (int index in faultCallbackIndexes)
                                kvp.Value.RemoveAt(index);
                    }
                    else
                        symbolDataNotFound = true;
                });
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }

            return !symbolDataNotFound;
        }

        #region Sending Snapshot SymbolData to Compliance once

        List<string> _requestedSymbol = new List<string>();

        /// <summary>
        /// Snapshot from TT for Compliance
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void RequestSnapshotForCompliance(List<string> requestedSymbols)
        {
            try
            {
                foreach (string symbol in requestedSymbols)
                {
                    if (!_requestedSymbol.Contains(symbol))
                        _requestedSymbol.Add(symbol);
                }
                _complianceSnapshotTmr.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sending Snapshot SymbolData to Compliance once
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _complianceSnapshotTmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (_lockOnComplianceSnapshot)
                {
                    List<string> requestedSymbols = DeepCopyHelper.Clone(_requestedSymbol);
                    foreach (string symbol in requestedSymbols)
                    {
                        SymbolData data = null;
                        lock (_lockOnPricingCache)
                            _pricingCache.TryGetValue(symbol, out data);

                        if (data != null)
                        {
                            if (data.SelectedFeedPrice != 0 || data.Ask != 0 || data.Bid != 0)
                            {
                                AmqpHelper.SendObject(data, "SymbolData", null);
                                InformationReporter.GetInstance.Write("Prices sent to Compliance for Symbol: " + data.Symbol + ", SelectedFeedPrice: " + data.SelectedFeedPrice + ", Ask: " + data.Ask + ", Bid: " + data.Bid);
                            }

                            if (_requestedSymbol.Contains(symbol))
                                _requestedSymbol.Remove(symbol);
                        }
                    }

                    if (_requestedSymbol != null && _requestedSymbol.Count == 0)
                    {
                        _complianceSnapshotTmr.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Snapshot from TT for Compliance
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void RemoveSnapshotForCompliance(List<string> symbolsToBeRemoved)
        {
            try
            {
                List<string> requestedSymbols = DeepCopyHelper.Clone(symbolsToBeRemoved);
                foreach (string symbol in requestedSymbols)
                {
                    if (_requestedSymbol.Contains(symbol))
                        _requestedSymbol.Remove(symbol);
                }
                _complianceSnapshotTmr.Stop();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        private void SendSnapshotSubscribers(ref SymbolData data, List<string> currentSymbols, KeyValuePair<string, List<ILiveFeedCallback>> symbolSubscribers)
        {
            lock (_lockOnPricingCache)
            {
                if (_pricingCache.ContainsKey(symbolSubscribers.Key))
                {
                    data = _pricingCache[symbolSubscribers.Key];

                    if (!currentSymbols.Contains(data.Symbol))
                        currentSymbols.Add(data.Symbol);
                }
                else
                {
                    var symbols = _pricingCache.Where(s => s.Value.FactSetSymbol == symbolSubscribers.Key).ToList();
                    if (symbols.Count != 0)
                    {
                        data = symbols[0].Value;

                        if (!currentSymbols.Contains(data.FactSetSymbol))
                            currentSymbols.Add(data.FactSetSymbol);
                    }

                    symbols = _pricingCache.Where(s => s.Value.ActivSymbol == symbolSubscribers.Key).ToList();
                    if (symbols.Count != 0)
                    {
                        data = symbols[0].Value;

                        if (!currentSymbols.Contains(data.FactSetSymbol))
                            currentSymbols.Add(data.ActivSymbol);
                    }

                    symbols = _pricingCache.Where(s => s.Value.BloombergSymbol == symbolSubscribers.Key).ToList();
                    if (symbols.Count != 0)
                    {
                        data = symbols[0].Value;

                        if (!currentSymbols.Contains(data.BloombergSymbol))
                            currentSymbols.Add(data.BloombergSymbol);
                    }

                    symbols = _pricingCache.Where(s => s.Value.BloombergSymbolWithExchangeCode == symbolSubscribers.Key).ToList();
                    if (symbols.Count != 0)
                    {
                        data = symbols[0].Value;

                        if (!currentSymbols.Contains(data.BloombergSymbolWithExchangeCode))
                            currentSymbols.Add(data.BloombergSymbolWithExchangeCode);
                    }
                }
            }

            if (data != null)
            {
                SubscriberInfo subInfo = new SubscriberInfo();
                subInfo.SubscribersList.AddRange(symbolSubscribers.Value);
                subInfo.Data = data;

                ThreadPool.QueueUserWorkItem(new WaitCallback(SnapshotResponse), subInfo);
            }
        }

        private void SnapshotResponse(object obj)
        {
            try
            {
                SubscriberInfo subInfo = obj as SubscriberInfo;
                List<ILiveFeedCallback> subscribersList = subInfo.SubscribersList;
                SymbolData data = subInfo.Data;

                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: PricingServiceManager.SnapshotResponse() sending started from PricingService2: {0}, Time: {1}", data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                }

                if (_enableMarketDataLogging)
                {
                    if (Prana.BusinessObjects.UserSettingConstants.IsDebugModeEnabled)
                    {
                        Logger.LoggerWrite("LiveFeed Snapshot Response Received from Pricing for Symbol : " + data.Symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                        InformationReporter.GetInstance.Write("LiveFeed Snapshot Response Received from Pricing for Symbol : " + data.Symbol + " Time : " + DateTime.UtcNow);
                    }
                    else
                    {
                        Logger.LoggerWrite("LiveFeed Snapshot Response Received from Pricing for Symbol : " + data.Symbol + " Time : " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    }
                }

                Parallel.ForEach(subscribersList, suscriber =>
                {
                    try
                    {
                        try
                        {
                            if (subInfo.SaveSymbolInDb && subInfo.Data.AUECID <= 0)
                            {
                                SymbolData symbolData = subInfo.Data;
                                GetAUECFromMapping(ref symbolData);
                                subInfo.Data = symbolData;
                            }
                            suscriber.SnapshotResponse(subInfo.Data, new SnapshotResponseData() { ShouldSaveInDb = subInfo.SaveSymbolInDb });
                        }
                        catch (ObjectDisposedException)
                        {
                            //continue;
                        }
                        // To avoid channel communication exceptions
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-3020
                        catch (CommunicationException)
                        {
                            //continue;
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
                });

                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: PricingServiceManager.SnapshotResponse() sending completed from PricingService2: {0}, Time: {1}", data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
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

        public List<UserOptModelInput> GetOMIDataFromCache(bool fetchZeroPositionData, string symbols = "")
        {
            List<UserOptModelInput> listOMIData = null;

            try
            {
                listOMIData = OptionModelUserInputCache.GetOMICollection(fetchZeroPositionData, symbols);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return listOMIData;
        }

        /// <summary>
        /// GetOMIDataFromCacheForCompliance
        /// </summary>
        /// <param name="fetchZeroPositionData"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public static List<UserOptModelInput> GetOMIDataFromCacheForCompliance()
        {
            List<UserOptModelInput> listOMIData = null;
            try
            {
                listOMIData = new List<UserOptModelInput>(((Dictionary<string, UserOptModelInput>)OptionModelUserInputCache.Clone()).Values);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return listOMIData;
        }

        public int SaveRunUploadFileDataForOMI(List<UserOptModelInput> OMIImportCollection)
        {
            try
            {
                List<UserOptModelInput> OMICollection = ApplyImportChecks(OMIImportCollection);
                string xml = XMLUtilities.SerializeToXML(OMICollection);
                int rowsupdated = XMLSaveManager.SaveThroughXML("[P_SaveOptionModelUserData_Import]", xml);
                if (rowsupdated > 0)
                {
                    OptionModelUserInputCache.UpdateCacheFromImportOMICollection(OMICollection);
                }
                return rowsupdated;
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

        public List<UserOptModelInput> ApplyImportChecks(List<UserOptModelInput> OMIImportCollection)
        {
            try
            {
                foreach (UserOptModelInput userOMI in OMIImportCollection)
                {
                    UserOptModelInput OMIData = OptionModelUserInputCache.GetUserOMIData(userOMI.Symbol);
                    if (OMIData != null)
                    {
                        //If Column value is Zero then Old value will be assign.
                        if (userOMI.Volatility.Equals(0))
                        {
                            userOMI.Volatility = OMIData.Volatility * 100;
                            userOMI.VolatilityUsed = OMIData.VolatilityUsed;
                        }
                        else
                            userOMI.VolatilityUsed = true;

                        if (userOMI.IntRate.Equals(0))
                        {
                            userOMI.IntRate = OMIData.IntRate * 100;
                            userOMI.IntRateUsed = OMIData.IntRateUsed;
                        }
                        else
                            userOMI.IntRateUsed = true;

                        if (userOMI.Dividend.Equals(0))
                        {
                            userOMI.Dividend = OMIData.Dividend * 100;
                            userOMI.DividendUsed = OMIData.DividendUsed;
                        }
                        else
                            userOMI.DividendUsed = true;

                        if (userOMI.StockBorrowCost.Equals(0))
                        {
                            userOMI.StockBorrowCost = OMIData.StockBorrowCost * 100;
                            userOMI.StockBorrowCostUsed = OMIData.StockBorrowCostUsed;
                        }
                        else
                            userOMI.StockBorrowCostUsed = true;

                        if (userOMI.Delta.Equals(0))
                        {
                            userOMI.Delta = OMIData.Delta;
                            userOMI.DeltaUsed = OMIData.DeltaUsed;
                        }
                        else
                            userOMI.DeltaUsed = true;

                        if (userOMI.SharesOutstanding.Equals(0))
                        {
                            userOMI.SharesOutstanding = OMIData.SharesOutstanding;
                            userOMI.SharesOutstandingUsed = OMIData.SharesOutstandingUsed;
                        }
                        else
                            userOMI.SharesOutstandingUsed = true;


                        if (userOMI.LastPrice.Equals(0))
                        {
                            userOMI.LastPrice = OMIData.LastPrice;
                            userOMI.LastPriceUsed = OMIData.LastPriceUsed;
                        }
                        else
                            userOMI.LastPriceUsed = true;

                        if (userOMI.ForwardPoints.Equals(0))
                        {
                            userOMI.ForwardPoints = OMIData.ForwardPoints;
                            userOMI.ForwardPointsUsed = OMIData.ForwardPointsUsed;
                        }
                        else
                            userOMI.ForwardPointsUsed = true;

                        userOMI.TheoreticalPriceUsed = OMIData.TheoreticalPriceUsed;
                        userOMI.HistoricalVolUsed = OMIData.HistoricalVolUsed;
                    }
                    else
                    {
                        if (!userOMI.Volatility.Equals(0))
                            userOMI.VolatilityUsed = true;

                        if (!userOMI.IntRate.Equals(0))
                            userOMI.IntRateUsed = true;

                        if (!userOMI.Dividend.Equals(0))
                            userOMI.DividendUsed = true;

                        if (!userOMI.StockBorrowCost.Equals(0))
                            userOMI.StockBorrowCostUsed = true;

                        if (!userOMI.Delta.Equals(0))
                            userOMI.DeltaUsed = true;

                        if (!userOMI.SharesOutstanding.Equals(0))
                            userOMI.SharesOutstandingUsed = true;

                        if (!userOMI.LastPrice.Equals(0))
                            userOMI.LastPriceUsed = true;

                        if (!userOMI.ForwardPoints.Equals(0))
                            userOMI.ForwardPointsUsed = true;
                    }

                    //Checks for same pair values like from Historical Volatility and User Volatility only one of them checked, Both can not be checked together.
                    if (userOMI.VolatilityUsed)
                        userOMI.HistoricalVolUsed = false;

                    if (userOMI.LastPriceUsed && !userOMI.ForwardPointsUsed)
                    {
                        userOMI.ForwardPointsUsed = false;
                        userOMI.TheoreticalPriceUsed = false;
                    }

                    if (userOMI.ForwardPointsUsed && !userOMI.LastPriceUsed)
                    {
                        userOMI.LastPriceUsed = false;
                        userOMI.TheoreticalPriceUsed = false;
                    }

                    if (userOMI.LastPriceUsed && userOMI.ForwardPointsUsed)
                    {
                        userOMI.ForwardPointsUsed = false;
                        userOMI.TheoreticalPriceUsed = false;
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
            return OMIImportCollection;
        }

        public bool SaveOMIData(DataSet ds, List<string> modifiedSymbolsList)
        {
            try
            {
                OptionModelDataManager.SaveOptionModelUserDataToDB(ds.Tables[0]);
                List<ProxyDataEventArgs> listProxySymbols = new List<ProxyDataEventArgs>();
                OptionModelUserInputCache.UpdateOMICache(ds.Tables[0], ref listProxySymbols);
                if (listProxySymbols != null && listProxySymbols.Count > 0)
                {
                    foreach (ProxyDataEventArgs e in listProxySymbols)
                    {
                        _liveFeedManager.PlugUnplugProxy(e);
                    }
                }
                if (IsEsperEngineConnected)
                {
                    Thread.Sleep(1000); //Sometimes cache updation takes time.
                    SendOMIDataToEsperEngine(modifiedSymbolsList);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return true;
        }

        public void RestartLiveFeed()
        {
            try
            {
                if (_liveFeedManager != null)
                    _liveFeedManager.ConnectLiveFeed();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        // underlying symbol is mandatory
        // if do not want to provide month please enter string.empty for month
        // if don not want to provide lowerstrike or upperstrike please provide the values
        // zero for them.
        public async System.Threading.Tasks.Task SubscribeStaticOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                ILiveFeedCallback subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();

                lock (_optionChainSubscribers)
                {
                    if (_optionChainSubscribers.ContainsKey(underlyingSymbol))
                    {
                        List<ILiveFeedCallback> subscribers = _optionChainSubscribers[underlyingSymbol];
                        if (!subscribers.Contains(subscriber))
                        {
                            subscribers.Add(subscriber);
                        }
                    }
                    else
                    {
                        _optionChainSubscribers.Add(underlyingSymbol, new List<ILiveFeedCallback>() { subscriber });
                    }
                }
                if (_liveFeedManager != null)
                {
                    _liveFeedManager.GetStaticOptionChain(underlyingSymbol, optionChainFilter);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        // underlying symbol is mandatory
        public async System.Threading.Tasks.Task UnsubscribeStaticOptionChain(string underlyingSymbol)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                ILiveFeedCallback subscriber = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();

                lock (_optionChainSubscribers)
                {
                    if (_optionChainSubscribers.ContainsKey(underlyingSymbol))
                    {
                        if (_optionChainSubscribers[underlyingSymbol].Contains(subscriber))
                        {
                            if (_liveFeedManager != null)
                            {
                                RemoveMultipleOptions(underlyingSymbol, subscriber);
                            }

                            _optionChainSubscribers[underlyingSymbol].Remove(subscriber);
                            if (_optionChainSubscriberOptionMappings.ContainsKey(subscriber))
                                _optionChainSubscriberOptionMappings.Remove(subscriber);
                        }

                        if (_optionChainSubscribers[underlyingSymbol].Count == 0)
                        {
                            _optionChainSubscribers.Remove(underlyingSymbol);
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
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public void InitializeMarkPricesCache(string auecString)
        {
            try
            {
                //Initialize Mark Prices Cache for nirvana release
                MarkCacheManager.InitializeMarkPricesCache(auecString);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public void AdjustMarkPriceByTodaysSplitFactor(string todayAUECString, bool isUpdated, Dictionary<int, DateTime> dictMarketEndTime)
        {
            try
            {
                MarkCacheManager.AdjustMarkPriceByTodaysSplitFactor(todayAUECString, isUpdated);
                PricingUtilityCache.UpdateMarketEndTimeCache(dictMarketEndTime);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public double GetMarkPriceForDateAndSymbol(DateTime date, string symbol)
        {
            try
            {
                return MarkCacheManager.GetMarkPriceForDateAndSymbol(date, symbol);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        public Dictionary<Tuple<string, DateTime>, double> GetMarkPricesForOptExpiry(List<Tuple<string, DateTime>> symbolDatePairs)
        {
            try
            {
                return MarkCacheManager.GetMarkPricesForOptExpiry(symbolDatePairs);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the markprices for future symbols
        /// </summary>
        /// <param name="dictSymbolWithSettlementDate">Used for passing dictionary of string,date type.</param>
        /// <returns></returns>
        public Dictionary<string, double> GetMarkPricesForSymbolAndExactDate(Dictionary<string, DateTime> dictSymbolWithSettlementDate)
        {
            try
            {
                return MarkCacheManager.GetMarkPricesForSymbolAndExactDate(dictSymbolWithSettlementDate);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the mark prices for symbol on exact date.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public double GetMarkPricesForSymbolOnExactDate(string symbol, DateTime date)
        {
            try
            {
                return MarkCacheManager.GetMarkPricesForSymbolOnExactDate(symbol, date);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return double.MinValue;
        }

        public Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceForDate(DateTime date, int dateMethodology, bool isFxFxForward)
        {
            try
            {
                return MarkCacheManagerNew.GetMarkPriceForDate(date, dateMethodology, isFxFxForward, _getSameDayClosedDataOnDV);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the collateral price date wise.
        /// </summary>
        /// <param name="dateSelected">The date selected.</param>
        /// <param name="dateMethodology">The date methodology.</param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public DataTable GetCollateralPriceDateWise(DateTime dateSelected, int dateMethodology)
        {
            try
            {
                return MarkCacheManager.GetCollateralPriceDateWise(dateSelected, dateMethodology, _getSameDayClosedDataOnDV, isOnlyFixedIncomeSymbols);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        public bool GetSameDayClosedDataConfigValue()
        {
            try
            {
                return _getSameDayClosedDataOnDV;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return false;
        }

        public int SaveMarkPrices(DataTable dt, bool isAutoApproved)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveMarkPrices(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        /// <summary>
        /// Saves the collateral values.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public int SaveCollateralValues(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveCollateralValues(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        public int SaveBeta(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveBeta(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        public int SaveOutStandings(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveOutStandings(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        public int SaveForexRate(DataTable dt)
        {
            int rowsEffected = 0;
            try
            {
                return rowsEffected = MarkCacheManager.SaveForexRate(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return rowsEffected;
        }

        public int SaveStandardCurrencyPair(DataTable dtCurrencyPair)
        {
            int rowsEffected = 0;
            try
            {
                return rowsEffected = MarkCacheManagerNew.SaveStandardCurrencyPair(dtCurrencyPair);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return rowsEffected;
        }

        public int SaveDividendYield(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveDividendYield(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        public int SaveVolatility(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveVolatility(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        /// <summary>
        /// Saves the vwap.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public int SaveVWAP(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SaveVWAP(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }

        public int SavePerformanceNumberValues(DataTable dt)
        {
            int rowsAffected;
            try
            {
                return rowsAffected = MarkCacheManager.SavePerformanceNumbers(dt);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return 0;
        }
        #endregion IPricingService Members

        public Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceForDateRangeWithAccounts(string xmlAccount, DateTime startDate, DateTime endDate, int dateMethodology, bool isFxFxForward, int filter)
        {
            try
            {
                return MarkCacheManagerNew.GetMarkPriceForDateRange(xmlAccount, startDate, endDate, dateMethodology, isFxFxForward, filter);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
            return null;
        }

        public void GetMarkPriceForAccountSymbolCollection(ref Dictionary<int, Dictionary<string, double>> dictAccountSymbolCollection)
        {
            MarkCacheManagerNew.GetMarkPriceForAccountSymbolCollection(ref dictAccountSymbolCollection);
        }

        /// <summary>
        /// added by: Bharat Raturi
        /// </summary>
        /// <param name="startDate">start date of the mark price data </param>
        /// <param name="endDate">End date of the mark price data</param>
        /// <returns>datatable holding the data</returns>
        public DataTable GetUnapprovedMarkPrices(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = MarkCacheManagerNew.GetUnapprovedMarkPrices(startDate, endDate);
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
            return dt;
        }

        /// <summary>
        /// Get the Request Field Data from DB
        /// </summary>
        public DataSet GetSAPIRequestFieldData(string requestField)
        {
            try
            {
                return MarkCacheManagerNew.GetSAPIRequestFieldData(requestField);
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
        public void SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField)
        {
            try
            {
                MarkCacheManagerNew.SaveSAPIRequestFieldData(saveDataSetTemp, requestField);
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

        public int ApproveMarkPrices(DataTable dtMarkPrice)
        {
            int i = 0;
            try
            {
                i = MarkCacheManagerNew.ApproveMarkPrices(dtMarkPrice);
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
            return i;
        }

        public int RescindMarkPrices(DataTable dtMarkPrice)
        {
            int i = 0;
            try
            {
                i = MarkCacheManagerNew.RescindMarkPrices(dtMarkPrice);
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
            return i;
        }

        public List<string> GetAdvicedSymbols()
        {
            return _liveFeedManager.GetAdvicedSymbols();
        }

        public void CheckAndAdviseSymbol(string symbol)
        {
            try
            {
                if (_liveFeedManager != null)
                    _liveFeedManager.CheckAndAdviseSymbol(symbol);
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

        public void CheckAndAdviseSymbolBulk(List<string> symbols)
        {
            try
            {
                if (_liveFeedManager != null)
                {
                    foreach (string symbol in symbols)
                    {
                        _liveFeedManager.CheckAndAdviseSymbol(symbol);
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
        }

        public void CheckAndAdviseSymbolForFX(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode)
        {
            try
            {
                if (_liveFeedManager != null)
                {
                    _liveFeedManager.CheckAndAdviseSymbolForFX(symbol, fromCurrency, toCurrency, categoryCode);
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

        public void DeleteAdvisedSymbol(string symbol)
        {
            try
            {
                if (_liveFeedManager != null)
                {
                    _liveFeedManager.DeleteAdvisedSymbol(symbol);
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

        public void DeleteSymbolFromPI(string symbol)
        {
            try
            {
                OptionModelDataManager.DeleteSymbolfromPI(symbol);
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
        /// Advises the symbol for TT and PTT.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void RequestSymbol_TTandPTT(string symbol, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, bool isSnapshot)
        {
            try
            {
                if (callBackClass == null)
                {
                    callBackClass = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                }
                if (!isSnapshot)
                {
                    _liveFeedManager.CheckAndAdviseSymbol(symbol, true);

                    if (callBackClass != null)
                    {
                        lock (_clientCallBackTTPTT)
                        {
                            if (!_clientCallBackTTPTT.ContainsKey(symbol))
                                _clientCallBackTTPTT.Add(symbol, new List<ILiveFeedCallback>());
                            if (!_clientCallBackTTPTT[symbol].Contains(callBackClass))
                                _clientCallBackTTPTT[symbol].Add(callBackClass);
                        }
                        _snapShotTmr.Start();
                    }
                }
                else
                    RequestSnapshot(new List<string>() { symbol }, ApplicationConstants.SymbologyCodes.TickerSymbol, false, callBackClass, true);
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
        /// This method is used to request live feed signal for the symbols set.
        /// </summary>
        /// <param name="symbolSet">HashSet<string></param>
        /// <param name="callBackClass">ILiveFeedCallback</param>
        /// <param name="isSnapshot">bool</param>
        public void RequestMultipleSymbols(List<string> symbolSet, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, bool isSnapshot)
        {
            try
            {
                if (callBackClass == null)
                {
                    callBackClass = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                }

                if (!isSnapshot)
                {
                    foreach (string symbol in symbolSet)
                    {
                        _liveFeedManager.CheckAndAdviseSymbol(symbol, true);
                        if (callBackClass != null)
                        {
                            lock (_clientCallBackTTPTT)
                            {
                                if (!_clientCallBackTTPTT.ContainsKey(symbol))
                                    _clientCallBackTTPTT.Add(symbol, new List<ILiveFeedCallback>());
                                _clientCallBackTTPTT[symbol].Add(callBackClass);
                            }
                        }
                    }
                    _snapShotTmr.Start();
                }
                else
                    RequestSnapshot(symbolSet, ApplicationConstants.SymbologyCodes.TickerSymbol, false, callBackClass, true);
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
        /// This method is used to request live feed signal for the symbols set.
        /// </summary>
        public void RequestMultipleOptions(string underlyingSymbol, HashSet<string> optionsSet, ILiveFeedCallback callBackClass)
        {
            try
            {
                if (callBackClass != null)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Sending advice request for options of {0}: {1}", underlyingSymbol, string.Join(", ", optionsSet)), LoggingConstants.OptionChain_Logging, 1, 1, TraceEventType.Verbose);
                    Parallel.ForEach(optionsSet, (option) => _liveFeedManager.CheckAndAdviseOptionForOptionChain(underlyingSymbol, option));
                    _snapShotTmr.Start();
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
        /// Remove the symbol advised for TT and PTT.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public void RemoveSymbol_TTandPTT(string symbol, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass)
        {
            try
            {
                if (callBackClass == null)
                    callBackClass = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                if (callBackClass != null)
                {
                    lock (_clientCallBackTTPTT)
                    {
                        if (_clientCallBackTTPTT.ContainsKey(symbol))
                        {
                            _clientCallBackTTPTT[symbol].Remove(callBackClass);
                            if (_clientCallBackTTPTT[symbol].Count == 0)
                            {
                                _clientCallBackTTPTT.Remove(symbol);
                                if (!(_snapshotSubscribers.ContainsKey(symbol) || _WithoutGreeksSnapshotSubscribers.ContainsKey(symbol)))
                                {
                                    _liveFeedManager.DeleteAdvisedSymbol(symbol, true);
                                }
                            }
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
        }

        /// <summary>
        /// This method is used to remove symbols from live feed symbol list.
        /// </summary>
        /// <param name="symbolSet">HashSet<string></param>
        public void RemoveMultipleSymbols(List<string> symbolSet, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass)
        {
            try
            {
                if (callBackClass == null)
                    callBackClass = OperationContext.Current.GetCallbackChannel<ILiveFeedCallback>();
                if (callBackClass != null)
                {
                    lock (_clientCallBackTTPTT)
                    {
                        foreach (string symbol in symbolSet)
                        {
                            if (_clientCallBackTTPTT.ContainsKey(symbol))
                            {
                                _clientCallBackTTPTT[symbol].Remove(callBackClass);
                                if (_clientCallBackTTPTT[symbol].Count == 0)
                                {
                                    _clientCallBackTTPTT.Remove(symbol);
                                    if (!(_snapshotSubscribers.ContainsKey(symbol) || _WithoutGreeksSnapshotSubscribers.ContainsKey(symbol)))
                                    {
                                        _liveFeedManager.DeleteAdvisedSymbol(symbol, true);
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

        /// <summary>
        /// This method is used to remove symbols from live feed symbol list.
        /// </summary>
        /// <param name="symbolSet">HashSet<string></param>
        public void RemoveMultipleOptions(string underlyingSymbol, ILiveFeedCallback callBackClass)
        {
            try
            {
                if (callBackClass != null)
                {
                    List<string> options = null;

                    lock (_optionChainSubscriberOptionMappings)
                    {
                        if (_optionChainSubscriberOptionMappings.ContainsKey(callBackClass))
                            options = new List<string>(_optionChainSubscriberOptionMappings[callBackClass]);
                    }

                    if (options != null)
                    {
                        foreach (ILiveFeedCallback subscriber in _optionChainSubscribers[underlyingSymbol])
                        {
                            if (subscriber != callBackClass && _optionChainSubscriberOptionMappings.ContainsKey(subscriber))
                                options = options.Except(_optionChainSubscriberOptionMappings[subscriber]).ToList();
                        }

                        foreach (string option in options)
                        {
                            _liveFeedManager.DeleteOptionChainAdvisedOption(underlyingSymbol, option);
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
        /// Gets the live feed for symbol list.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <returns></returns>
        public Dictionary<string, SymbolData> GetLiveFeedForSymbolListTouch(Dictionary<string, AdviceSymbolInfo> symbols)
        {
            Dictionary<string, SymbolData> liveFeedDict = new Dictionary<string, SymbolData>();
            try
            {
                foreach (KeyValuePair<string, AdviceSymbolInfo> kvp in symbols)
                {
                    SymbolData data = null;
                    lock (_lockOnTouchPricingCache)
                    {
                        _pricingCacheTouch.TryGetValue(kvp.Key, out data);
                    }
                    if (data != null)
                    {
                        liveFeedDict.Add(kvp.Key, data);
                    }
                }
                lock (_lockOnTouchPricingCache)
                {
                    foreach (KeyValuePair<string, SymbolData> kvp in _pricingCacheTouch)
                    {
                        if (kvp.Value != null && kvp.Value.CategoryCode == AssetCategory.Indices)
                        {
                            liveFeedDict[kvp.Key] = kvp.Value;
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

            return liveFeedDict;
        }

        /// <summary>
        /// Gets the live feed for symbol list.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <returns></returns>
        public Dictionary<string, SymbolData> GetLiveFeedForSymbolList(Dictionary<string, AdviceSymbolInfo> symbols)
        {
            Dictionary<string, SymbolData> liveFeedDict = new Dictionary<string, SymbolData>();
            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (KeyValuePair<string, AdviceSymbolInfo> kvp in symbols)
                {
                    if (_liveFeedManager != null)
                    {
                        if (kvp.Value.AssetId.Equals(AssetCategory.FX) || kvp.Value.AssetId.Equals(AssetCategory.FXForward) || kvp.Value.AssetId.Equals(AssetCategory.Forex))
                            _liveFeedManager.CheckAndAdviseSymbolForFX(kvp.Value.Symbol, kvp.Value.FromCurrencyId, kvp.Value.ToCurrencyId, kvp.Value.AssetId);
                        else
                            _liveFeedManager.CheckAndAdviseSymbol(kvp.Key);
                    }

                    SymbolData data = null;
                    lock (_lockOnPricingCache)
                    {
                        _pricingCache.TryGetValue(kvp.Key, out data);
                    }
                    if (data != null)
                    {
                        if (_enableMarketData_ContinuousDataLogging)
                        {
                            builder.AppendLine(DateTime.Now.ToLongTimeString() + " Symbol= " + kvp.Key + " SelectedFeedPrice= " + data.SelectedFeedPrice + " Last= " + data.LastPrice + " Bid= " + data.Bid + " Ask= " + data.Ask + " Change= " + data.Change + " Low= " + data.Low + " High= " + data.High + " Previous= " + data.Previous + " Currency= " + data.CurencyCode);
                        }
                        liveFeedDict.Add(kvp.Key, data);
                    }
                }
                if (_enableMarketData_ContinuousDataLogging)
                    Logger.LoggerWrite(builder.ToString(), LoggingConstants.LIVEDATA_LOGGING);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

            return liveFeedDict;
        }

        public Dictionary<string, string> GetUserInformation()
        {
            try
            {
                return _liveFeedManager.GetUserInformation();
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

            return new Dictionary<string, string>();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            try
            {
                return _liveFeedManager.GetUserPermissionsInformation();
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

            return new List<Dictionary<string, string>>();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            try
            {
                return _liveFeedManager.GetSubscriptionInformation();
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

            return new Dictionary<string, int>();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            try
            {
                return _liveFeedManager.GetTickersLastStatusCode();
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

            return new Dictionary<string, string>();
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _snapShotTmr) _snapShotTmr.Dispose();
                if (_complianceSnapshotTmr != null) _complianceSnapshotTmr.Dispose();
                if (_liveFeedManager != null) _liveFeedManager.Dispose();
            }
        }
        #endregion
    }
}