using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    class SetTTCompliancePreference : TTGeneralPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not Able to Set Compliance Preferences.!\n (" + ex.Message + " )</exception>

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenTTCompliancePreference();
                     if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputDetailsInTTCompliancePreferences(dr);
                        Wait(2000);
                        BtnSave.Click(MouseButtons.Left);
                    }
                }
                else
                {
                    _result.AddResult(false, "Input data is not provided");
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetTTCompliancePreference");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// Inputs the details in compliancce preferences.
        /// </summary>
        /// <param name="dr">The dr.</param>

        private void InputDetailsInTTCompliancePreferences(DataRow dr)
        {
            try
            {
                if ((dr[TestDataConstants.Col_PopUpTargetQty].ToString().ToUpper().Equals("TRUE",StringComparison.InvariantCultureIgnoreCase)))
                {
                    Yes.Click(MouseButtons.Left);
                }
                else 
                {
                    No.Click(MouseButtons.Left);
                }

                if ((dr[TestDataConstants.COL_PopUpNewOrder].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase)))
                {
                    Yes1.Click(MouseButtons.Left);
                }
                else
                {
                    No1.Click(MouseButtons.Left);
                }

                if ((dr[TestDataConstants.COL_PopUpCXL].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase)))
                {
                    Yes2.Click(MouseButtons.Left);
                }
                else
                {
                    No2.Click(MouseButtons.Left);
                }

                if ((dr[TestDataConstants.COL_PopUpReplace].ToString().ToUpper().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase)))
                {
                    Yes3.Click(MouseButtons.Left);
                }
                else
                {
                    No3.Click(MouseButtons.Left);
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
