using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System;
using System.Collections.Specialized;

namespace Prana.LogManager.EnterpriseLibrary
{
    /// <summary>
    /// Summary description for GlobalPolicyExceptionHandler.
    /// </summary>
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class MessageReportingExceptionHandler : IExceptionHandler
    {
        public MessageReportingExceptionHandler()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "ignore")]
        public MessageReportingExceptionHandler(NameValueCollection ignore)
        {
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            try
            {
                InformationReporter.GetInstance.Write(exception.Message);
            }
            catch (Exception)
            {
            }
            return exception;
        }

        public Exception HandleException(Exception exception)
        {
            try
            {
                InformationReporter.GetInstance.Write(exception.Message);
            }
            catch (Exception)
            {
            }
            return exception;
        }
    }
}