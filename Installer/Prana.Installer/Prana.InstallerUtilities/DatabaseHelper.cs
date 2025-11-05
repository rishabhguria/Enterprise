using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Prana.InstallerUtilities
{
    public static class DatabaseHelper
    {
        private static String publishStringSMDB = @"/Action:Publish /SourceFile:""{0}"" /TargetConnectionString:""{1}"" /p:BlockOnPossibleDataLoss=False";
        private static String publishStringClient = @"/Action:Publish /SourceFile:""{0}"" /TargetConnectionString:""{1}"" /p:BlockOnPossibleDataLoss=False /Variables:SecurityMaster={2} /Variables:Historicals=Historicals /Variables:HistoricalServer=HistoricalServer /Variables:Veda=Veda_SharedData /Variables:VedaServer=VedaServer /Variables:Bob=Bob /Variables:BobServer=BobServer";

        private static String[] _sqlVersionName = ConfigurationManager.AppSettings["SQLVersionName"].Split(',');
        private static String[] _sqlVersionNumber = ConfigurationManager.AppSettings["SQLVersionNumber"].Split(',');

        public static event EventHandler<String> Log;

        public static void PublishDataBase(String clientCon, String clientDac, String smdbCon, String smdbDac)
        {
            try
            {
                if (smdbDac != null)
                    PublishDacPac(String.Format(publishStringSMDB, smdbDac, smdbCon), true);

                PublishDacPac(String.Format(publishStringClient, clientDac, clientCon, GetDatabaseName(smdbCon)));
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        private static void PublishDacPac(String args, bool isSMDatabase = false)
        {
            try
            {
                System.Diagnostics.Process publish = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new ProcessStartInfo("SqlPackage.exe");
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.Arguments = args;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                publish.StartInfo = startInfo;
                publish.ErrorDataReceived += publish_ErrorDataReceived;
                publish.OutputDataReceived += publish_OutputDataReceived;
                if (publish.Start())
                {
                    publish.BeginOutputReadLine();
                    publish.BeginErrorReadLine();
                    publish.WaitForExit();
                    if (publish.ExitCode == 0)
                    {
                        if (isSMDatabase)
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Successfully published SM database", true, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Successfully published Client database", true, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    if (publish.ExitCode == 1)
                    {
                        if (isSMDatabase)
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Failed to publish SM database", true, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Failed to publish Client database.", true, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    LoggingHelper.GetInstance().LoggerWrite("Unable to start database publish process", true, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Handles the OutputDataReceived event of the publish control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        static void publish_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                String line = e.Data;
                if (line != null && !line.Trim().Equals(":") && !line.Trim().Equals(string.Empty) && !line.Trim().EndsWith("row(s) affected)"))
                {
                    Console.WriteLine(line);
                    Log(null, line);
                }
            }
            catch (Exception ex)
            {
                Log(null, ex.Message);
            }
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the publish control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        static void publish_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                String line = e.Data;
                if (line != null && !line.Trim().Equals(":") && !line.Trim().Equals(string.Empty))
                {
                    Console.WriteLine(line);

                    LoggingHelper.GetInstance().LoggerWrite(line);

                    Log(null, line);
                }
            }
            catch (Exception ex)
            {
                Log(null, ex.Message);
            }
        }

        /// <summary>
        /// Return the database name from the connection string
        /// </summary>
        /// <param name="conString"></param>
        /// <returns></returns>
        private static String GetDatabaseName(String conString)
        {
            IDbConnection connection = new SqlConnection(conString);
            return connection.Database;
        }

        private static String GetServer(String conString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
            return builder.DataSource;
        }

        public static Boolean CheckSqlPackagePath()
        {
            bool isProcessExists = false;
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo("SqlPackage.exe");
                info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo = info;
                if (process.Start())
                {
                    isProcessExists = true;
                }
                else
                {
                    isProcessExists = false;
                }
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("SqlPackage.exe path not exist");
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);

                return isProcessExists;
            }
            return isProcessExists;
        }

        /// <summary>
        /// Backup a whole database to the specified file.
        /// </summary>
        /// <remarks>
        /// The database must not be in use when backing up
        /// The folder holding the file must have appropriate permissions given
        /// </remarks>
        /// <param name="backUpFile">Full path to file to hold the backup</param>
        public static void BackupDatabase(string connectionString, String path, bool isSMDatabase = false)
        {
            try
            {
                String database = GetDatabaseName(connectionString);
                System.Diagnostics.Process backup = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "SqlCmd";
                startInfo.Arguments = String.Format(@"-S {0} -Q ""BACKUP DATABASE [{1}] TO DISK='{2}' with stats=1"" ", GetServer(connectionString), database, path);
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                backup.StartInfo = startInfo;
                backup.ErrorDataReceived += publish_ErrorDataReceived;

                if (backup.Start())
                {
                    backup.BeginErrorReadLine();

                    bool isBackupSuccess = true;
                    while (!backup.StandardOutput.EndOfStream)
                    {
                        String line = backup.StandardOutput.ReadLine();
                        if (!line.Trim().Equals(":") && !line.Trim().Equals(string.Empty))
                        {
                            Console.WriteLine(line);
                            LoggingHelper.GetInstance().LoggerWrite(line);
                            Log(null, database + " : " + line);

                            if (line.Equals("BACKUP DATABASE is terminating abnormally."))
                            {
                                isBackupSuccess = false;
                            }
                        }
                    }

                    if (backup.ExitCode == 0 && isBackupSuccess)
                    {
                        if (isSMDatabase)
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Successfully backup SM database", true, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Successfully backup Client database", true, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (isSMDatabase)
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Failed to backup SM database. ExitCode: " + backup.ExitCode, true, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        else
                        {
                            LoggingHelper.GetInstance().LoggerWrite("Failed to backup Client database. ExitCode: " + backup.ExitCode, true, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    LoggingHelper.GetInstance().LoggerWrite("Unable to start database backup process", true, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        public static string GetRealDBVersion(string clientConString)
        {
            string realDBVersion = string.Empty;

            DataSet dataset = new DataSet();
            try
            {
                LoggingHelper.GetInstance().LoggerWrite("Fetching Real DB Version. Client Connection: " + clientConString);

                SqlConnection connection = new SqlConnection(clientConString);
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT @@VERSION", connection);
                connection.Open();
                adapter.Fill(dataset);
                connection.Close();
                LoggingHelper.GetInstance().LoggerWrite("Database connected.");

                if (dataset != null && dataset.Tables != null && dataset.Tables.Count == 1 && dataset.Tables[0].Rows != null && dataset.Tables[0].Rows.Count == 1)
                {
                    string versionDetails = dataset.Tables[0].Rows[0][0].ToString();

                    if (_sqlVersionName.Length == _sqlVersionNumber.Length)
                    {
                        for (int i = 0; i < _sqlVersionName.Length; i++)
                        {
                            if (versionDetails.StartsWith(_sqlVersionName[i]))
                            {
                                realDBVersion = _sqlVersionNumber[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        LoggingHelper.GetInstance().LoggerWrite("SQLVersionName & SQLVersionNumber values mismatch in config.");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.GetInstance().LoggerWrite("Unable to fetch Real DB Version (" + string.Join("/", _sqlVersionNumber));

                LoggingHelper.GetInstance().LoggerWrite("Error Message: " + ex.Message, "StackTrace: " + ex.StackTrace);
            }

            return realDBVersion;
        }
    }
}