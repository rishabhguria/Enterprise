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
namespace Nirvana.TestAutomation.Steps.Closing
{
    class AutomaticClosing : ClosingUIMap, ITestStep
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
                OpenClosingUI();
                ClosedAmend.Click(MouseButtons.Left);
                CmbMethodology.Click(MouseButtons.Left);
                Keyboard.SendKeys(TestDataConstants.COL_AUTOMATIC + KeyboardConstants.ENTERKEY);
                AutomaticClose(testData, sheetIndexToName);
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
                MinimizeClosing();
            }
            return _result;
        }

        /// <summary>
        /// Automatic Close
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private TestResult AutomaticClose(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];

                foreach (DataRow dataRow in excelFileData.Rows)
                {
                    if (Convert.ToBoolean(dataRow[TestDataConstants.COL_CLOSESSW_BUYBTW].ToString()) && !ChkBoxBuyAndBuyToCover.IsChecked)
                        ChkBoxBuyAndBuyToCover.Click(MouseButtons.Left);
                    else if (ChkBoxBuyAndBuyToCover.IsChecked && !Convert.ToBoolean(dataRow[TestDataConstants.COL_CLOSESSW_BUYBTW].ToString()))
                        ChkBoxBuyAndBuyToCover.Click(MouseButtons.Left);

                    if (Convert.ToBoolean(dataRow[TestDataConstants.COL_AUTOCLOSE_STRATEGY].ToString()) && !ChkBxIsAutoCloseStrategy.IsChecked)
                        ChkBxIsAutoCloseStrategy.Click(MouseButtons.Left);
                    else if (ChkBxIsAutoCloseStrategy.IsChecked && !Convert.ToBoolean(dataRow[TestDataConstants.COL_AUTOCLOSE_STRATEGY].ToString()))
                        ChkBxIsAutoCloseStrategy.Click(MouseButtons.Left);

                    CmbAlgorithm.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.COL_ALGORITHM].ToString() + KeyboardConstants.ENTERKEY);

                    CmbSecondarySort.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.COL_SECONDARYSORT_CRITERIA].ToString() + KeyboardConstants.ENTERKEY);

                    CmbClosingField.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.COL_CLOSING_FIELD].ToString() + KeyboardConstants.ENTERKEY);

                    BtnRun.Click(MouseButtons.Left);
                    if (InformationMsgButtonOK.IsVisible)
                        InformationMsgButtonOK.Click(MouseButtons.Left);
                    if (CloseTradeErrorButtonOK.IsVisible)
                        CloseTradeErrorButtonOK.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
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
