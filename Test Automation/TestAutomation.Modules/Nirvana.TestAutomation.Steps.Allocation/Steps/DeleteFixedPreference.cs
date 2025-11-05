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
    class DeleteFixedPreference : FixedPreferencesUIMap, ITestStep
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
                DeleteFixedPreferenceName(testData, sheetIndexToName);

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DeleteFixedPreference");
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
        /// Deletes the name of the fixed preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void DeleteFixedPreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
              
                string deletePref = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString();
                string text = PART_EditableTextBox.Value;
                if (deletePref != string.Empty)
                {
                    if (deletePref.Equals(text))
                    {
                        Delete.Click(MouseButtons.Left);
                        if (NirvanaPreferences.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
                    }
                    else
                    {
                        DropdownButton.Click(MouseButtons.Left);
                        Wait(2000);
                        ClickOnComboBoxItem(deletePref, ComboBox);
                        Delete.Click(MouseButtons.Left);
                        if (NirvanaPreferences.IsVisible)
                            ButtonYes.Click(MouseButtons.Left);
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
