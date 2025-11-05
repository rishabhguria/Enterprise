using Castle.Windsor;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.CoreService.Interfaces;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LogManager;
using Prana.WatchListData;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;

namespace Prana.WatchlistDataService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class WatchlistDataService : IWatchlistDataService, IDisposable
    {
        #region Variables
        private IWindsorContainer _container;
        private Dictionary<int, WatchlistDataManager> _userWiseWatchlistData = new Dictionary<int, WatchlistDataManager>();
        private ServerHeartbeatManager _WatchlistDataServiceHeartbeatManager;
        #endregion

        #region Private Methods

        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                string[] messageList = message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                //PublishPreparation(Topics.Topic_WatchlistDataServiceLogsData, messageList.ToList());
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
                InformationReporter.GetInstance.InformationReceived += new InformationReporter.InformationReceivedHandler(GetInstance_InformationReceived);
                this._container = container;

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistTabNamesRequest, KafkaManager_MessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistTabWiseSymbolsRequest, KafkaManager_MessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistAddTabRequest, KafkaManager_MessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistRenameTabRequest, KafkaManager_MessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistDeleteTabRequest, KafkaManager_MessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistAddSymbolInTabRequest, KafkaManager_MessageReceived);

                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_WatchlistRemoveSymbolFromTabRequest, KafkaManager_MessageReceived);

                #region Server Heartbeat Setup
                _WatchlistDataServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                Logger.LogMsg(LoggerLevel.Information,"{0}", "**** Service started successfully ****");

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                    "InitialiseService Completed for WatchlistDataService in {0} ms.", sw.ElapsedMilliseconds);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitialiseService encountered an error");
            }

            return false;
        }

        private WatchlistDataManager GetWatchListDataManager(int userId)
        {
            if (!_userWiseWatchlistData.ContainsKey(userId))
                _userWiseWatchlistData.Add(userId, new WatchlistDataManager(userId));
            return _userWiseWatchlistData[userId];
        }

        private async void KafkaManager_MessageReceived(string topic, RequestResponseModel message)
        {
            WatchlistDataManager watchlistDataManager = GetWatchListDataManager(message.CompanyUserID);
            string[] data;
            switch (topic)
            {
                case KafkaConstants.TOPIC_WatchlistTabNamesRequest:
                    message.Data = JsonHelper.SerializeObject(watchlistDataManager.GetAllTabNames());
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_WatchlistTabNamesResponse, message);
                    break;
                case KafkaConstants.TOPIC_WatchlistTabWiseSymbolsRequest:
                    message.Data = JsonHelper.SerializeObject(watchlistDataManager.GetTabWiseSymbols());
                    await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_WatchlistTabWiseSymbolsResponse, message);
                    break;
                case KafkaConstants.TOPIC_WatchlistAddTabRequest:
                    watchlistDataManager.AddTab(message.Data);
                    break;
                case KafkaConstants.TOPIC_WatchlistRenameTabRequest:
                    data = JsonHelper.DeserializeToObject<string[]>(message.Data);
                    watchlistDataManager.RenameTab(data[0], data[1]);
                    break;
                case KafkaConstants.TOPIC_WatchlistDeleteTabRequest:
                    watchlistDataManager.DeleteTab(message.Data);
                    break;
                case KafkaConstants.TOPIC_WatchlistAddSymbolInTabRequest:
                    data = JsonHelper.DeserializeToObject<string[]>(message.Data);
                    watchlistDataManager.AddSymbolInTab(data[0], data[1]);
                    break;
                case KafkaConstants.TOPIC_WatchlistRemoveSymbolFromTabRequest:
                    data = JsonHelper.DeserializeToObject<string[]>(message.Data);
                    watchlistDataManager.RemoveSymbolFromTab(data[0], data[1]);
                    break;
            }
        }

        public void CleanUp()
        {
            // Perform any last minute clean here.
            // Note: Please add light functions only.
            Logger.LogMsg(LoggerLevel.Fatal, "{0}", "Shutting down service.");
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
                    // Subscriber added successfully
                    return true;
                }
                else if (_WatchlistDataServiceHeartbeatManager != null && !isRetryRequest)
                {
                    // Subscribe failed
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Subscribe method encountered an error");
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
                Logger.LogError(ex, "UnSubscribe method encountered an error");
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
                Logger.LogError(ex, "RequestStartupData method encountered an error");
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
                Logger.LogError(ex, "OpenLog method encountered an error");
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
                Logger.LogError(ex, "LoadLog method encountered an error");
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        public async System.Threading.Tasks.Task StopService()
        {
            try
            {
                // Awaiting for a completed task to make function asynchronous
                Logger.LogMsg(LoggerLevel.Information, "Stop Service requested...");
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
                //IWatchlistDataCallback WatchlistDataConnectionStatusObject = new WatchlistDataConnectionStatus();

                var taskList = new List<System.Threading.Tasks.Task>()
                {
                };

                await System.Threading.Tasks.Task.WhenAll(taskList);

                return hostedServicesStatus;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetClientServicesStatus encountered an error");
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
                Logger.LogError(ex, "GetDebugModeStatus encountered an error");
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
                    if (_WatchlistDataServiceHeartbeatManager != null)
                        _WatchlistDataServiceHeartbeatManager.Dispose();
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
