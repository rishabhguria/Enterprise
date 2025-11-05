using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    class CreateDoneAwayOrder : TradingTicketUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenManualTradingTicket();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputDetails(dr, BtnDoneAway);
                        //Wait(3000);
                        BtnDoneAway.Click(MouseButtons.Left);
                        if (UltraButtonOk.IsVisible)
                        {
                            UltraButtonOk.Click(MouseButtons.Left);
                            BtnPadlock.Click(MouseButtons.Left);
                            CmbBroker.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_BROKER].ToString();
                            //CmbBroker.Click(MouseButtons.Left);
                            //Keyboard.SendKeys(dr[TestDataConstants.COL_BROKER].ToString());
                            //dr[TestDataConstants.COL_BROKER].ToString();
                            BtnDoneAway.Click(MouseButtons.Left);
                        }
                                                
                        if (UltraButtonYes1.IsVisible)
                        {
                            UltraButtonYes1.Click(MouseButtons.Left);
                        }
                        if (PromptWindow_Fill_Panel.IsVisible)
                        {
                            if (dr[TestDataConstants.COL_PopUpQuantity].ToString() != String.Empty && NmrcTargetQuantity1.IsEnabled)
                            {
                                BtnEdit.Click(MouseButtons.Left);
                                BtnDoneAway.Click(MouseButtons.Left);
                                NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PopUpQuantity].ToString();
                                BtnPlace.Click(MouseButtons.Left);
                            }
                            else if (dr.Table.Columns.Contains(TestDataConstants.Col_Res_Allowed_List_PopUp))
                            {
                                if (dr[TestDataConstants.Col_Res_Allowed_List_PopUp].ToString().Equals("No"))
                                {
                                    BtnEdit.Click(MouseButtons.Left);
                                    if (dr.Table.Columns.Contains(TestDataConstants.COL_DONEAWAYCLICKONNO))
                                    {
                                        if (PromptWindow_Fill_Panel.IsVisible)
                                        {
                                            if (dr[TestDataConstants.COL_DONEAWAYCLICKONNO].ToString().Equals("No"))
                                            {                                                
                                                BtnDoneAway.Click(MouseButtons.Left);
                                                Wait(1000);
                                                BtnPlace.Click(MouseButtons.Left);
                                            }
                                            else
                                            {
                                                BtnPlace.Click(MouseButtons.Left);
                                            }
                                        }
                                    }
                            }
                            else
                            {
                                BtnPlace.Click(MouseButtons.Left);
                            }
                        }
                        
                        }
                        Wait(3000);
                        
                        if (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp_Qty) && (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp)))
                        {
                            if (PromptWindow_Fill_Panel.IsVisible)
                            {
                                if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString() != String.Empty && NmrcTargetQuantity1.IsEnabled)
                                {
                                    NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString();
                                    BtnPlace.Click(MouseButtons.Left);
                                }
                            }
                        }

                        if (TradingTicket1.IsValid)
                        {
                            if (TradingTicket2.IsVisible)
                            {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                            Wait(2000);
                            if (TradingTicket1.IsValid)
                            {
                                if (uiWindow1.IsVisible)
                                {
                                    if (uiWindow1.MsaaObject.CachedChildren.Count > 2)
                                    {
                                        string val = uiWindow1.MsaaObject.CachedChildren[2].Name.ToString();
                                        if (val.Contains("NAV Lock date"))
                                        { //do nothing 
                                        }
                                       //handled the popup "please enter a value for qty"
                                        else
                                        {
                                            ButtonOK2.Click(MouseButtons.Left);
                                        }
                                    }
                                    else
                                    {
                                        ButtonOK2.Click(MouseButtons.Left);
                                    }
                                }
                            }
                            if (TradingTicket1.IsValid)
                            {
                                if (TradingRulesViolatedPopUp.IsVisible)
                                {
                                    if (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp))
                                    {
                                        if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("No"))
                                        {
                                            UltraButtonNo.Click(MouseButtons.Left);
                                        }
                                        /*  else
                                          {
                                              UltraButtonYes.Click(MouseButtons.Left);
                                          }*/


                                        else if (dr.Table.Columns.Contains(TestDataConstants.COL_TRVTRADEQUANTITY))//
                                        {
                                            if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_TRVTRADEQUANTITY].ToString()))
                                            {
                                                List<string> TRVNav = new List<string>();
                                                List<string> TRVCurrentPosition = new List<string>();
                                                List<string> TRVAccounts = new List<string>(dr[TestDataConstants.COL_TRVACCOUNTNAME].ToString().Split(','));
                                                List<string> TRVTradeQuantity = new List<string>(dr[TestDataConstants.COL_TRVTRADEQUANTITY].ToString().Split(','));


                                                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_TRVNAV].ToString()))
                                                {
                                                    string[] values = dr[TestDataConstants.COL_TRVNAV].ToString().Split(',');
                                                    foreach (var val in values)
                                                    {
                                                        TRVNav.Add(val);
                                                    }
                                                }
                                                if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_TRVCURRENTPOSITION].ToString()))
                                                {
                                                    string[] values = dr[TestDataConstants.COL_TRVCURRENTPOSITION].ToString().Split(',');

                                                    foreach (var val in values)
                                                    {
                                                        TRVCurrentPosition.Add(val);
                                                    }
                                                }

                                                DataTable Uidata = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.UltraGridOverBuyOverSellRuleViolated.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                                                DataTable dt = GetexcelData(Uidata, TRVAccounts, TRVTradeQuantity, TRVNav.Count > 0 ? TRVNav : TRVCurrentPosition, dr[TestDataConstants.COL_SYMBOL].ToString());
                                                Uidata = DataUtilities.RemoveCommas(Uidata);
                                                //  verifyTRVPopUp()
                                                List<String> columns = new List<string>();
                                                columns.Add("Account Name");
                                                columns.Add("Trade Quantity");
                                                columns.Add("Current Position");

                                                List<String> errors = Recon.RunRecon(Uidata, dt, columns, 0.01);

                                                if (errors.Count > 1)
                                                {

                                                    throw new Exception(String.Format("Verification Failed at TRV :-The mismatches are:{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, errors)));
                                                }
                                                if (errors.Count < 1)
                                                {

                                                    Console.WriteLine("VERIFICATION OF TRADING RULES VOILATED PASSED SUCCESSFULLY!!");
                                                    if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("No"))
                                                    {
                                                        UltraButtonNo.Click(MouseButtons.Left);
                                                    }
                                                    else if (String.IsNullOrEmpty(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString()))
                                                        UltraButtonYes.Click(MouseButtons.Left);
                                                }
                                            }
                                            else
                                            {
                                                UltraButtonYes.Click(MouseButtons.Left);
                                            }
                                        }
                                        if (!dr.Table.Columns.Contains(TestDataConstants.COL_TRVTRADEQUANTITY) && (String.IsNullOrEmpty(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString()) || dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("Yes")))
                                        {
                                            UltraButtonYes.Click(MouseButtons.Left);
                                        }


                                    }
                                    else
                                    {
                                        UltraButtonYes.Click(MouseButtons.Left);
                                    }
                                }

                            }

                            if (TradingTicket1.IsValid)
                            {
                                if (popUpWindow != null && popUpWindow.IsVisible)
                                {
                                    string error = popUpWindow.MsaaObject.CachedChildren[2].ToString();
                                    ButtonOK1.Click(MouseButtons.Left);
                                    if (!error.Contains("Trading Tool"))
                                    {
                                        _result.ErrorMessage = error.ToString();
                                        //throw;
                                    }
                                }
                            }
                            //if (!string.IsNullOrWhiteSpace(LblErrorMessage.Text) && LblErrorMessage.Text.Contains("cannot pass the trade"))
                            //{
                            //    string errors = LblErrorMessage.Text;
                            //    _result.ErrorMessage = errors.ToString();
                            //}
                        }
                        if (PromptWindow_Fill_Panel.IsVisible)
                        {
                            if (dr[TestDataConstants.COL_PopUpQuantity].ToString() != String.Empty && NmrcTargetQuantity1.IsEnabled)
                            {
                                BtnEdit.Click(MouseButtons.Left);
                                BtnDoneAway.Click(MouseButtons.Left);
                                NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PopUpQuantity].ToString();
                                BtnPlace.Click(MouseButtons.Left);
                            }
                            else if (dr.Table.Columns.Contains(TestDataConstants.Col_Res_Allowed_List_PopUp))
                            {
                                if (dr[TestDataConstants.Col_Res_Allowed_List_PopUp].ToString().Equals("No"))
                                {
                                    BtnEdit.Click(MouseButtons.Left);
                                }
                                else
                                {
                                    BtnPlace.Click(MouseButtons.Left);
                                }
                            }
                            else
                            {
                                BtnPlace.Click(MouseButtons.Left);
                            }
                        }

                        if (dr.Table.Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                        {
                            if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString() != String.Empty)
                            {
                                CheckCompliance(dr);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateDoneAwayOrder");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {

                CloseTradingTicket();
            }

            return _result;
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

        public DataTable GetexcelData(DataTable Uidata, List<string> TRVAccounts, List<string> TRVTradeQuantity, List<string> ThirdColumn, string symbol)
        {
            DataTable DataTb = new DataTable();
            List<string> ColumnsList = new List<string>();
            foreach (DataColumn col in Uidata.Columns)
            {
                DataTb.Columns.Add(col.ColumnName);
                ColumnsList.Add(col.ColumnName);
            }
            for (int i = 0; i < TRVAccounts.Count; i++)
            {
                DataRow row = DataTb.NewRow();
                row[ColumnsList[0]] = symbol;
                row[ColumnsList[1]] = TRVAccounts[i];
                row[ColumnsList[2]] = TRVTradeQuantity[i];
                row[ColumnsList[3]] = ThirdColumn[i];
                DataTb.Rows.Add(row);
                DataTb.AcceptChanges();
            }

            return DataTb;

        }
    }
}