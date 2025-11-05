using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public partial class RemoveManualExecution : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                PranaApplication.BringToFrontOnAttach = false;
              //  Wait(6000);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        RemoveManualExec(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "RemoveManualExecution");
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
        private void RemoveManualExec(DataRow dr)
        {
            try
            {
                //SubOrderBlotterGrid.Click(MouseButtons.Right);
                var msaaObj = DgBlotter2.MsaaObject;
                var maingrid = msaaObj;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter),dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during RemoveManualExecution step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Quantity"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter2.InvokeMethod("ScrollToRow", index);
                //Correct Right Click Action on SubOrder Grid
                var rowIndex = index + 1;
                //msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                try
                {
                    maingrid = msaaObj.FindDescendantByName("OrderBindingList row " + rowIndex, 4000);

                }
                catch
                {
                    maingrid = msaaObj.FindDescendantByName("OrderBindingList row 1", 4000);
                }
                maingrid.Click(MouseButtons.Right);
                if (PopupMenuContext.IsVisible)
                {
                    //RemoveManualExecution.Click(MouseButtons.Left);
                    RemoveExecution.Click(MouseButtons.Left);
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
