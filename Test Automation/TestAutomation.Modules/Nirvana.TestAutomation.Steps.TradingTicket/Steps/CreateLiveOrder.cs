using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class CreateLiveOrder : TradingTicketUIMap, ITestStep
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
                        InputDetails(dr, BtnSend);
                        Wait(3000);
                        BtnSend.Click(MouseButtons.Left);
                        Wait(2000);
                        if (UltraButtonYes1.IsVisible)
                        {
                            UltraButtonYes1.Click(MouseButtons.Left);
                        }
                        if (dr.Table.Columns.Contains(TestDataConstants.COL_SENDCLICKONNO))
                        {
                            if (PromptWindow_Fill_Panel.IsVisible)
                            {
                                if (dr[TestDataConstants.COL_SENDCLICKONNO].ToString().Equals("No"))
                                {
                                    BtnEdit.Click(MouseButtons.Left);
                                    BtnSend.Click(MouseButtons.Left);
                                    Wait(1000);
                                    BtnPlace.Click(MouseButtons.Left);
                                }
                                else
                                {
                                    BtnPlace.Click(MouseButtons.Left);
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
                                    ButtonOK2.Click(MouseButtons.Left);
                                }
                            }
                            if (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp_Qty) && (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp)))
                            {
                                if (PromptWindow_Fill_Panel.IsVisible)
                                {
                                    if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString() != String.Empty && NmrcTargetQuantity1.IsEnabled)
                                    {
                                        NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.Col_Shares_Outstanding_PopUp_Qty].ToString();
                                        BtnPlace.Click(MouseButtons.Left);
                                    }
                                    else if(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString() == "No")
                                        BtnEdit.Click(MouseButtons.Left);
                                    else
                                        BtnPlace.Click(MouseButtons.Left);
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
                                    }
                                }
                                if (PromptWindow_Fill_Panel.IsVisible)
                                {
                                    if (dr[TestDataConstants.COL_EditOrder].ToString() != String.Empty && dr[TestDataConstants.COL_EditOrder].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        BtnEdit.Click(MouseButtons.Left);
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
                                Wait(3000);
                            }
                            if (dr.Table.Columns.Contains(TestDataConstants.COL_ORDER_PENDING_NEW_POPUP))
                            {
                                if (dr[TestDataConstants.COL_ORDER_PENDING_NEW_POPUP].ToString().Equals("Yes"))
                                {
                                    Wait(62000);
                                    if (TradingTicket1.IsValid)
                                    {
                                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
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
                            if (dr.Table.Columns.Contains(TestDataConstants.Col_PendingApprovalPopUp) && !String.IsNullOrWhiteSpace(dr[TestDataConstants.Col_PendingApprovalPopUp].ToString()))
                            {
                                if (NirvanaCompliance.IsVisible && dr[TestDataConstants.Col_PendingApprovalPopUp].ToString().Equals("Yes"))
                                {
                                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                }
                                else if (NirvanaCompliance.IsVisible && dr[TestDataConstants.Col_PendingApprovalPopUp].ToString().Equals("No"))
                                {
                                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                                }
                            }
                        }
                    }
                    if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_BBGSYMBOLOGYUSED))
                    {//condition that column contains vaklues
                        foreach (DataRow dr in testData.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_BBGSYMBOLOGYUSED].ToString()))
                            {
                                string bbgSymbol = dr[TestDataConstants.COL_SYMBOL].ToString();
                                string columnValueToFetch = TestDataConstants.COL_TICKERSYMBOL;
                                string query = "SELECT " + columnValueToFetch + " FROM T_SMSymbolLookUpTable WHERE BloombergSymbol = " +"'"+ bbgSymbol+"'";
                                string getTickerSymbol = SqlUtilities.ExecuteQueryWithResult(query, columnValueToFetch);

                                dr[TestDataConstants.COL_SYMBOL] = getTickerSymbol;
                            }
                        }

                    }

                    SaveData(testData);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateLiveOrder");
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

		// Save the multiple trades as it was overwriting the data previously
        public void SaveData(DataSet LiveData)
        {
            try
            {
                //Code Change : 07/19/2018 
                //Author : Sumit Chand
                //Comment : SimulatorData folder if doesn't exist must be created and LiveTrades.xml file should be copied
                LiveData.Tables[0].TableName = "CreateLiveOrder";
                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CreateNewSubLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null ||  xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null)
                        {
                            xml2.LoadXml(LiveData.GetXml());
                            foreach (XmlNode list in xml2.SelectSingleNode("NewDataSet").ChildNodes)
                            xml1.SelectSingleNode("NewDataSet").AppendChild(xml1.ImportNode(list, true));
                            xml1.Save(@"SimulatorData/LiveTrades.xml");
                        }
                        else
                        {
                            LiveData.WriteXml(@"SimulatorData/LiveTrades.xml");
                        }
                    }
                    catch { LiveData.WriteXml(@"SimulatorData/LiveTrades.xml"); }
                }
                else
                    LiveData.WriteXml(@"SimulatorData/LiveTrades.xml");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
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
