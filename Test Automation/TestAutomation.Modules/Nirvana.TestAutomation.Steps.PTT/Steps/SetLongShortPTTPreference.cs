using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Windows.Forms;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Diagnostics;
using System.Data;


namespace Nirvana.TestAutomation.Steps.PTT
{
    class SetLongShortPTTPreference : PTTPreferencesUIMap, ITestStep
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
                
                foreach (DataRow dataRow in testData.Tables[0].Rows)
                {
                    bool isMessageVisible = false;
                    InputLongShortPTTPreference(dataRow); //testData, sheetIndexToName);
                    BtnSave.Click(MouseButtons.Left);
                    if (PercentTradingTool1.IsVisible)
                    {
                        isMessageVisible = true;
                        ButtonOK.Click(MouseButtons.Left);
                    }
                    if (dataRow.Table.Columns.Contains(TestDataConstants.COL_MESSAGEBOX))
                    {
                        if (dataRow[TestDataConstants.COL_MESSAGEBOX].ToString().Equals("YES", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isMessageVisible.Equals(true))
                            {
                                Console.WriteLine("Sum of accounts not equals to 100 messgae box is visible");
                            }
                            else
                            {
                                throw new Exception("Sum of accounts not equals to 100 messgae box is not visible");
                            }
                        }
                        else if (dataRow[TestDataConstants.COL_MESSAGEBOX].ToString().Equals("No", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isMessageVisible.Equals(true))
                            {
                                throw new Exception("Sum of accounts not equals to 100 messgae box is visible but it should not according to testvalue");
                            }
                        }

                    }
                    if (ApplicationError.IsVisible)
                    {
                         ButtonOK1.Click(MouseButtons.Left);
                    }
                    Wait(2000);
                }
                BtnClose.Click(MouseButtons.Left);
                //Wait(1000);
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
        /// This function returns index of the specified row of Master Fund.
        /// </summary>
        /// <param name="grid">Grid Name</param>
        /// <param name="fundName">Master Fund Name</param>
        /// <returns>index</returns>
        protected int FindIndex(TestAutomationFX.UI.UIUltraGrid grid, string fundName)
        {
            try
            {
                int i;
                DataTable dtAccountFactor1 = CSVHelper.CSVAsDataTable(grid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow foundRow = null;

                for (i = 0; i < dtAccountFactor1.Rows.Count; i++)   //Use to know index of the particular row
                {
                    if (dtAccountFactor1.Rows[i][0].Equals(fundName))
                    {
                        foundRow = dtAccountFactor1.Rows[i];
                        break;
                    }
                }
                return i;
            }
            catch (Exception) { throw; }
        }


        /// <summary>
        /// For setting Long Short Preferences
        /// </summary>
        /// <param name="dataRow">Data Row.</param>
        protected void InputLongShortPTTPreference(DataRow dataRow)
        {
            try
            {
                //If no value is provided in UseLongShortPreference Column,then it will take default value in PTTPreference
                if (dataRow[TestDataConstants.COL_USE_SHORTLONG_PREFERENCE].ToString() != string.Empty)
                {
                    bool isChecked = ChkboxShortLongPref.IsChecked;
                    if (dataRow.Table.Columns.Contains(TestDataConstants.COL_PREFCHECK))
                    {
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_PREFCHECK].ToString()))
                        {
                            if (dataRow[TestDataConstants.COL_PREFCHECK].ToString().ToLower().Equals(isChecked.ToString().ToLower()))
                            {
                                Console.WriteLine("Use Long Short Pref is Correct");
                            }
                            else
                                throw new Exception("LongShortPreference is not correct");
                        }
                    }
                    if (isChecked == true)
                    {
                        ChkboxShortLongPref.Uncheck();
                    }
                    string item = dataRow[TestDataConstants.COL_USE_SHORTLONG_PREFERENCE].ToString();
                    if (item.Equals("Checked"))
                    {
                        ChkboxShortLongPref.Check();
                    }
                }

