using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.Exceptions;
using TestAutomationFX.UI;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Configuration;
using System.Linq;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;
using Nirvana.TestAutomation.UIAutomation;
using System.Runtime.Caching;
using System.Threading;
using OpenQA.Selenium;
using System.Runtime.InteropServices;
using UIAutomationClient;
using System.Text;

namespace Nirvana.TestAutomation.TestExecutor
{
    [UITestFixture]
    public partial class CoreExecutor : UIMap
    {
        #region Members
        /// <summary>
        /// The modules and steps mapping
        /// </summary>
        List<string> _modulesAndStepsMapping = new List<string>();
        private static CUIAutomation automation = new CUIAutomation();
        Stopwatch _timer = null;

        private Stopwatch GetTimerInstance()
        {
            return _timer = (_timer != null) ? _timer : new Stopwatch();
        }
        public bool IUIAutomationFileLoaded = false;
        private static int RetryCounter = 0;
        private static int MaxTries = int.Parse(ConfigurationManager.AppSettings["RetryCountForLogin"]);
        private static bool Retry = true;
        public bool LoginSamsaratype = true;
        public static int count = 0;
        public static string previousStep = string.Empty;
        List<string> UndefinedModuleSteps = new List<string>();
        MemoryCache cache = CacheManager.Instance.GetCache();
        bool prefStepExistedonStepList = false;
        #endregion Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreExecutor"/> class.
        /// </summary>
        public CoreExecutor()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreExecutor"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CoreExecutor(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        
        #endregion Constructors

        #region Methods
        /// <summary>
        /// UI test for application startup
        /// </summary>
        [UITest]
        public void ApplicationStartUp()
        {
            
            //ShutDownSamsaraRelease();
            if (count > 0)
            {
                ApplicationArguments.SkipStartUp = true;
                ApplicationArguments.SkipLogin = true;
            }
            else
            {
                ShutDownSamsaraRelease();
                Process[] myProcesses = Process.GetProcesses();
                ShutDownEnterprise(myProcesses);
                TestExecutor.ShutDownSimulator();
                StartPortIssue();
                TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "startRecording", "fail");
            }
                
            Stopwatch timer = GetTimerInstance();
            try
            {
                if (!ApplicationArguments.SkipStartUp)
                {
                    StartDropCopy();
                    StartSimulator();
                    timer.Restart();
                    StartPricing();
                    StartJboss();
                    StartServer();
                    StartExpnl();
                    StartRuleMediator();
                    StartEsper();
                    StartBasketCompliance();
                    StartServiceGatway();
                    StartChromeDriver();
                    StartSamsaraServices();
                    StartSamsaraApplication();
                    ProcessControlManager.MinimizeAllWindowsExceptSpecified();
                    //For kill the process of winros
                    Process[] _processesWinros = Process.GetProcessesByName("Winros");
                    foreach (Process proc in _processesWinros)
                        proc.Kill();

                    Log.Success("Application started sucessfully.");
                    TestStatusLog.TestCaseResult(string.Empty, "0", MessageConstants.APPLICATION_START_UP, string.Empty, string.Empty, timer.Elapsed);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to start Application. Reason : " + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
                TestStatusLog.TestCaseResult(string.Empty, "0", MessageConstants.APPLICATION_START_UP, ex.Message, string.Empty, timer.Elapsed);
                SaveAndExit(0);
            }
        }


        public int CountCurrentTestcase()
        {
            return count;
        }
        public void setCountCurrentTestcase(int newCount)
        {
            count = newCount;
        }

        public void StartSamsaraApplication()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(10000);

                    /*ProcessStartInfo StartChromeExe = new ProcessStartInfo();
                    StartChromeExe.FileName = "Webapplication.bat";
                    StartChromeExe.WorkingDirectory = ConfigurationManager.AppSettings["ChromeDriverExePath"];
                    StartChromeExe.WindowStyle = ProcessWindowStyle.Minimized;
                    Process ChromeProcess = new Process();
                    ChromeProcess.StartInfo = StartChromeExe;
                    ChromeProcess.Start();
                    Wait(60000);*/

