using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;
using System.IO;

namespace Nirvana.TestAutomation.Steps.Expnl
{
    [UITestFixture]
    public partial class RestartExpnl : ExPNLUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (PranaExpnlServiceHostApplication.IsVisible)
                {
                    Process[] _process = Process.GetProcessesByName("Prana.ExpnlServiceHost");
                    foreach (Process proc in _process)
                        proc.Kill();
                    Wait(5000);
                    PranaExpnlServiceMinusv2600.WaitForVisible();
                    PranaExpnlServiceMinusv2600.WaitForRespondingOrExited();
                    Nirvana.TestAutomation.Utilities.ExtentionMethods.SwitchToPartialWindowTitle(ConfigurationManager.AppSettings["ExpnlServiceWindow"].ToString());
                    KeyboardUtilities.MinimizeWindow(ref TitleBar);
                    try
                    {
                        ExpnlLogging();
                    }
                    catch
                    {
                        _process = Process.GetProcessesByName("Prana.ExpnlServiceHost");
                        foreach (Process proc in _process)
                            proc.Kill();
                        Wait(5000);
                        PranaExpnlServiceMinusv2600.WaitForVisible();
                        PranaExpnlServiceMinusv2600.WaitForRespondingOrExited();
                        Wait(15000);
                    }
                }
                
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            /*finally
            {
                Minimize.Click(MouseButtons.Left);
            }*/
            return _result;   
        }
        private void ExpnlLogging()
        {
            try
            {
                bool flag = true;
                int retry = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (flag)
                {
                    Wait(3000);
                    string ExpnlPath = ConfigurationManager.AppSettings["-expnlReleasePath"] + "\\Logs\\ExpnlService_InformationReporter.log";
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(ExpnlPath), "ExpnlLog_backup.log");
                    if (!File.Exists(ExpnlPath))
                    {
                        Wait(4000);
                        if (!File.Exists(ExpnlPath))
                        {
                            throw new Exception("ExpnlLog file doesnot exist!!");
                        }
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(ExpnlPath))
                    {
                        var today = DateTime.Now;
                        string formattedTime = today.ToString("hh:mm");
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("hh:mm");

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("hh:mm");
                        File.Copy(ExpnlPath, tempFilePath, true);
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("ExpnlService started ") && (line.Contains(formattedTime) || line.Contains(oneMinuteAheadTime) || line.Contains(oneMinuteBehindTime)))
                            {
                                Console.WriteLine("ExpnlService Started");
                                flag = false;
                                Console.WriteLine("Time Taken To Start ExpnlService : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 10)
                        {
                            Console.WriteLine("Time Taken To Start ExpnlService : " + stopwatch.ElapsedMilliseconds * 0.001);
                            stopwatch.Stop();
                            flag = false;
                        }
                    }
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
