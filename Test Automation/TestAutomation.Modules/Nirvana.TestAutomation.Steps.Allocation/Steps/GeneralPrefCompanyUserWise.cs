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
using System.Data.SqlClient;
using System.Configuration;
using Nirvana.TestAutomation.Steps.Allocation.Scripts;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class GeneralPrefCompanyUserWise : PreferencesUIMap , ITestStep 
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
                OpenAllocation();
                SetPreferences(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "GeneralPrefCompanyUserWise");
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
        /// Opens the General Preferences.
        /// </summary>
        private void OpenGeneralPreferences()
        {
            try
            {
                Preferences.Click(MouseButtons.Left);
                GeneralPreferences.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the General Preferences.
        /// </summary>
        private void SetPreferences(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                //foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                //{
                    OpenGeneralPreferences();
                    InputPreferences(testData.Tables[sheetIndexToName[0]].Rows[0]);
                    Save.Click(MouseButtons.Left);
                    Close.Click(MouseButtons.Left);
                //}

                  
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InputPreferences(DataRow dtRow)
        {
            try
            {
                Dictionary<string, int> AssetNameToId = CreateAssetDictionary();
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString()))
                {
                    /*Previous Handling*/

                    //if (!ValidateChecksideWhileAllocation.IsChecked)
                    //{
                    //    ValidateChecksideWhileAllocation.Click(MouseButtons.Left);
                    //}

                    //if (ValidateChecksideWhileAllocation.IsChecked && !dtRow[TestDataConstants.COL_DISABLED_CHECKSIDE_ASSETS].ToString().Equals(String.Empty))
                    //{
                    //    String[] disabledAssets = dtRow[TestDataConstants.COL_DISABLED_CHECKSIDE_ASSETS].ToString().Split(',');
                    //    ToggleButton.Click(MouseButtons.Left);
                    //    foreach (String str in disabledAssets)
                    //    {
                    //        String temp = "[" + AssetNameToId[str] + ", " + str + "]";
                    //        SelectAssets(temp);
                    //    }
                    //    ToggleButton.Click(MouseButtons.Left);
                    //}


                    /*New Handling*/

                    string CheckSideValue;

                    CheckSideValue = dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString();
                    SQLQueriesConstants.setCheckSideValue(CheckSideValue);
                    SqlUtilities.ExecuteQuery(SQLQueriesConstants.updateCheckSideValueQuery);
                    // Wait(5000);
                }


                if (!dtRow[TestDataConstants.COL_PRECISION_VALUE].ToString().Equals(string.Empty))
                {
                    TxtboxPrecisionDigit.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_PRECISION_VALUE].ToString(), string.Empty, true);
                }
                //else 
                //{
                //    TxtboxPrecisionDigit.Click(MouseButtons.Left);
                //    ExtentionMethods.CheckCellValueConditions("0", string.Empty, true);
                //}

                if (!dtRow[TestDataConstants.COL_USE_COMMISSION_IN_NET_AMOUNT].ToString().Equals(String.Empty))
                {
                    String[] UseCommissionInNetAmount = dtRow[TestDataConstants.COL_USE_COMMISSION_IN_NET_AMOUNT].ToString().Split(',');
                    ToggleButton1.Click(MouseButtons.Left);
                    foreach (String str in UseCommissionInNetAmount)
                    {
                        String temp = "[" + AssetNameToId[str] + ", " + str + "]";
                        SelectAssets(temp);
                    }
                    ToggleButton1.Click(MouseButtons.Left);
                }
                if (dtRow.Table.Columns.Contains(TestDataConstants.COL_PRORATA_SCHEME_NAME))
                {
                    if (!dtRow[TestDataConstants.COL_PRORATA_SCHEME_NAME].ToString().Equals(String.Empty))
                    {
                        TextBoxPresenter.Click(MouseButtons.Left);
                        ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_PRORATA_SCHEME_NAME].ToString(), string.Empty, true);
                        Keyboard.SendKeys("[ENTER]");
                    }
                }
                if (dtRow.Table.Columns.Contains(TestDataConstants.COL_PRORATA_ALLOCATION_SCHEME_BASIS))
                {
                    if (!dtRow[TestDataConstants.COL_PRORATA_ALLOCATION_SCHEME_BASIS].ToString().Equals(String.Empty))
                    {

                        ProrataSchemeBasisDropButton();
                        Dictionary<String, int> Schemebasis = CreateDictionary(CmbboxProrataAllocationSchemeBasis);
                        //Wait(2000);
                        CmbboxProrataAllocationSchemeBasis.AutomationElementWrapper.CachedChildren[Schemebasis[dtRow[TestDataConstants.COL_PRORATA_ALLOCATION_SCHEME_BASIS].ToString()]].CachedChildren[0].WpfClick();

                    }

                }
                if (dtRow.Table.Columns.Contains(TestDataConstants.COL_SHOW_ADVANCED_PRORATA_UI))
                {
                    if (!dtRow[TestDataConstants.COL_SHOW_ADVANCED_PRORATA_UI].ToString().Equals(String.Empty))
                    {
                        if (String.Equals(dtRow[TestDataConstants.COL_SHOW_ADVANCED_PRORATA_UI].ToString(), "TRUE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!ShowadvancedProrataUI.IsChecked)
                            {
                                ShowadvancedProrataUI.Click(MouseButtons.Left);
                            }
                        }
                        else if (String.Equals(dtRow[TestDataConstants.COL_SHOW_ADVANCED_PRORATA_UI].ToString(), "FALSE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (ShowadvancedProrataUI.IsChecked)
                            {
                                ShowadvancedProrataUI.Click(MouseButtons.Left);
                            }
                        }
                    }

                }
                if (dtRow.Table.Columns.Contains(TestDataConstants.COL_CUSTOM))
                {
                    if (!dtRow[TestDataConstants.COL_CUSTOM].ToString().Equals(String.Empty))
                    {
                        if (String.Equals(dtRow[TestDataConstants.COL_CUSTOM].ToString(), "TRUE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!Custom.IsChecked)
                            {
                                Custom.Click(MouseButtons.Left);
                            }
                        }
                        else if (String.Equals(dtRow[TestDataConstants.COL_CUSTOM].ToString(), "FALSE", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Custom.IsChecked)
                            {
                                Custom.Click(MouseButtons.Left);
                            }
                        }
                    }

                }
                bool result = false;

                /*Previous Handling*/

                //Boolean.TryParse(dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString(),out result);
                //if (result && !ValidateChecksideWhileAllocation.IsChecked)
                //{
                //    ValidateChecksideWhileAllocation.Click(MouseButtons.Left);
                //}
                //else if (!result && ValidateChecksideWhileAllocation.IsChecked && dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString()!=string.Empty)
                //{
                //    ValidateChecksideWhileAllocation.Click(MouseButtons.Left);
                //}
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_ENABLE_MASTER_FUND].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_ENABLE_MASTER_FUND].ToString(), out result);
                    if (result && !EnableMasterFundRatioAllocation1.IsChecked)
                    {
                        EnableMasterFundRatioAllocation1.Click(MouseButtons.Left);
                        if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_1_MASTER_FUND_1_SYMBOL].ToString()))
                        {
                            Boolean.TryParse(dtRow[TestDataConstants.COL_1_MASTER_FUND_1_SYMBOL].ToString(), out result);
                            if (result && !uiAutomationElement8.IsChecked)
                            {
                                uiAutomationElement8.Click(MouseButtons.Left);
                            }
                            else
                                if (!result && uiAutomationElement8.IsChecked)
                                {
                                    uiAutomationElement8.Click(MouseButtons.Left);
                                }
                        }
                    }
                    else
                    {
                        if (!result && EnableMasterFundRatioAllocation1.IsChecked)
                        {
                            EnableMasterFundRatioAllocation1.Click(MouseButtons.Left);
                        }
                    }
                }
                Boolean.TryParse(dtRow[TestDataConstants.COL_SAVE_WITHOUT_STATE].ToString(), out result);

                if (result && !IncludeSaveWithoutState.IsChecked)
                {
                    IncludeSaveWithoutState.Click(MouseButtons.Left);
                    Wait(2000);
                }
                else if (!result && IncludeSaveWithoutState.IsChecked && dtRow[TestDataConstants.COL_SAVE_WITHOUT_STATE].ToString() != string.Empty)
                {
                    IncludeSaveWithoutState.Click(MouseButtons.Left);
                }

                Boolean.TryParse(dtRow[TestDataConstants.COL_SAVE_WITH_STATE].ToString(), out result);
                if (result && !IncludeSaveWithState.IsChecked)
                {
                    IncludeSaveWithState.Click(MouseButtons.Left);
                }
                else if (!result && IncludeSaveWithState.IsChecked && dtRow[TestDataConstants.COL_SAVE_WITH_STATE].ToString() != string.Empty)
                {
                    IncludeSaveWithState.Click(MouseButtons.Left);
                }

                Boolean.TryParse(dtRow[TestDataConstants.COL_CLEAR_QUANTITIES].ToString(), out result);
                if (result && !ClearQuantitiesafterallocation.IsChecked)
                {
                    ClearQuantitiesafterallocation.Click(MouseButtons.Left);
                }
                else if (result && ClearQuantitiesafterallocation.IsChecked && dtRow[TestDataConstants.COL_CLEAR_QUANTITIES].ToString() != string.Empty)
                {
                    ClearQuantitiesafterallocation.Click(MouseButtons.Left);
                }

                Boolean.TryParse(dtRow[TestDataConstants.COL_REAPPLY_ALLOCATION].ToString(), out result);
                if (result && !Reapplydefaultallocationafterchanges.IsChecked)
                {
                    Reapplydefaultallocationafterchanges.Click(MouseButtons.Left);
                }
                else if (!result && Reapplydefaultallocationafterchanges.IsChecked && dtRow[TestDataConstants.COL_REAPPLY_ALLOCATION].ToString() != string.Empty)
                {
                    Reapplydefaultallocationafterchanges.Click(MouseButtons.Left);
                }

                String defaultAllocation = dtRow[TestDataConstants.COL_DEFAULT_ALLOCATION].ToString();
                if (!defaultAllocation.Equals(String.Empty))
                {
                    if (defaultAllocation.Equals("Calculated"))
                    {
                        Calculated.Click(MouseButtons.Left);
                    }
                    else if (defaultAllocation.Equals("Fixed"))
                    {
                        Fixed.Click(MouseButtons.Left);
                    }

                }

                Boolean.TryParse(dtRow[TestDataConstants.COL_IS_CUSTOM_PREFERENCE].ToString(), out result);
                if (result && !Custom.IsChecked)
                {
                    Custom.Click(MouseButtons.Left);
                }
                else if (!result && Custom.IsChecked && dtRow[TestDataConstants.COL_IS_CUSTOM_PREFERENCE].ToString() != string.Empty)
                {
                    Custom.Click(MouseButtons.Left);
                }
            }
            catch (Exception) { throw; }
            }
        

        private Dictionary<string, int> CreateAssetDictionary()
        {
            Dictionary<string, int> AssetNameToId = new Dictionary<string, int>();
            AssetNameToId.Add("Equity", 1);
            AssetNameToId.Add("EquityOption", 2);
            AssetNameToId.Add("Future", 3);
            AssetNameToId.Add("FutureOption", 4);
            AssetNameToId.Add("FX", 5);
            AssetNameToId.Add("Cash", 6);
            AssetNameToId.Add("Indices", 7);
            AssetNameToId.Add("FixedIncome", 8);
            AssetNameToId.Add("PrivateEquity", 9);
            AssetNameToId.Add("FXOption", 10);
            AssetNameToId.Add("FXForward", 11);
            AssetNameToId.Add("Forex", 12);
            AssetNameToId.Add("ConvertibleBond", 13);
            AssetNameToId.Add("CreditDefaultSwap", 14);
            return AssetNameToId;
        }
        /// <summary>
        /// Selects assets.
        /// </summary>
        private void SelectAssets(String str)
        {
            UIAutomationElement ComboItem = new UIAutomationElement();
            ComboItem.AutomationName = str;
            ComboItem.ClassName = "ComboEditorItemControl";
            ComboItem.Comment = null;
            ComboItem.ItemType = "";
            ComboItem.MatchedIndex = 0;
            ComboItem.Name = "uiAutomationElement11";
            ComboItem.Parent = this.Popup1;
            ComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            ComboItem.UseCoordinatesOnClick = true;
            UIAutomationElement checkbox = new UIAutomationElement();
            checkbox.AutomationId = "SelectedCheckbox";
             checkbox.AutomationName = "";
             checkbox.ClassName = "CheckBox";
             checkbox.Comment = null;
             checkbox.ItemType = "";
             checkbox.MatchedIndex = 0;
             checkbox.Name = "checkbox";
             checkbox.Parent = ComboItem;
             checkbox.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Checkbox;
             checkbox.UseCoordinatesOnClick = false;
             if (!checkbox.IsChecked)
            {
                ComboItem.Click(MouseButtons.Left);
                
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
