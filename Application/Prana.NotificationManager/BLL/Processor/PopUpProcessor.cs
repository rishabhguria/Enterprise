using Prana.AmqpAdapter.Amqp;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using System;

namespace Prana.NotificationManager.BLL.Processor
{
    internal class PopUpProcessor : INotificationProcessor
    {


        #region INotificationProcessor Members

        /// <summary>
        /// Process pop up on client.
        /// </summary>
        /// <param name="alert"></param>
        public void Process(Alert alert, NotificationSetting notification, NotificationStrategy NotificationStrategy)
        {
            try
            {
                notification.PopUpEnabledUsers.ForEach(userId =>
                    {
                        if (alert.PackageName == RulePackage.PreTrade)
                            AmqpHelper.SendObject(Alert.GetAlertDataSet(alert), "NotificationSender", "PreAlertFromNotificationManager_" + userId);
                        else
                            AmqpHelper.SendObject(Alert.GetAlertDataSet(alert), "NotificationSender", "PostAlertFromNotificationManager_" + userId);
                    });
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
