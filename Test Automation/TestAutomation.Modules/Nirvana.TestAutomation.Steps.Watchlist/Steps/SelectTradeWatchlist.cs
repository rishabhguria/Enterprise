using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Watchlist
{
    public class SelectTradeWatchlist : WatchlistUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenWatchList();
                MaximizeWatchlist();
              
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AddSymbolToWatchlist");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            
            return _result;
        }

        private void InputEnter(DataRow dr)
        {
            try
            {
               
                string checkBoxValue = "";
                if (dr.Table.Columns.Contains(TestDataConstants.CheckBox) && !string.IsNullOrEmpty(dr[TestDataConstants.CheckBox].ToString()))
                {
                    checkBoxValue = dr[TestDataConstants.CheckBox].ToString();
                    dr[TestDataConstants.CheckBox] = "";

                }
                uiUltraGrid1.Click(MouseButtons.Left);
                var msaaObj = uiUltraGrid1.MsaaObject;
                DataTable dtWatchList = CSVHelper.CSVAsDataTable(this.uiUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                string symbol = dr[TestDataConstants.COL_SYMBOL].ToString();
                DataRow[] foundRow = dtWatchList.Select(String.Format(@"[Symbol]='{0}'", symbol));
                int index = dtWatchList.Rows.IndexOf(foundRow[0]);
                uiUltraGrid1.InvokeMethod("ScrollToRow", index);

                if (string.Equals(checkBoxValue, "ToggleState_On", StringComparison.OrdinalIgnoreCase) || string.Equals(checkBoxValue, "true", StringComparison.OrdinalIgnoreCase))
                {

                    MsaaObject checkBox = null;
                    try
                    {
                        checkBox = msaaObj.CachedChildren[1].CachedChildren[index + 1].CachedChildren[0];
                    }
                    catch (Exception)
                    {
                        checkBox = msaaObj.CachedChildren[0].CachedChildren[1 + index].CachedChildren[0];
                    }

                    if (checkBox != null)
                    {
                        string uicheckBoxValue = checkBox.Value.ToString();
                        if (string.Equals(uicheckBoxValue, "False", StringComparison.OrdinalIgnoreCase))
                        {
                            checkBox.Click(MouseButtons.Left);
                        }
                    }
                }
                else
                {
                 
                    try
                    {
                         msaaObj.CachedChildren[1].CachedChildren[index + 1].CachedChildren[1].Click(MouseButtons.Left); ;
                    }
                    catch (Exception)
                    {
                         msaaObj.CachedChildren[0].CachedChildren[1 + index].CachedChildren[1].Click(MouseButtons.Left); ;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
