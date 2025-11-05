using Bloomberglp.Blpapi;
using System;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class SubscriptionFailure
    /// </summary>
    [Serializable]
    public class SubscriptionFailure : EventArgs
    {
        string Text;
        //CHMW-2571	Apply Microsoft Managed Recommended Rules in Prana.Prana.SAPIAdapter project
        //SubscriptionFailureDetails Details;
        public CorrelationID CorrelationId;

        public SubscriptionError Error;
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionFailure"/> class.
        /// </summary>
        public SubscriptionFailure() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionFailure"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SubscriptionFailure(object message)

        {
            CorrelationId = ((Message)message).CorrelationID;

            if (((Message)message).HasElement(new Name("reason")))
            {
                Element element2 = ((Message)message).GetElement(new Name("reason"));
                Error = new SubscriptionError(((Message)message).GetElement(new Name("reason")));

            }
            this.Text = message.ToString();
        }

    }
}
