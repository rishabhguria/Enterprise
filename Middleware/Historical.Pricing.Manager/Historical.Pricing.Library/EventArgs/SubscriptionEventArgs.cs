using System;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Subscription Event Args
    /// </summary>
    /// <remarks></remarks>
    public class SubscriptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        /// <value>The subscription.</value>
        /// <remarks></remarks>
        public Subscription Subscription { get; set; }
    }
}
