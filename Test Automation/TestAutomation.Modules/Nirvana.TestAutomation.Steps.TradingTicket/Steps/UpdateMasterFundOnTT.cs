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
    class UpdateMasterFundOnTT : TradingTicketUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string masterFund = String.Empty;
                OpenManualTradingTicket();
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    masterFund = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_MASTER_FUND].ToString();
                    CmbFunds.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(masterFund, KeyboardConstants.ENTERKEY, true);
                    Wait(4000);
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
