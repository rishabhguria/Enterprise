using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Configuration;

class ThirdPartyExecutionStatusReporter
{
    public static int RunHelperExe(string arguments)
    {
        const int maxRetries = 3;
        const int timeoutMilliseconds = 5000;
        string exePath = string.Empty;
        string exeNameWithoutExtension = string.Empty;

        try
        {
            exePath = ConfigurationManager.AppSettings["RunHelperExePath"];
            exeNameWithoutExtension = Path.GetFileNameWithoutExtension(exePath);

            if (string.IsNullOrEmpty(exePath) || !File.Exists(exePath))
            {
                Console.WriteLine("Error: RunHelperExePath not found or invalid.");
                return -3;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to read exe path from config: " + ex.Message);
            return -10;
        }


        bool isHelperRunning = false;
        try
        {
            Process[] existingProcesses = Process.GetProcessesByName(exeNameWithoutExtension);
            if (existingProcesses != null && existingProcesses.Length > 0)
            {
                isHelperRunning = true;
                Console.WriteLine("Helper EXE already running.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error checking if EXE is running: " + ex.Message);
        }

        if (!isHelperRunning)
        {
            try
            {
                Console.WriteLine("Helper EXE not running. Starting it...");
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = exePath;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = false;
                Process.Start(psi);
                Thread.Sleep(2000); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to start helper EXE: " + ex.Message);
                return -4;
            }
        }

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                using (NamedPipeClientStream client = new NamedPipeClientStream(".", "UIAutomationPipe", PipeDirection.InOut))
                {
                    Console.WriteLine("[Attempt " + attempt + "] Connecting to named pipe...");
                    client.Connect(timeoutMilliseconds);

                    byte[] request = Encoding.UTF8.GetBytes(arguments);
                    client.Write(request, 0, request.Length);
                    client.Flush();

                    byte[] response = new byte[100];
                    int bytesRead = client.Read(response, 0, response.Length);
                    string resultStr = Encoding.UTF8.GetString(response, 0, bytesRead);

                    Console.WriteLine("Received from helper: " + resultStr);
                    int resultCode = 0;
                    if (int.TryParse(resultStr, out resultCode))
                        return resultCode;
                    else
                        return -1;
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("[Attempt " + attempt + "] Timeout while connecting to named pipe.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Attempt " + attempt + "] Pipe communication failed: " + ex.Message);
            }

            Thread.Sleep(1000); 
        }

        return -2;
    }
}
