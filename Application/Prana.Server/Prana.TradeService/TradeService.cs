using Castle.Windsor;
using Prana.Allocation.Common.Helper;
using Prana.Authentication;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.DataManager;
using Prana.Fix.FixDictionary;
using Prana.FixEngineConnectionManager;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.MessageProcessor;
using Prana.OrderProcessor;
using Prana.PostTrade;
using Prana.PostTradeServices;
using Prana.PreTrade;
using Prana.PubSubService;
using Prana.PubSubService.Interfaces;
using Prana.QueueManager;
using Prana.SecurityMasterNew;
using Prana.SecurityMasterNew.BLL;
using Prana.ServerCommon;
using Prana.MonitoringProcessor;
using Prana.ServiceConnector;
using Prana.SocketCommunication;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Xml;
using Prana.ThirdPartyManager.Helper;
using Prana.PostTradeServices.RollOver;
using Prana.ThirdPartyManager.BusinessLogic;
using Prana.ThirdPartyManager.Helpers;

namespace Prana.TradeService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class TradeService : ITradeService, IDisposable
    {
        #region Variables
        private IWindsorContainer _container;
        private int _companyID = int.MinValue;
        private System.Threading.Timer _resendWaitTimer = null;
        private System.Threading.Timer _sendUnsentOrdersWaitTimer = null;
        private string _serverIdentifier;
        private int fixConnectionTimeToWait;
        private ITradeQueueProcessor _inFixMgrQueue;
        private ITradeQueueProcessor _outFixMgrQueue;
        private List<IQueueProcessor> _inQueueProcessorList = new List<IQueueProcessor>();
        private List<IQueueProcessor> _outQueueProcessorList = new List<IQueueProcessor>();
        private IPreTradeService _preTradeService;

        private bool _isPricingServiceConnected = false;

        private readonly object _publishLock = new object();
        private ProxyBase<IPublishing> _proxyPublishing = null;

        private ISecMasterServices _secMasterServices;
        private IActivityServices _activityServices;
        private IClosingServices _closingServices = null;

        private bool _isTradeServiceReadyForClose;

        private ServerHeartbeatManager _tradeServiceHeartbeatManager;

        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;
        private ClientHeartbeatManager<IExpnlService> _expnlServiceClientHeartbeatManager;
        #endregion

        #region Public Properties
        private IAllocationServices _allocationServices = null;
        public IAllocationServices AllocationServices
        {
            set { _allocationServices = value; }
        }

        private IAllocationManager _allocationManager = null;
        public IAllocationManager AllocationManager
        {
            set { _allocationManager = value; }
        }

        private ICashManagementService _cashManagementServices = null;
        public ICashManagementService CashManagementServices
        {
            set { _cashManagementServices = value; }
        }
        #endregion

        #region Constructor
        public TradeService()
        {
            ServicesHeartbeatSubscribersCollection.GetInstance().SubscribersUpdated += ServicesHeartbeatSubscribersCollection_SubscribersUpdated;
        }
        #endregion

        #region Private Methods
        private void User_Connected(object sender, EventArgs<ConnectionProperties> e)
        {
            try
            {
                ConnectionProperties connProperties = e.Value;
                ServicesHeartbeatSubscribersCollection.GetInstance().AddSubscriber(connProperties.IdentifierName, null);
                int companyUserId = int.MinValue;
                if (int.TryParse(connProperties.IdentifierID, out companyUserId))
                {
                    AuthenticateUser.GetInstance().AddLoggedInUser(companyUserId);
                }

                Dictionary<int, FixPartyDetails> allPartyDetails = FixEngineConnectionPoolManager.GetInstance().GetAllFixConnections();
                foreach (KeyValuePair<int, FixPartyDetails> partyDetails in allPartyDetails)
                {
                    int status;
                    if (partyDetails.Value.BuySideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED && partyDetails.Value.BuyToSellSideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        status = (int)PranaInternalConstants.ConnectionStatus.CONNECTED;
                    }
                    else
                    {
                        status = (int)PranaInternalConstants.ConnectionStatus.DISCONNECTED;
                    }

                    string statusReport = AdminMessageHandler.CreateCounterPartyStatusReport(partyDetails.Value.ConnectionID, partyDetails.Value.PartyID, partyDetails.Value.PartyName, status, partyDetails.Value.HostName, partyDetails.Value.Port, partyDetails.Value.OriginatorType, partyDetails.Value.BrokerConnectionType).ToString();
                    QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_COUNTERPARTY_CONNECTIONSTATUS_REPORT, statusReport);

                    ServerCustomCommunicationManager.GetInstance().SendMsgToUser(qMsg, connProperties.IdentifierID);
                }

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
                int companyUserId = int.MinValue;
                if (int.TryParse(connProperties.IdentifierID, out companyUserId))
                {
                    if (AuthenticateUser._dictLoggedInUser.ContainsKey(companyUserId) && AuthenticateUser._dictLoggedInUser[companyUserId].AuthenticationType == AuthenticationTypes.EnterpriseLoggedIn)
                        AuthenticateUser.GetInstance().RemoveLoggedInUser(companyUserId);
                }

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

        private void ServicesHeartbeatSubscribersCollection_SubscribersUpdated(object sender, EventArgs e)
        {
            try
            {
                PublishPreparation(Topics.Topic_TradeServiceConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void StartCommunicationManager()
        {
            try
            {
                List<string> tradingAccounts = Prana.CommonDataCache.WindsorContainerManager.GetAllTradingAccounts();
                ServerCustomCommunicationManager.GetInstance().Initialise(_inQueueProcessorList, _outQueueProcessorList, tradingAccounts);
                ServerCustomCommunicationManager.GetInstance().Connected += new ConnectionMessageReceivedDelegate(User_Connected);
                ServerCustomCommunicationManager.GetInstance().Disconnected += new ConnectionMessageReceivedDelegate(User_Disconnected);
                ServerCustomCommunicationManager.GetInstance().UserExceptionDelegate += ClearAccountUserCache;
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

        private void StartCentralSMCommunicationManager(string serverIdentifier)
        {
            try
            {
                CentralSMCommunicationManager.Instance.Initialize(serverIdentifier);
                CentralSMCommunicationManager.Instance.CentralSMConnected += Instance_CentralSMConnected;
                CentralSMCommunicationManager.Instance.CentralSMDisconnected += Instance_CentralSMDisconnected;
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
        /// clears the Account Locked By User
        /// </summary>
        /// <param name="userID"></param>
        private void ClearAccountUserCache(int userID)
        {
            try
            {
                Dictionary<int, int> accountUserLockDetail = CachedDataManager.GetInstance.GetAccountUserLockDetail();
                //newaccountUserLockDetail dictionary used because foreach cannot  while changing the structure
                Dictionary<int, int> newaccountUserLockDetail = new Dictionary<int, int>();
                //Release all CashAccounts
                foreach (KeyValuePair<int, int> kvp in accountUserLockDetail)
                {
                    //add only the locked accounts
                    if (kvp.Value != userID)
                    {
                        newaccountUserLockDetail.Add(kvp.Key, kvp.Value);
                    }
                }
                CachedDataManager.GetInstance.SetAccountUserLockDetail(newaccountUserLockDetail);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return;
        }

        private void Instance_CentralSMConnected(object sender, EventArgs<string> e)
        {
            try
            {
                ServicesHeartbeatSubscribersCollection.GetInstance().AddSubscriber(e.Value, null);

                SecMasterServerComponent.GetInstance.CentralSMConnected(e.Value);
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

        private void Instance_CentralSMDisconnected(object sender, EventArgs<string> e)
        {
            try
            {
                ServicesHeartbeatSubscribersCollection.GetInstance().RemoveSubscriber(e.Value);

                _secMasterServices.CentralSMDisconnected();
                SecMasterServerComponent.GetInstance.CentralSMDisconnected(e.Value);
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

        private void Server_FixConnectionStatusUpdate(object sender, EventArgs<FixPartyDetails> e)
        {
            try
            {
                PublishPreparation(Topics.Topic_TradeServiceFixConnectionData, e.Value);

                int status;
                if (e.Value.BuySideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED && e.Value.BuyToSellSideStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    status = (int)PranaInternalConstants.ConnectionStatus.CONNECTED;
                }
                else
                {
                    status = (int)PranaInternalConstants.ConnectionStatus.DISCONNECTED;

                    ReconnectParty(e.Value.ConnectionID);
                }

                QueueMessage qMsg = new QueueMessage(AdminMessageHandler.CreateCounterPartyStatusReport(e.Value.ConnectionID, e.Value.PartyID, e.Value.PartyName, status, e.Value.HostName, e.Value.Port, e.Value.OriginatorType, e.Value.BrokerConnectionType));
                ServerCustomCommunicationManager.GetInstance().SendMsgToAllUsers(qMsg);
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

        private void ReconnectParty(int connectionID)
        {
            try
            {
                if (FixEngineConnectionPoolManager.GetInstance().AutoReconnect && fixConnectionTimeToWait != 0)
                {
                    BackgroundWorker bgWorkerReconnect = new BackgroundWorker();
                    bgWorkerReconnect.DoWork += new DoWorkEventHandler(bgWorkerReconnect_DoWork);
                    int timeToWait = fixConnectionTimeToWait * 1000;
                    bgWorkerReconnect.RunWorkerAsync(new object[] { connectionID, timeToWait });
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

        private void bgWorkerReconnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;

                int connectionID = (int)data[0];
                int timeToWait = (int)data[1];

                System.Threading.Thread.Sleep(timeToWait);

                FixEngineConnectionPoolManager.GetInstance().ConnectBuySide(FixEngineConnectionPoolManager.GetInstance().GetPortForParty(connectionID));
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

        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                string[] messageList = message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                PublishPreparation(Topics.Topic_TradeServiceLogsData, messageList.ToList());

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

        private Dictionary<int, FixPartyDetails> SetAllCounterPartiesDetails(string path)
        {
            Dictionary<int, FixPartyDetails> dictFixPartyDetails = new Dictionary<int, FixPartyDetails>();
            try
            {
                XmlDocument fixdetailsXML = new XmlDocument();

                fixdetailsXML.Load(path);
                string hostName = fixdetailsXML.SelectSingleNode("AppSettings/CounterPartyConnectionDetails").Attributes["host"].Value;

                if (hostName.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    hostName = ServerCustomCommunicationManager.GetInstance().EndPoint.Address.ToString();
                }

                XmlNodeList xmlNodes = fixdetailsXML.SelectNodes("AppSettings/CounterPartyConnectionDetails/CounterParty");
                foreach (XmlNode xmlnode in xmlNodes)
                {
                    string targetSubID = string.Empty;
                    if (xmlnode.Attributes["TargetSubID"] != null)
                    {
                        targetSubID = xmlnode.Attributes["TargetSubID"].Value;
                    }
                    int originatorType = (int)PranaServerConstants.OriginatorType.BuySide;
                    if (xmlnode.Attributes["OriginatorType"] != null)
                    {
                        originatorType = int.Parse(xmlnode.Attributes["OriginatorType"].Value);
                    }
                    int brokerConnectionType = (int)PranaServerConstants.BrokerConnectionType.None;
                    if (xmlnode.Attributes["BrokerConnectionType"] != null)
                    {
                        brokerConnectionType = int.Parse(xmlnode.Attributes["BrokerConnectionType"].Value);
                    }
                    string fixDllAdapterName = string.Empty;
                    if (xmlnode.Attributes["FIXDllName"] != null)
                    {
                        fixDllAdapterName = xmlnode.Attributes["FIXDllName"].Value;
                    }
                    string partyName = string.Empty;
                    if (xmlnode.Attributes["CounterPartyName"] != null)
                    {
                        partyName = xmlnode.Attributes["CounterPartyName"].Value;
                    }
                    DateTime resetTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " " + xmlnode.Attributes["ResetTime"].Value, "yyyy-MM-dd hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    if (resetTime > DateTime.UtcNow)
                        resetTime = resetTime.AddDays(-1);

                    FixPartyDetails fixPartyDetails = new FixPartyDetails(int.Parse(xmlnode.Attributes["CounterPartyID"].Value), partyName, xmlnode.Attributes["SenderCompID"].Value, xmlnode.Attributes["TargetCompID"].Value, int.Parse(xmlnode.Attributes["Port"].Value), targetSubID, hostName, originatorType, brokerConnectionType, fixDllAdapterName, resetTime);

                    int symbology = 0;
                    if (xmlnode.Attributes["Symbology"] != null)
                    {
                        int.TryParse(xmlnode.Attributes["Symbology"].Value, out symbology);
                        fixPartyDetails.Symbology = symbology;
                    }

                    dictFixPartyDetails.Add(fixPartyDetails.ConnectionID, fixPartyDetails);
                }

                foreach (KeyValuePair<int, FixPartyDetails> party in dictFixPartyDetails)
                {
                    Int64 lastmsgSeqNumberReceived = ServerDataManager.GetCounterPartyLastMsgSeqNumber(party.Value.TargetCompID, party.Value.ResetTime);
                    if (lastmsgSeqNumberReceived != Int64.MinValue)
                    {
                        party.Value.LastSeqNumberRecevied = lastmsgSeqNumberReceived;
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
            return dictFixPartyDetails;
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

        private void Server_SendInTradeData(object sender, EventArgs e)
        {
            try
            {
                _preTradeService.SendInTradeToEsper(ServerDbManager.GetBlotterLaunchData(true), true);
                _preTradeService.MakeProxy();
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

        private void PricingService2ClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                _isPricingServiceConnected = true;
                PublishPreparation(Topics.Topic_TradeServicePricingConnectionData, _isPricingServiceConnected);

                //Narendra Kumar Jangir
                //Jan 27 2014
                //Whenever mark price or fx rate is changed,need to publish data from pricing so that revaluation can be done from that date
                //pricing proxy is connected after pricingservice2 starts
                _cashManagementServices.CreateSubscriptionServicesProxyPricing(_isPricingServiceConnected);
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
                _isPricingServiceConnected = false;
                PublishPreparation(Topics.Topic_TradeServicePricingConnectionData, _isPricingServiceConnected);

                //Narendra Kumar Jangir
                //Jan 27 2014
                //Whenever mark price or fx rate is changed,need to publish data from pricing so that revaluation can be done from that date
                //pricing proxy is disconnected after pricingservice2 stops
                _cashManagementServices.CreateSubscriptionServicesProxyPricing(_isPricingServiceConnected);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ExpnlServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                List<string> message = new List<string>();
                message.Add(DateTime.Now.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));

                PublishPreparation(Topics.Topic_TradeServiceLogsData, message);
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

        private void ExpnlServiceClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                ExpnlServiceConnector.GetInstance().IsExpnlServiceConnected = String.IsNullOrEmpty(ExpnlServiceConnector.GetInstance().TryGetChannel());
                if (ExpnlServiceConnector.GetInstance().IsExpnlServiceConnected && CachedDataManager.GetInstance.GetIsMarketDataPermissionEnabledForTradingRules())
                {
                    Server_SendInTradeData(null, null);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ExpnlServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                ExpnlServiceConnector.GetInstance().IsExpnlServiceConnected = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void PricingService2ClientHeartbeatManager_AnotherInstanceSubscribedEvent(object sender, EventArgs<string, string> e)
        {
            try
            {
                List<string> message = new List<string>();
                message.Add(DateTime.Now.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));

                PublishPreparation(Topics.Topic_TradeServiceLogsData, message);
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

        private void HostServicesSnapShotPositionManagement()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradePositionServiceEndpointAddress"))
                {
                    IPranaPositionServices pranaPositionServices = _container.Resolve<IPranaPositionServices>();
                    PranaServiceHost.HostPranaService(pranaPositionServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostServicesCashManagement()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeCashServiceEndpointAddress"))
                {
                    ICashManagementService cashManagementService = _container.Resolve<ICashManagementService>();
                    PranaServiceHost.HostPranaService(cashManagementService);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostServicesActivity()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeActivityServiceEndpointAddress"))
                {
                    _activityServices = _container.Resolve<IActivityServices>();
                    PranaServiceHost.HostPranaService(_activityServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostAllocationServices()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeAllocationServiceEndpointAddress"))
                {
                    IAllocationServices allocationServices = _container.Resolve<IAllocationServices>();
                    PranaServiceHost.HostPranaService(allocationServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// This method is to host third party service
        /// </summary>
        private void HostThirdPartyService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeThirdPartyServiceEndpointAddress"))
                {
                    IThirdPartyService thirdPartyService = _container.Resolve<IThirdPartyService>();
                    PranaServiceHost.HostPranaService(thirdPartyService);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostAllocationManager()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeAllocationServiceNewEndpointAddress"))
                {
                    IAllocationManager allocationManager = _container.Resolve<IAllocationManager>();
                    PranaServiceHost.HostPranaService(allocationManager);
                    allocationManager.Initialize();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostCAServices()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeCAServiceEndpointAddress"))
                {
                    ICAServices caServices = _container.Resolve<ICAServices>();
                    PranaServiceHost.HostPranaService(caServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostClosingServices()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeClosingServiceEndpointAddress"))
                {
                    _closingServices = _container.Resolve<IClosingServices>();
                    PranaServiceHost.HostPranaService(_closingServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostAuditServices()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeAuditTrailServiceEndpointAddress"))
                {
                    IAuditTrailService auditServices = _container.Resolve<IAuditTrailService>();
                    PranaServiceHost.HostPranaService(auditServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostSecMasterSyncServices()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeSecMasterSyncServiceEndpointAddress"))
                {
                    ISecMasterSyncServices secMasterSyncServices = _container.Resolve<ISecMasterSyncServices>();
                    PranaServiceHost.HostPranaService(secMasterSyncServices);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostCompliancePreTradeService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradePreTradeComplianceServiceEndpointAddress"))
                {
                    Prana.Interfaces.IPreTradeService preTradeService = _container.Resolve<Prana.Interfaces.IPreTradeService>();
                    PranaServiceHost.HostPranaService(preTradeService);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostRebalancerBLService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeRebalancerBLServiceEndpointAddress"))
                {
                    IRebalancerBLService rebalancerBLService = _container.Resolve<IRebalancerBLService>();
                    PranaServiceHost.HostPranaService(rebalancerBLService);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void HostSecMasterOTCService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeOTCServiceEndpointAddress"))
                {
                    ISecMasterOTCService secMasterOTCService = _container.Resolve<ISecMasterOTCService>();
                    PranaServiceHost.HostPranaService(secMasterOTCService);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Hosts the AuthenticateUserService.
        /// </summary>
        private void HostAuthenticateUserService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted(EndPointAddressConstants.CONST_TradeAuthenticateUserServiceEndpoint))
                {
                    IAuthenticateUser authenticateLoginUserService = _container.Resolve<IAuthenticateUser>();
                    PranaServiceHost.HostPranaService(authenticateLoginUserService);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Hosts the ClientConnectivityService.
        /// </summary>
        private void HostClientConnectivityService()
        {
            try
            {
                if (!PranaServiceHost.IsServiceHosted("TradeClientConnectivityServiceEndpointAddress"))
                {
                    IClientConnectivityService clientConnectivityService = _container.Resolve<IClientConnectivityService>();
                    PranaServiceHost.HostPranaService(clientConnectivityService);
                }
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
                _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
                            if (_proxyPublishing != null)
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

        private void resendWaitTimer_Tick(object state)
        {
            try
            {
                // Request CounterParties to get all lost messages
                _resendWaitTimer.Change(Timeout.Infinite, Timeout.Infinite);
                Logger.HandleException(new Exception("Before Sending to Resend Request"), LoggingConstants.POLICY_LOGANDSHOW);

                FixEngineConnectionPoolManager.GetInstance().ResendRequest();
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

        private void sendUnsentOrdersWaitTimer_Tick(object state)
        {
            try
            {
                _sendUnsentOrdersWaitTimer.Change(Timeout.Infinite, Timeout.Infinite);
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

        private List<PranaMessage> GetClientReceivedTrades()
        {
            try
            {
                List<PranaMessage> list = new List<PranaMessage>();
                string path = ConfigurationManager.AppSettings[PranaServerConstants.CLIENT_RECEIVED_PATH].ToString() + "_" + _companyID.ToString();
                MSMQQueueManager queue = new MSMQQueueManager(path);
                List<string> messages = queue.GetAllMessages();

                foreach (string msg in messages)
                {
                    PranaMessage pranaMsg = new PranaMessage(msg);
                    list.Add(pranaMsg);
                }
                return list;
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

                return null;
            }
        }

        private List<PranaMessage> GetCPReceivedTrades()
        {
            try
            {
                List<PranaMessage> list = new List<PranaMessage>();
                string path = ConfigurationManager.AppSettings[PranaServerConstants.CP_RECEIVED_MSGS_PATH].ToString() + "_" + _companyID.ToString(); ;
                MSMQQueueManager queue = new MSMQQueueManager(path);
                List<string> messages = queue.GetAllMessages();
                foreach (string msg in messages)
                {
                    PranaMessage pranaMsg = FixEngineConnectionPoolManager.GetInstance().CreatePranaMessageFromFixMessage(msg);
                    list.Add(pranaMsg);
                }
                return list;
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

                return null;
            }
        }

        private List<PranaMessage> GetCpSentTrades()
        {
            try
            {
                List<PranaMessage> list = new List<PranaMessage>();
                string path = ConfigurationManager.AppSettings[PranaServerConstants.CP_SENT_MSGS_PATH].ToString() + "_" + _companyID.ToString(); ;
                MSMQQueueManager queue = new MSMQQueueManager(path);
                List<string> messages = queue.GetAllMessages();

                foreach (string msg in messages)
                {
                    PranaMessage pranaMsg = FixEngineConnectionPoolManager.GetInstance().CreatePranaMessageFromFixMessage(msg);
                    list.Add(pranaMsg);
                }
                return list;
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

                return null;
            }
        }

        private void DeleteFromQueue(string path)
        {
            try
            {
                MSMQQueueManager queue = new MSMQQueueManager(path);
                queue.Purge();
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

        private void SaveMessages(List<PranaMessage> msgs)
        {
            try
            {
                string path = ConfigurationManager.AppSettings[PranaServerConstants.OLDTRADES_QUEUE_PATH].ToString() + "_" + _companyID.ToString();
                IQueueProcessor queue = new MSMQQueueManager(path);
                foreach (PranaMessage msg in msgs)
                {
                    QueueMessage qPersistedMsg = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", msg);
                    queue.SendMessage(qPersistedMsg);
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

        private void RefreshClosingServices()
        {
            try
            {
                _closingServices.RefreshClosingData();
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Closing data cache has been refreshed!");
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

        private void CreateClosingServicesObject()
        {
            try
            {
                _closingServices = new ClosingManager();
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Closing data cache has been refreshed!");
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

        private void ReprocessFIXStuckTrades()
        {
            try
            {
                //getting stuck trade from cahce based on wait time
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Started reprocessing all stuck trades ");
                IProcessingUnit processor = MessageProcessor.MessageEngine.GetProcessor("DropCopy_PostTrade");
                if (processor != null)
                {
                    Dictionary<string, List<PranaMessage>> msgs = processor.GetAndClearPranaMessages();

                    // for each security send list of trades
                    foreach (List<PranaMessage> pranaMsgList in msgs.Values)
                    {
                        FixEngineConnectionPoolManager.GetInstance().ReProcessMsg(pranaMsgList);
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("DropCopy_PostTrade Processor not found.");
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

        private void Server_GroupsSaved(object sender, EventArgs e)
        {
            _isTradeServiceReadyForClose = true;
        }

        /// <summary>
        /// Sends message to client to Refresh the Blotter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshBlotterAfterImport(object sender, EventArgs e)
        {
            try
            {
                MessageData messageData = new MessageData();
                messageData.EventData = new List<string>();
                messageData.TopicName = Topics.Topic_RefreshBlotterAfterImport;
                CentralizePublish(messageData);
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
        /// Sends message to client to set ImportStarted as true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportStarted(object sender, EventArgs e)
        {
            try
            {
                MessageData messageData = new MessageData();
                messageData.EventData = new List<string>();
                messageData.TopicName = Topics.Topic_ImportStarted;
                CentralizePublish(messageData);
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

                _preTradeService = new PreTradeService();

                #region Client Heartbeat Setup
                _pricingService2ClientHeartbeatManager = new ClientHeartbeatManager<IPricingService2>("PricingService2EndpointAddress");
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

                _inFixMgrQueue = new TradeQueueManager(this);
                _outFixMgrQueue = new TradeQueueManager(this);

                MessageEngine.GetInstance();

                _companyID = Convert.ToInt32(CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"]);

                CachedDataManager.GetInstance.WaitTimeToGetStuckTrade = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("StuckTradeWaitTime").ToString());
                CachedDataManager.GetInstance.EmailIntervalForStuckTrades = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("StuckTradeEmailInterval").ToString());

                CreatePublishingProxy();
                PranaPubSubService.Initialize();

                if (Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("DeleteObsoleteAllocationPreference")))
                {
                    ServerDbManager.DeleteObsoleteAllocationPreference();
                }

                _secMasterServices = _container.Resolve<ISecMasterServices>();

                HostServicesSnapShotPositionManagement();
                HostServicesCashManagement();
                HostServicesActivity();
                HostAllocationServices();
                HostAllocationManager();
                HostCAServices();
                HostClosingServices();
                HostAuditServices();
                HostSecMasterSyncServices();
                HostCompliancePreTradeService();
                HostRebalancerBLService();
                HostSecMasterOTCService();
                HostAuthenticateUserService();
                HostClientConnectivityService();
                HostThirdPartyService();

                IQueueProcessor inTradeComMgrQueue = new QueueProcessor(HandlerType.TradeHandler);
                IQueueProcessor inSecMasterComMgrQueue = new QueueProcessor(HandlerType.SecurityMasterHandler);
                IQueueProcessor inPostTraderServicesQueue = new QueueProcessor(HandlerType.PostTradeServicesHandler);

                IQueueProcessor outTradeComMgrQueue = new QueueProcessor(HandlerType.TradeHandler);
                IQueueProcessor outSecMasterComMgrQueue = new QueueProcessor(HandlerType.SecurityMasterHandler);
                IQueueProcessor outPostTraderServicesQueue = new QueueProcessor(HandlerType.PostTradeServicesHandler);
                IQueueProcessor outPostTradeQueue = new QueueProcessor(HandlerType.PostTradeHandler);
                IQueueProcessor outMonitoringQueue = new QueueProcessor(HandlerType.MonitoringServices);

                _inQueueProcessorList.Add(inTradeComMgrQueue);
                _inQueueProcessorList.Add(inSecMasterComMgrQueue);
                _inQueueProcessorList.Add(inPostTraderServicesQueue);

                _outQueueProcessorList.Add(outTradeComMgrQueue);
                _outQueueProcessorList.Add(outSecMasterComMgrQueue);
                _outQueueProcessorList.Add(outPostTraderServicesQueue);
                _outQueueProcessorList.Add(outPostTradeQueue);
                _outQueueProcessorList.Add(outMonitoringQueue);

                string dbQueuePath = ConfigurationManager.AppSettings[PranaServerConstants.DBQUEUE_PATH].ToString() + "_" + _companyID.ToString();
                string errorQueuePath = ConfigurationManager.AppSettings[PranaServerConstants.ERRORQUEUE_PATH].ToString() + "_" + _companyID.ToString();
                long lastSeqNumberfromDB = ServerDataManager.GetMaxSeqNumber();

                IQueueProcessor dbQueue = new MSMQQueueManager(dbQueuePath);
                long lastSeqNumberfromMsgStore = dbQueue.getLastSeqNumber();

                if (lastSeqNumberfromMsgStore > lastSeqNumberfromDB)
                {
                    lastSeqNumberfromDB = lastSeqNumberfromMsgStore;
                }

                UniqueIDGenerator.SetMaxGeneratedIDFromDB(ServerDataManager.GetMaxGeneratedIDFromDBForOrders());
                UniqueIDGenerator.Initlise(lastSeqNumberfromDB);
                uIDGenerator.SetMaxGeneratedIDFromDB(ServerDataManager.GetMaxGeneratedIDFromDB());

                AllocationIDGenerator.SetMaxGeneratedIDFromDB(ServerDataManager.GetMaxGeneratedIDFromDBForGroup());

                long symbolPK = ServerDataManager.GetMaxSymbolPKIDFromDB();
                SecurityMasterSymbolIDGenerator.SetMaxGeneratedIDFromDB(symbolPK);
                IQueueProcessor errorQueue = new MSMQQueueManager(errorQueuePath);

                DbQueueManager.GetInstance().Initlise(dbQueue, _allocationServices);
                StartCommunicationManager();
                MessageEngine.GetInstance().Initilise(inTradeComMgrQueue, outTradeComMgrQueue, _inFixMgrQueue, _outFixMgrQueue, dbQueue, errorQueue, _allocationServices, _secMasterServices, _allocationManager);

                SecMasterServerComponent.GetInstance.Initilise(inSecMasterComMgrQueue, outSecMasterComMgrQueue, _secMasterServices);
                Dictionary<int, FixPartyDetails> dictFixPartyDetails = SetAllCounterPartiesDetails(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\AppSettings.xml");

                fixConnectionTimeToWait = int.Parse(ConfigurationManager.AppSettings["FixConnectionTimeToWait"]);

                int CompanyID = Convert.ToInt32(CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"]);
                FixEngineConnectionPoolManager.GetInstance().Initilise(_inFixMgrQueue, _outFixMgrQueue, CompanyID);

                PranaTaxLotCacheManager.GetInstance.Initlise(outPostTradeQueue, dbQueue, _allocationServices);

                PranaMonitoringProcessor.GetInstance.Initlise(outMonitoringQueue);
                PostTradeServicesServer.GetInstance.Initilise(inPostTraderServicesQueue, outPostTraderServicesQueue, _secMasterServices);

                bool isTroubleshootModeStart = false;
                bool.TryParse(ConfigurationManager.AppSettings["IsTroubleshootModeStart"], out isTroubleshootModeStart);

                FixEngineConnectionPoolManager.GetInstance().ConnectionStatusUpdate += new EventHandler<EventArgs<FixPartyDetails>>(Server_FixConnectionStatusUpdate);
                FixEngineConnectionPoolManager.GetInstance().ConnectAllBuySides(dictFixPartyDetails, isTroubleshootModeStart);

                SecMasterSyncCacheManager.GetInstance().Initlise(_secMasterServices);
                SecMasterRequestObj secMasterReqObj = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAlltradedSymbols();
                _secMasterServices.GetSecMasterDataForListSync(secMasterReqObj, 0);

                _activityServices.Initialize();
                ThirdPartyFixManager.Instance.WireEvents();

                DbQueueManager.GetInstance().RefreshBlotterAfterImport += new EventHandler(RefreshBlotterAfterImport);
                DbQueueManager.GetInstance().ImportStarted += new EventHandler(ImportStarted);

                OrderCacheManager.FillMultiDayOrderAllocationCache();
                OrderCacheManager.FillMultiDayOrderReplacedClOrderIdCache();

                OrderCacheManager.FillMultiBrokerTradeMappingCache();

                #region Compliance section
                try
                {
                    if (CommonDataCache.ComplianceCacheManager.GetPreOrPostModuleEnabled() || CommonDataCache.Cache_Classes.HeatMapCacheManager.GetHeatMapModuleEnabled())
                    {
                        AmqpPlugin.AmqpPluginManager.GetInstance().Initialise(this._secMasterServices);
                        AmqpPlugin.AmqpPluginManager.Connected += AmqpServer_Connected;
                        AmqpPlugin.AmqpPluginManager.Disconnected += AmqpServer_Disconnected;
                        AmqpPlugin.AmqpPluginManager.GetInstance().SendInTradeData += Server_SendInTradeData;
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

                SecMasterCommonCache.Instance.FillSecMasterCommonCache();
                PranaPricingSource pricingSource = PranaPricingSource.Esignal;
                string histPricing = CachedDataManager.GetInstance.GetPranaPreferenceByKey("PricingSource");
                Enum.TryParse<PranaPricingSource>(histPricing, true, out pricingSource);

                ManualOrderSendHelper.Instance().Initilise(_secMasterServices, CompanyID);

                if (pricingSource == PranaPricingSource.Bloomberg)
                {
                    string hostName = Dns.GetHostName();
                    IEnumerable<IPAddress> ipAddresses = Dns.GetHostEntry(hostName).AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    _serverIdentifier = "TradeServer" + ipAddresses.Last().ToString() + ":" + ConfigurationHelper.Instance.GetAppSettingValueByKey("OrderRequestPort");
                    StartCentralSMCommunicationManager(_serverIdentifier);
                }
                if (isTroubleshootModeStart)
                {
                    _resendWaitTimer = new System.Threading.Timer(new TimerCallback(resendWaitTimer_Tick), null, 0, 15000);
                    _sendUnsentOrdersWaitTimer = new System.Threading.Timer(new TimerCallback(sendUnsentOrdersWaitTimer_Tick), null, 0, 20000);
                }
                GtcGtdEmailNotificationManager.GetInstance().ScheduleEmailNotification();
                ThirdPartyEmailHelper.SendEmailOnScheduledTime();
                ThirdPartyTimedBatchHelper.SetTimedBatchesScheduler();
                ThirdPartyLogic.PublishThirdPartyAutomatedBatchStatus();
                ClearanceManager.GetInstance.AddClearanceSchedulerTasks(_companyID);

                #region Server Heartbeat Setup
                _tradeServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("TradeService started at:- " + DateTime.Now + " (local time)", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

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

            Console.WriteLine("Shutting down service.");
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("TradeService successfully closed at:- " + DateTime.Now + " (local time)", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
                else if (_tradeServiceHeartbeatManager != null && !isRetryRequest)
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
                PublishPreparation(Topics.Topic_TradeServiceConnectedUserData, ServicesHeartbeatSubscribersCollection.GetInstance().GetSubscribersNames());
                PublishPreparation(Topics.Topic_TradeServicePricingConnectionData, _isPricingServiceConnected);
                PublishPreparation(Topics.Topic_TradeServiceFixAutoConnectionStatus, FixEngineConnectionPoolManager.GetInstance().AutoReconnect);

                Dictionary<int, FixPartyDetails> fixConnDetails = FixEngineConnectionPoolManager.GetInstance().GetAllFixConnections();
                foreach (KeyValuePair<int, FixPartyDetails> fixdetails in fixConnDetails)
                {
                    PublishPreparation(Topics.Topic_TradeServiceFixConnectionData, fixdetails.Value);
                }

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

                DbQueueManager.GetInstance().GroupsSaved += new EventHandler(Server_GroupsSaved);
                FixEngineConnectionPoolManager.GetInstance().DisconnectAllBuySides();
                DbQueueManager.GetInstance().StopTimerAndUnwireEvent();

                try
                {
                    if (ComplianceCacheManager.GetPreOrPostModuleEnabled())
                        AmqpPlugin.AmqpPluginManager.GetInstance().Close();
                }
                catch (Exception ex)
                {
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                }

                PranaPricingSource pricingSource = PranaPricingSource.Esignal;
                string histPricing = CachedDataManager.GetInstance.GetPranaPreferenceByKey("PricingSource");
                Enum.TryParse<PranaPricingSource>(histPricing, true, out pricingSource);

                if (pricingSource == PranaPricingSource.Bloomberg)
                {
                    Instance_CentralSMDisconnected(this, new EventArgs<string>("CentralSM"));
                }

                ServerCustomCommunicationManager.GetInstance().DisConnectAllClients();

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
                            var proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("TradePublishing", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradePublishing", false));
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
                            var proxy = new ProxyBase<IAllocationServices>("TradeAllocationServiceEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("TradeAllocationService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradeAllocationService", false));
                        }
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new ProxyBase<IAuditTrailService>("TradeAuditTrailServiceEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("TradeAuditTrailService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("TradeAuditTrailService", false));
                        }
                    }),
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            var proxy = new ProxyBase<IExpnlCalculationService>("ExpnlCalculationServiceEndpointAddress");
                            hostedServicesStatus.Add(new HostedService("ExpnlCalculationService", await proxy.InnerChannel.HealthCheck()));
                        }
                        catch
                        {
                            hostedServicesStatus.Add(new HostedService("ExpnlCalculationService", false));
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
                Prana.ServerCommon.UserSettingConstants.IsDebugModeEnabled = isDebugModeEnabled;

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

                return Prana.ServerCommon.UserSettingConstants.IsDebugModeEnabled;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }
        #endregion

        #region ITradeService Methods
        public async System.Threading.Tasks.Task ReloadRules()
        {
            try
            {
                Prana.Fix.FixDictionary.FixDictionaryHelper.LoadFixDictionary();
                Prana.CustomMapper.PranaCustomMapper.LoadDictionary();

                //Processing All stuck trades after reload the rules
                ReprocessFIXStuckTrades();

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task ReloadXslt()
        {
            try
            {
                string xsltPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\xslts\SymbolTransformer.xslt";
                XslTransformer.GetInstance(xsltPath).LoadXslt(xsltPath);

                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("SymbolTransformer xslt successfully reloaded from path : " + xsltPath);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> IsTradeServiceReadyForClose()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return _isTradeServiceReadyForClose;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task GetMessageStatus(Order order)
        {
            try
            {
                FixEngineConnectionPoolManager.GetInstance().GetMessageStatus(order);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task MoveOldTrade()
        {
            try
            {
                List<PranaMessage> listcprec = GetCPReceivedTrades();
                List<PranaMessage> listcpsent = GetCpSentTrades();
                List<PranaMessage> listclient = GetClientReceivedTrades();
                SaveMessages(listcprec);
                DeleteFromQueue(ConfigurationManager.AppSettings[PranaServerConstants.CP_RECEIVED_MSGS_PATH].ToString() + "_" + _companyID.ToString());
                SaveMessages(listcpsent);
                DeleteFromQueue(ConfigurationManager.AppSettings[PranaServerConstants.CP_SENT_MSGS_PATH].ToString() + "_" + _companyID.ToString());
                SaveMessages(listclient);
                DeleteFromQueue(ConfigurationManager.AppSettings[PranaServerConstants.CLIENT_RECEIVED_PATH].ToString() + "_" + _companyID.ToString());

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task RefreshCacheClosing()
        {
            try
            {
                if (_closingServices != null)
                {
                    RefreshClosingServices();
                }
                else if (_closingServices == null)
                {
                    CreateClosingServicesObject();
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// This method refreshes the allocation preference cache
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        public async System.Threading.Tasks.Task RefreshPreferenceCache()
        { 
            try
            {
                Logger.LoggerWrite("Refreshing Allocation Preference Data", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                _allocationManager.RefreshAllocationPreferenceCache();
                Logger.LoggerWrite("Refreshing Allocation Preference Data - Done", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information);
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Allocation preference data cache has been refreshed!");

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// Sends the manual drops on fix.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FaultException{PranaAppException}"></exception>
        /// <exception cref="PranaAppException"></exception>
        public async System.Threading.Tasks.Task SendManualDropsOnFix()
        {
            try
            {
                ManualOrderSendHelper.Instance().ProcessAllAuecManualOrders(_companyID);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task ClearFixTradeOrderCache()
        {
            try
            {
                Prana.DropCopyProcessor_PostTrade.DropCopyCacheManager_PostTrade.ClearOrderCache();

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task ClearFixTradeOrder(string orderID)
        {
            try
            {
                Prana.DropCopyProcessor_PostTrade.DropCopyCacheManager_PostTrade.ClearOrder(orderID);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<List<string>> FetchProcessorNames()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return Prana.MessageProcessor.MessageEngine.Names;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<OrderCollection> ShowMessagesForProcessor(string processorName)
        {
            OrderCollection orderCollection = new OrderCollection();
            try
            {
                IProcessingUnit processor = MessageProcessor.MessageEngine.GetProcessor(processorName);
                if (processor != null)
                {
                    List<PranaMessage> msgs = processor.GetAllCachedMessages();
                    foreach (PranaMessage msg in msgs)
                    {
                        Order order = Transformer.CreateOrder(msg);
                        NameValueFiller.FillNameDetailsOfOrder(order);
                        orderCollection.Add(order);
                    }

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("No. of cached messages for " + processorName + " :" + msgs.Count);
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(processorName + " Processor not found.");
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return orderCollection;
        }

        public async System.Threading.Tasks.Task<OrderCollection> ShowErrorMessagesForProcessor(string processorName)
        {
            OrderCollection orderCollection = new OrderCollection();
            try
            {
                IProcessingUnit processor = MessageProcessor.MessageEngine.GetProcessor(processorName);
                if (processor != null)
                {
                    List<PranaMessage> msgs = processor.GetCachedErrorOrders();
                    foreach (PranaMessage msg in msgs)
                    {
                        Order order = Transformer.CreateOrder(msg);
                        NameValueFiller.FillNameDetailsOfOrder(order);
                        orderCollection.Add(order);
                    }

                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("No. of error messages for " + processorName + " :" + msgs.Count);
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(processorName + " Processor not found.");
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return orderCollection;
        }

        public async System.Threading.Tasks.Task<List<string>> PersistedMessagesReceivedFromClient()
        {
            List<string> result = new List<string>();
            try
            {
                List<PranaMessage> list = GetClientReceivedTrades();
                List<string> columnnames = Transformer.GetColumnsNames(list);
                DataTable dt = Transformer.CreateDataTable(columnnames, list);
                result = DataTableToListConverter.GetListFromDataTable(dt);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return result;
        }

        public async System.Threading.Tasks.Task<List<string>> PersistedMessagesSentToBroker()
        {
            List<string> result = new List<string>();
            try
            {
                List<PranaMessage> list = GetCpSentTrades(); List<string> columnnames = Transformer.GetColumnsNames(list);
                DataTable dt = Transformer.CreateDataTable(columnnames, list);
                result = DataTableToListConverter.GetListFromDataTable(dt);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return result;
        }

        public async System.Threading.Tasks.Task<List<string>> PersistedMessagesReceivedFromBroker()
        {
            List<string> result = new List<string>();
            try
            {
                List<PranaMessage> list = GetCPReceivedTrades(); List<string> columnnames = Transformer.GetColumnsNames(list);
                DataTable dt = Transformer.CreateDataTable(columnnames, list);
                result = DataTableToListConverter.GetListFromDataTable(dt);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return result;
        }

        public async System.Threading.Tasks.Task<OrderCollection> PendingPreTradeCompliance()
        {
            OrderCollection orderCollection = new OrderCollection();
            try
            {
                Dictionary<String, PranaMessage> msgs = _preTradeService.GetAllCachedMessages();
                foreach (String key in _preTradeService.GetAllCachedMessages().Keys)
                {
                    Order order = Transformer.CreateOrder(msgs[key]);
                    Prana.CommonDataCache.NameValueFiller.FillNameDetailsOfOrder(order);
                    orderCollection.Add(order);
                    order.OrderID = key;
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return orderCollection;
        }

        public async System.Threading.Tasks.Task<OrderCollection> PendingApprovalTrades()
        {
            OrderCollection orderCollection = new OrderCollection();
            try
            {
                Dictionary<String, PranaMessage> msgs = _preTradeService.GetPendingApprovalOrderCache();
                foreach (String key in msgs.Keys)
                {
                    Order order = Transformer.CreateOrder(msgs[key]);
                    Prana.CommonDataCache.NameValueFiller.FillNameDetailsOfOrder(order);
                    orderCollection.Add(order);
                    order.OrderID = key;
                }

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return orderCollection;
        }

        public async System.Threading.Tasks.Task<Dictionary<int, FixPartyDetails>> GetFixAllPartyDetails()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return FixEngineConnectionPoolManager.GetInstance().GetAllFixConnections();
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task OverideTrade(bool isAllowed, String orderId)
        {
            try
            {
                _preTradeService.OverideTrade(isAllowed, orderId);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task ReProcessMsg(string jsonDataRow)
        {
            try
            {
                DataRow dataRow = Prana.Global.Utilities.JsonHelper.DeserializeToObject<DataRow>(jsonDataRow);

                PranaMessage pranaMsg = Transformer.CreatePranaMessageThroughReflection(dataRow);

                List<PranaMessage> pranaMsgList = new List<PranaMessage>
                {
                    pranaMsg
                };
                FixEngineConnectionPoolManager.GetInstance().ReProcessMsg(pranaMsgList);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task ReProcessMsg2(Order pranaOrder)
        {
            try
            {
                PranaMessage pranaMsg = Transformer.CreatePranaMessageThroughReflection(pranaOrder);

                List<PranaMessage> pranaMsgList = new List<PranaMessage>
                {
                    pranaMsg
                };
                FixEngineConnectionPoolManager.GetInstance().ReProcessMsg(pranaMsgList);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task FixEngineConnectBuySide(int connectionID)
        {
            try
            {
                FixEngineConnectionPoolManager.GetInstance().ConnectBuySide(FixEngineConnectionPoolManager.GetInstance().GetPortForParty(connectionID));

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task FixEngineDisconnectBuySide(int connectionID)
        {
            try
            {
                FixEngineConnectionPoolManager.GetInstance().DisconnectBuySide(connectionID);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SetFixConnectionsAutoReconnectStatus(bool autoConnectStatus)
        {
            try
            {
                FixEngineConnectionPoolManager.GetInstance().SetFixConnectionsAutoReconnectStatus(autoConnectStatus);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<bool> IsComplianceModulePermitted()
        {
            try
            {
                bool isComplianceModulePermitted = false;
                isComplianceModulePermitted = CommonDataCache.ComplianceCacheManager.GetPreOrPostModuleEnabled() || CommonDataCache.Cache_Classes.HeatMapCacheManager.GetHeatMapModuleEnabled();

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                return isComplianceModulePermitted;
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

                    if (_expnlServiceClientHeartbeatManager != null)
                        _expnlServiceClientHeartbeatManager.Dispose();

                    if (_tradeServiceHeartbeatManager != null)
                        _tradeServiceHeartbeatManager.Dispose();

                    if (_proxyPublishing != null)
                        _proxyPublishing.Dispose();

                    if (_resendWaitTimer != null)
                        _resendWaitTimer.Dispose();

                    if (_sendUnsentOrdersWaitTimer != null)
                        _sendUnsentOrdersWaitTimer.Dispose();

                    ServerCustomCommunicationManager.GetInstance().Connected -= new ConnectionMessageReceivedDelegate(User_Connected);
                    ServerCustomCommunicationManager.GetInstance().Disconnected -= new ConnectionMessageReceivedDelegate(User_Disconnected);
                    ServerCustomCommunicationManager.GetInstance().UserExceptionDelegate -= ClearAccountUserCache;
                    FixEngineConnectionPoolManager.GetInstance().ConnectionStatusUpdate -= new EventHandler<EventArgs<FixPartyDetails>>(Server_FixConnectionStatusUpdate);

                    FixEngineConnectionPoolManager.GetInstance().Dispose();
                    ServerCustomCommunicationManager.GetInstance().Dispose();

                    if (MessageEngine.GetInstance() != null)
                        MessageEngine.GetInstance().Dispose();

                    PranaServiceHost.CleanUp();

                    if (_inFixMgrQueue != null)
                        _inFixMgrQueue.Dispose();

                    if (_outFixMgrQueue != null)
                        _outFixMgrQueue.Dispose();
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