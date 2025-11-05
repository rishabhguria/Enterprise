using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class EditAllocatedGrid : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                //Trade.Click(MouseButtons.Left);
                //Allocation2.Click(MouseButtons.Left);
                OpenAllocation();
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                Button2.Click();
                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                string error = EditGrid(testData, sheetIndexToName);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    _res.ErrorMessage = error;
                    _res.IsPassed = false;
                }
               // Wait(500);
                if (SavewDivideStatus.Bounds.X >= 0 && SavewDivideStatus.Bounds.Y >= 0)
                    SavewDivideStatus.Click(MouseButtons.Left);
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                    SavewDivideoStatus.Click(MouseButtons.Left);
                Button2.Click();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EditAllocatedGrid");
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
        /// Edits the grid.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        private string EditGrid(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;
            try
            {
                StringBuilder activityError = new StringBuilder(String.Empty);
                String path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                ITestDataProvider provider = Factory.TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(path + @"\" + ExcelStructureConstants.AllocatedTradesExportFileName);
                DataTable dtExportedTrades = testCases.Tables[0];
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                SortedDictionary<int, Tuple<string, bool>> columnDetails = new SortedDictionary<int, Tuple<string, bool>>();
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (!string.IsNullOrEmpty(row[col].ToString()))
                        {
                            if (!dtExportedTrades.Columns.Contains(col.ColumnName))
                            {
                                activityError.AppendLine("Column not found: " + col.ColumnName);
                            }
                            else
                            {
                                int index = dtExportedTrades.Columns.IndexOf(col.ColumnName);
                                columnDetails.Add(index, new Tuple<string, bool>(row[col].ToString(), col.ColumnName.StartsWith("Trade Attribute")));
                            }
                        }
                    }
                    EnterValuesInGrid(columnDetails);
                }
                errorMessage = activityError.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMessage;
        }

        /// <summary>
        /// Enters the values in grid.
        /// </summary>
        /// <param name="columnDetails">The indexes.</param>
        private void EnterValuesInGrid(SortedDictionary<int, Tuple<string, bool>> columnDetails)
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.HOMEKEY);
                int lastindex = 0;
                foreach (int index in columnDetails.Keys)
                {
                    EnterValueInGrid(index - lastindex, columnDetails[index].Item1, columnDetails[index].Item2);
                    lastindex = index;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}