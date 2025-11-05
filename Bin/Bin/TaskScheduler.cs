using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;

namespace Nirvana.Global
{
    public static class TaskScheduler
    {
        private static TimerCallback timerCallBack = new TimerCallback(RaiseEvent);
        private static System.Threading.Timer schedulingTimer = new System.Threading.Timer(timerCallBack);

        static ArrayList dateTimeList = new System.Collections.ArrayList();
        public static void AddNewTask(DateTime dateTime)
        {

            dateTimeList.Add(dateTime);
            dateTimeList.Sort();
            


            SetTimer();

        }

        private static void SetTimer()
        {
            if (dateTimeList.Count > 0)
            {
                TimeSpan interval = (DateTime)dateTimeList[0] - DateTime.Now;
                schedulingTimer.Change(Convert.ToInt64(interval.TotalMilliseconds), Convert.ToInt64(interval.TotalMilliseconds));
            }
            else
            {
                schedulingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }


        public static EventHandler RunScheduleTask;
        static void RaiseEvent(object state)
        {
            //if (dateTimeList.Count > 0)
            //{
            if (RunScheduleTask != null)
            {
                RunScheduleTask(null, EventArgs.Empty);
            }
            dateTimeList.RemoveAt(0);
            SetTimer();
            //}
            //else
            //{
            //    schedulingTimer.Change(Timeout.Infinite, Timeout.Infinite);
            //}

        }




    }
}
