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
    public class Subscription : ISubscription
    {
        static Subscription _subscription = null;
        static Subscription()
        {
            _subscription = new Subscription();
        }

        public static Subscription getInstance()
        {
            return _subscription;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        #region ISubscription Members
        public void Subscribe(string topicName, List<FilterData> FilterData)
        {
            try
            {
                SubscriberData subscriberdata = new SubscriberData();
                IPublishing subscriber = OperationContext.Current.GetCallbackChannel<IPublishing>();

                Logger.LoggerWrite("Requested to Subscribe for Topic " + topicName + " by " + subscriber.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Warning);

                OperationContext.Current.Channel.Closed += new EventHandler(Channel_Closed);
                OperationContext.Current.Channel.Faulted += new EventHandler(Channel_Faulted);
                subscriberdata.Subscriber = subscriber;
                if (FilterData != null)
                {
                    subscriberdata.Filters = FilterData;
                }
                SubscriberCollection.AddSubscriber(topicName, subscriberdata);
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

        private void Channel_Faulted(object sender, EventArgs e)
        {
            try
            {
                IPublishing subscriber = sender as IPublishing;
                SubscriberData data = new SubscriberData();
                data.Subscriber = subscriber;
                SubscriberCollection.RemoveSubscriber(data);
            }
            catch (Exception)
            {
            }
        }

        private void Channel_Closed(object sender, EventArgs e)
        {
            try
            {
                IPublishing subscriber = sender as IPublishing;
                SubscriberData data = new SubscriberData();
                data.Subscriber = subscriber;
                SubscriberCollection.RemoveSubscriber(data);
            }
            catch (Exception)
            {
            }
        }

        public void UnSubscribe(string topicName)
        {
            try
            {
                SubscriberData subscriberdata = new SubscriberData();
                IPublishing subscriber = OperationContext.Current.GetCallbackChannel<IPublishing>();
                subscriberdata.Subscriber = subscriber;
                SubscriberCollection.RemoveSubscriber(topicName, subscriberdata);
                Logger.LoggerWrite("Requested to UnSubscribe for Topic " + topicName + " by " + subscriber.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Warning);
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
