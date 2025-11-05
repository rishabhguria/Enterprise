using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class SendOrderUsingMTT : MultiTradingTicketUIMap, ITestStep 
    {
        
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                MultiTradingTicket.BringToFront();
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                //SelectAllTradeCheckBox.Click(MouseButtons.Left);
                MultiTradingTicket.InvokeMethod("UnselectAllOrders", null);
                Wait(1000);
                if (testData != null)
                {
                    InputEnter(testData); // changes to use testdata in the save data method
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
                    KeyboardUtilities.CloseWindow(ref MultiTradingTicket_UltraFormManager_Dock_Area_Top);
                }
            }
            return _result;
        }

        private string InputEnter(DataSet dataSet)
        {
            try
            {
                DataTable dTable = dataSet.Tables[0];
                DataTable dt = new DataTable();
                DataTable dtBlotter = new DataTable();
                dt = dTable.Copy();
                List<string> popUpValues = new List<string>();
                //List<string> popUpQuantityValues = new List<string>();
                List<string> popUpQuantityValues = new List<string>();
                string errorMessage = string.Empty;
                int indexer = 0;
                if (dTable.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp))
                {
                    foreach (DataRow dr in dTable.Rows)
                    {
                        popUpValues.Add(dr[TestDataConstants.Col_Shares_Outstanding_PopUp].ToString());
                    }
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
                    dtBlotter = CSVHelper.CSVAsDataTable(this.GrdTrades.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                    if (dTable.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp))
                    {
                        dTable.Columns.Remove(TestDataConstants.Col_Shares_Outstanding_PopUp);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_PopUpQuantity))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_PopUpQuantity);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_WARNING_RESPONSE))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_WARNING_RESPONSE);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_VERIFY_CLICKABILITY))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_VERIFY_CLICKABILITY);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_ALLOW_TRADE);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_PENDINGAPPROVALPOPUP))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_PENDINGAPPROVALPOPUP);
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
                    BtnSend1.Click(MouseButtons.Left);
                    Wait(1000);
                    break;
                }
                /* foreach (var l1 in popUpValues)
                 {
                     if (MultiTradingTicket.IsValid)
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
                         }
                     }                
                 }*/
                if (popUpValues.Count > 0)
                {
                    handlepopUp(TradingRulesViolatedPopUp1, popUpValues, popUpValues.Count, UltraButtonNo1, UltraButtonYes1, 0);
                }
                foreach (var l1 in popUpQuantityValues)
                {
                    if (MultiTradingTicket.IsValid)
                    {
                        bool isExpandable = false;

                        if (l1.ToString() != String.Empty && l1.Equals("YES", StringComparison.OrdinalIgnoreCase))
                        {
                            SendKeys.SendWait("{ENTER}");
                            // NmrcTargetQuantity.Click(MouseButtons.Left);
                            //KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                            // Keyboard.SendKeys(l1.ToString());
                            //  NmrcTargetQuantity.Properties[TestDataConstants.TEXT_PROPERTY] = l1.ToString();
                            //BtnPlace.Click(MouseButtons.Left);
                        }
                        else if (l1.ToString() != String.Empty && l1.Equals("No", StringComparison.OrdinalIgnoreCase))
                        {
                            SendKeys.SendWait("{TAB}");
                            SendKeys.SendWait("{ENTER}");
                            //BtnPlace.Click(MouseButtons.Left);
                        }

                        Wait(3000);

                        if (popUpValues[indexer].Equals("No", StringComparison.OrdinalIgnoreCase))
                        {
                            SendKeys.SendWait("{TAB}");
                            SendKeys.SendWait("{ENTER}");
                            indexer++;
                        }

                        else if (popUpValues[indexer].Equals("Yes", StringComparison.OrdinalIgnoreCase))
                        {
                            SendKeys.SendWait("{ENTER}");
                            indexer++;
                        }




                        /* if (l1.ToString() != String.Empty && NmrcTargetQuantity.IsEnabled)
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
                        
                         Wait(3000);
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
                         * */

                    }
                }

                if (MultiTradingTicket.IsValid)
                {
                    if (PromptWindow_Fill_Panel.IsVisible)
                    {
                        if (dt.Columns.Contains(TestDataConstants.COL_VERIFY_CLICKABILITY))
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[0][TestDataConstants.COL_VERIFY_CLICKABILITY].ToString()))
                                if (MultiTradingTicket_UltraFormManager_Dock_Area_Top.IsVisible)
                                {
                                    Close.Click(MouseButtons.Left);

                                }
                            Wait(5000);
                            if (PromptWindow_Fill_Panel.IsVisible && MultiTradingTicket_UltraFormManager_Dock_Area_Top.IsVisible)
                            {
                                if (BtnPlace.IsVisible)
                                {

                                    Console.WriteLine("Outside Warning popup MTT is not clickable");
                                }

                            }

                        }
                        if (BtnPlace.IsVisible)
                        {
                            Console.WriteLine("commit and send BUTTON is visible");
                        }
                        if (BtnEdit.IsVisible)
                        {
                            Console.WriteLine("Review BUTTON is visible");
                        }
                        if (dt.Columns.Contains(TestDataConstants.COL_WARNING_RESPONSE))
                        {

                            if (dt.Rows[0][TestDataConstants.COL_WARNING_RESPONSE].ToString() == "Commit & Send" || string.IsNullOrEmpty(dt.Rows[0][TestDataConstants.COL_WARNING_RESPONSE].ToString()))
                                BtnPlace.Click(MouseButtons.Left);
                            else if (dt.Rows[0][TestDataConstants.COL_WARNING_RESPONSE].ToString() == "Review")
                                BtnEdit.Click(MouseButtons.Left);

                        }
                        else
                            BtnPlace.Click(MouseButtons.Left);//commit and send
                    }
                    if (PromptWindow2.IsVisible)//new order prompt handling
                    {
                        if (BtnPlace2.IsEnabled)
                        {
                            BtnPlace2.Click(MouseButtons.Left);
                        }
                    }
                }
                if (dt.Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                {
                    if (dt.Rows[0][TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper() == "YES")
                    {
                        ResponseButton.Click(MouseButtons.Left);
                    }
                    else if (dt.Rows[0][TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper() == "NO")
                    {
                        CancelButton.Click(MouseButtons.Left);
                    }
                    else if (ResponseButton.IsEnabled)
                    {
                        ResponseButton.Click(MouseButtons.Left);
                    }
                }
                
                if (dt.Columns.Contains(TestDataConstants.COL_PENDINGAPPROVALPOPUP))
                {
                    if (dt.Rows[0][TestDataConstants.COL_PENDINGAPPROVALPOPUP].ToString().ToUpper() == "YES")
                    {
                        ButtonYes2.Click(MouseButtons.Left);
                    }
                    else if (dt.Rows[0][TestDataConstants.COL_PENDINGAPPROVALPOPUP].ToString().ToUpper() == "NO")
                    {
                        ButtonNo1.Click(MouseButtons.Left);
                    }
                    else if (ButtonNo1.IsEnabled)
                    {
                        ButtonNo1.Click(MouseButtons.Left);
                    }
                }

                // Wrong XMl Generate from the old Save data method
                SaveData(dTable,dtBlotter);
                return errorMessage;
            }
            catch (Exception) { throw; }
        }
        /// <summary>
        /// Save the grid Data to Simulator xml
        /// </summary>
        public void SaveData(DataTable dt, DataTable table)
        {
            try
            {
                //Code Change : 07/19/2018 
                //Author : Sumit Chand
                //Comment : SimulatorData folder if doesn't exist must be created and LiveTrades.xml file should be copied
                // Reverse the xml generate from send order using mtt step and generated from excel data rather than UI data
                DataTable newTable = dt.Copy();
                int totalRows = dt.Rows.Count;
                int index = 0;
                int targetIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dr = DataUtilities.GetMatchingDataRow(table, row);
                    index = table.Rows.IndexOf(dr);
                    if (totalRows > 1)
                    {
                        targetIndex = totalRows - index - 1;
                    } 
                    newTable.Rows[targetIndex].ItemArray = row.ItemArray;
                }
                DataSet LiveData = new DataSet();
                
                newTable.TableName = "SendOrderUsingMTT";
                LiveData.Tables.Add(newTable);
                if (!Directory.Exists("SimulatorData"))
                    Directory.CreateDirectory("SimulatorData");
                if (File.Exists(@"SimulatorData/LiveTrades.xml"))
                {
                    try
                    {
                        XmlDocument xml1 = new XmlDocument();
                        XmlDocument xml2 = new XmlDocument();
                        xml1.Load(@"SimulatorData/LiveTrades.xml");

                        if (xml1.SelectSingleNode("NewDataSet/CreateNewSubLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/CreateReplaceLiveOrder") != null || xml1.SelectSingleNode("NewDataSet/SendOrderUsingMTT") != null)
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
        public void handlepopUp( TestAutomationFX.UI.UIWindow PopupWindow, List<string> PopupValues, int count, TestAutomationFX.UI.UIWindow buttonNo, TestAutomationFX.UI.UIWindow buttonYes, int index)
        {
            try
            {
                if (count > 0)
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
                        handlepopUp(PopupWindow, PopupValues, count, buttonNo, buttonYes, index + 1);
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
