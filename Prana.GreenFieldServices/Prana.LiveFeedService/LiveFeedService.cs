using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.BlotterDataService;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Permissions;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.GreenfieldServices.Common;
using Prana.Interfaces;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace Prana.LiveFeedService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class LiveFeedService : BaseService, ILiveFeedService, IMarketDataPermissionServiceCallback, IDisposable
    {
        #region Variables
        private IWindsorContainer _container;

        private ServerHeartbeatManager _LiveFeedServiceHeartbeatManager;
        /// <summary>
        /// Symbol wise live feed cache
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> symbolData = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Subscription wise Symbol cache
        /// </summary>
        private static Dictionary<string, string> liveFeedSymbolCache = new Dictionary<string, string>();

        /// <summary>
        /// Compliance permissions data
        /// </summary>
        Dictionary<string, string> _compliancePermissionsData = new Dictionary<string, string>();

        /// <summary>
        /// Compliance permissions for user
        /// </summary>
        Dictionary<int, CompliancePermissions> _compliancePermissions = new Dictionary<int, CompliancePermissions>();

        /// <summary>
        /// Stores information of Users logged into Web application.
        /// </summary>
        private static Dictionary<int, AuthenticatedUserInfo> _dictLoggedInUser = new Dictionary<int, AuthenticatedUserInfo>();

        /// <summary>
        /// User wise pre trade compliance check permission
        /// </summary>
        Dictionary<int, bool> _preTradeModule = new Dictionary<int, bool>();

        private DuplexProxyBase<IMarketDataPermissionService> _marketDataPermissionServiceProxy = null;

        private object _isMarketDataPermissionEnabledLock = new object();

        private System.Timers.Timer _marketDataPermissionRecheckTimer;

        private int _marketDataPermissionRecheckInterval = int.Parse(ConfigurationManager.AppSettings["MarketDataPermissionRecheckInterval"]);

        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;

        /// <summary>
        /// Tracks whether the logged-in user response has been received from Auth Service
        /// True if received; otherwise, false.
        /// </summary>
        bool _loggedInUserResponseReceivedFromAuth = false;

        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

        private int _cleanedUp = 0;

        #endregion

        #region Private Methods

        /// <summary>  
        /// Publishes the current service status to a Kafka topic.  
        /// This method retrieves the service status, serializes it, and sends it to the Kafka topic  
        /// specified in the KafkaConstants.TOPIC_ServiceHealthStatus.  
        /// </summary>  
        private async void ProduceServiceStatusMessage()
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_LiveFeed_Name, ServiceNameConstants.CONST_LiveFeed_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_LiveFeed_Name);
                var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);

                Logger.LogMsg(LoggerLevel.Verbose, "Service status published to Kafka topic {0}", KafkaConstants.TOPIC_ServiceHealthStatus);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceServiceStatusMessage encountered an error");
            }
        }

        private void CreateMarketDataPermissionServiceProxy()
        {
            try
            {
                _marketDataPermissionServiceProxy = new DuplexProxyBase<IMarketDataPermissionService>("PricingMarketDataPermissionServiceEndpointAddress", this);
                Logger.LogMsg(LoggerLevel.Information, "Created MarketDataPermissionServiceProxy successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CreateMarketDataPermissionServiceProxy encountered an error");
            }
        }

        public void PermissionCheckResponse(int companyUserID, bool isPermitted)
        {
            try
            {
                lock (_isMarketDataPermissionEnabledLock)
                {
                    RequestResponseModel response = new RequestResponseModel(companyUserID, JsonHelper.SerializeObject(isPermitted));
                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_MarketDataPermissionResponse, response);
                    Logger.LogMsg(LoggerLevel.Information, "PermissionCheck response from Pricing Server:{0} for user {1}",
                        response.Data, companyUserID);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "PermissionCheckResponse encountered an error");
            }
        }

        private void ResetPermissionRecheckTimer()
        {
            try
            {
                if (_marketDataPermissionRecheckTimer == null)
                {
                    _marketDataPermissionRecheckTimer = new System.Timers.Timer();
                    _marketDataPermissionRecheckTimer.Elapsed += RecheckMarketDataPermission_Elapsed;
                    _marketDataPermissionRecheckTimer.Interval = _marketDataPermissionRecheckInterval * 1000;

                    Logger.LogMsg(LoggerLevel.Information, "PermissionRecheckTimer reset done");
                }
                else
                {
                    _marketDataPermissionRecheckTimer.Stop();
                    Logger.LogMsg(LoggerLevel.Information, "MarketDataPermissionRecheck timer stop");
                }

                _marketDataPermissionRecheckTimer.Start();
                Logger.LogMsg(LoggerLevel.Information, "MarketDataPermissionRecheck timer started");

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ResetPermissionRecheckTimer encountered an error");

            }
        }

        private void RecheckMarketDataPermission_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                GetMarketDataPermissions();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RecheckMarketDataPermission_Elapsed encountered an error");

            }
        }

        private void GetMarketDataPermissions()
        {
            try
            {
                System.Threading.Tasks.Task.Run(new Action(() =>
                {
                    lock (_dictLoggedInUser)
                    {
                        foreach (AuthenticatedUserInfo authenticatedUserInfo in _dictLoggedInUser.Values)
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Authenticated User Details. UserID: {0}, MarketDataProvider: {1}", authenticatedUserInfo.CompanyUserId, authenticatedUserInfo.CompanyMarketDataProvider), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                            Logger.LogMsg(LoggerLevel.Information, "Company market data provider for User {0} is {1}: ", authenticatedUserInfo.CompanyUser.LoginID,
                                Convert.ToString(authenticatedUserInfo.CompanyMarketDataProvider));
                            ;
                            if (authenticatedUserInfo.CompanyMarketDataProvider == MarketDataProvider.FactSet)
                            {
                                Logger.LogMsg(LoggerLevel.Information, "FactSet User PermissionCheck request.FactSetSerialNumber: {0}, MarketDetailIPAdresses: {1}", authenticatedUserInfo.CompanyUser.FactSetUsernameAndSerialNumber);

                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(authenticatedUserInfo.CompanyUser.FactSetUsernameAndSerialNumber) && !string.IsNullOrWhiteSpace(authenticatedUserInfo.CompanyUser.MarketDataAccessIPAddresses))
                                    {
                                        _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new FactSetMarketDataPermissionRequest()
                                        {
                                            CompanyUserID = authenticatedUserInfo.CompanyUserId,
                                            FactSetSerialNumber = authenticatedUserInfo.CompanyUser.FactSetUsernameAndSerialNumber,
                                            IsFactSetSupportUser = authenticatedUserInfo.CompanyUser.IsFactSetSupportUser,
                                            IpAddress = authenticatedUserInfo.CompanyUser.MarketDataAccessIPAddresses
                                        }, LiveFeedConstants.CONST_SAMSARA);
                                    }
                                    else
                                    {
                                        Logger.LogMsg(LoggerLevel.Fatal, "Unable to retrieve logged in machine's IPAddress for User: " + authenticatedUserInfo.CompanyUser.LoginID);
                                    }
                                }
                                catch (Exception)
                                {
                                    Logger.LogMsg(LoggerLevel.Fatal, "Error encountered while getting factset configuration/permmsision for User: " + authenticatedUserInfo.CompanyUser.LoginID);
                                }
                            }
                            else if (authenticatedUserInfo.CompanyMarketDataProvider == MarketDataProvider.ACTIV)
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("ACTIV User PermissionCheck request. ACTIV Username: {0}, ACTIV Password: {1}", authenticatedUserInfo.CompanyUser.ActivUsername, authenticatedUserInfo.CompanyUser.ActivPassword), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                                Logger.LogMsg(LoggerLevel.Information, "ACTIV User PermissionCheck request. ACTIV Username: {0}, ACTIV Password: {1}", authenticatedUserInfo.CompanyUser.ActivUsername, authenticatedUserInfo.CompanyUser.ActivPassword);

                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(authenticatedUserInfo.CompanyUser.ActivUsername) && !string.IsNullOrWhiteSpace(authenticatedUserInfo.CompanyUser.ActivPassword))
                                    {
                                        _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new ActivMarketDataPermissionRequest()
                                        {
                                            CompanyUserID = authenticatedUserInfo.CompanyUserId,
                                            ActivUsername = authenticatedUserInfo.CompanyUser.ActivUsername,
                                            ActivPassword = authenticatedUserInfo.CompanyUser.ActivPassword
                                        }, "Client");
                                    }
                                }
                                catch (Exception)
                                {
                                    Logger.LogMsg(LoggerLevel.Fatal, "Error encountered while getting ACTIV configuration/permisision for User: " + authenticatedUserInfo.CompanyUser.LoginID);
                                }
                            }
                            else if (authenticatedUserInfo.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                            {
                                //Do nothing, this call is redirected from Authentication Service
                            }
                            else
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("Other User PermissionCheck request. UserID: {0}", authenticatedUserInfo.CompanyUserId), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                                try
                                {
                                    _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(new MarketDataPermissionRequest()
                                    {
                                        CompanyUserID = authenticatedUserInfo.CompanyUserId
                                    }, "Client");
                                }
                                catch (Exception ex)
                                {
                                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                }
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetMarketDataPermissions encountered an error");
            }
        }

        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                Console.WriteLine(e.Value);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetInstance_InformationReceived encountered an error");
            }
        }

        /// <summary>
        /// PricingService2ClientHeartbeatManager_ConnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PricingService2ClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Pricing_Name, ServiceNameConstants.CONST_Pricing_DisplayName, true);
                CreateMarketDataPermissionServiceProxy();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// PricingService2ClientHeartbeatManager_DisconnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PricingService2ClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Pricing_Name, ServiceNameConstants.CONST_Pricing_DisplayName, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Information, PranaMessageConstants.MSG_AnotherInstanceSubscribed);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region IPranaServiceCommon Methods
        public async System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                #region Client Heartbeat Setup
                _pricingService2ClientHeartbeatManager = new ClientHeartbeatManager<IPricingService2>(EndPointAddressConstants.CONST_PricingService2Endpoint);
                _pricingService2ClientHeartbeatManager.ConnectedEvent += PricingService2ClientHeartbeatManager_ConnectedEvent;
                _pricingService2ClientHeartbeatManager.DisconnectedEvent += PricingService2ClientHeartbeatManager_DisconnectedEvent;
                _pricingService2ClientHeartbeatManager.AnotherInstanceSubscribedEvent += PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent;
                #endregion

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);
                CreateMarketDataPermissionServiceProxy();
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("LiveFeedService started at:- " + DateTime.Now + " (local time)", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                MarketDataHelper.GetInstance().OnResponse += LiveFeedService_OnResponse;
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompliancePermissionsResponse, KafkaManager_CompliancePermissionsResponse);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LiveFeedRequest, KafkaManager_MessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UnSubscribeLiveFeedRequest, KafkaManager_UnSubscribeLiveFeedRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UpdateMarketDataTokenRequest, UpdateMarketDataTokenRequestHandler);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_MultipleSymbolsLiveFeedSnapshotRequest, KafkaManager_MultipleSymbolsLiveFeedSnapshotRequestReceived);

                #region Topics for user login/logout
                //Topic for logged-in user(s) information
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_UserLoggedInInformation);
                //Topic for logged-out user(s) information
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_UserLoggedOutInformation);

                // fire and forget method for handling of auth service connection
                ServiceConnectionPoller.PollUntilServiceReady(() => _loggedInUserResponseReceivedFromAuth, KafkaConstants.TOPIC_InitializeLoggedInUserRequest, ServiceNameConstants.CONST_Auth_DisplayName);
                #endregion

                ResetPermissionRecheckTimer();

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");

                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);

                #region Server Heartbeat Setup
                _LiveFeedServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                //Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                    "InitialiseService Completed for Live Feed Service in {0} ms.", sw.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitialiseService encountered an error");
            }

            return false;
        }

        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.
            if (Interlocked.Exchange(ref _cleanedUp, 1) == 1) return;

            // 1) Stop periodic callbacks BEFORE disposing anything they use
            StopServiceHealthPollingTimer();

            // 2) Mark down (and publish a final “down”)
            UpdateServiceStatus(ServiceNameConstants.CONST_LiveFeed_Name, ServiceNameConstants.CONST_LiveFeed_DisplayName, false);

            Console.WriteLine("Shutting down Service...");
            Logger.LogMsg(LoggerLevel.Information, "Shutting down Service...");
            _container.Dispose();
        }
        #endregion

        #region Kafka Methods
        /// <summary>
        /// UnSubscribe prices for symbol based on LiveFeedCache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UnSubscribeLiveFeedRequestReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_LiveFeedUnsubscriptionReceived + message.CompanyUserID);
                    lock (symbolData)
                    {
                        dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                        string requestID = data.RequestedInstance;

                        // Find matching keys starting with requestID
                        var matchingKeys = liveFeedSymbolCache.Keys
                            .Where(k => k.StartsWith(requestID, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        foreach (var key in matchingKeys)
                        {
                            var symbol = liveFeedSymbolCache[key];

                            if (!string.IsNullOrEmpty(symbol) && !CheckIfSymbolSubscribedInOtherUsers(symbol, key))
                            {
                                MarketDataHelper.GetInstance().RemoveSingleSymbol(symbol);
                                if (symbolData.ContainsKey(symbol))
                                    symbolData.Remove(symbol);
                            }

                            if (matchingKeys.Count == 1 && GetPreTradeCheckForLiveOrders(message.CompanyUserID))
                            {
                                List<string> _requestedSymbolsComplianceSnapshot = new List<string> { symbol };
                                MarketDataHelper.GetInstance().RemoveSnapshotForCompliance(_requestedSymbolsComplianceSnapshot);
                            }

                            liveFeedSymbolCache[key] = string.Empty;
                        }

                        Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_LiveFeedUnsubscriptionProcessed + message.CompanyUserID);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UnSubscribeLiveFeedRequestReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Subscribe prices for symbol based on LiveFeedCache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_MessageReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    lock (symbolData)
                    {
                        Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_LiveFeedSubscriptionReceived + message.CompanyUserID);
                        dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                        string requestedSymbol = data.RequestedSymbol;
                        string requestID = data.RequestedInstance;

                        Console.WriteLine(LiveFeedConstants.MSG_LiveFeedSubscriptionForSymbol, requestedSymbol, message.CompanyUserID.ToString());
                        if (symbolData.ContainsKey(requestedSymbol))
                        {
                            symbolData.Remove(requestedSymbol);
                        }

                        if (liveFeedSymbolCache.ContainsKey(requestID))
                        {
                            if (!CheckIfSymbolSubscribed(requestedSymbol))
                                MarketDataHelper.GetInstance().RequestSingleSymbol(requestedSymbol, false);

                            if (!string.IsNullOrEmpty(liveFeedSymbolCache[requestID]) && !CheckIfSymbolSubscribedInOtherUsers(liveFeedSymbolCache[requestID], requestID))
                            {
                                MarketDataHelper.GetInstance().RemoveSingleSymbol(liveFeedSymbolCache[requestID]);
                                if (symbolData.ContainsKey(liveFeedSymbolCache[requestID]))
                                    symbolData.Remove(liveFeedSymbolCache[requestID]);
                            }
                            liveFeedSymbolCache[requestID] = requestedSymbol;
                        }
                        else
                        {
                            if (!CheckIfSymbolSubscribed(requestedSymbol))
                                MarketDataHelper.GetInstance().RequestSingleSymbol(requestedSymbol, false);
                            liveFeedSymbolCache.Add(requestID, requestedSymbol);
                        }

                        if (GetPreTradeCheckForLiveOrders(message.CompanyUserID))
                        {
                            List<string> _requestedSymbolsComplianceSnapshot = new List<string>() { requestedSymbol };
                            MarketDataHelper.GetInstance().RequestSnapshotForCompliance(_requestedSymbolsComplianceSnapshot);
                        }
                        Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_LiveFeedSubscriptionProcessed + message.CompanyUserID);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_MessageReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Prices for multiple symbols based on LiveFeedCache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_MultipleSymbolsLiveFeedSnapshotRequestReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Live feed snapshot request received for multiple symbols for company user: {0}", message.CompanyUserID);

                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    JArray symbolArray = data?.RequestedSymbols;
                    string requestedInstance = data?.RequestedInstance;

                    List<string> requestedSymbols = symbolArray
                        ?.ToObject<List<string>>()
                        ?.Select(s => s.Trim().ToUpper())
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList();

                    lock (symbolData)
                    {
                        Logger.LogMsg(LoggerLevel.Information, "Subscribing multiple symbols: {0} for company user: {1}",
                            string.Join(", ", requestedSymbols), message.CompanyUserID);

                        foreach (var symbol in requestedSymbols)
                        {
                            string requestId = $"{message.CompanyUserID}_{requestedInstance}_{symbol}";

                            if (symbolData.ContainsKey(symbol))
                            {
                                symbolData.Remove(symbol);
                            }

                            // Clean old symbol if requestId already exists in cache
                            if (liveFeedSymbolCache.ContainsKey(requestId))
                            {
                                string oldSymbol = liveFeedSymbolCache[requestId];

                                if (!string.IsNullOrEmpty(oldSymbol) &&
                                    !CheckIfSymbolSubscribedInOtherUsers(oldSymbol, requestId))
                                {
                                    MarketDataHelper.GetInstance().RemoveSingleSymbol(oldSymbol);
                                    if (symbolData.ContainsKey(oldSymbol))
                                        symbolData.Remove(oldSymbol);
                                }
                            }

                            MarketDataHelper.GetInstance().RequestSingleSymbol(symbol, true);

                            liveFeedSymbolCache[requestId] = symbol;
                        }

                        Logger.LogMsg(LoggerLevel.Information, "Live feed subscription for multiple symbols processed for company user: {0}", message.CompanyUserID);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_MultipleSymbolsLiveFeedRequestReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// User-Wise Compliance Permission
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompliancePermissionsResponse(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_CompliancePermissionReceived + message.CompanyUserID);
                    _compliancePermissionsData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data);
                    if (_preTradeModule.Count > 0)
                        _preTradeModule.Clear();
                    if (_compliancePermissions.Count > 0)
                        _compliancePermissions.Clear();
                    _preTradeModule = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_PRE_TRADE_USERS]);
                    _compliancePermissions = JsonHelper.DeserializeToObject<Dictionary<int, CompliancePermissions>>(_compliancePermissionsData[ComplianceConstants.CONST_COMPLIANCE_PERMISSIONS]);
                    Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_CompliancePermissionProcessed + message.CompanyUserID);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_CompliancePermissionsResponse encountered an error");
                }
            }
        }

        private void UpdateMarketDataTokenRequestHandler(string topic, RequestResponseModel model)
        {
            using (LoggerHelper.PushLoggingProperties(model.CorrelationId, model.RequestID, model.CompanyUserID))
            {
                try
                {
                    lock (_dictLoggedInUser)
                    {
                        Dictionary<string, string> data = JsonHelper.DeserializeToObject<Dictionary<string, string>>(model.Data);
                        int companyUserID = model.CompanyUserID;
                        if (_dictLoggedInUser.ContainsKey(companyUserID))
                        {
                            if (data.ContainsKey(LiveFeedConstants.CONST_MarketDataAccessIPAddresses))
                            {
                                string updatedMarketDataAccessIPAddresses = data[LiveFeedConstants.CONST_MarketDataAccessIPAddresses];
                                if (!string.IsNullOrEmpty(updatedMarketDataAccessIPAddresses))
                                {
                                    _dictLoggedInUser[companyUserID].CompanyUser.MarketDataAccessIPAddresses = updatedMarketDataAccessIPAddresses;
                                    Logger.LogMsg(LoggerLevel.Information, LiveFeedConstants.MSG_MarketDataIPAddressesUpdated + companyUserID.ToString() + " with values " + _dictLoggedInUser[companyUserID].CompanyUser.MarketDataAccessIPAddresses);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UpdateMarketDataTokenRequestHandler encountered an error");
                }
            }
        }

        private void KafkaManager_UserLoggedInInformation(string arg1, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                if (!_loggedInUserResponseReceivedFromAuth)
                {
                    _loggedInUserResponseReceivedFromAuth = true;
                    Logger.LogMsg(LoggerLevel.Information, "Successfully connected Auth Service.");
                }
                try
                {
                    lock (_dictLoggedInUser)
                    {
                        Dictionary<int, AuthenticatedUserInfo> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<int, AuthenticatedUserInfo>>(message.Data);
                        foreach (var kvp in keyValuePairs)
                        {
                            if (!_dictLoggedInUser.ContainsKey(kvp.Key))
                            {
                                _dictLoggedInUser.Add(kvp.Key, kvp.Value);
                                Logger.LogMsg(LoggerLevel.Information, BlotterDataConstants.MSG_UserLoggedInInformationProcessed + kvp.Key);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_UserLoggedInInformation encountered an error");
                }
            }
        }

        private void KafkaManager_UserLoggedOutInformation(string arg1, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_UserLoggedOutInformationReceived + message.CompanyUserID);
                    lock (_dictLoggedInUser)
                    {
                        if (_dictLoggedInUser.ContainsKey(message.CompanyUserID))
                        {
                            _dictLoggedInUser.Remove(message.CompanyUserID);
                            ClearSymbolCompressionCache(message.CompanyUserID);
                        }
                    }
                    Logger.LogMsg(LoggerLevel.Information, BlotterDataConstants.MSG_UserLoggedOutInformationProcessed + message.CompanyUserID);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_UserLoggedOutInformation encountered an error");
                }
            }
        }
        #endregion

        #region Cache Maintenance methods
        /// <summary>
        /// Check if the symbol is subscribed by any other users (exculding itself)
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="requestUserID"></param>
        private bool CheckIfSymbolSubscribedInOtherUsers(string symbol, string requestUserID)
        {
            int userID;
            bool success = int.TryParse(requestUserID, out userID);

            using (LoggerHelper.PushUserPropInLogging(userID))
            {
                try
                {
                    foreach (string RequestID in liveFeedSymbolCache.Keys)
                    {
                        if (!RequestID.Equals(requestUserID))
                        {
                            if (liveFeedSymbolCache[RequestID].Equals(symbol))
                                return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"CheckIfSymbolSubscribedInOtherUsers encountered an error for user {requestUserID} & symbol {symbol}");
                }
                return false;
            }
        }

        /// <summary>
        /// Check if the symbol is subscribed by any users (including itself)
        /// </summary>
        /// <param name="symbol"></param>
        private bool CheckIfSymbolSubscribed(string symbol)
        {
            try
            {
                if (liveFeedSymbolCache.ContainsValue(symbol))
                    return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CheckIfSymbolSubscribed encountered an error for symbol:" + symbol);
            }
            return false;
        }

        /// <summary>
        /// Sends a live feed response message to all users who are currently subscribed to the specified symbol.
        /// Iterates through the <c>liveFeedSymbolCache</c> and, for each user subscribed to <paramref name="symbol"/>,
        /// sets the <see cref="RequestResponseModel.CompanyUserID"/> and produces a message to the Kafka live feed response topic.
        /// </summary>
        /// <param name="symbol">
        /// The symbol for which to send the live feed response to all subscribed users.
        /// </param>
        /// <param name="message">
        /// The <see cref="RequestResponseModel"/> containing the data to be sent to each user.
        /// The <c>CompanyUserID</c> property will be set for each user before sending.
        /// </param>
        private async void SendUserWiseMessageIfSymbolSubscribed(string symbol, RequestResponseModel message)
        {
            try
            {
                foreach (string requestID in liveFeedSymbolCache.Keys)
                {
                    if (!string.IsNullOrEmpty(liveFeedSymbolCache[requestID]) && liveFeedSymbolCache[requestID].Equals(symbol))
                    {
                        string[] splitRequestID = requestID.Split('_');
                        if (int.TryParse(splitRequestID[0], out int userID))
                        {
                            message.CompanyUserID = userID;
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LiveFeedResponse, message);
                        }
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
        /// Clears all symbol subscriptions and related cache entries for a specific user.
        /// This method removes all symbols associated with the given <paramref name="companyUserID"/> from the <c>liveFeedSymbolCache</c>.
        /// If a symbol is not subscribed by any other user, it is also removed from the <c>symbolData</c> cache,
        /// and any compliance snapshots for that symbol are removed if pre-trade checks are enabled for the user.
        /// </summary>
        /// <param name="companyUserID">
        /// The company user ID whose symbol subscriptions and cache entries should be cleared.
        /// </param>
        private void ClearSymbolCompressionCache(int companyUserID)
        {
            try
            {
                // Create a list to store keys to remove to avoid modifying the collection during iteration
                List<string> keysToRemove = new List<string>();

                foreach (string requestID in liveFeedSymbolCache.Keys)
                {
                    string[] splitRequestID = requestID.Split('_');
                    if (splitRequestID.Length > 0 && int.TryParse(splitRequestID[0], out int userID))
                    {
                        if (companyUserID == userID)
                        {
                            // Add the key to the removal list
                            keysToRemove.Add(requestID);

                            string subscribedSymbol = liveFeedSymbolCache[requestID];
                            // Dispose of and clear the inner dictionary
                            if (!string.IsNullOrEmpty(subscribedSymbol))
                            {
                                if (!CheckIfSymbolSubscribedInOtherUsers(subscribedSymbol, requestID))
                                {
                                    MarketDataHelper.GetInstance().RemoveSingleSymbol(subscribedSymbol);
                                    if (symbolData.ContainsKey(subscribedSymbol))
                                        symbolData.Remove(subscribedSymbol);

                                    if (GetPreTradeCheckForLiveOrders(userID))
                                    {
                                        List<string> _requestedSymbolsComplianceSnapshot = new List<string>() { subscribedSymbol };
                                        MarketDataHelper.GetInstance().RemoveSnapshotForCompliance(_requestedSymbolsComplianceSnapshot);
                                    }
                                }
                            }
                        }
                    }
                }

                // Remove keys after iteration to avoid runtime exceptions
                foreach (string key in keysToRemove)
                {
                    liveFeedSymbolCache.Remove(key);
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
        /// Event receiving livefeed data after requesting
        /// </summary>
        private void LiveFeedService_OnResponse(object sender, EventArgs<SymbolData> e)
        {
            try
            {
                bool isResponseNeedToSend = false;
                Dictionary<string, Dictionary<string, string>> dataNeededToBeSent = new Dictionary<string, Dictionary<string, string>>();

                lock (symbolData)
                {
                    string symbol = e.Value.Symbol;
                    string ask = e.Value.Ask.ToString();
                    string bid = e.Value.Bid.ToString();
                    string change = e.Value.Change.ToString();
                    string lastPrice = e.Value.LastPrice.ToString();
                    string updateTime = e.Value.UpdateTime.ToString();

                    //Updating the cache as some calls before removal still fills the cache
                    if (!CheckIfSymbolSubscribed(symbol))
                    {
                        symbolData.Remove(symbol);
                        return;
                    }

                    if (symbolData.ContainsKey(symbol))
                    {
                        string symbolDataCacheUpdateTime = symbolData[symbol][LiveFeedConstants.CONST_UPDATE_TIME];

                        if (!(symbolDataCacheUpdateTime.Equals(updateTime)))
                        {
                            symbolData[symbol][LiveFeedConstants.CONST_ASK] = ask;
                            symbolData[symbol][LiveFeedConstants.CONST_BID] = bid;
                            symbolData[symbol][LiveFeedConstants.CONST_CHANGE] = change;
                            symbolData[symbol][LiveFeedConstants.CONST_LAST_PRICE] = lastPrice;
                            symbolData[symbol][LiveFeedConstants.CONST_UPDATE_TIME] = updateTime;
                            isResponseNeedToSend = true;
                        }
                    }
                    else
                    {
                        var data = new Dictionary<string, string>()
                        {
                            {LiveFeedConstants.CONST_ASK, ask }, {LiveFeedConstants.CONST_BID, bid},
                            {LiveFeedConstants.CONST_CHANGE, change}, {LiveFeedConstants.CONST_LAST_PRICE, lastPrice }, {LiveFeedConstants.CONST_UPDATE_TIME, updateTime }
                        };
                        symbolData.Add(symbol, data);
                        isResponseNeedToSend = true;
                    }
                    if (isResponseNeedToSend)
                    {
                        dataNeededToBeSent.Add(symbol, symbolData[symbol]);
                    }
                }
                if (isResponseNeedToSend && dataNeededToBeSent.Count > 0)
                {
                    RequestResponseModel response = new RequestResponseModel(0, JsonHelper.SerializeObject(dataNeededToBeSent));
                    SendUserWiseMessageIfSymbolSubscribed(dataNeededToBeSent.Keys.FirstOrDefault(), response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "LiveFeedService_OnResponse encountered an error");
            }
        }
        #endregion

        #region Compliance Permission
        /// <summary>
        /// Check if CompanyUserID has Pre Trade Permission for Live Orders
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetPreTradeCheckForLiveOrders(int userId)
        {
            try
            {
                if (_preTradeModule.ContainsKey(userId) && _compliancePermissions.ContainsKey(userId))
                    return _compliancePermissions[userId].RuleCheckPermission.IsPreTradeEnabled && _compliancePermissions[userId].RuleCheckPermission.IsTrading;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetPreTradeCheckForLiveOrders encountered an error for user:" + userId);
                return false;
            }
        }
        #endregion 

        #region IServiceStatus Methods
        public async System.Threading.Tasks.Task<bool?> Subscribe(string subscriberName, bool isRetryRequest)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                if (ServicesHeartbeatSubscribersCollection.GetInstance().AddSubscriber(subscriberName, OperationContext.Current.GetCallbackChannel<IServiceStatusCallback>()))
                {
                    // Subscriber added successfully
                    return true;
                }
                else if (_LiveFeedServiceHeartbeatManager != null && !isRetryRequest)
                {
                    // Subscribe failed
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

            return null;
        }

        public async System.Threading.Tasks.Task UnSubscribe(string subscriberName)
        {
            try
            {
                if (ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(subscriberName))
                {
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
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

        #region IContainerService Methods
        public async System.Threading.Tasks.Task RequestStartupData()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<byte[]> OpenLog()
        {
            try
            {
                byte[] buffer = new byte[0];
                string strFilePath = Logger.GetFlatFilelistnerLogFileName(LoggingConstants.LISTENER_RollingFlatFile_Error_Message_Logging);
                if (File.Exists(strFilePath))
                {
                    using (FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        buffer = System.Text.Encoding.Unicode.GetBytes(await new StreamReader(fs).ReadToEndAsync());
                    }
                }
                return buffer;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<byte[]> LoadLog()
        {
            try
            {
                byte[] buffer = new byte[0];
                string strFilePath = Logger.GetFlatFilelistnerLogFileName(LoggingConstants.LISTENER_RollingInformationReporterTraceListener);
                if (File.Exists(strFilePath))
                {
                    using (FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        buffer = System.Text.Encoding.Unicode.GetBytes(await new StreamReader(fs).ReadToEndAsync());
                    }
                }
                return buffer;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task StopService()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                CleanUp();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus()
        {
            try
            {
                List<HostedService> hostedServicesStatus = new List<HostedService>();

                //IPublishing publishingObject = new Prana.PubSubService.Publishing();
                //ILiveFeedCallback liveFeedConnectionStatusObject = new LiveFeedConnectionStatus();

                var taskList = new List<System.Threading.Tasks.Task>()
                {
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", liveFeedConnectionStatusObject);
                    //            hostedServicesStatus.Add(new HostedService("PricingService", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("PricingService", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", publishingObject);
                    //            hostedServicesStatus.Add(new HostedService("PricingSubscription", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("PricingSubscription", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
                    //            hostedServicesStatus.Add(new HostedService("TradePublishing", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("TradePublishing", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", publishingObject);
                    //            hostedServicesStatus.Add(new HostedService("TradeSubscription", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("TradeSubscription", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
                    //            hostedServicesStatus.Add(new HostedService("TradePositionService", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("TradePositionService", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new ProxyBase<IAllocationServices>("TradeAllocationServiceEndpointAddress");
                    //            hostedServicesStatus.Add(new HostedService("TradeAllocationService", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("TradeAllocationService", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new ProxyBase<IAuditTrailService>("TradeAuditTrailServiceEndpointAddress");
                    //            hostedServicesStatus.Add(new HostedService("TradeAuditTrailService", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("TradeAuditTrailService", false));
                    //        }
                    //    }),
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new ProxyBase<IExpnlCalculationService>("ExpnlCalculationServiceEndpointAddress");
                    //            hostedServicesStatus.Add(new HostedService("ExpnlCalculationService", await proxy.InnerChannel.HealthCheck()));
                    //        }
                    //        catch
                    //        {
                    //            hostedServicesStatus.Add(new HostedService("ExpnlCalculationService", false));
                    //        }
                    //    })
                };

                await System.Threading.Tasks.Task.WhenAll(taskList);

                return hostedServicesStatus;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> GetDebugModeStatus()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_LiveFeedServiceHeartbeatManager != null)
                        _LiveFeedServiceHeartbeatManager.Dispose();
                    //if (_timer != null)
                    //{
                    //    _timer.Dispose();
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }
        #endregion

        #region Kafka reporter
        private void Kafka_ProducerReporter(string topic)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_PRODUCE, topic);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void Kafka_ConsumerReporter(string topic)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion
    }
}
