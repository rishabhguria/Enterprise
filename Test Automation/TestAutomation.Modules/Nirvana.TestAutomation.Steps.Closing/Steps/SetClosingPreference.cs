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
    class SetClosingPreference : ClosingPreferencesUIMap,ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingPreferences();
                if (testData != null)
                {
                    PreferencesMain_UltraFormManager_Dock_Area_Top.DoubleClick();
                    BtnRestoreDefault.Click(MouseButtons.Left);
                    InputDetailsInPreferences(testData.Tables[0].Rows[0]);
                    BtnSave.Click(MouseButtons.Left);
                }
                else
                {
                    _result.AddResult(false, "Input data is not provided");
                }
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
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        private void InputDetailsInPreferences(DataRow dataRow)
        {
            try
            {
                if (dataRow[TestDataConstants.COL_METHOD].ToString() != string.Empty)
                {
                    CmbMethodology.Click(MouseButtons.Left);
                    string temp = dataRow[TestDataConstants.COL_METHOD].ToString();
                    Keyboard.SendKeys(temp.ToString());
                    CmbMethodology.Click(MouseButtons.Left);
                }

                if (dataRow[TestDataConstants.COL_GLOBAL_ALGO].ToString() != string.Empty)
                {
                    CmbClosingAlgo.Click(MouseButtons.Left);
                    string temp = dataRow[TestDataConstants.COL_GLOBAL_ALGO].ToString();
                    Keyboard.SendKeys(temp.ToString());
                    CmbClosingAlgo.Click(MouseButtons.Left);
                }

                if (dataRow[TestDataConstants.COL_SECONDARY_SORT_CRITERIA].ToString() != string.Empty)
                {
                    CmbSecondarySort.Click(MouseButtons.Left);
                    string temp = dataRow[TestDataConstants.COL_SECONDARY_SORT_CRITERIA].ToString();
                    Keyboard.SendKeys(temp.ToString());
                    CmbSecondarySort.Click(MouseButtons.Left);
                }

                if (dataRow[TestDataConstants.COL_OVERRIDE_GLOBAL_ALGO].ToString() != string.Empty)
                {
                    if ((dataRow[TestDataConstants.COL_OVERRIDE_GLOBAL_ALGO].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!ChkBoxOverrideGlobal.IsChecked)) || (dataRow[TestDataConstants.COL_OVERRIDE_GLOBAL_ALGO].ToString().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase) && (ChkBoxOverrideGlobal.IsChecked)))
                    {
                        ChkBoxOverrideGlobal.Click(MouseButtons.Left);
                    }
                }

                if (dataRow[TestDataConstants.COL_CLOSE_SHORT_BUY_BTO].ToString() != string.Empty)
                {
                    if ((dataRow[TestDataConstants.COL_CLOSE_SHORT_BUY_BTO].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!ChkBoxIsShortWithBuyAndBTC.IsChecked)) || (dataRow[TestDataConstants.COL_CLOSE_SHORT_BUY_BTO].ToString().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase) && (ChkBoxIsShortWithBuyAndBTC.IsChecked)))
                    {
                        ChkBoxIsShortWithBuyAndBTC.Click(MouseButtons.Left);
                    }
                }

                if (dataRow[TestDataConstants.COL_CLOSE_SELL_BTC].ToString() != string.Empty)
                {
                    if ((dataRow[TestDataConstants.COL_CLOSE_SELL_BTC].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!ChkBoxIsSellWithBTC.IsChecked)) || (dataRow[TestDataConstants.COL_CLOSE_SELL_BTC].ToString().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase) && (ChkBoxIsSellWithBTC.IsChecked)))
                    {
                        ChkBoxIsSellWithBTC.Click(MouseButtons.Left);
                    }
                }

                if (dataRow[TestDataConstants.COL_AUTO_CLOSE_STRATEGY].ToString() != string.Empty)
                {
                    if ((dataRow[TestDataConstants.COL_AUTO_CLOSE_STRATEGY].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!ChkAutoCloseStrategy.IsChecked)) || (dataRow[TestDataConstants.COL_AUTO_CLOSE_STRATEGY].ToString().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase) && (ChkAutoCloseStrategy.IsChecked)))
                    {
                        ChkAutoCloseStrategy.Click(MouseButtons.Left);
                    }
                }

                if (dataRow[TestDataConstants.COL_EXERCISE_ASSIGN_CHECKSIDE].ToString() != string.Empty)
                {
                    if ((dataRow[TestDataConstants.COL_EXERCISE_ASSIGN_CHECKSIDE].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase) && (!CheckExerciseAssigment.IsChecked)) || (dataRow[TestDataConstants.COL_EXERCISE_ASSIGN_CHECKSIDE].ToString().Equals("FALSE", StringComparison.InvariantCultureIgnoreCase) && (CheckExerciseAssigment.IsChecked)))
                    {
                        CheckExerciseAssigment.Click(MouseButtons.Left);
                    }
                }

                if (dataRow[TestDataConstants.COL_LONG_TERM_TAXRATE].ToString() != string.Empty)
                {
                    TxtBoxLongTermTaxRate.Click(MouseButtons.Left);
                    TxtBoxLongTermTaxRate.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_LONG_TERM_TAXRATE].ToString();
                }

                if (dataRow[TestDataConstants.COL_SHORT_TERM_TAXRATE].ToString() != string.Empty)
                {
                    TxtBoxShortTermTaxRate.Click(MouseButtons.Left);
                    TxtBoxShortTermTaxRate.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_SHORT_TERM_TAXRATE].ToString();
                }

                if (dataRow[TestDataConstants.COL_QTY_ROUNDOFF_DIGITS].ToString() != string.Empty)
                {
                    TxtQtyRoundoffDigits.Click(MouseButtons.Left);
                    TxtQtyRoundoffDigits.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_QTY_ROUNDOFF_DIGITS].ToString();
                }

                if (dataRow[TestDataConstants.COL_PRICE_ROUNDOFF_DIGITS].ToString() != string.Empty)
                {
                    TxtPriceRoundoffDigits.Click(MouseButtons.Left);
                    TxtPriceRoundoffDigits.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_PRICE_ROUNDOFF_DIGITS].ToString();
                }

                if (dataRow[TestDataConstants.COL_CLOSING_FIELD1].ToString() != string.Empty)
                {
                    CmbClosingField.Click(MouseButtons.Left);
                    string temp = dataRow[TestDataConstants.COL_CLOSING_FIELD1].ToString();
                    Keyboard.SendKeys(temp.ToString());
                    CmbClosingField.Click(MouseButtons.Left);
                }
                if (dataRow.Table.Columns.Contains(TestDataConstants.COL_AUTO_OPTN_EX_VALUE))
                {
                    if (dataRow[TestDataConstants.COL_AUTO_OPTN_EX_VALUE].ToString() != string.Empty)
                    {
                        string temp = dataRow[TestDataConstants.COL_AUTO_OPTN_EX_VALUE].ToString();

                        ClearOldData(TxtAutoOptExerciseValue.Text.Length,TxtAutoOptExerciseValue);
                        TxtAutoOptExerciseValue.Click(MouseButtons.Left);
                        Keyboard.SendKeys(temp.ToString());
                       
                    }
                }


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
        private void ClearOldData(int size ,TestAutomationFX.UI.UIUltraTextEditor TxtAutoOptExerciseValue)
        {
            try
            {
                while (size != 0)
                {
                    TxtAutoOptExerciseValue.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    size--;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
