using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.IO;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    public class VerifyShortLocateGrid : ShortLocateUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenShortLocateUI();
                _res.ErrorMessage = VerifyShortLocateData(testData, sheetIndexToName[0], GrdShortLocate);
                return _res;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeShortLocate();
            }
            return _res;
        }
        private string VerifyShortLocateData(DataSet testData, string sheetName, UIWindow grid)
        {
           string errorMessage = string.Empty;
            try
            {
                StringBuilder activityError = new StringBuilder(String.Empty);
                string CsvString = modifyCSV();
                DataTable currentDataGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(CsvString));
                currentDataGrid = DataUtilities.RemoveCommas(currentDataGrid);
                DataTable excelFileData = testData.Tables[sheetName];
                List<string> keyColumns = new List<string>() { TestDataConstants.COL_TICKER, TestDataConstants.COL_Borrower_ID};
                List<string> errors = Recon.RunRecon(currentDataGrid, excelFileData, keyColumns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                if (errors.Count > 0)
                {
                    errorMessage = "Errors:-" + String.Join("\n\r", errors);
                }
                if (!string.IsNullOrWhiteSpace(activityError.ToString()))
                {
                    errorMessage = "Errors:-" + String.Join("\n\r", errors);
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
