using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Prana.ExpnlService
{
    public class ExPnlCache : IDisposable
    {
        const string SECTION_OTCExchanges = "otcExchanges";

        private bool _isEpnlRunning = false;
        public bool IsEpnlRunning
        {
            get { return _isEpnlRunning; }
            set { _isEpnlRunning = value; }
        }

        public Stopwatch RefreshStopwatch
        {
            get { return _refreshSW; }
            set { _refreshSW = value; }
        }

        public int RefreshInterval
        {
            get { return _updatedTimerDueTime; }
        }

        bool _isNAVSaved = false;
        bool _isClearAccountWiseOrdersCacheRequired = false;

        private DataTable _dtIndicesReturn = null;
        public DataTable DTIndicesReturn
        {
            get
            {
                if (_dtIndicesReturn != null)
                {
                    return _dtIndicesReturn.Copy();
                }
                return null;
            }
            set
            {
                _dtIndicesReturn = value;
            }
        }

        Dictionary<string, double> _indicesMarkPriceCache = null;

        #region Instances
        IGroupedDataProvider _dataProvider = null;
        IDataCalculator _datacalculator = null;
        ForexConverter _forexConverter = null;
        ExposurePnLScheduler _epnlScheduler = null;

        private Dictionary<int, string> _otcExchanges;
        public Dictionary<int, string> OTCExchanges
        {
            get { return _otcExchanges; }
        }
        #endregion

        #region Timer Related Values
        Timer _calculationTimer = null;
        Stopwatch _refreshSW = new Stopwatch();
        #endregion

        private void OnCalculatedExposureAndPnlOrdersChanged()
        {
            try
            {
                ExposureAndPnlOrderCollection temp_markedcollection = DeepCopyHelper.Clone<ExposureAndPnlOrderCollection>(_markedcollection);
                Dictionary<int, ExposureAndPnlOrderCollection> temp_calculatedOrdersAndPositions = DeepCopyHelper.Clone<Dictionary<int, ExposureAndPnlOrderCollection>>(_calculatedOrdersAndPositions);
                Dictionary<int, DistinctAccountSetWiseSummaryCollection> temp_distinctAccountSetWiseSummaryCollection = DeepCopyHelper.Clone<Dictionary<int, DistinctAccountSetWiseSummaryCollection>>(_distinctAccountSetWiseSummaryCollection);
                if (ServiceManager.GetInstance().ICompressor != null && ServiceManager.GetInstance().ICompressor.Count > 0)
                {
                    //Only one compressor's input data setup. Need to initialize account-symbol if it is not
                    //Also if it one object collection then lock needs to be verified
                    ServiceManager.GetInstance().ICompressor[0].SetInputData(temp_calculatedOrdersAndPositions, _accountWiseOrderAndSummary.AccountWiseSummary, temp_markedcollection, temp_distinctAccountSetWiseSummaryCollection);

                    if (!CachedDataManager.GetInstance.IsFilePricingForTouch() && ServiceManager.GetInstance().ICompressor[0].GroupingComponentName != CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION
                        && ServiceManager.GetInstance().ICompressor.Count > 1)
                    {
                        ServiceManager.GetInstance().ICompressor
                            .FirstOrDefault(x => x.GroupingComponentName == CompressionViewFactory.ACCOUNTSYMBOL_COMPRESSION)
                            .SetInputData(temp_calculatedOrdersAndPositions, _accountWiseOrderAndSummary.AccountWiseSummary, temp_markedcollection, temp_distinctAccountSetWiseSummaryCollection);
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
        /// Called when [calculated exposure and PNL orders changed touch].
        /// </summary>
        private void OnCalculatedExposureAndPnlOrdersChangedTouch()
        {
            try
            {
                ExposureAndPnlOrderCollection temp_markedcollection = DeepCopyHelper.Clone<ExposureAndPnlOrderCollection>(_markedcollection);
                Dictionary<int, ExposureAndPnlOrderCollection> temp_calculatedOrdersAndPositions = DeepCopyHelper.Clone<Dictionary<int, ExposureAndPnlOrderCollection>>(_calculatedOrdersAndPositions);
                Dictionary<int, DistinctAccountSetWiseSummaryCollection> temp_distinctAccountSetWiseSummaryCollection = DeepCopyHelper.Clone<Dictionary<int, DistinctAccountSetWiseSummaryCollection>>(_distinctAccountSetWiseSummaryCollection);
                if (ServiceManager.GetInstance().ICompressor != null && ServiceManager.GetInstance().ICompressor.Count > 1)
                {
                    //Only one compressor's input data setup. Need to initialize account-symbol if it is not
                    //Also if it one object collection then lock needs to be verified
                    ServiceManager.GetInstance().ICompressor[1].SetInputData(temp_calculatedOrdersAndPositions, _accountWiseOrderAndSummary.AccountWiseSummary, temp_markedcollection, temp_distinctAccountSetWiseSummaryCollection);
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

        #region Account Properties to Publish Calculated Exposure And Pnl Data !
        private Dictionary<int, DistinctAccountSetWiseSummaryCollection> _distinctAccountSetWiseSummaryCollection = null;
        private Dictionary<int, ExposureAndPnlOrderCollection> _calculatedOrdersAndPositions = null;
        private ExposureAndPnlOrderCollection _markedcollection = null;
        private AccountInfo _accountWiseOrderAndSummary = new AccountInfo();
        private Dictionary<int, List<int>> _distinctAccountPermissionSets = new Dictionary<int, List<int>>();
        private Dictionary<int, ExposureAndPnlOrderSummary> _accountWiseSummary = new Dictionary<int, ExposureAndPnlOrderSummary>();
        private Dictionary<int, ExposureAndPnlOrderCollection> _accountWiseOrders = new Dictionary<int, ExposureAndPnlOrderCollection>();
        #endregion

        #region Instance

        private ExPnlCache()
        {
            _otcExchanges = new Dictionary<int, string>();
            _indicesMarkPriceCache = new Dictionary<string, double>();
            _dataProvider = DataManager.GetInstance();
            _dataProvider.UpdateIndicesMarkCacheEvent += new EventHandler(DataManager_UpdateIndicesMarkCacheEvent);
        }

        void DataManager_UpdateIndicesMarkCacheEvent(object sender, EventArgs e)
        {
            _dataProvider.UpdateIndicesMarkCache(_indicesMarkPriceCache);
        }

        public event EventHandler LogAUECDatesToManager;
        void _dataProvider_LogAUECDates(object sender, EventArgs e)
        {
            if (LogAUECDatesToManager != null)
            {
                LogAUECDatesToManager(sender, EventArgs.Empty);
            }
        }

        private static ExPnlCache _instance = null;
        public static ExPnlCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExPnlCache();
                }
                return _instance;
            }
        }
        #endregion

        void _dataProvider_UpdateRemoveOrder(object sender, EventArgs<EPnlOrder, ApplicationConstants.TaxLotState, bool> e)
        {
            try
            {
                if (e.Value2 == ApplicationConstants.TaxLotState.Deleted)
                {
                    lock (_locker)
                    {
                        if (e.Value == null)
                            _accountWiseOrders.Clear();
                        else
                        {
                            foreach (KeyValuePair<int, ExposureAndPnlOrderCollection> OrderCollection in _accountWiseOrders)
                            {
                                OrderCollection.Value.Remove(e.Value.ID);
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

        #region Initiate method
        internal void Start()
        {
            try
            {
                _isNAVSaved = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsNAVSaved"));
                _dataProvider.LogOnScreen += new EventHandler(_dataProvider_LogAUECDates);
                _dataProvider.SetQuartzScheduler();

                _forexConverter = ForexConverter.GetInstance(_dataProvider.CompanyID);
                _dataProvider.UpdateRemoveOrder += new EventHandler<EventArgs<EPnlOrder, ApplicationConstants.TaxLotState, bool>>(_dataProvider_UpdateRemoveOrder);

                _dataProvider.GetUpdatedDataFromDB(null);

                _epnlScheduler = ExposurePnLScheduler.GetInstance();
                _epnlScheduler.ScheduleElapsed += new EventHandler(_epnlScheduler_ScheduleElapsed);

                NameValueCollection _AppOTCExchanges = new NameValueCollection();
                _AppOTCExchanges = (NameValueCollection)ConfigurationManager.GetSection(SECTION_OTCExchanges);
                if (_AppOTCExchanges != null)
                {
                    for (int i = 0; i < _AppOTCExchanges.Count; i++)
                    {
                        if (!_otcExchanges.ContainsKey(int.Parse(_AppOTCExchanges.GetValues(i)[0])))
                        {
                            _otcExchanges.Add(int.Parse(_AppOTCExchanges.GetValues(i)[0]), _AppOTCExchanges.GetKey(i));
                        }
                    }
                }

                _datacalculator = new DataCalculator(TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates, TimeZoneHelper.GetInstance().ClearanceTime);
                FillIndicesData();
                _isEpnlRunning = true;
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

        internal void Stop()
        {
            try
            {
                lock (_locker)
                {
                    //Raturi: Null reference check
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6668
                    if (_epnlScheduler != null)
                    {
                        _epnlScheduler.ScheduleElapsed -= new EventHandler(_epnlScheduler_ScheduleElapsed);
                    }
                    //Raturi: Null reference check
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6668
                    if (_dataProvider != null)
                    {
                        _dataProvider.UpdateRemoveOrder -= new EventHandler<EventArgs<EPnlOrder, ApplicationConstants.TaxLotState, bool>>(_dataProvider_UpdateRemoveOrder);
                        _dataProvider.LogOnScreen -= new EventHandler(_dataProvider_LogAUECDates);
                        _dataProvider.Close();
                    }

                    #region Account Collections Clearing
                    _datacalculator = null;
                    if (_markedcollection != null)
                    {
                        _markedcollection.Clear();
                    }
                    if (_accountWiseOrderAndSummary != null)
                    {
                        if (_accountWiseOrderAndSummary.AccountWiseOrderCollection != null)
                        {
                            _accountWiseOrderAndSummary.AccountWiseOrderCollection.Clear();
                        }
                        if (_accountWiseOrderAndSummary.AccountWiseSummary != null)
                        {
                            _accountWiseOrderAndSummary.AccountWiseSummary.Clear();
                        }
                    }

                    if (_calculatedOrdersAndPositions != null)
                    {
                        _calculatedOrdersAndPositions.Clear();
                    }

                    _isEpnlRunning = false;
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

        void _epnlScheduler_ScheduleElapsed(object sender, EventArgs e)
        {
            try
            {
                if (!_isEpnlRunning)
                {
                    return;
                }
                RefreshCacheDataAsync(null);
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

        int _updatedTimerDueTime = 30000;
        internal void StartCalculations()
        {
            try
            {
                // Default calculation interval is 30 seconds which is later changed by reading it from the config file.
                //_calculationTimer = new System.Windows.Forms.Timer();
                _calculationTimer = new Timer(DoWork, null, 0, Timeout.Infinite);
                if (_refreshSW != null && !_refreshSW.IsRunning)
                {
                    _refreshSW.Start();
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

        internal void StopCalculations()
        {
            try
            {
                if (_calculationTimer == null)
                {
                    return;
                }
                _calculationTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _calculationTimer = null;
                if (_refreshSW != null)
                {
                    _refreshSW.Stop();
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

        public void UpdateCalculationInterval(int calculationRefreshInterval)
        {
            try
            {
                if (_calculationTimer != null)
                {
                    _updatedTimerDueTime = calculationRefreshInterval * 1000;
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
        #endregion

        #region Indices Code
        private void FillIndicesData()
        {
            try
            {
                _dtIndicesReturn = _dataProvider.DtIndicesReturn;
                DataSet ds = _dataProvider.IndicesMarkPrice;
                if (ds != null)
                {
                    FillIndicesMarkCache(ds);
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

        private void FillIndicesMarkCache(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Duration dur = (Duration)(Convert.ToInt32(dr["durationCode"]) - 1);
                    string key = dr["Symbol"].ToString() + "_" + dur.ToString();
                    double markPrice = Convert.ToDouble(dr["MarkPrice"]);

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
        #endregion

        #region Calculations logic here!
        private object _locker = new object();

        public void DoWork(object state)
        {
            try
            {
                lock (_locker)
                {
                    DateTime startTime = DateTime.UtcNow;
                    StringBuilder calculationTimeLog = new StringBuilder("New calculation cycle started at " + startTime.ToString());

                    int countTaxLots = CalculateAndCompress(false);
                    if (CachedDataManager.GetInstance.IsFilePricingForTouch())
                    {
                        CalculateAndCompress(true);
                    }

                    DateTime endTime = DateTime.UtcNow;
                    calculationTimeLog.Append(" || Full calculation ended at " + endTime.ToString() + " || Time taken: " + (endTime - startTime).TotalSeconds + " second(s) || Number of taxlots: " + countTaxLots);
                    Logger.LoggerWrite(calculationTimeLog, LoggingConstants.CATEGORY_GENERAL);

                    if (_refreshSW != null)
                    {
                        _refreshSW.Reset();
                        _refreshSW.Start();
                    }

                    if (_calculationTimer != null)
                        _calculationTimer.Change(_updatedTimerDueTime, Timeout.Infinite);
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                if (_calculationTimer != null)
                    _calculationTimer.Change(_updatedTimerDueTime, Timeout.Infinite);

                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Calculates the and compress.
        /// </summary>
        /// <param name="isTouchData">if set to <c>true</c> [is touch data].</param>
        /// <returns></returns>
        private int CalculateAndCompress(bool isTouchData)
        {
            try
            {
                /// The calculators calculate the data on a calculation interval. So here, clean up the LiveFeed cache so that it could fetch 
                /// new prices and calculate on the basis of those. If we keep fetching the new prices within the same calculation interval, 
                /// then there could be multiple prices for the same symbol and thus showing multiple in the prices as well.
                LiveFeedManager.Instance.ClearLiveFeedCache(isTouchData);

                // clone inital filled summary to start calculation
                _accountWiseOrderAndSummary.AccountWiseSummary = DeepCopyHelper.Clone(_dataProvider.Summaries);
                _accountWiseSummary = _accountWiseOrderAndSummary.AccountWiseSummary;
                _markedcollection = _dataProvider.GetUncalculatedDataClone();

                if (_markedcollection.Count == 0)
                {
                    _accountWiseOrders = null;
                    _accountWiseOrders = new Dictionary<int, ExposureAndPnlOrderCollection>();
                }

                if (_isClearAccountWiseOrdersCacheRequired)
                {
                    _accountWiseOrders = null;
                    _accountWiseOrders = new Dictionary<int, ExposureAndPnlOrderCollection>();
                    _isClearAccountWiseOrdersCacheRequired = false;
                }

                _distinctAccountPermissionSets = DeepCopyHelper.Clone(SessionManager.DistinctAccountPermissionSets);
                if (_datacalculator != null)
                    _datacalculator.CalculateData(_markedcollection, ref _accountWiseOrders, _distinctAccountPermissionSets);

                // sending blotter prices to compliance
                if (!isTouchData)
                    LiveFeedManager.Instance.SendBlotterPrices();

                _accountWiseOrderAndSummary.AccountWiseOrderCollection = _accountWiseOrders;
                _calculatedOrdersAndPositions = _accountWiseOrderAndSummary.AccountWiseOrderCollection;
                int countTaxLots = _accountWiseOrderAndSummary.AccountWiseOrderCollection.Sum(x => x.Value.Count);

                //calculate Summaries from calculated data
                _datacalculator.CalculateSummariesFromData(ref _distinctAccountSetWiseSummaryCollection, ref _accountWiseSummary, ref _calculatedOrdersAndPositions, _distinctAccountPermissionSets, _isNAVSaved);

                _datacalculator.ClearSummaryCalculatorCache();

                if (isTouchData)
                    OnCalculatedExposureAndPnlOrdersChangedTouch();
                else
                {
                    //calculate Index Returns
                    _datacalculator.CalculateIndexReturns(_dataProvider.IndexSymbols, ref _dtIndicesReturn, _indicesMarkPriceCache);
                    OnCalculatedExposureAndPnlOrdersChanged();
                }
                // Data has been passed to local references of Grouping Component, here local reference to be removed
                _accountWiseOrderAndSummary.AccountWiseSummary = null;
                _calculatedOrdersAndPositions = null;
                _distinctAccountSetWiseSummaryCollection = null;
                return countTaxLots;
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


        #endregion

        #region Refresh Data
        bool _isRefreshingData = false;

        /// <summary>
        /// Here we got the message to refresh the expnl data .
        /// </summary>
        public void RefreshExPNLData(List<int> auecsList)
        {
            try
            {
                RefreshCacheDataAsync(auecsList);
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

        private void RefreshCacheDataAsync(List<int> auecsList)
        {
            try
            {
                if (_isRefreshingData)
                {
                    return;
                }
                Logger.LoggerWrite("Calculation refresh started at " + DateTime.UtcNow.ToString(), LoggingConstants.CATEGORY_GENERAL);

                _calculationTimer.Change(Timeout.Infinite, Timeout.Infinite);
                if (_refreshSW != null && _refreshSW.IsRunning)
                {
                    _refreshSW.Reset();
                    _refreshSW.Start();
                }

                _isRefreshingData = true;
                _dataProvider.SetQuartzScheduler();

                lock (_locker)
                {
                    _accountWiseOrders.Clear();
                }
                _dataProvider.GetUpdatedDataFromDB(auecsList);

                ///Refresh the forex data again from db.
                _forexConverter.GetForexData();

                _datacalculator.UpdateCurrentDatesAndClearanceTime(TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates, TimeZoneHelper.GetInstance().ClearanceTime);

                FillIndicesData();
                /// Has called the DoWork function here to remove a bug that on refreshing, it used to pick up the old calculated cached data and 
                /// send it to client. Once client received the data with the flag "IsDataReceivedFirstTime", client use to update all the
                /// static fields as well and next time onwards it only updated the dynamic fields. Now as this data was not the newly 
                /// received data from db as the calculation on the newly received data from the db is taken later for calculation. Due to this 
                /// problem, we had to add lots of dynamic columns in the dynamic columns xml.
                /// 
                /// Now as the data refresh is in progress, we call do work in last here so that it prepares the calculated data with newly received
                /// db data. So as soon as this refresh function ends, the senddatatoclientforfirsttime is called and client fetches
                /// the correct newly fetched static information.
                DoWork(null);

                //The following line shifted to finally as case of exception, _isRefreshingData remains true and not allows the previous code to run again
                _calculationTimer.Change(_updatedTimerDueTime, Timeout.Infinite);
                if (_refreshSW != null && _refreshSW.IsRunning)
                {
                    _refreshSW.Reset();
                    _refreshSW.Start();
                }

                Logger.LoggerWrite("Calculation refresh finished at " + DateTime.UtcNow.ToString(), LoggingConstants.CATEGORY_GENERAL);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                _isRefreshingData = false;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                _isRefreshingData = false;
            }
        }
        #endregion

        /// <summary>
        /// Gets the market value.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        internal Dictionary<string, double> GetMarketValue(List<TaxLot> taxlotList)
        {
            Dictionary<string, double> groupWiseMarketValue = new Dictionary<string, double>();
            try
            {
                ExposureAndPnlOrderCollection markedcollection = new ExposureAndPnlOrderCollection();
                foreach (TaxLot taxlot in taxlotList)
                {
                    EPnlOrder order = CommonCacheHelper.GetEPnlOrderFromTaxlot(taxlot);
                    markedcollection.Add(order);
                }

                Dictionary<int, ExposureAndPnlOrderCollection> accountWiseOrders = new Dictionary<int, ExposureAndPnlOrderCollection>();
                Dictionary<int, List<int>> distinctAccountPermissionSets = DeepCopyHelper.Clone(SessionManager.DistinctAccountPermissionSets);
                lock (_locker)
                {
                    LiveFeedManager.Instance.ClearLiveFeedCache(false);
                    _datacalculator.CalculateData(markedcollection, ref accountWiseOrders, distinctAccountPermissionSets);
                    _datacalculator.ClearSummaryCalculatorCache();
                }

                //if calculations exist for unallocated trades
                if (accountWiseOrders.Count > 0 && accountWiseOrders.ContainsKey(-1))
                    groupWiseMarketValue = accountWiseOrders[-1].ToDictionary(x => x.ID, y => y.MarketValueInBaseCurrency);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return groupWiseMarketValue;
        }

        internal void UpdateNewlySentOrderStatus()
        {
            try
            {
                _isClearAccountWiseOrdersCacheRequired = true;
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

        #region Clearance Code
        public void UpdateClearance(Dictionary<int, DateTime> updatedClearanceTime)
        {
            try
            {
                TimeZoneHelper.GetInstance().ClearanceTime = updatedClearanceTime;
                if (_isEpnlRunning)
                {
                    RefreshCacheDataAsync(null);
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

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_forexConverter != null)
                    _forexConverter.Dispose();
                if (_dtIndicesReturn != null)
                    _dtIndicesReturn.Dispose();
                if (_epnlScheduler != null)
                    _epnlScheduler.Dispose();
                if (_calculationTimer != null)
                    _calculationTimer.Dispose();
            }
        }
        #endregion
    }
}