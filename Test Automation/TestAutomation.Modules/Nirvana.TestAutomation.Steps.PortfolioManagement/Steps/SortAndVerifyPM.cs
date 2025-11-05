using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core.UIAutomationSupport;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public class SortAndVerifyPM : PortfolioManagementUIMap, ITestStep
    {
           public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
               // Wait(9000);

                List<String> errors = SortAndCheckPM(testData, sheetIndexToName);
                Main.Click(MouseButtons.Left);
                if (errors.Count > 0)
                {
                    _result.IsPassed = false;
                    _result.ErrorMessage = String.Join(", ", errors.ToArray());
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
                KeyboardUtilities.MinimizeWindow(ref PM_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// Checks the pm.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
           private List<string> SortAndCheckPM(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                string sortingASCorDSC = string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_SORT_ASC_DSC].ToString())? "ASC":testData.Tables[0].Rows[0][TestDataConstants.COL_SORT_ASC_DSC].ToString().ToUpper();
               string sortingColumnName =  string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_SORTINGCOLUMNNAME].ToString())? "Symbol":testData.Tables[0].Rows[0][TestDataConstants.COL_SORTINGCOLUMNNAME].ToString();
                var gridMssaObject = Main.MsaaObject;
                Main.WaitForRespondingOrExited();
                Main.Click(MouseButtons.Left);
                AutomationElementWrapper wrapper = new AutomationElementWrapper(Main.MsaaObject.Children[1].WindowHandle);
                WaitOnItems(wrapper);
                Wait(4000);
                DataTable superset2 = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                List<String> errors = new List<String>();
                columns.Add("Symbol");
                //columns.Add("Order Side");
                subset.Columns.Remove(TestDataConstants.COL_SORT_ASC_DSC);
                subset.Columns.Remove(TestDataConstants.COL_SORTINGCOLUMNNAME);
               
                Sorting(Main, sortingASCorDSC,sortingColumnName,superset2);

                //Wait(6000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
               
                for (int i = 0; i < subset.Rows.Count; i++) // the number of rows in ui and excel should be same
                {
                    DataRow dr = subset.Rows[i];
                    DataRow drMain = superset.Rows[i];
                    bool isDataRight = AreDataRowsEqual(dr, drMain);
                    if (isDataRight == false)
                    {
                        throw new Exception(i +"th DATA  DIDNOT MATCH INDEX WISE");
                    }
                    else
                        Console.WriteLine(i + "th row in PM grid contains correct info");

                }

                    errors = Recon.RunRecon(superset, subset, columns, 0.0000000001);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void WaitOnItems(AutomationElementWrapper wrapper)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();

                while (wrapper.Children.Count <= 1)
                {
                    if (tmr.ElapsedMilliseconds >= 30000)
                    {
                        break;
                    }
                }
                tmr.Stop();
            }
            catch (Exception)
            {                
                throw;
            }
        }
        public static void Sorting(UIUltraGrid Main, string sortingASCorDSC, string sortingColumnName, DataTable superset)
        {
            try
            {
                var gridMssaObject = Main.MsaaObject;
                var columnNameMsaa = gridMssaObject.FindDescendantByName("Column Headers", 3000);
                var columnNameMsaa2 = columnNameMsaa.FindDescendantByName(sortingColumnName, 3000);
                var index = superset.Columns[sortingColumnName];
                Dictionary<string, int> columnToIndexMapping = GetColumnIndexMapings(columnNameMsaa, superset);

               // Wait(6000);

                if (index != null)
                {
                    int colIndex = columnToIndexMapping[sortingColumnName];
                    Main.InvokeMethod(TestDataConstants.COL_SCROLLTOCOLUMNNAME, sortingColumnName);
                    Wait(5000);
                }

                if (sortingASCorDSC.Equals("ASC"))
                    columnNameMsaa2.Click();

                else
                {
                    columnNameMsaa2.Click();
                    columnNameMsaa2.Click();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message+ "->"+ " sorting issue");
            }

        }

        public bool AreDataRowsEqual(DataRow row1, DataRow row2)
        {
            try
            {
                // if (row1.Table.Columns.Count != row2.Table.Columns.Count)
                //   return false;


                for (int i = 0; i < row1.Table.Columns.Count; i++)
                {
                    var column1 = row1.Table.Columns[i];


                    if (!row2.Table.Columns.Contains(column1.ColumnName))
                        continue;

                    var column2 = row2.Table.Columns[column1.ColumnName];

                    if (!string.IsNullOrEmpty(row1[column1].ToString()))
                    {
                        if (!Equals(row1[column1], row2[column2]))
                            return false;
                    }
                }


                return true;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message + " row not found " + row1); }
        }
        public static Dictionary<string, int> GetColumnIndexMapings(MsaaObject msaa, DataTable gridTable)
        {
            Dictionary<string, int> columnToIndexMapping = new Dictionary<string, int>();
            try
            {
                for (int index = 0; index < msaa.CachedChildren.Count; index++)
                {
                    string temp = msaa.CachedChildren[index].Name.Trim();
                    if (gridTable.Columns.Contains(temp))
                    {
                        if (!columnToIndexMapping.ContainsKey(temp))
                        {
                            columnToIndexMapping.Add(temp, index);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Get column index mapping failed :" + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return columnToIndexMapping;
        }
    }

    
}
