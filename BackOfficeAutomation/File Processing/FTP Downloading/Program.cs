using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using WinSCP;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Security.Policy;
using System.Collections.Generic;
using Renci.SshNet;
using System.Reflection;
using FTP_Multiple;

public class XMLDateExtractor
{
    private string xmlContent;

    public XMLDateExtractor() => this.xmlContent = string.Empty;

    public XMLDateExtractor(string xmlContent) => this.xmlContent = xmlContent;

    public DateTime ExtractFilterValue()
    {
        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(this.xmlContent);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("//FilterValue");
            if (xmlNode != null)
            {
                DateTime result;
                if (DateTime.TryParse(xmlNode.InnerText, out result))
                    return result;
                Console.WriteLine("Failed to parse FilterValue as DateTime.");
            }
            else
                Console.WriteLine("FilterValue node not found in XML.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error extracting FilterValue: " + ex.Message);
        }
        return DateTime.MinValue;
    }
}

class Program
{
    public static DateTime initialTime = DateTime.Now;
    public static DateTime finalTime;
    public static string currentDirectory = Directory.GetCurrentDirectory();
    public static string parentDirectory = Directory.GetParent(currentDirectory).FullName;
    public static DateTime currentDate = DateTime.Now;
    public static string currentDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
    public static string logFolderPath = "Logs";
    public static string logFilePath = Path.Combine(logFolderPath, $"FTP_Log_{currentDate.ToString("yyyyMMdd")}.txt");
    public static string processPath = Directory.GetCurrentDirectory() + @"\Config Files\ProcessDateXmlPath.txt";
    public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\Config Files\XmlPath.txt");
    public static List<string> extraCondition = new List<string>();
    public static List<string> partialName2 = new List<string>();
    public static List<string> dateModified = new List<string>();
    public static List<string> shouldSaveDateModified = new List<string>();
    public static List<string> allowMultipleFiles = new List<string>();
    public static int exitCode = -1;
    public static int ftpCount = 0;
    //public static DateTime processDate = GetProcessDate("C:\\XmlPath.txt");

