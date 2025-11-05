using Castle.Windsor;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Constants;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.OptionCalculator.CalculationComponent;
using Prana.OptionCalculator.Common;
using Prana.OptionServer;
using Prana.PricingAnalysisModels;
using Prana.PricingService2.AmqpPlugin;
using Prana.PubSubService;
using Prana.PubSubService.Interfaces;
using Prana.QueueManager;
using Prana.RiskServer;
using Prana.MonitoringProcessor;
using Prana.SocketCommunication;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Prana.Global.Utilities;

namespace Prana.PricingService2
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class PricingService2 : IPricingService2, ILiveFeedCallback, IDisposable
    {
        #region Variables
        private IWindsorContainer _container;
        private List<IQueueProcessor> _inQueueProcessorList = new List<IQueueProcessor>();
        private List<IQueueProcessor> _outQueueProcessorList = new List<IQueueProcessor>();

        private bool _isLiveFeedConnected = false;

        private string _oldSymbol = string.Empty;

        private object _publishLock = new object();
        private ProxyBase<IPublishing> _proxyPublishing = null;

        private ServerHeartbeatManager _pricingService2HeartbeatManager;

        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        #endregion

        #region Public Properties
        private Prana.Interfaces.IPricingService _pricingService = null;
        public Prana.Interfaces.IPricingService PricingService
        {
            set
            {
                _pricingService = value;
                _pricingService.LiveFeedConnectionStatusChanged += new EventHandler<EventArgs<bool>>(LiveFeedConnectionStatus);
            }
        }
        #endregion

        #region Constructor
        public PricingService2()
        {
            ServicesHeartbeatSubscribersCollection.GetInstance().SubscribersUpdated += ServicesHeartbeatSubscribersCollection_SubscribersUpdated;
        }
        #endregion

        #region Private Methods
        private void LiveFeedConnectionStatus(object sender, EventArgs<bool> e)
        {
            try
            {
                _isLiveFeedConnected = e.Value;
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("LiveFeed Status Changed to " + _isLiveFeedConnected + " at " + DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt"), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                PublishPreparation(Topics.Topic_PricingService2LiveFeedConnectionData, _isLiveFeedConnected);
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

        private void StartCommunicationManager()
        {
            try
            {
                List<string> tradingAccounts = CommonDataCache.WindsorContainerManager.GetAllTradingAccounts();
                ServerCustomCommunicationManager.GetInstance().Initialise(_inQueueProcessorList, _outQueueProcessorList, tradingAccounts);
                ServerCustomCommunicationManager.GetInstance().Connected += new ConnectionMessageReceivedDelegate(User_Connected);
                ServerCustomCommunicationManager.GetInstance().Disconnected += new ConnectionMessageReceivedDelegate(User_Disconnected);
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

        private void User_Connected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                ConnectionProperties connProperties = e.Value;
                ServicesHeartbeatSubscribersCollection.GetInstance().AddSubscriber(connProperties.IdentifierName, null);

                QueueMessage qMsgConn = new QueueMessage();
                ArrayList logOnData = new ArrayList();
                logOnData.Add(connProperties);
                logOnData.Add(ServerCustomCommunicationManager.GetInstance().ConnectedClientIDNames);
                qMsgConn.Message = logOnData;
                qMsgConn.MsgType = FIXConstants.MSGLogon;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsgConn);
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

        private void User_Disconnected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                ConnectionProperties connProperties = e.Value;
                ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(connProperties.IdentifierName);

                QueueMessage qMsg = new QueueMessage();
                qMsg.Message = connProperties;
                qMsg.MsgType = FIXConstants.MSGLogout;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsg);
                if (connProperties.HandlerType == HandlerType.OptionCalcLive)
                {
                    SubscriberCollection.UnRegisterSymbols(connProperties.IdentifierID);
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

        private void TradeServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                ProxySymbolHelper.GetInstance().CreateSubscriptionServicesProxy();
                ProxySymbolHelper.UpdatePICacheWithCustomSymbols();
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

        private void TradeServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                ProxySymbolHelper.GetInstance().DisposeSubscriptionServicesProxy();
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

        private void TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                PublishPreparation(Topics.Topic_PricingService2LogsData, DateTime.UtcNow.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));
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

        private void HostPricingService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("PricingServiceEndpointAddress"))
                {
                    Prana.Interfaces.IPricingService pricingServices = _container.Resolve<Prana.Interfaces.IPricingService>();
                    PranaServiceHost.HostPranaService(pricingServices);
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

        private void HostRiskService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("PricingRiskServiceEndpointAddress"))
                {
                    IRiskServices riskServices = _container.Resolve<IRiskServices>();
                    PranaServiceHost.HostPranaService(riskServices);
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

        private void HostGreekAnalysisService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("PricingGreekAnalysisServiceEndpointAddress"))
                {
                    PranaServiceHost.HostPranaService(typeof(GreekAnalysisManager));
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

        private void HostMarketDataPermissionService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("PricingMarketDataPermissionServiceEndpointAddress"))
                {
                    IMarketDataPermissionService marketDataPermissionService = _container.Resolve<IMarketDataPermissionService>();
                    PranaServiceHost.HostPranaService(marketDataPermissionService);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                PublishPreparation(Topics.Topic_PricingService2LogsData, message);

                QueueMessage qmsg = new QueueMessage();
                qmsg.MsgType = CustomFIXConstants.MSG_ExceptionRaised;
                qmsg.Message = message;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qmsg);
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

        private void AmqpServer_Connected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                if (e.Value.IdentifierID != "MonitoringServices")
                {
                    ServicesHeartbeatSubscribersCollection.GetInstance().AddSubscriber(e.Value.IdentifierName, null);
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

        private void AmqpServer_Disconnected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(e.Value.IdentifierName);
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

        private void ServicesHeartbeatSubscribersCollection_SubscribersUpdated(object sender, EventArgs e)
        {
            try
            {
                PublishPreparation(Topics.Topic_PricingService2ConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void SetWinDaleParams(WinDaleParams winDaleParams)
        {
            try
            {
                OptionInputValuesCache.BinomialSteps = winDaleParams.BinomialSteps;
                OptionInputValuesCache.SelectedPricingModel = (PricingAnalysisModelsEnum)winDaleParams.PricingModel;
                OptionInputValuesCache.VolatilityIterations = winDaleParams.VolatilityIterations;
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

        private void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("PricingPublishingEndpointAddress");
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void PublishPreparation(string topicName, object publishData)
        {
            try
            {
                List<object> listData = new List<object>();
                listData.Add(publishData);

                MessageData messageData = new MessageData();
                messageData.EventData = listData;
                messageData.TopicName = topicName;
                CentralizePublish(messageData);
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

        private void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxyPublishing.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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

        #region ILiveFeedCallback
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data.Symbol.Equals(_oldSymbol))
                {
                    PublishPreparation(Topics.Topic_DMSymbolDataResponse, data);
                }
                if (data.Symbol.Equals("$WRSYMCNT") && data.LastPrice > 0D)
                {
                    PublishPreparation(Topics.Topic_DMSymbolLimitResponse, data);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void LiveFeedConnected()
        {

        }

        public void LiveFeedDisConnected()
        {

        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {

        }
        #endregion

        #region IPranaServiceCommon Methods
        public async System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container)
        {
            try
            {
                #region Client Heartbeat Setup
                _tradeServiceClientHeartbeatManager = new ClientHeartbeatManager<ITradeService>("TradeServiceEndpointAddress");
                _tradeServiceClientHeartbeatManager.ConnectedEvent += TradeServiceClientHeartbeatManager_ConnectedEvent;
                _tradeServiceClientHeartbeatManager.DisconnectedEvent += TradeServiceClientHeartbeatManager_DisconnectedEvent;
                _tradeServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;
                #endregion

                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                CreatePublishingProxy();
                PranaPubSubService.Initialize();

                HostPricingService();
                HostRiskService();
                HostGreekAnalysisService();
                HostMarketDataPermissionService();

                IQueueProcessor outMonitoringQueue = new QueueProcessor(HandlerType.MonitoringServices);
                _outQueueProcessorList.Add(outMonitoringQueue);

                //If provider type API
                if (CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.API))
                {
                    PricingServiceManager sm = (PricingServiceManager)_pricingService;
                    ILiveFeedAdapter pricingApiServices = _container.Resolve<ILiveFeedAdapter>();
                    sm.SetPricingApiService(pricingApiServices);
                }

                //initialize Core prices service and load balancer 
                SetWinDaleParams(await GetWinDaleParams());
                IQueueProcessor outPricingQueue = new BroadcastMemoryQueueManager();
                _pricingService.Initialize(outPricingQueue);

                //initialize risk manager which also handles option calculation
                PricingInstanceHandler.GetInstance.PricingService = _pricingService;
                StartCommunicationManager();
                PranaMonitoringProcessor.GetInstance.Initlise(outMonitoringQueue);

                await RiskAPIEachCalDurationThreasholdToLogUpdateTime(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RISKAPIEachCalDurationThreasholdToLog"].ToString()));

                #region Compliance Section
                try
                {
                    //Initialising only when pre trade compliance module is enabled
                    //TODO:Needs to change to only pre trade effect will be on esper communication manager as it depends on pricing status
                    if (ComplianceCacheManager.GetPreOrPostModuleEnabled() || CommonDataCache.Cache_Classes.HeatMapCacheManager.GetHeatMapModuleEnabled())
                    {
                        AmqpPluginManager.GetInstance().Initialise(this._pricingService);
                        AmqpPluginManager.Connected += AmqpServer_Connected;
                        AmqpPluginManager.Disconnected += AmqpServer_Disconnected;
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
                LogFileHelper.GetInstance().AddLogFileToZip();
                #endregion

                #region Server Heartbeat Setup
                _pricingService2HeartbeatManager = new ServerHeartbeatManager();
                #endregion

                if (CachedDataManager.CompanyMarketDataProvider.Equals(MarketDataProvider.FactSet))
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Market Data Provider Contract Type:  " + CachedDataManager.CompanyFactSetContractType, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }

                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 started at:- " + DateTime.Now + " (local time)", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return true;
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

        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Pricing Service2 successfully closed at:- " + DateTime.Now + " (local time)", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
            _container.Dispose();
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
                    ConnectionProperties connProperties = new ConnectionProperties();
                    connProperties.IdentifierID = subscriberName;
                    connProperties.IdentifierName = subscriberName;
                    ServerCustomCommunicationManager.GetInstance().HandleClientConnectWCF(connProperties);

                    QueueMessage qMsgConn = new QueueMessage();
                    ArrayList logOnData = new ArrayList();
                    logOnData.Add(connProperties);
                    logOnData.Add(ServerCustomCommunicationManager.GetInstance().ConnectedClientIDNames);
                    qMsgConn.Message = logOnData;
                    qMsgConn.MsgType = FIXConstants.MSGLogon;
                    PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsgConn);

                    // Subscriber added successfully
                    return true;
                }
                else if (_pricingService2HeartbeatManager != null && !isRetryRequest)
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
                    ConnectionProperties connProperties = new ConnectionProperties();
                    connProperties.IdentifierID = subscriberName;
                    connProperties.IdentifierName = subscriberName;
                    ServerCustomCommunicationManager.GetInstance().HandleClientDisconnectWCF(subscriberName);

                    QueueMessage qMsg = new QueueMessage();
                    qMsg.Message = connProperties;
                    qMsg.MsgType = FIXConstants.MSGLogout;
                    PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsg);
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
                PublishPreparation(Topics.Topic_PricingService2LiveFeedConnectionData, _isLiveFeedConnected);
                PublishPreparation(Topics.Topic_PricingService2ConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());

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
                string strFilePath = Logger.GetFlatFilelistnerLogFileName(LoggingConstants.LISTENER_RollingFile_ExceptionListener);
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

                if (ComplianceCacheManager.GetPreOrPostModuleEnabled())
                {
                    AmqpPlugin.AmqpPluginManager.GetInstance().Close();
                }

                OptionModelUserInputCache.SaveOMIDataToDB();

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

                IPublishing publishingObject = new Prana.PubSubService.Publishing();

                var taskList = new List<System.Threading.Tasks.Task>()
                {
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new ProxyBase<IPublishing>("PricingPublishingEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("PricingPublishing", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("PricingPublishing", false));
                        }
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", publishingObject);
                            hostedServicesStatus.Add(new HostedService("PricingSubscription", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("PricingSubscription", false));
                        }
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", publishingObject);
                            hostedServicesStatus.Add(new HostedService("TradeSubscription", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradeSubscription", false));
                        }
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("TradeSecMasterSyncService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradeSecMasterSyncService", false));
                        }
                    })
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
                UserSettingConstants.IsDebugModeEnabled = isDebugModeEnabled;

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

                return UserSettingConstants.IsDebugModeEnabled;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }
        #endregion

        #region IPricingService2 Methods
        public async System.Threading.Tasks.Task SetRiskLoggingStatus(bool isRiskLoggingEnabled)
        {
            try
            {
                UserSettingConstants.IsRiskLoggingEnabled = isRiskLoggingEnabled;

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> GetRiskLoggingStatus()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return UserSettingConstants.IsRiskLoggingEnabled;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task RiskAPIEachCalDurationThreasholdToLogUpdateTime(int timeValue)
        {
            try
            {
                UserSettingConstants.RISKAPIEachCalDurationThreasholdToLog = timeValue;

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<int> GetRiskAPIEachCalDurationThreasholdToLogUpdateTime()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return UserSettingConstants.RISKAPIEachCalDurationThreasholdToLog;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<List<EnumerationValue>> GetPricingModels()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PricingAnalysisModelsEnum));
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<WinDaleParams> GetWinDaleParams()
        {
            try
            {
                WinDaleParams winDaleParams = new WinDaleParams();
                if (File.Exists("PricingModelPreference.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load("PricingModelPreference.xml");
                    XmlElement root = doc.DocumentElement;
                    winDaleParams.PricingModel = Convert.ToInt32(root.SelectSingleNode("Model").InnerText);
                    winDaleParams.BinomialSteps = Convert.ToInt32(root.SelectSingleNode("Steps").InnerText);
                    winDaleParams.VolatilityIterations = Convert.ToInt32(root.SelectSingleNode("Iterations").InnerText);
                }
                else
                {
                    winDaleParams.PricingModel = Convert.ToInt32(PricingAnalysisModelsEnum.Black_Scholes);
                    winDaleParams.BinomialSteps = 25;
                    winDaleParams.VolatilityIterations = 2;
                }
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return winDaleParams;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SaveWinDaleParams(WinDaleParams winDaleParams)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create("PricingModelPreference.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("PricingModel");
                    writer.WriteElementString("Model", winDaleParams.PricingModel.ToString());
                    writer.WriteElementString("Steps", winDaleParams.BinomialSteps.ToString());
                    writer.WriteElementString("Iterations", winDaleParams.VolatilityIterations.ToString());
                    writer.WriteEndDocument();
                }
                SetWinDaleParams(winDaleParams);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<MarketDataProvider?> GetFeedProvider()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return CachedDataManager.CompanyMarketDataProvider;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<SecondaryMarketDataProvider?> GetSecondaryFeedProvider()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return CachedDataManager.SecondaryCompanyMarketDataProvider;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task UpdateLiveFeedDetails(string username, string password, [Optional, DefaultParameterValue("")] string host,
             [Optional, DefaultParameterValue("")] string port, [Optional, DefaultParameterValue("")] string supportUsername, [Optional, DefaultParameterValue("")] string supportPassword)
        {
            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.Esignal)
                {
                    PublicConst.g_username = username;
                    PublicConst.g_password = password;
                    PublicConst.g_internetAddress = host;

                    ESignalCredentialManager.SaveESignalFile(username + Seperators.SEPERATOR_6 + password + Seperators.SEPERATOR_6 + host);
                }
                else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet)
                {
                    FactSetAdapter.FactSetManager.GetInstance().UpdateCredentials(username, password, host, port, supportUsername, supportPassword);
                }
                else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV)
                {
                    ActivAdapter.ActivManager.GetInstance().UpdateCredentials(username, password);
                }
                else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                {
                    SAPIAdapter.SAPIManager.GetInstance().UpdateCredentials(username, password, port);
                }

                _pricingService.RestartLiveFeed();

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SecondaryUpdateLiveFeedDetails(string username, string password)
        {
            try
            {
                if (CachedDataManager.SecondaryCompanyMarketDataProvider == SecondaryMarketDataProvider.BloombergDLWS)
                {
                    BloombergAdapter.BloombergManager.GetInstance().UpdateCredentials(username, password);
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task RestartLiveFeed()
        {
            try
            {
                _pricingService.RestartLiveFeed();

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<List<string>> DataManagerSetup()
        {
            List<string> credentials = new List<string>();

            try
            {
                if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.Esignal)
                {
                    if (PublicConst.ParseINI())
                    {
                        credentials.Add(PublicConst.g_username);
                        credentials.Add(PublicConst.g_password);
                        credentials.Add(PublicConst.g_internetAddress);

                        if (String.IsNullOrWhiteSpace(PublicConst.g_internetAddress) || PublicConst.g_internetAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                        {
                            _pricingService.RequestSymbol_TTandPTT("$WRSYMCNT", this, false);
                        }
                        else
                        {
                            _pricingService.RequestSymbol_TTandPTT(eSignBin.ROSCOMMAND_TICK_PROFILEUSER, this, true);
                            _pricingService.RequestSymbol_TTandPTT(eSignBin.ROSCOMMAND_OTHER_PROFILEUSER, this, true);
                        }
                    }
                    else
                        return null;
                }
                else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet)
                {
                    credentials = FactSetAdapter.FactSetManager.GetInstance().GetCredentials();
                }
                else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV)
                {
                    credentials = ActivAdapter.ActivManager.GetInstance().GetCredentials();
                }
                else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI)
                {
                    credentials = SAPIAdapter.SAPIManager.GetInstance().GetCredentials();
                }
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

            return credentials;
        }

        public async System.Threading.Tasks.Task<List<string>> SecondaryDataManagerSetup()
        {
            List<string> credentials = new List<string>();

            try
            {
                if (CachedDataManager.SecondaryCompanyMarketDataProvider == SecondaryMarketDataProvider.BloombergDLWS)
                {
                    credentials = BloombergAdapter.BloombergManager.GetInstance().GetCredentials();
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

            return credentials;
        }

        public async System.Threading.Tasks.Task DataManagerClose()
        {
            try
            {
                _pricingService.RemoveSymbol_TTandPTT(_oldSymbol, this);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task RequestSymbol(string requestedSymbol)
        {
            try
            {
                _pricingService.RemoveSymbol_TTandPTT(_oldSymbol, this);
                _pricingService.RequestSymbol_TTandPTT(requestedSymbol, this, false);
                _oldSymbol = requestedSymbol;
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task GetServices()
        {
            try
            {
                _pricingService.RequestSymbol_TTandPTT(eSignBin.ROSCOMMAND_TICK_PROFILEUSER, this, true);
                _pricingService.RequestSymbol_TTandPTT(eSignBin.ROSCOMMAND_OTHER_PROFILEUSER, this, true);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task InitializeLiveFeedViewerData()
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

        public async System.Threading.Tasks.Task<List<SymbolData>> GetLiveFeedDataList()
        {
            try
            {
                return await LiveFeedManager.LiveFeedManager.GetInstance().GetLiveFeedDataListAsync();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<string, SymbolData>> GetUpdatedLiveFeedDataFromLiveCache()
        {
            try
            {
                return await LiveFeedManager.LiveFeedManager.GetInstance().GetUpdatedLiveFeedDataFromCache();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async Task<object> GetLiveDataDirectlyFromFeed()
        {
            try
            {
                return await LiveFeedManager.LiveFeedManager.GetInstance().GetLiveDataDirectlyFromFeed();

            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {
            try
            {
                await LiveFeedManager.LiveFeedManager.GetInstance().SetDebugEnableDisable(isDebugEnable, pctTolerance);

            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async Task<Tuple<bool, double>> GetDebugEnableDisableParams()
        {
            try
            {
                var data = await LiveFeedManager.LiveFeedManager.GetInstance().GetDebugEnableDisableParams();
                return data;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task StopLiveFeedViewerData()
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

        public async System.Threading.Tasks.Task DeleteAdvisedSymbol(string symbol)
        {
            try
            {
                _pricingService.DeleteAdvisedSymbol(symbol);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<List<string>> GetAdvicedSymbols()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _pricingService.GetAdvicedSymbols();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<string, string>> GetTickersLastStatusCode()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _pricingService.GetTickersLastStatusCode();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<string, int>> GetSubscriptionInformation()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _pricingService.GetSubscriptionInformation();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<string, string>> GetUserInformation()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _pricingService.GetUserInformation();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<List<Dictionary<string, string>>> GetUserPermissionsInformation()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _pricingService.GetUserPermissionsInformation();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// This method is for fetching FactSet Contract Type
        /// </summary>
        /// <returns>FactSetContractType</returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public async Task<FactSetContractType?> GetFactsetContractType()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return CachedDataManager.CompanyFactSetContractType;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        #region MarketDataAdapter
        public async System.Threading.Tasks.Task<Dictionary<string, MarketDataSymbolResponse>> GetAllMarketDataSymbolInformation()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
                var data = Prana.MarketDataAdapter.Common.MarketDataAdapterExtension.GetAllMarketDataSymbolInformation();
                return data;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// Get Subscribed Symbols Monitoring Data
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<DataTable> GetSubscribedSymbolsMonitoringData()
        {
            DataTable dataTable = new DataTable("MonitoringData");
            dataTable.Columns.Add(new DataColumn("Ticker Symbol (Nirvana End)", typeof(string)));
            dataTable.Columns.Add(new DataColumn(CachedDataManager.CompanyMarketDataProvider + " Symbol (Nirvana End)", typeof(string)));
            dataTable.Columns.Add(new DataColumn("AUEC ID", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Asset Category", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Ticker Symbol (" + CachedDataManager.CompanyMarketDataProvider + " End)", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Realtime/Delayed", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Delay Interval", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Subscription Count", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Last Status Code", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Update Time", typeof(string)));

            try
            {
                List<string> nirvanaSubscribedSymbols = _pricingService.GetAdvicedSymbols();
                Dictionary<string, MarketDataSymbolResponse> nirvanaMarketDataSymbolData = Prana.MarketDataAdapter.Common.MarketDataAdapterExtension.GetAllMarketDataSymbolInformation();
                Dictionary<string, SymbolData> nirvanaSubscribedSymbolsData = Prana.MarketDataAdapter.Common.MarketDataAdapterExtension.GetSnapshotsSymbolData();
                Dictionary<string, string> nirvanaSymbolsLastStatusCode = _pricingService.GetTickersLastStatusCode();
                Dictionary<string, int> liveFeedSubscribedSymbols = _pricingService.GetSubscriptionInformation();

                foreach (string symbol in nirvanaSubscribedSymbols)
                {
                    DataRow symbolRow = dataTable.NewRow();
                    symbolRow[0] = symbol;
                    if (nirvanaMarketDataSymbolData.ContainsKey(symbol))
                    {
                        switch (CachedDataManager.CompanyMarketDataProvider)
                        {
                            case MarketDataProvider.ACTIV:
                                symbolRow[1] = nirvanaMarketDataSymbolData[symbol].ActivSymbol;
                                break;
                            case MarketDataProvider.FactSet:
                                symbolRow[1] = nirvanaMarketDataSymbolData[symbol].FactSetSymbol;
                                break;
                            case MarketDataProvider.SAPI:
                                symbolRow[1] = nirvanaMarketDataSymbolData[symbol].BloombergSymbol;
                                break;
                        }
                        symbolRow[2] = nirvanaMarketDataSymbolData[symbol].AUECID;
                        symbolRow[3] = nirvanaMarketDataSymbolData[symbol].AssetCategory;
                    }

                    if (nirvanaSubscribedSymbolsData.ContainsKey(symbol))
                    {
                        SymbolData symbolData = nirvanaSubscribedSymbolsData[symbol];

                        if (liveFeedSubscribedSymbols.ContainsKey(symbolData.Symbol))
                        {
                            switch (CachedDataManager.CompanyMarketDataProvider)
                            {
                                case MarketDataProvider.ACTIV:
                                    symbolRow[4] = symbolData.Symbol;
                                    symbolRow[5] = symbolData.PricingStatus;
                                    symbolRow[6] = symbolData.DelayInterval;
                                    symbolRow[7] = liveFeedSubscribedSymbols[symbolData.Symbol];
                                    symbolRow[9] = symbolData.UpdateTime.ToLocalTime().ToString("MM/dd/yyy hh:mm:ss");
                                    break;
                                case MarketDataProvider.FactSet:
                                    break;
                                case MarketDataProvider.SAPI:
                                    symbolRow[4] = symbolData.Symbol;
                                    symbolRow[5] = symbolData.PricingStatus;
                                    symbolRow[6] = symbolData.DelayInterval;
                                    symbolRow[7] = liveFeedSubscribedSymbols[symbolData.Symbol];
                                    symbolRow[9] = symbolData.UpdateTime.ToLocalTime().ToString("MM/dd/yyy hh:mm:ss");
                                    break;
                            }
                        }

                        if (nirvanaSymbolsLastStatusCode.ContainsKey(symbol))
                            symbolRow[8] = nirvanaSymbolsLastStatusCode[symbol];
                    }

                    dataTable.Rows.Add(symbolRow);
                }

                foreach (KeyValuePair<string, int> symbol in liveFeedSubscribedSymbols)
                {
                    if (!nirvanaSubscribedSymbols.Contains(symbol.Key))
                        dataTable.Rows.Add(string.Empty, string.Empty, null, string.Empty, symbol.Key, string.Empty, null, symbol.Value);
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;
            return dataTable;
        }

        public async System.Threading.Tasks.Task RefreshMarketDataSymbolInformation(string tickerSymbol)
        {
            try
            {
                Prana.MarketDataAdapter.Common.MarketDataAdapterExtension.RefreshMarketDataSymbolInformation(tickerSymbol);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<string, SymbolData>> GetSnapshotsSymbolData()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return Prana.MarketDataAdapter.Common.MarketDataAdapterExtension.GetSnapshotsSymbolData();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// This method get the data from DB
        /// </summary>
        public async System.Threading.Tasks.Task<DataSet> GetSAPIRequestFieldData(string requestField)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _pricingService.GetSAPIRequestFieldData(requestField);

            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// This Method saves the Data in the DB
        /// </summary>
        public async System.Threading.Tasks.Task SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                _pricingService.SaveSAPIRequestFieldData(saveDataSetTemp, requestField);
                Prana.MarketDataAdapter.Common.MarketDataAdapterExtension.UpdateBloombergDictionary(saveDataSetTemp, requestField);
                if (requestField != "Snapshot")
                    SAPIAdapter.SAPIManager.GetInstance().ResubscribeSymbols();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        #endregion

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
                    if (_tradeServiceClientHeartbeatManager != null)
                        _tradeServiceClientHeartbeatManager.Dispose();

                    if (_pricingService2HeartbeatManager != null)
                        _pricingService2HeartbeatManager.Dispose();

                    if (_proxyPublishing != null)
                        _proxyPublishing.Dispose();

                    PranaServiceHost.CleanUp();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }
        #endregion
    }
}