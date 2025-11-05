using Prana.BusinessObjects.Compliance.Definition;

namespace Prana.BusinessObjects.Compliance.Alerting
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationProcessor
    {
        /// <summary>
        /// method in processors. used when alert can be send without notification settings
        /// Pop up Db save
        /// </summary>
        /// <param name="alert"></param>
        //void Process(Alert alert);

        /// <summary>
        /// method in processors. used when alert can not be send without notification settings
        /// Email
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="notification"></param>
        void Process(Alert alert, NotificationSetting notification, NotificationStrategy NotificationStrategy);
    }
}
