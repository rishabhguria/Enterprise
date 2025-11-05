using System;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using System.IO;

class Program
{

    
    public static DateTime currentDate = DateTime.Now;
    public static string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
    public static string logFolderPath = "ClosingLogs";
    public static string logFilePath = Path.Combine(logFolderPath, $"Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static DateTime initialTime;
    public static DateTime finalTime;
    public static int closed = 0;

    static void Main()
    {
        initialTime = DateTime.Now;

        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        string currentUser = "Current User : " + identity?.Name;

        // Get the local IP address of the machine
        string localIp = "IP : " + GetLocalIPAddress();

        CreateLogs("", 0);
        CreateLogs(currentUser, 0); 
        CreateLogs(localIp, 0);
        CreateLogs("", 0);

        string[] applicationFullPaths = ReadApplicationFullPathsFromFile("ApplicationListToClose.txt");

        if(applicationFullPaths == null)
        {
            Console.WriteLine("The Given file path is Not valid for ApplicationListToClose.txt !!");
            string msg = "The Given file path is Not valid for ApplicationListToClose.txt !!";
            CreateLogs(msg);
            return;
        }
        
        int notCloseCount = 0;
        

        foreach (string fullPath in applicationFullPaths)
        {
            if (IsApplicationRunning(fullPath))
            {
                try
                {
                    bool isApplicationClose = StopApplication(fullPath);
                    if (isApplicationClose == false)
                    {
                        notCloseCount++;
                        Console.WriteLine($"{fullPath} is Not Closed !!");
                        string msg = $"{fullPath} is Not Closed !!";
                        CreateLogs(msg);
                    }
                    else
                    {
                        
                    }
                }
                catch
                {

                }
            }
            else
            {
                notCloseCount++;
                Console.WriteLine($"{fullPath} is not running.");
                string msg = $"{fullPath} is not running.";
                CreateLogs(msg);
            }
        }

        finalTime = DateTime.Now;

        Console.WriteLine("");
        CreateLogs("", 0);
        Console.WriteLine("[" + closed +"]     Applications Closed SuccessFully!!");
        CreateLogs("[" + closed + "]     Applications Closed SuccessFully!!", 0);

        TimeSpan timeDifference = finalTime - initialTime;
        Console.WriteLine("");
        string totalTimeTaken = timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";

        CreateLogs("Total Time Taken : " + totalTimeTaken, 0);
        string msg1 = "*****************************************************************************************************************************";
        CreateLogs(msg1, 0);
        
    }

    static bool IsApplicationRunning(string applicationPath)
    {
        Process[] processes = Process.GetProcesses();

        foreach (Process process in processes)
        {
            try
            {
                string processPath = process.MainModule.FileName;

                if (String.Compare(processPath, applicationPath, StringComparison.OrdinalIgnoreCase) == 0)

                {
                    //Console.WriteLine(applicationPath + " is running!!");
                    return true;
                }
            }
            catch (Exception)
            {
                // Some processes might not allow access to their MainModule, ignore them
            }
        }

        return false;
    }

    public static string GetLocalIPAddress()
    {
        string localIp = "N/A";

        try
        {
            // Get the host name of the local machine
            string hostName = Dns.GetHostName();

            // Get the IP addresses associated with the host
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // Find the first IPv4 address (ignore IPv6 for simplicity)
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIp = address.ToString();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting local IP address: {ex.Message}");
        }

        return localIp;
    }

    static bool StopApplication(string applicationPath)
    {
        Process[] processes = Process.GetProcesses();
        bool atLeastOneClosed = false;

        foreach (Process process in processes)
        {
            try
            {
                string processPath = process.MainModule.FileName;

                if (String.Compare(processPath, applicationPath, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    process.Kill();
                    Console.WriteLine(applicationPath + " is Closed Successfully!!");
                    string msg = applicationPath + " is Closed Successfully!!";
                    CreateLogs(msg);
                    atLeastOneClosed = true;
                    closed++;
                }
            }
            catch
            {

            }
        }
        return atLeastOneClosed;
    }


    static string[] ReadApplicationFullPathsFromFile(string filePath)
    {

        if (!File.Exists(filePath))
        {
            return null;
        }
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length == 0)
        {
            return new string[0];
        }

        string mainFolderPath = lines[0];

        string[] fullPaths = new string[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            fullPaths[i - 1] = Path.Combine(mainFolderPath, lines[i]);
        }

        return fullPaths;
    }

    public static void CreateLogs(string logMessage, int dateCheck = 1)
    {

        try
        {
            if (File.Exists(logFilePath))
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                    string msg;

                    if (dateCheck == 0)
                    {
                        msg = logMessage;
                    }
                    else
                    {
                        msg = formattedDateTime + "    " + logMessage;
                    }
                    writer.WriteLine(msg);
                    
                }

            }
            else
            {
                using (StreamWriter writer = File.CreateText(logFilePath))
                {
                    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                    string msg;

                    if (dateCheck == 0)
                    {
                        msg = logMessage;
                    }
                    else
                    {
                        msg = formattedDateTime + "    " + logMessage;
                    }
                    writer.WriteLine(msg);
                    
                }
            }
        }
        catch { }

    }
}
