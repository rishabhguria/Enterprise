using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TestAutomationFX.Core;
using System.Reflection;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.Steps;
using Nirvana.TestAutomation.Steps.PricingServer;
using Nirvana.TestAutomation.Steps.PranaClient;
using Nirvana.TestAutomation.Steps.Expnl;
using Nirvana.TestAutomation.Steps.Compliance;
using Nirvana.TestAutomation.Steps.TradeServer;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.Steps.Simulator;
using System.Windows.Forms;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.Compliance
{
    public class RestartReleasesAndServices : ComplianceEngineUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {

                ShutdownReleaseAndServices(testData);
                RestartAllServices(testData.Tables[0].Rows[0]);


            }

            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }


            return _res;
        }

        public void ShutdownReleaseAndServices(DataSet testData)
        {

            Process[] myProcesses = Process.GetProcesses();

            foreach (Process myProcess in myProcesses)
            {


                if (myProcess.MainWindowTitle.Contains("'Rule Mediator Engine'"))
                {
                    myProcess.CloseMainWindow();
                }
                if (myProcess.MainWindowTitle.Contains("'Esper Calculation Engine'"))
                {
                    myProcess.CloseMainWindow();
                }
                if (myProcess.MainWindowTitle.Contains("DropCopyFileReader"))
                {
                    myProcess.CloseMainWindow();
                }
                ///for basket compliance service close'
                if (myProcess.MainWindowTitle.Contains("'Basket Compliance Service'"))
                {
                    myProcess.CloseMainWindow();
                }
            }
            ICommandUtilities cmdUtilities = null;
            cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(Nirvana.TestAutomation.Interfaces.Enums.CommandType.Bat);
            cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
            string simulatorvalue = string.Empty;
            if(testData.Tables[0].Columns.Contains("DontShutSimulator") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["DontShutSimulator"].ToString())){
                simulatorvalue = testData.Tables[0].Rows[0]["DontShutSimulator"].ToString().ToUpper();
            }
            if (string.IsNullOrEmpty(simulatorvalue))
            {
                ShutDownSimulator();
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["ShutDownSimulator"]))
                {
                    cmdUtilities.ExecuteCommand("ShutDownSimulator.Bat");
                }
            }

        }
        private static void ShutDownSimulator()
        {
            try
            {
                Process[] proc = Process.GetProcessesByName("cmd");
                foreach (Process myProc in proc)
                {
                    if (myProc.MainWindowTitle.Contains("Buy") || myProc.MainWindowTitle.Contains("Sell Side Simulator") || myProc.MainWindowTitle.Contains("cmd.exe"))
                    {
                        myProc.CloseMainWindow();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void RestartAllServices(DataRow dr)
        {
            string simulatorvalue = string.Empty;
            if (dr.Table.Columns.Contains("DontShutSimulator") && !string.IsNullOrEmpty(dr["DontShutSimulator"].ToString()))
            {
                simulatorvalue = dr["DontShutSimulator"].ToString().ToUpper();
                AccessBridgeHelper.Inititalize();
            }
            if (string.IsNullOrEmpty(simulatorvalue))
            {
                StartSimulator();
            }
            //  StartServices();

            StartPricing();
            if (!ApplicationArguments.SkipCompliance && string.IsNullOrEmpty(simulatorvalue))
            {
                StartJboss();
            }
            StartServer();
            StartExpnl();
            if (!ApplicationArguments.SkipCompliance)
            {
                StartRuleMediator();
            }
            if (!ApplicationArguments.SkipCompliance)
            {
                StartEsper();
            }

            if (dr[TestDataConstants.RESTARTBASKET].ToString().ToUpper() == "YES" && !ApplicationArguments.SkipCompliance)
            {
                StartBasketCompliance();
            }

            StartApplication();

        }
        private void StartServices()
        {
            ICommandUtilities cmdUtilities = null;
            cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(Nirvana.TestAutomation.Interfaces.Enums.CommandType.Bat);
            cmdUtilities.ExecuteCommand("RestartServices.Bat");

        }
        private void StartApplication()
        {

            try
            {
                ProcessControlManager.MinimizeAllWindowsExceptSpecified();
                ExtentionMethods.WaitForVisibleUIApplication(ref PranaApplication, 40);
                Wait(3000);
                PranaClientUIMap obj = new PranaClientUIMap();
                obj.TxtLoginID.Click(MouseButtons.Left);
                Keyboard.SendKeys(ApplicationArguments.ReleaseUserName);
                obj.TxtPassword.Click(MouseButtons.Left);
                Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                obj.BtnLogin.Click(MouseButtons.Left);
                Wait(5000);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }
        private void StartJboss()
        {

            try
            {
                Wait(5000);
                ProcessStartInfo startJboss = new ProcessStartInfo();
                startJboss.FileName = "standalone.bat";
                startJboss.WorkingDirectory = ApplicationArguments.JbossCompliancePath;
                startJboss.WindowStyle = ProcessWindowStyle.Minimized;
                Process jbossProcess = new Process();
                jbossProcess.StartInfo = startJboss;
                jbossProcess.Start();
                Wait(45000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }


        private void StartBasketCompliance()
        {
            try
            {
                //Checking skip basket compliance value

                ProcessStartInfo start1 = new ProcessStartInfo();
                start1.FileName = "StartBasketComplianceService.bat";
                start1.WorkingDirectory = ApplicationArguments.BasketCompliancePath;
                start1.WindowStyle = ProcessWindowStyle.Minimized;
                Process java1 = new Process();
                java1.StartInfo = start1;
                java1.Start();
                System.Threading.Thread.Sleep(7000);
                Wait(90000);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        private void StartRuleMediator()
        {
            try
            {

                //Running Rule Engine Mediator.
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "StartRuleEngineMediator.bat";
                start.WorkingDirectory = ApplicationArguments.RuleEngineCompliancePath;
                start.WindowStyle = ProcessWindowStyle.Minimized;
                Process java = new Process();
                java.StartInfo = start;
                java.Start();
                System.Threading.Thread.Sleep(3000);
                Wait(5000);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        private void StartExpnl()
        {
            try
            {
                Wait(5000);
                ExPNLUIMap obj = new ExPNLUIMap();
                obj.PranaExpnlServiceHostApplication.Start();
                obj.PranaExpnlServiceMinusv2600.WaitForRespondingOrExited();
                Wait(5000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Start Server
        /// </summary>
        private void StartServer()
        {
            try
            {
                TradeServerUIMap obj = new TradeServerUIMap();
                obj.PranaTradeConsoleHostApplication.Start();
                obj.PranaTradeServiceMinusv2600.WaitForRespondingOrExited();
                Wait(5000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Start Pricing
        /// </summary>
        private void StartPricing()
        {
            try
            {
                Wait(1000);
                PricingServerUIMap obj = new PricingServerUIMap();
                obj.PranaPricingService2HostApplication.Start();
                obj.PranaPricingServiceMinusv2700.WaitForRespondingOrExited();
                Wait(7000);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        /* private void StartDropCopy()
         {
             try
             {
                 if (!ApplicationArguments.SkipDropCopyStartUp)
                 {
                     PranaDropCopyFileReader.WaitForVisible();
                     PranaDropCopyFileReader.WaitForResponding();
                 }
             }
             catch (Exception ex)
             {
                 bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                 if (rethrow)
                     throw;
             }

         }*/

        /// <summary>
        /// Start Simulator
        /// </summary>
        private void StartSimulator()
        {
            int retry = 0;
            while (retry < 2)
            {
                try
                {
                    CameronSimulator obj = new CameronSimulator();
                    obj.StartFixApplication.Start();
                    if (obj.Form1.IsVisible)
                    {
                        obj.Form1.BringToFront();
                        obj.Config_TT.Click(MouseButtons.Left);
                    }
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    bool flag = true;
                    bool firstRun = true;
                    while (flag)
                    {
                        Thread.Sleep(3000);
                        Process[] processes = Process.GetProcesses();
                        foreach (Process process in processes)
                        {
                            if (process.MainWindowTitle.Contains("MS"))
                            {
                                IntPtr hWnd = process.MainWindowHandle;
                                ExtentionMethods.ShowWindow(hWnd, 9);
                                Console.WriteLine("Time Taken To Start Simulator : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                flag = false;
                                break;
                            }

                        }
                        if (firstRun)
                        {

                            try
                            {
                                if (obj.Form1.IsVisible)
                                {
                                    obj.Form1.BringToFront();
                                    obj.Config_TT.Click(MouseButtons.Left);
                                }
                            }
                            catch { }
                        }
                        firstRun = false;
                    }

                    SetManualResponse();
                    break;
                    /*obj.Config_TT.Click(MouseButtons.Left);
                    Wait(15000);
                    // Working fine without minimizing the window but need to look for right solution
                    //KeyboardUtilities.MinimizeWindow(ref TitleBar3);
                    SetManualResponse();*/

                }
                catch (Exception)
                {
                    ShutDownSimulator();
                    retry++;
                }
            }
        }

        
        /// <summary>
        /// Setting manual response before performing any action on cameron simulator
        /// </summary>
        private void SetManualResponse()
        {
            try
            {
                ITestStep SettingManualResp = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, ExcelStructureConstants.CAMERON_SIMULATOR, ExcelStructureConstants.SET_MANUAL_RESP);
                TestResult obj = (TestResult)SettingManualResp.RunTest(null, null);
                if (!obj.IsPassed)
                {
                    Log.Error("Setting manual response failed.");
                }
                ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.SetResponseTo, TestDataConstants.Manual);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }

        private void StartEsper()
        {
            ProcessStartInfo start1 = new ProcessStartInfo();
            start1.FileName = "StartEsperCalculator.bat";
            start1.WorkingDirectory = ApplicationArguments.EsperCompliancePath;
            start1.WindowStyle = ProcessWindowStyle.Minimized;
            Process java1 = new Process();
            java1.StartInfo = start1;
            java1.Start();
            System.Threading.Thread.Sleep(7000);
            Wait(50000);

        }



    }
}