using Prana.LogManager;
using Quartz;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    class ClearanceJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            //Call the refresh method here.
            List<int> auecsList = new List<int>();
            auecsList = (List<int>)context.JobDetail.JobDataMap["AUECID"];
            try
            {
                if (!ExPnlCache.Instance.IsEpnlRunning)
                {
                    return;
                }
                ExPnlCache.Instance.RefreshExPNLData(auecsList);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
