using System;

namespace Installer.Library
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        /// <remarks></remarks>
        public string Message { get; set; }
        public int StepValue { get; set; }
    }
}
