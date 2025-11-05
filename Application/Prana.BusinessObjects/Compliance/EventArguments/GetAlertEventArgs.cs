using Prana.BusinessObjects.Compliance.Enums;
using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class GetAlertEventArgs : EventArgs
    {
        /// <summary>
        /// Defines operation
        /// </summary>
        public AlertHistoryOperations Operation { get; set; }

        /// <summary>
        /// Defines start date for operation
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Defines end date for operation
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Defines pageNumber for ultragrid
        /// </summary>
        public int PageNo { get; set; }

        // <summary>
        /// Defines pageSize for ultragrid
        /// </summary>
        public int PageSize { get; set; }
    }
}
