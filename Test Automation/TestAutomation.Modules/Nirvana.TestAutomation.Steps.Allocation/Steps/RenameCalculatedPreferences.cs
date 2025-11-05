using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class RenameCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
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
                RenamePreferenceName(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "RenameCalculatedPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Renames the preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private void RenamePreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                string renameFromPrefName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_OLD_PREF_NAME].ToString();
                if (!renameFromPrefName.Equals(string.Empty))
                {
                        Records.Click(MouseButtons.Left);
                        if (preferenceIndexMap.ContainsKey(renameFromPrefName))
                        {
                            Records.AutomationElementWrapper.FindDescendantByName(renameFromPrefName).CachedChildren[0].CachedChildren[0].WpfClickLeftBound(MouseButtons.Right);
                            RenamePreference.Click(MouseButtons.Left);
                            Keyboard.SendKeys(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_NEW_PREF_NAME].ToString());
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            if (NirvanaPreferences.IsVisible)
                            {
                                ButtonOK.Click(MouseButtons.Left);
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            }
                        }
                        else
                        {
                            throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND + " for rename");
                        }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
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
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}