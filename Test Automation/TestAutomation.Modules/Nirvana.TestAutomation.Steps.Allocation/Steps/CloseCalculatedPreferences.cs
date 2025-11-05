using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class CloseCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;
                Close.Click(MouseButtons.Left);
              //  Wait(1000);
                ClosePopup();
                if (EditAllocationPreferences2.IsVisible)
                    Close.Click(MouseButtons.Left);
              //  Wait(1000);
                ClosePopup();

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CloseCalculatedPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {

                MinimizeAllocation();
            }
            return _res;
        }

        private void ClosePopup()
        {
            try
            {
                if (EditAllocationPreferences2.IsVisible && NirvanaPreferences.IsVisible)
                {
                    if (ButtonYes.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    else if(ButtonOK.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                  //  Wait(1000);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
