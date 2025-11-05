using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Data;
using Nirvana.TestAutomation.BussinessObjects;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Diagnostics;
using Nirvana.TestAutomation.UIAutomation;
using System.Text.RegularExpressions;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class BulkEditUpdateFromMTT : MultiTradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref  MultiTradingTicket_UltraFormManager_Dock_Area_Top);

                Wait(2000);
               
                // EditTrades(testData, sheetIndexToName, GrdTrades); 
                if (testData != null)
                {
                    DataRow dr = testData.Tables[0].Rows[0];// first row will always be for bulk update of trades
                    if(testData.Tables[0].Columns.Contains(TestDataConstants.COL_VERIFYSUGGESTIONS ))
                    {
                        bool doVerifySuggestions = !string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_VERIFYSUGGESTIONS].ToString());
                        BulkChange(dr, BtnCommit, doVerifySuggestions);
                    }
                    else
                    {
                        BulkChange(dr, BtnCommit);
                    }

                    if (dr[TestDataConstants.COL_BULKALERT].ToString().ToUpper().Equals("YES") ||uiWindow1.IsEnabled || uiWindow1.IsVisible)
                    {
                        if (ButtonOK.IsEnabled)
                            {
                                ButtonOK.Click(MouseButtons.Left);
                            }
                         else
                            Keyboard.SendKeys(KeyboardConstants.SPACE);
                    }

                  /*  if (uiWindow1.IsEnabled || uiWindow1.IsVisible)
                    {
                       
                    }*/
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_Btn].ToString()))
                    {
                        if (dr[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("REPLACE"))
                        {
                            BtnReplace.Click(MouseButtons.Left);
                            if (ButtonYes1.IsVisible)
                            {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                        }
                        if (dr[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("CANCEL"))
                        {
                            BtnCancel.Click(MouseButtons.Left);
                        }
                        if (dr[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("DONEAWAY"))
                        {
                            BtnDoneAway1.Click(MouseButtons.Left);
                            BtnDoneAway1.Click(MouseButtons.Left);
                            Wait(5000);
                        }
                        if (dr[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("SEND"))
                        {
                            BtnSend1.Click(MouseButtons.Left);
                            Wait(1000);
                        }

                    }


                    if (testData.Tables[0].Rows.Count > 1)
                    {
                        SelectAndEditTradesWithQuantity(testData, sheetIndexToName, GrdTrades);
                        MultiTradingTicket.InvokeMethod("UnselectAllOrders", null);
                        MultiTradingTicket.InvokeMethod("SelectAllOrders", null);
                        //IDictionary<int, string> Dict = CheckTradesStatus();
                       // DataRow[] EditTradeButton = testData.Tables[0].Select().ToList();
                        
                        DataRow LastRow = testData.Tables[0].Rows[testData.Tables[0].Rows.Count - 1];
                        if (LastRow.Table.Columns.Contains("Click on Clear"))
                        {
                            if (!String.IsNullOrEmpty(LastRow[TestDataConstants.COL_CLRBtn].ToString()))
                            {
                                BtnClear.Click(MouseButtons.Left);
                            }
                        }
                        if (!String.IsNullOrEmpty(LastRow[TestDataConstants.COL_Btn].ToString()))
                        {
                            if (LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("REPLACE"))
                            {
                                BtnReplace.Click(MouseButtons.Left);
                                if (ButtonYes1.IsVisible)
                                {
                                    ButtonYes1.Click(MouseButtons.Left);
                                }
                            }
                            if (LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("CANCEL"))
                            {
                                BtnCancel.Click(MouseButtons.Left);
                            }
                            if (LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("DONEAWAY"))
                            {
                                BtnDoneAway1.Click(MouseButtons.Left);
                                BtnDoneAway1.Click(MouseButtons.Left);
                                Wait(5000);
                            }
                            if (LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("SEND"))
                            {
                                BtnSend1.Click(MouseButtons.Left);
                                Wait(1000);
                            }

                        }
                        if (!String.IsNullOrEmpty(LastRow[TestDataConstants.COL_BULKALERT].ToString()) && LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("YES"))
                        {
                            Keyboard.SendKeys(KeyboardConstants.SPACE);
                        }
                        if (!String.IsNullOrEmpty(LastRow[TestDataConstants.COL_CANCELWITHOUTREPLACE].ToString()))
                        {
                            if (LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("YES"))
                                Keyboard.SendKeys(KeyboardConstants.SPACE);

                            else if (LastRow[TestDataConstants.COL_Btn].ToString().ToUpper().Equals("NO"))
                            {
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                Keyboard.SendKeys(KeyboardConstants.SPACE);
                            }


                        }
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

      
        private void SelectAndEditTradesWithQuantity(DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid)
        {
            try
            {
                int index=0;
                DataTable newData = testData.Tables[0].Copy();
                //first select row
                DataTable SelectRow = testData.Tables[0].Copy();
               
                newData.Columns.Remove("Bulk Update Alert");
                newData.Columns.Remove("CancelWithoutReplace");
                newData.Columns.Remove("BULK_OR_EDITTRADE");
                SelectRow.Columns.Remove("Bulk Update Alert");
                SelectRow.Columns.Remove("CancelWithoutReplace");
                SelectRow.Columns.Remove("BULK_OR_EDITTRADE");
                SelectRow.Columns.Remove("Button");
                newData.Columns.Remove("Button");

                for (int ind = testData.Tables[0].Rows.Count - 1; ind >= 0; ind--)
                {
                    if (testData.Tables[0].Rows[ind][TestDataConstants.COL_BULKOREDITTARDE].ToString().ToUpper() != "SELECTROW")
                    {
                        SelectRow.Rows[ind].Delete();
                        SelectRow.AcceptChanges();
                    }

                }
                
                for (int ind = testData.Tables[0].Rows.Count - 1; ind >= 0; ind--)
                {


                    if (testData.Tables[0].Rows[ind][TestDataConstants.COL_BULKOREDITTARDE].ToString().ToUpper() == "BULK" || testData.Tables[0].Rows[ind][TestDataConstants.COL_BULKOREDITTARDE].ToString().ToUpper() == "SELECTROW")
                    {
                        newData.Rows[ind].Delete();

                        newData.AcceptChanges();

                    }
                }
                MultiTradingTicket.InvokeMethod("UnselectAllOrders", null);
                foreach (DataRow dr in SelectRow.Rows)
                {
                    SelectTrades(dr);// it will select only one row at a time

                  
                    DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());


                    int rowIndex = (int)GrdTrades.InvokeMethod("GetActiveRow", null);

                    MsaaObject obj = DataGrid.MsaaObject.FindDescendantByName("OrderBindingList", 3000);
                    Dictionary<string, int> columnToIndexMapping = obj.CachedChildren[1].GetColumnIndexMaping(newData);
                    DataRow dtRow = newData.Rows[index];;
                    
                    if (rowIndex >= 0)
                    {
                        var Row = obj.CachedChildren[rowIndex + 1];
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, rowIndex);
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, 0);
                        for (int i = 0; i < newData.Columns.Count; i++)
                        {
                            string colName = newData.Columns[i].ColumnName.ToString();
                            
                               if (colName == "Commission Basis" || colName == "Soft Basis")
                            {
                                string columnValue = dtRow[colName].ToString();
                                
                                   if (!string.IsNullOrEmpty(columnValue))
                                
                                   {
                                
                                       int colIndex = 0;
                                
                                       if (columnToIndexMapping.ContainsKey(colName))
                                {
                                    colIndex = columnToIndexMapping[colName];

                                    if (columnValue == "$#$" || columnValue == "")
                                    {
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }

                                DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                                Wait(500);
                                Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                Dictionary<string, int> dict = new Dictionary<string, int>();
                                dict = WinDataUtilities.GetCurrentSuggestionsList(ref TestDataConstants.Mtt, dict);
                                int elementIndex = GetElementIndex(dict, columnValue);
                                if (elementIndex <= -1)
                                {
                                    Console.WriteLine(columnValue + " does not exist");
                                    throw new Exception(columnValue + " does not exist");
                                }
                                else
                                {
                                    KeyboardUtilities.PressUpKeyWithWait(2 * dict.Count);
                                    for (int j = 0; j < elementIndex; j++)
                                    {
                                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                                    }

                                }
                            }
                               
                                
                                Wait(500);
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
 
                            

                            else if (colName != "Settlement Fx Rate" || colName != "Settlement Amount" || colName != "Settlement Fx Operator" || colName != "PriceButtonPressCount")
                            {
                                string columnValue = dtRow[colName].ToString();
                                int colIndex = 0;
                                if (columnToIndexMapping.ContainsKey(colName))
                                {
                                    colIndex = columnToIndexMapping[colName];

                                    if (columnValue == "$#$" || columnValue == "")
                                    {
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }

                                DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                                Wait(500);
                                Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                                if(string.Equals(colName,TestDataConstants.COL_PRICE))
                                {
                                    if (dtRow.Table.Columns.Contains(TestDataConstants.COL_PRESSPRICEBTN))
                                    {
                                        if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_PRESSPRICEBTN].ToString()))
                                        {
                                            UpdateCellValueConditionsMTT(dtRow[TestDataConstants.COL_PRESSPRICEBTN].ToString(), "",true );
                                        }
                                        else
                                        {
                                UpdateCellValueConditionsMTT(columnValue, "");
                                        }

                                    }
                                    else
                                    {
                                        UpdateCellValueConditionsMTT(columnValue, "");
                                    }
                                }
                                else
                                {
                                    UpdateCellValueConditionsMTT(columnValue, "");
                                }
                                Wait(500);
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                        }
                        DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, "");
                    }
                    index++;
                }
            }
            catch(Exception ex)
            {
                System.Console.Write(ex);
                throw;
            }
        }
        private new void SelectTrades(DataRow dr)
        {
            try
            {

                var msaaObj = GrdTrades.MsaaObject;
                DataTable dtmtt = CSVHelper.CSVAsDataTable(this.GrdTrades.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtmtt), dr);
                int index = dtmtt.Rows.IndexOf(dtRow);
                GrdTrades.InvokeMethod("ScrollToRow", index);
                Wait(1000);
                msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Left);
                msaaObj.FindDescendantByName("OrderBindingList", 5000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                //msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                Wait(1000);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public static void UpdateCellValueConditionsMTT(string cellValue, string appnedValue ,bool presskeys =false)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    Stopwatch tmr = new Stopwatch();
                    tmr.Start();
                    Keyboard.SendKeys("[END]");
                    while (tmr.ElapsedMilliseconds <= 5000)
                    {
                        Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                    }
                    tmr.Stop();
                    if (presskeys != false)
                    {
                        int intValue = int.Parse(cellValue);
                        int numberOfPresses = Math.Abs(intValue) + 1; 

                        if (intValue > 0)
                        {
                            for (int i = 0; i < numberOfPresses; i++)
                            {
                                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                            }
                        }
                        else if (intValue < 0)
                        {
                            for (int i = 0; i < numberOfPresses; i++)
                            {
                                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                            }
                        }
                    }

                    else
                    {
                    if (!cellValue.Equals(ExcelStructureConstants.BLANK_CONST))
                        Keyboard.SendKeys(cellValue + appnedValue);
                }
            }
            }
            catch (Exception ex)
            {
                Log.Error("Update cell value is failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        private void BulkChange(DataRow dr, UIWindow btn,bool verifySuggesstions = false)
        {
            try
            {
                BtnClear.Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_ORDER_SIDE].ToString() != String.Empty && CmbOrderSide.IsEnabled && dr[TestDataConstants.COL_ORDER_SIDE].ToString() != "$#$")
                {

                    if (verifySuggesstions != false)
                    {
                        try
                        {
                            CmbOrderSide.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_ORDER_SIDE].ToString());
                            ExtentionMethods.EnsureTextWindowLength(CmbOrderSide, dr[TestDataConstants.COL_ORDER_SIDE].ToString());
                            Wait(6000);
                            bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_ORDER_SIDE].ToString(), CmbOrderSide, TestDataConstants.Mtt);
                            if (isverificationsuceeded == false)
                            {
                                throw new Exception("VerifySuggestionList failed");
                            }
                            MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                            if (rethrow)
                                throw;
                        }
                    }
                    else
                    {
                        CmbOrderSide.Click(MouseButtons.Left);
                        CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_SIDE].ToString();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }

                }


                String[] Accounts = dr["Account"].ToString().Split(',');
                String[] AllocationPercentage = dr["Account %"].ToString().Split(',');

                if (dr[TestDataConstants.COL_ALLOCATION].ToString() != string.Empty && CmbAllocation.IsEnabled && dr[TestDataConstants.COL_ALLOCATION].ToString() != "$#$")
                {


                    if (verifySuggesstions != false)
                    {
                        try
                        {

                            CmbAllocation.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_ALLOCATION].ToString());
                            ExtentionMethods.EnsureTextWindowLength(CmbAllocation, dr[TestDataConstants.COL_ALLOCATION].ToString());
                            Wait(2000);



                            bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_ALLOCATION].ToString(), CmbAllocation, TestDataConstants.Mtt);
                            MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                            if (isverificationsuceeded == false)
                            {
                                throw new Exception("VerifySuggestionList failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                            if (rethrow)
                                throw;
                        }
                    }
                    else
                    {
                        Stopwatch timer = new Stopwatch();
                        timer.Start();
                        while (!(CmbAllocation.Text.Equals(dr[TestDataConstants.COL_ALLOCATION].ToString())) || timer.ElapsedMilliseconds >= 6000)
                        {
                            //CmbAllocation.Click(MouseButtons.Left);
                            CmbAllocation.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ALLOCATION].ToString();

                        }
                        timer.Stop();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }


                }

                else if (Accounts.Length > 1)
                {
                    if (dr.Table.Columns.Contains(TestDataConstants.COL_Account) && dr[TestDataConstants.COL_ALLOCATION].ToString() == String.Empty)
                    {
                        BtnCustomAllocation.Click(MouseButtons.Left);
                        Wait(2000);


                        if (dr.Table.Columns.Contains(TestDataConstants.CLEAROLDCA))
                        {

                            if (dr[TestDataConstants.CLEAROLDCA].ToString().ToUpper() == "YES")
                            {
                                GrdAccounts.Click(MouseButtons.Left);
                                BtnClear.Click(MouseButtons.Left);
                                Wait(6000);

                            }
                            else
                            {
                                // no action needed 
                            }

                        }

                        int rowCount = 0, msaaId = 0;
                        const int colCount = 2;
                        foreach (String Account in Accounts)
                        {
                            int isNewRow = 0;
                            if (rowCount == 1)
                                msaaId = 1;

                            var gridMssaObject = GrdAccounts.MsaaObject;

                            for (int i = 1; i < gridMssaObject.CachedChildren[msaaId].ChildCount - 1; i++)
                            {

                                if (gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Value != null && gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Value.Equals(Account))
                                {
                                    gridMssaObject.CachedChildren[msaaId].CachedChildren[i].CachedChildren[0].Click(MouseButtons.Left);

                                    for (int j = 1; j < colCount; j++)
                                        Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                    Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                    rowCount++;
                                    isNewRow++;
                                    break;
                                }
                            }
                            if (isNewRow == 0)
                            {
                                for (int k = 1; k < gridMssaObject.CachedChildren[msaaId].ChildCount - 1; k++)
                                {
                                    if (gridMssaObject.CachedChildren[msaaId].CachedChildren[k].Name.Equals("Template Add Row"))
                                    {
                                        gridMssaObject.CachedChildren[msaaId].CachedChildren[k].CachedChildren[0].Click(MouseButtons.Left);

                                        Keyboard.SendKeys(Account);
                                        for (int j = 1; j < colCount; j++)
                                            Keyboard.SendKeys(KeyboardConstants.TABKEY);

                                        Keyboard.SendKeys(AllocationPercentage[rowCount]);
                                        rowCount++;
                                        break;
                                    }
                                }
                            }
                        }
                        BtnOK.Click(MouseButtons.Left);
                    }

                }

                Wait(3000);
                //Keyboard.SendKeys(KeyboardConstants.TABKEY);

                if (dr[TestDataConstants.COL_BROKER].ToString() != string.Empty && CmbBroker.IsEnabled && dr[TestDataConstants.COL_BROKER].ToString() != "$#$")
                {

                    if (verifySuggesstions != false)
                    {
                        try
                        {
                            CmbBroker.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_BROKER].ToString());
                            ExtentionMethods.EnsureTextWindowLength(CmbBroker, dr[TestDataConstants.COL_BROKER].ToString());
                            Wait(6000);
                            bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_BROKER].ToString(), CmbBroker, TestDataConstants.Mtt);
                            if (isverificationsuceeded == false)
                            {
                                throw new Exception("VerifySuggestionList failed");
                            }
                            MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                            if (rethrow)
                                throw;
                        }
                    }
                    else
                    {
                        CmbBroker.Click(MouseButtons.Left);
                        CmbBroker.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_BROKER].ToString();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }


                }

                if (dr[TestDataConstants.COL_VENUE].ToString() != string.Empty && Cmbvenue.IsEnabled && dr[TestDataConstants.COL_VENUE].ToString() != "$#$")
                {

                    if (verifySuggesstions != false)
                    {
                        try
                        {
                            Cmbvenue.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_VENUE].ToString());
                            ExtentionMethods.EnsureTextWindowLength(Cmbvenue, dr[TestDataConstants.COL_VENUE].ToString());
                            Wait(6000);

                            bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_VENUE].ToString(), Cmbvenue, TestDataConstants.Mtt);
                            if (isverificationsuceeded == false)
                            {
                                throw new Exception("VerifySuggestionList failed");
                            }
                            MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                            if (rethrow)
                                throw;
                        }
                    }
                    else
                    {
                        Cmbvenue.Click(MouseButtons.Left);
                        Cmbvenue.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_VENUE].ToString();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    }


                }

                if (dr[TestDataConstants.COL_TIF].ToString() != string.Empty && CmbTIF.IsEnabled && dr[TestDataConstants.COL_TIF].ToString() != "$#$")
                {

                    if (verifySuggesstions != false)
                    {
                        try
                        {
                            CmbTIF.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_TIF].ToString());
                            ExtentionMethods.EnsureTextWindowLength(CmbTIF, dr[TestDataConstants.COL_TIF].ToString());
                            Wait(6000);
                            bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_TIF].ToString(), CmbTIF, TestDataConstants.Mtt);
                            if (isverificationsuceeded == false)
                            {
                                throw new Exception("VerifySuggestionList failed");
                            }
                            MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                            if (rethrow)
                                throw;
                        }
                    }
                    else
                    {
                        CmbTIF.Click(MouseButtons.Left);
                        CmbTIF.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TIF].ToString();
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        if (CmbTIF.Properties[TestDataConstants.TEXT_PROPERTY].ToString().Equals("Good Till Date"))
                        {
                            if (BtnExpireTime.IsVisible)
                            {
                                BtnExpireTime.Click(MouseButtons.Left);
                                Wait(1500);
                                GTD(dr);
                            }

                        }
                    }
                }

                    if (dr[TestDataConstants.COL_ORDER_TYPE].ToString() != string.Empty && CmbOrderType.IsEnabled && dr[TestDataConstants.COL_ORDER_TYPE].ToString() != "$#$")
                    {

                        if (verifySuggesstions != false)
                        {
                            try
                            {
                                CmbOrderType.Click(MouseButtons.Left);
                                Keyboard.SendKeys(dr[TestDataConstants.COL_ORDER_TYPE].ToString());
                                ExtentionMethods.EnsureTextWindowLength(CmbOrderType, dr[TestDataConstants.COL_ORDER_TYPE].ToString());
                                Wait(6000);
                                bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_ORDER_TYPE].ToString(), CmbOrderType, TestDataConstants.Mtt);
                                if (isverificationsuceeded == false)
                                {
                                    throw new Exception("VerifySuggestionList failed");
                                }
                                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                                if (rethrow)
                                    throw;
                            }
                        }
                        else
                        {
                            CmbOrderType.Click(MouseButtons.Left);
                            CmbOrderType.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_TYPE].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }

                    }

                    if (dr[TestDataConstants.COL_LIMIT].ToString() != string.Empty && NmrcLimit.IsEnabled && dr[TestDataConstants.COL_LIMIT].ToString() != "$#$")
                    {

                        if (verifySuggesstions != false)
                        {
                            try
                            {
                                NmrcLimit.Click(MouseButtons.Left);
                                Keyboard.SendKeys(dr[TestDataConstants.COL_LIMIT].ToString());
                                ExtentionMethods.EnsureTextWindowLength(NmrcLimit, dr[TestDataConstants.COL_LIMIT].ToString());
                                Wait(6000);

                                bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_LIMIT].ToString(), NmrcLimit, TestDataConstants.Mtt);
                                if (isverificationsuceeded == false)
                                {
                                    throw new Exception("VerifySuggestionList failed");
                                }
                                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                                if (rethrow)
                                    throw;
                            }
                        }
                        else
                        {
                            NmrcLimit.Click(MouseButtons.Left);
                            NmrcLimit.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_LIMIT].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }


                    }

                    if (dr[TestDataConstants.COL_STOP].ToString() != string.Empty && NmrcStop.IsEnabled && dr[TestDataConstants.COL_STOP].ToString() != "$#$")
                    {

                        if (verifySuggesstions != false)
                        {
                            try
                            {
                                NmrcStop.Click(MouseButtons.Left);
                                Keyboard.SendKeys(dr[TestDataConstants.COL_STOP].ToString());
                                SendKeys.SendWait("{DELETE}");
                                Wait(6000);
                                bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_STOP].ToString(), NmrcStop, TestDataConstants.Mtt);
                                if (isverificationsuceeded == false)
                                {
                                    throw new Exception("VerifySuggestionList failed");
                                }
                                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                                if (rethrow)
                                    throw;
                            }
                        }
                        else
                        {
                            NmrcStop.Click(MouseButtons.Left);
                            NmrcStop.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STOP].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }

                    }

                    if (dr[TestDataConstants.COL_STRATEGY].ToString() != string.Empty && CmbStrategy.IsEnabled && dr[TestDataConstants.COL_STRATEGY].ToString() != "$#$")
                    {

                        if (verifySuggesstions != false)
                        {
                            try
                            {
                                CmbStrategy.Click(MouseButtons.Left);
                                Keyboard.SendKeys(dr[TestDataConstants.COL_STRATEGY].ToString());
                                ExtentionMethods.EnsureTextWindowLength(CmbStrategy, dr[TestDataConstants.COL_STRATEGY].ToString());
                                Wait(6000);

                                bool isverificationsuceeded = VerifySuggestionList(dr[TestDataConstants.COL_STRATEGY].ToString(), CmbStrategy, TestDataConstants.Mtt);
                                if (isverificationsuceeded == false)
                                {
                                    throw new Exception("VerifySuggestionList failed");
                                }
                                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                                if (rethrow)
                                    throw;
                            }
                        }
                        else
                        {
                            CmbStrategy.Click(MouseButtons.Left);
                            CmbStrategy.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STRATEGY].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }

                    }
                    if (dr[TestDataConstants.COL_COMMISSION_TYPE].ToString() != string.Empty || dr[TestDataConstants.COL_SOFTTYPE_BASIS].ToString() != string.Empty)
                    {
                        if (GrpBoxCommisionAttribute.Expanded != true)
                        {
                            GrpBoxCommisionAttribute.Click(MouseButtons.Left);
                        }
                        Commission.Click(MouseButtons.Left);
                        if (dr[TestDataConstants.COL_COMMISSION_TYPE].ToString() != string.Empty && dr[TestDataConstants.COL_COMMISSION_TYPE].ToString() != "$#$")
                        {
                            CmbCommission.Click(MouseButtons.Left);
                            CmbCommission.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_COMMISSION_TYPE].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        if (dr[TestDataConstants.COL_COMMISSION_RATEMTT].ToString() != string.Empty && dr[TestDataConstants.COL_COMMISSION_RATEMTT].ToString() != "$#$")
                        {
                            NmrcCommission.Click(MouseButtons.Left);
                            NmrcCommission.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_COMMISSION_RATEMTT].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        if (dr[TestDataConstants.COL_SOFTTYPE_BASIS].ToString() != string.Empty && dr[TestDataConstants.COL_SOFTTYPE_BASIS].ToString() != "$#$")
                        {
                            CmbSoft.Click(MouseButtons.Left);
                            CmbSoft.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SOFTTYPE_BASIS].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        if (dr[TestDataConstants.COL_SOFT_RATEMTT].ToString() != string.Empty && dr[TestDataConstants.COL_SOFT_RATEMTT].ToString() != "$#$")
                        {
                            NmrcSoft.Click(MouseButtons.Left);
                            NmrcSoft.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_SOFT_RATEMTT].ToString();
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        }
                        if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE1_MTT].ToString() != string.Empty || dr[TestDataConstants.COL_TRADE_ATTRIBUTE2_MTT].ToString() != string.Empty || dr[TestDataConstants.COL_TRADE_ATTRIBUTE3_MTT].ToString() != string.Empty || dr[TestDataConstants.COL_TRADE_ATTRIBUTE4_MTT].ToString() != string.Empty || dr[TestDataConstants.COL_TRADE_ATTRIBUTE5_MTT].ToString() != string.Empty || dr[TestDataConstants.COL_TRADE_ATTRIBUTE6_MTT].ToString() != string.Empty)
                        {
                            if (GrpBoxCommisionAttribute.Expanded != true)
                            {
                                GrpBoxCommisionAttribute.Click(MouseButtons.Left);
                            }
                            TradeAttributes.Click(MouseButtons.Left);
                            if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE1_MTT].ToString() != string.Empty && dr[TestDataConstants.COL_TRADE_ATTRIBUTE1_MTT].ToString() != "$#$")
                            {
                                CmbTradeAttribute1.Click(MouseButtons.Left);
                                CmbTradeAttribute1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE1_MTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE2_MTT].ToString() != string.Empty && dr[TestDataConstants.COL_TRADE_ATTRIBUTE2_MTT].ToString() != "$#$")
                            {
                                CmbTradeAttribute2.Click(MouseButtons.Left);
                                CmbTradeAttribute2.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE2_MTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE3_MTT].ToString() != string.Empty && dr[TestDataConstants.COL_TRADE_ATTRIBUTE3_MTT].ToString() != "$#$")
                            {
                                CmbTradeAttribute3.Click(MouseButtons.Left);
                                CmbTradeAttribute3.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE3_MTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE4_MTT].ToString() != string.Empty && dr[TestDataConstants.COL_TRADE_ATTRIBUTE4_MTT].ToString() != "$#$")
                            {
                                CmbTradeAttribute4.Click(MouseButtons.Left);
                                CmbTradeAttribute4.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE4_MTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE5_MTT].ToString() != string.Empty && dr[TestDataConstants.COL_TRADE_ATTRIBUTE5_MTT].ToString() != "$#$")
                            {
                                CmbTradeAttribute5.Click(MouseButtons.Left);
                                CmbTradeAttribute5.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE5_MTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_TRADE_ATTRIBUTE6_MTT].ToString() != string.Empty && dr[TestDataConstants.COL_TRADE_ATTRIBUTE6_MTT].ToString() != "$#$")
                            {
                                CmbTradeAttribute6.Click(MouseButtons.Left);
                                CmbTradeAttribute6.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADE_ATTRIBUTE6_MTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                        }
                        if (dr[TestDataConstants.COL_EXECUTION_INSTRUCTIONSMTT].ToString() != string.Empty || dr[TestDataConstants.COL_HANDLING_INSTRUCTIONSMTT].ToString() != string.Empty || dr[TestDataConstants.COL_TRADER].ToString() != string.Empty)
                        {
                            if (GrpBoxCommisionAttribute.Expanded != true)
                            {
                                GrpBoxCommisionAttribute.Click(MouseButtons.Left);
                            }
                            Other.Click(MouseButtons.Left);
                            if (dr[TestDataConstants.COL_EXECUTION_INSTRUCTIONSMTT].ToString() != string.Empty && dr[TestDataConstants.COL_EXECUTION_INSTRUCTIONSMTT].ToString() != "$#$")
                            {
                                CmbExecution.Click(MouseButtons.Left);
                                CmbExecution.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_EXECUTION_INSTRUCTIONSMTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_HANDLING_INSTRUCTIONSMTT].ToString() != string.Empty && dr[TestDataConstants.COL_HANDLING_INSTRUCTIONSMTT].ToString() != "$#$")
                            {
                                CmbHandling.Click(MouseButtons.Left);
                                CmbHandling.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_HANDLING_INSTRUCTIONSMTT].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                            if (dr[TestDataConstants.COL_TRADER].ToString() != string.Empty && dr[TestDataConstants.COL_TRADER].ToString() != "$#$")
                            {
                                CmbTrader.Click(MouseButtons.Left);
                                CmbTrader.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TRADER].ToString();
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            }
                        }

                    }
                    if (dr.Table.Columns.Contains(TestDataConstants.COL_CLICK_CLEAR_AFTER_ENTERING_VALUES))
                    {
                        if (dr[TestDataConstants.COL_CLICK_CLEAR_AFTER_ENTERING_VALUES].ToString().ToUpper().Equals("YES"))
                        {
                            BtnClear.Click(MouseButtons.Left);
                        }
                        else
                            BtnCommit.Click(MouseButtons.Left);
                    }
                    else
                        BtnCommit.Click(MouseButtons.Left);

                }
            

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        
        }
        
        protected void GTD(DataRow dr)
        {
            try
            {
                string str = dr[TestDataConstants.COL_Expiration_Date].ToString();
                string[] dates = str.Split('/');
                var nextDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[0]), int.Parse(dates[1]));
                var today = DateTime.Now;
                var diffOfDates = nextDate.Day - today.Day;
                Wait(2000);
                for (int i = 0; i < diffOfDates; i++)
                {
                    Keyboard.SendKeys(KeyboardConstants.RIGHT_ARROWKEY);
                }
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                return;
            }
            catch (Exception) { throw; }
        }
        static bool IsNumeric(string input)
        {
           

            return Regex.IsMatch(input, @"^[-]?\d+(\.\d+)?$");

        }
    }
    
}

