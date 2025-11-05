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
    public class ValidateSymbolTT : TradingTicketUIMap, ITestStep
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
                OpenManualTradingTicket();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        ValidateSymbol(dr);
                        Wait(1000);
                    }
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
                Wait(2000);
                KeyboardUtilities.MinimizeWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// Validates the symbol.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <exception cref="System.Exception">Symbol does not exist in Database!</exception>
        private string ValidateSymbol(DataRow dr)
        {
            string errorMessage = string.Empty;
            string query = "SELECT * FROM T_SMSymbolLookUpTable WHERE TickerSymbol = " + "'" + dr[TestDataConstants.COL_SYMBOL].ToString() + "'";
            DataTable data = SqlUtilities.GetDataFromQuery(query);
            if (data == null)
            {
                errorMessage = "Symbol does not exist in Database";
                return errorMessage;
            }

            TxtSymbol.Click(MouseButtons.Left);
            ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_SYMBOL].ToString(), KeyboardConstants.ENTERKEY, true);
            Wait(1000);
            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            Wait(2000);



            if (!NmrcQuantity.IsEnabled && !NmrcPrice.IsEnabled)
            {
                errorMessage = "Symbol does not exist in Database";
            }
            return errorMessage;
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
