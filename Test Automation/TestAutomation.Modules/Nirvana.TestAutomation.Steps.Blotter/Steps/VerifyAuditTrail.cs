using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
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
    public class VerifyAuditTrail : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();            
            try
            {
                OpenBlotter();
                OrdersTab.Click(MouseButtons.Left);
                List<String> errors = InputEnter(testData.Tables[0]);
                                
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyAuditTrail");
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
        private List<String> InputEnter(DataTable dTable)
        {
            try
            {                
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dTable.Rows[0]);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].DoubleClick(MouseButtons.Right);
                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Audit_Trail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].DoubleClick(MouseButtons.Right);
                    if (AuditTrail.IsVisible)
                    {
                        AuditTrail.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", AuditTrail.MsaaName);
                    }
                }
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdTicketDetails.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                superset = DataUtilities.RemoveCommas(superset);

                List<String> columns = new List<string>();

                List<String> errors = Recon.RunRecon(superset, dTable, columns, 0.01);
                return errors;
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
