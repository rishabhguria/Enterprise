using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using TestAutomationFX.Core;
using TestAutomationFX.UI;


namespace Nirvana.TestAutomation.Steps.PTT
{
    class SetPTTPreference : PTTPreferencesUIMap  ,ITestStep 
    {
        /// <summary>
        /// Run the step.
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenPTTPreference();
                InputParametersPTTPreference(testData, sheetIndexToName);
                BtnSave.DoubleClick(MouseButtons.Left);           
                Wait(1000);
                ClosePreferencePTT();
                //Wait(2000);
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
        /// Set Input Parameters of PTT Preference Module
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name</param>
        protected void InputParametersPTTPreference(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    if (dataRow[TestDataConstants.COL_TYPE].ToString() != string.Empty) // Type  
                    {
                        CmbCalculationType.Click(MouseButtons.Left);              
                        CmbCalculationType.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_TYPE].ToString();
                    }
                    
                    if (dataRow[TestDataConstants.COL_ADD_SUBTRACT_SET].ToString() != string.Empty)// Add/set
                    {
                        CmbAddSet.Click(MouseButtons.Left);
                        CmbAddSet.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_ADD_SUBTRACT_SET].ToString();
                    }

                    if (dataRow[TestDataConstants.COL_COMBINED_ACCOUNT_TOTAL].ToString() != string.Empty)//Combined account total
                    {
                        CmbCombineAccountTotal.Click(MouseButtons.Left);
                        CmbCombineAccountTotal.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_COMBINED_ACCOUNT_TOTAL].ToString();
                    }

                    if (dataRow[TestDataConstants.COL_MASTERFUND_fUND].ToString() != string.Empty)// MasterFund/Account
                    {
                        CmbMasterFundOrAccountValue.Click(MouseButtons.Left);
                        CmbMasterFundOrAccountValue.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_MASTERFUND_fUND].ToString();
                    }

                    if (dataRow[TestDataConstants.COL_INCREASE_DECIMAL_DIGITS].ToString() != String.Empty)// Increase Decimal Digit
                    {
                        NumIncreaseDecimalPrecision.DoubleClick(MouseButtons.Left);
                        NumIncreaseDecimalPrecision.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_INCREASE_DECIMAL_DIGITS].ToString();
                    }
                    if (dataRow[TestDataConstants.COL_REMOVE_ACCOUNT_WITH_ZERO_NAV].ToString() != string.Empty)// checkbox Remove Account with zero NAV
                    {
                        bool isChecked = ChkRemoveAccountsWithZeroNAV.IsChecked;
                        if (isChecked == true)
                        {
                            ChkRemoveAccountsWithZeroNAV.Uncheck();
                        }
                        string item = dataRow[TestDataConstants.COL_REMOVE_ACCOUNT_WITH_ZERO_NAV].ToString();
                        if (item.Equals("Checked"))
                        {
                            ChkRemoveAccountsWithZeroNAV.Check();
                        }
                    }

                    if (dataRow[TestDataConstants.COL_ACCOUNTS].ToString() != string.Empty)// Multiple Selection Account
                    {
                        List<string> selectItems = new List<string>();
                        String totalAccounts = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ACCOUNTS].ToString();
                        List<string> accountFilterlist = totalAccounts.Split(',').ToList();
                        foreach (string acc in accountFilterlist)
                        {
                            totalAccounts = acc.Trim();
                            selectItems.Add(totalAccounts);
                        }
                        MultiSelectDropDownAccount.Click(MouseButtons.Left);
                        ExtentionMethods.SelectMultipleItemsFromCombo(selectItems,MultiSelectDropDownAccount);
                   }

                    if (dataRow[TestDataConstants.COL_SYMBOLOGY].ToString() != String.Empty)// Select Symbology
                    {
                        CmbSymbologyPref.Click(MouseButtons.Left);
                        CmbSymbologyPref.Properties[TestDataConstants.TEXT_PROPERTY] = dataRow[TestDataConstants.COL_SYMBOLOGY].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
