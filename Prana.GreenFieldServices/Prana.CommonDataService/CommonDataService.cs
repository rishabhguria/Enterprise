using Castle.Windsor;
using Prana.BusinessObjects;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global.Utilities;
using Prana.KafkaWrapper;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Interfaces;
using Prana.BusinessLogic;
using System.Threading;
using Prana.BusinessObjects.Compliance.Constants;
using System.Configuration;
using Prana.Authentication.Common;
using System.Linq;
using Prana.TradeManager.Extension;
using System.Xml;
using Prana.KafkaWrapper.Extension.Classes;
using System.Data;
using Prana.PubSubService.Interfaces;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.GreenfieldServices.Common;
using Newtonsoft.Json;

namespace Prana.CommonDataService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class CommonDataService : BaseService, ICommonDataService, IDisposable, IPublishing
    {
        #region Variables
        private IWindsorContainer _container;

        private ServerHeartbeatManager _CommonDataServiceHeartbeatManager;

        /// <summary>
        /// Logged in users details.
        /// </summary>
        private Dictionary<int, CompanyUser> _loggedInUsersCachedData = new Dictionary<int, CompanyUser>();

        /// <summary>
        /// Logged in users data cache.
        /// </summary>
        private Dictionary<int, UserCachedData> _userWiseCachedData = new Dictionary<int, UserCachedData>();

        /// <summary>
        /// Logged in users TT preferences.
        /// </summary>
        private Dictionary<int, TradingTicketPrefManager> _userWiseTTPreferences = new Dictionary<int, TradingTicketPrefManager>();

        /// <summary>
        /// Logged in users wise Client preferences.
        /// </summary>
        private Dictionary<int, TradingPreference> _userWiseTradingPrefs = new Dictionary<int, TradingPreference>();

        /// <summary>
        /// Logged in users wise XML preferences.
        /// </summary>
        private Dictionary<int, Dictionary<string, string>> _userWiseXMLpreferences = new Dictionary<int, Dictionary<string, string>>();

        /// <summary>
        /// Logged in users wise TT collection Data.
        /// </summary>
        private Dictionary<int, string> _userWiseTTCollectionData = new Dictionary<int, string>();

        /// <summary>
        /// To determine if Auth Service connected or not
        /// </summary>
        private static bool _isAuthServiceConnected = false;

        /// <summary>
        ///WPF connection for Allocation Manager
        /// </summary>
        ProxyBase<IAllocationManager> _allocationProxy = null;

        /// <summary>
        /// XML preferences required for TT
        /// </summary>
        string requestedPrefernces = "QuickTTPrefs.dat" + "~" + "TTGeneralPrefs.xml" + "~" + "CounterPartyWiseCommissionBasis.xml" + "~" + "PTTPreferences.xml";

        /// <summary>
        /// Key wise TT data collection
        /// </summary>
        private Dictionary<string, string> _tradingTicketData = new Dictionary<string, string>();

        /// <summary>
        /// Instance of OpenfinDataManagerService
        /// </summary>
        private static OpenfinDataManagerService _openfinDataManagerService;

        /// <summary>
        /// companyBlotterPref
        /// </summary>
        private BlotterClearanceCommonData companyBlotterPref = null;

        /// <summary>
        /// _tradeproxy to store tradeproxy of trade server connection
        /// </summary>
        private DuplexProxyBase<ISubscription> _tradeproxy;

        /// <summary>
        /// Hold the active Account count of the application.
        /// </summary>
        private int _totalActiveAccountsCount = 0;

        /// <summary>
        /// Tracks whether the logged-in user response has been received from Auth Service
        /// True if received; otherwise, false.
        /// </summary>
        bool _loggedInUserResponseReceivedFromAuth = false;

        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;

        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

        private int _cleanedUp = 0;

        private const string CONST_CACHE_NOT_YET_CREATED = "Cache was not created at the time of kafka request {0} for user: {1}";
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
                UpdateServiceStatus(ServiceNameConstants.CONST_CommonData_Name, ServiceNameConstants.CONST_CommonData_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_CommonData_Name);
                var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);

                Logger.LogMsg(LoggerLevel.Verbose, "Service status published to Kafka topic {0}", KafkaConstants.TOPIC_ServiceHealthStatus);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceServiceStatusMessage encountered an error");
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
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// TradeServiceClientHeartbeatManager_ConnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Trade_Name, ServiceNameConstants.CONST_Trade_DisplayName, true);
                // To Create and initialize the subscription service proxy for trade service.
                CreateSubscriptionServicesProxyTradeService();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// TradeServiceClientHeartbeatManager_DisconnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Trade_Name, ServiceNameConstants.CONST_Trade_DisplayName, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
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
                _tradeServiceClientHeartbeatManager = new ClientHeartbeatManager<ITradeService>(EndPointAddressConstants.CONST_TradeServiceEndpoint);
                _tradeServiceClientHeartbeatManager.ConnectedEvent += TradeServiceClientHeartbeatManager_ConnectedEvent;
                _tradeServiceClientHeartbeatManager.DisconnectedEvent += TradeServiceClientHeartbeatManager_DisconnectedEvent;
                _tradeServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;
                #endregion

                // To Create and initialize the subscription service proxy for trade service.
                CreateSubscriptionServicesProxyTradeService();

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                WindsorContainerManager.Container = container;

                _totalActiveAccountsCount = WindsorContainerManager.GetAccounts().Count;

                if (companyBlotterPref == null)
                    companyBlotterPref = DBTradeManager.GetInstance().GetCompanyClearanceCommonData(CachedDataManager.GetInstance.GetCompanyID());

                #region Server Heartbeat Setup
                _CommonDataServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                #region Kafka-Subscription
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_EquityOptionManualValidationRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWisePermittedAUECCVRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseLoggedInInformationRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_PreferenceDataRequest, KafkaManager_SendRequestedPreferencesMessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_AllocationPreferencesDetailRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingPreferencesRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_TradingTicketUIPrefsRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseBrokerDataRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseOrderSideDataRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseTIFDataRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseAllocationDataRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserDataForBlotterRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingTicketRequest, KafkaManager_CompanyTradingTicketRequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_InitializeLoggedInUsers);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_InitializeLoggedOutUsers);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompliancePermissionsRequest, KafkaManager_CompliancePermissionsRequest);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserPermittedAccountsRequest, KafkaManager_RequestReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_ForcefulLogoutWeb, ForcefulLogout);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetTradeAttributeLabels, KafkaManager_GetTradeAttributeLabels);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsRequest, KafkaManager_RtpnlTradeAttributeLabelsRequest);

                // fire and forget method for handling of auth service connection
                ServiceConnectionPoller.PollUntilServiceReady(() => _loggedInUserResponseReceivedFromAuth, KafkaConstants.TOPIC_InitializeLoggedInUserRequest, ServiceNameConstants.CONST_Auth_DisplayName, false);

                //Loading permissions for compliance
                ProduceCompliancePermissionsResponse();

                _openfinDataManagerService = OpenfinDataManagerService.GetInstance();
                //Loading all the saved workspaces as per user ID 
                //OpenfinDataManagerService.CreateUserWiseWorkspaceInformation();
                #endregion

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");

                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                    "InitialiseService Completed for CommonData Service in {0} ms.", sw.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitialiseService encountered an error");
            }

            return false;
        }

        /// <summary>
        /// Clean up Common Data Service
        /// </summary>
        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.CleanUp
            if (Interlocked.Exchange(ref _cleanedUp, 1) == 1) return;

            // 1) Stop periodic callbacks BEFORE disposing anything they use
            StopServiceHealthPollingTimer();

            // 2) Mark down (and publish a final “down”)
            UpdateServiceStatus(ServiceNameConstants.CONST_CommonData_Name, ServiceNameConstants.CONST_CommonData_DisplayName, false);
            Console.WriteLine("Shutting down Service...");
            Logger.LogMsg(LoggerLevel.Information, "Shutting down Service...");
            _container.Dispose();
        }

        #endregion

        #region IPublishing Members

        /// <summary>
        /// Gets the unique name of the receiver.
        /// Currently not implemented.
        /// </summary>
        public string getReceiverUniqueName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates and initializes the subscription service proxy for trade service.
        /// </summary>
        private void CreateSubscriptionServicesProxyTradeService()
        {
            try
            {
                if (_tradeproxy == null)
                    _tradeproxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);

                _tradeproxy.Subscribe(Topics.Topic_AllocationPreferenceUpdated, null);
                _tradeproxy.Subscribe(Topics.Topic_AllocationSchemeUpdated, null);
                Logger.LoggerWrite("ProxyTrade is initialized");
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
        /// Publishes the given message to the specified topic.
        /// </summary>
        /// <param name="e">The message data to publish.</param>
        /// <param name="topicName">The topic name to publish the message to.</param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                object[] dataList = (object[])e.EventData;
                switch (e.TopicName)
                {
                    case Topics.Topic_AllocationPreferenceUpdated:
                        CachedDataManager.UpdateAttributeLabels();
                        UpdateAllocationPreferenceInTTCache();
                        break;
                    case Topics.Topic_AllocationSchemeUpdated:
                        UpdateAllocationPreferenceInTTCache();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Publish encountered an error"); ;
            }
        }

        #endregion

        #region Compliance Permission
        /// <summary>
        /// KafkaManager_CompliancePermissionsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="requestResponseModel"></param>
        private void KafkaManager_CompliancePermissionsRequest(string topic, RequestResponseModel requestResponseModel)
        {
            using (LoggerHelper.PushLoggingProperties(requestResponseModel.CorrelationId, requestResponseModel.RequestID, requestResponseModel.CompanyUserID))
            {
                try
                {
                    ProduceCompliancePermissionsResponse(requestResponseModel.CorrelationId);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CompliancePermissionsRequest encountered an error");
                }
            }
        }

        /// <summary>
        /// Publish Compliance permissions data
        /// </summary>
        private async void ProduceCompliancePermissionsResponse(string correlationId = "")
        {
            try
            {
                ComplianceCachedData complianceCachedData = ComplianceCachedData.GetInstance();
                Dictionary<string, string> responseDictionary = new Dictionary<string, string>
                {
                    { ComplianceConstants.CONST_PRE_TRADE_COMPANY, JsonHelper.SerializeObject(complianceCachedData.PreEnabled) },
                    { ComplianceConstants.CONST_POST_TRADE_COMPANY, JsonHelper.SerializeObject(complianceCachedData.PostEnabled) },
                    { ComplianceConstants.CONST_PRE_TRADE_USERS, JsonHelper.SerializeObject(complianceCachedData.PreTradeModule) },
                    { ComplianceConstants.CONST_POST_TRADE_USERS, JsonHelper.SerializeObject(complianceCachedData.PostTradeModule) },
                    { ComplianceConstants.CONST_COMPLIANCE_PERMISSIONS, JsonHelper.SerializeObject(complianceCachedData.Permissions) }
                };
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompliancePermissionsResponse, new RequestResponseModel(0, JsonHelper.SerializeObject(responseDictionary)));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                var errModel = new RequestResponseModel(0, null, correlationId);
                await ProduceTopicNHandleException(errModel, ex, KafkaConstants.TOPIC_CompliancePermissionsResponse);
            }
        }
        #endregion

        #region Maintaining Login cache
        /// <summary>
        /// KafkaManager_InitializeLoggedInUsers
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_InitializeLoggedInUsers(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    if (!_loggedInUserResponseReceivedFromAuth)
                    {
                        _loggedInUserResponseReceivedFromAuth = true;
                        Logger.LogMsg(LoggerLevel.Information, "Successfully connected Auth Service.");
                    }

                    var sw = Stopwatch.StartNew();
                    Dictionary<int, AuthenticatedUserInfo> _loggedInUsers = JsonHelper.DeserializeToObject<Dictionary<int, AuthenticatedUserInfo>>(request.Data);
                    foreach (var kvp in _loggedInUsers)
                    {
                        int companyUserID = kvp.Key;
                        if (_loggedInUsers[kvp.Key] != null)
                        {
                            #region Adding logged in userDetails in cache
                            if (!_loggedInUsersCachedData.ContainsKey(companyUserID) && _loggedInUsers[companyUserID].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                            {
                                // in case of forcefull logout, this cache is not removed & remain stales.
                                // So we need to make sure to clear it first if exists
                                ClearUserCache(companyUserID);

                                Logger.LogMsg(LoggerLevel.Information, "Cache initialized for user:{0}", companyUserID);
                                CompanyUser loginUser = kvp.Value.CompanyUser;

                                if (loginUser != null)
                                    _loggedInUsersCachedData.Add(companyUserID, loginUser);

                                if (!_userWiseTradingPrefs.ContainsKey(companyUserID))
                                {
                                    CachedDataManager.GetInstance.SetCompanyUser(loginUser);
                                    // This block is added for handling the case of login and cache creation for web application
                                    if (!_userWiseCachedData.ContainsKey(companyUserID))
                                    {
                                        UserCachedData userCachedData = CachedDataManager.GetInstance.UserCachedData;
                                        if (userCachedData != null)
                                        {
                                            foreach (CounterParty counterparty in userCachedData.UserCounterParties.Cast<CounterParty>().ToList())
                                            {
                                                if (counterparty.IsOTDorEMS)
                                                {
                                                    userCachedData.UserCounterParties.Remove(counterparty);
                                                }
                                            }
                                            _userWiseCachedData.Add(companyUserID, userCachedData);
                                        }
                                    }

                                    if (_userWiseCachedData.ContainsKey(companyUserID))
                                    {
                                        Dictionary<int, string> userPermittedAccountsList = _userWiseCachedData[companyUserID].GetUserAccountsAsDict();
                                        List<int> userPermittedMasterFundsBasedOnFunds = _userWiseCachedData[companyUserID].UserPermittedMasterFundsBasedOnFunds;
                                        bool isUnallocatedAccountPermitted = _totalActiveAccountsCount == userPermittedAccountsList.Count;

                                        Dictionary<string, string> permittedFundsAndMasterFunds = new Dictionary<string, string>();
                                        permittedFundsAndMasterFunds.Add("permittedAccounts", JsonHelper.SerializeObject(userPermittedAccountsList));
                                        permittedFundsAndMasterFunds.Add("permittedMasterFunds", JsonHelper.SerializeObject(userPermittedMasterFundsBasedOnFunds));
                                        permittedFundsAndMasterFunds.Add("isUnallocatedAccountPermitted", isUnallocatedAccountPermitted.ToString());
                                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_PermittedMasterFundsBasedOnFundsResponse, new RequestResponseModel(companyUserID, JsonHelper.SerializeObject(permittedFundsAndMasterFunds)));
                                        Logger.LogMsg(LoggerLevel.Information, "Sent user permitted accounts and master funds to service gateway for user {0}", companyUserID);
                                    }

                                    TradingPreference _tradingPreference = new TradingPreference();
                                    _tradingPreference.SetClientCache(loginUser);
                                    _userWiseTradingPrefs.Add(companyUserID, _tradingPreference);
                                }

                                if (!_userWiseXMLpreferences.ContainsKey(companyUserID))
                                {
                                    Dictionary<string, string> seralizedData = new Dictionary<string, string>();
                                    Dictionary<string, byte[]> fileData = FileAndDbSyncManager.GetRequestedPreference(companyUserID, requestedPrefernces);
                                    foreach (string PreferenceName in fileData.Keys)
                                    {
                                        // This is done for easy binding and utilisation of this data on the frontend, as the xml is 
                                        // nested root and difficult to managed in frontent through simple plain JSON serialization (17188)
                                        if (PreferenceName == CommonDataConstants.COUNTER_PARTY_COMMISISON_BASIS_FILENAME)
                                        {
                                            var serData = JsonHelper.DeserializeAndSerializeToJson<CounterPartyWiseCommissionBasis>(fileData[PreferenceName]);
                                            seralizedData.Add(PreferenceName, serData);
                                        }
                                        else
                                        {
                                            using (StreamReader sr = new StreamReader(new MemoryStream(fileData[PreferenceName])))
                                            {
                                                var serializedData = JsonHelper.SerializeObject(sr.ReadToEnd());
                                                seralizedData.Add(PreferenceName, serializedData);
                                            }
                                        }
                                    }
                                    _userWiseXMLpreferences.Add(companyUserID, seralizedData);
                                }
                                if (!_userWiseTTPreferences.ContainsKey(loginUser.CompanyUserID))
                                {
                                    updateDictionaryForLoggedInUser(companyUserID);
                                }


                            }
                            // In the case when only service gateway is restarted
                            else
                            {
                                if (_userWiseTTPreferences.ContainsKey(companyUserID))
                                    KafkaManager_ProduceTradingTicketData(companyUserID);
                            }
                            #endregion
                        }

                        #region Create UserPermittedAccountsResponse
                        if (_userWiseCachedData.ContainsKey(companyUserID))
                        {
                            Dictionary<int, string> userPermittedAccountsList = _userWiseCachedData[companyUserID].GetUserAccountsAsDict();
                            RequestResponseModel message = new RequestResponseModel(companyUserID, JsonHelper.SerializeObject(userPermittedAccountsList));
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserPermittedAccountsResponse, message);
                        }
                        #endregion
                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LoggedInUserResponse, new RequestResponseModel(companyUserID, string.Empty));
                        // Awaiting for a completed task to make function asynchronous
                        await System.Threading.Tasks.Task.CompletedTask;
                    }
                    if (_loggedInUsers.Count > 0)
                        Logger.LogMsg(LoggerLevel.Debug, "InitializeLoggedInUsers event & user caches created successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "InitializeLoggedInUsers encountered an error");
                }
            }
        }

        /// <summary>
        /// KafkaManager_InitializeLoggedOutUsers
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_InitializeLoggedOutUsers(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    int companyUserID = message.CompanyUserID;

                    InformationReporter.GetInstance.Write(CommonDataConstants.MSG_CacheClearRequestReceived + companyUserID.ToString());

                    #region Clear cache for logged out user
                    ClearUserCache(companyUserID);
                    #endregion

                    Logger.LogMsg(LoggerLevel.Debug, "InitializeLoggedOutUsers event processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "InitializeLoggedOutUsers encountered an error");
                }
            }
        }

        private void ForcefulLogout(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserId = message.CompanyUserID;
                if (companyUserId > 0)
                {
                    ClearUserCache(companyUserId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Forceful logout User Cache Clearing encountered an error");
            }
        }

        private void ClearUserCache(int companyUserID)
        {
            try
            {
                if (_loggedInUsersCachedData.ContainsKey(companyUserID))
                {
                    _loggedInUsersCachedData[companyUserID] = null;
                    _loggedInUsersCachedData.Remove(companyUserID);
                }
                if (_userWiseCachedData.ContainsKey(companyUserID))
                {
                    _userWiseCachedData[companyUserID].Dispose();
                    _userWiseCachedData.Remove(companyUserID);
                }
                if (_userWiseTradingPrefs.ContainsKey(companyUserID))
                {
                    _userWiseTradingPrefs[companyUserID] = null;
                    _userWiseTradingPrefs.Remove(companyUserID);
                }
                if (_userWiseTTPreferences.ContainsKey(companyUserID))
                {
                    _userWiseTTPreferences[companyUserID].Dispose();
                    _userWiseTTPreferences.Remove(companyUserID);
                }
                if (_userWiseXMLpreferences.ContainsKey(companyUserID))
                {
                    _userWiseXMLpreferences[companyUserID].Clear();
                    _userWiseXMLpreferences.Remove(companyUserID);
                }
                if (_userWiseTTCollectionData.ContainsKey(companyUserID))
                {
                    _userWiseTTCollectionData[companyUserID] = null;
                    _userWiseTTCollectionData.Remove(companyUserID);
                }

                Logger.LogMsg(LoggerLevel.Information, "User {a} cache has been Cleared ", companyUserID);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ClearUserCache encountered an error");
            }
        }
        #endregion

        #region Creation and Maintaining TT preferences cache
        /// <summary>
        /// TT XML preferences cache creation
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SendRequestedPreferencesMessageReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    InformationReporter.GetInstance.Write(CommonDataConstants.MSG_GetTradingTicketXMLPreferencesReceived + message.CompanyUserID);
                    lock (_tradingTicketData)
                    {
                        if (_userWiseXMLpreferences.ContainsKey(message.CompanyUserID))
                        {
                            if (!_tradingTicketData.ContainsKey("PreferencesData"))
                            {
                                InformationReporter.GetInstance.Write(CommonDataConstants.MSG_GetTradingTicketXMLPreferencesProcessed + message.CompanyUserID);
                                _tradingTicketData.Add("PreferencesData", JsonHelper.SerializeObject(_userWiseXMLpreferences[message.CompanyUserID]));
                            }
                        }
                        else
                        {
                            InformationReporter.GetInstance.Write(CommonDataConstants.MSG_ProblemInTTXMLPreferencesCacheCreation + message.CompanyUserID);
                            Dictionary<string, string> seralizedData = new Dictionary<string, string>();
                            Dictionary<string, byte[]> data = FileAndDbSyncManager.GetRequestedPreference(message.CompanyUserID, message.Data);
                            foreach (string PreferenceName in data.Keys)
                            {
                                // This is done for easy binding and utilisation of this data on the frontend, as the xml is 
                                // nested root and difficult to managed in frontent through simple plain JSON serialization (17188)
                                if (PreferenceName == CommonDataConstants.COUNTER_PARTY_COMMISISON_BASIS_FILENAME)
                                {
                                    var serData = JsonHelper.DeserializeAndSerializeToJson<CounterPartyWiseCommissionBasis>(data[PreferenceName]);
                                    seralizedData.Add(PreferenceName, serData);
                                }
                                else
                                {
                                    using (StreamReader sr = new StreamReader(new MemoryStream(data[PreferenceName])))
                                    {
                                        var serializedData = JsonHelper.SerializeObject(sr.ReadToEnd());
                                        seralizedData.Add(PreferenceName, serializedData);
                                    }
                                }
                            }
                            if (!_tradingTicketData.ContainsKey("PreferencesData"))
                            {
                                _tradingTicketData.Add("PreferencesData", JsonHelper.SerializeObject(seralizedData));
                            }
                        }
                    }
                    Logger.LogMsg(LoggerLevel.Debug, "SendRequestedPreferencesMessageReceived (TT xml preference) event processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SendRequestedPreferencesMessageReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Fetching all TT preferences from Cache request
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_CompanyTradingTicketRequestReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    if (_userWiseTTCollectionData != null)
                    {
                        if (_userWiseTTCollectionData.ContainsKey(request.CompanyUserID))
                        {
                            var result = new { data = _userWiseTTCollectionData[request.CompanyUserID], tradingTicketId = request.Data };
                            request.Data = JsonHelper.SerializeObject(result);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompanyTradingTicketResponse, request);
                            InformationReporter.GetInstance.Write(CommonDataConstants.MSG_GetTradingTicketDataProcessed + request.CompanyUserID);

                            KafkaManager_ProduceTradingTicketData(request.CompanyUserID);
                            Logger.LogMsg(LoggerLevel.Debug, "CompanyTradingTicketRequestReceived (Get all trading ticket req) event processed successfully in {0} ms", sw.ElapsedMilliseconds);
                        }
                        else
                        {
                            Logger.LogMsg(LoggerLevel.Fatal, "Problem in TT Cache Creations, Retrying to create new cache.....");
                            UpdateDictionaryForLoggedInUser(request); // This method will update the dictionary for currently looged in user 
                        }
                    }

                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(request, ex, KafkaConstants.TOPIC_CompanyTradingTicketResponse);
                }
            }
        }

        /// <summary>
        /// Creation of TT preferences cache during login
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="prefManager"></param>
        void CreateTradingTicketData(string topic, RequestResponseModel message, TradingTicketPrefManager prefManager)
        {
            try
            {
                lock (_tradingTicketData)
                {
                    switch (topic)
                    {
                        case KafkaConstants.TOPIC_UserWiseAllocationDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.Accounts);
                            if (!_tradingTicketData.ContainsKey(CommonDataConstants.CONST_USER_ALLOCATION_DATA))
                            {
                                _tradingTicketData.Add(CommonDataConstants.CONST_USER_ALLOCATION_DATA, message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_UserWiseOrderSideDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.AssetSides);
                            if (!_tradingTicketData.ContainsKey("UserOrderSide"))
                            {
                                _tradingTicketData.Add("UserOrderSide", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.OrderTypes);
                            if (!_tradingTicketData.ContainsKey("UserOrderType"))
                            {
                                _tradingTicketData.Add("UserOrderType", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_UserWiseTIFDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.TimeInForces);
                            if (!_tradingTicketData.ContainsKey("UserTIF"))
                            {
                                _tradingTicketData.Add("UserTIF", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_TradingTicketUIPrefsRequest:
                            TradingTicketUIPrefs _tradingTicketUIPrefs = SetTicketPreferences(prefManager.TradingTicketUiPrefs, prefManager.CompanyTradingTicketUiPrefs);
                            message.Data = JsonHelper.SerializeObject(_tradingTicketUIPrefs);
                            if (!_tradingTicketData.ContainsKey("TradingTicketUIPrefs"))
                            {
                                _tradingTicketData.Add("TradingTicketUIPrefs", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_CompanyTradingPreferencesRequest:
                            if (_userWiseTradingPrefs.ContainsKey(message.CompanyUserID))
                            {
                                message.Data = JsonHelper.SerializeObject(_userWiseTradingPrefs[message.CompanyUserID]);
                            }
                            else
                            {
                                message.Data = String.Empty;
                            }
                            if (!_tradingTicketData.ContainsKey("UserPreferencesData"))
                            {
                                _tradingTicketData.Add("UserPreferencesData", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_AllocationPreferencesDetailRequest:
                            if (_allocationProxy == null)
                            {
                                _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                            }
                            Dictionary<int, AllocationOperationPreference> allocationPreferencesDetails = new Dictionary<int, AllocationOperationPreference>();
                            foreach (Account account in prefManager.Accounts)
                            {
                                var operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(account.AccountID);
                                if (operationPreference != null)
                                {
                                    if (!allocationPreferencesDetails.ContainsKey(account.AccountID))
                                        allocationPreferencesDetails.Add(account.AccountID, operationPreference);
                                }
                            }
                            message.Data = JsonHelper.SerializeObject(allocationPreferencesDetails);
                            if (!_tradingTicketData.ContainsKey(CommonDataConstants.CONST_ALLOCATION_PREFERENCE_DATA))
                            {
                                _tradingTicketData.Add(CommonDataConstants.CONST_ALLOCATION_PREFERENCE_DATA, message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest:
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                message.Data = JsonHelper.SerializeObject(_userWiseCachedData[message.CompanyUserID]);
                            }
                            else
                            {
                                message.Data = String.Empty;
                            }
                            if (!_tradingTicketData.ContainsKey("UserTransferTradeRules"))
                            {
                                _tradingTicketData.Add("UserTransferTradeRules", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_CompliancePreTradePermissionCheckRequest:
                            if (_loggedInUsersCachedData.ContainsKey(message.CompanyUserID))
                            {
                                message.Data = ComplianceCacheManager.GetPreTradeCheck(message.CompanyUserID).ToString();
                            }
                            else
                            {
                                message.Data = false.ToString();
                            }
                            if (!_tradingTicketData.ContainsKey("CompliancePreTradePermission"))
                            {
                                _tradingTicketData.Add("CompliancePreTradePermission", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_CompliancePreTradeStagingPermissionCheckRequest:
                            if (_loggedInUsersCachedData.ContainsKey(message.CompanyUserID))
                            {
                                message.Data = ComplianceCacheManager.GetPreTradeCheckStaging(message.CompanyUserID).ToString();
                            }
                            else
                            {
                                message.Data = false.ToString();
                            }
                            if (!_tradingTicketData.ContainsKey("CompliancePreTradeStagingPermission"))
                            {
                                _tradingTicketData.Add("CompliancePreTradeStagingPermission", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_CompliancePreTradeManualPermissionCheckRequest:
                            if (_loggedInUsersCachedData.ContainsKey(message.CompanyUserID))
                            {
                                message.Data = (ComplianceCacheManager.GetPreTradeModuleEnabledForUser(message.CompanyUserID) && ComplianceCacheManager.GetApplyToManualPermission(message.CompanyUserID)).ToString();
                            }
                            else
                            {
                                message.Data = false.ToString();
                            }
                            if (!_tradingTicketData.ContainsKey("CompliancePreTradeManualPermission"))
                            {
                                _tradingTicketData.Add("CompliancePreTradeManualPermission", message.Data);
                            }
                            break;
                        case KafkaConstants.TOPIC_PreferenceDataRequest:
                            message.Data = requestedPrefernces;
                            KafkaManager_SendRequestedPreferencesMessageReceived(KafkaConstants.TOPIC_PreferenceDataRequest, message);
                            break;
                        case CommonDataConstants.CONST_ShortLocate_BorrowBroker_Information:
                            Dictionary<int, string> borrowBrokerData = CachedDataManager.GetInstance.GetAllThirdPartiesWithShortName();
                            if (!_tradingTicketData.ContainsKey("borrowBrokerData"))
                            {
                                _tradingTicketData.Add("borrowBrokerData", JsonHelper.SerializeObject(borrowBrokerData));
                            }
                            break;
                        case KafkaConstants.TOPIC_UserPermittedAccountsRequest:
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                Dictionary<int, string> userPermittedAccountsList = _userWiseCachedData[message.CompanyUserID].GetUserAccountsAsDict();
                                message.Data = JsonHelper.SerializeObject(userPermittedAccountsList);
                            }
                            else
                            {
                                message.Data = String.Empty;
                            }
                            if (!_tradingTicketData.ContainsKey("PermittedAccountsList"))
                            {
                                _tradingTicketData.Add("PermittedAccountsList", message.Data);
                            }
                            break;
                        case CommonDataConstants.CONST_EXCHANGE_INFORMATION:
                            Dictionary<int, string> exchangesData = CachedDataManager.GetInstance.GetAllExchanges();
                            if (!_tradingTicketData.ContainsKey("ExchangesData"))
                            {
                                _tradingTicketData.Add("ExchangesData", JsonHelper.SerializeObject(exchangesData));
                            }
                            break;
                        case KafkaConstants.TOPIC_AccountWiseExecutingBrokerInfo:
                            string isCustodianPreferenceTrue = "";
                            if (_userWiseXMLpreferences.ContainsKey(message.CompanyUserID) && _userWiseXMLpreferences[message.CompanyUserID].ContainsKey(CommonDataConstants.CONST_TTGENERALPREF))
                            {
                                string tradingGeneralPreference = _userWiseXMLpreferences[message.CompanyUserID][CommonDataConstants.CONST_TTGENERALPREF];
                                tradingGeneralPreference = JsonHelper.DeserializeToObject<string>(tradingGeneralPreference).Trim();
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(tradingGeneralPreference);
                                XmlNode node = xmlDoc.SelectSingleNode("//IsUseCustodianAsExecutingBroker");
                                if (node != null)
                                {
                                    isCustodianPreferenceTrue = node.InnerText.ToLower();
                                }
                            }
                            if (isCustodianPreferenceTrue.Equals("true") && !_tradingTicketData.ContainsKey("accountWiseExecutingBrokers"))
                            {
                                Dictionary<int, int> accountWiseExecutingBrokers = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                                _tradingTicketData.Add("accountWiseExecutingBrokers", JsonHelper.SerializeObject(accountWiseExecutingBrokers));
                            }
                            break;
                        case CommonDataConstants.CONST_USER_WISE_STRATEGY_DATA:
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                StrategyCollection userWiseStrategies = _userWiseCachedData[message.CompanyUserID].UserStrategies;
                                if (userWiseStrategies.Contains(int.MinValue))
                                {
                                    userWiseStrategies.RemoveAt(userWiseStrategies.IndexOf(int.MinValue));
                                }
                                if (userWiseStrategies != null && userWiseStrategies.Count > 0)
                                {
                                    string strategies = JsonHelper.SerializeObject(userWiseStrategies);
                                    if (!_tradingTicketData.ContainsKey(CommonDataConstants.CONST_USER_WISE_STRATEGY_DATA))
                                    {
                                        _tradingTicketData.Add("UserStrategyData", strategies);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CreateTradingTicketData encountered an error");
            }
        }

        /// <summary>
        /// Fetching requested data from preferences for other greenfield services
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RequestReceived(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    TradingTicketPrefManager prefManager = null;
                    if (_userWiseTTPreferences.ContainsKey(message.CompanyUserID))
                        prefManager = _userWiseTTPreferences[message.CompanyUserID];
                    else
                    {
                        prefManager = TradingTicketPrefManager.GetInstanceForUser(message.CompanyUserID, CachedDataManager.GetInstance.GetCompanyID());
                    }

                    switch (topic)
                    {
                        case KafkaConstants.TOPIC_UserWiseAllocationDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.Accounts);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseAllocationDataResponse, message);
                            break;
                        case KafkaConstants.TOPIC_UserWiseBrokerDataRequest:
                            prefManager.Brokers.Sort();
                            message.Data = JsonHelper.SerializeObject(prefManager.Brokers);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseBrokerDataResponse, message);
                            break;
                        case KafkaConstants.TOPIC_UserWiseOrderSideDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.AssetSides);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseOrderSideDataResponse, message);
                            break;
                        case KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.OrderTypes);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseOrderTypeDataResponse, message);
                            break;
                        case KafkaConstants.TOPIC_UserWiseTIFDataRequest:
                            message.Data = JsonHelper.SerializeObject(prefManager.TimeInForces);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseTIFDataResponse, message);
                            break;
                        case KafkaConstants.TOPIC_TradingTicketUIPrefsRequest:
                            TradingTicketUIPrefs _tradingTicketUIPrefs = SetTicketPreferences(prefManager.TradingTicketUiPrefs, prefManager.CompanyTradingTicketUiPrefs);
                            message.Data = JsonHelper.SerializeObject(_tradingTicketUIPrefs);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_TradingTicketUIPrefsResponse, message);
                            break;
                        case KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest:
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                TranferTradeRules companyTransferTradeRules = _userWiseCachedData[message.CompanyUserID].TranferTradeRules;
                                Dictionary<string, string> updatedData = new Dictionary<string, string>
                                {
                                    { CommonDataConstants.CONST_COMPANY_TRANSFER_TRADE_RULES, JsonHelper.SerializeObject(companyTransferTradeRules) },
                                    { CommonDataConstants.CONST_ROLLOVER_PERMITTED_USERID, companyBlotterPref.RolloverPermittedUserID.ToString() }
                                };
                                message.Data = JsonHelper.SerializeObject(updatedData);
                                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompanyTransferTradeRulesResponse, message);
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, CONST_CACHE_NOT_YET_CREATED, topic, message.CompanyUserID);
                            }
                            break;
                        case KafkaConstants.TOPIC_UserWisePermittedAUECCVRequest:
                            List<string> PermittedAUECCV = null;
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                PermittedAUECCV = _userWiseCachedData[message.CompanyUserID].PermittedAUECCV;
                                message.Data = JsonHelper.SerializeObject(PermittedAUECCV);
                                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWisePermittedAUECCVResponse, message);
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, CONST_CACHE_NOT_YET_CREATED, topic, message.CompanyUserID);
                            }
                            break;
                        case KafkaConstants.TOPIC_UserWiseLoggedInInformationRequest:
                            CompanyUser loggedInInformartion = null;
                            if (_loggedInUsersCachedData.ContainsKey(message.CompanyUserID))
                            {
                                loggedInInformartion = _loggedInUsersCachedData[message.CompanyUserID];
                                message.Data = JsonHelper.SerializeObject(loggedInInformartion);
                                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseLoggedInInformationResponse, message);
                            }
                            break;
                        case KafkaConstants.TOPIC_AllocationPreferencesDetailRequest:
                            if (_allocationProxy == null)
                            {
                                _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                            }
                            Dictionary<int, AllocationOperationPreference> allocationPreferencesDetails = new Dictionary<int, AllocationOperationPreference>();
                            foreach (Account account in prefManager.Accounts)
                            {
                                var operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(account.AccountID);
                                if (operationPreference != null)
                                {
                                    if (!allocationPreferencesDetails.ContainsKey(account.AccountID))
                                        allocationPreferencesDetails.Add(account.AccountID, operationPreference);
                                }
                            }
                            message.Data = JsonHelper.SerializeObject(allocationPreferencesDetails);
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_AllocationPreferencesDetailResponse, message);
                            break;
                        case KafkaConstants.TOPIC_UserDataForBlotterRequest:
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                Dictionary<string, string> responseData = new Dictionary<string, string>();
                                TradingTicketUIPrefs tradingTicketUIPrefs = SetTicketPreferences(prefManager.TradingTicketUiPrefs, prefManager.CompanyTradingTicketUiPrefs);
                                responseData.Add("TradingTicketUIPreference", JsonHelper.SerializeObject(tradingTicketUIPrefs));
                                Dictionary<int, string> accountList = _userWiseCachedData[message.CompanyUserID].GetUserAccountsAsDict();
                                responseData.Add("AccountList", JsonHelper.SerializeObject(accountList));
                                message.Data = JsonHelper.SerializeObject(responseData);
                                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserDataForBlotterResponse, message);
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, CONST_CACHE_NOT_YET_CREATED, topic, message.CompanyUserID);
                            }
                            break;
                        case KafkaConstants.TOPIC_EquityOptionManualValidationRequest:
                            message.Data = CachedDataManager.GetInstance.IsEquityOptionManualValidation().ToString();
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_EquityOptionManualValidationResponse, message);
                            break;
                        case KafkaConstants.TOPIC_CompanyTradingPreferencesRequest:
                            if (_userWiseTradingPrefs.ContainsKey(message.CompanyUserID))
                            {
                                message.Data = JsonHelper.SerializeObject(_userWiseTradingPrefs[message.CompanyUserID]);
                                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompanyTradingPreferencesResponse, message);
                            }
                            break;
                        case KafkaConstants.TOPIC_UserPermittedAccountsRequest:
                            if (_userWiseCachedData.ContainsKey(message.CompanyUserID))
                            {
                                Dictionary<int, string> userPermittedAccountsList = _userWiseCachedData[message.CompanyUserID].GetUserAccountsAsDict();
                                message.Data = JsonHelper.SerializeObject(userPermittedAccountsList);
                                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserPermittedAccountsResponse, message);
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, CONST_CACHE_NOT_YET_CREATED, topic, message.CompanyUserID);
                            }
                            break;
                    }
                    Logger.LogMsg(LoggerLevel.Debug, "Message has been passed to topic {0} & KafkaManager_RequestReceived processed in{0} ms", topic, sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_RequestReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Create Service Gateway Data based on given user
        /// </summary>
        /// <param name="userID"></param>
        //private string CreateServiceGatewayData(int userID)
        //{
        //    string data = string.Empty;
        //    try
        //    {
        //        //TODO: Can add more details from user cache as per requirement.
        //        if (_userWiseCachedData.ContainsKey(userID))
        //        {
        //            List<int> userTradingAccounts = new List<int>();
        //            foreach (TradingAccount tradingAccount in _userWiseCachedData[userID].UserTradingAccounts)
        //            {
        //                userTradingAccounts.Add(tradingAccount.TradingAccountID);
        //            }
        //            Dictionary<string, string> finalData = new Dictionary<string, string>
        //                {
        //                    { "UserTradingAccounts", JsonHelper.SerializeObject(userTradingAccounts) },
        //                };
        //            data = JsonHelper.SerializeObject(finalData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return data;
        //}

        /// <summary>
        /// Creation of Trading Ticket UI preferences for TT dock
        /// </summary>
        /// <param name="userTradingTicketuserUiPrefs"></param>
        /// <param name="companyTradingTicketUiPrefs"></param>
        private TradingTicketUIPrefs SetTicketPreferences(TradingTicketUIPrefs userTradingTicketuserUiPrefs, TradingTicketUIPrefs companyTradingTicketUiPrefs)
        {
            TradingTicketUIPrefs _tradingTicketUIPrefs = new TradingTicketUIPrefs();
            try
            {
                if (userTradingTicketuserUiPrefs != null && companyTradingTicketUiPrefs != null)
                {
                    _tradingTicketUIPrefs.Account = userTradingTicketuserUiPrefs.Account.HasValue ? userTradingTicketuserUiPrefs.Account : companyTradingTicketUiPrefs.Account;
                    _tradingTicketUIPrefs.OrderType = userTradingTicketuserUiPrefs.OrderType.HasValue ? userTradingTicketuserUiPrefs.OrderType : companyTradingTicketUiPrefs.OrderType;
                    _tradingTicketUIPrefs.TimeInForce = userTradingTicketuserUiPrefs.TimeInForce.HasValue ? userTradingTicketuserUiPrefs.TimeInForce : companyTradingTicketUiPrefs.TimeInForce;
                    _tradingTicketUIPrefs.Broker = userTradingTicketuserUiPrefs.Broker.HasValue ? userTradingTicketuserUiPrefs.Broker : companyTradingTicketUiPrefs.Broker;
                    _tradingTicketUIPrefs.Quantity = userTradingTicketuserUiPrefs.Quantity.HasValue ? userTradingTicketuserUiPrefs.Quantity : companyTradingTicketUiPrefs.Quantity;
                    _tradingTicketUIPrefs.IsShowTargetQTY = userTradingTicketuserUiPrefs.IsShowTargetQTY.HasValue ? userTradingTicketuserUiPrefs.IsShowTargetQTY : companyTradingTicketUiPrefs.IsShowTargetQTY;
                    _tradingTicketUIPrefs.TradingAccount = userTradingTicketuserUiPrefs.TradingAccount.HasValue ? userTradingTicketuserUiPrefs.TradingAccount : companyTradingTicketUiPrefs.TradingAccount.HasValue ? companyTradingTicketUiPrefs.TradingAccount : -1;
                    _tradingTicketUIPrefs.ExecutionInstruction = userTradingTicketuserUiPrefs.ExecutionInstruction.HasValue ? userTradingTicketuserUiPrefs.ExecutionInstruction : companyTradingTicketUiPrefs.ExecutionInstruction.HasValue ? companyTradingTicketUiPrefs.ExecutionInstruction : -1;
                    _tradingTicketUIPrefs.HandlingInstruction = userTradingTicketuserUiPrefs.HandlingInstruction.HasValue ? userTradingTicketuserUiPrefs.HandlingInstruction : companyTradingTicketUiPrefs.HandlingInstruction.HasValue ? companyTradingTicketUiPrefs.HandlingInstruction : -1;
                    _tradingTicketUIPrefs.DefAssetSides = companyTradingTicketUiPrefs.DefAssetSides;
                    foreach (DefAssetSide defAssetSide in userTradingTicketuserUiPrefs.DefAssetSides)
                    {
                        if (defAssetSide.OrderSide != null)
                        {
                            int index = _tradingTicketUIPrefs.DefAssetSides.FindIndex(DefAssetSide => DefAssetSide.Asset == defAssetSide.Asset);
                            if (index == -1)
                            {
                                _tradingTicketUIPrefs.DefAssetSides.Add(defAssetSide);
                            }
                            else
                            {
                                _tradingTicketUIPrefs.DefAssetSides[index].OrderSide = defAssetSide.OrderSide;
                            }
                        }
                    }
                    _tradingTicketUIPrefs.Strategy = userTradingTicketuserUiPrefs.Strategy.HasValue ? userTradingTicketuserUiPrefs.Strategy : companyTradingTicketUiPrefs.Strategy;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SetTicketPreferences encountered an error");
            }
            return _tradingTicketUIPrefs;
        }

        /// <summary>
        /// Updating the dictionary in case of failed attempts
        /// </summary>
        /// <param name="request"></param>
        private void UpdateDictionaryForLoggedInUser(RequestResponseModel requestResponseModel)
        {
            try
            {
                updateDictionaryForLoggedInUser(requestResponseModel.CompanyUserID);

                if (_userWiseTTCollectionData.ContainsKey(requestResponseModel.CompanyUserID))
                {
                    KafkaManager_CompanyTradingTicketRequestReceived(KafkaConstants.TOPIC_CompanyTradingTicketRequest, requestResponseModel);
                }
                else
                {
                    Logger.LoggerWrite(CommonDataConstants.MSG_GetTradingTicketDataReceived + requestResponseModel.CompanyUserID + " " + JsonHelper.SerializeObject(_userWiseTTCollectionData));
                    InformationReporter.GetInstance.Write(CommonDataConstants.MSG_ProblemInTTCacheCreation + requestResponseModel.CompanyUserID);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UpdateDictionaryForLoggedInUser encountered an error");
            }
        }

        /// <summary>
        /// Updating the dictionary in case of failed attempts
        /// </summary>
        /// <param name="request"></param>
        private void updateDictionaryForLoggedInUser(int companyUserID)
        {
            try
            {
                string[] topics = { KafkaConstants.TOPIC_TradingTicketUIPrefsRequest, KafkaConstants.TOPIC_UserWiseAllocationDataRequest,
                                                KafkaConstants.TOPIC_UserWiseTIFDataRequest, KafkaConstants.TOPIC_UserWiseOrderSideDataRequest,
                                                KafkaConstants.TOPIC_UserWiseOrderTypeDataRequest,KafkaConstants.TOPIC_AllocationPreferencesDetailRequest,
                                                KafkaConstants.TOPIC_CompanyTradingPreferencesRequest, KafkaConstants.TOPIC_CompanyTransferTradeRulesRequest,
                                                KafkaConstants.TOPIC_CompliancePreTradePermissionCheckRequest,KafkaConstants.TOPIC_CompliancePreTradeStagingPermissionCheckRequest,
                                                KafkaConstants.TOPIC_PreferenceDataRequest,KafkaConstants.TOPIC_CompliancePreTradeManualPermissionCheckRequest,CommonDataConstants.CONST_ShortLocate_BorrowBroker_Information,
                                                KafkaConstants.TOPIC_UserPermittedAccountsRequest, CommonDataConstants.CONST_EXCHANGE_INFORMATION , KafkaConstants.TOPIC_AccountWiseExecutingBrokerInfo, CommonDataConstants.CONST_USER_WISE_STRATEGY_DATA,
                    };

                TradingTicketPrefManager prefManager = TradingTicketPrefManager.GetInstanceForUser(companyUserID, CachedDataManager.GetInstance.GetCompanyID());

                // Updating dictionary based on specific conditions
                if (prefManager != null)
                {
                    _tradingTicketData.Clear();
                    RequestResponseModel message = new RequestResponseModel(companyUserID, String.Empty);

                    InformationReporter.GetInstance.Write(CommonDataConstants.MSG_CreateTTpreferencesRequestReceived + companyUserID.ToString());
                    foreach (var topicData in topics)
                    {
                        CreateTradingTicketData(topicData, message, prefManager);
                    }

                    InformationReporter.GetInstance.Write(CommonDataConstants.MSG_CreateTTpreferencesRequestProcessed + companyUserID.ToString());

                    if (!_userWiseTTPreferences.ContainsKey(companyUserID))
                    {
                        _userWiseTTPreferences.Add(companyUserID, prefManager);
                    }
                    else
                    {
                        _userWiseTTPreferences[companyUserID] = prefManager;
                    }

                    if (!_userWiseTTCollectionData.ContainsKey(companyUserID))
                    {
                        if (!_tradingTicketData.ContainsKey(CommonDataConstants.MF_CUSTOM_GROUP_WT_ACCOUNT_DATA))
                        {
                            var mfAndCustomGrpDataStr = JsonHelper.SerializeObject(_userWiseCachedData[companyUserID].UsersMasterFundNCustomGrpWithAccounts);
                            _tradingTicketData.Add(CommonDataConstants.MF_CUSTOM_GROUP_WT_ACCOUNT_DATA, mfAndCustomGrpDataStr);
                        }
                        if (!_tradingTicketData.ContainsKey(CommonDataConstants.CONST_CALCULATED_PREFERENCES_OF_ALLOCATION))
                        {
                            //For Fetching calculatedPrefrences of Allocation.
                            var calculatedAccountPreferences = _allocationProxy.InnerChannel.GetCalculatedPreferencesByCompanyId(_loggedInUsersCachedData[companyUserID].CompanyID, companyUserID).ToList();

                            var filteredMasterFundPreferences = _allocationProxy.InnerChannel
                           .GetMasterFundPrefByCompanyId(_loggedInUsersCachedData[companyUserID].CompanyID, companyUserID)?.ToList();

                            var currentMfPreferences = GetCurrentPreferencesMasterFunds(filteredMasterFundPreferences, calculatedAccountPreferences);
                            bool hasMFPref = calculatedAccountPreferences?.Any(item => item?.OperationPreferenceName?.Contains(CommonDataConstants.CONST_MF_PREFERENCE_NAME) == true) ?? false;
                            var finalPreferences = (calculatedAccountPreferences?.Any(item =>
                             item?.OperationPreferenceName?.Contains(CommonDataConstants.CONST_MF_PREFERENCE_NAME) == true) ?? false)
                             ? (object)currentMfPreferences
                             : (object)calculatedAccountPreferences;


                            // Add the final preferences to the trading ticket data
                            _tradingTicketData.Add(CommonDataConstants.CONST_CALCULATED_PREFERENCES_OF_ALLOCATION, JsonHelper.SerializeObject(finalPreferences));

                            if (hasMFPref)
                            {
                                var AccountDetailsOfTheMasterFundPreferences = GetMasterFundPreferenceWithTargetPercentage(filteredMasterFundPreferences, calculatedAccountPreferences);
                                _tradingTicketData.Add(CommonDataConstants.CONST_ACCOUNT_DETAILS_FOR_MASTERFUND_PREFERENCES, JsonHelper.SerializeObject(AccountDetailsOfTheMasterFundPreferences));
                            }
                        }
                        if (!_tradingTicketData.ContainsKey(CommonDataConstants.CONST_COMPLIANCE_PERMISSION) && ComplianceCachedData.GetInstance() != null && ComplianceCachedData.GetInstance().Permissions != null && ComplianceCachedData.GetInstance().Permissions.Keys.Contains(companyUserID))
                        {
                            //can add other permission as common separated in this dyamic var.
                            var compliancePermisstion = new
                            {
                                EnableBasketComplianceCheck = ComplianceCachedData.GetInstance().Permissions[companyUserID].EnableBasketComplianceCheck
                            };
                            _tradingTicketData.Add(CommonDataConstants.CONST_COMPLIANCE_PERMISSION, JsonHelper.SerializeObject(compliancePermisstion));
                        }

                        _userWiseTTCollectionData.Add(companyUserID, JsonHelper.SerializeObject(_tradingTicketData));
                    }

                    KafkaManager_ProduceTradingTicketData(companyUserID);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "updateDictionaryForLoggedInUser encountered an error");
            }
        }

        /// <summary>
        /// Producing TT data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_ProduceTradingTicketData(int companyUserID)
        {
            try
            {
                if (_userWiseTTCollectionData != null || _userWiseTTCollectionData.Count != 0)
                {
                    if (_userWiseTTCollectionData.ContainsKey(companyUserID))
                    {
                        var userWiseTTData = new Dictionary<int, string> { { companyUserID, _userWiseTTCollectionData[companyUserID] } };
                        var requestResponseModel = new RequestResponseModel(companyUserID: userWiseTTData.Keys.First(), data: userWiseTTData.Values.First());
                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseTradingTicketDataResponse, requestResponseModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while producing trading ticket data");
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
                else if (_CommonDataServiceHeartbeatManager != null && !isRetryRequest)
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
                //ICommonDataCallback CommonDataConnectionStatusObject = new CommonDataConnectionStatus();

                var taskList = new List<System.Threading.Tasks.Task>()
                {
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", CommonDataConnectionStatusObject);
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
                    if (_CommonDataServiceHeartbeatManager != null)
                        _CommonDataServiceHeartbeatManager.Dispose();
                    if (_allocationProxy != null)
                        _allocationProxy.Dispose();
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
            Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_PRODUCE, topic);
        }

        private void Kafka_ConsumerReporter(string topic)
        {
            Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);
        }
        #endregion

        private static async System.Threading.Tasks.Task ProduceTopicNHandleException(
          RequestResponseModel message,
          Exception ex,
          string topicName)
        {
            try
            {
                message.Data = null;
                message.ErrorMsg = $"Error while producing to topic {topicName}, err msg:{ex.Message}";
                await KafkaManager.Instance.Produce(topicName, message);
                Logger.LogError(ex, $"Error while producing  to topic {topicName}");
            }
            catch (Exception ex2)
            {
                Logger.LogError(ex2, $"ProduceTopicNHandleException encountered an error,  message might not have been published to event {topicName}");
            }
        }

        /// <summary>
        /// Maps MasterFundPreferenceId to AccountValue lists based on matching TargetPercentages.
        /// </summary>
        public List<KeyValuePair<int, List<AccountValue>>> GetMasterFundPreferenceWithTargetPercentage(List<AllocationMasterFundPreference> calculatedMasterFundPreferencesofAllocation, List<AllocationOperationPreference> filteredPreferences)
        {
            var resultDictionary = new Dictionary<int, List<AccountValue>>();

            var preferenceLookup = filteredPreferences
                .Where(pref => pref?.TargetPercentage != null)
                .ToDictionary(pref => pref.OperationPreferenceId, pref => pref.TargetPercentage);

            // Iterate through each AllocationMasterFundPreference
            foreach (var allocation in calculatedMasterFundPreferencesofAllocation)
            {
                if (allocation?.MasterFundPreference is SerializableDictionary<int, int> masterFundPreference)
                {
                    // Iterate over each key-value pair in the MasterFundPreference dictionary
                    foreach (var kvp in masterFundPreference)
                    {
                        int masterFundPreferenceId = kvp.Key;
                        int value = kvp.Value;

                        if (value == default) continue;

                        if (preferenceLookup.TryGetValue(value, out var targetPercentageDict))
                        {
                            if (targetPercentageDict is SerializableDictionary<int, AccountValue> accountValues)
                            {
                                if (!resultDictionary.ContainsKey(allocation.MasterFundPreferenceId))
                                {
                                    resultDictionary[allocation.MasterFundPreferenceId] = new List<AccountValue>();
                                }

                                resultDictionary[allocation.MasterFundPreferenceId].AddRange(accountValues.Values);
                            }
                        }
                    }
                }
            }

            // Convert the dictionary to a list of KeyValuePairs and return
            return resultDictionary.ToList();
        }

        /// <summary>
        /// Filters and returns AllocationMasterFundPreference objects where all MasterFundPreference values match valid OperationPreferenceIds in the filtered preferences.
        /// </summary>
        private List<AllocationMasterFundPreference> GetCurrentPreferencesMasterFunds(List<AllocationMasterFundPreference> calculatedMasterFundPreferencesofAllocation, List<AllocationOperationPreference> filteredPreferences)
        {
            // Create a lookup for TargetPercentage based on OperationPreferenceId
            var preferenceLookup = filteredPreferences
                .Where(pref => pref?.TargetPercentage != null)
                .ToDictionary(pref => pref.OperationPreferenceId);

            var unmatchedAllocations = new List<AllocationMasterFundPreference>();

            foreach (var allocation in calculatedMasterFundPreferencesofAllocation)
            {
                if (allocation?.MasterFundPreference is SerializableDictionary<int, int> masterFundPreference)
                {
                    // Check if any value in MasterFundPreference does not match the lookup
                    var hasUnmatchedValue = masterFundPreference
                        .Values
                        .Any(value => !preferenceLookup.ContainsKey(value));

                    if (!hasUnmatchedValue)
                    {
                        unmatchedAllocations.Add(allocation);
                    }
                }
            }

            return unmatchedAllocations;
        }

        // <summary>
        /// Sends the first 6 trade attribute labels from cache to a Kafka response topic.
        /// </summary>
        public async void KafkaManager_GetTradeAttributeLabels(string topic, RequestResponseModel message)
        {
            try
            {
                if (_allocationProxy == null)
                {
                    _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                }
                DataSet tradeAttributesDataset = _allocationProxy.InnerChannel.GetAttributeNames();
                string requestId = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data)["RequestId"];
                if (tradeAttributesDataset == null ||
                    tradeAttributesDataset.Tables == null ||
                    tradeAttributesDataset.Tables.Count == 0 ||
                    tradeAttributesDataset.Tables[0] == null)
                {
                    Logger.LogError(new Exception("Trade attributes dataset is null or empty"), $"Trade attributes dataset is null or empty for topic {topic}.");
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_TradeAttributeLabelsResponse, new RequestResponseModel
                    {
                        ErrorMsg = "Trade attributes dataset is null or empty",
                        CorrelationId = message?.CorrelationId
                    });
                    return;
                }

                DataTable table = tradeAttributesDataset.Tables[0];

                var tradeAttributeLabels = table.AsEnumerable()
                .Take(6)
                .Select(row => new Dictionary<string, string>
                {
                    ["key"] = (row["AttributeValue"]?.ToString() ?? "").Replace(" ", ""),
                    ["value"] = row["AttributeName"]?.ToString() ?? ""
                })
                .ToList();

                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_TradeAttributeLabelsResponse,
                    new RequestResponseModel(message.CompanyUserID,
                    JsonHelper.SerializeObject(new
                    {
                        IsSuccess = true,
                        Data = new
                        {
                            Labels = tradeAttributeLabels,
                            RequestId = requestId
                        }
                    }),
                    message.CorrelationId
                ));
                Logger.LoggerWrite("Successfully sent trade attribute labels from KafkaManager_GetTradeAttributeLabels with CorrelationId: {0}", message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error while producing trade attribute details to {topic}");
            }
        }

        // <summary>
        /// Sends the first 6 trade attribute labels from cache to a Kafka response topic.
        /// </summary>
        public async void KafkaManager_RtpnlTradeAttributeLabelsRequest(string topic, RequestResponseModel message)
        {
            try
            {
                if (_allocationProxy == null)
                {
                    _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                }
                DataSet tradeAttributesDataset = _allocationProxy.InnerChannel.GetAttributeNames();

                if (tradeAttributesDataset == null ||
                    tradeAttributesDataset.Tables == null ||
                    tradeAttributesDataset.Tables.Count == 0 ||
                    tradeAttributesDataset.Tables[0] == null)
                {
                    Logger.LogError(new Exception("Trade attributes dataset is null or empty"), $"Trade attributes dataset is null or empty for topic {topic}.");
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse, new RequestResponseModel
                    {
                        ErrorMsg = "Trade attributes dataset is null or empty",
                        CorrelationId = message?.CorrelationId
                    });
                    return;
                }

                DataTable table = tradeAttributesDataset.Tables[0];

                var tradeAttributeLabels = table.AsEnumerable()
                    .Take(6)
                    .Select(row => new Dictionary<string, string>
                    {
                        ["key"] = row["AttributeValue"]?.ToString() ?? "",
                        ["value"] = row["AttributeName"]?.ToString() ?? ""
                    })
                    .ToList();

                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlTradeAttributeLabelsResponse,
                new RequestResponseModel(message.CompanyUserID,
                JsonHelper.SerializeObject(new
                {
                    IsSuccess = true,
                    Data = tradeAttributeLabels
                }),
                    message.CorrelationId
                ));
                Logger.LoggerWrite("Successfully sent trade attribute labels from KafkaManager_RtpnlTradeAttributeLabelsRequest with CorrelationId: {0}", message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error while producing trade attribute details to {topic}");
            }
        }

        /// <summary>
        /// Updates the allocation preferences in the trading ticket cache for each user.
        /// </summary>
        private void UpdateAllocationPreferenceInTTCache()
        {
            try
            {
                if (_allocationProxy == null)
                {
                    _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                }
                var companyUserIds = _userWiseTTCollectionData.Keys.ToList();
                foreach (var companyUserId in companyUserIds)
                {
                    if (!string.IsNullOrEmpty(_userWiseTTCollectionData[companyUserId]))
                    {
                        var prefManager = TradingTicketPrefManager.GetInstanceForUser(companyUserId, _loggedInUsersCachedData[companyUserId].CompanyID);
                        prefManager.Accounts = TradingTicketPreferenceDataManager.GetAccounts(companyUserId);
                        var userWiseTTData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(_userWiseTTCollectionData[companyUserId]);
                        Dictionary<int, string> preferences = _allocationProxy.InnerChannel.GetAllocationPreferences(_loggedInUsersCachedData[companyUserId].CompanyID, companyUserId, true, true);
                        if (preferences != null)
                        {
                            foreach (Account accountRow in preferences.Select(allocations => new Account
                            {
                                AccountID = allocations.Key,
                                Name = allocations.Value
                            }))
                            {
                                if (!prefManager.Accounts.Contains(accountRow.AccountID))
                                {
                                    prefManager.Accounts.Add(accountRow);
                                }
                            }

                            if (!userWiseTTData.ContainsKey(CommonDataConstants.CONST_USER_ALLOCATION_DATA))
                            {
                                userWiseTTData.Add(CommonDataConstants.CONST_USER_ALLOCATION_DATA, JsonHelper.SerializeObject(prefManager.Accounts));
                            }
                            else
                            {
                                userWiseTTData[CommonDataConstants.CONST_USER_ALLOCATION_DATA] = JsonHelper.SerializeObject(prefManager.Accounts);
                            }
                        }

                        //For Fetching calculatedPrefrences of Allocation.
                        var calculatedAccountPreferences = _allocationProxy.InnerChannel.GetCalculatedPreferencesByCompanyId(_loggedInUsersCachedData[companyUserId].CompanyID, companyUserId)
                        ?.Where(item => ((int?)item?.DefaultRule?.RuleType) == 1).ToList();

                        var filteredMasterFundPreferences = _allocationProxy.InnerChannel
                       .GetMasterFundPrefByCompanyId(_loggedInUsersCachedData[companyUserId].CompanyID, companyUserId)?
                       .Where(item => item?.DefaultRule?.RuleType == MatchingRuleType.None).ToList();

                        var currentMfPreferences = GetCurrentPreferencesMasterFunds(filteredMasterFundPreferences, calculatedAccountPreferences);
                        bool hasMFPref = calculatedAccountPreferences?.Any(item => item?.OperationPreferenceName?.Contains(CommonDataConstants.CONST_MF_PREFERENCE_NAME) == true) ?? false;
                        var finalPreferences = (calculatedAccountPreferences?.Any(item =>
                         item?.OperationPreferenceName?.Contains(CommonDataConstants.CONST_MF_PREFERENCE_NAME) == true) ?? false)
                         ? (object)currentMfPreferences
                         : (object)calculatedAccountPreferences;

                        // Add the final preferences to the user wise trading ticket data
                        if (!userWiseTTData.ContainsKey(CommonDataConstants.CONST_CALCULATED_PREFERENCES_OF_ALLOCATION))
                            userWiseTTData.Add(CommonDataConstants.CONST_CALCULATED_PREFERENCES_OF_ALLOCATION, JsonHelper.SerializeObject(finalPreferences));
                        else
                            userWiseTTData[CommonDataConstants.CONST_CALCULATED_PREFERENCES_OF_ALLOCATION] = JsonHelper.SerializeObject(finalPreferences);

                        if (hasMFPref)
                        {
                            var AccountDetailsOfTheMasterFundPreferences = GetMasterFundPreferenceWithTargetPercentage(filteredMasterFundPreferences, calculatedAccountPreferences);
                            if (!userWiseTTData.ContainsKey(CommonDataConstants.CONST_ACCOUNT_DETAILS_FOR_MASTERFUND_PREFERENCES))
                                userWiseTTData.Add(CommonDataConstants.CONST_ACCOUNT_DETAILS_FOR_MASTERFUND_PREFERENCES, JsonHelper.SerializeObject(AccountDetailsOfTheMasterFundPreferences));
                            else
                                userWiseTTData[CommonDataConstants.CONST_ACCOUNT_DETAILS_FOR_MASTERFUND_PREFERENCES] = JsonHelper.SerializeObject(AccountDetailsOfTheMasterFundPreferences);
                        }

                        //Update the allocation preferences details in the trading ticket data
                        Dictionary<int, AllocationOperationPreference> allocationPreferencesDetails = new Dictionary<int, AllocationOperationPreference>();
                        foreach (Account account in prefManager.Accounts)
                        {
                            var operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(account.AccountID);
                            if (operationPreference != null)
                            {
                                if (!allocationPreferencesDetails.ContainsKey(account.AccountID))
                                    allocationPreferencesDetails.Add(account.AccountID, operationPreference);
                            }
                        }
                        if (!_tradingTicketData.ContainsKey(CommonDataConstants.CONST_ALLOCATION_PREFERENCE_DATA))
                        {
                            userWiseTTData.Add(CommonDataConstants.CONST_ALLOCATION_PREFERENCE_DATA, JsonHelper.SerializeObject(allocationPreferencesDetails));
                        }
                        else
                        {
                            userWiseTTData[CommonDataConstants.CONST_ALLOCATION_PREFERENCE_DATA] = JsonHelper.SerializeObject(allocationPreferencesDetails);
                        }

                        _userWiseTTCollectionData[companyUserId] = JsonHelper.SerializeObject(userWiseTTData);
                        KafkaManager_ProduceTradingTicketData(companyUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UpdateAllocationPreferenceInTTCache encountered an error");
            }
        }
    }
}