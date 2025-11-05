using Castle.Windsor;
using Prana.BBGImportManager;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.CommonDataCache.Cache_Classes;
using Prana.CoreService.Interfaces;
using Prana.ExpnlService.AmqpPlugin;
using Prana.ExpnlService.DataDumper;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService;
using Prana.PubSubService.Interfaces;
using Prana.MonitoringProcessor;
using Prana.SocketCommunication;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace Prana.ExpnlService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ExpnlService : IExpnlService, IDisposable
    {
        #region Variables
        private IWindsorContainer _container = null;
        private bool _isExPNLRunning = false;
        private bool _isPricingService2Connected = false;
        private bool _isTradeServiceConnected = false;
        private bool _isLiveFeedConnected = false;
        private string _expnlCompression = ConfigurationManager.AppSettings["ExpnlCompression"].ToString().Trim();
        private bool _isDataDumperEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDataDumper"]);
        private bool _isDataDumperRunning = false;
        private int _timerRefreshInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerRefreshInterval"]);

        private LiveFeedConnectionStatus _liveFeedConnectionStatus = new LiveFeedConnectionStatus();

        private object _publishLock = new object();
        private ProxyBase<IPublishing> _proxyPublishing = null;

        private ServerHeartbeatManager _expnlServiceHeartbeatManager;

        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        #endregion

        #region Constructor
        public ExpnlService()
        {
            ServicesHeartbeatSubscribersCollection.GetInstance().SubscribersUpdated += ServicesHeartbeatSubscribersCollection_SubscribersUpdated;
        }
        #endregion

        #region Private Methods
        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                PublishPreparation(Topics.Topic_ExpnlServiceLogsData, message);

                QueueMessage qmsg = new QueueMessage();
                qmsg.MsgType = CustomFIXConstants.MSG_ExceptionRaised;
                qmsg.Message = message;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qmsg);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ServicesHeartbeatSubscribersCollection_SubscribersUpdated(object sender, EventArgs e)
        {
            try
            {
                PublishPreparation(Topics.Topic_ExpnlServiceConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void CreatePublishingProxy()
        {
            try
            {
                _proxyPublishing = new ProxyBase<IPublishing>("ExpnlPublishingEndpointAddress");
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

        private void HostService<T>()
        {
            try
            {
                var service = _container.Resolve<T>();
                PranaServiceHost.HostPranaService(service);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void SetPricingService2ConnectionStatus(bool isConnected)
        {
            try
            {
                PublishPreparation(Topics.Topic_ExpnlServicePricingConnectionData, isConnected);
                PranaMonitoringProcessor.GetInstance.SendStatusMessages("PricingService2", _isPricingService2Connected);
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

        private void SetTradeServiceConnectionStatus(bool isConnected)
        {
            try
            {
                PublishPreparation(Topics.Topic_ExpnlServiceTradeConnectionData, isConnected);

                PranaMonitoringProcessor.GetInstance.SendStatusMessages("TradeService", _isTradeServiceConnected);
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

        private void ClientBroadCastingManager_Connected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                ConnectionProperties connProperties = e.Value;

                if (connProperties.IdentifierID != "MonitoringServices")
                {
                    ServicesHeartbeatSubscribersCollection.GetInstance().AddSubscriber(connProperties.IdentifierName, null);
                    ServiceManager.GetInstance().HandleExPnlSubscription(connProperties.IdentifierID);
                }

                QueueMessage qMsgConn = new QueueMessage();
                ArrayList logOnData = new ArrayList();
                logOnData.Add(connProperties);
                logOnData.Add(ServerCustomCommunicationManager.GetInstance().ConnectedClientIDNames);
                qMsgConn.Message = logOnData;
                qMsgConn.MsgType = FIXConstants.MSGLogon;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsgConn);

                PranaMonitoringProcessor.GetInstance.SendStatusMessages("TradeService", _isTradeServiceConnected);
                PranaMonitoringProcessor.GetInstance.SendStatusMessages("PricingService2", _isPricingService2Connected);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ClientBroadCastingManager_Disconnected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                ConnectionProperties connProperties = e.Value;
                ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(connProperties.IdentifierName);

                QueueMessage qMsg = new QueueMessage();
                qMsg.Message = connProperties;
                qMsg.MsgType = FIXConstants.MSGLogout;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsg);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void Stop()
        {
            try
            {
                if (_isExPNLRunning)
                {
                    try
                    {
                        if (ComplianceCacheManager.GetPreOrPostModuleEnabled())
                            AmqpPluginManager.GetInstance().Close();
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

                    BBGFileWatcher.GetInstance().Dispose();
                    SessionManager.DistinctAccountPermissionSets.Clear();

                    ServiceManager.GetInstance().ClientBroadCastingManager.Connected -= new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Connected);
                    ServiceManager.GetInstance().ClientBroadCastingManager.Disconnected -= new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Disconnected);
                    ServiceManager.GetInstance().PreferencesUpdated -= new EventHandler<UserPreferencesEventArgs>(MainForm_PreferencesUpdated);
                    ServiceManager.GetInstance().UserInitiatedDataRefreshed -= new EventHandler(MainForm_UserInitiatedDataRefreshed);
                    ServiceManager.GetInstance().UserDataRefreshedRejected -= new EventHandler(MainForm_UserDataRefreshedRejected);
                    ServiceManager.GetInstance().UserDataRefreshCompleted -= new EventHandler(MainForm_UserDataRefreshCompleted);
                    ServiceManager.GetInstance().LogOnScreenToMain -= new EventHandler(MainForm_LogAUECDatesToMain);
                    ServiceManager.GetInstance().Stop();
                    OrderFillManager.GetInstance().Dispose();

                    PublishPreparation(Topics.Topic_ExpnlServiceConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());

                    SetTradeServiceConnectionStatus(false);
                    SetPricingService2ConnectionStatus(false);
                    _isExPNLRunning = false;

                    CleanUp();
                    Environment.Exit(0);
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

        private void PricingService2ClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                _isPricingService2Connected = true;
                SetPricingService2ConnectionStatus(_isPricingService2Connected);

                _liveFeedConnectionStatus.Subscribe();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void PricingService2ClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                _isPricingService2Connected = false;
                SetPricingService2ConnectionStatus(_isPricingService2Connected);

                _liveFeedConnectionStatus.LiveFeedDisConnected();

                QueueMessage qMsg = new QueueMessage();
                qMsg.Message = "PricingService2 disconnected from ExpnlService";
                qMsg.MsgType = CustomFIXConstants.MSG_ExceptionRaised;
                PranaMonitoringProcessor.GetInstance.ProcessMessage(qMsg);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                PublishPreparation(Topics.Topic_ExpnlServiceLogsData, DateTime.UtcNow.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));
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
                _isTradeServiceConnected = true;
                SetTradeServiceConnectionStatus(_isTradeServiceConnected);
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
                _isTradeServiceConnected = false;
                SetTradeServiceConnectionStatus(_isTradeServiceConnected);
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

        private void PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                PublishPreparation(Topics.Topic_ExpnlServiceLogsData, DateTime.UtcNow.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));
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

        private void LiveFeedConnectionStatusChanged(object sender, EventArgs<bool> e)
        {
            try
            {
                _isLiveFeedConnected = e.Value;
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("LiveFeed Status Changed to " + _isLiveFeedConnected + " at " + DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt"), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                PublishPreparation(Topics.Topic_ExpnlServiceLiveFeedConnectionData, _isLiveFeedConnected);
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

        private void MainForm_PreferencesUpdated(Object sender, UserPreferencesEventArgs updatedPrefs)
        {
            CommonCacheHelper.XPercentOfAvgVolume = updatedPrefs.XPercentOfAvgVolume;
        }

        private void MainForm_LogAUECDatesToMain(object sender, EventArgs e)
        {
            GetInstance_InformationReceived(sender, new LoggingEventArgs<string>(sender.ToString()));
        }

        private void MainForm_UserDataRefreshedRejected(object sender, EventArgs e)
        {
            try
            {
                String[] username = new String[3];
                username = sender.ToString().Split(Seperators.SEPERATOR_2);
                GetInstance_InformationReceived(sender, new LoggingEventArgs<string>("Data refresh at <" + DateTime.UtcNow + "> by the user : " + username[1] + " is Rejected as Data refresh is ongoing for user:" + username[2]));
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

        private void MainForm_UserInitiatedDataRefreshed(object sender, EventArgs e)
        {
            try
            {
                String[] username = new String[10];
                username = sender.ToString().Split(Seperators.SEPERATOR_2);
                GetInstance_InformationReceived(sender, new LoggingEventArgs<string>("Data refresh was initiated at <" + DateTime.UtcNow + "> by the user : " + username[1]));
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

        private void MainForm_UserDataRefreshCompleted(object sender, EventArgs e)
        {
            GetInstance_InformationReceived(sender, new LoggingEventArgs<string>("Data refresh was completed at <" + DateTime.UtcNow + ">"));
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
        #endregion

        #region IPranaServiceCommon Methods
        public async System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container)
        {
            try
            {
                #region Client Heartbeat Setup
                _pricingService2ClientHeartbeatManager = new ClientHeartbeatManager<IPricingService2>("PricingService2EndpointAddress");
                _pricingService2ClientHeartbeatManager.ConnectedEvent += PricingService2ClientHeartbeatManager_ConnectedEvent;
                _pricingService2ClientHeartbeatManager.DisconnectedEvent += PricingService2ClientHeartbeatManager_DisconnectedEvent;
                _pricingService2ClientHeartbeatManager.AnotherInstanceSubscribedEvent += PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent;

                _tradeServiceClientHeartbeatManager = new ClientHeartbeatManager<ITradeService>("TradeServiceEndpointAddress");
                _tradeServiceClientHeartbeatManager.ConnectedEvent += TradeServiceClientHeartbeatManager_ConnectedEvent;
                _tradeServiceClientHeartbeatManager.DisconnectedEvent += TradeServiceClientHeartbeatManager_DisconnectedEvent;
                _tradeServiceClientHeartbeatManager.AnotherInstanceSubscribedEvent += TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent;
                #endregion

                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                CreatePublishingProxy();
                PranaPubSubService.Initialize();

                _liveFeedConnectionStatus.LiveFeedConnectionStatusChanged += new EventHandler<EventArgs<bool>>(LiveFeedConnectionStatusChanged);

                while (!_isPricingService2Connected || !_isTradeServiceConnected)
                {
                    Console.WriteLine("Waiting for PricingService2 & TradeService...");
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Waiting for PricingService2 & TradeService", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    Thread.Sleep(2000);
                }

                ServiceManager.GetInstance().SetRunningView(_expnlCompression);

                if (ServiceManager.GetInstance().Start())
                {
                    await UpdateRefreshTimeInterval(_timerRefreshInterval);

                    if (!PranaServiceHost.IsServiceHosted("ExpnlCalculationServiceEndpointAddress"))
                        HostService<IExpnlCalculationService>();

                    ServiceManager.GetInstance().ClientBroadCastingManager.Connected += new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Connected);
                    ServiceManager.GetInstance().ClientBroadCastingManager.Disconnected += new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Disconnected);
                    ServiceManager.GetInstance().PreferencesUpdated += new EventHandler<UserPreferencesEventArgs>(MainForm_PreferencesUpdated);
                    ServiceManager.GetInstance().UserInitiatedDataRefreshed += new EventHandler(MainForm_UserInitiatedDataRefreshed);
                    ServiceManager.GetInstance().UserDataRefreshedRejected += new EventHandler(MainForm_UserDataRefreshedRejected);
                    ServiceManager.GetInstance().UserDataRefreshCompleted += new EventHandler(MainForm_UserDataRefreshCompleted);
                    ServiceManager.GetInstance().LogOnScreenToMain += new EventHandler(MainForm_LogAUECDatesToMain);

                    _isExPNLRunning = true;

                    BBGFileWatcher.GetInstance().SetupBBGFileWatcher();

                    #region Compliance section
                    try
                    {
                        if (ComplianceCacheManager.GetPreOrPostModuleEnabled() || HeatMapCacheManager.GetHeatMapModuleEnabled())
                        {
                            AmqpPluginManager.GetInstance().Initialise();
                            AmqpPluginManager.Connected += AmqpServer_Connected;
                            AmqpPluginManager.Disconnected += AmqpServer_Disconnected;
                        }
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
                    LogFileHelper.GetInstance().AddLogFileToZip();
                    #endregion

                    await StartDataDumper();

                    #region Server Heartbeat Setup
                    _expnlServiceHeartbeatManager = new ServerHeartbeatManager();
                    #endregion

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ExpnlService started at:- " + DateTime.Now + " (local time)", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    // Awaiting for a completed task to make function asynchronous
                    await System.Threading.Tasks.Task.CompletedTask;

                    return true;
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Unable to start ExpnlService", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    Console.WriteLine("Unable to start ExpnlService");
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

        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.

            Console.WriteLine("Shutting down service.");
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ExpnlService successfully closed at:- " + DateTime.Now + " (local time) ", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
                else if (_expnlServiceHeartbeatManager != null && !isRetryRequest)
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
                PublishPreparation(Topics.Topic_ExpnlServiceConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());
                PublishPreparation(Topics.Topic_ExpnlServicePricingConnectionData, _isPricingService2Connected);
                PublishPreparation(Topics.Topic_ExpnlServiceTradeConnectionData, _isTradeServiceConnected);
                PublishPreparation(Topics.Topic_ExpnlServiceLiveFeedConnectionData, _isLiveFeedConnected);
                PublishPreparation(Topics.Topic_ExpnlServiceCompressionData, _expnlCompression);

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
                string strFilePath = Logger.GetFlatFilelistnerLogFileName(LoggingConstants.LISTENER_RollingFileErrorListener);
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
                Console.WriteLine("Shutting down service.");

                await StopDataDumper();
                Stop();
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
                ILiveFeedCallback liveFeedConnectionStatusObject = new LiveFeedConnectionStatus();

                var taskList = new List<System.Threading.Tasks.Task>()
                {
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", liveFeedConnectionStatusObject);
                            hostedServicesStatus.Add(new HostedService("PricingService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("PricingService", false));
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
                            var proxy = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("TradePositionService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradePositionService", false));
                        }
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("TradeCashService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradeCashService", false));
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
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new ProxyBase<IPublishing>("ExpnlPublishingEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("ExpnlPublishing", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("ExpnlPublishing", false));
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

        #region IExpnlService Methods
        public async System.Threading.Tasks.Task UpdateRefreshTimeInterval(int seconds)
        {
            try
            {
                _timerRefreshInterval = seconds;
                ServiceManager.GetInstance().UpdateCalculationInterval(seconds);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task RefreshData()
        {
            try
            {
                if (_isExPNLRunning)
                {
                    ServiceManager.GetInstance().RefreshData();
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Service not started", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task StartDataDumper()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                if (_isDataDumperEnabled)
                {
                    DataDumpProcessor.Instance.Start();
                    _isDataDumperRunning = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task StopDataDumper()
        {
            try
            {
                if (_isDataDumperEnabled)
                {
                    DataDumpProcessor.Instance.Stop();
                    _isDataDumperRunning = false;
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> IsDataDumperEnabled()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _isDataDumperEnabled;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> IsDataDumperRunning()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _isDataDumperRunning;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<int> GetRefreshTimeInterval()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _timerRefreshInterval;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<TimeZoneAndTime> GetBaseTimeZoneAndBaseTimeZoneTime()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return DatabaseManager.GetBaseTimeZoneAndBaseTimeZoneTime();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SaveBaseTimeZoneAndBaseTimeZoneTime(string timeZone, DateTime dateTime)
        {
            try
            {
                DatabaseManager.GetInstance().SaveBaseTimeZoneAndBaseTimeZoneTime(timeZone, dateTime);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SaveClearanceTime(DataTable clearanceTable)
        {
            try
            {
                DatabaseManager.GetInstance().SaveClearanceTime(clearanceTable);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, DateTime>> GetDBClearanceTime()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return ServiceManager.GetInstance().GetDBClearanceTime();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task UpdateClearance(Dictionary<int, DateTime> dictionary)
        {
            try
            {
                ServiceManager.GetInstance().UpdateClearance(dictionary);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, MarketTimes>> GetMarketStartEndTime()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return WindsorContainerManager.GetMarketStartEndTime();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, DateTime>> FetchClearanceTime()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return WindsorContainerManager.FetchClearanceTime();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<Dictionary<int, BusinessObjects.TimeZone>> GetAllAUECTimeZones()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return CachedDataManager.GetInstance.GetAllAUECTimeZones().OrderBy(x => CachedDataManager.GetInstance.GetAUECText(x.Key)).ToDictionary(y => y.Key, y => y.Value);
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<string> GetAUECText(int auecID)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return CachedDataManager.GetInstance.GetAUECText(auecID);
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<BusinessObjects.TimeZone> GetAUECTimeZone(int auecID)
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return CachedDataManager.GetInstance.GetAUECTimeZone(auecID);
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
                    if (_pricingService2ClientHeartbeatManager != null)
                        _pricingService2ClientHeartbeatManager.Dispose();

                    if (_tradeServiceClientHeartbeatManager != null)
                        _tradeServiceClientHeartbeatManager.Dispose();

                    if (_expnlServiceHeartbeatManager != null)
                        _expnlServiceHeartbeatManager.Dispose();

                    if (ComplianceCacheManager.GetPreOrPostModuleEnabled() || HeatMapCacheManager.GetHeatMapModuleEnabled())
                    {
                        AmqpPluginManager.Connected -= AmqpServer_Connected;
                        AmqpPluginManager.Disconnected -= AmqpServer_Disconnected;
                    }

                    InformationReporter.GetInstance.InformationReceived -= new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                    if (ServiceManager.GetInstance().ClientBroadCastingManager != null)
                    {
                        ServiceManager.GetInstance().ClientBroadCastingManager.Connected -= new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Connected);
                        ServiceManager.GetInstance().ClientBroadCastingManager.Disconnected -= new ConnectionMessageReceivedDelegate(ClientBroadCastingManager_Disconnected);
                    }
                    ServiceManager.GetInstance().PreferencesUpdated -= new EventHandler<UserPreferencesEventArgs>(MainForm_PreferencesUpdated);
                    ServiceManager.GetInstance().UserInitiatedDataRefreshed -= new EventHandler(MainForm_UserInitiatedDataRefreshed);
                    ServiceManager.GetInstance().UserDataRefreshedRejected -= new EventHandler(MainForm_UserDataRefreshedRejected);
                    ServiceManager.GetInstance().UserDataRefreshCompleted -= new EventHandler(MainForm_UserDataRefreshCompleted);
                    ServiceManager.GetInstance().LogOnScreenToMain -= new EventHandler(MainForm_LogAUECDatesToMain);

                    _liveFeedConnectionStatus.LiveFeedConnectionStatusChanged -= new EventHandler<EventArgs<bool>>(LiveFeedConnectionStatusChanged);
                    _liveFeedConnectionStatus.Dispose();

                    if (_proxyPublishing != null)
                        _proxyPublishing.Dispose();
                    if (_container != null)
                        _container.Dispose();

                    ServerCustomCommunicationManager.GetInstance().Dispose();

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
