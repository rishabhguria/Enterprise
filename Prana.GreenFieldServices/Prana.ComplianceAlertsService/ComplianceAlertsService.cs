using Castle.Windsor;
using Newtonsoft.Json;
using Prana.AmqpAdapter.Amqp;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.KafkaWrapper;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prana.SocketCommunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static Prana.ComplianceAlertsService.ComplianceAlertsConstants;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.WCFConnectionMgr;
using Prana.GreenfieldServices.Common;

namespace Prana.ComplianceAlertsService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ComplianceAlertsService : BaseService, IComplianceAlertsService, IDisposable
    {
        #region Variables

        private IWindsorContainer _container;

        /// <summary>
        /// _companyID
        /// </summary>
        private int _companyID;
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        /// <summary>
        /// Stores information of Users logged into Web application.
        /// </summary>
        private static Dictionary<int, object> _dictLoggedInUser = new Dictionary<int, object>();

        /// <summary>
        /// Cache to store Compliance permissions data received from Common data service.
        /// </summary>
        Dictionary<string, string> _compliancePermissionsData = new Dictionary<string, string>();

        /// <summary>
        /// Company pre trade Compliance permission
        /// </summary>
        Dictionary<int, bool> _preTradeEnabledCompany = new Dictionary<int, bool>();

        /// <summary>
        ///  Company post trade Compliance permission
        /// </summary>
        Dictionary<int, bool> _postTradeEnabledCompany = new Dictionary<int, bool>();

        /// <summary>
        /// User wise pre trade Compliance check permission
        /// </summary>
        Dictionary<int, bool> _preTradeModuleUsers = new Dictionary<int, bool>();

        /// <summary>
        /// User wise post trade Compliance check permission
        /// </summary>
        Dictionary<int, bool> _postTradeModuleUsers = new Dictionary<int, bool>();

        /// <summary>
        /// Stores boolean value to check service started.
        /// true if service started completely else false
        /// </summary>
        bool _startComplianceAlertService = false;

        private int _cleanedUp = 0;

        /// <summary>
        /// Trade Service Heartbeat manager instance
        /// </summary>
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;

        private string _navAndStartingPositionOfAcntResp = "_NavAndStartingPositionOfAccountsResponse";

        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

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
                #endregion

                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                WindsorContainerManager.Container = container;
                CompanyID = CachedDataManager.GetInstance.GetCompanyID();
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                #region Topic to fetch Compliance permission
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompliancePermissionsResponse, KafkaManager_CompliancePermissionsResponse);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasket, KafkaManager_GetAccountNavNStartingValueFromBasketRequest);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CheckComplianceFromBasket, KafkaManager_CheckComplianceRequest);

                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompliancePermissionsRequest, new RequestResponseModel(0, ""));
                #endregion

                #region Waiting for response from Common data greenfield service
                int totalSleepCycle = 0;
                while (!_startComplianceAlertService)
                {
                    Console.WriteLine(ComplianceAlertsConstants.CONST_WAITING_FOR_COMMON_DATA_SERVICE);
                    Thread.Sleep(3000);
                    totalSleepCycle += 1;
                    if (totalSleepCycle % 3 == 0)
                        _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompliancePermissionsRequest, new RequestResponseModel(0, ""));
                }
                #endregion

                #region Initialize service if Company has Compliance permission
                if ((_preTradeEnabledCompany.ContainsKey(CompanyID) && _preTradeEnabledCompany[CompanyID]) || (_postTradeEnabledCompany.ContainsKey(CompanyID) && _postTradeEnabledCompany[CompanyID]))
                {
                    ConnectToAllSockets();
                    _tradeCommunicationManager.Connect(GetTradeServerConnectionDetails());

                    #region Kafka subscribe and consume
                    //Topic for logged-in user(s) information
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_UserLoggedInInformation);
                    //Topic to send user response for Manual and Live Order to Trade server
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendComplianceDataResponse, KafkaManager_SendOverrideResponse);
                    ////Topic to send alerts to user for Stage Orders
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendStageOrderForComplianceAlerts, KafkaManager_SendStageOrderForComplianceAlertsRequest);
                    //Topic to send user response for Stage Order to Trade server
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SendComplianceDataResponseForStage, KafkaManager_SendOverrideResponseForStage);


                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_InitializeLoggedInUserRequest, new RequestResponseModel(0, ""));
                    #endregion

                    #region AmqpPluginManager Initialize and Events wire
                    AmqpPluginManager.GetInstance().InitializeOnce();
                    AmqpPluginManager.GetInstance().OverrideRequestReceived += new OverrideRequestReceivedHandler(OverrideRequestReceived);

                    InformationReporter.GetInstance.Write(ComplianceAlertsConstants.MSG_CONST_COMPLIANCE_ALERTS_SERVICE_STARTED);
                    #endregion
                }
                else
                {
                    InformationReporter.GetInstance.Write(ComplianceAlertsConstants.MSG_CONST_COMPANY_HAS_NO_COMPLIANCE_PERMISSION);
                }
                #endregion

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");
                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                   "InitialiseService Completed for Compliance alert Service in {0} ms.", sw.ElapsedMilliseconds);
                //AmqpPluginManager.GetInstance().Initialise(1.ToString());
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
            try
            {
                // Perform any last minute clean here.
                // Note: Please add light functions only.
                if (Interlocked.Exchange(ref _cleanedUp, 1) == 1) return;

                // 1) Stop periodic callbacks BEFORE disposing anything they use
                StopServiceHealthPollingTimer();

                // 2) Mark down (and publish a final “down”)
                UpdateServiceStatus(ServiceNameConstants.CONST_ComplianceAlerts_Name, ServiceNameConstants.CONST_ComplianceAlerts_DisplayName, false);

                Console.WriteLine("Shutting down Service...");
                Logger.LogMsg(LoggerLevel.Information, "Shutting down Service...");

                _container.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CleanUp encountered an error");
            }
        }
        #endregion

        #region Kafka response handlers

        /// <summary>
        /// Cache Compliance permissions for Company and Users
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompliancePermissionsResponse(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    InformationReporter.GetInstance.Write(ComplianceAlertsConstants.CONST_COMPLIANCE_PERMISSION_RECEIVED);
                    _compliancePermissionsData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data);
                    ClearAllCache();
                    _preTradeEnabledCompany = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_PRE_TRADE_COMPANY]);
                    _postTradeEnabledCompany = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_POST_TRADE_COMPANY]);
                    _preTradeModuleUsers = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_PRE_TRADE_USERS]);
                    _postTradeModuleUsers = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_POST_TRADE_USERS]);

                    if (_startComplianceAlertService && _preTradeEnabledCompany.ContainsKey(CompanyID) && !_preTradeEnabledCompany[CompanyID] && _postTradeEnabledCompany.ContainsKey(CompanyID) && !_postTradeEnabledCompany[CompanyID])
                    {
                        Logger.LogMsg(LoggerLevel.Fatal, "Company compliance permission is disabled. Please restart the service");
                        //Console.WriteLine(ComplianceAlertsConstants.CONST_COMPLIANCE_PERMISSION_CHANGED);
                    }
                    _startComplianceAlertService = true;
                    Logger.LogMsg(LoggerLevel.Information, "CompliancePermissions request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CompliancePermissionsResponse encountered an error");
                }
            }
        }

        /// <summary>
        /// Cache logged-in web users information and checks Compliance permission
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserLoggedInInformation(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Dictionary<int, AuthenticatedUserInfo> _loggedInUsers = JsonConvert.DeserializeObject<Dictionary<int, AuthenticatedUserInfo>>(message.Data);
                    foreach (var kvp in _loggedInUsers)
                    {
                        if (_loggedInUsers[kvp.Key] != null)
                        {
                            int companyUserId = kvp.Key;
                            if (!_dictLoggedInUser.ContainsKey(kvp.Key) && _loggedInUsers[kvp.Key].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                            {
                                _dictLoggedInUser.Add(kvp.Key, _loggedInUsers[kvp.Key]);

                                #region Compliance Section
                                try
                                {
                                    //Checking for Compliance module and disabling/enabling if not enabled
                                    if (_preTradeModuleUsers.ContainsKey(companyUserId) || _postTradeModuleUsers.ContainsKey(companyUserId))
                                    {
                                        AmqpPluginManager.GetInstance().Initialise(companyUserId.ToString());
                                        //AmqpPlugin.AmqpPluginManager.GetInstance().PendingApprovalInfoDataSet += PranaMain_PendingApprovalInfoDataSet;
                                        //AmqpPlugin.AmqpPluginManager.GetInstance().PendingApprovalFrozeUnfroze += PranaMain_PendingApprovalFrozeUnfroze;
                                        Logger.LogMsg(LoggerLevel.Information, "Fetching permission for the given user:{0}", companyUserId);
                                    }
                                    else
                                        Logger.LogMsg(LoggerLevel.Information, "User:{0} not in preTradeModulesUser or _postTradeModuleUsers cache", companyUserId);

                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "UserLoggedInInformation users compliance sections encountered an error");
                                }
                                #endregion
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "UserLoggedInInformation encountered an error");
                }
            }
        }

        /// <summary>
        /// Sends received alerts to User for Manual/Live orders
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void OverrideRequestReceived(string message, DataSet data)
        {
            try
            {
                if (message == _navAndStartingPositionOfAcntResp)
                {
                    BasketComplianceOverideRequestReceived(message, data);
                    return;
                }

                var sw = Stopwatch.StartNew();

                string userId = data.Tables[0].Rows[0][ComplianceAlertsConstants.CONST_USERID].ToString();
                Dictionary<string, string> dataToSend = new Dictionary<string, string>
                {
                    { ComplianceAlertsConstants.CONST_HEADER_ALERTS, JsonConvert.SerializeObject(data.Tables[ComplianceAlertsConstants.CONST_ALERTS]) },
                    { ComplianceAlertsConstants.CONST_HEADER_POPUP_TYPE, data.Tables[0].Rows[0][ComplianceAlertsConstants.CONST_POPUP_TYPE].ToString() },
                    { ComplianceAlertsConstants.CONST_COMPANY_USERID, userId },
                    { ComplianceAlertsConstants.CONST_ORDERID, data.Tables[0].Rows[0][ComplianceAlertsConstants.CONST_ORDERID].ToString() }
                };

                RequestResponseModel response = new RequestResponseModel(Convert.ToInt32(userId), JsonConvert.SerializeObject(dataToSend));
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ComplianceAlertsData, response);

                Logger.LogMsg(LoggerLevel.Information, "Override request processed for the topic {0} for UserId {1} in {2}",
                    KafkaConstants.TOPIC_ComplianceAlertsData, userId, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "OverrideRequestReceived encountered an error");
            }
        }

        /// <summary>
        /// Received basket compliance response here
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>

        private void BasketComplianceOverideRequestReceived(string message, DataSet data)
        {
            try
            {
                if (data.Tables.Count == 0 || data.Tables[0].Rows?.Count == 0)
                {
                    Logger.LogMsg(LoggerLevel.Information, "Response Payload is empty for NavAndStartingPositionOfAccountsResponse");
                    return;
                }

                var correlationId = data.Tables[0].Rows[0]["CorrelationId"]?.ToString();
                var userId = Convert.ToInt32(data.Tables[0].Rows[0]["UserId"]);
                using (LoggerHelper.PushLoggingProperties(correlationId, Guid.Empty, userId))
                {
                    string json = JsonConvert.SerializeObject((data.Tables[0], Formatting.Indented));
                    var reqRespObj = new RequestResponseModel(userId, json, correlationId);
                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_GetAccountNavNStartingValueFromBasketResponse, reqRespObj);

                    Logger.LogMsg(LoggerLevel.Information, "Received BasketComplianceOverideRequestReceived (NavAndStartingPositionOfAccountsResponse) Payload from basket compliance for PST and has been published to kafka.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "OverrideRequestReceived encountered an error");
            }
        }

        /// <summary>
        /// Sends User response to Trade server for Manual/Live orders
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SendOverrideResponse(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    AlertPopUpType popUpType = (AlertPopUpType)(Convert.ToInt32(data.PopUpType));
                    string orderId = data.OrderId;
                    string userResponse = data.userResponse;
                    DataTable dataTable = data.Alerts.ToObject<DataTable>();
                    List<Alert> alerts = Alert.GetAlertObjectFromDataTable(dataTable);
                    bool finalResponse = GetResultBasedOnButtonClicked(userResponse);

                    if (popUpType == AlertPopUpType.Inform)
                        finalResponse = false;

                    //Sending response to server
                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(dataTable);
                    ComplianceServiceConnector.GetInstance().UpdateAlerts(alerts, orderId);

                    AmqpPluginManager.GetInstance().SendResponse(dataSet, finalResponse, popUpType);
                    Logger.LogMsg(LoggerLevel.Information, "Overide response proccessed for userId {0}, and orderId {1}",
                        message.CompanyUserID, orderId);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SendOverrideResponse encountered an error");
                }
            }
        }

        /// <summary>
        /// Sends alerts to User for Stage orders
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SendStageOrderForComplianceAlertsRequest(string topic, RequestResponseModel message)
        {
            string orderIdsStr = "";
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    Logger.LogMsg(LoggerLevel.Information, ComplianceAlertsConstants.CONST_STAGE_OVERRIDE_REQUEST_RECEIVED);

                    Dictionary<string, string> dataForStageOrderComplianceAlert = JsonConvert.DeserializeObject<Dictionary<string, string>>(message.Data);

                    List<OrderSingle> orders = JsonConvert.DeserializeObject<List<OrderSingle>>(dataForStageOrderComplianceAlert[ComplianceAlertsConstants.CONST_ORDER]);

                    Logger.LogMsg(LoggerLevel.Information, "Received complaince request for stage orders for user {user}, with order count {orderCount}", message.CompanyUserID, orders?.Count);

                    int companyUserID = Convert.ToInt32(dataForStageOrderComplianceAlert[ComplianceAlertsConstants.CONST_USERID]);
                    SimulationResult result = ComplianceServiceConnector.GetInstance().SimulateTrade(orders, PreTradeType.Stage, companyUserID, true, false);
                    List<Alert> alerts = result.Alerts;
                    Logger.LogMsg(LoggerLevel.Information,
                        "Received complaince response from simulate trade server for user {user}, with simulationId {simulationId} and alert count {alert}",
                        message.CompanyUserID,
                        result?.SimulationId,
                        alerts?.Count);

                    AlertPopUpType alertPopUpType = AlertPopUpType.None;
                    var orderIds = orders.Select(x => x.OrderID).ToList();
                    orderIdsStr = string.Join(",", orderIds);
                    if (result.Allowed)
                    {
                        if (alerts.Count == 0)
                        {
                            alerts.ForEach(x => x.Status = ComplianceAlertsConstants.MSG_STAGING_ALLOWED);
                            var responseForServer = new { Response = new { IsAllowed = false, OrderId = result.SimulationId, UserId = orders[0].CompanyUserID, isApprovalRequired = false } };
                            AmqpHelper.SendObject(responseForServer, PreTradeConstants.Const_OverrideResponse, null);
                            Logger.LogMsg(LoggerLevel.Information, "No alert found, for orderIds {0}", orderIdsStr);
                        }
                        else
                        {
                            if (result.OverrideType == RuleOverrideType.Soft)
                            {
                                alertPopUpType = AlertPopUpType.Override;
                            }
                            else if (result.OverrideType == RuleOverrideType.RequiresApproval)
                            {
                                alertPopUpType = AlertPopUpType.PendingApproval;
                            }
                            Logger.LogMsg(LoggerLevel.Information, "Found alert RuleOverrideType {0}, for orderIds {1}, setting alertPopType  {2}",
                                Convert.ToString(result.OverrideType), orderIdsStr, alertPopUpType);
                        }
                    }
                    else
                    {
                        if (result.Alerts.Count == 1 && result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))//required in response as well
                        {
                            if (orders.Count == 1)
                                result.Alerts[0].Description = result.Alerts[0].Description + TagDatabaseManager.GetInstance.GetOrderSideText(orders[0].OrderSideTagValue) + " " + orders[0].Symbol + " " + orders[0].Quantity + " @" + orders[0].AvgPriceForCompliance + " for " + TagDatabaseManager.GetInstance.GetOrderTypeText(orders[0].OrderTypeTagValue) + " Order";
                            else
                                result.Alerts[0].Description = result.Alerts[0].Description + ComplianceAlertsConstants.CONST_MULTIPLE_ORDER;
                            if (result.OverrideType == RuleOverrideType.Hard)
                            {
                                alertPopUpType = AlertPopUpType.Inform;
                            }
                            else if (result.OverrideType == RuleOverrideType.Soft)
                            {
                                alertPopUpType = AlertPopUpType.Override;
                            }
                        }
                        else
                        {
                            if (result.OverrideType == RuleOverrideType.Hard)
                            {
                                alertPopUpType = AlertPopUpType.Inform;
                            }
                        }
                    }

                    var desc = result.Alerts.Count > 0 ? result.Alerts[0]?.Description : "";
                    Logger.LogMsg(LoggerLevel.Information, "StageOrder For ComplianceAlerts summary: alertPopUpType:{0}, alert desc: {1}",
                        alertPopUpType, desc);

                    Dictionary<string, string> dataToSend = new Dictionary<string, string>
                {
                    { ComplianceAlertsConstants.CONST_HEADER_ALERTS, JsonConvert.SerializeObject(alerts) },
                    { ComplianceAlertsConstants.CONST_HEADER_POPUP_TYPE, Convert.ToString((int)alertPopUpType) },
                    { ComplianceAlertsConstants.CONST_COMPANY_USERID, companyUserID.ToString() },
                    { ComplianceAlertsConstants.CONST_ORDERID, result.SimulationId },
                    { ComplianceAlertsConstants.CONST_TRADE_TYPE,PreTradeType.Stage.ToString()},
                    { ComplianceAlertsConstants.CONST_RULE_TYPE, Convert.ToString((int)result.OverrideType) },
                    { ComplianceAlertsConstants.CONST_RESULT_ALLOWED,  Convert.ToString(result.Allowed?1:0) }
                };

                    RequestResponseModel response = new RequestResponseModel(Convert.ToInt32(companyUserID), JsonConvert.SerializeObject(dataToSend));
                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ComplianceAlertsDataSync, response);

                    Logger.LogMsg(LoggerLevel.Information, "Send Stage Order For Compliance Alerts Request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SendStageOrderForComplianceAlertsRequest encountered an error for orderId " + orderIdsStr);
                }
            }
        }

        /// <summary>
        /// Sends user response to Trade server for Stage orders
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_SendOverrideResponseForStage(string topic, RequestResponseModel message)
        {
            string orderId = "";
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    Logger.LogMsg(LoggerLevel.Information, ComplianceAlertsConstants.CONST_STAGE_OVERRIDE_RESPONSE_RECEIVED + message.CompanyUserID);

                    dynamic data = JsonHelper.DeserializeToObject<dynamic>(message.Data);
                    AlertPopUpType popUpType = (AlertPopUpType)(Convert.ToInt32(data.PopUpType));
                    orderId = data.OrderId;
                    string userResponse = data.userResponse;
                    DataTable dataTable = data.Alerts.ToObject<DataTable>();
                    List<Alert> alerts = Alert.GetAlertObjectFromDataTable(dataTable);
                    bool finalResponse = GetResultBasedOnButtonClicked(userResponse);
                    int alerttype = Convert.ToInt32(data.RuleType);
                    int resultAllowed = Convert.ToInt32(data.ResultAllowed);

                    if (resultAllowed == 1)
                    {
                        if (alerttype == (int)RuleOverrideType.Soft)
                        {
                            if (finalResponse)
                                alerts.ForEach(x => x.Status = ComplianceAlertsConstants.MSG_STAGING_ALLOWED_RULE_OVERRIDEN);
                            else
                                alerts.ForEach(x => x.Status = ComplianceAlertsConstants.MSG_STAGING_BLOCKED);

                            ComplianceServiceConnector.GetInstance().UpdateAlerts(alerts, alerts[0].OrderId);
                            var response = new { Response = new { IsAllowed = finalResponse, OrderId = alerts[0].OrderId, UserId = message.CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.Override } };
                            AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                        }
                        else if (alerttype == (int)RuleOverrideType.RequiresApproval)
                        {
                            ComplianceServiceConnector.GetInstance().UpdateAlerts(alerts, alerts[0].OrderId);
                            var response = new { Response = new { IsAllowed = finalResponse, OrderId = alerts[0].OrderId, UserId = message.CompanyUserID, isApprovalRequired = finalResponse, popUpType = AlertPopUpType.PendingApproval } };

                            AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                        }
                    }
                    else
                    {
                        if (alerts.Count == 1 && alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                        {
                            bool validation = false;
                            if (alerttype == (int)RuleOverrideType.Hard)
                            {
                                validation = false;

                            }
                            else if (alerttype == (int)RuleOverrideType.Soft)
                            {
                                validation = finalResponse;
                            }
                            var response = new { Response = new { IsAllowed = validation, OrderId = alerts[0].OrderId, UserId = message.CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.Inform } };
                            AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                        }
                        else
                        {
                            bool validation = false;
                            alerts.ForEach(x => x.Status = ComplianceAlertsConstants.MSG_STAGING_BLOCKED);
                            ComplianceServiceConnector.GetInstance().UpdateAlerts(alerts, alerts[0].OrderId);
                            var response = new { Response = new { IsAllowed = validation, OrderId = alerts[0].OrderId, UserId = message.CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.Inform } };
                            AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                        }
                    }
                    Logger.LogMsg(LoggerLevel.Information, "SendOverrideResponseForStage processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "SendOverrideResponseForStage encountered an error for orderId:" + orderId);
                }
            }
        }
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
                Logger.LogError(ex, "Kafka_ProducerReporter encountered an error");
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
                Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Kafka_ConsumerReporter encountered an error");
            }
        }
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
                UpdateServiceStatus(ServiceNameConstants.CONST_ComplianceAlerts_Name, ServiceNameConstants.CONST_ComplianceAlerts_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_ComplianceAlerts_Name);
                var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);

                Logger.LogMsg(LoggerLevel.Verbose, "Service status published to Kafka topic {0}", KafkaConstants.TOPIC_ServiceHealthStatus);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceServiceStatusMessage encountered an error");
            }
        }

        /// <summary>
        /// Gets the response based on button clicked
        /// </summary>
        /// <param name="userResponse"></param>
        /// <returns></returns>
        private bool GetResultBasedOnButtonClicked(string userResponse)
        {
            bool finalResponse = false;
            try
            {
                if (userResponse.ToUpper().Equals(ComplianceAlertsConstants.BTN_TEXT_YES) || userResponse.ToUpper().Equals(ComplianceAlertsConstants.BTN_TEXT_SEND))
                    finalResponse = true;
                else if (userResponse.ToUpper().Equals(ComplianceAlertsConstants.BTN_TEXT_NO) || userResponse.ToUpper().Equals(ComplianceAlertsConstants.BTN_TEXT_CANCEL))
                    finalResponse = false;
                Logger.LogMsg(LoggerLevel.Information, "Got user response {0}", userResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetResultBasedOnButtonClicked encountered an error");
            }
            return finalResponse;
        }

        /// <summary>
        /// Clears all cache
        /// </summary>
        private void ClearAllCache()
        {
            try
            {
                if (_preTradeEnabledCompany.Count > 0)
                    _preTradeEnabledCompany.Clear();
                if (_postTradeEnabledCompany.Count > 0)
                    _postTradeEnabledCompany.Clear();
                if (_preTradeModuleUsers.Count > 0)
                    _preTradeModuleUsers.Clear();
                if (_postTradeModuleUsers.Count > 0)
                    _postTradeModuleUsers.Clear();

                Logger.LogMsg(LoggerLevel.Information, "Clearing pre-post trade module user cache complete...");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ClearAllCache encountered an error");
            }
        }

        /// <summary>
        /// For connectivity with Trade server.
        /// </summary>
        ICommunicationManager _tradeCommunicationManager;
        private void ConnectToAllSockets()
        {
            try
            {
                _tradeCommunicationManager = new ClientTradeCommManager();
                _tradeCommunicationManager.Disconnected += new EventHandler(CommunicationManager_Disconnected);
                _tradeCommunicationManager.Connected += new EventHandler(CommunicationManager_Connected);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ConnectToAllSockets encountered an error");
            }
        }

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
                connProperties.IdentifierID = "ComplianceAlertsService_SocketConn";
                connProperties.IdentifierName = "Compliance Alerts Service SocketConn";
                connProperties.ConnectedServerName = "Trade ";
                connProperties.HandlerType = HandlerType.TradeHandler;

                Logger.LogMsg(LoggerLevel.Information, "Trade server connection details. Port:{0}, Ip:{1}", connProperties.Port, connProperties.ServerIPAddress);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetTradeServerConnectionDetails encountered an error");
            }
            return connProperties;
        }

        /// <summary>
        /// Event for InformationReceived handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void KafkaManager_GetAccountNavNStartingValueFromBasketRequest(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Received NavAndStartingPositionOfAccountsRequest to process NAV and starting value from basket");
                    //_compliancePermissionsData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data);

                    IDictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("payLoad", message.Data);
                    dict.Add("correlationId", message.CorrelationId);
                    dict.Add("ResponseType", "GetAccountNavNStartingValueFromBasket");

                    AmqpHelper.SendObject(dict, PreTradeConstants.CONST_BASKET_COMPLIANCE, "NavAndStartingPositionOfAccountsRequest");

                    Logger.LogMsg(LoggerLevel.Information, "NavAndStartingPositionOfAccountsRequest request processed successfully in {a} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CompliancePermissionsResponse encountered an error");
                }
            }
        }

        private void KafkaManager_CheckComplianceRequest(string topic, RequestResponseModel message)
        {
            var sw = Stopwatch.StartNew();
            if (message == null)
            {
                Logger.LogMsg(LoggerLevel.Information, "Check compliance request body is null or empty...");
                return;
            }

            var respObject = new RequestResponseModel(message.CompanyUserID,
                null, message?.CorrelationId);

            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    Logger.LogMsg(LoggerLevel.Information, "Check compliance request recevived...");
                    Logger.LogMsg(LoggerLevel.Debug, "Check compliance request data is :{a}", message.Data);

                    var orders = JsonHelper.DeserializeToObject<List<OrderSingle>>(message.Data);


                    Logger.LogMsg(LoggerLevel.Debug, "Check compliance orderList count {a}", orders.Count);
                    // var orderList = JsonConvert.DeserializeObject<List<OrderSingle>>(message.Data);

                    SimulationResult result = ComplianceServiceConnector
                        .GetInstance()
                        .SimulateTrade(orders, PreTradeType.ComplianceCheck, message.CompanyUserID, true, false);

                    if (result == null)
                    {
                        Logger.LogMsg(LoggerLevel.Fatal, "Check compliance Simulate Trade result is null.");
                        respObject.ErrorMsg = "Check compliance trade server error. No data returned from trade server";
                        _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ComplianceAlertsData, respObject);
                        return;
                    }

                    List<Alert> alerts = result.Alerts;
                    AlertPopUpType alertPopUpType = AlertPopUpType.None;
                    var orderIds = orders.Select(x => x.OrderID).ToList();
                    var orderIdsStr = string.Join(",", orderIds);
                    if (result.Allowed)
                    {
                        if (result.OverrideType == RuleOverrideType.Soft)
                        {
                            alertPopUpType = AlertPopUpType.Override;
                        }
                        else if (result.OverrideType == RuleOverrideType.RequiresApproval)
                        {
                            alertPopUpType = AlertPopUpType.PendingApproval;
                        }
                        Logger.LogMsg(LoggerLevel.Information, "Found alert RuleOverrideType {0}, for orderIds {1}, setting alertPopType  {2}",
                            Convert.ToString(result.OverrideType), orderIdsStr, alertPopUpType);
                    }
                    else
                    {
                        if (result.Alerts.Count == 1 && result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))//required in response as well
                        {
                            if (orders.Count == 1)
                                result.Alerts[0].Description = result.Alerts[0].Description + TagDatabaseManager.GetInstance.GetOrderSideText(orders[0].OrderSideTagValue) + " " + orders[0].Symbol + " " + orders[0].Quantity + " @" + orders[0].AvgPriceForCompliance + " for " + TagDatabaseManager.GetInstance.GetOrderTypeText(orders[0].OrderTypeTagValue) + " Order";
                            else
                                result.Alerts[0].Description = result.Alerts[0].Description + ComplianceAlertsConstants.CONST_MULTIPLE_ORDER;
                            if (result.OverrideType == RuleOverrideType.Hard)
                            {
                                alertPopUpType = AlertPopUpType.Inform;
                            }
                            else if (result.OverrideType == RuleOverrideType.Soft)
                            {
                                alertPopUpType = AlertPopUpType.Override;
                            }
                        }
                        else
                        {
                            if (result.OverrideType == RuleOverrideType.Hard)
                            {
                                alertPopUpType = AlertPopUpType.Inform;
                            }
                        }
                    }

                    var desc = result.Alerts.Count > 0 ? result.Alerts[0]?.Description : "";
                    Logger.LogMsg(LoggerLevel.Information, "Checked compliance For ComplianceAlerts summary: alertPopUpType:{0}, alert desc: {1}",
                        alertPopUpType, desc);

                    Dictionary<string, string> dataToSend = new Dictionary<string, string>
                    {
                        { ComplianceAlertsConstants.CONST_HEADER_ALERTS, JsonConvert.SerializeObject(alerts) },
                        { ComplianceAlertsConstants.CONST_HEADER_POPUP_TYPE, Convert.ToString((int)alertPopUpType) },
                        { ComplianceAlertsConstants.CONST_COMPANY_USERID, message.CompanyUserID.ToString()},
                        { ComplianceAlertsConstants.CONST_ORDERID, result.SimulationId },
                        { ComplianceAlertsConstants.CONST_TRADE_TYPE,PreTradeType.Stage.ToString()},
                        { ComplianceAlertsConstants.CONST_RULE_TYPE, Convert.ToString((int)result.OverrideType) },
                        { ComplianceAlertsConstants.CONST_RESULT_ALLOWED,  Convert.ToString(result.Allowed?1:0) },
                        { ComplianceAlertsConstants.CONST_CORRELATIONID,  message.CorrelationId},
                        { ComplianceAlertsConstants.CONST_PRETRADETYPE,  PreTradeType.ComplianceCheck.ToString()},

                    };

                    var response = new
                    {
                        Response = new
                        {
                            IsAllowed = false,
                            OrderId = result.SimulationId,
                            UserId = orders[0].CompanyUserID,
                            isApprovalRequired = false,
                            popUpType = AlertPopUpType.ComplianceCheck
                        }
                    };

                    AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                    Logger.LogMsg(LoggerLevel.Debug, "Check compliance response receveid from trade server: for simulationId {b}, {c} and acknowledge send to trade server", result.SimulationId,
                        JsonConvert.SerializeObject(result));

                    respObject.Data = JsonConvert.SerializeObject(dataToSend);

                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ComplianceAlertsDataSync, respObject);

                    Logger.LogMsg(LoggerLevel.Information,
                        "Checked compliance request has been processed successfully in {a} ms and the response has been send in kafka",
                        sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "CheckComplianceRequest encountered an error");
                    respObject.ErrorMsg = "Internal server error in compliance service for CheckComplianceRequest";
                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ComplianceAlertsData, respObject);
                }
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
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "TradeServiceClientHeartbeatManager_ConnectedEvent encountered an error");
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
                Logger.LogError(ex, "TradeServiceClientHeartbeatManager_DisconnectedEvent encountered an error");
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
                Logger.LogError(ex, "TradeServiceClientHeartbeatManager_AnotherInstanceSubscribedEvent encountered an error");
            }
        }
        #endregion

        #region IDisposable Method
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Methods to dispose objects on close.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    //TODO: Dispose proxy and timer here
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Dispose() encountered an  error");
            }
        }
        #endregion

        #region Unimplemented methods
        private void CommunicationManager_Connected(object sender, EventArgs e)
        {
        }

        private void CommunicationManager_Disconnected(object sender, EventArgs e)
        {
        }

        public Task<List<HostedService>> GetClientServicesStatus()
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetDebugModeStatus()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HealthCheck()
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

        public Task<bool?> Subscribe(string subscriberName, bool isRetryRequest)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UnSubscribe(string subscriberName)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}