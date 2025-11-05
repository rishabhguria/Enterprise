using Prana.CoreService.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Prana.WCFConnectionMgr
{
    public class ServicesHeartbeatSubscribersCollection
    {
        private static ServicesHeartbeatSubscribersCollection _servicesHeartbeatSubscribersCollection;
        private static readonly object _locker = new object();

        public ConcurrentDictionary<string, IServiceStatusCallback> Subscribers = new ConcurrentDictionary<string, IServiceStatusCallback>();

        public static ServicesHeartbeatSubscribersCollection GetInstance()
        {
            if (_servicesHeartbeatSubscribersCollection == null)
            {
                lock (_locker)
                {
                    if (_servicesHeartbeatSubscribersCollection == null)
                    {
                        _servicesHeartbeatSubscribersCollection = new ServicesHeartbeatSubscribersCollection();
                    }
                }
            }
            return _servicesHeartbeatSubscribersCollection;
        }

        private ServicesHeartbeatSubscribersCollection()
        {
        }

        public event EventHandler SubscribersUpdated;

        public bool AddSubscriber(string subscriberName, IServiceStatusCallback callbackReference)
        {
            try
            {
                if (Subscribers.TryAdd(subscriberName, callbackReference))
                {
                    if (SubscribersUpdated != null)
                        SubscribersUpdated(this, null);

                    return true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public bool RemoveSubscriber(string subscriberName)
        {
            try
            {
                IServiceStatusCallback callbackReference;

                if (Subscribers.TryRemove(subscriberName, out callbackReference))
                {
                    if (SubscribersUpdated != null)
                        SubscribersUpdated(this, null);

                    return true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public List<string> GetSubscribersNames()
        {
            return Subscribers.Keys.ToList();
        }
    }
}
