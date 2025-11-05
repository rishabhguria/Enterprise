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
    public class CheckBlotter : BlotterUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
               // Wait(10000);
                List<String> errors = new List<String>();
                Boolean found = false;

                if (testData.Tables.Count > 1)
                {
                    if (testData.Tables[sheetIndexToName[1]].Rows[0]["BlotterGrid"].ToString().Equals("SubOrder"))
                    {
                        OrdersTab.Click(MouseButtons.Left);
                        DgBlotter1.Click(MouseButtons.Left);
                        found = SelectBlotterTrades(DgBlotter1, testData.Tables[sheetIndexToName[0]].Rows[0]);
                        DgBlotter2.Click(MouseButtons.Left);
                        errors = VerifyBlotter(testData, sheetIndexToName, DgBlotter2);
                    }
                }
                else
                {
                    if (testData.Tables[sheetIndexToName[0]].Rows[0]["BlotterGrid"].ToString().Equals("WorkingSubs"))
                    {
                        WorkingSubsTab.Click(MouseButtons.Left);
                        errors = VerifyBlotter(testData, sheetIndexToName, DgBlotter);

                    }
                    else if (testData.Tables[sheetIndexToName[0]].Rows[0]["BlotterGrid"].ToString().Equals("Order"))
                    {
                        OrdersTab.Click(MouseButtons.Left);
                        DgBlotter1.Click(MouseButtons.Left);
                        errors = VerifyBlotter(testData, sheetIndexToName, DgBlotter1);
                    }
                }

               // Wait(3000);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);
               

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
                KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
            }
            return _res;
        }

        /// <summary>
        /// Checks the blotter.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        private List<String> VerifyBlotter(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName,UIUltraGrid dgblotter)
        {
            try
            {
                DataTable subset = new DataTable();
                // get the trades from blotter grid
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(dgblotter.Properties[ExcelStructureConstants.COL_DESCRIPTION].ToString());
                if (testData.Tables.Count > 1)
                {
                     subset = testData.Tables[sheetIndexToName[1]];
                }
                else
                {
                     subset = testData.Tables[sheetIndexToName[0]];                
                }
                subset.Columns.Remove("BlotterGrid");
                List<String> columns = new List<String>();
                columns.Add(TestDataConstants.COL_SYMBOL);
                columns.Add(TestDataConstants.COL_ORDERSIDE);
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtBlotter, subset, columns, 0.01);
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