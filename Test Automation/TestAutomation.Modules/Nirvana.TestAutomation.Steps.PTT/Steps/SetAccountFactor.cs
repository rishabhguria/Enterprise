using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PTT
{
    class SetAccountFactor : PTTPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Run the step.
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    OpenPTTPreference();
                   // Wait(2000);
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputParametersPTTPreferences(dr);
                    }

                    BtnSave.Click(MouseButtons.Left);
                    Wait(2000);
                    BtnClose.Click(MouseButtons.Left);
                  //  Wait(2000);

                }
            }
            catch (Exception ex )
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

        /// <summary>
        /// set account factor 
        /// </summary>
        /// <param name="dataRow">Factor data.</param>
        public void InputParametersPTTPreferences(DataRow dataRow)
        {
            try
            {
                var msaaObj = GrdAccountFactor.MsaaObject;
                DataTable dtAccountFactor = CSVHelper.CSVAsDataTable(this.GrdAccountFactor.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRow = dtAccountFactor.Select(String.Format(@"[" + TestDataConstants.COL_ACCOUNT + "]='{0}'", dataRow[TestDataConstants.COL_ACCOUNT]));
                Wait(1000);

                if (foundRow.Length > 0)
                {
                    int index = dtAccountFactor.Rows.IndexOf(foundRow[0]);
                    GrdAccountFactor.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);

                    msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName(TestDataConstants.COL_ACCOUNT, 3000).Click(MouseButtons.Left);
                    Wait(2000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(dataRow[TestDataConstants.COL_ACCOUNT_FACTOR].ToString());

                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }
    }
}