                    string baseFileName = "output";
                    string extension = "txt";
                    string uniqueFileName = DataUtilities.GetUniqueFileName(baseFileName, extension);
                    string sourceFilePath = Path.Combine(ConfigurationManager.AppSettings["SamsaraDirectory"], uniqueFileName);
                    //string sourceFilePath = ConfigurationManager.AppSettings["SamsaraDirectory"] + "\\output.txt";
                    try
                    {

                        if (File.Exists(sourceFilePath))
                        {
                            File.Delete(sourceFilePath);
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    ProcessControlManager.ProcessStarter("SamsaraCacheHandler.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"]);
                    ProcessControlManager.ProcessStarter("Webapplication.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"], uniqueFileName);
                    CommonMethods.VerifyStringInTextFile((sourceFilePath), "http://", 15000);
                    ProcessControlManager.ProcessStarter("YarnStartBatch.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"]);
                    ProcessControlManager.MinimizeAllWindowsExceptSpecified();



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void ShutDownSamsaraRelease()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {

                    Wait(1000);
                    ProcessStartInfo StartSamsaraServices = new ProcessStartInfo();
                    StartSamsaraServices.FileName = "Stop_All_GreenFieldMiniServices.bat";
                    StartSamsaraServices.WorkingDirectory = ConfigurationManager.AppSettings["SamsaraServicePath"];
                    StartSamsaraServices.WindowStyle = ProcessWindowStyle.Minimized;
                    Process SamsaraProcess = new Process();
                    SamsaraProcess.StartInfo = StartSamsaraServices;
                    SamsaraProcess.Start();
                    Wait(5000);
                    ProcessControlManager.CloseCmdProcess("webpack");

                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at ShutDownSamsaraRelease class", "Service");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [ShutDownSamsaraRelease class]");
                }
            }

        }

        private void StartZookeperService()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(5000);
                    ProcessStartInfo StartZookeeper = new ProcessStartInfo();
                    StartZookeeper.FileName = "1. Start Zookeeper Server.bat";
                    StartZookeeper.WorkingDirectory = ApplicationArguments.KafkaPath;
                    StartZookeeper.WindowStyle = ProcessWindowStyle.Minimized;
                    Process ZookeeperProcess = new Process();
                    ZookeeperProcess.StartInfo = StartZookeeper;
                    ZookeeperProcess.Start();
                    Console.WriteLine(StartZookeeper.WorkingDirectory.ToString() + StartZookeeper.FileName.ToString());
                    Wait(5000);
                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartZookeperService class");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartZookeperService class]");
                }
            }

        }

        private void StartServiceGatway()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    ProcessStartInfo StartServiceGateway = new ProcessStartInfo();
                    StartServiceGateway.FileName = "Prana.ServiceGateway.exe";
                    StartServiceGateway.WorkingDirectory = ConfigurationManager.AppSettings["ServiceGatewayPath"];
                    StartServiceGateway.WindowStyle = ProcessWindowStyle.Minimized;
                    Process ServiceGatwayProcess = new Process();
                    ServiceGatwayProcess.StartInfo = StartServiceGateway;
                    ServiceGatwayProcess.Start();
                    try
                    {
                        ServiceGatewayLogging();
                    }
                    catch
                    {
                    Wait(45000);
                }
                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartServiceGatway class", "Service");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartServiceGatway class]");
                }
            }
        }

        private void ServiceGatewayLogging()
        {
            try
            {
                bool flag = true;
                int retry = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (flag)
                {
                    Wait(7000);
                    var today = DateTime.Now;
                    var todayDate = DateTime.Today.ToString("dd");
                    string ServiceGatewayLogPath = ConfigurationManager.AppSettings["ServiceGatewayLog"] + today.Year + today.Month + todayDate + ".log";
                    if (!File.Exists(ServiceGatewayLogPath))
                    {
                        int month = today.Month;
                        string mon = month.ToString();
                        if (month < 10)
                        {
                            mon = "0" + mon;
                        }
                        if (Int32.Parse(todayDate) < 10)
                        {
                            int dateYesterDay = Int32.Parse(todayDate);
                            string yesterDay = "0" + dateYesterDay;
                            ServiceGatewayLogPath = ConfigurationManager.AppSettings["ServiceGatewayLog"] + today.Year + mon + yesterDay + ".log";
                            if (!File.Exists(ServiceGatewayLogPath))
                            {
                                Wait(9000);
                                ServiceGatewayLogPath = ConfigurationManager.AppSettings["ServiceGatewayLog"] + today.Year + mon + yesterDay + ".log";
                                if (!File.Exists(ServiceGatewayLogPath))
                                {
                                    throw new Exception("ServiceGatewayLogPath file doesnot exist!!");
                                }
                            }
                        }
                        else
                        {
                            ServiceGatewayLogPath = ConfigurationManager.AppSettings["ServiceGatewayLog"] + today.Year + mon + todayDate + ".log";
                            if (!File.Exists(ServiceGatewayLogPath))
                            {
                                Wait(9000);
                                ServiceGatewayLogPath = ConfigurationManager.AppSettings["ServiceGatewayLog"] + today.Year + mon + todayDate + ".log";
                                if (!File.Exists(ServiceGatewayLogPath))
                                {
                                    throw new Exception("ServiceGatewayLogPath file doesnot exist!!");
                                }
                            }
                        }
                    }

                    string tempFilePath = Path.Combine(Path.GetDirectoryName(ServiceGatewayLogPath), "Prana.ServiceGateway_backup.log");
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                    if (File.Exists(ServiceGatewayLogPath))
                    {
                        File.Copy(ServiceGatewayLogPath, tempFilePath, true);

                        // Format the time as "23 Oct 10:08"
                        string formattedTime = today.ToString("dd MMM HH:mm");
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("dd MMM HH:mm");

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("dd MMM HH:mm");

                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("Service Gateway is now running") && (line.Contains(formattedTime) || line.Contains(oneMinuteAheadTime) || line.Contains(oneMinuteBehindTime)))
                            {
                                Console.WriteLine("Service GateWay hosted");
                                flag = false;
                                Console.WriteLine("Time Taken To Start ServiceGateway : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 40)
                        {
                            Console.WriteLine("Time Taken To Start ServiceGateway : " + stopwatch.ElapsedMilliseconds * 0.001);
                            stopwatch.Stop();
                            flag = false;
                            try
                            {
                                Process[] processes = Process.GetProcessesByName("Prana.ServiceGateway");
                                foreach (Process process in processes)
                                {
                                    process.Kill();
                                    process.WaitForExit();
                                }
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine("Error killing the 'Prana.ServiceGateway' process: " + ex.Message);
                            }
                            ProcessStartInfo StartServiceGateway = new ProcessStartInfo();
                            StartServiceGateway.FileName = "Prana.ServiceGateway.exe";
                            StartServiceGateway.WorkingDirectory = ConfigurationManager.AppSettings["ServiceGatewayPath"];
                            StartServiceGateway.WindowStyle = ProcessWindowStyle.Minimized;
                            Process ServiceGatwayProcess = new Process();
                            ServiceGatwayProcess.StartInfo = StartServiceGateway;
                            ServiceGatwayProcess.Start();
                        }
                    }

                }
            }
            catch
            {
                throw;
            }
        }

        private void StartPortIssue()
        {
            //"E:\PortIssue\ZookeeperKafkaWindowService - Shortcut"
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(3000);
                    ProcessStartInfo StartPort = new ProcessStartInfo();
                    StartPort.FileName = "ZookeeperKafkaWindowService - Shortcut";
                    StartPort.WorkingDirectory = ConfigurationManager.AppSettings["PortIssuePath"];
                    Process StartPortProc = new Process();
                    StartPortProc.StartInfo = StartPort;
                    StartPortProc.Start();
                    Wait(5000);
                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartPortIssue class");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartPortIssue class]");
                }
            }
        }

        private void StartChromeDriver()
        {
            //"E:\DistributedAutomation\chromedriver.exe"
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    ProcessStartInfo StartChromeExe = new ProcessStartInfo();
                    StartChromeExe.FileName = "chromedriver.exe";
                    StartChromeExe.WorkingDirectory = ConfigurationManager.AppSettings["ChromeDriverExePath"];
                    StartChromeExe.WindowStyle = ProcessWindowStyle.Minimized;
                    StartChromeExe.Arguments = "--port=9515";
                    Process ChromeProcess = new Process();
                    ChromeProcess.StartInfo = StartChromeExe;
                    ChromeProcess.Start();
                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartChromeDriver class", "Service");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartChromeDriver class]");
                }
            }
        }

        private void StartKafkaServices()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(5000);
                    ProcessStartInfo StartKafka = new ProcessStartInfo();
                    StartKafka.FileName = "2. Start Kafka.bat";
                    StartKafka.WorkingDirectory = ApplicationArguments.KafkaPath;
                    StartKafka.WindowStyle = ProcessWindowStyle.Minimized;
                    Process KafkaProcess = new Process();
                    KafkaProcess.StartInfo = StartKafka;
                    KafkaProcess.Start();
                    Wait(5000);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartKafkaServices class]");
                }
            }
        }

        private void StartSamsaraServices()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(5000);
                    ProcessStartInfo StartSamsaraServices = new ProcessStartInfo();
                    StartSamsaraServices.FileName = "Start_All_GreenFieldMiniServices.bat";
                    StartSamsaraServices.WorkingDirectory = ConfigurationManager.AppSettings["SamsaraServicePath"];
                    StartSamsaraServices.WindowStyle = ProcessWindowStyle.Minimized;
                    Process SamsaraProcess = new Process();
                    SamsaraProcess.StartInfo = StartSamsaraServices;
                    SamsaraProcess.Start();

                    Wait(30000);
                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartSamsaraServices class", "Service");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartSamsaraServices class]");
                }
            }
        }

        /// <summary>
        /// Starting JBOSS
        /// </summary>
        private void StartJboss()
        {
            //Checking skip compliance value
            if (!ApplicationArguments.SkipCompliance)
            {
                try
                {
                    ProcessStartInfo startJboss = new ProcessStartInfo();
                    startJboss.FileName = "standalone.bat";
                    startJboss.WorkingDirectory = ApplicationArguments.JbossCompliancePath;
                    startJboss.WindowStyle = ProcessWindowStyle.Minimized;
                    Process jbossProcess = new Process();
                    jbossProcess.StartInfo = startJboss;
                    jbossProcess.Start();
                    try
                    {
                        jbossLogging();
                    }
                    catch {
                        Wait(40000);                        
                    }
                }
                catch (Exception ex)
                {
                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartJboss class", "Service");
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw new Exception(ex.Message + " [StartJboss class]");
                }
            }
        }

        private void jbossLogging()
        {
            try
            {
                bool flag = true;
                int retry = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (flag)
                {
                    Wait(5000);
                    string JbossPath = ConfigurationManager.AppSettings["JbossLog"];
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(JbossPath), "JbossLog_backup.log");
                    if (!File.Exists(JbossPath))
                    {
                        throw new Exception("JbossLog file doesnot exist!!");
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(JbossPath))
                    {
                        File.Copy(JbossPath, tempFilePath, true);
                        var today = DateTime.Now;
                        string formattedTime = today.ToString("HH:mm");
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("HH:mm");

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("HH:mm");
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("passive or on-demand") && (line.Contains(formattedTime) || line.Contains(oneMinuteAheadTime) || line.Contains(oneMinuteBehindTime)))
                            {
                                Console.WriteLine("Jboss hosted");
                                flag = false;
                                Console.WriteLine("Time Taken To Start Jboss : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 50)
                        {
                            Console.WriteLine("Time Taken To Start Jboss : " + stopwatch.ElapsedMilliseconds * 0.001);
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

        /// <summary>
        /// Starting Rule Mediator
        /// </summary>
        private void StartRuleMediator()
        {
            try
            {
                //Checking skip compliance value
                if (!ApplicationArguments.SkipCompliance)
                {
                    //Running Rule Engine Mediator.
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = "StartRuleEngineMediator.bat";
                    start.WorkingDirectory = ApplicationArguments.RuleEngineCompliancePath;
                    start.WindowStyle = ProcessWindowStyle.Minimized;
                    Process java = new Process();
                    java.StartInfo = start;
                    java.Start();

                    try
                    {
                        MediatorLogging();
                    }
                    catch
                    {
                        Wait(10000);
                    }
                }
                
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartRuleMediator class", "Service");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartRuleMediator class]");
            }
        }

        private void MediatorLogging()
        {
            try
            {
                bool flag = true;
                int retry = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (flag)
                {
                    Wait(5000);
                    string MediatorPath = ConfigurationManager.AppSettings["-ruleEngineCompliancePath"] + "\\Log\\RuleMediator.log";
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(MediatorPath), "RuleMediator_backup.log");
                    if (!File.Exists(MediatorPath))
                    {
                        Wait(5000);
                        MediatorPath = ConfigurationManager.AppSettings["-ruleEngineCompliancePath"] + "\\Log\\RuleMediator.log";
                        if (!File.Exists(MediatorPath))
                        {
                            throw new Exception("MediatorLog file doesnot exist!!");
                        }
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(MediatorPath))
                    {
                        File.Copy(MediatorPath, tempFilePath, true);
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("Other Event Listener started"))
                            {
                                Console.WriteLine("rule mediator hosted");
                                flag = false;
                                Console.WriteLine("Time Taken To Start mediator : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 40)
                        {
                            Console.WriteLine("Time Taken To Start mediator : " + stopwatch.ElapsedMilliseconds * 0.001);
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

        /// <summary>
        /// Running Esper
        /// </summary>
        private void StartEsper()
        {
            try
            {
                //Checking skip compliance value
                if (!ApplicationArguments.SkipCompliance)
                {
                    ProcessStartInfo start1 = new ProcessStartInfo();
                    start1.FileName = "StartEsperCalculator.bat";
                    start1.WorkingDirectory = ApplicationArguments.EsperCompliancePath;
                    start1.WindowStyle = ProcessWindowStyle.Minimized;
                    Process java1 = new Process();
                    java1.StartInfo = start1;
                    java1.Start();
                    try
                    {
                        esperLogging();
                    }
                    catch {
                        Wait(45000);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartEsper class", "Service");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartEsper class]");
            }
        }

        private void esperLogging()
        {
            try
            {
                bool flag = true;
                int retry = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (flag)
                {
                    Wait(8000);
                    string EsperPath = ConfigurationManager.AppSettings["-esperCompliancePath"] + "\\Log\\EsperLog.log";
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(EsperPath), "EsperPathLog_backup.log");
                    if (!File.Exists(EsperPath))
                    {
                        Wait(8000);
                        EsperPath = ConfigurationManager.AppSettings["-esperCompliancePath"] + "\\Log\\EsperLog.log";
                        if (!File.Exists(EsperPath))
                        {
                            throw new Exception("EsperLog file doesnot exist!!");
                        }
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(EsperPath))
                    {
                        File.Copy(EsperPath, tempFilePath, true);
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("Esper calculation engine started"))
                            {
                                Console.WriteLine("Esper hosted");
                                flag = false;
                                Console.WriteLine("Time Taken To Start Esper : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 40)
                        {
                            Console.WriteLine("Time Taken To Start Esper : " + stopwatch.ElapsedMilliseconds * 0.001);
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

        public static void StartBasketCompliance()
        {
            try
            {
                //Checking skip basket compliance value
                if (!ApplicationArguments.SkipBasketCompliance)
                {
                    ProcessStartInfo start1 = new ProcessStartInfo();
                    start1.FileName = "StartBasketComplianceService.bat";
                    start1.WorkingDirectory = ApplicationArguments.BasketCompliancePath;
                    start1.WindowStyle = ProcessWindowStyle.Minimized;
                    Process java1 = new Process();
                    java1.StartInfo = start1;
                    java1.Start();
                    try
                    {
                        BasketComplianceLogging();
                    }
                    catch {
                        Wait(30000);                        
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartBasketCompliance class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartBasketCompliance class]");
            }
        }

        private static void BasketComplianceLogging()
        {
            try
            {
                bool flag = true;
                int retry = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (flag)
                {
                    Wait(8000);
                    string BasketPath = ConfigurationManager.AppSettings["-basketCompliancePath"] + "\\Log\\BasketComplianceLog.log";
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(BasketPath), "BasketLog_backup.log");
                    if (!File.Exists(BasketPath))
                    {
                        throw new Exception("BasketCompliance file doesnot exist!!");
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(BasketPath))
                    {
                        File.Copy(BasketPath, tempFilePath, true);
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("Basket Compliance engine started"))
                            {
                                Console.WriteLine("Basket Compliance hosted");
                                flag = false;
                                Console.WriteLine("Time Taken To Start Basket Compliance : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 40)
                        {
                            Console.WriteLine("Time Taken To Start Basket Compliance : " + stopwatch.ElapsedMilliseconds * 0.001);
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
        /// <summary>
        /// Start Expnl
        /// </summary>
        private void StartExpnl()
        {
            try
            {
                if (string.Equals(ApplicationArguments.AutomationProviderKey, "WINAPP", StringComparison.Ordinal))
                {
                    WinAppUtility.OpenAndMinimizeApp(ApplicationArguments.ExpnlReleasePath + "\\Prana.ExpnlServiceHost.exe");
                }
                else
                {
                    PranaExpnlServiceMinusv2600.WaitForVisible();
                    /* MainForm.WaitForVisible();
                      if (ApplicationArguments.Compression != Convert.ToInt32(SelectCompressionItem.Properties["SelectedIndex"].ToString()))
                      {
                          SelectCompressionItem.Properties["SelectedIndex"] = ApplicationArguments.Compression;
                      }
                      BtnStartExpnl.DoubleClick(MouseButtons.Left);
                      MainForm.WaitForRespondingOrExited();
                      KeyboardUtilities.MinimizeWindow(ref TitleBar4);*/
                    PranaExpnlServiceMinusv2600.WaitForRespondingOrExited();
                    Nirvana.TestAutomation.Utilities.ExtentionMethods.SwitchToPartialWindowTitle(ConfigurationManager.AppSettings["ExpnlServiceWindow"].ToString());
                    KeyboardUtilities.MinimizeWindow(ref TitleBar12);
                    try
                    {
                        ExpnlLogging();
                    }
                    catch {
                        Wait(15000);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartExpnl class", "Service");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartExpnl class]");
            }
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
                        string formattedTime = today.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour
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

        /// <summary>
        /// Start Server
        /// </summary>
        private void StartServer()
        {
            try
            {
                if (string.Equals(ApplicationArguments.AutomationProviderKey, "WINAPP", StringComparison.Ordinal))
                {
                    WinAppUtility.OpenAndMinimizeApp(ApplicationArguments.TradeServiceUIPath + "\\Prana.TradeServiceHost.exe");
                }
                else
                {
                    PranaTradeServiceMinusv2400.WaitForVisible();
                    /*Server.WaitForVisible();
                   /* BtnStartTradeServer.DoubleClick(MouseButtons.Left);
                    Server.WaitForRespondingOrExited();
                    KeyboardUtilities.MinimizeWindow(ref TitleBar2);*/
                    PranaTradeServiceMinusv2400.WaitForRespondingOrExited();
                    Nirvana.TestAutomation.Utilities.ExtentionMethods.SwitchToPartialWindowTitle(ConfigurationManager.AppSettings["TradeServiceWindow"].ToString());
                    KeyboardUtilities.MinimizeWindow(ref TitleBar10);
                    try
                    {
                        ServerLogging();
                    }
                    catch
                    {
                        Wait(10000);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartServer class", "Service");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartServer class]");
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
                        string formattedTime = today.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour
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

        /// <summary>
        /// Start Pricing
        /// </summary>
        private void StartPricing()
        {
            try
            {
                if (string.Equals(ApplicationArguments.AutomationProviderKey, "WINAPP", StringComparison.Ordinal))
                {

                    try
                    {

                        WinAppUtility.OpenAndMinimizeApp(ApplicationArguments.PricingReleasePath + "\\Prana.PricingService2Host.exe");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    PranaPricingServiceMinusv24001.WaitForVisible();
                    /*TitleBar.Click();
                    BtnStartPricing.DoubleClick(MouseButtons.Left);
                    for (int checkFailure = 5; checkFailure > 0; checkFailure--)
                    {
                        if (FailureReportForm.IsVisible)
                        {
                            btnOkOnFailureReport.Click(MouseButtons.Left);
                            Close.Click(MouseButtons.Left);
                            if (Warning.IsVisible)
                                ButtonYes.Click(MouseButtons.Left);
                            OptionCalculatorServer.WaitForVisible();
                            //manual wait is required to start OptionCalculatorServer again
                            Wait(5000);
                            BtnStartPricing.DoubleClick(MouseButtons.Left);
                        }
                        else
                            break;
                    }
                    OptionCalculatorServer.WaitForRespondingOrExited();
                    KeyboardUtilities.MinimizeWindow(ref TitleBar);*/
                    PranaPricingServiceMinusv24001.WaitForRespondingOrExited();
                    Nirvana.TestAutomation.Utilities.ExtentionMethods.SwitchToPartialWindowTitle(ConfigurationManager.AppSettings["PricingServiceWindow"].ToString());
                    KeyboardUtilities.MinimizeWindow(ref TitleBar7);
                    try
                    {
                        PricingLogging();
                    }
                    catch
                    {
                        Wait(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartPricing class", "Service");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartPricing class]");
            }
        }

        private void PricingLogging()
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
                    string PricingServerPath = ConfigurationManager.AppSettings["-pricingReleasePath"] + "\\Logs\\PricingService2_InformationReporter.log";
                    string tempFilePath = Path.Combine(Path.GetDirectoryName(PricingServerPath), "TradeLog_backup.log");
                    if (!File.Exists(PricingServerPath))
                    {
                        Wait(4000);
                        if (!File.Exists(PricingServerPath))
                        {
                            throw new Exception("PricingServerLog file doesnot exist!!");
                        }
                    }
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }

                    if (File.Exists(PricingServerPath))
                    {
                        var today = DateTime.Now;
                        string formattedTime = today.ToString("HH:mm"); //time stamp changed from 12 hour to 24 hour
                        DateTime oneMinuteAhead = today.AddMinutes(1);
                        string oneMinuteAheadTime = oneMinuteAhead.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour

                        DateTime oneMinuteBehind = today.AddMinutes(-1);
                        string oneMinuteBehindTime = oneMinuteBehind.ToString("HH:mm");//time stamp changed from 12 hour to 24 hour
                        File.Copy(PricingServerPath, tempFilePath, true);
                        string[] lines = File.ReadAllLines(tempFilePath);
                        foreach (string line in lines)
                        {
                            if (line.Contains("PricingService2 started ") && (line.Contains(formattedTime) || line.Contains(oneMinuteAheadTime) || line.Contains(oneMinuteBehindTime)))
                            {
                                Console.WriteLine("PricingService Started");
                                flag = false;
                                Console.WriteLine("Time Taken To Start PricingService : " + stopwatch.ElapsedMilliseconds * 0.001);
                                stopwatch.Stop();
                                break;
                            }
                        }
                        retry++;
                        if (retry == 10)
                        {
                            Console.WriteLine("Time Taken To Start PricingService : " + stopwatch.ElapsedMilliseconds * 0.001);
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

        /// <summary>
        /// Start DropCopy
        /// </summary>
        private void StartDropCopy()
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
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartDropCopy class", "Service");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartDropCopy class]");
            }

        }

        /// <summary>
        /// Start Simulator
        /// </summary>
        private void StartSimulator()
        {
            try
            {
               /* if (!ApplicationArguments.SkipSimulatorStartUp)
                {
                    StartFixApplication.Start();
                    Config_TT.Click(MouseButtons.Left);
                    Wait(15000);
                    // Working fine without minimizing the window but need to look for right solution
                    //KeyboardUtilities.MinimizeWindow(ref TitleBar3);
                    SetManualResponse();
                }*/
                // Always Open the simulator without closing it after release restart
                


                #region Check the Opened Simulator and close window if simulator form1 window already opened 
                Process[] simulatorProcesses = Process.GetProcesses();
                bool isOpened = false;
                foreach (Process process in simulatorProcesses)
                {
                    if (process.MainWindowTitle.Equals("MS"))
                    {
                        isOpened = true;
                        if (process.ProcessName.Contains("StartFix"))
                        {
                            process.Kill();
                            break;
                        }

                    }
                    else if(process.ProcessName.Contains("StartFix"))
                    {
                        process.Kill();
                        break;
                    }

                }
                if (isOpened)
                {
                    Console.WriteLine("Simulator Already Opened with MS Title Window");
                    return;
                }
                #endregion
                StartFixApplication.Start();
               /* Wait(3500); //Karan Singh sir 
                IUIAutomationElement appWindow = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Children,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Form1"));*/

                IUIAutomationElement appWindow = null;
                int retry = 0;
                while (appWindow == null && retry < 10)
                {
                    appWindow = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Children,
                        automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Form1"));
                    Thread.Sleep(1000);
                    retry++;
                }
                if (appWindow == null)
                    throw new Exception("Form1 window not found.");

                IUIAutomationCondition buttonCondition = automation.CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId,
                    "config_TT"
                );

                IUIAutomationElement buttonElement = appWindow.FindFirst(
                    TreeScope.TreeScope_Descendants,
                    buttonCondition
                );

                if (buttonElement != null)
                {
                    IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;
                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                    }
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int maxCheck = 0; 
                bool flag = true;
                while (flag)
                {
                    Thread.Sleep(5000);
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
                    maxCheck++;
                    if (maxCheck == 5)
                    {
                        flag = false;
                    }
                }
                SetManualResponse();

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [StartSimulator class]");
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
                    throw new Exception(ex.Message + " [SetManualResponse class]");
            }

        }

        public void ClearSimulator()
        {
            try
            {
                ITestStep SettingManualResp = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, ExcelStructureConstants.CAMERON_SIMULATOR, ExcelStructureConstants.ClearUI);
                TestResult obj = (TestResult)SettingManualResp.RunTest(null, null);
                if (!obj.IsPassed)
                {
                    Log.Error("Setting Clear UI failed.");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [ClearSimulator class]");
            }
        }


        /// <summary>
        /// UI test for application login
        /// </summary>
        [UITest]
        public void ApplicationLogin()
        {

            Stopwatch timer = GetTimerInstance();
            try
            {
                if (Retry == false)
                {
                    /*Wait(10000);
                    Nirvana.TestAutomation.Utilities.ExtentionMethods.SwitchToPartialWindowTitle(ConfigurationManager.AppSettings["NirvanaLoginWindow"].ToString());
                    UserName.Click(MouseButtons.Left);
                    Keyboard.SendKeys(ApplicationArguments.ReleaseUserName);
                    TxtPassword.Click(MouseButtons.Left);
                    Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                    LoginButton.Click(MouseButtons.Left);
                    */
                    EnterpriseLogin();
                    Wait(5000);
                }
                while (Retry == true)
                {
                    try
                    {
                        if (!ApplicationArguments.SkipLogin)
                        {
                            timer.Restart();
                            if (!PranaApplication.IsVisible)
                            {
                                ExtentionMethods.WaitForVisible(ref Login, 80);
                            }
                            else if (PranaApplication.IsVisible)
                            {
                                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                                DataSet workbook = new DataSet();
                                if (cache.Contains("Regression Test Cases"))
                                {
                                    workbook = cache["Regression Test Cases"] as DataSet;
                                    Console.WriteLine("RegressionTestCases DataSet loaded from Cache");
                                }

                                else
                                {
                                    Stopwatch stopwatch = new Stopwatch();
                                    stopwatch.Start();
                                    // get list of sheets in workbook, the provider reads from 5th row and 2nd column to read data as per current test cases file format

                                    workbook = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + ApplicationArguments.Workbook, 5, 2);

                                    stopwatch.Stop();
                                    long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                                    Console.WriteLine("Time taken to get Regression Test Cases dataset: " + elapsedMilliseconds + " ms");
                                    DataSet workbookCopy = workbook.Copy();
                                    cache.Add("Regression Test Cases", workbookCopy, new CacheItemPolicy());
                                }

                                DataRow[] testData = workbook.Tables[ApplicationArguments.SheetName].Select(String.Format(ExcelStructureConstants.COL_TESTCASEID + " = '{0}'", ApplicationArguments.TestCaseToBeRun));


                                DataSet testCaseData = new DataSet();
                                String testCaseFileLocation = ApplicationArguments.TestDataFolderPath;
                                string dataSheetPath = testCaseFileLocation + "\\" + ApplicationArguments.TestCaseToBeRun + "\\" + ApplicationArguments.TestCaseToBeRun + ".xlsx";

                                if (ConfigurationManager.AppSettings["-AllowSkippingofBlankSheets"].ToLower().Equals("true"))
                                {

                                    foreach (var sheets in ConfigurationManager.AppSettings["UndefinedModuleSteps"].Split(',').ToList())
                                    {

                                        UndefinedModuleSteps.Add(sheets);
                                    }
                                    Console.WriteLine("AllowSkippingofBlankSheets list is generated");
                                }
                                if (ConfigurationManager.AppSettings["-AllowDeleteUnnecessaryFiles"].ToLower().Equals("true"))
                                {
                                    Dictionary<string, string> StepValueMapping = DataUtilities.GetTestCaseStepMapping(dataSheetPath, workbook.Tables[ApplicationArguments.SheetName], ApplicationArguments.TestCaseToBeRun);

                                    DataUtilities.DeleteUneccessarySheetsFromWorkbook(dataSheetPath, StepValueMapping, UndefinedModuleSteps);
                                }




                                if (File.Exists(dataSheetPath))
                                // testCaseData = provider.GetTestData(dataSheetPath, 5, 2);   
                                {

                                    if (cache.Contains(ApplicationArguments.TestCaseToBeRun))
                                    {
                                        testCaseData = cache[ApplicationArguments.TestCaseToBeRun] as DataSet;
                                        Console.WriteLine("Taking testsheetdata from cache => " + ApplicationArguments.TestCaseToBeRun);
                                    }

                                    else
                                    {
                                        Stopwatch stopwatch = new Stopwatch();
                                        stopwatch.Start();
                                        testCaseData = DataUtilities.GetTestCaseTestData(dataSheetPath, 5, 2, UndefinedModuleSteps);
                                        stopwatch.Stop();
                                        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                                        Console.WriteLine("Time taken to get TestCase dataset: " + elapsedMilliseconds + " ms");
                                        if (ApplicationArguments.RetrySize > 0)
                                        {
                                            cache.Add(ApplicationArguments.TestCaseToBeRun, testCaseData, new CacheItemPolicy());
                                            Console.WriteLine(ApplicationArguments.TestCaseToBeRun + "added to cache as retry ecexution is twice or more");
                                        }
                                    }
                                }



                                TestResult stepResult = new TestResult();
                                DataRow dr = null;
                                //discardingLoginClient Here AS Same Step is Reproducing in coreexecutor
                                /*if (testData[0].ItemArray[4].Equals(AutomationStepsConstants.LOGIN_CLIENT))   //"LoginClient"
                                {
                                    dr = testData[0];
                                }*/

                                //It checks whether there is any "LoginClient" sheet in the testcase. If not,it opens Client Apllication with its default value i.e. Support1
                                if (dr == null)
                                {
                                    UserName.Click(MouseButtons.Left);
                                    Keyboard.SendKeys(ApplicationArguments.ReleaseUserName);
                                    TxtPassword.Click(MouseButtons.Left);
                                    Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                                    LoginButton.Click(MouseButtons.Left);
                                    Wait(5000);
                                    // Open Compliance UI Before running compliance test cases to sync rule ID in the database 
                                    OpenAndCloseCompliance();

                                }
                                else
                                {
                                    Dictionary<int, string> sheetIndexToName = new Dictionary<int, string>();
                                    // get data required for this step from test case data
                                    DataSet stepData = new DataSet();
                                    for (int i = 1; i <= ExcelStructureConstants.Total_input_sheets; i++)
                                    {
                                        if (!String.IsNullOrWhiteSpace(dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()))
                                        {
                                            DataTable dt = testCaseData.Tables[dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()];
                                            sheetIndexToName.Add(i - 1, dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()); //FOR SheetName
                                            stepData.Tables.Add(dt.Copy());
                                        }
                                    }
                                    ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, "PranaClient", "LoginClient");
                                    stepResult = (TestResult)step.RunTest(stepData, sheetIndexToName);
                                    if (stepResult.IsPassed)
                                    {
                                        Log.Success(AutomationStepsConstants.LOGIN_CLIENT + " passed");
                                    }
                                    else
                                    {
                                        Log.Error(AutomationStepsConstants.LOGIN_CLIENT + " failed");
                                        Log.Error(AutomationStepsConstants.LOGIN_CLIENT + "_" + AutomationStepsConstants.PRANA_CLIENT + " failed.");

                                    }
                                    OpenAndCloseCompliance();
                                }
                            }
                        }
                        //PranaMain.WaitForVisible();
                        if (!PranaMain.IsVisible)
                        {
                            ExtentionMethods.WaitForVisible(ref PranaMain, 80);
                        }
                        else if (PranaMain.IsVisible && LblConnectionStatus.Text.Equals("Trade Engine", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Log.Success("Logged in sucessfully.");
                            TestStatusLog.TestCaseResult(string.Empty, "0", MessageConstants.LOGIN_TO_APPLICATION, String.Empty, String.Empty, timer.Elapsed);
                        }

                        Retry = false;
                        count += 1;
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        ++RetryCounter;
                        if (RetryCounter == MaxTries)
                        {
                            Log.Error("Failed to log in. Reason : " + ex.Message);
                            bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                            if (rethrow)
                                throw;
                            TestStatusLog.TestCaseResult(string.Empty, "0", MessageConstants.LOGIN_TO_APPLICATION, ex.Message, String.Empty, timer.Elapsed);
                            // SaveAndExit(102);
                            SaveAndExit(0);
                        }
                        else
                        {
                            Retry = true;

                            KillPranaProcess();
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                ++RetryCounter;
                if (RetryCounter == MaxTries)
                {
                    Log.Error("Failed to log in. Reason : " + ex.Message);
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                    if (rethrow)
                        throw;
                    TestStatusLog.TestCaseResult(string.Empty, "0", MessageConstants.LOGIN_TO_APPLICATION, ex.Message, String.Empty, timer.Elapsed);
                    // SaveAndExit(102);
                    SaveAndExit(0);
                }

            }

        }
        private void KillPranaProcess()
        {
            try
            {
                if (PranaMain.IsVisible)
                {
                    ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                    if (SaveLayout.IsVisible)
                    {
                        ButtonNo.Click(MouseButtons.Left);
                    }
                }
                Wait(2000);
                Process[] processes = Process.GetProcessesByName("Prana");
                foreach (Process process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error killing the 'Prana' process: " + ex.Message);
            }
        }
        /// <summary>
        /// Save logs and exits the code
        /// </summary>
        /// <param name="exitCode"></param>
        private static void SaveAndExit(int exitCode)
        {
            try
            {
                TestStatusLog.PublishLog();
                ApplicationArguments.ExitCode = exitCode;
                Log.Success("Save and exit the application successfully.");
            }
            catch (Exception ex)
            {
                Log.Error("Save and exit the application failed: " + ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception("Save and exit the application failed: " + ex.Message);
            }
        }
        static Dictionary<string, int> totalFailedResults = new Dictionary<string, int>();
        static public bool IsCAandGLCasesResolutionIssue = false;
        /// <summary>
        /// UI test for user defined cases
        /// </summary>
        [UITest]
        public void UserDefinedTestCases()
        {
            try
            {
                if (ApplicationArguments.RunCleanUp)
                {
                    RunCleanUp();
                }
                else
                {

                    ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);

                    DataSet dtModulesAndStepsMapping = provider.GetTestData(ApplicationArguments.StepMappingFilePath + ExcelStructureConstants.FILE_STEPS_MAPPING, 3, 1);
                    _modulesAndStepsMapping = new List<string>();
                    for (int counter = 0; counter < dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows.Count; counter++)
                    {
                        if (!_modulesAndStepsMapping.Contains(dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_MODULES].ToString() + "_" + dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_STEPS].ToString()))
                        {
                            _modulesAndStepsMapping.Add(dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_MODULES].ToString() + "_" + dtModulesAndStepsMapping.Tables[ExcelStructureConstants.COL_MODULES].Rows[counter][ExcelStructureConstants.COL_STEPS].ToString());
                        }
                    }

                    // get list of sheets in workbook, the provider reads from 5th row and 2nd column to read data as per current test cases file format

                    DataSet workbook = new DataSet();

                    if (cache.Contains("Regression Test Cases"))
                    {
                        Console.WriteLine("RegressionTestCases DataSet loaded from Cache");
                        workbook = cache["Regression Test Cases"] as DataSet;

                    }
                    else
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        // get list of sheets in workbook, the provider reads from 5th row and 2nd column to read data as per current test cases file format
                        workbook = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + ApplicationArguments.Workbook, 5, 2);

                        stopwatch.Stop();
                        long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                        Console.WriteLine("Time taken to get Regression Test Cases dataset: " + elapsedMilliseconds + " ms");
                        DataSet workbookCopy = workbook.Copy();
                        cache.Add("Regression Test Cases", workbookCopy, new CacheItemPolicy());

                    }
                    //workbook= provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + ApplicationArguments.Workbook, 5, 2);

                    String testCaseFileLocation = ApplicationArguments.TestDataFolderPath;

                    //fill up the test case id in merged cells
                    String testCase = string.Empty;
                    String Col_Module = string.Empty;
                    String testCaseDescription = string.Empty;
                    foreach (DataRow dr in workbook.Tables[ApplicationArguments.SheetName].Rows)
                    {
                        if (String.IsNullOrWhiteSpace(dr[ExcelStructureConstants.COL_TESTCASEID].ToString()))
                        {
                            dr[ExcelStructureConstants.COL_TESTCASEID] = testCase;
                            ApplicationArguments.SheetName = Col_Module;
                        }
                        else
                            testCase = dr[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        Col_Module = ApplicationArguments.SheetName;
                    }

                    Stopwatch timer = GetTimerInstance();
                    string category = string.Empty;
                    string moduleName = String.Empty;
                    string stepName = String.Empty;
                    try
                    {
                        if (string.Equals(ApplicationArguments.testcaseTracker.portfolioDBRestoreFail, "true", StringComparison.OrdinalIgnoreCase))
                        {
                            ApplicationArguments.testcaseTracker.portfolioDBRestoreFail = string.Empty;
                            throw new Exception("Portfolio DB doesnot Exist" );
                        }

                        DataRow[] testData = workbook.Tables[ApplicationArguments.SheetName].Select(String.Format(ExcelStructureConstants.COL_TESTCASEID + " = '{0}'", ApplicationArguments.TestCaseToBeRun));

                            foreach (DataRow dr in testData)
                            {
                                foreach (var defect in ConfigurationManager.AppSettings["CaseSpecificIssues"].Split(',').ToList())
                                {
                                    if (dr[4].ToString().Equals(defect))
                                    {
                                        if (TestExecutor.IsMachineFaulty(TestStatusLog.GetSytemIP().ToString(), ConfigurationManager.AppSettings["CaseSpecificIssues"].Split(',').ToList(), false))
                                        {
                                            TestExecutor.UpdateStatus(MessageConstants.MACHINE_ERROR);
                                            ApplicationArguments.ExitCode = 0;
                                            return;
                                        }
                                    }
                                }
                            }

                        ApplicationArguments.testcaseTracker.TestCaseTrackerClear(); 
                        Log.Information("Running test case : " + ApplicationArguments.TestCaseToBeRun);
                        ApplicationArguments.GlobalErrorList = new List<string>();
                        ApplicationArguments.GlobalStepCounter = 0;
                        ApplicationArguments.testcaseTracker.TestCaseID = ApplicationArguments.TestCaseToBeRun;
                        ApplicationArguments.testcaseTracker.PreRequisiteType = ApplicationArguments.isPreRequisiteType;
                        prefStepExistedonStepList = false;
                        moduleName = MessageConstants.MODULE_APPLICATION;
                        stepName = MessageConstants.APPLICATION_START_UP;
                        if (!ApplicationArguments.SkipStartUp)
                            ApplicationStartUp();
                        stepName = MessageConstants.LOGIN_TO_APPLICATION;

                        ConfigurationManager.AppSettings["IsMultiUser"] = "true";
                        string[] StepsList = ConfigurationManager.AppSettings["StepToPerformWithSingleUser"].Split(',');
                        bool multiUserflag = true;
                        List<string> stepForGTD = new List<string>();
                        if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                        {
                            foreach (var row in testData)
                            {
                                if (StepsList.Contains(row[4].ToString()))
                                {
                                    ConfigurationManager.AppSettings["IsMultiUser"] = "false";
                                    ApplicationArguments.SamsaraReleaseUserName = ConfigurationManager.AppSettings["-firstUser"].ToString();//Support1
                                    ApplicationArguments.SamsaraActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                                    multiUserflag = false;
                                    break;
                                }
                            }
                            if (multiUserflag)
                            {
                                /// taki next case me impact na aajae
                                ApplicationArguments.SamsaraReleaseUserName = ConfigurationManager.AppSettings["-samsaraMultiUser"].ToString();
                            }
                        }
                        foreach (var row in testData)
                        {
                            stepForGTD.Add(row[4].ToString());
                        }
                        if (stepForGTD.Contains("ChangeDate") && !stepForGTD.Contains("UpdateServerConfig"))
                            throw new Exception("Case is GTD/GTC type but TradeServer config is not Updated for Simulator");
                        if (stepForGTD.Contains("VerifyToastMessage"))
                        {
                            ConfigurationManager.AppSettings["ToastVerify"] = "true";
                        }
                        else {
                            ConfigurationManager.AppSettings["ToastVerify"] = "false";
                        }
                        if (!ApplicationArguments.SkipLogin)
                        {

                            if (ConfigurationManager.AppSettings["IsMultiUser"].ToString() != "true")
                            {
                                if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                                {
                                    SamsaraApplicationLogin();
                                    ApplicationArguments.CurrentlyActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                                    ApplicationArguments.RequiredActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                                    System.Threading.Thread.Sleep(10000);
                                    ProcessControlManager.MinimizeAllWindowsExceptSpecified();
                                }

                                else
                                {
                                    ApplicationLogin();//onlyenterpriseCases
                                    ProcessControlManager.MinimizeAllWindowsExceptSpecified();
                            	}

                            }
                            else
                            {
                                if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                                {
                                    SamsaraApplicationLogin();
                                    string sourceFolder = ConfigurationManager.AppSettings["sourceFolderPref"];
                                    string destinationFolder = ConfigurationManager.AppSettings["destinationFolderPref"];
                                    if (Directory.Exists(destinationFolder))
                                    {
                                        Directory.Delete(destinationFolder, true); // 'true' deletes all contents recursively
                                        Console.WriteLine("Folder deleted successfully.");
                                    }
                                    CopyFolder(sourceFolder, destinationFolder);
                                    System.Threading.Thread.Sleep(10000);
                                    ApplicationLogin();
                                }
                                else
                                {
                                    ApplicationLogin();
                                }

                            }


                        }
                        CheckActiveUserDuringAutomation();

                        category = testData[0]["Category"].ToString();

                        //get data required for test case, the provider reads from 5th row and 2nd column to read data as per current test cases file format
                        DataSet testCaseData = new DataSet();
                        string dataSheetPath = testCaseFileLocation + "\\" + ApplicationArguments.TestCaseToBeRun + "\\" + ApplicationArguments.TestCaseToBeRun + ".xlsx";
                        if (File.Exists(dataSheetPath))
                        {
                            if (cache.Contains(ApplicationArguments.TestCaseToBeRun))
                            {
                                testCaseData = cache[ApplicationArguments.TestCaseToBeRun] as DataSet;
                                Console.WriteLine("Taking testsheetdata from cache => " + ApplicationArguments.TestCaseToBeRun);
                            }
                            else
                            {
                                testCaseData = provider.GetTestData(dataSheetPath, 5, 2, "testCase");
                            }

                            if (ConfigurationManager.AppSettings["-TestCaseFixingMode"].ToLower().Equals("true"))
                            {
                                ApplicationArguments.GlobalTestCaseDataSet = testCaseData.Copy();
                            }

                        }
                        if (ConfigurationManager.AppSettings["ACCenvironment"].ToString().Equals("true") && ConfigurationManager.AppSettings["IsMultiUser"].ToString().Equals("true") && ApplicationArguments.ProductDependency.ToLower().Equals("samsara"))
                        {
                            KillPranaProcess();
                            Console.WriteLine("ACC envrionment found, Hence changing case into SingleUser scenario");
                            ConfigurationManager.AppSettings["IsMultiUser"] = "false";
                            LogOutSamsaraWithVerification();
                            StartSamsaraYarnStart();
                            ApplicationArguments.CurrentlyActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                            ApplicationArguments.RequiredActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                            SamsaraApplicationLoginWithRequiredUser();

                        }
                        timer.Restart();
                        TestResult stepResult = new TestResult();
                        TestResult stepResult1 = new TestResult();
                        //Check the step that can perform with multiuser or not
                       
                        //loop for each step and perform step action
                        foreach (DataRow dr in testData)
                        {
                            moduleName = dr[ExcelStructureConstants.COL_MODULE].ToString();
                            stepName = dr[ExcelStructureConstants.COL_STEP].ToString();
                            try
                            {
                                ApplicationArguments.ActiveInputStep = dr[ExcelStructureConstants.COL_INPUT_SHEET1].ToString();
                            
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            if (!ApplicationArguments.ProductDependency.ToLower().Equals("samsara") && (moduleName.Equals("RTPNL") || moduleName.Equals("OpenFin")))
                            {
                                Console.WriteLine(stepName + " step skipped as ProductDependency is not samsara and Step is samsara specific");
                                continue;
                            }
                            if (testCaseDescription.Equals(string.Empty))
                                testCaseDescription = dr[ExcelStructureConstants.COL_DESCRIPTION].ToString();
                            Dictionary<int, string> sheetIndexToName = new Dictionary<int, string>();
                            // get data required for this step from test case data
                            DataSet stepData = new DataSet();
                            for (int i = 1; i <= ExcelStructureConstants.Total_input_sheets; i++)
                            {
                                //if (!String.IsNullOrWhiteSpace(dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()) && !UndefinedModuleSteps.Contains(dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()))
                                if (!String.IsNullOrWhiteSpace(dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()) && !ConfigurationManager.AppSettings["UndefinedModuleSteps"].Split(',').Contains(dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()))
                                {
                                    DataTable dt = testCaseData.Tables[dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString()];

                                    sheetIndexToName.Add(i - 1, dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString());
                                    try
                                    {
                                        stepData.Tables.Add(dt.Copy());

                                     
                                    }
                                    catch {
                                        throw new Exception("Reference error at step: " + dr[ExcelStructureConstants.COL_INPUT_SHEET + i].ToString());
                                    }

                                    

                                }
                            }
                            //skip fix steps as per config
                            ApplicationArguments.GlobalStepCounter++;
                            if (ConfigurationManager.AppSettings["-AllowFixSteps"].ToLower().Equals("false") && ApplicationArguments.GlobalStepCounter == 1)
                            {
                                stepResult = ExecuteStep("Simulator", "ClearAllFixLogs", null, sheetIndexToName);
                            }
                            //run the test step
                            //Exception handling new returning class instead of bool [Interface modification]
                            if (_modulesAndStepsMapping.Contains(moduleName + "_" + stepName))
                            {
                                if (ConfigurationManager.AppSettings["-AllowFixSteps"].ToLower().Equals("false") && ApplicationArguments.SkipFixSteps.Contains(stepName))
                                {
                                    Console.WriteLine(stepName + " step skipped as AllowFixSteps is set to false and SkipFixSteps contains " + stepName);
                                    continue;
                                }

                                if (ApplicationArguments.ProductDependency.ToLower().Equals("samsara") && (stepName.Equals(AutomationStepsConstants.CLEAN_UP) || (prefStepExistedonStepList == true && stepName.ToLower().Contains("restart"))))
                                {
                                    Console.WriteLine(stepName + " step skipped");
                                    continue;
                                }
                                bool value = SkipSteps(stepName);
                                if(value.Equals(true))
                                    continue;
                                if (ApplicationArguments.PrefRestartList.Contains(stepName))
                                {
                                    prefStepExistedonStepList = true;
                                }
                                else
                                {
                                    prefStepExistedonStepList = false;
                                }
                                if (ConfigurationManager.AppSettings["IsMultiUser"].ToString() != "true")
                                {
                                    if (moduleName.ToUpper().Equals("SIMULATOR") && !stepName.Equals("VerifyRowAllFixLogs"))
                                    {
                                        //ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                        stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                        continue;
                                    }
                                    if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                    {
                                        if (ApplicationArguments.runType.ToLower() == "samsara")
                                        {
                                            bool IsStepCreatedInSamsara = SamsaraHelperClass.checkStepIspresentOrNot(stepName);
                                            bool isControlShiftedToEnterprise = checkTestCaseWithStep(ref stepName, ref ApplicationArguments.StepProductTypeControlHandler, ApplicationArguments.TestCaseToBeRun);

                                            if (IsStepCreatedInSamsara && (!isControlShiftedToEnterprise))
                                            {
                                                string StepType = ApplicationArguments._ModuleStepMapping[moduleName + stepName].ToString();
                                                bool result_case = true;


                                                if (String.Equals(TestDataConstants.RestartServer, stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        LogOutSamsaraWithVerification();
                                                        ShutDownAndRestartServices();
                                                        if (prefStepExistedonStepList == true)
                                                            prefStepExistedonStepList = false;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartServer..");
                                                    }
                                                }
                                                else if (String.Equals(TestDataConstants.RestartClient, stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        KillPranaProcess();
                                                        SamsaraHelperClass.LogOutUser();
                                                        SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                                        ApplicationArguments.RequiredActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString(); 
                                                        //onlyYarnStart
                                                        StartSamsaraYarnStart();
                                                        SamsaraHelperClass.RunOpenFinTest("samsara", "", "", stepName);
                                                        LoginSamsaratype = true;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartClient..");
                                                    }
                                                    
                                                }
                                                else if (String.Equals(TestDataConstants.LoginClient, stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        KillPranaProcess();
                                                        SamsaraHelperClass.LogOutUser();
                                                        SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                                        ApplicationArguments.RequiredActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                                                        //onlyYarnStart
                                                        StartSamsaraYarnStart();
                                                        if (SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed)
                                                        {
                                                            Log.Success("Samsara " + stepName + " passed");
                                                            LoginSamsaratype = true;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartClient..");
                                                    }

                                                }
                                                else if (String.Equals("RestartReleasesAndServices", stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        SamsaraHelperClass.LogOutUser();
                                                        SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                                        ApplicationArguments.RequiredActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString(); 
                                                        ShutDownAndRestartServices(stepName, stepData);
                                                        StartSamsaraYarnStart();
                                                        Thread.Sleep(6000);
                                                        SamsaraHelperClass.RunOpenFinTest("samsara", "", "", stepName);
                                                        Thread.Sleep(6000);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartReleasesAndServices..");
                                                    }

                                                }

                                                else if (StepType == "Samsara")
                                                {

                                                    if (!LoginSamsaratype)
                                                    {
                                                        KillPranaProcess();
                                                        LogOutSamsaraWithVerification();
                                                        StartSamsaraYarnStart();
                                                        SamsaraApplicationLoginWithRequiredUser();
                                                        Wait(5000);
                                                        LoginSamsaratype = true;
                                                        if (SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed)
                                                        {
                                                            Log.Success("Samsara " + stepName + " passed");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result_case = SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed;
                                                        LoginSamsaratype = true;
                                                    }
                                                }

                                                else if (StepType != "Input")
                                                {
                                                    //output ya select hoga
                                                    if (!LoginSamsaratype)
                                                    {
                                                        KillPranaProcess();
                                                        LogOutSamsaraWithVerification();
                                                        // login samsara
                                                        StartSamsaraYarnStart();
                                                        SamsaraApplicationLoginWithRequiredUser();
                                                        Wait(5000);
                                                        LoginSamsaratype = true;
                                                        //need to check whether prana application is closed or not here

                                                        result_case = SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed;
                                                    }
                                                    else
                                                    {
                                                        result_case = SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed;
                                                        LoginSamsaratype = true;
                                                    }

                                                    if (result_case)
                                                    {

                                                        Log.Success("Samsara " + stepName + " passed");
                                                       /* try
                                                        {
                                                            if (ApplicationArguments.RestartActionList.Contains(stepName))
                                                            {
                                                                ITestStep tempStep = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, "PranaClient", "RestartClient");
                                                                //LoginSamsaratype should be false here
                                                                LoginSamsaratype = false;
                                                                stepResult = (TestResult)tempStep.RunTest(stepData, sheetIndexToName);
                                                                if (stepResult.IsPassed)
                                                                {
                                                                    Log.Success(AutomationStepsConstants.LOGIN_CLIENT + " passed");
                                                                }
                                                                else
                                                                {
                                                                    Log.Error(AutomationStepsConstants.LOGIN_CLIENT + " failed");
                                                                    Log.Error(AutomationStepsConstants.LOGIN_CLIENT + "_" + AutomationStepsConstants.PRANA_CLIENT + " failed.");

                                                                }
                                                            }
                                                        }

                                                        catch (Exception ex) { Console.Write(ex.Message); }*/
                                                    }
                                                    if (!ApplicationArguments.PopUpStepsList.Contains(stepName))
                                                    {
                                                        LoginSamsaratype = false;
                                                        SamsaraHelperClass.LogOutUser();
                                                        //ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                                        if (!PranaMain.IsVisible)
                                                        {
                                                            EnterpriseLogin();
                                                        }
                                                        else
                                                            Console.WriteLine("Prana application is already login");
                                                        stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                                    }
                                                }
                                               
                                                else
                                                {
                                                    if (!LoginSamsaratype)
                                                    {
                                                        KillPranaProcess();
                                                        LogOutSamsaraWithVerification();
                                                        StartSamsaraYarnStart();
                                                        SamsaraApplicationLoginWithRequiredUser();
                                                        LoginSamsaratype = true;
                                                        Wait(5000);
                                                        SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName);
                                                    }
                                                    else
                                                    {
                                                        SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName);
                                                        LoginSamsaratype = true;
                                                    }

                                                    if (prefStepExistedonStepList == true)
                                                    {
                                                        try
                                                        {
                                                            handlePrefStepsPostActions();
                                                            LoginSamsaratype = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message + "on handlePrefStepsPostActions..");
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                
                                                //ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                                if (!PranaMain.IsVisible)
                                                {
                                                    LoginSamsaratype = false;
                                                    bool isSamsaraUp = CheckSamsaraUp();

                                                    if (isSamsaraUp)
                                                    {

                                                        SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Both Applications are UP");
                                                        Console.WriteLine("Both Applications are UP!!");
                                                        Console.WriteLine("ClosingSamsaraApplication!!");
                                                        LogOutSamsaraWithVerification();
                                                        LoginSamsaratype = false;
                                                    }
                                                    EnterpriseLogin();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Prana application is already login");
                                                    try
                                                    {
                                                        bool isSamsaraUp = CheckSamsaraUp();

                                                        if (isSamsaraUp)
                                                        {
                                                            SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Both Applications are UP");
                                                            Console.WriteLine("Both Applications are UP!!");
                                                            Console.WriteLine("ClosingSamsaraApplication!!");
                                                            LogOutSamsaraWithVerification();
                                                            LoginSamsaratype = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //can throw exception here..
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                                //stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                                stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                                LoginSamsaratype = false;

                                                if (prefStepExistedonStepList == true)
                                                {
                                                    try
                                                    {
                                                        handlePrefStepsPostActions();
                                                        LoginSamsaratype = true;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on handlePrefStepsPostActions..");
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    else
                                    {
                                        //ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                        if (!PranaMain.IsVisible)
                                        {
                                            EnterpriseLogin();
                                            LoginSamsaratype = false;

                                        }
                                        else
                                        {
                                            Console.WriteLine("Prana application is already login");
                                            try
                                            {
                                                bool isSamsaraUp = CheckSamsaraUp();

                                                if (isSamsaraUp)
                                                {
                                                    SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                                                    Console.WriteLine("Both Applications are UP!!");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //can throw exception here..
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                        LoginSamsaratype = false;
                                    }
                                }
                                else
                                {
                                    if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                    {
                                        if (ApplicationArguments.runType.ToLower() == "samsara")
                                        {
                                            bool IsStepCreatedInSamsara = SamsaraHelperClass.checkStepIspresentOrNot(stepName);
                                            bool isControlShiftedToEnterprise = checkTestCaseWithStep(ref stepName, ref ApplicationArguments.StepProductTypeControlHandler, ApplicationArguments.TestCaseToBeRun);
                                            if (IsStepCreatedInSamsara && (!isControlShiftedToEnterprise))
                                            {
                                                string StepType = ApplicationArguments._ModuleStepMapping[moduleName + stepName].ToString();

                                                if (String.Equals(TestDataConstants.RestartServer, stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        LogOutSamsaraWithVerification();
                                                        ShutDownAndRestartServices();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartServer..");
                                                    }

                                                }
                                                else if (String.Equals(TestDataConstants.RestartClient, stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        SamsaraHelperClass.LogOutUser();
                                                        SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                                        //onlyYarnStart
                                                        StartSamsaraYarnStart();
                                                        SamsaraHelperClass.RunOpenFinTest("samsara", "", "", stepName);
                                                        EnterpriseLogin();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartClient..");
                                                    }

                                                }
                                                else if (String.Equals("RestartReleasesAndServices", stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    try
                                                    {
                                                        SamsaraHelperClass.LogOutUser();
                                                        SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                                        ShutDownAndRestartServices(stepName, stepData);
                                                        StartSamsaraYarnStart();
                                                        Thread.Sleep(6000);
                                                        SamsaraHelperClass.RunOpenFinTest("samsara", "", "", stepName);
                                                        Thread.Sleep(6000);
                                                        EnterpriseLogin();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on RestartReleasesAndServices..");
                                                    }

                                                }
                                                else if (String.Equals("OpenReleaseClient", stepName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    SamsaraHelperClass.LogOutUser();
                                                    KillPranaProcess();
                                                    ApplicationArguments.SamsaraReleaseUserName = ConfigurationManager.AppSettings["-firstUser"].ToString();//Support1
                                                    ApplicationArguments.SamsaraActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                                                    string[] release = stepData.Tables[0].Rows[0]["ReleaseName"].ToString().Split(',');
                                                    if (release[0].Equals("Prana"))
                                                    {
                                                        EnterpriseLogin();
                                                        StartSamsaraYarnStart();
                                                        SamsaraHelperClass.RunOpenFinTest("samsara", "", "", "Login");
                                                    }
                                                    else {
                                                        StartSamsaraYarnStart();
                                                        SamsaraHelperClass.RunOpenFinTest("samsara", "", "", "Login");
                                                        Thread.Sleep(5000);
                                                        EnterpriseLogin();
                                                    
                                                    }
                                                    
                                                }


                                                else if (StepType == "Samsara")
                                                {
                                                    if (SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed)
                                                    {
                                                        Log.Success("Samsara " + stepName + " passed");
                                                    }
                                                }
                                                else if (StepType != "Input")
                                                {
                                                    if (SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName).IsPassed)
                                                    {
                                                        Log.Success("Samsara " + stepName + " passed");
                                                    }
                                                    try
                                                    {
                                                        if (ApplicationArguments.RestartActionList.Contains(stepName))
                                                        {
                                                            ITestStep tempStep = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, "PranaClient", "RestartClient");

                                                            stepResult = (TestResult)tempStep.RunTest(stepData, sheetIndexToName);
                                                            if (stepResult.IsPassed)
                                                            {
                                                                Log.Success(AutomationStepsConstants.LOGIN_CLIENT + " passed");
                                                            }
                                                            else
                                                            {
                                                                Log.Error(AutomationStepsConstants.LOGIN_CLIENT + " failed");
                                                                Log.Error(AutomationStepsConstants.LOGIN_CLIENT + "_" + AutomationStepsConstants.PRANA_CLIENT + " failed.");

                                                            }
                                                        }
                                                    }

                                                    catch (Exception ex) { Console.Write(ex.Message + "  LoginClient Failed after - VerificationStepOnSamsara"); }

                                                    if (!ApplicationArguments.PopUpStepsList.Contains(stepName))
                                                    {
                                                        //ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                                        stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                                    }
                                                }
                                                else
                                                {
                                                    SamsaraHelperClass.GenericMethod(moduleName, stepData, stepName);
                                                }
                                            }
                                            else
                                            {
                                               // ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                                stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                                if (prefStepExistedonStepList == true)
                                                {
                                                    try
                                                    {
                                                        handlePrefStepsPostActions();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + "on handlePrefStepsPostActions..");
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (stepName.Equals(AutomationStepsConstants.RESTART_RELEASE_AND_SERVICES))
                                        {
                                            if (stepData.Tables[0].Columns.Contains("DontShutSimulator") && !string.IsNullOrEmpty(stepData.Tables[0].Rows[0]["DontShutSimulator"].ToString()))
                                            {
                                                stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                                continue;
                                            }
                                            if (!ApplicationArguments.SkipBasketCompliance || !ApplicationArguments.SkipCompliance)
                                            {
                                                ShutDownAndRestartServices();
                                                EnterpriseLogin();
                                                continue;
                                            }
                                        }
                                        else if (stepName.Equals(AutomationStepsConstants.RESTART_SERVERONLY))
                                        {
                                            stepName = AutomationStepsConstants.RESTART_SERVER.ToString();
                                        }
                                        else if (stepName.Equals(AutomationStepsConstants.RESTART_SERVER))
                                        {
                                            if (!ApplicationArguments.SkipBasketCompliance || !ApplicationArguments.SkipCompliance)
                                            {
                                                ShutDownAndRestartServices();
                                                EnterpriseLogin();
                                                continue;
                                            }
                                        }
                                       // ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
                                        stepResult = ExecuteStep(moduleName, stepName, stepData, sheetIndexToName);
                                    }
                                }

                                if (stepResult.IsPassed)
                                {
                                    previousStep = stepName;
                                    Log.Success(stepName + " passed");
                                }
                                else
                                {
                                    if (ApplicationArguments.runType.ToLower() == "samsara")
                                    {
                                        SamsaraHelperClass.CaptureMyScreen(stepName, ApplicationArguments.TestCaseToBeRun, stepName + " failed");
                                    }
                                    Log.Error(stepName + " failed");
                                    Log.Error(moduleName + "_" + stepName + " failed.");
                                    break;
                                      
                                }
                            }
                        }

                        if (ConfigurationManager.AppSettings["-TestCaseFixingMode"].ToLower().Equals("true") && (ApplicationArguments.GlobalErrorList.Count > 0 || ConfigurationManager.AppSettings["-FixPassCases"].ToLower().Equals("true")))
                        {
                            if (ApplicationArguments.GlobalErrorList.Count > 0)
                            {
                                stepResult.IsPassed = false;

                                StringBuilder sb = new StringBuilder();
                                HashSet<string> seenErrors = new HashSet<string>();

                                foreach (string err in ApplicationArguments.GlobalErrorList)
                                {
                                    if (!seenErrors.Contains(err))
                                    {
                                        sb.AppendLine(err);
                                        seenErrors.Add(err);
                                    }
                                }

                                stepResult.ErrorMessage = sb.ToString();
                            }
                            try
                            {
                                string fileBasePath =  ConfigurationManager.AppSettings["TestCaseFixingModeBaseFolder"];
                                TestCaseFixingMode.FixTestCase(fileBasePath, ApplicationArguments.GlobalTestCaseDataSet, testCaseData.Copy(), ApplicationArguments.TestCaseToBeRun);

                                if (ConfigurationManager.AppSettings["-DeleteCasesifCaseNotExistinReport"].ToLower().Equals("true") )
                                {
                                   CoreModeHelper.DeleteCaseifCaseNotExistinReport();
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred while executing TestCaseFixingMode CaseCreation Process: " + ex.Message);
                                if (ex.InnerException != null)
                                {
                                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                                    Console.WriteLine("Inner Exception Stack Trace: " + ex.InnerException.StackTrace);
                                }
                                throw new Exception("An error occurred while executing TestCaseFixingMode CaseCreation Process: " + ex.Message);
                            }
                            
                        }

                        if (stepResult.IsPassed)
                        {

                            Log.Success(ApplicationArguments.TestCaseToBeRun + " completed successfully.");
                            ApplicationArguments.testcaseTracker.Result = "Pass";
                            if (ConfigurationManager.AppSettings["-AllowFixSteps"].ToLower().Equals("false"))
                            {
                                try
                                {
                                    CopyFixLogsForTestCase(ApplicationArguments.TestCaseToBeRun);  
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine("An error occurred while executing FixLogs Copy Process: " + ex.Message);
                                    if (ex.InnerException != null)
                                    {
                                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                                        Console.WriteLine("Inner Exception Stack Trace: " + ex.InnerException.StackTrace);
                                    }
                                    throw new Exception("An error occurred while executing FixLogs Copy Process: " + ex.Message);
                                }
                            }

                            TestStatusLog.TestCaseResult(Col_Module, ApplicationArguments.TestCaseToBeRun, testCaseDescription, String.Empty, category, timer.Elapsed);
                            TestExecutor.UpdateStatus(String.Empty);
                            timer.Stop();
                            TestExecutor.CopyLogsToMaster();
                            TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Pass");
                            SaveAndExit(0);
                        }
                        else
                        {
                            
                            Console.WriteLine(ApplicationArguments.TestCaseToBeRun + " completed with failure.");
                            Console.WriteLine("Cause for failure : " + stepResult.ErrorMessage);
                            ApplicationArguments.testcaseTracker.Result = "Cause for failure : " + stepResult.ErrorMessage;
                            ApplicationArguments.testcaseTracker.Log = stepResult.ErrorMessage;
                            TestStatusLog.PublishLog();
                            string message = ErrorMessages.getErrorMessages(moduleName, stepName, stepResult.ErrorMessage);
                            TestStatusLog.TestCaseResult(Col_Module, ApplicationArguments.TestCaseToBeRun, testCaseDescription, message, category, timer.Elapsed);
                            TestExecutor.UpdateStatus(message);
                            timer.Stop();
                            TestExecutor.CopyLogsToMaster();
                            TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Fail");
                            string[] ResolutionCAandGLissues = ConfigurationManager.AppSettings["resolutionIssuesForCAandGLCases"].Split(',');
                            foreach (var issue in ResolutionCAandGLissues)
                            {
                                String InternalErrorMessage = ConfigurationManager.AppSettings["resolutionissueInternalMessage"];
                                if (message.Contains(issue))
                                {
                                    if (!message.Contains(InternalErrorMessage))
                                    {
                                        IsCAandGLCasesResolutionIssue = true;
                                    }
                                }
                            }
                            SaveAndExit(0);

                        }
                        if (ApplicationArguments.RunCleanUpAfterTestCase)
                        {
                            RunCleanUp();
                        }
                    }
                    catch (Exception ex)
                    {
                        TestExecutor.UpdateStatus(ex.Message);
                        if (ex.Message.Contains("is not running.") && ApplicationArguments.runType.ToLower() == "samsara")
                        {
                            LogOutSamsaraWithVerification();
                            ShutDownAndRestartServices();
                        }
                        // If Test Cancelled By User
                        if (ex.Message.Contains("Test cancelled by user."))
                        {

                            Log.Error("Test cancelled by user.");
                            Log.Error(moduleName + "_" + stepName + " failed.");
                            TestStatusLog.PublishLog();
                            TestStatusLog.TestCaseResult(Col_Module, ApplicationArguments.TestCaseToBeRun, testCaseDescription, ex.Message, category, timer.Elapsed);
                            TestExecutor.UpdateStatus(ex.Message);
                            TestExecutor.CopyLogsToMaster();
                            TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Fail");
                            timer.Stop();
                            SaveAndExit(101); // 101 : cancelled by user
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["-CodeSpecificIssue"]))
                            {

                                List<string> issueList = ConfigurationManager.AppSettings["-CodeSpecificIssue"].ToString().Split(',').ToList();
                                foreach (string codeIssue in issueList)
                                {
                                    if (ex.Message.Contains(codeIssue))
                                    {
                                        try
                                        {
                                            List<string> fileColumnList = new List<string> { "IP", "Reason", "TestCase", "UpdatingTime" };
                                            DataTable dataTable = new DataTable("CodeSpecificIssue");
                                            DataUtilities.UpdateColumnsInDataTable(ref fileColumnList, ref dataTable, true);
                                            DataRow newRow = dataTable.NewRow();
                                            List<string> datarow = new List<string> { TestStatusLog.GetSytemIP().ToString(), codeIssue, ApplicationArguments.TestCaseToBeRun, DateTime.Now.ToString() };
                                            for (int i = 0; i < dataTable.Columns.Count; i++)
                                            {
                                                newRow[i] = datarow[i];
                                            }
                                            dataTable.Rows.Add(newRow);
                                            string filepath = ConfigurationManager.AppSettings["-masterMachinePath"].ToString();
                                            string fileName = dataTable.TableName.ToString();
                                            DataUtilities.CreateOrUpdateExcelFile(ref fileName, ref dataTable, ref filepath);

                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Error while generating file -CodeSpecificIssue" + ex.Message);

                                        }
                                    }
                                }


                                if ((moduleName + "_" + stepName + " failed.") == "Prana Application_Login to the application failed.")
                                {
                                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MachineIssues"]))
                                    {
                                        TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Fail");
                                        SaveAndExit(1);
                                    }
                                }
                                //Handling for machine specific error on UpdatedailyCash step
                                else if (ex.Message.Contains("Index was out of range. Must be non-negative and less than the size of the collection."))
                                {
                                    ApplicationArguments.RetryCount = ApplicationArguments.RetrySize;
                                    Console.WriteLine(ApplicationArguments.TestCaseToBeRun + " completed with failure.");
                                    Log.Error(moduleName + "_" + stepName + " failed.");
                                    Console.WriteLine("Cause for failure : " + ex.Message);
                                    TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Fail");
                                    timer.Stop();

                                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MachineIssues"]))
                                    {
                                        TestExecutor.UpdateStatus(MessageConstants.MACHINE_ERROR);
                                        SaveAndExit(2); // 2 : Test case failed due to machine specific error. 
                                    }
                                    else
                                    {
                                        TestStatusLog.PublishLog();
                                        TestStatusLog.TestCaseResult(Col_Module, ApplicationArguments.TestCaseToBeRun, testCaseDescription, ex.Message, category, timer.Elapsed);
                                        TestExecutor.UpdateStatus(ex.Message);
                                        TestExecutor.CopyLogsToMaster();
                                    }



                                }
                                //Handling for Application Start Up failed 
                                else if (ex.Message.Contains("Application path not found. ImagePath: \\Prana.PricingService2Host.exe.") || ex.Message.Contains("Application path not found. ImagePath: \\Prana.TradeServiceHost.exe.") || ex.Message.Contains("Application path not found. ImagePath: \\Prana.ExpnlServiceHost.exe."))
                                {
                                    Console.WriteLine(ApplicationArguments.TestCaseToBeRun + " completed with failure.");
                                    Log.Error(moduleName + "_" + stepName + " failed.");
                                    Console.WriteLine("Cause for failure : " + ex.Message);
                                    string message = ErrorMessages.getErrorMessages(moduleName, stepName, ex.Message);
                                    TestStatusLog.TestCaseResult(Col_Module, ApplicationArguments.TestCaseToBeRun, testCaseDescription, message, category, timer.Elapsed);
                                    TestExecutor.UpdateStatus(ex.Message);
                                    TestExecutor.CopyLogsToMaster();
                                    TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Fail");
                                    timer.Stop();
                                    string[] ResolutionCAandGLissues = ConfigurationManager.AppSettings["resolutionIssuesForCAandGLCases"].Split(',');
                                    foreach (var issue in ResolutionCAandGLissues)
                                    {
                                        String InternalErrorMessage = ConfigurationManager.AppSettings["resolutionissueInternalMessage"];
                                        if (message.Contains(issue))
                                        {
                                            if (!message.Contains(InternalErrorMessage))
                                            {
                                                IsCAandGLCasesResolutionIssue = true;
                                            }
                                        }
                                    }
                                    string[] Issues = ConfigurationManager.AppSettings["CommonIssues"].Split(';');
                                    string Commonissue = "";
                                    foreach (var issue in Issues)
                                    {
                                        if (message.Contains(issue))
                                        {
                                            Commonissue = issue;
                                            break;
                                        }
                                    }
                                    bool flag = false;
                                    Console.WriteLine("issue is " + message);
                                    if (Commonissue != "")
                                    {
                                        if (totalFailedResults.ContainsKey(Commonissue))
                                            totalFailedResults[Commonissue] += 1;
                                        else
                                            totalFailedResults.Add(Commonissue, 1);
                                    }
                                    foreach (var values in totalFailedResults.Values)
                                    {
                                        if (Convert.ToInt32(ConfigurationManager.AppSettings["CommonFailureCounter"]) <= values)
                                        {
                                            Console.Write("Reason is");
                                            Console.WriteLine(Commonissue + "count is " + values);
                                            flag = true;
                                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CommonIssues"]))
                                            {
                                                SaveAndExit(3);
                                            }
                                        }
                                    }
                                    if (!flag)
                                    {
                                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MachineIssues"]))
                                        {
                                            SaveAndExit(1);
                                        }
                                    }
                                }
                                else
                                {
                                    //if (ApplicationArguments.RetryCount >= ApplicationArguments.RetrySize)
                                    // UpdateNotRunCases(ApplicationArguments.TestCaseToBeRun);
                                    Console.WriteLine(ApplicationArguments.TestCaseToBeRun + " completed with failure.");
                                    Log.Error(moduleName + "_" + stepName + " failed.");
                                    Console.WriteLine("Cause for failure : " + ex.Message);
                                    string message = ErrorMessages.getErrorMessages(moduleName, stepName, ex.Message);
                                    TestStatusLog.TestCaseResult(Col_Module, ApplicationArguments.TestCaseToBeRun, testCaseDescription, message, category, timer.Elapsed);
                                    TestExecutor.UpdateStatus(ex.Message);
                                    TestExecutor.CopyLogsToMaster();
                                    TestExecutor.VideoRecording(ApplicationArguments.TestCaseToBeRun, "stopRecording", "Fail");
                                    timer.Stop();
                                    //For Check the Resolution issue of Machine( GL & CA cases )
                                    string[] ResolutionCAandGLissues = ConfigurationManager.AppSettings["resolutionIssuesForCAandGLCases"].Split(',');
                                    foreach (var issue in ResolutionCAandGLissues)
                                    {
                                        String InternalErrorMessage = ConfigurationManager.AppSettings["resolutionissueInternalMessage"];
                                        if (message.Contains(issue))
                                        {
                                            if (!message.Contains(InternalErrorMessage))
                                            {
                                                IsCAandGLCasesResolutionIssue = true;
                                            }
                                        }
                                    }
                                    // To blacklist the ip if CommonFailureIssue occurrence hit the CommonIssueCounter value
                                    string[] Issues = ConfigurationManager.AppSettings["CommonIssues"].Split(';');
                                    string Commonissue = "";
                                    foreach (var issue in Issues)
                                    {
                                        if (message.Contains(issue))
                                        {
                                            Commonissue = issue;
                                            break;
                                        }
                                    }
                                    bool flag = false;
                                    Console.WriteLine("issue is " + message);
                                    if (Commonissue != "")
                                    {
                                        if (totalFailedResults.ContainsKey(Commonissue))
                                            totalFailedResults[Commonissue] += 1;
                                        else
                                            totalFailedResults.Add(Commonissue, 1);
                                    }
                                    foreach (var values in totalFailedResults.Values)
                                    {
                                        if (Convert.ToInt32(ConfigurationManager.AppSettings["CommonFailureCounter"]) <= values)
                                        {
                                            Console.Write("Reason is");
                                            Console.WriteLine(Commonissue + "count is " + values);
                                            flag = true;
                                            SaveAndExit(3);
                                        }
                                    }
                                    if (!flag)
                                        // SaveAndExit(102); //  102 : The test case failed due to blocked or missing UI. Application restart is recommended

                                        SaveAndExit(0);
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Log.Error("Running user defined test cases is failed");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }


        public static void CheckActiveUserDuringAutomation()
        {
            string[] usernameToCheck = ConfigurationManager.AppSettings["-AutomationUser"].Split(',');
            string command = "quser";
            List<string> activeUser = new List<string>();
            string output = ExecuteCommand(command);
            foreach (string userToCheck in usernameToCheck)
            {
                if (IsUserActive(ref output, userToCheck, ref activeUser))
                {
                    Console.WriteLine(userToCheck + " is active during test run");
                    break;
                }
            }
            foreach (string userToCheck in usernameToCheck)
            {
                if (!IsUserActive(ref output, userToCheck, ref activeUser))
                {
                    Console.WriteLine(userToCheck + " is Activate so that killing the automation");
                    Process[] myProcesses = Process.GetProcesses();
                    ShutDownSamsaraRelease();
                    //KillPranaProcess();
                    ShutDownEnterprise(myProcesses);
                    ShutDownComplianceServices(ref myProcesses);
                    string today = DateTime.Now.ToString("yyyyMMdd");
                    string filePath = "E:\\ExcludeIP_"+today+".txt";

                    string contentToWrite = "IP is excluded for today execution "+today;
                    File.WriteAllText(filePath, contentToWrite);
                    Console.WriteLine("File created and content written successfully.\n");

                    string fileContent = File.ReadAllText(filePath);
                    Console.WriteLine("Content of the file:");
                    Console.WriteLine("--------------------");
                    Console.WriteLine(fileContent);
                    Environment.Exit(0);
                }
                else
                    break;
            }
        }

        static string ExecuteCommand(string command)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            StreamWriter sw = process.StandardInput;
            StreamReader sr = process.StandardOutput;

            sw.WriteLine(command + " 2>&1");
            sw.Close();

            string output = sr.ReadToEnd();
            process.WaitForExit();
            process.Close();

            return output;
        }

        static bool IsUserActive(ref string quserData, string username, ref List<string> activeUser)
        {
            string[] lines = quserData.Split('\n');
            bool ans = false;
            foreach (string line in lines)
            {
                if (line.Contains("Active"))
                {
                    activeUser.Add(line);

                }
                if ((line.Contains(username.ToLower()) || line.Contains(username.ToUpper()) || line.Contains(username)) && line.Contains("Active") && !line.Contains("Disc"))
                {
                    ans = true;
                }
            }

            return ans;
        }

        /// <summary>
        /// Replace CounterParty custom mapping file for All Fix cases
        /// </summary>
        public static void ReplaceCounterPartyCustomMapping()
        {
            try
            {
                string DefaultCounterPartyFile = ConfigurationManager.AppSettings["DefaultCounterPartyFile"];
                string NewCounterPartyFile = ConfigurationManager.AppSettings["NewCounterPartyFile"];
                string TempCounterPartyFile = ConfigurationManager.AppSettings["TempCounterPartyFile"];
                //Temporary backup of original file to symbol_bak
                if (!File.Exists(TempCounterPartyFile))
                {
                    CreateTempFileAndCopyFromOriginal(DefaultCounterPartyFile, NewCounterPartyFile, TempCounterPartyFile);
                }

                if (File.Exists(DefaultCounterPartyFile) && File.Exists(NewCounterPartyFile))
                {
                    File.Copy(NewCounterPartyFile, DefaultCounterPartyFile, true);
                }

                // if newCounterParty file is empty  or path doesn't exist 
                if (!File.Exists(NewCounterPartyFile) || String.IsNullOrEmpty(NewCounterPartyFile))
                {
                    Console.WriteLine("newCounterParty file is empty  or File path doesn't exist");
                    throw new ApplicationException("new CounterParty file is empty  or File path doesn't exist");

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Create a temporary CounterParty file
        /// </summary>
        private static void CreateTempFileAndCopyFromOriginal(string DefaultCounterPartyFile, string NewCounterPartyFile, string TempCounterPartyFile)
        {
            File.Copy(DefaultCounterPartyFile, TempCounterPartyFile, true);
        }
        public TestResult ExecuteStep(string moduleName, string stepName, DataSet stepData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                ApplicationArguments.UIAutomationRunDataStep = false;
                ApplicationArguments.ActiveStep = stepName;
                StepWrapper stepWrapper = TestStepFactory.GetStepWrapper(ApplicationArguments.ApplicationStartUpPath, moduleName, stepName);
               
                if (stepWrapper == null)
                {
                    throw new Exception("Step '" + stepName + "' not found in module '" + moduleName + "'.");

                }
                var fallbackStepsConfig = ConfigurationManager.AppSettings["AllowedFallbackSteps"];
                var fallbackSteps = fallbackStepsConfig != null
                                      ? fallbackStepsConfig.Split(',').Select(s => s.Trim()).ToList()
                                         : new List<string>();

                TestResult stepResult = new TestResult();
                if (!IUIAutomationFileLoaded && stepWrapper.CanRunUIAutomationTest)
                {
                    try
                    {
                        IUIAutomationFileLoading();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while IUIAutomationFileLoading: " + ex.Message);
                        return new TestResult { IsPassed = false, ErrorMessage = ex.Message + stepName + " failed" };
                    }
                    IUIAutomationFileLoaded = true;
                }
                if (ApplicationArguments.isFallbackEnabled && fallbackSteps.Contains(stepName) && stepWrapper.CanRunUIAutomationTest && stepWrapper.CanRunTest)
                {
                    
                    stepResult = (TestResult)stepWrapper.RunUIAutomationTestMethod.Invoke(stepWrapper.Instance, new object[] { stepData, sheetIndexToName });

                    if (!stepResult.IsPassed)
                    {
                        stepResult = (TestResult)stepWrapper.RunTestMethod.Invoke(stepWrapper.Instance, new object[] { stepData, sheetIndexToName });
                    }
                }
                else
                {
                    if (stepWrapper.CanRunUIAutomationTest && !ApplicationArguments.UIAutomationRunDataStep)
                    {
                        if (stepName.Equals("OperationsonAllocation")) {
                            stepData.Tables[0].Columns.Add("Previous Step");
                            stepData.Tables[0].Rows[0]["Previous Step"] = previousStep;
                         }
                        stepResult = (TestResult)stepWrapper.RunUIAutomationTestMethod.Invoke(stepWrapper.Instance, new object[] { stepData, sheetIndexToName });
                    }
                    else if (stepWrapper.CanRunTest && !ApplicationArguments.UIAutomationRunDataStep)
                    {
                        stepResult = (TestResult)stepWrapper.RunTestMethod.Invoke(stepWrapper.Instance, new object[] { stepData, sheetIndexToName });
                    }
                    else
                    {
                        if (ApplicationArguments.UIAutomationRunDataStep && stepWrapper.CanRunUIAutomationTest)
                        {
                            stepResult = (TestResult)stepWrapper.RunUIAutomationTestMethod.Invoke(stepWrapper.Instance, new object[] { stepData, sheetIndexToName });
                        }
                        else
                        throw new InvalidOperationException("No valid test method found to execute.");
                    }
                }
           
                return stepResult;
            }
            catch (Exception ex)
            {

                 Console.WriteLine("An error occurred while executing the step: "+ex.Message);
                 if (ex.InnerException != null)
                 {
                     Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                     Console.WriteLine("Inner Exception Stack Trace: " + ex.InnerException.StackTrace);
                 }
                 return new TestResult
                 {
                     IsPassed = false,
                     ErrorMessage = ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message)
                         ? ex.InnerException.Message + " " + stepName + " failed"
                         : ex.Message + " " + stepName + " failed"
                 };

            }

        }
        public static void CopyFixLogsForTestCase(string testCaseToBeRun)
        {
            try
            {
                Console.WriteLine("FixLogs Copy Process Started for TestCase: " + testCaseToBeRun);

                string logPath = ConfigurationManager.AppSettings["AllFixLog"];
                if (!File.Exists(logPath))
                    throw new FileNotFoundException("AllFixLog file not found at: " + logPath);

                string tempFilePath = Path.Combine(Path.GetDirectoryName(logPath), "AllFixLogs_backup.log");
               
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
               
                File.Copy(logPath, tempFilePath);

                string fixProcessBaseFolder = ConfigurationManager.AppSettings["FixProcessbaseFolder"];
                string productDependency = ConfigurationManager.AppSettings["-ProductDependency"];

                string destinationFilePath = Path.Combine(fixProcessBaseFolder, productDependency, testCaseToBeRun + ".log");

                string destinationDirectory = Path.GetDirectoryName(destinationFilePath);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                File.Copy(logPath, destinationFilePath, true);

                Console.WriteLine("FixLogs Copy Process Completed for TestCase: " + testCaseToBeRun);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during FixLogs Copy Process: " + ex.Message);
                throw;
            }

        }
        public void IUIAutomationFileLoading()
        {
            try
            {
                if (ApplicationArguments.IUIAutomationDataTables == null)
                {
                    ApplicationArguments.IUIAutomationDataTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationDataFile"]);
                }
                if (ApplicationArguments.IUIAutomationMappingTables == null)
                {
                    ApplicationArguments.IUIAutomationMappingTables = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["IUIAutomationMappingFile"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        static void CopyFolder(string sourceFolder, string destinationFolder)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceFolder);

            if (!dir.Exists)
            {
                Console.WriteLine("source folder not exist" + sourceFolder);
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationFolder, file.Name);
                file.CopyTo(tempPath, false);
            }

            // Copy subdirectories and their contents to new location.
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destinationFolder, subdir.Name);
                CopyFolder(subdir.FullName, tempPath);
            }
        }

        public void EnterpriseLogOut()
        {
            try
            {
                CoreExecutor cs = new CoreExecutor();
                if (PranaApplication.IsVisible)
                {

                    Process[] processes = Process.GetProcessesByName("Prana");
                    if (processes.Length == 0)
                    {
                        Console.WriteLine("Prana Not running");
                    }
                    else
                    {

                        Process.GetProcessesByName("Prana")[0].Kill();
                    }
                    Wait(2000);
                    PranaApplication.Start();
                    Wait(5000);

                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at EnterpriseLogOut class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

        }


        public void EnterpriseLogin()
        {
            try
            {
                ProcessControlManager.MinimizeAllWindowsExceptSpecified();
                CoreExecutor cs = new CoreExecutor();
                if (PranaApplication.IsVisible)
                {
                    try
                    {
                        if (PranaMain.IsVisible)
                        {
                            ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                            if (SaveLayout.IsVisible)
                            {
                                ButtonNo.Click(MouseButtons.Left);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Prana application is not Visible");
                    }
                    Wait(2000);
                    Process[] processes = Process.GetProcessesByName("Prana");
                    if (processes.Length == 0)
                    {
                        Console.WriteLine("Prana Not running");
                    }
                    else
                    {
                        // implement solution here for if both application is up and running type is singleuser then throw error

                        Process.GetProcessesByName("Prana")[0].Kill();
                    }
                    Wait(2000);
                    PranaApplication.Start();
                    Wait(5000);
                    PranaApplication.BringToFront();
                }

                if (ConfigurationManager.AppSettings["IsMultiUser"].ToString() != "true")
                {
                    Login.BringToFront();
                    UserName.Click(MouseButtons.Left);
                    Keyboard.SendKeys(ApplicationArguments.RequiredActiveUser);//active user se kre login taaki agr support2 se krna ho...to 1 se na kre..
                    TxtPassword.Click(MouseButtons.Left);
                    Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                    LoginButton.Click(MouseButtons.Left);
                    if (CustomMessageBox.IsEnabled)
                    {
                        UltraOkButton.Click();
                    }
                }
                else
                {
                    UserName.Click(MouseButtons.Left);
                    Keyboard.SendKeys(ApplicationArguments.ReleaseUserName);
                    TxtPassword.Click(MouseButtons.Left);
                    Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                    LoginButton.Click(MouseButtons.Left);
                    Wait(5000);
                    
                }
                Wait(2000);
                // Open Compliance UI Before running compliance test cases to sync rule ID in the database 
                OpenAndCloseCompliance();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at EnterpriseLogin class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        private void OpenAndCloseCompliance()
        {
            try
            {
                bool IsOpen = bool.Parse(ConfigurationManager.AppSettings["IsComplianceModuleOpen"]);
                // On behalf of Karan (Task 63442: Fix the Compliance UI open Issue in Samsara cases)
                if (count <= 1 && IsOpen)
                {
                    if (!ApplicationArguments.SkipCompliance || ApplicationArguments.TestCaseToBeRun.ToString().Contains("RTPNL"))
                    {
                        //  Shortcut to open ComplianceEngine module(CTRL + SHIFT + C)
                        Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_COM_ENGINE"]);
                        Wait(15000);

                        if (!ComplianceEngine.IsVisible)
                        {
                            ExtentionMethods.WaitForVisible(ref ComplianceEngine, 10);
                        }
                        ComplianceEngine_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                        KeyboardUtilities.CloseWindow(ref  ComplianceEngine_UltraFormManager_Dock_Area_Top);
                        Wait(500);
                        //KeyboardUtilities.MaximizeWindow(ref );
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at Open Compliance UI");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void RestartRtpnlService()
        {
            try
            {
                var allProcceses = Process.GetProcesses();
                if (!ApplicationArguments.SkipCompliance)
                {
                    foreach (Process process in allProcceses)
                    {
                        if (process.ProcessName.Contains("cmd") && process.MainWindowTitle.Contains("Esper Calculation Engine"))
                        {
                            process.CloseMainWindow();
                            process.CloseMainWindow();
                        }
                    }
                    StartEsper();
                }
                if (ApplicationArguments.TestCaseToBeRun.ToString().Contains("RTPNL"))
                {
                    allProcceses = Process.GetProcesses();
                    foreach (Process process in allProcceses)
                    {
                        if (process.ProcessName.Contains("Prana.CalculationServiceHost"))
                        {
                            process.Kill();
                        }
                    }
                    StartCalculationService();
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at RestartRtpnlService class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        private void StartCalculationService()
        {
            try
            {
                
                    ProcessStartInfo start1 = new ProcessStartInfo();
                    start1.FileName = "Prana.CalculationServiceHost.exe";
                    start1.WorkingDirectory = ApplicationArguments.CalculationServicePath;
                    start1.WindowStyle = ProcessWindowStyle.Minimized;
                    Process java1 = new Process();
                    java1.StartInfo = start1;
                    java1.Start();
                    System.Threading.Thread.Sleep(7000);
                    Wait(60000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at StartCalculationService class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public void RestartSamsara(string stepName = "", DataSet testData = null)
        {
            string simulatorvalue = string.Empty;
            if (stepName.Equals("RestartReleasesAndServices"))
            {
                if (testData.Tables[0].Columns.Contains("DontShutSimulator") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["DontShutSimulator"].ToString()))
                {
                    AccessBridgeHelper.Inititalize();
                    simulatorvalue = testData.Tables[0].Rows[0]["DontShutSimulator"].ToString().ToUpper();
                }
            }
            ShutDownSamsaraRelease();
            //StartZookeperService();
            //StartKafkaServices();
            StartDropCopy();
            if (string.IsNullOrEmpty(simulatorvalue))
            {
                StartSimulator();
            }
            StartPricing();
            StartJboss();
            StartServer();
            StartExpnl();
            StartRuleMediator();
            StartEsper();
            StartBasketCompliance();
            StartChromeDriver();
            StartServiceGatway();
            StartSamsaraServices();
            if (string.IsNullOrEmpty(simulatorvalue))
            {
                StartSamsaraYarnSetUp();
            }
            ProcessControlManager.MinimizeAllWindowsExceptSpecified();
            //StartSamsaraApplication();          
        }


        public void SamsaraApplicationLogin()
        {
            try
            {
                System.Threading.Thread.Sleep(10000);
                if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                {
                    TestResult stepResult = new TestResult();
                    IOpenFinTestStep OpenFinStep = TestStepFactory.GetOpenFinStep(ApplicationArguments.ApplicationStartUpPath, "PranaClient", "LoginClient");
                    stepResult = (TestResult)OpenFinStep.RunOpenFinTest(new DataSet(), new Dictionary<int, string>(), "Samsara");
                    if (!stepResult.IsPassed)
                    {
                        SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                    }
                }
                Retry = false;
                count += 1;
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun);
                Log.Error(ex.Message);
                throw;
            }
        }


        public void RefreshExpnlUi()
        {
            try
            {
                TestResult stepResult = new TestResult();
                ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, "Expnl", "RefreshExpnlUi");
                stepResult = (TestResult)step.RunTest(null,null);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to refresh expnl Ui" + ex.Message);
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Unable to refresh expnl Ui");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
        }

        public void ClearCache()
        {
            try
            {
                TestResult stepResult = new TestResult();
                ITestStep step = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, "TradeServer", "ClearCache");
                stepResult = (TestResult)step.RunTest(null, null);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to Clear Cache" + ex.Message);
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Unable to Clear Cache");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
 

        /// <summary>
        /// Run clean up to clean the data using Closing & Allocation module.
        /// </summary>
        /// <param name="stepName"></param>
        private static void RunCleanUp()
        {
            try
            {
                ITestStep cleancloseUp = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, ExcelStructureConstants.CLOSING, ExcelStructureConstants.CLOSING_CLEAN_UP);
                TestResult obj = (TestResult)cleancloseUp.RunTest(null, null);
                if (!obj.IsPassed)
                {
                    Log.Error("Closing cleanup failed.");
                }

                ITestStep cleanStep = TestStepFactory.GetStep(ApplicationArguments.ApplicationStartUpPath, ExcelStructureConstants.ALLOCATION, ExcelStructureConstants.ALLOCATION_CLEAN_UP);
                if (!obj.IsPassed)
                {
                    Log.Error("Allocation cleanup failed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("CleanUp has failed.! : " + ex.Message);
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at RunCleanUp class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;

            }
        }
        private static bool checkTestCaseWithStep(ref string stepName, ref Dictionary<string, StepWiseController> StepProductTypeControlHandler, string TestCase)
        {
            try
            {
                if (StepProductTypeControlHandler.ContainsKey(TestCase))
                {
                    foreach (string stepNameinList in StepProductTypeControlHandler[TestCase].StepsToRunOnEnterprise)
                    {
                        if (String.Equals(stepNameinList, stepName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }

            return false;
        }

        private void ShutDownAndRestartServices(string stepName = "", DataSet testData = null)
        {
            try
            {
                Process[] myProcesses = Process.GetProcesses();
                ShutDownSamsaraRelease();
                //KillPranaProcess();
                ShutDownEnterprise(myProcesses, stepName, testData);
                ShutDownComplianceServices(ref myProcesses, stepName, testData);
                CoreExecutor obj = new CoreExecutor();
                obj.RestartSamsara(stepName, testData);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }


        }
        private static void ShutDownEnterprise(Process[] myProcesses, string stepName = "", DataSet testData = null)
        {
            try
            {
                ICommandUtilities cmdUtilities = null;
                cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                string simulatorvalue = string.Empty;
                if (stepName.Equals("RestartReleasesAndServices"))
                {
                    if (testData.Tables[0].Columns.Contains("DontShutSimulator") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["DontShutSimulator"].ToString()))
                    {
                        simulatorvalue = testData.Tables[0].Rows[0]["DontShutSimulator"].ToString().ToUpper();
                    }
                }
                foreach (Process myProcess in myProcesses)
                {

                    if (myProcess.MainWindowTitle.Contains("Prana Pricing Service"))
                    {
                        myProcess.CloseMainWindow();
                    }
                    if (myProcess.MainWindowTitle.Contains("Prana Expnl Service"))
                    {
                        myProcess.CloseMainWindow();
                    }
                    if (myProcess.MainWindowTitle.Contains("Prana Trade Service"))
                    {
                        myProcess.CloseMainWindow();
                    }
                    if (string.IsNullOrEmpty(simulatorvalue))
                    {
                        if (myProcess.MainWindowTitle.Contains("Buy") || myProcess.MainWindowTitle.Contains("Sell Side Simulator") || myProcess.MainWindowTitle.Contains("cmd.exe"))
                        {
                            myProcess.CloseMainWindow();
                        }
                    }

                    if (myProcess.MainWindowTitle.Contains("'Rule Mediator Engine'"))
                    {
                        myProcess.CloseMainWindow();
                        myProcess.CloseMainWindow();
                    }
                    if (myProcess.MainWindowTitle.Contains("'Esper Calculation Engine'"))
                    {
                        myProcess.CloseMainWindow();
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


            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }


        }
        private static void ShutDownComplianceServices(ref Process[] myProcesses, string stepName = "", DataSet testData = null)
        {
            try
            {
                string simulatorvalue = string.Empty;
                if (stepName.Equals("RestartReleasesAndServices")) 
                {
                    if (testData.Tables[0].Columns.Contains("DontShutSimulator") && !string.IsNullOrEmpty(testData.Tables[0].Rows[0]["DontShutSimulator"].ToString()))
                    {
                        simulatorvalue = testData.Tables[0].Rows[0]["DontShutSimulator"].ToString().ToUpper();
                    }
                }
                foreach (Process myProcess in myProcesses)
                {

                    if (myProcess.MainWindowTitle.Contains("Rule Mediator Engine -"))
                    {
                        myProcess.CloseMainWindow();
                    }

                    if (myProcess.MainWindowTitle.Contains("Esper Calculation Engine -"))
                    {
                        myProcess.CloseMainWindow();
                    }

                    if (myProcess.MainWindowTitle.Contains("DropCopyFileReader"))
                    {
                        myProcess.CloseMainWindow();
                    }
                    ///for basket compliance service close'
                    if (myProcess.MainWindowTitle.Contains("Basket Compliance Service -"))
                    {
                        myProcess.CloseMainWindow();
                    }
                    if (string.IsNullOrEmpty(simulatorvalue))
                    {
                        if (myProcess.MainWindowTitle.Contains("Buy") || myProcess.MainWindowTitle.Contains("Sell Side Simulator") || myProcess.MainWindowTitle.Contains("cmd.exe"))
                        {
                            myProcess.CloseMainWindow();
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }


        }


        private void StartZookeperServiceAndKafkaServices()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                string batchFilePath = ApplicationArguments.KafkaPath + "\\kafka.bat";
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"& '" + batchFilePath + "'\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        WorkingDirectory = @"E:\kafka\"
                    };

                    Process process = new Process
                    {
                        StartInfo = startInfo
                    };

                    process.Start();
                    Console.WriteLine("Batch file started successfully.");
                }

                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                    if (rethrow)
                        throw;
                }
            }
        }
        public void StartSamsaraYarnSetUp()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(10000);
                    string baseFileName = "output";
                    string extension = "txt";
                    string uniqueFileName = DataUtilities.GetUniqueFileName(baseFileName, extension);
                    string sourceFilePath = Path.Combine(ConfigurationManager.AppSettings["SamsaraDirectory"], uniqueFileName);                
                    try
                    {

                        if (File.Exists(sourceFilePath))
                        {
                            File.Delete(sourceFilePath);
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                   // ProcessControlManager.ProcessStarter("SamsaraCacheHandler.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"]);
                    ProcessControlManager.ProcessStarter("Webapplication.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"], uniqueFileName);
                    CommonMethods.VerifyStringInTextFile((sourceFilePath), "http://", 15000);
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void StartSamsaraYarnStart()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    ProcessControlManager.ProcessStarter("YarnStartBatch.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"]);
                    ProcessControlManager.MinimizeAllWindowsExceptSpecified();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public bool IsCaseMultiUser()
        {
            bool isMultiUser = true;
            String testCase = string.Empty;
            String Col_Module = string.Empty;
            DataSet workbook = new DataSet();
            try {
                if (cache.Contains("Regression Test Cases"))
                {
                    workbook = cache["Regression Test Cases"] as DataSet;
                    Console.WriteLine("RegressionTestCases DataSet loaded from Cache");
                }

                foreach (DataRow dr in workbook.Tables[ApplicationArguments.SheetName].Rows)
                {
                    if (String.IsNullOrWhiteSpace(dr[ExcelStructureConstants.COL_TESTCASEID].ToString()))
                    {
                        dr[ExcelStructureConstants.COL_TESTCASEID] = testCase;
                        ApplicationArguments.SheetName = Col_Module;
                    }
                    else
                        testCase = dr[ExcelStructureConstants.COL_TESTCASEID].ToString();
                    Col_Module = ApplicationArguments.SheetName;
                }

                DataRow[] testData = workbook.Tables[ApplicationArguments.SheetName].Select(String.Format(ExcelStructureConstants.COL_TESTCASEID + " = '{0}'", ApplicationArguments.TestCaseToBeRun));
                string[] StepsList = ConfigurationManager.AppSettings["StepToPerformWithSingleUser"].Split(',');
                foreach (var row in testData)
                {
                    if (StepsList.Contains(row[4].ToString()))
                    {
                        
                        isMultiUser = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return isMultiUser;
        }

        private void SamsaraApplicationLoginWithRequiredUser()
        {
            try
            {
                System.Threading.Thread.Sleep(10000);
                if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                {
                    SamsaraHelperClass.RunOpenFinTest("samsara", "", "", "RequiredUserLogin");
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SamsaraApplicationLoginWithRequiredUser");
                Log.Error(ex.Message);
                throw;
            }
        }

        private bool LogOutSamsaraWithVerification()
        {
           bool logoutDone = true;
            
            try
            {
                if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                {
                    SamsaraHelperClass.LogOutUser();
                    logoutDone = SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "LogOutSamsaraWithVerification");
                Log.Error(ex.Message);
                throw;
            }

            return logoutDone;
        }

        private bool CheckSamsaraUp()
        {
            bool up = true;
            try
            {

                if (ConfigurationManager.AppSettings["-ProductDependency"].ToLower() == "samsara")
                {
                    up = SamsaraHelperClass.IsWindowOpen("Dock");
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckSamsaraUp");
                Log.Error(ex.Message);
                throw;
            }
            return up;
        }
        private void  handlePrefStepsPostActions()
        {
            try
            {
                LogOutSamsaraWithVerification();
                KillPranaProcess();
                ShutDownAndRestartServices();
                StartSamsaraYarnStart();
                if (ConfigurationManager.AppSettings["IsMultiUser"].ToString() != "true")
                {
                    SamsaraApplicationLoginWithRequiredUser();
                }
                else {
                    SamsaraHelperClass.RunOpenFinTest("samsara", "", "", "RestartClient");
                    EnterpriseLogin();
                }
                Wait(3000);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "on Restarting Services after pref changes..");
            }
        }

        private static bool SkipSteps(string targetStepName)
        {
            try
            {

                var dataTables = new Dictionary<string, DataTable>();
                string filePath = ConfigurationManager.AppSettings["-skipsteps"].ToString();
                string targetCaseId = ApplicationArguments.TestCaseToBeRun;                
                using (ExcelPackage xlpackage = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var worksheet in xlpackage.Workbook.Worksheets)
                    {
                        DataTable datatable = new DataTable(worksheet.Name);

                        for (int column = 1; column <= worksheet.Dimension.Columns; column++)
                        {
                            try
                            {
                                datatable.Columns.Add(worksheet.Cells[1, column].Value.ToString());
                            }
                            catch { }
                        }
                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            try
                            {
                                DataRow dataRow = datatable.NewRow();
                                for (int column = 1; column <= worksheet.Dimension.Columns; column++)
                                {
                                    dataRow[column - 1] = worksheet.Cells[row, column].Value;
                                }
                                datatable.Rows.Add(dataRow);
                            }
                            catch { }
                        }
                        dataTables.Add(worksheet.Name, datatable);
                        foreach (DataRow row in datatable.Rows)
                        {
                            string caseId =row["TestCaseID"].ToString();
                            string stepNames = row["StepsToSkip"].ToString();
                            if (caseId == targetCaseId)
                            {
                                var steps = stepNames.Split(',').Select(s => s.Trim()).ToList();
                                if (steps.Contains(targetStepName))
                                {
                                    return true;
                                }
                            }
                        }       
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SkipStep");
                Log.Error(ex.Message);
                throw;
            }
        }

        #endregion Methods




    }


}