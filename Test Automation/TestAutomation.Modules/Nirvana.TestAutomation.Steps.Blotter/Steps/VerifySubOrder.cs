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
    public class VerifySubOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                List<String> columns = new List<String>();
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                try
                {
                    string StepName = "VerifySubOrder";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception)
                { }
                List<String> errors = InputEnter(testData.Tables[0]);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifySubOrder");
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

           protected List<String> InputEnter(DataTable dTable)
        {
            try
            {
                ViewAllColumnsOnGrid(dTable);
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtBlotter = DataUtilities.RemoveCommas(dtBlotter);
                List<String> columns = new List<string>();
                /*columns = (from DataColumn x in dTable.Columns
                select x.ColumnName).ToList();*/
                DataRow drow = null;
                if (dTable.Rows.Count > 0)
                {
                    drow = dTable.Rows[0];
                }
                // If Table contains mandatory columns then verify mandatory columns
                if (dTable.Columns.Contains("MandatoryColumn"))
                {

                    if (!String.IsNullOrEmpty(drow["MandatoryColumn"].ToString()))
                    {
                        columns = MandatoryColumns.SubOrder();
                    }
                    else
                    {
                        columns.Add("Symbol");
                    }
                    dTable.Columns.Remove("MandatoryColumn");
                }
                else
                {
                    columns.Add("Symbol");
                }

                if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                {
                    try
                    {
                        string StepName = "VerifySubOrder";
                        SamsaraTestDataHandler(StepName, dtBlotter, dTable, new List<String>());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occur on SamsaraTestDataHandler :" + ex.Message);
                    }
                }
                DataUtilities.VerifyDate(dTable, dtBlotter);
                List<String> errors = Recon.RunRecon(dtBlotter, dTable, columns, 0.01);
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
