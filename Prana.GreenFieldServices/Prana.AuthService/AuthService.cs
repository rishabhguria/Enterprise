using Castle.Windsor;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.CoreService.Interfaces;
using Prana.KafkaWrapper;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.ServiceModel;
using System.Threading.Tasks;
using Prana.Interfaces;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.Global.Utilities;
using System.Linq;
using Task = System.Threading.Tasks.Task;
using Prana.GreenfieldServices.Common;
using Prana.Global;
using Prana.BusinessObjects.GreenFieldModels;
using System.Threading;

namespace Prana.AuthService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class AuthService : BaseService, IAuthService, IDisposable, IClientConnectivityServiceCallback, IMarketDataPermissionServiceCallback
    {
        #region Variables

        private IWindsorContainer _container;
        private ServerHeartbeatManager _AuthServiceHeartbeatManager;
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;
        ProxyBase<IAuthenticateUser> _authenticateUserService = new ProxyBase<IAuthenticateUser>(EndPointAddressConstants.CONST_TradeAuthenticateUserServiceEndpoint);
        DuplexProxyBase<IClientConnectivityService> _clientConnectivityService = null;
        DuplexProxyBase<IMarketDataPermissionService> _marketDataPermissionServiceProxy = null;
        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());
        private int _hasServiceShutdownCleanupStarted = 0;

        #endregion

        #region IPranaServiceCommon Methods
        /// <summary>
        /// InitialiseService
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public async Task<bool> InitialiseService(IWindsorContainer container)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                #region Client Heartbeat Setup
                _tradeServiceClientHeartbeatManager = new ClientHeartbeatManager<ITradeService>(EndPointAddressConstants.CONST_TradeServiceEndpoint);
                _tradeServiceClientHeartbeatManager.ConnectedEvent += TradeServiceClientHeartbeatManager_ConnectedEvent;
                _tradeServiceClientHeartbeatManager.DisconnectedEvent += TradeServiceClientHeartbeatManager_DisconnectedEvent;
                _tradeServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;

                _pricingService2ClientHeartbeatManager = new ClientHeartbeatManager<IPricingService2>(EndPointAddressConstants.CONST_PricingService2Endpoint);
                _pricingService2ClientHeartbeatManager.ConnectedEvent += PricingService2ClientHeartbeatManager_ConnectedEvent;
                _pricingService2ClientHeartbeatManager.DisconnectedEvent += PricingService2ClientHeartbeatManager_DisconnectedEvent;
                _pricingService2ClientHeartbeatManager.AnotherInstanceSubscribedEvent += PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent;
                #endregion

                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_ServiceStatusRequest, KafkaManager_ServiceStatusMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_AuthServiceRequest, KafkaManager_LoginMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LogoutRequest, KafkaManager_LogoutMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserRequest, KafkaManager_InitializeLoggedInUserRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UpdateCacheRequestForLoginUser, KafkaManager_UpdateCacheForLoginUser);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BloombergAuthentication, KafkaManager_ProcessBloombergAuthentication);

                ProduceAuthServiceInitializeResponse();
                ProduceLoggedInUserResponse();
                CreateAuthServiceProxy();
                CreateMarketDataPermissionServiceProxy();

                #region Server Heartbeat Setup
                _AuthServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");
                Logger.LogMsg(LoggerLevel.Information, "Auth Service initialisation completed in {0} ms", sw.ElapsedMilliseconds);

                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while initializing services");
            }
            return false;
        }

        /// <summary>
        /// CleanUp
        /// </summary>
        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.
            if (Interlocked.Exchange(ref _hasServiceShutdownCleanupStarted, 1) == 1) return;

            // 1) Stop periodic callbacks BEFORE disposing anything they use
            StopServiceHealthPollingTimer();

            // 2) Mark down (and publish a final “down”)
            UpdateServiceStatus(ServiceNameConstants.CONST_Auth_Name, ServiceNameConstants.CONST_Auth_DisplayName, false);
            Console.WriteLine(AuthenticationConstants.MSG_SERVICE_SHUTDOWN);

            Logger.LogMsg(LoggerLevel.Fatal, "Shutting down service");
            _container.Dispose();
        }
        #endregion

        #region Kafka Message Handlers
        /// <summary>
        /// KafkaManager_InitializeLoggedInUserRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_InitializeLoggedInUserRequest(string topic, RequestResponseModel message)
        {
            try
            {
                ProduceLoggedInUserResponse();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// KafkaManager_LoginMessageReceived
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// 
        private async void KafkaManager_LoginMessageReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Processing log-in request...");
                    dynamic data = (JObject)JsonConvert.DeserializeObject<dynamic>(message.Data);
                    var is2FaLoginRequest = Is2FaLoginRequest(data);
                    var authUser = new AuthenticatedUserInfo();
                    string samsaraAzureId = string.Empty;
                    string msalToken = data.msalToken;

                    if (is2FaLoginRequest)
                    {
                        samsaraAzureId = GetUserSamsaraAzureId(msalToken);

                        var tokenInfo = new AzureTokenInfo(
                            msalToken,
                            Convert.ToString(data.clientIds),
                            Convert.ToString(data.issuers),
                            samsaraAzureId
                         );

                        bool is2faLoginValidated = await Validate2FaLogin(
                            tokenInfo,
                            message,
                            authUser);

                        if (!is2faLoginValidated)
                            return;
                    }

                    if (IsLoginThroughQuitCase(message.Data))
                    {
                        HandleQuitCaseLogin(data, message, authUser);
                    }
                    else
                    {
                        bool isNormalLoginProcess = await HandleNormalLogin(data, message, authUser, samsaraAzureId);
                        if (!isNormalLoginProcess)
                            return;
                    }

                    await ProduceAuthResponse(message, authUser, data, samsaraAzureId, is2FaLoginRequest);
                }
                catch (Exception ex)
                {
                    await HandleLoginException(message, ex);
                }
            }
        }

        /// <summary>
        /// KafkaManager_ServiceStatusMessageReceived
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_ServiceStatusMessageReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {

                    Logger.LogMsg(LoggerLevel.Debug, "Received ServiceStatusMessage request");

                    ServiceStatusInfo serviceStatus = _authenticateUserService.InnerChannel.GetServiceStatus();
                    message.Data = JsonConvert.SerializeObject(serviceStatus);

                    if (!string.IsNullOrEmpty(serviceStatus.ErrorMessage))
                        Logger.LogMsg(LoggerLevel.Information, "Service Status request failed with error {0}", serviceStatus.ErrorMessage);

                    bool isSuccess = string.IsNullOrWhiteSpace(serviceStatus.ErrorMessage);
                    if (isSuccess)
                    {
                        Logger.LogMsg(LoggerLevel.Debug, "Service Status request processed successfully");
                    }

                    Logger.LogMsg(LoggerLevel.Debug,
                            "Producing to the topic {0} with status {1}", topic, isSuccess.ToString());
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceStatusResponse, message);
                }
                catch (Exception ex)
                {
                    message.ErrorMsg = $"Error while producing topic {KafkaConstants.TOPIC_ServiceStatusResponse}:{ex.Message}";
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceStatusResponse, message);
                    Logger.LogError(ex, $"Error  while producing topic {KafkaConstants.TOPIC_ServiceStatusResponse}");
                }
            }
        }
        /// <summary>
        /// Produce Logged-In User Data Response
        /// </summary>
        private async void ProduceLoggedInUserResponse()
        {
            try
            {
                var loggedInUserInformations = _authenticateUserService.InnerChannel.GetLoggedInUser();

                ConcurrentDictionary<int, AuthenticatedUserInfo> loggedInUsers = new ConcurrentDictionary<int, AuthenticatedUserInfo>();
                foreach (var kvp in loggedInUserInformations)
                {
                    int userId = kvp.Key;
                    AuthenticatedUserInfo loggedInUser = kvp.Value;
                    if (!string.IsNullOrEmpty(loggedInUser.Token) && loggedInUser.AuthenticationType == AuthenticationTypes.WebLoggedIn)
                    {
                        loggedInUsers.TryAdd(userId, loggedInUser);
                        Logger.LogMsg(LoggerLevel.Debug, "Logged-in user information processed for user {0}", userId);
                    }
                }

                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_InitializeLoggedInUserResponse,
                    new RequestResponseModel(0, JsonConvert.SerializeObject(loggedInUsers)));

                Logger.LogMsg(LoggerLevel.Information, "Producing total of {0} logged-in user(s) info to the topic {1}",
                    loggedInUsers?.Count,
                    KafkaConstants.TOPIC_InitializeLoggedInUserResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceLoggedInUserResponse encountered an error");
            }
        }

        /// <summary>
        /// KafkaManager_LogoutMessageReceived
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_LogoutMessageReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    if (message.Data != string.Empty)
                    {
                        //This block will execute in case of forcefull logout user
                        dynamic data = (JObject)JsonConvert.DeserializeObject<dynamic>(message.Data);
                        if (data.AuthenticationType == AuthenticationTypes.EnterpriseLoggedIn)
                        {
                            Logger.LogMsg(LoggerLevel.Information, "Received forceful Logout request from {0} ", "Enterprise");

                            //This block will execute when there is a request from samsara to forceful logout enterprise
                            var result = _authenticateUserService.InnerChannel.CompanyUserLogout(message.CompanyUserID.ToString(), true, false);
                            Logger.LogMsg(LoggerLevel.Information, "Received forceful Logout response from core service as: {0} ", result);
                            if (!result.Contains(AuthenticationConstants.MSG_FAILED_LOGGED_OUT))
                                ProduceLoggedOutUserResponse(message);
                        }
                        else if (data.AuthenticationType == AuthenticationTypes.WebAlreadyLoggedInForAnotherWebSession)
                        {
                            //This block will execute when there is a request from samsara to forceful logout samsara

                            Logger.LogMsg(LoggerLevel.Information, "Received forceful Logout request from {0} ", "WebAlreadyLoggedInForAnotherWebSession");
                            var result = _authenticateUserService.InnerChannel.CompanyUserLogout(message.CompanyUserID.ToString(), false, true);
                            Logger.LogMsg(LoggerLevel.Information, "Received forceful Logout response from core service as: {0} ", result);
                            if (!result.Contains(AuthenticationConstants.MSG_FAILED_LOGGED_OUT))
                                ProduceLoggedOutUserResponse(message);

                        }
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "Received normal Logout request");

                        //This block will execute in case of normal logout flow from Samsara
                        string result = _authenticateUserService.InnerChannel.CompanyUserLogout(message.CompanyUserID.ToString(), false, false);
                        message.Data = result;

                        Logger.LogMsg(LoggerLevel.Information, "Received normal Logout response from core service as: {0} ", result);

                        _clientConnectivityService.InnerChannel.RemoveClientInfoFromCache(Convert.ToInt32(message.CompanyUserID), false, false);

                        if (!result.Contains(AuthenticationConstants.MSG_FAILED_LOGGED_OUT))
                            ProduceLoggedOutUserResponse(message);
                    }
                    try
                    {
                        //Removing the user from User Identity mapping                        
                        _marketDataPermissionServiceProxy.InnerChannel.RemoveSubscriptionToGetPermissionFromCache(Convert.ToInt32(message.CompanyUserID), "Samsara");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error while removing user from User Identity mapping");
                    }
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LogoutResponse, message);
                }
                catch (Exception ex)
                {
                    message.ErrorMsg = $"Error while producing topic {topic}:{ex.Message}";
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LogoutResponse, message);
                    Logger.LogError(ex, $"KafkaManager_LogoutMessageReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// This block will execute in the case when there is a request to update the cache for logged in user from samsara
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UpdateCacheForLoginUser(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    //This block will execute in the case when there is a request to update the cache for logged in user from samsara
                    Logger.LogMsg(LoggerLevel.Information, "Received request to update cache from samsara");

                    AuthenticatedUserInfo authUser = new AuthenticatedUserInfo();
                    dynamic data = (JObject)JsonConvert.DeserializeObject<dynamic>(message.Data);

                    authUser = UpdateCacheForLoginUser(data);
                    if (string.IsNullOrEmpty(authUser.ErrorMessage))
                    {
                        //Produces a topic with logged in user in order to update it in common data service in case of service restart
                        ProduceLoggedInUserResponse();
                    }

                    Logger.LogMsg(LoggerLevel.Debug, "Received request to update cache from samsara {0}", "completed");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UpdateCacheForLoginUser encountered an error");
                }
            }
        }

        /// <summary>
        /// This block will execute in the case when there is a request to login a user from Bloomberg in samsara
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_ProcessBloombergAuthentication(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    //This block will execute in the case when there is a request to update the cache for logged in user from samsara
                    Logger.LogMsg(LoggerLevel.Information, "Received request to authenticate bloomberg user from samsara");
                    _marketDataPermissionServiceProxy.InnerChannel.PermissionCheck(null, "Samsara", message.Data);

                    Logger.LogMsg(LoggerLevel.Debug, "Received request to authenticate bloomberg user from samsara {0}", "completed");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UpdateCacheForLoginUser encountered an error");
                }
            }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// This is a common method for updating the cache and returns the authUser object which contains the userDetails
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AuthenticatedUserInfo UpdateCacheForLoginUser(dynamic data)
        {
            //This is a common method for updating the cache and returns the authUser object which contains the userDetails
            AuthenticatedUserInfo authUser = new AuthenticatedUserInfo();
            try
            {
                string userName = data.userName;
                string companyUserId = data.companyUserId;
                string token = data.token;

                Logger.LogMsg(LoggerLevel.Information, "Updating cache for the user {0} in UpdateCacheForLoginUser()", companyUserId);

                _clientConnectivityService.InnerChannel.RemoveClientInfoFromCache(Convert.ToInt32(companyUserId), false, false);
                authUser = _clientConnectivityService.InnerChannel.UpdateCacheForLoginUser(companyUserId, userName, token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UpdateCacheForLoginUser encountered an error");
                throw;
            }
            return authUser;
        }

        /// <summary>
        /// Handles companyUser logout requested from Enterprise.
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="isForcefulLogout"></param>
        /// <returns></returns>
        public bool CompanyUserLogout(string companyUserId, bool isForcefulLogoutEnterprise, bool isForcefulLogoutWeb)
        {
            bool result = false;
            try
            {
                string msg = _authenticateUserService.InnerChannel.CompanyUserLogout(companyUserId.ToString(), false, false);

                Logger.LogMsg(LoggerLevel.Debug, "Received {0} response message from core service for CompanyUserLogout() for userId:{1}", msg, companyUserId);

                if (msg.Contains(AuthenticationConstants.MSG_SUCCESSFUL_LOGGED_OUT))
                    result = true;

                //This below code will remove the user from "ClientConnectivity.cs" from trade service
                _clientConnectivityService.InnerChannel.RemoveClientInfoFromCache(Convert.ToInt32(companyUserId), false, true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"CompanyUserLogout encountered an error for user:{companyUserId}, isForceLogoutEnterpr:{isForcefulLogoutEnterprise}, isForceLogoutWeb:{isForcefulLogoutWeb}");
            }
            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// AuthServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, Prana.Global.EventArgs<string, string> e)
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

        /// <summary>
        /// AuthServiceClientHeartbeatManager_ConnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Trade_Name, ServiceNameConstants.CONST_Trade_DisplayName, true);
                _authenticateUserService = new ProxyBase<IAuthenticateUser>(EndPointAddressConstants.CONST_TradeAuthenticateUserServiceEndpoint);
                CreateAuthServiceProxy();
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
        /// AuthServiceClientHeartbeatManager_DisconnectedEvent
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

        /// <summary>
        /// Converts the array of Ascii code into string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ConvertAsciiArrayToString(string[] data)
        {
            char[] characters = new char[data.Length];
            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    characters[i] = (char)int.Parse(data[i]);
                    if (i % 2 == 0)
                    {
                        characters[i] = (char)(characters[i] - 2);
                    }
                    else
                    {
                        characters[i] = (char)(characters[i] + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return new string(characters);
        }

        /// <summary>
        /// Method used to validate the MSAL Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="expectedIssuer"></param>
        /// <returns> Returns true if the MSAL Token is valid; otherwise, returns false</returns>

        private async Task<bool> ValidateMsalToken(AzureTokenInfo tokenInfo)
        {
            try
            {
                var validAudiences = tokenInfo.ClientIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToArray();

                var validIssuers = tokenInfo.Issuers
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Trim())
                    .ToArray();

                var issuerForDiscovery = validIssuers.First();     // Use the first issuer to fetch OpenID config

                var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{issuerForDiscovery}/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever());

                var config = await configManager.GetConfigurationAsync();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = validIssuers,
                    ValidateAudience = true,
                    ValidAudiences = validAudiences,
                    ValidateLifetime = true,
                    IssuerSigningKeys = config.SigningKeys,
                    ClockSkew = TimeSpan.Zero
                };

                var handler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(tokenInfo.MsalToken, validationParameters, out validatedToken);

                // If no exception is throw, the token is valid
                return true;
            }
            catch (SecurityTokenExpiredException ex)
            {
                Logger.LogError(ex, "Login failed: The authentication token has expired. User must re-authenticate.");
                return false;
            }
            catch (SecurityTokenInvalidIssuerException e)
            {
                Logger.LogError(e, "Login failed: The authentication token was issued by an untrusted or unexpected authority.");
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Login failed: An unexpected error occurred during token validation.");
                return false;
            }
        }

        /// <summary>
        /// Produce Logged-out User data response
        /// </summary>
        private async void ProduceLoggedOutUserResponse(RequestResponseModel model)
        {
            var respData = new RequestResponseModel(model.CompanyUserID,
                AuthenticationConstants.CONST_LOGOUT,
                model.CorrelationId);
            var topic = KafkaConstants.TOPIC_InitializeLoggedOutUserResponse;
            try
            {
                await KafkaManager.Instance.Produce(
                    topic,
                    respData);
            }
            catch (Exception ex)
            {
                respData.ErrorMsg = $"Error while producing topic {topic}:{ex.Message}";
                await KafkaManager.Instance.Produce(topic, respData);
                Logger.LogError(ex, "ProduceLoggedOutUserResponse encountered an error");
            }
        }

        /// <summary>
        /// Produce AuthService initialize response to Service Gateway
        /// </summary>
        private async void ProduceAuthServiceInitializeResponse()
        {
            try
            {
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_AuthServiceInitialized,
                    new RequestResponseModel(0, string.Empty));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceAuthServiceInitializeResponse encountered an error");
            }
        }

        /// <summary>
        /// GetInstance_InformationReceived
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                string[] messageList = message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void CreateAuthServiceProxy()
        {
            try
            {
                _clientConnectivityService = new DuplexProxyBase<IClientConnectivityService>(EndPointAddressConstants.CONST_TradeClientConnectivityServiceEndpoint, this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void CreateMarketDataPermissionServiceProxy()
        {
            try
            {
                _marketDataPermissionServiceProxy = new DuplexProxyBase<IMarketDataPermissionService>(EndPointAddressConstants.CONST_PricingMarketDataPermissionServiceEndpoint, this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private string GetUserSamsaraAzureId(string msalToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(msalToken);

                string samsaraAzureId = jwt.Claims
                    .FirstOrDefault(c => c.Type == "preferred_username")?
                    .Value;

                Logger.LogMsg(LoggerLevel.Information, "SamsaraAzureId from msalToken is {0}", samsaraAzureId);

                if (string.IsNullOrWhiteSpace(samsaraAzureId))
                    Logger.LogMsg(LoggerLevel.Information,
                        "Unable to fetch user details (samsaraAzureId) from MSAL token. Token may be malformed or missing required claims");
                return samsaraAzureId;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while extracting Samsara Azure ID from MSAL token");
                return null;
            }
        }

        private bool Is2FaLoginRequest(dynamic data)
        {
            string msalToken = data.msalToken;
            return !string.IsNullOrWhiteSpace(msalToken);
        }

        private bool IsLoginThroughQuitCase(string messageData)
        {
            return messageData.Contains(AuthenticationConstants.CONST_TOKEN);
        }

        private async Task<bool> Validate2FaLogin(AzureTokenInfo tokenInfo,
            RequestResponseModel message,
            AuthenticatedUserInfo authUser)
        {
            bool isMsalTokenValid = await ValidateMsalToken(tokenInfo);

            if (!isMsalTokenValid || string.IsNullOrWhiteSpace(tokenInfo.SamsaraAzureId))
            {
                authUser.ErrorMessage = $"Your login session has expired or is invalid. Please sign in again. CorrelationId:{message.CorrelationId}";
                message.StatusCode = -1;
                message.Data = JsonConvert.SerializeObject(authUser);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_AuthServiceResponse, message);
                return false;
            }
            return true;
        }

        private async Task<bool> HandleNormalLogin(dynamic data, RequestResponseModel message, AuthenticatedUserInfo authUser, string samsaraAzureId)
        {
            string userName = data.userName;
            string[] asciiPassword = data.password.ToObject<string[]>();
            string password = ConvertAsciiArrayToString(asciiPassword);

            authUser = _authenticateUserService.InnerChannel.ValidateCompanyUserLogin(userName, password, true, samsaraAzureId);
            message.Data = JsonConvert.SerializeObject(authUser);
            _clientConnectivityService.InnerChannel.AddClientInfoInCache(authUser.CompanyUserId);

            if (!string.IsNullOrEmpty(authUser.ErrorMessage))
            {
                Logger.LogMsg(LoggerLevel.Fatal, "Login failed from trade server with message : {0}", authUser.ErrorMessage);
                message.StatusCode = -2;
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_AuthServiceResponse, message);
                return false;
            }
            return true;
        }

        private void HandleQuitCaseLogin(dynamic data, RequestResponseModel message, AuthenticatedUserInfo authUser)
        {
            authUser = UpdateCacheForLoginUser(data);
            message.Data = JsonConvert.SerializeObject(authUser);

            Logger.LogMsg(LoggerLevel.Information,
                "Updating cache for login user. No token present in request. This may occur if the user has exited and relaunched Samsara. UserName: {0}",
                data.userName);
        }

        private async Task ProduceAuthResponse(RequestResponseModel message, AuthenticatedUserInfo authUser, dynamic data, string samsaraAzureId, bool is2FaLoginRequest)
        {
            bool isSuccess = string.IsNullOrWhiteSpace(authUser.ErrorMessage);

            if (isSuccess)
            {
                ProduceLoggedInUserResponse();
                Logger.LogMsg(LoggerLevel.Information,
                    "Login request successfully UserName:{0}, samsaraAzureId:{1}, 2FaEnabled:{2}",
                    data.userName, samsaraAzureId, is2FaLoginRequest);
            }

            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_AuthServiceResponse, message);
        }

        private async Task HandleLoginException(RequestResponseModel message, Exception ex)
        {
            message.ErrorMsg = $"Error while producing topic {KafkaConstants.TOPIC_AuthServiceResponse}:{ex.Message}";
            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_AuthServiceResponse, message);
            Logger.LogError(ex, $"Error  while producing topic {KafkaConstants.TOPIC_AuthServiceResponse}");
        }


        /// <summary>  
        /// Publishes the current service status to a Kafka topic.  
        /// This method retrieves the service status, serializes it, and sends it to the Kafka topic  
        /// specified in the KafkaConstants.TOPIC_ServiceHealthStatus.  
        /// </summary>  
        public async void ProduceServiceStatusMessage()
        {
            try
            {
                // Capture current live flags once
                bool pricingIsLive = GetServiceStatus(ServiceNameConstants.CONST_Pricing_Name)?.IsLive ?? false;
                bool tradeIsLive = GetServiceStatus(ServiceNameConstants.CONST_Trade_Name)?.IsLive ?? false;
                var services = new (string Name, string DisplayName, bool IsLive)[]
                {
                    (ServiceNameConstants.CONST_Auth_Name,    ServiceNameConstants.CONST_Auth_DisplayName,    true),
                    (ServiceNameConstants.CONST_Pricing_Name, ServiceNameConstants.CONST_Pricing_DisplayName, pricingIsLive),
                    (ServiceNameConstants.CONST_Trade_Name,   ServiceNameConstants.CONST_Trade_DisplayName,   tradeIsLive),
                };

                foreach (var s in services)
                {
                    UpdateServiceStatus(s.Name, s.DisplayName, s.IsLive);
                    var serviceStatus = GetServiceStatus(s.Name);
                    var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));

                    // Publish the message to the Kafka topic
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);
                }

                Logger.LogMsg(LoggerLevel.Verbose, "Service statuses published to Kafka topic {0}", KafkaConstants.TOPIC_ServiceHealthStatus);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceServiceStatusMessage encountered an error");
            }
        }
        #endregion

        #region IServiceStatus Methods
        /// <summary>
        /// Subscribe
        /// </summary>
        /// <param name="subscriberName"></param>
        /// <param name="isRetryRequest"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public async Task<bool?> Subscribe(string subscriberName, bool isRetryRequest)
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
                else if (_AuthServiceHeartbeatManager != null && !isRetryRequest)
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

        /// <summary>
        /// UnSubscribe
        /// </summary>
        /// <param name="subscriberName"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public async System.Threading.Tasks.Task UnSubscribe(string subscriberName)
        {
            try
            {
                ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(subscriberName);

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
        public Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IContainerService Methods
        public Task<List<HostedService>> GetClientServicesStatus()
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetDebugModeStatus()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> LoadLog()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> OpenLog()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task RequestStartupData()
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task StopService()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDisposable Methods
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_tradeServiceClientHeartbeatManager != null)
                        _tradeServiceClientHeartbeatManager.Dispose();
                    if (_authenticateUserService != null)
                        _authenticateUserService.Dispose();
                    if (_AuthServiceHeartbeatManager != null)
                        _AuthServiceHeartbeatManager.Dispose();

                    if (ServiceHealthPollingTimer != null)
                    {
                        ServiceHealthPollingTimer.Dispose();
                        ServiceHealthPollingTimer = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Client connectivity response from _clientConnectivityService.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="isLoggedOut"></param>
        public void ClientConnectivityResponseEnterprise(int companyUserID, bool isLoggedOut)
        {
        }

        /// <summary>
        /// Client connectivity response from _clientConnectivityService.
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="isLoggedOut"></param>
        public async void ClientConnectivityResponseWeb(int companyUserID, bool isLoggedOut)
        {
            try
            {
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ForcefulLogoutWeb, new RequestResponseModel(companyUserID, String.Empty));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Kafka Reporters
        private void Kafka_ProducerReporter(string topic)
        {
            //We have to stop these log flood for service health topic
            if(topic == KafkaConstants.TOPIC_ServiceHealthStatus)
                Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_PRODUCE, topic);
            else 
                Logger.LoggerWrite(string.Format(KafkaLoggingConstants.MSG_KAFKA_PRODUCE, topic), LoggingConstants.Category_Kafka_Reporter, 1, 1, TraceEventType.Information);
        }

        private void Kafka_ConsumerReporter(string topic)
        {
            Logger.LoggerWrite(topic + KafkaLoggingConstants.MSG_KAFKA_CONSUMER, LoggingConstants.Category_Kafka_Reporter, 1, 1, TraceEventType.Information);
        }
        #endregion

        #region IMarketDataPermissionServiceCallback Members
        public async void PermissionCheckResponse(int companyUserID, bool isPermitted)
        {
            try
            {
                RequestResponseModel response = new RequestResponseModel(companyUserID, JsonHelper.SerializeObject(isPermitted));
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BloombergEventResponse, response);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion
        class AzureTokenInfo
        {
            public AzureTokenInfo(string token, string clientIds, string issuers, string samsaraAzureId)
            {
                MsalToken = token;
                ClientIds = clientIds;
                Issuers = issuers;
                SamsaraAzureId = samsaraAzureId;
            }

            public string SamsaraAzureId { get; set; }
            public string MsalToken { get; set; }
            public string ClientIds { get; set; }
            public string Issuers { get; set; }
        }

    }
}