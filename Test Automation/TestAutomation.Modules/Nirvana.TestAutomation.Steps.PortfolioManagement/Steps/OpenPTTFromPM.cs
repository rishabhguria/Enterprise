using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class OpenPTTFromPM : PortfolioManagementUIMap, ITestStep 
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
               // Wait(5000);
                Main.Click(MouseButtons.Left);
                DataTable dtPMGrid = CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var gridMssaObject = Main.MsaaObject;
                DataTable table = new DataTable();
                table.Clear();
                table.Columns.Add("Account");
                table.Columns.Add("MasterFund");
                table.Columns.Add("Symbol");
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                   
                    DataRow Dr = table.NewRow();
                    Dr["Account"] = row.ItemArray[0].ToString();
                    Dr["MasterFund"] = row.ItemArray[1].ToString();
                    Dr["Symbol"] = row.ItemArray[2].ToString();
                    
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtPMGrid), Dr);
                    int indexLong = dtPMGrid.Rows.IndexOf(dtRow);
                   
                    if(indexLong >= 0) 
                    {
                        Main.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexLong);                        
                        var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList", 3000);

                        Row.CachedChildren[indexLong + 1].Click(MouseButtons.Left);
                        Wait(1000);
                        Main.Click(MouseButtons.Right);
                        
                        if (!PopupMenuDropDown.IsVisible)
                        {
                            Main.Click(MouseButtons.Right);
                        }

                        if (row[TestDataConstants.Col_AdjustPosition].ToString() != String.Empty && row[TestDataConstants.Col_AdjustPosition].ToString().Equals("Increase", StringComparison.InvariantCultureIgnoreCase))
                        {
                            AdjustPosition.Click(MouseButtons.Left);
                            Wait(1000);
                            Increase.Click(MouseButtons.Left);
                        }
                        else if (row[TestDataConstants.Col_AdjustPosition].ToString() != String.Empty && row[TestDataConstants.Col_AdjustPosition].ToString().Equals("Decrease", StringComparison.InvariantCultureIgnoreCase))
                        {
                            AdjustPosition.Click(MouseButtons.Left);
                            Wait(1000);
                            Decrease.Click(MouseButtons.Left);
                        }
                        else if (row[TestDataConstants.Col_AdjustPosition].ToString() != String.Empty && row[TestDataConstants.Col_AdjustPosition].ToString().Equals("Set", StringComparison.InvariantCultureIgnoreCase))
                        {
                            AdjustPosition.Click(MouseButtons.Left);
                            Wait(1000);
                            Set.Click(MouseButtons.Left);
                        }                            
                        Wait(5000);
                        if (PositionManagement.IsVisible)
                        {
                            ButtonYes1.Click(MouseButtons.Left);
                        }
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
