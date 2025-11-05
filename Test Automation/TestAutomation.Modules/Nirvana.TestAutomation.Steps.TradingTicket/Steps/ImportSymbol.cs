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
    public partial class ImportSymbol : TTRestristedAllowedUIMap, ITestStep
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
                ImportSecuritiesButton.Click(MouseButtons.Left);
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                List<string> colList = new List<string>();
                foreach (DataColumn col in testData.Tables[sheetIndexToName[0]].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                foreach (string colName in colList)
                {
                    if (colName.Equals(TestDataConstants.COL_SELECT_FILE))
                    {
                        ButtonOpen.Click();
                        TextBoxFilename.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[colName].ToString());
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        Wait(20000);
                    }
                    ButtonOK.Click(MouseButtons.Left);
                }
                BtnSave.Click(MouseButtons.Left);
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
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }
    }
}
