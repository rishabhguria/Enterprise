using Prana.LogManager;
using Quartz;
using System;

namespace Prana.PostTradeServices
{
    public class GtcGtdEmailNotifcationJob : IJob
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(JobExecutionContext context)
        {
            try
            {
                if (context.JobDetail.Name.Equals(GtcGtdEmailNotificationManager.CONST_JOB_NAME))
                {
                    GtcGtdEmailNotificationManager.GetInstance().SendActiveGtcGtdOrdersEmail();
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
    }
}
