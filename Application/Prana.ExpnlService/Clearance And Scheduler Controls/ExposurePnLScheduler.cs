using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public class ExposurePnLScheduler : IDisposable
    {
        private static ExposurePnLScheduler _exposurePnLScheduler = null;
        private Scheduler _clearanceScheduler;

        private ExposurePnLScheduler()
        {
            try
            {
                _clearanceScheduler = new Scheduler();
                _clearanceScheduler.RunScheduleTask += new EventHandler<Task>(_clearanceScheduler_RunScheduleTask);
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

        public event EventHandler ScheduleElapsed;

        public static ExposurePnLScheduler GetInstance()
        {
            if (_exposurePnLScheduler == null)
            {
                _exposurePnLScheduler = new ExposurePnLScheduler();
            }
            return _exposurePnLScheduler;
        }

        public void ScheduleTasks()
        {
            try
            {
                QuartzScheduler _quartzScheduler = QuartzScheduler.GetInstance();
                Dictionary<int, DateTime> quartzSchAuecClearanceTimeDict = new Dictionary<int, DateTime>();
                Dictionary<int, DateTime> currentAUECDateTimes = new Dictionary<int, DateTime>(CachedDataManager.GetInstance.GetAUECCount() + 1);
                TimeZoneHelper.GetInstance().CurrentAUECDateTimes(currentAUECDateTimes);

                foreach (KeyValuePair<int, DateTime> auecTimes in currentAUECDateTimes)
                {
                    // now schedule one entry in scheduler for this timeremaining to clearance
                    DateTime utcTimeToRaiseEvent;
                    if (TimeZoneHelper.GetInstance().ClearanceTime.ContainsKey(auecTimes.Key))
                    {
                        utcTimeToRaiseEvent = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(TimeZoneHelper.GetInstance().ClearanceTime[auecTimes.Key], CachedDataManager.GetInstance.GetAUECTimeZone(auecTimes.Key));
                    }
                    else
                    {
                        //add midnight refresh
                        DateTime midNightTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        utcTimeToRaiseEvent = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(TimeZoneHelper.GetInstance().MostLeadingAUECDateTime(true).Add(midNightTime.TimeOfDay), CachedDataManager.GetInstance.GetAUECTimeZone(auecTimes.Key));
                    }

                    //Quartz: adding the aueckey & time in the list that is to be passed to QuartzScheduler.
                    quartzSchAuecClearanceTimeDict.Add(auecTimes.Key, utcTimeToRaiseEvent);
                }

                _quartzScheduler.CreateTriggerForAuecsClearance(quartzSchAuecClearanceTimeDict, TimeZoneHelper.GetInstance().InUseAUECIDs);
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

        private void _clearanceScheduler_RunScheduleTask(object sender, Task e)
        {
            try
            {
                //if (e is EPNLSchedulerTask)
                //using 'as' in place of 'is' would avoid double verification
                //TODO: Check for all such cases where incoming eventargs is passed as eventArgs to further events. Make sure to take such
                //arguements in local variables before futher processing, this would ensure no other thread alters the args before processing
                EPNLSchedulerTask currentTask = e as EPNLSchedulerTask;
                if (currentTask != null)
                {
                    if (ScheduleElapsed != null)
                    {
                        ScheduleElapsed(this, currentTask);
                    }
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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_clearanceScheduler != null)
                    _clearanceScheduler.Dispose();
            }
        }

        #endregion
    }
}