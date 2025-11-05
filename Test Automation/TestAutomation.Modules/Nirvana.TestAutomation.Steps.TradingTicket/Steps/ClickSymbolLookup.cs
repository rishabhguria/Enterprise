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
  public  class ClickSymbolLookup : TradingTicketUIMap, ITestStep
    {
      
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
                        if (TxtSymbol.IsEnabled && !string.IsNullOrEmpty(dr[TestDataConstants.COL_SYMBOL].ToString()))
                        {
                            TxtSymbol.Click(MouseButtons.Left);
                            ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_SYMBOL].ToString(), KeyboardConstants.ENTERKEY, true);
                            Wait(1000);
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            Wait(2000);
                        }
                        BtnSymbolLookup1.Click(MouseButtons.Left);
                       // Wait(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ClickSymbolLookup");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
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