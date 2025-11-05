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

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class DoneAwayUsingMTT : MultiTradingTicketUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (uiWindow1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
                //   Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                MultiTradingTicket.InvokeMethod("UnselectAllOrders", null);
                Wait(1000);
                DataTable dt = testData.Tables[0].Copy();
                if (dt.Columns.Contains("INFORMATIONRELATEDPOPUP_YESORNO"))
                    dt.Columns.Remove("INFORMATIONRELATEDPOPUP_YESORNO");

                if (dt != null)
                {
                    InputEnter(dt);
                }
                Wait(5000);
                if (MultiTradingTicket.IsValid)
                {
                    if (uiWindow1.IsAttached || uiWindow1.IsEnabled)
                    {
                        if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_INFORMATION))
                        {
                            if (testData.Tables[0].Rows[0][TestDataConstants.COL_INFORMATION].ToString().ToUpper().Equals("YES"))
                            {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                            else if (testData.Tables[0].Rows[0][TestDataConstants.COL_INFORMATION].ToString().ToUpper().Equals("NO"))
                            {
                                ButtonNo.Click(MouseButtons.Left);
                            }
                        }

                        else
                            ButtonYes1.Click(MouseButtons.Left);
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
            finally
            {
                
                    if (MultiTradingTicket.IsValid)
                    {

                        if (uiWindow1.IsVisible)
                            if (uiWindow1.MsaaObject.CachedChildren.Count > 2)
                            {
                                string val = uiWindow1.MsaaObject.CachedChildren[2].Name.ToString();
                                if (val.Contains("NAV Lock date"))
                                { //do nothing 
                                }
                            }
                            else
                            {
                                KeyboardUtilities.CloseWindow(ref MultiTradingTicket_UltraFormManager_Dock_Area_Top);
                            }
                    }
                
            }
            return _result;
        }

        private string InputEnter(DataTable dTable)
        {
            try
            {
                List<string> popUpValues = new List<string>();
                List<string> popUpQuantityValues = new List<string>();
                string errorMessage = string.Empty;
                int indexer = 0;
                foreach (DataRow dr in dTable.Rows)
                {

                    popUpValues.Add(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString());
                }
                if (dTable.Columns.Contains(TestDataConstants.COL_PopUpQuantity))
                {
                    foreach (DataRow dr in dTable.Rows)
                    {
                        popUpQuantityValues.Add(dr[TestDataConstants.COL_PopUpQuantity].ToString());
                    }
                }
                foreach (DataRow dr in dTable.Rows)
                {
                    var msaaObj = GrdTrades.MsaaObject;
                    DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.GrdTrades.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                    dTable.Columns.Remove(TestDataConstants.Col_Shares_Outstanding_PopUp);
                    if (dTable.Columns.Contains(TestDataConstants.COL_PopUpQuantity))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_PopUpQuantity);
                    }
                    DataRow[] dRows = DataUtilities.GetMatchingMultipleDataRows(DataUtilities.RemoveTrailingZeroes(dtBlotter), DataUtilities.RemoveTrailingZeroes(dTable), errorMessage);
                    // DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                    int index = 0;
                    foreach (var row in dRows)
                    {
                        index = dtBlotter.Rows.IndexOf(row);
                        GrdTrades.InvokeMethod("ScrollToRow", index);
                        Wait(1000);
                        msaaObj.FindDescendantByName("OrderBindingList", 5000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                        Wait(1000);
                    }

                    BtnDoneAway1.Click(MouseButtons.Left);
                    BtnDoneAway1.Click(MouseButtons.Left);
                    Wait(5000);
                    break;
                }

                // foreach (var l1 in popUpValues)
                //{
                /*if (MultiTradingTicket.IsValid)
                {
                    if (TradingRulesViolatedPopUp.IsVisible)
                    {
                        if (l1.Equals("No"))
                        {
                            UltraButtonNo.Click(MouseButtons.Left);
                        }

                        else
                        {
                            UltraButtonYes.Click(MouseButtons.Left);
                        }
                        Wait(5000);
                    }
                }*/
                if (popUpValues.Count > 0)
                {
                    handlepopUp(MultiTradingTicket, TradingRulesViolatedPopUp, popUpValues, popUpValues.Count, UltraButtonNo, UltraButtonYes, 0);
                }
                foreach (var l1 in popUpQuantityValues)
                {
                    bool isExpandable = false;
                    if (MultiTradingTicket.IsValid)
                    {
                        if (PromptWindow_Fill_Panel.IsVisible)
                        {
                            if (l1.ToString() != String.Empty && NmrcTargetQuantity.IsEnabled)
                            {
                                NmrcTargetQuantity.Click(MouseButtons.Left);
                                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                                Keyboard.SendKeys(l1.ToString());
                                NmrcTargetQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = l1.ToString();
                                BtnPlace.Click(MouseButtons.Left);
                            }
                            else
                            {
                                BtnPlace.Click(MouseButtons.Left);
                            }
                        }
                        if (TradingRulesViolatedPopUp.IsVisible)
                        {
                            isExpandable = UltraExpandableGroupBoxSharesOutstanding.Expanded;
                            if (popUpValues[indexer].Equals("No"))
                            {
                                UltraButtonNo.Click(MouseButtons.Left);
                                indexer++;
                            }

                            else
                            {
                                UltraButtonYes.Click(MouseButtons.Left);
                                indexer++;
                            }
                        }
                    }
                }
                if (MultiTradingTicket.IsValid)
                {
                    if (uiWindow1.IsVisible)
                        if (uiWindow1.MsaaObject.CachedChildren.Count > 2)
                        {
                            string val = uiWindow1.MsaaObject.CachedChildren[2].Name.ToString();
                            if (val.Contains("NAV Lock date"))
                            { //do nothing 
                            }
                        }
                        else
                        {
                            ButtonOK.Click(MouseButtons.Left);
                        }
                }



                return errorMessage;
            }
            catch (Exception) { throw; }
        }
        public void handlepopUp(TestAutomationFX.UI.UIWindow parent, TestAutomationFX.UI.UIWindow PopupWindow, List<string> PopupValues, int count, TestAutomationFX.UI.UIWindow buttonNo, TestAutomationFX.UI.UIWindow buttonYes, int index)
        {
            try
            {
                if (count > 0)
                {
                    if (parent.IsValid)
                    {
                        if (PopupWindow.IsVisible)
                        {
                            if (PopupValues[index].Equals("No"))
                            {
                                buttonNo.Click(MouseButtons.Left);
                                count--;
                            }

                            else
                            {
                                buttonYes.Click(MouseButtons.Left);
                                count--;
                            }
                            Wait(5000);
                            handlepopUp(parent, PopupWindow, PopupValues, count, buttonNo, buttonYes, index + 1);
                            return;
                        }
                        else
                            return;

                    }
                    else
                        return;

                }
            }
            catch (Exception) { throw; }
        }
    }
}

