using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;

namespace Prana.ThirdPartyManager.Helpers
{
    public class ThirdPartyTimedBatchHelper
    {
        /// <summary>
        /// The scheduler
        /// </summary>
        private static IScheduler _scheduler;
        /// <summary>
        /// The scheduler factory
        /// </summary>
        private static ISchedulerFactory _schedulerFactory = new StdSchedulerFactory();

        /// <summary>
        /// Shutdowns the scheduler.
        /// </summary>
        private static void ShutdownScheduler()
        {
            try
            {
                if (_scheduler != null)
                {
                    _scheduler.Standby();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Starts the scheduler.
        /// </summary>
        private static void StartScheduler()
        {
            try
            {
                _scheduler = _schedulerFactory.GetScheduler();
                _scheduler.Start();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets up the scheduler for timed batch jobs.
        /// </summary>
        public static void SetTimedBatchesScheduler()
        {
            try
            {
                Dictionary<int, List<ThirdPartyTimeBatch>> thirdPartyTimeBatches = ThirdPartyDataManager.GetAllThirdPartyTimeBatches();

                ShutdownScheduler();
                StartScheduler();

                // Retrieve job names associated with the "ThirdPartyTimeBatchGroup"
                string[] jobNames = _scheduler.GetJobNames(ThirdPartyConstants.THIRD_PARTY_TIME_BATCH_GROUP);

                // Iterate through each job name and delete the corresponding job
                foreach (string jobName in jobNames)
                {
                    _scheduler.DeleteJob(jobName, ThirdPartyConstants.THIRD_PARTY_TIME_BATCH_GROUP);                   
                }

                // Retrieve job names associated with the "ThirdPartyJobExecutionNotificationGroup"
                jobNames = _scheduler.GetJobNames(ThirdPartyConstants.THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_GROUP);

                // Iterate through each job name and delete the corresponding job
                foreach (string jobName in jobNames)
                {
                    _scheduler.DeleteJob(jobName, ThirdPartyConstants.THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_GROUP);
                }

                foreach (var timeBatches in thirdPartyTimeBatches)
                {
                    foreach(ThirdPartyTimeBatch timeBatch in timeBatches.Value)
                    {
                        // Calculate scheduled time
                        DateTime currentESTTime = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, BusinessObjects.TimeZoneInfo.EasternTimeZone);
                        DateTime scheduledTime = currentESTTime.Date + timeBatch.BatchRunTime.TimeOfDay;

                        #region Job Scheduler that send batch at scheduled time
                        JobDetail jobdetail = new JobDetail(ThirdPartyConstants.JOB_SEND_THIRD_PARTY_TIME_BATCHES + timeBatch.ID, ThirdPartyConstants.THIRD_PARTY_TIME_BATCH_GROUP, typeof(ThirdPartySendTimeBatchesJob));
                        jobdetail.JobDataMap["BatchId"] = timeBatches.Key;
                        jobdetail.JobDataMap["ScheduledTime"] = scheduledTime;
                        jobdetail.JobDataMap["TimeBatchId"] = timeBatch.ID;

                        // Create a simple trigger 
                        SimpleTrigger simpleTrigger = new SimpleTrigger();
                        simpleTrigger.StartTimeUtc = BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(scheduledTime, BusinessObjects.TimeZoneInfo.EasternTimeZone);
                        simpleTrigger.Name = ThirdPartyConstants.THIRD_PARTY_TIME_BATCH_TRIGGER_GROUP + timeBatch.ID;
                        simpleTrigger.Group = ThirdPartyConstants.THIRD_PARTY_TIME_BATCH_GROUP;
                        simpleTrigger.RepeatInterval = TimeSpan.FromDays(1);
                        simpleTrigger.RepeatCount = SimpleTrigger.RepeatIndefinitely;
                      
                        // Schedule the job
                        _scheduler.ScheduleJob(jobdetail, simpleTrigger);
                        #endregion

                        #region Job Scheduler that send notification before the Job execution

                        JobDetail notificationJobdetail = new JobDetail(ThirdPartyConstants.JOB_SEND_THIRD_PARTY_JOB_EXECUTION_NOTIFICATION + timeBatch.ID, ThirdPartyConstants.THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_GROUP, typeof(ThirdPartySendTimeBatchesJob));
                        notificationJobdetail.JobDataMap["BatchId"] = timeBatches.Key;
                        notificationJobdetail.JobDataMap["ScheduledTime"] = scheduledTime;

                        TimeSpan notificationTimeSpan = TimeSpan.FromMinutes(ThirdPartyConstants.CONST_TIME_BEFORE_JOB_EXECUTION_FOR_NOTIFICATION);                       

                        // Create a simple trigger for job execution notification
                        SimpleTrigger notificationTrigger = new SimpleTrigger();
                        notificationTrigger.StartTimeUtc = BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(scheduledTime - notificationTimeSpan, BusinessObjects.TimeZoneInfo.EasternTimeZone);
                        notificationTrigger.Name = ThirdPartyConstants.THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_SCHEDULER + timeBatch.ID;
                        notificationTrigger.Group = ThirdPartyConstants.THIRD_PARTY_JOB_EXECUTION_NOTIFICATION_TRIGGER_GROUP;
                        notificationTrigger.RepeatInterval = TimeSpan.FromDays(1);
                        notificationTrigger.RepeatCount = SimpleTrigger.RepeatIndefinitely;

                        // Schedule the job
                        _scheduler.ScheduleJob(notificationJobdetail, notificationTrigger); 
                        #endregion
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
    }
}
