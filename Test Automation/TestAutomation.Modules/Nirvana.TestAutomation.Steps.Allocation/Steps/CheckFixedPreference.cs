using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.Text;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class CheckFixedPreference : FixedPreferencesUIMap, ITestStep
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
                OpenFixedPreferences();
                CheckFixedPreferenceName(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckFixedPreference");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseFixedPreferences();
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Checks the name of the fixed preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.Exception"></exception>
        private void CheckFixedPreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                StringBuilder error = new StringBuilder(String.Empty);
                GetScheme.Click(MouseButtons.Left);
                Records = GetLatestGridObject(FixedPreferenceGrid);
                Export1.Click(MouseButtons.Left);
               
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                TextBoxFilename.Click(MouseButtons.Left);
                clearText(TextBoxFilename);
                Keyboard.SendKeys(path + ExcelStructureConstants.FIXEDPREFERENCE_FILENAME);
                ButtonSave.Click(MouseButtons.Left);
                if (ConfirmSaveAs.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }

                ButtonNo1.Click(MouseButtons.Left);

                ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(path + ExcelStructureConstants.FIXEDPREFERENCE_FILENAME);
                DataTable exportedPref = testCases.Tables[0];
                List<String> columns = new List<String>();
                columns.Add(TestDataConstants.COL_TICKER_SYMBOL);
                columns.Add(TestDataConstants.COL_SIDE);

                var reconErrors = Recon.RunRecon(exportedPref, testData.Tables[sheetIndexToName[0]], columns, 0.01);
                if (reconErrors.Count > 0)
                    error.Append("Errors:-" + String.Join("\n\r", reconErrors));
                if (!string.IsNullOrEmpty(error.ToString()))
                    throw new Exception("Fixed preference details have not been matched!." +error.ToString());
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
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