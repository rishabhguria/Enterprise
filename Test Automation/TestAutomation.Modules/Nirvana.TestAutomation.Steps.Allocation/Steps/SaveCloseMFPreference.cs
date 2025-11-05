using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class SaveCloseMFPreference : MasterFundPreferencesUIMap, ITestStep
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
                OpenMFPreferences();
                Save.Click(MouseButtons.Left);
               // Wait(1000);
                ClosePopups();
                Save.Click(MouseButtons.Left);
              //  Wait(1000);
                ClosePopups();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SaveCloseMFPreference");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally 
            {
                CloseMFPreference();
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Closes the popups.
        /// </summary>
        private void ClosePopups()
        {
            try
            {
                while (NirvanaPreferences.IsVisible)
                {
                    if (ButtonYes.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    else
                        ButtonOK.Click(MouseButtons.Left);
                    Wait(1000);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Closes the mf preference.
        /// </summary>
        protected void CloseMFPreference()
        {
            try
            {
                Close.Click(MouseButtons.Left);
              //  Wait(1000);
                ClosePopup();
                if (EditAllocationPreferences.IsVisible)
                    Close.Click(MouseButtons.Left);
              //  Wait(1000);
                ClosePopup();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Closes the popup.
        /// </summary>
        private void ClosePopup()
        {
            try
            {
                if (EditAllocationPreferences.IsVisible && NirvanaPreferences.IsVisible)
                {
                    if (ButtonYes.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    else if (ButtonOK.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                    Wait(1000);
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
