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
    public class DeleteCalculatedPreferencesTool : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCalculatedPreferences();
                DeletePreferenceFromToolbar(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DeleteCalculatedPreferencesTool");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;

        }

        /// <summary>
        /// Deletes the preference from toolbar.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private TestResult DeletePreferenceFromToolbar(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                string deletePref = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString();
                if (deletePref != string.Empty)
                {
                    Records.Click(MouseButtons.Left);
                    if (preferenceIndexMap.ContainsKey(deletePref))
                    {
                        Records.AutomationElementWrapper.FindDescendantByName(deletePref).WpfClick();
                        Delete.Click(MouseButtons.Left);
                        if (NirvanaPreferences.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
                    }
                    else
                    {
                        _res.AddResult(false, "Unable to Delete Prefrence");
                    }
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
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