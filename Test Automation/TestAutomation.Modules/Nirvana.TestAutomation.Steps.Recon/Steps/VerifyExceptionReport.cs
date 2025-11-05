using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;
using ExcelDataReader;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.Recon
{
    public class VerifyExceptionReport : ReconUIMap, ITestStep
    {
        public static string exceptionFileName = string.Empty;
        public static string exceptionFilePath = string.Empty;
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRecon();
                MaximizeRecon();
                string sheetName = sheetIndexToName[0];
                List<String> errors = CheckExceptionReport(testData, sheetName);
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

        public List<string> CheckExceptionReport(DataSet testData, String sheetName)
        {
            try
            {
                DataTable exceldata = testData.Tables[sheetName];
                exceptionFileName = setName(testData);
                DataTable dtExceptionData = ExceptionReportData(exceptionFileName);
                List<String> Columns = new List<String>();
                List<String> errors = Utilities.Recon.RunRecon(uiData: dtExceptionData, excelData: exceldata, columns: Columns, reconType: ReconType.RoundingMatch, midpointRounding: MidpointRounding.AwayFromZero,roundingDigit: 2);
                return errors;

            }

            catch (Exception)
            {
                throw;
            }
        }
        public string setName(DataSet testData)
        {
            
             
           List<String> details= ViewDataOnRecon.ViewDetails;
           
           String from = details[0];
           from = from.Replace("/", "-");
           String to = details[1];
           to = to.Replace("/", "-");
           String type = details[2];
           String format = details[3];
            string fileName = format+"~" + from + "~" + to + ".xls";


            exceptionFilePath = "7\\" + type + "\\" + format;
            
            
            return fileName;
        }
    }
}
