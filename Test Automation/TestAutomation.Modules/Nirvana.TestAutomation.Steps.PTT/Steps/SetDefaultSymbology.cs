using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using TestAutomationFX.Core;
using TestAutomationFX.UI;


namespace Nirvana.TestAutomation.Steps.PTT
{
    class SetDefaultSymbology : PTTPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Run the step.
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenPTTPreference();
                // New handling for default symbbology
                ExtentionMethods.WaitForVisible(ref PreferencesMain, 10);
                if (PreferencesMain.IsVisible)
                {
                    //Tools.Click(MouseButtons.Left);
                    //Preferences.Click(MouseButtons.Left);
                    Trading.Click(MouseButtons.Left);
                    GeneralPreferences.Click(MouseButtons.Left);
                }
                setSymbologytoDefault(testData, sheetIndexToName);
                BtnSave.DoubleClick(MouseButtons.Left);
                Wait(1000);
                ClosePreferencePTT();
               // Wait(2000);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _result;
        }
        /// <summary>
        /// Set Input Parameters of PTT Preference Module
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name</param>
        protected void setSymbologytoDefault(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (dataRow[TestDataConstants.COL_SYMBOLOGY].ToString() != String.Empty)// Select Symbology to Default
                    {
                        CmbSymbology.Click(MouseButtons.Left);
                        CmbSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_SYMBOLOGY].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
