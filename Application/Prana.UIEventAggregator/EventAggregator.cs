using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prana.UIEventAggregator
{
    /// <summary>
    /// Central point or aggregate events and trow to other UI, reduces coupling among classes
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// The _event subsribers
        /// </summary>
        private Dictionary<Type, List<WeakReference>> _eventSubsribers = new Dictionary<Type, List<WeakReference>>();
        /// <summary>
        /// The _lock subscriber dictionary
        /// </summary>
        private readonly object _lockSubscriberDictionary = new object();
        /// <summary>
        /// The _sync lock
        /// </summary>
        private static readonly object _syncLock = new object();
        /// <summary>
        /// The _instance
        /// </summary>
        private static EventAggregator _instance;

        private Dictionary<WeakReference, SynchronizationContext> _subscriberSynchronizationContexts = new Dictionary<WeakReference, SynchronizationContext>();
        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public static EventAggregator GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EventAggregator();
                        }
                    }
                }
                return _instance;
            }
        }

        #region IEventAggregator Members

        /// <summary>
        /// Publishes the event.
        /// </summary>
        /// <typeparam name="TEventType">The type of the event type.</typeparam>
        /// <param name="eventToPublish">The event to publish.</param>
        public void PublishEvent<TEventType>(TEventType eventToPublish)
        {
            try
            {
                var subsriberType = typeof(IEventAggregatorSubscriber<>).MakeGenericType(typeof(TEventType));
                var subscribers = GetSubscriberList(subsriberType);
                List<WeakReference> subsribersToBeRemoved = new List<WeakReference>();
                foreach (var weakSubsriber in subscribers)
                {
                    if (weakSubsriber.IsAlive)
                    {
                        InvokeSubscriberEvent<TEventType>(eventToPublish, weakSubsriber);
                    }
                    else
                    {
                        subsribersToBeRemoved.Add(weakSubsriber);
                    }
                }
                if (subsribersToBeRemoved.Any())
                {
                    lock (_lockSubscriberDictionary)
                    {
                        foreach (var remove in subsribersToBeRemoved)
                        {
                            subscribers.Remove(remove);
                            if (_subscriberSynchronizationContexts.ContainsKey(remove))
                            {
                                _subscriberSynchronizationContexts.Remove(remove);
                            }
                        }
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

        /// <summary>
        /// Subsribes the event.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="syncContext"></param>
        public void SubsribeEvent(object subscriber, SynchronizationContext syncContext)
        {
            try
            {
                lock (_lockSubscriberDictionary)
                {
                    var subsriberTypes = subscriber.GetType().GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventAggregatorSubscriber<>));
                    WeakReference weakReference = new WeakReference(subscriber);
                    foreach (var subsriberType in subsriberTypes)
                    {
                        List<WeakReference> subscribers = GetSubscriberList(subsriberType);
                        subscribers.Add(weakReference);
                    }
                    if (_subscriberSynchronizationContexts.ContainsKey(weakReference))
                    {
                        _subscriberSynchronizationContexts[weakReference] = syncContext;
                    }
                    else
                    {
                        _subscriberSynchronizationContexts.Add(weakReference, syncContext);
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

        #endregion IEventAggregator Members

        /// <summary>
        /// Invokes the subscriber event.
        /// </summary>
        /// <typeparam name="TEventType">The type of the event type.</typeparam>
        /// <param name="eventToPublish">The event to publish.</param>
        /// <param name="weakSubscriber"></param>
        private void InvokeSubscriberEvent<TEventType>(TEventType eventToPublish, WeakReference weakSubscriber)
        {
            try
            {
                var subscriber = (IEventAggregatorSubscriber<TEventType>)weakSubscriber.Target;
                SynchronizationContext syncContext = GetSubscriberSynchonizationContext(weakSubscriber);
                syncContext.Post(s => subscriber.OnEventHandler(eventToPublish), null);
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

        private SynchronizationContext GetSubscriberSynchonizationContext(WeakReference weakReference)
        {
            SynchronizationContext syncContext = null;
            try
            {
                if (_subscriberSynchronizationContexts.ContainsKey(weakReference))
                {
                    syncContext = _subscriberSynchronizationContexts[weakReference];
                }
                if (syncContext == null)
                {
                    syncContext = new SynchronizationContext();
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
            return syncContext;
        }

        /// <summary>
        /// Gets the subscriber list.
        /// </summary>
        /// <param name="subsriberType">Type of the subsriber.</param>
        /// <returns></returns>
        private List<WeakReference> GetSubscriberList(Type subsriberType)
        {
            List<WeakReference> subsribersList = null;
            try
            {
                lock (_lockSubscriberDictionary)
                {
                    bool found = this._eventSubsribers.TryGetValue(subsriberType, out subsribersList);
                    if (!found)
                    {
                        subsribersList = new List<WeakReference>();
                        this._eventSubsribers.Add(subsriberType, subsribersList);
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
            return subsribersList;
        }
    }
}