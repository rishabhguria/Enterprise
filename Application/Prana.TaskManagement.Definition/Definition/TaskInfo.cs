using System;

namespace Prana.TaskManagement.Definition.Definition
{
    [Serializable]
    public class TaskInfo
    {
        public int TaskId { get; set; }

        public String TaskName { get; set; }

        public String AssemblyName { get; set; }

        public String QClassName { get; set; }

    }
}
