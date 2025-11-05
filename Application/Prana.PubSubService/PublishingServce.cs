using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.PubSubService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class Publishing : IPublishing
    {
        static Publishing _publishing = null;
        static Publishing()
        {
            _publishing = new Publishing();
        }

        public static Publishing getInstance()
        {
            return _publishing;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        #region IPublishing Members
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                List<SubscriberData> subscribers = SubscriberCollection.GetSubscribers(topicName);
                if (subscribers == null) return;

                //Need to properly test cloning of subscribers, earlier it was throwing exception while stopping application and data publishing simultaneously
                subscribers = new List<SubscriberData>(subscribers);

                List<SubscriberData> faultedChannels = new List<SubscriberData>();
                foreach (SubscriberData subscriber in subscribers)
                {
                    try
                    {
                        List<IFilterable> allPassedData = new List<IFilterable>();
                        if (subscriber.Filters != null && subscriber.Filters.Count > 0)
                        {
                            System.Object[] datatoFilter = (System.Object[])e.EventData;
                            List<IFilterable> filteredData = null;
                            foreach (FilterData filter in subscriber.Filters)
                            {
                                filteredData = filter.Filterdata(ref datatoFilter, topicName, e.UserId);
                            }
                            allPassedData.AddRange(filteredData);
                            if (allPassedData.Count > 0)
                            {
                                //Updated topic name and user id before publish as MessageData.topic name is used at client
                                MessageData passedData = new MessageData();
                                passedData.TopicName = e.TopicName;
                                passedData.UserId = e.UserId;
                                passedData.EventData = allPassedData;
                                subscriber.Subscriber.Publish(passedData, topicName);
                            }
                        }
                        else
                        {
                            subscriber.Subscriber.Publish(e, topicName);
                        }
                    }
                    catch (Exception ex)
                    {
                        faultedChannels.Add(subscriber);
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                foreach (SubscriberData subscriber in faultedChannels)
                {
                    SubscriberCollection.RemoveSubscriber(topicName, subscriber);
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

        public string getReceiverUniqueName()
        {
            return "Publishing";
        }
        #endregion
    }
}

