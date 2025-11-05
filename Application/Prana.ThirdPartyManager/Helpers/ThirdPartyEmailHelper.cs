using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.BusinessLogic;
using Prana.ThirdPartyManager.Helpers;
using Prana.Utilities.MiscUtilities;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.ThirdPartyManager.Helper
{
    public class ThirdPartyEmailHelper
    {
        #region Config AppSettings
        private static string _senderAddress = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderAddress");
        private static string _senderName = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderName");
        private static string _senderPassword = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailSenderPassword");
        private static string _hostName = ConfigurationHelper.Instance.GetAppSettingValueByKey("MailHostName");
        private static int _port = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MailPort"));
        private static bool _enableSSL = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableSSL"));
        private static string[] _recipients = ConfigurationHelper.Instance.GetAppSettingValueByKey("ThirdPartyMailReceiverAddress").Split(',');
        private static string[] _ccRecipients = ConfigurationHelper.Instance.GetAppSettingValueByKey("ThirdPartyMailCCAddress").Split(',');
        private static string[] _bccRecipients = ConfigurationHelper.Instance.GetAppSettingValueByKey("ThirdPartyMailBCCAddress").Split(',');
        #endregion

        #region Constants
        public const string CONST_JOB_NAME = "ThirdPartyEmailJob";
        private const string CONST_JOB_GROUP = "ThirdPartyEmailGroup";
        private const string CONST_EMAIL_TRIGGER_NAME = "ThirdPartyEmailTrigger";
        private const string CONST_EMAIL_TRIGGER_GROUP = "ThirdPartyEmailTriggerGroup";
        #endregion

        /// <summary>
        /// The sched
        /// </summary>
        private static IScheduler _scheduler;
        /// <summary>
        /// The sched fact
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
        /// Send mail for allocation match status for complete match, complete mismatch and partial mismatch
        /// </summary>
        /// <param name="thirdPartyJobName"></param>
        /// <param name="allocationMatchStatus"></param>
        public static void SendAllocationStatusChangeMail(string thirdPartyJobName, ApplicationConstants.AllocationMatchStatus allocationMatchStatus)
        {
            try
            {
                string subject = string.Empty;
                switch (allocationMatchStatus)
                {
                    case ApplicationConstants.AllocationMatchStatus.CompleteMatch:
                        subject = string.Format("All allocation Instructions are accepted by the {0} for {1}", thirdPartyJobName, CachedDataManager.GetInstance.GetCompanyName());
                        break;
                    case ApplicationConstants.AllocationMatchStatus.CompleteMismatch:
                        subject = string.Format("All allocation Instructions are rejected by the {0} for {1}", thirdPartyJobName, CachedDataManager.GetInstance.GetCompanyName());
                        break;
                    case ApplicationConstants.AllocationMatchStatus.PartialMismatch:
                        subject = string.Format("Some allocation Instructions are accepted by the {0} for {1}", thirdPartyJobName, CachedDataManager.GetInstance.GetCompanyName());
                        break;
                }
                string body = string.Empty;
                if (!string.IsNullOrEmpty(subject))
                {
                    if (_ccRecipients != null && string.IsNullOrEmpty(_ccRecipients[0]))
                        _ccRecipients = null;
                    if (_bccRecipients != null && string.IsNullOrEmpty(_bccRecipients[0]))
                        _bccRecipients = null;
                    EmailsHelper.MailSend(subject, body, _senderAddress, _senderName, _senderPassword, _recipients, _port, _hostName, _enableSSL, true, false, _ccRecipients, _bccRecipients);
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

        /// <summary>
        /// This method is to send scheduled job status email
        /// </summary>
        public static void SendScheduledJobStatusEmail()
        {
            try
            {
                var jobStatuses = GetDailyJobStatus();
                if (jobStatuses != null && jobStatuses.Count > 0)
                {
                    var subject = "Job Status"; //To be changed as per product's decision
                    var body = CreateBodyForJobStatusEmail(jobStatuses);
                    if (!string.IsNullOrEmpty(subject))
                    {
                        if (_ccRecipients != null && string.IsNullOrEmpty(_ccRecipients[0]))
                            _ccRecipients = null;
                        if (_bccRecipients != null && string.IsNullOrEmpty(_bccRecipients[0]))
                            _bccRecipients = null;
                        EmailsHelper.MailSend(subject, body, _senderAddress, _senderName, _senderPassword, _recipients, _port, _hostName, _enableSSL, true, true, _ccRecipients, _bccRecipients);
                    }
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
        /// This method is to create body for job status email
        /// </summary>
        /// <param name="jobStatuses"></param>
        /// <returns>Email body</returns>
        public static string CreateBodyForJobStatusEmail(Dictionary<string, string> jobStatuses)
        {
            StringBuilder body = new StringBuilder();
            try
            {
                body.Append("<table border='1'>");
                body.Append("<tr><th>Job Name</th><th>Allocation Status</th></tr>");
                foreach (var jobStatus in jobStatuses)
                {
                    body.Append(string.Format("<tr><th>{0}</th><th>{1}</th></tr>", jobStatus.Key, jobStatus.Value));
                }
                body.Append("</table>");
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
            return body.ToString();
        }

        /// <summary>
        /// This method is to send mail at scheduled time.
        /// </summary>
        public static void SendEmailOnScheduledTime()
        {
            try
            {
                string scheduledTimeString = ConfigurationHelper.Instance.GetAppSettingValueByKey("ScheduledEmailSendingTime");
                if (!scheduledTimeString.Equals(DateTime.MinValue))
                {
                    ShutdownScheduler();
                    StartScheduler();

                    JobDetail jobdetail = new JobDetail(CONST_JOB_NAME, CONST_JOB_GROUP, typeof(ThirdPartyEmailNotifcationJob));
                    TimeSpan timespan = new TimeSpan(864000000000); // for whole 24day set it to 864000000000
                    SimpleTrigger simpleTrigger = new SimpleTrigger();
                    simpleTrigger.RepeatCount = SimpleTrigger.RepeatIndefinitely;
                    simpleTrigger.RepeatInterval = timespan;
                    DateTime scheduledTime = DateTime.Parse(scheduledTimeString);
                    simpleTrigger.StartTimeUtc = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(scheduledTime, Prana.BusinessObjects.TimeZoneInfo.EasternTimeZone);
                    simpleTrigger.Name = CONST_EMAIL_TRIGGER_NAME;
                    simpleTrigger.Group = CONST_EMAIL_TRIGGER_GROUP;
                    _scheduler.DeleteJob(CONST_JOB_NAME, CONST_JOB_GROUP);
                    _scheduler.ScheduleJob(jobdetail, simpleTrigger);
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

        /// <summary>
        /// This method is to get status for all the jobs for today
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetDailyJobStatus()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (ThirdPartyCache.DateWiseAllocationBlockDetails.Count == 0 
                    || !ThirdPartyCache.DateWiseAllocationBlockDetails.ContainsKey(DateTime.Today.ToShortDateString()))
                {
                    ThirdPartyLogic.InitiateAllocationBlockCache(DateTime.Today);
                }
                foreach (var job in ThirdPartyCache.DateWiseAllocationBlockDetails[DateTime.Today.ToShortDateString()].Values)
                {
                    if (!result.ContainsKey(job.ThirdPartyJobName))
                    {
                        result.Add(job.ThirdPartyJobName, EnumHelper.GetDescription(job.AllocationMatchStatus));
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
            return result;
        }

        /// <summary>
        /// Sends an email notification about the status of a scheduled allocation batch.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the batch was successful.</param>
        /// <param name="jobName">The name of the job associated with the batch.</param>
        public static void SendScheduledTimeBatchStatusMail(bool isSuccess, string jobName)
        {
            try
            {

                var subject = isSuccess ? "Scheduled Allocation Batch successful for " + jobName : "Scheduled Allocation Batch failed for " + jobName;
                if (!string.IsNullOrEmpty(subject))
                {
                    if (_ccRecipients != null && string.IsNullOrEmpty(_ccRecipients[0]))
                        _ccRecipients = null;
                    if (_bccRecipients != null && string.IsNullOrEmpty(_bccRecipients[0]))
                        _bccRecipients = null;
                    EmailsHelper.MailSend(subject, string.Empty, _senderAddress, _senderName, _senderPassword, _recipients, _port, _hostName, _enableSSL, true, true, _ccRecipients, _bccRecipients);
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
