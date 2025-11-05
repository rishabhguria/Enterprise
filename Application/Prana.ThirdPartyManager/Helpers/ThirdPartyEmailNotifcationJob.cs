using Prana.LogManager;
using Prana.ThirdPartyManager.Helper;
using Quartz;
using System;

namespace Prana.ThirdPartyManager.Helpers
{
    public class ThirdPartyEmailNotifcationJob : IJob
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(JobExecutionContext context)
        {
            try
            {
                if (context.JobDetail.Name.Equals(ThirdPartyEmailHelper.CONST_JOB_NAME))
                {
                    ThirdPartyEmailHelper.SendScheduledJobStatusEmail();
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
