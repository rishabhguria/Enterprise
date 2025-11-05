using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Prana.LogManager.EnterpriseLibrary
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class PopupMessageExceptionHandler : IExceptionHandler
    {
        public PopupMessageExceptionHandler()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "ignore")]
        public PopupMessageExceptionHandler(NameValueCollection ignore)
        {
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            ShowThreadExceptionDialog(exception);

            return exception;
        }

        // Creates the error message and displays it.
        private DialogResult ShowThreadExceptionDialog(Exception ex)
        {
            return MessageBox.Show(ServiceModelHelper.ReturnExceptionMessage(ex), "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}
