using System;

namespace Prana.BusinessObjects
{
    public interface ITask
    {
        DateTime ScheduleTime
        {
            get;
            set;
        }
    }
}
