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
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class EnterAccountValuesToAllocate : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Test execution begins here
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                AllocateManually(testData, sheetIndexToName);
                PinnedIcon.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EnterAccountValuesToAllocate");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally 
            {
                MinimizeAllocation();
            }
            return _res;
        }


        /// <summary>
        /// Allocates the manually.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void AllocateManually(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                ClickOnAccountStrategyGrid();
                Clear.Click(MouseButtons.Left);
                AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (row[TestDataConstants.COL_ACCOUNT_NAME].ToString() != string.Empty)
                    {
                        AllocateTrade(row);
                    }
                }
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.CTRLA + KeyboardConstants.BACKSPACEKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Clicks the on account strategy grid.
        /// </summary>
        private void ClickOnAccountStrategyGrid()
        {
            try
            {
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
                AccountStartegyGrid1.Click(MouseButtons.Left);
                PinnedIcon.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Allocates the trades.
        /// </summary>
        /// <param name="row">The row.</param>
        private void AllocateTrade(DataRow row)
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.CTRLA + KeyboardConstants.BACKSPACEKEY);
                if (row != null)
                    Keyboard.SendKeys(row[TestDataConstants.COL_ACCOUNT_NAME].ToString());
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                if (!string.IsNullOrEmpty(row[TestDataConstants.COL_ACCOUNT_QUANTITY].ToString()))
                {
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(row[TestDataConstants.COL_ACCOUNT_QUANTITY].ToString() + KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.SHIFT_TABKEY);
                }
                if (!string.IsNullOrEmpty(row[TestDataConstants.COL_ACCOUNT_PERCENTAGE].ToString()))
                    Keyboard.SendKeys(row[TestDataConstants.COL_ACCOUNT_PERCENTAGE].ToString() + KeyboardConstants.ENTERKEY);
                Keyboard.SendKeys(KeyboardConstants.SHIFT_TABKEY);
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
