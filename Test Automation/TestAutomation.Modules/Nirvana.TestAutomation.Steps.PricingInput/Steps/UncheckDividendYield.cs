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
    public class UncheckDividendYield : PricingInputUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PRICING_INP"]);
                //Wait(5000);
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
                        string columnName = dataRow[TestDataConstants.COL_UncheckColumn].ToString();
                        UncheckColumn(columToMSAAIndexMapping, dataRow, gridRow, columnName, index);
                    }
                }
                BtnSave.Click(MouseButtons.Left);
               // Wait(2000);
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
        public void UncheckColumn(Dictionary<string, int> columToIndexMapping, DataRow dataRow, MsaaObject gridRow, string columnName, int index)
        {

            try
            {
                if (!String.IsNullOrEmpty(columnName))
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
