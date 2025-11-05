using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class CopyCalculatedPreferenceToolbar : CalculatedPreferencesUIMap, ITestStep
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
                CopyPreferenceFromToolbar(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CopyCalculatedPreferenceToolbar");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Copies the preference from toolbar.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private TestResult CopyPreferenceFromToolbar(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                string copyFromPrefName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_COPY_PREF_FROM].ToString();
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                if (!copyFromPrefName.Equals(string.Empty))
                {
                    Records.Click(MouseButtons.Left);
                    if (preferenceIndexMap.ContainsKey(copyFromPrefName))
                    {
                        Records.AutomationElementWrapper.FindDescendantByName(copyFromPrefName).WpfClick();
                        Copy.Click(MouseButtons.Left);
                        Keyboard.SendKeys(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_NEW_PREF_NAME].ToString());
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        if (NirvanaPreferences.IsVisible)
                            ButtonOK.Click(MouseButtons.Left);
                    }
                    else
                    {
                        _res.AddResult(false,"Unable to copy prefernce from Toolbar");
                       // throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND+" for copy from Toolbar");
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
