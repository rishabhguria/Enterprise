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

namespace Nirvana.TestAutomation.Steps.Watchlist
{
    public class AddSymbolToWatchlist : WatchlistUIMap, ITestStep
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
                OpenWatchList();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddSymbolToWatchlist");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseWatchlist();
            }
            return _result;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {
                string TickerSymbol = "";
                
                if (dr.Table.Columns.Contains(TestDataConstants.COL_SYMBOL) && !string.IsNullOrEmpty(dr[TestDataConstants.COL_SYMBOL].ToString()))
                {
                    TickerSymbol = dr[TestDataConstants.COL_SYMBOL].ToString();
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_MARKPRICE_SYMBOL) && !string.IsNullOrEmpty(dr[TestDataConstants.COL_MARKPRICE_SYMBOL].ToString()))
                {
                    TickerSymbol = dr[TestDataConstants.COL_MARKPRICE_SYMBOL].ToString();
                }

                if (!string.IsNullOrEmpty(TickerSymbol))
                {

                    //   SymbolValue.Click(MouseButtons.Left);
                    //  Keyboard.SendKeys(dr[TestDataConstants.COL_SYMBOL].ToString());
                    // Wait(2000);
                    DataUtilities.KeyboardInputWithVerification(SymbolValue, TickerSymbol);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                    AddSymbolBtn.Click(MouseButtons.Left);
                    AddSymbolBtn.Click(MouseButtons.Left);
                    ClearOldSymbolFromSearchBox(TickerSymbol);
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void ClearOldSymbolFromSearchBox(string TickerSymbol)
        {
            try
            {
                SymbolValue.Click(MouseButtons.Left);
                int size = TickerSymbol.Length;
                while (size != 0)
                {
                    SymbolValue.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    size--;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
