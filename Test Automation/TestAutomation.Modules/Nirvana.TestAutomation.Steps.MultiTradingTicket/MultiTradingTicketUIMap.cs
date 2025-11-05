using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Diagnostics;
using Nirvana.TestAutomation.UIAutomation;
namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    [UITestFixture]
    public partial class MultiTradingTicketUIMap : UIMap
    {
        public MultiTradingTicketUIMap()
        {
            InitializeComponent();

        }

        public MultiTradingTicketUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Edit top row on MTT
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <param name="DataGrid"></param>
        public void EditTrades(DataSet testData, Dictionary<int, string> sheetIndexToName, UIUltraGrid DataGrid)
        {
            try
            {
                DataTable excelData = testData.Tables[sheetIndexToName[0]];
                DataTable dtGridData = CSVHelper.CSVAsDataTable(DataGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var MssaObj = DataGrid.MsaaObject.FindDescendantByName("OrderBindingList row 1", 3000);
                Dictionary<string, int> columnToIndexMapping = MssaObj.GetColumnIndexMaping(excelData);
                DataRow excelDataRow = excelData.Rows[0];
                var Row = MssaObj;
                for (int i = 0; i < excelData.Columns.Count; i++)
                {
                    string colName = excelData.Columns[i].ColumnName.ToString();
                    string columnValue = excelDataRow[colName].ToString();
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
                    Wait(1000);
                    DataGrid.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, colName);
                    Wait(500);
                    Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                    Row.CachedChildren[colIndex].Click(MouseButtons.Left);
                    Keyboard.SendKeys(columnValue);
                    //Wait(1000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void BulkChange(DataRow dr, UIWindow btn )
        {
            try
            {
                BtnClear.Click(MouseButtons.Left);
                if (dr[TestDataConstants.COL_ORDER_SIDE].ToString() != String.Empty && CmbOrderSide.IsEnabled && dr[TestDataConstants.COL_ORDER_SIDE].ToString() != "$#$")
                {
                    CmbOrderSide.Click(MouseButtons.Left);
                    CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_SIDE].ToString();

                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_ACCOUNT].ToString() != string.Empty && CmbAllocation.IsEnabled && dr[TestDataConstants.COL_ACCOUNT].ToString() != "$#$")
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    while (!(CmbAllocation.Text.Equals(dr[TestDataConstants.COL_ACCOUNT].ToString())) || timer.ElapsedMilliseconds >= 6000)
                    {
                        //CmbAllocation.Click(MouseButtons.Left);
                        CmbAllocation.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ACCOUNT].ToString();
                    }
                    timer.Stop();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }
                if (dr.Table.Columns.Contains(TestDataConstants.COL_CUSTOMACCOUNT) && (dr[TestDataConstants.COL_ACCOUNT].ToString() == String.Empty))
                {
                    String[] Accounts = dr["Custom Account"].ToString().Split(',');
                    String[] AllocationPercentage = dr["Custom Account %"].ToString().Split(',');

                    if (Accounts.Length > 1)
                    {

                        BtnCustomAllocation.Click(MouseButtons.Left);
                        Wait(2000);


                        if (dr.Table.Columns.Contains(TestDataConstants.CLEAROLDCA))
                        {

                            if (dr[TestDataConstants.CLEAROLDCA].ToString().ToUpper() == "YES")
                            {
                                GrdAccounts.Click(MouseButtons.Left);
                                BtnClear.Click(MouseButtons.Left);
                              //  Wait(6000);

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

               // Wait(3000);


                if (dr[TestDataConstants.COL_BROKER].ToString() != string.Empty && CmbBroker.IsEnabled && dr[TestDataConstants.COL_BROKER].ToString() != "$#$")
                {
                    CmbBroker.Click(MouseButtons.Left);
                    CmbBroker.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_BROKER].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_VENUE].ToString() != string.Empty && Cmbvenue.IsEnabled && dr[TestDataConstants.COL_VENUE].ToString() != "$#$")
                {
                    Cmbvenue.Click(MouseButtons.Left);
                    Cmbvenue.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_VENUE].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr.Table.Columns.Contains(TestDataConstants.COL_ALGOTYPE))
                {
                    if (dr[TestDataConstants.COL_VENUE].ToString() == "Algo" && dr[TestDataConstants.COL_BROKER].ToString() != string.Empty)
                    {
                        if (BtnAlgo.IsVisible && dr[TestDataConstants.COL_ALGOTYPE].ToString() == string.Empty)
                        {
                            BtnAlgo.Click(MouseButtons.Left);
                            Wait(3000);
                            if (AlgoControlPopUp.IsVisible)
                            {
                                BtnOk1.Click(MouseButtons.Left);
                            }
                        }
                        if (dr[TestDataConstants.COL_ALGOTYPE].ToString() != string.Empty)
                        {
                            BtnAlgo.Click(MouseButtons.Left);
                            Keyboard.SendKeys(dr[TestDataConstants.COL_ALGOTYPE].ToString());
                            BtnOk1.Click(MouseButtons.Left);
                            
                        }
                    }
                    Wait(3000);
                }

                if (dr[TestDataConstants.COL_TIF].ToString() != string.Empty && CmbTIF.IsEnabled && dr[TestDataConstants.COL_TIF].ToString() != "$#$")
                {
                    CmbTIF.Click(MouseButtons.Left);
                    CmbTIF.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_TIF].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_ORDER_TYPE].ToString() != string.Empty && CmbOrderType.IsEnabled && dr[TestDataConstants.COL_ORDER_TYPE].ToString() != "$#$")
                {
                    CmbOrderType.Click(MouseButtons.Left);
                    CmbOrderType.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_ORDER_TYPE].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_LIMIT].ToString() != string.Empty && NmrcLimit.IsEnabled && dr[TestDataConstants.COL_LIMIT].ToString() != "$#$")
                {
                    NmrcLimit.Click(MouseButtons.Left);
                    NmrcLimit.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_LIMIT].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_STOP].ToString() != string.Empty && NmrcStop.IsEnabled && dr[TestDataConstants.COL_STOP].ToString() != "$#$")
                {
                    NmrcStop.Click(MouseButtons.Left);
                    NmrcStop.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STOP].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                }

                if (dr[TestDataConstants.COL_STRATEGY].ToString() != string.Empty && CmbStrategy.IsEnabled && dr[TestDataConstants.COL_STRATEGY].ToString() != "$#$")
                {
                    CmbStrategy.Click(MouseButtons.Left);
                    CmbStrategy.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_STRATEGY].ToString();
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
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
                    {
                        BtnCommit.Click(MouseButtons.Left);
                    }
                }
                else if (dr.Table.Columns.Contains(TestDataConstants.COL_CLICK_ON_COMMIT))
                {
                    if (string.IsNullOrEmpty(dr[TestDataConstants.COL_CLICK_ON_COMMIT].ToString()) || dr[TestDataConstants.COL_CLICK_ON_COMMIT].ToString().ToLower() == "yes")
                    {
                        if (BtnCommit.IsVisible)
                        {
                            Console.WriteLine("COMMIT Button is Visible and verified");
                            // BtnCommit.CurrentImage.GetThumbnailImage


                        }
                        BtnCommit.Click(MouseButtons.Left);
                    }

                    // else if (dr[TestDataConstants.COL_CLICK_ON_COMMIT].ToString().ToLower() == "no")=>donothing

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

        protected void SelectTrades(DataRow dr)
        {
            try
            {

                var msaaObj = GrdTrades.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.GrdTrades.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                GrdTrades.InvokeMethod("ScrollToRow", index);
                Wait(1000);
                msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Left);
                msaaObj.FindDescendantByName("OrderBindingList", 5000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                //msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
              //  Wait(1000);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        protected void GetAllColumnsOnGrid(DataTable newData)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataRow row in newData.Rows)
                {
                    string value = row["AddOrRemoveColumn"].ToString();
                    columns.Add(value);
                }
                this.GrdTrades.InvokeMethod("AddColumnsToGrid", columns);

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void RemoveColumnFromGrid(DataTable newData)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataRow row in newData.Rows)
                {
                    string value = row["AddOrRemoveColumn"].ToString();
                    columns.Add(value);
                }
                this.GrdTrades.InvokeMethod("RemoveColumnsToGrid", columns);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public HashSet<string> provideAllPossibleSuggestionValues(UIWindow cmb, string uniqueDescendantByName)
        {
            try
            {
                HashSet<string> hs = new HashSet<string>();
                var msaaObj = cmb.MsaaObject;

                var childmsaaObj = msaaObj.FindDescendantByName(uniqueDescendantByName, 5000).CachedChildren;
                int itemsCount = childmsaaObj.Count;
                for (int i = 0; i < itemsCount; i++)
                {
                    var Item = childmsaaObj[i];
                    //  
                    var boundingRectangle = childmsaaObj[i].Bounds;
                    Console.WriteLine(boundingRectangle.Top + "," + boundingRectangle.Bottom + "," + boundingRectangle.Left + "," + boundingRectangle.Right);
                    //string itemText = menuItem.Value;

                    if (!string.IsNullOrEmpty(Item.Name))
                    {
                        //li.Add(menuItem.Name.ToString());
                        string itemName = Item.Name.ToString();
                        if (!hs.Contains(itemName))
                        {
                            hs.Add(itemName);
                        }
                    }
                }
                return hs;
            }
            catch (Exception) { throw; }
        }
        public bool VerifySuggestionList(string Value,UIWindow targetwindow,string MainWindow)
        {
            bool verified = false;
            try
            {
               // string venueValue = dr[TestDataConstants.COL_BROKER].ToString();
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
               // Wait(1000);

                HashSet<string> hs = provideAllPossibleSuggestionValues(targetwindow, "List");
                HashSet<string> activesuggesstionList = WinDataUtilities.GetCurrentSuggestionsList(MainWindow);

                foreach (string dataitem in activesuggesstionList)
                {
                    bool wordVerified = false;
                    if (hs.Contains(dataitem))
                    {
                        Console.WriteLine(dataitem + " exists in suggestions");
                    }
                    if (!hs.Contains(dataitem))
                    {
                        throw new Exception("DataItem verification failed");
                    }
                    if (wordVerified != true)
                    {
                        if (string.Equals(Value, dataitem.Substring(0, Value.Length)))
                        {
                            wordVerified = true;
                        }
                    }
                    if (wordVerified == false)
                    {
                        // Throw an exception or handle as needed
                        throw new Exception("Word verification failed");
                    }
                }
                verified =true;
                Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                return verified;
            }
            catch (Exception ex)
            {
               Console.WriteLine("Exception occured while verification:-" +ex.Message);
               return verified;
            }
            return verified;
        }
        public static int GetElementIndex(Dictionary<string, int> dict, string element)
        {

            int index = 0;
            try
            {
                foreach (var kvp in dict)
                {
                    if (kvp.Key == element)
                    {
                        return dict[element];
                    }
                    index++;
                }
            }
            catch (Exception) { throw; }
          
            return -1;
       }

    }
}
