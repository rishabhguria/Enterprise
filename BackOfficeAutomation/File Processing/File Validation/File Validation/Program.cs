using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Data;
using System.Security.Cryptography;
using ExcelDataReader;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using File_Validation.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FileProcessingUtility
{
    public enum CheckType
    {
        ColumnCount,
        BlankFilesSummary,
        DuplicateCheck,
        ColumnNameExists,
        CountColumnValueOccurrences,
        InitializeCountFile,
        CompareAndLog,
        CompareHeaderAndLog,
        CheckForSpecialCharacters,
        CountProcessDateRecord,
        CountNumberofAccounts
    }

    public static class GlobalState
    {
        // Declare the global exit code variable
        public static int ExitCode { get; set; } = 0; // Default to success
    }

    public class DataProcessor
    {
        private readonly object _consoleLock = new object();
        private static readonly object logFileLock = new object();
        private readonly object _consoleLockHeader = new object();
        private static readonly object logFileLockHeader = new object();

        public (List<Dictionary<string, string>> duplicates, string logMessages) CheckDuplicateRows(List<Dictionary<string, string>> records)
        {
            var duplicates = new List<Dictionary<string, string>>();
            var seen = new HashSet<string>();
            var logMessages = new StringBuilder();
            int blankRowCount = 0;  // Track the number of blank rows

            try
            {
                foreach (var row in records)
                {
                    var rowKeyBuilder = new System.Text.StringBuilder();
                    bool isRowEmpty = true;  // Variable to check if the row is blank

                    foreach (var value in row.Values)
                    {
                        // Treat a cell as blank if:
                        // 1. It's null
                        // 2. It's a string and is empty or whitespace
                        // 3. It's not a string (like numbers, booleans) but should still be considered "blank" for your case

                        bool isCellBlank = value == null || (value is string str && string.IsNullOrWhiteSpace(str));

                        // Mark the row as non-empty if any value is not blank
                        if (!isCellBlank)
                        {
                            isRowEmpty = false;
                        }

                        // For the key, use the actual value or empty string if it's blank
                        rowKeyBuilder.Append(value?.ToString() ?? string.Empty).Append(",");
                    }

                    // Skip processing this row if it's entirely blank
                    if (isRowEmpty)
                    {
                        blankRowCount++;
                        continue;
                    }

                    var rowKey = rowKeyBuilder.ToString().TrimEnd(',');

                    // Check if this row is a duplicate
                    if (!seen.Add(rowKey))
                    {
                        duplicates.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                // Append the error details to the logMessages
                logMessages.AppendLine("An error occurred while checking duplicate rows:");
                logMessages.AppendLine($"Error Message: {ex.Message}");
                logMessages.AppendLine($"Stack Trace: {ex.StackTrace}");
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error Message: {ex.Message}");
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            logMessages.AppendLine($"Total Records: {records.Count}");
            logMessages.AppendLine($"Blank Rows Ignored: {blankRowCount}");
            logMessages.AppendLine($"Duplicate Records: {duplicates.Count}");

            return (duplicates, logMessages.ToString());
        }

        public string CheckColumnCount(int? headerCount, string checkDetails)
        {
            var logMessages = new System.Text.StringBuilder();
            try
            {
                if (!int.TryParse(checkDetails, out var expectedColumnCount))
                {
                    logMessages.AppendLine("Invalid expected column count in the config.");
                    return logMessages.ToString();
                }

                if (headerCount == null)
                {
                    logMessages.AppendLine($"File is empty without Header row, thus it does not have the expected column count of {expectedColumnCount}.");
                }
                if (headerCount != expectedColumnCount)
                {
                    logMessages.AppendLine($"Invalid column count in the header row. Expected {expectedColumnCount}, but found {headerCount}.");
                }
                else
                {
                    logMessages.AppendLine($"Header row has the expected column count of {expectedColumnCount}.");
                }
            }
            catch (Exception ex)
            {
                // Append the error details to the logMessages
                logMessages.AppendLine("An error occurred while checking duplicate rows:");
                logMessages.AppendLine($"Error Message: {ex.Message}");
                logMessages.AppendLine($"Stack Trace: {ex.StackTrace}");
                GlobalState.ExitCode = -1;
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error Message: {ex.Message}");
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
            }

            return logMessages.ToString();
        }

        public string ComputeHash(List<Dictionary<string, string>> records)
        {
            try
            {
                // Combine rows into a single string
                string combinedData = string.Join(Environment.NewLine, records.Select(r => string.Join(",", r.Values)));

                // Hash the combined data
                using (var sha256 = SHA256.Create())
                {
                    var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combinedData));
                    return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                // Append the error details to the logMessages

                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error computing hash : {ex.Message}");
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                return null;
            }
        }

        // Method to compute file hash
        public string ComputeFileHash(string filePath)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        byte[] hashBytes = sha256.ComputeHash(stream);
                        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    }
                }
            }
            catch (Exception ex)
            {
                // Append the error details to the logMessages

                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error computing hash for file {filePath}: {ex.Message}");
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                return null;
            }
        }

        // Method to read the last hash for a specific file from the log file
        public string ReadLastHashForFileFromLog(string logFilePath, string filePath, string fileName, object logFileLock)
        {
            try
            {
                // Lock to ensure thread-safe access to the log file
                lock (logFileLock)
                {
                    if (File.Exists(logFilePath))
                    {
                        string json = File.ReadAllText(logFilePath);
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            JObject jsonObject = JObject.Parse(json);
                            return jsonObject[fileName]?["Hash"]?.ToString();
                        }
                    }
                    return null; // Return null if no entry for the specified file is found
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error reading log file {logFilePath} for file {filePath}: {ex.Message}";

                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                return null;
            }
        }

        // Method to replace the hash for a specific file in the log file
        public void ReplaceFileHashInLog(string newHash, string logFilePath, string filePath, string fileName, object logFileLock)
        {
            try
            {
                // Lock to ensure thread-safe access to the log file
                lock (logFileLock)
                {
                    JObject jsonObject;
                    if (File.Exists(logFilePath))
                    {
                        string json = File.ReadAllText(logFilePath);
                        jsonObject = !string.IsNullOrWhiteSpace(json) ? JObject.Parse(json) : new JObject();
                    }
                    else
                    {
                        jsonObject = new JObject();
                        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
                    }

                    // Update or add the entry
                    if (jsonObject[fileName] == null)
                    {
                        jsonObject[fileName] = new JObject();
                    }
                    jsonObject[fileName]["Hash"] = newHash;
                    jsonObject[fileName]["Timestamp"] = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

                    // Write back to the file
                    File.WriteAllText(logFilePath, jsonObject.ToString(Newtonsoft.Json.Formatting.Indented));
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error replacing hash in log file {logFilePath} for file {filePath}: {ex.Message}";

                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }
        }

        public string CheckForSpecialCharacters(string filePath, Logger _logger)
        {
            var logMessages = new System.Text.StringBuilder();

            try
            {
                // Define a valid character set (alphanumeric, punctuation, spaces, etc.)
                // ASCII range: 32 (space) to 126 (~) are common printable characters
                HashSet<char> validChars = new HashSet<char>(
                   Enumerable.Range(32, 95).Select(i => (char)i)
               );

                // Open the file with UTF-8 encoding and a fallback for invalid characters
                var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);
                var lines = File.ReadAllLines(filePath, encoding);
                var isCorrupted = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    // Check each character in the line
                    foreach (char c in line)
                    {
                        // Log any character not in the valid character set
                        if (!validChars.Contains(c))
                        {
                            logMessages.AppendLine(
                                $"There is Potential issue of File Corruption."
                            );
                            isCorrupted = true;
                            break;
                        }
                        if (isCorrupted) break;
                    }
                }

                // If no special characters were found, add a success message
                if (logMessages.Length == 0)
                {
                    logMessages.AppendLine("No special characters found. File seems fine.");
                }
            }
            catch (DecoderFallbackException ex)
            {
                string errorMessage = $"Encoding issue: {ex.Message}";
                logMessages.AppendLine($"Encoding issue: {ex.Message}");
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error reading file: {ex.Message}";
                logMessages.AppendLine($"Error reading file: {ex.Message}");
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                // Log any other exceptions
            }

            return logMessages.ToString();
        }

        public string CompareHeaderAndLog(string filePath, string checkDetails, string splitParameter, ClientPartialFileNamesConfig partialFileNamesConfig)
        {
            var logMessages = new StringBuilder();
            char ch = Convert.ToChar(splitParameter);
            string[] arr = checkDetails.Split(ch);
            string logFilePath = arr[0];
            int headerRowIndex = int.Parse(arr[1]);

            try
            {
                string fileName = GetCleanedFileName(filePath, partialFileNamesConfig);

                // Read the previous day's header row hash from the log file
                string previousHash = ReadLastHashForFileFromLog(logFilePath, filePath, fileName, logFileLockHeader);

                if (previousHash == null)
                    logMessages.AppendLine("No previous header row hash found in the log file.");
                else
                    logMessages.AppendLine($"Previous header row hash: {previousHash}");

                // Compute the current header row's hash
                string currentHeaderRowHash = ComputeHeaderRowHash(filePath, headerRowIndex);

                logMessages.Append($"{DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]")}: {fileName} - Current Hash: {currentHeaderRowHash}");

                // Compare the hashes
                bool match = false;
                string resultMessage = string.Empty;
                if (previousHash != null)
                {
                    match = currentHeaderRowHash == previousHash;
                    resultMessage = match ? "The header rows match." : "The header rows do not match.";

                    // Print the result to the console along with the file name
                    logMessages.AppendLine($"{DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]")}: {fileName} - {resultMessage}");
                }

                // Replace the last hash in the log with the current header row's hash
                ReplaceFileHashInLog(currentHeaderRowHash, logFilePath, filePath, fileName, logFileLockHeader);

                return logMessages.ToString();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error: {ex.Message}";
                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                return logMessages.ToString();
            }
        }

        // Method to compute the hash of a specific row (header row in this case)
        public string ComputeHeaderRowHash(string filePath, int rowIndex)
        {
            try
            {
                string fileExtension = Path.GetExtension(filePath).ToLower();
                string headerRow = string.Empty;
                if (fileExtension == ".xlsx")
                {
                    headerRow = new ExcelFileReader().ReadHeaderRowForXLSX(filePath, rowIndex);
                }
                else if (fileExtension == ".xls")
                {
                    headerRow = new ExcelFileReader().ReadHeaderRowForXLS(filePath, rowIndex);
                }
                else if (fileExtension == ".txt" || fileExtension == ".csv")
                {
                    headerRow = new CsvFileReader().ReadHeaderRowForCSV(filePath, rowIndex);
                }
                else
                {
                    throw new NotSupportedException($"File extension {fileExtension} is not supported.");
                }

                // Generate a hash for the row
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] rowBytes = Encoding.UTF8.GetBytes(headerRow);
                    byte[] hashBytes = sha256.ComputeHash(rowBytes);

                    // Convert hash to a readable format (hex string)
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error computing header row hash.: {ex.Message}";

                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                return null;
            }
        }

        public string CheckBlankFilesSummary(List<Dictionary<string, string>> records)
        {
            var logMessages = new StringBuilder();
            try
            {
                if (!records.Any())
                {
                    logMessages.AppendLine("The file is blank.");
                }
                else
                {
                    logMessages.AppendLine("The file is not blank.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error : {ex.Message}";

                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            return logMessages.ToString();
        }

        public string CheckColumnNameExists(string[] headers, string columnName)
        {
            var logMessages = new System.Text.StringBuilder();
            try
            {
                if (headers.Contains(columnName))
                {
                    logMessages.AppendLine($"The column '{columnName}' exists in the header row.");
                }
                else
                {
                    logMessages.AppendLine($"The column '{columnName}' does not exist in the header row.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error : {ex.Message}";

                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            return logMessages.ToString();
        }

        public string CountColumnValueOccurrences(List<Dictionary<string, string>> records, string checkDetails, string splitParameter)
        {
            var logMessages = new System.Text.StringBuilder();
            try
            {
                // Replace placeholders in checkDetails
                var valueToCount = checkDetails;

                // Extract column name and value to search
                // Extract column name and value to search
                var parts = valueToCount.Split(new string[] { splitParameter }, StringSplitOptions.None);
                if (parts.Length != 2)
                {
                    logMessages.AppendLine("Invalid checkDetails format. Expected format: '<value>{splitParameter}<columnName>'");
                    return logMessages.ToString();
                }

                var valueToSearch = parts[0];
                var columnName = parts[1];

                // Count occurrences
                int count = records.Count(record =>
                {
                    if (record.TryGetValue(columnName, out var cellValue))
                    {
                        return cellValue == valueToSearch;
                    }
                    return false;
                });

                logMessages.AppendLine($"Count of value '{valueToSearch}' in column '{columnName}': {count}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error : {ex.Message}";

                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            return logMessages.ToString();
        }

        public string InitializeCountFile(string filePath, int count, Logger _logger)
        {
            var logMessages = new System.Text.StringBuilder();
            try
            {
                if (_logger.IsFirstLogOfTheDay())
                {
                    File.WriteAllText(filePath, count.ToString());
                    logMessages.AppendLine($"CountFiles.txt initialized with value: {count}");
                }
                else
                {
                    logMessages.AppendLine("CountFiles.txt was not initialized as it's not the first log of the day.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error : {ex.Message}";

                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }
            return logMessages.ToString();
        }

        // TO DO Checkdetails need to be handled
        public string CompareFileHashAndLog(string filePath, string checkDetails, string splitParameter, ClientPartialFileNamesConfig partialFileNamesConfig, List<Dictionary<string, string>> records = null)
        {
            var logMessages = new StringBuilder();
            char ch = Convert.ToChar(splitParameter);
            string[] arr = checkDetails.Split(ch);
            string logFilePath = arr[0];

            try
            {
                string fileName = GetCleanedFileName(filePath, partialFileNamesConfig);

                // Read the previous day's hash from the log file
                string previousHash = ReadLastHashForFileFromLog(logFilePath, filePath, fileName, logFileLock);

                if (previousHash == null)
                    logMessages.AppendLine("No previous hash found in the log file.");
                else
                    logMessages.AppendLine($"Previous file's hash: {previousHash}");

                // Compute the current hash based on the filePath or records
                string currentHash;

                if (records != null)
                    currentHash = ComputeHash(records);
                else
                    currentHash = ComputeFileHash(filePath);

                logMessages.Append($"{DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]")}: {fileName} - Current Hash: {currentHash}");

                // Compare the hashes
                string resultMessage = string.Empty;
                if (previousHash != null)
                {
                    resultMessage = currentHash == previousHash ? "The files match." : "The files do not match.";
                    // Print the result to the log along with the file name
                    logMessages.AppendLine($"{DateTime.Now: [yy-MM-dd HH:mm:ss]}: {fileName} - {resultMessage}");
                }

                // Replace the last hash in the log with the current hash
                ReplaceFileHashInLog(currentHash, logFilePath, filePath, fileName, logFileLock);
                return logMessages.ToString();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error: {ex.Message}";
                logMessages.AppendLine($"Error: {ex.Message}");
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
                return logMessages.ToString();
            }
        }

        public string CountNumberofAccounts(List<Dictionary<string, string>> records, string checkDetails, string splitParameter, Logger _logger)
        {
            var logMessages = new StringBuilder();
            try
            {
                // Extract column number and expected account count from checkDetails
                var valueToCount = checkDetails;
                var parts = valueToCount.Split(new string[] { splitParameter }, StringSplitOptions.None);

                int columnNumber;
                if (!int.TryParse(parts[0], out columnNumber) || columnNumber < 1)
                {
                    logMessages.AppendLine("Invalid column number. It must be a positive integer.");
                    return logMessages.ToString();
                }
                int startRow;
                if (!int.TryParse(parts[1], out startRow) || startRow < 1)
                {
                    logMessages.AppendLine("Invalid start row. It must be a positive integer.");
                    return logMessages.ToString();
                }
                int expectedAccountCount = -1;
                if (parts.Length == 3 && !int.TryParse(parts[2], out expectedAccountCount))
                {
                    logMessages.AppendLine("Invalid expected account count. It must be an integer.");
                    return logMessages.ToString();
                }

                // Validate records
                if (records == null || !records.Any())
                {
                    logMessages.AppendLine("Records list is empty or null.");
                    return logMessages.ToString();
                }

                if (startRow < 1 || startRow > records.Count)
                {
                    logMessages.AppendLine("Invalid start row. It must be within the range of the records.");
                    return logMessages.ToString();
                }

                // Get unique accounts based on column number starting from the specified row, ignoring null or blank cell values
                var uniqueAccounts = records.Skip(startRow - 1).Select(record =>
                {
                    var columnName = record.Keys.ElementAtOrDefault(columnNumber - 1); // Adjust for zero-based index
                    return columnName != null && record.TryGetValue(columnName, out var accountValue) && !string.IsNullOrWhiteSpace(accountValue) ? accountValue : null;
                }).Where(value => !string.IsNullOrEmpty(value)).Distinct().Count();

                if (uniqueAccounts == expectedAccountCount)
                {
                    lock (_consoleLock)
                    {
                        Console.WriteLine($"The number of unique accounts in column number '{columnNumber}' starting from row '{startRow}' matches the expected count: {expectedAccountCount}.");
                    }
                    _logger.Log($"The number of unique accounts in column number '{columnNumber}' starting from row '{startRow}' matches the expected count: {expectedAccountCount}.");
                }
                else if (expectedAccountCount == -1)
                {
                    lock (_consoleLock)
                    {
                        Console.WriteLine($"The number of unique accounts in column number '{columnNumber}' starting from row '{startRow}' is {uniqueAccounts}");
                    }
                    _logger.Log($"The number of unique accounts in column number '{columnNumber}' starting from row '{startRow}' is {uniqueAccounts}");
                }
                else
                {
                    logMessages.AppendLine($"The number of unique accounts in column number '{columnNumber}' starting from row '{startRow}' is {uniqueAccounts}, which does not match the expected count: {expectedAccountCount}.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error : {ex.Message}";

                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            return logMessages.ToString();
        }

        public string CountProcessDateRecord(List<Dictionary<string, string>> records, string checkDetails, string splitParameter, Logger _logger)
        {
            var logMessages = new System.Text.StringBuilder();
            try
            {
                // Replace placeholders in checkDetails
                var valueToCount = checkDetails;

                // Extract process date value and column number
                var parts = valueToCount.Split(new string[] { splitParameter }, StringSplitOptions.None);

                var processDateValue = parts[0];
                string columnName = parts[1];

                int startRow;
                if (!int.TryParse(parts[2], out startRow) || startRow < 1)
                {
                    logMessages.AppendLine("Invalid column number. It must be a positive integer.");
                    return logMessages.ToString();
                }
                // Validate records
                if (records == null || !records.Any())
                {
                    logMessages.AppendLine("Records list is empty or null.");
                    return logMessages.ToString();
                }

                if (startRow < 1 || startRow > records.Count)
                {
                    logMessages.AppendLine("Invalid start row. It must be within the range of the records.");
                    return logMessages.ToString();
                }

                bool isDateFormatPrinted = false;
                int count = 0;

                int endRow = (parts.Length == 4 && int.TryParse(parts[3], out int parsedEndRow)) ? parsedEndRow : 0;
                int rowCount = (endRow >= startRow && endRow > 0) ? (endRow - startRow + 1) : records.Count - (startRow - 1);

                foreach (var record in records.Skip(startRow - 1).Take(rowCount))
                {
                    // Normalize column names (remove quotes and trim)
                    var normalizedColumnName = columnName.Trim('"').Trim();
                    // var normalizedKeys = record.Keys.Select(k => k.Trim('"').Trim().Replace("\\", ""));

                    // Attempt exact or fuzzy match
                    var matchingKey = record.Keys.FirstOrDefault(k => k.Trim('"').Trim().Replace("\\", "")
                                      .Equals(normalizedColumnName, StringComparison.OrdinalIgnoreCase));

                    if (matchingKey != null)
                    {
                        // Use TryGetValue to safely access the value for matchingKey
                        if (record.TryGetValue(matchingKey, out var cellValue))
                        {
                            if (!isDateFormatPrinted)
                            {
                                _logger.Log($"Debug: Found match for column '{columnName}' (actual key: '{matchingKey}'). Value: '{cellValue}'.");
                            }

                            // Count matching rows
                            if (!string.IsNullOrWhiteSpace(cellValue) &&
                                cellValue.Trim().Contains(processDateValue, StringComparison.OrdinalIgnoreCase))
                            {
                                count++;
                            }
                            isDateFormatPrinted = true;
                        }
                        else
                        {
                            logMessages.AppendLine($"Debug: Key '{matchingKey}' found, but value could not be retrieved.");
                        }
                    }
                }

                if (count == 0)
                {
                    logMessages.AppendLine($"There are 0 rows of process date '{processDateValue}' in the file starting from row '{startRow}'.");
                }
                else
                {
                    lock (_consoleLock)
                    {
                        Console.WriteLine($"Count of process date '{processDateValue}' in column  '{columnName}' starting from row '{startRow}': {count}.");
                    }
                    _logger.Log($"Count of process date '{processDateValue}' in column  '{columnName}' starting from row '{startRow}': {count}.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error : {ex.Message}";

                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            return logMessages.ToString();
        }

        #region Private Methods

        private string GetCleanedFileName(string filepath, ClientPartialFileNamesConfig partialFileNamesConfig)
        {
            var filename = Path.GetFileName(filepath);
            var partialFileName = GetPartialFileName(filepath, filename, partialFileNamesConfig);

            string extension = Path.GetExtension(filename);
            string baseFilename = Path.GetFileNameWithoutExtension(filename);
            string cleanedBaseFilename;
            string pattern = @"(\d)|(?<![a-zA-Z])[._-]|[._-](?![a-zA-Z])"; // Pattern to match digits and [,_-] between non-alphabet characters

            if (!string.IsNullOrEmpty(partialFileName))
            {
                string replacement = "######";
                // Step 1: Preserve the partialName by removing it temporarily
                string tempFilename = baseFilename.Replace(partialFileName, $"{{{replacement}}}");

                // Step 2: Remove the matches from the base filename
                tempFilename = Regex.Replace(tempFilename, pattern, string.Empty);

                // Step 3: Restore the partial file name in the cleaned file name
                cleanedBaseFilename = tempFilename.Replace($"{{{replacement}}}", partialFileName);
            }
            else
            {
                // Remove the matches from the base filename
                cleanedBaseFilename = Regex.Replace(baseFilename, pattern, string.Empty);
            }

            // Add back the extension to base filename
            string cleanedFilename = $"{cleanedBaseFilename}{extension}";

            return cleanedFilename;
        }

        private string GetPartialFileName(string filepath, string filename, ClientPartialFileNamesConfig partialFileNamesConfig)
        {
            if (partialFileNamesConfig.ClientPartialFileNames != null && partialFileNamesConfig.ClientPartialFileNames.Any())
            {
                var clientNames = partialFileNamesConfig.ClientPartialFileNames.Keys.ToArray();
                foreach (var clientName in clientNames)
                {
                    if (filepath.Contains(clientName))
                    {
                        // Found a match for the client name in the filename
                        var partialFileNames = partialFileNamesConfig.ClientPartialFileNames[clientName];
                        foreach (var partialFileName in partialFileNames)
                        {
                            if (filename.Contains(partialFileName))
                            {
                                return partialFileName;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        #endregion Private Methods
    }

    public class FileProcessor
    {
        private readonly Logger _logger;
        private readonly DataProcessor _dataProcessor;
        private readonly object _consoleLock = new object();
        private static readonly object logFileLock = new object();

        public FileProcessor(Logger logger, DataProcessor dataProcessor)
        {
            _logger = logger;
            _dataProcessor = dataProcessor;
        }

        public void ProcessFile(string filePath, int? headerRow, CheckType checkType, string checkDetails, string splitParameter, ClientPartialFileNamesConfig partialFileNamesConfig)
        {
            try
            {
                var fileExtension = Path.GetExtension(filePath).ToLower();

                var logMessages = new StringBuilder();
                var consoleLogMessages = new StringBuilder();
                int? headerCount = 0;
                bool hasIssues = false;
                string[] headers = null;
                List<Dictionary<string, string>> records = new List<Dictionary<string, string>>();
                var readLogMessages = string.Empty;

                switch (fileExtension?.ToLower())
                {
                    case ".csv":
                    case ".txt":
                        (records, headerCount, readLogMessages, headers) = new CsvFileReader().ReadCsvFile(filePath, headerRow);
                        logMessages.AppendLine(readLogMessages);
                        consoleLogMessages.AppendLine(readLogMessages);
                        break;

                    case ".xlsx":
                    case ".xls":
                        (records, headerCount, readLogMessages, headers) = new ExcelFileReader().ReadExcelFile(filePath, headerRow);
                        logMessages.AppendLine(readLogMessages);
                        consoleLogMessages.AppendLine(readLogMessages);
                        break;
                    case ".pdf":
                        if (checkType != CheckType.CompareAndLog) return;
                        break;

                    default:
                        throw new NotSupportedException($"File extension {fileExtension} is not supported.");
                }

                switch (checkType)
                {
                    case CheckType.DuplicateCheck:
                        var (duplicates, duplicateLogMessages) = _dataProcessor.CheckDuplicateRows(records);
                        logMessages.AppendLine(duplicateLogMessages);
                        logMessages.AppendLine($"Found {duplicates.Count} duplicate rows in {filePath}");
                        if (duplicates.Any())
                        {
                            consoleLogMessages.AppendLine(duplicateLogMessages);
                            consoleLogMessages.AppendLine($"Found {duplicates.Count} duplicate rows in {filePath}");
                            hasIssues = true;
                        }
                        break;

                    case CheckType.ColumnCount:
                        var columnCountLogMessages = _dataProcessor.CheckColumnCount(headerCount, checkDetails);
                        logMessages.AppendLine(columnCountLogMessages);
                        if (columnCountLogMessages.Contains("Invalid"))
                        {
                            consoleLogMessages.AppendLine(columnCountLogMessages);
                            hasIssues = true;
                        }

                        break;

                    case CheckType.BlankFilesSummary:
                        var blankFilesSummaryLogMessages = _dataProcessor.CheckBlankFilesSummary(records);
                        logMessages.AppendLine(blankFilesSummaryLogMessages);
                        if (blankFilesSummaryLogMessages.Contains("The file is blank"))
                        {
                            consoleLogMessages.AppendLine(blankFilesSummaryLogMessages);
                            hasIssues = true;
                        }
                        break;

                    case CheckType.ColumnNameExists:
                        var columnNameExistsLogMessages = _dataProcessor.CheckColumnNameExists(headers, checkDetails);
                        logMessages.AppendLine(columnNameExistsLogMessages);
                        if (columnNameExistsLogMessages.Contains("does not exist"))
                        {
                            consoleLogMessages.AppendLine(columnNameExistsLogMessages);
                            hasIssues = true;
                        }
                        break;

                    case CheckType.CountColumnValueOccurrences:
                        var countDatesLogMessages = _dataProcessor.CountColumnValueOccurrences(records, checkDetails, splitParameter);
                        logMessages.AppendLine(countDatesLogMessages);
                        consoleLogMessages.AppendLine(countDatesLogMessages);
                        hasIssues = true;
                        break;

                    case CheckType.InitializeCountFile:
                        int count = int.Parse(checkDetails);
                        var initializeCountFileLogMessages = _dataProcessor.InitializeCountFile(filePath, count, _logger);
                        logMessages.AppendLine(initializeCountFileLogMessages);
                        consoleLogMessages.AppendLine(initializeCountFileLogMessages);
                        hasIssues = true;
                        break;

                    case CheckType.CompareAndLog:
                        string hashLogMessages = string.Empty;

                        if (fileExtension.ToLower() == ".pdf")
                            hashLogMessages = _dataProcessor.CompareFileHashAndLog(filePath, checkDetails, splitParameter, partialFileNamesConfig);
                        else if (records == null || !records.Any())
                            hashLogMessages = "File is empty, no need to match file Hash";
                        else
                            hashLogMessages = _dataProcessor.CompareFileHashAndLog(filePath, checkDetails, splitParameter, partialFileNamesConfig, records);

                        logMessages.AppendLine(hashLogMessages);

                        if (hashLogMessages.Contains("The files match"))
                        {
                            consoleLogMessages.AppendLine(hashLogMessages);
                            hasIssues = true;
                        }
                        break;

                    case CheckType.CompareHeaderAndLog:
                        var headerLogMessages = _dataProcessor.CompareHeaderAndLog(filePath, checkDetails, splitParameter, partialFileNamesConfig);
                        logMessages.AppendLine(headerLogMessages);
                        if (headerLogMessages.Contains("The header rows do not match"))
                        {
                            consoleLogMessages.AppendLine(headerLogMessages);
                            hasIssues = true;
                        }
                        break;

                    case CheckType.CountProcessDateRecord:
                        var CountProcessDateRecordLogMessages = _dataProcessor.CountProcessDateRecord(records, checkDetails, splitParameter, _logger);
                        if (!string.IsNullOrEmpty(CountProcessDateRecordLogMessages))
                        {
                            logMessages.AppendLine(CountProcessDateRecordLogMessages);
                            consoleLogMessages.AppendLine(CountProcessDateRecordLogMessages);
                            hasIssues = true;
                        }

                        break;

                    case CheckType.CountNumberofAccounts:
                        var CountAccountsLogMessages = _dataProcessor.CountNumberofAccounts(records, checkDetails, splitParameter, _logger);
                        // TO DO Implement better approach
                        if (!string.IsNullOrEmpty(CountAccountsLogMessages))
                        {
                            logMessages.AppendLine(CountAccountsLogMessages);
                            consoleLogMessages.AppendLine(CountAccountsLogMessages);
                            hasIssues = true;
                        }

                        break;

                    case CheckType.CheckForSpecialCharacters:
                        var SpecialCharactersLogMessages = _dataProcessor.CheckForSpecialCharacters(filePath, _logger);
                        logMessages.AppendLine(SpecialCharactersLogMessages);
                        if (SpecialCharactersLogMessages.Contains("There is Potential issue of File Corruption"))
                        {
                            consoleLogMessages.AppendLine(SpecialCharactersLogMessages);
                            hasIssues = true;
                        }
                        break;

                        //case CheckType.CheckForSpecialCharacters:
                        //    var SpecialCharactersLogMessages = _dataProcessor.CheckForSpecialCharacters(filePath, _logger);
                        //    logMessages.AppendLine(SpecialCharactersLogMessages);
                        //    if (SpecialCharactersLogMessages.Contains("The header rows do not match"))
                        //    {
                        //        consoleLogMessages.AppendLine(SpecialCharactersLogMessages);
                        //        hasIssues = true;
                        //    }
                        //    break;
                }

                if (hasIssues)
                {
                    GlobalState.ExitCode = -1;
                    _logger.ConsoleLog(consoleLogMessages.ToString());
                }

                _logger.Log(logMessages.ToString());
            }
            catch (Exception ex)
            {
                _logger.Log($"Error processing file {filePath}: {ex.Message}");
                _logger.ConsoleLog($"Error processing file {filePath}: {ex.Message}");
                GlobalState.ExitCode = -1; // Set exit code to indicate failure
            }
        }

        //public void ProcessFiles(List<(string filePath, int? headerRow, CheckType checkType, string checkDetails, bool fileNameContainsFlag,string splitParameter)> fileConfigs)
        //{
        //    foreach (var config in fileConfigs)
        //    {
        //        if (config.fileNameContainsFlag)
        //        {
        //            var directory = Path.GetDirectoryName(config.filePath);
        //            var fileNamePattern = Path.GetFileName(config.filePath);
        //            var matchingFiles = Directory.GetFiles(directory, $"*{fileNamePattern}*");

        //            Parallel.ForEach(matchingFiles, filePath =>
        //            {
        //                ProcessFile(filePath, config.headerRow, config.checkType, config.checkDetails,config.splitParameter);
        //            });
        //        }
        //        else
        //        {
        //            ProcessFile(config.filePath, config.headerRow, config.checkType, config.checkDetails,config.splitParameter);
        //        }
        //    }
        //}
        public void ProcessFiles(List<(string filePath, int? headerRow, CheckType checkType, string checkDetails, bool fileNameContainsFlag, string splitParameter)> fileConfigs, ClientPartialFileNamesConfig partialFileNamesConfig)
        {
            try
            {
                var filesToProcess = new List<(string filePath, int? headerRow, CheckType checkType, string checkDetails, string splitParameter)>();

                foreach (var config in fileConfigs)
                {
                    if (config.fileNameContainsFlag)
                    {
                        var directory = Path.GetDirectoryName(config.filePath);
                        var fileNamePattern = Path.GetFileName(config.filePath);
                        var matchingFiles = Directory.GetFiles(directory, $"*{fileNamePattern}*");

                        foreach (var filePath in matchingFiles)
                        {
                            filesToProcess.Add((filePath, config.headerRow, config.checkType, config.checkDetails, config.splitParameter));
                        }
                    }
                    else
                    {
                        filesToProcess.Add((config.filePath, config.headerRow, config.checkType, config.checkDetails, config.splitParameter));
                    }
                }

                Parallel.ForEach(filesToProcess, file =>
                {
                    ProcessFile(file.filePath, file.headerRow, file.checkType, file.checkDetails, file.splitParameter, partialFileNamesConfig);
                });
            }
            catch (Exception ex)
            {
                _logger.Log($"Error processing files: {ex.Message}");
                _logger.ConsoleLog($"Error processing files: {ex.Message}");
                GlobalState.ExitCode = -1; // Set exit code to indicate failure
            }
        }
    }

    public class Logger
    {
        private readonly object _lock = new object();
        private readonly object _consoleLock = new object();

        private StreamWriter _logWriter;
        private readonly string _logDirectory;

        // public Logger(string arg)
        public Logger(string arg)
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");

            //  _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs", currentDate);
            _logDirectory = arg;
            //  Console.WriteLine(_logDirectory);

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            var logFilePath = Path.Combine(_logDirectory, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            _logWriter = new StreamWriter(logFilePath) { AutoFlush = true };
        }

        public bool IsFirstLogOfTheDay()
        {
            var logFiles = Directory.GetFiles(_logDirectory, "log_*.txt");
            return logFiles.Length == 1;
        }

        public void Log(string message)
        {
            lock (_lock)
            {
                _logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }

        public void ConsoleLog(string message)
        {
            lock (_consoleLock)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = originalColor; // Reset to the original color
            }
        }

        public void Close()
        {
            lock (_lock)
            {
                _logWriter.Close();
            }
        }
    }

    public class CsvFileReader
    {
        private readonly object _consoleLock = new object();

        public (List<Dictionary<string, string>>, int?, string, string[]) ReadCsvFile(string filePath, int? headerRow)
        {
            var logMessages = new System.Text.StringBuilder();
            var records = new List<Dictionary<string, string>>();
            int? headerCount = 0;
            string[] headers = null;
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false, // We will handle the header manually
                    TrimOptions = TrimOptions.Trim,
                    IgnoreBlankLines = true,
                    BadDataFound=null
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    if (headerRow.HasValue)
                    {
                        for (int i = 0; i < headerRow.Value; i++)
                        {
                            csv.Read();

                            if (i == headerRow.Value - 1)
                            {
                                headers = csv.Context.Parser.Record;
                                headerCount = headers?.Length; // Count the number of headers
                            }
                        }
                    }

                    while (csv.Read())
                    {
                        var recordDict = new Dictionary<string, string>();

                        for (int i = 0; i < csv.Parser.Record.Length; i++)
                        {
                            string header = (headers != null && i < headers.Length && !string.IsNullOrWhiteSpace(headers[i])) ? headers[i] : $"Column{i + 1}";
                            recordDict[header] = csv.GetField(i);
                        }

                        records.Add(recordDict);
                    }

                    logMessages.AppendLine($"Read {records.Count} records from {filePath}");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error: {ex.Message}";
                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }
            return (records, headerCount, logMessages.ToString(), headers);
        }

        public string ReadHeaderRowForCSV(string filePath, int rowIndex)
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(filePath);

            // Ensure rowIndex is valid

            if (rowIndex >= 1 && rowIndex <= lines.Length)
            {
                // Get the specific row (header row) and remove extra spaces and normalize line endings
                return lines[rowIndex - 1].Trim().Replace("\r\n", "\n");
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Header Row index is out of range.");
            }
        }
    }

    public class ExcelFileReader
    {
        private readonly object _consoleLock = new object();

        public (List<Dictionary<string, string>>, int?, string, string[]) ReadExcelFile(string filePath, int? headerRow)
        {
            var logMessages = new System.Text.StringBuilder();
            var records = new List<Dictionary<string, string>>();
            int? headerCount = 0;
            string[] headers = null;
            try
            {
                // Read the file into memory
                byte[] fileBytes = File.ReadAllBytes(filePath);

                if (filePath.ToLower().EndsWith(".xlsx"))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var stream = new MemoryStream(fileBytes))
                    using (var package = new ExcelPackage(stream))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets.First();

                        var rowCount = worksheet.Dimension.Rows;
                        var colCount = worksheet.Dimension.Columns;

                        headers = new string[colCount];
                        int actualHeaderRow = headerRow ?? 1;

                        for (int col = 1; col <= colCount; col++)
                        {
                            headers[col - 1] = worksheet.Cells[actualHeaderRow, col].Text;
                        }

                        headerCount = headers.Length;

                        for (int row = actualHeaderRow + 1; row <= rowCount; row++)
                        {
                            var record = new Dictionary<string, string>();
                            bool isRowEmpty = true;

                            for (int col = 1; col <= colCount; col++)
                            {
                                var cellValue = worksheet.Cells[row, col].Text;

                                if (!string.IsNullOrWhiteSpace(cellValue))
                                {
                                    isRowEmpty = false;
                                }

                                string header = (headers != null && col <= headers.Length && !string.IsNullOrWhiteSpace(headers[col - 1])) ? headers[col-1] : $"Column{col}";
                                record[header] = cellValue;
                            }

                            // Add the record if not empty 
                            if (!isRowEmpty)
                            {
                                records.Add(record);
                            }
                        }

                        logMessages.AppendLine($"Read {records.Count} records from {filePath}");
                    }
                }
                else if (filePath.ToLower().EndsWith(".xls"))
                {
                    try
                    {
                        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                        using (var stream = new MemoryStream(fileBytes))
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            var table = result.Tables[0];

                            int actualHeaderRow = headerRow ?? 1;
                            headerCount = table.Columns.Count;
                            headers = new string[headerCount.Value]; // Todo: Null handling

                            for (int col = 0; col < headerCount; col++)
                            {
                                headers[col] = table.Rows[actualHeaderRow - 1][col].ToString();
                            }

                            for (int row = actualHeaderRow; row < table.Rows.Count; row++)
                            {
                                var record = new Dictionary<string, string>();
                                bool isRowEmpty = true;

                                for (int col = 0; col < headerCount; col++)
                                {
                                    var cellValue = table.Rows[row][col].ToString();
                                    if (!string.IsNullOrWhiteSpace(cellValue))
                                    {
                                        isRowEmpty = false;
                                    }


                                    string header = (headers != null && col < headers.Length && !string.IsNullOrWhiteSpace(headers[col])) ? headers[col] : $"Column{col}";
                                    record[header] = cellValue;
                                }

                                // Add the record if not empty or if ignoreEmptyRows is false
                                if (!isRowEmpty)
                                {
                                    records.Add(record);
                                }
                            }

                            logMessages.AppendLine($"Read {records.Count} records from {filePath}");
                        }
                    }
                    catch (Exception)
                    {
                        string logMessagesStr = string.Empty;
                        (records, headerCount, logMessagesStr, headers) = new CsvFileReader().ReadCsvFile(filePath, headerRow);
                        logMessages.Append(logMessagesStr);
                    }
                }
                else
                {
                    logMessages.AppendLine($"File format not supported: {filePath}");
                    lock (_consoleLock)
                    {
                        var originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"File format not supported: {filePath}");
                        Console.ForegroundColor = originalColor; // Reset to the original color
                    }
                    GlobalState.ExitCode = -1;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error: {ex.Message}";
                logMessages.AppendLine(errorMessage);
                // Safely print the error to the console within the console lock
                lock (_consoleLock)
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                GlobalState.ExitCode = -1;
            }

            return (records, headerCount, logMessages.ToString(), headers);
        }

        public string ReadHeaderRowForXLSX(string filePath, int rowIndex)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                // Validate row index
                if (rowIndex < 1 || rowIndex > worksheet.Dimension.End.Row)
                    throw new ArgumentOutOfRangeException(nameof(rowIndex), "Header Row index is out of range.");

                // Concatenate row values into a single string
                return string.Join(",", worksheet.Cells[rowIndex, 1, rowIndex, worksheet.Dimension.End.Column].Select(cell => cell.Text));
            }
        }

        public string ReadHeaderRowForXLS(string filePath, int rowIndex)
        {
            try
            {
                // Read the file into memory
                byte[] fileBytes = File.ReadAllBytes(filePath);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var stream = new MemoryStream(fileBytes))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];

                    // Validate row index
                    if (rowIndex < 1 || rowIndex > table.Rows.Count)
                        throw new ArgumentOutOfRangeException(nameof(rowIndex), "Header Row index is out of range.");

                    return string.Join(",", table.Rows[rowIndex - 1].ItemArray);
                }
            }
            catch
            {
                return new CsvFileReader().ReadHeaderRowForCSV(filePath, rowIndex);
            }
        }
    }

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
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to parse FilterValue as DateTime.");
                    Console.ForegroundColor = originalColor; // Reset to the original color
                }
                else
                {
                    var originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FilterValue node not found in XML.");
                    Console.ForegroundColor = originalColor; // Reset to the original color
                    GlobalState.ExitCode = -1;
                }
            }
            catch (Exception ex)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error extracting FilterValue: " + ex.Message);
                Console.ForegroundColor = originalColor; // Reset to the original color
                GlobalState.ExitCode = -1;
            }
            return DateTime.MinValue;
        }
    }

    internal class Program
    {
        public static DateTime processDate = GetProcessDate(Directory.GetCurrentDirectory() + @"\Config\ProcessDateXmlPath.txt");

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
                        var originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Failed to extract FilterValue.");
                        Console.ForegroundColor = originalColor; // Reset to the original color
                        GlobalState.ExitCode = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message.ToString());
                Console.ForegroundColor = originalColor; // Reset to the original color
                GlobalState.ExitCode = -1;
            }
            return DateTime.MinValue;
        }

        public static string GetFormattedDateTimeCurrentDate(string format)
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = "";
            try
            {
                formattedDate = currentDate.ToString(format);
                // Add the dateFactor to the currentDate object
                //DateTime modifiedDate = currentDate.AddDays(dateFactor);

                // Convert the modified date to string with the required format
            }
            catch (Exception ex)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message.ToString());
                Console.ForegroundColor = originalColor; // Reset to the original color
                GlobalState.ExitCode = -1;
            }

            return formattedDate;
        }

        public static string ReplaceDateTimePlaceholders(string input)
        {
            //string dateFactors = "";
            int i = 0;
            string pattern = @"\(([^)]+)\)";
            string pattern1 = @"\{([^}]+)\}";
            Regex regex = new Regex(pattern);
            Regex regex1 = new Regex(pattern1);
            // Find the index of the opening and closing curly braces
            //int startIndex = dateFactors.IndexOf('{');
            //int endIndex = dateFactors.IndexOf('}');
            //string[] factorsArray;
            //int[] factors = new int[100];
            // Check if both opening and closing curly braces exist
            try
            {
                //if (startIndex != -1 && endIndex != -1)
                //{
                //    // Extract the substring within the curly braces
                //    string factorsSubstring = dateFactors.Substring(startIndex + 1, endIndex - startIndex - 1);

                //    // Split the substring by commas
                //    factorsArray = factorsSubstring.Split(';');

                //    // Convert the string array to an integer array
                //    factors = factorsArray.Select(int.Parse).ToArray();

                //}
                //else
                //{
                //}

                MatchCollection matches = regex.Matches(input);

                foreach (Match match in matches)
                {
                    //int dateFactor = 0;
                    string placeholder = match.Groups[1].Value;

                    //if (factors.Length > i)
                    //{
                    //    dateFactor = factors[i];
                    //}
                    //else
                    //{
                    //    dateFactor = 0;
                    //}

                    string replacement = GetFormattedDateTime(placeholder);
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
                    //int dateFactor = 0;
                    string placeholder = match.Groups[1].Value;

                    //if (factors.Length > i)
                    //{
                    //    dateFactor = factors[i];
                    //}
                    //else
                    //{
                    //    dateFactor = 0;
                    //}
                    string replacement = GetFormattedDateTimeCurrentDate(placeholder);
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
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message.ToString());
                Console.ForegroundColor = originalColor; // Reset to the original color
                GlobalState.ExitCode = -1;
            }

            return input;
        }

        public static string GetFormattedDateTime(string format)
        {
            return processDate.ToString(format);
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
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading XML path from config: {ex.Message}");
                Console.ForegroundColor = originalColor; // Reset to the original color
                GlobalState.ExitCode = -1;
                return null;
            }
        }

        private static void Main(string[] args)
        {
            // string[] arr = args[0].Split('#');
            //string currDirectory = args[0];
            //Environment.CurrentDirectory = currDirectory;
            var logger = new Logger(args[1]);
            var dataProcessor = new DataProcessor();

            // Read the appsettings.json file

            // To DO--> Fix this in future
            var json = File.ReadAllText(@"D:\Backoffice Bots Different Clients\OneClick_File Sequencing\File Processing Bots\File Validation\appsettings.json");

            // Parse the JSON into a dictionary
            var partialFileNamesConfig = JsonConvert.DeserializeObject<ClientPartialFileNamesConfig>(json);

            try
            {
                var fileProcessor = new FileProcessor(logger, dataProcessor);
                // Console.WriteLine(args[0]);

                var fileConfigs = ReadConfig(args[0]);
                // var fileConfigs = ReadConfig(@"Config\config.csv");
                fileProcessor.ProcessFiles(fileConfigs, partialFileNamesConfig);
            }
            catch (Exception ex)
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message.ToString());
                Console.ForegroundColor = originalColor; // Reset to the original color
            }
            finally
            {
                logger.Close();
                Environment.Exit(GlobalState.ExitCode);
                //  Console.ReadLine();
            }
        }

        public static List<(string filePath, int? headerRow, CheckType checkType, string checkDetails, bool fileNameContainsFlag, string splitParameter)> ReadConfig(string configFilePath)
        {
            var config = new List<(string filePath, int? headerRow, CheckType checkType, string checkDetails, bool fileNameContainsFlag, string splitParameter)>();

            using (var reader = new StreamReader(configFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true
            }))
            {
                // Ensure the header row is read
                if (csv.Read())
                {
                    csv.ReadHeader();
                }

                while (csv.Read())
                {
                    var filePath = csv.GetField<string>("FilePath");
                    // Parse the CheckType and handle it

                    var checkDetails = csv.GetField<string>("CheckDetails");
                    var fileNameContainsFlag = csv.GetField<bool>("FileNameContainsFlag");
                    filePath = ReplaceDateTimePlaceholders(filePath);
                    checkDetails = ReplaceDateTimePlaceholders(checkDetails);
                    var splitParameter = csv.GetField<string>("SplitParameter");
                    // Read headerRow if it exists; otherwise, default to null
                    int? headerRow = null;
                    var headerRowField = csv.GetField<string>("HeaderRow");
                    if (!string.IsNullOrWhiteSpace(headerRowField) && int.TryParse(headerRowField, out var headerRowValue))
                    {
                        headerRow = headerRowValue;
                    }
                    // Parse the CheckType and handle it
                    if (Enum.TryParse<CheckType>(csv.GetField<string>("CheckType"), true, out var checkType))
                    {
                        config.Add((filePath, headerRow, checkType, checkDetails, fileNameContainsFlag, splitParameter));
                    }
                    else
                    {
                        // Handle invalid CheckType if necessary
                        throw new ArgumentException($"Invalid CheckType value: {checkType}");
                    }
                }
            }

            return config;
        }
    }
}