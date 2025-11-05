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
    public class RemoveSelectedOrders : BlotterUIMap, ITestStep
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
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "RemoveSelectedOrders");
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
            try
            {
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow[] dRows = DataUtilities.GetMatchingMultipleDataRows(DataUtilities.RemoveTrailingZeroes(dtBlotter), DataUtilities.RemoveTrailingZeroes(dTable),errorMessage);
                int index = 0;
                foreach (var row in dRows)
                {
                    index = dtBlotter.Rows.IndexOf(row);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dTable, new List<string>(), 0.01);
                            throw new Exception("Trade not found during RemoveSelectedOrders step. [Symbol= " + row["Symbol"] + "], Quantity = [" + row["Target Qty"] + "] Side = [" + row["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    DgBlotter1.InvokeMethod("ScrollToRow", index);
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].CachedChildren[3].Click(MouseButtons.Left);
                }
                RemoveOrders.Click(MouseButtons.Left);
                ButtonYes.Click(MouseButtons.Left);
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
