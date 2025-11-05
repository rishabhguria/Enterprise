using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Steps.Closing.Classes;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Closing
{
    class UnwindClosing: ClosingUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenClosingUI();
                ClosedAmend.Click(MouseButtons.Left);
                ClosingCommonMethods.UnwindClosingData(testData, sheetIndexToName, GrdNetPosition);
                Unwind.Click(MouseButtons.Left);
                if (Warning1.IsVisible) {
                    Warning1.BringToFront();
                    ButtonYes1.Click(MouseButtons.Left);
                }

                if (MessageLabel.IsVisible)
                { Okbutton.Click(MouseButtons.Left); }

            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                if (!NAVLock.IsEnabled)
                {
                    MinimizeClosing();
                }
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
