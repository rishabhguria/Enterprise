using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.LogManager;
using Prana.NotificationManager.DAL;
using System;

namespace Prana.NotificationManager.BLL.Processor
{
    internal class DBUpdator : INotificationProcessor
    {



        #region INotificationProcessor Members

        /// <summary>
        /// Saves alert in alert history table.
        /// </summary>
        /// <param name="alert"></param>
        public void Process(Alert alert, NotificationSetting notification, NotificationStrategy NotificationStrategy)
        {
            try
            {
                AlertsDataManager.SaveAlertsInDB(alert);
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
