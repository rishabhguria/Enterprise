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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Watchlist
{
    class TradeFromWatchlist : WatchlistUIMap, ITestStep
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
                OpenWatchList();
                MaximizeWatchlist();
                //DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    InputEnter(dr);
                }
                
                  uiUltraGrid1.Click(MouseButtons.Right);


                if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ORDER_SIDE].ToString() == "Buy")
                {
                    bool iswindowVisible = false;
                      iswindowVisible = DataUtilities.pickFromMenuItem(PopupMenuDropDown, Buy.Name);
                    if(iswindowVisible ==false)
                     Buy.Click(MouseButtons.Left);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                else if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ORDER_SIDE].ToString() == "Adjust")
                {
                    //AdjustPosition.Click(MouseButtons.Left);
                    bool iswindowVisible = false;
                     iswindowVisible = DataUtilities.pickFromMenuItem(PopupMenuDropDown, AdjustPosition.Name);
                     if (iswindowVisible == false)
                         AdjustPosition.Click(MouseButtons.Left);
                   
                    if (PositionManagement.IsVisible)
                    {
                        ButtonYes.Click(MouseButtons.Left);
                    }
                }
                else
                    Sellshort.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "TradeFromWatchlist");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
               // Wait(3000);
                if (testData.Tables[0].Rows.Count <= 1 && testData.Tables[0].Rows.Count >0)
                {
                    WatchListMain.BringToFront();
                    CloseWatchlist();
                }
                else if (testData.Tables[0].Rows.Count > 1)
                {
                    Keyboard.SendKeys(KeyboardConstants.SPACE);
                }
            }
            return _result;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {
                uiUltraGrid1.Click(MouseButtons.Left);
                var msaaObj = uiUltraGrid1.MsaaObject;
                DataTable dtWatchList = CSVHelper.CSVAsDataTable(this.uiUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                string symbol = dr[TestDataConstants.COL_SYMBOL].ToString();
                DataRow[] foundRow = dtWatchList.Select(String.Format(@"[Symbol]='{0}'", symbol));
                int index = dtWatchList.Rows.IndexOf(foundRow[0]); 
                uiUltraGrid1.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[1].CachedChildren[index + 1].CachedChildren[0].Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
