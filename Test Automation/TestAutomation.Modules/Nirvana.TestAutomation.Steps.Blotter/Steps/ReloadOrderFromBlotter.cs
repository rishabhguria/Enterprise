using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Steps.Blotter;
using System.Linq;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class ReloadOrderFromBlotter : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                OrdersTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    _res.ErrorMessage = InputEnter(testData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ReloadOrderFromBlotter");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseBlotter();
            }
            return _res;
        }
        private string InputEnter(DataTable dTable)
        {
            string errorMessage = string.Empty;
            try
            {
                GetAllColumnsOnGrid(dTable);
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] dRows = DataUtilities.GetMatchingMultipleDataRows(DataUtilities.RemoveTrailingZeroes(dtBlotter), DataUtilities.RemoveTrailingZeroes(dTable), errorMessage);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                    return errorMessage;

                int index = 0;
                foreach (var row in dRows)
                {
                    index = dtBlotter.Rows.IndexOf(row);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dTable, new List<string>(), 0.01);
                            throw new Exception("Trade not found during ReloadOrderFromBlotter step. [Symbol= " + dTable.Rows[0]["Symbol"] + "], Quantity = [" + dTable.Rows[0]["Target Qty"] + "] Side = [" + dTable.Rows[0]["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    DgBlotter1.InvokeMethod("ScrollToRow", index);
                    // For checking the check box  used FindDescendantByName()
                    msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                }

                msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Right);

                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.ReloadOrder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (ReloadOrder.IsVisible)
                    {
                        ReloadOrder.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", ReloadOrder.MsaaName);
                    }
                }
                
                
                //ReloadOrder.Click(MouseButtons.Left);

                return errorMessage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetAllColumnsOnGrid(DataTable dTable)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dTable.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.DgBlotter1.InvokeMethod("AddColumnsToGrid", columns);
                SaveAllLayout.Click(MouseButtons.Left);
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
