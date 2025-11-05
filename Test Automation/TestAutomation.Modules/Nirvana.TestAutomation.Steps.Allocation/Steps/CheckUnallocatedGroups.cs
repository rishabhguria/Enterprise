using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class CheckUnallocatedGroups : AllocationUIMap, ITestStep
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
                _res = CheckUnallocation(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckUnallocatedGroups");
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
        /// Checks the unallocation.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private TestResult CheckUnallocation(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                StringBuilder error = new StringBuilder(String.Empty);
                DataTable dtExportedTrades = new DataTable();

                Records = GetLatestGridObject(GridUnallocated);
                dtExportedTrades = ExportTrades(Records, TextBoxFilename1, ButtonSave, ConfirmSaveAs, ButtonYesCnfrmSave, false, false);
                DataUtilities.VerifyDate(testData.Tables[0], dtExportedTrades);
                var reconErrors = VerifyAllocationData(testData, dtExportedTrades, sheetIndexToName);
                //Wait(2000);
                if (reconErrors.Count > 0)
                    error.Append("Errors:-" + String.Join("\n\r", reconErrors));
                if (!string.IsNullOrWhiteSpace(error.ToString()))
                    _res.AddResult(false, error.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Verifies the allocation data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="unallocatedData">The unallocated data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        private List<string> VerifyAllocationData(DataSet testData, DataTable unallocatedData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                try
                {
                    string StepName = "CheckUnallocatedGroups";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception)
                { }
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
                        columns = MandatoryColumns.AllocationGrid();
                        columns.Remove("Account Name");
                    }
                    else
                    {
                        columns.Add(TestDataConstants.COL_SYMBOL);
                        columns.Add(TestDataConstants.COL_SIDE);
                        columns.Add(TestDataConstants.COL_QUANTITY);
                        columns.Add(TestDataConstants.COL_EXECUTED_QTY);
                    }
                    subset.Columns.Remove("MandatoryColumn");
                }
                else
                {
                    columns.Add(TestDataConstants.COL_SYMBOL);
                    columns.Add(TestDataConstants.COL_SIDE);
                    columns.Add(TestDataConstants.COL_QUANTITY);
                    columns.Add(TestDataConstants.COL_EXECUTED_QTY);
                }
                
                if (unallocatedData != null)
                {
                    unallocatedData = DataUtilities.RemoveCommas(unallocatedData);
                }
                if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                {
                    try
                    {
                        string StepName = "CheckUnallocatedGroups";
                        SamsaraTestDataHandler(StepName, unallocatedData, subset, new List<String>());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occur on SamsaraTestDataHandler :" + ex.Message);
                    }
                }
                return Recon.RunRecon(unallocatedData, subset, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                return null;
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
