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
    class CloseOrder : CloseOrderTabUIMAP, ITestStep
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
				CloseOrderTab.Click(MouseButtons.Left);
                ClosedOrder(testData, sheetIndexToName);
               // return _result;
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
        ///Close Order
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private TestResult ClosedOrder(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                DataTable excelFileData = testData.Tables[sheetIndexToName[0]];

                foreach (DataRow dataRow in excelFileData.Rows)
                {
                   
                    CmbAccounts.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.CONST_ACCOUNT].ToString() + KeyboardConstants.ENTERKEY);

                    CmbStrategy.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.CONST_STRATEGY].ToString() + KeyboardConstants.ENTERKEY);

                    CmbClosingMethodlogy.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.CONST_CLOSING_METHODOLOGY].ToString() + KeyboardConstants.ENTERKEY);

                    CmbClosingField.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dataRow[TestDataConstants.CONST_CLOSING_FIELD].ToString() + KeyboardConstants.ENTERKEY);

                    if (Convert.ToBoolean(dataRow[TestDataConstants.COL_AUTOCLOSE_STRATEGY].ToString()) && !ChkMatchStrategy.IsChecked)
                        ChkMatchStrategy.Click(MouseButtons.Left);
                    else if (ChkMatchStrategy.IsChecked && !Convert.ToBoolean(dataRow[TestDataConstants.COL_AUTOCLOSE_STRATEGY].ToString()))
                        ChkMatchStrategy.Click(MouseButtons.Left);

                    BtnClose.Click(MouseButtons.Left);
                    if (Information.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                    BtnSave.Click(MouseButtons.Left);
                    if (Information.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
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
