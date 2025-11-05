using Prana.BusinessLogic;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;

namespace Prana.NotificationManager.BLL.Extractor
{
    internal static class NotificaitonExtractorExtension
    {

        static DateTime _lastValidatedTime = DateTime.Now.Date.AddDays(-1);
        static Object _timeLockerObject = new object();
        static bool _lastValidationResult = false;

        /// <summary>
        /// Checks alert for holidays and market hours.
        /// </summary>
        /// <param name="extractor"></param>
        /// <param name="notificationSetting"></param>
        /// <returns></returns>
        internal static bool GetIsValidAlert(this INotificationExtractor extractor, NotificationSetting notificationSetting, Alert alert)
        {
            try
            {
                //bool isValidSend = false;
                lock (_timeLockerObject)
                {
                    if (alert.ValidationTime.Date > _lastValidatedTime)
                    {
                        _lastValidatedTime = alert.ValidationTime.Date;
                        _lastValidationResult = BusinessDayCalculator.CheckForHoliday(_lastValidatedTime);
                    }
                }

                if (notificationSetting.StopAlertOnHolidays && _lastValidationResult)
                    return false;


                if (notificationSetting.AlertInTimeRange && notificationSetting.LimitFrequencyMinutes != 8)
                {
                    /// removing seconds so that alert is validated wrt minutes
                    DateTime startTime = notificationSetting.StartTime.AddSeconds(-notificationSetting.StartTime.Second);
                    DateTime endTime = notificationSetting.EndTime.AddSeconds(-notificationSetting.EndTime.Second);
                    DateTime validationTime = alert.ValidationTime.AddSeconds(-alert.ValidationTime.Second);

                    if (validationTime.TimeOfDay >= startTime.TimeOfDay && validationTime.TimeOfDay < endTime.TimeOfDay)
                        return true;
                    // Returning false in otherCase
                    else
                        return false;
                }
                else
                    return true;

                //return isValidSend;
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
                return false;
            }
        }
    }
}
