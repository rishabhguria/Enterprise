using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class ClosePositionFromPM : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">test data.</param>
        /// <param name="sheetIndexToName">sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
                Wait(5000);
                Main.Click(MouseButtons.Left);                
                DataTable dtPMGrid = CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var gridMssaObject = Main.MsaaObject;
                DataRow dr = testData.Tables[0].Rows[0];

                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtPMGrid), row);
                    int indexLong = dtPMGrid.Rows.IndexOf(dtRow);
                    indexLong = indexLong + 1;
                    if (indexLong >= 0)
                    {
                        Main.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexLong);
                        try
                        {
                            var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList row " + indexLong, 3000);
                            Wait(1000);
                            Row.Click(MouseButtons.Left);
                            Wait(1000);
                            Row.Click(MouseButtons.Right);
                        }
                        catch
                        {
                            // retry added to get correct row before throwing the error
                            try
                            {
                                gridMssaObject = Main.MsaaObject;
                                var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList row " + indexLong, 3000);
                                Row.Click(MouseButtons.Left);
                                Main.Click(MouseButtons.Right);
                            }
                            catch
                            {
                                Console.WriteLine("data sheet row not found on UI");
                                throw;
                            }
                        }

                        if (!PopupMenuDropDown.IsVisible)
                        {
                            var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList row " + indexLong, 3000);
                            Row.Click(MouseButtons.Left);
                            Main.Click(MouseButtons.Right);
                        }
                        if (dr.Table.Columns.Contains(TestDataConstants.COL_ORDER_SIDE))
                        {
                            if (dr[TestDataConstants.COL_ORDER_SIDE].ToString().Equals("Sell") || dr[TestDataConstants.COL_ORDER_SIDE].ToString().Equals("Sell to Close") || dr[TestDataConstants.COL_ORDER_SIDE].ToString().Equals("Buy to Close"))
                            {
                                if (ExitPosition.IsEnabled)
                                {
                                    Console.WriteLine("Button is Enabled");
                                    _result.IsPassed = false;
                                }
                                break;
                            }

                        }

                        Wait(2000);
                        bool flag = DataUtilities.pickFromMenuItem(PopupMenuDropDown, "Exit Position");
                        break;
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
            finally
            {
                PM.BringToFront();
                PMclose();
            }
            return _result;
        } 
    }
}
