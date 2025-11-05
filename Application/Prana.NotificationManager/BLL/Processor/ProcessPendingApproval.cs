using Prana.AmqpAdapter.Amqp;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.NotificationManager.BLL.Processor
{
    internal static class ProcessPendingApproval
    {
        public static void Process(DataSet dsreceived)
        {
            try
            {
                if (dsreceived != null)
                {
                    var pendingApprovalObject = new { pendingApprovalObject = dsreceived };
                    AmqpHelper.SendObject(pendingApprovalObject, "NotificationSender", "ProcessPendingApproval");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

    }
}
