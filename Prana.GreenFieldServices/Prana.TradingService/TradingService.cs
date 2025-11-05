using Castle.Windsor;
using Prana.ATDLLibrary.Model.Elements;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.ATDLLibrary.Model.Reference;
using Prana.ATDLLibrary.Providers;
using Prana.Authentication.Common;
using Prana.BusinessLogic;
using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.Permissions;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.KafkaWrapper;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prana.SocketCommunication;
using Prana.TradeManager.Extension;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.ComponentModel;

using static Prana.KafkaWrapper.Extension.Classes.KafkaConstants;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using Prana.BusinessObjects.Classes;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.Allocation.ClientLibrary.DataAccess;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using Prana.GreenfieldServices.Common;
using Prana.BusinessObjects.Constants;

namespace Prana.TradingService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class TradingService : BaseService, ITradingService, IDisposable, ILiveFeedCallback
    {
        #region Variables
        private IWindsorContainer _container;
        private ServerHeartbeatManager _TradingServiceHeartbeatManager;
        //private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;
        //private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        ProxyBase<IAllocationManager> _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");

        /// <summary>
        /// Trade Status Message for currently ongoing trade
        /// </summary>
        private Dictionary<int, string> TradeStatusWithMessage = new Dictionary<int, string>();

        /// <summary>
        /// Database Manager instance 
        /// </summary>
        private IClientsCommonDataManager _clientsCommonDataManager;

        /// <summary>
        /// Static instance for TradeManagerExtension
        /// </summary>
        private static TradeManagerExtension _tradeManagerExtension;

        /// <summary>
        /// Static instance for UserTradeCacheManager
        /// </summary>
        private static UserTradeCacheManager _userTradeCacheManager;

        /// <summary>
        /// Static instance for UserTradeCacheManager
        /// </summary>
        private static ShortLocateManager _shortLocateManager;

        /// <summary>
        /// Algo-Strategy based  instance
        /// </summary>
        private StrategyProvider _strategyProvider = new StrategyProvider();

        /// <summary>
        /// The _allocation operation preference
        /// </summary>
        private AllocationOperationPreference _allocationOperationPreference = null;

        /// <summary>
        /// Collection of userwise permitted Accounts .
        /// </summary>
        Dictionary<int, AccountCollection> _userWiseAccountCollection = new Dictionary<int, AccountCollection>();

        /// <summary>
        /// Collection of userwise permitted AUECCV .
        /// </summary>
        Dictionary<int, List<string>> _UserWisePermittedAUECCVCollection = new Dictionary<int, List<string>>();

        /// <summary>
        /// Collection of venue .
        /// </summary>
        Dictionary<string, Dictionary<int, string>> venueCollection = new Dictionary<string, Dictionary<int, string>>();

        /// <summary>
        /// For Connecting to Server
        /// </summary>
        ICommunicationManager _tradeCommManager;

        /// <summary>
        /// UserWise- Symbol based Timer cache for ExPNL compression
        /// </summary>
        static Dictionary<string, Dictionary<string, Timer>> SymbolCompressionCache = new Dictionary<string, Dictionary<string, Timer>>();

        /// <summary>
        /// Collecting logged in user preference
        /// </summary>
        dynamic _userPreference = null;

        /// <summary>
        /// Compliance permissions data
        /// </summary>
        Dictionary<string, string> _compliancePermissionsData = new Dictionary<string, string>();

        /// <summary>
        /// Compliance permissions for user
        /// </summary>
        Dictionary<int, CompliancePermissions> _compliancePermissions = new Dictionary<int, CompliancePermissions>();

        /// <summary>
        /// User wise pre trade compliance check permission
        /// </summary>
        Dictionary<int, bool> _preTradeModule = new Dictionary<int, bool>();

        #region Company-Wise Preferences
        static List<DefTTControlsMapping> listTTControlsMapping = new List<DefTTControlsMapping>();
        bool IsDuplicateTradeAllowed = false;
        int DuplicateTradeTimer = 0;

        /// <summary>
        /// Tracks whether the compliance permissions response has been received from the Common Data Service.
        /// True if received; otherwise, false.
        /// </summary>
        bool _compliancePermissionsResponseReceivedFromCommonData = false;
        /// <summary>
        /// Tracks whether the logged-in user response has been received from Auth Service
        /// True if received; otherwise, false.
        /// </summary>
        bool _loggedInUserResponseReceivedFromAuth = false;

        #endregion

        /// <summary>
        /// Stores information of Users logged into Web application.
        /// </summary>
        private static Dictionary<int, object> _dictLoggedInUser = new Dictionary<int, object>();

        /// <summary>
        /// User wise Duplicate Trade Response
        /// </summary>
        Dictionary<int, string> userWiseDuplicateTradeResponse = new Dictionary<int, string>();

        /// <summary>
        /// Logged in users wise TT collection Data.
        /// </summary>
        private Dictionary<int, string> _userWiseTTCollectionData = new Dictionary<int, string>();

        /// <summary>
        /// Custom Allocation grid table
        /// </summary>
        DataTable custAllocationTable = new DataTable();

        /// <summary>
        /// Pricing server proxy
        /// </summary>
        private DuplexProxyBase<IPricingService> _pricingServicesProxy = null;

        /// <summary>
        /// Currency pair fx rate cache
        /// </summary>
        private Dictionary<string, double> _currencyPairFxRateDict = new Dictionary<string, double>();

        /// <summary>
        /// Trade service heartbeat manager
        /// </summary>
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;

        /// <summary>
        /// Pricing service heartbeat manager
        /// </summary>
        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;

        /// <summary>
        /// ExPnl service heartbeat manager
        /// </summary>
        private ClientHeartbeatManager<IExpnlService> _expnlServiceClientHeartbeatManager;

        /// <summary>
        /// Heart beat interval from config
        /// </summary>
        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

        private int _cleanedUp = 0;

        #endregion

        private const string MANUAL = "Manual", STAGE = "Stage", SEND = "Live";

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
                UpdateServiceStatus(ServiceNameConstants.CONST_Trading_Name, ServiceNameConstants.CONST_Trading_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_Trading_Name);
                var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);

                //for expnl service
                bool isExpnlLive = GetServiceStatus(ServiceNameConstants.CONST_Expnl_Name).IsLive;
                UpdateServiceStatus(ServiceNameConstants.CONST_Expnl_Name, ServiceNameConstants.CONST_Expnl_DisplayName, isExpnlLive);
                serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_Expnl_Name);
                message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
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
                Logger.LogMsg(LoggerLevel.Debug, "Information recieved in GetInstance_InformationReceived()");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetInstance_InformationReceived encountered an error");
            }
        }

        /// <summary>
        /// Connection To Trade Server
        /// </summary>
        private void ConnectToAllSockets()
        {
            try
            {
                _tradeCommManager = new ClientTradeCommManager();

                _tradeCommManager.Disconnected += new EventHandler(_communicationManager_Disconnected);
                _tradeCommManager.Connected += new EventHandler(_communicationManager_Connected);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ConnectToAllSockets encounter an error");
                throw;
            }
        }

        private void _communicationManager_Connected(object sender, EventArgs e)
        {
        }

        private void _communicationManager_Disconnected(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Set Coonection details from config
        /// </summary>
        private ConnectionProperties getTradeServerConnectionDetails()
        {
            ConnectionProperties connProperties = new ConnectionProperties();
            try
            {
                connProperties.Port = ClientAppConfiguration.TradeServer.Port;
                connProperties.ServerIPAddress = ClientAppConfiguration.TradeServer.IpAddress;
                connProperties.User = null;
                connProperties.IdentifierID = "TradingService_SocketConn";
                connProperties.IdentifierName = "Trading Service SocketConn";
                connProperties.ConnectedServerName = "Trade ";
                connProperties.HandlerType = HandlerType.TradeHandler;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "getTradeServerConnectionDetails encounter an error");
                throw;
            }
            Logger.LogMsg(LoggerLevel.Information,
                "Connection Properties for trade server are port:{0}, ip:{1}",
                Convert.ToString(connProperties.Port), connProperties.ServerIPAddress);

            return connProperties;
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
        /// ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Information, PranaMessageConstants.MSG_AnotherInstanceSubscribed);
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
        /// ExpnlServiceClientHeartbeatManager_ConnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpnlServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Expnl_Name, ServiceNameConstants.CONST_Expnl_DisplayName, true);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// ExpnlServiceClientHeartbeatManager_DisconnectedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpnlServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Expnl_Name, ServiceNameConstants.CONST_Expnl_DisplayName, false);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }


        #endregion

        #region IPranaServiceCommon Methods
        public async System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container)
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

                _expnlServiceClientHeartbeatManager = new ClientHeartbeatManager<IExpnlService>("ExpnlServiceEndpointAddress");
                _expnlServiceClientHeartbeatManager.ConnectedEvent += ExpnlServiceClientHeartbeatManager_ConnectedEvent;
                _expnlServiceClientHeartbeatManager.DisconnectedEvent += ExpnlServiceClientHeartbeatManager_DisconnectedEvent;
                _expnlServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;
                #endregion

                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                MakeProxy();
                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                #region CommonDataManager Intialization
                WindsorContainerManager.Container = container;
                _clientsCommonDataManager = _container.Resolve<IClientsCommonDataManager>();
                #endregion

                #region CustomAllocationDataSchema
                string[] columnNames = { TradingServiceConstants.CONST_ACCOUNT_NAME, TradingServiceConstants.CONST_CURRENT_QUANTITY, TradingServiceConstants.CONST_CURRENT_PERCENT, TradingServiceConstants.CONST_ALLOCATED_QUANTITY, TradingServiceConstants.CONST_ALLOCATED_PERCENT };
                foreach (string columnName in columnNames)
                {
                    DataColumn dtaColumn = new DataColumn();
                    dtaColumn.ColumnName = columnName;
                    if (columnName.Equals(TradingServiceConstants.CONST_ACCOUNT_NAME))
                        dtaColumn.DataType = typeof(string);
                    else
                        dtaColumn.DataType = typeof(double);
                    custAllocationTable.Columns.Add(dtaColumn);
                }
                #endregion

                #region Loading Mappings And Setting Up Socket Connection
                ConnectToAllSockets();
                Fix.FixDictionary.FixDictionaryHelper.LoadFixDictionary();
                TTHelperManagerExtension.GetInstance().GetCVAccountMappings(CachedDataManager.GetInstance.GetCompanyID());
                TTHelperManagerExtension.GetInstance().GetAccountCounterPartyVenueMappings(CachedDataManager.GetInstance.GetCompanyID());
                #endregion

                #region Server Heartbeat Setup
                _TradingServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                #region TradeManagerExtension
                _tradeManagerExtension = TradeManagerExtension.GetInstance();
                _userTradeCacheManager = UserTradeCacheManager.GetInstance();
                _shortLocateManager = ShortLocateManager.GetInstance();
                TradeManagerExtension.GetInstance().SetCommunicationManager = _tradeCommManager;
                _tradeCommManager.Connect(getTradeServerConnectionDetails());
                _tradeManagerExtension.CounterPartyStatusUpdate += _tradeManager_CounterPartyStatusUpdate;
                _tradeManagerExtension.CheckForDuplicateTradeEvent += CheckForDuplicateTrade;
                #endregion

                #region Kafka-Subscription
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompliancePermissionsResponse, KafkaManager_CompliancePermissionsResponse);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_UserLoggedInInformation);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_InitializeLoggedOutUsers);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseAllocationDataResponse, KafkaManager_UserWiseAllocation);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingPreferencesResponse, KafkaManager_CompanyTradingPreferencesReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWisePermittedAUECCVResponse, KafkaManager_UserWisePermittedAUECCV);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendLiveOrderRequest, KafkaManager_SendLiveOrderMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendManualOrderRequest, KafkaManager_SendManualOrderMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderRequest, KafkaManager_SendStageOrderMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SymbolAccountWisePositionRequest, KafkaManager_SymbolAccountWisePositionReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesRequest, KafkaManager_CompanyUserHotKeyPreferenceReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesRequest, KafkaManager_UpdateCompanyUserHotKeyPreferenceReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsRequest, KafkaManager_CompanyUserHotKeyPreferencesDetailsReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UpdateCompanyUserHotKeyPreferencesDetailsRequest, KafkaManager_UpdateCompanyUserHotKeyPreferencesDetailsReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveCompanyUserHotKeyPreferencesDetailsRequest, KafkaManager_SaveCompanyUserHotKeyPreferencesDetailsReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteCompanyUserHotKeyPreferencesDetailsRequest, KafkaManager_DeleteCompanyUserHotKeyPreferencesDetailsReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendReplaceOrderRequest, KafkaManager_SendReplaceOrderMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerWiseAlgoStrategiesRequest, KafkaManager_BrokerWiseAlgoStrategiesRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CustomAllocationDetailsRequest, KafkaManager_CustomAllocationDetails);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SavedCustomAllocationDetailsRequest, KafkaManager_SavedCustomAllocationDetails);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_FetchSavedCustomAllocationDetailsBulkRequest, KafkaManager_SavedCustomAllocationDetailsBulk);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetSMDetailsRequest, KafkaManager_GetSMDetailsMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CreateOptionSymbolRequest, KafkaManager_CreateOptionSymbolReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePopUpTextRequest, KafkaManager_CreatePopUpTextReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UnsubscribeSymbolCompressionFeedRequest, KafkaManager_UnSubscribeSymbolCompressionRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BrokerConnectionAndVenuesDataRequest, KafkaManager_BrokerConnectionAndVenuesDataRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_ShortLocateOrdersSymbolWiseRequest, KafkaManager_ShortLocateOrdersFilteredBySymbolRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineSecurityBorrowTypeRequest, KafkaManager_DetermineSecurtiesBorrowType);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetRegionOfBrokerFrSymbolAUECIDRequest, KafkaManger_GetRegionOfBrokerFrSymbolAUECIDRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_ProcessDataForCheckComplianceFromBasket, KafkaManager_ProcessDataForCheckCompliance);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CreatePstAllocatonPreferenceRequest, KafkaManager_CreatePstAllocacatioPrefRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_PSTOrderRequest, KafkaManager_PSTOrderRequestReceived);
                KafkaManager.Instance.SubscribeAndConsume(TOPIC_BookAsSwapRequest, KafkaManager_BookAsSwapReplaceMessageReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetTradeAttributeValues, KafkaManager_GetTradeAttributeValues);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendOrdersToMarketRequest, KafkaManager_SendOrdersToMarketRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoggedInUserResponse, KafkaManager_UpdateLoggedInUserData);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DetermineMultipleSecurityBorrowTypeRequest, KafkaManager_DetermineMultipleSecuritiesBorrowType);

                #endregion
                #region AlgoRelated
                LoadAlgoStrategies();
                #endregion

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");

                Logger.LogMsg(LoggerLevel.Information, "Trading Service initialisation completed in {0}", sw.ElapsedMilliseconds);
                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                // fire and forget method for handling of auth service connection
                ServiceConnectionPoller.PollUntilServiceReady(() => _compliancePermissionsResponseReceivedFromCommonData, KafkaConstants.TOPIC_CompliancePermissionsRequest, ServiceNameConstants.CONST_CommonData_DisplayName);
                ServiceConnectionPoller.PollUntilServiceReady(() => _loggedInUserResponseReceivedFromAuth, KafkaConstants.TOPIC_InitializeLoggedInUserRequest, ServiceNameConstants.CONST_Auth_DisplayName);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitialiseService encounter an error");
            }

            return false;
        }

        /// <summary>
        /// Trading Service Clean Up
        /// </summary>
        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.
            if (Interlocked.Exchange(ref _cleanedUp, 1) == 1) return;

            // 1) Stop periodic callbacks BEFORE disposing anything they use
            StopServiceHealthPollingTimer();

            // 2) Mark down (and publish a final “down”)
            UpdateServiceStatus(ServiceNameConstants.CONST_Trading_Name, ServiceNameConstants.CONST_Trading_DisplayName, false);

            Console.WriteLine("Shutting down service.");

            Logger.LogMsg(LoggerLevel.Fatal, "Shutting down service...");
            _container.Dispose();
            Logger.LogMsg(LoggerLevel.Information, "TradingService successfully closed");
        }
        #endregion

        #region Return trading ticket data for dock call
        /// <summary>
        /// Sending Combined Broker and Venue details
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_BrokerConnectionAndVenuesDataRequestReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    if (_userWiseTTCollectionData.ContainsKey(request.CompanyUserID))
                    {

                        Dictionary<string, string> tradingTicketData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(_userWiseTTCollectionData[request.CompanyUserID]);
                        tradingTicketData.Add("BrokerConnectionStatus", JsonHelper.SerializeObject(_tradeManagerExtension.GetAllCounterPartyConnectionSatus().Values));
                        tradingTicketData.Add("BrokerAlgoStrategies", JsonHelper.SerializeObject(_strategyProvider.GetAllStrategiesAlgoInfo()));

                        var result = new { data = tradingTicketData, tradingTicketId = request.Data };
                        request.Data = JsonHelper.SerializeObject(result);

                        await KafkaManager.Instance
                            .Produce(TOPIC_BrokerConnectionAndVenuesDataResponse, request);
                        KafkaManager_ProduceTradingTicketData(request.CompanyUserID);

                        Logger.LogMsg(LoggerLevel.Information, "BrokerConnection and venue data request has been processed successfully");
                    }
                    else
                    {
                        //Logger.LogMsg(LoggerLevel.Fatal, "Problem in cache Creations for trading service, Please restart the service...");
                        throw new Exception("Problem in cache Creations for trading service, Please logout and relogin");
                    }
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(request, ex, TOPIC_BrokerConnectionAndVenuesDataResponse);
                }
            }
        }
        #endregion

        #region Custom Allocation Handling
        /// <summary>
        /// Get saved custom Allocation from DB
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SavedCustomAllocationDetails(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    Logger.LogMsg(LoggerLevel.Debug, "Get allocation preference (method SavedCustomAllocationDetails) request received with preferenceId {0}", info?.preferenceId);
                    AllocationOperationPreference operationPreference = _allocationProxy
                        .InnerChannel
                        .GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), message.CompanyUserID, Convert.ToInt32(info.preferenceId));

                    var data = new { savedCustomAllocationDetails = operationPreference == null ? string.Empty : JsonHelper.SerializeObject(operationPreference), tradingTicketId = info.tradingTicketId };

                    message.Data = JsonHelper.SerializeObject(data);

                    await KafkaManager.Instance.Produce(TOPIC_SavedCustomAllocationDetailsResponse, message);

                    Logger.LogMsg(LoggerLevel.Debug, "Get allocation preference (method SavedCustomAllocationDetails) request process successfully for preferenceId {0}", info?.preferenceId);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_SavedCustomAllocationDetailsResponse);
                }
            }
        }


        /// <summary>
        /// Get saved custom Allocation from DB from multiple preferenceIds
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SavedCustomAllocationDetailsBulk(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    Logger.LogMsg(LoggerLevel.Debug, "Get allocation preferences (method SavedCustomAllocationDetailsBulk) request received with preferenceIds {0}", info?.preferenceId);
                    int companyId = CachedDataManager.GetInstance.GetCompanyID();
                    int companyUserId = message.CompanyUserID;

                    // Expecting preferenceIds to be an array in the request
                    List<int> preferenceIds = ((IEnumerable<dynamic>)info.preferenceIds).Select(id => (int)id).ToList();
                    string tradingTicketId = info.tradingTicketId;

                    var allPreferences = new List<object>();

                    foreach (int preferenceId in preferenceIds)
                    {
                        AllocationOperationPreference operationPreference = _allocationProxy
                            .InnerChannel
                            .GetPreferenceById(companyId, companyUserId, preferenceId);

                        if (operationPreference != null)
                        {
                            allPreferences.Add(operationPreference);
                        }
                    }

                    var data = new
                    {
                        savedCustomAllocationDetails = JsonHelper.SerializeObject(allPreferences),
                        tradingTicketId
                    };

                    message.Data = JsonHelper.SerializeObject(data);

                    await KafkaManager.Instance.Produce(TOPIC_SavedCustomAllocationDetailsResponse, message);


                    Logger.LogMsg(LoggerLevel.Debug, "Get allocation preferences (method SavedCustomAllocationDetailsBulk) request has been processed successfully for preferenceIds {0}", info?.preferenceId);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_SavedCustomAllocationDetailsResponse);
                }
            }
        }


        /// <summary>
        /// Save Custom Allocation in DB and return the same
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_CustomAllocationDetails(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                PreferenceUpdateResult result;
                try
                {
                    Logger.LogMsg(LoggerLevel.Debug, "Get Custom Allocation request has been received");

                    Dictionary<string, object> responseData = JsonHelper
                        .DeserializeToObject<Dictionary<string, object>>(message.Data);

                    List<AccountValue> accountValues = new List<AccountValue>();
                    bool isOpendFromRTPNL = Convert.ToBoolean(responseData["IsOpenedFromRTPNL"].ToString());
                    string viewId = responseData["ViewId"].ToString();

                    if (!isOpendFromRTPNL)
                    {
                        List<dynamic> rows = JsonHelper.DeserializeToList<dynamic>(responseData["Griddata"]
                            .ToString());

                        custAllocationTable.Clear();
                        foreach (dynamic row in rows)
                        {
                            custAllocationTable.Rows.Add(
                                row[TradingServiceConstants.CONST_ACCOUNT_NAME],
                                row[TradingServiceConstants.CONST_CURRENT_QUANTITY],
                                row[TradingServiceConstants.CONST_CURRENT_PERCENT],
                                row[TradingServiceConstants.CONST_ALLOCATED_QUANTITY],
                                row[TradingServiceConstants.CONST_ALLOCATED_PERCENT]);
                        }
                    }
                    else
                    {
                        Dictionary<string, double> accountRows = JsonHelper.DeserializeToObject<Dictionary<string, double>>(
                            responseData["PositionWiseArray"].ToString());

                        double totalPositionforRTPNL = Convert.ToDouble(responseData["TotalPositionforRTPNL"]
                            .ToString());

                        foreach (string Key in accountRows.Keys)
                        {
                            decimal allocatedpercent = (decimal)Math.Abs((accountRows[Key] / totalPositionforRTPNL) * 100);
                            if (allocatedpercent > 0)
                            {
                                accountValues.Add(new AccountValue(Convert.ToInt32(Key), allocatedpercent));
                            }
                        }

                        Logger.LogMsg(LoggerLevel.Debug, "Total Position for RTPL count is {0}", totalPositionforRTPNL);
                    }

                    string symbol = responseData["Symbol"].ToString();
                    bool isCustomAllocationUpdated = Convert.ToBoolean(responseData["IsCustomAllocationUpdated"]
                        .ToString());

                    int operationID = Convert.ToInt32(responseData["OperationID"]
                        .ToString());

                    if (_allocationProxy == null)
                    {
                        _allocationProxy = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                    }

                    if (!isCustomAllocationUpdated)
                    {
                        _allocationOperationPreference = null;
                    }
                    else
                    {
                        if (operationID != -1)
                            _allocationOperationPreference = _allocationProxy.InnerChannel
                                .GetPreferenceById(
                                    CachedDataManager.GetInstance.GetCompanyID(),
                                    message.CompanyUserID,
                                    operationID);
                        else
                            _allocationOperationPreference = null;
                    }

                    if (_allocationOperationPreference == null)
                    {
                        string prefName = "*Custom#_" + symbol + "_" + message.CompanyUserID + "_" + DateTime.Now.Ticks;

                        result = _allocationProxy.InnerChannel.AddPreference(
                            prefName,
                            CachedDataManager.GetInstance.GetCompanyID(),
                            AllocationPreferencesType.CalculatedAllocationPreference,
                            false);

                        AllocationOperationPreference pref = result.Preference;
                        _allocationOperationPreference = pref;
                    }

                    SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();

                    if (!isOpendFromRTPNL)
                    {
                        foreach (DataRow dr in custAllocationTable.Rows)
                        {
                            if (Decimal.Parse(dr[TradingServiceConstants.CONST_ALLOCATED_PERCENT].ToString()) > 0)
                            {
                                AccountValue fv = new AccountValue(CachedDataManager.GetInstance.GetAccountID(
                                    dr[TradingServiceConstants.CONST_ACCOUNT_NAME].ToString()),
                                    Decimal.Parse(dr[TradingServiceConstants.CONST_ALLOCATED_PERCENT].ToString()));

                                fv.StrategyValueList.Add(new StrategyValue(0, 100, 0));

                                targetPercs.Add(CachedDataManager.GetInstance.GetAccountID(dr[TradingServiceConstants.CONST_ACCOUNT_NAME].ToString()), fv);
                            }
                        }
                    }
                    else
                    {
                        foreach (var accountValue in accountValues)
                        {
                            if (!targetPercs.ContainsKey(accountValue.AccountId))
                            {
                                // adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                                accountValue.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                                targetPercs.Add(accountValue.AccountId, accountValue);
                            }
                        }
                    }
                    _allocationOperationPreference.TryUpdateTargetPercentage(targetPercs);

                    //Set Default rule for Allocation
                    AllocationRule defaulfRule = new AllocationRule();
                    defaulfRule.BaseType = AllocationBaseType.CumQuantity;
                    defaulfRule.RuleType = MatchingRuleType.None;
                    defaulfRule.PreferenceAccountId = -1;
                    defaulfRule.MatchClosingTransaction = MatchClosingTransactionType.None;
                    _allocationOperationPreference.TryUpdateDefaultRule(defaulfRule);

                    result = _allocationProxy.InnerChannel.UpdatePreference(_allocationOperationPreference);

                    var response = new { CustomAllocationData = result, ViewId = viewId };
                    message.Data = JsonHelper.SerializeObject(response);
                    await KafkaManager.Instance.Produce(TOPIC_CustomAllocationDetailsResponse, message);

                    Logger.LogMsg(LoggerLevel.Debug, "Get Custom Allocation request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_CustomAllocationDetailsResponse);
                }
            }
        }
        #endregion

        #region Subscribe/Unsubscribe proxy
        /// <summary>
        /// Subscribe PRice subscription proxy
        /// </summary>
        private void MakeProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
                Logger.LogMsg(LoggerLevel.Information, "Created PricingServiceProxy successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "MakeProxy encountered an error");

            }
        }

        /// <summary>
        /// UnSubscribe Price subscription proxy
        /// </summary>
        internal void UnSubscribeProxy()
        {
            try
            {
                if (_pricingServicesProxy != null)
                {
                    _pricingServicesProxy.Dispose();
                    _pricingServicesProxy = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UnSubscribeProxy encountered an error");
            }
        }
        #endregion

        #region ExPNL Symbol Compression
        /// <summary>
        /// Async Timer method for ExPNL compression
        /// </summary>
        /// <param name="object"></param>
        private async void SymbolAccountWisePosition(object o)
        {
            RequestResponseModel message = new RequestResponseModel(0, string.Empty, string.Empty);
            try
            {
                dynamic data = o;
                string symbolName = data[0];

                Logger.LogMsg(LoggerLevel.Verbose, "Processing SymbolAccountWisePosition  for symbol {0}", symbolName);

                int currencyID = Convert.ToInt32(data[1]);
                int companyUserID = Convert.ToInt32(data[2]);
                string correlationId = "";

                try { correlationId = data[3]; } catch (Exception) { }

                List<int> ListofIDs = new List<int>();
                if (_userWiseAccountCollection.ContainsKey(companyUserID))
                {
                    ListofIDs = (from Account Account in _userWiseAccountCollection[companyUserID]
                                 select Account.AccountID).ToList();
                    ListofIDs.Remove(Int32.MinValue);
                    ListofIDs.Add(-1);
                }

                StringBuilder errorMessage = new StringBuilder();
                if (!string.IsNullOrEmpty(symbolName) && ListofIDs.Count > 0)
                {
                    Dictionary<int, decimal> dictAccountWisePosition = ExpnlServiceConnector.GetInstance()
                        .GetPositionForSymbolAndAccounts(symbolName, ListofIDs, ref errorMessage);

                    Dictionary<string, string> finalDict = new Dictionary<string, string>();
                    string _fxOperator;
                    decimal _fxRate = 1;
                    int companyCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();

                    switch (CachedDataManager.GetInstance.GetCurrencyText(currencyID))
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                        case "USD":
                            _fxOperator = TradingServiceConstants.CONST_FXOPERATOR_MULTIPLY; // Operator M
                            break;

                        default:
                            _fxOperator = TradingServiceConstants.CONST_FXOPERATOR_DIVIDE; // Operator D
                            break;
                    }

                    if (currencyID != companyCurrencyID)
                    {
                        int fromCurrencyID = _fxOperator.Equals(TradingServiceConstants.CONST_FXOPERATOR_DIVIDE) ? companyCurrencyID : currencyID;
                        int toCurrencyID = _fxOperator.Equals(TradingServiceConstants.CONST_FXOPERATOR_DIVIDE) ? currencyID : companyCurrencyID;
                        string forexSymbol = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetPranaForexSymbolFromCurrencies(fromCurrencyID, toCurrencyID);

                        fxInfo fxSymbolInfo = new fxInfo
                        {
                            FromCurrencyID = fromCurrencyID,
                            ToCurrencyID = toCurrencyID,
                            PranaSymbol = forexSymbol,
                            CategoryCode = AssetCategory.Forex
                        };

                        List<fxInfo> listFxSymbols = new List<fxInfo>() { fxSymbolInfo };
                        _pricingServicesProxy.InnerChannel.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, false, null, true);

                        _fxRate = _currencyPairFxRateDict.TryGetValue(forexSymbol, out var rate) ? Convert.ToDecimal(rate) : RequestFXSnapshot(currencyID);
                    }
                    else
                    {
                        _fxRate = 1;
                    }

                    finalDict.Add("Symbol", symbolName);
                    finalDict.Add("accountWisePosition", JsonHelper.SerializeObject(dictAccountWisePosition));
                    finalDict.Add("_fxRate", JsonHelper.SerializeObject(_fxRate));
                    finalDict.Add("_fxOperator", JsonHelper.SerializeObject(_fxOperator));

                    Dictionary<int, decimal> dictAccountWiseExposure = ExpnlServiceConnector.GetInstance()
                        .GetGrossExposureForSymbolAndAccounts(symbolName, ListofIDs, ref errorMessage);

                    finalDict.Add("accountWiseExposure", JsonHelper.SerializeObject(dictAccountWiseExposure));

                    Dictionary<int, decimal> dictAccountWiseDayPNL = ExpnlServiceConnector.GetInstance()
                        .GetDayPNLForSymbolAndAccounts(symbolName, ListofIDs, ref errorMessage);

                    finalDict.Add("accountWiseDayPNL", JsonHelper.SerializeObject(dictAccountWiseDayPNL));

                    message = new RequestResponseModel(0, string.Empty, correlationId);

                    message.Data = JsonHelper.SerializeObject(finalDict);

                    SendUserWiseMessageIfSymbolSubscribed(symbolName, message);

                }
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, TOPIC_SymbolAccountWisePositionResponse);
            }
        }

        /// <summary>
        /// To return a requestID that is subscribed to a symbol and doesn't have alloted timer 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="currentRequestedID"></param>
        private string ReturnRequestIDSubscribingSymbol(string symbol, string currentRequestedID)
        {
            try
            {
                foreach (string requestID in SymbolCompressionCache.Keys)
                {
                    if (!currentRequestedID.Equals(requestID) && SymbolCompressionCache[requestID].ContainsKey(symbol) && SymbolCompressionCache[requestID][symbol] == null)
                        return requestID;
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
            return string.Empty;
        }

        /// <summary>
        /// To check if symbol is subscribed by other instances (excluding itself)
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="requestUserID"></param>
        private bool CheckIfSymbolSubscribedByOtherUsers(string symbol, string requestUserID)
        {
            try
            {
                foreach (string RequestID in SymbolCompressionCache.Keys)
                {
                    if (!RequestID.Equals(requestUserID))
                    {
                        if (SymbolCompressionCache[RequestID].ContainsKey(symbol))
                            return true;
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
            return false;
        }

        /// <summary>
        /// To check if symbol is subscribed by any instances (including itself)
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="requestUserID"></param>
        private bool CheckIfSymbolSubscribed(string symbol)
        {
            try
            {
                foreach (string requestID in SymbolCompressionCache.Keys)
                {
                    if (SymbolCompressionCache[requestID].ContainsKey(symbol))
                    {
                        return true;
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
            return false;
        }

        /// <summary>
        /// Sends a message to all users subscribed to a specific symbol.
        /// </summary>
        /// <param name="symbol">The symbol for which the message is being sent.</param>
        /// <param name="message">The message to be sent to the subscribed users.</param>
        /// <remarks>
        /// This method iterates through the `SymbolCompressionCache` to find all users subscribed to the given symbol.
        /// For each user, it extracts the user ID from the request ID and sends the provided message to the Kafka topic
        /// `TOPIC_SymbolAccountWisePositionResponse`.
        /// </remarks>
        private async void SendUserWiseMessageIfSymbolSubscribed(string symbol, RequestResponseModel message)
        {
            try
            {
                foreach (string requestID in SymbolCompressionCache.Keys)
                {
                    if (SymbolCompressionCache[requestID].ContainsKey(symbol))
                    {
                        string[] splitRequestID = requestID.Split('_');
                        if (int.TryParse(splitRequestID[0], out int userID))
                        {
                            message.CompanyUserID = userID;
                            await KafkaManager.Instance.Produce(TOPIC_SymbolAccountWisePositionResponse, message);
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
        /// Clears and disposes all symbol compression cache entries for a specific company user.
        /// </summary>
        /// <param name="companyUserID">The company user ID whose symbol compression cache should be cleared.</param>
        /// <remarks>
        /// This method iterates through the <c>SymbolCompressionCache</c> dictionary and identifies all entries
        /// associated with the specified <paramref name="companyUserID"/>. For each matching entry:
        /// <list type="bullet">
        /// <item>Disposes the associated <see cref="Timer"/> object if it is not used by other users.</item>
        /// <item>If the timer is shared with other users, it is reassigned to another request ID; otherwise, it is disposed.</item>
        /// <item>Clears the inner dictionary and removes the entry from the cache.</item>
        /// </list>
        /// </remarks>
        private void ClearSymbolCompressionCache(int companyUserID)
        {
            try
            {
                // Create a list to store keys to remove to avoid modifying the collection during iteration
                List<string> keysToRemove = new List<string>();

                foreach (string requestID in SymbolCompressionCache.Keys)
                {
                    string[] splitRequestID = requestID.Split('_');
                    if (splitRequestID.Length > 0 && int.TryParse(splitRequestID[0], out int userID))
                    {
                        if (companyUserID == userID)
                        {
                            // Add the key to the removal list
                            keysToRemove.Add(requestID);

                            // Dispose of and clear the inner dictionary
                            if (SymbolCompressionCache[requestID] != null)
                            {
                                string subscribedSymbol = SymbolCompressionCache[requestID].Keys.FirstOrDefault();
                                if (!subscribedSymbol.Equals(string.Empty))
                                {
                                    if (!CheckIfSymbolSubscribedByOtherUsers(subscribedSymbol, requestID))
                                    {
                                        var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                        if (_timer != null)
                                        {
                                            _timer.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                        if (_timer != null)
                                        {
                                            string newRequestIDToBeAssigned = ReturnRequestIDSubscribingSymbol(subscribedSymbol, requestID);
                                            if (!newRequestIDToBeAssigned.Equals(string.Empty))
                                                SymbolCompressionCache[newRequestIDToBeAssigned][subscribedSymbol] = _timer;
                                            else
                                                _timer.Dispose();
                                        }
                                    }
                                }
                                SymbolCompressionCache[requestID].Clear(); // Clear the inner dictionary
                            }
                        }
                    }
                }

                // Remove keys after iteration to avoid runtime exceptions
                foreach (string key in keysToRemove)
                {
                    SymbolCompressionCache.Remove(key);
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
        /// UnSubscribe ExPNL compression for symbol based on SymbolCompressionCache
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UnSubscribeSymbolCompressionRequestReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                lock (SymbolCompressionCache)
                {
                    try
                    {
                        dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                        string requestID = data.RequestedInstance;

                        if (SymbolCompressionCache.ContainsKey(requestID))
                        {
                            var subscribedSymbolDictionary = SymbolCompressionCache[requestID];
                            var subscribedSymbol = SymbolCompressionCache[requestID].Keys.FirstOrDefault();
                            if (!CheckIfSymbolSubscribedByOtherUsers(subscribedSymbol, requestID))
                            {
                                var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                if (_timer != null)
                                {
                                    _timer.Dispose();
                                }
                            }
                            else
                            {
                                var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                if (_timer != null)
                                {
                                    string newRequestIDToBeAssigned = ReturnRequestIDSubscribingSymbol(subscribedSymbol, requestID);
                                    SymbolCompressionCache[newRequestIDToBeAssigned][subscribedSymbol] = _timer;
                                }
                            }
                            Dictionary<string, Timer> symbolWiseTimer = new Dictionary<string, Timer>();
                            symbolWiseTimer.Add(String.Empty, null);
                            SymbolCompressionCache[requestID] = symbolWiseTimer;
                            Logger.LogMsg(LoggerLevel.Information, "UnSubscribe Symbol request has been processed successfully for instance:{0}", requestID);
                        }
                        else
                            Logger.LogMsg(LoggerLevel.Debug, "RequestId {0} not present in SymbolCompressionCache", requestID);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "KafkaManager_UnSubscribeSymbolCompressionRequestReceived encountered an error for instanceId:" + message.Data);
                    }
                }
            }
        }

        /// <summary>
        /// Requests the fx snapshot.
        /// </summary>
        /// <param name="currencyID">The currency identifier.</param>
        private decimal RequestFXSnapshot(int currencyID)
        {
            decimal _fxRate = 0;
            try
            {
                ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID())
                    .GetConversionRateFromCurrencies(currencyID, CachedDataManager.GetInstance.GetCompanyBaseCurrencyID(), 0);

                if (conversionRate != null)
                {
                    if (conversionRate.RateValue > double.Epsilon)
                    {
                        _fxRate = (decimal)conversionRate.RateValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestFXSnapshot encountered an error");
            }
            return _fxRate;
        }

        /// <summary>
        /// Fetch Symbol wise position from EXPNL service connector
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SymbolAccountWisePositionReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    lock (SymbolCompressionCache)
                    {
                        Logger.LogMsg(LoggerLevel.Debug, "KafkaManager_SymbolAccountWisePositionReceived request has been received");
                        dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                        string symbolName = data.Symbol;
                        int currencyID = data.CurrencyID;
                        string requestID = data.RequestID;
                        string[] frontendData = {
                            symbolName,
                            currencyID.ToString(),
                            message.CompanyUserID.ToString(),
                            message.CorrelationId
                        };

                        Logger.LogMsg(LoggerLevel.Debug, "Initiating symbol {0} compression...", symbolName);

                        if (SymbolCompressionCache.ContainsKey(requestID))
                        {
                            var subscribedSymbolDictionary = SymbolCompressionCache[requestID];
                            var subscribedSymbol = SymbolCompressionCache[requestID].Keys.FirstOrDefault();

                            if (!subscribedSymbol.Equals(symbolName))
                            {
                                if (!CheckIfSymbolSubscribed(symbolName))
                                {
                                    if (!CheckIfSymbolSubscribedByOtherUsers(subscribedSymbol, requestID))
                                    {
                                        var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                        if (_timer != null)
                                        {
                                            _timer.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                        if (_timer != null)
                                        {
                                            string newRequestIDToBeAssigned = ReturnRequestIDSubscribingSymbol(subscribedSymbol, requestID);
                                            SymbolCompressionCache[newRequestIDToBeAssigned][subscribedSymbol] = _timer;
                                        }
                                    }
                                    Dictionary<string, Timer> symbolWiseTimer = new Dictionary<string, Timer>();
                                    Timer timer = new Timer(SymbolAccountWisePosition, frontendData, 0, 2000);
                                    symbolWiseTimer.Add(symbolName, timer);
                                    SymbolCompressionCache[requestID] = symbolWiseTimer;
                                }
                                else
                                {
                                    if (!CheckIfSymbolSubscribedByOtherUsers(subscribedSymbol, requestID))
                                    {
                                        var _timer = SymbolCompressionCache[requestID][subscribedSymbol];
                                        if (_timer != null)
                                        {
                                            _timer.Dispose();
                                        }
                                    }
                                    Dictionary<string, Timer> symbolWiseTimer = new Dictionary<string, Timer>() { { symbolName, null } };
                                    SymbolCompressionCache[requestID] = symbolWiseTimer;
                                }
                            }
                        }
                        else
                        {
                            Logger.LogMsg(LoggerLevel.Debug, "Symbol {0} does not exists in SymbolCompressionCache", symbolName);
                            if (!CheckIfSymbolSubscribed(symbolName))
                            {
                                Dictionary<string, Timer> symbolWiseTimer = new Dictionary<string, Timer>();
                                Timer timer = new Timer(SymbolAccountWisePosition, frontendData, 0, 2000);
                                symbolWiseTimer.Add(symbolName, timer);
                                SymbolCompressionCache.Add(requestID, symbolWiseTimer);
                            }
                            else
                            {
                                Dictionary<string, Timer> symbolWiseTimer = new Dictionary<string, Timer>() { { symbolName, null } };
                                SymbolCompressionCache.Add(requestID, symbolWiseTimer);
                            }
                        }

                        Logger.LogMsg(LoggerLevel.Information, "KafkaManager_SymbolAccountWisePositionReceived request has been processed");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_SymbolAccountWisePositionReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Fetch Company Hot Key Preferences for User
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompanyUserHotKeyPreferenceReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    int companyUserID = data.CompanyUserID;
                    CompanyUserHotKeyPreferences(companyUserID, message.CorrelationId);
                    Logger.LogMsg(LoggerLevel.Debug,
                        "CompanyUserHotKeyPreferenceReceived request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CompanyUserHotKeyPreferenceReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Update Company Hot Key Preferences for User
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UpdateCompanyUserHotKeyPreferenceReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    int companyUserID = data.CompanyUserID;
                    bool enableBookMarkIcon = data.EnableBookMarkIcon;
                    bool hotKeyOrderChanged = data.HotKeyOrderChanged;
                    bool tTTogglePreferenceForWeb = data.TTTogglePreferenceForWeb;
                    string hotKeyPreferenceElements = data.HotKeyPreferenceElements;

                    TTHelperManagerExtension.GetInstance().UpdateCompanyUserHotKeyPreferences(companyUserID, enableBookMarkIcon, hotKeyOrderChanged, tTTogglePreferenceForWeb, hotKeyPreferenceElements);

                    CompanyUserHotKeyPreferences(companyUserID, message.CorrelationId);

                    Logger.LogMsg(LoggerLevel.Debug, "UpdateCompanyUserHotKeyPreferenceReceived request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UpdateCompanyUserHotKeyPreferenceReceived encountered an error");
                }
            }
        }

        private async void CompanyUserHotKeyPreferences(int companyUserID, string correlationId)
        {
            RequestResponseModel responseMessage = new RequestResponseModel(companyUserID, string.Empty, correlationId);
            try
            {
                TTHelperManagerExtension.GetInstance().GetCompanyUserHotKeyPreferences(companyUserID);
                var ttHotKeyPreferences = TTHelperManagerExtension.GetInstance().CompanyUserHotKeyPreferences;
                responseMessage.Data = JsonHelper.SerializeObject(ttHotKeyPreferences);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesResponse, responseMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CompanyUserHotKeyPreferences encountered an error");
            }
        }

        /// <summary>
        /// Fetch Company Hot Key Preferences Details for User Hot Keys
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompanyUserHotKeyPreferencesDetailsReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    int companyUserID = data.CompanyUserID;
                    CompanyUserHotKeyPreferencesDetails(companyUserID, message.CorrelationId);
                    Logger.LogMsg(LoggerLevel.Debug,
                        "CompanyUserHotKeyPreferencesDetailsReceived request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CompanyUserHotKeyPreferencesDetailsReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Update Company Hot Key Preferences Details for User
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UpdateCompanyUserHotKeyPreferencesDetailsReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    int companyUserID = 0;
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    if (data.HotKeySequenceOrder != null)
                    {
                        foreach (var kv in data.HotKeySequenceOrder)
                        {
                            int companyUserHotKeyID = kv.CompanyUserHotKeyID;
                            companyUserID = kv.CompanyUserID;
                            int hotKeySequence = kv.HotKeySequence;
                            TTHelperManagerExtension.GetInstance().UpdateCompanyUserHotKeySequenceOrder(companyUserHotKeyID, companyUserID, hotKeySequence);
                        }
                    }
                    else
                    {
                        int companyUserHotKeyID = data.CompanyUserHotKeyID;
                        companyUserID = data.CompanyUserID;
                        string hotKeyPreferenceNameValue = data.HotKeyPreferenceNameValue;
                        string companyUserHotKeyName = data.CompanyUserHotKeyName;
                        bool isFavourites = data.IsFavourites;
                        int hotKeySequence = data.HotKeySequence;
                        TTHelperManagerExtension.GetInstance().UpdateCompanyUserHotKeyPreferencesDetails(companyUserHotKeyID, companyUserID, companyUserHotKeyName, hotKeyPreferenceNameValue, isFavourites, hotKeySequence);
                    }
                    CompanyUserHotKeyPreferencesDetails(companyUserID, message.CorrelationId);

                    Logger.LogMsg(LoggerLevel.Debug,
                         "UpdateCompanyUserHotKeyPreferencesDetailsReceived request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UpdateCompanyUserHotKeyPreferencesDetailsReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Save Company Hot Key Preferences Details for User
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SaveCompanyUserHotKeyPreferencesDetailsReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_SAVE_HOT_KEY_PREFERENCES_DETAILS_RECEIVED
                        + message.CompanyUserID.ToString());
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    int companyUserID = data.CompanyUserID;
                    string hotKeyPreferenceNameValue = data.HotKeyPreferenceNameValue;
                    string companyUserHotKeyName = data.CompanyUserHotKeyName;
                    bool isFavourites = data.IsFavourites;
                    int hotKeySequence = data.HotKeySequence;
                    string module = data.Module;
                    string hotButtonType = data.HotButtonType;
                    TTHelperManagerExtension.GetInstance().SaveCompanyUserHotKeyPreferencesDetails(companyUserID, companyUserHotKeyName, hotKeyPreferenceNameValue, isFavourites, hotKeySequence, module, hotButtonType);

                    CompanyUserHotKeyPreferencesDetails(companyUserID, message.CorrelationId);

                    Logger.LogMsg(LoggerLevel.Debug, "SaveCompanyUserHotKeyPreferencesDetails request processed successfully in {0} ms for hot key name {1}", sw.ElapsedMilliseconds, companyUserHotKeyName);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SaveCompanyUserHotKeyPreferencesDetailsReceived  encountered an error");
                }
            }
        }

        /// <summary>
        /// Delete Company Hot Key Preferences Details for User
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_DeleteCompanyUserHotKeyPreferencesDetailsReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_DELETE_HOT_KEY_PREFERENCES_DETAILS_RECEIVED
                        + message.CompanyUserID.ToString());
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    int companyUserID = data.CompanyUserID;
                    string companyUserHotKeyName = data.CompanyUserHotKeyName;
                    TTHelperManagerExtension.GetInstance().DeleteCompanyUserHotKeyPreferencesDetails(companyUserID, companyUserHotKeyName);

                    CompanyUserHotKeyPreferencesDetails(companyUserID, message.CorrelationId);

                    Logger.LogMsg(LoggerLevel.Debug, "DeleteCompanyUserHotKeyPreferencesDetails request processed successfully in {0} ms for hot key name {1}", sw.ElapsedMilliseconds, companyUserHotKeyName);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "DeleteCompanyUserHotKeyPreferencesDetailsReceived  encountered an error");
                }
            }
        }

        private async void CompanyUserHotKeyPreferencesDetails(int companyUserID, string correlationId)
        {
            RequestResponseModel responseMessage = new RequestResponseModel(companyUserID, string.Empty, correlationId);
            try
            {
                TTHelperManagerExtension.GetInstance().GetCompanyUserHotKeyPreferencesDetails(companyUserID);
                var ttHotKeyPreferencesDetails = TTHelperManagerExtension.GetInstance().CompanyUserHotKeyPreferencesDetails;
                responseMessage.Data = JsonHelper.SerializeObject(ttHotKeyPreferencesDetails);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompanyUserHotKeyPreferencesDetailsResponse, responseMessage);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(responseMessage, ex, TOPIC_SaveAllocationDetailsResponse);
            }
        }
        #endregion

        #region Internal use cache creation through greenfield services
        /// <summary>
        /// Fetch permitted AUECCV
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserWisePermittedAUECCV(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Debug, "User Permitted AUECCV request received");
                    if (!string.IsNullOrEmpty(message.Data))
                    {
                        List<string> _userPermittedAUECCV = new List<string>();
                        _userPermittedAUECCV = JsonHelper.DeserializeToObject<List<string>>(message.Data);
                        if (!_UserWisePermittedAUECCVCollection.ContainsKey(message.CompanyUserID))
                        {
                            _UserWisePermittedAUECCVCollection.Add(message.CompanyUserID, _userPermittedAUECCV);
                            InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_USER_PERMITTED_AUECCV_PROCESSED + message.CompanyUserID.ToString());
                        }
                    }
                    Logger.LogMsg(LoggerLevel.Information, "User Permitted AUECCV request has been processed successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_UserWisePermittedAUECCV encountered an error");
                }
            }
        }

        /// <summary>
        /// Fetch Binded Allocation List in TT
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserWiseAllocation(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Debug, "User wise allocation request has been received");
                    InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_USER_PERMITTED_ACCOUNTS_RECEIVED + message.CompanyUserID.ToString());
                    if (!string.IsNullOrEmpty(message.Data))
                    {
                        AccountCollection _accountIds = new AccountCollection();
                        var AccountCollection = JsonHelper.DeserializeToObject<IEnumerable<Account>>(message.Data);
                        foreach (Account account in AccountCollection)
                        {
                            _accountIds.Add(account);
                        }
                        if (!_userWiseAccountCollection.ContainsKey(message.CompanyUserID))
                        {
                            _userWiseAccountCollection.Add(message.CompanyUserID, _accountIds);

                        }
                    }
                    Logger.LogMsg(LoggerLevel.Information, "User wise allocation request has been processed successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_UserWiseAllocation encountered an error");
                }
            }
        }

        /// <summary>
        /// Fetch user wise Broker details and their conenction details
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private string SetupUserWiseBrokerDetails(int companyUserID)
        {

            try
            {
                Logger.LogMsg(LoggerLevel.Debug, "SetupUserWiseBrokerDetails request has been received");

                Dictionary<string, string> collection = new Dictionary<string, string>();

                var counterpartylist = _clientsCommonDataManager.GetCompanyUserCounterParties(companyUserID);
                foreach (CounterParty counterparty in counterpartylist.Cast<CounterParty>().ToList())
                {
                    if (counterparty.IsOTDorEMS)
                    {
                        counterpartylist.Remove(counterparty);
                    }
                }

                collection.Add("UserWiseBrokers", JsonHelper.SerializeObject(counterpartylist));
                
                #region CounterParties based on Asset And underlying
                //clearing
                TTHelperManagerExtension.GetInstance().VenueCollection.Clear();
                TTHelperManagerExtension.GetInstance().TTCollection.Clear();
                TTHelperManagerExtension.GetInstance().CounterPartyCollection.Clear();
                TTHelperManagerExtension.GetInstance().VenueCollectionQTT.Clear();
                TTHelperManagerExtension.GetInstance().CounterPartyCollectionQTT.Clear();

                TradeManagerExtension.GetInstance().UserID = companyUserID;
                int companyId = CachedDataManager.GetInstance.GetCompanyID();
                TTHelperManagerExtension.GetInstance().GetCVAUECMappings(companyId, companyUserID);

                Dictionary<string, Dictionary<int, string>> counterPartyValueListCollection = new Dictionary<string, Dictionary<int, string>>();
                Dictionary<string, Dictionary<int, string>> counterPartyVenueValueListCollection = new Dictionary<string, Dictionary<int, string>>();

                foreach (TTHelper tt in TTHelperManagerExtension.GetInstance().TTCollection)
                {
                    string key = TTHelperManagerExtension.GetInstance().AUECKey(tt.AuecID);
                    string keyQTT = TTHelperManagerExtension.GetInstance().AUKey(tt.AssetID, tt.UnderlyingID);
                    TTHelperManagerExtension.GetInstance().AddCounterparties(key, tt);
                    TTHelperManagerExtension.GetInstance().AddCounterpartiesQTT(keyQTT, tt);
                }

                foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in TTHelperManagerExtension.GetInstance().CounterPartyCollection)
                {
                    Dictionary<int, string> vl = new Dictionary<int, string>();
                    foreach (KeyValuePair<int, string> cp in keyvaluepair.Value)
                    {
                        vl.Add(cp.Key, cp.Value);
                    }
                    counterPartyValueListCollection.Add(keyvaluepair.Key, vl);
                }

                collection.Add("CounterPartyAUECCollection", JsonHelper.SerializeObject(counterPartyValueListCollection));
                #endregion

                collection.Add("AllocationWiseBroker", JsonHelper.SerializeObject(TTHelperManagerExtension.GetInstance().AccountWiseCounterPartyVenueCollection));

                InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_USER_PERMITTED_BROKER_PROCESSED + companyUserID.ToString());

                return JsonHelper.SerializeObject(collection);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SetupUserWiseBrokerDetails encountered an error");
            }
            return null;
        }

        /// <summary>
        /// Fetch Broker- Venue collection details
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private string SetupUserWise_BrokerWise_VenueDetails(int companyUserID)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Debug, "SetupUserWise_BrokerWise_VenueDetails request has been received");

                Dictionary<int, string> dictVenueCollectionQTT = new Dictionary<int, string>();
                Dictionary<string, Dictionary<int, string>> venueListCollectionQTT = new Dictionary<string, Dictionary<int, string>>();
                Dictionary<string, string> brokerVenuePairsQTT = new Dictionary<string, string>();

                Dictionary<int, string> dictVenueCollection = new Dictionary<int, string>();
                Dictionary<string, Dictionary<int, string>> venueListCollection = new Dictionary<string, Dictionary<int, string>>();
                Dictionary<string, string> brokerVenuePairs = new Dictionary<string, string>();

                #region Broker-Venue pairs for QTT
                SetupBrokerVenuePairCollection(TTHelperManagerExtension.GetInstance().VenueCollectionQTT, dictVenueCollectionQTT, venueListCollectionQTT, brokerVenuePairsQTT);
                #endregion

                #region CounterParties based on Asset And underlying
                SetupBrokerVenuePairCollection(TTHelperManagerExtension.GetInstance().VenueCollection, dictVenueCollection, venueListCollection, brokerVenuePairs);
                #endregion

                Dictionary<string, string> finalDict = new Dictionary<string, string>
                {
                    { "distinctVenues", JsonHelper.SerializeObject(dictVenueCollection) },
                    { "brokerVenuePairs", JsonHelper.SerializeObject(brokerVenuePairs) },
                    { "brokerVenuePairsQTT", JsonHelper.SerializeObject(brokerVenuePairsQTT) }
                };

                Logger.LogMsg(LoggerLevel.Information, "SetupUserWise_BrokerWise_VenueDetails request has been processed successfully");

                return JsonHelper.SerializeObject(finalDict);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SetupUserWise_BrokerWise_VenueDetails encountered an error");
            }
            return null;
        }

        /// <summary>
        /// SetupBrokerVenuePairCollection
        /// </summary>
        /// <param name="venueCollection"></param>
        /// <param name="dictVenueCollection"></param>
        /// <param name="venueListCollection"></param>
        /// <param name="brokerVenuePairs"></param>
        private void SetupBrokerVenuePairCollection(Dictionary<string, Dictionary<int, string>> venueCollection, Dictionary<int, string> dictVenueCollection, Dictionary<string, Dictionary<int, string>> venueListCollection, Dictionary<string, string> brokerVenuePairs)
        {
            try
            {
                foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in venueCollection)
                {
                    Dictionary<int, string> brokerVenues = new Dictionary<int, string>();
                    foreach (KeyValuePair<int, string> venue in keyvaluepair.Value)
                    {
                        brokerVenues.Add(venue.Key, venue.Value);
                        // Fill all the venues in the dictionary
                        if (!dictVenueCollection.ContainsKey(venue.Key))
                        {
                            dictVenueCollection.Add(venue.Key, venue.Value);
                        }
                    }
                    venueListCollection.Add(keyvaluepair.Key, brokerVenues);
                }
                string venuesSet;
                foreach (var Key in venueListCollection.Keys)
                {
                    Dictionary<int, string> bindedVenues = venueListCollection[Key];
                    venuesSet = "";
                    foreach (int venueId in bindedVenues.Keys)
                    {
                        venuesSet += venueId.ToString() + ",";
                    }
                    brokerVenuePairs.Add(Key, venuesSet.Substring(0, venuesSet.Length - 1));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while processing broker venue collection.");
            }
        }

        /// <summary>
        /// Create Company Preferences
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompanyTradingPreferencesReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Debug, "Company Wise Trading Preference request received");

                    if (!string.IsNullOrEmpty(message.Data))
                    {
                        _userPreference = JsonHelper.DeserializeToObject<dynamic>(message.Data);

                        dynamic _defTTControlsMapping = (_userPreference.CompanyTradingTicketUiPrefs.listTTControlsMapping);
                        dynamic _tradingTicketRulesPrefs = _userPreference.TradingTicketRulesPrefs;

                        IsDuplicateTradeAllowed = Convert.ToBoolean(_tradingTicketRulesPrefs.IsDuplicateTradeAlert);
                        DuplicateTradeTimer = Convert.ToInt32(_tradingTicketRulesPrefs.DuplicateTradeAlertTime);

                        listTTControlsMapping.Clear();
                        foreach (dynamic data in _defTTControlsMapping)
                        {
                            DefTTControlsMapping defTTControls = new DefTTControlsMapping();
                            defTTControls.FromControl = data.FromControl;
                            defTTControls.ToControl = data.ToControl;
                            listTTControlsMapping.Add(defTTControls);
                        }
                    }

                    Logger.LogMsg(LoggerLevel.Information, "Company Wise Trading Preference request has been processed");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_CompanyTradingPreferencesReceived encountered an error");
                }
            }
        }
        #endregion

        #region SM data binding
        /// <summary>
        /// Get SM details for binding
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_GetSMDetailsMessageReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "SM Data for binding request has been received");

                    Dictionary<string, string> finalDict = new Dictionary<string, string>();

                    string stringfiedValues = string.Empty;
                    Dictionary<int, string> dictAssets = CachedDataManager.GetInstance.GetAllAssets();
                    foreach (KeyValuePair<int, string> kvp in dictAssets)
                    {
                        stringfiedValues += kvp.Key.ToString() + ',' + kvp.Value + ';';
                    }
                    if (stringfiedValues.Length > 0)
                        stringfiedValues = stringfiedValues.Substring(0, stringfiedValues.Length - 1);
                    finalDict.Add("Asset", stringfiedValues);

                    stringfiedValues = string.Empty;
                    Dictionary<int, string> dictUnderlyings = CachedDataManager.GetInstance.GetAllUnderlyings();
                    foreach (KeyValuePair<int, string> kvp in dictUnderlyings)
                    {
                        stringfiedValues += kvp.Key.ToString() + ',' + kvp.Value + ';';
                    }
                    if (stringfiedValues.Length > 0)
                        stringfiedValues = stringfiedValues.Substring(0, stringfiedValues.Length - 1);
                    finalDict.Add("Underlying", stringfiedValues);

                    stringfiedValues = string.Empty;
                    Dictionary<int, string> dictExchanges = CachedDataManager.GetInstance.GetAllExchanges();
                    foreach (KeyValuePair<int, string> kvp in dictExchanges)
                    {
                        stringfiedValues += kvp.Key.ToString() + ',' + kvp.Value + ';';
                    }
                    if (stringfiedValues.Length > 0)
                        stringfiedValues = stringfiedValues.Substring(0, stringfiedValues.Length - 1);
                    finalDict.Add("Exchange", stringfiedValues);

                    stringfiedValues = string.Empty;
                    Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                    foreach (KeyValuePair<int, string> kvp in dictCurrencies)
                    {
                        stringfiedValues += kvp.Key.ToString() + ',' + kvp.Value + ';';
                    }
                    if (stringfiedValues.Length > 0)
                        stringfiedValues = stringfiedValues.Substring(0, stringfiedValues.Length - 1);
                    finalDict.Add("Currency", stringfiedValues);

                    Dictionary<int, string> dictAuecs = CachedDataManager.GetInstance.GetAllAuecs();
                    Dictionary<int, double> dictMultipliers = CachedDataManager.GetInstance.AuecMultipliers;
                    Dictionary<int, decimal> dictRoundLots = CommonDataCache.CachedDataManager.GetInstance.AuecRoundLot;
                    Dictionary<int, ValidAUEC> _dictAuec = new Dictionary<int, ValidAUEC>();

                    foreach (KeyValuePair<int, string> kvpAuec in dictAuecs)
                    {
                        string[] auecdetails = (kvpAuec.Value).Split(',');
                        int auecID = kvpAuec.Key;
                        ValidAUEC auecdetailwise = new ValidAUEC();

                        auecdetailwise.AuecID = auecID;
                        auecdetailwise.AssetID = int.Parse(auecdetails[0].ToString());
                        auecdetailwise.UnderlyingID = int.Parse(auecdetails[1].ToString());
                        auecdetailwise.ExchangeID = int.Parse(auecdetails[2].ToString());
                        auecdetailwise.CurrencyID = int.Parse(auecdetails[3].ToString());

                        auecdetailwise.Asset = CachedDataManager.GetInstance.GetAssetText(auecdetailwise.AssetID);
                        auecdetailwise.Underlying = CachedDataManager.GetInstance.GetUnderLyingText(auecdetailwise.UnderlyingID);
                        auecdetailwise.Exchange = CachedDataManager.GetInstance.GetExchangeText(auecdetailwise.ExchangeID);
                        auecdetailwise.DefaultCurrency = CachedDataManager.GetInstance.GetCurrencyText(auecdetailwise.CurrencyID);
                        auecdetailwise.ExchangeIdentifier = CachedDataManager.GetInstance.GetAUECText(auecID);
                        auecdetailwise.Multiplier = dictMultipliers[auecID];
                        auecdetailwise.RoundLot = dictRoundLots[auecID];
                        _dictAuec.Add(auecID, auecdetailwise);
                    }
                    stringfiedValues = string.Empty;
                    foreach (KeyValuePair<int, ValidAUEC> kvp in _dictAuec)
                    {
                        stringfiedValues += kvp.Key.ToString() + ',' + JsonHelper.SerializeObject(kvp.Value) + ';';
                    }
                    if (stringfiedValues.Length > 0)
                        stringfiedValues = stringfiedValues.Substring(0, stringfiedValues.Length - 1);
                    finalDict.Add("ExchangeIdentifier", stringfiedValues);

                    var result = new { SMDetails = finalDict, TradingTicketId = message.Data };
                    message.Data = JsonHelper.SerializeObject(result);

                    await KafkaManager.Instance.Produce(TOPIC_GetSMDetailsResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "SM Data for binding request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_GetSMDetailsResponse);
                }
            }
        }
        #endregion

        #region Create Popup text for Popups
        /// <summary>
        /// Create Popup text from Order object
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_CreatePopUpTextReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    OrderSingle orderSingle = new OrderSingle();
                    if (Convert.ToInt32(data.PranaMsgType) == -1)
                    {
                        orderSingle = CreateReplaceOrderFromData(message.CompanyUserID, data.ttOrderInfo, false);
                    }
                    else
                        orderSingle = CreateOrderFromJSON(data.ttOrderInfo, message.CompanyUserID, false, true);
                    string popUpText = string.Empty;
                    popUpText = ValidationManagerExtension.GetOrderText(orderSingle, true);
                    var result = new { popUpText = popUpText, TradingTicketId = data.TradingTicketId };
                    message.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_CreatePopUpTextResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "CreatePopUpTextReceived request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_AllocationWiseBrokerResponse);
                }
            }
        }
        #endregion 

        #region Equity Option symbol generation 
        /// <summary>
        /// Create Option Sybmol from Underlying Symbol
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_CreateOptionSymbolReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    var data = info.optionSymbolData;
                    OptionDetail optionDetail = new OptionDetail();
                    optionDetail.ExpirationDate = data.ExpirationDate;
                    optionDetail.StrikePrice = data.StrikePrice;
                    optionDetail.UnderlyingSymbol = data.UnderlyingSymbol;
                    optionDetail.Symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                    optionDetail.AssetCategory = data.Asset;
                    optionDetail.AUECID = data.AUECID;
                    optionDetail.OptionType = (OptionType)data.OptionType;
                    optionDetail.StrikePriceMultiplier = data.StrikePriceMultiplier;
                    optionDetail.EsignalOptionRoot = data.EsignalOptionRoot;
                    optionDetail.BloombergOptionRoot = data.BloombergOptionRoot;
                    OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                    if (!string.IsNullOrEmpty(optionDetail.Symbol))
                        message.Data = optionDetail.Symbol;
                    else
                        message.Data = string.Empty;

                    var result = new { OptionSymbol = message, TradingTicketId = info.TradingTicketId };
                    message.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_CreateOptionSymbolResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "CreateOptionSymbolReceived request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_CreateOptionSymbolResponse);
                }
            }
        }
        #endregion

        #region Trading operation methods

        /// <summary>
        /// Get Region of Broker from Symbol and AUECID.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManger_GetRegionOfBrokerFrSymbolAUECIDRequest(string topic, RequestResponseModel message)
        {
            try
            {
                dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                string countryISOCode = CachedDataManager.GetInstance.GetCountryISOCode(Convert.ToInt32(data.AUECID));
                IsoCountryCodeA3 countryCode;
                Enum.TryParse(countryISOCode, out countryCode);
                Region targetRegion = RegionsA3.GetRegionForCountry(countryCode);
                data.TargetRegion = targetRegion;
                message.Data = JsonHelper.SerializeObject(new { AUECID = data.AUECID, TargetRegion = data.TargetRegion, Symbol = data.TickerSymbol });
                await KafkaManager.Instance
                              .Produce(TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse, message);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, TOPIC_GetRegionOfBrokerFrSymbolAUECIDResponse);
            }
        }

        /// <summary>
        /// Send Book As Swap Replace Order
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private void KafkaManager_BookAsSwapReplaceMessageReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    _tradeManagerExtension.TradeStatus = string.Empty;
                    TradeStatusWithMessage.Clear();
                    _tradeManagerExtension.PermittedAUECCV = new List<string>();

                    dynamic dataObject = JsonHelper.DeserializeToObject<dynamic>(request.Data);

                    OrderSingle replacedOrder = CreateReplaceOrderFromData(request.CompanyUserID, dataObject, false, false, true);
                    replacedOrder.AvgPriceForCompliance = dataObject.Price;
                    replacedOrder.IsSamsaraUser = true;

                    SetTTPropertiesMapping(dataObject, replacedOrder);
                    replacedOrder.AvgPrice = 0;
                    ValidationManagerExtension.GetOrderDetails(replacedOrder);
                    replacedOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace;
                    replacedOrder.OrderStatus = TradingServiceConstants.CONST_ORDER_STATUS_PENDINGNEW;

                    Logger.LogMsg(LoggerLevel.Information, "Order received to Book As Swap for Symbol:{0}", replacedOrder.Symbol);

                    if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                    {
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (replacedOrder.IsUseCustodianBroker && replacedOrder.AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(replacedOrder.AccountBrokerMapping.ToString());
                        }
                        _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];
                        if ((!replacedOrder.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(replacedOrder)) || (replacedOrder.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(replacedOrder, accountBrokerMapping)))
                        {
                            _tradeManagerExtension.SendValidatedTrades(replacedOrder, true);
                            TradeStatusWithMessage.Add(0, "TradeSuccessful");
                            #region Save Audit trail
                            string originalValues = " Original Quantity:" + replacedOrder.Quantity + ", Avg Price:" + replacedOrder.AvgPrice + ", Order Type:" + replacedOrder.OrderType + ",Trade Date:" + replacedOrder.AUECLocalDate;
                            string newValues = "Replaced Quantity:" + replacedOrder.Quantity + ", Avg Price:" + replacedOrder.AvgPrice + ", Order Type:" + replacedOrder.OrderType + ",Trade Date:" + replacedOrder.AUECLocalDate;
                            TradeAuditActionType.ActionType action = string.IsNullOrEmpty(replacedOrder.StagedOrderID) ? TradeAuditActionType.ActionType.OrderReplaced : TradeAuditActionType.ActionType.SubOrderReplaced;
                            AddOrderDataAuditEntryAndSaveInDB(replacedOrder, action, request.CompanyUserID, originalValues, newValues);
                            #endregion
                        }
                        else
                        {
                            TradeStatusWithMessage.Add(0, _tradeManagerExtension.TradeStatus);
                        }
                    }
                    else
                    {
                        TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                    }

                    //request.Data = JsonHelper.SerializeObject(result);
                    //await KafkaManager.Instance.Produce(TOPIC_SendReplaceOrderResponse, request);

                    Logger.LogMsg(LoggerLevel.Information, "Book As Swap for Replace operation request has been processed successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "KafkaManager_BookAsSwapReplaceMessageReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Send replaced Order from TT
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_SendReplaceOrderMessageReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Replace order request received from userId: {UserId} with payload: {Payload}", request?.CompanyUserID, request?.Data);
                    _tradeManagerExtension.TradeStatus = string.Empty;
                    TradeStatusWithMessage.Clear();
                    _tradeManagerExtension.PermittedAUECCV = new List<string>();

                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var data = info.OrderInfos;

                    bool isSuborderReplaced = Convert.ToBoolean(data[2]);
                    bool isSuborderLive = Convert.ToBoolean(data[3]);

                    OrderSingle parentOrder = null;

                    OrderSingle originalOrder = CreateReplaceOrderFromData(request.CompanyUserID, data[0], false, true);
                    OrderSingle replacedOrder = CreateReplaceOrderFromData(request.CompanyUserID, data[1], isSuborderReplaced);
                    replacedOrder.AvgPriceForCompliance = data[1].Price;
                    replacedOrder.IsSamsaraUser = true;

                    Logger.LogMsg(LoggerLevel.Information, "Replace Order OrderSingle object created successfully for. Symbol: {0}, Quantity: {1}, AvgPriceForComp:{2},isSuborderReplaced:{3},isSuborderLive:{4} ",
                        replacedOrder?.Symbol, replacedOrder?.Quantity, replacedOrder?.AvgPriceForCompliance, isSuborderReplaced, isSuborderLive);

                    if (isSuborderReplaced)
                    {
                        if (isSuborderLive)
                        {
                            replacedOrder.PranaMsgType = 2;
                        }
                        parentOrder = CreateParentOrderForReplace(request.CompanyUserID, data[4]);
                        if (replacedOrder.MsgType == FIXConstants.MSGOrderCancelReplaceRequest && (replacedOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSub || replacedOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDNewSubChild || replacedOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManualSub || replacedOrder.PranaMsgType == (int)OrderFields.PranaMsgTypes.ORDManual))
                        {
                            Logger.LogMsg(
                                 LoggerLevel.Information,
                                 "Replace Order Pending Replace initiated for ParentOrder. MsgType: {MsgType}, PranaMsgType: {PranaMsgType}, ReplacedQty: {ReplacedQty}, OriginalQty: {OriginalQty}, UnsentQty: {UnsentQty}, UpdatedQty: {UpdatedQty}, ClOrderID: {ClOrderID}",
                                 replacedOrder?.MsgType, replacedOrder?.PranaMsgType, replacedOrder?.Quantity,
                                 originalOrder?.Quantity, parentOrder?.UnsentQty, parentOrder?.Quantity, parentOrder?.ClOrderID
                             );

                            if (Convert.ToDouble(replacedOrder.Quantity) - originalOrder.Quantity >= parentOrder.UnsentQty)
                            {
                                parentOrder.Quantity += Convert.ToDouble(replacedOrder.Quantity) - originalOrder.Quantity - parentOrder.UnsentQty;
                                parentOrder.ClientTime = DateTime.Now.ToLongTimeString();
                                parentOrder.OrderStatus = TagDatabaseManager.GetInstance.GetOrderStatusText(FIXConstants.ORDSTATUS_PendingReplace.ToString());
                                parentOrder.OrigClOrderID = parentOrder.ClOrderID;
                                parentOrder.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                                parentOrder.TransactionTime = DateTime.Now.ToUniversalTime();
                                parentOrder.CumQty = 0;
                                parentOrder.AvgPrice = 0;
                                parentOrder.TIF = replacedOrder.TIF;
                            }
                        }
                    }

                    SetTTPropertiesMapping(data[1], replacedOrder);
                    ValidationManagerExtension.GetOrderDetails(replacedOrder);
                    replacedOrder.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace;
                    replacedOrder.OrderStatus = TradingServiceConstants.CONST_ORDER_STATUS_PENDINGNEW;

                    //Added this if block in order to get the updated value of _userPermittedAUECCV if any case the collection is not created or cleared
                    CheckForUserWisePermittedAUECCCollections(request);

                    if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                    {
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (replacedOrder.IsUseCustodianBroker && replacedOrder.AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(replacedOrder.AccountBrokerMapping.ToString());
                        }
                        _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];
                        if ((!replacedOrder.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(replacedOrder)) || (replacedOrder.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(replacedOrder, accountBrokerMapping)))
                        {
                            if (isSuborderReplaced && Convert.ToDouble(replacedOrder.Quantity) - originalOrder.Quantity >= parentOrder.UnsentQty)
                                _tradeManagerExtension.SendValidatedTrades(parentOrder, true);
                            _tradeManagerExtension.SendValidatedTrades(replacedOrder, true);
                            TradeStatusWithMessage.Add(0, "TradeSuccessful");
                            #region Save Audit trail
                            string originalValues = " Original Quantity:" + originalOrder.Quantity + ", Avg Price:" + originalOrder.AvgPrice + ", Order Type:" + data[0].OrderType + ",Trade Date:" + originalOrder.AUECLocalDate;
                            string newValues = "Replaced Quantity:" + replacedOrder.Quantity + ", Avg Price:" + replacedOrder.AvgPrice + ", Order Type:" + replacedOrder.OrderType + ",Trade Date:" + replacedOrder.AUECLocalDate;
                            TradeAuditActionType.ActionType action = string.IsNullOrEmpty(replacedOrder.StagedOrderID) ? TradeAuditActionType.ActionType.OrderReplaced : TradeAuditActionType.ActionType.SubOrderReplaced;
                            AddOrderDataAuditEntryAndSaveInDB(replacedOrder, action, request.CompanyUserID, originalValues, newValues);
                            #endregion
                        }
                        else
                        {
                            TradeStatusWithMessage.Add(0, _tradeManagerExtension.TradeStatus);
                        }
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId data. Cache may be incomplete or corrupt for Replace Order Trade.");
                        TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                    }

                    var result = new { TradeStatusWithMessage = TradeStatusWithMessage, TradingTicketId = info.TradingTicketId };
                    request.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_SendReplaceOrderResponse, request);

                    Logger.LogMsg(LoggerLevel.Information, "Replace Order proccess successfully for symbol:{0} and status msg:{2}", replacedOrder?.Symbol, TradeStatusWithMessage);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(request, ex, TOPIC_SendReplaceOrderResponse);
                }
            }

        }

        /// <summary>
        /// Send Stage Order from TT
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_SendStageOrderMessageReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Stage order request received from userId: {UserId} with payload: {Payload}", request?.CompanyUserID, request?.Data);
                    //Only for Stage Order
                    _tradeManagerExtension.TradeStatus = string.Empty;
                    TradeStatusWithMessage.Clear();
                    _tradeManagerExtension.PermittedAUECCV = new List<string>();

                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var data = info.StageTradeHandler;

                    bool IsShortLocateParameterNotRequired = false;
                    if (!data[1].Value.Equals(string.Empty))
                    {
                        IsShortLocateParameterNotRequired = true;
                    }

                    bool isStageAllowedforComplianceAlert = data[1] == "YES";
                    OrderSingle orderSingle = CreateOrderFromJSON(data[0], request.CompanyUserID, false, IsShortLocateParameterNotRequired);
                    orderSingle.IsStageRequired = false;
                    orderSingle.IsInternalOrder = true;

                    orderSingle.AvgPriceForCompliance = data[0].Price;
                    orderSingle.IsSamsaraUser = true;

                    Logger.LogMsg(LoggerLevel.Information, "Stage order OrderSingle object created successfully for Symbol: {0}, Quantity: {1}, AvgPriceForComp:{2}", orderSingle?.Symbol, orderSingle?.Quantity, orderSingle?.AvgPriceForCompliance);

                    SetTTPropertiesMapping(data[0], orderSingle);

                    //Added this if block in order to get the updated value of _userPermittedAUECCV if any case the collection is not created or cleared
                    CheckForUserWisePermittedAUECCCollections(request);

                    if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                    {
                        _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (orderSingle.IsUseCustodianBroker && data[0].AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(data[0].AccountBrokerMapping.ToString());
                        }
                        if ((!orderSingle.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(orderSingle))
                            || (orderSingle.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(orderSingle, accountBrokerMapping)))
                        {
                            if (GetPreTradeCheckForStageOrders(request.CompanyUserID) && !isStageAllowedforComplianceAlert)
                            {
                                Logger.LogMsg(LoggerLevel.Information, "Stage Order GetPreTradeCheckForStageOrders passed for symbol:{0}, Qty:{1}", orderSingle?.Symbol, orderSingle.Quantity);

                                List<OrderSingle> orders = new List<OrderSingle>() { orderSingle };
                                if (orderSingle.AssetID != (int)AssetCategory.FX)
                                {
                                    Dictionary<string, string> dataForStageOrderComplianceAlert = new Dictionary<string, string>
                                {
                                    { "Order", JsonHelper.SerializeObject(orders) },
                                    { "userId", orderSingle.CompanyUserID.ToString() },
                                };
                                    _ = KafkaManager.Instance.Produce(TOPIC_SendStageOrderForComplianceAlerts, new RequestResponseModel(request.CompanyUserID, JsonHelper.SerializeObject(dataForStageOrderComplianceAlert), request.CorrelationId));
                                    TradeStatusWithMessage.Add(0, "StageOrderComplianceAlert");
                                }
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, "Stage Order with GetPreTradeCheckForStageOrders conditions false (compliance response)  for symbol:{0}, Qty:{1}", orderSingle?.Symbol, orderSingle.Quantity);
                                AlertPopUpType popUpType = AlertPopUpType.None;
                                if (data.Count == 3 && !string.IsNullOrEmpty(Convert.ToString(data[2])))
                                    popUpType = (AlertPopUpType)(Convert.ToInt32(data[2]));
                                if (popUpType != AlertPopUpType.PendingApproval)
                                    _tradeManagerExtension.SendValidatedTrades(orderSingle, true);
                                if (!GetPreTradeCheckForStageOrders(request.CompanyUserID))
                                    TradeStatusWithMessage.Add(0, "TradeSuccessful");
                                else
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Stage order sent for Compliance operation");
                                    TradeStatusWithMessage.Add(0, "TradeSuccessfulCompliance");
                                }
                            }
                        }
                        else
                        {
                            TradeStatusWithMessage.Add(0, _tradeManagerExtension.TradeStatus);
                        }
                    }
                    else
                    {
                        TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                    }
                    var result = new { TradeStatusWithMessage = TradeStatusWithMessage, TradingTicketId = info.TradingTicketId };
                    request.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_SendStageOrderResponse, request);

                    Logger.LogMsg(LoggerLevel.Information, "Stage Order proccess successfully for symbol:{0} and Qty:{1} and status msg:{2}",
                                orderSingle?.Symbol, orderSingle.Quantity, TradeStatusWithMessage);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(request, ex, TOPIC_SendStageOrderResponse);
                }

            }
        }

        /// <summary>
        /// Send Manaual Order from TT
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_SendManualOrderMessageReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request?.CorrelationId, request?.RequestID, request?.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Manual order request received from userId: {UserId} with payload: {Payload}", request?.CompanyUserID, request?.Data);
                    //Only for Manual Order
                    _tradeManagerExtension.TradeStatus = string.Empty;
                    TradeStatusWithMessage.Clear();
                    _tradeManagerExtension.PermittedAUECCV = new List<string>();

                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var data = info.ManualTradeHandler;
                    OrderSingle orderSingle = CreateOrderFromJSON(data[0], request.CompanyUserID, true);
                    orderSingle.IsManualOrder = orderSingle.IsInternalOrder = true;
                    //TODO: Adding this check in case venue is left blank from Samsara side
                    if (orderSingle.VenueID == 0)
                    {
                        Logger.LogMsg(LoggerLevel.Information, "Manual order VenueId is 0 for with Symbol: {0}, Quantity: {1}. Setting default venueId 1", orderSingle?.Symbol, orderSingle?.Quantity);
                        orderSingle.VenueID = 1;
                        orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(orderSingle.VenueID);
                    }

                    orderSingle.AvgPriceForCompliance = 0;
                    orderSingle.IsSamsaraUser = true;

                    SetTTPropertiesMapping(data[0], orderSingle);
                    if (!userWiseDuplicateTradeResponse.ContainsKey(request.CompanyUserID))
                    {
                        userWiseDuplicateTradeResponse.Add(request.CompanyUserID, Convert.ToString(data[1]));
                    }
                    else
                    {
                        userWiseDuplicateTradeResponse[request.CompanyUserID] = Convert.ToString(data[1]);
                    }

                    //Added this if block in order to get the updated value of _userPermittedAUECCV if any case the collection is not created or cleared
                    CheckForUserWisePermittedAUECCCollections(request);

                    Logger.LogMsg(LoggerLevel.Information, "Manual order OrderSingle object created successfully for Symbol: {0}, Quantity: {1}, AvgPriceForComp:{2}", orderSingle?.Symbol, orderSingle?.Quantity, orderSingle?.AvgPriceForCompliance);

                    if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                    {
                        _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (orderSingle.IsUseCustodianBroker && data[0].AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(data[0].AccountBrokerMapping.ToString());
                        }

                        if ((!orderSingle.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(orderSingle))
                            || (orderSingle.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(orderSingle, accountBrokerMapping)))
                        {
                            bool isallowed = _tradeManagerExtension.SendValidatedTrades(orderSingle, true);
                            if (isallowed)
                            {
                                if (!GetPreTradeCheckForManualOrders(request.CompanyUserID))
                                    TradeStatusWithMessage.Add(0, "TradeSuccessful");
                                else
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Manual Trade order sent for Compliance operation");
                                    TradeStatusWithMessage.Add(0, "TradeSuccessfulCompliance");
                                }
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, "SendValidatedTrades condition failed for manual order: Symbol: {0}");
                            }
                        }
                        else
                        {
                            TradeStatusWithMessage.Add(0, _tradeManagerExtension.TradeStatus);
                        }
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId data. Cache may be incomplete or corrupt for manualOrder Trade.");
                        TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                    }

                    userWiseDuplicateTradeResponse[request.CompanyUserID] = String.Empty;
                    var result = new { TradeStatusWithMessage = TradeStatusWithMessage, TradingTicketId = info.TradingTicketId };
                    request.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_SendManualOrderResponse, request);

                    Logger.LogMsg(LoggerLevel.Information, "Manual Order proccess successfully for symbol:{0} and Qty:{1} and status msg:{2}",
                            orderSingle?.Symbol, orderSingle.Quantity, TradeStatusWithMessage);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(request, ex, TOPIC_SendManualOrderResponse);
                }
            }
        }

        /// <summary>
        /// Send Live Order from TT
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_SendLiveOrderMessageReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request?.CorrelationId, request?.RequestID, request?.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Live order request received from userId: {UserId} with payload: {Payload}", request?.CompanyUserID, request?.Data);
                    //Only for Live Order
                    _tradeManagerExtension.TradeStatus = string.Empty;
                    TradeStatusWithMessage.Clear();
                    _tradeManagerExtension.PermittedAUECCV = new List<string>();

                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var data = info.LiveTradeHandler;

                    OrderSingle orderSingle = CreateOrderFromJSON(data[0], request.CompanyUserID);

                    //Adding this check in case venue is left blank from Samsara side
                    if (orderSingle.VenueID == 0)
                    {
                        Logger.LogMsg(LoggerLevel.Information, "Live order VenueId is 0 for  with Symbol: {0}, Quantity: {1}. Setting default venueId 1", orderSingle?.Symbol, orderSingle?.Quantity);
                        orderSingle.VenueID = 1;
                        orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(orderSingle.VenueID);
                    }

                    orderSingle.AvgPriceForCompliance = Convert.ToDouble(data[0].Price.ToString());
                    orderSingle.IsSamsaraUser = true;

                    Logger.LogMsg(LoggerLevel.Information, "live order OrderSingle object created successfully for Symbol: {0}, Quantity: {1}, AvgPriceForComp:{2}", orderSingle?.Symbol, orderSingle?.Quantity, orderSingle?.AvgPriceForCompliance);

                    if (!userWiseDuplicateTradeResponse.ContainsKey(request.CompanyUserID))
                    {
                        userWiseDuplicateTradeResponse.Add(request.CompanyUserID, data[1].ToString());
                    }
                    else
                    {
                        userWiseDuplicateTradeResponse[request.CompanyUserID] = data[1];
                    }

                    SetTTPropertiesMapping(data[0], orderSingle);

                    //Added this if block in order to get the updated value of _userPermittedAUECCV if any case the collection is not created or cleared
                    CheckForUserWisePermittedAUECCCollections(request);

                    if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                    {
                        _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (orderSingle.IsUseCustodianBroker && data[0].AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(data[0].AccountBrokerMapping.ToString());
                        }
                        if ((!orderSingle.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(orderSingle))
                            || (orderSingle.IsUseCustodianBroker && _tradeManagerExtension.CheckTradeConditions(orderSingle, accountBrokerMapping)))
                        {
                            bool isallowed = _tradeManagerExtension.SendValidatedTrades(orderSingle, true);//
                            if (isallowed)
                            {
                                if (!GetPreTradeCheckForManualOrders(request.CompanyUserID))
                                    TradeStatusWithMessage.Add(0, "TradeSuccessful");
                                else
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Live Trade order sent for Compliance operation");
                                    TradeStatusWithMessage.Add(0, "TradeSuccessfulCompliance");
                                }
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, "SendValidatedTrades condition failed for live order: Symbol: {0}");
                            }
                        }
                        else
                        {
                            TradeStatusWithMessage.Add(0, _tradeManagerExtension.TradeStatus);
                        }
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId data. Cache may be incomplete or corrupt for liveOrder Trade.");
                        TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                    }

                    userWiseDuplicateTradeResponse[request.CompanyUserID] = String.Empty;
                    var result = new { TradeStatusWithMessage = TradeStatusWithMessage, TradingTicketId = info.TradingTicketId };
                    request.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_SendLiveOrderResponse, request);

                    Logger.LogMsg(LoggerLevel.Information, "Live Order proccess successfully for symbol:{0} and Qty:{1} and status msg:{2}",
                        orderSingle?.Symbol, orderSingle.Quantity, TradeStatusWithMessage);

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Live order encounter an error");
                }
            }
        }
        #endregion

        #region Trading Operation Order Single creation methods
        /// <summary>
        /// Create OrderSingle object for replace order
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="data"></param>
        OrderSingle CreateReplaceOrderFromData(int companyUserID, dynamic data, bool IsSubOrderReplaced, bool isOriginalOrder = false, bool IsBookAsSwap = false)
        {
            OrderSingle orderSingle = new OrderSingle();
            try
            {
                orderSingle.Symbol = data.Symbol;
                orderSingle.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                orderSingle.AssetID = data.AssetID;
                orderSingle.AUECID = data.AUECID;
                orderSingle.AUECLocalDate = data.AUECLocalDate;
                orderSingle.CumQty = 0;
                orderSingle.CumQtyForSubOrder = 0;
                orderSingle.Quantity = data.Quantity;
                orderSingle.Price = data.Limit;
                orderSingle.IsManualOrder = Convert.ToBoolean(data.IsManualOrder);
                orderSingle.LastPrice = Convert.ToDouble(Convert.ToString(data.LastPrice));
                orderSingle.CurrencyID = data.CurrencyID;
                orderSingle.FXRate = data.FXRate;
                orderSingle.FXConversionMethodOperator = data.FXConversionMethodOperator;
                string OrderSide_ID = data.Side;
                string venueID = data.Venue;
                if (IsBookAsSwap)
                {
                    if (!String.IsNullOrEmpty(venueID))
                    {
                        orderSingle.VenueID = CachedDataManager.GetInstance.GetVenueID(venueID);
                        orderSingle.Venue = venueID;
                    }
                }
                else
                {
                    if (int.TryParse(venueID, out int result))
                    {
                        orderSingle.VenueID = Convert.ToInt32(venueID);
                        orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(orderSingle.VenueID);
                    }
                }
                if (IsBookAsSwap)
                {
                    if (!String.IsNullOrEmpty(OrderSide_ID))
                    {
                        orderSingle.OrderSide = OrderSide_ID;
                        orderSingle.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideValue(OrderSide_ID);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(OrderSide_ID))
                    {
                        orderSingle.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(OrderSide_ID);
                        orderSingle.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderSingle.OrderSideTagValue);
                    }
                }
                orderSingle.PranaMsgType = 3;
                orderSingle.IsStageRequired = false;
                orderSingle.ValidationStatus = "None";
                string BrokerName = data.Broker.Value;
                if (!String.IsNullOrEmpty(BrokerName))
                {
                    orderSingle.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(BrokerName);
                    orderSingle.CounterPartyName = BrokerName;
                }
                else
                {
                    orderSingle.CounterPartyID = int.MinValue;
                    orderSingle.CounterPartyName = string.Empty;
                }
                orderSingle.AvgPrice = data.AvgPrice;
                orderSingle.StopPrice = data.Stop;
                if (isOriginalOrder)
                {
                    if (data.ExecutionInstruction != null && Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                    {
                        string handlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionIdBasedOnTagValue(Convert.ToString(data.HandlingInstruction));
                        orderSingle.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(handlingInstruction);
                    }
                    if (data.ExecutionInstruction != null && Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                    {
                        string executionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionIdBasedOnTagValue(Convert.ToString(data.ExecutionInstruction));
                        orderSingle.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(Convert.ToString(data.ExecutionInstruction));
                    }
                }
                else
                {
                    if (data.ExecutionInstruction != null && Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                        orderSingle.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(Convert.ToString(data.ExecutionInstruction));
                    if (data.HandlingInstruction != null && Convert.ToString(data.HandlingInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                        orderSingle.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(Convert.ToString(data.HandlingInstruction));
                }
                if (IsBookAsSwap)
                {
                    if (data.ExecutionInstruction != null && Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                        orderSingle.ExecutionInstruction = data.ExecutionInstruction;
                    if (data.HandlingInstruction != null && Convert.ToString(data.HandlingInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                        orderSingle.HandlingInstruction = data.HandlingInstruction;
                }
                if (IsBookAsSwap)
                {
                    orderSingle.OrderType = data.OrderType;
                    orderSingle.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValue(orderSingle.OrderType);
                }
                else
                {
                    orderSingle.OrderTypeTagValue = data.OrderType;
                    orderSingle.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(orderSingle.OrderTypeTagValue);
                }
                orderSingle.Level1ID = data.Level1Id;
                orderSingle.TransactionSourceTag = data.TransactionSourceTag;
                orderSingle.TransactionSource = (TransactionSource)orderSingle.TransactionSourceTag;
                string accountText = data.AccountText;
                if (orderSingle.Level1ID > 0 && accountText.StartsWith("Custom"))
                {
                    orderSingle.OriginalAllocationPreferenceID = orderSingle.Level1ID;
                }
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), companyUserID, orderSingle.Level1ID);
                if (operationPreference != null)
                {
                    orderSingle.OriginalAllocationPreferenceID = orderSingle.Level1ID;
                    orderSingle.AllocationSchemeName = operationPreference.OperationPreferenceName;
                    if (operationPreference.TargetPercentage.Count == 1)
                    {
                        foreach (int accountId in operationPreference.TargetPercentage.Keys)
                        {
                            orderSingle.Account = CachedDataManager.GetInstance.GetAccount(accountId);
                        }
                    }
                    else
                    {
                        orderSingle.Account = OrderFields.PROPERTY_MULTIPLE;
                    }
                }
                else
                {
                    //If any account for given level1Id is not found, we consider this as unallocated
                    orderSingle.Account = string.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(orderSingle.Level1ID))
                                        ? orderSingle.Account
                                        : CachedDataManager.GetInstance.GetAccount(orderSingle.Level1ID);
                }
                string TIF_ID = data.TIF;
                if (IsBookAsSwap)
                {
                    if (!String.IsNullOrEmpty(TIF_ID))
                        orderSingle.TIF = TagDatabaseManager.GetInstance.GetTIFTagValueBasedOnText(TIF_ID);
                }
                else
                {
                    if (!String.IsNullOrEmpty(TIF_ID))
                        orderSingle.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(TIF_ID);
                }
                orderSingle.ExchangeID = data.ExchangeID;
                orderSingle.UnderlyingID = data.UnderlyingID;
                orderSingle.ClientTime = DateTime.Now.ToLongTimeString();
                orderSingle.OrderID = data.OrderId;
                orderSingle.ParentClOrderID = data.ParentClOrderID;
                orderSingle.ClOrderID = data.ClOrderID;
                orderSingle.StagedOrderID = data.StagedOrderID;
                orderSingle.OrigClOrderID = data.ClOrderID;
                orderSingle.TransactionTime = data.TransactionTime;
                orderSingle.NirvanaProcessDate = DateTime.Now;
                orderSingle.LeavesQty = data.WorkingQuantity;
                orderSingle.LastShares = data.ExecutedQuantity;
                if (Convert.ToString(data.TradingAccount) != TradingServiceConstants.CONST_MINUS_ONE)
                    orderSingle.TradingAccountID = data.TradingAccount;
                orderSingle.CompanyUserID = companyUserID;
                orderSingle.ActualCompanyUserID = companyUserID;
                orderSingle.ModifiedUserId = companyUserID;
                if (IsSubOrderReplaced)
                {
                    orderSingle.CumQty = data.CumQty;
                    orderSingle.CumQtyForSubOrder = data.CumQty;
                    orderSingle.PranaMsgType = 4;
                }

                if (data.AlgoStrategyID != null && data.AlgoStrategyID != string.Empty)
                {
                    orderSingle.AlgoStrategyID = data.AlgoStrategyID;
                    orderSingle.AlgoStrategyName = data.Algo;
                    orderSingle.AlgoProperties.TagValueDictionary = JsonHelper.DeserializeToObject<Dictionary<string, string>>(data.TagValueDictionary.ToString());
                    orderSingle.AlgoProperties.AlgoStartegyID = data.AlgoStrategyID;
                    if (IsSubOrderReplaced)
                    {
                        orderSingle.OrderID = string.Empty; // Set OrderID to match the flow with Enterprise for suborders with Algo strategy
                    }
                }
                orderSingle.Text = data.Text;
                SetExpireTimeFromData(data, orderSingle);

                string parameter = Convert.ToString(data.SwapParameter);
                if (!string.IsNullOrEmpty(parameter))
                {
                    SwapParameters swapObj = JsonHelper.DeserializeToObject<SwapParameters>(parameter);
                    orderSingle.SwapParameters = swapObj;
                }
                if (IsBookAsSwap)
                {
                    double Price = Convert.ToDouble(data.Price);
                    SwapParameters selectedParams = new SwapParameters();
                    selectedParams.SwapDescription = string.Empty;
                    selectedParams.OrigCostBasis = Price;
                    selectedParams.DayCount = 365;
                    selectedParams.Differential = 0 / ApplicationConstants.BASISPOINTTOPERCENTAGE;
                    selectedParams.NotionalValue = orderSingle.Quantity * Price;
                    selectedParams.FirstResetDate = DateTime.Now.Date.AddDays(1);
                    selectedParams.OrigTransDate = DateTime.Now.Date;
                    selectedParams.BenchMarkRate = 0;
                    selectedParams.ResetFrequency = "Quarterly";
                    orderSingle.SwapParameters = selectedParams;
                }
                SetFieldsInOrderSingleForFixMsg(orderSingle, data);
                orderSingle.IsUseCustodianBroker = data.IsUseCustodianBroker;
                if (orderSingle.IsUseCustodianBroker)
                {
                    orderSingle.AccountBrokerMapping = CreateAccountBrokerMapping(data, orderSingle, operationPreference);
                }
                orderSingle.TradeApplicationSource = data.TradeApplicationSource ?? orderSingle.TradeApplicationSource;
                orderSingle.TradeAttribute1 = data.TradeAttribute1;
                orderSingle.TradeAttribute2 = data.TradeAttribute2;
                orderSingle.TradeAttribute3 = data.TradeAttribute3;
                orderSingle.TradeAttribute4 = data.TradeAttribute4;
                orderSingle.TradeAttribute5 = data.TradeAttribute5;
                orderSingle.TradeAttribute6 = data.TradeAttribute6;

                for (int i = 7; i <= 45; i++)
                {
                    var propName = $"TradeAttribute{i}";

                    var value = data[propName];

                    if (value != null)
                    {
                        orderSingle.SetTradeAttributeValue(propName, value.ToString());
                    }
                }

                if (int.TryParse(data.Level2ID?.ToString(), out int level2ID))
                {
                    orderSingle.Level2ID = level2ID;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }

            return orderSingle;
        }

        /// <summary>
        /// Create OrderSingle object for Parent order
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="data"></param>
        OrderSingle CreateParentOrderForReplace(int companyUserID, dynamic data)
        {
            OrderSingle orderSingle = new OrderSingle();
            try
            {
                orderSingle.Symbol = data.Symbol;
                orderSingle.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                orderSingle.AssetID = data.AssetID;
                orderSingle.AUECID = data.AUECID;
                orderSingle.CumQty = 0;
                orderSingle.CumQtyForSubOrder = 0;
                orderSingle.Quantity = data.Quantity;
                orderSingle.IsManualOrder = Convert.ToBoolean(data.IsManualOrder);
                orderSingle.LastPrice = Convert.ToDouble(Convert.ToString(data.LastPrice));
                orderSingle.CurrencyID = data.CurrencyID;
                orderSingle.FXRate = data.FXRate;
                orderSingle.FXConversionMethodOperator = data.FXConversionMethodOperator;
                string OrderSide_ID = data.OrderSideTagValue;
                string venueID = data.VenueID;
                if (int.TryParse(venueID, out int result))
                {
                    orderSingle.VenueID = Convert.ToInt32(venueID);
                    orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(orderSingle.VenueID);
                }
                if (!String.IsNullOrEmpty(OrderSide_ID))
                {
                    orderSingle.OrderSideTagValue = OrderSide_ID; // In case of replace, parent order's OrderSide_ID comes as OrderSideTagValue
                    orderSingle.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(OrderSide_ID);
                }
                orderSingle.PranaMsgType = 3;
                orderSingle.IsStageRequired = false;
                orderSingle.ValidationStatus = "None";
                string BrokerName = data.Broker.Value;
                if (!String.IsNullOrEmpty(BrokerName))
                {
                    orderSingle.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(BrokerName);
                    orderSingle.CounterPartyName = BrokerName;
                }
                else
                {
                    orderSingle.CounterPartyID = int.MinValue;
                    orderSingle.CounterPartyName = string.Empty;
                }
                orderSingle.Price = data.Limit;
                // orderSingle.StopPrice = data.Stop;
                if (data.ExecutionInstruction != null && Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                {
                    string handlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionIdBasedOnTagValue(Convert.ToString(data.HandlingInstruction));
                    orderSingle.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(handlingInstruction);
                }
                if (data.ExecutionInstruction != null && Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                {
                    string executionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionIdBasedOnTagValue(Convert.ToString(data.ExecutionInstruction));
                    orderSingle.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(Convert.ToString(data.ExecutionInstruction));
                }
                orderSingle.OrderTypeTagValue = data.OrderTypeTagValue;
                orderSingle.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(orderSingle.OrderTypeTagValue);
                orderSingle.Level1ID = data.AccountID;
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), companyUserID, orderSingle.Level1ID);
                if (operationPreference != null)
                {
                    orderSingle.OriginalAllocationPreferenceID = orderSingle.Level1ID;
                    orderSingle.AllocationSchemeName = operationPreference.OperationPreferenceName;
                    if (operationPreference.TargetPercentage.Count == 1)
                    {
                        foreach (int accountId in operationPreference.TargetPercentage.Keys)
                        {
                            orderSingle.Account = CachedDataManager.GetInstance.GetAccount(accountId);
                        }
                    }
                    else
                    {
                        orderSingle.Account = OrderFields.PROPERTY_MULTIPLE;
                    }
                }
                else
                {
                    //If any account for given level1Id is not found, we consider this as unallocated
                    orderSingle.Account = string.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(orderSingle.Level1ID))
                                        ? orderSingle.Account
                                        : CachedDataManager.GetInstance.GetAccount(orderSingle.Level1ID);
                }
                orderSingle.ExchangeID = data.ExchangeID;
                orderSingle.UnderlyingID = data.UnderlyingID;
                orderSingle.ClientTime = DateTime.Now.ToLongTimeString();
                orderSingle.OrderID = data.OrderId;
                orderSingle.ParentClOrderID = data.ParentClOrderID;
                orderSingle.ClOrderID = data.ClOrderID;
                orderSingle.StagedOrderID = data.StagedOrderID;
                orderSingle.TransactionTime = data.TransactionTime;
                orderSingle.NirvanaProcessDate = DateTime.Now;
                orderSingle.UnsentQty = data.UnsentQty;
                if (data.TradingAccountID != null && Convert.ToString(data.TradingAccountID) != TradingServiceConstants.CONST_MINUS_ONE)
                    orderSingle.TradingAccountID = data.TradingAccountID;
                orderSingle.CompanyUserID = companyUserID;
                orderSingle.ActualCompanyUserID = companyUserID;

                if (data.AlgoStrategyID != null && data.AlgoStrategyID != string.Empty)
                {
                    orderSingle.AlgoStrategyID = data.AlgoStrategyID;
                    orderSingle.AlgoStrategyName = data.Algo;
                    orderSingle.AlgoProperties.TagValueDictionary = JsonHelper.DeserializeToObject<Dictionary<string, string>>(data.TagValueDictionary.ToString());
                    orderSingle.AlgoProperties.AlgoStartegyID = data.AlgoStrategyID;
                }
                orderSingle.Text = data.Text;
                SetExpireTimeFromData(data, orderSingle);
                string parameter = Convert.ToString(data.SwapParameter);
                if (!string.IsNullOrEmpty(parameter))
                {
                    SwapParameters swapObj = JsonHelper.DeserializeToObject<SwapParameters>(parameter);
                    orderSingle.SwapParameters = swapObj;
                }
                SetFieldsInOrderSingleForFixMsg(orderSingle, data);
                orderSingle.TradeApplicationSource = data.TradeApplicationSource ?? orderSingle.TradeApplicationSource;
                orderSingle.TradeAttribute1 = data.TradeAttribute1;
                orderSingle.TradeAttribute2 = data.TradeAttribute2;
                orderSingle.TradeAttribute3 = data.TradeAttribute3;
                orderSingle.TradeAttribute4 = data.TradeAttribute4;
                orderSingle.TradeAttribute5 = data.TradeAttribute5;
                orderSingle.TradeAttribute6 = data.TradeAttribute6;

                for (int i = 7; i <= 45; i++)
                {
                    var propName = $"TradeAttribute{i}";

                    var value = data[propName];

                    if (value != null)
                    {
                        orderSingle.SetTradeAttributeValue(propName, value.ToString());
                    }
                }

                if (int.TryParse(data.Level2ID?.ToString(), out int level2ID))
                {
                    orderSingle.Level2ID = level2ID;
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }

            return orderSingle;

        }

        /// <summary>
        /// this method is used to create account broker mapping for PST orders based on its Target percenatege.
        /// </summary>
        /// <param name="accountIds"></param>
        /// <param name="AccountBrokerMapping"></param>
        /// <returns></returns>

        private Dictionary<int, int> CreateAccountBrokerMapping(List<int> accountIds, string AccountBrokerMapping)
        {
            var result = new Dictionary<int, int>();
            try
            {
                Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(AccountBrokerMapping);
                foreach (var accountId in accountIds)
                {
                    var brokerId = accountBrokerMapping.ContainsKey(accountId) && accountBrokerMapping[accountId] != int.MinValue ? accountBrokerMapping[accountId] : -1;
                    result.Add(accountId, brokerId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        /// <summary>
        /// To Handle the cases when account broker mapping is coming empty from Client
        /// </summary>
        /// <param name="order"></param>
        /// <param name="operationPreference"></param>
        /// <returns></returns>
        private string CreateAccountBrokerMappingWhenEmpty(OrderSingle order, AllocationOperationPreference operationPreference)
        {
            var result = new Dictionary<int, int>();
            try
            {
                var accountWiseExecutingBrokerMapping = CachedDataManager.GetInstance.GetAccountWiseExecutingBrokerMapping();
                var accountIds = operationPreference.TargetPercentage.Keys.ToList();
                foreach (var accountId in accountIds)
                {
                    if (accountWiseExecutingBrokerMapping.ContainsKey(accountId))
                    {
                        result.Add(accountId, accountWiseExecutingBrokerMapping[accountId]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return JsonHelper.SerializeObject(result);
        }

        /// <summary>
        /// To crate account broker mapping for orders when IsUseCustodianPrefrenceIsTrue.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderSingle"></param>
        /// <param name="operationPreference"></param>
        /// <returns></returns>
        private string CreateAccountBrokerMapping(dynamic data, OrderSingle orderSingle, AllocationOperationPreference operationPreference)
        {
            try
            {
                if (data.AccountBrokerMapping != null)
                {
                    Dictionary<int, int> accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(data.AccountBrokerMapping.ToString());
                    if (accountBrokerMapping.Count == 0)
                        return CreateAccountBrokerMappingWhenEmpty(orderSingle, operationPreference);
                }

                if (orderSingle.TransactionSource == TransactionSource.PST && operationPreference != null)
                {
                    List<int> accountIds = new List<int>();
                    if (operationPreference.CheckListWisePreference != null && operationPreference.CheckListWisePreference.Count > 0)
                    {
                        foreach (CheckListWisePreference checkListwisePrefrence in operationPreference.CheckListWisePreference.Values)
                        {
                            if ((checkListwisePrefrence.OrderSideList[0] == orderSingle.OrderSideTagValue) || ((checkListwisePrefrence.OrderSideList[0] == "10") && (orderSingle.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed)))
                            {
                                accountIds = checkListwisePrefrence.TargetPercentage.Keys.ToList();
                                continue;
                            }
                        }
                        if (accountIds.Count == 0)
                        {
                            accountIds = operationPreference.TargetPercentage.Keys.ToList();
                        }
                    }
                    else
                    {
                        accountIds = operationPreference.TargetPercentage.Keys.ToList();
                    }
                    return JsonHelper.SerializeObject(CreateAccountBrokerMapping(accountIds, data.AccountBrokerMapping.ToString()));
                }
                else
                    return data.AccountBrokerMapping?.ToString();    //adding terminator to avoid null reference exception
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return string.Empty;
        }
        /// <summary>
        /// Create OrderSingle object for Stage,Manual and Live order
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <param name="data"></param>
        OrderSingle CreateOrderFromJSON(dynamic data, int companyUserID, bool isTradeManual = false, bool isMethodRequiredforPopuptext = false)
        {
            OrderSingle orderSingle = new OrderSingle();
            try
            {
                dynamic shortlocateData = data.ShortLocateParameter;
                orderSingle.Symbol = data.Symbol;
                if (!isMethodRequiredforPopuptext)
                    CreateShortLocateParameter(orderSingle, shortlocateData);
                orderSingle.MsgType = FIXConstants.MSGOrderSingle;
                orderSingle.AssetID = data.AssetID;
                orderSingle.AUECID = data.AUECID;
                orderSingle.CumQty = 0;
                orderSingle.CumQtyForSubOrder = data.CumQty;
                orderSingle.Quantity = data.Quantity;
                orderSingle.Price = data.Limit;
                orderSingle.IsManualOrder = Convert.ToBoolean(data.IsManualOrder);
                string venueID = data.Venue;
                if (int.TryParse(venueID, out int result))
                {
                    orderSingle.VenueID = Convert.ToInt32(venueID);
                    orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(orderSingle.VenueID);
                }
                string OrderSide_ID = data.Side;
                if (!String.IsNullOrEmpty(OrderSide_ID))
                    orderSingle.OrderSideTagValue = TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(OrderSide_ID);
                orderSingle.PranaMsgType = 3;
                orderSingle.IsStageRequired = true;
                orderSingle.ValidationStatus = "None";
                orderSingle.CounterPartyID = CachedDataManager.GetInstance.GetCounterPartyID(data.Broker.Value);
                orderSingle.CounterPartyName = data.Broker.Value;
                orderSingle.AvgPrice = data.AvgPrice;
                orderSingle.StopPrice = data.Stop;
                if (Convert.ToString(data.ExecutionInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                    orderSingle.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(Convert.ToString(data.ExecutionInstruction));
                if (Convert.ToString(data.HandlingInstruction) != TradingServiceConstants.CONST_MINUS_ONE)
                    orderSingle.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(Convert.ToString(data.HandlingInstruction));
                orderSingle.OrderTypeTagValue = data.OrderType;
                orderSingle.Level1ID = data.Level1Id;
                orderSingle.TransactionSourceTag = data.TransactionSourceTag;
                orderSingle.TransactionSource = (TransactionSource)orderSingle.TransactionSourceTag;
                string accountText = data.AccountText;
                if (orderSingle.Level1ID > 0 && accountText.StartsWith("Custom"))
                {
                    orderSingle.OriginalAllocationPreferenceID = orderSingle.Level1ID;
                }
                AllocationOperationPreference operationPreference = _allocationProxy.InnerChannel.GetPreferenceById(CachedDataManager.GetInstance.GetCompanyID(), companyUserID, orderSingle.Level1ID);
                if (operationPreference != null)
                {
                    orderSingle.OriginalAllocationPreferenceID = orderSingle.Level1ID;
                    orderSingle.AllocationSchemeName = operationPreference.OperationPreferenceName;
                    //For trades sent from dock TT, we don't need to set Account
                    if (Convert.ToString(data.ParentClOrderID) != string.Empty)
                    {
                        if (operationPreference.TargetPercentage.Count == 1)
                        {
                            foreach (int accountId in operationPreference.TargetPercentage.Keys)
                            {
                                orderSingle.Account = CachedDataManager.GetInstance.GetAccount(accountId);
                            }
                        }
                        else
                        {
                            orderSingle.Account = OrderFields.PROPERTY_MULTIPLE;
                        }
                    }
                }
                else if (Convert.ToString(data.ParentClOrderID) != string.Empty)
                {
                    //If any account for given level1Id is not found, we consider this as unallocated
                    orderSingle.Account = string.IsNullOrEmpty(CachedDataManager.GetInstance.GetAccount(orderSingle.Level1ID))
                                        ? orderSingle.Account
                                        : CachedDataManager.GetInstance.GetAccount(orderSingle.Level1ID);
                }
                string TIF_ID = data.TIF;
                if (!String.IsNullOrEmpty(TIF_ID))
                    orderSingle.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(TIF_ID);
                orderSingle.OrderStatus = TradingServiceConstants.CONST_ORDER_STATUS_PENDINGNEW;
                orderSingle.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingNew;
                orderSingle.ExchangeID = data.ExchangeID;
                orderSingle.UnderlyingID = data.UnderlyingID;

                #region TradeAttributes

                orderSingle.TradeAttribute1 = data.TradeAttribute1;
                orderSingle.TradeAttribute2 = data.TradeAttribute2;
                orderSingle.TradeAttribute3 = data.TradeAttribute3;
                orderSingle.TradeAttribute4 = data.TradeAttribute4;
                orderSingle.TradeAttribute5 = data.TradeAttribute5;
                orderSingle.TradeAttribute6 = data.TradeAttribute6;

                for (int i = 7; i <= 45; i++)
                {
                    string propName = $"TradeAttribute{i}";
                    if (data[propName] != null)
                    {
                        orderSingle.SetTradeAttributeValue(propName, data[propName].ToString());
                    }
                }

                #endregion

                #region Trade New Sub Flow
                if (!isTradeManual)
                {
                    if (data.OrderId != string.Empty)
                    {
                        orderSingle.OrderID = data.OrderId;
                        orderSingle.OrderStatus = data.Status;
                        orderSingle.TransactionTime = data.TransactionTime;
                        orderSingle.IsStageRequired = false;
                        orderSingle.PranaMsgType = 2;
                        orderSingle.StagedOrderID = data.OrderId;
                        orderSingle.ClientTime = DateTime.Now.ToLongTimeString();
                        orderSingle.NirvanaProcessDate = DateTime.Now;
                        string parameter = Convert.ToString(data.SwapParameter);
                        if (!string.IsNullOrEmpty(parameter))
                        {
                            SwapParameters swapObj = JsonHelper.DeserializeToObject<SwapParameters>(parameter);
                            double Price = Convert.ToDouble(data.Price);
                            swapObj.OrigCostBasis = Price;
                            swapObj.NotionalValue = orderSingle.Quantity * Price;
                            orderSingle.SwapParameters = swapObj;
                        }
                    }
                }
                else
                {
                    if (data.OrderId != string.Empty)
                    {
                        //Manual order through Trade new sub
                        orderSingle.CumQty = data.CumQty;
                        orderSingle.CumQtyForSubOrder = 0;
                        orderSingle.OrderID = data.OrderId;
                        //orderSingle.OrderStatus = data.Status;
                        orderSingle.TransactionTime = data.TransactionTime;
                        orderSingle.IsStageRequired = false;
                        orderSingle.PranaMsgType = 5;
                        orderSingle.StagedOrderID = data.OrderId;
                        orderSingle.ClientTime = DateTime.Now.ToLongTimeString();
                        orderSingle.NirvanaProcessDate = DateTime.Now;
                        string parameter = Convert.ToString(data.SwapParameter);
                        if (!string.IsNullOrEmpty(parameter))
                        {
                            SwapParameters swapObj = JsonHelper.DeserializeToObject<SwapParameters>(parameter);
                            double Price = Convert.ToDouble(data.Price);
                            swapObj.OrigCostBasis = Price;
                            swapObj.NotionalValue = orderSingle.Quantity * Price;
                            orderSingle.SwapParameters = swapObj;
                        }
                    }
                }
                #endregion
                if (Convert.ToString(data.TradingAccount) != TradingServiceConstants.CONST_MINUS_ONE)
                    orderSingle.TradingAccountID = data.TradingAccount;
                orderSingle.CompanyUserID = companyUserID;
                orderSingle.ActualCompanyUserID = companyUserID;
                orderSingle.ModifiedUserId = companyUserID;

                if (data.AlgoStrategyID != null && data.AlgoStrategyID != string.Empty)
                {
                    orderSingle.AlgoStrategyID = data.AlgoStrategyID;
                    orderSingle.AlgoStrategyName = data.Algo;
                    orderSingle.AlgoProperties.TagValueDictionary = JsonHelper.DeserializeToObject<Dictionary<string, string>>(data.TagValueDictionary.ToString());
                    orderSingle.AlgoProperties.AlgoStartegyID = data.AlgoStrategyID;
                }
                orderSingle.CurrencyID = data.CurrencyID;
                SetFXRateAndOperator(orderSingle);
                orderSingle.Text = data.Text;
                SetExpireTimeFromData(data, orderSingle);

                //Swap setup
                bool IsSwapTrade = Convert.ToBoolean(data.IsSwapTrade);
                if (IsSwapTrade)
                {
                    double Price = Convert.ToDouble(data.Price);
                    SwapParameters selectedParams = new SwapParameters();
                    selectedParams.SwapDescription = string.Empty;
                    selectedParams.OrigCostBasis = Price;
                    selectedParams.DayCount = 365;
                    selectedParams.Differential = 0 / ApplicationConstants.BASISPOINTTOPERCENTAGE;
                    selectedParams.NotionalValue = orderSingle.Quantity * Price;
                    selectedParams.FirstResetDate = DateTime.Now.Date.AddDays(1);
                    selectedParams.OrigTransDate = DateTime.Now.Date;
                    selectedParams.BenchMarkRate = 0;
                    selectedParams.ResetFrequency = "Quarterly";
                    orderSingle.SwapParameters = selectedParams;
                }
                SetFieldsInOrderSingleForFixMsg(orderSingle, data);
                orderSingle.IsUseCustodianBroker = data.IsUseCustodianBroker;
                if (orderSingle.IsUseCustodianBroker)
                {
                    orderSingle.AccountBrokerMapping = CreateAccountBrokerMapping(data, orderSingle, operationPreference);
                }
                orderSingle.TradeApplicationSource = data.TradeApplicationSource ?? orderSingle.TradeApplicationSource;
                if (int.TryParse(data.Level2ID?.ToString(), out int level2ID))
                {
                    orderSingle.Level2ID = level2ID;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }

            return orderSingle;
        }

        private void SetFieldsInOrderSingleForFixMsg(OrderSingle orderSingle, dynamic data)
        {
            try
            {
                if (orderSingle.AssetID == 2)
                {
                    orderSingle.StrikePrice = data.StrikePrice ?? orderSingle.StrikePrice;
                    orderSingle.MaturityMonthYear = data.MaturityMonthYear ?? orderSingle.MaturityMonthYear;
                    orderSingle.MaturityDay = data.MaturityDay ?? orderSingle.MaturityDay;
                    orderSingle.SecurityType = FIXConstants.SECURITYTYPE_Options;
                    orderSingle.PegDifference = data.PegDifference ?? orderSingle.PegDifference;
                }
                else if (orderSingle.AssetID == 1)
                {
                    orderSingle.SecurityType = FIXConstants.SECURITYTYPE_Equity;
                }
                else if (orderSingle.AssetID == 3)
                {
                    orderSingle.SecurityType = FIXConstants.SECURITYTYPE_Futures;
                    orderSingle.PegDifference = data.PegDifference ?? orderSingle.PegDifference;
                    orderSingle.MaturityMonthYear = data.MaturityMonthYear ?? orderSingle.MaturityMonthYear;
                }
                if (String.IsNullOrEmpty(orderSingle.BorrowerBroker) && orderSingle.OrderSideTagValue == FIXConstants.SIDE_SellShort)
                {
                    orderSingle.BorrowerBroker = data.BorrowerBroker ?? orderSingle.BorrowerBroker;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error in setting options fields");
            }
        }

        /// <summary>
        /// This method sets expiration date and time in replaced order
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderSingle"></param>
        private static void SetExpireTimeFromData(dynamic data, OrderSingle orderSingle)
        {
            try
            {
                DateTime expirationDate = DateTime.Now;
                if (data.ExpiryDate != null && data.ExpiryDate != string.Empty && DateTime.TryParse(data.ExpiryDate.ToString(), out expirationDate))
                {
                    DateTime TimeStamp = TimeZoneHelper.GetInstance().MarketEndTimeInfo[Convert.ToInt32(data.AUECID)];
                    orderSingle.ExpireTime = new DateTime(expirationDate.Year, expirationDate.Month, expirationDate.Day, TimeStamp.Hour, TimeStamp.Minute, TimeStamp.Second).ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
        }

        private async void KafkaManager_DetermineMultipleSecuritiesBorrowType(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    List<string> symbols = JsonHelper.DeserializeToObject<List<string>>(message.Data);

                    _shortLocateManager.ShortLocateCollection = ShortLocateManager.GetInstance().FetchShortLocateDetailsForTrade();

                    List<dynamic> result = new List<dynamic>();
                    foreach (var symbol in symbols)
                    {
                        dynamic securityBorrowType = new System.Dynamic.ExpandoObject();
                        securityBorrowType.Symbol = symbol;

                        bool isAvailable = _shortLocateManager.ShortLocateCollection.Any(x => x.Ticker.Equals(symbol, StringComparison.OrdinalIgnoreCase));
                        securityBorrowType.BorrowType = isAvailable ? "HTB" : "ETB";

                        result.Add(securityBorrowType);
                    }

                    message.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_DetermineMultipleSecurityBorrowTypeResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "KafkaManager_DetermineMultipleSecuritiesBorrowType request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_DetermineMultipleSecurityBorrowTypeResponse);
                }
            }
        }
        #endregion

        #region Trade Manager Extension Events 
        /// <summary>
        /// Event for Updating Connected Broker details
        /// </summary>
        private async void _tradeManager_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {
            try
            {
                RequestResponseModel response = new RequestResponseModel(0, JsonHelper.SerializeObject(e.Value));
                using (LoggerHelper.PushLoggingProperties(response.CorrelationId, response.RequestID, response.CompanyUserID))
                {
                    await KafkaManager.Instance.Produce(TOPIC_BrokerStatusResponse, response);
                    Logger.LogMsg(LoggerLevel.Verbose, TradingServiceConstants.CONST_BROKER_STATUS_CHANGED + response.Data);  //changing loglevel from info to verbose as it print lot of logs msg(7Aug)
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Checks for duplicate trade.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private bool CheckForDuplicateTrade(object sender, EventArgs<OrderSingle> e)
        {
            bool allowTrade = true;
            int timeInterval = 0;
            try
            {
                if (IsDuplicateTradeAllowed)
                {
                    timeInterval = DuplicateTradeTimer != 0 ? DuplicateTradeTimer : 0;
                }

                int companyUserID = e.Value.CompanyUserID;
                UserAction userAction = UserAction.None;
                string userActionType = string.Empty;

                if (_userTradeCacheManager.ExistsInUserTradesCache(e.Value, timeInterval, companyUserID)
                    || !userWiseDuplicateTradeResponse[companyUserID].Equals(string.Empty))
                {
                    if (userWiseDuplicateTradeResponse[companyUserID].Equals(string.Empty))
                    {
                        TradeStatusWithMessage.Add(0, "DUPLICATE");
                        return false;
                    }
                    userAction = userWiseDuplicateTradeResponse[companyUserID] == "YES" ? UserAction.Yes : UserAction.No;
                    userActionType = userWiseDuplicateTradeResponse[companyUserID] == "YES" ? "Duplicate trade allowed by user." : "Duplicate trade rejected by user.";
                    if (userWiseDuplicateTradeResponse[companyUserID] == "No")
                        allowTrade = false;
                }
                _userTradeCacheManager.AddTradesToUserTradesCache(e.Value, userAction, userActionType, companyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allowTrade;
        }
        #endregion

        #region Short Locate Section
        /// <summary>
        /// fetches Short Locate Orders filtered by symbol
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_ShortLocateOrdersFilteredBySymbolRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    string symbol = JsonHelper.DeserializeToObject<string>(message.Data);
                    var symbolWiseFilteredShortLocateInformation = _shortLocateManager.GetSymbolWiseShortLocateInformation(symbol);
                    message.Data = JsonHelper.SerializeObject(symbolWiseFilteredShortLocateInformation.OrderBy(x => x.BorrowRate));
                    await KafkaManager.Instance.Produce(TOPIC_ShortLocateOrdersSymbolWiseResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "ShortLocateOrdersFilteredBySymbol request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_ShortLocateOrdersSymbolWiseRequest);
                }
            }
        }

        /// <summary>
        /// Determines if the security is ETB/HTB
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_DetermineSecurtiesBorrowType(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    string symbol = JsonHelper.DeserializeToObject<string>(message.Data);
                    dynamic SecurityBorrowType = new System.Dynamic.ExpandoObject();
                    SecurityBorrowType.Symbol = symbol;
                    _shortLocateManager.ShortLocateCollection = ShortLocateManager.GetInstance().FetchShortLocateDetailsForTrade();
                    var isSymbolAvailableForShortLocate = _shortLocateManager.ShortLocateCollection.FindAll(x => x.Ticker.Equals(symbol));
                    if (isSymbolAvailableForShortLocate != null && isSymbolAvailableForShortLocate.Count > 0)
                    {
                        SecurityBorrowType.BorrowType = "HTB";
                    }
                    else
                    {
                        SecurityBorrowType.BorrowType = "ETB";
                    }
                    message.Data = JsonHelper.SerializeObject(SecurityBorrowType);
                    await KafkaManager.Instance.Produce(TOPIC_DetermineSecurityBorrowTypeResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "KafkaManager_DetermineSecurtiesBorrowType request has been processed successfully");
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_DetermineSecurityBorrowTypeResponse);
                }
            }
        }
        #endregion

        #region Algo section 
        /// <summary>
        /// Loads AlgoStrategies details broker-wise from config section "AlgoStrategiesPath"
        /// </summary>
        private void LoadAlgoStrategies()
        {
            try
            {
                NameValueCollection _algoStrategiesSection = ConfigurationManager.GetSection("AlgoStrategiesPath") as NameValueCollection;
                if (_algoStrategiesSection != null)
                {
                    foreach (string key in _algoStrategiesSection.AllKeys)
                    {
                        string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + _algoStrategiesSection[key];
                        if (File.Exists(path)) 
                        { 
                            _strategyProvider.Load(key, path); 
                            _strategyProvider.LogAtdlFile(key, path); 
                        }
                    }
                }
                string strategiesInfo = JsonHelper.SerializeObject(_strategyProvider.GetAllStrategiesInfo());
                _ = KafkaManager.Instance.Produce(TOPIC_AllAlgoStrategiesInfoResponse, new RequestResponseModel(0, strategiesInfo));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Fetch Algo Strategies Details for the given broker
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BrokerWiseAlgoStrategiesRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {

                    Dictionary<string, string> dataToSend = new Dictionary<string, string>();

                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    var brokerId = Convert.ToInt32(data.BrokerID.ToString());
                    Strategies_t strategies = _strategyProvider.GetStrategiesByProvider(data.BrokerID.ToString());

                    var isAlgoBrokerExistInCache = CachedDataManager.GetInstance.IsAlgoBrokerFromID(brokerId);

                    Logger.LogMsg(LoggerLevel.Information, "Received algo strategy request for broker {0} and isAlgoStrategiesExists: {1} and isAlgoBrokerExistInCache:{2}",
                        brokerId, strategies != null, isAlgoBrokerExistInCache);

                    if (strategies != null && isAlgoBrokerExistInCache)
                    {
                        DateTime marketEndTime = TimeZoneHelper.GetInstance().MarketEndTimeInfo[Convert.ToInt32(data.AUECID)];
                        string baseOffset = Prana.BusinessObjects.TimeZoneInfo.GetBaseOffset(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(Convert.ToInt32(data.AUECID))).TotalSeconds.ToString();

                        string countryISOCode = CachedDataManager.GetInstance.GetCountryISOCode(Convert.ToInt32(data.AUECID));
                        IsoCountryCodeA3 countryCode;
                        Enum.TryParse(countryISOCode, out countryCode);
                        Region targetRegion = RegionsA3.GetRegionForCountry(countryCode);

                        dataToSend["StrategiesData"] = JsonHelper.SerializeObject(strategies);
                        dataToSend["MarketEndTime"] = JsonHelper.SerializeObject(marketEndTime);
                        dataToSend["BaseOffset"] = baseOffset;
                        dataToSend["RegionOfBroker"] = JsonHelper.SerializeObject(targetRegion);

                        message.Data = JsonHelper.SerializeObject(dataToSend);
                    }
                    else
                    {
                        message.Data = string.Empty;
                        Logger.LogMsg(LoggerLevel.Information, "No Algo Strategies found for broker {0, either there may be issue in cache generation", brokerId);
                        message.ErrorMsg = $"No strategies available for the selected broker. This may be due to a cache issue or try restarting the trading service.";
                    }

                    var result = new { AlgoData = message.Data, TradingTicketId = data.TradingTicketId };
                    message.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_BrokerWiseAlgoStrategiesResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "BrokerWiseAlgoStrategiesRequest process successfully for broker {0}", brokerId);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BrokerWiseAlgoStrategiesResponse);
                }
            }
        }
        #endregion

        #region Other Private methods
        /// <summary>
        /// Add Audit Trail Collection
        /// </summary>
        /// <param name="orRequest"></param>
        /// <param name="action"></param>
        private void AddOrderDataAuditEntryAndSaveInDB(OrderSingle order, TradeAuditActionType.ActionType action, int userId, string originalValues, string newValues)
        {
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    List<TradeAuditEntry> auditCollection = new List<TradeAuditEntry>();
                    TradeAuditEntry audit = new TradeAuditEntry()
                    {
                        Action = action,
                        AUECLocalDate = DateTime.Now,
                        OriginalDate = order.AUECLocalDate,
                        CompanyUserId = userId,
                        GroupID = string.Empty,
                        TaxLotID = string.Empty,
                        ParentClOrderID = order.ParentClOrderID,
                        ClOrderID = order.ClOrderID,
                        Symbol = order.Symbol,
                        Level1ID = order.Level1ID,
                        OrderSideTagValue = order.OrderSideTagValue,
                        OriginalValue = originalValues,
                        Comment = newValues,
                        Source = TradeAuditActionType.ActionSource.Trade
                    };
                    auditCollection.Add(audit);
                    AuditManager.Instance.SaveAuditList(auditCollection);
                });

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
        /// Create ShortLocate Parameter for the trade
        /// </summary>
        /// <param name="orderSingle"></param>
        void CreateShortLocateParameter(OrderSingle orderSingle, dynamic shortlocateData)
        {
            try
            {
                if (shortlocateData != null)
                {
                    bool isNewShortLocateRowAdded = Convert.ToBoolean(shortlocateData.isNewRow);

                    ShortLocateListParameter shortLocateListParameter = new ShortLocateListParameter();
                    shortLocateListParameter.Broker = shortlocateData.Broker;
                    shortLocateListParameter.BorrowerId = shortlocateData.BorrowerId;
                    shortLocateListParameter.BorrowRate = shortlocateData.BorrowRate;
                    shortLocateListParameter.NirvanaLocateID = shortlocateData.NirvanaLocateID;
                    shortLocateListParameter.BorrowSharesAvailable = shortlocateData.BorrowSharesAvailable;
                    shortLocateListParameter.ReplaceQuantity = shortlocateData.ReplaceQuantity;
                    shortLocateListParameter.BorrowQuantity = shortlocateData.BorrowQuantity;

                    if (isNewShortLocateRowAdded)
                    {
                        ShortLocateOrder NewOrder = new ShortLocateOrder();
                        NewOrder.Ticker = orderSingle.Symbol;
                        NewOrder.Broker = shortLocateListParameter.Broker;
                        NewOrder.ClientMasterfund = string.Empty;
                        NewOrder.TradeQuantity = 0;
                        NewOrder.BorrowSharesAvailable = shortLocateListParameter.BorrowSharesAvailable;
                        NewOrder.BorrowRate = shortLocateListParameter.BorrowRate;
                        NewOrder.BorrowerId = shortLocateListParameter.BorrowerId;
                        NewOrder.BorrowedShare = 0;
                        NewOrder.BorrowedRate = 0;
                        NewOrder.SODBorrowRate = shortLocateListParameter.BorrowRate;
                        NewOrder.SODBorrowshareAvailable = shortLocateListParameter.BorrowSharesAvailable;
                        NewOrder.StatusSource = "API";
                        _shortLocateManager.SaveShortLocateData(new BindingList<ShortLocateOrder>() { NewOrder });
                        var slcollection = _shortLocateManager.FetchShortLocateDetailsForTrade();
                        if (slcollection.Count > 0)
                        {
                            shortLocateListParameter.NirvanaLocateID = slcollection[slcollection.Count - 1].NirvanaLocateID;
                        }
                    }
                    orderSingle.ShortLocateParameter = shortLocateListParameter;
                    orderSingle.BorrowerID = shortLocateListParameter.BorrowerId;
                    orderSingle.ShortRebate = shortLocateListParameter.BorrowRate;
                    orderSingle.BorrowerBroker = shortLocateListParameter.Broker;
                    orderSingle.CumQty = 0.0;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Set FXrate and FXRateMethodOperator based on currenyID
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        void SetFXRateAndOperator(OrderSingle orderSingle)
        {
            try
            {
                int companyBaseCurrenyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                if (orderSingle.CurrencyID == companyBaseCurrenyID)
                {
                    orderSingle.FXRate = 1.0;
                }
                else
                {
                    orderSingle.FXRate = 0.0;
                }
                switch (CachedDataManager.GetInstance.GetCurrencyText(orderSingle.CurrencyID))
                {
                    case "EUR":
                    case "GBP":
                    case "NZD":
                    case "AUD":
                        orderSingle.FXConversionMethodOperator = TradingServiceConstants.CONST_FXOPERATOR_MULTIPLY;
                        break;

                    default:
                        orderSingle.FXConversionMethodOperator = TradingServiceConstants.CONST_FXOPERATOR_DIVIDE;
                        break;
                }
                if (orderSingle.CurrencyID == companyBaseCurrenyID)
                {
                    orderSingle.FXConversionMethodOperator = TradingServiceConstants.CONST_FXOPERATOR_MULTIPLY;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Set TT Properties Mapping
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderSingle"></param>
        private static void SetTTPropertiesMapping(dynamic data, OrderSingle orderSingle)
        {
            try
            {
                if (listTTControlsMapping.Count >= 1)
                {
                    double _avgPrice = Convert.ToDouble((data.Price).ToString());
                    foreach (DefTTControlsMapping mappings in listTTControlsMapping)
                    {
                        string fromControl = "_" + mappings.FromControl.ToLower();
                        string toControl = "_" + mappings.ToControl.ToLower();


                        var fromFields = orderSingle.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        var fromField = fromFields.SingleOrDefault(a => a.Name.ToLower().Contains(fromControl));
                        if (fromField == null)
                        {
                            fromFields = orderSingle.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                            if (fromFields != null)
                                fromField = fromFields.SingleOrDefault(a => a.Name.ToLower().Contains(fromControl));
                        }

                        var toFields = orderSingle.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        var toField = toFields.SingleOrDefault(a => a.Name.ToLower().Contains(toControl));

                        if (toField == null)
                        {
                            toFields = orderSingle.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                            if (toFields != null)
                                toField = toFields.SingleOrDefault(a => a.Name.ToLower().Contains(toControl));
                        }
                        if (toField != null && fromField != null)
                        {
                            switch (toField.FieldType.Name)
                            {
                                case "Double":
                                    double _doubleValue;
                                    if (Double.TryParse((fromField.GetValue(orderSingle)).ToString(), out _doubleValue))
                                    {
                                        if (fromField.Name.Equals("_avgPrice"))
                                            _doubleValue = _avgPrice;
                                        toField.SetValue(orderSingle, _doubleValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Double Type " + fromField.FieldType.Name);
                                    }
                                    break;
                                case "String":
                                    var _stringValue = (fromField.GetValue(orderSingle)).ToString();
                                    if (fromField.Name.Equals("_avgPrice"))
                                        _stringValue = _avgPrice.ToString();
                                    toField.SetValue(orderSingle, _stringValue);
                                    break;
                                case "Int":
                                    int _intValue;
                                    if (int.TryParse((fromField.GetValue(orderSingle)).ToString(), out _intValue))
                                    {
                                        if (fromField.Name.Equals("_avgPrice"))
                                            _intValue = Convert.ToInt16(_avgPrice);
                                        toField.SetValue(orderSingle, _intValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Int Type " + fromField.FieldType.Name);
                                    }
                                    break;

                                case "Boolean":
                                    bool _boolValue;
                                    if (Boolean.TryParse((fromField.GetValue(orderSingle)).ToString(), out _boolValue))
                                    {
                                        toField.SetValue(orderSingle, _boolValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Boolean Type " + fromField.FieldType.Name);
                                    }
                                    break;

                                case "DateTime":
                                    DateTime _dateValue;
                                    if (DateTime.TryParse((fromField.GetValue(orderSingle)).ToString(), out _dateValue))
                                    {
                                        toField.SetValue(orderSingle, _dateValue);
                                    }
                                    else
                                    {
                                        Logger.LoggerWrite("Unable to parse the value to Date Time Type " + fromField.FieldType.Name);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        #endregion

        #region login cache maintenance methods
        /// <summary>
        /// Cache creation for logged in user
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
                        if (!_dictLoggedInUser.ContainsKey(kvp.Key) && loggedInUsersReceived[kvp.Key].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                        {
                            #region Cache creation for logged in user
                            _dictLoggedInUser.Add(kvp.Key, loggedInUsersReceived[kvp.Key]);

                            InformationReporter.GetInstance.Write(TradingServiceConstants.CONST_USERID + TradingServiceConstants.CONST_COLON + kvp.Key.ToString() + TradingServiceConstants.CONST_CACHE_CREATED);
                            _userTradeCacheManager.AddUserInUserWiseTradeCache(kvp.Key);

                            lock (_userWiseTTCollectionData)
                            {
                                if (!_userWiseTTCollectionData.ContainsKey(kvp.Key))
                                {
                                    Dictionary<string, string> tradingTicketData = new Dictionary<string, string>
                                    {
                                        { "UserWiseBrokerData", SetupUserWiseBrokerDetails(kvp.Key) },
                                        { "BrokerWiseVenues", SetupUserWise_BrokerWise_VenueDetails(kvp.Key) }
                                    };

                                    _userWiseTTCollectionData.Add(kvp.Key, JsonHelper.SerializeObject(tradingTicketData));
                                }
                            }
                            #endregion
                            ProduceKafkaRequests(kvp.Key);

                            KafkaManager_ProduceTradingTicketData(kvp.Key);
                        }
                        // In the case when only service gateway is restarted
                        else
                        {
                            if (_userWiseTTCollectionData.ContainsKey(kvp.Key))
                                KafkaManager_ProduceTradingTicketData(kvp.Key);
                        }
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

                #region Clear cache for logged out user
                if (_dictLoggedInUser.ContainsKey(companyUserID))
                    _dictLoggedInUser.Remove(companyUserID);

                InformationReporter.GetInstance.Write(TradingServiceConstants.CONST_USERID + TradingServiceConstants.CONST_COLON + companyUserID.ToString() + TradingServiceConstants.CONST_CACHE_CLEARED);
                _userTradeCacheManager.RemoveUserFromUserWiseTradeCache(companyUserID);

                if (_userWiseTTCollectionData.ContainsKey(companyUserID))
                    _userWiseTTCollectionData.Remove(companyUserID);
                if (_userWiseAccountCollection.ContainsKey(companyUserID))
                {
                    _userWiseAccountCollection[companyUserID].Clear();
                    _userWiseAccountCollection.Remove(companyUserID);
                }
                if (_UserWisePermittedAUECCVCollection.ContainsKey(companyUserID))
                {
                    _UserWisePermittedAUECCVCollection[companyUserID].Clear();
                    _UserWisePermittedAUECCVCollection.Remove(companyUserID);
                }

                ClearSymbolCompressionCache(companyUserID);
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
                        Dictionary<string, string> tradingTicketData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(_userWiseTTCollectionData[companyUserID]);
                        tradingTicketData.Add("BrokerConnectionStatus", JsonHelper.SerializeObject(_tradeManagerExtension.GetAllCounterPartyConnectionSatus().Values));
                        tradingTicketData.Add("BrokerAlgoStrategies", JsonHelper.SerializeObject(_strategyProvider.GetAllStrategiesAlgoInfo()));

                        var userWiseTTData = new Dictionary<int, dynamic> { { companyUserID, tradingTicketData } };
                        var requestResponseModel = new RequestResponseModel(companyUserID: userWiseTTData.Keys.First(), data: JsonHelper.SerializeObject(userWiseTTData.Values.First()));
                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseTTCollectionBrokerDataResponse, requestResponseModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while producing trading ticket data");
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
                    ProduceKafkaRequests(companyUserId);
                    Logger.LogMsg(LoggerLevel.Information, "Kafka topic UserWiseAllocationDataRequest, UserWisePermittedAUECCVRequest, CompanyTradingPreferencesRequest produced for user:{0}", companyUserId);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        #region Compliance Permission Section
        /// <summary>
        /// KafkaManager_CompliancePermissionsResponse
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompliancePermissionsResponse(string topic, RequestResponseModel message)
        {
            try
            {
                if (!_compliancePermissionsResponseReceivedFromCommonData)
                {
                    _compliancePermissionsResponseReceivedFromCommonData = true;
                    Logger.LogMsg(LoggerLevel.Information, "Successfully connected Common Data Service.");
                }
                InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_COMPLIANCE_PERMISSION_RECEIVED);
                _compliancePermissionsData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data);
                if (_preTradeModule.Count > 0)
                    _preTradeModule.Clear();
                if (_compliancePermissions.Count > 0)
                    _compliancePermissions.Clear();
                _preTradeModule = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_PRE_TRADE_USERS]);
                _compliancePermissions = JsonHelper.DeserializeToObject<Dictionary<int, CompliancePermissions>>(_compliancePermissionsData[ComplianceConstants.CONST_COMPLIANCE_PERMISSIONS]);
                InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_COMPLIANCE_PERMISSION_PROCESSED);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Check if CompanyUserID has Pre Trade Permission for Manual Orders
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetPreTradeCheckForManualOrders(int userId)
        {
            try
            {
                if (_preTradeModule.ContainsKey(userId) && _compliancePermissions.ContainsKey(userId))
                    return _compliancePermissions[userId].RuleCheckPermission.IsPreTradeEnabled && _compliancePermissions[userId].RuleCheckPermission.IsApplyToManual;
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Check if CompanyUserID has Pre Trade Permission for Stage Orders
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool GetPreTradeCheckForStageOrders(int userId)
        {
            try
            {
                if (_preTradeModule.ContainsKey(userId) && _compliancePermissions.ContainsKey(userId))
                    return _compliancePermissions[userId].RuleCheckPermission.IsPreTradeEnabled && _compliancePermissions[userId].RuleCheckPermission.IsStaging;
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
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
                else if (_TradingServiceHeartbeatManager != null && !isRetryRequest)
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
                UnSubscribeProxy();
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
                    //if (_pricingService2ClientHeartbeatManager != null)
                    //    _pricingService2ClientHeartbeatManager.Dispose();

                    //if (_tradeServiceClientHeartbeatManager != null)
                    //    _tradeServiceClientHeartbeatManager.Dispose();

                    if (_TradingServiceHeartbeatManager != null)
                        _TradingServiceHeartbeatManager.Dispose();

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }
        #endregion

        #region kafka reporter
        private void Kafka_ProducerReporter(string topic)
        {
            Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_PRODUCE, topic);
        }

        private void Kafka_ConsumerReporter(string topic)
        {
            Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);
        }
        #endregion

        #region Produce Kafka Requests
        private void ProduceKafkaRequests(int userId)
        {
            try
            {
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWiseAllocationDataRequest, new RequestResponseModel(userId, ""));
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserWisePermittedAUECCVRequest, new RequestResponseModel(userId, ""));
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompanyTradingPreferencesRequest, new RequestResponseModel(userId, ""));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while producing kafka topics");
            }
        }

        private static async System.Threading.Tasks.Task ProduceTopicNHandleException(
            RequestResponseModel message,
            Exception ex,
            string topicName)
        {
            try
            {
                // safeguard against null message to avoid NullReferenceException
                if (message == null)
                {
                    Logger.LogMsg(LoggerLevel.Information, "Message is null in ProduceTopicNHandleException for topic {TopicName}, creating a new instance.",
                        topicName);
                    message = new RequestResponseModel(0, string.Empty, string.Empty);
                }

                message.Data = null;
                message.ErrorMsg = $"Error while producing to topic {topicName}, err msg:{ex.Message}";
                await KafkaManager.Instance.Produce(topicName, message);
                Logger.LogError(ex, $"Error while producing to topic {topicName}");
            }
            catch (Exception ex2)
            {
                Logger.LogError(ex2, $"ProduceTopicNHandleException encountered an error,  message might not have been published to event {topicName}");
            }
        }
        #endregion

        #region MTT ORDERS  

        private async void KafkaManager_PSTOrderRequestReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);

                    var executionType = Convert.ToString(info.ExecutionType);
                    Logger.LogMsg(LoggerLevel.Information, "Recevied PST Order request of execution type {0} with data:{1}", executionType, request.Data);

                    switch (executionType)
                    {
                        case MANUAL:
                            SendMultiTradeManualOrders(topic, request);
                            break;
                        case STAGE:
                            SendMultiTradeStageOrders(topic, request);
                            break;
                        case SEND:
                            SendMultiTradeLiveOrders(topic, request);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error while processing pst orders");
                }
                await System.Threading.Tasks.Task.CompletedTask;
            }
        }

        private async void KafkaManager_SendOrdersToMarketRequest(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Received multi send Order to Market trade liveOrder request from HotButton....");
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var orders = info.Orders;

                    bool isSingleOrder = orders.Count == 1;
                    SendMultiTradeLiveOrders(topic, request, isSingleOrder);
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "Error while processing Send Order to Market from Hot Button");
                }
            }
            await System.Threading.Tasks.Task.CompletedTask;
        }

        //this will create the pst allocationDetails and level1Id will be set in th client side.
        private async void KafkaManager_CreatePstAllocacatioPrefRequestReceived(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                var tradingTicketId = "";
                try
                {
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    tradingTicketId = info.TradingTicketId;
                    var orders = info.Orders;

                    List<OrderSingle> orderSingles = new List<OrderSingle>();

                    //if isStageAllowedforComplianceAlert, we expect orders with customa allocationid to be come from UI.
                    orderSingles = GetSingleOrderListForStageOrders(request, orders, true);

                    var allocationObj = HandlePstOperations(request, orderSingles, true);

                    if (allocationObj == null)
                        throw new Exception("Error while creating AllocationPreference object.");

                    string respData = JsonHelper.SerializeObject(new { AllocationDetail = allocationObj, TradingTicketId = info.TradingTicketId });
                    request.Data = respData;
                    await KafkaManager.Instance.Produce(TOPIC_CreatePstAllocationPrefResponse, request);
                }
                catch (Exception ex)
                {
                    request.Data = JsonHelper.SerializeObject(new { TradingTicketId = tradingTicketId });
                    request.ErrorMsg = $"Internal Server error :{ex.Message}, RequestId:{request.CorrelationId}";
                    await KafkaManager.Instance.Produce(TOPIC_CreatePstAllocationPrefResponse, request);
                }
            }
        }

        public async void KafkaManager_ProcessDataForCheckCompliance(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Received {0} request", "processDataForCheckComplaince");
                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var orders = info.Orders;
                    List<OrderSingle> orderSingles = GetSingleOrderListForStageOrders(request, orders, true);

                    //to get check compliance request, we need to create allocation, and save the detail in pttDetails table just like PTT.
                    HandlePstOperations(request, orderSingles, false);

                    var newReq = new RequestResponseModel(request.CompanyUserID,
                        JsonHelper.SerializeObject(orderSingles),
                        request.CorrelationId);

                    //produce to compliance alert service.
                    await KafkaManager.Instance.Produce(TOPIC_CheckComplianceFromBasket, newReq);
                }
                catch (Exception e)
                {
                    await ProduceTopicNHandleException(request, e, TOPIC_ComplianceAlertsData);
                }
            }
        }

        private async void SendMultiTradeStageOrders(string topic, RequestResponseModel request)
        {
            using (LoggerHelper.PushLoggingProperties(request.CorrelationId, request.RequestID, request.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Multi Trade stage order received");
                    // Only for Stage Order
                    _tradeManagerExtension.TradeStatus = string.Empty;
                    TradeStatusWithMessage.Clear();
                    _tradeManagerExtension.PermittedAUECCV = new List<string>();

                    dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                    var orders = info.Orders;

                    bool IsShortLocateParameterNotRequired = false;
                    string PSTStageOrderResponse = info.SendStageOrderForComplianceAlert;
                    if (!PSTStageOrderResponse.Equals(string.Empty))
                    {
                        IsShortLocateParameterNotRequired = true;
                    }

                    bool isStageAllowedforComplianceAlert = info.SendStageOrderForComplianceAlert == "YES";
                    List<OrderSingle> orderSingles = new List<OrderSingle>();

                    //if isStageAllowedforComplianceAlert, we expect orders with customa allocationid to be come from UI.
                    orderSingles = GetSingleOrderListForStageOrders(request, orders, IsShortLocateParameterNotRequired);

                    // Added this if block in order to get the updated value of _userPermittedAUECCV when the services are restarted
                    CheckForUserWisePermittedAUECCCollections(request);

                    Logger.LogMsg(LoggerLevel.Information, "Multi Trade stage OrderSingle object created with order counts {0}", orderSingles.Count);

                    // Check the trade conditions for all orders at once
                    if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                    {
                        _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];

                        bool allConditionsMet = true;

                        // Check trade conditions for all orders
                        foreach (var orderSingle in orderSingles)
                        {
                            Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                            if (orderSingle.IsUseCustodianBroker && orderSingle.AccountBrokerMapping != null)
                            {
                                accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(orderSingle.AccountBrokerMapping.ToString());
                            }
                            if ((!orderSingle.IsUseCustodianBroker && !_tradeManagerExtension.CheckTradeConditions(orderSingle))
                                || (orderSingle.IsUseCustodianBroker && !_tradeManagerExtension.CheckTradeConditions(orderSingle, accountBrokerMapping)))
                            {
                                allConditionsMet = false;
                                Logger.LogMsg(LoggerLevel.Information, "Multi Trade Stage Order Condition failed for order with qty {0} & level1Id {1} & isCustodianBroker {2} ",
                                    orderSingle.Quantity, orderSingle.Level1ID, orderSingle.IsUseCustodianBroker);
                                break; // If any condition fails, break out of the loop
                            }
                        }

                        if (allConditionsMet)
                        {
                            // If all trade conditions are met, proceed with compliance alert
                            if (GetPreTradeCheckForStageOrders(request.CompanyUserID) && !isStageAllowedforComplianceAlert)
                            {
                                Logger.LogMsg(LoggerLevel.Information, "Multi Trade Stage Order GetPreTradeCheckForStageOrders passed");
                                Dictionary<string, string> dataForStageOrderComplianceAlert = new Dictionary<string, string>
                                {
                                    { "Order", JsonHelper.SerializeObject(orderSingles) },
                                    { "userId", request.CompanyUserID.ToString() },
                                };

                                // Send all orders at once for compliance check
                                _ = KafkaManager.Instance.Produce(TOPIC_SendStageOrderForComplianceAlerts,
                                    new RequestResponseModel(request.CompanyUserID,
                                    JsonHelper.SerializeObject(dataForStageOrderComplianceAlert), request.CorrelationId));
                                TradeStatusWithMessage.Add(0, "StageOrderComplianceAlert");
                            }
                            else
                            {
                                Logger.LogMsg(LoggerLevel.Information, "Multi Trade Stage Order with GetPreTradeCheckForStageOrders conditions false (compliance response)");
                                // If compliance is not needed, send all orders at once
                                foreach (var orderSingle in orderSingles)
                                {
                                    AlertPopUpType popUpType = AlertPopUpType.None;

                                    if (!string.IsNullOrEmpty(info.SendStageOrderPopUpForComplianceAlert?.ToString()))
                                        popUpType = (AlertPopUpType)(Convert.ToInt32(info.SendStageOrderPopUpForComplianceAlert));

                                    if (popUpType != AlertPopUpType.PendingApproval)
                                        _tradeManagerExtension.SendValidatedTrades(orderSingle, true);
                                }

                                // Add success status message
                                if (!GetPreTradeCheckForStageOrders(request.CompanyUserID))
                                    TradeStatusWithMessage.Add(0, "TradeSuccessful");
                                else
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Multi Trade Stage order sent for Compliance operation with symbol");
                                    TradeStatusWithMessage.Add(0, "TradeSuccessfulCompliance");
                                }
                            }
                        }
                        else
                        {
                            Logger.LogMsg(LoggerLevel.Information, "Multi Trade Stage Order All condition check failed, Trade Status: {0}", _tradeManagerExtension.TradeStatus);
                            TradeStatusWithMessage.Add(0, _tradeManagerExtension.TradeStatus);
                        }
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId data. Cache may be incomplete or corrupt for multi stage order");
                        TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                    }

                    var result = new
                    {
                        TradeStatusWithMessage = TradeStatusWithMessage,
                        TradingTicketId = info.TradingTicketId,
                        //OrderList = isStageAllowedforComplianceAlert ? null : orderSingles,    //if complianceAlloweed come in req, dont send orderlist, 
                        IsPst = true
                    };
                    request.Data = JsonHelper.SerializeObject(result);
                    await KafkaManager.Instance.Produce(TOPIC_SendStageOrderResponse, request);
                    InformationReporter.GetInstance.Write(TradingServiceConstants.MSG_SEND_ORDER_FOR_STAGE_PROCESSED + request.CompanyUserID.ToString());
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(request, ex, TOPIC_SendStageOrderResponse);
                }
            }
        }

        private List<OrderSingle> GetSingleOrderListForStageOrders(RequestResponseModel request, dynamic orders, bool IsShortLocateParameterNotRequired)
        {
            List<OrderSingle> orderSingles = new List<OrderSingle>();

            foreach (var orderData in orders)
            {
                // Process each order (orderData) and create OrderSingle object
                OrderSingle orderSingle = CreateOrderFromJSON(orderData,
                    request.CompanyUserID, false,
                    IsShortLocateParameterNotRequired);
                orderSingle.IsStageRequired = false;
                orderSingle.IsInternalOrder = true;
                orderSingle.AvgPriceForCompliance = orderData.Price;
                orderSingle.IsSamsaraUser = true;
                SetTTPropertiesMapping(orderData, orderSingle);
                orderSingle.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderSingle.OrderSideTagValue);
                orderSingle.TransactionType = TagDatabaseManager.GetInstance.GetOrderSideText(orderSingle.OrderSideTagValue);
                orderSingle.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(orderSingle.CurrencyID);
                //orderSingle.FXConversionMethodOperator = Operator.M.ToString();
                //orderSingle.AvgPrice = orderData.Price;
                orderSingle.ContractMultiplier = orderData.ContractMultiplier == null ? 0 : orderData.ContractMultiplier;

                orderSingle.TransactionSource = TransactionSource.TradingTicket;

                // Add the order to the list
                orderSingles.Add(orderSingle);
            }

            return orderSingles;
        }

        private async void SendMultiTradeManualOrders(string topic, RequestResponseModel request)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Information, "Multi Trade manual order received");
                var pstTradeManager = new PstTradeManager(_allocationProxy.InnerChannel);
                //Only for Manual Order
                _tradeManagerExtension.TradeStatus = string.Empty;
                TradeStatusWithMessage.Clear();
                _tradeManagerExtension.PermittedAUECCV = new List<string>();

                dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                var orders = info.Orders;
                List<OrderSingle> orderSingles = new List<OrderSingle>();
                var multiTradeId = IDGenerator.GenerateMultiTradeId(); ;
                // Loop through each order in StageTradeHandler and collect orders in a list
                var msg = "";
                foreach (var orderData in orders)
                {
                    // Process each order (orderData) and create OrderSingle object
                    OrderSingle orderSingle = CreateOrderFromJSON(orderData, request.CompanyUserID, true);
                    orderSingle.IsManualOrder = orderSingle.IsInternalOrder = true;

                    SetTTPropertiesMapping(orderData, orderSingle);
                    orderSingle.AvgPriceForCompliance = 0;

                    orderSingle.IsStageRequired = true;
                    orderSingle.CumQty = 0;
                    orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;

                    SetTransactionSourceNFxRateInOrder(orderSingle, orderData);
                    orderSingle.MultiTradeName = pstTradeManager.GetMultiTradeName();
                    orderSingle.MultiTradeId = multiTradeId;
                    msg += $"Symbol:${orderSingle.Symbol},Quantity:${orderSingle.Quantity},TransType:${orderSingle.TransactionType};;";
                    // Add the order to the list
                    orderSingles.Add(orderSingle);
                }

                Logger.LogMsg(LoggerLevel.Information, "Multi Trade manual object created with order counts {0} and multiTradeId: {1} and orderDetals:{2}", orderSingles.Count, multiTradeId, msg);
                //Added this if block in order to get the updated value of _userPermittedAUECCV when the services are restarted
                CheckForUserWisePermittedAUECCCollections(request);

                AddDuplicateTradeResponseInUserCache(request, "");

                if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                {
                    _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];

                    bool allConditionsMet = true;

                    // Check trade conditions for all orders
                    int counter = 0;
                    foreach (var orderSingle in orderSingles)
                    {
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (orderSingle.IsUseCustodianBroker && orderSingle.AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(orderSingle.AccountBrokerMapping.ToString());
                        }

                        if ((!orderSingle.IsUseCustodianBroker && !_tradeManagerExtension.CheckTradeConditions(orderSingle))
                            || (orderSingle.IsUseCustodianBroker && !_tradeManagerExtension.CheckTradeConditions(orderSingle, accountBrokerMapping)))
                        {
                            TradeStatusWithMessage.Add(counter++, _tradeManagerExtension.TradeStatus);
                            allConditionsMet = false;
                            Logger.LogMsg(LoggerLevel.Information, "Multi Trade manual Order Condition failed for order with qty {0} & level1Id {1} & isCustodianBroker {2} ",
                             orderSingle.Quantity, orderSingle.Level1ID, orderSingle.IsUseCustodianBroker);
                            break;
                        }
                    }

                    if (allConditionsMet)
                    {
                        var successfullTradeCounter = 0;
                        foreach (var orderSingle in orderSingles)
                        {
                            bool isallowed = _tradeManagerExtension.SendValidatedTrades(orderSingle, true);
                            if (isallowed)
                            {
                                successfullTradeCounter++;
                                if (!GetPreTradeCheckForManualOrders(request.CompanyUserID))
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Chec PreTrade Condtion successfull for multiTradeId:{0} and qty:{1} and tranType:{2} ", multiTradeId, orderSingle.Quantity, orderSingle.TransactionType);
                                    TradeStatusWithMessage.Add(counter++, "TradeSuccessful");
                                }
                                else
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Multi Trade Manual order sent for Compliance operation for symbol {0} and qty:{1}", orderSingle.Symbol, orderSingle.Quantity);
                                    TradeStatusWithMessage.Add(counter++, "TradeSuccessfulCompliance");
                                }
                            }
                        }
                        TradeManagerExtension.GetInstance().SendMultiTradeDetails(multiTradeId, successfullTradeCounter, request.CompanyUserID);

                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "Multi Trade manual Order All condition check failed");
                    }
                }
                else
                {
                    Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId data. Cache may be incomplete or corrupt for multi trade manual order");
                    TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                }

                var result = new { TradeStatusWithMessage = TradeStatusWithMessage, TradingTicketId = info.TradingTicketId };
                request.Data = JsonHelper.SerializeObject(result);
                await KafkaManager.Instance.Produce(TOPIC_SendManualOrderResponse, request);
                Logger.LogMsg(LoggerLevel.Information, "Multi Trade Manual Order proccess successfully status msg:{0}", TradeStatusWithMessage);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(request, ex, TOPIC_SendManualOrderResponse);
            }
        }

        private void SetTransactionSourceNFxRateInOrder(OrderSingle orderSingle, dynamic orderData)
        {
            orderSingle.TransactionSource = TransactionSource.TradingTicket;
            orderSingle.FXRate = orderData.FXRate;
            orderSingle.IsSamsaraUser = true;
            if (orderSingle.VenueID == 0)
            {
                orderSingle.VenueID = 1;
                orderSingle.Venue = CachedDataManager.GetInstance.GetVenueText(orderSingle.VenueID);
            }
        }

        private async void SendMultiTradeLiveOrders(string topic, RequestResponseModel request, bool isDuplicateCheckReq = true)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Information, "Multi Trade live order received");
                var pstTradeManager = new PstTradeManager(_allocationProxy.InnerChannel);

                _tradeManagerExtension.TradeStatus = string.Empty;
                TradeStatusWithMessage.Clear();
                _tradeManagerExtension.PermittedAUECCV = new List<string>();

                dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
                var orders = info.Orders;
                List<OrderSingle> orderSingles = new List<OrderSingle>();

                var multiTradeId = IDGenerator.GenerateMultiTradeId();
                var msg = "";
                // Loop through each order in StageTradeHandler and collect orders in a list
                foreach (var orderData in orders)
                {
                    // Process each order (orderData) and create OrderSingle object
                    OrderSingle orderSingle = CreateOrderFromJSON(orderData, request.CompanyUserID);
                    SetTTPropertiesMapping(orderData, orderSingle);
                    orderSingle.AvgPriceForCompliance = orderData.Price;
                    orderSingle.PranaMsgType = (int)OrderFields.PranaMsgTypes.ORDStaged;
                    orderSingle.IsStageRequired = true;
                    SetTransactionSourceNFxRateInOrder(orderSingle, orderData);
                    orderSingle.MultiTradeName = pstTradeManager.GetMultiTradeName();
                    orderSingle.MultiTradeId = multiTradeId;
                    HandleOrderSingleUpdationForHotButtons(orderSingle, orderData);
                    msg += $"Symbol:${orderSingle.Symbol},Quantity:${orderSingle.Quantity},TransType:${orderSingle.TransactionType};;";
                    // Add the order to the list
                    orderSingles.Add(orderSingle);
                }

                Logger.LogMsg(LoggerLevel.Information, "Multi Trade live Orders count {0} and multiTradeId {1} and ordersDetails:{2}", orderSingles.Count, multiTradeId, msg);
                //Added this if block in order to get the updated value of _userPermittedAUECCV when the services are restarted
                CheckForUserWisePermittedAUECCCollections(request);

                AddDuplicateTradeResponseInUserCache(request, "");

                if (_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
                {
                    _tradeManagerExtension.PermittedAUECCV = _UserWisePermittedAUECCVCollection[request.CompanyUserID];

                    bool allConditionsMet = true;

                    // Check trade conditions for all orders
                    int counter = 0;
                    foreach (var orderSingle in orderSingles)
                    {
                        Dictionary<int, int> accountBrokerMapping = new Dictionary<int, int>();
                        if (orderSingle.IsUseCustodianBroker && orderSingle.AccountBrokerMapping != null)
                        {
                            accountBrokerMapping = JsonHelper.DeserializeToObject<Dictionary<int, int>>(orderSingle.AccountBrokerMapping.ToString());
                        }
                        if ((!orderSingle.IsUseCustodianBroker && !_tradeManagerExtension.CheckTradeConditions(orderSingle))
                            || (orderSingle.IsUseCustodianBroker && !_tradeManagerExtension.CheckTradeConditions(orderSingle, accountBrokerMapping)))
                        {
                            TradeStatusWithMessage.Add(counter++, _tradeManagerExtension.TradeStatus);
                            allConditionsMet = false;
                            Logger.LogMsg(LoggerLevel.Information, "Trade Check condition failed for multiTradeId:{0} and qty:{1} ", multiTradeId, orderSingle.Quantity);
                            break;
                        }
                    }
                    string serializedOrders = JsonConvert.SerializeObject(orderSingles, Formatting.Indented);

                    if (allConditionsMet)
                    {
                        var successfullTradeCounter = 0;
                        foreach (var orderSingle in orderSingles)
                        {
                            bool isallowed = _tradeManagerExtension.SendValidatedTrades(orderSingle, true, isDuplicateCheckReq);
                            if (isallowed)
                            {
                                successfullTradeCounter++;
                                if (!GetPreTradeCheckForManualOrders(request.CompanyUserID))
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Check PreTrade Condtion successfull for multiTradeId:{0} and qty:{1} and tranType:{2} ",
                                        multiTradeId, orderSingle.Quantity, orderSingle.TransactionType);
                                    TradeStatusWithMessage.Add(counter++, "TradeSuccessful");
                                }
                                else
                                {
                                    Logger.LogMsg(LoggerLevel.Information, "Multi Trade Live order sent for Compliance operation for symbol {0} & qty {1}",
                                        orderSingle.Symbol, orderSingle.Quantity);
                                    TradeStatusWithMessage.Add(counter++, "TradeSuccessfulCompliance");
                                }
                            }
                        }
                        TradeManagerExtension.GetInstance().SendMultiTradeDetails(multiTradeId, successfullTradeCounter, request.CompanyUserID);
                    }
                    else
                    {
                        Logger.LogMsg(LoggerLevel.Information, "Multi Trade live Order All condition check failed.");
                    }
                }
                else
                {
                    Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId data. Cache may be incomplete or corrupt for multi trade live order");
                    TradeStatusWithMessage.Add(0, PranaMessageConstants.MSG_SendOrderError);
                }

                userWiseDuplicateTradeResponse[request.CompanyUserID] = String.Empty;
                var result = new { TradeStatusWithMessage = TradeStatusWithMessage, TradingTicketId = info.TradingTicketId };
                request.Data = JsonHelper.SerializeObject(result);
                await KafkaManager.Instance.Produce(TOPIC_SendLiveOrderResponse, request);

                Logger.LogMsg(LoggerLevel.Information, "Multi Trade liveOrder proccess successfully for symbol with status msg:{0}", TradeStatusWithMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Live order encountered an error");
            }
        }

        private async void CheckForUserWisePermittedAUECCCollections(RequestResponseModel request)
        {
            if (!_UserWisePermittedAUECCVCollection.ContainsKey(request.CompanyUserID))
            {
                Logger.LogMsg(LoggerLevel.Information, "_UserWisePermittedAUECCVCollection does not contain userId as key");
                await KafkaManager.Instance.Produce(TOPIC_UserWisePermittedAUECCVRequest, new RequestResponseModel(request.CompanyUserID, ""));
            }
        }

        private AllocationOperationPreference HandlePstOperations(RequestResponseModel request, List<OrderSingle> orders, bool isPstOrder)
        {

            dynamic info = JsonHelper.DeserializeToObject<dynamic>(request.Data);
            var pstTradeManager = new PstTradeManager(_allocationProxy.InnerChannel);
            var pstReqDynamic = JsonHelper.DeserializeToObject<dynamic>(info.PstRequestObj.ToString());

            if (pstReqDynamic == null)
            {
                Logger.LogMsg(LoggerLevel.Fatal, "PstRequestObj is null or empty, AllocationOperationPreference will not be created");
                return null;//handle error
            }

            var pstRequestObj = new PTTRequestObject
            {
                Target = (decimal)info.PstRequestObj.Target,
                AddOrSet = new EnumerationValue(pstReqDynamic.AddOrSet?.DisplayText?.ToString(), pstReqDynamic.AddOrSet?.Value),
                MasterFundOrAccount = new EnumerationValue(pstReqDynamic.MasterFundOrAccount?.DisplayText?.ToString(), pstReqDynamic.MasterFundOrAccount?.Value),
                SelectedFeedPrice = (decimal)info.PstRequestObj.SelectedFeedPrice,
                Type = new EnumerationValue(pstReqDynamic.Type?.ToString(), pstReqDynamic.Type?.Value),
                IsUseRoundLot = (bool)info.PstRequestObj.IsUseRoundLot,
                CombinedAccountsTotalValue = (bool)info.PstRequestObj.CombinedAccountsTotalValue,
                CombineAccountEnumValue = new EnumerationValue(pstReqDynamic.CombineAccountEnumValue?.DisplayText?.ToString(), Convert.ToInt32(pstReqDynamic.CombineAccountEnumValue?.Value)),
                Symbol = pstReqDynamic.TickerSymbol.ToString(),
                TickerSymbol = pstReqDynamic.TickerSymbol.ToString()
            };//

            List<PTTResponseObject> pstResponseList = JsonHelper.DeserializeToObject<List<PTTResponseObject>>(info.PstResponseList.ToString());

            if (pstResponseList == null || pstResponseList.Count == 0)
            {
                Logger.LogMsg(LoggerLevel.Fatal, "pstResponseList is null or empty, AllocationOperationPreference will not be created");
                return null;//handle error
            }

            var oper = new AllocationPreferenceOperationHelper(_allocationProxy.InnerChannel);
            int prefId = int.MinValue;

            Dictionary<string, List<PTTResponseObject>> dictionaryOrderWise = pstTradeManager.CreateDictionaryOnOrderSides(pstResponseList);

            var orderNumber = 0;
            AllocationOperationPreference operationPreference = null;
            var multiTradeId = IDGenerator.GenerateMultiTradeId(); ;
            foreach (var orderResponseList in dictionaryOrderWise.Values)
            {
                //var order = orders.FirstOrDefault(x => x.OrderSideTagValue == orderResponseList[0].OrderSide);

                var orderSideTag = TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(orderResponseList[0].OrderSide);

                var order = orders.FirstOrDefault(x => x.OrderSideTagValue == orderSideTag);
                if (orderSideTag == "" || order == null)
                    throw new Exception("The trade could not be processed because the order side is invalid or no matching order was found. Please verify the order details and try again.");

                order.TransactionSource = TransactionSource.TradingTicket;
                order.TransactionSourceTag = (int)TransactionSource.PST;

                order.MultiTradeName = pstTradeManager.GetMultiTradeName();
                order.MultiTradeId = multiTradeId;
                if (orderNumber == 0)
                {
                    operationPreference = oper.CreateAllocationPreference(prefId, pstRequestObj, orderResponseList);
                    SetLevel1Id(order, orderResponseList, operationPreference.OperationPreferenceId);
                    orderNumber++;
                }
                else
                {
                    oper.CreateCheckListWisePrefAndOrder(pstRequestObj, orderResponseList, orderNumber, operationPreference);
                    SetLevel1Id(order, orderResponseList, operationPreference.OperationPreferenceId);
                    orderNumber++;
                }
            }

            pstResponseList.ForEach(x =>
                x.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideTagValueBasedOnId(x.OrderSide));
            //This method is also called for check complaince, we dont need to save pst detail for that cases, 
            var selectedFunds = JsonHelper.DeserializeToObject<string[]>(info.PstRequestObj.SelectedFundIds.ToString());
            string selectedFundsCommaSeparated = string.Join(", ", selectedFunds);
            if (isPstOrder)
                pstTradeManager.SavePTTPreferenceDetails(pstRequestObj,
                    pstResponseList,
                    operationPreference.OperationPreferenceId,
                    selectedFundsCommaSeparated);


            return operationPreference;

            void SetLevel1Id(OrderSingle order,
                       List<PTTResponseObject> orderResponseList,
                       int preferenceId)
            {
                var accIDs = orderResponseList.Select(o => o.AccountId).Distinct();
                order.Level1ID = preferenceId;

                order.OriginalAllocationPreferenceID = order.Level1ID;
            }
        }

        private void AddDuplicateTradeResponseInUserCache(RequestResponseModel request, string message)
        {
            try
            {
                if (!userWiseDuplicateTradeResponse.ContainsKey(request.CompanyUserID))
                {
                    userWiseDuplicateTradeResponse.Add(request.CompanyUserID, message);
                }
                else
                {
                    userWiseDuplicateTradeResponse[request.CompanyUserID] = message;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while setting duplicateTradeResponse in user cache");
            }
        }

        /// <summary>
        /// Symbol data snapshot response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="snapshotResponseData"></param>
        void ILiveFeedCallback.SnapshotResponse(SymbolData data, SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data != null && data.SelectedFeedPrice != 0)
                {
                    _currencyPairFxRateDict[data.Symbol] = data.SelectedFeedPrice;
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

        void ILiveFeedCallback.OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        void ILiveFeedCallback.LiveFeedConnected()
        {

        }

        void ILiveFeedCallback.LiveFeedDisConnected()
        {
        }
        #endregion

        #region Trade Attributes
        // <summary>
        /// Retrieves trade attribute values for a given user ID, filters the first 6 attributes, and publishes them to a Kafka topic.
        /// </summary>
        public async void KafkaManager_GetTradeAttributeValues(string topic, RequestResponseModel message)
        {
            try
            {
                int userId = message.CompanyUserID;
                List<string>[] tradeAttributes = AllocationClientDataManager.GetInstance.GetTradeAttributes(userId);
                string requestId = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data)["RequestId"];
                if (tradeAttributes == null || tradeAttributes.Length == 0)
                {
                    Logger.LogMsg(LoggerLevel.Verbose, "Trade attributes values are null or empty for {0} with CorrelationId: {1}", topic, message.CorrelationId);
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_TradeAttributeValuesResponse, new RequestResponseModel
                    {
                        ErrorMsg = "Trade attributes values is null or empty",
                        CorrelationId = message.CorrelationId
                    });
                    return;
                }

                var filteredTradeAttributeValues = tradeAttributes.Take(6).ToArray();
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_TradeAttributeValuesResponse, new RequestResponseModel(message.CompanyUserID,
                JsonHelper.SerializeObject(new
                {
                    IsSuccess = true,
                    Data = new
                    {
                        Values = tradeAttributes,
                        RequestId = requestId
                    }

                }),
                    message.CorrelationId
                ));
                Logger.LoggerWrite("Successfully sent trade attribute values from KafkaManager_GetTradeAttributeValues with CorrelationId: {0}", message.CorrelationId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error while producing trade attribute details to {topic}");
            }
        }
        #endregion

        #region Hot Buttons
        public void HandleOrderSingleUpdationForHotButtons(OrderSingle orderSingle, dynamic data)
        {
            try
            {
                if (orderSingle.TransactionSourceTag == (int)TransactionSource.HotButton)
                {
                    orderSingle.PranaMsgType = 2;
                    orderSingle.IsStageRequired = false;
                    orderSingle.ClOrderID = data.ClOrderID;
                    orderSingle.ParentClOrderID = data.ParentClOrderID;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "HandleOrderSingleUpdationForHotButtons encounter an error");
            }
        }
        #endregion
    }
}