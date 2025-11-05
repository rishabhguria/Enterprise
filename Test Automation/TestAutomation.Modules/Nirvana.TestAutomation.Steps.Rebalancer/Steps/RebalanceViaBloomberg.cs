using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.Steps.Rebalancer;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class RebalanceViaBloomberg : RebalancerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);
                RebalanceAcrossSecurities2.Click(MouseButtons.Left);
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                    Wait(2000);
                    AddSymbols(dr);
                    SelectTradingRules(dr);
                }
                //clcik Scroll bar of window               
                //for (int i = 1; i < 11; i++)
                //{ IncreaseBtn.Click(MouseButtons.Left);
                //}
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                IncreaseBtn.Click(MouseButtons.Right);
                for (int i = 1; i < 3; i++)
                {
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                }

                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);

                //Minimize Rebalancer
                // KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
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
                string DefaultSymbologySourceNewFile = ConfigurationManager.AppSettings["DefaultSymbologySourceNewFile"];
                if (File.Exists(DefaultSymbologySourceNewFile))
                {
                    RevertTTGenPref();
                }
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

                //Select Bloomberg symbol
                if (!dr[TestDataConstants.Text_BLOOMBERG].ToString().Equals(string.Empty))
                {
                    CboTickerSymbol.Click(MouseButtons.Left);
                    Keyboard.SendKeys(dr[TestDataConstants.Text_BLOOMBERG].ToString());

                    //Click Tab               
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Wait(2000);

                    // Code to handle Pop-up of Live Feed
                    if (NirvanaAlert2.IsVisible)
                    {
                        ButtonOK1.Click(MouseButtons.Left);

                        //Enter Price manuually if live feed not present

                        XamMaskedEditor1.Click(MouseButtons.Left);
                        Keyboard.SendKeys("10");
                    }
                }

                //Select +/-/= symbol
                XamComboEditor5.Click(MouseButtons.Left);

                if (!dr[TestDataConstants.COL_IncreaseDecreaseSet].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_IncreaseDecreaseSet].ToString();
                    if (value.Equals("Increase"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("Decrease"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    if (value.Equals("Set"))
                    {
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                }

                //Enter Target %
                Wait(1000);
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
                if (!dr[TestDataConstants.Text_PRICE].ToString().Equals(string.Empty))
                {
                    XamMaskedEditor1.ClearText();
                    Wait(1000);
                    Keyboard.SendKeys(dr[TestDataConstants.Text_PRICE].ToString());
                }

                // Enter Fx rate at run time

                Wait(1000);
                if (!dr[TestDataConstants.Text_Fx].ToString().Equals(string.Empty))
                {
                    Wait(1000);
                    XamMaskedEditor2.ClearText();
                    Keyboard.SendKeys(dr[TestDataConstants.Text_Fx].ToString());
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
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                    XamMaskedEditor1.Click(MouseButtons.Left);
                    Keyboard.SendKeys("10");
                    ADD.Click(MouseButtons.Left);
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
                    value = dr[TestDataConstants.COL_SellToRaiseCash].ToString();
                    if (value.Equals("Check"))
                    {
                        CheckBox5.Click(MouseButtons.Left);
                    }
                }
                //Select Allow Negative Cash trading rule
                if (!dr[TestDataConstants.COL_AllowNegCash].ToString().Equals(string.Empty))
                {
                    value = dr[TestDataConstants.COL_AllowNegCash].ToString();
                    if (value.Equals("Check"))
                    {
                        CheckBox7.Click(MouseButtons.Left);
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
    }
}
