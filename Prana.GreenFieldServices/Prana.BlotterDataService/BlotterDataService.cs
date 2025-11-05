using Castle.Windsor;
using Newtonsoft.Json;
using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.Authentication.Common;
using Prana.BlotterDataService.DTO;
using Prana.BusinessObjects;
using Prana.BusinessObjects.BlotterDataService;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.Blotter;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.SocketCommunication;
using Prana.TradeManager.Extension;
using Prana.TradeManager.Extension.CacheStore;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using static Prana.BlotterDataService.DTO.CompanyTransferTradeRules;
using static Prana.KafkaWrapper.Extension.Classes.KafkaConstants;
using static Prana.BlotterDataService.DTO.EditTradeAttributesDto;
using Prana.Allocation.Common.Helper;
using Prana.GreenfieldServices.Common;
using System.Threading;

namespace Prana.BlotterDataService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class BlotterDataService : BaseService, IBlotterDataService, IDisposable, IPublishing
    {
        #region Variables

        private IWindsorContainer _container;

        /// <summary>
        /// Trade subsciption proxy for blotter updates
        /// </summary>
        private DuplexProxyBase<ISubscription> _tradeSubscriptionProxy;

        #region Blotter timer
        /// <summary>
        /// Stores value of BlotterUpdateTimeInterval from App.config file
        /// </summary>
        private int _blotterUpdateTimeInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_BlotterUpdateTimeInterval));

        /// <summary>
        ///Timer based on BlotterUpdateTimeInterval to sends updated blotter data to Web application
        /// </summary>
        private System.Timers.Timer _blotterUpdateTimer = new System.Timers.Timer();

        /// <summary>
        /// Updated orders cache for blotter when _blotterUpdateTimeInterval not elapsed
        /// </summary>
        private Dictionary<string, OrderSingle> _dictOrdersForBlotter = new Dictionary<string, OrderSingle>();

        /// <summary>
        /// Updated orders cache for Web application when _blotterUpdateTimeInterval gets elapsed
        /// </summary>
        private Dictionary<string, List<OrderSingle>> _updatedBlotterData = new Dictionary<string, List<OrderSingle>>();

        /// <summary>
        /// Locker object for _dictOrdersForBlotter
        /// </summary>
        private readonly object _lockerDictMessageToBeSendToBlotter = new object();
        #endregion

        /// <summary>
        /// Cache to maintain order(s) with pendingNew status
        /// </summary>
        private Dictionary<string, OrderSingle> _dictPendingNewAlertCollection = new Dictionary<string, OrderSingle>();

        /// <summary>
        ///  Cache to maintain order(s) with pendingNew status used for notification
        /// </summary>
        private Dictionary<string, OrderSingle> _dictPendingAssignedMethodCalled = new Dictionary<string, OrderSingle>();

        /// <summary>
        /// Logged-in company users information collection
        ///</summary>
        Dictionary<int, CompanyUser> _userWiseLoggedInUserInformation = new Dictionary<int, CompanyUser>();

        /// <summary>
        /// Company pending new trade alert preference
        /// </summary>
        bool IsPendingNewTradeAlertAllowed = false;

        /// <summary>
        /// Comapny pending new trade alert time preference 
        /// </summary>
        int PendingNewOrderAlertTime = 0;

        /// <summary>
        /// _companyID
        /// </summary>
        private int _companyID;
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        private int _cleanedUp = 0;
        /// <summary>
        /// Stores information of Users logged into Web application.
        /// </summary>
        private static Dictionary<int, object> _dictLoggedInUser = new Dictionary<int, object>();

        /// <summary>
        /// Locker object
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// Cache to maintain logged-in user's information requested or not
        /// Need to request data once for each logged-in user
        /// Data not requested when user logged-in because cache creation takes time at common data service
        /// </summary>
        Dictionary<int, bool> _loggedInUserInformationRquested = new Dictionary<int, bool>();

        /// <summary>
        /// Tracks whether the logged-in user response has been received from Auth Service
        /// True if received; otherwise, false.
        /// </summary>
        bool _loggedInUserResponseReceivedFromAuth = false;

        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

        #region Instance variables
        private static TradeManagerExtension _tradeManagerExtension;
        private static TradeManager _tradeManager;
        private static BlotterLayoutManager _blotterLayoutManager;
        private static ShortLocateManager _shortLocateManager;
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        #endregion

        #endregion

        #region IPranaServiceCommon Methods
        /// <summary>
        /// InitialiseService
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
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
                #endregion

                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                WindsorContainerManager.Container = container;

                #region Socket connection and proxy creation
                MakeProxy();
                ConnectToAllSockets();
                #endregion

                CompanyID = CachedDataManager.GetInstance.GetCompanyID();
                FixDictionaryHelper.LoadFixDictionary();

                #region SubscribeAndConsume
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                #region Topics for user login/logout
                //Topic for logged-in user(s) information
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_UserLoggedInInformation);
                //Topic for logged-out user(s) information
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_UserLoggedOutInformation);
                #endregion

                //Topic to fetch blotter data
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetDataRequest, KafkaManager_BlotterGetDataRequest);
                //Topic to get required preference data for logged-in user
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserDataForBlotterResponse, KafkaManager_UserDataForBlotterReceived);
                //Topic for algo strategies information
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_AllAlgoStrategiesInfoResponse, KafkaManager_AllAlgoStrategiesInfoResponse, true);
                //Topic to get company's trading preferences
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompanyTradingPreferencesResponse, KafkaManager_CompanyTransferTradeRulesReceived);
                //Topic for logged in user(s) information
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserWiseLoggedInInformationResponse, KafkaManager_UserWiseLoggedInInformationReceived);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetPSTPreferenceDetailsRequest, KafkaManager_BlotterGetPSTPreferenceDetailsRequest);

                #region Topic for Blotter operations
                //Topic for cancel order(s) operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterCancelAllSubsRequest, KafkaManager_BlotterCancelAllSubsRequest);
                //Topic for remove order(s) operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveOrdersRequest, KafkaManager_BlotterRemoveOrdersRequest);
                //Topic to freeze order(s) in pending compliance UI
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_FreezePendingComplianceRowsRequest, KafkaManager_FreezePendingComplianceRows);
                //Topic to unfreeze order(s) in pending compliance UI
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UnfreezePendingComplianceRowsRequest, KafkaManager_UnfreezePendingComplianceRows);
                //Topic for remove manual execution operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRemoveManualExecutionRequest, KafkaManager_BlotterRemoveManualExecutionRequest);
                //Topic to get order details for add/modify fills operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterGetManualFillsRequest, KafkaManager_BlotterGetManualFillsRequest);
                //Topic to save order details for add/modify fills operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterSaveManualFillsRequest, KafkaManager_BlotterSaveManualFillsRequest);
                //Topic to get order allocation details for allocate operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetAllocationDetailsRequest, KafkaManager_GetAllocationDetailsRequest);
                //Topic to save order allocation details for allocate operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveAllocationDetailsRequest, KafkaManager_SaveAllocationDetailsRequest);
                //Topic for rollover sub order(s) operation
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_BlotterRolloverAllSubsRequest, KafkaManager_BlotterRolloverAllSubsRequest);
                //Topic for get pst allocation details
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetPstAllocationDetailsRequest, KafkaManager_GetPstAllocationDetailsRequest);
                //Topic to transfer trades
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_TransferUserRequest, KafkaManager_TransferUser);
                #endregion
                #endregion

                #region Instance and event wire
                _tradeManagerExtension = TradeManagerExtension.GetInstance();
                _tradeManagerExtension.SetCommunicationManager = _tradeCommunicationManager;
                _tradeManagerExtension.CompanyID = CompanyID;
                _tradeManagerExtension.IsWebApplication = true;
                _shortLocateManager = ShortLocateManager.GetInstance();
                _shortLocateManager.ShortLocateCollection = _shortLocateManager.FetchShortLocateDetailsForTrade();
                _tradeManager = TradeManager.GetInstance();
                _tradeManager.CompanyID = CompanyID;
                _tradeCommunicationManager.Connect(GetTradeServerConnectionDetails());
                _blotterLayoutManager = BlotterLayoutManager.GetInstance();
                BlotterOrderCollections.GetInstance().SendBlotterTradesEventHandler += SendBlotterTradesEvent;
                BlotterOrderCollections.GetInstance().UpdateShortLocateData += BlotterDataService_UpdateShortLocateData;
                BlotterOrderCollections.GetInstance().RequiresComplianceApprovalEventHandler += SendRequiresComplianceApprovalUpdates;
                GetBlotterPreferenceDataAsync();
                #endregion

                #region Blotter update data timer
                _blotterUpdateTimer.Interval = _blotterUpdateTimeInterval;
                _blotterUpdateTimer.Elapsed += BlotterUpdateTimer_Elapsed;
                _blotterUpdateTimer.Start();
                #endregion

                #region TradeAttributes
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesRequest, KafkaManager_GetOrderDetailsForEditTradeAttributesRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveEditedTradeAttributesRequest, KafkaManager_SaveEditedTradeAttributesRequest);

                #endregion
                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");

                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                // fire and forget method for handling of auth service connection
                ServiceConnectionPoller.PollUntilServiceReady(() => _loggedInUserResponseReceivedFromAuth, KafkaConstants.TOPIC_InitializeLoggedInUserRequest, ServiceNameConstants.CONST_Auth_DisplayName);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                    "InitialiseService Completed for BlotterData Service in {0} ms.", sw.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitialiseService encountered an error");
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
            if (Interlocked.Exchange(ref _cleanedUp, 1) == 1) return;

            // 1) Stop periodic callbacks BEFORE disposing anything they use
            StopServiceHealthPollingTimer();

            // 2) Mark down (and publish a final “down”)
            UpdateServiceStatus(ServiceNameConstants.CONST_BlotterData_Name, ServiceNameConstants.CONST_BlotterData_DisplayName, false);

            Console.WriteLine(BlotterDataConstants.MSG_ShutdownService);

            Logger.LogMsg(LoggerLevel.Information, "Shutting down Service...");
            _container.Dispose();
        }
        #endregion

        #region Subscribe/Unsubscribe proxy
        /// <summary>
        /// Subscribe Trade subscription proxy
        /// </summary>
        private void MakeProxy()
        {
            try
            {
                _tradeSubscriptionProxy = new DuplexProxyBase<ISubscription>(EndPointAddressConstants.CONST_TradeSubscriptionEndpoint, this);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_CreateGroup, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_CreateGroupForWeb, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_UpdatesForWebBlotter, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_StageOrderRemovalFromBlotter, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_SubOrderRemovalFromBlotter, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_PendingOrderUpdatesForWeb, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_OverrideOrderUpdatesForWeb, null);
                _tradeSubscriptionProxy.Subscribe(Topics.Topic_UpdateBlotterStatusBarMessage, null);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "MakeProxy encountered an error");

            }
        }

        /// <summary>
        /// UnSubscribe Trade subscription proxy
        /// </summary>
        internal void UnSubscribeProxy()
        {
            try
            {
                if (_tradeSubscriptionProxy != null)
                {
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_CreateGroup);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_CreateGroupForWeb);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_UpdatesForWebBlotter);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_StageOrderRemovalFromBlotter);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_SubOrderRemovalFromBlotter);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_PendingOrderUpdatesForWeb);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_OverrideOrderUpdatesForWeb);
                    _tradeSubscriptionProxy.InnerChannel.UnSubscribe(Topics.Topic_UpdateBlotterStatusBarMessage);
                    _tradeSubscriptionProxy.Dispose();
                    _tradeSubscriptionProxy = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UnSubscribeProxy encountered an error");
            }
        }
        #endregion

        #region Event handlers

        /// <summary>
        /// Event for SendBlotterTradesEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBlotterTradesEvent(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                _tradeManagerExtension.SendTradeAfterCheckCPConnection(e.Value);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SendBlotterTradesEvent encountered an error");
            }
        }

        /// <summary>
        /// Event for Updating Short Locate details Trades EventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlotterDataService_UpdateShortLocateData(object sender, EventArgs<ShortLocateListParameter> e)
        {
            try
            {
                _shortLocateManager.UpdateShortLocateData(e.Value);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "BlotterDataService_UpdateShortLocateData encountered an error");
            }
        }

        /// <summary>  
        /// Publishes the current service status to a Kafka topic.  
        /// This method retrieves the service status, serializes it, and sends it to the Kafka topic  
        /// specified in the KafkaConstants.TOPIC_ServiceHealthStatus.  
        /// </summary>  
        private async void ProduceServiceStatusMessage()
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_BlotterData_Name, ServiceNameConstants.CONST_BlotterData_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_BlotterData_Name);
                var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
                // Publish the message to the Kafka topic  
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);

                Logger.LogMsg(LoggerLevel.Verbose, "Service status published to Kafka topic {0}", KafkaConstants.TOPIC_ServiceHealthStatus);
            }
            catch (Exception ex)
            {
                // Log any errors encountered during the process  
                Logger.LogError(ex, "ProduceServiceStatusMessage encountered an error");
            }
        }
        #endregion

        #region Background worker
        /// <summary>
        /// Get/Sets Blotter preference data asynchornously.
        /// </summary>
        private async void GetBlotterPreferenceDataAsync()
        {
            try
            {
                var sw = Stopwatch.StartNew();

                BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC.Clear();
                _tradeManagerExtension.BlotterClearanceCommonData = DBTradeManager.GetInstance().GetCompanyClearanceCommonData(_companyID);
                BlotterCommonCache.GetInstance().ClearanceDataFull = DBTradeManager.GetInstance().GetClearanceData(CompanyID, ref BlotterCommonCache.GetInstance().DictRolloverPermittedAUEC);

                #region Code regarding Clearance job scheduler
                BlotterOrderCollections.GetInstance().ClearAllCollections();
                _tradeManagerExtension.AddClearanceSchedulerTasks();
                _tradeManagerExtension.SetupAUECWiseClearanceTime();
                #endregion

                Logger.LogMsg(LoggerLevel.Debug, "GetBlotterPreferenceDataAsync processed successfully in {0} ms for compId", sw.ElapsedMilliseconds, CompanyID);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetBlotterPreferenceDataAsync encountered an error");
            }
        }
        #endregion

        #region Centralized publish receiver
        /// <summary>
        /// Centralized publish receiver.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="topicName"></param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                lock (_locker)
                {
                    object[] dataList = (object[])e.EventData;
                    switch (e.TopicName)
                    {
                        case Topics.Topic_CreateGroup:
                            //Response Group List
                            PublishGroupUpdates(dataList);
                            break;
                        case Topics.Topic_CreateGroupForWeb:
                            //This will only be done in case of Edit Trade Attributes operation from Web Blotter
                            PublishGroupUpdates(dataList);
                            break;

                        case Topics.Topic_UpdatesForWebBlotter:
                            List<PranaMessage> messagesReceived = dataList.Where(x => x is PranaMessage).Select(y => (y as PranaMessage)).ToList();
                            foreach (PranaMessage pranaMsg in messagesReceived)
                            {
                                OrderSingle message = Transformer.CreateOrderSingle(pranaMsg);
                                if (message != null)
                                {
                                    lock (_lockerDictMessageToBeSendToBlotter)
                                    {
                                        if (message.Text.Equals(PreTradeConstants.MSG_PENDING_COMPLIANCE_APPROVAL_CHANGE_ORDER_STATUS) && message.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingNew))
                                        {
                                            message.Text = string.Empty;
                                            message.OrderStatusTagValue = FIXConstants.ORDSTATUS_PendingReplace;
                                        }
                                        if (_dictOrdersForBlotter.ContainsKey(message.ParentClOrderID))
                                        {
                                            if (_dictOrdersForBlotter[message.ParentClOrderID].OrderSeqNumber <= message.OrderSeqNumber && _dictOrdersForBlotter[message.ParentClOrderID].OrderSeqNumber != long.MinValue)
                                            {
                                                _dictOrdersForBlotter[message.ParentClOrderID] = message;
                                            }
                                            else
                                            {
                                                if (message.Text.Equals(PreTradeConstants.MsgTradePending) || message.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingReplace) || message.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingCancel))
                                                    _dictOrdersForBlotter[message.ParentClOrderID] = message;
                                            }
                                        }
                                        else
                                        {
                                            _dictOrdersForBlotter.Add(message.ParentClOrderID, message);
                                        }
                                    }

                                    //Sends live and manual order update.
                                    if (!string.IsNullOrEmpty(message.StagedOrderID))
                                        SendOrderUpdates(message);

                                    if (!message.IsManualOrder && !string.IsNullOrEmpty(message.StagedOrderID))
                                    {
                                        if (_dictPendingNewAlertCollection.ContainsKey(message.ParentClOrderID))
                                        {
                                            _dictPendingNewAlertCollection[message.ParentClOrderID] = message;
                                        }
                                        else
                                        {
                                            _dictPendingNewAlertCollection.Add(message.ParentClOrderID, message);
                                        }
                                    }

                                    if (IsPendingNewTradeAlertAllowed && !message.IsManualOrder && message.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew &&
                                        _dictLoggedInUser.ContainsKey(message.CompanyUserID))
                                    {
                                        CheckPendingNewStatus(message);
                                    }

                                    #region Adding pending compliance approval trades in cache
                                    BlotterOrderCollections.GetInstance().AddRemovePendingApprovalOrders(message);
                                    #endregion
                                }
                            }
                            break;

                        case Topics.Topic_StageOrderRemovalFromBlotter:
                            if (dataList != null && dataList.Length > 0)
                            {
                                StageOrderRemovalData stageOrderRemovalData = (StageOrderRemovalData)dataList[0];
                                Dictionary<string, Dictionary<string, OrderSingle>> result = _tradeManager.RemoveStageOrSubOrderFromCollection(stageOrderRemovalData.ParentClOrderIds);
                                if (result != null && result.Count > 0 && result.ContainsKey(BlotterDataConstants.CONST_RemovedOrders))
                                    SendRemoveOrdersInfo(result[BlotterDataConstants.CONST_RemovedOrders], BlotterRequestType.RemoveStageOrder);
                            }
                            break;

                        case Topics.Topic_SubOrderRemovalFromBlotter:
                            if (dataList != null && dataList.Length > 0)
                            {
                                SubOrderRemovalData stageOrderRemovalData = (SubOrderRemovalData)dataList[0];
                                Dictionary<string, Dictionary<string, OrderSingle>> finalResult = _tradeManager.RemoveStageOrSubOrderFromCollection(stageOrderRemovalData.SubOrdersClOrderIds, true);
                                if (finalResult != null && finalResult.Count > 0)
                                {
                                    if (finalResult.ContainsKey(BlotterDataConstants.CONST_RemovedOrders) && finalResult[BlotterDataConstants.CONST_RemovedOrders].Count > 0)
                                        SendRemoveOrdersInfo(finalResult[BlotterDataConstants.CONST_RemovedOrders], BlotterRequestType.RemoveExecution);
                                    else if (!string.IsNullOrEmpty(stageOrderRemovalData.SubOrdersClOrderIds))
                                    {
                                        Dictionary<string, OrderSingle> subOrderToRemove = new Dictionary<string, OrderSingle>();
                                        Dictionary<string, OrderSingle> ordersToUpdate = new Dictionary<string, OrderSingle>();
                                        foreach (OrderSingle stageOrder in BlotterOrderCollections.GetInstance().DictParentClOrderIDCollection.Values)
                                        {
                                            if (stageOrder.OrderCollection != null)
                                            {
                                                OrderSingle subOrder = stageOrder.OrderCollection.FirstOrDefault(x => x.ClOrderID.Equals(stageOrderRemovalData.SubOrdersClOrderIds));
                                                if (subOrder != null)
                                                {
                                                    subOrderToRemove.Add(subOrder.ParentClOrderID, subOrder);
                                                    stageOrder.OrderCollection.Remove(subOrder);
                                                    BlotterOrderCollections.GetInstance().UpdateStatusFromChildCollection(stageOrder);
                                                    ordersToUpdate.Add(stageOrder.ParentClOrderID, stageOrder);
                                                }
                                            }
                                        }
                                        SendRemoveOrdersInfo(subOrderToRemove, BlotterRequestType.RemoveExecution);
                                        Dictionary<string, List<OrderSingle>> updatedStageOrders = BlotterOrderCollections.GetInstance().UpdateBlotterCollection(ordersToUpdate.Values.ToList());
                                        SendBlotterData(updatedStageOrders);
                                    }

                                    #region Sending updated stage orders to service gateway
                                    if (finalResult.ContainsKey(BlotterDataConstants.CONST_OrderTab) && finalResult[BlotterDataConstants.CONST_OrderTab].Count > 0)
                                    {
                                        Dictionary<string, List<OrderSingle>> updatedOrders = new Dictionary<string, List<OrderSingle>>
                                        {
                                            { BlotterDataConstants.CONST_OrderTab, finalResult[BlotterDataConstants.CONST_OrderTab].Values.ToList() }
                                        };
                                        SendBlotterData(updatedOrders);
                                    }
                                    #endregion
                                }
                            }
                            break;

                        case Topics.Topic_OverrideOrderUpdatesForWeb:
                            List<PranaMessage> messagesList = dataList.Where(x => x is PranaMessage).Select(y => (y as PranaMessage)).ToList();
                            foreach (PranaMessage pranaMsg in messagesList)
                            {
                                OrderSingle message = Transformer.CreateOrderSingle(pranaMsg);
                                SendOrderUpdates(message, true);
                            }
                            break;

                        case Topics.Topic_PendingOrderUpdatesForWeb:
                            List<PranaMessage> messages = dataList.Where(x => x is PranaMessage).Select(y => (y as PranaMessage)).ToList();
                            foreach (PranaMessage pranaMsg in messages)
                            {
                                OrderSingle order = Transformer.CreateOrderSingle(pranaMsg);
                                SendRequiresComplianceApprovalUpdates(this, new EventArgs<OrderSingle>(order));
                            }
                            break;
                        case Topics.Topic_UpdateBlotterStatusBarMessage:
                            if (dataList != null && dataList.Length > 0)
                            {
                                PublishOrderUpdates(BlotterDataConstants.TITLE_ORDER_ROLLOVER, dataList[0].ToString(), Convert.ToInt32(dataList[1]), 0);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Publish encountered an error");
            }
        }
        /// <summary>
        /// Publishes group updates to the service gateway.
        /// </summary>
        /// <param name="dataList"></param>
        private void PublishGroupUpdates(object[] dataList)
        {
            List<AllocationGroup> responseGroups = dataList.Where(x => x is AllocationGroup).Select(y => (y as AllocationGroup)).ToList();
            if (responseGroups.Count > 0)
            {
                List<AllocationDetails> allocationDetails = new List<AllocationDetails>();
                //Update Order Allocation State Dictionary
                responseGroups.FindAll(x => x.PersistenceStatus != ApplicationConstants.PersistenceStatus.UnGrouped).ForEach(group =>
                {
                    group.Orders.ForEach(order =>
                    {
                        AllocationDetails allocationDetail = new AllocationDetails();
                        allocationDetail.ClOrderID = order.ParentClOrderID;
                        allocationDetail.AllocationStatus = group.State;
                        allocationDetail.Level1Allocation = group.Allocations.Collection;
                        allocationDetail.AllocationSchemeName = group.AllocationSchemeName;
                        allocationDetail.TradeAttribute1 = group.TradeAttribute1;
                        allocationDetail.TradeAttribute2 = group.TradeAttribute2;
                        allocationDetail.TradeAttribute3 = group.TradeAttribute3;
                        allocationDetail.TradeAttribute4 = group.TradeAttribute4;
                        allocationDetail.TradeAttribute5 = group.TradeAttribute5;
                        allocationDetail.TradeAttribute6 = group.TradeAttribute6;
                        allocationDetail.SetTradeAttribute(group.GetTradeAttributesAsDict());

                        if (!allocationDetails.Contains(allocationDetail))
                            allocationDetails.Add(allocationDetail);
                    });
                });

                Dictionary<string, List<OrderSingle>> updatedOrders = _tradeManager.UpdateAllocationDetails(allocationDetails);

                //Sending updated data to service gateway.
                if (updatedOrders != null && updatedOrders.Count > 0)
                {
                    SendBlotterData(updatedOrders);
                }
                updatedOrders.Clear();
                allocationDetails.Clear();
            }
        }
        #endregion

        #region Blotter timer elapse
        /// <summary>
        /// BlotterUpdateTimer_Elapsed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlotterUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (_lockerDictMessageToBeSendToBlotter)
                {
                    if (_dictOrdersForBlotter != null && _dictOrdersForBlotter.Count > 0)
                    {
                        _updatedBlotterData = BlotterOrderCollections.GetInstance().UpdateBlotterCollection(_dictOrdersForBlotter.Values.ToList());

                        #region Sending updated data to service gateway.
                        if (_updatedBlotterData != null && _updatedBlotterData.Count > 0)
                        {
                            SendBlotterData(_updatedBlotterData);
                        }
                        #endregion

                        _updatedBlotterData.Clear();
                        _dictOrdersForBlotter.Clear();
                    }

                    _blotterUpdateTimer.Stop();
                    _blotterUpdateTimer.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        #region Notification for Order(s) updates
        /// <summary>
        /// Sends notification for order(s) which requires Compliance Approval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendRequiresComplianceApprovalUpdates(object sender, EventArgs<OrderSingle> e)
        {
            try
            {
                OrderSingle incomingOrder = e.Value;
                string title = string.Empty;
                string message = string.Empty;
                string orderside = TagDatabaseManager.GetInstance.GetOrderSideText(incomingOrder.OrderSideTagValue);
                string ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(incomingOrder.OrderTypeTagValue);
                double limitPrice = 0;
                if (incomingOrder.Price != double.Epsilon)
                    limitPrice = Math.Round(incomingOrder.Price, 4, MidpointRounding.AwayFromZero);
                double stopPrice = Math.Round(incomingOrder.StopPrice, 4, MidpointRounding.AwayFromZero);
                double quantity = Math.Round(incomingOrder.Quantity, 4, MidpointRounding.AwayFromZero);
                string tif = TagDatabaseManager.GetInstance.GetTIFText(incomingOrder.TIF);

                message = orderside + BlotterDataConstants.CONST_SPACE + incomingOrder.Symbol + BlotterDataConstants.CONST_SPACE + quantity + BlotterDataConstants.CONST_AT;

                if (incomingOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)
                    message += limitPrice;
                else if (incomingOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Stoplimit)
                    message += limitPrice + BlotterDataConstants.MSG_WITH_STOP + BlotterDataConstants.CONST_AT + stopPrice;
                else if (incomingOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop)
                    message += BlotterDataConstants.MSG_MARKET + BlotterDataConstants.MSG_WITH_STOP + BlotterDataConstants.CONST_AT + stopPrice;
                else
                    message += BlotterDataConstants.MSG_MARKET;

                message += string.IsNullOrEmpty(tif) ? string.Empty : " " + tif;

                #region Set message and title for requires compliance approval orders
                if (incomingOrder.Text == PreTradeConstants.MsgTradePending && !incomingOrder.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Cancelled))
                {
                    title = BlotterDataConstants.TITLE_ORDER_SENT_TO_COMPLIANCE_OFFICER;
                    message += BlotterDataConstants.MSG_IS_SENT_FOR_APPROVAL;
                }
                else if (incomingOrder.Text == string.Empty)
                {
                    title = BlotterDataConstants.TITLE_ORDER_APPROVED_COMPLIANCE_OFFICER;
                    message += BlotterDataConstants.MSG_IS_APPROVED;
                }
                else if (incomingOrder.Text == PreTradeConstants.MsgTradeReject)
                {
                    title = BlotterDataConstants.TITLE_ORDER_REJECTED_COMPLIANCE_OFFICER;
                    message += BlotterDataConstants.MSG_IS_REJECTED;
                }
                #endregion

                #region Send updates
                if (!string.IsNullOrEmpty(title))
                    PublishOrderUpdates(title, message, incomingOrder.CompanyUserID, incomingOrder.AssetID);
                #endregion
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sends notification for order(s)
        /// </summary>
        /// <param name="incomingOrder"></param>
        private void SendOrderUpdates(OrderSingle incomingOrder, bool isSendRequired = false)
        {
            try
            {
                string title = string.Empty;
                string message = string.Empty;
                string orderside = TagDatabaseManager.GetInstance.GetOrderSideText(incomingOrder.OrderSideTagValue);
                string ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(incomingOrder.OrderTypeTagValue);
                string tif = TagDatabaseManager.GetInstance.GetTIFText(incomingOrder.TIF);
                double limitPrice = 0;
                if (incomingOrder.Price != double.Epsilon)
                    limitPrice = Math.Round(incomingOrder.Price, 4, MidpointRounding.AwayFromZero);
                double stopPrice = Math.Round(incomingOrder.StopPrice, 4, MidpointRounding.AwayFromZero);
                double quantity = Math.Round(incomingOrder.Quantity, 4, MidpointRounding.AwayFromZero);
                string counterpartyname = incomingOrder.CounterPartyName;
                if (incomingOrder.CounterPartyName == BlotterDataConstants.CONST_DEFAULT_BROKERS && incomingOrder.CounterPartyID > 0)
                {
                    counterpartyname = CachedDataManager.GetInstance.GetCounterPartyText(incomingOrder.CounterPartyID);
                }

                message = orderside + BlotterDataConstants.CONST_SPACE + incomingOrder.Symbol + BlotterDataConstants.CONST_SPACE + quantity + BlotterDataConstants.CONST_SPACE + BlotterDataConstants.CONST_AT;

                if (incomingOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Limit)
                    message += limitPrice;
                else if (incomingOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Stoplimit)
                    message += limitPrice + BlotterDataConstants.MSG_WITH_STOP + BlotterDataConstants.CONST_AT + stopPrice;
                else if (incomingOrder.OrderTypeTagValue == FIXConstants.ORDTYPE_Stop)
                    message += BlotterDataConstants.MSG_MARKET + BlotterDataConstants.MSG_WITH_STOP + BlotterDataConstants.CONST_AT + stopPrice;
                else
                    message += BlotterDataConstants.MSG_MARKET;

                message += (string.IsNullOrEmpty(tif) ? string.Empty : " " + tif) + BlotterDataConstants.MSG_To + counterpartyname;

                #region Set message and title for live orders in market
                if (!incomingOrder.IsManualOrder)
                {
                    if ((incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected && !string.IsNullOrEmpty(incomingOrder.ExecID)) || incomingOrder.Text.Equals(BlotterDataConstants.MSG_USER_REQUESTED_REJECT))
                    {
                        title = BlotterDataConstants.TITLE_ORDER_REJECTED + counterpartyname;
                        message += BlotterDataConstants.MSG_IS_REJECTED;
                    }
                    else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_New && incomingOrder.CumQty == 0 && incomingOrder.ExecID.Contains(BlotterDataConstants.CONST_ACK))
                    {
                        title = BlotterDataConstants.TITLE_ORDER_ACKNOWLEDGED + counterpartyname;
                        message += BlotterDataConstants.MSG_IS_ACKNOWLEDGED;
                    }
                    else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Filled && !string.IsNullOrEmpty(incomingOrder.ExecID))
                    {
                        title = BlotterDataConstants.TITLE_ORDER_COMPLETED;
                        message += BlotterDataConstants.MSG_IS_COMPLETED;
                    }
                    else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Cancelled && incomingOrder.ExecID.Contains(BlotterDataConstants.CONST_CANCEL_ACK) && !incomingOrder.Text.Equals(BlotterDataConstants.CONST_BlockedByCompliance))
                    {
                        title = BlotterDataConstants.TITLE_ORDER_CANCELLED + counterpartyname;
                        message += BlotterDataConstants.MSG_IS_ACKNOWLEDGED;
                    }
                    else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Replaced && incomingOrder.ExecType == FIXConstants.ORDSTATUS_Replaced && !string.IsNullOrEmpty(incomingOrder.ExecID))
                    {
                        title = BlotterDataConstants.TITLE_ORDER_REPLACED + counterpartyname;
                        message += BlotterDataConstants.MSG_IS_ACKNOWLEDGED;
                    }
                }
                #endregion

                #region Set message and title for live and manual orders
                if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_DoneForDay)
                {
                    title = BlotterDataConstants.TITLE_ORDER_DONE_FOR_DAY;
                    message += BlotterDataConstants.MSG_IS_DONE_FOR_DAY;
                }
                else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_RollOver && incomingOrder.MsgType != FIXConstants.MSGOrderRollOverRequest)
                {
                    title = BlotterDataConstants.TITLE_ORDER_ROLLOVER;
                    message += BlotterDataConstants.MSG_IS_ROLLOVER;
                }
                else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Stopped)
                {
                    title = BlotterDataConstants.TITLE_ORDER_STOPPED;
                    message += BlotterDataConstants.MSG_IS_STOPPED;
                }
                else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Suspended)
                {
                    title = BlotterDataConstants.TITLE_ORDER_SUSPENDED;
                    message += BlotterDataConstants.MSG_IS_SUSPENDED;
                }
                else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Calculated)
                {
                    title = BlotterDataConstants.TITLE_ORDER_CALCULATED;
                    message += BlotterDataConstants.MSG_IS_CALCULATED;
                }
                else if (incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Expired)
                {
                    title = BlotterDataConstants.TITLE_ORDER_EXPIRED;
                    message += BlotterDataConstants.MSG_IS_EXPIRED;
                }
                #endregion

                #region Set title for override Manual and stage orders
                if (isSendRequired && incomingOrder.IsManualOrder && !string.IsNullOrEmpty(incomingOrder.StagedOrderID))
                {
                    title = BlotterDataConstants.TITLE_MANUAL_ORDER_SUCESS;
                }
                else if (isSendRequired && !incomingOrder.IsManualOrder && incomingOrder.IsStageRequired && incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                {
                    title = BlotterDataConstants.TITLE_ORDER_SENT_SUCCESSFULLY;
                }
                else if (isSendRequired && string.IsNullOrEmpty(title) && string.IsNullOrEmpty(incomingOrder.StagedOrderID) && incomingOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                {
                    title = BlotterDataConstants.TITLE_STAGE_ORDER_SENT;
                }
                #endregion

                #region Send updates
                if (!string.IsNullOrEmpty(title))
                {
                    PublishOrderUpdates(title, message, incomingOrder.ModifiedUserId, incomingOrder.AssetID);
                    Logger.LogMsg(LoggerLevel.Information, "Order Update published for orderId {0} with msg {1}",
                        incomingOrder.ParentClOrderID, message);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SendOrderUpdates encountered an error for incoming order parentClOrderID:" + incomingOrder.ParentClOrderID);
            }
        }

        /// <summary>
        /// Publish order updates for Web application
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="companyUserId"></param>
        /// <param name="assetID">Added this parameter so that on frontend we can check asset class and show order side in notifcation message according to it</param>
        private void PublishOrderUpdates(string title, string message, int companyUserId, int assetID)
        {
            using (LoggerHelper.PushUserPropInLogging(companyUserId))
            {
                try
                {
                    Dictionary<string, string> OrderUpdatesInfo = new Dictionary<string, string>
                {
                    { BlotterDataConstants.CONST_TITLE, title },
                    { BlotterDataConstants.CONST_MESSAGE, message },
                    { BlotterDataConstants.CONST_COMPANY_USER_ID, companyUserId.ToString() },
                    { BlotterDataConstants.CONST_ASSET_ID, assetID.ToString() }
                };

                    BlotterResponse blotterResponse = new BlotterResponse(BlotterRequestType.OrderUpdates, null, null, "", JsonConvert.SerializeObject(OrderUpdatesInfo));
                    RequestResponseModel response = new RequestResponseModel(companyUserId, JsonConvert.SerializeObject(blotterResponse), null);

                    _ = KafkaManager.Instance.Produce(TOPIC_OrderUpdatesData, response);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"PublishOrderUpdates encountered an error with title:{title} & message:{message}");
                }
            }
        }

        /// <summary>
        /// Checks the pending new status.
        /// </summary>
        /// <param name="incomingOrder">The incoming order.</param>
        private void CheckPendingNewStatus(OrderSingle incomingOrder)
        {
            try
            {
                if (!_dictPendingAssignedMethodCalled.ContainsKey(incomingOrder.ParentClOrderID))
                {
                    _dictPendingAssignedMethodCalled.Add(incomingOrder.ParentClOrderID, incomingOrder);
                    PendingNewAlertInformation(incomingOrder.ParentClOrderID);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CheckPendingNewStatus encountered an error for orderId:" + incomingOrder.OrderID);
            }
        }

        /// <summary>
        /// Creates message header and body for pop up 
        /// </summary>
        /// <param name="ParentClOrderID">The ParentClOrderID.</param>
        private async void PendingNewAlertInformation(string ParentClOrderID)
        {
            try
            {
                await System.Threading.Tasks.Task.Delay(1000 * PendingNewOrderAlertTime);
                string message = ShowPendingPopUpMessage(ParentClOrderID);
                if (!string.IsNullOrEmpty(message))
                {
                    int companyUserID = _dictPendingNewAlertCollection[ParentClOrderID].CompanyUserID;
                    string title = "Trader : " + (_userWiseLoggedInUserInformation.ContainsKey(companyUserID) ? _userWiseLoggedInUserInformation[companyUserID].FirstName : string.Empty);

                    Dictionary<string, string> OrderUpdatesInfo = new Dictionary<string, string>
                    {
                        { BlotterDataConstants.CONST_TITLE, title },
                        { BlotterDataConstants.CONST_MESSAGE, message },
                        { BlotterDataConstants.CONST_COMPANY_USER_ID, companyUserID.ToString() }
                    };
                    BlotterResponse blotterResponse = new BlotterResponse(BlotterRequestType.PendingNewAlert, null, null, "", JsonConvert.SerializeObject(OrderUpdatesInfo));
                    RequestResponseModel response = new RequestResponseModel(companyUserID,
                        JsonConvert.SerializeObject(blotterResponse), null);
                    _ = KafkaManager.Instance.Produce(TOPIC_OrderUpdatesData, response);
                }
                if (_dictPendingNewAlertCollection.ContainsKey(ParentClOrderID))
                    _dictPendingNewAlertCollection.Remove(ParentClOrderID);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"PendingNewAlertInformation encountered an error for ParentClOrderID {ParentClOrderID}");
            }
        }

        /// <summary>
        /// Shows the pending pop up message.
        /// </summary>
        /// <param name="ClOrderID">The cl order identifier.</param>
        /// <returns></returns>
        public string ShowPendingPopUpMessage(string parentCLOrderID)
        {
            string message = string.Empty;
            try
            {
                var ordSingle = _dictPendingNewAlertCollection.ContainsKey(parentCLOrderID) ? _dictPendingNewAlertCollection[parentCLOrderID] : null;
                if (ordSingle != null && ordSingle.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew)
                {
                    string orderside = TagDatabaseManager.GetInstance.GetOrderSideText(ordSingle.OrderSideTagValue);
                    string tif = TagDatabaseManager.GetInstance.GetTIFText(ordSingle.TIF);
                    string tifText = string.IsNullOrEmpty(tif) ? string.Empty : " " + tif;
                    string ordertype = TagDatabaseManager.GetInstance.GetOrderTypeText(ordSingle.OrderTypeTagValue);
                    if (ordertype == BlotterDataConstants.CONST_LIMIT)
                        message = orderside + BlotterDataConstants.CONST_COMMA_SPACE + ordSingle.Quantity + BlotterDataConstants.CONST_SPACE + ordSingle.Symbol + BlotterDataConstants.CONST_SPACE_AT + ordertype + BlotterDataConstants.MSG_PRICE_COLON + ordSingle.Price + tifText + BlotterDataConstants.MSG_To + ordSingle.CounterPartyName + Environment.NewLine + BlotterDataConstants.MSG_ORDER_NOT_ACK_BY_BROKER + Environment.NewLine + BlotterDataConstants.MSG_CONTACT_SUPPORT_REPRESENTATIVE;
                    else
                        message = orderside + BlotterDataConstants.CONST_COMMA_SPACE + ordSingle.Quantity + BlotterDataConstants.CONST_SPACE + ordSingle.Symbol + BlotterDataConstants.CONST_SPACE_AT + ordertype + BlotterDataConstants.MSG_PRICE + tifText + BlotterDataConstants.MSG_To + ordSingle.CounterPartyName + Environment.NewLine + BlotterDataConstants.MSG_ORDER_NOT_ACK_BY_BROKER + Environment.NewLine + BlotterDataConstants.MSG_CONTACT_SUPPORT_REPRESENTATIVE;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"ShowPendingPopUpMessage encountered an error for parentCLOrderID {parentCLOrderID}");
            }
            return message;
        }
        #endregion

        #region Kafka response handlers

        /// <summary>
        /// Logged In Company Trading pref 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompanyTransferTradeRulesReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    if (!string.IsNullOrEmpty(message.Data))
                    {
                        TradingTicket _userPreference = JsonHelper.DeserializeToObject<TradingTicket>(message.Data);
                        TradingTicketRulesPrefs _tradingTicketRulesPrefs = _userPreference.TradingTicketRulesPrefs;

                        IsPendingNewTradeAlertAllowed = Convert.ToBoolean(_tradingTicketRulesPrefs.IsPendingNewTradeAlert);
                        PendingNewOrderAlertTime = Convert.ToInt32(_tradingTicketRulesPrefs.PendingNewOrderAlertTime);
                    }
                    Logger.LogMsg(LoggerLevel.Information,
                        "CompanyTransferTradeRulesReceived event processed successfully in {0}ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CompanyTransferTradeRulesReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// KafkaManager User LoggedIn Information Details
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserWiseLoggedInInformationReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
                    {
                        if (!string.IsNullOrEmpty(message.Data))
                        {
                            CompanyUser _loggedInInformation = JsonHelper.DeserializeToObject<CompanyUser>(message.Data);
                            if (!_userWiseLoggedInUserInformation.ContainsKey(message.CompanyUserID))
                            {
                                _userWiseLoggedInUserInformation.Add(message.CompanyUserID, _loggedInInformation);
                            }
                        }
                        Logger.LogMsg(LoggerLevel.Information, "UserWiseLoggedInInformationReceived event processed successfully in {0}ms", sw.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UserWiseLoggedInInformationReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Cache logged-in web users information 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserLoggedInInformation(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    if (!_loggedInUserResponseReceivedFromAuth)
                    {
                        _loggedInUserResponseReceivedFromAuth = true;
                        Logger.LogMsg(LoggerLevel.Information, "Successfully connected Auth Service.");
                    }

                    var sw = Stopwatch.StartNew();
                    using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
                    {
                        Dictionary<int, AuthenticatedUserInfo> _loggedInUsers = JsonConvert.DeserializeObject<Dictionary<int, AuthenticatedUserInfo>>(message.Data);
                        foreach (var kvp in _loggedInUsers)
                        {
                            if (_loggedInUsers[kvp.Key] != null)
                            {
                                if (!_dictLoggedInUser.ContainsKey(kvp.Key) && _loggedInUsers[kvp.Key].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                                {
                                    _dictLoggedInUser.Add(kvp.Key, _loggedInUsers[kvp.Key]);
                                    _blotterLayoutManager.LoadLayoutsForLoggedInUser(kvp.Key);
                                }
                            }
                        }
                        if (_loggedInUsers.Count > 0)
                            Logger.LogMsg(LoggerLevel.Information, "UserLoggedInInformation event processed successfully in {0} ms", sw.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UserLoggedInInformation encountered an error");
                }
            }
        }

        /// <summary>
        /// Cache logged-out web users information
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserLoggedOutInformation(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
                    {
                        int companyUserID = message.CompanyUserID;
                        if (_dictLoggedInUser.ContainsKey(companyUserID))
                        {
                            _dictLoggedInUser.Remove(companyUserID);
                        }
                        if (_loggedInUserInformationRquested.ContainsKey(companyUserID))
                        {
                            _loggedInUserInformationRquested.Remove(companyUserID);
                        }
                        Logger.LogMsg(LoggerLevel.Information, "UserLoggedOutInformation event processed successfully in {0} ms", sw.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UserLoggedOutInformation encountered an error");
                }
            }
        }

        /// <summary>
        /// Cache logged-in user's required preference data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserDataForBlotterReceived(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    int companyUserID = message.CompanyUserID;
                    Dictionary<string, string> responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);

                    TradingTicketUIPrefs _tradingTicketUIPrefs = JsonConvert.DeserializeObject<TradingTicketUIPrefs>(
                        responseData[BlotterDataConstants.CONST_TRADINGTICKETUIPREF]);

                    Dictionary<int, string> _userPermittedAccounts = JsonConvert
                        .DeserializeObject<Dictionary<int, string>>(responseData[BlotterDataConstants.CONST_ACCOUNTLIST]);

                    CacheUserDataReceived(companyUserID, _tradingTicketUIPrefs, _userPermittedAccounts);

                    Logger.LogMsg(LoggerLevel.Information, "UserDataForBlotterReceived event processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UserDataForBlotterReceived encountered an error");
                }
            }
        }

        /// <summary>
        /// Cache user data received
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void CacheUserDataReceived(int companyUserID, TradingTicketUIPrefs tradingTicketUIPrefs, Dictionary<int, string> userPermittedAccounts)
        {
            try
            {
                if (tradingTicketUIPrefs != null)
                {
                    if (_tradeManager.UserwiseTradingTicketUIPrefs.ContainsKey(companyUserID))
                    {
                        _tradeManager.UserwiseTradingTicketUIPrefs.Remove(companyUserID);
                        _tradeManager.UserwiseTradingTicketUIPrefs.Add(companyUserID, tradingTicketUIPrefs);
                    }
                    else
                    {
                        _tradeManager.UserwiseTradingTicketUIPrefs.Add(companyUserID, tradingTicketUIPrefs);
                    }
                }
                if (userPermittedAccounts != null)
                {
                    if (_tradeManager.UserPermittedAccounts.ContainsKey(companyUserID))
                    {
                        _tradeManager.UserPermittedAccounts.Remove(companyUserID);
                        _tradeManager.UserPermittedAccounts.Add(companyUserID, userPermittedAccounts);
                    }
                    else
                    {
                        _tradeManager.UserPermittedAccounts.Add(companyUserID, userPermittedAccounts);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CacheUserDataReceived encountered an error");
            }
        }

        /// <summary>
        /// Configures the allocation group filters for both allocated and unallocated dictionaries.
        /// This ensures only relevant group-based filtering is applied during allocation operations.
        /// </summary>
        /// <param name="allocated">Dictionary to apply group filter for allocated orders.</param>
        /// <param name="unallocated">Dictionary to apply group filter for unallocated orders.</param>
        /// <param name="groupIds">List of group IDs to be joined and assigned to the filters.</param>
        private void SetAllocationGroupFilter(Dictionary<string, string> allocated, Dictionary<string, string> unallocated, List<string> groupIds)
        {
            string groupIdValue = string.Join(",", groupIds);

            allocated["GroupID"] = groupIdValue;
            unallocated["GroupID"] = groupIdValue;

            if (unallocated.ContainsKey("AccountID"))
                unallocated.Remove("AccountID");
        }

        /// <summary>
        /// Bind the message data to the AlgoStrategyNamesInfo dictionary 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_AllAlgoStrategiesInfoResponse(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<string, string> responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);
                AlgoStrategyNamesDetails.AlgoStrategyNamesInfo = responseData;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "AllAlgoStrategiesInfoResponse encountered an error");
            }
        }

        /// <summary>
        /// KafkaManager_BlotterGetDataRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterGetDataRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    dynamic info = JsonConvert.DeserializeObject<dynamic>(message.Data);
                    Logger.LogMsg(LoggerLevel.Information, "KafkaManager_BlotterGetDataRequest is invoked.");
                    bool getCompanyTransferTradeRules = Convert.ToBoolean(info.isComTransTradeRulesRequired);
                    BlotterResponse blotterResponse = _tradeManager.GetTradesFromDatabase(message.CompanyUserID);
                    if (blotterResponse != null)
                    {
                        var data = new { blotterData = blotterResponse, blotterId = info.blotterId };
                        message.Data = JsonConvert.SerializeObject(data);
                        await KafkaManager.Instance.Produce(TOPIC_BlotterGetDataResponse, message);
                    }
                    Logger.LogMsg(LoggerLevel.Information, "Blotter get data request processed successfully in {0} ms with total of {1} orderTabData",
                        sw.ElapsedMilliseconds, blotterResponse?.OrderTabData?.Count);

                    #region Request required data once for company user
                    _ = RequestStartupDataForUser(message.CompanyUserID, getCompanyTransferTradeRules);
                    #endregion
                }
                catch (Exception ex)
                {
                    message.IsDisplayError = true;
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterGetDataResponse);
                }
            }
        }
        /// <summary>
        /// KafkaManager_BlotterGetPSTPreferenceDetailsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterGetPSTPreferenceDetailsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    PSTRequestDTO pstRequestDTO = JsonConvert.DeserializeObject<PSTRequestDTO>(message.Data);

                    PSTResponseDTO pstResponseDTO = _tradeManager.GetPSTPreferenceDetails(pstRequestDTO);
                    if (pstResponseDTO != null)
                    {
                        var data = new { pstData = pstResponseDTO };
                        message.Data = JsonConvert.SerializeObject(data);
                        await KafkaManager.Instance.Produce(TOPIC_BlotterGetPSTPreferenceDetailsResponse, message);
                    }
                    Logger.LogMsg(LoggerLevel.Information, "Blotter GetPSTPreferenceDetails request processed successfully in {0} ms with pstRequestObject- {1}",
                        sw.ElapsedMilliseconds, pstResponseDTO.pttRequestObject);
                }
                catch (Exception ex)
                {
                    message.IsDisplayError = true;
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterGetPSTPreferenceDetailsResponse);
                }
            }
        }

        /// <summary>
        /// Requests startup data for company user
        /// </summary>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task RequestStartupDataForUser(int companyUserID, bool getCompanyTransferTradeRules)
        {
            using (LoggerHelper.PushUserPropInLogging(companyUserID))
            {
                try
                {
                    if (getCompanyTransferTradeRules)
                    {
                        await KafkaManager.Instance.Produce(TOPIC_CompanyTransferTradeRulesRequest,
                            new RequestResponseModel(companyUserID, "", null));

                        var UserWiseTradingAccounts = _tradeManager.GetTradingAccountsByUserID(companyUserID);

                        if (UserWiseTradingAccounts != null)
                        {
                            await KafkaManager.Instance.Produce(TOPIC_UserWiseTradingAccountsRequest,
                            new RequestResponseModel(companyUserID, JsonHelper.SerializeObject(UserWiseTradingAccounts)));
                        }
                        else
                        {
                            Logger.LogMsg(LoggerLevel.Information, "Issue with Trading Account request for User: " + companyUserID.ToString());
                        }
                    }
                    if (_loggedInUserInformationRquested != null && !_loggedInUserInformationRquested.ContainsKey(companyUserID))
                    {
                        _ = KafkaManager.Instance.Produce(TOPIC_UserDataForBlotterRequest,
                            new RequestResponseModel(companyUserID, "", null));

                        _loggedInUserInformationRquested.Add(companyUserID, true);
                        Logger.LogMsg(LoggerLevel.Information, "User cached request proccesed successully");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "RequestStartupDataForUser encountered an error");
                }
            }
        }

        #region Blotter operations
        /// <summary>
        /// KafkaManager_BlotterCancelAllSubsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterCancelAllSubsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string commaSaperateParentClOrderId = message.Data;
                    string errorMessage = _tradeManager.CancelAllSubOrders(message.CompanyUserID, commaSaperateParentClOrderId);

                    var response = new { errorMessage = errorMessage };
                    message.Data = JsonConvert.SerializeObject(response);

                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BlotterCancelAllSubsResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "BlotterCancelAllSubsRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterCancelAllSubsResponse);
                }
            }
        }
        /// <summary>
        /// KafkaManager_TransferUser processes the transfer of users and interacts with Kafka.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_TransferUser(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(message.Data);

                    // Access the data from the dictionary
                    List<string> orderIDs = JsonConvert.DeserializeObject<List<string>>(data["orderIds"].ToString());
                    bool isSendSubOrder = Convert.ToBoolean(data["includeSubOrders"]);
                    bool isOrderTab = Convert.ToBoolean(data["isOrderTab"]);
                    int transferUserId = Convert.ToInt32(data["targetUserId"]?.ToString());
                    bool isAllowUserToTansferTrade = Convert.ToBoolean(data["isAllowUserToTansferTrade"]);
                    string uniqueIdentifier = data["uniqueIdentifier"]?.ToString();

                    var result = _tradeManager.TransferUser(message.CompanyUserID, orderIDs, transferUserId, isOrderTab, isSendSubOrder, isAllowUserToTansferTrade);

                    if (result != null)
                    {
                        string successfullTransferOrderCount = result["successMessage"];
                        if (!string.IsNullOrEmpty(successfullTransferOrderCount))
                        {
                            //section to notify transffered user
                            if (message.CompanyUserID != transferUserId)
                            {
                                string TransferredByUsername = CachedDataManager.GetInstance.GetUserText(message.CompanyUserID);
                                string TransferredMessage = Convert.ToInt32(successfullTransferOrderCount) > 1 ? TransferredByUsername + " has transferred " + successfullTransferOrderCount + " orders to you" : TransferredByUsername + " has transferred an order to you";
                                await KafkaManager.Instance.Produce(TOPIC_TransferUserFromRequest, new RequestResponseModel(transferUserId, TransferredMessage));
                            }
                            result["successMessage"] = successfullTransferOrderCount + " order(s) transferred to user: " + CachedDataManager.GetInstance.GetUserText(transferUserId);
                        }
                        result["uniqueIdentifier"] = uniqueIdentifier;
                        message.Data = JsonHelper.SerializeObject(result);
                    }
                    else
                        Logger.LogMsg(LoggerLevel.Information, "Issue faced while transferring Orders.");

                    await KafkaManager.Instance.Produce(TOPIC_TransferUserResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "BlotterTransferUser request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_TransferUserResponse);
                }
            }
        }

        /// <summary>
        /// Shared helper to encapsulate the logic of resolving a parentClOrderId into its AllocationGroup,
        /// including fetching groupId, tradeDate, constructing the AllocationPrefetchFilter, and calling
        /// GetGroupedOrderDetails.
        /// </summary>
        private AllocationGroupDetails GetFirstAllocationGroup(string parentClOrderId, string topic, RequestResponseModel message)
        {
            var details = BlotterCacheManager.GetInstance().GetGroupIdDateByClOrderID(parentClOrderId);
            if (details.Rows.Count == 0 || details.Rows[0][OrderFields.PROPERTY_AUECLOCALDATE] == DBNull.Value)
            {
                Logger.LogMsg(LoggerLevel.Verbose,
                    "GroupIdDate not found or invalid for parentClOrderId '{ParentClOrderId}'. Topic: {Topic}",
                    parentClOrderId, topic);
                return null;
            }

            string groupId = details.Rows[0][OrderFields.CAPTION_GROUPID].ToString();
            DateTime tradeDate = Convert.ToDateTime(details.Rows[0][OrderFields.PROPERTY_AUECLOCALDATE]).Date;

            var allocationGroupFilter = new AllocationPrefetchFilter();
            SetAllocationGroupFilter(allocationGroupFilter.Allocated, allocationGroupFilter.Unallocated, new List<string> { groupId });

            var selectedGroups = AllocationClientServiceConnector.Allocation.InnerChannel
                .GetGroupedOrderDetails(tradeDate, tradeDate, allocationGroupFilter, true);

            if (selectedGroups == null || !selectedGroups.Any())
            {
                Logger.LogMsg(LoggerLevel.Verbose,
                    "GroupedOrderDetails not found for GroupId '{GroupId}' on TradeDate '{TradeDate}'. Topic: {Topic}, CompanyUserID: {CompanyUserID}, CorrelationID: {CorrelationID}",
                    groupId, tradeDate, topic, message.CompanyUserID, message.CorrelationId);
                return null;
            }

            return new AllocationGroupDetails
            {
                Group = selectedGroups.First(),
                GroupId = groupId,
                TradeDate = tradeDate
            };
        }

        /// <summary>
        /// Handles Kafka request for retrieving order details required to edit trade attributes.
        /// </summary>
        /// <param name="topic">The Kafka topic name where the request was received.</param>
        /// <param name="message">The message containing correlation and company user info, and the ParentClOrderId in Data.</param>
        private async void KafkaManager_GetOrderDetailsForEditTradeAttributesRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    string parentClOrderId = message.Data;

                    Logger.LogMsg(LoggerLevel.Information,
                        "GetOrderDetailsForEditTradeAttributes request received. Topic: {Topic}",
                        topic);

                    var allocationDetails = GetFirstAllocationGroup(parentClOrderId, topic, message);
                    if (allocationDetails == null)
                    {
                        Logger.LogMsg(LoggerLevel.Information,
                        "GroupDetails is null or empty. Topic: {Topic}", topic);
                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_GetOrderDetailsForEditTradeAttributesResponse, new RequestResponseModel
                        {
                            ErrorMsg = "GroupDetails is null or empty",
                            CorrelationId = message.CorrelationId
                        });
                        return;
                    }

                    var groupObj = allocationDetails.Group;

                    // Fetch Broker
                    string brokerName = CommonDataCache.CachedDataManager.GetInstance.GetCounterPartyText(groupObj.CounterPartyID);

                    // Convert OrderSide if B
                    var orderSideValue = groupObj.OrderSideTagValue?.ToString();
                    var response = new EditTradeAttributesResponseDto
                    {
                        TradeDetails = new TradeDetailsDto
                        {
                            Symbol = groupObj.Symbol,
                            OrderSide = orderSideValue == "B" ? "10" : orderSideValue,
                            Broker = brokerName,
                            TotalQuantity = groupObj.Quantity
                        },
                        GroupOrdersDetails = groupObj
                    };

                    message.Data = JsonConvert.SerializeObject(response);

                    Logger.LogMsg(LoggerLevel.Information,
                        "GetOrderDetailsForEditTradeAttributes response sent. Topic: {Topic}",
                        TOPIC_GetOrderDetailsForEditTradeAttributesResponse);

                    await KafkaManager.Instance.Produce(TOPIC_GetOrderDetailsForEditTradeAttributesResponse, message);
                }
                catch (Exception ex)
                {
                    Logger.LogMsg(LoggerLevel.Error,
                        "GetOrderDetailsForEditTradeAttributes exception occurred. Topic: {Topic}, Error: {ErrorMessage}",
                        topic, ex.Message);

                    await ProduceTopicNHandleException(message, ex, TOPIC_GetOrderDetailsForEditTradeAttributesResponse);
                }
            }
        }

        /// <summary>
        /// Handles the SaveEditedTradeAttributesRequest Kafka message.
        /// Deserializes the incoming allocation group data, cleans invalid values,
        /// saves the updated trade attributes via the Allocation service, 
        /// and produces a response message back to Kafka.
        /// </summary>
        /// <param name="topic">The Kafka topic from which the message was received.</param>
        /// <param name="message">The request message containing correlation info and allocation data.</param>
        private async void KafkaManager_SaveEditedTradeAttributesRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    EditTradeAttributesPayload payload = JsonConvert.DeserializeObject<EditTradeAttributesPayload>(message.Data);
                    Logger.LogMsg(LoggerLevel.Information,
                        "SaveEditedTradeAttributes request received. Topic: {Topic}", topic);

                    var allocationDetails = GetFirstAllocationGroup(payload.ParentClOrderId, topic, message);
                    if (allocationDetails == null)
                    {
                        Logger.LogMsg(LoggerLevel.Information,
                        "GroupDetails is null or empty. Topic: {Topic}", topic);
                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, new RequestResponseModel
                        {
                            ErrorMsg = "GroupDetails is null or empty",
                            CorrelationId = message.CorrelationId
                        });
                        return;
                    }

                    var groupObj = allocationDetails.Group;

                    // Apply to group-level attributes
                    ApplyTradeAttributeEdits(groupObj, payload.EditedTradeAttributes);

                    // Update persistence status
                    var persistenceProp = groupObj.GetType().GetProperty("PersistenceStatus");
                    if (persistenceProp != null && persistenceProp.CanWrite)
                    {
                        persistenceProp.SetValue(groupObj, ApplicationConstants.PersistenceStatus.ReAllocated);
                    }

                    // For Unallocated Trades - First clear the dict and then assign.
                    if (Enum.TryParse<PostTradeConstants.ORDERSTATE_ALLOCATION>(groupObj.StateID.ToString(), out var allocationState))
                    {
                        if ((PostTradeConstants.ORDERSTATE_ALLOCATION)allocationState == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                        {
                            groupObj.ClearTaxlotDictionary();
                            AllocationGroupingHelper.UpdateTaxlotDetails(groupObj, ApplicationConstants.TaxLotState.Updated);
                        }
                    }

                    // Apply to each taxlot - For Allocated Trades
                    foreach (var taxlot in groupObj.TaxLots)
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                        ApplyTradeAttributeEdits(taxlot, payload.EditedTradeAttributes);
                        taxlot.Level2Name = CachedDataManager.GetInstance.GetStrategyText(taxlot.Level2ID);
                        groupObj.AddOrUpdateTaxLot(taxlot.TaxLotID, taxlot);
                    }

                    // Save groups
                    List<AllocationGroup> groupData = new List<AllocationGroup> { groupObj };
                    var savedGroup = AllocationClientServiceConnector.Allocation.InnerChannel.SaveGroups(groupData, message.CompanyUserID);
                    var response = new RequestResponseModel
                    {
                        CorrelationId = message.CorrelationId,
                        RequestID = message.RequestID,
                        CompanyUserID = message.CompanyUserID,
                        Data = JsonConvert.SerializeObject(savedGroup)
                    };

                    Logger.LogMsg(LoggerLevel.Information,
                        "SaveEditedTradeAttributes response sent. Topic: {Topic}",
                        TOPIC_SaveAllocationDetailsRequest);

                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveEditedTradeAttributesResponse, response);
                }
                catch (Exception ex)
                {
                    Logger.LogMsg(LoggerLevel.Error,
                        "SaveEditedTradeAttributes exception occurred. Topic: {Topic}, CompanyUserID: {CompanyUserID}, CorrelationID: {CorrelationID}, Error: {ErrorMessage}",
                        topic, message.CompanyUserID, message.CorrelationId, ex.Message);

                    await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SaveEditedTradeAttributesRequest);
                }
            }
        }

        /// <summary>
        /// Applies edited trade attribute values to the given target object.
        /// The keys in the dictionary must match property names (e.g., "TradeAttribute1").
        /// </summary>
        private void ApplyTradeAttributeEdits(dynamic target, Dictionary<string, string> edits)
        {
            if (target == null || edits == null) return;

            var type = target.GetType();
            foreach (var kv in edits)
            {
                var prop = type.GetProperty(kv.Key);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(target, kv.Value);
                }
            }
        }

        /// <summary>
        /// KafkaManager_BlotterRemoveOrdersRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterRemoveOrdersRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string commaSaperateParentClOrderId = message.Data;
                    string errorMessage = _tradeManager.RemoveOrders(message.CompanyUserID, commaSaperateParentClOrderId);

                    var response = new { errorMessage = errorMessage };
                    message.Data = JsonConvert.SerializeObject(response);

                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BlotterRemoveOrdersResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "BlotterRemoveOrdersRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterRemoveOrdersResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_FreezePendingComplianceRows
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_FreezePendingComplianceRows(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string commaSaperateParentClOrderId = message.Data;
                    _tradeManager.FreezePendingComplianceRows(message.CompanyUserID, commaSaperateParentClOrderId);

                    message.Data = JsonConvert.SerializeObject("");
                    await KafkaManager.Instance.Produce(TOPIC_FreezePendingComplianceRowsResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "FreezePendingComplianceRows request processed successfully in {0} ms",
                      sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_FreezePendingComplianceRowsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_UnfreezePendingComplianceRows
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_UnfreezePendingComplianceRows(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string commaSaperateParentClOrderId = message.Data;
                    _tradeManager.UnfreezePendingComplianceRows(message.CompanyUserID, commaSaperateParentClOrderId);

                    message.Data = JsonConvert.SerializeObject("");
                    await KafkaManager.Instance.Produce(TOPIC_UnfreezePendingComplianceRowsResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "UnfreezePendingComplianceRows request processed successfully in {0} ms",
                      sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_UnfreezePendingComplianceRowsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_BlotterRemoveManualExecutionRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterRemoveManualExecutionRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string parentClOrderId = message.Data;
                    string errorMessage = _tradeManager.RemoveManualExecution(message.CompanyUserID, parentClOrderId);

                    var response = new { errorMessage = errorMessage };
                    message.Data = JsonConvert.SerializeObject(response);

                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BlotterRemoveManualExecutionResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "BlotterRemoveManualExecutionRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterRemoveManualExecutionResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_BlotterGetManualFillsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterGetManualFillsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
                    {
                        string parentClOrderId = message.Data;
                        List<BlotterOrder> response = _tradeManager.BlotterGetManualFills(message.CompanyUserID, parentClOrderId);

                        message.Data = JsonConvert.SerializeObject(response);
                        await KafkaManager.Instance.Produce(TOPIC_BlotterGetManualFillsResponse, message);

                        Logger.LogMsg(LoggerLevel.Information, "BlotterGetManualFills request processed successfully in {0} ms",
                          sw.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterGetManualFillsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_BlotterSaveManualFillsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterSaveManualFillsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
                    {
                        List<SaveManualFillsDetails> fillsData = JsonConvert.DeserializeObject<List<SaveManualFillsDetails>>(message.Data);
                        string errorMsg = _tradeManager.SaveManualFills(message.CompanyUserID, fillsData);

                        var response = new { errorMessage = errorMsg };
                        message.Data = JsonConvert.SerializeObject(response);

                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BlotterSaveManualFillsResponse, message);
                        Logger.LogMsg(LoggerLevel.Information, "BlotterSaveManualFillsRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterSaveManualFillsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_GetAllocationDetailsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_GetAllocationDetailsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string parentClOrderId = message.Data;
                    TradeManager.AllocationDetailsInformation detailedInfo = _tradeManager.GetAllocationDetails(message.CompanyUserID, parentClOrderId);

                    message.Data = JsonConvert.SerializeObject(detailedInfo);
                    await KafkaManager.Instance.Produce(TOPIC_GetAllocationDetailsResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "GetAllocationDetailsRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_GetAllocationDetailsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_SaveAllocationDetailsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveAllocationDetailsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string errorMsg = string.Empty;
                    Dictionary<string, string> responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);

                    if (responseData != null && responseData.Count > 0)
                    {
                        int allocationPrefValue = Convert.ToInt32(responseData[BlotterDataConstants.CONST_ALLOCATION_PREFERENCE]);
                        string orderGroupId = responseData[OrderFields.CAPTION_GROUPID].ToString();
                        string parentClOrderId = responseData[OrderFields.PROPERTY_PARENT_CL_ORDERID].ToString();
                        bool isGroupedOrder = Convert.ToBoolean(responseData[BlotterDataConstants.CONST_IS_GROUPED_ORDER].ToString());
                        List<SaveEasyAllocateDetails> allocationDetails = JsonConvert.DeserializeObject<List<SaveEasyAllocateDetails>>(responseData[BlotterDataConstants.CONST_ALLOCATED_DATA]);
                        errorMsg = _tradeManager.SaveAllocationDetails(message.CompanyUserID, allocationDetails, orderGroupId, parentClOrderId, allocationPrefValue, isGroupedOrder);
                    }
                    else
                        errorMsg = BlotterDataConstants.MSG_ErrorSavingAllocationDetails;

                    var response = new { errorMessage = errorMsg };
                    message.Data = JsonConvert.SerializeObject(response);

                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveAllocationDetailsResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "SaveAllocationDetailsRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_SaveAllocationDetailsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_BlotterRolloverAllSubsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_BlotterRolloverAllSubsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string commaSaperateParentClOrderId = message.Data;
                    string errorMessage = _tradeManager.RolloverAllSubOrders(message.CompanyUserID, commaSaperateParentClOrderId);

                    var response = new { errorMessage = errorMessage };
                    message.Data = JsonConvert.SerializeObject(response);

                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BlotterRolloverAllSubsResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "BlotterRolloverAllSubsRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_BlotterRolloverAllSubsResponse);
                }
            }
        }

        /// <summary>
        /// KafkaManager_GetPstAllocationDetailsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_GetPstAllocationDetailsRequest(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    string data = message.Data;
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    string[] parameters = Array.ConvertAll(dictionary.Values.ToArray(), value => value.ToString());
                    var msgData = _tradeManager.GetPstAllocationDetails(parameters[0], parameters[1], parameters[2]);
                    message.Data = JsonConvert.SerializeObject(new { msgData, Identifier = parameters[4] });
                    await KafkaManager.Instance.Produce(TOPIC_GetPstAllocationDetailsResponse, message);
                    Logger.LogMsg(LoggerLevel.Information, "GetPstAllocationDetailsRequest request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, TOPIC_GetPstAllocationDetailsResponse);
                }
            }
        }
        private static async System.Threading.Tasks.Task ProduceTopicNHandleException(
            RequestResponseModel message,
            Exception ex,
            string topicName)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
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
                    Logger.LogError(ex2, $"ProduceTopicNHandleException encountered an error, message might not have been published to event {topicName}");
                }
            }
        }
        #endregion

        #endregion

        #region Kafka reporter
        /// <summary>
        /// Kafka_ProducerReporter
        /// </summary>
        /// <param name="topic"></param>
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

        /// <summary>
        /// Kafka_ConsumerReporter
        /// </summary>
        /// <param name="topic"></param>
        private void Kafka_ConsumerReporter(string topic)
        {
            try
            {
                Logger.LogMsg(LoggerLevel.Verbose, "{0}" + KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

        #region Publish order(s) updates for web application.
        /// <summary>
        /// Sends orders to Service gateway.
        /// </summary>
        /// <param name="updatedOrders"></param>
        private void SendBlotterData(Dictionary<string, List<OrderSingle>> updatedOrders)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                BlotterResponse blotterResponse = null;
                List<BlotterOrder> listOfOrders = new List<BlotterOrder>();
                List<BlotterOrder> listOfSubOrders = new List<BlotterOrder>();
                foreach (var item in updatedOrders)
                {
                    foreach (OrderSingle order in item.Value)
                    {
                        if (!string.IsNullOrEmpty(order.OrderID) || order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PartiallyFilled) || order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingNew) || order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_Cancelled) || order.OrderStatusTagValue.Equals(FIXConstants.ORDSTATUS_PendingReplace))
                        {
                            bool orderInPendingApprovalCache = BlotterOrderCollections.GetInstance().IsOrderInPendingApprovalCache(order.ClOrderID);
                            BlotterOrder blotterData = new BlotterOrder();
                            blotterData.UpdateDataFromOrderSingle(order, true, orderInPendingApprovalCache);
                            if (item.Key.Equals(BlotterDataConstants.CONST_OrderTab))
                            {
                                List<BlotterOrder> subOrdersOfCurrentOrder = new List<BlotterOrder>();
                                if (order != null && order.OrderCollection != null)
                                {
                                    foreach (OrderSingle subOrder in order.OrderCollection)
                                    {
                                        BlotterOrder subOrderBlotter = new BlotterOrder();
                                        subOrderBlotter.UpdateDataFromOrderSingle(subOrder, true);
                                        subOrdersOfCurrentOrder.Add(subOrderBlotter);
                                    }
                                }
                                blotterData.OrderCollection = subOrdersOfCurrentOrder;
                                listOfOrders.Add(blotterData);
                            }
                            else
                                listOfSubOrders.Add(blotterData);
                        }
                    }
                }
                blotterResponse = new BlotterResponse(BlotterRequestType.PublishOrder, listOfOrders, listOfSubOrders);

                UpdateWorkingTabOrderAllocation(blotterResponse);

                if (blotterResponse.OrderTabData.Count > 0 || blotterResponse.WorkingTabData.Count > 0)
                {
                    RequestResponseModel response = new RequestResponseModel(0, JsonConvert.SerializeObject(blotterResponse), null);

                    Logger.LogMsg(LoggerLevel.Debug,
                        "SendBlotterData request processed successfully in {0} ms, with grid count {1}, working tab count {2}",
                        sw.ElapsedMilliseconds, blotterResponse.OrderTabData.Count, blotterResponse.WorkingTabData.Count);

                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_BlotterUpdatedData, response);
                }

                listOfOrders.Clear();
                listOfSubOrders.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestStartupDataForUser encountered an error");
            }
        }

        /// <summary>
        /// Update Working Tab Order Allocation based on Order Tab sub-order allocation
        /// </summary>
        private void UpdateWorkingTabOrderAllocation(BlotterResponse blotterResponse)
        {
            try
            {
                foreach (BlotterOrder workingTabOrder in blotterResponse.WorkingTabData)
                {
                    if (!(workingTabOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_PendingNew || workingTabOrder.OrderStatusTagValue == FIXConstants.ORDSTATUS_Rejected))
                    {
                        foreach (BlotterOrder orderTabData in blotterResponse.OrderTabData)
                        {
                            if (workingTabOrder.StagedOrderID.Equals(orderTabData.ClOrderID))
                            {
                                foreach (BlotterOrder subOrder in orderTabData.OrderCollection)
                                {
                                    if (workingTabOrder.ClOrderID.Equals(subOrder.ClOrderID))
                                    {
                                        workingTabOrder.Account = subOrder.Account;
                                        workingTabOrder.AllocationSchemeName = subOrder.AllocationSchemeName;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "UpdateWorkingTabOrderAllocation encountered an error");
            }
        }

        /// <summary>
        /// Sends removed orders information to service gateway.
        /// </summary>
        /// <param name="ordersRemoved"></param>
        /// <param name="blotterRequestType"></param>
        private async void SendRemoveOrdersInfo(Dictionary<string, OrderSingle> ordersRemoved,
            BlotterRequestType blotterRequestType)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                if (ordersRemoved != null && ordersRemoved.Count > 0)
                {
                    BlotterResponse blotterResponse = new BlotterResponse(blotterRequestType, null, null, String.Join(",", ordersRemoved.Keys));
                    RequestResponseModel response = new RequestResponseModel(0, JsonConvert.SerializeObject(blotterResponse), null);
                    InformationReporter.GetInstance.Write(string.Format(BlotterDataConstants.MSG_RemoveDataOnBlotterForAllUserAndCount, ordersRemoved.Keys.Count, blotterRequestType));
                    await KafkaManager.Instance.Produce(TOPIC_BlotterRemovedData, response);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        #region Communication manager
        /// <summary>
        /// Trade server connection properties.
        /// </summary>
        /// <returns></returns>
        private ConnectionProperties GetTradeServerConnectionDetails()
        {
            ConnectionProperties connProperties = new ConnectionProperties();
            try
            {
                connProperties.Port = ClientAppConfiguration.TradeServer.Port;
                connProperties.ServerIPAddress = ClientAppConfiguration.TradeServer.IpAddress;
                connProperties.User = null;
                connProperties.IdentifierID = "BlotterDataService_SocketConn";
                connProperties.IdentifierName = "Blotter Data Service SocketConn";
                connProperties.ConnectedServerName = "Trade ";
                connProperties.HandlerType = HandlerType.TradeHandler;
                Logger.LogMsg(LoggerLevel.Information, "Trade Server connection details set successfully with port:{0}", connProperties.Port);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetTradeServerConnectionDetails encountered an error");
            }
            return connProperties;
        }

        /// <summary>
        /// For connectivity with Trade server.
        /// </summary>
        ICommunicationManager _tradeCommunicationManager;
        private void ConnectToAllSockets()
        {
            try
            {
                InformationReporter.GetInstance.Write(BlotterDataConstants.MSG_ConnectToAllSocketsRequest);
                _tradeCommunicationManager = new ClientTradeCommManager();
                _tradeCommunicationManager.Disconnected += new EventHandler(CommunicationManager_Disconnected);
                _tradeCommunicationManager.Connected += new EventHandler(CommunicationManager_Connected);
                Logger.LogMsg(LoggerLevel.Information, BlotterDataConstants.MSG_ConnectToAllSocketsResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ConnectToAllSockets encountered an error");
            }
        }

        private void CommunicationManager_Connected(object sender, EventArgs e)
        {
        }

        private void CommunicationManager_Disconnected(object sender, EventArgs e)
        {
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

        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                Console.WriteLine(e.Value);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
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
            await System.Threading.Tasks.Task.CompletedTask;
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

        public System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus()
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task SetDebugModeStatus(bool isDebugModeEnabled)
        {
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async System.Threading.Tasks.Task<bool> GetDebugModeStatus()
        {
            await System.Threading.Tasks.Task.CompletedTask;

            return false;
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
                    if (_tradeSubscriptionProxy != null)
                    {
                        _tradeSubscriptionProxy.Dispose();
                    }
                    if (_blotterUpdateTimer != null)
                    {
                        _blotterUpdateTimer.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        public string getReceiverUniqueName()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
