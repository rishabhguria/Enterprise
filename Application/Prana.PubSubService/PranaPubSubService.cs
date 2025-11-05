using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;
using System.ServiceModel;

namespace Prana.PubSubService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class PranaPubSubService
    {
        #region Pub Sub Service Hosting
        private static void HostSubscriptionService()
        {
            try
            {
                PranaServiceHost.HostPranaService(Subscription.getInstance());
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

        private static void HostPublishService()
        {
            try
            {
                PranaServiceHost.HostPranaService(Publishing.getInstance());
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

        public static void Initialize()
        {
            try
            {
                HostSubscriptionService();
                HostPublishService();
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
    }
}
