using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Utilities.DateTimeUtilities
{
    public class Scheduler : IDisposable
    {
        readonly object _locker = new object();
        readonly object _lockerForList = new object();
        //TODO: check this if can be put on threading.timer.
        // lot of UI timers hanging around
        private System.Windows.Forms.Timer _schedulingTimer;
        private List<Task> _clearanceDateTimeList = new List<Task>();
        Comparison<Task> _timeComparisonHandler;


        private Scheduler _blotterClearanceScheduler = null;
        private TimeSpan _zeroSpan = new TimeSpan(0, 0, 0);
        private TimeSpan _maxSpan = new TimeSpan(24, 0, 0);
        #region Singleton Implementation

        public List<Task> ClearanceDateTimeList
        {
            get { return _clearanceDateTimeList; }
        }

        public Scheduler()
        {
            this.Initialize();
        }

        /// <summary>
        /// Get Single instance 
        /// </summary>
        /// <returns></returns>
        public Scheduler GetInstance()
        {
            if (_blotterClearanceScheduler == null)
            {
                lock (_locker)
                {
                    if (_blotterClearanceScheduler == null)
                    {
                        _blotterClearanceScheduler = new Scheduler();
                    }
                }
            }
            return _blotterClearanceScheduler;
        }

        #endregion

        public void Initialize()
        {
            _schedulingTimer = new System.Windows.Forms.Timer();
            _schedulingTimer.Tick += new EventHandler(_schedulingTimer_Tick);
            _timeComparisonHandler = new Comparison<Task>(CompareTimes);
        }

        void _schedulingTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Task taskJustExpired = null;
                lock (_lockerForList)
                {
                    if (_clearanceDateTimeList.Count > 0)
                    {
                        taskJustExpired = _clearanceDateTimeList[0];
                        _clearanceDateTimeList.RemoveAt(0);
                        // Reschedule task to a time after 24 hours
                        taskJustExpired.ScheduleTime = taskJustExpired.ScheduleTime.AddHours(24);
                        _clearanceDateTimeList.Add(taskJustExpired);
                    }
                }

                if (RunScheduleTask != null && taskJustExpired != null)
                {
                    RunScheduleTask(null, taskJustExpired);
                }

                SetTimer();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddNewTask(Task task)
        {
            try
            {
                lock (_lockerForList)
                {
                    _clearanceDateTimeList.Add(task);
                    _clearanceDateTimeList.Sort(_timeComparisonHandler);
                }
                SetTimer();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void CollapseEventsAtSameTime()
        {
            try
            {
                lock (_lockerForList)
                {
                    List<Task> _consolidatedclearanceDateTimeList = new List<Task>(_clearanceDateTimeList.Count);
                    _consolidatedclearanceDateTimeList.Add(_clearanceDateTimeList[0]);
                    for (int i = 0, j = 0; i < _clearanceDateTimeList.Count - 1; i++)
                    {
                        if (_clearanceDateTimeList[i].ScheduleTime == _clearanceDateTimeList[i + 1].ScheduleTime)
                        {

                            _consolidatedclearanceDateTimeList[j].AddTasksAtSameTime(_clearanceDateTimeList[i + 1]);
                        }
                        else
                        {
                            _consolidatedclearanceDateTimeList.Add(_clearanceDateTimeList[i + 1]);
                            j++;
                        }
                    }
                    _clearanceDateTimeList = _consolidatedclearanceDateTimeList;
                }
                SetTimer();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private int CompareTimes(Task first, Task second)
        {
            try
            {
                if (first != null && second != null)
                {
                    return first.ScheduleTime.CompareTo(second.ScheduleTime);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        private void SetTimer()
        {
            try
            {
                TimeSpan interval = TimeSpan.MinValue;
                lock (_lockerForList)
                {
                    if (_clearanceDateTimeList.Count > 0)
                    {
                        if (_clearanceDateTimeList[0].ScheduleTime != null)
                        {
                            interval = _clearanceDateTimeList[0].ScheduleTime.TimeOfDay - DateTime.Now.TimeOfDay;
                        }
                    }
                }

                if (interval.CompareTo(_zeroSpan) <= 0)
                {
                    interval = interval.Add(_maxSpan);
                }
                _schedulingTimer.Stop();
                if (interval != TimeSpan.MinValue)
                {
                    _schedulingTimer.Interval = (int)interval.TotalMilliseconds;
                    _schedulingTimer.Start();
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public event EventHandler<Task> RunScheduleTask;

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _schedulingTimer.Dispose();
                _blotterClearanceScheduler.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
