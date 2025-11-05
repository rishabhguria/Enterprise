using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace Prana.WCFConnectionMgr
{
    public abstract class Proxy<T> : IDisposable, IConnectionNotify
    {
        public delegate void ConnectionEventHandler(object sender, EventArgs e);
        public event ConnectionEventHandler DisconnectedEvent;
        public event ConnectionEventHandler ConnectedEvent;

        private bool isDisposed = true;
        private readonly object m_channelLock = new object();
        private readonly object _subscribeLock = new object();

        protected T m_innerChannel = default(T);

        protected string _endpointConfigurationName;
        protected abstract T CreateChannel(string clientName);

        public Proxy(string endpointConfigurationName)
        {
            try
            {
                lock (m_channelLock)
                {
                    _endpointConfigurationName = endpointConfigurationName;
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
        }

        public void SetOperationTimeout(object innerChannel)
        {
            if (innerChannel is IContextChannel)
            {
                ((IContextChannel)innerChannel).OperationTimeout = TimeSpan.FromDays(1);
            }
        }

        public bool IsContainerServiceConnected()
        {
            if (ConnectionMgr.GetServiceConnectionStatus(_endpointConfigurationName) == PranaInternalConstants.ConnectionStatus.CONNECTED)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public T InnerChannel
        {
            get
            {
                try
                {
                    GetInnerChannel();
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return m_innerChannel;
            }
        }

        private void GetInnerChannel()
        {
            try
            {
                lock (m_channelLock)
                {
                    // if the channel is not null check if it is not faulted . if so close it 
                    if (!object.Equals(m_innerChannel, default(T)))
                    {
                        ICommunicationObject channelObject = m_innerChannel as ICommunicationObject;
                        if (channelObject.State == CommunicationState.Faulted || channelObject.State == CommunicationState.Closed)
                        {
                            //set channel to Null
                            m_innerChannel = default(T);
                        }
                    }

                    //If Channel id null / Not Set Create it
                    if (object.Equals(m_innerChannel, default(T)))
                    {
                        //set channel to Null
                        isDisposed = false;
                        m_innerChannel = CreateChannel(_endpointConfigurationName);
                        ConnectionMgr.CreateConnection(_endpointConfigurationName, this);
                        SetOperationTimeout(m_innerChannel);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        Dictionary<string, List<FilterData>> topicList = new Dictionary<string, List<FilterData>>();
        public void Subscribe(string topic, List<FilterData> filters)
        {
            try
            {
                if (!topicList.ContainsKey(topic))
                {
                    topicList.Add(topic, filters);
                }
                lock (_subscribeLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            if (InnerChannel is ISubscription)
                            {
                                ((ISubscription)InnerChannel).Subscribe(topic, filters);
                            }
                        }
                        catch
                        {
                        }
                    });
                }
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

        public void UnSubscribe()
        {
            try
            {
                if (topicList != null && topicList.Count > 0 && InnerChannel is ISubscription)
                {
                    foreach (KeyValuePair<string, List<FilterData>> topic in topicList)
                    {
                        try
                        {
                            ((ISubscription)InnerChannel).UnSubscribe(topic.Key);
                        }
                        catch
                        {
                        }
                    }
                }
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

        private void ReSubscribe()
        {
            try
            {
                if (topicList != null && topicList.Count > 0 && InnerChannel is ISubscription)
                {
                    foreach (KeyValuePair<string, List<FilterData>> topic in topicList)
                    {
                        try
                        {
                            ((ISubscription)InnerChannel).Subscribe(topic.Key, topic.Value);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
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

        #region IConnectionNotify Members
        public void Notify(PranaInternalConstants.ConnectionStatus status)
        {
            try
            {
                if (status == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    ReSubscribe();

                    if (ConnectedEvent != null)
                    {
                        ConnectedEvent(this, new EventArgs());
                    }
                }
                else if (status == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
                {
                    if (DisconnectedEvent != null)
                    {
                        DisconnectedEvent(this, new EventArgs());
                    }

                    ReleaseInternalChannel();
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
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    WaitCallback callBackHandler = new WaitCallback(DisposeAsync);
                    ThreadPool.QueueUserWorkItem(callBackHandler, null);
                }
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

        private readonly object _disposeLocker = new object();
        private void DisposeAsync(object param)
        {
            lock (_disposeLocker)
            {
                ReleaseInternalChannel();
            }
        }

        private void ReleaseInternalChannel()
        {
            try
            {
                if (!isDisposed)
                {
                    //close the channel set set the channel to null
                    ICommunicationObject channelObject = m_innerChannel as ICommunicationObject;
                    if (channelObject != null)
                    {
                        UnSubscribe();
                        m_innerChannel = default(T);
                        channelObject.Abort();
                    }
                    isDisposed = true;
                }
            }
            // no need to throw exception if it is proxy exception
            catch (ProxyException)
            {
            }
            // no need to throw exception if it is CommunicationException raised when Closeing the channelObject becouse it's alredy aborted
            catch (CommunicationException)
            {
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
        #endregion
    }
}