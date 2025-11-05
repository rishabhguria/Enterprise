using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nirvana.TestAutomation.Steps;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using TestAutomationFX.Core;
using System.IO;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Factory;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;
using System.Runtime.InteropServices;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.Compliance
{
    class VerifyTaxlotWindow : PopUpUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                List<String> columns = new List<String>();
                try
                {
                    string StepName = "VerifyTaxlotWindow";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
                string tableName = sheetIndexToName[0];
                testData.Tables.Remove(tableName);
                DataTable updatedTable = subset.Copy();
                updatedTable.TableName = tableName;
                testData.Tables.Add(updatedTable);

               string DevPath = ApplicationArguments.EsperCompliancePath;
               if (File.Exists(DevPath + TestDataConstants.DUMP_PATH + "TaxlotWindow.xml"))
                   File.Delete(DevPath + TestDataConstants.DUMP_PATH + "TaxlotWindow.xml");
                while(!File.Exists(DevPath + TestDataConstants.DUMP_PATH + "TaxlotWindow.xml"))
                { PopUpUIMap.DumpWindow("TaxlotWindow");}
                StringBuilder errorBuilder = new StringBuilder();
                DataTable xmltoDataTable = CommonMethods.ReadXML(DevPath + TestDataConstants.DUMP_PATH + "TaxlotWindow.xml");
                List<string> keyColumns = new List<string>() { "symbol", "quantity" };
                DataTable dumpData = DataUtilities.RemoveTrailingZeroes(xmltoDataTable);
                List<string> errors = Recon.RunRecon(dumpData, testData.Tables[sheetIndexToName[0]], keyColumns, 0.01);  //  RunRecon(xmltoDataTable,testData.Tables[sheetIndexToName[0]],null);
                if (errors.Count > 0)
                    errorBuilder.Append("Errors:-" + String.Join("\n\r", errors));

                if (!String.IsNullOrEmpty(errorBuilder.ToString()))
                    _result.AddResult(false, errorBuilder.ToString());
                return _result;
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyTaxlotWindow");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }
    }
}
