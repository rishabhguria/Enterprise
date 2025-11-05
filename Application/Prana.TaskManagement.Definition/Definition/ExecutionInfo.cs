using System;
using System.Collections.Generic;

namespace Prana.TaskManagement.Definition.Definition
{
    /// <summary>
    /// Contains data for a task to get executed
    /// </summary>
    [Serializable]
    public class ExecutionInfo
    {
        public String ExecutionId { get; set; }

        public String ExecutionName { get; set; }

        public TaskInfo TaskInfo { get; set; }

        public TaskInfo SuccessTaskInfo { get; set; }

        public TaskInfo FailureTaskInfo { get; set; }

        public bool DoRetry { get; set; }

        public int RetryCount { get; set; }

        public int RetryInterval { get; set; }

        public String InputData { get; set; }

        public List<object> InputObjects { get; set; }

        public String CronExpression { get; set; }


        public bool IsAutoImport { get; set; }
    }
}
