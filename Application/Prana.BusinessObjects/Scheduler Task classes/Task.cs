using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    public abstract class Task : EventArgs, ITask
    {
        private DateTime _datetime;

        public DateTime ScheduleTime
        {
            get { return _datetime; }
            set { _datetime = value; }
        }

        public abstract void AddTasksAtSameTime(ITask schedulerTask);
    }
}
