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
    class VerifyAggregationSectorWithNav : PopUpUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                PopUpUIMap.DumpWindow("AggregationSectorWithNav");
                StringBuilder errorBuilder = new StringBuilder();
                string DevPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\.."));
                DataTable xmltoDataTable = CommonMethods.ReadXML(DevPath + TestDataConstants.DUMP_PATH + "AggregationSectorWithNav.xml");
                List<string> keyColumns = new List<string>() {"sector"};
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
