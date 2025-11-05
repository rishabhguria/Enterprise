using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class BrowserLoadCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Url of browser
        /// </summary>
        public String URI { get; set; }

        /// <summary>
        /// is save successfull
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// true if rule save request recieved.
        /// </summary>
        public bool IsPostBack { get; set; }

        /// <summary>
        /// Stores any save info that may be required
        /// Incase of Custom rules stores the new value of constants
        /// not in use for user defined rules so far
        /// </summary>
        public Object Tag { get; set; }
    }
}
