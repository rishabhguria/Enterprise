using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
  public  class EnterDetailsOnTT:TradingTicketUIMap,ITestStep
    {
     
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                String symbol = String.Empty;
                OpenManualTradingTicket();
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    symbol = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_SYMBOL].ToString();
                    TxtSymbol.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(symbol, KeyboardConstants.ENTERKEY, true);
                    Wait(4000);
                }
                BtnSymbolLookup.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EnterDetailsOnTT");
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
