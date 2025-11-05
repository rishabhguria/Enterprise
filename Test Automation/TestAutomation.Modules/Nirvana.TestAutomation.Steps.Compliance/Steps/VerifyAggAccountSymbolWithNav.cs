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
   public class VerifyAggAccountSymbolWithNav : PopUpUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string DevPath = ApplicationArguments.EsperCompliancePath;
                if (File.Exists(DevPath + TestDataConstants.DUMP_PATH + "AggregationAccountSymbolWithNav.xml"))
                    File.Delete(DevPath + TestDataConstants.DUMP_PATH + "AggregationAccountSymbolWithNav.xml");
                while (!File.Exists(DevPath + TestDataConstants.DUMP_PATH + "AggregationAccountSymbolWithNav.xml"))
                { PopUpUIMap.DumpWindow("AccountSymbolWithNav"); }
                StringBuilder errorBuilder = new StringBuilder();
                DataTable xmltoDataTable = CommonMethods.ReadXML(DevPath + TestDataConstants.DUMP_PATH + "AggregationAccountSymbolWithNav.xml");
                List<string> keyColumns = new List<string>() {"symbol"};
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
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }
    }
}
