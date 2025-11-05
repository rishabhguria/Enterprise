using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.SymbolLookup
{
    public class UpdateSecurityInfoOnSM : SymbolLookupUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenSymbolLookup();
                if (testData != null)
                {
                    foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                    {
                        uiMsaa1.DoubleClick(MouseButtons.Left);
                        BtnNextTab.Click(MouseButtons.Left);
                        UgpcShares.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dtRow[TestDataConstants.COL_Shares_Outstanding].ToString());
                        Wait(2000);
                    }
                    BtnOK.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseSymbolLookup();
               // Wait(3000);
            }

            return _result;
        }
    }
}
