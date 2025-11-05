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
using System.Diagnostics;


namespace Nirvana.TestAutomation.Steps.Compliance
{
   public class RestartEsper : PopUpUIMap, ITestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                var allProcceses = Process.GetProcesses();
                foreach (Process process in allProcceses)
                {
                    if (process.ProcessName.Contains("cmd") && process.MainWindowTitle.Contains("'Esper Calculation Engine'"))
                    {
                        process.CloseMainWindow();
                        break;
                    }
                }
                ProcessStartInfo start1 = new ProcessStartInfo();
                start1.FileName = "StartEsperCalculator.bat";
                start1.WorkingDirectory = ApplicationArguments.EsperCompliancePath;
                start1.WindowStyle = ProcessWindowStyle.Minimized;
                Process java1 = new Process();
                java1.StartInfo = start1;
                java1.Start();
                System.Threading.Thread.Sleep(7000);
                Wait(90000);
                /*ICommandUtilities cmdUtilities = null;
                cmdUtilities= new BatchCommandUtilities();
                cmdUtilities.ExecuteCommand("StartEsperCalculator.Bat");
                Wait(10000);*/
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
