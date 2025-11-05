using Bloomberglp.Blpapi;
using System.Collections.Generic;

namespace Bloomberg.Library.Requests
{
    public class MarketDataRequest : List<UserSubscription>
    {

        /// <summary>
        /// Sets a defined period in seconds for which updates will be received for the subscription.
        /// The range for this argument is 0.10 to 86400.00, which is equal to 100ms to 24hours.
        /// For example setting this argument to 30 will result in the requesting application to receive updates every 30 seconds for the requested securities.
        /// </summary>
        /// <value>The interval.</value>
        public float Interval;

        /// <summary>
        /// Forces the subscription to be delayed even if the requestor has realtime exchange entitlements.
        /// </summary>
        /// <value>The delayed.</value> 
        public bool Delayed;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <returns>List{System.String}.</returns>
        private List<string> Options
        {
            get
            {
                List<string> options = new List<string>();
                if (Interval > 0.0F)
                {
                    options.Add(string.Format("interval={0}", Interval));
                }

                if (Delayed)
                {
                    options.Add("delayed");
                }
                return options;
            }
            set {; }

        }

        public UserSubscription GetSubscription(string security)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].SecurityInfo.RequestId == security)
                {
                    return this[i];
                }
                if (this[i].SecurityInfo.Security == security)
                    System.Diagnostics.Debug.Print("Found problem");
            }

            return null;
        }
        public UserSubscription Create(string security)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].SecurityInfo.RequestId == security)
                {
                    return this[i];
                }
            }
            return null;
        }
        public bool Contains(SecurityInfo info)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].SecurityInfo.RequestId == info.RequestId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteSecurity(string security)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].SecurityInfo.RequestId == security)
                {
                    this.Remove(this[i]);
                    return true;
                }
            }
            return false;
        }

        public IList<Subscription> Subscriptions
        {
            get
            {
                List<Subscription> items = new List<Subscription>();
                foreach (UserSubscription item in this)
                {
                    if (item.SubscriptionStatus == Session.SubscriptionStatus.UNSUBSCRIBED)
                        items.Add(item as Subscription);
                }
                return items;
            }
        }


    }
}
