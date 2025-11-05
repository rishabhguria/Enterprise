using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using System.Collections.Generic;
using System;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class ExportAllCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
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
                OpenCalculatedPreferences();
                ExportAllCalculatedPerferencesName(testData,sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ExportAllCalculatedPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Exports the name of all calculated perferences.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void ExportAllCalculatedPerferencesName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                string exportFolderName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FOLDER_NAME].ToString();
                Records.Click(MouseButtons.Left);
                Records.ClickRightBound(MouseButtons.Right);
                ExportAll.Click(MouseButtons.Left);
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
