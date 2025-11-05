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
    class DeleteMFPreferences : MasterFundPreferencesUIMap, ITestStep
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
                OpenMFPreferences();
                DeletePreferenceName(testData, sheetIndexToName);
              
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "DeleteMFPreferences");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeEditAllocationPreference();
            }
            return _res;
        }

        /// <summary>
        /// Deletes the name of the preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.Exception">Could not found required preference to delete</exception>
        private void DeletePreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
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
                        throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND + " for deletion.");
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
