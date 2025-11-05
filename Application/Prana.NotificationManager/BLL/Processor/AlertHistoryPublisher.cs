using Prana.AmqpAdapter.Amqp;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.LogManager;
using System;

namespace Prana.NotificationManager.BLL.Processor
{
    internal class AlertHistoryPublisher : INotificationProcessor
    {


        #region INotificationProcessor Members

        /// <summary>
        /// publises alert on alert hoistory.
        /// </summary>
        /// <param name="alert"></param>     
        public void Process(Alert alert, NotificationSetting notification, NotificationStrategy NotificationStrategy)
        {
            try
            {
                AmqpHelper.SendObject(Alert.GetAlertDataSet(alert), "NotificationSender", "AlertFromNotificationManager");
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


        #endregion
    }
}
