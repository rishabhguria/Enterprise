using Prana.LogManager;
using System;
using System.ServiceModel;

namespace Prana.WCFConnectionMgr
{
    public class ProxyBase<T> : Proxy<T>
    {
        public ProxyBase(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        protected override T CreateChannel(string endpointConfigurationName)
        {
            try
            {
                m_innerChannel = new ChannelFactory<T>(endpointConfigurationName).CreateChannel();
                base.SetOperationTimeout(m_innerChannel);
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
            return m_innerChannel;
        }
    }
}
