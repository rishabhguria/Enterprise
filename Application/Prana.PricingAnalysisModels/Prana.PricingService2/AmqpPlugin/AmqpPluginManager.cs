using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Prana.PricingService2.AmqpPlugin
{
    /// <summary>
    /// Plugin manager for AMQP connection
    /// </summary>
    internal class AmqpPluginManager
    {
        static AmqpPluginManager _amqpPluginManager;

        static ConnectionStatusManager _esperConnection;

        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate Connected;
        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate Disconnected;


        /// <summary>
        /// Singleton pattern implemented
        /// </summary>
        /// <returns>Singleton instance of AmqpPluginManager</returns>
        internal static AmqpPluginManager GetInstance()
        {
            if (_amqpPluginManager == null)
                _amqpPluginManager = new AmqpPluginManager();
            return _amqpPluginManager;
        }


        #region Private properties

        IPricingService _pricingService;
        WhatIfSnapShotCallBack _whatIfSnapShotCallBack;

        //String _hostName;
        String _esperRequestExchangeName;
        String _otherDataExchangeName;
        String _liveFeedExchangeName;
        int _companyBaseCurrencyId;
        #endregion


        /// <summary>
        /// Initilise the manager instance
        /// </summary>
        internal void Initialise(IPricingService pricingService)
        {
            try
            {
                SetupWhatifSnapshot();
                SetUpAMQPListener();
                this._pricingService = pricingService;
                this._pricingService.LiveFeedConnectionStatusChanged += new EventHandler<EventArgs<bool>>(_pricingService_LiveFeedConnectionStatusChanged);
                //new Prana.BusinessObjects.BoolHandler(_pricingService_LiveFeedConnectionStatusChanged);

                _esperConnection = new ConnectionStatusManager(Module.EsperCalculator);
                _esperConnection.StatusChanged += Connection_StatusChanged;
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
        /// Logs and updates the client with connection status
        /// </summary>
        /// <param name="e"></param>
        void Connection_StatusChanged(Object sender, ConnectionEventArguments e)
        {
            try
            {
                switch (e.Module)
                {
                    case Module.EsperCalculator:
                        if (e.ConnectionStatus)
                        {
                            if (Connected != null)
                            {
                                Connected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Esper Calculation Engine Connected.\n");
                                Logger.LoggerWrite("Esper Calculation Engine Connected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                            }
                        }
                        else
                        {
                            if (Disconnected != null)
                            {
                                Disconnected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Esper Calculation Engine Disconnected.\n");
                                Logger.LoggerWrite("Esper Calculation Engine Disconnected.\n", LoggingConstants.CATEGORY_FLAT_FILE_ERROR_MSG, 1, 1, TraceEventType.Information);
                            }
                        }
                        PricingServiceManager.SetEsperEngineConnectionStatus(true);
                        LiveFeedManager.LiveFeedManager.SetEsperEngineConnectionStatus(e.ConnectionStatus);
                        break;
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
        /// Initialises whatif snapshot instance
        /// </summary>
        private void SetupWhatifSnapshot()
        {
            try
            {
                _whatIfSnapShotCallBack = new WhatIfSnapShotCallBack();
                _whatIfSnapShotCallBack.FurtherSnapshotRequest += new FurtherSnapShotRequestHandler(_whatIfSnapShotCallBack_FurtherSnapshotRequest);
                _whatIfSnapShotCallBack.FurtherFxSnapshotRequest += new FurtherSnapShotFxRequestHandler(_whatIfSnapShotCallBack_FurtherFxSnapshotRequest);
                _whatIfSnapShotCallBack.FurtherFxForwardSpotRequest += new FurtherFxForwardSpotRequestHandler(_whatIfSnapShotCallBack_FurtherFxForwardSpotRequest);
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


        void _pricingService_LiveFeedConnectionStatusChanged(object sender, EventArgs<bool> e)
        {
            try
            {
                if (e.Value)
                {
                    _whatIfSnapShotCallBack.LiveFeedConnected();
                }
                else
                {
                    _whatIfSnapShotCallBack.LiveFeedDisConnected();
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
        /// setting up amqp receivers
        /// </summary>
        private void SetUpAMQPListener()
        {
            try
            {
                _companyBaseCurrencyId = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                //_hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings,ConfigurationHelper.CONFIGKEY_AmqpServer);
                _esperRequestExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_EsperRequestExchange);
                _otherDataExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
                _liveFeedExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_LiveFeedQueue);
                String symbolDataQueue = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_SymbolDataQueue);
                AmqpHelper.InitializeSender("SymbolData", symbolDataQueue, MediaType.Queue);
                AmqpHelper.Started += new ListenerStarted(AmqpListenerStarted);
                List<String> keyList = new List<string>();
                keyList.Add("Pricing");
                AmqpHelper.InitializeListenerForExchange(_esperRequestExchangeName, MediaType.Exchange_Direct, keyList);

                List<String> keyList1 = new List<string>();
                //keyList.Clear();
                keyList1.Add("EsperHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList1);

                AmqpHelper.InitializeSender("LiveFeed", _liveFeedExchangeName, MediaType.Exchange_Direct);

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

        void _whatIfSnapShotCallBack_FurtherFxForwardSpotRequest(string symbol, int leadCurrencyId, int vsCurrencyId, AssetCategory assetCategory)
        {
            try
            {
                fxInfo fxIn = new fxInfo();
                fxIn.PranaSymbol = symbol;
                fxIn.ToCurrencyID = vsCurrencyId;
                fxIn.FromCurrencyID = leadCurrencyId;
                fxIn.CategoryCode = assetCategory;
                List<fxInfo> fxList = new List<fxInfo>();
                fxList.Add(fxIn);

                _pricingService.RequestSnapshot(fxList, ApplicationConstants.SymbologyCodes.TickerSymbol, true, _whatIfSnapShotCallBack, true);
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


        void _whatIfSnapShotCallBack_FurtherFxSnapshotRequest(string symbol, int fromCurencyCode)
        {
            try
            {
                fxInfo fxIn = new fxInfo();
                fxIn.PranaSymbol = symbol;
                fxIn.ToCurrencyID = _companyBaseCurrencyId;
                fxIn.FromCurrencyID = fromCurencyCode;
                fxIn.CategoryCode = AssetCategory.Forex;
                List<fxInfo> fxList = new List<fxInfo>();
                fxList.Add(fxIn);

                _pricingService.RequestSnapshot(fxList, ApplicationConstants.SymbologyCodes.TickerSymbol, true, _whatIfSnapShotCallBack, true);
                //sw.Stop();
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

        void _whatIfSnapShotCallBack_FurtherSnapshotRequest(List<String> symbols)
        {
            try
            {
                _pricingService.RequestSnapshot(symbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, _whatIfSnapShotCallBack, true);
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
        /// Attach different handlers to diffrent exchange
        /// </summary>
        /// <param name="amqpReceiver"></param>
        void AmqpListenerStarted(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                SendMarkPricesToAmqp();
                _whatIfSnapShotCallBack.SendCurrentStatus();
                if (e.AmqpReceiver.MediaName == _esperRequestExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(AmqpDataReceived_Esper);
                }
                else if (e.AmqpReceiver.MediaName == _otherDataExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += AmqpReceiver_AmqpDataReceived_Hearrtbeat;
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
        /// Process the heart beat
        /// </summary>
        /// <param name="dsReceived"></param>
        /// <param name="mediaName"></param>
        /// <param name="mediaType"></param>
        /// <param name="routingKey"></param>
        void AmqpReceiver_AmqpDataReceived_Hearrtbeat(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.RoutingKey.Contains("HEARTBEAT"))
                    HeartbeatReceived(e.RoutingKey, e.DsReceived);
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
        /// Inform the object about the ping
        /// </summary>
        /// <param name="routingkey"></param>
        private void HeartbeatReceived(String routingkey, DataSet ds)
        {
            try
            {
                int interval = Convert.ToInt32(ds.Tables[0].Rows[0]["Interval"]);
                switch (routingkey)
                {
                    case "_EsperHEARTBEAT":
                        _esperConnection.GotAPing(interval);
                        break;
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

        void AmqpDataReceived_Esper(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                amqpHelper_amqpDataRecieved(e.DsReceived);
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
        /// When any event arrives from epser
        /// </summary>
        /// <param name="dataSet"></param>
        void amqpHelper_amqpDataRecieved(DataSet dataSet)
        {
            Dictionary<String, String> responseObj = new Dictionary<string, string>();
            try
            {
                //DataSet dataSet = JsonHelper.Deserialize(data as String);
                String typeOfRequest = dataSet.Tables[0].Rows[0]["TypeOfRequest"].ToString();

                if (typeOfRequest.Equals("InitializationRequest"))
                {
                    SendMarkPricesToAmqp();
                    SendSymbolDataTimerValue();
                    SendOMIDataInformationToEsper();
                    ////Dictionary<String, double> markPrices = new Dictionary<string, double>();
                    //if (MarkCacheManager.LatesMarkPrices != null)
                    //{
                    //    foreach (String symbol in MarkCacheManager.LatesMarkPrices.Keys)
                    //    {
                    //        AmqpHelper.SendObject(MarkCacheManager.LatesMarkPrices[symbol], "OtherData", "MarkPrice");
                    //        //markPrices.Add(symbol, MarkCacheManager.LatesMarkPrices[symbol].MarkPrice);
                    //    }
                    //}


                    _whatIfSnapShotCallBack.SendCurrentStatus();

                    responseObj.Add("ResponseType", "InitCompletePricing");
                    AmqpHelper.SendObject(responseObj, "OtherData", "InitCompleteInfo");
                }
                else if (typeOfRequest.Equals("LiveFeedRequestSnapShot"))
                {
                    //TODO: Needs to check for Multiple symbol at the same time 
                    //might be a foreach loop will work
                    DataTable dtTemp = dataSet.Tables[0];
                    String symbolToBeFetched = dtTemp.Rows[0]["symbol"].ToString();
                    String underlyingSymbol = String.Empty;
                    String fxSymbol = String.Empty;
                    int currencyCode = 0;
                    int leadCurrencyId = -1;
                    int vsCurrencyId = -1;
                    int assetId = 0;
                    if (dtTemp.Columns.Contains("underlyingSymbol"))
                    {
                        underlyingSymbol = dtTemp.Rows[0]["underlyingSymbol"].ToString();
                        if (underlyingSymbol == symbolToBeFetched)
                            underlyingSymbol = String.Empty;
                    }
                    if (dtTemp.Columns.Contains("fxSymbol"))
                    {
                        fxSymbol = dtTemp.Rows[0]["fxSymbol"].ToString();


                        if (!String.IsNullOrEmpty(fxSymbol))
                            currencyCode = Convert.ToInt32(dtTemp.Rows[0]["currencyId"]);
                        if (currencyCode == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                        {
                            fxSymbol = String.Empty;
                            currencyCode = 0;
                        }
                    }

                    if (dtTemp.Columns.Contains("leadCurrencyId"))
                        leadCurrencyId = Convert.ToInt32(dtTemp.Rows[0]["leadCurrencyId"]);

                    if (dtTemp.Columns.Contains("vsCurrencyId"))
                        vsCurrencyId = Convert.ToInt32(dtTemp.Rows[0]["vsCurrencyId"]);

                    if (dtTemp.Columns.Contains("assetId"))
                        assetId = Convert.ToInt32(dtTemp.Rows[0]["assetId"]);


                    _whatIfSnapShotCallBack.AddPendingSnapshot(symbolToBeFetched, underlyingSymbol, fxSymbol, currencyCode, leadCurrencyId, vsCurrencyId, (AssetCategory)assetId);

                }
                else if (typeOfRequest.Equals("EsperStartedCompletely"))
                {
                    PricingServiceManager.SetEsperEngineConnectionStatus(true);
                    LiveFeedManager.LiveFeedManager.IsEsperStartedCompletely = true;
                }

            }
            catch (Exception ex)
            {

                responseObj.Add("ResponseType", "InitCompleteInfoPricingFailed");
                AmqpHelper.SendObject(responseObj, "OtherData", "InitCompleteInfo");
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        internal void SendMarkPricesToAmqp()
        {
            try
            {
                if (MarkCacheManager.LatesMarkPrices != null)
                {
                    Dictionary<string, MarkPriceInfo> latesMarkPrices = DeepCopyHelper.Clone(MarkCacheManager.LatesMarkPrices);
                    InformationReporter.GetInstance.ComplianceLogWrite("Sending mark prices to Amqp.");
                    foreach (String symbol in latesMarkPrices.Keys)
                    {
                        if (string.IsNullOrEmpty(latesMarkPrices[symbol].Symbol))
                        {
                            InformationReporter.GetInstance.ComplianceLogWrite("Corrected blank symbol for Compliance (Mark Price), Symbol is: " + symbol);
                            latesMarkPrices[symbol].Symbol = symbol;
                        }
                        AmqpHelper.SendObject(latesMarkPrices[symbol], "OtherData", "MarkPrice");
                        //markPrices.Add(symbol, MarkCacheManager.LatesMarkPrices[symbol].MarkPrice);
                    }
                    InformationReporter.GetInstance.ComplianceLogWrite("Mark prices sent to Amqp.");
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
        /// Sends value of SendSymbolDataTimerInterval set in config to Esper engine.
        /// </summary>
        internal static void SendSymbolDataTimerValue()
        {
            try
            {
               int symbolDataTimerInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_SendSymbolDataTimerInterval));
                Dictionary<string, int> responseObj = new Dictionary<string, int>
                {
                    { "SymbolDataTimerIntervalValue", symbolDataTimerInterval }
                };
                AmqpHelper.SendObject(responseObj, "OtherData", "SymbolDataTimerInterval");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SendOMIDataInformationToEsper
        /// </summary>
        internal static void SendOMIDataInformationToEsper()
        {
            try
            {
                List<string> symbolsFXForwards = new List<string>();
                List<UserOptModelInput> listOMIdata = PricingServiceManager.GetOMIDataFromCacheForCompliance();

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
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Closes all amqp connection
        /// </summary>
        internal void Close()
        {
            try
            {
                AmqpHelper.Started -= new ListenerStarted(AmqpListenerStarted);
                AmqpHelper.CloseAllConnection();
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
    }
}