using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class ApplyBulkUpdateOnTaxlots : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenApplyBulkUpdateOnTaxlots();
                InputChanges(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ApplyBulkUpdateOnTaxlots");
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
        /// Opens the apply bulk update on taxlots
        /// </summary>
        private void OpenApplyBulkUpdateOnTaxlots()
        {
            try
            {
                OpenAllocation();
                Commisionbulkchange.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inputs the apply bulk update on taxlots
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void InputChanges(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                TaxlotLevelSpeceficCommissionDivideFxRates1.Click(MouseButtons.Left);

                if (!dr[TestDataConstants.COL_MASTER_FUND].ToString().Equals(String.Empty))
                {
                    ToggleBtnComboBox5.Click(MouseButtons.Left);
                    ClickOnComboBoxItem(dr[TestDataConstants.COL_MASTER_FUND].ToString(),ComboBox5);
                }

                if (!dr[TestDataConstants.COL_PB].ToString().Equals(String.Empty))
                {
                    ToggleBtnComboBox6.Click(MouseButtons.Left);
                    ClickOnComboBoxItem(dr[TestDataConstants.COL_PB].ToString(), ComboBox6);
                }

                if (!dr[TestDataConstants.COL_ACCOUNTS].ToString().Equals(String.Empty))
                {
                    XamComboEditor2.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_ACCOUNTS].ToString(), string.Empty, true);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (!dr[TestDataConstants.COL_FXRATE].ToString().Equals(String.Empty))
                {
                    if (!FxRate.IsChecked)
                        FxRate.Click(MouseButtons.Left);
                    FxRateEditor.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    KeyboardUtilities.PressKey(5, KeyboardConstants.BACKSPACEKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                    Keyboard.SendKeys(dr[TestDataConstants.COL_FXRATE].ToString());
                }
                if (!dr[TestDataConstants.COL_FXRATE_OPERATOR].ToString().Equals(String.Empty))
                {
                    if (!FxRateOperator.IsChecked)
                        FxRateOperator.Click(MouseButtons.Left);
                    ToggleBtnComboBox8.Click(MouseButtons.Left);
                    ClickOnComboBoxItem(dr[TestDataConstants.COL_FXRATE_OPERATOR].ToString(), ComboBox8);
                }

                UIMap.Wait(500);
                MouseController.ScrollWheelDown();
                UpdateAll.Click(MouseButtons.Left);
                UIMap.Wait(500);
                SavewDivideStatus.Click(MouseButtons.Left);
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