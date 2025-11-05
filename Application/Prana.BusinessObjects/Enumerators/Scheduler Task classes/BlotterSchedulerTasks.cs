using System;
using System.Collections.Generic;


namespace Prana.BusinessObjects
{
    public class BlotterSchedulerTask : Task
    {
        //this was not made a struct as modifying struct while it belongs to a list throws exception
        public BlotterSchedulerTask(DateTime dateTime, ClearanceData clearanceData)
        {
            if (_clearanceData == null)
            {
                _clearanceData = new List<ClearanceData>();
            }
            ScheduleTime = dateTime;
            _clearanceData.Add(clearanceData);
        }

        public void AddTaskAtSameTime(ClearanceData taskAtSameTime)
        {
            if (_clearanceData == null)
            {
                _clearanceData = new List<ClearanceData>();
            }
            _clearanceData.Add(taskAtSameTime);
        }

        public override void AddTasksAtSameTime(ITask scheduledtasksAtSameTime)
        {
            BlotterSchedulerTask taskatsameTime = scheduledtasksAtSameTime as BlotterSchedulerTask;
            if (taskatsameTime != null)
            {
                if (_clearanceData == null)
                {
                    _clearanceData = new List<ClearanceData>();
                }
                foreach (ClearanceData taskAtSameTime in (taskatsameTime.ClearanceData))
                {
                    _clearanceData.Add(taskAtSameTime);
                }
            }
        }



        private List<ClearanceData> _clearanceData;

        public List<ClearanceData> ClearanceData
        {
            get { return _clearanceData; }
            set { _clearanceData = value; }
        }

    }
}
