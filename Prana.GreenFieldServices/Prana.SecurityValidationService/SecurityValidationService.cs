using Castle.Windsor;
using Newtonsoft.Json;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.GreenfieldServices.Common;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;


namespace Prana.SecurityValidationService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class SecurityValidationService : BaseService, ISecurityValidationService, IDisposable, IPublishing
    {
        #region Variables
        private IWindsorContainer _container;

        private ServerHeartbeatManager _SecurityValidationServiceHeartbeatManager;

        private readonly object _locker = new object();

        private DuplexProxyBase<ISubscription> _proxy;

        /// <summary>
        /// The new Equity Option validation allowed
        /// </summary>
        private bool _equityOption = false;

        /// <summary>
        /// The _security master
        /// </summary>
        private SecMasterClientNew _securityMaster;

        ///<summary>
        /// Logged-in company users information collection
        ///</summary>
        Dictionary<int, CompanyUser> _userWiseLoggedInUserInformation = new Dictionary<int, CompanyUser>();

        ///<summary>
        ///The page index
        ///</summary>
        private int pageIndex = 0;

        ///<summary>
        ///The page size
        ///</summary>
        private int _pageSize = 20;

        /// <summary>
        /// _tradeServiceClientHeartbeatManager to maintain the heartbeat connection with trade service.
        /// </summary>
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;

        private ConcurrentDictionary<string, string> _securityValidationCache = new ConcurrentDictionary<string, string>();

        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

        private int _cleanedUp = 0;

        /// <summary>
        /// Tracks whether the logged-in user response has been received from Auth Service
        /// True if received; otherwise, false.
        /// </summary>
        bool _loggedInUserResponseReceivedFromAuth = false;
        /// <summary>
        /// Tracks whether the compliance permissions response has been received from the Common Data Service.
        /// True if received; otherwise, false.
        /// </summary>
        bool _compliancePermissionsResponseReceivedFromCommonData = false;

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
                UpdateServiceStatus(ServiceNameConstants.CONST_SecurityValidation_Name, ServiceNameConstants.CONST_SecurityValidation_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_SecurityValidation_Name);
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
                MakeProxy();
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

        #region Constructor
        /// <summary>
        /// Sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        internal SecMasterClientNew SecurityMaster
        {
            set
            {
                if (_securityMaster == null)
                {
                    _securityMaster = value;
                    _securityMaster.SecMstrDataResponse += _securityMaster_SecMstrDataResponse;
                    _securityMaster.Disconnected += _securityMaster_Disconnected;
                    _securityMaster.Connected += _securityMaster_Connected;
                    _securityMaster.SecMstrDataSymbolSearcResponse += _securityMaster_SecMstrDataSymbolSearcResponse;
                    _securityMaster.SymbolLookUpDataResponse += _securityMaster_SymbolLookUpDataResponse;
                    _securityMaster.ResponseCompleted += new EventHandler<EventArgs<QueueMessage>>(_securityMaster_ResponseCompleted);
                }
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

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                #region socket connection
                SecurityMaster = new SecMasterClientNew();
                _securityMaster.ConnectToServer();
                MakeProxy();
                #endregion

                #region Kafka subscription
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_UserLoggedInInformation);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_InitializeLoggedOutUsers);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseLoggedInInformationResponse, KafkaManager_UserWiseLoggedInInformationReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_EquityOptionManualValidationResponse, KafkaManager_EquityOptionManualValidationResponseReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SecurityValidationRequest, KafkaManager_SymbolValidationReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SecuritySearchRequest, KafkaManager_SecuritySearchRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SMSecuritySearchRequest, KafkaManager_SMSecuritySearchRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SMSaveNewSymbolRequest, KafkaManager_SMSaveNewSymbolRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoggedInUserResponse, KafkaManager_UpdateLoggedInUserData);

                #endregion

                #region Server Heartbeat Setup
                _SecurityValidationServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");
                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                // fire and forget method for handling of auth service connection
                ServiceConnectionPoller.PollUntilServiceReady(() => _compliancePermissionsResponseReceivedFromCommonData, KafkaConstants.TOPIC_EquityOptionManualValidationRequest, ServiceNameConstants.CONST_CommonData_DisplayName);
                ServiceConnectionPoller.PollUntilServiceReady(() => _loggedInUserResponseReceivedFromAuth, KafkaConstants.TOPIC_InitializeLoggedInUserRequest, ServiceNameConstants.CONST_Auth_DisplayName);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                   "InitialiseService Completed for SecurityValidation Service in {0} ms.", sw.ElapsedMilliseconds);

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
            UpdateServiceStatus(ServiceNameConstants.CONST_SecurityValidation_Name, ServiceNameConstants.CONST_SecurityValidation_DisplayName, false);

            Console.WriteLine("Shutting down service.");
            Logger.LogMsg(LoggerLevel.Information, "Shutting down Service...");
            _container.Dispose();
        }
        #endregion

        #region Security Master Methods
        /// <summary>
        /// Handles the Connected event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _securityMaster_Connected(object sender, EventArgs e)
        {
            try
            {

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
        /// Handles the SecMstrDataResponse event of the _securityMaster control.
        /// Sends back the validation symbol response
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SecMasterBaseObj"/> instance containing the event data.</param>
        protected void _securityMaster_SecMstrDataResponse(object sender, SecMasterBaseObj e)
        {
            try
            {
                string ValidationResponse = string.Empty;
                int RequestedCompanyUserID = 0;

                // Check if the response contains a hashcode and user ID
                if (!string.IsNullOrEmpty(e.RequestedHashcode) && !string.IsNullOrEmpty(e.RequestedUserID))
                {
                    ValidationResponse = JsonHelper.SerializeObject(e);
                    if (!string.IsNullOrEmpty(ValidationResponse))
                    {
                        RequestedCompanyUserID = int.TryParse(e.RequestedUserID, out int result) ? result : 0;
                    }

                    SendValidationResponse(ValidationResponse, RequestedCompanyUserID);
                    InformationReporter.GetInstance.Write(SecurityServiceConstants.MSG_SecurityValidationValidated + e.TickerSymbol);

                    // Generate a unique key for the request and remove related cache entries
                    string uniqueKeyRequestID = RequestedCompanyUserID + "_" + e.RequestedHashcode;
                    foreach (var key in _securityValidationCache.Keys.Where(key => key.Contains(uniqueKeyRequestID)).ToList())
                    {
                        _securityValidationCache.TryRemove(key, out _);
                    }
                }
                else
                {
                    // Process broadcast messages for symbols
                    var uniqueKeys = GetKeysByValue(e);
                    foreach (var key in uniqueKeys)
                    {
                        string[] dataObject = key.Split('_');
                        RequestedCompanyUserID = int.TryParse(dataObject[0], out int result) ? result : 0;

                        string RequestID = dataObject.Length > 1 ? dataObject[1] : string.Empty;
                        e.RequestedHashcode = RequestID;
                        e.RequestedUserID = RequestedCompanyUserID.ToString();

                        ValidationResponse = JsonHelper.SerializeObject(e);

                        SendValidationResponse(ValidationResponse, RequestedCompanyUserID);
                        Logger.LogMsg(LoggerLevel.Information, "Broadcast message received for symbol: {0}. Response sent to user with ID: {1} and Request ID: {2}", e.TickerSymbol, RequestedCompanyUserID, RequestID);

                        // Remove related cache entries
                        _securityValidationCache.TryRemove(key, out _);
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
        /// Handles the SecMstrDataSymbolSearcResponse event of the _securityMaster control.
        /// sends back the search symbol response
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SecMasterSymbolSearchRes"/> instance containing the event data.</param>
        protected async void _securityMaster_SecMstrDataSymbolSearcResponse(object sender, SecMasterSymbolSearchRes e)
        {
            try
            {
                string SearchResponse = JsonHelper.SerializeObject(e);
                if (!string.IsNullOrEmpty(SearchResponse))
                {
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SecuritySearchResponse, new RequestResponseModel(e.UserID, SearchResponse));
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
        /// Handles the Disconnected event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void _securityMaster_Disconnected(object sender, EventArgs e)
        {
            try
            {

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
        /// Handles the ResponseCompleted event of the _securityMaster control.
        /// Returns the response information of the symbol saved
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Data.DataSet"/> instance containing the event data.</param>
        private async void _securityMaster_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Value.Message.ToString()))
                {
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SMSymbolSaveClickedResponse, new RequestResponseModel(0, e.Value.Message.ToString()));
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
        /// Handles the SymbolLookUpDataResponse event of the _securityMaster control.
        /// Returns the symbol lookup response from Security master
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Data.DataSet"/> instance containing the event data.</param>
        private async void _securityMaster_SymbolLookUpDataResponse(object sender, EventArgs<System.Data.DataSet> e)
        {
            try
            {
                string Symbol_PK = string.Empty;
                DateTime expirationDate = DateTime.MinValue;
                if (e.Value.Tables.Count > 0)
                {
                    foreach (DataRow row in e.Value.Tables[0].Rows)
                    {
                        Symbol_PK = row["Symbol_PK"].ToString();
                        if (Convert.ToInt32(row["AssetID"]) == 2)
                        {
                            if (row["OPTExpiration"] != DBNull.Value && row["OPTExpiration"] != null && !string.IsNullOrWhiteSpace(row["OPTExpiration"].ToString()))
                            {
                                expirationDate = Convert.ToDateTime(row["OPTExpiration"]);
                            }
                        }
                        else if (Convert.ToInt32(row["AssetID"]) == 3)
                        {
                            if (row["FUTExpiration"] != DBNull.Value && row["FUTExpiration"] != null && !string.IsNullOrWhiteSpace(row["FUTExpiration"].ToString()))
                            {
                                expirationDate = Convert.ToDateTime(row["FUTExpiration"]);
                            }
                        }
                    }
                }
                Dictionary<string, object> SymbolSearch_Response = new Dictionary<string, object>();
                SymbolSearch_Response.Add("Symbol_PK", Symbol_PK);
                SymbolSearch_Response.Add("Symbol_SearchData", e);
                if (expirationDate != DateTime.MinValue)
                {
                    int maturityDay = expirationDate.Day;
                    int maturityMonth = SecMasterOptObj.GetMaturityMonth(expirationDate);
                    SymbolSearch_Response.Add("MaturityDay", maturityDay);
                    SymbolSearch_Response.Add("MaturityMonth", maturityMonth);
                }

                string SMSymbolLookupDataresponse = JsonHelper.SerializeObject(SymbolSearch_Response);
                if (!string.IsNullOrEmpty(SMSymbolLookupDataresponse))
                {
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SMSecuritySearchResponse, new RequestResponseModel(0, SMSymbolLookupDataresponse));
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

        #region Other methods
        /// <summary>
        /// gets Sec master object from UI object
        /// </summary>
        /// <param name="uiObj"></param>
        private SecMasterBaseObj GetSecMasterObjFromUIObject(SecMasterUIObj uiObj)
        {
            SecMasterBaseObj secMasterBaseObj = null;
            try
            {
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)uiObj.AssetID);

                switch (baseAssetCategory)
                {
                    case AssetCategory.Equity:
                    case AssetCategory.PrivateEquity:
                    case AssetCategory.CreditDefaultSwap:
                        secMasterBaseObj = new SecMasterEquityObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.Option:
                    case AssetCategory.FXOption:
                        secMasterBaseObj = new SecMasterOptObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.Future:
                        if ((AssetCategory)uiObj.AssetID == AssetCategory.FXForward)
                        {
                            secMasterBaseObj = new SecMasterFXForwardObj();
                            secMasterBaseObj.FillUIData(uiObj);
                        }
                        else
                        {
                            secMasterBaseObj = new SecMasterFutObj();
                            secMasterBaseObj.FillUIData(uiObj);
                        }
                        break;
                    case AssetCategory.FX:
                        secMasterBaseObj = new SecMasterFxObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.Indices:
                        secMasterBaseObj = new SecMasterIndexObj();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                    case AssetCategory.FixedIncome:
                    case AssetCategory.ConvertibleBond:
                        secMasterBaseObj = new SecMasterFixedIncome();
                        secMasterBaseObj.FillUIData(uiObj);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetSecMasterObjFromUIObject encountered an error");
                throw;
            }
            return secMasterBaseObj;
        }

        /// <summary>
        /// Sends the request for symbol search to sm.
        /// </summary>
        /// <param name="data">Thesymbol.</param>
        private void SMSecuritySearchRequest(string data)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    dynamic frontEndData = JsonHelper.DeserializeToObject<dynamic>(data);
                    string symbol = frontEndData.Symbol;
                    string securityAction = frontEndData.SecurityAction;
                    SymbolLookupRequestObject symbolLookupRequest = new SymbolLookupRequestObject();
                    SecMasterConstants.SecurityActions symbolAction = (SecMasterConstants.SecurityActions)Enum.Parse(typeof(SecMasterConstants.SecurityActions), securityAction); ;
                    SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), "Ticker");

                    switch (symbolAction)
                    {
                        case SecMasterConstants.SecurityActions.SEARCH:
                            switch (searchCriteria)
                            {
                                case SecMasterConstants.SearchCriteria.Ticker:
                                    symbolLookupRequest.TickerSymbol = symbol;
                                    break;
                                case SecMasterConstants.SearchCriteria.Bloomberg:
                                    symbolLookupRequest.BloombergSymbol = symbol;
                                    break;
                                case SecMasterConstants.SearchCriteria.FactSetSymbol:
                                    symbolLookupRequest.FactSetSymbol = symbol;
                                    break;
                                case SecMasterConstants.SearchCriteria.ActivSymbol:
                                    symbolLookupRequest.ActivSymbol = symbol;
                                    break;
                            }
                            break;
                    }
                    RequestData(symbolLookupRequest, searchCriteria);
                    Logger.LogMsg(LoggerLevel.Debug, "SMSecuritySearchRequest request processed successfully in {0} ms for symbol:{1}", symbol);
                }
                else
                {
                    Logger.LogMsg(LoggerLevel.Fatal, "{0}", "SecMasterClientNew is disconnected.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SMSecuritySearchRequest encountered an error");
            }
        }

        /// <summary>
        /// Creates the symbol search criteria for requested symbol.
        /// </summary>
        /// <param name="symbolLookupRequestObject">The requested symbol.</param>
        /// <param name="SearchCriteria">The search criteria.</param>
        private void RequestData(SymbolLookupRequestObject symbolLookupRequestObject, SecMasterConstants.SearchCriteria SearchCriteria)
        {
            if (symbolLookupRequestObject != null)
            {
                SecMasterConstants.SearchCriteria searchCriteria = (SecMasterConstants.SearchCriteria)Enum.Parse(typeof(SecMasterConstants.SearchCriteria), SearchCriteria.ToString());
                _pageSize = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SMChunkSize").ToString());
                //Condition modified to check if search criteria is company name or underlying symbol, PRANA-13894
                _pageSize = (pageIndex == 0 && searchCriteria != SecMasterConstants.SearchCriteria.CompanyName && searchCriteria != SecMasterConstants.SearchCriteria.UnderlyingSymbol) ? (_pageSize - 1) : _pageSize;
                symbolLookupRequestObject.StartIndex = pageIndex * _pageSize + 1;
                symbolLookupRequestObject.EndIndex = (pageIndex + 1) * _pageSize;
                symbolLookupRequestObject.RequestID = Guid.NewGuid().ToString();
                _securityMaster.GetSymbolLookupRequestedData(symbolLookupRequestObject);
            }
        }

        // <summary>
        /// Symbol validation request by user
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SymbolValidationReceived(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();

            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    if (!string.IsNullOrEmpty(message.Data))
                    {
                        dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                        // Case 1: Option Symbol
                        if (!string.IsNullOrEmpty((string)data.OptionSymbol))
                        {
                            ValidateSymbols(
                                new List<string> { (string)data.OptionSymbol },
                                (bool)data.hasMarketDataPermission,
                                (string)data.UnderLyingSymbol,
                                (string)data.RequestID,
                                message.CompanyUserID
                            );
                        }
                        // Case 2: Security Validation 
                        else
                        {
                            string serializedSymbols = data.serializedSymbols;
                            bool hasMarketDataPermission = data.hasMarketDataPermission;
                            string requestId = data.requestId;
                            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                            if(data.symbology != null)
                            {
                                symbology = (ApplicationConstants.SymbologyCodes)Enum.Parse(typeof(ApplicationConstants.SymbologyCodes), (string)data.symbology);
                            }

                            ValidateSymbols(
                                JsonHelper.DeserializeToObject<List<string>>(serializedSymbols),
                                hasMarketDataPermission,
                                string.Empty,
                                requestId,
                                message.CompanyUserID,
                                symbology
                            );

                            Logger.LogMsg(LoggerLevel.Debug, "SecurityValidationRequestReceived request processed successfully in {0} ms", sw.ElapsedMilliseconds);
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
        }


        /// <summary>
        /// Validates a collection of symbols using their symbology codes.
        /// Optionally includes a request ID and company user ID for tracking purposes.
        /// </summary>
        /// <param name="symbols">An enumerable collection of tuples containing symbols and their associated symbology codes.</param>
        /// /// <param name="isMarketDataPermissionEnabled">isMarketDataPermissionEnabled</param>
        /// <param name="requestID">The unique identifier for the validation request (optional).</param>
        /// <param name="companyUserID">The ID of the company user initiating the validation (optional).</param>
        private void ValidateSymbols(List<string> symbols, bool isMarketDataPermissionEnabled, string underlyingSymbol = "", string requestID = "", int companyUserID = 0, ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol)
        {
            try
            {
                if (_securityMaster != null && _securityMaster.IsConnected)
                {
                    string uniqueKeyRequestID = companyUserID + "_" + requestID;
                    SecMasterRequestObj reqObj = new SecMasterRequestObj();

                    // Remove existing cache entries for the request ID
                    foreach (var key in _securityValidationCache.Keys.Where(key => key.Contains(uniqueKeyRequestID)).ToList())
                    {
                        _securityValidationCache.TryRemove(key, out _);
                    }

                    // Add symbols to the request object and update the cache
                    foreach (string symbol in symbols)
                    {
                        if (!string.IsNullOrWhiteSpace(symbol))
                        {
                            reqObj.AddData(symbol.Trim(), symbology);

                            string uniqueKeyWithSymbol = uniqueKeyRequestID + "_" + symbol.Trim();

                            if (!_securityValidationCache.ContainsKey(uniqueKeyRequestID))
                            {
                                _securityValidationCache.TryAdd(uniqueKeyWithSymbol, symbol.Trim());
                            }
                        }
                    }

                    // Set additional properties for the request object
                    reqObj.HashCode = GetHashCode();
                    reqObj.RequestID = requestID;
                    reqObj.UserID = companyUserID.ToString();
                    reqObj.IsSearchInLocalOnly = !isMarketDataPermissionEnabled;

                    if (_equityOption)
                        reqObj.UseOptionManualvalidation = true;
                    if (!string.IsNullOrEmpty(underlyingSymbol))
                        reqObj.SymbolDataRowCollection[0].UnderlyingSymbol = underlyingSymbol;

                    // Send the validation request
                    _securityMaster.SendRequest(reqObj);
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
        /// Serch security with criteria
        /// </summary>
        private void SymbolSearch(string startWith, int hashCode, int userID)
        {
            if (_securityMaster != null)
            {
                try
                {
                    SecMasterSymbolSearchReq searchReq = new SecMasterSymbolSearchReq(startWith, ApplicationConstants.SymbologyCodes.TickerSymbol)
                    {
                        HashCode = hashCode,
                        UserID = userID
                    };

                    if (_securityMaster != null && _securityMaster.IsConnected)
                    {
                        _securityMaster.searchSymbols(searchReq);
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
        }

        /// <summary>
        /// Sends the validation response to Kafka topics.
        /// </summary>
        /// <param name="validationResponse">The validation response to be sent.</param>
        /// <param name="requestedCompanyUserID">The ID of the company user requesting the validation.</param>
        /// <remarks>
        private async void SendValidationResponse(string validationResponse, int requestedCompanyUserID)
        {
            try
            {
                if (!string.IsNullOrEmpty(validationResponse))
                {
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDRequest, new RequestResponseModel(0, validationResponse));
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SecurityValidationResponse, new RequestResponseModel(requestedCompanyUserID, validationResponse));
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

        /// <summary>  
        /// Retrieves a list of cache keys that match the specified security master object properties.  
        /// This method compares various symbol properties of the provided <see cref="SecMasterBaseObj"/>  
        /// with the values stored in the security validation cache and returns the matching keys.  
        /// </summary>  
        /// <param name="secMasterBaseObj">The security master object containing symbol properties to match.</param>  
        /// <returns>A list of cache keys that match the specified security master object properties.</returns>  
        private List<string> GetKeysByValue(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                return _securityValidationCache
                    .Where(kvp =>
                        kvp.Value.Equals(secMasterBaseObj.TickerSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.IDCOOptionSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.SedolSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.ISINSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.BloombergSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.BloombergSymbolWithExchangeCode.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.CusipSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.OpraSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.ReutersSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.FactSetSymbol.ToUpper()) ||
                        kvp.Value.Equals(secMasterBaseObj.ActivSymbol.ToUpper()) ||
                        (!string.IsNullOrWhiteSpace(secMasterBaseObj.BBGID) && kvp.Value.Equals(secMasterBaseObj.BBGID.ToUpper()))
                    )
                    .Select(kvp => kvp.Key)
                    .ToList();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return new List<string>();
        }
        #endregion

        #region Kafka Methods
        /// <summary>
        /// Creation of Equity Option Symbol validation variable.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_EquityOptionManualValidationResponseReceived(string topic, RequestResponseModel message)
        {
            try
            {
                if (!_compliancePermissionsResponseReceivedFromCommonData)
                {
                    _compliancePermissionsResponseReceivedFromCommonData = true;
                    Logger.LogMsg(LoggerLevel.Information, "Successfully connected Common Data Service.");
                }
                _equityOption = Convert.ToBoolean(message.Data.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the loggedin Information cache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserWiseLoggedInInformationReceived(string topic, RequestResponseModel message)
        {
            try
            {
                CompanyUser _loggedInInformation = JsonHelper.DeserializeToObject<CompanyUser>(message.Data);
                if (!_userWiseLoggedInUserInformation.ContainsKey(message.CompanyUserID))
                {
                    _userWiseLoggedInUserInformation.Add(message.CompanyUserID, _loggedInInformation);
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
        /// Gets the logged in user information
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserLoggedInInformation(string topic, RequestResponseModel message)
        {
            try
            {
                if (!_loggedInUserResponseReceivedFromAuth)
                {
                    _loggedInUserResponseReceivedFromAuth = true;
                    Logger.LogMsg(LoggerLevel.Information, "Successfully connected Auth Service.");
                }
                Dictionary<int, AuthenticatedUserInfo> loggedInUsersReceived = JsonHelper.DeserializeToObject<Dictionary<int, AuthenticatedUserInfo>>(message.Data);

                foreach (var kvp in loggedInUsersReceived)
                {
                    if (loggedInUsersReceived[kvp.Key] != null)
                    {
                        if (!_userWiseLoggedInUserInformation.ContainsKey(kvp.Key) && loggedInUsersReceived[kvp.Key].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                            _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseLoggedInInformationRequest, new RequestResponseModel(kvp.Key, ""));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Clearing Cache for logged out user
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_InitializeLoggedOutUsers(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserID = message.CompanyUserID;

                if (_userWiseLoggedInUserInformation.ContainsKey(companyUserID))
                    _userWiseLoggedInUserInformation.Remove(companyUserID);


                // Remove cache entries related to the logged-out user  
                var keysToRemove = _securityValidationCache.Keys
                    .Where(key => key.StartsWith($"{companyUserID}_"))
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    _securityValidationCache.TryRemove(key, out _);
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
        /// Creates the new/update symbol object for SM save
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SMSaveNewSymbolRequestReceived(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    SecMasterbaseList lst = new SecMasterbaseList();
                    lst.RequestID = System.Guid.NewGuid().ToString();
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    SecMasterUIObj uiObj = new SecMasterUIObj();
                    uiObj.AssetID = data.AssetID;
                    uiObj.ExchangeID = data.ExchangeID;
                    uiObj.UnderLyingID = data.UnderlyingID;
                    uiObj.CurrencyID = data.CurrencyID;
                    uiObj.TickerSymbol = data.TickerSymbol;
                    uiObj.Multiplier = data.Multiplier;
                    uiObj.LongName = data.Description;
                    uiObj.UnderLyingSymbol = data.UnderlyingSymbol;
                    uiObj.BloombergSymbol = data.BloombergSymbol;
                    uiObj.FactSetSymbol = data.FactsetSymbol;
                    uiObj.ActivSymbol = data.ActivSymbol;
                    uiObj.AUECID = data.AUECID;
                    uiObj.ExpirationDate = data.ExpirationDate;
                    uiObj.RoundLot = data.RoundLot;
                    uiObj.SedolSymbol = data.SedolSymbol;

                    uiObj.IsSecApproved = true;
                    if (uiObj.AssetID == (int)AssetCategory.EquityOption)
                    {
                        uiObj.StrikePrice = data.StrikePrice;
                        uiObj.PutOrCall = (int)(OptionType)data.PutAndCall;
                    }
                    uiObj.SymbolType = Int32.Parse(Convert.ToString(data.SecurityAction)) == 0 ? SymbolType.New : SymbolType.Updated;
                    if (uiObj.SymbolType == SymbolType.Updated)
                    {
                        uiObj.Symbol_PK = Int64.Parse(Convert.ToString(data.Symbol_PK));
                    }
                    else
                    {
                        uiObj.Symbol_PK = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        uiObj.ApprovalDate = uiObj.CreationDate = uiObj.ModifiedDate = DateTime.Now;
                        if (_userWiseLoggedInUserInformation.ContainsKey(message.CompanyUserID))
                        {
                            uiObj.CreatedBy = uiObj.ApprovedBy = _userWiseLoggedInUserInformation[message.CompanyUserID].ShortName + '_' + _userWiseLoggedInUserInformation[message.CompanyUserID].CompanyName;
                        }
                    }
                    uiObj.DataSource = SecMasterConstants.SecMasterSourceOfData.SymbolLookup;

                    SecMasterBaseObj secMasterBaseObj = GetSecMasterObjFromUIObject(uiObj);
                    if (secMasterBaseObj != null)
                    {
                        secMasterBaseObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.SymbolLookup;


                        // update symbol type to unchanged for new symbol after it is saved sucessfully, PRANA-9269
                        if (uiObj.SymbolType != SymbolType.New)
                            uiObj.SymbolType = BusinessObjects.AppConstants.SymbolType.Unchanged;

                        if (uiObj.SymbolType == BusinessObjects.AppConstants.SymbolType.Updated)
                        {
                            if (_userWiseLoggedInUserInformation.ContainsKey(message.CompanyUserID))
                            {
                                secMasterBaseObj.ModifiedBy = _userWiseLoggedInUserInformation[message.CompanyUserID].ShortName + "_" + _userWiseLoggedInUserInformation[message.CompanyUserID].CompanyName;
                            }
                            secMasterBaseObj.ModifiedDate = DateTime.Now;
                        }

                        lst.Add(secMasterBaseObj);

                        if (lst.Count > 0)
                        {
                            _securityMaster.SaveNewSymbols(lst);
                        }
                    }
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SMSaveNewSymbolResponse, message);

                    Logger.LogMsg(LoggerLevel.Debug, "SMSaveNewSymbolRequestReceived request processed successfully in {0} ms for tickerSymbol {1}",
                        sw.ElapsedMilliseconds, uiObj.TickerSymbol);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SMSaveNewSymbolResponse);
                }
            }
        }

        /// <summary>
        /// Security search request for SM
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SMSecuritySearchRequestReceived(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    SMSecuritySearchRequest(message.Data);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SMSecuritySearchRequestReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Security search in DB request
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SecuritySearchRequestReceived(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    int hashcode = Convert.ToInt32(data.HashCode);
                    string symbol = Convert.ToString(data.Symbol);
                    SymbolSearch(symbol, hashcode, message.CompanyUserID);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SecuritySearchRequestReceived encountered an error"); ;
                }
            }
        }

        /// <summary>
        /// Update logged in user data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UpdateLoggedInUserData(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserId = message.CompanyUserID;
                if (companyUserId != 0 && companyUserId != int.MinValue)
                {
                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseLoggedInInformationRequest, new RequestResponseModel(companyUserId, ""));
                    Logger.LogMsg(LoggerLevel.Information, "Kafka topic UserWiseLoggedInInformationRequest produced for user:{0}", companyUserId);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        #region Proxy for trade subscription
        /// <summary>
        /// Proxy for Trade subscription.
        /// </summary>
        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>(EndPointAddressConstants.CONST_TradeSubscriptionEndpoint, this);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "MakeProxy encountered an error"); ;
            }
        }

        /// <summary>
        /// Publish message method on the basis of topic
        /// </summary>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                lock (_locker)
                {
                    object[] dataList = (object[])e.EventData;
                    switch (e.TopicName)
                    {
                        case Topics.Topic_SecurityMaster:

                            foreach (object obj in dataList)
                            {
                                SecMasterBaseObj secMasterObj = (SecMasterBaseObj)obj;
                                RequestResponseModel response = new RequestResponseModel(0, JsonHelper.SerializeObject(secMasterObj));
                                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SMSymbolUpdateResponse, response);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Publish encountered an error"); ;
            }
        }

        public string getReceiverUniqueName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// UnSubscribeProxy
        /// </summary>
        internal void UnSubscribeProxy()
        {
            try
            {
                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                    _proxy.Dispose();
                    _proxy = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UnSubscribeProxy encountered an error");
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
                else if (_SecurityValidationServiceHeartbeatManager != null && !isRetryRequest)
                {
                    // Subscribe failed
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Subscribe encountered an error");
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
                Logger.LogError(ex, "UnSubscribe encountered an error");
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
                    if (_SecurityValidationServiceHeartbeatManager != null)
                        _SecurityValidationServiceHeartbeatManager.Dispose();
                    if (_securityMaster != null)
                    {
                        _securityMaster.SecMstrDataResponse -= _securityMaster_SecMstrDataResponse;
                        _securityMaster.Disconnected -= _securityMaster_Disconnected;
                        _securityMaster.Connected -= _securityMaster_Connected;
                        _securityMaster.SecMstrDataSymbolSearcResponse -= _securityMaster_SecMstrDataSymbolSearcResponse;
                        _securityMaster.SymbolLookUpDataResponse -= _securityMaster_SymbolLookUpDataResponse;
                    }
                    if (_proxy != null)
                    {
                        _proxy.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }
        #endregion

        #region Kafka Reporter
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
    }
}
