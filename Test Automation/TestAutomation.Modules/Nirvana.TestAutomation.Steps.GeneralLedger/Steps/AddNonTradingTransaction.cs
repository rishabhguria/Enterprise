using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    /// <summary>
    /// Add a new transaction
    /// </summary>
    class AddNonTradingTransaction : CashJournalUIMap, ITestStep
    {
        /// <summary>
        /// Add a new transaction
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenNonTradingTransaction();
                GrdNonTradingTransactions.Click(MouseButtons.Left);
                GrdNonTradingTransactions.Click(MouseButtons.Right);
                if (!PopupMenuDropDown.IsVisible)
                {
                    GrdNonTradingTransactions.Click(MouseButtons.Right);
                }
                AddTransaction1.Click(MouseButtons.Left);
                AddTransaction(testData, sheetIndexToName, GrdNonTradingTransactions);

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
                MinimizeGeneralLedger();
            }
            return _res;
        }

        /// <summary>
        /// Opens non trading transaction tab in Cash journal
        /// </summary>
        private void OpenNonTradingTransaction()
        {
            try
            {
                OpenCashJournal();
                NonTradingTransaction.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
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
