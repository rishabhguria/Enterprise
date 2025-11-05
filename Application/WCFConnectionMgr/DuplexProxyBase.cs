using Prana.LogManager;
using System;
using System.ServiceModel;

namespace Prana.WCFConnectionMgr
{
    public class DuplexProxyBase<T> : Proxy<T>
    {
        Object _callBackChannel;
        public DuplexProxyBase(string clientName, Object callBackChannel)
            : base(clientName)
        {
            _callBackChannel = callBackChannel;
        }

        protected override T CreateChannel(string endpointConfigurationName)
        {
            try
            {
                if (_callBackChannel != null)
                {
                    m_innerChannel = new DuplexChannelFactory<T>(new InstanceContext(_callBackChannel), endpointConfigurationName).CreateChannel();
                    base.SetOperationTimeout(m_innerChannel);
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
            return m_innerChannel;
        }

        protected override void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    _callBackChannel = null;
                }
                base.Dispose(isDisposing);
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
    }
}