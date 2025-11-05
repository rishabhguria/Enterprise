using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Steps.DailyValuation;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Configuration;
using System.Globalization;
using Nirvana.TestAutomation.UIAutomation;
namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    class UpdateMPAndFC : DailyCashUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                
                if (testData.Tables[0].Columns.Contains("Account") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["Account"].ToString()))
                {
                    if (testData.Tables[0].Columns.Contains("Master Fund"))
                    {
                        UIAutomationHelper uiAutomationHelper = new UIAutomationHelper();
                        bool result = uiAutomationHelper.UpdateMPFCNav(testData.Tables[0], "OPEN_DAILY_VAL", "MarkPriceAndForexConversion");
                        if (!result)
                        {
                            throw new Exception("Update Nav Failed at grid Action");
                        }
                    }
                    else
                    UpdateDailyCash(testData);
                }
                else if (testData.Tables[0].Columns.Contains("From Currency") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["From Currency"].ToString()))
                {
                    string columnName = string.Empty;
                    foreach (DataColumn column in testData.Tables[0].Columns)
                    {
                        bool isDateFormat = true;

                        var value = column.ColumnName;

                        DateTime parsedDate;

                        if (!DateTime.TryParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            isDateFormat = false;
                        }


                        if (isDateFormat)
                        {
                            columnName = value;
                            break;
                        }
                    }
                    testData.Tables[0].Columns[columnName].ColumnName = "Price";

                    for (int i = testData.Tables[sheetIndexToName[0]].Rows.Count - 1; i >= 0; i--)
                    {
                        if (!string.IsNullOrEmpty(testData.Tables[0].Rows[i]["Price"].ToString()) && string.Equals(testData.Tables[0].Rows[i]["Price"].ToString(), "0", StringComparison.OrdinalIgnoreCase))
                        {
                            testData.Tables[sheetIndexToName[0]].Rows[i].Delete();
                        }
                    }
                    UpdateForexConversion(testData, sheetIndexToName, columnName);
                }
                else if (testData.Tables[0].Columns.Contains("Symbol") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["Symbol"].ToString()))
                {
                    string columnName = string.Empty;
                    foreach (DataColumn column in testData.Tables[0].Columns)
                    {
                        bool isDateFormat = true;

                        var value = column.ColumnName;
                
                        DateTime parsedDate;
                
                        if (!DateTime.TryParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            isDateFormat = false;
                        }
                        

                        if (isDateFormat)
                        {
                            columnName = value;
                            break;
                        }
                    }
                    testData.Tables[0].Columns[columnName].ColumnName = "Price";

                    for (int i = testData.Tables[sheetIndexToName[0]].Rows.Count - 1; i >= 0; i--)
                    {
                        if (!string.IsNullOrEmpty(testData.Tables[0].Rows[i]["Price"].ToString()) && string.Equals(testData.Tables[0].Rows[i]["Price"].ToString(), "0", StringComparison.OrdinalIgnoreCase))
                        {
                            testData.Tables[sheetIndexToName[0]].Rows[i].Delete();
                        }
                    } 
                    OpenMarkPriceTab();
                    UpdateNewMarkPrices(testData, sheetIndexToName, columnName);
                    BtnSave.Click(MouseButtons.Left);
                    Wait(1000);
                    GrdPivotDisplay2.Click(MouseButtons.Left);
                }
                return _result;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                _result.IsPassed = false;
                return _result;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref MarkPriceAndForexConversion_UltraFormManager_Dock_Area_Top);
            }
        }

        private void UpdateNewMarkPrices(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName, string date)
        {
            try
            {
                DataTable dtDailyVolatility = CSVHelper.CSVAsDataTable(GrdPivotDisplay2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay2.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow;
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_Account))
                        foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_SYMBOL + "]='{0}' AND [" + TestDataConstants.COL_Account + "] ='{1}'", dataRow[TestDataConstants.COL_SYMBOL], dataRow[TestDataConstants.COL_Account]));
                    else
                        foundRow = dtDailyVolatility.Select(String.Format(@"[" + TestDataConstants.COL_SYMBOL + "]='{0}'", dataRow[TestDataConstants.COL_SYMBOL]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtDailyVolatility.Rows.IndexOf(foundRow[0]);
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay2.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index + 1);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            //TODO: GetColumnIndexMaping() call this method on grid and out of this for loop
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtDailyVolatility);
                            SetValueInGrid1(columToMSAAIndexMapping, dataRow, gridRow, date);

                            // Changes done for adding mark prices in case of single date and multiple symbols.
                            columToMSAAIndexMapping.Clear();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetValueInGrid1(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName)
        {
            try
            {
                if (!String.IsNullOrEmpty(dataRow["Price"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdPivotDisplay2.Click(MouseButtons.Left);
                        GrdPivotDisplay2.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow["Price"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void OpenMarkPriceTab()
        {
            try
            {
                //Shortcut to open daily valuation module(CTRL + SHIFT + D)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                // Wait(5000);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
                //DataManagement.Click(MouseButtons.Left);
                //DailyValuation.Click(MouseButtons.Left);
                MarkPriceAndForexConversion.WaitForVisible();
                MarkPrice.Click(MouseButtons.Left);
                GrdPivotDisplay2.WaitForVisible();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }

        
        private void UpdateForexConversion(DataSet testData, Dictionary<int, string> sheetIndexToName, string date)
        {
            OpenForexConversion();
            /*Added this pop-up handling here which was needed in order for the step to work properly.
             * https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/58128
             * Modified by Yash Gupta
             */
            Wait(2000);
            if (CONFIRMATION.IsVisible || CONFIRMATION.IsEnabled)//Added wait for the popup to be discovered
            {
                if (ButtonYes.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
                }
            }
            UpdateForexConversionData(testData, sheetIndexToName, date);
            BtnSave.Click(MouseButtons.Left);
            Wait(1000);
            GrdPivotDisplay1.Click(MouseButtons.Left);
        }

        
        private void UpdateDailyCash(DataSet testData)
        {
            OpenDailyValuation();
            /*Added this pop-up handling here which was needed in order for the step to work properly.
             * https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/58128
             * Modified by Yash Gupta
             */
            Wait(2000);
            if (CONFIRMATION.IsVisible || CONFIRMATION.IsEnabled)//Added wait for the popup to be discovered
            {
                if (ButtonYes.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
                }
            }
            ClearOldCash();
            var gridMssaObject = GrdPivotDisplay.MsaaObject;
            BtnAdd.Click(MouseButtons.Left);

            //Dictionary that stores mapping of column names with it's index
            Dictionary<string, int> colToIndexMappingDictionary = new Dictionary<string, int>();

            int Children = 0;

            //if( (gridMssaObject.CachedChildren[0].CachedChildren[1].Accessible).Equals( true))
            // Children = gridMssaObject.CachedChildren[0].CachedChildren[1].ChildCount;
            // else
            Children = gridMssaObject.CachedChildren[0].CachedChildren[1].ChildCount;


            for (int i = 0; i < Children; i++)
            {
                //All colums i.e=> account,local currency, cash value
                colToIndexMappingDictionary.Add(gridMssaObject.CachedChildren[0].CachedChildren[1].CachedChildren[i].Name, i);
            }

            //List that stores column names from excel sheet whose value needs to be inserted in grid
            List<string> columnlist = new List<string>();

            foreach (DataColumn columnName in testData.Tables[0].Columns)
            {
                // jo exceeel sshet ke columns h
                columnlist.Add(columnName.ColumnName);
            }
            int recordsCounter = 0;
            int RecordsCount = 1;

            while (RecordsCount != testData.Tables[0].Rows.Count)
            {
                BtnAdd.Click(MouseButtons.Left);
                RecordsCount++;
            }
            gridMssaObject = GrdPivotDisplay.MsaaObject;
            foreach (DataRow dr in testData.Tables[0].Rows)
            {

                // gridMssaObject.CachedChildren[0].CachedChildren[recordsCounter + 1].Click();
                foreach (string col in columnlist)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[recordsCounter + 1].CachedChildren[colToIndexMappingDictionary[col]].Click();
                    gridMssaObject.CachedChildren[0].CachedChildren[recordsCounter + 1].CachedChildren[colToIndexMappingDictionary[col]].Click();
                    Keyboard.SendKeys(dr[col].ToString());
                }
                recordsCounter++;

            }
            GrdPivotDisplay.Click(MouseButtons.Left);
            BtnSave.Click(MouseButtons.Left);
            GrdPivotDisplay.Click(MouseButtons.Left);
        }


        private void ClearOldCash()
        {
            try
            {
                var gridMssaObject = GrdPivotDisplay.MsaaObject;
                int childs = gridMssaObject.CachedChildren[0].ChildCount;
                for (int i = 1; i < childs; i++)
                {
                    gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[1].Click();
                    gridMssaObject.CachedChildren[0].CachedChildren[i].CachedChildren[1].Click();
                    BtnRemove.Click(MouseButtons.Left);

                }
                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void OpenDailyValuation()
        {
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
                // Wait(5000);
                //DataManagement.Click(MouseButtons.Left);
                //DailyValuation.Click(MouseButtons.Left);
                DailyCash.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void UpdateForexConversionData(DataSet testData, Dictionary<int, string> sheetIndexToName, string date)
        {
            try
            {
                DataTable dtForexConversion = CSVHelper.CSVAsDataTable(GrdPivotDisplay1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                var msaaObj = GrdPivotDisplay1.MsaaObject;
                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow;
                    DataRow[] foundRow1;
                    if (testData.Tables[sheetIndexToName[0]].Columns.Contains(TestDataConstants.COL_Account))
                    {
                        foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}' AND [" + TestDataConstants.COL_Account + "] ='{2}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_Account]));
                        foundRow1 = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}' AND [" + TestDataConstants.COL_Account + "] ='{2}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_Account]));
                    }
                    else
                    {
                        foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                        foundRow1 = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY]));
                    }
                    if (foundRow.Length > 0 || foundRow1.Length > 0)
                    {
                        int index;
                        if (foundRow.Length > 0)
                        {
                            index = dtForexConversion.Rows.IndexOf(foundRow[0]);
                        }
                        else
                        {
                            index = dtForexConversion.Rows.IndexOf(foundRow1[0]);

                        }
                        var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                        GrdPivotDisplay1.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtForexConversion);
                        }
                        SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth1.Properties["Text"].ToString());
                    }
                    else
                    {
                        if (!dataRow[TestDataConstants.COL_FROM_CURRENCY].ToString().Contains(dataRow[TestDataConstants.COL_TO_CURRENCY].ToString()))
                        {
                            AddForexConversionData(dataRow, sheetIndexToName);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        private void SetValueInGrid(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName)
        {

            try
            {
                if (!String.IsNullOrEmpty(dataRow["Price"].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdPivotDisplay1.Click(MouseButtons.Left);
                        GrdPivotDisplay1.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        if (!gridRow.CachedChildren[columnIndex].Value.ToString().Equals(dataRow["Price"].ToString()))
                        {
                            Keyboard.SendKeys(dataRow["Price"].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        private void AddForexConversionData(DataRow dataRow, Dictionary<int, string> sheetIndexToName)
        {
            try
            {

                BtnAdd.Click(MouseButtons.Left);
                Wait(1000);
                CmbFromCurrency.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(dataRow[TestDataConstants.COL_FROM_CURRENCY].ToString());

                CmbToCurrency.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(dataRow[TestDataConstants.COL_TO_CURRENCY].ToString());

                TxtFxSymbol.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ENDKEY);
                KeyboardUtilities.PressKey(TestDataConstants.NO_OF_TIMES_BACKSPACE, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(dataRow[TestDataConstants.COL_SYMBOL].ToString());

                DataTable dtForexConversion = CSVHelper.CSVAsDataTable(GrdPivotDisplay1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRow = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                DataRow[] foundRow1 = dtForexConversion.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY]));

                if (String.IsNullOrEmpty(TestDataConstants.COL_SYMBOL.ToString()))
                {
                    throw new System.InvalidOperationException("Add of new pair of currency not done. Symbol is not present in the sheet.");
                }
                if (foundRow.Length > 0 || foundRow1.Length > 0)
                {
                    throw new System.InvalidOperationException("same pair-currency alredy exit or oppsotie, select new pair.");
                }

                BtnAdd1.Click(MouseButtons.Left);
                Wait(3000);
                UpdateAddedForexConversion(dataRow, dtForexConversion);

            }
            catch (Exception)
            {

                throw;
            }
        }
        

        private void UpdateAddedForexConversion(DataRow dataRow, DataTable dtForexConversion)
        {
            try
            {
                int index;
                DataTable dtForexConversionUpdated = CSVHelper.CSVAsDataTable(GrdPivotDisplay1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] foundRowUpdated = dtForexConversionUpdated.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_FROM_CURRENCY], dataRow[TestDataConstants.COL_TO_CURRENCY]));
                DataRow[] foundRow1Updated = dtForexConversionUpdated.Select(String.Format(@"[" + TestDataConstants.COL_FROM_CURRENCY + "] ='{0}' AND [" + TestDataConstants.COL_TO_CURRENCY + "] ='{1}'", dataRow[TestDataConstants.COL_TO_CURRENCY], dataRow[TestDataConstants.COL_FROM_CURRENCY]));
                if (foundRowUpdated.Length > 0)
                {
                    index = dtForexConversionUpdated.Rows.IndexOf(foundRowUpdated[0]);
                }
                else
                {
                    index = dtForexConversionUpdated.Rows.IndexOf(foundRow1Updated[0]);

                }
                GrdPivotDisplay1.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                var msaaObj = GrdPivotDisplay1.MsaaObject;
                var gridRow = msaaObj.CachedChildren[0].CachedChildren[index + 1];
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();
                GrdPivotDisplay1.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                if (columToMSAAIndexMapping.Count == 0)
                {
                    columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtForexConversion);
                    SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, DtDateMonth.Properties["Text"].ToString());
                }

                BtnSave.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected void OpenForexConversion()
        {
            try
            {
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_DAILY_VAL"]);
                ExtentionMethods.WaitForVisible(ref MarkPriceAndForexConversion, 15);
                ForexConversion.Click(MouseButtons.Left);
                /*Added this pop-up handling here which was needed in order for the step to work properly.
                * Modified by Yash Gupta
                */
                Wait(2000);
                if (CONFIRMATION.IsVisible || CONFIRMATION.IsEnabled)//Added wait for the popup to be discovered
                {
                    if (ButtonYes.IsVisible)
                    {
                        ButtonYes.Click(MouseButtons.Left);
                    }
                }
                GrdPivotDisplay1.WaitForVisible();
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
