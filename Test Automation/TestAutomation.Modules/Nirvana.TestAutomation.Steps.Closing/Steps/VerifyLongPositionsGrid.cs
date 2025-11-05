using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.IO;
using Nirvana.TestAutomation.Steps.Closing.Classes;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Closing
{
    class VerifyLongPositionsGrid : ClosingUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                //Shortcut to open closing module(CTRL + ALT + C )
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_CLOSING"]);
                //Wait(5000);
                //PortfolioManagement.Click(MouseButtons.Left);
                //ClosePositions.Click(MouseButtons.Left);
                ExtentionMethods.WaitForVisible(ref CloseTrade, 10);
                if (CloseTrade.IsVisible)
                {
                    ClosedAmend.Click(MouseButtons.Left);
                    BtnRefresh.Click(MouseButtons.Left);
                    _res = ClosingCommonMethods.VerifyClosingData(testData, sheetIndexToName[0], GrdLong);
                    return _res;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeClosing();
            }
            return _res;
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
