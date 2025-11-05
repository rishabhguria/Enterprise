using Prana.LogManager;
using System;

using System.ServiceModel;
namespace Prana.WCFConnectionMgr
{
    public class ServiceModelHelper
    {
        public static InstanceContext SetInstanceContext(object obj)
        {
            InstanceContext context = null;
            try
            {
                context = new InstanceContext(obj);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return context;
        }

        public static string ReturnExceptionMessage(Exception exceptionreceived)
        {
            string message = exceptionreceived.Message;
            try
            {
                FaultException<PranaAppException> exception = exceptionreceived as FaultException<PranaAppException>;
                if (exception != null)
                {
                    message = exception.Detail.Message;
                }
                else
                {
                    message = exceptionreceived.Message;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return message;
        }
    }
}
