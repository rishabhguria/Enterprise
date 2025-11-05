using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using TestAutomationFX.UI;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using System.Data.SqlClient;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class RebalanceAcrossSecurities : RebalancerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();

                //Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);
                String RebalExpandAcrossSecuritiesCheck = AccountVisibilityCheck();
                Console.WriteLine(RebalExpandAcrossSecuritiesCheck);
                //  RebalanceAcrossSecurities1.AutomationElementWrapper.WpfClickLeftBound(MouseButtons.Left);
                if (RebalExpandAcrossSecuritiesCheck == "False")
                {
                    RebalanceAcrossSecurities2.Click(MouseButtons.Left);
                }

                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        AddSymbols(dr);
                        SelectTradingRules(dr);
                    }
                }
                //if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                //{
                //    DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];

                //}
                //clcik Scroll bar of window               
                //for (int i = 1; i < 11; i++)
                //{ IncreaseBtn.Click(MouseButtons.Left);
                //}
                /*Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                IncreaseBtn.Click(MouseButtons.Right);
                for (int i = 1; i < 3; i++)
                {
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                }

                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);*/

                //Restore Rebalance Across Securities
                Wait(2000);
                RebalanceAcrossSecurities2.Click(MouseButtons.Left);
                //Minimize Rebalancer
                // KeyboardUtilities.MinimizeWindow(ref RebalanceTab);

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
        /// Add Symbols in Rebalance Across Securities 
        /// </summary>
        /// <param name="dr"></param>
        private void AddSymbols(DataRow dr)
        {
            try
            {
                string value = string.Empty;

                //Select Ticker symbol
                if (!dr[TestDataConstants.Text_TICKER].ToString().Equals(string.Empty))
                {
                    CboTickerSymbol.Click(MouseButtons.Left);
                    Wait(2000);
                    DataUtilities.clearTextData();
                    Keyboard.SendKeys(dr[TestDataConstants.Text_TICKER].ToString());
                    Wait(1500); // KARAN SINGH 
                    //Click Tab               
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Wait(2000);

                    // Code to handle Pop-up of Live Feed
                    if (NirvanaAlert2.IsVisible)
                    {
                        NirvanaAlert2.BringToFront();
                        ButtonOK1.Click(MouseButtons.Left);
                        //CboTickerSymbol.Click(MouseButtons.Left);

                        Wait(2000);
                        //Enter Price manuually if live feed not present

                        /*XamMaskedEditor1.Click(MouseButtons.Left);
                        Keyboard.SendKeys("10");*/
                    }
                }

                //Select +/-/= symbol
                

                if (!dr[TestDataConstants.COL_IncreaseDecreaseSet].ToString().Equals(string.Empty))
                {
                    // Handling for Empty values of  increase, decrease and set as well as Capital values in the test cases
                    XamComboEditor5.Click(MouseButtons.Left);
                    Wait(3000);
                    value = dr[TestDataConstants.COL_IncreaseDecreaseSet].ToString();
                    if (value.Equals("Increase") || value.Equals("INCREASE"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("Decrease") || value.Equals("DECREASE"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("Set") || value.Equals("SET"))
                    {

                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }
                
                //Enter Target %
                Wait(2000);
                XamMaskedEditor.ClearText();
                Wait(1000);
                if (!dr[TestDataConstants.Text_TARGET].ToString().Equals(string.Empty))
                {
                    Keyboard.SendKeys(dr[TestDataConstants.Text_TARGET].ToString());
                }

                //Select BPS/%
                if (dr.Table.Columns.Contains(TestDataConstants.COL_BPS_SET))
                {
                    if (!dr[TestDataConstants.COL_BPS_SET].ToString().Equals(String.Empty))
                    {
                        XamComboEditor3.Click(MouseButtons.Left);
                        Wait(4000);
                        //Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        value = dr[TestDataConstants.COL_BPS_SET].ToString();
                        if (value.Equals("Percentage"))
                        {
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                        if (value.Equals("BPS"))
                        {

                            Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                            Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        }
                    }
                }

                // Enter / Edit Price at run time
                Wait(1000);
                if (dr.Table.Columns.Contains(TestDataConstants.Text_PRICE))
                {
                    if (!dr[TestDataConstants.Text_PRICE].ToString().Equals(string.Empty))
                    {
                        XamMaskedEditor1.ClearText();
                        Wait(1000);
                        Keyboard.SendKeys(dr[TestDataConstants.Text_PRICE].ToString());
                    }
                }

                // Enter Fx rate at run time

                Wait(1000);
                if (dr.Table.Columns.Contains(TestDataConstants.Text_Fx))
                {
                    if (!dr[TestDataConstants.Text_Fx].ToString().Equals(string.Empty))
                    {
                        Wait(1000);
                        XamMaskedEditor2.ClearText();
                        Keyboard.SendKeys(dr[TestDataConstants.Text_Fx].ToString());
                    }
                }

                //Select Account Or Group
                //XamComboEditor4.Click(MouseButtons.Left);
                //if (!dr[TestDataConstants.COL_AccountOrGroup].ToString().Equals(string.Empty))
                //{
                //    value = dr[TestDataConstants.COL_AccountOrGroup].ToString();
                //    if (value.Equals("Pro-Rata"))
                //    {
                //        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                //    }

                //    if (value.Equals("Allocation1"))
                //    {
                //        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                //        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                //    }

                //    if (value.Equals("Allocation3"))
                //    {
                //        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                //        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                //        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                //    }
                //}
                // Select Account Or Group
                if (!dr[TestDataConstants.COL_AccountOrGroup].ToString().Equals(String.Empty))
                {
                    value = dr[TestDataConstants.COL_AccountOrGroup].ToString();

                    XamComboEditor6.Click(MouseButtons.Left);
                    Wait(4000);

                    List<string> listCmbAccountGroups = new List<string>();
                    listCmbAccountGroups.Add("Pro-Rata");
                    listCmbAccountGroups.Add("Allocation1");
                    listCmbAccountGroups.Add("Allocation3");
                    listCmbAccountGroups.Add("Allocation2");
                    listCmbAccountGroups.Add("OFFSHORE");
                    listCmbAccountGroups.Add("Allocation4");
                    listCmbAccountGroups.Add("LP C/O");
                    listCmbAccountGroups.Add("rt");

                    int indexofAccountOrGroupItem = listCmbAccountGroups.IndexOf(value);

                    for (int i = 0; i < indexofAccountOrGroupItem; i++)
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);

                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                }



                //Click on Add button
                ADD.Click(MouseButtons.Left);
                string popupvalue = string.Empty;
                if (dr.Table.Columns.Contains("ContinuePopup") && !string.IsNullOrEmpty(dr["ContinuePopup"].ToString()))
                {
                    popupvalue = dr["ContinuePopup"].ToString().ToUpper();
                }
                if (string.IsNullOrEmpty(popupvalue) || !dr.Table.Columns.Contains("ContinuePopup"))
                {
                    if (NirvanaAlert1.IsVisible)
                    {
                        ButtonOK.Click(MouseButtons.Left);
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        // Select trading rule method 
        private void SelectTradingRules(DataRow dr)
        {
            try
            {
                string value = string.Empty;
                //Select ReInvest trading rule
                if (!dr[TestDataConstants.COL_ReinvestCheck].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_ReinvestCheck].ToString();
                    if (value.Equals("Check"))
                    {
                        CheckBox4.Click(MouseButtons.Left);
                    }
                }

                //Select Sell to Raise trading rule
                if (!dr[TestDataConstants.COL_SellToRaiseCash].ToString().Equals(string.Empty))
                {
                    CheckBox7.Click(MouseButtons.Left);
                    value = dr[TestDataConstants.COL_SellToRaiseCash].ToString();
                    if (value.Equals("Check"))
                    {
                       //CheckBox5.Click(MouseButtons.Left);
                       XamComboEditor8.Click(MouseButtons.Left);
                       Selltoraisecash4.Click(MouseButtons.Left);
                       XamComboEditor8.Click(MouseButtons.Left);
                    }
                }

                //Select Allow Negative Cash trading rule
                if (!dr[TestDataConstants.COL_AllowNegCash].ToString().Equals(string.Empty))
                {
                    if (!CheckBox7.IsChecked)
                    {
                        CheckBox7.Click(MouseButtons.Left);
                    }
                    value = dr[TestDataConstants.COL_AllowNegCash].ToString();
                    if (value.Equals("Check"))
                    {
                        //CheckBox7.Click(MouseButtons.Left);
                        XamComboEditor8.Click(MouseButtons.Left);
                        Allownegativecash7.Click(MouseButtons.Left);
                        XamComboEditor8.Click(MouseButtons.Left);
                    }
                }

                //Select Set Cash Target and Set Cash Target % for trading rule
                if (dr.Table.Columns.Contains(TestDataConstants.Col_SetCashTarget))
                {
                    if (!dr[TestDataConstants.Col_SetCashTarget].ToString().Equals(string.Empty))
                    {
                        CheckBox7.Click(MouseButtons.Left);
                        value = dr[TestDataConstants.Col_SetCashTarget].ToString();
                        if (value.Equals("Check"))
                        {
                            //CheckBox7.Click(MouseButtons.Left);
                            XamComboEditor8.Click(MouseButtons.Left);
                            Setcashtarget.Click(MouseButtons.Left);
                            XamComboEditor8.Click(MouseButtons.Left);

                            if (!dr[TestDataConstants.Col_SetCashTargetPercentage].ToString().Equals(string.Empty))
                            {
                                XamNumericEditorCashTargetPercentage.Click(MouseButtons.Left);
                                Wait(1000);
                                Keyboard.SendKeys(dr[TestDataConstants.Col_SetCashTargetPercentage].ToString());
                            }
                        }
                    }
                }

                //Select both Set cash target and Sell to raise cash together
                if (dr.Table.Columns.Contains(TestDataConstants.Col_SellTRaiseAndSetCTarget))
                {
                    if (!dr[TestDataConstants.Col_SellTRaiseAndSetCTarget].ToString().Equals(string.Empty))
                    {
                        CheckBox7.Click(MouseButtons.Left);
                        value = dr[TestDataConstants.Col_SellTRaiseAndSetCTarget].ToString();
                        if (value.Equals("Check"))
                        {
                            XamComboEditor8.Click(MouseButtons.Left);
                            Selltoraisecash4.Click(MouseButtons.Left);
                            Setcashtarget.Click(MouseButtons.Left);
                            XamComboEditor8.Click(MouseButtons.Left);

                            if (!dr[TestDataConstants.Col_SetCashTargetPercentage].ToString().Equals(string.Empty))
                            {
                                XamNumericEditorCashTargetPercentage.Click(MouseButtons.Left);
                                Wait(1000);
                                Keyboard.SendKeys(dr[TestDataConstants.Col_SetCashTargetPercentage].ToString());
                            }
                        }
                    }
                }
                //Select both Set cash target and Allow negative cash together
                if (dr.Table.Columns.Contains(TestDataConstants.Col_AllowNegCashAndSetCTarget))
                {
                    if (!dr[TestDataConstants.Col_AllowNegCashAndSetCTarget].ToString().Equals(string.Empty))
                    {
                        CheckBox7.Click(MouseButtons.Left);
                        value = dr[TestDataConstants.Col_AllowNegCashAndSetCTarget].ToString();
                        if (value.Equals("Check"))
                        {
                            XamComboEditor8.Click(MouseButtons.Left);
                            Allownegativecash7.Click(MouseButtons.Left);
                            Setcashtarget.Click(MouseButtons.Left);
                            XamComboEditor8.Click(MouseButtons.Left);
                            if (NirvanaAlert1.IsVisible)
                            {
                                ButtonOK.Click(MouseButtons.Left);
                            }

                            if (!dr[TestDataConstants.Col_SetCashTargetPercentage].ToString().Equals(string.Empty))
                            {
                                XamNumericEditorCashTargetPercentage.Click(MouseButtons.Left);
                                Wait(1000);
                                Keyboard.SendKeys(dr[TestDataConstants.Col_SetCashTargetPercentage].ToString());
                            }

                        }


                    }
                }

                if (dr.Table.Columns.Contains(TestDataConstants.Col_SetCashTargetPercentage))
                {
                    if ((dr[TestDataConstants.Col_SetCashTargetPercentage].ToString().Equals(string.Empty)) && (dr.Table.Columns.Contains(TestDataConstants.Col_DisableCT)) && dr[TestDataConstants.Col_DisableCT].ToString().ToUpper() == "TRUE")
                    {
                        if (!XamNumericEditorCashTargetPercentage.IsEnabled)
                        {
                            Console.WriteLine("Set Cash Target% box has found Disabled");
                        }
                        else
                        {
                            throw new Exception("Set Cash Target% box has found enabled");
                        }

                    }
                }
                


               //Select both Sell To Raise and Allow negative cash together
                if (dr.Table.Columns.Contains(TestDataConstants.Col_AllowNegCashAndSellToRaise))
                {
                    if (!dr[TestDataConstants.Col_AllowNegCashAndSellToRaise].ToString().Equals(string.Empty))
                    {
                        CheckBox7.Click(MouseButtons.Left);
                        value = dr[TestDataConstants.Col_AllowNegCashAndSellToRaise].ToString();
                        if (value.Equals("Check"))
                        {
                            XamComboEditor8.Click(MouseButtons.Left);
                            Allownegativecash7.Click(MouseButtons.Left);
                            Selltoraisecash4.Click(MouseButtons.Left);
                            XamComboEditor8.Click(MouseButtons.Left);
                            if (NirvanaAlert1.IsVisible)
                            {
                                ButtonOK.Click(MouseButtons.Left);
                            }

                            if (!dr[TestDataConstants.Col_SetCashTargetPercentage].ToString().Equals(string.Empty))
                            {
                                XamNumericEditorCashTargetPercentage.Click(MouseButtons.Left);
                                Wait(1000);
                                Keyboard.SendKeys(dr[TestDataConstants.Col_SetCashTargetPercentage].ToString());
                            }

                        }


                    }
                }
                
                //Select No Shortning trading rule
                if (!dr[TestDataConstants.COL_NoShortning].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_NoShortning].ToString();
                    if (value.Equals("Check"))
                    {
                        CheckBox6.Click(MouseButtons.Left);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }
        public static String AccountVisibilityCheck()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Select * from T_RebalPreferences where preferencekey='RebalExpandAcrossSecurities'", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //String Data = "";
                            reader.Read();
                            String Data = reader.GetValue(1).ToString();
                            Console.WriteLine(Data);
                            return Data;

                            //return Data;
                        }
                    }
                }
            }
            catch (Exception) { throw; }

        }
    }
}