    static void Main(string[] args)
    {

        string csvFilePath = @"Config Files\FTPDetails.csv";
        string brokerName;
        string remoteFile;
        // string extraCondtion = "false";
        string[] remoteFilePaths;

        DataTable ftpDetails;

        try
        {
            try
            {
                ftpDetails = DataFromCsvFile(csvFilePath);
            }
            catch
            {
                string msg1 = "Invalid FTPDetails in CSV File Path or there is problem with the file.";
                Console.WriteLine(msg1);
                CreateLogs(msg1);
                exitCode = 1;
                return;
            }
            foreach (DataRow row in ftpDetails.Rows)
            {
                string ftpHost = row["hostName"].ToString();
                string ftpPort = row["port"].ToString();
                string ftpUsername = row["userName"].ToString();
                string ftpPassword = row["password"].ToString();
                string ftpSecure = row["ftpSecure"].ToString();
                string ftpProtocol = row["protocolType"].ToString();
                string serverName = row["name"].ToString();
                string sshPrivateKeyPath = row["sshPrivateKeyPath"].ToString();

                string sshPrivateKeyPassphrase = row["sshPrivateKeyPassphrase"].ToString();
                string sshHostKeyFingerprint = row["sshHostKeyFingerprint"] == null ? "" : row["sshHostKeyFingerprint"].ToString();
                if (sshPrivateKeyPath == "")
                {
                    sshPrivateKeyPath = null;
                }
                if (sshHostKeyFingerprint == "")
                {
                    sshHostKeyFingerprint = null;
                }
                if (sshPrivateKeyPassphrase == "")
                {
                    sshPrivateKeyPassphrase = null;
                }
                Console.WriteLine("Host Name : " + ftpHost);
                CreateLogs("Host Name : " + ftpHost, 0);

                Console.WriteLine("Port : " + ftpPort);
                CreateLogs("Port : " + ftpPort, 0);

                Console.WriteLine("UserName : " + ftpUsername);
                CreateLogs("UserName : " + ftpUsername, 0);

                Console.WriteLine("BrokerName : " + serverName);
                CreateLogs("BrokerName : " + serverName, 0);

                Console.WriteLine("");
                CreateLogs("", 0);

                Console.WriteLine("");
                CreateLogs("", 0);

                remoteFile = $@"Config Files\{serverName}.csv";
                DataTable remoteFilesDetails;

                string fullPathCsv = currentDirectory + $@"\Downloaded Files\{processDate.ToString("yyyyMMdd")}\{serverName}";

                try
                {
                    remoteFilesDetails = DataFromCsvFile(remoteFile);
                }
                catch
                {
                    string msg1 = "Invalid details in Broker File Path or there is problem with the file.";
                    Console.WriteLine(msg1);
                    CreateLogs(msg1);
                    exitCode = 1;
                    return;
                }
                remoteFilePaths = GetRemoteFilePaths(remoteFile);

                int i = 0;
                int index = 0;
                foreach (DataRow row1 in remoteFilesDetails.Rows)
                {
                    extraCondition.Add(row1["extraCondition"].ToString().ToLower());
                    allowMultipleFiles.Add(row1["allowMultipleFiles"].ToString().ToLower());
                    partialName2.Add(row1["partialName2"].ToString());
                    string dateFactors = row1["dateFactors"].ToString();
                    remoteFilePaths[i] = ReplaceDateTimePlaceholders(remoteFilePaths[i], dateFactors, ref index);
                    dateModified.Add(row1["dateModified"].ToString());

                    if (remoteFilesDetails.Columns.Contains("SaveDateModified"))
                    {
                        shouldSaveDateModified.Add(row1["SaveDateModified"]?.ToString());
                    }
                    else
                    {
                        shouldSaveDateModified.Add(null); 
                    }

                    if (dateModified[i] != null)
                    {
                        dateModified[i] = ReplaceDateTimePlaceholders(dateModified[i], dateFactors, ref index);
                    }
                    index = 0;
                    i++;
                }

                DownloadFilesFromFtp(ftpHost, ftpPort, ftpUsername, ftpPassword, remoteFilePaths, ftpSecure, ftpProtocol, serverName, sshPrivateKeyPath, sshPrivateKeyPassphrase, extraCondition, partialName2, dateModified, allowMultipleFiles, sshHostKeyFingerprint, shouldSaveDateModified);

                dateModified.Clear();
                extraCondition.Clear();
                partialName2.Clear();
                shouldSaveDateModified.Clear();

                Console.WriteLine("");
                CreateLogs("", 0);
            }

            if (ftpCount == ftpDetails.Rows.Count)
                exitCode = 0;
            finalTime = DateTime.Now;
            TimeSpan timeDifference = finalTime - initialTime;

            Console.WriteLine("");
            CreateLogs("", 0);

            string totalTimeTaken = timeDifference.Minutes + " Minutes " + timeDifference.Seconds + " Seconds " + timeDifference.Milliseconds + " Milliseconds ";
            Console.WriteLine("Total Time Taken : " + totalTimeTaken);

            string timeLogMessage = "Total Time Taken : " + totalTimeTaken;
            CreateLogs(timeLogMessage, 0);
            string msg = "********************************************************************************************************************************";
            CreateLogs(msg, 0);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            string msg = $"Error: {e.Message}";
            CreateLogs(msg);
        }
        CreateLogs("Exited with exit code : " + exitCode);
        Environment.Exit(exitCode);
    }

