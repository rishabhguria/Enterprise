using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Newtonsoft.Json;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OfficeOpenXml;
using Nirvana.TestAutomation.UIAutomation;
using System.Runtime.Caching;
using System.Diagnostics;

namespace Nirvana.TestAutomation.TestExecutor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static class GlobalArgs
        {
            public static string[] Args { get; set; }
            public static bool usingTempFile = false;
        }

        [STAThread]
        public static void Main(String[] args)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            string ExcludeFilePath = "E:\\ExcludeIP_" + today + ".txt";
            if (File.Exists(ExcludeFilePath))
            {
                string fileContent = File.ReadAllText(ExcludeFilePath);
                Console.WriteLine(fileContent);
                Environment.Exit(0);
            }
            GlobalArgs.Args = args;
            MemoryCache cache = CacheManager.Instance.GetCache();
            if (cache.GetCount() > 0)
            {
                Console.WriteLine("Removing " + cache.GetCount() + " No. of entries from cache");
                cache.Trim(100);
            }
            ApplicationArguments.ApplicationStartUpPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string slaveIP = TestStatusLog.GetSytemIP().ToString();
            string slaveIPName = slaveIP.Substring(8).Replace('.', '_');
            try
            {
                ApplicationArguments.ExcludeSlavesFilePath = ConfigurationManager.AppSettings["ExcludeSlavesSheetPath"];
                TestExecutor.IsMachineFaulty(slaveIP, ConfigurationManager.AppSettings["MachineIssues"].Split(',').ToList(), true);
                KeyboardUtilities.OffCapsLock();
                if (ConfigurationManager.AppSettings["viaCmdArguments"].Equals("True"))
                {
                    ApplicationArguments.SkipLogin = Boolean.Parse(ConfigurationManager.AppSettings["-skiplogin"]);
                    ApplicationArguments.SkipStartUp = Boolean.Parse(ConfigurationManager.AppSettings["-skipstartup"]);
                    //ApplicationArguments.SkipCompliance = Boolean.Parse(ConfigurationManager.AppSettings["-skipCompliance"]);
                    ApplicationArguments.DownloadData = Boolean.Parse(ConfigurationManager.AppSettings["-downloaddata"]);
                    ApplicationArguments.TestDataFolderPath = ConfigurationManager.AppSettings["-testFolder"];
                    // ApplicationArguments.MasterSheetPath = ConfigurationManager.AppSettings["-masterSheetPath"];
                    ApplicationArguments.PricingReleasePath = ConfigurationManager.AppSettings["-pricingReleasePath"];
                    ApplicationArguments.ServerReleasePath = ConfigurationManager.AppSettings["-serverReleasePath"];
                    ApplicationArguments.ExpnlReleasePath = ConfigurationManager.AppSettings["-expnlReleasePath"];
                    ApplicationArguments.ClientReleasePath = ConfigurationManager.AppSettings["-clientReleasePath"];
                    ApplicationArguments.CameronSimulatorPath = ConfigurationManager.AppSettings["-cameronsimulatorpath"];
                    ApplicationArguments.DropCopyPath = ConfigurationManager.AppSettings["-dropcopypath"];
                    ApplicationArguments.LogFolder = ConfigurationManager.AppSettings["-logFolder"];
                    ApplicationArguments.SendEmailNotifcations = Boolean.Parse(ConfigurationManager.AppSettings["-sendemailnotification"]);
                    ApplicationArguments.ReportFolderId = ConfigurationManager.AppSettings["-reportfolderid"];
                    ApplicationArguments.ClientDB = ConfigurationManager.AppSettings["-clientDB"];
                    ApplicationArguments.JbossCompliancePath = ConfigurationManager.AppSettings["-jbossCompliancePath"];
                    ApplicationArguments.EsperCompliancePath = ConfigurationManager.AppSettings["-esperCompliancePath"];
                    ApplicationArguments.BasketCompliancePath = ConfigurationManager.AppSettings["-basketCompliancePath"];
                    ApplicationArguments.RuleEngineCompliancePath = ConfigurationManager.AppSettings["-ruleEngineCompliancePath"];
                    ApplicationArguments.DBInstanceName = ConfigurationManager.AppSettings["-dBInstanceName"];
                    ApplicationArguments.MasterPranaPrefPath = ConfigurationManager.AppSettings["-masterPranaPrefPath"];
                    ApplicationArguments.CurrentPranaPrefPath = ConfigurationManager.AppSettings["-currentPranaPrefPath"];
                    ApplicationArguments.DataBasePath = ConfigurationManager.AppSettings["-dataBasePath"];
                    ApplicationArguments.MasterDB = ConfigurationManager.AppSettings["-masterDB"];
                    ApplicationArguments.TradeServiceUIPath = ConfigurationManager.AppSettings["-tradeServiceUIPath"];
                    ApplicationArguments.AutomationProviderKey = ConfigurationManager.AppSettings["-automationProviderKey"];
                    ApplicationArguments.WinAppMappingsFilePath = ConfigurationManager.AppSettings["-WinAPPMappingsFile"];
                    ApplicationArguments.RestartActionList = ConfigurationManager.AppSettings["clientRestartAction"].ToString().Split(',').ToList();
                    ApplicationArguments.PopUpStepsList = ConfigurationManager.AppSettings["-PopUpStep"].ToString().Split(',').ToList();
                    ApplicationArguments.columnMappingFile = ConfigurationManager.AppSettings["-columnMappingFile"].ToString();
                    ApplicationArguments.GroupedDataOnStepsList = ConfigurationManager.AppSettings["-getGroupedDataOnExport"].ToString().Split(',').ToList();
                    ApplicationArguments.KafkaPath = ConfigurationManager.AppSettings["-kafkaBatchFilePath"].ToString();
                    ApplicationArguments.PrefRestartList = ConfigurationManager.AppSettings["-PrefSteps"].ToString().Split(',').ToList();
                    ApplicationArguments.ExpnlReleaseUIPath = ConfigurationManager.AppSettings["-expnlReleaseUIPath"].ToString();
                    ApplicationArguments.ServerReleaseUIPath = ConfigurationManager.AppSettings["-serverReleaseUIPath"].ToString();
                    ApplicationArguments.CalculationServicePath = ConfigurationManager.AppSettings["-calculationServicePath"].ToString();
                    ApplicationArguments.isFallbackEnabled = bool.Parse(ConfigurationManager.AppSettings["enableFallback"] ?? "false");
                    ApplicationArguments.PortfolioDBBackUpsMaster = ConfigurationManager.AppSettings["-PortfolioDBBackUpsMaster"].ToString();
                    ApplicationArguments.PortfolioDBBackUpsSlave = ConfigurationManager.AppSettings["-PortfolioDBBackUpsSlave"].ToString();

                    if (ApplicationArguments.UploadSlaveTestReport)
                    {
                        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["-runDescription"]))
                        {
                            ApplicationArguments.RunDescription = String.Format(ApplicationArguments.RunDescription + "(Distributed)" + '_' + slaveIPName + "-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                        }
                        else
                        {
                            ApplicationArguments.RunDescription = String.Format((ConfigurationManager.AppSettings["-runDescription"]) + '_' + slaveIPName + "-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["-runDescription"]))
                        {
                            ApplicationArguments.RunDescription = String.Format(ApplicationArguments.RunDescription + "(Distributed)" + '_' + slaveIPName + "-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                        }
                        else
                        {
                            ApplicationArguments.RunDescription = String.Format((ConfigurationManager.AppSettings["-runDescription"]) + '_' + slaveIPName + "-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                        }
                    }
                 


                    if (args.Length > 0)
                    {
                        //update master sheet path
                        var index = Array.FindIndex(args, row => row.Contains(CommandLineConstants.CONST_MASTER_SHEET_PATH));
                        if (index >= 0)
                        {
                            ApplicationArguments.MasterSheetPath = args[index + 1].Replace("\"", string.Empty);
                            string tempFilePath = null;
                            try
                            {
                                using (FileStream stream = new FileInfo(ApplicationArguments.MasterSheetPath).Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                                {
                                    stream.Close();
                                }
                            }
                            catch (IOException)
                            {
                                Console.WriteLine(ApplicationArguments.MasterSheetPath + " file opened hence creating temporary file");
                                tempFilePath = Path.Combine(Path.GetDirectoryName(ApplicationArguments.MasterSheetPath), "temp_" + Path.GetFileName(ApplicationArguments.MasterSheetPath));
                                File.Copy(ApplicationArguments.MasterSheetPath, tempFilePath, true);
                                ApplicationArguments.MasterSheetPath = tempFilePath;
                                GlobalArgs.usingTempFile = true;
                            }
                        }
                        //update master sheet path
                        var index1 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.CONST_MASTER_SHEET_BK_PATH));
                        if (index1 >= 0)
                            ApplicationArguments.MasterSheetBackupPath = args[index1 + 1].Replace("\"", string.Empty);
                        //Allow Log FIle to be Copied to Master Machine
                        var index2 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.CONST_ALLOW_COPY_LOG_FILE_TO_MASTER));
                        if (index2 >= 0)
                        {
                            ConfigurationManager.AppSettings["-AllowCopyLogFileToMaster"] = args[index2 + 1];
                            ApplicationArguments.CopyLogToMaster = args[index2 + 1];
                        }
                        var index3 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.CONST_ALLOW_DELETE_UNNECESSARY_FILES));
                        if (index3 >= 0)
                        {
                            ConfigurationManager.AppSettings["-AllowDeleteUnnecessaryFiles"] = args[index3 + 1];
                            ApplicationArguments.DeleteUnneccessarySheets = args[index3 + 1];
                        }
                        var index4 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.ProductDependency));
                        if (index4 >= 0)
                        {
                           
                            var table = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["SamsaraDataFile"])["Login"];
                            var table1 = SamsaraHelperClass.ExcelToSamsaraDataTable();
                            string xpath = table.Rows[0]["Key Name"].ToString();
                           
                            ConfigurationManager.AppSettings["-ProductDependency"] = args[index4 + 1];
                            ApplicationArguments.ProductDependency = args[index4 + 1];
                            try
                            {
                                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings["ModuleStepSettingFile"])))
                                {
                                    ExcelWorksheet MappingSheet = excelPackage.Workbook.Worksheets["Modules"];
                                    for (int i = 4; i <= MappingSheet.Dimension.Rows; i++)
                                    {
                                        if (MappingSheet.Cells[i, 4].Value != null)
                                            ApplicationArguments._ModuleStepMapping.Add((MappingSheet.Cells[i, 1].Value.ToString()+MappingSheet.Cells[i, 2].Value.ToString()), MappingSheet.Cells[i, 4].Value.ToString());
                                        else if (MappingSheet.Cells[i, 2].Value != null)
                                            ApplicationArguments._ModuleStepMapping[(MappingSheet.Cells[i, 1].Value.ToString()+MappingSheet.Cells[i, 2].Value.ToString())] = "Input";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        var index5 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.RunType));
                        if (index5 >= 0)
                        {
                            ApplicationArguments.runType = args[index5 + 1];
                        }
                        var index6 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.AUTOMATIONPROVIDER_KEY));
                        if (index6 >= 0)
                        {
                            ConfigurationManager.AppSettings["-automationProviderKey"] = args[index6 + 1];
                            ApplicationArguments.AutomationProviderKey = args[index6 + 1];
                        }

                        var index7 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.WINAPPMAPPINGSFILE));
                        if (index7 >= 0)
                        {
                            ConfigurationManager.AppSettings["-WinAPPMappingsFile"] = args[index7 + 1];
                            ApplicationArguments.WinAppMappingsFilePath = args[index7 + 1];
                        }
                        var index8 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.INTERNALTRETRYSIZE));
                        if (index8 >= 0)
                        {
                            ConfigurationManager.AppSettings["-InternalRetrySize"] = args[index8 + 1];
                        }
                        var index9 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.automationUserName));
                        if (index9 >= 0)
                        {
                            ConfigurationManager.AppSettings["-AutomationUser"] = args[index9 + 1];
                        }
                        var index10 = Array.FindIndex(args, row => row.Contains(CommandLineConstants.checkActiveUserNameBeforeCaseRun));
                        if (index10 >= 0)
                        {
                            ConfigurationManager.AppSettings["-CheckActiveUser"] = args[index10 + 1];
                        }

                        
                    }

                    if (string.Equals(ConfigurationManager.AppSettings["-CheckActiveUser"].ToString(), "true", StringComparison.OrdinalIgnoreCase))
                    {
                        Nirvana.TestAutomation.Utilities.ExtentionMethods.CheckActiveUserBeforeAutomation();
                    }

                    DataContainer.WinAppMapper = WinAppUtility.MappingsReader(ApplicationArguments.WinAppMappingsFilePath);
                   
                   

                    if (string.Equals(ApplicationArguments.AutomationProviderKey, "WINAPP", StringComparison.Ordinal))
                    {
                        WinAppUtility.StartWinAppDriverServer();
                        ApplicationArguments.winappStarted = true;
                       
                    }

                    string filePath_Name = CommonMethods.GetLogPath(ApplicationArguments.LogFolder, ApplicationArguments.RunDescription);
                    string filePath_Namexlsx = filePath_Name.Substring(0, filePath_Name.IndexOf(".")) + ".xlsx";
                    if (Directory.Exists(ApplicationArguments.LogFolder))
                    {
                        List<string> li = new List<string>();
                        string type = ConfigurationManager.AppSettings["-runDescription"].ToString();
                        string masterFilesPath = ConfigurationManager.AppSettings["-MasterReportPath"].ToString().Replace("?", type.Substring(0, type.IndexOf(" ")));


                        foreach (var sourceFile in Directory.GetFiles(ApplicationArguments.LogFolder, "*.xlsx"))
                        {
                            string[] timeparts = sourceFile.Substring(sourceFile.LastIndexOf("_") + 1).Split('-');
                            li.Add(timeparts[2]);
                        }
                        if (Directory.Exists(masterFilesPath + slaveIPName + "\\TestLogs"))
                        {
                            foreach (var destFile in Directory.GetFiles(masterFilesPath + slaveIPName + "\\TestLogs", "*.xlsx"))
                            {
                                string[] timeparts = destFile.Substring(destFile.LastIndexOf("_") + 1).Split('-');
                                li.Add(timeparts[2]);
                            }

                        }
                        bool FileExist = false;
                        string reNamedFile = string.Empty;
                        FileExist = checkFileExist(li, ApplicationArguments.RunDescription);
                        if (FileExist.Equals(true))
                        {
                            ApplicationArguments.RunDescription = generateNewFileName(li,ApplicationArguments.RunDescription.ToString()) ;
 
                        }

                        if( ConfigurationManager.AppSettings["ControlProductDependencyStepWise"].ToLower().Equals("true"))
                        {
                            string filePath = ConfigurationManager.AppSettings["-ControlProductDependencyStepWiseFile"].ToString();
                            DataUtilities.MappingsReader(ref filePath ,ref ApplicationArguments.StepProductTypeControlHandler);
                        }

                    }
                    if (ConfigurationManager.AppSettings["-AllowFixSteps"].ToLower().Equals("false"))
                    {
                       ApplicationArguments.SkipFixSteps =  ConfigurationManager.AppSettings["-skipFixSteps"].Split(',').ToList();

                    }
                    if (ConfigurationManager.AppSettings["UseCustomApproach"].ToLower() == "true")
                    {
                        ApplicationArguments.ITestCaseFixingTables = SamsaraHelperClass.ExcelToSamsaraDataTable(
                            ConfigurationManager.AppSettings["TestCaseFixingFile"]);
                        if (ApplicationArguments.ITestCaseFixingTables.Count > 0)
                        {
                            foreach (var kvp in ApplicationArguments.ITestCaseFixingTables)
                            {
                                DataTable table = kvp.Value; 

                                if (table == null || table.Rows.Count == 0)
                                {
                                    throw new Exception(kvp.Key +"Sheet not loaded successfully.");
                                }
                            }
                           
                            try
                            {
                                CoreModeHelper.BackupOldFixingReport();
                                CoreModeHelper.InitializeFixingReport();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("An error occurred while creating TestCaseFixingMode Report : " + ex.Message);
                            }
                        }
                        else
                        {
                            throw new Exception("No tables loaded from the TestCaseFixingFile.");
                        }
                    }
                    try
                    {
                        int i = 0;
                        while (i <= 10)
                        {

                            TestExecutor.Run(true);
                        }

                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Cause for failure : " + ex.Message);
                    }
                }

                try
                {
                    //var application = new App();
                    //application.InitializeComponent();
                    //application.Run();
                    ProcessStartInfo info = new ProcessStartInfo(@"E:\DistributedAutomation\RestartMachine.bat");
                    info.CreateNoWindow = true;
                    info.UseShellExecute = false;
                    Process.Start(info);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cause for failure : " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_ONLY_POLICY);
                if (rethrow)
                    throw;
            }
            
        }
        private static bool checkFileExist(List<string> li, string RunDescription)
        {
            string timeStamp = RunDescription.Substring(RunDescription.LastIndexOf("_") + 1);
            string[] timeparts = RunDescription.Substring(RunDescription.LastIndexOf("_") + 1).Split('-');
            foreach (var variable in li)
            {
                if (timeparts[2] == variable)
                {
                    return true;
 
                }

            }
            return false;
        }


        private static string generateNewFileName(List<string> li, string RunDescription)
        {
             string timeStamp = RunDescription.Substring(RunDescription.LastIndexOf("_") + 1);
            string[] timeparts = RunDescription.Substring(RunDescription.LastIndexOf("_") + 1).Split('-');
            int random;
            int NewTimePart = Convert.ToInt32(timeparts[2]);
            while (true)
            {
                NewTimePart += 1;
                if (!li.Contains((NewTimePart).ToString()))
                {
                    random = NewTimePart;
                    break;
                }

                    
            }

            timeparts[2] = random.ToString();
            li.Add(random.ToString());
            string newTimeStamp = string.Empty;
            foreach (var value in timeparts)
            {
                newTimeStamp = newTimeStamp + value + "-";
            }
            newTimeStamp = newTimeStamp.Remove(newTimeStamp.Length - 1);
            string resultedRenamedFile = RunDescription.Substring(0, RunDescription.LastIndexOf("_") + 1) + newTimeStamp;
            return resultedRenamedFile;
        }
    }
}





