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
    class CommissionBulkChangeOnTaxlots : AllocationUIMap, ITestStep
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
                OpenCommissionBulkChangeOnTaxlots();
                InputChanges(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CommissionBulkChangeOnTaxlots");
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
        /// Opens the commission bulk change on groups.
        /// </summary>
        private void OpenCommissionBulkChangeOnTaxlots()
        {

            try
            {
                OpenAllocation();
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                Commisionbulkchange.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inputs the changes.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void InputChanges(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                TaxlotLevelSpeceficCommissionDivideFxRates1.Click(MouseButtons.Left);

                if (!dr[TestDataConstants.COL_MASTER_FUND].ToString().Equals(String.Empty))
                {
                    ToggleBtnComboBox5.Click(MouseButtons.Left);
                    Dictionary<String, int> NameToIndex = CreateDictionary(ComboBox5);
                    Wait(2000);
                    ComboBox5.AutomationElementWrapper.CachedChildren[NameToIndex[dr[TestDataConstants.COL_MASTER_FUND].ToString()]].CachedChildren[0].WpfClick();
                }

                if (!dr[TestDataConstants.COL_PB].ToString().Equals(String.Empty))
                {
                    ToggleBtnComboBox6.Click(MouseButtons.Left);
                    Dictionary<String, int> NameToIndex = CreateDictionary(ComboBox6);
                    Wait(2000);
                    ComboBox6.AutomationElementWrapper.CachedChildren[NameToIndex[dr[TestDataConstants.COL_PB].ToString()]].CachedChildren[0].WpfClick();
                }

                if (!dr[TestDataConstants.COL_ACCOUNTS].ToString().Equals(String.Empty))
                {
                    XamComboEditor2.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dr[TestDataConstants.COL_ACCOUNTS].ToString(), string.Empty, true);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_USE_SELECT_SPECIFY].ToString().Equals("Use"))
                {
                    UseDefaultRulesforallCommissionandFeeFields.Click(MouseButtons.Left);
                }
                else if (dr[TestDataConstants.COL_USE_SELECT_SPECIFY].ToString().Equals("Select"))
                {
                    SelectCommissionRule.Click(MouseButtons.Left);
                    if (!dr[TestDataConstants.COL_COMMISSION_RULE].ToString().Equals(String.Empty))
                    {
                        ToggleBtnComboBox9.Click(MouseButtons.Left);
                        Dictionary<String, int> NameToIndex = CreateDictionary(ComboBox9);
                        Wait(2000);
                        ComboBox9.AutomationElementWrapper.CachedChildren[NameToIndex[dr[TestDataConstants.COL_COMMISSION_RULE].ToString()]].CachedChildren[0].WpfClick();
                        
                    }
                }
                else if (dr[TestDataConstants.COL_USE_SELECT_SPECIFY].ToString().Equals("Specify"))
                {
                    SpecifyCommissionRule.Click(MouseButtons.Left);
                    DataTable test = testData.Tables[sheetIndexToName[0]];
                    int counter = 0;
                    int matchedindex = 0;
                    int comboindex = 0;
                    UIAutomationElement comboBox = new UIAutomationElement();
                    UIControlPart toggleBtn = new UIControlPart();
                    foreach (DataColumn col in test.Columns)
                    {
                       
                            if (counter < 4 || counter % 2 == 0)    //For cliking on check box except value
                            {
                                counter++;
                                continue;
                            }
                            else if (!dr[counter].ToString().Equals(String.Empty) && !dr[counter + 1].ToString().Equals(String.Empty))
                            {

                                String ColName = col.ColumnName;
                                if (ColName.Equals(TestDataConstants.COL_TRANSACTION_LEVY))
                                {
                                    ColName = TestDataConstants.CTRL_TRANSACTION_LEVY;
                                }
                                ClickCheckBox(ColName);
                                comboBox = CreateComboItem(comboindex);
                                toggleBtn = CreateToggleItem(comboBox);


                                toggleBtn.Click(MouseButtons.Left);
                                Dictionary<String, int> NameToIndex = CreateDictionary(comboBox);
                              //  Wait(2000);
                                comboBox.AutomationElementWrapper.CachedChildren[NameToIndex[dr[col.ColumnName].ToString()]].CachedChildren[0].WpfClick();
                                Wait(1000);
                                ClickTextBox(matchedindex);
                                Keyboard.SendKeys(dr[counter + 1].ToString());
                                matchedindex++;
                                comboindex++;
                                counter++;
                            }
                            else
                            {
                                comboindex++;
                                matchedindex++;
                                counter++;
                            }
                    }
                }
                UIMap.Wait(500);
                MouseController.ScrollWheelDown();
                Calculate.Click(MouseButtons.Left);
               
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
