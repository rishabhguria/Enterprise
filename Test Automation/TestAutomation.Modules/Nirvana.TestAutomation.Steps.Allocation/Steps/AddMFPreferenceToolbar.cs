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
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class AddMFPreferenceToolbar : MasterFundPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// begins the test execution process
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
                AddMFPreference(testData, sheetIndexToName);
               
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddMFPreferenceToolbar");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeEditAllocationPreference();
            }
            return _res;
        }

        /// <summary>
        /// Add Master Fund Preference
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void AddMFPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    string prefname = row[TestDataConstants.COL_PREFERENCE_NAME].ToString();
                    Add.Click(MouseButtons.Left);
                    Keyboard.SendKeys(prefname);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    if (NirvanaPreferences.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
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

