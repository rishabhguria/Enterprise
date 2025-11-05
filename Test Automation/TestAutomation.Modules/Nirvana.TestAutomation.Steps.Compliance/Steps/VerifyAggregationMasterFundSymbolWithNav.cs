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
    class VerifyAggregationMasterFundSymbolWithNav : PopUpUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //PopUpUIMap.DumpWindow("AggregationMasterFundSymbolWithNav");
                // Take Dump for All Files
                try
                {
                    IntPtr CmdHandle = FindWindow(null, "'Esper Calculation Engine'");
                    if (CmdHandle == IntPtr.Zero)
                    {
                        Console.WriteLine("Esper is not running.");
                    }
                    SetForegroundWindow(CmdHandle);
                    Keyboard.SendKeys("dump");
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
                StringBuilder errorBuilder = new StringBuilder();
                //Update Dump File Path
                string DevPath = ApplicationArguments.EsperCompliancePath;
                DataTable xmltoDataTable = CommonMethods.ReadXML(DevPath + TestDataConstants.DUMP_PATH + "AggregationMasterFundSymbolWithNav.xml");
                List<string> keyColumns = new List<string>() { "symbol"};
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
