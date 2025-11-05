using System;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Message Info Event Args
    /// </summary>
    /// <remarks></remarks>
    public class MessageInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        /// <remarks></remarks>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        /// <remarks></remarks>
        public string Source { get; set; }
        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>The details.</value>
        /// <remarks></remarks>
        public string Details { get; set; }
    }

    /// <summary>
    /// Exception Event Args
    /// </summary>
    /// <remarks></remarks>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the inner message.
        /// </summary>
        /// <value>The inner message.</value>
        /// <remarks></remarks>
        public object InnerMessage {get;set;}
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        /// <remarks></remarks>
        public Exception Exception { get; set; }
    }
}
