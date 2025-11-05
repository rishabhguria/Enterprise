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
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Closing
{
    class ClosingCleanUpUI : ClosingUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingUI();
                if (NoData.IsVisible)
                    NoDataButtonOK.Click(MouseButtons.Left);
                ClosedAmend.Click(MouseButtons.Left);
                GetHistoricalData();

                //For Closed Amend, Closing History Grid
                ClosedAmend.Click(MouseButtons.Left);
                ColumnHeader.Click(MouseButtons.Left);
                GrdNetPosition.Click(MouseButtons.Left);
                SelectAllUnwind();

                //For Expiration/Settlement, Espired/Settled Grid
                ExpirationDivideSettlement.Click(MouseButtons.Left);
                ColumnHeader1.Click(MouseButtons.Left);
                GrdAccountExpired.Click(MouseButtons.Left);
                SelectAllUnwind();
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

        /// <summary>
        /// Get The Historical Data for CleanUP
        /// </summary>
        private string GetHistoricalData()
        {
            string errorMessage = string.Empty;
            try
            {
                ExtentionMethods.WaitForEnabled(ref RbHistorical, 10);
                if (RbHistorical.IsEnabled)
                {
                    RbHistorical.Click(MouseButtons.Left);
                    FromDatePicker.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(TestDataConstants.CONST_DEFAULT_START_DATE);
                    ToDatePicker.Click(MouseButtons.Left);
                    BtnRefresh.Click(MouseButtons.Left);
                    if (NoData.IsVisible)
                    {
                        NoDataButtonOK.Click(MouseButtons.Left);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Select All Unwind
        /// </summary>
        private string SelectAllUnwind()
        {
            string errorMessage = string.Empty;
            try
            {
                MouseController.RightClick();
                MouseController.RightClick();
                Unwind.Click(MouseButtons.Left);
                if (Warning1.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Select All Check Box is not Checked.!\n ( " + ex.Message + " )";
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
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
