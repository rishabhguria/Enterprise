using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class StatusBarEventArgs : EventArgs
    {
        /// <summary>
        /// Status to be shown on status bar
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        ///True if Operation Completed.
        /// </summary>
        public bool IsCompleteState { get; set; }
    }
}
