using Prana.BusinessObjects.Compliance.Enums;
using System;
using System.Data;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class UpdateAlertGridEventArgs : EventArgs
    {
        /// <summary>
        /// Updates ALert grid.
        /// </summary>
        public DataSet DsRecieved { get; set; }
        public AlertHistoryOperations Operation { get; set; }
    }
}
