using Prana.LogManager;
using Quartz;
using System;

namespace Prana.PostTradeServices
{
    class ManualOrderSendSchedularDataJob : IJob
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(JobExecutionContext context)
        {
            try
            {
                int auecId = Convert.ToInt32(context.JobDetail.JobDataMap["AUECID"].ToString());
                DateTime lastRunTime = Convert.ToDateTime(context.JobDetail.JobDataMap["LastManualOrderRunTriggerTime"].ToString());
                int companyId = Convert.ToInt32(context.JobDetail.JobDataMap["companyId"].ToString());
                ManualOrderSendHelper.ProcessManualOrderSend(auecId, lastRunTime, companyId);
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