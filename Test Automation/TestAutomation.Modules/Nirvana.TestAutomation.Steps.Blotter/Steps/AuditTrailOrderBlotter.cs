using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class AuditTrailOrderBlotter : BlotterUIMap, ITestStep
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
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "AuditTrailOrderBlotter");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during AuditTrailOrderBlotter step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Quantity"] + "] Side = [" + dr["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter1.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                AuditTrail.Click(MouseButtons.Left);
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
