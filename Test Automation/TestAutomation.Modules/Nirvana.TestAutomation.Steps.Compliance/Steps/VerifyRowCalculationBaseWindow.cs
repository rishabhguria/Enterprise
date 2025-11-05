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
using Nirvana.TestAutomation.Interfaces.Enums;


namespace Nirvana.TestAutomation.Steps.Compliance
{
    class VerifyRowCalculationBaseWindow : PopUpUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                int i = 1;
                string DevPath = ApplicationArguments.EsperCompliancePath;
                if (File.Exists(DevPath + TestDataConstants.DUMP_PATH + "RowCalculationBaseWindow.xml"))
                    File.Delete(DevPath + TestDataConstants.DUMP_PATH + "RowCalculationBaseWindow.xml");
                while (!File.Exists(DevPath + TestDataConstants.DUMP_PATH + "RowCalculationBaseWindow.xml"))
                {
                    i++;
                    if (i > 5)
                        throw new Exception("Unable to create dump file at " + DevPath);
                    PopUpUIMap.DumpWindow("RowCalculationBaseWindow"); 
				}
               StringBuilder errorBuilder = new StringBuilder();
               DataTable xmltoDataTable = ReadXML(DevPath + TestDataConstants.DUMP_PATH + "RowCalculationBaseWindow.xml");
               if (xmltoDataTable != null && testData.Tables[0].Rows.Count > 0)
               {
                   List<string> keyColumns = new List<string>() { "symbol", "quantity" };
                   DataTable dumpData = DataUtilities.RemoveTrailingZeroes(xmltoDataTable);
                   DataTable subset = testData.Tables[sheetIndexToName[0]];
                   try
                   {
                       string StepName = "VerifyRowCalculationBaseWindow";
                       DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, keyColumns);
                       Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                       SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                   }
                   catch (Exception)
                   { }
                   // verify values upto 2 decimals
                   List<string> errors = Recon.RunRecon(dumpData, subset, keyColumns, 0.1, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);  //  RunRecon(xmltoDataTable,testData.Tables[sheetIndexToName[0]],null);
                   if (errors.Count > 0)
                       errorBuilder.Append("Errors:-" + String.Join("\n\r", errors));

                   if (!String.IsNullOrEmpty(errorBuilder.ToString()))
                       _result.AddResult(false, errorBuilder.ToString());
                   return _result;
               }
               else if (xmltoDataTable == null && testData.Tables[0].Rows.Count>0)
               {
                   throw new ArgumentException("RowCalculationBaseWindow is null but excel contains rows");
                  // _result.AddResult(false, "RowCalculationBaseWindow is null but excel contains rows");
                   
               }

               else if (xmltoDataTable == null && testData.Tables[0].Rows.Count == 0) 
               {
                   return _result;

               }
               _result.IsPassed = false;
               return _result;

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyRowCalculationBaseWindow");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }
        // this method returns xml content in table form..if file is empty it returns null table
        private static DataTable ReadXML(string file)
        {
            DataTable table = new DataTable("Item");
            try
            {
                DataSet lstNode = new DataSet();
                lstNode.ReadXml(file);
                if (lstNode.Tables.Count > 0)
                {
                    table = lstNode.Tables[0];
                    return table;
                }
                else 
                    return null;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return null;
        }
    }
}
