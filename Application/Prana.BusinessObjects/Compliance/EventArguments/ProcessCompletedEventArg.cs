using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class ProcessCompletedEventArg<T> : EventArgs
    {
        /// <summary>
        /// for event triggered when unApproved list gets Empty.
        /// ApprovedObjects object on which  operation is successfull
        /// failedObjects object on which  operation is unsuccessfull
        /// ProcessTag- OPeration
        /// Message- If there is any error message.
        /// </summary>
        public List<T> ApprovedObjects { get; set; }
        public List<T> FailedObjects { get; set; }
        public String ProcessTag { get; set; }
        public String Message { get; set; }
        // public String CompletedMessage { get; set; }
    }
}
