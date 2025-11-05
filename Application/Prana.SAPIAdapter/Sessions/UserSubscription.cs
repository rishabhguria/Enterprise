// ***********************************************************************
// Assembly         : Bloomberg Library
// Author           : MJCarlucci
// Created          : 04-29-2013
//
// Last Modified By : MJCarlucci
// Last Modified On : 04-29-2013
// ***********************************************************************
// <copyright file="UserSubscription.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Bloomberglp.Blpapi;


namespace Bloomberg.Library
{
    public enum SubscriptionState
    {
        None,
        Subscribing,
        Subscribed
    }
    /// <summary>
    /// Class UserSubscription
    /// </summary>
    public class UserSubscription : Subscription
    {

        public SubscriptionState SubscriptionState = SubscriptionState.None;
        /// <summary>
        /// The security info
        /// </summary>
        public SecurityInfo SecurityInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="securityInfo">The security info.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="delayed">if set to <c>true</c> [delayed].</param>
        /// <param name="id">The id.</param>
        public UserSubscription(SecurityInfo securityInfo, string fields, float interval, bool delayed, CorrelationID id)
            : base(securityInfo.RequestId, fields, string.Format("Interval={0},delayed={1}", interval, delayed), id)
        {
            this.SecurityInfo = securityInfo;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="subscriptionString">The subscription string.</param>
        public UserSubscription(string subscriptionString)
            : base(subscriptionString)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="subscriptionString">The subscription string.</param>
        /// <param name="correlationID">The correlation ID.</param>
        public UserSubscription(string subscriptionString, CorrelationID correlationID)
            : base(subscriptionString, correlationID)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        public UserSubscription(string security, string fields)
            : base(security, fields)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="correlationId">The correlation id.</param>
        public UserSubscription(string security, string fields, CorrelationID correlationId)
            : base(security, fields, correlationId)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="options">The options.</param>
        public UserSubscription(string security, string fields, string options)
            : base(security, fields, options)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="options">The options.</param>
        /// <param name="correlationID">The correlation ID.</param>
        public UserSubscription(string security, string fields, string options, CorrelationID correlationID)
            : base(security, fields, options, correlationID)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        public UserSubscription(string security, System.Collections.Generic.IList<System.String> fields)
            : base(security, fields)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="correlationID">The correlation ID.</param>
        public UserSubscription(string security, System.Collections.Generic.IList<System.String> fields, CorrelationID correlationID)
            : base(security, fields, correlationID)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="options">The options.</param>
        public UserSubscription(string security, System.Collections.Generic.IList<System.String> fields, System.Collections.Generic.IList<System.String> options)
            : base(security, fields, options)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSubscription"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="options">The options.</param>
        /// <param name="correlationID">The correlation ID.</param>
        public UserSubscription(string security, System.Collections.Generic.IList<System.String> fields, System.Collections.Generic.IList<System.String> options, CorrelationID correlationID)
            : base(security, fields, options, correlationID)
        {

        }

    }
}
