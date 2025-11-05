using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class DeleteCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
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
               _res= DeletePreferenceName(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DeleteCalculatedPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;

        }

        /// <summary>
        /// Deletes the name of the preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.Exception">Could not found required preference to delete</exception>
        private TestResult DeletePreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
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
                        Records.AutomationElementWrapper.FindDescendantByName(deletePref).CachedChildren[0].CachedChildren[0].WpfClickLeftBound(MouseButtons.Right);
                        DeletePreference.Click(MouseButtons.Left);
                        if (NirvanaPreferences.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
                    }
                    else 
                    {
                        _res.AddResult(false, "Unable to Delete Calculated Prefernce");
                       // throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND + " for deletion.");
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