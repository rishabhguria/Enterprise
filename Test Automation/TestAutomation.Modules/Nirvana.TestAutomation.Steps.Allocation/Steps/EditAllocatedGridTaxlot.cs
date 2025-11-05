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
    internal class EditAllocatedGridTaxlot : AllocationUIMap, ITestStep
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
                    Dictionary<string, int> taxlotColumnMapping = new Dictionary<string, int>();
                    AutomationElement[] x = aeGrid.Current.GetColumnHeaders();
                    for (int i = 0; i < x.Length; i++)
                    {
                        gridColumnMapping.Add(x[i].Current.Name, i);
                    }

                    if (gridColumnMapping.ContainsKey(TestDataConstants.COL_SYMBOL) && gridColumnMapping.ContainsKey(TestDataConstants.COL_QUANTITY) && gridColumnMapping.ContainsKey(TestDataConstants.COL_AVERAGE_PRICE_BASE))
                    {
                        DataTable dt = testData.Tables[sheetIndexToName[0]];
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
                            TablePattern taxGrid;
                            errorMessage = FindTaxlotGrid(aeGrid, columnValues, gridColumnMapping, out taxGrid);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                _res.ErrorMessage = errorMessage;
                                _res.IsPassed = false;
                            }
                            else
                            {
                                if (taxlotColumnMapping == null || taxlotColumnMapping.Count == 0)
                                {
                                    taxlotColumnMapping = new Dictionary<string, int>();
                                    AutomationElement[] taxcol = taxGrid.Current.GetColumnHeaders();
                                    for (int i = 0; i < taxcol.Length; i++)
                                    {
                                        taxlotColumnMapping.Add(taxcol[i].Current.Name, i);
                                    }
                                }
                                if (taxlotColumnMapping.ContainsKey(TestDataConstants.COL_ACCOUNT_NAME))
                                {
                                    errorMessage = EnterValuesInGrid(taxGrid, columnValues, taxlotColumnMapping);
                                    if (!string.IsNullOrEmpty(errorMessage))
                                    {
                                        _res.ErrorMessage = errorMessage;
                                        _res.IsPassed = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _res.ErrorMessage = "Identifier columns(Symbol, Quantity, Avg price(Base)) not found on Grid";
                        _res.IsPassed = false;
                    }
                   // Wait(500);
                    Button2.Click();
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EditAllocatedGridTaxlot");
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
        /// Finds the grid.
        /// </summary>
        /// <param name="aeGrid">The grid to find.</param>
        /// <returns></returns>
        private string FindTaxlotGrid(TablePattern aeGrid, Dictionary<string, string> columnValues, Dictionary<string, int> gridColumnMapping, out TablePattern taxGrid)
        {
            taxGrid = null;
            string errorMsg = "Group not found in Allocated Grid";
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
                    Wait(100);
                    if (symPat.Current.Value.Equals(columnValues[TestDataConstants.COL_SYMBOL]))
                        Wait(1000);
                    {
                        ValuePattern qPat = (ValuePattern)aeGrid.GetItem(i, quantityIndex).GetCurrentPattern(ValuePattern.Pattern);
                        Wait(100);
                        if (qPat.Current.Value.Equals(columnValues[TestDataConstants.COL_QUANTITY]))
                            Wait(1000);
                        {
                            ValuePattern avgPat = (ValuePattern)aeGrid.GetItem(i, priceBaseIndex).GetCurrentPattern(ValuePattern.Pattern);
                            Wait(100);
                            double actualValue = Math.Round(Convert.ToDouble(avgPat.Current.Value), 3);
                            double expectedValue = Math.Round(Convert.ToDouble(columnValues[TestDataConstants.COL_AVERAGE_PRICE_BASE]), 3);

                            // Compare rounded values
                            if (actualValue == expectedValue)
                            {
                                break;
                            }
                        }
                    }
                }
                if (i < rowCount)
                {
                    AutomationElement cell = aeGrid.GetItem(i, 1);
                    AutomationElement row = TreeWalker.ControlViewWalker.GetParent(cell);
                    AutomationElementCollection rowChilds = row.FindAll(TreeScope.Descendants, Condition.TrueCondition);
                    foreach (AutomationElement child in rowChilds)
                    {
                        if (child.Current.Name.Equals("TaxLots"))
                        {
                            AutomationElement taxAE = child.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Records"));
                            taxGrid = (TablePattern)taxAE.GetCurrentPattern(TablePattern.Pattern);
                            errorMsg = string.Empty;
                            break;
                        }
                    }
                    i++;
                }

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
        /// Enters the values in grid.
        /// </summary>
        /// <param name="columnValues">The indexes.</param>
        private string EnterValuesInGrid(TablePattern taxGrid, Dictionary<string, string> columnValues, Dictionary<string, int> taxlotColumnMapping)
        {
            string errorMsg = string.Empty;
            try
            {
                int accountNameIndex = taxlotColumnMapping[TestDataConstants.COL_ACCOUNT_NAME];

                int rowCount = taxGrid.Current.RowCount;
                int i;
                for (i = 0; i < rowCount; i++)
                {
                    ValuePattern accPat = (ValuePattern)taxGrid.GetItem(i, accountNameIndex).GetCurrentPattern(ValuePattern.Pattern);
                    if (accPat.Current.Value.Equals(columnValues[TestDataConstants.COL_ACCOUNT_NAME]))
                    {
                        break;
                    }
                }
                if (i < rowCount)
                {
                    foreach (string columnName in columnValues.Keys)
                    {
                        if (!(columnName.Equals(TestDataConstants.COL_SYMBOL) || columnName.Equals(TestDataConstants.COL_QUANTITY) || columnName.Equals(TestDataConstants.COL_AVERAGE_PRICE_BASE) || columnName.Equals(TestDataConstants.COL_ACCOUNT_NAME)))
                        {
                            ValuePattern vp =
                            (ValuePattern)taxGrid.GetItem(i, taxlotColumnMapping[columnName]).GetCurrentPattern(ValuePattern.Pattern);
                            vp.SetValue(columnValues[columnName]);
                        }
                    }
                }
                else
                    errorMsg = "Taxlot not found in Allocated Grid";

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
