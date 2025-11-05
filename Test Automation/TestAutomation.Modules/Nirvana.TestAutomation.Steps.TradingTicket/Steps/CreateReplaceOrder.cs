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
    public class CreateReplaceOrder : TradingTicketUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputDetails(dr, BtnReplace);
                        BtnReplace.Click(MouseButtons.Left);

                        if (uiWindow1.IsVisible)
                        {
                            if (uiWindow1.MsaaObject.CachedChildren.Count > 2)
                            {
                                string val = uiWindow1.MsaaObject.CachedChildren[2].Name.ToString();
                                if (val.Contains("NAV Lock date"))
                                { //do nothing 
                                }
                                else 
                                {
                                    ButtonOK2.Click(MouseButtons.Left);
                                    CloseTradingTicket();
                                }
                            }
                            else
                            {
                                if (uiWindow1.IsVisible)
                                {
                                    ButtonOK2.Click(MouseButtons.Left);
                                    CloseTradingTicket();
                                }
                            }
                            
                        }
                        if (TradingTicket1.IsValid)
                        {
                            if (TradingTicket2.IsVisible)
                            {
                                ButtonYes1.Click(MouseButtons.Left);
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
                                    /*else
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
                                                else if (String.IsNullOrEmpty(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString()) || dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("Yes"))
                                                    UltraButtonYes.Click(MouseButtons.Left);
                                            }
                                        }
                                        else
                                        {
                                            UltraButtonYes.Click(MouseButtons.Left);
                                        }
                                    }
                                    if (!dr.Table.Columns.Contains(TestDataConstants.COL_TRVTRADEQUANTITY) && (String.IsNullOrEmpty(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString()) || dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("Yes") ))
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

                        if (dr.Table.Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                        {
                            if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString() != String.Empty)
                            {
                                CheckCompliance(dr);

                            }

                        }
                        if (PromptWindow_Fill_Panel.IsVisible)
                        {
                            BtnPlace.Click();
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateReplaceOrder");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
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