    public static string ReplaceDateTimePlaceholders(string input, string dateFactors, ref int i)
    {
        string pattern = @"\(([^)]+)\)";
        string pattern1 = @"\{([^}]+)\}";
        Regex regex = new Regex(pattern);
        Regex regex1 = new Regex(pattern1);
        // Find the index of the opening and closing curly braces
        int startIndex = dateFactors.IndexOf('{');
        int endIndex = dateFactors.IndexOf('}');
        string[] factorsArray;
        int[] factors = new int[100];
        // Check if both opening and closing curly braces exist
        try
        {
            if (startIndex != -1 && endIndex != -1)
            {
                // Extract the substring within the curly braces
                string factorsSubstring = dateFactors.Substring(startIndex + 1, endIndex - startIndex - 1);

                // Split the substring by commas
                factorsArray = factorsSubstring.Split(';');

                // Convert the string array to an integer array
                factors = factorsArray.Select(int.Parse).ToArray();
            }
            else
            {
            }

            MatchCollection matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                int dateFactor = 0;
                string placeholder = match.Groups[1].Value;

                if (factors.Length > i)
                {
                    dateFactor = factors[i];
                }
                else
                {
                    dateFactor = 0;
                }

                string replacement = GetFormattedDateTime(placeholder, dateFactor);
                int firstOccurenceIndex = input.IndexOf(match.Value);

                if (firstOccurenceIndex != -1)
                {
                    // Replace the first occurrence only
                    input = input.Substring(0, firstOccurenceIndex) + replacement + input.Substring(firstOccurenceIndex + match.Value.Length);
                }

                i++;
            }
            MatchCollection matches1 = regex1.Matches(input);

            foreach (Match match in matches1)
            {
                int dateFactor = 0;
                string placeholder = match.Groups[1].Value;

                if (factors.Length > i)
                {
                    dateFactor = factors[i];
                }
                else
                {
                    dateFactor = 0;
                }
                string replacement = GetFormattedDateTimeCurrentDate(placeholder, dateFactor);
                int firstOccurenceIndex = input.IndexOf(match.Value);

                if (firstOccurenceIndex != -1)
                {
                    // Replace the first occurrence only
                    input = input.Substring(0, firstOccurenceIndex) + replacement + input.Substring(firstOccurenceIndex + match.Value.Length);
                }

                i++;
            }
        }
        catch (Exception ex)
        {
            CreateLogs(ex.ToString());
        }

