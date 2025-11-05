using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleApplication
{
    static class XMLService
    {
        private static string _currentXMLElement = string.Empty;

        public static void ConvertXmlToCSV(string xmlFilePath)
        {
            //string csvFilePath = Path.Combine(@"C:\Users\Himanshu.Panai\Desktop\Enterprise\BackOfficeAutomation\File Processing\FTP Modifications\ConsoleApplication1", "newfile" + ".csv");

            string fname = Path.GetFileName(xmlFilePath);
            string csvFilePath = Program.outputFolder + @"\" + fname + ".csv";

            try
            {
                // Load the XML document
                XDocument xmlDocument = XDocument.Load(xmlFilePath);

                // Generate CSV from the XML document
                string csvData = ConvertXmlDocumentToCsvData(xmlDocument);

                // Write the CSV data to the output file
                File.WriteAllText(csvFilePath, csvData);

                Console.WriteLine("XML successfully converted to CSV!");
                Program.CreateLogs("XML successfully converted to CSV!", Program.logFilePath);
            }
            catch (Exception ex)
            {
                Program.exitcode = 3;
                Program.LogWithRed(ex.Message.ToString());
            }
        }

        private static string ConvertXmlDocumentToCsvData(XDocument xmlDocument)
        {
            // List to store the column headers and rows
            var columnHeaders = new HashSet<string>();
            var rows = new List<Dictionary<string, string>>();

            // Recursively extract data
            var rootName = xmlDocument.Root.Elements().FirstOrDefault().Name;
            int i = 0;
            int j = 0;

            foreach (var record in xmlDocument.Descendants(rootName))
            {
                var rowData = new Dictionary<string, string>();
                ExtractData(record, "", rowData, ref i, ref j);
                rows.Add(rowData);

                // Add keys (headers) to the columnHeaders set
                foreach (var key in rowData.Keys)
                {
                    columnHeaders.Add(key);
                }
            }

            // Generate the CSV
            var headerRow = string.Join(",", columnHeaders);
            var csvRows = new List<string> { headerRow };

            foreach (var row in rows)
            {
                var rowValues = columnHeaders.Select(header =>
                {
                    // If the row contains this header, return its value; otherwise, return an empty string
                    return row.ContainsKey(header) ? string.Join(";", row[header]) : "";
                });

                // Add the row values as a CSV line
                csvRows.Add(string.Join(",", rowValues));
            }


            return string.Join(Environment.NewLine, csvRows);
        }

        private static void ExtractData(XElement element, string parentPath, Dictionary<string, string> rowData, ref int i, ref int j)
        {
            foreach (var child in element.Elements())
            {
                string currentPath = string.Empty;

                currentPath = string.IsNullOrEmpty(parentPath) ? child.Name.LocalName : $"{parentPath}/{child.Name.LocalName}";

                if (child.HasAttributes)
                {
                    AddAttributes(child, currentPath, rowData, ref i);
                    currentPath = $"{currentPath}/{i}/__text";
                }

                if (child.HasElements)
                {
                    if (_currentXMLElement != child.Name.LocalName)
                    {
                        _currentXMLElement = child.Name.LocalName;
                        j = 0;
                    }
                    i = 0;
                    currentPath = string.IsNullOrEmpty(parentPath) ? child.Name.LocalName : $"{parentPath}/{child.Name.LocalName}/{j}";

                    // Recurse into child elements
                    ExtractData(child, currentPath, rowData, ref i, ref j);
                    j++;
                }
                else
                {
                    // Add leaf node value
                    if (rowData.TryGetValue(currentPath, out var value))
                    {
                    }
                    else
                    {
                        rowData[currentPath] = child.Value;
                    }

                    i++;
                }
            }
        }

        private static void AddAttributes(XElement element, string parentPath, Dictionary<string, string> rowData, ref int i)
        {
            foreach (var child in element.Attributes())
            {
                var currentPath = string.IsNullOrEmpty(parentPath) ? child.Name.LocalName : $"{parentPath}/{i}/_{child.Name.LocalName}";

                {
                    rowData[currentPath] = child.Value;
                }
            }
        }
    }
}