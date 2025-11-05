using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class CheckAllocatedTaxlots : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                _res.ErrorMessage = CheckAllocationTaxlots(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckAllocatedTaxlots");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// Checks the allocation.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private string CheckAllocationTaxlots(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;
            try
            {
                StringBuilder activityError = new StringBuilder(String.Empty);
                DataTable dtExportedTrades = new DataTable();

                Records1 = GetLatestGridObject(GridAllocated);
                dtExportedTrades = ExportTrades(Records1, TextBoxFilename2, ButtonSave1, ConfirmSaveAs2, ButtonYes2, true, true);
                // change the test case data for ui changes before verification
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                try
                {
                    string StepName = "CheckAllocatedTaxlots";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }

                var reconErrors = VerifyAllocationData(testData, dtExportedTrades, sheetIndexToName);
               // Wait(2000);

                if (reconErrors.Count > 0)
                {
                    errorMessage = "Errors:-" + String.Join("\n\r", reconErrors);
                }
                if (!string.IsNullOrWhiteSpace(activityError.ToString()))
                {
                    errorMessage = "Errors:-" + String.Join("\n\r", activityError);
                }
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
        /// Verifies the allocation data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="allocatedData">The allocated data.</param>
        /// <returns></returns>
        private List<string> VerifyAllocationData(DataSet testData, DataTable allocatedData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                DataRow drow = null;
                if (subset.Rows.Count > 0)
                {
                    drow = subset.Rows[0];
                }
                // If Table contains mandatory columns then verify mandatory columns
                if (subset.Columns.Contains("MandatoryColumn"))
                {

                    if (!String.IsNullOrEmpty(drow["MandatoryColumn"].ToString()))
                    {
                        columns = MandatoryColumns.AllocationTaxlot();
                    }
                    subset.Columns.Remove("MandatoryColumn");
                }
               
                //columns.Add(TestDataConstants.COL_SYMBOL);
                //columns.Add(TestDataConstants.COL_SIDE);
                if (allocatedData != null)
                {
                    allocatedData = DataUtilities.RemoveCommas(allocatedData);
                }

                if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                {
                    try
                    {
                        string StepName = "CheckAllocatedTaxlots";
                        SamsaraTestDataHandler(StepName, allocatedData, subset, new List<String>());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occur on SamsaraTestDataHandler :" + ex.Message);
                    }
                }
                return Recon.RunRecon(allocatedData, subset, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero, true);
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