                //COL_ACCOUNTS and COL_ACCOUNT_PERCENTAGE is used if Use shortLongPreference is checked OR if unchecked then it is used for Long account and % 
                //COL_ACCOUNT_SHORT and COL_ACCOUNT_SHORT_PERCENTAGE is used for Account and % For Short Preference respectively
                //MasterFundAccountGrid2 - UnCheck UseLongShortPreference
                //MasterFundAccountGid - Long (If Checked)
                //MasterFundAccountGid1 - Short (If Checked)
                if (dataRow[TestDataConstants.COL_MASTER_FUND].ToString() != string.Empty)
                {
                    string fundName = dataRow[TestDataConstants.COL_MASTER_FUND].ToString();
                    int i, index;
                    Dictionary<string, string> dictionary = new Dictionary<string, string>(); //FOR HOLDING ACCOUNTS AND ITS PERCENTAGE
                    bool flagcheck = false;
                    //var msaaObj;// = MasterFundAccountGrid2.MsaaObject;
                    bool isChecked = ChkboxShortLongPref.IsChecked;

                    if (isChecked == true)
                    {
                        flagcheck = true;
                        Long.Click(MouseButtons.Left);
                         var msaaObj = MasterFundAccountGrid3.MsaaObject;
                        index = FindIndex(this.MasterFundAccountGrid3, fundName);
                    }
                    else
                    {
                        var msaaObj = MasterFundAccountGrid5.MsaaObject;
                        index = FindIndex(this.MasterFundAccountGrid5, fundName);
                    }
                    Wait(2000);


                    if (dataRow[TestDataConstants.COL_CHECK_USE_PRORATA_PERCERNTAGE].ToString() != string.Empty) //Column Prorata Percentage
                    {
                        var msaaObj = (flagcheck == true)? MasterFundAccountGrid3.MsaaObject : MasterFundAccountGrid5.MsaaObject;
                        string item = dataRow[TestDataConstants.COL_CHECK_USE_PRORATA_PERCERNTAGE].ToString();
                        bool isChecked1 = msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Use Prorata Preference", 3000).IsChecked;
                        if (isChecked1 == true)
                        {
                            msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Use Prorata Preference", 3000).Click(MouseButtons.Left);
                        }
                        Wait(1000);
                        if (item.Equals("Checked"))
                        {
                            msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Use Prorata Preference", 3000).Click(MouseButtons.Left);
                        }
                    }

                    //Column Account and Percentage
                    if (dataRow[TestDataConstants.COL_ACCOUNTS].ToString() != String.Empty && dataRow[TestDataConstants.COL_ACCOUNT_PERCENTAGE].ToString() != String.Empty)
                    {
                        var msaaObj = (flagcheck == true) ? MasterFundAccountGrid3.MsaaObject : MasterFundAccountGrid5.MsaaObject;
                        msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].DoDefaultAction();
                        Wait(1000);
                        msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Account", 3000).Click(MouseButtons.Left);
                        Wait(2000);
                        msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Master Fund", 3000).Click(MouseButtons.Left);

                        String totalAccounts = dataRow[TestDataConstants.COL_ACCOUNTS].ToString();
                        List<string> accountFilterlist = totalAccounts.Split(',').ToList();
                        String totalPercentage = dataRow[TestDataConstants.COL_ACCOUNT_PERCENTAGE].ToString();
                        List<string> percentageFilterlist = totalPercentage.Split(',').ToList();
                        for (i = 0; i < accountFilterlist.Count; i++)
                        {
                            dictionary.Add(accountFilterlist[i], percentageFilterlist[i]);
                        }

                        Keyboard.SendKeys(KeyboardConstants.TABKEY + KeyboardConstants.TABKEY + KeyboardConstants.TABKEY + KeyboardConstants.TABKEY);
                        for (i = 0; i < percentageFilterlist.Count; i++)
                        {
                            Keyboard.SendKeys(percentageFilterlist[i] + KeyboardConstants.TABKEY + KeyboardConstants.TABKEY);
                        }
                        msaaObj.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].DoDefaultAction();
                        Wait(2000);
                    }
                    if (isChecked == true)
                    {
                        Short.Click(MouseButtons.Left);
                        var msaaObj2 = MasterFundAccountGrid4.MsaaObject;

                        index = FindIndex(this.MasterFundAccountGrid4, fundName);

                        if (dataRow[TestDataConstants.COL_SHORT_CHECK_USE_PRORATA_PERCERNTAGE].ToString() != string.Empty)  //Column Prorata Percentage for Short Preference
                        {
                            string item = dataRow[TestDataConstants.COL_SHORT_CHECK_USE_PRORATA_PERCERNTAGE].ToString();
                            bool isChecked1 = msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Use Prorata Preference", 3000).IsChecked;
                            if (isChecked1 == true)
                            {
                                msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Use Prorata Preference", 3000).Click(MouseButtons.Left);
                            }
                            Wait(1000);
                            if (item.Equals("Checked"))
                            {
                                msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Use Prorata Preference", 3000).Click(MouseButtons.Left);
                            }
                        }

                        //Column Account and Percentage for Short Preference
                        if (dataRow[TestDataConstants.COL_ACCOUNT_SHORT].ToString() != String.Empty && dataRow[TestDataConstants.COL_ACCOUNT_SHORT_PERCENTAGE].ToString() != String.Empty)
                        {
                            dictionary.Clear();
                            String totalAccounts2 = dataRow[TestDataConstants.COL_ACCOUNT_SHORT].ToString();
                            List<String> accountFilterlist2 = totalAccounts2.Split(',').ToList();
                            String totalPercentage2 = dataRow[TestDataConstants.COL_ACCOUNT_SHORT_PERCENTAGE].ToString();
                            List<String> percentageFilterlist2 = totalPercentage2.Split(',').ToList();
                            for (i = 0; i < accountFilterlist2.Count; i++)
                            {
                                dictionary.Add(accountFilterlist2[i], percentageFilterlist2[i]);
                            }

                            msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].DoDefaultAction();
                            Wait(1000);
                            msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Account", 3000).Click(MouseButtons.Left);
                            Wait(1000);
                            msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].FindDescendantByName("Master Fund", 3000).Click(MouseButtons.Left);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY + KeyboardConstants.TABKEY + KeyboardConstants.TABKEY + KeyboardConstants.TABKEY);
                            for (i = 0; i < percentageFilterlist2.Count; i++)
                            {
                                Keyboard.SendKeys(percentageFilterlist2[i] + KeyboardConstants.TABKEY + KeyboardConstants.TABKEY);
                            }

                            msaaObj2.FindDescendantByName("BindingList`1", 3000).CachedChildren[index + 1].DoDefaultAction();
                            Wait(2000);
                        }
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
