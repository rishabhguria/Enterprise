using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Collections.Specialized;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Utilities
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class CaptureExceptionHandler : IExceptionHandler
    {
        public CaptureExceptionHandler()
        { }
        public CaptureExceptionHandler(NameValueCollection ignore)
        {
        }
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            LogCapture(exception);
            return exception;
        }
        
        /// <summary>
        /// Capture screen shot and log to file.
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="stepName"></param>
        private void LogCapture(Exception exception)
        {
            try
            {
                String currentTime = String.Format("{0:HH_mm tt}", DateTime.Now);
                String screenShotName = exception.Source + "_" + currentTime;
                String screenShotFolderPath = CommonMethods.GetDirectoryPathForScreenshot(ApplicationArguments.TestCaseToBeRun);
                ScreenCapture.SaveScreenShot(screenShotFolderPath + "\\" + screenShotName + ".jpeg");

                // Write screenshot information into file.
                String reason = "Screenshot_" + exception.Source + "_captured_" + String.Format("{0:HH_mm tt}", DateTime.Now) + "_" + DateTime.Now.ToString("MM-dd-yyyy");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
