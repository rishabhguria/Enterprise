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

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    class SearchShortLocateUI : ShortLocateUIMap, ITestStep
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
                OpenShortLocateUI();
                UltraPanel1ClientArea3.Click(MouseButtons.Left);
                DataRow row = testData.Tables[sheetIndexToName[0]].Rows[0];
                SearchSymbolUi(row);
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
                MinimizeShortLocate();
            }
            return _result;
        }

        /// <summary>
        /// Enter search filters
        /// </summary>
        /// <param name="row"></param>
        private void SearchSymbolUi(DataRow dr)
        {
            try
            {
                GrpBoxSearchSymbol1.Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_SYMBOL].ToString() != string.Empty)
                {
                    TextEditorSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_SYMBOL].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_QUANTITY].ToString() != string.Empty)
                {
                    NumericEditorShares.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[END][SHIFT+HOME][BACKSPACE]");
                    Keyboard.SendKeys(dr[TestDataConstants.COL_QUANTITY].ToString());
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr[TestDataConstants.COL_ClearFilters].ToString() != string.Empty && dr[TestDataConstants.COL_ClearFilters].ToString().Equals("True"))
                {
                    UltraPictureBoxEraser.Click(MouseButtons.Left);
                }
            }
            catch (Exception) { throw; }
        }
    }
}
