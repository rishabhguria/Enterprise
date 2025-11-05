using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.PubSubService
{
    static class SubscriberCollection
    {
        private static readonly object _syncRoot = new object();
        private static Dictionary<string, List<SubscriberData>> _subscribersList = new Dictionary<string, List<SubscriberData>>();
        public static Dictionary<string, List<SubscriberData>> SubscribersList
        {
            get
            {
                lock (_syncRoot)
                {
                    return _subscribersList;
                }
            }

        }

        public static List<SubscriberData> GetSubscribers(String topicName)
        {
            lock (_syncRoot)
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    return SubscribersList[topicName];
                }
                else
                    return null;
            }
        }

        public static void AddSubscriber(String topicName, SubscriberData subscriberCallbackReference)
        {
            try
            {
                lock (_syncRoot)
                {
                    if (SubscribersList.ContainsKey(topicName))
                    {
                        if (!SubscribersList[topicName].Contains(subscriberCallbackReference))
                        {
                            SubscribersList[topicName].Add(subscriberCallbackReference);
                        }
                    }
                    else
                    {
                        List<SubscriberData> newSubscribersList = new List<SubscriberData>();
                        newSubscribersList.Add(subscriberCallbackReference);
                        SubscribersList.Add(topicName, newSubscribersList);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    List<SubscriberData> newSubscribersList = new List<SubscriberData>();
                    newSubscribersList.Add(subscriberCallbackReference);
                    SubscribersList.Add(topicName, newSubscribersList);
                    throw;
                }
            }
        }

        public static void RemoveSubscriber(SubscriberData subscriberCallbackReference)
        {
            lock (_syncRoot)
            {
                foreach (KeyValuePair<string, List<SubscriberData>> data in _subscribersList)
                {
                    data.Value.Remove(subscriberCallbackReference);
                }
            }
        }

        public static void RemoveSubscriber(String topicName, SubscriberData subscriberCallbackReference)
        {
            try
            {
                lock (_syncRoot)
                {
                    if (SubscribersList.ContainsKey(topicName))
                    {
                        if (SubscribersList[topicName].Contains(subscriberCallbackReference))
                        {
                            SubscribersList[topicName].Remove(subscriberCallbackReference);
                        }
                    }
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
    }
}

