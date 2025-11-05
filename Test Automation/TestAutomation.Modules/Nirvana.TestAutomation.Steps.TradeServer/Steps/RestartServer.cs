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
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.TradeServer
{
    public partial class RestartServer : TradeServerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (PranaTradeConsoleHostApplication.IsVisible)
                {
                    Process.GetProcessesByName("Prana.TradeServiceHost")[0].Kill();
                    PranaTradeConsoleHostApplication.Start();
                    PranaTradeServiceMinusv2600.WaitForRespondingOrExited();
                    KeyboardUtilities.MinimizeWindow(ref TitleBar2);
                    //Minimise.Click(MouseButtons.Left);
                    try
                    {
                        ServerLogging();
                    }
                    catch
                    {
                        Wait(10000);
                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                throw;
                return _result;
            }
            
        }
        private void ServerLogging()
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
                    string TradeServerPath = ConfigurationManager.AppSettings["-serverReleasePath"] + "\\Logs\\TradeService_InformationReporter.log";
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(TradeServerPath), "TradeLog_backup.log");
                    if (!File.Exists(TradeServerPath))
                    {
                        Wait(4000);
                        if (!File.Exists(TradeServerPath))
                        {
                            throw new Exception("TradeServerLog file doesnot exist!!");
                        }
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(TradeServerPath))
                    {
                        var today = DateTime.Now;
                        string formattedTime = today.ToString("hh:mm");
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("hh:mm");

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("hh:mm");
                        File.Copy(TradeServerPath, tempFilePath, true);
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("TradeService started ") && (line.Contains(formattedTime) || line.Contains(oneMinuteAheadTime) || line.Contains(oneMinuteBehindTime)))
                            {
                                Console.WriteLine("TradeService Started");
                                flag = false;
                                Console.WriteLine("Time Taken To Start TradeService : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 10)
                        {
                            Console.WriteLine("Time Taken To Start TradeService : " + stopwatch.ElapsedMilliseconds * 0.001);
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