        return input;
    }

    public static string GetFormattedDateTime(string format, int dateFactor)
    {
        DateTime modifiedDate = processDate.AddDays(dateFactor);

        // Convert the modified date to string with the required format
        string formattedDate = modifiedDate.ToString(format);

        return formattedDate;
    }

    public static string GetFormattedDateTimeCurrentDate(string format, int dateFactor)
    {
        // Add the dateFactor to the currentDate object
        DateTime modifiedDate = currentDate.AddDays(dateFactor);

        // Convert the modified date to string with the required format
        string formattedDate = modifiedDate.ToString(format);

        return formattedDate;
    }

    public static DateTime GetProcessDate(string configPath)
    {
        try
        {
            string xmlFilePath = ReadXmlPathFromConfig(configPath);
            //string xmlFilePath = @"Config Files\ProcessDate.xml";

            if (!string.IsNullOrEmpty(xmlFilePath))
            {
                // Read the XML content from the specified path
                string xmlContent = File.ReadAllText(xmlFilePath);
                XMLDateExtractor parser = new XMLDateExtractor(xmlContent);
                DateTime filterValue = parser.ExtractFilterValue();

                if (filterValue != DateTime.MinValue)
                {
                    return filterValue;
                }
                else
                {
                    Console.WriteLine("Failed to extract FilterValue.");
                }
            }
        }
        catch (Exception ex)
        {
        }
        return DateTime.MinValue;
    }

    public static string ReadXmlPathFromConfig(string configPath)
    {
        try
        {
            // Read the XML file path from the config file
            //string xmlFilePath = File.ReadAllText(configPath);
            //return xmlFilePath;

            using (StreamReader reader = new StreamReader(configPath))
            {
                // Read the entire content of the file
                string fileContent = reader.ReadToEnd();
                return fileContent;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading XML path from config: {ex.Message}");
            return null;
        }
    }

    static void DownloadFilesFromFtp(string ftpHost, string ftpPort, string ftpUsername, string ftpPassword, string[] remoteFilePaths, string ftpSecure, string ftpProtocol, string brokerName, string sshPrivateKeyPath, string sshPrivateKeyPassphrase, List<string> extraCondition, List<string> partialName2, List<string> dateModified, List<string> allowMultipleFiles, string sshHostKeyFingerprint, List<string> shouldSaveDateModified)
    {
        try
        {
            using (WinSCP.Session session = new WinSCP.Session())
            {
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = ftpHost,
                    PortNumber = int.Parse(ftpPort),
                    UserName = ftpUsername,
                    Password = ftpPassword,
                    //FtpSecure = FtpSecure.Explicit,
                    //uncomment
                    FtpSecure = FtpSecure.None,
                    //comment
                };

                if (ftpProtocol.ToLower() == "ftp")
                {
                    sessionOptions.Protocol = Protocol.Ftp;
                }
                else if (ftpProtocol.ToLower() == "sftp")
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                }

                if (ftpSecure.ToLower() == "yes")
                {
                    // Set the SSH private key file path here

                    sessionOptions.SshPrivateKeyPath = sshPrivateKeyPath;
                    // Set passphrase if your private key is encrypted
                    sessionOptions.SshPrivateKeyPassphrase = sshPrivateKeyPassphrase;
                    // Disable host key checking if needed (not recommended for production)
                    // GiveUpSecurityAndAcceptAnySshHostKey = true,
                    // Set FtpSecure to None, as SFTP is already secured
                    sessionOptions.SshHostKeyFingerprint = sshHostKeyFingerprint;
                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                }
                else
                {
                    sessionOptions.FtpSecure = FtpSecure.None;
                }

                // Connect
                session.Open(sessionOptions);

                int fileCount = 0;

                int i = 0;

                foreach (string path in remoteFilePaths)
                {
                    string remoteFilePath = path;
                    string fileName = Path.GetFileName(remoteFilePath);
                    string containsExtraCondition = extraCondition[i];
                    string otherPartOfFileName = partialName2[i];
                    string dateModifiedCheck = dateModified[i];
                    string containsMultipleFiles = allowMultipleFiles[i];
                    //string fullPathCsv = parentDirectory + @"\Import\ImportConfigs\ImportCsvFiles\";
                    string formattedDate = currentDate.ToString("ddMM");
                    string fullPathCsv = currentDirectory + $@"\Downloaded Files\{brokerName}";

                    if (!Directory.Exists(fullPathCsv))
                    {
                        Directory.CreateDirectory(fullPathCsv);
                    }

                    string localFilePath = fullPathCsv + @"\" + fileName;

                    try
                    {
                        bool isFileDownload = DownloadFileFromFtp(ftpHost, ftpUsername, ftpPassword, remoteFilePath, localFilePath, session, containsExtraCondition, otherPartOfFileName, dateModifiedCheck, containsMultipleFiles, shouldSaveDateModified[i]);
                        if (isFileDownload == true)
                        {
                            fileCount++;
                            //exitCode = 1;
                        }
                        i++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Problem with the file Download for {path}: {ex.Message}");
                        string msg1 = $"Problem with the file Download for {path}: {ex.Message}";
                        CreateLogs(msg1);
                    }
                }

                //if (fileCount >= remoteFilePaths.Count())
                if (fileCount > 0)
                    ftpCount++;
                Console.WriteLine("");
                CreateLogs("", 0);
                CreateLogs("[" + fileCount + "] " + " Files Downloaded", 0);
                Console.WriteLine("[" + fileCount + "] " + " Files Downloaded");
                Console.WriteLine("*****************************************");
                CreateLogs("*****************************************", 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Login attempt Failed.");
            string msg1 = "Login attempt Failed.";
            CreateLogs(msg1);
            exitCode = 2;
            return;
        }
    }

    static bool DownloadFileFromFtp(string ftpHost, string ftpUsername, string ftpPassword, string remoteFilePath, string localFilePath, WinSCP.Session session, string containsExtraCondition, string partialName2, string dateModifiedCheck, string allowMultipleFiles, string shouldSaveDateModified)
    {
        try
        {
            DateTime specificDate = DateTime.Now;
            //23 date file working on same date
            //current date - date in file name

            int lastIndexOfRemoteFilePath = remoteFilePath.LastIndexOf('/');
            string remotePath = remoteFilePath.Substring(0, lastIndexOfRemoteFilePath);
            remotePath = remotePath + "/";
            int lastIndexOfLocalFilePath = localFilePath.LastIndexOf('\\');
            string localPath = localFilePath.Substring(0, lastIndexOfLocalFilePath);
            localPath = localPath + "\\";
            bool isFileDownloaded = false;
            if (containsExtraCondition.Equals("true"))
            {
                RemoteDirectoryInfo directory = session.ListDirectory(remotePath);
                string fileName = Path.GetFileNameWithoutExtension(remoteFilePath);
                // Partial string provided by the user
                string partialString1 = fileName;
                int i = 0;
                partialName2 = ReplaceDateTimePlaceholders(partialName2, "", ref i);
                string partialString2 = partialName2;

                // Dictionary to store mappings between partialString1 and list of RemoteFileInfo objects
                Dictionary<string, List<RemoteFileInfo>> fileInfoMappings = new Dictionary<string, List<RemoteFileInfo>>();

                // Iterate through files in the remote directory
                foreach (RemoteFileInfo file in directory.Files)
                {
                    // Convert filename and partialString1 to lowercase for case-insensitive matching
                    string lowerFileName = file.Name.ToLower();
                    string lowerPartialString1 = partialString1.ToLower();

                    // Check if partialString1 is contained within the filename
                    if (lowerFileName.Contains(lowerPartialString1) && lowerFileName.Contains(partialString2))
                    {
                        if (!fileInfoMappings.ContainsKey(partialString1))
                        {
                            fileInfoMappings[partialString1] = new List<RemoteFileInfo>();
                        }
                        if (allowMultipleFiles == "true" || fileInfoMappings[partialString1].Count == 0)
                        {
                            // Add file to the list for the key
                            fileInfoMappings[partialString1].Add(file);
                            CreateLogs("partialString1 -> " + partialString1 + "----" + file);
                        }
                    }
                }

                if (fileInfoMappings.ContainsKey(partialString1))
                {
                    foreach (RemoteFileInfo temp in fileInfoMappings[partialString1])
                    {
                        string actualFileName = temp.ToString();
                        remoteFilePath = remotePath + actualFileName;
                        CreateLogs("remoteFilePath -> " + remoteFilePath);
                        localFilePath = localPath + actualFileName;

                        RemoteFileInfo fileInfo = session.GetFileInfo(remoteFilePath);

                        try
                        {
                            string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                            // Parse the LastWriteTime string to a DateTime
                            DateTime fileChangedDate = specificDate.Date;

                            // Check the changed date of the file
                            // if (fileInfo.LastWriteTime.Date != specificDate.Date)
                            // {
                            //     // The file was not changed on the specified date, so skip downloading
                            //     Console.WriteLine($"{formattedDateTime}   File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.");
                            //     string errMsg = $"File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.";
                            //     CreateLogs(errMsg);
                            //     return false;
                            // }
                        }
                        catch
                        {
                            string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                            Console.WriteLine($"{formattedDateTime}   File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.");
                            string errMsg = $"File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.";
                            CreateLogs(errMsg);
                            continue;
                        }

                        //if (File.Exists(localFilePath))
                        //{
                        //    DateTime localFileLastModifiedDate = File.GetLastWriteTime(localFilePath);
                        //    string file = Path.GetFileName(localFilePath);
                        //    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                        //    if (fileInfo.LastWriteTime == localFileLastModifiedDate)
                        //    {
                        //        Console.WriteLine($"{formattedDateTime}   File {file} with the same modification date already exists locally. Skipping download.");
                        //        string errMsg = $"File {file} with the same modification date already exists locally. Skipping download.";
                        //        CreateLogs(errMsg);
                        //        continue;
                        //    }
                        //}


                        if (dateModifiedCheck != null)
                        {
                            string lastWriteTime = fileInfo.LastWriteTime.ToString("[yyyyMMdd]");

                            if (!lastWriteTime.Contains(dateModifiedCheck))
                            {
                                Console.WriteLine($"{fileInfo.FullName.ToString()} Last Write Time is not correct.");
                                string errMsg = $"{fileInfo.FullName.ToString()} Last Write Time is not correct.";
                                CreateLogs(errMsg);
                                continue;
                            }
                        }

                        if (shouldSaveDateModified?.ToLower() == "true" && ModifiedFileService.IsSameLastModifiedFileAlreadyDownloaded(localFilePath, fileInfo.LastWriteTime.ToString()))
                        {
                            string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                            Console.WriteLine($"{formattedDateTime} File {fileInfo.Name} has been already downloaded for same date modified. Skipping download.");
                            string errMsg = $"File {fileInfo.Name} has been already downloaded for same date modified. Skipping download.";
                            CreateLogs(errMsg);
                            continue;
                        }

                        // For Old changes we are using FileName, as we can have files with same name using full path
                        if (ProcessedFileService.IsFileProcessed(fileInfo.Name) || ProcessedFileService.IsFileProcessed(localFilePath))
                        {
                            string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                            Console.WriteLine($"{formattedDateTime} File {partialString1} has been processed. Skipping download.");
                            string errMsg = $"File {partialString1} has been processed. Skipping download.";
                            CreateLogs(errMsg);
                            continue;
                        }

                        TransferOptions transferOptions = new TransferOptions();
                        // Download file
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult = session.GetFiles(remoteFilePath, localFilePath, false, transferOptions);

                        // Check for errors
                        transferResult.Check();

                        // Print results
                        foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                            string fileSizeMessage = $"File Size: {transfer.Length / 1024 + 1} KB";
                            string transferFileName = Path.GetFileName(transfer.FileName);

                            Console.WriteLine(formattedDateTime + "   Download of " + transferFileName + " Succeeded!!    " + fileSizeMessage);
                            string msg = transferFileName + " Downloaded Successfully !!     " + fileSizeMessage;
                            CreateLogs(msg);
                            isFileDownloaded = true;
                            //exitCode = 1;
                        }
                    }
                    return isFileDownloaded;
                }
            }
            else
            {
                try
                {
                    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                    // Parse the LastWriteTime string to a DateTime
                    DateTime fileChangedDate = specificDate.Date;

                    //DateTime a = fileInfo.LastWriteTime.Date;

                    //while(fileInfo.LastWriteTime.Date != specificDate.Date)
                    //{
                    //    specificDate = specificDate.AddDays(-1);
                    //}

                    // Check the changed date of the file
                    //if (fileInfo.LastWriteTime.Date != specificDate.Date)
                    //{
                    //    // The file was not changed on the specified date, so skip downloading
                    //    Console.WriteLine($"{formattedDateTime}   File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.");
                    //    string errMsg = $"File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.";
                    //    CreateLogs(errMsg);
                    //    return false;
                    //}
                }
                catch
                {
                    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                    Console.WriteLine($"{formattedDateTime}   File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.");
                    string errMsg = $"File {Path.GetFileName(remoteFilePath)} was not modified on {specificDate:MM/dd/yyyy}. Skipping download.";
                    CreateLogs(errMsg);
                }

                RemoteFileInfo fileInfo = session.GetFileInfo(remoteFilePath);

                //if (File.Exists(localFilePath))
                //{
                //    DateTime localFileLastModifiedDate = File.GetLastWriteTime(localFilePath);
                //    string file = Path.GetFileName(localFilePath);
                //    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                //    if (fileInfo.LastWriteTime == localFileLastModifiedDate)
                //    {
                //        Console.WriteLine($"{formattedDateTime}   File {file} with the same modification date already exists locally. Skipping download.");
                //        string errMsg = $"File {file} with the same modification date already exists locally. Skipping download.";
                //        CreateLogs(errMsg);exitcode
                //        ftpCount++;
                //        return false;
                //    }
                //}

                if (dateModifiedCheck != null)
                {
                    string lastWriteTime = fileInfo.LastWriteTime.ToString("[yyyyMMdd]");

                    if (!lastWriteTime.Contains(dateModifiedCheck))
                    {
                        Console.WriteLine($"{fileInfo.FullName.ToString()} Last Write Time is not correct.");
                        string errMsg = $"{fileInfo.FullName.ToString()} Last Write Time is not correct.";
                        CreateLogs(errMsg);
                        return false;
                    }
                }

                if (shouldSaveDateModified?.ToLower() == "true" && ModifiedFileService.IsSameLastModifiedFileAlreadyDownloaded(localFilePath, fileInfo.LastWriteTime.ToString()))
                {
                    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                    Console.WriteLine($"{formattedDateTime} File {fileInfo.Name} has been already downloaded for same date modified. Skipping download.");
                    string errMsg = $"File {fileInfo.Name} has been already downloaded for same date modified. Skipping download.";
                    CreateLogs(errMsg);
                    return false;
                }

                // For Old changes we are using FileName, as we can have files with same name using full path
                if (ProcessedFileService.IsFileProcessed(fileInfo.Name) || ProcessedFileService.IsFileProcessed(localFilePath))
                {
                    string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");

                    Console.WriteLine($"{formattedDateTime} File {fileInfo.Name} has been processed. Skipping download.");
                    string errMsg = $"File {fileInfo.Name} has been processed. Skipping download.";
                    CreateLogs(errMsg);
                    return false;
                }
                else
                {
                    TransferOptions transferOptions = new TransferOptions();
                    // Download file
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult = session.GetFiles(remoteFilePath, localFilePath, false, transferOptions);

                    // Check for errors
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        string formattedDateTime = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
                        string fileSizeMessage = $"File Size: {transfer.Length / 1024 + 1} KB";
                        string fileName = Path.GetFileName(transfer.FileName);

                        if (shouldSaveDateModified?.ToLower() == "true") { ModifiedFileService.UpdateLastModifiedDateInFile(localFilePath, fileInfo.LastWriteTime.ToString()); }

                        Console.WriteLine(formattedDateTime + "   Download of " + fileName + " Succeeded!!    " + fileSizeMessage);
                        string msg = fileName + " Downloaded Successfully !!     " + fileSizeMessage;
                        CreateLogs(msg);
                        isFileDownloaded = true;
                    }
                }
                return isFileDownloaded;
            }
        }
        catch
        {
            Console.WriteLine($"File : {remoteFilePath} not Found on the FTP");
            string msg = $"File : {remoteFilePath} not Found on the FTP";
            CreateLogs(msg);
            return false;
        }
        return false;
    }

    public static string[] GetRemoteFilePaths(string remoteFile)
    {
        string[] paths = null;
        try
        {
            string[] allLines = File.ReadAllLines(remoteFile);
            // Assuming the first line contains headers, find the index of the "remoteFilePath" column
            int columnIndex = Array.IndexOf(allLines[0].Split(','), "remoteFilePath");

            if (columnIndex == -1)
            {
                Console.WriteLine("Column 'remoteFilePath' not found in the CSV file.");
                string msg = "Column 'remoteFilePath' not found in the CSV file.";
                CreateLogs(msg);
                return null;
            }

            // Extract the paths from the "remoteFilePath" column and store them in a string array
            paths = allLines.Skip(1) // Skip the header line
                                     .Select(line => line.Split(',')[columnIndex])
                                     .ToArray();
        }
        catch
        {
            Console.WriteLine("Please Close the file " + remoteFile);
            string msg = "Please Close the file " + remoteFile;
            CreateLogs(msg);
        }

        return paths;
    }

    public static DataTable DataFromCsvFile(string csvFile)
    {
        DataTable dataTable = new DataTable();
        try
        {
            using (TextFieldParser parser = new TextFieldParser(csvFile))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                if (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    foreach (string field in fields)
                    {
                        dataTable.Columns.Add(field);
                    }

                    while (!parser.EndOfData)
                    {
                        string[] data = parser.ReadFields();
                        DataRow row = dataTable.NewRow();

                        for (int i = 0; i < data.Length; i++)
                        {
                            row[i] = data[i];
                        }

                        dataTable.Rows.Add(row);
                    }
                }
            }
        }
        catch
        {
            Console.WriteLine("Please Close the file " + csvFile);
            string msg = "Please Close the file " + csvFile;
            CreateLogs(msg);
        }

        return dataTable;
    }

    public static string GetValueFromMainFile(DataTable dt, string Name)
    {
        foreach (DataRow row in dt.Rows)
        {
            string data = row["Name"].ToString();
            if (data == Name)
            {
                return row["Value"].ToString();
            }
        }
        return null;
    }

    public static void CreateLogs(string logMessage, int dateCheck = 1)
    {
        try
        {
            if (File.Exists(logFilePath))
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
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
                }
            }
            else
            {
                using (StreamWriter writer = File.CreateText(logFilePath))
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
                }
            }
        }
        catch { }
    }

    // Method to verify if a single filename exists in the text file
    public static bool FileNameExists(string filePath, string fileName)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exist.");
        }

        // Read all lines and check for the filename
        var lines = File.ReadAllLines(filePath);
        return lines.Contains(fileName, StringComparer.OrdinalIgnoreCase);
    }
}