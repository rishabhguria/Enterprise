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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class ApplyBulkUpdateOnGroups : AllocationUIMap, ITestStep
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
                OpenApplyBulkUpdateOnGroups();
                InputChanges(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ApplyBulkUpdateOnGroups");
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
        /// Opens the Commission Attribute bulk change.
        /// </summary>
        private void OpenApplyBulkUpdateOnGroups()
        {
            try
            {
                //  Shortcut to open allocation module(CTRL + SHIFT + A)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_ALLOCATION"]);
                ExtentionMethods.WaitForVisible(ref Allocation, 15);
                //Wait(15000);
                //Trade.Click(MouseButtons.Left);
                //Allocation2.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                Commisionbulkchange.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw ;
            }
        }

        /// <summary>
        /// Inputs the Commission Attribute Bulk Changes on Groups.
        /// </summary>
        private void InputChanges(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];

                GroupLevelDefaultCommissionsDivideBulkUpdates.Click(MouseButtons.Left);
                if (!dr[TestDataConstants.COL_AVG_PRICE].ToString().Equals(String.Empty))
                {
                    if (!AvgPrice.IsChecked)
                        AvgPrice.Click(MouseButtons.Left);
                    AvgPriceEditor.Click(MouseButtons.Left);
                    ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_AVG_PRICE].ToString(), string.Empty);
                }

                if (!dr[TestDataConstants.COL_ACCRUED_INTEREST].ToString().Equals(String.Empty))
                {
                    if (!AccruedInterest.IsChecked)
                        AccruedInterest.Click(MouseButtons.Left);
                    AccruedInterestEditor.Click(MouseButtons.Left);
                    ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_ACCRUED_INTEREST].ToString(), string.Empty);
                }

                if (!dr[TestDataConstants.COL_BROKER].ToString().Equals(String.Empty))
                {
                    if (!Broker.IsChecked)
                        Broker.Click(MouseButtons.Left);
                    ToggleBtnComboBox7.Click(MouseButtons.Left);
                    Dictionary<String, int> NameToIndex = CreateDictionary(ComboBox7);
                    //Wait(2000);
                    ComboBox7.AutomationElementWrapper.CachedChildren[NameToIndex[dr[TestDataConstants.COL_BROKER].ToString()]].CachedChildren[0].WpfClick();
                }

                if (!dr[TestDataConstants.COL_DESCRIPTION].ToString().Equals(String.Empty))
                {
                    if (!Description.IsChecked)
                        Description.Click(MouseButtons.Left);
                    DescriptionBox.Click(MouseButtons.Left);
                    ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_DESCRIPTION].ToString(), string.Empty);
                }

                if (!dr[TestDataConstants.COL_INTERNAL_COMMENTS].ToString().Equals(String.Empty))
                {
                    if (!InternalComments.IsChecked)
                        InternalComments.Click(MouseButtons.Left);
                    InternalCommentsBox.Click(MouseButtons.Left);
                    ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_INTERNAL_COMMENTS].ToString(), string.Empty);
                }

                if (!dr[TestDataConstants.COL_FXRATE].ToString().Equals(String.Empty))
                {
                    if (!FxRate.IsChecked)
                        FxRate.Click(MouseButtons.Left);
                    FxRateEditor.Click(MouseButtons.Left);
                    ExtentionMethods.UpdateCellValueConditions(dr[TestDataConstants.COL_FXRATE].ToString(), string.Empty);
                }
                if (!dr[TestDataConstants.COL_FXRATE_OPERATOR].ToString().Equals(String.Empty))
                {
                    if (!FxRateOperator.IsChecked)
                        FxRateOperator.Click(MouseButtons.Left);
                    ToggleBtnComboBox8.Click(MouseButtons.Left);
                    Dictionary<String, int> NameToIndex = CreateDictionary(ComboBox8);
                    //Wait(2000);
                    ComboBox8.AutomationElementWrapper.CachedChildren[NameToIndex[dr[TestDataConstants.COL_FXRATE_OPERATOR].ToString()]].CachedChildren[0].WpfClick();
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_AVG_PRICE_ROUNDING) && !dr[TestDataConstants.COL_AVG_PRICE_ROUNDING].ToString().Equals(String.Empty))
                {
                    if (!AvgPriceRounding.IsChecked)
                        AvgPriceRounding.Click(MouseButtons.Left);
                    AvgPriceRoundingEditor.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.CTRLA);
                    if (!dr[TestDataConstants.COL_AVG_PRICE_ROUNDING].ToString().Equals(ExcelStructureConstants.BLANK_CONST) && !string.IsNullOrWhiteSpace(dr[TestDataConstants.COL_AVG_PRICE_ROUNDING].ToString()))
                    {
                        Keyboard.SendKeys(dr[TestDataConstants.COL_AVG_PRICE_ROUNDING].ToString());                       
                    }
                }
               
                UIMap.Wait(500);
                UpdateAll.Click(MouseButtons.Left);

                Wait(1000);
                SavewDivideStatus.Click(MouseButtons.Left);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
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

