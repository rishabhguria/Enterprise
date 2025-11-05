using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class OpenClosingFromPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">test data.</param>
        /// <param name="sheetIndexToName">sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
               // Wait(5000);
                Main.Click(MouseButtons.Left);
                ClosePositions.Click(MouseButtons.Left);
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
                PMclose();
            }
            return _result;
        }
    }
}
