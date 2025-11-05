using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Diagnostics;
using Nirvana.TestAutomation.Utilities;
using System.Configuration;
using System.Net.Mail;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;
using Newtonsoft.Json;
using System.Web;
using System.Timers;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.UI.TestRunner;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Utilities;
using Color = System.Drawing.Color;
using System.Globalization;
using Nirvana.TestAutomation.BussinessObjects;
using System.Runtime.Caching;


namespace Nirvana.TestAutomation.TestExecutor
{
    internal class TestExecutor
    {
        public enum Status { Not_Run, In_Progress, Fail, Pass, Invalid_Data };
        public enum Dependency { None, Simulator, DropCopy, CheckSide, Compliance, BasketCompliance };
        public enum Values { True, False };
        public static int rowcountforcase;
        public static DataTable NotRunTable = new DataTable();
        private static string previousCaseId = string.Empty;


        // Mastersheet and MasterSheetbACKUP sheet empty or not check
        public static bool TCFILE_BLANK = false;
        public static bool BACKUPFILE_BLANK = false;
        public static string _restoreMasterDBCases = string.Empty;
        public static string _listNonVerificationOnBlotter = string.Empty;
        public static string typeRelease = string.Empty;
        public static string _allFixCasesList = string.Empty;

        /// <summary>
        /// Runs the specified is process kill after test case.
        /// </summary>
        /// <param name="isProcessKillAfterTestCase">if set to <c>true</c> [is process kill after test case].</param>
        public static void Run(bool isProcessKillAfterTestCase)
        {
            try
            {
                ApplicationArguments.SkipDropCopyStartUp = true;
                ApplicationArguments.SkipSimulatorStartUp = true;
                ApplicationArguments.SkipCompliance = true;
                ApplicationArguments.SkipBasketCompliance = true;
                /*if (ConfigurationManager.AppSettings["-runDescription"].ToString().Contains("Dev") || ConfigurationManager.AppSettings["-runDescription"].ToString().Contains("V3.") || ConfigurationManager.AppSettings["-runDescription"].ToString().Contains("Samsara"))
                {
                    typeRelease = "Dev";
                    ApplicationArguments.releaseType = "Dev";
                }
                else
                {
                    typeRelease = "Prod";
                    ApplicationArguments.releaseType = "Prod";
                }*/
                _restoreMasterDBCases = GetAllRestoreDBCases();
                _listNonVerificationOnBlotter = GetNonVerificationCasesList();
                _allFixCasesList = GetAllFixCasesList(); // Get the list of Fix cases
                bool isRestartDone = false;
                if (ConfigurationManager.AppSettings["viaCmdArguments"].Equals("True"))
                {
                    TestCasesForRun();
                }
                DataUtilities.UpdateCheck(_listNonVerificationOnBlotter);
                if (ApplicationArguments.TestCasesDictionary.Count > 0)
                {
                    string testcaseID = ApplicationArguments.TestCaseToBeRun;
                    // var temp = (ApplicationArguments.TestCasesDictionary["Regression Test Cases.xlsx"]).FirstOrDefault().Value.FirstOrDefault();
                    var temp = string.Empty;
                    if (ApplicationArguments.TestCasesDictionary.ContainsKey("Regression Test Cases.xlsx"))
                    {
                        var dictionaryValue = ApplicationArguments.TestCasesDictionary["Regression Test Cases.xlsx"];
                        if (dictionaryValue.Count > 0)
                        {
                            var innerDictionary = dictionaryValue.First().Value;
                            if (innerDictionary.Count > 0)
                            {
                                temp = innerDictionary.First().ToString();
                            }
                        }
                    }

                    bool isFirstRun = true;
                    TestStatusLog.Initialize(CommonMethods.GetLogPath(ApplicationArguments.LogFolder, ApplicationArguments.RunDescription));

                    if (!ApplicationArguments.SkipStartUp && !ApplicationArguments.SkipLogin)
                    {
                        if (!string.IsNullOrEmpty(previousCaseId))
                        {
                            if ((_restoreMasterDBCases.Contains(previousCaseId) && !string.IsNullOrEmpty(previousCaseId)))
                            {
                                if (!isRestartDone)
                                {
                                    ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                    isRestartDone = true;
                                }
                            }
                            else
                            {
                                if ((!string.IsNullOrEmpty(testcaseID)) && _restoreMasterDBCases.Contains(testcaseID))
                                {
                                    if (!isRestartDone)
                                    {
                                        ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                        isRestartDone = true;
                                    }
                                }
                                else
                                {
                                    if (!isRestartDone)
                                    {
                                        isRestartDone = true;
                                        ShutdownReleaseAndCleanData();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(testcaseID)) && _restoreMasterDBCases.Contains(testcaseID))
                            {
                                if (!isRestartDone)
                                {
                                    ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                    isRestartDone = true;
                                }
                            }
                            else
                            {
                                if (!isRestartDone)
                                {
                                    isRestartDone = true;
                                    ShutdownReleaseAndCleanData();
                                }
                            }
                        }
                    }

                    ReleaseControlManager.Initialise();



                    foreach (string workbook in ApplicationArguments.TestCasesDictionary.Keys)
                    {
                        foreach (string sheet in ApplicationArguments.TestCasesDictionary[workbook].Keys)
                        {
                            ApplicationArguments.RetryCount = 0;
                            //  ApplicationArguments.RetrySize = ApplicationArguments.TestCasesDictionary[workbook][sheet].Count; 
                            ApplicationArguments.RetrySize = Math.Min(1, int.Parse(ConfigurationManager.AppSettings["-InternalRetrySize"]));

                            int i = 0;
                            for (i = 0; i < ApplicationArguments.TestCasesDictionary[workbook][sheet].Count; i++)
                            {
                                if (i > 0)
                                {
                                    isRestartDone = false;
                                }
                                string singletestcase = ApplicationArguments.TestCasesDictionary[workbook][sheet][i];
                                if (ApplicationArguments.RetryCount >= ApplicationArguments.RetrySize && !ApplicationArguments.SkipStartUp)
                                {
                                    if (!string.IsNullOrEmpty(previousCaseId) && (!isRestartDone))
                                    {
                                        //CoreExecutor.ShutDownSamsaraRelease();
                                        if (_restoreMasterDBCases.Contains(previousCaseId) && !string.IsNullOrEmpty(previousCaseId))
                                        {
                                            isRestartDone = true;
                                            ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                        }
                                        else
                                        {
                                            isRestartDone = true;
                                            ShutdownReleaseAndCleanData();
                                        }
                                    }
                                    else
                                    {
                                        if (!isRestartDone)
                                        {
                                            ShutdownReleaseAndCleanData();
                                            isRestartDone = true;
                                        }
                                    }

                                    ApplicationArguments.SkipStartUp = false;
                                    ApplicationArguments.SkipLogin = false;
                                }
                                previousCaseId = testcaseID;
                                if (!string.IsNullOrWhiteSpace(singletestcase))
                                {
                                    ApplicationArguments.TestCaseToBeRun = singletestcase;
                                    ReleaseControlManager.UpdateNewTestCaseStarted();
                                    ApplicationArguments.Workbook = workbook;
                                    ApplicationArguments.SheetName = sheet;

                                    RunExecutable(isProcessKillAfterTestCase);

                                    if (isFirstRun)
                                    {
                                        /* skip login and startup for the future runs in case of multiple runs */
                                        isFirstRun = false;
                                        ApplicationArguments.SkipLogin = true;
                                        ApplicationArguments.SkipStartUp = true;
                                        ApplicationArguments.DownloadData = false;
                                    }
                                    if (singletestcase.Contains(AutomationStepsConstants.CLEAN_UP_MODULE))
                                    {
                                        ApplicationArguments.SkipLogin = false;
                                        ApplicationArguments.SkipStartUp = false;
                                        isFirstRun = true;
                                    }
                                    if (singletestcase.Contains(AutomationStepsConstants.CONFIG))
                                    {
                                        if (!string.IsNullOrEmpty(temp))
                                        {
                                            if (_restoreMasterDBCases.Contains(temp) || (_restoreMasterDBCases.Contains(testcaseID) && !string.IsNullOrEmpty(testcaseID)))
                                                ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                            else
                                                ShutdownReleaseAndCleanData();
                                        }
                                        else
                                        { ShutdownReleaseAndCleanData(); }

                                        ApplicationArguments.SkipLogin = false;
                                        ApplicationArguments.SkipStartUp = false;
                                        isFirstRun = true;
                                    }
                                    if (ApplicationArguments.ExitCode == 0)
                                    {
                                        isFirstRun = true;
                                    }
                                    switch (ApplicationArguments.ExitCode)
                                    {
                                        case -1:    //-1 means process was killed due to time limit exceed                            
                                        case 102:
                                            // 102 means the UI was blocked. So will be restarting the application here
                                            if (!string.IsNullOrEmpty(temp))
                                            {
                                                if (_restoreMasterDBCases.Contains(temp) || (_restoreMasterDBCases.Contains(testcaseID) && !string.IsNullOrEmpty(testcaseID)))
                                                    ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                                else
                                                    ShutdownReleaseAndCleanData();
                                            }
                                            else
                                            { ShutdownReleaseAndCleanData(); }
                                            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                            {
                                                SamsaraHelperClass.LogOutUser();
                                                SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                            }
                                            Process[] _processes = Process.GetProcessesByName("Prana");
                                            foreach (Process proc in _processes)
                                                proc.Kill();
                                            // killed the prcess nor restarting the application with login  
                                            ApplicationArguments.SkipLogin = false;
                                            ApplicationArguments.SkipStartUp = false;
                                            ApplicationArguments.TestCaseToBeRun = singletestcase;
                                            ReleaseControlManager.UpdateNewTestCaseStarted();
                                            if (ApplicationArguments.TestCasesDictionary[ApplicationArguments.Workbook][ApplicationArguments.SheetName][ApplicationArguments.TestCasesDictionary[workbook][sheet].Count - 1].ToString().Equals(singletestcase))
                                                ApplicationArguments.TestCasesDictionary[ApplicationArguments.Workbook][ApplicationArguments.SheetName].RemoveAt(ApplicationArguments.TestCasesDictionary[workbook][sheet].Count - 1);
                                            RunExecutable(isProcessKillAfterTestCase);
                                            if (ApplicationArguments.ExitCode == 102) // if we get the same error again, then restart the app and run the next case
                                            {
                                                // ShutdownReleaseAndCleanData();
                                                if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                                {
                                                    SamsaraHelperClass.LogOutUser();
                                                    SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                                }
                                                _processes = Process.GetProcessesByName("Prana");
                                                foreach (Process proc in _processes)
                                                    proc.Kill();
                                                isFirstRun = true;
                                                ApplicationArguments.RetryCount++;

                                                continue;
                                            }
                                            else
                                            {
                                                ApplicationArguments.SkipLogin = true;
                                                ApplicationArguments.SkipStartUp = true;
                                            }
                                            break;

                                        case 101:
                                            // Kill the prana application if test case cancelled by user and restart the prana.
                                            //SamsaraHelperClass.LogOutUser();
                                            ProcessKiller();
                                            Process[] _processesPrana = Process.GetProcessesByName("Prana");
                                            foreach (Process proc in _processesPrana)
                                                proc.Kill();
                                            ApplicationArguments.SkipLogin = false;
                                            ApplicationArguments.SkipStartUp = true;
                                            RunExecutable(isProcessKillAfterTestCase);
                                            break;

                                        case 2:
                                            // Case failed due to machine specific error, restart the app and run the next case.
                                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MachineIssues"]))
                                            {
                                                //adding check as currently blacklisting machines not ideal 
                                                blacklistIP(ApplicationArguments.ExitCode);
                                            }
                                            if (!string.IsNullOrEmpty(temp))
                                            {
                                                if (_restoreMasterDBCases.Contains(temp) || (_restoreMasterDBCases.Contains(testcaseID) && !string.IsNullOrEmpty(testcaseID)))
                                                    ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                                else
                                                    ShutdownReleaseAndCleanData();
                                            }
                                            else
                                            { ShutdownReleaseAndCleanData(); }
                                            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                            {
                                                SamsaraHelperClass.LogOutUser();
                                                SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                            }
                                            Process[] _processPrana = Process.GetProcessesByName("Prana");
                                            foreach (Process proc in _processPrana)
                                                proc.Kill();
                                            isFirstRun = true;
                                            ApplicationArguments.SkipLogin = false;
                                            ApplicationArguments.SkipStartUp = false;
                                            ApplicationArguments.RetryCount++;
                                            continue;


                                        case 0:
                                            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                            {
                                                SamsaraHelperClass.LogOutUser();
                                                SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                                            }
                                            // ShutdownReleaseAndCleanData();
                                            //ProcessKiller();
                                            Process[] process = Process.GetProcessesByName("Prana");
                                            foreach (Process proc in process)
                                                proc.Kill();
                                            ApplicationArguments.SkipLogin = false;
                                            ApplicationArguments.SkipStartUp = false;
                                            break;
                                        case 3:
                                            // machine specific issue related to common reason
                                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CommonIssues"])) // adding condition as blacklistingIP not required until CommonIssues are updated on App.config
                                            {
                                                blacklistIPByCommonIssue(ApplicationArguments.ExitCode);
                                            }
                                            if (!string.IsNullOrEmpty(temp))
                                            {
                                                if (_restoreMasterDBCases.Contains(temp) || (_restoreMasterDBCases.Contains(testcaseID) && !string.IsNullOrEmpty(testcaseID)))
                                                    ShutdownReleaseAndCleanData(ApplicationArguments.MasterDB);
                                                else
                                                    ShutdownReleaseAndCleanData();
                                            }
                                            else
                                            { ShutdownReleaseAndCleanData(); }

                                            break;
                                    }
                                    ApplicationArguments.RetryCount++;
                                }
                                if (ApplicationArguments.ExitCode == 1)
                                {
                                    UpdateStatus(MessageConstants.LOGIN_ERROR);
                                    if ((!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MachineIssues"])) && (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MachineIssues"])))
                                    {
                                        blacklistIP(ApplicationArguments.ExitCode);
                                    }
                                    break;
                                }
                            }
                            if (ApplicationArguments.ExitCode == 1)
                                break;
                        }
                        if (ApplicationArguments.ExitCode == 1)
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Test Case Dictionary is Empty");
                }
                SendNotification();
                /*CompressScreenshotFolder();
                DeleteScreenshotFolder();*/
                if (ApplicationArguments.ExitCode == 1)
                    Environment.Exit(1);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, ex.Message);
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue in Test Case Dictionary in Run class]"); 
            }
        }

        /// <summary>
        /// Delete Uncompressed Screenshot folder
        /// </summary>
        private static void DeleteScreenshotFolder()
        {
            try
            {
                string directoryPath = CommonMethods.DirectoryFixPath;
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                    Console.WriteLine("Delete folder");
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error while deleting Screenshot folder");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue while deleting " + CommonMethods.DirectoryFixPath + " folder]"); 
            }
        }

        /// <summary>
        /// Compress Screenshot folder into zip file
        /// </summary>
        private static void CompressScreenshotFolder()
        {
            // Delete The Previous Month Zip Folders There .
            try
            {
                string filePath = ApplicationArguments.ApplicationStartUpPath + "\\" + "Screenshots" + "\\";
                DirectoryInfo d = new DirectoryInfo(filePath);
                //To check Screenshots folder
                if (!Directory.Exists(filePath))
                {
                    foreach (var file in d.GetFiles("*.zip"))
                    {
                        DateTime zipDate = DateTime.ParseExact(file.Name.Substring(0, file.Name.Length - 4), "MM-dd-yy-hh-mm-ss", CultureInfo.InvariantCulture);
                        if (!zipDate.Month.Equals(DateTime.Now.Month))
                        {
                            if (File.Exists(file.FullName))
                            {
                                File.Delete(file.FullName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error while CompressScreenshot folder");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue while compressing " + ApplicationArguments.ApplicationStartUpPath + "\\" + "Screenshots" + "\\" + " folder]"); 
            }

            try
            {
                string directoryPath = CommonMethods.DirectoryFixPath;
                string zipPath = directoryPath + ".zip";
                if (System.IO.Directory.Exists(directoryPath))
                {
                    ZipFile.CreateFromDirectory(directoryPath, zipPath);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue while compressing " + CommonMethods.DirectoryFixPath + " folder]"); 
            }
        }

        /// <summary>
        /// Sends the email notification on test Case failure.
        /// </summary>
        private static void SendNotification()
        {
            try
            {
                bool sendEmailNotifications = ApplicationArguments.SendEmailNotifcations;
                bool uploadSlaveTestReport = ApplicationArguments.UploadSlaveTestReport;
                // if Master is Download the Slaves File to Merge And Send Again
                if (ApplicationArguments.OnMasterConfig)
                {
                    string filePath = ApplicationArguments.ApplicationStartUpPath + "\\" + "MasterLogs" + "\\";
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    DataFolderDownloader.DownloadFolder(ApplicationArguments.ReportFolderId, "MasterLogs", ApplicationArguments.ApplicationStartUpPath, true);
                    MergeFile();
                    string logPath = ApplicationArguments.ApplicationStartUpPath + "\\MasterLogs\\" + "Master" + "(Distributed)-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                    DataFolderUploader.UploadFolder(ApplicationArguments.ReportFolderId, logPath, ApplicationArguments.ApplicationStartUpPath, true);
                    sendEmailNotifications = true;
                }
                else if (uploadSlaveTestReport)     // if UploadData true then Slvae Upload s The Sheets To Drive
                {
                    string logPath = CommonMethods.GetLogPath(ApplicationArguments.LogFolder, ApplicationArguments.RunDescription);
                    DataFolderUploader.UploadFolder(ApplicationArguments.ReportFolderId, logPath.Replace("xml", "xlsx"), ApplicationArguments.ApplicationStartUpPath, true);
                }
                if (ApplicationArguments.OnMasterConfig && sendEmailNotifications)    // The Master Send The Mails To Users.
                {
                    DataFolderUploader.SearchFile(ApplicationArguments.ApplicationStartUpPath, "Master" + "(Distributed)-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx");
                    SendMail();
                    // Delete The MasterLog Folder From The Local PC
                    try
                    {
                        string filePath = ApplicationArguments.ApplicationStartUpPath + "\\" + "MasterLogs" + "\\";
                        var dir = new DirectoryInfo(@filePath);
                        dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                        dir.Delete(true);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (sendEmailNotifications)  // Normal Local User Want To SEnd The Report
                {
                    string logPath = CommonMethods.GetLogPath(ApplicationArguments.LogFolder, ApplicationArguments.RunDescription);
                    DataFolderUploader.UploadFolder(ApplicationArguments.ReportFolderId, logPath.Replace("xml", "xlsx"), ApplicationArguments.ApplicationStartUpPath, true);
                    DataFolderUploader.SearchFile(ApplicationArguments.ApplicationStartUpPath, ApplicationArguments.RunDescription + ".xlsx");
                    SendMail();
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error while SendNotification");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue in SendNotification class]");
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        public static void SendMail()
        {
            try
            {
                string senderEmail = ApplicationArguments.SenderEmail;
                string receiverEmail = ApplicationArguments.ReceiverEmail;
                string subject = "Prana Automated Testing Report " + ApplicationArguments.RunDescription.Substring(0, ApplicationArguments.RunDescription.Length - 23); //remove date and time of fixed length 23 from the run description

                string testData = CommonMethods.GetFolderLinkFromId(ApplicationArguments.DriveFolderId);
                string message = CommonMethods.GetEmailContent();
                string senderPassword = "Nirvana@@1234567";
                string hostServerName = "smtp.gmail.com";
                string hostPort = "587";

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(senderEmail, receiverEmail, subject, message);
                mail.IsBodyHtml = true;
                string mailTocc = ApplicationArguments.CcEmail;
                if (!mailTocc.Equals(string.Empty))
                    foreach (string email in mailTocc.Split(','))
                    {
                        mail.CC.Add(email);
                    }
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(hostServerName, int.Parse(hostPort));
                smtp.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at SendMail class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue in SendMail class]");
            }
        }

        public static TestResult ClearAllFixLog()
        {
            TestResult _result = new TestResult();
            int MAX_RETRY = 3;
            int RETRY_DELAY_MS = 1000;
            try
            {
                string logPath = ConfigurationManager.AppSettings["AllFixLog"];
                string shouldClear = ConfigurationManager.AppSettings["ClearAllFixLogs"];

                if (string.IsNullOrEmpty(logPath))
                {
                    throw new Exception("Log path is not configured in AppSettings with key 'AllFixLog'.");
                }

                if (string.IsNullOrWhiteSpace(shouldClear) || !shouldClear.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("ClearAllFixLogs is not enabled. Skipping log file deletion.");
                    return _result; // No action needed
                }

                bool isDeleted = false;
                int attempt = 0;

                while (attempt < MAX_RETRY)
                {
                    attempt++;

                    try
                    {
                        if (File.Exists(logPath))
                        {
                            File.Delete(logPath);
                        }

                        if (!File.Exists(logPath))
                        {
                            isDeleted = true;
                            break;
                        }

                        Thread.Sleep(RETRY_DELAY_MS);
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(RETRY_DELAY_MS);
                    }
                }

                if (!isDeleted)
                {
                    throw new Exception("Failed to delete log file after " + MAX_RETRY + " attempts: " + logPath);
                }

                Console.WriteLine("AllFixLog file cleared successfully.");
                return _result;
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                _result.ErrorMessage = "Exception occurred while clearing log: " + ex.Message;

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;

                return _result;
            }
        }
        /// <summary>
        /// Shutdowns the release and clean data.
        /// </summary>
        private static void ShutdownReleaseAndCleanData(string masterDB = "")
        {
            try
            {
                
                Process[] myProcesses = Process.GetProcesses();
                string TempInterceptorFile = ConfigurationManager.AppSettings["TempInterceptorFile"];
                string TempCounterPartyFile = ConfigurationManager.AppSettings["TempCounterPartyFile"];
                ICommandUtilities cmdUtilities = null;
                bool isportfoliodbrestored = false;
                bool isCasePortfolio = false;

                if (ConfigurationManager.AppSettings["-IsAllFixLogCases"].ToLower().Equals("true"))
                {
                    if (_allFixCasesList.Contains(ApplicationArguments.TestCaseToBeRun))
                    {
                        CoreExecutor.ReplaceCounterPartyCustomMapping();
                        Console.WriteLine("CounterPartyCustomMapping File replaced");
                    }
                }
                if (!string.IsNullOrEmpty(ApplicationArguments.testcaseTracker.TestCaseID) && ApplicationArguments.testcaseTracker.TestCaseID.Contains("-") && string.Equals(ApplicationArguments.testcaseTracker.PreRequisiteType, "Portfolio", StringComparison.OrdinalIgnoreCase) )
                {
                    string fullPathMaster = Path.Combine(ApplicationArguments.PortfolioDBBackUpsMaster, ApplicationArguments.testcaseTracker.TestCaseID); 
                    FileHelper.DeleteFolder(fullPathMaster);
                    isCasePortfolio = true;
                    if (!string.IsNullOrEmpty(ApplicationArguments.testcaseTracker.Result) && string.Equals(ApplicationArguments.testcaseTracker.Result, "Pass", StringComparison.OrdinalIgnoreCase))  
                    {
                        try
                        {
                            CloseProcessesAndExecuteCommands(typeRelease, ApplicationArguments.MasterDB,ApplicationArguments.DataBasePath , true, fullPathMaster);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                             isCasePortfolio  = false;
                        }
                      
                    }
                    else
                    {
                        Console.WriteLine("TestCase:"+ApplicationArguments.testcaseTracker.TestCaseID+" status: "+ ApplicationArguments.testcaseTracker.Result+".Reason :"+ApplicationArguments.testcaseTracker.Log+"Therefore no DB Backup");
                    }
                    
                }
                if (isCasePortfolio)
                {
                    Console.WriteLine("Portfolio-related test case processed successfully.");
                }
                

                if (!string.IsNullOrEmpty(ApplicationArguments.isPreRequisiteType) && ApplicationArguments.isPreRequisiteType.Contains("-") && !string.Equals(ApplicationArguments.isPreRequisiteType, "Portfolio", StringComparison.OrdinalIgnoreCase))
                {
                    string fullPathMaster = Path.Combine(ApplicationArguments.PortfolioDBBackUpsMaster, ApplicationArguments.isPreRequisiteType);
                    string fullPathSlave = Path.Combine(ApplicationArguments.PortfolioDBBackUpsSlave, ApplicationArguments.isPreRequisiteType);

                     if ( (Directory.Exists(fullPathMaster) && FileHelper.CopyDirectory(fullPathMaster, fullPathSlave)))  //&& FileHelper.DeleteFilesOlderThanToday(fullPathMaster)
                     {
                         try
                         {
                             CloseProcessesAndExecuteCommands(typeRelease, ApplicationArguments.MasterDB, fullPathSlave);

                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine(ex.Message);
                             isportfoliodbrestored = false;
                         }
                         isportfoliodbrestored = true;

                     }
                     else
                     {
                         //method to check db files is of today's date
                         ApplicationArguments.testcaseTracker.portfolioDBRestoreFail = "true";
                         //need to throw exception //mark testcase as fail//as there is no portfolioDb

                     }
                }


                if (isportfoliodbrestored)
                {
                    Console.WriteLine("PortfolioCase Triggered with portfolioDB");
                }
                // Restore Database functionality configurable
                else if (ConfigurationManager.AppSettings["IsRestoreDatabase"].ToString() == "true" && !isportfoliodbrestored)
                {
                    foreach (Process myProcess in myProcesses)
                    {

                        if (myProcess.MainWindowTitle.Contains("Prana TradeService UI"))
                        {
                            myProcess.CloseMainWindow();
                        }
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
                    cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                    cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                    ShutDownSimulator();
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["ShutDownSimulator"]))
                    {
                        cmdUtilities.ExecuteCommand("ShutDownSimulator.Bat");
                    }
                    cmdUtilities.ExecuteCommand("RevertingPref.bat");

                    // Restore Database after execution of every testcase
                    if (!string.IsNullOrEmpty(masterDB))
                    {
                        cmdUtilities.ExecuteCommand("RestoreDBScript.bat", masterDB);

                    }
                    else
                        cmdUtilities.ExecuteCommand("RestoreDBScript.bat");
                    CoreExecutor.count = 0;
                }
                else
                {

                    if (!string.IsNullOrEmpty(masterDB))
                    {
                        foreach (Process myProcess in myProcesses)
                        {

                            if (myProcess.MainWindowTitle.Contains("Prana TradeService UI"))
                            {
                                myProcess.CloseMainWindow();
                            }
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

                        cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                        cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                        ShutDownSimulator();
                        cmdUtilities.ExecuteCommand("RevertingPref.bat");
                        cmdUtilities.ExecuteCommand("RestoreDBScript.bat", masterDB);
                        ClearAllFixLog();
                        CoreExecutor.count = 0;

                    }
                    else if (!ApplicationArguments.SkipBasketCompliance)
                    {
                        foreach (Process myProcess in myProcesses)
                        {

                            if (myProcess.MainWindowTitle.Contains("Prana TradeService UI"))
                            {
                                myProcess.CloseMainWindow();
                            }
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

                        cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                        cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                        ShutDownSimulator();
                        cmdUtilities.ExecuteCommand("RevertingPref.bat");
                        cmdUtilities.ExecuteCommand("RestoreDBScript.bat");
                        CoreExecutor.count = 0;
                    }

                    else
                    {
                        cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.SqlQuery);
                        cmdUtilities.ExecuteCommand("DeleteClientData.sql");
                        cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                        cmdUtilities.ExecuteCommand("RevertingPref.bat");
                        //cmdUtilities.ExecuteCommand("Application_Kill.bat");
                        StartAppplication();
                    }
                }

                string TCFile = ApplicationArguments.MasterSheetPath;
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(TCFile)))
                {
                    int colCount, totalRowCount, rowCount;
                    Dictionary<string, int> colDictionary = new Dictionary<string, int>();
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    colCount = DataUtilities.GetLastUsedColumn(worksheet);
                    totalRowCount = DataUtilities.GetLastUsedRow(worksheet);
                    colDictionary = DataUtilities.ColToIndexMapping(worksheet, colCount);
                    for (rowCount = 2; rowCount <= totalRowCount; rowCount++)
                    {

                        if (worksheet.Cells[rowCount, colDictionary["Status"]].Value.Equals(Status.In_Progress.ToString()))
                        {
                            if (!ApplicationArguments.SkipBasketCompliance)
                            {
                                SqlUtilities.ExecuteQuery("Update T_CA_CompliancePreferences Set IsBasketComplianceEnabledCompany = 1 Where CompanyId > 0");

                                SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission Set IsPreTradeEnabled = 1, Trading = 1, IsApplyToManual = 1, Staging = 1, EnableBasketComplianceCheck = 1 where CompanyId > 0");
                                Console.WriteLine("Basket Compliance Enabled");

                                // query to enable basket permission check to user specific
                                SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '1' where UserId = '17' OR UserId= '18'");
                                if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                {
                                    SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '1' where UserId = '35'");
                                }
                                Console.WriteLine("Basket Compliance PRE-TRADE CHECK PERMISSION MOVED TO BASKET INSTEAD OF REGULAR");


                            }
                            if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.CheckSide.ToString()))
                            {
                                SqlUtilities.ExecuteQuery("Update T_AL_AllocationDefaultRule SET CheckSidePreference = '{" + "DisableCheckSidePref" + ":{}," + "DoCheckSideSystem" + ":true}' where CompanyId > 0");
                                Console.WriteLine("Triggered true query");
                            }
                        }
                    }
                }



                // check if symbol_bak file exist...if exist then interceptor file needs to be set to default

                if (File.Exists(TempInterceptorFile))
                {
                    string DefaultInterceptorFile = ConfigurationManager.AppSettings["DefaultInterceptorFile"];
                    System.IO.File.Copy(TempInterceptorFile, DefaultInterceptorFile, true);
                    System.IO.File.Delete(TempInterceptorFile);

                }
                // Replace CounterPart File back to default
                if (!_allFixCasesList.Contains(ApplicationArguments.TestCaseToBeRun))
                {
                    if (File.Exists(TempCounterPartyFile))
                    {
                        string DefaultCounterPartyFile = ConfigurationManager.AppSettings["DefaultCounterPartyFile"];
                        System.IO.File.Copy(TempCounterPartyFile, DefaultCounterPartyFile, true);
                        System.IO.File.Delete(TempCounterPartyFile);
                        Console.WriteLine("Replace orginal file of Counterpartycustommapping file");
                    }
                }
                

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at ShutdownReleaseAndCleanData class");
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message + " [issue in ShutdownReleaseAndCleanData class]");
            }
            finally
            {
                //PRANA client returned to default configuration in key's value 
                ConfigModificatorSettings _file = new ConfigModificatorSettings(ApplicationArguments.ClientReleasePath + "\\Prana.exe.config");
                String path = "";
                ConfigUpdater.ChangeValueByKey(TestDataConstants.ISDEFAULTFILTERTOSHOWACCOUNTWISEDATAONDAILYVALUATION, Values.False.ToString(), _file);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.ISFILTERINGACCOUNTWISEDATAALLOWEDONDAILYVALUATION, Values.False.ToString(), _file);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.ISCSVEXPORTENABLED_PTT, Values.False.ToString(), _file);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.CSVEXPORTFILEPATH_PTT, path, _file);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.ISGROUPINGREQBLOTTERSTAGEIMPORT, Values.False.ToString(), _file);

                //Expnl returned to default configuration in key's value 
                String value1 = "0";
                ConfigModificatorSettings _file1 = new ConfigModificatorSettings(ApplicationArguments.ExpnlReleasePath + "\\Prana.ExpnlServiceHost.exe.config");
                ConfigUpdater.ChangeValueByKey(TestDataConstants.EQUITYSWAPSMARKETVALUEASEQUITY, Values.False.ToString(), _file1);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.FETCHDATABYDATE, value1, _file1);

                //Server returned to default configuration in key's value
                ConfigModificatorSettings _file2 = new ConfigModificatorSettings(ApplicationArguments.ServerReleasePath + "\\Prana.TradeServiceHost.exe.config");
                ConfigUpdater.ChangeValueByKey(TestDataConstants.MATCHCLOSINGTRANSACTIONATPORTFOLIOONLY, Values.True.ToString(), _file2);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.USECACHEINALLOCATION, Values.False.ToString(), _file2);
                ConfigUpdater.ChangeValueByKey(TestDataConstants.CREATECACHEINALLOCATION, Values.True.ToString(), _file2);
            }



        }

        private static void StartAppplication()
        {
            ConfigModificatorSettings _file = new ConfigModificatorSettings(ApplicationArguments.ServerReleasePath + "\\Prana.TradeServiceHost.exe.config");
            ConfigUpdater.ChangeValueByKey("MultiDayTIF", "", _file);
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                DataUtilities.UpdateJson(ConfigurationManager.AppSettings["SamsaraDirectory"] + "\\public\\config\\config.json", "Always_PopUp_Short_Locate", false);
                DataUtilities.UpdateJsonFileKeys(ConfigurationManager.AppSettings["SamsaraDirectory"] + "\\public\\config\\config.json", "CONFIG_RTPNL_MAX_WIDGETS_LIMIT", 14);
                DataUtilities.UpdateJsonFileKeys(ConfigurationManager.AppSettings["SamsaraDirectory"] + "\\public\\config\\config.json", "CONFIG_RTPNL_MAX_GROUPING_WIDGET_LIMIT", 12);
            }  
            CoreExecutor cs = new CoreExecutor();
            if (cs.CountCurrentTestcase() > 0)
            {
                
                //cs.StartSamsaraApplication();
                // to check the check type is single user based or multiple user
                bool caseType = cs.IsCaseMultiUser();
                VideoRecording(ApplicationArguments.TestCaseToBeRun, "startRecording", "fail");
                if (!caseType)
                {
                    ConfigurationManager.AppSettings["IsMultiUser"] = "false";
                    ApplicationArguments.SamsaraReleaseUserName = ConfigurationManager.AppSettings["-firstUser"].ToString();//Support1
                    ApplicationArguments.SamsaraActiveUser = ConfigurationManager.AppSettings["-firstUser"].ToString();
                }
                else
                {
                    ConfigurationManager.AppSettings["IsMultiUser"] = "true";
                    ApplicationArguments.SamsaraReleaseUserName = ConfigurationManager.AppSettings["-samsaraMultiUser"].ToString();
                    ApplicationArguments.ReleaseUserName = ConfigurationManager.AppSettings["-firstUser"].ToString();
                }
                
                
                if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                {
                    cs.StartSamsaraYarnStart();
                    SamsaraHelperClass.RunOpenFinTest("samsara", "", "", "LoginClient");
                    cs.LoginSamsaratype = true;
                }
                if (ConfigurationManager.AppSettings["IsMultiUser"] == "true" || !ApplicationArguments.ProductDependency.ToLower().Equals("samsara"))
                {
                    cs.EnterpriseLogin();
                }
                cs.RefreshExpnlUi();
                try
                {
                    cs.ClearCache();
                }
                catch (Exception clearCacheException)
                {
                    Console.WriteLine("Exception in clear cache so set count to 0" + clearCacheException.Message);
                    cs.setCountCurrentTestcase(0);
                }
                if (ApplicationArguments.TestCaseToBeRun.ToString().Contains("RTPNL"))
                {
                    cs.RestartRtpnlService();
                }
                else if (!ApplicationArguments.SkipCompliance)
                {
                    cs.RestartRtpnlService();
                }
                cs.ClearSimulator();
            }
        }
        public static void CloseProcessesAndExecuteCommands(string typeRelease, string masterDB, string DB_Path = "", bool backupDBbeforeEmpty = false, string DB_BackupPath = "")
        {
            try
            {
                Process[] myProcesses = Process.GetProcesses();

                foreach (Process myProcess in myProcesses)
                {
                    if (myProcess.MainWindowTitle.Contains("Prana TradeService UI"))
                    {
                        myProcess.CloseMainWindow();
                    }
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
                    if (myProcess.MainWindowTitle.Contains("'Basket Compliance Service'"))
                    {
                        myProcess.CloseMainWindow();
                    }
                }
                ICommandUtilities cmdUtilities = null;
                cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                ShutDownSimulator();
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["ShutDownSimulator"]))
                {
                    cmdUtilities.ExecuteCommand("ShutDownSimulator.Bat");
                }
                string original_Path = ApplicationArguments.DataBasePath;
                //backup db.
                if (backupDBbeforeEmpty && !string.IsNullOrEmpty(DB_BackupPath))
                {
                    if (!Directory.Exists(DB_BackupPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(DB_BackupPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error creating backup directory: "+ex.Message);
                            
                        }
                    }
                    ApplicationArguments.DataBasePath = DB_BackupPath;
                    cmdUtilities.ExecuteCommand("DBBackUpScript.bat");
                    ApplicationArguments.DataBasePath = original_Path; 
                }


                cmdUtilities.ExecuteCommand("RevertingPref.bat");

                // Restore Database after execution of every testcase

                if (!string.IsNullOrEmpty(DB_Path) && string.IsNullOrEmpty(DB_BackupPath))
                {
                    ApplicationArguments.DataBasePath = DB_Path;
                    if (!string.IsNullOrEmpty(masterDB))
                    {
                        cmdUtilities.ExecuteCommand("RestoreDBScript.bat", masterDB);

                    }
                    else
                        cmdUtilities.ExecuteCommand("RestoreDBScript.bat");
                   
                }

                ApplicationArguments.DataBasePath = original_Path; 
                CoreExecutor.count = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during process closing or command execution: " + ex.Message);
            }
        }
        public static void ShutDownSimulator()
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
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at ShutDownSimulator class");
                throw new Exception(ex.Message + " [issue in ShutDownSimulator class]");
            }
        }

        /// <summary>
        /// Runs the executable.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        private static void RunExecutable(bool isProcessKillAfterTestCase)
        {
            try
            {
                TestAutomationFX.Core.Log.LogToConsoleEnabled = true;
                var coreExec = new CoreExecutor();
                //if (!ApplicationArguments.SkipStartUp)
                //    coreExec.ApplicationStartUp();
                //if (!ApplicationArguments.SkipLogin)
                //    coreExec.ApplicationLogin();
                coreExec.UserDefinedTestCases();
                coreExec.Dispose();
                if (isProcessKillAfterTestCase)
                {
                    // Causes testautomation to fail as testautomationuser does not have admin rights
                    //KillUnexceptedProcess();
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at RunExecutable class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue in RunExecutable class]");
            }
        }

        /// <summary>
        /// Merge The Files 
        /// </summary>
        /// 
        public static void MergeFile()
        {
            string logPath = string.Empty;
            try
            {
                //Check Whether The Master File Exist Or Not
                logPath = ApplicationArguments.ApplicationStartUpPath + "\\MasterLogs\\" + "Master" + "(Distributed)-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                if (!File.Exists(logPath))
                    CreateMasterSheet();  // Create New Master Sheet
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at MergeFile class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue while creating mastersheet " + logPath + " in  MergeFile class]");
            }
            try
            {
                int Pass = 0;
                int Fail = 0;
                string filePath = ApplicationArguments.ApplicationStartUpPath + "\\" + "MasterLogs" + "\\";
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath + "Master" + "(Distributed)-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx")))
                {
                    var MasterWorkSheet = xlPackage.Workbook.Worksheets["TEST REPORT"];  // New WorkSheet TestReport
                    DirectoryInfo d = new DirectoryInfo(filePath);
                    int masterRow = MasterWorkSheet.Dimension.End.Row + 1;

                    foreach (var file in d.GetFiles("*.xlsx"))
                    {
                        if (file.Name.Contains("(Distributed)") && !file.Name.Contains("Master"))
                            using (ExcelPackage childPackage = new ExcelPackage(new FileInfo(file.FullName)))
                            {
                                int childRow = 18;
                                int childTotalRow = 0;
                                ExcelWorksheet ChildWorkSheet = childPackage.Workbook.Worksheets["TestReport"];
                                try
                                {
                                    childTotalRow = ChildWorkSheet.Dimension.End.Row;
                                }
                                catch (NullReferenceException ex)
                                {
                                    bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                                    if (rethrow)
                                        throw;
                                }
                                // Merging The Total Time Of All Sheets
                                var TestTime = TimeSpan.Parse("00:00:00");
                                TestTime = TimeSpan.Parse(MasterWorkSheet.Cells[10, 3].Value.ToString());
                                TestTime = TestTime + TimeSpan.Parse(ChildWorkSheet.Cells[10, 3].Value.ToString());
                                MasterWorkSheet.Cells[10, 3].Value = TestTime.ToString();
                                MasterWorkSheet.Cells[11, 3].Value = MasterWorkSheet.Cells[11, 3].Value.ToString() + "," + ChildWorkSheet.Cells[11, 3].Value.ToString();
                                //Minimum Start  Time workSheets Needed 
                                MasterWorkSheet.Cells[12, 3].Value = Min(DateTime.Parse(MasterWorkSheet.Cells[12, 3].Value.ToString()), DateTime.Parse(ChildWorkSheet.Cells[12, 3].Value.ToString())).TimeOfDay.ToString();
                                //Maximum End Time 
                                MasterWorkSheet.Cells[13, 3].Value = Max(DateTime.Parse(MasterWorkSheet.Cells[13, 3].Value.ToString()), DateTime.Parse(ChildWorkSheet.Cells[13, 3].Value.ToString())).TimeOfDay.ToString();
                                //  Copying The Data
                                while (!(ChildWorkSheet.Cells[childRow, 2].Value == null))
                                {
                                    var range = MasterWorkSheet.Cells[masterRow, 2, masterRow, 7];
                                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    MasterWorkSheet.Cells[masterRow, 2].Value = ChildWorkSheet.Cells[childRow, 2].Value.ToString();
                                    MasterWorkSheet.Cells[masterRow, 3].Value = ChildWorkSheet.Cells[childRow, 3].Value.ToString();
                                    MasterWorkSheet.Cells[masterRow, 4].Value = ChildWorkSheet.Cells[childRow, 4].Value.ToString();
                                    MasterWorkSheet.Cells[masterRow, 5].Value = ChildWorkSheet.Cells[childRow, 5].Value.ToString();
                                    MasterWorkSheet.Cells[masterRow, 6].Value = ChildWorkSheet.Cells[childRow, 6].Value.ToString();
                                    MasterWorkSheet.Cells[masterRow, 7].Value = ChildWorkSheet.Cells[childRow, 7].Value.ToString();
                                    if (ChildWorkSheet.Cells[childRow, 6].Value.ToString().Equals("Pass"))
                                    {
                                        Pass++;
                                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Green);
                                        range.Style.Fill.BackgroundColor.SetColor(Color.PaleGreen);
                                    }
                                    else
                                    {
                                        Fail++;
                                        range.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Red);
                                        range.Style.Fill.BackgroundColor.SetColor(Color.MistyRose);
                                    }
                                    masterRow++;
                                    childRow++;

                                }
                            }
                    }
                    // Calculating total Pass and Fail Cases
                    MasterWorkSheet.Cells[7, 3].Value = int.Parse(MasterWorkSheet.Cells[7, 3].Value.ToString()) + Pass + Fail;
                    MasterWorkSheet.Cells[8, 3].Value = int.Parse(MasterWorkSheet.Cells[8, 3].Value.ToString()) + Pass;
                    MasterWorkSheet.Cells[9, 3].Value = int.Parse(MasterWorkSheet.Cells[9, 3].Value.ToString()) + Fail;

                    // Save The file
                    xlPackage.Save();
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at MergeFile class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue while editing mastersheet " + logPath + " in MergeFile class]");
            }
        }

        /// <summary>
        /// Minimum Start time Of All The sheets .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static T Min<T>(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y) < 0 ? x : y;
        }

        /// <summary>
        /// Maximum Start time Of All The sheets .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static T Max<T>(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y) < 0 ? y : x;
        }

        /// <summary>
        /// Creates the blank master sheet.
        /// </summary>
        public static void CreateMasterSheet()
        {
            try
            {
                string filePath = ApplicationArguments.ApplicationStartUpPath + "\\" + "MasterLogs" + "\\";
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath + "Master" + "(Distributed)-" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx")))
                {
                    var MasterWorkSheet = xlPackage.Workbook.Worksheets.Add("TEST REPORT");
                    MasterWorkSheet.Cells[5, 2, 15, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    MasterWorkSheet.Cells[5, 2, 15, 2].Style.Font.Bold = true;
                    MasterWorkSheet.Cells[5, 2, 15, 3].Style.Font.Size = 12;
                    MasterWorkSheet.Cells[5, 2, 15, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    MasterWorkSheet.Cells[3, 2, 3, 3].Style.Font.Bold = true;
                    MasterWorkSheet.Cells[3, 2, 3, 3].Style.Font.Size = 14;
                    MasterWorkSheet.Cells[3, 2].Value = ApplicationArguments.RunDescription.Substring(0, ApplicationArguments.RunDescription.Length - 23) + " Test Report ";
                    MasterWorkSheet.Cells[5, 2].Value = TestDataConstants.CAP_PRANA_VERSION;
                    MasterWorkSheet.Cells[6, 2].Value = TestDataConstants.CAP_RELEASE_DATE_TIME;
                    MasterWorkSheet.Cells[6, 3].Value = DateTime.Now.ToString();
                    MasterWorkSheet.Cells[7, 2].Value = TestDataConstants.CAP_TOTAL_TEST_CASES;
                    MasterWorkSheet.Cells[7, 3].Value = 0;
                    MasterWorkSheet.Cells[8, 2].Value = TestDataConstants.CAP_PASSED;
                    MasterWorkSheet.Cells[8, 3].Value = 0;
                    MasterWorkSheet.Cells[9, 2].Value = TestDataConstants.CAP_FAILED;
                    MasterWorkSheet.Cells[9, 3].Value = 0;
                    MasterWorkSheet.Cells[11, 2].Value = TestDataConstants.CAP_TOTAL_TIME_TAKEN;
                    MasterWorkSheet.Cells[11, 3].Value = new TimeSpan(0, 0, 0, 0).ToString();
                    MasterWorkSheet.Cells[12, 2].Value = TestDataConstants.CAP_SYSTEM_IP;
                    MasterWorkSheet.Cells[12, 3].Value = TestStatusLog.GetSytemIP().ToString();
                    MasterWorkSheet.Cells[13, 2].Value = TestDataConstants.TestCase_Start_Time;
                    MasterWorkSheet.Cells[13, 3].Value = "11:59:59 PM";
                    MasterWorkSheet.Cells[14, 2].Value = TestDataConstants.TestCase_End_Time;
                    MasterWorkSheet.Cells[14, 3].Value = "00:00:00 AM";
                    MasterWorkSheet.Cells[15, 2].Value = TestDataConstants.CAP_AUTOMATION_CODE_REVISION;
                    string testAutomationPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Test Automation "));
                    if (testAutomationPath != null)
                    {
                        MasterWorkSheet.Cells[15, 3].Value = TestStatusLog.GetRevision(testAutomationPath);
                        // excelWorksheet.Cells[14, 2].Value = TestDataConstants.CAP_PRANA_CODE_REVISION;
                        string DevPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\.."));
                        MasterWorkSheet.Cells[5, 3].Value = ApplicationArguments.RunDescription.Substring(0, ApplicationArguments.RunDescription.Length - 23) + "." + ((DevPath != null) ? TestStatusLog.GetRevision(DevPath) : 0).ToString();
                    }
                    MasterWorkSheet.Cells[17, 2].Value = TestDataConstants.CAP_MODULE_NAME;
                    MasterWorkSheet.Cells[17, 3].Value = TestDataConstants.CAP_TEST_CASE_ID;
                    MasterWorkSheet.Cells[17, 4].Value = TestDataConstants.CAP_TEST_CASE_DESCRIPTION;
                    MasterWorkSheet.Cells[17, 5].Value = TestDataConstants.CAP_CATEGORY;
                    MasterWorkSheet.Cells[17, 6].Value = TestDataConstants.CAP_ERROR;
                    MasterWorkSheet.Cells[17, 7].Value = TestDataConstants.CAP_RESULT;
                    MasterWorkSheet.Cells[17, 8].Value = TestDataConstants.CAP_RUNNING_TIME;


                    MasterWorkSheet.Column(2).AutoFit();
                    MasterWorkSheet.Column(3).AutoFit();
                    MasterWorkSheet.Column(4).Width = 40.0;
                    MasterWorkSheet.Column(5).Width = 20;
                    MasterWorkSheet.Column(6).Width = 40.0;
                    MasterWorkSheet.Column(8).AutoFit();
                    for (var i = 2; i <= 8; i++)
                    {
                        MasterWorkSheet.Column(i).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    }

                    var range = MasterWorkSheet.Cells[17, 2, 17, 8];
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Brown);
                    range.Style.Font.Color.SetColor(Color.WhiteSmoke);
                    range.Style.Locked = true;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.ShrinkToFit = false;

                    // Pie chart
                    var chart = (ExcelPieChart)MasterWorkSheet.Drawings.AddChart("Test Summary", eChartType.Pie3D);
                    chart.Fill.Color = Color.LightSteelBlue;
                    chart.Border.Fill.Color = Color.LightSkyBlue;
                    chart.PlotArea.Fill.Color = Color.LightSteelBlue;
                    chart.Border.Width = 3;
                    chart.Title.Text = "Test Summary";
                    chart.SetPosition(0, 0, 3, 4);
                    chart.SetSize(570, 200);
                    chart.Series.Add(MasterWorkSheet.Cells[8, 3, 9, 3], MasterWorkSheet.Cells[8, 2, 9, 2]);
                    chart.Legend.Border.LineStyle = eLineStyle.Solid;
                    chart.Legend.Border.Fill.Style = eFillStyle.SolidFill;
                    chart.Legend.Border.Fill.Color = Color.DarkBlue;
                    chart.DataLabel.ShowPercent = true;
                    chart.DataLabel.ShowLeaderLines = true;

                    var ws = chart.WorkSheet;
                    var nsm = ws.Drawings.NameSpaceManager;
                    var nschart = nsm.LookupNamespace("c");
                    var nsa = nsm.LookupNamespace("a");
                    var node = chart.ChartXml.SelectSingleNode("c:chartSpace/c:chart/c:plotArea/c:pie3DChart/c:ser", nsm);
                    var doc = chart.ChartXml;

                    SetPieChartSegmentColorMaster(doc, nschart, 0, node, nsa, Color.ForestGreen);
                    SetPieChartSegmentColorMaster(doc, nschart, 1, node, nsa, Color.Red);

                    xlPackage.Save();

                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at CreateMasterSheet class");
                bool rethrow = ExceptionPolicy.HandleException(ex, "LogAndThrowPolicy");
                if (rethrow)
                    throw new Exception(ex.Message + " [issue while creating mastersheet in CreateMasterSheet class]");
            }
        }

        /// <summary>
        /// Sets the pie chart segment color master.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="nschart">The nschart.</param>
        /// <param name="i">The i.</param>
        /// <param name="node">The node.</param>
        /// <param name="nsa">The nsa.</param>
        /// <param name="color">The color.</param>
        private static void SetPieChartSegmentColorMaster(XmlDocument doc, string nschart, int i, XmlNode node, string nsa, Color color)
        {
            try
            {
                //Add the node
                var dPt = doc.CreateElement("dPt", nschart);
                var idx = dPt.AppendChild(doc.CreateElement("idx", nschart));
                var valattrib = idx.Attributes.Append(doc.CreateAttribute("val"));
                valattrib.Value = i.ToString(System.Globalization.CultureInfo.InvariantCulture);
                node.AppendChild(dPt);

                //Add the solid fill node
                var spPr = doc.CreateElement("spPr", nschart);
                var solidFill = spPr.AppendChild(doc.CreateElement("solidFill", nsa));
                var srgbClr = solidFill.AppendChild(doc.CreateElement("srgbClr", nsa));
                valattrib = srgbClr.Attributes.Append(doc.CreateAttribute("val"));

                //Set the color
                var colorPie = Color.FromArgb(color.ToArgb());
                valattrib.Value = System.Drawing.ColorTranslator.ToHtml(colorPie).Replace("#", String.Empty);
                dPt.AppendChild(spPr);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "LogAndThrowPolicy");
                if (rethrow)
                    throw new Exception(ex.Message + " [issue in SetPieChartSegmentColorMaster class]");
            }
        }

        /// <summary>
        /// Kill all the process which are unexcepted opened while running the test cases and other test case fail
        /// </summary>
        /// 
        public static void KillUnexceptedProcess()
        {
            try
            {
                List<string> procNameList = new List<string>();
                procNameList.Add("Wmplayer");
                procNameList.Add("Calc");
                procNameList.Add("Chrome");
                procNameList.Add("Calc");
                procNameList.Add("Iexplore");
                procNameList.Add("Rundll32");
                procNameList.Add("Explorer");
                procNameList.Add("et");
                procNameList.Add("wps");
                procNameList.Add("wpp");
                procNameList.Add("dllhost");
                procNameList.Add("Notepad");
                procNameList.Add("Notepad++");

                foreach (string procName in procNameList)
                {
                    Process[] _process = Process.GetProcessesByName(procName);
                    foreach (Process proc in _process)
                        proc.Kill();
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at KillUnexceptedProcess class");
                throw new Exception(ex.Message + " [issue while killing processes in KillUnexceptedProcess class]");
            }
        }
        static bool Flag = true;
        static int count = 0;
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

        public static string RunTestCases()
        {
            int totalRowCount;
            int rowCount, colCount;
            int maxRetryCount = int.Parse(ConfigurationManager.AppSettings["MaxRetryCount"].ToString());
            String sheet = String.Empty;
            String path = string.Empty;
            String module = string.Empty;
            bool retry = true;
            List<string> previousIPList = new List<string>();
            List<string> currentIPList = new List<string>();
            Dictionary<string, int> colDictionary = new Dictionary<string, int>();
            previousIPList.Clear();
            currentIPList.Clear();
            bool isCacheTrimmed = false;
            while (retry)
            {
                try
                {
                    string TCFile = ApplicationArguments.MasterSheetPath;
                    ///
                    if (!String.IsNullOrEmpty(ApplicationArguments.MasterSheetBackupPath)) // if MasterSheetBackupPath is not empty then
                    {
                        string BACKPATH = ApplicationArguments.MasterSheetBackupPath;
                        if (File.Exists(BACKPATH))
                        {
                            BACKUPFILE_BLANK = isFileEmpty(BACKPATH);
                        }
                    }

                    //copy if MasterSheetpath file is empty and Mastersheetbackup path exist and file is not empty

                    if (!File.Exists(TCFile) && !String.IsNullOrEmpty(ApplicationArguments.MasterSheetBackupPath) && BACKUPFILE_BLANK != true)
                    {
                        File.Copy(ApplicationArguments.MasterSheetBackupPath, ApplicationArguments.MasterSheetPath);
                    }
                    //Mastersheet path exist but file is empty and Mastersheetbackup path exist and its file is not empty


                    if (File.Exists(TCFile) && !String.IsNullOrEmpty(ApplicationArguments.MasterSheetBackupPath) && BACKUPFILE_BLANK != true)
                    {
                        TCFILE_BLANK = isFileEmpty(TCFile);
                        if (TCFILE_BLANK == true)
                        {
                            // File.Delete(ApplicationArguments.MasterSheetPath);
                            File.Copy(ApplicationArguments.MasterSheetBackupPath, ApplicationArguments.MasterSheetPath, true);
                        }
                    }

                    if (File.Exists(TCFile) && !String.IsNullOrEmpty(ApplicationArguments.MasterSheetBackupPath))
                    {
                        TCFILE_BLANK = isFileEmpty(TCFile);
                        if (TCFILE_BLANK == true)
                        {
                            // File.Delete(ApplicationArguments.MasterSheetPath);
                            File.Copy(ApplicationArguments.MasterSheetBackupPath, ApplicationArguments.MasterSheetPath, true);
                        }
                    }



                    ////


                    using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(TCFile)))
                    {

                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                        totalRowCount = DataUtilities.GetLastUsedRow(worksheet);
                        colCount = DataUtilities.GetLastUsedColumn(worksheet);
                        colDictionary = DataUtilities.ColToIndexMapping(worksheet, colCount);

                        // if any empty row exist then delete it
                        DeleteEmptyRow(totalRowCount, ApplicationArguments.MasterSheetPath);

                        String testCase = string.Empty;
                        for (rowCount = 2; rowCount <= totalRowCount; rowCount++)
                        {
                            //To Skip the Test cases of GL & CA module if there is Resolution issue
                            bool IsResolutionIssueInCAandGL = CoreExecutor.IsCAandGLCasesResolutionIssue;
                            if (IsResolutionIssueInCAandGL && (worksheet.Cells[rowCount, colDictionary["Module"]].Value.ToString().Equals("CorporateActions") || worksheet.Cells[rowCount, colDictionary["Module"]].Value.ToString().Equals("CA") || worksheet.Cells[rowCount, colDictionary["Module"]].Value.ToString().Equals("GeneralLedger")))
                                continue;
                            try
                            {
                                rowcountforcase = rowCount;
                                //Pick Test case with status = 'Not Run' from the sheet.
                                if (worksheet.Cells[rowCount, colDictionary["Status"]].Value.Equals(Status.Not_Run.ToString()))
                                {
                                    if (worksheet.Cells[rowCount, colDictionary["Previous IP Address"]].Value != null && worksheet.Cells[rowCount, colDictionary["Previous IP Address"]].Value.ToString().Split(',').ToList().Contains(TestStatusLog.GetSytemIP().ToString()))
                                        continue;
                                    //Check dependency, if simulator is there then it will start simulator, similarly for dropcopy it will start dropcopy
                                    if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.Simulator.ToString()))
                                    {
                                        ApplicationArguments.SkipSimulatorStartUp = false;
                                    }
                                    if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.Compliance.ToString()))
                                    {
                                        ApplicationArguments.SkipSimulatorStartUp = false;
                                        ApplicationArguments.SkipCompliance = false;
                                    }
                                    if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.CheckSide.ToString()))
                                    {
                                        SqlUtilities.ExecuteQuery("Update T_AL_AllocationDefaultRule SET CheckSidePreference = '{" + "DisableCheckSidePref" + ":{}," + "DoCheckSideSystem" + ":true}' where CompanyId > 0");
                                        Console.WriteLine("Triggered true query");
                                    }
                                    else
                                    {
                                        SqlUtilities.ExecuteQuery("Update T_AL_AllocationDefaultRule SET CheckSidePreference = '{" + "DisableCheckSidePref" + ":{}," + "DoCheckSideSystem" + ":false}' where CompanyId > 0");
                                        Console.WriteLine("Triggered false query");
                                    }
                                    if (colDictionary.ContainsKey("PreRequisiteType"))
                                    {
                                        object value = worksheet.Cells[rowCount, colDictionary["PreRequisiteType"]].Value;

                                        if (value != null)
                                            ApplicationArguments.isPreRequisiteType = worksheet.Cells[rowCount, colDictionary["PreRequisiteType"]].Value.ToString();
                                        else
                                            ApplicationArguments.isPreRequisiteType = "";

                                    }
                                    if (colDictionary.ContainsKey("Member"))
                                    {
                                        object value = worksheet.Cells[rowCount, colDictionary["Member"]].Value;

                                        if (value != null)
                                            ApplicationArguments.MemberPath = worksheet.Cells[rowCount, colDictionary["Member"]].Value.ToString();

                                    }

                                    
                                    // Check if current cases having dependency of basket compliance
                                    if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.BasketCompliance.ToString()))
                                    {
                                        ApplicationArguments.SkipSimulatorStartUp = false;
                                        ApplicationArguments.SkipCompliance = false;
                                        ApplicationArguments.SkipBasketCompliance = false;
                                        SqlUtilities.ExecuteQuery("Update T_CA_CompliancePreferences Set IsBasketComplianceEnabledCompany = 1 Where CompanyId > 0");

                                        SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission Set IsPreTradeEnabled = 1, Trading = 1, IsApplyToManual = 1, Staging = 1, EnableBasketComplianceCheck = 1 where CompanyId > 0");
                                        Console.WriteLine("Basket Compliance Enabled");

                                        /*If there is dependency of basket compliance case -- then Update ExpnlServiceHost.exe.config to default key's value using  ConfigUpdater */
                                        ConfigModificatorSettings _file = new ConfigModificatorSettings(ApplicationArguments.ExpnlReleasePath + "\\Prana.ExpnlServiceHost.exe.config");

                                        ConfigUpdater.ChangeValueByKey(TestDataConstants.CALCULATEFXGAINLOSSONFUTURES, TestDataConstants.KEYVALUETRUE, _file);
                                        ConfigUpdater.ChangeValueByKey(TestDataConstants.ISM2MINCLUDEDINCASH, TestDataConstants.KEYVALUETRUE, _file);


                                        // query to enable basket permission check to user specific
                                        SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '1' where UserId = '17' OR UserId= '18'");
                                        if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                        {
                                            SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '1' where UserId = '35' ");
                                        }
                                        Console.WriteLine("Basket Compliance PRE-TRADE CHECK PERMISSION MOVED TO BASKET INSTEAD OF REGULAR");

                                    }
                                    else
                                    {
                                        SqlUtilities.ExecuteQuery("Update T_CA_CompliancePreferences Set IsBasketComplianceEnabledCompany = 0 Where CompanyId > 0");
                                        SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '0' where UserId = '17' OR UserId= '18' ");
                                        if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                        {
                                            SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '0' where UserId = '35' ");
                                        }
                                        Console.WriteLine("Basket Compliance Disbaled");
                                    }
                                    //Check if dropcopy is there in dependency then it will start dropcopy first, then simulator
                                    if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.DropCopy.ToString()))
                                    {
                                        ApplicationArguments.SkipDropCopyStartUp = false;
                                        ApplicationArguments.SkipSimulatorStartUp = false;
                                    }

                                    sheet = worksheet.Cells[rowCount, colDictionary["Regression Sheet"]].Value.ToString();

                                    module = worksheet.Cells[rowCount, colDictionary["Module"]].Value.ToString();
                                    testCase = worksheet.Cells[rowCount, colDictionary["Testcase ID"]].Value.ToString();
                                    ApplicationArguments.SheetName = module;
                                    ApplicationArguments.TestCaseToBeRun = testCase;
                                    path = "{'" + sheet + ".xlsx':{'" + module + "':['" + testCase + "']}}";
                                    worksheet.Cells[rowCount, colDictionary["Current IP Address"]].Value = TestStatusLog.GetSytemIP().ToString();
                                    worksheet.Cells[rowCount, colDictionary["Update Time"]].Value = "UpdateTime";

                                    ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);

                                    // get list of sheets in workbook, the provider reads from 5th row and 2nd column to read data as per current test cases file format
                                    DataSet workbook = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + sheet + ".xlsx", 5, 2);

                                    DataTable table = workbook.Tables[module];


                                    // To verify regression sheet is in correct format

                                    if (sheet != "Regression Test Cases")
                                    {
                                        Console.WriteLine("Sheet name is not in correct format");
                                        worksheet.Cells[rowCount, colDictionary["Status"]].Value = Status.Invalid_Data.ToString();
                                        xlPackage.Save();
                                        continue;
                                    }
                                    else
                                        worksheet.Cells[rowCount, colDictionary["Status"]].Value = Status.In_Progress.ToString();

                                    // To verify test case ID is exist in regression sheet
                                    bool iscaseExist = IsValueExists(table, testCase);
                                    if (!iscaseExist)
                                    {
                                        Console.WriteLine("Test case " + testCase + " does not in the regression sheet");
                                        worksheet.Cells[rowCount, colDictionary["Status"]].Value = Status.Invalid_Data.ToString();
                                        xlPackage.Save();
                                        continue;
                                    }

                                    if (!File.Exists(ApplicationArguments.TestDataFolderPath + "\\" + testCase + "\\" + testCase + ".xlsx"))
                                    {
                                        Console.WriteLine("Test case " + testCase + " does not exist in the current path " + ApplicationArguments.TestDataFolderPath);
                                        worksheet.Cells[rowCount, colDictionary["Status"]].Value = Status.Invalid_Data.ToString();
                                        xlPackage.Save();
                                        continue;
                                    }

                                    xlPackage.Save();
                                    retry = false;
                                    Flag = false;
                                    return path;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                throw;
                            }

                        }


                        if (!Flag && count == 0)
                        {
                            bool IsInProgress = false;
                            //for(rowCount = 2; rowCount <= totalRowCount; rowCount++)
                            //{
                            //    bool checkInprogress = false;
                            //    if(worksheet.Cells[rowCount, colDictionary["Status"]].Value.ToString().Equals(Status.In_Progress.ToString())){
                            //        IsInProgress = true;
                            //        checkInprogress = true;
                            //        Console.WriteLine("waiting for status from inprogress to fail or pass");
                            //        while (checkInprogress)
                            //        {
                            //            if (!worksheet.Cells[rowCount, colDictionary["Status"]].Value.ToString().Equals(Status.In_Progress.ToString()))
                            //            {
                            //                IsInProgress = false;
                            //                checkInprogress = false;
                            //            }
                            //        }
                            //        Console.WriteLine("wait ends");
                            //    }
                            //}

                            if (!IsInProgress)
                            {
                                Console.WriteLine("there is no InProcess status test case");
                                if (isCacheTrimmed == false)
                                {
                                    MemoryCache cache = CacheManager.Instance.GetCache();
                                    Console.WriteLine("Removing " + cache.GetCount() + " No. of entries from cache");
                                    cache.Trim(100);
                                    isCacheTrimmed = true;
                                }
                                ProcessKiller();
                                ProcessStartInfo info = new ProcessStartInfo(@"E:\DistributedAutomation\RestartMachine.bat");
                                info.CreateNoWindow = true;
                                info.UseShellExecute = false;
                                Process.Start(info);
                                SaveAndExit(0);
                                System.Environment.Exit(1);

                            }
                        }
                        count++;


                        if (String.IsNullOrEmpty(testCase))
                        {
                            for (rowCount = 2; rowCount <= totalRowCount; rowCount++)
                            {
                                rowcountforcase = rowCount;
                                //  Pick up the case with status "Failed", retry counter is less than max retry val , slaveIP is not equal to current IP
                                if (worksheet.Cells[rowCount, colDictionary["Status"]].Value.ToString().Equals(Status.Fail.ToString()))
                                {
                                    string retryVal = worksheet.Cells[rowCount, colDictionary["Retry Counter"]].Value.ToString();
                                    string currentIP = worksheet.Cells[rowCount, colDictionary["Current IP Address"]].Value.ToString();
                                    string previousIP = worksheet.Cells[rowCount, colDictionary["Previous IP Address"]].Value.ToString();
                                    previousIPList = previousIP.Split(',').ToList();
                                    currentIPList.Add(currentIP);


                                    if (previousIPList.Contains(TestStatusLog.GetSytemIP().ToString()) || currentIPList.Contains(TestStatusLog.GetSytemIP().ToString()))
                                    {
                                        previousIPList.Clear();
                                        currentIPList.Clear();
                                        continue;
                                    }
                                    int val = Int32.Parse(retryVal);
                                    if (val < maxRetryCount)
                                    {
                                        if (!previousIPList.Contains(currentIP))
                                        {
                                            previousIPList.Add(currentIP);
                                        }
                                        worksheet.Cells[rowCount, colDictionary["Previous IP Address"]].Value = string.Join(",", previousIPList);
                                        //worksheet.Cells[rowCount, 10].Value = string.Concat(worksheet.Cells[rowCount,10].Value, ",", currentIP);
                                        //Check dependency, if simulator is there then it will start simulator, similarly for dropcopy it will start dropcopy
                                        if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.Simulator.ToString()))
                                        {
                                            ApplicationArguments.SkipSimulatorStartUp = false;
                                        }
                                        if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.Compliance.ToString()))
                                        {
                                            ApplicationArguments.SkipSimulatorStartUp = false;
                                            ApplicationArguments.SkipCompliance = false;
                                        }
                                        //Check if dropcopy is there in dependency then it will start dropcopy first, then simulator
                                        if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.DropCopy.ToString()))
                                        {
                                            ApplicationArguments.SkipDropCopyStartUp = false;
                                            ApplicationArguments.SkipSimulatorStartUp = false;
                                        }
                                        if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.CheckSide.ToString()))
                                        {
                                            SqlUtilities.ExecuteQuery("Update T_AL_AllocationDefaultRule SET CheckSidePreference = '{" + "DisableCheckSidePref" + ":{}," + "DoCheckSideSystem" + ":true}' where CompanyId > 0");
                                            Console.WriteLine("Triggered true query");
                                        }
                                        else
                                        {
                                            SqlUtilities.ExecuteQuery("Update T_AL_AllocationDefaultRule SET CheckSidePreference = '{" + "DisableCheckSidePref" + ":{}," + "DoCheckSideSystem" + ":false}' where CompanyId > 0");
                                            Console.WriteLine("Triggered false query");
                                        }
                                        if (colDictionary.ContainsKey("PreRequisiteType"))
                                        {
                                            object value = worksheet.Cells[rowCount, colDictionary["PreRequisiteType"]].Value;

                                            if (value != null)
                                                ApplicationArguments.isPreRequisiteType = worksheet.Cells[rowCount, colDictionary["PreRequisiteType"]].Value.ToString();
                                            else
                                                ApplicationArguments.isPreRequisiteType = "";

                                        }
                                        if (colDictionary.ContainsKey("Member"))
                                        {
                                            object value = worksheet.Cells[rowCount, colDictionary["Member"]].Value;

                                            if (value != null)
                                                ApplicationArguments.MemberPath = worksheet.Cells[rowCount, colDictionary["Member"]].Value.ToString();

                                        }

                                        
                                        // Check if current cases having dependency of basket compliance
                                        if (worksheet.Cells[rowCount, colDictionary["Dependency"]].Value.Equals(Dependency.BasketCompliance.ToString()))
                                        {
                                            ApplicationArguments.SkipSimulatorStartUp = false;
                                            ApplicationArguments.SkipCompliance = false;
                                            ApplicationArguments.SkipBasketCompliance = false;
                                            SqlUtilities.ExecuteQuery("Update T_CA_CompliancePreferences Set IsBasketComplianceEnabledCompany = 1 Where CompanyId > 0");

                                            SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission Set IsPreTradeEnabled = 1, Trading = 1, IsApplyToManual = 1, Staging = 1, EnableBasketComplianceCheck = 1 where CompanyId > 0");
                                            Console.WriteLine("Basket Compliance Enabled");

                                            /*If there is dependency of basket compliance case -- then Update ExpnlServiceHost.exe.config to default key's value using  ConfigUpdater */
                                            ConfigModificatorSettings _file = new ConfigModificatorSettings(ApplicationArguments.ExpnlReleasePath + "\\Prana.ExpnlServiceHost.exe.config");

                                            ConfigUpdater.ChangeValueByKey(TestDataConstants.CALCULATEFXGAINLOSSONFUTURES, TestDataConstants.KEYVALUETRUE, _file);
                                            ConfigUpdater.ChangeValueByKey(TestDataConstants.ISM2MINCLUDEDINCASH, TestDataConstants.KEYVALUETRUE, _file);


                                            // query to enable basket permission check to user specific
                                            SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '1' where UserId = '17' OR UserId= '18'");
                                            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                            {
                                                SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '1' where UserId = '35' ");
                                            }
                                            Console.WriteLine("Basket Compliance PRE-TRADE CHECK PERMISSION MOVED TO BASKET INSTEAD OF REGULAR");

                                        }
                                        else
                                        {
                                            SqlUtilities.ExecuteQuery("Update T_CA_CompliancePreferences Set IsBasketComplianceEnabledCompany = 0 Where CompanyId > 0");
                                            SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '0' where UserId = '17' OR UserId= '18' ");
                                            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                                            {
                                                SqlUtilities.ExecuteQuery("Update T_CA_OtherCompliancePermission set EnableBasketComplianceCheck = '0' where UserId = '35' ");
                                            }
                                            Console.WriteLine("Basket Compliance Disbaled");
                                        }
                                        sheet = worksheet.Cells[rowCount, colDictionary["Regression Sheet"]].Value.ToString();
                                        module = worksheet.Cells[rowCount, colDictionary["Module"]].Value.ToString();
                                        testCase = worksheet.Cells[rowCount, colDictionary["Testcase ID"]].Value.ToString();
                                        path = "{'" + sheet + ".xlsx':{'" + module + "':['" + testCase + "']}}";
                                        worksheet.Cells[rowCount, colDictionary["Current IP Address"]].Value = TestStatusLog.GetSytemIP().ToString();
                                        worksheet.Cells[rowCount, colDictionary["Status"]].Value = Status.In_Progress.ToString();
                                        //  worksheet.Cells[rowCount, colDictionary["Last Run update"]].Value = DateTime.Now.ToString("HH:mm:ss");
                                        val++;
                                        worksheet.Cells[rowCount, colDictionary["Retry Counter"]].Value = val.ToString();
                                        worksheet.Cells[rowCount, colDictionary["Update Time"]].Value = "UpdateTime";
                                        xlPackage.Save();
                                        retry = false;
                                        return path;
                                    }
                                }

                            }

                        }
                        if (String.IsNullOrEmpty(testCase))
                        {
                            for (rowCount = 2; rowCount <= totalRowCount; rowCount++)
                            {
                                if (worksheet.Cells[rowCount, colDictionary["Status"]].Value.Equals(Status.Pass.ToString()))
                                    continue;

                                else if ((worksheet.Cells[rowCount, colDictionary["Status"]].Value.Equals(Status.Fail.ToString()) && worksheet.Cells[rowCount, colDictionary["Retry Counter"]].Value.Equals(maxRetryCount)))
                                    continue;

                            }
                            retry = false;
                            CompressScreenshotFolder();
                            DeleteScreenshotFolder();
                            ApplicationArguments.ExitCode = 0;
                            Environment.Exit(0);
                        }
                    }
                }



                catch (IOException)
                {
                    //retry if excel sheet is already open 
                    retry = true;

                    Thread.Sleep(2000);
                }
                catch (SystemException)
                {
                    retry = true;
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    retry = true;
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
            return path;
        }

        private static void DeleteEmptyRow(int totalRowCount, string path)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
            {

                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                for (int row = totalRowCount; row >= 1; row--)
                {
                    bool isBlankRow = true;

                    // Loop through the columns of the current row
                    for (int column = worksheet.Dimension.Start.Column; column <= worksheet.Dimension.End.Column; column++)
                    {
                        // Check if any cell in the current row is not blank
                        if (worksheet.Cells[row, column].Value != null)
                        {
                            isBlankRow = false;
                            break;
                        }
                    }

                    // If the current row is blank, delete it
                    if (isBlankRow)
                    {
                        worksheet.DeleteRow(row);
                        xlPackage.Save();
                    }
                }

            }

        }

        private static bool IsValueExists(DataTable table, string testCase)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (row[column].ToString() == testCase)
                    {
                        Console.WriteLine("Test case Found");
                        return true;
                    }

                }
            }
            return false;
        }

        public static void UpdateStatus(string error)
        {
            if (ConfigurationManager.AppSettings["viaCmdArguments"].Equals("True"))
            {
                bool retry = true;
                Status resultStatus;
                while (retry)
                {
                    try
                    {
                        using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ApplicationArguments.MasterSheetPath)))
                        {
                            if (!File.Exists(ApplicationArguments.MasterSheetPath) && !String.IsNullOrEmpty(ApplicationArguments.MasterSheetBackupPath))
                            {
                                File.Copy(ApplicationArguments.MasterSheetBackupPath, ApplicationArguments.MasterSheetPath);
                            }
                            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                            int colCount = DataUtilities.GetLastUsedColumn(worksheet);
                            Dictionary<string, int> colDictionary = DataUtilities.ColToIndexMapping(worksheet, colCount);
                            if (String.IsNullOrEmpty(error))
                                resultStatus = Status.Pass;
                            else if (error.Equals(MessageConstants.MACHINE_ERROR))
                            {
                                resultStatus = Status.Not_Run;
                                if (worksheet.Cells[rowcountforcase, colDictionary["Previous IP Address"]].Value == null)
                                    worksheet.Cells[rowcountforcase, colDictionary["Previous IP Address"]].Value = TestStatusLog.GetSytemIP().ToString();
                            }

                            else if (error.StartsWith(MessageConstants.LOGIN_ERROR) || error.StartsWith(MessageConstants.STARTUP_ERROR))
                            {
                                if (!String.IsNullOrEmpty(error) && ApplicationArguments.RetryCount < ApplicationArguments.RetrySize)
                                    resultStatus = Status.In_Progress;
                                else
                                    resultStatus = Status.Not_Run;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(error) && ApplicationArguments.RetryCount < ApplicationArguments.RetrySize)
                                    resultStatus = Status.In_Progress;
                                else
                                    resultStatus = Status.Fail;
                            }
                            string status = resultStatus.ToString();

                            worksheet.Cells[rowcountforcase, colDictionary["Status"]].Value = resultStatus;
                            if (status.Equals(Status.Fail.ToString()))
                            {
                                if (worksheet.Cells[rowcountforcase, colDictionary["Previous IP Address"]].Value == null)
                                    worksheet.Cells[rowcountforcase, colDictionary["Previous IP Address"]].Value = TestStatusLog.GetSytemIP().ToString();
                            }
                            xlPackage.Save();
                            retry = false;

                            /////if MasterSheetBackupPath  exist and MasterSheet is not empty
                            if (!String.IsNullOrEmpty(ApplicationArguments.MasterSheetBackupPath) && TCFILE_BLANK != true)
                            {
                                File.Copy(ApplicationArguments.MasterSheetPath, ApplicationArguments.MasterSheetBackupPath, true);
                            }

                        }
                        try
                        {
                            if (App.GlobalArgs.usingTempFile)
                            {
                                string[] args = App.GlobalArgs.Args;
                                var index = Array.FindIndex(args, row => row.Contains(CommandLineConstants.CONST_MASTER_SHEET_PATH));

                                using (FileStream stream = new FileInfo(args[index + 1].Replace("\"", string.Empty)).Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                                {
                                    ApplicationArguments.MasterSheetPath = args[index + 1].Replace("\"", string.Empty);
                                    string tempFilePath = Path.Combine(Path.GetDirectoryName(ApplicationArguments.MasterSheetPath), "temp_" + Path.GetFileName(ApplicationArguments.MasterSheetPath));
                                    App.GlobalArgs.usingTempFile = false;
                                }
                            }
                        }
                        catch (IOException)
                        {
                        }
                    }
                    catch (IOException)
                    {
                        //retry if excel sheet is already open 
                        retry = true;
                        Thread.Sleep(2000);
                    }
                    catch (SystemException)
                    {
                        retry = true;
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        retry = true;
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        public static void TestCasesForRun()
        {
            string path = RunTestCases();
            //if (args.Length > 0)
            {
                //update testcase dictionary in Application Arguments
                // var index = Array.FindIndex(args, row => row.Contains(CommandLineConstants.CONST_TEST_CASES));
                //if(index > 0 )
                {
                    ApplicationArguments.TestCasesDictionary = GetDictionary(path.ToString());
                    ApplicationArguments.TestCaseWeightDict = GetModuleTestCaseWeights();
                    string jsonSerialisedString = JsonConvert.SerializeObject(ApplicationArguments.TestCasesDictionary);
                    jsonSerialisedString = jsonSerialisedString.Replace("\"", "'");
                    //StringBuilder modifiedTestCases = new StringBuilder();
                    //modifiedTestCases.Append(jsonSerialisedString);
                    //args[index + 1] = modifiedTestCases.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the selected workbooks.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>.
        private static Dictionary<string, Dictionary<string, List<string>>> GetDictionary(string args)
        {
            try
            {
                Dictionary<string, Dictionary<string, List<string>>> dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(args);
                //if no worksheet present in dictionary then add all the worksheet in dictonary 
                foreach (string workbook in dictionary.Keys)
                {
                    if (dictionary[workbook].Count == 0)
                    {
                        List<string> sheets;
                        sheets = GetAllWorkSheets(workbook);
                        foreach (string sheet in sheets)
                        {
                            dictionary[workbook].Add(sheet, new List<string>());
                        }
                    }
                    //check every worksheet present in dictionary 
                    //if present worksheet contains no test cases in dictionary then add all the test case of that worksheet in dictionary
                    foreach (string testCasesList in dictionary[workbook].Keys)
                    {
                        if (dictionary[workbook][testCasesList].Count == 0)
                        {
                            List<string> testCases = GetAllTestCases(workbook, testCasesList);
                            foreach (string testCase in testCases)
                            {
                                if (!dictionary[workbook][testCasesList].Contains(testCase))
                                    dictionary[workbook][testCasesList].Add(testCase);
                            }
                        }
                    }
                }
                return dictionary;
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at GetDictionary class");
                throw new Exception(ex.Message + " [issue in class GetDictionary while creating dict from path: " + args + "]");
            }
        }
        /// <summary>
        /// Gets the module test case weights.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, int> GetModuleTestCaseWeights()
        {
            Dictionary<string, int> testCasesWeights = new Dictionary<string, int>();
            try
            {
                foreach (string workBookName in ApplicationArguments.TestCasesDictionary.Keys)
                {
                    ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                    DataSet testCases = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + workBookName, 5, 2);
                    foreach (string moduleName in ApplicationArguments.TestCasesDictionary[workBookName].Keys.ToList())
                    {
                        if (testCases.Tables.Contains(moduleName))
                        {
                            foreach (DataRow row in testCases.Tables[moduleName].Rows)
                            {
                                int testcaseWeight = 1;

                                try { testcaseWeight = int.Parse(row[ExcelStructureConstants.COL_TESTCASEWeight].ToString()); }
                                catch (Exception) { testcaseWeight = 1; }

                                String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();

                                if (String.IsNullOrWhiteSpace(testcaseid))
                                    continue;
                                if (!testCasesWeights.ContainsKey(testcaseid))
                                    testCasesWeights.Add(testcaseid, testcaseWeight);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at GetModuleTestCaseWeights class");
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw new Exception(ex.Message + " [issue in class GetModuleTestCaseWeights]");
            }
            return testCasesWeights;
        }

        /// <summary>
        /// Get all the worksheet
        /// </summary>
        /// <param name="workBook"></param>
        /// <returns></returns>
        private static List<string> GetAllWorkSheets(string workBook)
        {
            try
            {
                List<string> workSheets = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + workBook, 5, 2);
                for (int tablesCount = 0; tablesCount < testCases.Tables.Count; tablesCount++)
                {
                    if (!workSheets.Contains(testCases.Tables[tablesCount].TableName))
                        workSheets.Add(testCases.Tables[tablesCount].TableName);
                }
                return workSheets;
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Error at GetAllWorkSheets class");
                throw new Exception(ex.Message + " [issue in class GetAllWorkSheets, workbook path: " + workBook + "]");
            }
        }

        /// <summary>
        /// get All the test cases
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="workSheet"></param>
        /// <returns></returns>
        private static List<string> GetAllTestCases(string workBook, string workSheet)
        {
            try
            {
                List<string> tCases = new List<string>();
                ITestDataProvider provider = TestDataProvider.GetProvider(ProviderType.OpenXml);
                DataSet testCases = provider.GetTestData(ApplicationArguments.TestDataFolderPath + "\\" + workBook, 5, 2);

                if (testCases.Tables.Contains(workSheet))
                {
                    foreach (DataRow row in testCases.Tables[workSheet].Rows)
                    {
                        String testcaseid = row[ExcelStructureConstants.COL_TESTCASEID].ToString();
                        if (String.IsNullOrWhiteSpace(testcaseid))
                            continue;
                        if (!tCases.Contains(testcaseid))
                        {
                            tCases.Add(testcaseid);
                        }
                    }
                }
                return tCases;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " [issue in class GetAllWorkSheets, workbook path: " + workBook + " , worksheet path:" + workSheet +"]");
            }
        }
        public static void blacklistIPByCommonIssue(int exitCode)
        {
            int colCount, rowCount;
            Dictionary<string, int> colDictionary = new Dictionary<string, int>();
            bool retry = true;

            if (!File.Exists(ApplicationArguments.ExcludeSlavesFilePath))
            {
                TestExecutor.CreateExcludeSlavesFile(ApplicationArguments.ExcludeSlavesFilePath);
            }

            while (retry)
            {
                try
                {

                    using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ApplicationArguments.ExcludeSlavesFilePath)))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                        rowCount = DataUtilities.GetLastUsedRow(worksheet);
                        colCount = DataUtilities.GetLastUsedColumn(worksheet);
                        colDictionary = DataUtilities.ColToIndexMapping(worksheet, colCount);

                        //Add IP in exclude slaves file
                        rowCount++;
                        worksheet.Cells[rowCount, colDictionary["IP"]].Value = TestStatusLog.GetSytemIP().ToString();
                        worksheet.Cells[rowCount, colDictionary["Reason"]].Value = ApplicationArguments.ExitCodeDictionary[exitCode];
                        Console.WriteLine("Ip is blacklisting by Machine specific issue");
                        xlPackage.Save();
                    }
                    retry = false;
                }

                catch (IOException)
                {
                    //retry if excel sheet is already open 
                    retry = true;

                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }

        }
        public static void blacklistIP(int exitCode)
        {

            int colCount, rowCount;
            Dictionary<string, int> colDictionary = new Dictionary<string, int>();
            bool retry = true;

            if (!File.Exists(ApplicationArguments.ExcludeSlavesFilePath))
            {
                TestExecutor.CreateExcludeSlavesFile(ApplicationArguments.ExcludeSlavesFilePath);
            }

            while (retry)
            {
                try
                {

                    using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ApplicationArguments.ExcludeSlavesFilePath)))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                        rowCount = DataUtilities.GetLastUsedRow(worksheet);
                        colCount = DataUtilities.GetLastUsedColumn(worksheet);
                        colDictionary = DataUtilities.ColToIndexMapping(worksheet, colCount);

                        //Add IP in exclude slaves file
                        rowCount++;
                        worksheet.Cells[rowCount, colDictionary["IP"]].Value = TestStatusLog.GetSytemIP().ToString();
                        worksheet.Cells[rowCount, colDictionary["Reason"]].Value = ApplicationArguments.ExitCodeDictionary[exitCode];
                        xlPackage.Save();
                    }
                    retry = false;
                }

                catch (IOException)
                {
                    //retry if excel sheet is already open 
                    retry = true;

                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }


        }

        public static void CreateExcludeSlavesFile(string path)
        {

            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "IP";
                worksheet.Cells[1, 2].Value = "Reason";

                xlPackage.Save();
            }


        }

        /// <summary>
        /// Copy test logs to master machine
        /// </summary>
        public static void CopyLogsToMaster()
        {
            try
            {

                if (ConfigurationManager.AppSettings["-AllowCopyLogFileToMaster"].Equals("false"))
                {
                    Console.WriteLine("Logs will not be copied to master machine..");
                    return;
                }
                /*else if (ApplicationArguments.RetryCount < ApplicationArguments.RetrySize)
                {
                    Console.WriteLine("Started else if part..");
                    return;
                }*/
                else
                {
                    Console.WriteLine("Started copying logs..");
                    string IP = TestStatusLog.GetSytemIP().ToString();
                    string nodeName = IP.Substring(8).Replace('.', '_');
                    Process copyLogs = new Process();
                    copyLogs.StartInfo.FileName = ConfigurationManager.AppSettings["UtilityManager"];
                    string[] arr = ConfigurationManager.AppSettings["-runDescription"].Split(' ');
                    copyLogs.StartInfo.Arguments = nodeName + " " + arr[0] + " " + "CopyLogs";
                    Console.WriteLine("Logs copying in {0} Release", arr[0]);
                    copyLogs.Start();
                    copyLogs.WaitForExit();
                    copyLogs.Close();
                    Console.WriteLine("Copied logs!");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }


        }

        /// <summary>
        /// Check if machine belongs to black list of IPs
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="defect"></param>
        /// <param name="terminateExecution"></param>
        /// <returns></returns>
        public static bool IsMachineFaulty(string IP, List<string> defect, bool terminateExecution)
        {
            bool retry = true;

            int totalRowCount, rowCount;

            if (!File.Exists(ApplicationArguments.ExcludeSlavesFilePath))
            {
                return false;
            }

            while (retry)
            {
                try
                {

                    using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ApplicationArguments.ExcludeSlavesFilePath)))
                    {
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                        totalRowCount = DataUtilities.GetLastUsedRow(worksheet);

                        for (rowCount = 2; rowCount <= totalRowCount; rowCount++)
                        {
                            if (worksheet.Cells[rowCount, 1].Value.ToString().Equals(IP) && defect.Contains(worksheet.Cells[rowCount, 2].Value.ToString()))
                            {
                                if (terminateExecution)
                                {
                                    Console.WriteLine("IP is blacklisted, so terminating the execution.");
                                    Environment.Exit(1);
                                }
                                else
                                    return true;
                            }
                        }

                        retry = false;
                        return false;
                    }
                }
                catch (IOException)
                {
                    //retry if excel sheet is already open 
                    retry = true;

                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }


        public static bool isFileEmpty(string path)
        {
            if (File.Exists(path))
            {
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
                {
                    ExcelWorksheet excelworksheet = xlPackage.Workbook.Worksheets[1];

                    var START_ROW_END_ROW = 1;
                    //var END_ROW = DataUtilities.GetLastUsedRow(excelworksheet);
                    var Col_START = 1;
                    var CELLRANGE = excelworksheet.Cells[START_ROW_END_ROW, Col_START, START_ROW_END_ROW, excelworksheet.Cells.End.Column];

                    var ISROWEMPTY = CELLRANGE.All(c => c.Value == null);
                    return ISROWEMPTY;




                }
            }
            else
            {
                throw new Exception("Sheet not found path:" + path);
            }

        }

        public static string GetAllRestoreDBCases()
        {
            string testCasesList = string.Empty;
            List<string> testCases = new List<string>();
            try
            {

                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings["-RestoreMasterDBCasesPath"].ToString())))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    int totalRowCount = DataUtilities.GetLastUsedRow(worksheet);

                    for (int rowCount = 2; rowCount <= totalRowCount; rowCount++)
                    {
                        if (worksheet.Cells[rowCount, 3].Value != null)
                        {
                            testCases.Add(worksheet.Cells[rowCount, 3].Value.ToString());
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
            if (testCases.Count > 0)
            {
                testCasesList = string.Join(",", testCases.ToArray());
            }

            return testCasesList;
        }

        public static string GetNonVerificationCasesList()
        {
            string testCasesList = string.Empty;
            List<string> testCases = new List<string>();
            try
            {

                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings["-NonVerificationCasesList"].ToString())))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    int totalRowCount = DataUtilities.GetLastUsedRow(worksheet);

                    for (int rowCount = 2; rowCount <= totalRowCount; rowCount++)
                    {
                        if (worksheet.Cells[rowCount, 3].Value != null)
                        {
                            testCases.Add(worksheet.Cells[rowCount, 3].Value.ToString());
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
            if (testCases.Count > 0)
            {
                testCasesList = string.Join(",", testCases.ToArray());
            }

            return testCasesList;
        }

        public static string GetAllFixCasesList()
        {
            string testCasesList = string.Empty;
            List<string> testCases = new List<string>();
            try
            {

                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings["-AllFixLogCasesList"].ToString())))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    int totalRowCount = DataUtilities.GetLastUsedRow(worksheet);

                    for (int rowCount = 2; rowCount <= totalRowCount; rowCount++)
                    {
                        if (worksheet.Cells[rowCount, 1].Value != null)
                        {
                            testCases.Add(worksheet.Cells[rowCount, 1].Value.ToString());
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
            if (testCases.Count > 0)
            {
                testCasesList = string.Join(",", testCases.ToArray());
            }

            return testCasesList;
        }

        public static void ProcessKiller()
        {
            if (bool.Parse(ConfigurationManager.AppSettings["KillProcessesAfterCasesCompletion"]))
            {
                try
                {
                    if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                    {
                        SamsaraHelperClass.LogOutUser();
                        SamsaraHelperClass.WaitForWindowToQuit("Logout", TimeSpan.FromMinutes(6));
                    }
                    Process[] _processes = Process.GetProcessesByName("Prana");
                    foreach (Process proc in _processes)
                    {
                        proc.Kill();
                    }


                    string[] windowTitlesToClose = {
                   "Prana TradeService UI",
                   "'Rule Mediator Engine'",
                   "'Esper Calculation Engine'",
                   "DropCopyFileReader",
                   "'Basket Compliance Service'"
                   };


                    Process[] myProcesses = Process.GetProcesses();
                    foreach (Process myProcess in myProcesses)
                    {
                        foreach (string title in windowTitlesToClose)
                        {
                            if (myProcess.MainWindowTitle.Contains(title))
                            {
                                myProcess.CloseMainWindow();
                            }
                        }
                    }

                    ICommandUtilities cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                    cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                    ShutDownSimulator();

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["ShutDownSimulator"]))
                    {
                        cmdUtilities.ExecuteCommand("ShutDownSimulator.Bat");
                    }

                    //cmdUtilities.ExecuteCommand("RevertingPref.bat");
                }
                catch (Exception ex)
                {

                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

        }

        public static void VideoRecording(string testCaseID, string action, string testCaseResult)
        {
            try
            {
                if (ConfigurationManager.AppSettings["VideoRecordFailedTestCases"].ToString().ToLower().Equals("true"))
                {    
                    string screenRecordingEssentials = @"E:\ScreenRecordingEssentials";
                    string masterScreenRecordingEssentials = ConfigurationManager.AppSettings["masterScreenRecordingEssentials"];
                    string startRecordingBatchFile = @"E:\ScreenRecordingEssentials\start_recording.bat";
                    string stopRecordingBatchFile = @"E:\ScreenRecordingEssentials\stop_recording.bat";
                    string nircmdApplicationFile = @"E:\ScreenRecordingEssentials\nircmd.exe";
                    string recordingsFolder = @"E:\TestCasesVideoRecordings";

                    // Ensure the recordings essentials folder exists
                    if (!Directory.Exists(screenRecordingEssentials))
                    {
                        Console.WriteLine("ScreenRecordingEssentials folder not found. Copying from master...");
                        if (!Directory.Exists(screenRecordingEssentials))
                        {
                            Directory.CreateDirectory(screenRecordingEssentials);
                            Console.WriteLine("Created recordings folder at: " + screenRecordingEssentials);
                        }
                        File.Copy(Path.Combine(masterScreenRecordingEssentials, "start_recording.bat"), startRecordingBatchFile, true);
                        File.Copy(Path.Combine(masterScreenRecordingEssentials, "stop_recording.bat"), stopRecordingBatchFile, true);
                        File.Copy(Path.Combine(masterScreenRecordingEssentials, "nircmd.exe"), nircmdApplicationFile, true);
                        Console.WriteLine("ScreenRecordingEssentials folder copied to: " + screenRecordingEssentials);
                    }

                    else
                    {
                        if (!File.Exists(startRecordingBatchFile) && !File.Exists(stopRecordingBatchFile) && !File.Exists(nircmdApplicationFile))
                        {
                            Console.WriteLine("ScreenRecordingEssentials files not found. Copying from master...");
                            File.Copy(Path.Combine(masterScreenRecordingEssentials, "start_recording.bat"), startRecordingBatchFile, true);
                            File.Copy(Path.Combine(masterScreenRecordingEssentials, "stop_recording.bat"), stopRecordingBatchFile, true);
                            File.Copy(Path.Combine(masterScreenRecordingEssentials, "nircmd.exe"), nircmdApplicationFile, true);
                            Console.WriteLine("ScreenRecordingEssentials folder copied to: " + screenRecordingEssentials);
                        }
                    }

                    // Ensure the recordings folder exists
                    if (!Directory.Exists(recordingsFolder))
                    {
                        Directory.CreateDirectory(recordingsFolder);
                        Console.WriteLine("Created recordings folder at: " + recordingsFolder);
                    }
                    // Validate action and decide which batch file to call
                    string batchFileToExecute = null;
                    if (action.Equals("startRecording", StringComparison.OrdinalIgnoreCase))
                    {
                        batchFileToExecute = startRecordingBatchFile;
                    }
                    else if (action.Equals("stopRecording", StringComparison.OrdinalIgnoreCase))
                    {
                        batchFileToExecute = stopRecordingBatchFile;
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        Console.WriteLine("Invalid action specified.");
                        return;
                    }

                    // Execute the batch file with the testCaseID as an argument
                    if (batchFileToExecute != null)
                    {
                        ExecuteBatchFile(batchFileToExecute, testCaseID);
                    }
                    // If the action is stopRecording, handle the handle the recording file based on test case result
                    if (action.Equals("stopRecording", StringComparison.OrdinalIgnoreCase))
                    {
                        string recordedFilePath = Path.Combine(recordingsFolder, string.Format("{0}.webm", testCaseID));
                        Thread.Sleep(3000);
                        if (testCaseResult.Equals("Pass", StringComparison.OrdinalIgnoreCase) && File.Exists(recordedFilePath))
                        {
                            // Delete the recorded file if the test case result is fail
                            File.Delete(recordedFilePath);
                            Console.WriteLine("Deleted recorded file: {0}", recordedFilePath);
                        }
                        else
                        {
                            Console.WriteLine("Recorded file kept: {0}", recordedFilePath);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Video recording of failed test cases is disabled" );
                }
                
            }

            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error in recording test case: {0}", ex.Message));
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }

        }


        static void ExecuteBatchFile(string batchFilePath, string argument)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = batchFilePath;
                process.StartInfo.Arguments = argument;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error executing batch file: {0}", ex.Message));
            }
        }




    }
}