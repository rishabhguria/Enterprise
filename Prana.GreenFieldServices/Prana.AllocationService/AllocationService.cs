using Castle.Windsor;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Constants;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.KafkaWrapper;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;

namespace Prana.AllocationService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class AllocationService : IAllocationService, IDisposable
    {
        #region Variables
        private IWindsorContainer _container;
        //private bool _isPricingServiceConnected = false;

        private ServerHeartbeatManager _AllocationServiceHeartbeatManager;

        private ClientHeartbeatManager<IPricingService2> _pricingService2ClientHeartbeatManager;
        private ClientHeartbeatManager<ITradeService> _tradeServiceClientHeartbeatManager;
        #endregion

        #region Private Methods
        private void PricingService2ClientHeartbeatManager_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                //_isPricingServiceConnected = true;
                //PublishPreparation(Topics.Topic_TradeServicePricingConnectionData, _isPricingServiceConnected);
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
                //_isPricingServiceConnected = false;
                //PublishPreparation(Topics.Topic_TradeServicePricingConnectionData, _isPricingServiceConnected);
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

                //PublishPreparation(Topics.Topic_AllocationServiceLogsData, message);
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
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void TradeServiceClientHeartbeatManager_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
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
                List<string> message = new List<string>();
                message.Add(DateTime.Now.ToString("M/d/yyyy hh:mm:ss tt") + " : " + string.Format(PranaMessageConstants.MSG_AnotherInstanceSubscribed, e.Value, e.Value2));

                //PublishPreparation(Topics.Topic_AllocationServiceLogsData, message);
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

        private void GetInstance_InformationReceived(object sender, LoggingEventArgs<string> e)
        {
            try
            {
                string message = e.Value;
                string[] messageList = message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                //PublishPreparation(Topics.Topic_AllocationServiceLogsData, messageList.ToList());
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

                KafkaManager.Instance.Initialize(ConfigurationManager.AppSettings["KafkaConfigPath"]);

                #region Server Heartbeat Setup
                _AllocationServiceHeartbeatManager = new ServerHeartbeatManager();
                #endregion

                Logger.LogMsg(LoggerLevel.Information,"{0}", "**** Service started successfully ****");

                // Awaiting for a completed task to make function asynchronous
                await System.Threading.Tasks.Task.CompletedTask;

                Logger.LogMsg(LoggerLevel.Information,
                    "InitialiseService Completed for Allocation service in {0} ms.", sw.ElapsedMilliseconds);

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

            Logger.LogMsg(LoggerLevel.Information, "{0}", "Shutting down Service...");

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
                else if (_AllocationServiceHeartbeatManager != null && !isRetryRequest)
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
                //IAllocationCallback AllocationConnectionStatusObject = new AllocationConnectionStatus();

                var taskList = new List<System.Threading.Tasks.Task>()
                {
                    //    System.Threading.Tasks.Task.Run(async () =>
                    //    {
                    //        try
                    //        {
                    //            var proxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", AllocationConnectionStatusObject);
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
                    if (_pricingService2ClientHeartbeatManager != null)
                        _pricingService2ClientHeartbeatManager.Dispose();

                    if (_tradeServiceClientHeartbeatManager != null)
                        _tradeServiceClientHeartbeatManager.Dispose();

                    if (_AllocationServiceHeartbeatManager != null)
                        _AllocationServiceHeartbeatManager.Dispose();
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
