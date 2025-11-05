using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using System.Diagnostics;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    public class RebalancePreference : RebalancerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
               // Wait(4000);
                DataPreference.Click(MouseButtons.Left);
                if (UIPreference.IsVisible)
                {
                    if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                    {
                        DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                        if (dr[TestDataConstants.COL_ROUNDLOTPREFERENCE].ToString().Equals("True"))
                        {
                            Yes.Click(MouseButtons.Left);
                            Wait(200);
                        }
                        

                        /*else
                            No.Click(MouseButtons.Left);*/

                        if (dr[TestDataConstants.COL_REINVESTCASH].ToString().Equals("True"))
                        {
                            ReinvestCash2.Click(MouseButtons.Left);
                            Wait(200);
                        }
                        if (dr[TestDataConstants.COL_SELLTORAISECASH].ToString().Equals("True"))
                        {
                            Selltoraisecash2.Click(MouseButtons.Left);
                            Wait(200);
                        }
                        if (dr[TestDataConstants.COL_NOSHORTING].ToString().Equals("True"))
                        {
                            NoShorting2.Click(MouseButtons.Left);
                            Wait(200);
                        }
                        if (dr[TestDataConstants.COL_ALLOWNEGATIVECASH].ToString().Equals("True"))
                        {
                            AllowNegativeCash5.Click(MouseButtons.Left);
                            Wait(200);
                        }

                        if (testData.Tables[0].Columns.Contains("Rebalance Across Securities"))
                        {
                            if (dr[TestDataConstants.COL_EXPANDREBALANCEACROSSSECURITY].ToString().Equals("True"))
                            {
                                Alwaysexpandrebalanceacrosssecurities.Click(MouseButtons.Left);
                            }
                        }

                        if (testData.Tables[0].Columns.Contains("Other Items Impacting NAV"))
                        {
                            if (String.IsNullOrEmpty(dr[TestDataConstants.OTHERITEMSIMPACTINGNAV].ToString()).Equals(false))
                            {
                                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                                String[] ImpactingNAV = dr[TestDataConstants.OTHERITEMSIMPACTINGNAV].ToString().Split(',');
                                foreach (string NAV in ImpactingNAV)
                                {
                                    if (NAV.Equals("Cash"))
                                        Cash.Click(MouseButtons.Left);

                                    if (NAV.Equals("Accruals"))
                                        Accruals.Click(MouseButtons.Left);

                                    if (NAV.Equals("Other Assets Market Value"))
                                        OtherAssetsMarketValue.Click(MouseButtons.Left);

                                    if (NAV.Equals("Swap NAV Adjustments"))
                                        SwapNAVAdjustment.Click(MouseButtons.Left);

                                    if (NAV.Equals("Unrealized P&L of Swaps"))
                                        UnrealizedPLofSwaps.Click(MouseButtons.Left);
                                }
                                
                            }
                        }

                        if (testData.Tables[0].Columns.Contains("NAV Calculation Preference"))
                        {
                            if (String.IsNullOrEmpty(dr[TestDataConstants.NAVCALCULATIONPREFERENCE].ToString()).Equals(false))
                            {

                                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                                String[] CalculationPreference = dr[TestDataConstants.NAVCALCULATIONPREFERENCE].ToString().Split(',');
                                foreach (string NAV in CalculationPreference)
                                {
                                    if (CalculationPreference[0] == "Allocation1")
                                    {
                                        if (NAV.Equals("Cash"))
                                            PART_FocusSite.Click(MouseButtons.Left);

                                        if (NAV.Equals("Accruals"))
                                            PART_FocusSite1.Click(MouseButtons.Left);

                                        if (NAV.Equals("Other Assets Market Value"))
                                            PART_FocusSite2.Click(MouseButtons.Left);

                                        if (NAV.Equals("Swap NAV Adjustments"))
                                            PART_FocusSite3.Click(MouseButtons.Left);

                                        if (NAV.Equals("Unrealized P&L of Swaps"))
                                            PART_FocusSite4.Click(MouseButtons.Left);
                                    }
                                    if (CalculationPreference[0] == "Allocation2")
                                    {
                                        if (NAV.Equals("Cash"))
                                            PART_FocusSite5.Click(MouseButtons.Left);

                                        if (NAV.Equals("Accruals"))
                                            PART_FocusSite6.Click(MouseButtons.Left);

                                        if (NAV.Equals("Other Assets Market Value"))
                                            PART_FocusSite7.Click(MouseButtons.Left);

                                        if (NAV.Equals("Swap NAV Adjustments"))
                                            PART_FocusSite8.Click(MouseButtons.Left);

                                        if (NAV.Equals("Unrealized P&L of Swaps"))
                                            PART_FocusSite9.Click(MouseButtons.Left);

                                    }
                                    if (CalculationPreference[0] == "Allocation3")
                                    {
                                        if (NAV.Equals("Cash"))
                                            PART_FocusSite10.Click(MouseButtons.Left);

                                        if (NAV.Equals("Accruals"))
                                            PART_FocusSite11.Click(MouseButtons.Left);

                                        if (NAV.Equals("Other Assets Market Value"))
                                            PART_FocusSite20.Click(MouseButtons.Left);

                                        if (NAV.Equals("Swap NAV Adjustments"))
                                            PART_FocusSite21.Click(MouseButtons.Left);

                                        if (NAV.Equals("Unrealized P&L of Swaps"))
                                            PART_FocusSite22.Click(MouseButtons.Left);

                                    }
                                    if (CalculationPreference[0] == "Allocation4")
                                    {
                                        if (NAV.Equals("Cash"))

                                            PART_FocusSite15.Click(MouseButtons.Left);

                                        if (NAV.Equals("Accruals"))
                                            PART_FocusSite16.Click(MouseButtons.Left);

                                        if (NAV.Equals("Other Assets Market Value"))
                                            PART_FocusSite17.Click(MouseButtons.Left);

                                        if (NAV.Equals("Swap NAV Adjustments"))
                                            PART_FocusSite18.Click(MouseButtons.Left);

                                        if (NAV.Equals("Unrealized P&L of Swaps"))
                                            PART_FocusSite19.Click(MouseButtons.Left);

                                    }
                                }
                            }
                        }
                        if (testData.Tables[0].Columns.Contains("Account/Groups Visibility"))
                        {
                            if (String.IsNullOrEmpty(dr[TestDataConstants.ACCOUNTGROUPSVISIBILITY].ToString()).Equals(false))
                            {
                                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                                String[] Visibility = dr[TestDataConstants.ACCOUNTGROUPSVISIBILITY].ToString().Split(',');
                                foreach (string NAV in Visibility)
                                {
                                    if (NAV.Equals("Account"))
                                        CheckBox8.Click(MouseButtons.Left);

                                    if (NAV.Equals("Master Fund"))
                                        CheckBox9.Click(MouseButtons.Left);

                                    if (NAV.Equals("Custom Group"))
                                        CheckBox10.Click(MouseButtons.Left);
                                }
                               
                            }

                        }

                        if (testData.Tables[0].Columns.Contains("Always Ask for Saving Orders"))
                        {
                            if (String.IsNullOrEmpty(dr[TestDataConstants.ALWAYSASKFORSAVINGORDERS].ToString()).Equals(false))
                            {
                                if (dr[TestDataConstants.ALWAYSASKFORSAVINGORDERS].ToString().Equals("True"))
                                {
                                    Alwaysaskforsavingorders.Click(MouseButtons.Left);
                                    Wait(200);
                                }
                            }
                        }
                        if (testData.Tables[0].Columns.Contains("Rounding Type"))
                        {
                            if (String.IsNullOrEmpty(dr[TestDataConstants.ROUNDINGTYPE].ToString()).Equals(false))
                            {
                                KeyboardUtilities.MaximizeWindow(ref RebalanceTab);
                                while (TextBoxPresenter2.Text.Length > 0)
                                {
                                    TextBoxPresenter2.Click(MouseButtons.Left);
                                    DataUtilities.clearTextData();
                                }
                                Keyboard.SendKeys(dr[TestDataConstants.ROUNDINGTYPE].ToString());
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                        }
                        if (testData.Tables[0].Columns.Contains("/=/-/=:"))
                        {
                            if (String.IsNullOrEmpty(dr[TestDataConstants.COL_REBALANCEACROSSSECURITIESPROPERTIES].ToString()).Equals(false))
                            {
                                while (TextBoxPresenter10.Text.Length > 0)
                                {
                                    TextBoxPresenter10.Click(MouseButtons.Left);
                                    DataUtilities.clearTextData();
                                }
                                Keyboard.SendKeys(dr[TestDataConstants.COL_REBALANCEACROSSSECURITIESPROPERTIES].ToString());
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                        }
                        Wait(2000);
                        Save1.Click(MouseButtons.Left);
                        if (NirvanaAlert1.IsVisible)
                            ButtonOK.Click(MouseButtons.Left);

                        KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
                        
                    }
                }
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
    }
}
