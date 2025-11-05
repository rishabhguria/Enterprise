using Castle.Windsor;
using Newtonsoft.Json;
using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global.Utilities;
using Prana.GreenfieldServices.Common;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LayoutService.Layout_Managers;
using Prana.LayoutService.Models;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.LayoutService
{
    /// <summary>
    /// This Class is responsible for Managing Layout Service.
    /// It's Primary job is to implement interface methods that govern the service lifecycle
    /// It also maintains connection with Auth GreenField Service to track logged in Users
    /// </summary>        
    /// <author> Karan Joshi </author> 
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class LayoutService : BaseService, ILayoutService, IDisposable
    {
        #region Variables Declaration

        private IWindsorContainer _container;

        private ServerHeartbeatManager _LayoutServiceHeartbeatManager;

        /// <summary>
        /// To determine if Auth Service connected or not
        /// </summary>
        private static bool _isAuthServiceConnected = false;

        /// <summary>
        /// Instance for ApplicationLayoutManager
        /// </summary>
        private static ApplicationLayoutManager _applicationLayoutManager;

        /// <summary>
        /// Instance for PageLayoutManager
        /// </summary>
        private static PageLayoutManager _pageLayoutManager;

        /// <summary>
        /// Instance for ViewLayoutManager
        /// </summary>
        private static ViewLayoutManager _viewLayoutManager;

        /// <summary>
        /// Tracks whether the logged-in user response has been received from Auth Service
        /// True if received; otherwise, false.
        /// </summary>
        bool _loggedInUserResponseReceivedFromAuth = false;

        private int _cleanedUp = 0;

        /// <summary>
        /// Heart beat interval from config
        /// </summary>
        private int _heartBeatInterval = int.Parse(ConfigurationManager.AppSettings["HeartBeatInterval"].ToString());


        #endregion

        #region IPranaServiceCommon Methods
        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.
            if (Interlocked.Exchange(ref _cleanedUp, 1) == 1) return;

            // 1) Stop periodic callbacks BEFORE disposing anything they use
            StopServiceHealthPollingTimer();

            // 2) Mark down (and publish a final “down”)
            UpdateServiceStatus(ServiceNameConstants.CONST_Layout_Name, ServiceNameConstants.CONST_Layout_DisplayName, false);

            Console.WriteLine("Shutting down Service...");
            Logger.LogMsg(LoggerLevel.Information, "Shutting down Service...");
            _container.Dispose();
        }

        public async System.Threading.Tasks.Task<bool> InitialiseService(IWindsorContainer container)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings[LayoutServiceConstants.MSG_KafkaPath]);

                WindsorContainerManager.Container = container;

                #region Server Heartbeat Setup
                _LayoutServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                #region Kafka-Subscription
                KafkaManager.Instance.ProducerReporterEvent += Kafka_ProducerReporter;
                KafkaManager.Instance.ConsumerReporterEvent += Kafka_ConsumerReporter;

                //This Consumer receives message when user is logged in.
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedInUserResponse, KafkaManager_InitializeLoggedInUsers);

                //This Consumer receives message when user is logged out.
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_InitializeLoggedOutUserResponse, KafkaManager_InitializeLoggedOutUsers);

                //This Method is a centeral method to handle openfin RTPnl's page and it's views.
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoadViewsRequest, KafkaManager_GetUserSpecificPageLayout);

                //This Method is a central method to handle saving of RTPnl's page and it's views.
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveOrUpdateViewsRequest, KafkaManager_SaveUserSpecificPageLayout);

                //This Method is a central method to delete data from Page , views and widgets 
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteOpenfinPageRequest, KafkaManager_DeleteUserSpecificPageLayout);

                //This Method is used to remove all the pages for an user
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RemovePagesForAnUser, KafkaManager_RemovePagesForAnUser);

                //Getting the instances of the layout managers
                _applicationLayoutManager = ApplicationLayoutManager.GetInstance();

                _pageLayoutManager = PageLayoutManager.GetInstance();

                _viewLayoutManager = ViewLayoutManager.GetInstance();

                #endregion

                Logger.LogMsg(LoggerLevel.Information, "{0}", "**** Service started successfully ****");
                StartServiceHealthPollingTimer(ProduceServiceStatusMessage, _heartBeatInterval);
                // fire and forget method for handling of auth service connection
                ServiceConnectionPoller.PollUntilServiceReady(() => _loggedInUserResponseReceivedFromAuth, KafkaConstants.TOPIC_InitializeLoggedInUserRequest, ServiceNameConstants.CONST_Auth_DisplayName, false);

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                    "InitialiseService Completed for Layout Service in {0} ms.", sw.ElapsedMilliseconds);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitialiseService encountered an error");
            }

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
                    if (_LayoutServiceHeartbeatManager != null)
                        _LayoutServiceHeartbeatManager.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }
        #endregion

        #region IContainerService Methods

        public async System.Threading.Tasks.Task<List<HostedService>> GetClientServicesStatus()
        {
            try
            {
                List<HostedService> hostedServicesStatus = new List<HostedService>();
                var taskList = new List<System.Threading.Tasks.Task>()
                {
                };

                await System.Threading.Tasks.Task.WhenAll(taskList);

                return hostedServicesStatus;
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
                else if (_LayoutServiceHeartbeatManager != null && !isRetryRequest)
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

        #region Private Methods

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
        /// Publishes the current service status to a Kafka topic.  
        /// This method retrieves the service status, serializes it, and sends it to the Kafka topic  
        /// specified in the KafkaConstants.TOPIC_ServiceHealthStatus.  
        /// </summary>  
        private async void ProduceServiceStatusMessage()
        {
            try
            {
                UpdateServiceStatus(ServiceNameConstants.CONST_Layout_Name, ServiceNameConstants.CONST_Layout_DisplayName, true);
                var serviceStatus = GetServiceStatus(ServiceNameConstants.CONST_Layout_Name);
                var message = new RequestResponseModel(0, JsonConvert.SerializeObject(serviceStatus));
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_ServiceHealthStatus, message);

                Logger.LogMsg(LoggerLevel.Verbose, "Service status published to Kafka topic {0}", KafkaConstants.TOPIC_ServiceHealthStatus);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ProduceServiceStatusMessage encountered an error");
            }
        }

        #endregion

        #region Kafka reporter
        private void Kafka_ProducerReporter(string topic)
        {
            Logger.LogMsg(LoggerLevel.Verbose, "Producing to {0}", topic);
        }

        private void Kafka_ConsumerReporter(string topic)
        {
            Logger.LogMsg(LoggerLevel.Verbose, KafkaLoggingConstants.MSG_KAFKA_CONSUMER, topic);
        }
        #endregion

        #region Kafka Methods

        #region Handling of User Login/Logout Session and Auth Service Connectivity

        /// <summary>
        /// KafkaManager_InitializeLoggedInUsers
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="request"></param>
        private async void KafkaManager_InitializeLoggedInUsers(string topic, RequestResponseModel request)
        {
            try
            {
                if (!_loggedInUserResponseReceivedFromAuth)
                {
                    _loggedInUserResponseReceivedFromAuth = true;
                    Logger.LogMsg(LoggerLevel.Information, "Successfully connected Auth Service.");
                }

                Dictionary<int, AuthenticatedUserInfo> _loggedInUsers = JsonHelper.DeserializeToObject<Dictionary<int, AuthenticatedUserInfo>>(request.Data);

                //Looping on the data received from Auth service regarding user logged in . 
                foreach (var kvp in _loggedInUsers)
                {
                    if (_loggedInUsers[kvp.Key] != null)
                    {
                        int companyUserID = kvp.Key;

                        //TODO : Calling of methods to handle Application level , page level , view level cache will be done here on user login.
                        _applicationLayoutManager.LoadLayoutForLoggedInUser(companyUserID);

                        _pageLayoutManager.LoadLayoutForLoggedInUser(companyUserID);

                        _viewLayoutManager.LoadLayoutForLoggedInUser(companyUserID);
                    }

                    // Awaiting for a completed task to make function asynchronous
                    await System.Threading.Tasks.Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// KafkaManager_InitializeLoggedOutUsers
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void KafkaManager_InitializeLoggedOutUsers(string topic, RequestResponseModel message)
        {
            try
            {
                int companyUserID = message.CompanyUserID;
                InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_CacheClearRequestReceived + companyUserID.ToString());

                #region Clear cache for logged out user
                //TODO :  will be using this region later to clear out the caches created inside this service on user logout .
                _applicationLayoutManager.RemoveLayoutForLoggedOutUser(companyUserID);

                _pageLayoutManager.RemoveLayoutForLoggedOutUser(companyUserID);

                _viewLayoutManager.RemoveLayoutForLoggedOutUser(companyUserID);
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        /// <summary>
        /// This method will be called to send the collected pages and views data for RTPnl .
        /// This will individually call the methods to get pages and views from it's layout Managers and respond will one array object 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_GetUserSpecificPageLayout(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_LoadViewsRequestedBy + message.CompanyUserID);
                    Logger.LoggerWrite(LayoutServiceConstants.MSG_LoadViewsRequestedBy + message.CompanyUserID + JsonConvert.SerializeObject(message));

                    //Getting the users page details 
                    Dictionary<string, PageInfo> userPages = _pageLayoutManager.GetUserSpecificPageLayout(message.CompanyUserID);

                    // Getting the users view details.
                    Dictionary<string, ViewInfo> userViews = _viewLayoutManager.GetUserSpecificViewLayout(message.CompanyUserID);

                    // Convert dictionary values to lists
                    List<PageInfo> pageList = userPages.Values.ToList();
                    List<ViewInfo> viewList = userViews.Values.ToList();

                    RtpnlLayoutInfoResponse rtpnlLayoutInfoResponse = new RtpnlLayoutInfoResponse
                    {
                        Pages = pageList,
                        Views = viewList
                    };
                    // Serialize the combined object
                    message.Data = JsonConvert.SerializeObject(rtpnlLayoutInfoResponse);
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LoadViewsResponse, message);

                    Logger.LogMsg(LoggerLevel.Information, "GetUserSpecificPageLayout (Load view) request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_LoadViewsResponse);
                }
            }
        }

        /// <summary>
        /// This method saves the pages and it's views in the database .
        /// Takes Page and View Info and calls the respective layout managers to save the data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveUserSpecificPageLayout(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_SaveViewsRequestedBy + message.CompanyUserID);

                    string errorMessage = string.Empty;

                    RtpnlLayoutInfo layoutDetails = JsonConvert.DeserializeObject<RtpnlLayoutInfo>(message.Data);
                    WidgetConfigAndOldWidgetIds widgetConfigAndOldWidgetIds = JsonConvert.DeserializeObject<WidgetConfigAndOldWidgetIds>(message.Data);

                    if (layoutDetails != null)
                    {
                        PageInfo pageInfo = layoutDetails.pageInfo;
                        List<ViewInfo> internalPageInfoList = layoutDetails.internalPageInfo;

                        //saving the page data in DB and cache
                        errorMessage = _pageLayoutManager.SaveUserSpecificPageLayout(message.CompanyUserID, pageInfo);

                        if (String.IsNullOrEmpty(errorMessage))
                        {
                            //saving the view data in DB and cache
                            errorMessage = _viewLayoutManager.SaveUserSpecificViewLayout(message.CompanyUserID, internalPageInfoList);
                        }
                    }

                    var response = new { errorMessage = errorMessage, pageInfo = layoutDetails };
                    message.Data = JsonConvert.SerializeObject(response);
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveOrUpdateViewsResponse, message);

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        if (widgetConfigAndOldWidgetIds.oldWidgetId != null && widgetConfigAndOldWidgetIds.oldWidgetId.Count != 0 && widgetConfigAndOldWidgetIds.widgetConfigDetails != null && widgetConfigAndOldWidgetIds.widgetConfigDetails.Count != 0)
                        {
                            RequestResponseModel requestResponseObj1 = new RequestResponseModel(message.CompanyUserID, JsonConvert.SerializeObject(widgetConfigAndOldWidgetIds));
                            await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveConfigDetailsForSavePageAsRequest, requestResponseObj1);
                        }

                    }

                    Logger.LogMsg(LoggerLevel.Information, "SaveUserSpecificPageLayout request processed successfully in {0} ms", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SaveOrUpdateViewsResponse);
                }
            }
        }

        /// <summary>
        /// This method is responsible for deleting the pages , views and widget data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_DeleteUserSpecificPageLayout(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_DeleteOpenfinPageInformationReceived + message.CompanyUserID);

                    Logger.LoggerWrite(LayoutServiceConstants.MSG_DeleteOpenfinPageInformationReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));

                    string errorMessage = string.Empty;
                    DeletePageDTO jsonDataObject = JsonConvert.DeserializeObject<DeletePageDTO>(message.Data);
                    try
                    {
                        string pageId = jsonDataObject.pageId;
                        string commaSeparatedViewIds = string.Join(",", jsonDataObject.viewIds);
                        object[] parameters = new object[3];
                        parameters[0] = message.CompanyUserID;
                        parameters[1] = pageId;
                        parameters[2] = commaSeparatedViewIds;

                        // object to log the deleted page information
                        object[] logParameters = new object[3];
                        logParameters[0] = message.CompanyUserID;
                        logParameters[1] = pageId;
                        logParameters[2] = jsonDataObject.title;

                        // Deleting the Page information from DB
                        DatabaseManager.DatabaseManager.ExecuteNonQuery(LayoutServiceConstants.CONST_P_Samsara_DeleteOpenfinPage, parameters);
                        // Logging the deleted page information in DB
                        DatabaseManager.DatabaseManager.ExecuteNonQuery(LayoutServiceConstants.CONST_P_Samsara_LogDeletedPageForUser, logParameters);

                        Logger.LoggerWrite(LayoutServiceConstants.MSG_DeleteOpenfinPageInformationReceived + message.CompanyUserID + "::" + jsonDataObject.pageId);

                        //Now removing the user related data ( pages , views ) from layout managers respective cache's
                        _pageLayoutManager.DeleteUserSpecificPageLayout(message.CompanyUserID, pageId);
                        _viewLayoutManager.DeleteUserSpecificViewLayout(message.CompanyUserID, jsonDataObject.viewIds);

                        // Removing the deleted page from workspace as well if it exists
                        // This block is no longer needed, as workspace handling is now managed on the frontend to support merged page scenarios.
                        // _applicationLayoutManager.RemoveDeletedPageDataFromlayout(topic, pageId, message);

                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    var response = new { errorMessage = errorMessage, pageInfo = jsonDataObject };
                    message.Data = JsonConvert.SerializeObject(response);
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_DeleteOpenfinPageResponse, message);
                    Logger.LoggerWrite(LayoutServiceConstants.MSG_DeleteOpenfinPageInformationProcessed + message.CompanyUserID + JsonConvert.SerializeObject(message));
                    InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_DeleteOpenfinPageInformationProcessed + message.CompanyUserID);

                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_DeleteOpenfinPageResponse);
                }
            }
        }

        /// <summary>
        /// This method is responsible for deleting the pages , views and widget data
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_RemovePagesForAnUser(string topic, RequestResponseModel message)
        {
            using (LoggerHelper.PushLoggingProperties(message.CorrelationId, message.RequestID, message.CompanyUserID))
            {
                try
                {
                    int companyUserID = message.CompanyUserID;
                    Object[] parameter = new Object[2];
                    parameter[0] = companyUserID;
                    parameter[1] = message.Data;

                    var x = DatabaseManager.DatabaseManager.ExecuteNonQuery(LayoutServiceConstants.CONST_P_Samsara_RemovePagesForAnUser, parameter);

                    #region Clear cache for logged out user

                    _applicationLayoutManager.RemoveLayoutForLoggedOutUser(companyUserID);
                    _pageLayoutManager.RemoveLayoutForLoggedOutUser(companyUserID);
                    _viewLayoutManager.RemoveLayoutForLoggedOutUser(companyUserID);

                    #endregion
                }
                catch (Exception ex)
                {
                    await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_DeleteOpenfinPageResponse);
                }
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
    }
}
