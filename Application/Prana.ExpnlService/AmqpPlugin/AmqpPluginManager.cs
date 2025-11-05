using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.LiveFeed;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.ExpnlService.AmqpPlugin
{
    /// <summary>
    /// Singleton definition to handle all amqp related communication
    /// </summary>
    internal class AmqpPluginManager
    {
        static AmqpPluginManager _amqpPluginManager;

        static ConnectionStatusManager _esperConnection;
        static ConnectionStatusManager _ruleMediatorConnection;

        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate Connected;
        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate Disconnected;

        /// <summary>
        /// Pending orders cache, Adding orders in this cache while Esper is not connected
        /// </summary>
        private Dictionary<string, EPnlOrder> _pendingOrders = new Dictionary<string, EPnlOrder>();

        /// <summary>
        /// Cache lock object
        /// </summary>
        private static object _cacheLockerObject = new object();

        /// <summary>
        /// Initialize request from Esper
        /// </summary>
        private bool _initializeRequestFromEsper = false;

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
        String _esperRequestExchangeName;
        String _liveFeedExchangeName;
        String _otherDataExchangeName;
        String _orderQueueName;
        int _companyId;
        #endregion

        internal void Initialise()
        {
            try
            {
                LoadAppSettings();
                SetUpAMQPListener();
                //LiveFeedManager.Instance.LiveFeedReceived += new EventHandler<EventArgs<List<SymbolData>>>(LiveFeedReceived);
                DataManager.GetInstance().OrderReceived += new EventHandler<EventArgs<EPnlOrder>>(OrderReceived);
                DataManager.GetInstance().DayEndCashReceived += new EventHandler<EventArgs<List<DayEndAccountCash>>>(DayEndCashReceived);
                DataManager.GetInstance().AuecDetailsUpdated += new EventHandler<EventArgs<AuecDetails>>(AuecDetailsUpdated);
                DataManager.GetInstance().YesterdayFxRateUpdated += new EventHandler<EventArgs<List<YesterdayFxRate>>>(AmqpPluginManager_YesterdayFxRateUpdated);
                DataManager.GetInstance().DbNavUpdate += new EventHandler<EventArgs<List<DbNav>>>(AmqpPluginManager_DbNavUpdate);
                DataManager.GetInstance().BetaUpdate += new EventHandler<EventArgs<Dictionary<string, double>>>(AmqpPluginManager_BetaUpdate);
                ServiceManager.GetInstance().ERefreshData += new EventHandler(AmqpPluginManager_RefreshData);
                DataManager.GetInstance().StartOfMonthCapitalAccountUpdated += new EventHandler<EventArgs<Dictionary<int, double>>>(AmqpPluginManager_startOfMonthCapitalAccountUpdated);
                DataManager.GetInstance().UserDefinedMTDPnLCollectionUpdated += new EventHandler<EventArgs<Dictionary<int, double>>>(AmqpPluginManager_userDefinedMTDPnLCollectionUpdated);
                DataManager.GetInstance().AccountWiseAccrualEvent += new EventHandler<EventArgs<List<Accurals>>>(AmqpPluginManager_AccountWiseAccrualEvent);
                DataManager.GetInstance().AccountWiseStartOfDayAccrualEvent += new EventHandler<EventArgs<List<Accurals>>>(AmqpPluginManager_AccountWiseStartOfDayAccrualEvent);
                DataManager.GetInstance().AccountWiseDayAccrualEvent += new EventHandler<EventArgs<List<Accurals>>>(AmqpPluginManager_AccountWiseDayAccrualEvent);
                DataManager.GetInstance().DailyCreditLimitEvent += AmqpPluginManager_DailyCreditLimitEvent;
                _esperConnection = new ConnectionStatusManager(Module.EsperCalculator);
                _ruleMediatorConnection = new ConnectionStatusManager(Module.RuleEnginMediator);
                _esperConnection.StatusChanged += Connection_StatusChanged;
                _ruleMediatorConnection.StatusChanged += Connection_StatusChanged;
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
                                Logger.LoggerWrite("Esper Calculation Engine Connected.\n");
                            }
                        }
                        else
                        {
                            if (Disconnected != null)
                            {
                                Disconnected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Esper Calculation Engine Disconnected.\n");
                                Logger.LoggerWrite("Esper Calculation Engine Disconnected.\n");
                            }
                        }
                        break;
                    case Module.RuleEnginMediator:
                        if (e.ConnectionStatus)
                        {
                            if (Connected != null)
                            {
                                Connected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Rule Mediator", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Rule Mediator Connected.\n");
                                Logger.LoggerWrite("Rule Mediator Connected.\n");
                            }
                        }
                        else
                        {
                            if (Disconnected != null)
                            {
                                Disconnected(this, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Rule Mediator", IdentifierID = "" }));
                                InformationReporter.GetInstance.ComplianceLogWrite("Rule Mediator Disconnected.\n");
                                Logger.LoggerWrite("Rule Mediator Disconnected.\n");
                            }
                        }
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

        void AmqpPluginManager_DailyCreditLimitEvent(object sender, EventArgs<Dictionary<int, DailyCreditLimit>> e)
        {
            try
            {
                Dictionary<int, DailyCreditLimit> dailyCreditLimit = e.Value;
                if (dailyCreditLimit != null && dailyCreditLimit.Count > 0)
                {
                    foreach (int accountId in dailyCreditLimit.Keys)
                    {
                        Dictionary<String, Object> creditLimit = new Dictionary<String, Object>();
                        creditLimit.Add("AccountId", accountId);
                        creditLimit.Add("LongDebitBalance", dailyCreditLimit[accountId].LongDebitBalance);
                        creditLimit.Add("LongDebitLimit", dailyCreditLimit[accountId].LongDebitLimit);
                        creditLimit.Add("ShortCreditBalance", dailyCreditLimit[accountId].ShortCreditBalance);
                        creditLimit.Add("ShortCreditLimit", dailyCreditLimit[accountId].ShortCreditLimit);

                        AmqpHelper.SendObject(creditLimit, "OtherDataSender", "DailyCreditLimit");
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
        /// Sends account wise accruals to Esper omn AUEC change
        /// </summary>
        /// <param name="accountWiseAccrual"></param>
        void AmqpPluginManager_AccountWiseAccrualEvent(object sender, EventArgs<List<Accurals>> e)
        {
            try
            {
                if (e.Value != null && e.Value.Count > 0)
                {
                    //Added Accurals class with accountId, StartOfDay Accrual and Day Accrual
                    foreach (Accurals accrualsObject in e.Value)
                    {
                        Accurals accrual = new Accurals();
                        accrual.AccountId = accrualsObject.AccountId;
                        accrual.StartOfDayAccruals = accrualsObject.StartOfDayAccruals;
                        accrual.DayAccruals = accrualsObject.DayAccruals;
                        AmqpHelper.SendObject(accrual, "OtherDataSender", "AccrualForAccount");
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
        /// Sends account wise start of day accruals to Esper on AUEC change
        /// </summary>
        /// <param name="accountWiseAccrual"></param>
        void AmqpPluginManager_AccountWiseStartOfDayAccrualEvent(object sender, EventArgs<List<Accurals>> e)
        {
            try
            {
                if (e.Value != null && e.Value.Count > 0)
                {
                    //Added Accurals class with accountId, StartOfDay Accrual and Day Accrual
                    foreach (Accurals accrualsObject in e.Value)
                    {
                        Accurals accrual = new Accurals();
                        accrual.AccountId = accrualsObject.AccountId;
                        accrual.StartOfDayAccruals = accrualsObject.StartOfDayAccruals;
                        accrual.DayAccruals = accrualsObject.DayAccruals;
                        AmqpHelper.SendObject(accrual, "OtherDataSender", "StartOfDayAccrualForAccount");
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
        /// Sends account wise day accruals to Esper on AUEC change
        /// </summary>
        /// <param name="accountWiseAccrual"></param>
        void AmqpPluginManager_AccountWiseDayAccrualEvent(object sender, EventArgs<List<Accurals>> e)
        {
            try
            {
                if (e.Value != null && e.Value.Count > 0)
                {
                    //Added Accurals class with accountId, StartOfDay Accrual and Day Accrual
                    foreach (Accurals accrualsObject in e.Value)
                    {
                        Accurals accrual = new Accurals();
                        accrual.AccountId = accrualsObject.AccountId;
                        accrual.StartOfDayAccruals = accrualsObject.StartOfDayAccruals;
                        accrual.DayAccruals = accrualsObject.DayAccruals;
                        AmqpHelper.SendObject(accrual, "OtherDataSender", "DayAccrualForAccount");
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

        void AmqpPluginManager_BetaUpdate(object sender, EventArgs<Dictionary<string, double>> e)
        {
            try
            {
                if (e.Value != null && e.Value.Count > 0)
                {
                    AmqpHelper.SendObject(e.Value, "OtherDataSender", "BetaForSymbol");
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
        /// App settings related to amqp communication will be loaded
        /// </summary>
        private void LoadAppSettings()
        {
            try
            {
                _esperRequestExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_EsperRequestExchange);
                _liveFeedExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_LiveFeedExchangeName);
                _otherDataExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
                _orderQueueName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OrderQueueName);
                _companyId = CommonDataCache.CachedDataManager.GetInstance.GetCompanyID();
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

        private void SetUpAMQPListener()
        {
            try
            {
                AmqpHelper.InitializeSender("OtherDataSender", _otherDataExchangeName, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("LiveFeed", _liveFeedExchangeName, MediaType.Exchange_Direct);
                AmqpHelper.InitializeSender("Order", _orderQueueName, MediaType.Queue);

                AmqpHelper.Started += new ListenerStarted(AmqpListenerStarted);
                List<String> keyList = new List<string>();
                keyList.Add("ExPnL");
                AmqpHelper.InitializeListenerForExchange(_esperRequestExchangeName, MediaType.Exchange_Direct, keyList);
                List<String> keyList1 = new List<string>();
                keyList1.Add("EsperHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList1);
                List<String> keyList2 = new List<string>();
                keyList2.Add("RuleMediatorHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList2);
                List<String> keyList3 = new List<string>();
                keyList3.Add("SymbolAdviceRequest");
                AmqpHelper.InitializeListenerForExchange(_otherDataExchangeName, MediaType.Exchange_Direct, keyList3);

                NotificationManager.BLL.NotificationManager.GetInstance().InitializeManager();
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
        /// When any reciever is started
        /// </summary>
        /// <param name="amqpReceiver"></param>
        void AmqpListenerStarted(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _esperRequestExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(amqpReceiver_AmqpDataReceived_esperRequest);
                    // Object is send to Amqp when Amqp listener started
                    InformationReporter.GetInstance.ComplianceLogWrite("Sending objects to amqp server.\n");
                    SendObjectToAmqp();
                    InformationReporter.GetInstance.ComplianceLogWrite("Objects sent to amqp server.\n");
                }
                else if (e.AmqpReceiver.MediaName == _otherDataExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += AmqpReceiver_AmqpDataReceived_Heartbeat;
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
        void AmqpReceiver_AmqpDataReceived_Heartbeat(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.RoutingKey.Contains("SymbolAdviceRequest"))
                {
                    // store the symbol in the cache
                    String symbol = e.DsReceived.Tables[0].Rows[0]["Symbol"].ToString();
                    String underlyingSymbol = e.DsReceived.Tables[0].Rows[0]["underlyingSymbol"].ToString();
                    String fxSymbol = e.DsReceived.Tables[0].Rows[0]["fxSymbol"].ToString();
                    AssetCategory assetId = (AssetCategory)Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["assetId"].ToString());
                    int fromCurr = Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["leadCurrencyId"]); ;
                    int toCurr = Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["vsCurrencyId"]);
                    int currencyCode = Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["currencyId"]);
                    AdviceSymbolInfo symbolInfo;

                    if (assetId == AssetCategory.FXForward || assetId == AssetCategory.FX)
                    {
                        // symbol
                        symbolInfo = new AdviceSymbolInfo(symbol, fromCurr, toCurr, assetId);
                        LiveFeedManager.Instance.AddToAdviceCache(symbolInfo);
                        LiveFeedManager.Instance.AdviseSymbolForFX(symbol, fromCurr, toCurr, assetId);

                        // underlying
                        symbolInfo = new AdviceSymbolInfo(underlyingSymbol, fromCurr, toCurr, AssetCategory.FX);
                        LiveFeedManager.Instance.AddToAdviceCache(symbolInfo);
                        LiveFeedManager.Instance.AdviseSymbolForFX(underlyingSymbol, fromCurr, toCurr, assetId);
                    }
                    else
                    {
                        // symbol
                        symbolInfo = new AdviceSymbolInfo(symbol);
                        symbolInfo.AssetId = assetId;
                        LiveFeedManager.Instance.AddToAdviceCache(symbolInfo);
                        LiveFeedManager.Instance.AdviseSymbol(symbol);

                        // underlying
                        symbolInfo = new AdviceSymbolInfo(underlyingSymbol);
                        symbolInfo.AssetId = assetId;
                        LiveFeedManager.Instance.AddToAdviceCache(symbolInfo);
                        LiveFeedManager.Instance.AdviseSymbol(underlyingSymbol);
                    }

                    if (currencyCode != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                    {
                        // fx
                        symbolInfo = new AdviceSymbolInfo(fxSymbol, fromCurr, toCurr, AssetCategory.FX);
                        LiveFeedManager.Instance.AddToAdviceCache(symbolInfo);
                        LiveFeedManager.Instance.AdviseSymbolForFX(fxSymbol, fromCurr, toCurr, assetId);
                    }

                }
                else if (e.RoutingKey.Contains("HEARTBEAT"))
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

        #region Event listeners
        void amqpReceiver_AmqpDataReceived_esperRequest(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                EsperRequestRecieved(e.DsReceived);
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
        /// Inform the object about the ping
        /// </summary>
        /// <param name="routingkey"></param>
        private void HeartbeatReceived(String routingkey, DataSet ds)
        {
            try
            {
                int interval = Convert.ToInt32(ds.Tables[0].Rows[0]["Interval"].ToString());
                switch (routingkey)
                {
                    case "_EsperHEARTBEAT":
                        if (_esperConnection != null)
                        {
                            _esperConnection.GotAPing(interval);
                        }
                        break;
                    case "_RuleMediatorHEARTBEAT":
                        if (_ruleMediatorConnection != null)
                        {
                            _ruleMediatorConnection.GotAPing(interval);
                        }
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


        /*void LiveFeedReceived(object sender, EventArgs<List<SymbolData>> e)
        {
            try
            {
                if (!_initializeRequestFromEsper && e.Value != null)
                    AmqpHelper.SendObject(e.Value, "LiveFeed", "All");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //throwing exception up to the stack
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }*/

        void OrderReceived(object sender, EventArgs<EPnlOrder> e)
        {
            try
            {
                //this block of code has been executed also in DataCalculator
                //Need to move it to central position
                if (!e.Value.IsFXRateSavedWithTrade)
                {
                    ConversionRate conversionRateTradeDt = ForexConverter.GetInstance(_companyId).GetConversionRateForCurrencyToBaseCurrency(e.Value.CurrencyID, e.Value.TransactionDate, e.Value.Level1ID);
                    if (conversionRateTradeDt != null)
                    {
                        if (e.Value.FXRateOnTradeDate <= 0.0)
                        {
                            if (conversionRateTradeDt.ConversionMethod == Operator.D)
                            {
                                e.Value.FXConversionMethodOnTradeDate = Operator.M;
                                e.Value.FXRateOnTradeDate = conversionRateTradeDt.RateValue == 0 ? 0 : 1 / conversionRateTradeDt.RateValue;
                            }
                            else
                            {
                                e.Value.FXConversionMethodOnTradeDate = Operator.M;
                                e.Value.FXRateOnTradeDate = conversionRateTradeDt.RateValue;
                            }
                        }
                    }
                }

                //Send order to Esper
                SendOrdersToEsperEngine(e.Value);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                //throwing exception up to the stack
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void DayEndCashReceived(object sender, EventArgs<List<DayEndAccountCash>> e)
        {
            try
            {
                foreach (DayEndAccountCash dayEndAccountCash in e.Value)
                    AmqpHelper.SendObject(dayEndAccountCash, "OtherDataSender", "DayEndCash");
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

        void AmqpPluginManager_startOfMonthCapitalAccountUpdated(object sender, EventArgs<Dictionary<int, double>> e)
        {
            try
            {
                AmqpHelper.SendObject(e.Value, "OtherDataSender", "StartOfMonthCapitalAccount");
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

        void AmqpPluginManager_userDefinedMTDPnLCollectionUpdated(object sender, EventArgs<Dictionary<int, double>> e)
        {
            try
            {
                AmqpHelper.SendObject(e.Value, "OtherDataSender", "UserDefinedMTDPnl");
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

        void AmqpPluginManager_DbNavUpdate(object sender, EventArgs<List<DbNav>> e)
        {
            try
            {
                //sending dbnav to esper in case of isNavSaved=false
                foreach (DbNav dbNav in e.Value)
                {
                    AmqpHelper.SendObject(dbNav, "OtherDataSender", "DbNav");
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

        void AmqpPluginManager_RefreshData(Object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, object> dictCom = new Dictionary<string, object>();
                dictCom.Add("Request", "RefreshData");
                dictCom.Add("Status", true);
                AmqpHelper.SendObject(dictCom, "OtherDataSender", "RefreshData");

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

        void AuecDetailsUpdated(object sender, EventArgs<AuecDetails> e)
        {
            try
            {
                AmqpHelper.SendObject(e.Value, "OtherDataSender", "AuecDetails");
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

        void AmqpPluginManager_YesterdayFxRateUpdated(object sender, EventArgs<List<YesterdayFxRate>> e)
        {
            try
            {
                foreach (YesterdayFxRate yesterdayFxRate in e.Value)
                {
                    AmqpHelper.SendObject(yesterdayFxRate, "OtherDataSender", "YesterdayFxRates");
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


        #region Esper Request Manager
        /// <summary>
        /// Sending all data to esper through amqp
        /// </summary>
        internal void SendObjectToAmqp()
        {
            Dictionary<String, String> completionInfo = new Dictionary<string, string>();
            Dictionary<int, bool> accountNavPreferenceDict = new Dictionary<int, bool>();
            try
            {
                //Sending preferences
                Dictionary<String, Object> preferences = DataManager.GetInstance().GetPreferences();
                AmqpHelper.SendObject(preferences, "OtherDataSender", "Preferences");
                //Sending account details to esper
                foreach (AccountDetails details in DataManager.GetInstance().GetAccountCollection())
                {
                    AmqpHelper.SendObject(details, "OtherDataSender", "AccountCollection");
                    if (!accountNavPreferenceDict.ContainsKey(details.AccountId))
                        accountNavPreferenceDict.Add(details.AccountId, (bool)preferences["IsNavSaved"]);
                }
                //sending account wise nav preference to esper. Currently the isNavSaved reference is sent by default. In future the accountNavPreference Dictionary can be populated from where it is saved.
                AmqpHelper.SendObject(accountNavPreferenceDict, "OtherDataSender", "AccountNavPreferences");

                foreach (StrategyDetails strat in DataManager.GetInstance().GetStrategyCollection())
                {
                    AmqpHelper.SendObject(strat, "OtherDataSender", "StrategyCollection");
                }
                //sending dbnav to esper in case of isNavSaved=false
                foreach (DbNav dbNav in DataManager.GetInstance().GetDbNav())
                {
                    AmqpHelper.SendObject(dbNav, "OtherDataSender", "DbNav");
                }

                //Added Accurals class with accountId, StartOfDay Accrual and Day Accrual
                List<Accurals> clonedAccrual = DataManager.GetInstance().GetAccountWiseAccrual();
                if (clonedAccrual != null && clonedAccrual.Count > 0)
                {
                    foreach (Accurals accrualsObject in clonedAccrual)
                    {
                        Accurals accrual = new Accurals();
                        accrual.AccountId = accrualsObject.AccountId;
                        accrual.StartOfDayAccruals = accrualsObject.StartOfDayAccruals;
                        accrual.DayAccruals = accrualsObject.DayAccruals;
                        AmqpHelper.SendObject(accrual, "OtherDataSender", "AccrualForAccount");
                    }
                }

                //sending day end cash funnwise to esper
                foreach (DayEndAccountCash cash in DataManager.GetInstance().GetDayEndAccountCash())
                {
                    AmqpHelper.SendObject(cash, "OtherDataSender", "DayEndCash");
                }

                //sending account wise Stopout Preference to esper. 
                Dictionary<int, PMCalculationPrefs> pmPref = DataManager.GetInstance().GetPMCalculationPrefs();
                if (pmPref != null && pmPref.Count > 0)
                {
                    foreach (int accountId in pmPref.Keys)
                    {
                        Dictionary<String, Object> pmPreference = new Dictionary<String, Object>();
                        pmPreference.Add("AccountId", accountId);
                        pmPreference.Add("HighWaterMark", pmPref[accountId].HighWaterMark);
                        pmPreference.Add("StopOut", pmPref[accountId].StopOut);
                        pmPreference.Add("TradersPayoutPercent", pmPref[accountId].TraderPayoutPercent);

                        AmqpHelper.SendObject(pmPreference, "OtherDataSender", "PMCalculationPrefs");
                    }
                }

                Dictionary<int, DailyCreditLimit> creditLimit = DataManager.GetInstance().GetDailyCreditLimit();
                if (creditLimit != null && creditLimit.Count > 0)
                {
                    foreach (int accountId in creditLimit.Keys)
                    {
                        Dictionary<String, Object> dailyCreditLimit = new Dictionary<String, Object>();
                        dailyCreditLimit.Add("AccountId", accountId);
                        dailyCreditLimit.Add("LongDebitBalance", creditLimit[accountId].LongDebitBalance);
                        dailyCreditLimit.Add("LongDebitLimit", creditLimit[accountId].LongDebitLimit);
                        dailyCreditLimit.Add("ShortCreditBalance", creditLimit[accountId].ShortCreditBalance);
                        dailyCreditLimit.Add("ShortCreditLimit", creditLimit[accountId].ShortCreditLimit);

                        AmqpHelper.SendObject(dailyCreditLimit, "OtherDataSender", "DailyCreditLimit");
                    }
                }

                //sending account wise start of month capital to esper
                Dictionary<int, double> startOfMonthCapital = DataManager.GetInstance().GetStartOfMonthCapitalAccount();
                if (startOfMonthCapital != null && startOfMonthCapital.Count > 0)
                {
                    AmqpHelper.SendObject(startOfMonthCapital, "OtherDataSender", "StartOfMonthCapitalAccount");
                }
                //sending account wise mtdPnl to esper
                Dictionary<int, double> mtdPnl = DataManager.GetInstance().GetMTDPnl();
                if (mtdPnl != null && mtdPnl.Count > 0)
                {
                    AmqpHelper.SendObject(mtdPnl, "OtherDataSender", "UserDefinedMTDPnl");
                }

                //Sending auec details to esper
                foreach (AuecDetails auecDetails in DataManager.GetInstance().GetAUECDetails())
                {
                    AmqpHelper.SendObject(auecDetails, "OtherDataSender", "AuecDetails");
                }
                Dictionary<string, string> orderTypeDict = TagDatabaseManager.GetInstance.GetAllOrderTypeTags();
                orderTypeDict.Add("-1", "Not Applicable");
                AmqpHelper.SendObject(orderTypeDict, "OtherDataSender", "OrderTypeTags");
                AmqpHelper.SendObject(TagDatabaseManager.GetInstance.GetAllOrderSides(), "OtherDataSender", "OrderSides");

                ProcessAndSendYesterdayFxRates();

                //sending venue and counterparty
                AmqpHelper.SendObject(CachedDataManager.GetInstance.GetAllCounterParties(), "OtherDataSender", "CounterPartyCollection");
                AmqpHelper.SendObject(CachedDataManager.GetInstance.GetAllVenues(), "OtherDataSender", "VenueCollection");

                //sending weekly holidays per AUEC
                AmqpHelper.SendObject(BusinessDayCalculator.GetInstance().GetAUECWiseWeeklyHolidaysFromCache(), "OtherDataSender", "WeeklyHolidaysEvent");

                //sending auec holidays
                AmqpHelper.SendObject(BusinessDayCalculator.GetInstance().GetAUECWiseYearlyHolidaysFromCache(), "OtherDataSender", "YearlyHolidaysEvent");

                foreach (EPnlOrder historicalOrder in DataManager.GetInstance().GetUncalculatedDataClone())
                {
                    //Loading Trade FX rates
                    if (!historicalOrder.IsFXRateSavedWithTrade)
                    {
                        ConversionRate conversionRateTradeDt = ForexConverter.GetInstance(_companyId).GetConversionRateForCurrencyToBaseCurrency(historicalOrder.CurrencyID, historicalOrder.TransactionDate, historicalOrder.Level1ID);
                        if (conversionRateTradeDt != null)
                        {
                            if (historicalOrder.FXRateOnTradeDate <= 0.0)
                            {
                                if (conversionRateTradeDt.ConversionMethod == Operator.D)
                                {
                                    historicalOrder.FXConversionMethodOnTradeDate = Operator.M;
                                    historicalOrder.FXRateOnTradeDate = conversionRateTradeDt.RateValue == 0 ? 0 : 1 / conversionRateTradeDt.RateValue;
                                }
                                else
                                {
                                    historicalOrder.FXConversionMethodOnTradeDate = Operator.M;
                                    historicalOrder.FXRateOnTradeDate = conversionRateTradeDt.RateValue;
                                }
                            }
                        }
                    }

                    AmqpHelper.SendObject(historicalOrder, "OtherDataSender", "HistoricalTaxlots");
                }
                Dictionary<String, object> dictCom = new Dictionary<string, object>();
                dictCom.Clear();
                dictCom.Add("Request", "HistoricalTaxlotCompleted");
                dictCom.Add("Status", true);
                AmqpHelper.SendObject(dictCom, "OtherDataSender", "HistoricalTaxlotCompleted");

                Dictionary<String, Double> clonedBeta = DataManager.GetInstance().GetBetaForSymbol();
                if (clonedBeta != null && clonedBeta.Count > 0)
                {
                    AmqpHelper.SendObject(clonedBeta, "OtherDataSender", "BetaForSymbol");
                }

                Dictionary<int, double> accountWiseNra = DataManager.GetInstance().GetAccountWiseNra();
                if (accountWiseNra != null && accountWiseNra.Count > 0)
                {
                    AmqpHelper.SendObject(accountWiseNra, "OtherDataSender", "AccountWiseNRA");
                }
                Dictionary<String, double> avgVolCustomDays = DataManager.GetInstance().GetAvgVolumeCustomDays();
                if (avgVolCustomDays != null && avgVolCustomDays.Count > 0)
                {
                    AmqpHelper.SendObject(avgVolCustomDays, "OtherDataSender", "AvgVolCustomDays");
                }

                Dictionary<String, SymbolData> liveFeedCacheLocal = LiveFeedManager.Instance.GetLiveFeedCacheClone();
                if (liveFeedCacheLocal != null && liveFeedCacheLocal.Count > 0)
                {
                    foreach (String symbol in liveFeedCacheLocal.Keys)
                    {
                        AmqpHelper.SendObject(liveFeedCacheLocal[symbol], "OtherDataSender", "InitialLiveFeed");
                    }
                }

                completionInfo.Add("ResponseType", "InitCompleteExPnL");
                AmqpHelper.SendObject(completionInfo, "OtherDataSender", "InitCompleteInfo");
            }
            catch (Exception ex)
            {
                completionInfo.Add("ResponseType", "InitCompleteInfoExPnLFailed");
                AmqpHelper.SendObject(completionInfo, "OtherDataSender", "InitCompleteInfo");
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Processes and sends FX conversion rates for "yesterday" (or latest available).
        /// </summary>
        private void ProcessAndSendYesterdayFxRates()
        {
            try
            {
                var yesterdayFxRates = DataManager.GetInstance().GetYesterdayFxRates();
                var fxRateDict = yesterdayFxRates.ToDictionary(rate => rate.CurrencySymbol, rate => rate);

                var forexConverter = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID());
                var currencyPairs = forexConverter.GetStandardCurrencyPairsWithIDs();

                // Populate fxRateDict for all remaining standard currency pairs
                foreach (var (fromCurrency, toCurrency) in currencyPairs)
                {
                    string symbol = forexConverter.GetPranaForexSymbolFromCurrencies(fromCurrency, toCurrency);
                    string reverseSymbol = forexConverter.GetPranaForexSymbolFromCurrencies(toCurrency, fromCurrency);

                    // Avoid duplicate entries by checking both symbol directions
                    if (!fxRateDict.ContainsKey(symbol) && !fxRateDict.ContainsKey(reverseSymbol))
                    {
                        var rate = forexConverter.GetConversionRateFromCurrencies(fromCurrency, toCurrency, 0);

                        var newFxRate = new YesterdayFxRate
                        {
                            CurrencySymbol = symbol,
                            ConversionMethodOperator = rate.ConversionMethod.ToString(),
                            ConversionRate = rate.RateValue,
                            RateTime = rate.Date
                        };

                        fxRateDict[symbol] = newFxRate;
                    }
                }

                foreach (YesterdayFxRate fxRate in fxRateDict.Values)
                {
                    AmqpHelper.SendObject(fxRate, "OtherDataSender", "YesterdayFxRates");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions using the logging policy
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void EsperRequestRecieved(DataSet dsRceived)
        {
            Dictionary<String, String> completionInfo = new Dictionary<string, string>();
            try
            {
                if (dsRceived.Tables[0].Rows[0]["TypeOfRequest"].ToString() == "InitializationRequest")
                {
                    //Object is send to Amqp when esper Request is recieved (or esper started)
                    InformationReporter.GetInstance.ComplianceLogWrite("Refreshed Data initiated by Esper calculation engine.\n");
                    SendObjectToAmqp();
                    InformationReporter.GetInstance.ComplianceLogWrite("Refreshed Data completed.\n");
                    _initializeRequestFromEsper = true;
                    Logger.LoggerWrite("Collecting live fills as Esper is in start mode");
                }
                else if (dsRceived.Tables[0].Rows[0]["TypeOfRequest"].ToString() == "EsperStartedCompletely")
                {
                    ProcessPendingOrders();
                }
            }
            catch (Exception ex)
            {
                completionInfo.Add("ResponseType", "InitCompleteInfoExPnLFailed");
                AmqpHelper.SendObject(completionInfo, "OtherDataSender", "InitCompleteInfo");
                //Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Process pending Taxlots
        /// </summary>
        private void ProcessPendingOrders()
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    if (_pendingOrders.Count > 0)
                    {
                        foreach (EPnlOrder order in _pendingOrders.Values)
                            AmqpHelper.SendObject(order, "Order", null);

                        Logger.LoggerWrite("Sending collected live fills to Esper. Total fills sent: " + _pendingOrders.Count);
                        _pendingOrders.Clear();
                    }

                    //Change _isEsperConnected variable to True, after sending all pending orders to Esper
                    _initializeRequestFromEsper = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Send orders to Esper Engine
        /// </summary>
        /// <param name="e"></param>
        private void SendOrdersToEsperEngine(EPnlOrder epnlOrder)
        {
            try
            {
                lock (_cacheLockerObject)
                {
                    if (_initializeRequestFromEsper)
                    {
                        if (!_pendingOrders.ContainsKey(epnlOrder.ID))
                            _pendingOrders.Add(epnlOrder.ID, epnlOrder);
                        else
                            _pendingOrders[epnlOrder.ID] = epnlOrder;
                    }
                    else
                        AmqpHelper.SendObject(epnlOrder, "Order", null);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
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
                //Invoke our policy that is responsible for making sure no secure information
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