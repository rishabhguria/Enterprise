using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Admin.Scripts;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Principal;

namespace Nirvana.TestAutomation.Steps.Admin
{
    class ChangeDate : IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string date = string.Empty;
                string datetoset = testData.Tables[0].Rows[0][0].ToString();
                if (datetoset.ToLower().Contains("reset"))
                {
                    date = "w32tm /resync";
                }
                else 
                {
                    string tempDate = DataUtilities.DateHandler(datetoset);
                    date = String.Format(ExcelStructureConstants.TT_DATE_FORMAT, DateTime.Parse(tempDate));
                    var fordate = DateTime.ParseExact(date, "MM/dd/yyyy", null);
                    string formattedDate = fordate.ToString("MM-dd-yy");
                    date = "date " + formattedDate;
                }
                if (datetoset.ToLower().Contains("reset"))
                {
                    string command = "/c sc config w32time start= auto && sc start w32time";

                    Process.Start(new ProcessStartInfo("cmd.exe", command)
                    {
                        Verb = "runas",
                        UseShellExecute = true
                    });

                }
                else 
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", "/c sc config w32time start= disabled && sc stop w32time")
                    {
                        Verb = "runas", 
                        UseShellExecute = true
                    });
                }
                var cmd = new ProcessStartInfo("cmd.exe", "/c " + date)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                var process = new Process { StartInfo = cmd };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("Output:\n" + output);
                Console.WriteLine("Error:\n" + error);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

    }
}
