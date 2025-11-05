using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.UI;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class SelectTradeUsingMTT : MultiTradingTicketUIMap, ITestStep
    {
       
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                Wait(1000);
                //handling for popup for live pricing data not available
                //SelectAllTradeCheckBox.Click(MouseButtons.Left);
               // ColumnHeader.Click(MouseButtons.Left);
                MultiTradingTicket.InvokeMethod("UnselectAllOrders", null);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    SelectTrades(dr);
                }
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

             
    }
}
