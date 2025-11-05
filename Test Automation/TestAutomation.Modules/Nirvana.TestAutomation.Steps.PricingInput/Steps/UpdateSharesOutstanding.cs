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
    public class UpdateSharesOutstanding : PricingInputUIMap, ITestStep
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
                    ExtentionMethods.WaitForVisible(ref PranaMain, 60);
                }
                //Shortcut to open pricing input module(CTRL + ALT + P)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_PRICING_INP"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref OptionModelInputs, 15);
                //Tools.Click(MouseButtons.Left);
                //PricingInputsMenuItem.Click(MouseButtons.Left);
                BtnRefresh.WaitForEnabled();
                All.Click(MouseButtons.Left);
                if (CheckBoxAll.IsChecked)
                {
                    CheckBoxAll.Click(MouseButtons.Left);
                }

                if (!CheckBoxSharesOutstanding.IsChecked)
                    CheckBoxSharesOutstanding.Click(MouseButtons.Left);

                DataTable dtPricingInputs = CSVHelper.CSVAsDataTable(this.GrdOptionModel.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var mssaObject = GrdOptionModel.MsaaObject;
                Dictionary<string, int> columToIndexMapping = new Dictionary<string, int>();
                int counter = 0;
                foreach (DataColumn col in dtPricingInputs.Columns)
                {
                    columToIndexMapping.Add(col.ColumnName, counter);
                    counter++;
                }

                foreach (DataRow dataRow in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow[] foundRow = dtPricingInputs.Select(String.Format(@"" + TestDataConstants.COL_SYMBOL + "='{0}'", dataRow[TestDataConstants.COL_SYMBOL]));
                    if (foundRow.Length > 0)
                    {
                        int index = dtPricingInputs.Rows.IndexOf(foundRow[0]);
                        var gridRow = mssaObject.CachedChildren[0].CachedChildren[index + 1];
                        GrdOptionModel.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMN, columToIndexMapping[TestDataConstants.COL_SHARES_OUT_STANDING]);
                        gridRow.CachedChildren[31].Click(MouseButtons.Left);
                        gridRow.CachedChildren[32].Click(MouseButtons.Left);
                        ExtentionMethods.CheckCellValueConditions(dataRow[TestDataConstants.COL_USER_SHARES_OUTSTANDING].ToString(), String.Empty, false);
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
    }
}