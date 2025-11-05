using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;
using System.IO;
using System.Globalization;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class VerifyRowAllFixLogs : CameronSimulator, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {

                bool allVerified = true;

                // Read log lines once
                string logPath = ConfigurationManager.AppSettings["AllFixLog"];
                if (!File.Exists(logPath))
                    throw new FileNotFoundException("AllFixLog file not found at: " + logPath);

                string tempFilePath = Path.Combine(Path.GetDirectoryName(logPath), "AllFixLogs_backup.log");
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                File.Copy(logPath, tempFilePath);

                List<string> logLines = File.ReadLines(tempFilePath).ToList();

                HashSet<int> usedLogLines = new HashSet<int>();
                List<string> errorRowDetails = new List<string>();
                foreach (DataRow row in testData.Tables[0].Rows)
                {
                    bool verified = VerifyRowAgainstFixLog(row, logLines, usedLogLines);
                    if (!verified)
                    {
                        string rowDetails = string.Join(" | ", row.Table.Columns
                                                .Cast<DataColumn>()
                                                .Select(col => col.ColumnName + "=" + row[col]));

                        Console.WriteLine("Row verification failed.");
                        Console.WriteLine("Failed Row Details: " + rowDetails);

                        errorRowDetails.Add(rowDetails);
                        allVerified = false;
                    }
                }

                if (!allVerified)
                {
                    string errorSummary = string.Join(Environment.NewLine, errorRowDetails);
                    throw new Exception("One or more rows failed FIX tag verification." + Environment.NewLine + errorSummary);
                }

                return _result;
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }

        private bool VerifyRowAgainstFixLog(DataRow dataRow, List<string> logLines, HashSet<int> usedLogLines)
        {
            string tagListFromConfig = ConfigurationManager.AppSettings["TagsToSkip"];
            if (string.IsNullOrWhiteSpace(tagListFromConfig))
                throw new Exception("TagsToSkip is missing or empty in App.config");
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                tagListFromConfig = tagListFromConfig + "," + ConfigurationManager.AppSettings["TagsToSkipInSamsara"];

            List<string> tagsToSkip = tagListFromConfig
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => tag.Trim())
                .ToList();

            if (!dataRow.Table.Columns.Contains("RowLog"))
                throw new Exception("Missing 'RowLog' column in test data.");

            string fixString = dataRow["RowLog"] != null ? dataRow["RowLog"].ToString().Trim() : string.Empty;
            if (string.IsNullOrWhiteSpace(fixString))
                throw new Exception("Empty FIX string in RowLog column.");

            fixString = fixString.Trim('(', ')');
            string[] pairs = fixString.Split(new char[] { '\x01' }, StringSplitOptions.RemoveEmptyEntries);

            if (pairs.Length < 2)
            {
                string[] delimitersToTry = new string[]
                    {
                        "_x0001_",
                        "|",
                        "^A",
                        "\\x01",
                        "&#1;"
                    };

                foreach (string delimiter in delimitersToTry)
                {
                    pairs = fixString.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                    if (pairs.Length >= 2)
                    {
                        Console.WriteLine("Used fallback delimiter: " + delimiter);
                        break;
                    }
                }
            }

            Dictionary<string, string> tagValuePairs = new Dictionary<string, string>();
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('=');
                if (parts.Length == 2 && !tagsToSkip.Contains(parts[0]) && !tagValuePairs.ContainsKey(parts[0]))
                {
                    tagValuePairs[parts[0]] = parts[1];
                }
            }

            if (tagValuePairs.Count == 0)
            {
                Console.WriteLine("No tag-value pairs to verify for this row.");
                return false;
            }

            for (int i = 0; i < logLines.Count; i++)
            {
                if (usedLogLines.Contains(i))
                {
                    Console.WriteLine("Tag logs already verified in line index: " + i);
                    continue;
                }

                string line = logLines[i];
                bool allMatched = true;

                foreach (KeyValuePair<string, string> pair in tagValuePairs)
                {
                    if (pair.Key.Equals("126")) 
                    {
                        //Expire Time verification
                        char soh = '\x01';

                        // Split the message
                        string[] fields = line.Split(soh);

                        string tag126Value = null;

                        // Find the 126 tag
                        foreach (string field in fields)
                        {
                            if (field.StartsWith("126="))
                            {
                                tag126Value = field.Substring(4); // remove "126="
                                break;
                            }
                        }

                        // Verify if it ends with -21:00:00
                        bool endsWith21 = false;
                        if (tag126Value != null)
                        {
                            endsWith21 = tag126Value.EndsWith("-21:00:00");
                        }

                        if (!endsWith21)
                            throw new Exception("Tag 126 ExpireTime verification Failed " + tag126Value);
                        continue;

                    }
                    string search = pair.Key + "=" + pair.Value;
                    if (!line.Contains(search))
                    {
                        allMatched = false;
                        break;
                    }
                    else
                    {

                        Console.WriteLine("Matched tag: '" + search + "' found in line: '" + line + "'");
                        try
                        {
                            int index = line.IndexOf(search);

                            char prevChar = index > 0 ? line[index - 1] : '-';
                            char nextChar = index + search.Length < line.Length ? line[index + search.Length] : '-';

                            bool isPrevAlphaNum = char.IsLetterOrDigit(prevChar);
                            bool isNextAlphaNum = char.IsLetterOrDigit(nextChar);

                            Console.WriteLine("Matched tag: '" + search + "' found in line: '" + line + "'");
                            Console.WriteLine("Previous char: '" + (char.IsControl(prevChar) ? "\\x" + ((int)prevChar).ToString("X2") : prevChar.ToString()) +
                                              "', Next char: '" + (char.IsControl(nextChar) ? "\\x" + ((int)nextChar).ToString("X2") : nextChar.ToString()) + "'");
                            if (isPrevAlphaNum || isNextAlphaNum)
                            {
                                throw new Exception("Invalid match for tag '" + search + "' in line. May be a partial match. " +
                                                    "Prev: '" + prevChar + "', Next: '" + nextChar + "'");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                    }
                }

                if (allMatched)
                {
                    usedLogLines.Add(i);
                    Console.WriteLine("Row verified on line: " + i);
                    return true;
                }
            }

            Console.WriteLine("No unique log line found for this row's tags.");
            return false;
        }
    }
}

