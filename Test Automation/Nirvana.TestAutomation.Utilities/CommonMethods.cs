using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFX.Core;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace Nirvana.TestAutomation.Utilities
{
    public static class CommonMethods
    {
        private static string _directoryFixPath = _directoryFixPath = ApplicationArguments.ApplicationStartUpPath + "\\Screenshots\\" + DateTime.Now.ToString("MM-dd-yy-hh-mm-ss");

        /// <summary>
        /// This Property is used to get the screenshot directory path where the screenshot images going to save.
        /// </summary>
        public static string DirectoryFixPath
        {
            get { return _directoryFixPath; }
        }

        /// <summary>
        /// Gets the directory path for screenshot.
        /// </summary>
        /// <param name="testCaseId">The test case identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Screenshot folder has not been created! \n ("+ ex.Message + " )</exception>
        public static string GetDirectoryPathForScreenshot(string testCaseId)
        {
            string directoryPath = String.Empty;
            try
            {
                directoryPath = DirectoryFixPath + "\\" + testCaseId;
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
            }
            catch(Exception)
            {
                throw;
            }
            return directoryPath;
        }
        /// <summary>
        /// Gets the folder link from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetFolderLinkFromId(string id)
        {
            string GoogleFolderPath = string.Empty;
            try
            {
                GoogleFolderPath = "https://drive.google.com/drive/folders/"+id;
            }
            catch (Exception)
            {
                throw;
            }
            return GoogleFolderPath;
        }
        /// <summary>
        /// Gets the speadsheet link from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetSpeadsheetLinkFromId(string id)
        {
            string GoogleSpeadsheetPath = string.Empty;
            try
            {
                GoogleSpeadsheetPath = "https://docs.google.com/spreadsheets/d/" + id;
            }
            catch(Exception)
            {
                throw;
            }
            return GoogleSpeadsheetPath;
        }

        /// <summary>
        /// Gets the File link from identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static string GetFileLinkFromId(string id)
        {
            string GoogleFilePath = string.Empty;
            try
            {
                GoogleFilePath = "https://drive.google.com/open?id=" + id;
            }
            catch(Exception)
            {
                throw;
            }
            return GoogleFilePath;
        }
        /// <summary>
        /// Gets the log path.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="runDescription">The run description.</param>
        /// <returns></returns>
        public static string GetLogPath(string folder, string runDescription)
        {
            try
            {
                if (folder.Equals(string.Empty))
                {
                    return @"TestLogs\" + runDescription + ".xml";
                }
                else
                {
                    return folder + "\\" + runDescription + ".xml";
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the content of the email.
        /// </summary>
        /// <param name="testReportLink">The test report link.</param>
        /// <param name="testCaseLink">The test case link.</param>
        /// <param name="runDescription">The run description.</param>
        /// <returns></returns>
        public static string GetEmailContent()
        {
			try
            {
            	string reportFileLink = GetFileLinkFromId(ApplicationArguments.ReportFile);
            	string testCaseFileLink = GetSpeadsheetLinkFromId(ApplicationArguments.TestCasesFile);
            	string logPath = GetLogPath(ApplicationArguments.LogFolder, ApplicationArguments.RunDescription);
            	string emailContent = null;
                emailContent = "This is automated testing report for version " + ApplicationArguments.RunDescription.Substring(0, ApplicationArguments.RunDescription.Length - 23) + ". Click <a href=" + reportFileLink + ">here</a> to view the test report. Please see the <a href=" + testCaseFileLink + " >test cases</a> to dig into the details.<br><br> We welcome all comments/suggestions to improve it further.";
                return String.Format(@"" + emailContent, System.IO.Path.GetFileNameWithoutExtension(logPath));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
           return null ;
        }

        // <summary>
        /// method for reading an XML file into a DataTable
        /// </summary>
        /// <param name="file">name (and path) of the XML file</param>
        /// <returns></returns>
        public static DataTable ReadXML(string file)
        {
            DataTable table = new DataTable("Item");
            try
            {
                DataSet lstNode = new DataSet();
                lstNode.ReadXml(file);
                table = lstNode.Tables[0];
                return table;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                   if (rethrow)
                       throw;
            }
            return null;
        }

        /// <summary>
        /// Captures the screenshot.
        /// </summary>
        /// <param name="uiName">Name of the UI.</param>
        public static void CaptureScreenShot(string uiName, string screenShotFolderPath)
        {
            try
            {
                String currentTime = String.Format("{0:HH_mm tt}", DateTime.Now);
                String screenShotName = uiName + "_" + currentTime;
                ScreenCapture.SaveScreenShot(screenShotFolderPath + screenShotName + ".jpeg");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Saves screenshot of UI and get preferences from db
        /// </summary>
        /// <param name="uiName"></param>
        public static void SaveScreenshotAndPreferences(string uiName)
        {
            try
            {

                uiName = "ReconError_" + uiName;
                string screenShotFolderPath = GetDirectoryPathForScreenshot(ApplicationArguments.TestCaseToBeRun) + "\\";
                CaptureScreenShot(uiName, screenShotFolderPath);

                /*string connString = "Server=" + ApplicationArguments.DBInstanceName + ";Database=" + ApplicationArguments.ClientDB + ";Trusted_Connection=True; User Id=sa; Password=NIRvana2@@6";
                SqlConnection sqlConn = new SqlConnection(connString);
                SqlDataAdapter sqlDA = new SqlDataAdapter("select * from T_AL_AllocationDefaultRule", sqlConn);
                DataTable pref = new DataTable();
                sqlDA.Fill(pref);
                StringBuilder str = new StringBuilder();
                str.AppendLine(string.Join(",", pref.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList()));
                foreach (DataRow dr in pref.Rows)
                {
                    str.AppendLine(string.Join(",", dr.ItemArray));
                }
                File.WriteAllText(screenShotFolderPath + @"PrefData_" + uiName + ".csv", str.ToString());*/
                
                string reconFile = "E:\\DistributedAutomation\\ReconErrors\\" + ApplicationArguments.TestCaseToBeRun + ".xlsx";
                if (File.Exists(reconFile))
                {
                    string destinationPath = screenShotFolderPath + ApplicationArguments.TestCaseToBeRun + ".xlsx";
                    File.Copy(reconFile, destinationPath, true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        public static bool VerifyStringInTextFile(string filePath, string searchString, int checkInterval= 6000)
        {
            bool isVerificationSuccessful = false;
            try
            {
                int maxTimeout = 15 * 60 * 1000; 
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds < maxTimeout)
                {
                    Thread.Sleep(checkInterval);
                    string copyFilePath = Path.Combine(Path.GetDirectoryName( filePath), "output_copy.txt");
                    CopyFile(filePath, copyFilePath);
                    if (IsStringPresent(copyFilePath, searchString))
                    {
                        Console.WriteLine("Desired output found.");
                        Console.WriteLine("Time Taken To Completed : " + stopwatch.ElapsedMilliseconds * 0.001);
                        isVerificationSuccessful = true;
                        return isVerificationSuccessful;

                    }

                    Console.WriteLine("Time Running : " + stopwatch.ElapsedMilliseconds * 0.001 + ".......");
                    Thread.Sleep(checkInterval);
                }
                Console.WriteLine("Desired output not found within the timeout period.");
                return isVerificationSuccessful;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Verifying file: " + ex.Message);
                return false;
            }
        }
        public static bool IsStringPresent(string filePath, string searchString)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string fileContent = reader.ReadToEnd();

                    return fileContent.Contains(searchString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return false;
            }
        }
        public static void CopyFile(string sourceFilePath, string destinationFilePath, int checkInterval = 6000)
        {
            int maxAttempts = 5;
            int attempt = 0;

            while (attempt < maxAttempts)
            {
                try
                {
                    if (File.Exists(destinationFilePath))
                    {
                        File.Delete(destinationFilePath);
                    }
                    File.Copy(sourceFilePath, destinationFilePath);
                    return;
                }
                catch (Exception)
                {

                    Thread.Sleep(checkInterval);
 
                }
                attempt++;
            }
           
        }
    }
}
