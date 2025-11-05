using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class GetFixedPreference : FixedPreferencesUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenFixedPreferences();
                GetFixedPreferenceName(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "GetFixedPreference");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Gets the name of the fixed preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.Exception">FixedPreference column is empty in excel</exception>
        private void GetFixedPreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                string prefName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString();
                string text = PART_EditableTextBox.Value;
                if (!prefName.Equals(String.Empty))
                {
                    if(prefName.Equals(text))
                        GetScheme.Click(MouseButtons.Left);
                    else
                    {
                      ComboBox.ClickRightBound(MouseButtons.Left);
                    Wait(2000);
                   ClickOnComboBoxItem(prefName, ComboBox);
                        GetScheme.Click(MouseButtons.Left);
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
