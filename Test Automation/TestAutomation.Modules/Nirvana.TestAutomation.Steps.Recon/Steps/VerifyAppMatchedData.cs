using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using ExcelDataReader;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.Recon
{
    public class VerifyAppMatchedData : ReconUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRecon();
                MaximizeRecon();
                string sheetName = sheetIndexToName[0];
                List<String> errors = VerifyApplicationData(testData, sheetName);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage = String.Join("\n\r", errors);
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseRecon();
            }
            return _result;
        }
        public List<string> VerifyApplicationData(DataSet testData, String sheetName)
        {
            try
            {
                string uiName = "ApplicationMatchedData";
                DataTable dtApplicationData = ExportData(uiName);
                List<String> columns = new List<String>();
                columns.Add("AccountName");
                columns.Add("Symbol");
                columns.Add("Side");
                DataTable excelData = testData.Tables[sheetName];
                List<String> errors = Utilities.Recon.RunRecon(uiData: dtApplicationData, excelData: excelData, columns: columns, tolerance: 0.01, toleranceFlag: false, dateTimeFlag: false, reconType: ReconType.RoundingMatch, roundingDigit: 2, midpointRounding: MidpointRounding.AwayFromZero);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
