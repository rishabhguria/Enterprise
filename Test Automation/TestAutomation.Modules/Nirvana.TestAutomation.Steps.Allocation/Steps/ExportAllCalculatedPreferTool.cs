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
    public class ExportAllCalculatedPreferTool : CalculatedPreferencesUIMap, ITestStep
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
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                ExportAllPreferenceFromToolbar(testData,sheetIndexToName);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ExportAllCalculatedPreferTool");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;

        }

        /// <summary>
        /// Exports all preference from toolbar.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExportAllPreferenceFromToolbar(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                string exportFolderName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FOLDER_NAME].ToString();
                ExportAll1.Click(MouseButtons.Left);
                if (BrowseForFolder.IsVisible)
                {
                    ButtonMakeNewFolder.Click(MouseButtons.Left);
                    Keyboard.SendKeys(exportFolderName);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    if (ConfirmFolderReplace.IsVisible)
                        ButtonYes1.Click(MouseButtons.Left);
                    ButtonOK1.Click(MouseButtons.Left);
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