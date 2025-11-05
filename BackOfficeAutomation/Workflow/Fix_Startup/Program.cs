using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UIAutomationClient;

class Program
{
      
    public class Config
    {
        public string FIXExePath { get; set; }
        public string LogFilePath { get; set; }
        public string LinePattern { get; set; }

        public string Portnumber { get; set; }
       
    }
    // public static string logFolderPath = Directory.GetCurrentDirectory() + "\\Logs";
    public static string MainLogFilePath="";
    public static void CreateLogs(string logMessage, int dateCheck = 1)
    {
        try
        {
            Console.WriteLine(logMessage);
            if (File.Exists(MainLogFilePath))
            {
                using (StreamWriter writer = File.AppendText(MainLogFilePath))
                {
                    string formattedDateTime = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
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
                    writer.WriteLine("");
                }

            }
            else
            {
                using (StreamWriter writer = File.CreateText(MainLogFilePath))
                {
                    string formattedDateTime = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
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
                    writer.WriteLine("");
                }
            }
        }
        catch (Exception ex)
        {
            //exitcode = 9;
            logMessage = "Getting exception " + ex.Message + " while saving logs.";
            Console.WriteLine(logMessage);
            CreateLogs(logMessage);
        }

    }

    static async Task Main()
    {
        InitializeLogFile();
        Thread.Sleep(5000);
        string folderpath=Directory.GetCurrentDirectory()+ @"\Logs\";
        CreateLogs("Starting FIX Startup Bot....");
        DateTime compareTime = GetLatestFileCreationTime(folderpath);

        if (compareTime == DateTime.MinValue)
        {
            CreateLogs("No files found in the folder.");
            //Console.WriteLine("No files found in the folder.");
            return;
        }
        string configFilePath = "FixStartupConfig.csv"; // Path to your config file
        var configs = LoadConfig(configFilePath);
        foreach (var config in configs)
        {
            //DateTime compareTime = DateTime.ParseExact("7/5/2024 03:10:36 AM", "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            string fixexepath = config.FIXExePath;
            string logFilePath = config.LogFilePath;
            string linepattern=config.LinePattern;
            string portnumber = config.Portnumber;

            int portnum=int.Parse(portnumber);
           
            
            int result = await LaunchAndMonitor(fixexepath, logFilePath, linepattern,compareTime,portnum);
            if(result == 0)
            {
                CreateLogs("Component " + fixexepath + "  Started");
                Console.WriteLine("Component " + fixexepath + "  Started");
                Console.WriteLine("--------------------------------------------------------------------");
            }
            else
            {
                CreateLogs("Component " + fixexepath + "  Not Started");

                Console.WriteLine("Component " + fixexepath + "  Not Started");
                Console.WriteLine("--------------------------------------------------------------------");
               // Console.ReadLine();
                Environment.Exit(1);
            }
        }
        //Console.ReadLine();
        Environment.Exit(0);
    }
    
            public static DateTime GetLatestFileCreationTime(string folderPath)
    {
        try
        {
            var directoryInfo = new DirectoryInfo(folderPath);
            var latestFile = directoryInfo.GetFiles().OrderByDescending(f => f.CreationTime).FirstOrDefault();

            return latestFile != null ? latestFile.CreationTime : DateTime.MinValue;
        }
        catch (Exception ex)
        {

            CreateLogs("Error getting latest file creation time: " + ex.Message);
            return DateTime.MinValue;
        }
    }
    private static void InitializeLogFile()
    {
        // Get the current date and time
        DateTime currentDate = DateTime.Now;

        // Create a filename with the current date and time
        
        string logFilename = Directory.GetCurrentDirectory() + $"\\Logs\\Log_{currentDate.ToString("yyyyMMdd_HHmmss")}.txt";
        MainLogFilePath= logFilename;
        Directory.CreateDirectory(Path.GetDirectoryName(logFilename));

        try
        {
            // Create the log file
            Directory.CreateDirectory(Path.GetDirectoryName(logFilename));
            using (StreamWriter writer = File.CreateText(logFilename))
            {
                writer.WriteLine($"--- Log file created on {currentDate} ---");
            }

            CreateLogs("Log file created: " + logFilename);
        }
        catch (Exception ex)
        {
            // If there is an error, display the error message
           CreateLogs("Error creating log file: " + ex.Message);
        }
    }
    static async Task<int> IsPortInUse(int port)
    {
        const int checkIntervalMs = 1000; // Check every 1 second
        const int durationMs = 180000;    // Total duration: 3 minutes
        int elapsedMs = 0;

        while (elapsedMs < durationMs)
        {
            if (IsPortAvailable(port))
            {
                await Task.Delay(checkIntervalMs);
                elapsedMs += checkIntervalMs;
            }
            else
            {
                return 1; // Port is in use
            }
        }

        return 0; // Port is not in use
    }

