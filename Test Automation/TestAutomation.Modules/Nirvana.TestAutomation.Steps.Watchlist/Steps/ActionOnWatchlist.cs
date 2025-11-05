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
    class ActionOnWatchlist : WatchlistUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
               

                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        string columnName = col.ColumnName.ToString();
                        if (columnName.Contains("MenuItem"))
                        {
                            if (!string.IsNullOrEmpty(dr[columnName].ToString()))
                            {
                                uiUltraGrid1.Click(MouseButtons.Right);
                                try
                                {
                                    bool iswindowVisible = DataUtilities.pickFromMenuItem(PopupMenuDropDown, dr[columnName].ToString());
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                }

                
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
               
                CloseWatchlist();
            }
            return _result;
        }
        
    }
}
