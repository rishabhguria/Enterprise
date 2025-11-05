using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    public class EPNLSchedulerTask : Task
    {
         //this was not made a struct as modifying struct while it belongs to a list throws exception
        public EPNLSchedulerTask(DateTime dateTime)
        {
            ScheduleTime = dateTime;
        }
     
        public override void AddTasksAtSameTime(ITask scheduledtasksAtSameTime)
        {
            
        } 
    }
}
