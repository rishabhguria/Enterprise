using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Allocation
{
   public class ImportCalculatedPreferences : CalculatedPreferencesUIMap, ITestStep
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
                ImportCalculatedPreferenceName(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ImportCalculatedPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Imports the calculated name of the preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ImportCalculatedPreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Records.Click(MouseButtons.Left);
                Records.ClickRightBound(MouseButtons.Right);
                Import.Click(MouseButtons.Left);

                string importFilePath = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_IMPORT_FILE_PATH].ToString();
                if (!importFilePath.Equals(string.Empty))
                {
                    if (Open.IsVisible)
                        Keyboard.SendKeys(importFilePath);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    if (NirvanaPreferences.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                    if (Open1.IsVisible && Open.IsVisible)
                    {
                        ButtonOK2.Click(MouseButtons.Left);
                        ButtonCancel1.Click(MouseButtons.Left);
                    }
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
