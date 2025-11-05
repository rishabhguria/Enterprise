using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using TestAutomationFX.Core;
using System.IO;
using System.Reflection;
namespace Nirvana.TestAutomation.Steps.Blotter
{
   public class BlotterExecutionReportSnapshot: BlotterExecutionReportUIMap,ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                BlotterReports.Click(MouseButtons.Left);
                UltraButton1.Click(MouseButtons.Left);
                Printpreview.WaitForObject();
                CaptureScreen();
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                
                MinimizeBlotterExecutionReport();
            }
            return _res;
        }

       /// <summary>
       /// method to take picture of the current screen
       /// </summary>
        private void CaptureScreen()
        {
            try
            {
                 string  screenShotFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                    if (!Directory.Exists(screenShotFolderPath))
                    {
                        Directory.CreateDirectory(screenShotFolderPath);
                    }
                    CommonMethods.CaptureScreenShot(PreviewControl.InstanceName , screenShotFolderPath);
            }
            catch ( Exception ex)
            {
                
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
  }

