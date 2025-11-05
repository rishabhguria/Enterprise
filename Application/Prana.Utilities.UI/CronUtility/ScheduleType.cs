namespace Prana.Utilities.UI.CronUtility
{
    /// <summary>
    /// Describes schedule type in task scheduler
    /// </summary>
    public enum ScheduleType
    {
        /// <summary>
        /// task gets executed only one time 
        /// </summary>
        OneTime,

        /// <summary>
        /// task gets executed on daily basis 
        /// </summary>
        Daily,

        /// <summary>
        /// task gets executed on weekly basis 
        /// </summary>
        Weekly,

        /// <summary>
        /// task gets executed on monthly basis 
        /// </summary>
        Monthly
    }
}
