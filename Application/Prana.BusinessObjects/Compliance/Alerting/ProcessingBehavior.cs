namespace Prana.BusinessObjects.Compliance.Alerting
{
    /// <summary>
    /// Chennels for sending alert.
    /// </summary>
    public enum ProcessingBehavior
    {
        Email,
        PopUp,
        AlertHistoryPublisher,
        DBUpdater
    }

}
