using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Configuration;
using System.ServiceModel;

namespace Prana.MarketDataPermissionService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class MarketDataPermissionService : IMarketDataPermissionService
    {
        private bool _configEnableMarketDataSimulationForAutomation = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableMarketDataSimulationForAutomation"]);

        #region Constructor
        public MarketDataPermissionService()
        {
        }
        #endregion

        #region IMarketDataPermissionService Methods
        public void PermissionCheck(MarketDataPermissionRequest marketDataPermissionRequest, string source, string userDetails = null)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    switch (CachedDataManager.CompanyMarketDataProvider)
                    {
                        case BusinessObjects.AppConstants.MarketDataProvider.FactSet:
                            FactSetMarketDataPermissionDetail.PermissionCheck(marketDataPermissionRequest, source, OperationContext.Current.GetCallbackChannel<IMarketDataPermissionServiceCallback>());
                            break;
                        case BusinessObjects.AppConstants.MarketDataProvider.ACTIV:
                            ActivMarketDataPermissionDetail.PermissionCheck(marketDataPermissionRequest, source, OperationContext.Current.GetCallbackChannel<IMarketDataPermissionServiceCallback>());
                            break;
                        case BusinessObjects.AppConstants.MarketDataProvider.SAPI:
                            SapiMarketDataPermissionDetail.PermissionCheck(marketDataPermissionRequest, source, OperationContext.Current.GetCallbackChannel<IMarketDataPermissionServiceCallback>(), userDetails);
                            break;
                        default:
                            OperationContext context = OperationContext.Current;
                            System.Threading.Tasks.Task.Run(new Action(() =>
                            {
                                context.GetCallbackChannel<IMarketDataPermissionServiceCallback>().PermissionCheckResponse(marketDataPermissionRequest.CompanyUserID, true);
                            }));
                            break;
                    }
                }
                else
                {
                    OperationContext context = OperationContext.Current;
                    System.Threading.Tasks.Task.Run(new Action(() =>
                    {
                        context.GetCallbackChannel<IMarketDataPermissionServiceCallback>().PermissionCheckResponse(marketDataPermissionRequest.CompanyUserID, true);
                    }));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public void AddSubscriptionToGetPermissionFromCache(int companyUserID, string source)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    switch (CachedDataManager.CompanyMarketDataProvider)
                    {
                        case BusinessObjects.AppConstants.MarketDataProvider.FactSet:
                            FactSetMarketDataPermissionDetail.AddSubscriptionToGetPermissionFromCache(companyUserID, source, OperationContext.Current.GetCallbackChannel<IMarketDataPermissionServiceCallback>());
                            break;
                        case BusinessObjects.AppConstants.MarketDataProvider.ACTIV:
                            ActivMarketDataPermissionDetail.AddSubscriptionToGetPermissionFromCache(companyUserID, source, OperationContext.Current.GetCallbackChannel<IMarketDataPermissionServiceCallback>());
                            break;
                        case BusinessObjects.AppConstants.MarketDataProvider.SAPI:
                            SapiMarketDataPermissionDetail.AddSubscriptionToGetPermissionFromCache(companyUserID, source, OperationContext.Current.GetCallbackChannel<IMarketDataPermissionServiceCallback>());
                            break;
                        default:
                            OperationContext context = OperationContext.Current;
                            System.Threading.Tasks.Task.Run(new Action(() =>
                            {
                                context.GetCallbackChannel<IMarketDataPermissionServiceCallback>().PermissionCheckResponse(companyUserID, true);
                            }));
                            break;
                    }
                }
                else
                {
                    OperationContext context = OperationContext.Current;
                    System.Threading.Tasks.Task.Run(new Action(() =>
                    {
                        context.GetCallbackChannel<IMarketDataPermissionServiceCallback>().PermissionCheckResponse(companyUserID, true);
                    }));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
            }
        }

        public void RemoveSubscriptionToGetPermissionFromCache(int companyUserID, string source, bool isResponseRequired = true)
        {
            try
            {
                if (!_configEnableMarketDataSimulationForAutomation)
                {
                    switch (CachedDataManager.CompanyMarketDataProvider)
                    {
                        case BusinessObjects.AppConstants.MarketDataProvider.FactSet:
                            FactSetMarketDataPermissionDetail.RemoveSubscriptionToGetPermissionFromCache(companyUserID, source, isResponseRequired);
                            break;
                        case BusinessObjects.AppConstants.MarketDataProvider.ACTIV:
                            ActivMarketDataPermissionDetail.RemoveSubscriptionToGetPermissionFromCache(companyUserID, source, isResponseRequired);
                            break;
                        case BusinessObjects.AppConstants.MarketDataProvider.SAPI:
                            SapiMarketDataPermissionDetail.RemoveSubscriptionToGetPermissionFromCache(companyUserID, source, isResponseRequired);
                            break;
                        default:
                            OperationContext context = OperationContext.Current;
                            System.Threading.Tasks.Task.Run(new Action(() =>
                            {
                                context.GetCallbackChannel<IMarketDataPermissionServiceCallback>().PermissionCheckResponse(companyUserID, true);
                            }));
                            break;
                    }
                }
                else
                {
                    OperationContext context = OperationContext.Current;
                    System.Threading.Tasks.Task.Run(new Action(() =>
                    {
                        context.GetCallbackChannel<IMarketDataPermissionServiceCallback>().PermissionCheckResponse(companyUserID, true);
                    }));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    PranaAppException theFault = new PranaAppException(ex);
                    throw new FaultException<PranaAppException>(theFault, ex.Message);
                }
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
    }
}
