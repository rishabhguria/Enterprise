using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.PricingInput
{
    class UpdatePricingInput : PricingInputUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PRICING_INP"]);
                ExtentionMethods.WaitForVisible(ref OptionModelInputs, 15);
                OptionModelInputs.WaitForVisible();
                PricingInputs.Click(MouseButtons.Left);
                BtnRefresh.WaitForEnabled();
                All.Click(MouseButtons.Left);
                if (!CheckBoxAll.IsChecked)
                {
                    CheckBoxAll.Click(MouseButtons.Left);
                }
                DataTable dtPricingInputs = CSVHelper.CSVAsDataTable(GrdOptionModel.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var mssaObject = GrdOptionModel.MsaaObject;
                Dictionary<string, int> columnToGridVisiblePosition = new Dictionary<string, int>();
                for (int i = 0; i < dtPricingInputs.Columns.Count; i++)
                {
                    columnToGridVisiblePosition.Add(dtPricingInputs.Columns[i].ColumnName, i);
                }
                Dictionary<string, int> columToMSAAIndexMapping = new Dictionary<string, int>();

                for (int i = testData.Tables[sheetIndexToName[0]].Rows.Count - 1; i >= 0; i--) {

                    if (string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[i]["User Px"].ToString()) || testData.Tables[sheetIndexToName[0]].Rows[i]["User Px"].ToString().Equals("0") || testData.Tables[sheetIndexToName[0]].Rows[i]["User Px"].ToString().Equals("0.0"))
                    {
                        testData.Tables[sheetIndexToName[0]].Rows[i].Delete();
                    }
                }

                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow = dtPricingInputs.Select(String.Format(@"" + TestDataConstants.COL_SYMBOL + "='{0}'", dataRow[TestDataConstants.COL_SYMBOL]));
                    if (foundRow.Length > 0)
                    {

                        int index = dtPricingInputs.Rows.IndexOf(foundRow[0]);
                        var gridRow = mssaObject.CachedChildren[0].CachedChildren[index + 1];
                        if (columToMSAAIndexMapping.Count == 0)
                        {
                            columToMSAAIndexMapping = gridRow.GetColumnIndexMaping(dtPricingInputs);
                        }
                        SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_USERPX, index);
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_USERVOL].ToString()) && dataRow[TestDataConstants.COL_USERVOL].ToString() != "0")
                        {
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_USERVOL, index);
                        }
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_USER_INTEREST_RATE].ToString()) && dataRow[TestDataConstants.COL_USER_INTEREST_RATE].ToString() != "0")
                        {
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_USER_INTEREST_RATE, index);
                        }
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_USER_DIVIDEND_YIELD].ToString()) && dataRow[TestDataConstants.COL_USER_DIVIDEND_YIELD].ToString() != "0")
                        {
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_USER_DIVIDEND_YIELD, index);
                        }
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_USER_SHARES_OUTSTANDING].ToString()) && dataRow[TestDataConstants.COL_USER_SHARES_OUTSTANDING].ToString() != "0")
                        {
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_USER_SHARES_OUTSTANDING, index);
                        }
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_FORWARDPOINTS].ToString()) && dataRow[TestDataConstants.COL_FORWARDPOINTS].ToString() != "0")
                        {
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_FORWARDPOINTS, index);
                        }
                        if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_SM_SHAREOUTSTANDING].ToString()) && dataRow[TestDataConstants.COL_SM_SHAREOUTSTANDING].ToString() != "0")
                        {
                            SetValueInGrid(columToMSAAIndexMapping, dataRow, gridRow, TestDataConstants.COL_SM_SHAREOUTSTANDING, index);
                        }
                    }
                }
                BtnSave.Click(MouseButtons.Left);
                //  Wait(2000);


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
                KeyboardUtilities.CloseWindow(ref OptionModelInputs_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }
        private void SetValueInGrid(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName, int index)
        {

            try
            {
                if (!String.IsNullOrEmpty(dataRow[columnName].ToString()))
                {
                    if (columToIndexMapping.ContainsKey(columnName))
                    {
                        int columnIndex = columToIndexMapping[columnName];
                        GrdOptionModel.Click(MouseButtons.Left);
                        GrdOptionModel.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                        GrdOptionModel.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, columnName);
                        Wait(1000);
                        if (!gridRow.CachedChildren[columnIndex - 1].IsVisible)
                        {
                            gridRow.CachedChildren[columnIndex - 2].Click(MouseButtons.Left);
                        }
                        else
                        {
                            gridRow.CachedChildren[columnIndex - 1].Click(MouseButtons.Left);
                        }
                        gridRow.CachedChildren[columnIndex].Click(MouseButtons.Left);
                        Keyboard.SendKeys(dataRow[columnName].ToString());
                        GrdOptionModel.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, "Symbol");
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

    }
}
