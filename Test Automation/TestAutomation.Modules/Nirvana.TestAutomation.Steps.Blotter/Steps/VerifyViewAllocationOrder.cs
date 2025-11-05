using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.IO;


namespace Nirvana.TestAutomation.Steps.Blotter
{
    class VerifyViewAllocationOrder : BlotterUIMap, ITestStep
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
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref StagedOrders_UltraFormManager_Dock_Area_Top);
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
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.ViewAllocation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].DoubleClick(MouseButtons.Right);
                    if (ViewAllocationDetails1.IsVisible)
                    {
                        ViewAllocationDetails1.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", ViewAllocationDetails1.MsaaName);
                    }
                }

                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdStagedOrder.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
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

    }
}
