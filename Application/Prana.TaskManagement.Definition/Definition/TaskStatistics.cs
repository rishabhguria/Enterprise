using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.TaskManagement.Definition.Definition
{
    public class TaskStatistics
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int ExecutedBy { get; set; }

        public NirvanaTaskStatus Status { get; set; }


        private TaskSpecificDataPoints _taskSpecificData = new TaskSpecificDataPoints();
        public TaskSpecificDataPoints TaskSpecificData { get { return _taskSpecificData; } set { _taskSpecificData = value; } }

    }
}
