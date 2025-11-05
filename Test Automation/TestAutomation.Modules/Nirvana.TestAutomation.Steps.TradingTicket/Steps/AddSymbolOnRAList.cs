using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public partial class AddSymbolOnRAList : TTRestristedAllowedUIMap, ITestStep
    {
         /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRestrictedAllowedTab();
                AddSymbol(testData,sheetIndexToName);
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddSymbolOnRAList");
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

        private void AddSymbol(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                foreach (DataRow dr in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    TxtSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SYMBOL].ToString());
                    Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Wait(1000);
                    AddButton.Click(MouseButtons.Left);
                    Wait(12000);
                }
            }
            catch(Exception)
            {
                throw;
            }
           
        }

    }
}
