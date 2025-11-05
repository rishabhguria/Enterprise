using System;
using System.Collections.Generic;
using System.Text;

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
