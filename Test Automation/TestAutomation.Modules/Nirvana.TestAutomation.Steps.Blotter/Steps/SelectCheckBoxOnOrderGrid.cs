using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class SelectCheckBoxOnOrderGrid : BlotterUIMap, ITestStep
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
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SelectCheckBoxOnOrderGrid");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private void InputEnter(DataTable dt)
        {
            try
            {

                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                    int index = dtBlotter.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                            throw new Exception("Trade not found during SelectCheckBoxOnOrderGrid step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    DgBlotter1.InvokeMethod("ScrollToRow", index);
                    Wait(5000);
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].FindDescendantByName("",3000).Click(MouseButtons.Left);
                }
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
