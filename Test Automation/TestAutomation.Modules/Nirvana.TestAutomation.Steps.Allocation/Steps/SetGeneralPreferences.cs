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
using System.Text.RegularExpressions;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class SetGeneralPreferences : PreferencesUIMap, ITestStep
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
                foreach (DataRow dr in testData.Tables[0].Rows) {
                    for (int i = 0; i < testData.Tables[0].Columns.Count; i++) {
                        if (dr[i].ToString().Equals("ToggleState_On")) {
                            dr[i] = "True";
                        }

                        if (dr[i].ToString().Equals("ToggleState_Off"))
                        {
                            dr[i] = "False";
                        }

                        if (dr[i].ToString().Contains("["))
                        {
                            Regex regex = new Regex(@"\b[A-Za-z]+[0-9]*\b");
                            MatchCollection matches = regex.Matches(dr[i].ToString());
                            dr[i] = string.Empty;
                            foreach (Match match in matches)
                            {
                                if (string.IsNullOrEmpty(dr[i].ToString()))
                                {
                                    dr[i] = match;
                                }
                                else {
                                    dr[i] = dr[i] + "," + match;
                                }
                            }

                        }
                    }
                }
                string err_msg = SetPreferences(testData, sheetIndexToName);
                if (!string.IsNullOrEmpty(err_msg))
                {
                    _res.ErrorMessage = err_msg;
                    _res.IsPassed = false;
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetGeneralPreferences");
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
                MaximizePreferenceWindow();
                GeneralPreferences.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the General Preferences.
        /// </summary>
        private string SetPreferences(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string err_msg = string.Empty;
            try
            {
                OpenGeneralPreferences();
                err_msg = InputPreferences(testData.Tables[sheetIndexToName[0]].Rows[0]);
                Save.Click(MouseButtons.Left);
                Close.Click(MouseButtons.Left);
               

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }

        /// <summary>
        /// Inputs the preferences.
        /// </summary>
        /// <param name="dtRow">The dt row.</param>
        private string InputPreferences(DataRow dtRow)
        {
            string err_val="Input data is not correct for ";
            StringBuilder err_msg = new StringBuilder();
            try
            {
                bool result = false;
                err_msg.Append(CompanyWisePreferencesSet(dtRow, result));
                err_msg.Append(UserWisePreferencesSet(dtRow, result));
                err_msg.Append(DefaultRuleSet(dtRow, result));
                err_msg.Append(CommissionPreferencesSet(dtRow, result));
                if (!string.IsNullOrWhiteSpace(err_msg.ToString()))
                {
                    err_msg.Insert(0, err_val);
                    err_msg.Remove(err_msg.Length - 2, 2);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg.ToString();
        }

        /// <summary>
        /// Dictionary created for various drop down options
        /// </summary>
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
        /// Company Wise Preferences Set
        /// </summary>
        /// <param name="dtRow"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private string CompanyWisePreferencesSet(DataRow dtRow, bool result)
        {
            string err_msg = string.Empty;
            Dictionary<string, int> AssetNameToId = CreateAssetDictionary();
            try
            {
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString()))
                {

                    /*Previous Handling*/
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "ValidateCheckSide, ";

                    }
                    else
                    {
                        if (result && !ValidateChecksideWhileAllocation.IsChecked)
                        {
                            ValidateChecksideWhileAllocation.Click(MouseButtons.Left);
                            if (!dtRow[TestDataConstants.COL_DISABLED_CHECKSIDE_ASSETS].ToString().Equals(String.Empty))
                            {
                                String[] disabledAssets = dtRow[TestDataConstants.COL_DISABLED_CHECKSIDE_ASSETS].ToString().Split(',');
                                ToggleButton.Click(MouseButtons.Left);
                                foreach (String str in disabledAssets)
                                {
                                    Keyboard.SendKeys(str);
                                    Wait(1000);
                                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                   // Wait(1000);
                                }
                            }
                        }else if (result && ValidateChecksideWhileAllocation.IsChecked)
                        {
                            if (!dtRow[TestDataConstants.COL_DISABLED_CHECKSIDE_ASSETS].ToString().Equals(String.Empty))
                            {
                                String[] disabledAssets = dtRow[TestDataConstants.COL_DISABLED_CHECKSIDE_ASSETS].ToString().Split(',');
                                ToggleButton.Click(MouseButtons.Left);
                                foreach (String str in disabledAssets)
                                {
                                    Keyboard.SendKeys(str);
                                    Wait(1000);
                                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                    // Wait(1000);
                                }
                            }
                        }
                        else
                        {
                            if (!result && ValidateChecksideWhileAllocation.IsChecked)
                            {
                                ValidateChecksideWhileAllocation.Click(MouseButtons.Left);
                            }
                        }
                    }

                //    /*New Handling*/
                //    string checkSideValue;

                //    checkSideValue = dtRow[TestDataConstants.COL_VALIDATE_CHECKSIDE].ToString();
                //    string  updateCheckSideValueQuery = "UPDATE T_AL_AllocationDefaultRule SET T_AL_AllocationDefaultRule.CheckSidePreference='{\"DisableCheckSidePref\":{},\"DoCheckSideSystem\":" + checkSideValue.ToLower() + "}'";
                //   // SQLQueriesConstants.setCheckSideValue(checkSideValue);
                //   // Wait(5000);
               
                }
                if (!dtRow[TestDataConstants.COL_PRECISION_VALUE].ToString().Equals(string.Empty))
                {
                    TxtboxPrecisionDigit.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_PRECISION_VALUE].ToString(), string.Empty, true);
                }

                if (!dtRow[TestDataConstants.COL_USE_COMMISSION_IN_NET_AMOUNT].ToString().Equals(String.Empty))
                {
                    String[] UseCommissionInNetAmount = dtRow[TestDataConstants.COL_USE_COMMISSION_IN_NET_AMOUNT].ToString().Split(',');
                    ToggleButton1.Click(MouseButtons.Left);
                    foreach (String str in UseCommissionInNetAmount)
                    {
						Wait(1000);
                        String temp = "[" + AssetNameToId[str] + ", " + str + "]";
                        SelectAssets(temp);
                    }
                }
                //adding scheme name
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
                        Wait(1000);
                        Dictionary<String, int> Schemebasis = CreateDictionary(CmbboxProrataAllocationSchemeBasis);
                       // Wait(2000);
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
          
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_ENABLE_MASTER_FUND].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_ENABLE_MASTER_FUND].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "EnableMasterFund, ";
                      
                    }
                    else
                    {
                        if (result && !EnableMasterFundRatioAllocation1.IsChecked)
                        {
                            EnableMasterFundRatioAllocation1.Click(MouseButtons.Left);
                            if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_1_MASTER_FUND_1_SYMBOL].ToString()))
                            {
                                bool isParseSuccess1 = Boolean.TryParse(dtRow[TestDataConstants.COL_1_MASTER_FUND_1_SYMBOL].ToString(), out result);
                                if (!isParseSuccess1)
                                {
                                    err_msg += "1MasterFund/1Symbol, ";
                               
                                }
                                else
                                {
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
                        }
                        else
                        {
                            if (!result && EnableMasterFundRatioAllocation1.IsChecked)
                            {
                                EnableMasterFundRatioAllocation1.Click(MouseButtons.Left);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }

        /// <summary>
        /// User Wise Preferences Set
        /// </summary>
        /// <param name="dtRow"></param>
        /// <param name="result"></param>
        private string UserWisePreferencesSet(DataRow dtRow, bool result)
        {
            string err_msg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_SAVE_WITHOUT_STATE].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_SAVE_WITHOUT_STATE].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "SaveWithoutState, ";
                       
                    }
                    else
                    {
                        if (result && !IncludeSaveWithoutState.IsChecked)
                        {
                            IncludeSaveWithoutState.Click(MouseButtons.Left);
                        }
                        else if (!result && IncludeSaveWithoutState.IsChecked)
                        {
                            IncludeSaveWithoutState.Click(MouseButtons.Left);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_SAVE_WITH_STATE].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_SAVE_WITH_STATE].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "SaveWithState, ";
                     
                    }
                    else
                    {
                        if (result && !IncludeSaveWithState.IsChecked)
                        {
                            IncludeSaveWithState.Click(MouseButtons.Left);
                        }
                        else if (!result && IncludeSaveWithState.IsChecked)
                        {
                            IncludeSaveWithState.Click(MouseButtons.Left);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_CLEAR_QUANTITIES].ToString()))
                {
                    bool isParseSuccess= Boolean.TryParse(dtRow[TestDataConstants.COL_CLEAR_QUANTITIES].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "ClearQuantities, ";
                      
                    }
                    else
                    {
                        if (result && !ClearQuantitiesafterallocation.IsChecked)
                        {
                            ClearQuantitiesafterallocation.Click(MouseButtons.Left);
                        }
                        else if (!result && ClearQuantitiesafterallocation.IsChecked)
                        {
                            ClearQuantitiesafterallocation.Click(MouseButtons.Left);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_REAPPLY_ALLOCATION].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_REAPPLY_ALLOCATION].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "ReapplyAllocation, ";
                      
                    }
                    else
                    {
                        if (result && !Reapplydefaultallocationafterchanges.IsChecked)
                        {
                            Reapplydefaultallocationafterchanges.Click(MouseButtons.Left);
                        }
                        else if (!result && Reapplydefaultallocationafterchanges.IsChecked)
                        {
                            Reapplydefaultallocationafterchanges.Click(MouseButtons.Left);
                        }
                    }
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

                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_IS_CUSTOM_PREFERENCE].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_IS_CUSTOM_PREFERENCE].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "IsCustomPreference, ";
                     
                    }
                    else
                    {
                        if (result && !Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
                        }
                        else if (!result && Custom.IsChecked)
                        {
                            Custom.Click(MouseButtons.Left);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }

        /// <summary>
        /// Default Rule Set
        /// </summary>
        /// <param name="dtRow"></param>
        /// <param name="result"></param>
        private string DefaultRuleSet(DataRow dtRow, bool result)
        {
            string err_msg = string.Empty;
            try
            {
                String TargetPercentage = dtRow[TestDataConstants.COL_TARGET_PERCENTAGE_AS_OF].ToString();
                if (!TargetPercentage.Equals(String.Empty))
                {
                    ToggleBtnTargetPercentage.Click(MouseButtons.Left);
                    Wait(2000);
                    ClickOnComboBoxItem(TargetPercentage, CmbboxTargetPercentage);
                }

                String AllocationMethod = dtRow[TestDataConstants.COL_ALLOCATION_METHOD].ToString();
                if (!AllocationMethod.Equals(String.Empty) && !TargetPercentage.Equals(TestDataConstants.COL_PRORATA) && !TargetPercentage.Equals(TestDataConstants.COL_LEVELING))
                {
                    ToggleBtnCmbboxAllocationMethod.Click(MouseButtons.Left);
                    Wait(2000);
                    ClickOnComboBoxItem(AllocationMethod, CmbboxAllocationMethod);
                }

                String RemainderAllocation = dtRow[TestDataConstants.COL_REMAINDER_ALLOCATION_TO].ToString();
                ToggleBtnRemainderAllocation.Click(MouseButtons.Left);
                Wait(2000);
                if (RemainderAllocation.Equals(String.Empty))
                {
                    CmbboxRemainderAllocation.AutomationElementWrapper.CachedChildren[1].CachedChildren[0].WpfClick();
                }
                else
                {
                    ClickOnComboBoxItem(RemainderAllocation, CmbboxRemainderAllocation);
                }

                if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA) || TargetPercentage.Equals(TestDataConstants.COL_LEVELING))
                {
                    if (!dtRow[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString().Equals(String.Empty))
                    {
                        CmbboxAccountsForProrata.Click(MouseButtons.Left);
                        ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_PRORATA_ACCOUNTS].ToString(), string.Empty, true);
                    }
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (TargetPercentage.Equals(TestDataConstants.COL_PRORATA))
                {

                    if (dtRow[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString().Equals(String.Empty))
                    {
                        ExtentionMethods.CheckCellValueConditions("0", string.Empty, true);
                    }
                    else
                    {
                        ExtentionMethods.CheckCellValueConditions(dtRow[TestDataConstants.COL_DATE_UP_TO_DAYS].ToString(), string.Empty, true);
                    }
                }

                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_MATCH_CLOSING].ToString()))
                {
                    bool isParseSuccess= Boolean.TryParse(dtRow[TestDataConstants.COL_MATCH_CLOSING].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg = "MatchClosing, ";
                        return err_msg;
                    }
                    else
                    {
                        if (result && !ChkboxMatchClosingTransaction.IsChecked)
                        {
                            ChkboxMatchClosingTransaction.Click(MouseButtons.Left);
                        }
                        else if (!result && ChkboxMatchClosingTransaction.IsChecked)
                        {
                            ChkboxMatchClosingTransaction.Click(MouseButtons.Left);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
        }

        /// <summary>
        /// Commission Prefereneces Set
        /// </summary>
        /// <param name="dtRow"></param>
        /// <param name="result"></param>

        private string CommissionPreferencesSet(DataRow dtRow, bool result)
        {
            string err_msg = string.Empty;
            try
            {
               
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_COMMISSION_ALERT_ON_BROKER].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_COMMISSION_ALERT_ON_BROKER].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg = "CommissionAlertOnBrokerChange, ";
                      
                    }
                    else
                    {
                        if (result)
                        {
                            if (!Alertwhencommissionwouldberecalculatedonchangeofbroker.IsChecked)
                                Alertwhencommissionwouldberecalculatedonchangeofbroker.Click(MouseButtons.Left);
                        }
                        else
                        {
                            if (Alertwhencommissionwouldberecalculatedonchangeofbroker.IsChecked)
                                Alertwhencommissionwouldberecalculatedonchangeofbroker.Click(MouseButtons.Left);

                            String cmb_val = dtRow[TestDataConstants.COL_RECALCULATE_COMM_BROKER].ToString();
                            if (!cmb_val.Equals(String.Empty))
                            {
                                ToggleCmbboxBrokerChange.Click(MouseButtons.Left);
                                Wait(2000);
                                ClickOnComboBoxItem(cmb_val, CmbboxBrokerChangeRecalculate);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_COMMISSION_ALERT_ON_ALLOCATION].ToString()))
                {
                    bool isParseSuccess = Boolean.TryParse(dtRow[TestDataConstants.COL_COMMISSION_ALERT_ON_ALLOCATION].ToString(), out result);
                    if (!isParseSuccess)
                    {
                        err_msg += "CommissionAlertOnAllocation, ";
                      
                    }
                    else
                    {
                        if (result)
                        {
                            if (!AlertwhencommissionwouldberecalculatedonAllocation.IsChecked)
                                AlertwhencommissionwouldberecalculatedonAllocation.Click(MouseButtons.Left);
                        }
                        else
                        {
                            if (AlertwhencommissionwouldberecalculatedonAllocation.IsChecked)
                                AlertwhencommissionwouldberecalculatedonAllocation.Click(MouseButtons.Left);

                            String cmb_val = dtRow[TestDataConstants.COL_RECALCULATE_COMM_ALLOCATION].ToString();
                            if (!cmb_val.Equals(String.Empty))
                            {
                                ToggleCmbboxAllocation.Click(MouseButtons.Left);
                                Wait(2000);
                                ClickOnComboBoxItem(cmb_val, CmbboxAllocationRecalculate);
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return err_msg;
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