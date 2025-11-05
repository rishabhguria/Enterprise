using Prana.TaskManagement.Definition.Definition;

namespace Prana.TaskManagement.Definition.Interfaces
{
    public interface ITaskCallback
    {
        void TaskExecutionComplete(TaskResult result);

    }
}
