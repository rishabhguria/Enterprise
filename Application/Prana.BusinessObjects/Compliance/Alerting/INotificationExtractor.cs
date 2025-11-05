using Prana.BusinessObjects.Compliance.Definition;

namespace Prana.BusinessObjects.Compliance.Alerting
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationExtractor
    {
        /// <summary>
        /// Extracts notification settings.
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        NotificationSetting Extract(Alert alert, RuleBase rule);

        /// <summary>
        /// Initializes cache.
        /// </summary>
        void InitializeCache();

        /// <summary>
        /// Disposes  extractor.
        /// </summary>
        void CallDispose();
    }
}
