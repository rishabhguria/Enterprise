using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class EditTradesUnallocated : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                Records = AllocationUIMap.GetLatestGridObject(this.GridUnallocated);
                Records.Click(MouseButtons.Left);
                GetAllocationDataBasedOnDateRange(testData, sheetIndexToName);
                DataTable dtUnallocationTest = ExportUnallocateTrades();
                SelectAndEditTrades(testData, dtUnallocationTest, sheetIndexToName);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                if (SavewDivideStatus.Bounds.X >=0 && SavewDivideStatus.Bounds.Y >= 0)
                    SavewDivideStatus.Click(MouseButtons.Left);
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                    SavewDivideoStatus.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALTF4);
                if (SaveChangesWindow.IsVisible)
                {
                    SavewDivideStatus2.Click(MouseButtons.Left);
                }
                // As the UI changes so closing allocation for now to reset UI
                //TODO: reset UI in the step itself
                CloseAllocation();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EditTradesUnallocated");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Edits the commission.
        /// </summary>
        /// <param name="dataRow">The dataRow</param>
        private void EditCommission(DataRow dataRow)
        {
            try
            {
                EditTrade.Click(MouseButtons.Left);
                Commission.DoubleClick(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(dataRow[TestDataConstants.COL_COMMISSION].ToString(), string.Empty, true);
                SoftCommission.DoubleClick(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(dataRow[TestDataConstants.COL_SOFTCOMMISSION].ToString(), string.Empty, true);
                Apply.DoubleClick(MouseButtons.Left);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Selects the and edit trades.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="dtUnallocationTest">The dt unallocation test.</param>
        private void SelectAndEditTrades(DataSet testData, DataTable dtUnallocationTest, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                EditTrade.Click(MouseButtons.Left);
                BasicDetailsArrow.Click(MouseButtons.Left);
                foreach (DataRow testRow in testData.Tables[sheetIndexToName[1]].Rows)
                {
                    DataRow[] dtrow = dtUnallocationTest.Select(String.Format(@"" + TestDataConstants.COL_SYMBOL + "='{0}' AND " + TestDataConstants.COL_QUANTITY + "='{1}' AND " + TestDataConstants.COL_SIDE + "='{2}'",
                               testRow[TestDataConstants.COL_SYMBOL], testRow[TestDataConstants.COL_QUANTITY], testRow[TestDataConstants.COL_SIDE]));
                    if (dtrow.Length > 0)
                    {
                        Records.Click(MouseButtons.Left);
                        KeyboardUtilities.PressKey(3, KeyboardConstants.HOMEKEY);
                        KeyboardUtilities.PressDownKeyWithWait(dtUnallocationTest.Rows.IndexOf(dtrow[0]));
                        DataRow[] dtEdtRows = testData.Tables[sheetIndexToName[2]].Select(String.Format(@"" + TestDataConstants.COL_TRADEID + "='{0}'", testRow[TestDataConstants.COL_TRADEID]));
                        EditCommission(dtEdtRows[0]);
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Exports the unallocate trades.
        /// </summary>
        /// <returns></returns>
        private DataTable ExportUnallocateTrades()
        {
            try
            {
                Records = AllocationUIMap.GetLatestGridObject(this.GridUnallocated);
                Records.Click(MouseButtons.Left);
                MouseController.RightClick();
                ExportData.Click(MouseButtons.Left);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                TextBoxFilename1.Click(MouseButtons.Left);
                KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(path + ExcelStructureConstants.UnAllocatedTradesExportFileName);
                if (ConfirmSaveAs4.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }
                ButtonNo.Click(MouseButtons.Left);

                ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.UnAllocatedTradesExportFileName);
                DataTable dtUnallocationTest = testCases.Tables[0];
                return dtUnallocationTest;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the allocation data based on date range.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void GetAllocationDataBasedOnDateRange(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                //If RangeType cell value is Blank or White Space then by default Current Radio button will be selected
                String rangetype = TestDataConstants.COL_CURRENT;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    if (!String.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_RANGETYPE].ToString()))
                        rangetype = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_RANGETYPE].ToString();
                }
                if (rangetype.Equals(TestDataConstants.COL_CURRENT, StringComparison.InvariantCultureIgnoreCase))
                {
                    Current.Click(MouseButtons.Left);
                }
                else
                {
                    Historical.Click(MouseButtons.Left);
                    String fromdate = string.Empty;
                    if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()))
                            fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()));
                        FromDate.Click(MouseButtons.Left);
                        ExtentionMethods.CheckCellValueConditions(fromdate, string.Empty, true);
                    }
                }
                GetData.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}