using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using System.ServiceProcess;
namespace Nirvana.TestAutomation.Utilities
{
    public class SqlUtilities : ICommandUtilities
    {
        public void ExecuteCommand<T1>(T1 queryName)
        {
            try
            {
                string newPath = ApplicationArguments.ApplicationStartUpPath + "\\SQLQueries";
                string command = string.Empty;
                command = "sqlcmd -S " + ApplicationArguments.DBInstanceName + " -U sa -P NIRvana2@@6 -d " + ApplicationArguments.ClientDB + " -i ";
                string query = command + "\"" + newPath + "\\" + queryName + "\"";
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + query);
                Process process = null;
                process = new Process();
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;
                ProcessInfo.RedirectStandardError = true;
                ProcessInfo.RedirectStandardOutput = true;
                process = Process.Start(ProcessInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("output>>" + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("error>>" + e.Data);
                process.BeginErrorReadLine();

                process.WaitForExit();

                Console.WriteLine("ExitCode: {0}", process.ExitCode);
                process.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void ExecuteCommand<T1>(T1 queryName,string masterDB)
        {
            try
            {
                string newPath = ApplicationArguments.ApplicationStartUpPath + "\\SQLQueries";
                string command = string.Empty;
                command = "sqlcmd -S " + ApplicationArguments.DBInstanceName + " -U sa -P NIRvana2@@6 -d " + ApplicationArguments.ClientDB + " -i ";
                string query = command + "\"" + newPath + "\\" + queryName + "\"";
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + query);
                Process process = null;
                process = new Process();
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;
                ProcessInfo.RedirectStandardError = true;
                ProcessInfo.RedirectStandardOutput = true;
                process = Process.Start(ProcessInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("output>>" + e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("error>>" + e.Data);
                process.BeginErrorReadLine();

                process.WaitForExit();

                Console.WriteLine("ExitCode: {0}", process.ExitCode);
                process.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void ExecuteQuery(string Query)
        {
            try
            {
                string[] servicesToCheck = GetServicesFromConfig();

                foreach (var serviceName in servicesToCheck)
                {

                    if (!EnsureSqlServiceIsRunning(serviceName))
                    {
                        Console.WriteLine("Failed to ensure SQL Server service is running.");
                        ProcessStartInfo info = new ProcessStartInfo(@"E:\DistributedAutomation\RestartMachine.bat");
                        info.CreateNoWindow = true;
                        info.UseShellExecute = false;
                        Process.Start(info);
                        SaveAndExit(0);
                        System.Environment.Exit(1);
                    }
                }
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString);
                con.Open();
                Console.WriteLine("Database connection established successfully!");
                SqlCommand sc = new SqlCommand(Query, con);
                object o = sc.ExecuteScalar();
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:-" + ex.Message);
                throw;
            }
        }

        public static void ExecuteQueryParameter(string Query, String Account, String UserID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString);
            con.Open();
            SqlCommand sc = new SqlCommand(Query, con);
            sc.Parameters.AddWithValue("@Account", Account); // Pass Account to @FundName
            sc.Parameters.AddWithValue("@UserID", UserID);
            sc.ExecuteNonQuery();
            con.Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void ExecuteQuerySM(string Query)
        {
            try
            {
                SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["SMConnectionString"].ConnectionString);
                con1.Open();
                SqlCommand sc = new SqlCommand(Query, con1);
                object o = sc.ExecuteScalar();
                con1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:-" + ex.Message);
                throw;
            }
        }

        static string[] GetServicesFromConfig()
        {
            string servicesConfig = ConfigurationManager.AppSettings["ServicesToCheck"];
            if (string.IsNullOrEmpty(servicesConfig))
            {
                Console.WriteLine("No services specified in the configuration.");
                return Array.Empty<string>();
            }

            return servicesConfig.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

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
                    throw;
            }
        }

        private static bool EnsureSqlServiceIsRunning(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);

                if (service.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("SQL Server service- " + serviceName + " is already running.");
                    return true;
                }

                Console.WriteLine("SQL Server service- " + serviceName + " is not running. Attempting to start the service...");
                string batchFilePath = @"E:\DistributedAutomation\" + serviceName + ".lnk";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = batchFilePath,
                    UseShellExecute = true,
                    Verb = "runas",
                };

                Console.WriteLine("Running batch file to start the service...");
                Process process = Process.Start(startInfo);
                process.WaitForExit(10000);
                Console.WriteLine("Batch file executed successfully.");
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));

                if (service.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("SQL Server service- " + serviceName + " started successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to start SQL Server service- " + serviceName + ".");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while checking or starting SQL Server service: " + ex.Message);
                return false;
            }
        }

        public static string ExecuteQueryWithResult(string Query, string columnValueToFetch="ALLDATA",string connDBString ="SMConnectionString" )
        {
            string data = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connDBString].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!columnValueToFetch.Equals("ALLDATA"))
                                {
                                    data = reader[columnValueToFetch].ToString();

                                }
                                Console.WriteLine(data);
                                return data;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
           
                Console.WriteLine("An exception occurred while executing the query: " + ex.Message);
            }

            return data; 
        }

        public static DataTable GetDataFromQuery(string query, string db = null)
        {
            DataTable resultTable = new DataTable();
            try
            {
                if (db == null)
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SMConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Load the data into the result DataTable
                                resultTable.Load(reader);
                            }
                        }
                    }
                }
                else {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Load the data into the result DataTable
                                resultTable.Load(reader);
                            }
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving data: " + ex.Message);
            }

        return resultTable;
    
        }

    }

}
