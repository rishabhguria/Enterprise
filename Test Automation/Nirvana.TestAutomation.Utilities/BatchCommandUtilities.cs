using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Interfaces;

namespace Nirvana.TestAutomation.Utilities
{
    public class BatchCommandUtilities :ICommandUtilities
    {
        public void ExecuteCommand<T1>(T1 batchName, string masterDB = "")
        {
            try
            {
                string newPath = ApplicationArguments.ApplicationStartUpPath + "\\BatchFiles";
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo(newPath + "\\" + batchName);
                Process process = null;
                process = new Process();
                ProcessInfo.WorkingDirectory = newPath;
                ProcessInfo.Arguments = "";
                if (batchName.ToString() == "RestoreDBScript.bat")
                {
                    string DB_Path = ApplicationArguments.DataBasePath;
                    string Instancename = ApplicationArguments.DBInstanceName;
                    string ClientDB_name = ApplicationArguments.ClientDB;
                    //string masterDB = ApplicationArguments.MasterDB;
                    if (!string.IsNullOrEmpty(masterDB))
                    {
                        ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" " + ClientDB_name + " " + masterDB;
                    }
                    else
                    {
                        ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" " + ClientDB_name + "";
                    }
                    
                    //ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" + ClientDB_name + "\"   + masterDB + "\"";
                   
                    //ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" \"" + ClientDB_name + "\" \"" + masterDB + "\"";

                }

                else
                {
                    string master = ApplicationArguments.MasterPranaPrefPath;
                    string current = ApplicationArguments.CurrentPranaPrefPath;
                    ProcessInfo.Arguments = "\"" + master + "\" \"" + current + "\"";
                }
                
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
        public void ExecuteCommand<T1>(T1 batchName)
        {
            try
            {
                string newPath = ApplicationArguments.ApplicationStartUpPath + "\\BatchFiles";
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo(newPath + "\\" + batchName);
                Process process = null;
                process = new Process();
                ProcessInfo.WorkingDirectory = newPath;
                ProcessInfo.Arguments = "";
                if (string.Equals(batchName.ToString(), "DBBackUpScript.bat",StringComparison.OrdinalIgnoreCase))
                {
                    string DB_Path = ApplicationArguments.DataBasePath;
                    string Instancename = ApplicationArguments.DBInstanceName;
                    string ClientDB_name = ApplicationArguments.ClientDB;
                    string masterDB = ApplicationArguments.MasterDB;
                    ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" " + ClientDB_name + " " + masterDB;

                }
                else if (batchName.ToString() == "RestoreDBScript.bat")
                {
                    string DB_Path = ApplicationArguments.DataBasePath;
                    string Instancename = ApplicationArguments.DBInstanceName;
                    string ClientDB_name = ApplicationArguments.ClientDB;
                    //string masterDB = ApplicationArguments.MasterDB;
                   
                        ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" " + ClientDB_name + "";
                    

                    //ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" + ClientDB_name + "\"   + masterDB + "\"";

                    //ProcessInfo.Arguments = "\"" + DB_Path + "\" \"" + Instancename + "\" \"" + ClientDB_name + "\" \"" + masterDB + "\"";

                }

                else
                {
                    string master = ApplicationArguments.MasterPranaPrefPath;
                    string current = ApplicationArguments.CurrentPranaPrefPath;
                    ProcessInfo.Arguments = "\"" + master + "\" \"" + current + "\"";
                }

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
    }
}
