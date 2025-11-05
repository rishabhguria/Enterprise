using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.AmqpAdapter.Amqp;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Constants;
using Prana.CalculationService.CacheStore;
using Prana.CalculationService.Constants;
using Prana.CalculationService.Models;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.GreenfieldServices.Common;
using Prana.Interfaces;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ServiceCommon.Interfaces;
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
using System.Threading.Tasks;

namespace Prana.CalculationService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class CalculationService : BaseService, ICalculationService, IDisposable
    {
        #region Variables
        private IWindsorContainer _container;

        /// <summary>
        /// Stores boolean value to check service started.
        /// true if service started completely else false
        /// </summary>
        private bool _isCalculationServiceStarted = false;

        /// <summary>
        /// Instance for RtpnlLayoutManager
        /// </summary>
        private static RtpnlLayoutManager _rtpnlLayoutManager;

        /// <summary>
        /// Instance for CompressionsCache
        /// </summary>
        private static CompressionsCache _compressionsCache;

        /// <summary>
        /// Stores information of Users logged into Web application.
        /// </summary>
        private static Dictionary<int, object> _dictLoggedInUser = new Dictionary<int, object>();

        /// <summary>
        /// Instance for ConfigurationDetails
        /// </summary>
        private static ConfigurationDetails _configurationDetails;

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
        /// Cache to store info about user has permission for all Accounts or not.
        /// </summary>
        Dictionary<int, bool> _userUnallocatedAccPermission = new Dictionary<int, bool>();

        /// <summary>
        /// Stores boolean value to check Compliance permissions received.
        /// true if received else false
        /// </summary>
        bool _isCompliancePermissionReceived = false;

        /// <summary>
        /// Stores boolean value to check initialization request sent to Compliance engine. 
        /// true if service started completely else false
        /// </summary>
        private bool _initRequestSentToCompliance = false;

        /// <summary>
        /// Stores information of all accounts.
        /// </summary>
        private AccountCollection _allCompanyAccounts = new AccountCollection();

        /// <summary>
        /// SemaphoreSlim object
        /// </summary>
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());

        #region Rtpnl timer

        /// <summary>
        /// Stores value of RtpnlUpdateTimeInterval from App.config file
        /// </summary>
        private int _rtpnlUpdateTimeInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(RtpnlConstants.CONFIG_APPSETTING_RtpnlUpdateTimeInterval));

        /// <summary>
        /// Stores value of _rtpnlEventsCountLogInterval from App.config file
        /// </summary>
        private int _rtpnlEventsCountLogInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey(RtpnlConstants.CONFIG_APPSETTING_RtpnlEventsCountLogInterval));

        /// <summary>
        /// Stores value of CustomGroupingPermission from App.config file
        /// </summary>
        private bool _isCustomGroupingEnabled = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey(RtpnlConstants.CONFIG_APPSETTING_IsCustomGroupingEnabled));

        /// <summary>
        ///Timer based on RtpnlUpdateTimeInterval to sends updated rtpnl data to Web application for Row Calculation
        /// </summary>
        private System.Timers.Timer _rtpnlUpdateTimerForRowCalculation = new System.Timers.Timer();

        /// <summary>
        /// true if updates logging required else false
        /// </summary>
        private bool _isLoggingRequired = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey(RtpnlConstants.CONFIG_APPSETTING_IsUpdatesLogsRequired));

        /// <summary>
        ///Timer for logging updates count for RTPNL
        /// </summary>
        private System.Timers.Timer _timerForLog = new System.Timers.Timer();

        /// <summary>
        /// Proxy for Dynamic UDA
        /// </summary>
        ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;
        public ProxyBase<ISecMasterSyncServices> SecMasterSyncService
        {
            set { _secMasterSyncService = value; }
        }

        /// <summary>
        /// Cache to store dynamic UDAs data
        /// </summary>
        SerializableDictionary<string, DynamicUDA> _dynamicUDACache = new SerializableDictionary<string, DynamicUDA>();

        private int eventsReceivedFromEsper = 0;
        #endregion

        static int sentUpdatesNotInLast5Minute = 0;
        static int sentRowCalculationCount = 0;
        int messagePrintingCounter = 0;
        #endregion

        #region IPranaServiceCommon Methods
        public async System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container)
        {
            try
            {
                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                WindsorContainerManager.Container = container;
                CompanyID = CachedDataManager.GetInstance.GetCompanyID();
                if (_allCompanyAccounts == null || _allCompanyAccounts.Count == 0)
                {
                    _allCompanyAccounts = WindsorContainerManager.GetAccounts();
                }
                MarketDataProvider marketDataProvider = CachedDataManager.CompanyMarketDataProvider;
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                #region Topic to fetch Compliance permission
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CompliancePermissionsResponse, KafkaManager_CompliancePermissionsResponse);
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompliancePermissionsRequest, new RequestResponseModel(0, ""));
                #endregion

                #region Instances

                _rtpnlLayoutManager = RtpnlLayoutManager.GetInstance();
                _compressionsCache = CompressionsCache.GetInstance();
                InitializeTimerStatisticsLogging();
                _compressionsCache.MarketDataProvider = marketDataProvider;
                _configurationDetails = ConfigurationDetails.GetInstance();
                #endregion

                #region Waiting for response from Common data greenfield service
                int totalSleepCycle = 0;
                while (!_isCompliancePermissionReceived)
                {
                    Console.WriteLine(RtpnlConstants.MSG_WAITING_FOR_COMMON_DATA_SERVICE);
                    Thread.Sleep(2000);
                    totalSleepCycle += 1;
                    if (totalSleepCycle % 3 == 0)
                        _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CompliancePermissionsRequest, new RequestResponseModel(0, ""));
                }
                #endregion

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_CheckCalculationServicIsRunningRequest, KafkaManager_CalculationServiceRunningRequest);

                if ((_preTradeEnabledCompany.ContainsKey(CompanyID) && _preTradeEnabledCompany[CompanyID]) || (_postTradeEnabledCompany.ContainsKey(CompanyID) && _postTradeEnabledCompany[CompanyID]))
                {
                    CreateProxy();

                    #region SubscribeAndConsume
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_UserLoggedInInformation);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_UserLoggedOutInformation);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveUpdateConfigDetailsRequest, KafkaManager_SaveUpdateConfigDetailsRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_UserPermittedAccountsResponse, KafkaManager_UserPermittedAccountsReceived);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetCountRequest, KafkaManager_RtpnlWidgetCountRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetDataRequest, KafkaManager_RtpnlWidgetDataRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDataForExtractRequest, KafkaManager_SaveConfigDataForExtractRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteRemovedWidgetConfigDetailsRequest, KafkaManager_DeleteRemovedWidgetConfigDetailsRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveConfigDetailsForSavePageAsRequest, KafkaManager_SaveConfigDetailsForSavePageAsRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RtpnlWidgetConfigDataRequest, KafkaManager_RtpnlConfigDataRequest);
                    KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoggedInUserResponse, KafkaManager_UpdateLoggedInUserData);
                    #endregion

                    #region Amqp plugin
                    AmqpPluginManager.GetInstance().Initialise();
                    AmqpPluginManager.GetInstance().StartCommunication += new StartCommunicationHandler(StartCommunicationProcess);
                    AmqpPluginManager.GetInstance().RtpnlCompressionsDataReceived += new RtpnlCompressionsDataReceivedHandler(RtpnlStartupAndPostDataReceived);
                    #endregion
                    _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                    RowCalculationBaseNav.DynamicUDAKeys = _dynamicUDACache.Keys.ToList();
                }
                else
                {
                    Console.WriteLine(RtpnlConstants.MSG_DashLines);
                    InformationReporter.GetInstance.Write(RtpnlConstants.MSG_CONST_COMPANY_HAS_NO_COMPLIANCE_PERMISSION);
                    Console.WriteLine(RtpnlConstants.MSG_DashLines);
                }
                #region Waiting for response from Compliance engine
                while (!_isCalculationServiceStarted)
                {
                    if (_initRequestSentToCompliance)
                        Console.WriteLine(RtpnlConstants.MSG_WAITING_FOR_COMPLIANCE_ENGINE);
                    Thread.Sleep(2000);
                }
                #endregion


                #region Rtpnl update data timer

                _rtpnlUpdateTimerForRowCalculation.Interval = _rtpnlUpdateTimeInterval;
                _rtpnlUpdateTimerForRowCalculation.Elapsed += RtpnlUpdateTimerForRowCalculation_Elapsed;
                _rtpnlUpdateTimerForRowCalculation.Start();

                #endregion

                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(RtpnlConstants.MSG_CalculationServicestarted + DateTime.UtcNow, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");

                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
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
            UpdateServiceStatus(ServiceNameConstants.CONST_Calculation_Name, ServiceNameConstants.CONST_Calculation_DisplayName, false);

            CheckCalculationServiceRunningRequest(false);
            Console.WriteLine(RtpnlConstants.MSG_ShutdownService);
            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(RtpnlConstants.MSG_CalculationServiceClosed + DateTime.UtcNow, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
            _container.Dispose();
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
                UpdateServiceStatus(ServiceNameConstants.CONST_Calculation_Name, ServiceNameConstants.CONST_Calculation_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_Calculation_Name);
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
        /// Logs a message to a file if message printing is enabled and the message count is below the limit.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        private void PrintMessageLog(object message)
        {
            try
            {
                // Check if message printing is enabled and the counter is below the limit
                if (message != null)
                {
                    // Increment the message printing counter
                    messagePrintingCounter++;

                    // Serialize the message content to JSON format
                    string messagecontent = JsonHelper.SerializeObject(message);

                    // Calculate the size of the serialized message in bytes
                    long objectSize = System.Text.Encoding.Unicode.GetByteCount(messagecontent);

                    // Log the message details to a file
                    Logger.LogMsg(LoggerLevel.Verbose, "-------------------------------------------------------");
                    Logger.LogMsg(LoggerLevel.Verbose, string.Format("Message No.{0} and Message Size:{1} bytes", messagePrintingCounter, objectSize));
                    Logger.LogMsg(LoggerLevel.Verbose, messagecontent);
                    Logger.LogMsg(LoggerLevel.Verbose, "-------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during logging
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        /// <summary>
        /// Starts communication process with Esper engine.
        /// </summary>
        private void StartCommunicationProcess()
        {
            try
            {
                Dictionary<string, string> completionInfo = new Dictionary<string, string>
                {
                    { RtpnlConstants.CONST_ResponseType, RtpnlConstants.CONST_InitRequest },
                    { RtpnlConstants.CONST_CUSTOM_GROUPING_PERMISSION, _isCustomGroupingEnabled.ToString() }
                };
                AmqpHelper.SendObject(completionInfo, RtpnlConstants.CONST_OtherDataSender, RtpnlConstants.CONST_InitRequest);
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_InitRequestForEsper);
                _initRequestSentToCompliance = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sends communication response to Esper engine.
        /// </summary>
        private void SendCommunicationResponse()
        {
            try
            {
                Dictionary<string, string> completionInfo = new Dictionary<string, string>
                {
                    { RtpnlConstants.CONST_ResponseType, RtpnlConstants.CONST_CommunicationResponseForEsper }
                };
                AmqpHelper.SendObject(completionInfo, RtpnlConstants.CONST_OtherDataSender, RtpnlConstants.CONST_CommunicationResponseForEsper);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// RTPNL startup and Post data receiver method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void RtpnlStartupAndPostDataReceived(string routingKey, DataSet data, string jsonData)
        {
            try
            {
                if (routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_InitResponse))
                {
                    InformationReporter.GetInstance.Write(RtpnlConstants.MSG_InitCompleteFromEsper);
                    _isCalculationServiceStarted = true;
                    CheckCalculationServiceRunningRequest(true);
                }
                else if (routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_CommunicationRequestFromEsper))
                    SendCommunicationResponse();
                else if (routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_RowCalculationBaseWithNavStartupData))
                    _compressionsCache.StartupDataReceived(routingKey, jsonData);
                else if (routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_ExtendedAccountSymbolWithNav))
                {
                    if (_isCalculationServiceStarted)
                    {
                        JToken rootObject = JToken.Parse(jsonData);
                        JObject hashMap = rootObject[CompressionConstants.CONST_HashMap] as JObject;
                        string symbol = hashMap[CompressionConstants.CONST_Symbol].ToString();
                        if (_compressionsCache.SymbolsList.Count > 0 && _compressionsCache.SymbolsList.Contains(symbol))
                        {
                            double quantity = Convert.ToDouble(hashMap[CompressionConstants.CONST_Quantity].ToString());
                            string uniqueId = Convert.ToInt32(hashMap[CompressionConstants.CONST_AccountID]) + RtpnlConstants.CONST_Underscore + hashMap[CompressionConstants.CONST_Symbol].ToString();
                            double feedPriceB = Convert.ToDouble(hashMap[CompressionConstants.CONST_CostBasisPnLBase].ToString());

                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - Update Start SYM: " + symbol + " QTY: " + quantity + " SFP: " + feedPriceB + " UQID: " + uniqueId);
                        }
                        eventsReceivedFromEsper++;
                        _compressionsCache.RowCalculationBaseWithNavReceived(routingKey, jsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Check Calculation Service is Running
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        private void CheckCalculationServiceRunningRequest(bool isCalculationServiceStarted)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_CheckCalculationServiceRunningReceived);
                Dictionary<string, string> responseInfo = new Dictionary<string, string>
                {
                    { RtpnlConstants.CONST_CALCULATION_SERVICE_STARTED, isCalculationServiceStarted.ToString().ToLower() },
                    { RtpnlConstants.CONST_CUSTOM_GROUPING_PERMISSION, _isCustomGroupingEnabled.ToString().ToLower() }
                };
                RequestResponseModel response = new RequestResponseModel(0, JsonConvert.SerializeObject(responseInfo));
                _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_CheckCalculationServicIsRunning, response, _isLoggingRequired);
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_CheckCalculationServiceRunningProcessed + isCalculationServiceStarted.ToString());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            }
        }
        #region Rtpnl timer elapse
        /// <summary>
        /// RtpnlUpdateTimer_Elapsed event for Row Calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RtpnlUpdateTimerForRowCalculation_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (_compressionsCache.LockForOutgoingRowCalculation)
                {
                    Dictionary<string, RowCalculationBaseNav> outgoingBatches = new Dictionary<string, RowCalculationBaseNav>();
                    ConcurrentDictionary<string, RowCalculationBaseNav> outgoingCache = _compressionsCache.GetRowCalculationOutgoingUpdates();
                    Dictionary<string, RowCalculationMarketDataBased> outgoingMarketDataBasedBatches = new Dictionary<string, RowCalculationMarketDataBased>();

                    if (outgoingCache != null && outgoingCache.Count > 0)
                    {
                        bool _logForTimerCompletedOutgoingBatch = false;
                        bool _logForTimerCompletedOutgoingMarketDataBasedBatch = false;

                        foreach (KeyValuePair<string, RowCalculationBaseNav> rowCalculation in outgoingCache)
                        {
                            if (!_compressionsCache.DictRowCalculation.ContainsKey(rowCalculation.Key))
                            {
                                if (!rowCalculation.Value.Symbol.Equals(RtpnlConstants.CONST_NO_POSITION))
                                {
                                    _compressionsCache.DictRowCalculation.TryAdd(rowCalculation.Key, rowCalculation.Value);
                                }
                                rowCalculation.Value.PermOverride = true;
                                outgoingBatches.Add(rowCalculation.Key, rowCalculation.Value);
                                PrintMessageLog(rowCalculation.Value);
                            }
                            else
                            {
                                rowCalculation.Value.PermOverride = _compressionsCache.DictRowCalculation[rowCalculation.Key].Equals(rowCalculation.Value);
                                object outgoingUpdate = null;

                                if (_compressionsCache.DictRowCalculation[rowCalculation.Key].AreBasePropertiesChanged(rowCalculation.Value))
                                {
                                    outgoingBatches.Add(rowCalculation.Key, rowCalculation.Value);
                                    outgoingUpdate = rowCalculation.Value;
                                    if (_compressionsCache.SymbolsList.Count > 0 && _compressionsCache.SymbolsList.Contains(rowCalculation.Value.Symbol))
                                    {
                                        _logForTimerCompletedOutgoingBatch = true;
                                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - Outgoing Batch SYM: " + rowCalculation.Value.Symbol + " QTY: " + rowCalculation.Value.Qty + " SFP: " + rowCalculation.Value.FeedPriceB + " UQID: " + rowCalculation.Value.UnqId);
                                    }
                                }
                                else
                                {
                                    RowCalculationMarketDataBased rowCalculationMarketDataBased = new RowCalculationMarketDataBased();
                                    rowCalculationMarketDataBased.UpdateMarketDataDependentValues(rowCalculation.Value);
                                    outgoingMarketDataBasedBatches.Add(rowCalculation.Key, rowCalculationMarketDataBased);
                                    outgoingUpdate = rowCalculationMarketDataBased;
                                    if (_compressionsCache.SymbolsList.Count > 0 && _compressionsCache.SymbolsList.Contains(rowCalculation.Value.Symbol))
                                    {
                                        _logForTimerCompletedOutgoingMarketDataBasedBatch = true;
                                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - Outgoing Market Based Batch SYM: " + rowCalculation.Value.Symbol + " QTY: " + rowCalculation.Value.Qty + " SFP: " + rowCalculation.Value.FeedPriceB + " UQID: " + rowCalculation.Value.UnqId);
                                    }
                                }

                                _compressionsCache.DictRowCalculation[rowCalculation.Key] = rowCalculation.Value;
                                PrintMessageLog(outgoingUpdate);
                            }
                            ++sentRowCalculationCount;
                        }

                        if (outgoingBatches != null && outgoingBatches.Count > 0)
                        {
                            RequestResponseModel response = new RequestResponseModel(0, RowCalculationBaseNav.GetCustomSerializedRowCalculationDictionaryData(outgoingBatches));
                            _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, response, _isLoggingRequired);
                            if (_logForTimerCompletedOutgoingBatch)
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " ----- Outgoing Batch sent ------");
                        }
                        if (outgoingMarketDataBasedBatches != null && outgoingMarketDataBasedBatches.Count > 0)
                        {
                            RequestResponseModel response = new RequestResponseModel(0, JsonConvert.SerializeObject(outgoingMarketDataBasedBatches));
                            _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlRowCalculationUpdates, response, _isLoggingRequired);
                            if (_logForTimerCompletedOutgoingMarketDataBasedBatch)
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " ----- Outgoing Market Based Batch sent ------");
                        }
                    }
                    outgoingCache = null;
                    outgoingBatches = null;
                    outgoingMarketDataBasedBatches = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void InitializeTimerStatisticsLogging()
        {
            try
            {
                System.Threading.Tasks.Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromMinutes(5));
                        var (totalRowsReceivedForConflation, rowsLeftAfterConflation) = _compressionsCache.GetStatistics();
                        var (duplicateRowCalculationCount, duplicateRowInLast5Min, totalZeroQuantityRowCalculationUpdates) = _compressionsCache.GetDuplicateStatistics();
                        Logger.LogMsg(LoggerLevel.Information, "-------------------------------------------------------");
                        Logger.LogMsg(LoggerLevel.Information, $"1.Total events received before conflation in the last 5 minutes: {totalRowsReceivedForConflation}");
                        Logger.LogMsg(LoggerLevel.Information, $"2.Events left after conflation in the last 5 minutes: {rowsLeftAfterConflation}");
                        Logger.LogMsg(LoggerLevel.Information, $"3.Events that were identtified as duplicates and not transmitted to the frontend in the previous 5 Minutes:{duplicateRowInLast5Min}");
                        Logger.LogMsg(LoggerLevel.Information, $"4.Total events identified as duplicates, and therefore not forwarded to the frontend, are:{duplicateRowCalculationCount}");
                        Logger.LogMsg(LoggerLevel.Information, $"5.Total events identified as zero quantity updates are:{totalZeroQuantityRowCalculationUpdates}");
                        Logger.LogMsg(LoggerLevel.Information, $"6.Total events sent to Frontend in the last 5-minutes:{sentRowCalculationCount - sentUpdatesNotInLast5Minute}");
                        Logger.LogMsg(LoggerLevel.Information, $"7.Events for price changes sent to the frontend till now are:{sentRowCalculationCount}");
                        Logger.LogMsg(LoggerLevel.Information, "8.Events for price changes received from Esper till now are: " + eventsReceivedFromEsper);
                        Logger.LogMsg(LoggerLevel.Information, "-------------------------------------------------------");
                        sentUpdatesNotInLast5Minute = sentRowCalculationCount;
                        _compressionsCache.ResetStatistics();
                    }
                }, System.Threading.Tasks.TaskCreationOptions.LongRunning);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void KafkaManager_DeleteRemovedWidgetConfigDetailsRequest(string topic, RequestResponseModel message)
        {
            try
            {
                WidgetData[] widgetsConfigData = JsonConvert.DeserializeObject<WidgetData[]>(message.Data);
                DeleteRemovedWidgetsfromViews(message.CompanyUserID, widgetsConfigData);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// This method will delete removed widgets from database 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ViewName"></param>
        private void DeleteRemovedWidgetsfromViews(int userID, WidgetData[] removedWidgetItems)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_DeleteUnsavedWidgetMES);
                lock (ConfigurationDetails.GetInstance().UsersConfigurationDetails)
                {
                    string concatedRemovedWidgets = string.Empty;
                    string[] RemovedWidgetArray = null;
                    string pageId = string.Empty;
                    string viewName = string.Empty;

                    if (removedWidgetItems != null)
                    {
                        foreach (WidgetData item in removedWidgetItems)
                        {
                            if (item != null)
                            {
                                string widgetId = item.widgetId;
                                concatedRemovedWidgets += widgetId + RtpnlConstants.CONST_TILDE;
                                if (string.IsNullOrEmpty(viewName))
                                    viewName = item.viewName;
                                if (string.IsNullOrEmpty(pageId))
                                    pageId = item.pageId;
                            }
                        }
                    }
                    if (concatedRemovedWidgets != string.Empty)
                    {
                        concatedRemovedWidgets = concatedRemovedWidgets.Substring(0, concatedRemovedWidgets.Length - 1);
                        RemovedWidgetArray = concatedRemovedWidgets.Split(RtpnlConstants.CONST_TILDE);
                    }
                    DbManager.GetInstance().DeleteRemovedWidgetsFromView(userID, viewName, concatedRemovedWidgets, pageId);

                    InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SpecificWidgetDataDeleted + RtpnlConstants.MSG_UserId + userID + RtpnlConstants.MSG_LayoutItem + concatedRemovedWidgets + RtpnlConstants.MSG_ViewName + viewName);

                    //Handling common collections
                    Dictionary<int, List<RtpnlResponse>> UsersConfigurationDetails = ConfigurationDetails.GetInstance().UsersConfigurationDetails;
                    if (UsersConfigurationDetails != null && UsersConfigurationDetails.ContainsKey(userID))
                    {
                        List<RtpnlResponse> configurationDetails = UsersConfigurationDetails[userID].ToList();
                        List<RtpnlResponse> updatedResponse = new List<RtpnlResponse>();
                        foreach (RtpnlResponse configuration in configurationDetails)
                        {
                            if (configuration.viewName == viewName && configuration.pageId == pageId)
                            {
                                if (RemovedWidgetArray != null && !RemovedWidgetArray.Contains(configuration.widgetId))
                                {
                                    updatedResponse.Add(configuration);
                                }
                            }
                            else
                            {
                                updatedResponse.Add(configuration);
                            }
                        }
                        //TODO: Should use Setter for update, currently updating through reference
                        UsersConfigurationDetails[userID] = updatedResponse;
                    }
                    else
                    {
                        InformationReporter.GetInstance.Write(RtpnlConstants.MSG_UserInformationNotAvailable + userID + RtpnlConstants.MSG_ViewName + viewName);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region Kafka handlers
        /// <summary>
        /// Cache logged-in web users information 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserLoggedInInformation(string topic, RequestResponseModel message)
        {
            try
            {
                Dictionary<int, AuthenticatedUserInfo> _loggedInUsers = JsonConvert.DeserializeObject<Dictionary<int, AuthenticatedUserInfo>>(message.Data);
                foreach (var kvp in _loggedInUsers)
                {
                    if (_loggedInUsers[kvp.Key] != null)
                    {
                        if (!_dictLoggedInUser.ContainsKey(kvp.Key) && _loggedInUsers[kvp.Key].AuthenticationType == AuthenticationTypes.WebLoggedIn)
                        {
                            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_UserLoggedInInfoReceived + kvp.Key);
                            _dictLoggedInUser.Add(kvp.Key, _loggedInUsers[kvp.Key]);
                            _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserPermittedAccountsRequest, new RequestResponseModel(kvp.Key, ""));
                            //_rtpnlLayoutManager.LoadRtpnlViewsForLoggedInUser(kvp.Key);
                            _configurationDetails.LoadConfigurationDetailsForUser(kvp.Key);
                            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_UserLoggedInInfoProcessed + kvp.Key);
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
        /// Cache logged-out web users information
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserLoggedOutInformation(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_UserLoggedOutInformationReceived + message.CompanyUserID);
                int companyUserID = message.CompanyUserID;
                if (_dictLoggedInUser.ContainsKey(companyUserID))
                {
                    _dictLoggedInUser.Remove(companyUserID);
                    _rtpnlLayoutManager.RemoveRtpnlViewsForLoggedInUser(companyUserID);
                    _configurationDetails.RemoveConfigurationDetailsForUser(companyUserID);
                    _rtpnlLayoutManager.RemoveLayoutsForLoggedOutUser(companyUserID);
                    if (_userUnallocatedAccPermission.ContainsKey(companyUserID))
                        _userUnallocatedAccPermission.Remove(companyUserID);
                }
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_UserLoggedOutInformationProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Cache Compliance permissions for Company and Users
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CompliancePermissionsResponse(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_COMPLIANCE_PERMISSION_RECEIVED);
                _compliancePermissionsData = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data);
                ClearAllCache();
                _preTradeEnabledCompany = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_PRE_TRADE_COMPANY]);
                _postTradeEnabledCompany = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_POST_TRADE_COMPANY]);
                _preTradeModuleUsers = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_PRE_TRADE_USERS]);
                _postTradeModuleUsers = JsonHelper.DeserializeToObject<Dictionary<int, bool>>(_compliancePermissionsData[ComplianceConstants.CONST_POST_TRADE_USERS]);

                if (_isCalculationServiceStarted && _preTradeEnabledCompany.ContainsKey(CompanyID) && !_preTradeEnabledCompany[CompanyID] && _postTradeEnabledCompany.ContainsKey(CompanyID) && !_postTradeEnabledCompany[CompanyID])
                {
                    Console.WriteLine(RtpnlConstants.MSG_COMPLIANCE_PERMISSION_CHANGED);
                }
                _isCompliancePermissionReceived = true;
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_COMPLIANCE_PERMISSION_PROCESSED);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
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
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Save and Update Configuration Detail request received handler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveUpdateConfigDetailsRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SAVE_UPDATE_CONFIG_DETAILS_REQUEST_RECEIVED + message.CompanyUserID);
            string errorMessage = string.Empty;
            string uniqueIdentifier = string.Empty;
            try
            {
                dynamic[] widgetsConfigData = JsonConvert.DeserializeObject<WidgetData[]>(message.Data);
                if (widgetsConfigData != null && widgetsConfigData.Length > 0)
                {
                    foreach (dynamic data in widgetsConfigData)
                    {
                        if (string.IsNullOrEmpty(data.widgetId))
                            errorMessage = RtpnlConstants.MSG_ErrorForSaveUpdateWidgetConfig;
                        else if (string.IsNullOrEmpty(data.widgetName))
                            errorMessage = RtpnlConstants.MSG_Error_Widget_Name_Cannot_Empty;
                        else if (data.widgetName.Length > 100)
                            errorMessage = RtpnlConstants.MSG_Error_Widget_Name_Too_Large;
                        //TODO: Now We have handled the duplicate logic on view level
                        //else if (_configurationDetails.IsWidgetNameDuplicate(message.CompanyUserID, data))
                        //    errorMessage = RtpnlConstants.MSG_Error_Widget_Name_Already_Exists;
                        if (string.IsNullOrEmpty(errorMessage))
                            _configurationDetails.SaveUpdateConfigurationDetailsForUser(message.CompanyUserID, data);
                    }
                }
                if (widgetsConfigData.Length > 0)
                {
                    uniqueIdentifier = widgetsConfigData[0].viewName + "_" + widgetsConfigData[0].pageId;
                }
                var response = new { errorMessage = errorMessage, uniqueIdentifier = uniqueIdentifier };
                message.Data = JsonConvert.SerializeObject(response);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse, message);
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SAVE_UPDATE_CONFIG_DETAILS_REQUEST_PROCESSED + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SaveUpdateConfigDetailsResponse);
            }
        }

        /// <summary>
        /// Save Configuration Detail for Extract request received handler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveConfigDataForExtractRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SAVE_CONFIG_FOR_EXTRACT_REQUEST_RECEIVED + message.CompanyUserID);
            string errorMessage = string.Empty;
            string uniqueIdentifier = string.Empty;
            try
            {
                dynamic data = JsonConvert.DeserializeObject<WidgetData[]>(message.Data);
                if (data == null)
                {
                    errorMessage = RtpnlConstants.MSG_ErrorForSaveUpdateWidgetConfig;
                }
                else
                {
                    foreach (dynamic newWidgetData in data)
                    {
                        _configurationDetails.SaveUpdateConfigurationDetailsForUser(message.CompanyUserID, newWidgetData);
                    }
                }
                if (data.Length > 0)
                {
                    uniqueIdentifier = data[0].viewName + "_" + data[0].pageId;
                }
                var response = new { errorMessage = errorMessage, uniqueIdentifier = uniqueIdentifier };
                message.Data = JsonConvert.SerializeObject(response);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveConfigDataForExtractResponse, message);
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SAVE_CONFIG_FOR_EXTRACT_REQUEST_PROCESSED + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SaveConfigDataForExtractResponse);
            }
        }

        /// <summary>
        /// User permitted accounts request received handler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_UserPermittedAccountsReceived(string topic, RequestResponseModel message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message.Data))
                {
                    Dictionary<int, string> _userPermittedAccounts = JsonConvert.DeserializeObject<Dictionary<int, string>>(message.Data);
                    if (!_userUnallocatedAccPermission.ContainsKey(message.CompanyUserID))
                    {
                        InformationReporter.GetInstance.Write(RtpnlConstants.MSG_USER_PERMITTED_ACCOUNTS_REQUEST_RECEIVED + message.CompanyUserID);
                        if (_allCompanyAccounts.Count == _userPermittedAccounts.Count)
                            _userUnallocatedAccPermission.Add(message.CompanyUserID, true);
                        else
                            _userUnallocatedAccPermission.Add(message.CompanyUserID, false);
                        InformationReporter.GetInstance.Write(RtpnlConstants.MSG_USER_PERMITTED_ACCOUNTS_REQUEST_PROCESSED + message.CompanyUserID);
                    }
                    else
                    {
                        if (_allCompanyAccounts.Count == _userPermittedAccounts.Count)
                            _userUnallocatedAccPermission[message.CompanyUserID] = true;
                        else
                            _userUnallocatedAccPermission[message.CompanyUserID] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Rtpnl widgets count request received handler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RtpnlWidgetCountRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlWidgetCountRequestReceived + message.CompanyUserID);
                Dictionary<string, int> widgetCount = _configurationDetails.GetWidgetCountForUser(message.CompanyUserID);

                message.Data = JsonConvert.SerializeObject(widgetCount);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlWidgetCountResponse, message);

                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlWidgetCountRequestProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Request from Frontend to check Calculation Service is Running
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_CalculationServiceRunningRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_CheckCalculationServiceRunningRequestReceived + message.CompanyUserID);

                CheckCalculationServiceRunningRequest(_isCalculationServiceStarted);

                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_CheckCalculationServiceRunningRequestProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Rtpnl widget data request received handler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RtpnlWidgetDataRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlWidgetDataRequestReceived + message.CompanyUserID);
                Dictionary<string, string> finalResponse = new Dictionary<string, string>();
                if (_isCalculationServiceStarted)
                {
                    Dictionary<string, string> startUpData = new Dictionary<string, string>();

                    startUpData.Add(RtpnlConstants.CONST_ROW_BASE_NAV_CALCULATION, RowCalculationBaseNav.GetCustomSerializedRowCalculationDictionaryData(new Dictionary<string, RowCalculationBaseNav>(_compressionsCache.DictRowCalculation)));
                    startUpData.Add(RtpnlConstants.CONST_ROW_BASE_NAV_CALCULATION_QUANTITY_ZERO, RowCalculationBaseNav.GetCustomSerializedRowCalculationDictionaryData(new Dictionary<string, RowCalculationBaseNav>(_compressionsCache.DictRowCalculationQuantityZero)));

                    finalResponse.Add(RtpnlConstants.CONST_START_UP_GRID_DATA, JsonConvert.SerializeObject(startUpData));

                    message.Data = JsonConvert.SerializeObject(finalResponse);
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlWidgetDataResponse, message);
                    InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlWidgetDataRequestProcessed + message.CompanyUserID);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Rtpnl widget data request received handler
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RtpnlConfigDataRequest(string topic, RequestResponseModel message)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlConfigWidgetDataRequestReceived + message.CompanyUserID);
                Dictionary<string, string> finalResponse = new Dictionary<string, string>();
                if (_isCalculationServiceStarted)
                {
                    Dictionary<string, string> dataobject = JsonHelper.DeserializeToObject<Dictionary<string, string>>(message.Data);
                    string pageId = string.Empty;
                    if (dataobject.ContainsKey(RtpnlConstants.CONST_PageId))
                    {
                        pageId = dataobject[RtpnlConstants.CONST_PageId];
                    }

                    List<RtpnlResponse> rtpnlResponse = _configurationDetails.GetConfigDetailsForUser(message.CompanyUserID, pageId);

                    if (rtpnlResponse != null)
                    {
                        if (_userUnallocatedAccPermission != null && _userUnallocatedAccPermission.ContainsKey(message.CompanyUserID))
                            finalResponse.Add(RtpnlConstants.CONST_UNALLOCATED_ACCOUNT_PERMISSION, JsonConvert.SerializeObject(_userUnallocatedAccPermission[message.CompanyUserID]));
                        else
                            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_UNALLOCATED_ACCOUNT_PERMISSION_NOT_AVAILABLE_ + message.CompanyUserID);

                        finalResponse.Add(RtpnlConstants.CONST_WIDGET_CONFIG_DATA, JsonConvert.SerializeObject(rtpnlResponse));

                        if (dataobject.ContainsKey(RtpnlConstants.CONST_ViewId))
                        {
                            if (dataobject[RtpnlConstants.CONST_ViewId] != "true")
                            {
                                finalResponse.Add(RtpnlConstants.CONST_ViewId, dataobject[RtpnlConstants.CONST_ViewId]);
                            }
                        }

                        List<DynamicUDA> dynamicUDAList = _dynamicUDACache.Values.ToList();
                        if (dynamicUDAList != null)
                        {
                            // Adding Dynamic UDA list
                            finalResponse.Add(RtpnlConstants.CONST_DYNAMIC_UDA_DATA, JsonConvert.SerializeObject(dynamicUDAList));
                        }

                        message.Data = JsonConvert.SerializeObject(finalResponse);
                        await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, message);
                        InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlConfigWidgetDataRequestProcessed + message.CompanyUserID);
                    }
                }
                else
                {
                    finalResponse.Add(RtpnlConstants.CONST_ERROR, JsonConvert.SerializeObject(RtpnlConstants.CONST_ERROR_MSG));
                    message.Data = JsonConvert.SerializeObject(finalResponse);
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RtpnlWidgetConfigDataResponse, message);
                    InformationReporter.GetInstance.Write(RtpnlConstants.MSG_RtpnlConfigWidgetDataRequestProcessed + message.CompanyUserID);
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                //When the task is ready, release the semaphore. It is vital to ALWAYS release the semaphore when we are ready, or else we will end up with a Semaphore that is forever locked.
                //This is why it is important to do the Release within a try...finally clause; program execution may crash or take a different path, this way you are guaranteed execution
                semaphoreSlim.Release();
            }
        }

        /// <summary>
        /// Save and Update Configuration Detail request received handler for Save Page As operation
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveConfigDetailsForSavePageAsRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SAVE_CONFIG_DETAILS_REQUEST_FOR_SAVEPAGEAS_RECEIVED + message.CompanyUserID);
            string errorMessage = string.Empty;
            try
            {
                dynamic widgetConfigDataAndOldWidgetIds = JsonConvert.DeserializeObject<WidgetConfigDataAndOldWidgetIds>(message.Data);
                if (widgetConfigDataAndOldWidgetIds.widgetConfigDetails != null && widgetConfigDataAndOldWidgetIds.widgetConfigDetails.Count > 0)
                {
                    foreach (dynamic data in widgetConfigDataAndOldWidgetIds.widgetConfigDetails)
                    {
                        if (string.IsNullOrEmpty(data.widgetId))
                            errorMessage = RtpnlConstants.MSG_ErrorForSaveUpdateWidgetConfig;
                        else if (string.IsNullOrEmpty(data.widgetName))
                            errorMessage = RtpnlConstants.MSG_Error_Widget_Name_Cannot_Empty;
                        else if (data.widgetName.Length > 100)
                            errorMessage = RtpnlConstants.MSG_Error_Widget_Name_Too_Large;

                        if (string.IsNullOrEmpty(errorMessage))
                            _configurationDetails.SaveUpdateConfigurationDetailsForUser(message.CompanyUserID, data, widgetConfigDataAndOldWidgetIds.oldWidgetId);
                    }
                }
                message.Data = JsonConvert.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveConfigDetailsForSavePageAsResponse, message);
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SAVE_CONFIG_DETAILS_REQUEST_FOR_SAVEPAGEAS_PROCESSED + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
                    _ = KafkaManager.Instance.Produce(KafkaConstants.TOPIC_UserPermittedAccountsRequest, new RequestResponseModel(companyUserId, ""));
                    Logger.LogMsg(LoggerLevel.Information, "Kafka topic UserPermittedAccountsRequest produced for user:{0}", companyUserId);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion

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

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
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
                Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
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
                Logger.LogError(ex, $"Error while producing to topic {topicName}");
            }
            catch (Exception ex2)
            {
                Logger.LogError(ex2, $"ProduceTopicNHandleException encountered an error,  message might not have been published to event {topicName}");
            }
        }

        /// <summary>
        /// Create Proxy for Dynamic UDA
        /// </summary>
        private void CreateProxy()
        {
            try
            {
                _secMasterSyncService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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
}

