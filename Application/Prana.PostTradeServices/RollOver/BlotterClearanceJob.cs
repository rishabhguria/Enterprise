using Prana.LogManager;
using Quartz;
using System;

namespace Prana.PostTradeServices.RollOver
{
    internal class BlotterClearanceJob : IJob
    {
        #region IJob Members
        public void Execute(JobExecutionContext context)
        {
            try
            {
                ClearanceManager.GetInstance.ClearTradesByAUECID(context);
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