    static bool IsPortAvailable(int port)
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.Stop();
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
    }
    static  async Task<int>  LaunchAndMonitor(string fixexepath,string logFilePath,string linepattern,DateTime compareTime,int portnum)
    {
       
        int result = 0;
        string workingDirectory = Path.GetDirectoryName(fixexepath);
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fixexepath,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = false,
                UseShellExecute = true,
                CreateNoWindow = false
            }
        };

        process.Start();
        Thread.Sleep(5000);

        if (portnum != 0)
        {

            int portresult = await IsPortInUse(portnum);

            if (portresult != 1)
            {
                CreateLogs($"Port {portnum} is Not use.");
                return 1;
            }

           CreateLogs($"Port {portnum} is in use. Now Checking Logs");
        }

        DateTime logfileupdatetime = new DateTime();
        if (File.Exists(logFilePath))
        {
            // Get the last write time of the file
            logfileupdatetime = File.GetLastWriteTime(logFilePath);

            CreateLogs($"Last write time of {logFilePath}: {logfileupdatetime}");
        }
        // process.WaitForExit();
        Regex regex = new Regex(linepattern);

        // Match the pattern in the line
        DateTime startTime = DateTime.Now;
        CreateLogs((DateTime.Now - startTime).TotalSeconds + "Should be less than 600 ");
        while ((DateTime.Now - startTime).TotalSeconds <= 600) // 5 minutes timeout
        {
             
            CreateLogs("Trying to Check Logs");
            string lastMatch = null;
           
            string filename=Path.GetFileName(logFilePath);
            string copiedFilePath = @"TempLogFiles/" + filename;
            CreateLogs("Going to Copy File");
           CreateLogs(logFilePath);
            CreateLogs(copiedFilePath);
            CopyFile(logFilePath, copiedFilePath);

            using (FileStream fileStream = new FileStream(copiedFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Seek to the end of the file
                fileStream.Seek(0, SeekOrigin.End);

                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    while (true)
                    {
                        // Read the previous line
                        string line = ReadPreviousLine(reader, fileStream);

                        if (line != null)
                        {
                            //CreateLogs(line); // For debugging purposes
                            Match match = regex.Match(line);
                            if (match.Success)
                            {
                                Console.WriteLine(match.Value);
                                lastMatch = match.Groups[1].Value;
                                string timestamp = match.Groups[1].Value;
                                Console.WriteLine($"Timestamp extracted: {timestamp}");
                                //break; // Exit the loop after finding the first match
                                if (lastMatch != null)
                                {
                                    DateTime lastTiming = new DateTime();
                                    //Console.WriteLine(lastMatch);
                                    string[] parts = lastMatch.Split(',');
                                    DateTime dateTime;
                                    if (parts.Length == 2)
                                    {
                                        string dateString = parts[0]; // Date and time part

                                        if (dateString.Length == 8)
                                        {
                                            string[] timeParts = dateString.Split(':');
                                            int hours = int.Parse(timeParts[0]);
                                            int minutes = int.Parse(timeParts[1]);
                                            int seconds = int.Parse(timeParts[2]);

                                            // Create a new DateTime variable by combining date from dateWithDate and time from timeString
                                            dateTime = new DateTime(logfileupdatetime.Year, logfileupdatetime.Month, logfileupdatetime.Day, hours, minutes, seconds);

                                        }
                                        else
                                        {
                                            // Parse dateString

                                            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                                            {
                                                Console.WriteLine("Parsed datetime: " + dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failed to parse datetime.");
                                            }
                                        }

                                        lastTiming = dateTime;

                                    }
                                    else
                                    {
                                        lastTiming = DateTime.ParseExact(lastMatch, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                                    }
                                    CreateLogs("Before Compare Time : ");
                                    CreateLogs(lastTiming.ToString());
                                    CreateLogs(compareTime.ToString());
                                    result = lastTiming >= compareTime ? 0 : 1;
                                    if (result == 0)
                                    {
                                        Console.WriteLine("Found Line With Expected Time");
                                        return result;
                                        
                                    }
                                    else
                                    {
                                        //Log file Does Not contain time stamp greater than compare time
                                        CreateLogs("Log File Did Not Contain time stamp greater than compare time");
                                        break;
                                    }
                                    Console.WriteLine(result);
                                }
                            }
                        }
                        else
                        {
                            CreateLogs("Reached Last Line of Log did not find Match");
                            // No more lines to read, exit loop
                            break;
                        }
                    }
                }
            }


                if (result==1)
            {
                CreateLogs("No match found. Waiting 30 seconds...");
                Thread.Sleep(3000); // Wait for 30 seconds before retrying
            }
        }
        //process.WaitForExit();
        return result;
    }

    static void CopyFile(string sourceFilePath, string destinationFilePath)
    {
        bool flag = true;
        DateTime startTime=DateTime.Now;
        while (flag && (DateTime.Now - startTime).TotalSeconds <= 300)
        {

            try
            {
                CreateLogs("Inside File Copying for " + sourceFilePath + destinationFilePath);
                const int bufferSize = 1024 * 1024; // 1MB buffer
                byte[] buffer = new byte[bufferSize];
                int bytesRead;

                using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destinationStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                flag = false;
            }
            catch (Exception ex)
            {
                CreateLogs(ex.Message);
                CreateLogs(ex.StackTrace);
                CreateLogs("error in copying files");
            }
        }
    }

    static string ReadPreviousLine(StreamReader reader,FileStream fileStream)
    {
        long position = fileStream.Position;
        if (position == 0)
            return null;

        byte[] buffer = new byte[1];
        StringBuilder line = new StringBuilder();
        bool endOfLineFound = false;

        while (position > 0)
        {
            fileStream.Seek(--position, SeekOrigin.Begin);
            fileStream.Read(buffer, 0, 1);

            char c = (char)buffer[0];
            if (c == '\n' && line.Length > 0)
            {
                endOfLineFound = true;
                break;
            }

            if (c != '\r' && c != '\n')
                line.Insert(0, c);
        }

        // If we're at the start of the file and haven't found a newline, read the remainder
        if (!endOfLineFound && position == 0)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(buffer, 0, 1);
            line.Insert(0, (char)buffer[0]);
        }

        return line.ToString();
    }
    public static string ReadFolderPath(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath).Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading folder path: " + ex.Message);
            return null;
        }
    }

    
    public static List<Config> LoadConfig(string configFilePath)
    {
        var configs = new List<Config>();

        try
        {
            using (var reader = new StreamReader(configFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, MissingFieldFound = null }))
            {
                configs = csv.GetRecords<Config>().ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading config file: " + ex.Message);
        }

        return configs;
    }
}

