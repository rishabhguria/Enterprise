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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class CheckAllocatedGroups : AllocationUIMap, ITestStep
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
                _res.ErrorMessage = CheckAllocation(testData, sheetIndexToName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckAllocatedGroups");
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
        private string CheckAllocation(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            string errorMessage = string.Empty;
            try
            {
                StringBuilder activityError = new StringBuilder(String.Empty);
                DataTable dtExportedTrades = new DataTable();

                Records1 = GetLatestGridObject(GridAllocated);
                dtExportedTrades = ExportTrades(Records1, TextBoxFilename, ButtonSave2, ConfirmSaveAs4, ButtonYes1,false,true);
               
                // change the test case data for ui changes before verification
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                try
                {
                    string StepName = "CheckAllocatedGroups";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
                string caseType = ApplicationArguments.TestCaseToBeRun.ToString();
                if (!caseType.Contains("CA") && !caseType.Contains("GL") && !caseType.Contains("Closing"))
                {
                    DataUtilities.VerifyDate(subset, dtExportedTrades);
                }
                var reconErrors = VerifyAllocationData(subset, dtExportedTrades, sheetIndexToName);
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
        private List<string> VerifyAllocationData(DataTable testData, DataTable allocatedData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataTable subset = testData;
                List<String> columns = new List<String>();
                /*try
                {
                    string StepName = "CheckAllocatedGroups";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception)
                { }*/
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
                    }
                    else
                    {
                        columns.Add(TestDataConstants.COL_SYMBOL);
                        columns.Add(TestDataConstants.COL_SIDE);
                    }
                    subset.Columns.Remove("MandatoryColumn");
                }
                else
                {
                    columns.Add(TestDataConstants.COL_SYMBOL);
                    columns.Add(TestDataConstants.COL_SIDE);
                }
           
                if (allocatedData != null)
                {
                    allocatedData = DataUtilities.RemoveCommas(allocatedData);
                }
                
                if (ApplicationArguments.ProductDependency.ToLower() == "samsara") // temp condition until samsara enterprise run with PlanB
                {
                    try
                    {
                        string RemoveColumns = ConfigurationManager.AppSettings["RemoveColumns"];
                        subset = DataUtilities.RemoveColumnsAndRows(RemoveColumns, subset);

                        string StepName = "CheckAllocatedGroups";
                        SamsaraTestDataHandler(StepName, allocatedData, subset, new List<String>());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occur on SamsaraTestDataHandler :" + ex.Message);
                    }
                }
                



                return Recon.RunRecon(allocatedData, subset, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
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