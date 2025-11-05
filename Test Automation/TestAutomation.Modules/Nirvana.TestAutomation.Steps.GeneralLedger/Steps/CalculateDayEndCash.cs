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

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// To calculate the day end cash
    /// </summary>
    class CalculateDayEndCash: DayEndCashUIMap, ITestStep
    {
        /// <summary>
        /// Commands to calculate day end cash
        /// </summary>
        /// <param name="testData">Test case data</param>
        /// <param name="sheetIndexToName">Sheet name of step</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenDayEndCash();
                BtnRunBatch.Click(MouseButtons.Left);
                Wait(100);
                if (DayEndCash1.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
                }

                BtnSave.Click(MouseButtons.Left);
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
                if (!NAVLock.IsVisible)
                {
                    MinimizeGeneralLedger();
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
