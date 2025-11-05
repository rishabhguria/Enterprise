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
    public partial class ToggleBlombergOnRA : TTRestristedAllowedUIMap, ITestStep
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
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (dr[TestDataConstants.Col_Switch].ToString() != String.Empty && dr[TestDataConstants.Col_Switch].ToString().Equals("Bloomberg", StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonBloomberg.Click(MouseButtons.Left);
                    }
                    else if (dr[TestDataConstants.Col_Switch].ToString() != String.Empty && dr[TestDataConstants.Col_Switch].ToString().Equals("Ticker", StringComparison.InvariantCultureIgnoreCase))
                    {
                        RadioButtonTicker.Click(MouseButtons.Left);
                    }
                }
                if (RestrictedSecuritiesList.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
                }
                if (AllowedSecuritiesList.IsVisible)
                {
                    ButtonYes2.Click(MouseButtons.Left);
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
            
            return _result;
        }                   
    }
}
