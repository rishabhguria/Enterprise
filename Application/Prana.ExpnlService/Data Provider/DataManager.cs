using Prana.BBGImportManager;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Prana.ExpnlService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class DataManager : IGroupedDataProvider, IPublishing, IDisposable
    {
        private const int CONSOLIDATEDID = int.MinValue;
        private const int UNALLOCATEDID = -1;
        private static ProxyBase<IPranaPositionServices> _positionManagementServices = null;
        private BBGFileWatcher _bbgFileWatcher = null;
        private Object _calcPrefsLockerObject = new object();
        private ProxyBase<ICashManagementService> _cashMgmtService = null;
        private Dictionary<int, DailyCreditLimit> _dailyCreditLimitCollection = null;
        private Object _dailyCreditLimitLockerObject = new object();
        private DatabaseManager _databaseManager = null;
        private int _fetchDataByDate = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("FetchDataByDate").ToString());
        private bool _fillCurrencyIfBlankCurrencyTradeFound = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("FetchCurrencyIfBlankCurrencyTradeFound").ToString());
        private Dictionary<int, double> _accountWiseNra = null;
        private Object _accountWiseNraLockerObject = new object();
        private bool _isNewAUECIDsTradeAdded = false;
        private bool _isTaxlotsBrokenForBoxedPositions = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsTaxlotsBrokenForBoxedPositions").ToString());
        private bool _isAccrualsNeeded = bool.Parse(ConfigurationManager.AppSettings["IsAccrualsNeededOnPM"]);
        private bool _isIncludeTradingDayAccruals = bool.Parse(ConfigurationManager.AppSettings["IsIncludeTradingDayAccruals"]);
        private bool _isTradesDeleted = false;
        private bool _isAutoUpdateOptionsUDAWithUnderlyingUpdate = true;
        private DateTime _lastDate;
        private object _lockerObj = new object();
        private Object _mtdPnLLockerObject = new object();
        private OrderFillManager _orderFillManager = null;
        private Dictionary<int, PMCalculationPrefs> _pmCalculationPrefCollection = null;

        private DuplexProxyBase<IPricingService> _pricingServiceProxy = null;

        private DuplexProxyBase<ISubscription> _proxyPricing;

        private DuplexProxyBase<ISubscription> _proxyServer;

        private ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;

        private int _splittedTaxlotsCacheBasis = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("SplittedTaxlotsCacheBasis").ToString());

        private Dictionary<int, double> _startOfMonthCapitalAccountCollection = null;

        private Object _startOfMonthCapitalAccountLockerObject = new object();

        private Dictionary<string, bool> _symbolsMarkedForCalculation = null;

        private Dictionary<string, int> _symbolWiseCurrencyIdCollection = null;

        private Dictionary<string, List<string>> _symbolWiseTaxLotsDictionary = null;

        private Dictionary<int, double> _userDefinedMTDPnLCollection = null;

        public event EventHandler LogOnScreen;

        public event EventHandler UpdateIndicesMarkCacheEvent;

        public event EventHandler<EventArgs<EPnlOrder, ApplicationConstants.TaxLotState, bool>> UpdateRemoveOrder;

        #region Compliance section Events

        /// <summary>
        /// Event which is raised when any AuecDetails is Updated
        /// </summary>
        public event EventHandler<EventArgs<AuecDetails>> AuecDetailsUpdated;

        /// <summary>
        /// Event which is raised when any Beta is updated
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<string, double>>> BetaUpdate;

        /// <summary>
        /// Event for publishing daily credit limit
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, DailyCreditLimit>>> DailyCreditLimitEvent;

        /// <summary>
        /// Event which is raised when any DayEndCash is Received
        /// </summary>
        public event EventHandler<EventArgs<List<DayEndAccountCash>>> DayEndCashReceived;

        /// <summary>
        /// Event which is raised when any DbNav is updated
        /// </summary>
        public event EventHandler<EventArgs<List<DbNav>>> DbNavUpdate;

        /// <summary>
        /// Event which is raised when any AccountWiseAccrual is Received
        /// </summary>
        public event EventHandler<EventArgs<List<Accurals>>> AccountWiseAccrualEvent;

        /// <summary>
        /// Event which is raised when any AccountWiseStartOfDayAccrual is Received
        /// </summary>
        public event EventHandler<EventArgs<List<Accurals>>> AccountWiseStartOfDayAccrualEvent;

        /// <summary>
        /// Event which is raised when any AccountWiseDayAccrual is Received
        /// </summary>
        public event EventHandler<EventArgs<List<Accurals>>> AccountWiseDayAccrualEvent;

        /// <summary>
        /// Event which is raised when any Order fill is received
        /// </summary>
        public event EventHandler<EventArgs<EPnlOrder>> OrderReceived;

        /// <summary>
        /// Event which is raised when any startOfMonthCapitalAccount is Received
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, double>>> StartOfMonthCapitalAccountUpdated;

        /// <summary>
        /// Event which is raised when any userDefinedMTDPnLCollection is Received
        /// </summary>
        public event EventHandler<EventArgs<Dictionary<int, double>>> UserDefinedMTDPnLCollectionUpdated;

        /// <summary>
        /// Event which is raised when any YesterdayFxRate is Updated
        /// </summary>
        public event EventHandler<EventArgs<List<YesterdayFxRate>>> YesterdayFxRateUpdated;
        #endregion

        public ProxyBase<ICashManagementService> CashMgmtService
        {
            set
            {
                _cashMgmtService = value;
            }
        }

        public ProxyBase<IPranaPositionServices> PositionManagementServices
        {
            set
            {
                _positionManagementServices = value;
            }
        }

        public DuplexProxyBase<IPricingService> PricingServiceProxy
        {
            set
            {
                _pricingServiceProxy = value;
                _pricingServiceProxy.ConnectedEvent += new Proxy<IPricingService>.ConnectionEventHandler(_pricingServiceProxy_ConnectedEvent);
            }
        }

        public ProxyBase<ISecMasterSyncServices> SecMasterSyncService
        {
            set
            {
                _secMasterSyncService = value;
            }
        }

        public static void UpdateData(EPnlOrder oldOrder, EPnlOrder neworder)
        {
            try
            {
                oldOrder.AvgPrice = neworder.AvgPrice;
                oldOrder.OrderSideTagValue = neworder.OrderSideTagValue;
                oldOrder.SideMultiplier = neworder.SideMultiplier;
                oldOrder.CounterPartyName = neworder.CounterPartyName;
                oldOrder.Quantity = neworder.Quantity;
                oldOrder.AUECLocalDate = neworder.AUECLocalDate;
                oldOrder.TransactionDate = neworder.TransactionDate;
                oldOrder.YesterdayMarkDateActual = neworder.YesterdayMarkDateActual;
                oldOrder.YesterdayMarkPrice = neworder.YesterdayMarkPrice;
                oldOrder.YesterdayMarkPriceStr = neworder.YesterdayMarkPriceStr;
                oldOrder.FXRateOnTradeDate = neworder.FXRateOnTradeDate;
                oldOrder.FXConversionMethodOnTradeDate = neworder.FXConversionMethodOnTradeDate;
                oldOrder.Description = neworder.Description;
                oldOrder.InternalComments = neworder.InternalComments;
                oldOrder.TradeAttribute1 = neworder.TradeAttribute1;
                oldOrder.TradeAttribute2 = neworder.TradeAttribute2;
                oldOrder.TradeAttribute3 = neworder.TradeAttribute3;
                oldOrder.TradeAttribute4 = neworder.TradeAttribute4;
                oldOrder.TradeAttribute5 = neworder.TradeAttribute5;
                oldOrder.TradeAttribute6 = neworder.TradeAttribute6;
                oldOrder.SetTradeAttribute(neworder.GetTradeAttributesAsDict());
                oldOrder.TransactionType = neworder.TransactionType;
                oldOrder.UserName = neworder.UserName;
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

        public void SubscribeDailyCreditLimit()
        {
            try
            {
                _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_DailyCreditLimit);
                _proxyServer.Subscribe(Topics.Topic_DailyCreditLimit, null);
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

        public void SubscribeCashData()
        {
            try
            {
                _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_CashData);
                _proxyServer.Subscribe(Topics.Topic_CashData, null);
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

        public void SubscribeDayEndCash()
        {
            try
            {
                FilterDataByExactDate filterdata = new FilterDataByExactDate();
                DateTime latestDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true);
                //Biz date calculated for auecid =1
                DateTime lastDate = latestDate.AddDays(-1);
                LogOnScreen("Day End Cash Filter subscribed for the date: " + lastDate, EventArgs.Empty);
                filterdata.GivenDate = lastDate.Date;
                List<FilterData> filters = new List<FilterData>();
                filters.Add(filterdata);
                _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_DayEndCash);
                _proxyServer.Subscribe(Topics.Topic_DayEndCash, filters);

                _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_StartDayOfAccrual);
                _proxyServer.Subscribe(Topics.Topic_StartDayOfAccrual, null);
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

        internal Dictionary<String, double> GetAvgVolumeCustomDays()
        {
            try
            {
                Dictionary<String, double> avgVolCustomDays = new Dictionary<String, double>();
                lock (_avgVolume90DaysLocker)
                {
                    if (_avgVolume90Days != null)
                    {
                        foreach (String key in _avgVolume90Days.Keys)
                        {
                            if (avgVolCustomDays.ContainsKey(key))
                                avgVolCustomDays[key] = _avgVolume90Days[key];
                            else
                                avgVolCustomDays.Add(key, _avgVolume90Days[key]);
                        }
                    }
                }
                return avgVolCustomDays;
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
        }

        internal Dictionary<int, DailyCreditLimit> GetDailyCreditLimit()
        {
            try
            {
                lock (_dailyCreditLimitLockerObject)
                {
                    Dictionary<int, DailyCreditLimit> clonedDailyCreditLimitCollection = new Dictionary<int, DailyCreditLimit>(_dailyCreditLimitCollection);
                    return clonedDailyCreditLimitCollection;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        internal Dictionary<int, double> GetAccountWiseNra()
        {
            Dictionary<int, double> accountWiseNra = new Dictionary<int, double>();
            try
            {
                lock (_accountWiseNraLockerObject)
                {
                    if (_accountWiseNra != null)
                    {
                        foreach (int key in _accountWiseNra.Keys)
                        {
                            if (accountWiseNra.ContainsKey(key))
                                accountWiseNra[key] = _accountWiseNra[key];
                            else
                                accountWiseNra.Add(key, _accountWiseNra[key]);
                        }
                    }
                }
                return accountWiseNra;
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
        }

        internal Dictionary<int, double> GetMTDPnl()
        {
            Dictionary<int, double> mtdPnl = new Dictionary<int, double>();
            try
            {
                lock (_mtdPnLLockerObject)
                {
                    foreach (int key in _userDefinedMTDPnLCollection.Keys)
                    {
                        if (mtdPnl.ContainsKey(key))
                            mtdPnl[key] = _userDefinedMTDPnLCollection[key];
                        else
                            mtdPnl.Add(key, _userDefinedMTDPnLCollection[key]);
                    }
                }
                return mtdPnl;
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
        }

        internal Dictionary<int, PMCalculationPrefs> GetPMCalculationPrefs()
        {
            try
            {
                lock (_calcPrefsLockerObject)
                {
                    return _pmCalculationPrefCollection;
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
        }

        internal Dictionary<int, double> GetStartOfMonthCapitalAccount()
        {
            Dictionary<int, double> startMonthCapital = new Dictionary<int, double>();
            try
            {
                lock (_startOfMonthCapitalAccountLockerObject)
                {
                    foreach (int key in _startOfMonthCapitalAccountCollection.Keys)
                    {
                        if (startMonthCapital.ContainsKey(key))
                            startMonthCapital[key] = _startOfMonthCapitalAccountCollection[key];
                        else
                            startMonthCapital.Add(key, _startOfMonthCapitalAccountCollection[key]);
                    }
                }
                return startMonthCapital;
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
        }

        private void _pricingServiceProxy_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                InitilizeMarkPriceCache();
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

        private void CreateAccrualSummaryCollection()
        {
            try
            {
                _accountWiseInitialAccruals.Clear();
                _accountWiseDayAccruals.Clear();
                if (_accounts != null)
                {
                    foreach (object obj in _accounts)
                    {
                        Account account = obj as Account;
                        Dictionary<int, double> dictCurrency = new Dictionary<int, double>();
                        if (account != null)
                        {
                            if (!_accountWiseInitialAccruals.ContainsKey(account.AccountID))
                            {
                                _accountWiseInitialAccruals.Add(account.AccountID, dictCurrency);
                            }
                            if (!_accountWiseDayAccruals.ContainsKey(account.AccountID))
                            {
                                _accountWiseDayAccruals.Add(account.AccountID, dictCurrency);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreateSubscriptionServicesProxyPricing()
        {
            try
            {
                if (_proxyPricing == null)
                    _proxyPricing = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", this);

                _proxyPricing.Subscribe(Topics.Topic_MarkPrice, null);
                _proxyPricing.Subscribe(Topics.Topic_Beta, null);
                _proxyPricing.Subscribe(Topics.Topic_OutStandings, null);
                _proxyPricing.Subscribe(Topics.Topic_ForexRate, null);
                _proxyPricing.Subscribe(Topics.Topic_PerformanceNumber, null);
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

        private void CreateSubscriptionServicesProxyServer()
        {
            try
            {
                if (_proxyServer == null)
                    _proxyServer = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);

                _proxyServer.Subscribe(Topics.Topic_Split, null);
                _proxyServer.Subscribe(Topics.Topic_SecurityMaster, null);
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

        private void DataManager_OrderFillReceived(TaxLot taxlot, ApplicationConstants.TaxLotState state, string topicName)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (topicName == Topics.Topic_Allocation && (taxlot.ClosingStatus == ClosingStatus.Closed || taxlot.ClosingStatus == ClosingStatus.PartiallyClosed))
                    {
                        if (_uncalculatedData.Contains(taxlot.TaxLotID))
                        {
                            taxlot.TaxLotQty = _uncalculatedData[taxlot.TaxLotID].Quantity;
                        }
                    }

                    List<TaxLot> listTaxlots = new List<TaxLot>();
                    listTaxlots.Add(taxlot);
                    Dictionary<String, EPnlOrder> modifiedTaxlotCache = null;
                    ExposureAndPnlOrderCollection tempCollectionFromDB = null;
                    modifiedTaxlotCache = GetExposureAndPnlOrderCollection(listTaxlots, state, true, out tempCollectionFromDB);
                    if (_isTaxlotsBrokenForBoxedPositions && state == ApplicationConstants.TaxLotState.Deleted)
                    {
                        state = ApplicationConstants.TaxLotState.New;
                    }

                    foreach (EPnlOrder exPnlOrder in tempCollectionFromDB)
                    {
                        if (state == ApplicationConstants.TaxLotState.Deleted)
                        {
                            _uncalculatedData.Remove(exPnlOrder.ID);
                            _isTradesDeleted = true;
                        }
                        else
                        {
                            _isUncalculatedDataChangedWhileSendingToClients = true;
                            exPnlOrder.HasBeenSentToUser = 0;
                        }

                        exPnlOrder.PricingSource = PricingSource.NewOrder;
                        ///Added this check for Correct DayPnlBase for international equities.
                        ///Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-1783
                        if (state != ApplicationConstants.TaxLotState.Deleted && !TimeZoneHelper.GetInstance().InUseAUECIDs.Contains(exPnlOrder.AUECID))
                        {
                            TimeZoneHelper.GetInstance().InUseAUECIDs.Add(exPnlOrder.AUECID);
                            QuartzScheduler _quartzScheduler = QuartzScheduler.GetInstance();
                            _quartzScheduler.CreateTriggerForNewAuecsClearance(exPnlOrder.AUECID);

                            #region Compliance Section
                            try
                            {
                                //sending updated Auec to esper through manager
                                if (AuecDetailsUpdated != null)
                                    AuecDetailsUpdated(this, new EventArgs<AuecDetails>(GetAUECDetails(exPnlOrder.AUECID)));
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
                            #endregion

                            _isNewAUECIDsTradeAdded = true;
                        }

                        if (_uncalculatedData != null && _uncalculatedData.Contains(exPnlOrder.ID))
                        {
                            if (state != ApplicationConstants.TaxLotState.Deleted)
                            {
                                if (_companyAccountMasterFundRelation != null && _companyAccountMasterFundRelation.ContainsKey(_uncalculatedData[exPnlOrder.ID].Level1ID))
                                {
                                    exPnlOrder.MasterFundID = _companyAccountMasterFundRelation[_uncalculatedData[exPnlOrder.ID].Level1ID];
                                }

                                if (exPnlOrder.ClassID == EPnLClassID.EPnLOrderEquitySwap)
                                {
                                    _uncalculatedData.UpdateOrder(exPnlOrder);
                                    exPnlOrder.IsMarkedForCalculation = true;
                                }
                                else
                                {
                                    UpdateData(_uncalculatedData[exPnlOrder.ID], exPnlOrder);
                                    _uncalculatedData[exPnlOrder.ID].IsMarkedForCalculation = true;
                                    _uncalculatedData[exPnlOrder.ID].HasBeenSentToUser = 0;
                                }

                                //symbol data updated, mark for calculation
                                _symbolsMarkedForCalculation[exPnlOrder.Symbol] = true;
                            }
                        }
                        else
                        {
                            if (state != ApplicationConstants.TaxLotState.Deleted)
                            {
                                lock (_betaLockerObject)
                                {
                                    if (exPnlOrder is EPnLOrderOption || exPnlOrder is EPnLOrderFXForward)
                                    {
                                        if (_betaForSymbols != null && _betaForSymbols.ContainsKey(exPnlOrder.UnderlyingSymbol))
                                        {
                                            exPnlOrder.Beta = _betaForSymbols[exPnlOrder.UnderlyingSymbol].Price;
                                        }
                                    }
                                    else
                                    {
                                        if (_betaForSymbols != null && _betaForSymbols.ContainsKey(exPnlOrder.Symbol))
                                        {
                                            exPnlOrder.Beta = _betaForSymbols[exPnlOrder.Symbol].Price;
                                        }
                                    }
                                }

                                //Done for the requirement where 0 is never displayed for Beta, the default value is 1
                                if (exPnlOrder.Beta == 0)
                                {
                                    exPnlOrder.Beta = 1;
                                }

                                lock (_sharesOutstandingLockerObject)
                                {
                                    if (_sharesOutstandingForSymbols != null && _sharesOutstandingForSymbols.ContainsKey(exPnlOrder.Symbol))
                                    {
                                        exPnlOrder.SharesOutstanding = _sharesOutstandingForSymbols[exPnlOrder.Symbol];
                                    }
                                }
                                if (_companyAccountMasterFundRelation != null && _companyAccountMasterFundRelation.ContainsKey(exPnlOrder.Level1ID))
                                {
                                    exPnlOrder.MasterFundID = _companyAccountMasterFundRelation[exPnlOrder.Level1ID];
                                }
                                else
                                {
                                    exPnlOrder.MasterFundID = -1;
                                }
                                if (_companyStrategyMasterStrategyRelation != null && _companyStrategyMasterStrategyRelation.ContainsKey(exPnlOrder.Level2ID))
                                {
                                    exPnlOrder.MasterStrategyID = _companyStrategyMasterStrategyRelation[exPnlOrder.Level2ID];
                                }

                                //add this order to the collection and user cache
                                exPnlOrder.IsMarkedForCalculation = true;
                                if (_symbolWiseTaxLotsDictionary != null && _symbolWiseTaxLotsDictionary.ContainsKey(exPnlOrder.Symbol))
                                {
                                    _symbolWiseTaxLotsDictionary[exPnlOrder.Symbol].Add(exPnlOrder.ID);
                                    _symbolsMarkedForCalculation[exPnlOrder.Symbol] = true;
                                }
                                else
                                {
                                    List<string> IDList = new List<string>();
                                    IDList.Add(exPnlOrder.ID);
                                    _symbolWiseTaxLotsDictionary.Add(exPnlOrder.Symbol, IDList);
                                    _symbolsMarkedForCalculation.Add(exPnlOrder.Symbol, true);
                                }

                                if (_fillCurrencyIfBlankCurrencyTradeFound && exPnlOrder.CurrencyID <= 0)
                                {
                                    if (_symbolWiseCurrencyIdCollection.ContainsKey(exPnlOrder.Symbol))
                                    {
                                        exPnlOrder.CurrencyID = _symbolWiseCurrencyIdCollection[exPnlOrder.Symbol];
                                    }
                                    else
                                    {
                                        int currencyID = _secMasterSyncService.InnerChannel.GetCurrencyIdForSymbol(exPnlOrder.Symbol);
                                        exPnlOrder.CurrencyID = currencyID;
                                        _symbolWiseCurrencyIdCollection.Add(exPnlOrder.Symbol, currencyID);
                                    }
                                    string logMessage = "Refilling currency for Symbol = " + exPnlOrder.Symbol + ", Taxlot ID = " + exPnlOrder.ID + ", Currency = " + CachedDataManager.GetInstance.GetCurrencyText(exPnlOrder.CurrencyID);
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(logMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                }

                                if (_uncalculatedData != null)
                                    _uncalculatedData.Add(exPnlOrder);
                            }
                        }

                        if (modifiedTaxlotCache.ContainsKey(exPnlOrder.ID))
                        {
                            modifiedTaxlotCache[exPnlOrder.ID] = exPnlOrder;
                        }
                        else
                        {
                            modifiedTaxlotCache.Add(exPnlOrder.ID, exPnlOrder);
                        }
                        modifiedTaxlotCache[exPnlOrder.ID].EpnlOrderState = state;
                    }

                    #region Compliance section
                    foreach (String id in modifiedTaxlotCache.Keys)
                    {
                        try
                        {
                            var exPnlOrder = modifiedTaxlotCache[id];
                            if (exPnlOrder.Quantity != 0 || state == ApplicationConstants.TaxLotState.Deleted)// && state == ApplicationConstants.TaxLotState.Deleted)
                            {
                                EPnlOrder cloneOrder = CloneHelper.CreateEPnlOrderClone(exPnlOrder);
                                cloneOrder.EpnlOrderState = state;
                                //sending order to esper through manager
                                if (OrderReceived != null)
                                    OrderReceived(this, new EventArgs<EPnlOrder>(cloneOrder));
                                Logger.LoggerWrite("SENT TO ESPER " + exPnlOrder.EpnlOrderState + " " + exPnlOrder.Quantity + " @ " + exPnlOrder.Level1ID + " || " + exPnlOrder.ID);
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
        }

        private void FillSummaryCollection()
        {
            try
            {
                if (_accounts == null || _accounts.Count == 0)
                {
                    _accounts = WindsorContainerManager.GetAccounts();
                }

                _summaries = GetEmptySummary();
                _accountWiseInitialCash.Clear();
                _accountWiseDayCash.Clear();
                _dailyCreditLimitCollection.Clear();

                //Initialize the account cash by 0 for all accounts.
                foreach (object obj in _accounts)
                {
                    Account account = obj as Account;
                    if (account != null)
                    {
                        if (!_accountWiseInitialCash.ContainsKey(account.AccountID))
                        {
                            _accountWiseInitialCash.Add(account.AccountID, 0);
                        }
                        if (!_accountWiseDayCash.ContainsKey(account.AccountID))
                        {
                            _accountWiseDayCash.Add(account.AccountID, new Tuple<double, double>(0, 0));
                        }
                        if (!_dailyCreditLimitCollection.ContainsKey(account.AccountID))
                        {
                            _dailyCreditLimitCollection.Add(account.AccountID, new DailyCreditLimit());
                        }
                    }
                }

                CreateAccrualSummaryCollection();

                //Biz date calculated for auecid =1
                //We pickup cash for latest day - 1 without considering the holidays as it may be holiday in one AUEC but not in other AUEC
                DateTime latestDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true);

                _lastDate = latestDate.AddDays(-1);
                string logMsg = "Day End Cash picked for the date : " + _lastDate;
                LogOnScreen(logMsg, EventArgs.Empty);
                Logger.LoggerWrite(logMsg);


                GenericDayEndData genericDayEndData = _cashMgmtService.InnerChannel.GetDayEndDataInBaseCurrency(latestDate, _isAccrualsNeeded, _isIncludeTradingDayAccruals);
                UpdateDayEndCash(genericDayEndData.StartOfDayAccountWiseCash);
                UpdateStartOfDayAccruals(genericDayEndData.StartOfDayAccountWiseAccruals);
                UpdateDayCashImpact(genericDayEndData.DayAccountWiseCash);
                UpdateDayAccruals(genericDayEndData.DayAccountWiseAccruals);

                //Getting this info from Trade Service config
                _isAutoUpdateOptionsUDAWithUnderlyingUpdate = _cashMgmtService.InnerChannel.IsAutoUpdateOptionsUDAWithUnderlyingUpdate();


                //get NAV from DB
                //if _accountWiseNAV.Count == 0 => NAV has not been saved
                DatabaseManager.GetStartofDayNAVValues(ref _accountWiseNAV);
                DatabaseManager.GetShadowNAVValues(ref _accountWiseShadowNAV);
                _databaseManager.GetYesterdayKeyReturnsAndPnLNumbers(ref _accountWiseKeyReturns);

                double totalMTDPnL = 0;
                double totalQTDPnL = 0;
                double totalYTDPnL = 0;
                double totalMTDReturn = 0;
                double totalQTDReturn = 0;
                double totalYTDReturn = 0;

                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_accountWiseNAV.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.YesterdayNAV = _accountWiseNAV[emptySummary.Key].NAVValue;
                    }

                    if (_accountWiseShadowNAV.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.NetAssetValue = _accountWiseShadowNAV[emptySummary.Key].NAVValue;
                    }

                    if (_accountWiseKeyReturns.ContainsKey(emptySummary.Key))
                    {
                        totalMTDPnL += _accountWiseKeyReturns[emptySummary.Key].MTDPnL;
                        totalQTDPnL += _accountWiseKeyReturns[emptySummary.Key].QTDPnL;
                        totalYTDPnL += _accountWiseKeyReturns[emptySummary.Key].YTDPnL;

                        totalMTDReturn += _accountWiseKeyReturns[emptySummary.Key].MTDReturn;
                        totalQTDReturn += _accountWiseKeyReturns[emptySummary.Key].QTDReturn;
                        totalYTDReturn += _accountWiseKeyReturns[emptySummary.Key].YTDReturn;

                        emptySummary.Value.MTDPnL = _accountWiseKeyReturns[emptySummary.Key].MTDPnL;
                        emptySummary.Value.QTDPnL = _accountWiseKeyReturns[emptySummary.Key].QTDPnL;
                        emptySummary.Value.YTDPnL = _accountWiseKeyReturns[emptySummary.Key].YTDPnL;

                        emptySummary.Value.MTDReturn = _accountWiseKeyReturns[emptySummary.Key].MTDReturn;
                        emptySummary.Value.QTDReturn = _accountWiseKeyReturns[emptySummary.Key].QTDReturn;
                        emptySummary.Value.YTDReturn = _accountWiseKeyReturns[emptySummary.Key].YTDReturn;
                    }
                    if (emptySummary.Key == int.MinValue)
                    {
                        emptySummary.Value.MTDPnL = totalMTDPnL;
                        emptySummary.Value.QTDPnL = totalQTDPnL;
                        emptySummary.Value.YTDPnL = totalYTDPnL;
                        emptySummary.Value.MTDReturn = totalMTDReturn;
                        emptySummary.Value.QTDReturn = totalQTDReturn;
                        emptySummary.Value.YTDReturn = totalYTDReturn;

                        totalMTDPnL = 0;
                        totalQTDPnL = 0;
                        totalYTDPnL = 0;
                        totalMTDReturn = 0;
                        totalQTDReturn = 0;
                        totalYTDReturn = 0;
                    }
                }

                UpdateDailyCreditLimitCollection(GetDailyCreditLimitValues());
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

        private DataTable GetDailyCreditLimitValues()
        {
            DataTable dtDailyCreditLimit = new DataTable();
            try
            {
                dtDailyCreditLimit = _cashMgmtService.InnerChannel.GetDailyCreditLimitValues();
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
            return dtDailyCreditLimit;
        }

        private Dictionary<int, ExposureAndPnlOrderSummary> GetEmptySummary()
        {
            Dictionary<int, ExposureAndPnlOrderSummary> emptySummaries = new Dictionary<int, ExposureAndPnlOrderSummary>();
            try
            {
                foreach (Account account in _accounts)
                {
                    ExposureAndPnlOrderSummary summary = new ExposureAndPnlOrderSummary();
                    summary.Level1ID = account.AccountID;
                    emptySummaries.Add(account.AccountID, summary);
                }
                ///This is one time initialization
                ///For unallocated accounts we need to calculate the summary
                if (!emptySummaries.ContainsKey(UNALLOCATEDID))
                {
                    ExposureAndPnlOrderSummary summary = new ExposureAndPnlOrderSummary();
                    summary.Level1ID = UNALLOCATEDID;
                    emptySummaries.Add(UNALLOCATEDID, summary);
                }

                //Overall Summary now included in the summary collection
                if (!emptySummaries.ContainsKey(CONSOLIDATEDID))
                {
                    ExposureAndPnlOrderSummary summary = new ExposureAndPnlOrderSummary();
                    summary.Level1ID = CONSOLIDATEDID;
                    emptySummaries.Add(CONSOLIDATEDID, summary);
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
            return emptySummaries;
        }

        private Dictionary<string, SymbolPriceWithDate> GetUpdatedBetaCache(Dictionary<string, DateTime> dateWiseSymbol)
        {
            try
            {
                _betaForSymbols = DatabaseManager.GetBetaValueDateWiseForZeroBetas(dateWiseSymbol);
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
            return _betaForSymbols;
        }

        private void PrepareSymbolGrouping()
        {
            try
            {
                foreach (EPnlOrder raworder in _uncalculatedData)
                {
                    if (_symbolWiseTaxLotsDictionary.ContainsKey(raworder.Symbol))
                    {
                        _symbolWiseTaxLotsDictionary[raworder.Symbol].Add(raworder.ID);
                    }
                    else
                    {
                        List<string> taxLotIDForSymbol = new List<string>();
                        taxLotIDForSymbol.Add(raworder.ID);
                        _symbolWiseTaxLotsDictionary.Add(raworder.Symbol, taxLotIDForSymbol);
                    }

                    if (!_symbolsMarkedForCalculation.ContainsKey(raworder.Symbol))
                    {
                        _symbolsMarkedForCalculation.Add(raworder.Symbol, true);
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

        private void SubscribeCashService()
        {
            try
            {
                if (TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates != null)
                {
                    List<FilterData> filters = new List<FilterData>();
                    _proxyServer.Subscribe(Topics.Topic_CashActivity, filters);
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

        #region Data Collections
        private Object _accrualLockerObject = new object();
        private Dictionary<string, SymbolPriceWithDate> _betaForSymbols = null;
        private Object _betaLockerObject = new object();
        private Dictionary<int, int> _companyAccountMasterFundRelation = null;
        private int _companyID;
        private Dictionary<int, int> _companyStrategyMasterStrategyRelation = null;
        private DataTable _dtIndicesReturn = null;
        private AccountCollection _accounts;
        private Dictionary<int, Dictionary<int, double>> _accountWiseInitialAccruals;
        private Dictionary<int, double> _accountWiseInitialCash;
        private Dictionary<int, Dictionary<int, double>> _accountWiseDayAccruals;
        private Dictionary<int, Tuple<double, double>> _accountWiseDayCash;
        private Object _accountWiseKeyReturnLockerObject = new Object();
        private Dictionary<int, KeyReturns> _accountWiseKeyReturns;
        private Dictionary<int, NAVStruct> _accountWiseNAV;
        /// <summary>
        /// CODE CHANGE:20120208:ISHANT KATHURIA:LFD 45:START OF THE DAY NAV NOT PICKED UP FROM THE DATABASE
        /// SEGREGATING SHADOW NAV AND START OF THE DAY NAV
        /// </summary>
        private Dictionary<int, NAVStruct> _accountWiseShadowNAV;
        private DataSet _indexDS = null;
        private Dictionary<string, double> _sharesOutstandingForSymbols = null;
        private Object _sharesOutstandingLockerObject = new object();
        private Dictionary<int, ExposureAndPnlOrderSummary> _summaries = null;
        private ExposureAndPnlOrderCollection _uncalculatedData = null;

        public int CompanyID
        {
            get { return _companyID; }
        }

        public DataTable DtIndicesReturn
        {
            get { return _dtIndicesReturn; }
        }

        public DataSet IndexSymbols
        {
            get { return _indexDS; }
        }

        public Dictionary<int, NAVStruct> NAVValues
        {
            get { return _accountWiseNAV; }
        }

        public Dictionary<int, NAVStruct> ShadowNAVValues
        {
            get { return _accountWiseShadowNAV; }
        }

        public Dictionary<int, ExposureAndPnlOrderSummary> Summaries
        {
            get
            {
                UpdateAccrualValueInSummary();
                return _summaries;
            }
        }

        public ExposureAndPnlOrderCollection UncalculatedData
        {
            get
            {
                return _uncalculatedData;
            }
            set
            {
                _uncalculatedData = value;
            }
        }
        #endregion

        #region Singleton Constructor Implementation.
        private static DataManager _instance = null;
        private bool _isNAVSaved = false;

        private DataManager()
        {
            try
            {
                _uncalculatedData = new ExposureAndPnlOrderCollection();
                _symbolWiseTaxLotsDictionary = new Dictionary<string, List<string>>();
                _symbolsMarkedForCalculation = new Dictionary<string, bool>();
                _indexDS = new DataSet();
                _dtIndicesReturn = new DataTable();
                _companyID = int.MinValue;
                _accounts = new AccountCollection();
                _accountWiseInitialCash = new Dictionary<int, double>();
                _accountWiseInitialAccruals = new Dictionary<int, Dictionary<int, double>>();
                _accountWiseDayCash = new Dictionary<int, Tuple<double, double>>();
                _accountWiseDayAccruals = new Dictionary<int, Dictionary<int, double>>();
                _accountWiseKeyReturns = new Dictionary<int, KeyReturns>();
                _accountWiseNAV = new Dictionary<int, NAVStruct>();
                _symbolWiseCurrencyIdCollection = new Dictionary<string, int>();
                lock (_betaLockerObject)
                {
                    _betaForSymbols = new Dictionary<string, SymbolPriceWithDate>();
                }
                lock (_sharesOutstandingLockerObject)
                {
                    _sharesOutstandingForSymbols = new Dictionary<string, double>();
                }
                _databaseManager = DatabaseManager.GetInstance();
                _companyID = _databaseManager.CompanyID;

                _orderFillManager = OrderFillManager.GetInstance();

                _bbgFileWatcher = BBGFileWatcher.GetInstance();
                _isNAVSaved = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsNAVSaved"));

                ///wireup for listening new orders from OrderFillManager!
                ///Gaurav: This event must be wire up after EXPNL start as it may cause dead lock while here are fills from trade server and expnl server is starting
                _orderFillManager.OrderFillReceived += new ParameterizedMethodHandler(DataManager_OrderFillReceived);

                _bbgFileWatcher.BBGFileImported += new EventHandler(BBGFileWatcher_BBGFileImported);

                lock (_calcPrefsLockerObject)
                {
                    _pmCalculationPrefCollection = new Dictionary<int, PMCalculationPrefs>();
                }
                lock (_startOfMonthCapitalAccountLockerObject)
                {
                    _startOfMonthCapitalAccountCollection = new Dictionary<int, double>();
                }
                lock (_mtdPnLLockerObject)
                {
                    _userDefinedMTDPnLCollection = new Dictionary<int, double>();
                }
                lock (_dailyCreditLimitLockerObject)
                {
                    _dailyCreditLimitCollection = new Dictionary<int, DailyCreditLimit>();
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

        public static DataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }

        private void BBGFileWatcher_BBGFileImported(object sender, EventArgs e)
        {
            try
            {
                ReloadCacheBasedOnBBGFileImport();
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("BBG file imported successfully at " + DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss tt"), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        private void ReloadCacheBasedOnBBGFileImport()
        {
            try
            {
                lock (_betaForSymbols)
                {
                    _betaForSymbols = DatabaseManager.GetBetaValueDateWise(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString());
                }
                lock (_sharesOutstandingLockerObject)
                {
                    _sharesOutstandingForSymbols = DatabaseManager.GetOutstandingsValueDateWise(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString());
                }

                FillBetaSharesOutstandingMasterFundMasterStrategyValues();
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
        #endregion

        #region IDataProvider Members
        private Dictionary<String, Double> _avgVolume90Days = new Dictionary<string, double>();

        private Object _avgVolume90DaysLocker = new object();

        private Dictionary<string, List<CashActivity>> _dicTaxlotIdWiseDividend;

        //Date is not key because any two dates or more may be the same in YesterdayDate,lastDateOfMonth,DateOfQuarter,DateOfYear
        private Dictionary<Duration, DateTime> _IndicesDatesWithDurationCode;

        private DataSet _indicesMarkPrices;

        //This will synchronize the Reseting of UncalculatedData State with data sending to clients (in ServiceManager.cs).
        //Updated Code to handle the scenario :- If Uncalculated Data is changed After the GetUncalculatedDataClone() and before sending to clients.
        private bool _isUncalculatedDataChangedWhileSendingToClients = false;

        private string _todayDateTimeString;

        private string _yesterdayDateTimeString;

        public DataSet IndicesMarkPrice
        {
            get { return _indicesMarkPrices; }
        }

        public void AdjustMarkPriceByTodaysSplitFactor(bool isUpdated)
        {
            try
            {
                if (_pricingServiceProxy != null && _todayDateTimeString != null)
                {
                    _pricingServiceProxy.InnerChannel.AdjustMarkPriceByTodaysSplitFactor(_todayDateTimeString, isUpdated, TimeZoneHelper.GetInstance().MarketEndTimeInfo);
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

        public void Close()
        {
            try
            {
                if (_uncalculatedData != null)
                {
                    _uncalculatedData.Clear();
                    _uncalculatedData = null;
                }
                _symbolsMarkedForCalculation.Clear();
                _symbolWiseTaxLotsDictionary.Clear();
                _symbolWiseCurrencyIdCollection.Clear();
                lock (_betaLockerObject)
                {
                    _betaForSymbols.Clear();
                }
                lock (_sharesOutstandingLockerObject)
                {
                    _sharesOutstandingForSymbols.Clear();
                }
                _instance = null;

                lock (_calcPrefsLockerObject)
                {
                    _pmCalculationPrefCollection.Clear();
                }
                lock (_startOfMonthCapitalAccountLockerObject)
                {
                    _startOfMonthCapitalAccountCollection.Clear();
                }
                lock (_mtdPnLLockerObject)
                {
                    _userDefinedMTDPnLCollection.Clear();
                }
                lock (_dailyCreditLimitLockerObject)
                {
                    _dailyCreditLimitCollection.Clear();
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

        public List<Accurals> GetAccountWiseAccrual()
        {
            try
            {
                List<Accurals> clonedAccrual = new List<Accurals>();
                lock (_accrualLockerObject)
                {
                    //    foreach (int accountId in _accountWiseInitialAccruals.Keys)
                    //    {
                    //        //If minimum value it will not be included in cloned cache as min value key contains total accrual for dshboard.
                    //        if (accountId != int.MinValue)
                    //            clonedAccrual.Add(accountId, _accountWiseInitialAccruals[accountId]);
                    //    }

                    foreach (Account account in _accounts)
                    {
                        Accurals accruals = new Accurals();
                        accruals.AccountId = account.AccountID;

                        if (_accountWiseInitialAccruals.ContainsKey(accruals.AccountId))
                            accruals.StartOfDayAccruals = GetSODAccrualsInBase(_accountWiseInitialAccruals[accruals.AccountId], accruals.AccountId, true);

                        if (_accountWiseDayAccruals.ContainsKey(accruals.AccountId))
                            accruals.DayAccruals = GetSODAccrualsInBase(_accountWiseDayAccruals[accruals.AccountId], accruals.AccountId, false);

                        if (accruals.AccountId != int.MinValue)
                            clonedAccrual.Add(accruals);
                    }
                }
                return clonedAccrual;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public Dictionary<string, bool> GetSymbolWiseCalculationMarking()
        {
            return _symbolsMarkedForCalculation;
        }

        public Dictionary<string, List<string>> GetSymbolWiseTaxLotIDs()
        {
            return _symbolWiseTaxLotsDictionary;
        }

        public ExposureAndPnlOrderCollection GetUncalculatedDataClone()
        {
            ExposureAndPnlOrderCollection clonedCollection;
            lock (_lockerObj)
            {
                //this will clear the _accountWiseOrders cache.
                if (_isTradesDeleted)
                {
                    UpdateRemoveOrder(this, new EventArgs<EPnlOrder, ApplicationConstants.TaxLotState, bool>(null, ApplicationConstants.TaxLotState.Deleted, false));
                    _isTradesDeleted = false;
                }
                if (_isNewAUECIDsTradeAdded)
                {
                    //To update mark price cache for new auecid
                    _yesterdayDateTimeString = TimeZoneHelper.GetInstance().GetAUECOffsetBusinessAdjustedYesterdayDateTimeString();
                    InitilizeMarkPriceCache();
                    CommonCacheHelper.LoadYesterdayFXRates(_yesterdayDateTimeString);
                    _isNewAUECIDsTradeAdded = false;
                }
                clonedCollection = CloneHelper.CreateEPnlOrderCollectionClone(_uncalculatedData);
                _isUncalculatedDataChangedWhileSendingToClients = false;
            }
            return clonedCollection;
        }

        public void GetUpdatedDataFromDB(List<int> auecsList)
        {
            try
            {
                _companyID = _databaseManager.CompanyID;
                _pmuiPrefs = _databaseManager.GetCompanyPMPreferences();
                Prana.BusinessObjects.Classes.EPNL_Business_Objects.PMPrefData dbSavedPrefData = _databaseManager.GetPMPrefDataFromDB();
                CommonCacheHelper.XPercentOfAvgVolume = dbSavedPrefData.XPercentofAvgVolume;

                string todayDateTimeStringForPrint = string.Empty;
                string yesterdayDateTimeStringForPrint = string.Empty;

                List<TaxLot> allTaxlots = new List<TaxLot>();
                if (auecsList != null) // call in case of clearance only
                {
                    CommonCacheHelper.LoadYesterdayFXRates(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedYesterdayDateTimeString());

                    _todayDateTimeString = TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString(auecsList);

                    _yesterdayDateTimeString = TimeZoneHelper.GetInstance().GetAUECOffsetBusinessAdjustedYesterdayDateTimeString(auecsList);
                    todayDateTimeStringForPrint = AddExchangeIdentifierInAUECString(_todayDateTimeString);
                    yesterdayDateTimeStringForPrint = AddExchangeIdentifierInAUECString(_yesterdayDateTimeString);
                    InitilizeMarkPriceCache();

                    List<TaxLot> positions = _positionManagementServices.InnerChannel.GetOpenPositionsOrTransactions(_yesterdayDateTimeString, false, string.Empty, _fetchDataByDate);
                    List<TaxLot> transactions = _positionManagementServices.InnerChannel.GetOpenPositionsOrTransactions(_todayDateTimeString, true, string.Empty, _fetchDataByDate);
                    List<TaxLot> openUnallocatedTrades = _positionManagementServices.InnerChannel.GetUnallocatedTradesForDateString(_todayDateTimeString);
                    List<TaxLot> postDatedTransactions = _positionManagementServices.InnerChannel.GetPostDatedTransactions(_todayDateTimeString, _fetchDataByDate);

                    // Getting data for cost adjustment taxlots
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6867
                    List<CostAdjustmentTaxlotsForSave> costAdjustmentTradesData = _positionManagementServices.InnerChannel.GetCostAdjustmentData();

                    //will be updated with cash management phase 2
                    _dicTaxlotIdWiseDividend = GetCurrentAuecDatesDividends(_todayDateTimeString);
                    allTaxlots.AddRange(positions);
                    allTaxlots.AddRange(transactions);
                    allTaxlots.AddRange(openUnallocatedTrades);
                    allTaxlots.AddRange(postDatedTransactions);

                    allTaxlots = allTaxlots.Where(t => t.AssetID != (int)AssetCategory.FX).ToList();

                    // Blocking taxlots closed through cost adjustment
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6867
                    List<string> costAdjustmentClosingTaxlotIDs = costAdjustmentTradesData.Where(i => !string.IsNullOrEmpty(i.ClosingTaxlotID)).Select(k => k.ClosingTaxlotID).ToList();
                    List<TaxLot> removeClosedCostAdjustmentTaxlots = allTaxlots.Where(t => costAdjustmentClosingTaxlotIDs.Contains(t.TaxLotID)).Select(k => k).ToList();
                    allTaxlots.RemoveAll(x => removeClosedCostAdjustmentTaxlots.Contains(x));

                    ExposureAndPnlOrderCollection tempCollectionFromDB;
                    lock (_lockerObj)
                    {
                        GetExposureAndPnlOrderCollection(allTaxlots, ApplicationConstants.TaxLotState.New, false, out tempCollectionFromDB);
                    }

                    lock (_lockerObj)
                    {
                        tempCollectionFromDB.Sort();
                    }

                    lock (_lockerObj)
                    {
                        foreach (EPnlOrder temp in tempCollectionFromDB)
                        {
                            if (_dicTaxlotIdWiseDividend != null && _dicTaxlotIdWiseDividend.ContainsKey(temp.ID))
                            {
                                List<CashActivity> _objDividend = _dicTaxlotIdWiseDividend[temp.ID];
                                UpdateDividendData(temp, _objDividend);
                            }
                            if (_uncalculatedData.Contains(temp.ID))
                            {
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-6572
                                // On clearance, all the dividend data is being lost. So applied check to retail dividend data for non- cleared AUEC as it will be the same.
                                //Actual issue is in GetAUECOffsetAdjustedYesterdayDateTimeString(List<int> auecsList)
                                //As it is getting positions for all the AUEC rather than the cleared AUEC. But it is a big change so created a separate JIRA for it.

                                if (!auecsList.Contains(temp.AUECID))
                                {
                                    temp.EarnedDividendLocal = _uncalculatedData[temp.ID].EarnedDividendLocal;
                                    temp.EarnedDividendBase = _uncalculatedData[temp.ID].EarnedDividendBase;
                                    temp.ExDividendDate = _uncalculatedData[temp.ID].ExDividendDate;
                                }
                                _uncalculatedData.Remove(temp.ID);
                                _uncalculatedData.Add(temp);
                            }
                            else
                            {
                                _uncalculatedData.Add(temp);
                            }
                        }
                        PrepareSymbolGrouping();
                    }
                    lock (_betaLockerObject)
                    {
                        _betaForSymbols = DatabaseManager.GetBetaValueDateWise(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString(auecsList));
                    }
                    lock (_sharesOutstandingLockerObject)
                    {
                        _sharesOutstandingForSymbols = DatabaseManager.GetOutstandingsValueDateWise(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString(auecsList));
                    }

                    if (LogOnScreen != null)
                    {
                        string logMsgToday = "Today DateTime String :\t" + todayDateTimeStringForPrint;
                        string logMsgYesterday = "Yesterday DateTime String :\t" + yesterdayDateTimeStringForPrint;
                        LogOnScreen(logMsgToday, EventArgs.Empty);
                        LogOnScreen(logMsgYesterday, EventArgs.Empty);

                        Logger.LoggerWrite(logMsgToday);
                        Logger.LoggerWrite(logMsgYesterday);
                    }
                }
                else
                {
                    CommonCacheHelper.LoadYesterdayFXRates(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedYesterdayDateTimeString());

                    _todayDateTimeString = TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString();
                    _yesterdayDateTimeString = TimeZoneHelper.GetInstance().GetAUECOffsetBusinessAdjustedYesterdayDateTimeString();
                    todayDateTimeStringForPrint = AddExchangeIdentifierInAUECString(_todayDateTimeString);
                    yesterdayDateTimeStringForPrint = AddExchangeIdentifierInAUECString(_yesterdayDateTimeString);
                    InitilizeMarkPriceCache();

                    List<TaxLot> positions = _positionManagementServices.InnerChannel.GetOpenPositionsOrTransactions(_yesterdayDateTimeString, false, string.Empty, _fetchDataByDate);
                    List<TaxLot> transactions = _positionManagementServices.InnerChannel.GetOpenPositionsOrTransactions(_todayDateTimeString, true, string.Empty, _fetchDataByDate);
                    List<TaxLot> openUnallocatedTrades = _positionManagementServices.InnerChannel.GetUnallocatedTradesForDateString(_todayDateTimeString);
                    List<TaxLot> postDatedTransactions = _positionManagementServices.InnerChannel.GetPostDatedTransactions(_todayDateTimeString, _fetchDataByDate);
                    _dicTaxlotIdWiseDividend = GetCurrentAuecDatesDividends(_todayDateTimeString);
                    allTaxlots.AddRange(positions);
                    allTaxlots.AddRange(transactions);
                    allTaxlots.AddRange(openUnallocatedTrades);
                    allTaxlots.AddRange(postDatedTransactions);

                    allTaxlots = allTaxlots.Where(t => t.AssetID != (int)AssetCategory.FX).ToList();

                    // Getting data for cost adjustment taxlots
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6867
                    List<CostAdjustmentTaxlotsForSave> costAdjustmentTradesData = _positionManagementServices.InnerChannel.GetCostAdjustmentData();

                    // Blocking taxlots closed through cost adjustment
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-6867
                    List<string> costAdjustmentClosingTaxlotIDs = costAdjustmentTradesData.Where(i => !string.IsNullOrEmpty(i.ClosingTaxlotID)).Select(k => k.ClosingTaxlotID).ToList();
                    List<TaxLot> removeClosedCostAdjustmentTaxlots = allTaxlots.Where(t => costAdjustmentClosingTaxlotIDs.Contains(t.TaxLotID)).Select(k => k).ToList();
                    allTaxlots.RemoveAll(x => removeClosedCostAdjustmentTaxlots.Contains(x));

                    ExposureAndPnlOrderCollection tempCollectionFromDB;
                    lock (_lockerObj)
                    {
                        GetExposureAndPnlOrderCollection(allTaxlots, ApplicationConstants.TaxLotState.New, false, out tempCollectionFromDB);
                    }

                    lock (_lockerObj)
                    {
                        tempCollectionFromDB.Sort();
                    }

                    foreach (EPnlOrder temp in tempCollectionFromDB)
                    {
                        if (_dicTaxlotIdWiseDividend != null && _dicTaxlotIdWiseDividend.ContainsKey(temp.ID))
                        {
                            List<CashActivity> _objDividend = _dicTaxlotIdWiseDividend[temp.ID];
                            UpdateDividendData(temp, _objDividend);
                        }
                    }

                    tempCollectionFromDB.LastDBPickTime = DateTime.Now;
                    lock (_lockerObj)
                    {
                        _uncalculatedData = tempCollectionFromDB;
                        PrepareSymbolGrouping();
                    }
                    lock (_betaForSymbols)
                    {
                        _betaForSymbols = DatabaseManager.GetBetaValueDateWise(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString());
                    }
                    lock (_sharesOutstandingLockerObject)
                    {
                        _sharesOutstandingForSymbols = DatabaseManager.GetOutstandingsValueDateWise(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString());
                    }

                    if (LogOnScreen != null)
                    {
                        LogOnScreen("Today DateTime String :\t" + todayDateTimeStringForPrint, EventArgs.Empty);
                        LogOnScreen("Yesterday DateTime String :\t" + yesterdayDateTimeStringForPrint, EventArgs.Empty);
                    }

                    lock (_accountWiseNraLockerObject)
                    {
                        _accountWiseNra = DatabaseManager.GetAccountWiseNra();
                    }
                    if (_pmuiPrefs.FetchDataFromHistoricalDb)
                    {
                        FillAvgVolume90Days();
                    }
                }

                ForexConverter.GetInstance(_companyID).GetForexData();
                _companyAccountMasterFundRelation = DatabaseManager.GetCompanyAccountMasterFundRelation();
                _companyStrategyMasterStrategyRelation = DatabaseManager.GetCompanyStrategyMasterStrategyRelation();

                FillBetaSharesOutstandingMasterFundMasterStrategyValues();

                _indexDS = DatabaseManager.GetIndexSymbols();
                //Create all of the columns for all indices day/month/quarter/year returns herer
                _dtIndicesReturn = CommonCacheHelper.GetIndicesReturnTable(_indexDS);
                StringBuilder indicesSB = new StringBuilder();

                foreach (DataRow row in _indexDS.Tables[0].Rows)
                {
                    indicesSB.Append(row["IndexSymbol"].ToString());
                    indicesSB.Append(",");
                }

                string indices = indicesSB.ToString().TrimEnd(new char[1] { ',' });

                string allDates = GetAllDates();
                DateTime BusinessAdjustedYesterdayDate = Convert.ToDateTime(yesterdayDate);
                DateTime BusinessAdjustedlastDateOfMonth = Convert.ToDateTime(lastDateOfMonth);
                DateTime BusinessAdjustedlastDateOfQuarter = Convert.ToDateTime(lastDateOfQuarter);
                DateTime BusinessAdjustedlastDateOfYear = Convert.ToDateTime(lastDateOfYear);

                if (BusinessAdjustedYesterdayDate != null && BusinessAdjustedlastDateOfMonth != null && BusinessAdjustedlastDateOfQuarter != null && BusinessAdjustedlastDateOfYear != null)
                {
                    _IndicesDatesWithDurationCode = new Dictionary<Duration, DateTime>();
                    _IndicesDatesWithDurationCode.Add((Duration)0, BusinessAdjustedYesterdayDate.Date);
                    _IndicesDatesWithDurationCode.Add((Duration)1, BusinessAdjustedlastDateOfMonth.Date);
                    _IndicesDatesWithDurationCode.Add((Duration)2, BusinessAdjustedlastDateOfQuarter.Date);
                    _IndicesDatesWithDurationCode.Add((Duration)3, BusinessAdjustedlastDateOfYear.Date);
                }

                _indicesMarkPrices = DatabaseManager.GetMarkPriceForDatesAndSymbols(indices, allDates);

                FillSummaryCollection();

                TimeZoneHelper.GetInstance().GetNextClearanceDateTimes();
                _orderFillManager.AuecWiseLocalDates = new System.Collections.Concurrent.ConcurrentDictionary<int, DateTime>(TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates);

                #region Stopout Collection
                // Stopout Dictionaries Collection is Accountwise Collection, so we use most leading time zone date.
                DateTime latestDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true);

                lock (_calcPrefsLockerObject)
                {
                    _pmCalculationPrefCollection = _databaseManager.GetCompanyPMCalculationPreferences();
                }
                lock (_startOfMonthCapitalAccountLockerObject)
                {
                    _startOfMonthCapitalAccountCollection = _databaseManager.GetStartOfMonthCapitalAccountValues(latestDate);
                }
                lock (_mtdPnLLockerObject)
                {
                    _userDefinedMTDPnLCollection = _databaseManager.GetUserDefinedMTDPnLValues(latestDate);
                }
                #endregion

                #region Compliance Section
                //Sending dbnav and cash event to esper through amqp-plugin manager
                try
                {
                    if (AuecDetailsUpdated != null)
                    {
                        if (auecsList == null)
                            foreach (AuecDetails auecDetails in GetAUECDetails())
                                AuecDetailsUpdated(this, new EventArgs<AuecDetails>(auecDetails));
                        else
                            foreach (int auecId in auecsList)
                                AuecDetailsUpdated(this, new EventArgs<AuecDetails>(GetAUECDetails(auecId)));
                    }

                    if (YesterdayFxRateUpdated != null)
                    {
                        if (auecsList == null)
                            YesterdayFxRateUpdated(this, new EventArgs<List<YesterdayFxRate>>(GetYesterdayFxRates()));
                        else
                            YesterdayFxRateUpdated(this, new EventArgs<List<YesterdayFxRate>>(GetYesterdayFxRates(auecsList)));
                    }

                    if (DayEndCashReceived != null)
                        DayEndCashReceived(this, new EventArgs<List<DayEndAccountCash>>(GetDayEndAccountCash()));

                    if (AccountWiseAccrualEvent != null)
                        AccountWiseAccrualEvent(this, new EventArgs<List<Accurals>>(GetAccountWiseAccrual()));

                    if (DbNavUpdate != null)
                        DbNavUpdate(this, new EventArgs<List<DbNav>>(GetDbNav()));
                    if (StartOfMonthCapitalAccountUpdated != null)
                        StartOfMonthCapitalAccountUpdated(this, new EventArgs<Dictionary<int, double>>(_startOfMonthCapitalAccountCollection));
                    if (UserDefinedMTDPnLCollectionUpdated != null)
                        UserDefinedMTDPnLCollectionUpdated(this, new EventArgs<Dictionary<int, double>>(_userDefinedMTDPnLCollection));
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                #endregion
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

        public void ResetUncalculatedDataState()
        {
            try
            {
                lock (_lockerObj)
                {
                    if (!_isUncalculatedDataChangedWhileSendingToClients)
                    {
                        foreach (EPnlOrder cachedOrder in _uncalculatedData)
                        {
                            cachedOrder.IsMarkedForCalculation = false;
                            //Resetting the state so that new/updated orders completly sent only once.
                            cachedOrder.HasBeenSentToUser = 1;
                            cachedOrder.PricingSource = PricingSource.None;
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

        public void SetQuartzScheduler()
        {
            try
            {
                CreateSubscriptionServicesProxyPricing();
                CreateSubscriptionServicesProxyServer();

                ExposurePnLScheduler.GetInstance().ScheduleTasks();

                /// Here cash service is being subscribed as cash service subscription depends on the date to pick up cash.
                //TODO : It could have been easily after the SetQuartzScheduler function.
                SubscribeCashService();
                SubscribeDayEndCash();
                SubscribeDailyCreditLimit();
                SubscribeCashData();
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

        private string AddExchangeIdentifierInAUECString(string timeStringForPrint)
        {
            StringBuilder dateTimeStringFormPrint = new StringBuilder();
            try
            {
                string[] stringSplittedByTilde = timeStringForPrint.Split(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                foreach (string item in stringSplittedByTilde)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        string ExchangeName = "Default";
                        string[] stringSplittedByXOR = item.Split(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                        int AuecID = Int32.Parse(stringSplittedByXOR[0]);
                        DateTime dateTime = DateTime.Parse(stringSplittedByXOR[1]);
                        if (AuecID != 0)
                        {
                            ExchangeName = CachedDataManager.GetInstance.GetAUECText(AuecID);
                        }
                        dateTimeStringFormPrint.Append("[");
                        dateTimeStringFormPrint.Append(AuecID);
                        dateTimeStringFormPrint.Append("(" + ExchangeName + ")");
                        dateTimeStringFormPrint.Append(" - ");
                        dateTimeStringFormPrint.Append(dateTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
                        dateTimeStringFormPrint.Append("] ");
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
            return dateTimeStringFormPrint.ToString();
        }

        private void FillAvgVolume90Days()
        {
            try
            {
                DataSet avgVolume = new DataSet();
                int days = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("AvgVolumeNumOfDays"));
                avgVolume = DatabaseManager.GetAvgVolumeSymbolWise(DateTime.Now, days);

                lock (_avgVolume90DaysLocker)
                {
                    foreach (DataRow dr in avgVolume.Tables[0].Rows)
                    {
                        String symbol = dr["Symbol"].ToString();
                        if (_avgVolume90Days.ContainsKey(symbol))
                            _avgVolume90Days[symbol] = Convert.ToDouble(dr["AverageVolume"].ToString());
                        else
                            _avgVolume90Days.Add(symbol, Convert.ToDouble(dr["AverageVolume"].ToString()));
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

        //update TransactionEntry > Cash Activity
        private Dictionary<string, List<CashActivity>> GetCurrentAuecDatesDividends(string todayDateTimeString)
        {
            Dictionary<string, List<CashActivity>> _dicTaxlotIdWiseDividend = new Dictionary<string, List<CashActivity>>();
            try
            {
                DataSet dsDividends = _cashMgmtService.InnerChannel.GetCashDividendFromActivities(todayDateTimeString);
                if (dsDividends != null && dsDividends.Tables[0].Rows.Count > 0)
                {
                    List<CashActivity> _dividendTaxLot;
                    CashActivity _appliedDividend;
                    foreach (DataRow dr in dsDividends.Tables[0].Rows)
                    {
                        _dividendTaxLot = new List<CashActivity>();
                        _appliedDividend = new CashActivity();
                        _appliedDividend.Amount = DBNull.Value.Equals(Convert.ToDecimal(dr["Amount"])) ? 0 : Convert.ToDecimal(dr["Amount"]);
                        //Transaction ID will be taxlot id
                        _appliedDividend.TaxLotId = DBNull.Value.Equals(dr["TaxlotId"]) ? string.Empty : dr["TaxlotId"].ToString();
                        //Date will be ExDate
                        _appliedDividend.Date = DBNull.Value.Equals(dr["ExDate"]) ? DateTime.MinValue : Convert.ToDateTime(dr["ExDate"]);
                        _appliedDividend.FKID = dr["CashTransactionId"].ToString();
                        _appliedDividend.FXRate = DBNull.Value.Equals(dr["FXRate"]) ? 0 : Convert.ToDouble(dr["FXRate"]);
                        _appliedDividend.FXConversionMethodOperator = DBNull.Value.Equals(dr["FXConversionMethodOperator"]) ? string.Empty : dr["FXConversionMethodOperator"].ToString();

                        _dividendTaxLot.Add(_appliedDividend);

                        if (!_dicTaxlotIdWiseDividend.ContainsKey(_appliedDividend.TaxLotId))
                            _dicTaxlotIdWiseDividend.Add(_appliedDividend.TaxLotId, _dividendTaxLot);
                        else
                            _dicTaxlotIdWiseDividend[_appliedDividend.TaxLotId].Add(_appliedDividend);
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
            return _dicTaxlotIdWiseDividend;
        }

        private Dictionary<String, EPnlOrder> GetExposureAndPnlOrderCollection(List<TaxLot> allTaxlots, ApplicationConstants.TaxLotState taxlotState, bool isPublishRequest, out ExposureAndPnlOrderCollection tempCollectionFromDB)
        {
            tempCollectionFromDB = new ExposureAndPnlOrderCollection();
            Dictionary<String, EPnlOrder> modifiedTaxlotCache = new Dictionary<String, EPnlOrder>();
            try
            {
                if (allTaxlots != null)
                {
                    List<string> symbols = allTaxlots.Where(t => (AssetCategory)t.AssetID != AssetCategory.FX && (AssetCategory)t.AssetID != AssetCategory.FXForward).Select(o => o.Symbol).Distinct().ToList();
                    LiveFeedManager.Instance.AdviseSymbolBulk(symbols);

                    foreach (TaxLot taxlot in allTaxlots)
                    {

                        EPnlOrder order = CommonCacheHelper.GetEPnlOrderFromTaxlot(taxlot);

                        if (tempCollectionFromDB.Contains(order.ID))
                        {
                            // If the same order id exist but the new order comes with the latest date, then it would be shown
                            //(e.g. in case of split applied on current date)
                            EPnlOrder existingOrder = tempCollectionFromDB[order.ID];
                            if (existingOrder.AUECLocalDate.Date < order.AUECLocalDate.Date)
                            {
                                tempCollectionFromDB.Remove(existingOrder.ID);
                                tempCollectionFromDB.Add(order);
                            }
                        }
                        else
                        {
                            tempCollectionFromDB.Add(order);
                        }

                        if (order.Asset == AssetCategory.FX)
                            LiveFeedManager.Instance.AdviseSymbolForFX(order.Symbol, ((EPnLOrderFX)order).LeadCurrencyID, ((EPnLOrderFX)order).VsCurrencyID, order.Asset);
                        else if (order.Asset == AssetCategory.FXForward)
                            LiveFeedManager.Instance.AdviseSymbolForFX(order.Symbol, ((EPnLOrderFXForward)order).LeadCurrencyID, ((EPnLOrderFXForward)order).VsCurrencyID, order.Asset);
                        //else
                        //    LiveFeedManager.Instance.AdviseSymbol(order.Symbol);

                        if (_isTaxlotsBrokenForBoxedPositions && isPublishRequest)
                        {
                            int accountID = 0;
                            if (_splittedTaxlotsCacheBasis == 1)
                                accountID = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(order.Level1ID);
                            else
                                accountID = order.Level1ID > 0 ? order.Level1ID : -1;

                            _uncalculatedData.RemoveOrders(order.Symbol, accountID, _splittedTaxlotsCacheBasis, out modifiedTaxlotCache);
                            _isTradesDeleted = true;
                        }
                    }

                    if (_isTaxlotsBrokenForBoxedPositions)
                    {
                        tempCollectionFromDB = SplitManager.GetInstance().GetSplittedUncalculatedData(tempCollectionFromDB, taxlotState, isPublishRequest);
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
            return modifiedTaxlotCache;
        }

        private void InitializeMarkPricesCacheAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_pricingServiceProxy != null && _yesterdayDateTimeString != null)
                {
                    _pricingServiceProxy.InnerChannel.InitializeMarkPricesCache(_yesterdayDateTimeString);
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

        private void InitializeMarkPricesCacheAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                AdjustMarkPriceByTodaysSplitFactor(false);
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

        private void InitilizeMarkPriceCache()
        {
            try
            {
                lock (_lockerObj)
                {
                    if (_pricingServiceProxy != null && _yesterdayDateTimeString != null)
                    {
                        BackgroundWorker InitializeMarkPricesCacheAsync = new BackgroundWorker();
                        InitializeMarkPricesCacheAsync.DoWork += new DoWorkEventHandler(InitializeMarkPricesCacheAsync_DoWork);
                        InitializeMarkPricesCacheAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InitializeMarkPricesCacheAsync_RunWorkerCompleted);
                        InitializeMarkPricesCacheAsync.RunWorkerAsync();
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

        private void UpdateDividendData(EPnlOrder temp, List<CashActivity> _objDividend)
        {
            try
            {
                foreach (CashActivity cashActivity in _objDividend)
                {
                    temp.EarnedDividendLocal += Convert.ToDouble(cashActivity.Amount);

                    if ((temp.FXConversionMethodOperator == Operator.M || temp.FXConversionMethodOperator == Operator.Multiple) && temp.FxRate != 0)
                        temp.EarnedDividendBase += Convert.ToDouble(cashActivity.Amount) * temp.FxRate;
                    else if (temp.FXConversionMethodOperator == Operator.D && temp.FxRate != 0)
                        temp.EarnedDividendBase += Convert.ToDouble(cashActivity.Amount) / temp.FxRate;
                    else if (temp.FxRate != 0)
                        temp.EarnedDividendBase += Convert.ToDouble(cashActivity.Amount) * temp.FxRate;
                    else
                        temp.EarnedDividendBase += Convert.ToDouble(cashActivity.Amount);
                    temp.ExDividendDate = cashActivity.Date;
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
        #endregion

        private void FillBeta()
        {
            try
            {
                foreach (EPnlOrder uncalculatedOrder in _uncalculatedData)
                {
                    lock (_betaLockerObject)
                    {
                        if (uncalculatedOrder is EPnLOrderOption || uncalculatedOrder is EPnLOrderFXForward)
                        {
                            if (_betaForSymbols != null && _betaForSymbols.ContainsKey(uncalculatedOrder.UnderlyingSymbol))
                            {
                                uncalculatedOrder.Beta = _betaForSymbols[uncalculatedOrder.UnderlyingSymbol].Price;
                            }
                        }
                        else
                        {
                            if (_betaForSymbols != null && _betaForSymbols.ContainsKey(uncalculatedOrder.Symbol))
                            {
                                uncalculatedOrder.Beta = _betaForSymbols[uncalculatedOrder.Symbol].Price;
                            }
                        }
                    }
                    //Done for the requirement where 0 is never displayed for Beta, the default value is 1
                    if (uncalculatedOrder.Beta == 0)
                    {
                        uncalculatedOrder.Beta = 1;
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

        private void FillBetaSharesOutstandingMasterFundMasterStrategyValues()
        {
            try
            {
                foreach (EPnlOrder uncalculatedOrder in _uncalculatedData)
                {
                    lock (_betaLockerObject)
                    {
                        if (uncalculatedOrder is EPnLOrderOption || uncalculatedOrder is EPnLOrderFXForward)
                        {
                            if (_betaForSymbols != null && _betaForSymbols.ContainsKey(uncalculatedOrder.UnderlyingSymbol))
                            {
                                uncalculatedOrder.Beta = _betaForSymbols[uncalculatedOrder.UnderlyingSymbol].Price;
                            }
                        }
                        else
                        {
                            if (_betaForSymbols != null && _betaForSymbols.ContainsKey(uncalculatedOrder.Symbol))
                            {
                                uncalculatedOrder.Beta = _betaForSymbols[uncalculatedOrder.Symbol].Price;
                            }
                        }
                    }
                    //Done for the requirement where 0 is never displayed for Beta, the default value is 1
                    if (uncalculatedOrder.Beta == 0)
                    {
                        uncalculatedOrder.Beta = 1;
                    }

                    lock (_sharesOutstandingLockerObject)
                    {
                        if (_sharesOutstandingForSymbols != null && _sharesOutstandingForSymbols.ContainsKey(uncalculatedOrder.Symbol))
                        {
                            uncalculatedOrder.SharesOutstanding = _sharesOutstandingForSymbols[uncalculatedOrder.Symbol];
                        }
                    }

                    //fill masterFund Data
                    if (_companyAccountMasterFundRelation.ContainsKey(uncalculatedOrder.Level1ID))
                    {
                        uncalculatedOrder.MasterFundID = _companyAccountMasterFundRelation[uncalculatedOrder.Level1ID];
                    }
                    else
                    {
                        uncalculatedOrder.MasterFundID = -1;
                    }

                    //fill masterStrategy Data
                    if (_companyStrategyMasterStrategyRelation.ContainsKey(uncalculatedOrder.Level2ID))
                    {
                        uncalculatedOrder.MasterStrategyID = _companyStrategyMasterStrategyRelation[uncalculatedOrder.Level2ID];
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

        private void FillSharesOutstanding()
        {
            try
            {
                lock (_sharesOutstandingLockerObject)
                {
                    foreach (EPnlOrder uncalculatedOrder in _uncalculatedData)
                    {
                        if (_sharesOutstandingForSymbols.ContainsKey(uncalculatedOrder.Symbol))
                        {
                            uncalculatedOrder.SharesOutstanding = _sharesOutstandingForSymbols[uncalculatedOrder.Symbol];
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

        private void UpdateSecurityMasterInfo(SecMasterbaseList secMasterUpdatedList)
        {
            try
            {
                lock (_lockerObj)
                {
                    _isUncalculatedDataChangedWhileSendingToClients = true;
                    foreach (EPnlOrder uncalculatedOrder in _uncalculatedData)
                    {
                        foreach (SecMasterBaseObj obj in secMasterUpdatedList)
                        {
                            if (obj.TickerSymbol.Equals(uncalculatedOrder.Symbol, StringComparison.OrdinalIgnoreCase))
                            {
                                uncalculatedOrder.CopyBasicDetails(obj, false);
                                //Modified by omshiv, Feb 17, Only updated order should be sent to Client not all
                                uncalculatedOrder.HasBeenSentToUser = 0;
                            }
                            else if (_isAutoUpdateOptionsUDAWithUnderlyingUpdate && obj.TickerSymbol.Equals(uncalculatedOrder.UnderlyingSymbol, StringComparison.OrdinalIgnoreCase))
                            {
                                uncalculatedOrder.CopyBasicDetails(obj, true);

                                //Modified by omshiv, Feb 17, Only updated order should be sent to Client not all
                                uncalculatedOrder.HasBeenSentToUser = 0;
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

        #region Indices Section
        private Dictionary<DateTime, Dictionary<string, double>> _UpdatedIndicesDatewiseMarkPrice;
        private string lastDateOfMonth, lastDateOfQuarter, lastDateOfYear, yesterdayDate;

        public void UpdateIndicesMarkCache(Dictionary<string, double> _indicesMarkPriceCache)
        {
            try
            {
                foreach (DateTime date in _UpdatedIndicesDatewiseMarkPrice.Keys)
                {
                    //-- This is becouse here may be same date for more than one duration code
                    foreach (Duration dur in _IndicesDatesWithDurationCode.Keys)
                    {
                        if (date == _IndicesDatesWithDurationCode[dur])
                        {
                            string key;
                            foreach (string symbol in _UpdatedIndicesDatewiseMarkPrice[date].Keys)
                            {
                                key = symbol + "_" + dur.ToString();
                                double markPrice = _UpdatedIndicesDatewiseMarkPrice[date][symbol];

                                lock (_indicesMarkPriceCache)
                                {
                                    if (_indicesMarkPriceCache.ContainsKey(key))
                                    {
                                        _indicesMarkPriceCache[key] = markPrice;
                                    }
                                    else
                                    {
                                        _indicesMarkPriceCache.Add(key, markPrice);
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

        private void FillUpdatedIndicesCacheThroughPublish(DateTime date, string symbol, double markPrice)
        {
            try
            {
                Dictionary<string, double> _dicNewSymbolWithMarkPrice;
                if (date != null && symbol != null)
                {
                    if (_UpdatedIndicesDatewiseMarkPrice.ContainsKey(date.Date))
                    {
                        if (_UpdatedIndicesDatewiseMarkPrice[date].ContainsKey(symbol))
                            _UpdatedIndicesDatewiseMarkPrice[date][symbol] = markPrice;
                        else
                        {
                            _UpdatedIndicesDatewiseMarkPrice[date].Add(symbol, markPrice);
                        }
                    }
                    else
                    {
                        _dicNewSymbolWithMarkPrice = new Dictionary<string, double>();
                        _dicNewSymbolWithMarkPrice.Add(symbol, markPrice);
                        _UpdatedIndicesDatewiseMarkPrice.Add(date.Date, _dicNewSymbolWithMarkPrice);
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

        private string GetAllDates()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int auecid = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier("Indices-Indices");
                if (auecid != int.MinValue)
                {
                    DateTime currentAUECDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(auecid));
                    yesterdayDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(currentAUECDate, -1, auecid).Date.ToString();

                    lastDateOfMonth = BusinessDayCalculator.GetInstance().GetLastBusinessDateOfMonth(auecid, currentAUECDate);
                    lastDateOfQuarter = BusinessDayCalculator.GetInstance().GetLastBusinessDateOfQuarter(auecid, currentAUECDate);
                    lastDateOfYear = BusinessDayCalculator.GetInstance().GetLastBusinessDateOfYear(auecid, currentAUECDate);

                    sb.Append(yesterdayDate);
                    sb.Append(",");
                    sb.Append(lastDateOfMonth);
                    sb.Append(",");
                    sb.Append(lastDateOfQuarter);
                    sb.Append(",");
                    sb.Append(lastDateOfYear);
                }
                else
                {
                    throw new Exception("Either AUECId for Indices with Exchange Identifier Indices-Indices not exist or clearance cache does not contain Indices data.Please check the same");
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
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// Updating credit limit cache.
        /// Bharat Kumar Jangir (13 June 2014)
        /// </summary>
        /// <param name="dailyCreditLimitCollection"></param>
        private void UpdateDailyCreditLimit()
        {
            try
            {
                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_dailyCreditLimitCollection.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.LongDebitLimit = _dailyCreditLimitCollection[emptySummary.Key].LongDebitLimit;
                        emptySummary.Value.ShortCreditLimit = _dailyCreditLimitCollection[emptySummary.Key].ShortCreditLimit;
                        emptySummary.Value.LongDebitBalance = _dailyCreditLimitCollection[emptySummary.Key].LongDebitBalance;
                        emptySummary.Value.ShortCreditBalance = _dailyCreditLimitCollection[emptySummary.Key].ShortCreditBalance;
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

        private void UpdateDailyCreditLimitCollection(DataTable dtDailyCreditLimit)
        {
            try
            {
                lock (_dailyCreditLimitLockerObject)
                {
                    if (dtDailyCreditLimit != null)
                    {
                        for (int i = 0; i < dtDailyCreditLimit.Rows.Count; i++)
                        {
                            if (_dailyCreditLimitCollection.ContainsKey(Convert.ToInt32(dtDailyCreditLimit.Rows[i][0])))
                            {
                                DailyCreditLimit dailyCreditLimit = new DailyCreditLimit();
                                dailyCreditLimit.AccountID = Convert.ToInt32(dtDailyCreditLimit.Rows[i][0]);
                                dailyCreditLimit.LongDebitLimit = Convert.ToDouble(dtDailyCreditLimit.Rows[i][1]);
                                dailyCreditLimit.ShortCreditLimit = Convert.ToDouble(dtDailyCreditLimit.Rows[i][2]);
                                dailyCreditLimit.LongDebitBalance = Convert.ToDouble(dtDailyCreditLimit.Rows[i][3]);
                                dailyCreditLimit.ShortCreditBalance = Convert.ToDouble(dtDailyCreditLimit.Rows[i][4]);

                                _dailyCreditLimitCollection[dailyCreditLimit.AccountID] = dailyCreditLimit;
                            }
                        }
                    }
                }

                UpdateDailyCreditLimit();
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

        private void UpdateDailyPerformanceNumbers()
        {
            try
            {
                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_accountWiseKeyReturns.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.MTDPnL = _accountWiseKeyReturns[emptySummary.Key].MTDPnL;
                        emptySummary.Value.YTDPnL = _accountWiseKeyReturns[emptySummary.Key].YTDPnL;
                        emptySummary.Value.QTDPnL = _accountWiseKeyReturns[emptySummary.Key].QTDPnL;
                        emptySummary.Value.MTDReturn = _accountWiseKeyReturns[emptySummary.Key].MTDReturn;
                        emptySummary.Value.QTDReturn = _accountWiseKeyReturns[emptySummary.Key].QTDReturn;
                        emptySummary.Value.YTDReturn = _accountWiseKeyReturns[emptySummary.Key].YTDReturn;
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

        private void UpdateDailyPerformancenumbersCollection(DataTable dtPerformanceNumber)
        {
            try
            {
                lock (_accountWiseKeyReturnLockerObject)
                {
                    foreach (int key in _accountWiseKeyReturns.Keys.ToList())
                    {
                        _accountWiseKeyReturns[key] = new KeyReturns();
                    }
                    if (dtPerformanceNumber != null)
                    {
                        for (int i = 0; i < dtPerformanceNumber.Rows.Count; i++)
                        {
                            KeyReturns dailyPerformancenumbers = new KeyReturns();
                            dailyPerformancenumbers.MTDPnL = Convert.ToDouble(dtPerformanceNumber.Rows[i][1]);
                            dailyPerformancenumbers.QTDPnL = Convert.ToDouble(dtPerformanceNumber.Rows[i][2]);
                            dailyPerformancenumbers.YTDPnL = Convert.ToDouble(dtPerformanceNumber.Rows[i][3]);
                            dailyPerformancenumbers.MTDReturn = Convert.ToDouble(dtPerformanceNumber.Rows[i][4]);
                            dailyPerformancenumbers.QTDReturn = Convert.ToDouble(dtPerformanceNumber.Rows[i][5]);
                            dailyPerformancenumbers.YTDReturn = Convert.ToDouble(dtPerformanceNumber.Rows[i][6]);

                            if (_accountWiseKeyReturns.ContainsKey(Convert.ToInt32(dtPerformanceNumber.Rows[i][0])))
                            {
                                _accountWiseKeyReturns[Convert.ToInt32(dtPerformanceNumber.Rows[i][0])] = dailyPerformancenumbers;
                            }
                            else
                            {
                                _accountWiseKeyReturns.Add(Convert.ToInt32(dtPerformanceNumber.Rows[i][0]), dailyPerformancenumbers);
                            }
                        }
                    }
                }

                UpdateDailyPerformanceNumbers();
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

        private void UpdateDayEndCash(Dictionary<int, double> accountWiseCashList)
        {
            try
            {
                //Dictionary<int, double> newTempDictionary = new Dictionary<int, double>(_accountWiseInitialCash);

                //foreach (int cashKeyValue in newTempDictionary.Keys)
                //{
                //    _accountWiseInitialCash[cashKeyValue] = 0;
                //}

                foreach (KeyValuePair<int, double> accountCash in accountWiseCashList)
                {
                    if (_accountWiseInitialCash.ContainsKey(accountCash.Key))
                    {
                        _accountWiseInitialCash[accountCash.Key] = accountCash.Value;
                    }
                }

                double total = 0.0;
                foreach (KeyValuePair<int, double> accountCash in _accountWiseInitialCash)
                {
                    if (!accountCash.Key.Equals(int.MinValue))
                        total += accountCash.Value;
                }

                /// Assign the total cash into consolidation value
                if (_accountWiseInitialCash.ContainsKey(int.MinValue))
                {
                    _accountWiseInitialCash[int.MinValue] = total;
                }
                else
                {
                    _accountWiseInitialCash.Add(int.MinValue, total);
                }
                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_accountWiseInitialCash.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.StartOfDayCash = _accountWiseInitialCash[emptySummary.Key];
                        emptySummary.Value.CashProjected = _accountWiseInitialCash[emptySummary.Key];

                        if (!_isNAVSaved)
                        {
                            emptySummary.Value.YesterdayNAV = _accountWiseInitialCash[emptySummary.Key];
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

        private void UpdateStartOfDayAccruals(Dictionary<int, Dictionary<int, double>> accountWiseAccrualsList)
        {
            try
            {
                double total = 0.0;
                Dictionary<int, double> temp = new Dictionary<int, double>();
                foreach (KeyValuePair<int, Dictionary<int, double>> accountAccruals in accountWiseAccrualsList)
                {
                    if (_accountWiseInitialAccruals.ContainsKey(accountAccruals.Key))
                    {
                        _accountWiseInitialAccruals[accountAccruals.Key] = accountAccruals.Value;
                    }
                    total += GetSODAccrualsInBase(accountAccruals.Value, accountAccruals.Key, true);
                }
                // by this _accountWiseInitialAccruals will have base value in int.minval fund and local for others. Need to check
                temp.Add(0, total);
                /// Assign the total accruals into consolidation value
                if (_accountWiseInitialAccruals.ContainsKey(int.MinValue))
                {
                    _accountWiseInitialAccruals[int.MinValue] = temp;
                }
                else
                {
                    _accountWiseInitialAccruals.Add(int.MinValue, temp);
                }
                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_accountWiseInitialAccruals.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.StartOfDayAccruals = GetSODAccrualsInBase(_accountWiseInitialAccruals[emptySummary.Key], emptySummary.Key, true);
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

        private double GetSODAccrualsInBase(Dictionary<int, double> fundWiseAccrualsinLocalCurrency, int accountId, bool isSOD)
        {
            double fundWiseAccrualsInBaseCurrency = 0;
            try
            {
                foreach (KeyValuePair<int, double> localCash in fundWiseAccrualsinLocalCurrency)
                {
                    SymbolData fxLevel1Data = null;
                    int accountBaseCurrency;
                    double fxRate = 1;
                    double yesterdayFxRate = 1;
                    if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(accountId))
                    {
                        accountBaseCurrency = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[accountId];
                    }
                    else
                    {
                        accountBaseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    }
                    if (localCash.Key != accountBaseCurrency)
                    {
                        string forexSymbol = ForexConverter.GetInstance(_companyID).GetPranaForexSymbolFromCurrencies(localCash.Key, accountBaseCurrency);
                        fxLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(forexSymbol, localCash.Key, accountBaseCurrency, AssetCategory.Forex);

                        if (fxLevel1Data != null)
                        {
                            fxRate = fxLevel1Data.SelectedFeedPrice;
                        }
                        else
                        {
                            ConversionRate todaysConversionRate = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(localCash.Key, DateTime.Now, accountId);
                            if (todaysConversionRate != null)
                            {
                                if (todaysConversionRate.ConversionMethod == Operator.M)
                                {
                                    fxRate = todaysConversionRate.RateValue;
                                }
                                else if (todaysConversionRate.RateValue != 0 && todaysConversionRate.ConversionMethod == Operator.D)
                                {
                                    fxRate = (1 / todaysConversionRate.RateValue);
                                }
                            }
                        }
                        ConversionRate yesterdaysConversionRate = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrency(localCash.Key, DateTime.Now.AddDays(-1), accountId);
                        if (yesterdaysConversionRate != null)
                        {
                            if (yesterdaysConversionRate.ConversionMethod == Operator.M)
                            {
                                yesterdayFxRate = yesterdaysConversionRate.RateValue;
                            }
                            else if (yesterdaysConversionRate.RateValue != 0 && yesterdaysConversionRate.ConversionMethod == Operator.D)
                            {
                                yesterdayFxRate = (1 / yesterdaysConversionRate.RateValue);
                            }
                        }
                        if (isSOD)
                        {
                            fxRate = yesterdayFxRate;
                        }
                        fundWiseAccrualsInBaseCurrency += localCash.Value * fxRate;
                    }
                    else
                    {
                        fundWiseAccrualsInBaseCurrency += localCash.Value * 1;
                    }
                    //multiply each Value of passed dictionary with FX Rate of corresponding Currency. and return sum.
                    //foreach (KeyValuePair<int, double> localCash in fundWiseAccrualsinLocalCurrency)
                    //{
                    //    ConversionRate conversionRate = ForexConverter.GetInstance(_companyID).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(localCash.Key, DateTime.Now, accountId);
                    //    fundWiseAccrualsInBaseCurrency += localCash.Value * (conversionRate.ConversionMethod.ToString().Equals("M") ? conversionRate.RateValue : 1 / conversionRate.RateValue);
                    //}
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
            return fundWiseAccrualsInBaseCurrency;
        }

        private void UpdateAccrualValueInSummary()
        {
            try
            {
                if (_summaries != null)
                {
                    foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                    {
                        if (_accountWiseDayAccruals.ContainsKey(emptySummary.Key) && _accountWiseDayAccruals[emptySummary.Key].Count > 0)
                        {
                            emptySummary.Value.DayAccruals = GetSODAccrualsInBase(_accountWiseDayAccruals[emptySummary.Key], emptySummary.Key, false);
                        }
                        if (_accountWiseInitialAccruals.ContainsKey(emptySummary.Key) && _accountWiseInitialAccruals[emptySummary.Key].Count > 0)
                        {
                            emptySummary.Value.StartOfDayAccruals = GetSODAccrualsInBase(_accountWiseInitialAccruals[emptySummary.Key], emptySummary.Key, true);
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
            }
        }
        private void UpdateDayCashImpact(Dictionary<int, Tuple<double, double>> accountWiseDayCashList)
        {
            try
            {
                Dictionary<int, Tuple<double, double>> newTempDictionary = new Dictionary<int, Tuple<double, double>>(_accountWiseDayCash);
                foreach (int cashKeyValue in newTempDictionary.Keys)
                {
                    _accountWiseDayCash[cashKeyValue] = new Tuple<double, double>(0, 0);
                }

                double totalDR = 0.0;
                double totalCR = 0.0;
                foreach (KeyValuePair<int, Tuple<double, double>> accountCash in accountWiseDayCashList)
                {
                    if (_accountWiseDayCash.ContainsKey(accountCash.Key))
                    {
                        _accountWiseDayCash[accountCash.Key] = new Tuple<double, double>(accountCash.Value.Item1, accountCash.Value.Item2);
                    }
                    totalDR += accountCash.Value.Item1;
                    totalCR += accountCash.Value.Item2;
                }

                /// Assign the total cash into consolidation value
                if (_accountWiseDayCash.ContainsKey(int.MinValue))
                {
                    _accountWiseDayCash[int.MinValue] = new Tuple<double, double>(totalDR, totalCR);
                }
                else
                {
                    _accountWiseDayCash.Add(int.MinValue, new Tuple<double, double>(totalDR, totalCR));
                }
                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_accountWiseDayCash.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.CashInflow = _accountWiseDayCash[emptySummary.Key].Item1;
                        emptySummary.Value.CashOutflow = _accountWiseDayCash[emptySummary.Key].Item2;
                        emptySummary.Value.CashProjected += _accountWiseDayCash[emptySummary.Key].Item1 - _accountWiseDayCash[emptySummary.Key].Item2;
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

        private void UpdateDayAccruals(Dictionary<int, Dictionary<int, double>> accountWiseDayAccrualsList)
        {
            try
            {
                double total = 0.0;

                Dictionary<int, double> temp = new Dictionary<int, double>();
                foreach (KeyValuePair<int, Dictionary<int, double>> accountAccruals in accountWiseDayAccrualsList)
                {
                    if (_accountWiseDayAccruals.ContainsKey(accountAccruals.Key))
                    {
                        _accountWiseDayAccruals[accountAccruals.Key] = accountAccruals.Value;
                    }
                    total += GetSODAccrualsInBase(accountAccruals.Value, accountAccruals.Key, false);
                }

                // by this _accountWiseInitialAccruals will have base value in int.minval fund and local for others. Need to check
                temp.Add(0, total);

                /// Assign the total accruals into consolidation value
                if (_accountWiseDayAccruals.ContainsKey(int.MinValue))
                {
                    _accountWiseDayAccruals[int.MinValue] = temp;
                }
                else
                {
                    _accountWiseDayAccruals.Add(int.MinValue, temp);
                }
                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                {
                    if (_accountWiseDayAccruals.ContainsKey(emptySummary.Key))
                    {
                        emptySummary.Value.DayAccruals = GetSODAccrualsInBase(_accountWiseDayAccruals[emptySummary.Key], emptySummary.Key, false);
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

        #region Amqp data producer functions
        /// <summary>
        /// Sends auec to amqp server
        /// </summary>
        public List<AuecDetails> GetAUECDetails()
        {
            List<AuecDetails> auecDetailsList = new List<AuecDetails>();

            try
            {
                //List<int> inUseAuecId = CachedDataManager.GetInstance.GetAllAuecs().Keys;
                //Using inUseAuecId what if we are using AuecId not in use.
                foreach (int id in CachedDataManager.GetInstance.GetAllAuecs().Keys)
                {
                    auecDetailsList.Add(GetAUECDetails(id));
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return auecDetailsList;
        }

        public AuecDetails GetAUECDetails(int id)
        {
            AuecDetails details = new AuecDetails();
            try
            {
                details.AuecId = id;

                details.Today = TimeZoneHelper.GetInstance().GetCurrentDateForAUECID(id);
                details.YesterDay = details.Today.AddDays(-1);

                details.AssetId = CachedDataManager.GetInstance.GetAssetIdByAUECId(id);
                details.Asset = CachedDataManager.GetInstance.GetAssetText(details.AssetId);

                details.ExchangeId = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(id);
                details.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(details.ExchangeId);

                details.CurrencyId = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(id);
                details.Currency = CachedDataManager.GetInstance.GetCurrencyText(details.CurrencyId);

                details.UnderlyingId = CachedDataManager.GetInstance.GetUnderlyingID(id);
                details.Underlying = CachedDataManager.GetInstance.GetUnderLyingText(details.UnderlyingId);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return details;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Double> GetBetaForSymbol()
        {
            try
            {
                Dictionary<String, Double> clonedBeta = new Dictionary<string, double>();
                lock (_betaLockerObject)
                {
                    foreach (String symbol in _betaForSymbols.Keys)
                    {
                        clonedBeta.Add(symbol, _betaForSymbols[symbol].Price);
                    }
                }

                return clonedBeta;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        public List<DayEndAccountCash> GetDayEndAccountCash()
        {
            List<DayEndAccountCash> dayEndAccountCashList = new List<DayEndAccountCash>();

            try
            {
                //foreach (KeyValuePair<int, double> keyValue in _accountWiseInitialCash)
                //{
                //    DayEndAccountCash dayEndCash = new DayEndAccountCash();
                //    dayEndCash.AccountId = keyValue.Key;
                //    dayEndCash.Cash = keyValue.Value;
                //    if (dayEndCash.AccountId != int.MinValue)
                //        dayEndAccountCashList.Add(dayEndCash);
                //}

                foreach (Account account in _accounts)
                {
                    DayEndAccountCash dayEndCash = new DayEndAccountCash();
                    dayEndCash.AccountId = account.AccountID;

                    if (_accountWiseInitialCash.ContainsKey(dayEndCash.AccountId))
                        dayEndCash.Cash = _accountWiseInitialCash[dayEndCash.AccountId];

                    if (_accountWiseDayCash.ContainsKey(dayEndCash.AccountId))
                        dayEndCash.DayCash = _accountWiseDayCash[dayEndCash.AccountId].Item1 + (_accountWiseDayCash[dayEndCash.AccountId].Item2 * (-1));

                    if (dayEndCash.AccountId != int.MinValue)
                        dayEndAccountCashList.Add(dayEndCash);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dayEndAccountCashList;
        }

        /// <summary>
        /// Sends both yesterdayNav and currenct nav saved in database to esper
        /// </summary>
        public List<DbNav> GetDbNav()
        {
            List<DbNav> dbNavList = new List<DbNav>();
            try
            {
                //for (int i = 0; i < _accounts.Count; i++)
                foreach (Account account in _accounts)
                {
                    //Account account = (Account)_accounts[i];
                    DbNav dbNav = new DbNav();
                    dbNav.AccountId = account.AccountID;
                    if (_accountWiseNAV.ContainsKey(dbNav.AccountId))
                        dbNav.StartOfDayNav = _accountWiseNAV[dbNav.AccountId].NAVValue;
                    if (_accountWiseShadowNAV.ContainsKey(dbNav.AccountId))
                        dbNav.CurrentNav = _accountWiseShadowNAV[dbNav.AccountId].NAVValue;

                    dbNavList.Add(dbNav);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return dbNavList;
        }

        public List<AccountDetails> GetAccountCollection()
        {
            List<AccountDetails> accountDetailsCollection = new List<AccountDetails>();
            try
            {
                #region Sending Unallocated as account
                AccountDetails fd = new AccountDetails();
                fd.CompanyId = _companyID;
                fd.AccountId = -1;
                fd.AccountLongName = fd.AccountShortName = "Unallocated";
                fd.MasterFundId = -1;
                fd.MasterFundName = "Unallocated";
                fd.BaseCurrencyId = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                accountDetailsCollection.Add(fd);
                #endregion

                ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();

                for (int i = 0; i < _accounts.Count; i++)
                {
                    Account account = (Account)_accounts[i];

                    AccountDetails details = new AccountDetails();
                    details.CompanyId = _companyID;
                    details.AccountId = account.AccountID;
                    details.AccountLongName = account.FullName;
                    details.AccountShortName = account.Name;

                    //If accountId does not belong to any masterfund then pass unallocated
                    if (_companyAccountMasterFundRelation.ContainsKey(details.AccountId))
                    {
                        details.MasterFundId = _companyAccountMasterFundRelation[details.AccountId];
                        details.MasterFundName = CachedDataManager.GetInstance.GetMasterFund(details.MasterFundId);
                    }
                    else
                    {
                        details.MasterFundId = -1;
                        details.MasterFundName = "Unallocated";
                    }
                    if (accountWiseBaseCurrencyID.ContainsKey(details.AccountId))
                        details.BaseCurrencyId = accountWiseBaseCurrencyID[details.AccountId];

                    if (details.AccountId != int.MinValue)
                        accountDetailsCollection.Add(details);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return accountDetailsCollection;
        }

        public Dictionary<String, Object> GetPreferences()
        {
            Dictionary<String, Object> preferences = new Dictionary<string, object>();
            try
            {
                preferences.Add("CompanyID", _companyID);
                preferences.Add("IsNavSaved", _isNAVSaved);//IsM2MIncludedInCash
                preferences.Add("IsM2MIncludedInCash", bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsM2MIncludedInCash")));
                preferences.Add("CompanyBaseCurrency", CachedDataManager.GetInstance.GetCurrencyText(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()));
                preferences.Add("IsPreTradeEnabled", ComplianceCacheManager.GetPreComplianceModuleEnabled());
                preferences.Add("IsPostTradeEnabled", ComplianceCacheManager.GetPostComplianceModuleEnabled());
                preferences.Add("IsCreditLimitBoxPositionAllowed", bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CreditLimitBoxPositionAllowed")));
                preferences.Add("EquitySwapsMarketValueAsEquity", bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("EquitySwapsMarketValueAsEquity")));
                preferences.Add("CalculateFxGainLossOnForexForwards", bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnForexForwards")));
                preferences.Add("CalculateFxGainLossOnSwaps", bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnSwaps")));
                preferences.Add("IsNetExposureZeroForForexForwards", bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsNetExposureZeroForForexForwards")));
                preferences.Add("PostWithInMarketInStage", ComplianceCacheManager.GetPostWithInMarketInStage());
                preferences.Add("IsBasketComplianceEnabled", Convert.ToBoolean(ComplianceCacheManager.GetIsBasketComplianceEnabledCompany()));
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return preferences;
        }

        public List<StrategyDetails> GetStrategyCollection()
        {
            List<StrategyDetails> strategyDetailsCollection = new List<StrategyDetails>();
            try
            {
                #region Sending Unallocated as strategy
                StrategyDetails sd = new StrategyDetails();
                sd.CompanyId = _companyID;
                sd.StrategyId = -1;
                sd.StrategyName = sd.StrategyFullName = "Unallocated";
                sd.MasterStrategyId = -1;
                sd.MasterStrategyName = "Unallocated";
                strategyDetailsCollection.Add(sd);
                #endregion

                foreach (Strategy stra in WindsorContainerManager.GetStrategies())
                {
                    StrategyDetails sd1 = new StrategyDetails();
                    sd1.StrategyId = stra.StrategyID;
                    sd1.StrategyName = stra.Name;
                    sd1.StrategyFullName = stra.FullName;
                    sd1.CompanyId = _companyID;

                    //If strategyId does not belong to any masterstrategy then pass unallocated
                    if (_companyStrategyMasterStrategyRelation.ContainsKey(stra.StrategyID))
                    {
                        sd1.MasterStrategyId = _companyStrategyMasterStrategyRelation[stra.StrategyID];
                        sd1.MasterStrategyName = CachedDataManager.GetInstance.GetMasterStrategy(sd1.MasterStrategyId);
                    }
                    else
                    {
                        sd1.MasterStrategyId = -1;
                        sd1.MasterStrategyName = "Unallocated";
                    }
                    if (sd1.StrategyId != int.MinValue)
                        strategyDetailsCollection.Add(sd1);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return strategyDetailsCollection;
        }

        public List<YesterdayFxRate> GetYesterdayFxRates(List<int> auecIdList)
        {
            List<YesterdayFxRate> yesterdayFxRateList = new List<YesterdayFxRate>();
            try
            {
                foreach (int auecId in auecIdList)
                {
                    int fromCurrency = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(auecId);
                    ConversionRate rate = CommonCacheHelper.GetYesterdayFXRateFromCurrency(fromCurrency);
                    if (rate != null)
                    {
                        YesterdayFxRate yesterday = new YesterdayFxRate();
                        yesterday.ConversionRate = rate.RateValue;
                        yesterday.ConversionMethodOperator = rate.ConversionMethod.ToString();
                        yesterday.RateTime = rate.Date;
                        ////CHMW-3132
                        yesterday.CurrencySymbol = ForexConverter.GetInstance(_companyID).GetPranaForexSymbolFromCurrencyToBaseCurrency(fromCurrency);

                        yesterdayFxRateList.Add(yesterday);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return yesterdayFxRateList;
        }

        public List<YesterdayFxRate> GetYesterdayFxRates()
        {
            List<YesterdayFxRate> yesterdayFxRateList = new List<YesterdayFxRate>();
            try
            {
                Dictionary<int, ConversionRate> obj = DatabaseManager.GetInstance().GetYesterdayFXRates(TimeZoneHelper.GetInstance().GetAUECOffsetBusinessAdjustedYesterdayDateTimeString());

                foreach (int key in obj.Keys)
                {
                    YesterdayFxRate rate = new YesterdayFxRate();
                    rate.ConversionRate = obj[key].RateValue;
                    rate.ConversionMethodOperator = obj[key].ConversionMethod.ToString();
                    rate.RateTime = obj[key].Date;
                    rate.CurrencySymbol = ForexConverter.GetInstance(_companyID).GetPranaForexSymbolFromCurrencyToBaseCurrency(key);

                    yesterdayFxRateList.Add(rate);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return yesterdayFxRateList;
        }

        #endregion

        #region IDataProvider Members
        private PMUIPrefs _pmuiPrefs = null;

        public PMUIPrefs PMUIPrefs
        {
            get { return _pmuiPrefs; }
        }

        #endregion

        #region IPublishing Members
        public string getReceiverUniqueName()
        {
            return "MarkCacheManager";
        }

        public void Publish(MessageData e, string topicName)
        {
            try
            {
                System.Object[] dataList = null;

                switch (topicName)
                {
                    case Topics.Topic_MarkPrice:
                        dataList = (System.Object[])e.EventData;
                        List<string> listMark = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                        DataTable dtMark = DataTableToListConverter.GetDataTableFromList(listMark);

                        if (dtMark == null)
                        {
                            return;
                        }
                        lock (_lockerObj)
                        {
                            _UpdatedIndicesDatewiseMarkPrice = new Dictionary<DateTime, Dictionary<string, double>>();
                            foreach (DataRow dr in dtMark.Rows)
                            {
                                string symbol = dr["Symbol"].ToString();
                                DateTime date = Convert.ToDateTime(dr["Date"]);
                                double markPrice = Convert.ToDouble(dr["MarkPrice"]);

                                #region Indices Code
                                if (symbol.Contains("$") && _IndicesDatesWithDurationCode.ContainsValue(date.Date))
                                {
                                    FillUpdatedIndicesCacheThroughPublish(date, symbol, markPrice);
                                }
                                #endregion
                            }
                        }
                        if (_UpdatedIndicesDatewiseMarkPrice != null && _UpdatedIndicesDatewiseMarkPrice.Count > 0 && UpdateIndicesMarkCacheEvent != null)
                            UpdateIndicesMarkCacheEvent(this, new EventArgs());
                        break;

                    case Topics.Topic_Beta:
                        Dictionary<string, DateTime> dateWiseSymbol = new Dictionary<string, DateTime>();
                        dataList = (System.Object[])e.EventData;
                        List<string> list = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                        DataTable dt = DataTableToListConverter.GetDataTableFromList(list);

                        if (dt == null)
                        {
                            return;
                        }

                        Dictionary<String, double> betaTempDict = new Dictionary<string, double>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            SymbolPriceWithDate betaObj = new SymbolPriceWithDate();
                            string symbol = dr["Symbol"].ToString();
                            int auecID = Convert.ToInt32(dr["AUECID"]);
                            double beta = 1;
                            if (dr["Beta"] != null && dr["Beta"].ToString() != string.Empty)
                            {
                                beta = Convert.ToDouble(dr["Beta"]);
                            }
                            DateTime currentDate = TimeZoneHelper.GetInstance().GetCurrentDateForAUECID(auecID);
                            if (beta == 0)
                            {
                                if (!dateWiseSymbol.ContainsKey(symbol))
                                {
                                    dateWiseSymbol.Add(symbol, currentDate);
                                }
                                continue;
                            }
                            else
                            {
                                DateTime date = Convert.ToDateTime(dr["Date"]);
                                bool shouldFillCompliance = false;

                                lock (_betaLockerObject)
                                {
                                    if (_betaForSymbols.ContainsKey(symbol))
                                    {
                                        if (_betaForSymbols[symbol].DateActual.Date <= date.Date && date.Date <= currentDate.Date)
                                        {
                                            _betaForSymbols[symbol].Price = beta;
                                            _betaForSymbols[symbol].DateActual = date;
                                            _betaForSymbols[symbol].DateRequired = currentDate;
                                            if (date.Date == currentDate.Date)
                                            {
                                                _betaForSymbols[symbol].Indicator = 0;
                                            }
                                            else
                                            {
                                                _betaForSymbols[symbol].Indicator = 1;
                                            }
                                            shouldFillCompliance = true;
                                        }
                                    }
                                    else
                                    {
                                        if (beta != 0 && date.Date <= currentDate.Date)
                                        {
                                            betaObj.DateActual = date;
                                            betaObj.Price = beta;
                                            betaObj.DateRequired = currentDate;
                                            if (date.Date == currentDate.Date)
                                            {
                                                betaObj.Indicator = 0;
                                            }
                                            else
                                            {
                                                betaObj.Indicator = 1;
                                            }
                                            _betaForSymbols.Add(symbol, betaObj);
                                            shouldFillCompliance = true;
                                        }
                                    }
                                }

                                #region Compliance Beta data fill

                                if (shouldFillCompliance)
                                {
                                    if (betaTempDict.ContainsKey(symbol))
                                        betaTempDict[symbol] = beta;
                                    else
                                        betaTempDict.Add(symbol, beta);
                                }

                                #endregion
                            }
                        }
                        if (dateWiseSymbol.Count > 0)
                        {
                            _betaForSymbols = GetUpdatedBetaCache(dateWiseSymbol);
                        }

                        #region Compliance event raise
                        if (BetaUpdate != null && betaTempDict.Count > 0)
                            BetaUpdate(this, new EventArgs<Dictionary<string, double>>(betaTempDict));
                        #endregion

                        FillBeta();

                        break;

                    case Topics.Topic_OutStandings:
                        dataList = (System.Object[])e.EventData;
                        List<string> listOutStandings = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                        DataTable dtOutStandings = DataTableToListConverter.GetDataTableFromList(listOutStandings);

                        if (dtOutStandings == null)
                        {
                            return;
                        }

                        foreach (DataRow dr in dtOutStandings.Rows)
                        {
                            string symbol = dr["Symbol"].ToString();
                            int auecID = Convert.ToInt32(dr["AUECID"]);
                            DateTime date = Convert.ToDateTime(dr["Date"]);
                            int outStandings = 0;
                            if (dr["OutStandings"] != null && dr["OutStandings"].ToString() != "")
                            {
                                bool isValidOutstanding = int.TryParse(dr["OutStandings"].ToString(), out outStandings);
                                if (!isValidOutstanding)
                                {
                                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Shares outstanding value is not valid for Symbol:- " + symbol + " and date:- " + date.ToShortDateString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                    continue;
                                }
                            }

                            DateTime currentDate = TimeZoneHelper.GetInstance().GetCurrentDateForAUECID(auecID);
                            if (date.Date >= currentDate.Date)
                            {
                                lock (_sharesOutstandingLockerObject)
                                {
                                    if (_sharesOutstandingForSymbols.ContainsKey(symbol))
                                    {
                                        _sharesOutstandingForSymbols[symbol] = outStandings;
                                    }
                                }
                            }
                        }

                        FillSharesOutstanding();
                        break;

                    // For now the cash impact is being calculated at the epnl itself. There were several issues like commision not required,
                    // calculation of cash impact on unallocated orders, so commented this subscription...
                    // http://209.234.251.99:8080/browse/PRANA-1386
                    // Commented on 15 July 2011
                    case Topics.Topic_Split:

                        _todayDateTimeString = TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedCurrentDateTimeString();
                        AdjustMarkPriceByTodaysSplitFactor(true);

                        break;

                    case Topics.Topic_CashActivity:
                        //case Topics.Topic_ManualJournalActivity:
                        System.Object[] publishDataList = null;
                        publishDataList = (System.Object[])e.EventData;
                        bool dividendUpdate = false;
                        foreach (Object obj in publishDataList)
                        {
                            CashActivity cashActivity = obj as CashActivity;
                            if ((cashActivity.TransactionSource.Equals(CashTransactionType.CashTransaction) || cashActivity.TransactionSource.Equals(CashTransactionType.CorpAction)) && (cashActivity.ActivityType.Equals(Convert.ToString(Activities.DividendExpense)) || cashActivity.ActivityType.Equals(Convert.ToString(Activities.DividendIncome)) || cashActivity.ActivityType.Equals(Convert.ToString(Activities.WithholdingTax))))
                            {
                                if (_uncalculatedData != null && cashActivity.TaxLotId != null && _uncalculatedData.Contains(cashActivity.TaxLotId) && TimeZoneHelper.GetInstance().GetCurrentDateByAUEC(_uncalculatedData[cashActivity.TaxLotId].AUECID).ToShortDateString().Equals(cashActivity.Date.ToShortDateString()))
                                {
                                    if (_dicTaxlotIdWiseDividend != null)
                                    {
                                        dividendUpdate = true;
                                        if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted)
                                        {
                                            if (_dicTaxlotIdWiseDividend.ContainsKey(cashActivity.TaxLotId))
                                            {
                                                int index = _dicTaxlotIdWiseDividend[cashActivity.TaxLotId].FindIndex(x => x.FKID.Equals(cashActivity.FKID));
                                                _dicTaxlotIdWiseDividend[cashActivity.TaxLotId].RemoveAt(index);
                                            }
                                        }
                                        else if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.Updated)
                                        {
                                            if (_dicTaxlotIdWiseDividend.ContainsKey(cashActivity.TaxLotId))
                                            {
                                                int index = _dicTaxlotIdWiseDividend[cashActivity.TaxLotId].FindIndex(x => x.FKID.Equals(cashActivity.FKID));
                                                _dicTaxlotIdWiseDividend[cashActivity.TaxLotId].RemoveAt(index);

                                                _dicTaxlotIdWiseDividend[cashActivity.TaxLotId].Add(cashActivity);
                                            }
                                        }
                                        else
                                        {
                                            if (!_dicTaxlotIdWiseDividend.ContainsKey(cashActivity.TaxLotId))
                                            {
                                                List<CashActivity> dividendTaxLot = new List<CashActivity>();
                                                dividendTaxLot.Add(cashActivity);
                                                _dicTaxlotIdWiseDividend.Add(cashActivity.TaxLotId, dividendTaxLot);
                                            }
                                            else
                                                _dicTaxlotIdWiseDividend[cashActivity.TaxLotId].Add(cashActivity);
                                        }
                                    }
                                }
                            }
                        }
                        if (_uncalculatedData != null)
                        {
                            if (_dicTaxlotIdWiseDividend != null && dividendUpdate == true)
                            {
                                foreach (KeyValuePair<string, List<CashActivity>> kvp in _dicTaxlotIdWiseDividend)
                                {
                                    if (_uncalculatedData.Contains(kvp.Key))
                                    {
                                        _uncalculatedData[kvp.Key].EarnedDividendLocal = 0;
                                        _uncalculatedData[kvp.Key].EarnedDividendBase = 0;

                                        foreach (CashActivity activity in kvp.Value)
                                        {
                                            _uncalculatedData[activity.TaxLotId].EarnedDividendLocal += Convert.ToDouble(activity.Amount);
                                            if ((_uncalculatedData[activity.TaxLotId].FXConversionMethodOperator == Operator.M || _uncalculatedData[activity.TaxLotId].FXConversionMethodOperator == Operator.Multiple) && _uncalculatedData[activity.TaxLotId].FxRate != 0)
                                                _uncalculatedData[activity.TaxLotId].EarnedDividendBase += Convert.ToDouble(activity.Amount) * _uncalculatedData[activity.TaxLotId].FxRate;
                                            else if (_uncalculatedData[activity.TaxLotId].FXConversionMethodOperator == Operator.D && _uncalculatedData[activity.TaxLotId].FxRate != 0)
                                                _uncalculatedData[activity.TaxLotId].EarnedDividendBase += Convert.ToDouble(activity.Amount) / _uncalculatedData[activity.TaxLotId].FxRate;
                                            else if (_uncalculatedData[activity.TaxLotId].FxRate != 0)
                                                _uncalculatedData[activity.TaxLotId].EarnedDividendBase += Convert.ToDouble(activity.Amount) * _uncalculatedData[activity.TaxLotId].FxRate;
                                            else
                                                _uncalculatedData[activity.TaxLotId].EarnedDividendBase += Convert.ToDouble(activity.Amount);
                                            _uncalculatedData[activity.TaxLotId].ExDividendDate = activity.Date;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case Topics.Topic_DayEndCash:
                        System.Object[] DataList = null;
                        DataList = (System.Object[])e.EventData;
                        if (DataList.Length > 0)
                        {
                            List<CompanyAccountCashCurrencyValue> lsDayEndCash = new List<CompanyAccountCashCurrencyValue>();
                            CompanyAccountCashCurrencyValue _DayEndCash;
                            foreach (Object obj in DataList)
                            {
                                _DayEndCash = obj as CompanyAccountCashCurrencyValue;
                                if (_DayEndCash != null && !lsDayEndCash.Contains(_DayEndCash))
                                    lsDayEndCash.Add(_DayEndCash);
                            }

                            Dictionary<int, double> accountWiseCashList = new Dictionary<int, double>();
                            List<int> listAccountIds = new List<int>();
                            foreach (CompanyAccountCashCurrencyValue accountCash in lsDayEndCash)
                            {
                                if (!listAccountIds.Contains(accountCash.AccountID))
                                {
                                    listAccountIds.Add(accountCash.AccountID);

                                    double accountWiseCash = 0;
                                    List<CompanyAccountCashCurrencyValue> listCashValues = lsDayEndCash.FindAll(delegate (CompanyAccountCashCurrencyValue obj)
                                    {
                                        if (obj.AccountID.Equals(accountCash.AccountID))
                                            return true;
                                        else
                                            return false;
                                    });

                                    foreach (CompanyAccountCashCurrencyValue Cashvalue in listCashValues)
                                    {
                                        accountWiseCash += Convert.ToDouble(Cashvalue.CashValueBase);
                                    }
                                    if (accountWiseCashList.ContainsKey(accountCash.AccountID))
                                    {
                                        accountWiseCashList[accountCash.AccountID] = accountWiseCash;
                                    }
                                    else
                                    {
                                        accountWiseCashList.Add(accountCash.AccountID, accountWiseCash);
                                    }
                                }
                            }
                            UpdateDayEndCash(accountWiseCashList);

                            #region Compliance Section
                            try
                            {
                                if (DayEndCashReceived != null)
                                    DayEndCashReceived(this, new EventArgs<List<DayEndAccountCash>>(GetDayEndAccountCash()));
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
                            #endregion
                        }
                        break;

                    case Topics.Topic_StartDayOfAccrual:
                        System.Object[] AccrualDataList = null;
                        AccrualDataList = (System.Object[])e.EventData;

                        bool isAccrualsNeeded = bool.Parse(ConfigurationManager.AppSettings["IsAccrualsNeededOnPM"]);
                        if (isAccrualsNeeded)
                        {
                            CreateAccrualSummaryCollection();
                            Dictionary<int, Dictionary<int, double>> accountWiseAccrualList = new Dictionary<int, Dictionary<int, double>>();
                            if (AccrualDataList.Length > 0)
                            {
                                List<CompanyAccountCashCurrencyValue> lsAccurals = new List<CompanyAccountCashCurrencyValue>();
                                CompanyAccountCashCurrencyValue _accruals;
                                foreach (Object obj in AccrualDataList)
                                {
                                    _accruals = obj as CompanyAccountCashCurrencyValue;
                                    if (_accruals != null && !lsAccurals.Contains(_accruals))
                                        lsAccurals.Add(_accruals);
                                }

                                foreach (CompanyAccountCashCurrencyValue accountCash in lsAccurals)
                                {
                                    if (!accountWiseAccrualList.ContainsKey(accountCash.AccountID))
                                    {
                                        Dictionary<int, double> dictCurrencyCash = new Dictionary<int, double>();
                                        dictCurrencyCash.Add(accountCash.LocalCurrencyID, Convert.ToDouble(accountCash.CashValueBase));
                                        accountWiseAccrualList.Add(accountCash.AccountID, dictCurrencyCash);
                                    }
                                    else
                                    {
                                        accountWiseAccrualList[accountCash.AccountID].Add(accountCash.LocalCurrencyID, Convert.ToDouble(accountCash.CashValueBase));
                                    }
                                }
                                // need to check publish cases later. If here we are just receiving base cash then need to publish fund-currency wise.
                                UpdateStartOfDayAccruals(accountWiseAccrualList);

                                #region Compliance Section
                                try
                                {
                                    if (AccountWiseStartOfDayAccrualEvent != null)
                                        AccountWiseStartOfDayAccrualEvent(this, new EventArgs<List<Accurals>>(GetAccountWiseAccrual()));
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
                                #endregion
                            }
                            else
                            {
                                UpdateStartOfDayAccruals(accountWiseAccrualList);
                            }
                        }
                        break;

                    case Topics.Topic_ForexRate:
                        dataList = (System.Object[])e.EventData;
                        List<string> listForexRate = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                        DataTable dtForexRate = DataTableToListConverter.GetDataTableFromList(listForexRate);

                        if (dtForexRate == null)
                        {
                            return;
                        }
                        bool isdtContainsBaseCurrency = false;
                        int companyBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                        foreach (DataRow dr in dtForexRate.Rows)
                        {
                            if (Convert.ToInt32(dr["FromCurrencyID"]) == companyBaseCurrencyID || Convert.ToInt32(dr["ToCurrencyID"]) == companyBaseCurrencyID)
                            {
                                isdtContainsBaseCurrency = true;
                                break;
                            }
                        }
                        if (isdtContainsBaseCurrency)
                        {
                            CommonCacheHelper.LoadYesterdayFXRates(TimeZoneHelper.GetInstance().GetAUECOffsetAdjustedYesterdayDateTimeString());
                        }

                        #region Compliance Section
                        //Sending dbnav and cash event to esper through amqp-plugin manager
                        try
                        {
                            if (YesterdayFxRateUpdated != null)
                            {
                                YesterdayFxRateUpdated(this, new EventArgs<List<YesterdayFxRate>>(GetYesterdayFxRates()));
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
                        #endregion

                        break;

                    case Topics.Topic_SecurityMaster:
                        dataList = (System.Object[])e.EventData;
                        SecMasterbaseList secMasterObjlist = new SecMasterbaseList();
                        foreach (Object secmasterObj in dataList)
                        {
                            secMasterObjlist.Add((SecMasterBaseObj)secmasterObj);
                        }
                        UpdateSecurityMasterInfo(secMasterObjlist);
                        break;

                    case Topics.Topic_DailyCreditLimit:
                        dataList = (System.Object[])e.EventData;
                        //The following code refreshes the _dailyCreditLimitCollection cache and only updates the changes obtained after publishing
                        lock (_dailyCreditLimitLockerObject)
                        {
                            foreach (int key in _dailyCreditLimitCollection.Keys.ToList())
                            {
                                _dailyCreditLimitCollection[key] = new DailyCreditLimit();
                            }
                        }
                        List<string> listDailyCreditLimit = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                        UpdateDailyCreditLimitCollection(DataTableToListConverter.GetDataTableFromList(listDailyCreditLimit));

                        #region Compliance Section
                        //Sending dbnav and cash event to esper through amqp-plugin manager
                        try
                        {
                            if (DailyCreditLimitEvent != null)
                            {
                                DailyCreditLimitEvent(this, new EventArgs<Dictionary<int, DailyCreditLimit>>(_dailyCreditLimitCollection));
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
                        #endregion

                        break;

                    case Topics.Topic_PerformanceNumber:
                        dataList = (System.Object[])e.EventData;
                        List<string> listfor = new List<string>(Array.ConvertAll<object, string>(dataList, new Converter<object, string>(Convert.ToString)));
                        DataTable dtPerformanceNumbers = DataTableToListConverter.GetDataTableFromList(listfor);
                        if (dtPerformanceNumbers == null)
                        {
                            return;
                        }
                        foreach (DataRow dr in dtPerformanceNumbers.Rows)
                        {
                            DateTime date = Convert.ToDateTime(dr["Date"]);

                            DateTime latestDate = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true);
                            DateTime lastDate = latestDate.AddDays(-1);
                            if (date.Date == lastDate.Date)
                            {
                                UpdateDailyPerformancenumbersCollection(dtPerformanceNumbers);
                                break;
                            }
                        }
                        break;

                    case Topics.Topic_CashData:
                        dataList = (System.Object[])e.EventData;
                        if (dataList.Count() > 0)
                        {
                            Transaction transaction = (Transaction)dataList[0];
                            if (!transaction.GetActivitySource().Equals((byte)ActivitySource.Trading))
                            {
                                _accountWiseDayCash.Clear();
                                foreach (object obj in _accounts)
                                {
                                    Account account = obj as Account;
                                    if (account != null)
                                    {
                                        if (!_accountWiseDayCash.ContainsKey(account.AccountID))
                                        {
                                            _accountWiseDayCash.Add(account.AccountID, new Tuple<double, double>(0, 0));
                                        }
                                    }
                                }

                                CreateAccrualSummaryCollection();
                                foreach (KeyValuePair<int, ExposureAndPnlOrderSummary> emptySummary in _summaries)
                                {
                                    if (_accountWiseInitialCash.ContainsKey(emptySummary.Key))
                                    {
                                        emptySummary.Value.CashInflow = 0;
                                        emptySummary.Value.CashOutflow = 0;
                                        emptySummary.Value.CashProjected = 0;

                                        emptySummary.Value.CashProjected = _accountWiseInitialCash[emptySummary.Key];
                                    }
                                }

                                DateTime latestDate1 = TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true);
                                GenericDayEndData genericDayEndData = _cashMgmtService.InnerChannel.GetDayEndDataInBaseCurrency(latestDate1, _isAccrualsNeeded, _isIncludeTradingDayAccruals);

                                UpdateDayCashImpact(genericDayEndData.DayAccountWiseCash);
                                UpdateDayAccruals(genericDayEndData.DayAccountWiseAccruals);

                                #region Compliance Section
                                try
                                {
                                    if (DayEndCashReceived != null)
                                        DayEndCashReceived(this, new EventArgs<List<DayEndAccountCash>>(GetDayEndAccountCash()));

                                    if (AccountWiseDayAccrualEvent != null)
                                        AccountWiseDayAccrualEvent(this, new EventArgs<List<Accurals>>(GetAccountWiseAccrual()));
                                }
                                catch (Exception ex)
                                {
                                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                                    if (rethrow)
                                        throw;
                                }
                                #endregion
                            }
                        }
                        break;

                    default:
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

        #endregion

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
                if (_proxyPricing != null)
                {
                    _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_MarkPrice);
                    _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_Beta);
                    _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_OutStandings);
                    _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_ForexRate);
                    _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_PerformanceNumber);
                    _proxyPricing.Dispose();
                }
                if (_proxyServer != null)
                {
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_Split);
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_DayEndCash);
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_CashActivity);
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_DailyCreditLimit);
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_StartDayOfAccrual);
                    _proxyServer.InnerChannel.UnSubscribe(Topics.Topic_CashData);
                    _proxyServer.Dispose();
                }
                if (_secMasterSyncService != null)
                    _secMasterSyncService.Dispose();
                if (_orderFillManager != null)
                    _orderFillManager.Dispose();
                if (_pricingServiceProxy != null)
                    _pricingServiceProxy.Dispose();
                if (_bbgFileWatcher != null)
                    _bbgFileWatcher.Dispose();
                if (_indicesMarkPrices != null)
                    _indicesMarkPrices.Dispose();
                if (_cashMgmtService != null)
                    _cashMgmtService.Dispose();
                _indexDS.Dispose();
                _dtIndicesReturn.Dispose();
            }
        }
        #endregion
    }
}