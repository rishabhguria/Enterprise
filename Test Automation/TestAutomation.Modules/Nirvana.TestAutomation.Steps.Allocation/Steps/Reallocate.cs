using System.Diagnostics;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class Reallocate : AllocationUIMap, ITestStep
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
                Records = AllocationUIMap.GetLatestGridObject(this.GridAllocated);
                Records.Click(MouseButtons.Left);
                GetAllocationDataBasedOnDateRange(testData, sheetIndexToName);

                DataTable dtAllocatedTrades = ExportTrades();
                List<int> index = new List<int>();

                if (testData != null)
                {
                    foreach (DataRow testRow in testData.Tables[sheetIndexToName[1]].Rows)
                    {
                        DataRow[] dtrow = dtAllocatedTrades.Select(String.Format(@"" + TestDataConstants.COL_SYMBOL + "='{0}' AND " + TestDataConstants.COL_QUANTITY + "='{1}' AND " + TestDataConstants.COL_SIDE + "='{2}'",
                            testRow[TestDataConstants.COL_SYMBOL], testRow[TestDataConstants.COL_QUANTITY], testRow[TestDataConstants.COL_SIDE]));
                        if (dtrow.Length > 0)
                        {
                            Records.Click(MouseButtons.Left);
                            index.Add(dtAllocatedTrades.Rows.IndexOf(dtrow[0]));
                        }
                    }
                }
                index.Sort();
                int oldindex = 0;
                KeyboardUtilities.PressKey(3, KeyboardConstants.HOMEKEY);
                foreach (var tradeno in index)
                {
                    KeyboardUtilities.PressDownKeyWithWait(tradeno - oldindex);
                    Keyboard.SendKeys(KeyboardConstants.F1KEY);
                    oldindex = tradeno;
                }

              //  Wait(1000);
                Wait(2000);
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
               // Wait(1000);
                String PrefName = String.Empty;
                bool isPttPrefNeeded = false;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    PrefName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE].ToString();

                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.COL_PTTPREFNEEDED].ToString()))
                    {
                        isPttPrefNeeded = bool.Parse(testData.Tables[0].Rows[0][TestDataConstants.COL_PTTPREFNEEDED].ToString());
                    }
                }
                if (!String.IsNullOrEmpty(PrefName))
                {
                    AllocateByPreference(PrefName, isPttPrefNeeded);
                }
                else
                {
                    AllocateManually(testData, sheetIndexToName);
                }
                btnAllocate.DoubleClick(MouseButtons.Left); 
                if (SavewDivideStatus.Bounds.X >= 0 && SavewDivideStatus.Bounds.Y >= 0)
                    SavewDivideStatus.Click(MouseButtons.Left);
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                    SavewDivideoStatus.Click(MouseButtons.Left);
              //  Wait(3000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Reallocate");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally 
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Allocates the manually.
        /// </summary>
        /// <param name="testData">The test data.</param>
        private void AllocateManually(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                PinnedIcon.Click(MouseButtons.Left);
                AccountStartegyGrid1.AutomationElementWrapper.CachedChildren[0].CachedChildren[0].CachedChildren[0].WpfClick();
                foreach (DataRow row in testData.Tables[sheetIndexToName[2]].Rows)
                {
                   AllocateTrades(row);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Allocates the by preference.
        /// </summary>
        /// <param name="PrefName">Name of the preference.</param>
        /// /// <param name="isPstPrefNeeded">Is PST preference required.</param>
        private void AllocateByPreference(String PrefName,bool isPttPrefNeeded)
        {
            try
            {
                ToggleButton12.Click(MouseButtons.Left);
               // Wait(3000);
                Dictionary<string, int> dictprefToColumnIndexMapping = new Dictionary<string, int>();

                for (int i = 0; i < XamComboEditor38.AutomationElementWrapper.CachedChildren.Count; i++)
                {
                    dictprefToColumnIndexMapping.Add(
                        XamComboEditor38.AutomationElementWrapper.CachedChildren[i].CachedChildren[0].Name, i);
                }
                XamComboEditor38.AutomationElementWrapper.CachedChildren[dictprefToColumnIndexMapping[PrefName]].CachedChildren[0].WpfClick();
                if (isPttPrefNeeded)
                {
                    if (!Custom.IsChecked)
                    {
                        Custom.Click(MouseButtons.Left);
            }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Exports the trades.
        /// </summary>
        /// <returns></returns>
        private DataTable ExportTrades()
        {
            try
            {
                Records = AllocationUIMap.GetLatestGridObject(this.GridAllocated);
                Records.Click(MouseButtons.Left);
                MouseController.RightClick();
                ExportData.Click(MouseButtons.Left);
                if (Groups.IsVisible)
                {
                    Groups.Click(MouseButtons.Left);
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ @"\";
                KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                clearText(TextBoxFilename);
                TextBoxFilename.Click(MouseButtons.Left);
                TextBoxFilename.Click(MouseButtons.Left);
                KeyboardUtilities.PressKey(2, KeyboardConstants.BACKSPACEKEY);
                Keyboard.SendKeys(path+ExcelStructureConstants.AllocatedTradesExportFileName);
                ButtonSave2.Click(MouseButtons.Left);
                if (ConfirmSaveAs4.IsVisible)
                {
                    ButtonYes1.Click(MouseButtons.Left);
                }
                ButtonNo.Click(MouseButtons.Left);

                ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.AllocatedTradesExportFileName);
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