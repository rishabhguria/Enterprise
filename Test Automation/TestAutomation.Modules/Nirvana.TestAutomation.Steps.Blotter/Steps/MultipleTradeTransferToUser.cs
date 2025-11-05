using System.Linq;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class MultipleTradeTransferToUser : BlotterUIMap, ITestStep
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
                   InputEnter(testData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private string InputEnter(DataTable dTable)
        {
            string errorMessage = string.Empty;
            foreach (DataRow dr in dTable.Rows)
            {
                try
                {
                    var msaaObj = DgBlotter1.MsaaObject;
                    DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                    string userName = dr[TestDataConstants.COL_USERNAME].ToString();
                    //DataTable dTemp = new DataTable();
                    //dTemp = dTable.Clone();
                    dTable.Columns.Remove(TestDataConstants.COL_USERNAME);
                    DataRow[] dRows = DataUtilities.GetMatchingMultipleDataRows(DataUtilities.RemoveTrailingZeroes(dtBlotter), DataUtilities.RemoveTrailingZeroes(dTable), errorMessage);
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        return errorMessage;

                    int index = 0;
                    foreach (var row in dRows)
                    {
                        index = dtBlotter.Rows.IndexOf(row);
                        DgBlotter1.InvokeMethod("ScrollToRow", index);
                        // For checking the check box  used FindDescendantByName()
                        msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                    }

                    msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Right);
                    TransfertoUser.Click(MouseButtons.Left);
                    var msaa = PopupMenuTransfertoUser.MsaaObject;
                    msaa.Click(userName);
                    //Wait(2000);
                    if (Warning1.IsVisible)
                    {
                        if (ButtonYes2.IsVisible)
                        {
                            ButtonYes2.Click(MouseButtons.Left);
                        }
                    }
                    break;
                }

                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
            }
                return errorMessage;
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
