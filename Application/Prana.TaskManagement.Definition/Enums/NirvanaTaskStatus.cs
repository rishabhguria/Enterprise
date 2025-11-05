using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.TaskManagement.Definition.Enums
{
    /// <summary>
    /// Task status of a task
    /// </summary>
    public enum NirvanaTaskStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Running = 2,

        /// <summary>
        /// There were some errors and task has been terminated
        /// </summary>
        Failure = 3,

        /// <summary>
        /// Task has been canceled by user and terminated
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// Task has been completed successfully
        /// </summary>
        Completed = 5,
        /// <summary>
        /// Batch is being imported into application
        /// </summary>
        Importing = 6
    }
}