using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace Nirvana.TestAutomation.Utilities
{
    public class ProcessControlManager
    {
        const int SW_MINIMIZE = 6;

        // External method to minimize window
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // External method to check if window is visible
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        public static void ProcessStarter(string batchFile, string directoryPath)
        {
            try
            {

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = batchFile;
                startInfo.WorkingDirectory = directoryPath;
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();


            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
        public static void CloseCmdProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName("cmd");

            foreach (Process process in processes)
            {
                try
                {
                    if (process.MainWindowTitle.Contains(processName))
                    {
                        process.CloseMainWindow();
                        process.WaitForExit(5000);
                        if (!process.HasExited)
                        {
                            process.Kill();
                        }
                        Console.WriteLine("Closed Command Prompt with name: " + processName);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error closing Command Prompt: " + ex.Message);
                }
            }
        }

        private static bool IsProcessNameExcluded(string processName)
        {
            try
            {
                string excludedNamesString = ConfigurationManager.AppSettings["-ExcludeMinimizeWindow"];
                string[] excludedNames = excludedNamesString.Split(',');

                return Array.Exists(excludedNames, name => name.Equals(processName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return false;
        }


        public static void MinimizeAllWindowsExceptSpecified()
        {
            try
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {
                    // Check if the process window is currently visible and it's not one of the specified names
                    if (IsWindowVisible(process.MainWindowHandle) &&
                        !IsProcessNameExcluded(process.ProcessName))
                    {
                        // Minimize the window
                        ShowWindow(process.MainWindowHandle, SW_MINIMIZE);
                        //Console.WriteLine(process.ProcessName+" window minimized...");
                    }
                    // If it's already minimized, you can add an optional message here.
                    else if (!IsWindowVisible(process.MainWindowHandle))
                    {
                        //Console.WriteLine(process.ProcessName+" window is already minimized.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static void KillApplication(string processName)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = string.Format("/F /IM {0} /T", processName),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();

                // Read the output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Successfully killed the "+processName+" process.");
                }
                else
                {
                    Console.WriteLine("Error killing the "+ processName+" process. ");
                }
            }
        }
        public static void ProcessStarter1(string batchFile, string directoryPath, string outputFile)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = batchFile;
                startInfo.WorkingDirectory = directoryPath;
                startInfo.Arguments = outputFile;
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

       
    }
}
