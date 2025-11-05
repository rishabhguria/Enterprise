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

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class VerifyCustomAllocationSecondUI: TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                ExtentionMethods.WaitForVisible(ref TradingTicket1, 20);
                if (TradingTicket1.IsVisible)
                {
                    if (testData.Tables[0].Rows.Count > 0)
                    {
                        BtnAccountQty.Click(MouseButtons.Left);
                        // Wait(6000);
                        if (StagedOrders_UltraFormManager_Dock_Area_Top.IsEnabled)
                        {
                            if (testData.Tables[0].Columns.Contains("IsDisabled"))
                            {
                                testData.Tables[0].Columns.Remove("IsDisabled");
                            }
                            List<String> errors = VerifyCustomAllocationGrid(testData.Tables[0]);
                            if (errors.Count > 0)
                                _result.ErrorMessage = String.Join("\n\r", errors);
                            else if (errors.Count == 0)
                            {
                                Console.WriteLine("Verification of Custom Allocation grid is passed");
                            }

                        }
                        else {
                            if (testData.Tables[0].Columns.Contains("IsDisabled") && !testData.Tables[0].Rows[0]["IsDisabled"].ToString().ToUpper().Equals("TRUE"))
                            {
                                throw new Exception("Custom Allocation grid is not visible");
                            }
                            else {
                                Console.WriteLine("Custom Allocation grid is disabled as per expectation");
                            }
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyCustomAllocationSecondUI");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            finally
            {
                //Wait(1000);
                try
                {
                    KeyboardUtilities.CloseWindow(ref StagedOrders_UltraFormManager_Dock_Area_Top);
                }
                catch { }
                CloseTradingTicket();
            }
            return _result;
        }

        private List<String> VerifyCustomAllocationGrid(DataTable dTable)
        {
            List<String> errors = new List<String>();
            // List<String> errors = Recon.RunRecon(TT, dTable, columns, 0.01);

            try
            {
                var gridMssaObject = GrdStagedOrder.MsaaObject;
                List<string> columns = new List<string>();
                ViewAllColumnsOnGrid(dTable);
                Dictionary<int, string> colToIndexMappingDictionary = new Dictionary<int, string>();
                DataTable dataTable = new DataTable();


                for (int i = 1; i < gridMssaObject.CachedChildren[0].CachedChildren[0].ChildCount; i++)
                {
                    dataTable.Columns.Add(gridMssaObject.CachedChildren[0].CachedChildren[0].CachedChildren[i].Name, typeof(string));
                }

                DataTable TT = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdStagedOrder.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                TT = DataUtilities.RemoveCommas(TT);




                errors = Recon.RunRecon(TT, dTable, columns, 0.01);
                return errors;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;

        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected void ViewAllColumnsOnGrid(DataTable dTable)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dTable.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.GrdStagedOrder.InvokeMethod("AddColumnsToGrid", columns);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }


    }

}
