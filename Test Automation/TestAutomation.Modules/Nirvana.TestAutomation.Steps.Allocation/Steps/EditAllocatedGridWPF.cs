using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using System.Threading;
using System.Windows.Automation;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class EditAllocatedGridWPF : AllocationUIMap, ITestStep
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
                OpenAllocation();
                //Trade.Click(MouseButtons.Left);
                //Allocation2.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                Button2.Click();
                TablePattern aeGrid;
                string errorMessage = FindGrid(out aeGrid);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _res.ErrorMessage = errorMessage;
                    _res.IsPassed = false;
                }
                else
                {
                    Dictionary<string, int> gridColumnMapping = new Dictionary<string, int>();
                    AutomationElement[] x = aeGrid.Current.GetColumnHeaders();
                    for (int i = 0; i < x.Length; i++)
                    {
                        gridColumnMapping.Add(x[i].Current.Name, i);
                    }

                    if (gridColumnMapping.ContainsKey(TestDataConstants.COL_SYMBOL) && gridColumnMapping.ContainsKey(TestDataConstants.COL_QUANTITY) && gridColumnMapping.ContainsKey(TestDataConstants.COL_AVERAGE_PRICE_BASE))
                    {
                        DataTable dt = testData.Tables[sheetIndexToName[0]];
                        List<String> columns = new List<String>();
                        try
                        {
                            string StepName = "EditAllocatedGridWPF";
                            DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                            Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref dt);
                            SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref dt);
                        }
                        catch (Exception ex)
                        { Console.WriteLine(ex.Message); }
                        foreach (DataRow row in dt.Rows)
                        {
                            Dictionary<string, string> columnValues = new Dictionary<string, string>();
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                if (!string.IsNullOrEmpty(row[col].ToString()))
                                {
                                    columnValues[col.ColumnName] = row[col].ToString();
                                }
                            }
                            errorMessage = EnterValuesInGrid(aeGrid, columnValues, gridColumnMapping);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                _res.ErrorMessage = errorMessage;
                                _res.IsPassed = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        _res.ErrorMessage = "Identifier columns(Symbol, Quantity, Avg price(Base)) not found on Grid";
                        _res.IsPassed = false;
                    }
                    Wait(500);
                    Button2.Click();
                }
                if (NirvanaAllocation.IsVisible)
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        if (dr.Table.Columns.Contains(TestDataConstants.COL_Recalculate_Comm_PopUp))
                        {
                            if (dr[TestDataConstants.COL_Recalculate_Comm_PopUp].Equals("Yes"))
                            {
                                ButtonYes.Click(MouseButtons.Left);
                            }
                            else
                            {
                                ButtonNo.Click(MouseButtons.Left);
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EditAllocatedGridWPF");
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
        /// Finds the grid.
        /// </summary>
        /// <param name="aeGrid">The grid to find.</param>
        /// <returns></returns>
        private string FindGrid(out TablePattern aeGrid)
        {
            aeGrid = null;
            string errorMessage = "Could not find the Allocated grid";
            try
            {
                AutomationElement aeDesktop = AutomationElement.RootElement;
                if (aeDesktop != null)
                {
                    AutomationElement aePrana = null;
                    int numWaits1 = 0;
                    do
                    {
                        aePrana = aeDesktop.FindFirst(TreeScope.Children,
                            new PropertyCondition(AutomationElement.NameProperty, "Nirvana"));
                        ++numWaits1;
                        Thread.Sleep(200);
                    }
                    while (aePrana == null && numWaits1 < 50);

                    if (aePrana != null)
                    {
                        AutomationElement aeAlGrid = aePrana.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, "GridAllocated"));

                        if (aeAlGrid != null)
                        {
                            AutomationElement aeGridrec = aeAlGrid.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Records"));
                            if (aeGridrec != null)
                            {
                                aeGrid = (TablePattern)aeGridrec.GetCurrentPattern(TablePattern.Pattern);
                                errorMessage = string.Empty;
                            }
                        }
                    }
                }
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
        /// <param name="columnValues">The indexes.</param>
        private string EnterValuesInGrid(TablePattern aeGrid, Dictionary<string, string> columnValues, Dictionary<string, int> gridColumnMapping)
        {
            string errorMsg = string.Empty;
            try
            {
                int symbolIndex = gridColumnMapping[TestDataConstants.COL_SYMBOL];
                int quantityIndex = gridColumnMapping[TestDataConstants.COL_QUANTITY];
                int priceBaseIndex = gridColumnMapping[TestDataConstants.COL_AVERAGE_PRICE_BASE];

                int rowCount = aeGrid.Current.RowCount;
                int i;
                for (i = 0; i < rowCount; i++)
                {
                    ValuePattern symPat = (ValuePattern)aeGrid.GetItem(i, symbolIndex).GetCurrentPattern(ValuePattern.Pattern);
                    if (symPat.Current.Value.Equals(columnValues[TestDataConstants.COL_SYMBOL]))
                    {
                        ValuePattern qPat = (ValuePattern)aeGrid.GetItem(i, quantityIndex).GetCurrentPattern(ValuePattern.Pattern);
                        if (qPat.Current.Value.Equals(columnValues[TestDataConstants.COL_QUANTITY]))
                            Wait(5000);
                        {
                            ValuePattern avgPat = (ValuePattern)aeGrid.GetItem(i, priceBaseIndex).GetCurrentPattern(ValuePattern.Pattern);
                            Wait(2000);
                            if (avgPat.Current.Value.Equals(columnValues[TestDataConstants.COL_AVERAGE_PRICE_BASE]))
                            {
                                break;
                            }
                        }
                    }
                }
                if (i < rowCount)
                {
                    foreach (string columnName in columnValues.Keys)
                    {
                        if (!(columnName.Equals(TestDataConstants.COL_SYMBOL) || columnName.Equals(TestDataConstants.COL_QUANTITY) || columnName.Equals(TestDataConstants.COL_AVERAGE_PRICE_BASE)))
                        {
                            if(columnName.Equals("RecalculateCommPopUp"))
                            {
                                break;
                            }
                            ValuePattern vp =
                            (ValuePattern)aeGrid.GetItem(i, gridColumnMapping[columnName]).GetCurrentPattern(ValuePattern.Pattern);
                            vp.SetValue(columnValues[columnName]);
                            //Need custom handling for Trade Attributes as they allow adding values at runtime in the dropdown list itself 
                            if (columnName.StartsWith("Trade Attribute"))
                            {
                                Keyboard.SendKeys(KeyboardConstants.TABKEY);
                                Keyboard.SendKeys(KeyboardConstants.CTRLA);
                                Keyboard.SendKeys(columnValues[columnName]);
                                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                            }
                        }
                    }
                }
                else
                    errorMsg = "Row not found in Allocated Grid";

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errorMsg;
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
