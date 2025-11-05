using Newtonsoft.Json;
using Prana.APIAdapter.Handlers;
using Prana.APIAdapter.HelperClasses;
using Prana.APIAdapter.Interfaces;
using Prana.APIAdapter.Models;
using Prana.APIAdapter.Sessions;
using Prana.APIAdapter.Utilities;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.APIAdapter
{
    public class APIAdapterManager : ILiveFeedAdapter, IDisposable
    {
        /// <summary>
        /// _greeks Calculation Interval
        /// </summary>
        private int _greeksCalculationInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("GreeksCalculationInterval"));

        /// <summary>
        /// _ApiService instance
        /// </summary>
        IPriceAPIService _ApiService;

        /// <summary>
        /// _dataTransFormer instance
        /// </summary>
        IDataTransformer _dataTransFormer;

        /// <summary>
        /// isConnected
        /// </summary>
        bool isStatusInformed = false;

        /// <summary>
        /// _lockerObject
        /// </summary>
        object _lockerObject = new object();


        int _retryCounter = 1;

        /// <summary>
        /// API Adapter Manager
        /// </summary>
        public APIAdapterManager()
        {
            try
            {
                _ApiService = _ApiService ?? new PricingApiHelper();
                _dataTransFormer = _dataTransFormer ?? new DataTransformer();
                APIConfigurationManager.InitializeConfiguration();
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ApiService"></param>
        //public APIAdapterManager(IPriceAPIService ApiService, IDataTransformer dataTransFormer)
        //{
        //    try
        //    {
        //        _ApiService = _ApiService ?? ApiService;
        //        _dataTransFormer = _dataTransFormer ?? dataTransFormer;
        //        ConfigurationManager.InitializeConfiguration();
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}

        /// <summary>
        /// copies the data after a specified time 
        /// which is based on feed back details
        /// </summary>
        private async void StartFetchingData(object state)
        {
            try
            {
                while (true)
                {
                    await GetDataFromAPI();

                    Thread.Sleep(_greeksCalculationInterval);

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

        private async System.Threading.Tasks.Task GetDataFromAPI()
        {
            try
            {
                if (_ApiService != null)
                {

                    var isValidSession = CheckIsValidSession();

                    if (!isValidSession)
                    {
                        //Authenticate from API
                        await _ApiService.Authenticate();

                        isValidSession = CheckIsValidSession();
                        if (isValidSession)
                            SetConnectionStatus(true);

                    }

                    //Fetch Prices from API
                    var priceString = await _ApiService.GetPrice();
                    if (!string.IsNullOrEmpty(priceString))
                    {
                        List<SymbolData> priceData = await _dataTransFormer.JsonStringToSymbolData(priceString);

                        SessionManager.Instance.SetIncomingPricesData(priceData);

                        if (SessionManager.Instance.IsDebugEnabled)
                        {
                            var priceDataCloned = DeepCopyHelper.Clone<List<SymbolData>>(priceData);
                            bool isDataCorrect = DataSanityChecker.CheckDataSanityAndDumpFile(priceDataCloned, priceString);

                        }
                    }
                    else
                    {
                        //Prices are not comming
                        SessionManager.Instance.Session.isAuthenticated = false;
                        SetConnectionStatus(false);
                    }

                }
                else
                {
                    _ApiService = _ApiService ?? new PricingApiHelper();
                    _dataTransFormer = _dataTransFormer ?? new DataTransformer();
                    APIConfigurationManager.InitializeConfiguration();
                    if (SessionManager.Instance.Session != null)
                        SessionManager.Instance.Session.isAuthenticated = false;
                }
            }
            catch (Exception ex)
            {

                if (_retryCounter == 0)
                {
                    _retryCounter = 1;

                    SetConnectionStatus(false);
                }
                _retryCounter--;
                if (_ApiService != null)
                    _ApiService = null;

                if (_dataTransFormer != null)
                    _dataTransFormer = null;


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
        /// check Is Valid Session
        /// </summary>
        /// <param name="authSession"></param>
        /// <returns></returns>
        private bool CheckIsValidSession()
        {
            try
            {
                var authSession = SessionManager.Instance.Session;
                bool isAuthenticated = false;
                if (authSession != null && authSession.isAuthenticated && (authSession.expires - DateTime.Now).Milliseconds >= 0)
                {
                    isAuthenticated = true;
                }
                else
                {
                    isAuthenticated = false;
                }

                // SetConnectionStatus(isAuthenticated);
                return isAuthenticated;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Set Connection Status
        /// </summary>
        /// <param name="isAuthenticated"></param>
        private void SetConnectionStatus(bool isAuthenticated)
        {
            if (isAuthenticated)
            {
                if (Connected != null && !isStatusInformed)
                {
                    Connected(this, (new EventArgs<bool>(true)));
                    //  MessageLogger.LoggerWrite("Connected API Service Time : " + DateTime.Now, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    isStatusInformed = true;
                }

            }
            else
            {
                if (Disconnected != null)
                {
                    Disconnected(this, (new EventArgs<bool>(false)));
                    // MessageLogger.LoggerWrite("DisConnected with  API Service Time : " + DateTime.Now, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    isStatusInformed = false;
                }

            }
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose all unmanaged resources
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {

            if (isDisposing)
            {
                if (_ApiService != null)
                {
                    _ApiService = null;
                }
                if (_dataTransFormer != null)
                {
                    _dataTransFormer = null;
                }
                SessionManager.Instance.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Connect()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(StartFetchingData));

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
        public void Disconnect()
        {
            try
            {
                this.Dispose();
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
        /// Get Continuous Data
        /// </summary>
        /// <param name="symbol"></param>
        public void GetContinuousData(string symbol)
        {

        }

        #region To handle the event errors
        public void events(string symbol)
        {
            if (ContinuousDataResponse != null)
            {
                ContinuousDataResponse(this, null);
            }
            if (SnapShotDataResponse != null)
                SnapShotDataResponse(this, null);

            Dictionary<string, List<OptionStaticData>> OptionChain = new Dictionary<string, List<OptionStaticData>>();
            if (OptionChainResponse != null)
                OptionChainResponse(this, new EventArgs<string, List<OptionStaticData>>(null, null));
        }

        #endregion

        /// <summary>
        /// Get Available Live Feed
        /// </summary>
        /// <returns></returns>
        public List<SymbolData> GetAvailableLiveFeed()
        {
            try
            {
                //Get Prices from cache
                var prices = SessionManager.Instance.GetOutgoingPricesData();

                if (prices != null && prices.Count > 0)
                {
                    return prices;
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
            return new List<SymbolData>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="symbologyCode"></param>
        /// <param name="completeInfo"></param>
        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool completeInfo)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        public void DeleteSymbol(string symbol)
        {

        }

        /// <summary>
        /// Get Option Chain
        /// </summary>
        /// <param name="underlyingSymbol"></param>
        /// <param name="month"></param>
        /// <param name="lowerstrike"></param>
        /// <param name="upperstrike"></param>
        /// <param name="turnaroundid"></param>
        /// <param name="categoryCode"></param>
        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {

        }

        /// <summary>
        /// Check If International Symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            return new Dictionary<string, bool>();
        }

        /// <summary>
        /// Connected Event Handler
        /// </summary>
        public event EventHandler<EventArgs<bool>> Connected;

        /// <summary>
        /// Disconnected Event Handler
        /// </summary>
        public event EventHandler<EventArgs<bool>> Disconnected;

        /// <summary>
        /// ContinuousDataResponse Event Handler
        /// </summary>
        public event EventHandler<Data> ContinuousDataResponse;

        /// <summary>
        /// SnapShotDataResponse Event Handler
        /// </summary>
        public event EventHandler<Data> SnapShotDataResponse;

        /// <summary>
        /// OptionChainResponse Event Handler
        /// </summary>
        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;

        /// <summary>
        /// Get Live Data Directly From Feed
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetLiveDataDirectlyFromFeed()
        {
            object data = null;
            try
            {
                if (_ApiService != null)
                {
                    //await System.Threading.Tasks.Task.Run(async() =>
                    //     {

                    var isValidSession = CheckIsValidSession();

                    if (!isValidSession)
                    {
                        //Authenticate from API
                        await _ApiService.Authenticate();
                    }

                    //Fetch Prices from API
                    var priceString = await _ApiService.GetPrice();

                    dynamic obj = JsonConvert.DeserializeObject<Prices>(priceString);

                    data = JsonConvert.SerializeObject(obj.prices);

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
            return data;
        }

        /// <inheritdoc/>
        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {
            try
            {
                SessionManager.Instance.IsDebugEnabled = isDebugEnable;
                SessionManager.Instance.PercentageTolerance = pctTolerance;

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

        /// <inheritdoc/>
        public void UpdateSecurityDetails(SecMasterbaseList secMasterList)
        {
            try
            {

                SessionManager.Instance.UpdateSymbolsCacheForNewSecurity(secMasterList);

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

        public Dictionary<string, string> GetUserInformation()
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            throw new NotImplementedException();
        }
    }
}
